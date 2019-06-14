namespace SigmaChart.CmdsMenu
{
    partial class RadioCommunicationsChanelsListForm
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
            this.All_listBox = new System.Windows.Forms.ListBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SelectedListBox = new System.Windows.Forms.ListBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 19);
            this.label1.TabIndex = 14;
            this.label1.Text = "Selected Chanels";
            // 
            // All_listBox
            // 
            this.All_listBox.FormattingEnabled = true;
            this.All_listBox.HorizontalScrollbar = true;
            this.All_listBox.ItemHeight = 19;
            this.All_listBox.Location = new System.Drawing.Point(470, 50);
            this.All_listBox.Name = "All_listBox";
            this.All_listBox.Size = new System.Drawing.Size(343, 422);
            this.All_listBox.TabIndex = 15;
            this.All_listBox.SelectedIndexChanged += new System.EventHandler(this.All_listBox_SelectedIndexChanged);
            this.All_listBox.DoubleClick += new System.EventHandler(this.All_listBox_DoubleClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Image = global::SigmaChart.Properties.Resources.GenericBlueRightArrowShortTail32;
            this.buttonRemove.Location = new System.Drawing.Point(411, 259);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(53, 37);
            this.buttonRemove.TabIndex = 17;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(585, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 19);
            this.label2.TabIndex = 18;
            this.label2.Text = "All Chanels";
            // 
            // SelectedListBox
            // 
            this.SelectedListBox.FormattingEnabled = true;
            this.SelectedListBox.HorizontalScrollbar = true;
            this.SelectedListBox.ItemHeight = 19;
            this.SelectedListBox.Location = new System.Drawing.Point(63, 50);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(343, 422);
            this.SelectedListBox.TabIndex = 19;
            this.SelectedListBox.DoubleClick += new System.EventHandler(this.SelectedListBox_DoubleClick);
            // 
            // buttonSelect
            // 
            this.buttonSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelect.Image = global::SigmaChart.Properties.Resources.GenericBlueLeftArrowShortTail32;
            this.buttonSelect.Location = new System.Drawing.Point(411, 216);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(53, 37);
            this.buttonSelect.TabIndex = 16;
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Image = global::SigmaChart.Properties.Resources.GenericBlueMoveToBottomArrow32;
            this.button6.Location = new System.Drawing.Point(4, 97);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(53, 37);
            this.button6.TabIndex = 13;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Image = global::SigmaChart.Properties.Resources.GenericBlueMoveToTopArrow32;
            this.button5.Location = new System.Drawing.Point(4, 50);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(53, 40);
            this.button5.TabIndex = 12;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Image = global::SigmaChart.Properties.Resources.GenericCheckMarkGreen32;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(714, 485);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 32);
            this.button2.TabIndex = 7;
            this.button2.Text = "OK";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RadioCommunicationsChanelsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 528);
            this.Controls.Add(this.SelectedListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.All_listBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RadioCommunicationsChanelsListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Channels list";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox All_listBox;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox SelectedListBox;
    }
}