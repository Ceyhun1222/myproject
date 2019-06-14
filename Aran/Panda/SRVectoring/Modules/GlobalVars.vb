Option Strict Off
Option Explicit On
Option Compare Text

Imports Microsoft.Win32
Imports System.Collections.Generic
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Controls

'Imports Aran.Interfaces
Imports Aran.PANDA.Common


Module GlobalVars

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
#Region "PANS-OPS Constants"	'   —рочно: внести в базу ArPANSOPS
#End Region

#Region "Product releated"
	Public Const PANSOPSVersion As String = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016"
	Public Const HelpFile As String = "PANDA.chm"
	Public PANDARootKey As RegistryKey = Registry.CurrentUser 'Registry.LocalMachine '

	Public Const PandaRegKey As String = "SOFTWARE\RISK\PANDA"
	Public Const ModuleRegKey As String = PandaRegKey + "\Conventional"
	Public Const LicenseKeyName As String = "Acar"
    Public Const ModuleCategory As String = "Conventional"

	Public Const ModuleName As String = "SRVectoring"

#End Region

#Region "Math releated"
	Public Const PI As Double = Math.PI	   '3.1415926535897931

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
	Public Const MaxTurnAngle As Double = 120.0
	Public Const MinTurnAngle As Double = 5.0
#End Region

#Region "MS Windows"
	Public Const WU_LOGPIXELSX As Integer = 88
	Public Const WU_LOGPIXELSY As Integer = 90

	Public Const GWL_WNDPROC As Integer = -4
	Public Const GWL_HINSTANCE As Integer = -6
	Public Const GWL_HWNDPARENT As Integer = -8
	Public Const GWL_STYLE As Integer = -16
	Public Const GWL_EXSTYLE As Integer = -20
	Public Const GWL_USERDATA As Integer = -21
	Public Const GWL_ID As Integer = -12

	Public Const HH_DISPLAY_TOPIC As Integer = &H0			' select last opened tab, [display a specified topic]
	Public Const HH_DISPLAY_TOC As Integer = &H1			' select contents tab, [display a specified topic]
	Public Const HH_DISPLAY_Index As Integer = &H2			' select Index tab and searches for a keyword
	Public Const HH_DISPLAY_SEARCH As Integer = &H3			' select search tab and perform a search
	Public Const HH_SET_WIN_TYPE As Integer = &H4
	Public Const HH_GET_WIN_TYPE As Integer = &H5
	Public Const HH_GET_WIN_HANDLE As Integer = &H6
	Public Const HH_DISPLAY_TEXT_POPUP As Integer = &HE		' Display string resource ID or text in a pop-up window.
	Public Const HH_HELP_CONTEXT As Integer = &HF			' Display mapped numeric value in dwData.
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


#If DEBUG__ Then
#Region "Public DEBUG Variables"
	Public m_DebugMapFileName As String
	Public m_DebugApplicationHWnd As OLE_HANDLE
	Public m_DebugMap As IMap
	Public m_DebugActiveView As IActiveView
	Public m_DebugForm As Form
	Public AppHInstance As Integer
#End Region
#End If

#Region "Public Public"

	Private Initialized As Boolean
	Public p_LicenseRect As ESRI.ArcGIS.Geometry.IPolygon



	Public UserName As String
	Public ConstDir As String


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
	'Public Application As ESRI.ArcGIS.Framework.IApplication 'ArcMap application
	'Public pDocument As ESRI.ArcGIS.ArcMapUI.IMxDocument

	Public gHookHelper As IHookHelper
	Public gAranEnv As Aran.AranEnvironment.IAranEnvironment
	Public gAranGraphics As AranEnvironment.IAranGraphics
	Public pMap As ESRI.ArcGIS.Carto.IMap
	Public pSpheroid As ESRI.ArcGIS.Geometry.ISpheroid
	Public pSpRefPrj As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pSpRefShp As ESRI.ArcGIS.Geometry.ISpatialReference
	Public pPCS As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem
	Public pGCS As ESRI.ArcGIS.Geometry.IGeographicCoordinateSystem
#End Region

#Region "Data managment"
	Public CurrADHP As ADHPType
	Public NavaidList() As NavaidType
	Public DMEList() As NavaidType
	Public TACANList() As NavaidType
	Public WPTList() As WPT_FIXType
	'Public ADHPList() As ADHPType
	Public ObstacleList() As ObstacleType
#End Region

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

		'==============================================================

		DistanceConverter(0).Multiplier = 0.001
		DistanceConverter(0).Rounding = settings.DistancePrecision
		DistanceConverter(0).Unit = My.Resources.str0100    '"км" '"km"
		DistanceConverter(1).Multiplier = 1.0 / 1852.0
		DistanceConverter(1).Rounding = settings.DistancePrecision
		DistanceConverter(1).Unit = My.Resources.str0101    '"м.м." '"NM"

		HeightConverter(0).Multiplier = 1.0
		HeightConverter(0).Rounding = settings.HeightPrecision
		HeightConverter(0).Unit = My.Resources.str0102 '"м" '"meter"
		HeightConverter(1).Multiplier = 1.0 / 0.3048
		HeightConverter(1).Rounding = settings.HeightPrecision
		HeightConverter(1).Unit = My.Resources.str0103 '"фт" '"feet"

		SpeedConverter(0).Multiplier = 3.6
		SpeedConverter(0).Rounding = settings.SpeedPrecision
		SpeedConverter(0).Unit = My.Resources.str0104 '"км/час" '"kM/h"
		SpeedConverter(1).Multiplier = 3.6 / 1.852
		SpeedConverter(1).Rounding = settings.SpeedPrecision
		SpeedConverter(1).Unit = My.Resources.str0105 '"узлы" '"Kt"

		DSpeedConverter(0).Multiplier = 1.0
		DSpeedConverter(0).Rounding = settings.DSpeedPrecision
		DSpeedConverter(0).Unit = My.Resources.str0106 '"м/мин" '"m/min"
		DSpeedConverter(1).Multiplier = 1.0 / 0.3048
		DSpeedConverter(1).Rounding = settings.DSpeedPrecision
		DSpeedConverter(1).Unit = My.Resources.str0107 '"фт/мин" '"feet/min"

	End Sub

	Function InitCommand() As Integer
		Dim Achar As String
		Dim isExists As Boolean

		ConstDir = Aran.PANDA.Common.RegFuncs.GetConstantsDir(isExists)

        If Not isExists Then
            MessageBox.Show("Invalid constants path.", "Registry Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1
        End If

		Dim _settings As Settings
		_settings = New Settings()
        _settings.Load(gAranEnv)

		LangCode = _settings.Language
		SetThreadLocale(LangCode)
		My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

		'Map projection ========================================================================

		pMap = GetMap()

		pSpRefPrj = pMap.SpatialReference
		If pSpRefPrj Is Nothing Then
			MessageBox.Show("Map projection is not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return 0
		End If

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

		pSpatRefFact = New ESRI.ArcGIS.Geometry.SpatialReferenceEnvironment	'DirectCast(New SpatialReferenceEnvironment, SpatialReferenceEnvironment) 'New SpatialReferenceEnvironmentClass '
		pGeoCoordSys = pSpatRefFact.CreateGeographicCoordinateSystem(ESRI.ArcGIS.Geometry.esriSRGeoCSType.esriSRGeoCS_WGS1984)
		pSpRefShp = pGeoCoordSys
		GetActiveView().Refresh()

		pSpRefShp.SetZDomain(-2000.0, 14000.0)
		pSpRefShp.SetMDomain(-2000.0, 14000.0)
		pSpRefShp.SetDomain(-360.0, 360.0, -360.0, 360.0)

		'Achar ==============================================================
        Achar = Common.RegFuncs.ReadConfig(ModuleCategory + "\" + ModuleName + "\" + LicenseKeyName, String.Empty)

		p_LicenseRect = DecodeLCode(Achar, ModuleName)

		If LstStDtWriter(Achar, ModuleName) <> 0 Then
			MessageBox.Show("CRITICAL ERROR!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If p_LicenseRect.IsEmpty Then
			MessageBox.Show("ERROR #10LR512", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		'ADHP ==============================================================
		CurrADHP.Identifier = _settings.Aeroport
		CurrADHP.pPtGeo = Nothing

		DBModule.InitModule()
		DBModule.FillADHPFields(CurrADHP)

		If CurrADHP.pPtGeo Is Nothing Then
			MessageBox.Show("Initialization of ADHP failed.")
			Return -1
		End If

		'Units ==============================================================
		InitUnits(_settings)

		'==============================================================

		If Not PANS_OPS_Core_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "PANS-OPS Core DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

		'=============================================================
		'Dim icaoPrefixList As IObjectList
		'icaoPrefixList = New ObjectList
		'icaoPrefixList.Add("-1")
		'FillADHPList(ADHPList, icaoPrefixList) ', CurrCmd = 2

		s_Win32Window = New Win32Window(GetApplicationHWnd())

		Initialized = True
		Return 1
	End Function


	Public Sub TerminateCommand()
		If (Not Initialized) Then Exit Sub

		'pDocument = Nothing
		pMap = Nothing
		pSpheroid = Nothing
		pSpRefPrj = Nothing
		pSpRefShp = Nothing
		pPCS = Nothing

		p_LicenseRect = Nothing
		'Application = Nothing

		Initialized = False
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
