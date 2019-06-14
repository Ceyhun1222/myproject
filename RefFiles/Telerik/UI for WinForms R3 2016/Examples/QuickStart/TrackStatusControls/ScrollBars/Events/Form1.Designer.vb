Namespace Telerik.Examples.WinControls.TrackStatusControls.ScrollBars.Events
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
			Me.contextMenuStrip1 = New ContextMenuStrip(Me.components)
			Me.scrollToToolStripMenuItem = New ToolStripMenuItem()
			Me.toolStripSeparator1 = New ToolStripSeparator()
			Me.leftTopToolStripMenuItem = New ToolStripMenuItem()
			Me.rightBottomToolStripMenuItem = New ToolStripMenuItem()
			Me.toolStripSeparator2 = New ToolStripSeparator()
			Me.pageLeftUpToolStripMenuItem = New ToolStripMenuItem()
			Me.pageRightBottomToolStripMenuItem = New ToolStripMenuItem()
			Me.toolStripSeparator3 = New ToolStripSeparator()
			Me.scrollLeftUpToolStripMenuItem = New ToolStripMenuItem()
			Me.scrollRightBottomToolStripMenuItem = New ToolStripMenuItem()
			Me.radVScrollBar1 = New Telerik.WinControls.UI.RadVScrollBar()
			Me.radHScrollBar1 = New Telerik.WinControls.UI.RadHScrollBar()
			Me.radPanelDemo = New Telerik.WinControls.UI.RadPanel()
			Me.radBtnClear = New Telerik.WinControls.UI.RadButton()
			Me.radTxtEvents = New Telerik.WinControls.UI.RadTextBox()
			Me.radLblEvents = New Telerik.WinControls.UI.RadLabel()
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radPanelDemoHolder.SuspendLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.contextMenuStrip1.SuspendLayout()
			CType(Me.radVScrollBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radHScrollBar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radPanelDemo, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radBtnClear, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTxtEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLblEvents, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' radPanelDemoHolder
			' 
			Me.radPanelDemoHolder.Controls.Add(Me.radPanelDemo)
			Me.radPanelDemoHolder.Controls.Add(Me.radHScrollBar1)
			Me.radPanelDemoHolder.Controls.Add(Me.radTxtEvents)
			Me.radPanelDemoHolder.Controls.Add(Me.radLblEvents)
			Me.radPanelDemoHolder.Controls.Add(Me.radBtnClear)
			Me.radPanelDemoHolder.Controls.Add(Me.radVScrollBar1)
			Me.radPanelDemoHolder.ForeColor = Color.Black
			Me.radPanelDemoHolder.Size = New Size(634, 584)
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(1023, 1)
			Me.settingsPanel.Size = New Size(200, 750)
			' 
			' contextMenuStrip1
			' 
			Me.contextMenuStrip1.Items.AddRange(New ToolStripItem() { Me.scrollToToolStripMenuItem, Me.toolStripSeparator1, Me.leftTopToolStripMenuItem, Me.rightBottomToolStripMenuItem, Me.toolStripSeparator2, Me.pageLeftUpToolStripMenuItem, Me.pageRightBottomToolStripMenuItem, Me.toolStripSeparator3, Me.scrollLeftUpToolStripMenuItem, Me.scrollRightBottomToolStripMenuItem})
			Me.contextMenuStrip1.Name = "contextMenuStrip1"
			Me.contextMenuStrip1.Size = New Size(177, 176)
			' 
			' scrollToToolStripMenuItem
			' 
			Me.scrollToToolStripMenuItem.Name = "scrollToToolStripMenuItem"
			Me.scrollToToolStripMenuItem.Size = New Size(176, 22)
			Me.scrollToToolStripMenuItem.Text = "Scroll To"

			' 
			' toolStripSeparator1
			' 
			Me.toolStripSeparator1.Name = "toolStripSeparator1"
			Me.toolStripSeparator1.Size = New Size(173, 6)
			' 
			' leftTopToolStripMenuItem
			' 
			Me.leftTopToolStripMenuItem.Name = "leftTopToolStripMenuItem"
			Me.leftTopToolStripMenuItem.Size = New Size(176, 22)
			Me.leftTopToolStripMenuItem.Text = "Left / Top"

			' 
			' rightBottomToolStripMenuItem
			' 
			Me.rightBottomToolStripMenuItem.Name = "rightBottomToolStripMenuItem"
			Me.rightBottomToolStripMenuItem.Size = New Size(176, 22)
			Me.rightBottomToolStripMenuItem.Text = "Right  /Bottom"

			' 
			' toolStripSeparator2
			' 
			Me.toolStripSeparator2.Name = "toolStripSeparator2"
			Me.toolStripSeparator2.Size = New Size(173, 6)
			' 
			' pageLeftUpToolStripMenuItem
			' 
			Me.pageLeftUpToolStripMenuItem.Name = "pageLeftUpToolStripMenuItem"
			Me.pageLeftUpToolStripMenuItem.Size = New Size(176, 22)
			Me.pageLeftUpToolStripMenuItem.Text = "Page Left / Up"

			' 
			' pageRightBottomToolStripMenuItem
			' 
			Me.pageRightBottomToolStripMenuItem.Name = "pageRightBottomToolStripMenuItem"
			Me.pageRightBottomToolStripMenuItem.Size = New Size(176, 22)
			Me.pageRightBottomToolStripMenuItem.Text = "Page Right / Down"

			' 
			' toolStripSeparator3
			' 
			Me.toolStripSeparator3.Name = "toolStripSeparator3"
			Me.toolStripSeparator3.Size = New Size(173, 6)
			' 
			' scrollLeftUpToolStripMenuItem
			' 
			Me.scrollLeftUpToolStripMenuItem.Name = "scrollLeftUpToolStripMenuItem"
			Me.scrollLeftUpToolStripMenuItem.Size = New Size(176, 22)
			Me.scrollLeftUpToolStripMenuItem.Text = "Scroll Left / Up"

			' 
			' scrollRightBottomToolStripMenuItem
			' 
			Me.scrollRightBottomToolStripMenuItem.Name = "scrollRightBottomToolStripMenuItem"
			Me.scrollRightBottomToolStripMenuItem.Size = New Size(176, 22)
			Me.scrollRightBottomToolStripMenuItem.Text = "Scroll Right / Down"

			' 
			' radVScrollBar1
			' 
			Me.radVScrollBar1.Anchor = AnchorStyles.None
			Me.radVScrollBar1.ContextMenuStrip = Me.contextMenuStrip1
			Me.radVScrollBar1.ForeColor = Color.Black
			Me.radVScrollBar1.LargeChange = 1
			Me.radVScrollBar1.Location = New Point(503, 0)
			Me.radVScrollBar1.Margin = New Padding(2)
			Me.radVScrollBar1.Maximum = 20
			Me.radVScrollBar1.Minimum = 10
			Me.radVScrollBar1.Name = "radVScrollBar1"
			Me.radVScrollBar1.Size = New Size(18, 226)
			Me.radVScrollBar1.TabIndex = 36
			Me.radVScrollBar1.Text = "radVScrollBar1"
			Me.radVScrollBar1.Value = 15


			' 
			' radHScrollBar1
			' 
			Me.radHScrollBar1.Anchor = AnchorStyles.None
			Me.radHScrollBar1.ContextMenuStrip = Me.contextMenuStrip1
			Me.radHScrollBar1.ForeColor = Color.Black
			Me.radHScrollBar1.Location = New Point(-1, 230)
			Me.radHScrollBar1.Margin = New Padding(2)
			Me.radHScrollBar1.Name = "radHScrollBar1"
			Me.radHScrollBar1.Size = New Size(501, 18)
			Me.radHScrollBar1.TabIndex = 35
			Me.radHScrollBar1.Text = "radHScrollBar1"

			' 
			' radPanelDemo
			' 
			Me.radPanelDemo.Anchor = AnchorStyles.None
			Me.radPanelDemo.ForeColor = Color.Black
			Me.radPanelDemo.Location = New Point(0, 0)
			Me.radPanelDemo.Name = "radPanelDemo"
			Me.radPanelDemo.Size = New Size(499, 225)
			Me.radPanelDemo.TabIndex = 55
			' 
			' radBtnClear
			' 
			Me.radBtnClear.Anchor = AnchorStyles.None
			Me.radBtnClear.Location = New Point(222, 481)
			Me.radBtnClear.Name = "radBtnClear"
			Me.radBtnClear.Size = New Size(77, 23)
			Me.radBtnClear.TabIndex = 56
			Me.radBtnClear.Text = "Clear"

			' 
			' radTxtEvents
			' 
			Me.radTxtEvents.Anchor = AnchorStyles.None
			Me.radTxtEvents.AutoSize = False
			Me.radTxtEvents.Location = New Point(0, 271)
			Me.radTxtEvents.Multiline = True
			Me.radTxtEvents.Name = "radTxtEvents"
			Me.radTxtEvents.Size = New Size(522, 204)
			Me.radTxtEvents.TabIndex = 57
			Me.radTxtEvents.TabStop = False
			' 
			' radLblEvents
			' 
			Me.radLblEvents.Anchor = AnchorStyles.None
			Me.radLblEvents.Location = New Point(0, 253)
			Me.radLblEvents.Name = "radLblEvents"
			Me.radLblEvents.Size = New Size(135, 18)
			Me.radLblEvents.TabIndex = 58
			Me.radLblEvents.Text = "Received Scrolling Events:"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Margin = New Padding(2)
			Me.Name = "Form1"
			Me.Size = New Size(1063, 531)
			Me.Controls.SetChildIndex(Me.radPanelDemoHolder, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radPanelDemoHolder.ResumeLayout(False)
			Me.radPanelDemoHolder.PerformLayout()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.contextMenuStrip1.ResumeLayout(False)
			CType(Me.radVScrollBar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radHScrollBar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radPanelDemo, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radBtnClear, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTxtEvents, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLblEvents, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radHScrollBar1 As Telerik.WinControls.UI.RadHScrollBar
		Private radVScrollBar1 As Telerik.WinControls.UI.RadVScrollBar
		Private contextMenuStrip1 As ContextMenuStrip
		Private scrollToToolStripMenuItem As ToolStripMenuItem
		Private toolStripSeparator1 As ToolStripSeparator
		Private leftTopToolStripMenuItem As ToolStripMenuItem
		Private rightBottomToolStripMenuItem As ToolStripMenuItem
		Private toolStripSeparator2 As ToolStripSeparator
		Private pageLeftUpToolStripMenuItem As ToolStripMenuItem
		Private pageRightBottomToolStripMenuItem As ToolStripMenuItem
		Private toolStripSeparator3 As ToolStripSeparator
		Private scrollLeftUpToolStripMenuItem As ToolStripMenuItem
		Private scrollRightBottomToolStripMenuItem As ToolStripMenuItem
		Private radPanelDemo As Telerik.WinControls.UI.RadPanel
		Private radBtnClear As Telerik.WinControls.UI.RadButton
		Private radTxtEvents As Telerik.WinControls.UI.RadTextBox
		Private radLblEvents As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace