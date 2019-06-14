Imports Telerik.WinControls.UI
Namespace Telerik.Examples.WinControls.DropDownListAndListControl.DropDownList.AutoComplete
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
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim radListDataItem18 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem19 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem20 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem21 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem4 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem5 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem6 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem7 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem8 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem9 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem10 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem11 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem12 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem13 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem14 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem15 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem16 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem17 As New Telerik.WinControls.UI.RadListDataItem()
			Me.imageList1 = New ImageList(Me.components)
			Me.radGroupBoxSettings = New Telerik.WinControls.UI.RadGroupBox()
			Me.radBtnAdd = New Telerik.WinControls.UI.RadButton()
			Me.radBtnRemove = New Telerik.WinControls.UI.RadButton()
			Me.radTxtText = New Telerik.WinControls.UI.RadTextBox()
			Me.radTxtIndex = New Telerik.WinControls.UI.RadTextBox()
			Me.radLblText = New Telerik.WinControls.UI.RadLabel()
			Me.radLblItemIndex = New Telerik.WinControls.UI.RadLabel()
			Me.radCheckCaseSens = New Telerik.WinControls.UI.RadCheckBox()
			Me.radLblAutoComplete = New Telerik.WinControls.UI.RadLabel()
			Me.radComboAutoCompl = New Telerik.WinControls.UI.RadDropDownList()
			Me.radComboDemo = New Telerik.WinControls.UI.RadDropDownList()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radGroupBoxSettings, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBoxSettings.SuspendLayout()
			CType(Me.radBtnAdd, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnRemove, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTxtText, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTxtIndex, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblText, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblItemIndex, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckCaseSens, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblAutoComplete, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radComboAutoCompl, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radComboDemo, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
            Me.radPanelDemoHolder.Controls.Add(Me.radComboDemo)
			Me.radPanelDemoHolder.Size = New Size(362, 100)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBoxSettings)
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(200, 735)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBoxSettings, 0)
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Transparent
			Me.imageList1.Images.SetKeyName(0, "WizardPicture.bmp")
			Me.imageList1.Images.SetKeyName(1, "bulb_on.GIF")
			Me.imageList1.Images.SetKeyName(2, "CLSDFOLD.BMP")
			Me.imageList1.Images.SetKeyName(3, "test.bmp")
			Me.imageList1.Images.SetKeyName(4, "untitled.bmp")
			' 
			' radGroupBoxSettings
			' 
			Me.radGroupBoxSettings.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupBoxSettings.Anchor = AnchorStyles.Top
			Me.radGroupBoxSettings.Controls.Add(Me.radBtnAdd)
			Me.radGroupBoxSettings.Controls.Add(Me.radBtnRemove)
			Me.radGroupBoxSettings.Controls.Add(Me.radTxtText)
			Me.radGroupBoxSettings.Controls.Add(Me.radTxtIndex)
			Me.radGroupBoxSettings.Controls.Add(Me.radLblText)
			Me.radGroupBoxSettings.Controls.Add(Me.radLblItemIndex)
			Me.radGroupBoxSettings.Controls.Add(Me.radCheckCaseSens)
			Me.radGroupBoxSettings.Controls.Add(Me.radLblAutoComplete)
			Me.radGroupBoxSettings.Controls.Add(Me.radComboAutoCompl)
            Me.radGroupBoxSettings.FooterText = ""
			Me.radGroupBoxSettings.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupBoxSettings.HeaderText = " Settings "
			Me.radGroupBoxSettings.Location = New Point(10, 6)
			Me.radGroupBoxSettings.Name = "radGroupBoxSettings"
			Me.radGroupBoxSettings.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupBoxSettings.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupBoxSettings.Size = New Size(180, 198)
			Me.radGroupBoxSettings.TabIndex = 0
			Me.radGroupBoxSettings.Text = " Settings "
			' 
			' radBtnAdd
			' 
			Me.radBtnAdd.Location = New Point(7, 165)
			Me.radBtnAdd.Name = "radBtnAdd"
			Me.radBtnAdd.Size = New Size(66, 23)
			Me.radBtnAdd.TabIndex = 8
			Me.radBtnAdd.Tag = "NotTouch"
			Me.radBtnAdd.Text = "Add"

			' 
			' radBtnRemove
			' 
			Me.radBtnRemove.Location = New Point(86, 165)
			Me.radBtnRemove.Name = "radBtnRemove"
			Me.radBtnRemove.Size = New Size(67, 23)
			Me.radBtnRemove.TabIndex = 7
			Me.radBtnRemove.Tag = "NotTouch"
			Me.radBtnRemove.Text = "Remove"

			' 
			' radTxtText
			' 

			Me.radTxtText.Location = New Point(43, 138)
			Me.radTxtText.Name = "radTxtText"
			Me.radTxtText.Size = New Size(110, 20)
			Me.radTxtText.TabIndex = 6
			Me.radTxtText.TabStop = False
			Me.radTxtText.Tag = "Right"

			' 
			' radTxtIndex
			' 

			Me.radTxtIndex.Location = New Point(43, 111)
			Me.radTxtIndex.Name = "radTxtIndex"
			Me.radTxtIndex.ReadOnly = True
			Me.radTxtIndex.Size = New Size(109, 20)
			Me.radTxtIndex.TabIndex = 5
			Me.radTxtIndex.TabStop = False
			Me.radTxtIndex.Tag = "Right"
			' 
			' radLblText
			' 
			Me.radLblText.Location = New Point(13, 141)
			Me.radLblText.Name = "radLblText"
			Me.radLblText.Size = New Size(30, 18)
			Me.radLblText.TabIndex = 4
			Me.radLblText.Text = "Text:"
			' 
			' radLblItemIndex
			' 
			Me.radLblItemIndex.Location = New Point(7, 114)
			Me.radLblItemIndex.Name = "radLblItemIndex"
			Me.radLblItemIndex.Size = New Size(36, 18)
			Me.radLblItemIndex.TabIndex = 3
			Me.radLblItemIndex.Text = "Index:"
			' 
			' radCheckCaseSens
			' 
			Me.radCheckCaseSens.Location = New Point(8, 76)
			Me.radCheckCaseSens.Name = "radCheckCaseSens"
			Me.radCheckCaseSens.Size = New Size(98, 33)
			Me.radCheckCaseSens.TabIndex = 2
			Me.radCheckCaseSens.Text = "Case sensitive " & vbCrLf & "AutoComplete"

			' 
			' radLblAutoComplete
			' 
			Me.radLblAutoComplete.Location = New Point(16, 28)
			Me.radLblAutoComplete.Name = "radLblAutoComplete"
			Me.radLblAutoComplete.Size = New Size(114, 18)
			Me.radLblAutoComplete.TabIndex = 1
			Me.radLblAutoComplete.Text = "AutoComplete Mode:"
			' 
			' radComboAutoCompl
			' 
			Me.radComboAutoCompl.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radComboAutoCompl.FormatString = "{0}"
			radListDataItem18.Text = "None"
			radListDataItem18.TextWrap = True
			radListDataItem19.Text = "Suggest"
			radListDataItem19.TextWrap = True
			radListDataItem20.Text = "Append"
			radListDataItem20.TextWrap = True
			radListDataItem21.Text = "SuggestAppend"
			radListDataItem21.TextWrap = True
			Me.radComboAutoCompl.Items.Add(radListDataItem18)
			Me.radComboAutoCompl.Items.Add(radListDataItem19)
			Me.radComboAutoCompl.Items.Add(radListDataItem20)
			Me.radComboAutoCompl.Items.Add(radListDataItem21)
			Me.radComboAutoCompl.Location = New Point(13, 49)
			Me.radComboAutoCompl.Name = "radComboAutoCompl"
			' 
			' 
			' 
			Me.radComboAutoCompl.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboAutoCompl.Size = New Size(139, 20)
			Me.radComboAutoCompl.TabIndex = 0
			Me.radComboAutoCompl.Text = "Select mode:"

			' 
			' radComboDemo
			' 
			Me.radComboDemo.FormatString = "{0}"
			radListDataItem1.Image = My.Resources.BUL
			radListDataItem1.Text = "Bulgaria"
			radListDataItem1.TextWrap = True
			radListDataItem2.Image = My.Resources.DE
			radListDataItem2.Text = "Germany"
			radListDataItem2.TextWrap = True
			radListDataItem3.Image = My.Resources.FR
			radListDataItem3.Text = "France"
			radListDataItem3.TextWrap = True
            radListDataItem4.Image = My.Resources._IN
			radListDataItem4.Text = "India"
			radListDataItem4.TextWrap = True
			radListDataItem5.Image = My.Resources.US
			radListDataItem5.Text = "USA"
			radListDataItem5.TextWrap = True
			radListDataItem6.Image = My.Resources.CN
			radListDataItem6.Text = "China"
			radListDataItem6.TextWrap = True
			radListDataItem7.Image = My.Resources.JP
			radListDataItem7.Text = "Japan"
			radListDataItem7.TextWrap = True
			radListDataItem8.Image = My.Resources.NL
			radListDataItem8.Text = "Netherlands"
			radListDataItem8.TextWrap = True
			radListDataItem9.Image = My.Resources.BE
			radListDataItem9.Text = "Belgium"
			radListDataItem9.TextWrap = True
			radListDataItem10.Image = My.Resources.SP
			radListDataItem10.Text = "Spain"
			radListDataItem10.TextWrap = True
			radListDataItem11.Image = My.Resources.PT
			radListDataItem11.Text = "Portugal"
			radListDataItem11.TextWrap = True
			radListDataItem12.Image = My.Resources.CH
			radListDataItem12.Text = "Switzerland"
			radListDataItem12.TextWrap = True
			radListDataItem13.Image = My.Resources.SE
			radListDataItem13.Text = "Sweden"
			radListDataItem13.TextWrap = True
			radListDataItem14.Image = My.Resources.EG
			radListDataItem14.Text = "Egypt"
			radListDataItem14.TextWrap = True
			radListDataItem15.Image = My.Resources.CA
			radListDataItem15.Text = "Canada"
			radListDataItem15.TextWrap = True
			radListDataItem16.Image = My.Resources.RU
			radListDataItem16.Text = "Russia"
			radListDataItem16.TextWrap = True
			radListDataItem17.Image = My.Resources.BR
			radListDataItem17.Text = "Brazil"
			radListDataItem17.TextWrap = True
			Me.radComboDemo.Items.Add(radListDataItem1)
			Me.radComboDemo.Items.Add(radListDataItem2)
			Me.radComboDemo.Items.Add(radListDataItem3)
			Me.radComboDemo.Items.Add(radListDataItem4)
			Me.radComboDemo.Items.Add(radListDataItem5)
			Me.radComboDemo.Items.Add(radListDataItem6)
			Me.radComboDemo.Items.Add(radListDataItem7)
			Me.radComboDemo.Items.Add(radListDataItem8)
			Me.radComboDemo.Items.Add(radListDataItem9)
			Me.radComboDemo.Items.Add(radListDataItem10)
			Me.radComboDemo.Items.Add(radListDataItem11)
			Me.radComboDemo.Items.Add(radListDataItem12)
			Me.radComboDemo.Items.Add(radListDataItem13)
			Me.radComboDemo.Items.Add(radListDataItem14)
			Me.radComboDemo.Items.Add(radListDataItem15)
			Me.radComboDemo.Items.Add(radListDataItem16)
			Me.radComboDemo.Items.Add(radListDataItem17)
			Me.radComboDemo.Location = New Point(0, 0)
			Me.radComboDemo.Name = "radComboDemo"
			' 
			' 
			' 
			Me.radComboDemo.Size = New Size(306, 20)
			Me.radComboDemo.TabIndex = 0
            Me.radComboDemo.Text = "Choose country:"
			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			Me.Size = New Size(1142, 516)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radGroupBoxSettings, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBoxSettings.ResumeLayout(False)
			Me.radGroupBoxSettings.PerformLayout()
			CType(Me.radBtnAdd, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnRemove, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTxtText, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTxtIndex, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblText, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblItemIndex, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckCaseSens, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblAutoComplete, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radComboAutoCompl, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radComboDemo, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private imageList1 As ImageList
		Private radGroupBoxSettings As Telerik.WinControls.UI.RadGroupBox
		Private radLblAutoComplete As Telerik.WinControls.UI.RadLabel
		Private radComboAutoCompl As Telerik.WinControls.UI.RadDropDownList
		Private radBtnAdd As Telerik.WinControls.UI.RadButton
		Private radBtnRemove As Telerik.WinControls.UI.RadButton
		Private radTxtText As Telerik.WinControls.UI.RadTextBox
		Private radTxtIndex As Telerik.WinControls.UI.RadTextBox
		Private radLblText As Telerik.WinControls.UI.RadLabel
		Private radLblItemIndex As Telerik.WinControls.UI.RadLabel
		Private radCheckCaseSens As Telerik.WinControls.UI.RadCheckBox
		Private radComboDemo As Telerik.WinControls.UI.RadDropDownList
	End Class
End Namespace
