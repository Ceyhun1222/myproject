namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnl_MainPanel = new System.Windows.Forms.Panel();
            this.btn_Prev = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btn_DrawnElements = new System.Windows.Forms.Button();
            this.btn_Parameters = new System.Windows.Forms.Button();
            this.btn_Report = new System.Windows.Forms.CheckBox();
            this.btn_newVisualFeature = new System.Windows.Forms.Button();
            this.toolTip_reportBtn = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_parameterBtn = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_drawnElementsBtn = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_vfCreatorBtn = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // pnl_MainPanel
            // 
            this.pnl_MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_MainPanel.Location = new System.Drawing.Point(10, 12);
            this.pnl_MainPanel.Name = "pnl_MainPanel";
            this.pnl_MainPanel.Size = new System.Drawing.Size(560, 400);
            this.pnl_MainPanel.TabIndex = 1;
            // 
            // btn_Prev
            // 
            this.btn_Prev.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Prev.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Prev.Enabled = false;
            this.btn_Prev.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Prev.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Prev.Image = ((System.Drawing.Image)(resources.GetObject("btn_Prev.Image")));
            this.btn_Prev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Prev.Location = new System.Drawing.Point(10, 421);
            this.btn_Prev.Name = "btn_Prev";
            this.btn_Prev.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_Prev.Size = new System.Drawing.Size(84, 29);
            this.btn_Prev.TabIndex = 522;
            this.btn_Prev.Text = "Prev";
            this.btn_Prev.UseVisualStyleBackColor = false;
            this.btn_Prev.Click += new System.EventHandler(this.btn_Prev_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Cancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(280, 421);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_Cancel.Size = new System.Drawing.Size(84, 29);
            this.btn_Cancel.TabIndex = 525;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Ok.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Ok.Enabled = false;
            this.btn_Ok.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_Ok.Image")));
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(190, 421);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_Ok.Size = new System.Drawing.Size(84, 29);
            this.btn_Ok.TabIndex = 524;
            this.btn_Ok.Text = "Ok";
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Next.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Next.Enabled = false;
            this.btn_Next.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Next.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Next.Image = ((System.Drawing.Image)(resources.GetObject("btn_Next.Image")));
            this.btn_Next.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Next.Location = new System.Drawing.Point(100, 421);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_Next.Size = new System.Drawing.Size(84, 29);
            this.btn_Next.TabIndex = 523;
            this.btn_Next.Text = "Next";
            this.btn_Next.UseVisualStyleBackColor = false;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btn_DrawnElements
            // 
            this.btn_DrawnElements.Image = ((System.Drawing.Image)(resources.GetObject("btn_DrawnElements.Image")));
            this.btn_DrawnElements.Location = new System.Drawing.Point(495, 421);
            this.btn_DrawnElements.Name = "btn_DrawnElements";
            this.btn_DrawnElements.Size = new System.Drawing.Size(34, 29);
            this.btn_DrawnElements.TabIndex = 528;
            this.toolTip_drawnElementsBtn.SetToolTip(this.btn_DrawnElements, "Drawn Elements");
            this.btn_DrawnElements.UseVisualStyleBackColor = true;
            this.btn_DrawnElements.Click += new System.EventHandler(this.btn_DrawnElements_Click);
            // 
            // btn_Parameters
            // 
            this.btn_Parameters.Image = global::Aran.Panda.VisualManoeuvring.Properties.Resources.settings;
            this.btn_Parameters.Location = new System.Drawing.Point(455, 421);
            this.btn_Parameters.Name = "btn_Parameters";
            this.btn_Parameters.Size = new System.Drawing.Size(34, 29);
            this.btn_Parameters.TabIndex = 527;
            this.toolTip_parameterBtn.SetToolTip(this.btn_Parameters, "Parameters");
            this.btn_Parameters.UseVisualStyleBackColor = true;
            this.btn_Parameters.Click += new System.EventHandler(this.btn_Parameters_Click);
            // 
            // btn_Report
            // 
            this.btn_Report.Appearance = System.Windows.Forms.Appearance.Button;
            this.btn_Report.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Report.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_Report.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Report.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Report.Image = ((System.Drawing.Image)(resources.GetObject("btn_Report.Image")));
            this.btn_Report.Location = new System.Drawing.Point(413, 421);
            this.btn_Report.Name = "btn_Report";
            this.btn_Report.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_Report.Size = new System.Drawing.Size(36, 29);
            this.btn_Report.TabIndex = 526;
            this.btn_Report.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip_reportBtn.SetToolTip(this.btn_Report, "Report");
            this.btn_Report.UseVisualStyleBackColor = false;
            this.btn_Report.CheckedChanged += new System.EventHandler(this.btn_Report_CheckedChanged);
            // 
            // btn_newVisualFeature
            // 
            this.btn_newVisualFeature.Image = ((System.Drawing.Image)(resources.GetObject("btn_newVisualFeature.Image")));
            this.btn_newVisualFeature.Location = new System.Drawing.Point(535, 421);
            this.btn_newVisualFeature.Name = "btn_newVisualFeature";
            this.btn_newVisualFeature.Size = new System.Drawing.Size(34, 29);
            this.btn_newVisualFeature.TabIndex = 529;
            this.toolTip_drawnElementsBtn.SetToolTip(this.btn_newVisualFeature, "Visual Feature Creator");
            this.btn_newVisualFeature.UseVisualStyleBackColor = true;
            this.btn_newVisualFeature.Click += new System.EventHandler(this.btn_newVisualFeature_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 453);
            this.Controls.Add(this.btn_newVisualFeature);
            this.Controls.Add(this.btn_DrawnElements);
            this.Controls.Add(this.btn_Parameters);
            this.Controls.Add(this.btn_Report);
            this.Controls.Add(this.btn_Prev);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.pnl_MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Visual Manoeuvring using prescribed track";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_MainPanel;
        public System.Windows.Forms.Button btn_Prev;
        public System.Windows.Forms.Button btn_Cancel;
        public System.Windows.Forms.Button btn_Ok;
        public System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.Button btn_DrawnElements;
        private System.Windows.Forms.Button btn_Parameters;
        public System.Windows.Forms.CheckBox btn_Report;
        private System.Windows.Forms.Button btn_newVisualFeature;
        private System.Windows.Forms.ToolTip toolTip_reportBtn;
        private System.Windows.Forms.ToolTip toolTip_parameterBtn;
        private System.Windows.Forms.ToolTip toolTip_drawnElementsBtn;
        private System.Windows.Forms.ToolTip toolTip_vfCreatorBtn;

    }
}