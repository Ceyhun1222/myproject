Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.CartesianAnnotations
    Public Class CompanyEvent
        Private m_eventDescription As String
        Private m_timeOfEvent As DateTime
        Private m_offset As SizeF

        Public Sub New(ByVal timeOfEvent As DateTime, ByVal eventDescription As String, ByVal offset As SizeF)
            Me.m_timeOfEvent = timeOfEvent
            Me.m_eventDescription = eventDescription
            Me.m_offset = offset
        End Sub

        Public Property EventDescription() As String
            Get
                Return m_eventDescription
            End Get
            Set(value As String)
                m_eventDescription = Value
            End Set
        End Property

        Public Property TimeOfEvent() As DateTime
            Get
                Return m_timeOfEvent
            End Get
            Set(value As DateTime)
                m_timeOfEvent = Value
            End Set
        End Property

        Public Property Offset() As SizeF
            Get
                Return m_offset
            End Get
            Set(value As SizeF)
                m_offset = Value
            End Set
        End Property
    End Class
End Namespace
