Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.NullValues
    Public Class FootballTeamStats
        Private _name As String
        Private _logoPath As String
        Private _season As String
        Private _wins As System.Nullable(Of Integer)
        Private _draws As System.Nullable(Of Integer)
        Private _losses As System.Nullable(Of Integer)
        Private _goalDifference As System.Nullable(Of Integer)
        Private _points As System.Nullable(Of Integer)
        Private _position As System.Nullable(Of Integer)

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        Public Property LogoPath() As String
            Get
                Return Me._logoPath
            End Get
            Set(value As String)
                Me._logoPath = value
            End Set
        End Property

        Public Property Season() As String
            Get
                Return Me._season
            End Get
            Set(value As String)
                Me._season = value
            End Set
        End Property

        Public Property Wins() As System.Nullable(Of Integer)
            Get
                Return Me._wins
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._wins = value
            End Set
        End Property

        Public Property Draws() As System.Nullable(Of Integer)
            Get
                Return Me._draws
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._draws = value
            End Set
        End Property

        Public Property Losses() As System.Nullable(Of Integer)
            Get
                Return Me._losses
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._losses = value
            End Set
        End Property

        Public Property GoalDifference() As System.Nullable(Of Integer)
            Get
                Return Me._goalDifference
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._goalDifference = value
            End Set
        End Property

        Public Property Points() As System.Nullable(Of Integer)
            Get
                Return Me._points
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._points = value
            End Set
        End Property

        Public Property Position() As System.Nullable(Of Integer)
            Get
                Return Me._position
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._position = value
            End Set
        End Property
    End Class
End Namespace