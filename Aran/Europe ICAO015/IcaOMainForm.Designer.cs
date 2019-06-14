namespace Europe_ICAO015
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnCalculate = new System.Windows.Forms.Button();
            this.TrwViewNavaids = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnClear = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LblLoadingTxt = new System.Windows.Forms.Label();
            this.BtnObstcInput = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ObjComboRwyDirList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DatGridVRunwayList = new System.Windows.Forms.DataGridView();
            this.GroupBoxRunwayList = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.BtnHelp = new System.Windows.Forms.Button();
            this.ChkBoxWindTurbine = new System.Windows.Forms.CheckBox();
            this.TooltipForWindTurbine = new System.Windows.Forms.ToolTip(this.components);
            this.TabNavaidControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DGridVNvidsParameters = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DatGridILSParameters = new System.Windows.Forms.DataGridView();
            this.backgroundWorkeprogres = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DatGridVRunwayList)).BeginInit();
            this.GroupBoxRunwayList.SuspendLayout();
            this.TabNavaidControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGridVNvidsParameters)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DatGridILSParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnCalculate
            // 
            this.BtnCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnCalculate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnCalculate.Location = new System.Drawing.Point(465, 438);
            this.BtnCalculate.Name = "BtnCalculate";
            this.BtnCalculate.Size = new System.Drawing.Size(142, 33);
            this.BtnCalculate.TabIndex = 1;
            this.BtnCalculate.Text = "Calculate";
            this.BtnCalculate.UseVisualStyleBackColor = true;
            this.BtnCalculate.Click += new System.EventHandler(this.BtnCalculate_Click);
            // 
            // TrwViewNavaids
            // 
            this.TrwViewNavaids.BackColor = System.Drawing.SystemColors.Control;
            this.TrwViewNavaids.CheckBoxes = true;
            this.TrwViewNavaids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrwViewNavaids.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TrwViewNavaids.ImeMode = System.Windows.Forms.ImeMode.On;
            this.TrwViewNavaids.Location = new System.Drawing.Point(3, 17);
            this.TrwViewNavaids.Name = "TrwViewNavaids";
            this.TrwViewNavaids.Size = new System.Drawing.Size(269, 400);
            this.TrwViewNavaids.TabIndex = 5;
            this.TrwViewNavaids.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TrwViewNavaids_AfterCheck);
            this.TrwViewNavaids.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TrwViewNavaids_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TrwViewNavaids);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(4, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 420);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Navaids";
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnClear.Location = new System.Drawing.Point(4, 438);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(272, 33);
            this.BtnClear.TabIndex = 9;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LblLoadingTxt
            // 
            this.LblLoadingTxt.AutoSize = true;
            this.LblLoadingTxt.Location = new System.Drawing.Point(6, -1);
            this.LblLoadingTxt.Name = "LblLoadingTxt";
            this.LblLoadingTxt.Size = new System.Drawing.Size(63, 13);
            this.LblLoadingTxt.TabIndex = 12;
            this.LblLoadingTxt.Text = "Loading .....";
            this.LblLoadingTxt.Visible = false;
            // 
            // BtnObstcInput
            // 
            this.BtnObstcInput.Enabled = false;
            this.BtnObstcInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnObstcInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnObstcInput.Location = new System.Drawing.Point(694, 438);
            this.BtnObstcInput.Name = "BtnObstcInput";
            this.BtnObstcInput.Size = new System.Drawing.Size(141, 33);
            this.BtnObstcInput.TabIndex = 14;
            this.BtnObstcInput.Text = "Obstacle Input";
            this.BtnObstcInput.UseVisualStyleBackColor = true;
            this.BtnObstcInput.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "if_folder_36059.png");
            this.imageList1.Images.SetKeyName(1, "foldr.ico");
            // 
            // ObjComboRwyDirList
            // 
            this.ObjComboRwyDirList.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ObjComboRwyDirList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ObjComboRwyDirList.FormattingEnabled = true;
            this.ObjComboRwyDirList.Location = new System.Drawing.Point(417, 181);
            this.ObjComboRwyDirList.Name = "ObjComboRwyDirList";
            this.ObjComboRwyDirList.Size = new System.Drawing.Size(142, 21);
            this.ObjComboRwyDirList.TabIndex = 31;
            this.ObjComboRwyDirList.SelectedIndexChanged += new System.EventHandler(this.ObjComboRwyDirList_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(283, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 15);
            this.label6.TabIndex = 32;
            this.label6.Text = "Runway Direction List:";
            // 
            // DatGridVRunwayList
            // 
            this.DatGridVRunwayList.AllowUserToAddRows = false;
            this.DatGridVRunwayList.AllowUserToOrderColumns = true;
            this.DatGridVRunwayList.AllowUserToResizeColumns = false;
            this.DatGridVRunwayList.AllowUserToResizeRows = false;
            this.DatGridVRunwayList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DatGridVRunwayList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DatGridVRunwayList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DatGridVRunwayList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DatGridVRunwayList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DatGridVRunwayList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DatGridVRunwayList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DatGridVRunwayList.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DatGridVRunwayList.DefaultCellStyle = dataGridViewCellStyle4;
            this.DatGridVRunwayList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DatGridVRunwayList.Location = new System.Drawing.Point(3, 17);
            this.DatGridVRunwayList.Name = "DatGridVRunwayList";
            this.DatGridVRunwayList.ReadOnly = true;
            this.DatGridVRunwayList.RowHeadersVisible = false;
            this.DatGridVRunwayList.RowTemplate.Height = 40;
            this.DatGridVRunwayList.RowTemplate.ReadOnly = true;
            this.DatGridVRunwayList.Size = new System.Drawing.Size(619, 130);
            this.DatGridVRunwayList.TabIndex = 33;
            this.DatGridVRunwayList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DatGridVRunwayList_CellClick);
            this.DatGridVRunwayList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DatGridVRunwayList_CellContentClick);
            this.DatGridVRunwayList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DatGridVRunwayList_CellMouseClick);
            this.DatGridVRunwayList.Sorted += new System.EventHandler(this.DatGridVRunwayList_Sorted);
            // 
            // GroupBoxRunwayList
            // 
            this.GroupBoxRunwayList.Controls.Add(this.DatGridVRunwayList);
            this.GroupBoxRunwayList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxRunwayList.Location = new System.Drawing.Point(285, 12);
            this.GroupBoxRunwayList.Name = "GroupBoxRunwayList";
            this.GroupBoxRunwayList.Size = new System.Drawing.Size(625, 150);
            this.GroupBoxRunwayList.TabIndex = 33;
            this.GroupBoxRunwayList.TabStop = false;
            this.GroupBoxRunwayList.Text = "RunwayList";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.progressBar1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.progressBar1.Location = new System.Drawing.Point(75, -1);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(843, 10);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 37;
            this.progressBar1.Visible = false;
            // 
            // BtnHelp
            // 
            this.BtnHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnHelp.Location = new System.Drawing.Point(285, 438);
            this.BtnHelp.Name = "BtnHelp";
            this.BtnHelp.Size = new System.Drawing.Size(78, 33);
            this.BtnHelp.TabIndex = 34;
            this.BtnHelp.Text = "Help";
            this.BtnHelp.UseVisualStyleBackColor = true;
            // 
            // ChkBoxWindTurbine
            // 
            this.ChkBoxWindTurbine.AutoSize = true;
            this.ChkBoxWindTurbine.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ChkBoxWindTurbine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChkBoxWindTurbine.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ChkBoxWindTurbine.Image = ((System.Drawing.Image)(resources.GetObject("ChkBoxWindTurbine.Image")));
            this.ChkBoxWindTurbine.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkBoxWindTurbine.Location = new System.Drawing.Point(694, 171);
            this.ChkBoxWindTurbine.Name = "ChkBoxWindTurbine";
            this.ChkBoxWindTurbine.Size = new System.Drawing.Size(141, 48);
            this.ChkBoxWindTurbine.TabIndex = 35;
            this.ChkBoxWindTurbine.Text = "Wind Turbine              ";
            this.ChkBoxWindTurbine.UseVisualStyleBackColor = false;
            this.ChkBoxWindTurbine.CheckedChanged += new System.EventHandler(this.ChkBoxWindTurbine_CheckedChanged);
            this.ChkBoxWindTurbine.MouseLeave += new System.EventHandler(this.ChkBoxWindTurbine_MouseLeave);
            this.ChkBoxWindTurbine.MouseHover += new System.EventHandler(this.ChkBoxWindTurbine_MouseHover);
            // 
            // TooltipForWindTurbine
            // 
            this.TooltipForWindTurbine.BackColor = System.Drawing.Color.DimGray;
            this.TooltipForWindTurbine.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            // 
            // TabNavaidControl
            // 
            this.TabNavaidControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.TabNavaidControl.Controls.Add(this.tabPage1);
            this.TabNavaidControl.Controls.Add(this.tabPage2);
            this.TabNavaidControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TabNavaidControl.Location = new System.Drawing.Point(288, 244);
            this.TabNavaidControl.Multiline = true;
            this.TabNavaidControl.Name = "TabNavaidControl";
            this.TabNavaidControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TabNavaidControl.SelectedIndex = 0;
            this.TabNavaidControl.Size = new System.Drawing.Size(619, 185);
            this.TabNavaidControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabNavaidControl.TabIndex = 36;
            this.TabNavaidControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabNavaidControl_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DGridVNvidsParameters);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(611, 159);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DME";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DGridVNvidsParameters
            // 
            this.DGridVNvidsParameters.AllowUserToAddRows = false;
            this.DGridVNvidsParameters.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGridVNvidsParameters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGridVNvidsParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGridVNvidsParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGridVNvidsParameters.GridColor = System.Drawing.SystemColors.Control;
            this.DGridVNvidsParameters.Location = new System.Drawing.Point(3, 3);
            this.DGridVNvidsParameters.Name = "DGridVNvidsParameters";
            this.DGridVNvidsParameters.ReadOnly = true;
            this.DGridVNvidsParameters.Size = new System.Drawing.Size(605, 153);
            this.DGridVNvidsParameters.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DatGridILSParameters);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(611, 159);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ILS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DatGridILSParameters
            // 
            this.DatGridILSParameters.AllowUserToAddRows = false;
            this.DatGridILSParameters.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.DatGridILSParameters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DatGridILSParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DatGridILSParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DatGridILSParameters.Location = new System.Drawing.Point(3, 3);
            this.DatGridILSParameters.Name = "DatGridILSParameters";
            this.DatGridILSParameters.ReadOnly = true;
            this.DatGridILSParameters.Size = new System.Drawing.Size(605, 153);
            this.DatGridILSParameters.TabIndex = 0;
            // 
            // backgroundWorkeprogres
            // 
            this.backgroundWorkeprogres.WorkerReportsProgress = true;
            this.backgroundWorkeprogres.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkeprogres_DoWork);
            this.backgroundWorkeprogres.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkeprogres_ProgressChanged);
            this.backgroundWorkeprogres.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkeprogres_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(913, 476);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.TabNavaidControl);
            this.Controls.Add(this.ChkBoxWindTurbine);
            this.Controls.Add(this.BtnHelp);
            this.Controls.Add(this.GroupBoxRunwayList);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ObjComboRwyDirList);
            this.Controls.Add(this.BtnObstcInput);
            this.Controls.Add(this.LblLoadingTxt);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnCalculate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DatGridVRunwayList)).EndInit();
            this.GroupBoxRunwayList.ResumeLayout(false);
            this.TabNavaidControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGridVNvidsParameters)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DatGridILSParameters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnCalculate;
        private System.Windows.Forms.TreeView TrwViewNavaids;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnClear;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LblLoadingTxt;
        private System.Windows.Forms.Button BtnObstcInput;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox ObjComboRwyDirList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView DatGridVRunwayList;
        private System.Windows.Forms.GroupBox GroupBoxRunwayList;
        private System.Windows.Forms.Button BtnHelp;
        private System.Windows.Forms.CheckBox ChkBoxWindTurbine;
        private System.Windows.Forms.ToolTip TooltipForWindTurbine;
        private System.Windows.Forms.TabControl TabNavaidControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView DGridVNvidsParameters;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView DatGridILSParameters;
        private System.ComponentModel.BackgroundWorker backgroundWorkeprogres;
        private System.Windows.Forms.ProgressBar progressBar1;
        //private ChoosePointNS.PointPicker pointPicker1;
    }
}

