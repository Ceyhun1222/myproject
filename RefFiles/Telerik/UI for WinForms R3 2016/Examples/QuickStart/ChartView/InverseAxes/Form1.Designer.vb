Imports Microsoft.VisualBasic
Imports System
Imports Telerik.WinControls.UI
Namespace Telerik.Examples.WinControls.ChartView.InverseAxes
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
			Dim cartesianArea1 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
			Dim cartesianArea2 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
			Dim cartesianArea3 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
			Me.radChartViewWellsDepth = New Telerik.WinControls.UI.RadChartView()
			Me.radCheckBoxDryHoles = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxNaturalGas = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxCrudeOil = New Telerik.WinControls.UI.RadCheckBox()
			Me.customTableLayoutPanel1 = New Telerik.Examples.WinControls.ChartView.Appearance.CustomTableLayoutPanel()
			Me.customTableLayoutPanel2 = New Telerik.Examples.WinControls.ChartView.Appearance.CustomTableLayoutPanel()
			Me.radChartViewAvgVsMaxDepth = New Telerik.WinControls.UI.RadChartView()
			Me.radChartViewDepthVsResource = New Telerik.WinControls.UI.RadChartView()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radChartViewWellsDepth, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxDryHoles, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxNaturalGas, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxCrudeOil, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.customTableLayoutPanel1.SuspendLayout()
			Me.customTableLayoutPanel2.SuspendLayout()
			CType(Me.radChartViewAvgVsMaxDepth, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radChartViewDepthVsResource, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radCheckBoxCrudeOil)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxNaturalGas)
			Me.settingsPanel.Controls.Add(Me.radCheckBoxDryHoles)
			Me.settingsPanel.Location = New System.Drawing.Point(1564, 3)
			Me.settingsPanel.Size = New System.Drawing.Size(304, 832)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxDryHoles, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxNaturalGas, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxCrudeOil, 0)
			' 
			' radChartViewWellsDepth
			' 
			Me.radChartViewWellsDepth.AreaDesign = cartesianArea1
			Me.radChartViewWellsDepth.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartViewWellsDepth.Location = New System.Drawing.Point(3, 3)
			Me.radChartViewWellsDepth.MinimumSize = New System.Drawing.Size(680, 420)
			Me.radChartViewWellsDepth.Name = "radChartViewWellsDepth"
			Me.radChartViewWellsDepth.RootElement.MinSize = New System.Drawing.Size(680, 420)
			Me.radChartViewWellsDepth.ShowGrid = False
			Me.radChartViewWellsDepth.ShowTitle = True
			Me.radChartViewWellsDepth.Size = New System.Drawing.Size(1865, 754)
			Me.radChartViewWellsDepth.TabIndex = 1
			Me.radChartViewWellsDepth.Text = "radChartView1"
			Me.radChartViewWellsDepth.Title = "U.S. Average Depth of Wells Drilled"
			CType(Me.radChartViewWellsDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Text = "U.S. Average Depth of Wells Drilled"
			CType(Me.radChartViewWellsDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Font = New System.Drawing.Font("Segoe UI Light", 20F)
			CType(Me.radChartViewWellsDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
			' 
			' radCheckBoxDryHoles
			' 
			Me.radCheckBoxDryHoles.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxDryHoles.CheckState = System.Windows.Forms.CheckState.Checked
			Me.radCheckBoxDryHoles.Location = New System.Drawing.Point(10, 5)
			Me.radCheckBoxDryHoles.Name = "radCheckBoxDryHoles"
			Me.radCheckBoxDryHoles.Size = New System.Drawing.Size(69, 18)
			Me.radCheckBoxDryHoles.TabIndex = 1
			Me.radCheckBoxDryHoles.Text = "Dry Holes"
			Me.radCheckBoxDryHoles.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			Me.radCheckBoxDryHoles.ButtonElement.CheckMarkPrimitive.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radCheckBoxNaturalGas
			' 
			Me.radCheckBoxNaturalGas.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxNaturalGas.Location = New System.Drawing.Point(10, 30)
			Me.radCheckBoxNaturalGas.Name = "radCheckBoxNaturalGas"
			Me.radCheckBoxNaturalGas.Size = New System.Drawing.Size(79, 18)
			Me.radCheckBoxNaturalGas.TabIndex = 2
			Me.radCheckBoxNaturalGas.Text = "Natural Gas"
			Me.radCheckBoxNaturalGas.ButtonElement.CheckMarkPrimitive.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' radCheckBoxCrudeOil
			' 
			Me.radCheckBoxCrudeOil.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radCheckBoxCrudeOil.Location = New System.Drawing.Point(10, 55)
			Me.radCheckBoxCrudeOil.Name = "radCheckBoxCrudeOil"
			Me.radCheckBoxCrudeOil.Size = New System.Drawing.Size(67, 18)
			Me.radCheckBoxCrudeOil.TabIndex = 2
			Me.radCheckBoxCrudeOil.Text = "Crude Oil"
			Me.radCheckBoxCrudeOil.ButtonElement.CheckMarkPrimitive.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
			' 
			' customTableLayoutPanel1
			' 
			Me.customTableLayoutPanel1.ColumnCount = 1
			Me.customTableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.customTableLayoutPanel1.Controls.Add(Me.customTableLayoutPanel2, 0, 1)
			Me.customTableLayoutPanel1.Controls.Add(Me.radChartViewWellsDepth, 0, 0)
			Me.customTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.customTableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
			Me.customTableLayoutPanel1.Name = "customTableLayoutPanel1"
			Me.customTableLayoutPanel1.RowCount = 2
			Me.customTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F))
			Me.customTableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F))
			Me.customTableLayoutPanel1.Size = New System.Drawing.Size(1871, 1086)
			Me.customTableLayoutPanel1.TabIndex = 2
			' 
			' customTableLayoutPanel2
			' 
			Me.customTableLayoutPanel2.ColumnCount = 2
			Me.customTableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F))
			Me.customTableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F))
			Me.customTableLayoutPanel2.Controls.Add(Me.radChartViewAvgVsMaxDepth, 0, 0)
			Me.customTableLayoutPanel2.Controls.Add(Me.radChartViewDepthVsResource, 1, 0)
			Me.customTableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.customTableLayoutPanel2.Location = New System.Drawing.Point(3, 763)
			Me.customTableLayoutPanel2.Name = "customTableLayoutPanel2"
			Me.customTableLayoutPanel2.RowCount = 1
			Me.customTableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.customTableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F))
			Me.customTableLayoutPanel2.Size = New System.Drawing.Size(1865, 320)
			Me.customTableLayoutPanel2.TabIndex = 0
			' 
			' radChartViewAvgVsMaxDepth
			' 
			Me.radChartViewAvgVsMaxDepth.AreaDesign = cartesianArea2
			Me.radChartViewAvgVsMaxDepth.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartViewAvgVsMaxDepth.Location = New System.Drawing.Point(3, 3)
			Me.radChartViewAvgVsMaxDepth.Name = "radChartViewAvgVsMaxDepth"
			Me.radChartViewAvgVsMaxDepth.ShowGrid = False
			Me.radChartViewAvgVsMaxDepth.ShowTitle = True
			Me.radChartViewAvgVsMaxDepth.Size = New System.Drawing.Size(926, 314)
			Me.radChartViewAvgVsMaxDepth.TabIndex = 0
			Me.radChartViewAvgVsMaxDepth.Text = "radChartView2"
			Me.radChartViewAvgVsMaxDepth.Title = "Average vs Maximum Depth Drilled"
			CType(Me.radChartViewAvgVsMaxDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Text = "Average vs Maximum Depth Drilled"
			CType(Me.radChartViewAvgVsMaxDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Font = New System.Drawing.Font("Segoe UI Light", 20F)
			CType(Me.radChartViewAvgVsMaxDepth.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
			' 
			' radChartViewDepthVsResource
			' 
			Me.radChartViewDepthVsResource.AreaDesign = cartesianArea3
			Me.radChartViewDepthVsResource.Dock = System.Windows.Forms.DockStyle.Fill
			Me.radChartViewDepthVsResource.Location = New System.Drawing.Point(935, 3)
			Me.radChartViewDepthVsResource.Name = "radChartViewDepthVsResource"
			Me.radChartViewDepthVsResource.ShowGrid = False
			Me.radChartViewDepthVsResource.ShowTitle = True
			Me.radChartViewDepthVsResource.Size = New System.Drawing.Size(927, 314)
			Me.radChartViewDepthVsResource.TabIndex = 1
			Me.radChartViewDepthVsResource.Text = "radChartView3"
			Me.radChartViewDepthVsResource.Title = "Depths by Resource Type"
			CType(Me.radChartViewDepthVsResource.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Text = "Depths by Resource Type"
			CType(Me.radChartViewDepthVsResource.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Font = New System.Drawing.Font("Segoe UI Light", 20F)
			CType(Me.radChartViewDepthVsResource.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.ChartTitleElement).Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoScrollMinSize = New System.Drawing.Size(680, 420)
			Me.Controls.Add(Me.customTableLayoutPanel1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1881, 1096)
			Me.Controls.SetChildIndex(Me.customTableLayoutPanel1, 0)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radChartViewWellsDepth, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxDryHoles, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxNaturalGas, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxCrudeOil, System.ComponentModel.ISupportInitialize).EndInit()
			Me.customTableLayoutPanel1.ResumeLayout(False)
			Me.customTableLayoutPanel2.ResumeLayout(False)
			CType(Me.radChartViewAvgVsMaxDepth, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radChartViewDepthVsResource, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radCheckBoxCrudeOil As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxNaturalGas As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBoxDryHoles As Telerik.WinControls.UI.RadCheckBox
		Private radChartViewWellsDepth As Telerik.WinControls.UI.RadChartView
		Private customTableLayoutPanel1 As Appearance.CustomTableLayoutPanel
		Private customTableLayoutPanel2 As Appearance.CustomTableLayoutPanel
		Private radChartViewAvgVsMaxDepth As RadChartView
		Private radChartViewDepthVsResource As RadChartView
	End Class
End Namespace