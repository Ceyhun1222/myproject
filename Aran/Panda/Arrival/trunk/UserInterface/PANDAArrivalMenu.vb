'Imports Microsoft.Win32
'Imports ESRI.ArcGIS.ADF.CATIDs
'Imports ESRI.ArcGIS.ADF.BaseClasses
'Imports System.Runtime.InteropServices

'#If ADD_NET Then
'<ComClass(PANDAArrivalMenu.ClassId, PANDAArrivalMenu.InterfaceId, PANDAArrivalMenu.EventsId), ProgId("PANDA_NET.ArrivalMenu")> Public NotInheritable Class PANDAArrivalMenu
'#Else
'<ComClass(PANDAArrivalMenu.ClassId, PANDAArrivalMenu.InterfaceId, PANDAArrivalMenu.EventsId), ProgId("PANDA.ArrivalMenu")> Public NotInheritable Class PANDAArrivalMenu
'#End If

'	Inherits BaseMenu

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
'	Public Const ClassId As String = "defe08e0-eaed-4d51-b1d3-46366d39baf2"
'	Public Const InterfaceId As String = "66b66fd2-11b9-42f3-8e89-925b343eb69e"
'	Public Const EventsId As String = "e439c081-fe03-4d15-8d55-0ae43c966ec8"
'#End Region

'	' A creatable COM class must have a Public Sub New() 
'	' with no parameters, otherwise, the class will not be 
'	' registered in the COM registry and cannot be created 
'	' via CreateObject.
'	Public Sub New()
'		'
'#If ADD_NET Then
'		AddItem("Arrival_NET.NonPrecArrivalCMD")
'		AddItem("Arrival_NET.PrecArrivalCMD")
'		AddItem("InitialArea_NET.InitialAreaCMD")
'        AddItem("InitialArea_NET.DeadRekconingCMD")
'        'AddItem("Arrival_NET.VisualManoeuvringCMD")
'#Else
'		AddItem("Arrival.NonPrecArrivalCMD")
'		AddItem("Arrival.PrecArrivalCMD")
'		AddItem("InitialArea.InitialAreaCMD")
'		AddItem("InitialArea.DeadRekconingCMD")
'        'AddItem("Arrival.VisualManoeuvringCMD")
'#End If
'	End Sub

'	Public Overrides ReadOnly Property Caption() As String
'		Get
'			Dim Value As Object
'			Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'			LangCode = If(IsNumeric(Value), CInt(Value), 1033)
'			SetThreadLocale(LangCode)
'			Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'#If ADD_NET Then
'			Return My.Resources.str90 + "_NET" '"Заход на посадку..." '"Approach..." '
'#Else
'            return My.Resources.str90 '"Заход на посадку..." '"Approach..." '
'#End If
'		End Get
'	End Property

'	Public Overrides ReadOnly Property Name() As String
'		Get
'#If ADD_NET Then
'			Return "PANDA_NET.ArrivalMenu"
'#Else
'			Return "PANDA.ArrivalMenu"
'#End If
'		End Get
'	End Property
'End Class


