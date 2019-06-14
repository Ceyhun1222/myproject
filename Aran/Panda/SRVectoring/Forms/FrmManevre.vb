Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports Aran.Aim.Features
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Enums
Imports Aran.Aim
Imports Aran.Queries
Imports Aran.Geometries

Friend Class CFrmManevre
	Inherits System.Windows.Forms.Form

	'Private Const ErrorText = "Error"
	'Private Const DoYouWishToExit = "Do you want to exit?"
	'Private Const NoSolution = "–ешение не существует"
	'Private ErrorText As String
	'Private DoYouWishToExit As String

	Private NoSolution As String

	Private aBufferWidth(1) As Double

	Private prevFrameInterceptionIndex As Integer
	Private prevFrameTurnIndex As Integer
	Private prevFrameArcDMEIndex As Integer

	'Private OptionButton201_PrevIndex As Long
	'Private OldCurPtDir As Double
	'Private NewCurPtDir As Double

	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
	Private pLegElem As ESRI.ArcGIS.Carto.IElement
	Private pLegPolyElem As ESRI.ArcGIS.Carto.IElement

	Private TextBox104Val As Double
	Private ArcLeg As ESRI.ArcGIS.Geometry.IPolyline
	Private pLegProtectArea As ESRI.ArcGIS.Geometry.IPolygon

	Private lNextPage As Integer
	Private CourseMode As Integer

	Private TurnDirection As Integer
	Private LegCount As Integer
	'Private ProcType As Long
	Private iCategory As Integer

	Private fBufferWidth As Double
	Private fInnerMin As Double
	Private fOuterMin As Double
	Private fOuterMax As Double

	Private AztDir As Double
	Private PrevH As Double

	Private RefH As Double
	Private fIAS As Double
	Private fTurnR As Double
	Private fBankLimit As Double

	Private FullLength As Double
	Private CurLength As Double

	Private DMEArcNav As NavaidType
	Private RDME As Double

	Private Textbox205MinMax As MinMaxDist
	Private TextBox206MinMax As MinMax

	Private CurrPt As WPT_FIXType
	Private sSIDProcName As String

	Private fMOCArray() As Double = {300.0, 450.0, 600.0}

	Private LegList() As MyArrivalLeg

	Private TPIntersectNav As NavaidType
	'Private ComboBox202NavList() As NavaidType
	'Private ComboBox204NavList() As NavaidType
	Private ComboBox301FIXList() As WPT_FIXType

	Private ComboBox402NavList() As WPT_FIXType	'NavaidType
	Private ComboBox402Intervals() As MinMax

	Private SelectedFIX() As WPT_FIXType
	Private OutBoundNav() As WPT_FIXType

	Private pIAPList As List(Of InstrumentApproachProcedure)
	Private pFacil_ComboBox605 As NavaidType
	Private CurrIAP As InstrumentApproachProcedure

	'Private helpIDs(7) As Integer

	Private ApproachFIXCount As Integer
	Private frStCourse() As GroupBox

	'Private RWYList() As RWYType

	Private InfoForm As CInfoForm
	Private ReportForm As CReportForm
	Private RemoveForm As CRemoveForm

	Private bFormInitialised As Boolean = False

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		''''''''''
		Dim I As Integer
		Dim N As Integer

		bFormInitialised = True

		MainSSTab.SelectedIndex = 0
		MainSSTab.TabPages.Item(2).Visible = False
		MainSSTab.TabPages.Item(3).Visible = False
		MainSSTab.TabPages.Item(4).Visible = False

		frStCourse = {frStCourse_0, frStCourse_1, frStCourse_2, frStCourse_3}

		Load_Recourse()

		FormatFrame(frStCourse)

		InfoForm = New CInfoForm()
		RemoveForm = New CRemoveForm()
		ReportForm = New CReportForm()
		ReportForm.SetReportBtn(ReportBtn)

		aBufferWidth(0) = 5700.0
		aBufferWidth(1) = 9300.0

		ComboBox003.Items.Add(CStr(ConvertDistance(aBufferWidth(0))))
		ComboBox003.Items.Add(CStr(ConvertDistance(aBufferWidth(1))))
		ComboBox003.SelectedIndex = 0

		ComboBox102.SelectedIndex = 0

		TextBox302.Text = "25"
		TextBox401.Text = "25"

		CurrPt.pPtPrj = New ESRI.ArcGIS.Geometry.Point

		CurrPt.pPtPrj.X = 0.0
		CurrPt.pPtPrj.Y = 0.0
		CurrPt.pPtPrj.Z = fMOCArray(0)

		TextBox302_Validating(TextBox302, New System.ComponentModel.CancelEventArgs())

		ohvat_tbox.Text = CStr(ConvertDistance(50000.0))

		lNextPage = 2
		LegCount = 0

		pLegElem = Nothing
		ReDim LegList(30)

		pGraphics = GetActiveView().GraphicsContainer

		adhp_cmbox.Items.Clear()

		'N = UBound(ADHPList)
		'For I = 0 To N
		'	adhp_cmbox.Items.Add(ADHPList(I).Name)
		'Next
		'If N >= 0 Then adhp_cmbox.SelectedIndex = 0

		'Dim K As Integer
		Dim pIAP As InstrumentApproachProcedure

		'FillADHPFields(CurrADHP)

		pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)

		N = pIAPList.Count

		For I = 0 To N - 1
			pIAP = pIAPList.Item(I)
			proc_cmbox.Items.Add(pIAP.Name)
		Next

		If N > 0 Then
			proc_cmbox.SelectedIndex = 0
			NextBtn.Enabled = True
			MainSSTab.TabPages.Item(1).Enabled = True
		Else
			MsgBox(My.Resources.str2001)
			NextBtn.Enabled = False
			MainSSTab.TabPages.Item(1).Enabled = False
			Exit Sub
		End If

		ohvat_tbox_Validating(ohvat_tbox, New System.ComponentModel.CancelEventArgs())


		'    J = UBound(RWYList)
		'    For I = 0 To J
		'        If I = 0 Then
		'            RWYMinMax.Max = RWYList(I).pPtGeo.Z
		'            RWYMinMax.Min = RWYList(I).pPtGeo.Z
		'        Else
		'            If RWYMinMax.Max < RWYList(I).pPtGeo.Z Then RWYMinMax.Max = RWYList(I).pPtGeo.Z
		'            If RWYMinMax.Min > RWYList(I).pPtGeo.Z Then RWYMinMax.Min = RWYList(I).pPtGeo.Z
		'        End If
		'    Next I
		'
		'    If FIXCount >= 0 Then
		'        For I = 0 To FIXCount
		'            ComboBox002.AddItem WPTList(I).Name
		'            ComboBox004.AddItem WPTList(I).Name
		'            ComboBox403.AddItem WPTList(I).Name    '"""""""????????
		'        Next I
		'
		'        ComboBox002.ListIndex = 0
		'        ComboBox004.ListIndex = 0
		'    End If


		'    ' ================= 2007 =================
		'
		'        Label1(0).Caption = LoadResString(1001)
		'        Label1(1).Caption = LoadResString(1002)
		'        Label1(2).Caption = LoadResString(1003)
		'        Label1(3).Caption = LoadResString(1004)
		'        Label1(4).Caption = LoadResString(1005)
		'
		'
		'        FocusStepCaption 0
		'        'MainSSTab.Top = -320
		'        Me.Height = 5415
		'
		'        FrameButtonContiner.Top = 4165
		'
		'        Dim m_CLBitmap As IPictureDisp
		'        Set m_CLBitmap = LoadResPicture("SHOW_INFO", 0)
		'        ShowImfFrame.Picture = m_CLBitmap
		'        Set m_CLBitmap = Nothing
		Me.Text = My.Resources.str1000

		MainSSTab.Top = -27
		FrameButtonContiner.Top = 306
		Height = 408

		'    HiddenForm.ChildLoad
	End Sub

	Private Sub FrmManevre_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		On Error Resume Next
		If (Not pLegElem Is Nothing) Then pGraphics.DeleteElement(pLegElem)

		pLegElem = Nothing

		For I = 0 To UBound(LegList) 'LegCount - 1
			If (Not LegList(I).pLegElement Is Nothing) Then pGraphics.DeleteElement(LegList(I).pLegElement)
			LegList(I).pLegElement = Nothing

			If Not (LegList(I).pLegPolygonElement Is Nothing) Then pGraphics.DeleteElement(LegList(I).pLegPolygonElement)
			LegList(I).pLegPolygonElement = Nothing
		Next I
		On Error GoTo 0

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

		If Not InfoForm Is Nothing Then InfoForm.Close()
		InfoForm = Nothing

		If Not ReportForm Is Nothing Then ReportForm.Close()
		ReportForm = Nothing

		If Not RemoveForm Is Nothing Then RemoveForm.Close()
		RemoveForm = Nothing

		On Error Resume Next

		N = UBound(NavaidList)
		For I = 0 To N
			NavaidList(I).pPtGeo = Nothing
			NavaidList(I).pPtPrj = Nothing
		Next I

		N = UBound(DMEList)
		For I = 0 To N
			DMEList(I).pPtGeo = Nothing
			DMEList(I).pPtPrj = Nothing
		Next I

		N = UBound(WPTList)
		For I = 0 To N
			WPTList(I).pPtGeo = Nothing
			WPTList(I).pPtPrj = Nothing
		Next I

		Erase NavaidList
		Erase DMEList
		Erase WPTList
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&AboutЕ")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

	Sub Load_Recourse()
		'===================== LoadResurse ========================================================
		'===================== Load Units ========================================================
		Label001_04.Text = HeightConverter(HeightUnit).Unit
		Label001_06.Text = HeightConverter(HeightUnit).Unit
		Label001_09.Text = DistanceConverter(DistanceUnit).Unit
		Label001_13.Text = DistanceConverter(DistanceUnit).Unit

		Label201_09.Text = DistanceConverter(DistanceUnit).Unit
		Label201_18.Text = DistanceConverter(DistanceUnit).Unit
		Label201_19.Text = DistanceConverter(DistanceUnit).Unit

		Label301_09.Text = SpeedConverter(SpeedUnit).Unit
		Label301_11.Text = DistanceConverter(DistanceUnit).Unit

		Label401_12.Text = SpeedConverter(SpeedUnit).Unit
		Label401_14.Text = DistanceConverter(DistanceUnit).Unit

		'===================== Load Consts ========================================================
		NoSolution = My.Resources.str0500 '"–ешение не существует"
		ComboBox001.Items.Clear()
		ComboBox001.Items.Add(CStr(ConvertHeight(300.0, eRoundMode.CEIL)))
		ComboBox001.Items.Add(CStr(ConvertHeight(450.0, eRoundMode.CEIL)))
		ComboBox001.Items.Add(CStr(ConvertHeight(600.0, eRoundMode.CEIL)))
		ComboBox001.SelectedIndex = 0
		TextBox205.Text = CStr(ConvertDistance(18520.0))

		'===================== Main Form ========================================================
		Text = My.Resources.str1000
		MainSSTab.TabPages.Item(0).Text = My.Resources.str1001
		MainSSTab.TabPages.Item(1).Text = My.Resources.str1002
		MainSSTab.TabPages.Item(2).Text = My.Resources.str1003
		MainSSTab.TabPages.Item(3).Text = My.Resources.str1004
		MainSSTab.TabPages.Item(4).Text = My.Resources.str1005

		PrevBtn.Text = My.Resources.str1006
		NextBtn.Text = My.Resources.str1007
		CalcBtn.Text = My.Resources.str1008
		AddBtn.Text = My.Resources.str1009
		ReturnBtn.Text = My.Resources.str1010
		RemoveBtn.Text = My.Resources.str1011
		HelpBtn.Text = My.Resources.str1012
		SaveBtn.Text = My.Resources.str1013
		CancelBtn.Text = My.Resources.str1014
		ReportBtn.Text = My.Resources.str1015
		InfoBtn.Text = My.Resources.str1016

		'===================== Page0 ========================================================
		Label001_00.Text = My.Resources.str1100
		Label001_05.Text = My.Resources.str1101
		Label001_03.Text = My.Resources.str1102
		Label001_07.Text = My.Resources.str0205
		Label001_08.Text = My.Resources.str1108
		Label001_15.Text = My.Resources.str1109
		Label001_02.Text = My.Resources.str0206

		Frame002.Text = My.Resources.str1103
		OptionButton002_0.Text = My.Resources.str1104
		OptionButton002_1.Text = My.Resources.str1105
		OptionButton001_0.Text = My.Resources.str1106
		OptionButton001_1.Text = My.Resources.str1107

		Label001_12.Text = My.Resources.str2002
		Label1.Text = My.Resources.str2003
		Label001_14.Text = My.Resources.str2004
		ComboBox005.SelectedIndex = 0

		'===================== Page1 ========================================================
		Frame101.Text = My.Resources.str1200
		Frame102.Text = My.Resources.str1201

		Label101_00.Text = My.Resources.str1202
		Label101_01.Text = My.Resources.str1205

		OptionButton101.Text = My.Resources.str1003
		OptionButton102.Text = My.Resources.str1004
		OptionButton103.Text = My.Resources.str1005

		ComboBox101.Items.Clear()
		ComboBox101.Items.Add((My.Resources.str1203))
		ComboBox101.Items.Add((My.Resources.str1204))
		ComboBox101.SelectedIndex = 0

		'===================== Page2 ========================================================
		CheckBoxChangeCourse.Text = My.Resources.str1300
		Label201_01.Text = My.Resources.str0207
		Label201_02.Text = My.Resources.str0205
		Label201_03.Text = My.Resources.str0206

		Frame202.Text = My.Resources.str1301

		OptionButton201.Text = My.Resources.str1302
		OptionButton202.Text = My.Resources.str1305
		OptionButton203.Text = My.Resources.str1306
		OptionButton204.Text = My.Resources.str1307

		frStCourse(0).Text = My.Resources.str1302
		frStCourse(1).Text = My.Resources.str1305
		frStCourse(2).Text = My.Resources.str1306
		frStCourse(3).Text = My.Resources.str1307

		Label201_08.Text = My.Resources.str0202
		Label201_10.Text = My.Resources.str1018
		ComboBox203.Items.Clear()
		ComboBox203.Items.Add(My.Resources.str1303)
		ComboBox203.Items.Add(My.Resources.str1304)

		Label201_12.Text = My.Resources.str0203 + "/" + My.Resources.str0204 + ":"
		Label201_14.Text = My.Resources.str1018

		CheckBox203.Text = My.Resources.str1308
		Label201_17.Text = My.Resources.str0202
		'===================== Page3 ========================================================
		Frame302.Text = My.Resources.str1400
		OptionButton301.Text = My.Resources.str1401
		OptionButton302.Text = My.Resources.str1402
		Label301_00.Text = My.Resources.str0205
		Label301_02.Text = My.Resources.str0206
		'   Label301(4).Caption = LoadResString(206)

		'===================== Page4 ========================================================
		Label401_00.Text = My.Resources.str1017 '200
		Label401_02.Text = My.Resources.str0205
		Label401_04.Text = My.Resources.str0206
		'===================== Finish ========================================================
	End Sub

	'Private Sub Form_Terminate()
	'    HiddenForm.DoUnload
	'    CloseConfig
	'    If (Not vFormUnload) And LegCount > 0 Then
	'        If MessageBox(hWnd, DoYouWishToExit, Caption, MB_YESNO Or MB_ICONQUESTION) = IDNO Then
	'            Cancel = 1
	'            Exit Sub
	'        End If
	'    End If
	'End Sub

	Function FillComboBox402(ByRef fTurnRadius As Double) As Integer
		Dim CLShift As Double

		Dim fTmp As Double
		Dim Radical As Double
		Dim DistFromCurrPt As Double

		Dim MinStartOfTurnX As Double
		Dim MaxStartOfTurnX As Double
		Dim MaxShiftAngle As Double

		Dim PossibleMaxTurnAngle As Double
		Dim PossibleMinTurnAngle As Double

		Dim Skip As Boolean

		Dim I As Integer
		Dim J As Integer
		Dim FIXCount As Integer

		FIXCount = WPTList.Length

		ComboBox402.Items.Clear()

		If FIXCount > 0 Then
			ReDim ComboBox402NavList(FIXCount - 1)
			ReDim ComboBox402Intervals(FIXCount - 1)
		Else
			ReDim ComboBox402NavList(-1)
			ReDim ComboBox402Intervals(-1)

			FillComboBox402 = -1
			Exit Function
		End If

		J = -1
		For I = 0 To FIXCount - 1
			'        If NavaidList(I).TypeCode <> 3 Then
			Skip = True
			DistFromCurrPt = Point2LineDistancePrj(CurrPt.pPtPrj, WPTList(I).pPtPrj, CurrPt.pPtPrj.M + 90.0) * SideDef(WPTList(I).pPtPrj, CurrPt.pPtPrj.M - 90.0, CurrPt.pPtPrj)
			CLShift = Point2LineDistancePrj(CurrPt.pPtPrj, WPTList(I).pPtPrj, CurrPt.pPtPrj.M) * SideDef(WPTList(I).pPtPrj, CurrPt.pPtPrj.M + 180.0, CurrPt.pPtPrj) * TurnDirection

			If CLShift > 0.0 Then '   Donme terefi
				If CLShift < 2.0 * fTurnRadius Then
					MaxShiftAngle = 2.0 * Math.Asin(System.Math.Sqrt(0.5 * CLShift / fTurnRadius))
					Skip = RadToDeg(MaxShiftAngle) > MaxTurnAngle

					If Not Skip Then
						MaxStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(MaxShiftAngle) - fTurnRadius * System.Math.Tan(0.5 * MaxShiftAngle)
						Skip = MaxStartOfTurnX < 0.0

						If Not Skip Then
							fTmp = DegToRad(MinTurnAngle)
							MinStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)

							fTmp = DegToRad(MaxTurnAngle)
							MaxStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)

							If MinStartOfTurnX < 0.0 Then
								MinStartOfTurnX = 0.0
								Radical = DistFromCurrPt * DistFromCurrPt - CLShift * (2 * fTurnRadius - CLShift)
								Skip = Radical < 0.0

								If Not Skip Then
									fTmp = (DistFromCurrPt - System.Math.Sqrt(Radical)) / (2 * fTurnRadius - CLShift)
									PossibleMinTurnAngle = RadToDeg(2 * System.Math.Atan(fTmp))

									fTmp = (DistFromCurrPt + System.Math.Sqrt(Radical)) / (2 * fTurnRadius - CLShift)
									PossibleMaxTurnAngle = RadToDeg(2 * System.Math.Atan(fTmp))
								End If
							Else
								PossibleMinTurnAngle = MinTurnAngle
								PossibleMaxTurnAngle = MaxTurnAngle
							End If
						End If
					End If
				ElseIf CLShift > 2 * fTurnRadius Then
					fTmp = DegToRad(MinTurnAngle)
					MinStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)

					fTmp = DegToRad(MaxTurnAngle)
					MaxStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)
					Skip = MaxStartOfTurnX < 0.0

					If Not Skip Then
						PossibleMaxTurnAngle = MaxTurnAngle

						If MinStartOfTurnX >= 0.0 Then
							PossibleMinTurnAngle = MinTurnAngle
						Else
							MinStartOfTurnX = 0.0

							Radical = DistFromCurrPt * DistFromCurrPt - CLShift * (2 * fTurnRadius - CLShift)
							Skip = Radical < 0.0

							If Not Skip Then
								fTmp = (DistFromCurrPt - System.Math.Sqrt(Radical)) / (2 * fTurnRadius - CLShift)
								PossibleMinTurnAngle = RadToDeg(2 * System.Math.Atan(fTmp))
							End If
						End If
					End If
				Else 'CLShift = 2 * fTurnRadius


				End If
			Else 'CLShift < 0
				fTmp = DegToRad(MaxTurnAngle)
				MinStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)

				fTmp = DegToRad(MinTurnAngle)
				MaxStartOfTurnX = DistFromCurrPt - CLShift / System.Math.Tan(fTmp) - fTurnRadius * System.Math.Tan(0.5 * fTmp)

				Skip = MaxStartOfTurnX < 0.0
				If Not Skip Then
					PossibleMinTurnAngle = MinTurnAngle
					PossibleMaxTurnAngle = MaxTurnAngle
					If MinStartOfTurnX < 0.0 Then
						MinStartOfTurnX = 0.0
						Radical = DistFromCurrPt * DistFromCurrPt - CLShift * (2 * fTurnRadius - CLShift)

						fTmp = (DistFromCurrPt + System.Math.Sqrt(Radical)) / (2 * fTurnRadius - CLShift)
						PossibleMaxTurnAngle = RadToDeg(2 * System.Math.Atan(fTmp))
					End If
				End If
			End If

			If Not Skip Then
				J = J + 1
				ComboBox402NavList(J) = WPTList(I)
				'                ComboBox402NavList(J).CLShift = CLShift
				'                ComboBox402NavList(J).DistFromYAx = DistFromCurrPt
				If TurnDirection < 0 Then
					ComboBox402Intervals(J).Min = CurrPt.pPtPrj.M + PossibleMinTurnAngle
					ComboBox402Intervals(J).Max = CurrPt.pPtPrj.M + PossibleMaxTurnAngle
				Else
					ComboBox402Intervals(J).Min = CurrPt.pPtPrj.M - PossibleMaxTurnAngle
					ComboBox402Intervals(J).Max = CurrPt.pPtPrj.M - PossibleMinTurnAngle
				End If

				ComboBox402.Items.Add(WPTList(I).Name)
			End If
			'        End If
		Next I

		FillComboBox402 = J

		If J >= 0 Then
			ReDim Preserve ComboBox402NavList(J)
			ReDim Preserve ComboBox402Intervals(J)
			ComboBox402.SelectedIndex = 0
		Else
			ReDim ComboBox402NavList(-1)
			ReDim ComboBox402Intervals(-1)
		End If
	End Function

	Sub InitLeg(ByRef Index As Integer)
		With LegList(Index)
			.pNominalPoly = Nothing
			.pLegElement = Nothing

			.GuidNav.TypeCode = -1
			.InterNav.TypeCode = -1

			.GuidNavType = ""
			.CODE_PHASE = ""
			.LegTypeARINC = CodeSegmentPath.CF
			.CODE_TYPE_COURSE = ""
			'.TurnDir = CodeDirectionTurn.EITHER
			.CODE_DESCR_DIST_VER = ""
			.CODE_DIST_VER_UPPER = ""
			.UOM_DIST_VER_UPPER = ""
			.CODE_DIST_VER_LOWER = ""
			.UOM_DIST_VER_LOWER = ""
			.UOM_SPEED = ""
			.CODE_SPEED_REF = ""
			.UOM_DUR = ""
			.UOM_DIST_HORZ = ""
			.CODE_REP_ATC = ""
			.CODE_ROLE_FIX = ""
			.TXT_RMK = ""

			.Guidance = False
			.T_TYPE = NO_DATA_VALUE
			.NO_SEQ = NO_DATA_VALUE
			.VAL_COURSE = NO_DATA_VALUE
			.VAL_DIST_VER_UPPER = NO_DATA_VALUE
			.VAL_DIST_VER_LOWER = NO_DATA_VALUE
			.VAL_VER_ANGLE = NO_DATA_VALUE
			.VAL_SPEED_LIMIT = NO_DATA_VALUE
			.VAL_DIST = NO_DATA_VALUE
			.VAL_DUR = NO_DATA_VALUE
			.VAL_THETA = NO_DATA_VALUE
			.VAL_RHO = NO_DATA_VALUE
			.VAL_BANK_ANGLE = 25.0
			.Radius_M = NO_DATA_VALUE
		End With
	End Sub

	Private Function CheckVOR_NDB(ByRef CheckFacil As NavaidType, ByRef fCurrDir As Double) As MinMax
		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint

		Dim LeftRightSide As Integer
		Dim AheadBehindMin As Integer
		Dim AheadBehindMax As Integer

		Dim ERange As Double
		Dim InterToler As Double
		Dim azt_Far As Double
		Dim azt_Near As Double
		Dim ValMin As Double
		Dim ValMax As Double
		Dim fDist As Double

		CheckVOR_NDB.Max = -1.0
		CheckVOR_NDB.Min = -1.0

		PtStart = CurrPt.pPtPrj
		fDist = Point2LineDistancePrj(CheckFacil.pPtPrj, PtStart, fCurrDir)

		If CheckFacil.TypeCode = 0 Then
			If fDist <= VOR.OnNAVRadius Then Exit Function
			InterToler = VOR.IntersectingTolerance
			ERange = VOR.Range
		Else
			If fDist <= NDB.OnNAVRadius Then Exit Function
			InterToler = NDB.IntersectingTolerance
			ERange = NDB.Range
		End If

		fDist = ReturnDistanceInMeters(CheckFacil.pPtPrj, PtStart)
		ptMin = PtStart

		AheadBehindMin = SideDef(PtStart, fCurrDir + 90.0, CheckFacil.pPtPrj)

		If fDist > ERange Then
			CircleVectorIntersect(CheckFacil.pPtPrj, ERange, PtStart, fCurrDir + (1 + AheadBehindMin) * 90.0, ptMin)
			If ptMin.IsEmpty Then Exit Function
			ERange = ReturnDistanceInMeters(CheckFacil.pPtPrj, ptMin)
		End If

		ptMax = PointAlongPlane(PtStart, fCurrDir, ERange)
		AheadBehindMax = SideDef(ptMax, fCurrDir + 90.0, CheckFacil.pPtPrj)

		fDist = ReturnDistanceInMeters(CheckFacil.pPtPrj, ptMax)
		If fDist > ERange Then
			CircleVectorIntersect(CheckFacil.pPtPrj, ERange, PtStart, fCurrDir + (1 + AheadBehindMax) * 90.0, ptMax)
			If ptMax.IsEmpty Then Exit Function
			ERange = ReturnDistanceInMeters(CheckFacil.pPtPrj, ptMax)
		End If

		LeftRightSide = SideDef(PtStart, fCurrDir, CheckFacil.pPtPrj)

		azt_Far = ReturnAngleInDegrees(CheckFacil.pPtPrj, ptMax)
		azt_Near = ReturnAngleInDegrees(CheckFacil.pPtPrj, ptMin)

		If SubtractAngles(azt_Near, azt_Far) >= 2.0 * InterToler Then
			If LeftRightSide > 0 Then
				ValMax = System.Math.Round(Dir2Azt(CheckFacil.pPtPrj, azt_Far + InterToler) - 0.4999999)
				ValMin = System.Math.Round(Dir2Azt(CheckFacil.pPtPrj, azt_Near - InterToler) + 0.4999999)
			Else
				ValMin = System.Math.Round(Dir2Azt(CheckFacil.pPtPrj, azt_Far - InterToler) + 0.4999999)
				ValMax = System.Math.Round(Dir2Azt(CheckFacil.pPtPrj, azt_Near + InterToler) - 0.4999999)
			End If

			If SubtractAngles(ValMax + InterToler, ValMin - InterToler) >= InterToler Then
				CheckVOR_NDB.Max = ValMax
				CheckVOR_NDB.Min = ValMin
			End If
		End If
	End Function

	Sub FillComboBox202(ByRef Direction As Double)
		Dim DMECount As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim PrevSelection As String

		DMECount = DMEList.Length

		PrevSelection = ""
		K = ComboBox202.SelectedIndex
		If K >= 0 Then PrevSelection = ComboBox202.SelectedText

		ComboBox202.Items.Clear()
		J = -1
		If DMECount > 0 Then
			K = 0
			For I = 0 To DMECount - 1
				If CheckDME(DMEList(I).pPtPrj, Direction) Then
					J = J + 1
					ComboBox202.Items.Add(DMEList(I))
					If DMEList(I).CallSign = PrevSelection Then K = J
				End If
			Next I
		End If

		If ComboBox202.Items.Count > 0 Then
			ComboBox202.SelectedIndex = K
		End If
	End Sub

	Sub FillComboBox204(ByRef Direction As Double)
		Dim NavCount As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim pMinMax As MinMax
		Dim PrevSelection As String

		NavCount = NavaidList.Length
		PrevSelection = ""
		K = ComboBox204.SelectedIndex
		If K >= 0 Then PrevSelection = ComboBox204.SelectedText

		ComboBox204.Items.Clear()

		J = -1
		If NavCount > 0 Then
			K = 0
			For I = 0 To NavCount - 1
				pMinMax = CheckVOR_NDB(NavaidList(I), Direction)
				If pMinMax.Max > 0 Then
					J = J + 1
					ComboBox204.Items.Add(NavaidList(I))
					If NavaidList(I).CallSign = PrevSelection Then K = J
				End If
			Next I
		End If

		If ComboBox204.Items.Count > 0 Then
			ComboBox204.SelectedIndex = K
		End If
	End Sub

	Sub FillComboBox205(ByVal Direction As Double)
		Dim fToler As Double
		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim FIXCount As Integer
		Dim PrevSelection As String

		FIXCount = WPTList.Length

		PrevSelection = ""
		K = ComboBox205.SelectedIndex
		If K >= 0 Then PrevSelection = ComboBox205.SelectedText

		ComboBox205.Items.Clear()

		If CheckBox203.Checked Then
			fToler = 90.0
			Direction = CurrPt.pPtPrj.M
		Else
			fToler = 1.0
		End If

		J = -1
		If FIXCount > 0 Then
			K = 0
			ReDim SelectedFIX(FIXCount - 1)
			For I = 0 To FIXCount - 1
				Side1 = SideDef(CurrPt.pPtPrj, Direction + fToler, WPTList(I).pPtPrj)
				Side2 = SideDef(CurrPt.pPtPrj, Direction - fToler, WPTList(I).pPtPrj)
				If (Side1 > 0) And (Side2 < 0) Then
					J = J + 1
					SelectedFIX(J) = WPTList(I)
					ComboBox205.Items.Add(WPTList(I).Name)
					If WPTList(I).Name = PrevSelection Then K = J
				End If
			Next I
		End If

		If J >= 0 Then
			ReDim Preserve SelectedFIX(J)
			ComboBox205.SelectedIndex = K
		Else
			ReDim SelectedFIX(-1)
		End If
	End Sub

	Sub To_StrightPage()
		Dim K As Integer
		CheckBox203.Enabled = LegCount > 0
		CheckBoxChangeCourse.Enabled = LegCount > 0


		If LegCount < 1 Then
			CheckBox203.Checked = False
			CheckBoxChangeCourse.Checked = False
			FullLength = 0.0
			ComboBox101.SelectedIndex = 0
		End If

		FillComboBox202((CurrPt.pPtPrj.M))
		FillComboBox204((CurrPt.pPtPrj.M))
		FillComboBox205(CurrPt.pPtPrj.M)

		OptionButton201.Enabled = ComboBox202.Items.Count > 0
		OptionButton202.Enabled = ComboBox204.Items.Count > 0
		OptionButton203.Enabled = ComboBox205.Items.Count > 0

		K = 0
		If Not OptionButton201.Enabled Then
			K = 1
			If Not OptionButton202.Enabled Then
				K = K + 1
				If Not OptionButton203.Enabled Then
					K = K + 1
					If OptionButton204.Checked Then
						OptionButton201_CheckedChanged(OptionButton204, New System.EventArgs())
					Else
						OptionButton204.Checked = True
					End If
				Else
					If OptionButton203.Checked Then
						OptionButton201_CheckedChanged(OptionButton203, New System.EventArgs())
					Else
						OptionButton203.Checked = True
					End If
				End If
			Else
				If OptionButton202.Checked Then
					OptionButton201_CheckedChanged(OptionButton202, New System.EventArgs())
				Else
					OptionButton202.Checked = True
				End If
			End If
		Else
			If OptionButton201.Checked Then
				OptionButton201_CheckedChanged(OptionButton202, New System.EventArgs())
			Else
				OptionButton201.Checked = True
			End If
		End If

		TextBoxChangeCourse_0.Text = CStr(System.Math.Round(CurrPt.pPtGeo.M))
		TextBoxChangeCourse_1.Text = CStr(System.Math.Round(Modulus(CurrPt.pPtGeo.M - CurrADHP.MagVar)))
	End Sub

	Sub To_TurnPage()
		Dim fPrevCourse As Double
		Dim RightTurn As Double
		Dim LeftTurn As Double
		Dim fTmp As Double
		Dim fBank As Double
		Dim fTAS As Double
		Dim pPtCnt As ESRI.ArcGIS.Geometry.IPoint

		Dim FIXCount As Integer
		Dim I As Integer
		Dim J As Integer

		FIXCount = WPTList.Length

		fIAS = cViafMax.Values(iCategory)
		TextBox303.Text = CStr(ConvertSpeed(fIAS))

		fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
		'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

		fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
		If fBank > fBankLimit Then fBank = fBankLimit

		fTurnR = Bank2Radius(fBank, fTAS)
		TextBox304.Text = CStr(ConvertDistance(fTurnR))
		TextBox403.Text = CStr(ConvertDistance(fTurnR))

		ComboBox301.Items.Clear()
		J = -1

		If FIXCount > 0 Then
			RightTurn = 0.5 * (1 - TurnDirection) * MaxTurnAngle
			LeftTurn = -0.5 * (TurnDirection + 1) * MaxTurnAngle
			ReDim ComboBox301FIXList(FIXCount - 1)

			fPrevCourse = CurrPt.pPtPrj.M
			For I = 0 To FIXCount - 1
				fTmp = ReturnAngleInDegrees(CurrPt.pPtPrj, WPTList(I).pPtPrj)
				If AngleInSector(fTmp, fPrevCourse + LeftTurn, fPrevCourse + RightTurn) Then
					pPtCnt = PointAlongPlane(CurrPt.pPtPrj, CurrPt.pPtPrj.M + 90.0 * TurnDirection, fTurnR)
					fTmp = ReturnDistanceInMeters(pPtCnt, WPTList(I).pPtPrj)

					If fTmp >= fTurnR Then
						J = J + 1
						ComboBox301FIXList(J) = WPTList(I)
						ComboBox301.Items.Add(WPTList(I).Name)
					End If
				End If
			Next I
		End If

		If J > -1 Then
			ReDim Preserve ComboBox301FIXList(J)
			ComboBox301.SelectedIndex = 0
			OptionButton302.Enabled = True
		Else
			'UPGRADE_ISSUE: Declaration type not supported: Array with upper bound less than zero. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="934BD4FF-1FF9-47BD-888F-D411E47E78FA"'
			ReDim ComboBox301FIXList(-1)
			OptionButton302.Enabled = False
			OptionButton302.Checked = False
		End If

		TextBox301_0.Text = CStr(System.Math.Round(CurrPt.pPtGeo.M, 1))
		TextBox301_1.Text = CStr(System.Math.Round(Modulus(CurrPt.pPtGeo.M - CurrADHP.MagVar), 1))

		If OptionButton301.Checked Then
			OptionButton301_CheckedChanged(OptionButton301, New System.EventArgs())
		Else
			OptionButton301.Checked = True
		End If
	End Sub

	Function To_IntersectPage() As Integer
		Dim fBank As Double
		Dim fTAS As Double

		fIAS = cViafMax.Values(iCategory)
		TextBox402.Text = CStr(ConvertSpeed(fIAS))

		fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
		'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

		fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
		If fBank > fBankLimit Then fBank = fBankLimit

		fTurnR = Bank2Radius(fBank, fTAS)
		TextBox304.Text = CStr(ConvertDistance(fTurnR))
		TextBox403.Text = CStr(ConvertDistance(fTurnR))

		To_IntersectPage = FillComboBox402(fTurnR)
	End Function

	Sub InitManevrePage()
		Dim K As Integer
		Dim fBank As Double
		Dim fTAS As Double
		K = ComboBox002.SelectedIndex

		LegCount = 0
		RefH = DeConvertHeight(CDbl(TextBox002.Text))

		CurrPt = WPTList(K)
		CurrPt.pPtPrj.Z = RefH
		CurrPt.pPtGeo.Z = RefH

		If OptionButton002_0.Checked Then
			CurrPt.pPtGeo.M = CDbl(TextBox004_0.Text)
		Else
			CurrPt.pPtGeo.M = CDbl(TextBox004_1.Text) + CurrADHP.MagVar
		End If

		CurrPt.pPtPrj.M = Azt2Dir(CurrPt.pPtGeo, CurrPt.pPtGeo.M)

		fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
		'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

		fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
		If fBank > fBankLimit Then fBank = fBankLimit

		fTurnR = Bank2Radius(fBank, fTAS)
		TextBox304.Text = CStr(ConvertDistance(fTurnR))
		TextBox403.Text = CStr(ConvertDistance(fTurnR))
	End Sub

	Function CalcStraightCourse(ByRef fParam As Double) As Boolean
		Dim fDir As Double
		Dim fTmp As Double
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pFacil As NavaidType
		Dim pFIX As WPT_FIXType
		Dim txtBox As TextBox

		If CourseMode = 0 Then
			txtBox = TextBoxChangeCourse_0
		Else
			txtBox = TextBoxChangeCourse_1
		End If

		On Error Resume Next
		If Not pLegElem Is Nothing Then pGraphics.DeleteElement(pLegElem)
		If Not pLegPolyElem Is Nothing Then pGraphics.DeleteElement(pLegPolyElem)
		On Error GoTo 0

		pLegElem = Nothing
		pLegPolyElem = Nothing

		InitLeg(LegCount)

		If CurrPt.TypeCode <> -1 Then LegList(LegCount).GuidNav = FixToNav(CurrPt)

		If CheckBoxChangeCourse.Checked And IsNumeric(txtBox.Text) Then
			If CourseMode = 0 Then
				fDir = Azt2Dir(CurrPt.pPtGeo, CDbl(txtBox.Text))
			Else
				fDir = Azt2Dir(CurrPt.pPtGeo, CDbl(txtBox.Text) + CurrADHP.MagVar)
			End If
		Else
			fDir = CurrPt.pPtPrj.M
		End If

		pClone = CurrPt.pPtPrj
		ptFrom = pClone.Clone

		CalcStraightCourse = True
		AddBtn.Enabled = False

		'========================== DME ===================================
		If OptionButton201.Checked Then 'DME
			pFacil = ComboBox202.SelectedItem
			fTmp = CircleVectorIntersect(pFacil.pPtPrj, fParam, ptFrom, fDir + 180.0 * (1 - ComboBox203.SelectedIndex), ptTo)

			If fTmp < 0.0 Then
				MsgBox(NoSolution, MsgBoxResult.Ok + MsgBoxStyle.Exclamation, Text)
				CalcStraightCourse = False
				Exit Function
			End If

			LegList(LegCount).VAL_RHO = fParam
			LegList(LegCount).InterNav = pFacil
			ptTo.Z = ptFrom.Z
			'========================== VOR ===================================
		ElseIf OptionButton202.Checked Then 'VOR
			pFacil = ComboBox204.SelectedItem

			ptTo = LineLineIntersect(ptFrom, fDir, pFacil.pPtPrj, fParam)
			If ptTo.IsEmpty Then
				MsgBox(NoSolution, MsgBoxResult.Ok + MsgBoxStyle.Exclamation, Text)
				CalcStraightCourse = False
				Exit Function
			End If

			If SideDef(ptFrom, fDir + 90.0, ptTo) < 0 Then
				MsgBox(NoSolution, MsgBoxResult.Ok + MsgBoxStyle.Exclamation, Text)
				CalcStraightCourse = False
				Exit Function
			End If

			LegList(LegCount).VAL_THETA = fParam
			LegList(LegCount).InterNav = pFacil
			'========================== FIX ===================================
		ElseIf OptionButton203.Checked Then 'FIX
			pFIX = SelectedFIX(ComboBox205.SelectedIndex)

			pClone = pFIX.pPtPrj
			ptTo = pClone.Clone

			fDir = ReturnAngleInDegrees(ptFrom, ptTo)

			LegList(LegCount).LegTypeARINC = CodeSegmentPath.CF
			'=================== Distance from Cur position ==============================
		ElseIf OptionButton204.Checked Then 'Distance from Cur position
			ptTo = PointAlongPlane(ptFrom, fDir, fParam)
			LegList(LegCount).LegTypeARINC = CodeSegmentPath.FC
		End If
		''=======================================================================

		ptTo.Z = ptFrom.Z
		ptFrom.M = fDir
		ptTo.M = fDir

		ArcLeg = New ESRI.ArcGIS.Geometry.Polyline
		ArcLeg.FromPoint = ptFrom
		ArcLeg.ToPoint = ptTo
		LegList(LegCount).pPtFlyBy = ptTo

		CurLength = System.Math.Round(ArcLeg.Length)
		AddBtn.Enabled = (CurLength > 1)

		pLegElem = DrawPolyLine(ArcLeg, 0, 2)
		pLegElem.Locked = True

		LegList(LegCount).CourseDir = fDir
		LegList(LegCount).VAL_COURSE = System.Math.Round(Dir2Azt(ptFrom, fDir), 1)

		InfoForm.AddEndPoint(ptTo, LegList(LegCount).VAL_COURSE, CurLength)

		CreateLegProtectArea()
	End Function

	Sub CalcTurn()
		Dim OutAzt As Double
		Dim OutDir As Double
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pConstructor As ESRI.ArcGIS.Geometry.IConstructPoint

		If MainSSTab.SelectedIndex = 0 Then
			Exit Sub
		End If

		On Error Resume Next
		If Not pLegElem Is Nothing Then pGraphics.DeleteElement(pLegElem)
		On Error GoTo 0

		pLegElem = Nothing
		AddBtn.Enabled = False

		InitLeg(LegCount)

		If CurrPt.TypeCode <> -1 Then LegList(LegCount).GuidNav = FixToNav(CurrPt)

		'---------------------- Intercept Cource
		Dim txtBox As TextBox = IIf(CourseMode = 0, TextBox301_0, TextBox301_1)

		If OptionButton301.Checked Then
			If IsNumeric(txtBox.Text) Then
				If CourseMode = 0 Then
					OutAzt = CDbl(txtBox.Text)
				Else
					OutAzt = CDbl(txtBox.Text) + CurrADHP.MagVar
				End If
			Else
				OutAzt = CurrPt.pPtGeo.M
				TextBox301_0.Text = CStr(System.Math.Round(OutAzt))
				TextBox301_1.Text = CStr(System.Math.Round(Modulus(OutAzt - CurrADHP.MagVar)))
			End If

			OutDir = Azt2Dir(CurrPt.pPtGeo, OutAzt)

			pPointCollection = New ESRI.ArcGIS.Geometry.Multipoint
			ptCnt = PointAlongPlane(CurrPt.pPtPrj, CurrPt.pPtPrj.M - 90.0 * TurnDirection, fTurnR)
			ptTo = PointAlongPlane(ptCnt, OutDir + 90.0 * TurnDirection, fTurnR)
			ptTo.M = OutDir

			pPointCollection.AddPoint(CurrPt.pPtPrj)
			pPointCollection.AddPoint(ptTo)
		Else
			'-------------------- Intercept FIX
			pPointCollection = TurnToFixPrj(CurrPt.pPtPrj, fTurnR, -TurnDirection, ComboBox301FIXList(ComboBox301.SelectedIndex).pPtPrj, OutDir)
			If pPointCollection.PointCount < 2 Then
				MsgBox(My.Resources.str0304, MsgBoxResult.Ok Or MsgBoxStyle.Exclamation, Text)
				Exit Sub
			End If
		End If

		ArcLeg = CalcTrajectoryFromMultiPoint(pPointCollection)
		ptTo = ArcLeg.ToPoint

		ptTo.Z = CurrPt.pPtPrj.Z
		If OptionButton302.Checked Then
			OutAzt = Dir2Azt(CurrPt.pPtPrj, ptTo.M)
			TextBox301_0.Text = CStr(System.Math.Round(OutAzt))
			TextBox301_1.Text = CStr(System.Math.Round(Modulus(OutAzt - CurrADHP.MagVar)))
		End If

		'==================================================================================================================
		pClone = CurrPt.pPtPrj
		LegList(LegCount).pPtFlyBy = pClone.Clone
		pConstructor = LegList(LegCount).pPtFlyBy
		pConstructor.ConstructAngleIntersection(CurrPt.pPtPrj, DegToRad(CurrPt.pPtPrj.M), ptTo, DegToRad(ptTo.M))
		'==================================================================================================================

		ArcLeg.ToPoint = ptTo
		pLegElem = DrawPolyLine(ArcLeg, 0, 2)
		pLegElem.Locked = True

		CurLength = System.Math.Round(ArcLeg.Length)
		AddBtn.Enabled = CurLength > 1

		LegList(LegCount).LegTypeARINC = CodeSegmentPath.RF

		LegList(LegCount).CourseDir = pPointCollection.Point(pPointCollection.PointCount - 1).M
		LegList(LegCount).VAL_COURSE = System.Math.Round(Dir2Azt(pPointCollection.Point(pPointCollection.PointCount - 1), LegList(LegCount).CourseDir), 1)
		LegList(LegCount).CODE_TURN_VALID = False

		InfoForm.AddEndPoint(ptTo, LegList(LegCount).VAL_COURSE, CurLength)
		CreateLegProtectArea()
	End Sub

	Sub CalcInterception()
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pConstructor As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint

		Dim fixDir As Double
		Dim OutDir As Double
		Dim K As Integer

		If MainSSTab.SelectedIndex = 0 Then
			Exit Sub
		End If

		On Error Resume Next
		If Not pLegElem Is Nothing Then pGraphics.DeleteElement(pLegElem)
		On Error GoTo 0
		pLegElem = Nothing

		AddBtn.Enabled = False

		InitLeg(LegCount)

		If CurrPt.TypeCode <> -1 Then LegList(LegCount).GuidNav = FixToNav(CurrPt)

		'------------------Intercept Cource Nav-Fix
		K = ComboBox402.SelectedIndex
		ptNav = ComboBox402NavList(K).pPtPrj
		'        fixDir = ReturnAngleInDegrees(ComboBox402NavList(K).pPtPrj, ComboBox402NavList(K).pPtPrj)
		If CourseMode = 0 Then
			fixDir = Azt2Dir(CurrPt.pPtGeo, CDbl(_TextBox404_0.Text))
		Else
			fixDir = Azt2Dir(CurrPt.pPtGeo, CDbl(_TextBox404_1.Text) + CurrADHP.MagVar)
		End If

		'        If (ComboBox402NavList(K).TypeCode = NavaidTypeCode.CodeVOR) Or (ComboBox402NavList(K).TypeCode = NavaidTypeCode.CodeNDB) Then
		'            fi = ComboBox402NavList(K)
		'        Else
		'            If (WPTList(ComboBox403.ListIndex).TypeCode = NavaidTypeCode.CodeVOR) Or (WPTList(ComboBox403.ListIndex).TypeCode = NavaidTypeCode.CodeNDB) Then
		'                fi = WPTList(ComboBox403.ListIndex)
		'            Else
		'                fi.TypeCode = -1
		'            End If
		'        End If

		'------------------Intercept Cource Nav-Fix

		OutDir = CurrPt.pPtPrj.M
		pPointCollection = CalcTrackByFixDir(CurrPt.pPtPrj, ptNav, OutDir, fixDir)

		If pPointCollection Is Nothing Then
			MsgBox(My.Resources.str0304, MsgBoxResult.Ok Or MsgBoxStyle.Exclamation, Text)
			Exit Sub
		End If

		ArcLeg = CalcTrajectoryFromMultiPoint(pPointCollection)

		ptTo = ArcLeg.ToPoint
		ptTo.Z = CurrPt.pPtPrj.Z

		pClone = ArcLeg.ToPoint
		LegList(LegCount).pPtFlyBy = pClone.Clone

		pConstructor = LegList(LegCount).pPtFlyBy
		pConstructor.ConstructAngleIntersection(CurrPt.pPtPrj, DegToRad(CurrPt.pPtPrj.M), ptTo, DegToRad(ptTo.M))
		'==========================================================

		ArcLeg.ToPoint = ptTo
		pLegElem = DrawPolyLine(ArcLeg, 0, 2)

		CurLength = System.Math.Round(ArcLeg.Length)
		AddBtn.Enabled = (CurLength > 1)

		LegList(LegCount).LegTypeARINC = CodeSegmentPath.CI

		LegList(LegCount).CourseDir = ptTo.M
		LegList(LegCount).VAL_COURSE = System.Math.Round(Dir2Azt(ptTo, LegList(LegCount).CourseDir), 1)
		LegList(LegCount).VAL_THETA = NO_DATA_VALUE
		LegList(LegCount).CODE_TURN_VALID = False

		InfoForm.AddEndPoint(ptTo, LegList(LegCount).VAL_COURSE, CurLength)
		CreateLegProtectArea()
	End Sub

	Function CalcTrackByFixDir(ByRef PtSt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFIX As ESRI.ArcGIS.Geometry.IPoint, ByRef dirCur As Double, ByRef DirFix As Double) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim dDir As Double
		Dim fTmp As Double
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim DistSign As Integer

		CalcTrackByFixDir = Nothing
		ptTmp = LineLineIntersect(PtSt, dirCur, ptFIX, DirFix)

		If ptTmp.IsEmpty Then Exit Function

		If Modulus(DirFix - dirCur, 360.0) > 180.0 Then
			DistSign = TurnDirection
		Else
			DistSign = -TurnDirection
		End If

		fTmp = SubtractAngles(DirFix, dirCur)
		dDir = fTurnR * System.Math.Tan(DegToRad(0.5 * fTmp))

		pPt1 = PointAlongPlane(ptTmp, dirCur + (DistSign + 1) * 90.0, dDir)
		If SideDef(pPt1, dirCur + 90.0, PtSt) > 0 Then Exit Function
		pPt1.M = dirCur

		CalcTrackByFixDir = New ESRI.ArcGIS.Geometry.Multipoint
		CalcTrackByFixDir.AddPoint(PtSt)
		CalcTrackByFixDir.AddPoint(pPt1)

		pPt2 = PointAlongPlane(ptTmp, DirFix + (DistSign - 1) * 90.0, dDir)
		pPt2.M = DirFix
		CalcTrackByFixDir.AddPoint(pPt2)
	End Function

	'UPGRADE_WARNING: Event ComboBox005.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox005_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox005.SelectedIndexChanged
		iCategory = ComboBox005.SelectedIndex
		fIAS = cViafMax.Values(iCategory)
	End Sub

	Private Sub ohvat_tbox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles ohvat_tbox.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			ohvat_tbox_Validating(ohvat_tbox, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (ohvat_tbox.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub ohvat_tbox_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles ohvat_tbox.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim Radius As Double
		Dim I As Integer
		Dim N As Integer

		If Not IsNumeric(ohvat_tbox.Text) Then GoTo EventExitSub
		Radius = DeConvertDistance(CDbl(ohvat_tbox.Text))

		ComboBox002.Items.Clear()
		ComboBox004.Items.Clear()
		ComboBox403.Items.Clear()

		If Not CurrADHP.pPtPrj Is Nothing Then
			FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, Radius)
			FillWPT_FIXList(WPTList, CurrADHP, Radius)
			GetObstaclesByDist(ObstacleList, CurrADHP, Radius, 0.0)
			N = UBound(WPTList)

			If N >= 0 Then
				For I = 0 To N
					ComboBox002.Items.Add(WPTList(I).Name)
					ComboBox004.Items.Add(WPTList(I).Name)
					ComboBox403.Items.Add(WPTList(I).Name) '"""""""????????
				Next I

				ComboBox002.SelectedIndex = 0
				ComboBox004.SelectedIndex = 0
			End If
		End If
EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub adhp_cmbox_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles adhp_cmbox.SelectedIndexChanged
		'Dim I As Integer
		'Dim K As Integer
		'Dim N As Integer

		'Dim pIAP As IAP

		'K = adhp_cmbox.SelectedIndex
		'proc_cmbox.Items.Clear()

		'If K < 0 Then Exit Sub

		'CurrADHP = ADHPList(K)

		'FillADHPFields(CurrADHP)

		'pIAPList = pObjectDir.GetARANDB_IAPList(CurrADHP.Identifier)

		'N = pIAPList.Count

		'For I = 0 To N - 1
		'	pIAP = pIAPList.GetItem(I)
		'	proc_cmbox.Items.Add(pIAP.Designator)
		'Next

		'If N > 0 Then
		'	proc_cmbox.SelectedIndex = 0
		'	NextBtn.Enabled = True
		'	MainSSTab.TabPages.Item(1).Enabled = True
		'Else
		'	MsgBox(My.Resources.str2001)
		'	NextBtn.Enabled = False
		'	MainSSTab.TabPages.Item(1).Enabled = False
		'	Exit Sub
		'End If

		'ohvat_tbox_Validating(ohvat_tbox, New System.ComponentModel.CancelEventArgs(False))

		'Private ObstacleList() As ObstacleType
		'    If FIXCount >= 0 Then
		'        For I = 0 To FIXCount
		'            ComboBox002.AddItem WPTList(I).Name
		'            ComboBox004.AddItem WPTList(I).Name
		'            ComboBox403.AddItem WPTList(I).Name    '"""""""????????
		'        Next I
		'
		'        ComboBox002.ListIndex = 0
		'        ComboBox004.ListIndex = 0
		'    End If

	End Sub

	Private Sub CalcBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CalcBtn.Click
		Dim Allowed As Boolean
		Dim CurrPDG As Boolean
		TurnDirection = 2 * ComboBox101.SelectedIndex - 1

		LegList(LegCount).VAL_DUR = NO_DATA_VALUE
		LegList(LegCount).VAL_THETA = NO_DATA_VALUE
		LegList(LegCount).VAL_RHO = NO_DATA_VALUE
		LegList(LegCount).VAL_BANK_ANGLE = 25.0

		If IsNumeric(TextBox104.Text) Then
			CurrPDG = CDbl(TextBox104.Text)
			If ComboBox102.SelectedIndex = 0 Then
				LegList(LegCount).VAL_VER_ANGLE = -RadToDeg(System.Math.Atan(CShort(0.01) * CShort(CurrPDG)))
			Else
				LegList(LegCount).VAL_VER_ANGLE = -CShort(CurrPDG)
			End If
		End If

		LegList(LegCount).VAL_DIST_VER_UPPER = CurrPt.pPtPrj.Z

		InfoForm.NewSegment()
		InfoForm.AddStartPoint(CurrPt.pPtGeo)
		Allowed = True

		Select Case lNextPage
			Case 2
				To_StrightPage()
			Case 3
				To_TurnPage()
			Case 4
				Allowed = To_IntersectPage() >= 0
				If Not Allowed Then MsgBox(My.Resources.str0500) '"Net podxodyashego sredstva."
		End Select

		If Allowed Then
			MainSSTab.SelectedIndex = lNextPage
			InfoBtn.Enabled = True
		End If
	End Sub

	Private Sub CancelBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
		Me.Close()
	End Sub

	'UPGRADE_WARNING: Event CheckBoxChangeCourse.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub CheckBoxChangeCourse_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBoxChangeCourse.CheckStateChanged
		Dim NewColor As Color

		TextBoxChangeCourse_0.Enabled = CheckBoxChangeCourse.Checked
		TextBoxChangeCourse_1.Enabled = CheckBoxChangeCourse.Checked
		NewColor = IIf(CheckBoxChangeCourse.Checked, EnabledColor, DisabledColor)

		If CourseMode = 0 Then
			TextBoxChangeCourse_0.BackColor = NewColor
		Else
			TextBoxChangeCourse_1.BackColor = NewColor
		End If

		Label201_02.Enabled = CheckBoxChangeCourse.Checked
		Label201_03.Enabled = CheckBoxChangeCourse.Checked
		Label201_04.Enabled = CheckBoxChangeCourse.Checked
		Label201_05.Enabled = CheckBoxChangeCourse.Checked

		If Not CheckBoxChangeCourse.Checked Then
			TextBoxChangeCourse_0.Text = CStr(System.Math.Round(CurrPt.pPtGeo.M))
			TextBoxChangeCourse_1.Text = CStr(System.Math.Round(Modulus(CurrPt.pPtGeo.M - CurrADHP.MagVar)))
			ApplyNewDirection((CurrPt.pPtPrj.M))
		End If
	End Sub

	'UPGRADE_WARNING: Event ComboBox003.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox003_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox003.SelectedIndexChanged
		fBufferWidth = aBufferWidth(ComboBox003.SelectedIndex)
	End Sub

	'UPGRADE_WARNING: Event ComboBox004.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox004_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox004.SelectedIndexChanged
		Dim angPrj As Double
		Dim fTmp As Double
		Dim K As Integer

		Label001_10.Text = ""

		If ComboBox004.SelectedIndex < 0 Then Exit Sub

		If ComboBox004.Items.Count < 2 Or ComboBox002.Items.Count < 2 Then Exit Sub
		If ComboBox004.SelectedIndex < 0 Then ComboBox004.SelectedIndex = IIf(ComboBox002.SelectedIndex = 0, ComboBox004.Items.Count - 1, 0)

		If ComboBox004.SelectedIndex = ComboBox002.SelectedIndex Then
			If ComboBox002.SelectedIndex + 1 > ComboBox004.Items.Count - 1 Then
				ComboBox004.SelectedIndex = 0
			Else
				ComboBox004.SelectedIndex = ComboBox002.SelectedIndex + 1
			End If
			Exit Sub
		End If

		K = ComboBox004.SelectedIndex
		Label001_10.Text = GetNavTypeName(WPTList(K).TypeCode)

		angPrj = ReturnAngleInDegrees(WPTList(ComboBox002.SelectedIndex).pPtPrj, WPTList(K).pPtPrj)
		fTmp = Dir2Azt(WPTList(ComboBox002.SelectedIndex).pPtPrj, angPrj)

		TextBox004_0.Text = CStr(System.Math.Round(fTmp, 2))
		TextBox004_1.Text = CStr(System.Math.Round(Modulus(fTmp - CurrADHP.MagVar), 2))
	End Sub

	'UPGRADE_WARNING: Event ComboBox402.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox402_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox402.SelectedIndexChanged
		Dim K As Integer
		Dim fTmpFrom As Double
		Dim fTmpTo As Double

		If ComboBox402.SelectedIndex < 0 Then Exit Sub
		K = ComboBox402.SelectedIndex
		Label401_01.Text = GetNavTypeName(ComboBox402NavList(K).TypeCode)
		'    Label401(1).Caption = WPTList(K).TypeName

		fTmpFrom = Dir2Azt(CurrPt.pPtPrj, ComboBox402Intervals(K).Max)
		fTmpTo = Dir2Azt(CurrPt.pPtPrj, ComboBox402Intervals(K).Min)

		Label401_08.Text = My.Resources.str0301 & vbCrLf & My.Resources.str0302 & CStr(System.Math.Round(fTmpFrom, 3)) & My.Resources.str0303 & CStr(System.Math.Round(fTmpTo, 3))

		_TextBox404_0.Text = CStr(Modulus(System.Math.Round(fTmpFrom + 0.49999)))
		_TextBox404_1.Text = CStr(Modulus(System.Math.Round(fTmpFrom - CurrADHP.MagVar + 0.49999)))

		CalcInterception()
	End Sub

	'UPGRADE_WARNING: Event ComboBox403.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox403_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox403.SelectedIndexChanged
		'    If ComboBox403.ListIndex < 0 Then Exit Sub
		'
		'    ComboBox402.Tag = "N"
		'    If ComboBox403.ListIndex = ComboBox402.ListIndex Then
		'        If ComboBox402.ListIndex < ComboBox402.ListCount - 1 Then
		'            ComboBox402.ListIndex = ComboBox402.ListIndex + 1
		'        Else
		'            ComboBox402.ListIndex = ComboBox402.ListIndex - 1
		'        End If
		'    End If
		'    ComboBox402.Tag = ""
		'
		'    Label401(9).Caption = WPTList(ComboBox403.ListIndex).TypeName

		'CalcInterception
	End Sub

	Private Sub CreateLegProtectArea()
		Dim N As Integer
		Dim I As Integer

		Dim pCircle1 As ESRI.ArcGIS.Geometry.Polygon
		Dim pCircle2 As ESRI.ArcGIS.Geometry.Polygon
		Dim pLegPoly As ESRI.ArcGIS.Geometry.Polygon
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim ReqH As Double
		Dim fMOC As Double


		fMOC = fMOCArray(ComboBox001.SelectedIndex)

		On Error Resume Next
		If Not pLegPolyElem Is Nothing Then pGraphics.DeleteElement(pLegPolyElem)
		On Error GoTo 0
		pLegPolyElem = Nothing

		If pLegProtectArea Is Nothing Then pLegProtectArea = New ESRI.ArcGIS.Geometry.Polygon
		pPolygon = pLegProtectArea
		If pPolygon.PointCount > 0 Then pPolygon.RemovePoints(0, pPolygon.PointCount)

		If LegCount <= 0 Then

			pPolygon.AddPoint(PointAlongPlane(ArcLeg.FromPoint, ArcLeg.FromPoint.M + 90.0, fBufferWidth))
			pPolygon.AddPoint(PointAlongPlane(ArcLeg.FromPoint, ArcLeg.FromPoint.M - 90.0, fBufferWidth))
			pCircle1 = CreatePrjCircle(ArcLeg.FromPoint, fBufferWidth)
		Else
			pPolygon.AddPoint(PointAlongPlane(LegList(LegCount - 1).pPtFlyBy, ArcLeg.FromPoint.M + 90.0, fBufferWidth))
			pPolygon.AddPoint(PointAlongPlane(LegList(LegCount - 1).pPtFlyBy, ArcLeg.FromPoint.M - 90.0, fBufferWidth))
			pCircle1 = CreatePrjCircle(LegList(LegCount - 1).pPtFlyBy, fBufferWidth)
		End If

		pPolygon.AddPoint(PointAlongPlane(LegList(LegCount).pPtFlyBy, ArcLeg.FromPoint.M - 90.0, fBufferWidth))
		pPolygon.AddPoint(PointAlongPlane(LegList(LegCount).pPtFlyBy, ArcLeg.FromPoint.M + 90.0, fBufferWidth))
		pCircle2 = CreatePrjCircle(LegList(LegCount).pPtFlyBy, fBufferWidth)

		pTopoOper = pPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pLegPoly = pTopoOper.Union(pCircle1)
		pTopoOper = pLegPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pLegProtectArea = pTopoOper.Union(pCircle2)
		pTopoOper = pLegProtectArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		N = UBound(ObstacleList)

		LegList(LegCount).VAL_DIST_VER_LOWER = CurrPt.pPtPrj.Z + CurLength * LegList(LegCount).VAL_VER_ANGLE
		'LegList(LegCount).VAL_DIST_VER_LOWER = CurrPt.pPtPrj.Z + CurLength * LegList(LegCount).VAL_VER_ANGLE

		If N >= 0 Then
			pRelation = pLegProtectArea

			For I = 0 To N
				If pRelation.Contains(ObstacleList(I).pPtPrj) Then
					ReqH = ObstacleList(I).Height + fMOC

					If LegList(LegCount).VAL_DIST_VER_LOWER < ReqH Then
						LegList(LegCount).VAL_DIST_VER_LOWER = ReqH
					End If
				End If
			Next I
		End If

		If (LegList(LegCount).VAL_DIST_VER_UPPER <> NO_DATA_VALUE) And (LegList(LegCount).VAL_DIST_VER_LOWER > LegList(LegCount).VAL_DIST_VER_UPPER) Then
			MsgBox("VAL_DIST_VER_LOWER > VAL_DIST_VER_UPPER", MsgBoxStyle.Critical)
		End If

		TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs(False))

		pLegPolyElem = DrawPolygon(pLegProtectArea, RGB(0, 0, 255))
		pLegPolyElem.Locked = True
	End Sub

	Private Sub AddBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles AddBtn.Click
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim iSign1 As Integer
		Dim iSign2 As Integer
		Dim STR_DIR_TURN(0 To 3) As CodeDirectionTurn

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		Dim LegObstList() As TypeDefinitions.ObstacleType

		Dim fMOC As Double
		Dim fTmp As Double


		fMOC = fMOCArray(ComboBox001.SelectedIndex)

		STR_DIR_TURN(0) = CodeDirectionTurn.EITHER 'NON
		STR_DIR_TURN(1) = CodeDirectionTurn.LEFT
		STR_DIR_TURN(2) = CodeDirectionTurn.RIGHT
		STR_DIR_TURN(3) = CodeDirectionTurn.EITHER

		On Error Resume Next
		If (Not pLegElem Is Nothing) Then pGraphics.DeleteElement(pLegElem)
		If Not pLegPolyElem Is Nothing Then pGraphics.DeleteElement(pLegPolyElem)
		On Error GoTo 0

		pLegElem = Nothing
		pLegPolyElem = Nothing
		'=================================================================================
		'Dim pCircle1 As Polygon
		'Dim pCircle2 As Polygon
		'Dim pLegPoly As Polygon
		'    Set pPolygon = New Polygon
		'    If LegCount <= 0 Then
		'        pPolygon.AddPoint PointAlongPlane(ArcLeg.FromPoint, ArcLeg.FromPoint.M + 90.0, fBufferWidth)
		'        pPolygon.AddPoint PointAlongPlane(ArcLeg.FromPoint, ArcLeg.FromPoint.M - 90.0, fBufferWidth)
		'        Set pCircle1 = CreatePrjCircle(ArcLeg.FromPoint, fBufferWidth)
		'    Else
		'        pPolygon.AddPoint PointAlongPlane(LegList(LegCount - 1).pPtFlyBy, ArcLeg.FromPoint.M + 90.0, fBufferWidth)
		'        pPolygon.AddPoint PointAlongPlane(LegList(LegCount - 1).pPtFlyBy, ArcLeg.FromPoint.M - 90.0, fBufferWidth)
		'        Set pCircle1 = CreatePrjCircle(LegList(LegCount - 1).pPtFlyBy, fBufferWidth)
		'    End If
		'
		'    pPolygon.AddPoint PointAlongPlane(LegList(LegCount).pPtFlyBy, ArcLeg.FromPoint.M - 90.0, fBufferWidth)
		'    pPolygon.AddPoint PointAlongPlane(LegList(LegCount).pPtFlyBy, ArcLeg.FromPoint.M + 90.0, fBufferWidth)
		'    Set pCircle2 = CreatePrjCircle(LegList(LegCount).pPtFlyBy, fBufferWidth)
		'
		'    Set pTopoOper = pPolygon
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify
		'
		'    Set pLegPoly = pTopoOper.Union(pCircle1)
		'    Set pTopoOper = pLegPoly
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify
		'
		'    Set pPolygon = pTopoOper.Union(pCircle2)
		'    Set pTopoOper = pPolygon
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify

		LegList(LegCount).pLegPolygon = pLegProtectArea

		N = UBound(ObstacleList)
		'LegList(LegCount).VAL_DIST_VER_LOWER = fMOC
		J = -1
		If N >= 0 Then
			pRelation = pLegProtectArea
			ReDim LegObstList(N)
			For I = 0 To N
				If pRelation.Contains(ObstacleList(I).pPtPrj) Then
					J = J + 1
					LegObstList(J) = ObstacleList(I)
					LegObstList(J).MOC = fMOC
					LegObstList(J).ReqH = LegObstList(J).Height + fMOC

					'If LegList(LegCount).VAL_DIST_VER_LOWER < LegObstList(J).ReqH Then
					'LegList(LegCount).VAL_DIST_VER_LOWER = LegObstList(J).ReqH
					'End If
				End If
			Next I
		End If

		If J >= 0 Then
			ReDim Preserve LegObstList(J)
		Else
			'UPGRADE_ISSUE: Declaration type not supported: Array with upper bound less than zero. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="934BD4FF-1FF9-47BD-888F-D411E47E78FA"'
			ReDim LegObstList(-1)
		End If

		'=================================================================================
		ReportForm.AddSegment(LegObstList, (CurrPt.pPtPrj.Z))

		LegList(LegCount).pLegPolygonElement = DrawPolygon(pLegProtectArea, RGB(0, 255, 0))
		LegList(LegCount).pLegElement = DrawPolyLine(ArcLeg, 255, 2)

		LegList(LegCount).pLegPolygonElement.Locked = True
		LegList(LegCount).pLegElement.Locked = True

		If IsNumeric(TextBox104.Text) Then
			fTmp = CDbl(TextBox104.Text)
			If ComboBox102.SelectedIndex = 0 Then
				LegList(LegCount).VAL_VER_ANGLE = -RadToDeg(System.Math.Atan(0.01 * fTmp))
			Else
				LegList(LegCount).VAL_VER_ANGLE = -fTmp
			End If
		End If

		LegList(LegCount).NO_SEQ = LegCount + 1
		LegList(LegCount).UOM_SPEED = "KM/H" 'T_10
		LegList(LegCount).CODE_SPEED_REF = "IAS" 'T_4
		LegList(LegCount).CODE_TYPE_COURSE = "TT"

		LegList(LegCount).VAL_DIST = ArcLeg.Length
		LegList(LegCount).UOM_DIST_HORZ = "M"

		LegList(LegCount).CODE_TURN_VALID = False

		If Not OptionButton101.Checked Then
			LegList(LegCount).TurnDir = STR_DIR_TURN(ComboBox101.SelectedIndex + 1)
			LegList(LegCount).CODE_TURN_VALID = ComboBox101.SelectedIndex >= 0
			'Else
			'	LegList(LegCount).TurnDir = CodeDirectionTurn.EITHER
		End If

		LegList(LegCount).pNominalPoly = ArcLeg

		iSign1 = 0
		iSign2 = 0

		If LegList(LegCount).VAL_DIST_VER_UPPER <> NO_DATA_VALUE Then iSign1 = 1
		If LegList(LegCount).VAL_DIST_VER_LOWER <> NO_DATA_VALUE Then iSign2 = 1

		If iSign1 = 1 Then
			LegList(LegCount).CODE_DIST_VER_UPPER = "ALT"
			LegList(LegCount).UOM_DIST_VER_UPPER = "M" 'UCase((HeightConverter(HeightUnit).Unit))
		Else
			LegList(LegCount).CODE_DIST_VER_UPPER = ""
			LegList(LegCount).UOM_DIST_VER_UPPER = ""
		End If

		If iSign2 = 1 Then
			LegList(LegCount).CODE_DIST_VER_LOWER = "ALT"
			LegList(LegCount).UOM_DIST_VER_LOWER = "M" 'UCase((HeightConverter(HeightUnit).Unit))
		Else
			LegList(LegCount).CODE_DIST_VER_LOWER = ""
			LegList(LegCount).UOM_DIST_VER_LOWER = ""
		End If

		Select Case 2 * iSign1 + iSign2
			Case 0
				LegList(LegCount).CODE_DESCR_DIST_VER = "NO"
			Case 1
				LegList(LegCount).CODE_DESCR_DIST_VER = "LA"
			Case 2
				LegList(LegCount).CODE_DESCR_DIST_VER = "BH"
			Case 3
				If System.Math.Abs(LegList(LegCount).VAL_DIST_VER_UPPER - LegList(LegCount).VAL_DIST_VER_LOWER) <= 1 Then
					LegList(LegCount).CODE_DESCR_DIST_VER = "L"
				Else
					LegList(LegCount).CODE_DESCR_DIST_VER = "B"
				End If
		End Select

		'    CurrPt.Name = tmpCurPt.Name
		'    CurrPt.TypeCode = tmpCurPt.TypeCode
		'    CurrPt.TypeName = tmpCurPt.TypeName

		CurrPt.pPtPrj = ArcLeg.ToPoint
		CurrPt.pPtGeo = ToGeo(CurrPt.pPtPrj)
		CurrPt.pPtGeo.M = Dir2Azt(CurrPt.pPtPrj, CurrPt.pPtPrj.M)

		PrevH = CurrPt.pPtPrj.Z

		If lNextPage = 2 Then
			RefH = DeConvertHeight(CDbl(TextBox201.Text))
			CurrPt.pPtPrj.Z = RefH
			CurrPt.pPtGeo.Z = RefH
		End If

		LegList(LegCount).startPoint = CurrPt

		FullLength = FullLength + ArcLeg.Length

		OptionButton102.Enabled = True
		OptionButton103.Enabled = True

		LegCount = LegCount + 1
		N = UBound(LegList)
		If LegCount > N Then
			ReDim Preserve LegList(N + 30)
		End If

		ReportBtn.Enabled = LegCount > 0
		MainSSTab.SelectedIndex = 1
	End Sub

	Private Sub CheckBox203_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox203.CheckStateChanged
		Dim fDir As Double
		Dim txtBox As TextBox = IIf(CourseMode = 0, TextBoxChangeCourse_0, TextBoxChangeCourse_1)

		If CheckBox203.Checked Then
			fDir = CurrPt.pPtPrj.M
			CheckBoxChangeCourse.Checked = True
		Else
			If CheckBoxChangeCourse.Checked And IsNumeric(txtBox.Text) Then
				If CourseMode = 0 Then
					fDir = Azt2Dir(CurrPt.pPtGeo, CDbl(txtBox.Text))
				Else
					fDir = Azt2Dir(CurrPt.pPtGeo, CDbl(txtBox.Text) + CurrADHP.MagVar)
				End If
			Else
				fDir = CurrPt.pPtPrj.M
			End If
		End If

		FillComboBox205(fDir)
	End Sub

	Private Sub ComboBox205_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox205.SelectedIndexChanged
		Dim K As Integer
		Dim fDir As Double
		Dim fAzt As Double

		If ComboBox205.Tag = "2" Then Exit Sub
		If (Not OptionButton203.Checked) Or (ComboBox205.SelectedIndex < 0) Then Exit Sub

		K = ComboBox205.SelectedIndex
		sSIDProcName = ComboBox205.Text
		Label201_16.Text = GetNavTypeName(SelectedFIX(K).TypeCode) ' SelectedFIX(K).TypeName_Renamed
		CalcStraightCourse(0)

		If CheckBox203.Checked Then
			fDir = ReturnAngleInDegrees(CurrPt.pPtPrj, SelectedFIX(K).pPtPrj)
			fAzt = Dir2Azt(CurrPt.pPtPrj, fDir)
			TextBoxChangeCourse_0.Text = CStr(System.Math.Round(fAzt))
			TextBoxChangeCourse_1.Text = CStr(System.Math.Round(Modulus(fAzt - CurrADHP.MagVar)))
			ComboBox205.Tag = "2"
			ApplyNewDirection(fDir)
			ComboBox205.Tag = ""
		End If
	End Sub

	Private Sub ComboBox204_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox204.SelectedIndexChanged
		Dim K As Integer
		Dim fDir As Double
		Dim txtBox As TextBox = IIf(CourseMode = 0, TextBoxChangeCourse_0, TextBoxChangeCourse_1)

		K = ComboBox204.SelectedIndex
		If (Not OptionButton202.Checked) Or (K < 0) Then Exit Sub

		If CheckBoxChangeCourse.Checked And IsNumeric(txtBox.Text) Then
			fDir = CDbl(txtBox.Text)
			If CourseMode = 1 Then fDir = fDir + CurrADHP.MagVar
			fDir = Azt2Dir(CurrPt.pPtGeo, fDir)
		Else
			fDir = CurrPt.pPtPrj.M
		End If

		Dim Nav As NavaidType
		Nav = ComboBox204.SelectedItem

		TextBox206MinMax = CheckVOR_NDB(Nav, fDir)
		Label201_15.Text = GetNavTypeName(Nav.TypeCode)
		Label201_07.Text = My.Resources.str0301 & vbCrLf & My.Resources.str0302 & CStr(TextBox206MinMax.Min) & My.Resources.str0303 & CStr(TextBox206MinMax.Max)
		TextBox206_Validating(TextBox206, New System.ComponentModel.CancelEventArgs(False))
	End Sub

	Private Sub ComboBox002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox002.SelectedIndexChanged
		Dim K As Integer

		Label001_01.Text = ""
		K = ComboBox002.SelectedIndex
		If K < 0 Then Exit Sub
		Label001_01.Text = GetNavTypeName(WPTList(K).TypeCode)

		TextBox002.Text = CStr(System.Math.Round(ConvertHeight(WPTList(K).pPtGeo.Z, 0), eRoundMode.CEIL))
	End Sub

	Private Sub ComboBox101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox101.SelectedIndexChanged
		TurnDirection = 2 * ComboBox101.SelectedIndex - 1
	End Sub

	Private Sub ComboBox202_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox202.SelectedIndexChanged
		Dim K As Integer
		Dim fDir As Double

		K = ComboBox202.SelectedIndex
		If (K < 0) Or Not OptionButton201.Checked Then Return

		Dim txtBox As TextBox = IIf(CourseMode = 0, TextBoxChangeCourse_0, TextBoxChangeCourse_1)

		If CheckBoxChangeCourse.Checked And IsNumeric(txtBox.Text) Then
			fDir = CDbl(txtBox.Text)
			If CourseMode = 1 Then fDir = fDir + CurrADHP.MagVar
			fDir = Azt2Dir(CurrPt.pPtGeo, fDir)
		Else
			fDir = CurrPt.pPtPrj.M
		End If

		Dim Nav As NavaidType
		Nav = ComboBox202.SelectedItem
		Textbox205MinMax = GetDMERange(Nav.pPtPrj, fDir)

		If Textbox205MinMax.MinTo < 0 Then
			TextBox205.Text = CStr(ConvertDistance(Textbox205MinMax.MinFrom, eRoundMode.CEIL))
			SetReadOnly(ComboBox203, True)
			If ComboBox203.SelectedIndex = 1 Then
				ComboBox203_SelectedIndexChanged(ComboBox203, New System.EventArgs())
			Else
				ComboBox203.SelectedIndex = 1
			End If
		Else
			TextBox205.Text = CStr(ConvertDistance(Textbox205MinMax.MinTo, eRoundMode.CEIL))
			SetReadOnly(ComboBox203, False)
			If ComboBox203.SelectedIndex = 0 Then
				ComboBox203_SelectedIndexChanged(ComboBox203, New System.EventArgs())
			Else
				ComboBox203.SelectedIndex = 0
			End If
		End If
	End Sub

	'UPGRADE_WARNING: Event proc_cmbox.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub proc_cmbox_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles proc_cmbox.SelectedIndexChanged
		Dim K As Integer
		K = proc_cmbox.SelectedIndex
		If K < 0 Then Exit Sub

		CurrIAP = pIAPList.Item(K)
	End Sub

	Private Sub ReportBtn_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReportBtn.CheckStateChanged
		If Not bFormInitialised Then Return

		If ReportBtn.Checked Then
			ReportForm.Show(s_Win32Window)
		Else
			ReportForm.Hide()
		End If
	End Sub

	Private Sub HelpBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles HelpBtn.Click
		'    Dim c As New CreateData.CreateFIXCommand
		'    c.FuncForTAP Application
		'    Exit Sub
		'    HtmlHelp hWnd, App.HelpFile, HH_HELP_CONTEXT, HelpContextID
	End Sub

	Private Sub InfoBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles InfoBtn.Click
		InfoForm.ShowInfo(CurrADHP.MagVar)
	End Sub

	Private Sub ReturnBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReturnBtn.Click
		AddBtn.Enabled = False
		MainSSTab.SelectedIndex = 1
	End Sub

	'UPGRADE_WARNING: Event OptionButton002.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'

	Private Sub OptionButton002_CheckedChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton002_0.CheckedChanged, OptionButton002_1.CheckedChanged
		If Not sender.Checked Then Return
		CourseMode = CInt(sender.tag)

		If IsNumeric(Frame002.Tag) Then
			If CInt(Frame002.Tag) = CourseMode Then Return
		End If

		SetReadOnly(TextBox004_0, (CourseMode <> 0) Or OptionButton001_1.Checked)
		SetReadOnly(TextBox004_1, (CourseMode <> 1) Or OptionButton001_1.Checked)

		SetReadOnly(TextBoxChangeCourse_0, CourseMode <> 0)
		SetReadOnly(TextBoxChangeCourse_1, CourseMode <> 1)

		SetReadOnly(TextBox301_0, CourseMode <> 0)
		SetReadOnly(TextBox301_1, CourseMode <> 1)

		Frame002.Tag = sender.tag
	End Sub

	Private Sub MainSSTab_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MainSSTab.SelectedIndexChanged
		Static PreviousTab As Short = MainSSTab.SelectedIndex()

		If (MainSSTab.SelectedIndex = 1) And (NextBtn.Enabled) Then
			InitManevrePage()
			OptionButton102.Enabled = False
			OptionButton103.Enabled = False

			LegCount = 0
			If OptionButton101.Checked Then
				OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
			Else
				OptionButton101.Checked = True
			End If
		ElseIf (MainSSTab.SelectedIndex > 1) And (CalcBtn.Enabled) Then
			CalcBtn_ClickEvent(CalcBtn, New EventArgs())
		End If

		NextBtn.Enabled = MainSSTab.SelectedIndex = 0
		PrevBtn.Enabled = (MainSSTab.SelectedIndex = 1) And (LegCount <= 0)
		CalcBtn.Enabled = MainSSTab.SelectedIndex = 1

		AddBtn.Visible = MainSSTab.SelectedIndex > 1
		ReturnBtn.Visible = MainSSTab.SelectedIndex > 1

		'    InfoBtn.Enabled = (MainSSTab.Tab = 1) And (LegCount > 0)
		SaveBtn.Visible = (MainSSTab.SelectedIndex = 1) And (LegCount > 0)
		RemoveBtn.Visible = SaveBtn.Visible
		PreviousTab = MainSSTab.SelectedIndex()
	End Sub

	Private Sub PrevBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click
		On Error Resume Next
		If Not pLegElem Is Nothing Then pGraphics.DeleteElement(pLegElem)
		On Error GoTo 0

		pLegElem = Nothing

		MainSSTab.SelectedIndex = 0

		'    FocusStepCaption MainSSTab.Tab
	End Sub

	Private Sub NextBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
		MainSSTab.SelectedIndex = 1

		' 2007
		'    FocusStepCaption MainSSTab.Tab
	End Sub

	Private Sub ComboBox301_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox301.SelectedIndexChanged
		If ComboBox301.SelectedIndex < 0 Or OptionButton301.Checked Then Exit Sub
		Label301_05.Text = GetNavTypeName(ComboBox301FIXList(ComboBox301.SelectedIndex).TypeCode)
		CalcTurn()
		'    ApplyBtn.Enabled = (ComboBox301.ListCount > 0)
	End Sub

	Private Sub OptionButton001_0_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton001_0.CheckedChanged
		If eventSender.Checked Then
			EnableControl(ComboBox004, False)
			Label001_10.Enabled = False

			OptionButton002_0.Enabled = True
			OptionButton002_1.Enabled = True

			If CourseMode = 0 Then
				SetReadOnly(TextBox004_0, False)
			Else
				SetReadOnly(TextBox004_1, False)
			End If
		End If
	End Sub

	Private Sub OptionButton001_1_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton001_1.CheckedChanged
		If eventSender.Checked Then
			EnableControl(ComboBox004, True)
			Label001_10.Enabled = True

			OptionButton002_0.Enabled = False
			OptionButton002_1.Enabled = False

			If CourseMode = 0 Then
				SetReadOnly(TextBox004_0, True)
			Else
				SetReadOnly(TextBox004_1, True)
			End If

			ComboBox004_SelectedIndexChanged(ComboBox004, New System.EventArgs())
		End If
	End Sub

	'Private Sub OptionButton103_Click(Index As Integer)
	'    Call EnableControl(TextBox102, (Index = 0))
	'    Call EnableControl(TextBox103, (Index = 1))
	'    SetReadOnly TextBox102, (Index = 1)
	'    SetReadOnly TextBox103, (Index = 0)

	'    If Index = 0 Then
	'        TextBox102_Validate False
	'    ElseIf Index = 1 Then
	'        TextBox103_Validate False
	'    End If
	'End Sub

	Private Sub FormatFrame(ByRef pFrame() As GroupBox)
		Dim I As Integer
		Dim N As Integer

		N = pFrame.Length - 1
		For I = 1 To N
			pFrame(I).Left = pFrame(0).Left
			pFrame(I).Top = pFrame(0).Top

			'pFrame(I).Width = pFrame(0).Width
			'pFrame(I).Height = pFrame(0).Height
		Next I
	End Sub

	Private Sub ShowFrame(ByVal Index As Integer, ByRef pFrame() As GroupBox)
		Dim I As Integer
		Dim N As Integer

		N = pFrame.Length - 1

		For I = 0 To N
			pFrame(I).Visible = I = Index
		Next I
	End Sub

	'Private Function CalcNomPos(Xs As Double, Ys As Double, RefZ As Double, fGRD As Double, OCH As Double, d0 As Double, AheadBehindSide As Long, NearSide As Long, ptDMEprj As IPoint, ptSOCPrj As IPoint) As Double
	Private Function CalcNomPos(ByRef Xs As Double, ByRef Ys As Double, ByRef RefZ As Double, ByRef OCH As Double, ByRef d0 As Double, ByRef AheadBehindSide As Integer, ByRef NearSide As Integer, ByRef ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByRef ptSOCPrj As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim dNomPosDer As Double
		Dim dNomPosDME As Double
		Dim dOldPosDME As Double
		Dim hMax As Double
		Dim I As Integer

		I = 0
		dNomPosDME = d0 + NearSide * DME.MinimalError
		hMax = 0.0

		Do
			'        If Ys > dNomPosDME Then dNomPosDME = Ys
			dNomPosDer = Xs + AheadBehindSide * System.Math.Sqrt(dNomPosDME * dNomPosDME - Ys * Ys)
			'        hMax = dNomPosDer * fGRD + OCH - ptDMEprj.Z + RefZ
			hMax = OCH - ptDMEprj.Z + RefZ
			dOldPosDME = dNomPosDME
			dNomPosDME = (d0 + NearSide * DME.MinimalError) / (1.0 - NearSide * DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)))

			I = I + 1
			If I > 5 Then Exit Do
		Loop While System.Math.Abs(dOldPosDME - dNomPosDME) > 0.00001

		CalcNomPos = dNomPosDME
	End Function

	'Private Function CalcDMERange(ByVal ptSOCPrj As IPoint, ByVal ptBasePrj As Point, _
	'ByVal NomDir As Double, ByVal RefZ As Double, ByVal fGRD As Double, _
	'ByVal OCH As Double, ByVal ptDMEprj As IPoint, ByVal KKhMin As IPoint, _
	'ByVal KKhMax As IPoint) As Interval

	Private Function CalcDMERange(ByVal ptSOCPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ptBasePrj As ESRI.ArcGIS.Geometry.Point, ByVal NomDir As Double, ByVal RefZ As Double, ByVal OCH As Double, ByVal ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByVal KKhMin As ESRI.ArcGIS.Geometry.IPoint, ByVal KKhMax As ESRI.ArcGIS.Geometry.IPoint) As Interval

		Dim side As Integer
		Dim d0 As Double
		Dim d1 As Double
		Dim Ys As Double
		Dim Xs As Double

		Dim dist0 As Double
		Dim Dist1 As Double
		Dim LeftRightSide As Integer
		Dim AheadBehindSide As Integer

		AheadBehindSide = SideDef(KKhMin, NomDir + 90.0, ptDMEprj)
		LeftRightSide = SideDef(ptBasePrj, NomDir, ptDMEprj)

		Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90.0) * SideDef(ptSOCPrj, NomDir + 90.0, ptDMEprj)
		Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir)

		If AheadBehindSide < 0 Then
			If LeftRightSide > 0 Then
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin)

				side = SideDef(KKhMax, NomDir, ptDMEprj)
				If side < 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMax, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax)
				End If
			Else
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin)

				side = SideDef(KKhMax, NomDir, ptDMEprj)
				If side > 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMax, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax)
				End If
			End If
		Else
			If LeftRightSide > 0 Then
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax)
				'DrawPoint KKhMax.ToPoint, 255
				side = SideDef(KKhMin, NomDir, ptDMEprj)
				If side < 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMin, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin)
				End If
			Else
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax)

				side = SideDef(KKhMin, NomDir, ptDMEprj)
				If side > 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMin, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin)
				End If
			End If
		End If

		'    Dist0 = CalcNomPos(Xs, Ys, RefZ, fGRD, OCH, d0, AheadBehindSide, 1, ptDMEprj, ptSOCPrj)
		'    Dist1 = CalcNomPos(Xs, Ys, RefZ, fGRD, OCH, d1, AheadBehindSide, -1, ptDMEprj, ptSOCPrj)
		dist0 = CalcNomPos(Xs, Ys, RefZ, OCH, d0, AheadBehindSide, 1, ptDMEprj, ptSOCPrj)
		Dist1 = CalcNomPos(Xs, Ys, RefZ, OCH, d1, AheadBehindSide, -1, ptDMEprj, ptSOCPrj)

		'DrawPoint ptSOCPrj, RGB(0, 255, 255)
		'DrawPoint ptDMEprj, RGB(255, 0, 255)
		CalcDMERange.Left = dist0
		CalcDMERange.Right = Dist1
	End Function

	Private Function GetDMERange(ByRef ptCheckDME As ESRI.ArcGIS.Geometry.IPoint, ByRef fDir As Double) As MinMaxDist
		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer
		Dim L As Integer

		Dim LeftRightSide As Integer
		Dim AheadBehindMin As Integer
		Dim AheadBehindMax As Integer

		Dim fTmp As Double

		Dim IntrH As Interval
		Dim Intr23 As Interval
		Dim Intr55 As Interval
		Dim IntrRes() As Interval
		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval

		GetDMERange.MaxFrom = -1.0
		GetDMERange.MinFrom = -1.0
		GetDMERange.MaxTo = -1.0
		GetDMERange.MinTo = -1.0

		PtStart = CurrPt.pPtPrj

		CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir, ptMax)
		fTmp = ReturnDistanceInMeters(ptCheckDME, PtStart)

		If fTmp < DME.MinimalError Then
			ptMin = PointAlongPlane(ptCheckDME, fDir, DME.MinimalError)
		Else
			ptMin = PtStart
		End If

		LeftRightSide = SideDef(PtStart, fDir, ptCheckDME)

		AheadBehindMin = SideDef(ptMin, fDir + 90.0, ptCheckDME)
		AheadBehindMax = SideDef(ptMax, fDir + 90.0, ptCheckDME)

		If AheadBehindMin < 0 Then
			If fTmp > DME.Range Then
				CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir + 180.0, ptMin)
			End If
		Else
			If fTmp > DME.Range Then
				CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir + 180.0, ptMin)
			End If
		End If

		IntrH.Left = ReturnDistanceInMeters(PtStart, ptMin) '-DME.Range
		IntrH.Right = ReturnDistanceInMeters(PtStart, ptMax) 'DME.Range

		If LeftRightSide <> 0 Then
			ptMin23 = New ESRI.ArcGIS.Geometry.Point
			ptMax23 = New ESRI.ArcGIS.Geometry.Point
			Construct = ptMin23
			Construct.ConstructAngleIntersection(PtStart, DegToRad(fDir), ptCheckDME, DegToRad(fDir - LeftRightSide * DME.TP_div))
			Construct = ptMax23
			Construct.ConstructAngleIntersection(PtStart, DegToRad(fDir), ptCheckDME, DegToRad(fDir + LeftRightSide * DME.TP_div))
		Else
			ptMin23 = ptCheckDME
			ptMax23 = ptCheckDME
		End If

		Intr23.Left = Point2LineDistancePrj(PtStart, ptMin23, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptMin23)
		Intr23.Right = Point2LineDistancePrj(PtStart, ptMax23, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptMax23)

		IntrRes = IntervalsDifference(IntrH, Intr23)

		Dim Xs As Double
		Dim Ys As Double
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double

		Xs = Point2LineDistancePrj(ptCheckDME, PtStart, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptCheckDME)
		Ys = Point2LineDistancePrj(ptCheckDME, PtStart, fDir)

		fTmp = 1.0 / System.Math.Tan(DegToRad(DME.SlantAngle))
		fTmp = fTmp * fTmp

		'A = fGRD * fGRD - fTmp
		A = -fTmp
		'B = 2.0 * ((ptStart.Z - ptCheckDME.Z) * fGRD + Xs * fTmp)
		B = 2.0 * Xs * fTmp
		C = (PtStart.Z - ptCheckDME.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
		D = B * B - 4.0 * A * C

		If D > 0.0 Then
			D = System.Math.Sqrt(D)
			If A > 0 Then
				Intr55.Left = 0.5 * (-B - D) / A
				Intr55.Right = 0.5 * (-B + D) / A
			Else
				Intr55.Left = 0.5 * (-B + D) / A
				Intr55.Right = 0.5 * (-B - D) / A
			End If

			N = UBound(IntrRes)
			ReDim IntrRes1(-1)

			For I = 0 To N
				IntrRes2 = IntervalsDifference(IntrRes(I), Intr55)

				If UBound(IntrRes1) < 0 Then
					IntrRes1 = IntrRes2
				Else
					L = UBound(IntrRes1)
					M = UBound(IntrRes2)
					If M >= 0 Then
						ReDim Preserve IntrRes1(L + M + 1)

						For J = 0 To M
							IntrRes1(J + L + 1) = IntrRes2(J)
						Next J
					End If
				End If
			Next I

			IntrRes = IntrRes1
		End If

		N = UBound(IntrRes)

		I = 0
		If N >= 0 Then
			Do
				If IntrRes(I).Left = IntrRes(I).Right Then
					For J = I To N - 1
						IntrRes(J) = IntrRes(J + 1)
					Next J
					N = N - 1
				Else
					I = I + 1
				End If
			Loop While I < N - 1
		End If

		I = 0
		While I < N - 1
			If IntrRes(I).Right = IntrRes(I + 1).Left Then
				IntrRes(I).Right = IntrRes(I + 1).Right
				For J = I + 1 To N - 1
					IntrRes(J) = IntrRes(J + 1)
				Next J
				N = N - 1
			Else
				I = I + 1
			End If
		End While

		If N < 0 Then Exit Function

		ReDim IntrRes1(N)
		M = 0

		For I = 0 To N
			If System.Math.Abs(IntrRes(I).Left - IntrRes(I).Right) > 2 * DME.MinimalError Then
				ptNearD = PointAlongPlane(PtStart, fDir, IntrRes(I).Left)
				ptFarD = PointAlongPlane(PtStart, fDir, IntrRes(I).Right)
				'DrawPoint ptNearD, 0
				'DrawPoint ptFarD, 255
				'fTmp = ReturnDistanceInMeters(ptStart, ptCheckDME)

				IntrRes1(M) = CalcDMERange(PtStart, PtStart, fDir, RefH, PtStart.Z, ptCheckDME, ptNearD, ptFarD)
				If IntrRes1(M).Left < IntrRes1(M).Right Then
					IntrRes1(M).Tag = I
					M = M + 1
				End If
			End If
		Next I

		M = M - 1
		If M >= 0 Then
			ptNearD = PointAlongPlane(PtStart, fDir, IntrRes(0).Left)
			AheadBehindMin = SideDef(ptNearD, fDir + 90.0, ptCheckDME)
			If ((AheadBehindMin > 0) And (IntrRes1(M).Tag = 0)) Or ((AheadBehindMin < 0) And (IntrRes1(M).Tag = 1)) Then
				GetDMERange.MinTo = System.Math.Round(IntrRes1(0).Left + 0.4999999)
				GetDMERange.MaxTo = System.Math.Round(IntrRes1(0).Right - 0.4999999)
			Else
				GetDMERange.MinFrom = System.Math.Round(IntrRes1(0).Left + 0.4999999)
				GetDMERange.MaxFrom = System.Math.Round(IntrRes1(0).Right - 0.4999999)
			End If

			If M >= 1 Then
				ptNearD = PointAlongPlane(PtStart, fDir, IntrRes(1).Left)
				AheadBehindMin = SideDef(ptNearD, fDir + 90.0, ptCheckDME)
				If AheadBehindMin > 0 Then
					GetDMERange.MinFrom = System.Math.Round(IntrRes1(1).Left + 0.4999999)
					GetDMERange.MaxFrom = System.Math.Round(IntrRes1(1).Right - 0.4999999)
				Else
					GetDMERange.MinTo = System.Math.Round(IntrRes1(1).Left + 0.4999999)
					GetDMERange.MaxTo = System.Math.Round(IntrRes1(1).Right - 0.4999999)
				End If
			End If
		End If

	End Function

	Private Function CheckDME(ByRef ptCheckDME As ESRI.ArcGIS.Geometry.IPoint, ByRef fDir As Double) As Boolean
		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer
		Dim L As Integer

		Dim LeftRightSide As Integer
		Dim AheadBehindMin As Integer
		Dim AheadBehindMax As Integer

		Dim fTmp As Double

		Dim IntrH As Interval
		Dim Intr23 As Interval
		Dim Intr55 As Interval
		Dim IntrRes() As Interval
		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval

		CheckDME = False

		PtStart = CurrPt.pPtPrj

		fTmp = ReturnDistanceInMeters(ptCheckDME, PtStart)
		If fTmp < DME.MinimalError Then
			ptMin = PointAlongPlane(ptCheckDME, fDir, DME.MinimalError)
		Else
			ptMin = PtStart
		End If

		If CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir, ptMax) < 0.0 Then Exit Function

		LeftRightSide = SideDef(PtStart, fDir, ptCheckDME)

		AheadBehindMin = SideDef(ptMin, fDir + 90.0, ptCheckDME)
		AheadBehindMax = SideDef(ptMax, fDir + 90.0, ptCheckDME)

		If AheadBehindMin < 0 Then
			If fTmp > DME.Range Then
				CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir + 180.0, ptMin)
			End If
		Else
			If fTmp > DME.Range Then
				CircleVectorIntersect(ptCheckDME, DME.Range, PtStart, fDir + 180.0, ptMin)
			End If
		End If

		IntrH.Left = ReturnDistanceInMeters(PtStart, ptMin) '-DME.Range
		IntrH.Right = ReturnDistanceInMeters(PtStart, ptMax) 'DME.Range

		If LeftRightSide <> 0 Then
			ptMin23 = New ESRI.ArcGIS.Geometry.Point
			ptMax23 = New ESRI.ArcGIS.Geometry.Point
			Construct = ptMin23
			Construct.ConstructAngleIntersection(PtStart, DegToRad(fDir), ptCheckDME, DegToRad(fDir - LeftRightSide * DME.TP_div))
			Construct = ptMax23
			Construct.ConstructAngleIntersection(PtStart, DegToRad(fDir), ptCheckDME, DegToRad(fDir + LeftRightSide * DME.TP_div))
		Else
			ptMin23 = ptCheckDME
			ptMax23 = ptCheckDME
		End If

		Intr23.Left = Point2LineDistancePrj(PtStart, ptMin23, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptMin23)
		Intr23.Right = Point2LineDistancePrj(PtStart, ptMax23, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptMax23)

		IntrRes = IntervalsDifference(IntrH, Intr23)

		Dim Xs As Double
		Dim Ys As Double
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double

		Xs = Point2LineDistancePrj(ptCheckDME, PtStart, fDir + 90.0) * SideDef(PtStart, fDir + 90.0, ptCheckDME)
		Ys = Point2LineDistancePrj(ptCheckDME, PtStart, fDir)

		fTmp = 1.0 / System.Math.Tan(DegToRad(DME.SlantAngle))
		fTmp = fTmp * fTmp

		'    A = fGRD * fGRD - fTmp
		A = -fTmp
		'    B = 2.0 * ((ptStart.Z - ptCheckDME.Z) * fGRD + Xs * fTmp)
		B = 2.0 * Xs * fTmp
		C = (PtStart.Z - ptCheckDME.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
		D = B * B - 4.0 * A * C

		If D > 0.0 Then
			D = System.Math.Sqrt(D)
			If A > 0 Then
				Intr55.Left = 0.5 * (-B - D) / A
				Intr55.Right = 0.5 * (-B + D) / A
			Else
				Intr55.Left = 0.5 * (-B + D) / A
				Intr55.Right = 0.5 * (-B - D) / A
			End If

			N = UBound(IntrRes)

			ReDim IntrRes1(-1)

			For I = 0 To N
				IntrRes2 = IntervalsDifference(IntrRes(I), Intr55)

				If UBound(IntrRes1) < 0 Then
					IntrRes1 = IntrRes2
				Else
					L = UBound(IntrRes1)
					M = UBound(IntrRes2)
					If M >= 0 Then
						ReDim Preserve IntrRes1(L + M + 1)

						For J = 0 To M
							IntrRes1(J + L + 1) = IntrRes2(J)
						Next J
					End If
				End If
			Next I

			IntrRes = IntrRes1
		End If

		N = UBound(IntrRes)

		I = 0
		If N >= 0 Then
			Do
				If IntrRes(I).Left = IntrRes(I).Right Then
					For J = I To N - 1
						IntrRes(J) = IntrRes(J + 1)
					Next J
					N = N - 1
				Else
					I = I + 1
				End If
			Loop While I < N - 1
		End If

		I = 0
		While I < N - 1
			If IntrRes(I).Right = IntrRes(I + 1).Left Then
				IntrRes(I).Right = IntrRes(I + 1).Right
				For J = I + 1 To N - 1
					IntrRes(J) = IntrRes(J + 1)
				Next J
				N = N - 1
			Else
				I = I + 1
			End If
		End While

		If N < 0 Then Exit Function

		ReDim IntrRes1(N)
		M = 0

		For I = 0 To N
			If System.Math.Abs(IntrRes(I).Left - IntrRes(I).Right) > 2 * DME.MinimalError Then
				ptNearD = PointAlongPlane(PtStart, fDir, IntrRes(I).Left)
				ptFarD = PointAlongPlane(PtStart, fDir, IntrRes(I).Right)

				IntrRes1(M) = CalcDMERange(PtStart, PtStart, fDir, RefH, PtStart.Z, ptCheckDME, ptNearD, ptFarD)
				If IntrRes1(M).Left < IntrRes1(M).Right Then M = M + 1
				'            IntrRes1(M).Tag = I
			End If
		Next I

		CheckDME = M - 1 >= 0
		'    M = M - 1
		'    If M >= 0 Then
		'        CheckDME.MinFrom = Round(IntrRes1(0).Left + 0.4999999)
		'        CheckDME.MaxFrom = Round(IntrRes1(0).Right - 0.4999999)
		'        If M >= 1 Then
		'            CheckDME.MinTo = Round(IntrRes1(1).Left + 0.4999999)
		'            CheckDME.MaxTo = Round(IntrRes1(1).Right - 0.4999999)
		'        End If
		'    End If
	End Function

	'UPGRADE_WARNING: Event OptionButton201.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub OptionButton201_CheckedChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton201.CheckedChanged, OptionButton202.CheckedChanged, OptionButton203.CheckedChanged, OptionButton204.CheckedChanged

		If Not sender.Checked Then Return

		Dim Index As Integer = CInt(sender.tag)

		ShowFrame(Index, frStCourse)

		Select Case Index
			Case 0
				If ComboBox202.SelectedIndex < 0 Then
					ComboBox202.SelectedIndex = 0
				Else
					ComboBox202_SelectedIndexChanged(ComboBox202, New System.EventArgs())
				End If
			Case 1
				If ComboBox204.SelectedIndex < 0 Then
					ComboBox204.SelectedIndex = 0
				Else
					ComboBox204_SelectedIndexChanged(ComboBox204, New System.EventArgs())
				End If
			Case 2
				CheckBox203.Enabled = LegCount > 0
				ComboBox205_SelectedIndexChanged(ComboBox205, New System.EventArgs())

				'            If CheckBox203.Value <> 1 Then
				'                CheckBox203.Value = 1
				'            Else
				'                CheckBox203_Click
				'            End If
			Case 3
				TextBox207_Validating(TextBox207, New System.ComponentModel.CancelEventArgs(False))
		End Select
	End Sub

	'UPGRADE_WARNING: Event ComboBox203.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub ComboBox203_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox203.SelectedIndexChanged
		Dim fMin As Double
		Dim fMax As Double

		If (Not OptionButton201.Checked) Then Exit Sub
		ToolTip1.SetToolTip(ComboBox203, ComboBox203.Text)

		If ComboBox203.SelectedIndex = 0 Then
			fMin = ConvertDistance(Textbox205MinMax.MinTo, eRoundMode.CEIL)
			fMax = ConvertDistance(Textbox205MinMax.MaxTo, eRoundMode.FLOOR)
		Else
			fMin = ConvertDistance(Textbox205MinMax.MinFrom, eRoundMode.CEIL)
			fMax = ConvertDistance(Textbox205MinMax.MaxFrom, eRoundMode.FLOOR)
		End If

		Label201_06.Text = My.Resources.str0301 & vbCrLf & My.Resources.str0302 & CStr(fMin) & My.Resources.str0303 & CStr(fMax)
		TextBox205_Validating(TextBox205, New System.ComponentModel.CancelEventArgs(False))
	End Sub

	Private Sub OptionButton301_CheckedChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton301.CheckedChanged
		If Not bFormInitialised Then Return
		If Not sender.Checked Then Return
		'Dim Index As Short = OptionButton301.GetIndex(sender)
		EnableControl(ComboBox301, False)
		Label301_04.Enabled = False
		Label301_05.Enabled = False

		If CourseMode = 0 Then
			SetReadOnly(TextBox301_0, False)
		Else
			SetReadOnly(TextBox301_1, False)
		End If

		CalcTurn()
	End Sub

	Private Sub OptionButton302_CheckedChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton302.CheckedChanged
		If Not bFormInitialised Then Return

		If Not sender.Checked Then Return
		'Dim Index As Short = OptionButton301.GetIndex(sender)
		EnableControl(ComboBox301, True)
		Label301_04.Enabled = True
		Label301_05.Enabled = True

		If CourseMode = 0 Then
			SetReadOnly(TextBox301_0, True)
		Else
			SetReadOnly(TextBox301_1, True)
		End If

		ComboBox301_SelectedIndexChanged(ComboBox301, New System.EventArgs())

		CalcTurn()
	End Sub

	Private Sub OptionButton101_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton101.CheckedChanged, OptionButton102.CheckedChanged, OptionButton103.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim Index As Integer = CInt(eventSender.tag)
		lNextPage = Index + 2

		If Index = 0 Then
			MainSSTab.TabPages.Item(2).Visible = True
			MainSSTab.TabPages.Item(3).Visible = False
			MainSSTab.TabPages.Item(4).Visible = False

			Label101_00.Enabled = False
			EnableControl(ComboBox101, False)

			Label101_01.Enabled = True
			EnableControl(TextBox104, True)
			EnableControl(ComboBox102, True)
		ElseIf Index = 1 Then
			MainSSTab.TabPages.Item(2).Visible = False
			MainSSTab.TabPages.Item(3).Visible = True
			MainSSTab.TabPages.Item(4).Visible = False

			Label101_00.Enabled = True
			EnableControl(ComboBox101, True)

			Label101_01.Enabled = False
			EnableControl(TextBox104, False)
			EnableControl(ComboBox102, False)
		ElseIf Index = 2 Then
			MainSSTab.TabPages.Item(2).Visible = False
			MainSSTab.TabPages.Item(3).Visible = False
			MainSSTab.TabPages.Item(4).Visible = True

			Label101_00.Enabled = True
			EnableControl(ComboBox101, True)

			Label101_01.Enabled = False
			EnableControl(TextBox104, False)
			EnableControl(ComboBox102, False)
		End If
	End Sub

	Private Sub RemoveBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles RemoveBtn.Click
		Dim I As Integer
		Dim K As Integer

		RemoveForm.ShowDialog(Me)

		If RemoveForm.Result = CRemoveForm.RemoveResult.Cancel Then	'-----------------Cancel
			Exit Sub
		ElseIf RemoveForm.Result = CRemoveForm.RemoveResult.Last Then	'-----------------Remove Last
			K = LegCount - 1
			On Error Resume Next
			If (Not LegList(K).pLegElement Is Nothing) Then pGraphics.DeleteElement(LegList(K).pLegElement)

			LegList(K).pLegElement = Nothing

			If (Not LegList(K).pLegPolygonElement Is Nothing) Then pGraphics.DeleteElement(LegList(K).pLegPolygonElement)

			LegList(K).pLegPolygonElement = Nothing
			On Error GoTo 0

			ReportForm.DeleteSegment(1)
			InfoForm.NewSegment()
			LegCount = K

			'        If LegCount = MaptLegIndex Then MaptLegIndex = -1
			CurrPt = LegList(LegCount).startPoint
		ElseIf RemoveForm.Result = CRemoveForm.RemoveResult.All Then  '-----------------Romove All
			On Error Resume Next
			If (Not pLegElem Is Nothing) Then pGraphics.DeleteElement(pLegElem)

			pLegElem = Nothing
			For I = 0 To UBound(LegList)
				If (Not LegList(I).pLegElement Is Nothing) Then pGraphics.DeleteElement(LegList(I).pLegElement)

				LegList(I).pLegElement = Nothing

				If (Not LegList(I).pLegPolygonElement Is Nothing) Then pGraphics.DeleteElement(LegList(I).pLegPolygonElement)
				LegList(I).pLegPolygonElement = Nothing
			Next I
			On Error GoTo 0

			ReportForm.DeleteSegment(LegCount)
			InfoForm.NewSegment()
			LegCount = 0
			OptionButton101.Checked = True
			OptionButton102.Enabled = False
			OptionButton103.Enabled = False

			'        MaptLegIndex = -1
			'        FromParameter_ToManevrePage
		End If

		If LegCount = 0 Then
			SaveBtn.Visible = False
			RemoveBtn.Visible = False
			PrevBtn.Enabled = True
			OptionButton101.Checked = True
			OptionButton102.Enabled = False
			OptionButton103.Enabled = False
		End If

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
	End Sub


	Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String)
		ReportForm.SaveProtocol(RepFileName, RepFileTitle)
	End Sub

	Private Sub SaveBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim RepFileName As String = ""
		Dim RepFileTitle As String = ""
		'Dim TrackPoints() As ReportPoint

		If ShowSaveDialog(RepFileName, RepFileTitle) Then SaveProtocol(RepFileName, RepFileTitle)
		If SaveProcedure() Then Me.Close()
	End Sub

	Private Function SRVectoringInitialLeg(ByVal Index As Integer, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As InitialLeg
		'Dim fTmp As Double
		Dim fDir As Double
		Dim fDistToNav As Double
		'Dim fAltitude As Double

		Dim Angle As Double
		Dim fDist As Double
		Dim fCourseDir As Double

		Dim fAltitudeMin As Double
		'Dim PostFixTolerance As Double
		'Dim PriorFixTolerance As Double

		Dim pInitialLeg As InitialLeg
		Dim pSegmentLeg As SegmentLeg

		Dim pDistanceIndication As DistanceIndication
		Dim pDistance As ValDistance

		Dim pFIXSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pAngleIndication As AngleIndication
		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pDistanceVertical As ValDistanceVertical
		Dim pSpeed As ValSpeed

		Dim pStartPoint As TerminalSegmentPoint
		Dim mHUomDistance As UomDistance
		Dim mVUomDistance As UomDistanceVertical
		Dim mUomSpeed As UomSpeed

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim uomDistHorzTab() As UomDistance

		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomSpeedTab() As UomSpeed
		Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidType
		Dim SttIntersectNav As NavaidType
		Dim EndIntersectNav As NavaidType
		Dim ptStart As ESRI.ArcGIS.Geometry.IPoint
		Dim ptEnd As ESRI.ArcGIS.Geometry.IPoint
		Dim startPoint As WPT_FIXType
		Dim endPoint As WPT_FIXType

		uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER}
		uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mHUomDistance = uomDistHorzTab(DistanceUnit)
		mVUomDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		GuidNav = LegList(Index).GuidNav
		SttIntersectNav = LegList(Index).InterNav
		startPoint = LegList(Index).startPoint
		ptStart = startPoint.pPtPrj

		pInitialLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		pInitialLeg.Approach = pProcedure.GetFeatureRef()
		pInitialLeg.AircraftCategory.Add(IsLimitedTo)
		'pInitialLeg.ApproachLegType = ApproachLegType.InitialLeg
		pSegmentLeg = pInitialLeg

		pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL

		pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN
		pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK
		pSegmentLeg.EndConditionDesignator = Aim.Enums.CodeSegmentTermination.INTERCEPT

		pSegmentLeg.TurnDirection = LegList(Index).TurnDir
		pSegmentLeg.LegTypeARINC = LegList(Index).LegTypeARINC

		If GuidNav.TypeCode = eNavaidType.DME Then
			pSegmentLeg.LegPath = CodeTrajectory.ARC
		Else
			pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT
		End If

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =
		'Dim pDuration As IDuration
		'    Set pDuration = New Duration
		'    pDuration.Uom = UomDuration_MIN
		'    pDuration.Value = CDbl(TextBox0602.Text)
		'    Set pSegmentLeg.Duration = pDuration

		'    pSegmentLeg.EndConditionDesignator =
		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		'        pSegmentLeg.CourseDirection =
		'        pSegmentLeg.ProcedureTransition
		'        pSegmentLeg.StartPoint

		'Dim pAIXMCurve As AIXMCurve
		'Dim pGMLPolyline As GMLPolyline
		'Dim pGMLPart As GMLPart
		'Dim pGMLPoint As GMLPoint
		'=======================================================================================
		'If GuidNav.TypeCode = eNavaidType.DME Then
		'	Dim pArcCentre As TerminalSegmentPoint

		'	pSegmentLeg.LegPath = CodeTrajectory.ARC
		'	pSegmentLeg.LegTypeARINC = CodeSegmentPath.AF

		'	If SideDef(ptStart, LegList(Index).OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CCW
		'	End If

		'	pArcCentre = New TerminalSegmentPoint()
		'	pArcCentre.Waypoint = False
		'	pArcCentre.PointChoice = GuidNav.GetSignificantPoint()
		'	'pArcCentre.PointChoice.NavaidSystem = GuidNav.GetSignificantPoint.NavaidSystem
		'	pSegmentLeg.ArcCentre = pArcCentre

		'	pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
		'Else
		'	fCourseDir = LegList(Index).OutDir

		'	pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT
		'	pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF
		'	If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.FROM
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.TO
		'	End If
		'	pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
		'End If
		'=======================================================================================
		'pInt32 = New Int32Class
		'pInt32.Value = Index + 1
		'pSegmentLeg.SeqNumberARINC = pInt32

		'    If LegList(Index).TrackType = TrackTypeStraight Then
		'        fTmp = Dir2Azt(LegList(Index).pPtPrj, LegList(Index).OutDir)
		'    Else
		'        fTmp = Dir2Azt(LegList(Index).GuidNav.pPtPrj, ReturnAngleInDegrees(LegList(Index).GuidNav.pPtPrj, LegList(Index).pPtPrj))
		'    End If

		'    If LegList(Index).TrackType = TrackTypeStraight Then
		'        fTmp = Dir2Azt(LegList(Index).pPtPrj, LegList(Index).OutDir)
		'    Else
		'        fTmp = ReturnAngleInDegrees(LegList(Index).GuidNav.pPtPrj, LegList(Index).pPtPrj)
		'        fTmp = Dir2Azt(LegList(Index).GuidNav.pPtPrj, fTmp)
		'    End If

		pSegmentLeg.Course = LegList(Index).VAL_COURSE

		'=======================================================================================

		pSegmentLeg.BankAngle = LegList(Index).VAL_BANK_ANGLE
		'=======================================================================================
		'    N = LegCount - 1
		'        If I < N Then
		'            fTmp = ConvertHeight(LegList(I + 1).pPtPrj.Z, 2)
		'            itmX.SubItems(7) = Str(Round(fTmp, 1))
		'        End If
		'
		'        fTmp = ConvertHeight(LegList(I).pPtPrj.Z, 2)
		'        itmX.SubItems(8) = Str(Round(fTmp, 1))

		'    If Index = N Then
		'        fTmp = ConvertHeight(LegList(Index).pPtPrj.Z, 2)
		'    Else
		'    End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mVUomDistance
		pDistanceVertical.Value = Math.Round(ConvertHeight(LegList(Index).VAL_DIST_VER_LOWER), 2)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'===
		'    If LegList(LegCount).VAL_DIST_VER_UPPER <> NO_DATA_VALUE Then
		'        Set pDistance = New Distance
		'        pDistance.Uom = mVUomDistance
		'
		'        'fTmp = ConvertHeight(LegList(Index).MaxAlt, 2)
		'        pDistance.Value = CStr(LegList(LegCount).VAL_DIST_VER_UPPER)
		'        Set pSegmentLeg.UpperLimitAltitude = pDistance
		'    End If


		'=======================================================================================
		pDistance = New ValDistance
		pDistance.Uom = mHUomDistance
		pDistance.Value = ConvertDistance(LegList(Index).VAL_DIST)
		pSegmentLeg.Length = pDistance

		'===
		If LegList(Index).VAL_VER_ANGLE <> NO_DATA_VALUE Then
			'pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(LegList(Index).PDG))
			pSegmentLeg.VerticalAngle = LegList(Index).VAL_VER_ANGLE
		End If
		'===
		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed

		pSpeed.Value = ConvertSpeed(210.0 * 1.852 / 3.6)
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = CodeSpeedReference.TAS

		' Start Point ========================
		If Not (GuidNav.pFeature Is Nothing) Then
			pGuidNavSignPt = New SignificantPoint()
			pGuidNavSignPt.NavaidSystem = GuidNav.pFeature.GetFeatureRef()
		End If

		If Index > 0 Then
			pStartPoint = pEndPoint
		Else
			pStartPoint = New TerminalSegmentPoint()
			pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP

			pSegmentPoint = pStartPoint
			'pSegmentPoint.FlyOver = False
			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False

			'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
			'        pTerminalSegmentPoint.LeadDME =            ??????????
			'        pTerminalSegmentPoint.LeadRadial =         ??????????

			ptTmp = ToGeo(startPoint.pPtPrj)
			'============================================================

			'pSegmentPoint.FlyOver = False
			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False
			'=============================================================
			pPointReference = New PointReference()
			pInterNavSignPt = New SignificantPoint()
			pInterNavSignPt.NavaidSystem = SttIntersectNav.pFeature.GetFeatureRef()
			' Start Point ==================================================

			If SttIntersectNav.Tag = -1 Then
				pFIXSignPt = pInterNavSignPt
				pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
			Else
				pFixDesignatedPoint = CreateDesignatedPoint(ptStart, , LegList(Index).CourseDir) 'fCourseDir
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()
				If Not (GuidNav.pFeature Is Nothing) Then
					If GuidNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptStart)
						fAltitudeMin = ptStart.Z - GuidNav.pPtPrj.Z
						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, GuidNav.GetSignificantPoint)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					Else
						fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart)
						Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pAngleUse = New AngleUse()
						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = True

						pPointReference.FacilityAngle.Add(pAngleUse)
					End If
				End If

				' ========================

				If SttIntersectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(SttIntersectNav.pPtPrj, ptStart)
					fAltitudeMin = ptStart.Z - SttIntersectNav.pPtPrj.Z
					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
					pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, pInterNavSignPt)
					pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					pPointReference.Role = CodeReferenceRole.RAD_DME
				Else
					pAngleUse = New AngleUse
					fDir = ReturnAngleInDegrees(SttIntersectNav.pPtPrj, ptStart)

					Angle = Modulus(Dir2Azt(SttIntersectNav.pPtPrj, fDir) - SttIntersectNav.MagVar, 360.0)
					pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
					pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
					pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
					pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
					pAngleUse.AlongCourseGuidance = False

					pPointReference.FacilityAngle.Add(pAngleUse)
					pPointReference.Role = CodeReferenceRole.INTERSECTION
				End If
			End If
			'=================================================================
			'Dim pTolerArea As ESRI.ArcGIS.Geometry.IPolygon
			'pTolerArea = LegList(Index).pFixPoly

			'If Not pTolerArea Is Nothing Then
			'	PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

			'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			'	pDistanceSigned.Uom = mUomHDistance
			'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance))
			'	pPointReference.PriorFixTolerance = pDistanceSigned

			'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			'	pDistanceSigned.Uom = mUomHDistance
			'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance))

			'	pPointReference.PostFixTolerance = pDistanceSigned
			'	pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
			'End If
			'=================================================================

			pStartPoint.FacilityMakeup.Add(pPointReference)
			pSegmentPoint.PointChoice = pFIXSignPt
		End If

		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ========================

		' EndPoint ========================
		pEndPoint = Nothing

		If Index < LegCount - 1 Then
			EndIntersectNav = LegList(Index + 1).InterNav 'IntersectNav
			endPoint = LegList(Index + 1).startPoint
			ptEnd = endPoint.pPtPrj

			pEndPoint = New TerminalSegmentPoint
			'        pEndPoint.IndicatorFACF =      ??????????
			'        pEndPoint.LeadDME =            ??????????
			'        pEndPoint.LeadRadial =         ??????????

			pSegmentPoint = pEndPoint

			If LegList(Index).CODE_TURN_VALID Then
				'pSegmentPoint.FlyOver = False
				pEndPoint.Role = CodeProcedureFixRole.TP
			Else
				'pSegmentPoint.FlyOver = True
				pEndPoint.Role = CodeProcedureFixRole.SDF
			End If

			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False

			' Indication ======================================================================
			Dim bOnNav As Boolean
			bOnNav = False

			pPointReference = New PointReference()
			If (Not EndIntersectNav.pFeature Is Nothing) Then
				pInterNavSignPt = New SignificantPoint()
				pInterNavSignPt.NavaidSystem = EndIntersectNav.pFeature.GetFeatureRef()
				If ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd) < 1.0 Then EndIntersectNav.Tag = -1
				If EndIntersectNav.Tag = -1 Then
					bOnNav = True
					pFIXSignPt = pInterNavSignPt
				End If
			End If

			If Not bOnNav Then
				pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, , fCourseDir)
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				If Not (GuidNav.pFeature Is Nothing) Then
					If GuidNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptEnd)
						fAltitudeMin = ptEnd.Z - GuidNav.pPtPrj.Z
						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, GuidNav.GetSignificantPoint)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					Else
						fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd)
						Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pAngleUse = New AngleUse()
						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = True

						pPointReference.FacilityAngle.Add(pAngleUse)
					End If
				End If
			End If
			' End Of Indication ======================================================================

			If Not EndIntersectNav.pFeature Is Nothing Then

				If EndIntersectNav.Tag = -1 Then
					pFIXSignPt = pInterNavSignPt
					pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
				Else
					If EndIntersectNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd)
						fAltitudeMin = ptEnd.Z - EndIntersectNav.pPtPrj.Z

						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, pInterNavSignPt)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
						pPointReference.Role = CodeReferenceRole.RAD_DME
					Else
						pAngleUse = New AngleUse
						fDir = ReturnAngleInDegrees(EndIntersectNav.pPtPrj, ptEnd)

						Angle = Modulus(Dir2Azt(EndIntersectNav.pPtPrj, fDir) - EndIntersectNav.MagVar, 360.0)
						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = False

						pPointReference.FacilityAngle.Add(pAngleUse)
						pPointReference.Role = CodeReferenceRole.INTERSECTION
					End If
				End If
				'=================================================================
				'Dim pEndTolerArea As ESRI.ArcGIS.Geometry.IPolygon
				'pEndTolerArea = LegList(Index + 1).pFixPoly

				'If Not pEndTolerArea Is Nothing Then
				'	PriorPostFixTolerance(pEndTolerArea, ptEnd, fCourseDir, PriorFixTolerance, PostFixTolerance)

				'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				'	pDistanceSigned.Uom = mHUomDistance
				'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.rmNERAEST))
				'	pPointReference.PriorFixTolerance = pDistanceSigned

				'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				'	pDistanceSigned.Uom = mHUomDistance
				'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.rmNERAEST))

				'	pPointReference.PostFixTolerance = pDistanceSigned
				'	pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pEndTolerArea))
				'End If
				'=================================================================
				pSegmentPoint.PointChoice = pFIXSignPt
			End If

			pEndPoint.FacilityMakeup.Add(pPointReference)
			' End of EndPoint ========================
		End If

		pSegmentLeg.EndPoint = pEndPoint

		' Trajectory =====================================================
		Dim I As Integer
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLocation As Aran.Geometries.Point

		pCurve = New Curve
		pPolyline = LegList(Index).pNominalPoly

		For J = pPolyline.GeometryCount - 1 To 0 Step -1
			'For J = 0 To pPolyline.GeometryCount - 1
			pPath = pPolyline.Geometry(J)
			pLineStringSegment = New LineString

			For I = 0 To pPath.PointCount - 1
				pLocation = ESRIPointToARANPoint(ToGeo(pPath.Point(I)))
				pLineStringSegment.Add(pLocation)
			Next

			pCurve.Geo.Add(pLineStringSegment)
		Next
		pSegmentLeg.Trajectory = pCurve
		' Trajectory =============================================

		Return pInitialLeg
		' END =====================================================
	End Function

	Private Function SRVectoringArrivalLeg(ByVal Index As Integer, ByVal pProcedure As StandardInstrumentArrival, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ArrivalLeg
		'Dim fTmp As Double
		Dim fDir As Double
		Dim fDistToNav As Double
		'Dim fAltitude As Double

		Dim Angle As Double
		Dim fDist As Double
		Dim fCourseDir As Double

		Dim fAltitudeMin As Double
		'Dim PostFixTolerance As Double
		'Dim PriorFixTolerance As Double

		Dim pArrivalLeg As ArrivalLeg
		Dim pSegmentLeg As SegmentLeg

		Dim pDistanceIndication As DistanceIndication
		Dim pDistance As ValDistance

		Dim pFIXSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pAngleIndication As AngleIndication
		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pDistanceVertical As ValDistanceVertical
		Dim pSpeed As ValSpeed

		Dim pStartPoint As TerminalSegmentPoint
		Dim mHUomDistance As UomDistance
		Dim mVUomDistance As UomDistanceVertical
		Dim mUomSpeed As UomSpeed

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim uomDistHorzTab() As UomDistance

		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomSpeedTab() As UomSpeed
		Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidType
		Dim SttIntersectNav As NavaidType
		Dim EndIntersectNav As NavaidType
		Dim ptStart As ESRI.ArcGIS.Geometry.IPoint
		Dim ptEnd As ESRI.ArcGIS.Geometry.IPoint
		Dim startPoint As WPT_FIXType
		Dim endPoint As WPT_FIXType

		uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER}
		uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mHUomDistance = uomDistHorzTab(DistanceUnit)
		mVUomDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		GuidNav = LegList(Index).GuidNav
		SttIntersectNav = LegList(Index).InterNav
		startPoint = LegList(Index).startPoint
		ptStart = startPoint.pPtPrj

		pArrivalLeg = pObjectDir.CreateFeature(Of ArrivalLeg)()
		pArrivalLeg.Arrival = pProcedure.GetFeatureRef()
		pArrivalLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pArrivalLeg

		pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL

		pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN
		pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK
		pSegmentLeg.EndConditionDesignator = Aim.Enums.CodeSegmentTermination.INTERCEPT

		pSegmentLeg.TurnDirection = LegList(Index).TurnDir
		pSegmentLeg.LegTypeARINC = LegList(Index).LegTypeARINC

		If GuidNav.TypeCode = eNavaidType.DME Then
			pSegmentLeg.LegPath = CodeTrajectory.ARC
		Else
			pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT
		End If

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =
		'Dim pDuration As IDuration
		'    Set pDuration = New Duration
		'    pDuration.Uom = UomDuration_MIN
		'    pDuration.Value = CDbl(TextBox0602.Text)
		'    Set pSegmentLeg.Duration = pDuration

		'    pSegmentLeg.EndConditionDesignator =
		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		'        pSegmentLeg.CourseDirection =
		'        pSegmentLeg.ProcedureTransition
		'        pSegmentLeg.StartPoint

		'Dim pAIXMCurve As AIXMCurve
		'Dim pGMLPolyline As GMLPolyline
		'Dim pGMLPart As GMLPart
		'Dim pGMLPoint As GMLPoint
		'=======================================================================================
		'If GuidNav.TypeCode = eNavaidType.DME Then
		'	Dim pArcCentre As TerminalSegmentPoint

		'	pSegmentLeg.LegPath = CodeTrajectory.ARC
		'	pSegmentLeg.LegTypeARINC = CodeSegmentPath.AF

		'	If SideDef(ptStart, LegList(Index).OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CCW
		'	End If

		'	pArcCentre = New TerminalSegmentPoint()
		'	pArcCentre.Waypoint = False
		'	pArcCentre.PointChoice = GuidNav.GetSignificantPoint()
		'	'pArcCentre.PointChoice.NavaidSystem = GuidNav.GetSignificantPoint.NavaidSystem
		'	pSegmentLeg.ArcCentre = pArcCentre

		'	pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
		'Else
		'	fCourseDir = LegList(Index).OutDir

		'	pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT
		'	pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF
		'	If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.FROM
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.TO
		'	End If
		'	pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
		'End If
		'=======================================================================================
		'pInt32 = New Int32Class
		'pInt32.Value = Index + 1
		'pSegmentLeg.SeqNumberARINC = pInt32

		'    If LegList(Index).TrackType = TrackTypeStraight Then
		'        fTmp = Dir2Azt(LegList(Index).pPtPrj, LegList(Index).OutDir)
		'    Else
		'        fTmp = Dir2Azt(LegList(Index).GuidNav.pPtPrj, ReturnAngleInDegrees(LegList(Index).GuidNav.pPtPrj, LegList(Index).pPtPrj))
		'    End If

		'    If LegList(Index).TrackType = TrackTypeStraight Then
		'        fTmp = Dir2Azt(LegList(Index).pPtPrj, LegList(Index).OutDir)
		'    Else
		'        fTmp = ReturnAngleInDegrees(LegList(Index).GuidNav.pPtPrj, LegList(Index).pPtPrj)
		'        fTmp = Dir2Azt(LegList(Index).GuidNav.pPtPrj, fTmp)
		'    End If

		pSegmentLeg.Course = LegList(Index).VAL_COURSE

		'=======================================================================================

		pSegmentLeg.BankAngle = LegList(Index).VAL_BANK_ANGLE
		'=======================================================================================
		'    N = LegCount - 1
		'        If I < N Then
		'            fTmp = ConvertHeight(LegList(I + 1).pPtPrj.Z, 2)
		'            itmX.SubItems(7) = Str(Round(fTmp, 1))
		'        End If
		'
		'        fTmp = ConvertHeight(LegList(I).pPtPrj.Z, 2)
		'        itmX.SubItems(8) = Str(Round(fTmp, 1))

		'    If Index = N Then
		'        fTmp = ConvertHeight(LegList(Index).pPtPrj.Z, 2)
		'    Else
		'    End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mVUomDistance
		pDistanceVertical.Value = Math.Round(ConvertHeight(LegList(Index).VAL_DIST_VER_LOWER), 4)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'===
		'    If LegList(LegCount).VAL_DIST_VER_UPPER <> NO_DATA_VALUE Then
		'        Set pDistance = New Distance
		'        pDistance.Uom = mVUomDistance
		'
		'        'fTmp = ConvertHeight(LegList(Index).MaxAlt, 2)
		'        pDistance.Value = CStr(LegList(LegCount).VAL_DIST_VER_UPPER)
		'        Set pSegmentLeg.UpperLimitAltitude = pDistance
		'    End If


		'=======================================================================================
		pDistance = New ValDistance
		pDistance.Uom = mHUomDistance
		pDistance.Value = ConvertDistance(LegList(Index).VAL_DIST)
		pSegmentLeg.Length = pDistance

		'===
		If LegList(Index).VAL_VER_ANGLE <> NO_DATA_VALUE Then
			'pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(LegList(Index).PDG))
			pSegmentLeg.VerticalAngle = LegList(Index).VAL_VER_ANGLE
		End If
		'===
		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed

		pSpeed.Value = ConvertSpeed(210.0 * 1.852 / 3.6)
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = CodeSpeedReference.TAS

		' Start Point ========================
		If Not (GuidNav.pFeature Is Nothing) Then
			pGuidNavSignPt = New SignificantPoint()
			pGuidNavSignPt.NavaidSystem = GuidNav.pFeature.GetFeatureRef()
		End If

		If Index > 0 Then
			pStartPoint = pEndPoint
		Else
			pStartPoint = New TerminalSegmentPoint()
			pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP

			pSegmentPoint = pStartPoint
			'pSegmentPoint.FlyOver = False
			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False

			'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
			'        pTerminalSegmentPoint.LeadDME =            ??????????
			'        pTerminalSegmentPoint.LeadRadial =         ??????????

			ptTmp = ToGeo(startPoint.pPtPrj)
			'============================================================

			'pSegmentPoint.FlyOver = False
			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False
			'=============================================================
			pPointReference = New PointReference()
			pInterNavSignPt = New SignificantPoint()
			pInterNavSignPt.NavaidSystem = SttIntersectNav.pFeature.GetFeatureRef()
			' Start Point ==================================================

			If SttIntersectNav.Tag = -1 Then
				pFIXSignPt = pInterNavSignPt
				pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
			Else
				pFixDesignatedPoint = CreateDesignatedPoint(ptStart, , LegList(Index).CourseDir) 'fCourseDir
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()
				If Not (GuidNav.pFeature Is Nothing) Then
					If GuidNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptStart)
						fAltitudeMin = ptStart.Z - GuidNav.pPtPrj.Z
						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, GuidNav.GetSignificantPoint)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					Else
						fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart)
						Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pAngleUse = New AngleUse()
						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = True

						pPointReference.FacilityAngle.Add(pAngleUse)
					End If
				End If

				' ========================

				If SttIntersectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(SttIntersectNav.pPtPrj, ptStart)
					fAltitudeMin = ptStart.Z - SttIntersectNav.pPtPrj.Z
					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
					pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, pInterNavSignPt)
					pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					pPointReference.Role = CodeReferenceRole.RAD_DME
				Else
					pAngleUse = New AngleUse
					fDir = ReturnAngleInDegrees(SttIntersectNav.pPtPrj, ptStart)

					Angle = Modulus(Dir2Azt(SttIntersectNav.pPtPrj, fDir) - SttIntersectNav.MagVar, 360.0)
					pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
					pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
					pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
					pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
					pAngleUse.AlongCourseGuidance = False

					pPointReference.FacilityAngle.Add(pAngleUse)
					pPointReference.Role = CodeReferenceRole.INTERSECTION
				End If
			End If
			'=================================================================
			'Dim pTolerArea As ESRI.ArcGIS.Geometry.IPolygon
			'pTolerArea = LegList(Index).pFixPoly

			'If Not pTolerArea Is Nothing Then
			'	PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

			'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			'	pDistanceSigned.Uom = mUomHDistance
			'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.rmNERAEST))
			'	pPointReference.PriorFixTolerance = pDistanceSigned

			'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			'	pDistanceSigned.Uom = mUomHDistance
			'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.rmNERAEST))

			'	pPointReference.PostFixTolerance = pDistanceSigned
			'	pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
			'End If
			'=================================================================

			pStartPoint.FacilityMakeup.Add(pPointReference)
			pSegmentPoint.PointChoice = pFIXSignPt
		End If

		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ========================

		' EndPoint ========================
		pEndPoint = Nothing

		If Index < LegCount - 1 Then
			EndIntersectNav = LegList(Index + 1).InterNav 'IntersectNav
			endPoint = LegList(Index + 1).startPoint
			ptEnd = endPoint.pPtPrj

			pEndPoint = New TerminalSegmentPoint
			'        pEndPoint.IndicatorFACF =      ??????????
			'        pEndPoint.LeadDME =            ??????????
			'        pEndPoint.LeadRadial =         ??????????

			pSegmentPoint = pEndPoint

			If LegList(Index).CODE_TURN_VALID Then
				'pSegmentPoint.FlyOver = False
				pEndPoint.Role = CodeProcedureFixRole.TP
			Else
				'pSegmentPoint.FlyOver = True
				pEndPoint.Role = CodeProcedureFixRole.SDF
			End If

			pSegmentPoint.RadarGuidance = False
			pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT
			pSegmentPoint.Waypoint = False

			' Indication ======================================================================
			Dim bOnNav As Boolean
			bOnNav = False

			pPointReference = New PointReference()
			If (Not EndIntersectNav.pFeature Is Nothing) Then
				pInterNavSignPt = New SignificantPoint()
				pInterNavSignPt.NavaidSystem = EndIntersectNav.pFeature.GetFeatureRef()
				If ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd) < 1.0 Then EndIntersectNav.Tag = -1
				If EndIntersectNav.Tag = -1 Then
					bOnNav = True
					pFIXSignPt = pInterNavSignPt
				End If
			End If

			If Not bOnNav Then
				pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, , fCourseDir)
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				If Not (GuidNav.pFeature Is Nothing) Then
					If GuidNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptEnd)
						fAltitudeMin = ptEnd.Z - GuidNav.pPtPrj.Z
						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, GuidNav.GetSignificantPoint)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					Else
						fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd)
						Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pAngleUse = New AngleUse()
						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = True

						pPointReference.FacilityAngle.Add(pAngleUse)
					End If
				End If
			End If
			' End Of Indication ======================================================================

			If Not EndIntersectNav.pFeature Is Nothing Then

				If EndIntersectNav.Tag = -1 Then
					pFIXSignPt = pInterNavSignPt
					pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
				Else
					If EndIntersectNav.TypeCode = eNavaidType.DME Then
						fDistToNav = ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd)
						fAltitudeMin = ptEnd.Z - EndIntersectNav.pPtPrj.Z

						fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
						pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist), mHUomDistance, pInterNavSignPt)
						pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

						pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
						pPointReference.Role = CodeReferenceRole.RAD_DME
					Else
						pAngleUse = New AngleUse
						fDir = ReturnAngleInDegrees(EndIntersectNav.pPtPrj, ptEnd)

						Angle = Modulus(Dir2Azt(EndIntersectNav.pPtPrj, fDir) - EndIntersectNav.MagVar, 360.0)
						pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
						pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
						pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
						pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

						pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
						pAngleUse.AlongCourseGuidance = False

						pPointReference.FacilityAngle.Add(pAngleUse)
						pPointReference.Role = CodeReferenceRole.INTERSECTION
					End If
				End If
				'=================================================================
				'Dim pEndTolerArea As ESRI.ArcGIS.Geometry.IPolygon
				'pEndTolerArea = LegList(Index + 1).pFixPoly

				'If Not pEndTolerArea Is Nothing Then
				'	PriorPostFixTolerance(pEndTolerArea, ptEnd, fCourseDir, PriorFixTolerance, PostFixTolerance)

				'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				'	pDistanceSigned.Uom = mHUomDistance
				'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance))
				'	pPointReference.PriorFixTolerance = pDistanceSigned

				'	pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				'	pDistanceSigned.Uom = mHUomDistance
				'	pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance))

				'	pPointReference.PostFixTolerance = pDistanceSigned
				'	pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pEndTolerArea))
				'End If
				'=================================================================
				pSegmentPoint.PointChoice = pFIXSignPt
			End If

			pEndPoint.FacilityMakeup.Add(pPointReference)
			' End of EndPoint ========================
		End If

		pSegmentLeg.EndPoint = pEndPoint

		' Trajectory =====================================================
		Dim I As Integer
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLocation As Aran.Geometries.Point

		pCurve = New Curve
		pPolyline = LegList(Index).pNominalPoly

		For J = pPolyline.GeometryCount - 1 To 0 Step -1
			'For J = 0 To pPolyline.GeometryCount - 1
			pPath = pPolyline.Geometry(J)
			pLineStringSegment = New LineString

			For I = 0 To pPath.PointCount - 1
				pLocation = ESRIPointToARANPoint(ToGeo(pPath.Point(I)))
				pLineStringSegment.Add(pLocation)
			Next

			pCurve.Geo.Add(pLineStringSegment)
		Next
		pSegmentLeg.Trajectory = pCurve
		' Trajectory =============================================

		Return pArrivalLeg
		' END =====================================================
	End Function

	Private Function SaveProcedure() As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim sProcName As String

		Dim pTransition As ProcedureTransition
		Dim IsLimitedTo As AircraftCharacteristic

		Dim featureRef As Aim.DataTypes.FeatureRef
		Dim featureRefObject As Aim.Objects.FeatureRefObject
		'Dim pGuidanceServiceChose As New GuidanceService
		'Dim pLandingTakeoffAreaCollection As LandingTakeoffAreaCollection

		Dim pEndPoint As TerminalSegmentPoint = Nothing
		Dim pSegmentLeg As SegmentLeg
		Dim ptl As ProcedureTransitionLeg

		Dim pProcedure As Procedure

		pObjectDir.ClearAllFeatures()

		'Dim pBoolean As IBoolean
		'IsLimitedTo ==========================================================================
		IsLimitedTo = New AircraftCharacteristic
		'IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.ALL

		Select Case iCategory
			Case 0
				IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.A
			Case 1
				IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.B
			Case 2
				IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.C
			Case 3
				IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.D
			Case 4
				IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.E
		End Select

		' Procedure =================================================================================================
		'pProcedure = New InstrumentApproachProcedure()
		'pProcedure.Assign(CurrIAP)

		pProcedure = DBModule.pObjectDir.CreateFeature(Of StandardInstrumentArrival)() 'InstrumentApproachProcedure

		pProcedure.AircraftCharacteristic.Add(IsLimitedTo)

		'sProcName = CurrIAP.Designator
		sProcName = LegList(0).startPoint.Name + " 1A"
		pProcedure.Name = sProcName

		pProcedure.RNAV = False
		pProcedure.FlightChecked = False
		pProcedure.CodingStandard = Aran.Aim.Enums.CodeProcedureCodingStandard.PANS_OPS
		pProcedure.DesignCriteria = Aran.Aim.Enums.CodeDesignStandard.PANS_OPS

		''    pProcedure.Note =
		''    pProcedure.ProtectsSafeAltitudeAreaId =

		featureRefObject = New Objects.FeatureRefObject
		featureRef = New FeatureRef
		featureRef.Identifier = CurrADHP.pAirportHeliport.Identifier
		featureRefObject.Feature = featureRef
		pProcedure.AirportHeliport.Add(featureRefObject)

		' TimeSlice =================================================================================================
		pProcedure.TimeSlice.CorrectionNumber = pProcedure.TimeSlice.CorrectionNumber + 1
		pProcedure.TimeSlice.ValidTime.BeginPosition = DateTime.Now
		pProcedure.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA
		If pProcedure.TimeSlice.SequenceNumber = 1 Then
			pProcedure.TimeSlice.FeatureLifetime.BeginPosition = pProcedure.TimeSlice.ValidTime.BeginPosition
		End If

		'Transition ==========================================================================
		pTransition = New ProcedureTransition()
		'pTransition.DepartureRunwayTransition = CurrIAP.Landing
		pTransition.Type = Aim.Enums.CodeProcedurePhase.APPROACH

		'pTransition.Description =
		'pTransition.ID =
		'pTransition.LegList = pSegmentLegList
		'pTransition.Procedure =
		'Set pString = New StringClass
		'pString.Value = RWY_ID
		'Set pTransition.RunwayDirectionId = pString
		'pTransition.TransitionId = TextBox0???.Text

		'Legs ======================================================================================================
		N = LegCount
		pEndPoint = Nothing

		For I = 0 To N - 1
			'pSegmentLeg = SRVectoringInitialLeg(I, pProcedure, IsLimitedTo, pEndPoint)
			pSegmentLeg = SRVectoringArrivalLeg(I, pProcedure, IsLimitedTo, pEndPoint)
			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = I + 1
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()

			pTransition.TransitionLeg.Add(ptl)
		Next I

		pProcedure.FlightTransition.Add(pTransition)

		' Store =================================================================================================

		Try
			pObjectDir.SetFeature(pProcedure)
			'pObjectDir.SetRootFeatureType(FeatureType.InstrumentApproachProcedure)
			pObjectDir.SetRootFeatureType(FeatureType.StandardInstrumentArrival)

			SaveProcedure = pObjectDir.Commit({ _
			   FeatureType.DesignatedPoint, _
			   FeatureType.AngleIndication, _
			   FeatureType.DistanceIndication, _
			   FeatureType.StandardInstrumentDeparture, _
			   FeatureType.StandardInstrumentArrival, _
			   FeatureType.InstrumentApproachProcedure, _
			   FeatureType.ArrivalFeederLeg, _
			   FeatureType.ArrivalLeg, _
			   FeatureType.DepartureLeg, _
			   FeatureType.FinalLeg, _
			   FeatureType.InitialLeg, _
			   FeatureType.IntermediateLeg, _
			   FeatureType.MissedApproachLeg})

			gAranEnv.RefreshAllAimLayers()
		Catch ex As Exception
			MsgBox("Error on commit." + vbCrLf + ex.Message)
			Return False
		End Try

		Return True
	End Function

	Private Sub TextBox002_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox002.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		TextBoxFloat(KeyAscii, (TextBox002.Text))
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox002_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox002.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		NextBtn.Enabled = IsNumeric(TextBox002.Text)
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox004_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox004_0.KeyPress, TextBox004_1.KeyPress
		Dim KeyAscii As Short = Asc(e.KeyChar)

		If KeyAscii = 13 Then
			TextBox004_Validating(sender, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, CType(sender, TextBox).Text)
		End If

		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub TextBox004_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox004_0.Validating, TextBox004_1.Validating

		Dim I As Integer
		Dim fTmp As Double
		Dim txtBox As TextBox = sender
		Dim Index As Integer = CInt(txtBox.Tag)

		I = IIf(OptionButton002_0.Checked, 0, 1)

		If (Index <> I) Or Not IsNumeric(txtBox.Text) Then Return

		If OptionButton002_0.Checked Then
			fTmp = Modulus(CDbl(TextBox004_0.Text) - CurrADHP.MagVar)
			TextBox004_1.Text = CStr(System.Math.Round(fTmp, 2))
		Else
			fTmp = Modulus(CDbl(TextBox004_1.Text) + CurrADHP.MagVar)
			TextBox004_0.Text = CStr(System.Math.Round(fTmp, 2))
		End If
	End Sub

	Private Sub TextBox104_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox104.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox104_Validating(TextBox104, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox104.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox104_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox104.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim fGrad As Double
		Dim fNewVal As Double
		If Not IsNumeric(TextBox104.Text) Then GoTo EventExitSub

		fGrad = CDbl(TextBox104.Text)

		fNewVal = fGrad

		If fGrad < 4 Then fGrad = 4
		If fGrad > 8 Then fGrad = 8

		If fNewVal <> fGrad Then TextBox104.Text = CStr(fGrad)
EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox205_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox205.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox205_Validating(TextBox205, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox205.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox206_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox206.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox206_Validating(TextBox206, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox206.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox207_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox207.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox207_Validating(TextBox207, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox207.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBoxChangeCourse_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoxChangeCourse_0.KeyPress, TextBoxChangeCourse_1.KeyPress
		Dim KeyAscii As Short = Asc(e.KeyChar)

		If KeyAscii = 13 Then
			TextBoxChangeCourse_Validating(sender, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, sender.Text)
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub TextBox301_KeyPress(ByVal sender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox301_0.KeyPress, TextBox301_1.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

		If KeyAscii = 13 Then
			TextBox301_Validating(sender, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, sender.Text)
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox302_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox302.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox302_Validating(TextBox302, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox302.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox303_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox303.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox303_Validating(TextBox303, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox303.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox401_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox401.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox401_Validating(TextBox401, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox401.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox402_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox402.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox402_Validating(TextBox402, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox402.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox404_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles _TextBox404_0.KeyPress, _TextBox404_1.KeyPress
		Dim KeyAscii As Short = Asc(e.KeyChar)

		If KeyAscii = 13 Then
			TextBox404_Validating(sender, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, CType(sender, TextBox).Text)
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub TextBox201_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox201.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then
			TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox201.Text))
		End If
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox201_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox201.Validating
		Dim fHeight As Double
		Dim fNewVal As Double

		If Not IsNumeric(TextBox201.Text) Then Return

		fHeight = DeConvertHeight(CDbl(TextBox201.Text))
		fNewVal = fHeight

		If fHeight < LegList(LegCount).VAL_DIST_VER_LOWER Then
			fHeight = LegList(LegCount).VAL_DIST_VER_LOWER
		ElseIf (LegList(LegCount).VAL_DIST_VER_UPPER <> NO_DATA_VALUE) And (fHeight > LegList(LegCount).VAL_DIST_VER_UPPER) Then
			fHeight = LegList(LegCount).VAL_DIST_VER_UPPER
		End If

		If fNewVal <> fHeight Then TextBox201.Text = CStr(ConvertHeight(fHeight))
	End Sub

	Private Sub TextBox205_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox205.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim fDist As Double
		Dim fNewVal As Double
		If Not OptionButton201.Checked Or Not IsNumeric(TextBox205.Text) Then GoTo EventExitSub

		fDist = DeConvertDistance(CDbl(TextBox205.Text))

		fNewVal = fDist
		If ComboBox203.SelectedIndex = 0 Then
			If fDist < Textbox205MinMax.MinTo Then fDist = Textbox205MinMax.MinTo
			If fDist > Textbox205MinMax.MaxTo Then fDist = Textbox205MinMax.MaxTo
		Else
			If fDist < Textbox205MinMax.MinFrom Then fDist = Textbox205MinMax.MinFrom
			If fDist > Textbox205MinMax.MaxFrom Then fDist = Textbox205MinMax.MaxFrom
		End If
		If fNewVal <> fDist Then TextBox205.Text = CStr(ConvertDistance(fDist))

		CalcStraightCourse(fDist)
EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox206_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox206.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim fDirec As Double
		Dim fNewVal As Double
		If Not OptionButton202.Checked Or Not IsNumeric(TextBox206.Text) Then GoTo EventExitSub
		If TextBox206MinMax.Max < 0.0 Then GoTo EventExitSub

		fDirec = CDbl(TextBox206.Text)
		fNewVal = fDirec
		If Not AngleInSector(fDirec, TextBox206MinMax.Min, TextBox206MinMax.Max) Then
			If AnglesSideDef(TextBox206MinMax.Max + 0.5 * (TextBox206MinMax.Max + TextBox206MinMax.Min) + 180.0, fDirec) < 0 Then
				fDirec = TextBox206MinMax.Min
			Else
				fDirec = TextBox206MinMax.Max
			End If
		End If
		If fNewVal <> fDirec Then TextBox206.Text = CStr(fDirec)

		fDirec = Azt2Dir(CurrPt.pPtGeo, fDirec)
		CalcStraightCourse(fDirec)
EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox207_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox207.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim fTmp As Double
		If (Not OptionButton204.Checked) Or Not IsNumeric(TextBox207.Text) Then GoTo EventExitSub
		fTmp = DeConvertDistance(CDbl(TextBox207.Text))
		If fTmp > 200000.0 Then
			fTmp = 200000.0
			TextBox207.Text = CStr(ConvertDistance(fTmp))
		End If

		If fTmp < 0.0 Then
			fTmp = 0.0
			TextBox207.Text = CStr(0.0)
		End If

		CalcStraightCourse(fTmp)
EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox301_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox301_0.Validating, TextBox301_1.Validating
		Dim fPrevCourse As Double
		Dim fTmp As Double
		Dim LeftTurn As Double
		Dim RightTurn As Double

		Dim txtBox As TextBox = sender
		Dim Index As Integer = CInt(txtBox.Tag)

		If (Index <> CourseMode) Or OptionButton302.Checked Then Return

		If Not IsNumeric(txtBox.Text) Then Return

		If CourseMode = 0 Then
			fTmp = Modulus(CDbl(txtBox.Text))
		Else
			fTmp = Modulus(CDbl(txtBox.Text) + CurrADHP.MagVar)
		End If

		fPrevCourse = CurrPt.pPtGeo.M
		'    If Not AngleInSector(fTmp, fPrevCourse - MaxTurnAngle, fPrevCourse + MaxTurnAngle) Then
		LeftTurn = 0.5 * (TurnDirection - 1) * MaxTurnAngle
		RightTurn = 0.5 * (TurnDirection + 1) * MaxTurnAngle

		If Not AngleInSector(fTmp, fPrevCourse + LeftTurn, fPrevCourse + RightTurn) Then
			If AnglesSideDef(fPrevCourse + 0.5 * (LeftTurn + RightTurn) + 180.0, fTmp) < 0 Then
				fTmp = Modulus(fPrevCourse + LeftTurn)
			Else
				fTmp = Modulus(fPrevCourse + RightTurn)
			End If
		End If

		TextBox301_0.Text = CStr(System.Math.Round(fTmp))
		TextBox301_1.Text = CStr(System.Math.Round(Modulus(fTmp - CurrADHP.MagVar)))

		CalcTurn()
	End Sub

	Private Sub TextBox302_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox302.Validating
		Dim OldVal As Double
		Dim fBank As Double
		Dim fTAS As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox302.Text) Then Return

		OldVal = fBankLimit
		fTmp = CDbl(TextBox302.Text)
		fBankLimit = fTmp

		If fBankLimit < 15 Then fBankLimit = 15
		If fBankLimit > 25 Then fBankLimit = 25

		If fBankLimit <> fTmp Then TextBox302.Text = CStr(fBankLimit)

		If fBankLimit <> OldVal Then
			fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
			'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

			fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
			If fBank > fBankLimit Then fBank = fBankLimit

			OldVal = fTurnR
			fTurnR = Bank2Radius(fBank, fTAS)
			TextBox304.Text = CStr(ConvertDistance(fTurnR))

			If OldVal <> fTurnR Then CalcTurn()
		End If
	End Sub

	Private Sub TextBox303_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox303.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim OldVal As Double
		Dim fBank As Double
		Dim fTAS As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox303.Text) Then GoTo EventExitSub

		OldVal = fIAS
		fTmp = DeConvertSpeed(CDbl(TextBox303.Text))
		fIAS = fTmp

		If fIAS < cViafMin.Values(iCategory) Then fIAS = cViafMin.Values(iCategory)
		If fIAS > cViafMax.Values(iCategory) Then fIAS = cViafMax.Values(iCategory)

		If fIAS <> fTmp Then
			TextBox303.Text = CStr(ConvertSpeed(fIAS))
		End If

		If fIAS <> OldVal Then
			fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
			'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

			fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
			If fBank > fBankLimit Then fBank = fBankLimit

			OldVal = fTurnR
			fTurnR = Bank2Radius(fBank, fTAS)
			TextBox304.Text = CStr(ConvertDistance(fTurnR))

			If OldVal <> fTurnR Then
				CalcTurn()
			End If
		End If

EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox401_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox401.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim OldVal As Double
		Dim fBank As Double
		Dim fTAS As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox401.Text) Then GoTo EventExitSub

		OldVal = fBankLimit
		fTmp = CDbl(TextBox401.Text)
		fBankLimit = fTmp

		If fBankLimit < 15 Then fBankLimit = 15
		If fBankLimit > 25 Then fBankLimit = 25

		If fBankLimit <> fTmp Then
			TextBox401.Text = CStr(fBankLimit)
		End If

		If fBankLimit <> OldVal Then
			fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
			'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

			fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
			If fBank > fBankLimit Then fBank = fBankLimit

			OldVal = fTurnR
			fTurnR = Bank2Radius(fBank, fTAS)
			TextBox403.Text = CStr(ConvertDistance(fTurnR))

			If OldVal <> fTurnR Then
				CalcInterception()
			End If
		End If

EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox402_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox402.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim OldVal As Double
		Dim fBank As Double
		Dim fTAS As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox402.Text) Then GoTo EventExitSub

		OldVal = fIAS
		fTmp = DeConvertSpeed(CDbl(TextBox402.Text))
		fIAS = fTmp

		If fIAS < cViafMin.Values(iCategory) Then fIAS = cViafMin.Values(iCategory)
		If fIAS > cViafMax.Values(iCategory) Then fIAS = cViafMax.Values(iCategory)

		If fIAS <> fTmp Then
			TextBox402.Text = CStr(ConvertSpeed(fIAS))
		End If

		If fIAS <> OldVal Then
			fTAS = IAS2TAS(3.6 * fIAS, (CurrPt.pPtPrj.Z), fAirportISAtC.Value)
			'fTAS = IAS2TAS(3.6 * cVva.Values(Category), CurrADHP.pPtGeo.Z + 300, CurrADHP.ISAtC)

			fBank = RadToDeg(System.Math.Atan(0.003 * PI * fTAS / 6.355))
			If fBank > fBankLimit Then fBank = fBankLimit

			OldVal = fTurnR
			fTurnR = Bank2Radius(fBank, fTAS)
			TextBox403.Text = CStr(ConvertDistance(fTurnR))

			If OldVal <> fTurnR Then
				CalcInterception()
			End If
		End If

EventExitSub:
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub TextBox404_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles _TextBox404_0.Validating, _TextBox404_1.Validating
		Dim K As Integer
		Dim fTmp As Double
		Dim fTmpFrom As Double
		Dim fTmpTo As Double
		Dim txtBox404 As TextBox = sender

		Dim Index As Integer = CInt(txtBox404.Tag)

		'If (Not OptionButton401(1).Value) Or (ComboBox402.ListCount = 0) Then Exit Sub

		If (Index <> CourseMode) Or Not IsNumeric(txtBox404.Text) Then Return
		If ComboBox402.SelectedIndex < 0 Then Return
		K = ComboBox402.SelectedIndex

		fTmpFrom = Dir2Azt(CurrPt.pPtPrj, ComboBox402Intervals(K).Max)
		fTmpTo = Dir2Azt(CurrPt.pPtPrj, ComboBox402Intervals(K).Min)

		fTmp = CDbl(txtBox404.Text)
		If CourseMode = 1 Then fTmp = fTmp + CurrADHP.MagVar

		If Not AngleInSector(fTmp, fTmpFrom, fTmpTo) Then
			If AnglesSideDef(fTmpFrom + 0.5 * (fTmpTo - fTmpFrom) + 180.0, fTmp) < 0 Then
				fTmp = fTmpFrom
			Else
				fTmp = fTmpTo
			End If
		End If

		_TextBox404_0.Text = CStr(Modulus(System.Math.Round(fTmp + 0.49999)))
		_TextBox404_1.Text = CStr(Modulus(System.Math.Round(fTmp - CurrADHP.MagVar + 0.49999)))

		CalcInterception()
	End Sub

	Private Sub ApplyNewDirection(ByRef Direction As Double)
		Dim K As Integer

		FillComboBox202(Direction)
		FillComboBox204(Direction)
		FillComboBox205(Direction)

		OptionButton201.Enabled = ComboBox202.Items.Count > 0
		OptionButton202.Enabled = ComboBox204.Items.Count > 0
		OptionButton203.Enabled = ComboBox205.Items.Count > 0

		If OptionButton201.Checked Then
			K = 0
		ElseIf OptionButton202.Checked Then
			K = 1
		ElseIf OptionButton203.Checked Then
			K = 2
		Else
			K = 3
		End If

		'    K = 0
		If OptionButton201.Checked And Not OptionButton201.Enabled Then K = K + 1
		If OptionButton202.Checked And Not OptionButton202.Enabled Then K = K + 1
		If OptionButton203.Checked And Not OptionButton203.Enabled Then K = K + 1

		Select Case K
			Case 0
				If OptionButton201.Checked Then
					OptionButton201_CheckedChanged(OptionButton201, New System.EventArgs())
				Else
					OptionButton201.Checked = True
				End If
			Case 1
				If OptionButton202.Checked Then
					OptionButton201_CheckedChanged(OptionButton202, New System.EventArgs())
				Else
					OptionButton202.Checked = True
				End If
			Case 2
				If OptionButton203.Checked Then
					OptionButton201_CheckedChanged(OptionButton203, New System.EventArgs())
				Else
					OptionButton203.Checked = True
				End If
			Case 3
				If OptionButton204.Checked Then
					OptionButton201_CheckedChanged(OptionButton204, New System.EventArgs())
				Else
					OptionButton204.Checked = True
				End If
		End Select
	End Sub

	Private Sub TextBoxChangeCourse_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxChangeCourse_0.Validating, TextBoxChangeCourse_1.Validating
		Dim fTmp As Double
		Dim fPrevCourse As Double
		Dim fDir As Double
		Dim txtBox As TextBox = sender
		Dim Index As Integer = CInt(txtBox.Tag)

		If (Index <> CourseMode) Or Not IsNumeric(txtBox.Text) Then Return

		fTmp = Modulus(CDbl(txtBox.Text))
		If CourseMode = 1 Then fTmp = fTmp + CurrADHP.MagVar

		fPrevCourse = CurrPt.pPtGeo.M

		If Not AngleInSector(fTmp, fPrevCourse - MaxTurnAngle, fPrevCourse + MaxTurnAngle) Then
			If AnglesSideDef(fPrevCourse + 180.0, fTmp) < 0 Then
				fTmp = Modulus(fPrevCourse - MaxTurnAngle)
			Else
				fTmp = Modulus(fPrevCourse + MaxTurnAngle)
			End If
		End If

		TextBoxChangeCourse_0.Text = CStr(System.Math.Round(fTmp))
		TextBoxChangeCourse_1.Text = CStr(System.Math.Round(Modulus(fTmp - CurrADHP.MagVar)))

		fDir = Azt2Dir(CurrPt.pPtGeo, fTmp)
		ApplyNewDirection(fDir)

	End Sub

	'Sub OpenConfigFile()
	'    OpenConfig (App.Path + "\RISKAERO.xml"), "<RISKAero version=""1.0""></RISKAero>", "RISKAero/TAP/"
	'End Sub
	'
	'Sub LoadConfig()
	'Dim fTmp As Double
	'    OpenConfigFile
	'
	'    Dim indS As String
	'    indS = GetConfigValue("Page0/OptionButton001/@value")
	'    If IsNumeric(indS) Then OptionButton001(CLng(indS)).Value = True
	'
	''    TextBox001.Text = GetConfigValue("Page0/TextBox001.Text/@value")
	'    TextBox002.Text = GetConfigValue("Page0/TextBox002.Text/@value")
	''    TextBox003.Text = GetConfigValue("Page0/TextBox003.Text/@value")
	'    TextBox201.Text = GetConfigValue("Page0/TextBox201.Text/@value")
	'    If IsNumeric(TextBox201.Text) Then
	'        fTmp = CDbl(TextBox201.Text)
	'        TextBox004.Text = CStr(Round(Modulus(fTmp + CurrADHP.MagVar, 360.0), 2))
	'    End If
	''    TextBox004.Text = GetConfigValue("Page0/TextBox004.Text/@value")
	'
	''    CheckBox002.Value = IIf(GetConfigValue("Page0/CheckBox002.Value/@value") = "1", 1, 0)
	''    CheckBox002_Click
	'
	'End Sub
	'
	'Sub CloseConfig()
	'
	'    OpenConfigFile
	'
	''    SetConfigValue "Page0/TextBox001.Text/@value", TextBox001.Text
	'    SetConfigValue "Page0/TextBox002.Text/@value", TextBox002.Text
	'    SetConfigValue "Page0/OptionButton001/@value", GetSelectedOptionButton(OptionButton001)
	'    SetConfigValue "Page0/TextBox201.Text/@value", TextBox201.Text
	''    SetConfigValue "Page0/TextBox003.Text/@value", TextBox003.Text
	''    SetConfigValue "Page0/TextBox004.Text/@value", TextBox004.Text
	''    SetConfigValue "Page0/CheckBox002.Value/@value", CheckBox002.Value
	'
	'    SaveConfig
	'End Sub

End Class