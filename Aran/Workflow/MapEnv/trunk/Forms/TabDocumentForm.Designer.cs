namespace MapEnv
{
    partial class TabDocumentForm
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
            this.ui_tdiControl = new Aran.Controls.TDIControl();
            this.SuspendLayout();
            // 
            // ui_tdiControl
            // 
            this.ui_tdiControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_tdiControl.ButtonBarVisible = true;
            this.ui_tdiControl.CausesValidation = false;
            this.ui_tdiControl.CurrentTabDocument = null;
            this.ui_tdiControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_tdiControl.Location = new System.Drawing.Point(0, 0);
            this.ui_tdiControl.Name = "ui_tdiControl";
            this.ui_tdiControl.Size = new System.Drawing.Size(881, 441);
            this.ui_tdiControl.TabIndex = 0;
            this.ui_tdiControl.TabDocumentDockedClicked += new Aran.Controls.TabDocumentEventHandler(this.uiEvents_tdiControl_TabDocumentDockedClicked);
            this.ui_tdiControl.TabdocumentClosed += new Aran.Controls.TabDocumentEventHandler(this.TdiControl_TabdocumentClosed);
            this.ui_tdiControl.AllDocumentsClosed += new System.EventHandler(this.uiEvents_tdiControl_AllDocumentsClosed);
            // 
            // TabDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(881, 441);
            this.Controls.Add(this.ui_tdiControl);
            this.MaximizeBox = false;
            this.Name = "TabDocumentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Pages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TabDocumentForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Aran.Controls.TDIControl ui_tdiControl;
    }
}