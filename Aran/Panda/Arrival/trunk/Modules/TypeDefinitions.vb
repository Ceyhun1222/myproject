Option Strict Off
Option Explicit On
Imports Aran.Aim.Features
Imports Aran.Aim.Enums
Imports Aran.Queries
Imports ESRI.ArcGIS.Geometry
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Objects

<System.Runtime.InteropServices.ComVisible(False)> Module TypeDefinitions

	'Public Enum eInterfaceColors
	'   icMistyRose = &HE1E4FF
	'   icSlateGray = &H908070
	'   icDodgerBlue = &HFF901E
	'   icDeepSkyBlue = &HFFBF00
	'   icSpringGreen = &H7FFF00
	'   icForestGreen = &H228B22
	'   icGoldenrod = &H20A5DA
	'   icFirebrick = &H2222B2
	'End Enum

	Public Enum eRWY
		PtStart = 0
		PtTHR = 1
		PtEnd = 2
	End Enum

	Public Enum eSide
		Left = -1
		OnLine = 0
		Right = 1
	End Enum

	<System.Runtime.InteropServices.ComVisible(False)> Public Enum eSegmentType
		NONE
		straight
		toHeading
		courseIntercept
		directToFIX
		turnAndIntercept
		arcIntercept
		arcPath
	End Enum

	Public Enum eExpansion
		noChange = 0
		goParalel
		expandAngle
		selfExpand
	End Enum

	Public Enum eRoundMode
		NONE = 0
		FLOOR
		NEAREST
		CEIL
		SPECIAL
	End Enum

	Public Enum eOAS
		ZeroPlane = 0
		WPlane = 1
		XlPlane = 2
		YlPlane = 3
		ZPlane = 4
		YrPlane = 5
		XrPlane = 6
		WsPlane = 7
		CommonPlane = 8
		NonPrec = 9
	End Enum

	Public Enum TrackType
		Straight = 0
		Arc = 1
	End Enum

	Public Enum eIntersectionType
		ByDistance = 1
		ByAngle = 2
		OnNavaid = 3
		RadarFIX = 4
	End Enum

	Public Enum eSectorType
		WholeSector = 0
		InnerSector = 1
		OuterSector = 2
	End Enum

	<System.Runtime.InteropServices.ComVisible(False)>
	Public Enum Degree2StringMode
		DD
		DM
		DMS
		DMSLat 'NS
		DMSLon	'EW
	End Enum

	<FlagsAttribute()>
	Public Enum eAreaType As Integer
		PrimaryArea = 4
		BufferArea = 8
	End Enum

	'Structure LayerInfo
	'	Dim Initialised As Boolean
	'	Dim WorkspaceType As Integer
	'	Dim Source As String
	'	Dim LayerName As String
	'	Dim FileDate As Date
	'End Structure

	Public Structure DoublePoint
		Dim X As Double
		Dim Y As Double
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

	Structure ObstacleMSA
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry
		Dim TypeName As String
		Dim UnicalName As String
		Dim Identifier As Guid
		Dim ID As Long
		Dim Height As Double
		Dim ReqH As Double

		Dim fSort As Double
		Dim sSort As String
		Dim Index As Integer

		Function GetFeatureRef() As FeatureRef
			Return New FeatureRef(Identifier)
		End Function

	End Structure

	'Public Structure ObstaclePlaneData
	'	Dim Plane As Integer
	'	Dim minZPlane As Double
	'	Dim hPent As Double
	'End Structure

	Public Structure ObstacleData
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim Dist As Double
		Dim CLDist As Double
		Dim DistStar As Double
		Dim Height As Double
		Dim MOC As Double
		Dim hPenet As Double
		Dim EffectiveHeight As Double
		Dim ReqH As Double

		Dim ReqOCH As Double

		Dim TurnDistL As Double
		Dim TurnDistR As Double
		Dim TurnAngleL As Double
		Dim TurnAngleR As Double

		Dim Rmin As Double
		Dim Rmax As Double
		Dim dMin15 As Double
		Dim minZPlane As Double

		Dim MSA As Double
		Dim Angle As Double
		Dim iSector() As eAreaType

		Dim fTmp As Double
		Dim fSort As Double
		Dim sSort As String

		Dim Plane As Integer
		Dim Flags As Integer
		Dim SectorIndex As Integer
		Dim Owner As Integer
		Dim Index As Integer
	End Structure

	Public Structure Obstacle
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry

		Dim TypeName As String
		Dim UnicalName As String
		Dim Identifier As Guid
		Dim ID As Long
		Dim IgnoredByUser As Boolean

		Dim Height As Double
		Dim HorAccuracy As Double
		Dim VertAccuracy As Double

		Dim Parts() As Integer
		Dim PartsNum As Integer
		Dim NIx As Integer
		Dim Index As Integer

		Function GetFeatureRef() As FeatureRef
			Return New FeatureRef(Identifier)
		End Function

		'Dim AvDist As Double
		'Dim MaxReqH As Double
		'Dim MaxReqOCH As Double
		'Dim TurnDistL As Double
		'Dim TurnDistR As Double
		'Dim TurnAngleL As Double
		'Dim TurnAngleR As Double
		'Dim fSort As Double
		'Dim sSort As String
		'Dim stepNumber As Integer
	End Structure

	Public Structure ObstacleContainer
		Dim Obstacles() As Obstacle
		Dim Parts() As ObstacleData

		Sub AddPart(ByRef Last As Integer, ByVal Dest As Integer, ByVal from As ObstacleContainer, ByVal Src As ObstacleData)
			Parts(Dest) = Src

			If from.Obstacles(Src.Owner).NIx < 0 Then
				Last += 1
				Obstacles(Last) = from.Obstacles(Src.Owner)
				Obstacles(Last).PartsNum = 0
				ReDim Obstacles(Last).Parts(from.Obstacles(Src.Owner).PartsNum - 1)
				from.Obstacles(Src.Owner).NIx = Last
			End If

			Parts(Dest).Owner = from.Obstacles(Src.Owner).NIx
			Parts(Dest).Index = Obstacles(Parts(Dest).Owner).PartsNum
			Obstacles(Parts(Dest).Owner).Parts(Obstacles(Parts(Dest).Owner).PartsNum) = Dest
			Obstacles(Parts(Dest).Owner).PartsNum += 1
		End Sub

		Sub MovePart(ByVal Dest As Integer, ByVal Src As Integer)
			Dim own As Integer
			Dim ix As Integer

			If Dest = Src Then Return

			own = Parts(Dest).Owner
			ix = Parts(Dest).Index

			Obstacles(own).PartsNum -= 1

			If Obstacles(own).PartsNum > 0 Then
				Obstacles(own).Parts(ix) = Obstacles(own).Parts(Obstacles(own).PartsNum)
				Parts(Obstacles(own).Parts(ix)).Index = ix
			End If

			Parts(Dest) = Parts(Src)		'Dest -> X		Src -> O
			Obstacles(Parts(Dest).Owner).Parts(Parts(Dest).Index) = Dest
			'N -= 1
		End Sub

		Sub MoveObst(ByVal Dest As Integer, ByVal Src As Integer)

		End Sub
	End Structure

	Public Structure RWYType
		<VBFixedArray(2)> Dim pPtGeo() As ESRI.ArcGIS.Geometry.IPoint
		<VBFixedArray(2)> Dim pPtPrj() As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim PairName As String
		Dim Identifier As Guid
		Dim Pair_Ident As Guid
		Dim ADHP_Ident As Guid
		'Dim pRunwayDir As RunwayDirection
		'ILSID		As long

		Dim TrueBearing As Double
		Dim ClearWay As Double
		Dim Length As Double

		Dim Selected As Boolean
		Dim Index As Integer

		Public Function GetFeatureRefObject() As FeatureRefObject
			Dim fro As FeatureRefObject = New FeatureRefObject()
			fro.Feature = New FeatureRef(Identifier)				'Feature.GetFeatureRef()
			Return fro
		End Function

		Public Sub Initialize()
			ReDim pPtGeo(2)
			ReDim pPtPrj(2)
		End Sub
	End Structure

	Public Structure MKRType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim Identifier As Guid
		Dim CallSign As String
		Dim AxisBearing As Double

		Dim MKRClass As Aim.Enums.CodeMarkerBeaconSignal
		Dim Frequency As Double
		Dim DistFromTHR As Double
		Dim DistFromCL As Double
		Dim Altitude As Double
		Dim Height As Double
		Dim Index As Integer
		Dim ILS_ID As Guid
		Dim NDB_ID As Guid
		'Dim pFeature As Feature
	End Structure

	'Structure NavaidType
	'	Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim Name As String
	'	Dim RWY_Ident As Guid
	'	Dim Identifier As Guid
	'	Dim CallSign As String
	'	Dim MagVar As Double
	'	Dim TypeCode As eNavaidType
	'	Dim Range As Double

	'	Dim PairNavaidIndex As Integer
	'	Dim GPAngle As Double
	'	Dim GP_RDH As Double
	'	Dim Course As Double
	'	Dim LLZ_THR As Double
	'	Dim SecWidth As Double
	'	Dim AngleWidth As Double
	'	Dim Category As Integer
	'	Dim Index As Integer
	'	Dim Tag As Integer

	'	Dim MkrList() As MKRType
	'	Dim IntersectionType As eIntersectionType
	'	Dim pFeature As Feature

	'	Public Function GetSignificantPoint() As SignificantPoint
	'		Dim sp As New SignificantPoint
	'		sp.NavaidSystem = pFeature.GetFeatureRef()
	'		Return sp
	'	End Function
	'End Structure

	'Public Structure NavaidType
	'	Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim Name As String
	'	Dim RWY_Ident As Guid
	'	Dim Identifier As Guid
	'	Dim CallSign As String
	'	Dim MagVar As Double
	'	Dim TypeCode As eNavaidType
	'	Dim Range As Double

	'	Dim PairNavaidIndex As Integer
	'	Dim GPAngle As Double
	'	Dim GP_RDH As Double
	'	Dim Course As Double
	'	Dim LLZ_THR As Double
	'	Dim SecWidth As Double
	'	Dim AngleWidth As Double
	'	Dim Category As Integer

	'	Dim Distance As Double
	'	Dim FromAngle As Double
	'	Dim ToAngle As Double

	'	Dim Index As Integer
	'	Dim Tag As Integer

	'	Dim MkrList() As MKRType
	'	Dim IntersectionType As eIntersectionType
	'	Dim pFeature As Feature

	'	Public Function GetSignificantPoint() As SignificantPoint
	'		Dim sp As New SignificantPoint
	'		sp.NavaidSystem = pFeature.GetFeatureRef()
	'		Return sp
	'	End Function
	'End Structure

	Structure NavaidData
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim CallSign As String

		Dim RWY_Ident As Guid
		Dim NAV_Ident As Guid
		Dim Identifier As Guid

		Dim MagVar As Double
		Dim Range As Double
		Dim Disp As Double
		Dim HorAccuracy As Double

		Dim GPAngle As Double
		Dim GP_RDH As Double
		Dim Course As Double
		Dim LLZ_THR As Double
		Dim SecWidth As Double
		Dim AngleWidth As Double

		Dim Distance As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim TypeCode As eNavaidType
		Dim IntersectionType As eIntersectionType
		Dim Index As Integer
		Dim PairNavaidIndex As Integer
		Dim Category As Integer
		Dim Tag As Integer

		Dim MkrList() As MKRType
		'Dim pFeature As Feature

		Dim Front As Boolean
		Dim CLShift As Double
		Dim ValCnt As Integer
		Dim ValMin() As Double
		Dim ValMax() As Double

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				sp.NavaidSystem = New FeatureRef(NAV_Ident)						' pFeature.GetFeatureRef()
			Else
				sp.FixDesignatedPoint = New FeatureRef(Identifier)
			End If

			Return sp
		End Function

		Public Function GetFeatureRef() As FeatureRef
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				Return New FeatureRef(NAV_Ident)                                '	pFeature.GetFeatureRef()
			End If

			Return New FeatureRef(Identifier)
		End Function

		Public Overrides Function ToString() As String
			If Not (CallSign Is Nothing) Then Return CallSign
			If Not (Name Is Nothing) Then Return Name
			Return String.Empty
		End Function
	End Structure

	'Public Structure APP_FIXType
	'	Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ADHP_ID As String
	'	Dim Name As String
	'	Dim TypeCode As eNavaidType

	'	Dim Lat As String
	'	Dim Lon As String
	'	Dim ELEV_M As Double
	'	Dim Proc_Type As String
	'	Dim Proc_Name As String
	'	Dim GuidanceNav As NavaidType
	'	Dim IntersectNav As NavaidType
	'	Dim InputDate As Integer
	'End Structure

	<System.Runtime.InteropServices.ComVisible(False)> Public Structure WPT_FIXType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String

		Dim NAV_Ident As Guid
		Dim Identifier As Guid

		Dim MagVar As Double
		Dim TypeCode As eNavaidType
		'Dim pFeature As Feature
		Dim HorAccuracy As Double

		Dim GuidanceNav As NavaidData
		Dim IntersectNav As NavaidData

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				sp.NavaidSystem = New FeatureRef(NAV_Ident)						' pFeature.GetFeatureRef()
			Else
				sp.FixDesignatedPoint = New FeatureRef(Identifier)
			End If

			Return sp
		End Function

		Public Function GetFeatureRef() As FeatureRef
			If (TypeCode > eNavaidType.NONE) Then Return New FeatureRef(NAV_Ident)

			Return New FeatureRef(Identifier)
		End Function

		Public Overrides Function ToString() As String
			If Name Is Nothing Then Return String.Empty
			Return Name
		End Function
	End Structure

	Public Structure StepDownFIX
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtStart As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtWarn As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtEnd As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtCntr As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim Identifier As Guid
		Dim LegTypeARINC As CodeSegmentPath

		Dim MagVar As Double
		Dim Role As Aran.Aim.Enums.CodeProcedureFixRole
		'Dim pFeature As Feature
		Dim GuidanceNav As NavaidData
		Dim IntersectNav As NavaidData
		Dim WPT As WPT_FIXType
		Dim FixStartPoint As StepDownFIXPoint
		Dim FixEndPoint As StepDownFIXPoint

		Dim PDG As Double
		Dim InDir As Double
		Dim OutDir As Double
		Dim TurnAngle As Double
		Dim Length As Double
		Dim TotalLength As Double
		Dim MinAlt As Double
		Dim MaxAlt As Double
		Dim Counted As Integer
		Dim PropagateToPrevious As Boolean
		Dim TurnDir_A As Integer
		Dim TurnDir As Integer
		Dim ArcDir As Integer
		Dim Track As TrackType

		Dim ptElem As ESRI.ArcGIS.Carto.IElement
		Dim pFixPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pFixPolyElem As ESRI.ArcGIS.Carto.IElement
		Dim pIAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pIAreaPolyElem As ESRI.ArcGIS.Carto.IElement
		Dim pIIAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pIIAreaPolyElem As ESRI.ArcGIS.Carto.IElement
		Dim pNominalPoly As ESRI.ArcGIS.Geometry.IPolyline
		Dim pNominalElem As ESRI.ArcGIS.Carto.IElement
		Dim pArcPoly As ESRI.ArcGIS.Geometry.IPolyline
		Dim pArcElem As ESRI.ArcGIS.Carto.IElement
		Dim Tag As Integer
		Dim Obstacles As ObstacleContainer

		Dim Report As StepDownFIXReport

		Public Function GetFeatureRef() As FeatureRef
			Return New FeatureRef(Identifier)
		End Function
	End Structure

	Public Structure StepDownFIXPoint
		Dim Name As String
		Dim Snaped As Boolean
		Dim Created As Boolean
		Dim Inited As Boolean
		Dim OnNavaid As Boolean
		Dim NAV_Ident As Guid
		Dim Identifier As Guid
		'Dim pFeature As Feature
		Dim X As Double
		Dim Y As Double

		Public Function GetFeatureRef() As FeatureRef
			Return New FeatureRef(NAV_Ident)
		End Function
	End Structure

	Public Structure StepDownFIXReport
		Dim Role As String
		Dim LegType As String
		Dim WayPoint As String
		Dim FlyOver As String
		Dim Course As String
		Dim FullCourse As String
		Dim Distance As String
		Dim DistanceKM As String
		Dim MagVariation As String
		Dim TurnDirection As String
		Dim TurnAngle As String
		Dim InDir As String
		Dim OutDir As String
		Dim Altitude As String
		Dim AltitudeM As String
		Dim MinAltitude As String
		Dim MaxAltitude As String
		Dim Speed As String
		Dim GuidNav As String
		Dim Latitude As String
		Dim Longitude As String

		Public Sub CalculateMinAlt(ByVal Alt As Double)
			MinAltitude = Str(Math.Round(ConvertHeight(Alt, 2), 3))
			Altitude = MinAltitude
			AltitudeM = Str(Alt)
		End Sub

		Public Sub CalculateMaxAlt(ByVal Alt As Double)
			MaxAltitude = Str(Math.Round(ConvertHeight(Alt, 2), 3))
		End Sub

	End Structure

	'Public Structure DefData
	'	Dim ADHP_ID As Integer
	'	Dim WPT_Name As String
	'	Dim WPT_Type As String
	'	Dim Proc_Type As String
	'	Dim Proc_Name As String
	'	Dim FL_Min As Double
	'	Dim FL_Max As Double
	'	Dim H_Units As String
	'	Dim Homing_Nav As String
	'	Dim TypeOfHomingNav As eNavaidType
	'	Dim IntersectNav As String
	'	Dim TypeOfIntersectNav As eNavaidType
	'End Structure

	Public Structure IFProhibitionSector
		Dim AlphaFrom As Integer
		Dim AlphaTo As Integer
		Dim IndexI As Integer
		Dim IndexJ As Integer
		Dim DistToObst As Double
		Dim DirToObst As Double
		Dim rObs As Double
		Dim dHObst As Double
		Dim MOC As Double
		Dim ObsArea As ESRI.ArcGIS.Geometry.IPolygon
		Dim ObsAreaElement As ESRI.ArcGIS.Carto.IElement
	End Structure

	Public Structure SegmentDataType
		Dim FIXObstacles As ObstacleContainer
		Dim HPrevFIX As Double
		Dim FIXIx As Integer
	End Structure

	Public Structure TurnStruct
		Dim FromAngle As Double
		Dim ToAngle As Double
		Dim FromDist As Double
		Dim ToDist As Double
		Dim Tag As Integer
	End Structure

	Public Structure ProfilePoint
		Dim X As Double
		Dim Y As Double	'Z
		Dim Course As Double
		Dim PDG As Double
		Dim Role As Aran.Aim.Enums.CodeProcedureDistance
		'Dim Role As String
		'Dim atPoint As Aran.Aim.Enums.CodeProcedureDistance
	End Structure

	Public Structure D3DPlane
		Dim pPt As ESRI.ArcGIS.Geometry.IPoint

		Dim X As Double
		Dim Y As Double
		Dim Z As Double

		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double
	End Structure

	Public Structure D3DPolygone
		Dim Poly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Plane As D3DPlane
	End Structure

	Public Structure StarSegment
		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		Dim PtEnd As ESRI.ArcGIS.Geometry.IPoint
		Dim Nav1Type As Short
		Dim Nav2Type As Short
		Dim ptNav1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNav2 As ESRI.ArcGIS.Geometry.IPoint
		Dim Val1 As Double
		Dim Val2 As Double
	End Structure

	Structure SectorMSA
		Dim FromAngle As Double
		Dim ToAngle As Double
		Dim AbsAngle As Double
		Dim InnerDist As Double
		Dim OuterDist As Double
		Dim LowerLimit As Double
		Dim UpperLimit As Double
	End Structure

	Structure MSAType
		Dim Identifier As Guid
		Dim Sectors() As SectorMSA
	End Structure

	Public Structure TypeConvert
		Dim Multiplier As Double
		Dim Rounding As Double
		Dim Unit As String
	End Structure

	Structure PDGLevel
		Dim PDG As Double
		Dim Range As Double
		Dim AdjustANgle As Integer
	End Structure

	Structure Interval
		Dim Left As Double
		Dim Right As Double
		Dim Tag As Integer
	End Structure

	Structure LowHigh
		Dim Low As Double
		Dim High As Double
		Dim Tag As Integer
	End Structure

	Structure PointCoords
		Dim sLat As String
		Dim sLon As String
		Dim sDir As String
		Dim sH As String
	End Structure

	Public Structure ReportHeader
		Dim Procedure As String
		Dim Category As String
		Dim Aerodrome As String
		Dim Database As String
		'Dim EffectiveDate As Date
	End Structure

	'Structure TraceSegment
	'	Dim SegmentType As Integer
	'	Dim HStart As Double
	'	Dim HFinish As Double
	'	Dim H1 As Double
	'	Dim H2 As Double
	'	Dim Length As Double
	'	Dim ptIn As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
	'	Dim DirIn As Double
	'	Dim DirBetween As Double
	'	Dim DirOut As Double
	'	Dim PathPrj As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim StCoords As String
	'	Dim FinCoords As String
	'	Dim Center1Coords As String
	'	Dim Center2Coords As String
	'	Dim St1Coords As String
	'	Dim Fin1Coords As String
	'	Dim St2Coords As String
	'	Dim Fin2Coords As String
	'	Dim TurnR As Double
	'	Dim Turn1Dir As Double
	'	Dim Turn2Dir As Double
	'	Dim Turn1Angle As Double
	'	Dim Turn2Angle As Double
	'	Dim Turn1Length As Double
	'	Dim Turn2Length As Double
	'	Dim BetweenTurns As Double
	'	Dim Comment As String
	'	Dim RepComment As String
	'End Structure

	Structure TraceSegment
		Dim ptIn As IPoint			'pPtStart
		Dim ptOut As IPoint			'pPtEnd
		Dim PathPrj As IPolyline	'pNominalPoly
		Dim pProtectArea As IPolygon

		Dim SegmentCode As eSegmentType
		Dim LegType As CodeSegmentPath

		Dim GuidanceNav As NavaidData
		Dim InterceptionNav As NavaidData
		Dim SeminWidth As Double

		Dim HStart As Double
		Dim HFinish As Double
		Dim H1, H2 As Double

		Dim Length As Double
		Dim PDG As Double
		Dim BankAngle As Double
		Dim DirIn As Double
		Dim DirOut As Double

		'=========================
		Dim DirBetween As Double
		Dim StCoords As String
		Dim FinCoords As String
		'=========================
		Dim PtCenter1 As IPoint
		Dim Center1Coords As String
		Dim Turn1Dir As Integer
		Dim Turn1R As Double
		Dim Turn1Angle As Double
		Dim Turn1Length As Double
		Dim St1Coords As String
		Dim Fin1Coords As String
		'=========================
		Dim PtCenter2 As IPoint
		Dim Center2Coords As String
		Dim Turn2Dir As Integer
		Dim Turn2R As Double
		Dim Turn2Angle As Double
		Dim Turn2Length As Double
		Dim St2Coords As String
		Dim Fin2Coords As String
		'=========================
		Dim BetweenTurns As Double
		Dim Comment As String
		Dim RepComment As String
	End Structure

	Structure ReportPoint
		Dim Description As String
		Dim Prefix As String
		Dim Lat As String
		Dim Lon As String
		Dim Direction As Double
		Dim PDG As Double
		Dim Altitude As Double
		Dim ToNext As Double
		Dim Radius As Double
		Dim Turn As Integer
		Dim CenterLat As String
		Dim CenterLon As String
		Dim TurnAngle As Double
		Dim TurnArcLen As Double
	End Structure

	Structure CLPoint
		Dim pCLPoint As RunwayCentrelinePoint
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	End Structure

	Structure MSASectorType
		Dim FromAngle As Double
		Dim ToAngle As Double
		Dim AbsAngle As Double
		Dim InnerDist As Double
		Dim OuterDist As Double
		Dim LowerLimit As Double
		Dim UpperLimit As Double
		'===================================
		Dim SectorPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim BufferPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim InnerSector As ESRI.ArcGIS.Geometry.IPolygon
		Dim InnerBuffer As ESRI.ArcGIS.Geometry.IPolygon
		Dim OuterSector As ESRI.ArcGIS.Geometry.IPolygon
		Dim OuterBuffer As ESRI.ArcGIS.Geometry.IPolygon
		'    IsBuffSector As Boolean
		Dim IsCutable As Boolean
		Dim IsCuted As Boolean
		Dim DominicObstacle As ObstacleData
		Dim InnerDominicObstacle As ObstacleData
	End Structure

	Public Class VisualReferencePoint
		Private _gShape As IPoint
		Public Property gShape() As IPoint
			Get
				Return _gShape
			End Get
			Set(ByVal value As IPoint)
				_gShape = value
			End Set
		End Property
		Private _pShape As IPoint
		Public Property pShape() As IPoint
			Get
				Return _pShape
			End Get
			Set(ByVal value As IPoint)
				_pShape = value
			End Set
		End Property
		Private _name As String
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property
		Private _type As String
		Public Property Type() As String
			Get
				Return _type
			End Get
			Set(ByVal value As String)
				_type = value
			End Set
		End Property
		Private _description As String
		Public Property Description() As String
			Get
				Return _description
			End Get
			Set(ByVal value As String)
				_description = value
			End Set
		End Property
		Private _projPnt As IPoint
		Public Property ProjPnt() As IPoint
			Get
				Return _projPnt
			End Get
			Set(ByVal value As IPoint)
				_projPnt = value
			End Set
		End Property

		Public Sub New(gShapeValue As IPoint, pShapeValue As IPoint, nameValue As String, typeValue As String, descriptionValue As String)
			_gShape = gShapeValue
			_pShape = pShapeValue
			_name = nameValue
			_type = typeValue
			_description = descriptionValue
		End Sub
	End Class

	Public Class TrackStep
		Private _startGeoPoint As IPoint
		Public Property StartGeoPoint() As IPoint
			Get
				Return _startGeoPoint
			End Get
			Set(ByVal value As IPoint)
				_startGeoPoint = value
			End Set
		End Property
		Private _startPointGeoAngle As Double
		Public Property StartPointGeoAngle() As Double
			Get
				Return _startPointGeoAngle
			End Get
			Set(ByVal value As Double)
				_startPointGeoAngle = value
			End Set
		End Property
		Private _startPrjPoint As IPoint
		Public Property StartPrjPoint() As IPoint
			Get
				Return _startPrjPoint
			End Get
			Set(ByVal value As IPoint)
				_startPrjPoint = value
			End Set
		End Property
		Private _startPointPrjAngle As Double
		Public Property StartPointPrjAngle() As Double
			Get
				Return _startPointPrjAngle
			End Get
			Set(ByVal value As Double)
				_startPointPrjAngle = value
			End Set
		End Property

		Private _targetGeoPoint As IPoint
		Public Property TargetGeoPoint() As IPoint
			Get
				Return _targetGeoPoint
			End Get
			Set(ByVal value As IPoint)
				_targetGeoPoint = value
			End Set
		End Property
		Private _targetPointGeoAngle As Double
		Public Property TargetPointGeoAngle() As Double
			Get
				Return _targetPointGeoAngle
			End Get
			Set(ByVal value As Double)
				_targetPointGeoAngle = value
			End Set
		End Property
		Private _targetPrjPoint As IPoint
		Public Property TargetPrjPoint() As IPoint
			Get
				Return _targetPrjPoint
			End Get
			Set(ByVal value As IPoint)
				_targetPrjPoint = value
			End Set
		End Property
		Private _targetPointPrjAngle As Double
		Public Property TargetPointPrjAngle() As Double
			Get
				Return _targetPointPrjAngle
			End Get
			Set(ByVal value As Double)
				_targetPointPrjAngle = value
			End Set
		End Property

		Private _divergenceAngle As Double
		Public Property DivergenceAngle() As Double
			Get
				Return _divergenceAngle
			End Get
			Set(ByVal value As Double)
				_divergenceAngle = value
			End Set
		End Property
		Private _convergenceAngle As Double
		Public Property ConvergenceAngle() As Double
			Get
				Return _convergenceAngle
			End Get
			Set(ByVal value As Double)
				_convergenceAngle = value
			End Set
		End Property
		Private _distBetweenManeuvers As Double
		Public Property DistBetweenManeuvers() As Double
			Get
				Return _distBetweenManeuvers
			End Get
			Set(ByVal value As Double)
				_distBetweenManeuvers = value
			End Set
		End Property
		Private _intermediateSegmentGeoAngle As Double
		Public Property IntermediateSegmentAngle() As Double
			Get
				Return _intermediateSegmentGeoAngle
			End Get
			Set(ByVal value As Double)
				_intermediateSegmentGeoAngle = value
			End Set
		End Property

		Private _divergenceVisualReferencePoint As VisualReferencePoint
		Public Property DivergenceVisualReferencePoint() As VisualReferencePoint
			Get
				Return _divergenceVisualReferencePoint
			End Get
			Set(ByVal value As VisualReferencePoint)
				_divergenceVisualReferencePoint = value
			End Set
		End Property
		Private _convergenceVisualReferencePoint As VisualReferencePoint
		Public Property ConvergenceVisualReferencePoint() As VisualReferencePoint
			Get
				Return _convergenceVisualReferencePoint
			End Get
			Set(ByVal value As VisualReferencePoint)
				_convergenceVisualReferencePoint = value
			End Set
		End Property

		Private _initialSegmentCentrelinePointCollection As IPointCollection
		Public Property initialSegmentCentrelinePointCollection() As IPointCollection
			Get
				Return _initialSegmentCentrelinePointCollection
			End Get
			Set(ByVal value As IPointCollection)
				_initialSegmentCentrelinePointCollection = value
			End Set
		End Property
		Private _intermediateSegmentCentrelinePointCollection As IPointCollection
		Public Property intermediateSegmentCentrelinePointCollection() As IPointCollection
			Get
				Return _intermediateSegmentCentrelinePointCollection
			End Get
			Set(ByVal value As IPointCollection)
				_intermediateSegmentCentrelinePointCollection = value
			End Set
		End Property
		Private _finalSegmentCentrelinePointCollection As IPointCollection
		Public Property finalSegmentCentrelinePointCollection() As IPointCollection
			Get
				Return _finalSegmentCentrelinePointCollection
			End Get
			Set(ByVal value As IPointCollection)
				_finalSegmentCentrelinePointCollection = value
			End Set
		End Property

		Private _initialSegmentPolygon As IPolygon
		Public Property initialSegmentPolygon() As IPolygon
			Get
				Return _initialSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_initialSegmentPolygon = value
			End Set
		End Property
		Private _intermediateSegmentPolygon As IPolygon
		Public Property intermediateSegmentPolygon() As IPolygon
			Get
				Return _intermediateSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_intermediateSegmentPolygon = value
			End Set
		End Property
		Private _finalSegmentPolygon As IPolygon
		Public Property finalSegmentPolygon() As IPolygon
			Get
				Return _finalSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_finalSegmentPolygon = value
			End Set
		End Property


		Private _usingCirclingBox As Integer
		Public Property usingCirclingBox() As Integer
			Get
				Return _usingCirclingBox
			End Get
			Set(ByVal value As Integer)
				_usingCirclingBox = value
			End Set
		End Property
		Private _isFinalStep As Boolean
		Public Property isFinalStep() As Boolean
			Get
				Return _isFinalStep
			End Get
			Set(ByVal value As Boolean)
				_isFinalStep = value
			End Set
		End Property

	End Class

	Public Class TrackStep_2
		Private _startPrjPnt As IPoint
		Public Property startPrjPnt() As IPoint
			Get
				Return _startPrjPnt
			End Get
			Set(ByVal value As IPoint)
				_startPrjPnt = value
			End Set
		End Property
		Private _targetPrjPnt As IPoint
		Public Property targetPrjPnt() As IPoint
			Get
				Return _targetPrjPnt
			End Get
			Set(ByVal value As IPoint)
				_targetPrjPnt = value
			End Set
		End Property
		Private _divergencePrjPnt As IPoint
		Public Property divergencePrjPnt() As IPoint
			Get
				Return _divergencePrjPnt
			End Get
			Set(ByVal value As IPoint)
				_divergencePrjPnt = value
			End Set
		End Property
		Private _convergencePrjPnt As IPoint
		Public Property convergencePrjPnt() As IPoint
			Get
				Return _convergencePrjPnt
			End Get
			Set(ByVal value As IPoint)
				_convergencePrjPnt = value
			End Set
		End Property
		Private _divergenceAngle As Double
		Public Property divergenceAngle() As Double
			Get
				Return _divergenceAngle
			End Get
			Set(ByVal value As Double)
				_divergenceAngle = value
			End Set
		End Property

		Private _convergenceAngle As Double
		Public Property convergenceAngle() As Double
			Get
				Return _convergenceAngle
			End Get
			Set(ByVal value As Double)
				_convergenceAngle = value
			End Set
		End Property

		Private _trackStepCentreline As IPolyline
		Public Property trackStepCentreline() As IPolyline
			Get
				Return _trackStepCentreline
			End Get
			Set(ByVal value As IPolyline)
				_trackStepCentreline = value
			End Set
		End Property

		Private _trackStepPolygon As IPolygon
		Public Property trackStepPolygon() As IPolygon
			Get
				Return _trackStepPolygon
			End Get
			Set(ByVal value As IPolygon)
				_trackStepPolygon = value
			End Set
		End Property

	End Class

	'Public Class TrackStepObstacle
	'    Private _obstacle As ObstacleAr
	'    Public Property Obstacle() As ObstacleAr
	'        Get
	'            Return _obstacle
	'        End Get
	'        Set(ByVal value As ObstacleAr)
	'            _obstacle = value
	'        End Set
	'    End Property
	'    Private _stepNumber As Integer
	'    Public Property stepNumber() As Integer
	'        Get
	'            Return _stepNumber
	'        End Get
	'        Set(ByVal value As Integer)
	'            _stepNumber = value
	'        End Set
	'    End Property
	'End Class

	Public Class CirclingBoxObstacle
		Private _obstacle As ObstacleContainer
		Public Property Obstacle() As ObstacleContainer
			Get
				Return _obstacle
			End Get
			Set(ByVal value As ObstacleContainer)
				_obstacle = value
			End Set
		End Property

		Private _circlingBoxSide As Integer
		Public Property circlingBoxSide() As Integer
			Get
				Return _circlingBoxSide
			End Get
			Set(ByVal value As Integer)
				_circlingBoxSide = value
			End Set
		End Property
	End Class
End Module
