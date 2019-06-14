namespace MapEnv
{
    partial class NewProFileNamePage
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button button1;
            this.ui_fileNameTB = new System.Windows.Forms.TextBox();
            this.ui_errorLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(88, 13);
            label1.TabIndex = 0;
            label1.Text = "Project file name:";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(334, 25);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(24, 24);
            button1.TabIndex = 2;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // ui_fileNameTB
            // 
            this.ui_fileNameTB.Location = new System.Drawing.Point(15, 28);
            this.ui_fileNameTB.Name = "ui_fileNameTB";
            this.ui_fileNameTB.ReadOnly = true;
            this.ui_fileNameTB.Size = new System.Drawing.Size(317, 20);
            this.ui_fileNameTB.TabIndex = 1;
            // 
            // ui_errorLabel
            // 
            this.ui_errorLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_errorLabel.Location = new System.Drawing.Point(12, 64);
            this.ui_errorLabel.Name = "ui_errorLabel";
            this.ui_errorLabel.Size = new System.Drawing.Size(277, 42);
            this.ui_errorLabel.TabIndex = 5;
            this.ui_errorLabel.Text = "<Error Message>";
            this.ui_errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_errorLabel.Visible = false;
            // 
            // NewProFileNamePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_errorLabel);
            this.Controls.Add(button1);
            this.Controls.Add(this.ui_fileNameTB);
            this.Controls.Add(label1);
            this.Name = "NewProFileNamePage";
            this.Size = new System.Drawing.Size(370, 123);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_fileNameTB;
        private System.Windows.Forms.Label ui_errorLabel;

    }
}
