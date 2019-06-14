Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Layouts
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.GridView.CustomViews
	Partial Public Class Form1
		Inherits ExamplesForm
		#Region "Fields"

		Private columnChooserView As ColumnChooserGridViewElement
		Private detailView As GridViewDetailElement

		#End Region

		#Region "Constructor"

		Public Sub New()
			InitializeComponent()
		End Sub

		#End Region

		#Region "Event Handlers"

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)
			Me.InitializeGridControl()

			AddHandler gridView.ThemeNameChanged, AddressOf gridView_ThemeNameChanged
		End Sub

		Private Sub gridView_ThemeNameChanged(ByVal source As Object, ByVal args As ThemeNameChangedEventArgs)
			Me.detailView.DrawFill = True
			Me.detailView.BackColor = Color.FromArgb(240, 240, 240)
		End Sub

		#End Region

		#Region "Methods"

		Private Sub InitializeGridControl()
			Me.columnChooserView = New ColumnChooserGridViewElement()
			Me.gridView.GridViewElement.Panel.Children.Insert(0, columnChooserView)
			columnChooserView.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Right)

			Me.gridView.TableElement.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Top)
			'this.gridView.TableElement.Margin = new Padding(10, 0, 10, 10);

			Me.detailView = New GridViewDetailElement()
			Me.gridView.GridViewElement.Panel.Children.Insert(2, Me.detailView)
			Me.detailView.SetValue(DockLayoutPanel.DockProperty, Telerik.WinControls.Layouts.Dock.Bottom)
			'this.detailView.Margin = new Padding(10, 0, 10, 2);

			Me.employeesTableAdapter.Fill(Me.northwindDataSet.Employees)

			Me.gridView.Columns("LastName").SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending
			Me.gridView.Columns("City").SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending

			For Each col As GridViewColumn In Me.gridView.Columns
				If ShouldBeVisibleColumn(col.Name) Then
					col.Width = 100
					Continue For
				End If

				col.IsVisible = False

				If col.Name = "Photo" Then
					Me.gridView.Columns("Photo").VisibleInColumnChooser = False
				End If
			Next col

			Me.gridView.Rows(2).IsCurrent = True
			Me.gridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill
		End Sub

		Private Function ShouldBeVisibleColumn(ByVal column As String) As Boolean
			If column = "FirstName" OrElse column = "LastName" OrElse column = "HireDate" OrElse column = "HomePhone" OrElse column = "PostalCode" Then
				Return True
			End If

			Return False
		End Function

		#End Region

		Protected Overrides Sub WireEvents()
		End Sub
	End Class
End Namespace
