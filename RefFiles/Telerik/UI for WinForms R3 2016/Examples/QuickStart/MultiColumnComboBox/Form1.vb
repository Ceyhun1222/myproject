Imports System
Imports System.Drawing
Imports Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters
Imports Telerik.Examples.WinControls.DataSources
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Data
Imports Telerik.Examples.WinControls.Editors.ComboBox

Namespace Telerik.Examples.WinControls.MultiColumnComboBox
    Partial Public Class Form1
        Inherits EditorExampleBaseForm
        Public Sub New()
            InitializeComponent()

            Me.SelectedControl = Me.radMultiColumnComboBox1
            Me.radMultiColumnComboBox1.AutoSizeDropDownToBestFit = True

            Dim multiColumnComboElement As RadMultiColumnComboBoxElement = Me.radMultiColumnComboBox1.MultiColumnComboBoxElement
            multiColumnComboElement.DropDownSizingMode = SizingMode.UpDownAndRightBottom
            multiColumnComboElement.DropDownMinSize = New Size(420, 300)

            multiColumnComboElement.EditorControl.MasterTemplate.AutoGenerateColumns = False

            Dim column As New GridViewTextBoxColumn("CustomerID")
            column.HeaderText = "Customer ID"
            multiColumnComboElement.Columns.Add(column)
            column = New GridViewTextBoxColumn("ContactName")
            column.HeaderText = "Contact Name"
            multiColumnComboElement.Columns.Add(column)
            column = New GridViewTextBoxColumn("ContactTitle")
            column.HeaderText = "Contact Title"
            multiColumnComboElement.Columns.Add(column)
            column = New GridViewTextBoxColumn("Country")
            column.HeaderText = "Country"
            multiColumnComboElement.Columns.Add(column)
            column = New GridViewTextBoxColumn("Phone")
            column.HeaderText = "Phone"
            multiColumnComboElement.Columns.Add(column)


            Me.radMultiColumnComboBox1.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        End Sub

       

        Protected Overrides Sub WireEvents()
            AddHandler Me.radCheckRotate.ToggleStateChanged, AddressOf OnCheckRotate_ToggleStateChanged
            AddHandler Me.radCheckRTL.ToggleStateChanged, AddressOf OnCheckBoxRTL_ToggleStateChanged
            AddHandler Me.radComboAutoCompl.SelectedIndexChanged, AddressOf radComboAutoCompl_SelectedIndexChanged
            Me.radComboAutoCompl.SelectedIndex = 3
        End Sub

        Sub radComboAutoCompl_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Select Case Me.radComboAutoCompl.SelectedIndex
                Case 0
                    Me.radMultiColumnComboBox1.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
                    Exit Select
                Case 1
                    Me.radMultiColumnComboBox1.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
                    Exit Select
                Case 2
                    Me.radMultiColumnComboBox1.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
                    Exit Select
                Case 3
                    Me.radMultiColumnComboBox1.MultiColumnComboBoxElement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
                    Exit Select
            End Select
        End Sub


        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)

            Dim nwindDataSet As New NorthwindDataSet()
            Dim customersTableAdapter As New CustomersTableAdapter()
            customersTableAdapter.Fill(nwindDataSet.Customers)

            Me.radMultiColumnComboBox1.DataSource = nwindDataSet.Customers


            Dim descriptor As New FilterDescriptor(Me.radMultiColumnComboBox1.DisplayMember, FilterOperator.StartsWith, String.Empty)
            Me.radMultiColumnComboBox1.EditorControl.FilterDescriptors.Add(descriptor)
            Me.radMultiColumnComboBox1.DropDownStyle = RadDropDownStyle.DropDown
            ' Filtering END
        End Sub

        Private Sub OnCheckBoxRTL_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radMultiColumnComboBox1.RightToLeft = If(Me.radCheckRTL.Checked, System.Windows.Forms.RightToLeft.Yes, System.Windows.Forms.RightToLeft.No)
        End Sub

        Private Sub OnCheckRotate_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            Me.radMultiColumnComboBox1.DblClickRotate = Me.radCheckRotate.Checked
        End Sub
    End Class
End Namespace