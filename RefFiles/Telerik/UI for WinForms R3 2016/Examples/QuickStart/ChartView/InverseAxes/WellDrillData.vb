Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.InverseAxes
	Public Class WellDrillData

        Private m_date As DateTime
        Private m_depth As Double

        Public Sub New(ByVal drillDate As DateTime, ByVal depth As Double)
            m_date = drillDate
            m_depth = depth
        End Sub

        Public Property [Date] As DateTime
            Get
                Return m_date
            End Get
            Set(ByVal value As DateTime)
                m_date = value
            End Set
        End Property

        Public Property Depth() As Double
            Get
                Return m_depth
            End Get
            Set(ByVal value As Double)
                m_depth = value
            End Set
        End Property
    End Class
End Namespace
