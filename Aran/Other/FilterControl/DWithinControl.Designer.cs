namespace Aran.Controls
{
    partial class DWithinControl
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.ui_distUomCB = new System.Windows.Forms.ComboBox();
            this.ui_distNud = new System.Windows.Forms.NumericUpDown();
            this.ui_coordTB = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_distNud)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(2, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 13);
            label2.TabIndex = 2;
            label2.Text = "Distance:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(2, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(86, 13);
            label1.TabIndex = 0;
            label1.Text = "Coordinate (DD):";
            // 
            // ui_distUomCB
            // 
            this.ui_distUomCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_distUomCB.FormattingEnabled = true;
            this.ui_distUomCB.Location = new System.Drawing.Point(258, 29);
            this.ui_distUomCB.Name = "ui_distUomCB";
            this.ui_distUomCB.Size = new System.Drawing.Size(70, 21);
            this.ui_distUomCB.TabIndex = 4;
            // 
            // ui_distNud
            // 
            this.ui_distNud.DecimalPlaces = 4;
            this.ui_distNud.Location = new System.Drawing.Point(96, 29);
            this.ui_distNud.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.ui_distNud.Name = "ui_distNud";
            this.ui_distNud.Size = new System.Drawing.Size(157, 20);
            this.ui_distNud.TabIndex = 3;
            // 
            // ui_coordTB
            // 
            this.ui_coordTB.Location = new System.Drawing.Point(95, 2);
            this.ui_coordTB.Name = "ui_coordTB";
            this.ui_coordTB.Size = new System.Drawing.Size(233, 20);
            this.ui_coordTB.TabIndex = 1;
            // 
            // DWithinControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_distUomCB);
            this.Controls.Add(this.ui_distNud);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_coordTB);
            this.Name = "DWithinControl";
            this.Size = new System.Drawing.Size(331, 52);
            ((System.ComponentModel.ISupportInitialize)(this.ui_distNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_distUomCB;
        private System.Windows.Forms.NumericUpDown ui_distNud;
        private System.Windows.Forms.TextBox ui_coordTB;
    }
}
