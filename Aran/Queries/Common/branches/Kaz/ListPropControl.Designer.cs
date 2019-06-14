namespace Aran.Queries.Common
{
    partial class ListPropControl
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
            this.components = new System.ComponentModel.Container();
            this.mainGrb = new System.Windows.Forms.GroupBox();
            this.newItemButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.propNameLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mainGrb.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGrb
            // 
            this.mainGrb.Controls.Add(this.newItemButton);
            this.mainGrb.Controls.Add(this.flowLayoutPanel1);
            this.mainGrb.Controls.Add(this.propNameLabel);
            this.mainGrb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainGrb.Location = new System.Drawing.Point(0, 0);
            this.mainGrb.Name = "mainGrb";
            this.mainGrb.Size = new System.Drawing.Size(182, 57);
            this.mainGrb.TabIndex = 0;
            this.mainGrb.TabStop = false;
            // 
            // newItemButton
            // 
            this.newItemButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.newItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newItemButton.Image = global::Aran.Queries.Common.Properties.Resources.add_new;
            this.newItemButton.Location = new System.Drawing.Point(144, 16);
            this.newItemButton.Name = "newItemButton";
            this.newItemButton.Size = new System.Drawing.Size(32, 32);
            this.newItemButton.TabIndex = 2;
            this.toolTip1.SetToolTip(this.newItemButton, "Add new list item");
            this.newItemButton.UseVisualStyleBackColor = true;
            this.newItemButton.Click += new System.EventHandler(this.newItemButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(101, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(26, 31);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.SizeChanged += new System.EventHandler(this.flowLayoutPanel1_SizeChanged);
            // 
            // propNameLabel
            // 
            this.propNameLabel.AutoSize = true;
            this.propNameLabel.Location = new System.Drawing.Point(6, 25);
            this.propNameLabel.Name = "propNameLabel";
            this.propNameLabel.Size = new System.Drawing.Size(89, 13);
            this.propNameLabel.TabIndex = 0;
            this.propNameLabel.Text = "<Property Name>";
            // 
            // ListPropControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainGrb);
            this.Name = "ListPropControl";
            this.Size = new System.Drawing.Size(182, 57);
            this.mainGrb.ResumeLayout(false);
            this.mainGrb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mainGrb;
        private System.Windows.Forms.Label propNameLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button newItemButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
