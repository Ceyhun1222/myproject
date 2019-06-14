namespace MapEnv.Forms
{
	partial class FormUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUser));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxUserName = new System.Windows.Forms.TextBox();
            this.txtBxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBxConfirmPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.treeViewFeatTypes = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radBtnReadWrite = new System.Windows.Forms.RadioButton();
            this.radBtnReadOnly = new System.Windows.Forms.RadioButton();
            this.chckBxSelectAll = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chckBxShowPassword = new System.Windows.Forms.CheckBox();
            this.lblPasswordMatch = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name";
            // 
            // txtBxUserName
            // 
            this.txtBxUserName.BackColor = System.Drawing.SystemColors.Window;
            this.txtBxUserName.Location = new System.Drawing.Point(12, 68);
            this.txtBxUserName.Name = "txtBxUserName";
            this.txtBxUserName.Size = new System.Drawing.Size(149, 20);
            this.txtBxUserName.TabIndex = 1;
            this.txtBxUserName.TextChanged += new System.EventHandler(this.txtBxUserName_TextChanged);
            this.txtBxUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBxUserName_KeyPress);
            // 
            // txtBxPassword
            // 
            this.txtBxPassword.Location = new System.Drawing.Point(12, 113);
            this.txtBxPassword.Name = "txtBxPassword";
            this.txtBxPassword.Size = new System.Drawing.Size(149, 20);
            this.txtBxPassword.TabIndex = 2;
            this.txtBxPassword.UseSystemPasswordChar = true;
            this.txtBxPassword.TextChanged += new System.EventHandler(this.txtBxPassword_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // txtBxConfirmPassword
            // 
            this.txtBxConfirmPassword.Location = new System.Drawing.Point(12, 159);
            this.txtBxConfirmPassword.Name = "txtBxConfirmPassword";
            this.txtBxConfirmPassword.Size = new System.Drawing.Size(149, 20);
            this.txtBxConfirmPassword.TabIndex = 3;
            this.txtBxConfirmPassword.UseSystemPasswordChar = true;
            this.txtBxConfirmPassword.TextChanged += new System.EventHandler(this.txtBxConfirmPassword_TextChanged);
            this.txtBxConfirmPassword.Leave += new System.EventHandler(this.txtBxConfirmPassword_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Confirm password";
            // 
            // treeViewFeatTypes
            // 
            this.treeViewFeatTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewFeatTypes.CheckBoxes = true;
            this.treeViewFeatTypes.FullRowSelect = true;
            this.treeViewFeatTypes.Location = new System.Drawing.Point(185, 68);
            this.treeViewFeatTypes.Name = "treeViewFeatTypes";
            this.treeViewFeatTypes.Size = new System.Drawing.Size(304, 311);
            this.treeViewFeatTypes.TabIndex = 8;
            this.treeViewFeatTypes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFeatTypes_AfterCheck);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radBtnReadWrite);
            this.groupBox1.Controls.Add(this.radBtnReadOnly);
            this.groupBox1.Location = new System.Drawing.Point(185, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 37);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Priviliges";
            // 
            // radBtnReadWrite
            // 
            this.radBtnReadWrite.AutoSize = true;
            this.radBtnReadWrite.Location = new System.Drawing.Point(137, 14);
            this.radBtnReadWrite.Name = "radBtnReadWrite";
            this.radBtnReadWrite.Size = new System.Drawing.Size(81, 17);
            this.radBtnReadWrite.TabIndex = 6;
            this.radBtnReadWrite.Text = "Read/Write";
            this.radBtnReadWrite.UseVisualStyleBackColor = true;
            // 
            // radBtnReadOnly
            // 
            this.radBtnReadOnly.AutoSize = true;
            this.radBtnReadOnly.Checked = true;
            this.radBtnReadOnly.Location = new System.Drawing.Point(14, 14);
            this.radBtnReadOnly.Name = "radBtnReadOnly";
            this.radBtnReadOnly.Size = new System.Drawing.Size(75, 17);
            this.radBtnReadOnly.TabIndex = 5;
            this.radBtnReadOnly.TabStop = true;
            this.radBtnReadOnly.Text = "Read Only";
            this.radBtnReadOnly.UseVisualStyleBackColor = true;
            // 
            // chckBxSelectAll
            // 
            this.chckBxSelectAll.AutoSize = true;
            this.chckBxSelectAll.Location = new System.Drawing.Point(209, 47);
            this.chckBxSelectAll.Name = "chckBxSelectAll";
            this.chckBxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chckBxSelectAll.TabIndex = 7;
            this.chckBxSelectAll.Text = "Select All";
            this.chckBxSelectAll.UseVisualStyleBackColor = true;
            this.chckBxSelectAll.CheckedChanged += new System.EventHandler(this.chckBxSelectAll_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(414, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(333, 385);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 27);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chckBxShowPassword
            // 
            this.chckBxShowPassword.AutoSize = true;
            this.chckBxShowPassword.Location = new System.Drawing.Point(12, 196);
            this.chckBxShowPassword.Name = "chckBxShowPassword";
            this.chckBxShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chckBxShowPassword.TabIndex = 4;
            this.chckBxShowPassword.Text = "Show Password";
            this.chckBxShowPassword.UseVisualStyleBackColor = true;
            this.chckBxShowPassword.CheckedChanged += new System.EventHandler(this.chckBxShowPassword_CheckedChanged);
            // 
            // lblPasswordMatch
            // 
            this.lblPasswordMatch.AutoSize = true;
            this.lblPasswordMatch.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordMatch.Location = new System.Drawing.Point(11, 182);
            this.lblPasswordMatch.Name = "lblPasswordMatch";
            this.lblPasswordMatch.Size = new System.Drawing.Size(122, 13);
            this.lblPasswordMatch.TabIndex = 12;
            this.lblPasswordMatch.Text = "Password doesn\'t match";
            this.lblPasswordMatch.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(167, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "*";
            // 
            // FormUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(496, 421);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPasswordMatch);
            this.Controls.Add(this.chckBxShowPassword);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chckBxSelectAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeViewFeatTypes);
            this.Controls.Add(this.txtBxConfirmPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBxPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBxUserName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(457, 272);
            this.Name = "FormUser";
            this.ShowInTaskbar = false;
            this.Text = "User - ";
            this.Load += new System.EventHandler(this.FormUser_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBxUserName;
		private System.Windows.Forms.TextBox txtBxPassword;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBxConfirmPassword;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TreeView treeViewFeatTypes;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radBtnReadWrite;
		private System.Windows.Forms.RadioButton radBtnReadOnly;
		private System.Windows.Forms.CheckBox chckBxSelectAll;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.CheckBox chckBxShowPassword;
		private System.Windows.Forms.Label lblPasswordMatch;
		private System.Windows.Forms.Label label4;
	}
}