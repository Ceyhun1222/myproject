Imports Telerik.WinControls.UI
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.MenuStrip.EmbeddedCustomElements
	Partial Public Class Form1
		Inherits ExamplesForm
		Private sizeDropDownList As RadDropDownList
		Private fontDropDownList As RadDropDownList
		Private pageView As RadPageView

		Public Sub New()
			InitializeComponent()

			AddHandler radMenuDemo.ThemeNameChanged, AddressOf radMenuDemo_ThemeNameChanged
		End Sub

		Private Sub radMenuDemo_ThemeNameChanged(ByVal source As Object, ByVal args As ThemeNameChangedEventArgs)
			If pageView IsNot Nothing Then
				pageView.ThemeName = Me.radMenuDemo.ThemeName
				sizeDropDownList.ThemeName = Me.radMenuDemo.ThemeName
				fontDropDownList.ThemeName = Me.radMenuDemo.ThemeName
			End If
		End Sub

		Private Sub AddToolStrip()
			Dim commandBar As New RadCommandBar()
			commandBar.ImageList = Me.imageList1

			Dim element As New CommandBarRowElement()
			element.AllowDrag = False
			commandBar.Rows.Add(element)

			Dim toolStripItem As New CommandBarStripElement()
			toolStripItem.AllowDrag = False
			toolStripItem.OverflowButton.Visibility = ElementVisibility.Collapsed
			toolStripItem.StretchHorizontally = True
			element.Strips.Add(toolStripItem)

			Dim button1 As New CommandBarButton()
			button1.ImageIndex = 0
			toolStripItem.Items.Add(button1)

			Dim button2 As New CommandBarButton()
			button2.ImageIndex = 1
			toolStripItem.Items.Add(button2)

			Dim button3 As New CommandBarButton()
			button3.ImageIndex = 2
			toolStripItem.Items.Add(button3)

			Dim button4 As New CommandBarButton()
			button4.ImageIndex = 3
			toolStripItem.Items.Add(button4)

			Dim button5 As New CommandBarButton()
			button5.ImageIndex = 4
			toolStripItem.Items.Add(button5)

			commandBar.EndInit()

			Dim item As New RadMenuContentItem()
			item.ContentElement = New RadHostItem(commandBar)
			item.MinSize = New Size(120, 31)

			Me.radMenuItem1.Items.Add(item)
		End Sub

		Private Sub AddTabStrip()
			fontDropDownList = New RadDropDownList()
			fontDropDownList.ThemeName = Me.radMenuDemo.ThemeName
			fontDropDownList.Margin = New Padding(25, 5, 5, 5)
			fontDropDownList.Text = "Select Font"
			fontDropDownList.Items.Add(New RadListDataItem("Arial"))
			fontDropDownList.Items.Add(New RadListDataItem("Tahoma"))
			fontDropDownList.Items.Add(New RadListDataItem("Times New Roman"))
			fontDropDownList.Items.Add(New RadListDataItem("Verdana"))

			sizeDropDownList = New RadDropDownList()
			sizeDropDownList.ThemeName = Me.radMenuDemo.ThemeName
			sizeDropDownList.Margin = New Padding(25, 5, 5, 5)
			sizeDropDownList.Text = "8"
			sizeDropDownList.Items.Add(New RadListDataItem("7"))
			sizeDropDownList.Items.Add(New RadListDataItem("8"))
			sizeDropDownList.Items.Add(New RadListDataItem("9"))
			sizeDropDownList.Items.Add(New RadListDataItem("10"))
			sizeDropDownList.Items.Add(New RadListDataItem("11"))
			sizeDropDownList.Items.Add(New RadListDataItem("12"))

			pageView = New RadPageView()
			pageView.ThemeName = Me.radMenuDemo.ThemeName
			Dim fontPage As New RadPageViewPage()
			fontPage.Text = "Font"
			fontPage.Controls.Add(fontDropDownList)
			pageView.Pages.Add(fontPage)
			Dim sizePage As New RadPageViewPage()
			sizePage.Text = "Size"
			sizePage.Controls.Add(sizeDropDownList)
			pageView.Pages.Add(sizePage)

			Dim contentItem As New RadMenuContentItem()
			contentItem.ContentElement = New RadHostItem(pageView)
			contentItem.MinSize = New Size(120, 100)

			Me.radMenuItem2.Items.Insert(6, contentItem)
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			AddToolStrip()
			AddTabStrip()
		End Sub

		Protected Overrides Sub WireEvents()
		End Sub
	End Class
End Namespace