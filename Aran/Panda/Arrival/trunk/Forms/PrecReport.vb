Option Strict Off
Option Explicit On
Imports System.Collections.Generic
Imports Microsoft.Win32

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CPrecReportFrm
	Inherits Form

#Region "Variable declarations"
	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private pGeomElem As ESRI.ArcGIS.Carto.IElement

	Private LObstaclesPage01 As ObstacleContainer
	Private SortF01 As Integer

	Private LObstaclesPage02 As ObstacleContainer
	Private SortF02 As Integer

	Private LObstaclesPage03 As ObstacleContainer
	Private SortF03 As Integer

	Private LObstaclesPage04 As ObstacleContainer
	Private SortF04 As Integer
	'Private UseILSPlanes11 As Boolean

	Private LObstaclesPage05 As ObstacleContainer
	Private SortF05 As Integer

	Private LObstaclesPage06 As ObstacleContainer
	Private SortF06 As Integer
	'Private UseILSPlanes1 As Boolean

	Private LObstaclesPage07 As ObstacleContainer
	Private SortF07 As Integer
	'Private Ix07ID As Long
	Private PrecFlg07 As Integer
	Private UseILSPlanes07 As Boolean

	Private LObstaclesPage08 As ObstacleContainer
	Private SortF08 As Integer
	'Private UseILSPlanes2 As Boolean

	Private LObstaclesPage09 As ObstacleContainer
	Private SortF09 As Integer
	Private Ix09ID As Long

	Private HelpContextID As Integer = 11200
	Private ReportBtn As CheckBox

	Private lvwColumnSorter As ListViewColumnSorter
#End Region

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		ReDim LObstaclesPage01.Obstacles(-1)
		ReDim LObstaclesPage02.Obstacles(-1)
		ReDim LObstaclesPage03.Obstacles(-1)
		ReDim LObstaclesPage04.Obstacles(-1)
		ReDim LObstaclesPage05.Obstacles(-1)
		ReDim LObstaclesPage06.Obstacles(-1)
		ReDim LObstaclesPage07.Obstacles(-1)
		ReDim LObstaclesPage08.Obstacles(-1)
		ReDim LObstaclesPage09.Obstacles(-1)

		ReDim LObstaclesPage01.Parts(-1)
		ReDim LObstaclesPage02.Parts(-1)
		ReDim LObstaclesPage03.Parts(-1)
		ReDim LObstaclesPage04.Parts(-1)
		ReDim LObstaclesPage05.Parts(-1)
		ReDim LObstaclesPage06.Parts(-1)
		ReDim LObstaclesPage07.Parts(-1)
		ReDim LObstaclesPage08.Parts(-1)
		ReDim LObstaclesPage09.Parts(-1)

		SortF01 = 0
		SortF02 = 0
		SortF03 = 0
		SortF04 = 0
		SortF05 = 0
		SortF06 = 0
		SortF07 = 0
		SortF08 = 0
		SortF09 = 0

		'    ListView3.ToolTipText = "*-Obstacle + Final MOC is above the missed approach surface at the SOC" + Chr(9) + "**-Obstacle does not penetrate the extension of the missed approach surface"

		SaveBtn.Text = My.Resources.str30002
		CloseBtn.Text = My.Resources.str30001
		Me.Text = My.Resources.str30000

		MultiPage1.TabPages.Item(0).Text = My.Resources.str30025
		MultiPage1.TabPages.Item(1).Text = My.Resources.str30026
		MultiPage1.TabPages.Item(2).Text = My.Resources.str30027
		MultiPage1.TabPages.Item(3).Text = My.Resources.str30028
		MultiPage1.TabPages.Item(4).Text = My.Resources.str30006
		MultiPage1.TabPages.Item(5).Text = My.Resources.str30007
		MultiPage1.TabPages.Item(6).Text = My.Resources.str30008
		MultiPage1.TabPages.Item(7).Text = My.Resources.str30032
		MultiPage1.TabPages.Item(8).Text = My.Resources.str30010

		MultiPage1.SelectedIndex = 0

		ListView01.Columns.Item(0).Text = "Type"    'type
		ListView01.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView01.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Surface
		ListView01.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Abv. Tresh.
		ListView01.Columns.Item(4).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'Penetrate
		ListView01.Columns.Item(5).Text = "X (" + ReportHeightConverter(0).Unit + ")"                                     'LoadResString() 'X
		ListView01.Columns.Item(6).Text = "Y (" + ReportHeightConverter(0).Unit + ")"                                     'LoadResString() 'Y
		ListView01.Columns.Item(7).Text = My.Resources.str30052                                                     'Surface

		ListView02.Columns.Item(0).Text = "Type"    'type
		ListView02.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView02.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Surface
		ListView02.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Abv. Tresh.
		ListView02.Columns.Item(4).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'Penetrate
		ListView02.Columns.Item(5).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView02.Columns.Item(6).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y
		ListView02.Columns.Item(7).Text = My.Resources.str30052 'Surface

		ListView03.Columns.Item(0).Text = "Type"    'type
		ListView03.Columns.Item(1).Text = My.Resources.str30011    'name
		ListView03.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Surface
		ListView03.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Abv. Tresh.
		ListView03.Columns.Item(4).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'Penetrate
		ListView03.Columns.Item(5).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView03.Columns.Item(6).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y
		ListView03.Columns.Item(7).Text = My.Resources.str30052 'Surface
		ListView03.Columns.Item(8).Text = My.Resources.str30053 'ILS Category

		ListView04.Columns.Item(0).Text = "Type"    'type
		ListView04.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView04.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Surface
		ListView04.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Abv. Tresh.
		ListView04.Columns.Item(4).Text = "Equvalent height (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"             'Equvalent H
		ListView04.Columns.Item(5).Text = My.Resources.str30044 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'Req. OCH
		ListView04.Columns.Item(6).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView04.Columns.Item(7).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y
		ListView04.Columns.Item(8).Text = My.Resources.str30052 'Surface
		ListView04.Columns.Item(9).Text = My.Resources.str30053 'ILS Category
		ListView04.Columns.Item(10).Text = My.Resources.str30042    'Area
		ListView04.Columns.Item(11).Text = My.Resources.str30029 'Ignored

		ListView05.Columns.Item(0).Text = "Type"   'type
		ListView05.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView05.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'height
		ListView05.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'moc
		ListView05.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'req h
		ListView05.Columns.Item(5).Text = My.Resources.str30024 'area
		ListView05.Columns.Item(6).Text = My.Resources.str30054 + " (" + ReportHeightConverter(0).Unit + ")"
		ListView05.Columns.Item(7).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y

		ListView06.Columns.Item(0).Text = "Type"   'type
		ListView06.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView06.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Surface
		ListView06.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'H Abv. Tresh.
		ListView06.Columns.Item(4).Text = My.Resources.str30044 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Req. OCH
		ListView06.Columns.Item(5).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView06.Columns.Item(6).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y
		ListView06.Columns.Item(7).Text = My.Resources.str30052 'Surface
		ListView06.Columns.Item(8).Text = My.Resources.str30053 'ILS Category
		ListView06.Columns.Item(9).Text = My.Resources.str30042 'Area

		ListView07.Columns.Item(0).Text = "Type"   'type
		ListView07.Columns.Item(1).Text = My.Resources.str30011 'Name
		ListView07.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Height
		ListView07.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'MOC
		ListView07.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Req.H
		ListView07.Columns.Item(5).Text = My.Resources.str30044 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Req OCH
		ListView07.Columns.Item(6).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'hPnet
		ListView07.Columns.Item(7).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView07.Columns.Item(8).Text = My.Resources.str30046 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Avoid Dist
		ListView07.Columns.Item(9).Text = My.Resources.str30047 + " (°)"      'Avoid angle
		ListView07.Columns.Item(10).Text = My.Resources.str30024    'Area

		ListView08.Columns.Item(0).Text = "Type"   'type
		ListView08.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView08.Columns.Item(2).Text = My.Resources.str30049 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'H Surface
		ListView08.Columns.Item(3).Text = My.Resources.str30050 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'H Abv. Tresh.
		ListView08.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Req.H
		ListView08.Columns.Item(5).Text = "X (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView08.Columns.Item(6).Text = "Y (" + ReportHeightConverter(0).Unit + ")" 'LoadResString() 'Y
		ListView08.Columns.Item(7).Text = My.Resources.str30052 'Surface
		ListView08.Columns.Item(8).Text = My.Resources.str30053 'ILS Category
		ListView08.Columns.Item(9).Text = My.Resources.str30020 'Ignored

		ListView09.Columns.Item(0).Text = "Type"   'type
		ListView09.Columns.Item(1).Text = My.Resources.str30011    'Name
		ListView09.Columns.Item(2).Text = My.Resources.str40035 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")"    'Dist
		ListView09.Columns.Item(3).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Height
		ListView09.Columns.Item(4).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'MOC
		ListView09.Columns.Item(5).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'Req.H
		ListView09.Columns.Item(6).Text = My.Resources.str30038 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"     'TA Surface H
		ListView09.Columns.Item(7).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"    'hPnet
		ListView09.Columns.Item(8).Text = My.Resources.str30024 'Area


		lvwColumnSorter = New ListViewColumnSorter()

		ListView01.ListViewItemSorter = lvwColumnSorter
		ListView02.ListViewItemSorter = lvwColumnSorter
		ListView03.ListViewItemSorter = lvwColumnSorter
		ListView04.ListViewItemSorter = lvwColumnSorter
		ListView05.ListViewItemSorter = lvwColumnSorter
		ListView06.ListViewItemSorter = lvwColumnSorter
		ListView07.ListViewItemSorter = lvwColumnSorter
		ListView08.ListViewItemSorter = lvwColumnSorter
		ListView09.ListViewItemSorter = lvwColumnSorter
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

	Public Sub SortForSave()
		Dim CurrIndex As Integer
		Dim PrevIndex As Integer

		'Page 1 ========================================================================================
		CurrIndex = 4
		PrevIndex = System.Math.Abs(SortF01) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView01.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF01 = CurrIndex + 1
		UpdateListView01(CurrIndex)
		'ListView01_SelectedIndexChanged(ListView01, New System.EventArgs)

		'Page 2 ========================================================================================

		PrevIndex = System.Math.Abs(SortF02) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView02.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF02 = CurrIndex + 1
		UpdateListView02(CurrIndex)


		'Page 3 ========================================================================================

		PrevIndex = System.Math.Abs(SortF03) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView03.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF03 = CurrIndex + 1
		UpdateListView03(CurrIndex)


		'Page 4 ========================================================================================
		CurrIndex = 5
		PrevIndex = System.Math.Abs(SortF04) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView04.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF04 = CurrIndex + 1
		UpdateListView04(CurrIndex)


		'Page 5 ========================================================================================
		CurrIndex = 4
		PrevIndex = System.Math.Abs(SortF05) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView05.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF05 = CurrIndex + 1
		UpdateListView05(CurrIndex)

		'Page 6 ========================================================================================

		PrevIndex = System.Math.Abs(SortF06) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView06.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF06 = CurrIndex + 1
		UpdateListView06(CurrIndex)

		'Page 7 ========================================================================================
		CurrIndex = 8
		PrevIndex = System.Math.Abs(SortF07) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView07.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF07 = -CurrIndex - 1
		UpdateListView07(CurrIndex)

		'Page 8 ========================================================================================
		CurrIndex = 4
		PrevIndex = System.Math.Abs(SortF08) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView08.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF08 = CurrIndex + 1
		UpdateListView08(CurrIndex)

		'Page 9 ========================================================================================
		CurrIndex = 7

		PrevIndex = System.Math.Abs(SortF09) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView09.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF09 = CurrIndex + 1
		UpdateListView09(CurrIndex)
	End Sub

	Private Sub UpdateListView01(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage01.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF01) - 1 = sortByField Then
				SortF01 = -SortF01
				'If preClear Or (System.Math.Abs(SortF01) - 1 <> sortByField) Then
			Else 'If System.Math.Abs(SortF01) - 1 <> sortByField Then
				If SortF01 <> 0 Then ListView01.Columns.Item(System.Math.Abs(SortF01) - 1).ImageIndex = 2
				SortF01 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage01.Parts(I).fSort = LObstaclesPage01.Parts(I).fTmp
						Case 3
							LObstaclesPage01.Parts(I).fSort = LObstaclesPage01.Parts(I).Height
						Case 4
							LObstaclesPage01.Parts(I).fSort = LObstaclesPage01.Parts(I).hPenet
						Case 5
							LObstaclesPage01.Parts(I).fSort = LObstaclesPage01.Parts(I).Dist
						Case 6
							LObstaclesPage01.Parts(I).fSort = LObstaclesPage01.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage01.Parts(I).sSort = LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage01.Parts(I).sSort = LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).UnicalName
						Case 7
							LObstaclesPage01.Parts(I).sSort = OFZPlaneNames(LObstaclesPage01.Parts(I).Plane And 15)
					End Select
				Next I
			End If
			'End If

			If SortF01 > 0 Then
				ListView01.Columns(sortByField).ImageIndex = 0
			Else
				ListView01.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
				If SortF01 > 0 Then
					shall_SortfSort(LObstaclesPage01)
				Else
					shall_SortfSortD(LObstaclesPage01)
				End If
			Else
				If SortF01 > 0 Then
					shall_SortsSort(LObstaclesPage01)
				Else
					shall_SortsSortD(LObstaclesPage01)
				End If
			End If
		End If

		ListView01.BeginUpdate()
		'If preClear Then
		ListView01.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage01)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView01.Items.Add(LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage01.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage01.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, OFZPlaneNames(LObstaclesPage01.Parts(I).Plane)))
			itmX.Tag = I
			'Else
			'	itmX = ListView01.Items(I)
			'	itmX.Text = LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage01.Obstacles(LObstaclesPage01.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage01.Parts(I).hPenet, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(Math.Round(LObstaclesPage01.Parts(I).Dist, 1))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage01.Parts(I).DistStar, 1))
			'	itmX.SubItems(7).Text = OFZPlaneNames(LObstaclesPage01.Parts(I).Plane)
			'End If

			If LObstaclesPage01.Parts(I).hPenet > 0.0 Then
				itmX.ForeColor = Color.Red
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
			Else
				itmX.ForeColor = Color.Black
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
			End If

			For K = 1 To 7
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
				itmX.SubItems.Item(K).Font = itmX.Font
			Next K
		Next

		ListView01.EndUpdate()
	End Sub

	Private Sub UpdateListView02(Optional sortByField As Integer = -1)  ', Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage02.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF02) - 1 = sortByField Then SortF02 = -SortF02

			'If preClear Or (System.Math.Abs(SortF02) - 1 <> sortByField) Then
			If System.Math.Abs(SortF02) - 1 <> sortByField Then
				If SortF02 <> 0 Then ListView02.Columns.Item(System.Math.Abs(SortF02) - 1).ImageIndex = 2
				SortF02 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).fTmp
						Case 3
							LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Height
						Case 4
							LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).hPenet
						Case 5
							LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Dist
						Case 6
							LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage02.Parts(I).sSort = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage02.Parts(I).sSort = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).UnicalName
						Case 7
							LObstaclesPage02.Parts(I).sSort = ILSPlaneNames(LObstaclesPage02.Parts(I).Plane And 15)
					End Select
				Next I
			End If
			'End If

			If SortF02 > 0 Then
				ListView02.Columns(sortByField).ImageIndex = 0
			Else
				ListView02.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
				If SortF02 > 0 Then
					shall_SortfSort(LObstaclesPage02)
				Else
					shall_SortfSortD(LObstaclesPage02)
				End If
			Else
				If SortF02 > 0 Then
					shall_SortsSort(LObstaclesPage02)
				Else
					shall_SortsSortD(LObstaclesPage02)
				End If
			End If
		End If

		ListView02.BeginUpdate()
		'If preClear Then
		'	Dim Obstacles As ObstacleContainer

		'	Dim M As Integer = LObstaclesPage02.Obstacles.Length
		'	N = LObstaclesPage02.Parts.Length

		'	ReDim Obstacles.Obstacles(M - 1)
		'	ReDim Obstacles.Parts(N - 1)

		'	Array.Copy(LObstaclesPage02.Obstacles, Obstacles.Obstacles, M)
		'	Array.Copy(LObstaclesPage02.Parts, Obstacles.Parts, N)

		'	Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)
		'	N = unicalObstacleList.Count - 1
		'	M = 0

		'	ReDim LObstaclesPage02.Obstacles(N)
		'	ReDim LObstaclesPage02.Parts(N)

		'	For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
		'		I = item.Value
		'		LObstaclesPage02.Obstacles(M) = Obstacles.Obstacles(Obstacles.Parts(I).Owner)
		'		LObstaclesPage02.Parts(M) = Obstacles.Parts(I)
		'		LObstaclesPage02.Parts(M).Owner = M
		'		M += 1
		'	Next

		ListView02.Items.Clear()
		'End If

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage02)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView02.Items.Add(LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage02.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage02.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, ILSPlaneNames(LObstaclesPage02.Parts(I).Plane)))
			itmX.Tag = I
			'Else
			'	itmX = ListView02.Items(I)
			'	itmX.Text = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).hPenet, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(Math.Round(LObstaclesPage02.Parts(I).Dist, 1))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage02.Parts(I).DistStar, 1))
			'	itmX.SubItems(7).Text = ILSPlaneNames(LObstaclesPage02.Parts(I).Plane)
			'End If

			If LObstaclesPage02.Parts(I).hPenet > 0.0 Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.Red
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.Black
			End If

			For K = 1 To 7
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView02.EndUpdate()
	End Sub

	Private Sub UpdateListView03(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage03.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView03.Columns(sortByField)

			If System.Math.Abs(SortF03) - 1 = sortByField Then SortF03 = -SortF03

			'If preClear Or (System.Math.Abs(SortF03) - 1 <> sortByField) Then
			If System.Math.Abs(SortF03) - 1 <> sortByField Then
				If SortF03 <> 0 Then ListView03.Columns.Item(System.Math.Abs(SortF03) - 1).ImageIndex = 2
				SortF03 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).fTmp
							Case 3
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).Height
							Case 4
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).hPenet
							Case 5
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).Dist
							Case 6
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).DistStar
						End Select
					Next I
				Else
					For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage03.Parts(I).sSort = LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage03.Parts(I).sSort = LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).UnicalName
						Case 7
							LObstaclesPage03.Parts(I).sSort = OASPlaneNames(LObstaclesPage03.Parts(I).Plane And 15)
						Case 8
							If (LObstaclesPage03.Parts(I).Plane And 32) <> 0 Then
								LObstaclesPage03.Parts(I).sSort = "Cat 2"
							Else
								LObstaclesPage03.Parts(I).sSort = "Cat 1"
							End If
					End Select
				Next I
			End If
			'End If

			If SortF03 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField <= 6) Then
				If SortF03 > 0 Then
					shall_SortfSort(LObstaclesPage03)
				Else
					shall_SortfSortD(LObstaclesPage03)
				End If
			Else
				If SortF03 > 0 Then
					shall_SortsSort(LObstaclesPage03)
				Else
					shall_SortsSortD(LObstaclesPage03)
				End If
			End If
		End If

		ListView03.BeginUpdate()
		'If preClear Then
		ListView03.Items.Clear()


		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage03)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView03.Items.Add(LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage03.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage03.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage03.Parts(I).Plane And 15)))

			If (LObstaclesPage03.Parts(I).Plane And 32) <> 0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 2"))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 1"))
			End If
			itmX.Tag = I
			'Else
			'	itmX = ListView03.Items(I)
			'	itmX.Text = LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).hPenet, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(Math.Round(LObstaclesPage03.Parts(I).Dist, 1))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage03.Parts(I).DistStar, 1))
			'	itmX.SubItems(7).Text = OASPlaneNames(LObstaclesPage03.Parts(I).Plane And 15)

			'	If (LObstaclesPage03.Parts(I).Plane And 32) <> 0 Then
			'		itmX.SubItems(8).Text = "Cat 2"
			'	Else
			'		itmX.SubItems(8).Text = "Cat 1"
			'	End If
			'End If

			If LObstaclesPage03.Parts(I).hPenet > 0.0 Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.Red
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.Black
			End If

			For K = 1 To 8
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView03.EndUpdate()
	End Sub

	Private Sub UpdateListView04(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem
		Dim ZoneNames() As String = {My.Resources.str00820, My.Resources.str30007}

		N = UBound(LObstaclesPage04.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView04.Columns(sortByField)

			If System.Math.Abs(SortF04) - 1 = sortByField Then SortF04 = -SortF04

			'If preClear Or (System.Math.Abs(SortF04) - 1 <> sortByField) Then
			If System.Math.Abs(SortF04) - 1 <> sortByField Then
				If SortF04 <> 0 Then ListView04.Columns.Item(System.Math.Abs(SortF04) - 1).ImageIndex = 2
				SortF04 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField < 8) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).fTmp
						Case 3
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).Height
						Case 4
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).EffectiveHeight
						Case 5
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).ReqOCH
						Case 6
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).Dist
						Case 7
							LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage04.Parts(I).sSort = LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage04.Parts(I).sSort = LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).UnicalName
						Case 8
							LObstaclesPage04.Parts(I).sSort = OASPlaneNames(LObstaclesPage04.Parts(I).Plane And 15)
						Case 9
							If (LObstaclesPage04.Parts(I).Plane And 32) <> 0 Then
								LObstaclesPage04.Parts(I).sSort = "Cat 2"
							Else
								LObstaclesPage04.Parts(I).sSort = "Cat 1"
							End If
						Case 10
							LObstaclesPage04.Parts(I).sSort = ZoneNames(LObstaclesPage04.Parts(I).Flags)
						Case 11
							If (LObstaclesPage04.Parts(I).Plane And 16) <> 0 Then
								LObstaclesPage04.Parts(I).sSort = "Yes"
							Else
								LObstaclesPage04.Parts(I).sSort = "No"
							End If
					End Select
				Next I
			End If
			'End If

			If SortF04 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField < 8) Then
				If SortF04 > 0 Then
					shall_SortfSort(LObstaclesPage04)
				Else
					shall_SortfSortD(LObstaclesPage04)
				End If
			Else
				If SortF04 > 0 Then
					shall_SortsSort(LObstaclesPage04)
				Else
					shall_SortsSortD(LObstaclesPage04)
				End If
			End If
		End If

		'=====================================================================================================
		ListView04.BeginUpdate()
		'If preClear Then
		ListView04.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage04)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView04.Items.Add(LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).EffectiveHeight, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).ReqOCH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage04.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage04.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage04.Parts(I).Plane And 15)))

			If (LObstaclesPage04.Parts(I).Plane And 32) <> 0 Then
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "Cat 2"))
			Else
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "Cat 1"))
			End If

			itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, ZoneNames(LObstaclesPage04.Parts(I).Flags)))

			If (LObstaclesPage04.Parts(I).Plane And 16) <> 0 Then
				itmX.SubItems.Insert(11, New ListViewItem.ListViewSubItem(Nothing, "Yes"))
			Else
				itmX.SubItems.Insert(11, New ListViewItem.ListViewSubItem(Nothing, "No"))
			End If

			'If LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).IgnoredByUser Then
			'	itmX.SubItems.Insert(12, New ListViewItem.ListViewSubItem(Nothing, "Yes"))
			'Else
			'	itmX.SubItems.Insert(12, New ListViewItem.ListViewSubItem(Nothing, "No"))
			'End If
			itmX.Tag = I
			'Else
			'	itmX = ListView04.Items(I)
			'	itmX.Text = LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).EffectiveHeight, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).ReqOCH, eRoundMode.NEAREST))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage04.Parts(I).Dist, 1))
			'	itmX.SubItems(7).Text = CStr(Math.Round(LObstaclesPage04.Parts(I).DistStar, 1))
			'	itmX.SubItems(8).Text = OASPlaneNames(LObstaclesPage04.Parts(I).Plane And 15)

			'	If (LObstaclesPage04.Parts(I).Plane And 32) <> 0 Then
			'		itmX.SubItems(9).Text = "Cat 2"
			'	Else
			'		itmX.SubItems(9).Text = "Cat 1"
			'	End If

			'	itmX.SubItems(10).Text = ZoneNames(LObstaclesPage04.Parts(I).Flags)

			'	If (LObstaclesPage04.Parts(I).Plane And 16) <> 0 Then
			'		itmX.SubItems(11).Text = "Yes"
			'	Else
			'		itmX.SubItems(11).Text = "No"
			'	End If

			'	'If LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).IgnoredByUser Then
			'	'	itmX.SubItems(12).Text = "Yes"
			'	'Else
			'	'	itmX.SubItems(12).Text = "No"
			'	'End If
			'End If


			If LObstaclesPage04.Parts(I).ReqOCH > 0.0 Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.Red
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.Black
			End If

			For K = 1 To 10
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView04.EndUpdate()
	End Sub

	Private Sub UpdateListView05(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage05.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView05.Columns(sortByField)

			If System.Math.Abs(SortF05) - 1 = sortByField Then SortF05 = -SortF05

			'If preClear Or (System.Math.Abs(SortF05) - 1 <> sortByField) Then
			If System.Math.Abs(SortF05) - 1 <> sortByField Then
				If SortF05 <> 0 Then ListView05.Columns.Item(System.Math.Abs(SortF05) - 1).ImageIndex = 2
				SortF05 = -sortByField - 1
			End If

			If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 7) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Height
						Case 3
							LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).MOC
						Case 4
							LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).ReqH
						Case 6
							LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Dist
						Case 7
							LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage05.Parts(I).sSort = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage05.Parts(I).sSort = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).UnicalName
						Case 5
							If LObstaclesPage05.Parts(I).Flags = 0 Then
								LObstaclesPage05.Parts(I).sSort = "Primary"
							Else
								LObstaclesPage05.Parts(I).sSort = "Secondary"
							End If
					End Select
				Next I
			End If
			'End If

			If SortF05 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 7) Then
				If SortF05 > 0 Then
					shall_SortfSort(LObstaclesPage05)
				Else
					shall_SortfSortD(LObstaclesPage05)
				End If
			Else
				If SortF05 > 0 Then
					shall_SortsSort(LObstaclesPage05)
				Else
					shall_SortsSortD(LObstaclesPage05)
				End If
			End If
		End If

		ListView05.BeginUpdate()
		'If preClear Then
		ListView05.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage05)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView05.Items.Add(LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).ReqH, eRoundMode.NEAREST))))

			If LObstaclesPage05.Parts(I).Flags <> 0 Then
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage05.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage05.Parts(I).DistStar, 1))))
			itmX.Tag = I
			'Else
			'	itmX = ListView05.Items(I)
			'	itmX.Text = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).MOC, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).ReqH, eRoundMode.NEAREST))

			'	If LObstaclesPage05.Parts(I).Flags <> 0 Then
			'		itmX.SubItems(5).Text = "Primary"
			'	Else
			'		itmX.SubItems(5).Text = "Secondary"
			'	End If
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage05.Parts(I).Dist, 1))
			'	itmX.SubItems(7).Text = CStr(Math.Round(LObstaclesPage05.Parts(I).DistStar, 1))
			'End If
		Next

		ListView05.EndUpdate()
	End Sub

	Private Sub UpdateListView06(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem
		Dim ZoneNames() As String = {My.Resources.str00820, My.Resources.str30007}

		N = UBound(LObstaclesPage06.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView06.Columns(sortByField)

			If System.Math.Abs(SortF06) - 1 = sortByField Then SortF06 = -SortF06

			'If preClear Or (System.Math.Abs(SortF06) - 1 <> sortByField) Then
			If System.Math.Abs(SortF06) - 1 <> sortByField Then
				If SortF06 <> 0 Then ListView06.Columns.Item(System.Math.Abs(SortF06) - 1).ImageIndex = 2
				SortF06 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField < 7) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).fTmp
						Case 3
							LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).Height
						Case 4
							LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).ReqOCH
						Case 5
							LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).Dist
						Case 6
							LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage06.Parts(I).sSort = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage06.Parts(I).sSort = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).UnicalName
						Case 7
							LObstaclesPage06.Parts(I).sSort = OASPlaneNames(LObstaclesPage06.Parts(I).Plane And 15)
						Case 8
							If (LObstaclesPage06.Parts(I).Plane And 32) <> 0 Then
								LObstaclesPage06.Parts(I).sSort = "Cat 2"
							Else
								LObstaclesPage06.Parts(I).sSort = "Cat 1"
							End If
						Case 9
							LObstaclesPage06.Parts(I).sSort = ZoneNames(LObstaclesPage06.Parts(I).Flags)
					End Select
				Next I
			End If
			'End If

			If SortF06 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField < 7) Then
				If SortF06 > 0 Then
					shall_SortfSort(LObstaclesPage06)
				Else
					shall_SortfSortD(LObstaclesPage06)
				End If
			Else
				If SortF06 > 0 Then
					shall_SortsSort(LObstaclesPage06)
				Else
					shall_SortsSortD(LObstaclesPage06)
				End If
			End If
		End If

		ListView06.BeginUpdate()
		'If preClear Then 
		ListView06.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage06)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView06.Items.Add(LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).ReqOCH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage06.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage06.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage06.Parts(I).Plane And 15)))

			If (LObstaclesPage06.Parts(I).Plane And 32) <> 0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 2"))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 1"))
			End If
			itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, ZoneNames(LObstaclesPage06.Parts(I).Flags)))
			itmX.Tag = I
			'Else
			'	itmX = ListView06.Items(I)
			'	itmX.Text = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).ReqOCH, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(Math.Round(LObstaclesPage06.Parts(I).Dist, 1))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage06.Parts(I).DistStar, 1))
			'	itmX.SubItems(7).Text = OASPlaneNames(LObstaclesPage06.Parts(I).Plane And 15)

			'	If (LObstaclesPage06.Parts(I).Plane And 32) <> 0 Then
			'		itmX.SubItems(8).Text = "Cat 2"
			'	Else
			'		itmX.SubItems(8).Text = "Cat 1"
			'	End If
			'	itmX.SubItems(9).Text = ZoneNames(LObstaclesPage06.Parts(I).Flags)
			'End If
		Next

		ListView06.EndUpdate()
	End Sub

	Private Sub UpdateListView07(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage07.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF07) - 1 = sortByField Then SortF07 = -SortF07

			'If preClear Or (System.Math.Abs(SortF07) - 1 <> sortByField) Then
			If System.Math.Abs(SortF07) - 1 <> sortByField Then
				If SortF07 <> 0 Then ListView07.Columns.Item(System.Math.Abs(SortF07) - 1).ImageIndex = 2
				SortF07 = sortByField + 1
			End If

			If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 9) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Height
						Case 4
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).ReqH
						Case 5
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).ReqOCH
						Case 6
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).hPenet
						Case 7
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Dist
						Case 8
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).TurnDistL
						Case 9
							LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).TurnAngleL
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage07.Parts(I).sSort = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage07.Parts(I).sSort = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).UnicalName
						Case 10
							If PrecFlg07 < 0 Then
								If LObstaclesPage07.Parts(I).Plane <> 0 Then
									LObstaclesPage07.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage07.Parts(I).sSort = "Secondary"
								End If
							Else
								If UseILSPlanes07 Then
									LObstaclesPage07.Parts(I).sSort = ILSPlaneNames(LObstaclesPage07.Parts(I).Plane)
								Else
									LObstaclesPage07.Parts(I).sSort = OASPlaneNames(LObstaclesPage07.Parts(I).Plane And 15)
								End If
							End If
					End Select
				Next I
			End If
			'End If

			If SortF07 > 0 Then
				ListView07.Columns(sortByField).ImageIndex = 0
			Else
				ListView07.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 9) Then
				If SortF07 > 0 Then
					shall_SortfSort(LObstaclesPage07)
				Else
					shall_SortfSortD(LObstaclesPage07)
				End If
			Else
				If SortF07 > 0 Then
					shall_SortsSort(LObstaclesPage07)
				Else
					shall_SortsSortD(LObstaclesPage07)
				End If
			End If
		End If

		ListView07.BeginUpdate()
		'If preClear Then
		ListView07.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage07)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView07.Items.Add(LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, "-"))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).ReqOCH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage07.Parts(I).Dist, 1))))

			If LObstaclesPage07.Parts(I).TurnDistL > 0.0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(LObstaclesPage07.Parts(I).TurnDistL, eRoundMode.NEAREST))))
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(LObstaclesPage07.Parts(I).TurnAngleL, 1))))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "-"))
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "-"))
			End If

			If PrecFlg07 < 0 Then
				If LObstaclesPage07.Parts(I).Flags <> 0 Then
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
				Else
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
				End If
			Else
				If LObstaclesPage07.Parts(I).Plane = eOAS.NonPrec Then itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, "-"))

				If UseILSPlanes07 Then
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, ILSPlaneNames(LObstaclesPage07.Parts(I).Plane And 15)))
				Else
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage07.Parts(I).Plane And 15)))
				End If
			End If
			itmX.Tag = I
			'Else
			'	itmX = ListView07.Items(I)
			'	itmX.Text = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = "-"
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).ReqH, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).ReqOCH, eRoundMode.NEAREST))
			'	itmX.SubItems(6).Text = CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).hPenet, eRoundMode.NEAREST))
			'	itmX.SubItems(7).Text = CStr(Math.Round(LObstaclesPage07.Parts(I).Dist, 1))

			'	If LObstaclesPage07.Parts(I).TurnDistL > 0.0 Then
			'		itmX.SubItems(8).Text = CStr(ConvertDistance(LObstaclesPage07.Parts(I).TurnDistL, eRoundMode.NEAREST))
			'		itmX.SubItems(9).Text = CStr(System.Math.Round(LObstaclesPage07.Parts(I).TurnAngleL, 1))
			'	Else
			'		itmX.SubItems(8).Text = "-"
			'		itmX.SubItems(9).Text = "-"
			'	End If

			'	If PrecFlg07 < 0 Then
			'		If LObstaclesPage07.Parts(I).Flags <> 0 Then
			'			itmX.SubItems(10).Text = "Primary"
			'		Else
			'			itmX.SubItems(10).Text = "Secondary"
			'		End If
			'	Else
			'		If LObstaclesPage07.Parts(I).Plane = eOAS.NonPrec Then itmX.SubItems(6).Text = "-"

			'		If UseILSPlanes07 Then
			'			itmX.SubItems(10).Text = ILSPlaneNames(LObstaclesPage07.Parts(I).Plane And 15)
			'		Else
			'			itmX.SubItems(10).Text = OASPlaneNames(LObstaclesPage07.Parts(I).Plane And 15)
			'		End If
			'	End If
			'End If
		Next

		ListView07.EndUpdate()
	End Sub

	Private Sub UpdateListView08(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem
		Dim Ignored() As String = {"No", "Yes"}

		N = UBound(LObstaclesPage08.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF08) - 1 = sortByField Then SortF08 = -SortF08

			'If preClear Or (System.Math.Abs(SortF08) - 1 <> sortByField) Then
			If System.Math.Abs(SortF08) - 1 <> sortByField Then
				If SortF08 <> 0 Then ListView08.Columns.Item(System.Math.Abs(SortF08) - 1).ImageIndex = 2
				SortF08 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField < 7) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).fTmp
						Case 3
							LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).Height
						Case 4
							LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).ReqH
						Case 5
							LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).Dist
						Case 6
							LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).DistStar
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage08.Parts(I).sSort = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage08.Parts(I).sSort = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).UnicalName
						Case 7
							LObstaclesPage08.Parts(I).sSort = OASPlaneNames(LObstaclesPage08.Parts(I).Plane And 15)
						Case 8
							If (LObstaclesPage08.Parts(I).Plane And 32) <> 0 Then
								LObstaclesPage08.Parts(I).sSort = "Cat 2"
							Else
								LObstaclesPage08.Parts(I).sSort = "Cat 1"
							End If
						Case 9
							LObstaclesPage08.Parts(I).sSort = Ignored(LObstaclesPage08.Parts(I).Flags)
					End Select
				Next I
			End If
			'End If

			If SortF08 > 0 Then
				ListView08.Columns(sortByField).ImageIndex = 0
			Else
				ListView08.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField < 7) Then
				If SortF08 > 0 Then
					shall_SortfSort(LObstaclesPage08)
				Else
					shall_SortfSortD(LObstaclesPage08)
				End If
			Else
				If SortF08 > 0 Then
					shall_SortsSort(LObstaclesPage08)
				Else
					shall_SortsSortD(LObstaclesPage08)
				End If
			End If
		End If

		ListView08.BeginUpdate()
		'If preClear Then
		ListView08.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage08)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView08.Items.Add(LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage08.Parts(I).Dist, 1))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LObstaclesPage08.Parts(I).DistStar, 1))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage08.Parts(I).Plane And 15)))

			If (LObstaclesPage08.Parts(I).Plane And 32) <> 0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 2"))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Cat 1"))
			End If
			itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, Ignored(LObstaclesPage08.Parts(I).Flags)))
			itmX.Tag = I
			'Else
			'	itmX = ListView08.Items(I)
			'	itmX.Text = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).fTmp, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).ReqH, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(Math.Round(LObstaclesPage08.Parts(I).Dist, 1))
			'	itmX.SubItems(6).Text = CStr(Math.Round(LObstaclesPage08.Parts(I).DistStar, 1))
			'	itmX.SubItems(7).Text = OASPlaneNames(LObstaclesPage08.Parts(I).Plane And 15)

			'	If (LObstaclesPage08.Parts(I).Plane And 32) <> 0 Then
			'		itmX.SubItems(8).Text = "Cat 2"
			'	Else
			'		itmX.SubItems(8).Text = "Cat 1"
			'	End If
			'	itmX.SubItems(9).Text = Ignored(LObstaclesPage08.Parts(I).Flags)
			'End If
		Next

		ListView08.EndUpdate()
	End Sub

	Private Sub UpdateListView09(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage09.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF09) - 1 = sortByField Then SortF09 = -SortF09

			'If preClear Or (System.Math.Abs(SortF09) - 1 <> sortByField) Then
			If System.Math.Abs(SortF09) - 1 <> sortByField Then
				If SortF09 <> 0 Then ListView09.Columns.Item(System.Math.Abs(SortF09) - 1).ImageIndex = 2
				SortF09 = -sortByField - 1
			End If

			If (sortByField >= 2) And (sortByField <= 7) Then
				For I = 0 To N
					Select Case sortByField
						Case 2
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).Dist
						Case 3
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).Height
						Case 4
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).MOC
						Case 5
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).ReqH
						Case 6
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).EffectiveHeight
						Case 7
							LObstaclesPage09.Parts(I).fSort = LObstaclesPage09.Parts(I).hPenet
					End Select
				Next I
			Else
				For I = 0 To N
					Select Case sortByField
						Case 0
							LObstaclesPage09.Parts(I).sSort = LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).TypeName
						Case 1
							LObstaclesPage09.Parts(I).sSort = LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).UnicalName
						Case 8
							If LObstaclesPage09.Parts(I).Flags = 0 Then
								LObstaclesPage09.Parts(I).sSort = "Primary"
							Else
								LObstaclesPage09.Parts(I).sSort = "Secondary"
							End If
					End Select
				Next I
			End If
			'End If

			Dim pColumnHeader As ColumnHeader = ListView09.Columns(sortByField)
			If SortF09 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField <= 7) Then
				If SortF09 > 0 Then
					shall_SortfSort(LObstaclesPage09)
				Else
					shall_SortfSortD(LObstaclesPage09)
				End If
			Else
				If SortF09 > 0 Then
					shall_SortsSort(LObstaclesPage09)
				Else
					shall_SortsSortD(LObstaclesPage09)
				End If
			End If
		End If

		ListView09.BeginUpdate()
		'If preClear Then
		ListView09.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage09)
		'N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			'If preClear Then
			I = item.Value
			itmX = ListView09.Items.Add(LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage09.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).EffectiveHeight, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).hPenet, eRoundMode.NEAREST))))

			If LObstaclesPage09.Parts(I).Flags = 0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If
			itmX.Tag = I
			'Else
			'	itmX = ListView09.Items(I)
			'	itmX.Text = LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).TypeName
			'	itmX.SubItems(1).Text = LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).UnicalName
			'	itmX.SubItems(2).Text = CStr(ConvertReportDistance(LObstaclesPage09.Parts(I).Dist, eRoundMode.NEAREST))
			'	itmX.SubItems(3).Text = CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).Height, eRoundMode.NEAREST))
			'	itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).MOC, eRoundMode.NEAREST))
			'	itmX.SubItems(5).Text = CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).ReqH, eRoundMode.NEAREST))
			'	itmX.SubItems(6).Text = CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).EffectiveHeight, eRoundMode.NEAREST))
			'	itmX.SubItems(7).Text = CStr(ConvertReportHeight(LObstaclesPage09.Parts(I).hPenet, eRoundMode.NEAREST))

			'	If LObstaclesPage09.Parts(I).Flags = 0 Then
			'		itmX.SubItems(8).Text = "Primary"
			'	Else
			'		itmX.SubItems(8).Text = "Secondary"
			'	End If
			'End If

			If LObstaclesPage09.Obstacles(LObstaclesPage09.Parts(I).Owner).ID = Ix09ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.Red
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.Black
			End If

			For K = 1 To 8
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView09.EndUpdate()
	End Sub

	Public Sub FillPage01(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		'Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)
		'Dim N As Integer = unicalObstacleList.Count - 1

		'ReDim LObstaclesPage01.Obstacles(N)
		'ReDim LObstaclesPage01.Parts(N)
		'Dim M As Integer = 0

		'For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
		'	Dim I As Integer = item.Value
		'	LObstaclesPage01.Obstacles(M) = Obstacles.Obstacles(Obstacles.Parts(I).Owner)
		'	LObstaclesPage01.Parts(M) = Obstacles.Parts(I)
		'	LObstaclesPage01.Parts(M).Owner = M
		'	M += 1
		'Next

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage01.Obstacles(M - 1)
		ReDim LObstaclesPage01.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage01.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage01.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF01) - 1

		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView01.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF01 = CurrIndex + 1

		UpdateListView01(CurrIndex, True)
		lblCountNumber.Text = ListView01.Items.Count

		MultiPage1.SelectedIndex = 0
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage02(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage02.Obstacles(M - 1)
		ReDim LObstaclesPage02.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage02.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage02.Parts, N)

		'Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)
		'Dim N As Integer = unicalObstacleList.Count - 1

		'ReDim LObstaclesPage02.Obstacles(N)
		'ReDim LObstaclesPage02.Parts(N)
		'Dim M As Integer = 0

		'For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
		'	Dim I As Integer = item.Value
		'	LObstaclesPage02.Obstacles(M) = Obstacles.Obstacles(Obstacles.Parts(I).Owner)
		'	LObstaclesPage02.Parts(M) = Obstacles.Parts(I)
		'	LObstaclesPage02.Parts(M).Owner = M
		'	M += 1
		'Next

		Dim PrevIndex As Integer = System.Math.Abs(SortF02) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView02.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF02 = CurrIndex + 1

		UpdateListView02(CurrIndex) ', True)
		lblCountNumber.Text = ListView02.Items.Count

		MultiPage1.SelectedIndex = 1
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage03(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage03.Obstacles(M - 1)
		ReDim LObstaclesPage03.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage03.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage03.Parts, N)

		'Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)

		'Dim N As Integer = unicalObstacleList.Count - 1
		'ReDim LObstaclesPage03.Obstacles(N)
		'ReDim LObstaclesPage03.Parts(N)
		'Dim M As Integer = 0

		'For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
		'	Dim I As Integer = item.Value
		'	LObstaclesPage03.Obstacles(M) = Obstacles.Obstacles(Obstacles.Parts(I).Owner)
		'	LObstaclesPage03.Parts(M) = Obstacles.Parts(I)
		'	LObstaclesPage03.Parts(M).Owner = M
		'	M += 1
		'Next

		Dim PrevIndex As Integer = System.Math.Abs(SortF03) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView03.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF03 = CurrIndex + 1

		UpdateListView03(CurrIndex, True)
		lblCountNumber.Text = ListView03.Items.Count

		MultiPage1.SelectedIndex = 2
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage04(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 5

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage04.Obstacles(M - 1)
		ReDim LObstaclesPage04.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage04.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage04.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF04) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView04.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF04 = CurrIndex + 1

		UpdateListView04(CurrIndex, True)
		lblCountNumber.Text = ListView04.Items.Count

		MultiPage1.SelectedIndex = 3
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage05(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage05.Obstacles(M - 1)
		ReDim LObstaclesPage05.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage05.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage05.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF05) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView05.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF05 = CurrIndex + 1

		UpdateListView05(CurrIndex, True)
		lblCountNumber.Text = ListView05.Items.Count

		MultiPage1.SelectedIndex = 4
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage06(ByRef Obstacles As ObstacleContainer, Optional ByRef UseILSPlanes As Boolean = False)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage06.Obstacles(M - 1)
		ReDim LObstaclesPage06.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage06.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage06.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF06) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView06.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF06 = CurrIndex + 1

		UpdateListView06(CurrIndex, True)
		lblCountNumber.Text = ListView06.Items.Count

		MultiPage1.SelectedIndex = 5
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage07(ByRef Obstacles As ObstacleContainer, Optional ByRef PrecFlg As Integer = -1, Optional ByRef UseILSPlanes As Boolean = False)
		Const CurrIndex As Integer = 8

		PrecFlg07 = PrecFlg
		UseILSPlanes07 = UseILSPlanes

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage07.Obstacles(M - 1)
		ReDim LObstaclesPage07.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage07.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage07.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF07) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView07.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF07 = -CurrIndex - 1

		UpdateListView07(CurrIndex, True)
		lblCountNumber.Text = ListView07.Items.Count

		MultiPage1.SelectedIndex = 6
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage08(ByRef Obstacles As ObstacleContainer, Optional ByRef UseILSPlanes As Boolean = False)
		Const CurrIndex As Integer = 4

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage08.Obstacles(M - 1)
		ReDim LObstaclesPage08.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage08.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage08.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF08) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView08.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF08 = CurrIndex + 1

		UpdateListView08(CurrIndex, True)
		lblCountNumber.Text = ListView08.Items.Count

		MultiPage1.SelectedIndex = 7
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage09(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1)
		Const CurrIndex As Integer = 7

		If Ix >= 0 Then
			Ix09ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix09ID = -1
		End If

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		Dim M As Integer = Obstacles.Obstacles.Length
		Dim N As Integer = Obstacles.Parts.Length

		ReDim LObstaclesPage09.Obstacles(M - 1)
		ReDim LObstaclesPage09.Parts(N - 1)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage09.Obstacles, M)
		Array.Copy(Obstacles.Parts, LObstaclesPage09.Parts, N)

		Dim PrevIndex As Integer = System.Math.Abs(SortF09) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView09.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF09 = CurrIndex + 1

		UpdateListView09(CurrIndex, True)
		lblCountNumber.Text = ListView09.Items.Count

		MultiPage1.SelectedIndex = 8
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Private Sub ListView01_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView01.ColumnClick
		'UpdateListView01(eventArgs.Column)
		'If ReportBtn.Checked Then ListView01_SelectedIndexChanged(ListView01, New System.EventArgs())

		'Dim SortF As Integer = CType(eventSender, ListView).Tag
		'lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF) - 1
		'SortF = eventArgs.Column + 1
		'SortListView(eventArgs.Column, lvwColumnSorter, CType(eventSender, ListView))

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF01) - 1
		SortF01 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView01)
	End Sub

	Private Sub ListView02_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView02.ColumnClick
		'UpdateListView02(eventArgs.Column)
		'ListView02_SelectedIndexChanged(ListView02, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF02) - 1
		SortF02 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView02)
	End Sub

	Private Sub ListView03_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView03.ColumnClick
		'UpdateListView03(eventArgs.Column)
		'ListView03_SelectedIndexChanged(ListView03, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF03) - 1
		SortF03 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView03)
	End Sub

	Private Sub ListView04_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView04.ColumnClick
		'UpdateListView04(eventArgs.Column)
		'If ReportBtn.Checked Then ListView04_SelectedIndexChanged(ListView04, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF04) - 1
		SortF04 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView04)
	End Sub

	Private Sub ListView05_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView05.ColumnClick
		'UpdateListView05(eventArgs.Column)
		'ListView05_SelectedIndexChanged(ListView05, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF05) - 1
		SortF05 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView05)
	End Sub

	Private Sub ListView06_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView06.ColumnClick
		'UpdateListView06(eventArgs.Column)
		'If ReportBtn.Checked Then ListView06_SelectedIndexChanged(ListView06, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF06) - 1
		SortF06 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView06)
	End Sub

	Private Sub ListView07_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView07.ColumnClick
		'UpdateListView07(eventArgs.Column)
		'If ReportBtn.Checked Then ListView07_SelectedIndexChanged(ListView07, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF07) - 1
		SortF07 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView07)
	End Sub

	Private Sub ListView08_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView08.ColumnClick
		'UpdateListView08(eventArgs.Column)
		'If ReportBtn.Checked Then ListView08_SelectedIndexChanged(ListView08, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF08) - 1
		SortF08 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView08)
	End Sub

	Private Sub ListView09_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView09.ColumnClick
		'UpdateListView09(eventArgs.Column)
		'ListView09_SelectedIndexChanged(ListView09, New System.EventArgs())

		lvwColumnSorter.ColumntToSort = System.Math.Abs(SortF09) - 1
		SortF09 = eventArgs.Column + 1
		SortListView(eventArgs.Column, lvwColumnSorter, ListView09)
	End Sub

	Private Sub ListView01_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView01.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage01.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage01.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage01.Obstacles(Index).pGeomPrj
		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage01.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage01.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView02_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView02.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage02.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage02.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage02.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage02.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage02.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView03_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView03.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage03.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage03.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage03.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage03.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage03.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView04_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView04.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage04.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage04.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage04.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage04.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage04.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView05_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView05.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage05.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage05.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage05.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage05.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage05.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView06_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView06.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage06.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage06.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage06.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage06.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage06.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView07_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView07.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage07.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage07.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage07.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage07.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage07.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView08_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView08.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage08.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage08.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage08.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage08.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage08.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView09_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView09.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)
		If (sender.SelectedItems.Count = 0) Or (UBound(LObstaclesPage09.Obstacles) < 0) Or (Not Visible) Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If Item Is Nothing Then Return

		Dim Index As Integer = LObstaclesPage09.Parts(Item.Tag).Owner
		Dim pGeometry As ArcGIS.Geometry.IGeometry = LObstaclesPage09.Obstacles(Index).pGeomPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		Dim pPtTmp As ArcGIS.Geometry.IPoint = LObstaclesPage09.Parts(Item.Tag).pPtPrj
		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage09.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		Select Case MultiPage1.SelectedIndex()
			Case 0
				ListView01_SelectedIndexChanged(ListView01, New System.EventArgs())
				lblCountNumber.Text = ListView01.Items.Count
			Case 1
				ListView02_SelectedIndexChanged(ListView02, New System.EventArgs())
				lblCountNumber.Text = ListView02.Items.Count
			Case 2
				ListView03_SelectedIndexChanged(ListView03, New System.EventArgs())
				lblCountNumber.Text = ListView03.Items.Count
			Case 3
				ListView04_SelectedIndexChanged(ListView04, New System.EventArgs())
				lblCountNumber.Text = ListView04.Items.Count
			Case 4
				ListView05_SelectedIndexChanged(ListView05, New System.EventArgs())
				lblCountNumber.Text = ListView05.Items.Count
			Case 5
				ListView06_SelectedIndexChanged(ListView06, New System.EventArgs())
				lblCountNumber.Text = ListView06.Items.Count
			Case 6
				ListView07_SelectedIndexChanged(ListView07, New System.EventArgs())
				lblCountNumber.Text = ListView07.Items.Count
			Case 7
				ListView08_SelectedIndexChanged(ListView08, New System.EventArgs())
				lblCountNumber.Text = ListView08.Items.Count
			Case 8
				ListView09_SelectedIndexChanged(ListView09, New System.EventArgs())
				lblCountNumber.Text = ListView09.Items.Count
		End Select
	End Sub

	Private Sub CPrecReportFrm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
		Erase LObstaclesPage01.Parts
		Erase LObstaclesPage02.Parts
		Erase LObstaclesPage03.Parts
		Erase LObstaclesPage04.Parts
		Erase LObstaclesPage05.Parts
		Erase LObstaclesPage06.Parts
		Erase LObstaclesPage07.Parts
		Erase LObstaclesPage08.Parts
		Erase LObstaclesPage09.Parts

		Erase LObstaclesPage01.Obstacles
		Erase LObstaclesPage02.Obstacles
		Erase LObstaclesPage03.Obstacles
		Erase LObstaclesPage04.Obstacles
		Erase LObstaclesPage05.Obstacles
		Erase LObstaclesPage06.Obstacles
		Erase LObstaclesPage07.Obstacles
		Erase LObstaclesPage08.Obstacles
		Erase LObstaclesPage09.Obstacles
	End Sub

	Private Sub CPrecReportFrm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As FormClosingEventArgs) Handles Me.FormClosing
		If eventArgs.CloseReason = CloseReason.UserClosing Then
			eventArgs.Cancel = True
			CloseBtn_Click(CloseBtn, New EventArgs())
		End If
	End Sub

	Private Sub CloseBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		ReportBtn.Checked = False
		Hide()
	End Sub

#If SAVEREPORT Then
	Private Function CreateReportFile(ByRef pReport As ReportType) As Integer
		Dim L As Integer
		Dim pos As Integer
		Dim LogFileNum As Integer
		Dim FileName As String
		Dim pProcName As String
		Dim Delime As String
		Dim KeyStr As String
		Dim Flg As Boolean

		On Error GoTo EH

		FileName = GetMapFileName()

		L = Len(FileName)
		pos = InStrRev(FileName, ".")
		Delime = Chr(34)

		If pos <> 0 Then
			pProcName = Microsoft.VisualBasic.Left(FileName, pos)
			FileName = Microsoft.VisualBasic.Left(FileName, pos) + "htm"
		Else
			pProcName = FileName
			FileName = FileName + ".htm"
		End If

		L = Len(pProcName)
		pos = InStrRev(pProcName, "\")
		If pos <> 0 Then pProcName = Microsoft.VisualBasic.Right(pProcName, L - pos)

		LogFileNum = FreeFile()
		CreateReportFile = LogFileNum

		'Dim fso As New Scripting.FileSystemObject
		'Dim fil As Scripting.File
		'Debug.Print "File last modified: "; fil.DateLastModified ' Print info.

		'Flg = False
		FileOpen(LogFileNum, FileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockReadWrite)

		'fil = fso.GetFile(FileName) ' Get a File object to query.
		'Flg = fil.size < 10
		'Flg = True

		'If Flg Then
		PrintLine(LogFileNum, "<HTML>")
		PrintLine(LogFileNum, "<HEAD>")
		PrintLine(LogFileNum, "<TITLE>" + Text + "</TITLE>") 'PANDA REPORT
		PrintLine(LogFileNum, "<style>{font-family: " + Delime + "Arial" + Delime + "; font-size: 11pt;")
		PrintLine(LogFileNum, "color: #000000; margin-top: 0px; margin-left: 0px; }</style>")
		PrintLine(LogFileNum, "</HEAD>")
		PrintLine(LogFileNum, "<BODY>")
		'End If
		'Print #LogFileNum, "<p><Font Face=" + Delime + "Arial" + Delime + " Style=" + Delime + "font-size: 11pt" + Delime + ">"
		'Print #LogFileNum, "<style> font-family: " + Delime + "Arial" + Delime

		KeyStr = "Software\Microsoft\MS Setup (ACME)\User Info"
		PrintLine(LogFileNum, "<p></p>")
		PrintLine(LogFileNum, "<p></p>")
		PrintLine(LogFileNum, "<p><Font color=0000FF><b>" + Text + "</b></Font></p>")

		PrintLine(LogFileNum, "<p>User name: " + RegRead(PANDARootKey, KeyStr, "DefName") + "<br>")
		PrintLine(LogFileNum, "<b>Company name: " + RegRead(PANDARootKey, KeyStr, "DefCompany") + "</b></p>")
		'Print #LogFileNum, "<p>User name:       Vadims Tumarkins<br>"
		'Print #LogFileNum, "<b>Company name:    Latvijas Gaisa Satiksme</b></p>"

		PrintLine(LogFileNum, "<p><b>" + "PANDA Version: 1.00" + "</b><br>")
		PrintLine(LogFileNum, "ESRI Map document:  " + GetMapFileName() + "<br>")
		PrintLine(LogFileNum, "Approach procedure<br>")
		PrintLine(LogFileNum, "Specific procedure type:  " + pReport.Procedure + "</p>")

		PrintLine(LogFileNum, "<p>" + "Runway:  " + pReport.RWY + "<br>")
		PrintLine(LogFileNum, "Aircraft category: up to " + pReport.Category + "</p>")

		PrintLine(LogFileNum, "<p><b>" + "Date:" + "</b>    " + CStr(Today) + "<br>")
		PrintLine(LogFileNum, "<b>" + "Time:" + "</b>    " + CStr(TimeOfDay) + "</p>")


		PrintLine(LogFileNum, "<p>" + "Distances are in " + DistanceConverter(DistanceUnit).Unit + ".<br>")
		PrintLine(LogFileNum, "Elevations and heights are in " + HeightConverter(HeightUnit).Unit + ".</p>")

		PrintLine(LogFileNum, "<br>")
		PrintLine(LogFileNum, "Координаты узловых точек: <br>")

		'===================================================================================================================
		PrintLine(LogFileNum, "<Table border=1>")
		PrintLine(LogFileNum, "<Tr>")
		PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "><b> Название точки </b></Td>")
		PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> Широта </b></Td>")
		PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> Долгота </b></Td>")
		PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "><b> Абс. высота (м) </b></Td>")
		PrintLine(LogFileNum, "</Tr>")

		If (pReport.pPtIAF.sLat <> "") And (pReport.pPtIAF.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> IAF </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtIAF.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtIAF.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtIAF.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtTOB.sLat <> "") And (pReport.pPtIAF.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка начала разворота для выхода на курс, обратный посадочному </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTOB.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTOB.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtTOB.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtSOL.sLat <> "") And (pReport.pPtSOL.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка выхода из разворота на курс, обратный посадочному </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtSOL.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtSOL.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtSOL.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtEOL.sLat <> "") And (pReport.pPtEOL.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка начала разворота для выхода на посадочный курс </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtEOL.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtEOL.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtEOL.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtFarFAF.sLat <> "") And (pReport.pPtFarFAF.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка выхода на посадочный курс </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtFarFAF.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtFarFAF.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtFarFAF.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtSDF.sLat <> "") And (pReport.pPtSDF.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> КТ снижения </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtSDF.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtSDF.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtSDF.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtMAPt.sLat <> "") And (pReport.pPtMAPt.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> MAPt </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtMAPt.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtMAPt.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtMAPt.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtTP.sLat <> "") And (pReport.pPtTP.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка разворота </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTP.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTP.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtTP.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtTOUT.sLat <> "") And (pReport.pPtTOUT.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> Точка выхода из разворота на курс для полета на РНС/FIX/WPT </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTOUT.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtTOUT.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtTOUT.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If

		If (pReport.pPtEnd.sLat <> "") And (pReport.pPtEnd.sLon <> "") Then
			PrintLine(LogFileNum, "<Tr>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "40%" + Chr(34) + "> РНС/FIX/WPT, на который выполняется уход </Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtEnd.sLat + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "25%" + Chr(34) + "><b> " + pReport.pPtEnd.sLon + " </b></Td>")
			PrintLine(LogFileNum, "    <Td Width=" + Chr(34) + "10%" + Chr(34) + "> " + pReport.pPtEnd.sH + " </Td>")
			PrintLine(LogFileNum, "</Tr>")
		End If
		PrintLine(LogFileNum, "</Table><br>")
		'===================================================================================================================
		Exit Function

EH:
		'If Flg Then
		CreateReportFile = -1
		Exit Function
		'Else
		'    Flg = True
		'    Open FileName For Output Access Write Lock Read Write As #LogFileNum
		'    Resume Next
		'End If
	End Function

	Sub SaveReport(ByRef pReport As ReportType)
		Dim FileNum As Integer
		Dim ParaStr As String

		FileNum = CreateReportFile(pReport)
		ParaStr = "<p></p>"

		Select Case pReport.iProcedure
			Case 0

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(0).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(0, FileNum) 'BM

				If (pReport.iType And 1) = 1 Then 'Без FAF
					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(1).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)

					PrintLine(FileNum, "<p>Outbound flight time:  " + pReport.FlyTime + "</p>")
					SaveTabAsHTML(1, FileNum) 'Ippo
				End If

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(2).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p>GEO approach track:  " + pReport.FinalTrack + " ")

				If (pReport.iType And 1) = 0 Then 'С FAF
					PrintLine(FileNum, "Track offset:    " + pReport.TrackOffset + " ")
					PrintLine(FileNum, "Segment Length:  " + pReport.FinalLength + "</p>")
				End If
				SaveTabAsHTML(2, FileNum) 'Final

				If (pReport.iType And 1) = 0 Then 'С FAF
					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(3).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)

					PrintLine(FileNum, "<p>GEO segment track:  " + pReport.FinalTrack + " ")
					PrintLine(FileNum, "Track offset:    " + pReport.TrackOffset + " ")
					PrintLine(FileNum, "Segment Length:  " + pReport.InterLength + "</p>")
					SaveTabAsHTML(3, FileNum) 'Interm
				End If

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(4).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p>Surface gradient:  " + pReport.SurfGrad + "<br>")
				PrintLine(FileNum, "Area Length:  " + pReport.StraightLength + "</p>")

				If pReport.bMAPtVare Then
					PrintLine(FileNum, "<p>MAPt defined by a FIX<br>")
				Else
					PrintLine(FileNum, "<p>MAPt defined by a distance from nominal FAF<br>")
				End If
				PrintLine(FileNum, "MAPt is located " + pReport.MAPt2THR + " before/after THR" + "</p>")

				SaveTabAsHTML(4, FileNum) 'MA Area
				PrintLine(FileNum, "<p>* - Obstacle + Final MOC is above the missed approach surface at the SOC<br>")
				PrintLine(FileNum, "** - Obstacle does not penetrate the extension of the missed approach surface</p>")

				If (pReport.iType And 2) = 2 Then 'С разворотом

					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(5).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)
					SaveTabAsHTML(5, FileNum) 'by-pass

					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(6).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p>Area Length:  " + pReport.TARange + "</p>")
					SaveTabAsHTML(6, FileNum) 'MA Turn Area

					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(7).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)
					SaveTabAsHTML(7, FileNum) 'Turn Area

					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(11).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)
					SaveTabAsHTML(11, FileNum) 'ZNR
				End If
			Case 1
				'    Print #FileNum, ParaStr
				'    Print #FileNum, "<p><b>" + MultiPage1.TabCaption(2) + ":</b></p>"
				'    Print #FileNum, ParaStr
				'    SaveTabAsHTML 2, FileNum  'Final
				'
				'    Print #FileNum, ParaStr
				'    Print #FileNum, "<p><b>" + MultiPage1.TabCaption(3) + ":</b></p>"
				'    Print #FileNum, ParaStr
				'    SaveTabAsHTML 3, FileNum  'Interm
				'
				'    Print #FileNum, ParaStr
				'    Print #FileNum, "<p><b>" + MultiPage1.TabCaption(4) + ":</b></p>"
				'    Print #FileNum, ParaStr
				'    SaveTabAsHTML 4, FileNum  'MA Turn Area
				'
				'    Print #FileNum, ParaStr
				'    Print #FileNum, "<p><b>" + MultiPage1.TabCaption(5) + ":</b></p>"
				'    Print #FileNum, ParaStr
				'    SaveTabAsHTML 5, FileNum  'by-pass
				'
				'    Print #FileNum, ParaStr
				'    Print #FileNum, "<p><b>" + MultiPage1.TabCaption(6) + ":</b></p>"
				'    Print #FileNum, ParaStr
				'    SaveTabAsHTML 6, FileNum  'MA Area

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(7).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(7, FileNum) 'Turn Area

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(8).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(8, FileNum) 'Annex 14

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(9).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(9, FileNum) 'ILS

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(10).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(10, FileNum) 'OAS

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(11).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				SaveTabAsHTML(11, FileNum) 'ZNR
		End Select

		'Print #FileNum, "</BODY>"
		'Print #FileNum, "</HTML>"

		FileClose(FileNum)
	End Sub
#End If

	Private Sub SaveTabAsHTML(ByRef TabNum As Integer, ByRef FileNum As Integer)
		Dim I As Integer
		Dim J As Integer

		Dim ColorStr As String
		Dim ParaStr As String
		Dim FontStr As String
		Dim EndFace As String
		Dim Face As String

		Dim lListView As ListView
		Dim itmsX As ListView.ListViewItemCollection

		Select Case TabNum
			Case 0
				lListView = ListView01
			Case 1
				lListView = ListView02
			Case 2
				lListView = ListView03
			Case 3
				lListView = ListView04
			Case 4
				lListView = ListView05
			Case 5
				lListView = ListView06
			Case 6
				lListView = ListView07
			Case 7
				lListView = ListView08
			Case 8
				lListView = ListView09
		End Select
		ParaStr = "<p></p>"

		itmsX = lListView.Items

		PrintLine(FileNum, "<Table border=1>")
		PrintLine(FileNum, "<Tr>")
		For I = 0 To lListView.Columns.Count - 1
			PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + "><b>" + CStr(lListView.Columns.Item(I).Text) + "</b></Td>")
		Next
		PrintLine(FileNum, "</Tr>")

		For I = 0 To itmsX.Count - 1
			'    Print #FileNum,"<Tr>"
			If itmsX.Item(I).ForeColor.ToArgb() = 0 Then
				ColorStr = "000000"
			ElseIf itmsX.Item(I).ForeColor.ToArgb() = 255 Then
				ColorStr = "FF0000"
			ElseIf itmsX.Item(I).ForeColor.ToArgb() = RGB(0, 0, 255) Then
				ColorStr = "0000FF"
			Else
				ColorStr = CStr(Not System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor))
			End If

			If itmsX.Item(I).Font.Bold Then
				Face = "<b>"
				EndFace = "</b>"
			Else
				Face = ""
				EndFace = ""
			End If
			FontStr = "<Font Color=" + Chr(34) + ColorStr + Chr(34) + ">" + Face

			PrintLine(FileNum, "<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + itmsX.Item(I).Text + EndFace + "</Td>")
			For J = 1 To lListView.Columns.Count - 1
				PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + itmsX.Item(I).SubItems(J).Text + EndFace + "</Td>")
			Next

			PrintLine(FileNum, "</Tr>")
		Next

		PrintLine(FileNum, "</Table>")
		PrintLine(FileNum, ParaStr)
	End Sub

	Private Sub SaveTab(ByRef TabNum As Integer, ByRef FileNum As Integer)
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim TmpLen As Integer
		Dim maxLen As Integer
		Dim HeadersLen() As Integer
		Dim StrOut As String
		Dim HeadersText() As String

		Dim lListView As ListView
		Dim tmpStr As String
		Dim hdrX As ColumnHeader
		Dim itmX As ListViewItem

		Select Case TabNum
			Case 0
				lListView = ListView01
			Case 1
				lListView = ListView02
			Case 2
				lListView = ListView03
			Case 3
				lListView = ListView04
			Case 4
				lListView = ListView05
			Case 5
				lListView = ListView06
			Case 6
				lListView = ListView07
			Case 7
				lListView = ListView08
			Case 8
				lListView = ListView09
		End Select

		PrintLine(FileNum, Chr(9) + MultiPage1.TabPages.Item(TabNum).Text)
		PrintLine(FileNum)

		N = lListView.Columns.Count
		ReDim HeadersText(N - 1)
		ReDim HeadersLen(N - 1)

		maxLen = 0
		For I = 0 To N - 1
			hdrX = lListView.Columns.Item(I)
			HeadersText(I) = """" + hdrX.Text + """"
			HeadersLen(I) = Len(HeadersText(I))
			If HeadersLen(I) > maxLen Then
				maxLen = HeadersLen(I)
			End If
		Next I

		StrOut = ""
		For I = 0 To N - 1
			J = maxLen - HeadersLen(I)
			M = J \ 2
			If I < N Then
				tmpStr = Space(M) + "|"
			Else
				tmpStr = ""
			End If
			StrOut = StrOut + Space(J - M) + HeadersText(I) + tmpStr
		Next I

		PrintLine(FileNum, StrOut)
		StrOut = ""
		tmpStr = New String("-", maxLen) + "+"
		For I = 1 To N - 1
			StrOut = StrOut + tmpStr
		Next I
		StrOut = StrOut + New String("-", maxLen)

		PrintLine(FileNum, StrOut)

		M = lListView.Items.Count
		For I = 0 To M - 1
			itmX = lListView.Items.Item(I)
			TmpLen = Len(itmX.Text)
			If TmpLen > maxLen Then
				StrOut = Microsoft.VisualBasic.Left(itmX.Text, maxLen - 1) + "*"
			Else
				StrOut = Space(maxLen - TmpLen) + itmX.Text
			End If

			For J = 1 To N - 1
				tmpStr = itmX.SubItems(J).Text

				TmpLen = Len(tmpStr)

				If TmpLen > maxLen Then
					tmpStr = Microsoft.VisualBasic.Left(tmpStr, maxLen - 1) + "*"
				Else
					If J < N - 1 Or TmpLen > 0 Then
						tmpStr = Space(maxLen - TmpLen) + tmpStr
					End If
				End If

				StrOut = StrOut + "|" + tmpStr
			Next J
			PrintLine(FileNum, StrOut)
		Next I

		PrintLine(FileNum)
	End Sub

	Public Sub SaveReportTab(ByRef FileName As String, ByRef TabNum As Integer)
		Dim FileNum As Integer
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim TmpLen As Integer
		Dim maxLen As Integer
		Dim HeadersLen() As Integer
		Dim lListView As ListView

		Select Case TabNum
			Case 0
				lListView = ListView01
			Case 1
				lListView = ListView02
			Case 2
				lListView = ListView03
			Case 3
				lListView = ListView04
			Case 4
				lListView = ListView05
			Case 5
				lListView = ListView06
			Case 6
				lListView = ListView07
			Case 7
				lListView = ListView08
			Case 8
				lListView = ListView09
		End Select

		Dim StrOut As String
		Dim HeadersText() As String
		Dim tmpStr As String
		Dim hdrX As ColumnHeader
		Dim itmX As ListViewItem

		If True Then
			FileNum = FreeFile()
			FileOpen(FileNum, FileName, OpenMode.Output)

			PrintLine(FileNum, Text)
			PrintLine(FileNum, Chr(9) + MultiPage1.TabPages.Item(TabNum).Text)
			PrintLine(FileNum)

			N = lListView.Columns.Count
			ReDim HeadersText(N - 1)
			ReDim HeadersLen(N - 1)

			maxLen = 0
			For I = 0 To N - 1
				hdrX = lListView.Columns.Item(I)
				HeadersText(I) = """" + hdrX.Text + """"
				HeadersLen(I) = Len(HeadersText(I))
				If HeadersLen(I) > maxLen Then
					maxLen = HeadersLen(I)
				End If
			Next I

			StrOut = ""
			For I = 0 To N - 1
				J = maxLen - HeadersLen(I)
				M = J \ 2
				If I < N Then
					tmpStr = Space(M) + "|"
				Else
					tmpStr = ""
				End If
				StrOut = StrOut + Space(J - M) + HeadersText(I) + tmpStr
			Next I

			PrintLine(FileNum, StrOut)
			StrOut = ""
			tmpStr = New String("-", maxLen) + "+"
			For I = 1 To N - 1
				StrOut = StrOut + tmpStr
			Next I
			StrOut = StrOut + New String("-", maxLen)

			PrintLine(FileNum, StrOut)

			M = lListView.Items.Count
			For I = 0 To M - 1
				itmX = lListView.Items.Item(I)
				TmpLen = Len(itmX.Text)
				If TmpLen > maxLen Then
					StrOut = Microsoft.VisualBasic.Left(itmX.Text, maxLen - 1) + "*"
				Else
					StrOut = Space(maxLen - TmpLen) + itmX.Text
				End If

				For J = 1 To N - 1
					tmpStr = itmX.SubItems(J).Text

					TmpLen = Len(tmpStr)

					If TmpLen > maxLen Then
						tmpStr = Microsoft.VisualBasic.Left(tmpStr, maxLen - 1) + "*"
					Else
						If J < N - 1 Or TmpLen > 0 Then
							tmpStr = Space(maxLen - TmpLen) + tmpStr
						End If
					End If

					StrOut = StrOut + "|" + tmpStr
				Next J
				PrintLine(FileNum, StrOut)
			Next I
			FileClose(FileNum)
		End If
	End Sub

	Private Sub SaveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim L As Integer
		Dim pos As Integer
		Dim FileNum As Integer
		Dim sExt As String
		Dim bHtml As Boolean

		If SaveDlg.ShowDialog() <> DialogResult.OK Then Return
		L = Len(SaveDlg.FileName)
		pos = InStrRev(SaveDlg.FileName, ".")
		If pos <> 0 Then
			sExt = Microsoft.VisualBasic.Right(SaveDlg.FileName, L - pos)
			bHtml = (UCase(sExt) = "HTM") Or (UCase(sExt) = "HTML")
		Else
			bHtml = SaveDlg.FilterIndex > 1
		End If

		FileNum = FreeFile()
		FileOpen(FileNum, SaveDlg.FileName, OpenMode.Output)
		If bHtml Then
			SaveTabAsHTML((MultiPage1.SelectedIndex), FileNum)
		Else
			SaveTab((MultiPage1.SelectedIndex), FileNum)
		End If

		FileClose(FileNum)
	End Sub

	Sub SetBtn(ByRef Btn As CheckBox)
		ReportBtn = Btn
	End Sub

	'Sub SetVisible(ByVal I As Long)
	'Dim J As Long
	'    If i < 0 Then
	'        For J = 0 To MultiPage1.Tabs - 1
	'            MultiPage1.TabVisible(J) = True
	'        Next J
	'    ElseIf i < MultiPage1.Tabs Then
	'        MultiPage1.TabVisible(i) = True
	'    End If
	'End Sub

	'Sub SetUnVisible(ByVal I As Long)
	'Dim J As Long
	'    If (i < 0) Then
	'        For J = 0 To MultiPage1.Tabs - 1
	'            MultiPage1.TabVisible(J) = False
	'        Next J
	'    ElseIf (i < MultiPage1.Tabs) Then
	'        MultiPage1.TabVisible(i) = False
	'    End If
	'End Sub

	Private Sub CPrecReportFrm_KeyUp(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
	End Sub
End Class
