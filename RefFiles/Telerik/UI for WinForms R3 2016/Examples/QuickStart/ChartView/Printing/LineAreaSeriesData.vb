Namespace Telerik.Examples.WinControls.ChartView.Printing
    Public Class LineAreaSeriesData
        Private m_month As String
        Private m_profit As Double

        Public Sub New(ByVal profit As Double, ByVal month As String)
            Me.m_profit = profit
            Me.m_month = month
        End Sub

        Public ReadOnly Property Month() As String
            Get
                Return Me.m_month
            End Get
        End Property

        Public ReadOnly Property Profit() As Double
            Get
                Return Me.m_profit
            End Get
        End Property
    End Class
End Namespace
