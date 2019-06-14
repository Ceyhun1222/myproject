namespace ARENA
{
    partial class EnrouteDataView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.route_dataGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button_editADHP = new System.Windows.Forms.Button();
            this.button_DeleteADHP = new System.Windows.Forms.Button();
            this.button_AddADHP = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.routeSegment_dataGridView1 = new System.Windows.Forms.DataGridView();
            this.startPoint_dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.route_dataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.routeSegment_dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPoint_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.route_dataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.routeSegment_dataGridView1);
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Panel2.Controls.Add(this.startPoint_dataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(1048, 757);
            this.splitContainer1.SplitterDistance = 382;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // route_dataGridView
            // 
            this.route_dataGridView.AllowUserToAddRows = false;
            this.route_dataGridView.AllowUserToDeleteRows = false;
            this.route_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.route_dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.route_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.route_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.route_dataGridView.Location = new System.Drawing.Point(0, 0);
            this.route_dataGridView.MultiSelect = false;
            this.route_dataGridView.Name = "route_dataGridView";
            this.route_dataGridView.ReadOnly = true;
            this.route_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.route_dataGridView.ShowEditingIcon = false;
            this.route_dataGridView.Size = new System.Drawing.Size(382, 724);
            this.route_dataGridView.TabIndex = 14;
            this.route_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.route_dataGridView_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button_editADHP);
            this.panel1.Controls.Add(this.button_DeleteADHP);
            this.panel1.Controls.Add(this.button_AddADHP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 724);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 33);
            this.panel1.TabIndex = 15;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.ForeColor = System.Drawing.Color.Maroon;
            this.button4.Location = new System.Drawing.Point(3, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 27);
            this.button4.TabIndex = 6;
            this.button4.Text = "Delete All";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button_editADHP
            // 
            this.button_editADHP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_editADHP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_editADHP.Location = new System.Drawing.Point(196, 3);
            this.button_editADHP.Name = "button_editADHP";
            this.button_editADHP.Size = new System.Drawing.Size(87, 27);
            this.button_editADHP.TabIndex = 2;
            this.button_editADHP.Text = "Edit";
            this.button_editADHP.UseVisualStyleBackColor = true;
            this.button_editADHP.Visible = false;
            this.button_editADHP.Click += new System.EventHandler(this.button_editADHP_Click);
            // 
            // button_DeleteADHP
            // 
            this.button_DeleteADHP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_DeleteADHP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_DeleteADHP.Location = new System.Drawing.Point(287, 3);
            this.button_DeleteADHP.Name = "button_DeleteADHP";
            this.button_DeleteADHP.Size = new System.Drawing.Size(87, 27);
            this.button_DeleteADHP.TabIndex = 1;
            this.button_DeleteADHP.Text = "Delete";
            this.button_DeleteADHP.UseVisualStyleBackColor = true;
            this.button_DeleteADHP.Click += new System.EventHandler(this.button_DeleteADHP_Click);
            // 
            // button_AddADHP
            // 
            this.button_AddADHP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_AddADHP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_AddADHP.Location = new System.Drawing.Point(105, 3);
            this.button_AddADHP.Name = "button_AddADHP";
            this.button_AddADHP.Size = new System.Drawing.Size(87, 27);
            this.button_AddADHP.TabIndex = 0;
            this.button_AddADHP.Text = "Add";
            this.button_AddADHP.UseVisualStyleBackColor = true;
            this.button_AddADHP.Visible = false;
            this.button_AddADHP.Click += new System.EventHandler(this.button_AddADHP_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 663);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(661, 5);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // routeSegment_dataGridView1
            // 
            this.routeSegment_dataGridView1.AllowUserToAddRows = false;
            this.routeSegment_dataGridView1.AllowUserToDeleteRows = false;
            this.routeSegment_dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.routeSegment_dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.routeSegment_dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.routeSegment_dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeSegment_dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.routeSegment_dataGridView1.MultiSelect = false;
            this.routeSegment_dataGridView1.Name = "routeSegment_dataGridView1";
            this.routeSegment_dataGridView1.ReadOnly = true;
            this.routeSegment_dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.routeSegment_dataGridView1.ShowEditingIcon = false;
            this.routeSegment_dataGridView1.Size = new System.Drawing.Size(661, 663);
            this.routeSegment_dataGridView1.TabIndex = 15;
            // 
            // startPoint_dataGridView
            // 
            this.startPoint_dataGridView.AllowUserToAddRows = false;
            this.startPoint_dataGridView.AllowUserToDeleteRows = false;
            this.startPoint_dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.startPoint_dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.startPoint_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.startPoint_dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.startPoint_dataGridView.Location = new System.Drawing.Point(0, 668);
            this.startPoint_dataGridView.MultiSelect = false;
            this.startPoint_dataGridView.Name = "startPoint_dataGridView";
            this.startPoint_dataGridView.ReadOnly = true;
            this.startPoint_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.startPoint_dataGridView.ShowEditingIcon = false;
            this.startPoint_dataGridView.Size = new System.Drawing.Size(661, 89);
            this.startPoint_dataGridView.TabIndex = 16;
            // 
            // EnrouteDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "EnrouteDataView";
            this.Size = new System.Drawing.Size(1048, 757);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.route_dataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.routeSegment_dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPoint_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView route_dataGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_editADHP;
        private System.Windows.Forms.Button button_DeleteADHP;
        private System.Windows.Forms.Button button_AddADHP;
        private System.Windows.Forms.DataGridView routeSegment_dataGridView1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView startPoint_dataGridView;
    }
}
