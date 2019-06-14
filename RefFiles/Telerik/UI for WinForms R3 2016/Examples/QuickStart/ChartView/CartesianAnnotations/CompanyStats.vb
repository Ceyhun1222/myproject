Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.CartesianAnnotations
    Public Class CompanyStats
        Private m_name As String
        Private m_date As DateTime
        Private m_value As Double

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = Value
            End Set
        End Property

        Public Property [Date]() As DateTime
            Get
                Return m_date
            End Get
            Set(value As DateTime)
                m_date = Value
            End Set
        End Property

        Public Property Value() As Double
            Get
                Return Me.m_value
            End Get
            Set(value As Double)
                Me.m_value = Value
            End Set
        End Property
    End Class
End Namespace
