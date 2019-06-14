namespace IsolineToVS
{
    partial class IsolineToVSForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Label label2;
            this.ui_generateButton = new System.Windows.Forms.Button();
            this.ui_layersCB = new System.Windows.Forms.ComboBox();
            this.ui_horizontalAccuracyNud = new System.Windows.Forms.NumericUpDown();
            this.ui_write3dChB = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_horizontalAccuracyNud)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 13);
            label1.TabIndex = 0;
            label1.Text = "Isoline Layer:";
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button1.Location = new System.Drawing.Point(306, 135);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.Close_Click);
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button2.Location = new System.Drawing.Point(332, 10);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(49, 23);
            button2.TabIndex = 4;
            button2.Text = "Refresh";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.RefreshLayerList_Click);
            // 
            // ui_generateButton
            // 
            this.ui_generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_generateButton.Enabled = false;
            this.ui_generateButton.Location = new System.Drawing.Point(12, 135);
            this.ui_generateButton.Name = "ui_generateButton";
            this.ui_generateButton.Size = new System.Drawing.Size(178, 23);
            this.ui_generateButton.TabIndex = 3;
            this.ui_generateButton.Text = "Generate AIXM Message";
            this.ui_generateButton.UseVisualStyleBackColor = true;
            this.ui_generateButton.Click += new System.EventHandler(this.Generate_Click);
            // 
            // ui_layersCB
            // 
            this.ui_layersCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_layersCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_layersCB.FormattingEnabled = true;
            this.ui_layersCB.Location = new System.Drawing.Point(87, 12);
            this.ui_layersCB.Name = "ui_layersCB";
            this.ui_layersCB.Size = new System.Drawing.Size(239, 21);
            this.ui_layersCB.TabIndex = 1;
            this.ui_layersCB.SelectedIndexChanged += new System.EventHandler(this.CurrentLayers_SelectedIndexChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 47);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(105, 13);
            label2.TabIndex = 5;
            label2.Text = "Horizontal Accuracy:";
            // 
            // ui_horizontalAccuracyNud
            // 
            this.ui_horizontalAccuracyNud.DecimalPlaces = 2;
            this.ui_horizontalAccuracyNud.Location = new System.Drawing.Point(123, 45);
            this.ui_horizontalAccuracyNud.Name = "ui_horizontalAccuracyNud";
            this.ui_horizontalAccuracyNud.Size = new System.Drawing.Size(67, 20);
            this.ui_horizontalAccuracyNud.TabIndex = 6;
            // 
            // ui_write3dChB
            // 
            this.ui_write3dChB.AutoSize = true;
            this.ui_write3dChB.Location = new System.Drawing.Point(15, 71);
            this.ui_write3dChB.Name = "ui_write3dChB";
            this.ui_write3dChB.Size = new System.Drawing.Size(169, 17);
            this.ui_write3dChB.TabIndex = 7;
            this.ui_write3dChB.Text = "Contour value as Z coordinate";
            this.ui_write3dChB.UseVisualStyleBackColor = true;
            // 
            // IsolineToVSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 170);
            this.Controls.Add(this.ui_write3dChB);
            this.Controls.Add(this.ui_horizontalAccuracyNud);
            this.Controls.Add(label2);
            this.Controls.Add(button2);
            this.Controls.Add(this.ui_generateButton);
            this.Controls.Add(button1);
            this.Controls.Add(this.ui_layersCB);
            this.Controls.Add(label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IsolineToVSForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Isoline to Vertical Structure";
            this.Load += new System.EventHandler(this.IsolineToVSForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_horizontalAccuracyNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_layersCB;
        private System.Windows.Forms.Button ui_generateButton;
        private System.Windows.Forms.NumericUpDown ui_horizontalAccuracyNud;
        private System.Windows.Forms.CheckBox ui_write3dChB;
    }
}