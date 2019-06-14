Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.ListView.CustomItems
	Partial Public Class Form1
		Inherits ListViewExamplesControl
		Public Sub New()
			InitializeComponent()

			Me.radListView1.ItemSize = New Size(200, 300)
			Me.radListView1.AllowArbitraryItemHeight = True
			Me.radListView1.ItemSpacing = 10
			Me.radListView1.EnableKineticScrolling = True

			Me.radListView1.ListViewElement.ViewElement.ViewElement.Margin = New Padding(0, 10, 0, 10)
			Me.radListView1.ListViewElement.ViewElement.Orientation = Orientation.Horizontal
		End Sub

		Private Sub radListView1_VisualItemCreating(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.ListViewVisualItemCreatingEventArgs) Handles radListView1.VisualItemCreating
			e.VisualItem = New CustomVisualItem()
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

            Me.albumsDataTableTableAdapter.Fill(Me.musicCollectionDataSet.AlbumsDataTableProperty)
		End Sub

		Private Sub radCheckBox1_ToggleStateChanged(ByVal sender As Object, ByVal args As Telerik.WinControls.UI.StateChangedEventArgs) Handles radCheckBox1.ToggleStateChanged
			Me.radListView1.EnableKineticScrolling = Me.radCheckBox1.Checked
		End Sub

		Public Overrides ReadOnly Property ContentControl() As Control
			Get
				Return Me.radListView1
			End Get
		End Property

	End Class
End Namespace
