using System.Windows.Forms;
using Holding.Models;

namespace Holding.Forms
{
    partial class frmHoldingMain
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
                panel1.Controls.Clear();
                foreach (Control c in panel1.Controls)
                    c.Dispose();

                panel2.Controls.Clear();
                foreach (Control c in panel2.Controls)
                    c.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHoldingMain));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnNext = new System.Windows.Forms.Button();
            this.modelWizardChangeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPrevius = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dGridNavaid = new System.Windows.Forms.DataGridView();
            this.checkedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.designatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistanceNavaidToFix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CourseNavaidToFix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holdingNavListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.holdingNavOperationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ckbDrawDME = new System.Windows.Forms.CheckBox();
            this.grpDmeCovType = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rdbTwoDmeCoverage = new System.Windows.Forms.RadioButton();
            this.grpTime = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.modelProcedureTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.udWD = new System.Windows.Forms.NumericUpDown();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblWd = new System.Windows.Forms.Label();
            this.cmbTimeWd = new System.Windows.Forms.ComboBox();
            this.distanceTypeListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpIas = new System.Windows.Forms.GroupBox();
            this.grpIasMinMax = new System.Windows.Forms.GroupBox();
            this.lblIasType = new System.Windows.Forms.Label();
            this.lblIas = new System.Windows.Forms.Label();
            this.udIas = new System.Windows.Forms.NumericUpDown();
            this.modelAreaParamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rdbTrbCondition = new System.Windows.Forms.RadioButton();
            this.rdbNrmlCondition = new System.Windows.Forms.RadioButton();
            this.cmbAircraftCategory = new System.Windows.Forms.ComboBox();
            this.aircraftCategoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblAircraftCategory = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.rdbOmnidirectional = new System.Windows.Forms.RadioButton();
            this.rdbDirect = new System.Windows.Forms.RadioButton();
            this.grpAltitude = new System.Windows.Forms.GroupBox();
            this.uDRadial = new System.Windows.Forms.NumericUpDown();
            this.lblRadial = new System.Windows.Forms.Label();
            this.lblRadialCap = new System.Windows.Forms.Label();
            this.lblMocCap = new System.Windows.Forms.Label();
            this.grpbTurn = new System.Windows.Forms.GroupBox();
            this.rdbtLeft = new System.Windows.Forms.RadioButton();
            this.rdbRight = new System.Windows.Forms.RadioButton();
            this.lblMOC = new System.Windows.Forms.Label();
            this.lblAltitudeCap = new System.Windows.Forms.Label();
            this.cmbObstacle = new System.Windows.Forms.ComboBox();
            this.mocListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.udAltitude = new System.Windows.Forms.NumericUpDown();
            this.lblAltitudeUnit = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.modelPointChoiseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnHoldigArea = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.iChangedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.validationClassBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.modelPBNBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpHldgFunction = new System.Windows.Forms.GroupBox();
            this.rdbRNPHolding = new System.Windows.Forms.RadioButton();
            this.rdbWithoutHldgFunc = new System.Windows.Forms.RadioButton();
            this.rdbWithHldgFunction = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pointPicker1 = new ChoosePointNS.PointPicker();
            this.lblNavaid = new System.Windows.Forms.Label();
            this.lblWayPointChoise = new System.Windows.Forms.Label();
            this.cmbPointList = new System.Windows.Forms.ComboBox();
            this.pointListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblDesignatedPt = new System.Windows.Forms.Label();
            this.cmbNavaid = new System.Windows.Forms.ComboBox();
            this.navaidListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbWayPointChoise = new System.Windows.Forms.ComboBox();
            this.significantListBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.grpFlightCondition = new System.Windows.Forms.GroupBox();
            this.lblPBN = new System.Windows.Forms.Label();
            this.lblFlightReciever = new System.Windows.Forms.Label();
            this.cmbPBN = new System.Windows.Forms.ComboBox();
            this.pbnConditionListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbFlightReciever = new System.Windows.Forms.ComboBox();
            this.recieverListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblFlightPhase = new System.Windows.Forms.Label();
            this.cmbFlightPhases = new System.Windows.Forms.ComboBox();
            this.phaseListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.significantListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.bussinesLogicBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bussinesLogicBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnInfo = new System.Windows.Forms.Button();
            this.drawSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelWizardChangeBindingSource)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGridNavaid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingNavListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingNavOperationBindingSource)).BeginInit();
            this.grpDmeCovType.SuspendLayout();
            this.grpTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelProcedureTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceTypeListBindingSource)).BeginInit();
            this.grpIas.SuspendLayout();
            this.grpIasMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udIas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelAreaParamsBindingSource)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aircraftCategoriesBindingSource)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.grpAltitude.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDRadial)).BeginInit();
            this.grpbTurn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mocListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAltitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPointChoiseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iChangedBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.validationClassBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPBNBindingSource)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpHldgFunction.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pointListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navaidListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.significantListBindingSource1)).BeginInit();
            this.grpFlightCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnConditionListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recieverListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.significantListBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bussinesLogicBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bussinesLogicBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(150, 150);
            this.toolStripContainer1.Location = new System.Drawing.Point(62, 413);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(150, 175);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Enabled = false;
            // 
            // btnNext
            // 
            this.btnNext.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelWizardChangeBindingSource, "NextAvailable", true));
            this.btnNext.Image = global::Aran.Panda.Rnav.Holding.Properties.Resources.Forward;
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(228, 423);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(81, 24);
            this.btnNext.TabIndex = 24;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // modelWizardChangeBindingSource
            // 
            this.modelWizardChangeBindingSource.DataSource = typeof(ModelWizardChange);
            // 
            // btnPrevius
            // 
            this.btnPrevius.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelWizardChangeBindingSource, "PreviusAvailable", true));
            this.btnPrevius.Image = global::Aran.Panda.Rnav.Holding.Properties.Resources.Backward;
            this.btnPrevius.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrevius.Location = new System.Drawing.Point(141, 423);
            this.btnPrevius.Name = "btnPrevius";
            this.btnPrevius.Size = new System.Drawing.Size(81, 24);
            this.btnPrevius.TabIndex = 25;
            this.btnPrevius.Text = "Back";
            this.btnPrevius.UseVisualStyleBackColor = true;
            this.btnPrevius.Click += new System.EventHandler(this.btnPrevius_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(580, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(574, 392);
            this.panel2.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.grpTime);
            this.groupBox3.Controls.Add(this.grpIas);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.grpAltitude);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(564, 410);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Area Parametrs";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dGridNavaid);
            this.groupBox2.Controls.Add(this.ckbDrawDME);
            this.groupBox2.Controls.Add(this.grpDmeCovType);
            this.groupBox2.Location = new System.Drawing.Point(13, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 167);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Navaids";
            // 
            // dGridNavaid
            // 
            this.dGridNavaid.AllowUserToAddRows = false;
            this.dGridNavaid.AllowUserToDeleteRows = false;
            this.dGridNavaid.AutoGenerateColumns = false;
            this.dGridNavaid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dGridNavaid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGridNavaid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGridNavaid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGridNavaid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkedDataGridViewCheckBoxColumn,
            this.designatorDataGridViewTextBoxColumn,
            this.DistanceNavaidToFix,
            this.CourseNavaidToFix});
            this.dGridNavaid.DataSource = this.holdingNavListBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGridNavaid.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGridNavaid.Dock = System.Windows.Forms.DockStyle.Left;
            this.dGridNavaid.Location = new System.Drawing.Point(3, 16);
            this.dGridNavaid.Name = "dGridNavaid";
            this.dGridNavaid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGridNavaid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dGridNavaid.RowHeadersVisible = false;
            this.dGridNavaid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dGridNavaid.Size = new System.Drawing.Size(256, 148);
            this.dGridNavaid.TabIndex = 28;
            this.dGridNavaid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // checkedDataGridViewCheckBoxColumn
            // 
            this.checkedDataGridViewCheckBoxColumn.DataPropertyName = "Checked";
            this.checkedDataGridViewCheckBoxColumn.HeaderText = "";
            this.checkedDataGridViewCheckBoxColumn.Name = "checkedDataGridViewCheckBoxColumn";
            this.checkedDataGridViewCheckBoxColumn.Width = 18;
            // 
            // designatorDataGridViewTextBoxColumn
            // 
            this.designatorDataGridViewTextBoxColumn.DataPropertyName = "Designator";
            this.designatorDataGridViewTextBoxColumn.HeaderText = "Designator";
            this.designatorDataGridViewTextBoxColumn.Name = "designatorDataGridViewTextBoxColumn";
            this.designatorDataGridViewTextBoxColumn.Width = 81;
            // 
            // DistanceNavaidToFix
            // 
            this.DistanceNavaidToFix.DataPropertyName = "DistanceNavaidToFix";
            this.DistanceNavaidToFix.HeaderText = "Distance";
            this.DistanceNavaidToFix.Name = "DistanceNavaidToFix";
            this.DistanceNavaidToFix.Width = 74;
            // 
            // CourseNavaidToFix
            // 
            this.CourseNavaidToFix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CourseNavaidToFix.DataPropertyName = "CourseNavaidToFix";
            this.CourseNavaidToFix.HeaderText = "Azimuth";
            this.CourseNavaidToFix.Name = "CourseNavaidToFix";
            this.CourseNavaidToFix.Width = 69;
            // 
            // holdingNavListBindingSource
            // 
            this.holdingNavListBindingSource.DataMember = "HoldingNavList";
            this.holdingNavListBindingSource.DataSource = this.holdingNavOperationBindingSource;
            // 
            // holdingNavOperationBindingSource
            // 
            this.holdingNavOperationBindingSource.DataSource = typeof(Holding.HoldingNavOperation);
            // 
            // ckbDrawDME
            // 
            this.ckbDrawDME.AutoSize = true;
            this.ckbDrawDME.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.holdingNavOperationBindingSource, "DmeCoverageIsEnabled", true));
            this.ckbDrawDME.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbDrawDME.Location = new System.Drawing.Point(272, 136);
            this.ckbDrawDME.Name = "ckbDrawDME";
            this.ckbDrawDME.Size = new System.Drawing.Size(102, 17);
            this.ckbDrawDME.TabIndex = 29;
            this.ckbDrawDME.Text = "Draw DME-DME";
            this.ckbDrawDME.UseVisualStyleBackColor = true;
            this.ckbDrawDME.CheckedChanged += new System.EventHandler(this.ckbDrawDME_CheckedChanged);
            // 
            // grpDmeCovType
            // 
            this.grpDmeCovType.Controls.Add(this.radioButton2);
            this.grpDmeCovType.Controls.Add(this.rdbTwoDmeCoverage);
            this.grpDmeCovType.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.holdingNavOperationBindingSource, "DmeCoverageChooseIsEnabled", true));
            this.grpDmeCovType.Location = new System.Drawing.Point(272, 19);
            this.grpDmeCovType.Name = "grpDmeCovType";
            this.grpDmeCovType.Size = new System.Drawing.Size(101, 87);
            this.grpDmeCovType.TabIndex = 27;
            this.grpDmeCovType.TabStop = false;
            this.grpDmeCovType.Text = "DME Coverage";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.holdingNavOperationBindingSource, "ThreeDmeCovTypeIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButton2.Location = new System.Drawing.Point(10, 64);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "3 DME";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // rdbTwoDmeCoverage
            // 
            this.rdbTwoDmeCoverage.AutoSize = true;
            this.rdbTwoDmeCoverage.Checked = true;
            this.rdbTwoDmeCoverage.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.holdingNavOperationBindingSource, "TwoDmeCovTypeIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbTwoDmeCoverage.Location = new System.Drawing.Point(10, 31);
            this.rdbTwoDmeCoverage.Name = "rdbTwoDmeCoverage";
            this.rdbTwoDmeCoverage.Size = new System.Drawing.Size(58, 17);
            this.rdbTwoDmeCoverage.TabIndex = 0;
            this.rdbTwoDmeCoverage.TabStop = true;
            this.rdbTwoDmeCoverage.Text = "2 DME";
            this.rdbTwoDmeCoverage.UseVisualStyleBackColor = true;
            // 
            // grpTime
            // 
            this.grpTime.Controls.Add(this.numericUpDown1);
            this.grpTime.Controls.Add(this.udWD);
            this.grpTime.Controls.Add(this.lblTime);
            this.grpTime.Controls.Add(this.lblWd);
            this.grpTime.Controls.Add(this.cmbTimeWd);
            this.grpTime.Location = new System.Drawing.Point(360, 20);
            this.grpTime.Name = "grpTime";
            this.grpTime.Size = new System.Drawing.Size(197, 111);
            this.grpTime.TabIndex = 12;
            this.grpTime.TabStop = false;
            this.grpTime.Text = "Define Outbound Leg";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.modelProcedureTypeBindingSource, "Time", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelProcedureTypeBindingSource, "TimeEnabled", true));
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", this.modelProcedureTypeBindingSource, "MaxTime", true));
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Minimum", this.modelProcedureTypeBindingSource, "MinTime", true));
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(134, 51);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown1.TabIndex = 17;
            this.numericUpDown1.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // modelProcedureTypeBindingSource
            // 
            this.modelProcedureTypeBindingSource.DataSource = typeof(ModelProcedureType);
            // 
            // udWD
            // 
            this.udWD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.modelProcedureTypeBindingSource, "WD", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udWD.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelProcedureTypeBindingSource, "WDEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udWD.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", this.modelProcedureTypeBindingSource, "MaxWD", true));
            this.udWD.DataBindings.Add(new System.Windows.Forms.Binding("Minimum", this.modelProcedureTypeBindingSource, "MinWd", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udWD.Location = new System.Drawing.Point(134, 19);
            this.udWD.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.udWD.Name = "udWD";
            this.udWD.Size = new System.Drawing.Size(50, 20);
            this.udWD.TabIndex = 16;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(81, 53);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(36, 13);
            this.lblTime.TabIndex = 15;
            this.lblTime.Text = "Time :";
            // 
            // lblWd
            // 
            this.lblWd.AutoSize = true;
            this.lblWd.Location = new System.Drawing.Point(81, 22);
            this.lblWd.Name = "lblWd";
            this.lblWd.Size = new System.Drawing.Size(50, 13);
            this.lblWd.TabIndex = 14;
            this.lblWd.Text = "Wd (km):";
            // 
            // cmbTimeWd
            // 
            this.cmbTimeWd.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelProcedureTypeBindingSource, "CurDistanceType", true));
            this.cmbTimeWd.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelProcedureTypeBindingSource, "CurDistanceType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbTimeWd.DataSource = this.distanceTypeListBindingSource;
            this.cmbTimeWd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeWd.FormattingEnabled = true;
            this.cmbTimeWd.Location = new System.Drawing.Point(8, 23);
            this.cmbTimeWd.Name = "cmbTimeWd";
            this.cmbTimeWd.Size = new System.Drawing.Size(54, 21);
            this.cmbTimeWd.TabIndex = 13;
            // 
            // distanceTypeListBindingSource
            // 
            this.distanceTypeListBindingSource.DataMember = "DistanceTypeList";
            this.distanceTypeListBindingSource.DataSource = this.modelProcedureTypeBindingSource;
            // 
            // grpIas
            // 
            this.grpIas.Controls.Add(this.grpIasMinMax);
            this.grpIas.Controls.Add(this.groupBox8);
            this.grpIas.Controls.Add(this.cmbAircraftCategory);
            this.grpIas.Controls.Add(this.lblAircraftCategory);
            this.grpIas.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpIas.Location = new System.Drawing.Point(12, 140);
            this.grpIas.Name = "grpIas";
            this.grpIas.Size = new System.Drawing.Size(559, 82);
            this.grpIas.TabIndex = 6;
            this.grpIas.TabStop = false;
            this.grpIas.Text = "Selection Aircraft Category and Assigned IAS";
            // 
            // grpIasMinMax
            // 
            this.grpIasMinMax.Controls.Add(this.lblIasType);
            this.grpIasMinMax.Controls.Add(this.lblIas);
            this.grpIasMinMax.Controls.Add(this.udIas);
            this.grpIasMinMax.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.modelAreaParamsBindingSource, "MinMaxIas", true));
            this.grpIasMinMax.Location = new System.Drawing.Point(159, 19);
            this.grpIasMinMax.Name = "grpIasMinMax";
            this.grpIasMinMax.Size = new System.Drawing.Size(142, 49);
            this.grpIasMinMax.TabIndex = 25;
            this.grpIasMinMax.TabStop = false;
            this.grpIasMinMax.Text = " / ";
            // 
            // lblIasType
            // 
            this.lblIasType.AutoSize = true;
            this.lblIasType.Location = new System.Drawing.Point(103, 23);
            this.lblIasType.Name = "lblIasType";
            this.lblIasType.Size = new System.Drawing.Size(32, 13);
            this.lblIasType.TabIndex = 8;
            this.lblIasType.Text = "km/h";
            // 
            // lblIas
            // 
            this.lblIas.AutoSize = true;
            this.lblIas.Location = new System.Drawing.Point(6, 23);
            this.lblIas.Name = "lblIas";
            this.lblIas.Size = new System.Drawing.Size(30, 13);
            this.lblIas.TabIndex = 4;
            this.lblIas.Text = "IAS :";
            // 
            // udIas
            // 
            this.udIas.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.modelAreaParamsBindingSource, "Ias", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udIas.Location = new System.Drawing.Point(39, 21);
            this.udIas.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udIas.Name = "udIas";
            this.udIas.Size = new System.Drawing.Size(58, 20);
            this.udIas.TabIndex = 5;
            this.udIas.ValueChanged += new System.EventHandler(this.udIas_ValueChanged);
            // 
            // modelAreaParamsBindingSource
            // 
            this.modelAreaParamsBindingSource.DataSource = typeof(Holding.Models.ModelAreaParams);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radioButton1);
            this.groupBox8.Controls.Add(this.rdbTrbCondition);
            this.groupBox8.Controls.Add(this.rdbNrmlCondition);
            this.groupBox8.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelAreaParamsBindingSource, "IsEnroute", true));
            this.groupBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox8.Location = new System.Drawing.Point(307, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(246, 49);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Conditions";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelAreaParamsBindingSource, "InitialApproachCondition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButton1.Location = new System.Drawing.Point(144, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(98, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Initial Approach";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rdbTrbCondition
            // 
            this.rdbTrbCondition.AutoSize = true;
            this.rdbTrbCondition.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelAreaParamsBindingSource, "TurboCondition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbTrbCondition.Location = new System.Drawing.Point(67, 21);
            this.rdbTrbCondition.Name = "rdbTrbCondition";
            this.rdbTrbCondition.Size = new System.Drawing.Size(79, 17);
            this.rdbTrbCondition.TabIndex = 1;
            this.rdbTrbCondition.TabStop = true;
            this.rdbTrbCondition.Text = "Turbulence";
            this.rdbTrbCondition.UseVisualStyleBackColor = true;
            // 
            // rdbNrmlCondition
            // 
            this.rdbNrmlCondition.AutoSize = true;
            this.rdbNrmlCondition.Checked = true;
            this.rdbNrmlCondition.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelAreaParamsBindingSource, "NormalCondition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbNrmlCondition.Location = new System.Drawing.Point(9, 21);
            this.rdbNrmlCondition.Name = "rdbNrmlCondition";
            this.rdbNrmlCondition.Size = new System.Drawing.Size(58, 17);
            this.rdbNrmlCondition.TabIndex = 0;
            this.rdbNrmlCondition.TabStop = true;
            this.rdbNrmlCondition.Text = "Normal";
            this.rdbNrmlCondition.UseVisualStyleBackColor = true;
            // 
            // cmbAircraftCategory
            // 
            this.cmbAircraftCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelAreaParamsBindingSource, "Category", true));
            this.cmbAircraftCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelAreaParamsBindingSource, "Category", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbAircraftCategory.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelAreaParamsBindingSource, "IsEnroute", true));
            this.cmbAircraftCategory.DataSource = this.aircraftCategoriesBindingSource;
            this.cmbAircraftCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAircraftCategory.FormattingEnabled = true;
            this.cmbAircraftCategory.Location = new System.Drawing.Point(96, 39);
            this.cmbAircraftCategory.Name = "cmbAircraftCategory";
            this.cmbAircraftCategory.Size = new System.Drawing.Size(57, 21);
            this.cmbAircraftCategory.TabIndex = 3;
            // 
            // aircraftCategoriesBindingSource
            // 
            this.aircraftCategoriesBindingSource.DataMember = "AircraftCategories";
            this.aircraftCategoriesBindingSource.DataSource = this.modelAreaParamsBindingSource;
            this.aircraftCategoriesBindingSource.CurrentChanged += new System.EventHandler(this.aircraftCategoriesBindingSource_CurrentChanged);
            // 
            // lblAircraftCategory
            // 
            this.lblAircraftCategory.AutoSize = true;
            this.lblAircraftCategory.Location = new System.Drawing.Point(5, 43);
            this.lblAircraftCategory.Name = "lblAircraftCategory";
            this.lblAircraftCategory.Size = new System.Drawing.Size(85, 13);
            this.lblAircraftCategory.TabIndex = 2;
            this.lblAircraftCategory.Text = "Aircraft Category";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.rdbOmnidirectional);
            this.groupBox6.Controls.Add(this.rdbDirect);
            this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox6.Location = new System.Drawing.Point(401, 228);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(159, 91);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Protection of entries";
            // 
            // rdbOmnidirectional
            // 
            this.rdbOmnidirectional.AutoSize = true;
            this.rdbOmnidirectional.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelProcedureTypeBindingSource, "OmnidirectionalIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbOmnidirectional.Location = new System.Drawing.Point(12, 62);
            this.rdbOmnidirectional.Name = "rdbOmnidirectional";
            this.rdbOmnidirectional.Size = new System.Drawing.Size(97, 17);
            this.rdbOmnidirectional.TabIndex = 1;
            this.rdbOmnidirectional.TabStop = true;
            this.rdbOmnidirectional.Text = "Omnidirectional";
            this.rdbOmnidirectional.UseVisualStyleBackColor = true;
            // 
            // rdbDirect
            // 
            this.rdbDirect.AutoSize = true;
            this.rdbDirect.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelProcedureTypeBindingSource, "DirectIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbDirect.Location = new System.Drawing.Point(12, 30);
            this.rdbDirect.Name = "rdbDirect";
            this.rdbDirect.Size = new System.Drawing.Size(53, 17);
            this.rdbDirect.TabIndex = 0;
            this.rdbDirect.TabStop = true;
            this.rdbDirect.Text = "Direct";
            this.rdbDirect.UseVisualStyleBackColor = true;
            // 
            // grpAltitude
            // 
            this.grpAltitude.Controls.Add(this.uDRadial);
            this.grpAltitude.Controls.Add(this.lblRadial);
            this.grpAltitude.Controls.Add(this.lblRadialCap);
            this.grpAltitude.Controls.Add(this.lblMocCap);
            this.grpAltitude.Controls.Add(this.grpbTurn);
            this.grpAltitude.Controls.Add(this.lblMOC);
            this.grpAltitude.Controls.Add(this.lblAltitudeCap);
            this.grpAltitude.Controls.Add(this.cmbObstacle);
            this.grpAltitude.Controls.Add(this.udAltitude);
            this.grpAltitude.Controls.Add(this.lblAltitudeUnit);
            this.grpAltitude.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpAltitude.Location = new System.Drawing.Point(12, 20);
            this.grpAltitude.Name = "grpAltitude";
            this.grpAltitude.Size = new System.Drawing.Size(342, 111);
            this.grpAltitude.TabIndex = 3;
            this.grpAltitude.TabStop = false;
            this.grpAltitude.Text = "Flight Paramets";
            // 
            // uDRadial
            // 
            this.uDRadial.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", this.modelAreaParamsBindingSource, "MaxRadialInDegree", true));
            this.uDRadial.DataBindings.Add(new System.Windows.Forms.Binding("Minimum", this.modelAreaParamsBindingSource, "MinRadialInDegree", true));
            this.uDRadial.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.modelAreaParamsBindingSource, "Radial", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.uDRadial.DataBindings.Add(new System.Windows.Forms.Binding("Tag", this.modelAreaParamsBindingSource, "Tag", true));
            this.uDRadial.Location = new System.Drawing.Point(119, 85);
            this.uDRadial.Name = "uDRadial";
            this.uDRadial.Size = new System.Drawing.Size(58, 20);
            this.uDRadial.TabIndex = 30;
            // 
            // lblRadial
            // 
            this.lblRadial.AutoSize = true;
            this.lblRadial.Location = new System.Drawing.Point(185, 87);
            this.lblRadial.Name = "lblRadial";
            this.lblRadial.Size = new System.Drawing.Size(11, 13);
            this.lblRadial.TabIndex = 29;
            this.lblRadial.Text = "°";
            // 
            // lblRadialCap
            // 
            this.lblRadialCap.AutoSize = true;
            this.lblRadialCap.Location = new System.Drawing.Point(10, 87);
            this.lblRadialCap.Name = "lblRadialCap";
            this.lblRadialCap.Size = new System.Drawing.Size(97, 13);
            this.lblRadialCap.TabIndex = 28;
            this.lblRadialCap.Text = "Inboud course (T) :";
            // 
            // lblMocCap
            // 
            this.lblMocCap.AutoSize = true;
            this.lblMocCap.Location = new System.Drawing.Point(10, 53);
            this.lblMocCap.Name = "lblMocCap";
            this.lblMocCap.Size = new System.Drawing.Size(37, 13);
            this.lblMocCap.TabIndex = 27;
            this.lblMocCap.Text = "MOC :";
            // 
            // grpbTurn
            // 
            this.grpbTurn.Controls.Add(this.rdbtLeft);
            this.grpbTurn.Controls.Add(this.rdbRight);
            this.grpbTurn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpbTurn.Location = new System.Drawing.Point(224, 9);
            this.grpbTurn.Name = "grpbTurn";
            this.grpbTurn.Size = new System.Drawing.Size(113, 56);
            this.grpbTurn.TabIndex = 5;
            this.grpbTurn.TabStop = false;
            this.grpbTurn.Text = "Turn Direction";
            // 
            // rdbtLeft
            // 
            this.rdbtLeft.AutoSize = true;
            this.rdbtLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbtLeft.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelAreaParamsBindingSource, "LeftTur", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbtLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdbtLeft.Location = new System.Drawing.Point(6, 23);
            this.rdbtLeft.Name = "rdbtLeft";
            this.rdbtLeft.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rdbtLeft.Size = new System.Drawing.Size(43, 17);
            this.rdbtLeft.TabIndex = 1;
            this.rdbtLeft.TabStop = true;
            this.rdbtLeft.Text = "Left";
            this.rdbtLeft.UseVisualStyleBackColor = true;
            // 
            // rdbRight
            // 
            this.rdbRight.AutoSize = true;
            this.rdbRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdbRight.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelAreaParamsBindingSource, "RightTur", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbRight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdbRight.Location = new System.Drawing.Point(55, 23);
            this.rdbRight.Name = "rdbRight";
            this.rdbRight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rdbRight.Size = new System.Drawing.Size(50, 17);
            this.rdbRight.TabIndex = 0;
            this.rdbRight.TabStop = true;
            this.rdbRight.Text = "Right";
            this.rdbRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rdbRight.UseVisualStyleBackColor = true;
            // 
            // lblMOC
            // 
            this.lblMOC.AutoSize = true;
            this.lblMOC.Location = new System.Drawing.Point(186, 53);
            this.lblMOC.Name = "lblMOC";
            this.lblMOC.Size = new System.Drawing.Size(15, 13);
            this.lblMOC.TabIndex = 1;
            this.lblMOC.Text = "m";
            // 
            // lblAltitudeCap
            // 
            this.lblAltitudeCap.AutoSize = true;
            this.lblAltitudeCap.Location = new System.Drawing.Point(10, 21);
            this.lblAltitudeCap.Name = "lblAltitudeCap";
            this.lblAltitudeCap.Size = new System.Drawing.Size(108, 13);
            this.lblAltitudeCap.TabIndex = 26;
            this.lblAltitudeCap.Text = "Altitude ( from MSL ) :";
            // 
            // cmbObstacle
            // 
            this.cmbObstacle.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelAreaParamsBindingSource, "CurMoc", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbObstacle.DataSource = this.mocListBindingSource;
            this.cmbObstacle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbObstacle.FormattingEnabled = true;
            this.cmbObstacle.Location = new System.Drawing.Point(119, 50);
            this.cmbObstacle.Name = "cmbObstacle";
            this.cmbObstacle.Size = new System.Drawing.Size(58, 21);
            this.cmbObstacle.TabIndex = 0;
            // 
            // mocListBindingSource
            // 
            this.mocListBindingSource.DataMember = "MocList";
            this.mocListBindingSource.DataSource = this.modelAreaParamsBindingSource;
            // 
            // udAltitude
            // 
            this.udAltitude.DataBindings.Add(new System.Windows.Forms.Binding("Maximum", this.modelAreaParamsBindingSource, "MaxAltitude", true));
            this.udAltitude.DataBindings.Add(new System.Windows.Forms.Binding("Minimum", this.modelAreaParamsBindingSource, "MinAltitude", true));
            this.udAltitude.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.modelAreaParamsBindingSource, "Altitude", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udAltitude.Location = new System.Drawing.Point(119, 19);
            this.udAltitude.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udAltitude.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udAltitude.Name = "udAltitude";
            this.udAltitude.Size = new System.Drawing.Size(58, 20);
            this.udAltitude.TabIndex = 0;
            this.udAltitude.Value = new decimal(new int[] {
            3050,
            0,
            0,
            0});
            // 
            // lblAltitudeUnit
            // 
            this.lblAltitudeUnit.AutoSize = true;
            this.lblAltitudeUnit.Location = new System.Drawing.Point(186, 23);
            this.lblAltitudeUnit.Name = "lblAltitudeUnit";
            this.lblAltitudeUnit.Size = new System.Drawing.Size(15, 13);
            this.lblAltitudeUnit.TabIndex = 0;
            this.lblAltitudeUnit.Text = "m";
            // 
            // btnSave
            // 
            this.btnSave.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelPointChoiseBindingSource, "SaveIsEnabled", true));
            this.btnSave.Image = Aran.Panda.Rnav.Holding.Properties.Resources.apply;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(487, 423);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 24);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // modelPointChoiseBindingSource
            // 
            this.modelPointChoiseBindingSource.DataSource = typeof(ModelPointChoise);
            // 
            // btnHoldigArea
            // 
            this.btnHoldigArea.AutoSize = true;
            this.btnHoldigArea.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelPointChoiseBindingSource, "HoldingAreaIsEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnHoldigArea.Enabled = false;
            this.btnHoldigArea.Image = global::Aran.Panda.Rnav.Holding.Properties.Resources.draw;
            this.btnHoldigArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHoldigArea.Location = new System.Drawing.Point(315, 423);
            this.btnHoldigArea.Name = "btnHoldigArea";
            this.btnHoldigArea.Size = new System.Drawing.Size(81, 24);
            this.btnHoldigArea.TabIndex = 23;
            this.btnHoldigArea.Text = "Draw";
            this.btnHoldigArea.UseVisualStyleBackColor = true;
            this.btnHoldigArea.Click += new System.EventHandler(this.btnBaseArea_Click);
            // 
            // btnReport
            // 
            this.btnReport.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnReport.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.modelPointChoiseBindingSource, "ReportIsEnabled", true));
            this.btnReport.Image = Aran.Panda.Rnav.Holding.Properties.Resources.documents;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(400, 423);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(81, 24);
            this.btnReport.TabIndex = 29;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnDraw
            // 
            this.btnDraw.Image = global::Aran.Panda.Rnav.Holding.Properties.Resources.settings;
            this.btnDraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDraw.Location = new System.Drawing.Point(65, 423);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(25, 25);
            this.btnDraw.TabIndex = 30;
            this.btnDraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.myToolTip.SetToolTip(this.btnDraw, "Draw Settings");
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // iChangedBindingSource
            // 
            this.iChangedBindingSource.DataSource = typeof(Holding.IChanged);
            // 
            // validationClassBindingSource
            // 
            this.validationClassBindingSource.DataSource = typeof(Holding.ValidationClass);
            // 
            // modelPBNBindingSource
            // 
            this.modelPBNBindingSource.DataSource = typeof(ModelPBN);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(580, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.grpHldgFunction);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.grpFlightCondition);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 392);
            this.panel1.TabIndex = 0;
            // 
            // grpHldgFunction
            // 
            this.grpHldgFunction.Controls.Add(this.rdbRNPHolding);
            this.grpHldgFunction.Controls.Add(this.rdbWithoutHldgFunc);
            this.grpHldgFunction.Controls.Add(this.rdbWithHldgFunction);
            this.grpHldgFunction.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpHldgFunction.Location = new System.Drawing.Point(303, 19);
            this.grpHldgFunction.Name = "grpHldgFunction";
            this.grpHldgFunction.Size = new System.Drawing.Size(257, 140);
            this.grpHldgFunction.TabIndex = 28;
            this.grpHldgFunction.TabStop = false;
            this.grpHldgFunction.Text = "Holding Type";
            // 
            // rdbRNPHolding
            // 
            this.rdbRNPHolding.AutoSize = true;
            this.rdbRNPHolding.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelProcedureTypeBindingSource, "RNPIsActive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbRNPHolding.Location = new System.Drawing.Point(15, 92);
            this.rdbRNPHolding.Name = "rdbRNPHolding";
            this.rdbRNPHolding.Size = new System.Drawing.Size(85, 17);
            this.rdbRNPHolding.TabIndex = 14;
            this.rdbRNPHolding.TabStop = true;
            this.rdbRNPHolding.Text = "RNP holding";
            this.rdbRNPHolding.UseVisualStyleBackColor = true;
            // 
            // rdbWithoutHldgFunc
            // 
            this.rdbWithoutHldgFunc.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelProcedureTypeBindingSource, "WithoutHoldFuncIsActive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbWithoutHldgFunc.Location = new System.Drawing.Point(15, 54);
            this.rdbWithoutHldgFunc.Name = "rdbWithoutHldgFunc";
            this.rdbWithoutHldgFunc.Size = new System.Drawing.Size(221, 30);
            this.rdbWithoutHldgFunc.TabIndex = 1;
            this.rdbWithoutHldgFunc.TabStop = true;
            this.rdbWithoutHldgFunc.Text = "RNAV systems without holding functionality";
            this.rdbWithoutHldgFunc.UseVisualStyleBackColor = true;
            // 
            // rdbWithHldgFunction
            // 
            this.rdbWithHldgFunction.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.modelProcedureTypeBindingSource, "HoldFuncIsActive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdbWithHldgFunction.Location = new System.Drawing.Point(15, 16);
            this.rdbWithHldgFunction.Name = "rdbWithHldgFunction";
            this.rdbWithHldgFunction.Size = new System.Drawing.Size(221, 29);
            this.rdbWithHldgFunction.TabIndex = 0;
            this.rdbWithHldgFunction.TabStop = true;
            this.rdbWithHldgFunction.Text = "RNAV systems with holding functionality";
            this.rdbWithHldgFunction.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pointPicker1);
            this.groupBox1.Controls.Add(this.lblNavaid);
            this.groupBox1.Controls.Add(this.lblWayPointChoise);
            this.groupBox1.Controls.Add(this.cmbPointList);
            this.groupBox1.Controls.Add(this.lblDesignatedPt);
            this.groupBox1.Controls.Add(this.cmbNavaid);
            this.groupBox1.Controls.Add(this.cmbWayPointChoise);
            this.groupBox1.Location = new System.Drawing.Point(3, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 220);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set  Waypoint";
            // 
            // pointPicker1
            // 
            this.pointPicker1.ByClick = false;
            this.pointPicker1.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.modelPointChoiseBindingSource, "PointPickerIsActive", true));
            this.pointPicker1.DDAccuracy = 4;
            this.pointPicker1.DMSAccuracy = 2;
            this.pointPicker1.IsDD = true;
            this.pointPicker1.Latitude = 0D;
            this.pointPicker1.Location = new System.Drawing.Point(18, 73);
            this.pointPicker1.Longitude = 0D;
            this.pointPicker1.Name = "pointPicker1";
            this.pointPicker1.Size = new System.Drawing.Size(275, 105);
            this.pointPicker1.TabIndex = 40;
            this.pointPicker1.ByClickChanged += new System.EventHandler(this.pointPicker1_ByClickChanged);
            this.pointPicker1.LatitudeChanged += new System.EventHandler(this.pointPicker1_LatitudeChanged);
            this.pointPicker1.LongitudeChanged += new System.EventHandler(this.pointPicker1_LatitudeChanged);
            // 
            // lblNavaid
            // 
            this.lblNavaid.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNavaid.AutoSize = true;
            this.lblNavaid.Location = new System.Drawing.Point(15, 76);
            this.lblNavaid.Name = "lblNavaid";
            this.lblNavaid.Size = new System.Drawing.Size(47, 13);
            this.lblNavaid.TabIndex = 39;
            this.lblNavaid.Text = "Navaid :";
            // 
            // lblWayPointChoise
            // 
            this.lblWayPointChoise.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblWayPointChoise.AutoSize = true;
            this.lblWayPointChoise.Location = new System.Drawing.Point(15, 33);
            this.lblWayPointChoise.Name = "lblWayPointChoise";
            this.lblWayPointChoise.Size = new System.Drawing.Size(93, 13);
            this.lblWayPointChoise.TabIndex = 37;
            this.lblWayPointChoise.Text = "Waypoint Choise :";
            // 
            // cmbPointList
            // 
            this.cmbPointList.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPointList.DataSource = this.pointListBindingSource;
            this.cmbPointList.DisplayMember = "Designator";
            this.cmbPointList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPointList.FormattingEnabled = true;
            this.cmbPointList.Location = new System.Drawing.Point(123, 73);
            this.cmbPointList.Name = "cmbPointList";
            this.cmbPointList.Size = new System.Drawing.Size(125, 21);
            this.cmbPointList.TabIndex = 31;
            this.cmbPointList.SelectedValueChanged += new System.EventHandler(this.cmbPointList_SelectedValueChanged);
            // 
            // pointListBindingSource
            // 
            this.pointListBindingSource.DataMember = "PointList";
            this.pointListBindingSource.DataSource = this.modelPointChoiseBindingSource;
            // 
            // lblDesignatedPt
            // 
            this.lblDesignatedPt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDesignatedPt.AutoSize = true;
            this.lblDesignatedPt.Location = new System.Drawing.Point(15, 76);
            this.lblDesignatedPt.Name = "lblDesignatedPt";
            this.lblDesignatedPt.Size = new System.Drawing.Size(94, 13);
            this.lblDesignatedPt.TabIndex = 35;
            this.lblDesignatedPt.Text = "Designated Point :";
            // 
            // cmbNavaid
            // 
            this.cmbNavaid.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbNavaid.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.modelPointChoiseBindingSource, "NavaidListIsActive", true));
            this.cmbNavaid.DataSource = this.navaidListBindingSource;
            this.cmbNavaid.DisplayMember = "Designator";
            this.cmbNavaid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNavaid.FormattingEnabled = true;
            this.cmbNavaid.Location = new System.Drawing.Point(123, 73);
            this.cmbNavaid.Name = "cmbNavaid";
            this.cmbNavaid.Size = new System.Drawing.Size(125, 21);
            this.cmbNavaid.TabIndex = 38;
            this.cmbNavaid.SelectedValueChanged += new System.EventHandler(this.cmbNavaid_SelectedValueChanged);
            // 
            // navaidListBindingSource
            // 
            this.navaidListBindingSource.DataMember = "NavaidList";
            this.navaidListBindingSource.DataSource = this.modelPointChoiseBindingSource;
            // 
            // cmbWayPointChoise
            // 
            this.cmbWayPointChoise.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbWayPointChoise.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelPointChoiseBindingSource, "PointChoise", true));
            this.cmbWayPointChoise.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelPointChoiseBindingSource, "PointChoise", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbWayPointChoise.DataSource = this.significantListBindingSource1;
            this.cmbWayPointChoise.DisplayMember = "Text";
            this.cmbWayPointChoise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWayPointChoise.FormattingEnabled = true;
            this.cmbWayPointChoise.Location = new System.Drawing.Point(123, 30);
            this.cmbWayPointChoise.Name = "cmbWayPointChoise";
            this.cmbWayPointChoise.Size = new System.Drawing.Size(125, 21);
            this.cmbWayPointChoise.TabIndex = 36;
            this.cmbWayPointChoise.SelectedIndexChanged += new System.EventHandler(this.cmbWayPointChoise_SelectedIndexChanged);
            // 
            // significantListBindingSource1
            // 
            this.significantListBindingSource1.DataMember = "SignificantList";
            this.significantListBindingSource1.DataSource = this.modelPointChoiseBindingSource;
            // 
            // grpFlightCondition
            // 
            this.grpFlightCondition.Controls.Add(this.lblPBN);
            this.grpFlightCondition.Controls.Add(this.lblFlightReciever);
            this.grpFlightCondition.Controls.Add(this.cmbPBN);
            this.grpFlightCondition.Controls.Add(this.cmbFlightReciever);
            this.grpFlightCondition.Controls.Add(this.lblFlightPhase);
            this.grpFlightCondition.Controls.Add(this.cmbFlightPhases);
            this.grpFlightCondition.Location = new System.Drawing.Point(3, 19);
            this.grpFlightCondition.Name = "grpFlightCondition";
            this.grpFlightCondition.Size = new System.Drawing.Size(285, 140);
            this.grpFlightCondition.TabIndex = 26;
            this.grpFlightCondition.TabStop = false;
            this.grpFlightCondition.Text = "PBN";
            // 
            // lblPBN
            // 
            this.lblPBN.AutoSize = true;
            this.lblPBN.Location = new System.Drawing.Point(15, 95);
            this.lblPBN.Name = "lblPBN";
            this.lblPBN.Size = new System.Drawing.Size(32, 13);
            this.lblPBN.TabIndex = 5;
            this.lblPBN.Text = "PBN:";
            // 
            // lblFlightReciever
            // 
            this.lblFlightReciever.AutoSize = true;
            this.lblFlightReciever.Location = new System.Drawing.Point(15, 57);
            this.lblFlightReciever.Name = "lblFlightReciever";
            this.lblFlightReciever.Size = new System.Drawing.Size(58, 13);
            this.lblFlightReciever.TabIndex = 4;
            this.lblFlightReciever.Text = "Recievers:";
            // 
            // cmbPBN
            // 
            this.cmbPBN.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelPBNBindingSource, "CurPBN", true));
            this.cmbPBN.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelPBNBindingSource, "CurPBN", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbPBN.DataSource = this.pbnConditionListBindingSource;
            this.cmbPBN.DisplayMember = "PBNName";
            this.cmbPBN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPBN.FormattingEnabled = true;
            this.cmbPBN.Location = new System.Drawing.Point(106, 92);
            this.cmbPBN.Name = "cmbPBN";
            this.cmbPBN.Size = new System.Drawing.Size(142, 21);
            this.cmbPBN.TabIndex = 3;
            // 
            // pbnConditionListBindingSource
            // 
            this.pbnConditionListBindingSource.DataMember = "PbnConditionList";
            this.pbnConditionListBindingSource.DataSource = this.modelPBNBindingSource;
            // 
            // cmbFlightReciever
            // 
            this.cmbFlightReciever.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelPBNBindingSource, "CurReciever", true));
            this.cmbFlightReciever.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelPBNBindingSource, "CurReciever", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbFlightReciever.DataSource = this.recieverListBindingSource;
            this.cmbFlightReciever.DisplayMember = "RecieverName";
            this.cmbFlightReciever.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFlightReciever.FormattingEnabled = true;
            this.cmbFlightReciever.Location = new System.Drawing.Point(106, 54);
            this.cmbFlightReciever.Name = "cmbFlightReciever";
            this.cmbFlightReciever.Size = new System.Drawing.Size(142, 21);
            this.cmbFlightReciever.TabIndex = 2;
            // 
            // recieverListBindingSource
            // 
            this.recieverListBindingSource.DataMember = "RecieverList";
            this.recieverListBindingSource.DataSource = this.modelPBNBindingSource;
            // 
            // lblFlightPhase
            // 
            this.lblFlightPhase.AutoSize = true;
            this.lblFlightPhase.Location = new System.Drawing.Point(15, 19);
            this.lblFlightPhase.Name = "lblFlightPhase";
            this.lblFlightPhase.Size = new System.Drawing.Size(73, 13);
            this.lblFlightPhase.TabIndex = 1;
            this.lblFlightPhase.Text = "Flight Phases:";
            // 
            // cmbFlightPhases
            // 
            this.cmbFlightPhases.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.modelPBNBindingSource, "CurFlightPhase", true));
            this.cmbFlightPhases.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.modelPBNBindingSource, "CurFlightPhase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbFlightPhases.DataSource = this.phaseListBindingSource;
            this.cmbFlightPhases.DisplayMember = "UserFlightPhaseName";
            this.cmbFlightPhases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFlightPhases.FormattingEnabled = true;
            this.cmbFlightPhases.Location = new System.Drawing.Point(106, 16);
            this.cmbFlightPhases.Name = "cmbFlightPhases";
            this.cmbFlightPhases.Size = new System.Drawing.Size(142, 21);
            this.cmbFlightPhases.TabIndex = 0;
            // 
            // phaseListBindingSource
            // 
            this.phaseListBindingSource.DataMember = "PhaseList";
            this.phaseListBindingSource.DataSource = this.modelPBNBindingSource;
            // 
            // significantListBindingSource
            // 
            this.significantListBindingSource.DataMember = "SignificantList";
            this.significantListBindingSource.DataSource = this.modelPointChoiseBindingSource;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(588, 424);
            this.tabControl1.TabIndex = 23;
            // 
            // bussinesLogicBindingSource
            // 
            this.bussinesLogicBindingSource.DataSource = typeof(Bussines_Logic);
            // 
            // bussinesLogicBindingSource1
            // 
            this.bussinesLogicBindingSource1.DataSource = typeof(Bussines_Logic);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "apply.gif");
            this.imageList1.Images.SetKeyName(1, "cancel.gif");
            this.imageList1.Images.SetKeyName(2, "left.gif");
            this.imageList1.Images.SetKeyName(3, "panda.ico");
            this.imageList1.Images.SetKeyName(4, "print.gif");
            this.imageList1.Images.SetKeyName(5, "right.gif");
            this.imageList1.Images.SetKeyName(6, "save.gif");
            // 
            // btnInfo
            // 
            this.btnInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInfo.BackgroundImage")));
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInfo.Location = new System.Drawing.Point(35, 423);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(0);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(25, 25);
            this.btnInfo.TabIndex = 29;
            this.btnInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // drawSettingsToolStripMenuItem
            // 
            this.drawSettingsToolStripMenuItem.Name = "drawSettingsToolStripMenuItem";
            this.drawSettingsToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.drawSettingsToolStripMenuItem.Text = "Draw Settings";
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(4, 423);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 31;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmHoldingMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 452);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.btnPrevius);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnHoldigArea);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.tabControl1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.Name = "frmHoldingMain";
            this.Padding = new System.Windows.Forms.Padding(4, 5, 0, 0);
            this.Text = "RNAV Holding";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHoldingMain_FormClosed);
            this.Load += new System.EventHandler(this.frmHoldingMain_Load);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelWizardChangeBindingSource)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGridNavaid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingNavListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingNavOperationBindingSource)).EndInit();
            this.grpDmeCovType.ResumeLayout(false);
            this.grpDmeCovType.PerformLayout();
            this.grpTime.ResumeLayout(false);
            this.grpTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelProcedureTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceTypeListBindingSource)).EndInit();
            this.grpIas.ResumeLayout(false);
            this.grpIas.PerformLayout();
            this.grpIasMinMax.ResumeLayout(false);
            this.grpIasMinMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udIas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelAreaParamsBindingSource)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aircraftCategoriesBindingSource)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.grpAltitude.ResumeLayout(false);
            this.grpAltitude.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uDRadial)).EndInit();
            this.grpbTurn.ResumeLayout(false);
            this.grpbTurn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mocListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udAltitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPointChoiseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iChangedBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.validationClassBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelPBNBindingSource)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpHldgFunction.ResumeLayout(false);
            this.grpHldgFunction.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pointListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navaidListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.significantListBindingSource1)).EndInit();
            this.grpFlightCondition.ResumeLayout(false);
            this.grpFlightCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnConditionListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recieverListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.significantListBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bussinesLogicBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bussinesLogicBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevius;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnHoldigArea;
        private System.Windows.Forms.GroupBox grpbTurn;
        private System.Windows.Forms.RadioButton rdbtLeft;
        private System.Windows.Forms.RadioButton rdbRight;
        private System.Windows.Forms.GroupBox grpTime;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblWd;
        private System.Windows.Forms.ComboBox cmbTimeWd;
        private System.Windows.Forms.Label lblMOC;
        private System.Windows.Forms.ComboBox cmbObstacle;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox grpIas;
        private System.Windows.Forms.Label lblIasType;
        private System.Windows.Forms.NumericUpDown udIas;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton rdbTrbCondition;
        private System.Windows.Forms.RadioButton rdbNrmlCondition;
        private System.Windows.Forms.Label lblIas;
        private System.Windows.Forms.ComboBox cmbAircraftCategory;
        private System.Windows.Forms.Label lblAircraftCategory;
        private System.Windows.Forms.GroupBox grpAltitude;
        private System.Windows.Forms.NumericUpDown udAltitude;
        private System.Windows.Forms.Label lblAltitudeUnit;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpFlightCondition;
        private System.Windows.Forms.Label lblPBN;
        private System.Windows.Forms.Label lblFlightReciever;
        private System.Windows.Forms.ComboBox cmbPBN;
        private System.Windows.Forms.ComboBox cmbFlightReciever;
        private System.Windows.Forms.Label lblFlightPhase;
        private System.Windows.Forms.ComboBox cmbFlightPhases;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label lblDesignatedPt;
        private System.Windows.Forms.BindingSource modelProcedureTypeBindingSource;
        private System.Windows.Forms.BindingSource modelPointChoiseBindingSource;
        private System.Windows.Forms.BindingSource significantListBindingSource;
        private System.Windows.Forms.BindingSource modelPBNBindingSource;
        private System.Windows.Forms.BindingSource pbnConditionListBindingSource;
        private System.Windows.Forms.BindingSource recieverListBindingSource;
        private System.Windows.Forms.BindingSource phaseListBindingSource;
        private System.Windows.Forms.BindingSource bussinesLogicBindingSource;
        private System.Windows.Forms.BindingSource bussinesLogicBindingSource1;
        private System.Windows.Forms.BindingSource modelAreaParamsBindingSource;
        private System.Windows.Forms.BindingSource distanceTypeListBindingSource;
        private System.Windows.Forms.BindingSource aircraftCategoriesBindingSource;
        private System.Windows.Forms.BindingSource modelWizardChangeBindingSource;
        private System.Windows.Forms.BindingSource pointListBindingSource;
        private System.Windows.Forms.GroupBox grpIasMinMax;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox grpDmeCovType;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rdbTwoDmeCoverage;
        private System.Windows.Forms.NumericUpDown udWD;
        private System.Windows.Forms.BindingSource holdingNavListBindingSource;
        private System.Windows.Forms.BindingSource holdingNavOperationBindingSource;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.BindingSource mocListBindingSource;
        private System.Windows.Forms.CheckBox ckbDrawDME;
        private System.Windows.Forms.BindingSource validationClassBindingSource;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource iChangedBindingSource;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblMocCap;
        private System.Windows.Forms.Label lblAltitudeCap;
        private System.Windows.Forms.GroupBox grpHldgFunction;
        private System.Windows.Forms.RadioButton rdbRNPHolding;
        private System.Windows.Forms.RadioButton rdbWithoutHldgFunc;
        private System.Windows.Forms.RadioButton rdbWithHldgFunction;
        private System.Windows.Forms.NumericUpDown uDRadial;
        private System.Windows.Forms.Label lblRadial;
        private System.Windows.Forms.Label lblRadialCap;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.ToolStripMenuItem drawSettingsToolStripMenuItem;
        private System.Windows.Forms.Label lblNavaid;
        private System.Windows.Forms.ComboBox cmbNavaid;
        private System.Windows.Forms.Label lblWayPointChoise;
        private System.Windows.Forms.ComboBox cmbWayPointChoise;
        private System.Windows.Forms.ComboBox cmbPointList;
        private System.Windows.Forms.RadioButton rdbOmnidirectional;
        private System.Windows.Forms.RadioButton rdbDirect;
        private System.Windows.Forms.BindingSource significantListBindingSource1;
        private System.Windows.Forms.BindingSource navaidListBindingSource;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dGridNavaid;
        private ChoosePointNS.PointPicker pointPicker1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn designatorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistanceNavaidToFix;
        private System.Windows.Forms.DataGridViewTextBoxColumn CourseNavaidToFix;
        private RadioButton radioButton1;

    }
	}
