namespace Aran.Aim.FmdEditor
{
	partial class IdentificationControl
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.TabPage tabPage1;
			System.Windows.Forms.TabPage tabPage2;
			System.Windows.Forms.TabPage tabPage3;
			this.ui_languageCB = new System.Windows.Forms.ComboBox();
			this.ui_dataStatusCB = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ui_abstractTB = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ui_citiationCont = new Aran.Aim.FmdEditor.CitiationControl();
			this.ui_responsiblePartyCont = new Aran.Aim.FmdEditor.ResponsiblePartyControl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			label1 = new System.Windows.Forms.Label();
			tabPage1 = new System.Windows.Forms.TabPage();
			tabPage2 = new System.Windows.Forms.TabPage();
			tabPage3 = new System.Windows.Forms.TabPage();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			tabPage3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(24, 25);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(49, 13);
			label1.TabIndex = 0;
			label1.Text = "Abstract:";
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(label1);
			tabPage1.Controls.Add(this.ui_languageCB);
			tabPage1.Controls.Add(this.ui_dataStatusCB);
			tabPage1.Controls.Add(this.label3);
			tabPage1.Controls.Add(this.ui_abstractTB);
			tabPage1.Controls.Add(this.label2);
			tabPage1.Location = new System.Drawing.Point(4, 22);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(426, 307);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "General";
			// 
			// ui_languageCB
			// 
			this.ui_languageCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_languageCB.FormattingEnabled = true;
			this.ui_languageCB.Location = new System.Drawing.Point(106, 81);
			this.ui_languageCB.Name = "ui_languageCB";
			this.ui_languageCB.Size = new System.Drawing.Size(141, 21);
			this.ui_languageCB.TabIndex = 5;
			// 
			// ui_dataStatusCB
			// 
			this.ui_dataStatusCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_dataStatusCB.FormattingEnabled = true;
			this.ui_dataStatusCB.Location = new System.Drawing.Point(106, 50);
			this.ui_dataStatusCB.Name = "ui_dataStatusCB";
			this.ui_dataStatusCB.Size = new System.Drawing.Size(141, 21);
			this.ui_dataStatusCB.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 87);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Language:";
			// 
			// ui_abstractTB
			// 
			this.ui_abstractTB.Location = new System.Drawing.Point(106, 22);
			this.ui_abstractTB.Name = "ui_abstractTB";
			this.ui_abstractTB.Size = new System.Drawing.Size(141, 20);
			this.ui_abstractTB.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Data Status:";
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(this.ui_citiationCont);
			tabPage2.Location = new System.Drawing.Point(4, 22);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3);
			tabPage2.Size = new System.Drawing.Size(426, 307);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Citiation";
			// 
			// ui_citiationCont
			// 
			this.ui_citiationCont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_citiationCont.Location = new System.Drawing.Point(11, 10);
			this.ui_citiationCont.Name = "ui_citiationCont";
			this.ui_citiationCont.Size = new System.Drawing.Size(365, 143);
			this.ui_citiationCont.TabIndex = 0;
			// 
			// tabPage3
			// 
			tabPage3.Controls.Add(this.ui_responsiblePartyCont);
			tabPage3.Location = new System.Drawing.Point(4, 22);
			tabPage3.Name = "tabPage3";
			tabPage3.Padding = new System.Windows.Forms.Padding(3);
			tabPage3.Size = new System.Drawing.Size(426, 307);
			tabPage3.TabIndex = 2;
			tabPage3.Text = "PointOfContact";
			// 
			// ui_responsiblePartyCont
			// 
			this.ui_responsiblePartyCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_responsiblePartyCont.Location = new System.Drawing.Point(3, 3);
			this.ui_responsiblePartyCont.Name = "ui_responsiblePartyCont";
			this.ui_responsiblePartyCont.Size = new System.Drawing.Size(420, 301);
			this.ui_responsiblePartyCont.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(tabPage1);
			this.tabControl1.Controls.Add(tabPage2);
			this.tabControl1.Controls.Add(tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(434, 333);
			this.tabControl1.TabIndex = 6;
			// 
			// IdentificationControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "IdentificationControl";
			this.Size = new System.Drawing.Size(434, 333);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			tabPage2.ResumeLayout(false);
			tabPage3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox ui_dataStatusCB;
		private System.Windows.Forms.TextBox ui_abstractTB;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox ui_languageCB;
		private System.Windows.Forms.TabControl tabControl1;
		private CitiationControl ui_citiationCont;
		private ResponsiblePartyControl ui_responsiblePartyCont;
	}
}
