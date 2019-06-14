namespace ARENA
{
    partial class FilterForm
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
            this.comboBoxObjectTypes = new System.Windows.Forms.ComboBox();
            this.comboBoxObjectProperty = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxOperationType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFilterValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonAddFilter = new System.Windows.Forms.Button();
            this.listBoxFilters = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCloseForm = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonClearFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select object Type";
            // 
            // comboBoxObjectTypes
            // 
            this.comboBoxObjectTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxObjectTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObjectTypes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxObjectTypes.FormattingEnabled = true;
            this.comboBoxObjectTypes.Location = new System.Drawing.Point(151, 18);
            this.comboBoxObjectTypes.Name = "comboBoxObjectTypes";
            this.comboBoxObjectTypes.Size = new System.Drawing.Size(343, 23);
            this.comboBoxObjectTypes.TabIndex = 1;
            this.comboBoxObjectTypes.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjectTypes_SelectedIndexChanged);
            // 
            // comboBoxObjectProperty
            // 
            this.comboBoxObjectProperty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxObjectProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObjectProperty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxObjectProperty.FormattingEnabled = true;
            this.comboBoxObjectProperty.Location = new System.Drawing.Point(151, 47);
            this.comboBoxObjectProperty.Name = "comboBoxObjectProperty";
            this.comboBoxObjectProperty.Size = new System.Drawing.Size(343, 23);
            this.comboBoxObjectProperty.TabIndex = 3;
            this.comboBoxObjectProperty.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjectProperty_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select objects Property";
            // 
            // comboBoxOperationType
            // 
            this.comboBoxOperationType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOperationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxOperationType.FormattingEnabled = true;
            this.comboBoxOperationType.Items.AddRange(new object[] {
            "=",
            "<>",
            ">",
            "<"});
            this.comboBoxOperationType.Location = new System.Drawing.Point(151, 76);
            this.comboBoxOperationType.Name = "comboBoxOperationType";
            this.comboBoxOperationType.Size = new System.Drawing.Size(343, 23);
            this.comboBoxOperationType.TabIndex = 5;
            this.comboBoxOperationType.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperationType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select operation type";
            // 
            // comboBoxFilterValue
            // 
            this.comboBoxFilterValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFilterValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxFilterValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFilterValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxFilterValue.FormattingEnabled = true;
            this.comboBoxFilterValue.Location = new System.Drawing.Point(151, 105);
            this.comboBoxFilterValue.Name = "comboBoxFilterValue";
            this.comboBoxFilterValue.Size = new System.Drawing.Size(343, 23);
            this.comboBoxFilterValue.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Set filter value";
            // 
            // buttonAddFilter
            // 
            this.buttonAddFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddFilter.Image = global::ARENA.Properties.Resources.down;
            this.buttonAddFilter.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAddFilter.Location = new System.Drawing.Point(397, 144);
            this.buttonAddFilter.Name = "buttonAddFilter";
            this.buttonAddFilter.Size = new System.Drawing.Size(113, 48);
            this.buttonAddFilter.TabIndex = 8;
            this.buttonAddFilter.Text = "Add filter to list";
            this.buttonAddFilter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonAddFilter.UseVisualStyleBackColor = true;
            this.buttonAddFilter.Click += new System.EventHandler(this.buttonAddFilter_Click);
            // 
            // listBoxFilters
            // 
            this.listBoxFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFilters.FormattingEnabled = true;
            this.listBoxFilters.HorizontalScrollbar = true;
            this.listBoxFilters.ItemHeight = 15;
            this.listBoxFilters.Location = new System.Drawing.Point(0, 196);
            this.listBoxFilters.Name = "listBoxFilters";
            this.listBoxFilters.Size = new System.Drawing.Size(518, 246);
            this.listBoxFilters.TabIndex = 9;
            this.listBoxFilters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listBoxFilters_KeyPress);
            this.listBoxFilters.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxFilters_KeyUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.buttonAddFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 196);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonCloseForm);
            this.panel2.Controls.Add(this.buttonApply);
            this.panel2.Controls.Add(this.buttonClearFilter);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 442);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 36);
            this.panel2.TabIndex = 11;
            // 
            // buttonCloseForm
            // 
            this.buttonCloseForm.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonCloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseForm.Image = global::ARENA.Properties.Resources.cancel;
            this.buttonCloseForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCloseForm.Location = new System.Drawing.Point(302, 4);
            this.buttonCloseForm.Name = "buttonCloseForm";
            this.buttonCloseForm.Size = new System.Drawing.Size(108, 29);
            this.buttonCloseForm.TabIndex = 11;
            this.buttonCloseForm.Text = "Close";
            this.buttonCloseForm.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonApply.Enabled = false;
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Image = global::ARENA.Properties.Resources.filter;
            this.buttonApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonApply.Location = new System.Drawing.Point(414, 4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(98, 29);
            this.buttonApply.TabIndex = 10;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonClearFilter
            // 
            this.buttonClearFilter.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearFilter.Image = global::ARENA.Properties.Resources.refresh;
            this.buttonClearFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClearFilter.Location = new System.Drawing.Point(3, 4);
            this.buttonClearFilter.MinimumSize = new System.Drawing.Size(150, 30);
            this.buttonClearFilter.Name = "buttonClearFilter";
            this.buttonClearFilter.Size = new System.Drawing.Size(150, 30);
            this.buttonClearFilter.TabIndex = 9;
            this.buttonClearFilter.Text = "Clear filter list";
            this.buttonClearFilter.UseVisualStyleBackColor = true;
            this.buttonClearFilter.Click += new System.EventHandler(this.buttonClearFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxOperationType);
            this.groupBox1.Controls.Add(this.comboBoxObjectTypes);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxObjectProperty);
            this.groupBox1.Controls.Add(this.comboBoxFilterValue);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(6, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 135);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create Filter";
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 478);
            this.ControlBox = false;
            this.Controls.Add(this.listBoxFilters);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 377);
            this.Name = "FilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Arena ";
            this.Load += new System.EventHandler(this.FilterForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxObjectProperty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxOperationType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFilterValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonAddFilter;
        private System.Windows.Forms.ListBox listBoxFilters;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonCloseForm;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonClearFilter;
        public System.Windows.Forms.ComboBox comboBoxObjectTypes;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}