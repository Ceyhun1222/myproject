namespace Aran.Aim.Data.LocalDbLoader
{
    partial class LoaderForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.PictureBox pictureBox1;
            this.ui_featTypeLV = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ui_mainTS = new System.Windows.Forms.ToolStrip();
            this.ui_messagePanel = new System.Windows.Forms.Panel();
            this.ui_messagesTB = new System.Windows.Forms.TextBox();
            this.ui_loadingPanel = new System.Windows.Forms.Panel();
            this.ui_loadingLabel = new System.Windows.Forms.Label();
            this.ui_addTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_removeTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_reloadTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_reloadAllTSB = new System.Windows.Forms.ToolStripButton();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ui_mainTS.SuspendLayout();
            this.ui_messagePanel.SuspendLayout();
            this.ui_loadingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(2, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 13);
            label1.TabIndex = 0;
            label1.Text = "Messages:";
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Font = new System.Drawing.Font("Wingdings 2", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            button1.Location = new System.Drawing.Point(312, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(24, 22);
            button1.TabIndex = 3;
            button1.Text = "Ñ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_featTypeLV
            // 
            this.ui_featTypeLV.CheckBoxes = true;
            this.ui_featTypeLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ui_featTypeLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featTypeLV.HideSelection = false;
            this.ui_featTypeLV.Location = new System.Drawing.Point(0, 59);
            this.ui_featTypeLV.Name = "ui_featTypeLV";
            this.ui_featTypeLV.Size = new System.Drawing.Size(341, 308);
            this.ui_featTypeLV.TabIndex = 1;
            this.ui_featTypeLV.UseCompatibleStateImageBehavior = false;
            this.ui_featTypeLV.View = System.Windows.Forms.View.Details;
            this.ui_featTypeLV.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.FeatureType_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 260;
            // 
            // ui_mainTS
            // 
            this.ui_mainTS.AutoSize = false;
            this.ui_mainTS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ui_mainTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_addTSB,
            this.ui_removeTSB,
            this.ui_reloadTSB,
            this.ui_reloadAllTSB});
            this.ui_mainTS.Location = new System.Drawing.Point(0, 0);
            this.ui_mainTS.Name = "ui_mainTS";
            this.ui_mainTS.Size = new System.Drawing.Size(341, 29);
            this.ui_mainTS.TabIndex = 2;
            this.ui_mainTS.Text = "toolStrip1";
            // 
            // ui_messagePanel
            // 
            this.ui_messagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_messagePanel.Controls.Add(button1);
            this.ui_messagePanel.Controls.Add(this.ui_messagesTB);
            this.ui_messagePanel.Controls.Add(label1);
            this.ui_messagePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ui_messagePanel.Location = new System.Drawing.Point(0, 367);
            this.ui_messagePanel.Name = "ui_messagePanel";
            this.ui_messagePanel.Size = new System.Drawing.Size(341, 133);
            this.ui_messagePanel.TabIndex = 3;
            this.ui_messagePanel.Visible = false;
            // 
            // ui_messagesTB
            // 
            this.ui_messagesTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_messagesTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_messagesTB.Location = new System.Drawing.Point(2, 25);
            this.ui_messagesTB.Multiline = true;
            this.ui_messagesTB.Name = "ui_messagesTB";
            this.ui_messagesTB.ReadOnly = true;
            this.ui_messagesTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ui_messagesTB.Size = new System.Drawing.Size(334, 102);
            this.ui_messagesTB.TabIndex = 1;
            this.ui_messagesTB.WordWrap = false;
            // 
            // ui_loadingPanel
            // 
            this.ui_loadingPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ui_loadingPanel.Controls.Add(this.ui_loadingLabel);
            this.ui_loadingPanel.Controls.Add(pictureBox1);
            this.ui_loadingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_loadingPanel.Location = new System.Drawing.Point(0, 29);
            this.ui_loadingPanel.Name = "ui_loadingPanel";
            this.ui_loadingPanel.Size = new System.Drawing.Size(341, 30);
            this.ui_loadingPanel.TabIndex = 4;
            this.ui_loadingPanel.Visible = false;
            // 
            // ui_loadingLabel
            // 
            this.ui_loadingLabel.AutoSize = true;
            this.ui_loadingLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ui_loadingLabel.Location = new System.Drawing.Point(40, 8);
            this.ui_loadingLabel.Name = "ui_loadingLabel";
            this.ui_loadingLabel.Size = new System.Drawing.Size(50, 13);
            this.ui_loadingLabel.TabIndex = 0;
            this.ui_loadingLabel.Text = "loading...";
            // 
            // ui_addTSB
            // 
            this.ui_addTSB.Image = global::Aran.Aim.Data.LocalDbLoader.Properties.Resources.add;
            this.ui_addTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_addTSB.Name = "ui_addTSB";
            this.ui_addTSB.Size = new System.Drawing.Size(49, 26);
            this.ui_addTSB.Text = "Add";
            this.ui_addTSB.Click += new System.EventHandler(this.AddFeatureType_Click);
            // 
            // ui_removeTSB
            // 
            this.ui_removeTSB.Enabled = false;
            this.ui_removeTSB.Image = global::Aran.Aim.Data.LocalDbLoader.Properties.Resources.remove;
            this.ui_removeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_removeTSB.Name = "ui_removeTSB";
            this.ui_removeTSB.Size = new System.Drawing.Size(70, 26);
            this.ui_removeTSB.Text = "Remove";
            this.ui_removeTSB.ToolTipText = "Remove Checked Items";
            this.ui_removeTSB.Click += new System.EventHandler(this.RemoveFeatureType_Click);
            // 
            // ui_reloadTSB
            // 
            this.ui_reloadTSB.Image = global::Aran.Aim.Data.LocalDbLoader.Properties.Resources.reload;
            this.ui_reloadTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_reloadTSB.Name = "ui_reloadTSB";
            this.ui_reloadTSB.Size = new System.Drawing.Size(63, 26);
            this.ui_reloadTSB.Text = "Reload";
            this.ui_reloadTSB.ToolTipText = "Reload Checked Items";
            this.ui_reloadTSB.Click += new System.EventHandler(this.ReloadChecked_Click);
            // 
            // ui_reloadAllTSB
            // 
            this.ui_reloadAllTSB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ui_reloadAllTSB.Image = global::Aran.Aim.Data.LocalDbLoader.Properties.Resources.reload;
            this.ui_reloadAllTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_reloadAllTSB.Name = "ui_reloadAllTSB";
            this.ui_reloadAllTSB.Size = new System.Drawing.Size(80, 26);
            this.ui_reloadAllTSB.Text = "Reload All";
            this.ui_reloadAllTSB.Click += new System.EventHandler(this.ReloadAll_Click);
            // 
            // pictureBox1
            // 
            pictureBox1.Image = global::Aran.Aim.Data.LocalDbLoader.Properties.Resources.loader;
            pictureBox1.Location = new System.Drawing.Point(10, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(24, 24);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // LoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 500);
            this.Controls.Add(this.ui_featTypeLV);
            this.Controls.Add(this.ui_loadingPanel);
            this.Controls.Add(this.ui_messagePanel);
            this.Controls.Add(this.ui_mainTS);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(295, 340);
            this.Name = "LoaderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Cache Loader";
            this.ui_mainTS.ResumeLayout(false);
            this.ui_mainTS.PerformLayout();
            this.ui_messagePanel.ResumeLayout(false);
            this.ui_messagePanel.PerformLayout();
            this.ui_loadingPanel.ResumeLayout(false);
            this.ui_loadingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ui_featTypeLV;
        private System.Windows.Forms.ToolStrip ui_mainTS;
        private System.Windows.Forms.ToolStripButton ui_addTSB;
        private System.Windows.Forms.ToolStripButton ui_removeTSB;
        private System.Windows.Forms.ToolStripButton ui_reloadTSB;
        private System.Windows.Forms.ToolStripButton ui_reloadAllTSB;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel ui_messagePanel;
        private System.Windows.Forms.TextBox ui_messagesTB;
        private System.Windows.Forms.Panel ui_loadingPanel;
        private System.Windows.Forms.Label ui_loadingLabel;
    }
}