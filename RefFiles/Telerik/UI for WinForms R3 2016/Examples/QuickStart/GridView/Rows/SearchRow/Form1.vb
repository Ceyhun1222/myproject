Imports System.Windows.Forms
Imports Telerik.Examples.WinControls.DataSources
Imports Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.GridView.Rows.SearchRow
	Public Partial Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			Dim nwindDataSet As New NorthwindDataSet()
			Dim customersTableAdapter As New CustomersTableAdapter()
			Dim customersBindingSource As New BindingSource()
			customersTableAdapter.Fill(nwindDataSet.Customers)
			customersBindingSource.DataSource = nwindDataSet.Customers
			radGridView1.DataSource = customersBindingSource
		End Sub

		Protected Overrides Sub OnLoad(e As EventArgs)
			MyBase.OnLoad(e)

			Me.radGridView1.Columns("Region").IsVisible = False
			Me.radGridView1.Columns("Phone").IsVisible = False
			Me.radGridView1.Columns("Fax").IsVisible = False
		End Sub
	End Class
End Namespace
