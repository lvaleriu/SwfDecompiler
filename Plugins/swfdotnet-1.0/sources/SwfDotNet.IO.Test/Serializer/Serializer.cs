using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;

using SwfDotNet.IO;

namespace SwfDotNet.IO.Test.Serializer
{
	/// <summary>
	/// Description résumée de Form1.
	/// </summary>
	public class Serializer : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button buttSerialize;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private AxSHDocVw.AxWebBrowser axWebBrowser1;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button butSelect;
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Serializer()
		{
			//
			// Requis pour la prise en charge du Concepteur Windows Forms
			//
			InitializeComponent();

			//
			// TODO : ajoutez le code du constructeur après l'appel à InitializeComponent
			//
		}

		/// <summary>
		/// Nettoyage des ressources utilisées.
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

		#region Code généré par le Concepteur Windows Form
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Serializer));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttSerialize = new System.Windows.Forms.Button();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.panel1 = new System.Windows.Forms.Panel();
			this.butSelect = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butSelect);
			this.groupBox1.Controls.Add(this.buttSerialize);
			this.groupBox1.Controls.Add(this.txtPath);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(686, 51);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File to serialize";
			// 
			// buttSerialize
			// 
			this.buttSerialize.Location = new System.Drawing.Point(488, 16);
			this.buttSerialize.Name = "buttSerialize";
			this.buttSerialize.TabIndex = 2;
			this.buttSerialize.Text = "Serialize !";
			this.buttSerialize.Click += new System.EventHandler(this.buttSerialize_Click);
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(80, 16);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(320, 20);
			this.txtPath.TabIndex = 1;
			this.txtPath.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select a file:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Swf files|*.swf";
			this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.ContainingControl = this;
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 5);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(686, 340);
			this.axWebBrowser1.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.axWebBrowser1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.DockPadding.Top = 5;
			this.panel1.Location = new System.Drawing.Point(5, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(686, 345);
			this.panel1.TabIndex = 2;
			// 
			// butSelect
			// 
			this.butSelect.Location = new System.Drawing.Point(408, 16);
			this.butSelect.Name = "butSelect";
			this.butSelect.Size = new System.Drawing.Size(32, 23);
			this.butSelect.TabIndex = 3;
			this.butSelect.Text = "...";
			this.butSelect.Click += new System.EventHandler(this.butSelect_Click);
			// 
			// Serializer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(696, 406);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupBox1);
			this.DockPadding.All = 5;
			this.Name = "Serializer";
			this.Text = "Swf Serializer";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Serializer());
		}

        private void buttSerialize_Click(object sender, System.EventArgs e)
        {
			if (txtPath.Text == string.Empty)
				return;

			string swfPath = txtPath.Text;
			string xmlPath = txtPath.Text + ".xml";

			SwfReader reader = new SwfReader(swfPath);
			Swf swf = reader.ReadSwf();
			reader.Close();
			
			XmlTextWriter writer = new XmlTextWriter(xmlPath, Encoding.UTF8);
			swf.Serialize(writer);
			writer.Close();

			object o = null;
			this.axWebBrowser1.Navigate("file://" + xmlPath, ref o, ref o, ref o, ref o);		
        }

		private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.txtPath.Text = this.openFileDialog.FileName;
		}

		private void butSelect_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog.ShowDialog(this);	
		}
	}
}
