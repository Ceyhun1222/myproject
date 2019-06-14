Option Strict Off
Option Explicit On
Option Compare Text

Imports Microsoft.Win32
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Controls

Imports Aran.PANDA.Common
Imports Aran.AranEnvironment

#Const InitByDefault = False

Module GlobalVars
#Const CompileMDBVersion = False    'MDB or Server Version

	<System.Runtime.InteropServices.ComVisible(False)> Public Class Win32Window
		Implements System.Windows.Forms.IWin32Window
		Private _handle As IntPtr

		Public Sub New(ByVal handle As Int32)
			_handle = New IntPtr(handle)
		End Sub

		Public ReadOnly Property Handle() As IntPtr Implements System.Windows.Forms.IWin32Window.Handle
			Get
				Return _handle
			End Get
		End Property
	End Class

#Region "Public Constants"
#Region "PANS-OPS Constants"    '   —рочно: внести в базу ArPANSOPS
	'   —рочно: внести в базу ArPANSOPS
	Public Const THRAccuracy As Double = 259.28
	Public Const arInitApprBank As Double = 25.0
	Public Const arInitialApTurnRadius As Double = 2500.0 '3704.0 '2 NM

	'Public Const ArPANSOPS_MaxNavDist As Double = 46000.0
	Public Const OASZOrigin As Double = 900.0
	Public Const arHOASPlaneCat1 As Double = 300.0
	Public Const arHOASPlaneCat23 As Double = 150.0
	Public EnrouteMOCValues() As Double = {300.0, 450.0, 600.0}
#End Region

#Region "Product releated"
	Public Const PANSOPSVersion As String = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016"

	Public Const HelpFile As String = "PANDA.chm"
	Public PANDARootKey As RegistryKey = Registry.CurrentUser 'Registry.LocalMachine '

	Public Const PandaRegKey As String = "RISK\PANDA"
	Public Const ModuleRegKey As String = PandaRegKey + "\Conventional"
	Public Const LicenseKeyName As String = "Acar"
	Public Const ModuleCategory As String = "Conventional"
	Public Const ModuleName As String = "Arrival"
#End Region

#Region "Math releated"
	Public Const PI As Double = Math.PI    '3.1415926535897931

	Public Const DegToRadValue As Double = PI / 180.0
	Public Const RadToDegValue As Double = 180.0 / PI

	Public Const mEps As Double = 0.000000000001
	Public Const distEps As Double = 0.0001
	Public Const PDGEps As Double = 0.0001

	Public Const degEps As Double = 1.0 / 36000.0
	Public Const radEps As Double = degEps * DegToRadValue

	Public Const EpsilonDistance As Double = 0.001
#End Region

#Region "Model releated"
	Public Const NO_DATA_VALUE As Integer = -9999
	Public Const MaxModelRadius As Double = 150000.0
	Public Const MaxILSDist As Double = 20000.0
	Public Const MaxNAVDist As Double = 700000.0

	Public Const OffsetTreshold As Double = 1.0
	Public Const MinGPIntersectHeight As Double = 55.0
	Public Const MaxGPIntersectHeight As Double = 150.0
	Public Const LocOffsetOCHAdd As Double = 20.0
	Public Const MaxRefGPAngle As Double = 3.5
	Public Const MaxInterceptAngle As Double = 135.0

	Public Const C_ISAtC As Double = 15.0
	Public Const rDMEMin As Double = 7.0 * 1852.0
	Public Const rDMEMax As Double = 60.0 * 1852.0
#End Region

#Region "MS Windows"
	Public Const CBS_DROPDOWNLIST As Integer = &H3
	Public Const CB_SETDROPPEDWIDTH As Integer = &H160
	Public Const CB_SHOWDROPDOWN As Integer = &H14F

	Public Const GWL_WNDPROC As Integer = -4
	Public Const GWL_HINSTANCE As Integer = -6
	Public Const GWL_HWNDPARENT As Integer = -8
	Public Const GWL_STYLE As Integer = -16
	Public Const GWL_EXSTYLE As Integer = -20
	Public Const GWL_USERDATA As Integer = -21
	Public Const GWL_ID As Integer = -12

	Public Const HH_DISPLAY_TOPIC As Integer = &H0          ' select last opened tab, [display a specified topic]
	Public Const HH_DISPLAY_TOC As Integer = &H1            ' select contents tab, [display a specified topic]
	Public Const HH_DISPLAY_INDEX As Integer = &H2          ' select index tab and searches for a keyword
	Public Const HH_DISPLAY_SEARCH As Integer = &H3         ' select search tab and perform a search
	Public Const HH_SET_WIN_TYPE As Integer = &H4
	Public Const HH_GET_WIN_TYPE As Integer = &H5
	Public Const HH_GET_WIN_HANDLE As Integer = &H6
	Public Const HH_DISPLAY_TEXT_POPUP As Integer = &HE     ' Display string resource ID or text in a pop-up window.
	Public Const HH_HELP_CONTEXT As Integer = &HF           ' Display mapped numeric value in dwData.
	Public Const HH_TP_HELP_CONTEXTMENU As Integer = &H10   ' Text pop-up help, similar to WinHelp's HELP_CONTEXTMENU.
	Public Const HH_TP_HELP_WM_HELP As Integer = &H11       ' text pop-up help, similar to WinHelp's HELP_WM_HELP.
	Public Const HH_CLOSE_ALL As Integer = &H12
	' System menu
	Public Const WM_SYSCOMMAND As Integer = &H112
	Public Const MF_STRING As Integer = &H0
	Public Const MF_SEPARATOR As Integer = &H800
	Public SYSMENU_ABOUT_ID As Integer = &H1
#End Region
#End Region

#Region "Public Variables"

#Region "Public Public"
	Public CurrCmd As Integer = 0
	'Public Initialized As Boolean
	Public P_LicenseRect As ESRI.ArcGIS.Geometry.IPolygon

	Public RModel As Double

	Public UserName As String
	Public ConstDir As String
	Public arPANS_OPS As String

	Public _Win32Window As Win32Window
	Public _settings As Settings
#End Region

#Region "Error holders :)"
	'Public ErrorCode As Integer
	Public ErrorStr As String
#End Region

#Region "Visibility manipulation"
	Public CommandBar As ArrivalToolbar = Nothing
	Public WPTColor As Integer
	Public ObstColor As Integer

	'Public Const ptColor As Integer = &H1FFF1F

	'Public OASPlanesElement(8) As ESRI.ArcGIS.Carto.IElement
	'Public OASPlanesState As Boolean

	Public OASPlanesCat23Element(8) As ESRI.ArcGIS.Carto.IElement
	Public OASPlanesCat23State As Boolean

	Public OASPlanesCat1Element(8) As ESRI.ArcGIS.Carto.IElement
	Public OASPlanesCat1State As Boolean

	Public ILSPlanesElement(12) As ESRI.ArcGIS.Carto.IElement
	Public ILSPlanesState As Boolean

	Public OFZPlanesElement(7) As ESRI.ArcGIS.Carto.IElement
	Public OFZPlanesState As Boolean

	'Public VisualAreaState As Boolean
	'Public CLState As Boolean
	'Public ButtonControl2State As Boolean
	'Public ButtonControl3State As Boolean
	'Public ButtonControl4State As Boolean
	'Public ButtonControl5State As Boolean
	'Public ButtonControl6State As Boolean
	'Public ButtonControl8State As Boolean

	'Public VisualAreaElement As ESRI.ArcGIS.Carto.IElement
	'Public CLElement As ESRI.ArcGIS.Carto.IElement
	'Public pCircleElem As ESRI.ArcGIS.Carto.IElement
	'Public StraightAreaElem As ESRI.ArcGIS.Carto.IElement
	'Public StrTrackElem As ESRI.ArcGIS.Carto.IElement

	Public TurnAreaElem As ESRI.ArcGIS.Carto.IElement
	Public NomTrackElem As ESRI.ArcGIS.Carto.IElement
	Public PrimElem As ESRI.ArcGIS.Carto.IElement
	Public SecRElem As ESRI.ArcGIS.Carto.IElement
	Public SecLElem As ESRI.ArcGIS.Carto.IElement
	Public FIXElem As ESRI.ArcGIS.Carto.IElement

	Public K1K1Elem As ESRI.ArcGIS.Carto.IElement
	Public KKElem As ESRI.ArcGIS.Carto.IElement
#End Region

#Region "Display units managment"
	Public LangCode As Integer
	Public DistanceUnit As Integer
	Public HeightUnit As Integer
	Public SpeedUnit As Integer
	Public AngleUnit As Integer

	Public DistanceConverter(1) As TypeConvert
	Public HeightConverter(1) As TypeConvert
	Public SpeedConverter(1) As TypeConvert
	Public DSpeedConverter(1) As TypeConvert
	Public AngleConverter(1) As TypeConvert

	Public ReportDistanceConverter(2) As TypeConvert
	Public ReportHeightConverter(1) As TypeConvert
	Public ReportSpeedConverter(1) As TypeConvert

	Public ReportDistanceUnit As Integer
	Public ReportHeightUnit As Integer
	Public ReportSpeedUnit As Integer
#End Region

#Region "Document, projection, e.t.c."
	'Public Application As ESRI.ArcGIS.Framework.IApplication
	'Public pDocument As ESRI.ArcGIS.ArcMapUI.IMxDocument

	Public gAranEnv As Aran.AranEnvironment.IAranEnvironment
	Public gHookHelper As IHookHelper
	Public pMap As ESRI.ArcGIS.Carto.IMap
	Public pSpheroid As ESRI.ArcGIS.Geometry.ISpheroid
	Public pSpRefPrj As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pSpRefGeo As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pGCS As ESRI.ArcGIS.Geometry.IGeographicCoordinateSystem
	Public pPCS As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem
#End Region

#Region "Data managment"
	Public CurrADHP As ADHPType
	Public NavaidList() As NavaidData
	Public DMEList() As NavaidData
	Public TACANList() As NavaidData
	Public WPTList() As WPT_FIXType
	Public SegmentData() As SegmentDataType
#End Region

#Region "OAS & ILS planes"
	Public OASPlanes(8) As D3DPolygone
	Public OASPlanesCat23(8) As D3DPolygone
	Public wOASPlanes(8) As D3DPolygone

	Public ILSPlanes(12) As D3DPolygone
	Public OFZPlanes(7) As D3DPolygone

	Public OASPlaneNames() As String = {"Zero", "W", "X", "Y", "Z", "Y", "X", "W*", "Common", "Non Prec."}
	Public OFZPlaneNames() As String = {"Zero", "Inner Approach", "Inner transitional1", "Inner transitional2", "Balking landing", "Inner transitional2", "Inner transitional1", "Common"}
	Public ILSPlaneNames() As String = {"Zero", "Approach 1", "Approach 2", "Transitional A", "Transitional B", "Transitional C", "Transitional D", "Missed Approach", "Transitional D", "Transitional C", "Transitional B", "Transitional A", "Common"}
#End Region

#End Region

	'===============================================

	Function Initalize_PANDA() As Boolean
		On Error GoTo ErrorHandler

		On Error GoTo 0
		Return True

ErrorHandler:
		On Error GoTo 0
		MessageBox.Show(My.Resources.str00305, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
		Return False
	End Function

	Sub InitUnits(settings As Settings)
		DistanceUnit = settings.DistanceUnit
		If DistanceUnit < 0 Then
			DistanceUnit = 0
		ElseIf DistanceUnit > 1 Then
			DistanceUnit = 1
		End If

		HeightUnit = settings.HeightUnit
		If HeightUnit < 0 Then
			HeightUnit = 0
		ElseIf HeightUnit > 1 Then
			HeightUnit = 1
		End If

		SpeedUnit = settings.SpeedUnit
		If SpeedUnit < 0 Then
			SpeedUnit = 0
		ElseIf SpeedUnit > 1 Then
			SpeedUnit = 1
		End If

		AngleUnit = 0

		ReportDistanceUnit = settings.ReportIntefaceData.DistanceUnit
		If ReportDistanceUnit < 0 Then
			ReportDistanceUnit = 0
		ElseIf ReportDistanceUnit > 1 Then
			ReportDistanceUnit = 1
		End If

		ReportHeightUnit = settings.ReportIntefaceData.HeightUnit
		If ReportHeightUnit < 0 Then
			ReportHeightUnit = 0
		ElseIf ReportHeightUnit > 1 Then
			ReportHeightUnit = 1
		End If

		ReportSpeedUnit = settings.ReportIntefaceData.SpeedUnit
		If ReportSpeedUnit < 0 Then
			ReportSpeedUnit = 0
		ElseIf ReportSpeedUnit > 1 Then
			ReportSpeedUnit = 1
		End If

		'========================================================================

		DistanceConverter(0).Multiplier = 0.001
		DistanceConverter(0).Rounding = settings.DistancePrecision
		DistanceConverter(0).Unit = My.Resources.str00019 '"км" '"km"
		DistanceConverter(1).Multiplier = 1.0 / 1852.0
		DistanceConverter(1).Rounding = settings.DistancePrecision
		DistanceConverter(1).Unit = My.Resources.str00020 '"м.м." '"NM"

		HeightConverter(0).Multiplier = 1.0
		HeightConverter(0).Rounding = settings.HeightPrecision
		HeightConverter(0).Unit = My.Resources.str00021 '"м" '"meter"
		HeightConverter(1).Multiplier = 1.0 / 0.3048
		HeightConverter(1).Rounding = settings.HeightPrecision
		HeightConverter(1).Unit = My.Resources.str00018 '"фт" '"feet"

		SpeedConverter(0).Multiplier = 3.6
		SpeedConverter(0).Rounding = settings.SpeedPrecision
		SpeedConverter(0).Unit = My.Resources.str00010  '"км/час" '"kM/h"
		SpeedConverter(1).Multiplier = 3.6 / 1.852
		SpeedConverter(1).Rounding = settings.SpeedPrecision
		SpeedConverter(1).Unit = My.Resources.str00017  '"узлы" '"Kt"

		DSpeedConverter(0).Multiplier = 60.0
		DSpeedConverter(0).Rounding = settings.DSpeedPrecision
		DSpeedConverter(0).Unit = My.Resources.str00013 '"м/мин" '"m/min"
		DSpeedConverter(1).Multiplier = 60.0 / 0.3048
		DSpeedConverter(1).Rounding = settings.DSpeedPrecision
		DSpeedConverter(1).Unit = My.Resources.str00016 '"фт/мин" '"feet/min"

		AngleConverter(0).Multiplier = 1.0
		AngleConverter(0).Rounding = settings.AnglePrecision
		AngleConverter(0).Unit = "∞"

		'========================================================================================

		ReportDistanceConverter(0).Multiplier = 1.0
		ReportDistanceConverter(0).Rounding = settings.ReportIntefaceData.DistancePrecision
		ReportDistanceConverter(0).Unit = My.Resources.str00021 '"м.м." '"NM"

		ReportDistanceConverter(1).Multiplier = 0.001
		ReportDistanceConverter(1).Rounding = settings.ReportIntefaceData.DistancePrecision
		ReportDistanceConverter(1).Unit = My.Resources.str00019 '"км" '"km"

		ReportDistanceConverter(2).Multiplier = 1.0 / 1852.0
		ReportDistanceConverter(2).Rounding = settings.ReportIntefaceData.DistancePrecision
		ReportDistanceConverter(2).Unit = My.Resources.str00020 '"м.м." '"NM"

		ReportHeightConverter(0).Multiplier = 1.0
		ReportHeightConverter(0).Rounding = settings.ReportIntefaceData.HeightPrecision
		ReportHeightConverter(0).Unit = My.Resources.str00021   '"м" '"meter"
		ReportHeightConverter(1).Multiplier = 1.0 / 0.3048
		ReportHeightConverter(1).Rounding = settings.ReportIntefaceData.HeightPrecision
		ReportHeightConverter(1).Unit = My.Resources.str00018   '"фт" '"feet"

		ReportSpeedConverter(0).Multiplier = 3.6
		ReportSpeedConverter(0).Rounding = settings.ReportIntefaceData.SpeedPrecision
		ReportSpeedConverter(0).Unit = My.Resources.str00010    '"км/час" '"kM/h"
		ReportSpeedConverter(1).Multiplier = 3.6 / 1.852
		ReportSpeedConverter(1).Rounding = settings.ReportIntefaceData.SpeedPrecision
		ReportSpeedConverter(1).Unit = My.Resources.str00017    '"узлы" '"Kt"
	End Sub

	Function InitCommand() As Integer
		Dim Achar As String

#If Not InitByDefault Then
		If Not Initalize_PANDA() Then Return -1
#End If
		'Dim ge As IGeometryEnvironment
		'ge = New GeometryEnvironment
		'ge.AutoDensifyTolerance = 0.0001
		'ge.UseAlternativeTopoOps = True

		Dim isExists As Boolean
		ConstDir = RegFuncs.GetConstantsDir(isExists)

		If Not isExists Then
			MessageBox.Show("Invalid constants path.", "Registry Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		WPTColor = &H1FFF1F
		ObstColor = 255

		arPANS_OPS = RegFuncs.ReadConfig(Of String)(ModuleCategory + "\" + ModuleName + "\arPANS_OPS", "arPANSOPS")

		_settings = New Settings()

#If Not InitByDefault Then
		_settings.Load(GlobalVars.gAranEnv)
#End If

		LangCode = _settings.Language
		SetThreadLocale(LangCode)
		Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

		'Map projection ========================================================================

		pMap = GetMap()
		pSpRefPrj = pMap.SpatialReference

		If pSpRefPrj Is Nothing Then
			MessageBox.Show("Map projection is not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
		End If

		'Dim xMin, xMax, yMin, yMax As Double
		'Dim zMin, zMax, mMin, mMax As Double

		'pSpRefPrj.GetDomain(xMin, xMax, yMin, yMax)
		'pSpRefPrj.GetZDomain(zMin, zMax)
		'pSpRefPrj.GetMDomain(mMin, mMax)
		'pSpRefPrj.GetFalseOriginAndUnits(mMin, mMax, zMin)

		pSpRefPrj.SetZDomain(-2000.0, 14000.0)
		pSpRefPrj.SetMDomain(-2000.0, 14000.0)
		'pSpRefPrj.SetDomain(-360.0, 360.0, -360.0, 360.0)

		pPCS = Nothing
		Try
			pPCS = pSpRefPrj
		Catch
		End Try

		If pPCS Is Nothing Then
			pGCS = pSpRefPrj
		Else
			pGCS = pPCS.GeographicCoordinateSystem
		End If

		If pGCS Is Nothing Then
			MessageBox.Show("Invalid Map projection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
		End If

		pSpheroid = pGCS.Datum.Spheroid
		InitEllipsoid(pSpheroid.SemiMajorAxis, 1.0 / pSpheroid.Flattening)

		If Not pPCS Is Nothing Then
			InitProjection(pPCS.CentralMeridian(True), 0.0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing)
		Else
			MessageBox.Show("Invalid Map projection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
			'    InitProjection pPCS.CentralMeridian(True), 0.0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing
		End If

		'WGS 84 ==============================================================

		Dim pGeoCoordSys As IGeographicCoordinateSystem
		Dim pSpatRefFact As ISpatialReferenceFactory2

		pSpatRefFact = New SpatialReferenceEnvironment 'DirectCast(New SpatialReferenceEnvironment, SpatialReferenceEnvironment) 'New SpatialReferenceEnvironmentClass '
		pGeoCoordSys = pSpatRefFact.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984)
		pSpRefGeo = pGeoCoordSys
		GetActiveView().Refresh()

		pSpRefGeo.SetZDomain(-2000.0, 14000.0)
		pSpRefGeo.SetMDomain(-2000.0, 14000.0)
		pSpRefGeo.SetDomain(-360.0, 360.0, -360.0, 360.0)

		'Achar ==============================================================
		Achar = Common.RegFuncs.ReadConfig(Of String)(ModuleCategory + "\" + ModuleName + "\" + LicenseKeyName, String.Empty)

		P_LicenseRect = DecodeLCode(Achar, ModuleName)

		If LstStDtWriter(Achar, ModuleName) <> 0 Then
			MessageBox.Show("CRITICAL ERROR!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If P_LicenseRect.IsEmpty() Then
			MessageBox.Show("ERROR #10LR512", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		'Units ==============================================================
		InitUnits(_settings)

		'==============================================================

		If Not PANS_OPS_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "PANS-OPS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If Not Navaids_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "Navaids DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If Not Categories_DATABase.InitModule Then
			MessageBox.Show(ErrorStr, "Categories DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If Not PANS_OPS_Core_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "PANS-OPS Core DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If _settings.Aeroport = Guid.Empty Then
			Throw New AranException("Current Airport is not defined. Please, use PANDA Settings page to define current Airport.", ExceptionType.Warning)
		End If

		'ADHP ==============================================================
		CurrADHP.Identifier = _settings.Aeroport
		CurrADHP.pPtGeo = Nothing

		DBModule.InitModule()
		DBModule.FillADHPFields(CurrADHP)

		If CurrADHP.pPtGeo Is Nothing Then
			Throw New AranException("Initialization of ADHP failed.")
			Return -1
		End If
		'=============================================================

		'ReDim NavaidList(-1)
		'ReDim DMEList(-1)
		'ReDim TACANList(-1)


		FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, _settings.Radius) 'MaxNAVDist
		FillWPT_FIXList(WPTList, CurrADHP, _settings.Radius) 'MaxNAVDist

		_Win32Window = New Win32Window(GetApplicationHWnd())

		Return 1
	End Function

	Public Function GetMap() As ESRI.ArcGIS.Carto.IMap
		Return gHookHelper.FocusMap
	End Function

	Public Function GetActiveView() As ESRI.ArcGIS.Carto.IActiveView
		Return gHookHelper.FocusMap
	End Function

	Public Function GetMapFileName() As String
		If TypeOf gHookHelper.Hook Is ESRI.ArcGIS.Framework.IApplication Then
			Dim app As ESRI.ArcGIS.Framework.IApplication =
			 DirectCast(gHookHelper.Hook, ESRI.ArcGIS.Framework.IApplication)
			Return app.Templates.Item(app.Templates.Count - 1)
		Else
			Return gAranEnv.DocumentFileName
		End If
	End Function

	Public Function GetApplicationHWnd() As Integer
		If TypeOf gHookHelper.Hook Is ESRI.ArcGIS.Framework.IApplication Then
			Return DirectCast(gHookHelper.Hook, ESRI.ArcGIS.Framework.IApplication).hWnd
		Else
			Return gAranEnv.Win32Window.Handle
		End If
	End Function

	'Public Sub SaveDocument(fileName As String, saveAsCopy As Boolean)
	'    If TypeOf gHookHelper.Hook Is ESRI.ArcGIS.Framework.IApplication Then
	'        Dim app As ESRI.ArcGIS.Framework.IApplication =
	'         DirectCast(gHookHelper.Hook, ESRI.ArcGIS.Framework.IApplication)
	'        app.SaveAsDocument(fileName, saveAsCopy)
	'    Else
	'        gAranEnv.SaveDocumentAs(fileName, saveAsCopy)
	'    End If
	'End Sub

End Module
