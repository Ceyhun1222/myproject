namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class NS_Page1
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
            this.grpBox_initialPnt = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_pickInitialPnt = new System.Windows.Forms.Button();
            this.txtBox_initialDirection = new System.Windows.Forms.TextBox();
            this.cmbBox_initialPntLonSide = new System.Windows.Forms.ComboBox();
            this.cmbBox_initialPntLatSide = new System.Windows.Forms.ComboBox();
            this.lbl_initialDirection = new System.Windows.Forms.Label();
            this.lbl_initialPntLonSecond = new System.Windows.Forms.Label();
            this.txtBox_initialPntLonSecond = new System.Windows.Forms.TextBox();
            this.lbl_initialPntLonMinute = new System.Windows.Forms.Label();
            this.txtBox_initialPntLonMinute = new System.Windows.Forms.TextBox();
            this.lbl_initialPntLonDegree = new System.Windows.Forms.Label();
            this.txtBox_initialPntLonDegree = new System.Windows.Forms.TextBox();
            this.lbl_initialPntLatSecond = new System.Windows.Forms.Label();
            this.lbl_initialPntLatMinute = new System.Windows.Forms.Label();
            this.txtBox_initialPntLatSecond = new System.Windows.Forms.TextBox();
            this.txtBox_initialPntLatMinute = new System.Windows.Forms.TextBox();
            this.lbl_initialPntLatDegree = new System.Windows.Forms.Label();
            this.txtBox_initialPntLatDegree = new System.Windows.Forms.TextBox();
            this.lbl_initialPntLongitude = new System.Windows.Forms.Label();
            this.lbl_initialPntLatitude = new System.Windows.Forms.Label();
            this.grpBox_eliminationCriteria1 = new System.Windows.Forms.GroupBox();
            this.txtBox_maxConvergenceAngle = new System.Windows.Forms.TextBox();
            this.txtBox_maxDivergenceAngle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_maxDivergenceAngle = new System.Windows.Forms.Label();
            this.grpBox_eliminationCriteria2 = new System.Windows.Forms.GroupBox();
            this.chkBox_isFinalStep = new System.Windows.Forms.CheckBox();
            this.grpBox_initialPnt.SuspendLayout();
            this.grpBox_eliminationCriteria1.SuspendLayout();
            this.grpBox_eliminationCriteria2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox_initialPnt
            // 
            this.grpBox_initialPnt.Controls.Add(this.label1);
            this.grpBox_initialPnt.Controls.Add(this.btn_pickInitialPnt);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialDirection);
            this.grpBox_initialPnt.Controls.Add(this.cmbBox_initialPntLonSide);
            this.grpBox_initialPnt.Controls.Add(this.cmbBox_initialPntLatSide);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialDirection);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLonSecond);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLonSecond);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLonMinute);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLonMinute);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLonDegree);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLonDegree);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLatSecond);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLatMinute);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLatSecond);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLatMinute);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLatDegree);
            this.grpBox_initialPnt.Controls.Add(this.txtBox_initialPntLatDegree);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLongitude);
            this.grpBox_initialPnt.Controls.Add(this.lbl_initialPntLatitude);
            this.grpBox_initialPnt.Location = new System.Drawing.Point(3, 3);
            this.grpBox_initialPnt.Name = "grpBox_initialPnt";
            this.grpBox_initialPnt.Size = new System.Drawing.Size(282, 131);
            this.grpBox_initialPnt.TabIndex = 62;
            this.grpBox_initialPnt.TabStop = false;
            this.grpBox_initialPnt.Text = "Initial Position Parameters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(209, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "°";
            // 
            // btn_pickInitialPnt
            // 
            this.btn_pickInitialPnt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_pickInitialPnt.Location = new System.Drawing.Point(226, 130);
            this.btn_pickInitialPnt.Name = "btn_pickInitialPnt";
            this.btn_pickInitialPnt.Size = new System.Drawing.Size(48, 23);
            this.btn_pickInitialPnt.TabIndex = 25;
            this.btn_pickInitialPnt.Text = "+";
            this.btn_pickInitialPnt.UseVisualStyleBackColor = true;
            this.btn_pickInitialPnt.Click += new System.EventHandler(this.btn_pickInitialPnt_Click);
            // 
            // txtBox_initialDirection
            // 
            this.txtBox_initialDirection.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_initialDirection.Location = new System.Drawing.Point(102, 103);
            this.txtBox_initialDirection.Name = "txtBox_initialDirection";
            this.txtBox_initialDirection.Size = new System.Drawing.Size(107, 20);
            this.txtBox_initialDirection.TabIndex = 65;
            this.txtBox_initialDirection.TextChanged += new System.EventHandler(this.txtBox_initialDirection_TextChanged);
            this.txtBox_initialDirection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_initialDirection_KeyPress);
            // 
            // cmbBox_initialPntLonSide
            // 
            this.cmbBox_initialPntLonSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_initialPntLonSide.FormattingEnabled = true;
            this.cmbBox_initialPntLonSide.Location = new System.Drawing.Point(226, 67);
            this.cmbBox_initialPntLonSide.Name = "cmbBox_initialPntLonSide";
            this.cmbBox_initialPntLonSide.Size = new System.Drawing.Size(48, 21);
            this.cmbBox_initialPntLonSide.TabIndex = 39;
            // 
            // cmbBox_initialPntLatSide
            // 
            this.cmbBox_initialPntLatSide.AllowDrop = true;
            this.cmbBox_initialPntLatSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_initialPntLatSide.Location = new System.Drawing.Point(226, 27);
            this.cmbBox_initialPntLatSide.Name = "cmbBox_initialPntLatSide";
            this.cmbBox_initialPntLatSide.Size = new System.Drawing.Size(48, 21);
            this.cmbBox_initialPntLatSide.TabIndex = 38;
            // 
            // lbl_initialDirection
            // 
            this.lbl_initialDirection.AutoSize = true;
            this.lbl_initialDirection.Location = new System.Drawing.Point(11, 106);
            this.lbl_initialDirection.Name = "lbl_initialDirection";
            this.lbl_initialDirection.Size = new System.Drawing.Size(77, 13);
            this.lbl_initialDirection.TabIndex = 64;
            this.lbl_initialDirection.Text = "Initial direction:";
            // 
            // lbl_initialPntLonSecond
            // 
            this.lbl_initialPntLonSecond.AutoSize = true;
            this.lbl_initialPntLonSecond.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLonSecond.Location = new System.Drawing.Point(209, 68);
            this.lbl_initialPntLonSecond.Name = "lbl_initialPntLonSecond";
            this.lbl_initialPntLonSecond.Size = new System.Drawing.Size(12, 13);
            this.lbl_initialPntLonSecond.TabIndex = 35;
            this.lbl_initialPntLonSecond.Text = "\"";
            // 
            // txtBox_initialPntLonSecond
            // 
            this.txtBox_initialPntLonSecond.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLonSecond.Location = new System.Drawing.Point(148, 67);
            this.txtBox_initialPntLonSecond.Name = "txtBox_initialPntLonSecond";
            this.txtBox_initialPntLonSecond.ReadOnly = true;
            this.txtBox_initialPntLonSecond.Size = new System.Drawing.Size(61, 20);
            this.txtBox_initialPntLonSecond.TabIndex = 34;
            // 
            // lbl_initialPntLonMinute
            // 
            this.lbl_initialPntLonMinute.AutoSize = true;
            this.lbl_initialPntLonMinute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLonMinute.Location = new System.Drawing.Point(132, 67);
            this.lbl_initialPntLonMinute.Name = "lbl_initialPntLonMinute";
            this.lbl_initialPntLonMinute.Size = new System.Drawing.Size(9, 13);
            this.lbl_initialPntLonMinute.TabIndex = 33;
            this.lbl_initialPntLonMinute.Text = "\'";
            // 
            // txtBox_initialPntLonMinute
            // 
            this.txtBox_initialPntLonMinute.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLonMinute.Location = new System.Drawing.Point(102, 67);
            this.txtBox_initialPntLonMinute.Name = "txtBox_initialPntLonMinute";
            this.txtBox_initialPntLonMinute.ReadOnly = true;
            this.txtBox_initialPntLonMinute.Size = new System.Drawing.Size(30, 20);
            this.txtBox_initialPntLonMinute.TabIndex = 32;
            // 
            // lbl_initialPntLonDegree
            // 
            this.lbl_initialPntLonDegree.AutoSize = true;
            this.lbl_initialPntLonDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLonDegree.Location = new System.Drawing.Point(86, 67);
            this.lbl_initialPntLonDegree.Name = "lbl_initialPntLonDegree";
            this.lbl_initialPntLonDegree.Size = new System.Drawing.Size(11, 13);
            this.lbl_initialPntLonDegree.TabIndex = 31;
            this.lbl_initialPntLonDegree.Text = "°";
            // 
            // txtBox_initialPntLonDegree
            // 
            this.txtBox_initialPntLonDegree.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLonDegree.Location = new System.Drawing.Point(56, 67);
            this.txtBox_initialPntLonDegree.Name = "txtBox_initialPntLonDegree";
            this.txtBox_initialPntLonDegree.ReadOnly = true;
            this.txtBox_initialPntLonDegree.Size = new System.Drawing.Size(30, 20);
            this.txtBox_initialPntLonDegree.TabIndex = 30;
            // 
            // lbl_initialPntLatSecond
            // 
            this.lbl_initialPntLatSecond.AutoSize = true;
            this.lbl_initialPntLatSecond.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLatSecond.Location = new System.Drawing.Point(209, 27);
            this.lbl_initialPntLatSecond.Name = "lbl_initialPntLatSecond";
            this.lbl_initialPntLatSecond.Size = new System.Drawing.Size(12, 13);
            this.lbl_initialPntLatSecond.TabIndex = 29;
            this.lbl_initialPntLatSecond.Text = "\"";
            // 
            // lbl_initialPntLatMinute
            // 
            this.lbl_initialPntLatMinute.AutoSize = true;
            this.lbl_initialPntLatMinute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLatMinute.Location = new System.Drawing.Point(132, 27);
            this.lbl_initialPntLatMinute.Name = "lbl_initialPntLatMinute";
            this.lbl_initialPntLatMinute.Size = new System.Drawing.Size(9, 13);
            this.lbl_initialPntLatMinute.TabIndex = 28;
            this.lbl_initialPntLatMinute.Text = "\'";
            // 
            // txtBox_initialPntLatSecond
            // 
            this.txtBox_initialPntLatSecond.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLatSecond.Location = new System.Drawing.Point(148, 28);
            this.txtBox_initialPntLatSecond.Name = "txtBox_initialPntLatSecond";
            this.txtBox_initialPntLatSecond.ReadOnly = true;
            this.txtBox_initialPntLatSecond.Size = new System.Drawing.Size(61, 20);
            this.txtBox_initialPntLatSecond.TabIndex = 27;
            // 
            // txtBox_initialPntLatMinute
            // 
            this.txtBox_initialPntLatMinute.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLatMinute.Location = new System.Drawing.Point(102, 28);
            this.txtBox_initialPntLatMinute.Name = "txtBox_initialPntLatMinute";
            this.txtBox_initialPntLatMinute.ReadOnly = true;
            this.txtBox_initialPntLatMinute.Size = new System.Drawing.Size(30, 20);
            this.txtBox_initialPntLatMinute.TabIndex = 26;
            // 
            // lbl_initialPntLatDegree
            // 
            this.lbl_initialPntLatDegree.AutoSize = true;
            this.lbl_initialPntLatDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLatDegree.Location = new System.Drawing.Point(86, 28);
            this.lbl_initialPntLatDegree.Name = "lbl_initialPntLatDegree";
            this.lbl_initialPntLatDegree.Size = new System.Drawing.Size(11, 13);
            this.lbl_initialPntLatDegree.TabIndex = 25;
            this.lbl_initialPntLatDegree.Text = "°";
            // 
            // txtBox_initialPntLatDegree
            // 
            this.txtBox_initialPntLatDegree.BackColor = System.Drawing.Color.White;
            this.txtBox_initialPntLatDegree.Location = new System.Drawing.Point(56, 28);
            this.txtBox_initialPntLatDegree.Name = "txtBox_initialPntLatDegree";
            this.txtBox_initialPntLatDegree.ReadOnly = true;
            this.txtBox_initialPntLatDegree.Size = new System.Drawing.Size(30, 20);
            this.txtBox_initialPntLatDegree.TabIndex = 24;
            // 
            // lbl_initialPntLongitude
            // 
            this.lbl_initialPntLongitude.AutoSize = true;
            this.lbl_initialPntLongitude.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLongitude.Location = new System.Drawing.Point(11, 70);
            this.lbl_initialPntLongitude.Name = "lbl_initialPntLongitude";
            this.lbl_initialPntLongitude.Size = new System.Drawing.Size(28, 13);
            this.lbl_initialPntLongitude.TabIndex = 22;
            this.lbl_initialPntLongitude.Text = "Lon:";
            // 
            // lbl_initialPntLatitude
            // 
            this.lbl_initialPntLatitude.AutoSize = true;
            this.lbl_initialPntLatitude.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_initialPntLatitude.Location = new System.Drawing.Point(11, 31);
            this.lbl_initialPntLatitude.Name = "lbl_initialPntLatitude";
            this.lbl_initialPntLatitude.Size = new System.Drawing.Size(25, 13);
            this.lbl_initialPntLatitude.TabIndex = 21;
            this.lbl_initialPntLatitude.Text = "Lat:";
            // 
            // grpBox_eliminationCriteria1
            // 
            this.grpBox_eliminationCriteria1.Controls.Add(this.txtBox_maxConvergenceAngle);
            this.grpBox_eliminationCriteria1.Controls.Add(this.txtBox_maxDivergenceAngle);
            this.grpBox_eliminationCriteria1.Controls.Add(this.label2);
            this.grpBox_eliminationCriteria1.Controls.Add(this.lbl_maxDivergenceAngle);
            this.grpBox_eliminationCriteria1.Location = new System.Drawing.Point(291, 3);
            this.grpBox_eliminationCriteria1.Name = "grpBox_eliminationCriteria1";
            this.grpBox_eliminationCriteria1.Size = new System.Drawing.Size(266, 131);
            this.grpBox_eliminationCriteria1.TabIndex = 66;
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
            // grpBox_eliminationCriteria2
            // 
            this.grpBox_eliminationCriteria2.Controls.Add(this.chkBox_isFinalStep);
            this.grpBox_eliminationCriteria2.Location = new System.Drawing.Point(291, 140);
            this.grpBox_eliminationCriteria2.Name = "grpBox_eliminationCriteria2";
            this.grpBox_eliminationCriteria2.Size = new System.Drawing.Size(266, 61);
            this.grpBox_eliminationCriteria2.TabIndex = 67;
            this.grpBox_eliminationCriteria2.TabStop = false;
            this.grpBox_eliminationCriteria2.Text = "Elimination Criteria 2";
            // 
            // chkBox_isFinalStep
            // 
            this.chkBox_isFinalStep.AutoSize = true;
            this.chkBox_isFinalStep.Location = new System.Drawing.Point(6, 29);
            this.chkBox_isFinalStep.Name = "chkBox_isFinalStep";
            this.chkBox_isFinalStep.Size = new System.Drawing.Size(79, 17);
            this.chkBox_isFinalStep.TabIndex = 1;
            this.chkBox_isFinalStep.Text = "Is final step";
            this.chkBox_isFinalStep.UseVisualStyleBackColor = true;
            this.chkBox_isFinalStep.CheckedChanged += new System.EventHandler(this.chkBox_isFinalStep_CheckedChanged);
            // 
            // NS_Page1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBox_eliminationCriteria2);
            this.Controls.Add(this.grpBox_eliminationCriteria1);
            this.Controls.Add(this.grpBox_initialPnt);
            this.Name = "NS_Page1";
            this.Size = new System.Drawing.Size(560, 400);
            this.VisibleChanged += new System.EventHandler(this.Page1_VisibleChanged);
            this.grpBox_initialPnt.ResumeLayout(false);
            this.grpBox_initialPnt.PerformLayout();
            this.grpBox_eliminationCriteria1.ResumeLayout(false);
            this.grpBox_eliminationCriteria1.PerformLayout();
            this.grpBox_eliminationCriteria2.ResumeLayout(false);
            this.grpBox_eliminationCriteria2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox grpBox_initialPnt;
        internal System.Windows.Forms.Button btn_pickInitialPnt;
        internal System.Windows.Forms.ComboBox cmbBox_initialPntLonSide;
        internal System.Windows.Forms.ComboBox cmbBox_initialPntLatSide;
        internal System.Windows.Forms.Label lbl_initialPntLonSecond;
        internal System.Windows.Forms.TextBox txtBox_initialPntLonSecond;
        internal System.Windows.Forms.Label lbl_initialPntLonMinute;
        internal System.Windows.Forms.TextBox txtBox_initialPntLonMinute;
        internal System.Windows.Forms.Label lbl_initialPntLonDegree;
        internal System.Windows.Forms.TextBox txtBox_initialPntLonDegree;
        internal System.Windows.Forms.Label lbl_initialPntLatSecond;
        internal System.Windows.Forms.Label lbl_initialPntLatMinute;
        internal System.Windows.Forms.TextBox txtBox_initialPntLatSecond;
        internal System.Windows.Forms.TextBox txtBox_initialPntLatMinute;
        internal System.Windows.Forms.Label lbl_initialPntLatDegree;
        internal System.Windows.Forms.TextBox txtBox_initialPntLatDegree;
        internal System.Windows.Forms.Label lbl_initialPntLongitude;
        internal System.Windows.Forms.Label lbl_initialPntLatitude;
        private System.Windows.Forms.TextBox txtBox_initialDirection;
        private System.Windows.Forms.Label lbl_initialDirection;
        private System.Windows.Forms.GroupBox grpBox_eliminationCriteria1;
        private System.Windows.Forms.TextBox txtBox_maxConvergenceAngle;
        private System.Windows.Forms.TextBox txtBox_maxDivergenceAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_maxDivergenceAngle;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpBox_eliminationCriteria2;
        private System.Windows.Forms.CheckBox chkBox_isFinalStep;
    }
}
