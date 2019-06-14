Namespace Telerik.Examples.WinControls.Rotator
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
			Me.radRotator1 = New Telerik.WinControls.UI.RadRotator()
			Me.radItemsContainer1 = New Telerik.WinControls.UI.RadItemsContainer()
			Me.radImageItem1 = New Telerik.WinControls.UI.RadImageItem()
			Me.radItemsContainer2 = New Telerik.WinControls.UI.RadItemsContainer()
			Me.radButtonElement1 = New Telerik.WinControls.UI.RadButtonElement()
			Me.radLabelElement1 = New Telerik.WinControls.UI.RadLabelElement()
			Me.radTextBoxElement1 = New Telerik.WinControls.UI.RadTextBoxElement()
			Me.radWebBrowserItem1 = New Telerik.WinControls.UI.RadWebBrowserItem()
			Me.radButtonPrevious = New Telerik.WinControls.UI.RadButton()
			Me.radButtonNext = New Telerik.WinControls.UI.RadButton()
			Me.radStartStopButton = New Telerik.WinControls.UI.RadButton()
			Me.currentFrameTextBox = New Telerik.WinControls.UI.RadTextBox()
			Me.radLabelInterval = New Telerik.WinControls.UI.RadLabel()
			Me.intervalTextBox = New Telerik.WinControls.UI.RadTextBox()
			Me.radCheckBoxOpacity = New Telerik.WinControls.UI.RadCheckBox()
			Me.locationAnimationTextBox = New Telerik.WinControls.UI.RadTextBox()
			Me.radLabelMsec = New Telerik.WinControls.UI.RadLabel()
			Me.radLabelLocationAnimation = New Telerik.WinControls.UI.RadLabel()
			Me.radButtonApply = New Telerik.WinControls.UI.RadButton()
			Me.radGroupSettings = New Telerik.WinControls.UI.RadGroupBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radRotator1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPrevious, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonNext, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radStartStopButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.currentFrameTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelInterval, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.intervalTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxOpacity, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.locationAnimationTextBox, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelMsec, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelLocationAnimation, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonApply, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupSettings.SuspendLayout()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupSettings)
			Me.settingsPanel.ForeColor = Color.Black
			Me.settingsPanel.Location = New Point(1023, 1)
			' 
			' 
			' 
			Me.settingsPanel.RootElement.ForeColor = Color.Black
			Me.settingsPanel.Size = New Size(200, 818)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupSettings, 0)
			' 
			' radRotator1
			' 
			Me.radRotator1.Anchor = (CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radRotator1.BackColor = Color.FromArgb((CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))), (CInt(Fix((CByte(255))))))
			Me.radRotator1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radItemsContainer1, Me.radItemsContainer2, Me.radWebBrowserItem1})
			Me.radRotator1.Location = New Point(3, 3)
			Me.radRotator1.LocationAnimation = New SizeF(0F, -1F)
			Me.radRotator1.Name = "radRotator1"
			' 
			' 
			' 
			Me.radRotator1.RootElement.ForeColor = Color.Black
			Me.radRotator1.Running = False
			Me.radRotator1.Size = New Size(1218, 788)
			Me.radRotator1.TabIndex = 0
			Me.radRotator1.Text = "radRotator1"

			' 
			' radItemsContainer1
			' 
			Me.radItemsContainer1.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radImageItem1})
			Me.radItemsContainer1.Name = "radItemsContainer1"
			Me.radItemsContainer1.Text = "radItemsContainer1"
			Me.radItemsContainer1.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radImageItem1
			' 
			Me.radImageItem1.Alignment = ContentAlignment.MiddleCenter
			Me.radImageItem1.Image = My.Resources.RadRotatorWinF
			Me.radImageItem1.Name = "radImageItem1"
			Me.radImageItem1.StretchHorizontally = False
			Me.radImageItem1.StretchVertically = False
			Me.radImageItem1.Text = "radImageItem1"
			' 
			' radItemsContainer2
			' 
			Me.radItemsContainer2.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radButtonElement1, Me.radLabelElement1, Me.radTextBoxElement1})
			Me.radItemsContainer2.Name = "radItemsContainer2"
			Me.radItemsContainer2.Text = "radItemsContainer2"
			Me.radItemsContainer2.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radButtonElement1
			' 
			Me.radButtonElement1.CanFocus = True
			Me.radButtonElement1.Name = "radButtonElement1"
			Me.radButtonElement1.Padding = New Padding(0, 4, 0, 4)
			Me.radButtonElement1.StretchHorizontally = False
			Me.radButtonElement1.StretchVertically = False
			Me.radButtonElement1.Text = "Previous frame"

			' 
			' radLabelElement1
			' 
			Me.radLabelElement1.Alignment = ContentAlignment.MiddleCenter
			Me.radLabelElement1.Font = New Font("Calibri", 36F, (CType((FontStyle.Bold Or FontStyle.Italic), FontStyle)), GraphicsUnit.Point, (CByte(0)))
			Me.radLabelElement1.Name = "radLabelElement1"
			Me.radLabelElement1.Opacity = 0.25
			Me.radLabelElement1.StretchHorizontally = False
			Me.radLabelElement1.StretchVertically = False
			Me.radLabelElement1.Text = "Second rotator frame"
			Me.radLabelElement1.TextAlignment = ContentAlignment.MiddleLeft
			Me.radLabelElement1.TextWrap = True
			' 
			' radTextBoxElement1
			' 
			Me.radTextBoxElement1.Alignment = ContentAlignment.BottomRight
			Me.radTextBoxElement1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize
			Me.radTextBoxElement1.BackColor = Color.Black
			Me.radTextBoxElement1.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radTextBoxElement1.ForeColor = Color.White
			Me.radTextBoxElement1.MinSize = New Size(500, 0)
			Me.radTextBoxElement1.Name = "radTextBoxElement1"
			Me.radTextBoxElement1.Padding = New Padding(2, 2, 2, 3)
			Me.radTextBoxElement1.StretchHorizontally = False
			Me.radTextBoxElement1.StretchVertically = False
			' 
			' radWebBrowserItem1
			' 
			Me.radWebBrowserItem1.CanFocus = True
			Me.radWebBrowserItem1.DocumentText = "<HTML></HTML>" & vbNullChar
			Me.radWebBrowserItem1.Name = "radWebBrowserItem1"
			Me.radWebBrowserItem1.Text = "radWebBrowserItem1"
			Me.radWebBrowserItem1.Url = New Uri("about:blank", UriKind.Absolute)
			Me.radWebBrowserItem1.Visibility = Telerik.WinControls.ElementVisibility.Hidden
			' 
			' radButtonPrevious
			' 
			Me.radButtonPrevious.Anchor = (CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles))
			Me.radButtonPrevious.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radButtonPrevious.Location = New Point(1089, 797)
			Me.radButtonPrevious.Name = "radButtonPrevious"
			Me.radButtonPrevious.ShowItemToolTips = False
			Me.radButtonPrevious.Size = New Size(40, 20)
			Me.radButtonPrevious.TabIndex = 1
			Me.radButtonPrevious.Text = "|<"

			' 
			' radButtonNext
			' 
			Me.radButtonNext.Anchor = (CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles))
			Me.radButtonNext.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radButtonNext.Location = New Point(1181, 797)
			Me.radButtonNext.Name = "radButtonNext"
			Me.radButtonNext.Size = New Size(40, 20)
			Me.radButtonNext.TabIndex = 2
			Me.radButtonNext.Text = ">|"

			' 
			' radStartStopButton
			' 
			Me.radStartStopButton.Anchor = (CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles))
			Me.radStartStopButton.Font = New Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radStartStopButton.Location = New Point(1135, 797)
			Me.radStartStopButton.Name = "radStartStopButton"
			Me.radStartStopButton.Size = New Size(40, 20)
			Me.radStartStopButton.TabIndex = 3

			' 
			' currentFrameTextBox
			' 
			Me.currentFrameTextBox.Anchor = (CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles))
			Me.currentFrameTextBox.Location = New Point(1045, 797)
			Me.currentFrameTextBox.Name = "currentFrameTextBox"
			' 
			' 
			' 
			Me.currentFrameTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.currentFrameTextBox.Size = New Size(40, 20)
			Me.currentFrameTextBox.TabIndex = 4
			Me.currentFrameTextBox.TabStop = False

			' 
			' radLabelInterval
			' 
			Me.radLabelInterval.Location = New Point(9, 27)
			Me.radLabelInterval.Name = "radLabelInterval"
			Me.radLabelInterval.Size = New Size(43, 16)
			Me.radLabelInterval.TabIndex = 5
			Me.radLabelInterval.Text = "Interval"
			Me.radLabelInterval.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' intervalTextBox
			' 
			Me.intervalTextBox.Location = New Point(30, 47)
			Me.intervalTextBox.MaxLength = 5
			Me.intervalTextBox.Name = "intervalTextBox"
			' 
			' 
			' 
			Me.intervalTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.intervalTextBox.Size = New Size(36, 20)
			Me.intervalTextBox.TabIndex = 6
			Me.intervalTextBox.TabStop = False
			Me.intervalTextBox.Text = "2000"
			' 
			' radCheckBoxOpacity
			' 
			Me.radCheckBoxOpacity.Location = New Point(9, 119)
			Me.radCheckBoxOpacity.Name = "radCheckBoxOpacity"
			Me.radCheckBoxOpacity.Size = New Size(114, 17)
			Me.radCheckBoxOpacity.TabIndex = 7
			Me.radCheckBoxOpacity.Text = "Opacity Animation"
			Me.radCheckBoxOpacity.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On

			' 
			' locationAnimationTextBox
			' 
			Me.locationAnimationTextBox.Location = New Point(30, 93)
			Me.locationAnimationTextBox.Name = "locationAnimationTextBox"
			' 
			' 
			' 
			Me.locationAnimationTextBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.locationAnimationTextBox.Size = New Size(50, 20)
			Me.locationAnimationTextBox.TabIndex = 8
			Me.locationAnimationTextBox.TabStop = False
			Me.locationAnimationTextBox.Text = "0, -1"
			' 
			' radLabelMsec
			' 
			Me.radLabelMsec.Location = New Point(71, 53)
			Me.radLabelMsec.Name = "radLabelMsec"
			Me.radLabelMsec.Size = New Size(21, 16)
			Me.radLabelMsec.TabIndex = 9
			Me.radLabelMsec.Text = "ms"
			Me.radLabelMsec.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' radLabelLocationAnimation
			' 
			Me.radLabelLocationAnimation.Location = New Point(9, 73)
			Me.radLabelLocationAnimation.Name = "radLabelLocationAnimation"
			Me.radLabelLocationAnimation.Size = New Size(104, 16)
			Me.radLabelLocationAnimation.TabIndex = 10
			Me.radLabelLocationAnimation.Text = "Location animation:"
			Me.radLabelLocationAnimation.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' radButtonApply
			' 
			Me.radButtonApply.Location = New Point(9, 157)
			Me.radButtonApply.Name = "radButtonApply"
			Me.radButtonApply.Size = New Size(141, 24)
			Me.radButtonApply.TabIndex = 11
			Me.radButtonApply.Text = "Apply"

			' 
			' radGroupSettings
			' 
			Me.radGroupSettings.Anchor = AnchorStyles.Top
			Me.radGroupSettings.Controls.Add(Me.radLabelInterval)
			Me.radGroupSettings.Controls.Add(Me.intervalTextBox)
			Me.radGroupSettings.Controls.Add(Me.radButtonApply)
			Me.radGroupSettings.Controls.Add(Me.radLabelMsec)
			Me.radGroupSettings.Controls.Add(Me.radCheckBoxOpacity)
			Me.radGroupSettings.Controls.Add(Me.locationAnimationTextBox)
			Me.radGroupSettings.Controls.Add(Me.radLabelLocationAnimation)
			Me.radGroupSettings.FooterImageIndex = -1
			Me.radGroupSettings.FooterImageKey = ""
			Me.radGroupSettings.FooterText = ""
			Me.radGroupSettings.HeaderImageIndex = -1
			Me.radGroupSettings.HeaderImageKey = ""
			Me.radGroupSettings.HeaderMargin = New Padding(0)
			Me.radGroupSettings.HeaderText = " Settings "
			Me.radGroupSettings.Location = New Point(10, 5)
			Me.radGroupSettings.Name = "radGroupSettings"
			Me.radGroupSettings.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupSettings.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupSettings.Size = New Size(180, 203)
			Me.radGroupSettings.TabIndex = 0
			Me.radGroupSettings.Text = " Settings "
			' 
			' Form1
			' 
			Me.Controls.Add(Me.radStartStopButton)
			Me.Controls.Add(Me.currentFrameTextBox)
			Me.Controls.Add(Me.radButtonNext)
			Me.Controls.Add(Me.radButtonPrevious)
			Me.Controls.Add(Me.radRotator1)
			Me.Name = "Form1"
			Me.Size = New Size(1224, 820)
			Me.Controls.SetChildIndex(Me.radRotator1, 0)
			Me.Controls.SetChildIndex(Me.radButtonPrevious, 0)
			Me.Controls.SetChildIndex(Me.radButtonNext, 0)
			Me.Controls.SetChildIndex(Me.currentFrameTextBox, 0)
			Me.Controls.SetChildIndex(Me.radStartStopButton, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			CType(Me.radRotator1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPrevious, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonNext, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radStartStopButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.currentFrameTextBox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelInterval, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.intervalTextBox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxOpacity, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.locationAnimationTextBox, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelMsec, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelLocationAnimation, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonApply, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupSettings, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupSettings.ResumeLayout(False)
			Me.radGroupSettings.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private radRotator1 As Telerik.WinControls.UI.RadRotator
		Private radItemsContainer1 As Telerik.WinControls.UI.RadItemsContainer
		Private radImageItem1 As Telerik.WinControls.UI.RadImageItem
		Private radItemsContainer2 As Telerik.WinControls.UI.RadItemsContainer
		Private radButtonElement1 As Telerik.WinControls.UI.RadButtonElement
		Private radLabelElement1 As Telerik.WinControls.UI.RadLabelElement
		Private radTextBoxElement1 As Telerik.WinControls.UI.RadTextBoxElement
		Private radButtonPrevious As Telerik.WinControls.UI.RadButton
		Private radButtonNext As Telerik.WinControls.UI.RadButton
		Private radStartStopButton As Telerik.WinControls.UI.RadButton
		Private currentFrameTextBox As Telerik.WinControls.UI.RadTextBox
		Private radLabelInterval As Telerik.WinControls.UI.RadLabel
		Private intervalTextBox As Telerik.WinControls.UI.RadTextBox
		Private radCheckBoxOpacity As Telerik.WinControls.UI.RadCheckBox
		Private locationAnimationTextBox As Telerik.WinControls.UI.RadTextBox
		Private radLabelMsec As Telerik.WinControls.UI.RadLabel
		Private radLabelLocationAnimation As Telerik.WinControls.UI.RadLabel
		Private radButtonApply As Telerik.WinControls.UI.RadButton
		Private radWebBrowserItem1 As Telerik.WinControls.UI.RadWebBrowserItem
		Private radGroupSettings As Telerik.WinControls.UI.RadGroupBox
	End Class
End Namespace