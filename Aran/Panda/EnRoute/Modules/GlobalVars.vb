Option Strict Off
Option Explicit On
Option Compare Text

Imports Microsoft.Win32
Imports ESRI.ArcGIS.Controls
Imports Aran.PANDA.Common
Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Module GlobalVars

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

#Region "Product releated"
	Public Const PANSOPSVersion As String = "DOC 8168 OPS/611    Sixth Edition, Amendments 1 to 6 incorporated - 2016"

	Public Const HelpFile As String = "PANDA.chm"
	Public PANDARootKey As RegistryKey = Registry.CurrentUser 'Registry.LocalMachine '
	Public Const PandaRegKey As String = "SOFTWARE\RISK\PANDA"
	Public Const ModuleRegKey As String = PandaRegKey + "\Conventional"
	Public Const LicenseKeyName As String = "Acar"
    Public Const ModuleCategory As String = "Conventional"
	Public Const ModuleName As String = "EnRoute"
#End Region

#Region "Math releated"
	Public Const PI As Double = Math.PI

	Public Const DegToRadValue As Double = PI / 180.0
	Public Const RadToDegValue As Double = 180.0 / PI

	Public Const mEps As Double = 0.000000000001
	Public Const distEps As Double = 0.0001
	Public Const PDGEps As Double = 0.0001

	Public Const degEps As Double = 1.0 / 36000.0
	Public Const radEps As Double = degEps * DegToRadValue

	Public Const EpsilonDistance As Double = 0.001
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

	Public Const HH_DISPLAY_TOPIC As Integer = &H0			' select last opened tab, [display a specified topic]
	Public Const HH_DISPLAY_TOC As Integer = &H1			' select contents tab, [display a specified topic]
	Public Const HH_DISPLAY_INDEX As Integer = &H2			' select index tab and searches for a keyword
	Public Const HH_DISPLAY_SEARCH As Integer = &H3			' select search tab and perform a search
	Public Const HH_SET_WIN_TYPE As Integer = &H4
	Public Const HH_GET_WIN_TYPE As Integer = &H5
	Public Const HH_GET_WIN_HANDLE As Integer = &H6
	Public Const HH_DISPLAY_TEXT_POPUP As Integer = &HE		' Display string resource ID or text in a pop-up window.
	Public Const HH_HELP_CONTEXT As Integer = &HF			' Display mapped numeric value in dwData.
	Public Const HH_TP_HELP_CONTEXTMENU As Integer = &H10	' Text pop-up help, similar to WinHelp's HELP_CONTEXTMENU.
	Public Const HH_TP_HELP_WM_HELP As Integer = &H11       ' text pop-up help, similar to WinHelp's HELP_WM_HELP.
	Public Const HH_CLOSE_ALL As Integer = &H12
	' System menu
	Public Const WM_SYSCOMMAND As Integer = &H112
	Public Const MF_STRING As Integer = &H0
	Public Const MF_SEPARATOR As Integer = &H800
	Public SYSMENU_ABOUT_ID As Integer = &H1
#End Region

#Region "Public Public"
	'Public RModel As Double
	Public EnrouteMOCValues() As Double

	Public UserName As String
	Public ConstDir As String

	Public s_Win32Window As Win32Window
	Public P_LicenseRect As ESRI.ArcGIS.Geometry.IPolygon
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

	Public CurrADHP As ADHPType
	Public DMEList() As TypeDefinitions.NavaidType
	Public NavaidList() As TypeDefinitions.NavaidType
	Public TACANList() As TypeDefinitions.NavaidType
	Public WPTList() As TypeDefinitions.NavaidType
	Public ObsTableList() As TypeDefinitions.ObstacleType

	Public SegmentData() As SegmentDataType
	'============================================================================================
	Public ErrorStr As String
	Friend Const MaxInterceptAngle As Double = 120.0

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
		DistanceConverter(0).Unit = My.Resources.str0053
		DistanceConverter(1).Multiplier = 1.0 / 1852.0
		DistanceConverter(1).Rounding = settings.DistancePrecision
		DistanceConverter(1).Unit = My.Resources.str0054

		HeightConverter(0).Multiplier = 1.0
		HeightConverter(0).Rounding = settings.HeightPrecision
		HeightConverter(0).Unit = My.Resources.str0051
		HeightConverter(1).Multiplier = 1.0 / 0.3048
		HeightConverter(1).Rounding = settings.HeightPrecision
		HeightConverter(1).Unit = My.Resources.str0052

		SpeedConverter(0).Multiplier = 3.6
		SpeedConverter(0).Rounding = settings.SpeedPrecision
		SpeedConverter(0).Unit = My.Resources.str0055
		SpeedConverter(1).Multiplier = 3.6 / 1.852
		SpeedConverter(1).Rounding = settings.SpeedPrecision
		SpeedConverter(1).Unit = My.Resources.str0056

		DSpeedConverter(0).Multiplier = 60.0
		DSpeedConverter(0).Rounding = settings.DSpeedPrecision
		DSpeedConverter(0).Unit = My.Resources.str0057
		DSpeedConverter(1).Multiplier = 60.0 / 0.3048
		DSpeedConverter(1).Rounding = settings.DSpeedPrecision
		DSpeedConverter(1).Unit = My.Resources.str0058
	End Sub

	Function InitCommand() As Integer
		Dim J As Integer
        Dim Achar As String

        Dim isExists As Boolean
        ConstDir = Aran.PANDA.Common.RegFuncs.GetConstantsDir(isExists)

        If Not isExists Then
            MessageBox.Show("Invalid constants path.", "Registry Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1
        End If

		'InstallDir = Value
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
			InitCommand = 0
			Exit Function
		End If

		pSpRefPrj.SetZDomain(-2000.0, 14000.0)
		pSpRefPrj.SetMDomain(-2000.0, 14000.0)
		'pSpRefPrj.SetDomain(-360.0, 360.0, -360.0, 360.0)

		Try
			pPCS = pSpRefPrj
		Catch
			pPCS = Nothing
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
		End If

		'WGS 84 ==========================================================================================

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
        Achar = Aran.PANDA.Common.RegFuncs.ReadConfig(ModuleCategory + "\" + ModuleName + "\" + LicenseKeyName, String.Empty)

		P_LicenseRect = DecodeLCode(Achar, ModuleName)

		If LstStDtWriter(Achar, ModuleName) <> 0 Then
			MessageBox.Show("CRITICAL ERROR!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If P_LicenseRect.IsEmpty() Then
			MessageBox.Show("ERROR #10LR512", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If
		'КОНЕЦ проверка на лицензионную зону
		'Units ==============================================================
		InitUnits(_settings)
		'====================================================================

		If Not PANS_OPS_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "PANS-OPS DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If Not Navaids_DataBase.InitModule Then
			MessageBox.Show(ErrorStr, "Navaids DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		If _settings.Aeroport = Guid.Empty Then
			MessageBox.Show("Current Airport is not defined. Please, use PANDA Settings page to define current Airport.", "Settings-Aeroport", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If

		'==============================================================
		EnrouteMOCValues = New Double() {300.0, 450.0, 600.0}
		s_Win32Window = New Win32Window(GetApplicationHWnd())
		'ADHP ==============================================================
		CurrADHP.Identifier = _settings.Aeroport
		CurrADHP.pPtGeo = Nothing

		UserName = DBModule.InitModule()
		DBModule.FillADHPFields(CurrADHP)

		If CurrADHP.pPtGeo Is Nothing Then
			MessageBox.Show("Initialization of ADHP failed.", "Settings-Aeroport", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return -1
		End If
		'=============================================================

		'UserName =

		'==============================================================
		J = FillNavaidList(NavaidList, DMEList, TACANList, P_LicenseRect)
		J += FillWPT_FIXList(WPTList, P_LicenseRect)
		FillObstacleList(ObsTableList, P_LicenseRect)

		If J < 0 Then
			MessageBox.Show("There is not any WPT and Navaid in license area!", "DataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

		Return J
	End Function

	'Private Sub InitLayerInfo(ByRef pLayerInfo As TypeDefinitions.LayerInfo, ByRef LayerName As String, ByRef pFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass)
	'	Dim pDataset As ESRI.ArcGIS.Geodatabase.IDataset
	'	Dim pWs As ESRI.ArcGIS.Geodatabase.IWorkspace
	'	Dim tmpVar As Object

	'	pDataset = pFeatureClass
	'	pWs = pDataset.Workspace

	'	If pWs.WorkspaceFactory.WorkspaceType = ESRI.ArcGIS.Geodatabase.esriWorkspaceType.esriFileSystemWorkspace Then
	'		pLayerInfo.Source = pWs.PathName + "\" + pDataset.BrowseName

	'		tmpVar = FileDateTime(pLayerInfo.Source + ".shp")
	'		pLayerInfo.FileDate = FileDateTime(pLayerInfo.Source + ".dbf")

	'		If tmpVar > pLayerInfo.FileDate Then pLayerInfo.FileDate = tmpVar
	'		pLayerInfo.Source = pLayerInfo.Source + ".shp"
	'	Else
	'		pLayerInfo.Source = pWs.PathName
	'		pLayerInfo.FileDate = FileDateTime(pLayerInfo.Source)
	'	End If

	'	pLayerInfo.LayerName = LayerName
	'	pLayerInfo.Initialised = True
	'End Sub

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
