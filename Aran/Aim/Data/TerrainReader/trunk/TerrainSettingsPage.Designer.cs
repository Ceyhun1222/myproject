namespace Aran.Aim.Data.TerrainReader
{
    partial class TerrainSettingsPage
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
            System.Windows.Forms.Label label1;
            this.ui_terrainLayerCB = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(131, 13);
            label1.TabIndex = 0;
            label1.Text = "Terrain Esri Feature Layer:";
            // 
            // ui_terrainLayerCB
            // 
            this.ui_terrainLayerCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_terrainLayerCB.FormattingEnabled = true;
            this.ui_terrainLayerCB.Location = new System.Drawing.Point(14, 33);
            this.ui_terrainLayerCB.Name = "ui_terrainLayerCB";
            this.ui_terrainLayerCB.Size = new System.Drawing.Size(325, 21);
            this.ui_terrainLayerCB.TabIndex = 1;
            // 
            // TerrainSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_terrainLayerCB);
            this.Controls.Add(label1);
            this.Name = "TerrainSettingsPage";
            this.Size = new System.Drawing.Size(410, 181);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_terrainLayerCB;
    }
}
