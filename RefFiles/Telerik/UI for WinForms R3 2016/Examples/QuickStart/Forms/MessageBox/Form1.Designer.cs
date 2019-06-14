using Telerik.WinControls.UI;
using System.Collections.Generic;
namespace Telerik.Examples.WinControls.Forms.MessageBox
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem7 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem8 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem9 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem10 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem11 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem5 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem6 = new Telerik.WinControls.UI.RadListDataItem();
            this.radTxtMessage = new Telerik.WinControls.UI.RadTextBox();
            this.radCheckRTL = new Telerik.WinControls.UI.RadCheckBox();
            this.radTxtCaption = new Telerik.WinControls.UI.RadTextBox();
            this.radBtnShow = new Telerik.WinControls.UI.RadButton();
            this.radComboMessageType = new Telerik.WinControls.UI.RadDropDownList();
            this.radComboButtons = new Telerik.WinControls.UI.RadDropDownList();
            this.radGroupSettings = new Telerik.WinControls.UI.RadGroupBox();
            this.radLblDialog = new Telerik.WinControls.UI.RadLabel();
            this.radLblButtons = new Telerik.WinControls.UI.RadLabel();
            this.radLblType = new Telerik.WinControls.UI.RadLabel();
            this.radLblText = new Telerik.WinControls.UI.RadLabel();
            this.radLblCaption = new Telerik.WinControls.UI.RadLabel();
            this.radTxtDialogResult = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckRTL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboMessageType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).BeginInit();
            this.radGroupSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblDialog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtDialogResult)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupSettings);
            this.settingsPanel.Location = new System.Drawing.Point(940, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 597);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupSettings, 0);
            // 
            // radTxtMessage
            // 
            this.radTxtMessage.AcceptsReturn = true;
            this.radTxtMessage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radTxtMessage.AutoSize = false;
            this.radTxtMessage.Location = new System.Drawing.Point(5, 94);
            this.radTxtMessage.Multiline = true;
            this.radTxtMessage.Name = "radTxtMessage";
            this.radTxtMessage.Size = new System.Drawing.Size(170, 41);
            this.radTxtMessage.TabIndex = 0;
            this.radTxtMessage.Text = "Hello, World!";
            // 
            // radCheckRTL
            // 
            this.radCheckRTL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckRTL.BackColor = System.Drawing.Color.Transparent;
            this.radCheckRTL.Location = new System.Drawing.Point(5, 269);
            this.radCheckRTL.Name = "radCheckRTL";
            this.radCheckRTL.Size = new System.Drawing.Size(78, 18);
            this.radCheckRTL.TabIndex = 9;
            this.radCheckRTL.Text = "RightToLeft";
            // 
            // radTxtCaption
            // 
            this.radTxtCaption.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radTxtCaption.Location = new System.Drawing.Point(5, 49);
            this.radTxtCaption.Name = "radTxtCaption";
            this.radTxtCaption.NullText = "Type caption...";
            this.radTxtCaption.Size = new System.Drawing.Size(170, 20);
            this.radTxtCaption.TabIndex = 1;
            this.radTxtCaption.Text = "Message";
            // 
            // radBtnShow
            // 
            this.radBtnShow.Location = new System.Drawing.Point(0, 0);
            this.radBtnShow.Name = "radBtnShow";
            this.radBtnShow.Size = new System.Drawing.Size(229, 30);
            this.radBtnShow.TabIndex = 5;
            this.radBtnShow.Text = "Show RadMessageBox";
            // 
            // radComboMessageType
            // 
            this.radComboMessageType.AllowShowFocusCues = false;
            this.radComboMessageType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radComboMessageType.AutoCompleteDisplayMember = null;
            this.radComboMessageType.AutoCompleteValueMember = null;
            this.radComboMessageType.DescriptionTextMember = null;
            this.radComboMessageType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem7.Text = "None";
            radListDataItem8.Text = "Info";
            radListDataItem9.Text = "Question";
            radListDataItem10.Text = "Exclamation";
            radListDataItem11.Text = "Error";
            this.radComboMessageType.Items.Add(radListDataItem7);
            this.radComboMessageType.Items.Add(radListDataItem8);
            this.radComboMessageType.Items.Add(radListDataItem9);
            this.radComboMessageType.Items.Add(radListDataItem10);
            this.radComboMessageType.Items.Add(radListDataItem11);
            this.radComboMessageType.Location = new System.Drawing.Point(5, 161);
            this.radComboMessageType.Name = "radComboMessageType";
            // 
            // 
            // 
            this.radComboMessageType.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radComboMessageType.Size = new System.Drawing.Size(170, 20);
            this.radComboMessageType.TabIndex = 8;
            // 
            // radComboButtons
            // 
            this.radComboButtons.AllowShowFocusCues = false;
            this.radComboButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radComboButtons.AutoCompleteDisplayMember = null;
            this.radComboButtons.AutoCompleteValueMember = null;
            this.radComboButtons.DescriptionTextMember = null;
            this.radComboButtons.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem1.Text = "OK";
            radListDataItem2.Text = "OK, Cancel";
            radListDataItem3.Text = "Yes, No";
            radListDataItem4.Text = "Yes, No, Cancel";
            radListDataItem5.Text = "Retry, Cancel";
            radListDataItem6.Text = "Abort, Retry, Ignore";
            this.radComboButtons.Items.Add(radListDataItem1);
            this.radComboButtons.Items.Add(radListDataItem2);
            this.radComboButtons.Items.Add(radListDataItem3);
            this.radComboButtons.Items.Add(radListDataItem4);
            this.radComboButtons.Items.Add(radListDataItem5);
            this.radComboButtons.Items.Add(radListDataItem6);
            this.radComboButtons.Location = new System.Drawing.Point(5, 207);
            this.radComboButtons.Name = "radComboButtons";
            // 
            // 
            // 
            this.radComboButtons.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radComboButtons.Size = new System.Drawing.Size(170, 20);
            this.radComboButtons.TabIndex = 6;
            // 
            // radGroupSettings
            // 
            this.radGroupSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupSettings.Controls.Add(this.radLblDialog);
            this.radGroupSettings.Controls.Add(this.radLblButtons);
            this.radGroupSettings.Controls.Add(this.radCheckRTL);
            this.radGroupSettings.Controls.Add(this.radLblType);
            this.radGroupSettings.Controls.Add(this.radTxtMessage);
            this.radGroupSettings.Controls.Add(this.radLblText);
            this.radGroupSettings.Controls.Add(this.radLblCaption);
            this.radGroupSettings.Controls.Add(this.radComboButtons);
            this.radGroupSettings.Controls.Add(this.radTxtDialogResult);
            this.radGroupSettings.Controls.Add(this.radTxtCaption);
            this.radGroupSettings.Controls.Add(this.radComboMessageType);
            this.radGroupSettings.FooterText = "";
            this.radGroupSettings.HeaderText = " MessageBox Settings ";
            this.radGroupSettings.Location = new System.Drawing.Point(10, 5);
            this.radGroupSettings.Name = "radGroupSettings";
            this.radGroupSettings.Size = new System.Drawing.Size(180, 300);
            this.radGroupSettings.TabIndex = 0;
            this.radGroupSettings.Text = " MessageBox Settings ";
            // 
            // radLblDialog
            // 
            this.radLblDialog.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLblDialog.Location = new System.Drawing.Point(5, 243);
            this.radLblDialog.Name = "radLblDialog";
            this.radLblDialog.Size = new System.Drawing.Size(74, 18);
            this.radLblDialog.TabIndex = 12;
            this.radLblDialog.Text = "Dialog Result:";
            // 
            // radLblButtons
            // 
            this.radLblButtons.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLblButtons.Location = new System.Drawing.Point(5, 187);
            this.radLblButtons.Name = "radLblButtons";
            this.radLblButtons.Size = new System.Drawing.Size(46, 18);
            this.radLblButtons.TabIndex = 2;
            this.radLblButtons.Text = "Buttons:";
            // 
            // radLblType
            // 
            this.radLblType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLblType.Location = new System.Drawing.Point(5, 141);
            this.radLblType.Name = "radLblType";
            this.radLblType.Size = new System.Drawing.Size(79, 18);
            this.radLblType.TabIndex = 2;
            this.radLblType.Text = "Message Type:";
            // 
            // radLblText
            // 
            this.radLblText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLblText.Location = new System.Drawing.Point(5, 74);
            this.radLblText.Name = "radLblText";
            this.radLblText.Size = new System.Drawing.Size(76, 18);
            this.radLblText.TabIndex = 2;
            this.radLblText.Text = "Message Text:";
            // 
            // radLblCaption
            // 
            this.radLblCaption.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLblCaption.Location = new System.Drawing.Point(5, 28);
            this.radLblCaption.Name = "radLblCaption";
            this.radLblCaption.Size = new System.Drawing.Size(94, 18);
            this.radLblCaption.TabIndex = 0;
            this.radLblCaption.Text = "Message Caption:";
            // 
            // radTxtDialogResult
            // 
            this.radTxtDialogResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTxtDialogResult.Location = new System.Drawing.Point(93, 240);
            this.radTxtDialogResult.Name = "radTxtDialogResult";
            this.radTxtDialogResult.Size = new System.Drawing.Size(82, 20);
            this.radTxtDialogResult.TabIndex = 1;
            this.radTxtDialogResult.Tag = "Right";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radBtnShow);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(1355, 1000);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.radBtnShow, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckRTL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboMessageType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupSettings)).EndInit();
            this.radGroupSettings.ResumeLayout(false);
            this.radGroupSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblDialog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtDialogResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox radTxtMessage;
        private Telerik.WinControls.UI.RadCheckBox radCheckRTL;
        private Telerik.WinControls.UI.RadTextBox radTxtCaption;
        private Telerik.WinControls.UI.RadButton radBtnShow;
        private Telerik.WinControls.UI.RadDropDownList radComboMessageType;
        private Telerik.WinControls.UI.RadDropDownList radComboButtons;   
        private Telerik.WinControls.UI.RadGroupBox radGroupSettings;
        private Telerik.WinControls.UI.RadLabel radLblCaption;
        private Telerik.WinControls.UI.RadLabel radLblText;
        private Telerik.WinControls.UI.RadLabel radLblType;
        private Telerik.WinControls.UI.RadLabel radLblButtons;
        private Telerik.WinControls.UI.RadLabel radLblDialog;
        private Telerik.WinControls.UI.RadTextBox radTxtDialogResult;
    }
}
