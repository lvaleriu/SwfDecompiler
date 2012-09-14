/*
	This sample is an open source library for reading 
	Flash components (SWC files).
	Copyright (C) 2005 Olivier Carpentier - Adelina foundation
	see Licence.cs for GPL full text!
		
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	General Public License for more details.
	
	You should have received a copy of the GNU General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SwfDotNet.IO;
using SwfDotNet.IO.ByteCode;
using SwfDotNet.IO.ByteCode.Actions;
using SwfDotNet.IO.Tags;
using log4net;

namespace SwfDecompiler
{
    public partial class Form1 : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Form1));

        public Form1()
        {
            InitializeComponent();

            string currentPath = Directory.GetCurrentDirectory();
            if (currentPath.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                currentPath += Path.DirectorySeparatorChar;
            string output = currentPath + "output\\";
            string input = currentPath + "input\\sample.swf";
            input =
                @"\\frmitch-fs04\Quali_Appli\data\interDataProd\13-04-2012\Germany\BANNIERES\ALLEMAGNE INTERNET VALERIU\3942029__.FLV";
            textBoxDir.Text = output;
            textBoxSwf.Text = input;
        }

        private void buttonBrowseSwf_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (File.Exists(openFileDialog1.FileName))
                    textBoxSwf.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseDir_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialog1.SelectedPath))
                    textBoxDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void TestPath(string fileName, string outputDir)
        {
            if (File.Exists(fileName) == false)
            {
                MessageBox.Show("Input swf file doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Directory.Exists(outputDir) == false)
            {
                MessageBox.Show("Output directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            int numImg = 0;
            int numSound = 0;
            int numActionScript = 0;

            string fileName = textBoxSwf.Text;
            string outputDir = textBoxDir.Text;
            TestPath(fileName, outputDir);

            if (log.IsDebugEnabled)
                log.Debug("**************** Start to decompile file " + fileName);

            listViewPix.Items.Clear();
            listViewSounds.Items.Clear();
            listViewActionScript.Items.Clear();

            SwfReader swfReader = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //var flvReader = new FlvReader(fileName);
                //var flv = flvReader.ReadFlv();                

                swfReader = new SwfReader(fileName); // Create a swf stream reader
                Swf swf = swfReader.ReadSwf(); // Read the completed swf file

                //Read headers infos
                labelSwfVersion.Text = swf.Version.ToString();
                labelSwfDim.Text = swf.Header.Width + "x" + swf.Header.Height;
                labelSwfFps.Text = swf.Header.Fps.ToString();
                labelSwfFrames.Text = swf.Header.Frames.ToString();
                labelSwfSize.Text = swf.Header.FileSize.ToString();
                labelSwfSign.Text = swf.Header.Signature;

                //Read tags info
                IEnumerator tagsEnu = swf.Tags.GetEnumerator(); //Browse swf tags list
                while (tagsEnu.MoveNext())
                {
                    var tag = (BaseTag) tagsEnu.Current;
                    if (tag is SetBackgroundColorTag)
                    {
                        Color bgColor = ((SetBackgroundColorTag) tag).RGB.ToWinColor();
                        labelSwfBgColor.Text = "R:" + bgColor.R + " G:" + bgColor.G + " B:" + bgColor.B;
                    }
                    else if (tag is DefineBitsJpeg2Tag)
                    {
                        numImg++;
                        string outfileName = outputDir + GetRandomName() + ".jpg";
                        ((DefineBitsJpeg2Tag) tag).DecompileToFile(outfileName);

                        string shortName = Path.GetFileName(outfileName);
                        var listViewItem1 = new ListViewItem(new[] {shortName, "jpg"}, -1);
                        listViewPix.Items.Add(listViewItem1);
                    }
                    else if (tag is DefineSoundTag) //Extract a sound file:
                    {
                        numSound++;
                        string outfileName = outputDir + GetRandomName();
                        var soundTag = (DefineSoundTag) tag;
                        if (soundTag.SoundFormat == SoundCodec.MP3)
                            outfileName += ".mp3";
                        else
                            outfileName += ".wav";
                        soundTag.DecompileToFile(outfileName);

                        string shortName = Path.GetFileName(outfileName);
                        var listViewItem1 = new ListViewItem(new[] {shortName, ""}, -1);
                        listViewSounds.Items.Add(listViewItem1);
                    }

                    //If tag contains action script...
                    if (tag.ActionRecCount != 0)
                    {
                        var sb = new StringBuilder();
                        IEnumerator enum2 = tag.GetEnumerator();
                        while (enum2.MoveNext())
                        {
                            var dc = new Decompiler(swf.Version);
                            ArrayList actions = dc.Decompile((byte[]) enum2.Current);
                            foreach (BaseAction obj in actions)
                            {
                                sb.AppendLine(obj.ToString());
                            }
                        }
                        string outfileName = outputDir + GetRandomName() + ".as";

                        var writer = new StreamWriter(outfileName);
                        writer.Write(sb.ToString());
                        writer.Close();
                        numActionScript++;

                        string shortName = Path.GetFileName(outfileName);
                        var listViewItem1 = new ListViewItem(new[] {shortName, ""}, -1);
                        listViewActionScript.Items.Add(listViewItem1);
                    }
                }

                Cursor.Current = Cursors.Default;
                string mssg = "Swf decompiler extracts:\n";
                mssg += numImg + " pictures files\n";
                mssg += numSound + " sound files\n";
                mssg += numActionScript + " action script blocks\n";

                MessageBox.Show(mssg, "Decompilation finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (swfReader != null)
                    swfReader.Close(); // Closing stream reader
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetRandomName()
        {
            return RandomPassword.Generate(8, 10);
        }
    }
}