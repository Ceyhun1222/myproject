namespace KFileParser
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
            System.Windows.Forms.Label label1;
            this.ui_folderTB = new System.Windows.Forms.TextBox();
            this.ui_selectFolderButton = new System.Windows.Forms.Button();
            this.ui_parseRwyButton = new System.Windows.Forms.Button();
            this.ui_parseVSButton = new System.Windows.Forms.Button();
            this.ui_parseApronButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCntrlPnt = new System.Windows.Forms.Button();
            this.btnTaxiHoldingPos = new System.Windows.Forms.Button();
            this.btnRawPoint = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chckBxSortGuidanceLineTaxiway = new System.Windows.Forms.CheckBox();
            this.chckBxSortAircraftStand = new System.Windows.Forms.CheckBox();
            this.chckBxSortRwyCentPoint = new System.Windows.Forms.CheckBox();
            this.chckBxSortVertStruct = new System.Windows.Forms.CheckBox();
            this.chckBxSortApronElement = new System.Windows.Forms.CheckBox();
            this.chckBxSortGuidanceLineApron = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnNavaidComp = new System.Windows.Forms.Button();
            this.ui_parseSPButton = new System.Windows.Forms.Button();
            this.ui_parseApronDataButton = new System.Windows.Forms.Button();
            this.ui_parseSLButton = new System.Windows.Forms.Button();
            this.chckBxInsert2DB = new System.Windows.Forms.CheckBox();
            this.btnEtod = new System.Windows.Forms.Button();
            this.chckBxIsGDB = new System.Windows.Forms.CheckBox();
            this.chckBxReplaceDecimalWithDot = new System.Windows.Forms.CheckBox();
            this.btnArea2VertStructs = new System.Windows.Forms.Button();
            this.btnArea1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chckBxArea1ObsFromExcel = new System.Windows.Forms.CheckBox();
            this.btnExportToMdb = new System.Windows.Forms.Button();
            this.btnSetObsAccuracy = new System.Windows.Forms.Button();
            this.btnSetArea2_3ObsTypes = new System.Windows.Forms.Button();
            this.btnSetArea1Types = new System.Windows.Forms.Button();
            this.chckBxFromMdb = new System.Windows.Forms.CheckBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnObsRelateOrg = new System.Windows.Forms.Button();
            this.btnAirspace = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 47);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 13);
            label1.TabIndex = 0;
            label1.Text = "Folder:";
            // 
            // ui_folderTB
            // 
            this.ui_folderTB.Location = new System.Drawing.Point(54, 44);
            this.ui_folderTB.Name = "ui_folderTB";
            this.ui_folderTB.Size = new System.Drawing.Size(282, 20);
            this.ui_folderTB.TabIndex = 1;
            this.ui_folderTB.Text = "D:\\Work\\Kaz\\UACC(Astana) - new";
            this.ui_folderTB.TextChanged += new System.EventHandler(this.ui_folderTB_TextChanged);
            this.ui_folderTB.Leave += new System.EventHandler(this.ui_folderTB_Leave);
            // 
            // ui_selectFolderButton
            // 
            this.ui_selectFolderButton.Location = new System.Drawing.Point(342, 42);
            this.ui_selectFolderButton.Name = "ui_selectFolderButton";
            this.ui_selectFolderButton.Size = new System.Drawing.Size(31, 23);
            this.ui_selectFolderButton.TabIndex = 2;
            this.ui_selectFolderButton.Text = "...";
            this.ui_selectFolderButton.UseVisualStyleBackColor = true;
            this.ui_selectFolderButton.Click += new System.EventHandler(this.SelectFolder_Click);
            // 
            // ui_parseRwyButton
            // 
            this.ui_parseRwyButton.Location = new System.Drawing.Point(7, 130);
            this.ui_parseRwyButton.Name = "ui_parseRwyButton";
            this.ui_parseRwyButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseRwyButton.TabIndex = 11;
            this.ui_parseRwyButton.Text = "Runway Centerline Point";
            this.ui_parseRwyButton.UseVisualStyleBackColor = true;
            this.ui_parseRwyButton.Click += new System.EventHandler(this.ParseRwy_Click);
            // 
            // ui_parseVSButton
            // 
            this.ui_parseVSButton.Location = new System.Drawing.Point(6, 102);
            this.ui_parseVSButton.Name = "ui_parseVSButton";
            this.ui_parseVSButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseVSButton.TabIndex = 9;
            this.ui_parseVSButton.Text = "Vertical Structure";
            this.ui_parseVSButton.UseVisualStyleBackColor = true;
            this.ui_parseVSButton.Click += new System.EventHandler(this.ParseVS_Click);
            // 
            // ui_parseApronButton
            // 
            this.ui_parseApronButton.Location = new System.Drawing.Point(6, 159);
            this.ui_parseApronButton.Name = "ui_parseApronButton";
            this.ui_parseApronButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseApronButton.TabIndex = 13;
            this.ui_parseApronButton.Text = "Guidance Line (APRON)";
            this.ui_parseApronButton.UseVisualStyleBackColor = true;
            this.ui_parseApronButton.Click += new System.EventHandler(this.ParseGuidanceLine_Apron_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCntrlPnt);
            this.groupBox1.Controls.Add(this.btnTaxiHoldingPos);
            this.groupBox1.Controls.Add(this.btnRawPoint);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnNavaidComp);
            this.groupBox1.Controls.Add(this.ui_parseSPButton);
            this.groupBox1.Controls.Add(this.ui_parseApronDataButton);
            this.groupBox1.Controls.Add(this.ui_parseSLButton);
            this.groupBox1.Controls.Add(this.ui_parseApronButton);
            this.groupBox1.Controls.Add(this.ui_parseRwyButton);
            this.groupBox1.Controls.Add(this.ui_parseVSButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 308);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parse";
            // 
            // btnCntrlPnt
            // 
            this.btnCntrlPnt.Location = new System.Drawing.Point(6, 276);
            this.btnCntrlPnt.Name = "btnCntrlPnt";
            this.btnCntrlPnt.Size = new System.Drawing.Size(144, 23);
            this.btnCntrlPnt.TabIndex = 18;
            this.btnCntrlPnt.Text = "Control Points";
            this.btnCntrlPnt.UseVisualStyleBackColor = true;
            this.btnCntrlPnt.Click += new System.EventHandler(this.btnCntrlPnt_Click);
            // 
            // btnTaxiHoldingPos
            // 
            this.btnTaxiHoldingPos.Location = new System.Drawing.Point(6, 218);
            this.btnTaxiHoldingPos.Name = "btnTaxiHoldingPos";
            this.btnTaxiHoldingPos.Size = new System.Drawing.Size(144, 23);
            this.btnTaxiHoldingPos.TabIndex = 7;
            this.btnTaxiHoldingPos.Text = "Taxi Holding Position";
            this.btnTaxiHoldingPos.UseVisualStyleBackColor = true;
            this.btnTaxiHoldingPos.Click += new System.EventHandler(this.btnTaxiHoldingPos_Click);
            // 
            // btnRawPoint
            // 
            this.btnRawPoint.Location = new System.Drawing.Point(6, 247);
            this.btnRawPoint.Name = "btnRawPoint";
            this.btnRawPoint.Size = new System.Drawing.Size(144, 23);
            this.btnRawPoint.TabIndex = 17;
            this.btnRawPoint.Text = "Raw Point";
            this.btnRawPoint.UseVisualStyleBackColor = true;
            this.btnRawPoint.Click += new System.EventHandler(this.btnRawPoint_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chckBxSortGuidanceLineTaxiway);
            this.groupBox2.Controls.Add(this.chckBxSortAircraftStand);
            this.groupBox2.Controls.Add(this.chckBxSortRwyCentPoint);
            this.groupBox2.Controls.Add(this.chckBxSortVertStruct);
            this.groupBox2.Controls.Add(this.chckBxSortApronElement);
            this.groupBox2.Controls.Add(this.chckBxSortGuidanceLineApron);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(156, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(50, 308);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sort";
            // 
            // chckBxSortGuidanceLineTaxiway
            // 
            this.chckBxSortGuidanceLineTaxiway.AutoSize = true;
            this.chckBxSortGuidanceLineTaxiway.Location = new System.Drawing.Point(13, 194);
            this.chckBxSortGuidanceLineTaxiway.Name = "chckBxSortGuidanceLineTaxiway";
            this.chckBxSortGuidanceLineTaxiway.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortGuidanceLineTaxiway.TabIndex = 16;
            this.chckBxSortGuidanceLineTaxiway.UseVisualStyleBackColor = true;
            // 
            // chckBxSortAircraftStand
            // 
            this.chckBxSortAircraftStand.AutoSize = true;
            this.chckBxSortAircraftStand.Location = new System.Drawing.Point(13, 77);
            this.chckBxSortAircraftStand.Name = "chckBxSortAircraftStand";
            this.chckBxSortAircraftStand.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortAircraftStand.TabIndex = 6;
            this.chckBxSortAircraftStand.UseVisualStyleBackColor = true;
            // 
            // chckBxSortRwyCentPoint
            // 
            this.chckBxSortRwyCentPoint.AutoSize = true;
            this.chckBxSortRwyCentPoint.Location = new System.Drawing.Point(13, 135);
            this.chckBxSortRwyCentPoint.Name = "chckBxSortRwyCentPoint";
            this.chckBxSortRwyCentPoint.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortRwyCentPoint.TabIndex = 12;
            this.chckBxSortRwyCentPoint.UseVisualStyleBackColor = true;
            // 
            // chckBxSortVertStruct
            // 
            this.chckBxSortVertStruct.AutoSize = true;
            this.chckBxSortVertStruct.Location = new System.Drawing.Point(13, 106);
            this.chckBxSortVertStruct.Name = "chckBxSortVertStruct";
            this.chckBxSortVertStruct.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortVertStruct.TabIndex = 10;
            this.chckBxSortVertStruct.UseVisualStyleBackColor = true;
            this.chckBxSortVertStruct.Visible = false;
            // 
            // chckBxSortApronElement
            // 
            this.chckBxSortApronElement.AutoSize = true;
            this.chckBxSortApronElement.Location = new System.Drawing.Point(13, 49);
            this.chckBxSortApronElement.Name = "chckBxSortApronElement";
            this.chckBxSortApronElement.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortApronElement.TabIndex = 8;
            this.chckBxSortApronElement.UseVisualStyleBackColor = true;
            // 
            // chckBxSortGuidanceLineApron
            // 
            this.chckBxSortGuidanceLineApron.AutoSize = true;
            this.chckBxSortGuidanceLineApron.Location = new System.Drawing.Point(13, 164);
            this.chckBxSortGuidanceLineApron.Name = "chckBxSortGuidanceLineApron";
            this.chckBxSortGuidanceLineApron.Size = new System.Drawing.Size(15, 14);
            this.chckBxSortGuidanceLineApron.TabIndex = 14;
            this.chckBxSortGuidanceLineApron.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // btnNavaidComp
            // 
            this.btnNavaidComp.Location = new System.Drawing.Point(6, 15);
            this.btnNavaidComp.Name = "btnNavaidComp";
            this.btnNavaidComp.Size = new System.Drawing.Size(144, 23);
            this.btnNavaidComp.TabIndex = 3;
            this.btnNavaidComp.Text = "Navaid Component";
            this.btnNavaidComp.UseVisualStyleBackColor = true;
            this.btnNavaidComp.Click += new System.EventHandler(this.btnNavaid_Click);
            // 
            // ui_parseSPButton
            // 
            this.ui_parseSPButton.Location = new System.Drawing.Point(6, 72);
            this.ui_parseSPButton.Name = "ui_parseSPButton";
            this.ui_parseSPButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseSPButton.TabIndex = 5;
            this.ui_parseSPButton.Text = "AircraftStand";
            this.ui_parseSPButton.UseVisualStyleBackColor = true;
            this.ui_parseSPButton.Click += new System.EventHandler(this.ParseSP_Click);
            // 
            // ui_parseApronDataButton
            // 
            this.ui_parseApronDataButton.Location = new System.Drawing.Point(6, 44);
            this.ui_parseApronDataButton.Name = "ui_parseApronDataButton";
            this.ui_parseApronDataButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseApronDataButton.TabIndex = 7;
            this.ui_parseApronDataButton.Text = "Apron Elements";
            this.ui_parseApronDataButton.UseVisualStyleBackColor = true;
            this.ui_parseApronDataButton.Click += new System.EventHandler(this.ParseApronData_Click);
            // 
            // ui_parseSLButton
            // 
            this.ui_parseSLButton.Location = new System.Drawing.Point(6, 189);
            this.ui_parseSLButton.Name = "ui_parseSLButton";
            this.ui_parseSLButton.Size = new System.Drawing.Size(144, 23);
            this.ui_parseSLButton.TabIndex = 15;
            this.ui_parseSLButton.Text = "Guidance Line(TAXIWAY)";
            this.ui_parseSLButton.UseVisualStyleBackColor = true;
            this.ui_parseSLButton.Click += new System.EventHandler(this.ParseTaxiwayGuidanceLine_Click);
            // 
            // chckBxInsert2DB
            // 
            this.chckBxInsert2DB.AutoSize = true;
            this.chckBxInsert2DB.Location = new System.Drawing.Point(230, 128);
            this.chckBxInsert2DB.Name = "chckBxInsert2DB";
            this.chckBxInsert2DB.Size = new System.Drawing.Size(82, 17);
            this.chckBxInsert2DB.TabIndex = 8;
            this.chckBxInsert2DB.Text = "Insert to DB";
            this.chckBxInsert2DB.UseVisualStyleBackColor = true;
            this.chckBxInsert2DB.CheckedChanged += new System.EventHandler(this.chckBxInsert2DB_CheckedChanged);
            // 
            // btnEtod
            // 
            this.btnEtod.Location = new System.Drawing.Point(6, 21);
            this.btnEtod.Name = "btnEtod";
            this.btnEtod.Size = new System.Drawing.Size(112, 23);
            this.btnEtod.TabIndex = 9;
            this.btnEtod.Text = "eTOD";
            this.btnEtod.UseVisualStyleBackColor = true;
            this.btnEtod.Click += new System.EventHandler(this.btn_Click);
            // 
            // chckBxIsGDB
            // 
            this.chckBxIsGDB.AutoSize = true;
            this.chckBxIsGDB.Location = new System.Drawing.Point(230, 149);
            this.chckBxIsGDB.Name = "chckBxIsGDB";
            this.chckBxIsGDB.Size = new System.Drawing.Size(93, 17);
            this.chckBxIsGDB.TabIndex = 10;
            this.chckBxIsGDB.Text = "GeoDataBase";
            this.chckBxIsGDB.UseVisualStyleBackColor = true;
            // 
            // chckBxReplaceDecimalWithDot
            // 
            this.chckBxReplaceDecimalWithDot.Location = new System.Drawing.Point(230, 98);
            this.chckBxReplaceDecimalWithDot.Name = "chckBxReplaceDecimalWithDot";
            this.chckBxReplaceDecimalWithDot.Size = new System.Drawing.Size(112, 30);
            this.chckBxReplaceDecimalWithDot.TabIndex = 11;
            this.chckBxReplaceDecimalWithDot.Text = "Replace decimal separator with dot";
            this.chckBxReplaceDecimalWithDot.UseVisualStyleBackColor = true;
            // 
            // btnArea2VertStructs
            // 
            this.btnArea2VertStructs.Location = new System.Drawing.Point(6, 50);
            this.btnArea2VertStructs.Name = "btnArea2VertStructs";
            this.btnArea2VertStructs.Size = new System.Drawing.Size(112, 23);
            this.btnArea2VertStructs.TabIndex = 12;
            this.btnArea2VertStructs.Text = "Area 2 and 3";
            this.btnArea2VertStructs.UseVisualStyleBackColor = true;
            this.btnArea2VertStructs.Click += new System.EventHandler(this.btnArea2VertStructs_Click);
            // 
            // btnArea1
            // 
            this.btnArea1.Location = new System.Drawing.Point(6, 79);
            this.btnArea1.Name = "btnArea1";
            this.btnArea1.Size = new System.Drawing.Size(112, 23);
            this.btnArea1.TabIndex = 13;
            this.btnArea1.Text = "Area 1 ";
            this.btnArea1.UseVisualStyleBackColor = true;
            this.btnArea1.Click += new System.EventHandler(this.btnArea1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chckBxArea1ObsFromExcel);
            this.groupBox3.Controls.Add(this.btnExportToMdb);
            this.groupBox3.Controls.Add(this.btnSetObsAccuracy);
            this.groupBox3.Controls.Add(this.btnSetArea2_3ObsTypes);
            this.groupBox3.Controls.Add(this.btnSetArea1Types);
            this.groupBox3.Controls.Add(this.btnArea1);
            this.groupBox3.Controls.Add(this.btnArea2VertStructs);
            this.groupBox3.Controls.Add(this.btnEtod);
            this.groupBox3.Location = new System.Drawing.Point(224, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(198, 236);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Obstacles";
            // 
            // chckBxArea1ObsFromExcel
            // 
            this.chckBxArea1ObsFromExcel.AutoSize = true;
            this.chckBxArea1ObsFromExcel.Location = new System.Drawing.Point(119, 83);
            this.chckBxArea1ObsFromExcel.Name = "chckBxArea1ObsFromExcel";
            this.chckBxArea1ObsFromExcel.Size = new System.Drawing.Size(78, 17);
            this.chckBxArea1ObsFromExcel.TabIndex = 18;
            this.chckBxArea1ObsFromExcel.Text = "From Excel";
            this.chckBxArea1ObsFromExcel.UseVisualStyleBackColor = true;
            // 
            // btnExportToMdb
            // 
            this.btnExportToMdb.Location = new System.Drawing.Point(6, 209);
            this.btnExportToMdb.Name = "btnExportToMdb";
            this.btnExportToMdb.Size = new System.Drawing.Size(112, 23);
            this.btnExportToMdb.TabIndex = 17;
            this.btnExportToMdb.Text = "Export to arcmap";
            this.btnExportToMdb.UseVisualStyleBackColor = true;
            this.btnExportToMdb.Click += new System.EventHandler(this.btnExportToMdb_Click);
            // 
            // btnSetObsAccuracy
            // 
            this.btnSetObsAccuracy.Location = new System.Drawing.Point(6, 180);
            this.btnSetObsAccuracy.Name = "btnSetObsAccuracy";
            this.btnSetObsAccuracy.Size = new System.Drawing.Size(112, 23);
            this.btnSetObsAccuracy.TabIndex = 16;
            this.btnSetObsAccuracy.Text = "Set Accuracy";
            this.btnSetObsAccuracy.UseVisualStyleBackColor = true;
            this.btnSetObsAccuracy.Click += new System.EventHandler(this.btnSetObsAccuracy_Click);
            // 
            // btnSetArea2_3ObsTypes
            // 
            this.btnSetArea2_3ObsTypes.Location = new System.Drawing.Point(6, 137);
            this.btnSetArea2_3ObsTypes.Name = "btnSetArea2_3ObsTypes";
            this.btnSetArea2_3ObsTypes.Size = new System.Drawing.Size(112, 37);
            this.btnSetArea2_3ObsTypes.TabIndex = 15;
            this.btnSetArea2_3ObsTypes.Text = "Set Area 2 and 3 Types";
            this.btnSetArea2_3ObsTypes.UseVisualStyleBackColor = true;
            this.btnSetArea2_3ObsTypes.Click += new System.EventHandler(this.btnSetArea2_3ObsTypes_Click);
            // 
            // btnSetArea1Types
            // 
            this.btnSetArea1Types.Location = new System.Drawing.Point(6, 108);
            this.btnSetArea1Types.Name = "btnSetArea1Types";
            this.btnSetArea1Types.Size = new System.Drawing.Size(112, 23);
            this.btnSetArea1Types.TabIndex = 14;
            this.btnSetArea1Types.Text = "Set Area 1 Types";
            this.btnSetArea1Types.UseVisualStyleBackColor = true;
            this.btnSetArea1Types.Click += new System.EventHandler(this.btnSetArea1Types_Click);
            // 
            // chckBxFromMdb
            // 
            this.chckBxFromMdb.AutoSize = true;
            this.chckBxFromMdb.Location = new System.Drawing.Point(18, 72);
            this.chckBxFromMdb.Name = "chckBxFromMdb";
            this.chckBxFromMdb.Size = new System.Drawing.Size(76, 17);
            this.chckBxFromMdb.TabIndex = 15;
            this.chckBxFromMdb.Text = "From MDB";
            this.chckBxFromMdb.UseVisualStyleBackColor = true;
            this.chckBxFromMdb.CheckedChanged += new System.EventHandler(this.chckBxFromMdb_CheckedChanged);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(18, 12);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(355, 23);
            this.btnConvert.TabIndex = 16;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnObsRelateOrg
            // 
            this.btnObsRelateOrg.Location = new System.Drawing.Point(19, 409);
            this.btnObsRelateOrg.Name = "btnObsRelateOrg";
            this.btnObsRelateOrg.Size = new System.Drawing.Size(143, 23);
            this.btnObsRelateOrg.TabIndex = 17;
            this.btnObsRelateOrg.Text = "Connect to organistation";
            this.btnObsRelateOrg.UseVisualStyleBackColor = true;
            this.btnObsRelateOrg.Click += new System.EventHandler(this.btnObsRelateOrg_Click);
            // 
            // btnAirspace
            // 
            this.btnAirspace.Location = new System.Drawing.Point(230, 409);
            this.btnAirspace.Name = "btnAirspace";
            this.btnAirspace.Size = new System.Drawing.Size(129, 23);
            this.btnAirspace.TabIndex = 18;
            this.btnAirspace.Text = "Get airspaces from gdb";
            this.btnAirspace.UseVisualStyleBackColor = true;
            this.btnAirspace.Click += new System.EventHandler(this.btnAirspace_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 461);
            this.Controls.Add(this.btnAirspace);
            this.Controls.Add(this.btnObsRelateOrg);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.chckBxFromMdb);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chckBxReplaceDecimalWithDot);
            this.Controls.Add(this.chckBxIsGDB);
            this.Controls.Add(this.chckBxInsert2DB);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ui_selectFolderButton);
            this.Controls.Add(this.ui_folderTB);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Geo importer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_folderTB;
        private System.Windows.Forms.Button ui_selectFolderButton;
        private System.Windows.Forms.Button ui_parseRwyButton;
        private System.Windows.Forms.Button ui_parseVSButton;
        private System.Windows.Forms.Button ui_parseApronButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ui_parseApronDataButton;
        private System.Windows.Forms.Button ui_parseSPButton;
        private System.Windows.Forms.Button ui_parseSLButton;
		private System.Windows.Forms.Button btnNavaidComp;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chckBxSortGuidanceLineTaxiway;
		private System.Windows.Forms.CheckBox chckBxSortAircraftStand;
		private System.Windows.Forms.CheckBox chckBxSortRwyCentPoint;
		private System.Windows.Forms.CheckBox chckBxSortVertStruct;
		private System.Windows.Forms.CheckBox chckBxSortApronElement;
		private System.Windows.Forms.CheckBox chckBxSortGuidanceLineApron;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button btnRawPoint;
		private System.Windows.Forms.Button btnCntrlPnt;
		private System.Windows.Forms.Button btnTaxiHoldingPos;
		private System.Windows.Forms.CheckBox chckBxInsert2DB;
		private System.Windows.Forms.Button btnEtod;
		private System.Windows.Forms.CheckBox chckBxIsGDB;
		private System.Windows.Forms.CheckBox chckBxReplaceDecimalWithDot;
		private System.Windows.Forms.Button btnArea2VertStructs;
		private System.Windows.Forms.Button btnArea1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnSetArea1Types;
		private System.Windows.Forms.Button btnSetArea2_3ObsTypes;
		private System.Windows.Forms.Button btnSetObsAccuracy;
		private System.Windows.Forms.Button btnExportToMdb;
		private System.Windows.Forms.CheckBox chckBxFromMdb;
		private System.Windows.Forms.Button btnConvert;
		private System.Windows.Forms.Button btnObsRelateOrg;
		private System.Windows.Forms.CheckBox chckBxArea1ObsFromExcel;
		private System.Windows.Forms.Button btnAirspace;
    }
}

