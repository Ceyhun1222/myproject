namespace Aran.Aim.FmdEditor
{
	partial class ResponsiblePartyControl
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.TabPage tabPage1;
			System.Windows.Forms.TabPage tabPage2;
			this.ui_roleCB = new System.Windows.Forms.ComboBox();
			this.ui_digitalCertificateTB = new System.Windows.Forms.TextBox();
			this.ui_systemNameTB = new System.Windows.Forms.TextBox();
			this.ui_positionNameTB = new System.Windows.Forms.TextBox();
			this.ui_organisationNameTB = new System.Windows.Forms.TextBox();
			this.ui_individualNameTB = new System.Windows.Forms.TextBox();
			this.ui_contactCont = new Aran.Aim.FmdEditor.ContactControl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			tabPage1 = new System.Windows.Forms.TabPage();
			tabPage2 = new System.Windows.Forms.TabPage();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(11, 20);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(86, 13);
			label1.TabIndex = 0;
			label1.Text = "Individual Name:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(11, 51);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(100, 13);
			label2.TabIndex = 2;
			label2.Text = "Organisation Name:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(11, 80);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(78, 13);
			label3.TabIndex = 4;
			label3.Text = "Position Name:";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(11, 175);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(89, 13);
			label4.TabIndex = 10;
			label4.Text = "Digital Certificate:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(11, 142);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(75, 13);
			label5.TabIndex = 8;
			label5.Text = "System Name:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(11, 108);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(32, 13);
			label6.TabIndex = 6;
			label6.Text = "Role:";
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(label1);
			tabPage1.Controls.Add(label2);
			tabPage1.Controls.Add(this.ui_roleCB);
			tabPage1.Controls.Add(label3);
			tabPage1.Controls.Add(this.ui_digitalCertificateTB);
			tabPage1.Controls.Add(label6);
			tabPage1.Controls.Add(this.ui_systemNameTB);
			tabPage1.Controls.Add(label5);
			tabPage1.Controls.Add(this.ui_positionNameTB);
			tabPage1.Controls.Add(label4);
			tabPage1.Controls.Add(this.ui_organisationNameTB);
			tabPage1.Controls.Add(this.ui_individualNameTB);
			tabPage1.Location = new System.Drawing.Point(4, 22);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(409, 268);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "General";
			// 
			// ui_roleCB
			// 
			this.ui_roleCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_roleCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_roleCB.FormattingEnabled = true;
			this.ui_roleCB.Location = new System.Drawing.Point(117, 105);
			this.ui_roleCB.Name = "ui_roleCB";
			this.ui_roleCB.Size = new System.Drawing.Size(269, 21);
			this.ui_roleCB.TabIndex = 7;
			// 
			// ui_digitalCertificateTB
			// 
			this.ui_digitalCertificateTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_digitalCertificateTB.Location = new System.Drawing.Point(117, 172);
			this.ui_digitalCertificateTB.Name = "ui_digitalCertificateTB";
			this.ui_digitalCertificateTB.Size = new System.Drawing.Size(269, 20);
			this.ui_digitalCertificateTB.TabIndex = 11;
			// 
			// ui_systemNameTB
			// 
			this.ui_systemNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_systemNameTB.Location = new System.Drawing.Point(117, 139);
			this.ui_systemNameTB.Name = "ui_systemNameTB";
			this.ui_systemNameTB.Size = new System.Drawing.Size(269, 20);
			this.ui_systemNameTB.TabIndex = 9;
			// 
			// ui_positionNameTB
			// 
			this.ui_positionNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_positionNameTB.Location = new System.Drawing.Point(117, 77);
			this.ui_positionNameTB.Name = "ui_positionNameTB";
			this.ui_positionNameTB.Size = new System.Drawing.Size(269, 20);
			this.ui_positionNameTB.TabIndex = 5;
			// 
			// ui_organisationNameTB
			// 
			this.ui_organisationNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_organisationNameTB.Location = new System.Drawing.Point(117, 48);
			this.ui_organisationNameTB.Name = "ui_organisationNameTB";
			this.ui_organisationNameTB.Size = new System.Drawing.Size(269, 20);
			this.ui_organisationNameTB.TabIndex = 3;
			// 
			// ui_individualNameTB
			// 
			this.ui_individualNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_individualNameTB.Location = new System.Drawing.Point(117, 17);
			this.ui_individualNameTB.Name = "ui_individualNameTB";
			this.ui_individualNameTB.Size = new System.Drawing.Size(269, 20);
			this.ui_individualNameTB.TabIndex = 1;
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(this.ui_contactCont);
			tabPage2.Location = new System.Drawing.Point(4, 22);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3);
			tabPage2.Size = new System.Drawing.Size(409, 268);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Contact";
			// 
			// ui_contactCont
			// 
			this.ui_contactCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_contactCont.Location = new System.Drawing.Point(3, 3);
			this.ui_contactCont.Name = "ui_contactCont";
			this.ui_contactCont.Size = new System.Drawing.Size(403, 262);
			this.ui_contactCont.TabIndex = 12;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(tabPage1);
			this.tabControl1.Controls.Add(tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(417, 294);
			this.tabControl1.TabIndex = 13;
			// 
			// ResponsiblePartyControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "ResponsiblePartyControl";
			this.Size = new System.Drawing.Size(417, 294);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			tabPage2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox ui_individualNameTB;
		private System.Windows.Forms.TextBox ui_organisationNameTB;
		private System.Windows.Forms.TextBox ui_positionNameTB;
		private System.Windows.Forms.TextBox ui_digitalCertificateTB;
		private System.Windows.Forms.TextBox ui_systemNameTB;
		private System.Windows.Forms.ComboBox ui_roleCB;
		private ContactControl ui_contactCont;
		private System.Windows.Forms.TabControl tabControl1;

	}
}
