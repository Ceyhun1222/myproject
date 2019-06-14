Imports Telerik.WinControls.UI

Public Class MyRulerPrimitive
    Inherits RulerPrimitive
    Public Sub New(scheduler As RadScheduler, area As DayViewAppointmentsArea)
        MyBase.New(scheduler, area)
        Me.RulerRenderer = New MyRullerRender(Me)
    End Sub

    Protected Overrides ReadOnly Property ThemeEffectiveType() As Type
        Get
            Return GetType(RulerPrimitive)
        End Get
    End Property
End Class

Public Class MyRullerRender
    Inherits RulerRenderer
    Public Sub New(ruler As RulerPrimitive)
        MyBase.New(ruler)
    End Sub

    Public Overrides Sub RenderHour(g As Telerik.WinControls.Paint.IGraphics, hour As Integer, bounds As RectangleF)
        If hour < 9 OrElse hour > 17 Then
            g.FillRectangle(bounds, ColorTranslator.FromHtml("#EBECE8"))
        Else
            g.FillRectangle(bounds, ColorTranslator.FromHtml("#E6F6A5"))
        End If
        MyBase.RenderHour(g, hour, bounds)
    End Sub

    Public Overrides Sub RenderSubHour(g As Telerik.WinControls.Paint.IGraphics, hour As Integer, sectionIndex As Integer, bounds As RectangleF)
        If hour < 9 OrElse hour > 17 Then
            g.FillRectangle(bounds, ColorTranslator.FromHtml("#EBECE8"))
        Else
            g.FillRectangle(bounds, ColorTranslator.FromHtml("#E6F6A5"))
        End If
        MyBase.RenderSubHour(g, hour, sectionIndex, bounds)
    End Sub
End Class
