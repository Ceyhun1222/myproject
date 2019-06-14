Imports Telerik.WinControls.UI
Imports Telerik.Charting

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Bezier
    Public Class Form1
        Public Sub New()

            InitializeComponent()

            Me.SelectedControl = Me.RadChartView1
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)

            Dim bezier As BezierSeries = New BezierSeries()
            Dim point As BezierDataPoint = New BezierDataPoint(20, 40, 0, 0, 100, 140)
            bezier.DataPoints.Add(point)
            point = New BezierDataPoint(20, 100, 100, 0, 0, 0)
            bezier.DataPoints.Add(point)

            Me.RadChartView1.Series.Add(bezier)

            bezier = New BezierSeries()
            point = New BezierDataPoint(20, 150, 0, 0, 20, 250)
            bezier.DataPoints.Add(point)
            point = New BezierDataPoint(80, 150, 80, 250, 0, 0)
            bezier.DataPoints.Add(point)

            Me.RadChartView1.Series.Add(bezier)

            bezier = New BezierSeries()
            point = New BezierDataPoint(120, 80, 0, 0, 180, 10)
            bezier.DataPoints.Add(point)
            point = New BezierDataPoint(200, 80, 190, 10, 0, 0)
            bezier.DataPoints.Add(point)

            Me.RadChartView1.Series.Add(bezier)

            bezier = New BezierSeries()
            point = New BezierDataPoint(160, 120, 0, 0, 120, 180)
            bezier.DataPoints.Add(point)
            point = New BezierDataPoint(160, 180, 120, 120, 200, 240)
            bezier.DataPoints.Add(point)
            point = New BezierDataPoint(160, 240, 200, 180, 0, 0)
            bezier.DataPoints.Add(point)

            Me.RadChartView1.Series.Add(bezier)
        End Sub
    End Class
End Namespace
