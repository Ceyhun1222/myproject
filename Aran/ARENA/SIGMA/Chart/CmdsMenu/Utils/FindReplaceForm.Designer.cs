namespace SigmaChart
{
    partial class FindReplaceForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.oldTextBox = new System.Windows.Forms.TextBox();
            this.newTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CaseCheckBox = new System.Windows.Forms.CheckBox();
            this.WordCheckBox = new System.Windows.Forms.CheckBox();
            this.GraphicsElementsCheckBox = new System.Windows.Forms.CheckBox();
            this.AnnoCheckBox = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // oldTextBox
            // 
            this.oldTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oldTextBox.Location = new System.Drawing.Point(113, 13);
            this.oldTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.oldTextBox.Name = "oldTextBox";
            this.oldTextBox.Size = new System.Drawing.Size(314, 25);
            this.oldTextBox.TabIndex = 1;
            // 
            // newTextBox
            // 
            this.newTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newTextBox.Location = new System.Drawing.Point(113, 46);
            this.newTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.newTextBox.Name = "newTextBox";
            this.newTextBox.Size = new System.Drawing.Size(314, 25);
            this.newTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Replace with:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(281, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 27);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(357, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 27);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CaseCheckBox
            // 
            this.CaseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CaseCheckBox.AutoSize = true;
            this.CaseCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CaseCheckBox.Location = new System.Drawing.Point(142, 83);
            this.CaseCheckBox.Name = "CaseCheckBox";
            this.CaseCheckBox.Size = new System.Drawing.Size(93, 21);
            this.CaseCheckBox.TabIndex = 9;
            this.CaseCheckBox.Text = "Match case";
            this.CaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // WordCheckBox
            // 
            this.WordCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WordCheckBox.AutoSize = true;
            this.WordCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WordCheckBox.Location = new System.Drawing.Point(292, 83);
            this.WordCheckBox.Name = "WordCheckBox";
            this.WordCheckBox.Size = new System.Drawing.Size(135, 21);
            this.WordCheckBox.TabIndex = 10;
            this.WordCheckBox.Text = "Match whole word";
            this.WordCheckBox.UseVisualStyleBackColor = true;
            // 
            // GraphicsElementsCheckBox
            // 
            this.GraphicsElementsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphicsElementsCheckBox.AutoSize = true;
            this.GraphicsElementsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GraphicsElementsCheckBox.Location = new System.Drawing.Point(256, 110);
            this.GraphicsElementsCheckBox.Name = "GraphicsElementsCheckBox";
            this.GraphicsElementsCheckBox.Size = new System.Drawing.Size(171, 21);
            this.GraphicsElementsCheckBox.TabIndex = 12;
            this.GraphicsElementsCheckBox.Text = "Ignore graphics elements";
            this.GraphicsElementsCheckBox.UseVisualStyleBackColor = true;
            // 
            // AnnoCheckBox
            // 
            this.AnnoCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AnnoCheckBox.AutoSize = true;
            this.AnnoCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AnnoCheckBox.Location = new System.Drawing.Point(103, 110);
            this.AnnoCheckBox.Name = "AnnoCheckBox";
            this.AnnoCheckBox.Size = new System.Drawing.Size(132, 21);
            this.AnnoCheckBox.TabIndex = 11;
            this.AnnoCheckBox.Text = "Ignore annotations";
            this.AnnoCheckBox.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 151);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(313, 25);
            this.comboBox1.TabIndex = 13;
            // 
            // FindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 226);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GraphicsElementsCheckBox);
            this.Controls.Add(this.AnnoCheckBox);
            this.Controls.Add(this.WordCheckBox);
            this.Controls.Add(this.CaseCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.newTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oldTextBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FindReplaceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find and Replace";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox oldTextBox;
        public System.Windows.Forms.TextBox newTextBox;
        public System.Windows.Forms.CheckBox CaseCheckBox;
        public System.Windows.Forms.CheckBox WordCheckBox;
        public System.Windows.Forms.CheckBox GraphicsElementsCheckBox;
        public System.Windows.Forms.CheckBox AnnoCheckBox;
        public System.Windows.Forms.ComboBox comboBox1;
    }
}