namespace CDOTMA
{
	partial class WithCommonPoint
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Same direction tracks", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Crossing tracks", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Reciprocal tracks", System.Windows.Forms.HorizontalAlignment.Center);
			System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Vertically seperated", System.Windows.Forms.HorizontalAlignment.Center);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WithCommonPoint));
			this.LeftPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.RightPanel = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.listView2 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnProcedureType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnLowerLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnUpperLimit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label4 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.LeftPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.RightPanel.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// LeftPanel
			// 
			this.LeftPanel.Controls.Add(this.label6);
			this.LeftPanel.Controls.Add(this.label5);
			this.LeftPanel.Controls.Add(this.textBox1);
			this.LeftPanel.Controls.Add(this.comboBox1);
			this.LeftPanel.Controls.Add(this.label4);
			this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftPanel.Name = "LeftPanel";
			this.LeftPanel.Size = new System.Drawing.Size(152, 487);
			this.LeftPanel.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.treeView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(152, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(128, 487);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select point:";
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 17);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(128, 470);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Location = new System.Drawing.Point(280, 0);
			this.splitter1.MinExtra = 100;
			this.splitter1.MinSize = 75;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 487);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// RightPanel
			// 
			this.RightPanel.Controls.Add(this.panel3);
			this.RightPanel.Controls.Add(this.splitter2);
			this.RightPanel.Controls.Add(this.panel2);
			this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightPanel.Location = new System.Drawing.Point(283, 0);
			this.RightPanel.Name = "RightPanel";
			this.RightPanel.Size = new System.Drawing.Size(568, 487);
			this.RightPanel.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.listView2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 255);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(568, 232);
			this.panel3.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(568, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Routs intersecting with selected rout:";
			// 
			// listView2
			// 
			this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			listViewGroup1.Header = "Same direction tracks";
			listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup1.Name = "SameDirectionGroup";
			listViewGroup2.Header = "Crossing tracks";
			listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup2.Name = "CrossingTracksGroup";
			listViewGroup3.Header = "Reciprocal tracks";
			listViewGroup3.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup3.Name = "ReciprocalTracksGroup";
			listViewGroup4.Header = "Vertically seperated";
			listViewGroup4.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			listViewGroup4.Name = "VerticallySeperatedsGroup";
			this.listView2.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
			this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView2.HideSelection = false;
			this.listView2.LabelWrap = false;
			this.listView2.Location = new System.Drawing.Point(0, 17);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(568, 215);
			this.listView2.TabIndex = 5;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Details;
			this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Procedure name";
			this.columnHeader1.Width = 95;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Procedure type";
			this.columnHeader2.Width = 139;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Course (°)";
			this.columnHeader3.Width = 89;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Δ Azimuth (°)";
			this.columnHeader4.Width = 89;
			// 
			// splitter2
			// 
			this.splitter2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 252);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(568, 3);
			this.splitter2.TabIndex = 4;
			this.splitter2.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.listView1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(568, 252);
			this.panel2.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(568, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Routs pass through selected point:";
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnProcedureType,
            this.columnLowerLimit,
            this.columnUpperLimit,
            this.columnIn,
            this.columnOut});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.LabelWrap = false;
			this.listView1.Location = new System.Drawing.Point(0, 17);
			this.listView1.Name = "listView1";
			this.listView1.ShowGroups = false;
			this.listView1.Size = new System.Drawing.Size(568, 235);
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
			// columnIn
			// 
			this.columnIn.Text = "Entry course (°)";
			this.columnIn.Width = 80;
			// 
			// columnOut
			// 
			this.columnOut.Text = "Leave course (°)";
			this.columnOut.Width = 80;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(11, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(134, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Select conflict Flight Level:";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(11, 25);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(74, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(11, 84);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(74, 20);
			this.textBox1.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(11, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 13);
			this.label5.TabIndex = 3;
			this.label5.Text = "Conflict distance:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(92, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(21, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "km";
			// 
			// WithCommonPoint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 487);
			this.Controls.Add(this.RightPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.LeftPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WithCommonPoint";
			this.ShowInTaskbar = false;
			this.Text = "Intersecting tracks with common point";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WithCommonPoint_FormClosed);
			this.LeftPanel.ResumeLayout(false);
			this.LeftPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.RightPanel.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel LeftPanel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel RightPanel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnIn;
		private System.Windows.Forms.ColumnHeader columnOut;
		private System.Windows.Forms.ColumnHeader columnProcedureType;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnLowerLimit;
		private System.Windows.Forms.ColumnHeader columnUpperLimit;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
	}
}