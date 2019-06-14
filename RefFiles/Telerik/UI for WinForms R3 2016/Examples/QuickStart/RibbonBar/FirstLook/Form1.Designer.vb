Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.RibbonBar.FirstLook
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim radListDataItem7 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem8 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem9 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem10 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem11 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem12 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem13 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem14 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem15 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem16 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem17 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem18 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem21 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem22 As New Telerik.WinControls.UI.RadListDataItem()
			Me.radRibbonBarCommandTab1 = New Telerik.WinControls.UI.RibbonTab()
			Me.radRibbonBarChunk1 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement1 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem50 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem66 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarButtonGroup1 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement2 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement3 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement1 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarChunk3 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement2 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuHeaderItem1 = New Telerik.WinControls.UI.RadMenuHeaderItem()
			Me.radMenuItem9 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem10 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuHeaderItem2 = New Telerik.WinControls.UI.RadMenuHeaderItem()
			Me.radMenuItem11 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem15 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem16 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem17 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarButtonGroup6 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement19 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement20 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radDropDownButtonElement3 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem18 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem19 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem20 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem21 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem22 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarChunk4 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radRibbonBarButtonGroup7 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement16 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement17 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement18 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup9 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement9 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement14 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement15 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup2 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement4 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem44 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem23 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem36 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem38 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem39 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem40 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem41 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem42 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem43 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarChunk10 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radGalleryElement1 = New Telerik.WinControls.UI.RadGalleryElement()
			Me.radGalleryItem75 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem1 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem3 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem76 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem15 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem7 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem77 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem16 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem73 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem5 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem13 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem14 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem6 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem74 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem2 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem8 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem71 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem72 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem9 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem10 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem11 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem4 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radGalleryItem12 = New Telerik.WinControls.UI.RadGalleryItem()
			Me.radMenuSeparatorItem1 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuButtonItem1 = New Telerik.WinControls.UI.RadMenuButtonItem()
			Me.radRibbonBarButtonGroup10 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radDropDownButtonElement5 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem12 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem13 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem14 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem24 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem25 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem26 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem27 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radDropDownButtonElement9 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem28 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem29 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem30 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem31 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem32 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem33 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem34 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarGroup4 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement8 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem35 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem45 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem46 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem47 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarGroup5 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement25 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement26 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup14 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement49 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radOffice2007ScreenTip2 = New Telerik.WinControls.UI.RadOffice2007ScreenTip()
			Me.radOffice2007ScreenTip3 = New Telerik.WinControls.UI.RadOffice2007ScreenTip()
			Me.radOffice2007ScreenTip4 = New Telerik.WinControls.UI.RadOffice2007ScreenTip()
			Me.radOffice2007ScreenTip1 = New Telerik.WinControls.UI.RadOffice2007ScreenTip()
			Me.radRibbonBarCommandTab2 = New Telerik.WinControls.UI.RibbonTab()
			Me.radRibbonBarGroup1 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement5 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement29 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement30 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup6 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radCheckBoxElement1 = New Telerik.WinControls.UI.RadCheckBoxElement()
			Me.radCheckBoxElement2 = New Telerik.WinControls.UI.RadCheckBoxElement()
			Me.radCheckBoxElement3 = New Telerik.WinControls.UI.RadCheckBoxElement()
			Me.radRibbonBarGroup7 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement31 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement32 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup14 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement46 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement50 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement51 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radGalleryGroupFilter4 = New Telerik.WinControls.UI.RadGalleryGroupFilter()
			Me.radGalleryGroupItem3 = New Telerik.WinControls.UI.RadGalleryGroupItem()
			Me.radGalleryGroupItem4 = New Telerik.WinControls.UI.RadGalleryGroupItem()
			Me.radRibbonBarCommandTab3 = New Telerik.WinControls.UI.RibbonTab()
			Me.radRibbonBarGroup9 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement12 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radMenuItem37 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem48 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radRibbonBarButtonGroup3 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement52 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement53 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement40 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup11 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radRibbonBarButtonGroup25 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radDropDownListElement1 = New Telerik.WinControls.UI.RadDropDownListElement()
			Me.radDropDownListElement2 = New Telerik.WinControls.UI.RadDropDownListElement()
			Me.radRibbonBarButtonGroup26 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup30 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement75 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement86 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement87 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement88 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement89 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement90 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup31 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement91 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement92 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup12 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement76 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement77 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup13 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement27 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement44 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup15 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement45 = New Telerik.WinControls.UI.RadButtonElement()
			Me.contextualTabGroup1 = New Telerik.WinControls.UI.ContextualTabGroup()
			Me.radRibbonBar1 = New Telerik.WinControls.UI.RadRibbonBar()
			Me.radRibbonBarBackstageView1 = New Telerik.WinControls.UI.RadRibbonBarBackstageView()
			Me.backstageViewPage1 = New Telerik.WinControls.UI.BackstageViewPage()
			Me.pictureBox2 = New PictureBox()
			Me.radSeparator2 = New Telerik.WinControls.UI.RadSeparator()
			Me.radButton3 = New Telerik.WinControls.UI.RadButton()
			Me.radButton2 = New Telerik.WinControls.UI.RadButton()
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			Me.radSeparator1 = New Telerik.WinControls.UI.RadSeparator()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.backstageViewPage2 = New Telerik.WinControls.UI.BackstageViewPage()
			Me.radButton6 = New Telerik.WinControls.UI.RadButton()
			Me.radButton5 = New Telerik.WinControls.UI.RadButton()
			Me.radButton4 = New Telerik.WinControls.UI.RadButton()
			Me.radSeparator3 = New Telerik.WinControls.UI.RadSeparator()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.backstageButtonItem1 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageButtonItem2 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageButtonItem3 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageButtonItem4 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageTabItem1 = New Telerik.WinControls.UI.BackstageTabItem()
			Me.backstageTabItem2 = New Telerik.WinControls.UI.BackstageTabItem()
			Me.backstageButtonItem5 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageButtonItem6 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.backstageButtonItem7 = New Telerik.WinControls.UI.BackstageButtonItem()
			Me.imageList1 = New ImageList(Me.components)
			Me.radButtonElement41 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement42 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement47 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement48 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radMenuItem59 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem2 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem51 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem3 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem2 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem52 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuHeaderItem3 = New Telerik.WinControls.UI.RadMenuHeaderItem()
			Me.radMenuItem60 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem61 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem62 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem53 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem54 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem6 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem55 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuHeaderItem4 = New Telerik.WinControls.UI.RadMenuHeaderItem()
			Me.radMenuItem63 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem64 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem65 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem3 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem56 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem4 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem57 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem5 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem58 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem6 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem7 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem8 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem1 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radButtonElement43 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup4 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement4 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement6 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement7 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup5 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement8 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement10 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement11 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement13 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement12 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup11 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup12 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup18 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup19 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement28 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup13 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup8 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup2 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarGroup3 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement22 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement23 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement24 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radDropDownButtonElement6 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radDropDownButtonElement7 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radButtonElement21 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radPanelContent = New Telerik.WinControls.UI.RadPanel()
			Me.radScrollablePanel1 = New Telerik.WinControls.UI.RadScrollablePanel()
			Me.panel1 = New Panel()
			Me.pictureBoxTool = New PictureBox()
			Me.pictureBox1 = New PictureBox()
			Me.radRibbonBarGroup8 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement10 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radRibbonBarButtonGroup20 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup21 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement33 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement34 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement35 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup10 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radDropDownButtonElement11 = New Telerik.WinControls.UI.RadDropDownButtonElement()
			Me.radRibbonBarButtonGroup22 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup23 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement36 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement37 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement38 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement39 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup27 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radRibbonBarButtonGroup28 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement78 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement79 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement80 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement81 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement82 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement83 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarButtonGroup29 = New Telerik.WinControls.UI.RadRibbonBarButtonGroup()
			Me.radButtonElement84 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement85 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radRibbonBarGroup15 = New Telerik.WinControls.UI.RadRibbonBarGroup()
			Me.radButtonElement54 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement55 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radButtonElement56 = New Telerik.WinControls.UI.RadButtonElement()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radOffice2007ScreenTip2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radOffice2007ScreenTip3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radOffice2007ScreenTip4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radOffice2007ScreenTip1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDropDownListElement1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDropDownListElement2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRibbonBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRibbonBarBackstageView1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radRibbonBarBackstageView1.SuspendLayout()
			Me.backstageViewPage1.SuspendLayout()
			CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSeparator2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSeparator1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.backstageViewPage2.SuspendLayout()
			CType(Me.radButton6, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton5, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton4, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSeparator3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanelContent, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelContent.SuspendLayout()
			CType(Me.radScrollablePanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radScrollablePanel1.PanelContainer.SuspendLayout()
			Me.radScrollablePanel1.SuspendLayout()
			Me.panel1.SuspendLayout()
			CType(Me.pictureBoxTool, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Anchor = (CType((AnchorStyles.Top Or AnchorStyles.Left), AnchorStyles))
			Me.settingsPanel.ForeColor = Color.Black
			Me.settingsPanel.Location = New Point(1103, 1)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.ForeColor = Color.Black
			Me.settingsPanel.Size = New Size(200, 598)
			' 
			' radRibbonBarCommandTab1
			' 
			Me.radRibbonBarCommandTab1.AccessibleDescription = "Home"
			Me.radRibbonBarCommandTab1.AccessibleName = "Home"
			Me.radRibbonBarCommandTab1.Alignment = ContentAlignment.BottomLeft
			Me.radRibbonBarCommandTab1.CanFocus = True
			Me.radRibbonBarCommandTab1.Class = "TabItem"
			' 
			' radRibbonBarCommandTab1.ContentPanel
			' 
			Me.radRibbonBarCommandTab1.ContentPanel.BackColor = Color.Transparent
			Me.radRibbonBarCommandTab1.ContentPanel.CausesValidation = True
			Me.radRibbonBarCommandTab1.ContentPanel.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radRibbonBarCommandTab1.ContentPanel.ForeColor = SystemColors.ControlText
			Me.radRibbonBarCommandTab1.ContentPanel.Location = New Point(0, 0)
			Me.radRibbonBarCommandTab1.ContentPanel.Size = New Size(200, 100)
			Me.radRibbonBarCommandTab1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarChunk1, Me.radRibbonBarChunk3, Me.radRibbonBarChunk4, Me.radRibbonBarGroup2, Me.radRibbonBarChunk10, Me.radRibbonBarGroup4, Me.radRibbonBarGroup5})
			Me.radRibbonBarCommandTab1.Name = "radRibbonBarCommandTab1"
			Me.radRibbonBarCommandTab1.StretchHorizontally = False
			Me.radRibbonBarCommandTab1.Text = "Home"
			Me.radRibbonBarCommandTab1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarChunk1
			' 
			Me.radRibbonBarChunk1.AccessibleDescription = "Clipboard"
			Me.radRibbonBarChunk1.AccessibleName = "Clipboard"
			Me.radRibbonBarChunk1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement1, Me.radRibbonBarButtonGroup1})
			Me.radRibbonBarChunk1.MinSize = New Size(70, 77)
			Me.radRibbonBarChunk1.Name = "radRibbonBarChunk1"
			Me.radRibbonBarChunk1.Text = "Clipboard"
			Me.radRibbonBarChunk1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement1
			' 
			Me.radDropDownButtonElement1.AccessibleDescription = "Paste"
			Me.radDropDownButtonElement1.AccessibleName = "Paste"
			Me.radDropDownButtonElement1.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement1.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement1.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement1.ExpandArrowButton = False
			Me.radDropDownButtonElement1.Image = My.Resources.new_paste_32
			Me.radDropDownButtonElement1.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem50, Me.radMenuItem66})
			Me.radDropDownButtonElement1.Margin = New Padding(2)
			Me.radDropDownButtonElement1.Name = "radDropDownButtonElement1"
			Me.radDropDownButtonElement1.Text = "Paste"
			Me.radDropDownButtonElement1.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement1.TextImageRelation = TextImageRelation.Overlay
			Me.radDropDownButtonElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem50
			' 
			Me.radMenuItem50.AccessibleDescription = "Paste"
			Me.radMenuItem50.AccessibleName = "Paste"
			Me.radMenuItem50.Image = My.Resources.new_paste_16
			Me.radMenuItem50.Name = "radMenuItem50"
			Me.radMenuItem50.Text = "Paste"
			Me.radMenuItem50.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem66
			' 
			Me.radMenuItem66.AccessibleDescription = "Paste from"
			Me.radMenuItem66.AccessibleName = "Paste from"
			Me.radMenuItem66.Image = My.Resources.paste_form
			Me.radMenuItem66.Name = "radMenuItem66"
			Me.radMenuItem66.Text = "Paste from"
			Me.radMenuItem66.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup1
			' 
			Me.radRibbonBarButtonGroup1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement2, Me.radButtonElement3, Me.radButtonElement1})
			Me.radRibbonBarButtonGroup1.Margin = New Padding(2, 2, 2, 0)
			Me.radRibbonBarButtonGroup1.Name = "radRibbonBarButtonGroup1"
			Me.radRibbonBarButtonGroup1.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup1.StretchVertically = False
			Me.radRibbonBarButtonGroup1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement2
			' 
			Me.radButtonElement2.AccessibleDescription = "Cut"
			Me.radButtonElement2.AccessibleName = "Cut"
			Me.radButtonElement2.Class = "ButtonElement"
			Me.radButtonElement2.Image = (CType(resources.GetObject("radButtonElement2.Image"), Image))
			Me.radButtonElement2.ImageIndex = 6
			Me.radButtonElement2.Name = "radButtonElement2"
			Me.radButtonElement2.ShowBorder = False
			Me.radButtonElement2.Text = "Cut"
			Me.radButtonElement2.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement3
			' 
			Me.radButtonElement3.AccessibleDescription = "Copy"
			Me.radButtonElement3.AccessibleName = "Copy"
			Me.radButtonElement3.Class = "ButtonElement"
			Me.radButtonElement3.Image = (CType(resources.GetObject("radButtonElement3.Image"), Image))
			Me.radButtonElement3.ImageIndex = 4
			Me.radButtonElement3.Name = "radButtonElement3"
			Me.radButtonElement3.ShowBorder = False
			Me.radButtonElement3.Text = "Copy"
			Me.radButtonElement3.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement1
			' 
			Me.radButtonElement1.AccessibleDescription = "Paste"
			Me.radButtonElement1.AccessibleName = "Paste"
			Me.radButtonElement1.Class = "ButtonElement"
			Me.radButtonElement1.Image = (CType(resources.GetObject("radButtonElement1.Image"), Image))
			Me.radButtonElement1.ImageIndex = 25
			Me.radButtonElement1.Name = "radButtonElement1"
			Me.radButtonElement1.ShowBorder = False
			Me.radButtonElement1.Text = "Paste"
			Me.radButtonElement1.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarChunk3
			' 
			Me.radRibbonBarChunk3.AccessibleDescription = "Image"
			Me.radRibbonBarChunk3.AccessibleName = "Image"
			Me.radRibbonBarChunk3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement2, Me.radRibbonBarButtonGroup6})
			Me.radRibbonBarChunk3.Name = "radRibbonBarChunk3"
			Me.radRibbonBarChunk3.Text = "Image"
			Me.radRibbonBarChunk3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement2
			' 
			Me.radDropDownButtonElement2.AccessibleDescription = "Select"
			Me.radDropDownButtonElement2.AccessibleName = "Select"
			Me.radDropDownButtonElement2.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement2.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement2.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement2.ExpandArrowButton = False
            Me.radDropDownButtonElement2.Image = My.Resources._select
			Me.radDropDownButtonElement2.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement2.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuHeaderItem1, Me.radMenuItem9, Me.radMenuItem10, Me.radMenuHeaderItem2, Me.radMenuItem11, Me.radMenuItem15, Me.radMenuItem16, Me.radMenuItem17})
			Me.radDropDownButtonElement2.Name = "radDropDownButtonElement2"
			Me.radDropDownButtonElement2.Text = "Select"
			Me.radDropDownButtonElement2.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement2.TextImageRelation = TextImageRelation.Overlay
			Me.radDropDownButtonElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuHeaderItem1
			' 
			Me.radMenuHeaderItem1.AccessibleDescription = "Selected shapes"
			Me.radMenuHeaderItem1.AccessibleName = "Selected shapes"
			Me.radMenuHeaderItem1.Name = "radMenuHeaderItem1"
			Me.radMenuHeaderItem1.Text = "Selected shapes"
			Me.radMenuHeaderItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem9
			' 
			Me.radMenuItem9.AccessibleDescription = "Rectangular selection"
			Me.radMenuItem9.AccessibleName = "Rectangular selection"
			Me.radMenuItem9.Image = My.Resources.rect_select
			Me.radMenuItem9.Name = "radMenuItem9"
			Me.radMenuItem9.Text = "Rectangular selection"
			Me.radMenuItem9.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem10
			' 
			Me.radMenuItem10.AccessibleDescription = "Free-form selection"
			Me.radMenuItem10.AccessibleName = "Free-form selection"
			Me.radMenuItem10.Image = My.Resources.free_form_selection
			Me.radMenuItem10.Name = "radMenuItem10"
			Me.radMenuItem10.Text = "Free-form selection"
			Me.radMenuItem10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuHeaderItem2
			' 
			Me.radMenuHeaderItem2.AccessibleDescription = "Selection options"
			Me.radMenuHeaderItem2.AccessibleName = "Selection options"
			Me.radMenuHeaderItem2.Name = "radMenuHeaderItem2"
			Me.radMenuHeaderItem2.Text = "Selection options"
			Me.radMenuHeaderItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem11
			' 
			Me.radMenuItem11.AccessibleDescription = "Select all"
			Me.radMenuItem11.AccessibleName = "Select all"
			Me.radMenuItem11.Image = My.Resources.select_all
			Me.radMenuItem11.Name = "radMenuItem11"
			Me.radMenuItem11.Text = "Select all"
			Me.radMenuItem11.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem15
			' 
			Me.radMenuItem15.AccessibleDescription = "Invert selection"
			Me.radMenuItem15.AccessibleName = "Invert selection"
			Me.radMenuItem15.Enabled = False
			Me.radMenuItem15.Image = My.Resources.invert_selection_disable
			Me.radMenuItem15.Name = "radMenuItem15"
			Me.radMenuItem15.Text = "Invert selection"
			Me.radMenuItem15.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem16
			' 
			Me.radMenuItem16.AccessibleDescription = "Delete"
			Me.radMenuItem16.AccessibleName = "Delete"
			Me.radMenuItem16.Enabled = False
			Me.radMenuItem16.Image = My.Resources.delete_disable
			Me.radMenuItem16.Name = "radMenuItem16"
			Me.radMenuItem16.Text = "Delete"
			Me.radMenuItem16.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem17
			' 
			Me.radMenuItem17.AccessibleDescription = "Transperant selection"
			Me.radMenuItem17.AccessibleName = "Transperant selection"
			Me.radMenuItem17.Name = "radMenuItem17"
			Me.radMenuItem17.Text = "Transperant selection"
			Me.radMenuItem17.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup6
			' 
			Me.radRibbonBarButtonGroup6.Alignment = ContentAlignment.BottomCenter
			Me.radRibbonBarButtonGroup6.BackColor = Color.FromArgb((CInt(Fix((CByte(0))))), (CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))))
			Me.radRibbonBarButtonGroup6.ForeColor = Color.FromArgb((CInt(Fix((CByte(0))))), (CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))))
			Me.radRibbonBarButtonGroup6.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement19, Me.radButtonElement20, Me.radDropDownButtonElement3})
			Me.radRibbonBarButtonGroup6.Name = "radRibbonBarButtonGroup6"
			Me.radRibbonBarButtonGroup6.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup6.Padding = New Padding(2, 2, 2, 0)
			Me.radRibbonBarButtonGroup6.ShowBackColor = False
			Me.radRibbonBarButtonGroup6.ShowBorder = False
			Me.radRibbonBarButtonGroup6.Text = ""
			Me.radRibbonBarButtonGroup6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement19
			' 
			Me.radButtonElement19.AccessibleDescription = "radButtonElement19"
			Me.radButtonElement19.AccessibleName = "radButtonElement19"
			Me.radButtonElement19.Class = "RibbonBarButtonElement"
			Me.radButtonElement19.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement19.Image = My.Resources.crop
			Me.radButtonElement19.MinSize = New Size(21, 21)
			Me.radButtonElement19.Name = "radButtonElement19"
			Me.radButtonElement19.Text = "radButtonElement19"
			Me.radButtonElement19.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement20
			' 
			Me.radButtonElement20.AccessibleDescription = "radButtonElement20"
			Me.radButtonElement20.AccessibleName = "radButtonElement20"
			Me.radButtonElement20.Class = "RibbonBarButtonElement"
			Me.radButtonElement20.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement20.Image = My.Resources.resize
			Me.radButtonElement20.MinSize = New Size(21, 21)
			Me.radButtonElement20.Name = "radButtonElement20"
			Me.radButtonElement20.Text = "radButtonElement20"
			Me.radButtonElement20.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement3
			' 
			Me.radDropDownButtonElement3.AccessibleDescription = "radDropDownButtonElement3"
			Me.radDropDownButtonElement3.AccessibleName = "radDropDownButtonElement3"
			Me.radDropDownButtonElement3.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement3.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radDropDownButtonElement3.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement3.ExpandArrowButton = False
			Me.radDropDownButtonElement3.Image = My.Resources.rotate1
			Me.radDropDownButtonElement3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem18, Me.radMenuItem19, Me.radMenuItem20, Me.radMenuItem21, Me.radMenuItem22})
			Me.radDropDownButtonElement3.Name = "radDropDownButtonElement3"
			Me.radDropDownButtonElement3.Text = "radDropDownButtonElement3"
			Me.radDropDownButtonElement3.TextImageRelation = TextImageRelation.Overlay
			Me.radDropDownButtonElement3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem18
			' 
			Me.radMenuItem18.AccessibleDescription = "Rotate right 90°"
			Me.radMenuItem18.AccessibleName = "Rotate right 90°"
			Me.radMenuItem18.Image = My.Resources.rotate1
			Me.radMenuItem18.Name = "radMenuItem18"
			Me.radMenuItem18.Text = "Rotate right 90°"
			Me.radMenuItem18.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem19
			' 
			Me.radMenuItem19.AccessibleDescription = "Rotate left 90°"
			Me.radMenuItem19.AccessibleName = "Rotate left 90°"
			Me.radMenuItem19.Image = My.Resources.rotate2
			Me.radMenuItem19.Name = "radMenuItem19"
			Me.radMenuItem19.Text = "Rotate left 90°"
			Me.radMenuItem19.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem20
			' 
			Me.radMenuItem20.AccessibleDescription = "Rotate 180°"
			Me.radMenuItem20.AccessibleName = "Rotate 180°"
			Me.radMenuItem20.Image = My.Resources.rotate3
			Me.radMenuItem20.Name = "radMenuItem20"
			Me.radMenuItem20.Text = "Rotate 180°"
			Me.radMenuItem20.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem21
			' 
			Me.radMenuItem21.AccessibleDescription = "Flip vertical"
			Me.radMenuItem21.AccessibleName = "Flip vertical"
			Me.radMenuItem21.Image = My.Resources.rotate41
			Me.radMenuItem21.Name = "radMenuItem21"
			Me.radMenuItem21.Text = "Flip vertical"
			Me.radMenuItem21.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem22
			' 
			Me.radMenuItem22.AccessibleDescription = "Flip horizontal"
			Me.radMenuItem22.AccessibleName = "Flip horizontal"
			Me.radMenuItem22.Image = My.Resources.rotate5
			Me.radMenuItem22.Name = "radMenuItem22"
			Me.radMenuItem22.Text = "Flip horizontal"
			Me.radMenuItem22.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarChunk4
			' 
			Me.radRibbonBarChunk4.AccessibleDescription = "Tools"
			Me.radRibbonBarChunk4.AccessibleName = "Tools"
			Me.radRibbonBarChunk4.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup7, Me.radRibbonBarButtonGroup9})
			Me.radRibbonBarChunk4.Name = "radRibbonBarChunk4"
			Me.radRibbonBarChunk4.Orientation = Orientation.Vertical
			Me.radRibbonBarChunk4.Text = "Tools"
			Me.radRibbonBarChunk4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup7
			' 
			Me.radRibbonBarButtonGroup7.AccessibleDescription = "radRibbonBarButtonGroup7"
			Me.radRibbonBarButtonGroup7.AccessibleName = "radRibbonBarButtonGroup7"
			Me.radRibbonBarButtonGroup7.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement16, Me.radButtonElement17, Me.radButtonElement18})
			Me.radRibbonBarButtonGroup7.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup7.Name = "radRibbonBarButtonGroup7"
			Me.radRibbonBarButtonGroup7.Text = "radRibbonBarButtonGroup7"
			Me.radRibbonBarButtonGroup7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement16
			' 
			Me.radButtonElement16.AccessibleDescription = "pen"
			Me.radButtonElement16.AccessibleName = "pen"
			Me.radButtonElement16.Class = "ButtonElement"
			Me.radButtonElement16.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement16.Image = My.Resources.pen
			Me.radButtonElement16.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement16.MinSize = New Size(22, 23)
			Me.radButtonElement16.Name = "radButtonElement16"
			Me.radButtonElement16.Text = "pen"
			Me.radButtonElement16.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement17
			' 
			Me.radButtonElement17.AccessibleDescription = "paint bucket"
			Me.radButtonElement17.AccessibleName = "paint bucket"
			Me.radButtonElement17.Class = "ButtonElement"
			Me.radButtonElement17.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement17.Image = My.Resources.paint_bucket
			Me.radButtonElement17.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement17.MinSize = New Size(22, 23)
			Me.radButtonElement17.Name = "radButtonElement17"
			Me.radButtonElement17.Text = "paint bucket"
			Me.radButtonElement17.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement18
			' 
			Me.radButtonElement18.AccessibleDescription = "text"
			Me.radButtonElement18.AccessibleName = "text"
			Me.radButtonElement18.Class = "ButtonElement"
			Me.radButtonElement18.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement18.Image = My.Resources.text
			Me.radButtonElement18.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement18.MinSize = New Size(22, 23)
			Me.radButtonElement18.Name = "radButtonElement18"
			Me.radButtonElement18.ShowBorder = False
			Me.radButtonElement18.Text = "text"
			Me.radButtonElement18.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup9
			' 
			Me.radRibbonBarButtonGroup9.AccessibleDescription = "radRibbonBarButtonGroup9"
			Me.radRibbonBarButtonGroup9.AccessibleName = "radRibbonBarButtonGroup9"
			Me.radRibbonBarButtonGroup9.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement9, Me.radButtonElement14, Me.radButtonElement15})
			Me.radRibbonBarButtonGroup9.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup9.Name = "radRibbonBarButtonGroup9"
			Me.radRibbonBarButtonGroup9.Text = "radRibbonBarButtonGroup9"
			Me.radRibbonBarButtonGroup9.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement9
			' 
			Me.radButtonElement9.AccessibleDescription = "erase"
			Me.radButtonElement9.AccessibleName = "erase"
			Me.radButtonElement9.Class = "ButtonElement"
			Me.radButtonElement9.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement9.Image = My.Resources.eraser
			Me.radButtonElement9.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement9.MinSize = New Size(22, 23)
			Me.radButtonElement9.Name = "radButtonElement9"
			Me.radButtonElement9.Text = "erase"
			Me.radButtonElement9.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement14
			' 
			Me.radButtonElement14.AccessibleDescription = "eyedropper"
			Me.radButtonElement14.AccessibleName = "eyedropper"
			Me.radButtonElement14.Class = "ButtonElement"
			Me.radButtonElement14.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement14.Image = My.Resources.Eyedropper
			Me.radButtonElement14.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement14.MinSize = New Size(22, 23)
			Me.radButtonElement14.Name = "radButtonElement14"
			Me.radButtonElement14.Text = "eyedropper"
			Me.radButtonElement14.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement15
			' 
			Me.radButtonElement15.AccessibleDescription = "zoom"
			Me.radButtonElement15.AccessibleName = "zoom"
			Me.radButtonElement15.Class = "ButtonElement"
			Me.radButtonElement15.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement15.Image = My.Resources.zoom
			Me.radButtonElement15.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement15.MinSize = New Size(22, 23)
			Me.radButtonElement15.Name = "radButtonElement15"
			Me.radButtonElement15.ShowBorder = False
			Me.radButtonElement15.Text = "zoom"
			Me.radButtonElement15.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup2
			' 
			Me.radRibbonBarGroup2.AccessibleDescription = "Brushes"
			Me.radRibbonBarGroup2.AccessibleName = "Brushes"
			Me.radRibbonBarGroup2.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement4})
			Me.radRibbonBarGroup2.Name = "radRibbonBarGroup2"
			Me.radRibbonBarGroup2.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup2.Text = "Brushes"
			Me.radRibbonBarGroup2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement4
			' 
			Me.radDropDownButtonElement4.AccessibleDescription = "Brushes"
			Me.radDropDownButtonElement4.AccessibleName = "Brushes"
			Me.radDropDownButtonElement4.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement4.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement4.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement4.ExpandArrowButton = False
			Me.radDropDownButtonElement4.Image = My.Resources.brush1
			Me.radDropDownButtonElement4.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radDropDownButtonElement4.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem44, Me.radMenuItem23, Me.radMenuItem36, Me.radMenuItem38, Me.radMenuItem39, Me.radMenuItem40, Me.radMenuItem41, Me.radMenuItem42, Me.radMenuItem43})
			Me.radDropDownButtonElement4.MinSize = New Size(0, 72)
			Me.radDropDownButtonElement4.Name = "radDropDownButtonElement4"
			Me.radDropDownButtonElement4.Text = "Brushes"
			Me.radDropDownButtonElement4.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement4.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radDropDownButtonElement4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem44
			' 
			Me.radMenuItem44.AccessibleDescription = "Brush"
			Me.radMenuItem44.AccessibleName = "Brush"
			Me.radMenuItem44.Image = My.Resources.brush1
			Me.radMenuItem44.Name = "radMenuItem44"
			Me.radMenuItem44.Text = "Brush"
			Me.radMenuItem44.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem23
			' 
			Me.radMenuItem23.AccessibleDescription = "Calligraphy brush 1"
			Me.radMenuItem23.AccessibleName = "Calligraphy brush 1"
			Me.radMenuItem23.Image = My.Resources.brush2
			Me.radMenuItem23.Name = "radMenuItem23"
			Me.radMenuItem23.Text = "Calligraphy brush 1"
			Me.radMenuItem23.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem36
			' 
			Me.radMenuItem36.AccessibleDescription = "Calligraphy brush 2"
			Me.radMenuItem36.AccessibleName = "Calligraphy brush 2"
			Me.radMenuItem36.Image = My.Resources.brush3
			Me.radMenuItem36.Name = "radMenuItem36"
			Me.radMenuItem36.Text = "Calligraphy brush 2"
			Me.radMenuItem36.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem38
			' 
			Me.radMenuItem38.AccessibleDescription = "Air Brush"
			Me.radMenuItem38.AccessibleName = "Air Brush"
			Me.radMenuItem38.Image = My.Resources.brush4
			Me.radMenuItem38.Name = "radMenuItem38"
			Me.radMenuItem38.Text = "Air Brush"
			Me.radMenuItem38.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem39
			' 
			Me.radMenuItem39.AccessibleDescription = "Oil Brush"
			Me.radMenuItem39.AccessibleName = "Oil Brush"
			Me.radMenuItem39.Image = My.Resources.brush5
			Me.radMenuItem39.Name = "radMenuItem39"
			Me.radMenuItem39.Text = "Oil Brush"
			Me.radMenuItem39.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem40
			' 
			Me.radMenuItem40.AccessibleDescription = "Crayon"
			Me.radMenuItem40.AccessibleName = "Crayon"
			Me.radMenuItem40.Image = My.Resources.brush6
			Me.radMenuItem40.Name = "radMenuItem40"
			Me.radMenuItem40.Text = "Crayon"
			Me.radMenuItem40.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem41
			' 
			Me.radMenuItem41.AccessibleDescription = "Marker"
			Me.radMenuItem41.AccessibleName = "Marker"
			Me.radMenuItem41.Image = My.Resources.brush7
			Me.radMenuItem41.Name = "radMenuItem41"
			Me.radMenuItem41.Text = "Marker"
			Me.radMenuItem41.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem42
			' 
			Me.radMenuItem42.AccessibleDescription = "Natural pencil"
			Me.radMenuItem42.AccessibleName = "Natural pencil"
			Me.radMenuItem42.Image = My.Resources.brush8
			Me.radMenuItem42.Name = "radMenuItem42"
			Me.radMenuItem42.Text = "Natural pencil"
			Me.radMenuItem42.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem43
			' 
			Me.radMenuItem43.AccessibleDescription = "Watercolor brush"
			Me.radMenuItem43.AccessibleName = "Watercolor brush"
			Me.radMenuItem43.Image = My.Resources.brush9
			Me.radMenuItem43.Name = "radMenuItem43"
			Me.radMenuItem43.Text = "Watercolor brush"
			Me.radMenuItem43.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarChunk10
			' 
			Me.radRibbonBarChunk10.AccessibleDescription = "Shapes"
			Me.radRibbonBarChunk10.AccessibleName = "Shapes"
			Me.radRibbonBarChunk10.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radGalleryElement1, Me.radRibbonBarButtonGroup10})
			Me.radRibbonBarChunk10.Name = "radRibbonBarChunk10"
			Me.radRibbonBarChunk10.Text = "Shapes"
			Me.radRibbonBarChunk10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryElement1
			' 
			Me.radGalleryElement1.AccessibleDescription = "radGalleryElement1"
			Me.radGalleryElement1.AccessibleName = "radGalleryElement1"
			Me.radGalleryElement1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radGalleryItem75, Me.radGalleryItem1, Me.radGalleryItem3, Me.radGalleryItem76, Me.radGalleryItem15, Me.radGalleryItem7, Me.radGalleryItem77, Me.radGalleryItem16, Me.radGalleryItem73, Me.radGalleryItem5, Me.radGalleryItem13, Me.radGalleryItem14, Me.radGalleryItem6, Me.radGalleryItem74, Me.radGalleryItem2, Me.radGalleryItem8, Me.radGalleryItem71, Me.radGalleryItem72, Me.radGalleryItem9, Me.radGalleryItem10, Me.radGalleryItem11, Me.radGalleryItem4, Me.radGalleryItem12})
			Me.radGalleryElement1.ItemSelection = True
			Me.radGalleryElement1.Margin = New Padding(0, 4, 0, 0)
			Me.radGalleryElement1.MaxColumns = 7
			Me.radGalleryElement1.MaxSize = New Size(0, 60)
			Me.radGalleryElement1.Name = "radGalleryElement1"
			Me.radGalleryElement1.Padding = New Padding(2, 2, 0, 0)
			Me.radGalleryElement1.Text = "radGalleryElement1"
			Me.radGalleryElement1.Tools.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuSeparatorItem1, Me.radMenuButtonItem1})
			Me.radGalleryElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radGalleryItem75
			' 
			Me.radGalleryItem75.AccessibleDescription = "Line"
			Me.radGalleryItem75.AccessibleName = "Line"
			Me.radGalleryItem75.DescriptionText = ""
			Me.radGalleryItem75.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem75.Image = My.Resources.line
			Me.radGalleryItem75.Name = "radGalleryItem75"
			Me.radGalleryItem75.StretchHorizontally = False
			Me.radGalleryItem75.StretchVertically = False
			Me.radGalleryItem75.Text = "Line"
			Me.radGalleryItem75.ToolTipText = "Line"
			Me.radGalleryItem75.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem1
			' 
			Me.radGalleryItem1.AccessibleDescription = "Curve"
			Me.radGalleryItem1.AccessibleName = "Curve"
			Me.radGalleryItem1.DescriptionText = ""
			Me.radGalleryItem1.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem1.Image = (CType(resources.GetObject("radGalleryItem1.Image"), Image))
			Me.radGalleryItem1.Name = "radGalleryItem1"
			Me.radGalleryItem1.StretchHorizontally = False
			Me.radGalleryItem1.StretchVertically = False
			Me.radGalleryItem1.Text = "Curve"
			Me.radGalleryItem1.ToolTipText = "Curve"
			Me.radGalleryItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem3
			' 
			Me.radGalleryItem3.AccessibleDescription = "Oval"
			Me.radGalleryItem3.AccessibleName = "Oval"
			Me.radGalleryItem3.DescriptionText = ""
			Me.radGalleryItem3.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem3.Image = (CType(resources.GetObject("radGalleryItem3.Image"), Image))
			Me.radGalleryItem3.Name = "radGalleryItem3"
			Me.radGalleryItem3.StretchHorizontally = False
			Me.radGalleryItem3.StretchVertically = False
			Me.radGalleryItem3.Text = "Oval"
			Me.radGalleryItem3.ToolTipText = "Oval"
			Me.radGalleryItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem76
			' 
			Me.radGalleryItem76.AccessibleDescription = "Rectangle"
			Me.radGalleryItem76.AccessibleName = "Rectangle"
			Me.radGalleryItem76.DescriptionText = ""
			Me.radGalleryItem76.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem76.Image = My.Resources.rectangle
			Me.radGalleryItem76.Name = "radGalleryItem76"
			Me.radGalleryItem76.StretchHorizontally = False
			Me.radGalleryItem76.StretchVertically = False
			Me.radGalleryItem76.Text = "Rectangle"
			Me.radGalleryItem76.ToolTipText = "Rectangle"
			Me.radGalleryItem76.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem15
			' 
			Me.radGalleryItem15.AccessibleDescription = "Rounded rectangle"
			Me.radGalleryItem15.AccessibleName = "Rounded rectangle"
			Me.radGalleryItem15.DescriptionText = ""
			Me.radGalleryItem15.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem15.Image = (CType(resources.GetObject("radGalleryItem15.Image"), Image))
			Me.radGalleryItem15.Name = "radGalleryItem15"
			Me.radGalleryItem15.StretchHorizontally = False
			Me.radGalleryItem15.StretchVertically = False
			Me.radGalleryItem15.Text = "Rounded rectangle"
			Me.radGalleryItem15.ToolTipText = "Rounded rectangle"
			Me.radGalleryItem15.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem7
			' 
			Me.radGalleryItem7.AccessibleDescription = "Polygon"
			Me.radGalleryItem7.AccessibleName = "Polygon"
			Me.radGalleryItem7.DescriptionText = ""
			Me.radGalleryItem7.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem7.Image = (CType(resources.GetObject("radGalleryItem7.Image"), Image))
			Me.radGalleryItem7.Name = "radGalleryItem7"
			Me.radGalleryItem7.StretchHorizontally = False
			Me.radGalleryItem7.StretchVertically = False
			Me.radGalleryItem7.Text = "Polygon"
			Me.radGalleryItem7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem77
			' 
			Me.radGalleryItem77.AccessibleDescription = "Triangle"
			Me.radGalleryItem77.AccessibleName = "Triangle"
			Me.radGalleryItem77.DescriptionText = ""
			Me.radGalleryItem77.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem77.Image = My.Resources.triangle
			Me.radGalleryItem77.Name = "radGalleryItem77"
			Me.radGalleryItem77.StretchHorizontally = False
			Me.radGalleryItem77.StretchVertically = False
			Me.radGalleryItem77.Text = "Triangle"
			Me.radGalleryItem77.ToolTipText = "Triangle"
			Me.radGalleryItem77.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem16
			' 
			Me.radGalleryItem16.AccessibleDescription = "Right triangle"
			Me.radGalleryItem16.AccessibleName = "Right triangle"
			Me.radGalleryItem16.DescriptionText = ""
			Me.radGalleryItem16.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem16.Image = (CType(resources.GetObject("radGalleryItem16.Image"), Image))
			Me.radGalleryItem16.Name = "radGalleryItem16"
			Me.radGalleryItem16.StretchHorizontally = False
			Me.radGalleryItem16.StretchVertically = False
			Me.radGalleryItem16.Text = "Right triangle"
			Me.radGalleryItem16.ToolTipText = "Right triangle"
			Me.radGalleryItem16.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem73
			' 
			Me.radGalleryItem73.AccessibleDescription = "Diamond"
			Me.radGalleryItem73.AccessibleName = "Diamond"
			Me.radGalleryItem73.DescriptionText = ""
			Me.radGalleryItem73.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem73.Image = My.Resources.turned_rectangle
			Me.radGalleryItem73.Name = "radGalleryItem73"
			Me.radGalleryItem73.StretchHorizontally = False
			Me.radGalleryItem73.StretchVertically = False
			Me.radGalleryItem73.Text = "Diamond"
			Me.radGalleryItem73.ToolTipText = "Diamond"
			Me.radGalleryItem73.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem5
			' 
			Me.radGalleryItem5.AccessibleDescription = "Pendagon"
			Me.radGalleryItem5.AccessibleName = "Pendagon"
			Me.radGalleryItem5.DescriptionText = ""
			Me.radGalleryItem5.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem5.Image = (CType(resources.GetObject("radGalleryItem5.Image"), Image))
			Me.radGalleryItem5.Name = "radGalleryItem5"
			Me.radGalleryItem5.StretchHorizontally = False
			Me.radGalleryItem5.StretchVertically = False
			Me.radGalleryItem5.Text = "Pendagon"
			Me.radGalleryItem5.ToolTipText = "Pendagon"
			Me.radGalleryItem5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem13
			' 
			Me.radGalleryItem13.AccessibleDescription = "Hexagon"
			Me.radGalleryItem13.AccessibleName = "Hexagon"
			Me.radGalleryItem13.DescriptionText = ""
			Me.radGalleryItem13.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem13.Image = (CType(resources.GetObject("radGalleryItem13.Image"), Image))
			Me.radGalleryItem13.Name = "radGalleryItem13"
			Me.radGalleryItem13.StretchHorizontally = False
			Me.radGalleryItem13.StretchVertically = False
			Me.radGalleryItem13.Text = "Hexagon"
			Me.radGalleryItem13.ToolTipText = "Hexagon"
			Me.radGalleryItem13.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem14
			' 
			Me.radGalleryItem14.AccessibleDescription = "Right arrow"
			Me.radGalleryItem14.AccessibleName = "Right arrow"
			Me.radGalleryItem14.DescriptionText = ""
			Me.radGalleryItem14.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem14.Image = (CType(resources.GetObject("radGalleryItem14.Image"), Image))
			Me.radGalleryItem14.Name = "radGalleryItem14"
			Me.radGalleryItem14.StretchHorizontally = False
			Me.radGalleryItem14.StretchVertically = False
			Me.radGalleryItem14.Text = "Right arrow"
			Me.radGalleryItem14.ToolTipText = "Right arrow"
			Me.radGalleryItem14.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem6
			' 
			Me.radGalleryItem6.AccessibleDescription = "Left arrow"
			Me.radGalleryItem6.AccessibleName = "Left arrow"
			Me.radGalleryItem6.DescriptionText = ""
			Me.radGalleryItem6.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem6.Image = (CType(resources.GetObject("radGalleryItem6.Image"), Image))
			Me.radGalleryItem6.Name = "radGalleryItem6"
			Me.radGalleryItem6.StretchHorizontally = False
			Me.radGalleryItem6.StretchVertically = False
			Me.radGalleryItem6.Text = "Left arrow"
			Me.radGalleryItem6.ToolTipText = "Left arrow"
			Me.radGalleryItem6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem74
			' 
			Me.radGalleryItem74.AccessibleDescription = "Up arrow"
			Me.radGalleryItem74.AccessibleName = "Up arrow"
			Me.radGalleryItem74.DescriptionText = ""
			Me.radGalleryItem74.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem74.Image = My.Resources.up_arrow
			Me.radGalleryItem74.Name = "radGalleryItem74"
			Me.radGalleryItem74.StretchHorizontally = False
			Me.radGalleryItem74.StretchVertically = False
			Me.radGalleryItem74.Text = "Up arrow"
			Me.radGalleryItem74.ToolTipText = "Up arrow"
			Me.radGalleryItem74.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryItem2
			' 
			Me.radGalleryItem2.AccessibleDescription = "Down arrow"
			Me.radGalleryItem2.AccessibleName = "Down arrow"
			Me.radGalleryItem2.DescriptionText = ""
			Me.radGalleryItem2.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem2.Image = (CType(resources.GetObject("radGalleryItem2.Image"), Image))
			Me.radGalleryItem2.Name = "radGalleryItem2"
			Me.radGalleryItem2.StretchHorizontally = False
			Me.radGalleryItem2.StretchVertically = False
			Me.radGalleryItem2.Text = "Down arrow"
			Me.radGalleryItem2.ToolTipText = "Down arrow"
			Me.radGalleryItem2.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem8
			' 
			Me.radGalleryItem8.AccessibleDescription = "Four-point star"
			Me.radGalleryItem8.AccessibleName = "Four-point star"
			Me.radGalleryItem8.DescriptionText = ""
			Me.radGalleryItem8.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem8.Image = (CType(resources.GetObject("radGalleryItem8.Image"), Image))
			Me.radGalleryItem8.Name = "radGalleryItem8"
			Me.radGalleryItem8.StretchHorizontally = False
			Me.radGalleryItem8.StretchVertically = False
			Me.radGalleryItem8.Text = "Four-point star"
			Me.radGalleryItem8.ToolTipText = "Four-point star"
			Me.radGalleryItem8.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem71
			' 
			Me.radGalleryItem71.AccessibleDescription = "Five-point star"
			Me.radGalleryItem71.AccessibleName = "Five-point star"
			Me.radGalleryItem71.DescriptionText = ""
			Me.radGalleryItem71.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem71.Image = My.Resources.star
			Me.radGalleryItem71.Name = "radGalleryItem71"
			Me.radGalleryItem71.StretchHorizontally = False
			Me.radGalleryItem71.StretchVertically = False
			Me.radGalleryItem71.Text = "Five-point star"
			Me.radGalleryItem71.ToolTipText = "Five-point star"
			Me.radGalleryItem71.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem72
			' 
			Me.radGalleryItem72.AccessibleDescription = "Six-point star"
			Me.radGalleryItem72.AccessibleName = "Six-point star"
			Me.radGalleryItem72.DescriptionText = ""
			Me.radGalleryItem72.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem72.Image = My.Resources.star1
			Me.radGalleryItem72.Name = "radGalleryItem72"
			Me.radGalleryItem72.StretchHorizontally = False
			Me.radGalleryItem72.StretchVertically = False
			Me.radGalleryItem72.Text = "Six-point star"
			Me.radGalleryItem72.ToolTipText = "Six-point star"
			Me.radGalleryItem72.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem9
			' 
			Me.radGalleryItem9.AccessibleDescription = "Round rectangular callout"
			Me.radGalleryItem9.AccessibleName = "Round rectangular callout"
			Me.radGalleryItem9.DescriptionText = ""
			Me.radGalleryItem9.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem9.Image = (CType(resources.GetObject("radGalleryItem9.Image"), Image))
			Me.radGalleryItem9.Name = "radGalleryItem9"
			Me.radGalleryItem9.StretchHorizontally = False
			Me.radGalleryItem9.StretchVertically = False
			Me.radGalleryItem9.Text = "Round rectangular callout"
			Me.radGalleryItem9.ToolTipText = "Round rectangular callout"
			Me.radGalleryItem9.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem10
			' 
			Me.radGalleryItem10.AccessibleDescription = "Oval callout"
			Me.radGalleryItem10.AccessibleName = "Oval callout"
			Me.radGalleryItem10.DescriptionText = ""
			Me.radGalleryItem10.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem10.Image = (CType(resources.GetObject("radGalleryItem10.Image"), Image))
			Me.radGalleryItem10.Name = "radGalleryItem10"
			Me.radGalleryItem10.StretchHorizontally = False
			Me.radGalleryItem10.StretchVertically = False
			Me.radGalleryItem10.Text = "Oval callout"
			Me.radGalleryItem10.ToolTipText = "Oval callout"
			Me.radGalleryItem10.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem11
			' 
			Me.radGalleryItem11.AccessibleDescription = "Cloud callout"
			Me.radGalleryItem11.AccessibleName = "Cloud callout"
			Me.radGalleryItem11.DescriptionText = ""
			Me.radGalleryItem11.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem11.Image = (CType(resources.GetObject("radGalleryItem11.Image"), Image))
			Me.radGalleryItem11.Name = "radGalleryItem11"
			Me.radGalleryItem11.StretchHorizontally = False
			Me.radGalleryItem11.StretchVertically = False
			Me.radGalleryItem11.Text = "Cloud callout"
			Me.radGalleryItem11.ToolTipText = "Cloud callout"
			Me.radGalleryItem11.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem4
			' 
			Me.radGalleryItem4.AccessibleDescription = "Heart"
			Me.radGalleryItem4.AccessibleName = "Heart"
			Me.radGalleryItem4.DescriptionText = ""
			Me.radGalleryItem4.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem4.Image = (CType(resources.GetObject("radGalleryItem4.Image"), Image))
			Me.radGalleryItem4.Name = "radGalleryItem4"
			Me.radGalleryItem4.StretchHorizontally = False
			Me.radGalleryItem4.StretchVertically = False
			Me.radGalleryItem4.Text = "Heart"
			Me.radGalleryItem4.ToolTipText = "Heart"
			Me.radGalleryItem4.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radGalleryItem12
			' 
			Me.radGalleryItem12.AccessibleDescription = "Lightning"
			Me.radGalleryItem12.AccessibleName = "Lightning"
			Me.radGalleryItem12.DescriptionText = ""
			Me.radGalleryItem12.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radGalleryItem12.Image = (CType(resources.GetObject("radGalleryItem12.Image"), Image))
			Me.radGalleryItem12.Name = "radGalleryItem12"
			Me.radGalleryItem12.StretchHorizontally = False
			Me.radGalleryItem12.StretchVertically = False
			Me.radGalleryItem12.Text = "Lightning"
			Me.radGalleryItem12.ToolTipText = "Lightning"
			Me.radGalleryItem12.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radMenuSeparatorItem1
			' 
			Me.radMenuSeparatorItem1.AccessibleDescription = "radMenuSeparatorItem1"
			Me.radMenuSeparatorItem1.AccessibleName = "radMenuSeparatorItem1"
			Me.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1"
			Me.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1"
			Me.radMenuSeparatorItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuButtonItem1
			' 
			Me.radMenuButtonItem1.AccessibleDescription = "Draw smooth"
			Me.radMenuButtonItem1.AccessibleName = "Draw smooth"
			' 
			' 
			' 
			Me.radMenuButtonItem1.ButtonElement.AccessibleDescription = "Draw smooth"
			Me.radMenuButtonItem1.ButtonElement.AccessibleName = "Draw smooth"
			Me.radMenuButtonItem1.Name = "radMenuButtonItem1"
			Me.radMenuButtonItem1.Text = "Draw smooth"
			Me.radMenuButtonItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup10
			' 
			Me.radRibbonBarButtonGroup10.AccessibleDescription = "radRibbonBarButtonGroup10"
			Me.radRibbonBarButtonGroup10.AccessibleName = "radRibbonBarButtonGroup10"
			Me.radRibbonBarButtonGroup10.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement5, Me.radDropDownButtonElement9})
			Me.radRibbonBarButtonGroup10.Margin = New Padding(4, 4, 0, 4)
			Me.radRibbonBarButtonGroup10.Name = "radRibbonBarButtonGroup10"
			Me.radRibbonBarButtonGroup10.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup10.Padding = New Padding(1)
			Me.radRibbonBarButtonGroup10.ShowBackColor = False
			Me.radRibbonBarButtonGroup10.ShowBorder = False
			Me.radRibbonBarButtonGroup10.Text = "radRibbonBarButtonGroup10"
			Me.radRibbonBarButtonGroup10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement5
			' 
			Me.radDropDownButtonElement5.AccessibleDescription = "Outline"
			Me.radDropDownButtonElement5.AccessibleName = "Outline"
			Me.radDropDownButtonElement5.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement5.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement5.ExpandArrowButton = False
			Me.radDropDownButtonElement5.Image = My.Resources.outline
			Me.radDropDownButtonElement5.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem12, Me.radMenuItem13, Me.radMenuItem14, Me.radMenuItem24, Me.radMenuItem25, Me.radMenuItem26, Me.radMenuItem27})
			Me.radDropDownButtonElement5.Margin = New Padding(2)
			Me.radDropDownButtonElement5.Name = "radDropDownButtonElement5"
			Me.radDropDownButtonElement5.Text = "Outline"
			Me.radDropDownButtonElement5.TextAlignment = ContentAlignment.MiddleLeft
			Me.radDropDownButtonElement5.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radDropDownButtonElement5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem12
			' 
			Me.radMenuItem12.AccessibleDescription = "No outline"
			Me.radMenuItem12.AccessibleName = "No outline"
			Me.radMenuItem12.Image = My.Resources.no_outline
			Me.radMenuItem12.Name = "radMenuItem12"
			Me.radMenuItem12.Text = "No outline"
			Me.radMenuItem12.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem13
			' 
			Me.radMenuItem13.AccessibleDescription = "Solid color"
			Me.radMenuItem13.AccessibleName = "Solid color"
			Me.radMenuItem13.Image = My.Resources.solid_color
			Me.radMenuItem13.Name = "radMenuItem13"
			Me.radMenuItem13.Text = "Solid color"
			Me.radMenuItem13.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem14
			' 
			Me.radMenuItem14.AccessibleDescription = "Crayon"
			Me.radMenuItem14.AccessibleName = "Crayon"
			Me.radMenuItem14.Image = My.Resources.crayon
			Me.radMenuItem14.Name = "radMenuItem14"
			Me.radMenuItem14.Text = "Crayon"
			Me.radMenuItem14.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem24
			' 
			Me.radMenuItem24.AccessibleDescription = "Marker"
			Me.radMenuItem24.AccessibleName = "Marker"
			Me.radMenuItem24.Image = My.Resources.marker
			Me.radMenuItem24.Name = "radMenuItem24"
			Me.radMenuItem24.Text = "Marker"
			Me.radMenuItem24.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem25
			' 
			Me.radMenuItem25.AccessibleDescription = "Oil"
			Me.radMenuItem25.AccessibleName = "Oil"
			Me.radMenuItem25.Image = My.Resources.oil
			Me.radMenuItem25.Name = "radMenuItem25"
			Me.radMenuItem25.Text = "Oil"
			Me.radMenuItem25.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem26
			' 
			Me.radMenuItem26.AccessibleDescription = "Natural pencil"
			Me.radMenuItem26.AccessibleName = "Natural pencil"
			Me.radMenuItem26.Image = My.Resources.natural_pencil
			Me.radMenuItem26.Name = "radMenuItem26"
			Me.radMenuItem26.Text = "Natural pencil"
			Me.radMenuItem26.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem27
			' 
			Me.radMenuItem27.AccessibleDescription = "Watercolor"
			Me.radMenuItem27.AccessibleName = "Watercolor"
			Me.radMenuItem27.Image = My.Resources.watercolor
			Me.radMenuItem27.Name = "radMenuItem27"
			Me.radMenuItem27.Text = "Watercolor"
			Me.radMenuItem27.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement9
			' 
			Me.radDropDownButtonElement9.AccessibleDescription = "Fill"
			Me.radDropDownButtonElement9.AccessibleName = "Fill"
			Me.radDropDownButtonElement9.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement9.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement9.ExpandArrowButton = False
			Me.radDropDownButtonElement9.Image = My.Resources.fill
			Me.radDropDownButtonElement9.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem28, Me.radMenuItem29, Me.radMenuItem30, Me.radMenuItem31, Me.radMenuItem32, Me.radMenuItem33, Me.radMenuItem34})
			Me.radDropDownButtonElement9.Margin = New Padding(2)
			Me.radDropDownButtonElement9.Name = "radDropDownButtonElement9"
			Me.radDropDownButtonElement9.Text = "Fill"
			Me.radDropDownButtonElement9.TextAlignment = ContentAlignment.MiddleLeft
			Me.radDropDownButtonElement9.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radDropDownButtonElement9.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem28
			' 
			Me.radMenuItem28.AccessibleDescription = "No outline"
			Me.radMenuItem28.AccessibleName = "No outline"
			Me.radMenuItem28.Image = My.Resources.no_outline
			Me.radMenuItem28.Name = "radMenuItem28"
			Me.radMenuItem28.Text = "No outline"
			Me.radMenuItem28.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem29
			' 
			Me.radMenuItem29.AccessibleDescription = "Solid color"
			Me.radMenuItem29.AccessibleName = "Solid color"
			Me.radMenuItem29.Image = My.Resources.solid_color
			Me.radMenuItem29.Name = "radMenuItem29"
			Me.radMenuItem29.Text = "Solid color"
			Me.radMenuItem29.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem30
			' 
			Me.radMenuItem30.AccessibleDescription = "Crayon"
			Me.radMenuItem30.AccessibleName = "Crayon"
			Me.radMenuItem30.Image = My.Resources.crayon
			Me.radMenuItem30.Name = "radMenuItem30"
			Me.radMenuItem30.Text = "Crayon"
			Me.radMenuItem30.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem31
			' 
			Me.radMenuItem31.AccessibleDescription = "Marker"
			Me.radMenuItem31.AccessibleName = "Marker"
			Me.radMenuItem31.Image = My.Resources.marker
			Me.radMenuItem31.Name = "radMenuItem31"
			Me.radMenuItem31.Text = "Marker"
			Me.radMenuItem31.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem32
			' 
			Me.radMenuItem32.AccessibleDescription = "Oil"
			Me.radMenuItem32.AccessibleName = "Oil"
			Me.radMenuItem32.Image = My.Resources.oil
			Me.radMenuItem32.Name = "radMenuItem32"
			Me.radMenuItem32.Text = "Oil"
			Me.radMenuItem32.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem33
			' 
			Me.radMenuItem33.AccessibleDescription = "Natural pencil"
			Me.radMenuItem33.AccessibleName = "Natural pencil"
			Me.radMenuItem33.Image = My.Resources.natural_pencil
			Me.radMenuItem33.Name = "radMenuItem33"
			Me.radMenuItem33.Text = "Natural pencil"
			Me.radMenuItem33.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem34
			' 
			Me.radMenuItem34.AccessibleDescription = "Watercolor"
			Me.radMenuItem34.AccessibleName = "Watercolor"
			Me.radMenuItem34.Image = My.Resources.watercolor
			Me.radMenuItem34.Name = "radMenuItem34"
			Me.radMenuItem34.Text = "Watercolor"
			Me.radMenuItem34.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup4
			' 
			Me.radRibbonBarGroup4.AccessibleDescription = "Size"
			Me.radRibbonBarGroup4.AccessibleName = "Size"
			Me.radRibbonBarGroup4.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement8})
			Me.radRibbonBarGroup4.Name = "radRibbonBarGroup4"
			Me.radRibbonBarGroup4.Text = "Size"
			Me.radRibbonBarGroup4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement8
			' 
			Me.radDropDownButtonElement8.AccessibleDescription = "Size"
			Me.radDropDownButtonElement8.AccessibleName = "Size"
			Me.radDropDownButtonElement8.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement8.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement8.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement8.ExpandArrowButton = False
			Me.radDropDownButtonElement8.Image = My.Resources.size
			Me.radDropDownButtonElement8.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement8.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem35, Me.radMenuItem45, Me.radMenuItem46, Me.radMenuItem47})
			Me.radDropDownButtonElement8.Margin = New Padding(0, 5, 0, 0)
			Me.radDropDownButtonElement8.Name = "radDropDownButtonElement8"
			Me.radDropDownButtonElement8.Text = "Size"
			Me.radDropDownButtonElement8.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radDropDownButtonElement8.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem35
			' 
			Me.radMenuItem35.AccessibleDescription = "1px"
			Me.radMenuItem35.AccessibleName = "1px"
			Me.radMenuItem35.Image = My.Resources.size_1px1
			Me.radMenuItem35.Name = "radMenuItem35"
			Me.radMenuItem35.Text = "1px"
			Me.radMenuItem35.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem45
			' 
			Me.radMenuItem45.AccessibleDescription = "3px"
			Me.radMenuItem45.AccessibleName = "3px"
			Me.radMenuItem45.Image = My.Resources.size_3px1
			Me.radMenuItem45.Name = "radMenuItem45"
			Me.radMenuItem45.Text = "3px"
			Me.radMenuItem45.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem46
			' 
			Me.radMenuItem46.AccessibleDescription = "5px"
			Me.radMenuItem46.AccessibleName = "5px"
			Me.radMenuItem46.Image = My.Resources.size_5px
			Me.radMenuItem46.Name = "radMenuItem46"
			Me.radMenuItem46.Text = "5px"
			Me.radMenuItem46.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem47
			' 
			Me.radMenuItem47.AccessibleDescription = "8px"
			Me.radMenuItem47.AccessibleName = "8px"
			Me.radMenuItem47.Image = My.Resources.size_8px
			Me.radMenuItem47.Name = "radMenuItem47"
			Me.radMenuItem47.Text = "8px"
			Me.radMenuItem47.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup5
			' 
			Me.radRibbonBarGroup5.AccessibleDescription = "Colors"
			Me.radRibbonBarGroup5.AccessibleName = "Colors"
			Me.radRibbonBarGroup5.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement25, Me.radButtonElement26, Me.radRibbonBarButtonGroup14, Me.radButtonElement49})
			Me.radRibbonBarGroup5.Name = "radRibbonBarGroup5"
			Me.radRibbonBarGroup5.Text = "Colors"
			Me.radRibbonBarGroup5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement25
			' 
			Me.radButtonElement25.AccessibleDescription = "<html> Color <br> 1"
			Me.radButtonElement25.AccessibleName = "<html> Color <br> 1"
			Me.radButtonElement25.Class = "RibbonBarButtonElement"
			Me.radButtonElement25.Image = My.Resources.black_rect
			Me.radButtonElement25.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement25.Name = "radButtonElement25"
			Me.radButtonElement25.Text = "<html> Color <br> 1"
			Me.radButtonElement25.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement25.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement26
			' 
			Me.radButtonElement26.AccessibleDescription = "<html> Color <br> 2"
			Me.radButtonElement26.AccessibleName = "<html> Color <br> 2"
			Me.radButtonElement26.Class = "RibbonBarButtonElement"
			Me.radButtonElement26.Image = My.Resources.white_rect
			Me.radButtonElement26.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement26.Name = "radButtonElement26"
			Me.radButtonElement26.Text = "<html> Color <br> 2"
			Me.radButtonElement26.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement26.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup14
			' 
			Me.radRibbonBarButtonGroup14.AccessibleDescription = "radRibbonBarButtonGroup14"
			Me.radRibbonBarButtonGroup14.AccessibleName = "radRibbonBarButtonGroup14"
			Me.radRibbonBarButtonGroup14.Name = "radRibbonBarButtonGroup14"
			Me.radRibbonBarButtonGroup14.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup14.ShowBackColor = False
			Me.radRibbonBarButtonGroup14.Text = "radRibbonBarButtonGroup14"
			Me.radRibbonBarButtonGroup14.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement49
			' 
			Me.radButtonElement49.AccessibleDescription = "<html> Edit <br> Colors"
			Me.radButtonElement49.AccessibleName = "<html> Edit <br> Colors"
			Me.radButtonElement49.Class = "RibbonBarButtonElement"
			Me.radButtonElement49.Image = My.Resources.edit_color
			Me.radButtonElement49.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement49.Name = "radButtonElement49"
			Me.radButtonElement49.Text = "<html> Edit <br> Colors"
			Me.radButtonElement49.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement49.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radOffice2007ScreenTip2
			' 
			Me.radOffice2007ScreenTip2.CaptionVisible = True
			Me.radOffice2007ScreenTip2.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip2.FooterVisible = False
			Me.radOffice2007ScreenTip2.Location = New Point(0, 0)
			Me.radOffice2007ScreenTip2.Name = "radOffice2007ScreenTip2"
			' 
			' 
			' 
			Me.radOffice2007ScreenTip2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			' 
			' 
			' 
			Me.radOffice2007ScreenTip2.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip2.ScreenTipElement.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			Me.radOffice2007ScreenTip2.ScreenTipElement.TipSize = New Size(210, 50)
			Me.radOffice2007ScreenTip2.Size = New Size(336, 43)
			Me.radOffice2007ScreenTip2.TabIndex = 0
			Me.radOffice2007ScreenTip2.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			CType(Me.radOffice2007ScreenTip2.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Cut"
			CType(Me.radOffice2007ScreenTip2.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadLabelElement).Text = "Cut the selection from the document and put it on the clipboard"
			' 
			' radOffice2007ScreenTip3
			' 
			Me.radOffice2007ScreenTip3.CaptionVisible = True
			Me.radOffice2007ScreenTip3.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip3.FooterVisible = False
			Me.radOffice2007ScreenTip3.Location = New Point(0, 0)
			Me.radOffice2007ScreenTip3.Name = "radOffice2007ScreenTip3"
			' 
			' 
			' 
			Me.radOffice2007ScreenTip3.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			' 
			' 
			' 
			Me.radOffice2007ScreenTip3.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip3.ScreenTipElement.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			Me.radOffice2007ScreenTip3.ScreenTipElement.TipSize = New Size(210, 50)
			Me.radOffice2007ScreenTip3.Size = New Size(247, 43)
			Me.radOffice2007ScreenTip3.TabIndex = 0
			Me.radOffice2007ScreenTip3.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			CType(Me.radOffice2007ScreenTip3.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Copy"
			CType(Me.radOffice2007ScreenTip3.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadLabelElement).Text = "Copy the selection and put it on the clipboard"
			' 
			' radOffice2007ScreenTip4
			' 
			Me.radOffice2007ScreenTip4.CaptionVisible = True
			Me.radOffice2007ScreenTip4.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip4.FooterVisible = False
			Me.radOffice2007ScreenTip4.Location = New Point(0, 0)
			Me.radOffice2007ScreenTip4.Name = "radOffice2007ScreenTip4"
			' 
			' 
			' 
			Me.radOffice2007ScreenTip4.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			' 
			' 
			' 
			Me.radOffice2007ScreenTip4.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip4.ScreenTipElement.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			Me.radOffice2007ScreenTip4.ScreenTipElement.TipSize = New Size(210, 50)
			Me.radOffice2007ScreenTip4.Size = New Size(293, 43)
			Me.radOffice2007ScreenTip4.TabIndex = 0
			Me.radOffice2007ScreenTip4.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			CType(Me.radOffice2007ScreenTip4.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Format Painter"
			CType(Me.radOffice2007ScreenTip4.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadLabelElement).Text = "Copy formatting from one place and apply it to another"
			' 
			' radOffice2007ScreenTip1
			' 
			Me.radOffice2007ScreenTip1.CaptionVisible = True
			Me.radOffice2007ScreenTip1.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip1.FooterVisible = False
			Me.radOffice2007ScreenTip1.Location = New Point(0, 0)
			Me.radOffice2007ScreenTip1.Name = "radOffice2007ScreenTip1"
			' 
			' 
			' 
			Me.radOffice2007ScreenTip1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			' 
			' 
			' 
			Me.radOffice2007ScreenTip1.ScreenTipElement.Description = "Override this property and provide custom screentip template description in Desig" & "nTime."
			Me.radOffice2007ScreenTip1.ScreenTipElement.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			Me.radOffice2007ScreenTip1.ScreenTipElement.TipSize = New Size(210, 50)
			Me.radOffice2007ScreenTip1.Size = New Size(195, 43)
			Me.radOffice2007ScreenTip1.TabIndex = 0
			Me.radOffice2007ScreenTip1.TemplateType = GetType(Telerik.WinControls.UI.RadOffice2007ScreenTipElement)
			CType(Me.radOffice2007ScreenTip1.GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "Paste"
			CType(Me.radOffice2007ScreenTip1.GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadLabelElement).Text = "Paste the contents of the clipboard"
			' 
			' radRibbonBarCommandTab2
			' 
			Me.radRibbonBarCommandTab2.Alignment = ContentAlignment.BottomLeft
			Me.radRibbonBarCommandTab2.CanFocus = True
			Me.radRibbonBarCommandTab2.ContextualTabGroup = Me.contextualTabGroup1
            Me.radRibbonBarCommandTab2.IsSelected = False
			Me.radRibbonBarCommandTab2.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarGroup1, Me.radRibbonBarGroup6, Me.radRibbonBarGroup7, Me.radRibbonBarGroup14, Me.radRibbonBarGroup15})
			Me.radRibbonBarCommandTab2.Name = "radRibbonBarCommandTab2"
			Me.radRibbonBarCommandTab2.StretchHorizontally = False
			Me.radRibbonBarCommandTab2.Text = "View"
			' 
			' radRibbonBarGroup1
			' 
			Me.radRibbonBarGroup1.AccessibleDescription = "Zoom"
			Me.radRibbonBarGroup1.AccessibleName = "Zoom"
			Me.radRibbonBarGroup1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement5, Me.radButtonElement29, Me.radButtonElement30})
			Me.radRibbonBarGroup1.Name = "radRibbonBarGroup1"
			Me.radRibbonBarGroup1.Text = "Zoom"
			Me.radRibbonBarGroup1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement5
			' 
			Me.radButtonElement5.AccessibleDescription = "<html>Zoom <br>in"
			Me.radButtonElement5.AccessibleName = "<html>Zoom <br>in"
			Me.radButtonElement5.Class = "RibbonBarButtonElement"
			Me.radButtonElement5.Image = My.Resources.zoom_plus
			Me.radButtonElement5.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement5.Margin = New Padding(0, 5, 0, 0)
			Me.radButtonElement5.Name = "radButtonElement5"
			Me.radButtonElement5.Text = "<html>Zoom <br>in"
			Me.radButtonElement5.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement29
			' 
			Me.radButtonElement29.AccessibleDescription = "<html>Zoom <br>out"
			Me.radButtonElement29.AccessibleName = "<html>Zoom <br>out"
			Me.radButtonElement29.Class = "RibbonBarButtonElement"
			Me.radButtonElement29.Image = My.Resources.zoom_negative
			Me.radButtonElement29.Margin = New Padding(0, 5, 0, 0)
			Me.radButtonElement29.Name = "radButtonElement29"
			Me.radButtonElement29.Text = "<html>Zoom <br>out"
			Me.radButtonElement29.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement29.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement30
			' 
			Me.radButtonElement30.AccessibleDescription = "<html>100 <br> %"
			Me.radButtonElement30.AccessibleName = "<html>100 <br> %"
			Me.radButtonElement30.Class = "RibbonBarButtonElement"
			Me.radButtonElement30.Image = My.Resources.fill_zoom
			Me.radButtonElement30.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement30.Margin = New Padding(0, 5, 0, 0)
			Me.radButtonElement30.Name = "radButtonElement30"
			Me.radButtonElement30.Text = "<html>100 <br> %"
			Me.radButtonElement30.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement30.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup6
			' 
			Me.radRibbonBarGroup6.AccessibleDescription = "Show or hide"
			Me.radRibbonBarGroup6.AccessibleName = "Show or hide"
			Me.radRibbonBarGroup6.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radCheckBoxElement1, Me.radCheckBoxElement2, Me.radCheckBoxElement3})
			Me.radRibbonBarGroup6.Name = "radRibbonBarGroup6"
			Me.radRibbonBarGroup6.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup6.Text = "Show or hide"
			Me.radRibbonBarGroup6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radCheckBoxElement1
			' 
			Me.radCheckBoxElement1.AccessibleDescription = "Rules"
			Me.radCheckBoxElement1.AccessibleName = "Rules"
			Me.radCheckBoxElement1.Checked = False
			Me.radCheckBoxElement1.Margin = New Padding(0, 5, 0, 0)
			Me.radCheckBoxElement1.Name = "radCheckBoxElement1"
			Me.radCheckBoxElement1.StretchVertically = False
			Me.radCheckBoxElement1.Text = "Rules"
			Me.radCheckBoxElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radCheckBoxElement2
			' 
			Me.radCheckBoxElement2.AccessibleDescription = "Gridline"
			Me.radCheckBoxElement2.AccessibleName = "Gridline"
			Me.radCheckBoxElement2.Checked = False
			Me.radCheckBoxElement2.Margin = New Padding(0, 5, 0, 0)
			Me.radCheckBoxElement2.Name = "radCheckBoxElement2"
			Me.radCheckBoxElement2.StretchVertically = False
			Me.radCheckBoxElement2.Text = "Gridline"
			Me.radCheckBoxElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radCheckBoxElement3
			' 
			Me.radCheckBoxElement3.AccessibleDescription = "Status bar"
			Me.radCheckBoxElement3.AccessibleName = "Status bar"
			Me.radCheckBoxElement3.Checked = False
			Me.radCheckBoxElement3.Margin = New Padding(0, 5, 0, 0)
			Me.radCheckBoxElement3.Name = "radCheckBoxElement3"
			Me.radCheckBoxElement3.StretchVertically = False
			Me.radCheckBoxElement3.Text = "Status bar"
			Me.radCheckBoxElement3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup7
			' 
			Me.radRibbonBarGroup7.AccessibleDescription = "Display"
			Me.radRibbonBarGroup7.AccessibleName = "Display"
			Me.radRibbonBarGroup7.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement31, Me.radButtonElement32})
			Me.radRibbonBarGroup7.Name = "radRibbonBarGroup7"
			Me.radRibbonBarGroup7.Text = "Display"
			Me.radRibbonBarGroup7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement31
			' 
			Me.radButtonElement31.AccessibleDescription = "<html>Full <br>screen"
			Me.radButtonElement31.AccessibleName = "<html>Full <br>screen"
			Me.radButtonElement31.Class = "RibbonBarButtonElement"
			Me.radButtonElement31.Image = My.Resources.fill_screen
			Me.radButtonElement31.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement31.Margin = New Padding(0, 5, 0, 0)
			Me.radButtonElement31.Name = "radButtonElement31"
			Me.radButtonElement31.Text = "<html>Full <br>screen"
			Me.radButtonElement31.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement31.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement32
			' 
			Me.radButtonElement32.AccessibleDescription = "Thumbnail"
			Me.radButtonElement32.AccessibleName = "Thumbnail"
			Me.radButtonElement32.Class = "RibbonBarButtonElement"
			Me.radButtonElement32.Image = My.Resources.thumbnail_active
			Me.radButtonElement32.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement32.Margin = New Padding(0, 5, 0, 0)
			Me.radButtonElement32.Name = "radButtonElement32"
			Me.radButtonElement32.Text = "Thumbnail"
			Me.radButtonElement32.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement32.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup14
			' 
			Me.radRibbonBarGroup14.AccessibleDescription = "Themes"
			Me.radRibbonBarGroup14.AccessibleName = "Themes"
			Me.radRibbonBarGroup14.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement46, Me.radButtonElement50, Me.radButtonElement51})
			Me.radRibbonBarGroup14.Name = "radRibbonBarGroup14"
			Me.radRibbonBarGroup14.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup14.Text = "Office 2007 Themes"
			Me.radRibbonBarGroup14.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement46
			' 
			Me.radButtonElement46.AccessibleDescription = "Office 2007 Black"
			Me.radButtonElement46.AccessibleName = "Office 2007 Black"
			Me.radButtonElement46.Class = "RibbonBarButtonElement"
			Me.radButtonElement46.Image = My.Resources.Black
			Me.radButtonElement46.Name = "radButtonElement46"
			Me.radButtonElement46.Padding = New Padding(2)
			Me.radButtonElement46.Text = "Office 2007 Black"
			Me.radButtonElement46.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement46.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement46.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radButtonElement50
			' 
			Me.radButtonElement50.AccessibleDescription = "Office 2007 Blue"
			Me.radButtonElement50.AccessibleName = "Office 2007 Blue"
			Me.radButtonElement50.Class = "RibbonBarButtonElement"
			Me.radButtonElement50.Image = My.Resources.Blue
			Me.radButtonElement50.Name = "radButtonElement50"
			Me.radButtonElement50.Padding = New Padding(2)
			Me.radButtonElement50.Text = "Office 2007 Blue"
			Me.radButtonElement50.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement50.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement50.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radButtonElement51
			' 
			Me.radButtonElement51.AccessibleDescription = "Office 2007 Silver"
			Me.radButtonElement51.AccessibleName = "Office 2007 Silver"
			Me.radButtonElement51.Class = "RibbonBarButtonElement"
			Me.radButtonElement51.Image = My.Resources.Silver
			Me.radButtonElement51.Name = "radButtonElement51"
			Me.radButtonElement51.Padding = New Padding(2)
			Me.radButtonElement51.Text = "Office 2007 Silver"
			Me.radButtonElement51.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement51.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement51.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radGalleryGroupFilter4
			' 
			Me.radGalleryGroupFilter4.AccessibleDescription = "Greek Letters"
			Me.radGalleryGroupFilter4.AccessibleName = "Greek Letters"
			Me.radGalleryGroupFilter4.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radGalleryGroupItem3, Me.radGalleryGroupItem4})
			Me.radGalleryGroupFilter4.Name = "radGalleryGroupFilter4"
			Me.radGalleryGroupFilter4.Text = "Greek Letters"
			Me.radGalleryGroupFilter4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryGroupItem3
			' 
			Me.radGalleryGroupItem3.AccessibleDescription = "Lowercase"
			Me.radGalleryGroupItem3.AccessibleName = "Lowercase"
			Me.radGalleryGroupItem3.Name = "radGalleryGroupItem3"
			Me.radGalleryGroupItem3.Text = "Lowercase"
			Me.radGalleryGroupItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radGalleryGroupItem4
			' 
			Me.radGalleryGroupItem4.AccessibleDescription = "Uppercase"
			Me.radGalleryGroupItem4.AccessibleName = "Uppercase"
			Me.radGalleryGroupItem4.Name = "radGalleryGroupItem4"
			Me.radGalleryGroupItem4.Text = "Uppercase"
			Me.radGalleryGroupItem4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarCommandTab3
			' 
			Me.radRibbonBarCommandTab3.Alignment = ContentAlignment.BottomLeft
			Me.radRibbonBarCommandTab3.CanFocus = True
			Me.radRibbonBarCommandTab3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarGroup9, Me.radRibbonBarGroup11, Me.radRibbonBarGroup12, Me.radRibbonBarGroup13})
			Me.radRibbonBarCommandTab3.Name = "radRibbonBarCommandTab3"
			Me.radRibbonBarCommandTab3.StretchHorizontally = False
			Me.radRibbonBarCommandTab3.Text = "Text"
			' 
			' radRibbonBarGroup9
			' 
			Me.radRibbonBarGroup9.AccessibleDescription = "Clipboard"
			Me.radRibbonBarGroup9.AccessibleName = "Clipboard"
			Me.radRibbonBarGroup9.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement12, Me.radRibbonBarButtonGroup3})
			Me.radRibbonBarGroup9.Name = "radRibbonBarGroup9"
			Me.radRibbonBarGroup9.Text = "Clipboard"
			Me.radRibbonBarGroup9.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement12
			' 
			Me.radDropDownButtonElement12.AccessibleDescription = "Paste"
			Me.radDropDownButtonElement12.AccessibleName = "Paste"
			Me.radDropDownButtonElement12.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement12.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement12.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement12.ExpandArrowButton = False
			Me.radDropDownButtonElement12.Image = My.Resources.new_paste_32
			Me.radDropDownButtonElement12.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement12.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem37, Me.radMenuItem48})
			Me.radDropDownButtonElement12.Margin = New Padding(2)
			Me.radDropDownButtonElement12.Name = "radDropDownButtonElement12"
			Me.radDropDownButtonElement12.Text = "Paste"
			Me.radDropDownButtonElement12.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement12.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radDropDownButtonElement12.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem37
			' 
			Me.radMenuItem37.AccessibleDescription = "Paste"
			Me.radMenuItem37.AccessibleName = "Paste"
			Me.radMenuItem37.Image = My.Resources.new_paste_16
			Me.radMenuItem37.Name = "radMenuItem37"
			Me.radMenuItem37.Text = "Paste"
			Me.radMenuItem37.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem48
			' 
			Me.radMenuItem48.AccessibleDescription = "Paste from"
			Me.radMenuItem48.AccessibleName = "Paste from"
			Me.radMenuItem48.Image = My.Resources.paste_form
			Me.radMenuItem48.Name = "radMenuItem48"
			Me.radMenuItem48.Text = "Paste from"
			Me.radMenuItem48.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup3
			' 
			Me.radRibbonBarButtonGroup3.AccessibleDescription = "radRibbonBarButtonGroup3"
			Me.radRibbonBarButtonGroup3.AccessibleName = "radRibbonBarButtonGroup3"
			Me.radRibbonBarButtonGroup3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement52, Me.radButtonElement53, Me.radButtonElement40})
			Me.radRibbonBarButtonGroup3.Name = "radRibbonBarButtonGroup3"
			Me.radRibbonBarButtonGroup3.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup3.Text = "radRibbonBarButtonGroup3"
			Me.radRibbonBarButtonGroup3.Margin = New Padding(2, 2, 2, 0)
			Me.radRibbonBarButtonGroup3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement52
			' 
			Me.radButtonElement52.AccessibleDescription = "Cut"
			Me.radButtonElement52.AccessibleName = "Cut"
			Me.radButtonElement52.Class = "ButtonElement"
			Me.radButtonElement52.Image = (CType(resources.GetObject("radButtonElement52.Image"), Image))
			Me.radButtonElement52.ImageIndex = 6
			Me.radButtonElement52.ImageKey = ""
			Me.radButtonElement52.Name = "radButtonElement52"
			Me.radButtonElement52.ShowBorder = False
			Me.radButtonElement52.Text = "Cut"
			Me.radButtonElement52.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement52.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement53
			' 
			Me.radButtonElement53.AccessibleDescription = "Copy"
			Me.radButtonElement53.AccessibleName = "Copy"
			Me.radButtonElement53.Class = "ButtonElement"
			Me.radButtonElement53.Image = (CType(resources.GetObject("radButtonElement53.Image"), Image))
			Me.radButtonElement53.ImageIndex = 4
			Me.radButtonElement53.Name = "radButtonElement53"
			Me.radButtonElement53.ShowBorder = False
			Me.radButtonElement53.Text = "Copy"
			Me.radButtonElement53.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement53.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement40
			' 
			Me.radButtonElement40.AccessibleDescription = "Paste"
			Me.radButtonElement40.AccessibleName = "Paste"
			Me.radButtonElement40.Class = "ButtonElement"
			Me.radButtonElement40.Image = (CType(resources.GetObject("radButtonElement40.Image"), Image))
			Me.radButtonElement40.ImageIndex = 25
			Me.radButtonElement40.Name = "radButtonElement40"
			Me.radButtonElement40.ShowBorder = False
			Me.radButtonElement40.Text = "Paste"
			Me.radButtonElement40.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement40.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup11
			' 
			Me.radRibbonBarGroup11.AccessibleDescription = "Font"
			Me.radRibbonBarGroup11.AccessibleName = "Font"
			Me.radRibbonBarGroup11.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup25, Me.radRibbonBarButtonGroup26})
			Me.radRibbonBarGroup11.Name = "radRibbonBarGroup11"
			Me.radRibbonBarGroup11.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup11.Text = "Font"
			Me.radRibbonBarGroup11.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup25
			' 
			Me.radRibbonBarButtonGroup25.AccessibleDescription = "radRibbonBarButtonGroup25"
			Me.radRibbonBarButtonGroup25.AccessibleName = "radRibbonBarButtonGroup25"
			Me.radRibbonBarButtonGroup25.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownListElement1, Me.radDropDownListElement2})
			Me.radRibbonBarButtonGroup25.Name = "radRibbonBarButtonGroup25"
			Me.radRibbonBarButtonGroup25.Padding = New Padding(2)
			Me.radRibbonBarButtonGroup25.ShowBackColor = False
			Me.radRibbonBarButtonGroup25.ShowBorder = False
			Me.radRibbonBarButtonGroup25.Text = "radRibbonBarButtonGroup25"
			Me.radRibbonBarButtonGroup25.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownListElement1
			' 
			Me.radDropDownListElement1.AccessibleDescription = "Calibri (Body)"
			Me.radDropDownListElement1.AccessibleName = "Calibri (Body)"
			Me.radDropDownListElement1.AutoCompleteAppend = Nothing
			Me.radDropDownListElement1.AutoCompleteDataSource = Nothing
			Me.radDropDownListElement1.AutoCompleteDisplayMember = Nothing
			Me.radDropDownListElement1.AutoCompleteSuggest = Nothing
			Me.radDropDownListElement1.AutoCompleteValueMember = Nothing
			Me.radDropDownListElement1.DataSource = Nothing
			Me.radDropDownListElement1.DefaultItemsCountInDropDown = 6
			Me.radDropDownListElement1.DefaultValue = Nothing
			Me.radDropDownListElement1.DisplayMember = ""
			Me.radDropDownListElement1.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad
			Me.radDropDownListElement1.EditableElementText = "Calibri (Body)"
			Me.radDropDownListElement1.EditorElement = Me.radDropDownListElement1
			Me.radDropDownListElement1.EditorManager = Nothing
			Me.radDropDownListElement1.Filter = Nothing
			Me.radDropDownListElement1.FilterExpression = ""
			Me.radDropDownListElement1.Focusable = True
			Me.radDropDownListElement1.FormatInfo = New System.Globalization.CultureInfo("en-US")
			Me.radDropDownListElement1.FormatString = "{0}"
			Me.radDropDownListElement1.FormattingEnabled = True
			Me.radDropDownListElement1.ItemHeight = 18
			radListDataItem7.Text = "Calibri (Body)"
			radListDataItem7.TextWrap = True
			radListDataItem8.Text = "Arial"
			radListDataItem8.TextWrap = True
			radListDataItem9.Text = "Tahoma"
			radListDataItem9.TextWrap = True
			radListDataItem10.Text = "Verdana"
			radListDataItem10.TextWrap = True
			radListDataItem11.Text = "Times New Roman"
			radListDataItem11.TextWrap = True
			radListDataItem12.Text = "Cambria"
			radListDataItem12.TextWrap = True
			Me.radDropDownListElement1.Items.Add(radListDataItem7)
			Me.radDropDownListElement1.Items.Add(radListDataItem8)
			Me.radDropDownListElement1.Items.Add(radListDataItem9)
			Me.radDropDownListElement1.Items.Add(radListDataItem10)
			Me.radDropDownListElement1.Items.Add(radListDataItem11)
			Me.radDropDownListElement1.Items.Add(radListDataItem12)
			Me.radDropDownListElement1.MaxLength = 32767
			Me.radDropDownListElement1.MaxValue = Nothing
			Me.radDropDownListElement1.MinSize = New Size(140, 0)
			Me.radDropDownListElement1.MinValue = Nothing
			Me.radDropDownListElement1.Name = "radDropDownListElement1"
			Me.radDropDownListElement1.NullValue = Nothing
			Me.radDropDownListElement1.OwnerOffset = 0
			Me.radDropDownListElement1.ShowImageInEditorArea = True
			Me.radDropDownListElement1.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None
			Me.radDropDownListElement1.StretchVertically = False
			Me.radDropDownListElement1.Value = Nothing
			Me.radDropDownListElement1.ValueMember = ""
            Me.radDropDownListElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.radDropDownListElement1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			' 
			' radDropDownListElement2
			' 
			Me.radDropDownListElement2.AccessibleDescription = "8"
			Me.radDropDownListElement2.AccessibleName = "8"
			Me.radDropDownListElement2.AutoCompleteAppend = Nothing
			Me.radDropDownListElement2.AutoCompleteDataSource = Nothing
			Me.radDropDownListElement2.AutoCompleteDisplayMember = Nothing
			Me.radDropDownListElement2.AutoCompleteSuggest = Nothing
			Me.radDropDownListElement2.AutoCompleteValueMember = Nothing
			Me.radDropDownListElement2.DataSource = Nothing
			Me.radDropDownListElement2.DefaultItemsCountInDropDown = 6
			Me.radDropDownListElement2.DefaultValue = Nothing
			Me.radDropDownListElement2.DisplayMember = ""
			Me.radDropDownListElement2.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad
			Me.radDropDownListElement2.EditableElementText = "8"
			Me.radDropDownListElement2.EditorElement = Me.radDropDownListElement2
			Me.radDropDownListElement2.EditorManager = Nothing
			Me.radDropDownListElement2.Filter = Nothing
			Me.radDropDownListElement2.FilterExpression = ""
			Me.radDropDownListElement2.Focusable = True
			Me.radDropDownListElement2.FormatInfo = New System.Globalization.CultureInfo("en-US")
			Me.radDropDownListElement2.FormatString = "{0}"
			Me.radDropDownListElement2.FormattingEnabled = True
			Me.radDropDownListElement2.ItemHeight = 18
			radListDataItem13.Text = "8"
			radListDataItem13.TextWrap = True
			radListDataItem14.Text = "9"
			radListDataItem14.TextWrap = True
			radListDataItem15.Text = "10"
			radListDataItem15.TextWrap = True
			radListDataItem16.Text = "11"
			radListDataItem16.TextWrap = True
			radListDataItem17.Text = "12"
			radListDataItem17.TextWrap = True
			radListDataItem18.Text = "14"
			radListDataItem18.TextWrap = True
			radListDataItem21.Text = "16"
			radListDataItem21.TextWrap = True
			radListDataItem22.Text = "18"
			radListDataItem22.TextWrap = True
			Me.radDropDownListElement2.Items.Add(radListDataItem13)
			Me.radDropDownListElement2.Items.Add(radListDataItem14)
			Me.radDropDownListElement2.Items.Add(radListDataItem15)
			Me.radDropDownListElement2.Items.Add(radListDataItem16)
			Me.radDropDownListElement2.Items.Add(radListDataItem17)
			Me.radDropDownListElement2.Items.Add(radListDataItem18)
			Me.radDropDownListElement2.Items.Add(radListDataItem21)
			Me.radDropDownListElement2.Items.Add(radListDataItem22)
			Me.radDropDownListElement2.MaxLength = 32767
            Me.radDropDownListElement2.MaxSize = New Size(45, 0)
			Me.radDropDownListElement2.MaxValue = Nothing
			Me.radDropDownListElement2.MinValue = Nothing
			Me.radDropDownListElement2.Name = "radDropDownListElement2"
			Me.radDropDownListElement2.NullValue = Nothing
			Me.radDropDownListElement2.OwnerOffset = 0
			Me.radDropDownListElement2.ShowImageInEditorArea = True
			Me.radDropDownListElement2.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None
			Me.radDropDownListElement2.StretchVertically = False
			Me.radDropDownListElement2.Value = Nothing
			Me.radDropDownListElement2.ValueMember = ""
            Me.radDropDownListElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Me.radDropDownListElement2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			' 
			' radRibbonBarButtonGroup26
			' 
			Me.radRibbonBarButtonGroup26.AccessibleDescription = "radRibbonBarButtonGroup26"
			Me.radRibbonBarButtonGroup26.AccessibleName = "radRibbonBarButtonGroup26"
			Me.radRibbonBarButtonGroup26.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup30, Me.radRibbonBarButtonGroup31})
			Me.radRibbonBarButtonGroup26.Name = "radRibbonBarButtonGroup26"
			Me.radRibbonBarButtonGroup26.Padding = New Padding(1)
			Me.radRibbonBarButtonGroup26.ShowBackColor = False
			Me.radRibbonBarButtonGroup26.ShowBorder = False
			Me.radRibbonBarButtonGroup26.Text = "radRibbonBarButtonGroup26"
			Me.radRibbonBarButtonGroup26.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup30
			' 
			Me.radRibbonBarButtonGroup30.AccessibleDescription = "radRibbonBarButtonGroup30"
			Me.radRibbonBarButtonGroup30.AccessibleName = "radRibbonBarButtonGroup30"
			Me.radRibbonBarButtonGroup30.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement75, Me.radButtonElement86, Me.radButtonElement87, Me.radButtonElement88, Me.radButtonElement89, Me.radButtonElement90})
			Me.radRibbonBarButtonGroup30.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup30.Name = "radRibbonBarButtonGroup30"
			Me.radRibbonBarButtonGroup30.Text = "radRibbonBarButtonGroup30"
			Me.radRibbonBarButtonGroup30.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement75
			' 
			Me.radButtonElement75.AccessibleDescription = "radButtonElement75"
			Me.radButtonElement75.AccessibleName = "radButtonElement75"
			Me.radButtonElement75.Class = "ButtonElement"
			Me.radButtonElement75.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement75.Image = (CType(resources.GetObject("radButtonElement75.Image"), Image))
			Me.radButtonElement75.ImageIndex = 1
			Me.radButtonElement75.Name = "radButtonElement75"
			Me.radButtonElement75.Text = "radButtonElement75"
			Me.radButtonElement75.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement86
			' 
			Me.radButtonElement86.AccessibleDescription = "radButtonElement86"
			Me.radButtonElement86.AccessibleName = "radButtonElement86"
			Me.radButtonElement86.Class = "ButtonElement"
			Me.radButtonElement86.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement86.Image = (CType(resources.GetObject("radButtonElement86.Image"), Image))
			Me.radButtonElement86.ImageIndex = 16
			Me.radButtonElement86.Name = "radButtonElement86"
			Me.radButtonElement86.Text = "radButtonElement86"
			Me.radButtonElement86.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement87
			' 
			Me.radButtonElement87.AccessibleDescription = "radButtonElement87"
			Me.radButtonElement87.AccessibleName = "radButtonElement87"
			Me.radButtonElement87.Class = "ButtonElement"
			Me.radButtonElement87.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement87.Image = (CType(resources.GetObject("radButtonElement87.Image"), Image))
			Me.radButtonElement87.ImageIndex = 26
			Me.radButtonElement87.Name = "radButtonElement87"
			Me.radButtonElement87.Text = "radButtonElement87"
			Me.radButtonElement87.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement88
			' 
			Me.radButtonElement88.AccessibleDescription = "radButtonElement88"
			Me.radButtonElement88.AccessibleName = "radButtonElement88"
			Me.radButtonElement88.Class = "ButtonElement"
			Me.radButtonElement88.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement88.Image = (CType(resources.GetObject("radButtonElement88.Image"), Image))
			Me.radButtonElement88.ImageIndex = 22
			Me.radButtonElement88.Name = "radButtonElement88"
			Me.radButtonElement88.Text = "radButtonElement88"
			Me.radButtonElement88.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement89
			' 
			Me.radButtonElement89.AccessibleDescription = "radButtonElement89"
			Me.radButtonElement89.AccessibleName = "radButtonElement89"
			Me.radButtonElement89.Class = "ButtonElement"
			Me.radButtonElement89.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement89.Image = (CType(resources.GetObject("radButtonElement89.Image"), Image))
			Me.radButtonElement89.ImageIndex = 23
			Me.radButtonElement89.Name = "radButtonElement89"
			Me.radButtonElement89.Text = "radButtonElement89"
			Me.radButtonElement89.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement90
			' 
			Me.radButtonElement90.AccessibleDescription = "radButtonElement90"
			Me.radButtonElement90.AccessibleName = "radButtonElement90"
			Me.radButtonElement90.Class = "ButtonElement"
			Me.radButtonElement90.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement90.Image = (CType(resources.GetObject("radButtonElement90.Image"), Image))
			Me.radButtonElement90.ImageIndex = 24
			Me.radButtonElement90.Name = "radButtonElement90"
			Me.radButtonElement90.ShowBorder = False
			Me.radButtonElement90.Text = "radButtonElement90"
			Me.radButtonElement90.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup31
			' 
			Me.radRibbonBarButtonGroup31.AccessibleDescription = "radRibbonBarButtonGroup31"
			Me.radRibbonBarButtonGroup31.AccessibleName = "radRibbonBarButtonGroup31"
			Me.radRibbonBarButtonGroup31.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement91, Me.radButtonElement92})
			Me.radRibbonBarButtonGroup31.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup31.Name = "radRibbonBarButtonGroup31"
			Me.radRibbonBarButtonGroup31.Text = "radRibbonBarButtonGroup31"
			Me.radRibbonBarButtonGroup31.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement91
			' 
			Me.radButtonElement91.AccessibleDescription = "radButtonElement91"
			Me.radButtonElement91.AccessibleName = "radButtonElement91"
			Me.radButtonElement91.Class = "ButtonElement"
			Me.radButtonElement91.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement91.Image = (CType(resources.GetObject("radButtonElement91.Image"), Image))
			Me.radButtonElement91.ImageIndex = 43
			Me.radButtonElement91.Name = "radButtonElement91"
			Me.radButtonElement91.Text = "radButtonElement91"
			Me.radButtonElement91.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement92
			' 
			Me.radButtonElement92.AccessibleDescription = "radButtonElement92"
			Me.radButtonElement92.AccessibleName = "radButtonElement92"
			Me.radButtonElement92.Class = "ButtonElement"
			Me.radButtonElement92.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement92.Image = (CType(resources.GetObject("radButtonElement92.Image"), Image))
			Me.radButtonElement92.ImageIndex = 42
			Me.radButtonElement92.Name = "radButtonElement92"
			Me.radButtonElement92.ShowBorder = False
			Me.radButtonElement92.Text = "radButtonElement92"
			Me.radButtonElement92.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup12
			' 
			Me.radRibbonBarGroup12.AccessibleDescription = "Background"
			Me.radRibbonBarGroup12.AccessibleName = "Background"
			Me.radRibbonBarGroup12.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement76, Me.radButtonElement77})
			Me.radRibbonBarGroup12.Name = "radRibbonBarGroup12"
			Me.radRibbonBarGroup12.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup12.Text = "Background"
			Me.radRibbonBarGroup12.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement76
			' 
			Me.radButtonElement76.AccessibleDescription = "Opaque"
			Me.radButtonElement76.AccessibleName = "Opaque"
			Me.radButtonElement76.Class = "RibbonBarButtonElement"
			Me.radButtonElement76.Image = My.Resources.Opaque
			Me.radButtonElement76.Name = "radButtonElement76"
			Me.radButtonElement76.Padding = New Padding(2)
			Me.radButtonElement76.Text = "Opaque"
			Me.radButtonElement76.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement76.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement76.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement77
			' 
			Me.radButtonElement77.AccessibleDescription = "Transperant"
			Me.radButtonElement77.AccessibleName = "Transperant"
			Me.radButtonElement77.Class = "RibbonBarButtonElement"
			Me.radButtonElement77.Image = My.Resources.transparent
			Me.radButtonElement77.Name = "radButtonElement77"
			Me.radButtonElement77.Padding = New Padding(2)
			Me.radButtonElement77.Text = "Transperant"
			Me.radButtonElement77.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement77.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement77.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup13
			' 
			Me.radRibbonBarGroup13.AccessibleDescription = "Colors"
			Me.radRibbonBarGroup13.AccessibleName = "Colors"
			Me.radRibbonBarGroup13.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement27, Me.radButtonElement44, Me.radRibbonBarButtonGroup15, Me.radButtonElement45})
			Me.radRibbonBarGroup13.Name = "radRibbonBarGroup13"
			Me.radRibbonBarGroup13.Text = "Colors"
			Me.radRibbonBarGroup13.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement27
			' 
			Me.radButtonElement27.AccessibleDescription = "<html>Color <br>1"
			Me.radButtonElement27.AccessibleName = "<html>Color <br>1"
			Me.radButtonElement27.Class = "RibbonBarButtonElement"
			Me.radButtonElement27.Image = My.Resources.black_rect
			Me.radButtonElement27.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement27.Name = "radButtonElement27"
			Me.radButtonElement27.Text = "<html>Color <br>1"
			Me.radButtonElement27.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement27.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement44
			' 
			Me.radButtonElement44.AccessibleDescription = "<html>Color <br>2"
			Me.radButtonElement44.AccessibleName = "<html>Color <br>2"
			Me.radButtonElement44.Class = "RibbonBarButtonElement"
			Me.radButtonElement44.Image = My.Resources.white_rect
			Me.radButtonElement44.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement44.Name = "radButtonElement44"
			Me.radButtonElement44.Text = "<html>Color <br>2"
			Me.radButtonElement44.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement44.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup15
			' 
			Me.radRibbonBarButtonGroup15.AccessibleDescription = "radRibbonBarButtonGroup15"
			Me.radRibbonBarButtonGroup15.AccessibleName = "radRibbonBarButtonGroup15"
			Me.radRibbonBarButtonGroup15.Name = "radRibbonBarButtonGroup15"
			Me.radRibbonBarButtonGroup15.Text = "radRibbonBarButtonGroup15"
			Me.radRibbonBarButtonGroup15.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement45
			' 
			Me.radButtonElement45.AccessibleDescription = "<html>Edit <br>colors"
			Me.radButtonElement45.AccessibleName = "<html>Edit <br>colors"
			Me.radButtonElement45.Class = "RibbonBarButtonElement"
			Me.radButtonElement45.Image = My.Resources.edit_color
			Me.radButtonElement45.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButtonElement45.Name = "radButtonElement45"
			Me.radButtonElement45.Text = "<html>Edit <br>colors"
			Me.radButtonElement45.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radButtonElement45.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' contextualTabGroup1
			' 
			Me.contextualTabGroup1.AccessibleDescription = "Themes"
			Me.contextualTabGroup1.AccessibleName = "Themes"
			Me.contextualTabGroup1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize
			Me.contextualTabGroup1.BaseColor = Color.FromArgb((CInt(Fix((CByte(0))))), (CInt(Fix((CByte(165))))), (CInt(Fix((CByte(226))))), (CInt(Fix((CByte(142))))))
			Me.contextualTabGroup1.ForeColor = Color.FromArgb((CInt(Fix((CByte(21))))), (CInt(Fix((CByte(66))))), (CInt(Fix((CByte(139))))))
			Me.contextualTabGroup1.Name = "contextualTabGroup1"
			Me.contextualTabGroup1.TabItems.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarCommandTab2})
			Me.contextualTabGroup1.Text = "Themes"
			Me.contextualTabGroup1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBar1
			' 
			Me.radRibbonBar1.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView
			Me.radRibbonBar1.AutoSize = True
			Me.radRibbonBar1.BackColor = Color.FromArgb((CInt(Fix((CByte(191))))), (CInt(Fix((CByte(219))))), (CInt(Fix((CByte(254))))))
			Me.radRibbonBar1.BackstageControl = Me.radRibbonBarBackstageView1
			Me.radRibbonBar1.CommandTabs.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarCommandTab1, Me.radRibbonBarCommandTab3, Me.radRibbonBarCommandTab2})
			Me.radRibbonBar1.ContextualTabGroups.AddRange(New Telerik.WinControls.RadItem() { Me.contextualTabGroup1})
			Me.radRibbonBar1.Dock = DockStyle.Top
			Me.radRibbonBar1.EnableKeyMap = True
			' 
			' 
			' 
			Me.radRibbonBar1.ExitButton.AccessibleDescription = "Exit"
			Me.radRibbonBar1.ExitButton.AccessibleName = "Exit"
			Me.radRibbonBar1.ExitButton.Alignment = ContentAlignment.TopRight
			' 
			' 
			' 
			Me.radRibbonBar1.ExitButton.ButtonElement.AccessibleDescription = "Exit"
			Me.radRibbonBar1.ExitButton.ButtonElement.AccessibleName = "Exit"
			Me.radRibbonBar1.ExitButton.ButtonElement.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radRibbonBar1.ExitButton.CanFocus = True
			Me.radRibbonBar1.ExitButton.Image = My.Resources.exitButton
			Me.radRibbonBar1.ExitButton.Text = "Exit"
			Me.radRibbonBar1.ExitButton.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radRibbonBar1.ExitButton.ZIndex = 2
			Me.radRibbonBar1.ImageList = Me.imageList1
			Me.radRibbonBar1.Location = New Point(0, 0)
			Me.radRibbonBar1.Name = "radRibbonBar1"
			' 
			' 
			' 
			Me.radRibbonBar1.OptionsButton.AccessibleDescription = "Options"
			Me.radRibbonBar1.OptionsButton.AccessibleName = "Options"
			Me.radRibbonBar1.OptionsButton.Alignment = ContentAlignment.TopRight
			' 
			' 
			' 
			Me.radRibbonBar1.OptionsButton.ButtonElement.AccessibleDescription = "Options"
			Me.radRibbonBar1.OptionsButton.ButtonElement.AccessibleName = "Options"
			Me.radRibbonBar1.OptionsButton.ButtonElement.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radRibbonBar1.OptionsButton.CanFocus = True
			Me.radRibbonBar1.OptionsButton.Image = My.Resources.optionsButton
			Me.radRibbonBar1.OptionsButton.Text = "Options"
			Me.radRibbonBar1.OptionsButton.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radRibbonBar1.OptionsButton.ZIndex = 2
			Me.radRibbonBar1.QuickAccessToolBarItems.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement41, Me.radButtonElement42, Me.radButtonElement47, Me.radButtonElement48})
			' 
			' 
			' 
			Me.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radRibbonBar1.Size = New Size(1304, 161)
			Me.radRibbonBar1.StartButtonImage = (CType(resources.GetObject("radRibbonBar1.StartButtonImage"), Image))
			Me.radRibbonBar1.StartMenuItems.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem59, Me.radMenuItem2, Me.radMenuItem51, Me.radMenuItem3, Me.radMenuSeparatorItem2, Me.radMenuItem52, Me.radMenuItem53, Me.radMenuItem54, Me.radMenuSeparatorItem6, Me.radMenuItem55, Me.radMenuSeparatorItem3, Me.radMenuItem56, Me.radMenuSeparatorItem4, Me.radMenuItem57, Me.radMenuSeparatorItem5, Me.radMenuItem58})
			Me.radRibbonBar1.StartMenuRightColumnItems.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem6, Me.radMenuItem7, Me.radMenuItem8, Me.radMenuItem1})
			Me.radRibbonBar1.TabIndex = 1
			Me.radRibbonBar1.Text = "RadRibbonBar"
			' 
			' radRibbonBarBackstageView1
			' 
			Me.radRibbonBarBackstageView1.Controls.Add(Me.backstageViewPage1)
			Me.radRibbonBarBackstageView1.Controls.Add(Me.backstageViewPage2)
			Me.radRibbonBarBackstageView1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.backstageButtonItem1, Me.backstageButtonItem2, Me.backstageButtonItem3, Me.backstageButtonItem4, Me.backstageTabItem1, Me.backstageTabItem2, Me.backstageButtonItem5, Me.backstageButtonItem6, Me.backstageButtonItem7})
			Me.radRibbonBarBackstageView1.Location = New Point(0, 53)
			Me.radRibbonBarBackstageView1.Name = "radRibbonBarBackstageView1"
			Me.radRibbonBarBackstageView1.RightToLeft = RightToLeft.No
			Me.radRibbonBarBackstageView1.SelectedItem = Me.backstageTabItem1
			Me.radRibbonBarBackstageView1.Size = New Size(1304, 547)
			Me.radRibbonBarBackstageView1.TabIndex = 4
			' 
			' backstageViewPage1
			' 
			Me.backstageViewPage1.BackColor = Color.Transparent
			Me.backstageViewPage1.Controls.Add(Me.pictureBox2)
			Me.backstageViewPage1.Controls.Add(Me.radSeparator2)
			Me.backstageViewPage1.Controls.Add(Me.radButton3)
			Me.backstageViewPage1.Controls.Add(Me.radButton2)
			Me.backstageViewPage1.Controls.Add(Me.radButton1)
			Me.backstageViewPage1.Controls.Add(Me.radSeparator1)
			Me.backstageViewPage1.Controls.Add(Me.radLabel1)
			Me.backstageViewPage1.Location = New Point(191, 1)
			Me.backstageViewPage1.Name = "backstageViewPage1"
			Me.backstageViewPage1.Size = New Size(1113, 528)
			Me.backstageViewPage1.TabIndex = 0
			' 
			' pictureBox2
			' 
			Me.pictureBox2.Anchor = (CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.pictureBox2.Image = My.Resources.content
			Me.pictureBox2.Location = New Point(392, 16)
			Me.pictureBox2.Name = "pictureBox2"
			Me.pictureBox2.Size = New Size(701, 493)
			Me.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom
			Me.pictureBox2.TabIndex = 6
			Me.pictureBox2.TabStop = False
			' 
			' radSeparator2
			' 
			Me.radSeparator2.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left), AnchorStyles))
			Me.radSeparator2.Location = New Point(363, 0)
			Me.radSeparator2.Name = "radSeparator2"
			Me.radSeparator2.Orientation = Orientation.Vertical
			Me.radSeparator2.ShadowOffset = New Point(0, 0)
			Me.radSeparator2.Size = New Size(23, 528)
			Me.radSeparator2.TabIndex = 5
			Me.radSeparator2.Text = "radSeparator2"
			' 
			' radButton3
            ' 
            Me.radButton3.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
			Me.radButton3.Image = My.Resources.ribbon_print_preview
            Me.radButton3.Location = New Point(30, 178)
			Me.radButton3.Name = "radButton3"
            Me.radButton3.Size = New Size(306, 40)
			Me.radButton3.TabIndex = 4
			Me.radButton3.Text = "<html><b> Print Preview</b><br><span style=""color:#5e5e5e""> Preview and make chan" & "ges.</span>"
			Me.radButton3.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButton3.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radButton2
            ' 
            Me.radButton2.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
			Me.radButton2.Image = My.Resources.ribbon_pagesetup
            Me.radButton2.Location = New Point(30, 121)
			Me.radButton2.Name = "radButton2"
            Me.radButton2.Size = New Size(306, 40)
			Me.radButton2.TabIndex = 3
			Me.radButton2.Text = "<html><b> Print Setup</b><br><span style=""color:#5e5e5e""> Change the layout of th" & "e picture.</span>"
			Me.radButton2.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButton2.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radButton1
            ' 
            Me.radButton1.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
			Me.radButton1.Image = My.Resources.ribbon_print
            Me.radButton1.Location = New Point(30, 64)
			Me.radButton1.Name = "radButton1"
            Me.radButton1.Size = New Size(306, 40)
			Me.radButton1.TabIndex = 2
			Me.radButton1.Text = "<html><b> Print</b><br><span style=""color:#5e5e5e""> Select a printer, number of c" & "opies before printing.</span>"
			Me.radButton1.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButton1.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radSeparator1
			' 
			Me.radSeparator1.Location = New Point(32, 38)
			Me.radSeparator1.Name = "radSeparator1"
			Me.radSeparator1.ShadowOffset = New Point(0, 0)
			Me.radSeparator1.Size = New Size(302, 10)
			Me.radSeparator1.TabIndex = 1
			Me.radSeparator1.Text = "radSeparator1"
			' 
			' radLabel1
			' 
			Me.radLabel1.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
			Me.radLabel1.ForeColor = Color.FromArgb((CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))))
			Me.radLabel1.Location = New Point(32, 16)
			Me.radLabel1.Name = "radLabel1"
			' 
			' 
			' 
			Me.radLabel1.RootElement.ForeColor = Color.FromArgb((CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))))
			Me.radLabel1.Size = New Size(46, 25)
			Me.radLabel1.TabIndex = 0
			Me.radLabel1.Text = "Print"
			' 
			' backstageViewPage2
			' 
			Me.backstageViewPage2.BackColor = Color.Transparent
			Me.backstageViewPage2.Controls.Add(Me.radButton6)
			Me.backstageViewPage2.Controls.Add(Me.radButton5)
			Me.backstageViewPage2.Controls.Add(Me.radButton4)
			Me.backstageViewPage2.Controls.Add(Me.radSeparator3)
			Me.backstageViewPage2.Controls.Add(Me.radLabel2)
			Me.backstageViewPage2.Location = New Point(168, 1)
			Me.backstageViewPage2.Name = "backstageViewPage2"
			Me.backstageViewPage2.Size = New Size(1136, 546)
			Me.backstageViewPage2.TabIndex = 1
			' 
			' radButton6
			' 
			Me.radButton6.Image = My.Resources.ribbon_center_png
			Me.radButton6.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButton6.Location = New Point(205, 55)
			Me.radButton6.Name = "radButton6"
			Me.radButton6.Size = New Size(64, 64)
			Me.radButton6.TabIndex = 6
			Me.radButton6.Text = "Center"
			Me.radButton6.TextAlignment = ContentAlignment.TopCenter
			Me.radButton6.TextImageRelation = TextImageRelation.ImageAboveText
			' 
			' radButton5
			' 
			Me.radButton5.Image = My.Resources.ribbon_tile
			Me.radButton5.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButton5.Location = New Point(119, 55)
			Me.radButton5.Name = "radButton5"
			Me.radButton5.Size = New Size(64, 64)
			Me.radButton5.TabIndex = 5
			Me.radButton5.Text = "Tile"
			Me.radButton5.TextAlignment = ContentAlignment.TopCenter
			Me.radButton5.TextImageRelation = TextImageRelation.ImageAboveText
			' 
			' radButton4
			' 
			Me.radButton4.Image = My.Resources.ribbon_fill
			Me.radButton4.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButton4.Location = New Point(32, 55)
			Me.radButton4.Name = "radButton4"
			Me.radButton4.Size = New Size(64, 64)
			Me.radButton4.TabIndex = 4
			Me.radButton4.Text = "Fill"
			Me.radButton4.TextAlignment = ContentAlignment.TopCenter
			Me.radButton4.TextImageRelation = TextImageRelation.ImageAboveText
			' 
			' radSeparator3
			' 
			Me.radSeparator3.Location = New Point(32, 38)
			Me.radSeparator3.Name = "radSeparator3"
			Me.radSeparator3.ShadowOffset = New Point(0, 0)
			Me.radSeparator3.Size = New Size(259, 10)
			Me.radSeparator3.TabIndex = 3
			Me.radSeparator3.Text = "radSeparator3"
			' 
			' radLabel2
			' 
			Me.radLabel2.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
			Me.radLabel2.ForeColor = Color.FromArgb((CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))))
			Me.radLabel2.Location = New Point(32, 16)
			Me.radLabel2.Name = "radLabel2"
			' 
			' 
			' 
			Me.radLabel2.RootElement.ForeColor = Color.FromArgb((CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))), (CInt(Fix((CByte(94))))))
			Me.radLabel2.Size = New Size(220, 25)
			Me.radLabel2.TabIndex = 2
			Me.radLabel2.Text = "Set as Desktop Background"
			' 
			' backstageButtonItem1
			' 
			Me.backstageButtonItem1.AccessibleDescription = "New"
			Me.backstageButtonItem1.AccessibleName = "New"
			Me.backstageButtonItem1.Image = My.Resources._new
			Me.backstageButtonItem1.Name = "backstageButtonItem1"
			Me.backstageButtonItem1.Text = "New"
			Me.backstageButtonItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem2
			' 
			Me.backstageButtonItem2.AccessibleDescription = "Open"
			Me.backstageButtonItem2.AccessibleName = "Open"
			Me.backstageButtonItem2.Image = My.Resources.open
			Me.backstageButtonItem2.Name = "backstageButtonItem2"
			Me.backstageButtonItem2.Text = "Open"
			Me.backstageButtonItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem3
			' 
			Me.backstageButtonItem3.AccessibleDescription = "Save"
			Me.backstageButtonItem3.AccessibleName = "Save"
			Me.backstageButtonItem3.Image = My.Resources.save
			Me.backstageButtonItem3.Name = "backstageButtonItem3"
			Me.backstageButtonItem3.Text = "Save"
			Me.backstageButtonItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem4
			' 
			Me.backstageButtonItem4.AccessibleDescription = "Save As"
			Me.backstageButtonItem4.AccessibleName = "Save As"
			Me.backstageButtonItem4.Image = My.Resources.save_as1
			Me.backstageButtonItem4.Name = "backstageButtonItem4"
			Me.backstageButtonItem4.Text = "Save As"
			Me.backstageButtonItem4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageTabItem1
			' 
			Me.backstageTabItem1.AccessibleDescription = "Print"
			Me.backstageTabItem1.AccessibleName = "Print"
			Me.backstageTabItem1.Image = My.Resources.ribbon_print
			Me.backstageTabItem1.ImageAlignment = ContentAlignment.MiddleLeft
			Me.backstageTabItem1.Name = "backstageTabItem1"
			Me.backstageTabItem1.Page = Me.backstageViewPage1
			Me.backstageTabItem1.Text = "Print"
			Me.backstageTabItem1.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.backstageTabItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageTabItem2
			' 
			Me.backstageTabItem2.AccessibleDescription = "Set as Background"
			Me.backstageTabItem2.AccessibleName = "Set as Background"
			Me.backstageTabItem2.Image = My.Resources.set_as_desktop
			Me.backstageTabItem2.ImageAlignment = ContentAlignment.MiddleLeft
			Me.backstageTabItem2.Name = "backstageTabItem2"
			Me.backstageTabItem2.Page = Me.backstageViewPage2
			Me.backstageTabItem2.Text = "Set as Background"
			Me.backstageTabItem2.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.backstageTabItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem5
			' 
			Me.backstageButtonItem5.AccessibleDescription = "Properties"
			Me.backstageButtonItem5.AccessibleName = "Properties"
			Me.backstageButtonItem5.Image = My.Resources.optionsButton
			Me.backstageButtonItem5.Name = "backstageButtonItem5"
			Me.backstageButtonItem5.Text = "Properties"
			Me.backstageButtonItem5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem6
			' 
			Me.backstageButtonItem6.AccessibleDescription = "About"
			Me.backstageButtonItem6.AccessibleName = "About"
			Me.backstageButtonItem6.Image = My.Resources.info2
			Me.backstageButtonItem6.Name = "backstageButtonItem6"
			Me.backstageButtonItem6.Text = "About"
			Me.backstageButtonItem6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' backstageButtonItem7
			' 
			Me.backstageButtonItem7.AccessibleDescription = "Exit"
			Me.backstageButtonItem7.AccessibleName = "Exit"
			Me.backstageButtonItem7.Image = My.Resources.exit2
			Me.backstageButtonItem7.Name = "backstageButtonItem7"
			Me.backstageButtonItem7.Text = "Exit"
			Me.backstageButtonItem7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Transparent
			Me.imageList1.Images.SetKeyName(0, "")
			Me.imageList1.Images.SetKeyName(1, "")
			Me.imageList1.Images.SetKeyName(2, "")
			Me.imageList1.Images.SetKeyName(3, "")
			Me.imageList1.Images.SetKeyName(4, "")
			Me.imageList1.Images.SetKeyName(5, "")
			Me.imageList1.Images.SetKeyName(6, "")
			Me.imageList1.Images.SetKeyName(7, "")
			Me.imageList1.Images.SetKeyName(8, "")
			Me.imageList1.Images.SetKeyName(9, "")
			Me.imageList1.Images.SetKeyName(10, "")
			Me.imageList1.Images.SetKeyName(11, "")
			Me.imageList1.Images.SetKeyName(12, "")
			Me.imageList1.Images.SetKeyName(13, "")
			Me.imageList1.Images.SetKeyName(14, "")
			Me.imageList1.Images.SetKeyName(15, "")
			Me.imageList1.Images.SetKeyName(16, "")
			Me.imageList1.Images.SetKeyName(17, "")
			Me.imageList1.Images.SetKeyName(18, "")
			Me.imageList1.Images.SetKeyName(19, "")
			Me.imageList1.Images.SetKeyName(20, "")
			Me.imageList1.Images.SetKeyName(21, "")
			Me.imageList1.Images.SetKeyName(22, "")
			Me.imageList1.Images.SetKeyName(23, "")
			Me.imageList1.Images.SetKeyName(24, "")
			Me.imageList1.Images.SetKeyName(25, "")
			Me.imageList1.Images.SetKeyName(26, "")
			Me.imageList1.Images.SetKeyName(27, "")
			Me.imageList1.Images.SetKeyName(28, "")
			Me.imageList1.Images.SetKeyName(29, "")
			Me.imageList1.Images.SetKeyName(30, "")
			Me.imageList1.Images.SetKeyName(31, "")
			Me.imageList1.Images.SetKeyName(32, "")
			Me.imageList1.Images.SetKeyName(33, "pageLayout.gif")
			Me.imageList1.Images.SetKeyName(34, "insert5.gif")
			Me.imageList1.Images.SetKeyName(35, "insert6.gif")
			Me.imageList1.Images.SetKeyName(36, "insert.gif")
			Me.imageList1.Images.SetKeyName(37, "pageLayout1.gif")
			Me.imageList1.Images.SetKeyName(38, "pageLayout2.gif")
			Me.imageList1.Images.SetKeyName(39, "pageLayout3.gif")
			Me.imageList1.Images.SetKeyName(40, "pageLayout4.gif")
			Me.imageList1.Images.SetKeyName(41, "Eraser.gif")
			Me.imageList1.Images.SetKeyName(42, "Letter_down.gif")
			Me.imageList1.Images.SetKeyName(43, "Letter_up.gif")
			Me.imageList1.Images.SetKeyName(44, "small1.GIF")
			Me.imageList1.Images.SetKeyName(45, "small2.GIF")
			Me.imageList1.Images.SetKeyName(46, "small3.GIF")
			Me.imageList1.Images.SetKeyName(47, "small4.GIF")
			Me.imageList1.Images.SetKeyName(48, "small5.GIF")
			Me.imageList1.Images.SetKeyName(49, "small6.gif")
			Me.imageList1.Images.SetKeyName(50, "small7.gif")
			Me.imageList1.Images.SetKeyName(51, "small8.gif")
			Me.imageList1.Images.SetKeyName(52, "small9.gif")
			Me.imageList1.Images.SetKeyName(53, "small10.gif")
			Me.imageList1.Images.SetKeyName(54, "small11.gif")
			Me.imageList1.Images.SetKeyName(55, "small12.gif")
			Me.imageList1.Images.SetKeyName(56, "small13.gif")
			Me.imageList1.Images.SetKeyName(57, "small14.gif")
			Me.imageList1.Images.SetKeyName(58, "small15.gif")
			Me.imageList1.Images.SetKeyName(59, "small16.gif")
			Me.imageList1.Images.SetKeyName(60, "small17.gif")
			Me.imageList1.Images.SetKeyName(61, "small18.gif")
			Me.imageList1.Images.SetKeyName(62, "small19.gif")
			Me.imageList1.Images.SetKeyName(63, "small20.gif")
			Me.imageList1.Images.SetKeyName(64, "small21.gif")
			Me.imageList1.Images.SetKeyName(65, "small22.gif")
			Me.imageList1.Images.SetKeyName(66, "small23.gif")
			Me.imageList1.Images.SetKeyName(67, "small24.gif")
			Me.imageList1.Images.SetKeyName(68, "small25.gif")
			Me.imageList1.Images.SetKeyName(69, "small26.gif")
			Me.imageList1.Images.SetKeyName(70, "small27.gif")
			Me.imageList1.Images.SetKeyName(71, "small28.gif")
			Me.imageList1.Images.SetKeyName(72, "small29.gif")
			Me.imageList1.Images.SetKeyName(73, "small30.gif")
			Me.imageList1.Images.SetKeyName(74, "big1.gif")
			Me.imageList1.Images.SetKeyName(75, "big2.gif")
			Me.imageList1.Images.SetKeyName(76, "big3.gif")
			Me.imageList1.Images.SetKeyName(77, "big4.gif")
			Me.imageList1.Images.SetKeyName(78, "big5.gif")
			Me.imageList1.Images.SetKeyName(79, "big6.gif")
			Me.imageList1.Images.SetKeyName(80, "big7.gif")
			Me.imageList1.Images.SetKeyName(81, "big8.gif")
			Me.imageList1.Images.SetKeyName(82, "big9.gif")
			Me.imageList1.Images.SetKeyName(83, "big10.gif")
			Me.imageList1.Images.SetKeyName(84, "big11.gif")
			Me.imageList1.Images.SetKeyName(85, "big12.gif")
			Me.imageList1.Images.SetKeyName(86, "big13.gif")
			Me.imageList1.Images.SetKeyName(87, "big14.gif")
			Me.imageList1.Images.SetKeyName(88, "big15.gif")
			Me.imageList1.Images.SetKeyName(89, "big16.gif")
			Me.imageList1.Images.SetKeyName(90, "big17.gif")
			Me.imageList1.Images.SetKeyName(91, "big18.gif")
			Me.imageList1.Images.SetKeyName(92, "big19.gif")
			Me.imageList1.Images.SetKeyName(93, "big20.gif")
			Me.imageList1.Images.SetKeyName(94, "big21.gif")
			Me.imageList1.Images.SetKeyName(95, "big22.gif")
			Me.imageList1.Images.SetKeyName(96, "big23.gif")
			Me.imageList1.Images.SetKeyName(97, "big24.gif")
			' 
			' radButtonElement41
			' 
			Me.radButtonElement41.AccessibleDescription = "New"
			Me.radButtonElement41.AccessibleName = "New"
			Me.radButtonElement41.CanFocus = True
			Me.radButtonElement41.Class = "RibbonBarButtonElement"
			Me.radButtonElement41.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement41.Image = My.Resources.msIcon1
			Me.radButtonElement41.MaxSize = New Size(0, 18)
			Me.radButtonElement41.Name = "radButtonElement41"
			Me.radButtonElement41.Padding = New Padding(2, 1, 2, 2)
			Me.radButtonElement41.Text = "New"
			Me.radButtonElement41.Visibility = Telerik.WinControls.ElementVisibility.Visible
			Me.radButtonElement41.ZIndex = 3
			' 
			' radButtonElement42
			' 
			Me.radButtonElement42.AccessibleDescription = "Save"
			Me.radButtonElement42.AccessibleName = "Save"
			Me.radButtonElement42.CanFocus = True
			Me.radButtonElement42.Class = "RibbonBarButtonElement"
			Me.radButtonElement42.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement42.Image = My.Resources.msIcon3
			Me.radButtonElement42.MaxSize = New Size(0, 18)
			Me.radButtonElement42.Name = "radButtonElement42"
			Me.radButtonElement42.Padding = New Padding(2, 1, 2, 2)
			Me.radButtonElement42.Text = "Save"
			Me.radButtonElement42.Visibility = Telerik.WinControls.ElementVisibility.Visible
			Me.radButtonElement42.ZIndex = 2
			' 
			' radButtonElement47
			' 
			Me.radButtonElement47.AccessibleDescription = "Undo"
			Me.radButtonElement47.AccessibleName = "Undo"
			Me.radButtonElement47.Class = "RibbonBarButtonElement"
			Me.radButtonElement47.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement47.Image = My.Resources.undo
			Me.radButtonElement47.MaxSize = New Size(0, 18)
			Me.radButtonElement47.Name = "radButtonElement47"
			Me.radButtonElement47.Padding = New Padding(2, 1, 2, 2)
			Me.radButtonElement47.Text = "Undo"
			Me.radButtonElement47.Visibility = Telerik.WinControls.ElementVisibility.Visible
			Me.radButtonElement47.ZIndex = 1
			' 
			' radButtonElement48
			' 
			Me.radButtonElement48.AccessibleDescription = "Print"
			Me.radButtonElement48.AccessibleName = "Print"
			Me.radButtonElement48.Class = "RibbonBarButtonElement"
			Me.radButtonElement48.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement48.Image = My.Resources.print1
			Me.radButtonElement48.MaxSize = New Size(0, 18)
			Me.radButtonElement48.Name = "radButtonElement48"
			Me.radButtonElement48.Padding = New Padding(2, 1, 2, 2)
			Me.radButtonElement48.Text = "Print"
			Me.radButtonElement48.Visibility = Telerik.WinControls.ElementVisibility.Visible
			Me.radButtonElement48.ZIndex = 0
			' 
			' radMenuItem59
			' 
			Me.radMenuItem59.AccessibleDescription = "New"
			Me.radMenuItem59.AccessibleName = "New"
			Me.radMenuItem59.Image = My.Resources.RibbonMenuNewMagenta
			Me.radMenuItem59.Name = "radMenuItem59"
			Me.radMenuItem59.Text = "New"
			Me.radMenuItem59.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem2
			' 
			Me.radMenuItem2.AccessibleDescription = "Open"
			Me.radMenuItem2.AccessibleName = "Open"
			Me.radMenuItem2.Image = My.Resources.RibbonMenuOpenMagenta
			Me.radMenuItem2.Name = "radMenuItem2"
			Me.radMenuItem2.Text = "Open"
			Me.radMenuItem2.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem51
			' 
			Me.radMenuItem51.AccessibleDescription = "Save"
			Me.radMenuItem51.AccessibleName = "Save"
			Me.radMenuItem51.Image = My.Resources.RibbonMenuSaveMagenta
			Me.radMenuItem51.Name = "radMenuItem51"
			Me.radMenuItem51.Text = "Save"
			Me.radMenuItem51.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem3
			' 
			Me.radMenuItem3.AccessibleDescription = "Save As"
			Me.radMenuItem3.AccessibleName = "Save As"
			Me.radMenuItem3.Image = My.Resources.RibbonMenuSaveAsMagenta
			Me.radMenuItem3.Name = "radMenuItem3"
			Me.radMenuItem3.Text = "Save As"
			Me.radMenuItem3.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuSeparatorItem2
			' 
			Me.radMenuSeparatorItem2.AccessibleDescription = "radMenuSeparatorItem2"
			Me.radMenuSeparatorItem2.AccessibleName = "radMenuSeparatorItem2"
			Me.radMenuSeparatorItem2.Name = "radMenuSeparatorItem2"
			Me.radMenuSeparatorItem2.Text = "radMenuSeparatorItem2"
			Me.radMenuSeparatorItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem52
			' 
			Me.radMenuItem52.AccessibleDescription = "Print"
			Me.radMenuItem52.AccessibleName = "Print"
			Me.radMenuItem52.Image = My.Resources.ribbon_print
			Me.radMenuItem52.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuHeaderItem3, Me.radMenuItem60, Me.radMenuItem61, Me.radMenuItem62})
			Me.radMenuItem52.Name = "radMenuItem52"
			Me.radMenuItem52.Text = "Print"
			Me.radMenuItem52.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuHeaderItem3
			' 
			Me.radMenuHeaderItem3.AccessibleDescription = "Print"
			Me.radMenuHeaderItem3.AccessibleName = "Print"
			Me.radMenuHeaderItem3.Name = "radMenuHeaderItem3"
			Me.radMenuHeaderItem3.Text = "Print"
			Me.radMenuHeaderItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem60
			' 
			Me.radMenuItem60.AccessibleDescription = "Print"
			Me.radMenuItem60.AccessibleName = "Print"
			Me.radMenuItem60.DescriptionText = "Select a printer, number of copies before printing."
			Me.radMenuItem60.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radMenuItem60.Image = My.Resources.ribbon_print
			Me.radMenuItem60.Name = "radMenuItem60"
			Me.radMenuItem60.Text = "Print"
			Me.radMenuItem60.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem61
			' 
			Me.radMenuItem61.AccessibleDescription = "Print Setup"
			Me.radMenuItem61.AccessibleName = "Print Setup"
			Me.radMenuItem61.DescriptionText = "Change the layout of the picture."
			Me.radMenuItem61.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radMenuItem61.Image = My.Resources.ribbon_pagesetup
			Me.radMenuItem61.Name = "radMenuItem61"
			Me.radMenuItem61.Text = "Print Setup"
			Me.radMenuItem61.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem62
			' 
			Me.radMenuItem62.AccessibleDescription = "Print Preview"
			Me.radMenuItem62.AccessibleName = "Print Preview"
			Me.radMenuItem62.DescriptionText = "Preview and make changes."
			Me.radMenuItem62.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radMenuItem62.Image = My.Resources.ribbon_print_preview
			Me.radMenuItem62.Name = "radMenuItem62"
			Me.radMenuItem62.Text = "Print Preview"
			Me.radMenuItem62.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem53
			' 
			Me.radMenuItem53.AccessibleDescription = "From"
			Me.radMenuItem53.AccessibleName = "From"
			Me.radMenuItem53.Enabled = False
			Me.radMenuItem53.Image = My.Resources.from_scanner_camera
			Me.radMenuItem53.Name = "radMenuItem53"
			Me.radMenuItem53.Text = "From"
			Me.radMenuItem53.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem54
			' 
			Me.radMenuItem54.AccessibleDescription = "Send in"
			Me.radMenuItem54.AccessibleName = "Send in"
			Me.radMenuItem54.Image = My.Resources.send_email
			Me.radMenuItem54.Name = "radMenuItem54"
			Me.radMenuItem54.Text = "Send in"
			Me.radMenuItem54.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuSeparatorItem6
			' 
			Me.radMenuSeparatorItem6.AccessibleDescription = "radMenuSeparatorItem6"
			Me.radMenuSeparatorItem6.AccessibleName = "radMenuSeparatorItem6"
			Me.radMenuSeparatorItem6.Name = "radMenuSeparatorItem6"
			Me.radMenuSeparatorItem6.Text = "radMenuSeparatorItem6"
			Me.radMenuSeparatorItem6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem55
			' 
			Me.radMenuItem55.AccessibleDescription = "Set as desktop background"
			Me.radMenuItem55.AccessibleName = "Set as desktop background"
			Me.radMenuItem55.Image = My.Resources.set_as_desktop
			Me.radMenuItem55.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuHeaderItem4, Me.radMenuItem63, Me.radMenuItem64, Me.radMenuItem65})
			Me.radMenuItem55.Name = "radMenuItem55"
			Me.radMenuItem55.Text = "Set as desktop background"
			Me.radMenuItem55.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuHeaderItem4
			' 
			Me.radMenuHeaderItem4.AccessibleDescription = "Set as a desktop background"
			Me.radMenuHeaderItem4.AccessibleName = "Set as a desktop background"
			Me.radMenuHeaderItem4.Name = "radMenuHeaderItem4"
			Me.radMenuHeaderItem4.Text = "Set as a desktop background"
			Me.radMenuHeaderItem4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem63
			' 
			Me.radMenuItem63.AccessibleDescription = "Fill"
			Me.radMenuItem63.AccessibleName = "Fill"
			Me.radMenuItem63.DescriptionText = "Fill the entire screen with a picture."
			Me.radMenuItem63.Image = My.Resources.ribbon_fill
			Me.radMenuItem63.Name = "radMenuItem63"
			Me.radMenuItem63.Text = "Fill"
			Me.radMenuItem63.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem64
			' 
			Me.radMenuItem64.AccessibleDescription = "Tile"
			Me.radMenuItem64.AccessibleName = "Tile"
			Me.radMenuItem64.DescriptionText = "Tile the picture so it repeates and fill the entire screen."
			Me.radMenuItem64.Image = My.Resources.ribbon_tile
			Me.radMenuItem64.Name = "radMenuItem64"
			Me.radMenuItem64.Text = "Tile"
			Me.radMenuItem64.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem65
			' 
			Me.radMenuItem65.AccessibleDescription = "Center"
			Me.radMenuItem65.AccessibleName = "Center"
			Me.radMenuItem65.DescriptionText = "Center the picture in the middle of the screen."
			Me.radMenuItem65.Image = My.Resources.ribbon_center_png
			Me.radMenuItem65.Name = "radMenuItem65"
			Me.radMenuItem65.Text = "Center"
			Me.radMenuItem65.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuSeparatorItem3
			' 
			Me.radMenuSeparatorItem3.AccessibleDescription = "radMenuSeparatorItem3"
			Me.radMenuSeparatorItem3.AccessibleName = "radMenuSeparatorItem3"
			Me.radMenuSeparatorItem3.Name = "radMenuSeparatorItem3"
			Me.radMenuSeparatorItem3.Text = "radMenuSeparatorItem3"
			Me.radMenuSeparatorItem3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem56
			' 
			Me.radMenuItem56.AccessibleDescription = "Properties"
			Me.radMenuItem56.AccessibleName = "Properties"
			Me.radMenuItem56.Image = My.Resources.properties
			Me.radMenuItem56.Name = "radMenuItem56"
			Me.radMenuItem56.Text = "Properties"
			Me.radMenuItem56.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuSeparatorItem4
			' 
			Me.radMenuSeparatorItem4.AccessibleDescription = "radMenuSeparatorItem4"
			Me.radMenuSeparatorItem4.AccessibleName = "radMenuSeparatorItem4"
			Me.radMenuSeparatorItem4.Name = "radMenuSeparatorItem4"
			Me.radMenuSeparatorItem4.Text = "radMenuSeparatorItem4"
			Me.radMenuSeparatorItem4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem57
			' 
			Me.radMenuItem57.AccessibleDescription = "About"
			Me.radMenuItem57.AccessibleName = "About"
			Me.radMenuItem57.Image = My.Resources.about_paint
			Me.radMenuItem57.Name = "radMenuItem57"
			Me.radMenuItem57.Text = "About"
			Me.radMenuItem57.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuSeparatorItem5
			' 
			Me.radMenuSeparatorItem5.AccessibleDescription = "radMenuSeparatorItem5"
			Me.radMenuSeparatorItem5.AccessibleName = "radMenuSeparatorItem5"
			Me.radMenuSeparatorItem5.Name = "radMenuSeparatorItem5"
			Me.radMenuSeparatorItem5.Text = "radMenuSeparatorItem5"
			Me.radMenuSeparatorItem5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem58
			' 
			Me.radMenuItem58.AccessibleDescription = "Exit"
			Me.radMenuItem58.AccessibleName = "Exit"
            Me.radMenuItem58.Image = My.Resources._exit
			Me.radMenuItem58.Name = "radMenuItem58"
			Me.radMenuItem58.Text = "Exit"
			Me.radMenuItem58.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem6
			' 
			Me.radMenuItem6.AccessibleDescription = "Recent Documents"
			Me.radMenuItem6.AccessibleName = "Recent Documents"
			Me.radMenuItem6.Enabled = False
			Me.radMenuItem6.Name = "radMenuItem6"
			Me.radMenuItem6.Text = "Recent Documents"
			Me.radMenuItem6.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem7
			' 
			Me.radMenuItem7.AccessibleDescription = "Untitled.png"
			Me.radMenuItem7.AccessibleName = "Untitled.png"
			Me.radMenuItem7.Name = "radMenuItem7"
			Me.radMenuItem7.Text = "Untitled.png"
			Me.radMenuItem7.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem8
			' 
			Me.radMenuItem8.AccessibleDescription = "TelerikLogo.png"
			Me.radMenuItem8.AccessibleName = "TelerikLogo.png"
			Me.radMenuItem8.Name = "radMenuItem8"
			Me.radMenuItem8.Text = "TelerikLogo.png"
			Me.radMenuItem8.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem8.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radMenuItem1
			' 
			Me.radMenuItem1.AccessibleDescription = "Test.png"
			Me.radMenuItem1.AccessibleName = "Test.png"
			Me.radMenuItem1.Name = "radMenuItem1"
			Me.radMenuItem1.Text = "Test.png"
			Me.radMenuItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement43
			' 
			Me.radButtonElement43.AccessibleDescription = "Office 2007 Blue"
			Me.radButtonElement43.AccessibleName = "Office 2007 Blue"
			Me.radButtonElement43.CanFocus = True
			Me.radButtonElement43.Class = "RibbonBarButtonElement"
			Me.radButtonElement43.Image = My.Resources.Blue
			Me.radButtonElement43.Name = "radButtonElement43"
			Me.radButtonElement43.Padding = New Padding(2)
			Me.radButtonElement43.StretchVertically = False
			Me.radButtonElement43.Text = "Office 2007 Blue"
			Me.radButtonElement43.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement43.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement43.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup4
			' 
			Me.radRibbonBarButtonGroup4.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement4, Me.radButtonElement6, Me.radButtonElement7})
			Me.radRibbonBarButtonGroup4.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup4.Name = "radRibbonBarButtonGroup4"
			Me.radRibbonBarButtonGroup4.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup4.StretchVertically = False
			Me.radRibbonBarButtonGroup4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement4
			' 
			Me.radButtonElement4.AccessibleDescription = "Cut"
			Me.radButtonElement4.AccessibleName = "Cut"
			Me.radButtonElement4.CanFocus = True
			Me.radButtonElement4.Class = "ButtonElement"
			Me.radButtonElement4.Image = (CType(resources.GetObject("radButtonElement4.Image"), Image))
			Me.radButtonElement4.ImageIndex = 6
			Me.radButtonElement4.MaxSize = New Size(0, 20)
			Me.radButtonElement4.Name = "radButtonElement4"
			Me.radButtonElement4.ScreenTip = Me.radOffice2007ScreenTip2.ScreenTipElement
			Me.radButtonElement4.Text = "Cut"
			Me.radButtonElement4.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement6
			' 
			Me.radButtonElement6.AccessibleDescription = "Copy"
			Me.radButtonElement6.AccessibleName = "Copy"
			Me.radButtonElement6.CanFocus = True
			Me.radButtonElement6.Class = "ButtonElement"
			Me.radButtonElement6.Image = (CType(resources.GetObject("radButtonElement6.Image"), Image))
			Me.radButtonElement6.ImageIndex = 4
			Me.radButtonElement6.MaxSize = New Size(0, 19)
			Me.radButtonElement6.Name = "radButtonElement6"
			Me.radButtonElement6.ScreenTip = Me.radOffice2007ScreenTip3.ScreenTipElement
			Me.radButtonElement6.Text = "Copy"
			Me.radButtonElement6.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement7
			' 
			Me.radButtonElement7.AccessibleDescription = "Delete"
			Me.radButtonElement7.AccessibleName = "Delete"
			Me.radButtonElement7.CanFocus = True
			Me.radButtonElement7.Class = "ButtonElement"
			Me.radButtonElement7.Image = (CType(resources.GetObject("radButtonElement7.Image"), Image))
			Me.radButtonElement7.ImageIndex = 25
			Me.radButtonElement7.Name = "radButtonElement7"
			Me.radButtonElement7.ScreenTip = Me.radOffice2007ScreenTip4.ScreenTipElement
			Me.radButtonElement7.Text = "Delete"
			Me.radButtonElement7.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup5
			' 
			Me.radRibbonBarButtonGroup5.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement8, Me.radButtonElement10, Me.radButtonElement11})
			Me.radRibbonBarButtonGroup5.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup5.Name = "radRibbonBarButtonGroup5"
			Me.radRibbonBarButtonGroup5.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup5.StretchVertically = False
			Me.radRibbonBarButtonGroup5.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement8
			' 
			Me.radButtonElement8.AccessibleDescription = "Cut"
			Me.radButtonElement8.AccessibleName = "Cut"
			Me.radButtonElement8.CanFocus = True
			Me.radButtonElement8.Class = "ButtonElement"
			Me.radButtonElement8.Image = (CType(resources.GetObject("radButtonElement8.Image"), Image))
			Me.radButtonElement8.ImageIndex = 6
			Me.radButtonElement8.MaxSize = New Size(0, 20)
			Me.radButtonElement8.Name = "radButtonElement8"
			Me.radButtonElement8.ScreenTip = Me.radOffice2007ScreenTip2.ScreenTipElement
			Me.radButtonElement8.Text = "Cut"
			Me.radButtonElement8.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement8.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement10
			' 
			Me.radButtonElement10.AccessibleDescription = "Copy"
			Me.radButtonElement10.AccessibleName = "Copy"
			Me.radButtonElement10.CanFocus = True
			Me.radButtonElement10.Class = "ButtonElement"
			Me.radButtonElement10.Image = (CType(resources.GetObject("radButtonElement10.Image"), Image))
			Me.radButtonElement10.ImageIndex = 4
			Me.radButtonElement10.MaxSize = New Size(0, 19)
			Me.radButtonElement10.Name = "radButtonElement10"
			Me.radButtonElement10.ScreenTip = Me.radOffice2007ScreenTip3.ScreenTipElement
			Me.radButtonElement10.Text = "Copy"
			Me.radButtonElement10.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement11
			' 
			Me.radButtonElement11.AccessibleDescription = "Delete"
			Me.radButtonElement11.AccessibleName = "Delete"
			Me.radButtonElement11.CanFocus = True
			Me.radButtonElement11.Class = "ButtonElement"
			Me.radButtonElement11.Image = (CType(resources.GetObject("radButtonElement11.Image"), Image))
			Me.radButtonElement11.ImageIndex = 25
			Me.radButtonElement11.Name = "radButtonElement11"
			Me.radButtonElement11.ScreenTip = Me.radOffice2007ScreenTip4.ScreenTipElement
			Me.radButtonElement11.Text = "Delete"
			Me.radButtonElement11.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement11.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement13
			' 
			Me.radButtonElement13.AccessibleDescription = "radButtonElement13"
			Me.radButtonElement13.AccessibleName = "radButtonElement13"
			Me.radButtonElement13.AutoSize = False
			Me.radButtonElement13.Bounds = New Rectangle(0, 0, 16, 16)
			Me.radButtonElement13.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement13.Image = My.Resources.resize
			Me.radButtonElement13.Name = "radButtonElement13"
			Me.radButtonElement13.Text = "radButtonElement13"
			Me.radButtonElement13.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement12
			' 
			Me.radButtonElement12.AccessibleDescription = "radButtonElement12"
			Me.radButtonElement12.AccessibleName = "radButtonElement12"
			Me.radButtonElement12.AutoSize = False
			Me.radButtonElement12.Bounds = New Rectangle(0, 0, 16, 16)
			Me.radButtonElement12.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement12.Image = My.Resources.crop
			Me.radButtonElement12.Name = "radButtonElement12"
			Me.radButtonElement12.Text = "radButtonElement12"
			Me.radButtonElement12.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup11
			' 
			Me.radRibbonBarButtonGroup11.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup11.Name = "radRibbonBarButtonGroup11"
			Me.radRibbonBarButtonGroup11.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup11.ShowBackColor = False
			Me.radRibbonBarButtonGroup11.ShowBorder = False
			Me.radRibbonBarButtonGroup11.StretchVertically = False
			Me.radRibbonBarButtonGroup11.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup12
			' 
			Me.radRibbonBarButtonGroup12.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup12.Name = "radRibbonBarButtonGroup12"
			Me.radRibbonBarButtonGroup12.ShowBackColor = False
			Me.radRibbonBarButtonGroup12.ShowBorder = False
			Me.radRibbonBarButtonGroup12.StretchVertically = False
			Me.radRibbonBarButtonGroup12.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup18
			' 
			Me.radRibbonBarButtonGroup18.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup18.Name = "radRibbonBarButtonGroup18"
			Me.radRibbonBarButtonGroup18.StretchVertically = False
			Me.radRibbonBarButtonGroup18.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup19
			' 
			Me.radRibbonBarButtonGroup19.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement28})
			Me.radRibbonBarButtonGroup19.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup19.Name = "radRibbonBarButtonGroup19"
			Me.radRibbonBarButtonGroup19.StretchVertically = False
			Me.radRibbonBarButtonGroup19.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement28
			' 
			Me.radButtonElement28.AccessibleDescription = "New button"
			Me.radButtonElement28.AccessibleName = "New button"
			Me.radButtonElement28.CanFocus = True
			Me.radButtonElement28.Class = "ButtonElement"
			Me.radButtonElement28.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement28.Image = (CType(resources.GetObject("radButtonElement28.Image"), Image))
			Me.radButtonElement28.ImageIndex = 14
			Me.radButtonElement28.Name = "radButtonElement28"
			Me.radButtonElement28.Text = "New button"
			Me.radButtonElement28.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup13
			' 
			Me.radRibbonBarButtonGroup13.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup18, Me.radRibbonBarButtonGroup19})
			Me.radRibbonBarButtonGroup13.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup13.Name = "radRibbonBarButtonGroup13"
			Me.radRibbonBarButtonGroup13.ShowBackColor = False
			Me.radRibbonBarButtonGroup13.ShowBorder = False
			Me.radRibbonBarButtonGroup13.StretchVertically = False
			Me.radRibbonBarButtonGroup13.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup8
			' 
			Me.radRibbonBarButtonGroup8.AccessibleDescription = "radRibbonBarButtonGroup8"
			Me.radRibbonBarButtonGroup8.AccessibleName = "radRibbonBarButtonGroup8"
			Me.radRibbonBarButtonGroup8.Name = "radRibbonBarButtonGroup8"
			Me.radRibbonBarButtonGroup8.Text = "radRibbonBarButtonGroup8"
			Me.radRibbonBarButtonGroup8.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup2
			' 
			Me.radRibbonBarButtonGroup2.AccessibleDescription = "radRibbonBarButtonGroup2"
			Me.radRibbonBarButtonGroup2.AccessibleName = "radRibbonBarButtonGroup2"
			Me.radRibbonBarButtonGroup2.Name = "radRibbonBarButtonGroup2"
			Me.radRibbonBarButtonGroup2.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup2.Text = "radRibbonBarButtonGroup2"
			Me.radRibbonBarButtonGroup2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup3
			' 
			Me.radRibbonBarGroup3.AccessibleDescription = "Themes"
			Me.radRibbonBarGroup3.AccessibleName = "Themes"
			Me.radRibbonBarGroup3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement22, Me.radButtonElement23, Me.radButtonElement24})
			Me.radRibbonBarGroup3.Margin = New Padding(0)
			Me.radRibbonBarGroup3.MaxSize = New Size(0, 0)
			Me.radRibbonBarGroup3.MinSize = New Size(0, 0)
			Me.radRibbonBarGroup3.Name = "radRibbonBarGroup3"
			Me.radRibbonBarGroup3.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup3.Text = "Themes"
			Me.radRibbonBarGroup3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement22
			' 
			Me.radButtonElement22.AccessibleDescription = "Office 2007 Blue"
			Me.radButtonElement22.AccessibleName = "Office 2007 Blue"
			Me.radButtonElement22.CanFocus = True
			Me.radButtonElement22.Class = "RibbonBarButtonElement"
			Me.radButtonElement22.Image = My.Resources.Blue
			Me.radButtonElement22.Name = "radButtonElement22"
			Me.radButtonElement22.Padding = New Padding(2)
			Me.radButtonElement22.StretchVertically = False
			Me.radButtonElement22.Text = "Office 2007 Blue"
			Me.radButtonElement22.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement22.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement22.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement23
			' 
			Me.radButtonElement23.AccessibleDescription = "Office 2007 Black"
			Me.radButtonElement23.AccessibleName = "Office 2007 Black"
			Me.radButtonElement23.CanFocus = True
			Me.radButtonElement23.Class = "RibbonBarButtonElement"
			Me.radButtonElement23.Image = My.Resources.Black
			Me.radButtonElement23.Name = "radButtonElement23"
			Me.radButtonElement23.Padding = New Padding(2)
			Me.radButtonElement23.StretchVertically = False
			Me.radButtonElement23.Text = "Office 2007 Black"
			Me.radButtonElement23.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement23.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement23.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement24
			' 
			Me.radButtonElement24.AccessibleDescription = "Office 2007 Silver"
			Me.radButtonElement24.AccessibleName = "Office 2007 Silver"
			Me.radButtonElement24.CanFocus = True
			Me.radButtonElement24.Class = "RibbonBarButtonElement"
			Me.radButtonElement24.Image = My.Resources.Silver
			Me.radButtonElement24.Name = "radButtonElement24"
			Me.radButtonElement24.Padding = New Padding(2)
			Me.radButtonElement24.StretchVertically = False
			Me.radButtonElement24.Text = "Office 2007 Silver"
			Me.radButtonElement24.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement24.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement24.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement6
			' 
			Me.radDropDownButtonElement6.AccessibleDescription = "Brushes"
			Me.radDropDownButtonElement6.AccessibleName = "Brushes"
			Me.radDropDownButtonElement6.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement6.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement6.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement6.ExpandArrowButton = False
			Me.radDropDownButtonElement6.Image = My.Resources.brush1
			Me.radDropDownButtonElement6.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radDropDownButtonElement6.MinSize = New Size(0, 72)
			Me.radDropDownButtonElement6.Name = "radDropDownButtonElement6"
			Me.radDropDownButtonElement6.Text = "Brushes"
			Me.radDropDownButtonElement6.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement6.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radDropDownButtonElement6.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement7
			' 
			Me.radDropDownButtonElement7.AccessibleDescription = "Brushes"
			Me.radDropDownButtonElement7.AccessibleName = "Brushes"
			Me.radDropDownButtonElement7.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement7.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement7.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement7.ExpandArrowButton = False
			Me.radDropDownButtonElement7.Image = My.Resources.brush1
			Me.radDropDownButtonElement7.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radDropDownButtonElement7.MinSize = New Size(0, 72)
			Me.radDropDownButtonElement7.Name = "radDropDownButtonElement7"
			Me.radDropDownButtonElement7.Text = "Brushes"
			Me.radDropDownButtonElement7.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement7.TextImageRelation = TextImageRelation.ImageAboveText
			Me.radDropDownButtonElement7.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement21
			' 
			Me.radButtonElement21.AccessibleDescription = "Fill"
			Me.radButtonElement21.AccessibleName = "Fill"
			Me.radButtonElement21.Class = "ButtonElement"
			Me.radButtonElement21.Image = My.Resources.fill
			Me.radButtonElement21.Margin = New Padding(2, 0, 0, 1)
			Me.radButtonElement21.Name = "radButtonElement21"
			Me.radButtonElement21.ShowBorder = False
			Me.radButtonElement21.Text = "Fill"
			Me.radButtonElement21.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement21.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement21.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radPanelContent
			' 
			Me.radPanelContent.BackColor = Color.FromArgb((CInt(Fix((CByte(203))))), (CInt(Fix((CByte(203))))), (CInt(Fix((CByte(202))))))
			Me.radPanelContent.BackgroundImage = My.Resources.rules
			Me.radPanelContent.Controls.Add(Me.radScrollablePanel1)
			Me.radPanelContent.Dock = DockStyle.Fill
			Me.radPanelContent.Location = New Point(0, 161)
			Me.radPanelContent.Margin = New Padding(0)
			Me.radPanelContent.Name = "radPanelContent"
			Me.radPanelContent.Padding = New Padding(20, 20, 0, 0)
			' 
			' 
			' 
			Me.radPanelContent.RootElement.Padding = New Padding(20, 20, 0, 0)
			Me.radPanelContent.Size = New Size(1304, 439)
			Me.radPanelContent.TabIndex = 3
			CType(Me.radPanelContent.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Padding = New Padding(20, 20, 0, 0)
			CType(Me.radPanelContent.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize
			CType(Me.radPanelContent.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Width = 0F
			' 
			' radScrollablePanel1
			' 
			Me.radScrollablePanel1.AutoScroll = True
			Me.radScrollablePanel1.AutoScrollMargin = New Size(0, 0)
			Me.radScrollablePanel1.AutoScrollMinSize = New Size(0, 0)
			Me.radScrollablePanel1.Dock = DockStyle.Fill
			Me.radScrollablePanel1.Location = New Point(20, 20)
			Me.radScrollablePanel1.Name = "radScrollablePanel1"
			' 
			' radScrollablePanel1.PanelContainer
			' 
			Me.radScrollablePanel1.PanelContainer.AutoScroll = True
			Me.radScrollablePanel1.PanelContainer.Controls.Add(Me.panel1)
			Me.radScrollablePanel1.PanelContainer.Controls.Add(Me.pictureBox1)
			Me.radScrollablePanel1.PanelContainer.Dock = DockStyle.Fill
			Me.radScrollablePanel1.PanelContainer.Location = New Point(0, 0)
			Me.radScrollablePanel1.PanelContainer.Name = "PanelContainer"
			Me.radScrollablePanel1.PanelContainer.Size = New Size(1267, 419)
			Me.radScrollablePanel1.PanelContainer.TabIndex = 0
			Me.radScrollablePanel1.Size = New Size(1284, 419)
			Me.radScrollablePanel1.TabIndex = 1
			Me.radScrollablePanel1.Text = "radScrollablePanel1"
			CType(Me.radScrollablePanel1.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).Width = 0F
			' 
			' panel1
			' 
			Me.panel1.BackColor = Color.Transparent
			Me.panel1.BackgroundImage = My.Resources.ribbon_Bubble
			Me.panel1.BackgroundImageLayout = ImageLayout.None
			Me.panel1.Controls.Add(Me.pictureBoxTool)
			Me.panel1.Location = New Point(788, 35)
			Me.panel1.Name = "panel1"
			Me.panel1.Size = New Size(50, 49)
			Me.panel1.TabIndex = 2
			' 
			' pictureBoxTool
			' 
			Me.pictureBoxTool.Location = New Point(16, 14)
			Me.pictureBoxTool.Name = "pictureBoxTool"
			Me.pictureBoxTool.Size = New Size(16, 16)
			Me.pictureBoxTool.TabIndex = 0
			Me.pictureBoxTool.TabStop = False
			' 
			' pictureBox1
			' 
			Me.pictureBox1.Image = My.Resources.content
			Me.pictureBox1.Location = New Point(90, 90)
			Me.pictureBox1.Name = "pictureBox1"
			Me.pictureBox1.Size = New Size(765, 425)
			Me.pictureBox1.TabIndex = 1
			Me.pictureBox1.TabStop = False
			' 
			' radRibbonBarGroup8
			' 
			Me.radRibbonBarGroup8.AccessibleDescription = "Clipboard"
			Me.radRibbonBarGroup8.AccessibleName = "Clipboard"
			Me.radRibbonBarGroup8.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement10, Me.radRibbonBarButtonGroup20})
			Me.radRibbonBarGroup8.Margin = New Padding(0)
			Me.radRibbonBarGroup8.MaxSize = New Size(0, 0)
			Me.radRibbonBarGroup8.MinSize = New Size(70, 77)
			Me.radRibbonBarGroup8.Name = "radRibbonBarGroup8"
			Me.radRibbonBarGroup8.Text = "Clipboard"
			Me.radRibbonBarGroup8.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement10
			' 
			Me.radDropDownButtonElement10.AccessibleDescription = "Paste"
			Me.radDropDownButtonElement10.AccessibleName = "Paste"
			Me.radDropDownButtonElement10.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement10.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement10.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement10.ExpandArrowButton = False
			Me.radDropDownButtonElement10.Image = My.Resources.new_paste_32
			Me.radDropDownButtonElement10.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement10.Margin = New Padding(0, 0, 1, 1)
			Me.radDropDownButtonElement10.Name = "radDropDownButtonElement10"
			Me.radDropDownButtonElement10.Text = "Paste"
			Me.radDropDownButtonElement10.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement10.TextImageRelation = TextImageRelation.Overlay
			Me.radDropDownButtonElement10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup20
			' 
			Me.radRibbonBarButtonGroup20.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup21})
			Me.radRibbonBarButtonGroup20.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup20.Name = "radRibbonBarButtonGroup20"
			Me.radRibbonBarButtonGroup20.ShowBackColor = False
			Me.radRibbonBarButtonGroup20.ShowBorder = False
			Me.radRibbonBarButtonGroup20.StretchVertically = False
			Me.radRibbonBarButtonGroup20.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup21
			' 
			Me.radRibbonBarButtonGroup21.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement33, Me.radButtonElement34, Me.radButtonElement35})
			Me.radRibbonBarButtonGroup21.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup21.Name = "radRibbonBarButtonGroup21"
			Me.radRibbonBarButtonGroup21.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup21.ShowBackColor = False
			Me.radRibbonBarButtonGroup21.ShowBorder = False
			Me.radRibbonBarButtonGroup21.StretchVertically = False
			Me.radRibbonBarButtonGroup21.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement33
			' 
			Me.radButtonElement33.AccessibleDescription = "Cut"
			Me.radButtonElement33.AccessibleName = "Cut"
			Me.radButtonElement33.CanFocus = True
			Me.radButtonElement33.Class = "RibbonBarButtonElement"
			Me.radButtonElement33.Image = (CType(resources.GetObject("radButtonElement33.Image"), Image))
			Me.radButtonElement33.ImageIndex = 6
			Me.radButtonElement33.MaxSize = New Size(0, 20)
			Me.radButtonElement33.Name = "radButtonElement33"
			Me.radButtonElement33.ScreenTip = Me.radOffice2007ScreenTip2.ScreenTipElement
			Me.radButtonElement33.Text = "Cut"
			Me.radButtonElement33.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement33.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement33.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement34
			' 
			Me.radButtonElement34.AccessibleDescription = "Copy"
			Me.radButtonElement34.AccessibleName = "Copy"
			Me.radButtonElement34.CanFocus = True
			Me.radButtonElement34.Class = "RibbonBarButtonElement"
			Me.radButtonElement34.Image = (CType(resources.GetObject("radButtonElement34.Image"), Image))
			Me.radButtonElement34.ImageIndex = 4
			Me.radButtonElement34.MaxSize = New Size(0, 19)
			Me.radButtonElement34.Name = "radButtonElement34"
			Me.radButtonElement34.ScreenTip = Me.radOffice2007ScreenTip3.ScreenTipElement
			Me.radButtonElement34.Text = "Copy"
			Me.radButtonElement34.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement34.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement34.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement35
			' 
			Me.radButtonElement35.AccessibleDescription = "Delete"
			Me.radButtonElement35.AccessibleName = "Delete"
			Me.radButtonElement35.CanFocus = True
			Me.radButtonElement35.Class = "RibbonBarButtonElement"
			Me.radButtonElement35.Image = (CType(resources.GetObject("radButtonElement35.Image"), Image))
			Me.radButtonElement35.ImageIndex = 25
			Me.radButtonElement35.Name = "radButtonElement35"
			Me.radButtonElement35.ScreenTip = Me.radOffice2007ScreenTip4.ScreenTipElement
			Me.radButtonElement35.Text = "Delete"
			Me.radButtonElement35.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement35.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement35.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup10
			' 
			Me.radRibbonBarGroup10.AccessibleDescription = "Clipboard"
			Me.radRibbonBarGroup10.AccessibleName = "Clipboard"
			Me.radRibbonBarGroup10.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radDropDownButtonElement11, Me.radRibbonBarButtonGroup22})
			Me.radRibbonBarGroup10.Margin = New Padding(0)
			Me.radRibbonBarGroup10.MaxSize = New Size(0, 0)
			Me.radRibbonBarGroup10.MinSize = New Size(70, 77)
			Me.radRibbonBarGroup10.Name = "radRibbonBarGroup10"
			Me.radRibbonBarGroup10.Text = "Clipboard"
			Me.radRibbonBarGroup10.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radDropDownButtonElement11
			' 
			Me.radDropDownButtonElement11.AccessibleDescription = "Paste"
			Me.radDropDownButtonElement11.AccessibleName = "Paste"
			Me.radDropDownButtonElement11.ArrowButtonMinSize = New Size(12, 12)
			Me.radDropDownButtonElement11.ArrowPosition = Telerik.WinControls.UI.DropDownButtonArrowPosition.Bottom
			Me.radDropDownButtonElement11.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down
			Me.radDropDownButtonElement11.ExpandArrowButton = False
			Me.radDropDownButtonElement11.Image = My.Resources.new_paste_32
			Me.radDropDownButtonElement11.ImageAlignment = ContentAlignment.TopCenter
			Me.radDropDownButtonElement11.Margin = New Padding(0, 0, 1, 1)
			Me.radDropDownButtonElement11.Name = "radDropDownButtonElement11"
			Me.radDropDownButtonElement11.Text = "Paste"
			Me.radDropDownButtonElement11.TextAlignment = ContentAlignment.BottomCenter
			Me.radDropDownButtonElement11.TextImageRelation = TextImageRelation.Overlay
			Me.radDropDownButtonElement11.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup22
			' 
			Me.radRibbonBarButtonGroup22.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup23})
			Me.radRibbonBarButtonGroup22.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup22.Name = "radRibbonBarButtonGroup22"
			Me.radRibbonBarButtonGroup22.ShowBackColor = False
			Me.radRibbonBarButtonGroup22.ShowBorder = False
			Me.radRibbonBarButtonGroup22.StretchVertically = False
			Me.radRibbonBarButtonGroup22.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup23
			' 
			Me.radRibbonBarButtonGroup23.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement36, Me.radButtonElement37, Me.radButtonElement38})
			Me.radRibbonBarButtonGroup23.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup23.Name = "radRibbonBarButtonGroup23"
			Me.radRibbonBarButtonGroup23.Orientation = Orientation.Vertical
			Me.radRibbonBarButtonGroup23.ShowBackColor = False
			Me.radRibbonBarButtonGroup23.ShowBorder = False
			Me.radRibbonBarButtonGroup23.StretchVertically = False
			Me.radRibbonBarButtonGroup23.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement36
			' 
			Me.radButtonElement36.AccessibleDescription = "Cut"
			Me.radButtonElement36.AccessibleName = "Cut"
			Me.radButtonElement36.CanFocus = True
			Me.radButtonElement36.Class = "RibbonBarButtonElement"
			Me.radButtonElement36.Image = (CType(resources.GetObject("radButtonElement36.Image"), Image))
			Me.radButtonElement36.ImageIndex = 6
			Me.radButtonElement36.MaxSize = New Size(0, 20)
			Me.radButtonElement36.Name = "radButtonElement36"
			Me.radButtonElement36.ScreenTip = Me.radOffice2007ScreenTip2.ScreenTipElement
			Me.radButtonElement36.Text = "Cut"
			Me.radButtonElement36.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement36.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement36.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement37
			' 
			Me.radButtonElement37.AccessibleDescription = "Copy"
			Me.radButtonElement37.AccessibleName = "Copy"
			Me.radButtonElement37.CanFocus = True
			Me.radButtonElement37.Class = "RibbonBarButtonElement"
			Me.radButtonElement37.Image = (CType(resources.GetObject("radButtonElement37.Image"), Image))
			Me.radButtonElement37.ImageIndex = 4
			Me.radButtonElement37.MaxSize = New Size(0, 19)
			Me.radButtonElement37.Name = "radButtonElement37"
			Me.radButtonElement37.ScreenTip = Me.radOffice2007ScreenTip3.ScreenTipElement
			Me.radButtonElement37.Text = "Copy"
			Me.radButtonElement37.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement37.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement37.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement38
			' 
			Me.radButtonElement38.AccessibleDescription = "Delete"
			Me.radButtonElement38.AccessibleName = "Delete"
			Me.radButtonElement38.CanFocus = True
			Me.radButtonElement38.Class = "RibbonBarButtonElement"
			Me.radButtonElement38.Image = (CType(resources.GetObject("radButtonElement38.Image"), Image))
			Me.radButtonElement38.ImageIndex = 25
			Me.radButtonElement38.Name = "radButtonElement38"
			Me.radButtonElement38.ScreenTip = Me.radOffice2007ScreenTip4.ScreenTipElement
			Me.radButtonElement38.Text = "Delete"
			Me.radButtonElement38.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement38.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement38.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement39
			' 
			Me.radButtonElement39.AccessibleDescription = "Cut"
			Me.radButtonElement39.AccessibleName = "Cut"
			Me.radButtonElement39.CanFocus = True
			Me.radButtonElement39.Class = "RibbonBarButtonElement"
			Me.radButtonElement39.Image = (CType(resources.GetObject("radButtonElement39.Image"), Image))
			Me.radButtonElement39.ImageIndex = 6
			Me.radButtonElement39.MaxSize = New Size(0, 20)
			Me.radButtonElement39.Name = "radButtonElement39"
			Me.radButtonElement39.ScreenTip = Me.radOffice2007ScreenTip2.ScreenTipElement
			Me.radButtonElement39.ShowBorder = False
			Me.radButtonElement39.Text = "Cut"
			Me.radButtonElement39.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement39.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement39.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup27
			' 
			Me.radRibbonBarButtonGroup27.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radRibbonBarButtonGroup28, Me.radRibbonBarButtonGroup29})
			Me.radRibbonBarButtonGroup27.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup27.Name = "radRibbonBarButtonGroup27"
			Me.radRibbonBarButtonGroup27.ShowBackColor = False
			Me.radRibbonBarButtonGroup27.ShowBorder = False
			Me.radRibbonBarButtonGroup27.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup28
			' 
			Me.radRibbonBarButtonGroup28.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement78, Me.radButtonElement79, Me.radButtonElement80, Me.radButtonElement81, Me.radButtonElement82, Me.radButtonElement83})
			Me.radRibbonBarButtonGroup28.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup28.Name = "radRibbonBarButtonGroup28"
			Me.radRibbonBarButtonGroup28.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement78
			' 
			Me.radButtonElement78.AccessibleDescription = "New button"
			Me.radButtonElement78.AccessibleName = "New button"
			Me.radButtonElement78.CanFocus = True
			Me.radButtonElement78.Class = "ButtonElement"
			Me.radButtonElement78.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement78.ImageIndex = 1
			Me.radButtonElement78.Name = "radButtonElement78"
			Me.radButtonElement78.Text = "New button"
			Me.radButtonElement78.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement79
			' 
			Me.radButtonElement79.AccessibleDescription = "New button"
			Me.radButtonElement79.AccessibleName = "New button"
			Me.radButtonElement79.CanFocus = True
			Me.radButtonElement79.Class = "ButtonElement"
			Me.radButtonElement79.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement79.ImageIndex = 16
			Me.radButtonElement79.Name = "radButtonElement79"
			Me.radButtonElement79.Text = "New button"
			Me.radButtonElement79.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement80
			' 
			Me.radButtonElement80.AccessibleDescription = "New button"
			Me.radButtonElement80.AccessibleName = "New button"
			Me.radButtonElement80.CanFocus = True
			Me.radButtonElement80.Class = "ButtonElement"
			Me.radButtonElement80.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement80.ImageIndex = 26
			Me.radButtonElement80.Name = "radButtonElement80"
			Me.radButtonElement80.Text = "New button"
			Me.radButtonElement80.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement81
			' 
			Me.radButtonElement81.AccessibleDescription = "New button"
			Me.radButtonElement81.AccessibleName = "New button"
			Me.radButtonElement81.CanFocus = True
			Me.radButtonElement81.Class = "ButtonElement"
			Me.radButtonElement81.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement81.ImageIndex = 22
			Me.radButtonElement81.Name = "radButtonElement81"
			Me.radButtonElement81.Text = "New button"
			Me.radButtonElement81.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement82
			' 
			Me.radButtonElement82.AccessibleDescription = "New button"
			Me.radButtonElement82.AccessibleName = "New button"
			Me.radButtonElement82.CanFocus = True
			Me.radButtonElement82.Class = "ButtonElement"
			Me.radButtonElement82.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement82.ImageIndex = 23
			Me.radButtonElement82.Name = "radButtonElement82"
			Me.radButtonElement82.Text = "New button"
			Me.radButtonElement82.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement83
			' 
			Me.radButtonElement83.AccessibleDescription = "New button"
			Me.radButtonElement83.AccessibleName = "New button"
			Me.radButtonElement83.CanFocus = True
			Me.radButtonElement83.Class = "ButtonElement"
			Me.radButtonElement83.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement83.ImageIndex = 24
			Me.radButtonElement83.Name = "radButtonElement83"
			Me.radButtonElement83.Text = "New button"
			Me.radButtonElement83.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarButtonGroup29
			' 
			Me.radRibbonBarButtonGroup29.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement84, Me.radButtonElement85})
			Me.radRibbonBarButtonGroup29.Margin = New Padding(2)
			Me.radRibbonBarButtonGroup29.Name = "radRibbonBarButtonGroup29"
			Me.radRibbonBarButtonGroup29.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement84
			' 
			Me.radButtonElement84.AccessibleDescription = "New button"
			Me.radButtonElement84.AccessibleName = "New button"
			Me.radButtonElement84.CanFocus = True
			Me.radButtonElement84.Class = "ButtonElement"
			Me.radButtonElement84.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement84.ImageIndex = 43
			Me.radButtonElement84.Name = "radButtonElement84"
			Me.radButtonElement84.ShowBorder = False
			Me.radButtonElement84.Text = "New button"
			Me.radButtonElement84.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement85
			' 
			Me.radButtonElement85.AccessibleDescription = "New button"
			Me.radButtonElement85.AccessibleName = "New button"
			Me.radButtonElement85.CanFocus = True
			Me.radButtonElement85.Class = "ButtonElement"
			Me.radButtonElement85.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButtonElement85.ImageIndex = 42
			Me.radButtonElement85.Name = "radButtonElement85"
			Me.radButtonElement85.ShowBorder = False
			Me.radButtonElement85.Text = "New button"
			Me.radButtonElement85.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radRibbonBarGroup15
			' 
			Me.radRibbonBarGroup15.AccessibleDescription = "Office 2010 Themes"
			Me.radRibbonBarGroup15.AccessibleName = "Office 2010 Themes"
			Me.radRibbonBarGroup15.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement54, Me.radButtonElement55, Me.radButtonElement56})
			Me.radRibbonBarGroup15.Name = "radRibbonBarGroup15"
			Me.radRibbonBarGroup15.Orientation = Orientation.Vertical
			Me.radRibbonBarGroup15.Text = "Office 2010 Themes"
			Me.radRibbonBarGroup15.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' radButtonElement54
			' 
			Me.radButtonElement54.AccessibleDescription = "Office 2010 Black"
			Me.radButtonElement54.AccessibleName = "Office 2010 Black"
			Me.radButtonElement54.Class = "RibbonBarButtonElement"
			Me.radButtonElement54.Image = My.Resources.Black
			Me.radButtonElement54.Name = "radButtonElement54"
			Me.radButtonElement54.Padding = New Padding(2)
			Me.radButtonElement54.Text = "Office 2010 Black"
			Me.radButtonElement54.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement54.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement54.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radButtonElement55
			' 
			Me.radButtonElement55.AccessibleDescription = "Office 2010 Blue"
			Me.radButtonElement55.AccessibleName = "Office 2010 Blue"
			Me.radButtonElement55.Class = "RibbonBarButtonElement"
			Me.radButtonElement55.Image = My.Resources.Blue
			Me.radButtonElement55.Name = "radButtonElement55"
			Me.radButtonElement55.Padding = New Padding(2)
			Me.radButtonElement55.Text = "Office 2010 Blue"
			Me.radButtonElement55.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement55.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement55.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radButtonElement56
			' 
			Me.radButtonElement56.AccessibleDescription = "Office 2010 Silver"
			Me.radButtonElement56.AccessibleName = "Office 2010 Silver"
			Me.radButtonElement56.Class = "RibbonBarButtonElement"
			Me.radButtonElement56.Image = My.Resources.Silver
			Me.radButtonElement56.Name = "radButtonElement56"
			Me.radButtonElement56.Padding = New Padding(2)
			Me.radButtonElement56.Text = "Office 2010 Silver"
			Me.radButtonElement56.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButtonElement56.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radButtonElement56.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.BackColor = Color.Transparent
			Me.Controls.Add(Me.radPanelContent)
			Me.Controls.Add(Me.radRibbonBar1)
			Me.Controls.Add(Me.radRibbonBarBackstageView1)
			Me.MaximumSize = New Size(0, 800)
			Me.Name = "Form1"
			Me.Size = New Size(1304, 600)
			Me.Controls.SetChildIndex(Me.radRibbonBarBackstageView1, 0)
			Me.Controls.SetChildIndex(Me.radRibbonBar1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			Me.Controls.SetChildIndex(Me.radPanelContent, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radOffice2007ScreenTip2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radOffice2007ScreenTip3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radOffice2007ScreenTip4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radOffice2007ScreenTip1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDropDownListElement1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDropDownListElement2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRibbonBar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRibbonBarBackstageView1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radRibbonBarBackstageView1.ResumeLayout(False)
			Me.backstageViewPage1.ResumeLayout(False)
			Me.backstageViewPage1.PerformLayout()
			CType(Me.pictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSeparator2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSeparator1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.backstageViewPage2.ResumeLayout(False)
			Me.backstageViewPage2.PerformLayout()
			CType(Me.radButton6, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton5, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton4, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSeparator3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanelContent, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelContent.ResumeLayout(False)
			Me.radScrollablePanel1.PanelContainer.ResumeLayout(False)
			CType(Me.radScrollablePanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radScrollablePanel1.ResumeLayout(False)
			Me.radScrollablePanel1.PerformLayout()
			Me.panel1.ResumeLayout(False)
			CType(Me.pictureBoxTool, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private radRibbonBar1 As Telerik.WinControls.UI.RadRibbonBar
		Private radRibbonBarChunk1 As RadRibbonBarGroup
		Private imageList1 As ImageList
		Private radRibbonBarChunk3 As RadRibbonBarGroup
		Private radRibbonBarChunk4 As RadRibbonBarGroup
		Private radMenuItem2 As RadMenuItem
		Private radMenuItem3 As RadMenuItem
		Private radMenuItem6 As RadMenuItem
		Private radMenuItem7 As RadMenuItem
		Private radMenuItem8 As RadMenuItem
		Private contextualTabGroup1 As ContextualTabGroup
		Private radButtonElement41 As RadButtonElement
		Private radButtonElement42 As RadButtonElement
		Private radOffice2007ScreenTip1 As RadOffice2007ScreenTip
		Private radOffice2007ScreenTip2 As RadOffice2007ScreenTip
		Private radOffice2007ScreenTip3 As RadOffice2007ScreenTip
		Private radOffice2007ScreenTip4 As RadOffice2007ScreenTip
		Private radGalleryElement1 As RadGalleryElement
		Private radGalleryItem1 As RadGalleryItem
		Private radGalleryItem2 As RadGalleryItem
		Private radGalleryItem3 As RadGalleryItem
		Private radGalleryItem4 As RadGalleryItem
		Private radGalleryItem5 As RadGalleryItem
		Private radGalleryItem6 As RadGalleryItem
		Private radGalleryItem7 As RadGalleryItem
		Private radGalleryItem8 As RadGalleryItem
		Private radGalleryItem9 As RadGalleryItem
		Private radGalleryItem10 As RadGalleryItem
		Private radGalleryItem11 As RadGalleryItem
		Private radGalleryItem12 As RadGalleryItem
		Private radGalleryItem13 As RadGalleryItem
		Private radGalleryItem14 As RadGalleryItem
		Private radGalleryItem15 As RadGalleryItem
		Private radGalleryItem16 As RadGalleryItem
		Private radRibbonBarChunk10 As RadRibbonBarGroup
		Private radGalleryGroupFilter4 As RadGalleryGroupFilter
		Private radGalleryGroupItem3 As RadGalleryGroupItem
		Private radGalleryGroupItem4 As RadGalleryGroupItem
		Private radRibbonBarCommandTab1 As RibbonTab
		Private radRibbonBarCommandTab2 As RibbonTab
		Private radRibbonBarCommandTab3 As RibbonTab
		Private radButtonElement43 As RadButtonElement
		Private radButtonElement47 As RadButtonElement
		Private radButtonElement48 As RadButtonElement
		Private radRibbonBarButtonGroup1 As RadRibbonBarButtonGroup
		Private radDropDownButtonElement1 As RadDropDownButtonElement
		Private radDropDownButtonElement2 As RadDropDownButtonElement
		Private radMenuHeaderItem1 As RadMenuHeaderItem
		Private radMenuItem9 As RadMenuItem
		Private radMenuItem10 As RadMenuItem
		Private radMenuHeaderItem2 As RadMenuHeaderItem
		Private radMenuItem11 As RadMenuItem
		Private radMenuItem15 As RadMenuItem
		Private radMenuItem16 As RadMenuItem
		Private radMenuItem17 As RadMenuItem
		Private radRibbonBarButtonGroup6 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup4 As RadRibbonBarButtonGroup
		Private radButtonElement4 As RadButtonElement
		Private radButtonElement6 As RadButtonElement
		Private radButtonElement7 As RadButtonElement
		Private radRibbonBarButtonGroup5 As RadRibbonBarButtonGroup
		Private radButtonElement8 As RadButtonElement
		Private radButtonElement10 As RadButtonElement
		Private radButtonElement11 As RadButtonElement
		Private radDropDownButtonElement3 As RadDropDownButtonElement
		Private radMenuItem18 As RadMenuItem
		Private radMenuItem19 As RadMenuItem
		Private radMenuItem20 As RadMenuItem
		Private radMenuItem21 As RadMenuItem
		Private radMenuItem22 As RadMenuItem
		Private radButtonElement13 As RadButtonElement
		Private radButtonElement12 As RadButtonElement
		Private radRibbonBarButtonGroup11 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup12 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup18 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup19 As RadRibbonBarButtonGroup
		Private radButtonElement28 As RadButtonElement
		Private radRibbonBarButtonGroup13 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup7 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup8 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup9 As RadRibbonBarButtonGroup
		Private radButtonElement9 As RadButtonElement
		Private radButtonElement14 As RadButtonElement
		Private radButtonElement15 As RadButtonElement
		Private radButtonElement16 As RadButtonElement
		Private radButtonElement17 As RadButtonElement
		Private radButtonElement18 As RadButtonElement
		Private radButtonElement19 As RadButtonElement
		Private radButtonElement20 As RadButtonElement
		Private radRibbonBarGroup2 As RadRibbonBarGroup
		Private radDropDownButtonElement4 As RadDropDownButtonElement
		Private radGalleryItem71 As RadGalleryItem
		Private radGalleryItem72 As RadGalleryItem
		Private radGalleryItem73 As RadGalleryItem
		Private radGalleryItem74 As RadGalleryItem
		Private radGalleryItem75 As RadGalleryItem
		Private radGalleryItem76 As RadGalleryItem
		Private radGalleryItem77 As RadGalleryItem
		Private radMenuButtonItem1 As RadMenuButtonItem
		Private radMenuSeparatorItem1 As RadMenuSeparatorItem
		Private radRibbonBarButtonGroup10 As RadRibbonBarButtonGroup
		Private radDropDownButtonElement5 As RadDropDownButtonElement
		Private radRibbonBarButtonGroup2 As RadRibbonBarButtonGroup
		Private radMenuItem12 As RadMenuItem
		Private radMenuItem13 As RadMenuItem
		Private radMenuItem14 As RadMenuItem
		Private radMenuItem24 As RadMenuItem
		Private radMenuItem25 As RadMenuItem
		Private radMenuItem26 As RadMenuItem
		Private radMenuItem27 As RadMenuItem
		Private radRibbonBarGroup4 As RadRibbonBarGroup
		Private radRibbonBarGroup3 As RadRibbonBarGroup
		Private radButtonElement22 As RadButtonElement
		Private radButtonElement23 As RadButtonElement
		Private radButtonElement24 As RadButtonElement
		Private radDropDownButtonElement8 As RadDropDownButtonElement
		Private radDropDownButtonElement6 As RadDropDownButtonElement
		Private radDropDownButtonElement7 As RadDropDownButtonElement
		Private radRibbonBarGroup5 As RadRibbonBarGroup
		Private radButtonElement25 As RadButtonElement
		Private radButtonElement26 As RadButtonElement
		Private radButtonElement21 As RadButtonElement
		Private radDropDownButtonElement9 As RadDropDownButtonElement
		Private radMenuItem28 As RadMenuItem
		Private radMenuItem29 As RadMenuItem
		Private radMenuItem30 As RadMenuItem
		Private radMenuItem31 As RadMenuItem
		Private radMenuItem32 As RadMenuItem
		Private radMenuItem33 As RadMenuItem
		Private radMenuItem34 As RadMenuItem
		Private radButtonElement49 As RadButtonElement
		Private radPanelContent As RadPanel
		Private pictureBox1 As PictureBox
		Private radMenuItem23 As RadMenuItem
		Private radMenuItem36 As RadMenuItem
		Private radMenuItem38 As RadMenuItem
		Private radMenuItem39 As RadMenuItem
		Private radMenuItem40 As RadMenuItem
		Private radMenuItem41 As RadMenuItem
		Private radMenuItem42 As RadMenuItem
		Private radMenuItem43 As RadMenuItem
		Private radMenuItem44 As RadMenuItem
		Private radMenuItem35 As RadMenuItem
		Private radMenuItem45 As RadMenuItem
		Private radMenuItem46 As RadMenuItem
		Private radMenuItem47 As RadMenuItem
		Private radRibbonBarButtonGroup14 As RadRibbonBarButtonGroup
		Private radRibbonBarGroup1 As RadRibbonBarGroup
		Private radButtonElement5 As RadButtonElement
		Private radButtonElement29 As RadButtonElement
		Private radRibbonBarGroup6 As RadRibbonBarGroup
		Private radRibbonBarGroup7 As RadRibbonBarGroup
		Private radButtonElement30 As RadButtonElement
		Private radCheckBoxElement1 As RadCheckBoxElement
		Private radCheckBoxElement2 As RadCheckBoxElement
		Private radCheckBoxElement3 As RadCheckBoxElement
		Private radButtonElement31 As RadButtonElement
		Private radButtonElement32 As RadButtonElement
		Private radRibbonBarGroup9 As RadRibbonBarGroup
		Private radDropDownButtonElement12 As RadDropDownButtonElement
		Private radRibbonBarGroup8 As RadRibbonBarGroup
		Private radDropDownButtonElement10 As RadDropDownButtonElement
		Private radRibbonBarButtonGroup20 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup21 As RadRibbonBarButtonGroup
		Private radButtonElement33 As RadButtonElement
		Private radButtonElement34 As RadButtonElement
		Private radButtonElement35 As RadButtonElement
		Private radRibbonBarGroup10 As RadRibbonBarGroup
		Private radDropDownButtonElement11 As RadDropDownButtonElement
		Private radRibbonBarButtonGroup22 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup23 As RadRibbonBarButtonGroup
		Private radButtonElement36 As RadButtonElement
		Private radButtonElement37 As RadButtonElement
		Private radButtonElement38 As RadButtonElement
		Private radMenuItem37 As RadMenuItem
		Private radMenuItem48 As RadMenuItem
		Private radButtonElement39 As RadButtonElement
		Private radRibbonBarGroup11 As RadRibbonBarGroup
		Private radDropDownListElement1 As RadDropDownListElement
		Private radDropDownListElement2 As RadDropDownListElement
		Private radRibbonBarButtonGroup25 As RadRibbonBarButtonGroup
		Private radRibbonBarGroup12 As RadRibbonBarGroup
		Private radButtonElement76 As RadButtonElement
		Private radButtonElement77 As RadButtonElement
		Private radMenuItem51 As RadMenuItem
		Private radMenuItem52 As RadMenuItem
		Private radMenuItem53 As RadMenuItem
		Private radMenuSeparatorItem2 As RadMenuSeparatorItem
		Private radMenuItem54 As RadMenuItem
		Private radMenuItem55 As RadMenuItem
		Private radMenuSeparatorItem3 As RadMenuSeparatorItem
		Private radMenuItem56 As RadMenuItem
		Private radMenuSeparatorItem4 As RadMenuSeparatorItem
		Private radMenuItem57 As RadMenuItem
		Private radMenuSeparatorItem5 As RadMenuSeparatorItem
		Private radMenuItem58 As RadMenuItem
		Private radMenuSeparatorItem6 As RadMenuSeparatorItem
		Private radMenuItem59 As RadMenuItem
		Private radMenuItem1 As RadMenuItem
		Private radMenuItem60 As RadMenuItem
		Private radMenuItem61 As RadMenuItem
		Private radMenuItem62 As RadMenuItem
		Private radMenuHeaderItem3 As RadMenuHeaderItem
		Private radMenuItem63 As RadMenuItem
		Private radMenuItem64 As RadMenuItem
		Private radMenuItem65 As RadMenuItem
		Private radMenuHeaderItem4 As RadMenuHeaderItem
		Private radRibbonBarButtonGroup27 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup28 As RadRibbonBarButtonGroup
		Private radButtonElement78 As RadButtonElement
		Private radButtonElement79 As RadButtonElement
		Private radButtonElement80 As RadButtonElement
		Private radButtonElement81 As RadButtonElement
		Private radButtonElement82 As RadButtonElement
		Private radButtonElement83 As RadButtonElement
		Private radRibbonBarButtonGroup29 As RadRibbonBarButtonGroup
		Private radButtonElement84 As RadButtonElement
		Private radButtonElement85 As RadButtonElement
		Private radScrollablePanel1 As RadScrollablePanel
		Private radRibbonBarButtonGroup26 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup30 As RadRibbonBarButtonGroup
		Private radRibbonBarButtonGroup31 As RadRibbonBarButtonGroup
		Private radButtonElement75 As RadButtonElement
		Private radButtonElement86 As RadButtonElement
		Private radButtonElement87 As RadButtonElement
		Private radButtonElement88 As RadButtonElement
		Private radButtonElement89 As RadButtonElement
		Private radButtonElement90 As RadButtonElement
		Private radButtonElement91 As RadButtonElement
		Private radButtonElement92 As RadButtonElement
		Private panel1 As Panel
		Private pictureBoxTool As PictureBox
		Private radRibbonBarGroup13 As RadRibbonBarGroup
		Private radButtonElement27 As RadButtonElement
		Private radButtonElement44 As RadButtonElement
		Private radRibbonBarButtonGroup15 As RadRibbonBarButtonGroup
		Private radButtonElement45 As RadButtonElement
		Private radMenuItem50 As RadMenuItem
		Private radMenuItem66 As RadMenuItem
		Private radRibbonBarGroup14 As RadRibbonBarGroup
		Private radButtonElement46 As RadButtonElement
		Private radButtonElement50 As RadButtonElement
		Private radButtonElement51 As RadButtonElement
		Private radButtonElement2 As RadButtonElement
		Private radButtonElement3 As RadButtonElement
		Private radButtonElement1 As RadButtonElement
		Private radRibbonBarButtonGroup3 As RadRibbonBarButtonGroup
		Private radButtonElement52 As RadButtonElement
		Private radButtonElement53 As RadButtonElement
		Private radButtonElement40 As RadButtonElement
		Private radRibbonBarBackstageView1 As RadRibbonBarBackstageView
		Private backstageButtonItem1 As BackstageButtonItem
		Private backstageButtonItem2 As BackstageButtonItem
		Private backstageButtonItem3 As BackstageButtonItem
		Private backstageButtonItem4 As BackstageButtonItem
		Private backstageTabItem1 As BackstageTabItem
		Private backstageTabItem2 As BackstageTabItem
		Private backstageButtonItem5 As BackstageButtonItem
		Private backstageButtonItem6 As BackstageButtonItem
		Private backstageButtonItem7 As BackstageButtonItem
		Private backstageViewPage1 As BackstageViewPage
		Private radLabel1 As RadLabel
		Private radSeparator1 As RadSeparator
		Private radButton1 As RadButton
		Private radButton2 As RadButton
		Private radButton3 As RadButton
		Private radSeparator2 As RadSeparator
		Private pictureBox2 As PictureBox
		Private backstageViewPage2 As BackstageViewPage
		Private radSeparator3 As RadSeparator
		Private radLabel2 As RadLabel
		Private radButton4 As RadButton
		Private radButton5 As RadButton
		Private radButton6 As RadButton
		Private radRibbonBarGroup15 As RadRibbonBarGroup
		Private radButtonElement54 As RadButtonElement
		Private radButtonElement55 As RadButtonElement
		Private radButtonElement56 As RadButtonElement
	End Class
End Namespace