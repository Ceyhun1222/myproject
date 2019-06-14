Namespace Telerik.Examples.WinControls.Docking.Serialization
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
			Me.groupBox1 = New GroupBox()
			Me.addRadDropDownButton = New Telerik.WinControls.UI.RadDropDownButton()
			Me.radMenuItem1 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem2 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem3 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem4 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem1 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem5 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuSeparatorItem2 = New Telerik.WinControls.UI.RadMenuSeparatorItem()
			Me.radMenuItem7 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem8 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem9 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem10 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radMenuItem11 = New Telerik.WinControls.UI.RadMenuItem()
			Me.AddRandomRadButton = New Telerik.WinControls.UI.RadButton()
			Me.removeAllRadButton = New Telerik.WinControls.UI.RadButton()
			Me.saveRadButton = New Telerik.WinControls.UI.RadButton()
			Me.loadRadButton = New Telerik.WinControls.UI.RadButton()
			Me.logTextBox = New TextBox()
			Me.radMenuItem6 = New Telerik.WinControls.UI.RadMenuItem()
			Me.radDock1 = New Telerik.WinControls.UI.Docking.RadDock()
			Me.documentContainer1 = New Telerik.WinControls.UI.Docking.DocumentContainer()
			Me.groupBox1.SuspendLayout()
			CType(Me.addRadDropDownButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.AddRandomRadButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.removeAllRadButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.saveRadButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.loadRadButton, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDock1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radDock1.SuspendLayout()
			CType(Me.documentContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.addRadDropDownButton)
			Me.groupBox1.Controls.Add(Me.AddRandomRadButton)
			Me.groupBox1.Controls.Add(Me.removeAllRadButton)
			Me.groupBox1.Controls.Add(Me.saveRadButton)
			Me.groupBox1.Controls.Add(Me.loadRadButton)
			Me.groupBox1.Controls.Add(Me.logTextBox)
			Me.groupBox1.Dock = DockStyle.Bottom
			Me.groupBox1.Location = New Point(0, 369)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New Size(616, 131)
			Me.groupBox1.TabIndex = 5
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Settings"
			' 
			' addRadDropDownButton
			' 
			Me.addRadDropDownButton.Image = My.Resources.BindingNavigatorAddNewItem_Image1
			Me.addRadDropDownButton.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem1, Me.radMenuItem2, Me.radMenuItem3, Me.radMenuItem4, Me.radMenuSeparatorItem1, Me.radMenuItem5, Me.radMenuSeparatorItem2, Me.radMenuItem7})
			Me.addRadDropDownButton.Location = New Point(152, 19)
			Me.addRadDropDownButton.Name = "addRadDropDownButton"
			Me.addRadDropDownButton.Size = New Size(162, 30)
			Me.addRadDropDownButton.TabIndex = 9
			Me.addRadDropDownButton.Text = "Add Window"
			Me.addRadDropDownButton.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radMenuItem1
			' 
			Me.radMenuItem1.Name = "radMenuItem1"
			Me.radMenuItem1.Text = "Left"
			Me.radMenuItem1.TextImageRelation = TextImageRelation.ImageBeforeText

			' 
			' radMenuItem2
			' 
			Me.radMenuItem2.Name = "radMenuItem2"
			Me.radMenuItem2.Text = "Right"
			Me.radMenuItem2.TextImageRelation = TextImageRelation.ImageBeforeText

			' 
			' radMenuItem3
			' 
			Me.radMenuItem3.Name = "radMenuItem3"
			Me.radMenuItem3.Text = "Top"
			Me.radMenuItem3.TextImageRelation = TextImageRelation.ImageBeforeText

			' 
			' radMenuItem4
			' 
			Me.radMenuItem4.Name = "radMenuItem4"
			Me.radMenuItem4.Text = "Bottom"
			Me.radMenuItem4.TextImageRelation = TextImageRelation.ImageBeforeText

			' 
			' radMenuSeparatorItem1
			' 
			Me.radMenuSeparatorItem1.AutoSize = False
			Me.radMenuSeparatorItem1.Bounds = New Rectangle(0, 88, 81, 1)
			Me.radMenuSeparatorItem1.Class = "RadMenuItem"
			Me.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1"
			Me.radMenuSeparatorItem1.PositionOffset = New SizeF(26F, 0F)
			Me.radMenuSeparatorItem1.Text = "New item"
			' 
			' radMenuItem5
			' 
			Me.radMenuItem5.Name = "radMenuItem5"
			Me.radMenuItem5.Text = "Floating"
			Me.radMenuItem5.TextImageRelation = TextImageRelation.ImageBeforeText

			' 
			' radMenuSeparatorItem2
			' 
			Me.radMenuSeparatorItem2.AutoSize = False
			Me.radMenuSeparatorItem2.Bounds = New Rectangle(0, 111, 81, 1)
			Me.radMenuSeparatorItem2.Class = "RadMenuItem"
			Me.radMenuSeparatorItem2.Name = "radMenuSeparatorItem2"
			Me.radMenuSeparatorItem2.PositionOffset = New SizeF(26F, 0F)
			Me.radMenuSeparatorItem2.Text = "New item"
			' 
			' radMenuItem7
			' 
			Me.radMenuItem7.Items.AddRange(New Telerik.WinControls.RadItem() { Me.radMenuItem8, Me.radMenuItem9, Me.radMenuItem10, Me.radMenuItem11})
			Me.radMenuItem7.Name = "radMenuItem7"
			Me.radMenuItem7.Text = "AutoHide"
			Me.radMenuItem7.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radMenuItem8
			' 
			Me.radMenuItem8.Name = "radMenuItem8"
			Me.radMenuItem8.Text = "Left"
			Me.radMenuItem8.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radMenuItem9
			' 
			Me.radMenuItem9.Name = "radMenuItem9"
			Me.radMenuItem9.Text = "Right"
			Me.radMenuItem9.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radMenuItem10
			' 
			Me.radMenuItem10.Name = "radMenuItem10"
			Me.radMenuItem10.Text = "Top"
			Me.radMenuItem10.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radMenuItem11
			' 
			Me.radMenuItem11.Name = "radMenuItem11"
			Me.radMenuItem11.Text = "Bottom"
			Me.radMenuItem11.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' AddRandomRadButton
			' 
			Me.AddRandomRadButton.Image = My.Resources.Plus11
			Me.AddRandomRadButton.Location = New Point(152, 55)
			Me.AddRandomRadButton.Name = "AddRandomRadButton"
			Me.AddRandomRadButton.Size = New Size(162, 30)
			Me.AddRandomRadButton.TabIndex = 8
			Me.AddRandomRadButton.Text = "Add Random Window"
			Me.AddRandomRadButton.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' removeAllRadButton
			' 
			Me.removeAllRadButton.Image = My.Resources.BindingNavigatorDeleteItem_Image1
			Me.removeAllRadButton.Location = New Point(152, 91)
			Me.removeAllRadButton.Name = "removeAllRadButton"
			Me.removeAllRadButton.Size = New Size(162, 30)
			Me.removeAllRadButton.TabIndex = 7
			Me.removeAllRadButton.Text = "Remove All Windows"
			Me.removeAllRadButton.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' saveRadButton
			' 
			Me.saveRadButton.Image = My.Resources.RibbonMenuSaveMagenta
			Me.saveRadButton.Location = New Point(12, 73)
			Me.saveRadButton.Name = "saveRadButton"
			Me.saveRadButton.Size = New Size(118, 49)
			Me.saveRadButton.TabIndex = 6
			Me.saveRadButton.Text = "Save..."
            ' 
			' loadRadButton
			' 
			Me.loadRadButton.Image = My.Resources.RibbonMenuOpenMagenta
			Me.loadRadButton.Location = New Point(12, 19)
			Me.loadRadButton.Name = "loadRadButton"
			Me.loadRadButton.Size = New Size(118, 49)
			Me.loadRadButton.TabIndex = 5
			Me.loadRadButton.Text = "Load..."
            ' 
			' logTextBox
			' 
			Me.logTextBox.Anchor = (CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.logTextBox.BackColor = SystemColors.Window
			Me.logTextBox.Location = New Point(334, 18)
			Me.logTextBox.Multiline = True
			Me.logTextBox.Name = "logTextBox"
			Me.logTextBox.ReadOnly = True
			Me.logTextBox.ScrollBars = ScrollBars.Vertical
			Me.logTextBox.Size = New Size(270, 103)
			Me.logTextBox.TabIndex = 2
			Me.logTextBox.WordWrap = False
			' 
			' radMenuItem6
			' 
			Me.radMenuItem6.Name = "radMenuItem6"
			Me.radMenuItem6.Text = "New item"
			Me.radMenuItem6.TextImageRelation = TextImageRelation.ImageBeforeText
			' 
			' radDock1
			' 
			Me.radDock1.Controls.Add(Me.documentContainer1)
			Me.radDock1.Dock = DockStyle.Fill
			Me.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront
			Me.radDock1.Location = New Point(0, 0)
			Me.radDock1.MainDocumentContainer = Me.documentContainer1
			Me.radDock1.Name = "radDock1"
			' 
			' 
			' 
			Me.radDock1.RootElement.MinSize = New Size(25, 25)
			Me.radDock1.Size = New Size(616, 369)
			Me.radDock1.TabIndex = 3
			Me.radDock1.TabStop = False
			Me.radDock1.Text = "radDock1"

			' 
			' documentContainer1
			' 
			Me.documentContainer1.Location = New Point(0, 0)
			Me.documentContainer1.Name = "documentContainer1"
			' 
			' 
			' 
			Me.documentContainer1.RootElement.MinSize = New Size(25, 25)
			Me.documentContainer1.Size = New Size(616, 369)
			Me.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill
			Me.documentContainer1.TabIndex = 0
			Me.documentContainer1.TabStop = False
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.ClientSize = New Size(616, 500)
			Me.Controls.Add(Me.radDock1)
			Me.Controls.Add(Me.groupBox1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			CType(Me.addRadDropDownButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.AddRandomRadButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.removeAllRadButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.saveRadButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.loadRadButton, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDock1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radDock1.ResumeLayout(False)
			CType(Me.documentContainer1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private groupBox1 As GroupBox
		Private saveRadButton As Telerik.WinControls.UI.RadButton
		Private loadRadButton As Telerik.WinControls.UI.RadButton
		Private logTextBox As TextBox
		Private addRadDropDownButton As Telerik.WinControls.UI.RadDropDownButton
		Private AddRandomRadButton As Telerik.WinControls.UI.RadButton
		Private removeAllRadButton As Telerik.WinControls.UI.RadButton
		Private radMenuItem1 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem2 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem3 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem4 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuSeparatorItem1 As Telerik.WinControls.UI.RadMenuSeparatorItem
		Private radMenuItem5 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuSeparatorItem2 As Telerik.WinControls.UI.RadMenuSeparatorItem
		Private radMenuItem7 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem8 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem9 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem10 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem11 As Telerik.WinControls.UI.RadMenuItem
		Private radMenuItem6 As Telerik.WinControls.UI.RadMenuItem
		Private radDock1 As Telerik.WinControls.UI.Docking.RadDock
		Private documentContainer1 As Telerik.WinControls.UI.Docking.DocumentContainer
	End Class
End Namespace