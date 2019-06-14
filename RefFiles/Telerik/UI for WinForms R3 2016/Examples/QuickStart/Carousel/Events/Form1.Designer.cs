using System.Windows.Forms;

namespace Telerik.Examples.WinControls.Carousel.Events
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Telerik.WinControls.UI.CarouselBezierPath carouselBezierPath2 = new Telerik.WinControls.UI.CarouselBezierPath();
            this.imageList4 = new System.Windows.Forms.ImageList(this.components);
            this.radListEvents = new Telerik.WinControls.UI.RadListControl();
            this.radCarouselDemo = new Telerik.WinControls.UI.RadCarousel();
            this.radSpinEditor1 = new Telerik.WinControls.UI.RadSpinEditor();
            this.radCheckBox1 = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox2 = new Telerik.WinControls.UI.RadCheckBox();
            this.radGroupSettings = new Telerik.WinControls.UI.RadGroupBox();
            this.radLblEvents = new Telerik.WinControls.UI.RadLabel();
            this.radRadioEllipse = new Telerik.WinControls.UI.RadRadioButton();
            this.radRadioBezier = new Telerik.WinControls.UI.RadRadioButton();
            this.radLblReflectionPerc = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCarouselDemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).BeginInit();
            this.radGroupSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadioEllipse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadioBezier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblReflectionPerc)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupSettings);
            this.settingsPanel.Location = new System.Drawing.Point(1023, 1);
            // 
            // 
            // 
   
            this.settingsPanel.Size = new System.Drawing.Size(200, 550);
            this.settingsPanel.ThemeName = "ControlDefault";
            this.settingsPanel.Controls.SetChildIndex(this.radGroupSettings, 0);
            // 
            // imageList4
            // 
            this.imageList4.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList4.ImageStream")));
            this.imageList4.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList4.Images.SetKeyName(0, "Carousel01.png");
            this.imageList4.Images.SetKeyName(1, "Carousel02.png");
            this.imageList4.Images.SetKeyName(2, "Carousel03.png");
            this.imageList4.Images.SetKeyName(3, "Carousel04.png");
            this.imageList4.Images.SetKeyName(4, "Carousel05.png");
            this.imageList4.Images.SetKeyName(5, "Carousel06.png");
            this.imageList4.Images.SetKeyName(6, "Carousel07.png");
            // 
            // radListEvents
            // 
            this.radListEvents.Location = new System.Drawing.Point(11, 206);
            this.radListEvents.Name = "radListEvents";
            // 
            // 
            // 
            this.radListEvents.Size = new System.Drawing.Size(158, 272);
            this.radListEvents.TabIndex = 1;
            // 
            // radCarouselDemo
            // 
            this.radCarouselDemo.AutoLoopPauseCondition = Telerik.WinControls.UI.AutoLoopPauseConditions.OnMouseOverItem;
            this.radCarouselDemo.BackColor = System.Drawing.Color.Transparent;
            carouselBezierPath2.CtrlPoint1 = new Telerik.WinControls.UI.Point3D(125.91508052708639, 91.503267973856211, 100);
            carouselBezierPath2.CtrlPoint2 = new Telerik.WinControls.UI.Point3D(64.71449487554905, -35.62091503267974, -100);
            carouselBezierPath2.FirstPoint = new Telerik.WinControls.UI.Point3D(5.2708638360175692, 10.130718954248366, 0);
            carouselBezierPath2.LastPoint = new Telerik.WinControls.UI.Point3D(16.594516594516595, 79.950495049504951, 100);
            carouselBezierPath2.ZScale = 500;
            this.radCarouselDemo.CarouselPath = carouselBezierPath2;
            this.radCarouselDemo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCarouselDemo.EnableAutoLoop = true;
            this.radCarouselDemo.Location = new System.Drawing.Point(0, 0);
            this.radCarouselDemo.Name = "radCarouselDemo";
            // 
            // 
            // 
            this.radCarouselDemo.SelectedIndex = 0;
            this.radCarouselDemo.Size = new System.Drawing.Size(1224, 552);
            this.radCarouselDemo.TabIndex = 0;
            this.radCarouselDemo.Text = "radCarousel1";
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarouselDemo.GetChildAt(0).GetChildAt(3))).Image = global::Telerik.Examples.WinControls.Properties.Resources.carousel_leftArrow;
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarouselDemo.GetChildAt(0).GetChildAt(4))).Image = global::Telerik.Examples.WinControls.Properties.Resources.carousel_rightArrow;
            // 
            // radSpinEditor1
            // 
            this.radSpinEditor1.DecimalPlaces = 2;

            this.radSpinEditor1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.radSpinEditor1.Location = new System.Drawing.Point(11, 91);
            this.radSpinEditor1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.radSpinEditor1.Name = "radSpinEditor1";
            // 
            // 
            // 
            this.radSpinEditor1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren; 
            this.radSpinEditor1.ShowBorder = true;
            this.radSpinEditor1.Size = new System.Drawing.Size(158, 20);
            this.radSpinEditor1.TabIndex = 7;
            this.radSpinEditor1.Value = new decimal(new int[] {
            33,
            0,
            0,
            131072});      
            // 
            // radCheckBox1
            // 
            this.radCheckBox1.Location = new System.Drawing.Point(11, 117);
            this.radCheckBox1.Name = "radCheckBox1";
            this.radCheckBox1.Size = new System.Drawing.Size(101, 17);
            this.radCheckBox1.TabIndex = 10;
            this.radCheckBox1.Text = "Enable Looping";
            this.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
           
            // 
            // radCheckBox2
            // 
            this.radCheckBox2.Location = new System.Drawing.Point(11, 140);
            this.radCheckBox2.Name = "radCheckBox2";
            // 
            // 
            // 
            this.radCheckBox2.Size = new System.Drawing.Size(112, 17);
            this.radCheckBox2.TabIndex = 11;
            this.radCheckBox2.Text = "Enable Auto Loop";
            this.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
      
            // 
            // radGroupSettings
            // 
            this.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupSettings.Controls.Add(this.radLblEvents);
            this.radGroupSettings.Controls.Add(this.radRadioEllipse);
            this.radGroupSettings.Controls.Add(this.radRadioBezier);
            this.radGroupSettings.Controls.Add(this.radLblReflectionPerc);
            this.radGroupSettings.Controls.Add(this.radListEvents);
            this.radGroupSettings.Controls.Add(this.radCheckBox2);
            this.radGroupSettings.Controls.Add(this.radCheckBox1);
            this.radGroupSettings.Controls.Add(this.radSpinEditor1);
            this.radGroupSettings.FooterImageIndex = -1;
            this.radGroupSettings.FooterImageKey = "";
            this.radGroupSettings.FooterText = "";
      
            this.radGroupSettings.HeaderImageIndex = -1;
            this.radGroupSettings.HeaderImageKey = "";
            this.radGroupSettings.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupSettings.HeaderText = "Carousel Settings";
            this.radGroupSettings.Location = new System.Drawing.Point(10, 6);
            this.radGroupSettings.Name = "radGroupSettings";
            // 
            // 
            // 
     
            this.radGroupSettings.Size = new System.Drawing.Size(180, 493);
            this.radGroupSettings.TabIndex = 1;
            this.radGroupSettings.Text = "Carousel Settings";
            // 
            // radLblEvents
            // 

            this.radLblEvents.Location = new System.Drawing.Point(11, 186);
            this.radLblEvents.Name = "radLblEvents";
            // 
            // 
            // 
 
            this.radLblEvents.Size = new System.Drawing.Size(42, 14);
            this.radLblEvents.TabIndex = 13;
            this.radLblEvents.Text = "Events:";
            // 
            // radRadioEllipse
            // 
        
            this.radRadioEllipse.Location = new System.Drawing.Point(11, 49);
            this.radRadioEllipse.Name = "radRadioEllipse";
            this.radRadioEllipse.RadioCheckAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 
            // 
        
            this.radRadioEllipse.Size = new System.Drawing.Size(80, 16);
            this.radRadioEllipse.TabIndex = 12;
            this.radRadioEllipse.Text = "Ellipse Path";
            // 
            // radRadioBezier
            // 
    
            this.radRadioBezier.Location = new System.Drawing.Point(11, 27);
            this.radRadioBezier.Name = "radRadioBezier";
            this.radRadioBezier.RadioCheckAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // 
            // 
    
            this.radRadioBezier.Size = new System.Drawing.Size(79, 16);
            this.radRadioBezier.TabIndex = 12;
            this.radRadioBezier.Text = "Bezier Path";
          
            // 
            // radLblReflectionPerc
            // 

            this.radLblReflectionPerc.Location = new System.Drawing.Point(11, 71);
            this.radLblReflectionPerc.Name = "radLblReflectionPerc";
            // 
            // 
            // 
      
            this.radLblReflectionPerc.Size = new System.Drawing.Size(144, 14);
            this.radLblReflectionPerc.TabIndex = 1;
            this.radLblReflectionPerc.Text = "Item Reflection Percentage:";
            // 
            // Form1
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.radCarouselDemo);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1224, 552);

            this.Controls.SetChildIndex(this.radCarouselDemo, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCarouselDemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).EndInit();
            this.radGroupSettings.ResumeLayout(false);
            this.radGroupSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadioEllipse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRadioBezier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblReflectionPerc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCarousel radCarouselDemo;
        private System.Windows.Forms.ImageList imageList4;
        private Telerik.WinControls.UI.RadListControl radListEvents;
        private Telerik.WinControls.UI.RadSpinEditor radSpinEditor1;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox1;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox2;
        private Telerik.WinControls.UI.RadGroupBox radGroupSettings;
        private Telerik.WinControls.UI.RadLabel radLblReflectionPerc;
        private Telerik.WinControls.UI.RadRadioButton radRadioEllipse;
        private Telerik.WinControls.UI.RadRadioButton radRadioBezier;
        private Telerik.WinControls.UI.RadLabel radLblEvents;
    }
}