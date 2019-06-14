Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display
Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Geometry

<ComVisible(False)> Module Utils
	Public Function PointsToMapUnits(ByRef val As Double, toUnits As esriUnits) As Double
		'Converts from points to ESRI map unites
		Select Case toUnits
			Case esriUnits.esriMillimeters
				Return val * (25.4 / 72.0)
			Case esriUnits.esriCentimeters
				Return val * (2.54 / 72.0)
			Case esriUnits.esriInches
				Return val / 72.0
		End Select

		Return val
	End Function

	Public Function MapUnitsToPoints(ByRef val As Double, fromUnits As esriUnits) As Double
		'Convert from ESRI map unites to points
		Select Case fromUnits
			Case esriUnits.esriMillimeters
				Return val * (72.0 / 25.4)
			Case esriUnits.esriCentimeters
				Return val * (72.0 / 2.54)
			Case esriUnits.esriInches
				Return val * 72.0
		End Select

		Return val
	End Function

	Public Function GetStringRows(ByRef text As String) As String()
		Dim n As Short
		Dim i As Short
		Dim j As Short
		Dim curRow As String
		Dim Result() As String

		ReDim Result(100)
		j = 0
		n = text.Length
		curRow = ""

		For i = 0 To n - 1

			If text(i) = Chr(13) Then
				'If Asc(Mid(text, i)) = 13 Then
				Result(j) = curRow
				j = j + 1
				curRow = ""
			End If

			If text(i) <> Chr(10) Then
				'If Asc(Mid(text, i)) <> 10 Then
				curRow = curRow & text(i)
			End If
		Next i

		Result(j) = curRow
		ReDim Preserve Result(j)

		Return Result
	End Function

	Public Function CreatePoint(ByVal X As Double, ByVal Y As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim Result As ESRI.ArcGIS.Geometry.IPoint = New ESRI.ArcGIS.Geometry.Point()
		Result.PutCoords(X, Y)
		Return Result
	End Function

	Public Function CreateLine(ByVal pFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal pTo As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.ILine
		Dim Reasult As ESRI.ArcGIS.Geometry.ILine = New ESRI.ArcGIS.Geometry.Line
		Reasult.PutCoords(pFrom, pTo)
		Return Reasult
	End Function

    Public Sub FromMapPoint(ByVal displayTransform As ESRI.ArcGIS.Geometry.ITransformation, ByRef pMapPoint As ESRI.ArcGIS.Geometry.IPoint, ByRef dXIn As Integer, ByRef dYIn As Integer)
        Dim dXQuery As Double = 0
        Dim dYQuery As Double = 0

        If displayTransform Is Nothing Then
            pMapPoint.QueryCoords(dXQuery, dYQuery)
            dXIn = dXQuery
            dYIn = dYQuery
        Else
            Dim tempTransform As ESRI.ArcGIS.Display.IDisplayTransformation = CType(displayTransform, IDisplayTransformation)
            tempTransform.FromMapPoint(pMapPoint, dXIn, dYIn)
        End If
    End Sub

    Public Function DegToRad(deg As Double) As Double
        Return deg * Math.PI / 180.0
    End Function

    Public Function RadToDeg(rad As Double) As Double
        Return rad * 180.0 / Math.PI
    End Function

    Public Function PointAlongPlane(ByVal ptGeo As IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As IPoint
        dirAngle = DegToRad(dirAngle)
        Dim pClone As IClone
        pClone = ptGeo
        PointAlongPlane = pClone.Clone
        'Set PointAlongPlane = New Point
        PointAlongPlane.PutCoords(ptGeo.X + Dist * Math.Cos(dirAngle), ptGeo.Y + Dist * Math.Sin(dirAngle))
    End Function

End Module
