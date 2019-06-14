namespace Aran.Controls
{
    partial class TDIControl
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
            this.components = new System.ComponentModel.Container();
            this.ui_buttonBarPanel = new System.Windows.Forms.Panel();
            this.ui_pageBarFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_workAreaPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ui_buttonBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_buttonBarPanel
            // 
            this.ui_buttonBarPanel.Controls.Add(this.ui_pageBarFlowLayoutPanel);
            this.ui_buttonBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_buttonBarPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_buttonBarPanel.Name = "ui_buttonBarPanel";
            this.ui_buttonBarPanel.Size = new System.Drawing.Size(671, 35);
            this.ui_buttonBarPanel.TabIndex = 0;
            // 
            // ui_pageBarFlowLayoutPanel
            // 
            this.ui_pageBarFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_pageBarFlowLayoutPanel.Location = new System.Drawing.Point(2, 2);
            this.ui_pageBarFlowLayoutPanel.Name = "ui_pageBarFlowLayoutPanel";
            this.ui_pageBarFlowLayoutPanel.Size = new System.Drawing.Size(666, 29);
            this.ui_pageBarFlowLayoutPanel.TabIndex = 0;
            // 
            // ui_workAreaPanel
            // 
            this.ui_workAreaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ui_workAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_workAreaPanel.Location = new System.Drawing.Point(0, 35);
            this.ui_workAreaPanel.Name = "ui_workAreaPanel";
            this.ui_workAreaPanel.Size = new System.Drawing.Size(671, 427);
            this.ui_workAreaPanel.TabIndex = 1;
            // 
            // TDIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CausesValidation = false;
            this.Controls.Add(this.ui_workAreaPanel);
            this.Controls.Add(this.ui_buttonBarPanel);
            this.Name = "TDIControl";
            this.Size = new System.Drawing.Size(671, 462);
            this.ui_buttonBarPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ui_buttonBarPanel;
        private System.Windows.Forms.Panel ui_workAreaPanel;
        private System.Windows.Forms.FlowLayoutPanel ui_pageBarFlowLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
