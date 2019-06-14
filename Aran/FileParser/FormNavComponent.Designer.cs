namespace KFileParser
{
	partial class FormNavComponent
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
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbl1 = new System.Windows.Forms.Label();
			this.lbl2 = new System.Windows.Forms.Label();
			this.lbl3 = new System.Windows.Forms.Label();
			this.cmbBx1 = new System.Windows.Forms.ComboBox();
			this.cmbBx2 = new System.Windows.Forms.ComboBox();
			this.cmbBx3 = new System.Windows.Forms.ComboBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(306, 282);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(387, 282);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lbl1
			// 
			this.lbl1.AutoSize = true;
			this.lbl1.Location = new System.Drawing.Point(10, 35);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(31, 13);
			this.lbl1.TabIndex = 3;
			this.lbl1.Text = "DME";
			this.lbl1.Visible = false;
			// 
			// lbl2
			// 
			this.lbl2.AutoSize = true;
			this.lbl2.Location = new System.Drawing.Point(10, 74);
			this.lbl2.Name = "lbl2";
			this.lbl2.Size = new System.Drawing.Size(26, 13);
			this.lbl2.TabIndex = 4;
			this.lbl2.Text = "LLZ";
			this.lbl2.Visible = false;
			// 
			// lbl3
			// 
			this.lbl3.AutoSize = true;
			this.lbl3.Location = new System.Drawing.Point(10, 117);
			this.lbl3.Name = "lbl3";
			this.lbl3.Size = new System.Drawing.Size(80, 13);
			this.lbl3.TabIndex = 5;
			this.lbl3.Text = "Marker Beacon";
			this.lbl3.Visible = false;
			// 
			// cmbBx1
			// 
			this.cmbBx1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBx1.FormattingEnabled = true;
			this.cmbBx1.Location = new System.Drawing.Point(99, 30);
			this.cmbBx1.Name = "cmbBx1";
			this.cmbBx1.Size = new System.Drawing.Size(147, 21);
			this.cmbBx1.TabIndex = 6;
			this.cmbBx1.Visible = false;
			// 
			// cmbBx2
			// 
			this.cmbBx2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBx2.FormattingEnabled = true;
			this.cmbBx2.Location = new System.Drawing.Point(99, 71);
			this.cmbBx2.Name = "cmbBx2";
			this.cmbBx2.Size = new System.Drawing.Size(147, 21);
			this.cmbBx2.TabIndex = 7;
			this.cmbBx2.Visible = false;
			// 
			// cmbBx3
			// 
			this.cmbBx3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBx3.FormattingEnabled = true;
			this.cmbBx3.Location = new System.Drawing.Point(99, 114);
			this.cmbBx3.Name = "cmbBx3";
			this.cmbBx3.Size = new System.Drawing.Size(147, 21);
			this.cmbBx3.TabIndex = 8;
			this.cmbBx3.Visible = false;
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(12, 12);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(187, 286);
			this.treeView1.TabIndex = 9;
			this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnSave);
			this.groupBox1.Controls.Add(this.lbl3);
			this.groupBox1.Controls.Add(this.cmbBx3);
			this.groupBox1.Controls.Add(this.lbl1);
			this.groupBox1.Controls.Add(this.cmbBx2);
			this.groupBox1.Controls.Add(this.lbl2);
			this.groupBox1.Controls.Add(this.cmbBx1);
			this.groupBox1.Location = new System.Drawing.Point(205, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(252, 181);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Visible = false;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(171, 150);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// FormNavComponent
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(469, 310);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Name = "FormNavComponent";
			this.ShowInTaskbar = false;
			this.Text = "Navaid Components";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.Label lbl2;
		private System.Windows.Forms.Label lbl3;
		private System.Windows.Forms.ComboBox cmbBx1;
		private System.Windows.Forms.ComboBox cmbBx2;
		private System.Windows.Forms.ComboBox cmbBx3;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSave;
	}
}