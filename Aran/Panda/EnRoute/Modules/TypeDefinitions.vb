Option Strict Off
Option Explicit On

Imports Aran.Aim.Features
Imports System.Runtime.InteropServices
Imports Aran.Aim.DataTypes

<ComVisibleAttribute(False)> Module TypeDefinitions

	<ComVisibleAttribute(False)> Public Enum eRoundMode
		NONE = 0
		FLOOR
		NEAREST
		CEIL
		SPECIAL
	End Enum

	<ComVisibleAttribute(False)> Public Enum eSide
		Left = -1
		OnLine = 0
		Right = 1
	End Enum

	<ComVisibleAttribute(False)> Public Enum eIntersectionType
		ByDistance = 1
		ByAngle = 2
		OnNavaid = 3
		RadarFIX = 4
	End Enum

	<ComVisibleAttribute(False)> Public Structure Interval
		Dim Left As Double
		Dim Right As Double
		Dim Tag As Integer
	End Structure

	<ComVisibleAttribute(False)> Public Structure LowHigh
		Dim Low As Double
		Dim High As Double
		Dim Tag As Integer
	End Structure

	<ComVisibleAttribute(False)> Public Structure SquareSolutionArea
		Dim Solutions As Integer
		Dim First As Double
		Dim Second As Double
	End Structure

	<ComVisibleAttribute(False)> Public Structure TypeConvert
		Dim Multiplier As Double
		Dim Rounding As Double
		Dim Unit As String
	End Structure

	<ComVisibleAttribute(False)> Public Structure SegmentDataType
		Dim Obstacles() As TypeDefinitions.ObstacleType
		Dim Segment As TypeDefinitions.SegmentInfo
	End Structure

	<ComVisibleAttribute(False)> Public Structure LayerInfo
		Dim Initialised As Boolean
		Dim Source As String
		Dim LayerName As String
		Dim FileDate As Date
	End Structure

	<ComVisibleAttribute(False)> Public Structure NavaidType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim RWY_Ident As Guid
		Dim Identifier As Guid
		Dim NAV_Ident As Guid

		Dim Name As String
		Dim CallSign As String

		Dim Disp As Double
		Dim MagVar As Double
		Dim TypeCode As eNavaidType

		Dim Range As Double
		Dim Index As Integer

		Dim ValCnt As Integer
		Dim ValMin() As Double
		Dim ValMax() As Double

		'Dim MkrList() As MKRType
		Dim IntersectionType As eIntersectionType

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				sp.NavaidSystem = New FeatureRef(NAV_Ident)
			Else
				sp.FixDesignatedPoint = New FeatureRef(Identifier)
			End If

			Return sp
		End Function

		Public Function GetFeature() As Feature
			Dim result As Feature

			If (TypeCode <= eNavaidType.NONE) Then
				result = New DesignatedPoint()
				result.Identifier = Identifier
			Else
				result = New Navaid()
				result.Identifier = NAV_Ident
			End If

			Return result
		End Function

		Public Function GetFeatureRef() As FeatureRef
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				Return New FeatureRef(NAV_Ident)
			End If

			Return New FeatureRef(Identifier)
		End Function

		Public Overrides Function ToString() As String
			If CallSign Is Nothing Then Return String.Empty
			Return CallSign
		End Function

	End Structure

	'Public Structure FIXableNavaidType
	'	Dim ptGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim Name As String
	'	Dim ID As String
	'	Dim CallSign As String

	'	Dim MagVar As Integer
	'	Dim TypeCode As eNavaidType

	'	Dim Range As Double
	'	Dim Index As Integer

	'	'Dim PairTypeCode As eNavaidType
	'	'Dim PairIndex As Integer

	'	Dim ValCnt As Integer
	'	Dim ValMin() As Double
	'	Dim ValMax() As Double
	'End Structure

	<ComVisibleAttribute(False)> Public Structure ObstacleType
		Dim ptGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim ptPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim TypeName As String
		Dim UnicalName As String

		'Dim ID As Long ' ID из базы
		Dim Identifier As Guid
		Dim Index As Integer    ' индекс в базы
		Dim Height As Double    ' Shape.Z из базы
		Dim EffectiveHeight As Double
		Dim ReqH As Double      ' MOC + h
		Dim Dist As Double      ' Dist ot tochki otcheta
		Dim DistStar As Double  ' Dist ot tochki otcheta
		Dim MOC As Double       ' MOC
		Dim MCA As Double       ' MCA
		Dim ReqOCH As Double
		Dim hPent As Double     'Penetrate H
		Dim TurnDist As Double
		Dim TurnAngle As Double
		Dim Rmin As Double
		Dim Rmax As Double
		Dim fSort As Double
		Dim sSort As String
		Dim Prima As Integer
		Dim fTmp As Double      'Tag

		Dim pFeature As Feature
	End Structure

	<ComVisibleAttribute(False)> Public Structure SegmentInfo
		Dim StartFIX As NavaidType
		Dim EndFIX As NavaidType

		Dim StartNav As NavaidType
		Dim EndNav As NavaidType

		Dim StartInter As NavaidType
		Dim EndInter As NavaidType

		Dim pPrimPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSecPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutPrimPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutSecPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pNominalPoly As ESRI.ArcGIS.Geometry.IPolyline
		Dim pSecTurnArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPrimTurnArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pStartFixPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pEndFixPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pDefPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pStartFixElement As ESRI.ArcGIS.Carto.IElement
		Dim pEndFixElement As ESRI.ArcGIS.Carto.IElement
		Dim pStartPtElement As ESRI.ArcGIS.Carto.IElement
		Dim pEndPtElement As ESRI.ArcGIS.Carto.IElement

		Dim pFullSegElement As ESRI.ArcGIS.Carto.IElement
		Dim pPrimSegElement As ESRI.ArcGIS.Carto.IElement
		Dim pSegLineElement As ESRI.ArcGIS.Carto.IElement

		Dim ptCOP As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD0 As ESRI.ArcGIS.Geometry.IPoint
		Dim DirWPT As NavaidType
		Dim DominantObstacle As ObstacleType

		Dim FL As Integer
		Dim iTurnDir As Integer

		Dim fCopDist As Double
		Dim fStartNearTol As Double
		Dim fStartFarTol As Double

		Dim fEndNearTol As Double
		Dim fEndFarTol As Double

		Dim fDirection As Double
		Dim fTurnAngle As Double

		Dim fRegH As Double
		Dim fhFL As Double
		Dim fMOC As Double
		Dim fHGuidS As Double
		Dim fHInterS As Double
		Dim fHGuidE As Double
		Dim fHInterE As Double
		Dim fTurnRadius As Double
	End Structure

	<ComVisibleAttribute(False)> Public Structure ReportHeader
		Dim Procedure As String
		Dim Category As String
		Dim Aerodrome As String
		Dim Database As String
	End Structure

	<System.Runtime.InteropServices.ComVisible(False)> Public Structure ADHPType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim Identifier As Guid
		Dim OrgID As Guid

		Dim MagVar As Double
		Dim Elev As Double
		'Dim MinTMA As Double
		Dim WindSpeed As Double
		Dim ISAtC As Double
		Dim TransitionLevel As Double
		Dim Index As Integer
		Dim pAirportHeliport As AirportHeliport
	End Structure

End Module