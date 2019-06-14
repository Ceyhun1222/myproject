Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Radar
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
            Dim polarAxis1 As New Telerik.WinControls.UI.PolarAxis()
            Dim polarAxis2 As New Telerik.WinControls.UI.PolarAxis()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.radSpinEditorPointRadius = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radSpinEditorLineWidth = New Telerik.WinControls.UI.RadSpinEditor()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorPointRadius, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorLineWidth, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(1093, 52)
            Me.settingsPanel.Size = New System.Drawing.Size(153, 284)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            ' 
            ' radChartView1
            ' 
            Me.radChartView1.AreaType = Telerik.WinControls.UI.ChartAreaType.Polar
            polarAxis1.IsPrimary = True
            polarAxis2.IsPrimary = True
            Me.radChartView1.Axes.AddRange(New Telerik.WinControls.UI.Axis() {polarAxis1, polarAxis2})
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.MinimumSize = New System.Drawing.Size(280, 320)
            Me.radChartView1.Name = "radChartView1"
            ' 
            ' 
            ' 
            Me.radChartView1.RootElement.MinSize = New System.Drawing.Size(280, 320)
            Me.radChartView1.Size = New System.Drawing.Size(1158, 698)
            Me.radChartView1.TabIndex = 1
            Me.radChartView1.Text = "radChartView1"
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.radLabel2)
            Me.radGroupBox1.Controls.Add(Me.radLabel1)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditorPointRadius)
            Me.radGroupBox1.Controls.Add(Me.radSpinEditorLineWidth)
            Me.radGroupBox1.HeaderText = "Settings"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 5)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(133, 85)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Settings"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 50)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(53, 18)
            Me.radLabel2.TabIndex = 1
            Me.radLabel2.Text = "Point size"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(5, 24)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(57, 18)
            Me.radLabel1.TabIndex = 1
            Me.radLabel1.Text = "Line width"
            ' 
            ' radSpinEditorPointRadius
            ' 
            Me.radSpinEditorPointRadius.Location = New System.Drawing.Point(78, 48)
            Me.radSpinEditorPointRadius.Name = "radSpinEditorPointRadius"
            ' 
            ' 
            ' 
            Me.radSpinEditorPointRadius.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorPointRadius.Size = New System.Drawing.Size(50, 20)
            Me.radSpinEditorPointRadius.TabIndex = 0
            Me.radSpinEditorPointRadius.TabStop = False
            Me.radSpinEditorPointRadius.Tag = "Right"
            Me.radSpinEditorPointRadius.Value = New Decimal(New Integer() {6, 0, 0, 0})
            ' 
            ' radSpinEditorLineWidth
            ' 
            Me.radSpinEditorLineWidth.Location = New System.Drawing.Point(78, 22)
            Me.radSpinEditorLineWidth.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.radSpinEditorLineWidth.Name = "radSpinEditorLineWidth"
            ' 
            ' 
            ' 
            Me.radSpinEditorLineWidth.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radSpinEditorLineWidth.Size = New System.Drawing.Size(50, 20)
            Me.radSpinEditorLineWidth.TabIndex = 0
            Me.radSpinEditorLineWidth.TabStop = False
            Me.radSpinEditorLineWidth.Tag = "Right"
            Me.radSpinEditorLineWidth.Value = New Decimal(New Integer() {2, 0, 0, 0})
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScrollMinSize = New System.Drawing.Size(280, 320)
            Me.Controls.Add(Me.radChartView1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1168, 708)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radChartView1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorPointRadius, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorLineWidth, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private radChartView1 As Telerik.WinControls.UI.RadChartView
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radSpinEditorLineWidth As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditorPointRadius As Telerik.WinControls.UI.RadSpinEditor
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace