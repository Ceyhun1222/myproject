'Imports Microsoft.Win32

'Imports ESRI.ArcGIS.ADF.CATIDs
'Imports ESRI.ArcGIS.ADF.BaseClasses
'Imports System.Runtime.InteropServices
'#If ADD_NET Then
'<ComClass(PANDAArrivalToolbar.ClassId, PANDAArrivalToolbar.InterfaceId, PANDAArrivalToolbar.EventsId), ProgId("PANDA_NET.ArrivalToolbar")> Public NotInheritable Class PANDAArrivalToolbar
'#Else
'<ComClass(PANDAArrivalToolbar.ClassId, PANDAArrivalToolbar.InterfaceId, PANDAArrivalToolbar.EventsId), ProgId("PANDA.ArrivalToolbar")> Public NotInheritable Class PANDAArrivalToolbar
'#End If
'	Inherits BaseToolbar

'#Region "COM Registration Function(s)"
'	<ComRegisterFunction(), ComVisibleAttribute(False)> _
'	Private Shared Sub RegisterFunction(ByVal registerType As Type)
'		' Required for ArcGIS Component Category Registrar support
'		ArcGISCategoryRegistration(registerType)

'		'Add any COM registration code after the ArcGISCategoryRegistration() call

'	End Sub

'	<ComUnregisterFunction(), ComVisibleAttribute(False)> _
'	Private Shared Sub UnregisterFunction(ByVal registerType As Type)
'		' Required for ArcGIS Component Category Registrar support
'		ArcGISCategoryUnregistration(registerType)

'		'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

'	End Sub

'#Region "ArcGIS Component Category Registrar generated code"
'	''' <summary>
'	''' Required method for ArcGIS Component Category registration -
'	''' Do not modify the contents of this method with the code editor.
'	''' </summary>
'	Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
'		Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
'		MxCommandBars.Register(regKey)

'	End Sub
'	''' <summary>
'	''' Required method for ArcGIS Component Category unregistration -
'	''' Do not modify the contents of this method with the code editor.
'	''' </summary>
'	Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
'		Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
'		MxCommandBars.Unregister(regKey)

'	End Sub

'#End Region
'#End Region

'#Region "COM GUIDs"
'	' These  GUIDs provide the COM identity for this class 
'	' and its COM interfaces. If you change them, existing 
'	' clients will no longer be able to access the class.
'	Public Const ClassId As String = "02977bc2-8e92-405e-8ded-1e2ede105a04"
'	Public Const InterfaceId As String = "8ba9c6fe-0452-4f7e-8840-f06eedca9f36"
'	Public Const EventsId As String = "363c9fd2-b744-49fe-b4a3-12695422066f"
'#End Region

'	' A creatable COM class must have a Public Sub New() 
'	' with no parameters, otherwise, the class will not be 
'	' registered in the COM registry and cannot be created 
'	' via CreateObject.
'	Public Sub New()
'#If ADD_NET Then
'		AddItem("PANDA_NET.ArrivalMenu")
'#Else
'		AddItem("PANDA.ArrivalMenu")
'#End If
'	End Sub

'	Public Overrides ReadOnly Property Caption() As String
'		Get
'			Dim Value As Object
'			Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'			LangCode = If(IsNumeric(Value), CInt(Value), 1033)
'			SetThreadLocale(LangCode)
'			Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'			Return My.Resources.str80
'		End Get
'	End Property

'	Public Overrides ReadOnly Property Name() As String
'		Get
'#If ADD_NET Then
'			Return "PANDA_NET.ArrivalToolbar"
'#Else
'            Return "PANDA.ArrivalToolbar"
'#End If
'		End Get
'	End Property
'End Class
