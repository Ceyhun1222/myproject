
Imports System.Windows.Forms
Namespace Telerik.Examples.WinControls.RangeSelector.FirstLook
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
            Dim CartesianArea1 As Telerik.WinControls.UI.CartesianArea = New Telerik.WinControls.UI.CartesianArea()
            Dim CategoricalAxis1 As Telerik.WinControls.UI.CategoricalAxis = New Telerik.WinControls.UI.CategoricalAxis()
            Dim LinearAxis1 As Telerik.WinControls.UI.LinearAxis = New Telerik.WinControls.UI.LinearAxis()
            Dim LineSeries1 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint1 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint2 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint3 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint4 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint5 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint6 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint7 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint8 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint9 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint10 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint11 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint12 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim LineSeries2 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint13 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint14 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint15 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint16 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint17 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint18 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint19 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint20 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint21 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint22 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint23 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint24 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim LineSeries3 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint25 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint26 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint27 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint28 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint29 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint30 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint31 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint32 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint33 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint34 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint35 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint36 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim LineSeries4 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint37 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint38 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint39 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint40 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint41 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint42 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint43 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint44 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint45 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint46 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint47 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint48 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim LineSeries5 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint49 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint50 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint51 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint52 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint53 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint54 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint55 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint56 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint57 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint58 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint59 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint60 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim LineSeries6 As Telerik.WinControls.UI.LineSeries = New Telerik.WinControls.UI.LineSeries()
            Dim CategoricalDataPoint61 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint62 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint63 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint64 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint65 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint66 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint67 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint68 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint69 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint70 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint71 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Dim CategoricalDataPoint72 As Telerik.Charting.CategoricalDataPoint = New Telerik.Charting.CategoricalDataPoint()
            Me.radSplitContainer1 = New Telerik.WinControls.UI.RadSplitContainer()
            Me.splitPanel1 = New Telerik.WinControls.UI.SplitPanel()
            Me.radRangeSlider1 = New Telerik.WinControls.UI.RadRangeSelector()
            Me.radChartView1 = New Telerik.WinControls.UI.RadChartView()
            Me.splitPanel2 = New Telerik.WinControls.UI.SplitPanel()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radSplitContainer1.SuspendLayout()
            CType(Me.splitPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitPanel1.SuspendLayout()
            CType(Me.radRangeSlider1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.splitPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitPanel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Location = New System.Drawing.Point(766, 60)
            '
            'themePanel
            '
            Me.themePanel.Location = New System.Drawing.Point(766, 226)
            '
            'radSplitContainer1
            '
            Me.radSplitContainer1.Controls.Add(Me.splitPanel1)
            Me.radSplitContainer1.Controls.Add(Me.splitPanel2)
            Me.radSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radSplitContainer1.Location = New System.Drawing.Point(0, 0)
            Me.radSplitContainer1.Name = "radSplitContainer1"
            Me.radSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            '
            '
            Me.radSplitContainer1.RootElement.MinSize = New System.Drawing.Size(25, 25)
            Me.radSplitContainer1.Size = New System.Drawing.Size(25, 25)
            Me.radSplitContainer1.SplitterWidth = 4
            Me.radSplitContainer1.TabIndex = 2
            Me.radSplitContainer1.TabStop = False
            Me.radSplitContainer1.Text = "radSplitContainer1"
            '
            'splitPanel1
            '
            Me.splitPanel1.Controls.Add(Me.radRangeSlider1)
            Me.splitPanel1.Location = New System.Drawing.Point(0, 0)
            Me.splitPanel1.Name = "splitPanel1"
            '
            '
            '
            Me.splitPanel1.RootElement.MinSize = New System.Drawing.Size(25, 25)
            Me.splitPanel1.Size = New System.Drawing.Size(25, 25)
            Me.splitPanel1.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0.0!, -0.2330017!)
            Me.splitPanel1.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, -141)
            Me.splitPanel1.TabIndex = 0
            Me.splitPanel1.TabStop = False
            Me.splitPanel1.Text = "splitPanel1"
            '
            'radRangeSlider1
            '
            Me.radRangeSlider1.AssociatedControl = Me.radChartView1
            Me.radRangeSlider1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radRangeSlider1.Location = New System.Drawing.Point(0, 0)
            Me.radRangeSlider1.Name = "radRangeSlider1"
            Me.radRangeSlider1.Size = New System.Drawing.Size(25, 25)
            Me.radRangeSlider1.TabIndex = 0
            Me.radRangeSlider1.Text = "radRangeSlider1"
            '
            'radChartView1
            '
            Me.radChartView1.AreaDesign = CartesianArea1
            CategoricalAxis1.IsPrimary = True
            CategoricalAxis1.LabelRotationAngle = 300.0R
            CategoricalAxis1.Title = ""
            LinearAxis1.AxisType = Telerik.Charting.AxisType.Second
            LinearAxis1.IsPrimary = True
            LinearAxis1.LabelRotationAngle = 300.0R
            LinearAxis1.MajorStep = 20.0R
            LinearAxis1.Title = ""
            Me.radChartView1.Axes.AddRange(New Telerik.WinControls.UI.Axis() {CategoricalAxis1, LinearAxis1})
            Me.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radChartView1.Location = New System.Drawing.Point(0, 0)
            Me.radChartView1.Name = "radChartView1"
            CategoricalDataPoint1.Category = "Jan"
            CategoricalDataPoint1.Label = 41.0R
            CategoricalDataPoint1.Value = 41.0R
            CategoricalDataPoint2.Category = "Feb"
            CategoricalDataPoint2.Label = 77.0R
            CategoricalDataPoint2.Value = 77.0R
            CategoricalDataPoint3.Category = "Mar"
            CategoricalDataPoint3.Label = 14.0R
            CategoricalDataPoint3.Value = 14.0R
            CategoricalDataPoint4.Category = "Apr"
            CategoricalDataPoint4.Label = 63.0R
            CategoricalDataPoint4.Value = 63.0R
            CategoricalDataPoint5.Category = "May"
            CategoricalDataPoint5.Label = 28.0R
            CategoricalDataPoint5.Value = 28.0R
            CategoricalDataPoint6.Category = "Jun"
            CategoricalDataPoint6.Label = 39.0R
            CategoricalDataPoint6.Value = 39.0R
            CategoricalDataPoint7.Category = "Jul"
            CategoricalDataPoint7.Label = 77.0R
            CategoricalDataPoint7.Value = 77.0R
            CategoricalDataPoint8.Category = "Aug"
            CategoricalDataPoint8.Label = 87.0R
            CategoricalDataPoint8.Value = 87.0R
            CategoricalDataPoint9.Category = "Sep"
            CategoricalDataPoint9.Label = 94.0R
            CategoricalDataPoint9.Value = 94.0R
            CategoricalDataPoint10.Category = "Oct"
            CategoricalDataPoint10.Label = 82.0R
            CategoricalDataPoint10.Value = 82.0R
            CategoricalDataPoint11.Category = "Nov"
            CategoricalDataPoint11.Label = 90.0R
            CategoricalDataPoint11.Value = 90.0R
            CategoricalDataPoint12.Category = "Dec"
            CategoricalDataPoint12.Label = 95.0R
            CategoricalDataPoint12.Value = 95.0R
            LineSeries1.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint1, CategoricalDataPoint2, CategoricalDataPoint3, CategoricalDataPoint4, CategoricalDataPoint5, CategoricalDataPoint6, CategoricalDataPoint7, CategoricalDataPoint8, CategoricalDataPoint9, CategoricalDataPoint10, CategoricalDataPoint11, CategoricalDataPoint12})
            LineSeries1.HorizontalAxis = CategoricalAxis1
            LineSeries1.LabelAngle = 90.0R
            LineSeries1.LabelDistanceToPoint = 15.0R
            LineSeries1.LegendTitle = Nothing
            LineSeries1.VerticalAxis = LinearAxis1
            CategoricalDataPoint13.Category = "Jan"
            CategoricalDataPoint13.Label = 13.0R
            CategoricalDataPoint13.Value = 13.0R
            CategoricalDataPoint14.Category = "Feb"
            CategoricalDataPoint14.Label = 42.0R
            CategoricalDataPoint14.Value = 42.0R
            CategoricalDataPoint15.Category = "Mar"
            CategoricalDataPoint15.Label = 92.0R
            CategoricalDataPoint15.Value = 92.0R
            CategoricalDataPoint16.Category = "Apr"
            CategoricalDataPoint16.Label = 85.0R
            CategoricalDataPoint16.Value = 85.0R
            CategoricalDataPoint17.Category = "May"
            CategoricalDataPoint17.Label = 78.0R
            CategoricalDataPoint17.Value = 78.0R
            CategoricalDataPoint18.Category = "Jun"
            CategoricalDataPoint18.Label = 87.0R
            CategoricalDataPoint18.Value = 87.0R
            CategoricalDataPoint19.Category = "Jul"
            CategoricalDataPoint19.Label = 96.0R
            CategoricalDataPoint19.Value = 96.0R
            CategoricalDataPoint20.Category = "Aug"
            CategoricalDataPoint20.Label = 81.0R
            CategoricalDataPoint20.Value = 81.0R
            CategoricalDataPoint21.Category = "Sep"
            CategoricalDataPoint21.Label = 62.0R
            CategoricalDataPoint21.Value = 62.0R
            CategoricalDataPoint22.Category = "Oct"
            CategoricalDataPoint22.Label = 43.0R
            CategoricalDataPoint22.Value = 43.0R
            CategoricalDataPoint23.Category = "Nov"
            CategoricalDataPoint23.Label = 40.0R
            CategoricalDataPoint23.Value = 40.0R
            CategoricalDataPoint24.Category = "Dec"
            CategoricalDataPoint24.Label = 20.0R
            CategoricalDataPoint24.Value = 20.0R
            LineSeries2.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint13, CategoricalDataPoint14, CategoricalDataPoint15, CategoricalDataPoint16, CategoricalDataPoint17, CategoricalDataPoint18, CategoricalDataPoint19, CategoricalDataPoint20, CategoricalDataPoint21, CategoricalDataPoint22, CategoricalDataPoint23, CategoricalDataPoint24})
            LineSeries2.HorizontalAxis = CategoricalAxis1
            LineSeries2.LabelAngle = 90.0R
            LineSeries2.LabelDistanceToPoint = 15.0R
            LineSeries2.VerticalAxis = LinearAxis1
            CategoricalDataPoint25.Category = "Jan"
            CategoricalDataPoint25.Label = 90.0R
            CategoricalDataPoint25.Value = 90.0R
            CategoricalDataPoint26.Category = "Feb"
            CategoricalDataPoint26.Label = 70.0R
            CategoricalDataPoint26.Value = 70.0R
            CategoricalDataPoint27.Category = "Mar"
            CategoricalDataPoint27.Label = 105.0R
            CategoricalDataPoint27.Value = 105.0R
            CategoricalDataPoint28.Category = "Apr"
            CategoricalDataPoint28.Label = 42.0R
            CategoricalDataPoint28.Value = 42.0R
            CategoricalDataPoint29.Category = "May"
            CategoricalDataPoint29.Label = 120.0R
            CategoricalDataPoint29.Value = 120.0R
            CategoricalDataPoint30.Category = "Jun"
            CategoricalDataPoint30.Label = 5.0R
            CategoricalDataPoint30.Value = 5.0R
            CategoricalDataPoint31.Category = "Jul"
            CategoricalDataPoint31.Label = 20.0R
            CategoricalDataPoint31.Value = 20.0R
            CategoricalDataPoint32.Category = "Aug"
            CategoricalDataPoint32.Label = 32.0R
            CategoricalDataPoint32.Value = 32.0R
            CategoricalDataPoint33.Category = "Sep"
            CategoricalDataPoint33.Label = 11.0R
            CategoricalDataPoint33.Value = 11.0R
            CategoricalDataPoint34.Category = "Oct"
            CategoricalDataPoint34.Label = 2.0R
            CategoricalDataPoint34.Value = 2.0R
            CategoricalDataPoint35.Category = "Nov"
            CategoricalDataPoint35.Label = 3.0R
            CategoricalDataPoint35.Value = 3.0R
            CategoricalDataPoint36.Category = "Dec"
            CategoricalDataPoint36.Label = 1.0R
            CategoricalDataPoint36.Value = 1.0R
            LineSeries3.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint25, CategoricalDataPoint26, CategoricalDataPoint27, CategoricalDataPoint28, CategoricalDataPoint29, CategoricalDataPoint30, CategoricalDataPoint31, CategoricalDataPoint32, CategoricalDataPoint33, CategoricalDataPoint34, CategoricalDataPoint35, CategoricalDataPoint36})
            LineSeries3.HorizontalAxis = CategoricalAxis1
            LineSeries3.LabelAngle = 90.0R
            LineSeries3.LabelDistanceToPoint = 15.0R
            LineSeries3.VerticalAxis = LinearAxis1
            CategoricalDataPoint37.Category = "Jan"
            CategoricalDataPoint37.Label = 5.0R
            CategoricalDataPoint37.Value = 5.0R
            CategoricalDataPoint38.Category = "Feb"
            CategoricalDataPoint38.Label = 15.0R
            CategoricalDataPoint38.Value = 15.0R
            CategoricalDataPoint39.Category = "Mar"
            CategoricalDataPoint39.Label = 35.0R
            CategoricalDataPoint39.Value = 35.0R
            CategoricalDataPoint40.Category = "Apr"
            CategoricalDataPoint40.Label = 38.0R
            CategoricalDataPoint40.Value = 38.0R
            CategoricalDataPoint41.Category = "May"
            CategoricalDataPoint41.Label = 55.0R
            CategoricalDataPoint41.Value = 55.0R
            CategoricalDataPoint42.Category = "Jun"
            CategoricalDataPoint42.Label = 78.0R
            CategoricalDataPoint42.Value = 78.0R
            CategoricalDataPoint43.Category = "Jul"
            CategoricalDataPoint43.Label = 83.0R
            CategoricalDataPoint43.Value = 83.0R
            CategoricalDataPoint44.Category = "Aug"
            CategoricalDataPoint44.Label = 90.0R
            CategoricalDataPoint44.Value = 90.0R
            CategoricalDataPoint45.Category = "Sep"
            CategoricalDataPoint45.Label = 110.0R
            CategoricalDataPoint45.Value = 110.0R
            CategoricalDataPoint46.Category = "Oct"
            CategoricalDataPoint46.Label = 120.0R
            CategoricalDataPoint46.Value = 120.0R
            CategoricalDataPoint47.Category = "Nov"
            CategoricalDataPoint47.Label = 115.0R
            CategoricalDataPoint47.Value = 115.0R
            CategoricalDataPoint48.Category = "Dec"
            CategoricalDataPoint48.Label = 120.0R
            CategoricalDataPoint48.Value = 120.0R
            LineSeries4.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint37, CategoricalDataPoint38, CategoricalDataPoint39, CategoricalDataPoint40, CategoricalDataPoint41, CategoricalDataPoint42, CategoricalDataPoint43, CategoricalDataPoint44, CategoricalDataPoint45, CategoricalDataPoint46, CategoricalDataPoint47, CategoricalDataPoint48})
            LineSeries4.HorizontalAxis = CategoricalAxis1
            LineSeries4.LabelAngle = 90.0R
            LineSeries4.LabelDistanceToPoint = 15.0R
            LineSeries4.VerticalAxis = LinearAxis1
            CategoricalDataPoint49.Category = "Jan"
            CategoricalDataPoint49.Label = 0.0R
            CategoricalDataPoint49.Value = 0.0R
            CategoricalDataPoint50.Category = "Feb"
            CategoricalDataPoint50.Label = 35.0R
            CategoricalDataPoint50.Value = 35.0R
            CategoricalDataPoint51.Category = "Mar"
            CategoricalDataPoint51.Label = 45.0R
            CategoricalDataPoint51.Value = 45.0R
            CategoricalDataPoint52.Category = "Apr"
            CategoricalDataPoint52.Label = 35.0R
            CategoricalDataPoint52.Value = 35.0R
            CategoricalDataPoint53.Category = "May"
            CategoricalDataPoint53.Label = 20.0R
            CategoricalDataPoint53.Value = 20.0R
            CategoricalDataPoint54.Category = "Jun"
            CategoricalDataPoint54.Label = 35.0R
            CategoricalDataPoint54.Value = 35.0R
            CategoricalDataPoint55.Category = "Jul"
            CategoricalDataPoint55.Label = 45.0R
            CategoricalDataPoint55.Value = 45.0R
            CategoricalDataPoint56.Category = "Aug"
            CategoricalDataPoint56.Label = 55.0R
            CategoricalDataPoint56.Value = 55.0R
            CategoricalDataPoint57.Category = "Sep"
            CategoricalDataPoint57.Label = 45.0R
            CategoricalDataPoint57.Value = 45.0R
            CategoricalDataPoint58.Category = "Oct"
            CategoricalDataPoint58.Label = 65.0R
            CategoricalDataPoint58.Value = 65.0R
            CategoricalDataPoint59.Category = "Nov"
            CategoricalDataPoint59.Label = 55.0R
            CategoricalDataPoint59.Value = 55.0R
            CategoricalDataPoint60.Category = "Dec"
            CategoricalDataPoint60.Label = 100.0R
            CategoricalDataPoint60.Value = 100.0R
            LineSeries5.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint49, CategoricalDataPoint50, CategoricalDataPoint51, CategoricalDataPoint52, CategoricalDataPoint53, CategoricalDataPoint54, CategoricalDataPoint55, CategoricalDataPoint56, CategoricalDataPoint57, CategoricalDataPoint58, CategoricalDataPoint59, CategoricalDataPoint60})
            LineSeries5.HorizontalAxis = CategoricalAxis1
            LineSeries5.LabelAngle = 90.0R
            LineSeries5.LabelDistanceToPoint = 15.0R
            LineSeries5.VerticalAxis = LinearAxis1
            CategoricalDataPoint61.Category = "Jan"
            CategoricalDataPoint61.Label = 70.0R
            CategoricalDataPoint61.Value = 70.0R
            CategoricalDataPoint62.Category = "Feb"
            CategoricalDataPoint62.Label = 60.0R
            CategoricalDataPoint62.Value = 60.0R
            CategoricalDataPoint63.Category = "Mar"
            CategoricalDataPoint63.Label = 80.0R
            CategoricalDataPoint63.Value = 80.0R
            CategoricalDataPoint64.Category = "Apr"
            CategoricalDataPoint64.Label = 50.0R
            CategoricalDataPoint64.Value = 50.0R
            CategoricalDataPoint65.Category = "May"
            CategoricalDataPoint65.Label = 60.0R
            CategoricalDataPoint65.Value = 60.0R
            CategoricalDataPoint66.Category = "Jun"
            CategoricalDataPoint66.Label = 70.0R
            CategoricalDataPoint66.Value = 70.0R
            CategoricalDataPoint67.Category = "Jul"
            CategoricalDataPoint67.Label = 40.0R
            CategoricalDataPoint67.Value = 40.0R
            CategoricalDataPoint68.Category = "Aug"
            CategoricalDataPoint68.Label = 15.0R
            CategoricalDataPoint68.Value = 15.0R
            CategoricalDataPoint69.Category = "Sep"
            CategoricalDataPoint69.Label = 40.0R
            CategoricalDataPoint69.Value = 40.0R
            CategoricalDataPoint70.Category = "Oct"
            CategoricalDataPoint70.Label = 90.0R
            CategoricalDataPoint70.Value = 90.0R
            CategoricalDataPoint71.Category = "Nov"
            CategoricalDataPoint71.Label = 50.0R
            CategoricalDataPoint71.Value = 50.0R
            CategoricalDataPoint72.Category = "Dec"
            CategoricalDataPoint72.Label = 20.0R
            CategoricalDataPoint72.Value = 20.0R
            LineSeries6.DataPoints.AddRange(New Telerik.Charting.DataPoint() {CategoricalDataPoint61, CategoricalDataPoint62, CategoricalDataPoint63, CategoricalDataPoint64, CategoricalDataPoint65, CategoricalDataPoint66, CategoricalDataPoint67, CategoricalDataPoint68, CategoricalDataPoint69, CategoricalDataPoint70, CategoricalDataPoint71, CategoricalDataPoint72})
            LineSeries6.HorizontalAxis = CategoricalAxis1
            LineSeries6.LabelAngle = 90.0R
            LineSeries6.LabelDistanceToPoint = 15.0R
            LineSeries6.VerticalAxis = LinearAxis1
            Me.radChartView1.Series.AddRange(New Telerik.WinControls.UI.ChartSeries() {LineSeries1, LineSeries2, LineSeries3, LineSeries4, LineSeries5, LineSeries6})
            Me.radChartView1.ShowGrid = False
            Me.radChartView1.ShowPanZoom = True
            Me.radChartView1.Size = New System.Drawing.Size(25, 25)
            Me.radChartView1.TabIndex = 0
            Me.radChartView1.Text = "radChartView1"
            '
            'splitPanel2
            '
            Me.splitPanel2.Controls.Add(Me.radChartView1)
            Me.splitPanel2.Location = New System.Drawing.Point(0, 29)
            Me.splitPanel2.Name = "splitPanel2"
            '
            '
            '
            Me.splitPanel2.RootElement.MinSize = New System.Drawing.Size(25, 25)
            Me.splitPanel2.Size = New System.Drawing.Size(25, 25)
            Me.splitPanel2.SizeInfo.AutoSizeScale = New System.Drawing.SizeF(0.0!, 0.2330017!)
            Me.splitPanel2.SizeInfo.SplitterCorrection = New System.Drawing.Size(0, 141)
            Me.splitPanel2.TabIndex = 1
            Me.splitPanel2.TabStop = False
            Me.splitPanel2.Text = "splitPanel2"
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radSplitContainer1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(0, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radSplitContainer1, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radSplitContainer1.ResumeLayout(False)
            CType(Me.splitPanel1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitPanel1.ResumeLayout(False)
            CType(Me.radRangeSlider1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radChartView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.splitPanel2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitPanel2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radSplitContainer1 As Telerik.WinControls.UI.RadSplitContainer
        Private radRangeSlider1 As Telerik.WinControls.UI.RadRangeSelector
        Private radChartView1 As Telerik.WinControls.UI.RadChartView
        Friend WithEvents splitPanel1 As Telerik.WinControls.UI.SplitPanel
        Friend WithEvents splitPanel2 As Telerik.WinControls.UI.SplitPanel
    End Class
End Namespace