Namespace Telerik.Examples.WinControls.Buttons.ToggleButton
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Me.radToggleButton1 = New Telerik.WinControls.UI.RadToggleButton()
			Me.radToggleButton2 = New Telerik.WinControls.UI.RadToggleButton()
			Me.radToggleButton3 = New Telerik.WinControls.UI.RadToggleButton()
			Me.radToggleButton6 = New Telerik.WinControls.UI.RadToggleButton()
			Me.imageList1 = New ImageList(Me.components)
			Me.label1 = New Label()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radToggleButton2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radToggleButton3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radToggleButton6, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radToggleButton6)
			Me.radPanelDemoHolder.Controls.Add(Me.label1)
			Me.radPanelDemoHolder.ForeColor = Color.Black
			Me.radPanelDemoHolder.Size = New Size(254, 136)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(973, 1)
			Me.settingsPanel.Size = New Size(250, 534)
			' 
			' radToggleButton1
			' 
			Me.radToggleButton1.Location = New Point(42, 38)
			Me.radToggleButton1.Name = "radToggleButton1"
			Me.radToggleButton1.Size = New Size(96, 16)
			Me.radToggleButton1.TabIndex = 0
			Me.radToggleButton1.Text = "radToggleButton1"
			Me.radToggleButton1.TextAlignment = ContentAlignment.MiddleLeft
			' 
			' radToggleButton2
			' 
			Me.radToggleButton2.Font = New Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radToggleButton2.Location = New Point(42, 79)
			Me.radToggleButton2.Name = "radToggleButton2"
			Me.radToggleButton2.Size = New Size(113, 19)
			Me.radToggleButton2.TabIndex = 1
			Me.radToggleButton2.Text = "radToggleButton2"
			Me.radToggleButton2.TextAlignment = ContentAlignment.MiddleLeft
			' 
			' radToggleButton3
			' 
			Me.radToggleButton3.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radToggleButton3.Location = New Point(42, 120)
			Me.radToggleButton3.Name = "radToggleButton3"
			Me.radToggleButton3.Size = New Size(139, 23)
			Me.radToggleButton3.TabIndex = 1
			Me.radToggleButton3.Text = "radToggleButton3"
			Me.radToggleButton3.TextAlignment = ContentAlignment.MiddleLeft
			' 
			' radToggleButton6
			' 
			Me.radToggleButton6.Font = New Font("Microsoft Sans Serif", 8.25F)
			Me.radToggleButton6.ImageAlignment = ContentAlignment.MiddleCenter
			Me.radToggleButton6.ImageIndex = 3
			Me.radToggleButton6.ImageList = Me.imageList1
			Me.radToggleButton6.Location = New Point(0, 1)
			Me.radToggleButton6.Name = "radToggleButton6"
			Me.radToggleButton6.Size = New Size(125, 23)
			Me.radToggleButton6.TabIndex = 2
			Me.radToggleButton6.Text = "Toggle Button"
			Me.radToggleButton6.TextAlignment = ContentAlignment.MiddleLeft
			Me.radToggleButton6.TextImageRelation = TextImageRelation.ImageBeforeText

			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).Image = (CType(resources.GetObject("resource.Image"), Image))
			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).ImageIndex = 3
			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).TextImageRelation = TextImageRelation.ImageBeforeText
			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).ImageAlignment = ContentAlignment.MiddleCenter
			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).TextAlignment = ContentAlignment.MiddleLeft
			CType(Me.radToggleButton6.GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Toggle Button"
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Fuchsia
			Me.imageList1.Images.SetKeyName(0, "print.gif")
			Me.imageList1.Images.SetKeyName(1, "bulb_on.GIF")
			Me.imageList1.Images.SetKeyName(2, "bulb_off.GIF")
			Me.imageList1.Images.SetKeyName(3, "iconToggleOff.bmp")
			Me.imageList1.Images.SetKeyName(4, "iconToggleOn.bmp")
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, (CByte(204)))
			Me.label1.ForeColor = Color.FromArgb((CInt(Fix((CByte(64))))), (CInt(Fix((CByte(64))))), (CInt(Fix((CByte(64))))))
			Me.label1.Location = New Point(-3, 48)
			Me.label1.Name = "label1"
			Me.label1.Size = New Size(128, 17)
			Me.label1.TabIndex = 3
			Me.label1.Text = "Button Toggled Off"
			' 
			' Form1
			' 
			Me.Name = "Form1"
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radToggleButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radToggleButton2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radToggleButton3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radToggleButton6, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radToggleButton1 As Telerik.WinControls.UI.RadToggleButton
		Private radToggleButton2 As Telerik.WinControls.UI.RadToggleButton
		Private radToggleButton3 As Telerik.WinControls.UI.RadToggleButton
		Private radToggleButton6 As Telerik.WinControls.UI.RadToggleButton
		Private imageList1 As ImageList
		Private label1 As Label
	End Class
End Namespace