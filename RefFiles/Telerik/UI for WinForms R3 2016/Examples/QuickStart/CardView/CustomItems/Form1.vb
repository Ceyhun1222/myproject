Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.Data
Imports Telerik.WinControls.Primitives
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.CardView.CustomItems
    Partial Public Class Form1
        Inherits ExamplesForm
        Private features As List(Of String)
        Private makeFont As New Font("Segoe UI Light", 18.0F)
        Private yearFont As New Font("Segoe UI", 13.5F)
        Private checkBoxFont As New Font("Segoe UI", 10.5F)

        Public Sub New()
            InitializeComponent()

            Dim searchIcon As New ImagePrimitive()
            searchIcon.Image = My.Resources.TV_search
            searchIcon.Alignment = ContentAlignment.MiddleRight
            Me.commandBarTextBoxFilter.TextBoxElement.StretchHorizontally = True
            Me.commandBarTextBoxFilter.TextBoxElement.Alignment = ContentAlignment.MiddleRight
            Me.commandBarTextBoxFilter.TextBoxElement.Children.Add(searchIcon)
            Me.commandBarTextBoxFilter.TextBoxElement.TextBoxItem.Alignment = ContentAlignment.MiddleLeft
            AddHandler Me.commandBarTextBoxFilter.TextBoxElement.TextBoxItem.PropertyChanged, AddressOf TextBoxItem_PropertyChanged
            Me.radCardView1.AllowEdit = False
            Me.features = New List(Of String)() From { _
                "AirConditioner", _
                "Mp3Player", _
                "DVDPlayer", _
                "ABS", _
                "ASR", _
                "Navigation" _
            }
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)
            Me.carsRatesDataTableTableAdapter.Fill(Me.sofiaCarRentalDataSet.CarsRatesDataTable)

            Me.commandBarDropDownGroup.SelectedIndex = 1
        End Sub

        Protected Overrides Sub WireEvents()
            MyBase.WireEvents()
            AddHandler Me.radCardView1.CardViewItemCreating, AddressOf radCardView1_CardViewItemCreating
            AddHandler Me.radCardView1.CardViewItemFormatting, AddressOf radCardView1_CardViewItemFormatting
            AddHandler Me.radCardView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged
            AddHandler Me.commandBarTextBoxFilter.TextChanged, AddressOf commandBarTextBoxFilter_TextChanged
            AddHandler Me.commandBarDropDownSort.SelectedIndexChanged, AddressOf commandBarDropDownSort_SelectedIndexChanged
            AddHandler Me.commandBarDropDownGroup.SelectedIndexChanged, AddressOf commandBarDropDownGroup_SelectedIndexChanged
        End Sub

        Private Function ContainsFeature(item As ListViewDataItem, feature As String) As Boolean
            Return item(feature) IsNot Nothing AndAlso Convert.ToInt32(item(feature)) <> 0
        End Function

        Private checkBoxItemsFont As New Font("Segoe UI", 10.5F)
        Private Sub radCardView1_CardViewItemCreating(sender As Object, e As CardViewItemCreatingEventArgs)
            Dim groupItem As CardViewGroupItem = TryCast(e.NewItem, CardViewGroupItem)
            If groupItem IsNot Nothing Then
                groupItem.DrawText = True
                groupItem.Font = Me.checkBoxItemsFont
                groupItem.Text = "Extras"
            End If

            Dim cardViewItem As CardViewItem = TryCast(e.NewItem, CardViewItem)
            If cardViewItem Is Nothing OrElse String.IsNullOrEmpty(cardViewItem.FieldName) Then
                Return
            End If

            If Me.features.Contains(cardViewItem.FieldName) Then
                Dim checkBoxItem As New CheckBoxCardViewItem()
                checkBoxItem.FieldName = cardViewItem.FieldName
                e.NewItem = checkBoxItem
            End If
        End Sub

        Private Sub radCardView1_CardViewItemFormatting(sender As Object, e As CardViewItemFormattingEventArgs)
            Dim cardViewItem As CardViewItem = TryCast(e.Item, CardViewItem)
            If cardViewItem Is Nothing OrElse String.IsNullOrEmpty(cardViewItem.FieldName) Then
                Return
            End If

            If cardViewItem.FieldName = "ImageFileName" Then
                cardViewItem.TextSizeMode = LayoutItemTextSizeMode.Fixed
                cardViewItem.TextFixedSize = 0
                cardViewItem.EditorItem.DrawText = False
                cardViewItem.EditorItem.DrawImage = True
                Dim image__1 As Image = Image.FromFile(Application.StartupPath & "\Resources\CarRentalImages\" & e.VisualItem.Data("ImageFileName").ToString())
                Dim factor As Single = 160 / CSng(image__1.Height)
                Dim resizedImage As New Bitmap(image__1, New SizeF(factor * image__1.Width, factor * image__1.Height).ToSize())
                cardViewItem.EditorItem.Image = resizedImage
            ElseIf cardViewItem.FieldName = "Make" Then
                cardViewItem.EditorItem.Font = makeFont
                cardViewItem.EditorItem.Text = e.VisualItem.Data("Make").ToString() & "  " & e.VisualItem.Data("Model").ToString()
            ElseIf cardViewItem.FieldName = "CarYear" OrElse cardViewItem.FieldName = "CategoryName" Then
                cardViewItem.EditorItem.Font = yearFont
            End If

            Dim checkBoxItem As CheckBoxCardViewItem = TryCast(e.Item, CheckBoxCardViewItem)
            If checkBoxItem IsNot Nothing Then
                TryCast(checkBoxItem.EditorItem, CheckBoxEditorItem).Font = checkBoxFont
            End If
        End Sub

        Private Sub commandBarDropDownSort_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            RemoveHandler Me.radCardView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged

            Me.radCardView1.SortDescriptors.Clear()
            Select Case Me.commandBarDropDownSort.Text
                Case "Make"
                    Me.radCardView1.SortDescriptors.Add(New SortDescriptor("Make", ListSortDirection.Ascending))
                    Me.radCardView1.EnableSorting = True
                    Exit Select
                Case "Model"
                    Me.radCardView1.SortDescriptors.Add(New SortDescriptor("Model", ListSortDirection.Ascending))
                    Me.radCardView1.EnableSorting = True
                    Exit Select
                Case "Category"
                    Me.radCardView1.SortDescriptors.Add(New SortDescriptor("CategoryName", ListSortDirection.Ascending))
                    Me.radCardView1.EnableSorting = True
                    Exit Select
                Case "Year"
                    Me.radCardView1.SortDescriptors.Add(New SortDescriptor("CarYear", ListSortDirection.Ascending))
                    Me.radCardView1.EnableSorting = True
                    Exit Select
            End Select

            AddHandler Me.radCardView1.SortDescriptors.CollectionChanged, AddressOf SortDescriptors_CollectionChanged
        End Sub

        Private Sub commandBarDropDownGroup_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.radCardView1.GroupDescriptors.Clear()
            Select Case Me.commandBarDropDownGroup.Text
                Case "None"
                    Me.radCardView1.EnableGrouping = False
                    Me.radCardView1.ShowGroups = False
                    Exit Select
                Case "Make"
                    Me.radCardView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() {New SortDescriptor("Make", ListSortDirection.Ascending)}))
                    Me.radCardView1.EnableGrouping = True
                    Me.radCardView1.ShowGroups = True
                    Exit Select
                Case "Category"
                    Me.radCardView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() {New SortDescriptor("CategoryName", ListSortDirection.Ascending)}))
                    Me.radCardView1.EnableGrouping = True
                    Me.radCardView1.ShowGroups = True
                    Exit Select
                Case "Year"
                    Me.radCardView1.GroupDescriptors.Add(New GroupDescriptor(New SortDescriptor() {New SortDescriptor("CarYear", ListSortDirection.Ascending)}))
                    Me.radCardView1.EnableGrouping = True
                    Me.radCardView1.ShowGroups = True
                    Exit Select
            End Select
        End Sub

        Private Sub commandBarTextBoxFilter_TextChanged(sender As Object, e As EventArgs)
            Me.radCardView1.FilterDescriptors.Clear()

            If [String].IsNullOrEmpty(Me.commandBarTextBoxFilter.Text) Then
                Me.radCardView1.EnableFiltering = False
            Else
                Me.radCardView1.FilterDescriptors.LogicalOperator = FilterLogicalOperator.[Or]
                Me.radCardView1.FilterDescriptors.Add("Make", FilterOperator.Contains, Me.commandBarTextBoxFilter.Text)
                Me.radCardView1.FilterDescriptors.Add("Model", FilterOperator.Contains, Me.commandBarTextBoxFilter.Text)
                Me.radCardView1.EnableFiltering = True
            End If
        End Sub

        Private Sub TextBoxItem_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
            If e.PropertyName = "Bounds" Then
                commandBarTextBoxFilter.TextBoxElement.TextBoxItem.HostedControl.MaximumSize = New Size(CInt(commandBarTextBoxFilter.DesiredSize.Width) - 28, 0)
            End If
        End Sub

        Private Sub SortDescriptors_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
            If Me.radCardView1.SortDescriptors.Count = 0 Then
                Me.commandBarDropDownSort.SelectedIndex = 0
                Return
            End If

            Dim columnName As String = Me.radCardView1.Columns(Me.radCardView1.SortDescriptors(0).PropertyName).HeaderText
            If columnName = "Manufactured" Then
                columnName = "Year"
            End If
            Dim item As RadListDataItem = Me.commandBarDropDownSort.ListElement.FindItemExact(columnName, False)
            If item IsNot Nothing Then
                Me.commandBarDropDownSort.SelectedItem = item
            End If
        End Sub
    End Class

    Public Class CheckBoxCardViewItem
        Inherits CardViewItem
        Protected Overrides Sub CreateChildElements()
            MyBase.CreateChildElements()
            Me.TextSizeMode = LayoutItemTextSizeMode.Proportional
            Me.TextProportionalSize = 0
        End Sub

        Protected Overrides Function CreateEditorItem() As CardViewEditorItem
            Return New CheckBoxEditorItem()
        End Function

        Public Overrides Sub Synchronize()
            Dim cardVisualItem As CardListViewVisualItem = Me.FindAncestor(Of CardListViewVisualItem)()
            If Me.CardField Is Nothing OrElse cardVisualItem Is Nothing OrElse cardVisualItem.Data Is Nothing Then
                Return
            End If

            Dim checkBox As RadCheckBoxElement = TryCast(Me.EditorItem, CheckBoxEditorItem).Checkbox
            checkBox.Text = Me.CardField.HeaderText
            Dim data As Object = cardVisualItem.Data(Me.CardField)
            checkBox.Checked = Me.ContainsFeature(cardVisualItem.Data, Me.FieldName)
        End Sub

        Private Function ContainsFeature(item As ListViewDataItem, feature As String) As Boolean
            Return item(feature) IsNot Nothing AndAlso Convert.ToInt32(item(feature)) <> 0
        End Function
    End Class

    Public Class CheckBoxEditorItem
        Inherits CardViewEditorItem
        Private m_checkbox As RadCheckBoxElement

        Public Property Checkbox() As RadCheckBoxElement
            Get
                Return Me.m_checkbox
            End Get
            Set(value As RadCheckBoxElement)
                Me.m_checkbox = value
            End Set
        End Property

        Protected Overrides Sub CreateChildElements()
            MyBase.CreateChildElements()
            Me.m_checkbox = New RadCheckBoxElement()
            Me.Children.Add(Me.m_checkbox)
            AddHandler Me.m_checkbox.ToggleStateChanged, AddressOf checkbox_ToggleStateChanged
        End Sub

        Private Sub checkbox_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            ' on check box value changed we need to change the value in DataSource
        End Sub
    End Class
End Namespace