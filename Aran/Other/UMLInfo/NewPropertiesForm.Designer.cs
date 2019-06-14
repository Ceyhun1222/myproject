namespace UMLInfo
{
    partial class NewPropertiesForm
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
            this.propDGV = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pattern = new System.Windows.Forms.Label();
            this.maxLength = new System.Windows.Forms.Label();
            this.minLength = new System.Windows.Forms.Label();
            this.nilReason = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sort = new System.Windows.Forms.Button();
            this.methodShow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.propDGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propDGV
            // 
            this.propDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.propDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propDGV.Location = new System.Drawing.Point(12, 41);
            this.propDGV.Name = "propDGV";
            this.propDGV.Size = new System.Drawing.Size(682, 292);
            this.propDGV.TabIndex = 0;
            this.propDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.propDGV_CellMouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.pattern);
            this.groupBox1.Controls.Add(this.maxLength);
            this.groupBox1.Controls.Add(this.minLength);
            this.groupBox1.Controls.Add(this.nilReason);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 374);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 124);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // pattern
            // 
            this.pattern.AutoSize = true;
            this.pattern.Location = new System.Drawing.Point(78, 95);
            this.pattern.Name = "pattern";
            this.pattern.Size = new System.Drawing.Size(0, 13);
            this.pattern.TabIndex = 6;
            // 
            // maxLength
            // 
            this.maxLength.AutoSize = true;
            this.maxLength.Location = new System.Drawing.Point(78, 69);
            this.maxLength.Name = "maxLength";
            this.maxLength.Size = new System.Drawing.Size(0, 13);
            this.maxLength.TabIndex = 5;
            // 
            // minLength
            // 
            this.minLength.AutoSize = true;
            this.minLength.Location = new System.Drawing.Point(78, 41);
            this.minLength.Name = "minLength";
            this.minLength.Size = new System.Drawing.Size(0, 13);
            this.minLength.TabIndex = 4;
            // 
            // nilReason
            // 
            this.nilReason.AutoSize = true;
            this.nilReason.Location = new System.Drawing.Point(78, 16);
            this.nilReason.Name = "nilReason";
            this.nilReason.Size = new System.Drawing.Size(0, 13);
            this.nilReason.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "pattern";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "maxLength";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "minLength";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "nilReason";
            // 
            // sort
            // 
            this.sort.Location = new System.Drawing.Point(12, 12);
            this.sort.Name = "sort";
            this.sort.Size = new System.Drawing.Size(75, 23);
            this.sort.TabIndex = 2;
            this.sort.Text = "Sort";
            this.sort.UseVisualStyleBackColor = true;
            this.sort.Click += new System.EventHandler(this.sort_Click);
            // 
            // methodShow
            // 
            this.methodShow.Location = new System.Drawing.Point(93, 12);
            this.methodShow.Name = "methodShow";
            this.methodShow.Size = new System.Drawing.Size(86, 23);
            this.methodShow.TabIndex = 3;
            this.methodShow.Text = "Show Methods";
            this.methodShow.UseVisualStyleBackColor = true;
            this.methodShow.Click += new System.EventHandler(this.methodShow_Click);
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 498);
            this.Controls.Add(this.methodShow);
            this.Controls.Add(this.sort);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.propDGV);
            this.Name = "PropertiesForm";
            this.Text = "PropertiesForm";
            ((System.ComponentModel.ISupportInitialize)(this.propDGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView propDGV;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label pattern;
        private System.Windows.Forms.Label maxLength;
        private System.Windows.Forms.Label minLength;
        private System.Windows.Forms.Label nilReason;
        private System.Windows.Forms.Button sort;
        private System.Windows.Forms.Button methodShow;
    }
}