namespace Aran.Queries.Common
{
    partial class PropControl
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
            this.components = new System.ComponentModel.Container();
            this.stringTb = new System.Windows.Forms.TextBox();
            this.propNameLabel = new System.Windows.Forms.Label();
            this.boolChB = new System.Windows.Forms.CheckBox();
            this.enumCb = new System.Windows.Forms.ComboBox();
            this.uomPanel = new System.Windows.Forms.Panel();
            this.uomCb = new System.Windows.Forms.ComboBox();
            this.uomValueNud = new Aran.Queries.Common.NNumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ui_clearButton = new System.Windows.Forms.Button();
            this.ui_warningPicBox = new System.Windows.Forms.PictureBox();
            this.ui_selNilReason = new System.Windows.Forms.Button();
            this.ui_infoButton = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.coordPanel = new System.Windows.Forms.Panel();
            this.labelCoordY = new System.Windows.Forms.Label();
            this.coordYTB = new System.Windows.Forms.TextBox();
            this.labelCoordX = new System.Windows.Forms.Label();
            this.coordXTB = new System.Windows.Forms.TextBox();
            this.notePanel = new System.Windows.Forms.Panel();
            this.noteCB = new System.Windows.Forms.ComboBox();
            this.noteTB = new System.Windows.Forms.TextBox();
            this.ui_geomPanel = new System.Windows.Forms.Panel();
            this.ui_showGeomLink = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_nilReasonCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.intTb = new Aran.Queries.Common.NNumericUpDown();
            this.ui_coordinateControl = new Aran.Queries.Common.CoordinateControl();
            this.doubleNud = new Aran.Queries.Common.NNumericUpDown();
            this.uomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uomValueNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_warningPicBox)).BeginInit();
            this.coordPanel.SuspendLayout();
            this.notePanel.SuspendLayout();
            this.ui_geomPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intTb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doubleNud)).BeginInit();
            this.SuspendLayout();
            // 
            // stringTb
            // 
            this.stringTb.Location = new System.Drawing.Point(159, 5);
            this.stringTb.Name = "stringTb";
            this.stringTb.Size = new System.Drawing.Size(216, 20);
            this.stringTb.TabIndex = 0;
            this.stringTb.Visible = false;
            // 
            // propNameLabel
            // 
            this.propNameLabel.AutoEllipsis = true;
            this.propNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.propNameLabel.Location = new System.Drawing.Point(6, 9);
            this.propNameLabel.Name = "propNameLabel";
            this.propNameLabel.Size = new System.Drawing.Size(147, 15);
            this.propNameLabel.TabIndex = 1;
            this.propNameLabel.Text = "<property name>";
            this.propNameLabel.UseCompatibleTextRendering = true;
            // 
            // boolChB
            // 
            this.boolChB.Location = new System.Drawing.Point(159, 35);
            this.boolChB.Name = "boolChB";
            this.boolChB.Size = new System.Drawing.Size(216, 16);
            this.boolChB.TabIndex = 2;
            this.boolChB.ThreeState = true;
            this.boolChB.UseVisualStyleBackColor = true;
            this.boolChB.Visible = false;
            // 
            // enumCb
            // 
            this.enumCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.enumCb.FormattingEnabled = true;
            this.enumCb.Location = new System.Drawing.Point(159, 109);
            this.enumCb.Name = "enumCb";
            this.enumCb.Size = new System.Drawing.Size(216, 21);
            this.enumCb.TabIndex = 5;
            this.enumCb.Visible = false;
            // 
            // uomPanel
            // 
            this.uomPanel.Controls.Add(this.uomCb);
            this.uomPanel.Controls.Add(this.uomValueNud);
            this.uomPanel.Location = new System.Drawing.Point(160, 136);
            this.uomPanel.Name = "uomPanel";
            this.uomPanel.Size = new System.Drawing.Size(215, 27);
            this.uomPanel.TabIndex = 6;
            this.uomPanel.Visible = false;
            // 
            // uomCb
            // 
            this.uomCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uomCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uomCb.FormattingEnabled = true;
            this.uomCb.Location = new System.Drawing.Point(160, 1);
            this.uomCb.Name = "uomCb";
            this.uomCb.Size = new System.Drawing.Size(54, 21);
            this.uomCb.TabIndex = 1;
            // 
            // uomValueNud
            // 
            this.uomValueNud.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uomValueNud.DecimalPlaces = 1;
            this.uomValueNud.IsNull = true;
            this.uomValueNud.Location = new System.Drawing.Point(0, 2);
            this.uomValueNud.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.uomValueNud.Minimum = new decimal(new int[] {
            1215752192,
            23,
            0,
            -2147483648});
            this.uomValueNud.Name = "uomValueNud";
            this.uomValueNud.Size = new System.Drawing.Size(158, 20);
            this.uomValueNud.TabIndex = 0;
            // 
            // ui_clearButton
            // 
            this.ui_clearButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ui_clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_clearButton.Image = global::Aran.Queries.Common.Properties.Resources.edit_clear_24;
            this.ui_clearButton.Location = new System.Drawing.Point(3, 3);
            this.ui_clearButton.Name = "ui_clearButton";
            this.ui_clearButton.Size = new System.Drawing.Size(28, 28);
            this.ui_clearButton.TabIndex = 26;
            this.toolTip1.SetToolTip(this.ui_clearButton, "Set <NULL>");
            this.ui_clearButton.UseVisualStyleBackColor = true;
            this.ui_clearButton.Visible = false;
            this.ui_clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // ui_warningPicBox
            // 
            this.ui_warningPicBox.Image = global::Aran.Queries.Common.Properties.Resources.warning_icon;
            this.ui_warningPicBox.Location = new System.Drawing.Point(37, 3);
            this.ui_warningPicBox.Name = "ui_warningPicBox";
            this.ui_warningPicBox.Size = new System.Drawing.Size(35, 32);
            this.ui_warningPicBox.TabIndex = 28;
            this.ui_warningPicBox.TabStop = false;
            this.toolTip1.SetToolTip(this.ui_warningPicBox, "Warning");
            this.ui_warningPicBox.Visible = false;
            // 
            // ui_selNilReason
            // 
            this.ui_selNilReason.AutoSize = true;
            this.ui_selNilReason.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_selNilReason.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ui_selNilReason.FlatAppearance.BorderSize = 0;
            this.ui_selNilReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_selNilReason.Image = global::Aran.Queries.Common.Properties.Resources.letter_n_16;
            this.ui_selNilReason.Location = new System.Drawing.Point(81, 6);
            this.ui_selNilReason.Margin = new System.Windows.Forms.Padding(6);
            this.ui_selNilReason.Name = "ui_selNilReason";
            this.ui_selNilReason.Size = new System.Drawing.Size(22, 22);
            this.ui_selNilReason.TabIndex = 29;
            this.ui_selNilReason.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.ui_selNilReason, "Set NilReason");
            this.ui_selNilReason.UseVisualStyleBackColor = true;
            this.ui_selNilReason.Click += new System.EventHandler(this.SelNilReason_Click);
            // 
            // ui_infoButton
            // 
            this.ui_infoButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ui_infoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_infoButton.Image = global::Aran.Queries.Common.Properties.Resources.info_16;
            this.ui_infoButton.Location = new System.Drawing.Point(112, 3);
            this.ui_infoButton.Name = "ui_infoButton";
            this.ui_infoButton.Size = new System.Drawing.Size(28, 28);
            this.ui_infoButton.TabIndex = 30;
            this.toolTip1.SetToolTip(this.ui_infoButton, "Property Description");
            this.ui_infoButton.UseVisualStyleBackColor = true;
            this.ui_infoButton.Click += new System.EventHandler(this.Info_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.CustomFormat = "yyyy - MM - dd   HH:mm";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(159, 169);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(216, 20);
            this.dateTimePicker.TabIndex = 11;
            this.dateTimePicker.Visible = false;
            this.dateTimePicker.Enter += new System.EventHandler(this.dateTimePicker_Enter);
            this.dateTimePicker.Leave += new System.EventHandler(this.dateTimePicker_Leave);
            // 
            // coordPanel
            // 
            this.coordPanel.BackColor = System.Drawing.SystemColors.Window;
            this.coordPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.coordPanel.Controls.Add(this.labelCoordY);
            this.coordPanel.Controls.Add(this.coordYTB);
            this.coordPanel.Controls.Add(this.labelCoordX);
            this.coordPanel.Controls.Add(this.coordXTB);
            this.coordPanel.Location = new System.Drawing.Point(159, 193);
            this.coordPanel.Name = "coordPanel";
            this.coordPanel.Size = new System.Drawing.Size(214, 25);
            this.coordPanel.TabIndex = 19;
            this.coordPanel.Visible = false;
            // 
            // labelCoordY
            // 
            this.labelCoordY.BackColor = System.Drawing.SystemColors.Control;
            this.labelCoordY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCoordY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCoordY.Location = new System.Drawing.Point(106, 2);
            this.labelCoordY.Name = "labelCoordY";
            this.labelCoordY.Size = new System.Drawing.Size(20, 17);
            this.labelCoordY.TabIndex = 4;
            this.labelCoordY.Text = "y:";
            this.labelCoordY.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // coordYTB
            // 
            this.coordYTB.BackColor = System.Drawing.SystemColors.Window;
            this.coordYTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.coordYTB.Location = new System.Drawing.Point(130, 4);
            this.coordYTB.Name = "coordYTB";
            this.coordYTB.Size = new System.Drawing.Size(77, 13);
            this.coordYTB.TabIndex = 3;
            // 
            // labelCoordX
            // 
            this.labelCoordX.BackColor = System.Drawing.SystemColors.Control;
            this.labelCoordX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCoordX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCoordX.Location = new System.Drawing.Point(2, 2);
            this.labelCoordX.Name = "labelCoordX";
            this.labelCoordX.Size = new System.Drawing.Size(20, 17);
            this.labelCoordX.TabIndex = 2;
            this.labelCoordX.Text = "x:";
            this.labelCoordX.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // coordXTB
            // 
            this.coordXTB.BackColor = System.Drawing.SystemColors.Window;
            this.coordXTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.coordXTB.ForeColor = System.Drawing.SystemColors.WindowText;
            this.coordXTB.Location = new System.Drawing.Point(26, 4);
            this.coordXTB.Name = "coordXTB";
            this.coordXTB.Size = new System.Drawing.Size(77, 13);
            this.coordXTB.TabIndex = 0;
            // 
            // notePanel
            // 
            this.notePanel.Controls.Add(this.noteCB);
            this.notePanel.Controls.Add(this.noteTB);
            this.notePanel.Location = new System.Drawing.Point(159, 224);
            this.notePanel.Name = "notePanel";
            this.notePanel.Size = new System.Drawing.Size(216, 63);
            this.notePanel.TabIndex = 22;
            this.notePanel.Visible = false;
            // 
            // noteCB
            // 
            this.noteCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.noteCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noteCB.FormattingEnabled = true;
            this.noteCB.Location = new System.Drawing.Point(161, 19);
            this.noteCB.Name = "noteCB";
            this.noteCB.Size = new System.Drawing.Size(54, 21);
            this.noteCB.TabIndex = 2;
            // 
            // noteTB
            // 
            this.noteTB.AcceptsReturn = true;
            this.noteTB.AcceptsTab = true;
            this.noteTB.Location = new System.Drawing.Point(0, 2);
            this.noteTB.Multiline = true;
            this.noteTB.Name = "noteTB";
            this.noteTB.Size = new System.Drawing.Size(159, 58);
            this.noteTB.TabIndex = 1;
            // 
            // ui_geomPanel
            // 
            this.ui_geomPanel.Controls.Add(this.ui_showGeomLink);
            this.ui_geomPanel.Location = new System.Drawing.Point(159, 293);
            this.ui_geomPanel.Name = "ui_geomPanel";
            this.ui_geomPanel.Size = new System.Drawing.Size(215, 25);
            this.ui_geomPanel.TabIndex = 30;
            this.ui_geomPanel.Visible = false;
            // 
            // ui_showGeomLink
            // 
            this.ui_showGeomLink.AutoSize = true;
            this.ui_showGeomLink.Location = new System.Drawing.Point(6, 6);
            this.ui_showGeomLink.Name = "ui_showGeomLink";
            this.ui_showGeomLink.Size = new System.Drawing.Size(82, 13);
            this.ui_showGeomLink.TabIndex = 0;
            this.ui_showGeomLink.TabStop = true;
            this.ui_showGeomLink.Text = "Show Geometry";
            this.ui_showGeomLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ShowGeomLink_LinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.ui_clearButton);
            this.flowLayoutPanel1.Controls.Add(this.ui_warningPicBox);
            this.flowLayoutPanel1.Controls.Add(this.ui_selNilReason);
            this.flowLayoutPanel1.Controls.Add(this.ui_infoButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(381, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(143, 38);
            this.flowLayoutPanel1.TabIndex = 36;
            // 
            // ui_nilReasonCMS
            // 
            this.ui_nilReasonCMS.Name = "ui_nilReasonCMS";
            this.ui_nilReasonCMS.Size = new System.Drawing.Size(61, 4);
            this.ui_nilReasonCMS.Opening += new System.ComponentModel.CancelEventHandler(this.NilReasonCMS_Opening);
            // 
            // intTb
            // 
            this.intTb.IsNull = true;
            this.intTb.Location = new System.Drawing.Point(159, 83);
            this.intTb.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.intTb.Minimum = new decimal(new int[] {
            1215752192,
            23,
            0,
            -2147483648});
            this.intTb.Name = "intTb";
            this.intTb.Size = new System.Drawing.Size(216, 20);
            this.intTb.TabIndex = 12;
            this.intTb.Visible = false;
            // 
            // ui_coordinateControl
            // 
            this.ui_coordinateControl.Accuracy = 0;
            this.ui_coordinateControl.AutoSize = true;
            this.ui_coordinateControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_coordinateControl.DDAccuracy = 6;
            this.ui_coordinateControl.DMSAccuracy = 4;
            this.ui_coordinateControl.IsDD = true;
            this.ui_coordinateControl.Location = new System.Drawing.Point(159, 324);
            this.ui_coordinateControl.Name = "ui_coordinateControl";
            this.ui_coordinateControl.ReadOnly = false;
            this.ui_coordinateControl.Size = new System.Drawing.Size(293, 60);
            this.ui_coordinateControl.TabIndex = 34;
            // 
            // doubleNud
            // 
            this.doubleNud.DecimalPlaces = 4;
            this.doubleNud.IsNull = true;
            this.doubleNud.Location = new System.Drawing.Point(159, 57);
            this.doubleNud.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.doubleNud.Minimum = new decimal(new int[] {
            1215752192,
            23,
            0,
            -2147483648});
            this.doubleNud.Name = "doubleNud";
            this.doubleNud.Size = new System.Drawing.Size(216, 20);
            this.doubleNud.TabIndex = 3;
            this.doubleNud.Visible = false;
            // 
            // PropControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.coordPanel);
            this.Controls.Add(this.intTb);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.ui_coordinateControl);
            this.Controls.Add(this.notePanel);
            this.Controls.Add(this.stringTb);
            this.Controls.Add(this.propNameLabel);
            this.Controls.Add(this.ui_geomPanel);
            this.Controls.Add(this.uomPanel);
            this.Controls.Add(this.boolChB);
            this.Controls.Add(this.doubleNud);
            this.Controls.Add(this.enumCb);
            this.Name = "PropControl";
            this.Size = new System.Drawing.Size(578, 433);
            this.Load += new System.EventHandler(this.PropControl_Load);
            this.SizeChanged += new System.EventHandler(this.PropControl_SizeChanged);
            this.uomPanel.ResumeLayout(false);
            this.uomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uomValueNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_warningPicBox)).EndInit();
            this.coordPanel.ResumeLayout(false);
            this.coordPanel.PerformLayout();
            this.notePanel.ResumeLayout(false);
            this.notePanel.PerformLayout();
            this.ui_geomPanel.ResumeLayout(false);
            this.ui_geomPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intTb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doubleNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stringTb;
        private System.Windows.Forms.Label propNameLabel;
        private System.Windows.Forms.CheckBox boolChB;
        private NNumericUpDown doubleNud;
        private System.Windows.Forms.ComboBox enumCb;
        private System.Windows.Forms.Panel uomPanel;
        private System.Windows.Forms.ComboBox uomCb;
        private NNumericUpDown uomValueNud;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private NNumericUpDown intTb;
        private System.Windows.Forms.Panel coordPanel;
        private System.Windows.Forms.TextBox coordXTB;
        private System.Windows.Forms.Label labelCoordX;
        private System.Windows.Forms.Label labelCoordY;
        private System.Windows.Forms.TextBox coordYTB;
        private System.Windows.Forms.Panel notePanel;
        private System.Windows.Forms.ComboBox noteCB;
        private System.Windows.Forms.TextBox noteTB;
        private System.Windows.Forms.Button ui_clearButton;
        private System.Windows.Forms.PictureBox ui_warningPicBox;
        private System.Windows.Forms.Panel ui_geomPanel;
        private System.Windows.Forms.LinkLabel ui_showGeomLink;
        private CoordinateControl ui_coordinateControl;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button ui_selNilReason;
		private System.Windows.Forms.ContextMenuStrip ui_nilReasonCMS;
		private System.Windows.Forms.Button ui_infoButton;
    }
}
