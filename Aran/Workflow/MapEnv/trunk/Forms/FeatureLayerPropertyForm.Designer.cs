using Aran.Controls;
namespace MapEnv
{
    partial class FeatureLayerPropertyForm
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
            this.ui_mainTabControl = new System.Windows.Forms.TabControl();
            this.ui_styleTabPage = new System.Windows.Forms.TabPage();
            this.ui_filterTabPage = new System.Windows.Forms.TabPage();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_layerNameLabel = new System.Windows.Forms.Label();
            this.ui_layerNameTB = new System.Windows.Forms.TextBox();
            this.ui_filterControl = new Aran.Controls.FilterControl();
            this.ui_styleControl = new MapEnv.FeatureStyleControl();
            this.ui_mainTabControl.SuspendLayout();
            this.ui_styleTabPage.SuspendLayout();
            this.ui_filterTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_mainTabControl
            // 
            this.ui_mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainTabControl.Controls.Add(this.ui_styleTabPage);
            this.ui_mainTabControl.Controls.Add(this.ui_filterTabPage);
            this.ui_mainTabControl.Location = new System.Drawing.Point(6, 6);
            this.ui_mainTabControl.Name = "ui_mainTabControl";
            this.ui_mainTabControl.SelectedIndex = 0;
            this.ui_mainTabControl.Size = new System.Drawing.Size(644, 509);
            this.ui_mainTabControl.TabIndex = 0;
            // 
            // ui_styleTabPage
            // 
            this.ui_styleTabPage.Controls.Add(this.ui_styleControl);
            this.ui_styleTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_styleTabPage.Name = "ui_styleTabPage";
            this.ui_styleTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_styleTabPage.Size = new System.Drawing.Size(636, 483);
            this.ui_styleTabPage.TabIndex = 0;
            this.ui_styleTabPage.Text = "Style";
            // 
            // ui_filterTabPage
            // 
            this.ui_filterTabPage.Controls.Add(this.ui_filterControl);
            this.ui_filterTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_filterTabPage.Name = "ui_filterTabPage";
            this.ui_filterTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_filterTabPage.Size = new System.Drawing.Size(674, 471);
            this.ui_filterTabPage.TabIndex = 1;
            this.ui_filterTabPage.Text = "Filter";
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(483, 521);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 25);
            this.ui_okButton.TabIndex = 0;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.Location = new System.Drawing.Point(564, 521);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 25);
            this.ui_cancelButton.TabIndex = 1;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_layerNameLabel
            // 
            this.ui_layerNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_layerNameLabel.AutoSize = true;
            this.ui_layerNameLabel.Location = new System.Drawing.Point(3, 527);
            this.ui_layerNameLabel.Name = "ui_layerNameLabel";
            this.ui_layerNameLabel.Size = new System.Drawing.Size(67, 13);
            this.ui_layerNameLabel.TabIndex = 12;
            this.ui_layerNameLabel.Text = "Layer Name:";
            // 
            // ui_layerNameTB
            // 
            this.ui_layerNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_layerNameTB.Location = new System.Drawing.Point(76, 524);
            this.ui_layerNameTB.Name = "ui_layerNameTB";
            this.ui_layerNameTB.Size = new System.Drawing.Size(144, 20);
            this.ui_layerNameTB.TabIndex = 13;
            // 
            // ui_filterControl
            // 
            this.ui_filterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_filterControl.LoadFeatureListByDependHandler = null;
            this.ui_filterControl.Location = new System.Drawing.Point(3, 3);
            this.ui_filterControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_filterControl.Name = "ui_filterControl";
            this.ui_filterControl.Size = new System.Drawing.Size(668, 465);
            this.ui_filterControl.TabIndex = 14;
            // 
            // ui_styleControl
            // 
            this.ui_styleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_styleControl.Location = new System.Drawing.Point(3, 3);
            this.ui_styleControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_styleControl.Name = "ui_styleControl";
            this.ui_styleControl.Size = new System.Drawing.Size(630, 477);
            this.ui_styleControl.TabIndex = 1;
            // 
            // FeatureLayerPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 557);
            this.Controls.Add(this.ui_layerNameLabel);
            this.Controls.Add(this.ui_layerNameTB);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_mainTabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureLayerPropertyForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AIM Layer Properties";
            this.ui_mainTabControl.ResumeLayout(false);
            this.ui_styleTabPage.ResumeLayout(false);
            this.ui_filterTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl ui_mainTabControl;
        private System.Windows.Forms.TabPage ui_styleTabPage;
        private System.Windows.Forms.TabPage ui_filterTabPage;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Label ui_layerNameLabel;
        private System.Windows.Forms.TextBox ui_layerNameTB;
        private FeatureStyleControl ui_styleControl;
        private FilterControl ui_filterControl;
    }
}
