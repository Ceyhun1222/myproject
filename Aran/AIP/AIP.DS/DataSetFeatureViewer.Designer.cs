namespace AIP.DataSet
{
    partial class DataSetFeatureViewer
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
            this.radDataEntry1 = new Telerik.WinControls.UI.RadDataEntry();
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).BeginInit();
            this.radDataEntry1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radDataEntry1
            // 
            this.radDataEntry1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDataEntry1.Location = new System.Drawing.Point(0, 0);
            this.radDataEntry1.Name = "radDataEntry1";
            // 
            // radDataEntry1.PanelContainer
            // 
            this.radDataEntry1.PanelContainer.Size = new System.Drawing.Size(877, 675);
            this.radDataEntry1.Size = new System.Drawing.Size(879, 677);
            this.radDataEntry1.TabIndex = 0;
            this.radDataEntry1.Text = "radDataEntry1";
            this.radDataEntry1.ThemeName = "Office2013Light";
            this.radDataEntry1.ItemInitialized += new Telerik.WinControls.UI.ItemInitializedEventHandler(this.radDataEntry1_ItemInitialized);
            this.radDataEntry1.BindingCreated += new Telerik.WinControls.UI.BindingCreatedEventHandler(this.radDataEntry1_BindingCreated);
            // 
            // DataSetFeatureViewer
            // 
            this.ClientSize = new System.Drawing.Size(879, 677);
            this.Controls.Add(this.radDataEntry1);
            this.Name = "DataSetFeatureViewer";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DataSetFeatureViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).EndInit();
            this.radDataEntry1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadDataEntry radDataEntry1;
    }
}
