Option Strict Off
Option Explicit On
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Features
Imports Aran.Queries

Public Module TypeDefinitions

	'Public Enum eRWY
	'	PtStart = 0
	'	PtTHR = 1
	'	PtEnd = 2
	'End Enum

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

		SPECIAL_FLOOR
		SPECIAL_NERAEST
		SPECIAL_CEIL
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

	Public Structure D2Point
		Dim X As Double
		Dim Y As Double
	End Structure

	Public Structure PolarPoint
		Dim R As Double
		Dim a As Double
	End Structure

	Public Structure TypeConvert
		Dim Multiplier As Double
		Dim Rounding As Double
		Dim Unit As String
	End Structure

	Public Structure ADHPType
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

	Structure NavaidType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim Name As String
		Dim CallSign As String

		Dim NAV_Ident As Guid
		Dim Identifier As Guid

		Dim MagVar As Double
		Dim Disp As Double

		Dim TypeCode As eNavaidType
		Dim Range As Double
		Dim Index As Integer
		Dim PairNavaidIndex As Integer

		Dim GPAngle As Double
		Dim GP_RDH As Double
		Dim Course As Double
		Dim LLZ_THR As Double
		Dim SecWidth As Double
		Dim AngleWidth As Double
		Dim Tag As Integer

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				sp.NavaidSystem = New FeatureRef(NAV_Ident)
			Else
				sp.FixDesignatedPoint = New FeatureRef(Identifier)
			End If

			Return sp
		End Function

		Public Function GetFeatureRef() As FeatureRef
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				Return New FeatureRef(NAV_Ident)
			End If

			Return New FeatureRef(Identifier)
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
		Dim NAV_Ident As Guid

		Dim MagVar As Double
		Dim TypeCode As eNavaidType

		Dim GuidanceNav As NavaidType
		Dim IntersectNav As NavaidType

		Public Function GetSignificantPoint() As SignificantPoint
			Dim sp As New SignificantPoint
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				sp.NavaidSystem = New FeatureRef(NAV_Ident)
			Else
				sp.FixDesignatedPoint = New FeatureRef(Identifier)
			End If

			Return sp
		End Function

		Public Function GetFeatureRef() As FeatureRef
			If (TypeCode > eNavaidType.NONE) And (TypeCode <= eNavaidType.TACAN) Then
				Return New FeatureRef(NAV_Ident)
			End If

			Return New FeatureRef(Identifier)
		End Function

		Public Overrides Function ToString() As String
			Return Name
		End Function
	End Structure

	Structure CLPoint
		Dim pCLPoint As RunwayCentrelinePoint
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
	End Structure

	Structure ObstacleType
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry

		Dim FromPoint As PolarPoint
		Dim ToPoint As PolarPoint

		Dim UnicalName As String
		Dim TypeName As String
		Dim Identifier As Guid
		Dim ID As Long
		Dim Index As Integer
		Dim Height As Double
		Dim HorAccuracy As Double
		Dim VertAccuracy As Double
		Dim ReqH As Double
		Dim MSA As Double

		Dim MinDist As Double
		Dim MaxDist As Double

		Dim iSector() As Byte
		'Dim iFlag As Integer

		Dim fSort As Double
		Dim sSort As String

		Public Function GetFeatureRef() As FeatureRef
			Return New FeatureRef(Identifier)
		End Function

		Public Overrides Function ToString() As String
			Return String.Format("{0} / {1}", TypeName, UnicalName)
		End Function
	End Structure

	Structure MSAType
		Dim Name As String
		Dim Identifier As Guid
		Dim MaxHeight As Double
		Dim MaxRadius As Double
		Dim Navaid As NavaidType
		Dim Sectors() As MSASectorType
		Dim SectorsNum As Integer
	End Structure

	'Structure MSAType
	'	Dim Identifier As Guid
	'	Dim Sectors() As SectorMSA
	'End Structure

	Structure MSASectorType
		Dim FromDir As Double
		Dim ToDir As Double

		Dim FromAngle As Integer
		Dim ToAngle As Integer

		Dim AbsAngle As Double
		Dim InnerDist As Double
		Dim OuterDist As Double
		Dim LowerLimit As Double
		Dim UpperLimit As Double
		Dim BufferWidth As Double
		'===================================
		Dim SectorPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim BufferPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim InnerSector As ESRI.ArcGIS.Geometry.IPolygon
		Dim InnerBuffer As ESRI.ArcGIS.Geometry.IPolygon
		Dim OuterSector As ESRI.ArcGIS.Geometry.IPolygon
		Dim OuterBuffer As ESRI.ArcGIS.Geometry.IPolygon
		'Dim IsBuffSector As Boolean
		Dim IsCutable As Boolean
		Dim IsCuted As Boolean

		Dim DominicObstacle As ObstacleType
		Dim InnerDominicObstacle As ObstacleType

		Public Overrides Function ToString() As String
			'Return String.Format("{0}, {1}, {2}", FromAngle, ToAngle, OuterDist)
			'Return DominicObstacle.Name + ": " + Math.Round(FromAngle, 2).ToString + ", " + Math.Round(ToAngle, 2).ToString
			Return "[" + Math.Round(FromAngle, 2).ToString("000") + ", " + Math.Round(ToAngle, 2).ToString("000") + "]: " + DominicObstacle.Height.ToString()
		End Function
	End Structure

	Public Structure ApproachItem
		Dim pIAFPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim pIAFPtCh As SignificantPoint

		Dim FullPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim PrimPoly As ESRI.ArcGIS.Geometry.IPolygon

		Dim IAFtoIFDir As Double
		Dim StartDir As Double
		Dim EndDir As Double

		Dim fMOCA As Double
		Dim fMOC As Double

		Dim SignObstacle As ObstacleType
		Dim Obstacles() As ObstacleType
	End Structure

	Public Structure IAPArrayItem
		Dim pIAP As InstrumentApproachProcedure
		Dim pIFPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim pFAFPtPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim pIFPtCh As SignificantPoint

		Dim IFtoFAFDir As Double
		Dim IFtoFAFDist As Double

		Dim app() As ApproachItem

		Dim initialised As Boolean

		Public Overrides Function ToString() As String
			Return pIAP.Name
		End Function
	End Structure

End Module