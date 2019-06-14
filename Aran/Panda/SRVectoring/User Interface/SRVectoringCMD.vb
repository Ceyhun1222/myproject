'Option Strict Off
'Option Explicit On
'<System.Runtime.InteropServices.ProgId("SRVectoringCMD.SRVectoringCMD")> Public Class SRVectoringCMD
'	Implements ESRI.ArcGIS.SystemUI.ICommand


'	Dim m_ARRIVALBitmap As System.Drawing.Image


'	Protected Overrides Sub Finalize()
'		TerminateCommand()
'		MyBase.Finalize()
'	End Sub

'	Private ReadOnly Property ICommand_Bitmap() As Integer Implements ESRI.ArcGIS.SystemUI.ICommand.Bitmap
'		Get
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Caption() As String Implements ESRI.ArcGIS.SystemUI.ICommand.Caption
'		Get
'			ICommand_Caption = My.Resources.str1000	'"Radar vectoring"
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Category() As String Implements ESRI.ArcGIS.SystemUI.ICommand.Category
'		Get
'			ICommand_Category = "PANDA.Radar Vectoring"	'2007
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Checked() As Boolean Implements ESRI.ArcGIS.SystemUI.ICommand.Checked
'		Get
'			ICommand_Checked = False
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Enabled() As Boolean Implements ESRI.ArcGIS.SystemUI.ICommand.Enabled
'		Get
'			ICommand_Enabled = True
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_HelpContextID() As Integer Implements ESRI.ArcGIS.SystemUI.ICommand.HelpContextID
'		Get
'			ICommand_HelpContextID = 30100
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_HelpFile() As String Implements ESRI.ArcGIS.SystemUI.ICommand.HelpFile
'		Get
'			ICommand_HelpFile = "SRVectoring.chm"
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Message() As String Implements ESRI.ArcGIS.SystemUI.ICommand.Message
'		Get
'			ICommand_Message = "Terminal Procedure"	'LoadResString(157)
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Name() As String Implements ESRI.ArcGIS.SystemUI.ICommand.Name
'		Get
'			ICommand_Name = "SRVectoringCMD"
'		End Get
'	End Property

'	Private ReadOnly Property ICommand_Tooltip() As String Implements ESRI.ArcGIS.SystemUI.ICommand.Tooltip
'		Get
'			ICommand_Tooltip = My.Resources.str1000	'"Radar Vectoring"
'		End Get
'	End Property

'	Private Sub ICommand_OnClick() Implements ESRI.ArcGIS.SystemUI.ICommand.OnClick
'		'    If Not LimDat Then Exit Sub
'		If InitCommand() > 0 Then
'			Dim _FrmManevre As New FrmManevre()
'			_FrmManevre.Show(s_Win32Window)
'		End If
'	End Sub

'	Private Sub ICommand_OnCreate(ByVal hook As Object) Implements ESRI.ArcGIS.SystemUI.ICommand.OnCreate
'		Application = hook
'		pDocument = Application.Document
'		pMap = pDocument.FocusMap
'	End Sub
'End Class
