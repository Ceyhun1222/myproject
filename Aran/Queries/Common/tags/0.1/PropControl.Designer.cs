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
            this.clearButton = new System.Windows.Forms.Button();
            this.ui_warningPicBox = new System.Windows.Forms.PictureBox();
            this.lineGrb = new System.Windows.Forms.GroupBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.coordPanel = new System.Windows.Forms.Panel();
            this.labelCoordY = new System.Windows.Forms.Label();
            this.coordYTB = new System.Windows.Forms.TextBox();
            this.labelCoordX = new System.Windows.Forms.Label();
            this.coordXTB = new System.Windows.Forms.TextBox();
            this.notePanel = new System.Windows.Forms.Panel();
            this.noteCB = new System.Windows.Forms.ComboBox();
            this.noteTB = new System.Windows.Forms.TextBox();
            this.intTb = new Aran.Queries.Common.NNumericUpDown();
            this.doubleNud = new Aran.Queries.Common.NNumericUpDown();
            this.uomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uomValueNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_warningPicBox)).BeginInit();
            this.coordPanel.SuspendLayout();
            this.notePanel.SuspendLayout();
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
            this.uomValueNud.DecimalPlaces = 4;
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
            // clearButton
            // 
            this.clearButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Image = global::Aran.Queries.Common.Properties.Resources.edit_clear;
            this.clearButton.Location = new System.Drawing.Point(384, 6);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(28, 28);
            this.clearButton.TabIndex = 26;
            this.toolTip1.SetToolTip(this.clearButton, "Set <NULL>");
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Visible = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // ui_warningPicBox
            // 
            this.ui_warningPicBox.Image = global::Aran.Queries.Common.Properties.Resources.warning_icon;
            this.ui_warningPicBox.Location = new System.Drawing.Point(418, 3);
            this.ui_warningPicBox.Name = "ui_warningPicBox";
            this.ui_warningPicBox.Size = new System.Drawing.Size(35, 32);
            this.ui_warningPicBox.TabIndex = 28;
            this.ui_warningPicBox.TabStop = false;
            this.toolTip1.SetToolTip(this.ui_warningPicBox, "Warning");
            this.ui_warningPicBox.Visible = false;
            // 
            // lineGrb
            // 
            this.lineGrb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lineGrb.Location = new System.Drawing.Point(0, 288);
            this.lineGrb.Name = "lineGrb";
            this.lineGrb.Size = new System.Drawing.Size(463, 2);
            this.lineGrb.TabIndex = 8;
            this.lineGrb.TabStop = false;
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
            this.Controls.Add(this.coordPanel);
            this.Controls.Add(this.intTb);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.ui_warningPicBox);
            this.Controls.Add(this.notePanel);
            this.Controls.Add(this.lineGrb);
            this.Controls.Add(this.stringTb);
            this.Controls.Add(this.propNameLabel);
            this.Controls.Add(this.uomPanel);
            this.Controls.Add(this.boolChB);
            this.Controls.Add(this.doubleNud);
            this.Controls.Add(this.enumCb);
            this.Name = "PropControl";
            this.Size = new System.Drawing.Size(463, 290);
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
        private System.Windows.Forms.GroupBox lineGrb;
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
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.PictureBox ui_warningPicBox;
    }
}
