Imports System.Text

Namespace Telerik.Examples.WinControls.PivotGrid
	Public Class Order2
'INSTANT VB NOTE: The variable net was renamed since Visual Basic does not allow class members with the same name:
		Private net_Renamed As Double

		Public Sub New()
		End Sub

		Private privateDate As Date
		Public Property [Date]() As Date
			Get
				Return privateDate
			End Get
			Set(ByVal value As Date)
				privateDate = value
			End Set
		End Property
		Private privateProduct As String
		Public Property Product() As String
			Get
				Return privateProduct
			End Get
			Set(ByVal value As String)
				privateProduct = value
			End Set
		End Property
		Private privateQuantity As Integer
		Public Property Quantity() As Integer
			Get
				Return privateQuantity
			End Get
			Set(ByVal value As Integer)
				privateQuantity = value
			End Set
		End Property

		Public Property Net() As Double
			Get
				If Me.net_Renamed = 1025.64 Then
					' throw new Exception("Example for error value");
				End If
				Return Me.net_Renamed
			End Get
			Set(ByVal value As Double)
				Me.net_Renamed = value
			End Set
		End Property

		Private privatePromotion As String
		Public Property Promotion() As String
			Get
				Return privatePromotion
			End Get
			Set(ByVal value As String)
				privatePromotion = value
			End Set
		End Property
		Private privateAdvertisement As String
		Public Property Advertisement() As String
			Get
				Return privateAdvertisement
			End Get
			Set(ByVal value As String)
				privateAdvertisement = value
			End Set
		End Property

		Public Overrides Function ToString() As String
			Return Me.Product & Me.Promotion & Me.Quantity
		End Function
	End Class
End Namespace
