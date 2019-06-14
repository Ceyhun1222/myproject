namespace Aran.PANDA.CRMWall
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.ComboBox001 = new System.Windows.Forms.ComboBox();
			this.label001 = new System.Windows.Forms.Label();
			this.textBox001 = new System.Windows.Forms.TextBox();
			this.label002 = new System.Windows.Forms.Label();
			this.label003 = new System.Windows.Forms.Label();
			this.textBox002 = new System.Windows.Forms.TextBox();
			this.label004 = new System.Windows.Forms.Label();
			this.label005 = new System.Windows.Forms.Label();
			this.textBox003 = new System.Windows.Forms.TextBox();
			this.label006 = new System.Windows.Forms.Label();
			this.label007 = new System.Windows.Forms.Label();
			this.CreateBtn = new System.Windows.Forms.Button();
			this.InfoBtn = new System.Windows.Forms.Button();
			this.ExportBtn = new System.Windows.Forms.Button();
			this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox004 = new System.Windows.Forms.TextBox();
			this.label008 = new System.Windows.Forms.Label();
			this.CloseBtn = new System.Windows.Forms.Button();
			this.SavePolylineBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ComboBox001
			// 
			this.ComboBox001.BackColor = System.Drawing.SystemColors.Window;
			this.ComboBox001.Cursor = System.Windows.Forms.Cursors.Default;
			this.ComboBox001.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBox001.Font = new System.Drawing.Font("Arial", 8F);
			this.ComboBox001.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ComboBox001.Location = new System.Drawing.Point(168, 14);
			this.ComboBox001.Name = "ComboBox001";
			this.ComboBox001.Size = new System.Drawing.Size(67, 22);
			this.ComboBox001.TabIndex = 106;
			this.ComboBox001.SelectedIndexChanged += new System.EventHandler(this.ComboBox001_SelectedIndexChanged);
			// 
			// label001
			// 
			this.label001.AutoSize = true;
			this.label001.BackColor = System.Drawing.SystemColors.Control;
			this.label001.Cursor = System.Windows.Forms.Cursors.Default;
			this.label001.Font = new System.Drawing.Font("Arial", 8F);
			this.label001.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label001.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label001.Location = new System.Drawing.Point(22, 18);
			this.label001.Name = "label001";
			this.label001.Size = new System.Drawing.Size(51, 14);
			this.label001.TabIndex = 107;
			this.label001.Text = "Runway:";
			// 
			// textBox001
			// 
			this.textBox001.AcceptsReturn = true;
			this.textBox001.BackColor = System.Drawing.SystemColors.Window;
			this.textBox001.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox001.Font = new System.Drawing.Font("Arial", 8F);
			this.textBox001.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textBox001.Location = new System.Drawing.Point(168, 69);
			this.textBox001.MaxLength = 7;
			this.textBox001.Name = "textBox001";
			this.textBox001.Size = new System.Drawing.Size(67, 20);
			this.textBox001.TabIndex = 198;
			this.textBox001.Tag = "a";
			this.textBox001.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox001_KeyPress);
			this.textBox001.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
			this.textBox001.Validating += new System.ComponentModel.CancelEventHandler(this.textBox001_Validating);
			// 
			// label002
			// 
			this.label002.AutoSize = true;
			this.label002.BackColor = System.Drawing.SystemColors.Control;
			this.label002.Cursor = System.Windows.Forms.Cursors.Default;
			this.label002.Font = new System.Drawing.Font("Arial", 8F);
			this.label002.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label002.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label002.Location = new System.Drawing.Point(22, 72);
			this.label002.Name = "label002";
			this.label002.Size = new System.Drawing.Size(135, 14);
			this.label002.TabIndex = 200;
			this.label002.Text = "Protection area semiwidth:";
			// 
			// label003
			// 
			this.label003.AutoSize = true;
			this.label003.BackColor = System.Drawing.SystemColors.Control;
			this.label003.Cursor = System.Windows.Forms.Cursors.Default;
			this.label003.Font = new System.Drawing.Font("Arial", 8F);
			this.label003.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label003.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label003.Location = new System.Drawing.Point(245, 72);
			this.label003.Name = "label003";
			this.label003.Size = new System.Drawing.Size(19, 14);
			this.label003.TabIndex = 199;
			this.label003.Text = "---";
			// 
			// textBox002
			// 
			this.textBox002.AcceptsReturn = true;
			this.textBox002.BackColor = System.Drawing.SystemColors.Window;
			this.textBox002.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox002.Font = new System.Drawing.Font("Arial", 8F);
			this.textBox002.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textBox002.Location = new System.Drawing.Point(168, 121);
			this.textBox002.MaxLength = 7;
			this.textBox002.Name = "textBox002";
			this.textBox002.Size = new System.Drawing.Size(67, 20);
			this.textBox002.TabIndex = 201;
			this.textBox002.Tag = "a";
			this.textBox002.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox002_KeyPress);
			this.textBox002.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
			this.textBox002.Validating += new System.ComponentModel.CancelEventHandler(this.textBox002_Validating);
			// 
			// label004
			// 
			this.label004.AutoSize = true;
			this.label004.BackColor = System.Drawing.SystemColors.Control;
			this.label004.Cursor = System.Windows.Forms.Cursors.Default;
			this.label004.Font = new System.Drawing.Font("Arial", 8F);
			this.label004.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label004.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label004.Location = new System.Drawing.Point(22, 124);
			this.label004.Name = "label004";
			this.label004.Size = new System.Drawing.Size(74, 14);
			this.label004.TabIndex = 203;
			this.label004.Text = "Dist IF to THR:";
			// 
			// label005
			// 
			this.label005.AutoSize = true;
			this.label005.BackColor = System.Drawing.SystemColors.Control;
			this.label005.Cursor = System.Windows.Forms.Cursors.Default;
			this.label005.Font = new System.Drawing.Font("Arial", 8F);
			this.label005.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label005.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label005.Location = new System.Drawing.Point(245, 124);
			this.label005.Name = "label005";
			this.label005.Size = new System.Drawing.Size(19, 14);
			this.label005.TabIndex = 202;
			this.label005.Text = "---";
			// 
			// textBox003
			// 
			this.textBox003.AcceptsReturn = true;
			this.textBox003.BackColor = System.Drawing.SystemColors.Window;
			this.textBox003.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox003.Font = new System.Drawing.Font("Arial", 8F);
			this.textBox003.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textBox003.Location = new System.Drawing.Point(434, 121);
			this.textBox003.MaxLength = 7;
			this.textBox003.Name = "textBox003";
			this.textBox003.Size = new System.Drawing.Size(67, 20);
			this.textBox003.TabIndex = 204;
			this.textBox003.Tag = "a";
			this.textBox003.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox003_KeyPress);
			this.textBox003.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
			this.textBox003.Validating += new System.ComponentModel.CancelEventHandler(this.textBox003_Validating);
			// 
			// label006
			// 
			this.label006.BackColor = System.Drawing.SystemColors.Control;
			this.label006.Cursor = System.Windows.Forms.Cursors.Default;
			this.label006.Font = new System.Drawing.Font("Arial", 8F);
			this.label006.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label006.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label006.Location = new System.Drawing.Point(288, 115);
			this.label006.Name = "label006";
			this.label006.Size = new System.Drawing.Size(140, 32);
			this.label006.TabIndex = 206;
			this.label006.Text = "Distance from termination point to THR:";
			// 
			// label007
			// 
			this.label007.AutoSize = true;
			this.label007.BackColor = System.Drawing.SystemColors.Control;
			this.label007.Cursor = System.Windows.Forms.Cursors.Default;
			this.label007.Font = new System.Drawing.Font("Arial", 8F);
			this.label007.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label007.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label007.Location = new System.Drawing.Point(511, 124);
			this.label007.Name = "label007";
			this.label007.Size = new System.Drawing.Size(19, 14);
			this.label007.TabIndex = 205;
			this.label007.Text = "---";
			// 
			// CreateBtn
			// 
			this.CreateBtn.Location = new System.Drawing.Point(25, 165);
			this.CreateBtn.Name = "CreateBtn";
			this.CreateBtn.Size = new System.Drawing.Size(75, 23);
			this.CreateBtn.TabIndex = 208;
			this.CreateBtn.Text = "Create walls";
			this.CreateBtn.UseVisualStyleBackColor = true;
			this.CreateBtn.Click += new System.EventHandler(this.CreateBtn_Click);
			// 
			// InfoBtn
			// 
			this.InfoBtn.BackColor = System.Drawing.SystemColors.Control;
			this.InfoBtn.Cursor = System.Windows.Forms.Cursors.Default;
			this.InfoBtn.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InfoBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.InfoBtn.Image = ((System.Drawing.Image)(resources.GetObject("InfoBtn.Image")));
			this.InfoBtn.Location = new System.Drawing.Point(248, 15);
			this.InfoBtn.Name = "InfoBtn";
			this.InfoBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.InfoBtn.Size = new System.Drawing.Size(21, 21);
			this.InfoBtn.TabIndex = 238;
			this.InfoBtn.TabStop = false;
			this.InfoBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.InfoBtn.UseVisualStyleBackColor = false;
			this.InfoBtn.Click += new System.EventHandler(this.InfoBtn_Click);
			// 
			// ExportBtn
			// 
			this.ExportBtn.Enabled = false;
			this.ExportBtn.Location = new System.Drawing.Point(139, 165);
			this.ExportBtn.Name = "ExportBtn";
			this.ExportBtn.Size = new System.Drawing.Size(75, 23);
			this.ExportBtn.TabIndex = 242;
			this.ExportBtn.Text = "Export...";
			this.ExportBtn.UseVisualStyleBackColor = true;
			this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
			// 
			// saveFileDlg
			// 
			this.saveFileDlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(288, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 243;
			this.label1.Text = "Partitioning step:";
			// 
			// textBox004
			// 
			this.textBox004.Location = new System.Drawing.Point(434, 69);
			this.textBox004.Name = "textBox004";
			this.textBox004.Size = new System.Drawing.Size(67, 20);
			this.textBox004.TabIndex = 244;
			this.textBox004.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox004_KeyPress);
			this.textBox004.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
			this.textBox004.Validating += new System.ComponentModel.CancelEventHandler(this.textBox004_Validating);
			// 
			// label008
			// 
			this.label008.AutoSize = true;
			this.label008.Location = new System.Drawing.Point(511, 73);
			this.label008.Name = "label008";
			this.label008.Size = new System.Drawing.Size(16, 13);
			this.label008.TabIndex = 245;
			this.label008.Text = "---";
			// 
			// CloseBtn
			// 
			this.CloseBtn.Location = new System.Drawing.Point(452, 165);
			this.CloseBtn.Name = "CloseBtn";
			this.CloseBtn.Size = new System.Drawing.Size(75, 23);
			this.CloseBtn.TabIndex = 246;
			this.CloseBtn.Text = "Close";
			this.CloseBtn.UseVisualStyleBackColor = true;
			this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
			// 
			// SavePolylineBtn
			// 
			this.SavePolylineBtn.Location = new System.Drawing.Point(320, 165);
			this.SavePolylineBtn.Name = "SavePolylineBtn";
			this.SavePolylineBtn.Size = new System.Drawing.Size(75, 23);
			this.SavePolylineBtn.TabIndex = 247;
			this.SavePolylineBtn.Text = "test";
			this.SavePolylineBtn.UseVisualStyleBackColor = true;
			this.SavePolylineBtn.Visible = false;
			this.SavePolylineBtn.Click += new System.EventHandler(this.TestBtn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(553, 212);
			this.Controls.Add(this.SavePolylineBtn);
			this.Controls.Add(this.CloseBtn);
			this.Controls.Add(this.label008);
			this.Controls.Add(this.textBox004);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ExportBtn);
			this.Controls.Add(this.InfoBtn);
			this.Controls.Add(this.CreateBtn);
			this.Controls.Add(this.textBox003);
			this.Controls.Add(this.label006);
			this.Controls.Add(this.label007);
			this.Controls.Add(this.textBox002);
			this.Controls.Add(this.label004);
			this.Controls.Add(this.label005);
			this.Controls.Add(this.textBox001);
			this.Controls.Add(this.label002);
			this.Controls.Add(this.label003);
			this.Controls.Add(this.ComboBox001);
			this.Controls.Add(this.label001);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.ShowInTaskbar = false;
			this.Text = "Vertical structure to CRM obstacle model exporter";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.ComboBox ComboBox001;
		public System.Windows.Forms.Label label001;
		public System.Windows.Forms.TextBox textBox001;
		public System.Windows.Forms.Label label002;
		public System.Windows.Forms.Label label003;
		public System.Windows.Forms.TextBox textBox002;
		public System.Windows.Forms.Label label004;
		public System.Windows.Forms.Label label005;
		public System.Windows.Forms.TextBox textBox003;
		public System.Windows.Forms.Label label006;
		public System.Windows.Forms.Label label007;
		private System.Windows.Forms.Button CreateBtn;
		public System.Windows.Forms.Button InfoBtn;
		private System.Windows.Forms.Button ExportBtn;
		private System.Windows.Forms.SaveFileDialog saveFileDlg;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox004;
		private System.Windows.Forms.Label label008;
		private System.Windows.Forms.Button CloseBtn;
		private System.Windows.Forms.Button SavePolylineBtn;
	}
}