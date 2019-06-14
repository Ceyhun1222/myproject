namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class NS_Page3
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
            this.cmbBox_reachablePoints = new System.Windows.Forms.ComboBox();
            this.lbl_reachablePoints = new System.Windows.Forms.Label();
            this.lbl_distanceRange = new System.Windows.Forms.Label();
            this.nmrcUpDown_distanceRange = new System.Windows.Forms.NumericUpDown();
            this.lbl_rangeDist = new System.Windows.Forms.Label();
            this.txtBox_intermediateDirection = new System.Windows.Forms.TextBox();
            this.txtBox_initialDirection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_rangeAngle = new System.Windows.Forms.Label();
            this.lbl_polySize = new System.Windows.Forms.Label();
            this.txtBox_polySize = new System.Windows.Forms.TextBox();
            this.lbl_finalSegmentTime = new System.Windows.Forms.Label();
            this.nmrcUpDown_finalSegmentTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_distanceRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_finalSegmentTime)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBox_reachablePoints
            // 
            this.cmbBox_reachablePoints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBox_reachablePoints.FormattingEnabled = true;
            this.cmbBox_reachablePoints.Location = new System.Drawing.Point(102, 12);
            this.cmbBox_reachablePoints.Name = "cmbBox_reachablePoints";
            this.cmbBox_reachablePoints.Size = new System.Drawing.Size(121, 21);
            this.cmbBox_reachablePoints.TabIndex = 3;
            this.cmbBox_reachablePoints.SelectedIndexChanged += new System.EventHandler(this.cmbBox_reachablePoints_SelectedIndexChanged);
            // 
            // lbl_reachablePoints
            // 
            this.lbl_reachablePoints.AutoSize = true;
            this.lbl_reachablePoints.Location = new System.Drawing.Point(3, 15);
            this.lbl_reachablePoints.Name = "lbl_reachablePoints";
            this.lbl_reachablePoints.Size = new System.Drawing.Size(93, 13);
            this.lbl_reachablePoints.TabIndex = 2;
            this.lbl_reachablePoints.Text = "Reachable points:";
            // 
            // lbl_distanceRange
            // 
            this.lbl_distanceRange.AutoSize = true;
            this.lbl_distanceRange.Location = new System.Drawing.Point(3, 55);
            this.lbl_distanceRange.Name = "lbl_distanceRange";
            this.lbl_distanceRange.Size = new System.Drawing.Size(82, 13);
            this.lbl_distanceRange.TabIndex = 8;
            this.lbl_distanceRange.Text = "Distance range:";
            // 
            // nmrcUpDown_distanceRange
            // 
            this.nmrcUpDown_distanceRange.Location = new System.Drawing.Point(102, 53);
            this.nmrcUpDown_distanceRange.Name = "nmrcUpDown_distanceRange";
            this.nmrcUpDown_distanceRange.Size = new System.Drawing.Size(94, 20);
            this.nmrcUpDown_distanceRange.TabIndex = 7;
            this.nmrcUpDown_distanceRange.ValueChanged += new System.EventHandler(this.nmrcUpDown_distanceRange_ValueChanged);
            // 
            // lbl_rangeDist
            // 
            this.lbl_rangeDist.AutoSize = true;
            this.lbl_rangeDist.Location = new System.Drawing.Point(202, 55);
            this.lbl_rangeDist.Name = "lbl_rangeDist";
            this.lbl_rangeDist.Size = new System.Drawing.Size(35, 13);
            this.lbl_rangeDist.TabIndex = 6;
            this.lbl_rangeDist.Text = "label1";
            // 
            // txtBox_intermediateDirection
            // 
            this.txtBox_intermediateDirection.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_intermediateDirection.Location = new System.Drawing.Point(437, 41);
            this.txtBox_intermediateDirection.Name = "txtBox_intermediateDirection";
            this.txtBox_intermediateDirection.ReadOnly = true;
            this.txtBox_intermediateDirection.Size = new System.Drawing.Size(100, 20);
            this.txtBox_intermediateDirection.TabIndex = 14;
            // 
            // txtBox_initialDirection
            // 
            this.txtBox_initialDirection.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_initialDirection.Location = new System.Drawing.Point(437, 12);
            this.txtBox_initialDirection.Name = "txtBox_initialDirection";
            this.txtBox_initialDirection.ReadOnly = true;
            this.txtBox_initialDirection.Size = new System.Drawing.Size(100, 20);
            this.txtBox_initialDirection.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Intermediate direction:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(354, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Initial direction:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Angle range:";
            // 
            // lbl_rangeAngle
            // 
            this.lbl_rangeAngle.AutoSize = true;
            this.lbl_rangeAngle.Location = new System.Drawing.Point(99, 81);
            this.lbl_rangeAngle.Name = "lbl_rangeAngle";
            this.lbl_rangeAngle.Size = new System.Drawing.Size(35, 13);
            this.lbl_rangeAngle.TabIndex = 16;
            this.lbl_rangeAngle.Text = "label4";
            // 
            // lbl_polySize
            // 
            this.lbl_polySize.AutoSize = true;
            this.lbl_polySize.Location = new System.Drawing.Point(3, 192);
            this.lbl_polySize.Name = "lbl_polySize";
            this.lbl_polySize.Size = new System.Drawing.Size(69, 13);
            this.lbl_polySize.TabIndex = 17;
            this.lbl_polySize.Text = "Polygon size:";
            // 
            // txtBox_polySize
            // 
            this.txtBox_polySize.Location = new System.Drawing.Point(102, 189);
            this.txtBox_polySize.Name = "txtBox_polySize";
            this.txtBox_polySize.Size = new System.Drawing.Size(100, 20);
            this.txtBox_polySize.TabIndex = 18;
            this.txtBox_polySize.TextChanged += new System.EventHandler(this.txtBox_polySize_TextChanged);
            this.txtBox_polySize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_polySize_KeyPress);
            // 
            // lbl_finalSegmentTime
            // 
            this.lbl_finalSegmentTime.AutoSize = true;
            this.lbl_finalSegmentTime.Location = new System.Drawing.Point(3, 133);
            this.lbl_finalSegmentTime.Name = "lbl_finalSegmentTime";
            this.lbl_finalSegmentTime.Size = new System.Drawing.Size(97, 13);
            this.lbl_finalSegmentTime.TabIndex = 19;
            this.lbl_finalSegmentTime.Text = "Final segment time:";
            // 
            // nmrcUpDown_finalSegmentTime
            // 
            this.nmrcUpDown_finalSegmentTime.Location = new System.Drawing.Point(102, 131);
            this.nmrcUpDown_finalSegmentTime.Name = "nmrcUpDown_finalSegmentTime";
            this.nmrcUpDown_finalSegmentTime.Size = new System.Drawing.Size(120, 20);
            this.nmrcUpDown_finalSegmentTime.TabIndex = 20;
            this.nmrcUpDown_finalSegmentTime.ValueChanged += new System.EventHandler(this.nmrcUpDown_finalSegmentTime_ValueChanged);
            // 
            // NS_Page3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nmrcUpDown_finalSegmentTime);
            this.Controls.Add(this.lbl_finalSegmentTime);
            this.Controls.Add(this.txtBox_polySize);
            this.Controls.Add(this.lbl_polySize);
            this.Controls.Add(this.lbl_rangeAngle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBox_intermediateDirection);
            this.Controls.Add(this.txtBox_initialDirection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_distanceRange);
            this.Controls.Add(this.nmrcUpDown_distanceRange);
            this.Controls.Add(this.lbl_rangeDist);
            this.Controls.Add(this.cmbBox_reachablePoints);
            this.Controls.Add(this.lbl_reachablePoints);
            this.Name = "NS_Page3";
            this.Size = new System.Drawing.Size(560, 400);
            this.VisibleChanged += new System.EventHandler(this.NS_Page3_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_distanceRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDown_finalSegmentTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBox_reachablePoints;
        private System.Windows.Forms.Label lbl_reachablePoints;
        private System.Windows.Forms.Label lbl_distanceRange;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_distanceRange;
        private System.Windows.Forms.Label lbl_rangeDist;
        private System.Windows.Forms.TextBox txtBox_intermediateDirection;
        private System.Windows.Forms.TextBox txtBox_initialDirection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_rangeAngle;
        private System.Windows.Forms.Label lbl_polySize;
        private System.Windows.Forms.TextBox txtBox_polySize;
        private System.Windows.Forms.Label lbl_finalSegmentTime;
        private System.Windows.Forms.NumericUpDown nmrcUpDown_finalSegmentTime;
    }
}
