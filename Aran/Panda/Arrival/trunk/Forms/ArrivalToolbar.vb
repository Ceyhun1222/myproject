Public Class ArrivalToolbar
	Private StripButtons As ToolStripButton()

	Public ReadOnly OAS23 As Integer = 0
	Public ReadOnly wOAS As Integer = 1
	Public ReadOnly ILS As Integer = 2
	Public ReadOnly OFZ As Integer = 3

	Public Property isEnabled(ByVal index As Integer) As Boolean
		Get
			Dim N As Integer
			N = UBound(StripButtons)
			If index > N Then Return False
			Return StripButtons(index).Enabled
		End Get

		Set(ByVal value As Boolean)
			Dim N As Integer
			N = UBound(StripButtons)
			If index > N Then Return
			StripButtons(index).Enabled = value
		End Set
	End Property

	Public Sub New()
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		ReDim StripButtons(3)
		StripButtons(0) = tsBtnOAS_Cat2
		StripButtons(1) = tsBtnOAS_Cat1
		StripButtons(2) = tsBtnBasic_ILS
		StripButtons(3) = tsBtnOFZ

		RefresBar(0)
    End Sub

    Public ReadOnly Property Toolbar() As ToolStrip
        Get
            Return ToolStrip1
        End Get
    End Property


	Public Sub RefresBar(ByVal enables As Integer)
		Dim I, M, N As Integer
		N = UBound(StripButtons)
		M = 1

		For I = 0 To N
			StripButtons(I).Enabled = (enables And M) <> 0
			M += M
		Next
	End Sub

	Private Sub tsBtnOAS_Cat1_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnOAS_Cat1.Click
		Dim I As Integer
		Dim N As Integer
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		OASPlanesCat1State = tsBtnOAS_Cat1.Checked
		N = UBound(OASPlanesCat1Element)
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If OASPlanesCat1State Then
			For I = 0 To N
				pGraphics.AddElement(OASPlanesCat1Element(I), 0)
			Next I
		Else
			For I = 0 To N
				If Not OASPlanesCat1Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat1Element(I))
			Next I
		End If

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		On Error GoTo 0
	End Sub

	Private Sub tsBtnOAS_Cat2_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnOAS_Cat2.Click
		Dim I As Integer
		Dim N As Integer
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		OASPlanesCat23State = tsBtnOAS_Cat2.Checked
		N = UBound(OASPlanesCat23Element)
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If OASPlanesCat23State Then
			For I = 0 To N
				pGraphics.AddElement(OASPlanesCat23Element(I), 0)
			Next I
		Else
			For I = 0 To N
				If Not OASPlanesCat23Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat23Element(I))
			Next I
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		On Error GoTo 0
	End Sub

	Private Sub tsBtnBasic_ILS_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnBasic_ILS.Click
		Dim I As Integer
		Dim N As Integer
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		ILSPlanesState = tsBtnBasic_ILS.Checked
		pGraphics = GetActiveView().GraphicsContainer
		N = UBound(ILSPlanesElement)

		On Error Resume Next
		If ILSPlanesState Then
			For I = 0 To N
				pGraphics.AddElement(ILSPlanesElement(I), 0)
			Next I
		Else
			For I = 0 To N
				If Not ILSPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(ILSPlanesElement(I))
			Next I
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		On Error GoTo 0
	End Sub

	Private Sub tsBtnOFZ_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnOFZ.Click
		Dim I As Integer
		Dim N As Integer
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		OFZPlanesState = tsBtnOFZ.Checked
		pGraphics = GetActiveView().GraphicsContainer
		N = UBound(OFZPlanesElement)

		On Error Resume Next
		If OFZPlanesState Then
			For I = 0 To N
				pGraphics.AddElement(OFZPlanesElement(I), 0)
			Next I
		Else
			For I = 0 To N
				If Not OFZPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(OFZPlanesElement(I))
			Next I
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		On Error GoTo 0
	End Sub

End Class