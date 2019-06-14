namespace Aran.Queries.Common
{
    partial class TimeSliceControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.mainGB = new System.Windows.Forms.GroupBox();
            this.detailPpanel = new System.Windows.Forms.Panel();
            this.pcInterpretation = new Aran.Queries.Common.PropControl();
            this.pcSequenceNumber = new Aran.Queries.Common.PropControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pcStartOfLifeTime = new Aran.Queries.Common.PropControl();
            this.pcEndOfLifeTime = new Aran.Queries.Common.PropControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pcStartOfValidTime = new Aran.Queries.Common.PropControl();
            this.pcEndOfValidTime = new Aran.Queries.Common.PropControl();
            this.showExpanderChb = new System.Windows.Forms.CheckBox();
            this.pcCorrectionNumber = new Aran.Queries.Common.PropControl();
            this.mainGB.SuspendLayout();
            this.detailPpanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGB
            // 
            this.mainGB.Controls.Add(this.detailPpanel);
            this.mainGB.Location = new System.Drawing.Point(-5, -33);
            this.mainGB.Name = "mainGB";
            this.mainGB.Size = new System.Drawing.Size(392, 273);
            this.mainGB.TabIndex = 1;
            this.mainGB.TabStop = false;
            this.mainGB.SizeChanged += new System.EventHandler(this.mainGB_SizeChanged);
            // 
            // detailPpanel
            // 
            this.detailPpanel.Controls.Add(this.pcInterpretation);
            this.detailPpanel.Controls.Add(this.pcSequenceNumber);
            this.detailPpanel.Controls.Add(this.groupBox2);
            this.detailPpanel.Controls.Add(this.groupBox1);
            this.detailPpanel.Controls.Add(this.pcCorrectionNumber);
            this.detailPpanel.Location = new System.Drawing.Point(2, 32);
            this.detailPpanel.Name = "detailPpanel";
            this.detailPpanel.Size = new System.Drawing.Size(384, 246);
            this.detailPpanel.TabIndex = 8;
            this.detailPpanel.Visible = false;
            // 
            // pcInterpretation
            // 
            this.pcInterpretation.ControlLeft = 138;
            this.pcInterpretation.ControlWidth = 216;
            this.pcInterpretation.IsNullable = false;
            this.pcInterpretation.Location = new System.Drawing.Point(12, 1);
            this.pcInterpretation.Name = "pcInterpretation";
            this.pcInterpretation.PropertyTag = null;
            this.pcInterpretation.ReadOnly = false;
            this.pcInterpretation.Size = new System.Drawing.Size(368, 28);
            this.pcInterpretation.TabIndex = 2;
            // 
            // pcSequenceNumber
            // 
            this.pcSequenceNumber.ControlLeft = 138;
            this.pcSequenceNumber.ControlWidth = 216;
            this.pcSequenceNumber.IsNullable = false;
            this.pcSequenceNumber.Location = new System.Drawing.Point(12, 30);
            this.pcSequenceNumber.Name = "pcSequenceNumber";
            this.pcSequenceNumber.PropertyTag = null;
            this.pcSequenceNumber.ReadOnly = true;
            this.pcSequenceNumber.Size = new System.Drawing.Size(368, 28);
            this.pcSequenceNumber.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pcStartOfLifeTime);
            this.groupBox2.Controls.Add(this.pcEndOfLifeTime);
            this.groupBox2.Location = new System.Drawing.Point(7, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 75);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Feature Lifetime";
            // 
            // pcStartOfLifeTime
            // 
            this.pcStartOfLifeTime.ControlLeft = 105;
            this.pcStartOfLifeTime.ControlWidth = 140;
            this.pcStartOfLifeTime.IsNullable = false;
            this.pcStartOfLifeTime.Location = new System.Drawing.Point(40, 14);
            this.pcStartOfLifeTime.Name = "pcStartOfLifeTime";
            this.pcStartOfLifeTime.PropertyTag = null;
            this.pcStartOfLifeTime.ReadOnly = false;
            this.pcStartOfLifeTime.Size = new System.Drawing.Size(327, 28);
            this.pcStartOfLifeTime.TabIndex = 5;
            // 
            // pcEndOfLifeTime
            // 
            this.pcEndOfLifeTime.ControlLeft = 105;
            this.pcEndOfLifeTime.ControlWidth = 140;
            this.pcEndOfLifeTime.IsNullable = true;
            this.pcEndOfLifeTime.Location = new System.Drawing.Point(40, 43);
            this.pcEndOfLifeTime.Name = "pcEndOfLifeTime";
            this.pcEndOfLifeTime.PropertyTag = null;
            this.pcEndOfLifeTime.ReadOnly = false;
            this.pcEndOfLifeTime.Size = new System.Drawing.Size(327, 28);
            this.pcEndOfLifeTime.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pcStartOfValidTime);
            this.groupBox1.Controls.Add(this.pcEndOfValidTime);
            this.groupBox1.Location = new System.Drawing.Point(6, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 74);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Valid Time";
            // 
            // pcStartOfValidTime
            // 
            this.pcStartOfValidTime.ControlLeft = 105;
            this.pcStartOfValidTime.ControlWidth = 140;
            this.pcStartOfValidTime.IsNullable = false;
            this.pcStartOfValidTime.Location = new System.Drawing.Point(40, 12);
            this.pcStartOfValidTime.Name = "pcStartOfValidTime";
            this.pcStartOfValidTime.PropertyTag = null;
            this.pcStartOfValidTime.ReadOnly = false;
            this.pcStartOfValidTime.Size = new System.Drawing.Size(327, 28);
            this.pcStartOfValidTime.TabIndex = 5;
            // 
            // pcEndOfValidTime
            // 
            this.pcEndOfValidTime.ControlLeft = 105;
            this.pcEndOfValidTime.ControlWidth = 140;
            this.pcEndOfValidTime.IsNullable = true;
            this.pcEndOfValidTime.Location = new System.Drawing.Point(40, 42);
            this.pcEndOfValidTime.Name = "pcEndOfValidTime";
            this.pcEndOfValidTime.PropertyTag = null;
            this.pcEndOfValidTime.ReadOnly = false;
            this.pcEndOfValidTime.Size = new System.Drawing.Size(327, 28);
            this.pcEndOfValidTime.TabIndex = 6;
            // 
            // showExpanderChb
            // 
            this.showExpanderChb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.showExpanderChb.Checked = true;
            this.showExpanderChb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showExpanderChb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.showExpanderChb.Location = new System.Drawing.Point(9, 8);
            this.showExpanderChb.Name = "showExpanderChb";
            this.showExpanderChb.Size = new System.Drawing.Size(382, 33);
            this.showExpanderChb.TabIndex = 0;
            this.showExpanderChb.Text = "showExpanderChb";
            this.showExpanderChb.UseVisualStyleBackColor = false;
            this.showExpanderChb.Visible = false;
            this.showExpanderChb.CheckedChanged += new System.EventHandler(this.showExpanderChb_CheckedChanged);
            this.showExpanderChb.Paint += new System.Windows.Forms.PaintEventHandler(this.showExpanderChb_Paint);
            // 
            // pcCorrectionNumber
            // 
            this.pcCorrectionNumber.ControlLeft = 138;
            this.pcCorrectionNumber.ControlWidth = 216;
            this.pcCorrectionNumber.IsNullable = false;
            this.pcCorrectionNumber.Location = new System.Drawing.Point(12, 60);
            this.pcCorrectionNumber.Name = "pcCorrectionNumber";
            this.pcCorrectionNumber.PropertyTag = null;
            this.pcCorrectionNumber.ReadOnly = true;
            this.pcCorrectionNumber.Size = new System.Drawing.Size(368, 28);
            this.pcCorrectionNumber.TabIndex = 4;
            // 
            // TimeSliceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.showExpanderChb);
            this.Controls.Add(this.mainGB);
            this.Name = "TimeSliceControl";
            this.Size = new System.Drawing.Size(379, 244);
            this.Load += new System.EventHandler(this.TimeSliceControl_Load);
            this.mainGB.ResumeLayout(false);
            this.detailPpanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mainGB;
        private System.Windows.Forms.CheckBox showExpanderChb;
        private PropControl pcInterpretation;
        private PropControl pcSequenceNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private PropControl pcStartOfValidTime;
        private PropControl pcEndOfValidTime;
        private System.Windows.Forms.Panel detailPpanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private PropControl pcEndOfLifeTime;
        private PropControl pcStartOfLifeTime;
        private PropControl pcCorrectionNumber;
    }
}
