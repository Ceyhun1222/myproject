namespace Aran.PANDA.RNAV.SGBAS
{
	partial class ExcludeObstForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcludeObstForm));
			this.OKbtn = new System.Windows.Forms.Button();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._ListView1_ColumnHeader_3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._ListView1_ColumnHeader_2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._ListView1_ColumnHeader_1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ListView1 = new System.Windows.Forms.ListView();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// OKbtn
			// 
			this.OKbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKbtn.Location = new System.Drawing.Point(322, 314);
			this.OKbtn.Name = "OKbtn";
			this.OKbtn.Size = new System.Drawing.Size(93, 25);
			this.OKbtn.TabIndex = 11;
			this.OKbtn.Text = "&OK";
			this.OKbtn.Click += new System.EventHandler(this.OKbtn_Click);
			// 
			// _ListView1_ColumnHeader_3
			// 
			this._ListView1_ColumnHeader_3.Text = "H. abv. DER";
			this._ListView1_ColumnHeader_3.Width = 170;
			// 
			// _ListView1_ColumnHeader_2
			// 
			this._ListView1_ColumnHeader_2.Text = "ID";
			this._ListView1_ColumnHeader_2.Width = 170;
			// 
			// _ListView1_ColumnHeader_1
			// 
			this._ListView1_ColumnHeader_1.Text = "Name";
			this._ListView1_ColumnHeader_1.Width = 170;
			// 
			// ListView1
			// 
			this.ListView1.BackColor = System.Drawing.SystemColors.Window;
			this.ListView1.CheckBoxes = true;
			this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._ListView1_ColumnHeader_1,
            this._ListView1_ColumnHeader_2,
            this._ListView1_ColumnHeader_3});
			this.ListView1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ListView1.FullRowSelect = true;
			this.ListView1.GridLines = true;
			this.ListView1.LabelWrap = false;
			this.ListView1.Location = new System.Drawing.Point(4, 5);
			this.ListView1.Name = "ListView1";
			this.ListView1.Size = new System.Drawing.Size(513, 303);
			this.ListView1.TabIndex = 9;
			this.ListView1.UseCompatibleStateImageBehavior = false;
			this.ListView1.View = System.Windows.Forms.View.Details;
			this.ListView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView1_ColumnClick);
			this.ListView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(422, 314);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(93, 25);
			this.CancelBtn.TabIndex = 10;
			this.CancelBtn.Text = "&Cancel";
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// ExcludeObstFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(522, 345);
			this.Controls.Add(this.OKbtn);
			this.Controls.Add(this.ListView1);
			this.Controls.Add(this.CancelBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExcludeObstFrm";
			this.ShowInTaskbar = false;
			this.Text = "Exclude Ostacles";
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Button OKbtn;
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.ColumnHeader _ListView1_ColumnHeader_3;
		public System.Windows.Forms.ColumnHeader _ListView1_ColumnHeader_2;
		public System.Windows.Forms.ColumnHeader _ListView1_ColumnHeader_1;
		public System.Windows.Forms.ListView ListView1;
		public System.Windows.Forms.Button CancelBtn;
	}
}