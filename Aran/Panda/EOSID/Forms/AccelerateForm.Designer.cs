namespace EOSID
{
	partial class AccelerateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccelerateForm));
			this.label01 = new System.Windows.Forms.Label();
			this.textBox01 = new System.Windows.Forms.TextBox();
			this.label02 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label03 = new System.Windows.Forms.Label();
			this.label04 = new System.Windows.Forms.Label();
			this.textBox02 = new System.Windows.Forms.TextBox();
			this.label06 = new System.Windows.Forms.Label();
			this.textBox03 = new System.Windows.Forms.TextBox();
			this.label05 = new System.Windows.Forms.Label();
			this.textBox04 = new System.Windows.Forms.TextBox();
			this.label07 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label01
			// 
			resources.ApplyResources(this.label01, "label01");
			this.label01.Name = "label01";
			// 
			// textBox01
			// 
			resources.ApplyResources(this.textBox01, "textBox01");
			this.textBox01.Name = "textBox01";
			this.textBox01.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox01_KeyPress);
			this.textBox01.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
			this.textBox01.Validating += new System.ComponentModel.CancelEventHandler(this.textBox01_Validating);
			// 
			// label02
			// 
			resources.ApplyResources(this.label02, "label02");
			this.label02.Name = "label02";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label03
			// 
			resources.ApplyResources(this.label03, "label03");
			this.label03.Name = "label03";
			// 
			// label04
			// 
			resources.ApplyResources(this.label04, "label04");
			this.label04.Name = "label04";
			// 
			// textBox02
			// 
			resources.ApplyResources(this.textBox02, "textBox02");
			this.textBox02.Name = "textBox02";
			this.textBox02.ReadOnly = true;
			// 
			// label06
			// 
			resources.ApplyResources(this.label06, "label06");
			this.label06.Name = "label06";
			// 
			// textBox03
			// 
			resources.ApplyResources(this.textBox03, "textBox03");
			this.textBox03.Name = "textBox03";
			this.textBox03.ReadOnly = true;
			// 
			// label05
			// 
			resources.ApplyResources(this.label05, "label05");
			this.label05.Name = "label05";
			// 
			// textBox04
			// 
			resources.ApplyResources(this.textBox04, "textBox04");
			this.textBox04.Name = "textBox04";
			this.textBox04.ReadOnly = true;
			// 
			// label07
			// 
			resources.ApplyResources(this.label07, "label07");
			this.label07.Name = "label07";
			// 
			// AccelerateForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.textBox04);
			this.Controls.Add(this.label07);
			this.Controls.Add(this.label06);
			this.Controls.Add(this.textBox03);
			this.Controls.Add(this.label05);
			this.Controls.Add(this.label04);
			this.Controls.Add(this.textBox02);
			this.Controls.Add(this.label03);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label02);
			this.Controls.Add(this.textBox01);
			this.Controls.Add(this.label01);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AccelerateForm";
			this.ShowInTaskbar = false;
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AccelerateForm_KeyUp);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AccelerateForm_PreviewKeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label01;
		private System.Windows.Forms.TextBox textBox01;
		private System.Windows.Forms.Label label02;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label03;
		private System.Windows.Forms.Label label04;
		private System.Windows.Forms.TextBox textBox02;
		private System.Windows.Forms.Label label06;
		private System.Windows.Forms.TextBox textBox03;
		private System.Windows.Forms.Label label05;
		private System.Windows.Forms.TextBox textBox04;
		private System.Windows.Forms.Label label07;
	}
}