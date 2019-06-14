namespace Aran.Aim.InputFormLib
{
    partial class ParserErrorReportForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.Button button3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParserErrorReportForm));
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colFeatType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colIdentifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colPropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colErrorMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_contextMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showXmlPartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.ui_contextMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            button1.Location = new System.Drawing.Point(7, 566);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(96, 23);
            button1.TabIndex = 1;
            button1.Text = "Save Report";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.Save_Click);
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button2.Location = new System.Drawing.Point(877, 566);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(96, 23);
            button2.TabIndex = 2;
            button2.Text = "Stop Import";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.StopImport_Click);
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ui_dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colFeatType,
            this.ui_colIdentifier,
            this.ui_colPropertyName,
            this.ui_colErrorMessage,
            this.ui_colAction});
            this.ui_dgv.Location = new System.Drawing.Point(2, 2);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.Size = new System.Drawing.Size(978, 555);
            this.ui_dgv.TabIndex = 0;
            this.ui_dgv.VirtualMode = true;
            this.ui_dgv.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DGV_CellContextMenuStripNeeded);
            // 
            // ui_colFeatType
            // 
            this.ui_colFeatType.DataPropertyName = "FeatureType";
            this.ui_colFeatType.HeaderText = "FeatureType";
            this.ui_colFeatType.Name = "ui_colFeatType";
            this.ui_colFeatType.ReadOnly = true;
            this.ui_colFeatType.Width = 120;
            // 
            // ui_colIdentifier
            // 
            this.ui_colIdentifier.DataPropertyName = "Identifier";
            this.ui_colIdentifier.HeaderText = "Identifier";
            this.ui_colIdentifier.Name = "ui_colIdentifier";
            this.ui_colIdentifier.ReadOnly = true;
            this.ui_colIdentifier.Width = 230;
            // 
            // ui_colPropertyName
            // 
            this.ui_colPropertyName.DataPropertyName = "PropertyName";
            this.ui_colPropertyName.HeaderText = "Property Name";
            this.ui_colPropertyName.Name = "ui_colPropertyName";
            this.ui_colPropertyName.ReadOnly = true;
            this.ui_colPropertyName.Width = 360;
            // 
            // ui_colErrorMessage
            // 
            this.ui_colErrorMessage.DataPropertyName = "ErrorMessage";
            this.ui_colErrorMessage.HeaderText = "Message";
            this.ui_colErrorMessage.Name = "ui_colErrorMessage";
            this.ui_colErrorMessage.ReadOnly = true;
            this.ui_colErrorMessage.Width = 300;
            // 
            // ui_colAction
            // 
            this.ui_colAction.DataPropertyName = "Action";
            this.ui_colAction.HeaderText = "Action";
            this.ui_colAction.Name = "ui_colAction";
            this.ui_colAction.ReadOnly = true;
            this.ui_colAction.Width = 180;
            // 
            // ui_contextMS
            // 
            this.ui_contextMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.showXmlPartToolStripMenuItem});
            this.ui_contextMS.Name = "ui_contextMS";
            this.ui_contextMS.Size = new System.Drawing.Size(152, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyTSMI_Clic);
            // 
            // showXmlPartToolStripMenuItem
            // 
            this.showXmlPartToolStripMenuItem.Name = "showXmlPartToolStripMenuItem";
            this.showXmlPartToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.showXmlPartToolStripMenuItem.Text = "&Show Xml Part";
            this.showXmlPartToolStripMenuItem.Click += new System.EventHandler(this.ShowXmlPartTSMI_Click);
            // 
            // button3
            // 
            button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button3.Location = new System.Drawing.Point(775, 566);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(96, 23);
            button3.TabIndex = 3;
            button3.Text = "Continue";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.Continue_Click);
            // 
            // ParserErrorReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 598);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.Controls.Add(this.ui_dgv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParserErrorReportForm";
            this.Text = "Parser Error Report";
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ui_contextMS.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.ContextMenuStrip ui_contextMS;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showXmlPartToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colFeatType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colIdentifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colPropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colErrorMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colAction;
    }
}