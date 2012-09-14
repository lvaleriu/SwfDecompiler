namespace SwfDecompiler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSwf = new System.Windows.Forms.TextBox();
            this.buttonBrowseSwf = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listViewPix = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.buttonBrowseDir = new System.Windows.Forms.Button();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.listViewSounds = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.listViewActionScript = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelSwfVersion = new System.Windows.Forms.Label();
            this.labelSwfDim = new System.Windows.Forms.Label();
            this.labelSwfSize = new System.Windows.Forms.Label();
            this.labelSwfFps = new System.Windows.Forms.Label();
            this.labelSwfFrames = new System.Windows.Forms.Label();
            this.labelSwfSign = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelSwfBgColor = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonBrowseDir);
            this.panel1.Controls.Add(this.textBoxDir);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonGenerate);
            this.panel1.Controls.Add(this.buttonBrowseSwf);
            this.panel1.Controls.Add(this.textBoxSwf);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 63);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(522, 216);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Swf file:";
            // 
            // textBoxSwf
            // 
            this.textBoxSwf.Location = new System.Drawing.Point(84, 9);
            this.textBoxSwf.Name = "textBoxSwf";
            this.textBoxSwf.Size = new System.Drawing.Size(228, 20);
            this.textBoxSwf.TabIndex = 1;
            // 
            // buttonBrowseSwf
            // 
            this.buttonBrowseSwf.Location = new System.Drawing.Point(318, 8);
            this.buttonBrowseSwf.Name = "buttonBrowseSwf";
            this.buttonBrowseSwf.Size = new System.Drawing.Size(59, 23);
            this.buttonBrowseSwf.TabIndex = 2;
            this.buttonBrowseSwf.Text = "Browse";
            this.buttonBrowseSwf.UseVisualStyleBackColor = true;
            this.buttonBrowseSwf.Click += new System.EventHandler(this.buttonBrowseSwf_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(396, 9);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(108, 23);
            this.buttonGenerate.TabIndex = 3;
            this.buttonGenerate.Text = "Decompile it !";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(522, 216);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listViewPix);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(514, 190);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pictures";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewSounds);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(514, 190);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sounds";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listViewActionScript);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(514, 190);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Action script bytecode";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listViewPix
            // 
            this.listViewPix.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewPix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPix.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPix.Location = new System.Drawing.Point(3, 3);
            this.listViewPix.Name = "listViewPix";
            this.listViewPix.Size = new System.Drawing.Size(508, 184);
            this.listViewPix.TabIndex = 0;
            this.listViewPix.UseCompatibleStateImageBehavior = false;
            this.listViewPix.View = System.Windows.Forms.View.Details;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output dir:";
            // 
            // textBoxDir
            // 
            this.textBoxDir.Location = new System.Drawing.Point(84, 35);
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.Size = new System.Drawing.Size(228, 20);
            this.textBoxDir.TabIndex = 5;
            // 
            // buttonBrowseDir
            // 
            this.buttonBrowseDir.Location = new System.Drawing.Point(318, 35);
            this.buttonBrowseDir.Name = "buttonBrowseDir";
            this.buttonBrowseDir.Size = new System.Drawing.Size(59, 23);
            this.buttonBrowseDir.TabIndex = 6;
            this.buttonBrowseDir.Text = "Browse";
            this.buttonBrowseDir.UseVisualStyleBackColor = true;
            this.buttonBrowseDir.Click += new System.EventHandler(this.buttonBrowseDir_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File name";
            this.columnHeader1.Width = 391;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Format";
            // 
            // listViewSounds
            // 
            this.listViewSounds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listViewSounds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSounds.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSounds.Location = new System.Drawing.Point(3, 3);
            this.listViewSounds.Name = "listViewSounds";
            this.listViewSounds.Size = new System.Drawing.Size(508, 184);
            this.listViewSounds.TabIndex = 1;
            this.listViewSounds.UseCompatibleStateImageBehavior = false;
            this.listViewSounds.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "File name";
            this.columnHeader3.Width = 391;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Format";
            // 
            // listViewActionScript
            // 
            this.listViewActionScript.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.listViewActionScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewActionScript.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewActionScript.Location = new System.Drawing.Point(0, 0);
            this.listViewActionScript.Name = "listViewActionScript";
            this.listViewActionScript.Size = new System.Drawing.Size(514, 190);
            this.listViewActionScript.TabIndex = 1;
            this.listViewActionScript.UseCompatibleStateImageBehavior = false;
            this.listViewActionScript.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "File name";
            this.columnHeader5.Width = 391;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Format";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Swf file|*.swf";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelSwfBgColor);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.labelSwfSign);
            this.tabPage4.Controls.Add(this.labelSwfFrames);
            this.tabPage4.Controls.Add(this.labelSwfFps);
            this.tabPage4.Controls.Add(this.labelSwfSize);
            this.tabPage4.Controls.Add(this.labelSwfDim);
            this.tabPage4.Controls.Add(this.labelSwfVersion);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(514, 190);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "File info";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Swf version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Dimensions:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "File size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Fps:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Frames count:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Signature:";
            // 
            // labelSwfVersion
            // 
            this.labelSwfVersion.AutoSize = true;
            this.labelSwfVersion.Location = new System.Drawing.Point(107, 17);
            this.labelSwfVersion.Name = "labelSwfVersion";
            this.labelSwfVersion.Size = new System.Drawing.Size(27, 13);
            this.labelSwfVersion.TabIndex = 6;
            this.labelSwfVersion.Text = "N/A";
            // 
            // labelSwfDim
            // 
            this.labelSwfDim.AutoSize = true;
            this.labelSwfDim.Location = new System.Drawing.Point(107, 40);
            this.labelSwfDim.Name = "labelSwfDim";
            this.labelSwfDim.Size = new System.Drawing.Size(27, 13);
            this.labelSwfDim.TabIndex = 7;
            this.labelSwfDim.Text = "N/A";
            // 
            // labelSwfSize
            // 
            this.labelSwfSize.AutoSize = true;
            this.labelSwfSize.Location = new System.Drawing.Point(107, 62);
            this.labelSwfSize.Name = "labelSwfSize";
            this.labelSwfSize.Size = new System.Drawing.Size(27, 13);
            this.labelSwfSize.TabIndex = 8;
            this.labelSwfSize.Text = "N/A";
            // 
            // labelSwfFps
            // 
            this.labelSwfFps.AutoSize = true;
            this.labelSwfFps.Location = new System.Drawing.Point(107, 85);
            this.labelSwfFps.Name = "labelSwfFps";
            this.labelSwfFps.Size = new System.Drawing.Size(27, 13);
            this.labelSwfFps.TabIndex = 9;
            this.labelSwfFps.Text = "N/A";
            // 
            // labelSwfFrames
            // 
            this.labelSwfFrames.AutoSize = true;
            this.labelSwfFrames.Location = new System.Drawing.Point(107, 107);
            this.labelSwfFrames.Name = "labelSwfFrames";
            this.labelSwfFrames.Size = new System.Drawing.Size(27, 13);
            this.labelSwfFrames.TabIndex = 10;
            this.labelSwfFrames.Text = "N/A";
            // 
            // labelSwfSign
            // 
            this.labelSwfSign.AutoSize = true;
            this.labelSwfSign.Location = new System.Drawing.Point(107, 130);
            this.labelSwfSign.Name = "labelSwfSign";
            this.labelSwfSign.Size = new System.Drawing.Size(27, 13);
            this.labelSwfSign.TabIndex = 11;
            this.labelSwfSign.Text = "N/A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Bg color:";
            // 
            // labelSwfBgColor
            // 
            this.labelSwfBgColor.AutoSize = true;
            this.labelSwfBgColor.Location = new System.Drawing.Point(107, 154);
            this.labelSwfBgColor.Name = "labelSwfBgColor";
            this.labelSwfBgColor.Size = new System.Drawing.Size(27, 13);
            this.labelSwfBgColor.TabIndex = 13;
            this.labelSwfBgColor.Text = "N/A";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 279);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "A really simple Swf decompiler";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBrowseSwf;
        private System.Windows.Forms.TextBox textBoxSwf;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listViewPix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDir;
        private System.Windows.Forms.Button buttonBrowseDir;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listViewSounds;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView listViewActionScript;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelSwfSign;
        private System.Windows.Forms.Label labelSwfFrames;
        private System.Windows.Forms.Label labelSwfFps;
        private System.Windows.Forms.Label labelSwfSize;
        private System.Windows.Forms.Label labelSwfDim;
        private System.Windows.Forms.Label labelSwfVersion;
        private System.Windows.Forms.Label labelSwfBgColor;
        private System.Windows.Forms.Label label9;
    }
}

