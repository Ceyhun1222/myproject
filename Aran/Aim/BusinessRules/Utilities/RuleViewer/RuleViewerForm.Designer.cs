namespace RuleViewer
{
    partial class RuleViewerForm
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
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_menuButton = new System.Windows.Forms.Button();
            this.ui_ruleTypeCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ui_rulesCount = new System.Windows.Forms.Label();
            this.ui_searchTB = new System.Windows.Forms.TextBox();
            this.ui_rulesDGV = new System.Windows.Forms.DataGridView();
            this.ui_colUid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colProfile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colInfo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ui_colNotImplemented = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_groupedByTB = new System.Windows.Forms.TextBox();
            this.ui_notRealzedReasonTB = new System.Windows.Forms.TextBox();
            this.ui_commentsTB = new System.Windows.Forms.TextBox();
            this.ui_activeChB = new System.Windows.Forms.CheckBox();
            this.ui_profileTB = new System.Windows.Forms.TextBox();
            this.ui_nameTB = new System.Windows.Forms.TextBox();
            this.ui_uidTB = new System.Windows.Forms.TextBox();
            this.ui_taggedDGV = new System.Windows.Forms.DataGridView();
            this.ui_colTaggedKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colTaggedText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_rulesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllListItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllListItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label5 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).BeginInit();
            this.ui_mainSplitContainer.Panel1.SuspendLayout();
            this.ui_mainSplitContainer.Panel2.SuspendLayout();
            this.ui_mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_rulesDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_taggedDGV)).BeginInit();
            this.ui_rulesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(16, 139);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(78, 17);
            label5.TabIndex = 7;
            label5.Text = "Comments:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(42, 48);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(52, 17);
            label3.TabIndex = 4;
            label3.Text = "Profile:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(44, 80);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(49, 17);
            label2.TabIndex = 2;
            label2.Text = "Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(56, 16);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 17);
            label1.TabIndex = 0;
            label1.Text = "UID:";
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(16, 209);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(79, 59);
            label6.TabIndex = 9;
            label6.Text = "Not Realized Reason:";
            label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(16, 287);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(87, 17);
            label7.TabIndex = 11;
            label7.Text = "Grouped by:";
            // 
            // ui_mainSplitContainer
            // 
            this.ui_mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ui_mainSplitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.ui_mainSplitContainer.Name = "ui_mainSplitContainer";
            // 
            // ui_mainSplitContainer.Panel1
            // 
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_menuButton);
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_ruleTypeCB);
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.label4);
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_rulesCount);
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_searchTB);
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_rulesDGV);
            // 
            // ui_mainSplitContainer.Panel2
            // 
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.ui_mainSplitContainer.Size = new System.Drawing.Size(1071, 846);
            this.ui_mainSplitContainer.SplitterDistance = 396;
            this.ui_mainSplitContainer.SplitterWidth = 5;
            this.ui_mainSplitContainer.TabIndex = 1;
            // 
            // ui_menuButton
            // 
            this.ui_menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_menuButton.AutoSize = true;
            this.ui_menuButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_menuButton.Location = new System.Drawing.Point(359, 68);
            this.ui_menuButton.Margin = new System.Windows.Forms.Padding(4);
            this.ui_menuButton.Name = "ui_menuButton";
            this.ui_menuButton.Size = new System.Drawing.Size(30, 27);
            this.ui_menuButton.TabIndex = 7;
            this.ui_menuButton.Text = "...";
            this.ui_menuButton.UseVisualStyleBackColor = true;
            this.ui_menuButton.Click += new System.EventHandler(this.MenuButton_Click);
            // 
            // ui_ruleTypeCB
            // 
            this.ui_ruleTypeCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_ruleTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_ruleTypeCB.FormattingEnabled = true;
            this.ui_ruleTypeCB.Items.AddRange(new object[] {
            "All",
            "Error",
            "Warning",
            "Active",
            "LGS",
            "Realized",
            "NotRealized"});
            this.ui_ruleTypeCB.Location = new System.Drawing.Point(69, 6);
            this.ui_ruleTypeCB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_ruleTypeCB.Name = "ui_ruleTypeCB";
            this.ui_ruleTypeCB.Size = new System.Drawing.Size(318, 24);
            this.ui_ruleTypeCB.TabIndex = 6;
            this.ui_ruleTypeCB.SelectedIndexChanged += new System.EventHandler(this.RuleTypeCB_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Type:";
            // 
            // ui_rulesCount
            // 
            this.ui_rulesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_rulesCount.AutoSize = true;
            this.ui_rulesCount.Location = new System.Drawing.Point(8, 822);
            this.ui_rulesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ui_rulesCount.Name = "ui_rulesCount";
            this.ui_rulesCount.Size = new System.Drawing.Size(61, 17);
            this.ui_rulesCount.TabIndex = 0;
            this.ui_rulesCount.Text = "Count: 0";
            // 
            // ui_searchTB
            // 
            this.ui_searchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_searchTB.Location = new System.Drawing.Point(15, 38);
            this.ui_searchTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_searchTB.Name = "ui_searchTB";
            this.ui_searchTB.Size = new System.Drawing.Size(373, 22);
            this.ui_searchTB.TabIndex = 2;
            this.ui_searchTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_KeyUp);
            // 
            // ui_rulesDGV
            // 
            this.ui_rulesDGV.AllowUserToAddRows = false;
            this.ui_rulesDGV.AllowUserToDeleteRows = false;
            this.ui_rulesDGV.AllowUserToOrderColumns = true;
            this.ui_rulesDGV.AllowUserToResizeRows = false;
            this.ui_rulesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_rulesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ui_rulesDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.ui_rulesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_rulesDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colUid,
            this.ui_colProfile,
            this.ui_colInfo,
            this.ui_colNotImplemented});
            this.ui_rulesDGV.Location = new System.Drawing.Point(0, 100);
            this.ui_rulesDGV.Margin = new System.Windows.Forms.Padding(4);
            this.ui_rulesDGV.Name = "ui_rulesDGV";
            this.ui_rulesDGV.RowHeadersVisible = false;
            this.ui_rulesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_rulesDGV.Size = new System.Drawing.Size(396, 716);
            this.ui_rulesDGV.TabIndex = 1;
            this.ui_rulesDGV.VirtualMode = true;
            this.ui_rulesDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.RolesDGV_CellValueNeeded);
            this.ui_rulesDGV.CurrentCellChanged += new System.EventHandler(this.RolesDGV_CurrentCellChanged);
            // 
            // ui_colUid
            // 
            this.ui_colUid.HeaderText = "UID";
            this.ui_colUid.Name = "ui_colUid";
            this.ui_colUid.ReadOnly = true;
            this.ui_colUid.Width = 60;
            // 
            // ui_colProfile
            // 
            this.ui_colProfile.HeaderText = "Profile";
            this.ui_colProfile.Name = "ui_colProfile";
            this.ui_colProfile.ReadOnly = true;
            this.ui_colProfile.Width = 60;
            // 
            // ui_colInfo
            // 
            this.ui_colInfo.HeaderText = "Active";
            this.ui_colInfo.Name = "ui_colInfo";
            this.ui_colInfo.ReadOnly = true;
            this.ui_colInfo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_colInfo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ui_colInfo.Width = 50;
            // 
            // ui_colNotImplemented
            // 
            this.ui_colNotImplemented.HeaderText = "Realized";
            this.ui_colNotImplemented.Name = "ui_colNotImplemented";
            this.ui_colNotImplemented.ReadOnly = true;
            this.ui_colNotImplemented.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_colNotImplemented.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ui_colNotImplemented.Width = 70;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.Controls.Add(this.ui_groupedByTB);
            this.splitContainer1.Panel1.Controls.Add(label7);
            this.splitContainer1.Panel1.Controls.Add(this.ui_notRealzedReasonTB);
            this.splitContainer1.Panel1.Controls.Add(label6);
            this.splitContainer1.Panel1.Controls.Add(this.ui_commentsTB);
            this.splitContainer1.Panel1.Controls.Add(label5);
            this.splitContainer1.Panel1.Controls.Add(this.ui_activeChB);
            this.splitContainer1.Panel1.Controls.Add(this.ui_profileTB);
            this.splitContainer1.Panel1.Controls.Add(label3);
            this.splitContainer1.Panel1.Controls.Add(this.ui_nameTB);
            this.splitContainer1.Panel1.Controls.Add(label2);
            this.splitContainer1.Panel1.Controls.Add(this.ui_uidTB);
            this.splitContainer1.Panel1.Controls.Add(label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_taggedDGV);
            this.splitContainer1.Size = new System.Drawing.Size(670, 846);
            this.splitContainer1.SplitterDistance = 351;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // ui_groupedByTB
            // 
            this.ui_groupedByTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_groupedByTB.Location = new System.Drawing.Point(102, 284);
            this.ui_groupedByTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_groupedByTB.Name = "ui_groupedByTB";
            this.ui_groupedByTB.Size = new System.Drawing.Size(262, 22);
            this.ui_groupedByTB.TabIndex = 12;
            this.ui_groupedByTB.Leave += new System.EventHandler(this.GroupedByTB_Leave);
            // 
            // ui_notRealzedReasonTB
            // 
            this.ui_notRealzedReasonTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_notRealzedReasonTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_notRealzedReasonTB.Location = new System.Drawing.Point(102, 206);
            this.ui_notRealzedReasonTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_notRealzedReasonTB.Multiline = true;
            this.ui_notRealzedReasonTB.Name = "ui_notRealzedReasonTB";
            this.ui_notRealzedReasonTB.Size = new System.Drawing.Size(550, 70);
            this.ui_notRealzedReasonTB.TabIndex = 10;
            this.ui_notRealzedReasonTB.Leave += new System.EventHandler(this.NotRealzedReason_Leave);
            // 
            // ui_commentsTB
            // 
            this.ui_commentsTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_commentsTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_commentsTB.Location = new System.Drawing.Point(102, 135);
            this.ui_commentsTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_commentsTB.Multiline = true;
            this.ui_commentsTB.Name = "ui_commentsTB";
            this.ui_commentsTB.ReadOnly = true;
            this.ui_commentsTB.Size = new System.Drawing.Size(550, 62);
            this.ui_commentsTB.TabIndex = 8;
            // 
            // ui_activeChB
            // 
            this.ui_activeChB.AutoSize = true;
            this.ui_activeChB.Location = new System.Drawing.Point(420, 14);
            this.ui_activeChB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_activeChB.Name = "ui_activeChB";
            this.ui_activeChB.Size = new System.Drawing.Size(68, 21);
            this.ui_activeChB.TabIndex = 6;
            this.ui_activeChB.Tag = "is_active";
            this.ui_activeChB.Text = "Active";
            this.ui_activeChB.UseVisualStyleBackColor = true;
            this.ui_activeChB.Click += new System.EventHandler(this.RuleStatus_Click);
            // 
            // ui_profileTB
            // 
            this.ui_profileTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_profileTB.Location = new System.Drawing.Point(102, 44);
            this.ui_profileTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_profileTB.Name = "ui_profileTB";
            this.ui_profileTB.ReadOnly = true;
            this.ui_profileTB.Size = new System.Drawing.Size(262, 22);
            this.ui_profileTB.TabIndex = 5;
            // 
            // ui_nameTB
            // 
            this.ui_nameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nameTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_nameTB.Location = new System.Drawing.Point(102, 76);
            this.ui_nameTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_nameTB.Multiline = true;
            this.ui_nameTB.Name = "ui_nameTB";
            this.ui_nameTB.ReadOnly = true;
            this.ui_nameTB.Size = new System.Drawing.Size(550, 52);
            this.ui_nameTB.TabIndex = 3;
            // 
            // ui_uidTB
            // 
            this.ui_uidTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_uidTB.Location = new System.Drawing.Point(102, 12);
            this.ui_uidTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_uidTB.Name = "ui_uidTB";
            this.ui_uidTB.ReadOnly = true;
            this.ui_uidTB.Size = new System.Drawing.Size(262, 22);
            this.ui_uidTB.TabIndex = 1;
            // 
            // ui_taggedDGV
            // 
            this.ui_taggedDGV.AllowUserToAddRows = false;
            this.ui_taggedDGV.AllowUserToDeleteRows = false;
            this.ui_taggedDGV.AllowUserToOrderColumns = true;
            this.ui_taggedDGV.AllowUserToResizeRows = false;
            this.ui_taggedDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_taggedDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.ui_taggedDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_taggedDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colTaggedKey,
            this.ui_colTaggedText});
            this.ui_taggedDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_taggedDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_taggedDGV.Margin = new System.Windows.Forms.Padding(4);
            this.ui_taggedDGV.Name = "ui_taggedDGV";
            this.ui_taggedDGV.RowHeadersVisible = false;
            this.ui_taggedDGV.Size = new System.Drawing.Size(670, 490);
            this.ui_taggedDGV.TabIndex = 0;
            this.ui_taggedDGV.VirtualMode = true;
            this.ui_taggedDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.TaggedDGV_CellValueNeeded);
            // 
            // ui_colTaggedKey
            // 
            this.ui_colTaggedKey.HeaderText = "Type";
            this.ui_colTaggedKey.Name = "ui_colTaggedKey";
            this.ui_colTaggedKey.ReadOnly = true;
            this.ui_colTaggedKey.Width = 120;
            // 
            // ui_colTaggedText
            // 
            this.ui_colTaggedText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colTaggedText.HeaderText = "Text";
            this.ui_colTaggedText.Name = "ui_colTaggedText";
            // 
            // ui_rulesContextMenu
            // 
            this.ui_rulesContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_rulesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllListItemsToolStripMenuItem,
            this.uncheckAllListItemsToolStripMenuItem});
            this.ui_rulesContextMenu.Name = "ui_rulesContextMenu";
            this.ui_rulesContextMenu.Size = new System.Drawing.Size(220, 52);
            // 
            // checkAllListItemsToolStripMenuItem
            // 
            this.checkAllListItemsToolStripMenuItem.Name = "checkAllListItemsToolStripMenuItem";
            this.checkAllListItemsToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.checkAllListItemsToolStripMenuItem.Text = "Active all list items";
            this.checkAllListItemsToolStripMenuItem.Click += new System.EventHandler(this.CheckAllListItems_Click);
            // 
            // uncheckAllListItemsToolStripMenuItem
            // 
            this.uncheckAllListItemsToolStripMenuItem.Name = "uncheckAllListItemsToolStripMenuItem";
            this.uncheckAllListItemsToolStripMenuItem.Size = new System.Drawing.Size(219, 24);
            this.uncheckAllListItemsToolStripMenuItem.Text = "Deactive all list items";
            this.uncheckAllListItemsToolStripMenuItem.Click += new System.EventHandler(this.UncheckAllListItems_Click);
            // 
            // RuleViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1071, 846);
            this.Controls.Add(this.ui_mainSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RuleViewerForm";
            this.ShowIcon = false;
            this.Text = "Rule Viewer";
            this.Load += new System.EventHandler(this.RuleViewerForm_Load);
            this.ui_mainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_mainSplitContainer.Panel1.PerformLayout();
            this.ui_mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).EndInit();
            this.ui_mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_rulesDGV)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_taggedDGV)).EndInit();
            this.ui_rulesContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer ui_mainSplitContainer;
        private System.Windows.Forms.DataGridView ui_taggedDGV;
        private System.Windows.Forms.DataGridView ui_rulesDGV;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox ui_uidTB;
        private System.Windows.Forms.TextBox ui_profileTB;
        private System.Windows.Forms.TextBox ui_nameTB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colTaggedKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colTaggedText;
        private System.Windows.Forms.TextBox ui_searchTB;
        private System.Windows.Forms.Label ui_rulesCount;
        private System.Windows.Forms.CheckBox ui_activeChB;
        private System.Windows.Forms.ComboBox ui_ruleTypeCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ui_commentsTB;
        private System.Windows.Forms.Button ui_menuButton;
        private System.Windows.Forms.ContextMenuStrip ui_rulesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem checkAllListItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllListItemsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colProfile;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ui_colInfo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ui_colNotImplemented;
        private System.Windows.Forms.TextBox ui_notRealzedReasonTB;
        private System.Windows.Forms.TextBox ui_groupedByTB;
    }
}