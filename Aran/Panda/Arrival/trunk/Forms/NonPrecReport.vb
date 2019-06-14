Option Strict Off
Option Explicit On
Imports System.Collections.Generic


<System.Runtime.InteropServices.ComVisible(False)> Friend Class CNonPrecReportFrm
	Inherits Form

#Region "Variable declarations"

	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private pGeomElem As ESRI.ArcGIS.Carto.IElement

	Private LObstaclesPage01() As ObstacleMSA
	Private SortF01 As Integer
	Private Ix01ID As Long
	Private MOC01 As Double

	Private LObstaclesPage02 As ObstacleContainer
	Private SortF02 As Integer
	Private Ix02ID As Long

	Private LObstaclesPage03 As ObstacleContainer
	Private SortF03 As Integer
	Private Ix03ID As Long
	Private bFAF03 As Boolean
	Private MaxReqH03 As Double

	Private LObstaclesPage04 As ObstacleContainer
	Private SortF04 As Integer

	Private LObstaclesPage05 As ObstacleContainer
	Private SortF05 As Integer
	Private Ix05ID As Long
	Private FinalMOC05 As Double
	'Private maPDG3 As Double

	Private LObstaclesPage06 As ObstacleContainer
	Private SortF06 As Integer
	'Private Ix06ID As Long
	Private PrecFlg06 As Integer
	Private UseILSPlanes06 As Boolean

	Private LObstaclesPage07 As ObstacleContainer
	Private SortF07 As Integer
	Private Ix07ID As Long
	Private FinalMOC07 As Double
	'Private maPDG14 As Double

	Private LObstaclesPage08 As ObstacleContainer
	Private SortF08 As Integer
	Private Ix08ID As Long

	Private HelpContextID As Integer = 9800
	Private ReportBtn As CheckBox

	Private lvwColumnSorter1 As ListViewColumnSorter
	Private lvwColumnSorter2 As ListViewColumnSorter
	Private lvwColumnSorter3 As ListViewColumnSorter
	Private lvwColumnSorter4 As ListViewColumnSorter
	Private lvwColumnSorter5 As ListViewColumnSorter
	Private lvwColumnSorter6 As ListViewColumnSorter
	Private lvwColumnSorter7 As ListViewColumnSorter
	Private lvwColumnSorter8 As ListViewColumnSorter
	'Private bFormInitialised As Boolean = False
#End Region

	Public Sub New()
		MyBase.New()
		' This call is required by the Windows Form Designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		ReDim LObstaclesPage01(-1)
		ReDim LObstaclesPage02.Obstacles(-1)
		ReDim LObstaclesPage03.Obstacles(-1)
		ReDim LObstaclesPage04.Obstacles(-1)
		ReDim LObstaclesPage05.Obstacles(-1)
		ReDim LObstaclesPage06.Obstacles(-1)
		ReDim LObstaclesPage07.Obstacles(-1)
		ReDim LObstaclesPage08.Obstacles(-1)

		ReDim LObstaclesPage02.Parts(-1)
		ReDim LObstaclesPage03.Parts(-1)
		ReDim LObstaclesPage04.Parts(-1)
		ReDim LObstaclesPage05.Parts(-1)
		ReDim LObstaclesPage06.Parts(-1)
		ReDim LObstaclesPage07.Parts(-1)
		ReDim LObstaclesPage08.Parts(-1)

		SortF01 = 0
		SortF02 = 0
		SortF03 = 0
		SortF04 = 0
		SortF05 = 0
		SortF06 = 0
		SortF07 = 0
		SortF08 = 0

		SaveBtn.Text = My.Resources.str30002
		CloseBtn.Text = My.Resources.str30001
		Me.Text = My.Resources.str30000

		'SetFormParented(Handle.ToInt32)
		'    ListView3.ToolTipText = "*-Obstacle + Final MOC is above the missed approach surface at the SOC" + Chr(9) + "**-Obstacle does not penetrate the extension of the missed approach surface"

		MultiPage1.TabPages.Item(0).Text = My.Resources.str30003
		MultiPage1.TabPages.Item(1).Text = My.Resources.str30004
		MultiPage1.TabPages.Item(2).Text = My.Resources.str30005
		MultiPage1.TabPages.Item(3).Text = My.Resources.str30006
		MultiPage1.TabPages.Item(4).Text = My.Resources.str30007
		MultiPage1.TabPages.Item(5).Text = My.Resources.str30008
		MultiPage1.TabPages.Item(6).Text = My.Resources.str30009
		MultiPage1.TabPages.Item(7).Text = My.Resources.str30010
		MultiPage1.SelectedIndex = 0

		ListView01.Columns.Item(0).Text = "Type"   'type
		ListView01.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView01.Columns.Item(2).Text = My.Resources.str30035 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'h abv arp
		ListView01.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'moc
		ListView01.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'req h

		ListView02.Columns.Item(0).Text = "Type"   'type
		ListView02.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView02.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'height
		ListView02.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'moc
		ListView02.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'req h
		ListView02.Columns.Item(5).Text = My.Resources.str30024 'area

		ListView03.Columns.Item(0).Text = "Type"   'type
		ListView03.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView03.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'height
		ListView03.Columns.Item(3).Text = My.Resources.str30024 'area
		ListView03.Columns.Item(4).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'moc
		ListView03.Columns.Item(5).Text = My.Resources.str30044 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'LoadResString(30037) 'req OCH
		ListView03.Columns.Item(6).Text = ListView03.Columns.Item(6).Text + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'X
		ListView03.Columns.Item(7).Text = ListView03.Columns.Item(7).Text + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Y


		ListView04.Columns.Item(0).Text = "Type"   'type
		ListView04.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView04.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'height
		ListView04.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'moc
		ListView04.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'req h
		ListView04.Columns.Item(5).Text = My.Resources.str30024 'area
		ListView04.Columns.Item(6).Text = My.Resources.str30043 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'X from FAF
		ListView04.Columns.Item(7).Text += " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")"

		ListView05.Columns.Item(0).Text = "Type"   'type
		ListView05.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView05.Columns.Item(2).Text = My.Resources.str30013 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'dist from soc
		ListView05.Columns.Item(3).Text = My.Resources.str30014 'phase
		ListView05.Columns.Item(4).Text = My.Resources.str30015 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'Obs.H abv. ref.pt.
		ListView05.Columns.Item(5).Text = My.Resources.str30016 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'moc30
		ListView05.Columns.Item(6).Text = My.Resources.str30017 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. OCH
		ListView05.Columns.Item(7).Text = My.Resources.str30018 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MA Surface H
		ListView05.Columns.Item(8).Text = My.Resources.str30019 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MA Surface penet.
		ListView05.Columns.Item(9).Text = My.Resources.str30020 'Ignored
		ListView05.Columns.Item(10).Text = My.Resources.str30021 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Final Req. H
		ListView05.Columns.Item(11).Text = My.Resources.str30022 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC50
		ListView05.Columns.Item(12).Text = My.Resources.str30023 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. H50
		ListView05.Columns.Item(13).Text = My.Resources.str30024    'Area

		ListView06.Columns.Item(0).Text = "Type"   'type
		ListView06.Columns.Item(1).Text = My.Resources.str30011 'Name
		ListView06.Columns.Item(2).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Height
		ListView06.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC
		ListView06.Columns.Item(4).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req.H
		ListView06.Columns.Item(5).Text = My.Resources.str30044 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req OCH
		ListView06.Columns.Item(6).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'hPnet
		ListView06.Columns.Item(7).Text = "X (" + HeightConverter(0).Unit + ")" 'LoadResString() 'X
		ListView06.Columns.Item(8).Text = My.Resources.str30046 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Avoid Dist
		ListView06.Columns.Item(9).Text = My.Resources.str30047 + " (°)" 'Avoid angle
		ListView06.Columns.Item(10).Text = My.Resources.str30024    'Area

		ListView07.Columns.Item(0).Text = "Type"   'type
		ListView07.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView07.Columns.Item(2).Text = My.Resources.str30013 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'dist from soc
		ListView07.Columns.Item(3).Text = My.Resources.str30014 'phase
		ListView07.Columns.Item(4).Text = My.Resources.str30015 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Obs.H abv. ref.pt.
		ListView07.Columns.Item(5).Text = My.Resources.str30016 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'moc30
		ListView07.Columns.Item(6).Text = My.Resources.str30017 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. H30
		ListView07.Columns.Item(7).Text = My.Resources.str30018 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MA Surface H
		ListView07.Columns.Item(8).Text = My.Resources.str30019 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MA Surface penet.
		ListView07.Columns.Item(9).Text = My.Resources.str30020 'Ignored
		ListView07.Columns.Item(10).Text = My.Resources.str30021 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Final Req. H
		ListView07.Columns.Item(11).Text = My.Resources.str30022 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC50
		ListView07.Columns.Item(12).Text = My.Resources.str30023 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. H50
		ListView07.Columns.Item(13).Text = My.Resources.str30024 'Area

		ListView08.Columns.Item(0).Text = "Type"   'Type
		ListView08.Columns.Item(1).Text = My.Resources.str30011 'Name
		ListView08.Columns.Item(2).Text = My.Resources.str40035 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Dist
		ListView08.Columns.Item(3).Text = My.Resources.str40031 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Height
		ListView08.Columns.Item(4).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC
		ListView08.Columns.Item(5).Text = My.Resources.str30037 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req.H
		ListView08.Columns.Item(6).Text = My.Resources.str30038 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'TA Surface H
		ListView08.Columns.Item(7).Text = My.Resources.str30039 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'hPnet
		ListView08.Columns.Item(8).Text = My.Resources.str30024 'Area

		lvwColumnSorter1 = New ListViewColumnSorter()
		lvwColumnSorter2 = New ListViewColumnSorter()
		lvwColumnSorter3 = New ListViewColumnSorter()
		lvwColumnSorter4 = New ListViewColumnSorter()
		lvwColumnSorter5 = New ListViewColumnSorter()
		lvwColumnSorter6 = New ListViewColumnSorter()
		lvwColumnSorter7 = New ListViewColumnSorter()
		lvwColumnSorter8 = New ListViewColumnSorter()

		ListView01.ListViewItemSorter = lvwColumnSorter1
		ListView02.ListViewItemSorter = lvwColumnSorter2
		ListView03.ListViewItemSorter = lvwColumnSorter3
		ListView04.ListViewItemSorter = lvwColumnSorter4
		ListView05.ListViewItemSorter = lvwColumnSorter5
		ListView06.ListViewItemSorter = lvwColumnSorter6
		ListView07.ListViewItemSorter = lvwColumnSorter7
		ListView08.ListViewItemSorter = lvwColumnSorter8

		ListView01.View = View.Details
		ListView02.View = View.Details
		ListView03.View = View.Details
		ListView04.View = View.Details
		ListView05.View = View.Details
		ListView06.View = View.Details
		ListView07.View = View.Details
		ListView08.View = View.Details
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
		UpdateListView01(CurrIndex, True)
		'ListView01_SelectedIndexChanged(ListView01, New System.EventArgs)

		'Page 2 ========================================================================================

		PrevIndex = System.Math.Abs(SortF02) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView02.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF02 = CurrIndex + 1
		UpdateListView02(CurrIndex, True)
		'If ReportBtn.Checked Then ListView02_SelectedIndexChanged(ListView02, New System.EventArgs)

		'Page 3 ========================================================================================
		CurrIndex = 5
		PrevIndex = System.Math.Abs(SortF03) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView03.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF03 = CurrIndex + 1
		UpdateListView03(CurrIndex, True)
		'If ReportBtn.Checked Then ListView03_SelectedIndexChanged(ListView03, New System.EventArgs)

		'Page 4 ========================================================================================
		CurrIndex = 4
		PrevIndex = System.Math.Abs(SortF04) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView04.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF04 = CurrIndex + 1
		UpdateListView04(CurrIndex, True)
		'ListView04_SelectedIndexChanged(ListView04, New System.EventArgs)

		'Page 5 ========================================================================================
		CurrIndex = 6
		PrevIndex = System.Math.Abs(SortF05) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView05.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF05 = CurrIndex + 1
		UpdateListView05(CurrIndex, True)
		'ListView05_SelectedIndexChanged(ListView05, New System.EventArgs)
		'Page 6 ========================================================================================
		CurrIndex = 8
		PrevIndex = System.Math.Abs(SortF06) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView06.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF06 = -CurrIndex - 1
		UpdateListView06(CurrIndex, True)
		''ListView06_SelectedIndexChanged(ListView06, New System.EventArgs)
		'Page 7 ========================================================================================
		CurrIndex = 6
		PrevIndex = System.Math.Abs(SortF07) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView07.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF07 = CurrIndex + 1
		UpdateListView07(CurrIndex, True)
		'ListView07_SelectedIndexChanged(ListView07, New System.EventArgs)
		'Page 8 ========================================================================================
		CurrIndex = 7
		PrevIndex = System.Math.Abs(SortF08) - 1
		If CurrIndex <> PrevIndex Then
			If PrevIndex >= 0 Then ListView08.Columns.Item(PrevIndex).ImageIndex = 2
		End If

		SortF08 = CurrIndex + 1
		UpdateListView08(CurrIndex, True)
		'ListView08_SelectedIndexChanged(ListView08, New System.EventArgs)
	End Sub

	Private Sub UpdateListView01(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage01)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView01.Columns(sortByField)
			If System.Math.Abs(SortF01) - 1 = sortByField Then SortF01 = -SortF01

			If preClear Or (System.Math.Abs(SortF01) - 1 <> sortByField) Then
				If System.Math.Abs(SortF01) - 1 <> sortByField Then
					If SortF01 <> 0 Then ListView01.Columns.Item(System.Math.Abs(SortF01) - 1).ImageIndex = 2
					SortF01 = -sortByField - 1
				End If

				If (sortByField >= 2) And (sortByField < 5) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage01(I).fSort = LObstaclesPage01(I).Height
							Case 3
								Return
							Case 4
								LObstaclesPage01(I).fSort = LObstaclesPage01(I).ReqH
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage01(I).sSort = LObstaclesPage01(I).TypeName
							Case 1
								LObstaclesPage01(I).sSort = LObstaclesPage01(I).UnicalName
						End Select
					Next I
				End If
			End If

			If SortF01 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField < 5) Then
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
		If preClear Then ListView01.Items.Clear()

		For I = 0 To N
			If preClear Then
				itmX = ListView01.Items.Add(LObstaclesPage01(I).TypeName)
				itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage01(I).UnicalName))
				itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage01(I).Height, eRoundMode.NEAREST))))
				itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(MOC01, eRoundMode.NEAREST))))
				itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage01(I).ReqH, eRoundMode.NEAREST))))
			Else
				itmX = ListView01.Items.Item(I)
				itmX.Text = LObstaclesPage01(I).TypeName
				itmX.SubItems(1).Text = LObstaclesPage01(I).UnicalName
				itmX.SubItems(2).Text = CStr(ConvertReportHeight(LObstaclesPage01(I).Height, eRoundMode.NEAREST))
				itmX.SubItems(3).Text = CStr(ConvertReportHeight(MOC01, eRoundMode.NEAREST))
				itmX.SubItems(4).Text = CStr(ConvertReportHeight(LObstaclesPage01(I).ReqH, eRoundMode.NEAREST))
			End If

			itmX.Tag = I

			If LObstaclesPage01(I).ID = Ix01ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.Red
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.Black
			End If

			For K = 1 To 4
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K

			itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next I

		ListView01.EndUpdate()
	End Sub

	Private Sub UpdateListView02(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage02.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView02.Columns(sortByField)
			If System.Math.Abs(SortF02) - 1 = sortByField Then SortF02 = -SortF02

			If preClear Or (System.Math.Abs(SortF02) - 1 <> sortByField) Then
				If System.Math.Abs(SortF02) - 1 <> sortByField Then
					If SortF02 <> 0 Then ListView02.Columns.Item(System.Math.Abs(SortF02) - 1).ImageIndex = 2
					SortF02 = -sortByField - 1
				End If

				If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 11) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Height
							Case 3
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).MOC
							Case 4
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).ReqH
							Case 6
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Dist
							Case 7
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Rmin
							Case 8
								LObstaclesPage02.Parts(I).fSort = LObstaclesPage02.Parts(I).Rmax
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage02.Parts(I).sSort = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).TypeName
							Case 1
								LObstaclesPage02.Parts(I).sSort = LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).UnicalName
							Case 5
								If (LObstaclesPage02.Parts(I).Flags And 1) = 1 Then
									LObstaclesPage02.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage02.Parts(I).sSort = "Secondary"
								End If
						End Select
					Next I
				End If
			End If

			If SortF02 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 11) Then
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
		If preClear Then ListView02.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage02)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value

			itmX = ListView02.Items.Add(LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).UnicalName))

			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage02.Parts(I).ReqH, eRoundMode.NEAREST))))

			If (LObstaclesPage02.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If LObstaclesPage02.Obstacles(LObstaclesPage02.Parts(I).Owner).ID = Ix02ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For K = 1 To 5 '11
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K

			itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
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

			If preClear Or (System.Math.Abs(SortF03) - 1 <> sortByField) Then
				If System.Math.Abs(SortF03) - 1 <> sortByField Then
					If SortF03 <> 0 Then ListView03.Columns.Item(System.Math.Abs(SortF03) - 1).ImageIndex = 2
					SortF03 = -sortByField - 1
				End If

				If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 7) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).Height
							Case 4
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).MOC
							Case 5
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).ReqH
							Case 6
								LObstaclesPage03.Parts(I).fSort = LObstaclesPage03.Parts(I).Dist
							Case 7
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
							Case 3
								If (LObstaclesPage03.Parts(I).Flags And 1) = 1 Then
									LObstaclesPage03.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage03.Parts(I).sSort = "Secondary"
								End If
							Case 8
								If (LObstaclesPage03.Parts(I).Flags And 2) = 2 Then
									LObstaclesPage03.Parts(I).sSort = "Yes"
								Else
									LObstaclesPage03.Parts(I).sSort = "No"
								End If
						End Select
					Next I
				End If
			End If

			If SortF03 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 7) Then
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
		If preClear Then ListView03.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage03)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView03.Items.Add(LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).UnicalName))

			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).Height, eRoundMode.NEAREST))))

			If (LObstaclesPage03.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage03.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage03.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage03.Parts(I).DistStar, eRoundMode.NEAREST))))

			If bFAF03 Then
				If (LObstaclesPage03.Parts(I).Flags And 2) = 2 Then
					itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Yes"))
					If MaxReqH03 < LObstaclesPage03.Parts(I).ReqH Then
						itmX.ForeColor = Color.Red
						For K = 1 To 8
							itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
						Next K
					End If
				Else
					itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "No"))
				End If
			End If

			If LObstaclesPage03.Obstacles(LObstaclesPage03.Parts(I).Owner).ID = Ix03ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For K = 1 To 7
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K

			If bFAF03 Then
				itmX.SubItems.Item(8).Font = itmX.Font
				itmX.SubItems.Item(8).ForeColor = itmX.ForeColor
			End If
			itmX.SubItems.Item(5).Font = New Font(itmX.SubItems.Item(5).Font, FontStyle.Bold)
		Next

		ListView03.EndUpdate()
	End Sub

	Private Sub UpdateListView04(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage04.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView04.Columns(sortByField)

			If System.Math.Abs(SortF04) - 1 = sortByField Then SortF04 = -SortF04

			If preClear Or (System.Math.Abs(SortF04) - 1 <> sortByField) Then
				If System.Math.Abs(SortF04) - 1 <> sortByField Then
					If SortF04 <> 0 Then ListView04.Columns.Item(System.Math.Abs(SortF04) - 1).ImageIndex = 2
					SortF04 = -sortByField - 1
				End If

				If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 7) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).Height
							Case 3
								LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).MOC
							Case 4
								LObstaclesPage04.Parts(I).fSort = LObstaclesPage04.Parts(I).ReqH
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
							Case 5
								If LObstaclesPage04.Parts(I).Flags = 0 Then
									LObstaclesPage04.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage04.Parts(I).sSort = "Secondary"
								End If
						End Select
					Next I
				End If
			End If

			If SortF04 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 5) And (sortByField >= 2) And (sortByField <= 7) Then
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

		ListView04.BeginUpdate()
		If preClear Then ListView04.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage04)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView04.Items.Add(LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, CStr(LObstaclesPage04.Obstacles(LObstaclesPage04.Parts(I).Owner).UnicalName)))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage04.Parts(I).ReqH, eRoundMode.NEAREST))))

			If LObstaclesPage04.Parts(I).Flags <> 0 Then
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage04.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage04.Parts(I).DistStar, eRoundMode.NEAREST))))

			itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next

		ListView04.EndUpdate()
	End Sub

	Private Sub UpdateListView05(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim itmX As ListViewItem
		Dim aStr() As String = {"Initial", "Itermed", "Final"}

		N = UBound(LObstaclesPage05.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView05.Columns(sortByField)

			If System.Math.Abs(SortF05) - 1 = sortByField Then SortF05 = -SortF05

			If preClear Or (System.Math.Abs(SortF05) - 1 <> sortByField) Then
				If System.Math.Abs(SortF05) - 1 <> sortByField Then
					If SortF05 <> 0 Then ListView05.Columns.Item(System.Math.Abs(SortF05) - 1).ImageIndex = 2
					SortF05 = -sortByField - 1
				End If

				If (sortByField <> 3) And (sortByField <> 9) And (sortByField >= 2) And (sortByField <= 12) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Dist
							Case 4
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Height
							Case 5
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).MOC
							Case 6
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).EffectiveHeight
							Case 7
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).hPenet
							Case 8
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).ReqH - LObstaclesPage05.Parts(I).hPenet
							Case 10
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Height + FinalMOC05 * LObstaclesPage05.Parts(I).fTmp
							Case 11
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Rmin
							Case 12
								LObstaclesPage05.Parts(I).fSort = LObstaclesPage05.Parts(I).Rmax
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage05.Parts(I).sSort = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).TypeName
							Case 1
								LObstaclesPage05.Parts(I).sSort = LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).UnicalName
							Case 3
								LObstaclesPage05.Parts(I).sSort = aStr(CShort(0.5 * (LObstaclesPage05.Parts(I).Flags And 6)))        '0= Initial; 1=Itermed; 2=Final
							Case 10
								If (LObstaclesPage05.Parts(I).Flags And 8) <> 0 Then
									LObstaclesPage05.Parts(I).sSort = "Ignored"
								Else
									LObstaclesPage05.Parts(I).sSort = ""
								End If
							Case 14
								If (LObstaclesPage05.Parts(I).Flags And 1) = 1 Then
									LObstaclesPage05.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage05.Parts(I).sSort = "Secondary"
								End If
						End Select
					Next I
				End If
			End If

			If SortF05 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 3) And (sortByField <> 9) And (sortByField >= 2) And (sortByField <= 12) Then
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
		If preClear Then ListView05.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage05)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView05.Items.Add(LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage05.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, aStr(CShort(0.5 * (LObstaclesPage05.Parts(I).Flags And 6)))))
			'0= Initial; 2=Itermed; 4=Final
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).MOC, eRoundMode.NEAREST))))

			'        itmX.SubItems(6) = CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.rmNERAEST))
			If LObstaclesPage05.Parts(I).EffectiveHeight < 0.0 Then LObstaclesPage05.Parts(I).EffectiveHeight = 0.0

			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).EffectiveHeight, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).ReqH - LObstaclesPage05.Parts(I).hPenet, eRoundMode.NEAREST))))
			'MA Plane penet

			If (LObstaclesPage05.Parts(I).Flags And 8) <> 0 Then
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "Ignored"))
			Else
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, ""))
			End If

			itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Height + FinalMOC05 * LObstaclesPage05.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(11, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Rmin, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(12, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage05.Parts(I).Rmax, eRoundMode.NEAREST))))
			'ReqH50

			If (LObstaclesPage05.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(13, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(13, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If LObstaclesPage05.Obstacles(LObstaclesPage05.Parts(I).Owner).ID = Ix05ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For K = 1 To 13
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
				itmX.SubItems.Item(K).Font = itmX.Font
			Next K
		Next

		ListView05.EndUpdate()
	End Sub

	Private Sub UpdateListView06(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage06.Parts)

		If (sortByField >= 0) Then
			Dim pColumnHeader As ColumnHeader = ListView06.Columns(sortByField)

			If System.Math.Abs(SortF06) - 1 = sortByField Then SortF06 = -SortF06

			If preClear Or (System.Math.Abs(SortF06) - 1 <> sortByField) Then
				If System.Math.Abs(SortF06) - 1 <> sortByField Then
					If SortF06 <> 0 Then ListView06.Columns.Item(System.Math.Abs(SortF06) - 1).ImageIndex = 2
					SortF06 = sortByField + 1
				End If

				If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 9) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).Height
							Case 4
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).ReqH
							Case 5
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).ReqOCH
							Case 6
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).hPenet
							Case 7
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).Dist
							Case 8
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).TurnDistL
							Case 9
								LObstaclesPage06.Parts(I).fSort = LObstaclesPage06.Parts(I).TurnAngleL
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage06.Parts(I).sSort = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).TypeName
							Case 1
								LObstaclesPage06.Parts(I).sSort = LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).UnicalName
							Case 10
								If PrecFlg06 < 0 Then
									If LObstaclesPage06.Parts(I).Flags <> 0 Then
										LObstaclesPage06.Parts(I).sSort = "Primary"
									Else
										LObstaclesPage06.Parts(I).sSort = "Secondary"
									End If
								Else
									If UseILSPlanes06 Then
										LObstaclesPage06.Parts(I).sSort = ILSPlaneNames(LObstaclesPage06.Parts(I).Plane)
									Else
										LObstaclesPage06.Parts(I).sSort = OASPlaneNames(LObstaclesPage06.Parts(I).Plane)
									End If
								End If
						End Select
					Next I
				End If
			End If

			If SortF06 > 0 Then
				pColumnHeader.ImageIndex = 0
			Else
				pColumnHeader.ImageIndex = 1
			End If

			If (sortByField <> 3) And (sortByField >= 2) And (sortByField <= 9) Then
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
		If preClear Then ListView06.Items.Clear()


		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage06)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView06.Items.Add(LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage06.Obstacles(LObstaclesPage06.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, "-"))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).ReqOCH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage06.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage06.Parts(I).Dist, eRoundMode.NEAREST))))

			If LObstaclesPage06.Parts(I).TurnDistL > 0.0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage06.Parts(I).TurnDistL, eRoundMode.NEAREST))))
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(LObstaclesPage06.Parts(I).TurnAngleL, 1))))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "-"))
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "-"))
			End If

			If PrecFlg06 < 0 Then
				If LObstaclesPage06.Parts(I).Flags <> 0 Then
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
				Else
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
				End If
			Else
				If LObstaclesPage06.Parts(I).Plane = eOAS.NonPrec Then itmX.SubItems(6).Text = "-"

				If UseILSPlanes06 Then
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, ILSPlaneNames(LObstaclesPage06.Parts(I).Plane)))
				Else
					itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, OASPlaneNames(LObstaclesPage06.Parts(I).Plane)))
				End If
			End If
		Next

		ListView06.EndUpdate()
	End Sub

	Private Sub UpdateListView07(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim aStr() As String = {"Initial", "Itermed", "Final"}
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage07.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF07) - 1 = sortByField Then SortF07 = -SortF07

			If preClear Or (System.Math.Abs(SortF07) - 1 <> sortByField) Then
				If System.Math.Abs(SortF07) - 1 <> sortByField Then
					If SortF07 <> 0 Then ListView07.Columns.Item(System.Math.Abs(SortF07) - 1).ImageIndex = 2
					SortF07 = -sortByField - 1
				End If

				If (sortByField <> 3) And (sortByField <> 9) And (sortByField >= 2) And (sortByField <= 12) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Dist
							Case 4
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Height
							Case 5
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).MOC
							Case 6
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).EffectiveHeight
							Case 7
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).hPenet
							Case 8
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).ReqH - LObstaclesPage07.Parts(I).hPenet
							Case 10
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Height + FinalMOC07 * LObstaclesPage07.Parts(I).fTmp
							Case 11
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Rmin
							Case 12
								LObstaclesPage07.Parts(I).fSort = LObstaclesPage07.Parts(I).Rmax
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage07.Parts(I).sSort = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).TypeName
							Case 1
								LObstaclesPage07.Parts(I).sSort = LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).UnicalName
							Case 3
								LObstaclesPage07.Parts(I).sSort = aStr(CShort(0.5 * (LObstaclesPage07.Parts(I).Flags And 6)))     '0= Initial; 1=Itermed; 2=Final
							Case 9
								If (LObstaclesPage07.Parts(I).Flags And 8) <> 0 Then
									LObstaclesPage07.Parts(I).sSort = "Ignored"
								Else
									LObstaclesPage07.Parts(I).sSort = ""
								End If
							Case 13
								If (LObstaclesPage07.Parts(I).Flags And 1) = 1 Then
									LObstaclesPage07.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage07.Parts(I).sSort = "Secondary"
								End If
						End Select
					Next I
				End If
			End If

			If SortF07 > 0 Then
				ListView07.Columns(sortByField).ImageIndex = 0
			Else
				ListView07.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField <> 3) And (sortByField <> 9) And (sortByField >= 2) And (sortByField <= 12) Then
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
		ListView07.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage07)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView07.Items.Add(LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage07.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, aStr(CShort(0.5 * (LObstaclesPage07.Parts(I).Flags And 6)))))
			'0= Initial; 1=Itermed; 2=Final
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).MOC, eRoundMode.NEAREST))))

			'        itmX.SubItems(6) = CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.rmNERAEST))
			If LObstaclesPage07.Parts(I).EffectiveHeight < 0.0 Then LObstaclesPage07.Parts(I).EffectiveHeight = 0.0

			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).EffectiveHeight, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).hPenet, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).ReqH - LObstaclesPage07.Parts(I).hPenet, eRoundMode.NEAREST))))
			'MA Plane penet

			If (LObstaclesPage07.Parts(I).Flags And 8) <> 0 Then
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, "Ignored"))
			Else
				itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, ""))
			End If

			itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Height + FinalMOC07 * LObstaclesPage07.Parts(I).fTmp, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(11, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Rmin, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(12, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage07.Parts(I).Rmax, eRoundMode.NEAREST))))
			'ReqH50

			If (LObstaclesPage07.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(13, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(13, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If


			If LObstaclesPage07.Obstacles(LObstaclesPage07.Parts(I).Owner).ID = Ix07ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For K = 1 To 13
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView07.EndUpdate()
	End Sub

	Private Sub UpdateListView08(Optional sortByField As Integer = -1, Optional preClear As Boolean = False)
		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim itmX As ListViewItem

		N = UBound(LObstaclesPage08.Parts)

		If (sortByField >= 0) Then
			If System.Math.Abs(SortF08) - 1 = sortByField Then SortF08 = -SortF08

			If preClear Or (System.Math.Abs(SortF08) - 1 <> sortByField) Then
				If System.Math.Abs(SortF08) - 1 <> sortByField Then
					If SortF08 <> 0 Then ListView08.Columns.Item(System.Math.Abs(SortF08) - 1).ImageIndex = 2
					SortF08 = -sortByField - 1
				End If

				If (sortByField >= 2) And (sortByField <= 7) Then
					For I = 0 To N
						Select Case sortByField
							Case 2
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).Dist
							Case 3
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).Height
							Case 4
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).MOC
							Case 5
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).ReqH
							Case 6
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).EffectiveHeight
							Case 7
								LObstaclesPage08.Parts(I).fSort = LObstaclesPage08.Parts(I).hPenet
						End Select
					Next I
				Else
					For I = 0 To N
						Select Case sortByField
							Case 0
								LObstaclesPage08.Parts(I).sSort = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).TypeName
							Case 1
								LObstaclesPage08.Parts(I).sSort = LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).UnicalName
							Case 8
								If LObstaclesPage08.Parts(I).Flags = 0 Then
									LObstaclesPage08.Parts(I).sSort = "Primary"
								Else
									LObstaclesPage08.Parts(I).sSort = "Secondary"
								End If
						End Select
					Next I
				End If
			End If

			If SortF08 > 0 Then
				ListView08.Columns(sortByField).ImageIndex = 0
			Else
				ListView08.Columns(sortByField).ImageIndex = 1
			End If

			If (sortByField >= 2) And (sortByField <= 7) Then
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
		ListView08.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage08)
		N = unicalObstacleList.Count

		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			itmX = ListView08.Items.Add(LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).TypeName)
			itmX.Tag = I
			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(LObstaclesPage08.Parts(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).EffectiveHeight, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(LObstaclesPage08.Parts(I).hPenet, eRoundMode.NEAREST))))

			If LObstaclesPage08.Parts(I).Flags = 0 Then
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If LObstaclesPage08.Obstacles(LObstaclesPage08.Parts(I).Owner).ID = Ix08ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For K = 1 To 8
				itmX.SubItems.Item(K).Font = itmX.Font
				itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
			Next K
		Next

		ListView08.EndUpdate()
	End Sub

	Public Sub FillPage01(ByRef Obstacles() As ObstacleMSA, MOC As Double, Ix As Integer)
		Const CurrIndex As Integer = 4

		Dim N As Integer
		Dim PrevIndex As Integer

		MOC01 = MOC

		If Ix >= 0 Then
			Ix01ID = Obstacles(Ix).ID
		Else
			Ix01ID = -1
		End If

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		N = UBound(Obstacles)
		ReDim LObstaclesPage01(N)
		Array.Copy(Obstacles, LObstaclesPage01, N + 1)

		PrevIndex = System.Math.Abs(SortF01) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView01.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF01 = CurrIndex + 1

		UpdateListView01(CurrIndex, True)
		lblCountNumber.Text = ListView01.Items.Count

		MultiPage1.SelectedIndex = 0
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage02(ByRef Obstacles As ObstacleContainer, Optional ByVal Ix As Integer = -1)
		Const CurrIndex As Integer = 4

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		If Ix >= 0 Then
			Ix02ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix02ID = -1
		End If

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)

		ReDim LObstaclesPage02.Obstacles(M)
		ReDim LObstaclesPage02.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage02.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage02.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF02) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView02.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF02 = CurrIndex + 1

		UpdateListView02(CurrIndex, True)
		lblCountNumber.Text = ListView02.Items.Count

		MultiPage1.SelectedIndex = 1
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage03(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1, Optional ByRef bFAF As Boolean = False)
		Const CurrIndex As Integer = 5

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		bFAF03 = bFAF

		If bFAF Then
			If ListView03.Columns.Count = 8 Then ListView03.Columns.Add(My.Resources.str30029, 96, HorizontalAlignment.Right)
		Else
			If ListView03.Columns.Count = 9 Then ListView03.Columns.RemoveAt(8)
		End If

		If Ix >= 0 Then
			MaxReqH03 = Obstacles.Parts(Ix).ReqH
			Ix03ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			MaxReqH03 = 900000.0
			Ix03ID = -1
		End If

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)

		ReDim LObstaclesPage03.Obstacles(M)
		ReDim LObstaclesPage03.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage03.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage03.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF03) - 1
		If (PrevIndex >= 0) And (CurrIndex <> PrevIndex) Then
			ListView03.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF03 = CurrIndex + 1

		UpdateListView03(CurrIndex, True)
		lblCountNumber.Text = ListView03.Items.Count

		MultiPage1.SelectedIndex = 2
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage04(ByRef Obstacles As ObstacleContainer)
		Const CurrIndex As Integer = 4

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		N = UBound(Obstacles.Parts)
		M = UBound(Obstacles.Obstacles)

		ReDim LObstaclesPage04.Obstacles(M)
		ReDim LObstaclesPage04.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage04.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage04.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF04) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView04.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF04 = CurrIndex + 1

		UpdateListView04(CurrIndex, True)
		lblCountNumber.Text = ListView04.Items.Count

		MultiPage1.SelectedIndex = 3
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage05(ByRef Obstacles As ObstacleContainer, ByRef FinalMOC As Double, ByRef MAPDG As Double, ByRef Ix As Integer)
		Const CurrIndex As Integer = 6

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		If Ix >= 0 Then
			Ix05ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix05ID = -1
		End If
		FinalMOC05 = FinalMOC

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		ReDim LObstaclesPage05.Obstacles(M)
		Array.Copy(Obstacles.Obstacles, LObstaclesPage05.Obstacles, M + 1)

		N = UBound(Obstacles.Parts)
		ReDim LObstaclesPage05.Parts(N)
		Array.Copy(Obstacles.Parts, LObstaclesPage05.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF05) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView05.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF05 = CurrIndex + 1

		UpdateListView05(CurrIndex, True)
		lblCountNumber.Text = ListView05.Items.Count

		MultiPage1.SelectedIndex = 4
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage06(ByRef Obstacles As ObstacleContainer, Optional ByRef PrecFlg As Integer = -1, Optional ByRef UseILSPlanes As Boolean = False)
		Const CurrIndex As Integer = 8

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		PrecFlg06 = PrecFlg
		UseILSPlanes06 = UseILSPlanes

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)
		ReDim LObstaclesPage06.Obstacles(M)
		ReDim LObstaclesPage06.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage06.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage06.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF06) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView06.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF06 = -CurrIndex - 1

		UpdateListView06(CurrIndex, True)
		lblCountNumber.Text = ListView06.Items.Count

		MultiPage1.SelectedIndex = 5
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage07(ByRef Obstacles As ObstacleContainer, ByRef FinalMOC As Double, ByRef MAPDG As Double, Optional ByRef Ix As Integer = -1)
		Const CurrIndex As Integer = 6

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		If Ix >= 0 Then
			Ix07ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix07ID = -1
		End If
		FinalMOC07 = FinalMOC

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)
		ReDim LObstaclesPage07.Obstacles(M)
		ReDim LObstaclesPage07.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage07.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage07.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF07) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView07.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF07 = CurrIndex + 1

		UpdateListView07(CurrIndex, True)
		lblCountNumber.Text = ListView07.Items.Count

		MultiPage1.SelectedIndex = 6
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	Public Sub FillPage08(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1)
		Const CurrIndex As Integer = 7

		Dim M As Integer
		Dim N As Integer
		Dim PrevIndex As Integer

		If Ix >= 0 Then
			Ix08ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix08ID = -1
		End If

		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)
		ReDim LObstaclesPage08.Obstacles(M)
		ReDim LObstaclesPage08.Parts(N)

		Array.Copy(Obstacles.Obstacles, LObstaclesPage08.Obstacles, M + 1)
		Array.Copy(Obstacles.Parts, LObstaclesPage08.Parts, N + 1)

		PrevIndex = System.Math.Abs(SortF08) - 1
		If (PrevIndex >= 0) And (PrevIndex <> CurrIndex) Then
			ListView08.Columns.Item(PrevIndex).ImageIndex = 2
		End If
		SortF08 = CurrIndex + 1

		UpdateListView08(CurrIndex, True)
		lblCountNumber.Text = ListView08.Items.Count

		MultiPage1.SelectedIndex = 7
		If ReportBtn.Checked And (Not Me.Visible) Then Show(_Win32Window)
	End Sub

	'Public Function GetPageObstacles(ByVal pageNum As Integer) As List(Of Aran.Aim.Features.VerticalStructure)
	'    Dim list As List(Of Aran.Aim.Features.VerticalStructure) = New List(Of Aran.Aim.Features.VerticalStructure)
	'    Dim obsContainer As ObstacleContainer

	'    Select Case pageNum
	'        Case 1
	'            For i As Integer = 0 To LObstaclesPage01.Length - 1
	'                list.Add(LObstaclesPage01(i).VerticalStructure)
	'            Next i

	'            Return list
	'        Case 2
	'            obsContainer = LObstaclesPage02
	'        Case 3
	'            obsContainer = LObstaclesPage03
	'        Case 4
	'            obsContainer = LObstaclesPage04
	'        Case 5
	'            obsContainer = LObstaclesPage05
	'        Case 6
	'            obsContainer = LObstaclesPage06
	'        Case 7
	'            obsContainer = LObstaclesPage07
	'        Case 8
	'            obsContainer = LObstaclesPage08
	'    End Select

	'    For i As Integer = 0 To obsContainer.Obstacles.Length - 1
	'        list.Add(obsContainer.Obstacles(i).VerticalStructure)
	'    Next

	'    Return list
	'End Function

	Private Sub ListView01_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView01.ColumnClick
		UpdateListView01(eventArgs.Column)
		ListView01_SelectedIndexChanged(ListView01, New System.EventArgs)
	End Sub

	Private Sub ListView02_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView02.ColumnClick
		'UpdateListView02(eventArgs.Column)
		' If ReportBtn.Checked Then ListView02_SelectedIndexChanged(ListView02, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter2, ListView02)
	End Sub

	Private Sub ListView03_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView03.ColumnClick
		'UpdateListView03(eventArgs.Column)
		'If ReportBtn.Checked Then ListView03_SelectedIndexChanged(ListView03, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter3, ListView03)
	End Sub

	Private Sub ListView04_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView04.ColumnClick
		' UpdateListView04(eventArgs.Column)
		'ListView04_SelectedIndexChanged(ListView04, New System.EventArgs)
		SortListView(eventArgs.Column, lvwColumnSorter4, ListView04)
	End Sub

	Private Sub ListView05_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView05.ColumnClick
		'UpdateListView05(eventArgs.Column)
		'ListView05_SelectedIndexChanged(ListView05, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter5, ListView05)
	End Sub
	Private Sub ListView06_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView06.ColumnClick
		'UpdateListView06(eventArgs.Column)
		'ListView06_SelectedIndexChanged(ListView06, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter6, ListView06)
	End Sub

	Private Sub ListView07_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView07.ColumnClick
		'UpdateListView07(eventArgs.Column)
		'ListView07_SelectedIndexChanged(ListView07, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter7, ListView07)
	End Sub

	Private Sub ListView08_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView08.ColumnClick
		'UpdateListView08(eventArgs.Column)
		'ListView08_SelectedIndexChanged(ListView08, New System.EventArgs)

		SortListView(eventArgs.Column, lvwColumnSorter8, ListView08)
	End Sub

	Private Sub ListView01_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView01.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = ListView01.SelectedItems.Item(0)
		If (UBound(LObstaclesPage01) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		pGeometry = LObstaclesPage01(Item.Tag).pGeomPrj

		Dim pPtTmp As ArcGIS.Geometry.IPoint
		Dim pCurve As ArcGIS.Geometry.ICurve
		Dim pArea As ArcGIS.Geometry.IArea

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
			pPtTmp = pGeometry
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pCurve = pGeometry
			pPtTmp = New ArcGIS.Geometry.Point()
			pCurve.QueryPoint(ArcGIS.Geometry.esriSegmentExtension.esriNoExtension, 0.5, True, pPtTmp)
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		Else 'If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pArea = pGeometry
			pPtTmp = New ArcGIS.Geometry.Point()
			pArea.QueryLabelPoint(pPtTmp)
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage01(Item.Tag).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView02_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView02.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage02.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage02.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage02.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage02.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage02.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView03_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView03.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage03.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage03.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint

		pGeometry = LObstaclesPage03.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage03.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage03.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView04_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView04.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage04.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage04.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage04.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage04.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage04.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView05_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView05.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage05.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage05.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage05.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage05.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage05.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView06_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView06.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage06.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage06.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint

		pGeometry = LObstaclesPage06.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage06.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage06.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView07_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView07.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage07.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage07.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint

		pGeometry = LObstaclesPage07.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage07.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage07.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView08_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView08.SelectedIndexChanged
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage08.Parts) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage08.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint

		pGeometry = LObstaclesPage08.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage08.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage08.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		Select Case MultiPage1.SelectedIndex
			Case 0
				ListView01_SelectedIndexChanged(ListView01, New System.EventArgs)
				lblCountNumber.Text = ListView01.Items.Count
			Case 1
				ListView02_SelectedIndexChanged(ListView02, New System.EventArgs)
				lblCountNumber.Text = ListView02.Items.Count
			Case 2
				ListView03_SelectedIndexChanged(ListView03, New System.EventArgs)
				lblCountNumber.Text = ListView03.Items.Count
			Case 3
				ListView04_SelectedIndexChanged(ListView04, New System.EventArgs)
				lblCountNumber.Text = ListView04.Items.Count
			Case 4
				ListView05_SelectedIndexChanged(ListView05, New System.EventArgs)
				lblCountNumber.Text = ListView05.Items.Count
			Case 5
				ListView06_SelectedIndexChanged(ListView06, New System.EventArgs)
				lblCountNumber.Text = ListView06.Items.Count
			Case 6
				ListView07_SelectedIndexChanged(ListView07, New System.EventArgs)
				lblCountNumber.Text = ListView07.Items.Count
			Case 7
				ListView08_SelectedIndexChanged(ListView08, New System.EventArgs)
				lblCountNumber.Text = ListView08.Items.Count
		End Select
	End Sub

	Private Sub NonPrecReportFrm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As FormClosingEventArgs) Handles Me.FormClosing
		If eventArgs.CloseReason = CloseReason.UserClosing Then
			eventArgs.Cancel = True
			CloseBtn_ClickEvent(CloseBtn, New EventArgs())
		End If
	End Sub

	Private Sub CloseBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		SafeDeleteElement(pPointElem)
		SafeDeleteElement(pGeomElem)

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportBtn.Checked = False
		Hide()
	End Sub

#If SAVEREPORT Then
	Private Function CreateReportFile(ByRef pReport As ReportType) As Integer
		'Dim I As Integer
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
		'
		L = Len(FileName)
		pos = InStrRev(FileName, ".")
		Delime = Chr(34)

		If pos <> 0 Then
			pProcName = Left(FileName, pos)
			FileName = Left(FileName, pos) + "htm"
		Else
			pProcName = FileName
			FileName = FileName + ".htm"
		End If

		L = Len(pProcName)
		pos = InStrRev(pProcName, "\")
		If pos <> 0 Then pProcName = Right(pProcName, L - pos)

		LogFileNum = FreeFile()
		CreateReportFile = LogFileNum

		'Dim fso As New Scripting.FileSystemObject
		'Dim fil As Scripting.File
		'Debug.Print "File last modified: "; fil.DateLastModified ' Print info.

		Flg = False
		FileOpen(LogFileNum, FileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockReadWrite)

		'fil = fso.GetFile(FileName) ' Get a File object to query.
		'Flg = fil.size < 10
		Flg = True

		If Flg Then
			PrintLine(LogFileNum, "<HTML>")
			PrintLine(LogFileNum, "<HEAD>")
			PrintLine(LogFileNum, "<TITLE>" + Text + "</TITLE>") 'PANDA REPORT
			PrintLine(LogFileNum, "<style>{font-family: " + Delime + "Arial" + Delime + "; font-size: 11pt;")
			PrintLine(LogFileNum, "color: #000000; margin-top: 0px; margin-left: 0px; }</style>")
			PrintLine(LogFileNum, "</HEAD>")
			PrintLine(LogFileNum, "<BODY>")
		End If
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
					PrintLine(FileNum, "Segment length:  " + pReport.FinalLength + "</p>")
				End If
				SaveTabAsHTML(2, FileNum) 'Final

				If (pReport.iType And 1) = 0 Then 'С FAF
					PrintLine(FileNum, ParaStr)
					PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(3).Text + ":</b></p>")
					PrintLine(FileNum, ParaStr)

					PrintLine(FileNum, "<p>GEO segment track:  " + pReport.FinalTrack + " ")
					PrintLine(FileNum, "Track offset:    " + pReport.TrackOffset + " ")
					PrintLine(FileNum, "Segment length:  " + pReport.InterLength + "</p>")
					SaveTabAsHTML(3, FileNum) 'Interm
				End If

				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p><b>" + MultiPage1.TabPages.Item(4).Text + ":</b></p>")
				PrintLine(FileNum, ParaStr)
				PrintLine(FileNum, "<p>Surface gradient:  " + pReport.SurfGrad + "<br>")
				PrintLine(FileNum, "Area length:  " + pReport.StraightLength + "</p>")

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
					PrintLine(FileNum, "<p>Area length:  " + pReport.TARange + "</p>")
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
		'Dim hdrX As ColumnHeader
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
			Case Else
				Return
		End Select
		ParaStr = "<p></p>"

		itmsX = lListView.Items

		PrintLine(FileNum, "<Table border=1>")
		PrintLine(FileNum, "<Tr>")
		For I = 0 To lListView.Columns.Count - 1
			PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + "><b>" + CStr(lListView.Columns.Item(I).Text) + "</b></Td>")
		Next
		PrintLine(FileNum, "</Tr>")

		For I = 0 To itmsX.Count - 1 'lListView.ListItems.Count - 1
			'    Print #FileNum,"<Tr>"
			If System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = 0 Then
				ColorStr = "000000"
			ElseIf System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = 255 Then
				ColorStr = "FF0000"
			ElseIf System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = RGB(0, 0, 255) Then
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
		Dim tmpStr As String
		Dim StrOut As String
		Dim HeadersLen() As Integer
		Dim HeadersText() As String

		Dim hdrX As ColumnHeader
		Dim itmX As ListViewItem
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
			Case Else
				Return
		End Select

		PrintLine(FileNum, Chr(9) + MultiPage1.TabPages.Item(TabNum).Text)
		PrintLine(FileNum)

		N = lListView.Columns.Count
		ReDim HeadersText(N)
		ReDim HeadersLen(N)

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
			M = J / 2
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
				StrOut = Strings.Left(itmX.Text, maxLen - 1) + "*"
			Else
				StrOut = Space(maxLen - TmpLen) + itmX.Text
			End If

			For J = 1 To N - 1
				tmpStr = itmX.SubItems(J).Text

				TmpLen = Len(tmpStr)

				If TmpLen > maxLen Then
					tmpStr = Strings.Left(tmpStr, maxLen - 1) + "*"
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
			Case Else
				Return
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
			ReDim HeadersText(N)
			ReDim HeadersLen(N)

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
				M = J / 2
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
					StrOut = Strings.Left(itmX.Text, maxLen - 1) + "*"
				Else
					StrOut = Space(maxLen - TmpLen) + itmX.Text
				End If

				For J = 1 To N - 1
					tmpStr = itmX.SubItems(J).Text

					TmpLen = Len(tmpStr)

					If TmpLen > maxLen Then
						tmpStr = Strings.Left(tmpStr, maxLen - 1) + "*"
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

	Private Sub SaveBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim L As Integer
		Dim pos As Integer
		Dim FileNum As Integer
		Dim sExt As String
		Dim bHtml As Boolean

		If SaveDlg.ShowDialog() = DialogResult.OK Then
			L = Len(SaveDlg.FileName)
			pos = InStrRev(SaveDlg.FileName, ".")
			If pos <> 0 Then
				sExt = Strings.Right(SaveDlg.FileName, L - pos)
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
		End If
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

	Private Sub CNonPrecReportFrm_KeyUp(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
	End Sub
End Class
