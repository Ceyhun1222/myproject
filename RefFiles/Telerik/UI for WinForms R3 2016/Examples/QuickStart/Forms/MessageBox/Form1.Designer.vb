Imports Telerik.WinControls.UI
Namespace Telerik.Examples.WinControls.Forms.MessageBox
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Dim RadListDataItem7 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem8 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem9 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem10 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem11 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem1 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem2 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem3 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem4 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem5 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem6 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Me.radTxtMessage = New Telerik.WinControls.UI.RadTextBox()
            Me.radCheckRTL = New Telerik.WinControls.UI.RadCheckBox()
            Me.radTxtCaption = New Telerik.WinControls.UI.RadTextBox()
            Me.radBtnShow = New Telerik.WinControls.UI.RadButton()
            Me.radComboMessageType = New Telerik.WinControls.UI.RadDropDownList()
            Me.radComboButtons = New Telerik.WinControls.UI.RadDropDownList()
            Me.radGroupSettings = New Telerik.WinControls.UI.RadGroupBox()
            Me.radLblDialog = New Telerik.WinControls.UI.RadLabel()
            Me.radLblButtons = New Telerik.WinControls.UI.RadLabel()
            Me.radLblType = New Telerik.WinControls.UI.RadLabel()
            Me.radLblText = New Telerik.WinControls.UI.RadLabel()
            Me.radLblCaption = New Telerik.WinControls.UI.RadLabel()
            Me.radTxtDialogResult = New Telerik.WinControls.UI.RadTextBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTxtMessage, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckRTL, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTxtCaption, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radBtnShow, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radComboMessageType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radComboButtons, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupSettings.SuspendLayout()
            CType(Me.radLblDialog, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblButtons, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLblCaption, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTxtDialogResult, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radGroupSettings)
            Me.settingsPanel.Location = New System.Drawing.Point(940, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 597)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupSettings, 0)
            '
            'radTxtMessage
            '
            Me.radTxtMessage.AcceptsReturn = True
            Me.radTxtMessage.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radTxtMessage.AutoSize = False
            Me.radTxtMessage.Location = New System.Drawing.Point(5, 94)
            Me.radTxtMessage.Multiline = True
            Me.radTxtMessage.Name = "radTxtMessage"
            Me.radTxtMessage.Size = New System.Drawing.Size(170, 41)
            Me.radTxtMessage.TabIndex = 0
            Me.radTxtMessage.Text = "Hello, World!"
            '
            'radCheckRTL
            '
            Me.radCheckRTL.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckRTL.BackColor = System.Drawing.Color.Transparent
            Me.radCheckRTL.Location = New System.Drawing.Point(5, 269)
            Me.radCheckRTL.Name = "radCheckRTL"
            Me.radCheckRTL.Size = New System.Drawing.Size(78, 18)
            Me.radCheckRTL.TabIndex = 9
            Me.radCheckRTL.Text = "RightToLeft"
            '
            'radTxtCaption
            '
            Me.radTxtCaption.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radTxtCaption.Location = New System.Drawing.Point(5, 49)
            Me.radTxtCaption.Name = "radTxtCaption"
            Me.radTxtCaption.NullText = "Type caption..."
            Me.radTxtCaption.Size = New System.Drawing.Size(170, 20)
            Me.radTxtCaption.TabIndex = 1
            Me.radTxtCaption.Text = "Message"
            '
            'radBtnShow
            '
            Me.radBtnShow.Location = New System.Drawing.Point(0, 0)
            Me.radBtnShow.Name = "radBtnShow"
            Me.radBtnShow.Size = New System.Drawing.Size(229, 30)
            Me.radBtnShow.TabIndex = 5
            Me.radBtnShow.Text = "Show RadMessageBox"
            '
            'radComboMessageType
            '
            Me.radComboMessageType.AllowShowFocusCues = False
            Me.radComboMessageType.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radComboMessageType.AutoCompleteDisplayMember = Nothing
            Me.radComboMessageType.AutoCompleteValueMember = Nothing
            Me.radComboMessageType.DescriptionTextMember = Nothing
            Me.radComboMessageType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            RadListDataItem7.Text = "None"
            RadListDataItem8.Text = "Info"
            RadListDataItem9.Text = "Question"
            RadListDataItem10.Text = "Exclamation"
            RadListDataItem11.Text = "Error"
            Me.radComboMessageType.Items.Add(RadListDataItem7)
            Me.radComboMessageType.Items.Add(RadListDataItem8)
            Me.radComboMessageType.Items.Add(RadListDataItem9)
            Me.radComboMessageType.Items.Add(RadListDataItem10)
            Me.radComboMessageType.Items.Add(RadListDataItem11)
            Me.radComboMessageType.Location = New System.Drawing.Point(5, 161)
            Me.radComboMessageType.Name = "radComboMessageType"
            '
            '
            '
            Me.radComboMessageType.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radComboMessageType.Size = New System.Drawing.Size(170, 20)
            Me.radComboMessageType.TabIndex = 8
            '
            'radComboButtons
            '
            Me.radComboButtons.AllowShowFocusCues = False
            Me.radComboButtons.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radComboButtons.AutoCompleteDisplayMember = Nothing
            Me.radComboButtons.AutoCompleteValueMember = Nothing
            Me.radComboButtons.DescriptionTextMember = Nothing
            Me.radComboButtons.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            RadListDataItem1.Text = "OK"
            RadListDataItem2.Text = "OK, Cancel"
            RadListDataItem3.Text = "Yes, No"
            RadListDataItem4.Text = "Yes, No, Cancel"
            RadListDataItem5.Text = "Retry, Cancel"
            RadListDataItem6.Text = "Abort, Retry, Ignore"
            Me.radComboButtons.Items.Add(RadListDataItem1)
            Me.radComboButtons.Items.Add(RadListDataItem2)
            Me.radComboButtons.Items.Add(RadListDataItem3)
            Me.radComboButtons.Items.Add(RadListDataItem4)
            Me.radComboButtons.Items.Add(RadListDataItem5)
            Me.radComboButtons.Items.Add(RadListDataItem6)
            Me.radComboButtons.Location = New System.Drawing.Point(5, 207)
            Me.radComboButtons.Name = "radComboButtons"
            '
            '
            '
            Me.radComboButtons.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radComboButtons.Size = New System.Drawing.Size(170, 20)
            Me.radComboButtons.TabIndex = 6
            '
            'radGroupSettings
            '
            Me.radGroupSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupSettings.Controls.Add(Me.radLblDialog)
            Me.radGroupSettings.Controls.Add(Me.radLblButtons)
            Me.radGroupSettings.Controls.Add(Me.radCheckRTL)
            Me.radGroupSettings.Controls.Add(Me.radLblType)
            Me.radGroupSettings.Controls.Add(Me.radTxtMessage)
            Me.radGroupSettings.Controls.Add(Me.radLblText)
            Me.radGroupSettings.Controls.Add(Me.radLblCaption)
            Me.radGroupSettings.Controls.Add(Me.radComboButtons)
            Me.radGroupSettings.Controls.Add(Me.radTxtDialogResult)
            Me.radGroupSettings.Controls.Add(Me.radTxtCaption)
            Me.radGroupSettings.Controls.Add(Me.radComboMessageType)
            Me.radGroupSettings.FooterText = ""
            Me.radGroupSettings.HeaderText = " MessageBox Settings "
            Me.radGroupSettings.Location = New System.Drawing.Point(10, 5)
            Me.radGroupSettings.Name = "radGroupSettings"
            Me.radGroupSettings.Size = New System.Drawing.Size(180, 297)
            Me.radGroupSettings.TabIndex = 0
            Me.radGroupSettings.Text = " MessageBox Settings "
            '
            'radLblDialog
            '
            Me.radLblDialog.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblDialog.Location = New System.Drawing.Point(5, 244)
            Me.radLblDialog.Name = "radLblDialog"
            Me.radLblDialog.Size = New System.Drawing.Size(74, 18)
            Me.radLblDialog.TabIndex = 12
            Me.radLblDialog.Text = "Dialog Result:"
            '
            'radLblButtons
            '
            Me.radLblButtons.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblButtons.Location = New System.Drawing.Point(5, 187)
            Me.radLblButtons.Name = "radLblButtons"
            Me.radLblButtons.Size = New System.Drawing.Size(46, 18)
            Me.radLblButtons.TabIndex = 2
            Me.radLblButtons.Text = "Buttons:"
            '
            'radLblType
            '
            Me.radLblType.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblType.Location = New System.Drawing.Point(5, 141)
            Me.radLblType.Name = "radLblType"
            Me.radLblType.Size = New System.Drawing.Size(79, 18)
            Me.radLblType.TabIndex = 2
            Me.radLblType.Text = "Message Type:"
            '
            'radLblText
            '
            Me.radLblText.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblText.Location = New System.Drawing.Point(5, 74)
            Me.radLblText.Name = "radLblText"
            Me.radLblText.Size = New System.Drawing.Size(76, 18)
            Me.radLblText.TabIndex = 2
            Me.radLblText.Text = "Message Text:"
            '
            'radLblCaption
            '
            Me.radLblCaption.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLblCaption.Location = New System.Drawing.Point(5, 28)
            Me.radLblCaption.Name = "radLblCaption"
            Me.radLblCaption.Size = New System.Drawing.Size(94, 18)
            Me.radLblCaption.TabIndex = 0
            Me.radLblCaption.Text = "Message Caption:"
            '
            'radTxtDialogResult
            '
            Me.radTxtDialogResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.radTxtDialogResult.Location = New System.Drawing.Point(93, 241)
            Me.radTxtDialogResult.Name = "radTxtDialogResult"
            Me.radTxtDialogResult.Size = New System.Drawing.Size(82, 20)
            Me.radTxtDialogResult.TabIndex = 1
            Me.radTxtDialogResult.Tag = "Right"
            '
            'Form1
            '
            Me.Controls.Add(Me.radBtnShow)
            Me.Name = "Form1"
            Me.Padding = New System.Windows.Forms.Padding(20)
            Me.Size = New System.Drawing.Size(1355, 1000)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radBtnShow, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTxtMessage, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckRTL, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTxtCaption, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radBtnShow, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radComboMessageType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radComboButtons, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupSettings.ResumeLayout(False)
            Me.radGroupSettings.PerformLayout()
            CType(Me.radLblDialog, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblButtons, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLblCaption, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTxtDialogResult, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radTxtMessage As Telerik.WinControls.UI.RadTextBox
		Private radCheckRTL As Telerik.WinControls.UI.RadCheckBox
		Private radTxtCaption As Telerik.WinControls.UI.RadTextBox
		Private radBtnShow As Telerik.WinControls.UI.RadButton
		Private radComboMessageType As Telerik.WinControls.UI.RadDropDownList
		Private radComboButtons As Telerik.WinControls.UI.RadDropDownList
        Private radGroupSettings As Telerik.WinControls.UI.RadGroupBox
		Private radLblCaption As Telerik.WinControls.UI.RadLabel
		Private radLblText As Telerik.WinControls.UI.RadLabel
		Private radLblType As Telerik.WinControls.UI.RadLabel
		Private radLblButtons As Telerik.WinControls.UI.RadLabel
        Private radLblDialog As Telerik.WinControls.UI.RadLabel
		Private radTxtDialogResult As Telerik.WinControls.UI.RadTextBox
	End Class
End Namespace
