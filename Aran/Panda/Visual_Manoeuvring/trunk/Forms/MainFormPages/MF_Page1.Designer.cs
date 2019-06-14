namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class MF_Page1
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
            this.lbl_AircraftCat = new System.Windows.Forms.Label();
            this.cmbBox_AircraftCat = new System.Windows.Forms.ComboBox();
            this.lstVw_RWY = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_ArrivalRadius = new System.Windows.Forms.Label();
            this.lbl_distanceSign2 = new System.Windows.Forms.Label();
            this.lbl_IAFDesignatedPoint = new System.Windows.Forms.Label();
            this.cmbBox_DesignatedPoint = new System.Windows.Forms.ComboBox();
            this.lbl_GuidanceFacility = new System.Windows.Forms.Label();
            this.cmbBox_GuidanceFacility = new System.Windows.Forms.ComboBox();
            this.cmbBox_RWYTHR = new System.Windows.Forms.ComboBox();
            this.lbl_RWYTHR = new System.Windows.Forms.Label();
            this.nmrcUpDown_ArrivalRadius = new System.Windows.Forms.NumericUpDown();
            this.lbl_NavaidType = new System.Windows.Forms.Label();
            this.lbl_CirclingAreaEntranceDirection = new System.Windows.Forms.Label();
            this.txtBox_CirclingAreaEntranceDirection = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.cmbBox_IAP = new System.Windows.Forms.ComboBox();
            this.lbl_IAP = new System.Windows.Forms.Label();
            this.lbl_initialPositionAdjust = new System.Windows.Forms.Label();
            this.nmrcUpDown_initialPositionAdjust = new System.Windows.Forms.NumericUpDown();
            this.lbl_distanceSign = new System.Windows.Forms.Label();
            this.lbl_initialPositionAdjustRange = new System.Windows.Forms.Label();
            this.lbl_initialPositionAltitude = new System.Windows.Forms.Label();
            this.txtBox_initialPositionAltitude = new System.Windows.Forms.TextBox();
            this.lbl_heightSign = new System.Windows.Forms.Label();
            this.lbl_DivergenceVF = new System.Windows.Forms.Label();
            this.cmbBox_DivergenceVF = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_minOCA = new System.Windows.Forms.TextBox();
            this.lbl_heightSign2 = new System.Windows.Forms.Label();
            this.nmrcUpDown_visibilityDistance = new System.Windows.Forms.NumericUpDown();
            this.lbl_kmSign2 = new System.Windows.Forms.Label();
            this.lbl_visibilityDistance = new System.Windows.Forms.Label();
            this.cmbBox_MAG_TRUE = new System.Windows.Forms.ComboBox();
            this.cmbBox_OCH_OCA = new System.Windows.Forms.ComboBox();
            this.lbl_message = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_ArrivalRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_initialPositionAdjust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_visibilityDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_AircraftCat
            // 
            this.lbl_AircraftCat.AutoSize = true;
            this.lbl_AircraftCat.Location = new System.Drawing.Point(134, 49);
            this.lbl_AircraftCat.Name = "lbl_AircraftCat";
            this.lbl_AircraftCat.Size = new System.Drawing.Size(52, 13);
            this.lbl_AircraftCat.TabIndex = 0;
            this.lbl_AircraftCat.Text = "Category:";
            // 
            // cmbBox_AircraftCat
            // 
            this.cmbBox_AircraftCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_AircraftCat.FormattingEnabled = true;
            this.cmbBox_AircraftCat.Location = new System.Drawing.Point(262, 46);
            this.cmbBox_AircraftCat.Name = "cmbBox_AircraftCat";
            this.cmbBox_AircraftCat.Size = new System.Drawing.Size(80, 21);
            this.cmbBox_AircraftCat.TabIndex = 1;
            this.cmbBox_AircraftCat.SelectedIndexChanged += new System.EventHandler(this.cmbBox_AircraftCat_SelectedIndexChanged);
            // 
            // lstVw_RWY
            // 
            this.lstVw_RWY.CheckBoxes = true;
            this.lstVw_RWY.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1});
            this.lstVw_RWY.FullRowSelect = true;
            this.lstVw_RWY.Location = new System.Drawing.Point(3, 46);
            this.lstVw_RWY.Name = "lstVw_RWY";
            this.lstVw_RWY.Size = new System.Drawing.Size(125, 145);
            this.lstVw_RWY.TabIndex = 45;
            this.lstVw_RWY.UseCompatibleStateImageBehavior = false;
            this.lstVw_RWY.View = System.Windows.Forms.View.Details;
            this.lstVw_RWY.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstVw_RWY_ItemChecked);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "RWY";
            this.ColumnHeader1.Width = 120;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 30);
            this.panel1.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Circling Area Construction";
            // 
            // lbl_ArrivalRadius
            // 
            this.lbl_ArrivalRadius.AutoSize = true;
            this.lbl_ArrivalRadius.Location = new System.Drawing.Point(3, 331);
            this.lbl_ArrivalRadius.Name = "lbl_ArrivalRadius";
            this.lbl_ArrivalRadius.Size = new System.Drawing.Size(70, 13);
            this.lbl_ArrivalRadius.TabIndex = 47;
            this.lbl_ArrivalRadius.Text = "Arrival radius:";
            this.lbl_ArrivalRadius.Visible = false;
            // 
            // lbl_distanceSign2
            // 
            this.lbl_distanceSign2.AutoSize = true;
            this.lbl_distanceSign2.Location = new System.Drawing.Point(198, 331);
            this.lbl_distanceSign2.Name = "lbl_distanceSign2";
            this.lbl_distanceSign2.Size = new System.Drawing.Size(21, 13);
            this.lbl_distanceSign2.TabIndex = 49;
            this.lbl_distanceSign2.Text = "km";
            this.lbl_distanceSign2.Visible = false;
            // 
            // lbl_IAFDesignatedPoint
            // 
            this.lbl_IAFDesignatedPoint.AutoSize = true;
            this.lbl_IAFDesignatedPoint.Location = new System.Drawing.Point(3, 363);
            this.lbl_IAFDesignatedPoint.Name = "lbl_IAFDesignatedPoint";
            this.lbl_IAFDesignatedPoint.Size = new System.Drawing.Size(78, 26);
            this.lbl_IAFDesignatedPoint.TabIndex = 50;
            this.lbl_IAFDesignatedPoint.Text = "IAF designated\r\npoint:";
            this.lbl_IAFDesignatedPoint.Visible = false;
            // 
            // cmbBox_DesignatedPoint
            // 
            this.cmbBox_DesignatedPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_DesignatedPoint.FormattingEnabled = true;
            this.cmbBox_DesignatedPoint.Location = new System.Drawing.Point(87, 368);
            this.cmbBox_DesignatedPoint.Name = "cmbBox_DesignatedPoint";
            this.cmbBox_DesignatedPoint.Size = new System.Drawing.Size(105, 21);
            this.cmbBox_DesignatedPoint.TabIndex = 51;
            this.cmbBox_DesignatedPoint.Visible = false;
            this.cmbBox_DesignatedPoint.SelectedIndexChanged += new System.EventHandler(this.cmbBox_DesignatedPoint_SelectedIndexChanged);
            // 
            // lbl_GuidanceFacility
            // 
            this.lbl_GuidanceFacility.AutoSize = true;
            this.lbl_GuidanceFacility.Location = new System.Drawing.Point(202, 371);
            this.lbl_GuidanceFacility.Name = "lbl_GuidanceFacility";
            this.lbl_GuidanceFacility.Size = new System.Drawing.Size(88, 13);
            this.lbl_GuidanceFacility.TabIndex = 52;
            this.lbl_GuidanceFacility.Text = "Guidance facility:";
            this.lbl_GuidanceFacility.Visible = false;
            // 
            // cmbBox_GuidanceFacility
            // 
            this.cmbBox_GuidanceFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_GuidanceFacility.FormattingEnabled = true;
            this.cmbBox_GuidanceFacility.Location = new System.Drawing.Point(296, 368);
            this.cmbBox_GuidanceFacility.Name = "cmbBox_GuidanceFacility";
            this.cmbBox_GuidanceFacility.Size = new System.Drawing.Size(73, 21);
            this.cmbBox_GuidanceFacility.TabIndex = 53;
            this.cmbBox_GuidanceFacility.Visible = false;
            this.cmbBox_GuidanceFacility.SelectedIndexChanged += new System.EventHandler(this.cmbBox_GuidanceFacility_SelectedIndexChanged);
            // 
            // cmbBox_RWYTHR
            // 
            this.cmbBox_RWYTHR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_RWYTHR.FormattingEnabled = true;
            this.cmbBox_RWYTHR.Location = new System.Drawing.Point(262, 82);
            this.cmbBox_RWYTHR.Name = "cmbBox_RWYTHR";
            this.cmbBox_RWYTHR.Size = new System.Drawing.Size(80, 21);
            this.cmbBox_RWYTHR.TabIndex = 55;
            this.cmbBox_RWYTHR.SelectedIndexChanged += new System.EventHandler(this.cmbBox_RWYTHR_SelectedIndexChanged);
            // 
            // lbl_RWYTHR
            // 
            this.lbl_RWYTHR.AutoSize = true;
            this.lbl_RWYTHR.Location = new System.Drawing.Point(134, 85);
            this.lbl_RWYTHR.Name = "lbl_RWYTHR";
            this.lbl_RWYTHR.Size = new System.Drawing.Size(125, 13);
            this.lbl_RWYTHR.TabIndex = 54;
            this.lbl_RWYTHR.Text = "Prescribed track to THR:";
            // 
            // nmrcUpDown_ArrivalRadius
            // 
            this.nmrcUpDown_ArrivalRadius.Location = new System.Drawing.Point(87, 329);
            this.nmrcUpDown_ArrivalRadius.Name = "nmrcUpDown_ArrivalRadius";
            this.nmrcUpDown_ArrivalRadius.Size = new System.Drawing.Size(105, 20);
            this.nmrcUpDown_ArrivalRadius.TabIndex = 56;
            this.nmrcUpDown_ArrivalRadius.Visible = false;
            this.nmrcUpDown_ArrivalRadius.ValueChanged += new System.EventHandler(this.nmrcUpDown_ArrivalRadius_ValueChanged);
            // 
            // lbl_NavaidType
            // 
            this.lbl_NavaidType.AutoSize = true;
            this.lbl_NavaidType.Location = new System.Drawing.Point(375, 371);
            this.lbl_NavaidType.Name = "lbl_NavaidType";
            this.lbl_NavaidType.Size = new System.Drawing.Size(51, 13);
            this.lbl_NavaidType.TabIndex = 508;
            this.lbl_NavaidType.Text = "NavType";
            this.lbl_NavaidType.Visible = false;
            // 
            // lbl_CirclingAreaEntranceDirection
            // 
            this.lbl_CirclingAreaEntranceDirection.AutoSize = true;
            this.lbl_CirclingAreaEntranceDirection.Location = new System.Drawing.Point(134, 169);
            this.lbl_CirclingAreaEntranceDirection.Name = "lbl_CirclingAreaEntranceDirection";
            this.lbl_CirclingAreaEntranceDirection.Size = new System.Drawing.Size(135, 13);
            this.lbl_CirclingAreaEntranceDirection.TabIndex = 509;
            this.lbl_CirclingAreaEntranceDirection.Text = "IAP final segment direction:";
            // 
            // txtBox_CirclingAreaEntranceDirection
            // 
            this.txtBox_CirclingAreaEntranceDirection.Location = new System.Drawing.Point(277, 166);
            this.txtBox_CirclingAreaEntranceDirection.Name = "txtBox_CirclingAreaEntranceDirection";
            this.txtBox_CirclingAreaEntranceDirection.ReadOnly = true;
            this.txtBox_CirclingAreaEntranceDirection.Size = new System.Drawing.Size(85, 20);
            this.txtBox_CirclingAreaEntranceDirection.TabIndex = 510;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(368, 169);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(11, 13);
            this.Label7.TabIndex = 511;
            this.Label7.Text = "°";
            // 
            // cmbBox_IAP
            // 
            this.cmbBox_IAP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_IAP.FormattingEnabled = true;
            this.cmbBox_IAP.Location = new System.Drawing.Point(262, 122);
            this.cmbBox_IAP.Name = "cmbBox_IAP";
            this.cmbBox_IAP.Size = new System.Drawing.Size(191, 21);
            this.cmbBox_IAP.TabIndex = 512;
            this.cmbBox_IAP.SelectedIndexChanged += new System.EventHandler(this.cmbBox_IAP_SelectedIndexChanged);
            // 
            // lbl_IAP
            // 
            this.lbl_IAP.AutoSize = true;
            this.lbl_IAP.Location = new System.Drawing.Point(134, 125);
            this.lbl_IAP.Name = "lbl_IAP";
            this.lbl_IAP.Size = new System.Drawing.Size(114, 13);
            this.lbl_IAP.TabIndex = 513;
            this.lbl_IAP.Text = "Preceding circling IAP:";
            // 
            // lbl_initialPositionAdjust
            // 
            this.lbl_initialPositionAdjust.AutoSize = true;
            this.lbl_initialPositionAdjust.Location = new System.Drawing.Point(225, 329);
            this.lbl_initialPositionAdjust.Name = "lbl_initialPositionAdjust";
            this.lbl_initialPositionAdjust.Size = new System.Drawing.Size(73, 13);
            this.lbl_initialPositionAdjust.TabIndex = 514;
            this.lbl_initialPositionAdjust.Text = "Initial position:";
            this.lbl_initialPositionAdjust.Visible = false;
            // 
            // nmrcUpDown_initialPositionAdjust
            // 
            this.nmrcUpDown_initialPositionAdjust.Location = new System.Drawing.Point(304, 327);
            this.nmrcUpDown_initialPositionAdjust.Name = "nmrcUpDown_initialPositionAdjust";
            this.nmrcUpDown_initialPositionAdjust.Size = new System.Drawing.Size(73, 20);
            this.nmrcUpDown_initialPositionAdjust.TabIndex = 515;
            this.nmrcUpDown_initialPositionAdjust.Visible = false;
            this.nmrcUpDown_initialPositionAdjust.ValueChanged += new System.EventHandler(this.nmrcUpDown_initialPositionAdjust_ValueChanged);
            // 
            // lbl_distanceSign
            // 
            this.lbl_distanceSign.AutoSize = true;
            this.lbl_distanceSign.Location = new System.Drawing.Point(383, 329);
            this.lbl_distanceSign.Name = "lbl_distanceSign";
            this.lbl_distanceSign.Size = new System.Drawing.Size(21, 13);
            this.lbl_distanceSign.TabIndex = 516;
            this.lbl_distanceSign.Text = "km";
            this.lbl_distanceSign.Visible = false;
            // 
            // lbl_initialPositionAdjustRange
            // 
            this.lbl_initialPositionAdjustRange.AutoSize = true;
            this.lbl_initialPositionAdjustRange.Location = new System.Drawing.Point(225, 342);
            this.lbl_initialPositionAdjustRange.Name = "lbl_initialPositionAdjustRange";
            this.lbl_initialPositionAdjustRange.Size = new System.Drawing.Size(46, 13);
            this.lbl_initialPositionAdjustRange.TabIndex = 517;
            this.lbl_initialPositionAdjustRange.Text = "(xxx-xxx)";
            this.lbl_initialPositionAdjustRange.Visible = false;
            // 
            // lbl_initialPositionAltitude
            // 
            this.lbl_initialPositionAltitude.AutoSize = true;
            this.lbl_initialPositionAltitude.Location = new System.Drawing.Point(383, 342);
            this.lbl_initialPositionAltitude.Name = "lbl_initialPositionAltitude";
            this.lbl_initialPositionAltitude.Size = new System.Drawing.Size(73, 26);
            this.lbl_initialPositionAltitude.TabIndex = 518;
            this.lbl_initialPositionAltitude.Text = "Initial position \r\naltitude:";
            this.lbl_initialPositionAltitude.Visible = false;
            // 
            // txtBox_initialPositionAltitude
            // 
            this.txtBox_initialPositionAltitude.Location = new System.Drawing.Point(462, 347);
            this.txtBox_initialPositionAltitude.Name = "txtBox_initialPositionAltitude";
            this.txtBox_initialPositionAltitude.ReadOnly = true;
            this.txtBox_initialPositionAltitude.Size = new System.Drawing.Size(71, 20);
            this.txtBox_initialPositionAltitude.TabIndex = 519;
            this.txtBox_initialPositionAltitude.Visible = false;
            // 
            // lbl_heightSign
            // 
            this.lbl_heightSign.AutoSize = true;
            this.lbl_heightSign.Location = new System.Drawing.Point(539, 350);
            this.lbl_heightSign.Name = "lbl_heightSign";
            this.lbl_heightSign.Size = new System.Drawing.Size(15, 13);
            this.lbl_heightSign.TabIndex = 520;
            this.lbl_heightSign.Text = "m";
            this.lbl_heightSign.Visible = false;
            // 
            // lbl_DivergenceVF
            // 
            this.lbl_DivergenceVF.AutoSize = true;
            this.lbl_DivergenceVF.Location = new System.Drawing.Point(134, 235);
            this.lbl_DivergenceVF.Name = "lbl_DivergenceVF";
            this.lbl_DivergenceVF.Size = new System.Drawing.Size(83, 26);
            this.lbl_DivergenceVF.TabIndex = 521;
            this.lbl_DivergenceVF.Text = "Diverging point\r\ndefined with VF:";
            // 
            // cmbBox_DivergenceVF
            // 
            this.cmbBox_DivergenceVF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_DivergenceVF.FormattingEnabled = true;
            this.cmbBox_DivergenceVF.Location = new System.Drawing.Point(262, 245);
            this.cmbBox_DivergenceVF.Name = "cmbBox_DivergenceVF";
            this.cmbBox_DivergenceVF.Size = new System.Drawing.Size(100, 21);
            this.cmbBox_DivergenceVF.TabIndex = 522;
            this.cmbBox_DivergenceVF.SelectedIndexChanged += new System.EventHandler(this.cmbBox_DivergenceVF_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 26);
            this.label2.TabIndex = 523;
            this.label2.Text = "Lower limit \r\nfor OCA/OCH:";
            // 
            // txtBox_minOCA
            // 
            this.txtBox_minOCA.Location = new System.Drawing.Point(262, 289);
            this.txtBox_minOCA.Name = "txtBox_minOCA";
            this.txtBox_minOCA.ReadOnly = true;
            this.txtBox_minOCA.Size = new System.Drawing.Size(100, 20);
            this.txtBox_minOCA.TabIndex = 524;
            // 
            // lbl_heightSign2
            // 
            this.lbl_heightSign2.AutoSize = true;
            this.lbl_heightSign2.Location = new System.Drawing.Point(368, 292);
            this.lbl_heightSign2.Name = "lbl_heightSign2";
            this.lbl_heightSign2.Size = new System.Drawing.Size(15, 13);
            this.lbl_heightSign2.TabIndex = 525;
            this.lbl_heightSign2.Text = "m";
            // 
            // nmrcUpDown_visibilityDistance
            // 
            this.nmrcUpDown_visibilityDistance.Location = new System.Drawing.Point(262, 206);
            this.nmrcUpDown_visibilityDistance.Name = "nmrcUpDown_visibilityDistance";
            this.nmrcUpDown_visibilityDistance.Size = new System.Drawing.Size(100, 20);
            this.nmrcUpDown_visibilityDistance.TabIndex = 528;
            this.nmrcUpDown_visibilityDistance.ValueChanged += new System.EventHandler(this.nmrcUpDown_visibilityDistance_ValueChanged);
            // 
            // lbl_kmSign2
            // 
            this.lbl_kmSign2.AutoSize = true;
            this.lbl_kmSign2.Location = new System.Drawing.Point(374, 208);
            this.lbl_kmSign2.Name = "lbl_kmSign2";
            this.lbl_kmSign2.Size = new System.Drawing.Size(21, 13);
            this.lbl_kmSign2.TabIndex = 527;
            this.lbl_kmSign2.Text = "km";
            // 
            // lbl_visibilityDistance
            // 
            this.lbl_visibilityDistance.AutoSize = true;
            this.lbl_visibilityDistance.Location = new System.Drawing.Point(134, 208);
            this.lbl_visibilityDistance.Name = "lbl_visibilityDistance";
            this.lbl_visibilityDistance.Size = new System.Drawing.Size(87, 13);
            this.lbl_visibilityDistance.TabIndex = 526;
            this.lbl_visibilityDistance.Text = "Min. AD Visibility:";
            // 
            // cmbBox_MAG_TRUE
            // 
            this.cmbBox_MAG_TRUE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_MAG_TRUE.FormattingEnabled = true;
            this.cmbBox_MAG_TRUE.Location = new System.Drawing.Point(403, 166);
            this.cmbBox_MAG_TRUE.Name = "cmbBox_MAG_TRUE";
            this.cmbBox_MAG_TRUE.Size = new System.Drawing.Size(83, 21);
            this.cmbBox_MAG_TRUE.TabIndex = 529;
            this.cmbBox_MAG_TRUE.SelectedIndexChanged += new System.EventHandler(this.cmbBox_MAG_TRUE_SelectedIndexChanged);
            // 
            // cmbBox_OCH_OCA
            // 
            this.cmbBox_OCH_OCA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_OCH_OCA.FormattingEnabled = true;
            this.cmbBox_OCH_OCA.Location = new System.Drawing.Point(403, 289);
            this.cmbBox_OCH_OCA.Name = "cmbBox_OCH_OCA";
            this.cmbBox_OCH_OCA.Size = new System.Drawing.Size(83, 21);
            this.cmbBox_OCH_OCA.TabIndex = 530;
            this.cmbBox_OCH_OCA.SelectedIndexChanged += new System.EventHandler(this.cmbBox_OCH_OCA_SelectedIndexChanged);
            // 
            // lbl_message
            // 
            this.lbl_message.AutoSize = true;
            this.lbl_message.ForeColor = System.Drawing.Color.Red;
            this.lbl_message.Location = new System.Drawing.Point(374, 248);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(173, 26);
            this.lbl_message.TabIndex = 531;
            this.lbl_message.Text = "There should be at least one VF \r\nin the diverging polygon to proceed";
            // 
            // MF_Page1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.cmbBox_OCH_OCA);
            this.Controls.Add(this.cmbBox_MAG_TRUE);
            this.Controls.Add(this.nmrcUpDown_visibilityDistance);
            this.Controls.Add(this.lbl_kmSign2);
            this.Controls.Add(this.lbl_visibilityDistance);
            this.Controls.Add(this.lbl_heightSign2);
            this.Controls.Add(this.txtBox_minOCA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBox_DivergenceVF);
            this.Controls.Add(this.lbl_DivergenceVF);
            this.Controls.Add(this.lbl_heightSign);
            this.Controls.Add(this.txtBox_initialPositionAltitude);
            this.Controls.Add(this.lbl_initialPositionAltitude);
            this.Controls.Add(this.lbl_initialPositionAdjustRange);
            this.Controls.Add(this.lbl_distanceSign);
            this.Controls.Add(this.nmrcUpDown_initialPositionAdjust);
            this.Controls.Add(this.lbl_initialPositionAdjust);
            this.Controls.Add(this.lbl_IAP);
            this.Controls.Add(this.cmbBox_IAP);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.txtBox_CirclingAreaEntranceDirection);
            this.Controls.Add(this.lbl_CirclingAreaEntranceDirection);
            this.Controls.Add(this.lbl_NavaidType);
            this.Controls.Add(this.nmrcUpDown_ArrivalRadius);
            this.Controls.Add(this.cmbBox_RWYTHR);
            this.Controls.Add(this.lbl_RWYTHR);
            this.Controls.Add(this.cmbBox_GuidanceFacility);
            this.Controls.Add(this.lbl_GuidanceFacility);
            this.Controls.Add(this.cmbBox_DesignatedPoint);
            this.Controls.Add(this.lbl_IAFDesignatedPoint);
            this.Controls.Add(this.lbl_distanceSign2);
            this.Controls.Add(this.lbl_ArrivalRadius);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstVw_RWY);
            this.Controls.Add(this.cmbBox_AircraftCat);
            this.Controls.Add(this.lbl_AircraftCat);
            this.Name = "MF_Page1";
            this.Size = new System.Drawing.Size(560, 400);
            this.Load += new System.EventHandler(this.MF_Page1_Load);
            this.VisibleChanged += new System.EventHandler(this.MF_Page1_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_ArrivalRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_initialPositionAdjust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_visibilityDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_AircraftCat;
        private System.Windows.Forms.ComboBox cmbBox_AircraftCat;
        internal System.Windows.Forms.ListView lstVw_RWY;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_ArrivalRadius;
        private System.Windows.Forms.Label lbl_distanceSign2;
        private System.Windows.Forms.Label lbl_IAFDesignatedPoint;
        private System.Windows.Forms.ComboBox cmbBox_DesignatedPoint;
        private System.Windows.Forms.Label lbl_GuidanceFacility;
        private System.Windows.Forms.ComboBox cmbBox_GuidanceFacility;
        private System.Windows.Forms.ComboBox cmbBox_RWYTHR;
        private System.Windows.Forms.Label lbl_RWYTHR;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_ArrivalRadius;
        internal System.Windows.Forms.Label lbl_NavaidType;
        private System.Windows.Forms.Label lbl_CirclingAreaEntranceDirection;
        private System.Windows.Forms.TextBox txtBox_CirclingAreaEntranceDirection;
        internal System.Windows.Forms.Label Label7;
        private System.Windows.Forms.ComboBox cmbBox_IAP;
        private System.Windows.Forms.Label lbl_IAP;
        private System.Windows.Forms.Label lbl_initialPositionAdjust;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_initialPositionAdjust;
        private System.Windows.Forms.Label lbl_distanceSign;
        private System.Windows.Forms.Label lbl_initialPositionAdjustRange;
        private System.Windows.Forms.Label lbl_initialPositionAltitude;
        private System.Windows.Forms.TextBox txtBox_initialPositionAltitude;
        private System.Windows.Forms.Label lbl_heightSign;
        private System.Windows.Forms.Label lbl_DivergenceVF;
        private System.Windows.Forms.ComboBox cmbBox_DivergenceVF;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBox_minOCA;
        private System.Windows.Forms.Label lbl_heightSign2;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_visibilityDistance;
        private System.Windows.Forms.Label lbl_kmSign2;
        private System.Windows.Forms.Label lbl_visibilityDistance;
        private System.Windows.Forms.ComboBox cmbBox_MAG_TRUE;
        private System.Windows.Forms.ComboBox cmbBox_OCH_OCA;
        private System.Windows.Forms.Label lbl_message;
    }
}
