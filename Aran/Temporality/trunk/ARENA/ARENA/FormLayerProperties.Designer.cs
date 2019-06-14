namespace ARENA
{
    partial class FormLayerProperties
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonDn = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.DeleteLayerButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(248, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(55, 525);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.DeleteLayerButton);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.buttonDn);
            this.panel3.Controls.Add(this.buttonUp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(55, 410);
            this.panel3.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = global::ARENA.Properties.Resources.configure1;
            this.button3.Location = new System.Drawing.Point(2, 268);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 55);
            this.button3.TabIndex = 5;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonDn
            // 
            this.buttonDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDn.Image = global::ARENA.Properties.Resources.down;
            this.buttonDn.Location = new System.Drawing.Point(2, 73);
            this.buttonDn.Name = "buttonDn";
            this.buttonDn.Size = new System.Drawing.Size(49, 55);
            this.buttonDn.TabIndex = 4;
            this.buttonDn.UseVisualStyleBackColor = true;
            this.buttonDn.Click += new System.EventHandler(this.buttonDn_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Image = global::ARENA.Properties.Resources.up;
            this.buttonUp.Location = new System.Drawing.Point(2, 10);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(49, 55);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 410);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(55, 115);
            this.panel2.TabIndex = 4;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = global::ARENA.Properties.Resources.apply;
            this.button4.Location = new System.Drawing.Point(2, 55);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(49, 55);
            this.button4.TabIndex = 4;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(248, 525);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // DeleteLayerButton
            // 
            this.DeleteLayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteLayerButton.Image = global::ARENA.Properties.Resources.cancel;
            this.DeleteLayerButton.Location = new System.Drawing.Point(0, 174);
            this.DeleteLayerButton.Name = "DeleteLayerButton";
            this.DeleteLayerButton.Size = new System.Drawing.Size(49, 55);
            this.DeleteLayerButton.TabIndex = 6;
            this.DeleteLayerButton.UseVisualStyleBackColor = true;
            this.DeleteLayerButton.Click += new System.EventHandler(this.DeleteLayerButton_Click);
            // 
            // FormLayerProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 525);
            this.ControlBox = false;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "FormLayerProperties";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Layers";
            this.Load += new System.EventHandler(this.FormLayerProperties_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonDn;
        private System.Windows.Forms.Button buttonUp;
        public System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button DeleteLayerButton;

    }
}