
namespace Aran.PANDA.RNAV.DMECoverage
{
  partial class AddDMEForm
    {
        public System.Windows.Forms.Label Label6;
        public System.Windows.Forms.GroupBox GroupBox1;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label3;
        public System.Windows.Forms.Label Label4;
        public System.Windows.Forms.Label Label5;
        public System.Windows.Forms.TextBox Edit1;
        public System.Windows.Forms.TextBox Edit2;
        public System.Windows.Forms.TextBox Edit3;
        public System.Windows.Forms.ComboBox cbY;
        public System.Windows.Forms.TextBox Edit4;
        public System.Windows.Forms.TextBox Edit5;
        public System.Windows.Forms.TextBox Edit6;
        public System.Windows.Forms.ComboBox cbX;
        public System.Windows.Forms.CheckBox CheckBox1;
        public System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.TextBox Edit7;
        private System.Windows.Forms.ToolTip toolTip1 = null;

        // Clean up any resources being used.
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

#region Windows Form Designer generated code
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDMEForm));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.Label6 = new System.Windows.Forms.Label();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Edit1 = new System.Windows.Forms.TextBox();
			this.Edit2 = new System.Windows.Forms.TextBox();
			this.Edit3 = new System.Windows.Forms.TextBox();
			this.cbY = new System.Windows.Forms.ComboBox();
			this.Edit4 = new System.Windows.Forms.TextBox();
			this.Edit5 = new System.Windows.Forms.TextBox();
			this.Edit6 = new System.Windows.Forms.TextBox();
			this.cbX = new System.Windows.Forms.ComboBox();
			this.CheckBox1 = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.Edit7 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.GroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Label6
			// 
			this.Label6.Location = new System.Drawing.Point(8, 9);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(43, 13);
			this.Label6.TabIndex = 0;
			this.Label6.Text = "Call sign:";
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.Edit1);
			this.GroupBox1.Controls.Add(this.Edit2);
			this.GroupBox1.Controls.Add(this.Edit3);
			this.GroupBox1.Controls.Add(this.cbY);
			this.GroupBox1.Controls.Add(this.Edit4);
			this.GroupBox1.Controls.Add(this.Edit5);
			this.GroupBox1.Controls.Add(this.Edit6);
			this.GroupBox1.Controls.Add(this.cbX);
			this.GroupBox1.Location = new System.Drawing.Point(3, 33);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(353, 97);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "DME coordinates";
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(16, 37);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(43, 13);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Latitude:";
			// 
			// Label2
			// 
			this.Label2.Location = new System.Drawing.Point(14, 72);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(51, 13);
			this.Label2.TabIndex = 1;
			this.Label2.Text = "Longitude:";
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(95, 16);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(23, 13);
			this.Label3.TabIndex = 2;
			this.Label3.Text = "Deg.";
			// 
			// Label4
			// 
			this.Label4.Location = new System.Drawing.Point(168, 16);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(20, 13);
			this.Label4.TabIndex = 3;
			this.Label4.Text = "Min.";
			// 
			// Label5
			// 
			this.Label5.Location = new System.Drawing.Point(238, 16);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(31, 13);
			this.Label5.TabIndex = 4;
			this.Label5.Text = "Sec.ss";
			// 
			// Edit1
			// 
			this.Edit1.Location = new System.Drawing.Point(72, 33);
			this.Edit1.Name = "Edit1";
			this.Edit1.Size = new System.Drawing.Size(65, 20);
			this.Edit1.TabIndex = 0;
			this.Edit1.Text = "0";
			// 
			// Edit2
			// 
			this.Edit2.Location = new System.Drawing.Point(146, 33);
			this.Edit2.Name = "Edit2";
			this.Edit2.Size = new System.Drawing.Size(65, 20);
			this.Edit2.TabIndex = 1;
			this.Edit2.Text = "0";
			// 
			// Edit3
			// 
			this.Edit3.Location = new System.Drawing.Point(220, 33);
			this.Edit3.Name = "Edit3";
			this.Edit3.Size = new System.Drawing.Size(65, 20);
			this.Edit3.TabIndex = 2;
			this.Edit3.Text = "0";
			// 
			// cbY
			// 
			this.cbY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbY.ItemHeight = 13;
			this.cbY.Items.AddRange(new object[] {
            "N",
            "S"});
			this.cbY.Location = new System.Drawing.Point(295, 33);
			this.cbY.Name = "cbY";
			this.cbY.Size = new System.Drawing.Size(52, 21);
			this.cbY.TabIndex = 3;
			// 
			// Edit4
			// 
			this.Edit4.Location = new System.Drawing.Point(72, 68);
			this.Edit4.Name = "Edit4";
			this.Edit4.Size = new System.Drawing.Size(65, 20);
			this.Edit4.TabIndex = 4;
			this.Edit4.Text = "0";
			// 
			// Edit5
			// 
			this.Edit5.Location = new System.Drawing.Point(146, 68);
			this.Edit5.Name = "Edit5";
			this.Edit5.Size = new System.Drawing.Size(65, 20);
			this.Edit5.TabIndex = 5;
			this.Edit5.Text = "0";
			// 
			// Edit6
			// 
			this.Edit6.Location = new System.Drawing.Point(220, 68);
			this.Edit6.Name = "Edit6";
			this.Edit6.Size = new System.Drawing.Size(65, 20);
			this.Edit6.TabIndex = 6;
			this.Edit6.Text = "0";
			// 
			// cbX
			// 
			this.cbX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbX.ItemHeight = 13;
			this.cbX.Items.AddRange(new object[] {
            "E",
            "W"});
			this.cbX.Location = new System.Drawing.Point(295, 68);
			this.cbX.Name = "cbX";
			this.cbX.Size = new System.Drawing.Size(52, 21);
			this.cbX.TabIndex = 7;
			// 
			// CheckBox1
			// 
			this.CheckBox1.Location = new System.Drawing.Point(24, 138);
			this.CheckBox1.Name = "CheckBox1";
			this.CheckBox1.Size = new System.Drawing.Size(73, 17);
			this.CheckBox1.TabIndex = 1;
			this.CheckBox1.Text = "By Click";
			this.CheckBox1.Visible = false;
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(187, 136);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 25);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.btnOKClick);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(275, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			// 
			// Edit7
			// 
			this.Edit7.Location = new System.Drawing.Point(64, 5);
			this.Edit7.Name = "Edit7";
			this.Edit7.Size = new System.Drawing.Size(81, 20);
			this.Edit7.TabIndex = 4;
			this.Edit7.Text = "XDME";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(169, 9);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(55, 13);
			this.label7.TabIndex = 5;
			this.label7.Text = "Elevation:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(230, 5);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(81, 20);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "120";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(317, 9);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(27, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "feet";
			// 
			// AddDMEForm
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.ClientSize = new System.Drawing.Size(358, 166);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.Label6);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.CheckBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.Edit7);
			this.Font = new System.Drawing.Font("Tahoma", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(571, 259);
			this.Name = "AddDMEForm";
			this.ShowInTaskbar = false;
			this.Text = "Add new DME";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
#endregion

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label8;

    }
}
