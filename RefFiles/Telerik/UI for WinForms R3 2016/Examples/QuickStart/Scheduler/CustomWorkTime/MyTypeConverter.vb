Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text

Namespace Telerik.Examples.WinControls.Scheduler.CustomWorkTime
    Class MyTypeConverter
    Inherits TypeConverter
        Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
            If sourceType.Equals(GetType(DateTime)) Then
                Return True
            End If

            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object) As Object
            If TypeOf value Is DateTime Then
                Return DirectCast(value, DateTime).TimeOfDay
            End If

            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType.Equals(GetType(String)) OrElse destinationType.Equals(GetType(TimeSpan)) OrElse destinationType.Equals(GetType(DateTime)) Then
                Return True
            End If

            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As System.Globalization.CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType.Equals(GetType(String)) AndAlso TypeOf context Is Telerik.WinControls.UI.GridDataCellElement Then
                Dim cell As Telerik.WinControls.UI.GridDataCellElement = DirectCast(context, Telerik.WinControls.UI.GridDataCellElement)
                Dim span As TimeSpan = DirectCast(value, TimeSpan)

                Return span.Hours.ToString("00") + ":" + span.Minutes.ToString("00")
            End If

            If destinationType.Equals(GetType(TimeSpan)) Then
                Return value
            End If

            If destinationType.Equals(GetType(DateTime)) Then
                Dim dt As DateTime
                If DateTime.TryParse(value.ToString(), dt) Then
                    Return dt
                End If
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class
End Namespace
