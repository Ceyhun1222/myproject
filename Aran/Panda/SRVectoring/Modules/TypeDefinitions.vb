Option Strict Off
Option Explicit On

Imports Aran.Aim.Features
Imports Aran.Queries
Imports Aran.Aim.Enums

Public Module TypeDefinitions

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

	Public Enum eRoundMode
		NONE = 0
		FLOOR
		NERAEST
		CEIL
		SPECIAL
	End Enum

	Public Enum eNavaidType
		NONE = -1
		VOR = 0
		DME = 1
		NDB = 2
		LLZ = 3
		TACAN = 4
		RadarFIX = 5
		MKR = 6
	End Enum

	Public Enum eIntersectionType
		ByDistance = 1
		ByAngle = 2
		OnNavaid = 3
		RadarFIX = 4
	End Enum

	Public Structure ADHPType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim Identifier As Guid
		Dim OrgID As Guid

		Dim MagVar As Double
		Dim Elev As Double
		'MinTMA      As Double
		Dim WindSpeed As Double
		Dim ISAtC As Double
		Dim TransitionLevel As Double
		Dim Index As Integer
		Dim pAirportHeliport As AirportHeliport
	End Structure

	Public Structure RWYType
		<VBFixedArray(2)> Dim pPtGeo() As ESRI.ArcGIS.Geometry.IPoint
		<VBFixedArray(2)> Dim pPtPrj() As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim PairName As String
		Dim Identifier As Guid
		Dim Pair_Ident As Guid
		Dim ADHP_Ident As Guid
		'Dim ILSID As Long
		Dim ClearWay As Double
		Dim TrueBearing As Double
		Dim Length As Double
		Dim Selected As Boolean
		Dim Index As Integer
		Dim pRunwayDir As RunwayDirection

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
		Dim pFeature As Feature
	End Structure

	'Public Structure ILSType
	'	Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim RWY_ID As String
	'	Dim ID As String
	'	Dim CallSign As String
	'	Dim Category As Integer
	'	Dim MagVar As Double
	'	Dim GPAngle As Double
	'	Dim GP_RDH As Double
	'	Dim Course As Double
	'	Dim LLZ_THR As Double
	'	Dim SecWidth As Double
	'	Dim AngleWidth As Double
	'	Dim Index As Integer
	'End Structure

	Public Structure NavaidType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim RWY_Ident As Guid
		Dim Identifier As Guid
		Dim CallSign As String
		Dim MagVar As Double
		Dim TypeCode As eNavaidType
		Dim Range As Double
		Dim Disp As Double

		Dim PairNavaidIndex As Integer
		Dim GPAngle As Double
		Dim GP_RDH As Double
		Dim Course As Double
		Dim LLZ_THR As Double
		Dim SecWidth As Double
		Dim AngleWidth As Double
		Dim Category As Integer

		Dim Distance As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim Index As Integer
		Dim Tag As Integer

		Dim MkrList() As MKRType
		Dim pFeature As Feature
		Dim IntersectionType As eIntersectionType

		Dim Front As Boolean
		Dim CLShift As Double
		Dim ValCnt As Integer
		Dim ValMin() As Double
		Dim ValMax() As Double

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			sp.NavaidSystem = pFeature.GetFeatureRef()
			Return sp
		End Function

		Public Overrides Function ToString() As String
			Return CallSign
		End Function
	End Structure

	Public Structure WPT_FIXType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim Identifier As Guid

		Dim MagVar As Double
		Dim TypeCode As eNavaidType
		Dim pFeature As Feature

		Dim GuidanceNav As NavaidType
		Dim IntersectNav As NavaidType

		Dim Proc_Type As String
		Dim Proc_Name As String
		Dim H_Min As Double
		Dim H_Max As Double
		Dim H_Units As String
	End Structure

	Public Structure SegmentDataType
		Dim FIXObstacles() As TypeDefinitions.ObstacleType
		Dim HPrevFIX As Double
		Dim FIXIx As Integer
		''    TurnObstacles() As ObstacleType
		''    TurnIx As Long
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

	Structure MyArrivalLeg
		Dim pNominalPoly As ESRI.ArcGIS.Geometry.IPolyline 'Shape
		Dim pPtFlyBy As ESRI.ArcGIS.Geometry.IPoint	'Shape

		Dim pLegElement As ESRI.ArcGIS.Carto.IElement 'Element
		Dim pLegPolygon As ESRI.ArcGIS.Geometry.IPolygon 'Shape
		Dim pLegPolygonElement As ESRI.ArcGIS.Carto.IElement 'Element
		Dim NO_SEQ As Integer		'Number
		Dim T_TYPE As Integer		'Number
		Dim Guidance As Boolean

		Dim startPoint As WPT_FIXType

		Dim GuidNavType As String
		Dim GuidNav As NavaidType
		Dim InterNav As NavaidType
		Dim Radius_M As Double		'Number
		Dim TurnDir As CodeDirectionTurn

		Dim CODE_PHASE As String
		Dim LegTypeARINC As CodeSegmentPath

		Dim CODE_TYPE_COURSE As String
		Dim CODE_TURN_VALID As Boolean

		Dim CODE_DESCR_DIST_VER As String
		Dim CODE_DIST_VER_UPPER As String
		Dim CODE_DIST_VER_LOWER As String
		Dim CODE_SPEED_REF As String
		Dim CODE_REP_ATC As String
		Dim CODE_ROLE_FIX As String
		Dim VAL_COURSE As Double 'Number
		Dim CourseDir As Double
		Dim VAL_DIST_VER_UPPER As Double 'Number
		Dim VAL_DIST_VER_LOWER As Double 'Number
		Dim VAL_VER_ANGLE As Double	'Number
		Dim VAL_SPEED_LIMIT As Double 'Number
		Dim VAL_DIST As Double 'Number
		Dim VAL_DUR As Double 'Number
		Dim VAL_THETA As Double	'Number
		Dim VAL_RHO As Double 'Number
		Dim VAL_BANK_ANGLE As Double 'Number
		Dim UOM_DIST_VER_UPPER As String
		Dim UOM_DIST_VER_LOWER As String
		Dim UOM_SPEED As String
		Dim UOM_DUR As String
		Dim UOM_DIST_HORZ As String
		Dim TXT_RMK As String
	End Structure

	'==================================================================
	Structure Interval
		Dim Left As Double
		Dim Right As Double
		Dim Tag As Integer
	End Structure

	Public Structure MinMaxDist
		Dim MinFrom As Double
		Dim MaxFrom As Double
		Dim MinTo As Double
		Dim MaxTo As Double
	End Structure

	Public Structure MinMax
		Dim Min As Double
		Dim Max As Double
	End Structure

	Public Structure TypeConvert
		Dim Multiplier As Double
		Dim Rounding As Double
		Dim Unit As String
	End Structure

	'Type LayerInfo
	'    Initialised As Boolean
	'    Source As String
	'    LayerName As String
	'    FileDate As Date
	'End Type

	Public Structure ObstacleType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint ' Shape из базы
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint ' Проецированное Shape
		Dim Name As String ' Name из базы
		Dim ID As String ' ID из базы
		Dim Index As Integer ' индекс в базы
		Dim HorAccuracy As Double
		Dim VertAccuracy As Double
		Dim Height As Double ' Shape.Z из базы
		Dim MOC As Double
		Dim ReqH As Double
		Dim fSort As Double	'
		Dim sSort As String	'
		Dim Flags As Integer '   Flags
		Dim fTmp As Double '   Tag
		'==========================================================
		'    pPtGeo As IPoint
		'    pPtPrj As IPoint
		'
		'    Name As String
		'    ID As String
		'    Index As Long
		'
		'    HorAccuracy As Double
		'    VertAccuracy As Double
		'
		'    Height As Double
		'    EffectiveHeight As Double
		'    ReqH As Double
		'
		'    Dist As Double
		'    CLDist As Double
		'    DistStar As Double
		'
		'    MOC As Double
		'    ReqOCH As Double
		'    hPent As Double
		'
		''    TurnDist As Double
		''    TurnAngle As Double
		'    TurnDistL As Double
		'    TurnDistR As Double
		'    TurnAngleL As Double
		'    TurnAngleR As Double
		'
		'    Kmin As Double
		'    Kmax As Double
		'    Rmin As Double
		'    Rmax As Double
		'    dMin15 As Double
		'    dMax15 As Double
		'    dNom15 As Double
		'    fSort As Double
		'    sSort As String
		'
		'    Flags As Long
		'    fTmp As Double
	End Structure

	'Type ReportPoint
	'    Description As String
	'    Prefix As String
	'    Lat As String
	'    Lon As String
	'
	'    Direction As String
	'    PDG As Double
	'    Height As Double
	'    ToNext As Double
	'
	'    Raidus As Long
	'    Turn As Long
	'
	'    CenterLat As String
	'    CenterLon As String
	'
	'    TurnAngle As Double
	'    TurnArcLen As Double
	'End Type

	Structure CLPoint
		Dim pCLPoint As RunwayCentrelinePoint
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	End Structure

End Module
