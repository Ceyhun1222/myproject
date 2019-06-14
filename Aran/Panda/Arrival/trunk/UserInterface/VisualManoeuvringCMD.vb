'Imports Microsoft.Win32
'Imports System.Runtime.InteropServices
'Imports System.Drawing
'Imports ESRI.ArcGIS.ADF.BaseClasses
'Imports ESRI.ArcGIS.ADF.CATIDs
'Imports ESRI.ArcGIS.Framework
'Imports ESRI.ArcGIS.ArcMapUI

'#If ADD_NET Then
'<ComClass(VisualManoeuvringCMD.ClassId, VisualManoeuvringCMD.InterfaceId, VisualManoeuvringCMD.EventsId), ProgId("Arrival_NET.VisualManoeuvringCMD")> Public NotInheritable Class VisualManoeuvringCMD
'#Else
'<ComClass(VisualManoeuvringCMD.ClassId, VisualManoeuvringCMD.InterfaceId, VisualManoeuvringCMD.EventsId), ProgId("Arrival.VisualManoeuvringCMD")> Public NotInheritable Class VisualManoeuvringCMD
'#End If
'    Inherits BaseCommand

'#Region "COM GUIDs"
'    ' These  GUIDs provide the COM identity for this class 
'    ' and its COM interfaces. If you change them, existing 
'    ' clients will no longer be able to access the class.
'    Public Const ClassId As String = "23137ada-d49a-42a3-b747-63cf03334158"
'    Public Const InterfaceId As String = "2cee13f6-c36b-48e9-8467-33699e9ba490"
'    Public Const EventsId As String = "c9211367-1834-4804-ae90-fa191c0cea65"
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
'        MyBase.m_category = "PANDA_NET.VisualManoeuvring"  'localizable text 
'        MyBase.m_name = "VisualManoeuvringCMD_NET"   'unique id, non-localizable
'        MyBase.m_caption = My.Resources.str161 + "_NET" '"Линия пути счисления"
'#Else
'		MyBase.m_category = "PANDA.InitialArea"	 'localizable text 
'		MyBase.m_name = "VisualManoeuvringCMD"	 'unique id, non-localizable
'		MyBase.m_caption =  My.Resources.str160 '"Линия пути счисления"
'#End If

'        MyBase.m_helpFile = GlobalVars.HelpFile
'        MyBase.m_helpID = 13000
'        MyBase.m_bitmap = Nothing

'        MyBase.m_message = My.Resources.str161 '"Линия пути счисления"
'        MyBase.m_toolTip = My.Resources.str161 '"Линия пути счисления"
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
'            Caption = My.Resources.str161 + "_NET"  '"Линия пути счисления"
'#Else
'			Caption = My.Resources.str160	'"Линия пути счисления"
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

'            Message = My.Resources.str161 '"Линия пути счисления"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Tooltip() As String
'        Get
'            Dim Value As Object
'            Value = RegRead(PANDARootKey, PandaRegKey, "LanguageCode")
'            LangCode = If(IsNumeric(Value), CInt(Value), 1033)

'            SetThreadLocale(LangCode)
'            Arrival.My.Resources.Culture = New System.Globalization.CultureInfo(LangCode)

'            Tooltip = My.Resources.str161 '"Линия пути счисления"
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Checked() As Boolean
'        Get
'            Checked = CurrCmd = 5
'        End Get
'    End Property

'    Public Overrides ReadOnly Property Enabled() As Boolean
'        Get
'            Enabled = m_enabled And ((CurrCmd = 0) Or (CurrCmd = 5))
'        End Get
'    End Property

'    Dim VisualManoeuvring As VisualManoeuvringForm

'    Public Overrides Sub OnClick()
'        If CurrCmd = 5 Then Return

'        If InitCommand() > 0 Then
'            CurrCmd = 5
'            VisualManoeuvring = New VisualManoeuvringForm
'            VisualManoeuvring.Show(s_Win32Window)
'        Else
'            CurrCmd = 0
'        End If
'    End Sub
'End Class
