namespace AranUpdateManager
{
    partial class UserLogForm
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
            System.Windows.Forms.ToolStrip toolStrip1;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_titleTSLabel = new System.Windows.Forms.ToolStripLabel();
            this.ui_logDGV = new System.Windows.Forms.DataGridView();
            this.ui_colDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_logTB = new System.Windows.Forms.TextBox();
            this.ui_readModeUnreadRB = new System.Windows.Forms.RadioButton();
            this.ui_readModeAllRB = new System.Windows.Forms.RadioButton();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_logDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_titleTSLabel});
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(957, 44);
            toolStrip1.TabIndex = 10;
            toolStrip1.Text = "toolStrip1";
            // 
            // ui_titleTSLabel
            // 
            this.ui_titleTSLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_titleTSLabel.Margin = new System.Windows.Forms.Padding(160, 1, 10, 2);
            this.ui_titleTSLabel.Name = "ui_titleTSLabel";
            this.ui_titleTSLabel.Size = new System.Drawing.Size(63, 41);
            this.ui_titleTSLabel.Text = "<User>";
            // 
            // ui_logDGV
            // 
            this.ui_logDGV.AllowUserToAddRows = false;
            this.ui_logDGV.AllowUserToDeleteRows = false;
            this.ui_logDGV.AllowUserToResizeColumns = false;
            this.ui_logDGV.AllowUserToResizeRows = false;
            this.ui_logDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_logDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ui_logDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_logDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colDateTime});
            this.ui_logDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_logDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_logDGV.Margin = new System.Windows.Forms.Padding(4);
            this.ui_logDGV.MultiSelect = false;
            this.ui_logDGV.Name = "ui_logDGV";
            this.ui_logDGV.ReadOnly = true;
            this.ui_logDGV.RowHeadersVisible = false;
            this.ui_logDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_logDGV.Size = new System.Drawing.Size(350, 542);
            this.ui_logDGV.TabIndex = 5;
            this.ui_logDGV.CurrentCellChanged += new System.EventHandler(this.LogDGV_CurrentCellChanged);
            // 
            // ui_colDateTime
            // 
            this.ui_colDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colDateTime.HeaderText = "Date Time";
            this.ui_colDateTime.Name = "ui_colDateTime";
            this.ui_colDateTime.ReadOnly = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 48);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_logDGV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_logTB);
            this.splitContainer1.Size = new System.Drawing.Size(957, 542);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 6;
            // 
            // ui_logTB
            // 
            this.ui_logTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_logTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_logTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_logTB.Location = new System.Drawing.Point(0, 0);
            this.ui_logTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_logTB.Multiline = true;
            this.ui_logTB.Name = "ui_logTB";
            this.ui_logTB.ReadOnly = true;
            this.ui_logTB.Size = new System.Drawing.Size(602, 542);
            this.ui_logTB.TabIndex = 0;
            // 
            // ui_readModeUnreadRB
            // 
            this.ui_readModeUnreadRB.AutoSize = true;
            this.ui_readModeUnreadRB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_readModeUnreadRB.Checked = true;
            this.ui_readModeUnreadRB.Location = new System.Drawing.Point(16, 11);
            this.ui_readModeUnreadRB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_readModeUnreadRB.Name = "ui_readModeUnreadRB";
            this.ui_readModeUnreadRB.Size = new System.Drawing.Size(76, 21);
            this.ui_readModeUnreadRB.TabIndex = 7;
            this.ui_readModeUnreadRB.TabStop = true;
            this.ui_readModeUnreadRB.Text = "Unread";
            this.ui_readModeUnreadRB.UseVisualStyleBackColor = false;
            this.ui_readModeUnreadRB.CheckedChanged += new System.EventHandler(this.ReadMode_SelectedIndexChanged);
            // 
            // ui_readModeAllRB
            // 
            this.ui_readModeAllRB.AutoSize = true;
            this.ui_readModeAllRB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_readModeAllRB.Location = new System.Drawing.Point(109, 11);
            this.ui_readModeAllRB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_readModeAllRB.Name = "ui_readModeAllRB";
            this.ui_readModeAllRB.Size = new System.Drawing.Size(44, 21);
            this.ui_readModeAllRB.TabIndex = 8;
            this.ui_readModeAllRB.Text = "All";
            this.ui_readModeAllRB.UseVisualStyleBackColor = false;
            this.ui_readModeAllRB.CheckedChanged += new System.EventHandler(this.ReadMode_SelectedIndexChanged);
            // 
            // UserLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 590);
            this.Controls.Add(this.ui_readModeAllRB);
            this.Controls.Add(this.ui_readModeUnreadRB);
            this.Controls.Add(toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserLogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "User\'s Logs";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_logDGV)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_logDGV;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox ui_logTB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colDateTime;
        private System.Windows.Forms.RadioButton ui_readModeUnreadRB;
        private System.Windows.Forms.RadioButton ui_readModeAllRB;
        private System.Windows.Forms.ToolStripLabel ui_titleTSLabel;
    }
}