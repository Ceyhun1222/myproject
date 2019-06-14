namespace Aran.Omega.SettingsUI
{
	partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.settingsControl1 = new Aran.Omega.SettingsUI.SettingsControl();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.ui_mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.ui_mainTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.elementHost2);
            this.tabPage1.Controls.Add(this.elementHost1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // elementHost2
            // 
            resources.ApplyResources(this.elementHost2, "elementHost2");
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Child = this.settingsControl1;
            // 
            // elementHost1
            // 
            resources.ApplyResources(this.elementHost1, "elementHost1");
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // ui_mainTabControl
            // 
            resources.ApplyResources(this.ui_mainTabControl, "ui_mainTabControl");
            this.ui_mainTabControl.Controls.Add(this.tabPage1);
            this.ui_mainTabControl.Multiline = true;
            this.ui_mainTabControl.Name = "ui_mainTabControl";
            this.ui_mainTabControl.SelectedIndex = 0;
            // 
            // SettingsForm
            // 
            this.Controls.Add(this.ui_mainTabControl);
            this.Name = "SettingsForm";
            resources.ApplyResources(this, "$this");
            this.tabPage1.ResumeLayout(false);
            this.ui_mainTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl ui_mainTabControl;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private SettingsControl settingsControl1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;

    }
}