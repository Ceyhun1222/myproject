namespace ETOD.Forms
{
	partial class CTerrainAndObstacleFrm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTerrainAndObstacleFrm));
			this.OrganisationCombo = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.AerodromeCombo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.ReportBtn = new System.Windows.Forms.CheckBox();
			this.lvRWY = new System.Windows.Forms.ListView();
			this.RWYClmHdr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label4 = new System.Windows.Forms.Label();
			this.lvRWYDir = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.Area4Combo = new System.Windows.Forms.ComboBox();
			this.Area3Combo = new System.Windows.Forms.ComboBox();
			this.Area2Combo = new System.Windows.Forms.ComboBox();
			this.Area1Combo = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// OrganisationCombo
			// 
			this.OrganisationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.OrganisationCombo.FormattingEnabled = true;
			this.OrganisationCombo.Location = new System.Drawing.Point(90, 10);
			this.OrganisationCombo.Name = "OrganisationCombo";
			this.OrganisationCombo.Size = new System.Drawing.Size(76, 21);
			this.OrganisationCombo.TabIndex = 0;
			this.OrganisationCombo.SelectedIndexChanged += new System.EventHandler(this.OrganisationCombo_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Organisation:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Aerodrome:";
			// 
			// AerodromeCombo
			// 
			this.AerodromeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AerodromeCombo.FormattingEnabled = true;
			this.AerodromeCombo.Location = new System.Drawing.Point(90, 40);
			this.AerodromeCombo.Name = "AerodromeCombo";
			this.AerodromeCombo.Size = new System.Drawing.Size(76, 21);
			this.AerodromeCombo.TabIndex = 3;
			this.AerodromeCombo.SelectedIndexChanged += new System.EventHandler(this.AerodromeCombo_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(25, 79);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "RWY:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(287, 180);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Draw";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ReportBtn
			// 
			this.ReportBtn.Appearance = System.Windows.Forms.Appearance.Button;
			this.ReportBtn.BackColor = System.Drawing.SystemColors.Control;
			this.ReportBtn.Cursor = System.Windows.Forms.Cursors.Default;
			this.ReportBtn.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ReportBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.ReportBtn.Image = ((System.Drawing.Image)(resources.GetObject("ReportBtn.Image")));
			this.ReportBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ReportBtn.Location = new System.Drawing.Point(368, 180);
			this.ReportBtn.Name = "ReportBtn";
			this.ReportBtn.Size = new System.Drawing.Size(89, 25);
			this.ReportBtn.TabIndex = 211;
			this.ReportBtn.TabStop = false;
			this.ReportBtn.Text = "Report";
			this.ReportBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ReportBtn.UseVisualStyleBackColor = false;
			this.ReportBtn.CheckedChanged += new System.EventHandler(this.ReportBtn_CheckedChanged);
			// 
			// lvRWY
			// 
			this.lvRWY.CheckBoxes = true;
			this.lvRWY.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RWYClmHdr});
			this.lvRWY.FullRowSelect = true;
			this.lvRWY.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvRWY.LabelWrap = false;
			this.lvRWY.Location = new System.Drawing.Point(8, 95);
			this.lvRWY.MultiSelect = false;
			this.lvRWY.Name = "lvRWY";
			this.lvRWY.Size = new System.Drawing.Size(66, 106);
			this.lvRWY.TabIndex = 212;
			this.lvRWY.UseCompatibleStateImageBehavior = false;
			this.lvRWY.View = System.Windows.Forms.View.List;
			this.lvRWY.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvRWY_ItemChecked);
			// 
			// RWYClmHdr
			// 
			this.RWYClmHdr.Text = "RWY";
			this.RWYClmHdr.Width = 120;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(115, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(52, 13);
			this.label4.TabIndex = 213;
			this.label4.Text = "RWY Dir:";
			// 
			// lvRWYDir
			// 
			this.lvRWYDir.CheckBoxes = true;
			this.lvRWYDir.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvRWYDir.FullRowSelect = true;
			this.lvRWYDir.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvRWYDir.LabelWrap = false;
			this.lvRWYDir.Location = new System.Drawing.Point(106, 95);
			this.lvRWYDir.MultiSelect = false;
			this.lvRWYDir.Name = "lvRWYDir";
			this.lvRWYDir.Size = new System.Drawing.Size(69, 106);
			this.lvRWYDir.TabIndex = 214;
			this.lvRWYDir.UseCompatibleStateImageBehavior = false;
			this.lvRWYDir.View = System.Windows.Forms.View.List;
			this.lvRWYDir.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvRWYDir_ItemChecked);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "RWY Direction";
			this.columnHeader1.Width = 120;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.Area4Combo);
			this.groupBox1.Controls.Add(this.Area3Combo);
			this.groupBox1.Controls.Add(this.Area2Combo);
			this.groupBox1.Controls.Add(this.Area1Combo);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(199, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 165);
			this.groupBox1.TabIndex = 227;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Grids";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(187, 127);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(24, 13);
			this.label12.TabIndex = 238;
			this.label12.Text = "0x0";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(187, 96);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(24, 13);
			this.label11.TabIndex = 237;
			this.label11.Text = "0x0";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(187, 65);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(24, 13);
			this.label10.TabIndex = 236;
			this.label10.Text = "0x0";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(187, 33);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(24, 13);
			this.label9.TabIndex = 235;
			this.label9.Text = "0x0";
			// 
			// Area4Combo
			// 
			this.Area4Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Area4Combo.FormattingEnabled = true;
			this.Area4Combo.Location = new System.Drawing.Point(55, 124);
			this.Area4Combo.Name = "Area4Combo";
			this.Area4Combo.Size = new System.Drawing.Size(126, 21);
			this.Area4Combo.TabIndex = 234;
			this.Area4Combo.SelectedIndexChanged += new System.EventHandler(this.Area4Combo_SelectedIndexChanged);
			// 
			// Area3Combo
			// 
			this.Area3Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Area3Combo.FormattingEnabled = true;
			this.Area3Combo.Location = new System.Drawing.Point(55, 93);
			this.Area3Combo.Name = "Area3Combo";
			this.Area3Combo.Size = new System.Drawing.Size(126, 21);
			this.Area3Combo.TabIndex = 233;
			this.Area3Combo.SelectedIndexChanged += new System.EventHandler(this.Area3Combo_SelectedIndexChanged);
			// 
			// Area2Combo
			// 
			this.Area2Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Area2Combo.FormattingEnabled = true;
			this.Area2Combo.Location = new System.Drawing.Point(55, 62);
			this.Area2Combo.Name = "Area2Combo";
			this.Area2Combo.Size = new System.Drawing.Size(126, 21);
			this.Area2Combo.TabIndex = 232;
			this.Area2Combo.SelectedIndexChanged += new System.EventHandler(this.Area2Combo_SelectedIndexChanged);
			// 
			// Area1Combo
			// 
			this.Area1Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Area1Combo.FormattingEnabled = true;
			this.Area1Combo.Location = new System.Drawing.Point(55, 30);
			this.Area1Combo.Name = "Area1Combo";
			this.Area1Combo.Size = new System.Drawing.Size(126, 21);
			this.Area1Combo.TabIndex = 231;
			this.Area1Combo.SelectedIndexChanged += new System.EventHandler(this.Area1Combo_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 127);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(41, 13);
			this.label8.TabIndex = 230;
			this.label8.Text = "Area 4:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 96);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 13);
			this.label7.TabIndex = 229;
			this.label7.Text = "Area 3:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 65);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 13);
			this.label6.TabIndex = 228;
			this.label6.Text = "Area 2:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 33);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 13);
			this.label5.TabIndex = 227;
			this.label5.Text = "Area 1:";
			// 
			// CTerrainAndObstacleFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(473, 216);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lvRWYDir);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lvRWY);
			this.Controls.Add(this.ReportBtn);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.AerodromeCombo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.OrganisationCombo);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CTerrainAndObstacleFrm";
			this.ShowInTaskbar = false;
			this.Text = "ETOD";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CTerrainAndObstacleFrm_FormClosing);
			this.Load += new System.EventHandler(this.CTerrainAndObstacleFrm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox OrganisationCombo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox AerodromeCombo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		public System.Windows.Forms.CheckBox ReportBtn;
		private System.Windows.Forms.ListView lvRWY;
		private System.Windows.Forms.ColumnHeader RWYClmHdr;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView lvRWYDir;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox Area4Combo;
		private System.Windows.Forms.ComboBox Area3Combo;
		private System.Windows.Forms.ComboBox Area2Combo;
		private System.Windows.Forms.ComboBox Area1Combo;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}