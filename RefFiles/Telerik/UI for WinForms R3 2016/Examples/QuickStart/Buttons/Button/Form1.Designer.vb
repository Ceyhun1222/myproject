Namespace Telerik.Examples.WinControls.Buttons.Button
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
			Me.imageList1 = New ImageList(Me.components)
			Me.radButton3 = New Telerik.WinControls.UI.RadButton()
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			Me.radButton2 = New Telerik.WinControls.UI.RadButton()
			Me.groupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radRadioOverlay = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioTxtBeforeImg = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioTxtAboveImg = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioImgBeforeTxt = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioImgAboveTxt = New Telerik.WinControls.UI.RadRadioButton()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radButton3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.groupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.groupBox1.SuspendLayout()
			CType(Me.radRadioOverlay, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioTxtBeforeImg, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioTxtAboveImg, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioImgBeforeTxt, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioImgAboveTxt, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radLabel1)
			Me.radPanelDemoHolder.Controls.Add(Me.radLabel3)
			Me.radPanelDemoHolder.Controls.Add(Me.radButton3)
			Me.radPanelDemoHolder.Controls.Add(Me.radLabel2)
			Me.radPanelDemoHolder.Controls.Add(Me.radButton1)
			Me.radPanelDemoHolder.Controls.Add(Me.radButton2)
			Me.radPanelDemoHolder.Location = New Point(0, 0)
			Me.radPanelDemoHolder.Size = New Size(396, 185)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.groupBox1)
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(200, 386)
			Me.settingsPanel.Controls.SetChildIndex(Me.groupBox1, 0)
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Fuchsia
			Me.imageList1.Images.SetKeyName(0, "print.gif")
			Me.imageList1.Images.SetKeyName(1, "bulb_on.GIF")
			Me.imageList1.Images.SetKeyName(2, "bulb_off.GIF")
			Me.imageList1.Images.SetKeyName(3, "iconDropDown.bmp")
			Me.imageList1.Images.SetKeyName(4, "iconMoveToFolder.bmp")
			' 
			' radButton3
			' 
			Me.radButton3.AutoSize = True
			Me.radButton3.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButton3.ImageIndex = 3
			Me.radButton3.ImageList = Me.imageList1
			Me.radButton3.Location = New Point(147, 62)
			Me.radButton3.Name = "radButton3"
			' 
			' 
			' 
			Me.radButton3.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radButton3.Size = New Size(95, 29)
			Me.radButton3.TabIndex = 5
			Me.radButton3.Text = "Check Mail"
			Me.radButton3.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButton3.TextImageRelation = TextImageRelation.ImageBeforeText
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Image = (CType(resources.GetObject("resource.Image"), Image))
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).ImageIndex = 3
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).TextImageRelation = TextImageRelation.ImageBeforeText
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).ImageAlignment = ContentAlignment.MiddleCenter
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).TextAlignment = ContentAlignment.MiddleLeft
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Check Mail"
			CType(Me.radButton3.GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Padding = New Padding(4)
			' 
			' radButton1
			' 
			Me.radButton1.Location = New Point(147, 0)
			Me.radButton1.Name = "radButton1"
			Me.radButton1.Size = New Size(96, 25)
			Me.radButton1.TabIndex = 3
			Me.radButton1.Text = "Check Mail"
			' 
			' radButton2
			' 
			Me.radButton2.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButton2.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radButton2.ImageIndex = 3
			Me.radButton2.ImageList = Me.imageList1
			Me.radButton2.ImageScalingSize = New Size(24, 24)
			Me.radButton2.Location = New Point(147, 31)
			Me.radButton2.Name = "radButton2"
			' 
			' 
			' 
			Me.radButton2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radButton2.Size = New Size(96, 25)
			Me.radButton2.TabIndex = 3
			Me.radButton2.Text = "radButton2"
			Me.radButton2.TextAlignment = ContentAlignment.MiddleLeft
			Me.radButton2.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' groupBox1
			' 
			Me.groupBox1.AccessibleRole = AccessibleRole.Grouping
			Me.groupBox1.Anchor = AnchorStyles.Top
			Me.groupBox1.Controls.Add(Me.radRadioOverlay)
			Me.groupBox1.Controls.Add(Me.radRadioTxtBeforeImg)
			Me.groupBox1.Controls.Add(Me.radRadioTxtAboveImg)
			Me.groupBox1.Controls.Add(Me.radRadioImgBeforeTxt)
			Me.groupBox1.Controls.Add(Me.radRadioImgAboveTxt)
			Me.groupBox1.FooterText = ""
			Me.groupBox1.HeaderMargin = New Padding(10, 0, 0, 0)
			Me.groupBox1.HeaderText = "Text Image Relation"
			Me.groupBox1.Location = New Point(10, 6)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.groupBox1.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.groupBox1.Size = New Size(180, 152)
			Me.groupBox1.TabIndex = 19
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Text Image Relation"
			' 
			' radRadioOverlay
			' 
			Me.radRadioOverlay.Location = New Point(12, 122)
			Me.radRadioOverlay.Name = "radRadioOverlay"
			Me.radRadioOverlay.Size = New Size(58, 18)
			Me.radRadioOverlay.TabIndex = 0
			Me.radRadioOverlay.Text = "Overlay"
			' 
			' radRadioTxtBeforeImg
			' 
			Me.radRadioTxtBeforeImg.Location = New Point(12, 100)
			Me.radRadioTxtBeforeImg.Name = "radRadioTxtBeforeImg"
			Me.radRadioTxtBeforeImg.Size = New Size(111, 18)
			Me.radRadioTxtBeforeImg.TabIndex = 0
			Me.radRadioTxtBeforeImg.Text = "Text Before Image"
			' 
			' radRadioTxtAboveImg
			' 
			Me.radRadioTxtAboveImg.Location = New Point(12, 78)
			Me.radRadioTxtAboveImg.Name = "radRadioTxtAboveImg"
			Me.radRadioTxtAboveImg.Size = New Size(110, 18)
			Me.radRadioTxtAboveImg.TabIndex = 0
			Me.radRadioTxtAboveImg.Text = "Text Above Image"
			' 
			' radRadioImgBeforeTxt
			' 
			Me.radRadioImgBeforeTxt.Location = New Point(12, 56)
			Me.radRadioImgBeforeTxt.Name = "radRadioImgBeforeTxt"
			Me.radRadioImgBeforeTxt.Size = New Size(111, 18)
			Me.radRadioImgBeforeTxt.TabIndex = 0
			Me.radRadioImgBeforeTxt.Text = "Image Before Text"
			' 
			' radRadioImgAboveTxt
			' 
			Me.radRadioImgAboveTxt.Location = New Point(12, 34)
			Me.radRadioImgAboveTxt.Name = "radRadioImgAboveTxt"
			Me.radRadioImgAboveTxt.Size = New Size(110, 18)
			Me.radRadioImgAboveTxt.TabIndex = 0
			Me.radRadioImgAboveTxt.Text = "Image Above Text"
			' 
			' radLabel3
			' 
			Me.radLabel3.Location = New Point(-3, 71)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New Size(122, 18)
			Me.radLabel3.TabIndex = 6
			Me.radLabel3.Text = "Text And Image Button"
			' 
			' radLabel2
			' 
			Me.radLabel2.Location = New Point(-3, 38)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New Size(74, 18)
			Me.radLabel2.TabIndex = 6
			Me.radLabel2.Text = "Image Button"
			' 
			' radLabel1
			' 
			Me.radLabel1.Location = New Point(-3, 4)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New Size(64, 18)
			Me.radLabel1.TabIndex = 6
			Me.radLabel1.Text = "Text Button"
			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			Me.Size = New Size(1170, 671)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radButton3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.groupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			CType(Me.radRadioOverlay, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioTxtBeforeImg, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioTxtAboveImg, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioImgBeforeTxt, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioImgAboveTxt, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radButton1 As Telerik.WinControls.UI.RadButton
		Private radButton2 As Telerik.WinControls.UI.RadButton
		Private imageList1 As ImageList
		Private radButton3 As Telerik.WinControls.UI.RadButton
		Private groupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radRadioOverlay As Telerik.WinControls.UI.RadRadioButton
		Private radRadioTxtBeforeImg As Telerik.WinControls.UI.RadRadioButton
		Private radRadioTxtAboveImg As Telerik.WinControls.UI.RadRadioButton
		Private radRadioImgBeforeTxt As Telerik.WinControls.UI.RadRadioButton
		Private radRadioImgAboveTxt As Telerik.WinControls.UI.RadRadioButton
	End Class
End Namespace
