namespace AranUpdateManager
{
    partial class UsersPageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStrip toolStrip1;
            System.Windows.Forms.ToolStripLabel toolStripLabel1;
            System.Windows.Forms.ToolStripButton toolStripButton1;
            System.Windows.Forms.ToolStripLabel toolStripLabel2;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_editGroupTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_deleteGroupTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_newUserTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_userToolStrip = new System.Windows.Forms.ToolStrip();
            this.ui_editUserTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_deleteUserTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_moveUserTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_showLogTSB = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_groupDgv = new System.Windows.Forms.DataGridView();
            this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colCurrVers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_userDGV = new System.Windows.Forms.DataGridView();
            this.ui_colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colUserFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colUserLastVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colUserHasLog = new System.Windows.Forms.DataGridViewImageColumn();
            this.ui_userItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            toolStrip1.SuspendLayout();
            this.ui_userToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_groupDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_userDGV)).BeginInit();
            this.ui_userItemContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripLabel1,
            toolStripButton1,
            this.ui_editGroupTSB,
            this.ui_deleteGroupTSB});
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(446, 36);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            toolStripLabel1.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(100, 33);
            toolStripLabel1.Text = "User Groups:";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = global::AranUpdateManager.Properties.Resources.add_24;
            toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Margin = new System.Windows.Forms.Padding(4);
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(67, 28);
            toolStripButton1.Text = "New";
            toolStripButton1.ToolTipText = "New Group";
            toolStripButton1.Click += new System.EventHandler(this.NewGroup_Click);
            // 
            // ui_editGroupTSB
            // 
            this.ui_editGroupTSB.Enabled = false;
            this.ui_editGroupTSB.Image = global::AranUpdateManager.Properties.Resources.edit_24;
            this.ui_editGroupTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_editGroupTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_editGroupTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_editGroupTSB.Name = "ui_editGroupTSB";
            this.ui_editGroupTSB.Size = new System.Drawing.Size(63, 28);
            this.ui_editGroupTSB.Text = "Edit";
            this.ui_editGroupTSB.ToolTipText = "Edit Group";
            this.ui_editGroupTSB.Click += new System.EventHandler(this.EditGroup_Click);
            // 
            // ui_deleteGroupTSB
            // 
            this.ui_deleteGroupTSB.Enabled = false;
            this.ui_deleteGroupTSB.Image = global::AranUpdateManager.Properties.Resources.remove_24;
            this.ui_deleteGroupTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_deleteGroupTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_deleteGroupTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_deleteGroupTSB.Name = "ui_deleteGroupTSB";
            this.ui_deleteGroupTSB.Size = new System.Drawing.Size(91, 28);
            this.ui_deleteGroupTSB.Text = "Remove";
            this.ui_deleteGroupTSB.ToolTipText = "Remove Group";
            this.ui_deleteGroupTSB.Click += new System.EventHandler(this.DeleteGroup_Click);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            toolStripLabel2.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(45, 33);
            toolStripLabel2.Text = "User:";
            // 
            // ui_newUserTSB
            // 
            this.ui_newUserTSB.Image = global::AranUpdateManager.Properties.Resources.add_24;
            this.ui_newUserTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_newUserTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_newUserTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_newUserTSB.Name = "ui_newUserTSB";
            this.ui_newUserTSB.Size = new System.Drawing.Size(67, 28);
            this.ui_newUserTSB.Text = "New";
            this.ui_newUserTSB.ToolTipText = "New User";
            this.ui_newUserTSB.Click += new System.EventHandler(this.NewUser_Click);
            // 
            // ui_userToolStrip
            // 
            this.ui_userToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ui_userToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_userToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripLabel2,
            this.ui_newUserTSB,
            this.ui_editUserTSB,
            this.ui_deleteUserTSB,
            this.ui_moveUserTSB,
            this.ui_showLogTSB});
            this.ui_userToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ui_userToolStrip.Name = "ui_userToolStrip";
            this.ui_userToolStrip.Size = new System.Drawing.Size(873, 36);
            this.ui_userToolStrip.TabIndex = 3;
            this.ui_userToolStrip.Text = "toolStrip2";
            // 
            // ui_editUserTSB
            // 
            this.ui_editUserTSB.Enabled = false;
            this.ui_editUserTSB.Image = global::AranUpdateManager.Properties.Resources.edit_24;
            this.ui_editUserTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_editUserTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_editUserTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_editUserTSB.Name = "ui_editUserTSB";
            this.ui_editUserTSB.Size = new System.Drawing.Size(63, 28);
            this.ui_editUserTSB.Text = "Edit";
            this.ui_editUserTSB.ToolTipText = "Edit User";
            this.ui_editUserTSB.Click += new System.EventHandler(this.EditUser_Click);
            // 
            // ui_deleteUserTSB
            // 
            this.ui_deleteUserTSB.Enabled = false;
            this.ui_deleteUserTSB.Image = global::AranUpdateManager.Properties.Resources.remove_24;
            this.ui_deleteUserTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_deleteUserTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_deleteUserTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_deleteUserTSB.Name = "ui_deleteUserTSB";
            this.ui_deleteUserTSB.Size = new System.Drawing.Size(91, 28);
            this.ui_deleteUserTSB.Text = "Remove";
            this.ui_deleteUserTSB.ToolTipText = "Remove User";
            this.ui_deleteUserTSB.Click += new System.EventHandler(this.DeleteUser_Click);
            // 
            // ui_moveUserTSB
            // 
            this.ui_moveUserTSB.Enabled = false;
            this.ui_moveUserTSB.Image = global::AranUpdateManager.Properties.Resources.move_24;
            this.ui_moveUserTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_moveUserTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_moveUserTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_moveUserTSB.Name = "ui_moveUserTSB";
            this.ui_moveUserTSB.Size = new System.Drawing.Size(74, 28);
            this.ui_moveUserTSB.Text = "Move";
            this.ui_moveUserTSB.ToolTipText = "Move user to another group";
            this.ui_moveUserTSB.Click += new System.EventHandler(this.MoveUser_Click);
            // 
            // ui_showLogTSB
            // 
            this.ui_showLogTSB.Enabled = false;
            this.ui_showLogTSB.Image = global::AranUpdateManager.Properties.Resources.attach_24;
            this.ui_showLogTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_showLogTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_showLogTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_showLogTSB.Name = "ui_showLogTSB";
            this.ui_showLogTSB.Size = new System.Drawing.Size(102, 28);
            this.ui_showLogTSB.Text = "Show Log";
            this.ui_showLogTSB.ToolTipText = "Show User\'s Log";
            this.ui_showLogTSB.Click += new System.EventHandler(this.ShowLog_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_groupDgv);
            this.splitContainer1.Panel1.Controls.Add(toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_userDGV);
            this.splitContainer1.Panel2.Controls.Add(this.ui_userToolStrip);
            this.splitContainer1.Size = new System.Drawing.Size(1324, 644);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // ui_groupDgv
            // 
            this.ui_groupDgv.AllowUserToAddRows = false;
            this.ui_groupDgv.AllowUserToDeleteRows = false;
            this.ui_groupDgv.AllowUserToResizeRows = false;
            this.ui_groupDgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_groupDgv.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_groupDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ui_groupDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_groupDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName,
            this.ui_colCurrVers,
            this.ui_colDesc});
            this.ui_groupDgv.Location = new System.Drawing.Point(0, 48);
            this.ui_groupDgv.Margin = new System.Windows.Forms.Padding(4);
            this.ui_groupDgv.MultiSelect = false;
            this.ui_groupDgv.Name = "ui_groupDgv";
            this.ui_groupDgv.ReadOnly = true;
            this.ui_groupDgv.RowHeadersVisible = false;
            this.ui_groupDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_groupDgv.Size = new System.Drawing.Size(446, 596);
            this.ui_groupDgv.TabIndex = 2;
            this.ui_groupDgv.VirtualMode = true;
            this.ui_groupDgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GroupDgv_CellFormatting);
            this.ui_groupDgv.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GroupDgv_CellMouseDoubleClick);
            this.ui_groupDgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.GroupDGV_CellValueNeeded);
            this.ui_groupDgv.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GroupDGV_ColumnHeaderMouseClick);
            this.ui_groupDgv.CurrentCellChanged += new System.EventHandler(this.GroupDGV_CurrentCellChanged);
            // 
            // ui_colName
            // 
            this.ui_colName.HeaderText = "Name";
            this.ui_colName.Name = "ui_colName";
            this.ui_colName.ReadOnly = true;
            this.ui_colName.Width = 140;
            // 
            // ui_colCurrVers
            // 
            this.ui_colCurrVers.HeaderText = "Current Version";
            this.ui_colCurrVers.Name = "ui_colCurrVers";
            this.ui_colCurrVers.ReadOnly = true;
            this.ui_colCurrVers.Width = 80;
            // 
            // ui_colDesc
            // 
            this.ui_colDesc.HeaderText = "Description";
            this.ui_colDesc.Name = "ui_colDesc";
            this.ui_colDesc.ReadOnly = true;
            this.ui_colDesc.Width = 200;
            // 
            // ui_userDGV
            // 
            this.ui_userDGV.AllowUserToAddRows = false;
            this.ui_userDGV.AllowUserToDeleteRows = false;
            this.ui_userDGV.AllowUserToResizeRows = false;
            this.ui_userDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_userDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_userDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ui_userDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_userDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colUserName,
            this.ui_colUserFullName,
            this.ui_colUserLastVersion,
            this.ui_colUserHasLog});
            this.ui_userDGV.Location = new System.Drawing.Point(0, 48);
            this.ui_userDGV.Margin = new System.Windows.Forms.Padding(4);
            this.ui_userDGV.MultiSelect = false;
            this.ui_userDGV.Name = "ui_userDGV";
            this.ui_userDGV.ReadOnly = true;
            this.ui_userDGV.RowHeadersVisible = false;
            this.ui_userDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_userDGV.Size = new System.Drawing.Size(868, 596);
            this.ui_userDGV.TabIndex = 4;
            this.ui_userDGV.VirtualMode = true;
            this.ui_userDGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UserDGV_CellMouseDoubleClick);
            this.ui_userDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.UserDGV_CellValueNeeded);
            this.ui_userDGV.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UserDGV_ColumnHeaderMouseClick);
            this.ui_userDGV.CurrentCellChanged += new System.EventHandler(this.UserDGV_CurrentCellChanged);
            this.ui_userDGV.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.UserDGV_RowContextMenuStripNeeded);
            // 
            // ui_colUserName
            // 
            this.ui_colUserName.HeaderText = "User Name";
            this.ui_colUserName.Name = "ui_colUserName";
            this.ui_colUserName.ReadOnly = true;
            this.ui_colUserName.Width = 140;
            // 
            // ui_colUserFullName
            // 
            this.ui_colUserFullName.HeaderText = "Full Name";
            this.ui_colUserFullName.Name = "ui_colUserFullName";
            this.ui_colUserFullName.ReadOnly = true;
            this.ui_colUserFullName.Width = 200;
            // 
            // ui_colUserLastVersion
            // 
            this.ui_colUserLastVersion.HeaderText = "Last Version";
            this.ui_colUserLastVersion.Name = "ui_colUserLastVersion";
            this.ui_colUserLastVersion.ReadOnly = true;
            this.ui_colUserLastVersion.Width = 120;
            // 
            // ui_colUserHasLog
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "SystemDrawing.Bitmap";
            this.ui_colUserHasLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.ui_colUserHasLog.HeaderText = "Log";
            this.ui_colUserHasLog.Name = "ui_colUserHasLog";
            this.ui_colUserHasLog.ReadOnly = true;
            this.ui_colUserHasLog.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_colUserHasLog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ui_colUserHasLog.Width = 38;
            // 
            // ui_userItemContextMenu
            // 
            this.ui_userItemContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_userItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToToolStripMenuItem});
            this.ui_userItemContextMenu.Name = "ui_userItemContextMenu";
            this.ui_userItemContextMenu.Size = new System.Drawing.Size(143, 30);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupItemToolStripMenuItem});
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            this.moveToToolStripMenuItem.Text = "Move To";
            // 
            // groupItemToolStripMenuItem
            // 
            this.groupItemToolStripMenuItem.Name = "groupItemToolStripMenuItem";
            this.groupItemToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.groupItemToolStripMenuItem.Text = "<Group Item>";
            // 
            // UsersPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UsersPageControl";
            this.Size = new System.Drawing.Size(1324, 644);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            this.ui_userToolStrip.ResumeLayout(false);
            this.ui_userToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_groupDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_userDGV)).EndInit();
            this.ui_userItemContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton ui_editGroupTSB;
        private System.Windows.Forms.ToolStripButton ui_deleteGroupTSB;
        private System.Windows.Forms.DataGridView ui_groupDgv;
        private System.Windows.Forms.DataGridView ui_userDGV;
        private System.Windows.Forms.ToolStripButton ui_editUserTSB;
        private System.Windows.Forms.ToolStripButton ui_deleteUserTSB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colCurrVers;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colDesc;
        private System.Windows.Forms.ContextMenuStrip ui_userItemContextMenu;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ui_moveUserTSB;
        private System.Windows.Forms.ToolStrip ui_userToolStrip;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUserFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUserLastVersion;
        private System.Windows.Forms.DataGridViewImageColumn ui_colUserHasLog;
        private System.Windows.Forms.ToolStripButton ui_showLogTSB;
        private System.Windows.Forms.ToolStripButton ui_newUserTSB;
    }
}
