Imports Microsoft.VisualBasic
Imports System
Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Funnel
	Public Partial Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (Not components Is Nothing) Then
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
			Me.radLabelNeckRatio = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorNeckRatio = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabelSegmentSpacing = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditorSegmentSpacing = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radCheckBoxDynamicSlope = New Telerik.WinControls.UI.RadCheckBox()
			Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
			Me.radCheckBoxDynamicHeight = New Telerik.WinControls.UI.RadCheckBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelNeckRatio, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSpinEditorNeckRatio, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelSegmentSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radSpinEditorSegmentSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxDynamicSlope, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxDynamicHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radLabelNeckRatio)
			Me.settingsPanel.Controls.Add(Me.radLabelSegmentSpacing)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorNeckRatio)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxDynamicHeight)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxDynamicSlope)
			Me.settingsPanel.Controls.Add(Me.radSpinEditorSegmentSpacing)
			Me.settingsPanel.Location = New System.Drawing.Point(994, 46)
			Me.settingsPanel.Size = New System.Drawing.Size(252, 364)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorSegmentSpacing, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxDynamicSlope, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxDynamicHeight, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorNeckRatio, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabelSegmentSpacing, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radLabelNeckRatio, 0)
			' 
			' radLabelNeckRatio
			' 
			Me.radLabelNeckRatio.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabelNeckRatio.Location = New System.Drawing.Point(10, 35)
			Me.radLabelNeckRatio.Name = "radLabelNeckRatio"
			Me.radLabelNeckRatio.Size = New System.Drawing.Size(62, 18)
			Me.radLabelNeckRatio.TabIndex = 1
			Me.radLabelNeckRatio.Text = "Neck Ratio:"
			' 
			' radSpinEditorNeckRatio
			' 
			Me.radSpinEditorNeckRatio.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorNeckRatio.DecimalPlaces = 1
			Me.radSpinEditorNeckRatio.Increment = New Decimal(New Integer() { 1, 0, 0, 65536})
			Me.radSpinEditorNeckRatio.Location = New System.Drawing.Point(10, 56)
			Me.radSpinEditorNeckRatio.Maximum = New Decimal(New Integer() { 3, 0, 0, 0})
			Me.radSpinEditorNeckRatio.Name = "radSpinEditorNeckRatio"
			' 
			' 
			' 
			Me.radSpinEditorNeckRatio.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radSpinEditorNeckRatio.Size = New System.Drawing.Size(232, 20)
			Me.radSpinEditorNeckRatio.TabIndex = 2
			Me.radSpinEditorNeckRatio.TabStop = False
			Me.radSpinEditorNeckRatio.Value = New Decimal(New Integer() { 3, 0, 0, 65536})
			' 
			' radLabelSegmentSpacing
			' 
			Me.radLabelSegmentSpacing.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabelSegmentSpacing.Location = New System.Drawing.Point(10, 85)
			Me.radLabelSegmentSpacing.Name = "radLabelSegmentSpacing"
			Me.radLabelSegmentSpacing.Size = New System.Drawing.Size(95, 18)
			Me.radLabelSegmentSpacing.TabIndex = 3
			Me.radLabelSegmentSpacing.Text = "Segment Spacing:"
			' 
			' radSpinEditorSegmentSpacing
			' 
			Me.radSpinEditorSegmentSpacing.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditorSegmentSpacing.Location = New System.Drawing.Point(10, 106)
			Me.radSpinEditorSegmentSpacing.Maximum = New Decimal(New Integer() { 50, 0, 0, 0})
			Me.radSpinEditorSegmentSpacing.Name = "radSpinEditorSegmentSpacing"
			' 
			' 
			' 
			Me.radSpinEditorSegmentSpacing.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.radSpinEditorSegmentSpacing.Size = New System.Drawing.Size(232, 20)
			Me.radSpinEditorSegmentSpacing.TabIndex = 4
			Me.radSpinEditorSegmentSpacing.TabStop = False
			Me.radSpinEditorSegmentSpacing.Value = New Decimal(New Integer() { 1, 0, 0, 0})
			' 
			' radCheckBoxDynamicSlope
			' 
			Me.radCheckBoxDynamicSlope.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxDynamicSlope.Location = New System.Drawing.Point(10, 141)
			Me.radCheckBoxDynamicSlope.Name = "radCheckBoxDynamicSlope"
			Me.radCheckBoxDynamicSlope.Size = New System.Drawing.Size(97, 18)
			Me.radCheckBoxDynamicSlope.TabIndex = 5
			Me.radCheckBoxDynamicSlope.Text = "Dynamic Slope:"
			' 
			' radChartView1
			' 
			Me.radChartView1.AreaType = Telerik.WinControls.UI.ChartAreaType.Funnel
			Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartView1.Location = New System.Drawing.Point(0, 0)
			Me.radChartView1.MinimumSize = New System.Drawing.Size(280, 320)
			Me.radChartView1.Name = "radChartView1"
			' 
			' 
			' 
			Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(280, 320)
			Me.radChartView1.ShowGrid = False
			Me.radChartView1.Size = New System.Drawing.Size(1871, 1086)
			Me.radChartView1.TabIndex = 1
			Me.radChartView1.Text = "radChartView1"
			Me.radChartView1.Title = "Chart title"
			' 
			' radCheckBoxDynamicHeight
			' 
			Me.radCheckBoxDynamicHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxDynamicHeight.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxDynamicHeight.Location = New System.Drawing.Point(10, 174)
			Me.radCheckBoxDynamicHeight.Name = "radCheckBoxDynamicHeight"
			Me.radCheckBoxDynamicHeight.Size = New System.Drawing.Size(103, 18)
			Me.radCheckBoxDynamicHeight.TabIndex = 5
			Me.radCheckBoxDynamicHeight.Text = "Dynamic Height:"
			Me.radCheckBoxDynamicHeight.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoScrollMinSize = New System.Drawing.Size(280, 320)
			Me.Controls.Add(Me.radChartView1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.radChartView1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelNeckRatio, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSpinEditorNeckRatio, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelSegmentSpacing, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radSpinEditorSegmentSpacing, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxDynamicSlope, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxDynamicHeight, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radLabelNeckRatio As Telerik.WinControls.UI.RadLabel
		Private radSpinEditorNeckRatio As Telerik.WinControls.UI.RadSpinEditor
		Private radLabelSegmentSpacing As Telerik.WinControls.UI.RadLabel
		Private radSpinEditorSegmentSpacing As Telerik.WinControls.UI.RadSpinEditor
		Private radCheckBoxDynamicSlope As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxDynamicHeight As Telerik.WinControls.UI.RadCheckBox
	End Class
End Namespace