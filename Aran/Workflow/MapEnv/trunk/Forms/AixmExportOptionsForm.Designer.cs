namespace MapEnv
{
    partial class AixmExportOptionsForm
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            this.ui_srsNameCB = new System.Windows.Forms.ComboBox();
            this.ui_write3DisExistsChB = new System.Windows.Forms.CheckBox();
            this.ui_writeExtensionChB = new System.Windows.Forms.CheckBox();
            this.ui_fileNameTB = new System.Windows.Forms.TextBox();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_selFileNameButton = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(22, 48);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 17);
            label3.TabIndex = 16;
            label3.Text = "srsName:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 18);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 17);
            label1.TabIndex = 11;
            label1.Text = "File Name:";
            // 
            // ui_srsNameCB
            // 
            this.ui_srsNameCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_srsNameCB.FormattingEnabled = true;
            this.ui_srsNameCB.Location = new System.Drawing.Point(98, 43);
            this.ui_srsNameCB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_srsNameCB.Name = "ui_srsNameCB";
            this.ui_srsNameCB.Size = new System.Drawing.Size(232, 24);
            this.ui_srsNameCB.TabIndex = 15;
            // 
            // ui_write3DisExistsChB
            // 
            this.ui_write3DisExistsChB.AutoSize = true;
            this.ui_write3DisExistsChB.Checked = true;
            this.ui_write3DisExistsChB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_write3DisExistsChB.Location = new System.Drawing.Point(98, 104);
            this.ui_write3DisExistsChB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_write3DisExistsChB.Name = "ui_write3DisExistsChB";
            this.ui_write3DisExistsChB.Size = new System.Drawing.Size(206, 21);
            this.ui_write3DisExistsChB.TabIndex = 14;
            this.ui_write3DisExistsChB.Text = "Write 3D coordinate if exists";
            this.ui_write3DisExistsChB.UseVisualStyleBackColor = true;
            // 
            // ui_writeExtensionChB
            // 
            this.ui_writeExtensionChB.AutoSize = true;
            this.ui_writeExtensionChB.Checked = true;
            this.ui_writeExtensionChB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_writeExtensionChB.Location = new System.Drawing.Point(98, 75);
            this.ui_writeExtensionChB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_writeExtensionChB.Name = "ui_writeExtensionChB";
            this.ui_writeExtensionChB.Size = new System.Drawing.Size(140, 21);
            this.ui_writeExtensionChB.TabIndex = 13;
            this.ui_writeExtensionChB.Text = "Writer Extensions";
            this.ui_writeExtensionChB.UseVisualStyleBackColor = true;
            // 
            // ui_fileNameTB
            // 
            this.ui_fileNameTB.Location = new System.Drawing.Point(98, 13);
            this.ui_fileNameTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_fileNameTB.Name = "ui_fileNameTB";
            this.ui_fileNameTB.ReadOnly = true;
            this.ui_fileNameTB.Size = new System.Drawing.Size(355, 22);
            this.ui_fileNameTB.TabIndex = 12;
            this.ui_fileNameTB.TextChanged += new System.EventHandler(this.FileName_TextChanged);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(387, 151);
            this.ui_cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(100, 28);
            this.ui_cancelButton.TabIndex = 17;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(279, 151);
            this.ui_okButton.Margin = new System.Windows.Forms.Padding(4);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(100, 28);
            this.ui_okButton.TabIndex = 18;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(-4, 137);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(509, 1);
            this.panel1.TabIndex = 19;
            // 
            // ui_selFileNameButton
            // 
            this.ui_selFileNameButton.AutoSize = true;
            this.ui_selFileNameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_selFileNameButton.Location = new System.Drawing.Point(461, 9);
            this.ui_selFileNameButton.Margin = new System.Windows.Forms.Padding(4);
            this.ui_selFileNameButton.Name = "ui_selFileNameButton";
            this.ui_selFileNameButton.Size = new System.Drawing.Size(30, 27);
            this.ui_selFileNameButton.TabIndex = 20;
            this.ui_selFileNameButton.Text = "...";
            this.ui_selFileNameButton.UseVisualStyleBackColor = true;
            this.ui_selFileNameButton.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // AixmExportOptionsForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(500, 191);
            this.Controls.Add(this.ui_selFileNameButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_srsNameCB);
            this.Controls.Add(this.ui_write3DisExistsChB);
            this.Controls.Add(this.ui_writeExtensionChB);
            this.Controls.Add(this.ui_fileNameTB);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AixmExportOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to AIXM Message";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_srsNameCB;
        private System.Windows.Forms.CheckBox ui_write3DisExistsChB;
        private System.Windows.Forms.CheckBox ui_writeExtensionChB;
        private System.Windows.Forms.TextBox ui_fileNameTB;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ui_selFileNameButton;
    }
}