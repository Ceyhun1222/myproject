namespace Aran.Aim.FmdEditor
{
	partial class NavigatorControl
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigatorControl));
			this.ui_countLabel = new System.Windows.Forms.Label();
			this.ui_iOfCountTB = new System.Windows.Forms.TextBox();
			this.ui_lastButton = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.ui_nextButton = new System.Windows.Forms.Button();
			this.ui_prevButton = new System.Windows.Forms.Button();
			this.ui_firstButton = new System.Windows.Forms.Button();
			this.ui_removeButton = new System.Windows.Forms.Button();
			this.ui_addButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ui_countLabel
			// 
			this.ui_countLabel.AutoSize = true;
			this.ui_countLabel.Location = new System.Drawing.Point(193, 9);
			this.ui_countLabel.Name = "ui_countLabel";
			this.ui_countLabel.Size = new System.Drawing.Size(21, 13);
			this.ui_countLabel.TabIndex = 16;
			this.ui_countLabel.Text = "/ 0";
			// 
			// ui_iOfCountTB
			// 
			this.ui_iOfCountTB.Location = new System.Drawing.Point(162, 5);
			this.ui_iOfCountTB.Name = "ui_iOfCountTB";
			this.ui_iOfCountTB.ReadOnly = true;
			this.ui_iOfCountTB.Size = new System.Drawing.Size(27, 20);
			this.ui_iOfCountTB.TabIndex = 15;
			// 
			// ui_lastButton
			// 
			this.ui_lastButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_lastButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_lastButton.ImageIndex = 5;
			this.ui_lastButton.ImageList = this.imageList1;
			this.ui_lastButton.Location = new System.Drawing.Point(123, 6);
			this.ui_lastButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_lastButton.Name = "ui_lastButton";
			this.ui_lastButton.Size = new System.Drawing.Size(19, 19);
			this.ui_lastButton.TabIndex = 14;
			this.ui_lastButton.UseVisualStyleBackColor = true;
			this.ui_lastButton.Click += new System.EventHandler(this.Last_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "add_16.png");
			this.imageList1.Images.SetKeyName(1, "remove_16.png");
			this.imageList1.Images.SetKeyName(2, "first_16.png");
			this.imageList1.Images.SetKeyName(3, "prev_16.png");
			this.imageList1.Images.SetKeyName(4, "next_16.png");
			this.imageList1.Images.SetKeyName(5, "last_16.png");
			// 
			// ui_nextButton
			// 
			this.ui_nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_nextButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_nextButton.ImageIndex = 4;
			this.ui_nextButton.ImageList = this.imageList1;
			this.ui_nextButton.Location = new System.Drawing.Point(102, 6);
			this.ui_nextButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_nextButton.Name = "ui_nextButton";
			this.ui_nextButton.Size = new System.Drawing.Size(19, 19);
			this.ui_nextButton.TabIndex = 13;
			this.ui_nextButton.UseVisualStyleBackColor = true;
			this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
			// 
			// ui_prevButton
			// 
			this.ui_prevButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_prevButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_prevButton.ImageIndex = 3;
			this.ui_prevButton.ImageList = this.imageList1;
			this.ui_prevButton.Location = new System.Drawing.Point(81, 6);
			this.ui_prevButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_prevButton.Name = "ui_prevButton";
			this.ui_prevButton.Size = new System.Drawing.Size(19, 19);
			this.ui_prevButton.TabIndex = 12;
			this.ui_prevButton.UseVisualStyleBackColor = true;
			this.ui_prevButton.Click += new System.EventHandler(this.Prev_Click);
			// 
			// ui_firstButton
			// 
			this.ui_firstButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_firstButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_firstButton.ImageIndex = 2;
			this.ui_firstButton.ImageList = this.imageList1;
			this.ui_firstButton.Location = new System.Drawing.Point(60, 6);
			this.ui_firstButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_firstButton.Name = "ui_firstButton";
			this.ui_firstButton.Size = new System.Drawing.Size(19, 19);
			this.ui_firstButton.TabIndex = 11;
			this.ui_firstButton.UseVisualStyleBackColor = true;
			this.ui_firstButton.Click += new System.EventHandler(this.First_Click);
			// 
			// ui_removeButton
			// 
			this.ui_removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_removeButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_removeButton.ImageIndex = 1;
			this.ui_removeButton.ImageList = this.imageList1;
			this.ui_removeButton.Location = new System.Drawing.Point(27, 6);
			this.ui_removeButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_removeButton.Name = "ui_removeButton";
			this.ui_removeButton.Size = new System.Drawing.Size(19, 19);
			this.ui_removeButton.TabIndex = 10;
			this.ui_removeButton.UseVisualStyleBackColor = true;
			this.ui_removeButton.Click += new System.EventHandler(this.Remove_Click);
			// 
			// ui_addButton
			// 
			this.ui_addButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ui_addButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.ui_addButton.ImageIndex = 0;
			this.ui_addButton.ImageList = this.imageList1;
			this.ui_addButton.Location = new System.Drawing.Point(6, 6);
			this.ui_addButton.Margin = new System.Windows.Forms.Padding(1);
			this.ui_addButton.Name = "ui_addButton";
			this.ui_addButton.Size = new System.Drawing.Size(19, 19);
			this.ui_addButton.TabIndex = 9;
			this.ui_addButton.UseVisualStyleBackColor = true;
			this.ui_addButton.Click += new System.EventHandler(this.Add_Click);
			// 
			// NavigatorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ui_countLabel);
			this.Controls.Add(this.ui_iOfCountTB);
			this.Controls.Add(this.ui_lastButton);
			this.Controls.Add(this.ui_nextButton);
			this.Controls.Add(this.ui_prevButton);
			this.Controls.Add(this.ui_firstButton);
			this.Controls.Add(this.ui_removeButton);
			this.Controls.Add(this.ui_addButton);
			this.Name = "NavigatorControl";
			this.Size = new System.Drawing.Size(234, 32);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ui_iOfCountTB;
		private System.Windows.Forms.Button ui_lastButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button ui_nextButton;
		private System.Windows.Forms.Button ui_prevButton;
		private System.Windows.Forms.Button ui_firstButton;
		private System.Windows.Forms.Button ui_removeButton;
		private System.Windows.Forms.Button ui_addButton;
		private System.Windows.Forms.Label ui_countLabel;
	}
}
