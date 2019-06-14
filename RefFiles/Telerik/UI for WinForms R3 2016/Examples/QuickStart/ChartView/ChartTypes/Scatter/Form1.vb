Imports Telerik.Charting
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls
Imports Telerik.WinControls.Tests
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Scatter

	Partial Public Class Form1
		Inherits ExamplesForm
		Private customShape As CustomShape
		Private shapedForm As New ShapedForm()

		Public Sub New()
			InitializeComponent()

            Me.SelectedControl = Me.radChartView1

            Dim area As CartesianArea = Me.radChartView1.GetArea(Of CartesianArea)()
            area.ShowGrid = True
            Dim grid As CartesianGrid = area.GetGrid(Of CartesianGrid)()
            grid.DrawHorizontalStripes = True
            grid.DrawVerticalStripes = True

            InitializeChartAxes()
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

            Dim model As New ViewModel()
            Me.radChartView1.DataSource = model.GetData()
            Dim axes As LinearAxis() = Me.radChartView1.Axes.Get(Of LinearAxis)()

            Me.ChangeSeries("Scatter point")

            Me.Initialize()
		End Sub

		Private Sub Initialize()
            Me.radDropDownListSeriesType.Items.Add("Scatter point")
            Me.radDropDownListSeriesType.Items.Add("Scatter line")
            Me.radDropDownListSeriesType.Items.Add("Scatter area")
            Me.radDropDownListSeriesType.SelectedIndex = 0

            Me.customShape = New CustomShape()
            Me.customShape.CreateClosedShape(CreateInitialShape(5, 100, 60))

            Dim item As New RadListDataItem("CustomShape")
            item.Value = GetType(CustomShape)
            Me.radDropDownListShapes.Items.Add(item)
            item = New RadListDataItem("RoundRectShape")
            item.Value = GetType(RoundRectShape)
            Me.radDropDownListShapes.Items.Add(item)
            item = New RadListDataItem("EllipseShape")
            item.Value = GetType(EllipseShape)
            Me.radDropDownListShapes.Items.Add(item)
            item = New RadListDataItem("DonutShape")
            item.Value = GetType(DonutShape)
            Me.radDropDownListShapes.Items.Add(item)

            AddHandler Me.radDropDownListShapes.SelectedIndexChanged, AddressOf radDropDownListShapes_SelectedIndexChanged
            Me.radDropDownListShapes.SelectedIndex = 2

            Me.radSpinEditorPointRadius.Value = 6D

            Me.radChartView1.ShowTitle = True
            Me.radChartView1.Title = "Mean hourly earnings in the UK public and private sector (aged 16-64)"
            Me.radChartView1.ChartElement.TitleElement.Font = New Font("Segoe UI", 20.0F)

		End Sub

		Private Sub InitializeChartAxes()
            Dim area As CartesianArea = Me.radChartView1.GetArea(Of CartesianArea)()

            Dim horizontalAxis As New LinearAxis()
            horizontalAxis.LabelFitMode = AxisLabelFitMode.MultiLine
            horizontalAxis.Title = "Employee Age"
            horizontalAxis.Minimum = 16
            horizontalAxis.Maximum = 64
            horizontalAxis.MajorStep = 4
            area.Axes.Add(horizontalAxis)

            Dim verticalAxis As New LinearAxis()
            verticalAxis.AxisType = AxisType.Second
            verticalAxis.Title = "Earnings (GPB/hour)"
            verticalAxis.Minimum = 2
            verticalAxis.Maximum = 20
            area.Axes.Add(verticalAxis)
		End Sub

        Private Function CreateInitialShape(vertices As Integer, radius1 As Double, radius2 As Double) As List(Of PointF)
            Dim pts As New List(Of PointF)()

            If radius1 = 0 Then
                Return Nothing
            End If

            If radius2 = 0 Then
                Return Nothing
            End If

            For i As Integer = 0 To vertices - 1
                Dim angle1 As Double = ((4.0 * i - vertices) * Math.PI) / (2.0F * vertices)
                Dim angle2 As Double = ((4.0 * i - vertices + 2) * Math.PI) / (2.0F * vertices)
                pts.Add(New PointF(CSng(Math.Cos(angle1) * radius1), CSng(Math.Sin(angle1) * radius1)))
                pts.Add(New PointF(CSng(Math.Cos(angle2) * radius2), CSng(Math.Sin(angle2) * radius2)))
            Next

            Return pts
        End Function

        Private Sub ChangeSeries(seriesType As String)
            Me.radChartView1.Series.Clear()

            Dim seriesPublic As ScatterSeries
            Dim seriesPrivate As ScatterSeries
            Me.radCheckBoxSpline.Enabled = False

            If seriesType = "Scatter line" Then
                seriesPublic = New ScatterLineSeries() With { _
                    .Spline = Me.radCheckBoxSpline.IsChecked _
                }
                seriesPrivate = New ScatterLineSeries() With { _
                    .Spline = Me.radCheckBoxSpline.IsChecked _
                }
                Me.radCheckBoxSpline.Enabled = True
                Me.radSpinEditorPointRadius.Value = 0D

                seriesPublic.BorderColor = Color.FromArgb(27, 157, 222)
                seriesPrivate.BorderColor = Color.FromArgb(142, 196, 65)
            ElseIf seriesType = "Scatter area" Then
                seriesPublic = New ScatterAreaSeries() With { _
                     .Spline = Me.radCheckBoxSpline.IsChecked _
                }
                seriesPrivate = New ScatterAreaSeries() With { _
                     .Spline = Me.radCheckBoxSpline.IsChecked _
                }
                Me.radCheckBoxSpline.Enabled = True
                Me.radSpinEditorPointRadius.Value = 0D

                seriesPublic.BorderColor = Color.FromArgb(27, 157, 222)
                seriesPublic.BackColor = Color.FromArgb(100, 27, 157, 222)
                seriesPrivate.BorderColor = Color.FromArgb(142, 196, 65)
                seriesPrivate.BackColor = Color.FromArgb(100, 142, 196, 65)
            Else
                seriesPublic = New ScatterSeries()
                seriesPrivate = New ScatterSeries()
                Me.radSpinEditorPointRadius.Value = 6D

                seriesPublic.BackColor = Color.FromArgb(27, 157, 222)
                seriesPrivate.BackColor = Color.FromArgb(142, 196, 65)
            End If

            Dim axes As LinearAxis() = Me.radChartView1.Axes.[Get](Of LinearAxis)()

            seriesPublic.DataMember = "public"
            seriesPrivate.DataMember = "private"

            seriesPublic.XValueMember = "Age"
            seriesPrivate.XValueMember = "Age"
            seriesPublic.YValueMember = "Wage"
            seriesPrivate.YValueMember = "Wage"
            seriesPublic.HorizontalAxis = axes(0)
            seriesPrivate.HorizontalAxis = axes(0)
            seriesPublic.VerticalAxis = axes(1)
            seriesPrivate.VerticalAxis = axes(1)

            Me.radChartView1.Series.Add(seriesPublic)
            Me.radChartView1.Series.Add(seriesPrivate)

            ApplySelectedShape()
            ApplySelectedPointSize()
        End Sub

        Private Sub ApplyShapeToPoints(shape As ElementShape)
            For Each series As ScatterSeries In Me.radChartView1.Series
                For Each point As ScatterPointElement In series.Children
                    point.Shape = shape
                Next
            Next
        End Sub

        Private Sub ApplySelectedShape()
            Select Case Me.radDropDownListShapes.SelectedIndex
                Case 0
                    Me.ApplyShapeToPoints(Me.customShape)
                    Me.radButtonEditShape.Enabled = True
                    Exit Select
                Case 1
                    Me.ApplyShapeToPoints(New RoundRectShape(2))
                    Me.radButtonEditShape.Enabled = False
                    Exit Select
                Case 2
                    Me.ApplyShapeToPoints(New EllipseShape())
                    Me.radButtonEditShape.Enabled = False
                    Exit Select
                Case 3
                    Me.ApplyShapeToPoints(New DonutShape())
                    Me.radButtonEditShape.Enabled = False
                    Exit Select
                Case 4
                    Me.ApplyShapeToPoints(New TabOffice12Shape())
                    Me.radButtonEditShape.Enabled = False
                    Exit Select
                Case 5
                    Me.ApplyShapeToPoints(New TabVsShape())
                    Me.radButtonEditShape.Enabled = False
                    Exit Select
            End Select
        End Sub

        Private Sub ApplySelectedPointSize()
            For Each series As ScatterSeries In Me.radChartView1.Series
                series.PointSize = New SizeF(CSng(Me.radSpinEditorPointRadius.Value), CSng(Me.radSpinEditorPointRadius.Value))
            Next
        End Sub

        Private Sub radButtonEditShape_Click(sender As Object, e As EventArgs)
            If Me.radDropDownListShapes.SelectedIndex = 0 Then
                Dim editor As New CustomShapeEditorForm()
                Me.customShape = editor.EditShape(Me.customShape)
                Me.ApplyShapeToPoints(Me.customShape)
            End If
        End Sub

        Private Sub radSpinEditor1_ValueChanged(sender As Object, e As EventArgs)
            ApplySelectedPointSize()
        End Sub

        Private Sub radDropDownListShapes_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            ApplySelectedShape()
        End Sub

        Private Sub radDropDownListSeriesType_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.ChangeSeries(Me.radDropDownListSeriesType.Text)
        End Sub

        Private Sub radCheckBoxSpline_ToggleStateChanged(sender As Object, args As StateChangedEventArgs)
            For Each series As ScatterLineSeries In Me.radChartView1.Series
                series.Spline = args.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            Next
        End Sub

        Protected Overrides Sub WireEvents()
            AddHandler Me.radButtonEditShape.Click, AddressOf Me.radButtonEditShape_Click
            AddHandler Me.radSpinEditorPointRadius.ValueChanged, AddressOf Me.radSpinEditor1_ValueChanged
            AddHandler Me.radDropDownListSeriesType.SelectedIndexChanged, AddressOf radDropDownListSeriesType_SelectedIndexChanged
            AddHandler Me.radCheckBoxSpline.ToggleStateChanged, AddressOf radCheckBoxSpline_ToggleStateChanged
        End Sub
	End Class
End Namespace

