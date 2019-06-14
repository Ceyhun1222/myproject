Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Resources
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Map

Namespace Telerik.Examples.WinControls.Map.OpenStreetMaps
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()

            Me.SetupProviders()
            Me.SetupLayer()
            Me.SetupData()
        End Sub

        Private Sub SetupProviders()
            Dim osmProvider As New OpenStreetMapProvider()
            AddHandler osmProvider.InitializationComplete, AddressOf OsmProvider_InitializationComplete

            Me.radMap1.MapElement.Providers.Add(osmProvider)
        End Sub

        Private Sub OsmProvider_InitializationComplete(sender As Object, e As System.EventArgs)
            Me.radMap1.Pan(New SizeL(-240, -440))
        End Sub

        Private Sub SetupLayer()
            Dim pinsLayer As New MapLayer("Pins")
            Me.radMap1.Layers.Add(pinsLayer)
        End Sub

        Private Sub SetupData()
            Me.radDropDownListClusterStrategy.Items.Add(New RadListDataItem("Element cluster strategy", New ElementClusterStrategy()))
            Me.radDropDownListClusterStrategy.Items.Add(New RadListDataItem("Distance cluster strategy", New DistanceClusterStrategy()))
            Me.radDropDownListClusterStrategy.SelectedIndex = 0

            Me.radMap1.BeginUpdate()

            Using reader As New StringReader(My.Resources.PhotoSpots)
                Dim line As String = Nothing

                While InlineAssignHelper(line, reader.ReadLine()) IsNot Nothing
                    Dim coordinates As String() = line.Split(","c)
                    Dim latitude As Double = Double.Parse(coordinates(0))
                    Dim longitude As Double = Double.Parse(coordinates(1))

                    Dim pin As New MapPin(New PointG(latitude, longitude))
                    pin.BackColor = Color.FromArgb(37, 160, 218)
                    Me.radMap1.Layers("Pins").Add(pin)
                End While
            End Using

            Me.radMap1.EndUpdate()
        End Sub

        Private Sub radDropDownListClusterStrategy_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            If e.Position > -1 Then
                Me.radMap1.Layers(0).ClusterStrategy = DirectCast(Me.radDropDownListClusterStrategy.SelectedItem.Value, IMapClusterStrategy)
            End If
        End Sub

        Private Sub radSpinEditorDistance_ValueChanged(sender As Object, e As System.EventArgs)
            Me.radMap1.Layers("Pins").ClusterDistance = CLng(Me.radSpinEditorDistance.Value)
        End Sub
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace