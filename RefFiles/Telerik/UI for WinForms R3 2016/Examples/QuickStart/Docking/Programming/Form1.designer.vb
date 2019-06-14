Namespace Telerik.Examples.WinControls.Docking.Programming
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
			Me.radDock1 = New Telerik.WinControls.UI.Docking.RadDock()
			Me.toolWindow2 = New Telerik.WinControls.UI.Docking.ToolWindow()
			Me.toolTabStrip1 = New Telerik.WinControls.UI.Docking.ToolTabStrip()
			Me.toolWindow1 = New Telerik.WinControls.UI.Docking.ToolWindow()
			Me.radSplitContainer1 = New Telerik.WinControls.UI.RadSplitContainer()
			Me.toolTabStrip3 = New Telerik.WinControls.UI.Docking.ToolTabStrip()
			Me.documentContainer2 = New Telerik.WinControls.UI.Docking.DocumentContainer()
			Me.documentTabStrip1 = New Telerik.WinControls.UI.Docking.DocumentTabStrip()
			Me.documentWindow1 = New Telerik.WinControls.UI.Docking.DocumentWindow()
			Me.textBox1 = New TextBox()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radComboBox2 = New Telerik.WinControls.UI.RadDropDownList()
			Me.label3 = New Label()
			Me.label1 = New Label()
			Me.radComboBox1 = New Telerik.WinControls.UI.RadDropDownList()
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			Me.label2 = New Label()
			CType(Me.radDock1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radDock1.SuspendLayout()
			CType(Me.toolTabStrip1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.toolTabStrip1.SuspendLayout()
			CType(Me.radSplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radSplitContainer1.SuspendLayout()
			CType(Me.toolTabStrip3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.toolTabStrip3.SuspendLayout()
			CType(Me.documentContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.documentContainer2.SuspendLayout()
			CType(Me.documentTabStrip1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.documentTabStrip1.SuspendLayout()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			CType(Me.radComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radDock1
			' 
			Me.radDock1.ActiveWindow = Me.toolWindow2
			Me.radDock1.Controls.Add(Me.toolTabStrip1)
			Me.radDock1.Controls.Add(Me.radSplitContainer1)
			Me.radDock1.Cursor = Cursors.Arrow
			Me.radDock1.Dock = DockStyle.Fill
			Me.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront
			Me.radDock1.Location = New Point(0, 0)
			Me.radDock1.MainDocumentContainer = Me.documentContainer2
			Me.radDock1.Name = "radDock1"
			Me.radDock1.Orientation = Orientation.Horizontal
			' 
			' 
			' 
			Me.radDock1.RootElement.MinSize = New Size(25, 25)
			Me.radDock1.Size = New Size(731, 544)
			Me.radDock1.TabIndex = 4
			Me.radDock1.TabStop = False
			' 
			' toolWindow2
			' 
			Me.toolWindow2.Location = New Point(1, 26)
			Me.toolWindow2.Name = "toolWindow2"
			Me.toolWindow2.Size = New Size(198, 314)
			Me.toolWindow2.Text = "Tool Window 2"
			' 
			' toolTabStrip1
			' 
			Me.toolTabStrip1.Controls.Add(Me.toolWindow1)
			Me.toolTabStrip1.Location = New Point(0, 0)
			Me.toolTabStrip1.Name = "toolTabStrip1"
			' 
			' 
			' 
			Me.toolTabStrip1.RootElement.MinSize = New Size(25, 25)
			Me.toolTabStrip1.SelectedIndex = 0
			Me.toolTabStrip1.Size = New Size(731, 200)
			Me.toolTabStrip1.TabIndex = 1
			Me.toolTabStrip1.TabStop = False
			' 
			' toolWindow1
			' 
			Me.toolWindow1.Location = New Point(1, 26)
			Me.toolWindow1.Name = "toolWindow1"
			Me.toolWindow1.Size = New Size(729, 173)
			Me.toolWindow1.Text = "Tool Window 1"
			' 
			' radSplitContainer1
			' 
			Me.radSplitContainer1.Controls.Add(Me.toolTabStrip3)
			Me.radSplitContainer1.Controls.Add(Me.documentContainer2)
			Me.radSplitContainer1.Location = New Point(0, 203)
			Me.radSplitContainer1.Name = "radSplitContainer1"
			' 
			' 
			' 
			Me.radSplitContainer1.RootElement.MinSize = New Size(25, 25)
			Me.radSplitContainer1.Size = New Size(731, 341)
			Me.radSplitContainer1.TabIndex = 0
			Me.radSplitContainer1.TabStop = False
			' 
			' toolTabStrip3
			' 
			Me.toolTabStrip3.Controls.Add(Me.toolWindow2)
			Me.toolTabStrip3.Location = New Point(0, 0)
			Me.toolTabStrip3.Name = "toolTabStrip3"
			' 
			' 
			' 
			Me.toolTabStrip3.RootElement.MinSize = New Size(25, 25)
			Me.toolTabStrip3.SelectedIndex = 0
			Me.toolTabStrip3.Size = New Size(200, 341)
			Me.toolTabStrip3.SizeInfo.AutoSizeScale = New SizeF(-0.02554595F, 0F)
			Me.toolTabStrip3.SizeInfo.SplitterCorrection = New Size(-1190, 0)
			Me.toolTabStrip3.TabIndex = 3
			Me.toolTabStrip3.TabStop = False
			' 
			' documentContainer2
			' 
			Me.documentContainer2.Controls.Add(Me.documentTabStrip1)
			Me.documentContainer2.Location = New Point(203, 0)
			Me.documentContainer2.Name = "documentContainer2"
			' 
			' 
			' 
			Me.documentContainer2.RootElement.MinSize = New Size(25, 25)
			Me.documentContainer2.Size = New Size(528, 341)
			Me.documentContainer2.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill
			Me.documentContainer2.TabIndex = 0
			Me.documentContainer2.TabStop = False
			' 
			' documentTabStrip1
			' 
			Me.documentTabStrip1.Controls.Add(Me.documentWindow1)
			Me.documentTabStrip1.Location = New Point(0, 0)
			Me.documentTabStrip1.Name = "documentTabStrip1"
			' 
			' 
			' 
			Me.documentTabStrip1.RootElement.MinSize = New Size(25, 25)
			Me.documentTabStrip1.SelectedIndex = 0
			Me.documentTabStrip1.Size = New Size(528, 341)
			Me.documentTabStrip1.TabIndex = 0
			Me.documentTabStrip1.TabStop = False
			' 
			' documentWindow1
			' 
			Me.documentWindow1.Location = New Point(5, 27)
			Me.documentWindow1.Name = "documentWindow1"
			Me.documentWindow1.Size = New Size(518, 309)
			Me.documentWindow1.Text = "documentWindow1"
			' 
			' textBox1
			' 
			Me.textBox1.BackColor = SystemColors.Window
			Me.textBox1.Dock = DockStyle.Fill
			Me.textBox1.Location = New Point(4, 110)
			Me.textBox1.Multiline = True
			Me.textBox1.Name = "textBox1"
			Me.textBox1.ReadOnly = True
			Me.textBox1.ScrollBars = ScrollBars.Both
			Me.textBox1.Size = New Size(235, 430)
			Me.textBox1.TabIndex = 7
			Me.textBox1.WordWrap = False
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.Controls.Add(Me.radComboBox2)
			Me.radGroupBox1.Controls.Add(Me.label3)
			Me.radGroupBox1.Controls.Add(Me.label1)
			Me.radGroupBox1.Controls.Add(Me.radComboBox1)
			Me.radGroupBox1.Controls.Add(Me.radButton1)
			Me.radGroupBox1.Controls.Add(Me.label2)
			Me.radGroupBox1.Controls.Add(Me.textBox1)
			Me.radGroupBox1.Dock = DockStyle.Right
			Me.radGroupBox1.FooterImageIndex = -1
			Me.radGroupBox1.FooterImageKey = ""
			Me.radGroupBox1.HeaderImageIndex = -1
			Me.radGroupBox1.HeaderImageKey = ""
			Me.radGroupBox1.HeaderMargin = New Padding(0)
			Me.radGroupBox1.HeaderText = "Settings"
			Me.radGroupBox1.Location = New Point(731, 0)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Padding = New Padding(4, 110, 4, 4)
			' 
			' 
			' 
			Me.radGroupBox1.RootElement.Padding = New Padding(4, 110, 4, 4)
			Me.radGroupBox1.Size = New Size(243, 544)
			Me.radGroupBox1.TabIndex = 8
			Me.radGroupBox1.Text = "Settings"
			' 
			' radComboBox2
			' 
			Me.radComboBox2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radComboBox2.Location = New Point(85, 53)
			Me.radComboBox2.Name = "radComboBox2"
			' 
			' 
			' 
			Me.radComboBox2.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboBox2.Size = New Size(154, 20)
			Me.radComboBox2.TabIndex = 20
			Me.radComboBox2.TabStop = False
			Me.radComboBox2.Text = "radComboBox2"

			' 
			' label3
			' 
			Me.label3.AutoSize = True
			Me.label3.BackColor = Color.Transparent
			Me.label3.Location = New Point(1, 53)
			Me.label3.Name = "label3"
			Me.label3.Size = New Size(64, 13)
			Me.label3.TabIndex = 19
			Me.label3.Text = "Dock State:"
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.BackColor = Color.Transparent
			Me.label1.Location = New Point(1, 28)
			Me.label1.Name = "label1"
			Me.label1.Size = New Size(78, 13)
			Me.label1.TabIndex = 18
			Me.label1.Text = "Dock Window:"
			' 
			' radComboBox1
			' 
			Me.radComboBox1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radComboBox1.Location = New Point(85, 28)
			Me.radComboBox1.Name = "radComboBox1"
			' 
			' 
			' 
			Me.radComboBox1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radComboBox1.Size = New Size(154, 20)
			Me.radComboBox1.TabIndex = 17
			Me.radComboBox1.TabStop = False
			Me.radComboBox1.Text = "radComboBox1"

			' 
			' radButton1
			' 
			Me.radButton1.Anchor = (CType((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles))
			Me.radButton1.Location = New Point(164, 81)
			Me.radButton1.Name = "radButton1"
			Me.radButton1.Size = New Size(75, 23)
			Me.radButton1.TabIndex = 16
			Me.radButton1.Text = "Clear"

			' 
			' label2
			' 
			Me.label2.AutoSize = True
			Me.label2.BackColor = Color.Transparent
			Me.label2.Location = New Point(1, 81)
			Me.label2.Name = "label2"
			Me.label2.Size = New Size(59, 13)
			Me.label2.TabIndex = 15
			Me.label2.Text = "Event Log:"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.ClientSize = New Size(974, 544)
			Me.Controls.Add(Me.radDock1)
			Me.Controls.Add(Me.radGroupBox1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			CType(Me.radDock1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radDock1.ResumeLayout(False)
			CType(Me.toolTabStrip1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.toolTabStrip1.ResumeLayout(False)
			CType(Me.radSplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radSplitContainer1.ResumeLayout(False)
			CType(Me.toolTabStrip3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.toolTabStrip3.ResumeLayout(False)
			CType(Me.documentContainer2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.documentContainer2.ResumeLayout(False)
			CType(Me.documentTabStrip1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.documentTabStrip1.ResumeLayout(False)
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			CType(Me.radComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radDock1 As Telerik.WinControls.UI.Docking.RadDock
		Private toolWindow2 As Telerik.WinControls.UI.Docking.ToolWindow
		Private toolTabStrip3 As Telerik.WinControls.UI.Docking.ToolTabStrip
		Private textBox1 As TextBox
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private label2 As Label
		Private documentContainer2 As Telerik.WinControls.UI.Docking.DocumentContainer
		Private label1 As Label
		Private radComboBox1 As Telerik.WinControls.UI.RadDropDownList
		Private radButton1 As Telerik.WinControls.UI.RadButton
		Private label3 As Label
		Private radComboBox2 As Telerik.WinControls.UI.RadDropDownList
		Private toolTabStrip1 As Telerik.WinControls.UI.Docking.ToolTabStrip
		Private toolWindow1 As Telerik.WinControls.UI.Docking.ToolWindow
		Private radSplitContainer1 As Telerik.WinControls.UI.RadSplitContainer
		Private documentWindow1 As Telerik.WinControls.UI.Docking.DocumentWindow
		Private documentTabStrip1 As Telerik.WinControls.UI.Docking.DocumentTabStrip

	End Class
End Namespace