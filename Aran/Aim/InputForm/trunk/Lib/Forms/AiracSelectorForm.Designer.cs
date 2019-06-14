namespace Aran.Aim.InputFormLib
{
    partial class AiracSelectorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button2;
            this.ui_beginValidDateTimePicker = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_featureCountLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ui_beginValidDateTimePicker
            // 
            this.ui_beginValidDateTimePicker.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_beginValidDateTimePicker.Location = new System.Drawing.Point(116, 12);
            this.ui_beginValidDateTimePicker.Name = "ui_beginValidDateTimePicker";
            this.ui_beginValidDateTimePicker.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_beginValidDateTimePicker.Size = new System.Drawing.Size(215, 25);
            this.ui_beginValidDateTimePicker.TabIndex = 22;
            this.ui_beginValidDateTimePicker.Value = new System.DateTime(2014, 9, 10, 15, 36, 57, 0);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 18);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(96, 13);
            label2.TabIndex = 21;
            label2.Text = "Start of Valid Time:";
            // 
            // ui_featureCountLabel
            // 
            this.ui_featureCountLabel.Location = new System.Drawing.Point(12, 56);
            this.ui_featureCountLabel.Name = "ui_featureCountLabel";
            this.ui_featureCountLabel.Size = new System.Drawing.Size(405, 53);
            this.ui_featureCountLabel.TabIndex = 23;
            this.ui_featureCountLabel.Text = "<Feature Count>";
            this.ui_featureCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(265, 112);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 24;
            button1.Text = "Yes";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button2.Location = new System.Drawing.Point(346, 112);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 25;
            button2.Text = "No";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AiracSelectorForm
            // 
            this.AcceptButton = button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = button2;
            this.ClientSize = new System.Drawing.Size(431, 145);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.Controls.Add(this.ui_featureCountLabel);
            this.Controls.Add(this.ui_beginValidDateTimePicker);
            this.Controls.Add(label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AiracSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AIM Features";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Airac.AiracCycleControl ui_beginValidDateTimePicker;
        private System.Windows.Forms.Label ui_featureCountLabel;
    }
}