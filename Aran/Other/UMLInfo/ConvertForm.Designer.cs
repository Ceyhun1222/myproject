namespace ParseMDL
{
    partial class ConvertForm
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
            this.components = new System.ComponentModel.Container ();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle ();
            this.fillClassesButton = new System.Windows.Forms.Button ();
            this.classesDGV = new System.Windows.Forms.DataGridView ();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip ();
            this.infoStatusLabel = new System.Windows.Forms.ToolStripStatusLabel ();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel ();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip (this.components);
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.stereoTypeLabel = new System.Windows.Forms.TextBox ();
            this.generateCodeClassButton = new System.Windows.Forms.Button ();
            this.convertToAranClassesButton = new System.Windows.Forms.Button ();
            this.tabControl1 = new System.Windows.Forms.TabControl ();
            this.tabPage1 = new System.Windows.Forms.TabPage ();
            this.assocShowBtn = new System.Windows.Forms.Button ();
            this.propShowBtn = new System.Windows.Forms.Button ();
            this.comboBox1 = new System.Windows.Forms.ComboBox ();
            ((System.ComponentModel.ISupportInitialize) (this.classesDGV)).BeginInit ();
            this.statusStrip1.SuspendLayout ();
            this.contextMenuStrip1.SuspendLayout ();
            this.tabControl1.SuspendLayout ();
            this.tabPage1.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // fillClassesButton
            // 
            this.fillClassesButton.Location = new System.Drawing.Point (12, 12);
            this.fillClassesButton.Name = "fillClassesButton";
            this.fillClassesButton.Size = new System.Drawing.Size (75, 23);
            this.fillClassesButton.TabIndex = 0;
            this.fillClassesButton.Text = "Fill Classes";
            this.fillClassesButton.UseVisualStyleBackColor = true;
            this.fillClassesButton.Click += new System.EventHandler (this.fillClassesButton_Click);
            // 
            // classesDGV
            // 
            this.classesDGV.AllowUserToAddRows = false;
            this.classesDGV.AllowUserToOrderColumns = true;
            this.classesDGV.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.classesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.classesDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.classesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.classesDGV.Location = new System.Drawing.Point (6, 6);
            this.classesDGV.Name = "classesDGV";
            this.classesDGV.ReadOnly = true;
            this.classesDGV.Size = new System.Drawing.Size (630, 200);
            this.classesDGV.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.infoStatusLabel,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point (0, 434);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size (674, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoStatusLabel
            // 
            this.infoStatusLabel.AutoSize = false;
            this.infoStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides) ((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.infoStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.infoStatusLabel.Name = "infoStatusLabel";
            this.infoStatusLabel.Size = new System.Drawing.Size (200, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides) ((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size (459, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.goToToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size (111, 26);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.Size = new System.Drawing.Size (110, 22);
            this.goToToolStripMenuItem.Text = "GoTo";
            this.goToToolStripMenuItem.Click += new System.EventHandler (this.goToToolStripMenuItem_Click);
            // 
            // stereoTypeLabel
            // 
            this.stereoTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stereoTypeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stereoTypeLabel.Location = new System.Drawing.Point (418, 212);
            this.stereoTypeLabel.Multiline = true;
            this.stereoTypeLabel.Name = "stereoTypeLabel";
            this.stereoTypeLabel.ReadOnly = true;
            this.stereoTypeLabel.Size = new System.Drawing.Size (218, 135);
            this.stereoTypeLabel.TabIndex = 4;
            // 
            // generateCodeClassButton
            // 
            this.generateCodeClassButton.Location = new System.Drawing.Point (24, 293);
            this.generateCodeClassButton.Name = "generateCodeClassButton";
            this.generateCodeClassButton.Size = new System.Drawing.Size (141, 23);
            this.generateCodeClassButton.TabIndex = 5;
            this.generateCodeClassButton.Text = "Generate Code Class File";
            this.generateCodeClassButton.UseVisualStyleBackColor = true;
            this.generateCodeClassButton.Visible = false;
            // 
            // convertToAranClassesButton
            // 
            this.convertToAranClassesButton.Location = new System.Drawing.Point (171, 293);
            this.convertToAranClassesButton.Name = "convertToAranClassesButton";
            this.convertToAranClassesButton.Size = new System.Drawing.Size (144, 23);
            this.convertToAranClassesButton.TabIndex = 6;
            this.convertToAranClassesButton.Text = "Convert To Aran Classes";
            this.convertToAranClassesButton.UseVisualStyleBackColor = true;
            this.convertToAranClassesButton.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add (this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point (12, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size (650, 379);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add (this.stereoTypeLabel);
            this.tabPage1.Controls.Add (this.classesDGV);
            this.tabPage1.Controls.Add (this.convertToAranClassesButton);
            this.tabPage1.Controls.Add (this.generateCodeClassButton);
            this.tabPage1.Location = new System.Drawing.Point (4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding (3);
            this.tabPage1.Size = new System.Drawing.Size (642, 353);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "UML Classes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // assocShowBtn
            // 
            this.assocShowBtn.Location = new System.Drawing.Point (93, 12);
            this.assocShowBtn.Name = "assocShowBtn";
            this.assocShowBtn.Size = new System.Drawing.Size (105, 23);
            this.assocShowBtn.TabIndex = 11;
            this.assocShowBtn.Text = "Show Associations";
            this.assocShowBtn.UseVisualStyleBackColor = true;
            this.assocShowBtn.Click += new System.EventHandler (this.assocShowBtn_Click);
            // 
            // propShowBtn
            // 
            this.propShowBtn.Location = new System.Drawing.Point (204, 12);
            this.propShowBtn.Name = "propShowBtn";
            this.propShowBtn.Size = new System.Drawing.Size (92, 23);
            this.propShowBtn.TabIndex = 12;
            this.propShowBtn.Text = "Show Properties";
            this.propShowBtn.UseVisualStyleBackColor = true;
            this.propShowBtn.Click += new System.EventHandler (this.propShowBtn_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point (345, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size (121, 21);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler (this.comboBox1_SelectedIndexChanged);
            // 
            // ConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size (674, 456);
            this.Controls.Add (this.comboBox1);
            this.Controls.Add (this.propShowBtn);
            this.Controls.Add (this.tabControl1);
            this.Controls.Add (this.statusStrip1);
            this.Controls.Add (this.fillClassesButton);
            this.Controls.Add (this.assocShowBtn);
            this.Name = "ConvertForm";
            this.Text = "ConverForm";
            ((System.ComponentModel.ISupportInitialize) (this.classesDGV)).EndInit ();
            this.statusStrip1.ResumeLayout (false);
            this.statusStrip1.PerformLayout ();
            this.contextMenuStrip1.ResumeLayout (false);
            this.tabControl1.ResumeLayout (false);
            this.tabPage1.ResumeLayout (false);
            this.tabPage1.PerformLayout ();
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Button fillClassesButton;
        private System.Windows.Forms.DataGridView classesDGV;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.TextBox stereoTypeLabel;
        private System.Windows.Forms.Button generateCodeClassButton;
        private System.Windows.Forms.Button convertToAranClassesButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button assocShowBtn;
        private System.Windows.Forms.Button propShowBtn;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}