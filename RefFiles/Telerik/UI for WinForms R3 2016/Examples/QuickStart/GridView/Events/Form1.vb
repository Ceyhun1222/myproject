Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.GridView.Events
	Partial Public Class Form1
		Inherits ExamplesForm
		'additional custom columns
		Protected lookUpColumn As GridViewComboBoxColumn = Nothing
		Protected commandColumn As GridViewCommandColumn = Nothing

		Public Sub New()
			InitializeComponent()
			Me.SelectedControl = Me.radGridView1
			Me.radGridView1.EnableHotTracking = True
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Me.carsTableAdapter.Fill(Me.nwindDataSet.Cars)
			radGridView1.DataSource = Me.carsBindingSource
			Me.radGridView1.Columns(1).IsVisible = False
			Me.radGridView1.Columns(4).IsVisible = False
			Me.radGridView1.Columns("Picture").IsVisible = False

			'add new lookup column
			Dim table As New DataTable()
			table.Columns.Add("KBytes")
			table.Rows.Add(21)
			table.Rows.Add(30)
			table.Rows.Add(99)
			table.Rows.Add(50)

			lookUpColumn = New GridViewComboBoxColumn()
			radGridView1.MasterTemplate.Columns.Add(lookUpColumn)
			lookUpColumn.HeaderText = "ComboBox"
			lookUpColumn.TextAlignment = ContentAlignment.MiddleRight
			lookUpColumn.DataSource = table
			lookUpColumn.FieldName = "KBytes"
			lookUpColumn.ValueMember = "KBytes"
			lookUpColumn.Name = "comboColumnKBytes"

			radGridView1.Columns("Date").TextAlignment = ContentAlignment.MiddleRight
			'add button column
			commandColumn = New GridViewCommandColumn()
			commandColumn.HeaderText = "Command"
			radGridView1.MasterTemplate.Columns.Add(commandColumn)

			Me.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill

			'add command click event
			AddHandler radGridView1.CommandCellClick, AddressOf radGridView1_CommandCellClick
			AddHandler radGridView1.CellFormatting, AddressOf radGridView1_CellFormatting
			AddHandler radGridView1.CellBeginEdit, AddressOf radGridView1_CellBeginEdit
			AddHandler radGridView1.CellEndEdit, AddressOf radGridView1_CellEndEdit
			AddHandler radGridView1.EditorRequired, AddressOf radGridView1_EditorRequired
			AddHandler radGridView1.DoubleClick, AddressOf radGridView1_DoubleClick
		End Sub

		Private Sub radGridView1_EditorRequired(ByVal sender As Object, ByVal e As EditorRequiredEventArgs)
			AddTextToListBox("     EditorRequired  " & e.EditorType.ToString())
		End Sub

		#Region "Events"

		Private Sub radGridView1_CellFormatting(ByVal sender As Object, ByVal e As CellFormattingEventArgs)
			If TypeOf e.CellElement Is GridCommandCellElement Then
                e.CellElement.Text = "Btn " & e.CellElement.RowInfo.Cells("Id").Value.ToString()
			ElseIf TypeOf e.CellElement Is GridDateTimeCellElement Then
				Dim dateTimeCell As GridDateTimeCellElement = TryCast(e.CellElement, GridDateTimeCellElement)

				dateTimeCell.Text = String.Format("{0:ddd, MM/dd}", dateTimeCell.Value)
			End If
		End Sub

		#Region "Cell edit"
		Private Sub radGridView1_CellEndEdit(ByVal sender As Object, ByVal e As GridViewCellEventArgs)
			AddTextToListBox(String.Format(" Cell end edit column:{0}, row:{1}", e.ColumnIndex, e.RowIndex))
		End Sub

		Private Sub radGridView1_CellBeginEdit(ByVal sender As Object, ByVal e As GridViewCellCancelEventArgs)
			AddTextToListBox(String.Format(" Cell begin edit column:{0}, row:{1}", e.ColumnIndex, e.RowIndex))
		End Sub
		#End Region

		#Region "Click Events"

		Private Sub radGridView1_Click(ByVal sender As Object, ByVal e As EventArgs)
			AddEventRoot("Click")
		End Sub

		Private Sub radGridView1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
			If radGridView1.MasterView.CurrentRow IsNot Nothing Then
				MessageBox.Show(radGridView1.MasterView.CurrentRow.Cells(1).Value.ToString())
			End If
		End Sub

		Private Sub radGridView1_CellClick(ByVal sender As Object, ByVal e As GridViewCellEventArgs)
			AddEventRoot("CellClick")
			AddTextToListBox(String.Format("    Cell value: {0}", (TryCast(sender, GridCellElement)).Text))
		End Sub

		Private Sub radGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As GridViewCellEventArgs)
			AddEventRoot("CellDoubleClick")
			AddTextToListBox(String.Format("    Cell value: {0}", (TryCast(sender, GridCellElement)).Text))
		End Sub

		Private Sub radGridView1_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)
			AddEventRoot("MouseClick")
			AddTextToListBox(String.Format("    MouseClick {0}, btn: {1}", e.Location, e.Button))
		End Sub

		Private Sub radGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs)
			AddEventRoot("MouseDoubleClick")
			AddTextToListBox(String.Format("    MouseClick {0}, btn: {1}", e.Location, e.Button))
		End Sub

		Private Sub radGridView1_CommandCellClick(ByVal sender As Object, ByVal e As EventArgs)
			Dim clickedCommandColumn As GridCommandCellElement = TryCast(sender, GridCommandCellElement)

			AddEventRoot("CommandCellClick")
			AddTextToListBox(String.Format("    CommandCellClick on row with id = {0}", clickedCommandColumn.RowInfo.Cells("Id").Value))

		End Sub

		#End Region

		#Region "MouseMove Events"

		Private Sub radGridView1_CellMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
			If cell Is Nothing Then
				Return
			End If
			radLabel3.Text = String.Format("CellMouseMove: {0} [{1}, btn: {2}]", cell.Value, e.Location, e.Button)
		End Sub

		#End Region

		#Region "CurrentRowChanging and CurrentRowChanged event handlers"

		Private messageBoxShow As Boolean = True

		Private Sub radGridView1_CurrentRowChanging(ByVal sender As Object, ByVal e As CurrentRowChangingEventArgs)
			If e.NewRow Is Nothing Then
				AddEventRoot("CurrentRowChanging")
				AddTextToListBox("   non-data-bound row")

				Return
			End If

			If e.NewRow.Cells("Id").Value Is Nothing Then
				Return
			End If

			If Not(TypeOf e.NewRow.Cells("Id").Value Is Integer) Then
				Return
			End If

			Dim value As Integer = CInt(Fix(e.NewRow.Cells("Id").Value))
			Dim label As New RadListDataItem()


			If value = 3 Then
				e.Cancel = True
				label.Text = String.Format("   CurrentRowChanging, row (Id = 3) selection cancelled.")
				label.ForeColor = Color.Orange
				If messageBoxShow Then
					MessageBox.Show("The column with Id = 3 selection is cancelled.", "Selection cancellation though CurrentRowChanging event", MessageBoxButtons.OK, MessageBoxIcon.Information)
					messageBoxShow = False
				End If
			Else
				If e.CurrentRow IsNot Nothing Then
					label.Text = String.Format("   CurrentRowChanging, current Id = {0}, new Id = {1}", e.CurrentRow.Cells("Id").Value, value)
				Else
					label.Text = String.Format("   CurrentRowChanging, new Id = {0}", value)
				End If
			End If



			 AddEventRoot("CurrentRowChanging")
			 AddTextToListBox(label)


		End Sub

		Private Sub radGridView1_CurrentRowChanged(ByVal sender As Object, ByVal e As CurrentRowChangedEventArgs)
			Dim text As String

			If e.CurrentRow Is Nothing Then
				If e.OldRow IsNot Nothing Then
					text = String.Format("   CurrentRowChanged, old Id = {0}, current row is non-data-bound", e.OldRow.Cells("Id").Value)
				Else
					text = "   CurrentRowChanged, old and current rows are non-data-bound"
				End If
			Else
				If e.OldRow IsNot Nothing Then
					text = String.Format("   CurrentRowChanged, old Id = {0}, current Id = {1}", e.OldRow.Cells("Id").Value, e.CurrentRow.Cells("Id").Value)
				Else
					text = String.Format("   CurrentRowChanged, current Id = {0}", e.CurrentRow.Cells("Id").Value)
				End If
			End If

			AddEventRoot("CurrentRowChanged")
			AddTextToListBox(text)
		End Sub

		#End Region

		Private Sub radButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
			radListBoxEvents.Items.Clear()
			insertIndex = 0
		End Sub

		#End Region

		#Region "Helper Methods"
		Private insertIndex As Integer = 0
		Private Sub AddEventRoot(ByVal text As String)
			Dim item As New RadListDataItem()
			item.Text = text
			Me.AddEventRoot(item)
		End Sub

		Private Sub AddEventRoot(ByVal item As RadListDataItem)
			item.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(204)))
			radListBoxEvents.Items.Insert(insertIndex, item)
			insertIndex += 1
		End Sub

		Private Sub AddTextToListBox(ByVal item As RadListDataItem)
			If radListBoxEvents.Items.Count > 100 Then
				radListBoxEvents.Items.Clear()
				insertIndex = 0
			End If
			radListBoxEvents.Items.Insert(insertIndex, item)
			insertIndex += 1
		End Sub

		Private Sub AddTextToListBox(ByVal text As String)
			Dim label As New RadListDataItem()
			label.Text = text
			AddTextToListBox(label)
		End Sub

		#End Region

		Protected Overrides Sub WireEvents()
			AddHandler radButton1.Click, AddressOf radButton1_Click
			AddHandler radGridView1.CellMouseMove, AddressOf radGridView1_CellMouseMove
			AddHandler radGridView1.CellDoubleClick, AddressOf radGridView1_CellDoubleClick
			AddHandler radGridView1.MouseDoubleClick, AddressOf radGridView1_MouseDoubleClick
			AddHandler radGridView1.MouseClick, AddressOf radGridView1_MouseClick
			AddHandler radGridView1.Click, AddressOf radGridView1_Click
			AddHandler radGridView1.CellClick, AddressOf radGridView1_CellClick
			AddHandler radGridView1.CurrentRowChanged, AddressOf radGridView1_CurrentRowChanged
			AddHandler radGridView1.CurrentRowChanging, AddressOf radGridView1_CurrentRowChanging
		End Sub
	End Class
End Namespace
