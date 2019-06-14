Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Charting
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Range
	Public Partial Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			Me.radDropDownListChartType.SelectedIndex = 1

			Me.SelectedControl = Me.radChartView1
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Dim area As CartesianArea = Me.radChartView1.GetArea(Of CartesianArea)()
			area.ShowGrid = True

			Dim verticalAxis As LinearAxis = New LinearAxis()
			verticalAxis.LabelFormat = "{0}°C"
			verticalAxis.AxisType = AxisType.Second

			Dim horizontalAxis As DateTimeContinuousAxis = New DateTimeContinuousAxis()
			horizontalAxis.LabelFormat = "{0:MMM}"

			Dim model As WeatherModel = New WeatherModel()

			Dim barSeries As RangeBarSeries = New RangeBarSeries("High", "Low", "Time")
			barSeries.Name = "Temperature"
			barSeries.HorizontalAxis = horizontalAxis
			barSeries.VerticalAxis = verticalAxis

			Me.radChartView1.Series.Add(barSeries)

			Me.radChartView1.DataSource = model.GetTemperatureData()

			Dim line2011 As CartesianGridLineAnnotation = New CartesianGridLineAnnotation(horizontalAxis, New DateTime(2011, 1, 1))
			line2011.Label = "2011"
			line2011.DrawMode = AnnotationDrawMode.BelowSeries
			Dim line2012 As CartesianGridLineAnnotation = New CartesianGridLineAnnotation(horizontalAxis, New DateTime(2012, 1, 1))
			line2012.Label = "2012"
			line2012.DrawMode = AnnotationDrawMode.BelowSeries

			Me.radChartView1.Area.Annotations.Add(line2011)
			Me.radChartView1.Area.Annotations.Add(line2012)
			Me.radChartView1.View.Palette = KnownPalette.Metro
		End Sub

		Private Sub radDropDownListChartType_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
			Me.radCheckBoxSpline.Enabled = e.Position = 0

			Me.radChartView1.Series.Clear()

			If e.Position = 0 Then
				Dim rangeSeries As RangeSeries = New RangeSeries("High", "Low", "Time")
				rangeSeries.Name = "Temperature"
				rangeSeries.Spline = Me.radCheckBoxSpline.IsChecked

				Me.radChartView1.Series.Add(rangeSeries)
			Else
				Dim barSeries As RangeBarSeries = New RangeBarSeries("High", "Low", "Time")
				barSeries.Name = "Temperature"

				Me.radChartView1.Series.Add(barSeries)
			End If
		End Sub

		Private Sub showLabelsCheckBox_ToggleStateChanged(ByVal sender As Object, ByVal args As StateChangedEventArgs)
			Dim i As Integer = 0
			Do While i < Me.radChartView1.Series.Count
				Dim rangeSeries As RangeSeriesBase = Me.radChartView1.GetSeries(Of RangeSeriesBase)(i)
				rangeSeries.ShowLabels = Not rangeSeries.ShowLabels
				i += 1
			Loop
		End Sub

		Private Sub orientationCheckBox_ToggleStateChanged(ByVal sender As Object, ByVal args As StateChangedEventArgs)
			Dim grid As CartesianGrid = Me.radChartView1.GetArea(Of CartesianArea)().GetGrid(Of CartesianGrid)()

			If Me.orientationCheckBox.IsChecked Then
				Me.radChartView1.GetArea(Of CartesianArea)().Orientation = Orientation.Horizontal
				grid.DrawVerticalStripes = True
				grid.DrawHorizontalStripes = False
			Else
				Me.radChartView1.GetArea(Of CartesianArea)().Orientation = Orientation.Vertical
				grid.DrawVerticalStripes = False
				grid.DrawHorizontalStripes = True
			End If
		End Sub

		Private Sub radCheckBoxSpline_ToggleStateChanged(ByVal sender As Object, ByVal args As StateChangedEventArgs)
			Dim range As RangeSeries = TryCast(Me.radChartView1.Series(0), RangeSeries)

			If Not range Is Nothing Then
				range.Spline = Not range.Spline
			End If
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler Me.radDropDownListChartType.SelectedIndexChanged, AddressOf radDropDownListChartType_SelectedIndexChanged
			AddHandler Me.orientationCheckBox.ToggleStateChanged, AddressOf orientationCheckBox_ToggleStateChanged
			AddHandler Me.radCheckBoxSpline.ToggleStateChanged, AddressOf radCheckBoxSpline_ToggleStateChanged
		End Sub
	End Class
End Namespace

