﻿namespace MapEnv
{
    partial class PropertySelectorForm
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
            this.ui_treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // ui_treeView
            // 
            this.ui_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_treeView.Location = new System.Drawing.Point(2, 2);
            this.ui_treeView.Name = "ui_treeView";
            this.ui_treeView.Size = new System.Drawing.Size(370, 351);
            this.ui_treeView.TabIndex = 1;
            this.ui_treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.uiEvents_treeView_BeforeExpand);
            this.ui_treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.uiEvents_treeView_NodeMouseDoubleClick);
            // 
            // PropertySelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 355);
            this.Controls.Add(this.ui_treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PropertySelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PropertySelectorForm";
            this.Deactivate += new System.EventHandler(this.PropertySelectorForm_Deactivate);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView ui_treeView;
    }
}