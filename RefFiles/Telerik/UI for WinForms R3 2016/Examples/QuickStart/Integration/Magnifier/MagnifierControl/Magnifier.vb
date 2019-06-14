Imports System.Text
Imports Telerik.WinControls
Imports System.ComponentModel

Namespace Telerik.Examples.WinControls.Integration.Magnifier
	Public Class Magnifier
		Inherits RadControl
'INSTANT VB NOTE: The variable element was renamed since Visual Basic does not allow class members with the same name:
		Private element_Renamed As MagnifierElement
		Private timer As Timer
		Private mousePos As Point
'INSTANT VB NOTE: The variable zoomFactor was renamed since Visual Basic does not allow class members with the same name:
		Private zoomFactor_Renamed As Single = 3

		Public Sub New()
			timer = New Timer()
			AddHandler timer.Tick, AddressOf timer_Tick
			timer.Interval = 16
		End Sub

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				timer.Stop()
				RemoveHandler timer.Tick, AddressOf timer_Tick
				timer.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		<RadEditItemsAction, DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public ReadOnly Property Items() As RadItemOwnerCollection
			Get
				Return Element.Items
			End Get
		End Property

		<DefaultValue(3)> _
		Public Property Columns() As Integer
			Get
				Return Element.Layout.Columns
			End Get
			Set(ByVal value As Integer)
				Element.Layout.Columns = value
			End Set
		End Property

		Public Property DefaultCellSize() As Size
			Get
				Return Element.Layout.DefaultCellSize
			End Get
			Set(ByVal value As Size)
				Element.Layout.DefaultCellSize = value
			End Set
		End Property

		<DefaultValue(3f)> _
		Public Property ZoomFactor() As Single
			Get
				Return zoomFactor_Renamed
			End Get
			Set(ByVal value As Single)
				zoomFactor_Renamed = value
			End Set
		End Property

		<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
		Public ReadOnly Property Element() As MagnifierElement
			Get
				Return Me.element_Renamed
			End Get
		End Property

		Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
			MyBase.OnMouseEnter(e)
			timer.Start()
		End Sub

		Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
			MyBase.OnMouseLeave(e)
			timer.Stop()
			For Each element As RadElement In Me.Element.Layout.Children
				If element.ScaleTransform.Width <> 1f OrElse element.ScaleTransform.Height <> 1f Then
					element.ScaleTransform = New SizeF(1f, 1f)
				End If
			Next element
		End Sub

		Private Sub timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			Dim point As Point = PointToClient(Control.MousePosition)
			If mousePos <> point Then
				Dim maxFactor As Single = 0f
				Dim topMostCandidate As RadElement = Nothing
				For i As Integer = 0 To Element.Layout.Children.Count - 1
					Dim element As RadElement = Me.Element.Layout.Children(i)
					Dim column As Integer = i Mod Me.Element.Layout.Columns
					Dim row As Integer = i \ Me.Element.Layout.Columns

					element.ZIndex = 0

					Dim location As Point = element.BoundingRectangle.Location
					location.Offset(CInt(Fix(Me.Element.Viewport.PositionOffset.Width)), CInt(Fix(Me.Element.Viewport.PositionOffset.Height)))
					Dim dx As Double = location.X + element.BoundingRectangle.Width / 2f - point.X
					Dim dy As Double = location.Y + element.BoundingRectangle.Height / 2f - point.Y
					Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)
					Dim rad As Double = 150

					If dist < rad Then
						Dim factor As Single = CSng(Math.Cos(.5 * Math.PI * Math.Abs(dist / rad))) * zoomFactor_Renamed
						If factor >= 1f Then
							element.ScaleTransform = New SizeF(factor, factor)
						End If

						If maxFactor < factor Then
							maxFactor = Math.Max(maxFactor, factor)
							topMostCandidate = element
						End If
					Else
						element.ScaleTransform = New SizeF(1f, 1f)
					End If

					Dim cellSize As Size = Me.Element.Layout.DefaultCellSize
				Next i

				If topMostCandidate IsNot Nothing Then
					topMostCandidate.ZIndex = 1
				End If
			End If
			mousePos = point
		End Sub

		Protected Overrides Sub CreateChildItems(ByVal parent As RadElement)
			Me.element_Renamed = New MagnifierElement()
			parent.Children.Add(Me.element_Renamed)
		End Sub
	End Class
End Namespace
