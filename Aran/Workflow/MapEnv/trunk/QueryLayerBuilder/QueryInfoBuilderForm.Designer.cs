using Aran.Controls;
namespace MapEnv
{
    partial class QueryInfoBuilderForm
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
            this.components = new System.ComponentModel.Container ();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (QueryInfoBuilderForm));
            this.ui_nameOrPropLabel = new System.Windows.Forms.Label ();
            this.ui_queryInfosTreeView = new System.Windows.Forms.TreeView ();
            this.ui_addQIButton = new System.Windows.Forms.Button ();
            this.ui_tabControl = new System.Windows.Forms.TabControl ();
            this.ui_styleTabPage = new System.Windows.Forms.TabPage ();
            this.ui_featureStyleControl = new MapEnv.FeatureStyleControl ();
            this.ui_filterTabPage = new System.Windows.Forms.TabPage ();
            this.ui_filterControl = new Aran.Controls.FilterControl ();
            this.ui_addSubQueryButton = new System.Windows.Forms.Button ();
            this.toolTip1 = new System.Windows.Forms.ToolTip (this.components);
            this.ui_removeLayerButton = new System.Windows.Forms.Button ();
            this.ui_mainMenuButton = new System.Windows.Forms.Button ();
            this.ui_nameOrPropertyTB = new System.Windows.Forms.TextBox ();
            this.ui_okButton = new System.Windows.Forms.Button ();
            this.ui_cancelButton = new System.Windows.Forms.Button ();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel ();
            this.ui_mainContextMenu = new System.Windows.Forms.ContextMenuStrip (this.components);
            this.ui_openSettingsTSMI = new System.Windows.Forms.ToolStripMenuItem ();
            this.ui_saveSettingsTSMI = new System.Windows.Forms.ToolStripMenuItem ();
            this.ui_tabControl.SuspendLayout ();
            this.ui_styleTabPage.SuspendLayout ();
            this.ui_filterTabPage.SuspendLayout ();
            this.flowLayoutPanel1.SuspendLayout ();
            this.ui_mainContextMenu.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // ui_nameOrPropLabel
            // 
            this.ui_nameOrPropLabel.AutoSize = true;
            this.ui_nameOrPropLabel.Location = new System.Drawing.Point (194, 14);
            this.ui_nameOrPropLabel.Name = "ui_nameOrPropLabel";
            this.ui_nameOrPropLabel.Size = new System.Drawing.Size (69, 13);
            this.ui_nameOrPropLabel.TabIndex = 5;
            this.ui_nameOrPropLabel.Text = "Query Name:";
            // 
            // ui_queryInfosTreeView
            // 
            this.ui_queryInfosTreeView.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_queryInfosTreeView.HideSelection = false;
            this.ui_queryInfosTreeView.Location = new System.Drawing.Point (3, 42);
            this.ui_queryInfosTreeView.Name = "ui_queryInfosTreeView";
            this.ui_queryInfosTreeView.Size = new System.Drawing.Size (179, 413);
            this.ui_queryInfosTreeView.TabIndex = 0;
            this.ui_queryInfosTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler (this.QueryInfos_AfterSelect);
            // 
            // ui_addQIButton
            // 
            this.ui_addQIButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ui_addQIButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ui_addQIButton.Image = global::MapEnv.Properties.Resources.add_layer_24;
            this.ui_addQIButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_addQIButton.Location = new System.Drawing.Point (1, 3);
            this.ui_addQIButton.Margin = new System.Windows.Forms.Padding (1, 3, 1, 3);
            this.ui_addQIButton.Name = "ui_addQIButton";
            this.ui_addQIButton.Size = new System.Drawing.Size (59, 34);
            this.ui_addQIButton.TabIndex = 1;
            this.ui_addQIButton.Text = "Add";
            this.ui_addQIButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip (this.ui_addQIButton, "Add Layer");
            this.ui_addQIButton.UseVisualStyleBackColor = true;
            this.ui_addQIButton.Visible = false;
            this.ui_addQIButton.Click += new System.EventHandler (this.AddQI_Click);
            // 
            // ui_tabControl
            // 
            this.ui_tabControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.ui_tabControl.Controls.Add (this.ui_styleTabPage);
            this.ui_tabControl.Controls.Add (this.ui_filterTabPage);
            this.ui_tabControl.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.ui_tabControl.ItemSize = new System.Drawing.Size (140, 30);
            this.ui_tabControl.Location = new System.Drawing.Point (188, 42);
            this.ui_tabControl.Name = "ui_tabControl";
            this.ui_tabControl.SelectedIndex = 0;
            this.ui_tabControl.Size = new System.Drawing.Size (543, 413);
            this.ui_tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.ui_tabControl.TabIndex = 2;
            // 
            // ui_styleTabPage
            // 
            this.ui_styleTabPage.Controls.Add (this.ui_featureStyleControl);
            this.ui_styleTabPage.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
            this.ui_styleTabPage.Location = new System.Drawing.Point (4, 34);
            this.ui_styleTabPage.Name = "ui_styleTabPage";
            this.ui_styleTabPage.Padding = new System.Windows.Forms.Padding (3);
            this.ui_styleTabPage.Size = new System.Drawing.Size (535, 375);
            this.ui_styleTabPage.TabIndex = 0;
            this.ui_styleTabPage.Text = "Symbols";
            // 
            // ui_featureStyleControl
            // 
            this.ui_featureStyleControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featureStyleControl.Location = new System.Drawing.Point (6, 6);
            this.ui_featureStyleControl.Name = "ui_featureStyleControl";
            this.ui_featureStyleControl.Size = new System.Drawing.Size (523, 363);
            this.ui_featureStyleControl.TabIndex = 3;
            this.ui_featureStyleControl.ValueChanged += new System.EventHandler (this.FeatureStyleControl_ValueChanged);
            // 
            // ui_filterTabPage
            // 
            this.ui_filterTabPage.Controls.Add (this.ui_filterControl);
            this.ui_filterTabPage.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
            this.ui_filterTabPage.Location = new System.Drawing.Point (4, 34);
            this.ui_filterTabPage.Name = "ui_filterTabPage";
            this.ui_filterTabPage.Padding = new System.Windows.Forms.Padding (3);
            this.ui_filterTabPage.Size = new System.Drawing.Size (356, 375);
            this.ui_filterTabPage.TabIndex = 1;
            this.ui_filterTabPage.Text = "Filter";
            // 
            // ui_filterControl
            // 
            this.ui_filterControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_filterControl.FeatureDescription = null;
            this.ui_filterControl.FillDataGridColumnsHandler = null;
            this.ui_filterControl.LoadFeatureListByDependHandler = null;
            this.ui_filterControl.Location = new System.Drawing.Point (0, 0);
            this.ui_filterControl.Name = "ui_filterControl";
            this.ui_filterControl.SetDataGridRowHandler = null;
            this.ui_filterControl.Size = new System.Drawing.Size (356, 378);
            this.ui_filterControl.TabIndex = 0;
            this.ui_filterControl.ValueChanged += new System.EventHandler (this.FilterControl_ValueChanged);
            // 
            // ui_addSubQueryButton
            // 
            this.ui_addSubQueryButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ui_addSubQueryButton.Enabled = false;
            this.ui_addSubQueryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ui_addSubQueryButton.Image = global::MapEnv.Properties.Resources.link_add_24;
            this.ui_addSubQueryButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_addSubQueryButton.Location = new System.Drawing.Point (62, 3);
            this.ui_addSubQueryButton.Margin = new System.Windows.Forms.Padding (1, 3, 1, 3);
            this.ui_addSubQueryButton.Name = "ui_addSubQueryButton";
            this.ui_addSubQueryButton.Size = new System.Drawing.Size (78, 34);
            this.ui_addSubQueryButton.TabIndex = 3;
            this.ui_addSubQueryButton.Text = "Add Link";
            this.ui_addSubQueryButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip (this.ui_addSubQueryButton, "Add Link Layer");
            this.ui_addSubQueryButton.UseVisualStyleBackColor = true;
            this.ui_addSubQueryButton.Click += new System.EventHandler (this.AddSubQuery_Click);
            // 
            // ui_removeLayerButton
            // 
            this.ui_removeLayerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ui_removeLayerButton.Enabled = false;
            this.ui_removeLayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ui_removeLayerButton.Image = global::MapEnv.Properties.Resources.remove_layer_24;
            this.ui_removeLayerButton.Location = new System.Drawing.Point (142, 3);
            this.ui_removeLayerButton.Margin = new System.Windows.Forms.Padding (1, 3, 1, 3);
            this.ui_removeLayerButton.Name = "ui_removeLayerButton";
            this.ui_removeLayerButton.Size = new System.Drawing.Size (34, 34);
            this.ui_removeLayerButton.TabIndex = 4;
            this.toolTip1.SetToolTip (this.ui_removeLayerButton, "Remove Selected Layer");
            this.ui_removeLayerButton.UseVisualStyleBackColor = true;
            this.ui_removeLayerButton.Click += new System.EventHandler (this.RemoveLayer_Click);
            // 
            // ui_mainMenuButton
            // 
            this.ui_mainMenuButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_mainMenuButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ui_mainMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ui_mainMenuButton.Image = global::MapEnv.Properties.Resources.menu_24;
            this.ui_mainMenuButton.Location = new System.Drawing.Point (3, 460);
            this.ui_mainMenuButton.Margin = new System.Windows.Forms.Padding (1, 3, 1, 3);
            this.ui_mainMenuButton.Name = "ui_mainMenuButton";
            this.ui_mainMenuButton.Size = new System.Drawing.Size (34, 34);
            this.ui_mainMenuButton.TabIndex = 12;
            this.toolTip1.SetToolTip (this.ui_mainMenuButton, "Remove Selected Layer");
            this.ui_mainMenuButton.UseVisualStyleBackColor = true;
            this.ui_mainMenuButton.Click += new System.EventHandler (this.MainMenu_Click);
            // 
            // ui_nameOrPropertyTB
            // 
            this.ui_nameOrPropertyTB.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nameOrPropertyTB.Location = new System.Drawing.Point (272, 12);
            this.ui_nameOrPropertyTB.Name = "ui_nameOrPropertyTB";
            this.ui_nameOrPropertyTB.Size = new System.Drawing.Size (449, 20);
            this.ui_nameOrPropertyTB.TabIndex = 6;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Image = global::MapEnv.Properties.Resources.ok_apply_24;
            this.ui_okButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_okButton.Location = new System.Drawing.Point (559, 464);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Padding = new System.Windows.Forms.Padding (5, 0, 10, 0);
            this.ui_okButton.Size = new System.Drawing.Size (82, 26);
            this.ui_okButton.TabIndex = 8;
            this.ui_okButton.Text = "&OK";
            this.ui_okButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler (this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.Image = global::MapEnv.Properties.Resources.cancel_24;
            this.ui_cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_cancelButton.Location = new System.Drawing.Point (647, 464);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Padding = new System.Windows.Forms.Padding (5, 0, 5, 0);
            this.ui_cancelButton.Size = new System.Drawing.Size (82, 26);
            this.ui_cancelButton.TabIndex = 9;
            this.ui_cancelButton.Text = "&Cancel";
            this.ui_cancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler (this.Cancel_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add (this.ui_addQIButton);
            this.flowLayoutPanel1.Controls.Add (this.ui_addSubQueryButton);
            this.flowLayoutPanel1.Controls.Add (this.ui_removeLayerButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point (3, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size (177, 40);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ui_mainContextMenu
            // 
            this.ui_mainContextMenu.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.ui_openSettingsTSMI,
            this.ui_saveSettingsTSMI});
            this.ui_mainContextMenu.Name = "ui_mainContextMenu";
            this.ui_mainContextMenu.Size = new System.Drawing.Size (149, 48);
            // 
            // ui_openSettingsTSMI
            // 
            this.ui_openSettingsTSMI.Image = global::MapEnv.Properties.Resources.open_file_24;
            this.ui_openSettingsTSMI.Name = "ui_openSettingsTSMI";
            this.ui_openSettingsTSMI.Size = new System.Drawing.Size (148, 22);
            this.ui_openSettingsTSMI.Text = "Open Settings";
            this.ui_openSettingsTSMI.Click += new System.EventHandler (this.OpenSettings_Click);
            // 
            // ui_saveSettingsTSMI
            // 
            this.ui_saveSettingsTSMI.Image = global::MapEnv.Properties.Resources.save_24;
            this.ui_saveSettingsTSMI.Name = "ui_saveSettingsTSMI";
            this.ui_saveSettingsTSMI.Size = new System.Drawing.Size (148, 22);
            this.ui_saveSettingsTSMI.Text = "Save Settings";
            this.ui_saveSettingsTSMI.Click += new System.EventHandler (this.SaveSettings_Click);
            // 
            // QueryInfoBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size (736, 497);
            this.Controls.Add (this.ui_mainMenuButton);
            this.Controls.Add (this.flowLayoutPanel1);
            this.Controls.Add (this.ui_nameOrPropertyTB);
            this.Controls.Add (this.ui_cancelButton);
            this.Controls.Add (this.ui_nameOrPropLabel);
            this.Controls.Add (this.ui_okButton);
            this.Controls.Add (this.ui_tabControl);
            this.Controls.Add (this.ui_queryInfosTreeView);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QueryInfoBuilderForm";
            this.ShowInTaskbar = false;
            this.Text = "Query Layer Builder";
            this.ui_tabControl.ResumeLayout (false);
            this.ui_styleTabPage.ResumeLayout (false);
            this.ui_filterTabPage.ResumeLayout (false);
            this.flowLayoutPanel1.ResumeLayout (false);
            this.ui_mainContextMenu.ResumeLayout (false);
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.TreeView ui_queryInfosTreeView;
        private System.Windows.Forms.Button ui_addQIButton;
        private System.Windows.Forms.TabControl ui_tabControl;
        private System.Windows.Forms.TabPage ui_styleTabPage;
        private FeatureStyleControl ui_featureStyleControl;
        private System.Windows.Forms.TabPage ui_filterTabPage;
        private System.Windows.Forms.Button ui_addSubQueryButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button ui_removeLayerButton;
        private FilterControl ui_filterControl;
        private System.Windows.Forms.TextBox ui_nameOrPropertyTB;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Label ui_nameOrPropLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ui_mainMenuButton;
        private System.Windows.Forms.ContextMenuStrip ui_mainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ui_saveSettingsTSMI;
        private System.Windows.Forms.ToolStripMenuItem ui_openSettingsTSMI;
    }
}