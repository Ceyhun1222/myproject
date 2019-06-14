namespace Telerik.Examples.WinControls.Calendar.LayoutCustomization
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
            this.radCalendar1 = new Telerik.WinControls.UI.RadCalendar();
            this.radGroupDisplay = new Telerik.WinControls.UI.RadGroupBox();
            this.radRadio7Cols = new Telerik.WinControls.UI.RadRadioButton();
            this.radRadio21Cols = new Telerik.WinControls.UI.RadRadioButton();
            this.radRadio14Cols = new Telerik.WinControls.UI.RadRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupDisplay)).BeginInit();
            this.radGroupDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio7Cols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio21Cols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio14Cols)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupDisplay);
            this.settingsPanel.Location = new System.Drawing.Point(729, 1);
            this.settingsPanel.Size = new System.Drawing.Size(250, 867);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupDisplay, 0);
            // 
            // radCalendar1
            // 
            this.radCalendar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.radCalendar1.FocusedDate = new System.DateTime(2013, 3, 13, 0, 0, 0, 0);
            this.radCalendar1.ForeColor = System.Drawing.Color.Black;
            this.radCalendar1.Location = new System.Drawing.Point(0, 0);
            this.radCalendar1.Name = "radCalendar1";
            this.radCalendar1.Size = new System.Drawing.Size(215, 196);
            this.radCalendar1.TabIndex = 0;
            this.radCalendar1.Text = "radCalendar1";
            this.radCalendar1.ZoomFactor = 1.2F;
            // 
            // radGroupDisplay
            // 
            this.radGroupDisplay.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupDisplay.Controls.Add(this.radRadio7Cols);
            this.radGroupDisplay.Controls.Add(this.radRadio21Cols);
            this.radGroupDisplay.Controls.Add(this.radRadio14Cols);
            this.radGroupDisplay.HeaderText = "Display Options";
            this.radGroupDisplay.Location = new System.Drawing.Point(15, 6);
            this.radGroupDisplay.Name = "radGroupDisplay";
            // 
            // 
            // 
            this.radGroupDisplay.RootElement.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.radGroupDisplay.Size = new System.Drawing.Size(162, 116);
            this.radGroupDisplay.TabIndex = 2;
            this.radGroupDisplay.Text = "Display Options";
            // 
            // radRadio7Cols
            // 
            this.radRadio7Cols.Location = new System.Drawing.Point(17, 34);
            this.radRadio7Cols.Name = "radRadio7Cols";
            this.radRadio7Cols.Size = new System.Drawing.Size(123, 18);
            this.radRadio7Cols.TabIndex = 3;
            this.radRadio7Cols.Text = "7 columns by 6 rows";
          
            // 
            // radRadio21Cols
            // 
            this.radRadio21Cols.Location = new System.Drawing.Point(17, 78);
            this.radRadio21Cols.Name = "radRadio21Cols";
            this.radRadio21Cols.Size = new System.Drawing.Size(129, 18);
            this.radRadio21Cols.TabIndex = 3;
            this.radRadio21Cols.Text = "21 coulmns by 2 rows";
          
            // 
            // radRadio14Cols
            // 
            this.radRadio14Cols.Location = new System.Drawing.Point(17, 56);
            this.radRadio14Cols.Name = "radRadio14Cols";
            this.radRadio14Cols.Size = new System.Drawing.Size(129, 18);
            this.radRadio14Cols.TabIndex = 3;
            this.radRadio14Cols.Text = "14 coulmns by 3 rows";

            // 
            // Form1
            // 
            this.Controls.Add(this.radCalendar1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1170, 754);
            this.Controls.SetChildIndex(this.radCalendar1, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupDisplay)).EndInit();
            this.radGroupDisplay.ResumeLayout(false);
            this.radGroupDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio7Cols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio21Cols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadio14Cols)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private Telerik.WinControls.UI.RadCalendar radCalendar1;
        private Telerik.WinControls.UI.RadGroupBox radGroupDisplay;
        private Telerik.WinControls.UI.RadRadioButton radRadio7Cols;
        private Telerik.WinControls.UI.RadRadioButton radRadio14Cols;
        private Telerik.WinControls.UI.RadRadioButton radRadio21Cols;


	}
}