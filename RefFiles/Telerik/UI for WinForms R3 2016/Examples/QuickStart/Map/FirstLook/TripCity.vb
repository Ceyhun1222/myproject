Imports Telerik.WinControls.UI.Map

Namespace Telerik.Examples.WinControls.Map.FirstLook
    Public Class TripCity
        Private m_location As PointG
        Private m_name As String
        Private m_tripStop As Integer
        Private m_isFlight As Boolean

        Public Sub New(name As String, location As PointG, tripStop As Integer, isFlight As Boolean)
            Me.m_name = name
            Me.m_location = location
            Me.m_tripStop = tripStop
            Me.m_isFlight = isFlight
        End Sub

        Public Property Location() As PointG
            Get
                Return m_location
            End Get
            Set(value As PointG)
                Me.m_location = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                Me.m_name = value
            End Set
        End Property

        Public Property TripStop() As Integer
            Get
                Return m_tripStop
            End Get
            Set(value As Integer)
                Me.m_tripStop = value
            End Set
        End Property

        Public Property IsFlight() As Boolean
            Get
                Return m_isFlight
            End Get
            Set(value As Boolean)
                Me.m_isFlight = value
            End Set
        End Property
    End Class
End Namespace