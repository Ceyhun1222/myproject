Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.NullValues


    Public Class FootballTeam
        Private _name As String
        Private _logoPath As String
        Private _totalWins As System.Nullable(Of Integer)
        Private _totalDraws As System.Nullable(Of Integer)
        Private _totalLosses As System.Nullable(Of Integer)
        Private _stats As List(Of FootballTeamStats)

        Public Property Stats() As List(Of FootballTeamStats)
            Get
                Return Me._stats
            End Get
            Set(value As List(Of FootballTeamStats))
                Me._stats = value
            End Set
        End Property

        Public Property TotalLosses() As System.Nullable(Of Integer)
            Get
                Return Me._totalLosses
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._totalLosses = value
            End Set
        End Property

        Public Property TotalDraws() As System.Nullable(Of Integer)
            Get
                Return Me._totalDraws
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._totalDraws = value
            End Set
        End Property

        Public Property TotalWins() As System.Nullable(Of Integer)
            Get
                Return Me._totalWins
            End Get
            Set(value As System.Nullable(Of Integer))
                Me._totalWins = value
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

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property
    End Class
End Namespace