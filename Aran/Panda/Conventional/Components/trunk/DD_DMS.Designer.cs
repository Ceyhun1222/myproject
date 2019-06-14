namespace ChoosePointNS
{
	partial class DD_DMS
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.lbDecDegrees = new System.Windows.Forms.Label();
            this.cbNSEW = new System.Windows.Forms.ComboBox();
            this.lbSeconds = new System.Windows.Forms.Label();
            this.lbMinutes = new System.Windows.Forms.Label();
            this.lbDegrees = new System.Windows.Forms.Label();
            this.ui_contextMI = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_pasteCoordinateTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ntDecimals = new ChoosePointNS.NumericTextBox();
            this.ntSeconds = new ChoosePointNS.NumericTextBox();
            this.ntMinutes = new ChoosePointNS.NumericTextBox();
            this.ntDegrees = new ChoosePointNS.NumericTextBox();
            this.ui_contextMI.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDecDegrees
            // 
            this.lbDecDegrees.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDecDegrees.AutoSize = true;
            this.lbDecDegrees.Location = new System.Drawing.Point(95, 41);
            this.lbDecDegrees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDecDegrees.Name = "lbDecDegrees";
            this.lbDecDegrees.Size = new System.Drawing.Size(14, 17);
            this.lbDecDegrees.TabIndex = 8;
            this.lbDecDegrees.Text = "°";
            // 
            // cbNSEW
            // 
            this.cbNSEW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNSEW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNSEW.FormattingEnabled = true;
            this.cbNSEW.Items.AddRange(new object[] {
            "N",
            "S",
            "E",
            "W"});
            this.cbNSEW.Location = new System.Drawing.Point(197, 2);
            this.cbNSEW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbNSEW.Name = "cbNSEW";
            this.cbNSEW.Size = new System.Drawing.Size(45, 24);
            this.cbNSEW.TabIndex = 3;
            this.cbNSEW.SelectedIndexChanged += new System.EventHandler(this.cbNSEW_SelectedIndexChanged);
            this.cbNSEW.Leave += new System.EventHandler(this.cbNSEW_Leave);
            // 
            // lbSeconds
            // 
            this.lbSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSeconds.AutoSize = true;
            this.lbSeconds.Location = new System.Drawing.Point(173, 4);
            this.lbSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSeconds.Name = "lbSeconds";
            this.lbSeconds.Size = new System.Drawing.Size(13, 17);
            this.lbSeconds.TabIndex = 7;
            this.lbSeconds.Text = "\"";
            // 
            // lbMinutes
            // 
            this.lbMinutes.AutoSize = true;
            this.lbMinutes.Location = new System.Drawing.Point(108, 4);
            this.lbMinutes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMinutes.Name = "lbMinutes";
            this.lbMinutes.Size = new System.Drawing.Size(11, 17);
            this.lbMinutes.TabIndex = 6;
            this.lbMinutes.Text = "\'";
            // 
            // lbDegrees
            // 
            this.lbDegrees.AutoSize = true;
            this.lbDegrees.Location = new System.Drawing.Point(44, 0);
            this.lbDegrees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDegrees.Name = "lbDegrees";
            this.lbDegrees.Size = new System.Drawing.Size(14, 17);
            this.lbDegrees.TabIndex = 5;
            this.lbDegrees.Text = "°";
            // 
            // ui_contextMI
            // 
            this.ui_contextMI.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_contextMI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_pasteCoordinateTSMI});
            this.ui_contextMI.Name = "ui_contextMI";
            this.ui_contextMI.Size = new System.Drawing.Size(191, 28);
            // 
            // ui_pasteCoordinateTSMI
            // 
            this.ui_pasteCoordinateTSMI.Name = "ui_pasteCoordinateTSMI";
            this.ui_pasteCoordinateTSMI.Size = new System.Drawing.Size(190, 24);
            this.ui_pasteCoordinateTSMI.Text = "Paste Coordinate";
            this.ui_pasteCoordinateTSMI.Click += new System.EventHandler(this.PasteCoordinate_Click);
            // 
            // ntDecimals
            // 
            this.ntDecimals.Accuracy = 6;
            this.ntDecimals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ntDecimals.IsClockwise = true;
            this.ntDecimals.Location = new System.Drawing.Point(1, 41);
            this.ntDecimals.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ntDecimals.Maximum = 100D;
            this.ntDecimals.Minimum = 0D;
            this.ntDecimals.Name = "ntDecimals";
            this.ntDecimals.ReadOnly = false;
            this.ntDecimals.Size = new System.Drawing.Size(84, 30);
            this.ntDecimals.TabIndex = 4;
            this.ntDecimals.Value = 20D;
            this.ntDecimals.ValueIsAngle = true;
            this.ntDecimals.ValueChanged += new System.EventHandler(this.ntDecimals_ValueChanged);
            this.ntDecimals.KeyPressChanged += new System.Windows.Forms.KeyPressEventHandler(this.ntDegrees_KeyPressChanged);
            // 
            // ntSeconds
            // 
            this.ntSeconds.Accuracy = 2;
            this.ntSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ntSeconds.IsClockwise = true;
            this.ntSeconds.Location = new System.Drawing.Point(128, 4);
            this.ntSeconds.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ntSeconds.Maximum = 60D;
            this.ntSeconds.Minimum = 0D;
            this.ntSeconds.Name = "ntSeconds";
            this.ntSeconds.ReadOnly = false;
            this.ntSeconds.Size = new System.Drawing.Size(37, 28);
            this.ntSeconds.TabIndex = 2;
            this.ntSeconds.Value = 0D;
            this.ntSeconds.ValueIsAngle = true;
            this.ntSeconds.ValueChanged += new System.EventHandler(this.ntDegrees_ValueChanged);
            this.ntSeconds.KeyPressChanged += new System.Windows.Forms.KeyPressEventHandler(this.ntDegrees_KeyPressChanged);
            // 
            // ntMinutes
            // 
            this.ntMinutes.Accuracy = 0;
            this.ntMinutes.IsClockwise = true;
            this.ntMinutes.Location = new System.Drawing.Point(67, 4);
            this.ntMinutes.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ntMinutes.Maximum = 60D;
            this.ntMinutes.Minimum = 0D;
            this.ntMinutes.Name = "ntMinutes";
            this.ntMinutes.ReadOnly = false;
            this.ntMinutes.Size = new System.Drawing.Size(33, 28);
            this.ntMinutes.TabIndex = 1;
            this.ntMinutes.Value = 0D;
            this.ntMinutes.ValueIsAngle = true;
            this.ntMinutes.ValueChanged += new System.EventHandler(this.ntDegrees_ValueChanged);
            this.ntMinutes.KeyPressChanged += new System.Windows.Forms.KeyPressEventHandler(this.ntDegrees_KeyPressChanged);
            // 
            // ntDegrees
            // 
            this.ntDegrees.Accuracy = 0;
            this.ntDegrees.IsClockwise = true;
            this.ntDegrees.Location = new System.Drawing.Point(1, 4);
            this.ntDegrees.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ntDegrees.Maximum = 100D;
            this.ntDegrees.Minimum = 0D;
            this.ntDegrees.Name = "ntDegrees";
            this.ntDegrees.ReadOnly = false;
            this.ntDegrees.Size = new System.Drawing.Size(35, 30);
            this.ntDegrees.TabIndex = 0;
            this.ntDegrees.Value = 20D;
            this.ntDegrees.ValueIsAngle = true;
            this.ntDegrees.ValueChanged += new System.EventHandler(this.ntDegrees_ValueChanged);
            this.ntDegrees.KeyPressChanged += new System.Windows.Forms.KeyPressEventHandler(this.ntDegrees_KeyPressChanged);
            // 
            // DD_DMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.ui_contextMI;
            this.Controls.Add(this.lbDecDegrees);
            this.Controls.Add(this.ntDecimals);
            this.Controls.Add(this.cbNSEW);
            this.Controls.Add(this.lbSeconds);
            this.Controls.Add(this.ntSeconds);
            this.Controls.Add(this.lbMinutes);
            this.Controls.Add(this.ntMinutes);
            this.Controls.Add(this.lbDegrees);
            this.Controls.Add(this.ntDegrees);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DD_DMS";
            this.Size = new System.Drawing.Size(260, 68);
            this.Resize += new System.EventHandler(this.DD_DMS_Resize);
            this.ui_contextMI.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbDecDegrees;
		private NumericTextBox ntDecimals;
		private System.Windows.Forms.ComboBox cbNSEW;
		private System.Windows.Forms.Label lbSeconds;
		private NumericTextBox ntSeconds;
		private System.Windows.Forms.Label lbMinutes;
		private NumericTextBox ntMinutes;
		private System.Windows.Forms.Label lbDegrees;
		private NumericTextBox ntDegrees;
        private System.Windows.Forms.ContextMenuStrip ui_contextMI;
        private System.Windows.Forms.ToolStripMenuItem ui_pasteCoordinateTSMI;

	}
}
