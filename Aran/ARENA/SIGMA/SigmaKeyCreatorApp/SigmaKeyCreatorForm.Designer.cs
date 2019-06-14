namespace Encryptor
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.EncryptedText_Box = new System.Windows.Forms.TextBox();
            this.CheckText_Box = new System.Windows.Forms.TextBox();
            this.RSAKeyText_Box = new System.Windows.Forms.TextBox();
            this.SourceText_Box = new System.Windows.Forms.TextBox();
            this.CompressedRSAKeyText_Box = new System.Windows.Forms.TextBox();
            this.DeCompressedRSAKeyText_Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(529, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Encrypt";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EncryptedText_Box
            // 
            this.EncryptedText_Box.Location = new System.Drawing.Point(183, 54);
            this.EncryptedText_Box.Name = "EncryptedText_Box";
            this.EncryptedText_Box.ReadOnly = true;
            this.EncryptedText_Box.Size = new System.Drawing.Size(423, 20);
            this.EncryptedText_Box.TabIndex = 2;
            // 
            // CheckText_Box
            // 
            this.CheckText_Box.Location = new System.Drawing.Point(183, 226);
            this.CheckText_Box.Name = "CheckText_Box";
            this.CheckText_Box.ReadOnly = true;
            this.CheckText_Box.Size = new System.Drawing.Size(421, 20);
            this.CheckText_Box.TabIndex = 3;
            // 
            // RSAKeyText_Box
            // 
            this.RSAKeyText_Box.Location = new System.Drawing.Point(183, 95);
            this.RSAKeyText_Box.Name = "RSAKeyText_Box";
            this.RSAKeyText_Box.ReadOnly = true;
            this.RSAKeyText_Box.Size = new System.Drawing.Size(423, 20);
            this.RSAKeyText_Box.TabIndex = 6;
            // 
            // SourceText_Box
            // 
            this.SourceText_Box.Location = new System.Drawing.Point(183, 12);
            this.SourceText_Box.Name = "SourceText_Box";
            this.SourceText_Box.Size = new System.Drawing.Size(212, 20);
            this.SourceText_Box.TabIndex = 7;
            // 
            // CompressedRSAKeyText_Box
            // 
            this.CompressedRSAKeyText_Box.Location = new System.Drawing.Point(183, 138);
            this.CompressedRSAKeyText_Box.Name = "CompressedRSAKeyText_Box";
            this.CompressedRSAKeyText_Box.ReadOnly = true;
            this.CompressedRSAKeyText_Box.Size = new System.Drawing.Size(423, 20);
            this.CompressedRSAKeyText_Box.TabIndex = 10;
            // 
            // DeCompressedRSAKeyText_Box
            // 
            this.DeCompressedRSAKeyText_Box.Location = new System.Drawing.Point(183, 181);
            this.DeCompressedRSAKeyText_Box.Name = "DeCompressedRSAKeyText_Box";
            this.DeCompressedRSAKeyText_Box.ReadOnly = true;
            this.DeCompressedRSAKeyText_Box.Size = new System.Drawing.Size(421, 20);
            this.DeCompressedRSAKeyText_Box.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Sourcet text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "RSA key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Encrypt text";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Compressed RSA key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "DEcompressed RSA key";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Sourcet text";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(401, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(184, 20);
            this.dateTimePicker1.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(591, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(31, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(473, 310);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(131, 23);
            this.button3.TabIndex = 20;
            this.button3.Text = "Test OpenFile";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(183, 339);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(423, 20);
            this.textBox1.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 380);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeCompressedRSAKeyText_Box);
            this.Controls.Add(this.CompressedRSAKeyText_Box);
            this.Controls.Add(this.SourceText_Box);
            this.Controls.Add(this.RSAKeyText_Box);
            this.Controls.Add(this.CheckText_Box);
            this.Controls.Add(this.EncryptedText_Box);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox EncryptedText_Box;
        private System.Windows.Forms.TextBox CheckText_Box;
        private System.Windows.Forms.TextBox RSAKeyText_Box;
        private System.Windows.Forms.TextBox SourceText_Box;
        private System.Windows.Forms.TextBox CompressedRSAKeyText_Box;
        private System.Windows.Forms.TextBox DeCompressedRSAKeyText_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
    }
}

