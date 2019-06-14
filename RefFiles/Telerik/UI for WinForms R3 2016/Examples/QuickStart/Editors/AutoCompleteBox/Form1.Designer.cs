namespace Telerik.Examples.WinControls.Editors.AutoCompleteBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            this.radButtonSend = new Telerik.WinControls.UI.RadButton();
            this.radButtonTo = new Telerik.WinControls.UI.RadButton();
            this.radButtonCc = new Telerik.WinControls.UI.RadButton();
            this.radLabelSubject = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxControlSubject = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radAutoCompleteBox1 = new Telerik.WinControls.UI.RadAutoCompleteBox();
            this.radAutoCompleteBox2 = new Telerik.WinControls.UI.RadAutoCompleteBox();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rightPanel = new Telerik.WinControls.UI.RadPanel();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.radListControlRecipients = new Telerik.WinControls.UI.RadListControl();
            this.radLabel1Recipients = new Telerik.WinControls.UI.RadLabel();
            this.radListControlCarbonCopy = new Telerik.WinControls.UI.RadListControl();
            this.radLabelCarbonCopy = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownListAutoCompleteMode = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabelAutoComplete = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelSubject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSubject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radAutoCompleteBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radAutoCompleteBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightPanel)).BeginInit();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlRecipients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1Recipients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlCarbonCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelCarbonCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListAutoCompleteMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelAutoComplete)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radLabelAutoComplete);
            this.settingsPanel.Controls.Add(this.radDropDownListAutoCompleteMode);
            this.settingsPanel.Controls.Add(this.radLabelCarbonCopy);
            this.settingsPanel.Controls.Add(this.radLabel1Recipients);
            this.settingsPanel.Controls.Add(this.radListControlCarbonCopy);
            this.settingsPanel.Controls.Add(this.radListControlRecipients);
            this.settingsPanel.Location = new System.Drawing.Point(1085, 19);
            this.settingsPanel.Size = new System.Drawing.Size(0, 624);
            this.settingsPanel.Controls.SetChildIndex(this.radListControlRecipients, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radListControlCarbonCopy, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radLabel1Recipients, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radLabelCarbonCopy, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radDropDownListAutoCompleteMode, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radLabelAutoComplete, 0);
            // 
            // radButtonSend
            // 
            this.radButtonSend.Image = global::Telerik.Examples.WinControls.Properties.Resources.send_email;
            this.radButtonSend.ImageAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonSend.Location = new System.Drawing.Point(7, 7);
            this.radButtonSend.Name = "radButtonSend";
            this.radButtonSend.Size = new System.Drawing.Size(59, 85);
            this.radButtonSend.TabIndex = 1;
            this.radButtonSend.Text = "Send";
            this.radButtonSend.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.radButtonSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // radButtonTo
            // 
            this.radButtonTo.Location = new System.Drawing.Point(72, 7);
            this.radButtonTo.Name = "radButtonTo";
            this.radButtonTo.Size = new System.Drawing.Size(43, 24);
            this.radButtonTo.TabIndex = 2;
            this.radButtonTo.Text = "To...";
            // 
            // radButtonCc
            // 
            this.radButtonCc.Location = new System.Drawing.Point(72, 37);
            this.radButtonCc.Name = "radButtonCc";
            this.radButtonCc.Size = new System.Drawing.Size(43, 24);
            this.radButtonCc.TabIndex = 3;
            this.radButtonCc.Text = "Cc...";
            // 
            // radLabelSubject
            // 
            this.radLabelSubject.Location = new System.Drawing.Point(72, 69);
            this.radLabelSubject.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.radLabelSubject.Name = "radLabelSubject";
            this.radLabelSubject.Size = new System.Drawing.Size(45, 18);
            this.radLabelSubject.TabIndex = 4;
            this.radLabelSubject.Text = "Subject:";
            // 
            // radTextBoxControlSubject
            // 
            this.radTextBoxControlSubject.Location = new System.Drawing.Point(3, 57);
            this.radTextBoxControlSubject.Name = "radTextBoxControlSubject";
            // 
            // 
            // 
            this.radTextBoxControlSubject.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.radTextBoxControlSubject.Size = new System.Drawing.Size(507, 24);
            this.radTextBoxControlSubject.TabIndex = 5;
            this.radTextBoxControlSubject.Text = "Re: Feedback";
            ((Telerik.WinControls.UI.TextBoxViewElement)(this.radTextBoxControlSubject.GetChildAt(0).GetChildAt(0))).Padding = new System.Windows.Forms.Padding(2, 5, 2, 0);
            // 
            // radAutoCompleteBox1
            // 
            this.radAutoCompleteBox1.Location = new System.Drawing.Point(3, 31);
            this.radAutoCompleteBox1.Name = "radAutoCompleteBox1";
            // 
            // 
            // 
            this.radAutoCompleteBox1.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.radAutoCompleteBox1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.radAutoCompleteBox1.Size = new System.Drawing.Size(507, 24);
            this.radAutoCompleteBox1.TabIndex = 6;
            this.radAutoCompleteBox1.Text = "Samuel Jackson;";
            // 
            // radAutoCompleteBox2
            // 
            this.radAutoCompleteBox2.Location = new System.Drawing.Point(3, 5);
            this.radAutoCompleteBox2.Name = "radAutoCompleteBox2";
            // 
            // 
            // 
            this.radAutoCompleteBox2.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.radAutoCompleteBox2.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.radAutoCompleteBox2.Size = new System.Drawing.Size(507, 24);
            this.radAutoCompleteBox2.TabIndex = 7;
            this.radAutoCompleteBox2.Text = "Joe Smith;";
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radLabelSubject);
            this.radPanel1.Controls.Add(this.rightPanel);
            this.radPanel1.Controls.Add(this.radButtonCc);
            this.radPanel1.Controls.Add(this.radButtonTo);
            this.radPanel1.Controls.Add(this.radButtonSend);
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(640, 450);
            this.radPanel1.TabIndex = 8;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.radTextBox1);
            this.rightPanel.Controls.Add(this.radTextBoxControlSubject);
            this.rightPanel.Controls.Add(this.radAutoCompleteBox1);
            this.rightPanel.Controls.Add(this.radAutoCompleteBox2);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(127, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.rightPanel.Size = new System.Drawing.Size(513, 450);
            this.rightPanel.TabIndex = 1;
            // 
            // radTextBox1
            // 
            this.radTextBox1.AutoSize = false;
            this.radTextBox1.Font = new System.Drawing.Font("Calibri", 11F);
            this.radTextBox1.Location = new System.Drawing.Point(3, 85);
            this.radTextBox1.Multiline = true;
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.ReadOnly = true;
            this.radTextBox1.Size = new System.Drawing.Size(507, 362);
            this.radTextBox1.TabIndex = 8;
            this.radTextBox1.Text = resources.GetString("radTextBox1.Text");
            // 
            // radListControlRecipients
            // 
            this.radListControlRecipients.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radListControlRecipients.Location = new System.Drawing.Point(10, 76);
            this.radListControlRecipients.Name = "radListControlRecipients";
            this.radListControlRecipients.Size = new System.Drawing.Size(0, 119);
            this.radListControlRecipients.TabIndex = 1;
            this.radListControlRecipients.Text = "Recipients List";
            // 
            // radLabel1Recipients
            // 
            this.radLabel1Recipients.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel1Recipients.Location = new System.Drawing.Point(10, 52);
            this.radLabel1Recipients.Name = "radLabel1Recipients";
            this.radLabel1Recipients.Size = new System.Drawing.Size(76, 18);
            this.radLabel1Recipients.TabIndex = 2;
            this.radLabel1Recipients.Text = "Recipients List";
            // 
            // radListControlCarbonCopy
            // 
            this.radListControlCarbonCopy.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radListControlCarbonCopy.Location = new System.Drawing.Point(10, 225);
            this.radListControlCarbonCopy.Name = "radListControlCarbonCopy";
            this.radListControlCarbonCopy.Size = new System.Drawing.Size(0, 129);
            this.radListControlCarbonCopy.TabIndex = 3;
            this.radListControlCarbonCopy.Text = "radListControl2";
            // 
            // radLabelCarbonCopy
            // 
            this.radLabelCarbonCopy.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabelCarbonCopy.Location = new System.Drawing.Point(10, 201);
            this.radLabelCarbonCopy.Name = "radLabelCarbonCopy";
            this.radLabelCarbonCopy.Size = new System.Drawing.Size(90, 18);
            this.radLabelCarbonCopy.TabIndex = 4;
            this.radLabelCarbonCopy.Text = "Carbon Copy List";
            // 
            // radDropDownListAutoCompleteMode
            // 
            this.radDropDownListAutoCompleteMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radDropDownListAutoCompleteMode.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem1.Text = "Suggest";
            radListDataItem2.Text = "Append";
            radListDataItem3.Text = "SuggestAppend";
            this.radDropDownListAutoCompleteMode.Items.Add(radListDataItem1);
            this.radDropDownListAutoCompleteMode.Items.Add(radListDataItem2);
            this.radDropDownListAutoCompleteMode.Items.Add(radListDataItem3);
            this.radDropDownListAutoCompleteMode.Location = new System.Drawing.Point(10, 400);
            this.radDropDownListAutoCompleteMode.Name = "radDropDownListAutoCompleteMode";
            this.radDropDownListAutoCompleteMode.Size = new System.Drawing.Size(0, 0);
            this.radDropDownListAutoCompleteMode.TabIndex = 5;
            // 
            // radLabelAutoComplete
            // 
            this.radLabelAutoComplete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabelAutoComplete.Location = new System.Drawing.Point(10, 373);
            this.radLabelAutoComplete.Name = "radLabelAutoComplete";
            this.radLabelAutoComplete.Size = new System.Drawing.Size(110, 18);
            this.radLabelAutoComplete.TabIndex = 6;
            this.radLabelAutoComplete.Text = "AutoComplete Mode";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanel1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1414, 607);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.radPanel1, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelSubject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSubject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radAutoCompleteBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radAutoCompleteBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightPanel)).EndInit();
            this.rightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlRecipients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1Recipients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListControlCarbonCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelCarbonCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListAutoCompleteMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelAutoComplete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonSend;
        private Telerik.WinControls.UI.RadButton radButtonTo;
        private Telerik.WinControls.UI.RadButton radButtonCc;
        private Telerik.WinControls.UI.RadLabel radLabelSubject;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlSubject;
        private Telerik.WinControls.UI.RadAutoCompleteBox radAutoCompleteBox1;
        private Telerik.WinControls.UI.RadAutoCompleteBox radAutoCompleteBox2;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        //private System.Windows.Forms.PictureBox pictureBox1;
        private Telerik.WinControls.UI.RadLabel radLabelCarbonCopy;
        private Telerik.WinControls.UI.RadListControl radListControlCarbonCopy;
        private Telerik.WinControls.UI.RadLabel radLabel1Recipients;
        private Telerik.WinControls.UI.RadListControl radListControlRecipients;
        private Telerik.WinControls.UI.RadLabel radLabelAutoComplete;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListAutoCompleteMode;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
        private Telerik.WinControls.UI.RadPanel rightPanel;
    }
}