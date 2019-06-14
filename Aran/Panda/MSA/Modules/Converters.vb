Option Strict Off
Option Explicit On
Option Compare Text

Imports Aran.Aim.Features
Imports Aran.Geometries
Imports Aran.Converters
Imports ESRI.ArcGIS.Geometry
Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Module Converters
	Public Function ESRIPointToARANPoint(pPoint As IPoint) As Aran.Geometries.Point
		Dim Result As Aran.Geometries.Point

		Result = New Aran.Geometries.Point()
		Result.X = pPoint.X
		Result.Y = pPoint.Y
		Result.Z = pPoint.Z
		Result.M = pPoint.M
		Result.T = 0

		ESRIPointToARANPoint = Result
	End Function

	Public Function ESRIMultiPointToARANMultiPoint(pMultiPoint As IMultipoint) As Aran.Geometries.MultiPoint
		Dim I As Long
		Dim pPointCollection As IPointCollection
		Dim pPoint As IPoint

		Dim Result As Aran.Geometries.MultiPoint
		Dim aPoint As Aran.Geometries.Point

		pPointCollection = pMultiPoint

		Result = New Aran.Geometries.MultiPoint()

		For I = 0 To pPointCollection.PointCount - 1
			pPoint = pPointCollection.Point(I)
			aPoint = New Aran.Geometries.Point()

			aPoint.X = pPoint.X
			aPoint.Y = pPoint.Y
			aPoint.Z = pPoint.Z
			aPoint.M = pPoint.M
			aPoint.T = 0
			Result.Add(aPoint)
		Next

		ESRIMultiPointToARANMultiPoint = Result
	End Function

	Public Sub ESRIPointCollectionToARANPointCollection(pPointCollection As IPointCollection, ByRef Result As Aran.Geometries.LineString)
		Dim I As Long
		Dim pPoint As IPoint
		Dim aPoint As Aran.Geometries.Point

		For I = 0 To pPointCollection.PointCount - 1
			pPoint = pPointCollection.Point(I)
			aPoint = New Aran.Geometries.Point()

			aPoint.X = pPoint.X
			aPoint.Y = pPoint.Y
			aPoint.Z = pPoint.Z
			aPoint.M = pPoint.M
			aPoint.T = 0
			Result.Add(aPoint)
		Next
	End Sub

	Public Function ESRIPathToARANLineString(pPath As IPath) As Aran.Geometries.LineString
		pPath.Generalize(0.000000001)
		ESRIPathToARANLineString = New Aran.Geometries.LineString
		ESRIPointCollectionToARANPointCollection(pPath, ESRIPathToARANLineString)
	End Function

	Public Function ESRILineToARANLineString(pLine As ILine) As Aran.Geometries.LineString
		ESRILineToARANLineString = New Aran.Geometries.LineString
		ESRILineToARANLineString.Add(ESRIPointToARANPoint(pLine.FromPoint))
		ESRILineToARANLineString.Add(ESRIPointToARANPoint(pLine.ToPoint))
	End Function

	Public Function ESRIRingToARANRing(pRing As IRing) As Aran.Geometries.Ring
		pRing.Generalize(0.000000001)
		ESRIRingToARANRing = New Aran.Geometries.Ring
		ESRIPointCollectionToARANPointCollection(pRing, ESRIRingToARANRing)
	End Function

	Public Function ESRIPolylineToARANPolyline(pPolyline As IPolyline) As Aran.Geometries.MultiLineString
		Dim I As Long
		Dim pPath As IPath

		Dim pGeometryCollection As IGeometryCollection
		Dim LineString As Aran.Geometries.LineString
		Dim Result As Aran.Geometries.MultiLineString

		pGeometryCollection = pPolyline
		Result = New Aran.Geometries.MultiLineString

		For I = 0 To pGeometryCollection.GeometryCount - 1
			pPath = pGeometryCollection.Geometry(I)
			LineString = ESRIPathToARANLineString(pPath)
			Result.Add(LineString)
		Next

		Return Result
	End Function

	Public Function ESRIPolygonToARANPolygon(pPolygon As IPolygon) As Aran.Geometries.MultiPolygon
		Dim pRing As IRing

		Dim aRing As Aran.Geometries.Ring
		Dim aPolygon As Aran.Geometries.Polygon
		Dim Result As Aran.Geometries.MultiPolygon

		Dim pPolygon4 As IPolygon4
		Dim pExteriorRings As ESRI.ArcGIS.Geometry.IGeometryBag
		Dim pExteriorRingsEnum As ESRI.ArcGIS.Geometry.IEnumGeometry

		Dim pInteriorRings As ESRI.ArcGIS.Geometry.IGeometryBag
		Dim pInteriorRingsEnum As ESRI.ArcGIS.Geometry.IEnumGeometry

		pPolygon4 = pPolygon

		pExteriorRings = pPolygon4.ExteriorRingBag
		pExteriorRingsEnum = DirectCast(pExteriorRings, ESRI.ArcGIS.Geometry.IEnumGeometry)
		pExteriorRingsEnum.Reset()
		pRing = DirectCast(pExteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)

		Result = New Aran.Geometries.MultiPolygon

		While Not pRing Is Nothing
			aPolygon = New Aran.Geometries.Polygon
			aRing = ESRIRingToARANRing(pRing)
			aPolygon.ExteriorRing = aRing

			pInteriorRings = pPolygon4.InteriorRingBag(pRing)
			pInteriorRingsEnum = DirectCast(pInteriorRings, ESRI.ArcGIS.Geometry.IEnumGeometry)
			pInteriorRingsEnum.Reset()

			pRing = DirectCast(pInteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
			While Not pRing Is Nothing
				aRing = ESRIRingToARANRing(pRing)
				aPolygon.InteriorRingList.Add(aRing)
				pRing = DirectCast(pInteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
			End While

			Result.Add(aPolygon)
			pRing = DirectCast(pExteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
		End While

		Return Result
	End Function

	Public Function ESRIEnvelopeToARANBox(pEnvelope As IEnvelope) As Aran.Geometries.Box
		ESRIEnvelopeToARANBox = New Aran.Geometries.Box
		ESRIEnvelopeToARANBox.Item(0).X = pEnvelope.XMin
		ESRIEnvelopeToARANBox.Item(0).Y = pEnvelope.YMin
		ESRIEnvelopeToARANBox.Item(0).Z = pEnvelope.ZMin
		ESRIEnvelopeToARANBox.Item(0).M = pEnvelope.MMin
		ESRIEnvelopeToARANBox.Item(0).T = 0

		ESRIEnvelopeToARANBox.Item(1).X = pEnvelope.XMax
		ESRIEnvelopeToARANBox.Item(1).Y = pEnvelope.YMax
		ESRIEnvelopeToARANBox.Item(1).Z = pEnvelope.ZMax
		ESRIEnvelopeToARANBox.Item(1).M = pEnvelope.MMax
		ESRIEnvelopeToARANBox.Item(1).T = 0
	End Function

	Public Function ESRIGeometryToARANGeometry(pGeom As IGeometry) As Aran.Geometries.Geometry
		Select Case pGeom.GeometryType
			Case esriGeometryType.esriGeometryPoint
				ESRIGeometryToARANGeometry = ESRIPointToARANPoint(pGeom)
			Case esriGeometryType.esriGeometryMultipoint
				ESRIGeometryToARANGeometry = ESRIMultiPointToARANMultiPoint(pGeom)
			Case esriGeometryType.esriGeometryPolyline
				ESRIGeometryToARANGeometry = ESRIPolylineToARANPolyline(pGeom)
			Case esriGeometryType.esriGeometryPolygon
				ESRIGeometryToARANGeometry = ESRIPolygonToARANPolygon(pGeom)
			Case esriGeometryType.esriGeometryEnvelope
				ESRIGeometryToARANGeometry = ESRIEnvelopeToARANBox(pGeom)
			Case esriGeometryType.esriGeometryPath
				ESRIGeometryToARANGeometry = ESRIPathToARANLineString(pGeom)
				'			Case esriGeometryType.esriGeometryAny
				'			Case esriGeometryType.esriGeometryMultiPatch
			Case esriGeometryType.esriGeometryRing
				ESRIGeometryToARANGeometry = ESRIRingToARANRing(pGeom)
			Case esriGeometryType.esriGeometryLine
				ESRIGeometryToARANGeometry = ESRILineToARANLineString(pGeom)

				'			Case esriGeometryType.esriGeometryCircularArc
				'				ESRIToGeometry = ESRIPathToGeomLineString(pGeom)
				'			Case esriGeometryType.esriGeometryBezier3Curve
				'				ESRIToGeometry = ESRIPathToGeomLineString(pGeom)
				'			Case esriGeometryType.esriGeometryEllipticArc
				'				ESRIToGeometry = ESRIPathToGeomLineString(pGeom)
				'			Case esriGeometryType.esriGeometryBag
				'			Case esriGeometryType.esriGeometryTriangleStrip
				'			Case esriGeometryType.esriGeometryTriangleFan
				'			Case esriGeometryType.esriGeometryRay
				'			Case esriGeometryType.esriGeometrySphere
			Case Else
				ESRIGeometryToARANGeometry = Nothing
		End Select
	End Function

	Public Function ARANPointToESRIPoint(aPoint As Aran.Geometries.Point) As IPoint
		Dim pPoint As IPoint
		pPoint = New ESRI.ArcGIS.Geometry.Point()
		pPoint.X = aPoint.X
		pPoint.Y = aPoint.Y
		pPoint.Z = aPoint.Z
		pPoint.M = aPoint.M

		Return pPoint
	End Function

	Public Function ESRIPointToAixmPoint(pPoint As IPoint) As AixmPoint
		Dim aPoint As AixmPoint

		aPoint = New AixmPoint()
		aPoint.Geo.X = pPoint.X
		aPoint.Geo.Y = pPoint.Y
		aPoint.Geo.Z = pPoint.Z
		aPoint.Geo.M = pPoint.M
		aPoint.Geo.T = 0
		Return aPoint
	End Function

	Public Function ESRIPolygonToAIXMSurface(pPolygon As IPolygon) As Surface
		Dim pRing As IRing
		Dim pSurface As Surface
		Dim pPolygon4 As IPolygon4

		Dim aRing As Aran.Geometries.Ring
		Dim aPolygon As Aran.Geometries.Polygon

		Dim pExteriorRings As ESRI.ArcGIS.Geometry.IGeometryBag
		Dim pExteriorRingsEnum As ESRI.ArcGIS.Geometry.IEnumGeometry

		Dim pInteriorRings As ESRI.ArcGIS.Geometry.IGeometryBag
		Dim pInteriorRingsEnum As ESRI.ArcGIS.Geometry.IEnumGeometry

		pSurface = New Surface()
		pPolygon4 = pPolygon

		pExteriorRings = pPolygon4.ExteriorRingBag
		pExteriorRingsEnum = DirectCast(pExteriorRings, ESRI.ArcGIS.Geometry.IEnumGeometry)
		pExteriorRingsEnum.Reset()
		pRing = DirectCast(pExteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)

		While Not pRing Is Nothing
			aPolygon = New Aran.Geometries.Polygon

			aRing = ESRIRingToARANRing(pRing)
			aPolygon.ExteriorRing = aRing

			pInteriorRings = pPolygon4.InteriorRingBag(pRing)
			pInteriorRingsEnum = DirectCast(pInteriorRings, ESRI.ArcGIS.Geometry.IEnumGeometry)
			pInteriorRingsEnum.Reset()

			pRing = DirectCast(pInteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
			While Not pRing Is Nothing
				aRing = ESRIRingToARANRing(pRing)
				aPolygon.InteriorRingList.Add(aRing)
				pRing = DirectCast(pInteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
			End While

			pSurface.Geo.Add(aPolygon)
			pRing = DirectCast(pExteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
		End While

		Return pSurface
	End Function

	Public Function AIXMPointToESRIPoint(aPoint As AixmPoint) As IPoint
		'Return ConvertToEsriGeom.FromPoint(aPoint.Geo)

		Dim pPoint As IPoint
		pPoint = New ESRI.ArcGIS.Geometry.Point()
		pPoint.X = aPoint.Geo.X
		pPoint.Y = aPoint.Geo.Y
		pPoint.Z = aPoint.Geo.Z
		pPoint.M = aPoint.Geo.M
		Return pPoint
	End Function

	Public Function AIXMCurveToESRIPolyline(aCurve As Curve) As IPolyline
		Return ConvertToEsriGeom.FromMultiLineString(aCurve.Geo)
	End Function

	Public Function AIXMSurfaceToESRIPolygon(aSurface As Surface) As IPolygon
		Return ConvertToEsriGeom.FromMultiPolygon(aSurface.Geo)
	End Function

End Module
