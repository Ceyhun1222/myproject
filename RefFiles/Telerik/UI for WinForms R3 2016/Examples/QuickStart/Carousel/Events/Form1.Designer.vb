Namespace Telerik.Examples.WinControls.Carousel.Events
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
			Dim carouselBezierPath2 As New Telerik.WinControls.UI.CarouselBezierPath()
			Me.imageList4 = New ImageList(Me.components)
			Me.radListEvents = New Telerik.WinControls.UI.RadListControl()
			Me.radCarouselDemo = New Telerik.WinControls.UI.RadCarousel()
			Me.radSpinEditor1 = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radCheckBox1 = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
			Me.radGroupSettings = New Telerik.WinControls.UI.RadGroupBox()
			Me.radLblEvents = New Telerik.WinControls.UI.RadLabel()
			Me.radRadioEllipse = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioBezier = New Telerik.WinControls.UI.RadRadioButton()
			Me.radLblReflectionPerc = New Telerik.WinControls.UI.RadLabel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radListEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCarouselDemo, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupSettings.SuspendLayout()
			CType(Me.radLblEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioEllipse, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioBezier, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblReflectionPerc, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupSettings)
			Me.settingsPanel.Location = New Point(1023, 1)
			' 
			' 
			' 

			Me.settingsPanel.Size = New Size(200, 550)
			Me.settingsPanel.ThemeName = "ControlDefault"
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupSettings, 0)
			' 
			' imageList4
			' 
			Me.imageList4.ImageStream = (CType(resources.GetObject("imageList4.ImageStream"), ImageListStreamer))
			Me.imageList4.TransparentColor = Color.Transparent
			Me.imageList4.Images.SetKeyName(0, "Carousel01.png")
			Me.imageList4.Images.SetKeyName(1, "Carousel02.png")
			Me.imageList4.Images.SetKeyName(2, "Carousel03.png")
			Me.imageList4.Images.SetKeyName(3, "Carousel04.png")
			Me.imageList4.Images.SetKeyName(4, "Carousel05.png")
			Me.imageList4.Images.SetKeyName(5, "Carousel06.png")
			Me.imageList4.Images.SetKeyName(6, "Carousel07.png")
			' 
			' radListEvents
			' 
			Me.radListEvents.Location = New Point(11, 206)
			Me.radListEvents.Name = "radListEvents"
			Me.radListEvents.Size = New Size(158, 272)
			Me.radListEvents.TabIndex = 1
			' 
			' radCarouselDemo
			' 
			Me.radCarouselDemo.AutoLoopPauseCondition = Telerik.WinControls.UI.AutoLoopPauseConditions.OnMouseOverItem
			Me.radCarouselDemo.BackColor = Color.Transparent
			carouselBezierPath2.CtrlPoint1 = New Telerik.WinControls.UI.Point3D(125.91508052708639, 91.503267973856211, 100)
			carouselBezierPath2.CtrlPoint2 = New Telerik.WinControls.UI.Point3D(64.71449487554905, -35.62091503267974, -100)
			carouselBezierPath2.FirstPoint = New Telerik.WinControls.UI.Point3D(5.2708638360175692, 10.130718954248366, 0)
			carouselBezierPath2.LastPoint = New Telerik.WinControls.UI.Point3D(16.594516594516595, 79.950495049504951, 100)
			carouselBezierPath2.ZScale = 500
			Me.radCarouselDemo.CarouselPath = carouselBezierPath2
			Me.radCarouselDemo.Dock = DockStyle.Fill
			Me.radCarouselDemo.EnableAutoLoop = True
			Me.radCarouselDemo.Location = New Point(0, 0)
			Me.radCarouselDemo.Name = "radCarouselDemo"
			' 
			' 
			' 
			Me.radCarouselDemo.SelectedIndex = 0
			Me.radCarouselDemo.Size = New Size(1224, 552)
			Me.radCarouselDemo.TabIndex = 0
			Me.radCarouselDemo.Text = "radCarousel1"
			CType(Me.radCarouselDemo.GetChildAt(0).GetChildAt(3), Telerik.WinControls.UI.RadRepeatButtonElement).Image = My.Resources.carousel_leftArrow
			CType(Me.radCarouselDemo.GetChildAt(0).GetChildAt(4), Telerik.WinControls.UI.RadRepeatButtonElement).Image = My.Resources.carousel_rightArrow
			' 
			' radSpinEditor1
			' 
			Me.radSpinEditor1.DecimalPlaces = 2
			Me.radSpinEditor1.Increment = New Decimal(New Integer() { 1, 0, 0, 131072})
			Me.radSpinEditor1.Location = New Point(11, 91)
			Me.radSpinEditor1.Maximum = New Decimal(New Integer() { 1, 0, 0, 0})
			Me.radSpinEditor1.Name = "radSpinEditor1"
			' 
			' 
			' 
			Me.radSpinEditor1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radSpinEditor1.ShowBorder = True
			Me.radSpinEditor1.Size = New Size(158, 20)
			Me.radSpinEditor1.TabIndex = 7
			Me.radSpinEditor1.Value = New Decimal(New Integer() { 33, 0, 0, 131072})
			' 
			' radCheckBox1
			' 
			Me.radCheckBox1.Location = New Point(11, 117)
			Me.radCheckBox1.Name = "radCheckBox1"
			Me.radCheckBox1.Size = New Size(101, 17)
			Me.radCheckBox1.TabIndex = 10
			Me.radCheckBox1.Text = "Enable Looping"
			Me.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radCheckBox2
			' 
			Me.radCheckBox2.Location = New Point(11, 140)
			Me.radCheckBox2.Name = "radCheckBox2"
			Me.radCheckBox2.Size = New Size(112, 17)
			Me.radCheckBox2.TabIndex = 11
			Me.radCheckBox2.Text = "Enable Auto Loop"
			Me.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On

			' 
			' radGroupSettings
			' 
			Me.radGroupSettings.Anchor = AnchorStyles.Top
			Me.radGroupSettings.Controls.Add(Me.radLblEvents)
			Me.radGroupSettings.Controls.Add(Me.radRadioEllipse)
			Me.radGroupSettings.Controls.Add(Me.radRadioBezier)
			Me.radGroupSettings.Controls.Add(Me.radLblReflectionPerc)
			Me.radGroupSettings.Controls.Add(Me.radListEvents)
			Me.radGroupSettings.Controls.Add(Me.radCheckBox2)
			Me.radGroupSettings.Controls.Add(Me.radCheckBox1)
			Me.radGroupSettings.Controls.Add(Me.radSpinEditor1)
			Me.radGroupSettings.FooterImageIndex = -1
			Me.radGroupSettings.FooterImageKey = ""
			Me.radGroupSettings.FooterText = ""

			Me.radGroupSettings.HeaderImageIndex = -1
			Me.radGroupSettings.HeaderImageKey = ""
			Me.radGroupSettings.HeaderMargin = New Padding(0)
			Me.radGroupSettings.HeaderText = "Carousel Settings"
			Me.radGroupSettings.Location = New Point(10, 6)
			Me.radGroupSettings.Name = "radGroupSettings"
			' 
			' 
			' 

			Me.radGroupSettings.Size = New Size(180, 493)
			Me.radGroupSettings.TabIndex = 1
			Me.radGroupSettings.Text = "Carousel Settings"
			' 
			' radLblEvents
			' 

			Me.radLblEvents.Location = New Point(11, 186)
			Me.radLblEvents.Name = "radLblEvents"
			' 
			' 
			' 

			Me.radLblEvents.Size = New Size(42, 14)
			Me.radLblEvents.TabIndex = 13
			Me.radLblEvents.Text = "Events:"
			' 
			' radRadioEllipse
			' 

			Me.radRadioEllipse.Location = New Point(11, 49)
			Me.radRadioEllipse.Name = "radRadioEllipse"
			Me.radRadioEllipse.RadioCheckAlignment = ContentAlignment.MiddleLeft
			' 
			' 
			' 

			Me.radRadioEllipse.Size = New Size(80, 16)
			Me.radRadioEllipse.TabIndex = 12
			Me.radRadioEllipse.Text = "Ellipse Path"
			' 
			' radRadioBezier
			' 

			Me.radRadioBezier.Location = New Point(11, 27)
			Me.radRadioBezier.Name = "radRadioBezier"
			Me.radRadioBezier.RadioCheckAlignment = ContentAlignment.MiddleLeft
			' 
			' 
			' 

			Me.radRadioBezier.Size = New Size(79, 16)
			Me.radRadioBezier.TabIndex = 12
			Me.radRadioBezier.Text = "Bezier Path"

			' 
			' radLblReflectionPerc
			' 

			Me.radLblReflectionPerc.Location = New Point(11, 71)
			Me.radLblReflectionPerc.Name = "radLblReflectionPerc"
			' 
			' 
			' 

			Me.radLblReflectionPerc.Size = New Size(144, 14)
			Me.radLblReflectionPerc.TabIndex = 1
			Me.radLblReflectionPerc.Text = "Item Reflection Percentage:"
			' 
			' Form1
			' 
			Me.AutoSize = True
			Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
			Me.Controls.Add(Me.radCarouselDemo)
			Me.Name = "Form1"
			Me.Size = New Size(1224, 552)

			Me.Controls.SetChildIndex(Me.radCarouselDemo, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radListEvents, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCarouselDemo, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupSettings.ResumeLayout(False)
			Me.radGroupSettings.PerformLayout()
			CType(Me.radLblEvents, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioEllipse, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioBezier, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblReflectionPerc, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radCarouselDemo As Telerik.WinControls.UI.RadCarousel
		Private imageList4 As ImageList
		Private radListEvents As Telerik.WinControls.UI.RadListControl
		Private radSpinEditor1 As Telerik.WinControls.UI.RadSpinEditor
		Private radCheckBox1 As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
		Private radGroupSettings As Telerik.WinControls.UI.RadGroupBox
		Private radLblReflectionPerc As Telerik.WinControls.UI.RadLabel
		Private radRadioEllipse As Telerik.WinControls.UI.RadRadioButton
		Private radRadioBezier As Telerik.WinControls.UI.RadRadioButton
		Private radLblEvents As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace