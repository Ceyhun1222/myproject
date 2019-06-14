Imports Microsoft.VisualBasic
Imports System

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Range
	Public Class WeatherData
        Private m_time As DateTime
        Private m_low As Double
        Private m_high As Double

        Public Sub New(ByVal time As DateTime, ByVal low As Double, ByVal high As Double)
            Me.m_time = time
            Me.m_low = low
            Me.m_high = high
        End Sub

		Public ReadOnly Property Time() As DateTime
			Get
                Return m_time
			End Get
		End Property

		Public ReadOnly Property Low() As Double
			Get
                Return m_low
			End Get
		End Property

		Public ReadOnly Property High() As Double
			Get
                Return m_high
			End Get
		End Property
	End Class
End Namespace
