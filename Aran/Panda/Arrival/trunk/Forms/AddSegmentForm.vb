Imports System.ComponentModel
Imports Aran.AranEnvironment
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geometry

<System.Runtime.InteropServices.ComVisible(False)> Friend Class AddSegmentForm
	Inherits System.Windows.Forms.Form

#Region "declerations"
	Const RadiusCoeff As Double = 0.0000644666
	Const FootCoeff As Double = 0.3048
	Const HelpContextID As Integer = 2600
	Const NavSeeCoeff As Integer = 2500
	Const NavMinAngle As Integer = 0
	Const WPTFarness As Integer = 100

	Const AreaMinSeminwidth As Double = 2.0 * 1852.0
	Const AreaMaxSeminwidth As Double = 4.0 * 1852.0

	Const RMin As Double = 9.0 * 1852.0
	Const RMax As Double = 100.0 * 1852.0
	Const cMaxH As Double = 12800.0
	Const fMinDist As Double = 500.0
	Const fMaxDist As Double = 200000.0

	Const MinTurnAngle As Double = 1.0
	Const MaxTurnAngle As Double = 120.0

	'Public IsSectorized As Boolean
	'Public SectorRightDir As Double
	'Public SectorLeftDir As Double
	'Public SectorHeight As Double
	'Public SectorPoly As IPointCollection
	Dim m_refHeight As Double

	Dim CurrADHP As ADHPType
	Dim m_MasterForm As IProcedureForm

	Dim bVTextBox003 As Boolean
	Dim bVTextBox004 As Boolean
	Dim bVTextBox005 As Boolean
	Dim bVTextBox006 As Boolean

	Dim m_IAS As Double
	Dim m_PDG As Double
	'Dim m_AztDir As Double
	Dim m_CurrDir As Double
	Dim m_CurrPnt As ESRI.ArcGIS.Geometry.IPoint

	Dim m_CurrAzt As Double
	Dim m_HFinish As Double
	Dim m_BankAngle As Double
	Dim m_seminWidth As Double
	Dim m_MagVar As Double
	Dim m_TurnR As Double

	Dim PrevSegment As TraceSegment
	Dim CurrSegment As TraceSegment

	Dim pNominalTrackElem As ESRI.ArcGIS.Carto.IElement
	Dim pProtectAreaElem As ESRI.ArcGIS.Carto.IElement

	Dim Type1_RadInterval As Interval
	Dim Type1_CurNavaid As NavaidData
	Dim Type1_CurWPtFix As WPT_FIXType

	Dim Type1_CurDist As Double
	Dim Type1_CurDir As Double

	Dim Type2_CurDir As Double
	Dim Type2_CurTurnDir As Integer

	Dim Type3_CurNav As WPT_FIXType

	Dim Type3_CurDir As Double
	Dim Type3_CurTurnDir As Integer

	Dim Type3_CurrInterval As Interval

	Dim ComboBox301_LIntervals() As Interval
	Dim ComboBox301_RIntervals() As Interval

	Dim Type4_CurFix As WPT_FIXType
	Dim Type4_CurTurnDir As Integer

	Dim Type5_CurNav As WPT_FIXType
	Dim Type5_Intervals() As Interval
	Dim Type5_CurDir As Double
	Dim Type5_CurTurnDir As Integer
	Dim Type5_SnapAngle As Double

	Dim Type6_CurNav As NavaidData
	Dim Type6_Intervals() As Interval

	Dim Type7_CurFix As WPT_FIXType
	Dim Type7_CurNav As NavaidData
	Dim Type7_Intervals() As Interval

	Dim ComboBox101List() As WPT_FIXType
	Dim ComboBox102List() As NavaidData                'FIXableNavaidType
	'	Dim  WPT_FIXType[] ComboBox301List
	'	Dim  WPT_FIXType[] ComboBox303List
	'	Dim  WPT_FIXType[] ComboBox401List
	'	Dim  WPT_FIXType[] ComboBox501List
	'	Dim  WPT_FIXType[] ComboBox503List
	'	Dim  NavaidType[] ComboBox603List
	'	Dim  NavaidType[] ComboBox701List
	'	Dim  WPT_FIXType[] ComboBox703List

	Dim IsLoaded As Boolean
	Dim screenCapture As IScreenCapture

#End Region

#Region "Form"

	Public Sub New(screenCapture As IScreenCapture)
		InitializeComponent()

		Me.screenCapture = screenCapture
		IsLoaded = False
		'SSTab1.Enabled = false

		SSTab1.SelectedIndex = 0

		'Application.HelpFile = "Panda.chm"

		m_seminWidth = AreaMinSeminwidth
		m_BankAngle = 15.0

		TextBox005.Text = Functions.ConvertDistance(m_seminWidth, eRoundMode.NEAREST).ToString()
		TextBox006.Text = m_BankAngle.ToString()

		Label004.Text = GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit
		Label006.Text = GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit
		Label014.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit

		Label010.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label103.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label310.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label506.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label607.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label610.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit
		Label613.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit

		ComboBox704.Left = TextBox703.Left
		ComboBox704.Top = TextBox703.Top

		'================================================================= str60237
		SSTab1.TabPages(0).Text = My.Resources.str60003
		SSTab1.TabPages(1).Text = My.Resources.str60004
		SSTab1.TabPages(2).Text = My.Resources.str60005
		SSTab1.TabPages(3).Text = My.Resources.str60006
		SSTab1.TabPages(4).Text = My.Resources.str60007
		SSTab1.TabPages(5).Text = My.Resources.str60008
		SSTab1.TabPages(6).Text = My.Resources.str60009

		OkBtn.Text = My.Resources.str00152
		CancelBtn.Text = My.Resources.str00153

		Label001.Text = My.Resources.str00232

		ComboBox001.Items.Add(My.Resources.str00223)
		ComboBox001.Items.Add(My.Resources.str00224)

		ComboBox002.Items.Add(My.Resources.str00223)
		ComboBox002.Items.Add(My.Resources.str00224)

		'Label005.Caption = Resources.str60106
		'Label007.Caption = Resources.str60107
		ComboBox001.SelectedIndex = 0
		ComboBox002.SelectedIndex = 0

		'=================================================================
		Label102.Text = My.Resources.str00848
		Label101.Text = My.Resources.str60103
		Label106.Text = My.Resources.str10514

		OptionButton101.Text = My.Resources.str60104
		OptionButton102.Text = My.Resources.str60105
		OptionButton103.Text = My.Resources.str60108

		'=================================================================
		Label201.Text = My.Resources.str60202
		Label203.Text = My.Resources.str10701
		Label213.Text = My.Resources.str60306 'Resources.str13204

		ComboBox201.Items.Add(My.Resources.str00226)
		ComboBox201.Items.Add(My.Resources.str00225)
		ComboBox201.SelectedIndex = 0
		'=================================================================
		Label301.Text = My.Resources.str60302
		Label303.Text = My.Resources.str10701
		Label305.Text = My.Resources.str60306

		Label307.Text = My.Resources.str00232
		Label309.Text = My.Resources.str60309
		Label311.Text = My.Resources.str60310

		CheckBox301.Text = My.Resources.str60305

		ComboBox302.Items.Add(My.Resources.str00226)
		ComboBox302.Items.Add(My.Resources.str00225)
		ComboBox302.SelectedIndex = 0

		'=================================================================
		Label401.Text = My.Resources.str60203
		Label403.Text = My.Resources.str60306 'Resources.str13404
		Label405.Text = My.Resources.str60303
		Label407.Text = My.Resources.str10701

		ComboBox402.Items.Add(My.Resources.str00226)
		ComboBox402.Items.Add(My.Resources.str00225)
		ComboBox402.SelectedIndex = 0
		'=================================================================
		Label503.Text = My.Resources.str50217
		Label508.Text = My.Resources.str60302 'Resources.str13502
		Label509.Text = My.Resources.str00232
		Label510.Text = My.Resources.str60508
		Label517.Text = My.Resources.str10701
		Label518.Text = My.Resources.str10514

		CheckBox501.Text = My.Resources.str60305 '13507 xxx

		ComboBox502.Items.Add(My.Resources.str00226)
		ComboBox502.Items.Add(My.Resources.str00225)
		ComboBox502.SelectedIndex = 0
		'=================================================================
		TextBox603.Text = Functions.ConvertDistance(RMin, eRoundMode.NEAREST).ToString()
		TextBox604.Text = Functions.ConvertDistance(RMax, eRoundMode.NEAREST).ToString()
		TextBox605.Text = Functions.ConvertDistance(RMin, eRoundMode.NEAREST).ToString()

		ComboBox601.SelectedIndex = 0
		ComboBox602.SelectedIndex = 0
		'=================================================================
		ComboBox702.SelectedIndex = 0

		'Help Context Number: 2600
		'Topic ID: Dep_Omnidir_Route_Modelling
		'Anchor ID: Route_Modelling_Rt_Segm_DialogForm

		'Text = Resources.str13001 + " v: " + Application.ProductVersion.ToString()	//Application.ProductVersion.Major.ToString() + "." + Application.ProductVersion.Minor.ToString() + "." + Application.ProductVersion.Revision.ToString()
		Me.Text = My.Resources.str13001 '+ " " + My.Resources.str00818

		'SetFormParented(Handle.ToInt32)
		IsLoaded = True
	End Sub

	Private Sub AddSegmentForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
		ClearSegmentDrawings(True)

		If e.CloseReason = System.Windows.Forms.CloseReason.UserClosing Then

			If (m_MasterForm.IsClosing) Then Return

			Hide()
			e.Cancel = True

			Dim tmpSegment As TraceSegment = New TraceSegment()
			m_MasterForm.DialogHook(0, tmpSegment)
		End If
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

#Region "initialisations"

	Public Function CreateInitialSegment(pPtEnd As IPoint, pPolyline As IPolyline, pPolygon As IPolygon, dir As Double, PDG As Double, TNA As Double) As TraceSegment
		Dim result As TraceSegment = New TraceSegment()

		result.HStart = TNA '+ PANS_OPS_DataBase.dpH_abv_DER.Value
		result.HFinish = pPtEnd.Z
		result.PDG = PDG
		result.DirIn = dir
		result.DirOut = result.DirIn

		result.ptIn = pPolyline.FromPoint
		result.ptOut = pPtEnd   'Functions.PointAlongPlane(pPtStart, dir, (result.HFinish - result.HStart) / PDG)

		result.PathPrj = pPolyline
		result.Length = pPolyline.Length

		Dim pClone As IClone = pPolygon
		result.pProtectArea = pClone.Clone()
		Dim pTopo As ITopologicalOperator2 = result.pProtectArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		pCutter = New ESRI.ArcGIS.Geometry.Polyline
		pCutter.FromPoint = PointAlongPlane(result.ptOut, dir - 90.0, 10.0 * RModel)
		pCutter.ToPoint = PointAlongPlane(result.ptOut, dir + 90.0, 10.0 * RModel)

		CutPoly(result.pProtectArea, pCutter, 1)

		'====

		result.Comment = My.Resources.str30007      '// "Начальный участок"
		result.RepComment = result.Comment

		result.SegmentCode = eSegmentType.straight
		result.LegType = Aim.Enums.CodeSegmentPath.VA
		result.GuidanceNav.TypeCode = eNavaidType.NONE
		result.InterceptionNav.TypeCode = eNavaidType.NONE

		Dim PtGeoSt As IPoint = Functions.ToGeo(result.ptIn)
		Dim PtGeoFin As IPoint = Functions.ToGeo(result.ptOut)

		result.StCoords = Functions.DegreeToString(System.Math.Abs(PtGeoSt.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(System.Math.Abs(PtGeoSt.X), Degree2StringMode.DMSLon)
		result.FinCoords = Functions.DegreeToString(System.Math.Abs(PtGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(System.Math.Abs(PtGeoFin.X), Degree2StringMode.DMSLon)

		Return result
	End Function

	Public Sub CreateNextSegment(ADHP As ADHPType, refHeight As Double, fIAS As Double, fPDG As Double, PSegment As TraceSegment, masterForm As IProcedureForm)
		Dim fTASl As Double

		m_MasterForm = masterForm

		PrevSegment = PSegment
		pNominalTrackElem = Nothing
		pProtectAreaElem = Nothing
		'FormRes = false

		m_IAS = fIAS
		'm_AztDir = fAztDir
		m_PDG = fPDG

		CurrADHP = ADHP
		m_MagVar = ADHP.MagVar      '0.0;	'

		m_refHeight = refHeight

		CurrSegment.HStart = PrevSegment.HFinish
		m_HFinish = Math.Min(CurrSegment.HStart, cMaxH)

		m_CurrDir = PrevSegment.DirOut
		m_CurrPnt = PrevSegment.ptOut
		m_CurrPnt.M = m_CurrDir

		m_CurrAzt = Functions.Dir2Azt(m_CurrPnt, m_CurrDir)

		TextBox001.Text = ConvertAngle(m_CurrAzt).ToString()

		If ComboBox001.SelectedIndex = 0 Then
			TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart, eRoundMode.NEAREST).ToString()
		Else
			TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart - m_refHeight, eRoundMode.NEAREST).ToString()
		End If

		ComboBox002_SelectedIndexChanged(ComboBox002, New System.EventArgs())

		TextBox004.Text = (100.0 * m_PDG).ToString()

		fTASl = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC)
		m_TurnR = Functions.Bank2Radius(m_BankAngle, fTASl)
		TextBox007.Text = Functions.ConvertDistance(m_TurnR, eRoundMode.NEAREST).ToString()
		'=============================================================================================
		If PrevSegment.SegmentCode = eSegmentType.arcIntercept Then
			SSTab1.TabPages(0).Enabled = False
			SSTab1.TabPages(1).Enabled = False
			SSTab1.TabPages(2).Enabled = False
			SSTab1.TabPages(3).Enabled = False
			SSTab1.TabPages(4).Enabled = False
			SSTab1.TabPages(5).Enabled = False

			SSTab1.TabPages(6).Enabled = True
			SSTab1.TabPages(6).Visible = True

			If SSTab1.SelectedIndex = 6 Then
				Do_Option007()
			Else
				SSTab1.SelectedIndex = 6
			End If
		Else
			SSTab1.TabPages(0).Enabled = True
			SSTab1.TabPages(1).Enabled = True
			SSTab1.TabPages(2).Enabled = FillComboBox301Stations() > 0
			SSTab1.TabPages(3).Enabled = FillComboBox401Stations() > 0
			SSTab1.TabPages(4).Enabled = FillComboBox501Stations() > 0
			SSTab1.TabPages(5).Enabled = FillComboBox603DMEStations() > 0
			SSTab1.TabPages(6).Enabled = False
			SSTab1.TabPages(6).Visible = False

			If SSTab1.SelectedIndex = 0 Then
				Do_Option001()
			Else
				SSTab1.SelectedIndex = 0
			End If
		End If

		Show(GlobalVars._Win32Window)
	End Sub

	Sub ConstructNextSegment()

		CurrSegment.pProtectArea = Nothing
		If m_seminWidth = 0 Then Return

		ClearSegmentDrawings(False)
		Dim Assigned As Boolean = False

		If PrevSegment.SegmentCode = eSegmentType.arcIntercept Then
			Assigned = Type7Segment(Type7_CurNav, CurrSegment)
		ElseIf SSTab1.SelectedIndex = 0 Then

			If (OptionButton101.Checked) Then
				Assigned = Type1SegmentOnDistance(Type1_CurDist, CurrSegment)
			ElseIf (OptionButton102.Checked) Then
				Assigned = Type1SegmentOnWpt(Type1_CurWPtFix, CurrSegment)
			ElseIf (OptionButton103.Checked) Then
				Assigned = Type1SegmentOnNewFIX(Type1_CurNavaid, Type1_CurDir, CurrSegment)
			End If

		ElseIf (SSTab1.SelectedIndex = 1) Then
			Assigned = Type2Segment(Type2_CurDir, Type2_CurTurnDir, CurrSegment)
		ElseIf (SSTab1.SelectedIndex = 2) Then
			Assigned = Type3Segment(Type3_CurNav, Type3_CurDir, CurrSegment)
		ElseIf (SSTab1.SelectedIndex = 3) Then
			Assigned = Type4Segment(Type4_CurFix, Type4_CurTurnDir, CurrSegment)
		ElseIf (SSTab1.SelectedIndex = 4) Then
			Assigned = Type5Segment(Type5_CurNav, Type5_CurDir, Type5_CurTurnDir, Type5_SnapAngle, CurrSegment)
		ElseIf (SSTab1.SelectedIndex = 5) Then
			Assigned = Type6Segment(Type6_CurNav, CurrSegment)
		End If

		Dim bRefreshNeeded As Boolean = True

		If Assigned Then
			Dim PtGeoSt As ESRI.ArcGIS.Geometry.IPoint = CType(Functions.ToGeo(CurrSegment.ptIn), Global.ESRI.ArcGIS.Geometry.IPoint)
			Dim PtGeoFin As ESRI.ArcGIS.Geometry.IPoint = CType(Functions.ToGeo(CurrSegment.ptOut), Global.ESRI.ArcGIS.Geometry.IPoint)

			CurrSegment.HFinish = m_HFinish
			CurrSegment.BankAngle = m_BankAngle
			CurrSegment.PDG = m_PDG
			CurrSegment.StCoords = Functions.DegreeToString(System.Math.Abs(PtGeoSt.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(System.Math.Abs(PtGeoSt.X), Degree2StringMode.DMSLon)
			CurrSegment.FinCoords = Functions.DegreeToString(System.Math.Abs(PtGeoFin.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(System.Math.Abs(PtGeoFin.X), Degree2StringMode.DMSLon)

			ComboBox002_SelectedIndexChanged(ComboBox002, New System.EventArgs())

			'        TextBox003_Validate False

			If Not (CurrSegment.PathPrj Is Nothing) Then
				Dim pTopo As ITopologicalOperator2 = CType(CurrSegment.PathPrj, Global.ESRI.ArcGIS.Geometry.ITopologicalOperator2)
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				If Not CurrSegment.PathPrj.IsEmpty Then

					'**************************************************************
					Dim pIZ As IZ
					Dim pIZAware As IZAware
					Dim pPtTmp As IPoint

					pPtTmp = CurrSegment.PathPrj.FromPoint
					pPtTmp.Z = CurrSegment.HStart
					CurrSegment.PathPrj.FromPoint = pPtTmp

					pPtTmp = CurrSegment.PathPrj.ToPoint
					pPtTmp.Z = m_HFinish
					CurrSegment.PathPrj.ToPoint = pPtTmp

					pIZAware = CurrSegment.PathPrj
					pIZAware.ZAware = True

					pIZ = CurrSegment.PathPrj
					pIZ.CalculateNonSimpleZs()
					'**************************************************************

					CurrSegment.pProtectArea = CType(pTopo.Buffer(m_seminWidth), Global.ESRI.ArcGIS.Geometry.IPolygon)
					CurrSegment.SeminWidth = m_seminWidth
					pTopo = CType(CurrSegment.pProtectArea, Global.ESRI.ArcGIS.Geometry.ITopologicalOperator2)
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					pProtectAreaElem = Functions.DrawPolygon(CurrSegment.pProtectArea, RGB(0, 255, 0))
					pNominalTrackElem = Functions.DrawPolyLine(CurrSegment.PathPrj, RGB(255, 0, 0))
					bRefreshNeeded = False
				End If
			End If
		End If

		If (bRefreshNeeded) Then
			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Sub

	Sub ClearSegmentDrawings(Optional bRefresh As Boolean = True)

		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer = GlobalVars.GetActiveView().GraphicsContainer

		Functions.SafeDeleteElement(pNominalTrackElem)
		Functions.SafeDeleteElement(pProtectAreaElem)

		If (bRefresh) Then
			GlobalVars.GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Sub

	'Function RadialConflict(Segment As TraceSegment) As Boolean

	'	'If Segment.HStart >= SectorHeight Then Return False

	'	Dim pTopo As ITopologicalOperator2 = Segment.PathPrj
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	Dim PathPts As IPointCollection = Segment.PathPrj
	'	Dim i As Integer
	'	Dim n As Integer = PathPts.PointCount

	'	For i = 0 To n - 2

	'		Dim Ang As Double = NativeMethods.Modulus(90.0 - Functions.ReturnAngleInDegrees(PathPts.Point(i), PathPts.Point(i + 1)))
	'		Dim SegmOutDir As Double = NativeMethods.Modulus(Ang - NativeMethods.Modulus(90.0 - m_AztDir))

	'		If SegmOutDir > 180.0 Then SegmOutDir = SegmOutDir - 360.0
	'		If (SegmOutDir < SectorLeftDir) Or (SegmOutDir > SectorRightDir) Then Return True
	'	Next

	'	Return False
	'End Function

	Function CenterOfTurn(FromPt As IPoint, ToPt As IPoint) As IPoint
		Dim fTmp As Double = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M)
		Dim pPtX As IPoint = New ESRI.ArcGIS.Geometry.Point()
		Dim ptConstr As IConstructPoint = pPtX

		If (System.Math.Abs(System.Math.Sin(fTmp)) > GlobalVars.DegToRadValue * 0.5) Then
			ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * NativeMethods.Modulus(FromPt.M + 90.0), ToPt, GlobalVars.DegToRadValue * NativeMethods.Modulus(ToPt.M + 90.0))
		Else
			pPtX.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y))
		End If

		Return pPtX
	End Function

	Function FormatInterval(I As Interval) As String
		Dim result As String = ""

		If (I.Right = -1) Then
			result = ConvertAngle(I.Left).ToString() + "°"
		Else
			result = My.Resources.str60313 + ConvertAngle(I.Left).ToString() + My.Resources.str60314 + ConvertAngle(I.Right).ToString() + "°"
		End If

		Return result
	End Function

	Function FillComboBox101Stations() As Integer
		Dim i As Integer
		Dim j As Integer = -1
		Dim n As Integer = GlobalVars.WPTList.Length - 1

		ComboBox101.Items.Clear()
		ReDim ComboBox101List(n)

		For i = 0 To n

			Dim Type1PossibleWpt As Boolean = (Functions.SideDef(m_CurrPnt, m_CurrDir + 90.0, GlobalVars.WPTList(i).pPtPrj) > 0) And
					(Functions.Point2LineDistancePrj(GlobalVars.WPTList(i).pPtPrj, m_CurrPnt, m_CurrDir) < WPTFarness) And
					(Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList(i).pPtPrj) > 1)

			If (Type1PossibleWpt) Then

				j += 1
				ComboBox101List(j) = GlobalVars.WPTList(i)
				ComboBox101.Items.Add(ComboBox101List(j).Name)
			End If
		Next

		ReDim Preserve ComboBox101List(j)

		Return j
	End Function

	Function FillComboBox102Stations() As Integer

		Dim i, j, l, n, m As Integer

		Dim fTmp, navX, navY, navL, ConDist, interLeft, interRight, cotan23 As Double
		Dim VORConDist, NDBConDist, minInterceptAngle, maxInterceptAngle As Double

		minInterceptAngle = 30.0
		maxInterceptAngle = 180.0 - minInterceptAngle

		VORConDist = CurrSegment.HStart * Math.Tan(Functions.DegToRad(Navaids_DataBase.VOR.ConeAngle))
		NDBConDist = CurrSegment.HStart * Math.Tan(Functions.DegToRad(Navaids_DataBase.NDB.ConeAngle))

		ComboBox102.Items.Clear()

		n = GlobalVars.NavaidList.Length
		m = GlobalVars.DMEList.Length
		j = -1
		ReDim ComboBox102List(n + m)

		For i = 0 To n - 1
			Dim pPtNav As IPoint = GlobalVars.NavaidList(i).pPtPrj
			Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtNav.X, pPtNav.Y, navX, navY)

			If (GlobalVars.NavaidList(i).TypeCode = eNavaidType.VOR) Or (GlobalVars.NavaidList(i).TypeCode = eNavaidType.TACAN) Then
				ConDist = VORConDist
			Else
				ConDist = NDBConDist
			End If

			'DrawPointWithText pPtNav, "SL"
			'DrawPointWithText m_CurrPnt, "CurrPnt"
			'DrawPointWithText PointAlongPlane(m_CurrPnt, m_CurrDir + 90#, navY), "navY"

			If navX > GlobalVars.MaxNAVDist Then Continue For
			If navX < -0.5 * GlobalVars.MaxNAVDist Then Continue For

			If System.Math.Abs(navY) < ConDist Then Continue For
			If System.Math.Abs(navY) > GlobalVars.MaxNAVDist Then Continue For

			fTmp = NativeMethods.Modulus(90.0 - Functions.RadToDeg(Math.Atan(navX / navY)), 180.0)

			If (navY > 0.0) Then
				interLeft = Math.Max(fTmp, minInterceptAngle)
				If (interLeft > maxInterceptAngle) Then Continue For
				interLeft = interLeft + 180.0
				interRight = maxInterceptAngle + 180.0
			Else
				interRight = Math.Min(fTmp, maxInterceptAngle)
				If (interRight < minInterceptAngle) Then Continue For
				interLeft = minInterceptAngle
			End If

			j += 1

			ComboBox102List(j) = GlobalVars.NavaidList(i)
			ReDim ComboBox102List(j).ValMax(0)
			ReDim ComboBox102List(j).ValMin(0)

			ComboBox102List(j).ValMin(0) = interLeft
			ComboBox102List(j).ValMax(0) = interRight

			ComboBox102.Items.Add(ComboBox102List(j))
		Next

		Dim ii, jj, ll, kk As Integer
		kk = 0
		Dim Int23, IntRange As Interval
		Dim IntrRes(), IntrRes1() As Interval
		Dim Y55, cotan55, L55 As Double

		cotan23 = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.DME.TP_div) 'PANS_OPS_DataBase.dpTP_by_DME_div.Value)
		cotan55 = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.DME.SlantAngle)

		For i = 0 To m - 1
			Dim pPtNav As IPoint = GlobalVars.DMEList(i).pPtPrj
			Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtNav.X, pPtNav.Y, navX, navY)
			navY = System.Math.Abs(navY)

			navL = navY * cotan23

			Y55 = (CurrSegment.HStart - pPtNav.Z) * cotan55

			If (Y55 > navY) Then
				L55 = System.Math.Sqrt(Y55 * Y55 - navY * navY)
				navL = System.Math.Max(navL, L55)
			End If

			IntRange.Left = navX - Navaids_DataBase.DME.Range
			IntRange.Right = navX + Navaids_DataBase.DME.Range
			If IntRange.Left < fMinDist Then IntRange.Left = fMinDist
			If IntRange.Right > GlobalVars.MaxNAVDist Then IntRange.Right = GlobalVars.MaxNAVDist

			Int23.Left = navX - navL
			Int23.Right = navX + navL

			IntrRes = Functions.IntervalsDifference(IntRange, Int23)
			l = IntrRes.Length - 1
			ReDim IntrRes1(-1)

			ii = 0
			If l >= 0 Then
				Do
					If (IntrRes(ii).Left = IntrRes(ii).Right) Then

						For jj = ii To l - 1
							IntrRes(jj) = IntrRes(jj + 1)
						Next

						l -= 1

					Else
						ii += 1
					End If

				Loop While (ii < l - 1)
			End If

			ii = 0
			Do While (ii < l - 1)

				If (IntrRes(ii).Right = IntrRes(ii + 1).Left) Then

					IntrRes(ii).Right = IntrRes(ii + 1).Right
					For jj = ii + 1 To l - 1
						IntrRes(jj) = IntrRes(jj + 1)
					Next

					l -= 1
				Else
					ii += 1
				End If
			Loop

			If l < 0 Then Continue For

			ReDim IntrRes1(l)
			ll = -1

			For ii = 0 To l

				Int23.Left = IntrRes(ii).Left - navX
				Int23.Right = IntrRes(ii).Right - navX

				If (Int23.Left < Int23.Right) Then
					ll += 1

					If (Int23.Left < 0) Then
						kk = -1
						IntrRes1(ll).Right = System.Math.Abs(Int23.Left)
						IntrRes1(ll).Left = System.Math.Abs(Int23.Right)

					Else
						kk = 1
						IntrRes1(ll).Left = Int23.Left
						IntrRes1(ll).Right = Int23.Right
					End If

					'ptNearD = PointAlongPlane(m_CurrPnt, m_CurrDir, IntrRes(ii).Left)
					'kk = SideDef(KKhMinDME.FromPoint, m_CurrDir + 90#, pPtNav)
					'ComboBox102List(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
				End If
			Next

			If (ll < 0) Then Continue For
			If (ll > 0) Then kk = 0

			'if( IntrRes1(0).Right < IntrRes1(0).Left ) GoTo NextDME

			j += 1

			ComboBox102List(j) = GlobalVars.DMEList(i)

			ComboBox102List(j).ValCnt = kk
			ReDim ComboBox102List(j).ValMax(ll)
			ReDim ComboBox102List(j).ValMin(ll)

			For ii = 0 To ll
				ComboBox102List(j).ValMin(ii) = IntrRes1(ii).Left
				ComboBox102List(j).ValMax(ii) = IntrRes1(ii).Right
			Next

			ComboBox102.Items.Add(ComboBox102List(j))
		Next


		ReDim Preserve ComboBox102List(j)
		'System.Array.Resize<FIXableNavaidType>(ref ComboBox102List, j)
		j += 1
		Return j
	End Function

	Function FillComboBox301Stations() As Integer
		Dim i, j, n As Integer
		Dim fRefX, fRefY, InvTan1Deg, InvTanMaxDeg, fTmp, fMaxInterceptDist As Double

		Dim pPtRef As IPoint

		Dim LInterval, RInterval As Interval
		Dim bYaradi As Boolean
		Dim fT1, fT2, fAlpha1, fAlpha2, fAbsRefY As Double

		Dim fDeterOp, fDeterPo As Double

		InvTan1Deg = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue)
		InvTanMaxDeg = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * (180.0 - MaxTurnAngle))

		ComboBox301.Items.Clear()

		j = -1

		n = GlobalVars.WPTList.Length
		'ComboBox301List = new WPT_FIXType(n)
		ReDim ComboBox301_LIntervals(n - 1)
		ReDim ComboBox301_RIntervals(n - 1)

		For i = 0 To n - 1

			pPtRef = GlobalVars.WPTList(i).pPtPrj

			Functions.PrjToLocal(m_CurrPnt, m_CurrDir, pPtRef.X, pPtRef.Y, fRefX, fRefY)
			fAbsRefY = System.Math.Abs(fRefY)

			If (fRefX < -GlobalVars.MaxNAVDist) Or (fRefX > GlobalVars.MaxNAVDist) Or (fAbsRefY > GlobalVars.MaxNAVDist) Then
			Else
				fDeterOp = System.Math.Sqrt(fRefX * fRefX + fAbsRefY * (fAbsRefY + 2.0 * m_TurnR))
				fDeterPo = fRefX * fRefX + fAbsRefY * (fAbsRefY - 2 * m_TurnR)

				If (fDeterPo < 0) Then
					fDeterPo = -1
				Else
					fDeterPo = System.Math.Sqrt(fDeterPo)
				End If

				RInterval.Left = -1
				RInterval.Right = -1
				LInterval.Left = -1
				LInterval.Right = -1

				If (fRefY > 0.0) Then
					'Sola =========================================================
					If (fDeterPo >= 0.0) Then

						fAlpha2 = MaxTurnAngle

						If (fAbsRefY > 2.0 * m_TurnR) Then

							fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
							fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1)

							If (fAlpha1 < fAlpha2) Then
								LInterval.Left = fAlpha1
								LInterval.Right = fAlpha2
							End If
						ElseIf (fAbsRefY < 2.0 * m_TurnR) Then

							fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
							fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1)

							If (fAbsRefY > 0) Then
								fT2 = (-fRefX - fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
								fAlpha2 = Math.Min(2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2), fAlpha2)

								If (fAlpha2 > MinTurnAngle) And (fAlpha1 < fAlpha2) Then
									If (fAlpha1 < MinTurnAngle) Then fAlpha1 = MinTurnAngle
									If (fAlpha2 > MaxTurnAngle) Then fAlpha2 = MaxTurnAngle
									LInterval.Left = fAlpha1
									LInterval.Right = fAlpha2
								End If
							End If
						End If
					End If

					'Sag"a =========================================================
					fAlpha1 = MinTurnAngle
					fMaxInterceptDist = fRefX + fAbsRefY / System.Math.Tan(GlobalVars.DegToRadValue * MinTurnAngle)

					If (fMaxInterceptDist > fMaxDist) Then
						fTmp = fMaxDist - fRefX
						If System.Math.Abs(fTmp) < GlobalVars.distEps Then
							fAlpha1 = 90.0
						Else

							fAlpha1 = GlobalVars.RadToDegValue * System.Math.Atan(fAbsRefY / fTmp)
							If (fAlpha1 < 0) Then fAlpha1 += 180.0
						End If
					End If

					fT2 = (fRefX + fDeterOp) / (fAbsRefY + 2.0 * m_TurnR)
					fAlpha2 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2)
					If fAlpha2 > MaxTurnAngle Then fAlpha2 = MaxTurnAngle

					If (fAlpha2 >= MinTurnAngle) And (fAlpha2 - fAlpha1 > 1.0) Then
						RInterval.Left = fAlpha1
						RInterval.Right = fAlpha2
					End If
				ElseIf (fRefY < 0.0) Then
					'Saga =========================================================
					fAlpha1 = MinTurnAngle
					fMaxInterceptDist = fRefX + fAbsRefY / System.Math.Tan(GlobalVars.DegToRadValue * MinTurnAngle)

					If (fMaxInterceptDist > fMaxDist) Then
						fTmp = fMaxDist - fRefX
						If (System.Math.Abs(fTmp) < GlobalVars.distEps) Then
							fAlpha1 = 90.0
						Else
							fAlpha1 = GlobalVars.RadToDegValue * System.Math.Atan(fAbsRefY / fTmp)
							If fAlpha1 < 0.0 Then fAlpha1 += 180.0
						End If
					End If

					fT2 = (fRefX + fDeterOp) / (fAbsRefY + 2.0 * m_TurnR)
					fAlpha2 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2)
					If fAlpha2 > MaxTurnAngle Then fAlpha2 = MaxTurnAngle

					If (fAlpha2 >= MinTurnAngle) And (fAlpha2 - fAlpha1 > 1.0) Then
						LInterval.Left = fAlpha1
						LInterval.Right = fAlpha2
					End If

					'Sag"a =========================================================
					If (fDeterPo >= 0) Then

						fAlpha2 = MaxTurnAngle

						If (fAbsRefY > 2.0 * m_TurnR) Then
							fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
							fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1)

							If (fAlpha1 < fAlpha2) Then
								RInterval.Left = fAlpha1
								RInterval.Right = fAlpha2
							End If
						ElseIf (fAbsRefY < 2.0 * m_TurnR) Then

							fT1 = (-fRefX + fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
							fAlpha1 = 2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT1)

							If (fAbsRefY > 0) Then
								fT2 = (-fRefX - fDeterPo) / (fAbsRefY - 2.0 * m_TurnR)
								fAlpha2 = Math.Min(2.0 * GlobalVars.RadToDegValue * System.Math.Atan(fT2), fAlpha2)

								If (fAlpha2 > MinTurnAngle) And (fAlpha1 < fAlpha2) Then
									If (fAlpha1 < MinTurnAngle) Then fAlpha1 = MinTurnAngle
									If (fAlpha2 > MaxTurnAngle) Then fAlpha2 = MaxTurnAngle
									RInterval.Left = fAlpha1
									RInterval.Right = fAlpha2
								End If
							End If
						End If
					End If
				Else
					If (fRefX > 0) Then
						fT1 = fRefX / m_TurnR
						fAlpha1 = 2 * GlobalVars.RadToDegValue * System.Math.Atan(fT1)
						If fAlpha1 > MaxTurnAngle Then fAlpha1 = MaxTurnAngle

						If (fAlpha1 >= MinTurnAngle + 1.0) Then

							If (fAlpha1 > MaxTurnAngle) Then fAlpha1 = MaxTurnAngle
							LInterval.Left = MinTurnAngle
							LInterval.Right = fAlpha1

							RInterval.Left = MinTurnAngle
							RInterval.Right = fAlpha1
						End If

					End If
				End If

				bYaradi = (LInterval.Right > 0) Or (RInterval.Right > 0)

				If (bYaradi) Then

					j += 1
					ComboBox301_LIntervals(j) = LInterval
					ComboBox301_RIntervals(j) = RInterval
					'ComboBox301List(j) = GlobalVars.WPTList(i)
					ComboBox301.Items.Add(GlobalVars.WPTList(i))
				End If
			End If
		Next

		ReDim Preserve ComboBox301_LIntervals(j)
		ReDim Preserve ComboBox301_RIntervals(j)

		'Array.Resize<Interval>(ref ComboBox301_LIntervals, j)
		'Array.Resize<Interval>(ref ComboBox301_RIntervals, j)
		'Array.Resize<WPT_FIXType>(ref ComboBox301List, j)

		j += 1

		Return j
	End Function

	Function FillComboBox401Stations() As Integer
		Dim i As Integer
		Dim j As Integer = -1
		Dim n As Integer = GlobalVars.WPTList.Length

		'ComboBox401List = new WPT_FIXType(n)
		ComboBox401.Items.Clear()

		For i = 0 To n - 1
			'IPoint pPtCnt = Functions.PointAlongPlane(GlobalVars.WPTList[i].pPtPrj, m_CurrDir + 90.0, m_TurnR)
			'double fMinDist = Functions.ReturnDistanceInMeters(m_CurrPnt, pPtCnt)

			'pPtCnt = Functions.PointAlongPlane(GlobalVars.WPTList[I].pPtPrj, m_CurrDir - 90.0, m_TurnR)
			'double fDist = Functions.ReturnDistanceInMeters(m_CurrPnt, pPtCnt)
			'If (fDist < fMinDist) Then fMinDist = fDist
			'If (fMinDist > m_TurnR) Then
			'    j+=1

			'    ComboBox401List[j] = GlobalVars.WPTList[i]
			'    ComboBox401.Items.Add(ComboBox401List[j].Name)
			'end if

			If (Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList(i).pPtPrj) > m_TurnR) Then
				j += 1
				'ComboBox401List[j] = GlobalVars.WPTList(i)
				ComboBox401.Items.Add(GlobalVars.WPTList(i))
			End If
		Next

		Return j + 1
	End Function

	Function FillComboBox501Stations() As Integer
		Dim i As Integer
		Dim j As Integer = -1
		Dim n As Integer = GlobalVars.WPTList.Length

		ComboBox503.Enabled = False
		CheckBox501.Checked = False
		ComboBox501.Items.Clear()

		For i = 0 To n - 1
			If (Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList(i).pPtPrj) < GlobalVars.MaxNAVDist) Then
				j += 1
				ComboBox501.Items.Add(GlobalVars.WPTList(i))
			End If
		Next

		Return j + 1
	End Function

	Function FillComboBox603DMEStations() As Integer

		Dim i, j, n, Side, TurnDir As Integer

		Dim minR1R2, maxR1R2, R1, R2, R11, R12, R21, R22, CosA1, CosA2, Bheta, phi, R, fTmp, L As Double

		CosA1 = -2.0
		CosA2 = 1.0

		If (Not Double.TryParse(TextBox605.Text, fTmp)) Then

		End If

		R = Functions.DeConvertDistance(fTmp)

		j = -1
		n = GlobalVars.DMEList.Length
		'ComboBox603List = new NavaidType[N]
		ReDim Type6_Intervals(n - 1)

		Side = 2 * ComboBox601.SelectedIndex - 1
		TurnDir = 2 * ComboBox602.SelectedIndex - 1

		ComboBox603.Items.Clear()

		For i = 0 To n - 1

			Dim ptDME As IPoint = GlobalVars.DMEList(i).pPtPrj
			phi = Functions.ReturnAngleInDegrees(m_CurrPnt, ptDME)
			L = Functions.ReturnDistanceInMeters(m_CurrPnt, ptDME)

			Bheta = m_CurrDir - phi

			R11 = CosA1 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR      'if Cos(A1) = -0.5
			R12 = -1000000 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR   'if Cos(A1) = -0.5

			R21 = CosA2 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR      'if Cos(A2) = 1
			R22 = 1000000 * (Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) - Side * m_TurnR    'if Cos(A2) = 1

			If (R12 < 0) Then
				maxR1R2 = Math.Max(R21, R22)
				minR1R2 = Math.Min(R21, R22)
			Else
				maxR1R2 = Math.Max(R11, R12)
				minR1R2 = Math.Min(R11, R12)
			End If

			If (minR1R2 < RMax) And (maxR1R2 > RMin) Then
				R2 = Math.Min(maxR1R2, RMax)
				R1 = Math.Max(minR1R2, RMin)

				If Side > 0 Then
					fTmp = L - 1.5 * m_TurnR
					If (R2 > fTmp) Then R2 = fTmp
					If (System.Math.Cos(Functions.DegToRad(Bheta)) < 0) Then R2 = R1
				Else
					fTmp = L + 1.5 * m_TurnR
					If (R1 < fTmp) Then R1 = fTmp
				End If

				If (R1 < R2) Then
					j += 1
					'ComboBox603List(j) = GlobalVars.DMEList(i)
					Type6_Intervals(j).Left = R1
					Type6_Intervals(j).Right = R2
					ComboBox603.Items.Add(GlobalVars.DMEList(i))
				End If
			End If
		Next

		Return j + 1

	End Function

	Sub FillComboBox701Stations()
		Const CosaMax As Double = -0.5

		If (PrevSegment.SegmentCode <> eSegmentType.arcIntercept) Then Return

		Dim i As Integer
		Dim j As Integer = -1
		Dim n As Integer = GlobalVars.NavaidList.Length

		Dim ExitSide As Integer = 1 - 2 * ComboBox702.SelectedIndex

		'ComboBox701List = new NavaidType[n]
		ReDim Type7_Intervals(n - 1)
		ComboBox701.Items.Clear()

		For i = 0 To n - 1
			Dim minInter, maxInter, MinAngle, MaxAngle As Double
			Dim L As Double = Functions.ReturnDistanceInMeters(GlobalVars.NavaidList(i).pPtPrj, PrevSegment.PtCenter2)

			If (PrevSegment.Turn2Dir > 0) Then
				If (PrevSegment.Turn2R < L) Then
					minInter = Functions.RadToDeg(Math.Asin(-PrevSegment.Turn2R / L))
					maxInter = 180.0 - minInter
				Else
					minInter = 0.0
					maxInter = 360.0
				End If

				Dim fTmp As Double = ExitSide * m_TurnR - (PrevSegment.Turn2R + ExitSide * PrevSegment.Turn2R) * CosaMax
				If (System.Math.Abs(fTmp) < L) Then
					MaxAngle = Functions.RadToDeg(Math.Asin(fTmp / L))
					MinAngle = -180.0 - MaxAngle
				Else
					MinAngle = 0.0
					MaxAngle = 360.0
				End If
			Else
				If PrevSegment.Turn2R < L Then
					maxInter = Functions.RadToDeg(Math.Asin(PrevSegment.Turn2R / L))
					minInter = -180.0 - maxInter
				Else
					minInter = 0.0
					maxInter = 360.0
				End If

				Dim fTmp As Double = -ExitSide * m_TurnR + (PrevSegment.Turn2R + ExitSide * PrevSegment.Turn2R) * CosaMax
				If System.Math.Abs(fTmp) < L Then
					MinAngle = Functions.RadToDeg(Math.Asin(fTmp / L))
					MaxAngle = 180.0 - MinAngle
				Else
					MinAngle = 0.0
					MaxAngle = 360.0
				End If
			End If

			If maxInter < minInter Then maxInter = 360.0 + maxInter
			If MaxAngle < MinAngle Then MaxAngle = 360.0 + MaxAngle

			Dim MinOut As Double = Math.Max(minInter, MinAngle)
			Dim MaxOut As Double = Math.Min(maxInter, MaxAngle)

			If (MinOut < MaxOut) Then

				Dim phi As Double = Functions.ReturnAngleInDegrees(GlobalVars.NavaidList(i).pPtPrj, PrevSegment.PtCenter2)
				j += 1

				ComboBox701.Items.Add(GlobalVars.NavaidList(i))
				If (MinOut = 0.0) And (MaxOut = 360.0) Then
					Type7_Intervals(j).Left = 0.0
					Type7_Intervals(j).Right = 360.0
				Else
					Type7_Intervals(j).Left = NativeMethods.Modulus(phi + MinOut)
					Type7_Intervals(j).Right = NativeMethods.Modulus(phi + MaxOut)
				End If
			End If
		Next

		If (ComboBox701.Items.Count > 0) Then ComboBox701.SelectedIndex = 0
	End Sub

	Sub FillComboBox703List()
		Dim i As Integer
		Dim k As Integer = ComboBox701.SelectedIndex
		Dim n As Integer = GlobalVars.WPTList.Length

		ComboBox703.Items.Clear()

		For i = 0 To n - 1

			If (GlobalVars.WPTList(i).Name <> ComboBox701.Text) Then

				Dim NewVal As Double = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, GlobalVars.WPTList(i).pPtPrj)
				If (Type7_Intervals(k).Left = 0.0) And (Type7_Intervals(k).Right = 360.0) Then
					ComboBox703.Items.Add(GlobalVars.WPTList(i))
				ElseIf (Functions.AngleInSector(NewVal, Type7_Intervals(k).Left, Type7_Intervals(k).Right)) Then
					ComboBox703.Items.Add(GlobalVars.WPTList(i))
				ElseIf (Functions.AngleInSector(NewVal + 180.0, Type7_Intervals(k).Left, Type7_Intervals(k).Right)) Then
					ComboBox703.Items.Add(GlobalVars.WPTList(i))
				End If
			End If
		Next

		OptionButton702.Enabled = ComboBox703.Items.Count > 0

		If OptionButton702.Enabled Then
			ComboBox703.SelectedIndex = 0
		Else
			OptionButton701.Checked = True
		End If
	End Sub

#End Region

#Region "Common events"

	Private Sub HelpBtn_Click(sender As Object, e As EventArgs) Handles HelpBtn.Click
		NativeMethods.HtmlHelp(0, "PANDA.chm", GlobalVars.HH_HELP_CONTEXT, HelpContextID)
	End Sub

	Private Sub OkBtn_Click(sender As Object, e As EventArgs) Handles OkBtn.Click
		ClearSegmentDrawings(True)

		'If IsSectorized Then
		'	If (RadialConflict(CurrSegment)) Then
		'		If (MessageBox.Show(My.Resources.str00907, "", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.Cancel) Then
		'			Return
		'		End If
		'	End If
		'End If
		screenCapture.Save(Me)
		Hide()

		m_MasterForm.DialogHook(1, CurrSegment, m_PDG)

	End Sub

	Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
		ClearSegmentDrawings(True)
		Hide()

		Dim tmpSegment As TraceSegment = New TraceSegment()
		m_MasterForm.DialogHook(0, tmpSegment)
	End Sub

	Private Sub SSTab1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SSTab1.SelectedIndexChanged
		If Not IsLoaded Then Return

		Select Case SSTab1.SelectedIndex

			Case 0
				Do_Option001()
			Case 1
				Do_Option002()
			Case 2
				Do_Option003()
			Case 3
				Do_Option004()
			Case 4
				Do_Option005()
			Case 5
				Do_Option006()
			Case 6
				Do_Option007()
		End Select

	End Sub

	Private Sub ComboBox001_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox001.SelectedIndexChanged
		If Not IsLoaded Then Return

		If ComboBox001.SelectedIndex = 0 Then
			TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart, eRoundMode.NEAREST).ToString()
		Else
			TextBox002.Text = Functions.ConvertHeight(CurrSegment.HStart - m_refHeight, eRoundMode.NEAREST).ToString()
		End If
	End Sub

	Private Sub ComboBox002_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox002.SelectedIndexChanged
		If Not IsLoaded Then Return

		If ComboBox002.SelectedIndex = 0 Then
			TextBox003.Text = Functions.ConvertHeight(m_HFinish, eRoundMode.NEAREST).ToString()
		Else
			TextBox003.Text = Functions.ConvertHeight(m_HFinish - m_refHeight, eRoundMode.NEAREST).ToString()
		End If
	End Sub

	Private Sub TextBox003_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox003.KeyPress
		Dim KeyAscii As Char = e.KeyChar

		If KeyAscii = Chr(13) Then
			TextBox003_Validating(TextBox003, Nothing)
		Else
			TextBoxFloat(KeyAscii, TextBox003.Text)
			bVTextBox003 = True
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox003_Leave(sender As Object, e As EventArgs) Handles TextBox003.Leave
		TextBox003_Validating(TextBox003, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub TextBox003_Validating(sender As Object, e As CancelEventArgs) Handles TextBox003.Validating
		If Not bVTextBox003 Then Return

		bVTextBox003 = False

		Dim fTmp As Double
		If Not Double.TryParse(TextBox003.Text, fTmp) Then Return

		Dim hMax As Double = Math.Min(CurrSegment.HStart + CurrSegment.Length * m_PDG, cMaxH)
		Dim h As Double = Functions.DeConvertHeight(fTmp)

		If ComboBox002.SelectedIndex = 1 Then h += m_refHeight

		m_HFinish = h

		If m_HFinish < CurrSegment.HStart Then
			m_HFinish = CurrSegment.HStart
			ComboBox002_SelectedIndexChanged(ComboBox002, Nothing)
		ElseIf (m_HFinish > hMax) Then
			m_HFinish = hMax
			ComboBox002_SelectedIndexChanged(ComboBox002, Nothing)
		End If

		CurrSegment.HFinish = m_HFinish
	End Sub

	'Private Sub TextBox004_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox004.KeyPress
	'	Dim KeyAscii As Char = e.KeyChar
	'	If KeyAscii = Chr(13) Then
	'		TextBox004_Validating(TextBox004, Nothing)
	'	Else
	'		TextBoxFloat(KeyAscii, TextBox004.Text)
	'		bVTextBox004 = True
	'	End If

	'	e.KeyChar = KeyAscii
	'	If KeyAscii = Chr(0) Then e.Handled = True
	'End Sub

	'Private Sub TextBox004_Leave(sender As Object, e As EventArgs) Handles TextBox004.Leave
	'	TextBox004_Validating(TextBox004, Nothing)
	'End Sub

	'Private Sub TextBox004_Validating(sender As Object, e As CancelEventArgs) Handles TextBox004.Validating
	'	If Not bVTextBox004 Then Return

	'	bVTextBox004 = False
	'	Dim fTmp As Double
	'	If Not Double.TryParse(TextBox004.Text, fTmp) Then Return

	'	fTmp *= 0.01
	'	m_PDG = System.Math.Round(fTmp, 3)

	'	If m_PDG < PANS_OPS_DataBase.dpPDG_Nom.Value Then m_PDG = PANS_OPS_DataBase.dpPDG_Nom.Value
	'	If m_PDG > PANS_OPS_DataBase.dpMaxPosPDG.Value Then m_PDG = PANS_OPS_DataBase.dpMaxPosPDG.Value

	'	If m_PDG <> fTmp Then TextBox004.Text = (100.0 * m_PDG).ToString()

	'	ConstructNextSegment()
	'End Sub

	Private Sub TextBox005_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox005.KeyPress

		Dim KeyAscii As Char = e.KeyChar
		If (KeyAscii = Chr(13)) Then
			TextBox005_Validating(TextBox005, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox005.Text)
			bVTextBox005 = True
		End If

		e.KeyChar = KeyAscii
		If (KeyAscii = Chr(0)) Then e.Handled = True
	End Sub

	Private Sub TextBox005_Leave(sender As Object, e As EventArgs) Handles TextBox005.Leave
		TextBox005_Validating(TextBox005, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub TextBox005_Validating(sender As Object, e As CancelEventArgs) Handles TextBox005.Validating
		If Not bVTextBox005 Then Return
		bVTextBox005 = False

		Dim fTmp As Double
		If (Not Double.TryParse(TextBox005.Text, fTmp)) Then Return

		m_seminWidth = Functions.DeConvertDistance(fTmp)
		fTmp = m_seminWidth

		If (m_seminWidth < AreaMinSeminwidth) Then
			m_seminWidth = AreaMinSeminwidth
		ElseIf (m_seminWidth > AreaMaxSeminwidth) Then
			m_seminWidth = AreaMaxSeminwidth
		End If

		If m_seminWidth <> fTmp Then TextBox005.Text = Functions.ConvertDistance(m_seminWidth, eRoundMode.NEAREST).ToString()

		ConstructNextSegment()
	End Sub

	Private Sub TextBox006_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox006.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox006_Validating(TextBox006, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox006.Text)
			bVTextBox006 = True
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox006_Leave(sender As Object, e As EventArgs) Handles TextBox006.Leave
		TextBox006_Validating(TextBox006, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub TextBox006_Validating(sender As Object, e As CancelEventArgs) Handles TextBox006.Validating
		If Not bVTextBox006 Then Return
		bVTextBox006 = False

		Dim fTmp As Double
		If Not Double.TryParse(TextBox006.Text, fTmp) Then Return

		m_BankAngle = fTmp

		'if (m_BankAngle < PANS_OPS_DataBase.dpT_Bank.Value)
		'	m_BankAngle = PANS_OPS_DataBase.dpT_Bank.Value;
		If m_BankAngle < 3 Then
			m_BankAngle = 3
		ElseIf (m_BankAngle > 25) Then
			m_BankAngle = 25
		End If

		If m_BankAngle <> fTmp Then TextBox006.Text = m_BankAngle.ToString("0.0")

		Dim fTASl As Double = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC)
		m_TurnR = Functions.Bank2Radius(m_BankAngle, fTASl)
		TextBox007.Text = Functions.ConvertDistance(m_TurnR, eRoundMode.NEAREST).ToString()

		SSTab1_SelectedIndexChanged(SSTab1, New System.EventArgs())
	End Sub
#End Region

#Region "Type 1"

	Private Sub ComboBox101_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox101.SelectedIndexChanged
		If ComboBox101.SelectedIndex < 0 Then Return

		Type1_CurWPtFix = ComboBox101List(ComboBox101.SelectedIndex)
		ConstructNextSegment()
	End Sub

	Private Sub ComboBox102_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox102.SelectedIndexChanged
		Dim fTmp As Double      ', navX, navY;
		Dim inter As Interval
		'Dim pPtNav As IPoint
		Dim k As Integer = ComboBox102.SelectedIndex

		If k < 0 Then Return

		Type1_CurNavaid = ComboBox102List(k)
		Label104.Text = Navaids_DataBase.GetNavTypeName(Type1_CurNavaid.TypeCode)

		If Type1_CurNavaid.TypeCode <> eNavaidType.DME Then
			Type1_CurNavaid.IntersectionType = eIntersectionType.ByAngle

			ComboBox103.Enabled = False
			Label105.Text = "°"

			inter.Left = Type1_CurNavaid.ValMin(0)
			inter.Right = Type1_CurNavaid.ValMax(0)

			Type1_RadInterval.Left = m_CurrDir + inter.Left
			Type1_RadInterval.Right = m_CurrDir + inter.Right

			inter.Left = Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Right)
			inter.Right = Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, Type1_RadInterval.Left)
			Label107.Text = FormatInterval(inter)
			If Not Double.TryParse(TextBox102.Text, fTmp) Then TextBox102.Text = ConvertAngle(inter.Left).ToString()
			TextBox102_Validating(TextBox102, Nothing)
		Else
			Type1_CurNavaid.IntersectionType = eIntersectionType.ByDistance

			Label105.Text = GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit

			Dim inx As Integer = Type1_CurNavaid.ValMin.Length - 1
			ComboBox103.Enabled = inx > 0

			If (inx > 0) Then
				If ComboBox103.SelectedIndex < 0 Then
					ComboBox103.SelectedIndex = 0
				Else
					ComboBox103_SelectedIndexChanged(ComboBox103, Nothing)
				End If

			Else
				If ComboBox103.SelectedIndex = 0 Then
					ComboBox103_SelectedIndexChanged(ComboBox103, Nothing)
				Else
					ComboBox103.SelectedIndex = 0
				End If
			End If
		End If
	End Sub

	Private Sub ComboBox103_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox103.SelectedIndexChanged
		Dim k, inx As Integer
		Dim inter As Interval

		k = ComboBox102.SelectedIndex
		inx = ComboBox103.SelectedIndex

		inter.Left = ComboBox102List(k).ValMin(inx)
		inter.Right = ComboBox102List(k).ValMax(inx)


		Label107.Text = My.Resources.str60313 +
			Functions.ConvertDistance(inter.Left, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit + " " +
			Functions.ConvertDistance(inter.Right, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit

		Dim fTmp As Double
		If Not Double.TryParse(TextBox102.Text, fTmp) Then TextBox102.Text = Functions.ConvertDistance(inter.Left, eRoundMode.NEAREST).ToString()

		TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub OptionButton101_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton101.CheckedChanged
		If Not CType(sender, RadioButton).Checked Then Return

		TextBox101.ReadOnly = False
		TextBox101.BackColor = System.Drawing.SystemColors.Window

		ComboBox101.Enabled = False
		ComboBox102.Enabled = False
		ComboBox103.Enabled = False

		TextBox102.Enabled = False

		Label106.Visible = False
		Label107.Visible = False

		Dim fTmp As Double
		If Not Double.TryParse(TextBox101.Text, fTmp) Then TextBox101.Text = Functions.ConvertDistance(fMinDist, eRoundMode.CEIL).ToString()

		TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub OptionButton102_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton102.CheckedChanged
		If Not CType(sender, RadioButton).Checked Then Return

		TextBox101.ReadOnly = True
		TextBox101.BackColor = System.Drawing.SystemColors.Control

		ComboBox101.Enabled = True
		ComboBox102.Enabled = False
		ComboBox103.Enabled = False

		TextBox102.Enabled = False
		Label104.Visible = False

		Label106.Visible = False
		Label107.Visible = False

		If (ComboBox101.SelectedIndex < 0) Then
			ComboBox101.SelectedIndex = 0
		Else
			ComboBox101_SelectedIndexChanged(ComboBox101, New System.EventArgs())
		End If
	End Sub

	Private Sub OptionButton103_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton103.CheckedChanged
		If Not CType(sender, RadioButton).Checked Then Return

		TextBox101.ReadOnly = True
		TextBox101.BackColor = System.Drawing.SystemColors.Control

		ComboBox101.Enabled = False
		ComboBox102.Enabled = True
		ComboBox103.Enabled = False

		TextBox102.Enabled = True
		Label104.Visible = True

		Label106.Visible = True
		Label107.Visible = True

		If (ComboBox102.SelectedIndex < 0) Then
			ComboBox102.SelectedIndex = 0
		Else
			ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
		End If
	End Sub

	Private Sub TextBox101_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox101.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox101.Text)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox101_Validating(sender As Object, e As CancelEventArgs) Handles TextBox101.Validating
		Dim fTmp As Double
		If Not Double.TryParse(TextBox101.Text, fTmp) Then Return

		Type1_CurDist = Functions.DeConvertDistance(fTmp)

		If (Type1_CurDist < fMinDist) Then
			Type1_CurDist = fMinDist
			TextBox101.Text = Functions.ConvertDistance(Type1_CurDist, eRoundMode.CEIL).ToString()
		ElseIf (Type1_CurDist > GlobalVars.MaxNAVDist) Then
			Type1_CurDist = GlobalVars.MaxNAVDist
			TextBox101.Text = Functions.ConvertDistance(Type1_CurDist, eRoundMode.FLOOR).ToString()
		End If

		ConstructNextSegment()
	End Sub

	Private Sub TextBox102_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox102.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs())
		ElseIf Type1_CurNavaid.TypeCode = eNavaidType.DME Then
			TextBoxFloat(KeyAscii, TextBox102.Text)
		Else
			TextBoxInteger(KeyAscii)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox102_Validating(sender As Object, e As CancelEventArgs) Handles TextBox102.Validating
		Dim NewVal, fTmp As Double

		If Not Double.TryParse(TextBox102.Text, fTmp) Then Return

		If Type1_CurNavaid.TypeCode <> eNavaidType.DME Then

			NewVal = Functions.Azt2Dir(Type1_CurNavaid.pPtGeo, fTmp)
			fTmp = NewVal

			If Not Functions.AngleInInterval(NewVal, Type1_RadInterval) Then
				If Functions.SubtractAngles(Type1_RadInterval.Left, NewVal) < Functions.SubtractAngles(Type1_RadInterval.Right, NewVal) Then
					NewVal = Type1_RadInterval.Left
				Else
					NewVal = Type1_RadInterval.Right
				End If
			End If

			If fTmp <> NewVal Then TextBox102.Text = ConvertAngle(Functions.Dir2Azt(Type1_CurNavaid.pPtPrj, NewVal)).ToString()
		Else
			Dim inx As Integer = ComboBox103.SelectedIndex

			NewVal = Functions.DeConvertDistance(fTmp)
			fTmp = NewVal

			If (NewVal < Type1_CurNavaid.ValMin(inx)) Then
				NewVal = Type1_CurNavaid.ValMin(inx)
			ElseIf (NewVal > Type1_CurNavaid.ValMax(inx)) Then
				NewVal = Type1_CurNavaid.ValMax(inx)
			End If

			If fTmp <> NewVal Then TextBox102.Text = Functions.ConvertDistance(NewVal, eRoundMode.NEAREST).ToString()
		End If

		Type1_CurDir = NewVal
		ConstructNextSegment()

	End Sub
#End Region

#Region "Type 2"
	Private Sub ComboBox201_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox201.SelectedIndexChanged
		If ComboBox201.SelectedIndex < 0 Then Return

		Type2_CurTurnDir = IIf(ComboBox201.SelectedIndex = 0, 1, -1)

		If (IsLoaded) Then TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub TextBox201_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox201.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(KeyAscii)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox201_Validating(sender As Object, e As CancelEventArgs) Handles TextBox201.Validating
		Dim fTmp As Double

		If Not Double.TryParse(TextBox201.Text, fTmp) Then Return

		Dim pPtGeo As IPoint = Functions.ToGeo(m_CurrPnt)
		Type2_CurDir = Functions.Azt2Dir(pPtGeo, fTmp + m_MagVar)

		ConstructNextSegment()
	End Sub
#End Region

#Region "Type 3"
	Sub Type3_SetNewDirection(NewDir As Double)

		If Not Functions.AngleInInterval(NewDir, Type3_CurrInterval) Then
			If Functions.SubtractAngles(Type3_CurrInterval.Left, NewDir) < Functions.SubtractAngles(Type3_CurrInterval.Right, NewDir) Then
				NewDir = Type3_CurrInterval.Left
			Else
				NewDir = Type3_CurrInterval.Right
			End If
		End If

		Type3_CurDir = NewDir
		ConstructNextSegment()
	End Sub

	Private Sub ComboBox301_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox301.SelectedIndexChanged
		Dim k As Integer = ComboBox301.SelectedIndex
		If k < 0 Then Return

		Type3_CurNav = ComboBox301.SelectedItem

		Label302.Text = Navaids_DataBase.GetNavTypeName(Type3_CurNav.TypeCode)

		Dim i As Integer = ComboBox302.SelectedIndex
		ComboBox302.Enabled = (ComboBox301_LIntervals(k).Right > 0.0) And (ComboBox301_RIntervals(k).Right > 0.0)

		If (ComboBox301_LIntervals(k).Right < 0.0) And (i = 0) Then
			ComboBox302.SelectedIndex = 1
		ElseIf (ComboBox301_RIntervals(k).Right < 0.0) And (i = 1) Then
			ComboBox302.SelectedIndex = 0
		End If

		If i = ComboBox302.SelectedIndex Then ComboBox302_SelectedIndexChanged(ComboBox302, New System.EventArgs())
	End Sub

	Private Sub ComboBox302_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox302.SelectedIndexChanged
		Dim NavDir, TurnAngle As Double
		Dim inter As Interval

		If Not IsLoaded Then Return
		If ComboBox302.SelectedIndex < 0 Then Return

		Dim k As Integer = ComboBox301.SelectedIndex

		If ComboBox302.SelectedIndex = 0 Then
			Type3_CurTurnDir = 1
			inter = ComboBox301_LIntervals(k)

			Type3_CurrInterval.Left = m_CurrDir + inter.Left
			Type3_CurrInterval.Right = m_CurrDir + inter.Right
		Else
			Type3_CurTurnDir = -1
			inter = ComboBox301_RIntervals(k)

			Type3_CurrInterval.Left = m_CurrDir - inter.Right
			Type3_CurrInterval.Right = m_CurrDir - inter.Left
		End If

		Dim i As Integer
		Dim j As Integer = -1
		Dim n As Integer = GlobalVars.WPTList.Length
		'ComboBox303List = new WPT_FIXType(n)

		ComboBox303.Items.Clear()

		For i = 0 To n - 1

			If GlobalVars.WPTList(i).Name <> ComboBox301.Text Then
				If (Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrDir, GlobalVars.WPTList(i).pPtPrj) = Type3_CurTurnDir) Then
					NavDir = Functions.ReturnAngleInDegrees(GlobalVars.WPTList(i).pPtPrj, Type3_CurNav.pPtPrj)
				Else
					NavDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, GlobalVars.WPTList(i).pPtPrj)
				End If

				TurnAngle = NativeMethods.Modulus(Type3_CurTurnDir * (NavDir - m_CurrDir))

				If (TurnAngle >= inter.Left) And (TurnAngle <= inter.Right) Then
					j += 1
					ComboBox303.Items.Add(GlobalVars.WPTList(i))
				End If
			End If
		Next

		inter.Left = Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_CurrInterval.Right)
		inter.Right = Functions.Dir2Azt(Type3_CurNav.pPtPrj, Type3_CurrInterval.Left)
		Label312.Text = FormatInterval(inter)
		j += 1
		CheckBox301.Enabled = j > 0
		'Array.Resize<WPT_FIXType>(ref ComboBox303List, j)

		If (j > 0) Then
			ComboBox303.SelectedIndex = 0
		Else
			CheckBox301.Checked = False
		End If
		If Not (CheckBox301.Checked And CheckBox301.Enabled) Then TextBox302_Validating(TextBox302, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub ComboBox303_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox303.SelectedIndexChanged
		If Not (CheckBox301.Checked And CheckBox301.Enabled) Then Return

		Dim k As Integer = ComboBox303.SelectedIndex
		If k < 0 Then Return

		Dim pToPoint As IPoint = (ComboBox303.SelectedItem).pPtPrj      'ComboBox303List[K].pPtPrj;
		Dim NewDir As Double

		If Functions.SideDef(Type3_CurNav.pPtPrj, m_CurrDir, pToPoint) = Type3_CurTurnDir Then
			NewDir = Functions.ReturnAngleInDegrees(pToPoint, Type3_CurNav.pPtPrj)
		Else
			NewDir = Functions.ReturnAngleInDegrees(Type3_CurNav.pPtPrj, pToPoint)
		End If

		Type3_SetNewDirection(NewDir)
	End Sub

	Private Sub CheckBox301_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox301.CheckedChanged
		If Not CheckBox301.Checked Then
			ComboBox303.Enabled = False
			TextBox302.ReadOnly = False
			TextBox302.BackColor = System.Drawing.SystemColors.Window
			Label304.Visible = True
		Else
			Label304.Visible = True
			ComboBox303.Enabled = True
			TextBox302.ReadOnly = True
			TextBox302.BackColor = System.Drawing.SystemColors.Control
			ComboBox303_SelectedIndexChanged(ComboBox303, New System.EventArgs())
		End If
	End Sub

	Private Sub TextBox302_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox302.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox302_Validating(TextBox302, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(KeyAscii)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox302_Validating(sender As Object, e As CancelEventArgs) Handles TextBox302.Validating
		Dim fTmp, fDir As Double

		If Not Double.TryParse(TextBox302.Text, fTmp) Then Return

		fDir = Functions.Azt2Dir(Type3_CurNav.pPtGeo, fTmp)
		Type3_SetNewDirection(fDir)
	End Sub
#End Region

#Region "Type 4"
	Private Sub ComboBox401_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox401.SelectedIndexChanged
		If ComboBox401.SelectedIndex < 0 Then Return

		Type4_CurFix = ComboBox401.SelectedItem
		ConstructNextSegment()
	End Sub

	Private Sub ComboBox402_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox402.SelectedIndexChanged
		If ComboBox402.SelectedIndex < 0 Then Return
		Type4_CurTurnDir = IIf(ComboBox402.SelectedIndex = 0, 1, -1)
		If IsLoaded Then ConstructNextSegment()
	End Sub
#End Region

#Region "Type 5"
	Private Sub CheckBox501_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox501.CheckedChanged
		If Not CheckBox501.Checked Then
			ComboBox503.Enabled = False
			TextBox505.ReadOnly = False
			TextBox505.BackColor = System.Drawing.SystemColors.Window
		Else
			ComboBox503.Enabled = True
			Label519.Visible = True
			TextBox505.ReadOnly = True
			TextBox505.BackColor = System.Drawing.SystemColors.Control

			Dim i As Integer
			Dim n As Integer = GlobalVars.WPTList.Length

			ComboBox503.Items.Clear()

			For i = 0 To n - 1
				If (GlobalVars.WPTList(i).Name <> ComboBox501.Text) And (Functions.ReturnDistanceInMeters(m_CurrPnt, GlobalVars.WPTList(i).pPtPrj) < GlobalVars.MaxNAVDist) Then
					ComboBox503.Items.Add(GlobalVars.WPTList(i))
				End If
			Next

			If ComboBox503.Items.Count > 0 Then ComboBox503.SelectedIndex = 0
		End If
	End Sub

	Private Sub ComboBox501_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox501.SelectedIndexChanged
		If ComboBox501.SelectedIndex < 0 Then Return

		Type5_CurNav = ComboBox501.SelectedItem
		Label512.Text = Navaids_DataBase.GetNavTypeName(Type5_CurNav.TypeCode)

		TextBox506_Validating(TextBox506, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub ComboBox502_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox502.SelectedIndexChanged
		If ComboBox502.SelectedIndex < 0 Then Return

		If ComboBox502.SelectedIndex = 0 Then
			Type5_CurTurnDir = 1
		Else
			Type5_CurTurnDir = -1
		End If

		If IsLoaded Then TextBox506_Validating(TextBox506, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub ComboBox503_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox503.SelectedIndexChanged
		If ComboBox503.SelectedIndex < 0 Then Return

		Dim pToPoint As IPoint = CType(ComboBox503.SelectedItem, WPT_FIXType).pPtPrj
		Type5_CurDir = Functions.ReturnAngleInDegrees(Type5_CurNav.pPtPrj, pToPoint)
		ConstructNextSegment()
	End Sub

	Private Sub TextBox505_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox505.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox505_Validating(TextBox505, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(KeyAscii)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox505_Validating(sender As Object, e As CancelEventArgs) Handles TextBox505.Validating
		Dim fCourse As Double
		If Not Double.TryParse(TextBox505.Text, fCourse) Then Return

		Type5_CurDir = Functions.Azt2Dir(Type5_CurNav.pPtGeo, fCourse + m_MagVar)
		ConstructNextSegment()
	End Sub

	Private Sub TextBox506_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox506.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox506_Validating(TextBox506, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(KeyAscii)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox506_Validating(sender As Object, e As CancelEventArgs) Handles TextBox506.Validating
		Dim fTmp As Double
		If Not Double.TryParse(TextBox506.Text, fTmp) Then Return

		Type5_SnapAngle = fTmp

		If Type5_SnapAngle < 10.0 Then                                                  'PANS_OPS_DataBase.dpInterMinAngle.Value
			Type5_SnapAngle = 10.0                                                      'PANS_OPS_DataBase.dpInterMinAngle.Value
		ElseIf Type5_SnapAngle > 85.0 Then                                              'PANS_OPS_DataBase.dpInterMaxAngle.Value
			Type5_SnapAngle = 85.0                                                      'PANS_OPS_DataBase.dpInterMaxAngle.Value
		End If

		If fTmp <> Type5_SnapAngle Then TextBox506.Text = Type5_SnapAngle.ToString()

		Type5UpdateIntervals()
	End Sub

#End Region

#Region "Type 6"
	Private Sub ComboBox601_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox601.SelectedIndexChanged
		If Not IsLoaded Then Return
		FillComboBox603DMEStations()
		If ComboBox603.Items.Count > 0 Then ComboBox603.SelectedIndex = 0
	End Sub

	Private Sub ComboBox602_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox602.SelectedIndexChanged
		If Not IsLoaded Then Return
		FillComboBox603DMEStations()
		If ComboBox603.Items.Count > 0 Then ComboBox603.SelectedIndex = 0
	End Sub

	Private Sub ComboBox603_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox603.SelectedIndexChanged
		Dim k As Integer = ComboBox603.SelectedIndex
		If k < 0 Then Return

		Type6_CurNav = CType(ComboBox603.SelectedItem, NavaidData)
		ToolTip1.SetToolTip(TextBox605, "Min: " + Functions.ConvertDistance(Type6_Intervals(k).Left, eRoundMode.NEAREST).ToString() +
										" Max: " + Functions.ConvertDistance(Type6_Intervals(k).Right, eRoundMode.NEAREST).ToString())

		TextBox605_Validating(TextBox605, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub TextBox605_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox605.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox605_Validating(TextBox605, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox605.Text)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox605_Validating(sender As Object, e As CancelEventArgs) Handles TextBox605.Validating
		Dim fTmp As Double

		If Not Double.TryParse(TextBox605.Text, fTmp) Then Return

		Dim NewVal As Double = Functions.DeConvertDistance(fTmp)

		fTmp = NewVal
		Dim k As Integer = ComboBox603.SelectedIndex
		'if (K < 0) return
		Type6_CurNav = CType(ComboBox603.SelectedItem, NavaidData)      'ComboBox603List(k)

		If NewVal < Type6_Intervals(k).Left Then
			NewVal = Type6_Intervals(k).Left
		ElseIf NewVal > Type6_Intervals(k).Right Then
			NewVal = Type6_Intervals(k).Right
		End If

		If NewVal <> fTmp Then TextBox605.Text = Functions.ConvertDistance(NewVal, eRoundMode.NEAREST).ToString()

		ConstructNextSegment()
	End Sub

#End Region

#Region "Type 7"

	Private Sub ComboBox701_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox701.SelectedIndexChanged
		Dim k As Integer = ComboBox701.SelectedIndex
		If k < 0 Then Return

		Type7_CurNav = CType(ComboBox701.SelectedItem, NavaidData)      ' ComboBox701List(k)
		If (Type7_Intervals(k).Left <> 0.0) Or (Type7_Intervals(k).Right <> 360.0) Then
			ToolTip1.SetToolTip(TextBox703, "Min: " + ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals(k).Right)).ToString() +
					" Max: " + ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals(k).Left)).ToString())
		Else
			ToolTip1.SetToolTip(TextBox703, "Min: 0 Max: 360")
		End If

		FillComboBox703List()
		If (OptionButton701.Checked) Then
			TextBox703.Text = ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, Type7_Intervals(k).Right)).ToString()
			TextBox703_Validating(TextBox703, New System.ComponentModel.CancelEventArgs())
		End If
	End Sub

	Private Sub ComboBox702_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox702.SelectedIndexChanged
		If Not IsLoaded Then Return
		FillComboBox701Stations()
	End Sub

	Private Sub ComboBox703_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox703.SelectedIndexChanged
		If Not OptionButton702.Checked Then Return

		Dim k As Integer = ComboBox703.SelectedIndex
		If (k < 0) Then Return

		Type7_CurFix = CType(ComboBox703.SelectedItem, WPT_FIXType)      'ComboBox703List[K]

		k = ComboBox701.SelectedIndex
		If (k < 0) Then Return

		Label702.Text = Navaids_DataBase.GetNavTypeName(Type7_CurFix.TypeCode)

		Dim fTmp As Double = Functions.ReturnAngleInDegrees(Type7_CurNav.pPtPrj, Type7_CurFix.pPtPrj)

		If (Type7_Intervals(k).Left = 0.0) And (Type7_Intervals(k).Right = 360.0) Then
			ComboBox704.Items.Clear()

			ComboBox704.Items.Add(ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp)).ToString())
			ComboBox704.Items.Add(ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, fTmp + 180.0)).ToString())

			'	TextBox703.Locked = true
			'	TextBox703.BackColor = vbButtonFace
			'	ComboBox704.Enabled = true

			TextBox703.Visible = False
			ComboBox704.Visible = True
			ComboBox704.SelectedIndex = 0
		Else
			'	TextBox703.Locked = false;
			'	TextBox703.BackColor = vbWindowBackground;
			'	ComboBox704.Enabled = false;
			TextBox703.Visible = True
			ComboBox704.Visible = False

			Dim NewVal As Double

			If Functions.AngleInSector(fTmp, Type7_Intervals(k).Left, Type7_Intervals(k).Right) Then
				NewVal = fTmp
			Else
				NewVal = fTmp + 180.0
			End If

			TextBox703.Text = ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, NewVal)).ToString()
			TextBox703_Validating(TextBox703, New System.ComponentModel.CancelEventArgs())
		End If
	End Sub

	Private Sub ComboBox704_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox704.SelectedIndexChanged
		TextBox703.Text = ComboBox704.Text
		TextBox703_Validating(TextBox703, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub OptionButton701_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton701.CheckedChanged
		If Not OptionButton701.Checked Then Return

		ComboBox703.Enabled = False

		TextBox703.Visible = True
		ComboBox704.Visible = False

		TextBox703.ReadOnly = False
		TextBox703.BackColor = System.Drawing.SystemColors.Window

		TextBox703_Validating(TextBox703, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub OptionButton702_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton702.CheckedChanged
		If Not OptionButton702.Checked Then Return

		ComboBox703.Enabled = True
		ComboBox703_SelectedIndexChanged(ComboBox703, New System.EventArgs())
	End Sub

	Private Sub TextBox703_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox703.KeyPress
		Dim KeyAscii As Char = e.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox703_Validating(TextBox703, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox703.Text)
		End If

		e.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox703_Validating(sender As Object, e As CancelEventArgs) Handles TextBox703.Validating
		Dim fTmp As Double

		If Not Double.TryParse(TextBox703.Text, fTmp) Then Return

		Dim k As Integer = ComboBox701.SelectedIndex
		'if (k < 0) return

		Type7_CurNav = CType(ComboBox701.SelectedItem, NavaidData)

		Dim NewVal As Double = Functions.Azt2Dir(Type7_CurNav.pPtGeo, fTmp)

		If ((Type7_Intervals(k).Left <> 0.0) Or (Type7_Intervals(k).Right <> 360.0)) And Not Functions.AngleInSector(NewVal, Type7_Intervals(k).Left, Type7_Intervals(k).Right) Then
			If Functions.AnglesSideDef(NewVal, Type7_Intervals(k).Left) < 0 Then
				NewVal = Type7_Intervals(k).Left
			Else
				NewVal = Type7_Intervals(k).Right
			End If

			TextBox703.Text = ConvertAngle(Functions.Dir2Azt(Type7_CurNav.pPtPrj, NewVal)).ToString()
		End If

		ConstructNextSegment()

	End Sub
#End Region

#Region "Do :)"
	Sub Do_Option001()
		OptionButton102.Enabled = FillComboBox101Stations() > 0
		OptionButton103.Enabled = FillComboBox102Stations() > 0

		OptionButton102.Checked = False
		OptionButton103.Checked = False

		If OptionButton101.Checked Then
			OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
		Else
			OptionButton101.Checked = True
		End If
	End Sub

	Sub Do_Option002()
		TextBox201.Text = "0"
		TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs())
	End Sub

	Sub Do_Option003()
		FillComboBox301Stations()
		If ComboBox301.Items.Count > 0 Then ComboBox301.SelectedIndex = 0
	End Sub

	Sub Do_Option004()
		FillComboBox401Stations()
		If ComboBox401.Items.Count > 0 Then ComboBox401.SelectedIndex = 0
	End Sub

	Sub Do_Option005()
		TextBox506.Text = "30"
		FillComboBox501Stations()

		If ComboBox501.Items.Count > 0 Then ComboBox501.SelectedIndex = 0
	End Sub

	Sub Do_Option006()
		FillComboBox603DMEStations()
		If ComboBox603.Items.Count > 0 Then ComboBox603.SelectedIndex = 0
	End Sub

	Sub Do_Option007()
		If PrevSegment.SegmentCode <> eSegmentType.arcIntercept Then
			Exit Sub
		End If

		FillComboBox701Stations()

		If (OptionButton701.Checked) Then
			OptionButton701_CheckedChanged(OptionButton701, New System.EventArgs())
		Else
			OptionButton701.Checked = True
		End If
	End Sub

#End Region

#Region "Type 1 Implimentation"
	Function Type1SegmentOnDistance(Dist As Double, ByRef Segment As TraceSegment) As Boolean

		Dim PtOut As IPoint = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir, Dist)

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = PtOut
		Segment.DirOut = m_CurrDir

		Segment.Length = Dist
		Segment.Turn1R = 0.0

		Segment.PathPrj = New ESRI.ArcGIS.Geometry.Polyline()
		CType(Segment.PathPrj, IPointCollection).AddPoint(m_CurrPnt)
		CType(Segment.PathPrj, IPointCollection).AddPoint(PtOut)

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		Segment.Comment = My.Resources.str00509 + Functions.ConvertHeight(m_HFinish, eRoundMode.NEAREST).ToString() + " " + GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit
		Segment.RepComment = Segment.Comment

		Segment.SegmentCode = eSegmentType.straight

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Or (PrevSegment.SegmentCode = eSegmentType.arcPath) Then
			Segment.GuidanceNav = PrevSegment.InterceptionNav
		Else
			Segment.GuidanceNav = PrevSegment.GuidanceNav
		End If

		Segment.InterceptionNav.TypeCode = eNavaidType.NONE

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.directToFIX) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Then
			Segment.LegType = Aim.Enums.CodeSegmentPath.CA
		Else
			Segment.LegType = Aim.Enums.CodeSegmentPath.VA
		End If

		Return True
	End Function

	Function Type1SegmentOnWpt(WPtFix As WPT_FIXType, ByRef Segment As TraceSegment)
		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = WPtFix.pPtPrj
		Segment.DirOut = m_CurrDir

		Segment.Length = Functions.ReturnDistanceInMeters(m_CurrPnt, WPtFix.pPtPrj)
		TextBox101.Text = Functions.ConvertDistance(Segment.Length, eRoundMode.FLOOR).ToString()

		Segment.Turn1R = 0.0

		Segment.PathPrj = New ESRI.ArcGIS.Geometry.Polyline()
		CType(Segment.PathPrj, IPointCollection).AddPoint(Segment.ptIn)
		CType(Segment.PathPrj, IPointCollection).AddPoint(Segment.ptOut)

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		Segment.Comment = My.Resources.str00510 + ComboBox101.Text
		Segment.RepComment = Segment.Comment

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Or (PrevSegment.SegmentCode = eSegmentType.arcPath) Then
			Segment.GuidanceNav = PrevSegment.InterceptionNav
		Else
			Segment.GuidanceNav = PrevSegment.GuidanceNav
		End If

		If WPtFix.TypeCode <> eNavaidType.NONE Then
			Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix)
			Segment.InterceptionNav.IntersectionType = eIntersectionType.OnNavaid
		Else
			Segment.InterceptionNav.TypeCode = WPtFix.TypeCode
		End If

		Segment.SegmentCode = eSegmentType.straight
		Segment.LegType = Aim.Enums.CodeSegmentPath.CF

		Return True
	End Function

	Function Type1SegmentOnNewFIX(Navaid As NavaidData, NavDirDist As Double, ByRef Segment As TraceSegment) As Boolean
		Dim FinalPt As IPoint
		Dim NewAztDist As Double

		If Navaid.TypeCode = eNavaidType.DME Then
			If Navaid.ValCnt <> 0 Then
				NewAztDist = 90.0 * (Navaid.ValCnt - 1)
			Else
				Dim inx As Integer = 1 - ComboBox103.SelectedIndex
				NewAztDist = 180.0 * inx
			End If
			Functions.CircleVectorIntersect(Navaid.pPtPrj, NavDirDist, m_CurrPnt, m_CurrDir + NewAztDist, FinalPt)
		Else
			FinalPt = New ESRI.ArcGIS.Geometry.Point()
			Dim ConstPt As IConstructPoint = FinalPt
			ConstPt.ConstructAngleIntersection(m_CurrPnt, Functions.DegToRad(m_CurrDir), Navaid.pPtPrj, Functions.DegToRad(NavDirDist))
		End If

		If FinalPt.IsEmpty Then Return False

		'DrawPoint (FinalPt, RGB(0, 0, 255))
		'DrawPoint (m_CurrPnt, RGB(0, 255, 0))
		'DrawPoint (Navaid.pPtPrj, RGB(255, 0, 0))

		Segment.Length = Functions.ReturnDistanceInMeters(m_CurrPnt, FinalPt)
		TextBox101.Text = Functions.ConvertDistance(Segment.Length, eRoundMode.FLOOR).ToString()

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = FinalPt
		Segment.DirOut = Segment.DirIn

		Segment.Turn1R = 0

		Segment.PathPrj = New ESRI.ArcGIS.Geometry.Polyline()
		CType(Segment.PathPrj, IPointCollection).AddPoint(m_CurrPnt)
		CType(Segment.PathPrj, IPointCollection).AddPoint(FinalPt)

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		If Navaid.TypeCode = eNavaidType.VOR Then
			Segment.Comment = My.Resources.str00505 + TextBox102.Text + " " + ComboBox102.Text
		ElseIf Navaid.TypeCode = eNavaidType.NDB Then
			Segment.Comment = My.Resources.str00507 + TextBox102.Text + " " + ComboBox102.Text
		ElseIf Navaid.TypeCode = eNavaidType.DME Then
			Segment.Comment = My.Resources.str00508 + TextBox102.Text + " " + ComboBox102.Text
		End If

		Segment.RepComment = Segment.Comment
		Segment.SegmentCode = eSegmentType.straight

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Or (PrevSegment.SegmentCode = eSegmentType.arcPath) Then
			Segment.GuidanceNav = PrevSegment.InterceptionNav
		Else
			Segment.GuidanceNav = PrevSegment.GuidanceNav
		End If

		Segment.InterceptionNav = Navaid

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.directToFIX) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Then
			If (Navaid.TypeCode = eNavaidType.DME) Then
				Segment.LegType = Aim.Enums.CodeSegmentPath.CD
			Else
				Segment.LegType = Aim.Enums.CodeSegmentPath.CR
			End If
		ElseIf (Navaid.TypeCode = eNavaidType.DME) Then
			Segment.LegType = Aim.Enums.CodeSegmentPath.VD
		Else                            ' if (Navaid.TypeCode = eNavaidType.CodeNDB) then
			Segment.LegType = Aim.Enums.CodeSegmentPath.VR
		End If

		Return True
	End Function
#End Region

#Region "Type 2 Implimentation"
	Function Type2Segment(OutDir As Double, TurnDir As Integer, ByRef Segment As TraceSegment)

		Dim TurnAngle As Double = NativeMethods.Modulus((OutDir - m_CurrDir) * TurnDir)
		TextBox204.Text = ConvertAngle(TurnAngle).ToString()

		Dim pPtCenter As IPoint = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * TurnDir, m_TurnR)
		Dim Pt1 As IPoint = Functions.PointAlongPlane(pPtCenter, OutDir - 90.0 * TurnDir, m_TurnR)
		Pt1.M = OutDir

		Dim MyPC As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint()
		MyPC.AddPoint(m_CurrPnt)
		MyPC.AddPoint(Pt1)

		'---------------------------------
		Dim pPtXGeo As IPoint = Functions.ToGeo(pPtCenter)
		Segment.Center1Coords = Functions.DegreeToString((pPtXGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString((pPtXGeo.X), Degree2StringMode.DMSLon)
		Segment.Turn1Dir = -TurnDir         ' SideDef(Pt0, Pt0.M, Pt1)
		Segment.Turn1Angle = TurnAngle
		'---------------------------------

		Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC)
		Dim MyPoly As IPolyline = Segment.PathPrj
		Segment.Length = MyPoly.Length

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = Pt1
		Segment.DirOut = OutDir

		Segment.Turn1R = m_TurnR

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		Dim fTmp As Double = Double.Parse(TextBox201.Text)

		Segment.Comment = My.Resources.str60010 + TextBox001.Text + My.Resources.str60011 + TextBox201.Text
		Segment.RepComment = My.Resources.str60010 + ConvertAngle(NativeMethods.Modulus(m_CurrAzt + m_MagVar)).ToString() +
							My.Resources.str60011 + ConvertAngle(NativeMethods.Modulus(fTmp + m_MagVar)).ToString()

		Segment.GuidanceNav.TypeCode = eNavaidType.NONE
		Segment.InterceptionNav.TypeCode = eNavaidType.NONE

		Segment.LegType = Aim.Enums.CodeSegmentPath.CF
		Segment.SegmentCode = eSegmentType.toHeading
		Return True
	End Function

#End Region

#Region "Type 3 Implimentation"

	Function Type3Segment(Navaid As WPT_FIXType, NewDir As Double, ByRef Segment As TraceSegment) As Boolean

		Dim alpha, fRefX, fRefY, NewAzt, AddDist, fAbsRefY, TurnAngle As Double

		Functions.PrjToLocal(m_CurrPnt, m_CurrDir, Navaid.pPtPrj.X, Navaid.pPtPrj.Y, fRefX, fRefY)
		fAbsRefY = System.Math.Abs(fRefY)

		alpha = GlobalVars.DegToRadValue * NativeMethods.Modulus(Type3_CurTurnDir * (NewDir - m_CurrDir))

		If Type3_CurTurnDir * fRefY > 0.0 Then
			AddDist = fRefX - fAbsRefY / System.Math.Tan(alpha) - m_TurnR * System.Math.Tan(0.5 * alpha)
		Else
			AddDist = fRefX + fAbsRefY / System.Math.Tan(alpha) - m_TurnR * System.Math.Tan(0.5 * alpha)
		End If

		Dim MyPC As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint()
		MyPC.AddPoint(m_CurrPnt)

		Dim pPt0 As IPoint = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir, AddDist)
		pPt0.M = m_CurrDir

		Dim ptCenter As IPoint = Functions.PointAlongPlane(pPt0, m_CurrDir + 90.0 * Type3_CurTurnDir, m_TurnR)

		Dim pPt1 As IPoint = Functions.PointAlongPlane(ptCenter, NewDir - 90.0 * Type3_CurTurnDir, m_TurnR)
		pPt1.M = NewDir

		MyPC.AddPoint(pPt0)
		MyPC.AddPoint(pPt1)
		'---------------------------------
		TurnAngle = NativeMethods.Modulus(Type3_CurTurnDir * (NewDir - m_CurrDir))
		Segment.Turn1Angle = TurnAngle
		Segment.Turn1Dir = -Type3_CurTurnDir        'SideDef(pPt0, m_CurrDir, pPt1)

		Dim pPtTmpGeo As IPoint = Functions.ToGeo(ptCenter)
		Segment.Center1Coords = Functions.DegreeToString((pPtTmpGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString((pPtTmpGeo.X), Degree2StringMode.DMSLon)

		pPtTmpGeo = Functions.ToGeo(pPt0)
		Segment.St1Coords = Functions.DegreeToString((pPtTmpGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString((pPtTmpGeo.X), Degree2StringMode.DMSLon)

		Segment.BetweenTurns = Functions.ReturnDistanceInMeters(m_CurrPnt, pPt0)
		Segment.H1 = CurrSegment.HStart + Segment.BetweenTurns * m_PDG
		'---------------------------------
		Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC)
		Dim MyPoly As IPolyline = Segment.PathPrj
		Segment.Length = MyPoly.Length

		Segment.Turn1Length = Segment.Length - Segment.BetweenTurns

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = pPt1
		Segment.DirOut = NewDir

		Segment.Turn1R = m_TurnR


		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		NewAzt = Functions.Dir2Azt(Navaid.pPtPrj, Type3_CurDir)

		TextBox301.Text = ConvertAngle(TurnAngle).ToString()
		TextBox302.Text = ConvertAngle(NewAzt).ToString()
		TextBox303.Text = Functions.ConvertDistance(AddDist, eRoundMode.NEAREST).ToString()

		Dim fTmp As Double = Double.Parse(TextBox302.Text)
		Segment.Comment = My.Resources.str00511 + ConvertAngle(NativeMethods.Modulus(fTmp - m_MagVar)).ToString() + " " + ComboBox301.Text
		Segment.RepComment = My.Resources.str00511 + TextBox302.Text + " " + ComboBox301.Text

		Segment.SegmentCode = eSegmentType.courseIntercept

		Select Case (PrevSegment.LegType)
			Case Aim.Enums.CodeSegmentPath.TF
			Case Aim.Enums.CodeSegmentPath.CA
			Case Aim.Enums.CodeSegmentPath.CD
			Case Aim.Enums.CodeSegmentPath.CI
			Case Aim.Enums.CodeSegmentPath.CR
			Case Aim.Enums.CodeSegmentPath.CF
			Case Aim.Enums.CodeSegmentPath.DF
				Segment.LegType = Aim.Enums.CodeSegmentPath.CI
			Case Else
				Segment.LegType = Aim.Enums.CodeSegmentPath.VI
		End Select

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Or (PrevSegment.SegmentCode = eSegmentType.arcPath) Then
			Segment.GuidanceNav = PrevSegment.InterceptionNav
		Else
			Segment.GuidanceNav = PrevSegment.GuidanceNav
		End If

		'Segment.InterceptionNav.TypeCode = eNavaidType.NONE
		If (Navaid.TypeCode > eNavaidType.NONE) Then
			Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(Navaid)
		Else
			Segment.InterceptionNav.TypeCode = eNavaidType.NONE
		End If

		Return True
	End Function

#End Region

#Region "Type 4 Implimentation"
	Function Type4Segment(WPtFix As WPT_FIXType, TurnDir As Integer, ByRef Segment As TraceSegment) As Boolean
		Dim OutDir, TurnAngle As Double

		Dim Pt1, pPtXGeo, pPtCenter As IPoint
		Dim MyPoly As IPolyline
		Dim MyPC As IPointCollection

		MyPC = Functions.TurnToFixPrj(m_CurrPnt, m_TurnR, TurnDir, Type4_CurFix.pPtPrj)
		Pt1 = MyPC.Point(1)
		OutDir = Pt1.M

		'Functions.DrawPointWithText(Pt1, "PtCevr")
		'Application.DoEvents()

		TextBox401.Text = ConvertAngle(NativeMethods.Modulus(Functions.Dir2Azt(Pt1, OutDir))).ToString()

		TurnAngle = NativeMethods.Modulus((OutDir - m_CurrDir) * TurnDir)
		TextBox402.Text = ConvertAngle(TurnAngle).ToString()
		Label406.Text = Navaids_DataBase.GetNavTypeName(Type4_CurFix.TypeCode)

		'---------------------------------
		pPtCenter = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * TurnDir, m_TurnR)
		pPtXGeo = Functions.ToGeo(pPtCenter)

		Segment.Center1Coords = Functions.DegreeToString((pPtXGeo.Y), Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString((pPtXGeo.X), Degree2StringMode.DMSLon)
		Segment.Turn1Dir = -TurnDir     '	Functions.SideDef(m_CurrPnt, m_CurrPnt.M, Pt1)
		Segment.Turn1Angle = TurnAngle      '	Modulus((MyPC.Point(0).M - MyPC.Point(1).M) * Segment.Turn1Dir)

		Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC)
		MyPoly = Segment.PathPrj
		Segment.Length = MyPoly.Length

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = Pt1
		Segment.DirOut = OutDir

		Segment.Turn1R = m_TurnR
		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		Segment.Comment = My.Resources.str60010 + TextBox001.Text + My.Resources.str60011 + ComboBox401.Text
		Segment.RepComment = My.Resources.str60010 + ConvertAngle(NativeMethods.Modulus(m_CurrAzt + m_MagVar)).ToString() + My.Resources.str60011 + ComboBox401.Text
		'---------------------------------

		Segment.SegmentCode = eSegmentType.directToFIX
		Segment.LegType = Aim.Enums.CodeSegmentPath.DF

		Segment.InterceptionNav.TypeCode = eNavaidType.NONE

		If WPtFix.TypeCode = eNavaidType.NONE Then
			Segment.GuidanceNav.TypeCode = eNavaidType.NONE
		Else
			Segment.GuidanceNav = Navaids_DataBase.WPT_FIXToNavaid(WPtFix)
		End If

		Return True
	End Function

#End Region

#Region "Type 5 Implimentation"
	Sub Type5UpdateIntervals()
		Dim VTotal, lMinDR, tStabl As Double
		Dim fTASl, delta, alpha, xMin, xMax As Double
		Dim Snap, fTmp, ddr, bAz, dr, d, R As Double

		tStabl = 6
		Snap = Type5_SnapAngle

		Dim FixPntPrj As IPoint = Type5_CurNav.pPtPrj
		fTASl = Functions.IAS2TAS(m_IAS, CurrSegment.HStart, CurrADHP.ISAtC)
		VTotal = fTASl + CurrADHP.WindSpeed

		'==========================================
		ddr = m_TurnR / (System.Math.Tan(GlobalVars.DegToRadValue * ((180.0 - Snap) * 0.5)))
		dr = PANS_OPS_DataBase.arT_Gui_dist.Value + ddr
		lMinDR = VTotal * tStabl * 0.277777777777778 + ddr
		'MinDR = lMinDR - ddr;
		'TextBox001.ToolTipText = LoadResString(15250) + MinDR.ToString() + LoadResString(15251)     '"Минимальная длина DR = "" метр"
		'==========================================

		Dim ptCnt As IPoint = Functions.PointAlongPlane(m_CurrPnt, m_CurrDir + 90.0 * Type5_CurTurnDir, m_TurnR)
		bAz = Functions.ReturnAngleInDegrees(FixPntPrj, ptCnt)
		d = Functions.ReturnDistanceInMeters(ptCnt, FixPntPrj)

		R = System.Math.Sqrt(dr * dr + m_TurnR * m_TurnR)

		delta = GlobalVars.RadToDegValue * (System.Math.Atan(m_TurnR / dr))
		alpha = Snap - Type5_CurTurnDir * delta

		fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d
		If (fTmp > 1.0) Then
			xMin = 90.0
		ElseIf (fTmp < -1.0) Then
			xMin = -90.0
		Else
			xMin = GlobalVars.RadToDegValue * Math.Asin(fTmp)
		End If

		alpha = Snap + Type5_CurTurnDir * delta
		fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d
		If (fTmp > 1.0) Then
			xMax = 90.0
		ElseIf (fTmp < -1.0) Then
			xMax = -90.0
		Else
			xMax = GlobalVars.RadToDegValue * Math.Asin(fTmp)
		End If

		ReDim Type5_Intervals(3)

		Type5_Intervals(0).Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMin) - m_MagVar)
		Type5_Intervals(1).Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMax) - m_MagVar)
		Type5_Intervals(2).Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMax) + 180.0 - m_MagVar)
		Type5_Intervals(3).Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMin) + 180.0 - m_MagVar)

		R = System.Math.Sqrt(lMinDR * lMinDR + m_TurnR * m_TurnR)

		delta = GlobalVars.RadToDegValue * (System.Math.Atan(m_TurnR / lMinDR))
		alpha = Snap - Type5_CurTurnDir * delta

		fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d
		If (fTmp > 1.0) Then
			xMin = 90.0
		ElseIf (fTmp < -1.0) Then
			xMin = -90.0
		Else
			xMin = GlobalVars.RadToDegValue * Math.Asin(fTmp)
		End If

		alpha = Snap + Type5_CurTurnDir * delta
		fTmp = R * System.Math.Sin(GlobalVars.DegToRadValue * alpha) / d
		If (fTmp > 1.0) Then
			xMax = 90.0
		ElseIf (fTmp < -1.0) Then
			xMax = -90.0
		Else
			xMax = GlobalVars.RadToDegValue * Math.Asin(fTmp)
		End If

		Type5_Intervals(0).Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMin) - m_MagVar)
		Type5_Intervals(1).Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMax) - m_MagVar)
		Type5_Intervals(2).Right = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz + xMax) + 180.0 - m_MagVar)
		Type5_Intervals(3).Left = NativeMethods.Modulus(Functions.Dir2Azt(FixPntPrj, bAz - xMin) + 180.0 - m_MagVar)

		Dim i As Integer
		Dim j As Integer = 0
		Dim n As Integer = 4 'Type5_Intervals.Length

		'SortIntervals(Type5_Intervals)

		Do While j < n - 1
			If Functions.SubtractAngles(Type5_Intervals(j).Right, Type5_Intervals(j + 1).Left) <= 1.0 Then

				Type5_Intervals(j).Right = Type5_Intervals(j + 1).Right
				n -= 1
				For i = j + 1 To n - 1
					Type5_Intervals(i) = Type5_Intervals(i + 1)
				Next
			Else
				j += 1
			End If
		Loop

		If n > 1 Then
			If Functions.SubtractAngles(Type5_Intervals(0).Left, Type5_Intervals(n - 1).Right) <= 1.0 Then
				Type5_Intervals(0).Left = Type5_Intervals(n - 1).Left
				n -= 1
			End If
		End If

		ReDim Preserve Type5_Intervals(n - 1)
		'Array.Resize<Interval>(ref Type5_Intervals, n)

		For i = 0 To n - 1
			If Functions.SubtractAngles(ConvertAngle(Type5_Intervals(i).Left), ConvertAngle(Type5_Intervals(i).Right)) < 1.0 Then
				Type5_Intervals(i).Left = ConvertAngle(Type5_Intervals(i).Left)
				Type5_Intervals(i).Right = Type5_Intervals(i).Left
			Else
				Type5_Intervals(i).Left = ConvertAngle(Type5_Intervals(i).Left, eRoundMode.CEIL)
				Type5_Intervals(i).Right = ConvertAngle(Type5_Intervals(i).Right, eRoundMode.FLOOR)
			End If
		Next

		Functions.SortIntervals(Type5_Intervals)
		Dim tmpStr As String = ""

		For i = 0 To n - 1
			If Functions.SubtractAngles(Type5_Intervals(i).Left, Type5_Intervals(i).Right) <= GlobalVars.degEps Then
				tmpStr = Type5_Intervals(i).Left.ToString() + "°"
			Else
				tmpStr = tmpStr + My.Resources.str60313 + Type5_Intervals(i).Left.ToString() + My.Resources.str60314 + Type5_Intervals(i).Right.ToString() + "°"
			End If

			'if i = 0 then TextBox505.Text = Type5_Intervals(0).Left.ToString()

			If i <> n Then tmpStr = tmpStr + vbCrLf
		Next

		TextBox505_Validating(TextBox505, New System.ComponentModel.CancelEventArgs())
		Label516.Text = tmpStr
	End Sub

	Function Type5Segment(Navaid As WPT_FIXType, ByRef DirNav As Double, TurnDir As Integer, SnapAngle As Double, ByRef Segment As TraceSegment) As Boolean
		Dim dDir As Double
		Dim FlyBy As IPoint
		Dim TurnDir2 As Integer

		Dim MyPC As IPointCollection = Functions.CalcTouchByFixDir(m_CurrPnt, Navaid.pPtPrj, m_TurnR, m_CurrDir, DirNav, TurnDir, TurnDir2, SnapAngle, dDir, FlyBy)

		Dim iToFacility As Integer = Functions.SideDef(MyPC.Point(MyPC.PointCount - 1), DirNav + 90.0, Navaid.pPtPrj)
		'DrawPointWithText MyPC.Point(MyPC.PointCount - 1), "pt1"

		If (iToFacility > 0) Then
			Label520.Text = My.Resources.str60515
		Else
			Label520.Text = My.Resources.str60516
		End If

		TextBox503.Text = Functions.ConvertDistance(dDir, eRoundMode.NEAREST).ToString()

		Dim fCourse As Double = Functions.Dir2Azt(Type5_CurNav.pPtPrj, Type5_CurDir)
		TextBox505.Text = ConvertAngle(NativeMethods.Modulus(fCourse - m_MagVar)).ToString()

		'---------------------------------

		Dim pPtX As IPoint = CenterOfTurn(MyPC.Point(0), MyPC.Point(1))
		Dim pPtXGeo As IPoint = Functions.ToGeo(pPtX)
		'Segment.PtCenter1 = pPtX

		Segment.Center1Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)
		Segment.Turn1Dir = Functions.SideDef(MyPC.Point(0), MyPC.Point(0).M, MyPC.Point(1))
		Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point(0).M - MyPC.Point(1).M) * Segment.Turn1Dir)

		Dim pPC As IPointCollection = New ESRI.ArcGIS.Geometry.Polyline()
		pPC.AddPoint(MyPC.Point(0))
		pPC.AddPoint(MyPC.Point(1))
		Dim pPoly As IPolyline = Functions.CalcTrajectoryFromMultiPoint(pPC)

		Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG
		Segment.Turn1Length = pPoly.Length

		If MyPC.PointCount > 2 Then
			pPtX = CenterOfTurn(MyPC.Point(2), MyPC.Point(3))
			pPtXGeo = Functions.ToGeo(pPtX)

			'Segment.PtCenter2 = pPtX
			Segment.Center2Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)
			Segment.Turn2R = m_TurnR
			Segment.Turn2Dir = Functions.SideDef(MyPC.Point(2), MyPC.Point(2).M, MyPC.Point(3))
			Segment.Turn2Angle = NativeMethods.Modulus((MyPC.Point(2).M - MyPC.Point(3).M) * Segment.Turn2Dir)

			pPC = New ESRI.ArcGIS.Geometry.Polyline()
			pPC.AddPoint(MyPC.Point(2))
			pPC.AddPoint(MyPC.Point(3))
			pPoly = Functions.CalcTrajectoryFromMultiPoint(pPC)

			Segment.H2 = Segment.H1 + (dDir + pPoly.Length) * m_PDG
			Segment.Turn2Length = pPoly.Length

			Segment.PtCenter1 = MyPC.Point(1)
			pPtXGeo = Functions.ToGeo(MyPC.Point(1))
			Segment.Fin1Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)

			Segment.PtCenter2 = MyPC.Point(2)
			pPtXGeo = Functions.ToGeo(MyPC.Point(2))
			Segment.St2Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)

			Segment.BetweenTurns = dDir
			Segment.DirBetween = MyPC.Point(1).M
		Else
			Segment.Turn2R = -1.0
			'Segment.Turn2Length = 0.0
			'Segment.BetweenTurns = 0.0
			'Segment.DirBetween = 0.0
		End If
		'---------------------------------

		Segment.PathPrj = Functions.CalcTrajectoryFromMultiPoint(MyPC)
		pPC = Segment.PathPrj

		Segment.Length = Segment.PathPrj.Length

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = pPC.Point(pPC.PointCount - 1)
		Segment.DirOut = Segment.ptOut.M

		Segment.Turn1R = m_TurnR

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		If Not CheckBox501.Checked Then
			If iToFacility > 0 Then
				Segment.Comment = My.Resources.str60517 + TextBox505.Text + My.Resources.str60518 + ComboBox501.Text
				Segment.RepComment = My.Resources.str60517 + ConvertAngle(NativeMethods.Modulus(fCourse + m_MagVar)).ToString() + My.Resources.str60518 + ComboBox501.Text
			Else
				Segment.Comment = My.Resources.str60517 + TextBox505.Text + My.Resources.str60519 + ComboBox501.Text
				Segment.RepComment = My.Resources.str60517 + ConvertAngle(NativeMethods.Modulus(fCourse + m_MagVar)).ToString() + My.Resources.str60519 + ComboBox501.Text
			End If
		Else
			Segment.Comment = My.Resources.str60517 + ComboBox501.Text + My.Resources.str60520 + ComboBox503.Text
			Segment.RepComment = My.Resources.str60517 + ConvertAngle(NativeMethods.Modulus(fCourse + m_MagVar)).ToString() + My.Resources.str60520 + ComboBox503.Text
		End If

		Segment.SegmentCode = eSegmentType.turnAndIntercept
		Segment.LegType = Aim.Enums.CodeSegmentPath.CI

		'Segment.InterceptionNav.TypeCode = eNavaidType.NONE

		If (Navaid.TypeCode > eNavaidType.NONE) Then
			Segment.InterceptionNav = Navaids_DataBase.WPT_FIXToNavaid(Navaid)
		Else
			Segment.InterceptionNav.TypeCode = eNavaidType.NONE
		End If

		Segment.GuidanceNav.TypeCode = eNavaidType.NONE

		Return True
	End Function

#End Region

#Region "Type 6 Implimentation"

	Function Type6Segment(Nav As NavaidData, ByRef Segment As TraceSegment) As Boolean

		Dim Side As Integer = 2 * ComboBox601.SelectedIndex - 1
		Dim TurnDir As Integer = 2 * ComboBox602.SelectedIndex - 1

		Dim ptNav As IPoint = Nav.pPtPrj

		Dim phi As Double = Functions.ReturnAngleInDegrees(m_CurrPnt, ptNav)
		Dim L As Double = Functions.ReturnDistanceInMeters(m_CurrPnt, ptNav)
		Dim R As Double = Functions.DeConvertDistance(Double.Parse(TextBox605.Text))

		Segment.Turn2R = R
		Segment.Turn2Dir = 2 * ComboBox602.SelectedIndex - 1
		Segment.PtCenter2 = Nav.pPtPrj

		Dim Bheta As Double = m_CurrDir - phi
		Dim alpha As Double = Functions.RadToDeg(Math.Acos((Side * m_TurnR - TurnDir * L * System.Math.Sin(Functions.DegToRad(Bheta))) / (R + Side * m_TurnR)))
		Dim Gamma As Double = m_CurrDir + Side * TurnDir * (90.0 - alpha) + 90.0 * (1 + Side)
		Dim Lp As Double = R + Side * m_TurnR

		Dim ptCenter As IPoint = Functions.PointAlongPlane(ptNav, Gamma, Lp)
		Dim ptStart As IPoint = Functions.PointAlongPlane(ptCenter, m_CurrDir + 90.0 * Side * TurnDir, m_TurnR)
		ptStart.M = m_CurrDir

		Dim ptEnd As IPoint = Functions.PointAlongPlane(ptNav, Gamma, Lp - Side * m_TurnR)
		ptEnd.M = Gamma + 90.0 * TurnDir

		'Xe = (yt - y0) * Sin(t) * Cos(t) + xt * Cos(t) ^ 2 + X0 * Sin(t) ^ 2
		'Ye = y0 + (Xe - X0) * tg(t)

		Dim MyPC As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint()
		MyPC.AddPoint(m_CurrPnt)
		MyPC.AddPoint(ptStart)
		MyPC.AddPoint(ptEnd)

		Dim MyPoly As IPolyline = Functions.CalcTrajectoryFromMultiPoint(MyPC)
		'---------------------------------
		Dim pPtX As IPoint = ptCenter       'CenterOfTurn(MyPC.Point(0), MyPC.Point(1))
		Dim pPtXGeo As IPoint = Functions.ToGeo(pPtX)

		Segment.Center1Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)
		Segment.Turn1Dir = Functions.SideDef(MyPC.Point(0), MyPC.Point(0).M, MyPC.Point(1))
		Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point(0).M - MyPC.Point(1).M) * Segment.Turn1Dir)

		Dim pPC As IPointCollection = New ESRI.ArcGIS.Geometry.Polyline()
		pPC.AddPoint(MyPC.Point(0))
		pPC.AddPoint(MyPC.Point(1))

		Dim pPoly As IPolyline = Functions.CalcTrajectoryFromMultiPoint(pPC)

		Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG
		Segment.Turn1Length = pPoly.Length
		'---------------------------------
		MyPC = MyPoly

		Segment.PathPrj = MyPoly
		Segment.Length = MyPoly.Length

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = ptEnd
		Segment.DirOut = MyPC.Point(MyPC.PointCount - 1).M

		Segment.Turn1R = m_TurnR
		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)

		Segment.Comment = "Arc intercept"       'Resources.str608 + ComboBox601.Text + Resources.str522 + ComboBox603.Text
		Segment.RepComment = Segment.Comment

		Select Case PrevSegment.LegType
			Case Aim.Enums.CodeSegmentPath.TF
			Case Aim.Enums.CodeSegmentPath.CA
			Case Aim.Enums.CodeSegmentPath.CD
			Case Aim.Enums.CodeSegmentPath.CI
			Case Aim.Enums.CodeSegmentPath.CR
			Case Aim.Enums.CodeSegmentPath.CF
			Case Aim.Enums.CodeSegmentPath.DF
				Segment.LegType = Aim.Enums.CodeSegmentPath.CD
			Case Else
				Segment.LegType = Aim.Enums.CodeSegmentPath.VD
		End Select

		If (PrevSegment.SegmentCode = eSegmentType.courseIntercept) Or (PrevSegment.SegmentCode = eSegmentType.turnAndIntercept) Or (PrevSegment.SegmentCode = eSegmentType.arcPath) Then
			Segment.GuidanceNav = PrevSegment.InterceptionNav
		Else
			Segment.GuidanceNav = PrevSegment.GuidanceNav
		End If

		Segment.InterceptionNav = Nav

		Segment.SegmentCode = eSegmentType.arcIntercept
		Return True
	End Function

#End Region

#Region "Type 7 Implimentation"

	Function Type7Segment(pNav As NavaidData, ByRef Segment As TraceSegment) As Boolean

		Dim ExitSide As Integer = 1 - 2 * ComboBox702.SelectedIndex
		Dim ExitDir As Double = Functions.Azt2Dir(pNav.pPtGeo, Double.Parse(TextBox703.Text))

		If ComboBox702.SelectedIndex = 2 Then
			ExitSide = 1
		End If

		Dim DirToArcCenter As Double = Functions.ReturnAngleInDegrees(pNav.pPtPrj, PrevSegment.PtCenter2)
		Dim DistToArcCenter As Double = Functions.ReturnDistanceInMeters(pNav.pPtPrj, PrevSegment.PtCenter2)

		Dim betta As Double = Functions.DegToRad(ExitDir - DirToArcCenter)
		Dim fTmp As Double = (ExitSide * m_TurnR - PrevSegment.Turn2Dir * DistToArcCenter * System.Math.Sin(betta)) / (PrevSegment.Turn2R + ExitSide * m_TurnR)

		If Math.Abs(fTmp) > 1.0 Then
			If Math.Abs(fTmp) - 1 < EpsilonDistance Then
				fTmp = Math.Sign(fTmp)
			Else
			End If
		End If

		Dim TurnAngle As Double = Functions.RadToDeg(Math.Acos(fTmp))
		Dim startRad As Double = ExitDir - ExitSide * PrevSegment.Turn2Dir * (90.0 - TurnAngle) + 90.0 * (1 - ExitSide)

		Dim ptStart As IPoint = Functions.PointAlongPlane(PrevSegment.PtCenter2, startRad, PrevSegment.Turn2R)
		ptStart.M = startRad + PrevSegment.Turn2Dir * 90.0

		Dim ptEnd As IPoint
		Dim ptCenter As IPoint

		If ComboBox702.SelectedIndex = 2 Then
			ptEnd = Functions.PointAlongPlane(PrevSegment.PtCenter2, ExitDir, PrevSegment.Turn2R)
			ExitDir = Modulus(ExitDir + PrevSegment.Turn2Dir * ExitSide * 90.0)
			ptEnd.M = ExitDir '+ PrevSegment.Turn2Dir * ExitSide * 90.0

			'DrawPointWithText(ptEnd, "ptEnd")
			'Application.DoEvents()
		Else
			ptCenter = Functions.PointAlongPlane(ptStart, startRad, ExitSide * m_TurnR)
			ptEnd = Functions.PointAlongPlane(ptCenter, ExitDir + PrevSegment.Turn2Dir * ExitSide * 90.0, m_TurnR)
			ptEnd.M = ExitDir
		End If

		'ptEnd.M -= 180

		'DrawPointWithText(ptEnd, "ptEnd")
		'While True
		'	Application.DoEvents()
		'End While

		Dim MyPC As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint()
		MyPC.AddPoint(m_CurrPnt)
		MyPC.AddPoint(ptStart)
		MyPC.AddPoint(ptEnd)
		Dim MyPoly As IPolyline = Functions.CalcTrajectoryFromMultiPoint(MyPC)

		'DrawPolyLine(MyPoly, -1, 2)
		'Application.DoEvents()
		'DrawPointWithText PrevSegment.PtCenter2, "PtCenter2"
		'DrawPointWithText Navaid.pPtPrj, "Nav", RGB(0, 0, 255)
		'ptCenter = PointAlongPlane(ptStart, startRad - 90, m_TurnR)
		'DrawPointWithText ptCenter, "ptCenter2"

		'ExitDir + turn*(90°- TurnAngle) +180 или более универсально,
		'ExitDir + ExitSide*PrevSegment.Turn2Dir*(90- TurnAngle) +90*(1 + ExitSide)
		'? - uchushun direksion istiqametidir. ? - donme bucagidir.
		'? = arcos{(side*r -turn* L*sin?)/(R + side*r)}
		'---------------------------------
		If ComboBox702.SelectedIndex < 2 Then
			Dim pPtX As IPoint = ptCenter       'CenterOfTurn(MyPC.Point(0), MyPC.Point(1))
			Dim pPtXGeo As IPoint = Functions.ToGeo(pPtX)

			Segment.Center1Coords = Functions.DegreeToString(pPtXGeo.Y, Degree2StringMode.DMSLat) + ", " + Functions.DegreeToString(pPtXGeo.X, Degree2StringMode.DMSLon)
			Segment.Turn1Dir = Functions.SideDef(MyPC.Point(0), MyPC.Point(0).M, MyPC.Point(1))
			Segment.Turn1Angle = NativeMethods.Modulus((MyPC.Point(0).M - MyPC.Point(1).M) * Segment.Turn1Dir)
		End If

		'DrawPointWithText(ptCenter, "ptCenter")
		'Application.DoEvents()

		Dim pPC As IPointCollection = New ESRI.ArcGIS.Geometry.Polyline()
		pPC.AddPoint(MyPC.Point(0))
		pPC.AddPoint(MyPC.Point(1))

		Dim pPoly As IPolyline = Functions.CalcTrajectoryFromMultiPoint(pPC)

		Segment.H1 = CurrSegment.HStart + pPoly.Length * m_PDG
		Segment.Turn1Length = pPoly.Length
		'---------------------------------

		Segment.PathPrj = MyPoly
		Segment.Length = MyPoly.Length

		Segment.ptIn = m_CurrPnt
		Segment.DirIn = m_CurrDir

		Segment.ptOut = ptEnd
		Segment.DirOut = ExitDir

		Segment.Turn2R = m_TurnR

		m_HFinish = Math.Min(CurrSegment.HStart + Segment.Length * m_PDG, cMaxH)
		Segment.Comment = "Out from arc"        'LoadResString(608) + ComboBox601.Text + LoadResString(522) + ComboBox603.Text
		Segment.RepComment = Segment.Comment
		Segment.SegmentCode = eSegmentType.arcPath
		Segment.GuidanceNav = PrevSegment.InterceptionNav
		Segment.InterceptionNav = pNav
		Segment.LegType = Aim.Enums.CodeSegmentPath.AF

		Return True
	End Function

#End Region

End Class