namespace Aran.Controls
{
    partial class FilterControl
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
            this.ui_filterItemsFLP = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_newConditionButton = new System.Windows.Forms.Button();
            this.ui_newFilterButton = new System.Windows.Forms.Button();
            this.ui_filtersFLP = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_propSelControl = new Aran.Controls.PropertySelectorControl();
            this.SuspendLayout();
            // 
            // ui_filterItemsFLP
            // 
            this.ui_filterItemsFLP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_filterItemsFLP.AutoScroll = true;
            this.ui_filterItemsFLP.BackColor = System.Drawing.SystemColors.Window;
            this.ui_filterItemsFLP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_filterItemsFLP.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ui_filterItemsFLP.Location = new System.Drawing.Point(3, 107);
            this.ui_filterItemsFLP.Name = "ui_filterItemsFLP";
            this.ui_filterItemsFLP.Size = new System.Drawing.Size(494, 430);
            this.ui_filterItemsFLP.TabIndex = 3;
            this.ui_filterItemsFLP.WrapContents = false;
            this.ui_filterItemsFLP.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.FilterItemsFLP_ControlAdded);
            this.ui_filterItemsFLP.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.FilterItemsFLP_ControlRemoved);
            // 
            // ui_newConditionButton
            // 
            this.ui_newConditionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_newConditionButton.Enabled = false;
            this.ui_newConditionButton.Location = new System.Drawing.Point(392, 78);
            this.ui_newConditionButton.Name = "ui_newConditionButton";
            this.ui_newConditionButton.Size = new System.Drawing.Size(105, 23);
            this.ui_newConditionButton.TabIndex = 2;
            this.ui_newConditionButton.Text = "New Condition";
            this.ui_newConditionButton.UseVisualStyleBackColor = true;
            this.ui_newConditionButton.Click += new System.EventHandler(this.AddNewCondition_Click);
            // 
            // ui_newFilterButton
            // 
            this.ui_newFilterButton.Location = new System.Drawing.Point(3, 78);
            this.ui_newFilterButton.Name = "ui_newFilterButton";
            this.ui_newFilterButton.Size = new System.Drawing.Size(105, 23);
            this.ui_newFilterButton.TabIndex = 0;
            this.ui_newFilterButton.Text = "New Filter";
            this.ui_newFilterButton.UseVisualStyleBackColor = true;
            this.ui_newFilterButton.Click += new System.EventHandler(this.NewFilter_Click);
            // 
            // ui_filtersFLP
            // 
            this.ui_filtersFLP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_filtersFLP.AutoScroll = true;
            this.ui_filtersFLP.BackColor = System.Drawing.SystemColors.Window;
            this.ui_filtersFLP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_filtersFLP.Location = new System.Drawing.Point(3, 3);
            this.ui_filtersFLP.Name = "ui_filtersFLP";
            this.ui_filtersFLP.Size = new System.Drawing.Size(494, 69);
            this.ui_filtersFLP.TabIndex = 4;
            this.ui_filtersFLP.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.FiltersFLP_ControlAdded);
            this.ui_filtersFLP.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.FiltersFLP_ControlRemoved);
            // 
            // ui_propSelControl
            // 
            this.ui_propSelControl.ClassInfo = null;
            this.ui_propSelControl.Location = new System.Drawing.Point(133, 75);
            this.ui_propSelControl.Name = "ui_propSelControl";
            this.ui_propSelControl.Size = new System.Drawing.Size(70, 28);
            this.ui_propSelControl.TabIndex = 2;
            this.ui_propSelControl.Value = null;
            this.ui_propSelControl.Visible = false;
            this.ui_propSelControl.AfterSelect += new Aran.Controls.PropertySelectedEventHandler(this.PropSelControl_AfterSelect);
            this.ui_propSelControl.ValueChanged += new System.EventHandler(this.PropSelControl_ValueChanged);
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_filtersFLP);
            this.Controls.Add(this.ui_newFilterButton);
            this.Controls.Add(this.ui_propSelControl);
            this.Controls.Add(this.ui_newConditionButton);
            this.Controls.Add(this.ui_filterItemsFLP);
            this.Name = "FilterControl";
            this.Size = new System.Drawing.Size(500, 540);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel ui_filterItemsFLP;
        private System.Windows.Forms.Button ui_newConditionButton;
        private PropertySelectorControl ui_propSelControl;
        private System.Windows.Forms.Button ui_newFilterButton;
        private System.Windows.Forms.FlowLayoutPanel ui_filtersFLP;
    }
}
