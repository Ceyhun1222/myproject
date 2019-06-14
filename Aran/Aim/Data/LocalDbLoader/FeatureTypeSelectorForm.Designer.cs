namespace Aran.Aim.Data.LocalDbLoader
{
    partial class FeatureTypeSelectorForm
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
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_quickSearchTB = new System.Windows.Forms.TextBox();
            this.ui_featTypeGroupCB = new System.Windows.Forms.ComboBox();
            this.ui_listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(236, 439);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 0;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.Location = new System.Drawing.Point(317, 439);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 1;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_quickSearchTB
            // 
            this.ui_quickSearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_quickSearchTB.Location = new System.Drawing.Point(259, 4);
            this.ui_quickSearchTB.Name = "ui_quickSearchTB";
            this.ui_quickSearchTB.Size = new System.Drawing.Size(140, 20);
            this.ui_quickSearchTB.TabIndex = 16;
            this.ui_quickSearchTB.TextChanged += new System.EventHandler(this.QuickSearch_TextChanged);
            // 
            // ui_featTypeGroupCB
            // 
            this.ui_featTypeGroupCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_featTypeGroupCB.FormattingEnabled = true;
            this.ui_featTypeGroupCB.Location = new System.Drawing.Point(2, 4);
            this.ui_featTypeGroupCB.Name = "ui_featTypeGroupCB";
            this.ui_featTypeGroupCB.Size = new System.Drawing.Size(156, 21);
            this.ui_featTypeGroupCB.TabIndex = 15;
            this.ui_featTypeGroupCB.SelectedIndexChanged += new System.EventHandler(this.FeatureTypeGroup_SelectedIndexChanged);
            // 
            // ui_listView
            // 
            this.ui_listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_listView.CheckBoxes = true;
            this.ui_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ui_listView.HideSelection = false;
            this.ui_listView.Location = new System.Drawing.Point(2, 30);
            this.ui_listView.MultiSelect = false;
            this.ui_listView.Name = "ui_listView";
            this.ui_listView.ShowGroups = false;
            this.ui_listView.Size = new System.Drawing.Size(397, 400);
            this.ui_listView.TabIndex = 13;
            this.ui_listView.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Feature Type";
            this.columnHeader1.Width = 300;
            // 
            // FeatureTypeSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 471);
            this.Controls.Add(this.ui_quickSearchTB);
            this.Controls.Add(this.ui_featTypeGroupCB);
            this.Controls.Add(this.ui_listView);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureTypeSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Feature Type";
            this.Load += new System.EventHandler(this.FeatureTypeSelectorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.TextBox ui_quickSearchTB;
        private System.Windows.Forms.ComboBox ui_featTypeGroupCB;
        private System.Windows.Forms.ListView ui_listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}