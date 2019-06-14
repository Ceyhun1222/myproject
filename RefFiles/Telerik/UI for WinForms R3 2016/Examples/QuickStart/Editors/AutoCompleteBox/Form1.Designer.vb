Namespace Telerik.Examples.WinControls.Editors.AutoCompleteBox
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
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
			Me.radButtonSend = New Telerik.WinControls.UI.RadButton()
			Me.radButtonTo = New Telerik.WinControls.UI.RadButton()
			Me.radButtonCc = New Telerik.WinControls.UI.RadButton()
			Me.radLabelSubject = New Telerik.WinControls.UI.RadLabel()
			Me.radTextBoxControlSubject = New Telerik.WinControls.UI.RadTextBoxControl()
			Me.radAutoCompleteBox1 = New Telerik.WinControls.UI.RadAutoCompleteBox()
			Me.radAutoCompleteBox2 = New Telerik.WinControls.UI.RadAutoCompleteBox()
			Me.radPanel1 = New Telerik.WinControls.UI.RadPanel()
			Me.rightPanel = New Telerik.WinControls.UI.RadPanel()
			Me.radTextBox1 = New Telerik.WinControls.UI.RadTextBox()
			Me.radListControlRecipients = New Telerik.WinControls.UI.RadListControl()
			Me.radLabel1Recipients = New Telerik.WinControls.UI.RadLabel()
			Me.radListControlCarbonCopy = New Telerik.WinControls.UI.RadListControl()
			Me.radLabelCarbonCopy = New Telerik.WinControls.UI.RadLabel()
			Me.radDropDownListAutoCompleteMode = New Telerik.WinControls.UI.RadDropDownList()
			Me.radLabelAutoComplete = New Telerik.WinControls.UI.RadLabel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radButtonSend, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonTo, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonCc, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelSubject, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBoxControlSubject, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radAutoCompleteBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radAutoCompleteBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanel1.SuspendLayout()
			CType(Me.rightPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.rightPanel.SuspendLayout()
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radListControlRecipients, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1Recipients, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radListControlCarbonCopy, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelCarbonCopy, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDropDownListAutoCompleteMode, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelAutoComplete, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radLabelAutoComplete)
			Me.settingsPanel.Controls.Add(Me.radDropDownListAutoCompleteMode)
			Me.settingsPanel.Controls.Add(Me.radLabelCarbonCopy)
			Me.settingsPanel.Controls.Add(Me.radLabel1Recipients)
			Me.settingsPanel.Controls.Add(Me.radListControlCarbonCopy)
			Me.settingsPanel.Controls.Add(Me.radListControlRecipients)
			Me.settingsPanel.Location = New Point(1085, 19)
			Me.settingsPanel.Size = New Size(0, 624)
			Me.settingsPanel.Controls.SetChildIndex(Me.radListControlRecipients, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radListControlCarbonCopy, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1Recipients, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabelCarbonCopy, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radDropDownListAutoCompleteMode, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabelAutoComplete, 0)
			' 
			' radButtonSend
			' 
			Me.radButtonSend.Image = My.Resources.send_email
			Me.radButtonSend.ImageAlignment = ContentAlignment.BottomCenter
			Me.radButtonSend.Location = New Point(7, 7)
			Me.radButtonSend.Name = "radButtonSend"
			Me.radButtonSend.Size = New Size(59, 85)
			Me.radButtonSend.TabIndex = 1
			Me.radButtonSend.Text = "Send"
			Me.radButtonSend.TextAlignment = ContentAlignment.TopCenter
			Me.radButtonSend.TextImageRelation = TextImageRelation.ImageAboveText
			' 
			' radButtonTo
			' 
			Me.radButtonTo.Location = New Point(72, 7)
			Me.radButtonTo.Name = "radButtonTo"
			Me.radButtonTo.Size = New Size(43, 24)
			Me.radButtonTo.TabIndex = 2
			Me.radButtonTo.Text = "To..."
			' 
			' radButtonCc
			' 
			Me.radButtonCc.Location = New Point(72, 37)
			Me.radButtonCc.Name = "radButtonCc"
			Me.radButtonCc.Size = New Size(43, 24)
			Me.radButtonCc.TabIndex = 3
			Me.radButtonCc.Text = "Cc..."
			' 
			' radLabelSubject
			' 
			Me.radLabelSubject.Location = New Point(72, 69)
			Me.radLabelSubject.Margin = New Padding(3, 5, 3, 3)
			Me.radLabelSubject.Name = "radLabelSubject"
			Me.radLabelSubject.Size = New Size(45, 18)
			Me.radLabelSubject.TabIndex = 4
			Me.radLabelSubject.Text = "Subject:"
			' 
			' radTextBoxControlSubject
			' 
			Me.radTextBoxControlSubject.Location = New Point(3, 65)
			Me.radTextBoxControlSubject.Name = "radTextBoxControlSubject"
			' 
			' 
			' 
			Me.radTextBoxControlSubject.RootElement.MinSize = New Size(0, 0)
			Me.radTextBoxControlSubject.Size = New Size(507, 26)
			Me.radTextBoxControlSubject.TabIndex = 5
			Me.radTextBoxControlSubject.Text = "Re: Feedback"
			CType(Me.radTextBoxControlSubject.GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.TextBoxViewElement).Padding = New Padding(2, 5, 2, 0)
			' 
			' radAutoCompleteBox1
			' 
			Me.radAutoCompleteBox1.Location = New Point(3, 35)
			Me.radAutoCompleteBox1.Name = "radAutoCompleteBox1"
			' 
			' 
			' 
			Me.radAutoCompleteBox1.RootElement.MaxSize = New Size(0, 0)
			Me.radAutoCompleteBox1.RootElement.MinSize = New Size(0, 0)
            Me.radAutoCompleteBox1.Size = New Size(507, 22)
			Me.radAutoCompleteBox1.TabIndex = 6
			Me.radAutoCompleteBox1.Text = "Samuel Jackson;"
			' 
			' radAutoCompleteBox2
			' 
			Me.radAutoCompleteBox2.Location = New Point(3, 5)
			Me.radAutoCompleteBox2.Name = "radAutoCompleteBox2"
			' 
			' 
			' 
			Me.radAutoCompleteBox2.RootElement.MaxSize = New Size(0, 0)
			Me.radAutoCompleteBox2.RootElement.MinSize = New Size(0, 0)
            Me.radAutoCompleteBox2.Size = New Size(507, 22)
			Me.radAutoCompleteBox2.TabIndex = 7
			Me.radAutoCompleteBox2.Text = "Joe Smith;"
			' 
			' radPanel1
			' 
			Me.radPanel1.Controls.Add(Me.radLabelSubject)
			Me.radPanel1.Controls.Add(Me.rightPanel)
			Me.radPanel1.Controls.Add(Me.radButtonCc)
			Me.radPanel1.Controls.Add(Me.radButtonTo)
			Me.radPanel1.Controls.Add(Me.radButtonSend)
			Me.radPanel1.Location = New Point(0, 0)
			Me.radPanel1.Name = "radPanel1"
			Me.radPanel1.Size = New Size(640, 450)
			Me.radPanel1.TabIndex = 8
			' 
			' rightPanel
			' 
			Me.rightPanel.Controls.Add(Me.radTextBox1)
			Me.rightPanel.Controls.Add(Me.radTextBoxControlSubject)
			Me.rightPanel.Controls.Add(Me.radAutoCompleteBox1)
			Me.rightPanel.Controls.Add(Me.radAutoCompleteBox2)
			Me.rightPanel.Dock = DockStyle.Right
			Me.rightPanel.Location = New Point(127, 0)
			Me.rightPanel.Name = "rightPanel"
			Me.rightPanel.Padding = New Padding(0, 5, 0, 0)
			' 
			' 
			' 
			Me.rightPanel.RootElement.Padding = New Padding(0, 5, 0, 0)
			Me.rightPanel.Size = New Size(513, 450)
			Me.rightPanel.TabIndex = 1
			' 
			' radTextBox1
			' 
			Me.radTextBox1.AutoSize = False
			Me.radTextBox1.Font = New Font("Calibri", 11F)
			Me.radTextBox1.Location = New Point(3, 95)
			Me.radTextBox1.Multiline = True
			Me.radTextBox1.Name = "radTextBox1"
			Me.radTextBox1.ReadOnly = True
			Me.radTextBox1.Size = New Size(507, 352)
			Me.radTextBox1.TabIndex = 8
			Me.radTextBox1.Text = resources.GetString("radTextBox1.Text")
			' 
			' radListControlRecipients
			' 
			Me.radListControlRecipients.Location = New Point(29, 76)
			Me.radListControlRecipients.Name = "radListControlRecipients"
			Me.radListControlRecipients.Size = New Size(14, 119)
			Me.radListControlRecipients.TabIndex = 1
			Me.radListControlRecipients.Text = "Recipients List"
			' 
			' radLabel1Recipients
			' 
			Me.radLabel1Recipients.Location = New Point(29, 52)
			Me.radLabel1Recipients.Name = "radLabel1Recipients"
			Me.radLabel1Recipients.Size = New Size(77, 18)
			Me.radLabel1Recipients.TabIndex = 2
			Me.radLabel1Recipients.Text = "Recipients List"
			' 
			' radListControlCarbonCopy
			' 
			Me.radListControlCarbonCopy.Location = New Point(29, 225)
			Me.radListControlCarbonCopy.Name = "radListControlCarbonCopy"
			Me.radListControlCarbonCopy.Size = New Size(14, 129)
			Me.radListControlCarbonCopy.TabIndex = 3
			Me.radListControlCarbonCopy.Text = "radListControl2"
			' 
			' radLabelCarbonCopy
			' 
			Me.radLabelCarbonCopy.Location = New Point(29, 201)
			Me.radLabelCarbonCopy.Name = "radLabelCarbonCopy"
			Me.radLabelCarbonCopy.Size = New Size(91, 18)
			Me.radLabelCarbonCopy.TabIndex = 4
			Me.radLabelCarbonCopy.Text = "Carbon Copy List"
			' 
			' radDropDownListAutoCompleteMode
			' 
			radListDataItem1.Text = "Suggest"
			radListDataItem1.TextWrap = True
			radListDataItem2.Text = "Append"
			radListDataItem2.TextWrap = True
			radListDataItem3.Text = "SuggestAppend"
			radListDataItem3.TextWrap = True
			Me.radDropDownListAutoCompleteMode.Items.Add(radListDataItem1)
			Me.radDropDownListAutoCompleteMode.Items.Add(radListDataItem2)
			Me.radDropDownListAutoCompleteMode.Items.Add(radListDataItem3)
			Me.radDropDownListAutoCompleteMode.Location = New Point(29, 400)
			Me.radDropDownListAutoCompleteMode.Name = "radDropDownListAutoCompleteMode"
			Me.radDropDownListAutoCompleteMode.Size = New Size(126, 20)
            Me.radDropDownListAutoCompleteMode.TabIndex = 5
            Me.radDropDownListAutoCompleteMode.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			' 
			' radLabelAutoComplete
			' 
			Me.radLabelAutoComplete.Location = New Point(29, 373)
			Me.radLabelAutoComplete.Name = "radLabelAutoComplete"
			Me.radLabelAutoComplete.Size = New Size(111, 18)
			Me.radLabelAutoComplete.TabIndex = 6
			Me.radLabelAutoComplete.Text = "AutoComplete Mode"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.radPanel1)
			Me.Name = "Form1"
			Me.Size = New Size(1196, 599)
			Me.Controls.SetChildIndex(Me.radPanel1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radButtonSend, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonTo, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonCc, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelSubject, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBoxControlSubject, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radAutoCompleteBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radAutoCompleteBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanel1.ResumeLayout(False)
			Me.radPanel1.PerformLayout()
			CType(Me.rightPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.rightPanel.ResumeLayout(False)
			CType(Me.radTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radListControlRecipients, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1Recipients, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radListControlCarbonCopy, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelCarbonCopy, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDropDownListAutoCompleteMode, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelAutoComplete, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radButtonSend As Telerik.WinControls.UI.RadButton
		Private radButtonTo As Telerik.WinControls.UI.RadButton
		Private radButtonCc As Telerik.WinControls.UI.RadButton
		Private radLabelSubject As Telerik.WinControls.UI.RadLabel
		Private radTextBoxControlSubject As Telerik.WinControls.UI.RadTextBoxControl
		Private radAutoCompleteBox1 As Telerik.WinControls.UI.RadAutoCompleteBox
		Private radAutoCompleteBox2 As Telerik.WinControls.UI.RadAutoCompleteBox
		Private radPanel1 As Telerik.WinControls.UI.RadPanel
		'private System.Windows.Forms.PictureBox pictureBox1;
		Private radLabelCarbonCopy As Telerik.WinControls.UI.RadLabel
		Private radListControlCarbonCopy As Telerik.WinControls.UI.RadListControl
		Private radLabel1Recipients As Telerik.WinControls.UI.RadLabel
		Private radListControlRecipients As Telerik.WinControls.UI.RadListControl
		Private radLabelAutoComplete As Telerik.WinControls.UI.RadLabel
		Private radDropDownListAutoCompleteMode As Telerik.WinControls.UI.RadDropDownList
		Private radTextBox1 As Telerik.WinControls.UI.RadTextBox
		Private rightPanel As Telerik.WinControls.UI.RadPanel
	End Class
End Namespace