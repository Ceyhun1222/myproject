Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.ChartView.ScaleBreaks
	Partial Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
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
			Dim cartesianArea1 As New Telerik.WinControls.UI.CartesianArea()
			Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radDropDownListStyle = New Telerik.WinControls.UI.RadDropDownList()
			Me.radColorBoxBackColor = New Telerik.WinControls.UI.RadColorBox()
			Me.radColorBoxBorderColor = New Telerik.WinControls.UI.RadColorBox()
			Me.radSpinEditorSize = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radButtonScaleBreaks = New Telerik.WinControls.UI.RadButton()
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			DirectCast(Me.radDropDownListStyle, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radColorBoxBackColor, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radColorBoxBorderColor, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditorSize, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radButtonScaleBreaks, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radButtonScaleBreaks)
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Location = New System.Drawing.Point(989, 19)
			Me.settingsPanel.Size = New System.Drawing.Size(304, 832)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButtonScaleBreaks, 0)
			' 
			' radChartView1
			' 
			Me.radChartView1.AreaDesign = cartesianArea1
			Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartView1.Location = New System.Drawing.Point(0, 0)
			Me.radChartView1.MinimumSize = New System.Drawing.Size(680, 420)
			Me.radChartView1.Name = "radChartView1"
			' 
			' 
			' 
			Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(680, 420)
			Me.radChartView1.ShowGrid = False
			Me.radChartView1.ShowTitle = True
			Me.radChartView1.Size = New System.Drawing.Size(1871, 1086)
			Me.radChartView1.TabIndex = 1
			Me.radChartView1.Text = "radChartView1"
			Me.radChartView1.Title = "Sales by Region"
			DirectCast(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Text = "Sales by Region"
			DirectCast(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Font = New System.Drawing.Font("Segoe UI Light", 20F)
			DirectCast(Me.radChartView1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox1.Controls.Add(Me.radDropDownListStyle)
			Me.radGroupBox1.Controls.Add(Me.radColorBoxBackColor)
			Me.radGroupBox1.Controls.Add(Me.radColorBoxBorderColor)
			Me.radGroupBox1.Controls.Add(Me.radSpinEditorSize)
			Me.radGroupBox1.Controls.Add(Me.radLabel3)
			Me.radGroupBox1.Controls.Add(Me.radLabel2)
			Me.radGroupBox1.Controls.Add(Me.radLabel4)
			Me.radGroupBox1.Controls.Add(Me.radLabel1)
			Me.radGroupBox1.HeaderText = "Scale break settings"
			Me.radGroupBox1.Location = New System.Drawing.Point(10, 76)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Size = New System.Drawing.Size(284, 222)
			Me.radGroupBox1.TabIndex = 2
			Me.radGroupBox1.Text = "Scale break settings"
			' 
			' radDropDownListStyle
			' 
			Me.radDropDownListStyle.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownListStyle.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListStyle.Location = New System.Drawing.Point(5, 40)
			Me.radDropDownListStyle.Name = "radDropDownListStyle"
			Me.radDropDownListStyle.Size = New System.Drawing.Size(274, 20)
			Me.radDropDownListStyle.TabIndex = 3
			Me.radDropDownListStyle.Text = "radDropDownList1"
			' 
			' radColorBoxBackColor
			' 
			Me.radColorBoxBackColor.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radColorBoxBackColor.Location = New System.Drawing.Point(5, 190)
			Me.radColorBoxBackColor.Name = "radColorBoxBackColor"
			Me.radColorBoxBackColor.Size = New System.Drawing.Size(274, 20)
			Me.radColorBoxBackColor.TabIndex = 2
			Me.radColorBoxBackColor.Text = "radColorBox1"
			' 
			' radColorBoxBorderColor
			' 
			Me.radColorBoxBorderColor.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radColorBoxBorderColor.Location = New System.Drawing.Point(5, 140)
			Me.radColorBoxBorderColor.Name = "radColorBoxBorderColor"
			Me.radColorBoxBorderColor.Size = New System.Drawing.Size(274, 20)
			Me.radColorBoxBorderColor.TabIndex = 2
			Me.radColorBoxBorderColor.Text = "radColorBox1"
			' 
			' radSpinEditorSize
			' 
			Me.radSpinEditorSize.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorSize.Location = New System.Drawing.Point(5, 90)
			Me.radSpinEditorSize.Name = "radSpinEditorSize"
			Me.radSpinEditorSize.Size = New System.Drawing.Size(274, 20)
			Me.radSpinEditorSize.TabIndex = 1
			Me.radSpinEditorSize.TabStop = False
			' 
			' radLabel3
			' 
			Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel3.Location = New System.Drawing.Point(5, 171)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New System.Drawing.Size(57, 18)
			Me.radLabel3.TabIndex = 0
			Me.radLabel3.Text = "Back color"
			' 
			' radLabel2
			' 
			Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel2.Location = New System.Drawing.Point(5, 121)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New System.Drawing.Size(67, 18)
			Me.radLabel2.TabIndex = 0
			Me.radLabel2.Text = "Border color"
			' 
			' radLabel4
			' 
			Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel4.Location = New System.Drawing.Point(5, 21)
			Me.radLabel4.Name = "radLabel4"
			Me.radLabel4.Size = New System.Drawing.Size(29, 18)
			Me.radLabel4.TabIndex = 0
			Me.radLabel4.Text = "Style"
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(5, 71)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(25, 18)
			Me.radLabel1.TabIndex = 0
			Me.radLabel1.Text = "Size"
			' 
			' radButtonScaleBreaks
			' 
			Me.radButtonScaleBreaks.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radButtonScaleBreaks.Location = New System.Drawing.Point(10, 33)
			Me.radButtonScaleBreaks.Name = "radButtonScaleBreaks"
			Me.radButtonScaleBreaks.Size = New System.Drawing.Size(284, 37)
			Me.radButtonScaleBreaks.TabIndex = 3
			Me.radButtonScaleBreaks.Text = "Remove Scale Breaks"
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoScrollMinSize = New System.Drawing.Size(680, 420)
			Me.Controls.Add(Me.radChartView1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.radChartView1, 0)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			DirectCast(Me.radDropDownListStyle, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radColorBoxBackColor, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radColorBoxBorderColor, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditorSize, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radButtonScaleBreaks, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radGroupBox1 As RadGroupBox
		Private radDropDownListStyle As RadDropDownList
		Private radColorBoxBackColor As RadColorBox
		Private radColorBoxBorderColor As RadColorBox
		Private radSpinEditorSize As RadSpinEditor
		Private radLabel3 As RadLabel
		Private radLabel2 As RadLabel
		Private radLabel4 As RadLabel
		Private radLabel1 As RadLabel
		Private radButtonScaleBreaks As RadButton
	End Class
End Namespace