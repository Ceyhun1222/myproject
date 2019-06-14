Option Strict Off
Option Explicit On
Option Compare Text

Imports Microsoft.Win32
Imports ESRI.ArcGIS.Controls
Imports Aran.PANDA.Common
Imports Aran.AranEnvironment

<System.Runtime.InteropServices.ComVisible(False)> Module GlobalVars
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

#Region "PANS-OPS Constants"   '   —рочно: внести в базу ArPANSOPS
	'—рочно ввнести в PANS-OPS
	Public Const MinDistanceSect As Short = 18520
	Public Const MaxDistanceSect As Short = 27780
	Public Const arMSARange As Integer = 46300
#End Region

#Region "Product releated"
	Public Const PANSOPSVersion As String = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016"

	Public Const HelpFile As String = "PANDA.chm"
	Public PANDARootKey As RegistryKey = Registry.CurrentUser

	Public Const PandaRegKey As String = "SOFTWARE\RISK\PANDA"
	Public Const ModuleRegKey As String = PandaRegKey + "\Conventional"
	Public Const LicenseKeyName As String = "Acar"
    Public Const ModuleCategory As String = "Conventional"

	Public Const ModuleName As String = "Arrival" '"Arrival"
#End Region

#Region "Math releated"
	Public Const RadToDegValue As Double = 180.0 / Math.PI
	Public Const DegToRadValue As Double = Math.PI / 180.0

	Public Const degEps As Double = 1.0 / 36000.0
	Public Const distEps As Double = 0.0001

	Public Const Eps As Double = 0.0001
#End Region

#Region "Model releated"
	Public NavTypeNames() As String = {"VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX", "MKR"}
	Public EnrouteMOCValues() As Double = {300.0, 450.0, 600.0}

	Public Const InnerSector As Integer = 1
	Public Const OuterSector As Integer = 2
	Public Const WholeSector As Integer = InnerSector + OuterSector

	Public Const PrimaryArea As Integer = 4
	Public Const BufferArea As Integer = 8
	Public Const WholeArea As Integer = PrimaryArea + BufferArea

#End Region

#Region "MS Windows"
	Public Const WU_LOGPIXELSX As Short = 88
	Public Const WU_LOGPIXELSY As Short = 90

	Public Const GWL_WNDPROC As Short = -4
	Public Const GWL_HINSTANCE As Short = -6
	Public Const GWL_HWNDPARENT As Short = -8
	Public Const GWL_STYLE As Short = -16
	Public Const GWL_EXSTYLE As Short = -20
	Public Const GWL_USERDATA As Short = -21
	Public Const GWL_ID As Short = -12

	Public Const HH_DISPLAY_TOPIC As Integer = &H0			' select last opened tab, [display a specified topic]
	Public Const HH_DISPLAY_TOC As Integer = &H1			' select contents tab, [display a specified topic]
	Public Const HH_DISPLAY_Index As Integer = &H2			' select Index tab and searches for a keyword
	Public Const HH_DISPLAY_SEARCH As Integer = &H3			' select search tab and perform a search
	Public Const HH_SET_WIN_TYPE As Integer = &H4
	Public Const HH_GET_WIN_TYPE As Integer = &H5
	Public Const HH_GET_WIN_HANDLE As Integer = &H6
	Public Const HH_DISPLAY_TEXT_POPUP As Integer = &HE		' Display string resource ID or text in a pop-up window.
	Public Const HH_HELP_CONTEXT As Integer = &HF			' Display mapped numeric Value in dwData.
	Public Const HH_TP_HELP_CONTEXTMENU As Integer = &H10	' Text pop-up help, similar to WinHelp's HELP_CONTEXTMENU.
	Public Const HH_TP_HELP_WM_HELP As Integer = &H11		' text pop-up help, similar to WinHelp's HELP_WM_HELP.
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

	'Public RModel As Double

	Public UserName As String
	Public ConstDir As String
	Public arPANS_OPS As String

	Public s_Win32Window As Win32Window
#End Region

#Region "Error holders :)"
	'Public ErrorCode As Integer
	Public ErrorStr As String
#End Region


#Region "Visibility manipulation (NOTE: Not implimented)"
#End Region

#Region "Display units managment"
	Public LangCode As Integer
	Public DistanceUnit As Integer
	Public HeightUnit As Integer
	Public SpeedUnit As Integer

	Public DistanceConverter(1) As TypeConvert
	Public HeightConverter(1) As TypeConvert
	Public SpeedConverter(1) As TypeConvert
	Public DSpeedConverter(1) As TypeConvert
#End Region

#Region "Document, projection, e.t.c."
    Public gHookHelper As IHookHelper
	Public gAranEnv As Aran.AranEnvironment.IAranEnvironment
	Public pMap As ESRI.ArcGIS.Carto.IMap
	Public pSpheroid As ESRI.ArcGIS.Geometry.ISpheroid
	Public pSpRefPrj As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pSpRefShp As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pGCS As ESRI.ArcGIS.Geometry.IGeographicCoordinateSystem
	Public pPCS As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem
#End Region

#Region "Data managment"
	'Public ADHPList() As ADHPType
	'Public SegmentData() As SegmentDataType
	Public CurrADHP As ADHPType
	Public NavaidList() As NavaidType
	Public DMEList() As NavaidType
	Public TACANList() As NavaidType
	Public WPTList() As WPT_FIXType
	Public ObstacleList() As ObstacleType
#End Region

	Public AppHInstance As Integer
	Public SizeOfOnePixel As Double

#End Region

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

        '========================================================================
        DistanceConverter(0).Multiplier = 0.001
        DistanceConverter(0).Rounding = settings.DistancePrecision
        DistanceConverter(0).Unit = My.Resources.str00100 '"км" '"kM"
        DistanceConverter(1).Multiplier = 1.0 / 1852.0
        DistanceConverter(1).Rounding = settings.DistancePrecision
        DistanceConverter(1).Unit = My.Resources.str00101 '"м.м." '"NM"

        HeightConverter(0).Multiplier = 1.0
        HeightConverter(0).Rounding = settings.HeightPrecision
        HeightConverter(0).Unit = My.Resources.str00102 '"м" '"meter"
        HeightConverter(1).Multiplier = 1.0 / 0.3048
        HeightConverter(1).Rounding = settings.HeightPrecision
        HeightConverter(1).Unit = My.Resources.str00103 '"фт" '"feet"

        SpeedConverter(0).Multiplier = 3.6
        SpeedConverter(0).Rounding = settings.SpeedPrecision
        SpeedConverter(0).Unit = My.Resources.str00104 '"км/час" '"km/h"
        SpeedConverter(1).Multiplier = 3.6 / 1.852
        SpeedConverter(1).Rounding = settings.SpeedPrecision
        SpeedConverter(1).Unit = My.Resources.str00105 '"узлы" '"Kt"

        DSpeedConverter(0).Multiplier = 1.0
        DSpeedConverter(0).Rounding = settings.DSpeedPrecision
        DSpeedConverter(0).Unit = My.Resources.str00106 '"м/мин" '"M/min"
        DSpeedConverter(1).Multiplier = 1.0 / 0.3048
        DSpeedConverter(1).Rounding = settings.DSpeedPrecision
        DSpeedConverter(1).Unit = My.Resources.str00107 '"фт/мин" '"feet/min"

        'LonSubFix = Array("E", "W")
        'LatSubFix = Array("N", "S")
    End Sub

	Function InitCommand(Optional FillObstacles As Boolean = False) As Integer
		Dim Achar As String

		Dim isExists As Boolean
		ConstDir = Aran.PANDA.Common.RegFuncs.GetConstantsDir(isExists)

		If Not isExists Then
			MessageBox.Show("Invalid constants path.", "Registry Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		arPANS_OPS = Aran.PANDA.Common.RegFuncs.ReadConfig(Of String)(ModuleCategory + "\" + ModuleName + "\arPANS_OPS", "arPANSOPS")

		Dim _settings As Settings = New Settings()
		_settings.Load(gAranEnv)

		LangCode = _settings.Language
		SetThreadLocale(LangCode)
		My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

		'Init projection ========================================================================================
		pMap = GetMap()
		pSpRefPrj = pMap.SpatialReference

		If pSpRefPrj Is Nothing Then
			MessageBox.Show("Map projection is not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
		End If

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
			InitProjection(pPCS.CentralMeridian(True), 0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing)
		Else
			MessageBox.Show("Invalid Map projection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
			'    InitProjection pPCS.CentralMeridian(True), 0, pPCS.ScaleFactor, pPCS.FalseEasting, pPCS.FalseNorthing
		End If

		'WGS 84 ==============================================================

		Dim pGeoCoordSys As ESRI.ArcGIS.Geometry.IGeographicCoordinateSystem
		Dim pSpatRefFact As ESRI.ArcGIS.Geometry.ISpatialReferenceFactory2

		pSpatRefFact = New ESRI.ArcGIS.Geometry.SpatialReferenceEnvironment
		pGeoCoordSys = pSpatRefFact.CreateGeographicCoordinateSystem(ESRI.ArcGIS.Geometry.esriSRGeoCSType.esriSRGeoCS_WGS1984)
		pSpRefShp = pGeoCoordSys
		GetActiveView().Refresh()

		pSpRefShp.SetZDomain(-2000.0, 14000.0)
		pSpRefShp.SetMDomain(-2000.0, 14000.0)
		pSpRefShp.SetDomain(-360.0, 360.0, -360.0, 360.0)

		'Achar ==============================================================
		Achar = Aran.PANDA.Common.RegFuncs.ReadConfig(Of String)(ModuleCategory + "\" + ModuleName + "\" + LicenseKeyName, String.Empty)

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
			Return 0
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

		Buffer.InitModule()

		FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, _settings.Radius)
		FillWPT_FIXList(WPTList, CurrADHP, _settings.Radius)
		If FillObstacles Then FillObstacleList(ObstacleList, CurrADHP, _settings.Radius)

		s_Win32Window = New Win32Window(GetApplicationHWnd())

		Return NavaidList.Length
	End Function

	Function GetNavTypeName(NavaidType As eNavaidType) As String
		If NavaidType = eNavaidType.NONE Then
			Return "WPT"
		Else
			Return NavTypeNames(NavaidType)
		End If
	End Function

	Public Sub TerminateCommand()
		'Application = Nothing
		pMap = Nothing
		pSpRefPrj = Nothing
		pSpRefShp = Nothing
	End Sub

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
End Module
