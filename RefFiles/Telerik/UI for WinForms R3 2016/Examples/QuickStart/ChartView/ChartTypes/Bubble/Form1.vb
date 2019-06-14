Imports Telerik.WinControls.UI
Imports Telerik.Charting

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Bubble
    Public Class Form1

        Public Sub New()

            InitializeComponent()

            Me.SelectedControl = Me.RadChartView1

        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)

            Dim europe As New BubbleSeries()
            Dim northAmeruca As New BubbleSeries()
            Dim middleEast As New BubbleSeries()

            europe.XValueMember = "LifeExpectancy"
            europe.YValueMember = "FertilityRate"
            europe.ValueMember = "Population"
            europe.DataMember = "Europe"
            europe.AutoScaleMaxWidth = 150
            europe.LegendTitle = "Europe"

            northAmeruca.XValueMember = "LifeExpectancy"
            northAmeruca.YValueMember = "FertilityRate"
            northAmeruca.ValueMember = "Population"
            northAmeruca.DataMember = "NorthAmerica"
            northAmeruca.AutoScaleMaxWidth = 150
            northAmeruca.LegendTitle = "North America"

            middleEast.XValueMember = "LifeExpectancy"
            middleEast.YValueMember = "FertilityRate"
            middleEast.ValueMember = "Population"
            middleEast.DataMember = "MiddleEast"
            middleEast.AutoScaleMaxWidth = 150
            middleEast.LegendTitle = "Middle East"

            Me.RadChartView1.Series.Add(europe)
            Me.RadChartView1.Series.Add(northAmeruca)
            Me.RadChartView1.Series.Add(middleEast)

            Me.RadChartView1.ShowLegend = True
            Me.RadChartView1.DataSource = DataModel.GetData()

            Dim tooltip As New ChartTooltipController()
            AddHandler tooltip.DataPointTooltipTextNeeded, AddressOf tooltip_DataPointTooltipTextNeeded
            Me.RadChartView1.Controllers.Add(tooltip)
        End Sub

        Private Sub tooltip_DataPointTooltipTextNeeded(sender As Object, e As DataPointTooltipTextNeededEventArgs)
            Dim point As BubbleDataPoint = TryCast(e.DataPoint, BubbleDataPoint)
            Dim row As DataRowView = TryCast(point.DataItem, DataRowView)
            e.Text = String.Format("Country: {0}" & vbLf & "Life Expectancy: {1:F2}" & vbLf & "Fertility Rate: {2:F2}" & vbLf & "Region: {3}" & vbLf & "Population: {4}", row("Country"), point.XValue, point.YValue, row.Row.Table.TableName, point.Value)
        End Sub
    End Class
End Namespace