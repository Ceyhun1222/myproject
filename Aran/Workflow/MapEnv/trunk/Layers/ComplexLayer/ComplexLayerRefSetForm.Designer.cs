namespace MapEnv.ComplexLayer
{
    partial class ComplexLayerRefSetForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
			this.components = new System.ComponentModel.Container();
			MapEnv.Controls.LineControl lineControl1;
			this.ui_okButton = new System.Windows.Forms.Button();
			this.ui_cancelButton = new System.Windows.Forms.Button();
			this.ui_titleLabel = new System.Windows.Forms.Label();
			this.ui_leftTreeView = new System.Windows.Forms.TreeView();
			this.ui_treeViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.ui_middlePanel = new System.Windows.Forms.Panel();
			this.ui_removeButton = new System.Windows.Forms.Button();
			this.ui_addButton = new System.Windows.Forms.Button();
			this.ui_rightTreeView = new System.Windows.Forms.TreeView();
			lineControl1 = new MapEnv.Controls.LineControl();
			this.flowLayoutPanel1.SuspendLayout();
			this.ui_middlePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// lineControl1
			// 
			lineControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			lineControl1.Location = new System.Drawing.Point(-17, 371);
			lineControl1.Name = "lineControl1";
			lineControl1.Size = new System.Drawing.Size(781, 8);
			lineControl1.TabIndex = 6;
			// 
			// ui_okButton
			// 
			this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_okButton.Location = new System.Drawing.Point(579, 385);
			this.ui_okButton.Name = "ui_okButton";
			this.ui_okButton.Size = new System.Drawing.Size(75, 23);
			this.ui_okButton.TabIndex = 0;
			this.ui_okButton.Text = "OK";
			this.ui_okButton.UseVisualStyleBackColor = true;
			this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
			// 
			// ui_cancelButton
			// 
			this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_cancelButton.Location = new System.Drawing.Point(660, 385);
			this.ui_cancelButton.Name = "ui_cancelButton";
			this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
			this.ui_cancelButton.TabIndex = 1;
			this.ui_cancelButton.Text = "Cancel";
			this.ui_cancelButton.UseVisualStyleBackColor = true;
			this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// ui_titleLabel
			// 
			this.ui_titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_titleLabel.BackColor = System.Drawing.SystemColors.Window;
			this.ui_titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ui_titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ui_titleLabel.Location = new System.Drawing.Point(-6, -8);
			this.ui_titleLabel.Name = "ui_titleLabel";
			this.ui_titleLabel.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.ui_titleLabel.Size = new System.Drawing.Size(761, 50);
			this.ui_titleLabel.TabIndex = 3;
			this.ui_titleLabel.Text = "<FeatureType>";
			this.ui_titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ui_leftTreeView
			// 
			this.ui_leftTreeView.CheckBoxes = true;
			this.ui_leftTreeView.ImageIndex = 0;
			this.ui_leftTreeView.ImageList = this.ui_treeViewImageList;
			this.ui_leftTreeView.Location = new System.Drawing.Point(3, 3);
			this.ui_leftTreeView.Name = "ui_leftTreeView";
			this.ui_leftTreeView.SelectedImageIndex = 0;
			this.ui_leftTreeView.Size = new System.Drawing.Size(102, 161);
			this.ui_leftTreeView.TabIndex = 4;
			// 
			// ui_treeViewImageList
			// 
			this.ui_treeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.ui_treeViewImageList.ImageSize = new System.Drawing.Size(24, 24);
			this.ui_treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.ui_leftTreeView);
			this.flowLayoutPanel1.Controls.Add(this.ui_middlePanel);
			this.flowLayoutPanel1.Controls.Add(this.ui_rightTreeView);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 45);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(295, 167);
			this.flowLayoutPanel1.TabIndex = 5;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// ui_middlePanel
			// 
			this.ui_middlePanel.Controls.Add(this.ui_removeButton);
			this.ui_middlePanel.Controls.Add(this.ui_addButton);
			this.ui_middlePanel.Location = new System.Drawing.Point(111, 3);
			this.ui_middlePanel.Name = "ui_middlePanel";
			this.ui_middlePanel.Size = new System.Drawing.Size(73, 161);
			this.ui_middlePanel.TabIndex = 5;
			// 
			// ui_removeButton
			// 
			this.ui_removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_removeButton.Image = global::MapEnv.Properties.Resources.remove_to_left;
			this.ui_removeButton.Location = new System.Drawing.Point(13, 44);
			this.ui_removeButton.Name = "ui_removeButton";
			this.ui_removeButton.Size = new System.Drawing.Size(43, 35);
			this.ui_removeButton.TabIndex = 1;
			this.ui_removeButton.UseVisualStyleBackColor = true;
			this.ui_removeButton.Click += new System.EventHandler(this.RemoveToLeft_Click);
			// 
			// ui_addButton
			// 
			this.ui_addButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_addButton.Image = global::MapEnv.Properties.Resources.add_to_right_32;
			this.ui_addButton.Location = new System.Drawing.Point(13, 3);
			this.ui_addButton.Name = "ui_addButton";
			this.ui_addButton.Size = new System.Drawing.Size(43, 35);
			this.ui_addButton.TabIndex = 0;
			this.ui_addButton.UseVisualStyleBackColor = true;
			this.ui_addButton.Click += new System.EventHandler(this.AddToRight_Click);
			// 
			// ui_rightTreeView
			// 
			this.ui_rightTreeView.ImageIndex = 0;
			this.ui_rightTreeView.ImageList = this.ui_treeViewImageList;
			this.ui_rightTreeView.Location = new System.Drawing.Point(190, 3);
			this.ui_rightTreeView.Name = "ui_rightTreeView";
			this.ui_rightTreeView.SelectedImageIndex = 0;
			this.ui_rightTreeView.Size = new System.Drawing.Size(102, 161);
			this.ui_rightTreeView.TabIndex = 6;
			// 
			// ComplexLayerRefSetForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(747, 420);
			this.Controls.Add(this.ui_cancelButton);
			this.Controls.Add(lineControl1);
			this.Controls.Add(this.ui_okButton);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.ui_titleLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ComplexLayerRefSetForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Setup Complex Layer Reference";
			this.SizeChanged += new System.EventHandler(this.ComplexLayerRefSetForm_SizeChanged);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ui_middlePanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Label ui_titleLabel;
        private System.Windows.Forms.TreeView ui_leftTreeView;
        private System.Windows.Forms.ImageList ui_treeViewImageList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel ui_middlePanel;
        private System.Windows.Forms.TreeView ui_rightTreeView;
        private System.Windows.Forms.Button ui_addButton;
        private System.Windows.Forms.Button ui_removeButton;
    }
}