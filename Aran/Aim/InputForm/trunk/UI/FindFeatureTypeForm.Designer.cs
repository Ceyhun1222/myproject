namespace Aran.Aim.InputForm
{
	partial class FindFeatureTypeForm
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
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.Panel panel3;
			this.ui_closeButton = new System.Windows.Forms.Button();
			this.ui_findButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.ui_featureTypeCB = new System.Windows.Forms.ComboBox();
			this.ui_infoPanel = new System.Windows.Forms.Panel();
			this.ui_infoLabel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			panel1 = new System.Windows.Forms.Panel();
			panel3 = new System.Windows.Forms.Panel();
			this.flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			this.ui_infoPanel.SuspendLayout();
			panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(0, 7);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(70, 13);
			label1.TabIndex = 1;
			label1.Text = "FeatureType:";
			// 
			// ui_closeButton
			// 
			this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_closeButton.Location = new System.Drawing.Point(273, 12);
			this.ui_closeButton.Name = "ui_closeButton";
			this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
			this.ui_closeButton.TabIndex = 3;
			this.ui_closeButton.Text = "Close";
			this.ui_closeButton.UseVisualStyleBackColor = true;
			this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
			// 
			// ui_findButton
			// 
			this.ui_findButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_findButton.Location = new System.Drawing.Point(192, 12);
			this.ui_findButton.Name = "ui_findButton";
			this.ui_findButton.Size = new System.Drawing.Size(75, 23);
			this.ui_findButton.TabIndex = 2;
			this.ui_findButton.Text = "Find";
			this.ui_findButton.UseVisualStyleBackColor = true;
			this.ui_findButton.Click += new System.EventHandler(this.Find_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(panel1);
			this.flowLayoutPanel1.Controls.Add(this.ui_infoPanel);
			this.flowLayoutPanel1.Controls.Add(panel3);
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(6);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(381, 167);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// panel1
			// 
			panel1.Controls.Add(this.ui_featureTypeCB);
			panel1.Controls.Add(label1);
			panel1.Location = new System.Drawing.Point(9, 9);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(363, 65);
			panel1.TabIndex = 0;
			// 
			// ui_featureTypeCB
			// 
			this.ui_featureTypeCB.FormattingEnabled = true;
			this.ui_featureTypeCB.Location = new System.Drawing.Point(3, 33);
			this.ui_featureTypeCB.Name = "ui_featureTypeCB";
			this.ui_featureTypeCB.Size = new System.Drawing.Size(357, 21);
			this.ui_featureTypeCB.Sorted = true;
			this.ui_featureTypeCB.TabIndex = 2;
			// 
			// ui_infoPanel
			// 
			this.ui_infoPanel.AutoSize = true;
			this.ui_infoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ui_infoPanel.Controls.Add(this.ui_infoLabel);
			this.ui_infoPanel.Location = new System.Drawing.Point(9, 80);
			this.ui_infoPanel.Name = "ui_infoPanel";
			this.ui_infoPanel.Size = new System.Drawing.Size(75, 25);
			this.ui_infoPanel.TabIndex = 1;
			this.ui_infoPanel.Visible = false;
			// 
			// ui_infoLabel
			// 
			this.ui_infoLabel.AutoSize = true;
			this.ui_infoLabel.Location = new System.Drawing.Point(6, 12);
			this.ui_infoLabel.Name = "ui_infoLabel";
			this.ui_infoLabel.Size = new System.Drawing.Size(66, 13);
			this.ui_infoLabel.TabIndex = 0;
			this.ui_infoLabel.Text = "<Info Label>";
			// 
			// panel3
			// 
			panel3.Controls.Add(this.ui_closeButton);
			panel3.Controls.Add(this.ui_findButton);
			panel3.Location = new System.Drawing.Point(9, 111);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(360, 47);
			panel3.TabIndex = 2;
			// 
			// FindFeatureTypeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(388, 212);
			this.Controls.Add(this.flowLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindFeatureTypeForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Find FeatureType";
			this.Load += new System.EventHandler(this.FindFeatureTypeForm_Load);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			this.ui_infoPanel.ResumeLayout(false);
			this.ui_infoPanel.PerformLayout();
			panel3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ui_findButton;
		private System.Windows.Forms.Button ui_closeButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel ui_infoPanel;
		private System.Windows.Forms.Label ui_infoLabel;
		private System.Windows.Forms.ComboBox ui_featureTypeCB;
	}
}