Imports System.Windows.Forms.ListViewItem

Public Class ListViewColumnSorter
	Implements IComparer

	Dim ObjectCompare As CaseInsensitiveComparer

	Public Sub New()
		_columnToSort = 0
		_sortOrder = SortOrder.None
		ObjectCompare = New CaseInsensitiveComparer()
	End Sub

	Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
		If _sortOrder = SortOrder.None Then Return 0

		Try
			Dim listviewX As ListViewItem = CType(x, ListViewItem)
			Dim listviewY As ListViewItem = CType(y, ListViewItem)

			If (_columnToSort > listviewX.SubItems.Count Or _columnToSort > listviewY.SubItems.Count) Then Return 0

			Dim compareResult As Integer
			Dim column1 As ListViewSubItem = listviewX.SubItems(_columnToSort)
			Dim column2 As ListViewSubItem = listviewY.SubItems(_columnToSort)

			If (IsNumeric(column1.Text) And IsNumeric(column2.Text)) Then
				Dim val1 As Double = Convert.ToDouble(column1.Text)
				Dim val2 As Double = Convert.ToDouble(column2.Text)
				compareResult = val1.CompareTo(val2)
			Else
				compareResult = ObjectCompare.Compare(listviewX.SubItems(_columnToSort).Text, listviewY.SubItems(_columnToSort).Text)
			End If

			If (_sortOrder = SortOrder.Ascending) Then Return compareResult
			Return -compareResult
		Catch
			Return 0
		End Try
	End Function

	Private _sortOrder As SortOrder
	Public Property Order() As SortOrder
		Get
			Return _sortOrder
		End Get

		Set(ByVal value As SortOrder)
			_sortOrder = value
		End Set
	End Property

	Private _columnToSort As Integer
	Public Property ColumntToSort() As Integer
		Get
			Return _columnToSort
		End Get

		Set(ByVal value As Integer)
			_columnToSort = value
		End Set
	End Property
End Class
