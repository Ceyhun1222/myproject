Option Strict Off
Option Explicit On

Imports System.Collections.Generic
Imports System.ComponentModel
Imports ESRI
Imports ESRI.ArcGIS
Imports ArcGeometry = ESRI.ArcGIS.Geometry
'Imports ESRI.ArcGIS.Geometry

Imports Aran.Aim
Imports Aran.Aim.Enums
Imports Aran.Aim.Features
Imports Aran.Aim.DataTypes
Imports AranGeometry = Aran.Geometries

Imports Aran.Queries
Imports Aran.Queries.Panda_2
Imports System.Runtime.InteropServices


<ComVisibleAttribute(False)> Friend Class CEnrouteForm
	Inherits System.Windows.Forms.Form

	Private Const RangeReduse As Double = 50000.0

	Private pGraphics As Carto.IGraphicsContainer

	Private FixPointElement1 As Carto.IElement
	Private FixPointElement2 As Carto.IElement
	Private FullSegElement1 As Carto.IElement
	Private PrimSegElement1 As Carto.IElement
	Private FullSegElement2 As Carto.IElement
	Private PrimSegElement2 As Carto.IElement

	Private LineElement1 As Carto.IElement
	Private LineElement2 As Carto.IElement
	Private FixElement1 As Carto.IElement
	Private FixElement2 As Carto.IElement

	Private pLineSym As Display.ILineSymbol


	Private FullSegPoly1 As ArcGeometry.IPointCollection
	Private PrimSegPoly1 As ArcGeometry.IPointCollection

	Private FullSegPoly2 As ArcGeometry.IPointCollection
	Private PrimSegPoly2 As ArcGeometry.IPointCollection

	Private PrimFIXPoly As ArcGeometry.IPointCollection
	Private SecFIXPoly As ArcGeometry.IPointCollection

	Private pGuidPolyLine1 As ArcGeometry.IPolyline
	Private pGuidPolyLine2 As ArcGeometry.IPolyline

	Private pCurrSegLine As ArcGeometry.IPolyline

	Private pTrackLine1 As ArcGeometry.IPolyline
	Private pTrackLine2 As ArcGeometry.IPolyline

	Private hRouteInter As Double
	Private fMinCOPDist As Double
	Private fNeardLmax As Double
	Private hRouteGuid As Double

	Private fCurrDir As Double
	Private fNextDir As Double
	Private fNearTol As Double
	Private fFarTol As Double

	Private hRoute As Double
	Private fTurnR As Double
	Private fTAS As Double

	Private NewFixName As String

	Private RouteName As String

	Private bText1 As Boolean
	Private bText2 As Boolean

	Private iError As Integer
	Private NewFixNumber As Integer
	Private SegmentNum As Integer
	Private Segments() As SegmentInfo

	Private Dirs(2) As Double
	Private pEnRouteList As List(Of Route)

	Private NewFIX As TypeDefinitions.NavaidType
	Private EndWPT() As TypeDefinitions.NavaidType
	Private StartWPT() As TypeDefinitions.NavaidType
	Private DirWPT1() As TypeDefinitions.NavaidType
	Private DirWPT2() As TypeDefinitions.NavaidType

	'Private NavaidList() As TypeDefinitions.NavaidType
	Private GuidNavDat1() As TypeDefinitions.NavaidType
	Private GuidNavDat2() As TypeDefinitions.NavaidType
	Private InterNavDat1() As TypeDefinitions.NavaidType
	Private InterNavDat2() As TypeDefinitions.NavaidType

	Private EnrouteObstacleList() As TypeDefinitions.ObstacleType

	Private PageLabel(2) As Label
	Private InfoFrm As CInfoFrm
	Private ReportForm As CReportForm

	Private HelpContextID As Long
	Private bFormInitialised As Boolean = False

	Private Sub FocusStepCaption(ByRef StIndex As Integer)
		PageLabel(0).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
		PageLabel(0).Font = New Font(PageLabel(0).Font, FontStyle.Regular)

		PageLabel(1).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
		PageLabel(1).Font = New Font(PageLabel(1).Font, FontStyle.Regular)

		PageLabel(2).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
		PageLabel(2).Font = New Font(PageLabel(2).Font, FontStyle.Regular)

		PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
		PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

		Me.Text = My.Resources.str0006 + "  [" + PageLabel(StIndex).Text + "]"
	End Sub

#Region "Form"
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		Dim I As Integer
		Dim N As Integer

		ReDim StartWPT(-1)
		ReDim DirWPT1(-1)
		ReDim DirWPT2(-1)
		ReDim EndWPT(-1)
		ReDim InterNavDat1(-1)
		ReDim InterNavDat2(-1)
		ReDim GuidNavDat1(-1)
		ReDim GuidNavDat2(-1)

		InfoFrm = New CInfoFrm
		ReportForm = New CReportForm

		pTrackLine1 = New ArcGeometry.Polyline
		pTrackLine2 = New ArcGeometry.Polyline
		pCurrSegLine = New ArcGeometry.Polyline

		PageLabel(0) = PageLabel0
		PageLabel(1) = PageLabel1
		PageLabel(2) = PageLabel2

		'Application.HelpFile = "Panda.chm"
		Me.HelpContextID = 21100
		NewFixNumber = 1

		'FillEnrouteCmBox()
		'ComboBox100.Items.Add(My.Resources.str2500)	'"Create new"
		'ComboBox100.SelectedIndex = 0

		Dim pRGB As Display.IRgbColor

		'pLineSym = UseLineSymbolFromStyleGallery("Arrow at End")
		pLineSym = Nothing

		If pLineSym Is Nothing Then pLineSym = New Display.SimpleLineSymbol

		pRGB = New Display.RgbColor
		pRGB.RGB = 255
		pLineSym.Color = pRGB
		pLineSym.Width = 2

		RouteName = GetRouteName()

		pGraphics = GetActiveView().GraphicsContainer
		FullSegElement1 = Nothing
		PrimSegElement1 = Nothing

		'fBuffer = 100000.0
		hRouteGuid = hRoute
		hRouteInter = hRoute
		'hRouteMin = hRoute

		bText1 = False
		bText2 = False

		ReDim Segments(5)
		SegmentNum = 0

		ReDim SegmentData(-1)
		ReportForm.SetReportBtn(ReportBtn)

		'=====================================================================================
		N = WPTList.Length

		ReDim StartWPT(N - 1)
		Array.Copy(WPTList, StartWPT, N)

		ReDim DirWPT1(N - 1)
		Array.Copy(WPTList, DirWPT1, N)

		N = NavaidList.Length
		ReDim GuidNavDat1(N - 1)
		Array.Copy(NavaidList, GuidNavDat1, N)

		'ComboBox101.List = Array("Low Altitude", "High Altitude")
		ComboBox101.SelectedIndex = 1

		For I = 0 To N - 1
			ComboBox102.Items.Add(GuidNavDat1(I).CallSign)
		Next I
		If N > 0 Then ComboBox102.SelectedIndex = 0

		'ComboBox206.List = EnrouteMOCValues
		ComboBox206.Items.Clear()
		For I = 0 To UBound(EnrouteMOCValues)
			ComboBox206.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(I), 2)))
		Next I
		ComboBox206.SelectedIndex = 0

		'============2007
		'2007
		Label001.Text = My.Resources.str1054
		Label002.Text = My.Resources.str1053
		Label003.Text = HeightConverter(HeightUnit).Unit

		PageLabel(0).Text = My.Resources.str1000
		PageLabel(1).Text = My.Resources.str1001
		PageLabel(2).Text = My.Resources.str1002

		RemoveBtn.Text = My.Resources.str0112
		AddBtn.Text = My.Resources.str0113

		'=========================================================================================
		Label101.Text = My.Resources.str1052
		Label102.Text = My.Resources.str1051
		Label103.Text = My.Resources.str1046
		Label107.Text = My.Resources.str1035
		Label108.Text = My.Resources.str1036
		Label109.Text = My.Resources.str1037
		Label110.Text = My.Resources.str1038
		Label113.Text = My.Resources.str1049 + " (" + DistanceConverter(DistanceUnit).Unit + ")"
		Label114.Text = My.Resources.str1034

		TextBox101.Text = RouteName
		TextBox103.Text = GetFixName(NewFixNumber)

		'CheckBox101.Text = My.Resources.str1026 + CStr(SecAreaCutAngl.Value) + "°"
		CheckBox101.Text = My.Resources.str1055

		OptionButton101.Text = My.Resources.str1015
		OptionButton102.Text = My.Resources.str1016

		OptionButton103.Text = My.Resources.str1041
		OptionButton104.Text = My.Resources.str1042

		OptionButton105.Text = My.Resources.str1033
		OptionButton106.Text = My.Resources.str1032

		Frame100.Text = My.Resources.str1056
		Frame101.Text = My.Resources.str1043
		Frame102.Text = My.Resources.str1040
		Frame103.Text = My.Resources.str1031

		'=========================================================================================

		Label201.Text = My.Resources.str1019
		Label205.Text = My.Resources.str1011
		Label207.Text = My.Resources.str1007
		Label208.Text = DistanceConverter(DistanceUnit).Unit
		Label209.Text = Label113.Text           'My.Resources.str1049 + " (" + DistanceConverter(DistanceUnit).Unit + ")"
		Label211.Text = My.Resources.str1012
		Label215.Text = My.Resources.str1009
		Label216.Text = HeightConverter(HeightUnit).Unit
		Label217.Text = My.Resources.str0504

		TextBox201.Text = CStr(ConvertDistance(10000, 2))

		'CheckBox201.Text = CheckBox101.Text
		CheckBox201.Text = My.Resources.str1055

		OptionButton203.Text = My.Resources.str1015
		OptionButton204.Text = My.Resources.str1016

		OptionButton205.Text = My.Resources.str1005
		OptionButton206.Text = My.Resources.str1004

		OptionButton202.Text = My.Resources.str1029
		OptionButton201.Text = My.Resources.str1030

		ComboBox207.Items.Clear()
		ComboBox207.Items.Add(My.Resources.str0505)
		ComboBox207.Items.Add(My.Resources.str0506)

		Frame201.Text = My.Resources.str1014
		Frame202.Text = My.Resources.str1003
		Frame203.Text = My.Resources.str1056
		'=========================================================================================

		'Frame301.Text = My.Resources.str1020
		'OptionButton301.Text = My.Resources.str1021
		'OptionButton302.Text = My.Resources.str1022

		'Label302.Text = My.Resources.str1024
		'Label303.Text = My.Resources.str1025

		'Label301.Text = My.Resources.str1027
		'Label307.Text = My.Resources.str1028
		'=========================================================================================

		MultiPage001.SelectedIndex = 0

		FocusStepCaption(0)
		MultiPage001.Top = -21
		Frame001.Top = Frame001.Top - 21

		ShowPanelBtn.Image = My.Resources.gifSHOW_INFO
		ShowPanelBtn.Checked = False

		Me.Height = Me.Height - 21
		Me.Width = 650

		bFormInitialised = True
	End Sub

	Private Sub EnrouteForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim I As Integer

		On Error Resume Next
		For I = 0 To SegmentNum
			If Not Segments(I).pFullSegElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pFullSegElement)
			If Not Segments(I).pPrimSegElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pPrimSegElement)
			If Not Segments(I).pStartFixElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pStartFixElement)
			If Not Segments(I).pEndFixElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pEndFixElement)
			If Not Segments(I).pSegLineElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pSegLineElement)
			If Not Segments(I).pStartPtElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pStartPtElement)
			If Not Segments(I).pEndPtElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pEndPtElement)
		Next I

		If Not FullSegElement1 Is Nothing Then pGraphics.DeleteElement(FullSegElement1)
		If Not PrimSegElement1 Is Nothing Then pGraphics.DeleteElement(PrimSegElement1)
		If Not FullSegElement2 Is Nothing Then pGraphics.DeleteElement(FullSegElement2)
		If Not PrimSegElement2 Is Nothing Then pGraphics.DeleteElement(PrimSegElement2)

		If Not FixElement1 Is Nothing Then pGraphics.DeleteElement(FixElement1)
		If Not FixElement2 Is Nothing Then pGraphics.DeleteElement(FixElement2)
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		If Not LineElement2 Is Nothing Then pGraphics.DeleteElement(LineElement2)
		If Not FixPointElement1 Is Nothing Then pGraphics.DeleteElement(FixPointElement1)
		If Not FixPointElement2 Is Nothing Then pGraphics.DeleteElement(FixPointElement2)
		On Error GoTo 0
		GetActiveView().Refresh()

		'If Not ReportForm.IsDisposed() Then ReportForm.Close()
		'If Not InfoFrm.IsDisposed Then InfoFrm.Close()
		If (Not (ReportForm Is Nothing)) And (Not ReportForm.IsDisposed) Then ReportForm.Close()
		If (Not (InfoFrm Is Nothing)) And (Not InfoFrm.IsDisposed) Then InfoFrm.Close()
	End Sub

	Private Sub CEnrouteForm_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
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

	Public Shared Function CheckRouteName(ByVal Name As String) As Boolean
		'Dim I As Integer
		'Dim N As Integer
		Return False
	End Function

	Private Shared Function GetRouteName() As String
		Dim strName As String
		Dim I As Integer

		I = 1
		Do
			strName = "AROUTE" + CStr(I)
			I += 1
		Loop While CheckRouteName(strName)

		Return strName
	End Function

	Public Shared Function GetFixName(ByRef I As Integer) As String
		Dim strName As String

		'Do
		If (I < 10) Then
			strName = "WPT0" + CStr(I)
		Else
			strName = "WPT" + CStr(I)
		End If
		I += 1
		'Loop While CheckNameInTable(pFIXTable, strName)
		'I -= 1

		Return strName
	End Function

	'Private Function CalcTurnAngleIntervalToNavaid(ByVal ptD0 As ArcGeometry.IPoint, ByVal fDir As Double, ByVal ptNav As ArcGeometry.IPoint, ByVal MAXRange As Double, Optional ByVal ResInDegree As Boolean = True) As LowHigh	  'На средство
	'	Dim X0 As Double
	'	Dim Y0 As Double

	'	Dim X1 As Double
	'	Dim Y1 As Double

	'	Dim fD As Double
	'	Dim Det As Double
	'	Dim NavX As Double
	'	Dim NavY As Double

	'	Dim fMaxDist As Double
	'	Dim fMinDist As Double
	'	Dim NearDist As Double

	'	Dim Result As LowHigh
	'	Dim pPt0 As ArcGeometry.IPoint
	'	Dim pPt1 As ArcGeometry.IPoint

	'	PrjToLocal(ptD0, fCurrDir, ptNav, NavX, NavY)
	'	NavY = Math.Abs(NavY)

	'	fMaxDist = NavX + NavY / System.Math.Tan(DegToRad(180.0 - MaxTurnAngle.Value))
	'	If fMaxDist > MAXRange - fNeardLmax Then fMaxDist = MAXRange - fNeardLmax
	'	NearDist = fNeardLmax

	'	Result.High = -9999.0	'разворот на средство не допускается
	'	Result.Low = -9999.0	'разворот на средство не допускается
	'	Result.Tag = 0

	'	Det = NearDist * NearDist - 2 * fTurnR * NavY + NavY * NavY

	'	If Det < 0.0 Then Return Result 'Det < 0 разворот на средство не допускается

	'	fD = 2.0 * fTurnR - NavY
	'	If fD = 0.0 Then
	'		If NearDist = 0.0 Then Return Result 'NearDist = 0 разворот на средство не допускается

	'		X0 = 2 * NearDist
	'		Y0 = NavY
	'		X1 = 0.0  'Y1 = 1.0
	'	ElseIf fD > 0.0 Then
	'		X0 = fD
	'		Y0 = NearDist - System.Math.Sqrt(Det)
	'		X1 = fD
	'		Y1 = NearDist + System.Math.Sqrt(Det)
	'	ElseIf fD < 0.0 Then
	'		X0 = fD
	'		Y0 = NearDist - System.Math.Sqrt(Det)
	'		X1 = 0.0			'fD        Y1 = NearDist + Sqr(Det)
	'	End If

	'	fMinDist = fTurnR * Y0 / X0 + fNeardLmax	 'Нижняя граница угла разворота

	'	If fMinDist > fMaxDist Then Return Result 'разворот на средство не допускается

	'	Result.Low = fMinDist

	'	If X1 = 0.0 Then
	'		Result.High = fMaxDist						'Верхняя граница угла разворота
	'	Else
	'		Result.High = fTurnR * Y1 / X1 + fNeardLmax	'Верхняя граница угла разворота
	'	End If

	'	If Result.High > fMaxDist Then Result.High = fMaxDist
	'	If Result.Low < 0.0 Then Result.Low = 0.0

	'	If ResInDegree Then
	'		pPt0 = PointAlongPlane(ptD0, fDir, Result.Low)
	'		pPt1 = PointAlongPlane(ptD0, fDir, Result.High)

	'		X0 = ReturnAngleInDegrees(pPt0, ptNav)
	'		X1 = ReturnAngleInDegrees(pPt1, ptNav)

	'		If Modulus(X0 - X1) > 180.0 Then
	'			Result.Low = X0
	'			Result.High = X1
	'		Else
	'			Result.Low = X1
	'			Result.High = X0
	'		End If

	'		'If Result.High > fMaxDist Then Result.High = fMaxDist
	'		'If Result.Low < 0.0 Then Result.Low = 0.0

	'		'If X1 = 0.0 Then
	'		'	Result.High = MaxTurnAngle.Value
	'		'Else
	'		'	Result.High = RadToDeg(2.0 * Atn(Y1 / X1)) 'Верхняя граница  угла разворота
	'		'End If

	'		'Result.Low = RadToDeg(2.0 * Atn(Y0 / X0))  'Нижняя граница  угла разворота

	'		'If Result.Low > MaxTurnAngle.Value Then
	'		'	Result.High = -9999.0
	'		'	Result.Low = -9999.0
	'		'ElseIf Result.High > MaxTurnAngle.Value Then
	'		'	Result.High = MaxTurnAngle.Value
	'		'End If
	'	End If

	'	Result.Tag = 1
	'	Return Result
	'End Function

	'Private Function CalcTurnAngleIntervalFromNavaid(ByVal ptD0 As ArcGeometry.IPoint, ByVal fDir As Double, ByVal ptNav As ArcGeometry.IPoint, ByVal MAXRange As Double, Optional ByVal ResInDegree As Boolean = True) As LowHigh	'От средства
	'	Dim X0 As Double
	'	Dim X1 As Double
	'	Dim Det As Double
	'	Dim tan1 As Double
	'	Dim NavX As Double
	'	Dim NavY As Double
	'	Dim fMinDist As Double
	'	Dim fMaxDist As Double
	'	Dim NearDist As Double
	'	Dim Result As LowHigh

	'	Dim pPt0 As ArcGeometry.IPoint
	'	Dim pPt1 As ArcGeometry.IPoint

	'	PrjToLocal(ptD0, fCurrDir, ptNav, NavX, NavY)
	'	NavY = Math.Abs(NavY)

	'	Result.High = -9999.0	'разворот на средство не допускается
	'	Result.Low = -9999.0	'разворот на средство не допускается
	'	Result.Tag = 0

	'	fMaxDist = MAXRange - fNeardLmax				'Максимально допустимое расстояние разворота
	'	fMinDist = NavX + NavY / System.Math.Tan(DegToRad(MaxTurnAngle.Value))
	'	If fMinDist > fMaxDist Then Return Result

	'	NearDist = fNeardLmax 'NavX -

	'	Det = NearDist * NearDist + 2 * fTurnR * NavY + NavY * NavY
	'	tan1 = (NearDist + System.Math.Sqrt(Det)) / (2 * fTurnR + NavY)

	'	Det = fTurnR * tan1 + fNeardLmax				'Минимально допустимое расстояние разворота
	'	If Det > fMinDist Then fMinDist = Det

	'	If fMinDist > fMaxDist Then Return Result

	'	Result.Low = fMinDist
	'	Result.High = fMaxDist

	'	If ResInDegree Then
	'		pPt0 = PointAlongPlane(ptD0, fDir, Result.Low)
	'		pPt1 = PointAlongPlane(ptD0, fDir, Result.High)

	'		X0 = ReturnAngleInDegrees(ptNav, pPt0)
	'		X1 = ReturnAngleInDegrees(ptNav, pPt1)

	'		If Modulus(X0 - X1) > 180.0 Then
	'			Result.Low = X0
	'			Result.High = X1
	'		Else
	'			Result.Low = X1
	'			Result.High = X0
	'		End If

	'		'Result.High = 2.0 * RadToDeg(System.Math.Atan(tan1))	'Максимально допустимый угол разворота
	'		'Result.Low = 0.0										'Решение не имеет значение (минусовое значение)
	'		'If Result.High > MaxTurnAngle.Value Then Result.High = MaxTurnAngle.Value
	'	End If

	'	Result.Tag = 1
	'	Return Result
	'End Function


	'Private Sub CreateSegmentArea_()
	'Dim EnRouteInterToler As Double
	'Dim fDist As Double
	'Dim fDist1 As Double
	'Dim fDist2 As Double
	'Dim fDir As Double
	'
	'Dim ptFIX1 As IPoint
	'Dim ptFIX2 As IPoint
	'Dim GuidNav1 As FIXableNavaidType
	'Dim GuidNav2 As FIXableNavaidType
	'Dim InterNav1 As FIXableNavaidType
	'Dim InterNav2 As FIXableNavaidType
	'
	'Dim K As Long
	'Dim FullSeg As Polygon
	'Dim PrimSeg As Polygon
	'Dim pTriangle As IPointCollection
	'Dim pTopo As ITopologicalOperator2
	'Dim pFIX1Poly As Polygon
	'Dim pFIX2Poly As Polygon
	'
	'Dim NearToler1 As Double
	'Dim FarToler1 As Double
	'Dim NearToler2 As Double
	'Dim FarToler2 As Double
	'
	'If (ComboBox102.ListIndex < 0) Or (ComboBox8.ListIndex < 0) Then Exit Sub
	'If (ComboBox105.ListIndex < 0) Or (ComboBox6.ListIndex < 0) Then Exit Sub
	'
	'If bText1 Then
	'    fDist1 = CDbl(TextBox3.Text)
	'    fDist2 = CDbl(TextBox4.Text)
	'Else
	'    fDist1 = -1
	'    fDist2 = -1
	'End If
	'
	'On Error Resume Next
	'    If Not FullSegElement1 Is Nothing Then pGraphics.DeleteElement FullSegElement1
	'    If Not PrimSegElement1 Is Nothing Then pGraphics.DeleteElement PrimSegElement1
	'    If Not FixElement1 Is Nothing Then pGraphics.DeleteElement FixElement1
	'    If Not FixElement2 Is Nothing Then pGraphics.DeleteElement FixElement2
	'On Error GoTo 0
	'
	'If (ComboBox105.ListIndex = 0) And (ComboBox6.ListIndex = 0) Then
	'    Create2NavArea GuidNavDat1(ComboBox102.ListIndex), fDist1, GuidNavDat2(ComboBox8.ListIndex), fDist2, FullSegPoly, PrimSegPoly
	'
	''    Set FullSegElement1 = DrawPolygon(FullSegPoly, 255)
	''    Set PrimSegElement1 = DrawPolygon(PrimSegPoly, RGB(0, 0, 255))
	'Else
	'    If (ComboBox5.ListIndex < 0) Or (ComboBox7.ListIndex < 0) Then Exit Sub
	'
	'    Set pTriangle = New Polygon
	'
	'    GuidNav1 = GuidNavDat1(ComboBox102.ListIndex)
	'    GuidNav2 = GuidNavDat2(ComboBox8.ListIndex)
	'
	'    If ComboBox6.ListIndex = 1 Then
	'        Set ptFIX2 = EndWPT(ComboBox7.ListIndex).ptPrj
	'        InterNav2 = NavDat2(ComboBox9.ListIndex)
	'        Set pFIX2Poly = CreateFixArea(GuidNav2, InterNav2, ptFIX2)
	'    End If
	'
	'    If ComboBox105.ListIndex = 1 Then
	'        Set ptFIX1 = DirWPT1(ComboBox103.ListIndex).ptPrj   '????????????
	'        InterNav1 = InterNavDat1(ComboBox5.ListIndex)
	'        Set pFIX1Poly = CreateFixArea(GuidNav1, InterNav1, ptFIX1)
	'    Else
	'        fDist = ReturnDistanceInMeters(GuidNav1.ptPrj, ptFIX2)
	'        fDir = ReturnAngleInDegrees(GuidNav1.ptPrj, ptFIX2)
	'        CalcTolerance ptFIX2, pFIX2Poly, fDir + 180.0, NearToler2, FarToler2
	'        CreateNavArea GuidNav1, fDir, fDist + NearToler2, FullSeg, PrimSeg
	'
	''        Set FullSegElement1 = DrawPolygon(FullSeg, 255)
	''        Set PrimSegElement1 = DrawPolygon(PrimSeg, RGB(0, 0, 255))
	''        Set FixElement2 = DrawPolygon(pFIX2Poly, RGB(255, 255, 0))
	'    End If
	'
	'    If ComboBox6.ListIndex = 1 Then
	'        If ComboBox105.ListIndex = 1 Then
	'            fDir = ReturnAngleInDegrees(ptFIX1, ptFIX2)
	'            CalcTolerance ptFIX1, pFIX1Poly, fDir, NearToler1, FarToler1
	'            CalcTolerance ptFIX2, pFIX2Poly, fDir + 180.0, NearToler2, FarToler2
	''            Set FixElement1 = DrawPolygon(pFIX1Poly, RGB(255, 255, 0))
	''            Set FixElement2 = DrawPolygon(pFIX2Poly, RGB(255, 255, 0))
	'        End If
	'    Else
	'        fDist = ReturnDistanceInMeters(GuidNav2.ptPrj, ptFIX1)
	'        fDir = ReturnAngleInDegrees(GuidNav2.ptPrj, ptFIX1)
	'
	'        CalcTolerance ptFIX1, pFIX1Poly, fDir, NearToler1, FarToler1
	'        CreateNavArea GuidNav2, fDir, fDist + FarToler1, FullSeg, PrimSeg
	'
	''        Set FullSegElement1 = DrawPolygon(FullSeg, 255)
	''        Set PrimSegElement1 = DrawPolygon(PrimSeg, RGB(0, 0, 255))
	''        Set FixElement1 = DrawPolygon(pFIX1Poly, RGB(255, 255, 0))
	'    End If
	'End If
	'
	'End Sub
	'
	Private Function CreateFixArea(ByRef GuidNav As TypeDefinitions.NavaidType, ByRef InterNav As TypeDefinitions.NavaidType, ByRef pFix As TypeDefinitions.NavaidType) As ArcGeometry.Polygon
		Dim I As Integer

		Dim EnRouteTrackingToler As Double
		Dim EnRouteInterToler As Double
		Dim fDist1 As Double
		Dim fDist As Double
		Dim fDir As Double
		Dim D0 As Double
		Dim D As Double

		Dim pReleation As ArcGeometry.IRelationalOperator
		Dim lCollect As ArcGeometry.IGeometryCollection
		Dim pGeoCol As ArcGeometry.IGeometryCollection
		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pTriangle1 As ArcGeometry.IPointCollection
		Dim pTriangle2 As ArcGeometry.IPointCollection
		Dim pSect0 As ArcGeometry.IPointCollection
		Dim pSect1 As ArcGeometry.IPointCollection
		Dim pPoly As ArcGeometry.IPointCollection

		If pFix.TypeCode >= eNavaidType.VOR Then
			If (pFix.TypeCode = eNavaidType.VOR) Or (pFix.TypeCode = eNavaidType.TACAN) Then
				fDist1 = (hRoute - pFix.pPtPrj.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
			Else
				fDist1 = (hRoute - pFix.pPtPrj.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
			End If

			'If (pFix.TypeCode = eNavaidType.CodeVOR) Or (pFix.TypeCode = eNavaidType.CodeTACAN) Then
			'	fDist1 = VOR.OnNAVRadius / OverHeadToler.Value * (hRoute - pFix.pPtPrj.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
			'ElseIf pFix.TypeCode = eNavaidType.CodeNDB Then
			'	fDist1 = NDB.OnNAVRadius / OverHeadToler.Value * (hRoute - pFix.pPtPrj.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
			'End If

			Return CreatePrjCircle(pFix.pPtPrj, fDist1)
		End If

		pTriangle1 = New ArcGeometry.Polygon
		pTriangle2 = New ArcGeometry.Polygon

		If GuidNav.TypeCode = eNavaidType.VOR Then
			EnRouteTrackingToler = VOR.EnRouteTrackingToler
		Else
			EnRouteTrackingToler = NDB.EnRouteTrackingToler
		End If

		fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, pFix.pPtPrj)
		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, pFix.pPtPrj)

		pTriangle1.AddPoint(GuidNav.pPtPrj)
		pTriangle1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fDir + EnRouteTrackingToler, 10.0 * fDist))
		pTriangle1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fDir - EnRouteTrackingToler, 10.0 * fDist))

		pTopo = pTriangle1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		fDist = ReturnDistanceInMeters(InterNav.pPtPrj, pFix.pPtPrj)
		fDir = ReturnAngleInDegrees(InterNav.pPtPrj, pFix.pPtPrj)

		If InterNav.TypeCode = eNavaidType.DME Then
			D0 = System.Math.Sqrt(fDist * fDist + hRoute * hRoute) * DME.ErrorScalingUp + DME.MinimalError
			D = fDist + D0
			pSect0 = CreatePrjCircle(InterNav.pPtPrj, D)

			D = fDist - D0
			pSect1 = CreatePrjCircle(InterNav.pPtPrj, D)

			pTopo = pSect0
			pPoly = pTopo.Difference(pSect1)
			pTopo = pPoly

			pGeoCol = pTopo.Intersect(pTriangle1, ArcGeometry.esriGeometryDimension.esriGeometry2Dimension)

			If pGeoCol.GeometryCount > 1 Then
				lCollect = New ArcGeometry.Polygon
				pReleation = lCollect

				I = 0
				While I < pGeoCol.GeometryCount
					lCollect.AddGeometry(pGeoCol.Geometry(I))

					If Not pReleation.Contains(pFix.pPtPrj) Then
						pGeoCol.RemoveGeometries(I, 1)
					Else
						I += 1
					End If

					lCollect.RemoveGeometries(0, 1)
				End While

				lCollect = Nothing
			End If

			CreateFixArea = pGeoCol
		Else
			If InterNav.TypeCode = eNavaidType.VOR Then
				EnRouteInterToler = VOR.EnRouteInterToler
			Else
				EnRouteInterToler = NDB.EnRouteInterToler
			End If

			pTriangle2.AddPoint(InterNav.pPtPrj)
			pTriangle2.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + EnRouteInterToler, 10.0 * fDist))
			pTriangle2.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - EnRouteInterToler, 10.0 * fDist))

			pTopo = pTriangle2
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			CreateFixArea = pTopo.Intersect(pTriangle1, ArcGeometry.esriGeometryDimension.esriGeometry2Dimension)
		End If

		'pTriangle1 = Nothing
		'pTriangle2 = Nothing
	End Function

	Private Sub CreateNavArea(ByRef Nav As TypeDefinitions.NavaidType, ByRef Dir_Renamed As Double, ByRef Dist As Double, ByRef FullSeg As ArcGeometry.Polygon, ByRef PrimSeg As ArcGeometry.Polygon)
		Dim ptEndFull1 As ArcGeometry.IPoint
		Dim ptRange1 As ArcGeometry.IPoint = Nothing
		Dim ptBase As ArcGeometry.IPoint
		Dim ptTmp As ArcGeometry.IPoint
		Dim pt0 As ArcGeometry.IPoint
		Dim pt1 As ArcGeometry.IPoint
		Dim pt3 As ArcGeometry.IPoint

		Dim FullPolyLine1 As ArcGeometry.IPointCollection
		Dim PrimPolyLine1 As ArcGeometry.IPointCollection
		Dim FullPoly1 As ArcGeometry.IPointCollection
		Dim PrimPoly1 As ArcGeometry.IPointCollection

		Dim pConstructor As ArcGeometry.IConstructPoint
		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pCutter As ArcGeometry.IPolyline


		Dim EnRouteTrackingToler1 As Double
		Dim EnRouteTraking2Toler1 As Double
		Dim EnRoutePrimAreaWith1 As Double
		Dim EnRouteSecAreaWith1 As Double
		'Dim EnRouteInterToler1 As Double
		Dim fExtendFullDir As Double
		Dim InitWidth1 As Double
		Dim ConeAngle As Double
		Dim fTmp As Double

		Dim Side As Integer
		Dim I As Integer

		FullPoly1 = New ArcGeometry.Polygon
		PrimPoly1 = New ArcGeometry.Polygon

		FullPolyLine1 = New ArcGeometry.Polyline
		PrimPolyLine1 = New ArcGeometry.Polyline

		If Nav.TypeCode = eNavaidType.VOR Then
			EnRouteTrackingToler1 = VOR.EnRouteTrackingToler
			EnRouteTraking2Toler1 = VOR.EnRouteTracking2Toler
			'EnRouteInterToler1 = VOR.EnRouteInterToler
			EnRoutePrimAreaWith1 = VOR.EnRoutePrimAreaWith
			EnRouteSecAreaWith1 = VOR.EnRouteSecAreaWith
			InitWidth1 = VOR.InitWidth
			ConeAngle = VOR.ConeAngle
		Else
			EnRouteTrackingToler1 = NDB.EnRouteTrackingToler
			EnRouteTraking2Toler1 = NDB.EnRouteTracking2Toler
			'EnRouteInterToler1 = NDB.EnRouteInterToler
			EnRoutePrimAreaWith1 = NDB.EnRoutePrimAreaWith
			EnRouteSecAreaWith1 = NDB.EnRouteSecAreaWith
			InitWidth1 = NDB.InitWidth
			ConeAngle = NDB.ConeAngle
		End If

		pt0 = PointAlongPlane(Nav.pPtPrj, Dir_Renamed + 90.0, EnRoutePrimAreaWith1)
		pt1 = PointAlongPlane(Nav.pPtPrj, Dir_Renamed + 90.0, EnRouteSecAreaWith1)

		fTmp = System.Math.Tan(DegToRad(ConeAngle)) * (hRoute - Nav.pPtPrj.Z)
		PrimPolyLine1.AddPoint(PointAlongPlane(pt0, Dir_Renamed + 180.0, fTmp))
		FullPolyLine1.AddPoint(PointAlongPlane(pt1, Dir_Renamed + 180.0, fTmp))

		ptBase = PointAlongPlane(Nav.pPtPrj, Dir_Renamed + EnRouteTraking2Toler1 + 90.0, InitWidth1)
		pt3 = New ArcGeometry.Point
		pConstructor = pt3
		pConstructor.ConstructAngleIntersection(ptBase, DegToRad(Dir_Renamed + EnRouteTraking2Toler1), pt1, DegToRad(Dir_Renamed))

		ptTmp = New ArcGeometry.Point

		'fTmp = EnRoutePrimAreaWith1 / Sin(DegToRad(EnRouteTrackingToler1))

		pConstructor = ptTmp
		pConstructor.ConstructAngleIntersection(Nav.pPtPrj, DegToRad(Dir_Renamed + EnRouteTrackingToler1), pt0, DegToRad(Dir_Renamed))

		CircleVectorIntersect(Nav.pPtPrj, Nav.Range, pt0, Dir_Renamed, ptRange1)
		Side = SideDef(ptTmp, Dir_Renamed + 90.0, ptRange1)

		If Side > 0 Then 'Normal Range
			ptRange1 = PointAlongPlane(Nav.pPtPrj, Dir_Renamed + EnRouteTrackingToler1, Nav.Range)
			'DrawPointWithText ptRange1, "ptRange1"
			'DrawPointWithText ptBase, "ptBase"

			ptEndFull1 = New ArcGeometry.Point
			pConstructor = ptEndFull1
			pConstructor.ConstructAngleIntersection(ptBase, DegToRad(Dir_Renamed + EnRouteTraking2Toler1), ptRange1, DegToRad(Dir_Renamed + SplayAngl.Value))
			PrimPolyLine1.AddPoint(ptTmp)
		Else 'Anormal Range
			'    DrawPoint pt3, 255
			Side = SideDef(ptRange1, Dir_Renamed + SplayAngl.Value, pt3)

			If Side > 0 Then
				pConstructor = pt3
				pConstructor.ConstructAngleIntersection(pt1, DegToRad(Dir_Renamed), ptRange1, DegToRad(Dir_Renamed + SplayAngl.Value))
				ptEndFull1 = pt3
			Else
				ptEndFull1 = New ArcGeometry.Point
				pConstructor = ptEndFull1
				pConstructor.ConstructAngleIntersection(pt3, DegToRad(Dir_Renamed + EnRouteTraking2Toler1), ptRange1, DegToRad(Dir_Renamed + SplayAngl.Value))
			End If
		End If

		fExtendFullDir = ReturnAngleInDegrees(ptRange1, ptEndFull1)
		FullPolyLine1.AddPoint(pt3)
		FullPolyLine1.AddPoint(ptEndFull1)

		PrimPolyLine1.AddPoint(ptRange1)
		PrimPolyLine1.AddPoint(PointAlongPlane(ptEndFull1, fExtendFullDir, Dist))

		FullPoly1.AddPointCollection(FullPolyLine1)
		PrimPoly1.AddPointCollection(PrimPolyLine1)

		For I = FullPolyLine1.PointCount - 1 To 0 Step -1
			FullPoly1.AddPoint(GetSymmetricPoint(Nav.pPtPrj, Dir_Renamed, FullPolyLine1.Point(I)))
		Next I

		pTopo = FullPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		For I = PrimPolyLine1.PointCount - 1 To 0 Step -1
			PrimPoly1.AddPoint(GetSymmetricPoint(Nav.pPtPrj, Dir_Renamed, PrimPolyLine1.Point(I)))
		Next I

		pTopo = PrimPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pCutter = New ArcGeometry.Polyline
		ptTmp = PointAlongPlane(Nav.pPtPrj, Dir_Renamed, Dist)

		pCutter.FromPoint = PointAlongPlane(ptTmp, Dir_Renamed + 90.0, 2.0 * Dist)
		pCutter.ToPoint = PointAlongPlane(ptTmp, Dir_Renamed - 90.0, 2.0 * Dist)

		ClipByLine(FullPoly1, pCutter, Nothing, FullSeg, Nothing)
		ClipByLine(PrimPoly1, pCutter, Nothing, PrimSeg, Nothing)
	End Sub

	Private Sub Create2NavArea(ByRef Nav1 As TypeDefinitions.NavaidType, ByRef Dist1 As Double, ByRef Nav2 As TypeDefinitions.NavaidType, ByRef Dist2 As Double, ByRef bText As Boolean, ByRef bTVisible As Boolean, ByRef bCheck As Boolean, ByRef bCEnable As Boolean, ByRef FullSeg As ArcGeometry.Polygon, ByRef PrimSeg As ArcGeometry.Polygon)
		Dim ptCInterp1 As ArcGeometry.IPoint = Nothing
		Dim ptEndFull1 As ArcGeometry.IPoint
		Dim ptRange1 As ArcGeometry.IPoint = Nothing
		Dim ptBase As ArcGeometry.IPoint
		Dim ptTmp0 As ArcGeometry.IPoint
		Dim ptTmp1 As ArcGeometry.IPoint
		Dim ptTmp As ArcGeometry.IPoint
		Dim pt0 As ArcGeometry.IPoint
		Dim pt1 As ArcGeometry.IPoint
		Dim pt3 As ArcGeometry.IPoint

		Dim FullIntersection As ArcGeometry.IPointCollection = Nothing
		Dim PrimIntersection As ArcGeometry.IPointCollection = Nothing
		Dim FullPolyLine1 As ArcGeometry.IPointCollection
		Dim PrimPolyLine1 As ArcGeometry.IPointCollection
		Dim FullPolyLine2 As ArcGeometry.IPointCollection
		Dim PrimPolyLine2 As ArcGeometry.IPointCollection
		Dim FullPoly1 As ArcGeometry.IPointCollection
		Dim PrimPoly1 As ArcGeometry.IPointCollection
		Dim FullPoly2 As ArcGeometry.IPointCollection
		Dim PrimPoly2 As ArcGeometry.IPointCollection
		Dim pTriangle As ArcGeometry.IPointCollection

		Dim pCutLine As ArcGeometry.IPolyline
		Dim pCutLine2 As ArcGeometry.IPolyline
		Dim pCutter As ArcGeometry.IPolyline
		Dim pLine1 As ArcGeometry.IPolyline
		Dim pLine2 As ArcGeometry.IPolyline

		Dim pReleation1 As ArcGeometry.IRelationalOperator
		Dim pReleation2 As ArcGeometry.IRelationalOperator
		Dim pConstructor As ArcGeometry.IConstructPoint
		Dim pTopo As ArcGeometry.ITopologicalOperator2

		Dim fExtendFullDir As Double
		'Dim COP1Min As Double
		Dim COP1Max As Double
		Dim COP1Nom As Double

		'Dim COP2Min As Double
		Dim COP2Max As Double
		'Dim COP2Nom As Double

		Dim fDist As Double
		Dim fTmp As Double
		Dim fDir As Double
		Dim rDir As Double

		Dim EnRouteTrackingToler1 As Double
		Dim EnRouteTraking2Toler1 As Double
		Dim EnRoutePrimAreaWith1 As Double
		Dim EnRouteSecAreaWith1 As Double
		'Dim EnRouteInterToler1 As Double
		Dim InitWidth1 As Double

		Dim EnRouteTrackingToler2 As Double
		Dim EnRouteTraking2Toler2 As Double
		Dim EnRoutePrimAreaWith2 As Double
		Dim EnRouteSecAreaWith2 As Double
		'Dim EnRouteInterToler2 As Double
		Dim InitWidth2 As Double

		Dim bWithoutNav As Boolean
		Dim Contains1 As Boolean
		Dim Contains2 As Boolean
		Dim bFlg As Boolean
		Dim bCOP As Boolean

		Dim Side As Integer
		Dim I As Integer

		fDir = ReturnAngleInDegrees(Nav1.pPtPrj, Nav2.pPtPrj)
		fDist = ReturnDistanceInMeters(Nav1.pPtPrj, Nav2.pPtPrj)
		bWithoutNav = Nav1.Range + Nav2.Range < fDist

		FullPoly1 = New ArcGeometry.Polygon
		PrimPoly1 = New ArcGeometry.Polygon

		FullPolyLine1 = New ArcGeometry.Polyline
		PrimPolyLine1 = New ArcGeometry.Polyline

		If Nav1.TypeCode = eNavaidType.VOR Then
			EnRouteTrackingToler1 = VOR.EnRouteTrackingToler
			EnRouteTraking2Toler1 = VOR.EnRouteTracking2Toler
			'EnRouteInterToler1 = VOR.EnRouteInterToler
			EnRoutePrimAreaWith1 = VOR.EnRoutePrimAreaWith
			EnRouteSecAreaWith1 = VOR.EnRouteSecAreaWith
			InitWidth1 = VOR.InitWidth
		Else
			EnRouteTrackingToler1 = NDB.EnRouteTrackingToler
			EnRouteTraking2Toler1 = NDB.EnRouteTracking2Toler
			'EnRouteInterToler1 = NDB.EnRouteInterToler
			EnRoutePrimAreaWith1 = NDB.EnRoutePrimAreaWith
			EnRouteSecAreaWith1 = NDB.EnRouteSecAreaWith
			InitWidth1 = NDB.InitWidth
		End If

		pt0 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRoutePrimAreaWith1)
		pt1 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRouteSecAreaWith1)

		PrimPolyLine1.AddPoint(pt0)
		FullPolyLine1.AddPoint(pt1)

		ptBase = PointAlongPlane(Nav1.pPtPrj, fDir + EnRouteTraking2Toler1 + 90.0, InitWidth1)

		pt3 = New ArcGeometry.Point
		pConstructor = pt3
		pConstructor.ConstructAngleIntersection(ptBase, DegToRad(fDir + EnRouteTraking2Toler1), pt1, DegToRad(fDir))

		ptTmp = New ArcGeometry.Point
		pConstructor = ptTmp
		pConstructor.ConstructAngleIntersection(Nav1.pPtPrj, DegToRad(fDir + EnRouteTrackingToler1), pt0, DegToRad(fDir))

		CircleVectorIntersect(Nav1.pPtPrj, Nav1.Range, pt0, fDir, ptRange1)
		Side = SideDef(ptTmp, fDir + 90.0, ptRange1)
		If Side > 0 Then 'Normal Range
			ptRange1 = PointAlongPlane(Nav1.pPtPrj, fDir + EnRouteTrackingToler1, Nav1.Range)

			ptEndFull1 = New ArcGeometry.Point
			pConstructor = ptEndFull1
			pConstructor.ConstructAngleIntersection(ptBase, DegToRad(fDir + EnRouteTraking2Toler1), ptRange1, DegToRad(fDir + SplayAngl.Value))

			PrimPolyLine1.AddPoint(ptTmp)
		Else 'Anormal Range
			'    DrawPoint pt3, 255
			Side = SideDef(ptRange1, fDir + SplayAngl.Value, pt3)

			If Side > 0 Then
				pConstructor = pt3
				pConstructor.ConstructAngleIntersection(pt1, DegToRad(fDir), ptRange1, DegToRad(fDir + SplayAngl.Value))
				ptEndFull1 = pt3
			Else
				ptEndFull1 = New ArcGeometry.Point
				pConstructor = ptEndFull1
				pConstructor.ConstructAngleIntersection(pt3, DegToRad(fDir + EnRouteTraking2Toler1), ptRange1, DegToRad(fDir + SplayAngl.Value))
			End If
		End If

		fExtendFullDir = ReturnAngleInDegrees(ptRange1, ptEndFull1)

		FullPolyLine1.AddPoint(pt3)
		FullPolyLine1.AddPoint(ptEndFull1)

		PrimPolyLine1.AddPoint(ptRange1)
		PrimPolyLine1.AddPoint(PointAlongPlane(ptEndFull1, fExtendFullDir, fDist))

		FullPoly1.AddPointCollection(FullPolyLine1)
		PrimPoly1.AddPointCollection(PrimPolyLine1)

		For I = FullPolyLine1.PointCount - 1 To 0 Step -1
			FullPoly1.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, FullPolyLine1.Point(I)))
		Next I

		pTopo = FullPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		For I = PrimPolyLine1.PointCount - 1 To 0 Step -1
			PrimPoly1.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, PrimPolyLine1.Point(I)))
		Next I

		pTopo = PrimPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(FullPoly1, , Display.esriSimpleFillStyle.esriSFSVertical)
		'DrawPolygon(PrimPoly1, , Display.esriSimpleFillStyle.esriSFSVertical)
		'Application.DoEvents()

		rDir = Modulus(fDir + 180.0, 360.0)

		FullPoly2 = New ArcGeometry.Polygon
		PrimPoly2 = New ArcGeometry.Polygon

		FullPolyLine2 = New ArcGeometry.Polyline
		PrimPolyLine2 = New ArcGeometry.Polyline

		If Nav2.TypeCode = eNavaidType.VOR Then
			EnRouteTrackingToler2 = VOR.EnRouteTrackingToler
			EnRouteTraking2Toler2 = VOR.EnRouteTracking2Toler
			'EnRouteInterToler2 = VOR.EnRouteInterToler
			EnRoutePrimAreaWith2 = VOR.EnRoutePrimAreaWith
			EnRouteSecAreaWith2 = VOR.EnRouteSecAreaWith
			InitWidth2 = VOR.InitWidth
		Else
			EnRouteTrackingToler2 = NDB.EnRouteTrackingToler
			EnRouteTraking2Toler2 = NDB.EnRouteTracking2Toler
			'EnRouteInterToler2 = NDB.EnRouteInterToler
			EnRoutePrimAreaWith2 = NDB.EnRoutePrimAreaWith
			EnRouteSecAreaWith2 = NDB.EnRouteSecAreaWith
			InitWidth2 = NDB.InitWidth
		End If

		pt0 = PointAlongPlane(Nav2.pPtPrj, rDir - 90.0, EnRoutePrimAreaWith2)
		pt1 = PointAlongPlane(Nav2.pPtPrj, rDir - 90.0, EnRouteSecAreaWith2)

		PrimPolyLine2.AddPoint(pt0)
		FullPolyLine2.AddPoint(pt1)

		ptBase = PointAlongPlane(Nav2.pPtPrj, rDir - EnRouteTraking2Toler2 - 90.0, InitWidth2)

		pt3 = New ArcGeometry.Point
		pConstructor = pt3
		pConstructor.ConstructAngleIntersection(ptBase, DegToRad(rDir - EnRouteTraking2Toler2), pt1, DegToRad(rDir))

		ptTmp = New ArcGeometry.Point
		pConstructor = ptTmp
		pConstructor.ConstructAngleIntersection(Nav2.pPtPrj, DegToRad(rDir - EnRouteTrackingToler2), pt0, DegToRad(rDir))
		'==================================================================
		CircleVectorIntersect(Nav2.pPtPrj, Nav2.Range, pt0, rDir, ptRange1)
		Side = SideDef(ptTmp, rDir - 90.0, ptRange1)

		If Side < 0 Then 'Normal Range
			ptRange1 = PointAlongPlane(Nav2.pPtPrj, rDir - EnRouteTrackingToler2, Nav2.Range)
			ptEndFull1 = New ArcGeometry.Point
			pConstructor = ptEndFull1
			pConstructor.ConstructAngleIntersection(ptBase, DegToRad(rDir - EnRouteTraking2Toler2), ptRange1, DegToRad(rDir - SplayAngl.Value))

			PrimPolyLine2.AddPoint(ptTmp)
		Else 'Anormal Range
			Side = SideDef(ptRange1, rDir - SplayAngl.Value, pt3)

			If Side < 0 Then
				pConstructor = pt3
				pConstructor.ConstructAngleIntersection(pt1, DegToRad(rDir), ptRange1, DegToRad(rDir - SplayAngl.Value))
				ptEndFull1 = pt3
			Else
				ptEndFull1 = New ArcGeometry.Point
				pConstructor = ptEndFull1
				pConstructor.ConstructAngleIntersection(pt3, DegToRad(rDir - EnRouteTraking2Toler2), ptRange1, DegToRad(rDir - SplayAngl.Value))
			End If
		End If

		fExtendFullDir = ReturnAngleInDegrees(ptRange1, ptEndFull1)

		FullPolyLine2.AddPoint(pt3)
		FullPolyLine2.AddPoint(ptEndFull1)

		PrimPolyLine2.AddPoint(ptRange1)
		PrimPolyLine2.AddPoint(PointAlongPlane(ptEndFull1, fExtendFullDir, fDist))

		FullPoly2.AddPointCollection(FullPolyLine2)
		PrimPoly2.AddPointCollection(PrimPolyLine2)

		For I = FullPolyLine2.PointCount - 1 To 0 Step -1
			FullPoly2.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, FullPolyLine2.Point(I)))
		Next I

		pTopo = FullPoly2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		For I = PrimPolyLine2.PointCount - 1 To 0 Step -1
			PrimPoly2.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, PrimPolyLine2.Point(I)))
		Next I

		pTopo = PrimPoly2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(FullPoly1, , Display.esriSimpleFillStyle.esriSFSVertical)
		'DrawPolygon(PrimPoly1, , Display.esriSimpleFillStyle.esriSFSVertical)
		'Application.DoEvents()

		'DrawPolygon(FullPoly2, , Display.esriSimpleFillStyle.esriSFSHorizontal)
		'DrawPolygon(PrimPoly2, , Display.esriSimpleFillStyle.esriSFSHorizontal)
		'Application.DoEvents()
		'While True
		'	Application.DoEvents()
		'End While

		IntersectPolyLines(PrimPolyLine1, PrimPolyLine2, PrimIntersection)
		IntersectPolyLines(FullPolyLine1, FullPolyLine2, FullIntersection)
		bFlg = PrimIntersection.PointCount <> 1

		If bWithoutNav And Not bFlg Then
			If CircleVectorIntersect(Nav2.pPtPrj, Nav2.Range, PrimIntersection.Point(0), fDir + SplayAngl.Value + 180.0, ptCInterp1) < 0.0 Then
				MessageBox.Show(My.Resources.str0105)
				Exit Sub
			End If
			'    If OptionButton6 And CircleVectorIntersect(Nav1.ptPrj, Nav1.Range, PrimIntersection.Point(0), fDir - SplayAngl.Value, ptCInterp2) < 0.0 Then
			'		MessageBox.Show("В обратном направлении зоны невозможно стыковать.")
			'        Exit Sub
			'    End If
		End If

		'COP1Min = Math.Max(fDist - Nav2.Range, 0.0)
		COP1Max = Math.Min(Nav1.Range, fDist)

		'COP2Min = Math.Max(fDist - Nav1.Range, 0.0)
		COP2Max = Math.Min(Nav2.Range, fDist)

		'DrawPolyLine PrimPolyLine1, , 2
		'DrawPolyLine PrimPolyLine2, , 2
		'DrawPointWithText(FullIntersection.Point(0), "pTfull")
		'DrawPointWithText(PrimIntersection.Point(0), "pTprim")
		'Application.DoEvents()

		If PrimIntersection.PointCount = 1 Then
			COP1Nom = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir + 90.0)
			'COP1Nom = 0.5 * fDist
		Else
			COP1Nom = 0.5 * fDist
		End If

		'COP2Nom = fDist - COP1Nom
		'===========================================================
		bCOP = True 'False

		If bWithoutNav Or (Not bFlg And (FullIntersection.PointCount = 0)) Then
			bTVisible = False
			bText = False
		Else
			bTVisible = True

			If (Dist1 < 0.0) Or (Dist2 < 0.0) Then
				bText = False
				bCOP = True '?????????????????????????????????

				If PrimIntersection.PointCount = 1 Then
					fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir + 90.0)

					If (fTmp > COP1Max) Or (fDist - fTmp > COP2Max) Then
						bCOP = True
						If (fTmp > COP1Max) Then
							Dist1 = COP1Max
							Dist2 = fDist - COP1Max
						Else
							Dist1 = fDist - COP2Max
							Dist2 = COP2Max
						End If
					Else
						Dist1 = fTmp
						Dist2 = fDist - fTmp
					End If
				Else
					If EnRouteSecAreaWith2 <> EnRouteSecAreaWith1 Then
						If EnRouteSecAreaWith2 > EnRouteSecAreaWith1 Then
							Dist1 = COP1Max
							Dist2 = fDist - COP1Max
						Else
							Dist1 = fDist - COP2Max
							Dist2 = COP2Max
						End If
						'bTVisible = False
					Else
						fTmp = 0.5 * fDist
						If (fTmp > Nav1.Range) Or (fTmp > Nav2.Range) Then
							If (fTmp > Nav1.Range) Then
								Dist1 = Nav1.Range
								Dist2 = fDist - Nav1.Range
							Else
								Dist1 = fDist - Nav2.Range
								Dist2 = Nav2.Range
							End If
						Else
							Dist1 = fTmp
							Dist2 = fTmp
						End If
					End If
				End If
			Else
				bCOP = True
			End If
		End If

		'========================== ---------------- ================================
		bCEnable = bCOP
		pCutter = New ArcGeometry.Polyline

		'========================== FullIntersection ================================
		If bCOP Then
			fTmp = Math.Max(Dist1, distEps)
			ptTmp = PointAlongPlane(Nav1.pPtPrj, fDir, fTmp)
			pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 2.0 * fDist)
			pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 2.0 * fDist)
			'DrawPointWithText(ptTmp, "ptCut")
			'Application.DoEvents()
			'If Dist1 > COP1Nom Then
			If EnRouteSecAreaWith2 > EnRouteSecAreaWith1 Then
				pCutLine = ClipByPoly(pCutter, FullPoly2)

				If pCutLine.IsEmpty Then
					If PrimIntersection.PointCount > 0 Then
						fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						''Else
						''	pCutter.FromPoint = PointAlongPlane(Nav2.ptPrj, fDir + 90.0, 100.0)
						''	pCutter.ToPoint = PointAlongPlane(Nav2.ptPrj, fDir - 90.0, 3.0 * fTmp)
						''End If
						'pCutLine = ClipByPoly(pCutter, FullPoly2)
						'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
					End If
				End If

				ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
				ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()

					ptTmp0 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRouteSecAreaWith1)
					ptTmp = New ArcGeometry.Point
					pConstructor = ptTmp
					pConstructor.ConstructAngleIntersection(pCutLine.FromPoint, DegToRad(fDir + SecAreaCutAngl.Value), ptTmp0, DegToRad(fDir))

					'DrawPointWithText(ptTmp0, "ptTmp0")
					'DrawPointWithText(ptTmp, "ptTmp")
					'DrawPointWithText(pCutLine.FromPoint, "FromPoint")
					'Application.DoEvents()
					'bCEnable = SideDef(Nav2.ptPrj, fDir + 90.0, ptTmp) < 0          '????????????????????????????????????????????

					pTriangle = New ArcGeometry.Polygon
					If bCEnable And bCheck Then
						pTriangle.AddPoint(ptTmp)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp))
					Else
						pTriangle.AddPoint(PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, InitWidth2))
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(PointAlongPlane(Nav1.pPtPrj, fDir - 90.0, InitWidth2))
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon(FullPolyLine2, , Display.esriSimpleFillStyle.esriSFSVertical)
					'DrawPolygon(FullPolyLine1, , Display.esriSimpleFillStyle.esriSFSHorizontal)
					'DrawPolygon(pTriangle, RGB(0, 0, 255), Display.esriSimpleFillStyle.esriSFSCross)
					'DrawPolygon(pTriangle, RGB(0, 255, 0), Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
					'DrawPointWithText(ptTmp, "ptTmp")
					'DrawPolyLine(pCutLine, 0, 2)
					'Application.DoEvents()

					FullPolyLine1 = pTopo.Union(FullPolyLine1)
					ChangePoints(FullPolyLine1, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				pTopo = FullPolyLine2
				FullSeg = pTopo.Union(FullPolyLine1)
			ElseIf EnRouteSecAreaWith2 < EnRouteSecAreaWith1 Then
				pCutLine = ClipByPoly(pCutter, FullPoly1)

				'DrawPolyLine(pCutter, , 2)
				'DrawPolygon(FullPoly1, , Display.esriSimpleFillStyle.esriSFSHorizontal)
				'Application.DoEvents()

				If pCutLine.IsEmpty Then
					If PrimIntersection.PointCount > 0 Then
						fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						''Else
						''	pCutter.FromPoint = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, 100.0)
						''	pCutter.ToPoint = PointAlongPlane(Nav1.ptPrj, fDir - 90.0, 3.0 * fTmp)
						''End If

						'pCutLine = ClipByPoly(pCutter, FullPoly1)
						'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
					End If
				End If

				ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
				ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()

					ptTmp0 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRouteSecAreaWith2)
					ptTmp = New ArcGeometry.Point
					pConstructor = ptTmp
					pConstructor.ConstructAngleIntersection(pCutLine.FromPoint, DegToRad(fDir - SecAreaCutAngl.Value), ptTmp0, DegToRad(fDir))
					'bCEnable = SideDef(Nav1.ptPrj, fDir + 90.0, ptTmp) > 0			 '????????????????????????????????????????????

					pTriangle = New ArcGeometry.Polygon
					If bCEnable And bCheck Then
						pTriangle.AddPoint(ptTmp)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp))
					Else
						pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, InitWidth1))
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, InitWidth1))
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon(FullPolyLine1, , Display.esriSimpleFillStyle.esriSFSVertical)
					'DrawPolygon(pTriangle, RGB(0, 0, 255), Display.esriSimpleFillStyle.esriSFSCross)
					'DrawPolygon(pTriangle, RGB(0, 255, 0), Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
					'DrawPointWithText(ptTmp, "ptTmp")
					'DrawPolyLine(pCutLine, 0, 2)
					'Application.DoEvents()

					FullPolyLine2 = pTopo.Union(FullPolyLine2)
					ChangePoints(FullPolyLine2, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				pTopo = FullPolyLine1
				FullSeg = pTopo.Union(FullPolyLine2)
			Else
				pCutLine = ClipByPoly(pCutter, FullPoly1)
				pCutLine2 = ClipByPoly(pCutter, FullPoly2)
				pt0 = Nav2.pPtPrj
				'DrawPolyLine(pCutLine2, 0, 2)
				'Application.DoEvents()

				If pCutLine.IsEmpty Or (pCutLine.Length < pCutLine2.Length) Then
					pCutLine = pCutLine2
					pt0 = Nav1.pPtPrj

					If pCutLine.IsEmpty And (PrimIntersection.PointCount > 0) Then
						fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						'pt0 = Nav1.ptPrj

						''Else
						''	pCutter.FromPoint = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, 100.0)
						''	pCutter.ToPoint = PointAlongPlane(Nav1.ptPrj, fDir - 90.0, 3.0 * fTmp)
						''End If

						'pCutLine = ClipByPoly(pCutter, FullPoly1)
						'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
					End If
				End If

				ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
				ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()

					ptTmp0 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRouteSecAreaWith1)
					ptTmp = New ArcGeometry.Point
					pConstructor = ptTmp
					pConstructor.ConstructAngleIntersection(pCutLine.FromPoint, DegToRad(fDir - SecAreaCutAngl.Value), ptTmp0, DegToRad(fDir))
					'bCEnable = SideDef(Nav1.ptPrj, fDir + 90.0, ptTmp) > 0			 '????????????????????????????????????????????

					pTriangle = New ArcGeometry.Polygon
					If bCEnable And bCheck Then
						pTriangle.AddPoint(ptTmp)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp))
					Else
						pTriangle.AddPoint(PointAlongPlane(pt0, fDir + 90.0, InitWidth1))
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(PointAlongPlane(pt0, fDir - 90.0, InitWidth1))
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon(FullPolyLine2, , Display.esriSimpleFillStyle.esriSFSVertical)
					'DrawPolygon(pTriangle, RGB(0, 0, 255), Display.esriSimpleFillStyle.esriSFSCross)
					'DrawPolygon(pTriangle, RGB(0, 255, 0), Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
					'DrawPolyLine(pCutLine, 0, 2)
					'Application.DoEvents()

					FullPolyLine2 = pTopo.Union(FullPolyLine2)
					ChangePoints(FullPolyLine2, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				'While True
				'	Application.DoEvents()
				'End While
				'DrawPolygon(FullPolyLine2)
				'Application.DoEvents()

				pTopo = FullPolyLine1
				FullSeg = pTopo.Union(FullPolyLine2)
			End If
		Else
			Select Case FullIntersection.PointCount
				Case 0
					If bFlg Then
						If EnRouteSecAreaWith2 < EnRouteSecAreaWith1 Then
							fTmp = EnRouteSecAreaWith2
							pCutter.FromPoint = PointAlongPlane(Nav1.pPtPrj, fDir - 90.0, 2.0 * fTmp)
							pCutter.ToPoint = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, 2.0 * fTmp)
							ClipByLine(FullPoly2, pCutter, Nothing, FullSeg, Nothing)
						ElseIf EnRouteSecAreaWith2 > EnRouteSecAreaWith1 Then
							fTmp = EnRouteSecAreaWith1
							pCutter.FromPoint = PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, 2.0 * fTmp)
							pCutter.ToPoint = PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, 2.0 * fTmp)

							ClipByLine(FullPoly1, pCutter, Nothing, FullSeg, Nothing)
						Else
							ptTmp = PointAlongPlane(Nav1.pPtPrj, fDir, 0.5 * fDist)
							pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 2 * EnRouteSecAreaWith1)
							pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 2 * EnRouteSecAreaWith1)

							ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
							ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)

							pTopo = FullPolyLine1
							FullSeg = pTopo.Union(FullPolyLine2)
						End If
					Else
						If bWithoutNav Then
							pCutter.FromPoint = PointAlongPlane(ptCInterp1, fDir + 90.0, fDist)
							pCutter.ToPoint = PointAlongPlane(ptCInterp1, fDir - 90.0, 3.0 * fDist)

							ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)

							If FullPolyLine1.PointCount = 0 Then
								FullPolyLine1 = FullPoly1
							Else
								pCutLine = ClipByPoly(pCutter, FullPoly1)

								If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
									pCutLine.ReverseOrientation()
								End If

								pTriangle = New ArcGeometry.Polygon
								pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, InitWidth2))

								pTriangle.AddPoint(pCutLine.FromPoint)

								pTriangle.AddPoint(pCutLine.ToPoint)
								pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, InitWidth2))

								pTopo = pTriangle
								pTopo.IsKnownSimple_2 = False
								pTopo.Simplify()

								FullPolyLine1 = pTopo.Union(FullPolyLine1)

								ChangePoints(FullPolyLine1, pCutLine, Nav1.pPtPrj, fDir)
							End If

							ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)
							If FullPolyLine2.PointCount = 0 Then
								FullPolyLine2 = FullPoly2
							End If

							pTopo = FullPolyLine1
							FullSeg = pTopo.Union(FullPolyLine2)

							Contains1 = False
							Contains2 = False
						Else
							pReleation1 = FullPoly1
							pReleation2 = FullPoly2

							pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, fDist)

							pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fDist)
							Contains1 = pReleation1.Contains(PrimIntersection.Point(0))
							Contains2 = pReleation2.Contains(PrimIntersection.Point(0))

							If Contains1 And Contains2 Then
								pLine1 = ClipByPoly(pCutter, FullPoly1)
								pLine2 = ClipByPoly(pCutter, FullPoly2)

								If pLine1.Length > pLine2.Length Then
									Contains2 = False
								Else
									Contains1 = False
								End If
							End If

							If Contains1 Then
								pCutLine = ClipByPoly(pCutter, FullPoly1)

								If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
									pCutLine.ReverseOrientation()
								End If

								ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
								'                If Not pCutLine.IsEmpty Then
								pTriangle = New ArcGeometry.Polygon
								pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, InitWidth2))

								pTriangle.AddPoint(pCutLine.FromPoint)

								pTriangle.AddPoint(pCutLine.ToPoint)
								pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, InitWidth2))

								pTopo = pTriangle
								pTopo.IsKnownSimple_2 = False
								pTopo.Simplify()

								FullPolyLine2 = pTopo.Union(FullPoly2)

								ChangePoints(FullPolyLine2, pCutLine, Nav1.pPtPrj, fDir)
								ChangePoints(FullPolyLine1, pCutLine, Nav1.pPtPrj, fDir)    '"""
								'                End If

								pTopo = FullPolyLine2

								FullSeg = pTopo.Union(FullPolyLine1)
							ElseIf Contains2 Then
								pCutLine = ClipByPoly(pCutter, FullPoly2)

								If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
									pCutLine.ReverseOrientation()
								End If

								ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)
								'                If Not pCutLine.IsEmpty Then
								pTriangle = New ArcGeometry.Polygon
								pTriangle.AddPoint(PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, InitWidth1))

								pTriangle.AddPoint(pCutLine.FromPoint)

								pTriangle.AddPoint(pCutLine.ToPoint)
								pTriangle.AddPoint(PointAlongPlane(Nav1.pPtPrj, fDir - 90.0, InitWidth1))

								pTopo = pTriangle
								pTopo.IsKnownSimple_2 = False
								pTopo.Simplify()

								FullPolyLine1 = pTopo.Union(FullPoly1)

								ChangePoints(FullPolyLine1, pCutLine, Nav1.pPtPrj, fDir)
								ChangePoints(FullPolyLine2, pCutLine, Nav1.pPtPrj, fDir)    '"""
								'                End If

								pTopo = FullPolyLine1
								FullSeg = pTopo.Union(FullPolyLine2)
							Else
								pTopo = FullPoly1
								FullSeg = pTopo.Union(FullPoly2)
							End If
						End If
					End If
				Case 1
					If bWithoutNav Then
						pCutter.FromPoint = PointAlongPlane(ptCInterp1, fDir + 90.0, fDist)
						pCutter.ToPoint = PointAlongPlane(ptCInterp1, fDir - 90.0, 3.0 * fDist)
						ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)

						If FullPolyLine1.PointCount = 0 Then
							FullPolyLine1 = FullPoly1
						Else
							pCutLine = ClipByPoly(pCutter, FullPoly1)
							If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
								pCutLine.ReverseOrientation()
							End If

							pTriangle = New ArcGeometry.Polygon
							pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, InitWidth2))
							pTriangle.AddPoint(pCutLine.FromPoint)
							pTriangle.AddPoint(pCutLine.ToPoint)
							pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, InitWidth2))

							pTopo = pTriangle
							pTopo.IsKnownSimple_2 = False
							pTopo.Simplify()

							FullPolyLine1 = pTopo.Union(FullPolyLine1)
							ChangePoints(FullPolyLine1, pCutLine, Nav1.pPtPrj, fDir)
						End If

						ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)
						If FullPolyLine2.PointCount = 0 Then FullPolyLine2 = FullPoly2

						pTopo = FullPolyLine1
						FullSeg = pTopo.Union(FullPolyLine2)
					Else
						fTmp = Point2LineDistancePrj(FullIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(FullIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(FullIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
						ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)
						pTopo = FullPolyLine1
						FullSeg = pTopo.Union(FullPolyLine2)
					End If
				Case 2
					If EnRouteSecAreaWith2 < EnRouteSecAreaWith1 Then
						fTmp = EnRouteSecAreaWith2
						pCutter.FromPoint = PointAlongPlane(Nav1.pPtPrj, fDir - 90.0, 2.0 * fTmp)
						pCutter.ToPoint = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, 2.0 * fTmp)
						ClipByLine(FullPoly2, pCutter, Nothing, FullSeg, Nothing)
					ElseIf EnRouteSecAreaWith2 > EnRouteSecAreaWith1 Then
						fTmp = EnRouteSecAreaWith1
						pCutter.FromPoint = PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, 2.0 * fTmp)
						pCutter.ToPoint = PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, 2.0 * fTmp)
						ClipByLine(FullPoly1, pCutter, Nothing, FullSeg, Nothing)
					Else
						ptTmp = PointAlongPlane(Nav1.pPtPrj, fDir, 0.5 * fDist)
						pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 2 * EnRouteSecAreaWith1)
						pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 2 * EnRouteSecAreaWith1)

						ClipByLine(FullPoly1, pCutter, Nothing, FullPolyLine1, Nothing)
						ClipByLine(FullPoly2, pCutter, FullPolyLine2, Nothing, Nothing)
						pTopo = FullPolyLine1
						FullSeg = pTopo.Union(FullPolyLine2)
					End If
			End Select
		End If

		'========================== PrimIntersection ================================
		'Select Case PrimIntersection.PointCount

		If bCOP Then
			fTmp = Math.Max(Dist1, distEps)
			ptTmp0 = PointAlongPlane(Nav1.pPtPrj, fDir, fTmp)
			pCutter.FromPoint = PointAlongPlane(ptTmp0, fDir + 90.0, 2 * fDist)
			pCutter.ToPoint = PointAlongPlane(ptTmp0, fDir - 90.0, 2 * fDist)

			'If Dist1 > COP1Nom Then
			If EnRouteSecAreaWith2 > EnRouteSecAreaWith1 Then
				pCutLine = ClipByPoly(pCutter, PrimPoly2)

				If pCutLine.IsEmpty Then
					If PrimIntersection.PointCount > 0 Then
						fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						''Else
						''	pCutter.FromPoint = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, 100.0)
						''	pCutter.ToPoint = PointAlongPlane(Nav1.ptPrj, fDir - 90.0, 3.0 * fTmp)
						''End If

						'pCutLine = ClipByPoly(pCutter, FullPoly1)
						'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
					End If
				End If

				ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
				ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
					pTriangle = New ArcGeometry.Polygon

					If bCEnable And bCheck Then
						ptTmp1 = PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, EnRoutePrimAreaWith1)
						pConstructor = ptTmp0
						pConstructor.ConstructAngleIntersection(ptTmp, DegToRad(fDir - 90.0), ptTmp1, DegToRad(fDir))

						pTriangle.AddPoint(ptTmp0)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp0))
					Else
						pTriangle.AddPoint(Nav1.pPtPrj)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					PrimPolyLine1 = pTopo.Union(PrimPolyLine1)
					ChangePoints(PrimPolyLine1, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				pTopo = PrimPolyLine2
				PrimSeg = pTopo.Union(PrimPolyLine1)
			ElseIf EnRouteSecAreaWith2 < EnRouteSecAreaWith1 Then
				pCutLine = ClipByPoly(pCutter, PrimPoly1)

				If pCutLine.IsEmpty Then
					fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
					pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
					pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
					''Else
					''	pCutter.FromPoint = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, 100.0)
					''	pCutter.ToPoint = PointAlongPlane(Nav1.ptPrj, fDir - 90.0, 3.0 * fTmp)
					''End If

					'pCutLine = ClipByPoly(pCutter, FullPoly1)
					'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
				End If

				ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
				ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()

					pTriangle = New ArcGeometry.Polygon

					If bCEnable And bCheck Then
						ptTmp1 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRoutePrimAreaWith2)
						pConstructor = ptTmp0
						pConstructor.ConstructAngleIntersection(ptTmp, DegToRad(fDir - 90.0), ptTmp1, DegToRad(fDir))

						pTriangle.AddPoint(ptTmp0)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp0))
					Else
						pTriangle.AddPoint(Nav2.pPtPrj)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon(pTriangle, RGB(0, 255, 0), Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
					'Application.DoEvents()

					PrimPolyLine2 = pTopo.Union(PrimPolyLine2)
					ChangePoints(PrimPolyLine2, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				pTopo = PrimPolyLine1
				PrimSeg = pTopo.Union(PrimPolyLine2)
			Else
				pCutLine = ClipByPoly(pCutter, PrimPoly1)
				pCutLine2 = ClipByPoly(pCutter, PrimPoly2)
				pt0 = Nav2.pPtPrj
				'DrawPolyLine(pCutLine2, 0, 2)
				'Application.DoEvents()

				If pCutLine.IsEmpty Or (pCutLine.Length < pCutLine2.Length) Then
					pCutLine = pCutLine2
					pt0 = Nav1.pPtPrj

					If pCutLine.IsEmpty Then
						fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
						pCutter.FromPoint = PointAlongPlane(PrimIntersection.Point(0), fDir + 90.0, 100.0)
						pCutter.ToPoint = PointAlongPlane(PrimIntersection.Point(0), fDir - 90.0, 3.0 * fTmp)
						''Else
						''	pCutter.FromPoint = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, 100.0)
						''	pCutter.ToPoint = PointAlongPlane(Nav1.ptPrj, fDir - 90.0, 3.0 * fTmp)
					End If

					'pCutLine = ClipByPoly(pCutter, FullPoly1)
					'If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()
				End If

				ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
				ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)

				If Not pCutLine.IsEmpty Then
					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then pCutLine.ReverseOrientation()

					pTriangle = New ArcGeometry.Polygon

					If bCEnable And bCheck Then
						ptTmp1 = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, EnRoutePrimAreaWith2)
						pConstructor = ptTmp0
						pConstructor.ConstructAngleIntersection(ptTmp, DegToRad(fDir - 90.0), ptTmp1, DegToRad(fDir))

						pTriangle.AddPoint(ptTmp0)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp0))
					Else
						pTriangle.AddPoint(pt0)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon(pTriangle, RGB(0, 0, 255), Display.esriSimpleFillStyle.esriSFSCross)
					'DrawPolygon(PrimPolyLine1, , Display.esriSimpleFillStyle.esriSFSHorizontal)
					'Application.DoEvents()

					PrimPolyLine2 = pTopo.Union(PrimPolyLine2)
					ChangePoints(PrimPolyLine2, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
				End If

				pTopo = PrimPolyLine1
				PrimSeg = pTopo.Union(PrimPolyLine2)
			End If
		Else
			If Not bFlg Then
				'Case 1
				If bWithoutNav Then
					CircleVectorIntersect(Nav2.pPtPrj, COP2Max, PrimIntersection.Point(0), fDir + SplayAngl.Value + 180.0, ptTmp)
					fTmp = COP2Max
					pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 3.0 * fTmp)
					pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 3.0 * fTmp)
					ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
					'==========================================================================================
					pCutLine = ClipByPoly(pCutter, PrimPoly1)

					If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
						pCutLine.ReverseOrientation()
					End If

					pTriangle = New ArcGeometry.Polygon
					ptTmp0 = PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, EnRoutePrimAreaWith2)
					ptTmp = New ArcGeometry.Point
					pConstructor = ptTmp

					pConstructor.ConstructAngleIntersection(pCutLine.FromPoint, DegToRad(fDir - SecAreaCutAngl.Value), ptTmp0, DegToRad(fDir))

					If SideDef(Nav2.pPtPrj, fDir + 90.0, ptTmp) < 0 Then
						pTriangle.AddPoint(ptTmp)
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(GetSymmetricPoint(Nav1.pPtPrj, fDir, ptTmp))
					Else
						pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, InitWidth2))
						pTriangle.AddPoint(pCutLine.FromPoint)
						pTriangle.AddPoint(pCutLine.ToPoint)
						pTriangle.AddPoint(PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, InitWidth2))
					End If

					pTopo = pTriangle
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					PrimPolyLine1 = pTopo.Union(PrimPolyLine1)

					ChangePoints(PrimPolyLine1, pCutLine, Nav1.pPtPrj, fDir)    '?????????????
					'==========================================================================================
					'If OptionButton6 Then
					'	CircleVectorIntersect(Nav1.ptPrj, COP1Max, PrimIntersection.Point(0), fDir - SplayAngl.Value, ptTmp)
					'	pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 3.0 * fTmp)
					'	pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 3.0 * fTmp)

					'	pCutLine = ClipByPoly(pCutter, PrimPoly2)
					'	If SideDef(pCutLine.FromPoint, fDir, pCutLine.ToPoint) < 0 Then
					'		pCutLine.ReverseOrientation()
					'	End If
					'End If
					'==========================================================================================

					ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)
					'==========================================================================================
					'If OptionButton6 Then
					'	pTriangle = New Polygon
					'	ptTmp0 = PointAlongPlane(Nav1.ptPrj, fDir + 90.0, EnRoutePrimAreaWith1)
					'	ptTmp = New Point
					'	pConstructor = ptTmp
					'	pConstructor.ConstructAngleIntersection(pCutLine.FromPoint, DegToRad(fDir + SecAreaCutAngl.Value + 180.0), ptTmp0, DegToRad(fDir))

					'	If SideDef(Nav1.ptPrj, fDir - 90.0, ptTmp) < 0 Then
					'		pTriangle.AddPoint(ptTmp)
					'		pTriangle.AddPoint(pCutLine.FromPoint)
					'		pTriangle.AddPoint(pCutLine.ToPoint)
					'		pTriangle.AddPoint(GetSymmetricPoint(Nav1.ptPrj, fDir, ptTmp))
					'	Else
					'		pTriangle.AddPoint(PointAlongPlane(Nav1.ptPrj, fDir + 90.0, InitWidth1))
					'		pTriangle.AddPoint(pCutLine.FromPoint)
					'		pTriangle.AddPoint(pCutLine.ToPoint)
					'		pTriangle.AddPoint(PointAlongPlane(Nav1.ptPrj, fDir - 90.0, InitWidth1))
					'	End If

					'	pTopo = pTriangle
					'	pTopo.IsKnownSimple_2 = False
					'	pTopo.Simplify()

					'	PrimPolyLine1 = pTopo.Union(PrimPolyLine1)
					'End If
					'==========================================================================================
					pTopo = PrimPolyLine1

					PrimSeg = pTopo.Union(PrimPolyLine2)
				Else
					fTmp = Point2LineDistancePrj(PrimIntersection.Point(0), Nav1.pPtPrj, fDir)
					ptTmp = PrimIntersection.Point(0)
					pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 3.0 * fTmp)
					pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 3.0 * fTmp)
					ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
					ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)

					pTopo = PrimPolyLine1
					PrimSeg = pTopo.Union(PrimPolyLine2)
				End If
			Else
				'Case 0, 2
				If EnRoutePrimAreaWith2 < EnRoutePrimAreaWith1 Then
					fTmp = EnRoutePrimAreaWith2
					pCutter.FromPoint = PointAlongPlane(Nav1.pPtPrj, fDir - 90.0, 2.0 * fTmp)
					pCutter.ToPoint = PointAlongPlane(Nav1.pPtPrj, fDir + 90.0, 2.0 * fTmp)
					ClipByLine(PrimPoly2, pCutter, Nothing, PrimSeg, Nothing)
				ElseIf EnRoutePrimAreaWith2 > EnRoutePrimAreaWith1 Then
					fTmp = EnRoutePrimAreaWith1
					pCutter.FromPoint = PointAlongPlane(Nav2.pPtPrj, fDir + 90.0, 2.0 * fTmp)
					pCutter.ToPoint = PointAlongPlane(Nav2.pPtPrj, fDir - 90.0, 2.0 * fTmp)
					ClipByLine(PrimPoly1, pCutter, Nothing, PrimSeg, Nothing)
				Else
					ptTmp = PointAlongPlane(Nav1.pPtPrj, fDir, 0.5 * fDist)
					pCutter.FromPoint = PointAlongPlane(ptTmp, fDir + 90.0, 2.0 * EnRoutePrimAreaWith1)
					pCutter.ToPoint = PointAlongPlane(ptTmp, fDir - 90.0, 2.0 * EnRoutePrimAreaWith1)

					ClipByLine(PrimPoly1, pCutter, Nothing, PrimPolyLine1, Nothing)
					ClipByLine(PrimPoly2, pCutter, PrimPolyLine2, Nothing, Nothing)

					pTopo = PrimPolyLine2
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					pTopo = PrimPolyLine1
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					PrimSeg = pTopo.Union(PrimPolyLine2)
				End If
			End If
			'End Select
		End If
		'===========================================================================================
		'If COP1Min <= COP1Max Then
		'    TextBox3.ToolTipText = "Интервал: от " + CStr(Round(COP1Min)) + LoadResString(101) + CStr(Round(COP1Max))
		'Else
		'    TextBox3.ToolTipText = ""
		'End If

		'If COP2Min <= COP2Max Then
		'    TextBox4.ToolTipText = "Интервал: от " + CStr(Round(COP2Min)) + LoadResString(101) + CStr(Round(COP2Max))
		'Else
		'    TextBox4.ToolTipText = ""
		'End If
	End Sub

	Private Shared Function CreateTurnArea(ByRef WSpeed As Double, ByRef TurnR As Double, ByRef V As Double, ByRef AztEnter As Double, ByRef AztOut As Double, ByRef pTurnDir As Integer, ByRef ptFix As ArcGeometry.IPoint, ByRef BasePoints As ArcGeometry.IPointCollection) As ArcGeometry.IPointCollection
		Dim Rv As Double
		Dim dAng As Double
		Dim Bank As Double
		Dim coef As Double
		Dim azt12 As Double
		Dim dAng0 As Double

		Dim fDistP As Double
		Dim fDistL As Double
		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim fDist3 As Double
		Dim fDist4 As Double

		Dim AztCur As Double
		Dim AztNext As Double
		Dim TurnAng As Double
		Dim wAztOut As Double

		Dim I As Integer
		Dim N As Integer
		Dim K As Integer

		Dim pt1 As ArcGeometry.IPoint = Nothing
		Dim pt2 As ArcGeometry.IPoint = Nothing
		Dim Points(2) As ArcGeometry.IPoint
		Dim PtIntersect As ArcGeometry.IPoint

		Dim Constructor As ArcGeometry.IConstructPoint
		Dim TmpSpiral As ArcGeometry.IPointCollection

		N = BasePoints.PointCount

		fDist1 = -10.0
		fDist2 = -10.0
		fDist3 = -10.0
		fDist4 = -10.0

		For I = 0 To N - 1
			fDistP = Point2LineDistancePrj(BasePoints.Point(I), ptFix, AztEnter + 90.0)
			fDistL = Point2LineDistancePrj(BasePoints.Point(I), ptFix, AztEnter)
			If SideDef(ptFix, AztEnter + 90.0, BasePoints.Point(I)) > 0 Then
				If SideDef(ptFix, AztEnter, BasePoints.Point(I)) > 0 Then
					If fDistP > fDist1 Then
						fDist1 = fDistP
						fDist3 = fDistL
						pt1 = BasePoints.Point(I)
					ElseIf (System.Math.Abs(fDistP - fDist1) < distEps) And (fDistL > fDist3) Then
						fDist1 = fDistP
						fDist3 = fDistL
						pt1 = BasePoints.Point(I)
					End If
				Else
					If fDistP > fDist2 Then
						fDist2 = fDistP
						fDist4 = fDistL
						pt2 = BasePoints.Point(I)
					ElseIf (System.Math.Abs(fDistP - fDist2) < distEps) And (fDistL > fDist4) Then
						fDist2 = fDistP
						fDist4 = fDistL
						pt2 = BasePoints.Point(I)
					End If
				End If
			End If
		Next I

		'DrawPolygon BasePoints, 0

		If pTurnDir > 0 Then
			Points(0) = pt1
			Points(1) = pt2
		Else
			Points(0) = pt2
			Points(1) = pt1
		End If

		Points(2) = PointAlongPlane(Points(1), AztOut, 1000.0)

		Bank = Radius2Bank(TurnR, V)
		Rv = 6355.0 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
		If (Rv > 3.0) Then Rv = 3.0

		coef = WSpeed / (3.6 * Rv)

		AztOut = Modulus(AztOut, 360.0)
		wAztOut = AztOut

		AztEnter = Modulus(AztEnter, 360.0)

		If SubtractAngles(wAztOut, AztEnter) < 1.0 Then
			TurnAng = 0.0
		Else
			TurnAng = Modulus((wAztOut - AztEnter) * pTurnDir, 360.0)
		End If

		Dim result As ArcGeometry.IPointCollection
		result = New ArcGeometry.Polygon

		AztCur = AztEnter
		N = 3
		K = 0
		I = 0
		AztNext = ReturnAngleInDegrees(Points(0), Points(1))
		TmpSpiral = New ArcGeometry.Polyline
		'======================================
		Do
			azt12 = ReturnAngleInDegrees(Points(I), Points((I + 1) Mod N))

			If SideFrom2Angle(AztNext, azt12) * pTurnDir < 0 Then
				dAng0 = Modulus((AztCur - azt12) * pTurnDir, 360.0)
				dAng = dAng - dAng0
				AztNext = azt12

				CreateWindSpiral(Points(I), AztEnter, azt12 - 90.0 * pTurnDir, azt12, TurnR, coef, pTurnDir, TmpSpiral)

				PtIntersect = New ArcGeometry.Point
				Constructor = PtIntersect

				Constructor.ConstructAngleIntersection(result.Point(result.PointCount - 1), DegToRad(AztCur), TmpSpiral.Point(TmpSpiral.PointCount - 1), DegToRad(azt12))
				result.AddPoint(PtIntersect)
			Else
				dAng0 = Modulus((azt12 - AztCur) * pTurnDir, 360.0)
				dAng = dAng + dAng0

				If dAng < TurnAng Then
					AztNext = azt12
				Else
					AztNext = wAztOut
				End If

				CreateWindSpiral(Points(I), AztEnter, AztCur, AztNext, TurnR, coef, pTurnDir, result)
			End If

			I = (I + 1) Mod N
			AztCur = AztNext
			K += 1
		Loop While SubtractAngles(AztNext, wAztOut) > degEps

		Return result
	End Function

	Private Function Fill201() As Integer
		Dim bFlag As Boolean

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim M As Integer
		Dim N As Integer

		Dim Side2 As Integer
		Dim iCopSide As Integer

		Dim fD0 As Double
		Dim fRange As Double
		Dim fConRad As Double
		Dim dReserve As Double
		Dim TrackToler As Double
		Dim TrackRange As Double
		'Dim fInterToler As Double
		Dim CurrMaxAngle As Double
		Dim ptD0 As ArcGeometry.IPoint
		Dim ptCOP As ArcGeometry.IPoint
		'Dim pProxi1 As ArcGeometry.IProximityOperator
		Dim pProxi2 As ArcGeometry.IProximityOperator
		Dim pTopoOper As ArcGeometry.ITopologicalOperator2
		Dim pNav1TrackArea As ArcGeometry.IPointCollection

		Dim DirWPT As TypeDefinitions.NavaidType
		Dim StartFIX As TypeDefinitions.NavaidType
		Dim GNav1 As TypeDefinitions.NavaidType
		Dim GNav2 As TypeDefinitions.NavaidType = New TypeDefinitions.NavaidType()

		Dim NavLeftInt As LowHigh
		Dim NavRightInt As LowHigh

		GNav1 = Segments(SegmentNum).StartNav
		DirWPT = Segments(SegmentNum).DirWPT

		GNav2.Index = -1
		GNav2.TypeCode = eNavaidType.NONE
		If DirWPT.TypeCode > eNavaidType.NONE Then GNav2 = DirWPT

		ptCOP = Segments(SegmentNum).ptCOP
		If Not ptCOP Is Nothing Then iCopSide = SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, ptCOP)

		ComboBox201.Items.Clear()

		If OptionButton202.Checked Then
			ReDim GuidNavDat2(1)
			J = 0

			GuidNavDat2(0) = GNav1
			ComboBox201.Items.Add(GNav1.CallSign)
			ReDim GuidNavDat2(0).ValMin(0)
			ReDim GuidNavDat2(0).ValMax(0)
			GuidNavDat2(0).ValMin(0) = fCurrDir
			GuidNavDat2(0).ValMax(0) = fCurrDir
			GuidNavDat2(0).ValCnt = 7

			'If GNav2.TypeCode > eNavaidType.CodeNONE Then
			If (Not ptCOP Is Nothing) And (iCopSide > 0) Then
				Side2 = SideDef(GNav2.pPtPrj, fCurrDir + 90.0, GNav1.pPtPrj)
				If Side2 < 0 Then
					J = 1

					GuidNavDat2(1) = GNav2
					ComboBox201.Items.Add(GNav2.CallSign)
					ReDim GuidNavDat2(1).ValMin(0)
					ReDim GuidNavDat2(1).ValMax(0)
					GuidNavDat2(1).ValMin(0) = fCurrDir
					GuidNavDat2(1).ValMax(0) = fCurrDir
					GuidNavDat2(1).ValCnt = 7
				End If
			End If

			ReDim Preserve GuidNavDat2(J)
			ComboBox201.SelectedIndex = 0
			Return 1
		End If

		StartFIX = Segments(SegmentNum).StartFIX
		ptD0 = Segments(SegmentNum).ptD0
		dReserve = ReturnDistanceInMeters(ptD0, StartFIX.pPtPrj)

		If GNav1.TypeCode = eNavaidType.VOR Then
			TrackToler = VOR.EnRouteTrackingToler
			fConRad = (hRoute - GNav1.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle)) * System.Math.Sin(DegToRad(2 * VOR.TrackAccuracy))
		Else 'If GNav1.TypeCode = eNavaidType.CodeNDB Then
			TrackToler = NDB.EnRouteTrackingToler
			fConRad = (hRoute - GNav1.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle)) * System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + NDB.TrackAccuracy))
		End If
		TrackRange = GNav1.Range / System.Math.Cos(DegToRad(TrackToler))

		pNav1TrackArea = New ArcGeometry.Polygon
		pNav1TrackArea.AddPoint(GNav1.pPtPrj)
		pNav1TrackArea.AddPoint(PointAlongPlane(GNav1.pPtPrj, fCurrDir - TrackToler, 100.0 * TrackRange))
		pNav1TrackArea.AddPoint(PointAlongPlane(GNav1.pPtPrj, fCurrDir + TrackToler, 100.0 * TrackRange))
		pNav1TrackArea.AddPoint(GNav1.pPtPrj)
		pNav1TrackArea.AddPoint(PointAlongPlane(GNav1.pPtPrj, fCurrDir - TrackToler + 180.0, 100.0 * TrackRange))
		pNav1TrackArea.AddPoint(PointAlongPlane(GNav1.pPtPrj, fCurrDir + TrackToler + 180.0, 100.0 * TrackRange))
		pNav1TrackArea.AddPoint(GNav1.pPtPrj)

		pTopoOper = pNav1TrackArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
		'DrawPolygon(pNav1TrackArea, , Display.esriSimpleFillStyle.esriSFSCross)
		'================================================================================
		fRange = pTrackLine1.Length

		N = UBound(NavaidList)
		If SideDef(pTrackLine1.FromPoint, fCurrDir + 90.0, pTrackLine1.ToPoint) < 0 Then N = -1

		ReDim GuidNavDat2(N)

		'pProxi1 = pNav1TrackArea
		pProxi2 = pTrackLine1
		J = -1

		For I = 0 To N
			If pProxi2.ReturnDistance(NavaidList(I).pPtPrj) > NavaidList(I).Range Then Continue For

			'While True
			'	Application.DoEvents()
			'End While

			If NavaidList(I).TypeCode = eNavaidType.VOR Then
				'fInterToler = VOR.EnRouteInterToler
				fConRad = (hRoute - NavaidList(I).pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle)) * System.Math.Sin(DegToRad(2 * VOR.TrackAccuracy))
			Else
				'fInterToler = NDB.EnRouteInterToler
				fConRad = (hRoute - NavaidList(I).pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle)) * System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + NDB.TrackAccuracy))
			End If

			If NavaidList(I).Identifier = GNav2.Identifier Then
				bFlag = (SideDef(StartFIX.pPtPrj, fCurrDir + 90.0, GNav2.pPtPrj) > 0) And (SideDef(ptD0, fCurrDir + 90.0, GNav2.pPtPrj) > 0)
			ElseIf NavaidList(I).Identifier = GNav1.Identifier Then
				bFlag = (SideDef(StartFIX.pPtPrj, fCurrDir + 90.0, GNav1.pPtPrj) > 0) And (SideDef(ptD0, fCurrDir + 90.0, GNav1.pPtPrj) > 0)
			Else
				bFlag = (Point2LineDistancePrj(NavaidList(I).pPtPrj, StartFIX.pPtPrj, fCurrDir) < fConRad) And (SideDef(ptD0, fCurrDir + 90.0, NavaidList(I).pPtPrj) > 0) 'And (SideDef(FIX_WPT.ptPrj, fCurrDir + 90.0, NavaidList(I).ptPrj) > 0) 
			End If

			If bFlag Then
				fD0 = Point2LineDistancePrj(NavaidList(I).pPtPrj, ptD0, fCurrDir + 90.0)

				CurrMaxAngle = 2.0 * RadToDegValue * Math.Atan2(fD0, fTurnR)
				If CurrMaxAngle > MaxTurnAngle.Value Then CurrMaxAngle = MaxTurnAngle.Value

				J = J + 1
				GuidNavDat2(J) = NavaidList(I)
				GuidNavDat2(J).ValCnt = 7

				ReDim GuidNavDat2(J).ValMin(1)
				ReDim GuidNavDat2(J).ValMax(1)
				GuidNavDat2(J).ValMin(0) = fCurrDir
				GuidNavDat2(J).ValMax(0) = fCurrDir + CurrMaxAngle

				GuidNavDat2(J).ValMin(1) = fCurrDir - CurrMaxAngle
				GuidNavDat2(J).ValMax(1) = fCurrDir
			Else 'If pProxi1.ReturnDistance(NavaidList(I).ptPrj) > 2.0 Then 'And (pProxi2.ReturnDistance(NavaidList(I).ptPrj) > fConRad)fConRad
				NavRightInt = CalcNavInterval(ptD0, fCurrDir, fRange, dReserve, fTurnR, NavaidList(I), -1)
				NavLeftInt = CalcNavInterval(ptD0, fCurrDir, fRange, dReserve, fTurnR, NavaidList(I), 1)

				C = NavRightInt.Tag + NavLeftInt.Tag
				If C = 0 Then Continue For

				J = J + 1
				GuidNavDat2(J) = NavaidList(I)

				ReDim GuidNavDat2(J).ValMin(C - 1)
				ReDim GuidNavDat2(J).ValMax(C - 1)

				M = 0
				If NavRightInt.Tag > 0 Then
					GuidNavDat2(J).ValMin(M) = NavRightInt.Low
					GuidNavDat2(J).ValMax(M) = NavRightInt.High
					GuidNavDat2(J).ValCnt = 0
					M += 1
				End If

				If NavLeftInt.Tag > 0 Then
					GuidNavDat2(J).ValMin(M) = NavLeftInt.Low
					GuidNavDat2(J).ValMax(M) = NavLeftInt.High
					If M = 0 Then GuidNavDat2(J).ValCnt = 1
				End If

				'If C > 0 Then
				'	J = J + 1
				'	GuidNavDat2(J).pPtGeo = NavaidList(I).pPtGeo
				'	GuidNavDat2(J).pPtPrj = NavaidList(I).pPtPrj

				'	GuidNavDat2(J).Name = NavaidList(I).Name
				'	GuidNavDat2(J).CallSign = NavaidList(I).CallSign

				'	GuidNavDat2(J).Identifier = NavaidList(I).Identifier
				'	GuidNavDat2(J).Range = NavaidList(I).Range
				'	GuidNavDat2(J).Index = NavaidList(I).Index

				'	GuidNavDat2(J).TypeCode = NavaidList(I).TypeCode
				'End If
			End If
		Next I

		If J > -1 Then
			ReDim Preserve GuidNavDat2(J)
			For I = 0 To J
				ComboBox201.Items.Add(GuidNavDat2(I).CallSign)
			Next I
			ComboBox201.SelectedIndex = 0
		Else
			ReDim GuidNavDat2(-1)
		End If

		Return J
	End Function

	Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
		Me.Close()
	End Sub

	Private Sub ComboBox001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
		Dim I As Integer
		Dim fTmp As Double

		I = ComboBox001.SelectedIndex

		If ComboBox101.SelectedIndex = 0 Then
			hRoute = 0.3048 * (1000.0 * I + 4000)
		Else
			hRoute = 0.3048 * (1000.0 * I + 18000)
		End If

		If HeightUnit = 0 Then
			hRoute = 50.0 * System.Math.Round(0.02 * hRoute)
			TextBox001.Text = CStr(hRoute)
		Else
			fTmp = ConvertHeight(hRoute, 2)
			fTmp = 100.0 * System.Math.Round(0.01 * fTmp)
			TextBox001.Text = CStr(fTmp)
		End If

		fTAS = IAS2TAS(TurnIAS.Value / TurnIAS.Multiplier, hRoute, erISA.Value)
		fTurnR = Bank2Radius(BankAngle.Value, fTAS)
	End Sub

	Private Sub ComboBox101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox101.SelectedIndexChanged
		Dim flNames() As String

		ComboBox001.Items.Clear()
		If ComboBox101.SelectedIndex = 0 Then
			flNames = New String() {"FL40", "FL50", "FL60", "FL70", "FL80", "FL90", "FL100", "FL110", "FL120", "FL130", "FL140", "FL150", "FL160", "FL170"}
		Else
			flNames = New String() {"FL180", "FL190", "FL200", "FL210", "FL220", "FL230", "FL240", "FL250", "FL260", "FL270", "FL280", "FL290", "FL300", "FL310", "FL320", "FL330", "FL340", "FL350", "FL360", "FL370", "FL380", "FL390", "FL400", "FL410", "FL420", "FL430", "FL440", "FL450"}
		End If

		ComboBox001.Items.AddRange(flNames)
		ComboBox001.SelectedIndex = 0
	End Sub

	Private Sub ComboBox104T_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox104T.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			ComboBox104T_Validating(ComboBox104T, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (ComboBox104T.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub ComboBox104T_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles ComboBox104T.Validating
		ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
	End Sub

	Private Sub ComboBox105_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox105.SelectedIndexChanged
		Dim bBool As Boolean

		Dim I As Integer
		Dim N As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer

		Dim fDist As Double
		'Dim fRCone1 As Double
		'Dim fRCone2 As Double

		Dim DirWPT As TypeDefinitions.NavaidType = New TypeDefinitions.NavaidType()
		Dim StWPT As TypeDefinitions.NavaidType
		Dim GNav As TypeDefinitions.NavaidType

		If ComboBox105.SelectedIndex < 0 Then Return

		GNav = GuidNavDat1(ComboBox102.SelectedIndex)

		On Error Resume Next
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		If Not FixPointElement1 Is Nothing Then pGraphics.DeleteElement(FixPointElement1)
		If Not FixElement1 Is Nothing Then pGraphics.DeleteElement(FixElement1)

		LineElement1 = Nothing
		FixPointElement1 = Nothing
		FixElement1 = Nothing
		On Error GoTo 0

		If ComboBox105.SelectedIndex > UBound(StartWPT) Then
			Label108.Visible = True
			Label109.Visible = True
			Label110.Visible = True
			Label111.Visible = True
			Label112.Visible = True
			Label114.Visible = True

			Frame102.Visible = True
			'OptionButton103.Visible = True
			'OptionButton104.Visible = True
			OptionButton105.Visible = True
			OptionButton106.Visible = True
			ComboBox106.Visible = True

			TextBox102.Visible = True
			TextBox103.Visible = True

			If OptionButton103.Checked Then OptionButton103_CheckedChanged(OptionButton103, New System.EventArgs())
			If OptionButton104.Checked Then OptionButton104_CheckedChanged(OptionButton104, New System.EventArgs())
		Else
			Label108.Visible = False
			Label109.Visible = False
			Label111.Visible = False
			Label114.Visible = False

			Frame102.Visible = False
			'OptionButton103.Visible = False
			'OptionButton104.Visible = False
			OptionButton105.Visible = False
			OptionButton106.Visible = False

			TextBox102.Visible = False
			TextBox103.Visible = False

			StWPT = StartWPT(ComboBox105.SelectedIndex)

			If OptionButton101.Checked Then
				DirWPT = DirWPT1(ComboBox103.SelectedIndex)
				fDist = ReturnDistanceInMeters(GNav.pPtPrj, DirWPT.pPtPrj)
				If fDist < fMinCOPDist Then DirWPT.TypeCode = -1
			Else
				DirWPT.TypeCode = -1
			End If

			I = ComboBox106.SelectedIndex

			ComboBox106.Items.Clear()
			ComboBox106_SelectedIndexChanged(ComboBox106, New System.EventArgs())

			pTrackLine1.FromPoint = StWPT.pPtPrj
			pCurrSegLine.FromPoint = StWPT.pPtPrj

			'If GNav.TypeCode = eNavaidType.CodeVOR Then
			'	fRCone1 = (hRoute - GNav.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
			'Else
			'	fRCone1 = (hRoute - GNav.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
			'End If

			Side1 = SideDef(StWPT.pPtPrj, fCurrDir + 90.0, GNav.pPtPrj)

			bBool = (DirWPT.TypeCode >= eNavaidType.VOR) And TextBox104.Visible
			If bBool Then bBool = SideDef(GNav.pPtPrj, fCurrDir + 90.0, DirWPT.pPtPrj) > 0

			If bBool Then
				Side2 = SideDef(StWPT.pPtPrj, fCurrDir + 90.0, DirWPT.pPtPrj)
				'If DirWPT.TypeCode = eNavaidType.CodeVOR Then
				'	fRCone2 = (hRoute - DirWPT.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
				'Else
				'	fRCone2 = (hRoute - DirWPT.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
				'End If

				If Side1 > 0 Then
					pCurrSegLine.ToPoint = GNav.pPtPrj
					pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, 0.5) 'GNav.ptPrj 'PointAlongPlane(GNav.ptPrj, fCurrDir + 180.0, fRCone1)
				ElseIf (Side1 < 0) Or (Side2 > 0) Then
					pCurrSegLine.ToPoint = Segments(SegmentNum).ptCOP
					pTrackLine1.ToPoint = PointAlongPlane(DirWPT.pPtPrj, fCurrDir, 0.5) 'DWPT.ptPrj 'PointAlongPlane(DWPT.ptPrj, fCurrDir + 180.0, fRCone2)
				Else
					pCurrSegLine.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
					pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
				End If
			Else
				If Side1 > 0 Then
					pCurrSegLine.ToPoint = GNav.pPtPrj
					pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, 0.5) 'GNav.ptPrj 'PointAlongPlane(GNav.ptPrj, fCurrDir + 180.0, fRCone1)
				Else
					pCurrSegLine.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
					pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
				End If
			End If

			If StWPT.TypeCode >= eNavaidType.VOR Then
				Label110.Visible = False
				Label112.Visible = False
				ComboBox106.Visible = False
				If I < 0 Then ComboBox106_SelectedIndexChanged(ComboBox106, New System.EventArgs())
			Else
				Label110.Visible = True
				Label112.Visible = True
				ComboBox106.Visible = True

				InterNavDat1 = GetValidEnRouteInterNavs(StWPT.pPtPrj, fCurrDir, hRoute, GNav, 20000.0)

				N = UBound(InterNavDat1)

				If N >= 0 Then
					For I = 0 To N
						ComboBox106.Items.Add(InterNavDat1(I).CallSign)
					Next I
					ComboBox106.SelectedIndex = 0
				End If
			End If

			Label108.Visible = False
			Label109.Visible = False
			Label111.Visible = False
			TextBox102.Visible = False
			TextBox103.Visible = False

			LineElement1 = DrawPolyLineSFS(pCurrSegLine, pLineSym)
		End If
	End Sub

	Private Sub ComboBox106_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox106.SelectedIndexChanged
		'Dim I As Integer
		Dim N As Integer
		Dim K As Integer

		Dim fRCone As Double
		Dim fDistGuid As Double
		Dim fDistInter As Double

		Dim tmpStr As String

		Dim StWPT As TypeDefinitions.NavaidType
		Dim pClone As esriSystem.IClone
		Dim GNav As TypeDefinitions.NavaidType
		Dim InterNav As TypeDefinitions.NavaidType

		On Error Resume Next
		If Not FixElement1 Is Nothing Then pGraphics.DeleteElement(FixElement1)
		On Error GoTo 0

		K = ComboBox106.SelectedIndex

		If K >= 0 Then Label112.Text = GetNavTypeName(InterNavDat1(K).TypeCode)

		GNav = GuidNavDat1(ComboBox102.SelectedIndex)

		If ComboBox105.SelectedIndex > UBound(StartWPT) Then
			If K < 0 Then Exit Sub
			InterNav = InterNavDat1(K)
			N = UBound(InterNav.ValMin)

			OptionButton105.Enabled = N > 0
			OptionButton106.Enabled = N > 0

			OptionButton105.Visible = InterNav.TypeCode = eNavaidType.DME
			OptionButton106.Visible = InterNav.TypeCode = eNavaidType.DME

			If InterNav.TypeCode = eNavaidType.VOR Then
				Label108.Text = My.Resources.str0102
				Label111.Text = "°"
				TextBox102.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))))
				tmpStr = My.Resources.str0100 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0)))) + My.Resources.str0101 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))))
			ElseIf InterNav.TypeCode = eNavaidType.DME Then
				Label108.Text = My.Resources.str0104
				Label111.Text = DistanceConverter(DistanceUnit).Unit

				If N = 0 Then
					tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(0), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(0), 2))
					TextBox102.Text = CStr(ConvertDistance(InterNav.ValMin(0), 2))
					If InterNav.ValCnt > 0 Then
						OptionButton105.Checked = True
					Else
						OptionButton106.Checked = True
					End If
				Else
					If OptionButton105.Checked Then
						TextBox102.Text = CStr(ConvertDistance(InterNav.ValMin(0), 2))
						tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(0), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(0), 2))
					Else
						TextBox102.Text = CStr(ConvertDistance(InterNav.ValMin(1), 2))
						tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(1), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(1), 2))
					End If
				End If
			ElseIf InterNav.TypeCode = eNavaidType.NDB Then
				Label108.Text = My.Resources.str0103
				Label111.Text = "°"
				TextBox102.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0) + 180.0)))
				tmpStr = My.Resources.str0100 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0) + 180.0))) + My.Resources.str0101 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0) + 180.0)))
			Else
				Label108.Text = ""
				Label111.Text = ""
				tmpStr = ""
			End If
			Label114.Text = tmpStr

			TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs(True))

			StWPT = Segments(0).StartFIX
		Else
			StWPT = StartWPT(ComboBox105.SelectedIndex)
			InterNav.Index = -1

			If StWPT.TypeCode >= eNavaidType.VOR Then
				If StWPT.TypeCode = eNavaidType.VOR Then
					fRCone = (hRoute - StWPT.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
				Else
					fRCone = (hRoute - StWPT.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
				End If
				PrimFIXPoly = CreatePrjCircle(StWPT.pPtPrj, fRCone)

				InterNav = GNav
			Else
				If K < 0 Then Return

				InterNav = InterNavDat1(K)
				Label112.Text = GetNavTypeName(InterNav.TypeCode)
				PrimFIXPoly = CreateFixArea(GNav, InterNav, StWPT)
			End If

			CalcTolerance(StWPT.pPtPrj, PrimFIXPoly, fCurrDir, fNearTol, fFarTol)

			Segments(0).ptD0 = PointAlongPlane(StWPT.pPtPrj, fCurrDir, fFarTol)

			pTrackLine1.FromPoint = Segments(0).ptD0

			pClone = PrimFIXPoly
			Segments(0).pStartFixPoly = pClone.Clone

			Segments(0).StartFIX = StWPT
			Segments(0).StartNav = GNav
			Segments(0).StartInter = InterNav
			Segments(0).fStartNearTol = fNearTol
			Segments(0).fStartFarTol = fFarTol
			'===========================================================================

			FixElement1 = DrawPolygon(PrimFIXPoly, RGB(255, 0, 255))
			Segments(0).pStartFixElement = FixElement1

			fDistInter = ReturnDistanceInMeters(StWPT.pPtPrj, InterNav.pPtPrj)
			fDistGuid = ReturnDistanceInMeters(StWPT.pPtPrj, GNav.pPtPrj)

			hRouteInter = (fDistInter / 4130.0) * (fDistInter / 4130.0) + InterNav.pPtPrj.Z
			hRouteGuid = (fDistGuid / 4130.0) * (fDistGuid / 4130.0) + GNav.pPtPrj.Z

			Segments(0).fHGuidS = hRouteGuid
			Segments(0).fHInterS = hRouteInter

			InfoFrm.SetInterNav(InterNav)
			InfoFrm.SetNearTol(fNearTol, True)

			InfoFrm.SetGuidNavReqH(hRouteGuid)
			InfoFrm.SetInterNavReqH(hRouteInter)
		End If
	End Sub

	Private Sub ComboBox201_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox201.SelectedIndexChanged
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		Dim GNav As TypeDefinitions.NavaidType
		Dim tmpStr As String

		'Dim pGeomEnv As IGeometryEnvironment3
		'pGeomEnv = New GeometryEnvironment
		'pGeomEnv.UseAlternativeTopoOps = False

		AddBtn.Enabled = True

		K = ComboBox201.SelectedIndex
		If K < 0 Then Return

		GNav = GuidNavDat2(K)
		Label202.Text = GetNavTypeName(GNav.TypeCode)

		If OptionButton201.Checked Then
			I = ComboBox207.SelectedIndex
			If I < 0 Then I = 0

			N = UBound(GNav.ValMax)
			If N = 1 Then
				Label217.Enabled = True
				ComboBox207.Enabled = True
			Else
				Label217.Enabled = False
				ComboBox207.Enabled = False
				I = 1 - GNav.ValCnt
			End If

			If I = ComboBox207.SelectedIndex Then
				ComboBox207_SelectedIndexChanged(Nothing, Nothing)
			Else
				ComboBox207.SelectedIndex = I
			End If

			Return
		End If

		If GNav.TypeCode = eNavaidType.VOR Then
			fMinCOPDist = VOR.EnRoutePrimAreaWith / System.Math.Tan(DegToRad(VOR.EnRouteTracking2Toler))
		Else
			fMinCOPDist = NDB.EnRoutePrimAreaWith / System.Math.Tan(DegToRad(NDB.EnRouteTracking2Toler))
		End If

		'=======================================================================================
		TextBox206.Text = System.Math.Round(Dir2Azt(pTrackLine1.FromPoint, fCurrDir), 1).ToString(".0")
		tmpStr = My.Resources.str2003 + ": " + TextBox206.Text

		InfoFrm.SetGuidNavMsg(tmpStr)
		ToolTip1.SetToolTip(TextBox206, tmpStr)
		'=======================================================================================

		bText2 = False
		ComboBox202.Items.Clear()

		ReDim DirWPT2(0)

		If Segments(SegmentNum).StartNav.Index <> GNav.Index Then
			DirWPT2(0) = Segments(SegmentNum).StartNav
		Else
			If (Segments(SegmentNum).DirWPT.Index >= 0) Then
				DirWPT2(0) = Segments(SegmentNum).DirWPT
			Else
				DirWPT2(0) = Segments(SegmentNum).StartFIX
			End If
		End If

		ComboBox202.Items.Add(DirWPT2(0).CallSign)
		ComboBox202.Enabled = True
		OptionButton203.Enabled = True

		If OptionButton203.Checked Then
			ComboBox202.SelectedIndex = 0
		Else
			OptionButton204_CheckedChanged(OptionButton204, New System.EventArgs())
		End If
	End Sub

	Private Sub ComboBox202_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox202.SelectedIndexChanged
		Dim K As Integer
		Dim fDir As Double
		Dim GdNav2 As NavaidType
		Dim DirWPT As NavaidType

		K = ComboBox202.SelectedIndex
		If K < 0 Then Return

		DirWPT = DirWPT2(K)
		Label203.Text = GetNavTypeName(DirWPT.TypeCode)

		If OptionButton201.Checked Then
			GdNav2 = GuidNavDat2(ComboBox201.SelectedIndex)
			K = SideDef(DirWPT.pPtPrj, fCurrDir, GdNav2.pPtPrj)
			If K = 2 * ComboBox207.SelectedIndex - 1 Then
				fDir = ReturnAngleInDegrees(DirWPT.pPtPrj, GdNav2.pPtPrj)
			Else
				fDir = ReturnAngleInDegrees(GdNav2.pPtPrj, DirWPT.pPtPrj)
			End If

			TextBox206.Text = CStr(System.Math.Round(Dir2Azt(GdNav2.pPtPrj, fDir), 1))
		End If

		TextBox206.Tag = ""
		TextBox206_Validating(TextBox206, Nothing)
	End Sub

	Private Sub CheckBox201_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox201.CheckedChanged
		If Not bFormInitialised Then Return
		TextBox206.Tag = "A"
		TextBox206_Validating(Nothing, Nothing)
	End Sub

	Private Sub TextBox206_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox206.KeyPress
		If TextBox206.ReadOnly Then Return

		Dim KeyAscii As Char = eventArgs.KeyChar

		If KeyAscii = Chr(13) Then
			TextBox206_Validating(TextBox206, Nothing)
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (TextBox206.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox206_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox206.Validating
		Dim bVisible As Boolean
		Dim bCEnable As Boolean
		'Dim bSameNav As Boolean
		'Dim bFIXFlg As Boolean
		Dim bCheck As Boolean
		Dim bText As Boolean

		Dim NewFIXIndx As Integer
		Dim ValIndex As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer

		Dim fRadius1 As Double
		Dim fRadius2 As Double
		'Dim FullDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim fDist As Double
		'Dim fTmp2 As Double
		Dim fTmp As Double

		Dim GNav2 As TypeDefinitions.NavaidType = New NavaidType()
		Dim GNav As TypeDefinitions.NavaidType
		Dim pWPT00 As TypeDefinitions.NavaidType
		Dim DWPT As TypeDefinitions.NavaidType

		Dim pGeomCollect As ArcGeometry.IGeometryCollection
		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pConstruct As ArcGeometry.IConstructPoint
		'Dim pTmpPolyLine As ArcGeometry.IPolyline
		Dim pPrimPoly As ArcGeometry.IPolygon
		Dim pSecPoly As ArcGeometry.IPolygon
		Dim FullSeg1 As ArcGeometry.Polygon = Nothing
		Dim PrimSeg1 As ArcGeometry.Polygon = Nothing
		Dim FullSeg As ArcGeometry.Polygon = Nothing
		Dim PrimSeg As ArcGeometry.Polygon = Nothing
		Dim pClone As esriSystem.IClone
		Dim pLine As ArcGeometry.ILine
		Dim pPath As ArcGeometry.IPath
		Dim pt1 As ArcGeometry.IPoint
		'Dim pt2 As ArcGeometry.IPoint

		If Not IsNumeric(TextBox206.Text) Then Return
		If ComboBox201.SelectedIndex < 0 Then Return
		If TextBox206.Text = TextBox206.Tag Then Return

		GNav = GuidNavDat2(ComboBox201.SelectedIndex)

		If OptionButton203.Checked Then
			'If ComboBox203.Tag = ComboBox203.Text Then Exit Sub
			If ComboBox202.SelectedIndex < 0 Then Return

			DWPT = DirWPT2(ComboBox202.SelectedIndex)
			fDist1 = ReturnDistanceInMeters(GNav.pPtPrj, DWPT.pPtPrj)
			'FullDist = fDist1
			'If fDist1 < fMinCOPDist Then DWPT.TypeCode = -1
		Else
			If TextBox206.Tag = TextBox206.Text Then Return

			DWPT = New TypeDefinitions.NavaidType
			DWPT.Index = -1
			DWPT.TypeCode = eNavaidType.NONE
			DWPT.Identifier = Guid.Empty
		End If

		'    fTmp = ReturnDistanceInMeters(GNav.ptPrj, Segments(SegmentNum).StartNav.ptPrj)

		NewFIXIndx = -1 'Создать новый FIX

		ValIndex = ComboBox207.SelectedIndex
		If ValIndex < 0 Then Return
		If UBound(GNav.ValMin) = 0 Then
			ValIndex = 0
		ElseIf GNav.ValCnt < 2 Then
			ValIndex = IIf(GNav.ValCnt = ValIndex, 1, 0)
		End If

		'If (Segments(SegmentNum).StartNav.Index = GNav.Index) And (Segments(SegmentNum).StartNav.TypeCode = GNav.TypeCode) And OptionButton201.Value Then
		If (Segments(SegmentNum).StartNav.Identifier = GNav.Identifier) Then
			'bSameNav = True
			NewFIXIndx = 0 'Наводящие совпадают
			NewFIX = GNav

			'pClone = GNav.ptPrj
			'NewFIX.ptPrj = pClone.Clone
		ElseIf (Segments(SegmentNum).StartNav.Identifier = DWPT.Identifier) Then
			NewFIXIndx = 1 'I наводящий совпадает с II направляющим
			NewFIX = DWPT 'GNav

			'pClone = DWPT.ptPrj
			'NewFIX.ptPrj = pClone.Clone
		ElseIf (Segments(SegmentNum).DirWPT.Identifier = GNav.Identifier) And (Segments(SegmentNum).DirWPT.Identifier <> Guid.Empty) Then
			NewFIXIndx = 2 'I направляющий совпадает с II наводящим
			NewFIX = GNav

			'pClone = GNav.ptPrj
			'NewFIX.ptPrj = pClone.Clone
		ElseIf (Segments(SegmentNum).DirWPT.Identifier = DWPT.Identifier) And (Segments(SegmentNum).DirWPT.Identifier <> Guid.Empty) Then
			NewFIXIndx = 3 'Направляющие совпадают
			NewFIX = DWPT 'GNav

			'pClone = DWPT.ptPrj
			'NewFIX.ptPrj = pClone.Clone
		End If

		If GNav.TypeCode = eNavaidType.VOR Then
			fRadius1 = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		Else
			fRadius1 = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		End If

		'If OptionButton202.Value Then bSameNav = False

		fTmp = Azt2Dir(GNav.pPtGeo, CDbl(TextBox206.Text))

		pGuidPolyLine2 = New ArcGeometry.Polyline
		pGeomCollect = pGuidPolyLine2
		pPath = New ArcGeometry.Path

		Segments(SegmentNum + 1).ptCOP = Nothing
		GNav2.TypeCode = -1

		If OptionButton203.Checked Then
			If OptionButton201.Checked Then
				fNextDir = ReturnAngleInDegrees(GNav.pPtPrj, DWPT.pPtPrj)
				If SubtractAngles(fNextDir, fTmp) > 5 Then fNextDir = Modulus(fNextDir + 180.0)
			End If

			'If Not (bSameNav Or OptionButton202.Value) Then
			If NewFIXIndx < 0 Then
				NewFIX.pPtPrj = New ArcGeometry.Point()
				pConstruct = NewFIX.pPtPrj
				pConstruct.ConstructAngleIntersection(Segments(SegmentNum).StartNav.pPtPrj, DegToRad(fCurrDir), GNav.pPtPrj, DegToRad(fNextDir))
				NewFIX.TypeCode = eNavaidType.NONE
				NewFIX.Index = -1
				NewFIX.Identifier = Guid.Empty
				NewFIX.pPtPrj.Z = hRoute

				TextBox204.Text = NewFixName
				TextBox204.ReadOnly = False
				TextBox204.BackColor = SystemColors.Window
			Else
				TextBox204.Text = NewFIX.CallSign
				TextBox204.ReadOnly = True
				TextBox204.BackColor = SystemColors.ButtonFace
			End If

			Side2 = SideDef(GNav.pPtPrj, fNextDir + 90.0, NewFIX.pPtPrj)
			If Side2 = 0 Then Side2 = SideDef(GNav.pPtPrj, fNextDir + 90.0, DWPT.pPtPrj)

			If DWPT.TypeCode >= eNavaidType.VOR Then
				GNav2 = DWPT

				If DWPT.TypeCode = eNavaidType.VOR Then
					fRadius2 = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hRoute - DWPT.pPtGeo.Z)
				ElseIf DWPT.TypeCode = eNavaidType.NDB Then
					fRadius2 = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hRoute - DWPT.pPtGeo.Z)
				Else
					fRadius2 = 0.0
				End If

				If bText2 Then
					fDist1 = DeConvertDistance(CDbl(TextBox202.Text))
					fDist2 = DeConvertDistance(CDbl(TextBox203.Text))
				Else
					fDist1 = -1
					fDist2 = -1
				End If

				bCEnable = CheckBox201.Enabled
				bCheck = CheckBox201.Checked

				Create2NavArea(GNav, fDist1, GNav2, fDist2, bText, bVisible, bCheck, bCEnable, FullSeg1, PrimSeg1)

				TextBox202.Enabled = bCEnable
				TextBox202.Visible = bVisible And OptionButton201.Checked

				TextBox203.Enabled = bCEnable
				TextBox203.Visible = bVisible And OptionButton201.Checked

				Image201.Enabled = bCEnable
				Image201.Visible = bVisible And OptionButton201.Checked

				CheckBox201.Enabled = bCEnable
				CheckBox201.Checked = bCheck
				CheckBox201.Visible = bVisible And OptionButton201.Checked

				Label209.Visible = bVisible And OptionButton201.Checked

				Side1 = SideDef(GNav.pPtPrj, fNextDir + 90.0, GNav2.pPtPrj)

				If bVisible And OptionButton201.Checked Then
					TextBox202.Text = CStr(ConvertDistance(fDist1, 2))
					TextBox203.Text = CStr(ConvertDistance(fDist2, 2))
					TextBox202.Tag = TextBox202.Text
					TextBox203.Tag = TextBox203.Text

					Segments(SegmentNum + 1).fCopDist = fDist1

					If Side1 > 0 Then
						Segments(SegmentNum + 1).ptCOP = PointAlongPlane(GNav.pPtPrj, fNextDir, fDist1)
					Else
						Segments(SegmentNum + 1).ptCOP = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, fDist1)
					End If
				End If

				If Side1 > 0 Then
					CreateNavArea(GNav, fNextDir - 180.0, GNav.Range, FullSeg, PrimSeg)
				Else
					CreateNavArea(GNav, fNextDir, GNav.Range, FullSeg, PrimSeg)
				End If

				pTopo = FullSeg1
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				FullSegPoly2 = pTopo.Union(FullSeg)

				pTopo = PrimSeg1
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				PrimSegPoly2 = pTopo.Union(PrimSeg)

				If Side1 > 0 Then
					CreateNavArea(GNav2, fNextDir, GNav2.Range, FullSeg, PrimSeg)
				Else
					CreateNavArea(GNav2, fNextDir + 180.0, GNav2.Range, FullSeg, PrimSeg)
				End If

				pTopo = FullSegPoly2
				pSecPoly = pTopo.Union(FullSeg)

				pTopo = PrimSegPoly2
				pPrimPoly = pTopo.Union(PrimSeg)
			Else
				CheckBox201.Visible = False
				TextBox202.Visible = False
				TextBox203.Visible = False
				Image201.Visible = False
				Label209.Visible = False

				CreateNavArea(GNav, fNextDir - 180.0, GNav.Range, FullSeg1, PrimSeg1)
				CreateNavArea(GNav, fNextDir, GNav.Range, FullSeg, PrimSeg)

				pTopo = FullSeg1
				pSecPoly = pTopo.Union(FullSeg)

				pTopo = PrimSeg1
				pPrimPoly = pTopo.Union(PrimSeg)
			End If

			fDist = ReturnDistanceInMeters(GNav.pPtPrj, DWPT.pPtPrj)
			If Side1 < 0 Then
				If DWPT.TypeCode >= eNavaidType.VOR Then
					pt1 = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, fDist1)

					pPath.FromPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir + 180.0, DWPT.Range)
					pPath.ToPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir + 180.0, fRadius2)

					pGeomCollect.AddGeometry(pPath)

					pPath = New ArcGeometry.Path
					pPath.FromPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir, fRadius2)
				Else
					pt1 = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, GNav.Range)
					pPath.FromPoint = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, GNav.Range)
				End If

				pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, fRadius1)
				pGeomCollect.AddGeometry(pPath)
				pPath = New ArcGeometry.Path
				pPath.FromPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, fRadius1)
				pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, GNav.Range)

				'pt2 = pPath.ToPoint
			Else
				pt1 = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, GNav.Range)
				pPath.FromPoint = pt1
				pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, fRadius1)

				pGeomCollect.AddGeometry(pPath)

				pPath = New ArcGeometry.Path
				pPath.FromPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, fRadius1)

				If DWPT.TypeCode >= eNavaidType.VOR Then
					'pt2 = PointAlongPlane(GNav.pPtPrj, fNextDir, fDist1)
					pPath.ToPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir + 180.0, fRadius2)

					pGeomCollect.AddGeometry(pPath)

					pPath = New ArcGeometry.Path
					pPath.FromPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir, fRadius2)
					pPath.ToPoint = PointAlongPlane(DWPT.pPtPrj, fNextDir, DWPT.Range)
				Else
					pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, GNav.Range)
					'pt2 = pPath.ToPoint
				End If
			End If
		Else
			fNextDir = fTmp

			If Not AngleInSector(fNextDir, GNav.ValMin(ValIndex), GNav.ValMax(ValIndex)) Then
				If AnglesSideDef(fNextDir, GNav.ValMin(ValIndex)) < 0 Then
					fNextDir = GNav.ValMin(ValIndex)
				Else
					fNextDir = GNav.ValMax(ValIndex)
				End If
			End If
			TextBox206.Text = CStr(System.Math.Round(Dir2Azt(GNav.pPtPrj, fNextDir), 1))

			'If Not NewFIX.ptPrj.IsEmpty Then
			'	fTmp = NewFIX.ptPrj.Z
			'Else
			'	fTmp = hRoute
			'End If

			'If Not (bSameNav Or OptionButton202.value) Then
			If NewFIXIndx < 0 Then
				NewFIX.pPtPrj = New ArcGeometry.Point
				pConstruct = NewFIX.pPtPrj
				pConstruct.ConstructAngleIntersection(Segments(SegmentNum).StartNav.pPtPrj, DegToRad(fCurrDir), GNav.pPtPrj, DegToRad(fNextDir))
				pClone = Nothing

				If NewFIX.pPtPrj.IsEmpty Then
					If NewFIXIndx = 2 Then 'I направляющий совпадает с II наводящим
						pClone = GNav.pPtPrj
					Else
						pClone = DWPT.pPtPrj
					End If

					If Not pClone Is Nothing Then NewFIX.pPtPrj = pClone.Clone
				End If

				NewFIX.TypeCode = eNavaidType.NONE
				NewFIX.Index = -1
				NewFIX.Identifier = Guid.Empty
				NewFIX.pPtPrj.Z = hRoute

				TextBox204.Text = NewFixName
				TextBox204.ReadOnly = False
				TextBox204.BackColor = SystemColors.Window

				'Else
				'	Set NewFIX.ptPrj = PointAlongPlane(Segments(SegmentNum).StartFIX.ptPrj, fNextDir, 2 * Segments(SegmentNum).fStartFarTol)
			End If

			CreateNavArea(GNav, fNextDir - 180.0, GNav.Range, FullSeg1, PrimSeg1)
			CreateNavArea(GNav, fNextDir, GNav.Range, FullSeg, PrimSeg)

			pTopo = FullSeg1
			pSecPoly = pTopo.Union(FullSeg)

			pTopo = PrimSeg1
			pPrimPoly = pTopo.Union(PrimSeg)

			pPath.FromPoint = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, GNav.Range)
			pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir + 180.0, fRadius1)

			pt1 = pPath.FromPoint
			pGeomCollect.AddGeometry(pPath)

			pPath = New ArcGeometry.Path
			pPath.FromPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, fRadius1)
			pPath.ToPoint = PointAlongPlane(GNav.pPtPrj, fNextDir, GNav.Range)

			'pt2 = pPath.ToPoint
		End If

		pGeomCollect.AddGeometry(pPath)
		'DrawPolyLine(pGeomCollect, -1, 2)
		'Application.DoEvents()
		'DrawPolygon(pSecPoly)
		'DrawPolygon(pPrimPoly)
		'Application.DoEvents()

		On Error Resume Next
		If Not FullSegElement2 Is Nothing Then pGraphics.DeleteElement(FullSegElement2)
		If Not PrimSegElement2 Is Nothing Then pGraphics.DeleteElement(PrimSegElement2)
		If Not LineElement2 Is Nothing Then pGraphics.DeleteElement(LineElement2)
		On Error GoTo 0

		Segments(SegmentNum + 1).pSecPoly = pSecPoly
		Segments(SegmentNum + 1).pPrimPoly = pPrimPoly
		Segments(SegmentNum + 1).pFullSegElement = DrawPolygon(pSecPoly, RGB(0, 128, 255))
		Segments(SegmentNum + 1).pPrimSegElement = DrawPolygon(pPrimPoly, RGB(255, 128, 0))

		FullSegElement2 = Segments(SegmentNum + 1).pFullSegElement
		PrimSegElement2 = Segments(SegmentNum + 1).pPrimSegElement

		LineElement2 = DrawPolyLine(pGuidPolyLine2, RGB(0, 0, 255), 2)
		'Application.DoEvents()
		'==
		ComboBox204.Items.Clear()

		fDist = ReturnDistanceInMeters(NewFIX.pPtPrj, Segments(SegmentNum).StartFIX.pPtPrj)
		InfoFrm.SetSegLenght(fDist, OptionButton202.Checked)
		fDist2 = ReturnDistanceInMeters(Segments(SegmentNum).ptD0, Segments(SegmentNum).StartFIX.pPtPrj)

		'DrawPointWithText(NewFIX.pPtPrj, "NewFIX")
		'DrawPointWithText(Segments(SegmentNum).StartFIX.pPtPrj, "StartFIX")
		'DrawPointWithText(Segments(SegmentNum).ptD0, "ptD0")
		'Application.DoEvents()


		fTmp = SubtractAngles(fNextDir, fCurrDir)
		InfoFrm.SetTurnAngle(fTmp)
		fTmp = fTurnR * System.Math.Tan(DegToRad(0.5 * fTmp))
		InfoFrm.SetReqStabLenght(fTmp)

		If OptionButton202.Checked Then
			pTrackLine1.FromPoint = Segments(SegmentNum).ptD0

			InterNavDat2 = GetValidEnRouteNavs(GNav, hRoute, pTrackLine1, 15000.0)
			N = UBound(InterNavDat2)

			If SideDef(GNav.pPtPrj, fCurrDir + 90.0, pTrackLine1.FromPoint) < 1 Then
				If N >= 0 Then
					ReDim Preserve InterNavDat2(N + 1)
				Else
					ReDim InterNavDat2(0)
				End If
				N += 1

				InterNavDat2(N) = GNav
			End If

			If N >= 0 Then
				For I = 0 To N
					ComboBox204.Items.Add(InterNavDat2(I).CallSign)
				Next I
				ComboBox204.SelectedIndex = 0
			End If
			'    ElseIf Not bSameNav Then
		ElseIf (NewFIXIndx < 0) Or (NewFIXIndx > 2) Then
			fTmp = fDist - fTmp - fDist2
			If fTmp < 0 Then fTmp = 0

			InterNavDat2 = GetValidEnRouteInterNavs(NewFIX.pPtPrj, fCurrDir, hRoute, Segments(SegmentNum).StartNav, Math.Min(fTmp, 15000.0))
			N = UBound(InterNavDat2)

			If (N = -1) And (SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, GNav.pPtPrj) > 0) Then
				ReDim InterNavDat2(1)
				InterNavDat2(0) = GNav
				N = 0
			End If

			If OptionButton203.Checked And (DWPT.TypeCode >= eNavaidType.VOR) Then
				If SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, GNav2.pPtPrj) > 0 Then
					If N >= 0 Then
						ReDim Preserve InterNavDat2(N + 1)
						N += 1
					Else
						ReDim InterNavDat2(1)
						N = 0
					End If

					InterNavDat2(N) = GNav2
				End If
			End If

			For I = 0 To N
				ComboBox204.Items.Add(InterNavDat2(I).CallSign)
			Next I
			If N >= 0 Then ComboBox204.SelectedIndex = 0
		Else
			N = -1
			If SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, GNav.pPtPrj) > 0 Then
				ReDim InterNavDat2(1)
				N = 0

				InterNavDat2(N) = NewFIX 'GNav
			End If

			If OptionButton203.Checked And (DWPT.TypeCode >= eNavaidType.VOR) Then
				If SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, GNav2.pPtPrj) > 0 Then
					If N >= 0 Then
						ReDim Preserve InterNavDat2(N + 1)
						N += 1
					Else
						ReDim InterNavDat2(1)
						N = 0
					End If

					InterNavDat2(N) = GNav2
				End If
			End If

			For I = 0 To N
				ComboBox204.Items.Add(InterNavDat2(I).CallSign)
			Next I
			If N >= 0 Then ComboBox204.SelectedIndex = 0
		End If
		'=========================================================
		pLine = New ArcGeometry.Line
		pLine.FromPoint = Segments(SegmentNum).StartFIX.pPtPrj
		fDist2 = ReturnDistanceInMeters(GNav.pPtPrj, pLine.FromPoint)

		If SideDef(pLine.FromPoint, fCurrDir + 90.0, GNav.pPtPrj) > 0 Then
			fDist = fDist2
		ElseIf GNav2.TypeCode >= eNavaidType.VOR Then
			If SideDef(pLine.FromPoint, fCurrDir + 90.0, GNav2.pPtPrj) > 0 Then
				fDist = fDist1
			Else
				fDist = GNav.Range - fDist2
			End If
		Else
			fDist = GNav.Range - fDist2
		End If

		'bFIXFlg = (Segments(SegmentNum).DirWPT.Identifier = GNav.Identifier) Or (Segments(SegmentNum).StartNav.Identifier = GNav.Identifier)

		'If OptionButton203.Checked Then
		'	If (Segments(SegmentNum).DirWPT.Identifier = DWPT.Identifier) Or (Segments(SegmentNum).StartNav.Identifier = DWPT.Identifier) Then
		'		bFIXFlg = True
		'	End If
		'End If

		ComboBox205.Items.Clear()

		N = UBound(WPTList)
		M = UBound(NavaidList)

		ReDim EndWPT(N + M + 1)
		J = -1

		For I = 0 To N
			If (WPTList(I).TypeCode = eNavaidType.NONE) And SideDef(Segments(SegmentNum).ptD0, fCurrDir + 90.0, WPTList(I).pPtPrj) > 0 Then
				pLine.ToPoint = WPTList(I).pPtPrj

				If (pLine.Length <= fDist) And (SubtractAngles(RadToDeg(pLine.Angle), fCurrDir) <= 1.0) Then
					J += 1
					EndWPT(J) = WPTList(I)
					ComboBox205.Items.Add(EndWPT(J).CallSign)
				End If
			End If
		Next I

		For I = 0 To M
			If SideDef(Segments(SegmentNum).ptD0, fCurrDir + 90.0, NavaidList(I).pPtPrj) > 0 Then
				pLine.ToPoint = NavaidList(I).pPtPrj

				If (pLine.Length <= fDist) And (SubtractAngles(RadToDeg(pLine.Angle), fCurrDir) <= 1.0) Then
					J += 1
					EndWPT(J) = NavaidList(I)
					ComboBox205.Items.Add(EndWPT(J).CallSign)
				End If
			End If
		Next I

		If J >= 0 Then
			ReDim Preserve EndWPT(J)
		Else
			ReDim EndWPT(-1)
		End If

		ComboBox205.Items.Add(My.Resources.str1006)
		If OptionButton202.Checked Then ComboBox205.SelectedIndex = 0

		TextBox206.Tag = TextBox206.Text
	End Sub

	Private Sub ComboBox204_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox204.SelectedIndexChanged
		Dim iCopSide As Integer
		Dim I As Integer
		Dim K As Integer
		Dim C As Integer

		Dim bHFlg As Boolean
		Dim bFlg As Boolean

		Dim fDistInter As Double
		Dim fDistGuid As Double
		Dim fZGuid2 As Double
		Dim fZGuid1 As Double
		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim fCOP1 As Double
		Dim fCOP2 As Double
		'Dim fHmin As Double
		Dim fHmax As Double
		Dim fTmp As Double

		Dim GuidNav2 As TypeDefinitions.NavaidType
		Dim InterNav As TypeDefinitions.NavaidType
		Dim GuidNav As TypeDefinitions.NavaidType
		Dim ptTmp As ArcGeometry.IPoint

		K = ComboBox204.SelectedIndex
		If K < 0 Then Return

		InterNav = InterNavDat2(K)
		Label206.Text = GetNavTypeName(InterNav.TypeCode)
		GuidNav = Segments(SegmentNum).StartNav 'GuidNavDat1(ComboBox102.ListIndex)

		bFlg = False
		bHFlg = True

		If OptionButton202.Checked And (ComboBox205.SelectedIndex >= 0) And (ComboBox205.SelectedIndex <= UBound(EndWPT)) Then
			bFlg = True
		End If

		GuidNav2 = Segments(SegmentNum).DirWPT
		'==================================================================
		If bFlg Then
			If (NewFIX.TypeCode >= eNavaidType.VOR) And (NewFIX.TypeCode <> 1) And ((NewFIX.Identifier = GuidNav.Identifier) Or (NewFIX.Identifier = GuidNav2.Identifier)) Then
				'ComboBox204.Visible = False
				'Label205.Visible = False
				'Label206.Visible = False

				Label212.Visible = False
				Label213.Visible = False
				Label214.Visible = False
				TextBox205.Visible = False

				If NewFIX.Index = GuidNav.Index Then
					InterNav = GuidNav
					'TextBox204.Text = GuidNav.Name
				ElseIf NewFIX.Index = GuidNav2.Index Then
					InterNav = GuidNav2
					'TextBox204.Text = GuidNav2.Name
				End If

				InterNav.ValCnt = -1
				ReDim InterNav.ValMin(0)
				ReDim InterNav.ValMax(0)
				InterNav.ValMin(0) = Modulus(fCurrDir - 120.0)
				InterNav.ValMax(0) = Modulus(fCurrDir + 120.0)

				fDist1 = 0.0  'ReturnDistanceInMeters(GuidNav.ptPrj, NewFIX.ptPrj)
				C = 1
			Else
				fDist1 = ReturnDistanceInMeters(GuidNav.pPtPrj, NewFIX.pPtPrj)

				'ComboBox204.Visible = True
				'Label205.Visible = True
				'Label206.Visible = True

				Label212.Visible = True
				Label213.Visible = True
				Label214.Visible = False
				TextBox205.Visible = True
				C = 0
			End If
		Else '======================================================================================================
			If OptionButton201.Checked And ((InterNav.Identifier = GuidNav.Identifier) Or ((GuidNav2.TypeCode >= eNavaidType.VOR) And (GuidNav2.TypeCode <> 1) And (InterNav.Identifier = GuidNav2.Identifier))) Then
				'If NewFIX.Index = GuidNav.Index Then
				'	NewFIX = GuidNav
				'ElseIf NewFIX.Index = GuidNav2.Index Then
				'	NewFIX = GuidNav2
				'End If

				'ComboBox204.Visible = False
				'Label205.Visible = False
				'Label206.Visible = False

				Label212.Visible = False
				Label213.Visible = False
				Label214.Visible = False
				TextBox205.Visible = False
				fDist1 = 0.0
				C = 3
			Else
				'ComboBox204.Visible = True
				'Label205.Visible = True
				'Label206.Visible = True

				Label212.Visible = True
				Label213.Visible = True
				TextBox205.Visible = True

				If OptionButton201.Checked Then
					TextBox205.ReadOnly = True
					TextBox205.BackColor = SystemColors.ButtonFace

					Label214.Visible = False
				Else
					TextBox205.ReadOnly = False
					TextBox205.BackColor = SystemColors.Window

					Label214.Visible = True
				End If

				If SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, GuidNav.pPtPrj) > 0 Then
					fDist1 = ReturnDistanceInMeters(Segments(SegmentNum).StartFIX.pPtPrj, GuidNav.pPtPrj)
				Else
					fDist1 = GuidNav.Range
					bHFlg = False
				End If
				C = 2
			End If
		End If

		'======================================================================================================
		If Not Segments(SegmentNum).ptCOP Is Nothing Then
			iCopSide = SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, Segments(SegmentNum).ptCOP)
		End If

		fDist2 = ReturnDistanceInMeters(Segments(SegmentNum).StartFIX.pPtPrj, GuidNav.pPtPrj)
		fZGuid2 = GuidNav.pPtPrj.Z
		fZGuid1 = GuidNav.pPtPrj.Z

		If Not Segments(SegmentNum).DirWPT.pPtGeo Is Nothing Then

			GuidNav2 = Segments(SegmentNum).DirWPT

			If (GuidNav2.TypeCode >= eNavaidType.VOR) And (SideDef(GuidNav.pPtPrj, fCurrDir + 90.0, GuidNav2.pPtPrj) > 0) Then
				fTmp = ReturnDistanceInMeters(NewFIX.pPtPrj, GuidNav2.pPtPrj)
				If (Not Segments(SegmentNum).ptCOP Is Nothing) And (iCopSide > 0) Then
					ptTmp = Segments(SegmentNum).ptCOP                      'PointAlongPlane(GuidNav.ptPrj, fCurrDir, fCOP1)
					fCOP1 = ReturnDistanceInMeters(GuidNav.pPtPrj, ptTmp)   'CDbl(TextBox104.Text)
					fCOP2 = ReturnDistanceInMeters(GuidNav2.pPtPrj, ptTmp)  'CDbl(TextBox105.Text)

					If SideDef(ptTmp, fCurrDir - 90, NewFIX.pPtPrj) < 0 Then
						fDist1 = fCOP1

						GuidNav = GuidNav2
						If fCOP2 > fDist1 Then
							fDist1 = fCOP2
							fZGuid1 = GuidNav.pPtPrj.Z
						End If

						If fTmp > fDist1 Then
							fDist1 = fTmp
							fZGuid1 = GuidNav.pPtPrj.Z
						End If
					End If
				Else
					If fTmp < GuidNav2.Range Then
						fDist1 = 10.0                   'GuidNav.Range
						If GuidNav.Range < GuidNav2.Range Then
							'fDist1 = GuidNav2.Range
							fZGuid1 = GuidNav2.pPtPrj.Z
						End If

						GuidNav = GuidNav2
					End If
				End If
			End If
		End If

		fDistGuid = fDist1
		If fDist2 > fDistGuid Then
			fDistGuid = fDist2
			fZGuid1 = fZGuid2
		End If

		Dim N As Integer
		Dim iSide As Integer
		Dim tmpStr As String

		OptionButton205.Visible = InterNav.TypeCode = eNavaidType.DME
		OptionButton206.Visible = InterNav.TypeCode = eNavaidType.DME

		If InterNav.TypeCode = eNavaidType.VOR Then
			Label212.Text = My.Resources.str0102
			Label213.Text = "°"
			If (C And 1) = 0 Then TextBox205.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, ReturnAngleInDegrees(InterNav.pPtPrj, NewFIX.pPtPrj))))
		ElseIf InterNav.TypeCode = eNavaidType.DME Then
			Label212.Text = My.Resources.str0104
			Label213.Text = DistanceConverter(DistanceUnit).Unit

			OptionButton205.Enabled = False
			OptionButton206.Enabled = False

			If (C And 1) = 0 Then
				TextBox205.Text = CStr(ConvertDistance(ReturnDistanceInMeters(InterNav.pPtPrj, NewFIX.pPtPrj), 2))
				iSide = SideDef(NewFIX.pPtPrj, fCurrDir + 90.0, InterNav.pPtPrj)
				If iSide > 0 Then
					OptionButton205.Checked = True
				ElseIf iSide < 0 Then
					OptionButton206.Checked = True
				Else
					OptionButton205.Checked = False
					OptionButton206.Checked = False
				End If
			End If
		ElseIf InterNav.TypeCode = eNavaidType.NDB Then
			Label212.Text = My.Resources.str0103
			Label213.Text = "°"
			If (C And 1) = 0 Then TextBox205.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, ReturnAngleInDegrees(NewFIX.pPtPrj, InterNav.pPtPrj) + 180.0)))
		Else
			Label212.Text = ""
			Label213.Text = ""
		End If

		If OptionButton202.Checked Then
			N = UBound(InterNav.ValMin)

			OptionButton205.Enabled = N > 0
			OptionButton206.Enabled = N > 0

			OptionButton205.Visible = InterNav.TypeCode = eNavaidType.DME
			OptionButton206.Visible = InterNav.TypeCode = eNavaidType.DME
			'If (InterNav.Index = GuidNav.Index) And (InterNav.TypeCode >= eNavaidType.CodeVOR) And (InterNav.TypeCode <> 1) Then
			'    TextBox204.Visible = False
			'    TextBox205.Visible = False
			'Else
			'    TextBox204.Visible = True
			'    TextBox205.Visible = True
			'End If
			tmpStr = ""
			If InterNav.TypeCode = eNavaidType.VOR Then
				If C = 2 Then TextBox205.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))))
				tmpStr = My.Resources.str0100 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0)))) + My.Resources.str0101 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))))
			ElseIf InterNav.TypeCode = eNavaidType.DME Then
				If N = 0 Then
					tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(0), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(0), 2))
					If C = 2 Then TextBox205.Text = CStr(ConvertDistance(InterNav.ValMin(0), 2))
					If InterNav.ValCnt > 0 Then
						OptionButton205.Checked = True
					Else
						OptionButton206.Checked = True
					End If
				ElseIf OptionButton205.Checked Then
					If C = 2 Then TextBox205.Text = CStr(ConvertDistance(InterNav.ValMin(0), 2))
					tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(0), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(0), 2))
				Else
					If C = 2 Then TextBox205.Text = CStr(ConvertDistance(InterNav.ValMin(1), 2))
					tmpStr = My.Resources.str0100 + CStr(ConvertDistance(InterNav.ValMin(1), 2)) + My.Resources.str0101 + CStr(ConvertDistance(InterNav.ValMax(1), 2))
				End If
			ElseIf InterNav.TypeCode = eNavaidType.NDB Then
				If C = 2 Then TextBox205.Text = CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0) + 180.0)))
				tmpStr = My.Resources.str0100 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0) + 180.0))) + My.Resources.str0101 + CStr(System.Math.Round(Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0) + 180.0)))
			End If

			Label214.Text = tmpStr
			If C = 2 Then TextBox205_Validating(TextBox205, New System.ComponentModel.CancelEventArgs(True))
		End If

		'============================================================================
		fDistInter = ReturnDistanceInMeters(NewFIX.pPtPrj, InterNav.pPtPrj)
		If Not bHFlg Then fDistGuid = ReturnDistanceInMeters(NewFIX.pPtPrj, GuidNav.pPtPrj)

		hRouteInter = (fDistInter / 4130.0) * (fDistInter / 4130.0) + InterNav.pPtPrj.Z
		hRouteGuid = (fDistGuid / 4130.0) * (fDistGuid / 4130.0) + fZGuid1

		Segments(SegmentNum).fHGuidE = hRouteGuid
		Segments(SegmentNum).fHInterE = hRouteInter

		Segments(SegmentNum + 1).fHGuidS = hRouteGuid
		Segments(SegmentNum + 1).fHInterS = hRouteInter

		If (hRoute < hRouteInter) Or (hRoute < hRouteGuid) Then
			AddBtn.Enabled = False
			InterNav.Index = -1
			InfoFrm.SetInterNav(InterNav)
			InfoFrm.SetNearTol(-1.0, OptionButton202.Checked)
			'================================================
			If ComboBox101.SelectedIndex = 0 Then
				'fHmin = 0.3048 * 4000.0
				fHmax = 0.3048 * (1000.0 * 14.0 + 4000.0)
			Else
				'fHmin = 0.3048 * 18000.0
				fHmax = 0.3048 * (1000.0 * 28.0 + 18000.0)
			End If
			'================================================
			If hRoute < hRouteGuid Then
				If hRouteGuid > fHmax Then
					MessageBox.Show(My.Resources.str0106 + CStr(ConvertHeight(hRouteGuid, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0107)
					Exit Sub
				Else
					'"Выберите другое средсто или другой эшелон полёта."
					If MessageBox.Show(My.Resources.str0106 + CStr(ConvertHeight(hRouteGuid, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0109, Nothing, MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then Return
					If ComboBox101.SelectedIndex = 0 Then
						I = System.Math.Round(0.001 * (hRouteGuid / 0.3048 - 4000.0) + 0.4999999)
					Else
						I = System.Math.Round(0.001 * (hRouteGuid / 0.3048 - 18000.0) + 0.4999999)
					End If

					If I < ComboBox001.Items.Count Then
						ComboBox001.SelectedIndex = I
					Else
						MessageBox.Show(My.Resources.str0110)
					End If
				End If
			End If

			If hRoute < hRouteInter Then
				If hRouteInter > fHmax Then
					MessageBox.Show(My.Resources.str0111 + CStr(ConvertHeight(hRouteInter, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0107)
					Exit Sub
				Else
					'"Выберите другое средсто или другой эшелон полёта."
					If MessageBox.Show(My.Resources.str0111 + CStr(ConvertHeight(hRouteInter, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0109, Nothing, MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then Return
				End If
				If ComboBox101.SelectedIndex = 0 Then
					'hRoute = 0.3048 * (1000.0 * I + 4000)
					I = System.Math.Round(0.001 * (hRouteInter / 0.3048 - 4000.0) + 0.4999999)
				Else
					'hRoute = 0.3048 * (1000.0 * I + 18000.0)
					I = System.Math.Round(0.001 * (hRouteInter / 0.3048 - 18000.0) + 0.4999999)
				End If

				If I < ComboBox001.Items.Count Then
					ComboBox001.SelectedIndex = I
				Else
					MessageBox.Show(My.Resources.str0114)
				End If
			End If
		End If

		AddBtn.Enabled = True

		On Error Resume Next
		If Not FixPointElement2 Is Nothing Then pGraphics.DeleteElement(FixPointElement2)
		If Not FixElement2 Is Nothing Then pGraphics.DeleteElement(FixElement2)
		On Error GoTo 0

		SecFIXPoly = CreateFixArea(GuidNav, InterNav, NewFIX)

		FixElement2 = DrawPolygon(SecFIXPoly, RGB(255, 0, 255))
		FixPointElement2 = DrawPoint(NewFIX.pPtPrj, 255)
		'Application.DoEvents()

		CalcTolerance(NewFIX.pPtPrj, SecFIXPoly, fCurrDir, fNearTol, fFarTol)

		InfoFrm.SetInterNav(InterNav)
		InfoFrm.SetNearTol(fNearTol, OptionButton202.Checked)

		InfoFrm.SetGuidNavReqH(hRouteGuid)
		InfoFrm.SetInterNavReqH(hRouteInter)
	End Sub

	Private Sub ComboBox102_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox102.SelectedIndexChanged
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer
		Dim K As Integer
		Dim pt1 As ArcGeometry.IPoint

		K = ComboBox102.SelectedIndex
		If K < 0 Then Exit Sub
		Label104.Text = GetNavTypeName(GuidNavDat1(K).TypeCode)

		If GuidNavDat1(K).TypeCode = eNavaidType.VOR Then
			fMinCOPDist = 0.5 * VOR.EnRoutePrimAreaWith / System.Math.Tan(DegToRad(VOR.EnRouteTracking2Toler))
		Else
			fMinCOPDist = 0.5 * NDB.EnRoutePrimAreaWith / System.Math.Tan(DegToRad(NDB.EnRouteTracking2Toler))
		End If

		bText1 = False
		If OptionButton101.Checked Then
			pt1 = GuidNavDat1(K).pPtPrj
			ComboBox103.Items.Clear()

			N = UBound(WPTList)
			M = UBound(NavaidList)

			If N + M + 1 < 0 Then
				ReDim DirWPT1(-1)
				Return
			End If

			ReDim DirWPT1(N + M + 1)
			J = -1
			For I = 0 To N
				If (WPTList(I).TypeCode = eNavaidType.NONE) And (ReturnDistanceInMeters(pt1, WPTList(I).pPtPrj) < GuidNavDat1(K).Range) Then
					J = J + 1
					DirWPT1(J) = WPTList(I)
					ComboBox103.Items.Add(DirWPT1(J).Name)
				End If
			Next I

			For I = 0 To M
				If (NavaidList(I).Index <> GuidNavDat1(K).Index) And (ReturnDistanceInMeters(pt1, NavaidList(I).pPtPrj) < NavaidList(I).Range + GuidNavDat1(K).Range - RangeReduse) Then    '+ fBuffer
					J = J + 1
					DirWPT1(J) = NavaidList(I)
					ComboBox103.Items.Add(DirWPT1(J).CallSign)
				End If
			Next I

			If J >= 0 Then
				ReDim Preserve DirWPT1(J)
				ComboBox103.SelectedIndex = 0
			Else
				ReDim DirWPT1(-1)
			End If
		Else
			ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
		End If
	End Sub

	Private Sub ComboBox103_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox103.SelectedIndexChanged
		Dim K As Integer
		Dim fDir As Double
		Dim fAzt As Double

		K = ComboBox103.SelectedIndex
		If K < 0 Then
			Label105.Text = ""
			Return
		End If

		Segments(SegmentNum).DirWPT = DirWPT1(K)
		Label105.Text = GetNavTypeName(DirWPT1(K).TypeCode)
		bText1 = False
		ComboBox104.Items.Clear()

		fDir = ReturnAngleInDegrees(GuidNavDat1(ComboBox102.SelectedIndex).pPtPrj, DirWPT1(K).pPtPrj)
		'nDirs = 2
		Dirs(0) = fDir
		Dirs(1) = fDir + 180.0

		fAzt = Dir2Azt(GuidNavDat1(ComboBox102.SelectedIndex).pPtPrj, fDir)
		ComboBox104.Items.Add(CStr(System.Math.Round(fAzt, 1)))

		fAzt = Dir2Azt(GuidNavDat1(ComboBox102.SelectedIndex).pPtPrj, fDir + 180.0)
		ComboBox104.Items.Add(CStr(System.Math.Round(fAzt, 1)))

		ComboBox104.Tag = ""
		ComboBox104.SelectedIndex = 0
	End Sub

	Private Sub ComboBox104_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox104.SelectedIndexChanged
		Dim GNav2 As TypeDefinitions.NavaidType
		Dim GNav1 As TypeDefinitions.NavaidType
		Dim DirWPT As TypeDefinitions.NavaidType

		'Dim fRadius2 As Double
		Dim FullfDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim fTmp As Double
		Dim fTmp1 As Double

		Dim Side1 As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer

		Dim bVisible As Boolean
		Dim bCEnable As Boolean
		Dim bCheck As Boolean
		Dim bText As Boolean

		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pPrimPoly As ArcGeometry.IPolygon
		Dim pSecPoly As ArcGeometry.IPolygon
		Dim PrimSeg1 As ArcGeometry.IPolygon = Nothing
		Dim FullSeg1 As ArcGeometry.IPolygon = Nothing
		Dim PrimSeg As ArcGeometry.IPolygon = Nothing
		Dim FullSeg As ArcGeometry.IPolygon = Nothing
		Dim pLine As ArcGeometry.ILine

		If OptionButton101.Checked Then
			If Not IsNumeric(ComboBox104.Text) Then Exit Sub
			If ComboBox104.Tag = ComboBox104.Text Then Exit Sub
		Else
			If Not IsNumeric(ComboBox104T.Text) Then Exit Sub
			If ComboBox104.Tag = ComboBox104T.Text Then Exit Sub
		End If

		On Error Resume Next
		If Not FullSegElement1 Is Nothing Then pGraphics.DeleteElement(FullSegElement1)
		If Not PrimSegElement1 Is Nothing Then pGraphics.DeleteElement(PrimSegElement1)
		On Error GoTo 0

		GNav1 = GuidNavDat1(ComboBox102.SelectedIndex)

		TextBox104.Visible = False
		TextBox105.Visible = False
		Image101.Visible = False
		Label113.Visible = False
		CheckBox101.Visible = False

		If OptionButton101.Checked Then
			fTmp = Dirs(ComboBox104.SelectedIndex)
		Else
			fTmp = CDbl(ComboBox104T.Text)
			fTmp = Azt2Dir(GNav1.pPtGeo, fTmp)
		End If

		Segments(SegmentNum).ptCOP = Nothing

		If OptionButton101.Checked Then
			DirWPT = DirWPT1(ComboBox103.SelectedIndex)
			fCurrDir = ReturnAngleInDegrees(GNav1.pPtPrj, DirWPT.pPtPrj)
			FullfDist = ReturnDistanceInMeters(GNav1.pPtPrj, DirWPT.pPtPrj)
			fDist1 = ReturnDistanceInMeters(GNav1.pPtPrj, DirWPT.pPtPrj)
			If fDist1 < fMinCOPDist Then DirWPT.TypeCode = -1

			If SubtractAngles(fCurrDir, fTmp) > 5 Then
				fCurrDir = Modulus(fCurrDir + 180.0, 360.0)
			End If

			Side1 = SideDef(GNav1.pPtPrj, fCurrDir + 90.0, DirWPT.pPtPrj)

			If DirWPT.TypeCode >= eNavaidType.VOR Then
				TextBox104.Enabled = True
				TextBox105.Enabled = True
				Image101.Enabled = True
				GNav2 = DirWPT

				'If DirWPT.TypeCode = eNavaidType.CodeVOR Then
				'	fRadius2 = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hRoute - DirWPT.pPtGeo.Z)
				'ElseIf DirWPT.TypeCode = eNavaidType.CodeNDB Then
				'	fRadius2 = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hRoute - DirWPT.pPtGeo.Z)
				'Else
				'	fRadius2 = 0.0
				'End If

				If bText1 Then
					fDist1 = DeConvertDistance(CDbl(TextBox104.Text))
					fDist2 = DeConvertDistance(CDbl(TextBox105.Text))
				Else
					fDist1 = -1
					fDist2 = -1
				End If

				bCEnable = CheckBox101.Enabled
				bCheck = CheckBox101.Checked

				Create2NavArea(GNav1, fDist1, GNav2, fDist2, bText, bVisible, bCheck, bCEnable, FullSeg1, PrimSeg1)

				'DrawPolygon PrimSeg1

				CheckBox101.Enabled = bCEnable
				CheckBox101.Checked = bCheck

				TextBox104.Visible = bVisible
				TextBox105.Visible = bVisible
				Image101.Visible = bVisible
				Label113.Visible = bVisible
				CheckBox101.Visible = bVisible

				If bVisible Then
					TextBox104.Text = CStr(ConvertDistance(fDist1, 2))
					TextBox105.Text = CStr(ConvertDistance(fDist2, 2))
					TextBox104.Tag = TextBox104.Text
					TextBox105.Tag = TextBox105.Text

					Segments(SegmentNum).fCopDist = fDist1

					If Side1 > 0 Then
						Segments(SegmentNum).ptCOP = PointAlongPlane(GNav1.pPtPrj, fCurrDir, fDist1)
					Else
						Segments(SegmentNum).ptCOP = PointAlongPlane(GNav1.pPtPrj, fCurrDir + 180.0, fDist1)
					End If
				End If

				If Side1 > 0 Then
					CreateNavArea(GNav1, fCurrDir - 180.0, GNav1.Range, FullSeg, PrimSeg)
				Else
					CreateNavArea(GNav1, fCurrDir, GNav1.Range, FullSeg, PrimSeg)
				End If

				pTopo = FullSeg1
				FullSegPoly1 = pTopo.Union(FullSeg)

				pTopo = PrimSeg1
				PrimSegPoly1 = pTopo.Union(PrimSeg)

				If Side1 > 0 Then
					CreateNavArea(GNav2, fCurrDir, GNav2.Range, FullSeg, PrimSeg)
					fDist2 = FullfDist - 1 'fDist1
					fDist1 = -1 'GNav.Range
				Else
					CreateNavArea(GNav2, fCurrDir + 180.0, GNav2.Range, FullSeg, PrimSeg)
					fDist1 = -FullfDist + 1 'fDist1
					fDist2 = 1 'GNav.Range
				End If

				pTopo = FullSegPoly1
				pSecPoly = pTopo.Union(FullSeg)
				If SegmentNum = 0 Then Segments(SegmentNum).pSecPoly = pSecPoly

				pTopo = PrimSegPoly1
				pPrimPoly = pTopo.Union(PrimSeg)
				If SegmentNum = 0 Then Segments(SegmentNum).pPrimPoly = pPrimPoly
			Else
				CreateNavArea(GNav1, fCurrDir - 180.0, GNav1.Range, FullSeg1, PrimSeg1)
				CreateNavArea(GNav1, fCurrDir, GNav1.Range, FullSeg, PrimSeg)

				pTopo = FullSeg1

				pSecPoly = pTopo.Union(FullSeg)
				If SegmentNum = 0 Then Segments(SegmentNum).pSecPoly = pSecPoly

				pTopo = PrimSeg1

				pPrimPoly = pTopo.Union(PrimSeg)
				If SegmentNum = 0 Then Segments(SegmentNum).pPrimPoly = pPrimPoly

				fDist1 = -GNav1.Range
				fDist2 = GNav1.Range
			End If
		Else
			fCurrDir = fTmp
			CreateNavArea(GNav1, fCurrDir - 180.0, GNav1.Range, FullSeg1, PrimSeg1)
			CreateNavArea(GNav1, fCurrDir, GNav1.Range, FullSeg, PrimSeg)

			pTopo = FullSeg1

			pSecPoly = pTopo.Union(FullSeg)
			If SegmentNum = 0 Then Segments(SegmentNum).pSecPoly = pSecPoly

			pTopo = PrimSeg1

			pPrimPoly = pTopo.Union(PrimSeg)
			If SegmentNum = 0 Then Segments(SegmentNum).pPrimPoly = pPrimPoly

			fDist1 = -GNav1.Range
			fDist2 = GNav1.Range
		End If

		ComboBox105.Items.Clear()
		N = UBound(WPTList)
		M = UBound(NavaidList)

		If N + M + 1 < 0 Then
			ReDim StartWPT(-1)
		Else
			ReDim StartWPT(N + M + 1)

			pLine = New ArcGeometry.Line
			pLine.FromPoint = GNav1.pPtPrj

			J = -1

			For I = 0 To N
				pLine.ToPoint = WPTList(I).pPtPrj
				fTmp = pLine.Length * SideDef(GNav1.pPtPrj, fCurrDir + 90.0, WPTList(I).pPtPrj)
				fTmp1 = System.Math.Sin(DegToRad(SubtractAngles(RadToDeg(pLine.Angle), fCurrDir)))
				If (fTmp > fDist1) And (fTmp < fDist2) And (fTmp1 <= 0.0087265) Then
					J = J + 1
					StartWPT(J) = WPTList(I)
					If Not StartWPT(J).CallSign Is Nothing Then
						ComboBox105.Items.Add(StartWPT(J).CallSign)
					ElseIf Not StartWPT(J).Name Is Nothing Then
						ComboBox105.Items.Add(StartWPT(J).Name)
					Else
						ComboBox105.Items.Add("")
					End If
				End If
			Next I

			For I = 0 To M
				pLine.ToPoint = NavaidList(I).pPtPrj
				fTmp = pLine.Length * SideDef(GNav1.pPtPrj, fCurrDir + 90.0, NavaidList(I).pPtPrj)
				fTmp1 = System.Math.Sin(DegToRad(SubtractAngles(RadToDeg(pLine.Angle), fCurrDir)))
				If (fTmp > fDist1) And (fTmp < fDist2) And ((fTmp1 <= 0.0087265) Or (System.Math.Abs(fTmp) < distEps)) Then
					J = J + 1
					StartWPT(J) = NavaidList(I)
					ComboBox105.Items.Add(StartWPT(J).CallSign)
				End If
			Next I

			If J >= 0 Then
				ReDim Preserve StartWPT(J)
			Else
				ReDim StartWPT(-1)
			End If
		End If

		pGuidPolyLine1 = New ArcGeometry.Polyline
		pGuidPolyLine1.FromPoint = PointAlongPlane(GNav1.pPtPrj, fCurrDir - 180.0, -fDist1)
		pGuidPolyLine1.ToPoint = PointAlongPlane(GNav1.pPtPrj, fCurrDir, fDist2)

		FullSegElement1 = DrawPolygon(pSecPoly, RGB(0, 0, 255))
		PrimSegElement1 = DrawPolygon(pPrimPoly, 255)

		If SegmentNum = 0 Then
			Segments(SegmentNum).pFullSegElement = FullSegElement1
			Segments(SegmentNum).pPrimSegElement = PrimSegElement1
		End If

		If OptionButton101.Checked Then
			ComboBox104.Tag = ComboBox104.Text
		Else
			ComboBox104.Tag = ComboBox104T.Text
		End If

		ComboBox105.Items.Add(My.Resources.str1006)
		ComboBox105.SelectedIndex = 0
	End Sub

	Private Sub CheckBox101_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox101.CheckedChanged
		If Not bFormInitialised Then Return
		ComboBox104.Tag = ""
		ComboBox104_SelectedIndexChanged(Nothing, Nothing)
	End Sub

	Private Sub ComboBox205_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox205.SelectedIndexChanged
		Dim K As Integer
		Dim N As Integer
		Dim fDist As Double
		Dim GNav1 As TypeDefinitions.NavaidType
		Dim GNav2 As TypeDefinitions.NavaidType

		GNav1 = Segments(SegmentNum).StartNav
		GNav2 = Segments(SegmentNum).DirWPT

		K = ComboBox205.SelectedIndex
		If K < 0 Then Exit Sub

		N = UBound(EndWPT)
		If K > N Then
			ComboBox204.Visible = True
			Label205.Visible = True
			Label206.Visible = True

			Label211.Visible = True
			TextBox204.Visible = True
			TextBox204.Text = NewFixName

			TextBox204.ReadOnly = False
			TextBox204.BackColor = SystemColors.Window

			TextBox205.ReadOnly = False
			TextBox205.BackColor = SystemColors.Window

			Label214.Visible = True
		Else
			NewFIX = EndWPT(K)
			fDist = Point2LineDistancePrj(GNav1.pPtPrj, NewFIX.pPtPrj, fCurrDir + 90.0)
			NewFIX.pPtPrj = PointAlongPlane(GNav1.pPtPrj, fCurrDir, fDist)
			NewFIX.pPtPrj.Z = EndWPT(K).pPtPrj.Z

			If (NewFIX.TypeCode >= eNavaidType.VOR) And (NewFIX.TypeCode <> 1) And ((NewFIX.Index = GNav1.Index) Or (NewFIX.Index = GNav2.Index)) Then
				ComboBox204.Visible = False
				Label205.Visible = False
				Label206.Visible = False
			Else
				ComboBox204.Visible = True
				Label205.Visible = True
				Label206.Visible = True
			End If

			Label211.Visible = False
			TextBox204.Visible = False
			TextBox204.Text = NewFIX.Name
			'TextBox204.ReadOnly = True
			'TextBox204.BackColor = SystemColors.ButtonFace

			Label214.Visible = False
			TextBox205.ReadOnly = True
			TextBox205.BackColor = SystemColors.ButtonFace
		End If
		ComboBox204_SelectedIndexChanged(ComboBox204, New System.EventArgs())
	End Sub

	Private Sub ComboBox207_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox207.SelectedIndexChanged
		Dim I As Integer
		Dim J As Integer
		Dim M As Integer
		Dim N As Integer
		Dim ValIndex As Integer

		Dim fDir1 As Double

		'Dim GNav1 As TypeDefinitions.NavaidType
		Dim GNav2 As TypeDefinitions.NavaidType
		Dim pProximity As ArcGeometry.IProximityOperator

		Dim tmpStr As String

		ValIndex = ComboBox207.SelectedIndex
		If ValIndex < 0 Then Return
		'TurnTo = 2 * ValIndex - 1

		If ComboBox201.SelectedIndex < 0 Then Return
		'If MultiPage001.SelectedIndex < 1 Then Return

		'GNav1 = Segments(SegmentNum).StartNav
		GNav2 = GuidNavDat2(ComboBox201.SelectedIndex)

		N = UBound(GNav2.ValMin)
		If N = 0 Then
			ValIndex = 0
		ElseIf GNav2.ValCnt < 2 Then
			ValIndex = IIf(GNav2.ValCnt = ValIndex, 1, 0)
		End If

		bText2 = False
		ComboBox202.Items.Clear()

		tmpStr = My.Resources.str1010 + ": " + System.Math.Round(Dir2Azt(GNav2.pPtPrj, GNav2.ValMax(ValIndex)), 1).ToString(".0") + " - " + System.Math.Round(Dir2Azt(GNav2.pPtPrj, GNav2.ValMin(ValIndex)), 1).ToString(".0")

		InfoFrm.SetGuidNavMsg(tmpStr)
		ToolTip1.SetToolTip(TextBox206, tmpStr)

		N = UBound(WPTList)
		M = UBound(NavaidList)

		pProximity = pTrackLine1
		'DrawPolyLine(pTrackLine1, 0, 2)
		'Application.DoEvents()
		'While True
		'	Application.DoEvents()
		'End While

		ReDim DirWPT2(N + M + 1)
		J = -1

		For I = 0 To N
			If WPTList(I).TypeCode > eNavaidType.NONE Then Continue For
			If pProximity.ReturnDistance(WPTList(I).pPtPrj) > GNav2.Range Then Continue For

			fDir1 = ReturnAngleInDegrees(GNav2.pPtPrj, WPTList(I).pPtPrj)

			If AngleInSector(fDir1, GNav2.ValMin(ValIndex), GNav2.ValMax(ValIndex)) Or AngleInSector(fDir1 + 180.0, GNav2.ValMin(ValIndex), GNav2.ValMax(ValIndex)) Then
				J += 1
				DirWPT2(J) = WPTList(I)
				ComboBox202.Items.Add(DirWPT2(J).CallSign)
			End If
		Next I

		For I = 0 To M
			If (NavaidList(I).Identifier = GNav2.Identifier) Then Continue For 'Or (pNavaidList(I).ID = GNav1.ID) 
			If pProximity.ReturnDistance(NavaidList(I).pPtPrj) > GNav2.Range Then Continue For
			If (ReturnDistanceInMeters(GNav2.pPtPrj, NavaidList(I).pPtPrj) > NavaidList(I).Range) Then Continue For '+ fBuffer''+ GNav.Range - RangeReduse
			fDir1 = ReturnAngleInDegrees(GNav2.pPtPrj, NavaidList(I).pPtPrj)

			If AngleInSector(fDir1, GNav2.ValMin(ValIndex), GNav2.ValMax(ValIndex)) Or AngleInSector(fDir1 + 180.0, GNav2.ValMin(ValIndex), GNav2.ValMax(ValIndex)) Then
				J += 1
				DirWPT2(J) = NavaidList(I)
				ComboBox202.Items.Add(DirWPT2(J).CallSign)
			End If
		Next I

		If J >= 0 Then
			ReDim Preserve DirWPT2(J)
			ComboBox202.Enabled = True
			OptionButton203.Enabled = True '   ??????????????????????????????????
			If OptionButton203.Checked Then
				If ComboBox202.SelectedIndex = 0 Then
					ComboBox202_SelectedIndexChanged(ComboBox202, New System.EventArgs())
				Else
					ComboBox202.SelectedIndex = 0
				End If
			End If
		Else
			ReDim DirWPT2(-1)
			ComboBox202.Enabled = False
			OptionButton203.Enabled = False '   ??????????????????????????????????
			'ComboBox203.Tag = ""
			'ComboBox203_Click()
		End If

		'=======================================================================================
		'pGeomEnv.UseAlternativeTopoOps = True

		If OptionButton204.Checked Or (UBound(DirWPT2) < 0) Then
			If OptionButton204.Checked Then
				OptionButton204_CheckedChanged(OptionButton204, New System.EventArgs())
			Else
				OptionButton204.Checked = True
			End If
		End If
	End Sub

	Private Sub RemoveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles RemoveBtn.Click
		If SegmentNum > 0 Then
			SegmentNum = SegmentNum - 1
			ReportForm.DeleteSegment()

			On Error Resume Next
			If Not Segments(SegmentNum).pFullSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pFullSegElement)
			If Not Segments(SegmentNum).pPrimSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pPrimSegElement)
			If Not Segments(SegmentNum).pStartFixElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pStartFixElement)
			On Error GoTo 0

			RemoveBtn.Enabled = SegmentNum > 0
			ReportBtn.Enabled = SegmentNum > 0
		End If
	End Sub

	'#Region "Geometry"
	Private Sub CorrectFinal(ByVal CurrSegmentNum As Integer)
		Dim I As Integer
		Dim J As Integer

		Dim fTmp As Double
		Dim fDistIn As Double
		Dim fTmpDist As Double
		Dim fFarTol2 As Double
		Dim fLastDir As Double

		Dim ptTmp As ArcGeometry.IPoint
		Dim ptOut As ArcGeometry.IPoint
		Dim ptDIn As ArcGeometry.IPoint
		Dim ptOutS As ArcGeometry.IPoint
		Dim ptDOut As ArcGeometry.IPoint

		Dim pClone As esriSystem.IClone

		Dim pClipLine As ArcGeometry.IPolyline
		Dim pMeasureLine As ArcGeometry.IPolyline
		Dim pPrevClipLine As ArcGeometry.IPolyline

		Dim pOutLine As ArcGeometry.IPointCollection
		Dim pWSpiral As ArcGeometry.IPointCollection
		Dim pTmpPoly As ArcGeometry.IPointCollection
		Dim pTmpPoly1 As ArcGeometry.IPointCollection
		Dim pDefPoints As ArcGeometry.IPointCollection
		Dim pSpOutLine As ArcGeometry.IPointCollection
		Dim FullSegPoly As ArcGeometry.IPointCollection = Nothing
		Dim PrimSegPoly As ArcGeometry.IPointCollection = Nothing

		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pConstructor As ArcGeometry.IConstructPoint
		Dim pRelationP As ArcGeometry.IRelationalOperator
		Dim pRelation As ArcGeometry.IRelationalOperator

		Dim FullSegAt15 As ArcGeometry.Polygon = Nothing
		Dim PrimSegAt15 As ArcGeometry.Polygon = Nothing
		Dim pSecTurnArea As ArcGeometry.Polygon = Nothing
		Dim pPrimTurnArea As ArcGeometry.Polygon = Nothing

		Dim GuidNav2 As TypeDefinitions.NavaidType
		Dim NewFIX As TypeDefinitions.NavaidType

		GuidNav2 = Segments(CurrSegmentNum).EndNav
		NewFIX = Segments(CurrSegmentNum).EndFIX
		fLastDir = Segments(CurrSegmentNum).fDirection

		pClipLine = New ArcGeometry.Polyline

		fFarTol2 = Segments(CurrSegmentNum).fEndFarTol

		'=========================  Create Turn Area  ===========================
		ptTmp = PointAlongPlane(NewFIX.pPtPrj, fLastDir, fFarTol2)
		pClipLine.FromPoint = PointAlongPlane(ptTmp, fLastDir + 90.0, 3.0 * GuidNav2.Range)
		pClipLine.ToPoint = PointAlongPlane(ptTmp, fLastDir - 90.0, 3.0 * GuidNav2.Range)
		pRelation = pClipLine

		If Not (Segments(CurrSegmentNum).pSecTurnArea Is Nothing) Then
			If Not pRelation.Disjoint(Segments(CurrSegmentNum).pSecTurnArea) Then
				ClipByLine(Segments(CurrSegmentNum).pSecTurnArea, pClipLine, Segments(CurrSegmentNum).pSecTurnArea, Nothing, Nothing)
			End If
		End If

		If Not (Segments(CurrSegmentNum).pPrimTurnArea Is Nothing) Then
			If Not pRelation.Disjoint(Segments(CurrSegmentNum).pPrimTurnArea) Then
				ClipByLine(Segments(CurrSegmentNum).pPrimTurnArea, pClipLine, Segments(CurrSegmentNum).pPrimTurnArea, Nothing, Nothing)
			End If
		End If
		'=====================    =======================    ======================

		pSecTurnArea = Segments(CurrSegmentNum).pCutSecPoly
		pPrimTurnArea = Segments(CurrSegmentNum).pCutPrimPoly

		pDefPoints = ReArrangePolygon(pSecTurnArea, Segments(CurrSegmentNum).StartFIX.pPtPrj, fLastDir)
		Segments(CurrSegmentNum).pDefPoints = ReArrangePolygon(pPrimTurnArea, Segments(CurrSegmentNum).StartFIX.pPtPrj, fLastDir)
		'===========================================================================================================

		On Error Resume Next
		If Not Segments(CurrSegmentNum).pFullSegElement Is Nothing Then pGraphics.DeleteElement(Segments(CurrSegmentNum).pFullSegElement)
		If Not Segments(CurrSegmentNum).pPrimSegElement Is Nothing Then pGraphics.DeleteElement(Segments(CurrSegmentNum).pPrimSegElement)
		On Error GoTo 0

		'GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'Application.DoEvents()

		pTopo = pClipLine

		pMeasureLine = pTopo.Intersect(Segments(CurrSegmentNum).pSecPoly, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
		pTmpPoly1 = CreatePrjCircle(ptTmp, 0.5 * pMeasureLine.Length)

		pMeasureLine = pTopo.Intersect(Segments(CurrSegmentNum).pPrimPoly, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
		pTmpPoly = CreatePrjCircle(ptTmp, 0.5 * pMeasureLine.Length)

		ptTmp = PointAlongPlane(NewFIX.pPtPrj, fLastDir, fFarTol2 - Math.Min(fFarTol2, 2.0))

		pClipLine.FromPoint = PointAlongPlane(ptTmp, fLastDir + 90.0, 200000.0)
		pClipLine.ToPoint = PointAlongPlane(ptTmp, fLastDir - 90.0, 200000.0)

		ClipByLine(pTmpPoly1, pClipLine, pTmpPoly1, Nothing, Nothing)
		ClipByLine(pTmpPoly, pClipLine, pTmpPoly, Nothing, Nothing)

		pTopo = pTmpPoly1

		Segments(CurrSegmentNum).pCutSecPoly = pTopo.Union(pSecTurnArea)
		pTopo = Segments(CurrSegmentNum).pCutSecPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pTmpPoly

		Segments(CurrSegmentNum).pCutPrimPoly = pTopo.Union(pPrimTurnArea)
		pTopo = Segments(CurrSegmentNum).pCutPrimPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Segments(CurrSegmentNum).pFullSegElement = DrawPolygon(Segments(CurrSegmentNum).pCutSecPoly, RGB(0, 92, 255))
		Segments(CurrSegmentNum).pPrimSegElement = DrawPolygon(Segments(CurrSegmentNum).pCutPrimPoly, RGB(92, 255, 0))
		'End If
		'Application.DoEvents()

		I = GetEnrouteObstacles(ObsTableList, EnrouteObstacleList, Segments(CurrSegmentNum).pCutPrimPoly, Segments(CurrSegmentNum).pCutSecPoly, Segments(CurrSegmentNum).fMOC, fLastDir, pMeasureLine.FromPoint)

		fTmp = Math.Max(hRouteGuid, hRouteInter)
		If I >= 0 Then
			Segments(CurrSegmentNum).DominantObstacle = EnrouteObstacleList(I)
			Segments(CurrSegmentNum).fRegH = Math.Max(fTmp, EnrouteObstacleList(I).ReqH)
		Else
			Segments(CurrSegmentNum).fRegH = Math.Max(fTmp, Segments(CurrSegmentNum).fMOC)
			Segments(CurrSegmentNum).DominantObstacle.Height = 0.0
			Segments(CurrSegmentNum).DominantObstacle.ReqH = Segments(CurrSegmentNum).fMOC
			Segments(CurrSegmentNum).DominantObstacle.MOC = Segments(CurrSegmentNum).fMOC
			Segments(CurrSegmentNum).DominantObstacle.Index = -1
			Segments(CurrSegmentNum).DominantObstacle.Identifier = Guid.Empty
		End If

		Segments(CurrSegmentNum).pNominalPoly = New ArcGeometry.Polyline()
		Segments(CurrSegmentNum).pNominalPoly.FromPoint = Segments(CurrSegmentNum).StartFIX.pPtPrj
		Segments(CurrSegmentNum).pNominalPoly.FromPoint = Segments(CurrSegmentNum).EndFIX.pPtPrj

		'===========================================================================-
		On Error Resume Next
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		On Error GoTo 0

		'GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'Application.DoEvents()

		'ReportForm.FillReport(Segments)
		'ReportForm.AddSegment(Segments(CurrSegmentNum - 1), EnrouteObstacleList)
	End Sub

	Private Sub CreateGeometry(ByVal NextSegment As Integer, ByVal fTurnTanD As Double, ByVal fAlpha As Double, ByVal Turndir As Integer)
		Dim b15Degree As Boolean

		Dim I As Integer
		Dim J As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer

		Dim w As Double
		Dim fTmp As Double
		Dim fDist As Double
		Dim d15sec As Double
		Dim fDistIn As Double
		Dim fTmpDist As Double
		Dim fFarTol2 As Double
		Dim fNearTol2 As Double

		Dim ptTmp As ArcGeometry.IPoint
		Dim ptOut As ArcGeometry.IPoint
		Dim ptDIn As ArcGeometry.IPoint
		Dim ptOutS As ArcGeometry.IPoint
		Dim ptDOut As ArcGeometry.IPoint

		Dim pClone As esriSystem.IClone

		Dim pClipLine As ArcGeometry.IPolyline
		Dim pPrevClipLine As ArcGeometry.IPolyline
		Dim pMeasureLine As ArcGeometry.IPolyline

		Dim pOutLine As ArcGeometry.IPointCollection
		Dim pWSpiral As ArcGeometry.IPointCollection
		Dim pTmpPoly As ArcGeometry.IPointCollection
		Dim pTmpPoly1 As ArcGeometry.IPointCollection
		Dim pDefPoints As ArcGeometry.IPointCollection
		Dim pSpOutLine As ArcGeometry.IPointCollection
		Dim FullSegPoly As ArcGeometry.IPointCollection = Nothing
		Dim PrimSegPoly As ArcGeometry.IPointCollection = Nothing

		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pConstructor As ArcGeometry.IConstructPoint
		Dim pRelationP As ArcGeometry.IRelationalOperator
		Dim pRelation As ArcGeometry.IRelationalOperator

		Dim FullSegAt15 As ArcGeometry.Polygon = Nothing
		Dim PrimSegAt15 As ArcGeometry.Polygon = Nothing
		Dim pSecTurnArea As ArcGeometry.Polygon = Nothing
		Dim pPrimTurnArea As ArcGeometry.Polygon = Nothing
		Dim FullSegAtTanB As ArcGeometry.Polygon = Nothing
		Dim PrimSegAtTanB As ArcGeometry.Polygon = Nothing

		Dim GuidNav2 As TypeDefinitions.NavaidType

		GuidNav2 = Segments(SegmentNum).EndNav

		If ReturnDistanceInMeters(Segments(SegmentNum).StartNav.pPtPrj, NewFIX.pPtPrj) < distEps Then
			Side1 = SideDef(Segments(SegmentNum).StartNav.pPtPrj, fCurrDir + 90.0, Segments(SegmentNum).StartFIX.pPtPrj)
			Side2 = SideDef(Segments(SegmentNum).StartNav.pPtPrj, fCurrDir + 90.0, NewFIX.pPtPrj)
		End If

		pClipLine = New ArcGeometry.Polyline

		If SegmentNum = 0 Then
			ptTmp = PointAlongPlane(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 180.0, fNearTol)

			pClipLine.FromPoint = PointAlongPlane(ptTmp, fCurrDir + 90.0, 200000.0)
			pClipLine.ToPoint = PointAlongPlane(ptTmp, fCurrDir - 90.0, 200000.0)

			'DrawPolygon(Segments(SegmentNum).pSecPoly, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
			'DrawPolygon(Segments(SegmentNum).pPrimPoly, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
			'DrawPolyLine(pClipLine, -1, 2)
			'Application.DoEvents()

			'While True
			'	Application.DoEvents()
			'End While

			ClipByLine(Segments(SegmentNum).pSecPoly, pClipLine, FullSegPoly, Nothing, Nothing)
			ClipByLine(Segments(SegmentNum).pPrimPoly, pClipLine, PrimSegPoly, Nothing, Nothing)

			'DrawPolygon(FullSegPoly, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
			'DrawPolygon(PrimSegPoly, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
			'Application.DoEvents()

			pTopo = pClipLine

			pMeasureLine = pTopo.Intersect(Segments(SegmentNum).pSecPoly, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
			pTmpPoly1 = CreatePrjCircle(ptTmp, 0.5 * pMeasureLine.Length)

			pMeasureLine = pTopo.Intersect(Segments(SegmentNum).pPrimPoly, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
			pTmpPoly = CreatePrjCircle(ptTmp, 0.5 * pMeasureLine.Length)

			ptTmp = PointAlongPlane(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 180.0, fNearTol - Math.Min(fNearTol, 0.5))

			pClipLine.FromPoint = PointAlongPlane(ptTmp, fCurrDir + 90.0, 200000.0)
			pClipLine.ToPoint = PointAlongPlane(ptTmp, fCurrDir - 90.0, 200000.0)

			'DrawPolyLine(pClipLine, -1, 2)
			'Application.DoEvents()

			ClipByLine(pTmpPoly1, pClipLine, Nothing, pTmpPoly1, Nothing)
			ClipByLine(pTmpPoly, pClipLine, Nothing, pTmpPoly, Nothing)

			pTopo = pTmpPoly1
			FullSegPoly = pTopo.Union(FullSegPoly)

			pTopo = FullSegPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTopo = pTmpPoly
			PrimSegPoly = pTopo.Union(PrimSegPoly)

			pTopo = PrimSegPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pClone = PrimSegPoly
			Segments(SegmentNum).pCutPrimPoly = pClone.Clone

			pClone = FullSegPoly
			Segments(SegmentNum).pCutSecPoly = pClone.Clone
		Else
			If Not (Segments(SegmentNum - 1).pSecTurnArea Is Nothing) Then
				pRelation = Segments(SegmentNum).pCutSecPoly

				If Not pRelation.Disjoint(Segments(SegmentNum - 1).pSecTurnArea) Then
					pTopo = Segments(SegmentNum).pCutSecPoly

					FullSegPoly = pTopo.Union(Segments(SegmentNum - 1).pSecTurnArea)
					pTopo = FullSegPoly
					pTopo.IsKnownSimple_2 = False

					pTopo.Simplify()
				Else
					pClone = Segments(SegmentNum).pCutSecPoly
					FullSegPoly = pClone.Clone
				End If
			Else
				pClone = Segments(SegmentNum).pCutSecPoly
				FullSegPoly = pClone.Clone
			End If

			If Not (Segments(SegmentNum - 1).pPrimTurnArea Is Nothing) Then
				pRelation = Segments(SegmentNum).pCutPrimPoly

				If Not pRelation.Disjoint(Segments(SegmentNum - 1).pPrimTurnArea) Then
					pTopo = Segments(SegmentNum).pCutPrimPoly

					PrimSegPoly = pTopo.Union(Segments(SegmentNum - 1).pPrimTurnArea)
					pTopo = PrimSegPoly
					pTopo.IsKnownSimple_2 = False

					pTopo.Simplify()
				Else
					pClone = Segments(SegmentNum).pCutPrimPoly
					PrimSegPoly = pClone.Clone
				End If
			Else
				pClone = Segments(SegmentNum).pCutPrimPoly
				PrimSegPoly = pClone.Clone
			End If
		End If

		''While True
		''	Application.DoEvents()
		''End While
		'DrawPolygon(Segments(SegmentNum).pCutSecPoly, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(Segments(SegmentNum).pCutPrimPoly, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		''DrawPolyLine(pClipLine, -1, 2)
		'Application.DoEvents()


		fNearTol2 = Segments(SegmentNum).fEndNearTol
		fFarTol2 = Segments(SegmentNum).fEndFarTol

		w = 87.0 + 12.0 * 0.001 * hRoute
		d15sec = (ToBankTime.Value + MaxPilotToler.Value) * (fTAS + w) * TurnIAS.Multiplier
		fDist = fFarTol2 + d15sec

		ptTmp = PointAlongPlane(NewFIX.pPtPrj, fCurrDir, fDist)

		pClipLine.FromPoint = PointAlongPlane(ptTmp, fCurrDir + 90.0, GuidNav2.Range)
		pClipLine.ToPoint = PointAlongPlane(ptTmp, fCurrDir - 90.0, GuidNav2.Range)

		ClipByLine(FullSegPoly, pClipLine, Nothing, FullSegAt15, Nothing)
		ClipByLine(PrimSegPoly, pClipLine, Nothing, PrimSegAt15, Nothing)

		pTmpPoly = PrimSegAt15
		pTmpPoly1 = FullSegAt15

		If (pTmpPoly1.PointCount = 0) Or (pTmpPoly.PointCount = 0) Then
			MessageBox.Show(My.Resources.str0121)
			SegmentNum -= 1
			iError = -1
			Return
		End If

		'=====================  Create Turn Area  =======================
		fDist = fNearTol2 + fTurnTanD

		ptTmp = PointAlongPlane(NewFIX.pPtPrj, fCurrDir + 180.0, fDist)

		pMeasureLine = New ArcGeometry.Polyline
		pTmpPoly = pMeasureLine

		'While True
		'	Application.DoEvents()
		'End While

		'fNextDir + 0.5 * Turndir * fAlpha
		'fCurrDir - 0.5 * Turndir * fAlpha
		'DrawPointWithText(ptTmp, "pt-Tmp")

		'If Turndir > 0 Then
		'	'DrawPointWithText(PointAlongPlane(ptTmp, fCurrDir + 0.5 * fAlpha, -20000.0), "pt-04")
		'	'Application.DoEvents()

		'	pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir + 90.0, 3.0 * GuidNav2.Range))
		'	pTmpPoly.AddPoint(ptTmp)
		'	pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir + 0.5 * fAlpha, -GuidNav2.Range))
		'	'pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir - 90.0, 3.0 * GuidNav2.Range))
		'Else
		'	'DrawPointWithText(PointAlongPlane(ptTmp, fCurrDir - 0.5 * fAlpha, -20000.0), "pt-03")
		'	'Application.DoEvents()

		'	'pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir + 90.0, 3.0 * GuidNav2.Range))
		'	pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir - 0.5 * fAlpha, -GuidNav2.Range))
		'	pTmpPoly.AddPoint(ptTmp)
		'	pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fCurrDir - 90.0, 3.0 * GuidNav2.Range))
		'End If

		pMeasureLine.FromPoint = PointAlongPlane(ptTmp, fCurrDir + 90.0, 3.0 * GuidNav2.Range)
		pMeasureLine.ToPoint = PointAlongPlane(ptTmp, fCurrDir - 90.0, 3.0 * GuidNav2.Range)

		'DrawPolyLine(pMeasureLine) '+++++++++++++++++++++++++++++++++
		'DrawPolyLine(pMeasureLine, -1, 2)
		'Application.DoEvents()

		pRelation = pMeasureLine

		If Not (Segments(SegmentNum).pSecTurnArea Is Nothing) Then
			'pTopo = Segments(SegmentNum).pSecTurnArea
			If Not pRelation.Disjoint(Segments(SegmentNum).pSecTurnArea) Then
				ClipByLine(Segments(SegmentNum).pSecTurnArea, pMeasureLine, Segments(SegmentNum).pSecTurnArea, Nothing, Nothing)
			End If
		End If

		If Not (Segments(SegmentNum).pPrimTurnArea Is Nothing) Then
			'pTopo = Segments(SegmentNum).pPrimTurnArea
			If Not pRelation.Disjoint(Segments(SegmentNum).pPrimTurnArea) Then
				ClipByLine(Segments(SegmentNum).pPrimTurnArea, pMeasureLine, Segments(SegmentNum).pPrimTurnArea, Nothing, Nothing)
			End If
		End If

		'=====================    =======================    ======================
		'If Not pRelation.Disjoint(FullSegAt15) Then
		'	pTopo = FullSegAt15
		'	pTopo.Cut(pMeasureLine, pSecTurnArea, pTmpPoly)
		'	'ClipByLine(FullSegAt15, pMeasureLine, pSecTurnArea, Nothing, Nothing)
		'Else
		'	pSecTurnArea = FullSegAt15
		'End If

		'If Not pRelation.Disjoint(PrimSegAt15) Then
		'	pTopo = PrimSegAt15
		'	pTopo.Cut(pMeasureLine, pPrimTurnArea, pTmpPoly)
		'	'ClipByLine(PrimSegAt15, pMeasureLine, pPrimTurnArea, Nothing, Nothing)
		'Else
		'	pPrimTurnArea = PrimSegAt15
		'End If

		ClipByLine(FullSegAt15, pMeasureLine, pSecTurnArea, Nothing, Nothing)
		ClipByLine(PrimSegAt15, pMeasureLine, pPrimTurnArea, Nothing, Nothing)

		'DrawPolygon(pSecTurnArea, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pPrimTurnArea, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'While True
		'	Application.DoEvents()
		'End While

		'=====================    =======================    ======================

		pDefPoints = ReArrangePolygon(pSecTurnArea, Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir)
		Segments(SegmentNum).pDefPoints = ReArrangePolygon(pPrimTurnArea, Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir)

		ptTmp = PointAlongPlane(NewFIX.pPtPrj, fCurrDir, fFarTol2)
		pClipLine.FromPoint = PointAlongPlane(ptTmp, fCurrDir + 90.0, 3.0 * GuidNav2.Range)
		pClipLine.ToPoint = PointAlongPlane(ptTmp, fCurrDir - 90.0, 3.0 * GuidNav2.Range)

		'===========================================================================================================
		On Error Resume Next
		If Not Segments(SegmentNum).pFullSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pFullSegElement)
		If Not Segments(SegmentNum).pPrimSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pPrimSegElement)
		On Error GoTo 0
		'===========================================================================================================

		'DrawPolygon(FullSegAt15, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(PrimSegAt15, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolyLine(pClipLine, -1, 2)
		'Application.DoEvents()

		ClipByLine(FullSegAt15, pClipLine, Nothing, Segments(SegmentNum).pCutSecPoly, Nothing)
		ClipByLine(PrimSegAt15, pClipLine, Nothing, Segments(SegmentNum).pCutPrimPoly, Nothing)

		pTmpPoly = Segments(SegmentNum).pCutPrimPoly

		If Turndir <> 0 Then
			ClipByLine(pSecTurnArea, pClipLine, Nothing, pTmpPoly1, Nothing)

			pClipLine.FromPoint = PointAlongPlane(NewFIX.pPtPrj, fCurrDir, 3.0 * GuidNav2.Range)
			pClipLine.ToPoint = PointAlongPlane(NewFIX.pPtPrj, fCurrDir + 180.0, 3.0 * GuidNav2.Range)

			If Turndir > 0 Then
				ClipByLine(pTmpPoly1, pClipLine, pTmpPoly1, Nothing, Nothing)
			ElseIf Turndir < 0 Then
				ClipByLine(pTmpPoly1, pClipLine, Nothing, pTmpPoly1, Nothing)
			End If

			pTopo = pTmpPoly
			Segments(SegmentNum).pCutPrimPoly = pTopo.Union(pTmpPoly1)

			pTopo = Segments(SegmentNum).pCutPrimPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If

		'===========================================================================================================
		Segments(SegmentNum).pFullSegElement = DrawPolygon(Segments(SegmentNum).pCutSecPoly, RGB(0, 92, 255))
		Segments(SegmentNum).pPrimSegElement = DrawPolygon(Segments(SegmentNum).pCutPrimPoly, RGB(92, 255, 0))
		'===========================================================================================================

		'//NextSegment =============================================================================================
		pClone = Segments(NextSegment).pSecPoly
		FullSegPoly = pClone.Clone

		pClone = Segments(NextSegment).pPrimPoly
		PrimSegPoly = pClone.Clone

		'=============================================================================
		pTopo = FullSegPoly
		pClipLine.FromPoint = PointAlongPlane(NewFIX.pPtPrj, fNextDir + 90.0, GuidNav2.Range)
		pClipLine.ToPoint = PointAlongPlane(NewFIX.pPtPrj, fNextDir - 90.0, GuidNav2.Range)

		pClipLine = pTopo.Intersect(pClipLine, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)

		'=============================================================================

		pMeasureLine = pTopo.Intersect(pMeasureLine, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
		ptTmp = PointAlongPlane(ptTmp, fNextDir, fDist)
		'=============================================================================
		pPrevClipLine = New ArcGeometry.Polyline

		If (Turndir <> 0) And (fAlpha > 2.0) Then
			If SideDef(pClipLine.FromPoint, fNextDir, pClipLine.ToPoint) <> Turndir Then pClipLine.ReverseOrientation()

			'DrawPolyLine(pClipLine, -1, 2)
			'Application.DoEvents()

			ptDIn = pClipLine.ToPoint

			pClipLine.FromPoint = PointAlongPlane(ptTmp, fNextDir + 90.0, GuidNav2.Range)
			pClipLine.ToPoint = PointAlongPlane(ptTmp, fNextDir - 90.0, GuidNav2.Range)

			pClipLine = pTopo.Intersect(pClipLine, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)

			If SideDef(pClipLine.FromPoint, fNextDir, pClipLine.ToPoint) <> Turndir Then pClipLine.ReverseOrientation()
			If SideDef(pMeasureLine.FromPoint, fCurrDir, pMeasureLine.ToPoint) <> Turndir Then pMeasureLine.ReverseOrientation()

			'DrawPolyLine(pClipLine, -1, 2)
			'DrawPolyLine(pMeasureLine, -1, 2)
			'Application.DoEvents()

			''fTmp = ReturnAngleInDegrees(ptDIn, pClipLine.ToPoint)
			'DrawPointWithText(ptDIn, "ptDIn")
			'DrawPointWithText(pClipLine.ToPoint, "CliToPoint")
			'Application.DoEvents()

			If False And (AnglesSideDef(ReturnAngleInDegrees(ptDIn, pClipLine.ToPoint), fCurrDir - 90.0 * Turndir) = Turndir) Then
				pPrevClipLine.FromPoint = PointAlongPlane(pMeasureLine.FromPoint, fCurrDir + Turndir * 90.0, 1.0)
				pPrevClipLine.ToPoint = PointAlongPlane(pMeasureLine.ToPoint, fCurrDir - Turndir * 90.0, 1.0)

				'DrawPolyLine(pPrevClipLine, , 2)
				'DrawPolygon(FullSegPoly, -1, Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
				'DrawPolygon(PrimSegPoly, -1, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
				'Application.DoEvents()

				If (Turndir > 0) Then
					ClipByLine(FullSegPoly, pPrevClipLine, FullSegPoly, Nothing, pTmpPoly1)
				Else
					ClipByLine(FullSegPoly, pPrevClipLine, Nothing, FullSegPoly, pTmpPoly1)
				End If

				If FullSegPoly.PointCount = 0 Then
					pClone = Segments(NextSegment).pSecPoly
					FullSegPoly = pClone.Clone
				End If

				If (Turndir > 0) Then
					ClipByLine(PrimSegPoly, pPrevClipLine, PrimSegPoly, Nothing, pTmpPoly1)
				Else
					ClipByLine(PrimSegPoly, pPrevClipLine, Nothing, PrimSegPoly, pTmpPoly1)
				End If

				If PrimSegPoly.PointCount = 0 Then
					pClone = Segments(NextSegment).pPrimPoly
					PrimSegPoly = pClone.Clone
				End If
			End If

			pOutLine = New ArcGeometry.Polyline
			pTmpPoly = PrimSegPoly
			pOutLine.AddPointCollection(pTmpPoly)

			pRelation = Segments(NextSegment).pPrimPoly

			'DrawPolyLine(pOutLine, 255, 2)
			'Application.DoEvents()

			'Оснавная зона            =======================
			'   Внешная сторона

			pWSpiral = CreateTurnArea(w, fTurnR, fTAS, Segments(SegmentNum).fDirection, fNextDir, -Turndir, Segments(SegmentNum).EndFIX.pPtPrj, Segments(SegmentNum).pDefPoints)
			ptOut = pWSpiral.Point(pWSpiral.PointCount - 1)
			ptOutS = pWSpiral.Point(pWSpiral.PointCount - 1)

			'DrawPolygon pWSpiral, 233
			'DrawPointWithText ptOut, "ptOut", 0
			pTmpPoly1 = pClipLine
			If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

			'проверка 15 градусов

			If pRelation.Contains(ptOut) Then
				'pClipLine.FromPoint = PointAlongPlane(ptOut, fNextDir + Turndir * SplayAngl.Value + 180.0, 2.0 * GuidNav2.Range)
				'pClipLine.ToPoint = PointAlongPlane(ptOut, fNextDir + Turndir * SplayAngl.Value, 2.0 * GuidNav2.Range)

				If Turndir > 0 Then
					fTmp = ReturnAngleInDegrees(pWSpiral.Point(0), Segments(SegmentNum).pDefPoints.Point(0))
				Else
					fTmp = ReturnAngleInDegrees(pWSpiral.Point(0), Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1))
				End If

				pTmpPoly1.AddPoint(PointAlongPlane(pWSpiral.Point(0), fTmp, 2.0 * GuidNav2.Range))
				pTmpPoly1.AddPoint(pWSpiral.Point(0))
				pTmpPoly1.AddPoint(ptOut)
				pTmpPoly1.AddPoint(PointAlongPlane(ptOut, fNextDir + Turndir * SplayAngl.Value, 2.0 * GuidNav2.Range))

				pTopo = PrimSegPoly
				If Turndir < 0 Then
					pTopo.Cut(pClipLine, PrimSegPoly, pTmpPoly1)
					'ClipByLine PrimSegPoly, pClipLine, PrimSegPoly, Nothing, Nothing
				Else
					pTopo.Cut(pClipLine, pTmpPoly1, PrimSegPoly)
					'ClipByLine PrimSegPoly, pClipLine, Nothing, PrimSegPoly, Nothing
				End If

				pTopo = PrimSegPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pClone = pWSpiral
				pTmpPoly = pClone.Clone

				fTmp = 0.5 * ReturnDistanceInMeters(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1), Segments(SegmentNum).pDefPoints.Point(0))
				ptTmp = PointAlongPlane(pWSpiral.Point(0), fCurrDir - Turndir * 90.0, fTmp)
				pTmpPoly.AddPoint(ptTmp, 0)

				ptTmp = PointAlongPlane(ptOut, fNextDir - Turndir * 90.0, fTmp)
				pTmpPoly.AddPoint(ptTmp)

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPrimTurnArea = pTopo.Union(pPrimTurnArea)
				pTopo = pPrimTurnArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTopo = pWSpiral
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				'DrawPolygon PrimSegPoly, 233
			Else
				' проверка 30 градусов                ========================================
				If Turndir > 0 Then
					fTmp = ReturnAngleInDegrees(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1), Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 2))
					pTmpPoly1.AddPoint(PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1), fTmp + 180.0, 2.0 * GuidNav2.Range))

					For I = Segments(SegmentNum).pDefPoints.PointCount - 2 To 0 Step -1
						If SideDef(NewFIX.pPtPrj, fCurrDir, Segments(SegmentNum).pDefPoints.Point(I)) > 0 Then
							Exit For
						End If
						pTmpPoly1.AddPoint(Segments(SegmentNum).pDefPoints.Point(I))
					Next I
				Else
					fTmp = ReturnAngleInDegrees(Segments(SegmentNum).pDefPoints.Point(0), Segments(SegmentNum).pDefPoints.Point(1))
					pTmpPoly1.AddPoint(PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(0), fTmp + 180.0, 2.0 * GuidNav2.Range))

					For I = 1 To Segments(SegmentNum).pDefPoints.PointCount - 1
						If SideDef(NewFIX.pPtPrj, fCurrDir, Segments(SegmentNum).pDefPoints.Point(I)) < 0 Then
							Exit For
						End If
						pTmpPoly1.AddPoint(Segments(SegmentNum).pDefPoints.Point(I))
					Next I
				End If

				fTmp = ReturnAngleInDegrees(pWSpiral.Point(0), pWSpiral.Point(pWSpiral.PointCount - 1))
				pTmpPoly1.AddPoint(PointAlongPlane(pWSpiral.Point(pWSpiral.PointCount - 1), fTmp, 2.0 * GuidNav2.Range))

				'DrawPolygon(PrimSegPoly, RGB(0, 255, 255))
				'DrawPolyLine(pClipLine, 255, 2)
				'Application.DoEvents()

				pTopo = PrimSegPoly
				pTmpPoly1 = New ArcGeometry.Polygon

				On Error Resume Next
				If Turndir < 0 Then
					pTopo.Cut(pClipLine, PrimSegPoly, pTmpPoly1)
				Else
					pTopo.Cut(pClipLine, pTmpPoly1, PrimSegPoly)
				End If
				On Error GoTo 0

				pTopo = PrimSegPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				'DrawPolygon(PrimSegPoly, 255, Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
				'Application.DoEvents()

				'========================================
				pWSpiral = CreateTurnArea(w, fTurnR, fTAS, Segments(SegmentNum).fDirection, fNextDir - Turndir * SnapAngle.Value, -Turndir, Segments(SegmentNum).EndFIX.pPtPrj, Segments(SegmentNum).pDefPoints)
				ptOut = pWSpiral.Point(pWSpiral.PointCount - 1)
				'DrawPolygon pWSpiral

				pConstructor = ptTmp
				pConstructor.ConstructAngleIntersection(NewFIX.pPtPrj, DegToRad(fNextDir), ptOut, DegToRad(fNextDir - Turndir * SnapAngle.Value))
				pWSpiral.AddPoint(ptTmp)
				pWSpiral.AddPoint(NewFIX.pPtPrj)

				pTopo = pWSpiral
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPrimTurnArea = pTopo.Union(pPrimTurnArea)
				pTopo = pPrimTurnArea
				pTopo.IsKnownSimple_2 = False

				pTopo.Simplify()
			End If

			' Внутренная сторона            =======================
			If Turndir < 0 Then
				ptOut = Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1)
			Else
				ptOut = Segments(SegmentNum).pDefPoints.Point(0)
			End If

			'DrawPoint ptOut, 0
			' проверка 15 градусов

			If pRelation.Contains(ptOut) Or (fAlpha < 1.0) Then
				pTmpPoly1 = pClipLine
				If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)
				pTmpPoly1.AddPoint(PointAlongPlane(ptOut, fNextDir - Turndir * SplayAngl.Value + 180.0, GuidNav2.Range))
				pTmpPoly1.AddPoint(ptOut)
				pTmpPoly1.AddPoint(PointAlongPlane(ptOut, fNextDir - Turndir * SplayAngl.Value, GuidNav2.Range))

				pTopo = PrimSegPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				If Turndir < 0 Then
					'ClipByLine PrimSegPoly, pClipLine, Nothing, PrimSegPoly, Nothing
					pTopo.Cut(pClipLine, pTmpPoly1, PrimSegPoly)
				Else
					'ClipByLine PrimSegPoly, pClipLine, PrimSegPoly, Nothing, Nothing
					pTopo.Cut(pClipLine, PrimSegPoly, pTmpPoly1)
				End If

				'DrawPolygon(pTopo, , Display.esriSimpleFillStyle.esriSFSCross)
				'DrawPolyLine(pClipLine, , 2)
				'Application.DoEvents()

				pTopo = PrimSegPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPrimTurnArea = pTopo.Union(pPrimTurnArea)
				pTopo = pPrimTurnArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				'DrawPolygon pPrimTurnArea

				b15Degree = True
				'================================
				pTopo = pOutLine

				pTmpPoly = pTopo.Intersect(pClipLine, ArcGeometry.esriGeometryDimension.esriGeometry0Dimension)
				fDistIn = 0.0

				For I = 0 To pTmpPoly.PointCount - 1
					'?????????????????????????????????????????????????????????????????????????????????????
					fTmpDist = Point2LineDistancePrj(Segments(SegmentNum).EndFIX.pPtPrj, pTmpPoly.Point(I), fNextDir + 90.0) * SideDef(Segments(SegmentNum).EndFIX.pPtPrj, fNextDir + 90.0, pTmpPoly.Point(I))
					If fTmpDist > fDistIn Then fDistIn = fTmpDist
				Next I
			Else            ' проверка fAlpha / 2 градусов
				pConstructor = ptTmp
				pTmpPoly = New ArcGeometry.Polygon
				pConstructor.ConstructAngleIntersection(NewFIX.pPtPrj, DegToRad(fNextDir), ptOut, DegToRad(fNextDir + 0.5 * Turndir * fAlpha))

				pTmpPoly.AddPoint(PointAlongPlane(NewFIX.pPtPrj, fNextDir + 90.0 * Turndir, 100.0))
				pTmpPoly.AddPoint(NewFIX.pPtPrj)
				pTmpPoly.AddPoint(ptOut)
				pTmpPoly.AddPoint(ptTmp)
				pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fNextDir + 90.0 * Turndir, 100.0))

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				'DrawPolygon(pTmpPoly, , Display.esriSimpleFillStyle.esriSFSCross)
				'Application.DoEvents()

				pPrimTurnArea = pTopo.Union(pPrimTurnArea)
				pTopo = pPrimTurnArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				b15Degree = False
				'=========================================================================
				pSpOutLine = New ArcGeometry.Polyline
				pSpOutLine.AddPointCollection(pTmpPoly)

				pTopo = pOutLine

				pTmpPoly = pTopo.Intersect(pSpOutLine, ArcGeometry.esriGeometryDimension.esriGeometry0Dimension)
				fDistIn = 0.0

				For I = 0 To pTmpPoly.PointCount - 1
					fTmpDist = Point2LineDistancePrj(Segments(SegmentNum).EndFIX.pPtPrj, pTmpPoly.Point(I), fNextDir + 90.0) * SideDef(Segments(SegmentNum).EndFIX.pPtPrj, fNextDir + 90.0, pTmpPoly.Point(I))
					If fTmpDist > fDistIn Then
						fDistIn = fTmpDist
					End If
				Next I

				pSpOutLine = Nothing
			End If

			pSpOutLine = New ArcGeometry.Polyline
			pSpOutLine.AddPointCollection(pWSpiral)

			pTopo = pOutLine

			pTmpPoly = pTopo.Intersect(pSpOutLine, ArcGeometry.esriGeometryDimension.esriGeometry0Dimension)
			'DrawPolyLine pSpOutLine, 255, 2
			'DrawPolyLine pOutLine, 0, 2

			fDist = 0.0

			For I = 0 To pTmpPoly.PointCount - 1
				fTmpDist = Point2LineDistancePrj(Segments(SegmentNum).EndFIX.pPtPrj, pTmpPoly.Point(I), fNextDir + 90.0) * SideDef(Segments(SegmentNum).EndFIX.pPtPrj, fNextDir + 90.0, pTmpPoly.Point(I))
				If fTmpDist > fDist Then
					fDist = fTmpDist
				End If
			Next I

			If fDistIn > fDist Then
				Segments(NextSegment).ptD0 = PointAlongPlane(Segments(SegmentNum).EndFIX.pPtPrj, fNextDir, fDistIn)
			Else
				Segments(NextSegment).ptD0 = PointAlongPlane(Segments(SegmentNum).EndFIX.pPtPrj, fNextDir, fDist)
			End If

			'============================================================================================
			'Дополнительная зона
			'Внешная сторона

			fDist = 2.0 * (fNearTol2 + fTurnTanD + fFarTol2 + d15sec)
			If Turndir < 0 Then
				ptTmp = PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(0), fCurrDir, fDist)
			Else
				ptTmp = PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1), fCurrDir, fDist)
			End If

			'DrawPolygon(pPrimTurnArea, RGB(255, 0, 255))
			'DrawPolygon(PrimSegPoly, RGB(255, 0, 255), Display.esriSimpleFillStyle.esriSFSCross)
			'Application.DoEvents()

			J = -1
			For I = 0 To Segments(SegmentNum).pDefPoints.PointCount - 1
				fTmpDist = ReturnDistanceInMeters(ptTmp, Segments(SegmentNum).pDefPoints.Point(I))
				If fTmpDist < fDist Then
					fDist = fTmpDist
					J = I
				End If
			Next I

			ptDIn = Segments(SegmentNum).pDefPoints.Point(J)
			fDist = 2.0 * (fNearTol2 + fTurnTanD + fFarTol2 + d15sec)

			J = -1
			For I = 0 To pDefPoints.PointCount - 1
				fTmpDist = ReturnDistanceInMeters(ptTmp, pDefPoints.Point(I))
				If fTmpDist < fDist Then
					fDist = fTmpDist
					J = I
				End If
			Next I
			ptDOut = pDefPoints.Point(J)

			pTopo = pWSpiral
			fTmpDist = ReturnDistanceInMeters(ptDIn, ptDOut)

			pTmpPoly = pTopo.Buffer(fTmpDist)

			ptOut = PointAlongPlane(ptOutS, fNextDir + Turndir * 90.0, fTmpDist)

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTmpPoly1 = pClipLine
			If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

			ptTmp = PointAlongPlane(NewFIX.pPtPrj, fCurrDir + 180.0, fNearTol2 + fTurnTanD)
			pTmpPoly1.AddPoint(PointAlongPlane(ptTmp, fCurrDir + 90.0 * Turndir, 2.0 * GuidNav2.Range))
			pTmpPoly1.AddPoint(ptTmp)
			pTmpPoly1.AddPoint(NewFIX.pPtPrj)
			pTmpPoly1.AddPoint(PointAlongPlane(NewFIX.pPtPrj, fNextDir, 2.0 * GuidNav2.Range))
			'pTmpPoly1.AddPoint PointAlongPlane(ptTmp, fCurrDir + 90.0 * Turndir, 2.0 * GuidNav2.Range)


			'DrawPolygon pTmpPoly, RGB(0, 255, 255)
			'DrawPolygon pWSpiral

			'DrawPolyLine(pClipLine, RGB(0, 255, 0), 2)
			'Application.DoEvents()

			On Error Resume Next
			If Turndir < 0 Then
				pTopo.Cut(pClipLine, pTmpPoly1, pTmpPoly)
			Else
				pTopo.Cut(pClipLine, pTmpPoly, pTmpPoly1)
			End If
			On Error GoTo 0

			pTmpPoly1 = pClipLine
			pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pRelation = Segments(NextSegment).pSecPoly
			pRelationP = Segments(NextSegment).pPrimPoly

			If pRelation.Contains(ptOut) Then
				pClipLine.FromPoint = PointAlongPlane(ptOut, fNextDir + Turndir * SplayAngl.Value + 180.0, GuidNav2.Range)
				pClipLine.ToPoint = PointAlongPlane(ptOut, fNextDir + Turndir * SplayAngl.Value, GuidNav2.Range)

				If Turndir < 0 Then
					ClipByLine(FullSegPoly, pClipLine, FullSegPoly, Nothing, Nothing)
				Else
					ClipByLine(FullSegPoly, pClipLine, Nothing, FullSegPoly, Nothing)
				End If

				pClipLine.FromPoint = PointAlongPlane(ptOut, fCurrDir + Turndir * SplayAngl.Value + 180.0, GuidNav2.Range)
				pClipLine.ToPoint = PointAlongPlane(ptOut, fCurrDir + Turndir * SplayAngl.Value, GuidNav2.Range)

				If Turndir < 0 Then
					ClipByLine(FullSegPoly, pClipLine, FullSegPoly, Nothing, Nothing)
				Else
					ClipByLine(FullSegPoly, pClipLine, Nothing, FullSegPoly, Nothing)
				End If
			Else
				pTmpPoly1 = ReturnPolygonPartAsPolyline(pRelation, NewFIX.pPtPrj, fNextDir + 90.0 * (Turndir + 1), 1) 'Turndir
				pTopo = pTmpPoly

				pOutLine = pTopo.Intersect(pTmpPoly1, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)
				fDist = 10.0 * GuidNav2.Range
				pTopo = pRelation

				If Turndir < 0 Then
					pTmpPoly1 = pClipLine
					If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

					If Not pRelationP.Contains(pDefPoints.Point(pDefPoints.PointCount - 1)) Then
						pTmpPoly1.AddPoint(PointAlongPlane(pDefPoints.Point(pDefPoints.PointCount - 1), fNextDir + SplayAngl.Value, GuidNav2.Range))
						pTmpPoly1.AddPoint(pDefPoints.Point(pDefPoints.PointCount - 1))
						pTmpPoly1.AddPoint(pDefPoints.Point(0))
					Else
						pTmpPoly1.AddPoint(PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1), fNextDir + SplayAngl.Value, GuidNav2.Range))
						pTmpPoly1.AddPoint(Segments(SegmentNum).pDefPoints.Point(Segments(SegmentNum).pDefPoints.PointCount - 1))
						pTmpPoly1.AddPoint(pDefPoints.Point(0))
					End If

					For I = 1 To pDefPoints.PointCount - 1
						If SideDef(NewFIX.pPtPrj, fCurrDir, pDefPoints.Point(I)) < 0 Then
							Exit For
						End If
						pTmpPoly1.AddPoint(pDefPoints.Point(I))
					Next I

					If pRelation.Contains(pTmpPoly1.Point(pTmpPoly1.PointCount - 1)) Then
						fTmp = 1000000.0
						J = -1
						For I = 0 To pOutLine.PointCount - 1
							fTmpDist = Point2LineDistancePrj(pOutLine.Point(I), pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fCurrDir) * SideDef(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fCurrDir + 180.0, pOutLine.Point(I))
							If fTmpDist < fTmp Then
								fTmp = fTmpDist
								J = I
							End If
						Next I
						fTmp = ReturnAngleInDegrees(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), pOutLine.Point(J))
						pTmpPoly1.AddPoint(PointAlongPlane(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fTmp + 1.0, GuidNav2.Range))
					End If

					'DrawPolyLine(pClipLine, RGB(0, 255, 0), 3)
					'Application.DoEvents()

					pTopo = FullSegPoly
					pTopo.Cut(pClipLine, FullSegPoly, pTmpPoly1)
				Else
					pTmpPoly1 = pClipLine
					If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

					If Not pRelationP.Contains(pDefPoints.Point(0)) Then
						pTmpPoly1.AddPoint(PointAlongPlane(pDefPoints.Point(0), fNextDir - SplayAngl.Value, GuidNav2.Range))
						pTmpPoly1.AddPoint(pDefPoints.Point(0))
						pTmpPoly1.AddPoint(pDefPoints.Point(pDefPoints.PointCount - 1))
					Else
						pTmpPoly1.AddPoint(PointAlongPlane(Segments(SegmentNum).pDefPoints.Point(0), fNextDir - SplayAngl.Value, GuidNav2.Range))
						pTmpPoly1.AddPoint(Segments(SegmentNum).pDefPoints.Point(0))
						pTmpPoly1.AddPoint(pDefPoints.Point(pDefPoints.PointCount - 1))
					End If

					For I = pDefPoints.PointCount - 2 To 0 Step -1
						If SideDef(NewFIX.pPtPrj, fCurrDir, pDefPoints.Point(I)) > 0 Then
							Exit For
						End If
						pTmpPoly1.AddPoint(pDefPoints.Point(I))
					Next I

					If pRelation.Contains(pTmpPoly1.Point(pTmpPoly1.PointCount - 1)) Then
						fTmp = 1000000.0
						J = -1
						For I = 0 To pOutLine.PointCount - 1
							fTmpDist = Point2LineDistancePrj(pOutLine.Point(I), pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fCurrDir) * SideDef(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fCurrDir, pOutLine.Point(I))
							If fTmpDist < fTmp Then
								fTmp = fTmpDist
								J = I
							End If
						Next I

						fTmp = ReturnAngleInDegrees(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), pOutLine.Point(J))
						pTmpPoly1.AddPoint(PointAlongPlane(pTmpPoly1.Point(pTmpPoly1.PointCount - 1), fTmp - 1.0, GuidNav2.Range))
					End If
					pTopo = FullSegPoly

					pTopo.Cut(pClipLine, pTmpPoly1, FullSegPoly)
				End If

				If (FullSegPoly.PointCount = 0) And (pTmpPoly1.PointCount > 0) Then FullSegPoly = pTmpPoly1

				pTopo = FullSegPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
			End If

			'Set pTopo = pTmpPoly
			'pTopo.IsKnownSimple_2 = False
			'pTopo.Simplify
			'DrawPolygon FullSegPoly
			pTopo = pSecTurnArea

			pSecTurnArea = pTopo.Union(pTmpPoly)
			'DrawPolygon pTmpPoly
			'DrawPolygon pSecTurnArea
			Segments(NextSegment).pSecTurnArea = pTmpPoly
			Segments(NextSegment).pPrimTurnArea = pWSpiral

			' Внутренная сторона    =======================
			If Turndir < 0 Then
				ptOutS = pDefPoints.Point(pDefPoints.PointCount - 1)
			Else
				ptOutS = pDefPoints.Point(0)
			End If

			'DrawPolygon pSecTurnArea
			'Set pRelation = FullSegPoly
			pTmpPoly1 = pClipLine
			If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)

			If b15Degree And (fAlpha > 90.0 - SplayAngl.Value) Then
				pClipLine.FromPoint = PointAlongPlane(NewFIX.pPtPrj, fCurrDir + 180.0, 2.0 * GuidNav2.Range)
				pClipLine.ToPoint = PointAlongPlane(NewFIX.pPtPrj, fCurrDir, 2.0 * GuidNav2.Range)

				If Turndir < 0 Then
					ClipByLine(FullSegPoly, pClipLine, FullSegPoly, Nothing, Nothing)
				Else
					ClipByLine(FullSegPoly, pClipLine, Nothing, FullSegPoly, Nothing)
				End If
			Else
				If pRelation.Contains(ptOutS) Then
					pTmpPoly = pClipLine
					If pTmpPoly.PointCount > 0 Then pTmpPoly.RemovePoints(0, pTmpPoly.PointCount)
					pTmpPoly.AddPoint(PointAlongPlane(ptOutS, fCurrDir + Turndir * 90.0, GuidNav2.Range))
					pTmpPoly.AddPoint(ptOutS)
					pTmpPoly.AddPoint(PointAlongPlane(ptOutS, fNextDir - Turndir * SplayAngl.Value, GuidNav2.Range))

					pTopo = FullSegPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					'DrawPolygon FullSegPoly, RGB(0, 0, 255)
					'DrawPolyLine pTmpPoly, 255, 2
					'DrawPolygon FullSegPoly, RGB(255, 0, 255)
					On Error Resume Next
					If Turndir < 0 Then
						pTopo.Cut(pTmpPoly, pTmpPoly, FullSegPoly)
					Else
						pTopo.Cut(pTmpPoly, FullSegPoly, pTmpPoly)
					End If
					On Error GoTo 0
					If FullSegPoly.PointCount = 0 Then FullSegPoly = pTopo
					'DrawPolygon FullSegPoly, RGB(255, 0, 255)
					'pTmpPoly.RemovePoints 0, pTmpPoly.PointCount

					pTopo = FullSegPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
				Else
					pConstructor = ptTmp
					pTmpPoly = New ArcGeometry.Polygon
					pConstructor.ConstructAngleIntersection(NewFIX.pPtPrj, DegToRad(fNextDir), ptOutS, DegToRad(fNextDir + 0.5 * Turndir * fAlpha))

					pTmpPoly.AddPoint(PointAlongPlane(NewFIX.pPtPrj, fNextDir + 90.0 * Turndir, 200.0))
					pTmpPoly.AddPoint(NewFIX.pPtPrj)
					pTmpPoly.AddPoint(ptOutS)
					pTmpPoly.AddPoint(ptTmp)
					pTmpPoly.AddPoint(PointAlongPlane(ptTmp, fNextDir + 90.0 * Turndir, 200.0))

					pTopo = pTmpPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					pSecTurnArea = pTopo.Union(pSecTurnArea)
					pTopo = pSecTurnArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
				End If
			End If
		Else 'TurnDir = 0
			If SideDef(pMeasureLine.FromPoint, fCurrDir, pMeasureLine.ToPoint) > 0 Then pMeasureLine.ReverseOrientation()

			pPrevClipLine.FromPoint = PointAlongPlane(pMeasureLine.FromPoint, fCurrDir - 90.0, 1.0)
			pPrevClipLine.ToPoint = PointAlongPlane(pMeasureLine.ToPoint, fCurrDir + 90.0, 1.0)

			ClipByLine(FullSegPoly, pPrevClipLine, Nothing, FullSegPoly, pTmpPoly1)
			If FullSegPoly.PointCount = 0 Then
				pClone = Segments(NextSegment).pSecPoly
				FullSegPoly = pClone.Clone
			End If

			ClipByLine(PrimSegPoly, pPrevClipLine, Nothing, PrimSegPoly, pTmpPoly1)
			If PrimSegPoly.PointCount = 0 Then
				pClone = Segments(NextSegment).pPrimPoly
				PrimSegPoly = pClone.Clone
			End If

			fDist = fFarTol2 + d15sec
			Segments(NextSegment).ptD0 = PointAlongPlane(NewFIX.pPtPrj, fNextDir, fDist)
		End If

		'=======================
		pTmpPoly1 = pClipLine
		If pTmpPoly1.PointCount > 0 Then pTmpPoly1.RemovePoints(0, pTmpPoly1.PointCount)
		ptTmp = PointAlongPlane(GuidNav2.pPtPrj, fNextDir, GuidNav2.Range)

		pClipLine.FromPoint = PointAlongPlane(ptTmp, fNextDir + 90.0, GuidNav2.Range)
		pClipLine.ToPoint = PointAlongPlane(ptTmp, fNextDir - 90.0, GuidNav2.Range)

		'=========\/
		pTopo = FullSegPoly
		'Set Segments(NextSegment).pCutSecPoly = pTopo.Union(pSecTurnArea)

		pTmpPoly = pTopo.Union(pSecTurnArea)
		pTopo = pTmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		ClipByLine(pTmpPoly, pClipLine, Nothing, Segments(NextSegment).pCutSecPoly, pTmpPoly1)
		If Segments(NextSegment).pCutSecPoly.PointCount = 0 Then Segments(NextSegment).pCutSecPoly = pTmpPoly1
		'=========/\

		'=========\/
		pTopo = PrimSegPoly
		'Set Segments(NextSegment).pCutPrimPoly = pTopo.Union(pPrimTurnArea)

		pTmpPoly = pTopo.Union(pPrimTurnArea)
		pTopo = pTmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		ClipByLine(pTmpPoly, pClipLine, Nothing, Segments(NextSegment).pCutPrimPoly, pTmpPoly1)
		If Segments(NextSegment).pCutPrimPoly.PointCount = 0 Then Segments(NextSegment).pCutPrimPoly = pTmpPoly1
		'=========/\

		'Set pClone = pPrimTurnArea
		'Set Segments(NextSegment).pCutPrimPoly = pClone.Clone

		Segments(NextSegment).pFullSegElement = DrawPolygon(Segments(NextSegment).pCutSecPoly, RGB(0, 92, 255))
		Segments(NextSegment).pPrimSegElement = DrawPolygon(Segments(NextSegment).pCutPrimPoly, RGB(92, 255, 0))

	End Sub

	Private Sub AddBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles AddBtn.Click
		Dim bFlag As Boolean
		Dim bSameNav As Boolean
		Dim bDefined As Boolean

		Dim I As Integer
		Dim J As Integer
		Dim MaxFL As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim Turndir As Integer
		Dim NextSegment As Integer

		Dim fTmp As Double
		Dim fTmp1 As Double
		Dim fDist As Double
		Dim fAlpha As Double
		Dim fFarTol2 As Double
		Dim fNearTol2 As Double
		Dim fTurnTanD As Double
		Dim fCurrSegMOC As Double

		Dim pClone As esriSystem.IClone
		Dim pClipLine As ArcGeometry.IPolyline
		Dim pMeasureLine As ArcGeometry.IPolyline
		Dim pTmpPoly As ArcGeometry.IPointCollection
		Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pGeomCol As ArcGeometry.IGeometryCollection

		Dim DWPT As TypeDefinitions.NavaidType
		Dim GuidNav2 As TypeDefinitions.NavaidType

		iError = 0
		bDefined = (ComboBox201.SelectedIndex >= 0) And (OptionButton201.Checked Or (ComboBox202.SelectedIndex >= 0) Or OptionButton204.Checked) And ((ComboBox205.SelectedIndex < ComboBox205.Items.Count - 1) Or (ComboBox204.SelectedIndex >= 0))

		If Not bDefined Then
			MessageBox.Show(My.Resources.str0108)
			iError = -1
			Return
		End If

		GuidNav2 = GuidNavDat2(ComboBox201.SelectedIndex)

		If GuidNav2.TypeCode = eNavaidType.VOR Then
			fTmp = (hRoute - GuidNav2.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle)) * System.Math.Sin(DegToRad(2 * VOR.TrackAccuracy))
		Else
			fTmp = (hRoute - GuidNav2.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle)) * System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy + NDB.TrackAccuracy))
		End If

		bSameNav = ReturnDistanceInMeters(Segments(SegmentNum).StartNav.pPtPrj, GuidNav2.pPtPrj) < fTmp

		If (Not bSameNav) Then
			If (ComboBox204.SelectedIndex < 0) Then
				MessageBox.Show(My.Resources.str0108)
				iError = -1
				Return
			End If

			'NewFIX.Name = TextBox204.Text
			'NewFIX.CallSign = NewFIX.Name
		End If

		NewFIX.Name = TextBox204.Text       '???????????????????
		NewFIX.CallSign = NewFIX.Name       '???????????????????

		Segments(SegmentNum).fhFL = CDbl(TextBox001.Text)
		Segments(SegmentNum).FL = CShort(Mid(ComboBox001.Text, 3))

		'If CheckSegment(Segments(SegmentNum).StartFIX, NewFIX, RouteName, MaxFL) Then
		'	If MaxFL <> Segments(SegmentNum).FL Then
		'		MessageBox.Show(My.Resources.str116 + Segments(SegmentNum).StartFIX.Name + "-" + NewFIX.Name + vbCrLf + My.Resources.str117 + RouteName + My.Resources.str118 + CStr(MaxFL))
		'		Return
		'	End If
		'End If

		NextSegment = SegmentNum + 1

		Segments(NextSegment).pSecTurnArea = Nothing
		Segments(NextSegment).pPrimTurnArea = Nothing


		fNearTol = Segments(SegmentNum).fStartNearTol
		fFarTol = Segments(SegmentNum).fStartFarTol
		'    End If

		pClone = SecFIXPoly
		Segments(SegmentNum).pEndFixPoly = pClone.Clone

		Segments(SegmentNum).pStartPtElement = FixPointElement1
		Segments(SegmentNum).pStartFixElement = FixElement1

		FixPointElement1 = FixPointElement2
		FixElement1 = FixElement2

		Segments(SegmentNum).pEndPtElement = FixPointElement2
		Segments(SegmentNum).pEndFixElement = FixElement2

		'If Not Segments(I).pStartPtElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pStartPtElement)
		'If Not Segments(I).pEndPtElement Is Nothing Then pGraphics.DeleteElement(Segments(I).pEndPtElement)
		'On Error Resume Next
		'If Not FixElement2 Is Nothing Then pGraphics.DeleteElement(FixElement2)
		'On Error GoTo 0
		'FixElement2 = DrawPolygon(PrimFIXPoly, RGB(0, 255, 255))
		'=================================================================================

		Segments(SegmentNum).EndNav = GuidNav2

		If bSameNav And (ComboBox204.SelectedIndex < 0) Then
			Segments(SegmentNum).EndInter = GuidNav2
		Else
			Segments(SegmentNum).EndInter = InterNavDat2(ComboBox204.SelectedIndex)
		End If

		Segments(SegmentNum).EndFIX = NewFIX

		'Segments(SegmentNum).EndFIX.Name = TextBox204.Text
		'Segments(SegmentNum).EndFIX.pPtPrj = New ArcGeometry.Point
		'Segments(SegmentNum).EndFIX.pPtPrj.PutCoords(NewFIX.pPtPrj.X, NewFIX.pPtPrj.Y)

		CalcTolerance(NewFIX.pPtPrj, SecFIXPoly, fCurrDir, fNearTol2, fFarTol2)

		Segments(SegmentNum).fEndNearTol = fNearTol2
		Segments(SegmentNum).fEndFarTol = fFarTol2

		fAlpha = Modulus(fNextDir - fCurrDir, 360.0)
		If Modulus(fAlpha, 180.0) < degEps Then
			Turndir = 0
		ElseIf fAlpha > 180.0 Then
			fAlpha = 360.0 - fAlpha
			Turndir = 1
		ElseIf fAlpha > 0.0 Then
			Turndir = -1
		End If

		If fAlpha > MaxTurnAngle.Value Then
			MessageBox.Show(My.Resources.str0119 + CStr(fAlpha))
			iError = -1
			Return
		End If

		Segments(SegmentNum).fDirection = fCurrDir
		Segments(SegmentNum).fTurnAngle = fAlpha
		Segments(SegmentNum).iTurnDir = Turndir
		Segments(SegmentNum).fTurnRadius = fTurnR

		fTurnTanD = fTurnR * System.Math.Tan(DegToRad(0.5 * fAlpha))

		'DrawPoint Segments(SegmentNum).StartFIX.ptPrj, 0
		'DrawPoint Segments(SegmentNum).EndFIX.ptPrj, 255

		fDist = ReturnDistanceInMeters(Segments(SegmentNum).StartFIX.pPtPrj, Segments(SegmentNum).EndFIX.pPtPrj) - fNearTol2
		If fDist - fTurnTanD < 0.0 Then
			MessageBox.Show(My.Resources.str0120)
			iError = -1
			Return
		End If

		'If Not Segments(SegmentNum).ptCOP Is Nothing Then
		'	iCopSide = SideDef(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir + 90.0, Segments(SegmentNum).ptCOP)
		'End If

		CreateGeometry(NextSegment, fTurnTanD, fAlpha, Turndir)

		pClipLine = New ArcGeometry.Polyline()
		pClipLine.FromPoint = PointAlongPlane(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir, 2 * Segments(SegmentNum).fStartFarTol + 2000.0)
		pClipLine.ToPoint = PointAlongPlane(Segments(SegmentNum).StartFIX.pPtPrj, fCurrDir - 180.0, 2 * Segments(SegmentNum).fStartNearTol + 2000.0)
		'DrawPolyLine pClipLine, 255, 2

		pTopo = pClipLine

		pMeasureLine = pTopo.Intersect(Segments(SegmentNum).pStartFixPoly, ArcGeometry.esriGeometryDimension.esriGeometry1Dimension)

		If SideDef(pMeasureLine.FromPoint, fCurrDir - 90.0, pMeasureLine.ToPoint) < 0 Then pMeasureLine.ReverseOrientation()

		fCurrSegMOC = DeConvertHeight(CDbl(ComboBox206.Text))
		Segments(SegmentNum).fMOC = fCurrSegMOC

		I = GetEnrouteObstacles(ObsTableList, EnrouteObstacleList, Segments(SegmentNum).pCutPrimPoly, Segments(SegmentNum).pCutSecPoly, fCurrSegMOC, fCurrDir, pMeasureLine.FromPoint)
		If OptionButton204.Checked Then Segments(SegmentNum).DirWPT.pPtGeo = Nothing

		fTmp = Math.Max(hRouteGuid, hRouteInter)
		If I >= 0 Then
			Segments(SegmentNum).DominantObstacle = EnrouteObstacleList(I)
			Segments(SegmentNum).fRegH = Math.Max(fTmp, EnrouteObstacleList(I).ReqH)
		Else
			Segments(SegmentNum).fRegH = Math.Max(fTmp, fCurrSegMOC)
			Segments(SegmentNum).DominantObstacle.Height = 0.0
			Segments(SegmentNum).DominantObstacle.ReqH = fCurrSegMOC
			Segments(SegmentNum).DominantObstacle.MOC = fCurrSegMOC
			Segments(SegmentNum).DominantObstacle.Index = -1
			Segments(SegmentNum).DominantObstacle.Identifier = Guid.Empty
		End If

		Segments(NextSegment).pStartFixElement = FixElement2
		pClone = SecFIXPoly
		Segments(NextSegment).pStartFixPoly = pClone.Clone

		Segments(NextSegment).StartFIX = Segments(SegmentNum).EndFIX
		Segments(NextSegment).StartInter = Segments(SegmentNum).EndInter

		Segments(NextSegment).StartNav = Segments(SegmentNum).EndNav
		Segments(NextSegment).fStartNearTol = fNearTol2
		Segments(NextSegment).fStartFarTol = fFarTol2

		If MultiPage001.SelectedIndex <> 2 Then '???????????????????????????????????
			fCurrDir = fNextDir

			If OptionButton203.Checked Then
				DWPT = DirWPT2(ComboBox202.SelectedIndex)
				fDist = ReturnDistanceInMeters(GuidNav2.pPtPrj, DWPT.pPtPrj)
				If fDist < fMinCOPDist Then DWPT.TypeCode = -1
			Else
				DWPT = Segments(SegmentNum).EndFIX
			End If

			Segments(NextSegment).DirWPT = DWPT
			'============================================================================

			pTrackLine1.FromPoint = Segments(NextSegment).ptD0
			pCurrSegLine.FromPoint = NewFIX.pPtPrj

			'DrawPointWithText(Segments(NextSegment).ptD0, "ptD0", 0)
			'Application.DoEvents()

			Side1 = SideDef(pTrackLine1.FromPoint, fCurrDir + 90.0, GuidNav2.pPtPrj)

			bFlag = False
			If (DWPT.TypeCode >= eNavaidType.VOR) And (Side1 >= 0) Then
				bFlag = (SideDef(GuidNav2.pPtPrj, fCurrDir + 90.0, DWPT.pPtPrj) > 0)
			End If

			If bFlag Then
				Side2 = SideDef(pTrackLine1.FromPoint, fCurrDir + 90.0, DWPT.pPtPrj)

				If Side1 > 0 Then
					pCurrSegLine.ToPoint = GuidNav2.pPtPrj
					pTrackLine1.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, 0.5) 'GuidNav2.ptPrj 'PointAlongPlane(GuidNav2.ptPrj, fCurrDir + 180.0, fRCone1)
				ElseIf (Side1 < 0) Or (Side2 > 0) Then
					pCurrSegLine.ToPoint = Segments(NextSegment).ptCOP
					pTrackLine1.ToPoint = PointAlongPlane(DWPT.pPtPrj, fCurrDir, 0.5) 'DWPT.ptPrj ' PointAlongPlane(DWPT.ptPrj, fCurrDir + 180.0, fRCone2)
				Else
					pCurrSegLine.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, GuidNav2.Range)
					pTrackLine1.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, GuidNav2.Range)
				End If
			Else
				If Side1 > 0 Then
					pCurrSegLine.ToPoint = GuidNav2.pPtPrj
					pTrackLine1.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, 0.5) 'GuidNav2.ptPrj 'PointAlongPlane(GuidNav2.ptPrj, fCurrDir + 180.0, fRCone1)
				Else
					pCurrSegLine.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, GuidNav2.Range)
					pTrackLine1.ToPoint = PointAlongPlane(GuidNav2.pPtPrj, fCurrDir, GuidNav2.Range)
				End If
			End If
		End If

		'pClone = pCurrSegLine
		'Segments(SegmentNum).pNominalPoly = pClone.Clone

		Segments(SegmentNum).pNominalPoly = New ArcGeometry.Polyline()
		Segments(SegmentNum).pNominalPoly.FromPoint = Segments(SegmentNum).StartFIX.pPtPrj
		Segments(SegmentNum).pNominalPoly.FromPoint = Segments(SegmentNum).EndFIX.pPtPrj

		'DrawPolyLine pCurrSegLine, 255, 2
		'===========================================================================-
		On Error Resume Next
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		On Error GoTo 0

		pTmpPoly = pClipLine
		If pTmpPoly.PointCount > 0 Then pTmpPoly.RemovePoints(0, pTmpPoly.PointCount)

		pClipLine.FromPoint = Segments(SegmentNum).StartFIX.pPtPrj
		pClipLine.ToPoint = Segments(SegmentNum).EndFIX.pPtPrj

		Segments(SegmentNum).pSegLineElement = DrawPolyLineSFS(pClipLine, pLineSym)
		LineElement1 = DrawPolyLineSFS(pCurrSegLine, pLineSym)

		SegmentNum = NextSegment

		fNearTol = fNearTol2
		fFarTol = fFarTol2
		RemoveBtn.Enabled = True
		ReportBtn.Enabled = SegmentNum > 0

		ReDim Preserve Segments(SegmentNum + 1)
		ReportForm.FillReport(Segments)
		ReportForm.AddSegment(Segments(SegmentNum - 1), EnrouteObstacleList)

		NewFixName = GetFixName(NewFixNumber)
		TextBox204.Text = NewFixName
		'GetActiveView().Refresh
		If MultiPage001.SelectedIndex <> 2 Then Fill201()
	End Sub

	Private Sub InfoBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles InfoBtn.Click
		InfoFrm.ShowInfo(Me.Left + Frame001.Left + InfoBtn.Left, Me.Top + Frame001.Top + InfoBtn.Top + InfoBtn.Height)
	End Sub

	Private Sub ShowPanleBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ShowPanelBtn.CheckedChanged
		If ShowPanelBtn.Checked Then
			Me.Width = 810
			ShowPanelBtn.Image = EnRoute.My.Resources.gifHIDE_INFO
		Else
			Me.Width = 650
			ShowPanelBtn.Image = EnRoute.My.Resources.gifSHOW_INFO
		End If

		If NextBtn.Enabled Then
			NextBtn.Focus()
		Else
			CancelBtn.Focus()
		End If
	End Sub

	Public Sub FillReport()
		Dim N As Integer
		Dim I As Integer
		Dim fTmp As Double
		Dim itmX As System.Windows.Forms.ListViewItem

		ListView1.Items.Clear()

		N = UBound(Segments)

		For I = 0 To N - 2
			itmX = ListView1.Items.Add(CStr(I))

			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Segments(I).StartFIX.Name))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Segments(I).EndFIX.Name))

			fTmp = Dir2Azt(Segments(I).StartFIX.pPtPrj, Segments(I).fDirection)
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(fTmp, 1))))

			fTmp = ReturnDistanceInMeters(Segments(I).StartFIX.pPtPrj, Segments(I).EndFIX.pPtPrj)
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(fTmp, 2))))

			itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(Segments(I).fTurnAngle, 1))))

			fTmp = Math.Max(Math.Max(Math.Max(Math.Max(Segments(I).fHGuidE, Segments(I).fHGuidS), Segments(I).fHInterS), Segments(I).fHInterE), Segments(I).DominantObstacle.ReqH)
			itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(fTmp, 2))))
		Next I
	End Sub

	Private Sub NextBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
		Dim CurrVal As Integer
		Dim NextVal As Integer
		Dim N As Integer
		Dim K As Integer

		CurrVal = MultiPage001.SelectedIndex
		NextVal = CurrVal + 1

		Select Case CurrVal
			Case 0
				If ComboBox106.Visible And (UBound(InterNavDat1) < 0) Then
					MessageBox.Show(My.Resources.str0108, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Error)
					Exit Sub
				End If

				N = UBound(StartWPT)
				K = ComboBox105.SelectedIndex
				If K > N Then
					N = N + 1
					If N > 0 Then
						ReDim Preserve StartWPT(N)
					Else
						ReDim StartWPT(N)
					End If

					StartWPT(N) = Segments(0).StartFIX
				End If

				On Error Resume Next
				If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
				On Error GoTo 0
				LineElement1 = DrawPolyLine(pTrackLine1, 255, 2)

				If OptionButton102.Checked Then
					Segments(SegmentNum).DirWPT.TypeCode = -1
					Segments(SegmentNum).DirWPT.pPtGeo = Nothing
					Segments(SegmentNum).DirWPT.pPtPrj = Nothing
				End If

				Segments(SegmentNum).pSecTurnArea = Nothing
				Segments(SegmentNum).pPrimTurnArea = Nothing

				TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs(True))

				Fill201()

				If iError Then Return
				NextBtn.Enabled = SegmentNum > 0

				NewFixName = GetFixName(NewFixNumber)
				TextBox204.Text = NewFixName
			Case 1
				On Error Resume Next
				If Not FullSegElement2 Is Nothing Then pGraphics.DeleteElement(FullSegElement2)
				If Not PrimSegElement2 Is Nothing Then pGraphics.DeleteElement(PrimSegElement2)
				If Not LineElement2 Is Nothing Then pGraphics.DeleteElement(LineElement2)

				If Not FixElement2 Is Nothing Then pGraphics.DeleteElement(FixElement2)
				If Not FixPointElement2 Is Nothing Then pGraphics.DeleteElement(FixPointElement2)

				If Not Segments(SegmentNum).pFullSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pFullSegElement)
				If Not Segments(SegmentNum).pPrimSegElement Is Nothing Then pGraphics.DeleteElement(Segments(SegmentNum).pPrimSegElement)

				On Error GoTo 0
				'GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				'Application.DoEvents()

				CorrectFinal(SegmentNum - 1)

				GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				'Application.DoEvents()

				NextBtn.Text = "&Save"
				NextBtn.Image = EnRoute.My.Resources.gifSave
				FillReport()
			Case 2

				Dim RepFileName As String = ""
				Dim RepFileTitle As String = TextBox101.Text

				If Not ShowSaveDialog(RepFileName, RepFileTitle) Then Return

				Dim pReport As ReportHeader
				pReport.Procedure = RepFileTitle
				pReport.Aerodrome = CurrADHP.Name
				pReport.Database = gAranEnv.ConnectionInfo.Database
				pReport.Category = ComboBox101.Text
				'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition

				SaveProtocol(RepFileName, RepFileTitle, pReport)
				DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml")

				'If SaveReport() Then
				If SaveProcedure(RepFileTitle) Then Me.Close()
				'End If

				Return
		End Select

		MultiPage001.SelectedIndex = NextVal

		' 2007
		FocusStepCaption((MultiPage001.SelectedIndex))
		HelpContextID = 21000 + 100 * (MultiPage001.SelectedIndex + 1)
	End Sub

	Private Function RouteSegmentLeg(ByVal Index As Integer, ByVal RouteFormed As Aran.Aim.Features.Route, ByRef pEndPoint As EnRouteSegmentPoint) As RouteSegment
		Dim pRouteSegment As RouteSegment
		Dim pSegmentPoint As SegmentPoint
		Dim pPointReference As PointReference
		Dim pStartPoint As EnRouteSegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pFIXSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint

		Dim pAngleUse As AngleUse
		Dim pAngleIndication As AngleIndication
		Dim pDistanceIndication As DistanceIndication

		Dim SttGuidNav As NavaidType
		Dim EndGuidNav As NavaidType
		Dim SttIntesectNav As NavaidType
		Dim EndIntesectNav As NavaidType

		Dim pNominalPoly As ArcGeometry.IPolyline
		Dim ptStart As ArcGeometry.IPoint
		Dim ptEnd As ArcGeometry.IPoint

		Dim pDistanceSigned As ValDistanceSigned

		Dim mUomSpeed As UomSpeed
		Dim mUomHDistance As UomDistance
		Dim mUomVDistance As UomDistanceVertical

		Dim uomSpeedTab() As UomSpeed
		Dim uomDistHorzTab() As UomDistance
		Dim uomDistVerTab() As UomDistanceVertical

		Dim fTmp As Double
		Dim fDir As Double
		Dim fDist As Double
		Dim Angle As Double
		Dim fCourseDir As Double
		Dim fDistToNav As Double
		Dim fAltitudeMin As Double

		uomDistHorzTab = {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = {UomDistanceVertical.M, UomDistanceVertical.FT} ', UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER}
		uomSpeedTab = {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		SttGuidNav = Segments(Index).StartNav
		SttIntesectNav = Segments(Index).StartInter
		ptStart = Segments(Index).StartFIX.pPtPrj

		EndGuidNav = Segments(Index).EndNav
		EndIntesectNav = Segments(Index).EndInter
		ptEnd = Segments(Index).EndFIX.pPtPrj


		Segments(Index).StartFIX.pPtPrj.M = 0.0
		Segments(Index).StartFIX.pPtPrj.Z = 0.0

		Segments(Index).EndFIX.pPtPrj.M = 0.0
		Segments(Index).EndFIX.pPtPrj.Z = 0.0

		pNominalPoly = New ArcGeometry.Polyline
		pNominalPoly.FromPoint = ptStart
		pNominalPoly.ToPoint = ptEnd

		'DrawPolyLine(pNominalPoly, , 2)
		'Application.DoEvents()

		fCourseDir = Segments(Index).fDirection
		'=================================================================
		pRouteSegment = pObjectDir.CreateFeature(Of RouteSegment)()
		'pRouteSegment.DesignatorSuffix = CodeRouteDesignatorSuffix.F

		pRouteSegment.Level = 1 - ComboBox101.SelectedIndex   'CodeLevel.LOWER

		pRouteSegment.TrueTrack = Dir2Azt(Segments(Index).StartFIX.pPtPrj, fCourseDir)
		pRouteSegment.ReverseTrueTrack = Dir2Azt(Segments(Index).EndFIX.pPtPrj, fCourseDir + 180.0)

		pRouteSegment.MagneticTrack = Dir2Azt(Segments(Index).StartFIX.pPtPrj, fCourseDir - Segments(Index).StartFIX.MagVar)
		pRouteSegment.ReverseMagneticTrack = Dir2Azt(Segments(Index).EndFIX.pPtPrj, fCourseDir + 180.0 - Segments(Index).EndFIX.MagVar)

		'pRouteSegment.TurnDirection = (1 + Segments(Index).iTurnDir) / 2

		If Segments(Index).iTurnDir = 1 Then
			pRouteSegment.TurnDirection = CodeDirectionTurn.RIGHT
		Else
			pRouteSegment.TurnDirection = CodeDirectionTurn.LEFT
		End If

		pRouteSegment.NavigationType = CodeRouteNavigation.CONV
		pRouteSegment.PathType = CodeRouteSegmentPath.GDS

		''pRouteSegment.RequiredNavigationPerformance 

		pRouteSegment.SignalGap = False
		pRouteSegment.RouteFormed = RouteFormed.GetFeatureRef()

		'New ValDistance(ConvertDistance(pNominalPoly.Length, eRoundMode.rmNERAEST), mUomHDistance)
		'pNominalPoly = Segments(Index).

		pRouteSegment.Length = CreateValDistanceType(mUomHDistance, ConvertDistance(pNominalPoly.Length, eRoundMode.NEAREST))

		fTmp = Math.Max(Math.Max(Segments(Index).fHGuidE, Segments(Index).fHGuidS), Segments(Index).DominantObstacle.ReqH)
		pRouteSegment.LowerLimit = CreateValAltitudeType(mUomVDistance, ConvertHeight(fTmp, eRoundMode.NEAREST))
		pRouteSegment.LowerLimitReference = CodeVerticalReference.MSL

		pRouteSegment.UpperLimit = CreateValAltitudeType(mUomVDistance, ConvertHeight(Segments(Index).fhFL, eRoundMode.NEAREST))
		pRouteSegment.UpperLimitReference = CodeVerticalReference.MSL


		''pRouteSegment.MaximumCrossingAtEnd
		''pRouteSegment.MaximumCrossingAtEndReference

		''pRouteSegment.MinimumCrossingAtEnd
		''pRouteSegment.MinimumCrossingAtEndReference

		''pRouteSegment.MinimumEnrouteAltitude
		''pRouteSegment.MinimumObstacleClearanceAltitude

		'''pRouteSegment.WidthLeft
		'''pRouteSegment.WidthRight

		'pRouteSegment.WorksPackageId

		'=======================================================================================
		'If Not (Segments(Index).ptCOP Is Nothing) Then
		'	pRouteSegment.ValCopDist = CreateDouble(ConvertDistance(Segments(Index).fCopDist, 2))
		'End If
		'pRouteSegment.UomDist = mHUomDistance

		' Start Point ========================
		If Index > 0 Then   ' If Not pEndPoint is Nothing then
			pStartPoint = pEndPoint
		Else
			pStartPoint = New EnRouteSegmentPoint()
			pSegmentPoint = pStartPoint

			'pStartPoint.FlyOver = False
			pStartPoint.Waypoint = True
			pStartPoint.ReportingATC = CodeATCReporting.COMPULSORY
			'pStartPoint.RoleFreeFlight = 
			'pStartPoint.RoleMilitaryTraining = 
			'pStartPoint.RoleRVSM = 

			''pStartPoint.SegmentPointType = SegmentPointType.EnRouteSegmentPoint
			pStartPoint.RadarGuidance = False

			'pStartPoint.ExtendedServiceVolume

			' ========================
			pGuidNavSignPt = New SignificantPoint()
			pGuidNavSignPt.NavaidSystem = SttGuidNav.GetFeatureRef()

			pInterNavSignPt = New SignificantPoint()
			pInterNavSignPt.NavaidSystem = SttIntesectNav.GetFeatureRef()

			' Start Point ==================================================
			pPointReference = New PointReference()

			'If GuidNav.TypeCode = eNavaidType.CodeDME Then
			'	fCourseDir = ReturnAngleInDegrees(sttGuidNav.pPtPrj, ptStart) + 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW))	  'Azt2DirPrj(ptStart, pSegmentLeg.Course)
			'End If

			If ReturnDistanceInMeters(ptStart, SttIntesectNav.pPtPrj) < distEps Then
				SttIntesectNav.IntersectionType = eIntersectionType.OnNavaid
			End If

			If SttIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
				pFIXSignPt = pInterNavSignPt
				pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
			Else
				pFixDesignatedPoint = CreateDesignatedPoint(ptStart, , fCourseDir)
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				fDir = ReturnAngleInDegrees(SttGuidNav.pPtPrj, ptStart)
				Angle = Modulus(Dir2Azt(SttGuidNav.pPtPrj, fDir) - SttGuidNav.MagVar, 360.0)

				pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse = New AngleUse()
				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = True

				pPointReference.FacilityAngle.Add(pAngleUse)

				If SttIntesectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(SttIntesectNav.pPtPrj, ptStart)
					fAltitudeMin = ptStart.Z - SttIntesectNav.pPtPrj.Z

					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
					pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
					pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
					pPointReference.Role = CodeReferenceRole.RAD_DME
				Else
					pAngleUse = New AngleUse
					fDir = ReturnAngleInDegrees(SttIntesectNav.pPtPrj, ptStart)

					Angle = Modulus(Dir2Azt(SttIntesectNav.pPtPrj, fDir) - SttIntesectNav.MagVar, 360.0)
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
			'Dim pTolerArea As ArcGeometry.IPolygon
			'pTolerArea = Segments(Index).pStartFixPoly

			If Not Segments(Index).pStartFixPoly Is Nothing Then
				'PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)
				pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				pDistanceSigned.Uom = mUomHDistance
				pDistanceSigned.Value = Math.Abs(ConvertDistance(Segments(Index).fStartNearTol, eRoundMode.NEAREST))
				pPointReference.PriorFixTolerance = pDistanceSigned

				pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				pDistanceSigned.Uom = mUomHDistance
				pDistanceSigned.Value = Math.Abs(ConvertDistance(Segments(Index).fStartFarTol, eRoundMode.NEAREST))

				pPointReference.PostFixTolerance = pDistanceSigned
				pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(Segments(Index).pStartFixPoly))
			End If
			'=================================================================
			pStartPoint.FacilityMakeup.Add(pPointReference)
			pSegmentPoint.PointChoice = pFIXSignPt
		End If

		pRouteSegment.Start = pStartPoint

		'pRouteSegment.R_SignifigantPointStart = mt
		' End Of Start Point ========================

		' EndPoint ========================
		pEndPoint = New EnRouteSegmentPoint()
		pSegmentPoint = pEndPoint

		'pEndPoint.FlyOver = False
		pEndPoint.Waypoint = True
		pEndPoint.ReportingATC = CodeATCReporting.COMPULSORY
		'pStartPoint.RoleFreeFlight = 
		'pStartPoint.RoleMilitaryTraining = 
		'pStartPoint.RoleRVSM = 

		''pStartPoint.SegmentPointType = SegmentPointType.EnRouteSegmentPoint
		pEndPoint.RadarGuidance = False

		'pStartPoint.ExtendedServiceVolume

		If Segments(Index).iTurnDir <> 0 Then
			pEndPoint.TurnRadius = CreateValDistanceType(mUomHDistance, ConvertDistance(Segments(Index).fTurnRadius, eRoundMode.NEAREST))
		End If

		' ========================
		pGuidNavSignPt = New SignificantPoint()
		pGuidNavSignPt.NavaidSystem = EndGuidNav.GetFeatureRef()

		pInterNavSignPt = New SignificantPoint()
		pInterNavSignPt.NavaidSystem = EndIntesectNav.GetFeatureRef()

		' End Point ==================================================
		pPointReference = New PointReference()

		If ReturnDistanceInMeters(ptEnd, EndIntesectNav.pPtPrj) < distEps Then
			EndIntesectNav.IntersectionType = eIntersectionType.OnNavaid
		End If

		If EndIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, , fCourseDir)
			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			fDir = ReturnAngleInDegrees(EndGuidNav.pPtPrj, ptEnd)
			Angle = Modulus(Dir2Azt(EndGuidNav.pPtPrj, fDir) - EndGuidNav.MagVar, 360.0)

			pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
			pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
			pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
			pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

			pAngleUse = New AngleUse()
			pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
			pAngleUse.AlongCourseGuidance = True

			pPointReference.FacilityAngle.Add(pAngleUse)

			If EndIntesectNav.TypeCode = eNavaidType.DME Then
				fDistToNav = ReturnDistanceInMeters(EndIntesectNav.pPtPrj, ptEnd)
				fAltitudeMin = ptEnd.Z - EndIntesectNav.pPtPrj.Z

				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
				pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
				pPointReference.Role = CodeReferenceRole.RAD_DME
			Else
				pAngleUse = New AngleUse
				fDir = ReturnAngleInDegrees(EndIntesectNav.pPtPrj, ptEnd)

				Angle = Modulus(Dir2Azt(EndIntesectNav.pPtPrj, fDir) - EndIntesectNav.MagVar, 360.0)
				pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = False

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If
		End If
		'=================================================================

		If Not Segments(Index).pEndFixPoly Is Nothing Then
			pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(Segments(Index).fEndNearTol, eRoundMode.NEAREST))
			pPointReference.PriorFixTolerance = pDistanceSigned

			pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(Segments(Index).fEndFarTol, eRoundMode.NEAREST))

			pPointReference.PostFixTolerance = pDistanceSigned
			pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(Segments(Index).pEndFixPoly))
		End If
		'=================================================================
		pEndPoint.FacilityMakeup.Add(pPointReference)
		pSegmentPoint.PointChoice = pFIXSignPt
		pRouteSegment.End = pEndPoint
		' End of EndPoint ========================

		' Obstacle Assessment Area ===============
		Dim i As Integer
		Dim pEvaluationArea As ObstacleAssessmentArea
		Dim pSurface As Features.Surface
		Dim pMultipolygon As AranGeometry.MultiPolygon
		Dim pObst As Obstruction

		pEvaluationArea = New ObstacleAssessmentArea()
		'pEvaluationArea.AircraftCategory

		If Not Segments(Index).DominantObstacle.pFeature Is Nothing Then
			pObst = New Obstruction()
			pObst.Controlling = True
			'pObst.VerticalStructureObstruction = Segments(Index).DominantObstacle.pFeature.GetFeatureRef()
			pEvaluationArea.SignificantObstacle.Add(pObst)
		End If

		pEvaluationArea.SurfaceZone = CodeObstructionIdSurfaceZone.PRIMARY
		pEvaluationArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		pSurface = New Features.Surface()

		pMultipolygon = ESRIPolygonToARANPolygon(ToGeo(Segments(Index).pCutSecPoly))
		For i = 0 To pMultipolygon.Count - 1
			pSurface.Geo.Add(pMultipolygon(i))
		Next

		pMultipolygon = ESRIPolygonToARANPolygon(ToGeo(Segments(Index).pCutPrimPoly))
		For i = 0 To pMultipolygon.Count - 1
			pSurface.Geo.Add(pMultipolygon(i))
		Next

		pEvaluationArea.Surface = pSurface
		pRouteSegment.EvaluationArea = pEvaluationArea

		' Trajectory =====================================================
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLocation As AranGeometry.Point
		Dim pLineStringSegment As AranGeometry.LineString

		Dim pPolyline As ArcGeometry.IGeometryCollection
		Dim pPath As ArcGeometry.IPointCollection

		pPolyline = pNominalPoly
		pCurve = New Curve

		For J = 0 To pPolyline.GeometryCount - 1
			pPath = pPolyline.Geometry(J)
			pLineStringSegment = New AranGeometry.LineString

			For i = 0 To pPath.PointCount - 1
				pLocation = ESRIPointToARANPoint(ToGeo(pPath.Point(i)))
				pLineStringSegment.Add(pLocation)
			Next i
			pCurve.Geo.Add(pLineStringSegment)
		Next J

		pRouteSegment.CurveExtent = pCurve
		' END =====================================================

		Return pRouteSegment
	End Function

	Private Function SaveProcedure(procName As String) As Boolean
		Dim I As Integer
		Dim SecondLetter As Char

		Dim enrt As Aran.Aim.Features.Route
		Dim pRouteSegment As RouteSegment
		Dim pEndPoint As EnRouteSegmentPoint = Nothing

		Dim rnd As Random = New Random()

		pObjectDir.ClearAllFeatures()

		enrt = pObjectDir.CreateFeature(Of Route)()
		enrt.Name = Char.ToUpperInvariant(procName.Substring(1))

		SecondLetter = Char.ToUpperInvariant(procName.ToCharArray()(0))
		enrt.DesignatorSecondLetter = CType(Asc(SecondLetter) - Asc("A"), CodeRouteDesignatorLetter)
		enrt.DesignatorNumber = 1 + rnd.Next(998)

		enrt.DesignatorPrefix = ComboBox101.SelectedIndex

		'If enroute_cmbox.SelectedIndex > 0 Then
		'	Dim pEnRoute As Aran.Aim.Features.Route
		'	pEnRoute = pEnRouteList(enroute_cmbox.SelectedIndex)
		'	enrt.Id = pEnRoute.Id
		'	enrt.DesignatorPrefix = pEnRoute.DesignatorPrefix
		'	'enrt.DesignatorSecondLetter = pEnRoute.DesignatorSecondLetter
		'	enrt.DesignatorNumber = pEnRoute.DesignatorNumber
		'	enrt.Type = pEnRoute.Type
		'Else
		'	enrt.Type = CodeRoute.ATS
		'End If

		enrt.Type = CodeRoute.ATS

		pObjectDir.SetFeature(enrt)

		For I = 0 To SegmentNum - 1
			pRouteSegment = RouteSegmentLeg(I, enrt, pEndPoint)
			pObjectDir.SetFeature(pRouteSegment)
		Next I

		Try
			Try
				pObjectDir.AddCreatedRefToSrcLocalStorage()
			Catch exc As Exception
				MsgBox("Error on find ref features." + vbCrLf + exc.Message)
				Return False
			End Try

			pObjectDir.SetRootFeatureType(FeatureType.Route)

			Dim result As Boolean
			result = pObjectDir.Commit({FeatureType.Route, FeatureType.DesignatedPoint, FeatureType.AngleIndication, FeatureType.DistanceIndication, FeatureType.RouteSegment})

			If result Then
				pObjectDir.SaveSrcLocalStorage("")
				gAranEnv.RefreshAllAimLayers()
			End If
		Catch ex As Exception
			MsgBox("Error on commit." + vbCrLf + ex.Message)
			Return False
		End Try

		Return True
	End Function

	Private Function SaveReport() As Boolean
		Dim RepFileName As String = ""
		Dim RepFileTitle As String = TextBox101.Text

		If Not ShowSaveDialog(RepFileName, RepFileTitle) Then Return False

		Dim pReport As ReportHeader
		pReport.Procedure = RepFileTitle
		pReport.Aerodrome = CurrADHP.Name
		pReport.Database = gAranEnv.ConnectionInfo.Database
		'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition
		pReport.Category = ComboBox101.Text

		SaveProtocol(RepFileName, RepFileTitle, pReport)
		DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml")
		Return True
	End Function

	Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String, pReport As ReportHeader)
		Dim ProtRep As New ReportFile
		Dim I As Integer

		ProtRep.OpenFile(RepFileName + "_Protocol", My.Resources.str0510)
		'ProtRep.WriteMessage(My.Resources.str0510)
		ProtRep.WriteMessage(My.Resources.str0001 + " - " + RepFileTitle + ": " + My.Resources.str0509)
		ProtRep.WriteHeader(pReport)


		ProtRep.WriteMessage()
		ProtRep.WriteMessage()

		For I = 0 To SegmentNum - 1
			ProtRep.WriteMessage("Segment " + CStr(I + 1))
			ProtRep.WriteMessage("MAA = " + CStr(Segments(I).FL) + " " + My.Resources.str0067)

			ProtRep.WriteMessage()
			ProtRep.WriteSegmentData(SegmentData(I).Segment)
			ProtRep.WriteMessage()
			ProtRep.WriteObstData(SegmentData(I).Obstacles, -1)
		Next

		ProtRep.CloseFile()
		ProtRep = Nothing
	End Sub

	Private Sub OptionButton102_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton102.CheckedChanged
		If Not bFormInitialised Then Return

		If eventSender.Checked Then
			ComboBox103.Enabled = False
			ComboBox103.Items.Clear()

			Label113.Visible = Not OptionButton102.Checked
			Image101.Visible = Not OptionButton102.Checked
			CheckBox101.Visible = Not OptionButton102.Checked
			TextBox104.Visible = Not OptionButton102.Checked
			TextBox105.Visible = Not OptionButton102.Checked

			ComboBox104T.Text = ComboBox104.Text
			ComboBox104.Visible = Not OptionButton102.Checked
			ComboBox104T.Visible = OptionButton102.Checked

			'    ComboBox104.Style = fmStyleDropDownCombo
			'    ComboBox104.ShowDropButtonWhen = fmShowDropButtonWhenNever
			'    If SegmentNum > 0 Then Exit Sub
			Segments(SegmentNum).DirWPT.Index = -1
			ComboBox104.Tag = ""
			ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
		End If
	End Sub

	Private Sub OptionButton101_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton101.CheckedChanged
		If Not bFormInitialised Then Return

		If eventSender.Checked Then
			ComboBox103.Enabled = True
			ComboBox104T.Text = ComboBox104.Text
			ComboBox104.Visible = OptionButton101.Checked
			ComboBox104T.Visible = Not OptionButton101.Checked
			'    ComboBox104.Style = fmStyleDropDownList
			'    ComboBox104.ShowDropButtonWhen = fmShowDropButtonWhenAlways
			'    If SegmentNum > 0 Then Exit Sub
			ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
		End If
	End Sub

	Private Sub OptionButton103_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton103.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim I As Integer
		Dim N As Integer

		Dim fDist As Double

		Dim GNav As TypeDefinitions.NavaidType
		Dim pLine As ArcGeometry.IPolyline

		GNav = GuidNavDat1(ComboBox102.SelectedIndex)
		If GNav.TypeCode = eNavaidType.VOR Then
			fDist = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		ElseIf GNav.TypeCode = eNavaidType.NDB Then
			fDist = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		Else
			fDist = 0.0
		End If

		pLine = New ArcGeometry.Polyline
		pLine.FromPoint = pGuidPolyLine1.FromPoint
		pLine.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir + 180.0, fDist)

		On Error Resume Next
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		On Error GoTo 0

		LineElement1 = DrawPolyLine(pLine, 255, 2)

		InterNavDat1 = GetValidEnRouteNavs(GNav, hRoute, pLine, 15000.0)

		N = UBound(InterNavDat1)
		ComboBox106.Items.Clear()

		If N >= 0 Then
			For I = 0 To N
				ComboBox106.Items.Add(InterNavDat1(I).CallSign)
			Next I
			ComboBox106.SelectedIndex = 0
		End If
	End Sub

	Private Sub OptionButton104_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton104.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim I As Integer
		Dim N As Integer
		Dim fDist As Double
		Dim GNav As TypeDefinitions.NavaidType
		Dim pLine As ArcGeometry.IPolyline

		GNav = GuidNavDat1(ComboBox102.SelectedIndex)
		If GNav.TypeCode = eNavaidType.VOR Then
			fDist = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		ElseIf GNav.TypeCode = eNavaidType.NDB Then
			fDist = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hRoute - GNav.pPtGeo.Z)
		Else
			fDist = 0.0
		End If

		pLine = New ArcGeometry.Polyline
		pLine.FromPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, fDist)
		pLine.ToPoint = pGuidPolyLine1.ToPoint

		On Error Resume Next
		If Not LineElement1 Is Nothing Then pGraphics.DeleteElement(LineElement1)
		On Error GoTo 0

		LineElement1 = DrawPolyLine(pLine, 255, 2)

		InterNavDat1 = GetValidEnRouteNavs(GNav, hRoute, pLine, 15000.0)

		N = UBound(InterNavDat1)
		ComboBox106.Items.Clear()

		If N >= 0 Then
			For I = 0 To N
				ComboBox106.Items.Add(InterNavDat1(I).CallSign)
			Next I
			ComboBox106.SelectedIndex = 0
		End If
	End Sub

	Private Sub OptionButton105_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton105.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		ComboBox106_SelectedIndexChanged(ComboBox106, New System.EventArgs())
	End Sub

	Private Sub OptionButton106_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton106.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		ComboBox106_SelectedIndexChanged(ComboBox106, New System.EventArgs())
	End Sub

	Private Sub OptionButton201_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton201.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		TextBox206.ReadOnly = False
		TextBox206.BackColor = SystemColors.Window

		OptionButton203.Visible = True
		OptionButton204.Visible = True
		'ComboBox202.Visible = True
		'ComboBox201.Enabled = True
		'ComboBox202.Enabled = True
		ComboBox207.Visible = True

		'Label203.Visible = True
		Label211.Visible = True
		TextBox204.Visible = True
		Label217.Visible = True

		Label212.Visible = False
		Label213.Visible = False
		Label214.Visible = False

		TextBox205.Visible = False
		OptionButton205.Visible = False
		OptionButton206.Visible = False

		Label215.Visible = False
		ComboBox205.Visible = False

		Fill201()
		NextBtn.Enabled = False
	End Sub

	Private Sub OptionButton202_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton202.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return
		'    OptionButton203.Value = True

		TextBox206.ReadOnly = True
		TextBox206.BackColor = SystemColors.ButtonFace

		OptionButton203.Visible = False
		OptionButton204.Visible = False
		'ComboBox202.Visible = False
		'ComboBox201.Enabled = False
		'ComboBox202.Enabled = False
		ComboBox207.Visible = False

		'Label203.Visible = False
		'Label211.Visible = False
		'TextBox204.Visible = False
		'Label217.Visible = False

		Label209.Visible = False
		CheckBox201.Visible = False
		TextBox202.Visible = False
		TextBox203.Visible = False
		Image201.Visible = False

		'Label212.Visible = True
		'Label213.Visible = True
		Label214.Visible = True

		'TextBox205.Visible = True
		OptionButton205.Visible = True
		OptionButton206.Visible = True

		Label215.Visible = True
		ComboBox205.Visible = True

		bText2 = True
		fNextDir = fCurrDir
		Fill201()
		NextBtn.Enabled = True 'SegmentNum > 0
	End Sub

	Private Sub OptionButton203_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton203.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		'Label209.Visible = True
		'Image201.Visible = True
		'CheckBox201.Visible = True
		'TextBox202.Visible = True
		'TextBox203.Visible = True

		ComboBox202.Enabled = True
		If ComboBox202.Items.Count >= 0 Then
			If ComboBox202.SelectedIndex < 0 Then
				ComboBox202.SelectedIndex = 0
			Else
				ComboBox202_SelectedIndexChanged(ComboBox202, New System.EventArgs())
			End If
		End If

		TextBox206.BackColor = SystemColors.ButtonFace
		TextBox206.ReadOnly = True

		TextBox206.Tag = ""
		TextBox206_Validating(TextBox206, Nothing)
	End Sub

	Private Sub OptionButton204_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton204.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Label209.Visible = False
		Image201.Visible = False
		CheckBox201.Visible = False
		TextBox202.Visible = False
		TextBox203.Visible = False
		ComboBox202.Enabled = False
		Segments(SegmentNum + 1).DirWPT.Index = -1

		TextBox206.BackColor = SystemColors.Window
		TextBox206.ReadOnly = False
		TextBox206.Tag = ""
		TextBox206_Validating(TextBox206, Nothing)
	End Sub

	Private Sub ReportBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReportBtn.CheckedChanged
		If ReportBtn.Checked Then
			ReportForm.Show(s_Win32Window)
		Else
			ReportForm.Hide()
		End If
	End Sub

	Private Sub TextBox101_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox101.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		If KeyAscii = 13 Then TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs(False))
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub

	Private Sub TextBox101_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox101.Validating
		If CheckRouteName(TextBox101.Text) Then
			MessageBox.Show(My.Resources.str0122 + TextBox101.Text + My.Resources.str0123)
			TextBox101.Text = RouteName
		Else
			RouteName = TextBox101.Text
		End If
	End Sub

	Private Sub TextBox102_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox102.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, TextBox102.Text)
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox102_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox102.Validating
		'Dim pTopo As ArcGeometry.ITopologicalOperator2
		Dim pConstruct As ArcGeometry.IConstructPoint
		Dim pGeo As ArcGeometry.IGeometry
		Dim pClone As esriSystem.IClone

		Dim GNav As TypeDefinitions.NavaidType
		Dim DWPT As TypeDefinitions.NavaidType = New NavaidType()
		Dim StWPT As TypeDefinitions.NavaidType = New NavaidType()
		Dim InterNav As TypeDefinitions.NavaidType

		Dim Side1 As Integer
		Dim Side2 As Integer

		'Dim fRadius As Double
		Dim fVar As Double
		Dim fTmp As Double
		Dim fDistInter As Double
		Dim fDistGuid As Double
		If Not IsNumeric(TextBox102.Text) Then Return

		fVar = CDbl(TextBox102.Text)

		GNav = GuidNavDat1(ComboBox102.SelectedIndex)

		InterNav = InterNavDat1(ComboBox106.SelectedIndex)

		StWPT.Name = TextBox103.Text
		StWPT.Index = -1
		StWPT.TypeCode = eNavaidType.NONE
		'StWPT.pPtPrj = Nothing

		If InterNav.TypeCode = eNavaidType.DME Then
			fTmp = fCurrDir
			If OptionButton105.Checked Then fTmp = Modulus(fCurrDir + 180.0, 360.0)
			fVar = DeConvertDistance(fVar)
			CircleVectorIntersect(InterNav.pPtPrj, fVar, GNav.pPtPrj, fTmp, StWPT.pPtPrj)
		Else
			fVar = Azt2Dir(InterNav.pPtGeo, fVar)
			If InterNav.TypeCode = eNavaidType.NDB Then fVar = Modulus(fVar + 180.0, 360.0)
			StWPT.pPtPrj = New ArcGeometry.Point
			pConstruct = StWPT.pPtPrj
			pConstruct.ConstructAngleIntersection(GNav.pPtPrj, DegToRad(fCurrDir), InterNav.pPtPrj, DegToRad(fVar))
		End If

		StWPT.pPtPrj.Z = hRoute
		StWPT.pPtPrj.M = 0.0

		StWPT.pPtGeo = New ArcGeometry.Point
		StWPT.pPtGeo.PutCoords(StWPT.pPtPrj.X, StWPT.pPtPrj.Y)
		pGeo = StWPT.pPtGeo
		pGeo.SpatialReference = pSpRefPrj
		pGeo.Project(pSpRefShp)

		StWPT.pPtGeo.Z = hRoute
		StWPT.pPtGeo.M = 0.0

		If OptionButton101.Checked Then

			DWPT = DirWPT1(ComboBox103.SelectedIndex)
			fTmp = ReturnDistanceInMeters(GNav.pPtPrj, DWPT.pPtPrj)
			If fTmp < fMinCOPDist Then DWPT.TypeCode = -1
			Side2 = SideDef(StWPT.pPtPrj, fCurrDir + 90.0, DWPT.pPtPrj)
		Else
			DWPT.TypeCode = -1
		End If

		'pFixPolyLine1.FromPoint = StWPT.ptPrj

		Side1 = SideDef(StWPT.pPtPrj, fCurrDir + 90.0, GNav.pPtPrj)

		'If GNav.TypeCode = eNavaidType.CodeVOR Then
		'	fRadius = (hRoute - GNav.pPtGeo.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
		'Else
		'	fRadius = (hRoute - GNav.pPtGeo.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
		'End If

		'DrawPoint Segments(SegmentNum).ptCOP
		If DWPT.TypeCode >= eNavaidType.VOR Then
			If Side1 > 0 Then
				'pCurrSegLine.ToPoint = GNav.ptPrj
				pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, 0.5) 'GNav.ptPrj ' PointAlongPlane(GNav.ptPrj, fCurrDir + 180.0, fRadius)
			ElseIf (Side1 < 0) Or (Side2 > 0) Then
				'pCurrSegLine.ToPoint = Segments(SegmentNum).ptCOP
				pTrackLine1.ToPoint = DWPT.pPtPrj
			Else
				'pCurrSegLine.ToPoint = PointAlongPlane(GNav.ptPrj, fCurrDir, GNav.Range)
				pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
			End If
		Else
			If Side1 > 0 Then
				'pCurrSegLine.ToPoint = GNav.ptPrj
				pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, 0.5) 'GNav.ptPrj ' PointAlongPlane(GNav.ptPrj, fCurrDir + 180.0, fRadius)
			Else
				'pCurrSegLine.ToPoint = PointAlongPlane(GNav.ptPrj, fCurrDir, GNav.Range)
				pTrackLine1.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
			End If
		End If

		'===========================================================================
		PrimFIXPoly = CreateFixArea(GNav, InterNav, StWPT)

		CalcTolerance(StWPT.pPtPrj, PrimFIXPoly, fCurrDir, fNearTol, fFarTol)
		Segments(0).ptD0 = PointAlongPlane(StWPT.pPtPrj, fCurrDir, fFarTol)

		pTrackLine1.FromPoint = Segments(0).ptD0

		pClone = PrimFIXPoly

		Segments(0).pStartFixPoly = pClone.Clone
		Segments(0).StartFIX = StWPT
		Segments(0).StartNav = GNav
		Segments(0).StartInter = InterNav
		Segments(0).fStartNearTol = fNearTol
		Segments(0).fStartFarTol = fFarTol

		On Error Resume Next
		If Not FixElement1 Is Nothing Then pGraphics.DeleteElement(FixElement1)
		If Not FixPointElement1 Is Nothing Then pGraphics.DeleteElement(FixPointElement1)
		On Error GoTo 0

		FixElement1 = DrawPolygon(PrimFIXPoly, RGB(255, 0, 255))

		FixPointElement1 = DrawPoint(StWPT.pPtPrj, 255)

		Segments(0).pStartFixElement = FixElement1
		'===========================================================================
		fDistInter = ReturnDistanceInMeters(StWPT.pPtPrj, InterNav.pPtPrj)
		fDistGuid = ReturnDistanceInMeters(StWPT.pPtPrj, GNav.pPtPrj)

		hRouteInter = (fDistInter / 4130.0) * (fDistInter / 4130.0) + InterNav.pPtPrj.Z
		hRouteGuid = (fDistGuid / 4130.0) * (fDistGuid / 4130.0) + GNav.pPtPrj.Z

		Segments(0).fHGuidS = hRouteGuid
		Segments(0).fHInterS = hRouteInter

		InfoFrm.SetInterNav(InterNav)
		InfoFrm.SetNearTol(fNearTol, True)

		InfoFrm.SetGuidNavReqH(hRouteGuid)
		InfoFrm.SetInterNavReqH(hRouteInter)
	End Sub

	Private Sub TextBox104_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox104.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox104_Validating(TextBox104, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (TextBox104.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox104_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox104.Validating
		Dim fDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double

		If Not IsNumeric(TextBox104.Text) Then Return
		If TextBox104.Text = TextBox104.Tag Then Return
		TextBox104.Tag = TextBox104.Text

		fDist1 = DeConvertDistance(CDbl(TextBox104.Text))
		fDist = ReturnDistanceInMeters(GuidNavDat1(ComboBox102.SelectedIndex).pPtPrj, DirWPT1(ComboBox103.SelectedIndex).pPtPrj)
		fDist2 = fDist - fDist1
		TextBox105.Text = CStr(ConvertDistance(fDist2, 2))
		TextBox105.Tag = TextBox105.Text

		bText1 = True
		ComboBox104.Tag = ""
		ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
	End Sub

	Private Sub TextBox105_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox105.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox105_Validating(TextBox105, New System.ComponentModel.CancelEventArgs(False))
		Else
			TextBoxFloat(KeyAscii, (TextBox105.Text))
			KeyAscii = Chr(0)
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox105_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox105.Validating
		Dim fDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double
		If Not IsNumeric(TextBox105.Text) Then Return
		If TextBox105.Text = TextBox105.Tag Then Return
		TextBox105.Tag = TextBox105.Text

		fDist2 = DeConvertDistance(CDbl(TextBox105.Text))
		fDist = ReturnDistanceInMeters(GuidNavDat1(ComboBox102.SelectedIndex).pPtPrj, DirWPT1(ComboBox103.SelectedIndex).pPtPrj)
		fDist1 = fDist - fDist2
		TextBox104.Text = CStr(ConvertDistance(fDist1, 2))
		TextBox104.Tag = TextBox104.Text
		bText1 = True
		ComboBox104.Tag = ""
		ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
	End Sub

	Private Sub TextBox201_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox201.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox201_Validating(TextBox201, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (TextBox201.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox201_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox201.Validating
		If Not IsNumeric(TextBox201.Text) Then Return
		fNeardLmax = DeConvertDistance(CDbl(TextBox201.Text))
	End Sub

	Private Sub TextBox202_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox202.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox202_Validating(TextBox202, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (TextBox202.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox202_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox202.Validating
		Dim fDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double

		If Not (TextBox202.Enabled And IsNumeric(TextBox202.Text)) Then Return
		If TextBox202.Tag = TextBox202.Text Then Return
		TextBox202.Tag = TextBox202.Text

		fDist1 = DeConvertDistance(CDbl(TextBox202.Text))
		fDist = ReturnDistanceInMeters(GuidNavDat2(ComboBox201.SelectedIndex).pPtPrj, DirWPT2(ComboBox202.SelectedIndex).pPtPrj)
		fDist2 = fDist - fDist1
		TextBox203.Text = CStr(ConvertDistance(fDist2, 2))
		TextBox203.Tag = TextBox203.Text
		bText2 = True

		TextBox206.Tag = ""
		TextBox206_Validating(TextBox206, Nothing)
	End Sub

	Private Sub TextBox203_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox203.KeyPress
		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox203_Validating(TextBox203, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, (TextBox203.Text))
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox203_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox203.Validating
		Dim fDist As Double
		Dim fDist1 As Double
		Dim fDist2 As Double

		If Not (TextBox203.Enabled And IsNumeric(TextBox203.Text)) Then Return
		If TextBox203.Tag = TextBox203.Text Then Return
		TextBox203.Tag = TextBox203.Text

		fDist1 = DeConvertDistance(CDbl(TextBox203.Text))
		fDist = ReturnDistanceInMeters(GuidNavDat2(ComboBox201.SelectedIndex).pPtPrj, DirWPT2(ComboBox202.SelectedIndex).pPtPrj)

		fDist2 = fDist - fDist1
		TextBox202.Text = CStr(ConvertDistance(fDist2, 2))
		TextBox202.Tag = TextBox202.Text

		bText2 = True

		TextBox206.Tag = ""
		TextBox206_Validating(TextBox206, Nothing)
	End Sub

	Private Sub TextBox205_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox205.KeyPress
		If TextBox205.ReadOnly Then Return

		Dim KeyAscii As Char = eventArgs.KeyChar
		If KeyAscii = Chr(13) Then
			TextBox205_Validating(TextBox205, New System.ComponentModel.CancelEventArgs(False))
			KeyAscii = Chr(0)
		Else
			TextBoxFloat(KeyAscii, TextBox205.Text)
		End If

		eventArgs.KeyChar = KeyAscii
		If KeyAscii = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox205_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox205.Validating
		Dim pConstruct As ArcGeometry.IConstructPoint
		Dim pGeo As ArcGeometry.IGeometry

		Dim InterNav As TypeDefinitions.NavaidType
		Dim GNav As TypeDefinitions.NavaidType
		Dim DWPT As TypeDefinitions.NavaidType = New NavaidType()

		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim I As Integer

		Dim fVar As Double
		Dim fTmp As Double

		'Dim hRouteInter As Double
		'Dim hRouteGuid As Double

		Dim fDistInter As Double
		Dim fDistGuid As Double

		Dim fHmax As Double
		'Dim fHmin As Double

		If TextBox205.ReadOnly Then Return
		If OptionButton201.Checked Then Return

		GNav = GuidNavDat2(ComboBox201.SelectedIndex)

		InterNav = InterNavDat2(ComboBox204.SelectedIndex)

		If (InterNav.Index = GNav.Index) And (InterNav.TypeCode >= eNavaidType.VOR) And (InterNav.TypeCode <> eNavaidType.DME) Then
			NewFIX = GNav
		Else
			If Not IsNumeric(TextBox205.Text) Then Return

			fVar = CDbl(TextBox205.Text)

			NewFIX.Name = TextBox204.Text
			NewFIX.CallSign = NewFIX.Name

			NewFIX.TypeCode = eNavaidType.NONE
			NewFIX.Index = -1

			If InterNav.TypeCode = eNavaidType.DME Then
				fVar = DeConvertDistance(fVar)
				fTmp = fCurrDir
				If OptionButton205.Checked Then fTmp = Modulus(fCurrDir + 180.0, 360.0)

				CircleVectorIntersect(InterNav.pPtPrj, fVar, GNav.pPtPrj, fTmp, NewFIX.pPtPrj)
			Else
				fVar = Azt2Dir(InterNav.pPtGeo, fVar) '????????????????????????
				If InterNav.TypeCode = eNavaidType.NDB Then fVar = Modulus(fVar + 180.0, 360.0)
				NewFIX.pPtPrj = New ArcGeometry.Point
				pConstruct = NewFIX.pPtPrj
				pConstruct.ConstructAngleIntersection(GNav.pPtPrj, DegToRad(fCurrDir), InterNav.pPtPrj, DegToRad(fVar))
			End If

			NewFIX.pPtGeo = New ArcGeometry.Point
			NewFIX.pPtGeo.PutCoords(NewFIX.pPtPrj.X, NewFIX.pPtPrj.Y)
			NewFIX.pPtPrj.Z = hRoute
			NewFIX.pPtPrj.M = 0.0

			pGeo = NewFIX.pPtGeo
			pGeo.SpatialReference = pSpRefPrj
			pGeo.Project(pSpRefShp)

			NewFIX.pPtGeo.Z = hRoute
			NewFIX.pPtGeo.M = 0.0
		End If

		Side1 = SideDef(NewFIX.pPtPrj, fCurrDir + 90.0, GNav.pPtPrj)
		'=======================================================================
		fDistInter = ReturnDistanceInMeters(NewFIX.pPtPrj, InterNav.pPtPrj)
		fDistGuid = ReturnDistanceInMeters(NewFIX.pPtPrj, GNav.pPtPrj)

		hRouteInter = (fDistInter / 4130.0) * (fDistInter / 4130.0) + InterNav.pPtPrj.Z
		hRouteGuid = (fDistGuid / 4130.0) * (fDistGuid / 4130.0) + GNav.pPtPrj.Z

		Segments(SegmentNum).fHGuidE = hRouteGuid
		Segments(SegmentNum).fHInterE = hRouteInter

		Segments(SegmentNum + 1).fHGuidS = hRouteGuid
		Segments(SegmentNum + 1).fHInterS = hRouteInter

		If (hRoute < hRouteInter) Or (hRoute < hRouteGuid) Then
			AddBtn.Enabled = False
			InterNav.Index = -1
			InfoFrm.SetInterNav(InterNav)
			InfoFrm.SetNearTol(-1.0, OptionButton202.Checked)

			'================================================
			If ComboBox101.SelectedIndex = 0 Then
				'fHmin = 0.3048 * 4000.0
				fHmax = 0.3048 * (1000.0 * 14.0 + 4000.0)
			Else
				'fHmin = 0.3048 * 18000.0
				fHmax = 0.3048 * (1000.0 * 28.0 + 18000.0)
			End If
			'================================================

			If hRoute < hRouteGuid Then
				If hRouteGuid > fHmax Then
					MessageBox.Show(My.Resources.str0106 + CStr(ConvertHeight(hRouteGuid, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0107)
					Return
				Else
					'"Выберите другое средсто или другой эшелон полёта."
					If MessageBox.Show(My.Resources.str0106 + CStr(ConvertHeight(hRouteGuid, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0107, Nothing, MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then Return

					If ComboBox101.SelectedIndex = 0 Then
						'    hRoute = 0.3048 * (1000.0 * I + 4000.0)
						I = System.Math.Round(0.001 * (hRouteGuid / 0.3048 - 4000.0) + 0.4999999)
					Else
						'    hRoute = 0.3048 * (1000.0 * I + 18000.0)
						I = System.Math.Round(0.001 * (hRouteGuid / 0.3048 - 18000.0) + 0.4999999)
					End If

					If I < ComboBox001.Items.Count Then
						ComboBox001.SelectedIndex = I
					Else
						MessageBox.Show(My.Resources.str0110)
					End If
				End If
			End If

			If hRoute < hRouteInter Then
				If hRouteInter > fHmax Then
					MessageBox.Show(My.Resources.str0111 + CStr(ConvertHeight(hRouteInter, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0107)
					Return
				Else
					'"Выберите другое средсто или другой эшелон полёта."
					If MessageBox.Show(My.Resources.str0111 + CStr(ConvertHeight(hRouteInter, 2)) + HeightConverter(HeightUnit).Unit + vbCrLf + My.Resources.str0109, Nothing, MessageBoxButtons.OKCancel) Then Return
				End If
				If ComboBox101.SelectedIndex = 0 Then
					I = System.Math.Round(0.001 * (hRouteInter / 0.3048 - 4000.0) + 0.4999999)
				Else
					I = System.Math.Round(0.001 * (hRouteInter / 0.3048 - 18000.0) + 0.4999999)
				End If

				If I < ComboBox001.Items.Count Then
					ComboBox001.SelectedIndex = I
				Else
					MessageBox.Show(My.Resources.str0114)
				End If
			End If
		End If
		AddBtn.Enabled = True

		'=======================================================================
		If OptionButton203.Checked Then
			DWPT = DirWPT2(ComboBox202.SelectedIndex)
			If ReturnDistanceInMeters(GNav.pPtPrj, DWPT.pPtPrj) < fMinCOPDist Then DWPT.TypeCode = -1
			Side2 = SideDef(NewFIX.pPtPrj, fCurrDir + 90.0, DWPT.pPtPrj)
		Else
			DWPT.TypeCode = -1
		End If

		pTrackLine2.FromPoint = NewFIX.pPtPrj

		If DWPT.TypeCode >= eNavaidType.VOR Then
			If Side1 > 0 Then
				pTrackLine2.ToPoint = GNav.pPtPrj
			ElseIf (Side1 < 0) Or (Side2 > 0) Then
				pTrackLine2.ToPoint = DWPT.pPtPrj
			Else
				pTrackLine2.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
			End If
		Else
			If Side1 > 0 Then
				pTrackLine2.ToPoint = GNav.pPtPrj
			Else
				pTrackLine2.ToPoint = PointAlongPlane(GNav.pPtPrj, fCurrDir, GNav.Range)
			End If
		End If

		'===========================================================================
		SecFIXPoly = CreateFixArea(GNav, InterNav, NewFIX)

		On Error Resume Next
		If Not FixElement2 Is Nothing Then pGraphics.DeleteElement(FixElement2)
		If Not FixPointElement2 Is Nothing Then pGraphics.DeleteElement(FixPointElement2)
		On Error GoTo 0

		FixElement2 = DrawPolygon(SecFIXPoly, RGB(255, 0, 255))
		FixPointElement2 = DrawPoint(NewFIX.pPtPrj, 255)

		CalcTolerance(NewFIX.pPtPrj, SecFIXPoly, fCurrDir, fNearTol, fFarTol)

		InfoFrm.SetInterNav(InterNav)
		InfoFrm.SetNearTol(fNearTol, OptionButton202.Checked)

		InfoFrm.SetGuidNavReqH(hRouteGuid)
		InfoFrm.SetInterNavReqH(hRouteInter)
		'Set Segments(0).pStartFixElement = FixElement1
	End Sub

End Class
