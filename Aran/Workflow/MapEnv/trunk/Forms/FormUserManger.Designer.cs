namespace MapEnv.Forms
{
	partial class FormUserManger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserManger));
            this.btnShowHide = new System.Windows.Forms.Button();
            this.dataGridVwUsers = new System.Windows.Forms.DataGridView();
            this.ClmnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmnPrivilege = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chckBxShowPassword = new System.Windows.Forms.CheckBox();
            this.txtBxPassword = new System.Windows.Forms.TextBox();
            this.btnUserOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxUserName = new System.Windows.Forms.TextBox();
            this.btnCreateNewUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVwUsers)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowHide
            // 
            this.btnShowHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowHide.AutoSize = true;
            this.btnShowHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowHide.FlatAppearance.BorderSize = 0;
            this.btnShowHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowHide.Image = global::MapEnv.Properties.Resources.arrow_up_16;
            this.btnShowHide.Location = new System.Drawing.Point(427, 0);
            this.btnShowHide.Name = "btnShowHide";
            this.btnShowHide.Size = new System.Drawing.Size(24, 24);
            this.btnShowHide.TabIndex = 6;
            this.btnShowHide.UseVisualStyleBackColor = true;
            this.btnShowHide.Click += new System.EventHandler(this.btnShowHide_Click);
            // 
            // dataGridVwUsers
            // 
            this.dataGridVwUsers.AllowUserToAddRows = false;
            this.dataGridVwUsers.AllowUserToDeleteRows = false;
            this.dataGridVwUsers.AllowUserToOrderColumns = true;
            this.dataGridVwUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridVwUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridVwUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVwUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClmnName,
            this.ClmnPrivilege});
            this.dataGridVwUsers.Location = new System.Drawing.Point(0, 39);
            this.dataGridVwUsers.MultiSelect = false;
            this.dataGridVwUsers.Name = "dataGridVwUsers";
            this.dataGridVwUsers.ReadOnly = true;
            this.dataGridVwUsers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridVwUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridVwUsers.Size = new System.Drawing.Size(451, 313);
            this.dataGridVwUsers.TabIndex = 8;
            // 
            // ClmnName
            // 
            this.ClmnName.HeaderText = "User Name";
            this.ClmnName.Name = "ClmnName";
            this.ClmnName.ReadOnly = true;
            this.ClmnName.Width = 200;
            // 
            // ClmnPrivilege
            // 
            this.ClmnPrivilege.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClmnPrivilege.HeaderText = "Privilege";
            this.ClmnPrivilege.Name = "ClmnPrivilege";
            this.ClmnPrivilege.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chckBxShowPassword);
            this.groupBox1.Controls.Add(this.txtBxPassword);
            this.groupBox1.Controls.Add(this.btnUserOk);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtBxUserName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 65);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // chckBxShowPassword
            // 
            this.chckBxShowPassword.AutoSize = true;
            this.chckBxShowPassword.Location = new System.Drawing.Point(248, 40);
            this.chckBxShowPassword.Name = "chckBxShowPassword";
            this.chckBxShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chckBxShowPassword.TabIndex = 4;
            this.chckBxShowPassword.Text = "Show Password";
            this.chckBxShowPassword.UseVisualStyleBackColor = true;
            this.chckBxShowPassword.CheckedChanged += new System.EventHandler(this.chckBxShowPassword_CheckedChanged);
            // 
            // txtBxPassword
            // 
            this.txtBxPassword.Location = new System.Drawing.Point(245, 14);
            this.txtBxPassword.Name = "txtBxPassword";
            this.txtBxPassword.Size = new System.Drawing.Size(100, 20);
            this.txtBxPassword.TabIndex = 3;
            this.txtBxPassword.UseSystemPasswordChar = true;
            // 
            // btnUserOk
            // 
            this.btnUserOk.Location = new System.Drawing.Point(368, 12);
            this.btnUserOk.Name = "btnUserOk";
            this.btnUserOk.Size = new System.Drawing.Size(75, 23);
            this.btnUserOk.TabIndex = 5;
            this.btnUserOk.Text = "OK";
            this.btnUserOk.UseVisualStyleBackColor = true;
            this.btnUserOk.Click += new System.EventHandler(this.btnUserOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name";
            // 
            // txtBxUserName
            // 
            this.txtBxUserName.Location = new System.Drawing.Point(74, 14);
            this.txtBxUserName.Name = "txtBxUserName";
            this.txtBxUserName.Size = new System.Drawing.Size(100, 20);
            this.txtBxUserName.TabIndex = 2;
            // 
            // btnCreateNewUser
            // 
            this.btnCreateNewUser.AutoSize = true;
            this.btnCreateNewUser.Image = global::MapEnv.Properties.Resources.new_24;
            this.btnCreateNewUser.Location = new System.Drawing.Point(4, 2);
            this.btnCreateNewUser.Name = "btnCreateNewUser";
            this.btnCreateNewUser.Size = new System.Drawing.Size(73, 32);
            this.btnCreateNewUser.TabIndex = 7;
            this.btnCreateNewUser.Text = "New";
            this.btnCreateNewUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCreateNewUser.UseVisualStyleBackColor = true;
            this.btnCreateNewUser.Click += new System.EventHandler(this.btnCreateNewUser_Click);
            // 
            // btnEditUser
            // 
            this.btnEditUser.Image = ((System.Drawing.Image)(resources.GetObject("btnEditUser.Image")));
            this.btnEditUser.Location = new System.Drawing.Point(80, 3);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(73, 32);
            this.btnEditUser.TabIndex = 8;
            this.btnEditUser.Text = "Edit";
            this.btnEditUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditUser.UseVisualStyleBackColor = true;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Image = global::MapEnv.Properties.Resources.cancel_24;
            this.btnDeleteUser.Location = new System.Drawing.Point(158, 3);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(73, 32);
            this.btnDeleteUser.TabIndex = 9;
            this.btnDeleteUser.Text = "Delete";
            this.btnDeleteUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnShowHide);
            this.panel1.Controls.Add(this.dataGridVwUsers);
            this.panel1.Controls.Add(this.btnDeleteUser);
            this.panel1.Controls.Add(this.btnEditUser);
            this.panel1.Controls.Add(this.btnCreateNewUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 389);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(368, 359);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormUserManger
            // 
            this.AcceptButton = this.btnUserOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(451, 455);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserManger";
            this.ShowInTaskbar = false;
            this.Text = "User Manger";
            this.Load += new System.EventHandler(this.FormUserManger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVwUsers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnShowHide;
		private System.Windows.Forms.DataGridView dataGridVwUsers;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chckBxShowPassword;
		private System.Windows.Forms.TextBox txtBxPassword;
		private System.Windows.Forms.Button btnUserOk;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBxUserName;
		private System.Windows.Forms.Button btnCreateNewUser;
		private System.Windows.Forms.Button btnEditUser;
		private System.Windows.Forms.Button btnDeleteUser;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ClmnName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ClmnPrivilege;
		private System.Windows.Forms.Button btnClose;

	}
}