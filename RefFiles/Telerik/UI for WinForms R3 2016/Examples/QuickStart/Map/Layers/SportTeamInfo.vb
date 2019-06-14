
Imports System.Collections.Generic
Imports System.Text
Imports Telerik.WinControls.UI.Map

Namespace Telerik.Examples.WinControls.Map.Layers
    Public Class SportTeamInfo
        Private m_location As PointG
        Private m_city As String
        Private m_name As String
        Private m_arena As String

        Public Sub New(location As PointG, city As String, name As String, arena As String)
            Me.m_location = location
            Me.m_city = city
            Me.m_name = name
            Me.m_arena = arena
        End Sub

        Public Property Location() As PointG
            Get
                Return m_location
            End Get
            Set(value As PointG)
                m_location = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return m_city
            End Get
            Set(value As String)
                m_city = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property

        Public Property Arena() As String
            Get
                Return m_arena
            End Get
            Set(value As String)
                m_arena = value
            End Set
        End Property
    End Class
End Namespace

