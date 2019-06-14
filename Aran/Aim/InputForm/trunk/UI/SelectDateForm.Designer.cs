namespace Aran.Aim.InputForm
{
	partial class SelectDateForm
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
            System.Windows.Forms.Label label1;
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_airacCycleControl = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_errorLabel = new System.Windows.Forms.Label();
            this.ui_selDateLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(95, 13);
            label1.TabIndex = 1;
            label1.Text = "Desomission Date:";
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(100, 107);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 13;
            this.ui_okButton.Text = "&OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(181, 107);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 14;
            this.ui_cancelButton.Text = "&Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_airacCycleControl
            // 
            this.ui_airacCycleControl.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_airacCycleControl.Location = new System.Drawing.Point(12, 35);
            this.ui_airacCycleControl.Name = "ui_airacCycleControl";
            this.ui_airacCycleControl.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_airacCycleControl.Size = new System.Drawing.Size(241, 26);
            this.ui_airacCycleControl.TabIndex = 15;
            this.ui_airacCycleControl.Value = new System.DateTime(2016, 5, 26, 0, 0, 0, 0);
            this.ui_airacCycleControl.ValueChanged += new System.EventHandler(this.AiracCycleControl_ValueChanged);
            // 
            // ui_errorLabel
            // 
            this.ui_errorLabel.AutoSize = true;
            this.ui_errorLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_errorLabel.Location = new System.Drawing.Point(12, 82);
            this.ui_errorLabel.Name = "ui_errorLabel";
            this.ui_errorLabel.Size = new System.Drawing.Size(53, 13);
            this.ui_errorLabel.TabIndex = 16;
            this.ui_errorLabel.Text = "<Error....>";
            // 
            // ui_selDateLabel
            // 
            this.ui_selDateLabel.AutoSize = true;
            this.ui_selDateLabel.ForeColor = System.Drawing.Color.Blue;
            this.ui_selDateLabel.Location = new System.Drawing.Point(12, 64);
            this.ui_selDateLabel.Name = "ui_selDateLabel";
            this.ui_selDateLabel.Size = new System.Drawing.Size(53, 13);
            this.ui_selDateLabel.TabIndex = 17;
            this.ui_selDateLabel.Text = "<Error....>";
            // 
            // SelectDateForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(268, 148);
            this.Controls.Add(this.ui_selDateLabel);
            this.Controls.Add(this.ui_errorLabel);
            this.Controls.Add(this.ui_airacCycleControl);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decomission Feature";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Button ui_okButton;
		private System.Windows.Forms.Button ui_cancelButton;
        private Controls.Airac.AiracCycleControl ui_airacCycleControl;
        private System.Windows.Forms.Label ui_errorLabel;
        private System.Windows.Forms.Label ui_selDateLabel;

	}
}