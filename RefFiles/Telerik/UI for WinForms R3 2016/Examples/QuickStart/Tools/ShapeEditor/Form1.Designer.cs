namespace Telerik.Examples.WinControls.Tools.ShapeEditor
{
	partial class Form1
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
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.editShapeButton1 = new Telerik.WinControls.UI.RadButton();
            this.customShape1 = new Telerik.WinControls.CustomShape(this.components);
            this.customShape2 = new Telerik.WinControls.CustomShape(this.components);
            this.customShape3 = new Telerik.WinControls.CustomShape(this.components);
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.radGroupShape = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editShapeButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupShape)).BeginInit();
            this.radGroupShape.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupShape);
            this.settingsPanel.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Location = new System.Drawing.Point(1023, 1);
            // 
            // 
            // 
            this.settingsPanel.RootElement.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Size = new System.Drawing.Size(200, 735);
            this.settingsPanel.ThemeName = "ControlDefault";
            this.settingsPanel.Controls.SetChildIndex(this.radGroupShape, 0);
            // 
            // radButton1
            // 
            this.radButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radButton1.RootElement.BackColor = System.Drawing.Color.Transparent;
            this.radButton1.Location = new System.Drawing.Point(137, 50);
            this.radButton1.Name = "radButton1";
            // 
            // 
            // 
            this.radButton1.RootElement.ToolTipText = null;
            this.radButton1.Size = new System.Drawing.Size(130, 114);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Button";
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.radDropDownList1.Location = new System.Drawing.Point(26, 36);
            this.radDropDownList1.MaxDropDownItems = 6;
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // 
            // 
            this.radDropDownList1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radDropDownList1.RootElement.ForeColor = System.Drawing.Color.Black;
            this.radDropDownList1.RootElement.ToolTipText = null;
            this.radDropDownList1.Size = new System.Drawing.Size(126, 20);
            this.radDropDownList1.TabIndex = 6;
            this.radDropDownList1.TabStop = false;
           
            // 
            // editShapeButton1
            // 
            this.editShapeButton1.Location = new System.Drawing.Point(26, 65);
            this.editShapeButton1.Name = "editShapeButton1";
            // 
            // 
            // 
            this.editShapeButton1.RootElement.ToolTipText = null;
            this.editShapeButton1.Size = new System.Drawing.Size(126, 23);
            this.editShapeButton1.TabIndex = 10;
            this.editShapeButton1.Text = "Edit Shape";

            // 
            // customShape1
            // 
            this.customShape1.AsString = "0,0,0,0:";
            // 
            // customShape2
            // 
            this.customShape2.AsString = "0,0,224,128:0,0,False,0,0,0,0,0:224,0,False,0,0,0,0,0:192,96,False,0,0,0,0,0:32,1" +
                "28,False,0,0,0,0,0:";
            // 
            // customShape3
            // 
            this.customShape3.AsString = "0,0,192,128:0,32,True,64,0,128,0,0:192,0,False,0,0,0,0,0:192,96,True,128,128,64,1" +
                "28,0:0,128,False,0,0,0,0,0:";
            // 
            // radPanel2
            // 
            this.radPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.radPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.radPanel2.Controls.Add(this.radButton1);
            this.radPanel2.ForeColor = System.Drawing.Color.Black;
            this.radPanel2.Name = "radPanel2";
            // 
            // 
            // 
            this.radPanel2.RootElement.ForeColor = System.Drawing.Color.Black;
            this.radPanel2.Size = new System.Drawing.Size(404, 215);
            this.radPanel2.TabIndex = 19;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel2.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(235)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(235)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).NumberOfColors = 4;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).GradientPercentage = 0.3F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).GradientPercentage2 = 0.01F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(226)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(1))).GradientAngle = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(208)))), ((int)(((byte)(254)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(208)))), ((int)(((byte)(254)))));
            // 
            // roundRectShape1
            // 
            this.roundRectShape1.Radius = 10;
            // 
            // radGroupShape
            // 
            this.radGroupShape.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupShape.Controls.Add(this.radDropDownList1);
            this.radGroupShape.Controls.Add(this.editShapeButton1);
            this.radGroupShape.FooterImageIndex = -1;
            this.radGroupShape.FooterImageKey = "";
            this.radGroupShape.FooterText = "";
            this.radGroupShape.HeaderImageIndex = -1;
            this.radGroupShape.HeaderImageKey = "";
            this.radGroupShape.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupShape.HeaderText = "Shape Options";
            this.radGroupShape.Location = new System.Drawing.Point(10, 3);
            this.radGroupShape.Name = "radGroupShape";
            this.radGroupShape.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupShape.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupShape.Size = new System.Drawing.Size(180, 116);
            this.radGroupShape.TabIndex = 0;
            this.radGroupShape.Text = "Shape Options";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radPanel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1224, 737);
            this.Controls.SetChildIndex(this.radPanel2, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editShapeButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupShape)).EndInit();
            this.radGroupShape.ResumeLayout(false);
            this.radGroupShape.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadButton editShapeButton1;
        private Telerik.WinControls.CustomShape customShape1;
        private Telerik.WinControls.CustomShape customShape2;
        private Telerik.WinControls.CustomShape customShape3;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.RadGroupBox radGroupShape;
	}
}