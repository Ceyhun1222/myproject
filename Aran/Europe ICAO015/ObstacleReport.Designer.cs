namespace ICAO015
{

    partial class ObstacleReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObstacleReport));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GridObstclDataForDME = new System.Windows.Forms.DataGridView();
            this.GrpBoxSearchParam = new System.Windows.Forms.GroupBox();
            this.LblIlsRowCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblDmeRowCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GridObstclDataForILS = new System.Windows.Forms.DataGridView();
            this.triStateTreeView1 = new ICAO015.TriStateTreeView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclDataForDME)).BeginInit();
            this.GrpBoxSearchParam.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclDataForILS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.triStateTreeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 467);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Checked Navaid";
            // 
            // GridObstclDataForDME
            // 
            this.GridObstclDataForDME.AllowUserToAddRows = false;
            this.GridObstclDataForDME.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridObstclDataForDME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridObstclDataForDME.Location = new System.Drawing.Point(3, 3);
            this.GridObstclDataForDME.Name = "GridObstclDataForDME";
            this.GridObstclDataForDME.ReadOnly = true;
            this.GridObstclDataForDME.Size = new System.Drawing.Size(710, 435);
            this.GridObstclDataForDME.TabIndex = 0;
            this.GridObstclDataForDME.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridObstclDataForDME_ColumnHeaderMouseClick);
            // 
            // GrpBoxSearchParam
            // 
            this.GrpBoxSearchParam.Controls.Add(this.LblIlsRowCount);
            this.GrpBoxSearchParam.Controls.Add(this.label2);
            this.GrpBoxSearchParam.Controls.Add(this.LblDmeRowCount);
            this.GrpBoxSearchParam.Controls.Add(this.label1);
            this.GrpBoxSearchParam.Location = new System.Drawing.Point(15, 480);
            this.GrpBoxSearchParam.Name = "GrpBoxSearchParam";
            this.GrpBoxSearchParam.Size = new System.Drawing.Size(1020, 100);
            this.GrpBoxSearchParam.TabIndex = 3;
            this.GrpBoxSearchParam.TabStop = false;
            this.GrpBoxSearchParam.Text = "Search Parameters";
            this.GrpBoxSearchParam.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // LblIlsRowCount
            // 
            this.LblIlsRowCount.AutoSize = true;
            this.LblIlsRowCount.Location = new System.Drawing.Point(101, 55);
            this.LblIlsRowCount.Name = "LblIlsRowCount";
            this.LblIlsRowCount.Size = new System.Drawing.Size(34, 13);
            this.LblIlsRowCount.TabIndex = 3;
            this.LblIlsRowCount.Text = "count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ILS Row Count:";
            // 
            // LblDmeRowCount
            // 
            this.LblDmeRowCount.AutoSize = true;
            this.LblDmeRowCount.Location = new System.Drawing.Point(101, 22);
            this.LblDmeRowCount.Name = "LblDmeRowCount";
            this.LblDmeRowCount.Size = new System.Drawing.Size(34, 13);
            this.LblDmeRowCount.TabIndex = 1;
            this.LblDmeRowCount.Text = "count";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DME Row Count:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "foldr.png");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(311, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(724, 467);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.GridObstclDataForDME);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(716, 441);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DME";
            this.tabPage1.ToolTipText = "This is only for DME";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GridObstclDataForILS);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(716, 441);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ILS";
            this.tabPage2.ToolTipText = "This is only for ILS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GridObstclDataForILS
            // 
            this.GridObstclDataForILS.AllowUserToAddRows = false;
            this.GridObstclDataForILS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridObstclDataForILS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridObstclDataForILS.Location = new System.Drawing.Point(3, 3);
            this.GridObstclDataForILS.Name = "GridObstclDataForILS";
            this.GridObstclDataForILS.ReadOnly = true;
            this.GridObstclDataForILS.Size = new System.Drawing.Size(710, 435);
            this.GridObstclDataForILS.TabIndex = 0;
            this.GridObstclDataForILS.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridObstclDataForILS_ColumnHeaderMouseClick);
            // 
            // triStateTreeView1
            // 
            this.triStateTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triStateTreeView1.Location = new System.Drawing.Point(3, 16);
            this.triStateTreeView1.Name = "triStateTreeView1";
            this.triStateTreeView1.Size = new System.Drawing.Size(287, 448);
            this.triStateTreeView1.TabIndex = 0;
            this.triStateTreeView1.TriStateStyleProperty = ICAO015.TriStateTreeView.TriStateStyles.Installer;
            this.triStateTreeView1.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.triStateTreeView1_BeforeCheck);
            this.triStateTreeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.triStateTreeView1_AfterCheck);
            this.triStateTreeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.triStateTreeView1_BeforeExpand);
            this.triStateTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.triStateTreeView1_AfterSelect);
            // 
            // ObstacleReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(1038, 592);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.GrpBoxSearchParam);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ObstacleReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Obstacle Report";
            this.Load += new System.EventHandler(this.ObstacleReport_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclDataForDME)).EndInit();
            this.GrpBoxSearchParam.ResumeLayout(false);
            this.GrpBoxSearchParam.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclDataForILS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ICAO015.TriStateTreeView triStateTreeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox GrpBoxSearchParam;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridView GridObstclDataForDME;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView GridObstclDataForILS;
        private System.Windows.Forms.Label LblDmeRowCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblIlsRowCount;
        private System.Windows.Forms.Label label2;
    }



}