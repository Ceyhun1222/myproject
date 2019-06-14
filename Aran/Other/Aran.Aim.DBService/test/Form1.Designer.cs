namespace Test.Aran.Aim.DBService
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
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.ui_requestTB = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_responseTB = new System.Windows.Forms.TextBox();
            this.ui_sendButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_requestTB
            // 
            this.ui_requestTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_requestTB.Location = new System.Drawing.Point(0, 0);
            this.ui_requestTB.MaxLength = 100000000;
            this.ui_requestTB.Multiline = true;
            this.ui_requestTB.Name = "ui_requestTB";
            this.ui_requestTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ui_requestTB.Size = new System.Drawing.Size(545, 170);
            this.ui_requestTB.TabIndex = 0;
            this.ui_requestTB.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 43);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_requestTB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_responseTB);
            this.splitContainer1.Size = new System.Drawing.Size(545, 354);
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 1;
            // 
            // ui_responseTB
            // 
            this.ui_responseTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_responseTB.Location = new System.Drawing.Point(0, 0);
            this.ui_responseTB.MaxLength = 100000000;
            this.ui_responseTB.Multiline = true;
            this.ui_responseTB.Name = "ui_responseTB";
            this.ui_responseTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ui_responseTB.Size = new System.Drawing.Size(545, 180);
            this.ui_responseTB.TabIndex = 1;
            this.ui_responseTB.WordWrap = false;
            // 
            // ui_sendButton
            // 
            this.ui_sendButton.Location = new System.Drawing.Point(12, 12);
            this.ui_sendButton.Name = "ui_sendButton";
            this.ui_sendButton.Size = new System.Drawing.Size(75, 23);
            this.ui_sendButton.TabIndex = 2;
            this.ui_sendButton.Text = "Send";
            this.ui_sendButton.UseVisualStyleBackColor = true;
            this.ui_sendButton.Click += new System.EventHandler(this.ui_sendButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Get";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 409);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ui_sendButton);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ui_requestTB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox ui_responseTB;
        private System.Windows.Forms.Button ui_sendButton;
        private System.Windows.Forms.Button button1;
    }
}

