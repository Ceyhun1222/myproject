namespace Aran.Aim.InputFormLib
{
    partial class NonExistingReferenceForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ui_propInfoLabel = new System.Windows.Forms.Label();
            this.ui_cleanButton = new System.Windows.Forms.Button();
            this.ui_saveLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(389, 437);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 25);
            this.ui_closeButton.TabIndex = 0;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeColumns = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ui_dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.ColumnHeadersVisible = false;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName,
            this.ui_colButton});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ui_dgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.ui_dgv.Location = new System.Drawing.Point(2, 2);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.RowTemplate.Height = 30;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_dgv.Size = new System.Drawing.Size(463, 333);
            this.ui_dgv.TabIndex = 1;
            this.ui_dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            // 
            // ui_colName
            // 
            this.ui_colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colName.HeaderText = "Name";
            this.ui_colName.Name = "ui_colName";
            this.ui_colName.ReadOnly = true;
            // 
            // ui_colButton
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            this.ui_colButton.DefaultCellStyle = dataGridViewCellStyle1;
            this.ui_colButton.HeaderText = "Open";
            this.ui_colButton.Name = "ui_colButton";
            this.ui_colButton.ReadOnly = true;
            this.ui_colButton.Text = "Open";
            this.ui_colButton.UseColumnTextForButtonValue = true;
            // 
            // ui_propInfoLabel
            // 
            this.ui_propInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_propInfoLabel.BackColor = System.Drawing.SystemColors.Window;
            this.ui_propInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_propInfoLabel.Location = new System.Drawing.Point(2, 338);
            this.ui_propInfoLabel.Name = "ui_propInfoLabel";
            this.ui_propInfoLabel.Padding = new System.Windows.Forms.Padding(6);
            this.ui_propInfoLabel.Size = new System.Drawing.Size(463, 92);
            this.ui_propInfoLabel.TabIndex = 2;
            this.ui_propInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ui_cleanButton
            // 
            this.ui_cleanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_cleanButton.Location = new System.Drawing.Point(86, 437);
            this.ui_cleanButton.Name = "ui_cleanButton";
            this.ui_cleanButton.Size = new System.Drawing.Size(137, 25);
            this.ui_cleanButton.TabIndex = 3;
            this.ui_cleanButton.Text = "Clean and Continue";
            this.ui_cleanButton.UseVisualStyleBackColor = true;
            this.ui_cleanButton.Click += new System.EventHandler(this.CleanAndContinueButton_Click);
            // 
            // ui_saveLog
            // 
            this.ui_saveLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_saveLog.Location = new System.Drawing.Point(2, 437);
            this.ui_saveLog.Name = "ui_saveLog";
            this.ui_saveLog.Size = new System.Drawing.Size(78, 25);
            this.ui_saveLog.TabIndex = 4;
            this.ui_saveLog.Text = "Save Log";
            this.ui_saveLog.UseVisualStyleBackColor = true;
            this.ui_saveLog.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // NonExistingReferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 468);
            this.Controls.Add(this.ui_saveLog);
            this.Controls.Add(this.ui_cleanButton);
            this.Controls.Add(this.ui_propInfoLabel);
            this.Controls.Add(this.ui_dgv);
            this.Controls.Add(this.ui_closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NonExistingReferenceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Non Existing References";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.Label ui_propInfoLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
        private System.Windows.Forms.DataGridViewButtonColumn ui_colButton;
        private System.Windows.Forms.Button ui_cleanButton;
        private System.Windows.Forms.Button ui_saveLog;
    }
}