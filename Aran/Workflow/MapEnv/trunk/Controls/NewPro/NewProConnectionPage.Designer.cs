namespace MapEnv
{
    partial class NewProConnectionPage
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
            if (disposing && (components != null)) {
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
            this.ui_dbProviderCont = new Aran.Controls.DbProviderControl();
            this.SuspendLayout();
            // 
            // ui_dbProviderCont
            // 
            this.ui_dbProviderCont.ConnectionType = Aran.AranEnvironment.ConnectionType.Aran;
            this.ui_dbProviderCont.Location = new System.Drawing.Point(8, 5);
            this.ui_dbProviderCont.Name = "ui_dbProviderCont";
            this.ui_dbProviderCont.Size = new System.Drawing.Size(385, 106);
            this.ui_dbProviderCont.TabIndex = 0;
            this.ui_dbProviderCont.VisibleDbTypePanel = false;
            // 
            // NewProConnectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_dbProviderCont);
            this.Name = "NewProConnectionPage";
            this.Size = new System.Drawing.Size(395, 155);
            this.ResumeLayout(false);

        }

        #endregion

        private Aran.Controls.DbProviderControl ui_dbProviderCont;


    }
}
