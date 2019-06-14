namespace CDOTMA
{
	partial class NonInterSIDAndSTAR
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NonInterSIDAndSTAR));
			this.LeftPanel = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbIAP = new System.Windows.Forms.CheckBox();
			this.cbATS = new System.Windows.Forms.CheckBox();
			this.cbSTAR = new System.Windows.Forms.CheckBox();
			this.cbSID = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnProcedureType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnLowerLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnUpperLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.RightPanel = new System.Windows.Forms.Panel();
			this.LeftPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.RightPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// LeftPanel
			// 
			this.LeftPanel.Controls.Add(this.label3);
			this.LeftPanel.Controls.Add(this.comboBox1);
			this.LeftPanel.Controls.Add(this.label2);
			this.LeftPanel.Controls.Add(this.cbIAP);
			this.LeftPanel.Controls.Add(this.cbATS);
			this.LeftPanel.Controls.Add(this.cbSTAR);
			this.LeftPanel.Controls.Add(this.cbSID);
			this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftPanel.Name = "LeftPanel";
			this.LeftPanel.Size = new System.Drawing.Size(138, 460);
			this.LeftPanel.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(84, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(22, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Km";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(14, 160);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(64, 21);
			this.comboBox1.TabIndex = 8;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 134);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Buffer semi-width:";
			// 
			// cbIAP
			// 
			this.cbIAP.AutoSize = true;
			this.cbIAP.Checked = true;
			this.cbIAP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbIAP.Location = new System.Drawing.Point(11, 11);
			this.cbIAP.Name = "cbIAP";
			this.cbIAP.Size = new System.Drawing.Size(43, 17);
			this.cbIAP.TabIndex = 6;
			this.cbIAP.Text = "IAP";
			this.cbIAP.UseVisualStyleBackColor = true;
			this.cbIAP.CheckedChanged += new System.EventHandler(this.checkBoxCheckedChanged);
			// 
			// cbATS
			// 
			this.cbATS.AutoSize = true;
			this.cbATS.Checked = true;
			this.cbATS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbATS.Location = new System.Drawing.Point(11, 80);
			this.cbATS.Name = "cbATS";
			this.cbATS.Size = new System.Drawing.Size(73, 17);
			this.cbATS.TabIndex = 5;
			this.cbATS.Text = "ATS routs";
			this.cbATS.UseVisualStyleBackColor = true;
			this.cbATS.CheckedChanged += new System.EventHandler(this.checkBoxCheckedChanged);
			// 
			// cbSTAR
			// 
			this.cbSTAR.AutoSize = true;
			this.cbSTAR.Checked = true;
			this.cbSTAR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSTAR.Location = new System.Drawing.Point(11, 34);
			this.cbSTAR.Name = "cbSTAR";
			this.cbSTAR.Size = new System.Drawing.Size(55, 17);
			this.cbSTAR.TabIndex = 4;
			this.cbSTAR.Text = "STAR";
			this.cbSTAR.UseVisualStyleBackColor = true;
			this.cbSTAR.CheckedChanged += new System.EventHandler(this.checkBoxCheckedChanged);
			// 
			// cbSID
			// 
			this.cbSID.AutoSize = true;
			this.cbSID.Checked = true;
			this.cbSID.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSID.Location = new System.Drawing.Point(11, 57);
			this.cbSID.Name = "cbSID";
			this.cbSID.Size = new System.Drawing.Size(44, 17);
			this.cbSID.TabIndex = 3;
			this.cbSID.Text = "SID";
			this.cbSID.UseVisualStyleBackColor = true;
			this.cbSID.CheckedChanged += new System.EventHandler(this.checkBoxCheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.treeView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(138, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(128, 460);
			this.panel1.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select procedure:";
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 17);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(128, 443);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Location = new System.Drawing.Point(266, 0);
			this.splitter1.MinExtra = 100;
			this.splitter1.MinSize = 75;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 460);
			this.splitter1.TabIndex = 9;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.listView1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(522, 460);
			this.panel2.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Top;
			this.label5.Location = new System.Drawing.Point(0, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(522, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Routs within buffer area:";
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnProcedureType,
            this.columnLowerLimit,
            this.columnUpperLimit});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.LabelWrap = false;
			this.listView1.Location = new System.Drawing.Point(0, 17);
			this.listView1.Name = "listView1";
			this.listView1.ShowGroups = false;
			this.listView1.Size = new System.Drawing.Size(534, 443);
			this.listView1.TabIndex = 3;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// columnName
			// 
			this.columnName.Text = "Route name";
			this.columnName.Width = 80;
			// 
			// columnProcedureType
			// 
			this.columnProcedureType.Text = "Procedure type";
			this.columnProcedureType.Width = 100;
			// 
			// columnLowerLimit
			// 
			this.columnLowerLimit.Text = "Lower limit";
			this.columnLowerLimit.Width = 80;
			// 
			// columnUpperLimit
			// 
			this.columnUpperLimit.Text = "Upper limit";
			this.columnUpperLimit.Width = 80;
			// 
			// RightPanel
			// 
			this.RightPanel.Controls.Add(this.panel2);
			this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightPanel.Location = new System.Drawing.Point(269, 0);
			this.RightPanel.Name = "RightPanel";
			this.RightPanel.Size = new System.Drawing.Size(522, 460);
			this.RightPanel.TabIndex = 10;
			// 
			// NonInterSIDAndSTAR
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(791, 460);
			this.Controls.Add(this.RightPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.LeftPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NonInterSIDAndSTAR";
			this.ShowInTaskbar = false;
			this.Text = "Not Intersecting SID And STAR";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NonInterSIDAndSTAR_FormClosed);
			this.LeftPanel.ResumeLayout(false);
			this.LeftPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.RightPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel LeftPanel;
		private System.Windows.Forms.CheckBox cbATS;
		private System.Windows.Forms.CheckBox cbSTAR;
		private System.Windows.Forms.CheckBox cbSID;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.CheckBox cbIAP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnProcedureType;
		private System.Windows.Forms.ColumnHeader columnLowerLimit;
		private System.Windows.Forms.ColumnHeader columnUpperLimit;
		private System.Windows.Forms.Panel RightPanel;
	}
}