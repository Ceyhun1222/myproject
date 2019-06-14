'Option Strict Off
'Option Explicit On
'<System.Runtime.InteropServices.ProgId("SRVectoring_Menu.SRVectoring_Menu")> Public Class SRVectoring_Menu
'	Implements ESRI.ArcGIS.SystemUI.IMenuDef
'	Implements ESRI.ArcGIS.Framework.IRootLevelMenu

'	'Implement the IMenuDef interface and IRootLevelMenu interface
'	'Implements IShortcutMenu    ' Allows this menu should be treated as a context menu

'	Private ReadOnly Property IMenuDef_Caption() As String Implements ESRI.ArcGIS.SystemUI.IMenuDef.Caption
'		Get
'			IMenuDef_Caption = My.Resources.str1000	'"Radar Vectoring"
'		End Get
'	End Property

'	Private ReadOnly Property IMenuDef_ItemCount() As Integer Implements ESRI.ArcGIS.SystemUI.IMenuDef.ItemCount
'		Get
'			IMenuDef_ItemCount = 1
'		End Get
'	End Property

'	Private ReadOnly Property IMenuDef_Name() As String Implements ESRI.ArcGIS.SystemUI.IMenuDef.Name
'		Get
'			IMenuDef_Name = "PANDA.SRVectoring_Menu"
'		End Get
'	End Property

'	Private Sub IMenuDef_GetItemInfo(ByVal Pos As Integer, ByVal itemDef As ESRI.ArcGIS.SystemUI.IItemDef) Implements ESRI.ArcGIS.SystemUI.IMenuDef.GetItemInfo
'		Select Case Pos
'			Case 0
'				itemDef.ID = "SRVectoring.SRVectoringCMD"
'				itemDef.Group = False
'		End Select
'	End Sub
'End Class
