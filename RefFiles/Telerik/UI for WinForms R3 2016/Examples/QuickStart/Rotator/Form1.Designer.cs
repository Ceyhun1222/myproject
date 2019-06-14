namespace Telerik.Examples.WinControls.Rotator
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
            this.radRotator1 = new Telerik.WinControls.UI.RadRotator();
            this.radItemsContainer1 = new Telerik.WinControls.UI.RadItemsContainer();
            this.radImageItem1 = new Telerik.WinControls.UI.RadImageItem();
            this.radItemsContainer2 = new Telerik.WinControls.UI.RadItemsContainer();
            this.radButtonElement1 = new Telerik.WinControls.UI.RadButtonElement();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.radTextBoxElement1 = new Telerik.WinControls.UI.RadTextBoxElement();
            this.radWebBrowserItem1 = new Telerik.WinControls.UI.RadWebBrowserItem();
            this.radButtonPrevious = new Telerik.WinControls.UI.RadButton();
            this.radButtonNext = new Telerik.WinControls.UI.RadButton();
            this.radStartStopButton = new Telerik.WinControls.UI.RadButton();
            this.currentFrameTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radLabelInterval = new Telerik.WinControls.UI.RadLabel();
            this.intervalTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radCheckBoxOpacity = new Telerik.WinControls.UI.RadCheckBox();
            this.locationAnimationTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radLabelMsec = new Telerik.WinControls.UI.RadLabel();
            this.radLabelLocationAnimation = new Telerik.WinControls.UI.RadLabel();
            this.radButtonApply = new Telerik.WinControls.UI.RadButton();
            this.radGroupSettings = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRotator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStartStopButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentFrameTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intervalTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationAnimationTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMsec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelLocationAnimation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).BeginInit();
            this.radGroupSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupSettings);
            this.settingsPanel.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Location = new System.Drawing.Point(1023, 1);
            // 
            // 
            // 
            this.settingsPanel.RootElement.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Size = new System.Drawing.Size(200, 818);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupSettings, 0);
            // 
            // radRotator1
            // 
            this.radRotator1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radRotator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.radRotator1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radItemsContainer1,
            this.radItemsContainer2,
            this.radWebBrowserItem1});
            this.radRotator1.Location = new System.Drawing.Point(3, 3);
            this.radRotator1.LocationAnimation = new System.Drawing.SizeF(0F, -1F);
            this.radRotator1.Name = "radRotator1";
            // 
            // 
            // 
            this.radRotator1.RootElement.ForeColor = System.Drawing.Color.Black;
            this.radRotator1.Running = false;
            this.radRotator1.Size = new System.Drawing.Size(1218, 788);
            this.radRotator1.TabIndex = 0;
            this.radRotator1.Text = "radRotator1";

            // 
            // radItemsContainer1
            // 
            this.radItemsContainer1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radImageItem1});
            this.radItemsContainer1.Name = "radItemsContainer1";
            this.radItemsContainer1.Text = "radItemsContainer1";
            this.radItemsContainer1.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radImageItem1
            // 
            this.radImageItem1.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radImageItem1.Image = global::Telerik.Examples.WinControls.Properties.Resources.RadRotatorWinF;
            this.radImageItem1.Name = "radImageItem1";
            this.radImageItem1.StretchHorizontally = false;
            this.radImageItem1.StretchVertically = false;
            this.radImageItem1.Text = "radImageItem1";
            // 
            // radItemsContainer2
            // 
            this.radItemsContainer2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElement1,
            this.radLabelElement1,
            this.radTextBoxElement1});
            this.radItemsContainer2.Name = "radItemsContainer2";
            this.radItemsContainer2.Text = "radItemsContainer2";
            this.radItemsContainer2.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radButtonElement1
            // 
            this.radButtonElement1.CanFocus = true;
            this.radButtonElement1.Name = "radButtonElement1";
            this.radButtonElement1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.radButtonElement1.StretchHorizontally = false;
            this.radButtonElement1.StretchVertically = false;
            this.radButtonElement1.Text = "Previous frame";
        
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radLabelElement1.Font = new System.Drawing.Font("Calibri", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabelElement1.Name = "radLabelElement1";
            this.radLabelElement1.Opacity = 0.25;
            this.radLabelElement1.StretchHorizontally = false;
            this.radLabelElement1.StretchVertically = false;
            this.radLabelElement1.Text = "Second rotator frame";
            this.radLabelElement1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabelElement1.TextWrap = true;
            // 
            // radTextBoxElement1
            // 
            this.radTextBoxElement1.Alignment = System.Drawing.ContentAlignment.BottomRight;
            this.radTextBoxElement1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            this.radTextBoxElement1.BackColor = System.Drawing.Color.Black;
            this.radTextBoxElement1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTextBoxElement1.ForeColor = System.Drawing.Color.White;
            this.radTextBoxElement1.MinSize = new System.Drawing.Size(500, 0);
            this.radTextBoxElement1.Name = "radTextBoxElement1";
            this.radTextBoxElement1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 3);
            this.radTextBoxElement1.StretchHorizontally = false;
            this.radTextBoxElement1.StretchVertically = false;
            // 
            // radWebBrowserItem1
            // 
            this.radWebBrowserItem1.CanFocus = true;
            this.radWebBrowserItem1.DocumentText = "<HTML></HTML>\0";
            this.radWebBrowserItem1.Name = "radWebBrowserItem1";
            this.radWebBrowserItem1.Text = "radWebBrowserItem1";
            this.radWebBrowserItem1.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.radWebBrowserItem1.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radButtonPrevious
            // 
            this.radButtonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButtonPrevious.Location = new System.Drawing.Point(1089, 797);
            this.radButtonPrevious.Name = "radButtonPrevious";
            this.radButtonPrevious.ShowItemToolTips = false;
            this.radButtonPrevious.Size = new System.Drawing.Size(40, 20);
            this.radButtonPrevious.TabIndex = 1;
            this.radButtonPrevious.Text = "|<";
          
            // 
            // radButtonNext
            // 
            this.radButtonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButtonNext.Location = new System.Drawing.Point(1181, 797);
            this.radButtonNext.Name = "radButtonNext";
            this.radButtonNext.Size = new System.Drawing.Size(40, 20);
            this.radButtonNext.TabIndex = 2;
            this.radButtonNext.Text = ">|";
        
            // 
            // radStartStopButton
            // 
            this.radStartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radStartStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radStartStopButton.Location = new System.Drawing.Point(1135, 797);
            this.radStartStopButton.Name = "radStartStopButton";
            this.radStartStopButton.Size = new System.Drawing.Size(40, 20);
            this.radStartStopButton.TabIndex = 3;
 
            // 
            // currentFrameTextBox
            // 
            this.currentFrameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFrameTextBox.Location = new System.Drawing.Point(1045, 797);
            this.currentFrameTextBox.Name = "currentFrameTextBox";
            // 
            // 
            // 
            this.currentFrameTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.currentFrameTextBox.Size = new System.Drawing.Size(40, 20);
            this.currentFrameTextBox.TabIndex = 4;
            this.currentFrameTextBox.TabStop = false;
          
            // 
            // radLabelInterval
            // 
            this.radLabelInterval.Location = new System.Drawing.Point(9, 27);
            this.radLabelInterval.Name = "radLabelInterval";
            this.radLabelInterval.Size = new System.Drawing.Size(43, 16);
            this.radLabelInterval.TabIndex = 5;
            this.radLabelInterval.Text = "Interval";
            this.radLabelInterval.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Location = new System.Drawing.Point(30, 47);
            this.intervalTextBox.MaxLength = 5;
            this.intervalTextBox.Name = "intervalTextBox";
            // 
            // 
            // 
            this.intervalTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.intervalTextBox.Size = new System.Drawing.Size(36, 20);
            this.intervalTextBox.TabIndex = 6;
            this.intervalTextBox.TabStop = false;
            this.intervalTextBox.Text = "2000";
            // 
            // radCheckBoxOpacity
            // 
            this.radCheckBoxOpacity.Location = new System.Drawing.Point(9, 119);
            this.radCheckBoxOpacity.Name = "radCheckBoxOpacity";
            this.radCheckBoxOpacity.Size = new System.Drawing.Size(114, 17);
            this.radCheckBoxOpacity.TabIndex = 7;
            this.radCheckBoxOpacity.Text = "Opacity Animation";
            this.radCheckBoxOpacity.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
          
            // 
            // locationAnimationTextBox
            // 
            this.locationAnimationTextBox.Location = new System.Drawing.Point(30, 93);
            this.locationAnimationTextBox.Name = "locationAnimationTextBox";
            // 
            // 
            // 
            this.locationAnimationTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.locationAnimationTextBox.Size = new System.Drawing.Size(50, 20);
            this.locationAnimationTextBox.TabIndex = 8;
            this.locationAnimationTextBox.TabStop = false;
            this.locationAnimationTextBox.Text = "0, -1";
            // 
            // radLabelMsec
            // 
            this.radLabelMsec.Location = new System.Drawing.Point(71, 53);
            this.radLabelMsec.Name = "radLabelMsec";
            this.radLabelMsec.Size = new System.Drawing.Size(21, 16);
            this.radLabelMsec.TabIndex = 9;
            this.radLabelMsec.Text = "ms";
            this.radLabelMsec.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radLabelLocationAnimation
            // 
            this.radLabelLocationAnimation.Location = new System.Drawing.Point(9, 73);
            this.radLabelLocationAnimation.Name = "radLabelLocationAnimation";
            this.radLabelLocationAnimation.Size = new System.Drawing.Size(104, 16);
            this.radLabelLocationAnimation.TabIndex = 10;
            this.radLabelLocationAnimation.Text = "Location animation:";
            this.radLabelLocationAnimation.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radButtonApply
            // 
            this.radButtonApply.Location = new System.Drawing.Point(9, 157);
            this.radButtonApply.Name = "radButtonApply";
            this.radButtonApply.Size = new System.Drawing.Size(141, 24);
            this.radButtonApply.TabIndex = 11;
            this.radButtonApply.Text = "Apply";
       
            // 
            // radGroupSettings
            // 
            this.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupSettings.Controls.Add(this.radLabelInterval);
            this.radGroupSettings.Controls.Add(this.intervalTextBox);
            this.radGroupSettings.Controls.Add(this.radButtonApply);
            this.radGroupSettings.Controls.Add(this.radLabelMsec);
            this.radGroupSettings.Controls.Add(this.radCheckBoxOpacity);
            this.radGroupSettings.Controls.Add(this.locationAnimationTextBox);
            this.radGroupSettings.Controls.Add(this.radLabelLocationAnimation);
            this.radGroupSettings.FooterImageIndex = -1;
            this.radGroupSettings.FooterImageKey = "";
            this.radGroupSettings.FooterText = "";
            this.radGroupSettings.HeaderImageIndex = -1;
            this.radGroupSettings.HeaderImageKey = "";
            this.radGroupSettings.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupSettings.HeaderText = " Settings ";
            this.radGroupSettings.Location = new System.Drawing.Point(10, 5);
            this.radGroupSettings.Name = "radGroupSettings";
            this.radGroupSettings.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupSettings.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupSettings.Size = new System.Drawing.Size(180, 203);
            this.radGroupSettings.TabIndex = 0;
            this.radGroupSettings.Text = " Settings ";
            // 
            // Form1
            // 
            this.Controls.Add(this.radStartStopButton);
            this.Controls.Add(this.currentFrameTextBox);
            this.Controls.Add(this.radButtonNext);
            this.Controls.Add(this.radButtonPrevious);
            this.Controls.Add(this.radRotator1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1224, 820);
            this.Controls.SetChildIndex(this.radRotator1, 0);
            this.Controls.SetChildIndex(this.radButtonPrevious, 0);
            this.Controls.SetChildIndex(this.radButtonNext, 0);
            this.Controls.SetChildIndex(this.currentFrameTextBox, 0);
            this.Controls.SetChildIndex(this.radStartStopButton, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radRotator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStartStopButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentFrameTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intervalTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationAnimationTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMsec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelLocationAnimation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).EndInit();
            this.radGroupSettings.ResumeLayout(false);
            this.radGroupSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRotator radRotator1;
        private Telerik.WinControls.UI.RadItemsContainer radItemsContainer1;
        private Telerik.WinControls.UI.RadImageItem radImageItem1;
        private Telerik.WinControls.UI.RadItemsContainer radItemsContainer2;
        private Telerik.WinControls.UI.RadButtonElement radButtonElement1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.UI.RadTextBoxElement radTextBoxElement1;
        private Telerik.WinControls.UI.RadButton radButtonPrevious;
        private Telerik.WinControls.UI.RadButton radButtonNext;
        private Telerik.WinControls.UI.RadButton radStartStopButton;
        private Telerik.WinControls.UI.RadTextBox currentFrameTextBox;
        private Telerik.WinControls.UI.RadLabel radLabelInterval;
        private Telerik.WinControls.UI.RadTextBox intervalTextBox;
        private Telerik.WinControls.UI.RadCheckBox radCheckBoxOpacity;
        private Telerik.WinControls.UI.RadTextBox locationAnimationTextBox;
        private Telerik.WinControls.UI.RadLabel radLabelMsec;
        private Telerik.WinControls.UI.RadLabel radLabelLocationAnimation;
        private Telerik.WinControls.UI.RadButton radButtonApply;
        private Telerik.WinControls.UI.RadWebBrowserItem radWebBrowserItem1;
        private Telerik.WinControls.UI.RadGroupBox radGroupSettings;
    }
}