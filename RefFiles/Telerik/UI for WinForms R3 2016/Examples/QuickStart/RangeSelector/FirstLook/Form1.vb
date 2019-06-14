
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.RangeSelector.FirstLook
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            InitializeComponent()

            TryCast(Me.radRangeSlider1.RangeSelectorElement.AssociatedElement, RangeSelectorViewElement).ScalesPosition = ViewPosition.BottomRight

            Me.radChartView1.View.Margin = New Padding(0)
        End Sub
    End Class
End Namespace