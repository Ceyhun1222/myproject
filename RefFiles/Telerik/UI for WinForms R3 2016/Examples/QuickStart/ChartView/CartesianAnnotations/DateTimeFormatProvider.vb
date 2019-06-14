Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.ChartView.CartesianAnnotations
    Public Class DateTimeFormatProvider
        Implements IFormatProvider, ICustomFormatter
        Public Function GetFormat(ByVal formatType As Type) As Object Implements IFormatProvider.GetFormat
            Return Me
        End Function

        Public Function Format(ByVal formatString As String, ByVal arg As Object, ByVal formatProvider As IFormatProvider) As String Implements ICustomFormatter.Format
            Dim val As DateTime = CDate(arg)

            If val.Month = 1 Then
                Return val.ToString("MMM") & Environment.NewLine & val.Year
            Else
                Return val.ToString("MMM")
            End If
        End Function
    End Class
End Namespace
