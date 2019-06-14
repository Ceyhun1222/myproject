namespace MapEnv.Controls
{
    partial class LeftWindowTitle
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
            this.ui_closePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ui_closePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_closePictureBox
            // 
            this.ui_closePictureBox.BackgroundImage = global::MapEnv.Properties.Resources.left_win_title_backg;
            this.ui_closePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ui_closePictureBox.Image = global::MapEnv.Properties.Resources.close_title_24;
            this.ui_closePictureBox.Location = new System.Drawing.Point(238, 0);
            this.ui_closePictureBox.Name = "ui_closePictureBox";
            this.ui_closePictureBox.Size = new System.Drawing.Size(22, 21);
            this.ui_closePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ui_closePictureBox.TabIndex = 0;
            this.ui_closePictureBox.TabStop = false;
            this.ui_closePictureBox.Click += new System.EventHandler(this.Close_Click);
            this.ui_closePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClosePictureBox_MouseDown);
            this.ui_closePictureBox.MouseEnter += new System.EventHandler(this.CosePictureBox_MouseEnter);
            this.ui_closePictureBox.MouseLeave += new System.EventHandler(this.ClosePictureBox_MouseLeave);
            this.ui_closePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ClosePictureBox_MouseUp);
            // 
            // LeftWindowTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MapEnv.Properties.Resources.left_win_title_backg;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ui_closePictureBox);
            this.Name = "LeftWindowTitle";
            this.Size = new System.Drawing.Size(260, 21);
            ((System.ComponentModel.ISupportInitialize)(this.ui_closePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ui_closePictureBox;


    }
}
