'Imports Microsoft.Win32
'Imports System.Runtime.InteropServices
'Imports System.Drawing
'Imports ESRI.ArcGIS.ADF.BaseClasses
'Imports ESRI.ArcGIS.ADF.CATIDs
'Imports ESRI.ArcGIS.Framework
'Imports ESRI.ArcGIS.ArcMapUI
'Imports Aran.AranEnvironment

'#If ADD_NET Then
'<ComClass(NonPrecArrivalCMD.ClassId, NonPrecArrivalCMD.InterfaceId, NonPrecArrivalCMD.EventsId), ProgId("Arrival_NET.NonPrecArrivalCMD")> Public NotInheritable Class NonPrecArrivalCMD
'#Else
'<ComClass(NonPrecArrivalCMD.ClassId, NonPrecArrivalCMD.InterfaceId, NonPrecArrivalCMD.EventsId), ProgId("Arrival.NonPrecArrivalCMD")> Public NotInheritable Class NonPrecArrivalCMD
'#End If
'    Inherits BaseCommand

'#Region "COM GUIDs"
'    ' These  GUIDs provide the COM identity for this class 
'    ' and its COM interfaces. If you change them, existing 
'    ' clients will no longer be able to access the class.
'    Public Const ClassId As String = "19e58e78-2509-4ed9-80e7-15fc68e473eb"
'    Public Const InterfaceId As String = "43c066a3-43e4-41c3-bf61-5a95baa09dc3"
'    Public Const EventsId As String = "671c4fb8-54bb-49b6-b883-70e23c62a526"
'#End Region

'#Region "COM Registration Function(s)"
'    <ComRegisterFunction(), ComVisibleAttribute(False)> _
'    Private Shared Sub RegisterFunction(ByVal registerType As Type)
'        ' Required for ArcGIS Component Category Registrar support
'        ArcGISCategoryRegistration(registerType)

'        'Add any COM registration code after the ArcGISCategoryRegistration() call

'    End Sub

'    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
'    Private Shared Sub UnregisterFunction(ByVal registerType As Type)
'        ' Required for ArcGIS Component Category Registrar support
'        ArcGISCategoryUnregistration(registerType)

'        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

'    End Sub

'#Region "ArcGIS Component Category Registrar generated code"
'    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
'        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
'        MxCommands.Register(regKey)
'    End Sub

'    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
'        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
'        MxCommands.Unregister(regKey)
'    End Sub

'#End Region
'#End Region

'    ' A creatable COM class must have a Public Sub New() 
'    ' with no parameters, otherwise, the class will not be 
'    ' registered in the COM registry and cannot be created 
'    ' via CreateObject.
'    Public Sub New()
'        MyBase.New()

'        Dim Value As Object
'        Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'        LangCode = If(IsNumeric(Value), CInt(Value), 1033)

'        SetThreadLocale(LangCode)
'        Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)
'#If ADD_NET Then
'        MyBase.m_category = "PANDA_NET.Arrival"         'localizable text 
'        MyBase.m_name = "NonPrecArrivalCMD_NET"         'unique id, non-localizable
'        MyBase.m_caption = My.Resources.str157 + "_NET" '"Неточный"
'#Else
'		MyBase.m_category = "PANDA.Arrival"				'localizable text 
'		MyBase.m_name = "NonPrecArrivalCMD"				'unique id, non-localizable
'		MyBase.m_caption =  My.Resources.str157			'"Неточный"
'#End If

'        MyBase.m_helpFile = GlobalVars.HelpFile
'        MyBase.m_helpID = 8000
'        MyBase.m_bitmap = Nothing

'        MyBase.m_message = My.Resources.str157          '"Неточный"
'        MyBase.m_toolTip = My.Resources.str157          '"Неточный"
'    End Sub

'    Public Overrides Sub OnCreate(ByVal hook As Object)
'        MyBase.m_enabled = False

'        If Not hook Is Nothing Then
'            If gHookHelper Is Nothing Then gHookHelper = New ESRI.ArcGIS.Controls.HookHelper

'            If TypeOf hook Is IApplication Then
'                gHookHelper.Hook = hook
'            ElseIf TypeOf hook Is Aran.AranEnvironment.IAranEnvironment Then
'                gAranEnv = DirectCast(hook, Aran.AranEnvironment.IAranEnvironment)





'                gHookHelper.Hook = gAranEnv.HookObject
'            End If
'            MyBase.m_enabled = True
'        End If
'    End Sub

'    Public Overrides ReadOnly Property Caption() As String
'        Get
'            Dim Value As Object
'            Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'            LangCode = If(IsNumeric(Value), CInt(Value), 1033)

'            SetThreadLocale(LangCode)
'            Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'#If ADD_NET Then
'            Caption = My.Resources.str157 + "_NET"  '"Неточный"
'#Else
'			Caption = My.Resources.str157			'"Неточный"
'#End If
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Message() As String
'        Get
'            Dim Value As Object
'            Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'            LangCode = If(IsNumeric(Value), CInt(Value), 1033)

'            SetThreadLocale(LangCode)
'            Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'            Message = My.Resources.str157           '"Неточный"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Tooltip() As String
'        Get
'            Dim Value As Object
'            Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'            LangCode = If(IsNumeric(Value), CInt(Value), 1033)

'            SetThreadLocale(LangCode)
'            Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'            Tooltip = My.Resources.str157           '"Неточный"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Checked() As Boolean
'        Get
'            Checked = CurrCmd = 1
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Enabled() As Boolean
'        Get
'            Enabled = m_enabled And ((CurrCmd = 0) Or (CurrCmd = 1))
'        End Get
'    End Property

'    Dim ArrivalFrm As CArrivalFrm = Nothing

'    Public Overrides Sub OnClick()
'        If CurrCmd = 1 Then Return
'        'If CurrCmd <> 0 Then Return
'        CurrCmd = 0

'        If InitCommand() > 0 Then
'			If ArrivalFrm Is Nothing OrElse ArrivalFrm.IsDisposed Then
'				ArrivalFrm = New CArrivalFrm
'			End If

'			'Dim aranToolItem As AranTool = New AranTool()
'			'aranToolItem.Visible = True
'			'aranToolItem.Cursor = Cursors.Cross

'			'aranToolItem.MouseClickedOnMap = AddressOf ArrivalFrm.OnMouseclik
'			'gAranEnv.AranUI.AddMapTool(aranToolItem)

'			CurrCmd = 1

'            ArrivalFrm.Show(s_Win32Window)
'            ArrivalFrm.ListView0001.Focus()
'		End If
'    End Sub
'End Class
