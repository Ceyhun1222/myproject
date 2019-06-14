Namespace Telerik.Examples.WinControls.DropDownListAndListControl.DropDownList.Sorting
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
			Me.imageList1 = New ImageList(Me.components)
			Me.radThemeManager1 = New Telerik.WinControls.RadThemeManager()
			Me.radComboDemo = New Telerik.WinControls.UI.RadDropDownList()
			Me.radGroupSettings = New Telerik.WinControls.UI.RadGroupBox()
			Me.radBtnAdd = New Telerik.WinControls.UI.RadButton()
			Me.radBtnRemove = New Telerik.WinControls.UI.RadButton()
			Me.radTxtText = New Telerik.WinControls.UI.RadTextBox()
			Me.radTxtIndex = New Telerik.WinControls.UI.RadTextBox()
			Me.radLblText = New Telerik.WinControls.UI.RadLabel()
			Me.radLblItemIndex = New Telerik.WinControls.UI.RadLabel()
			Me.radComboSortMode = New Telerik.WinControls.UI.RadDropDownList()
			Me.radLblSortMode = New Telerik.WinControls.UI.RadLabel()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radComboDemo, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupSettings.SuspendLayout()
			CType(Me.radBtnAdd, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnRemove, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTxtText, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTxtIndex, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblText, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblItemIndex, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radComboSortMode, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblSortMode, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radComboDemo)
			Me.radPanelDemoHolder.ForeColor = Color.Black
			Me.radPanelDemoHolder.MaximumSize = New Size(362, 100)
			Me.radPanelDemoHolder.MinimumSize = New Size(362, 100)
			' 
			' 
			' 
			Me.radPanelDemoHolder.RootElement.MaxSize = New Size(362, 100)
			Me.radPanelDemoHolder.RootElement.MinSize = New Size(362, 100)
			Me.radPanelDemoHolder.Size = New Size(362, 100)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupSettings)
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(230, 755)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupSettings, 0)
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
			' radComboDemo
			' 
			Me.radComboDemo.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
			radListDataItem1.Text = "Amsterdam"
			radListDataItem1.TextWrap = True
			radListDataItem2.Text = "Barcelona"
			radListDataItem2.TextWrap = True
			radListDataItem3.Text = "Bonn"
			radListDataItem3.TextWrap = True
			radListDataItem4.Text = "Brussels"
			radListDataItem4.TextWrap = True
			radListDataItem5.Text = "New York"
			radListDataItem5.TextWrap = True
			radListDataItem6.Text = "London"
			radListDataItem6.TextWrap = True
			radListDataItem7.Text = "Paris"
			radListDataItem7.TextWrap = True
			radListDataItem8.Text = "Sofia"
			radListDataItem8.TextWrap = True
			radListDataItem9.Text = "Prague"
			radListDataItem9.TextWrap = True
			radListDataItem10.Text = "Hamburg"
			radListDataItem10.TextWrap = True
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
			Me.radComboDemo.Location = New Point(0, 0)
			Me.radComboDemo.Name = "radComboDemo"
			Me.radComboDemo.NullText = "Choose City..."
			' 
			' 
			' 
			Me.radComboDemo.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboDemo.Size = New Size(306, 20)
            Me.radComboDemo.TabIndex = 0

			' 
			' radGroupSettings
			' 
			Me.radGroupSettings.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupSettings.Anchor = AnchorStyles.Top
			Me.radGroupSettings.Controls.Add(Me.radBtnAdd)
			Me.radGroupSettings.Controls.Add(Me.radBtnRemove)
			Me.radGroupSettings.Controls.Add(Me.radTxtText)
			Me.radGroupSettings.Controls.Add(Me.radTxtIndex)
			Me.radGroupSettings.Controls.Add(Me.radLblText)
			Me.radGroupSettings.Controls.Add(Me.radLblItemIndex)
			Me.radGroupSettings.Controls.Add(Me.radComboSortMode)
			Me.radGroupSettings.Controls.Add(Me.radLblSortMode)
			Me.radGroupSettings.FooterText = ""
			Me.radGroupSettings.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.radGroupSettings.HeaderText = " Settings "
			Me.radGroupSettings.Location = New Point(25, 6)
			Me.radGroupSettings.Name = "radGroupSettings"
			Me.radGroupSettings.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupSettings.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupSettings.Size = New Size(180, 228)
			Me.radGroupSettings.TabIndex = 0
			Me.radGroupSettings.Text = " Settings "
			' 
			' radBtnAdd
			' 
			Me.radBtnAdd.Location = New Point(39, 191)
			Me.radBtnAdd.Name = "radBtnAdd"
			Me.radBtnAdd.Size = New Size(61, 23)
			Me.radBtnAdd.TabIndex = 14
			Me.radBtnAdd.Tag = "NotTouch"
			Me.radBtnAdd.Text = "Add"

			' 
			' radBtnRemove
			' 
			Me.radBtnRemove.Location = New Point(106, 191)
			Me.radBtnRemove.Name = "radBtnRemove"
			Me.radBtnRemove.Size = New Size(64, 23)
			Me.radBtnRemove.TabIndex = 13
			Me.radBtnRemove.Tag = "NotTouch"
			Me.radBtnRemove.Text = "Remove"

			' 
			' radTxtText
			' 
			Me.radTxtText.Location = New Point(55, 156)
			Me.radTxtText.Name = "radTxtText"
			Me.radTxtText.Size = New Size(94, 20)
			Me.radTxtText.TabIndex = 12
			Me.radTxtText.TabStop = False
			Me.radTxtText.Tag = "Right"

			' 
			' radTxtIndex
			' 
			Me.radTxtIndex.Location = New Point(55, 120)
			Me.radTxtIndex.Name = "radTxtIndex"
			Me.radTxtIndex.ReadOnly = True
			Me.radTxtIndex.Size = New Size(94, 20)
			Me.radTxtIndex.TabIndex = 11
			Me.radTxtIndex.TabStop = False
			Me.radTxtIndex.Tag = "Right"
			' 
			' radLblText
			' 
			Me.radLblText.Location = New Point(20, 159)
			Me.radLblText.Name = "radLblText"
			Me.radLblText.Size = New Size(30, 18)
			Me.radLblText.TabIndex = 10
			Me.radLblText.Text = "Text:"
			' 
			' radLblItemIndex
			' 
			Me.radLblItemIndex.Location = New Point(14, 123)
			Me.radLblItemIndex.Name = "radLblItemIndex"
			Me.radLblItemIndex.Size = New Size(36, 18)
			Me.radLblItemIndex.TabIndex = 9
			Me.radLblItemIndex.Text = "Index:"
			' 
			' radComboSortMode
			' 
			Me.radComboSortMode.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			radListDataItem11.Text = "Ascending"
			radListDataItem11.TextWrap = True
			radListDataItem12.Text = "Descending"
			radListDataItem12.TextWrap = True
			radListDataItem13.Text = "None"
			radListDataItem13.TextWrap = True
			Me.radComboSortMode.Items.Add(radListDataItem11)
			Me.radComboSortMode.Items.Add(radListDataItem12)
			Me.radComboSortMode.Items.Add(radListDataItem13)
			Me.radComboSortMode.Location = New Point(14, 52)
			Me.radComboSortMode.Name = "radComboSortMode"
			' 
			' 
			' 
			Me.radComboSortMode.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboSortMode.Size = New Size(135, 20)
			Me.radComboSortMode.TabIndex = 1

			' 
			' radLblSortMode
			' 
			Me.radLblSortMode.Location = New Point(12, 28)
			Me.radLblSortMode.Name = "radLblSortMode"
			Me.radLblSortMode.Size = New Size(61, 18)
			Me.radLblSortMode.TabIndex = 0
			Me.radLblSortMode.Text = "Sort mode:"
			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			Me.Size = New Size(1142, 536)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radComboDemo, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupSettings.ResumeLayout(False)
			Me.radGroupSettings.PerformLayout()
			CType(Me.radBtnAdd, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnRemove, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTxtText, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTxtIndex, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblText, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblItemIndex, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radComboSortMode, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblSortMode, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub


		#End Region

		Private radComboDemo As Telerik.WinControls.UI.RadDropDownList
		Private imageList1 As ImageList
		Private radThemeManager1 As Telerik.WinControls.RadThemeManager
		Private radGroupSettings As Telerik.WinControls.UI.RadGroupBox
		Private radComboSortMode As Telerik.WinControls.UI.RadDropDownList
		Private radLblSortMode As Telerik.WinControls.UI.RadLabel
		Private radBtnAdd As Telerik.WinControls.UI.RadButton
		Private radBtnRemove As Telerik.WinControls.UI.RadButton
		Private radTxtText As Telerik.WinControls.UI.RadTextBox
		Private radTxtIndex As Telerik.WinControls.UI.RadTextBox
		Private radLblText As Telerik.WinControls.UI.RadLabel
		Private radLblItemIndex As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace