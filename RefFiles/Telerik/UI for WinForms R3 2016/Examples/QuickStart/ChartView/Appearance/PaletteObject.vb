Imports System.ComponentModel
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.ChartView.Appearance
    Public Class PaletteObject
        Implements INotifyPropertyChanged
#Region "Fields"

        Private m_name As String
        Private m_palette As ChartPalette

#End Region

#Region "Constructor"
        Public Sub New(ByVal name As String, ByVal palette As ChartPalette)
            Me.m_name = name
            Me.m_palette = palette
        End Sub
#End Region

#Region "Properties"

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                If Me.m_name = value Then
                    Return
                End If

                Me.m_name = value
                Me.OnNotifyPropertyChanged("Name")
            End Set
        End Property

        Public Property Palette() As ChartPalette
            Get
                Return Me.m_palette
            End Get
            Set(ByVal value As ChartPalette)
                If Me.m_palette Is value Then
                    Return
                End If

                Me.m_palette = value
                Me.OnNotifyPropertyChanged("Palette")
            End Set
        End Property

#End Region

#Region "INotifyPropertyChanged Implementation"

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub OnNotifyPropertyChanged(ByVal [property] As String)
            If Me.PropertyChangedEvent IsNot Nothing Then
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs([property]))
            End If
        End Sub

#End Region
    End Class
End Namespace
