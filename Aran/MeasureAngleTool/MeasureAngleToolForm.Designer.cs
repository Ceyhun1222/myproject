namespace Aran.AimEnvironment.Tools
{
    partial class MeasureAngleToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureAngleToolForm));
            this.measureAngleToolstrip = new System.Windows.Forms.ToolStrip();
            this.measureAngelToolstripButton = new System.Windows.Forms.ToolStripButton();
            this.measureAngleToolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // measureAngleToolstrip
            // 
            this.measureAngleToolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.measureAngleToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.measureAngelToolstripButton});
            this.measureAngleToolstrip.Location = new System.Drawing.Point(0, 0);
            this.measureAngleToolstrip.Name = "measureAngleToolstrip";
            this.measureAngleToolstrip.Size = new System.Drawing.Size(589, 27);
            this.measureAngleToolstrip.TabIndex = 0;
            // 
            // measureAngelToolstripButton
            // 
            this.measureAngelToolstripButton.CheckOnClick = true;
            this.measureAngelToolstripButton.Image = ((System.Drawing.Image)(resources.GetObject("measureAngelToolstripButton.Image")));
            this.measureAngelToolstripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.measureAngelToolstripButton.Name = "measureAngelToolstripButton";
            this.measureAngelToolstripButton.Size = new System.Drawing.Size(108, 24);
            this.measureAngelToolstripButton.Text = "Measure Tools";
            this.measureAngelToolstripButton.Click += new System.EventHandler(this.measureAngleToolStripButtonClick);
            // 
            // MeasureAngleToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 148);
            this.Controls.Add(this.measureAngleToolstrip);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MeasureAngleToolForm";
            this.Text = "ToolbarForm";
            this.measureAngleToolstrip.ResumeLayout(false);
            this.measureAngleToolstrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip measureAngleToolstrip;
        private System.Windows.Forms.ToolStripButton measureAngelToolstripButton;
    }
}