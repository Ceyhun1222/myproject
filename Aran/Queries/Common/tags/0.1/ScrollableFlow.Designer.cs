namespace Aran.Queries.Common
{
    partial class ScrollableFlow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.heararchyFlowContainer = new System.Windows.Forms.Panel ();
            this.mainPanel = new System.Windows.Forms.Panel ();
            this.Flow = new System.Windows.Forms.FlowLayoutPanel ();
            this.rightPanel = new System.Windows.Forms.Panel ();
            this.rightLabel = new System.Windows.Forms.Label ();
            this.leftPanel = new System.Windows.Forms.Panel ();
            this.leftLabel = new System.Windows.Forms.Label ();
            this.heararchyFlowContainer.SuspendLayout ();
            this.mainPanel.SuspendLayout ();
            this.rightPanel.SuspendLayout ();
            this.leftPanel.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // heararchyFlowContainer
            // 
            this.heararchyFlowContainer.Controls.Add (this.mainPanel);
            this.heararchyFlowContainer.Controls.Add (this.rightPanel);
            this.heararchyFlowContainer.Controls.Add (this.leftPanel);
            this.heararchyFlowContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.heararchyFlowContainer.Location = new System.Drawing.Point (0, 0);
            this.heararchyFlowContainer.Name = "heararchyFlowContainer";
            this.heararchyFlowContainer.Size = new System.Drawing.Size (499, 47);
            this.heararchyFlowContainer.TabIndex = 7;
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainPanel.Controls.Add (this.Flow);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point (63, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size (378, 47);
            this.mainPanel.TabIndex = 9;
            // 
            // Flow
            // 
            this.Flow.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Flow.AutoScroll = true;
            this.Flow.Location = new System.Drawing.Point (1, 1);
            this.Flow.Name = "Flow";
            this.Flow.Size = new System.Drawing.Size (372, 41);
            this.Flow.TabIndex = 3;
            this.Flow.WrapContents = false;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add (this.rightLabel);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point (441, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size (58, 47);
            this.rightPanel.TabIndex = 1;
            this.rightPanel.Visible = false;
            // 
            // rightLabel
            // 
            this.rightLabel.AutoSize = true;
            this.rightLabel.Font = new System.Drawing.Font ("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.rightLabel.Location = new System.Drawing.Point (15, 11);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size (18, 14);
            this.rightLabel.TabIndex = 0;
            this.rightLabel.Text = "►";
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add (this.leftLabel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point (0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size (63, 47);
            this.leftPanel.TabIndex = 0;
            this.leftPanel.Visible = false;
            // 
            // leftLabel
            // 
            this.leftLabel.AutoSize = true;
            this.leftLabel.Font = new System.Drawing.Font ("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.leftLabel.Location = new System.Drawing.Point (23, 15);
            this.leftLabel.Name = "leftLabel";
            this.leftLabel.Size = new System.Drawing.Size (18, 14);
            this.leftLabel.TabIndex = 0;
            this.leftLabel.Text = "◄";
            this.leftLabel.Click += new System.EventHandler (this.leftLabel_Click);
            // 
            // ScrollableFlow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add (this.heararchyFlowContainer);
            this.Name = "ScrollableFlow";
            this.Size = new System.Drawing.Size (499, 47);
            this.heararchyFlowContainer.ResumeLayout (false);
            this.mainPanel.ResumeLayout (false);
            this.rightPanel.ResumeLayout (false);
            this.rightPanel.PerformLayout ();
            this.leftPanel.ResumeLayout (false);
            this.leftPanel.PerformLayout ();
            this.ResumeLayout (false);

        }

        #endregion

        private System.Windows.Forms.Panel heararchyFlowContainer;
        private System.Windows.Forms.Panel mainPanel;
        public System.Windows.Forms.FlowLayoutPanel Flow;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Label rightLabel;
        private System.Windows.Forms.Label leftLabel;
    }
}
