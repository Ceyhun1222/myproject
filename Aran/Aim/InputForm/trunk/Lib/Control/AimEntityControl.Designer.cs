namespace Aran.Aim.InputFormLib
{
    partial class AimEntityControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.ui_titlePanel = new System.Windows.Forms.Panel();
            this.ui_tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // ui_titlePanel
            // 
            this.ui_titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_titlePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_titlePanel.Location = new System.Drawing.Point(0, 0);
            this.ui_titlePanel.Name = "ui_titlePanel";
            this.ui_titlePanel.Size = new System.Drawing.Size(277, 33);
            this.ui_titlePanel.TabIndex = 3;
            this.ui_titlePanel.SizeChanged += new System.EventHandler(this.ui_titlePanel_SizeChanged);
            this.ui_titlePanel.Click += new System.EventHandler(this.ui_titlePanel_Click);
            this.ui_titlePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ui_titlePanel_Paint);
            // 
            // ui_tableLayoutPanel1
            // 
            this.ui_tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_tableLayoutPanel1.AutoSize = true;
            this.ui_tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.ui_tableLayoutPanel1.ColumnCount = 1;
            this.ui_tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ui_tableLayoutPanel1.Location = new System.Drawing.Point(3, 33);
            this.ui_tableLayoutPanel1.Name = "ui_tableLayoutPanel1";
            this.ui_tableLayoutPanel1.RowCount = 1;
            this.ui_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ui_tableLayoutPanel1.Size = new System.Drawing.Size(271, 12);
            this.ui_tableLayoutPanel1.TabIndex = 4;
            this.ui_tableLayoutPanel1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.flowLayoutPanel1_ControlAdded);
            this.ui_tableLayoutPanel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.flowLayoutPanel1_ControlRemoved);
            // 
            // DbEntityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(this.ui_tableLayoutPanel1);
            this.Controls.Add(this.ui_titlePanel);
            this.Name = "DbEntityControl";
            this.Size = new System.Drawing.Size(277, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ui_titlePanel;
        private System.Windows.Forms.TableLayoutPanel ui_tableLayoutPanel1;
    }
}
