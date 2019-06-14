Imports Microsoft.VisualBasic
Imports System
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Waterfall
	
	Public Partial Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			Me.SelectedControl = Me.radChartView1
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Dim series As WaterfallSeries = New WaterfallSeries("Value", "Summary", "Total", "Category")
			series.ShowLabels = True
			series.DataSource = DataModel.GetData()

			Me.radChartView1.Series.Add(series)
		End Sub
	End Class
End Namespace

