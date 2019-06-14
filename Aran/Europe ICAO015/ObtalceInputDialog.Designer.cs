namespace ICAO015
{
    partial class ObtalceInputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObtalceInputDialog));
            this.pointPicker1 = new ChoosePointNS.PointPicker();
            this.TxtBoxElevation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnObstclInptCalc = new System.Windows.Forms.Button();
            this.GridObstclInptReportForDME = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GridObstclInptReportFor_ILS = new System.Windows.Forms.DataGridView();
            this.LblObstacleCalcTxt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclInptReportForDME)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclInptReportFor_ILS)).BeginInit();
            this.SuspendLayout();
            // 
            // pointPicker1
            // 
            this.pointPicker1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pointPicker1.ByClick = false;
            this.pointPicker1.DDAccuracy = 4;
            this.pointPicker1.DMSAccuracy = 2;
            this.pointPicker1.IsDD = true;
            this.pointPicker1.Latitude = 0D;
            this.pointPicker1.Location = new System.Drawing.Point(238, 12);
            this.pointPicker1.Longitude = 0D;
            this.pointPicker1.Name = "pointPicker1";
            this.pointPicker1.Size = new System.Drawing.Size(262, 111);
            this.pointPicker1.TabIndex = 0;
            this.pointPicker1.ByClickChanged += new System.EventHandler(this.pointPicker1_ByClickChanged);
            // 
            // TxtBoxElevation
            // 
            this.TxtBoxElevation.Location = new System.Drawing.Point(238, 141);
            this.TxtBoxElevation.Name = "TxtBoxElevation";
            this.TxtBoxElevation.Size = new System.Drawing.Size(138, 20);
            this.TxtBoxElevation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(165, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Elevation:";
            // 
            // BtnObstclInptCalc
            // 
            this.BtnObstclInptCalc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnObstclInptCalc.Location = new System.Drawing.Point(395, 128);
            this.BtnObstclInptCalc.Name = "BtnObstclInptCalc";
            this.BtnObstclInptCalc.Size = new System.Drawing.Size(105, 33);
            this.BtnObstclInptCalc.TabIndex = 3;
            this.BtnObstclInptCalc.Text = "Calculator";
            this.BtnObstclInptCalc.UseVisualStyleBackColor = true;
            this.BtnObstclInptCalc.Click += new System.EventHandler(this.BtnObstclInptCalc_Click);
            // 
            // GridObstclInptReportForDME
            // 
            this.GridObstclInptReportForDME.AllowUserToAddRows = false;
            this.GridObstclInptReportForDME.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridObstclInptReportForDME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridObstclInptReportForDME.Location = new System.Drawing.Point(3, 3);
            this.GridObstclInptReportForDME.Name = "GridObstclInptReportForDME";
            this.GridObstclInptReportForDME.ReadOnly = true;
            this.GridObstclInptReportForDME.Size = new System.Drawing.Size(656, 212);
            this.GridObstclInptReportForDME.TabIndex = 4;
            this.GridObstclInptReportForDME.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridObstclInptReportForDME_ColumnHeaderMouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 190);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(670, 244);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.GridObstclInptReportForDME);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(662, 218);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DME";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GridObstclInptReportFor_ILS);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(662, 218);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ILS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GridObstclInptReportFor_ILS
            // 
            this.GridObstclInptReportFor_ILS.AllowUserToAddRows = false;
            this.GridObstclInptReportFor_ILS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridObstclInptReportFor_ILS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridObstclInptReportFor_ILS.Location = new System.Drawing.Point(3, 3);
            this.GridObstclInptReportFor_ILS.Name = "GridObstclInptReportFor_ILS";
            this.GridObstclInptReportFor_ILS.ReadOnly = true;
            this.GridObstclInptReportFor_ILS.Size = new System.Drawing.Size(656, 212);
            this.GridObstclInptReportFor_ILS.TabIndex = 0;
            this.GridObstclInptReportFor_ILS.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridObstclInptReportFor_ILS_ColumnHeaderMouseClick);
            // 
            // LblObstacleCalcTxt
            // 
            this.LblObstacleCalcTxt.AutoSize = true;
            this.LblObstacleCalcTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblObstacleCalcTxt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.LblObstacleCalcTxt.Location = new System.Drawing.Point(7, 168);
            this.LblObstacleCalcTxt.Name = "LblObstacleCalcTxt";
            this.LblObstacleCalcTxt.Size = new System.Drawing.Size(98, 15);
            this.LblObstacleCalcTxt.TabIndex = 6;
            this.LblObstacleCalcTxt.Text = "ObstacleInputTxt";
            this.LblObstacleCalcTxt.Visible = false;
            // 
            // ObtalceInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(674, 437);
            this.Controls.Add(this.LblObstacleCalcTxt);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BtnObstclInptCalc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtBoxElevation);
            this.Controls.Add(this.pointPicker1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ObtalceInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Obstacle Input Calculate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObtalceInputDialog_FormClosing);
            this.Load += new System.EventHandler(this.ObtalceInputDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclInptReportForDME)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridObstclInptReportFor_ILS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoosePointNS.PointPicker pointPicker1;
        private System.Windows.Forms.TextBox TxtBoxElevation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnObstclInptCalc;
        private System.Windows.Forms.DataGridView GridObstclInptReportForDME;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView GridObstclInptReportFor_ILS;
        private System.Windows.Forms.Label LblObstacleCalcTxt;
    }
}