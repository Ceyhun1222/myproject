namespace Telerik.Examples.WinControls.Buttons.RepeatButton
{
    partial class Form1
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
            this.radRepeatButton3 = new Telerik.WinControls.UI.RadRepeatButton();
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).BeginInit();
            this.radPanelDemoHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelDemoHolder
            // 
            this.radPanelDemoHolder.Controls.Add(this.radRepeatButton3);
            this.radPanelDemoHolder.Controls.Add(this.radProgressBar1);
            this.radPanelDemoHolder.Location = new System.Drawing.Point(0, 0);
            this.radPanelDemoHolder.Size = new System.Drawing.Size(219, 124);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Location = new System.Drawing.Point(1023, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 735);
            // 
            // radRepeatButton3
            // 
            this.radRepeatButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radRepeatButton3.Location = new System.Drawing.Point(0, 0);
            this.radRepeatButton3.Name = "radRepeatButton3";
            this.radRepeatButton3.Size = new System.Drawing.Size(140, 23);
            this.radRepeatButton3.TabIndex = 6;
            this.radRepeatButton3.Text = "Press and Hold";
    
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.Location = new System.Drawing.Point(0, 41);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.SeparatorColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.radProgressBar1.SeparatorWidth = 4;
            this.radProgressBar1.Size = new System.Drawing.Size(140, 23);
            this.radProgressBar1.StepWidth = 13;
            this.radProgressBar1.TabIndex = 7;
            this.radProgressBar1.Text = "radProgressBar1";
            // 
            // Form1
            // 
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(2, 35, 2, 4);
            this.Size = new System.Drawing.Size(1170, 671);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).EndInit();
            this.radPanelDemoHolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRepeatButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadRepeatButton radRepeatButton3;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;

    }
}