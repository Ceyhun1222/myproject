namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class MF_Page4
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_addNewStep = new System.Windows.Forms.Button();
            this.btn_removeLastStep = new System.Windows.Forms.Button();
            this.trackStepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chkBox_isFinalSegmentStep = new System.Windows.Forms.CheckBox();
            this.btn_LoadFromFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flwLytPnl_TrackSegments = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackStepBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Segments";
            // 
            // btn_addNewStep
            // 
            this.btn_addNewStep.Location = new System.Drawing.Point(425, 317);
            this.btn_addNewStep.Name = "btn_addNewStep";
            this.btn_addNewStep.Size = new System.Drawing.Size(122, 23);
            this.btn_addNewStep.TabIndex = 1;
            this.btn_addNewStep.Text = "Add new segment";
            this.btn_addNewStep.UseVisualStyleBackColor = true;
            this.btn_addNewStep.Click += new System.EventHandler(this.btn_addNewStep_Click);
            // 
            // btn_removeLastStep
            // 
            this.btn_removeLastStep.Enabled = false;
            this.btn_removeLastStep.Location = new System.Drawing.Point(425, 347);
            this.btn_removeLastStep.Name = "btn_removeLastStep";
            this.btn_removeLastStep.Size = new System.Drawing.Size(122, 23);
            this.btn_removeLastStep.TabIndex = 2;
            this.btn_removeLastStep.Text = "Remove last segment";
            this.btn_removeLastStep.UseVisualStyleBackColor = true;
            this.btn_removeLastStep.Click += new System.EventHandler(this.btn_removeLastStep_Click);
            // 
            // chkBox_isFinalSegmentStep
            // 
            this.chkBox_isFinalSegmentStep.AutoSize = true;
            this.chkBox_isFinalSegmentStep.Location = new System.Drawing.Point(326, 321);
            this.chkBox_isFinalSegmentStep.Name = "chkBox_isFinalSegmentStep";
            this.chkBox_isFinalSegmentStep.Size = new System.Drawing.Size(93, 17);
            this.chkBox_isFinalSegmentStep.TabIndex = 4;
            this.chkBox_isFinalSegmentStep.Text = "Final Segment";
            this.chkBox_isFinalSegmentStep.UseVisualStyleBackColor = true;
            // 
            // btn_LoadFromFile
            // 
            this.btn_LoadFromFile.Location = new System.Drawing.Point(9, 317);
            this.btn_LoadFromFile.Name = "btn_LoadFromFile";
            this.btn_LoadFromFile.Size = new System.Drawing.Size(114, 23);
            this.btn_LoadFromFile.TabIndex = 5;
            this.btn_LoadFromFile.Text = "Load from file";
            this.btn_LoadFromFile.UseVisualStyleBackColor = true;
            this.btn_LoadFromFile.Click += new System.EventHandler(this.btn_LoadFromFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Track Construction";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 30);
            this.panel1.TabIndex = 511;
            // 
            // flwLytPnl_TrackSegments
            // 
            this.flwLytPnl_TrackSegments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flwLytPnl_TrackSegments.AutoScroll = true;
            this.flwLytPnl_TrackSegments.BackColor = System.Drawing.SystemColors.Window;
            this.flwLytPnl_TrackSegments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwLytPnl_TrackSegments.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwLytPnl_TrackSegments.Location = new System.Drawing.Point(5, 106);
            this.flwLytPnl_TrackSegments.Name = "flwLytPnl_TrackSegments";
            this.flwLytPnl_TrackSegments.Size = new System.Drawing.Size(551, 200);
            this.flwLytPnl_TrackSegments.TabIndex = 512;
            this.flwLytPnl_TrackSegments.WrapContents = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(5, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 45);
            this.groupBox1.TabIndex = 514;
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(434, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 26);
            this.label8.TabIndex = 21;
            this.label8.Text = "Min. Flight Altitude\r\n(m)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 26);
            this.label3.TabIndex = 20;
            this.label3.Text = "Initial Direction\r\n(MAG BRG)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(352, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 26);
            this.label5.TabIndex = 19;
            this.label5.Text = "Final Direction\r\n(MAG BRG)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 26);
            this.label2.TabIndex = 18;
            this.label2.Text = "Interm. Direction\r\n(MAG BRG)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(99, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 26);
            this.label6.TabIndex = 17;
            this.label6.Text = "Length\r\n(km)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Name";
            // 
            // MF_Page4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flwLytPnl_TrackSegments);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_LoadFromFile);
            this.Controls.Add(this.chkBox_isFinalSegmentStep);
            this.Controls.Add(this.btn_removeLastStep);
            this.Controls.Add(this.btn_addNewStep);
            this.Controls.Add(this.label1);
            this.Name = "MF_Page4";
            this.Size = new System.Drawing.Size(560, 400);
            this.VisibleChanged += new System.EventHandler(this.MF_Page4_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.trackStepBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btn_addNewStep;
        public System.Windows.Forms.Button btn_removeLastStep;
        private System.Windows.Forms.BindingSource trackStepBindingSource;
        private System.Windows.Forms.CheckBox chkBox_isFinalSegmentStep;
        private System.Windows.Forms.Button btn_LoadFromFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flwLytPnl_TrackSegments;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}
