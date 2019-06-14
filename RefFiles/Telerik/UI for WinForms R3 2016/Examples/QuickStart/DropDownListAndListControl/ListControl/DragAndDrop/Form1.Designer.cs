using System;
using Telerik.WinControls;
namespace Telerik.Examples.WinControls.DropDownListAndListControl.ListControl.DragAndDrop
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.radListBox1 = new Telerik.WinControls.UI.RadListControl();
            this.radListBox2 = new Telerik.WinControls.UI.RadListControl();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.radButton7 = new Telerik.WinControls.UI.RadButton();
            this.radButton8 = new Telerik.WinControls.UI.RadButton();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.radButton6 = new Telerik.WinControls.UI.RadButton();
            this.radPanelDemoHolder = new Telerik.WinControls.UI.RadPanel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).BeginInit();
            this.radPanelDemoHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Location = new System.Drawing.Point(1023, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 818);
            this.settingsPanel.ThemeName = "ControlDefault";
            // 
            // radListBox1
            // 
            this.radListBox1.FormatString = "{0}";
            radListDataItem1.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_m4;
            radListDataItem1.Text = "Tuna Salad";
            radListDataItem1.TextWrap = true;
            radListDataItem2.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_m3;
            radListDataItem2.Text = "Grilled Salmon ";
            radListDataItem2.TextWrap = true;
            radListDataItem3.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_m2;
            radListDataItem3.Text = "Sashimi Salad";
            radListDataItem3.TextWrap = true;
            radListDataItem4.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_m1;
            radListDataItem4.Text = "Fresh Mussels";
            radListDataItem4.TextWrap = true;
            this.radListBox1.Items.Add(radListDataItem1);
            this.radListBox1.Items.Add(radListDataItem2);
            this.radListBox1.Items.Add(radListDataItem3);
            this.radListBox1.Items.Add(radListDataItem4);
            this.radListBox1.Location = new System.Drawing.Point(45, 176);
            this.radListBox1.Name = "radListBox1";
            this.radListBox1.Size = new System.Drawing.Size(255, 255);
            this.radListBox1.TabIndex = 1;
            ((Telerik.WinControls.UI.RadListElement)(this.radListBox1.GetChildAt(0))).Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ((Telerik.WinControls.UI.RadListElement)(this.radListBox1.GetChildAt(0))).Opacity = 0.8D;
            // 
            // radListBox2
            // 
            this.radListBox2.FormatString = "{0}";
            this.radListBox2.Location = new System.Drawing.Point(462, 176);
            this.radListBox2.Name = "radListBox2";
            this.radListBox2.Size = new System.Drawing.Size(255, 255);
            this.radListBox2.TabIndex = 2;
            ((Telerik.WinControls.UI.RadListElement)(this.radListBox2.GetChildAt(0))).Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ((Telerik.WinControls.UI.RadListElement)(this.radListBox2.GetChildAt(0))).Opacity = 0.8D;
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(327, 298);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(103, 24);
            this.radButton1.TabIndex = 5;
            this.radButton1.Text = "Include Selected";
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(327, 329);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(103, 24);
            this.radButton2.TabIndex = 6;
            this.radButton2.Text = "Exclude Selected";
            // 
            // radButton3
            // 
            this.radButton3.Location = new System.Drawing.Point(327, 360);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(103, 24);
            this.radButton3.TabIndex = 7;
            this.radButton3.Text = "Include All";
            // 
            // radButton4
            // 
            this.radButton4.Location = new System.Drawing.Point(327, 391);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(103, 24);
            this.radButton4.TabIndex = 8;
            this.radButton4.Text = "Exclude All";
            // 
            // radButton7
            // 
            this.radButton7.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_arr_up;
            this.radButton7.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButton7.Location = new System.Drawing.Point(327, 191);
            this.radButton7.Name = "radButton7";
            this.radButton7.Size = new System.Drawing.Size(26, 25);
            this.radButton7.TabIndex = 11;
            // 
            // radButton8
            // 
            this.radButton8.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_arr_down;
            this.radButton8.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButton8.Location = new System.Drawing.Point(327, 222);
            this.radButton8.Name = "radButton8";
            this.radButton8.Size = new System.Drawing.Size(26, 25);
            this.radButton8.TabIndex = 12;
            // 
            // radButton5
            // 
            this.radButton5.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_arr_up;
            this.radButton5.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButton5.Location = new System.Drawing.Point(738, 191);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(26, 25);
            this.radButton5.TabIndex = 13;
            // 
            // radButton6
            // 
            this.radButton6.Image = global::Telerik.Examples.WinControls.Properties.Resources.lb_arr_down;
            this.radButton6.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButton6.Location = new System.Drawing.Point(738, 222);
            this.radButton6.Name = "radButton6";
            this.radButton6.Size = new System.Drawing.Size(26, 25);
            this.radButton6.TabIndex = 14;
            // 
            // radPanelDemoHolder
            // 
            this.radPanelDemoHolder.Controls.Add(this.radPanel1);
            this.radPanelDemoHolder.ForeColor = System.Drawing.Color.Black;
            this.radPanelDemoHolder.Location = new System.Drawing.Point(0, 0);
            this.radPanelDemoHolder.Name = "radPanelDemoHolder";
            this.radPanelDemoHolder.Size = new System.Drawing.Size(811, 485);
            this.radPanelDemoHolder.TabIndex = 15;
            // 
            // radPanel1
            // 
            this.radPanel1.BackgroundImage = global::Telerik.Examples.WinControls.Properties.Resources.lb_bg;
            this.radPanel1.Controls.Add(this.radButton5);
            this.radPanel1.Controls.Add(this.radButton6);
            this.radPanel1.Controls.Add(this.radListBox1);
            this.radPanel1.Controls.Add(this.radListBox2);
            this.radPanel1.Controls.Add(this.radButton3);
            this.radPanel1.Controls.Add(this.radButton7);
            this.radPanel1.Controls.Add(this.radButton1);
            this.radPanel1.Controls.Add(this.radButton8);
            this.radPanel1.Controls.Add(this.radButton4);
            this.radPanel1.Controls.Add(this.radButton2);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(811, 485);
            this.radPanel1.TabIndex = 15;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelDemoHolder);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1142, 599);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            this.Controls.SetChildIndex(this.radPanelDemoHolder, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).EndInit();
            this.radPanelDemoHolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadListControl radListBox1;

        private Telerik.WinControls.UI.RadListControl radListBox2;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadButton radButton7;
        private Telerik.WinControls.UI.RadButton radButton8;
        private Telerik.WinControls.UI.RadButton radButton5;
        private Telerik.WinControls.UI.RadButton radButton6;
        private Telerik.WinControls.UI.RadPanel radPanelDemoHolder;
        private Telerik.WinControls.UI.RadPanel radPanel1;
    }
}
