namespace Aran.Aim.InputForm
{
    partial class EditCoordinateForm
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
			this.ui_coordinateControl = new ChoosePointNS.CoordinateControl();
			this.ui_okButton = new System.Windows.Forms.Button();
			this.ui_cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ui_coordinateControl
			// 
			this.ui_coordinateControl.Accuracy = 0;
			this.ui_coordinateControl.CaptionVisible = true;
			this.ui_coordinateControl.IsDD = true;
			this.ui_coordinateControl.Location = new System.Drawing.Point(12, 12);
			this.ui_coordinateControl.Name = "ui_coordinateControl";
			this.ui_coordinateControl.ReadOnly = false;
			this.ui_coordinateControl.Size = new System.Drawing.Size(274, 60);
			this.ui_coordinateControl.TabIndex = 0;
			// 
			// ui_okButton
			// 
			this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_okButton.Location = new System.Drawing.Point(114, 86);
			this.ui_okButton.Name = "ui_okButton";
			this.ui_okButton.Size = new System.Drawing.Size(75, 23);
			this.ui_okButton.TabIndex = 1;
			this.ui_okButton.Text = "OK";
			this.ui_okButton.UseVisualStyleBackColor = true;
			this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
			// 
			// ui_cancelButton
			// 
			this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ui_cancelButton.Location = new System.Drawing.Point(195, 86);
			this.ui_cancelButton.Name = "ui_cancelButton";
			this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
			this.ui_cancelButton.TabIndex = 2;
			this.ui_cancelButton.Text = "Cancel";
			this.ui_cancelButton.UseVisualStyleBackColor = true;
			// 
			// EditCoordinateForm
			// 
			this.AcceptButton = this.ui_okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ui_cancelButton;
			this.ClientSize = new System.Drawing.Size(282, 121);
			this.Controls.Add(this.ui_cancelButton);
			this.Controls.Add(this.ui_okButton);
			this.Controls.Add(this.ui_coordinateControl);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditCoordinateForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Coordinate";
			this.ResumeLayout(false);

        }

        #endregion

        private ChoosePointNS.CoordinateControl ui_coordinateControl;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
    }
}