namespace UMLInfo
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.featureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.choiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collectionMemberChoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xSDcomplexTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getChoiceNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDocumentInfoFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.curRowStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.analyseTSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showPropTSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.genEnumTSB = new System.Windows.Forms.ToolStripButton();
            this.createClassTSB = new System.Windows.Forms.ToolStripButton();
            this.createEnumAranObTSB = new System.Windows.Forms.ToolStripButton();
            this.genAllTSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.createTable_DataTypeTSB = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.docTextBox = new System.Windows.Forms.TextBox();
            this.objInfoDGV = new System.Windows.Forms.DataGridView();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.searchTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objInfoDGV)).BeginInit();
            this.objInfoDGV.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.showToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.testsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(816, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.featureToolStripMenuItem,
            this.objectsToolStripMenuItem,
            this.codeListToolStripMenuItem,
            this.dataTypesToolStripMenuItem,
            this.choiceToolStripMenuItem,
            this.simpleTypeToolStripMenuItem,
            this.collectionMemberChoiceToolStripMenuItem,
            this.messageToolStripMenuItem,
            this.xSDcomplexTypeToolStripMenuItem,
            this.toolStripSeparator2,
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.showToolStripMenuItem.Text = "&Show";
            // 
            // featureToolStripMenuItem
            // 
            this.featureToolStripMenuItem.Checked = true;
            this.featureToolStripMenuItem.CheckOnClick = true;
            this.featureToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.featureToolStripMenuItem.Name = "featureToolStripMenuItem";
            this.featureToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.featureToolStripMenuItem.Tag = "feature";
            this.featureToolStripMenuItem.Text = "Feature";
            // 
            // objectsToolStripMenuItem
            // 
            this.objectsToolStripMenuItem.Checked = true;
            this.objectsToolStripMenuItem.CheckOnClick = true;
            this.objectsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
            this.objectsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.objectsToolStripMenuItem.Tag = "object";
            this.objectsToolStripMenuItem.Text = "Object";
            // 
            // codeListToolStripMenuItem
            // 
            this.codeListToolStripMenuItem.Checked = true;
            this.codeListToolStripMenuItem.CheckOnClick = true;
            this.codeListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.codeListToolStripMenuItem.Name = "codeListToolStripMenuItem";
            this.codeListToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.codeListToolStripMenuItem.Tag = "codelist";
            this.codeListToolStripMenuItem.Text = "CodeList";
            // 
            // dataTypesToolStripMenuItem
            // 
            this.dataTypesToolStripMenuItem.Checked = true;
            this.dataTypesToolStripMenuItem.CheckOnClick = true;
            this.dataTypesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dataTypesToolStripMenuItem.Name = "dataTypesToolStripMenuItem";
            this.dataTypesToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.dataTypesToolStripMenuItem.Tag = "datatype";
            this.dataTypesToolStripMenuItem.Text = "DataType";
            // 
            // choiceToolStripMenuItem
            // 
            this.choiceToolStripMenuItem.Checked = true;
            this.choiceToolStripMenuItem.CheckOnClick = true;
            this.choiceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.choiceToolStripMenuItem.Name = "choiceToolStripMenuItem";
            this.choiceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.choiceToolStripMenuItem.Tag = "choice";
            this.choiceToolStripMenuItem.Text = "Choice";
            // 
            // simpleTypeToolStripMenuItem
            // 
            this.simpleTypeToolStripMenuItem.Checked = true;
            this.simpleTypeToolStripMenuItem.CheckOnClick = true;
            this.simpleTypeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.simpleTypeToolStripMenuItem.Name = "simpleTypeToolStripMenuItem";
            this.simpleTypeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.simpleTypeToolStripMenuItem.Tag = "XSDsimpleType";
            this.simpleTypeToolStripMenuItem.Text = "SimpleType";
            // 
            // collectionMemberChoiceToolStripMenuItem
            // 
            this.collectionMemberChoiceToolStripMenuItem.Checked = true;
            this.collectionMemberChoiceToolStripMenuItem.CheckOnClick = true;
            this.collectionMemberChoiceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.collectionMemberChoiceToolStripMenuItem.Name = "collectionMemberChoiceToolStripMenuItem";
            this.collectionMemberChoiceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.collectionMemberChoiceToolStripMenuItem.Tag = "CollectionMemberChoice";
            this.collectionMemberChoiceToolStripMenuItem.Text = "CollectionMemberChoice";
            // 
            // messageToolStripMenuItem
            // 
            this.messageToolStripMenuItem.Checked = true;
            this.messageToolStripMenuItem.CheckOnClick = true;
            this.messageToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.messageToolStripMenuItem.Name = "messageToolStripMenuItem";
            this.messageToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.messageToolStripMenuItem.Tag = "message";
            this.messageToolStripMenuItem.Text = "Message";
            // 
            // xSDcomplexTypeToolStripMenuItem
            // 
            this.xSDcomplexTypeToolStripMenuItem.Checked = true;
            this.xSDcomplexTypeToolStripMenuItem.CheckOnClick = true;
            this.xSDcomplexTypeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xSDcomplexTypeToolStripMenuItem.Name = "xSDcomplexTypeToolStripMenuItem";
            this.xSDcomplexTypeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.xSDcomplexTypeToolStripMenuItem.Tag = "XSDcomplexType";
            this.xSDcomplexTypeToolStripMenuItem.Text = "XSDcomplexType";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.checkAllToolStripMenuItem.Text = "Check All";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck All";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getChoiceNamesToolStripMenuItem,
            this.createDocumentInfoFileToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // getChoiceNamesToolStripMenuItem
            // 
            this.getChoiceNamesToolStripMenuItem.Name = "getChoiceNamesToolStripMenuItem";
            this.getChoiceNamesToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.getChoiceNamesToolStripMenuItem.Text = "Get Choice Names";
            this.getChoiceNamesToolStripMenuItem.Click += new System.EventHandler(this.getChoiceNamesToolStripMenuItem_Click);
            // 
            // createDocumentInfoFileToolStripMenuItem
            // 
            this.createDocumentInfoFileToolStripMenuItem.Name = "createDocumentInfoFileToolStripMenuItem";
            this.createDocumentInfoFileToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.createDocumentInfoFileToolStripMenuItem.Text = "Create Document Info File";
            this.createDocumentInfoFileToolStripMenuItem.Click += new System.EventHandler(this.CreateDocumentInfoFile_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.curRowStatusLabel,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 405);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(816, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(200, 17);
            this.toolStripStatusLabel1.Text = "Row count: 0";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // curRowStatusLabel
            // 
            this.curRowStatusLabel.AutoSize = false;
            this.curRowStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.curRowStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.curRowStatusLabel.Name = "curRowStatusLabel";
            this.curRowStatusLabel.Size = new System.Drawing.Size(200, 17);
            this.curRowStatusLabel.Text = "Cur Row Index: 0";
            this.curRowStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(401, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyseTSB,
            this.toolStripSeparator1,
            this.showPropTSB,
            this.toolStripSeparator3,
            this.genEnumTSB,
            this.createClassTSB,
            this.createEnumAranObTSB,
            this.genAllTSB,
            this.toolStripSeparator4,
            this.createTable_DataTypeTSB});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(816, 23);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // analyseTSB
            // 
            this.analyseTSB.Image = ((System.Drawing.Image)(resources.GetObject("analyseTSB.Image")));
            this.analyseTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.analyseTSB.Name = "analyseTSB";
            this.analyseTSB.Size = new System.Drawing.Size(68, 20);
            this.analyseTSB.Text = "Analyse";
            this.analyseTSB.Click += new System.EventHandler(this.analyseTSB_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // showPropTSB
            // 
            this.showPropTSB.Image = ((System.Drawing.Image)(resources.GetObject("showPropTSB.Image")));
            this.showPropTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showPropTSB.Name = "showPropTSB";
            this.showPropTSB.Size = new System.Drawing.Size(108, 20);
            this.showPropTSB.Text = "Show Properies";
            this.showPropTSB.Click += new System.EventHandler(this.showPropTSB_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // genEnumTSB
            // 
            this.genEnumTSB.Image = ((System.Drawing.Image)(resources.GetObject("genEnumTSB.Image")));
            this.genEnumTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.genEnumTSB.Name = "genEnumTSB";
            this.genEnumTSB.Size = new System.Drawing.Size(113, 20);
            this.genEnumTSB.Text = "Generate Enums";
            this.genEnumTSB.Click += new System.EventHandler(this.genEnumTSB_Click);
            // 
            // createClassTSB
            // 
            this.createClassTSB.Image = ((System.Drawing.Image)(resources.GetObject("createClassTSB.Image")));
            this.createClassTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createClassTSB.Name = "createClassTSB";
            this.createClassTSB.Size = new System.Drawing.Size(91, 20);
            this.createClassTSB.Text = "Create Class";
            this.createClassTSB.Click += new System.EventHandler(this.createClassTSB_Click);
            // 
            // createEnumAranObTSB
            // 
            this.createEnumAranObTSB.Image = ((System.Drawing.Image)(resources.GetObject("createEnumAranObTSB.Image")));
            this.createEnumAranObTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createEnumAranObTSB.Name = "createEnumAranObTSB";
            this.createEnumAranObTSB.Size = new System.Drawing.Size(192, 20);
            this.createEnumAranObTSB.Text = "Create Enum (AranObjectType)";
            this.createEnumAranObTSB.Click += new System.EventHandler(this.createEnumAranObTSB_Click);
            // 
            // genAllTSB
            // 
            this.genAllTSB.Image = ((System.Drawing.Image)(resources.GetObject("genAllTSB.Image")));
            this.genAllTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.genAllTSB.Name = "genAllTSB";
            this.genAllTSB.Size = new System.Drawing.Size(91, 20);
            this.genAllTSB.Text = "Cenerate All";
            this.genAllTSB.Click += new System.EventHandler(this.genAllTSB_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // createTable_DataTypeTSB
            // 
            this.createTable_DataTypeTSB.Image = ((System.Drawing.Image)(resources.GetObject("createTable_DataTypeTSB.Image")));
            this.createTable_DataTypeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createTable_DataTypeTSB.Name = "createTable_DataTypeTSB";
            this.createTable_DataTypeTSB.Size = new System.Drawing.Size(124, 20);
            this.createTable_DataTypeTSB.Text = "Cr.Table DataType";
            this.createTable_DataTypeTSB.Click += new System.EventHandler(this.createTable_DataTypeTSB_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.docTextBox);
            this.panel1.Controls.Add(this.objInfoDGV);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(816, 358);
            this.panel1.TabIndex = 3;
            // 
            // docTextBox
            // 
            this.docTextBox.AcceptsReturn = true;
            this.docTextBox.AcceptsTab = true;
            this.docTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.docTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.docTextBox.Location = new System.Drawing.Point(3, 296);
            this.docTextBox.Multiline = true;
            this.docTextBox.Name = "docTextBox";
            this.docTextBox.ReadOnly = true;
            this.docTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.docTextBox.Size = new System.Drawing.Size(810, 49);
            this.docTextBox.TabIndex = 3;
            // 
            // objInfoDGV
            // 
            this.objInfoDGV.AllowUserToAddRows = false;
            this.objInfoDGV.AllowUserToOrderColumns = true;
            this.objInfoDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objInfoDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.objInfoDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.objInfoDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.objInfoDGV.ColumnHeadersHeight = 30;
            this.objInfoDGV.Controls.Add(this.searchPanel);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.objInfoDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.objInfoDGV.Location = new System.Drawing.Point(3, 3);
            this.objInfoDGV.Name = "objInfoDGV";
            this.objInfoDGV.ReadOnly = true;
            this.objInfoDGV.RowHeadersVisible = false;
            this.objInfoDGV.RowHeadersWidth = 25;
            this.objInfoDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.objInfoDGV.Size = new System.Drawing.Size(810, 287);
            this.objInfoDGV.TabIndex = 2;
            this.objInfoDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.objInfoDGV_CellContentClick);
            this.objInfoDGV.CurrentCellChanged += new System.EventHandler(this.objInfoDGV_CurrentCellChanged);
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPanel.Controls.Add(this.searchTB);
            this.searchPanel.Controls.Add(this.label1);
            this.searchPanel.Location = new System.Drawing.Point(525, 10);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(263, 33);
            this.searchPanel.TabIndex = 3;
            this.searchPanel.Visible = false;
            // 
            // searchTB
            // 
            this.searchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTB.Location = new System.Drawing.Point(50, 7);
            this.searchTB.Name = "searchTB";
            this.searchTB.Size = new System.Drawing.Size(202, 20);
            this.searchTB.TabIndex = 1;
            this.searchTB.TextChanged += new System.EventHandler(this.searchTB_TextChanged);
            this.searchTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTB_KeyUp);
            this.searchTB.Leave += new System.EventHandler(this.searchTB_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search";
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test1ToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // test1ToolStripMenuItem
            // 
            this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            this.test1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.test1ToolStripMenuItem.Text = "Test-1";
            this.test1ToolStripMenuItem.Click += new System.EventHandler(this.test1ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 427);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objInfoDGV)).EndInit();
            this.objInfoDGV.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView objInfoDGV;
        private System.Windows.Forms.ToolStripButton analyseTSB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.TextBox searchTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem featureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem choiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collectionMemberChoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xSDcomplexTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton showPropTSB;
        private System.Windows.Forms.TextBox docTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton genEnumTSB;
        private System.Windows.Forms.ToolStripStatusLabel curRowStatusLabel;
        private System.Windows.Forms.ToolStripButton createClassTSB;
        private System.Windows.Forms.ToolStripButton createEnumAranObTSB;
        private System.Windows.Forms.ToolStripButton genAllTSB;
        private System.Windows.Forms.ToolStripButton createTable_DataTypeTSB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getChoiceNamesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createDocumentInfoFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
    }
}