namespace MapEnv
{
    partial class StartupForm
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
			System.Windows.Forms.PictureBox pictureBox1;
			System.Windows.Forms.PictureBox pictureBox2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.ui_newProRB = new System.Windows.Forms.RadioButton();
			this.ui_existingProRB = new System.Windows.Forms.RadioButton();
			this.ui_okButton = new System.Windows.Forms.Button();
			this.ui_recentFilesLB = new System.Windows.Forms.ListBox();
			this.ui_showOnStartupChB = new System.Windows.Forms.CheckBox();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			pictureBox2 = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.Image = global::MapEnv.Properties.Resources.new_pro_48;
			pictureBox1.Location = new System.Drawing.Point(29, 81);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(48, 48);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			pictureBox2.Image = global::MapEnv.Properties.Resources.open_pro_48;
			pictureBox2.Location = new System.Drawing.Point(254, 81);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new System.Drawing.Size(48, 48);
			pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			pictureBox2.TabIndex = 2;
			pictureBox2.TabStop = false;
			// 
			// label1
			// 
			label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			label1.BackColor = System.Drawing.SystemColors.Window;
			label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
			label1.Location = new System.Drawing.Point(-4, -6);
			label1.Name = "label1";
			label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			label1.Size = new System.Drawing.Size(479, 65);
			label1.TabIndex = 0;
			label1.Text = "IAIM Environment";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 149);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(86, 13);
			label2.TabIndex = 3;
			label2.Text = "Recent Projects:";
			// 
			// ui_newProRB
			// 
			this.ui_newProRB.AutoSize = true;
			this.ui_newProRB.Checked = true;
			this.ui_newProRB.Location = new System.Drawing.Point(83, 94);
			this.ui_newProRB.Name = "ui_newProRB";
			this.ui_newProRB.Size = new System.Drawing.Size(121, 17);
			this.ui_newProRB.TabIndex = 1;
			this.ui_newProRB.TabStop = true;
			this.ui_newProRB.Text = "A new empty project";
			this.ui_newProRB.UseVisualStyleBackColor = true;
			this.ui_newProRB.CheckedChanged += new System.EventHandler(this.ProRB_CheckedChanged);
			// 
			// ui_existingProRB
			// 
			this.ui_existingProRB.AutoSize = true;
			this.ui_existingProRB.Location = new System.Drawing.Point(308, 94);
			this.ui_existingProRB.Name = "ui_existingProRB";
			this.ui_existingProRB.Size = new System.Drawing.Size(111, 17);
			this.ui_existingProRB.TabIndex = 2;
			this.ui_existingProRB.Text = "An existing project";
			this.ui_existingProRB.UseVisualStyleBackColor = true;
			this.ui_existingProRB.CheckedChanged += new System.EventHandler(this.ProRB_CheckedChanged);
			// 
			// ui_okButton
			// 
			this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_okButton.Location = new System.Drawing.Point(382, 305);
			this.ui_okButton.Name = "ui_okButton";
			this.ui_okButton.Size = new System.Drawing.Size(75, 23);
			this.ui_okButton.TabIndex = 6;
			this.ui_okButton.Text = "OK";
			this.ui_okButton.UseVisualStyleBackColor = true;
			this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
			// 
			// ui_recentFilesLB
			// 
			this.ui_recentFilesLB.ForeColor = System.Drawing.Color.Gray;
			this.ui_recentFilesLB.FormattingEnabled = true;
			this.ui_recentFilesLB.Location = new System.Drawing.Point(12, 165);
			this.ui_recentFilesLB.Name = "ui_recentFilesLB";
			this.ui_recentFilesLB.Size = new System.Drawing.Size(445, 121);
			this.ui_recentFilesLB.TabIndex = 4;
			this.ui_recentFilesLB.SelectedIndexChanged += new System.EventHandler(this.RecentFilesLB_SelectedIndexChanged);
			this.ui_recentFilesLB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RecentFilesLB_MouseDoubleClick);
			// 
			// ui_showOnStartupChB
			// 
			this.ui_showOnStartupChB.AutoSize = true;
			this.ui_showOnStartupChB.Location = new System.Drawing.Point(12, 305);
			this.ui_showOnStartupChB.Name = "ui_showOnStartupChB";
			this.ui_showOnStartupChB.Size = new System.Drawing.Size(103, 17);
			this.ui_showOnStartupChB.TabIndex = 5;
			this.ui_showOnStartupChB.Text = "Show on startup";
			this.ui_showOnStartupChB.UseVisualStyleBackColor = true;
			// 
			// StartupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(469, 340);
			this.Controls.Add(label2);
			this.Controls.Add(this.ui_showOnStartupChB);
			this.Controls.Add(this.ui_recentFilesLB);
			this.Controls.Add(this.ui_okButton);
			this.Controls.Add(label1);
			this.Controls.Add(this.ui_existingProRB);
			this.Controls.Add(pictureBox2);
			this.Controls.Add(pictureBox1);
			this.Controls.Add(this.ui_newProRB);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StartupForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "IAIM Environment - Start using..";
			this.Load += new System.EventHandler(this.StartupForm_Load);
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton ui_newProRB;
        private System.Windows.Forms.RadioButton ui_existingProRB;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.ListBox ui_recentFilesLB;
        private System.Windows.Forms.CheckBox ui_showOnStartupChB;
    }
}