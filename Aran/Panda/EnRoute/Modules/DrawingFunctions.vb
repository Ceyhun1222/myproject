Option Strict Off
'Option Explicit On

Imports ESRI.ArcGIS
Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Module DrawingFunctions

	'	Public Sub ClearScr()
	'		Dim pGraphics As Carto.IGraphicsContainer
	'		Dim pElement As Carto.IElement
	'		Dim pActiveView As Carto.IActiveView

	'		pActiveView = GetActiveView()
	'		pGraphics = pActiveView.GraphicsContainer

	'		On Error GoTo Errnamed

	'		pGraphics.Reset()
	'		pElement = pGraphics.Next

	'		While Not pElement Is Nothing
	'			If pElement.Locked Then pGraphics.DeleteElement(pElement)

	'			'    If (pElement.Geometry.GeometryType = esriGeometryPoint) Or _
	'			''        (pElement.Geometry.GeometryType = esriGeometryPolygon) Or _
	'			''        (pElement.Geometry.GeometryType = esriGeometryPolyline) Then
	'			'        pGraphics.DeleteElement pElement
	'			'    End If
	'			pElement = pGraphics.Next
	'		End While

	'Errnamed:
	'		pActiveView.Refresh()
	'		On Error GoTo 0
	'	End Sub

	Public Function DrawPoint(ByVal pPoint As Geometry.Point, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
		Dim pMarkerShpElement As Carto.IMarkerElement
		Dim pElementofpPoint As Carto.IElement
		Dim pMarkerSym As Display.ISimpleMarkerSymbol
		Dim pRGB As Display.IRgbColor

		pMarkerShpElement = New Carto.MarkerElement()
		pElementofpPoint = pMarkerShpElement
		pElementofpPoint.Geometry = pPoint

		pRGB = New Display.RgbColor()
		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pMarkerSym = New Display.SimpleMarkerSymbol()
		pMarkerSym.Color = pRGB
		pMarkerSym.Size = 8
		pMarkerShpElement.Symbol = pMarkerSym

		If drawFlg Then
			GetActiveView().GraphicsContainer.AddElement(pElementofpPoint, 0)
			GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementofpPoint
	End Function

	Public Function DrawPointWithText(ByVal pPoint As Geometry.Point, ByVal sText As String, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
		Dim pRGB As Display.IRgbColor
		Dim pMarkerShpElement As Carto.IMarkerElement
		Dim pElementofpPoint As Carto.IElement
		Dim pMarkerSym As Display.ISimpleMarkerSymbol

		Dim pTextElement As Carto.ITextElement
		Dim pElementOfText As Carto.IElement
		Dim pGroupElement As Carto.IGroupElement
		Dim pCommonElement As Carto.IElement
		Dim pTextSymbol As Display.ITextSymbol

		pTextElement = New Carto.TextElement
		pElementOfText = pTextElement

		pTextSymbol = New Display.TextSymbol
		pTextSymbol.HorizontalAlignment = Display.esriTextHorizontalAlignment.esriTHALeft
		pTextSymbol.VerticalAlignment = Display.esriTextVerticalAlignment.esriTVABottom

		pTextElement.Text = sText
		pTextElement.ScaleText = False
		pTextElement.Symbol = pTextSymbol

		pElementOfText.Geometry = pPoint

		pMarkerShpElement = New Carto.MarkerElement

		pElementofpPoint = pMarkerShpElement
		pElementofpPoint.Geometry = pPoint

		pRGB = New Display.RgbColor
		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pMarkerSym = New Display.SimpleMarkerSymbol
		pMarkerSym.Color = pRGB
		pMarkerSym.Size = 6
		pMarkerShpElement.Symbol = pMarkerSym

		pGroupElement = New Carto.GroupElement
		pGroupElement.AddElement(pElementofpPoint)
		pGroupElement.AddElement(pTextElement)

		pCommonElement = pGroupElement

		If drawFlg Then
			GetActiveView().GraphicsContainer.AddElement(pCommonElement, 0)
			GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pCommonElement
	End Function

	'Function DrawLine(ByVal pLine As Geometry.Line, Optional ByVal Color As Integer = -1, Optional ByVal Width As Double = 1.0, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
	'	Dim pLineElement As Carto.ILineElement
	'	Dim pElementOfpLine As Carto.IElement
	'	Dim pLineSym As Display.ISimpleLineSymbol
	'	Dim pRGB As Display.IRgbColor
	'	Dim pPolyline As Geometry.IPolyline

	'	pRGB = New Display.RgbColor
	'	If Color <> -1 Then
	'		pRGB.RGB = Color
	'	Else
	'		pRGB.Red = System.Math.Round(255 * Rnd())
	'		pRGB.Green = System.Math.Round(255 * Rnd())
	'		pRGB.Blue = System.Math.Round(255 * Rnd())
	'	End If

	'	pLineSym = New Display.SimpleLineSymbol
	'	pLineSym.Color = pRGB
	'	pLineSym.Style = Display.esriSimpleLineStyle.esriSLSSolid
	'	pLineSym.Width = Width

	'	Dim plyLine As Geometry.ILine
	'	plyLine = pLine

	'	pPolyline = New Geometry.Polyline
	'	pPolyline.FromPoint = plyLine.FromPoint
	'	pPolyline.ToPoint = plyLine.ToPoint

	'	pElementOfpLine = New Carto.LineElement
	'	pElementOfpLine.Geometry = pPolyline

	'	pLineElement = pElementOfpLine
	'	pLineElement.Symbol = pLineSym

	'	If drawFlg Then
	'		GetActiveView().GraphicsContainer.AddElement(pElementOfpLine, 0)
	'		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	'	End If

	'	Return pElementOfpLine
	'End Function

	'Function DrawLineSFS(ByVal pLine As Geometry.Line, ByVal pLineSym As Display.ISimpleLineSymbol, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
	'	Dim pLineElement As Carto.ILineElement
	'	Dim pElementOfpLine As Carto.IElement
	'	Dim pPolyline As Geometry.IPolyline
	'	Dim plyLine As Geometry.ILine

	'	pLineElement = New Carto.LineElement
	'	pElementOfpLine = pLineElement
	'	pElementOfpLine.Geometry = pPolyline

	'	plyLine = pLine

	'	pPolyline = New Geometry.Polyline
	'	pPolyline.FromPoint = plyLine.FromPoint
	'	pPolyline.ToPoint = plyLine.ToPoint

	'	pLineElement.Symbol = pLineSym

	'	If drawFlg Then
	'		GetActiveView().GraphicsContainer.AddElement(pElementOfpLine, 0)
	'		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	'	End If

	'	Return pElementOfpLine
	'End Function

	Public Function DrawPolyLine(ByVal pPolyline As Geometry.Polyline, Optional ByVal Color As Integer = -1, Optional ByVal Width As Double = 1.0, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
		Dim pLineElement As Carto.ILineElement
		Dim pElementOfpLine As Carto.IElement
		Dim pLineSym As Display.ISimpleLineSymbol
		Dim pRGB As Display.IRgbColor

		pRGB = New Display.RgbColor
		pLineSym = New Display.SimpleLineSymbol

		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pLineSym.Color = pRGB
		pLineSym.Style = Display.esriSimpleLineStyle.esriSLSSolid
		pLineSym.Width = Width

		pLineElement = New Carto.LineElement
		pLineElement.Symbol = pLineSym

		pElementOfpLine = pLineElement
		pElementOfpLine.Geometry = pPolyline

		If drawFlg Then
			GetActiveView().GraphicsContainer.AddElement(pElementOfpLine, 0)
			GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementOfpLine
	End Function

	Public Function DrawPolyLineSFS(ByVal pPolyline As Geometry.Polyline, ByVal pLineSymbol As Display.ILineSymbol, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
		Dim pLineElement As Carto.ILineElement
		Dim pElementOfpLine As Carto.IElement

		pLineElement = New Carto.LineElement
		pLineElement.Symbol = pLineSymbol

		pElementOfpLine = pLineElement
		pElementOfpLine.Geometry = pPolyline

		If drawFlg Then
			GetActiveView().GraphicsContainer.AddElement(pElementOfpLine, 0)
			GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementOfpLine
	End Function

	'Public Function DrawRing(ByVal pRing As Geometry.Ring, Optional ByVal Color As Integer = -1, Optional ByVal SimpleFillStyle As Display.esriSimpleFillStyle = Display.esriSimpleFillStyle.esriSFSNull, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
	'	Dim pRGB As Display.IRgbColor
	'	Dim pFillSym As Display.ISimpleFillSymbol
	'	Dim pFillShpElement As Carto.IFillShapeElement
	'	Dim pElementofPoly As Carto.IElement
	'	Dim pPolygon As Geometry.IGeometryCollection

	'	pRGB = New Display.RgbColor()
	'	If Color <> -1 Then
	'		pRGB.RGB = Color
	'	Else
	'		pRGB.Red = Math.Round(255 * Rnd())
	'		pRGB.Green = Math.Round(255 * Rnd())
	'		pRGB.Blue = Math.Round(255 * Rnd())
	'	End If

	'	pFillSym = New Display.SimpleFillSymbol
	'	pFillSym.Style = SimpleFillStyle
	'	pFillSym.Color = pRGB

	'	pFillShpElement = New Carto.PolygonElement()
	'	pFillShpElement.Symbol = pFillSym

	'	pElementofPoly = pFillShpElement

	'	pPolygon = New Geometry.Polygon()
	'	pPolygon.AddGeometry(pRing)
	'	pElementofPoly.Geometry = pPolygon

	'	If drawFlg Then
	'		GetActiveView().GraphicsContainer.AddElement(pElementofPoly, 0)
	'		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	'	End If

	'	Return pElementofPoly
	'End Function

	Public Function DrawPolygon(ByVal pPolygon As Geometry.Polygon, Optional ByVal Color As Integer = -1, Optional ByVal SimpleFillStyle As Display.esriSimpleFillStyle = Display.esriSimpleFillStyle.esriSFSNull, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
		Dim pRGB As Display.IRgbColor
		Dim pFillSym As Display.ISimpleFillSymbol
		Dim pFillShpElement As Carto.IFillShapeElement
		Dim pElementofPoly As Carto.IElement

		pRGB = New Display.RgbColor()
		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = Math.Round(255 * Rnd())
			pRGB.Green = Math.Round(255 * Rnd())
			pRGB.Blue = Math.Round(255 * Rnd())
		End If

		pFillSym = New Display.SimpleFillSymbol()
		pFillSym.Style = IIf(CInt(SimpleFillStyle) = -1, Math.Floor(8 * Rnd()), SimpleFillStyle)
		'pFillSym.Style = IIf(CInt(SimpleFillStyle) = -1, 7 * Rnd(), SimpleFillStyle)
		pFillSym.Color = pRGB

		pFillShpElement = New Carto.PolygonElement()
		pFillShpElement.Symbol = pFillSym

		pElementofPoly = pFillShpElement
		pElementofPoly.Geometry = pPolygon

		If drawFlg Then
			GetActiveView().GraphicsContainer.AddElement(pElementofPoly, 0)
			GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementofPoly
	End Function

	'Public Function DrawPolygon(ByVal pPolygon As Geometry.Polygon, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True, Optional ByVal SimpleFillStyle As Display.esriSimpleFillStyle = Display.esriSimpleFillStyle.esriSFSNull) As Carto.IElement
	'	Dim pRGB As Display.IRgbColor
	'	Dim pElementofPoly As Carto.IElement
	'	Dim pFillSym As Display.ISimpleFillSymbol
	'	Dim pFillShpElement As Carto.IFillShapeElement
	'	Dim pSimpleLineSymbol As Display.ISimpleLineSymbol

	'	pRGB = New Display.RgbColor()

	'	If Color <> -1 Then
	'		pRGB.RGB = Color
	'	Else
	'		pRGB.Red = System.Math.Round(255.0 * Rnd())
	'		pRGB.Green = System.Math.Round(255.0 * Rnd())
	'		pRGB.Blue = System.Math.Round(255.0 * Rnd())
	'	End If

	'	pSimpleLineSymbol = New Display.SimpleLineSymbol
	'	pSimpleLineSymbol.Color = pRGB
	'	pSimpleLineSymbol.Width = 1

	'	pFillSym = New Display.SimpleFillSymbol
	'	pFillSym.Color = pRGB
	'	pFillSym.Style = SimpleFillStyle
	'	pFillSym.Outline = pSimpleLineSymbol

	'	pFillShpElement = New Carto.PolygonElement
	'	pFillShpElement.Symbol = pFillSym

	'	pElementofPoly = pFillShpElement
	'	pElementofPoly.Geometry = pPolygon

	'	If drawFlg Then
	'		GetActiveView().GraphicsContainer.AddElement(pElementofPoly, 0)
	'		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	'	End If

	'	Return pElementofPoly
	'End Function

	'Public Function DrawPolygonSFS(ByVal pPolygon As Geometry.Polygon, ByVal pFillSym As Display.ISimpleFillSymbol, Optional ByVal drawFlg As Boolean = True) As Carto.IElement
	'	Dim pFillShpElement As Carto.IFillShapeElement
	'	Dim pElementofPoly As Carto.IElement

	'	pFillShpElement = New Carto.PolygonElement()
	'	pFillShpElement.Symbol = pFillSym

	'	pElementofPoly = pFillShpElement
	'	pElementofPoly.Geometry = pPolygon

	'	If drawFlg Then
	'		GetActiveView().GraphicsContainer.AddElement(pElementofPoly, 0)
	'		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	'	End If

	'	Return pElementofPoly
	'End Function

End Module
