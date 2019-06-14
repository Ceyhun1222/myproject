namespace Aran.Aim.FmdEditor
{
	partial class ContactControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.Windows.Forms.TabControl tabControl1;
			System.Windows.Forms.TabPage tabPage1;
			this.ui_addressCont = new Aran.Aim.FmdEditor.CIAddressControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.ui_telephoneCont = new Aran.Aim.FmdEditor.TelephoneControl();
			tabControl1 = new System.Windows.Forms.TabControl();
			tabPage1 = new System.Windows.Forms.TabPage();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(this.tabPage2);
			tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl1.Location = new System.Drawing.Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new System.Drawing.Size(385, 244);
			tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(this.ui_addressCont);
			tabPage1.Location = new System.Drawing.Point(4, 22);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(377, 218);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Address";
			// 
			// ui_addressCont
			// 
			this.ui_addressCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_addressCont.Location = new System.Drawing.Point(3, 3);
			this.ui_addressCont.Name = "ui_addressCont";
			this.ui_addressCont.Size = new System.Drawing.Size(371, 212);
			this.ui_addressCont.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.ui_telephoneCont);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(377, 218);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Phone";
			// 
			// ui_telephoneCont
			// 
			this.ui_telephoneCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_telephoneCont.Location = new System.Drawing.Point(3, 3);
			this.ui_telephoneCont.Name = "ui_telephoneCont";
			this.ui_telephoneCont.Size = new System.Drawing.Size(371, 212);
			this.ui_telephoneCont.TabIndex = 0;
			// 
			// ContactControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(tabControl1);
			this.Name = "ContactControl";
			this.Size = new System.Drawing.Size(385, 244);
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabPage tabPage2;
		private CIAddressControl ui_addressCont;
		private TelephoneControl ui_telephoneCont;

	}
}
