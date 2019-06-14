'Option Strict Off
'Option Explicit On
'<System.Runtime.InteropServices.ProgId("SRVectoring_Toolbar.SRVectoring_Toolbar")> Public Class SRVectoring_Toolbar
'	Implements ESRI.ArcGIS.SystemUI.IToolBarDef
'	' This class shows how to create a custom toolbar.
'	' This toolbar has three built-in ArcMap commands on it.

'	' Implement the IToolBarDef interface

'	Private ReadOnly Property IToolBarDef_Caption() As String Implements ESRI.ArcGIS.SystemUI.IToolBarDef.Caption
'		Get
'			' Set the string that appears as the toolbar's title
'			IToolBarDef_Caption = "PANDA - " & My.Resources.str1000	'SRVectoring"
'		End Get
'	End Property

'	Private ReadOnly Property IToolBarDef_ItemCount() As Integer Implements ESRI.ArcGIS.SystemUI.IToolBarDef.ItemCount
'		Get
'			' Set how many commands will be on the toolbar
'			IToolBarDef_ItemCount = 1
'		End Get
'	End Property

'	Private ReadOnly Property IToolBarDef_Name() As String Implements ESRI.ArcGIS.SystemUI.IToolBarDef.Name
'		Get
'			' Set the internal name of the toolbar.
'			IToolBarDef_Name = "PANDA.SRVectoring"
'		End Get
'	End Property

'	Private Sub IToolBarDef_GetItemInfo(ByVal Pos As Integer, ByVal itemDef As ESRI.ArcGIS.SystemUI.IItemDef) Implements ESRI.ArcGIS.SystemUI.IToolBarDef.GetItemInfo
'		' Define the commands that will be on the toolbar. The built-in ArcMap AddData
'		' command, Full Extent command, and Pan tool are added to this custom toolbar.
'		' ID is the ClassID of the command. Group determines whether the command
'		' begins a new group on the toolbar.
'		Select Case Pos
'			Case 0
'				itemDef.ID = "SRVectoring.SRVectoring_Menu"
'				itemDef.Group = False
'		End Select
'	End Sub
'End Class
