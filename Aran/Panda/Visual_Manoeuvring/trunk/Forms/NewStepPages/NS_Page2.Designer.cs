namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class NS_Page2
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
            this.grpBox_eliminationCriteria1 = new System.Windows.Forms.GroupBox();
            this.txtBox_maxConvergenceAngle = new System.Windows.Forms.TextBox();
            this.txtBox_maxDivergenceAngle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_maxDivergenceAngle = new System.Windows.Forms.Label();
            this.grpBox_eliminationCriteria1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_eliminationCriteria1
            // 
            this.grpBox_eliminationCriteria1.Controls.Add(this.txtBox_maxConvergenceAngle);
            this.grpBox_eliminationCriteria1.Controls.Add(this.txtBox_maxDivergenceAngle);
            this.grpBox_eliminationCriteria1.Controls.Add(this.label2);
            this.grpBox_eliminationCriteria1.Controls.Add(this.lbl_maxDivergenceAngle);
            this.grpBox_eliminationCriteria1.Location = new System.Drawing.Point(3, 3);
            this.grpBox_eliminationCriteria1.Name = "grpBox_eliminationCriteria1";
            this.grpBox_eliminationCriteria1.Size = new System.Drawing.Size(264, 109);
            this.grpBox_eliminationCriteria1.TabIndex = 2;
            this.grpBox_eliminationCriteria1.TabStop = false;
            this.grpBox_eliminationCriteria1.Text = "Elimination Criteria 1";
            // 
            // txtBox_maxConvergenceAngle
            // 
            this.txtBox_maxConvergenceAngle.Location = new System.Drawing.Point(141, 64);
            this.txtBox_maxConvergenceAngle.Name = "txtBox_maxConvergenceAngle";
            this.txtBox_maxConvergenceAngle.Size = new System.Drawing.Size(100, 20);
            this.txtBox_maxConvergenceAngle.TabIndex = 3;
            this.txtBox_maxConvergenceAngle.TextChanged += new System.EventHandler(this.txtBox_maxConvergenceAngle_TextChanged);
            this.txtBox_maxConvergenceAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_maxConvergenceAngle_KeyPress);
            // 
            // txtBox_maxDivergenceAngle
            // 
            this.txtBox_maxDivergenceAngle.Location = new System.Drawing.Point(141, 27);
            this.txtBox_maxDivergenceAngle.Name = "txtBox_maxDivergenceAngle";
            this.txtBox_maxDivergenceAngle.Size = new System.Drawing.Size(100, 20);
            this.txtBox_maxDivergenceAngle.TabIndex = 2;
            this.txtBox_maxDivergenceAngle.TextChanged += new System.EventHandler(this.txtBox_maxDivergenceAngle_TextChanged);
            this.txtBox_maxDivergenceAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_maxDivergenceAngle_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max. convergence angle:";
            // 
            // lbl_maxDivergenceAngle
            // 
            this.lbl_maxDivergenceAngle.AutoSize = true;
            this.lbl_maxDivergenceAngle.Location = new System.Drawing.Point(7, 30);
            this.lbl_maxDivergenceAngle.Name = "lbl_maxDivergenceAngle";
            this.lbl_maxDivergenceAngle.Size = new System.Drawing.Size(118, 13);
            this.lbl_maxDivergenceAngle.TabIndex = 0;
            this.lbl_maxDivergenceAngle.Text = "Max. divergence angle:";
            // 
            // NS_Page2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_eliminationCriteria1);
            this.Name = "NS_Page2";
            this.Size = new System.Drawing.Size(560, 400);
            this.Load += new System.EventHandler(this.NS_Page2_Load);
            this.grpBox_eliminationCriteria1.ResumeLayout(false);
            this.grpBox_eliminationCriteria1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox_eliminationCriteria1;
        private System.Windows.Forms.TextBox txtBox_maxConvergenceAngle;
        private System.Windows.Forms.TextBox txtBox_maxDivergenceAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_maxDivergenceAngle;
    }
}
