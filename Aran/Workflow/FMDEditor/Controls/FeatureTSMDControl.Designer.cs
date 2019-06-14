namespace Aran.Aim.FmdEditor
{
	partial class FeatureTSMDControl
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
			System.Windows.Forms.Label label3;
			System.Windows.Forms.TabPage tabPage1;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.TabPage tabPage2;
			System.Windows.Forms.TabPage tabPage3;
			System.Windows.Forms.TabPage tabPage4;
			this.ui_dateStampNDTP = new Aran.Aim.FmdEditor.NullableDateTimePicker();
			this.ui_verticalResolutionNNud = new Aran.Aim.FmdEditor.NullableNumericUpDown();
			this.ui_horizontalResolutionNNud = new Aran.Aim.FmdEditor.NullableNumericUpDown();
			this.ui_dataIntegrityNNud = new Aran.Aim.FmdEditor.NullableNumericUpDown();
			this.ui_measureClassCB = new System.Windows.Forms.ComboBox();
			this.ui_measEquipClassTB = new System.Windows.Forms.TextBox();
			this.ui_responsiblePartyCont = new Aran.Aim.FmdEditor.ResponsiblePartyControl();
			this.ui_DQNavCont = new Aran.Aim.FmdEditor.NavigatorControl();
			this.ui_dataQualityCont = new Aran.Aim.FmdEditor.DataQualityControl();
			this.ui_identificationCont = new Aran.Aim.FmdEditor.IdentificationControl();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			label3 = new System.Windows.Forms.Label();
			tabPage1 = new System.Windows.Forms.TabPage();
			label6 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			tabPage2 = new System.Windows.Forms.TabPage();
			tabPage3 = new System.Windows.Forms.TabPage();
			tabPage4 = new System.Windows.Forms.TabPage();
			tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ui_verticalResolutionNNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ui_horizontalResolutionNNud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ui_dataIntegrityNNud)).BeginInit();
			tabPage2.SuspendLayout();
			tabPage3.SuspendLayout();
			tabPage4.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(15, 24);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(66, 13);
			label3.TabIndex = 7;
			label3.Text = "Date Stamp:";
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(this.ui_dateStampNDTP);
			tabPage1.Controls.Add(this.ui_verticalResolutionNNud);
			tabPage1.Controls.Add(this.ui_horizontalResolutionNNud);
			tabPage1.Controls.Add(this.ui_dataIntegrityNNud);
			tabPage1.Controls.Add(this.ui_measureClassCB);
			tabPage1.Controls.Add(this.ui_measEquipClassTB);
			tabPage1.Controls.Add(label6);
			tabPage1.Controls.Add(label5);
			tabPage1.Controls.Add(label4);
			tabPage1.Controls.Add(label2);
			tabPage1.Controls.Add(label1);
			tabPage1.Controls.Add(label3);
			tabPage1.Location = new System.Drawing.Point(4, 22);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(446, 344);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "General";
			// 
			// ui_dateStampNDTP
			// 
			this.ui_dateStampNDTP.CustomFormat = "yyyy-MM-dd";
			this.ui_dateStampNDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.ui_dateStampNDTP.Location = new System.Drawing.Point(164, 24);
			this.ui_dateStampNDTP.Name = "ui_dateStampNDTP";
			this.ui_dateStampNDTP.ShowCheckBox = true;
			this.ui_dateStampNDTP.Size = new System.Drawing.Size(167, 20);
			this.ui_dateStampNDTP.TabIndex = 23;
			// 
			// ui_verticalResolutionNNud
			// 
			this.ui_verticalResolutionNNud.Location = new System.Drawing.Point(164, 177);
			this.ui_verticalResolutionNNud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.ui_verticalResolutionNNud.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
			this.ui_verticalResolutionNNud.Name = "ui_verticalResolutionNNud";
			this.ui_verticalResolutionNNud.Size = new System.Drawing.Size(106, 20);
			this.ui_verticalResolutionNNud.TabIndex = 22;
			// 
			// ui_horizontalResolutionNNud
			// 
			this.ui_horizontalResolutionNNud.Location = new System.Drawing.Point(164, 145);
			this.ui_horizontalResolutionNNud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.ui_horizontalResolutionNNud.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
			this.ui_horizontalResolutionNNud.Name = "ui_horizontalResolutionNNud";
			this.ui_horizontalResolutionNNud.Size = new System.Drawing.Size(106, 20);
			this.ui_horizontalResolutionNNud.TabIndex = 21;
			// 
			// ui_dataIntegrityNNud
			// 
			this.ui_dataIntegrityNNud.Location = new System.Drawing.Point(164, 112);
			this.ui_dataIntegrityNNud.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.ui_dataIntegrityNNud.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
			this.ui_dataIntegrityNNud.Name = "ui_dataIntegrityNNud";
			this.ui_dataIntegrityNNud.Size = new System.Drawing.Size(106, 20);
			this.ui_dataIntegrityNNud.TabIndex = 20;
			// 
			// ui_measureClassCB
			// 
			this.ui_measureClassCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_measureClassCB.FormattingEnabled = true;
			this.ui_measureClassCB.Location = new System.Drawing.Point(164, 79);
			this.ui_measureClassCB.Name = "ui_measureClassCB";
			this.ui_measureClassCB.Size = new System.Drawing.Size(167, 21);
			this.ui_measureClassCB.TabIndex = 18;
			// 
			// ui_measEquipClassTB
			// 
			this.ui_measEquipClassTB.Location = new System.Drawing.Point(164, 49);
			this.ui_measEquipClassTB.Name = "ui_measEquipClassTB";
			this.ui_measEquipClassTB.Size = new System.Drawing.Size(167, 20);
			this.ui_measEquipClassTB.TabIndex = 14;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(15, 82);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(79, 13);
			label6.TabIndex = 13;
			label6.Text = "Measure Class:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(15, 179);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(98, 13);
			label5.TabIndex = 12;
			label5.Text = "Vertical Resolution:";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(15, 147);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(110, 13);
			label4.TabIndex = 11;
			label4.Text = "Horizontal Resolution:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(15, 112);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(73, 13);
			label2.TabIndex = 10;
			label2.Text = "Data Integrity:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(14, 52);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(132, 13);
			label1.TabIndex = 9;
			label1.Text = "Measure Equipment Class:";
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(this.ui_responsiblePartyCont);
			tabPage2.Location = new System.Drawing.Point(4, 22);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3);
			tabPage2.Size = new System.Drawing.Size(446, 344);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Contact";
			// 
			// ui_responsiblePartyCont
			// 
			this.ui_responsiblePartyCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_responsiblePartyCont.Location = new System.Drawing.Point(3, 3);
			this.ui_responsiblePartyCont.Name = "ui_responsiblePartyCont";
			this.ui_responsiblePartyCont.Size = new System.Drawing.Size(440, 338);
			this.ui_responsiblePartyCont.TabIndex = 0;
			// 
			// tabPage3
			// 
			tabPage3.Controls.Add(this.ui_DQNavCont);
			tabPage3.Controls.Add(this.ui_dataQualityCont);
			tabPage3.Location = new System.Drawing.Point(4, 22);
			tabPage3.Name = "tabPage3";
			tabPage3.Padding = new System.Windows.Forms.Padding(3);
			tabPage3.Size = new System.Drawing.Size(446, 344);
			tabPage3.TabIndex = 2;
			tabPage3.Text = "Data Quality Info";
			// 
			// ui_DQNavCont
			// 
			this.ui_DQNavCont.CurrentIndex = 0;
			this.ui_DQNavCont.ItemControl = this.ui_dataQualityCont;
			this.ui_DQNavCont.Location = new System.Drawing.Point(18, 18);
			this.ui_DQNavCont.Name = "ui_DQNavCont";
			this.ui_DQNavCont.Size = new System.Drawing.Size(234, 32);
			this.ui_DQNavCont.TabIndex = 1;
			// 
			// ui_dataQualityCont
			// 
			this.ui_dataQualityCont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dataQualityCont.Location = new System.Drawing.Point(13, 54);
			this.ui_dataQualityCont.Name = "ui_dataQualityCont";
			this.ui_dataQualityCont.Size = new System.Drawing.Size(427, 154);
			this.ui_dataQualityCont.TabIndex = 0;
			// 
			// tabPage4
			// 
			tabPage4.Controls.Add(this.ui_identificationCont);
			tabPage4.Location = new System.Drawing.Point(4, 22);
			tabPage4.Name = "tabPage4";
			tabPage4.Padding = new System.Windows.Forms.Padding(3);
			tabPage4.Size = new System.Drawing.Size(446, 344);
			tabPage4.TabIndex = 3;
			tabPage4.Text = "Identification Info";
			// 
			// ui_identificationCont
			// 
			this.ui_identificationCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_identificationCont.Location = new System.Drawing.Point(3, 3);
			this.ui_identificationCont.Name = "ui_identificationCont";
			this.ui_identificationCont.Size = new System.Drawing.Size(440, 338);
			this.ui_identificationCont.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(tabPage1);
			this.tabControl1.Controls.Add(tabPage2);
			this.tabControl1.Controls.Add(tabPage3);
			this.tabControl1.Controls.Add(tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(454, 370);
			this.tabControl1.TabIndex = 9;
			// 
			// FeatureTSMDControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "FeatureTSMDControl";
			this.Size = new System.Drawing.Size(454, 370);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ui_verticalResolutionNNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ui_horizontalResolutionNNud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ui_dataIntegrityNNud)).EndInit();
			tabPage2.ResumeLayout(false);
			tabPage3.ResumeLayout(false);
			tabPage4.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TextBox ui_measEquipClassTB;
		private System.Windows.Forms.ComboBox ui_measureClassCB;
		private ResponsiblePartyControl ui_responsiblePartyCont;
		private DataQualityControl ui_dataQualityCont;
		private NavigatorControl ui_DQNavCont;
		private IdentificationControl ui_identificationCont;
		private NullableNumericUpDown ui_dataIntegrityNNud;
		private NullableNumericUpDown ui_horizontalResolutionNNud;
		private NullableNumericUpDown ui_verticalResolutionNNud;
		private NullableDateTimePicker ui_dateStampNDTP;
	}
}
