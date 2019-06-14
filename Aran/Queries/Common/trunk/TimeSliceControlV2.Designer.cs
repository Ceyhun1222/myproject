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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            this.ui_endOfValidPanel = new System.Windows.Forms.Panel();
            this.ui_endValidTB = new System.Windows.Forms.TextBox();
            this.ui_beginOfLife = new System.Windows.Forms.Panel();
            this.ui_beginLifeTB = new System.Windows.Forms.TextBox();
            this.ui_endOfLife = new System.Windows.Forms.Panel();
            this.ui_endLifeTB = new System.Windows.Forms.TextBox();
            this.ui_interpretationCB = new System.Windows.Forms.ComboBox();
            this.ui_beginValidAiracCycle = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_seqNumTB = new System.Windows.Forms.TextBox();
            this.ui_corNumTB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            flowLayoutPanel1.SuspendLayout();
            this.ui_endOfValidPanel.SuspendLayout();
            this.ui_beginOfLife.SuspendLayout();
            this.ui_endOfLife.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 13);
            label1.TabIndex = 0;
            label1.Text = "Interpretation:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 46);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(99, 13);
            label2.TabIndex = 2;
            label2.Text = "Sequence Number:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 82);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 13);
            label3.TabIndex = 3;
            label3.Text = "Corrention Number:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 118);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(101, 13);
            label4.TabIndex = 4;
            label4.Text = "Begin of Valid Time:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(this.ui_endOfValidPanel);
            flowLayoutPanel1.Controls.Add(this.ui_beginOfLife);
            flowLayoutPanel1.Controls.Add(this.ui_endOfLife);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 139);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(341, 120);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // ui_endOfValidPanel
            // 
            this.ui_endOfValidPanel.Controls.Add(this.ui_endValidTB);
            this.ui_endOfValidPanel.Controls.Add(label5);
            this.ui_endOfValidPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_endOfValidPanel.Name = "ui_endOfValidPanel";
            this.ui_endOfValidPanel.Size = new System.Drawing.Size(335, 34);
            this.ui_endOfValidPanel.TabIndex = 0;
            this.ui_endOfValidPanel.Visible = false;
            // 
            // ui_endValidTB
            // 
            this.ui_endValidTB.Location = new System.Drawing.Point(131, 6);
            this.ui_endValidTB.Name = "ui_endValidTB";
            this.ui_endValidTB.ReadOnly = true;
            this.ui_endValidTB.Size = new System.Drawing.Size(196, 20);
            this.ui_endValidTB.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 9);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(93, 13);
            label5.TabIndex = 5;
            label5.Text = "End of Valid Time:";
            // 
            // ui_beginOfLife
            // 
            this.ui_beginOfLife.Controls.Add(this.ui_beginLifeTB);
            this.ui_beginOfLife.Controls.Add(label6);
            this.ui_beginOfLife.Location = new System.Drawing.Point(3, 43);
            this.ui_beginOfLife.Name = "ui_beginOfLife";
            this.ui_beginOfLife.Size = new System.Drawing.Size(335, 34);
            this.ui_beginOfLife.TabIndex = 1;
            // 
            // ui_beginLifeTB
            // 
            this.ui_beginLifeTB.Location = new System.Drawing.Point(131, 8);
            this.ui_beginLifeTB.Name = "ui_beginLifeTB";
            this.ui_beginLifeTB.ReadOnly = true;
            this.ui_beginLifeTB.Size = new System.Drawing.Size(196, 20);
            this.ui_beginLifeTB.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 11);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(88, 13);
            label6.TabIndex = 5;
            label6.Text = "Begin of Lifetime:";
            // 
            // ui_endOfLife
            // 
            this.ui_endOfLife.Controls.Add(this.ui_endLifeTB);
            this.ui_endOfLife.Controls.Add(label7);
            this.ui_endOfLife.Location = new System.Drawing.Point(3, 83);
            this.ui_endOfLife.Name = "ui_endOfLife";
            this.ui_endOfLife.Size = new System.Drawing.Size(335, 34);
            this.ui_endOfLife.TabIndex = 2;
            this.ui_endOfLife.Visible = false;
            // 
            // ui_endLifeTB
            // 
            this.ui_endLifeTB.Location = new System.Drawing.Point(131, 6);
            this.ui_endLifeTB.Name = "ui_endLifeTB";
            this.ui_endLifeTB.ReadOnly = true;
            this.ui_endLifeTB.Size = new System.Drawing.Size(196, 20);
            this.ui_endLifeTB.TabIndex = 7;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(6, 9);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(80, 13);
            label7.TabIndex = 5;
            label7.Text = "End of Lifetime:";
            // 
            // ui_interpretationCB
            // 
            this.ui_interpretationCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_interpretationCB.FormattingEnabled = true;
            this.ui_interpretationCB.Location = new System.Drawing.Point(129, 8);
            this.ui_interpretationCB.Name = "ui_interpretationCB";
            this.ui_interpretationCB.Size = new System.Drawing.Size(207, 21);
            this.ui_interpretationCB.TabIndex = 1;
            // 
            // ui_beginValidAiracCycle
            // 
            this.ui_beginValidAiracCycle.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_beginValidAiracCycle.Location = new System.Drawing.Point(129, 112);
            this.ui_beginValidAiracCycle.Name = "ui_beginValidAiracCycle";
            this.ui_beginValidAiracCycle.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_beginValidAiracCycle.Size = new System.Drawing.Size(207, 21);
            this.ui_beginValidAiracCycle.TabIndex = 11;
            this.ui_beginValidAiracCycle.Value = new System.DateTime(2014, 10, 16, 0, 0, 0, 0);
            this.ui_beginValidAiracCycle.ValueChanged += new System.EventHandler(this.BeginValidAiracCycle_ValueChanged);
            // 
            // ui_seqNumTB
            // 
            this.ui_seqNumTB.Location = new System.Drawing.Point(129, 43);
            this.ui_seqNumTB.Name = "ui_seqNumTB";
            this.ui_seqNumTB.ReadOnly = true;
            this.ui_seqNumTB.Size = new System.Drawing.Size(73, 20);
            this.ui_seqNumTB.TabIndex = 15;
            // 
            // ui_corNumTB
            // 
            this.ui_corNumTB.Location = new System.Drawing.Point(129, 79);
            this.ui_corNumTB.Name = "ui_corNumTB";
            this.ui_corNumTB.ReadOnly = true;
            this.ui_corNumTB.Size = new System.Drawing.Size(73, 20);
            this.ui_corNumTB.TabIndex = 16;
            // 
            // TimeSliceControlV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.ui_corNumTB);
            this.Controls.Add(this.ui_seqNumTB);
            this.Controls.Add(flowLayoutPanel1);
            this.Controls.Add(this.ui_beginValidAiracCycle);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_interpretationCB);
            this.Controls.Add(label1);
            this.Name = "TimeSliceControlV2";
            this.Size = new System.Drawing.Size(351, 263);
            flowLayoutPanel1.ResumeLayout(false);
            this.ui_endOfValidPanel.ResumeLayout(false);
            this.ui_endOfValidPanel.PerformLayout();
            this.ui_beginOfLife.ResumeLayout(false);
            this.ui_beginOfLife.PerformLayout();
            this.ui_endOfLife.ResumeLayout(false);
            this.ui_endOfLife.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_interpretationCB;
        private Controls.Airac.AiracCycleControl ui_beginValidAiracCycle;
        private System.Windows.Forms.Panel ui_endOfValidPanel;
        private System.Windows.Forms.Panel ui_beginOfLife;
        private System.Windows.Forms.Panel ui_endOfLife;
        private System.Windows.Forms.TextBox ui_endValidTB;
        private System.Windows.Forms.TextBox ui_beginLifeTB;
        private System.Windows.Forms.TextBox ui_endLifeTB;
        private System.Windows.Forms.TextBox ui_seqNumTB;
        private System.Windows.Forms.TextBox ui_corNumTB;
    }
}
