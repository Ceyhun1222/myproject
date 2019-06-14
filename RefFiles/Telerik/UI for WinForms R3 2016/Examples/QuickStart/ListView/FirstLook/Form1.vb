Imports System.ComponentModel
Imports System.Text
Imports Telerik.Examples.WinControls.DataSources
Imports System.IO
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.Data
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.Primitives
Imports Telerik.WinControls.Layouts

Namespace Telerik.Examples.WinControls.ListView.FirstLook
	Partial Public Class Form1
        Inherits ListViewExamplesControl

		Public Overrides ReadOnly Property ContentControl() As Control
			Get
				Return Me.radPanel1
			End Get
		End Property

		Public Sub New()
			InitializeComponent()

			Dim searchIcon As New ImagePrimitive()
			searchIcon.Image = My.Resources.TV_search
			searchIcon.Alignment = ContentAlignment.MiddleRight
			Me.commandBarTextBoxFilter.TextBoxElement.StretchHorizontally = True
			Me.commandBarTextBoxFilter.TextBoxElement.Alignment = ContentAlignment.MiddleRight
			Me.commandBarTextBoxFilter.TextBoxElement.Children.Add(searchIcon)
			Me.commandBarTextBoxFilter.TextBoxElement.TextBoxItem.Alignment = ContentAlignment.MiddleLeft
			AddHandler commandBarTextBoxFilter.TextBoxElement.TextBoxItem.PropertyChanged, AddressOf TextBoxItem_PropertyChanged

			AddHandler radListView1.VisualItemFormatting, AddressOf radListView1_VisualItemFormatting
			AddHandler radListView1.ViewTypeChanged, AddressOf radListView1_ViewTypeChanged
			AddHandler radListView1.CellFormatting, AddressOf radListView1_CellFormatting
			AddHandler radListView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged

			Me.radListView1.AllowEdit = False
			Me.radListView1.AllowRemove = False

			Me.radListView1_ViewTypeChanged(Me, EventArgs.Empty)
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radListView1.BindingCompleted, AddressOf radListView1_BindingCompleted
			AddHandler radListView1.VisualItemCreating, AddressOf radListView1_VisualItemCreating
			AddHandler radListView1.ItemDataBound, AddressOf radListView1_ItemDataBound
			AddHandler radListView1.ColumnCreating, AddressOf radListView1_ColumnCreating
			AddHandler commandBarToggleDetails.ToggleStateChanging, AddressOf ViewToggleButton_ToggleStateChanging
			AddHandler commandBarToggleDetails.ToggleStateChanged, AddressOf ViewToggleButton_ToggleStateChanged
			AddHandler commandBarToggleTiles.ToggleStateChanging, AddressOf ViewToggleButton_ToggleStateChanging
			AddHandler commandBarToggleTiles.ToggleStateChanged, AddressOf ViewToggleButton_ToggleStateChanged
			AddHandler commandBarToggleList.ToggleStateChanging, AddressOf ViewToggleButton_ToggleStateChanging
			AddHandler commandBarToggleList.ToggleStateChanged, AddressOf ViewToggleButton_ToggleStateChanged
			AddHandler commandBarTextBoxFilter.TextChanged, AddressOf commandBarTextBoxFilter_TextChanged
			AddHandler commandBarDropDownSort.SelectedIndexChanged, AddressOf commandBarDropDownSort_SelectedIndexChanged
			AddHandler commandBarDropDownGroup.SelectedIndexChanged, AddressOf commandBarDropDownGroup_SelectedIndexChanged
		End Sub

		Private Sub SortDescriptors_CollectionChanged(ByVal sender As Object, ByVal e As NotifyCollectionChangedEventArgs)
			If Me.radListView1.SortDescriptors.Count = 0 Then
				Me.commandBarDropDownSort.SelectedIndex = 0
				Return
			End If

			Dim columnName As String = Me.radListView1.Columns(Me.radListView1.SortDescriptors(0).PropertyName).HeaderText
			If columnName = "Manufactured" Then
				columnName = "Year"
			End If
			Dim item As RadListDataItem = Me.commandBarDropDownSort.ListElement.FindItemExact(columnName, False)
			If item IsNot Nothing Then
				Me.commandBarDropDownSort.SelectedItem = item
			End If
		End Sub

		Private Sub radListView1_BindingCompleted(ByVal sender As Object, ByVal e As EventArgs)
			Me.radListView1.Columns("ImageFileName").Width = 180
			Me.radListView1.Columns("ImageFileName").MinWidth = 180
			Me.radListView1.Columns("Make").Width = 90
			Me.radListView1.Columns("Make").MinWidth = 90
			Me.radListView1.Columns("Model").Width = 110
			Me.radListView1.Columns("Model").MinWidth = 110
			Me.radListView1.Columns("CarYear").Width = 90
			Me.radListView1.Columns("CarYear").MinWidth = 90
			Me.radListView1.Columns("CategoryName").Width = 90
			Me.radListView1.Columns("CategoryName").MinWidth = 90

			Dim pictureColumnIndex As Integer = Me.radListView1.Columns.IndexOf("ImageFileName")
			Me.radListView1.Columns.Move(pictureColumnIndex, 0)
		End Sub

        Private Sub radListView1_ColumnCreating(ByVal sender As Object, ByVal e As ListViewColumnCreatingEventArgs) Handles radListView1.ColumnCreating
            If e.Column.FieldName = "CarID" Then
                e.Column.Visible = False
            End If

            If e.Column.FieldName = "ImageFileName" Then
                e.Column.HeaderText = "Picture"
            End If

            If e.Column.FieldName = "CarYear" Then
                e.Column.HeaderText = "Manufactured"
            End If

            If e.Column.FieldName = "CategoryName" Then
                e.Column.HeaderText = "Category"
            End If

            If e.Column.FieldName = "Mp3Player" Then
                e.Column.HeaderText = "MP3"
            End If

            If e.Column.FieldName = "DVDPlayer" Then
                e.Column.HeaderText = "DVD"
            End If

            If e.Column.FieldName = "AirConditioner" Then
                e.Column.HeaderText = "Air Cond."
                e.Column.Width = 90
                e.Column.MinWidth = 90
            End If

            If e.Column.FieldName = "Daily" OrElse e.Column.FieldName = "Weekly" OrElse e.Column.FieldName = "Monthly" OrElse e.Column.FieldName = "Available" Then
                e.Column.Visible = False
            End If

            If features.Contains(e.Column.FieldName) Then
                e.Column.Width = 55
                e.Column.MinWidth = 55
            End If
        End Sub

		Private Sub radListView1_CellFormatting(ByVal sender As Object, ByVal e As ListViewCellFormattingEventArgs)

			If TypeOf e.CellElement Is DetailListViewHeaderCellElement Then
				Return
			End If

			If e.CellElement.Data.HeaderText = "Picture" Then
				CType(e.CellElement, DetailListViewDataCellElement).Image = (CType(e.CellElement, DetailListViewDataCellElement)).Row.Image
				e.CellElement.Text = ""
				e.CellElement.ImageAlignment = ContentAlignment.MiddleCenter
				e.CellElement.TextImageRelation = TextImageRelation.Overlay
			Else
				e.CellElement.Image = Nothing
			End If

			If e.CellElement.Data.HeaderText = "Make" OrElse e.CellElement.Data.HeaderText = "Model" Then
				e.CellElement.Text = "<html><span style=""color:#161112;font-size:11.5pt;"">" & e.CellElement.Text & "</span>"
			ElseIf Me.features.Contains(e.CellElement.Data.FieldName) Then
				Dim containsFeature As Boolean = Me.ContainsFeature((CType(e.CellElement, DetailListViewDataCellElement)).Row, e.CellElement.Data.FieldName)
				Dim color As String = If((containsFeature), "#050F70", "#B52822")
				e.CellElement.ForeColor = CType(New ColorConverter().ConvertFromString(color), Color)
				e.CellElement.Font = New Font(e.CellElement.Font.FontFamily, 10, GraphicsUnit.Point)
				e.CellElement.Text = If((containsFeature), "YES", "NO")
			ElseIf e.CellElement.Data.HeaderText <> "Picture" Then
				e.CellElement.ForeColor = CType(New ColorConverter().ConvertFromString("#050F70"), Color)
				e.CellElement.Font = New Font(e.CellElement.Font.FontFamily, 10, GraphicsUnit.Point)
				e.CellElement.Text = e.CellElement.Text
			End If
		End Sub

		Private Sub radListView1_ViewTypeChanged(ByVal sender As Object, ByVal e As EventArgs)
			Select Case radListView1.ViewType
				Case ListViewType.ListView
					SetupSimpleListView()
				Case ListViewType.IconsView
					SetupIconsView()
				Case ListViewType.DetailsView
					SetupDetailsView()
			End Select
		End Sub

		Private Sub SetupDetailsView()
			Me.radListView1.ItemSize = New Size(0, 110)
		End Sub

		Private Sub SetupIconsView()
			Me.radListView1.ItemSize = New Size(295, 120)
			Me.radListView1.ItemSpacing = 5
			Me.radListView1.GroupIndent = 0
		End Sub

		Private Sub SetupSimpleListView()
			Me.radListView1.AllowArbitraryItemHeight = True

		End Sub

		Private features As New List(Of String)(New String() {"AirConditioner", "Mp3Player", "DVDPlayer", "ABS", "ASR", "Navigation"})

		Private Function ContainsFeature(ByVal item As ListViewDataItem, ByVal feature As String) As Boolean
			Return item(feature) IsNot Nothing AndAlso Convert.ToInt32(item(feature)) <> 0
		End Function

		Private Function GetFeatures(ByVal item As ListViewDataItem) As String
			Dim featuresString As New StringBuilder()

			For Each feature As String In Me.features
				If ContainsFeature(item, feature) Then
					featuresString.Append(feature & ", ")
				End If
			Next feature

			If featuresString.Length > 1 Then
				featuresString.Remove(featuresString.Length - 2, 2)
			End If

			Return featuresString.ToString()
		End Function


		Private Sub radListView1_VisualItemCreating(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.ListViewVisualItemCreatingEventArgs)
			If Me.radListView1.ViewType = ListViewType.ListView AndAlso Not(TypeOf e.VisualItem Is BaseListViewGroupVisualItem) Then
				e.VisualItem = New CarsListVisualItem()
			End If
		End Sub

		Private Sub radListView1_VisualItemFormatting(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.ListViewVisualItemEventArgs)
			If TypeOf e.VisualItem Is BaseListViewGroupVisualItem Then
				Return
			End If

			If Me.radListView1.ViewType = ListViewType.IconsView Then
                e.VisualItem.Text = "<html>" & "<span style=""color:#040203;font-size:12pt;"">" & e.VisualItem.Data("Make").ToString() & " " & e.VisualItem.Data("Model").ToString() & "</span>" & "<br><span style=""color:#040203;font-size:9pt;"">" & e.VisualItem.Data("CarYear").ToString() & ", " & e.VisualItem.Data("CategoryName").ToString() & "</span>" & "<br><br><span style=""color:#112164;font-size:9pt;"">" & GetFeatures(e.VisualItem.Data).ToCharArray() & "</span>"

				e.VisualItem.ImageLayout = ImageLayout.Center
				e.VisualItem.ImageAlignment = ContentAlignment.MiddleCenter
			End If

			If Me.radListView1.ViewType = ListViewType.ListView Then
				e.VisualItem.Padding = New Padding(5, 5, 0, 5)
				e.VisualItem.Layout.LeftPart.Margin = New Padding(0, 0, 5, 0)
			End If
		End Sub

		Private Sub radListView1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.ListViewItemEventArgs)
            e.Item.Image = Image.FromFile(Application.StartupPath & "\Resources\CarRentalImages\" & e.Item("ImageFileName").ToString())

		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)
			Me.carsRatesDataTableTableAdapter.Fill(Me.sofiaCarRentalDataSet.CarsRatesDataTable)

			Me.commandBarDropDownGroup.SelectedIndex = 1
		End Sub

		Private updatingToggleState As Boolean = False

		Private Sub ViewToggleButton_ToggleStateChanged(ByVal sender As Object, ByVal args As StateChangedEventArgs)
			If updatingToggleState Then
				Return
			End If

			Me.updatingToggleState = True

			If Me.commandBarToggleDetails IsNot sender Then
				Me.commandBarToggleDetails.ToggleState = ToggleState.Off
			End If

			If Me.commandBarToggleList IsNot sender Then
				Me.commandBarToggleList.ToggleState = ToggleState.Off
			End If

			If Me.commandBarToggleTiles IsNot sender Then
				Me.commandBarToggleTiles.ToggleState = ToggleState.Off
			End If

			Me.updatingToggleState = False

			If Me.commandBarToggleDetails.ToggleState = ToggleState.On Then
				Me.radListView1.ViewType = ListViewType.DetailsView
			End If

			If Me.commandBarToggleList.ToggleState = ToggleState.On Then
				Me.radListView1.ViewType = ListViewType.ListView
			End If

			If Me.commandBarToggleTiles.ToggleState = ToggleState.On Then
				Me.radListView1.ViewType = ListViewType.IconsView
			End If

		End Sub

		Private Sub ViewToggleButton_ToggleStateChanging(ByVal sender As Object, ByVal args As StateChangingEventArgs)
			If (Not updatingToggleState) AndAlso args.OldValue = ToggleState.On Then
				args.Cancel = True
			End If
		End Sub

		Private Sub commandBarDropDownSort_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
			RemoveHandler Me.radListView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged

			Me.radListView1.SortDescriptors.Clear()
			Select Case Me.commandBarDropDownSort.Text
				Case "Make"
					Me.radListView1.SortDescriptors.Add(New SortDescriptor("Make", ListSortDirection.Ascending))
					Me.radListView1.EnableSorting = True
				Case "Model"
					Me.radListView1.SortDescriptors.Add(New SortDescriptor("Model", ListSortDirection.Ascending))
					Me.radListView1.EnableSorting = True
				Case "Category"
					Me.radListView1.SortDescriptors.Add(New SortDescriptor("CategoryName", ListSortDirection.Ascending))
					Me.radListView1.EnableSorting = True
				Case "Year"
					Me.radListView1.SortDescriptors.Add(New SortDescriptor("CarYear", ListSortDirection.Ascending))
					Me.radListView1.EnableSorting = True
			End Select

			AddHandler Me.radListView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged
		End Sub

		Private Sub commandBarDropDownGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
			Me.radListView1.GroupDescriptors.Clear()
			Select Case Me.commandBarDropDownGroup.Text
				Case "None"
					Me.radListView1.EnableGrouping = False
					Me.radListView1.ShowGroups = False
				Case "Make"
					Me.radListView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() { New SortDescriptor("Make", ListSortDirection.Ascending) }))
					Me.radListView1.EnableGrouping = True
					Me.radListView1.ShowGroups = True
				Case "Category"
					Me.radListView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() { New SortDescriptor("CategoryName", ListSortDirection.Ascending) }))
					Me.radListView1.EnableGrouping = True
					Me.radListView1.ShowGroups = True
				Case "Year"
					Me.radListView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() { New SortDescriptor("CarYear", ListSortDirection.Ascending) }))
					Me.radListView1.EnableGrouping = True
					Me.radListView1.ShowGroups = True
			End Select
		End Sub

		Private Sub commandBarTextBoxFilter_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
			Me.radListView1.FilterDescriptors.Clear()

			If String.IsNullOrEmpty(Me.commandBarTextBoxFilter.Text) Then
				Me.radListView1.EnableFiltering = False
			Else
				Me.radListView1.FilterDescriptors.LogicalOperator = FilterLogicalOperator.Or
				Me.radListView1.FilterDescriptors.Add("Make", FilterOperator.Contains, Me.commandBarTextBoxFilter.Text)
				Me.radListView1.FilterDescriptors.Add("Model", FilterOperator.Contains, Me.commandBarTextBoxFilter.Text)
				Me.radListView1.EnableFiltering = True
			End If
		End Sub

		Private Sub TextBoxItem_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
			If e.PropertyName = "Bounds" Then
				commandBarTextBoxFilter.TextBoxElement.TextBoxItem.HostedControl.MaximumSize = New Size(CInt(Fix(commandBarTextBoxFilter.DesiredSize.Width)) - 28, 0)
			End If
		End Sub
	End Class

	Public Class CarsListVisualItem
		Inherits SimpleListViewVisualItem
		Private element1 As LightVisualElement
		Private element2 As LightVisualElement
		Private element3 As LightVisualElement
		Private element4 As LightVisualElement
'INSTANT VB NOTE: The variable layout was renamed since Visual Basic does not allow class members with the same name:
		Private layout_Renamed As StackLayoutPanel

		Protected Overrides Sub CreateChildElements()
			MyBase.CreateChildElements()

			Me.layout_Renamed = New StackLayoutPanel()
			Me.layout_Renamed.EqualChildrenWidth = True
			Me.layout_Renamed.Margin = New Padding(180, 30, 0, 0)

			Me.element1 = New LightVisualElement()
			element1.TextAlignment = ContentAlignment.MiddleLeft
			element1.MinSize = New Size(170, 0)
			element1.NotifyParentOnMouseInput = True
			element1.ShouldHandleMouseInput = False
			Me.layout_Renamed.Children.Add(Me.element1)

			Me.element2 = New LightVisualElement()
			element2.TextAlignment = ContentAlignment.MiddleLeft
			element2.MinSize = New Size(170, 0)
			element2.NotifyParentOnMouseInput = True
			element2.ShouldHandleMouseInput = False
			Me.layout_Renamed.Children.Add(Me.element2)

			Me.element3 = New LightVisualElement()
			element3.TextAlignment = ContentAlignment.MiddleLeft
			element3.MinSize = New Size(170, 0)
			element3.NotifyParentOnMouseInput = True
			element3.ShouldHandleMouseInput = False
			Me.layout_Renamed.Children.Add(Me.element3)

			Me.element4 = New LightVisualElement()
			element4.TextAlignment = ContentAlignment.MiddleLeft
			element4.MinSize = New Size(170, 0)
			element4.NotifyParentOnMouseInput = True
			element4.ShouldHandleMouseInput = False
			Me.layout_Renamed.Children.Add(Me.element4)

			Me.Children.Add(Me.layout_Renamed)
		End Sub

		Private Function ContainsFeature(ByVal item As ListViewDataItem, ByVal feature As String) As Boolean
			Return item(feature) IsNot Nothing AndAlso Convert.ToInt32(item(feature)) <> 0
		End Function

		Protected Overrides Sub SynchronizeProperties()
			MyBase.SynchronizeProperties()

            Me.Text = "<html><span style=""color:#141718;font-size:14.5pt;"">" & Me.Data("Make").ToString() & " " & Me.Data("Model").ToString() & "</span>"

            Me.element1.Text = "<html><span style=""color:#010102;font-size:10.5pt;font-family:Segoe UI Semibold;"">" & "Manufactured:<span style=""color:#13224D;font-family:Segoe UI;"">" & Me.Data("CarYear").ToString() & "</span>" & "<br>Category:<span style=""color:#13224D;font-family:Segoe UI;"">" & Me.Data("CategoryName").ToString() & "</span></span>"

			Me.element2.Text = "<html><span style=""color:#010102;font-size:10.5pt;font-family:Segoe UI Semibold;"">" & "ABS:" & (If(Me.ContainsFeature(Me.Data, "ABS"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "<br>ESR:" & (If(Me.ContainsFeature(Me.Data, "ESR"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "</span>"

			Me.element3.Text = "<html><span style=""color:#010102;font-size:10.5pt;font-family:Segoe UI Semibold;"">" & "MP3 Player:" & (If(Me.ContainsFeature(Me.Data, "Mp3Player"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "<br>DVD Player:" & (If(Me.ContainsFeature(Me.Data, "DVDPlayer"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "</span>"

			Me.element4.Text = "<html><span style=""color:#010102;font-size:10.5pt;font-family:Segoe UI Semibold;"">" & "Air Conditioner:" & (If(Me.ContainsFeature(Me.Data, "AirConditioner"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "<br>Navigation:" & (If(Me.ContainsFeature(Me.Data, "Navigation"), "<span style=""color:#13224D;font-family:Segoe UI;"">YES", "<span style=""color:#D71B0E;"">NO")) & "</span>" & "</span>"

			Me.TextAlignment = ContentAlignment.TopLeft
		End Sub

		Protected Overrides ReadOnly Property ThemeEffectiveType() As Type
			Get
				Return GetType(SimpleListViewVisualItem)
			End Get
		End Property



	End Class
End Namespace
