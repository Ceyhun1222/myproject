Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Scatter
	Public Class HourlyEarnings
        Private m_sector As String
        Private m_wage As Double
        Private m_age As Integer

        Public Sub New(ByVal sector As String, ByVal wage As Double, ByVal age As Integer)
            Me.m_sector = sector
            Me.m_wage = wage
            Me.m_age = age
        End Sub

        Public ReadOnly Property Sector() As String
            Get
                Return Me.m_sector
            End Get
        End Property

        Public ReadOnly Property Wage() As Double
            Get
                Return Me.m_wage
            End Get
        End Property

        Public ReadOnly Property Age() As Integer
            Get
                Return Me.m_age
            End Get
        End Property
	End Class
End Namespace
