Public Class TAAReport
	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private pGeomElem As ESRI.ArcGIS.Carto.IElement

	Private LObstaclesPage01() As ObstacleType
	Private fMOC01 As Double
	Private SortF01 As Integer

	Private LObstaclesPage02() As ObstacleType
	Private fMOC02 As Double
	Private SortF02 As Integer

	Private LObstaclesPage03() As ObstacleType
	Private fMOC03 As Double
	Private SortF03 As Integer

	Private ReportBtn As System.Windows.Forms.CheckBox

#Region "Form"
	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.

		SortF01 = 0
		SortF02 = 0
		SortF03 = 0

		ListView01.Columns.Item(0).Text = "Type"    'type
		ListView01.Columns.Item(1).Text = "Name"    'name
		ListView01.Columns.Item(2).Text = "Elevation " + " (" + HeightConverter(HeightUnit).Unit + ")"              'Elevation
		ListView01.Columns.Item(3).Text = "MOC " + " (" + HeightConverter(HeightUnit).Unit + ")"                    'MOC
		ListView01.Columns.Item(4).Text = "Required altitude " + " (" + HeightConverter(HeightUnit).Unit + ")"      'Required altitude

		ListView02.Columns.Item(0).Text = "Type"    'type
		ListView02.Columns.Item(1).Text = "Name"    'name
		ListView02.Columns.Item(2).Text = "Elevation " + " (" + HeightConverter(HeightUnit).Unit + ")"              'Elevation
		ListView02.Columns.Item(3).Text = "MOC " + " (" + HeightConverter(HeightUnit).Unit + ")"                    'MOC
		ListView02.Columns.Item(4).Text = "Required altitude " + " (" + HeightConverter(HeightUnit).Unit + ")"      'Required altitude

		ListView03.Columns.Item(0).Text = "Type"    'type
		ListView03.Columns.Item(1).Text = "Name"    'name
		ListView03.Columns.Item(2).Text = "Elevation " + " (" + HeightConverter(HeightUnit).Unit + ")"              'Elevation
		ListView03.Columns.Item(3).Text = "MOC " + " (" + HeightConverter(HeightUnit).Unit + ")"                    'MOC
		ListView03.Columns.Item(4).Text = "Required altitude " + " (" + HeightConverter(HeightUnit).Unit + ")"      'Required altitude
	End Sub

	'Private Sub TAAReport_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
	'End Sub

	Private Sub TAAReport_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			HideBtn_Click(HideBtn, New System.EventArgs())
		End If
		e.Cancel = Cancel
	End Sub

	Private Sub TAAReport_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

#End Region

	Public Sub FillLeftPage(ByRef Obstacles() As ObstacleType, ByVal fMOC As Double)
		Dim I As Integer
		Dim N As Integer

		Dim itmX As System.Windows.Forms.ListViewItem
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		Dim pColumnHeader As System.Windows.Forms.ColumnHeader

		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		N = UBound(Obstacles)
		ReDim LObstaclesPage01(N)
		ListView01.Items.Clear()

		fMOC01 = fMOC
		If N < 0 Then Exit Sub

		For I = 0 To N
			Obstacles(I).ReqH = Obstacles(I).Height + fMOC01
			LObstaclesPage01(I) = Obstacles(I)

			itmX = ListView01.Items.Add(LObstaclesPage01(I).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles(I).UnicalName))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).Height, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(fMOC01, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.SPECIAL_CEIL))))
		Next I

		If SortF01 <> 0 Then
			pColumnHeader = ListView01.Columns.Item(System.Math.Abs(SortF01) - 1)
			SortF01 = -SortF01
			ListView01_ColumnClick(ListView01, New System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index))
		End If

		If ReportBtn.Checked And (Not Me.Visible) Then Show(s_Win32Window)
	End Sub

	Public Sub FillCentralPage(ByRef Obstacles() As ObstacleType, ByVal fMOC As Double)
		Dim I As Integer
		Dim N As Integer

		Dim itmX As System.Windows.Forms.ListViewItem
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		Dim pColumnHeader As System.Windows.Forms.ColumnHeader

		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		N = UBound(Obstacles)
		ReDim LObstaclesPage02(N)
		ListView02.Items.Clear()

		fMOC02 = fMOC
		If N < 0 Then Exit Sub

		For I = 0 To N
			Obstacles(I).ReqH = Obstacles(I).Height + fMOC02
			LObstaclesPage02(I) = Obstacles(I)

			itmX = ListView02.Items.Add(LObstaclesPage02(I).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles(I).UnicalName))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).Height, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(fMOC02, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.SPECIAL_CEIL))))
		Next I

		If SortF02 <> 0 Then
			pColumnHeader = ListView02.Columns.Item(System.Math.Abs(SortF02) - 1)
			SortF02 = -SortF02
			ListView02_ColumnClick(ListView02, New System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index))
		End If

		If ReportBtn.Checked And (Not Me.Visible) Then Show(s_Win32Window)
	End Sub

	Public Sub FillRightPage(ByRef Obstacles() As ObstacleType, ByVal fMOC As Double)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		Dim itmX As System.Windows.Forms.ListViewItem
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		Dim pColumnHeader As System.Windows.Forms.ColumnHeader

		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		N = UBound(Obstacles)
		ReDim LObstaclesPage03(N)
		ListView03.Items.Clear()

		fMOC03 = fMOC
		If N < 0 Then Exit Sub

		For I = 0 To N
			Obstacles(I).ReqH = Obstacles(I).Height + fMOC03
			LObstaclesPage03(I) = Obstacles(I)

			itmX = ListView03.Items.Add(LObstaclesPage03(I).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles(I).UnicalName))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).Height, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(fMOC03, eRoundMode.NERAEST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.SPECIAL_CEIL))))
		Next I

		If SortF03 <> 0 Then
			pColumnHeader = ListView03.Columns.Item(System.Math.Abs(SortF03) - 1)
			SortF03 = -SortF03
			ListView03_ColumnClick(ListView03, New System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index))
		End If

		If ReportBtn.Checked And (Not Me.Visible) Then Show(s_Win32Window)
	End Sub


	Private Sub ListView01_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView01.ColumnClick
		Dim i As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView01.Columns(e.Column)

		N = UBound(LObstaclesPage01)

		If N < 0 Then Exit Sub

		'ListView01.Sorted = False

		If System.Math.Abs(SortF01) - 1 = ColumnHeader.Index Then
			SortF01 = -SortF01
		Else
			If SortF01 <> 0 Then ListView01.Columns.Item(System.Math.Abs(SortF01) - 1).ImageIndex = -1
			SortF01 = -ColumnHeader.Index - 1
		End If

		If SortF01 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		'Название| ID | Абс. высота |

		If (ColumnHeader.Index > 1) Then
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage01(i).fSort = LObstaclesPage01(i).Height
					Case 3
						LObstaclesPage01(i).fSort = fMOC01
					Case 4
						LObstaclesPage01(i).fSort = LObstaclesPage01(i).ReqH
				End Select
			Next i

			If SortF01 > 0 Then
				shall_SortfSort(LObstaclesPage01)
			Else
				shall_SortfSortD(LObstaclesPage01)
			End If
		Else
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						LObstaclesPage01(i).sSort = LObstaclesPage01(i).TypeName
					Case 1
						LObstaclesPage01(i).sSort = LObstaclesPage01(i).UnicalName
				End Select
			Next i

			If SortF01 > 0 Then
				shall_SortsSort(LObstaclesPage01)
			Else
				shall_SortsSortD(LObstaclesPage01)
			End If
		End If

		'For i = N To ListView01.Items.Count - 2
		'	ListView01.Items.RemoveAt(N)
		'Next

		For i = 0 To N
			itmX = ListView01.Items(i)
			itmX.Text = LObstaclesPage01(i).TypeName
			itmX.SubItems(1).Text = LObstaclesPage01(i).UnicalName
			itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage01(i).Height, eRoundMode.NERAEST))
			itmX.SubItems(3).Text = CStr(ConvertHeight(fMOC01, eRoundMode.NERAEST))
			itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage01(i).ReqH, eRoundMode.SPECIAL_CEIL))
		Next i

		If Me.Visible Then
			itmX = ListView01.FocusedItem
			ListView01_SelectedIndexChanged(ListView01, New System.EventArgs())
		End If
	End Sub

	Private Sub ListView02_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView02.ColumnClick
		Dim i As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView02.Columns(e.Column)

		N = UBound(LObstaclesPage02)

		If N < 0 Then Exit Sub

		'ListView02.Sorted = False

		If System.Math.Abs(SortF02) - 1 = ColumnHeader.Index Then
			SortF02 = -SortF02
		Else
			If SortF02 <> 0 Then ListView02.Columns.Item(System.Math.Abs(SortF02) - 1).ImageIndex = -1
			SortF02 = -ColumnHeader.Index - 1
		End If

		If SortF02 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		'Название| ID | Абс. высота |

		If (ColumnHeader.Index > 1) Then
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage02(i).fSort = LObstaclesPage02(i).Height
					Case 3
						LObstaclesPage02(i).fSort = fMOC02
					Case 4
						LObstaclesPage02(i).fSort = LObstaclesPage02(i).ReqH
				End Select
			Next i

			If SortF02 > 0 Then
				shall_SortfSort(LObstaclesPage02)
			Else
				shall_SortfSortD(LObstaclesPage02)
			End If
		Else
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						LObstaclesPage02(i).sSort = LObstaclesPage02(i).TypeName
					Case 1
						LObstaclesPage02(i).sSort = LObstaclesPage02(i).UnicalName
				End Select
			Next i

			If SortF02 > 0 Then
				shall_SortsSort(LObstaclesPage02)
			Else
				shall_SortsSortD(LObstaclesPage02)
			End If
		End If

		'For i = N To ListView02.Items.Count - 2
		'	ListView02.Items.RemoveAt(N)
		'Next

		For i = 0 To N
			itmX = ListView02.Items(i)
			itmX.Text = LObstaclesPage02(i).TypeName
			itmX.SubItems(1).Text = LObstaclesPage02(i).UnicalName
			itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage02(i).Height, eRoundMode.NERAEST))
			itmX.SubItems(3).Text = CStr(ConvertHeight(fMOC02, eRoundMode.NERAEST))
			itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage02(i).ReqH, eRoundMode.SPECIAL_CEIL))
		Next i

		If Me.Visible Then
			itmX = ListView02.FocusedItem
			ListView02_SelectedIndexChanged(ListView02, New System.EventArgs())
		End If
	End Sub

	Private Sub ListView03_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView03.ColumnClick
		Dim i As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView03.Columns(e.Column)

		N = UBound(LObstaclesPage03)

		If N < 0 Then Exit Sub

		'ListView03.Sorted = False

		If System.Math.Abs(SortF03) - 1 = ColumnHeader.Index Then
			SortF03 = -SortF03
		Else
			If SortF03 <> 0 Then ListView03.Columns.Item(System.Math.Abs(SortF03) - 1).ImageIndex = -1
			SortF03 = -ColumnHeader.Index - 1
		End If

		If SortF03 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		'Название| ID | Абс. высота |

		If (ColumnHeader.Index > 1) Then
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage03(i).fSort = LObstaclesPage03(i).Height
					Case 3
						LObstaclesPage03(i).fSort = fMOC03
					Case 4
						LObstaclesPage03(i).fSort = LObstaclesPage03(i).ReqH
				End Select
			Next i

			If SortF03 > 0 Then
				shall_SortfSort(LObstaclesPage03)
			Else
				shall_SortfSortD(LObstaclesPage03)
			End If
		Else
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						LObstaclesPage03(i).sSort = LObstaclesPage03(i).TypeName
					Case 1
						LObstaclesPage03(i).sSort = LObstaclesPage03(i).UnicalName
				End Select
			Next i

			If SortF03 > 0 Then
				shall_SortsSort(LObstaclesPage03)
			Else
				shall_SortsSortD(LObstaclesPage03)
			End If
		End If

		'For i = N To ListView03.Items.Count - 2
		'	ListView03.Items.RemoveAt(N)
		'Next

		For i = 0 To N
			itmX = ListView03.Items(i)
			itmX.Text = LObstaclesPage03(i).TypeName
			itmX.SubItems(1).Text = LObstaclesPage03(i).UnicalName
			itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage03(i).Height, eRoundMode.NERAEST))
			itmX.SubItems(3).Text = CStr(ConvertHeight(fMOC03, eRoundMode.NERAEST))
			itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage03(i).ReqH, eRoundMode.SPECIAL_CEIL))
		Next i

		If Me.Visible Then
			itmX = ListView03.FocusedItem
			ListView03_SelectedIndexChanged(ListView03, New System.EventArgs())
		End If
	End Sub

	Private Sub ListView01_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView01.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage01) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = Item.Index

		pPointElem = DrawObstacle(LObstaclesPage01(Index), RGB(255, 0, 255))
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView02_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView02.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage02) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = Item.Index

		pPointElem = DrawObstacle(LObstaclesPage02(Index), RGB(255, 0, 255))
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView03_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView03.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage03) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = Item.Index

		pPointElem = DrawObstacle(LObstaclesPage03(Index), RGB(255, 0, 255))
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub HideBtn_Click(sender As System.Object, e As System.EventArgs) Handles HideBtn.Click
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportBtn.CheckState = False
		Hide()
	End Sub

	Sub SetBtn(ByRef Btn As System.Windows.Forms.CheckBox)
		ReportBtn = Btn
	End Sub

End Class