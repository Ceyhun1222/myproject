namespace Aran.Aim.FmdEditor
{
	partial class DataQualityControl
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			this.ui_attributesTB = new System.Windows.Forms.TextBox();
			this.ui_dataQualityElementGrB = new System.Windows.Forms.GroupBox();
			this.ui_passChB = new System.Windows.Forms.CheckBox();
			this.ui_evaluationMethodTypeCB = new System.Windows.Forms.ComboBox();
			this.ui_evaluationMethodNameTB = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			this.ui_dataQualityElementGrB.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 15);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(54, 13);
			label1.TabIndex = 0;
			label1.Text = "Attributes:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(11, 22);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(130, 13);
			label2.TabIndex = 0;
			label2.Text = "Evaluation Method Name:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(11, 53);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(126, 13);
			label3.TabIndex = 2;
			label3.Text = "Evaluation Method Type:";
			// 
			// ui_attributesTB
			// 
			this.ui_attributesTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_attributesTB.Location = new System.Drawing.Point(155, 12);
			this.ui_attributesTB.Name = "ui_attributesTB";
			this.ui_attributesTB.Size = new System.Drawing.Size(194, 20);
			this.ui_attributesTB.TabIndex = 1;
			// 
			// ui_dataQualityElementGrB
			// 
			this.ui_dataQualityElementGrB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dataQualityElementGrB.Controls.Add(this.ui_passChB);
			this.ui_dataQualityElementGrB.Controls.Add(this.ui_evaluationMethodTypeCB);
			this.ui_dataQualityElementGrB.Controls.Add(label3);
			this.ui_dataQualityElementGrB.Controls.Add(this.ui_evaluationMethodNameTB);
			this.ui_dataQualityElementGrB.Controls.Add(label2);
			this.ui_dataQualityElementGrB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ui_dataQualityElementGrB.Location = new System.Drawing.Point(8, 41);
			this.ui_dataQualityElementGrB.Name = "ui_dataQualityElementGrB";
			this.ui_dataQualityElementGrB.Size = new System.Drawing.Size(351, 109);
			this.ui_dataQualityElementGrB.TabIndex = 2;
			this.ui_dataQualityElementGrB.TabStop = false;
			this.ui_dataQualityElementGrB.Text = "Report";
			// 
			// ui_passChB
			// 
			this.ui_passChB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ui_passChB.Location = new System.Drawing.Point(12, 78);
			this.ui_passChB.Name = "ui_passChB";
			this.ui_passChB.Size = new System.Drawing.Size(149, 23);
			this.ui_passChB.TabIndex = 4;
			this.ui_passChB.Text = "Pass: ";
			this.ui_passChB.ThreeState = true;
			this.ui_passChB.UseVisualStyleBackColor = true;
			// 
			// ui_evaluationMethodTypeCB
			// 
			this.ui_evaluationMethodTypeCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_evaluationMethodTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_evaluationMethodTypeCB.FormattingEnabled = true;
			this.ui_evaluationMethodTypeCB.Location = new System.Drawing.Point(147, 50);
			this.ui_evaluationMethodTypeCB.Name = "ui_evaluationMethodTypeCB";
			this.ui_evaluationMethodTypeCB.Size = new System.Drawing.Size(194, 21);
			this.ui_evaluationMethodTypeCB.TabIndex = 3;
			// 
			// ui_evaluationMethodNameTB
			// 
			this.ui_evaluationMethodNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_evaluationMethodNameTB.Location = new System.Drawing.Point(147, 19);
			this.ui_evaluationMethodNameTB.Name = "ui_evaluationMethodNameTB";
			this.ui_evaluationMethodNameTB.Size = new System.Drawing.Size(194, 20);
			this.ui_evaluationMethodNameTB.TabIndex = 1;
			// 
			// DataQualityControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ui_dataQualityElementGrB);
			this.Controls.Add(this.ui_attributesTB);
			this.Controls.Add(label1);
			this.Name = "DataQualityControl";
			this.Size = new System.Drawing.Size(364, 154);
			this.ui_dataQualityElementGrB.ResumeLayout(false);
			this.ui_dataQualityElementGrB.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ui_attributesTB;
		private System.Windows.Forms.GroupBox ui_dataQualityElementGrB;
		private System.Windows.Forms.TextBox ui_evaluationMethodNameTB;
		private System.Windows.Forms.CheckBox ui_passChB;
		private System.Windows.Forms.ComboBox ui_evaluationMethodTypeCB;
	}
}
