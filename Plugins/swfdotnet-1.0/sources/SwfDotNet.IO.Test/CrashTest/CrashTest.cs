using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using log4net;

using SwfDotNet.IO;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.ByteCode;
using SwfDotNet.IO.ByteCode.Actions;

namespace SwfDotNet.IO.Test.Crash
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxPath;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.OpenFileDialog openFileDialogSWF;
		private System.Windows.Forms.Button buttonParse;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelFps;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelFileSize;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label labelSignature;
		private System.Windows.Forms.Label labelFrameC;
		private System.Windows.Forms.Label labelFrameCnt;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelASbytes;
		private System.Windows.Forms.ListBox listBoxActions;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelTagsCnt;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button buttonBrowseDir;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.TextBox textBoxDir;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label Result;
		private System.Windows.Forms.Label Time;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label labelCurrentFile;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.Button buttonBrowseOutput;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label WriteResult;
        private System.Windows.Forms.ColumnHeader columnHeader3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));
	
        /// <summary>
        /// Creates a new <see cref="Form1"/> instance.
        /// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.openFileDialogSWF = new System.Windows.Forms.OpenFileDialog();
            this.buttonParse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelASbytes = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelFrameCnt = new System.Windows.Forms.Label();
            this.labelFrameC = new System.Windows.Forms.Label();
            this.labelSignature = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFileSize = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFps = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxActions = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.labelTagsCnt = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.buttonBrowseDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.Result = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonBrowseOutput = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label15 = new System.Windows.Forms.Label();
            this.WriteResult = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "SWF File:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(72, 40);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(240, 20);
            this.textBoxPath.TabIndex = 1;
            this.textBoxPath.Text = "";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(320, 40);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(64, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // openFileDialogSWF
            // 
            this.openFileDialogSWF.Filter = "Swf files (*.swf)|*.swf";
            // 
            // buttonParse
            // 
            this.buttonParse.Location = new System.Drawing.Point(8, 144);
            this.buttonParse.Name = "buttonParse";
            this.buttonParse.Size = new System.Drawing.Size(80, 24);
            this.buttonParse.TabIndex = 3;
            this.buttonParse.Text = "Launch test";
            this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelASbytes);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.labelFrameCnt);
            this.groupBox1.Controls.Add(this.labelFrameC);
            this.groupBox1.Controls.Add(this.labelSignature);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelHeight);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labelWidth);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelFileSize);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelFps);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelVersion);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(456, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 120);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SWF Header info";
            // 
            // labelASbytes
            // 
            this.labelASbytes.Location = new System.Drawing.Point(136, 96);
            this.labelASbytes.Name = "labelASbytes";
            this.labelASbytes.Size = new System.Drawing.Size(72, 16);
            this.labelASbytes.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label8.Location = new System.Drawing.Point(16, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Actions blocks count:";
            // 
            // labelFrameCnt
            // 
            this.labelFrameCnt.Location = new System.Drawing.Point(208, 48);
            this.labelFrameCnt.Name = "labelFrameCnt";
            this.labelFrameCnt.Size = new System.Drawing.Size(32, 16);
            this.labelFrameCnt.TabIndex = 13;
            // 
            // labelFrameC
            // 
            this.labelFrameC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.labelFrameC.Location = new System.Drawing.Point(128, 48);
            this.labelFrameC.Name = "labelFrameC";
            this.labelFrameC.Size = new System.Drawing.Size(80, 16);
            this.labelFrameC.TabIndex = 12;
            this.labelFrameC.Text = "Frame count:";
            // 
            // labelSignature
            // 
            this.labelSignature.Location = new System.Drawing.Point(88, 48);
            this.labelSignature.Name = "labelSignature";
            this.labelSignature.Size = new System.Drawing.Size(40, 16);
            this.labelSignature.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 10;
            this.label7.Text = "Signature:";
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(184, 72);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(72, 16);
            this.labelHeight.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label6.Location = new System.Drawing.Point(128, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Height:";
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(72, 72);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(56, 16);
            this.labelWidth.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Width:";
            // 
            // labelFileSize
            // 
            this.labelFileSize.Location = new System.Drawing.Point(264, 24);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(48, 16);
            this.labelFileSize.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label4.Location = new System.Drawing.Point(200, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "File size:";
            // 
            // labelFps
            // 
            this.labelFps.Location = new System.Drawing.Point(144, 24);
            this.labelFps.Name = "labelFps";
            this.labelFps.Size = new System.Drawing.Size(32, 16);
            this.labelFps.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.Location = new System.Drawing.Point(104, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fps:";
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(72, 24);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(24, 16);
            this.labelVersion.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Version:";
            // 
            // listBoxActions
            // 
            this.listBoxActions.Location = new System.Drawing.Point(464, 216);
            this.listBoxActions.Name = "listBoxActions";
            this.listBoxActions.Size = new System.Drawing.Size(320, 251);
            this.listBoxActions.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(464, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 6;
            this.label9.Text = "Tags count:";
            // 
            // labelTagsCnt
            // 
            this.labelTagsCnt.Location = new System.Drawing.Point(528, 176);
            this.labelTagsCnt.Name = "labelTagsCnt";
            this.labelTagsCnt.Size = new System.Drawing.Size(40, 16);
            this.labelTagsCnt.TabIndex = 7;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                        this.columnHeader1,
                                                                                        this.columnHeader2,
                                                                                        this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(8, 184);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(432, 304);
            this.listView1.TabIndex = 8;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File parsed";
            this.columnHeader1.Width = 270;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Crash test";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Duration";
            this.columnHeader3.Width = 70;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(16, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Test dir:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxDir
            // 
            this.textBoxDir.Location = new System.Drawing.Point(72, 16);
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.Size = new System.Drawing.Size(240, 20);
            this.textBoxDir.TabIndex = 10;
            this.textBoxDir.Text = "";
            // 
            // buttonBrowseDir
            // 
            this.buttonBrowseDir.Location = new System.Drawing.Point(320, 16);
            this.buttonBrowseDir.Name = "buttonBrowseDir";
            this.buttonBrowseDir.Size = new System.Drawing.Size(64, 23);
            this.buttonBrowseDir.TabIndex = 11;
            this.buttonBrowseDir.Text = "Browse...";
            this.buttonBrowseDir.Click += new System.EventHandler(this.buttonBrowseDir_Click_1);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(160, 136);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 16);
            this.label11.TabIndex = 17;
            this.label11.Text = "Reading result:";
            // 
            // Result
            // 
            this.Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.Result.ForeColor = System.Drawing.Color.Red;
            this.Result.Location = new System.Drawing.Point(264, 136);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(72, 16);
            this.Result.TabIndex = 18;
            // 
            // Time
            // 
            this.Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.Time.ForeColor = System.Drawing.Color.Red;
            this.Time.Location = new System.Drawing.Point(344, 136);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(88, 16);
            this.Time.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(408, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 16);
            this.label12.TabIndex = 20;
            this.label12.Text = "Current file: ";
            // 
            // labelCurrentFile
            // 
            this.labelCurrentFile.Location = new System.Drawing.Point(464, 16);
            this.labelCurrentFile.Name = "labelCurrentFile";
            this.labelCurrentFile.Size = new System.Drawing.Size(320, 16);
            this.labelCurrentFile.TabIndex = 21;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(464, 200);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 16);
            this.label13.TabIndex = 22;
            this.label13.Text = "Tags List:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxPath);
            this.groupBox2.Controls.Add(this.buttonBrowse);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBoxDir);
            this.groupBox2.Controls.Add(this.buttonBrowseDir);
            this.groupBox2.Location = new System.Drawing.Point(8, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 72);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input files";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonBrowseOutput);
            this.groupBox3.Controls.Add(this.textBoxOutput);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Location = new System.Drawing.Point(8, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(392, 48);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output files";
            // 
            // buttonBrowseOutput
            // 
            this.buttonBrowseOutput.Location = new System.Drawing.Point(320, 16);
            this.buttonBrowseOutput.Name = "buttonBrowseOutput";
            this.buttonBrowseOutput.Size = new System.Drawing.Size(64, 23);
            this.buttonBrowseOutput.TabIndex = 2;
            this.buttonBrowseOutput.Text = "Browse...";
            this.buttonBrowseOutput.Click += new System.EventHandler(this.buttonBrowseOutput_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(72, 16);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(240, 20);
            this.textBoxOutput.TabIndex = 1;
            this.textBoxOutput.Text = "";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 16);
            this.label14.TabIndex = 0;
            this.label14.Text = "Output dir:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(160, 160);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 16);
            this.label15.TabIndex = 25;
            this.label15.Text = "Writing result:";
            // 
            // WriteResult
            // 
            this.WriteResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.WriteResult.ForeColor = System.Drawing.Color.Red;
            this.WriteResult.Location = new System.Drawing.Point(256, 160);
            this.WriteResult.Name = "WriteResult";
            this.WriteResult.Size = new System.Drawing.Size(80, 16);
            this.WriteResult.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 493);
            this.Controls.Add(this.WriteResult);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.labelCurrentFile);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.labelTagsCnt);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.listBoxActions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonParse);
            this.Name = "Form1";
            this.Text = "SwfDotNet Decompiler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void buttonBrowse_Click(object sender, System.EventArgs e)
		{
			DialogResult res = this.openFileDialogSWF.ShowDialog(this);
			if (res == DialogResult.OK)
				this.textBoxPath.Text = this.openFileDialogSWF.FileName;
		}

		private void buttonBrowseDir_Click_1(object sender, System.EventArgs e)
		{
			if (this.textBoxDir.Text != string.Empty)
				this.folderBrowserDialog.SelectedPath = this.textBoxDir.Text;
			DialogResult res = this.folderBrowserDialog.ShowDialog(this);
			if (res == DialogResult.OK)
			{
				this.textBoxDir.Text = this.folderBrowserDialog.SelectedPath;
			}
		}

		private void buttonParse_Click(object sender, System.EventArgs e)
		{
			if (this.textBoxDir.Text == string.Empty && this.textBoxPath.Text != string.Empty)
				TestFile(this.textBoxPath.Text);
			else
			{
				this.listView1.Items.Clear();
				totalParsed = 0;
				succeded = 0;
				succededWrite = 0;
				swfList.Clear();
				
				DateTime before = DateTime.Now;
				
				ReadDir(this.textBoxDir.Text);
				
				DateTime after = DateTime.Now;
				
                TimeSpan timeSpan = new TimeSpan(after.Ticks - before.Ticks);
                string min = timeSpan.Minutes.ToString();
                if (min.Length == 1)
                    min = "0" + min; 
                string sec = timeSpan.Seconds.ToString();
                if (sec.Length == 1)
                    sec = "0" + sec;
                string msec = timeSpan.Milliseconds.ToString();
                if (msec.Length == 1)
                    msec = "0" + msec;
                string dur = min + ":" + sec + ":" + msec;
                this.Time.Text = dur;
			}
		}

		private int totalParsed = 0;
		private int succeded = 0;
		private int succededWrite = 0;
		private ArrayList swfList = new ArrayList();
		
		private void ReadDir(string dirName)
		{
			System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirName);
			System.IO.FileInfo[] files = dir.GetFiles();
			foreach (System.IO.FileInfo file in files)
			{
				if (file.Extension.ToLower() == ".swf")
					TestFile(file.FullName);
			}

			System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
			foreach (System.IO.DirectoryInfo dir2 in dirs)
				ReadDir(dir2.FullName);
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
			
		}

		private void TestFile(string file)
		{
			if (log.IsInfoEnabled)
				log.Info("--- " + file + " (READING)");

			this.Cursor = Cursors.WaitCursor;
			
			Swf swf = null;

            DateTime readStart = DateTime.Now;
            DateTime readEnd;
			try
			{
				SwfReader reader = new SwfReader(file, true);
				swf = reader.ReadSwf();
				reader.Close();

                readEnd = DateTime.Now;
                TimeSpan duration = new TimeSpan(readEnd.Ticks - readStart.Ticks);
                string min = duration.Minutes.ToString();
                if (min.Length == 1)
                    min = "0" + min; 
                string sec = duration.Seconds.ToString();
                if (sec.Length == 1)
                    sec = "0" + sec;
                string msec = duration.Milliseconds.ToString();
                if (msec.Length == 1)
                    msec = "0" + msec;
                string dur = min + ":" + sec + ":" + msec;

				System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {"READ: " + file,"OK",dur}, -1);
				listViewItem1.ForeColor = Color.Black;
				this.listView1.Items.Add(listViewItem1);
				this.listView1.Refresh();
				succeded++;
                
                swfList.Add(swf);
			}
			catch (Exception e)
			{
                readEnd = DateTime.Now;
                TimeSpan duration = new TimeSpan(readEnd.Ticks - readStart.Ticks);
                string min = duration.Minutes.ToString();
                if (min.Length == 1)
                    min = "0" + min; 
                string sec = duration.Seconds.ToString();
                if (sec.Length == 1)
                    sec = "0" + sec;
                string msec = duration.Milliseconds.ToString();
                if (msec.Length == 1)
                    msec = "0" + msec;
                string dur = min + ":" + sec + ":" + msec;

				System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {"READ: " + file,"KO",dur}, -1);
				listViewItem1.ForeColor = Color.Red;
				this.listView1.Items.Add(listViewItem1);
				this.listView1.Refresh();
				if (log.IsErrorEnabled)
					log.Error("READING KO", e);
				swfList.Add(null);
			}
            
			if (log.IsInfoEnabled)
				log.Info("--- " + file + " (WRITING)");

            readStart = DateTime.Now;
			try
			{
				string fileName = System.IO.Path.GetFileName(file);
				string path = this.textBoxOutput.Text + fileName;
				
				SwfWriter writer = new SwfWriter(path);
				writer.Write(swf);
				writer.Close();

                readEnd = DateTime.Now;
                TimeSpan duration = new TimeSpan(readEnd.Ticks - readStart.Ticks);
                string min = duration.Minutes.ToString();
                if (min.Length == 1)
                    min = "0" + min; 
                string sec = duration.Seconds.ToString();
                if (sec.Length == 1)
                    sec = "0" + sec;
                string msec = duration.Milliseconds.ToString();
                if (msec.Length == 1)
                    msec = "0" + msec;
                string dur = min + ":" + sec + ":" + msec;

				System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {"WRITE: " + path, "OK",dur}, -1);
				listViewItem1.ForeColor = Color.Black;
				this.listView1.Items.Add(listViewItem1);
				this.listView1.Refresh();

				succededWrite++;
            }
			catch (Exception ee)
			{
                readEnd = DateTime.Now;
                TimeSpan duration = new TimeSpan(readEnd.Ticks - readStart.Ticks);
                string min = duration.Minutes.ToString();
                if (min.Length == 1)
                    min = "0" + min; 
                string sec = duration.Seconds.ToString();
                if (sec.Length == 1)
                    sec = "0" + sec;
                string msec = duration.Milliseconds.ToString();
                if (msec.Length == 1)
                    msec = "0" + msec;
                string dur = min + ":" + sec + ":" + msec;

				if (log.IsErrorEnabled)
					log.Error("WRITING KO", ee);
				
                System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {"WRITE: " + file,"KO",dur}, -1);
				listViewItem1.ForeColor = Color.Red;
				this.listView1.Items.Add(listViewItem1);
				this.listView1.Refresh();
			}
			
			totalParsed++;
			this.Cursor = Cursors.Default;
			Result.Text = succeded.ToString() + "/" + totalParsed.ToString();
			Result.Refresh();
			WriteResult.Text = succededWrite.ToString() + "/" + totalParsed.ToString();
			WriteResult.Refresh();
		}

		private void ReadSwf(Swf swf)
		{
			this.labelVersion.Text = swf.Version.ToString();
			this.labelFileSize.Text = swf.Header.FileSize.ToString();
			this.labelFps.Text = swf.Header.Fps.ToString();
			this.labelWidth.Text = swf.Header.Size.Rectangle.Width.ToString();
			this.labelHeight.Text = swf.Header.Size.Rectangle.Height.ToString();
			this.labelFrameCnt.Text = swf.Header.Frames.ToString();
			this.labelSignature.Text = swf.Header.Signature;
			this.labelASbytes.Text = swf.ActionCount.ToString();
				
			this.listBoxActions.Items.Clear();
			BaseTagCollection tags = swf.Tags;
			int i = 0;
			for (; i < tags.Count; i++)
			{
				BaseTag tag = tags[i];
				int code = tag.TagCode;
				if (code != -1)
				{
					SwfDotNet.IO.Tags.TagCodeEnum val = SwfDotNet.IO.Tags.TagCodeEnum.DefineBits;
					val = (SwfDotNet.IO.Tags.TagCodeEnum)System.Enum.Parse(val.GetType(), code.ToString());
					this.listBoxActions.Items.Add(val.ToString());

					if (tag is SetBackgroundColorTag)
					{
						this.listBoxActions.Items.Add("       R:" +  
							((SetBackgroundColorTag)tag).RGB.red +
							"  G:" + ((SetBackgroundColorTag)tag).RGB.green +
							"  B:" + ((SetBackgroundColorTag)tag).RGB.blue);
					}

					if (tag is FrameLabelTag)
					{
						this.listBoxActions.Items.Add("       Name: " +  
							((FrameLabelTag)tag).Name);
					}

					if (((SwfDotNet.IO.Tags.BaseTag)tag) is DefineFontInfo2Tag)
					{
						this.listBoxActions.Items.Add("       FontName: " +  
							((DefineFontInfo2Tag)tag).FontName);
					}
                    

					if (((SwfDotNet.IO.Tags.BaseTag)tag).ActionRecCount != 0)
					{
						IEnumerator enum2 = ((SwfDotNet.IO.Tags.BaseTag)tag).GetEnumerator();
						while (enum2.MoveNext())
						{
							SwfDotNet.IO.ByteCode.Decompiler dc = new SwfDotNet.IO.ByteCode.Decompiler(swf.Version);
                            ArrayList actions = dc.Decompile((byte[])enum2.Current);
							foreach (BaseAction obj in actions)
							{
                              	this.listBoxActions.Items.Add("       " +  obj.ToString());
							}
						}
					}
				}
			}
			this.labelTagsCnt.Text = i.ToString();
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			foreach (int index2 in listView1.SelectedIndices)
			{
				ListViewItem item = this.listView1.Items[index2];
				this.labelCurrentFile.Text = item.SubItems[0].Text;
				int index = (int)Math.Ceiling((double)index2 / 2);
				if (this.swfList[index] != null)
					ReadSwf((Swf)this.swfList[index]);
			}

		}

		private void buttonBrowseOutput_Click(object sender, System.EventArgs e)
		{
			if (this.textBoxOutput.Text != string.Empty)
				this.folderBrowserDialog1.SelectedPath = this.textBoxDir.Text;
			DialogResult res = this.folderBrowserDialog1.ShowDialog(this);
			if (res == DialogResult.OK)
			{
				this.textBoxOutput.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			string fullPath = "D:\\Oliv\\SwfDotNet\\test\\";
			string dir = System.IO.Path.GetDirectoryName(fullPath);
			this.textBoxDir.Text = dir + System.IO.Path.DirectorySeparatorChar + "input" + System.IO.Path.DirectorySeparatorChar;
			this.textBoxOutput.Text = dir + System.IO.Path.DirectorySeparatorChar + "output" + System.IO.Path.DirectorySeparatorChar;
		}

	}
}
