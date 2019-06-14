
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.SplitContainer.CollapsingPanels
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()

            Me.radSplitContainer1.EnableCollapsing = True
            Me.radCheckBox1.IsChecked = True

            Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList

            ExamplesForm.FillComboFromEnum(Me.radDropDownList1, GetType(Orientation), Me.radSplitContainer1.Orientation)
            Me.radSplitContainer1.UseSplitterButtons = True
            Me.radCheckBox2.Checked = True
        End Sub

        Protected Overrides Sub WireEvents()
            AddHandler Me.radDropDownList1.SelectedIndexChanged, AddressOf radDropDownList1_SelectedIndexChanged
            AddHandler Me.radCheckBox1.ToggleStateChanged, AddressOf radCheckBox1_ToggleStateChanged
            AddHandler Me.radCheckBox2.ToggleStateChanged, AddressOf radCheckBox2_ToggleStateChanged
        End Sub


        Private Sub radDropDownList1_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.radSplitContainer1.Orientation = DirectCast(Me.radDropDownList1.SelectedItem.Value, Orientation)
        End Sub

        Private Sub radCheckBox1_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs)
            Me.radSplitContainer1.EnableCollapsing = Me.radCheckBox1.IsChecked
            If Me.radCheckBox1.IsChecked AndAlso Me.radCheckBox2.IsChecked Then
                Me.radSplitContainer1.UseSplitterButtons = True
            End If

        End Sub

        Private Sub radCheckBox2_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs)
            Me.radSplitContainer1.UseSplitterButtons = Me.radCheckBox2.IsChecked
        End Sub
    End Class
End Namespace