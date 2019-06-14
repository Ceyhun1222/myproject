namespace Holding.Forms
{
    partial class frmDrawTest
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDrawTest));
			this.ckbShablon = new System.Windows.Forms.CheckBox();
			this.holdingGeometryBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.ckbToleranceArea = new System.Windows.Forms.CheckBox();
			this.ckbSector = new System.Windows.Forms.CheckBox();
			this.Buffers = new System.Windows.Forms.CheckBox();
			this.ckbTrack = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.holdingGeometryBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// ckbShablon
			// 
			this.ckbShablon.AutoSize = true;
			this.ckbShablon.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.holdingGeometryBindingSource, "ShablonIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ckbShablon.Location = new System.Drawing.Point(12, 78);
			this.ckbShablon.Name = "ckbShablon";
			this.ckbShablon.Size = new System.Drawing.Size(70, 17);
			this.ckbShablon.TabIndex = 0;
			this.ckbShablon.Text = "Template";
			this.ckbShablon.UseVisualStyleBackColor = true;
			this.ckbShablon.CheckedChanged += new System.EventHandler(this.ckbShablon_CheckedChanged);
			// 
			// holdingGeometryBindingSource
			// 
			this.holdingGeometryBindingSource.DataSource = typeof(Holding.HoldingGeometry);
			// 
			// ckbToleranceArea
			// 
			this.ckbToleranceArea.AutoSize = true;
			this.ckbToleranceArea.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.holdingGeometryBindingSource, "ToleranceIsChecked", true));
			this.ckbToleranceArea.Location = new System.Drawing.Point(12, 45);
			this.ckbToleranceArea.Name = "ckbToleranceArea";
			this.ckbToleranceArea.Size = new System.Drawing.Size(99, 17);
			this.ckbToleranceArea.TabIndex = 1;
			this.ckbToleranceArea.Text = "Tolerance Area";
			this.ckbToleranceArea.UseVisualStyleBackColor = true;
			this.ckbToleranceArea.CheckedChanged += new System.EventHandler(this.ckbToleranceArea_CheckedChanged);
			// 
			// ckbSector
			// 
			this.ckbSector.AutoSize = true;
			this.ckbSector.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.holdingGeometryBindingSource, "SectorProtectionIsChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ckbSector.Location = new System.Drawing.Point(146, 12);
			this.ckbSector.Name = "ckbSector";
			this.ckbSector.Size = new System.Drawing.Size(74, 17);
			this.ckbSector.TabIndex = 2;
			this.ckbSector.Text = "Entry area";
			this.ckbSector.UseVisualStyleBackColor = true;
			this.ckbSector.CheckedChanged += new System.EventHandler(this.ckbSector_CheckedChanged);
			// 
			// Buffers
			// 
			this.Buffers.AutoSize = true;
			this.Buffers.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.holdingGeometryBindingSource, "BufferIsChecked", true));
			this.Buffers.Location = new System.Drawing.Point(146, 45);
			this.Buffers.Name = "Buffers";
			this.Buffers.Size = new System.Drawing.Size(59, 17);
			this.Buffers.TabIndex = 7;
			this.Buffers.Text = "Buffers";
			this.Buffers.UseVisualStyleBackColor = true;
			this.Buffers.CheckedChanged += new System.EventHandler(this.Buffers_CheckedChanged);
			// 
			// ckbTrack
			// 
			this.ckbTrack.AutoSize = true;
			this.ckbTrack.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.holdingGeometryBindingSource, "HoldingTrackIsChecked", true));
			this.ckbTrack.Location = new System.Drawing.Point(12, 12);
			this.ckbTrack.Name = "ckbTrack";
			this.ckbTrack.Size = new System.Drawing.Size(114, 17);
			this.ckbTrack.TabIndex = 8;
			this.ckbTrack.Text = "Nominal Trajectory";
			this.ckbTrack.UseVisualStyleBackColor = true;
			this.ckbTrack.CheckedChanged += new System.EventHandler(this.ckbTrack_CheckedChanged);
			// 
			// frmDrawTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(239, 107);
			this.Controls.Add(this.ckbTrack);
			this.Controls.Add(this.Buffers);
			this.Controls.Add(this.ckbSector);
			this.Controls.Add(this.ckbToleranceArea);
			this.Controls.Add(this.ckbShablon);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmDrawTest";
			this.Text = "Draw Settings";
			this.Load += new System.EventHandler(this.frmDrawTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.holdingGeometryBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ckbShablon;
        private System.Windows.Forms.CheckBox ckbToleranceArea;
        private System.Windows.Forms.CheckBox ckbSector;
        private System.Windows.Forms.CheckBox Buffers;
        private System.Windows.Forms.BindingSource holdingGeometryBindingSource;
        private System.Windows.Forms.CheckBox ckbTrack;
    }
}