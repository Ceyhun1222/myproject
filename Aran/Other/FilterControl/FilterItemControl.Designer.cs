namespace Aran.Controls
{
    partial class FilterItemControl
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
            this.ui_operTypeCB = new System.Windows.Forms.ComboBox();
            this.ui_aimPropertyControl = new Aran.Controls.AimPropertyControl();
            this.ui_operValuePanel = new System.Windows.Forms.Panel();
            this.ui_dwithinGroupBox = new System.Windows.Forms.GroupBox();
            this.ui_dwithinControl = new Aran.Controls.DWithinControl();
            this.ui_propNameTB = new System.Windows.Forms.TextBox();
            this.ui_settingsPicBox = new System.Windows.Forms.PictureBox();
            this.ui_settingsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_operValuePanel.SuspendLayout();
            this.ui_dwithinGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_settingsPicBox)).BeginInit();
            this.ui_settingsMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(80, 13);
            label1.TabIndex = 0;
            label1.Text = "Property Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(1, 3);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(37, 13);
            label3.TabIndex = 7;
            label3.Text = "Value:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 35);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 8;
            label2.Text = "Operation:";
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            deleteToolStripMenuItem.Text = "Remove Filter Condition";
            deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // ui_operTypeCB
            // 
            this.ui_operTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_operTypeCB.FormattingEnabled = true;
            this.ui_operTypeCB.Location = new System.Drawing.Point(7, 51);
            this.ui_operTypeCB.Name = "ui_operTypeCB";
            this.ui_operTypeCB.Size = new System.Drawing.Size(94, 21);
            this.ui_operTypeCB.TabIndex = 9;
            this.ui_operTypeCB.SelectedIndexChanged += new System.EventHandler(this.OperType_SelectedIndexChanged);
            // 
            // ui_aimPropertyControl
            // 
            this.ui_aimPropertyControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_aimPropertyControl.FillDataGridColumnsHandler = null;
            this.ui_aimPropertyControl.LoadFeatureListByDependHandler = null;
            this.ui_aimPropertyControl.Location = new System.Drawing.Point(5, 19);
            this.ui_aimPropertyControl.Name = "ui_aimPropertyControl";
            this.ui_aimPropertyControl.PropInfo = null;
            this.ui_aimPropertyControl.SetDataGridRowHandler = null;
            this.ui_aimPropertyControl.Size = new System.Drawing.Size(335, 26);
            this.ui_aimPropertyControl.TabIndex = 6;
            // 
            // ui_operValuePanel
            // 
            this.ui_operValuePanel.AutoSize = true;
            this.ui_operValuePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_operValuePanel.Controls.Add(this.ui_aimPropertyControl);
            this.ui_operValuePanel.Controls.Add(this.ui_dwithinGroupBox);
            this.ui_operValuePanel.Controls.Add(label3);
            this.ui_operValuePanel.Location = new System.Drawing.Point(106, 31);
            this.ui_operValuePanel.Name = "ui_operValuePanel";
            this.ui_operValuePanel.Size = new System.Drawing.Size(344, 110);
            this.ui_operValuePanel.TabIndex = 10;
            this.ui_operValuePanel.Visible = false;
            // 
            // ui_dwithinGroupBox
            // 
            this.ui_dwithinGroupBox.Controls.Add(this.ui_dwithinControl);
            this.ui_dwithinGroupBox.Location = new System.Drawing.Point(4, 40);
            this.ui_dwithinGroupBox.Name = "ui_dwithinGroupBox";
            this.ui_dwithinGroupBox.Size = new System.Drawing.Size(337, 67);
            this.ui_dwithinGroupBox.TabIndex = 12;
            this.ui_dwithinGroupBox.TabStop = false;
            this.ui_dwithinGroupBox.Visible = false;
            // 
            // ui_dwithinControl
            // 
            this.ui_dwithinControl.Location = new System.Drawing.Point(4, 11);
            this.ui_dwithinControl.Name = "ui_dwithinControl";
            this.ui_dwithinControl.Size = new System.Drawing.Size(329, 54);
            this.ui_dwithinControl.TabIndex = 11;
            // 
            // ui_propNameTB
            // 
            this.ui_propNameTB.Location = new System.Drawing.Point(90, 6);
            this.ui_propNameTB.Name = "ui_propNameTB";
            this.ui_propNameTB.ReadOnly = true;
            this.ui_propNameTB.Size = new System.Drawing.Size(343, 20);
            this.ui_propNameTB.TabIndex = 11;
            // 
            // ui_settingsPicBox
            // 
            this.ui_settingsPicBox.Image = global::Aran.Controls.Properties.Resources.settings_16;
            this.ui_settingsPicBox.Location = new System.Drawing.Point(436, 8);
            this.ui_settingsPicBox.Name = "ui_settingsPicBox";
            this.ui_settingsPicBox.Size = new System.Drawing.Size(16, 16);
            this.ui_settingsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ui_settingsPicBox.TabIndex = 12;
            this.ui_settingsPicBox.TabStop = false;
            this.ui_settingsPicBox.Click += new System.EventHandler(this.SettingsPicBox_Click);
            // 
            // ui_settingsMenuStrip
            // 
            this.ui_settingsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            deleteToolStripMenuItem});
            this.ui_settingsMenuStrip.Name = "contextMenuStrip1";
            this.ui_settingsMenuStrip.Size = new System.Drawing.Size(203, 26);
            // 
            // FilterItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ui_settingsPicBox);
            this.Controls.Add(this.ui_propNameTB);
            this.Controls.Add(this.ui_operValuePanel);
            this.Controls.Add(this.ui_operTypeCB);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Name = "FilterItemControl";
            this.Size = new System.Drawing.Size(455, 144);
            this.ui_operValuePanel.ResumeLayout(false);
            this.ui_operValuePanel.PerformLayout();
            this.ui_dwithinGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_settingsPicBox)).EndInit();
            this.ui_settingsMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AimPropertyControl ui_aimPropertyControl;
        private System.Windows.Forms.ComboBox ui_operTypeCB;
        private System.Windows.Forms.Panel ui_operValuePanel;
        private DWithinControl ui_dwithinControl;
        private System.Windows.Forms.GroupBox ui_dwithinGroupBox;
        private System.Windows.Forms.TextBox ui_propNameTB;
        private System.Windows.Forms.PictureBox ui_settingsPicBox;
        private System.Windows.Forms.ContextMenuStrip ui_settingsMenuStrip;



    }
}
