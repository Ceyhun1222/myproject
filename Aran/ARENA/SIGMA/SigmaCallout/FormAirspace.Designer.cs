namespace SigmaCallout
{
	partial class FormAirspace
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ( );
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ( )
		{
            this.pnlAirspaceBackColor = new System.Windows.Forms.Panel();
            this.btnAirspaceBackColor = new System.Windows.Forms.Button();
            this.txtBxAirspaceClass = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radBtnRight = new System.Windows.Forms.RadioButton();
            this.radBtnLeft = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAirspaceBackColor
            // 
            this.pnlAirspaceBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlAirspaceBackColor.Location = new System.Drawing.Point(102, 34);
            this.pnlAirspaceBackColor.Name = "pnlAirspaceBackColor";
            this.pnlAirspaceBackColor.Size = new System.Drawing.Size(45, 23);
            this.pnlAirspaceBackColor.TabIndex = 24;
            this.pnlAirspaceBackColor.Visible = false;
            // 
            // btnAirspaceBackColor
            // 
            this.btnAirspaceBackColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAirspaceBackColor.Location = new System.Drawing.Point(148, 34);
            this.btnAirspaceBackColor.Name = "btnAirspaceBackColor";
            this.btnAirspaceBackColor.Size = new System.Drawing.Size(14, 23);
            this.btnAirspaceBackColor.TabIndex = 23;
            this.btnAirspaceBackColor.Text = "^";
            this.btnAirspaceBackColor.UseVisualStyleBackColor = true;
            this.btnAirspaceBackColor.Visible = false;
            this.btnAirspaceBackColor.Click += new System.EventHandler(this.btnAirspaceBackColor_Click);
            // 
            // txtBxAirspaceClass
            // 
            this.txtBxAirspaceClass.Location = new System.Drawing.Point(102, 8);
            this.txtBxAirspaceClass.Name = "txtBxAirspaceClass";
            this.txtBxAirspaceClass.Size = new System.Drawing.Size(45, 20);
            this.txtBxAirspaceClass.TabIndex = 21;
            this.txtBxAirspaceClass.Text = "D";
            this.txtBxAirspaceClass.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Symbol";
            this.label17.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(58, 13);
            this.label18.TabIndex = 22;
            this.label18.Text = "Back color";
            this.label18.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radBtnRight);
            this.groupBox1.Controls.Add(this.radBtnLeft);
            this.groupBox1.Location = new System.Drawing.Point(14, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 51);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            this.groupBox1.Visible = false;
            // 
            // radBtnRight
            // 
            this.radBtnRight.AutoSize = true;
            this.radBtnRight.Location = new System.Drawing.Point(88, 19);
            this.radBtnRight.Name = "radBtnRight";
            this.radBtnRight.Size = new System.Drawing.Size(50, 17);
            this.radBtnRight.TabIndex = 1;
            this.radBtnRight.TabStop = true;
            this.radBtnRight.Text = "Right";
            this.radBtnRight.UseVisualStyleBackColor = true;
            // 
            // radBtnLeft
            // 
            this.radBtnLeft.AutoSize = true;
            this.radBtnLeft.Location = new System.Drawing.Point(10, 19);
            this.radBtnLeft.Name = "radBtnLeft";
            this.radBtnLeft.Size = new System.Drawing.Size(43, 17);
            this.radBtnLeft.TabIndex = 0;
            this.radBtnLeft.TabStop = true;
            this.radBtnLeft.Text = "Left";
            this.radBtnLeft.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(146, 122);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 25);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormAirspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 162);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlAirspaceBackColor);
            this.Controls.Add(this.btnAirspaceBackColor);
            this.Controls.Add(this.txtBxAirspaceClass);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(220, 190);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(220, 190);
            this.Name = "FormAirspace";
            this.ShowIcon = false;
            this.Text = "Airspace element settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlAirspaceBackColor;
		private System.Windows.Forms.Button btnAirspaceBackColor;
		private System.Windows.Forms.TextBox txtBxAirspaceClass;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radBtnRight;
		private System.Windows.Forms.RadioButton radBtnLeft;
		private System.Windows.Forms.Button btnClose;
	}
}