namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class NS_Page4
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
            this.txtBox_convergenceAngle = new System.Windows.Forms.TextBox();
            this.txtBox_divergenceAngle = new System.Windows.Forms.TextBox();
            this.lbl_divergenceAngle = new System.Windows.Forms.Label();
            this.lbl_convergencePnt = new System.Windows.Forms.Label();
            this.lbl_VFsWithinPoly = new System.Windows.Forms.Label();
            this.cmbBox_VFsWithinPoly = new System.Windows.Forms.ComboBox();
            this.nmrcUpDown_finalDirection = new System.Windows.Forms.NumericUpDown();
            this.lbl_convergenceAngle = new System.Windows.Forms.Label();
            this.lbl_finalDirectionRange = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_finalDirection)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBox_convergenceAngle
            // 
            this.txtBox_convergenceAngle.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_convergenceAngle.Location = new System.Drawing.Point(112, 188);
            this.txtBox_convergenceAngle.Name = "txtBox_convergenceAngle";
            this.txtBox_convergenceAngle.ReadOnly = true;
            this.txtBox_convergenceAngle.Size = new System.Drawing.Size(100, 20);
            this.txtBox_convergenceAngle.TabIndex = 81;
            // 
            // txtBox_divergenceAngle
            // 
            this.txtBox_divergenceAngle.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_divergenceAngle.Location = new System.Drawing.Point(112, 149);
            this.txtBox_divergenceAngle.Name = "txtBox_divergenceAngle";
            this.txtBox_divergenceAngle.ReadOnly = true;
            this.txtBox_divergenceAngle.Size = new System.Drawing.Size(100, 20);
            this.txtBox_divergenceAngle.TabIndex = 80;
            // 
            // lbl_divergenceAngle
            // 
            this.lbl_divergenceAngle.AutoSize = true;
            this.lbl_divergenceAngle.Location = new System.Drawing.Point(3, 152);
            this.lbl_divergenceAngle.Name = "lbl_divergenceAngle";
            this.lbl_divergenceAngle.Size = new System.Drawing.Size(94, 13);
            this.lbl_divergenceAngle.TabIndex = 79;
            this.lbl_divergenceAngle.Text = "Divergence angle:";
            // 
            // lbl_convergencePnt
            // 
            this.lbl_convergencePnt.AutoSize = true;
            this.lbl_convergencePnt.Location = new System.Drawing.Point(3, 191);
            this.lbl_convergencePnt.Name = "lbl_convergencePnt";
            this.lbl_convergencePnt.Size = new System.Drawing.Size(103, 13);
            this.lbl_convergencePnt.TabIndex = 78;
            this.lbl_convergencePnt.Text = "Convergence angle:";
            // 
            // lbl_VFsWithinPoly
            // 
            this.lbl_VFsWithinPoly.AutoSize = true;
            this.lbl_VFsWithinPoly.Location = new System.Drawing.Point(3, 17);
            this.lbl_VFsWithinPoly.Name = "lbl_VFsWithinPoly";
            this.lbl_VFsWithinPoly.Size = new System.Drawing.Size(80, 13);
            this.lbl_VFsWithinPoly.TabIndex = 75;
            this.lbl_VFsWithinPoly.Text = "VFs within poly:";
            // 
            // cmbBox_VFsWithinPoly
            // 
            this.cmbBox_VFsWithinPoly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_VFsWithinPoly.FormattingEnabled = true;
            this.cmbBox_VFsWithinPoly.Location = new System.Drawing.Point(112, 14);
            this.cmbBox_VFsWithinPoly.Name = "cmbBox_VFsWithinPoly";
            this.cmbBox_VFsWithinPoly.Size = new System.Drawing.Size(100, 21);
            this.cmbBox_VFsWithinPoly.TabIndex = 74;
            this.cmbBox_VFsWithinPoly.SelectedIndexChanged += new System.EventHandler(this.cmbBox_VFsWithinPoly_SelectedIndexChanged);
            // 
            // nmrcUpDown_finalDirection
            // 
            this.nmrcUpDown_finalDirection.BackColor = System.Drawing.SystemColors.Window;
            this.nmrcUpDown_finalDirection.Location = new System.Drawing.Point(342, 15);
            this.nmrcUpDown_finalDirection.Name = "nmrcUpDown_finalDirection";
            this.nmrcUpDown_finalDirection.Size = new System.Drawing.Size(100, 20);
            this.nmrcUpDown_finalDirection.TabIndex = 84;
            this.nmrcUpDown_finalDirection.ValueChanged += new System.EventHandler(this.nmrcUpDown_finalDirection_ValueChanged);
            // 
            // lbl_convergenceAngle
            // 
            this.lbl_convergenceAngle.AutoSize = true;
            this.lbl_convergenceAngle.Location = new System.Drawing.Point(261, 17);
            this.lbl_convergenceAngle.Name = "lbl_convergenceAngle";
            this.lbl_convergenceAngle.Size = new System.Drawing.Size(75, 13);
            this.lbl_convergenceAngle.TabIndex = 83;
            this.lbl_convergenceAngle.Text = "Final direction:";
            // 
            // lbl_finalDirectionRange
            // 
            this.lbl_finalDirectionRange.AutoSize = true;
            this.lbl_finalDirectionRange.Location = new System.Drawing.Point(448, 17);
            this.lbl_finalDirectionRange.Name = "lbl_finalDirectionRange";
            this.lbl_finalDirectionRange.Size = new System.Drawing.Size(100, 13);
            this.lbl_finalDirectionRange.TabIndex = 82;
            this.lbl_finalDirectionRange.Text = "finalDirectionRange";
            // 
            // NS_Page4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nmrcUpDown_finalDirection);
            this.Controls.Add(this.lbl_convergenceAngle);
            this.Controls.Add(this.lbl_finalDirectionRange);
            this.Controls.Add(this.txtBox_convergenceAngle);
            this.Controls.Add(this.txtBox_divergenceAngle);
            this.Controls.Add(this.lbl_divergenceAngle);
            this.Controls.Add(this.lbl_convergencePnt);
            this.Controls.Add(this.lbl_VFsWithinPoly);
            this.Controls.Add(this.cmbBox_VFsWithinPoly);
            this.Name = "NS_Page4";
            this.Size = new System.Drawing.Size(560, 400);
            this.VisibleChanged += new System.EventHandler(this.NS_Page4_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_finalDirection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_convergenceAngle;
        private System.Windows.Forms.TextBox txtBox_divergenceAngle;
        private System.Windows.Forms.Label lbl_divergenceAngle;
        private System.Windows.Forms.Label lbl_convergencePnt;
        private System.Windows.Forms.Label lbl_VFsWithinPoly;
        private System.Windows.Forms.ComboBox cmbBox_VFsWithinPoly;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_finalDirection;
        private System.Windows.Forms.Label lbl_convergenceAngle;
        private System.Windows.Forms.Label lbl_finalDirectionRange;
    }
}
