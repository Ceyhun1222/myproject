namespace Aran.PANDA.Vss
{
    partial class SecondPageControl
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
            if (disposing && (components != null)) {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.ui_OffsetAngleTB = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ui_distanceToIntersTB = new System.Windows.Forms.TextBox();
            this.ui_abeamDistFrom1400mTB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(89, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 13);
            label1.TabIndex = 0;
            label1.Text = "Offset Angle:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(241, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(14, 20);
            label2.TabIndex = 22;
            label2.Text = "°";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(61, 41);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(96, 13);
            label3.TabIndex = 23;
            label3.Text = "Distance to Inters.:";
            this.toolTip1.SetToolTip(label3, "Distance between THR point and intersect point");
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 67);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(145, 13);
            label4.TabIndex = 25;
            label4.Text = "Abeam distance from 1400M:";
            this.toolTip1.SetToolTip(label4, "Abeam distance from 1400 m Point");
            // 
            // ui_OffsetAngleTB
            // 
            this.ui_OffsetAngleTB.Location = new System.Drawing.Point(163, 15);
            this.ui_OffsetAngleTB.Name = "ui_OffsetAngleTB";
            this.ui_OffsetAngleTB.ReadOnly = true;
            this.ui_OffsetAngleTB.Size = new System.Drawing.Size(75, 20);
            this.ui_OffsetAngleTB.TabIndex = 21;
            // 
            // ui_distanceToIntersTB
            // 
            this.ui_distanceToIntersTB.Location = new System.Drawing.Point(163, 41);
            this.ui_distanceToIntersTB.Name = "ui_distanceToIntersTB";
            this.ui_distanceToIntersTB.ReadOnly = true;
            this.ui_distanceToIntersTB.Size = new System.Drawing.Size(75, 20);
            this.ui_distanceToIntersTB.TabIndex = 24;
            // 
            // ui_abeamDistFrom1400mTB
            // 
            this.ui_abeamDistFrom1400mTB.Location = new System.Drawing.Point(163, 64);
            this.ui_abeamDistFrom1400mTB.Name = "ui_abeamDistFrom1400mTB";
            this.ui_abeamDistFrom1400mTB.ReadOnly = true;
            this.ui_abeamDistFrom1400mTB.Size = new System.Drawing.Size(75, 20);
            this.ui_abeamDistFrom1400mTB.TabIndex = 26;
            // 
            // SecondPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_abeamDistFrom1400mTB);
            this.Controls.Add(label4);
            this.Controls.Add(this.ui_distanceToIntersTB);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_OffsetAngleTB);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Name = "SecondPageControl";
            this.Size = new System.Drawing.Size(460, 346);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_OffsetAngleTB;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox ui_distanceToIntersTB;
        private System.Windows.Forms.TextBox ui_abeamDistFrom1400mTB;


    }
}
