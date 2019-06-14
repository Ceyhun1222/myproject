Namespace Telerik.Examples.WinControls.Buttons.SplitButton
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
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			Me.radSplitButton3 = New Telerik.WinControls.UI.RadSplitButton()
			Me.imageList1 = New ImageList(Me.components)
			Me.radMenuItem1 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem2 = New Telerik.WinControls.UI.RadMenuItem()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSplitButton3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radSplitButton3)
			Me.radPanelDemoHolder.Controls.Add(Me.radButton1)
			Me.radPanelDemoHolder.ForeColor = Color.Black
            Me.radPanelDemoHolder.Size = New Size(700, 400)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(200, 735)
			' 
			' radButton1
			' 
			Me.radButton1.DisplayStyle = Telerik.WinControls.DisplayStyle.Image
			Me.radButton1.Location = New Point(231, 0)
			Me.radButton1.Name = "radButton1"
			' 
			' 
			' 
			Me.radButton1.RootElement.StretchHorizontally = False
			Me.radButton1.RootElement.StretchVertically = False
			Me.radButton1.Size = New Size(171, 62)
			Me.radButton1.TabIndex = 8
			Me.radButton1.Text = "radButton2"
			' 
			' radSplitButton3
			' 
			Me.radSplitButton3.Font = New Font("Microsoft Sans Serif", 8.25F)
			Me.radSplitButton3.ImageList = Me.imageList1
			Me.radSplitButton3.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem1, Me.radMenuItem2})
			Me.radSplitButton3.Location = New Point(0, 0)
			Me.radSplitButton3.Name = "radSplitButton3"
			' 
			' 
			' 
			Me.radSplitButton3.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radSplitButton3.Size = New Size(161, 48)
			Me.radSplitButton3.TabIndex = 6
			Me.radSplitButton3.Text = "Split Button"
			Me.radSplitButton3.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' imageList1
			' 
			Me.imageList1.ImageStream = (CType(resources.GetObject("imageList1.ImageStream"), ImageListStreamer))
			Me.imageList1.TransparentColor = Color.Lime
			Me.imageList1.Images.SetKeyName(0, "2_redo.gif")
			Me.imageList1.Images.SetKeyName(1, "1_undo.gif")
			' 
			' radMenuItem1
			' 
			Me.radMenuItem1.AccessibleDescription = "Rotate" & vbCrLf & "Clockwise"
			Me.radMenuItem1.AccessibleName = "Rotate" & vbCrLf & "Clockwise"
			Me.radMenuItem1.ImageIndex = 0
			Me.radMenuItem1.Name = "radMenuItem1"
			Me.radMenuItem1.Text = "Rotate" & vbCrLf & "Clockwise"
			Me.radMenuItem1.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' radMenuItem2
			' 
			Me.radMenuItem2.AccessibleDescription = "Rotate" & vbCrLf & "Counter-clockwise"
			Me.radMenuItem2.AccessibleName = "Rotate" & vbCrLf & "Counter-clockwise"
			Me.radMenuItem2.ImageIndex = 1
			Me.radMenuItem2.Name = "radMenuItem2"
			Me.radMenuItem2.Text = "Rotate" & vbCrLf & "Counter-clockwise"
			Me.radMenuItem2.TextImageRelation = TextImageRelation.ImageBeforeText
			Me.radMenuItem2.Visibility = Telerik.WinControls.ElementVisibility.Visible

			' 
			' Form1
			' 
			Me.Name = "Form1"
			Me.Padding = New Padding(2, 35, 2, 4)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSplitButton3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radSplitButton3 As Telerik.WinControls.UI.RadSplitButton
		Private radMenuItem1 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem2 As Telerik.WinControls.UI.RadMenuItem
		Private imageList1 As ImageList
		Private radButton1 As Telerik.WinControls.UI.RadButton
	End Class
End Namespace