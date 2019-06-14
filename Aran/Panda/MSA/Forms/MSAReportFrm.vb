Option Strict Off
Option Explicit On

Friend Class CMSAReportFrm
    Inherits System.Windows.Forms.Form

    Private SortF As Integer
	Private nSectors As Integer
	Private _Navaid As NavaidType

    Private SectorSortFlg() As Integer
	Private _Sectors() As MSASectorType
	Private SectorObstacles() As ObstacleType
	Private CurrPageObstacles() As ObstacleType
	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private ReportBtn As System.Windows.Forms.CheckBox

	Private bUpdated As Boolean
	Private Combo1Data() As Integer

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		Me.Text = My.Resources.str10000
		SaveBtn.Text = My.Resources.str00507
		HideBtn.Text = My.Resources.str00508
		SaveDialog1.Title = My.Resources.str11021

		Dim ProjectPath As String
		Dim pos As Integer

		ProjectPath = GetMapFileName()
		pos = InStrRev(ProjectPath, "\")
		SaveDialog1.InitialDirectory = Strings.Left(ProjectPath, pos)

		Label1.Text = My.Resources.str10001

		ListView1.Columns.Item(0).Text = My.Resources.str10002
		ListView1.Columns.Item(1).Text = My.Resources.str10003
		ListView1.Columns.Item(2).Text = My.Resources.str10004 + " (" + DistanceConverter(DistanceUnit).Unit + ")"
		ListView1.Columns.Item(3).Text = My.Resources.str10005
		ListView1.Columns.Item(4).Text = My.Resources.str10006 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(5).Text = My.Resources.str10007 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(6).Text = My.Resources.str10008 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(7).Text = My.Resources.str10009

		ReDim CurrPageObstacles(-1)
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

	Public Sub FillList(ByRef Obstacles() As ObstacleType, ByRef sectors() As MSASectorType, ByRef Navaid As NavaidType)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim nSegNum As Integer

		nSegNum = sectors.Length
		N = Obstacles.Length

		ReDim SectorObstacles(N - 1)
		System.Array.Copy(Obstacles, SectorObstacles, N)

		N = sectors.Length
		ReDim _Sectors(N - 1)
		System.Array.Copy(sectors, _Sectors, N)

		_Navaid = Navaid

		nSectors = nSegNum

		If nSegNum <= 0 Then
			Combo1.Items.Clear()
			ReDim SectorSortFlg(-1)
			Exit Sub
		End If

		If Combo1.Items.Count > 0 Then
			ReDim Preserve SectorSortFlg(nSegNum - 1)
		Else
			ReDim SectorSortFlg(nSegNum - 1)
		End If

		K = Combo1.SelectedIndex
		If K >= nSegNum Then
			K = nSegNum - 1
		ElseIf K < 0 Then
			K = 0
		End If

		Combo1.Items.Clear()
		ReDim Combo1Data(nSegNum - 1)
		For I = 0 To nSegNum - 1
			Combo1.Items.Add("Sector " + CStr(I + 1))
			If SectorSortFlg(I) = 0 Then SectorSortFlg(I) = -5
			Combo1Data(I) = WholeSector + (32 * I)
		Next I

		Combo1.SelectedIndex = K
	End Sub

	Public Sub FillCuttedList(ByRef lSectors() As MSASectorType, ByRef Obstacles() As ObstacleType, ByRef Navaid As NavaidType)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		_Navaid = Navaid

		N = Obstacles.Length
		ReDim SectorObstacles(N - 1)
		System.Array.Copy(Obstacles, SectorObstacles, N)

		N = lSectors.Length
		ReDim _Sectors(N - 1)
		System.Array.Copy(lSectors, _Sectors, N)

		If N <= 0 Then
			Combo1.Items.Clear()
			ReDim SectorSortFlg(-1)
			Exit Sub
		End If

		K = Combo1.SelectedIndex

		If Combo1.Items.Count > 0 Then
			ReDim Preserve SectorSortFlg(2 * N - 1)
		Else
			ReDim SectorSortFlg(2 * N - 1)
		End If

		nSectors = 0
		Combo1.Items.Clear()
		ReDim Combo1Data(2 * N - 1)

		For I = 0 To N - 1
			If _Sectors(I).IsCuted Then
				Combo1.Items.Add("Sector " + CStr(I + 1) + My.Resources.str10013)
				If SectorSortFlg(nSectors) = 0 Then SectorSortFlg(nSectors) = -5
				Combo1Data(nSectors) = InnerSector + (32 * I)
				nSectors += 1

				Combo1.Items.Add("Sector " + CStr(I + 1) + My.Resources.str10014)
				If SectorSortFlg(nSectors) = 0 Then SectorSortFlg(nSectors) = -5
				Combo1Data(nSectors) = OuterSector + (32 * I)
				nSectors += 1
			Else
				Combo1.Items.Add("Sector " + CStr(I + 1))
				If SectorSortFlg(nSectors) = 0 Then SectorSortFlg(nSectors) = -5
				Combo1Data(nSectors) = WholeSector + (32 * I)
				nSectors += 1
			End If
		Next I

		ReDim Preserve SectorSortFlg(nSectors - 1)
		ReDim Preserve Combo1Data(nSectors - 1)

		If K > nSectors - 1 Then
			K = nSectors - 1
		ElseIf K < 0 Then
			K = 0
		End If

		Combo1.SelectedIndex = K
	End Sub

	Private Sub Combo1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Combo1.SelectedIndexChanged
		Dim i As Integer
		Dim j As Integer
		Dim K As Integer
		Dim N As Integer
		Dim SectorType As Integer
		Dim CurrSector As Integer

		j = Combo1.SelectedIndex
		If (j < 0) Or (j >= nSectors) Then Exit Sub

		N = UBound(SectorObstacles)
		If N < 0 Then Exit Sub '?????????????????????

		bUpdated = Me.Visible

		If Not bUpdated Then Exit Sub

		SectorType = Combo1Data(j) And 3
		CurrSector = Combo1Data(j) \ 32

		ReDim CurrPageObstacles(N)
		K = -1

		If SectorType = WholeSector Then
			For i = 0 To N
				If SectorObstacles(i).iSector(CurrSector) > 0 Then
					K = K + 1
					CurrPageObstacles(K) = SectorObstacles(i)
				End If
			Next i
		ElseIf SectorType = InnerSector Then
			For i = 0 To N
				If (SectorObstacles(i).iSector(CurrSector) And InnerSector) > 0 Then
					K = K + 1
					CurrPageObstacles(K) = SectorObstacles(i)
				End If
			Next i
		Else
			For i = 0 To N
				If (SectorObstacles(i).iSector(CurrSector) And OuterSector) > 0 Then
					K = K + 1
					CurrPageObstacles(K) = SectorObstacles(i)
				End If
			Next i
		End If

		If K >= 0 Then
			ReDim Preserve CurrPageObstacles(K)
		Else
			ReDim CurrPageObstacles(-1)
			ListView1.Items.Clear()

			On Error Resume Next
			If Not pPointElem Is Nothing Then GetActiveView().GraphicsContainer.DeleteElement(pPointElem)
			On Error GoTo 0
		End If

		Label2.Text = My.Resources.str10012 + " " + CStr(K + 1)

		'Dim InPrima As Boolean
		'Dim itmX As System.Windows.Forms.ListViewItem
		'Dim CutDist As Double

		''CurrSector = Combo1Data(j) \ 32
		'CutDist = Sectors(CurrSector).InnerDist
		''ListView1.Items.Clear()

		'For i = K To ListView1.Items.Count - 2
		'	ListView1.Items.RemoveAt(K)
		'Next

		'For i = 0 To K
		'	If i = ListView1.Items.Count Then
		'		itmX = ListView1.Items.Add(CurrPageObstacles(i).Type)
		'	Else
		'		itmX = ListView1.Items(i)
		'	End If

		'	itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CurrPageObstacles(i).Type))
		'	itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(CurrPageObstacles(i).Dist, eRoundMode.NERAEST))))
		'	itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(Modulus(Dir2Azt(CurrPageObstacles(i).pPtPrj, CurrPageObstacles(i).Angle) - MagVar), 1))))
		'	itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CurrPageObstacles(i).Height, eRoundMode.CEIL))))
		'	itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CurrPageObstacles(i).ReqH, eRoundMode.CEIL))))
		'	itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(CurrPageObstacles(i).MSA)))

		'	InPrima = (CurrPageObstacles(i).iSector(CurrSector) And PrimaryArea) > 0
		'	If InPrima Then
		'		If (SectorType = InnerSector) And (CurrPageObstacles(i).Dist > CutDist) Then
		'			InPrima = False
		'		End If
		'		If (SectorType = OuterSector) And (CurrPageObstacles(i).Dist < CutDist) Then
		'			InPrima = False
		'		End If
		'	End If

		'	If InPrima Then
		'		itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, My.Resources.str10010))
		'	Else
		'		itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, My.Resources.str10011))
		'	End If

		'	If Sectors(CurrSector).DominicObstacle.Identifier = CurrPageObstacles(i).Identifier Then
		'		itmX.Font = New Font(itmX.Font, FontStyle.Bold)
		'		itmX.ForeColor = Color.Red
		'		For j = 1 To 7
		'			itmX.SubItems.Item(j).Font = itmX.Font
		'			itmX.SubItems.Item(j).ForeColor = itmX.ForeColor
		'		Next j
		'	Else
		'		itmX.Font = New Font(itmX.Font, FontStyle.Regular)
		'		itmX.ForeColor = Color.Black
		'	End If
		'Next i
		'''MSAReportFrm_Load(Me, Nothing)

		i = System.Math.Abs(SectorSortFlg(j)) - 1
		If SectorSortFlg(j) <> 0 Then ListView1.Columns.Item(i).ImageIndex = 0

		SectorSortFlg(j) = -SectorSortFlg(j)
		ListView1_ColumnClick(ListView1, New System.Windows.Forms.ColumnClickEventArgs(i))
	End Sub

	Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(CurrPageObstacles) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = Item.Index

		'pPointElem = DrawPointWithText(CurrPageObstacles(Index).pGeomPrj, CurrPageObstacles(Index).Name, RGB(255, 0, 255))
		'pPointElem = DrawPointWithText(CurrPageObstacles(Index).pPtPrj, CurrPageObstacles(Index).Name, RGB(255, 0, 255))
		pPointElem = DrawObstacle(CurrPageObstacles(Index), RGB(255, 0, 255))
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView1_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
		Dim i As Integer
		Dim j As Integer
		Dim N As Integer
		Dim SectorType As Integer
		Dim CurrSector As Integer
		Dim CutDist As Double

		Dim InPrima As Boolean
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView1.Columns(eventArgs.Column)

		j = Combo1.SelectedIndex
		If (j < 0) Or (j >= nSectors) Then Return

		N = UBound(CurrPageObstacles)

		If N < 0 Then Exit Sub

		'ListView1.Sorted = False

		SectorType = Combo1Data(j) And 3
		CurrSector = Combo1Data(j) \ 32
		CutDist = _Sectors(CurrSector).InnerDist

		If System.Math.Abs(SectorSortFlg(j)) - 1 = ColumnHeader.Index Then
			SectorSortFlg(j) = -SectorSortFlg(j)
		Else
			If SectorSortFlg(j) <> 0 Then ListView1.Columns.Item(System.Math.Abs(SectorSortFlg(j)) - 1).ImageIndex = -1
			SectorSortFlg(j) = -ColumnHeader.Index - 1
		End If

		If SectorSortFlg(j) > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		'Название| ID |Удаление | Радиал °| Абс. высота | Мин. высота пролёта | Округ. мин. высота пролёта | Зона |

		If (ColumnHeader.Index < 7) And (ColumnHeader.Index > 1) Then
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						CurrPageObstacles(i).fSort = CurrPageObstacles(i).MinDist
					Case 3
						'CurrPageObstacles(i).fSort = System.Math.Round(Modulus(Dir2Azt(CurrPageObstacles(i).pPtPrj, CurrPageObstacles(i).Angle) - MagVar), 1)
						CurrPageObstacles(i).fSort = System.Math.Round(Modulus(Dir2Azt(_Navaid.pPtPrj, CurrPageObstacles(i).FromPoint.a) - _Navaid.MagVar), 1)
					Case 4
						CurrPageObstacles(i).fSort = CurrPageObstacles(i).Height
					Case 5
						CurrPageObstacles(i).fSort = CurrPageObstacles(i).ReqH
					Case 6
						CurrPageObstacles(i).fSort = CurrPageObstacles(i).MSA
				End Select
			Next i

			If SectorSortFlg(j) > 0 Then
				shall_SortfSort(CurrPageObstacles)
			Else
				shall_SortfSortD(CurrPageObstacles)
			End If
		Else
			For i = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						CurrPageObstacles(i).sSort = CurrPageObstacles(i).TypeName
					Case 1
						CurrPageObstacles(i).sSort = CurrPageObstacles(i).UnicalName
					Case 7
						InPrima = (CurrPageObstacles(i).iSector(CurrSector) And PrimaryArea) > 0
						If InPrima Then
							If (SectorType = InnerSector) And (CurrPageObstacles(i).MinDist > CutDist) Then
								InPrima = False
							End If

							If (SectorType = OuterSector) And (CurrPageObstacles(i).MinDist < CutDist) Then
								InPrima = False
							End If
						End If

						If InPrima Then
							CurrPageObstacles(i).sSort = My.Resources.str10010
						Else
							CurrPageObstacles(i).sSort = My.Resources.str10011
						End If
				End Select
			Next i

			If SectorSortFlg(j) > 0 Then
				shall_SortsSort(CurrPageObstacles)
			Else
				shall_SortsSortD(CurrPageObstacles)
			End If
		End If

		For i = N To ListView1.Items.Count - 2
			ListView1.Items.RemoveAt(N)
		Next

		For i = 0 To N
			InPrima = (CurrPageObstacles(i).iSector(CurrSector) And PrimaryArea) > 0

			If InPrima Then
				If (SectorType = InnerSector) And (CurrPageObstacles(i).MinDist > CutDist) Then
					InPrima = False
				End If

				If (SectorType = OuterSector) And (CurrPageObstacles(i).MinDist < CutDist) Then
					InPrima = False
				End If
			End If

			If i = ListView1.Items.Count Then
				itmX = ListView1.Items.Add(CurrPageObstacles(i).TypeName)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CurrPageObstacles(i).UnicalName))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(CurrPageObstacles(i).MinDist, eRoundMode.NERAEST))))
				itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(Modulus(Dir2Azt(_Navaid.pPtPrj, CurrPageObstacles(i).FromPoint.a) - _Navaid.MagVar), 1))))
				itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CurrPageObstacles(i).Height, eRoundMode.CEIL))))
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CurrPageObstacles(i).ReqH, eRoundMode.CEIL))))
				itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(CurrPageObstacles(i).MSA)))

				If InPrima Then
					itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, My.Resources.str10010))
				Else
					itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, My.Resources.str10011))
				End If
			Else
				itmX = ListView1.Items(i)

				itmX.Text = CurrPageObstacles(i).TypeName
				itmX.SubItems(1).Text = CurrPageObstacles(i).UnicalName
				itmX.SubItems(2).Text = CStr(ConvertDistance(CurrPageObstacles(i).MinDist, eRoundMode.NERAEST))
				itmX.SubItems(3).Text = CStr(System.Math.Round(Modulus(Dir2Azt(_Navaid.pPtPrj, CurrPageObstacles(i).FromPoint.a) - _Navaid.MagVar), 1))
				itmX.SubItems(4).Text = CStr(ConvertHeight(CurrPageObstacles(i).Height, eRoundMode.CEIL))
				itmX.SubItems(5).Text = CStr(ConvertHeight(CurrPageObstacles(i).ReqH, eRoundMode.CEIL))
				itmX.SubItems(6).Text = CStr(CurrPageObstacles(i).MSA)

				If InPrima Then
					itmX.SubItems(7).Text = My.Resources.str10010
				Else
					itmX.SubItems(7).Text = My.Resources.str10011
				End If
			End If

			If _Sectors(CurrSector).DominicObstacle.Identifier = CurrPageObstacles(i).Identifier Then
				If itmX.ForeColor <> Color.Red Then
					itmX.Font = New Font(itmX.Font, FontStyle.Bold)
					itmX.ForeColor = Color.Red
					For j = 1 To 7
						itmX.SubItems.Item(j).Font = itmX.Font
						itmX.SubItems.Item(j).ForeColor = itmX.ForeColor
					Next j
				End If
			ElseIf _Sectors(CurrSector).InnerDominicObstacle.Identifier = CurrPageObstacles(i).Identifier Then
				If itmX.ForeColor <> Color.Blue Then
					itmX.Font = New Font(itmX.Font, FontStyle.Bold)
					itmX.ForeColor = Color.Blue
					For j = 1 To 7
						itmX.SubItems.Item(j).Font = itmX.Font
						itmX.SubItems.Item(j).ForeColor = itmX.ForeColor
					Next j
				End If
			Else
				If itmX.ForeColor <> Color.Black Then
					itmX.Font = New Font(itmX.Font, FontStyle.Regular)
					itmX.ForeColor = Color.Black

					For j = 1 To 7
						itmX.SubItems.Item(j).Font = itmX.Font
						itmX.SubItems.Item(j).ForeColor = itmX.ForeColor
					Next j
				End If
			End If

		Next i

		If Me.Visible Then
			itmX = ListView1.FocusedItem
			'ListView1.SelectedItems.Add(itmX)
			ListView1_SelectedIndexChanged(ListView1, New System.EventArgs())
		End If
	End Sub

	Private Sub SaveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim InPrima As Boolean

		Dim i As Integer
		Dim j As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer
		Dim M As Integer
		Dim pos As Integer
		Dim pos2 As Integer
		Dim SortIndex As Integer
		Dim SectorType As Integer

		Dim CutDist As Double

		Dim BoltStr As String
		Dim FileName As String
		Dim FileTitle As String

		Dim MSARep As ReportFile
		Dim CurrObstacles() As ObstacleType
		Dim DominicObstacle As ObstacleType
		Dim createMSA As CCreateMSA = Owner

		SaveDialog1.FileName = createMSA.ComboBox001.Text + "_" + createMSA.Label003.Text + "_" + createMSA.TextBox001.Text + ".htm"

		If SaveDialog1.ShowDialog() <> DialogResult.OK Then Return

		FileName = SaveDialog1.FileName

		pos = InStrRev(FileName, ".")

		If (pos > 0) Then FileName = Strings.Left(FileName, pos - 1)

		FileTitle = FileName
		pos2 = InStrRev(FileTitle, "\")
		If (pos2 > 0) Then FileTitle = Strings.Right(FileTitle, FileTitle.Length - pos2)

		MSARep = New ReportFile

		MSARep.OpenFile(FileName, "MSA Report")

		MSARep.WriteMessage(My.Resources.str11022)
		MSARep.WriteMessage()
		MSARep.WriteMessage(FileTitle)
		MSARep.WriteParam(My.Resources.str11023, CStr(Today) + " - " + CStr(TimeOfDay))
		MSARep.WriteMessage()

		MSARep.WriteParam(createMSA.Label002.Text, createMSA.ComboBox001.Text)
		MSARep.WriteParam(createMSA.Label004.Text, createMSA.TextBox001.Text, createMSA.Label005.Text)
		MSARep.WriteParam(createMSA.Label006.Text, createMSA.TextBox002.Text, createMSA.Label007.Text)
		MSARep.WriteMessage()

		N = UBound(_Sectors)
		M = UBound(SectorObstacles)

		L = 0
		For j = 0 To N
			i = _Sectors(j).FromAngle
			K = _Sectors(j).ToAngle
			If K = 0 Then K = 360
			If _Sectors(j).IsCuted Then
				MSARep.WriteMessage()
				MSARep.WriteText("<b>" + My.Resources.str11025 & CStr(j + 1) & "</b> " & My.Resources.str10015 & ConvertDistance(_Sectors(j).InnerDist) & " " + DistanceConverter(DistanceUnit).Unit + "<br>")
				MSARep.WriteText("<b>" + My.Resources.str11025 & CStr(j + 1) & My.Resources.str10013 + ":</b> " + My.Resources.str11026 & CStr(i) & My.Resources.str11027 & CStr(K) + "°, <b>MSA:</b> " + CStr(_Sectors(j).InnerDominicObstacle.MSA) + " " + HeightConverter(HeightUnit).Unit + "<br>")
				MSARep.WriteText("<b>" + My.Resources.str11025 & CStr(j + 1) & My.Resources.str10014 + ":</b> " + My.Resources.str11026 & CStr(i) & My.Resources.str11027 & CStr(K) + "°, <b>MSA:</b> " + CStr(_Sectors(j).DominicObstacle.MSA) + " " + HeightConverter(HeightUnit).Unit + "<br>")
				MSARep.WriteMessage()
				L = L + 2
			Else
				MSARep.WriteText("<b>" + My.Resources.str11025 & CStr(j + 1) + ":</b> " + My.Resources.str11026 & CStr(i) & My.Resources.str11027 & CStr(K) + "°, <b>MSA:</b> " + CStr(_Sectors(j).DominicObstacle.MSA) + " " + HeightConverter(HeightUnit).Unit + "<br>")
				L = L + 1
			End If
		Next j

		MSARep.WriteMessage()
		MSARep.WriteParam(My.Resources.str11024, CStr(L))

		MSARep.WriteMessage()
		MSARep.WriteMessage()

		j = 0
		L = 0

		While j <= N
			If _Sectors(j).IsCuted Then
				If L = 0 Then
					MSARep.WriteMessage(My.Resources.str11025 & CStr(j + 1) & My.Resources.str10013)
					DominicObstacle = _Sectors(j).InnerDominicObstacle
					SectorType = InnerSector
				Else
					MSARep.WriteMessage(My.Resources.str11025 & CStr(j + 1) & My.Resources.str10014)
					DominicObstacle = _Sectors(j).DominicObstacle
					SectorType = OuterSector
				End If
			Else
				SectorType = WholeSector
				MSARep.WriteMessage(My.Resources.str11025 & CStr(j + 1))
				DominicObstacle = _Sectors(j).DominicObstacle
			End If

			MSARep.WriteMessage()
			MSARep.WriteParam("MSA", CStr(DominicObstacle.MSA), HeightConverter(HeightUnit).Unit)

			i = _Sectors(j).FromAngle
			MSARep.WriteParam(My.Resources.str11028, CStr(i), "°")

			i = _Sectors(j).ToAngle
			If i = 0 Then i = 360
			MSARep.WriteParam(My.Resources.str11029, CStr(i), "°")

			MSARep.WriteParam(My.Resources.str11030, DominicObstacle.UnicalName)
			MSARep.WriteParam(My.Resources.str11031, CStr(ConvertHeight(DominicObstacle.ReqH, eRoundMode.CEIL)), HeightConverter(HeightUnit).Unit)

			MSARep.WriteMessage()
			If SectorType = InnerSector Then
				MSARep.WriteMessage(My.Resources.str11032 & CStr(j + 1) & My.Resources.str10013 + " :")
			ElseIf SectorType = OuterSector Then
				MSARep.WriteMessage(My.Resources.str11032 & CStr(j + 1) & My.Resources.str10014 + " :")
			Else
				MSARep.WriteMessage(My.Resources.str11032 & CStr(j + 1) + " :")
			End If

			MSARep.WriteMessage()

			'Название| ID |Удаление | Радиал °| Абс. высота | Мин. высота пролёта | Округ. мин. высота пролёта | Зона |
			MSARep.WriteText("<table border='1' cellspacing='0' cellpadding='1'>")
			MSARep.WriteText("<tr>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10002 + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10003 + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10004 + " (" + DistanceConverter(DistanceUnit).Unit + ")" + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10005 + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10006 + " (" + HeightConverter(HeightUnit).Unit + ")" + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10007 + " (" + HeightConverter(HeightUnit).Unit + ")" + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10008 + " (" + HeightConverter(HeightUnit).Unit + ")" + "</b></td>")
			MSARep.WriteText("<td width=150><b>" + My.Resources.str10009 + "</b></td>")
			MSARep.WriteText("</tr>")

			SortIndex = System.Math.Abs(SectorSortFlg(j))
			CutDist = _Sectors(j).InnerDist

			ReDim CurrObstacles(M)
			K = -1
			For i = 0 To M
				If SectorObstacles(i).iSector(j) > 0 Then
					If (SectorType = WholeSector) Or (SectorObstacles(i).iSector(j) And SectorType) = SectorType Then
					Else
						GoTo NextI
					End If

					K = K + 1
					CurrObstacles(K) = SectorObstacles(i)

					If (SortIndex < 8) And (SortIndex > 2) Then
						Select Case SortIndex
							Case 3
								CurrObstacles(K).fSort = CurrObstacles(K).MinDist
							Case 4
								CurrObstacles(K).fSort = Dir2Azt(_Navaid.pPtPrj, CurrObstacles(K).FromPoint.a + 180.0)
							Case 5
								CurrObstacles(K).fSort = CurrObstacles(K).Height
							Case 6
								CurrObstacles(K).fSort = CurrObstacles(K).ReqH
							Case 7
								CurrObstacles(K).fSort = CurrObstacles(K).MSA
						End Select
					Else
						Select Case SortIndex
							Case 1
								CurrObstacles(K).sSort = CurrObstacles(K).TypeName
							Case 2
								CurrObstacles(K).sSort = CurrObstacles(K).UnicalName
							Case 8
								InPrima = (CurrObstacles(K).iSector(j) And PrimaryArea) > 0
								If InPrima Then
									If (SectorType = InnerSector) And (CurrObstacles(K).MinDist > CutDist) Then
										InPrima = False
									End If
									If (SectorType = OuterSector) And (CurrObstacles(K).MinDist < CutDist) Then
										InPrima = False
									End If
								End If

								If InPrima Then
									CurrObstacles(K).sSort = My.Resources.str10010
								Else
									CurrObstacles(K).sSort = My.Resources.str10011
								End If
						End Select
					End If
				End If
NextI:
			Next i

			If K >= 0 Then
				ReDim Preserve CurrObstacles(K)
				If (SortIndex < 8) And (SortIndex > 2) Then
					If SectorSortFlg(j) > 0 Then
						shall_SortfSort(CurrObstacles)
					Else
						shall_SortfSortD(CurrObstacles)
					End If
				Else
					If SectorSortFlg(j) > 0 Then
						shall_SortsSort(CurrObstacles)
					Else
						shall_SortsSortD(CurrObstacles)
					End If
				End If
			Else
				ReDim CurrObstacles(-1)
			End If

			For i = 0 To K
				If (CurrObstacles(i).Identifier = DominicObstacle.Identifier) Then
					BoltStr = "<b>"
				Else
					BoltStr = ""
				End If

				MSARep.WriteText("<tr>")
				MSARep.WriteText("<td>" + BoltStr & CurrObstacles(i).TypeName & BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CurrObstacles(i).UnicalName & BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CStr(ConvertDistance(CurrObstacles(i).MinDist, eRoundMode.NERAEST)) & BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CStr(System.Math.Round(Dir2Azt(_Navaid.pPtPrj, CurrObstacles(i).FromPoint.a + 180.0), 1)) & BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CStr(ConvertHeight(CurrObstacles(i).Height, eRoundMode.CEIL)) & BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CStr(ConvertHeight(CurrObstacles(i).ReqH, eRoundMode.CEIL)) + BoltStr + "</td>")
				MSARep.WriteText("<td>" + BoltStr & CStr(CurrObstacles(i).MSA) + BoltStr + "</td>")

				InPrima = (CurrObstacles(i).iSector(j) And PrimaryArea) > 0
				If InPrima Then
					If (SectorType = InnerSector) And (CurrObstacles(i).MinDist > CutDist) Then
						InPrima = False
					End If
					If (SectorType = OuterSector) And (CurrObstacles(i).MinDist < CutDist) Then
						InPrima = False
					End If
				End If

				If InPrima Then
					MSARep.WriteText("<td>" + BoltStr & My.Resources.str10010 & BoltStr + "</td>")
				Else
					MSARep.WriteText("<td>" + BoltStr & My.Resources.str10011 & BoltStr + "</td>")
				End If

				MSARep.WriteText("</tr>")
			Next i

			MSARep.WriteText("</table>")
			MSARep.WriteMessage()

			MSARep.WriteMessage()
			MSARep.WriteMessage()

			If SectorType = InnerSector Then
				L = 1
			Else
				L = 0
				j = j + 1
			End If
		End While

		MSARep.CloseFile()
		MSARep = Nothing
	End Sub

	Private Sub HideBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles HideBtn.Click
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportBtn.CheckState = False
		Hide()
	End Sub

	Private Sub MSAReportFrm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
		Dim SelectedItem As System.Windows.Forms.ListViewItem
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		Dim i As Integer

		If Not Me.Visible Then Exit Sub

		If Not bUpdated Then
			Combo1_SelectedIndexChanged(Combo1, New System.EventArgs())
		End If

		SelectedItem = ListView1.FocusedItem
		If SelectedItem Is Nothing Then Exit Sub
		i = SelectedItem.Index - 1

		If (UBound(CurrPageObstacles) < 0) Or (i < 0) Then Exit Sub

		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		pPointElem = DrawObstacle(CurrPageObstacles(i), RGB(255, 0, 255)) 'DrawPointWithText(CurrPageObstacles(i).pGeomPrj, CurrPageObstacles(i).Name, RGB(255, 0, 255))
		pPointElem.Locked = True
	End Sub

	Private Sub MSAReportFrm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			HideBtn_Click(HideBtn, New System.EventArgs())
		End If
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub MSAReportFrm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
        pGraphics = GetActiveView().GraphicsContainer
        On Error Resume Next
        If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
        On Error GoTo 0
    End Sub

    Sub SetBtn(ByRef Btn As System.Windows.Forms.CheckBox)
        ReportBtn = Btn
    End Sub
End Class
