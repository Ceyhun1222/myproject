unit ARANFunctions;

interface

uses
	Windows, SysUtils, ARANMath, Geometry,GeometryOperatorsContract;

type
  TLineArray = Array of TLine;
  IntegerArray = Array of Integer;

function ReturnAngleInDegrees (pointFrom, pointTo: TPoint): Double;
function ReturnAngleInRadians (pointFrom, pointTo: TPoint): Double;
function LineLineIntersect (Line1, Line2: TLine): TGeometry; overload;
function LineLineIntersect (point1: TPoint; DirInRadian1: Double; point2: TPoint; DirInRadian2: Double): TGeometry; overload;

function ReturnDistanceInMeters (pointFrom, pointTo: TPoint): Double;
function PointAlongGeodesic (point: TPoint; azimuth: Double; distance: Double): TPoint;
//==============================================================================
function LocalToPrj (Center: TPoint; DirInRadian, X, Y: Double): TPoint; overload;
function LocalToPrj (Center: TPoint; DirInRadian: Double; ptPrj: TPoint): TPoint; overload;

procedure LocalToPrj (Center: TPoint; DirInRadian, X, Y: Double; var res: TPoint); overload;
procedure LocalToPrj (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var res: TPoint); overload;

function LocalToPrj (AxisLine: TLine; X, Y: Double): TPoint; overload;
function LocalToPrj (AxisLine: TLine; ptPrj: TPoint): TPoint; overload;

procedure LocalToPrj (AxisLine: TLine; X, Y: Double; var res: TPoint); overload;
procedure LocalToPrj (AxisLine: TLine; ptPrj: TPoint; var res: TPoint); overload;
//========================
function PrjToLocal (Center: TPoint; DirInRadian, X, Y: Double): TPoint; overload;
function PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint): TPoint; overload;

procedure PrjToLocal (Center: TPoint; DirInRadian, X, Y: Double; var res: TPoint); overload;
procedure PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var res: TPoint); overload;

function PrjToLocal (AxisLine: TLine; X, Y: Double): TPoint; overload;
function PrjToLocal (AxisLine: TLine; ptPrj: TPoint): TPoint; overload;

procedure PrjToLocal (AxisLine: TLine; X, Y: Double; var res: TPoint); overload;
procedure PrjToLocal (AxisLine: TLine; ptPrj: TPoint; var res: TPoint); overload;

procedure PrjToLocal (Center: TPoint; DirInRadian, X, Y: Double; var resX, resY: Double); overload;
procedure PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var resX, resY: Double); overload;

procedure PrjToLocal (AxisLine: TLine; X, Y: Double; var resX, resY: Double); overload;
procedure PrjToLocal (AxisLine: TLine; ptPrj: TPoint; var resX, resY: Double); overload;

function IfThen (AValue: Boolean; const ATrue: String; const AFalse: String): String; overload;
//==============================================================================

function PointToLineDistance(RefPoint, LinePoint: TPoint; DirInRadian: Double): Double; overload;
function PointToLineDistance(RefPoint: TPoint; Line: TLine): Double; overload;

//function PointToSegmentDistance(RefPoint, SegPoint1, SegPoint2: TPoint): Double;
function PointToLineSegmentDistance(RefPoint, SegPoint1, SegPoint2: TPoint): Double;
function PointToPartDistance(Point: TPoint; Part: TPart): Double;
function PointToRingDistance(Point: TPoint; Ring: TRing): Double;
function PointToPolylineDistance(Point: TPoint; Polyline: TPolyline): Double;

function AztToDirection (pointGeo: TPoint; AzimuthInDeg: Double; geoSR: TSpatialReference; prjSR: TSpatialReference): Double;
function DirToAzimuth (pointPrj: TPoint; DirInRadian: Double; prjSR: TSpatialReference; geoSR: TSpatialReference): Double;

function PointAlongPlane (ptOrigin: TPoint; DirInRadian, dist: Double): TPoint;
function Point2LineDistancePrj (point, pointLine: TPoint; DirInRadian: Double): Double;

function AngleSideDef(InRadian, OutRadian: Double): TSideDirection;
function SideDef (pointInLine: TPoint; lineAngleInRadian: Double; pointOutLine: TPoint): TSideDirection;

function ReturnGeodesicAzimuth(Point0, Point1: TPoint; var DirectAzimuth, InverseAzimuth: Double): LongInt;
function SpiralTouchAngle (r0, coef0, nominalDir, touchDir: Double; turnDir: TSideDirection): Double;
function CreateWindSpiral (Pt: TPoint; radianNominalDir, radianStartDir, radianEndDir, startRadius, coefficient: Double; turnSide: TSideDirection): TMultiPoint;
function IsPointInPoly(Point: TPoint; Polygon: TPolygon): Boolean;

function CalcArea(Ring: TRing): Double; overload;
function CalcArea(Polygon: TPolygon): Double; overload;

function CalcAbsArea(Ring: TRing): Double; overload;
function CalcAbsArea(Polygon: TPolygon): Double; overload;

function CircleVectorIntersect (centPoint: TPoint; radius: Double; Line: TLine; var d: Double): TPoint; overload;
function CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double; var d: Double): TPoint; overload;

function CircleVectorIntersect (centPoint: TPoint; radius: Double; Line: TLine): Double; overload;
function CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double): Double; overload;

procedure CircleVectorIntersect (centPoint: TPoint; radius: Double; Line: TLine; var d: Double; var Result: TPoint); overload;
procedure CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double; var d: Double; var Result: TPoint); overload;

function FindPolygonLatestPoint (Polygon: TPolygon; ptVector: TPoint; Direction, OutDir: Double; var d: Double; FindNearest: Boolean = False): TPoint;

function PolygonVectorIntersect (Polygon: TPolygon; Line: TLine; var d: Double; FindNearest: Boolean = False): TPoint; overload;
function PolygonVectorIntersect (Polygon: TPolygon; ptVector: TPoint; Direction: Double; var d: Double; FindNearest: Boolean = False): TPoint; overload;

function FindRingLatestPoint (Ring: TRing; ptVector: TPoint; Direction, OutDir: Double; var d: Double; FindNearest: Boolean = False): TPoint;

function RingVectorIntersect (Ring: TRing; Line: TLine; var d: Double; FindNearest: Boolean = False): TPoint; overload;
function RingVectorIntersect (Ring: TRing; ptVector: TPoint; Direction: Double; var d: Double; FindNearest: Boolean = False): TPoint; overload;

function CutRingByNNLine(Ring: TRing; NNLine: TPart; var LeftRing, RightRing: TRing): Integer;

function DrawArcPrj(PtCnt, ptFrom, ptTo: TPoint; ClWise: Integer): TPolyline;
function CreateArcPrj(PtCnt, ptFrom, ptTo: TPoint; Direction: TSideDirection): TRing;
function CreateCirclePrj(PtCnt: TPoint; Radius: Double): TRing;

function CalcTrajectoryFromMultiPoint (lineArray: TLineArray): TPolyline;
function ConvertPointsToTrackLIne(MultiPoint: TMultiPoint): TPolyline;

function TangentCyrcleIntersectPoint (centrePoint: TPoint; radius: Double; outPoint: TPoint; side: TSideDirection): TPoint;
function FixToTouchSprial (startLine, endLine: TLine; coefficient, turnRadius: Double; turnSide: TSideDirection): Double;

function SortArray (items: Array of Double): IntegerArray;
function SortPoints (basePoint: TPoint; multiPoint: TMultiPoint): TMultiPoint;

function GeoToPrj (geoGeometry: TGeometry): TGeometry;
function PrjToGeo (prjGeometry: TGeometry): TGeometry;

procedure ShowError (msg: String); overload
//procedure ShowError (exc: Exception); overload

procedure FreeObject (var vObject: TObject);

function GeoOper(): TGeometryOperators;
//----------------------
//---begin MathFunctions
//----------------------
function ReturnGeodesicDistance(X0, Y0, X1, Y1: Double): Double; stdcall; external 'MathRNAV.dll' name '_ReturnGeodesicDistance@32';
procedure InitEllipsoid(EquatorialRadius, InverseFlattening: Double); stdcall; external 'MathRNAV.dll' name '_InitEllipsoid@16';
//--------------------
//---end MathFunctions
//--------------------

function ShowHelp (callerHWND: LongWord; helpContextId: Integer): Integer;
function HtmlHelp (hwndCaller: HWND; pszFile: PChar; uCommand: UINT; dwData: DWORD): HWND; stdcall; external 'HHCTRL.OCX' name 'HtmlHelpA';

implementation

uses
	Math, Classes,
	ARANGlobals,
	Dialogs,
	Forms,
	UIContract;

var
	_geoOper:	TGeometryOperators;

//----------------------
//---begin MathFunctions
//----------------------
function PointAlongGeodesic_MathFunctions(X, Y, Dist, Azimuth: Double; var resx, resy: Double): LongInt; stdcall; external 'MathRNAV.dll' name '_PointAlongGeodesic@40';
function ReturnGeodesicAzimuth_MathFunctions(X0, Y0, X1, Y1: Double; var DirectAzimuth, InverseAzimuth: Double): LongInt; stdcall; external 'MathRNAV.dll' name '_ReturnGeodesicAzimuth@40';
//--------------------
//---end MathFunctions
//--------------------

procedure ShowError (msg: String);
begin
	MessageDlg (msg, mtError, [mbOK], 0);
end;
{
procedure ShowError (exc: Exception);
begin
	if exc is EARANError then
		MessageDlg (exc.Message, EARANError (exc).ErrorType, [mbOK], 0)
	else
		ShowError (exc.Message);
end;
}
function GeoOper(): TGeometryOperators;
begin
	result := _geoOper;
end;

function ReturnGeodesicAzimuth(Point0, Point1: TPoint; var DirectAzimuth, InverseAzimuth: Double): LongInt;
begin
	Result := ReturnGeodesicAzimuth_MathFunctions(Point0.X, Point0.Y, Point1.X, Point1.Y, DirectAzimuth, InverseAzimuth);
end;

function PointAlongGeodesic (point: TPoint; azimuth: Double; distance: Double): TPoint;
var
	resX, resY:	Double;
begin
	result := TPoint.Create;
	PointAlongGeodesic_MathFunctions (point.X, point.Y, distance, azimuth, resX, resY);
	result.SetCoords (resX, resY);
end;

function AztToDirection (pointGeo: TPoint; AzimuthInDeg: Double; geoSR, prjSR: TSpatialReference): Double;
var
	pointToGeo:	 	TPoint;
	pointFromPrj:	TPoint;
	pointToPrj:		TPoint;
begin
	pointToGeo := PointAlongGeodesic (pointGeo, AzimuthInDeg, 10.0);

	pointFromPrj := nil;
	pointToPrj := nil;

	try
		pointFromPrj := _geoOper.geoTransformations (pointGeo, geoSR, prjSR).AsPoint;
		pointToPrj := _geoOper.geoTransformations (pointToGeo, geoSR, prjSR).AsPoint;
		result := ReturnAngleInRadians (pointFromPrj, pointToPrj);
	finally
		pointToGeo.Free;
		pointFromPrj.Free;
		pointToPrj.Free;
	end;
end;

function DirToAzimuth (pointPrj: TPoint; DirInRadian: Double; prjSR, geoSR: TSpatialReference): Double;
var
	directAzumuth:	Double;
	inverseAzumuth:	Double;
	pointToPrj:		TPoint;
	pointToGeo:		TPoint;
	pointFromGeo:	TPoint;
begin
	pointFromGeo := nil;
	pointToGeo := nil;
	pointToPrj := PointAlongPlane (pointPrj, DirInRadian, 10.0);

	try
		pointFromGeo := _geoOper.geoTransformations (pointPrj, prjSR, geoSR).AsPoint;
		pointToGeo := _geoOper.geoTransformations (pointToPrj, prjSR, geoSR).AsPoint;
		ReturnGeodesicAzimuth_MathFunctions(pointFromGeo.X, pointFromGeo.Y, pointToGeo.X, pointToGeo.Y, directAzumuth, inverseAzumuth);
		result := directAzumuth;
	finally
		pointFromGeo.Free;
		pointToGeo.Free;
		pointToPrj.Free;
	end;
end;

function ReturnAngleInDegrees(pointFrom, pointTo: TPoint): Double;
begin
	result := RadToDeg (ArcTan2 (pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X))
end;

function ReturnAngleInRadians(pointFrom, pointTo: TPoint): Double;
begin
	result := ArcTan2 (pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X)
end;
//==============================================================================
function LocalToPrj(Center: TPoint; DirInRadian, X, Y: Double): TPoint;
var
	SinA, CosA:		Extended;
	Xnew, Ynew:		Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	Xnew := Center.X + X * CosA - Y * SinA;
	Ynew := Center.Y + X * SinA + Y * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

function LocalToPrj (Center: TPoint; DirInRadian: Double; ptPrj: TPoint): TPoint;
var
	SinA, CosA:		Extended;
	Xnew, Ynew:		Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	Xnew := Center.X + ptPrj.X * CosA - ptPrj.Y * SinA;
	Ynew := Center.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

procedure LocalToPrj (Center: TPoint; DirInRadian, X, Y: Double; var res: TPoint);
var
	SinA, CosA:		Extended;
begin
	SinCos (DirInRadian, SinA, CosA);
	res.X := Center.X + X * CosA - Y * SinA;
	res.Y := Center.Y + X * SinA + Y * CosA;
end;

procedure LocalToPrj (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var res: TPoint);
var
	SinA, CosA:		Extended;
	X, Y:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	X := ptPrj.X;	Y := ptPrj.Y;

	res.X := Center.X + X * CosA - Y * SinA;
	res.Y := Center.Y + X * SinA + Y * CosA;
end;
//==================================
function LocalToPrj (AxisLine: TLine; X, Y: Double): TPoint;
var
	SinA, CosA,
	Xnew, Ynew:		Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	Xnew := AxisLine.RefPoint.X + X * CosA - Y * SinA;
	Ynew := AxisLine.RefPoint.Y + X * SinA + Y * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

function LocalToPrj (AxisLine: TLine; ptPrj: TPoint): TPoint;
var
	SinA, CosA,
	Xnew, Ynew:		Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	Xnew := AxisLine.RefPoint.X + ptPrj.X * CosA - ptPrj.Y * SinA;
	Ynew := AxisLine.RefPoint.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

procedure LocalToPrj (AxisLine: TLine; X, Y: Double; var res: TPoint);
var
	SinA, CosA:		double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	res.X := AxisLine.RefPoint.X + X * CosA - Y * SinA;
	res.Y := AxisLine.RefPoint.Y + X * SinA + Y * CosA;
end;

procedure LocalToPrj (AxisLine: TLine; ptPrj: TPoint; var res: TPoint);
var
	SinA, CosA:		Double;
	X, Y:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];
	X := ptPrj.X;	Y := ptPrj.Y;

	res.X := AxisLine.RefPoint.X + X * CosA - Y * SinA;
	res.Y := AxisLine.RefPoint.Y + X * SinA + Y * CosA;
end;
//==============================================================================

function PrjToLocal(Center: TPoint; DirInRadian, X, Y: Double): TPoint;
var
	SinA, CosA:		Extended;
	Xnew, Ynew:		Double;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := X - Center.X;
	dY := Y - Center.Y;

	Xnew := dX * CosA + dY * SinA;
	Ynew := -dX * SinA + dY * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

procedure PrjToLocal (Center: TPoint; DirInRadian, X, Y: Double; var res: TPoint);
var
	SinA, CosA:		Extended;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := X - Center.X;
	dY := Y - Center.Y;

	res.X := dX * CosA + dY * SinA;
	res.Y := -dX * SinA + dY * CosA;
end;

procedure PrjToLocal (Center: TPoint; DirInRadian, X, Y: Double; var resX, resY: Double); 
var
	SinA, CosA:		Extended;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := X - Center.X;
	dY := Y - Center.Y;

	resX := dX * CosA + dY * SinA;
	resY := -dX * SinA + dY * CosA;
end;

function PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint): TPoint;
var
	SinA, CosA:		Extended;
	Xnew, Ynew:		Double;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := ptPrj.X - Center.X;
	dY := ptPrj.Y - Center.Y;

	Xnew := dX * CosA + dY * SinA;
	Ynew := -dX * SinA + dY * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

procedure PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var res: TPoint);
var
	SinA, CosA:		Extended;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := ptPrj.X - Center.X;
	dY := ptPrj.Y - Center.Y;

	res.X := dX * CosA + dY * SinA;
	res.Y := -dX * SinA + dY * CosA;
end;

procedure PrjToLocal (Center: TPoint; DirInRadian: Double; ptPrj: TPoint; var resX, resY: Double);
var
	SinA, CosA:		Extended;
	dX, dY:			Double;
begin
	SinCos (DirInRadian, SinA, CosA);
	dX := ptPrj.X - Center.X;
	dY := ptPrj.Y - Center.Y;

	resX := dX * CosA + dY * SinA;
	resY := -dX * SinA + dY * CosA;
end;

//==============================================================================

function PrjToLocal (AxisLine: TLine; X, Y: Double): TPoint;
var
	SinA, CosA,
	Xnew, Ynew:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := X - AxisLine.RefPoint.X;
	dY := Y - AxisLine.RefPoint.Y;

	Xnew := dX * CosA + dY * SinA;
	Ynew := -dX * SinA + dY * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

function PrjToLocal (AxisLine: TLine; ptPrj: TPoint): TPoint;
var
	SinA, CosA,
	Xnew, Ynew:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := ptPrj.X - AxisLine.RefPoint.X;
	dY := ptPrj.Y - AxisLine.RefPoint.Y;

	Xnew := dX * CosA + dY * SinA;
	Ynew := -dX * SinA + dY * CosA;
	Result := TPoint.Create(Xnew, Ynew);
end;

procedure PrjToLocal (AxisLine: TLine; X, Y: Double; var res: TPoint);
var
	SinA, CosA:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := X - AxisLine.RefPoint.X;
	dY := Y - AxisLine.RefPoint.Y;

	res.X := dX * CosA + dY * SinA;
	res.Y := -dX * SinA + dY * CosA;
end;

procedure PrjToLocal (AxisLine: TLine; X, Y: Double; var resX, resY: Double);
var
	SinA, CosA:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := X - AxisLine.RefPoint.X;
	dY := Y - AxisLine.RefPoint.Y;

	resX := dX * CosA + dY * SinA;
	resY := -dX * SinA + dY * CosA;
end;

procedure PrjToLocal (AxisLine: TLine; ptPrj: TPoint; var res: TPoint);
var
	SinA, CosA:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := ptPrj.X - AxisLine.RefPoint.X;
	dY := ptPrj.Y - AxisLine.RefPoint.Y;

	res.X := dX * CosA + dY * SinA;
	res.Y := -dX * SinA + dY * CosA;
end;

procedure PrjToLocal (AxisLine: TLine; ptPrj: TPoint; var resX, resY: Double);
var
	SinA, CosA:		Double;
	dX, dY:			Double;
begin
	AxisLine.DirVector.Length := 1.0;
	CosA := AxisLine.DirVector.Component[0];
	SinA := AxisLine.DirVector.Component[1];

	dX := ptPrj.X - AxisLine.RefPoint.X;
	dY := ptPrj.Y - AxisLine.RefPoint.Y;

	resX := dX * CosA + dY * SinA;
	resY := -dX * SinA + dY * CosA;
end;

//==============================================================================

function Point2LineDistancePrj (point, pointLine: TPoint; DirInRadian: Double): Double;
var
	CosA, SinA:		Extended;
begin
	SinCos (DirInRadian, SinA, CosA);
	result := Abs((point.Y - pointLine.Y) * CosA - (point.X - pointLine.X) * SinA)
end;

function PointToLineDistance(RefPoint, LinePoint: TPoint; DirInRadian: Double): Double;
var
	CosA, SinA:		Extended;
begin
	SinCos (DirInRadian, SinA, CosA);
	result := (RefPoint.Y - LinePoint.Y) * CosA - (RefPoint.X - LinePoint.X) * SinA
end;

function PointToLineDistance(RefPoint: TPoint; Line: TLine): Double;
var
	CosA, SinA:		Extended;
begin
	CosA := Line.DirVector.Component[0];
	SinA := Line.DirVector.Component[1];
	result := (RefPoint.Y - Line.RefPoint.Y) * CosA - (RefPoint.X - Line.RefPoint.X) * SinA
end;
{
function PointToSegmentDistance(RefPoint, SegPoint1, SegPoint2: TPoint): Double;
var
	dx, dy,
	dx0, dy0,
	dx1, dy1:       Extended;

	SegLen,
	Dist0, Dist1:   Double;
begin

	dx := SegPoint2.X - SegPoint1.X;
	dy := SegPoint2.Y - SegPoint1.Y;

	dx0 := RefPoint.X - SegPoint1.X;
	dy0 := RefPoint.Y - SegPoint1.Y;

	dx1 := RefPoint.X - SegPoint2.X;
	dy1 := RefPoint.Y - SegPoint2.Y;

	SegLen := Hypot(dy, dx);
	Dist0 := Hypot(dy0, dx0);
	Dist1 := Hypot(dy1, dx1);

	if SegLen < EpsilonDistance then
	begin
		Result := IfThen(Dist0 < Dist1, Dist0, Dist1);
		exit;
	end;

	result := Abs(dy1 * dx0 - dx1 * dy0)/SegLen;
end;
}

function PointToLineSegmentDistance(RefPoint, SegPoint1, SegPoint2: TPoint): Double;
var
	dx,  dy,
	dx0, dy0,
	dx1, dy1,
	u, SegLen:	   Double;
begin
	dx := SegPoint2.X - SegPoint1.X;
	dy := SegPoint2.Y - SegPoint1.Y;

	dx0 := RefPoint.X - SegPoint1.X;
	dy0 := RefPoint.Y - SegPoint1.Y;

	dx1 := RefPoint.X - SegPoint2.X;
	dy1 := RefPoint.Y - SegPoint2.Y;

	SegLen := dx*dx + dy*dy;
	u := dx0*dx + dy0*dy;

	if (SegLen < EpsilonDistance*EpsilonDistance) or (u <= 0) or (u >= SegLen) then
	begin
		if u < 0 then
			result := Hypot(dy0, dx0)
		else
			result := Hypot(dy1, dx1)
	end
	else
	begin
		u := u / SegLen;
		dx := dx0 - u * (SegPoint2.X - SegPoint1.X);
		dy := dy0 - u * (SegPoint2.Y - SegPoint1.Y);
		result := Hypot(dy, dx)
	end;
end;

function PointToPartDistance(Point: TPoint; Part: TPart): Double;
var
	I, N:			Integer;
	distMin, d0:	Double;

	Len2:			Double;
begin
	N := Part.Count;
	distMin := PointToLineSegmentDistance(Point, Part.Point[0], Part.Point[1]);

	for I := 1 to N - 2 do
	begin
		d0 := PointToLineSegmentDistance(Point, Part.Point[I], Part.Point[I + 1]);

		if d0 < distMin then
			distMin := d0;
	end;

	result := distMin;
end;

function PointToRingDistance(Point: TPoint; Ring: TRing): Double;
var
	I, J, N:		Integer;
	Len2,
	distMin,
	X0, X, Eps,
	dXp, dYp,
	dX, dY, d0:		Double;
	P0, P1:			TPoint;
begin
	Eps := EpsilonDistance*EpsilonDistance;

	N := Ring.Count;
	I := N - 1;

	P0 := Ring.Point[I];
	dXp := Point.X - P0.X;
	dYp := Point.Y - P0.Y;
	distMin := Hypot(dYp, dXp);

	repeat
		I := (I + 1) and (0 - Integer(I + 1 < N));
		if I = N - 1 then
		begin
			result := distMin;
			exit;
		end;
		P1 := Ring.Point[I];
		dX := P1.X - P0.X;
		dY := P1.Y - P0.Y;
		Len2 := dY * dY + dX * dX;
	until Len2 >= Eps;

	dXp := Point.X - P0.X;
	dYp := Point.Y - P0.Y;

	X0 := (dXp * dX + dYp * dY) - Len2;

	while I < N do
	begin
		P0 := P1;
		J := I;
		repeat
			J := (J + 1) and (0 - Integer(J + 1 < N));
			if J = I then
			begin
				result := distMin;
				exit;
			end;
			P1 := Ring.Point[J];
			dX := P1.X - P0.X;
			dY := P1.Y - P0.Y;
			Len2 := dY * dY + dX * dX;
		until Len2 >= Eps;

		dXp := Point.X - P0.X;
		dYp := Point.Y - P0.Y;
		X := dXp * dX + dYp * dY;

		if X > 0 then
		begin
			if X < Len2 then
			begin
				d0 := Abs(dYp * dX - dXp * dY)/Sqrt(Len2);
				if d0 < distMin then distMin := d0;
			end;
		end
		else if X0 > 0 then
		begin
			d0 := Hypot(dYp, dXp);
			if d0 < distMin then distMin := d0;
		end;

		X0 := X - Len2;

		if J < I then break;
		I := J;
	end;

	result := distMin;
end;

function PointToPolylineDistance(Point: TPoint; Polyline: TPolyline): Double;
var
	I, N:			Integer;
	distMin, d0:	Double;
begin
	N := Polyline.Count;
	distMin := PointToPartDistance(Point, Polyline.Part[0]);

	for I := 1 to N - 1 do
	begin
		d0 := PointToPartDistance(Point, Polyline.Part[I]);

		if d0 < distMin then
			distMin := d0;
	end;

	result := distMin;
end;
//==============================================================================

function PointAlongPlane (ptOrigin: TPoint; DirInRadian, Dist: Double): TPoint;
var
	SinA, CosA:		Extended;
begin
	SinCos(DirInRadian, SinA, CosA);
	result := TPoint.Create(ptOrigin.X + Dist * CosA, ptOrigin.Y + Dist * SinA);
end;

function LineLineIntersect (point1: TPoint; DirInRadian1: Double; point2: TPoint; DirInRadian2: Double): TGeometry;
const
	Eps = 0.0001;
var
	d, Ua, Ub, k,
	cosF1, sinF1,
	cosF2, sinF2:	Double;
begin
	cosF1 := Cos (DirInRadian1);
	sinF1 := Sin (DirInRadian1);

	cosF2 := Cos (DirInRadian2);
	sinF2 := Sin (DirInRadian2);

	d := sinF2 * cosF1 - cosF2 * sinF1;
	Ua := cosF2 * (point1.Y - point2.Y) - sinF2 * (point1.X - point2.X);
	Ub := cosF1 * (point1.Y - point2.Y) - sinF1 * (point1.X - point2.X);

	if Abs(d) < Eps then
	begin
		if Abs(Ua) + Abs(Ub) < 2*Eps then
			result := Tline.Create(point1, DirInRadian1)
		else
			result := TNullGeometry.Create;

		exit;
	end;

	result := TPoint.Create;
	k := Ua/d;
	result.AsPoint.X := point1.X + k * cosF1;
	result.AsPoint.Y := point1.Y + k * sinF1;
end;

function LineLineIntersect(Line1, Line2: TLine): TGeometry;
const
	Eps = 0.0001;
var
	d, Ua, Ub, k,
	cosF1, sinF1,
	cosF2, sinF2:	Double;
begin
	cosF1 := Line1.DirVector.Component[0];
	sinF1 := Line1.DirVector.Component[1];

	cosF2 := Line2.DirVector.Component[0];
	sinF2 := Line2.DirVector.Component[1];

	d := sinF2 * cosF1 - cosF2 * sinF1;

	Ua := cosF2 * (Line1.RefPoint.Y - Line2.RefPoint.Y) -
		  sinF2 * (Line1.RefPoint.X - Line2.RefPoint.X);

	Ub := cosF1 * (Line1.RefPoint.Y - Line2.RefPoint.Y) -
		  sinF1 * (Line1.RefPoint.X - Line2.RefPoint.X);

	if Abs(d) < Eps then
	begin
		if Abs(Ua) + Abs(Ub) < 2*Eps  then
			result := Line1.Clone
		else
			result := TNullGeometry.Create;
		exit;
	end;

	k := Ua/d;
	result := TPoint.Create;
	result.AsPoint.X := Line1.RefPoint.X + k * cosF1;
	result.AsPoint.Y := Line1.RefPoint.Y + k * sinF1;
end;

var	cnt:	integer = 0;

function FindRingLatestPoint (Ring: TRing; ptVector: TPoint; Direction, OutDir: Double; var d: Double; FindNearest: Boolean = False): TPoint; overload;
var
	I, N:				Integer;
	nearDist, farDist,
	fTmp, tmpDist,
	bisect:				Double;
	farPT, PE:			TPoint;

	TEST:				Boolean;
	Polygon:			TPolygon;
begin
TEST := False;

	N := Ring.Count;
	result := nil;
	if N < 2 then exit;

	fTmp := Modulus(Direction - OutDir, C_2xPI);

	if fTmp > PI then
	begin
		fTmp := Modulus(OutDir - Direction, C_2xPI);
		bisect := Direction + 0.5 * fTmp;
	end
	else
		bisect := OutDir + 0.5 * fTmp;

	farDist := 0;

	for I := 0 to N - 1 do
	begin
		PE := Ring.Point[I];
		tmpDist := Sqr(ptVector.X - PE.X) + Sqr(ptVector.Y - PE.Y);
		if tmpDist > farDist then
			farDist := tmpDist;
	end;

IF TEST then
	farDist := 1 * (Sqrt(farDist) + 1)
ELSE
	farDist := 1 * (Sqrt(farDist) + 1);
//	farDist := 10 * (Sqrt(farDist) + 1);

	farPT := LocalToPrj(ptVector, bisect, farDist, 0);

IF TEST then
	GUI.DrawPointWithText(farPT, 255, 'farPT-' + IntToStr(cnt));

	nearDist := Sqr(100*farDist);
	Result := TPoint.Create;

	for I := 0 to N - 1 do
	begin
		PE := Ring.Point[I];
		tmpDist := Sqr(farPT.X - PE.X) + Sqr(farPT.Y - PE.Y);
		if tmpDist < nearDist then
		begin
			nearDist := tmpDist;
			Result.Assign(PE);
		end;
	end;
IF TEST then
begin
	GUI.DrawPointWithText(Result, 255, 'Result-' + IntToStr(cnt));
//	Inc(cnt);
//	if cnt = 3 then
	begin
		Polygon := TPolygon.Create;
		Polygon.AddRing(Ring);
		GUI.DrawPolygon(Polygon, 255, sfsCross);
		Polygon.Free;
	end;
END;

	farPT.Free;
	d := Abs(PointToLineDistance(Result, ptVector, Direction));
end;

function FindPolygonLatestPoint (Polygon: TPolygon; ptVector: TPoint; Direction, OutDir: Double; var d: Double; FindNearest: Boolean = False): TPoint;
var
	I, N:	Integer;
	ptTmp:	TPoint;
	Dist:	Double;
begin
	N := Polygon.Count;
	Result := nil;
	if N = 0 then	exit;

	Result := FindRingLatestPoint (Polygon.Ring[0].Create, ptVector, Direction, OutDir, d, FindNearest);
	Dist := d;

	for I := 1 to N - 1 do
	begin
		ptTmp := FindRingLatestPoint (Polygon.Ring[I].Create, ptVector, Direction, OutDir, d, FindNearest);

		if Assigned(ptTmp) then
		begin
			if (not Assigned(Result)) or (FindNearest and (Dist< d)) or ((Dist > d) and (not FindNearest )) then
			begin
				Result := ptTmp;
				d := Dist
			end
		end;
	end;
end;

function RingVectorIntersect (Ring: TRing; Line: TLine; var d: Double; FindNearest: Boolean = False): TPoint; overload;
var
	I, J, N:			Integer;
	SinA, CosA,
	dXE, dYE, X,
	X0, Y0, X1, Y1:		Double;
	HaveIntersection: Boolean;
	PE:				TPoint;
begin
	SinA := Line.DirVector.Component[1];
	CosA := Line.DirVector.Component[0];
	N := Ring.Count;
	result := nil;
	if N < 2 then exit;

	PE := Ring.Point[0];
	X1 := (PE.X - Line.RefPoint.X) * CosA + (PE.Y - Line.RefPoint.Y) * SinA;
	Y1 := -(PE.X - Line.RefPoint.X) * SinA + (PE.Y - Line.RefPoint.Y) * CosA;

	HaveIntersection := False;

	for I := 1 to N + 1 do
	begin
		X0 := X1;
		Y0 := Y1;

		J := I and (0 - Integer(I < N));
		PE := Ring.Point[J];
		X1 := (PE.X - Line.RefPoint.X) * CosA + (PE.Y - Line.RefPoint.Y) * SinA;
		Y1 := -(PE.X - Line.RefPoint.X) * SinA + (PE.Y - Line.RefPoint.Y) * CosA;

		if (Y0 * Y1 > 0) or ((X0 < 0)and(X1 < 0)) then
			continue;

		dXE := X1 - X0;
		dYE := Y1 - Y0;

		if Abs(dYE) < EpsilonDistance then	X := X0
		else								X := X0 - Y0*dXE/dYE;

		if(X >= 0) and((not HaveIntersection)or(FindNearest and (X < d))or((not FindNearest) and (X > d))) then
			d := X;

		HaveIntersection := True;
	end;

	if HaveIntersection then
		result := LocalToPrj(Line, d, 0);
end;

function RingVectorIntersect (Ring: TRing; ptVector: TPoint; Direction: Double; var d: Double; FindNearest: Boolean = False): TPoint; overload;
var
	I, J, N:		Integer;
	SinA, CosA:		Extended;
	dXE, dYE, X,
	X0, Y0, X1, Y1:	Double;
	HaveIntersection: Boolean;
	PE:				TPoint;
begin
	SinCos(Direction, SinA, CosA);

	N := Ring.Count;
	result := nil;
	if N < 2 then exit;

	PE := Ring.Point[0];
	X1 := (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA;
	Y1 := -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA;

	HaveIntersection := False;

	for I := 1 to N + 1 do
	begin
		X0 := X1;
		Y0 := Y1;

		J := I and (0 - Integer(I < N));
		PE := Ring.Point[J];

		X1 := (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA;
		Y1 := -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA;

		if (Y0 * Y1 > 0) or ((X0 < 0)and(X1 < 0)) then
			continue;

		dXE := X1 - X0;
		dYE := Y1 - Y0;

		if Abs(dYE) < EpsilonDistance then	X := X0
		else								X := X0 - Y0 * dXE/dYE;

		if(X >= 0)and((not HaveIntersection)  or
			(FindNearest and (X < d))or
		((not FindNearest) and (X > d))) then
			d := X;

		HaveIntersection := True;
	end;

	if HaveIntersection then
		result := LocalToPrj(ptVector, Direction, d, 0);
end;

function PolygonVectorIntersect (Polygon: TPolygon; Line: TLine; var d: Double; FindNearest: Boolean = False): TPoint; overload;
var
	I, N:	Integer;
	ptTmp:	TPoint;
	Dist:	Double;
begin
	N := Polygon.Count;
	Result := nil;
	if N = 0 then	exit;

	Result := RingVectorIntersect (Polygon.Ring[0].Create, Line, d, FindNearest);

	for I := 1 to N - 1 do
	begin
		ptTmp := RingVectorIntersect (Polygon.Ring[I].Create, Line, Dist, FindNearest);

		if Assigned(ptTmp) then
		begin
			if (not Assigned(Result)) or (FindNearest and (Dist< d)) or ((Dist > d) and (not FindNearest )) then
			begin
				Result := ptTmp;
				d := Dist
			end
		end;
	end;
end;

function PolygonVectorIntersect (Polygon: TPolygon; ptVector: TPoint; Direction: Double; var d: Double; FindNearest: Boolean = False): TPoint; overload;
var
	I, N:	Integer;
	ptTmp:	TPoint;
	Dist:	Double;
begin
	N := Polygon.Count;
	Result := nil;
	if N = 0 then	exit;

	Result := RingVectorIntersect (Polygon.Ring[0].Create, ptVector, Direction, d, FindNearest);

	for I := 1 to N - 1 do
	begin
		ptTmp := RingVectorIntersect (Polygon.Ring[I].Create, ptVector, Direction, Dist, FindNearest);

		if Assigned(ptTmp) then
		begin
			if (not Assigned(Result)) or (FindNearest and (Dist< d)) or ((Dist > d) and (not FindNearest )) then
			begin
				Result := ptTmp;
				d := Dist
			end
		end;
	end;
end;

function CutRingByNNLine(Ring: TRing; NNLine: TPart; var LeftRing, RightRing: TRing): Integer;
var
	I, I0, I1, J, N, M,
	IxA, IxB:			Integer;

	dA, dB, dX, dY,
	dXE, dYE, X,
	Len, K,
	SinA, CosA, SinB, CosB,
	X0A, Y0A, X1A, Y1A,
	X0B, Y0B, X1B, Y1B:		Double;
	PE,
	pt0, pt1,
	ptCenter:		TPoint;
begin
	N := Ring.Count;
	result := 0;
	LeftRing := nil;
	RightRing := nil;
	dA := 0;	//for remove compiler warning;
	dB := 0;	//for remove compiler warning;

	if N < 2 then exit;

	ptCenter := NNLine.Point[1];

	dX := NNLine.Point[0].X - ptCenter.X;
	dY := NNLine.Point[0].Y - ptCenter.Y;

	Len := Hypot(dY, dX);	K := 1.0 / Len;

	SinA := dY*K;
	CosA := dX*K;

	dX := NNLine.Point[2].X - ptCenter.X;
	dY := NNLine.Point[2].Y - ptCenter.Y;

	Len := Hypot(dY, dX);	K := 1.0 / Len;

	SinB := dY*K;
	CosB := dX*K;

	PE := Ring.Point[0];

	X1A := (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
	Y1A := -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;

	X1B := (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
	Y1B := -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;

	IxA := -1;	IxB := -1;

	for I := 0 to N do
	begin
		X0A := X1A;
		Y0A := Y1A;

		X0B := X1B;
		Y0B := Y1B;

		J := (I + 1) and (0 - Integer(I + 1 < N));
		PE := Ring.Point[J];

		X1A := (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
		Y1A := -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;

		X1B := (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
		Y1B := -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;

		if (Y0A * Y1A <= 0) and ((X0A >= 0)or(X1A >= 0)) then
		begin
			dXE := X1A - X0A;
			dYE := Y1A - Y0A;

			if Abs(dYE) < EpsilonDistance then	X := X0A
			else								X := X0A - Y0A*dXE/dYE;

			if (X >= 0)and((IxA < 0)or(X < dA)) then
			begin
				dA := X;
				IxA := I;
			end;
		end;

		if (Y0B * Y1B <= 0) and ((X0B >= 0)or(X1B >= 0)) then
		begin
			dXE := X1B - X0B;
			dYE := Y1B - Y0B;

			if Abs(dYE) < EpsilonDistance then	X := X0B
			else								X := X0B - Y0B*dXE/dYE;

			if(X >= 0)and((IxB < 0)or(X < dB))then
			begin
				dB := X;
				IxB := I;
			end;
		end;
	end;

	if (IxA >= 0)and (IxB >= 0)then
	begin
		X0A := ptCenter.X + dA * CosA;
		Y0A := ptCenter.Y + dA * SinA;

		X0B := ptCenter.X + dB * CosB;
		Y0B := ptCenter.Y + dB * SinB;

		if IxA  = IxB then
		begin
									result := 2;
									exit;
		end;

		LeftRing := TRing.Create;
		RightRing := TRing.Create;

		pt0 := TPoint.Create(X0A, Y0A);
		pt1 := TPoint.Create(X0B, Y0B);
		M := (IxA - IxB) Mod N;
		if M <= 0 then Inc(M, N);

		RightRing.AddPoint(pt0);
		RightRing.AddPoint(ptCenter);
		RightRing.AddPoint(pt1);

		for I := 1 to M do
		begin
			J := (I + IxB) Mod N;
			RightRing.AddPoint(Ring.Point[J]);
		end;

		M := N - M;

//		M := (IxB - IxA) Mod N;
//		if M < 0 then Inc(M, N);

		LeftRing.AddPoint(pt1);
		LeftRing.AddPoint(ptCenter);
		LeftRing.AddPoint(pt0);

		for I := 1 to M do
		begin
			J := (I + IxA) Mod N;
			LeftRing.AddPoint(Ring.Point[J]);
		end;
		result := 2;
		pt0.Free;
		pt1.Free;
	end;
end;

function ReturnDistanceInMeters (pointFrom, pointTo: TPoint): Double;
begin
	result := Hypot (pointTo.X - pointFrom.X, pointTo.Y - pointFrom.Y);
end;

function AngleSideDef(InRadian, OutRadian: Double): TSideDirection;
var
	Angle12, rAngle:	Double;
begin
	Angle12 := OutRadian - InRadian;

	rAngle := Modulus (Angle12, 2 * Pi);

	if (Abs (rAngle) < EpsilonRadian) or (Abs (rAngle - 2*Pi) < EpsilonRadian) or (Abs (rAngle - Pi) < EpsilonRadian) then
		result := sideOn
	else if rAngle < PI then
		result := sideLeft
	else
		result := sideRight;
end;

function SideDef (pointInLine: TPoint; lineAngleInRadian: Double; pointOutLine: TPoint): TSideDirection;
var
	Angle12, rAngle:	Double;
begin
	Angle12 := ReturnAngleInRadians (pointInLine, pointOutLine);
	rAngle := Modulus (lineAngleInRadian - Angle12, 2 * Pi);

	if (Abs (rAngle) < EpsilonRadian) or (Abs (rAngle - 2*Pi) < EpsilonRadian) or (Abs (rAngle - Pi) < EpsilonRadian) then
		result := sideOn
	else if rAngle < Pi then
		result := sideRight
	else
		result := sideLeft;
end;

function SpiralTouchAngle (r0, coef0, nominalDir, touchDir: Double; turnDir: TSideDirection): Double;
var
	I:				Integer;
	turnAngle,
	TouchAngle,
	d, delta, coef:	Double;
begin
	TouchAngle := Modulus ((nominalDir - touchDir) * Integer (turnDir), C_2xPI);
	turnAngle := TouchAngle;
	coef := RadToDeg(coef0);

	for I := 0 to 9 do
	begin
		d := coef / (r0 + coef * turnAngle);
		delta := (turnAngle - TouchAngle - ArcTan(d)) / (2 - 1 / (d * d + 1));
		turnAngle := turnAngle - delta;
		If (Abs(delta) < EpsilonRadian) then	break;
	end;
	result := turnAngle;
	if result < 0.0 then
		result := Modulus (result, C_2xPI);
end;

function CreateWindSpiral (
			Pt: TPoint;
			radianNominalDir,
			radianStartDir,
			radianEndDir,
			startRadius,
			coefficient: Double;
			turnSide: TSideDirection): TMultiPoint;
			//var startDphi: Double;
			//var endDphi: Double): TMultiPoint;
var
	I, N, TurnDir:	Integer;
	dAlpha, azt0,
	startDphi,
	endDphi,
	TurnAng, R:	Double;
	ptCur, PtCnt:	TPoint;
begin
	TurnDir := Integer (turnSide);
	result := TMultiPoint.Create;

	PtCnt := PointAlongPlane (Pt, radianNominalDir - 0.5*Pi*TurnDir, startRadius);

	if SubtractAngles (radianNominalDir, radianEndDir) < EpsilonRadian then
		radianEndDir := radianNominalDir;

	startDphi := Modulus ((radianStartDir - radianNominalDir) * TurnDir, 2*Pi);

	if startDphi < EpsilonRadian then	startDphi := 0
	else								startDphi := SpiralTouchAngle (startRadius, coefficient,
												radianNominalDir, radianStartDir, turnSide);

	endDphi := SpiralTouchAngle (startRadius, coefficient, radianNominalDir, radianEndDir, turnSide);
	TurnAng := endDphi - startDphi;
	azt0 := Modulus (radianNominalDir - (startDphi - 0.5*Pi) * TurnDir, 2*Pi);

	if TurnAng >= 0 then
	begin
		N := Floor(RadToDeg(TurnAng));

		if N = 0 then 		N := 1
		else if N < 10 then N := 10;

		dAlpha := TurnAng / N;

		for I := 0 to N do
		begin
			R := startRadius + (RadToDeg (dAlpha) * coefficient * I) + RadToDeg (startDphi) * coefficient;
			ptCur := PointAlongPlane (PtCnt, azt0 - I * dAlpha * TurnDir, R);
			result.AddPoint (ptCur);
			ptCur.Free;
		end
	end;
	PtCnt.Free;
end;

function IsPointInPoly(Point: TPoint; Polygon: TPolygon): Boolean;
var
	I, J, K, L, C, N:	Integer;
	CRing:				TRing;
	P0, P1, P2:			TPoint;
	fTmp, Eps:			Double;
	X0, Y0, X1, Y1,
	dX, dY, signX,
	signY:				TIntDouble;
begin
	C := 0;
	Eps := EpsilonDistance*EpsilonDistance;
	P2 := nil;		//for remove compiler warning;
	fTmp := 0;		//for remove compiler warning;

	for J := 0 to Polygon.Count - 1 do
	begin
		CRing := Polygon.Ring[J];
		N := CRing.Count;
		if N < 3 then	continue;

		P0 := CRing.Point[N-1];
		P1 := CRing.Point[0];

		dX.AsDouble := P1.X - P0.X;
		dY.AsDouble := P1.Y - P0.Y;

		if Sqr(dX.AsDouble) + Sqr(dY.AsDouble) < Eps then
			Dec(N);
		if N < 3 then	continue;

		Y1.AsDouble := P1.Y - Point.Y;
		X1.AsDouble := P1.X - Point.X;

		I := 0;		K := 0;

		while K >= I do
		begin
			I := K;

			P0 := P1;	X0 := X1;	Y0 := Y1;
			repeat
				K := (K + 1) and (0 - Integer(K + 1 < N));
				if K = I then break;
				P1 := CRing.Point[K];

				dX.AsDouble := P1.X - P0.X;
				dY.AsDouble := P1.Y - P0.Y;
			until Sqr(dX.AsDouble) + Sqr(dY.AsDouble) >= Eps;
			if K = I then break;

//========= Skip Horisontals ==========================================
			if dY.AsInt64 and $7FFFFFFFFFFFFFFF = 0  then	continue;

//=====================================================================

			X1.AsDouble := P1.X - Point.X;
			Y1.AsDouble := P1.Y - Point.Y;

//========= Skip left side edges ======================================
			if(X0.AsInteger[1] < 0)and(X1.AsInteger[1] < 0) then	continue;

//========= Skip if not intersect test line  ==========================
			signY.AsDouble := Y0.AsDouble * Y1.AsDouble;
			if signY.AsInt64 > 0 then	continue;

//========= Already analised ==========================================
			if Y0.AsInt64 and $7FFFFFFFFFFFFFFF = 0 then			continue;

//========= Point P1 lies on the test line ============================
			if Y1.AsInt64 and $7FFFFFFFFFFFFFFF = 0 then
			begin
				L := K;
				repeat
					L := (L + 1) and (0 - Integer(L + 1 < N));
					if L = K then break;

					P2 := CRing.Point[L];
					fTmp := P2.Y - P1.Y;
				until Sqr(fTmp) + Sqr(P2.X - P1.X) >= Eps;
				if L = K then break;

				signY.AsDouble := fTmp * dY.AsDouble;

				if(signY.AsInt64 and $7FFFFFFFFFFFFFFF <> 0) and
				  (signY.AsInteger[1] < 0) then						continue;
			end;
//=====================================================================

			signX.AsDouble := X0.AsDouble * X1.AsDouble;
			if signX.AsInt64 and $7FFFFFFFFFFFFFFF = 0 then
				signX.AsInteger[1] := 0;

			if (signX.AsInt64 > 0) or
				(X0.AsDouble * dY.AsDouble * dY.AsDouble >=
				Y0.AsDouble * dY.AsDouble * dX.AsDouble) then
				Inc(C);
		end;

		if C and 1 = 1 then break;
	end;

	result := C and 1 = 1;
end;
{
function PointToRingDistanceF(Point: TPoint; Ring: TRing): Double;
var
	I, Ix, J,
	N, M, Mn:		Integer;
	distMin:		Double;
	dX, dY, dXY,
	dXMin, dYMin,
	dXYMin, Len, K,
	dXp, dYp,
	X, d0, Eps:		Double;
	PtTmp:			TPoint;
	Pt0, Pt1:		TPoint;
	IArray:			Array [0..2] of Integer;
	IxArray:		Array [0..2] of Integer;
begin
	N := Ring.Count;
	Eps := EpsilonDistance*EpsilonDistance;

	IArray[0] := 0;
	IArray[1] := 0;
	IArray[2] := 0;

	PtTmp := Ring.Point[0];
	dXMin := Abs(PtTmp.X - Point.X);
	dYMin := Abs(PtTmp.Y - Point.Y);
	dXYMin := dXMin + dYMin;
	distMin := Hypot(dXMin, dYMin);

	for I := 1 to N-1 do
	begin
		PtTmp := Ring.Point[I];
		dX := Abs(PtTmp.X - Point.X);
		dY := Abs(PtTmp.Y - Point.Y);
		dXY := dX + dY;

		if dX < dXMin then
		begin
			dXMin := dX;
			IArray[0] := I;
		end;

		if dY < dYMin then
		begin
			dYMin := dY;
			IArray[1] := I;
		end;

		if dXY < dXYMin then
		begin
			dXYMin := dXY;
			IArray[2] := I;
		end;
	end;

	Mn := 2;
	I := 0;
	while I < Mn -1 do
	begin
		J := I + 1;
		while J < Mn do
		begin
			if IArray[I] = IArray[J] then
			begin
				if J < 2 then
					IArray[J] := IArray[J+1];
				Dec(Mn);
			end
			else
				Inc(J);
		end;
		Inc(I);
	end;

	for M := 0 to Mn do
	begin
		IxArray[1] := IArray[M];

		I := IxArray[1];
		J := (I + N - 1) mod N;

		Pt0 := Ring.Point[I];
		Pt1 := Ring.Point[J];

		dX := Pt0.X - Pt1.X;
		dY := Pt0.Y - Pt1.Y;
		Len := dY*dY + dX*dX;

		while Len < Eps do
		begin
			J := (J + N - 1) mod N;
			Pt1 := Ring.Point[J];
			dX := Pt0.X - Pt1.X;
			dY := Pt0.Y - Pt1.Y;
			Len := dY*dY + dX*dX;
		end;
		IxArray[0] := J;

		J := (I + 1) mod N;
		Pt1 := Ring.Point[J];
		dX := Pt0.X - Pt1.X;
		dY := Pt0.Y - Pt1.Y;
		Len := dY*dY + dX*dX;

		while Len < Eps do
		begin
			J := (J + 1) mod N;
			Pt1 := Ring.Point[J];
			dX := Pt0.X - Pt1.X;
			dY := Pt0.Y - Pt1.Y;
			Len := dY*dY + dX*dX;
		end;
		IxArray[2] := J;

		for Ix := 0 to 1 do
		begin
			I := IxArray[Ix];
			J := IxArray[Ix + 1];

			Pt0 := Ring.Point[I];
			Pt1 := Ring.Point[J];

			dX := Pt1.X - Pt0.X;
			dY := Pt1.Y - Pt0.Y;

			Len := Hypot(dY, dX);
			K := 1/Len;

			dXp := Point.X - Pt0.X;
			dYp := Point.Y - Pt0.Y;

			X := K*(dXp * dX + dYp * dY);

			if X < 0 then
			begin
				d0 := Hypot(dYp, dXp);
				if d0 < distMin then distMin := d0;
			end
			else if X > Len then
			begin
				dX := Pt1.X - Point.X;
				dY := Pt1.Y - Point.Y;
				d0 := Hypot(dY, dX);
				if d0 < distMin then distMin := d0;
			end
			else
			begin
				d0 := Abs(K*(-dXp * dY + dYp * dX));
				if d0 < distMin then distMin := d0;
			end
		end;
	end;

	result := distMin;
end;

function PointToRingDistanceOld(Point: TPoint; Ring: TRing): Double;
var
	P0, P1:			TPoint;
	I, J, N:		Integer;
	distMin, Eps,
	Len2, dist,
	dX, dY, X0, X1,
	dX0, dY0,
	dX1, dY1:		Double;
	d0:				Double;
begin
	N := Ring.Count;
	J := N - 1;
	Eps := EpsilonDistance*EpsilonDistance;

	P0 := Ring.Point[J];
	dX0 := P0.X - Point.X;
	dY0 := P0.Y - Point.Y;
	distMin := Hypot(dY0, dX0);

	repeat
		J := (J + 1) and (0 - Integer(J + 1 < N));
		if J = N - 1 then
		begin
			result := distMin;
			exit;
		end;
		P1 := Ring.Point[J];
		dX := P1.X - P0.X;
		dY := P1.Y - P0.Y;
		Len2 := dX*dX + dY*dY
	until Len2 >= Eps;

	dX1 := P1.X - Point.X;
	dY1 := P1.Y - Point.Y;
	X1 := dX1 * dX + dY1 * dY;

	I := J;
	while I < N do
	begin
		P0 := P1;
		repeat
			J := (J + 1) and (0 - Integer(J + 1 < N));
			if J = I then
			begin
				result := distMin;
				exit;
			end;
			P1 := Ring.Point[J];
			dX := P1.X - P0.X;
			dY := P1.Y - P0.Y;
			Len2 := dX*dX + dY*dY
		until Len2 >= Eps;

		dX0 := dX1;
		dY0 := dY1;
		d0 := X1;

		dX1 := P1.X - Point.X;
		dY1 := P1.Y - Point.Y;

		X0 := dX0 * dX + dY0 * dY;
		X1 := dX1 * dX + dY1 * dY;

		if X0 * X1 <= 0 then
		begin
			dist := Abs(dX0 * dY - dY0 * dX)/Sqrt(Len2);
			if distMin > dist then distMin := dist;
		end
		else if(d0 > 0)and(X0 < 0)then
		begin
			dist := Hypot(dY0, dX0);
			if distMin > dist then distMin := dist;
		end;

		if J < I then	break;
		I := J;
	end;
	result := distMin;
end;
}

function CalcArea(Ring: TRing): Double;
var
	I, N:	Integer;
	S:			Double;
begin
	Result := 0;
	N := Ring.Count;

	if N < 3 then	exit;

	S := Ring.Point[N-1].X * Ring.Point[0].Y - Ring.Point[0].X * Ring.Point[N - 1].Y;

	for I := 0 to Ring.Count - 2 do
		S := S + Ring.Point[I].X * Ring.Point[I + 1].Y - Ring.Point[I + 1].X * Ring.Point[I].Y;

	Result := S;
end;

function CalcArea(Polygon: TPolygon): Double;
var
	I:	Integer;
begin
	Result := 0;
	for I := 0 to Polygon.Count - 1 do
		Result := Result + CalcArea(Polygon.Ring[I]);
end;

function CalcAbsArea(Ring: TRing): Double;
var
	I, N:		Integer;
	S:			Double;
begin
	Result := 0;
	N := Ring.Count;

	if N < 3 then	exit;

	S := Ring.Point[N-1].X * Ring.Point[0].Y - Ring.Point[0].X * Ring.Point[N - 1].Y;

	for I := 0 to Ring.Count - 2 do
		S := S + Ring.Point[I].X * Ring.Point[I + 1].Y - Ring.Point[I + 1].X * Ring.Point[I].Y;

	Result := Abs(S);
end;

function CalcAbsArea(Polygon: TPolygon): Double;
var
	I:	Integer;
begin
	Result := 0;
	for I := 0 to Polygon.Count - 1 do
		Result := Result + CalcAbsArea(Polygon.Ring[I]);
end;

function CircleVectorIntersect (CentPoint: TPoint; Radius: Double; Line: TLine; var d: Double): TPoint;
var
	Line1:		TLine;
	geom:		TGeometry;
	distToVect:	Double;
begin
	result := nil;
	Line1 := TLine.Create (centPoint, Line.DirVector.Direction + 0.5*PI);

	geom := LineLineIntersect (Line, Line1);
	Line1.Free;

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CentPoint.X - geom.AsPoint.X,  CentPoint.Y - geom.AsPoint.Y);
	d :=  -1.0;

	if distToVect < radius then
	begin
		d :=  Sqrt (Sqr(radius) - Sqr(distToVect));
		result := PointAlongPlane (geom.AsPoint, line.DirVector.Direction, d);
	end;

	geom.Free;
end;

function CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double; var d: Double): TPoint;
var
	geom:		TGeometry;
	distToVect:	Double;
begin
	result := nil;
	geom := LineLineIntersect (ptVector, Direction, CenterPoint, Direction + 0.5 * PI);

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CenterPoint.X - geom.AsPoint.X,  CenterPoint.Y - geom.AsPoint.Y);
	d :=  -1.0;

	if distToVect < Radius then
	begin
		d :=  Sqrt (Sqr(Radius) - Sqr(distToVect));
		result := PointAlongPlane (geom.AsPoint, Direction, d);
	end;

	geom.Free;
end;

function CircleVectorIntersect (centPoint: TPoint; radius: Double; Line: TLine): Double;
var
	Line1:		TLine;
	geom:		TGeometry;
	distToVect:	Double;
	ptTmp:		TPoint;
begin
	Line1 := TLine.Create (centPoint, Line.DirVector.Direction + 0.5*PI);

	geom := LineLineIntersect (Line, Line1);
	Line1.Free;
	result :=  -1.0;

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CentPoint.X - geom.AsPoint.X,  CentPoint.Y - geom.AsPoint.Y);

	if distToVect < radius then
	begin
		result := Sqrt (Sqr(radius) - Sqr(distToVect));
		ptTmp := PointAlongPlane (geom.AsPoint, line.DirVector.Direction, result);
		ptTmp.Free;
	end;

	geom.Free;
end;

function CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double): Double;
var
	geom:		TGeometry;
	distToVect:	Double;
	ptTmp:		TPoint;
begin
	geom := LineLineIntersect (ptVector, Direction, CenterPoint, Direction + 0.5 * PI);
	result := -1.0;

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CenterPoint.X - geom.AsPoint.X,  CenterPoint.Y - geom.AsPoint.Y);

	if distToVect < Radius then
	begin
		result := Sqrt (Sqr(Radius) - Sqr(distToVect));
		ptTmp := PointAlongPlane (geom.AsPoint, Direction, result);
		ptTmp.Free;
	end;

	geom.Free;
end;

procedure CircleVectorIntersect (centPoint: TPoint; radius: Double; Line: TLine; var d: Double; var Result: TPoint);
var
	Line1:		TLine;
	geom:		TGeometry;
	distToVect:	Double;
	ptTmp:		TPoint;
begin
	if Assigned(Result) then
		Result.SetEmpty;

	Line1 := TLine.Create (centPoint, Line.DirVector.Direction + 0.5*PI);

	geom := LineLineIntersect (Line, Line1);
	Line1.Free;

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CentPoint.X - geom.AsPoint.X,  CentPoint.Y - geom.AsPoint.Y);
	d :=  -1.0;

	if distToVect < radius then
	begin
		d :=  Sqrt (Sqr(radius) - Sqr(distToVect));
		ptTmp := PointAlongPlane (geom.AsPoint, line.DirVector.Direction, d);
		if Assigned(Result) then
			Result.Assign(ptTmp);
		ptTmp.Free;
	end;

	geom.Free;
end;

procedure CircleVectorIntersect (CenterPoint: TPoint; Radius: Double; ptVector: TPoint; Direction: Double; var d: Double; var Result: TPoint);
var
	geom:		TGeometry;
	distToVect:	Double;
	ptTmp:		TPoint;
begin
	if Assigned(Result) then
		Result.SetEmpty;

	geom := LineLineIntersect (ptVector, Direction, CenterPoint, Direction + 0.5 * PI);

	if geom.GeometryType <> gtPoint then
	begin
		geom.Free;
		exit;
	end;

	distToVect := Hypot (CenterPoint.X - geom.AsPoint.X,  CenterPoint.Y - geom.AsPoint.Y);
	d :=  -1.0;

	if distToVect < Radius then
	begin
		d :=  Sqrt (Sqr(Radius) - Sqr(distToVect));
		ptTmp := PointAlongPlane (geom.AsPoint, Direction, d);
		if Assigned(Result) then
			Result.Assign(ptTmp);
		ptTmp.Free;
	end;

	geom.Free;
end;

function CreateCirclePrj(PtCnt: TPoint; Radius: Double): TRing;
var
	I:				Integer;
	AngleStep,
	iInRad:			Double;
	Pt:				TPoint;
begin
	AngleStep := DegToRad(1.0);
	Pt := TPoint.Create;
	result := TRing.Create;

	for I := 1 to 360 do
	begin
		iInRad := I * AngleStep;
		Pt.X := PtCnt.X + Radius * Cos(iInRad);
		Pt.Y := PtCnt.Y + Radius * Sin(iInRad);
		result.AddPoint(Pt);
	end;

	Pt.Free;
end;

function CreateArcPrj(PtCnt, ptFrom, ptTo: TPoint; Direction: TSideDirection): TRing;
var
	N, I:			Integer;
	AngleStep, fDir,
	R, dX, dY,
	AztFrom, AztTo,
	iInRad:			Double;
	dAz:			Double;
	Pt:				TPoint;
begin
	dX := ptFrom.X - PtCnt.X;
	dY := ptFrom.Y - PtCnt.Y;
	R := Sqrt(dX * dX + dY * dY);
	Pt := TPoint.Create;

	AztFrom := ArcTan2(dY, dX);
	AztTo := ArcTan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X);

	fDir := -Integer(Direction);
	dAz := Modulus((AztTo - AztFrom) * fDir, 2 * PI);

	N := floor(RadToDeg(dAz));
	if N = 0 then 		N := 1
	else if N < 10 then N := 10;

	AngleStep := daz / N;
	result := TRing.Create;
	result.AddPoint(ptFrom);

	for I := 1 to N - 1 do
	begin
		iInRad := AztFrom + I * AngleStep * fDir;
		Pt.X := PtCnt.X + R * Cos(iInRad);
		Pt.Y := PtCnt.Y + R * Sin(iInRad);
		result.AddPoint(pt);
	end;

	result.AddPoint(ptTo);
	Pt.Free;
end;

function DrawArcPrj(PtCnt, ptFrom, ptTo: TPoint; ClWise: Integer): TPolyline;
var
	N, I:			Integer;
	AngStep,
	R, dX, dY,
	AztFrom, AztTo,
	iInRad:			Double;
	daz:			Integer;
	Pt:		TPoint;
	part:			TPart;
begin
	dX := ptFrom.X - PtCnt.X;
	dY := ptFrom.Y - PtCnt.Y;
	R := Sqrt(dX * dX + dY * dY);

	Pt := TPoint.Create;

	AztFrom := Modulus(RadToDeg(ArcTan2(dY, dX)), 360);
	AztTo := Modulus(RadToDeg(ArcTan2(ptTo.Y - PtCnt.Y, ptTo.X - PtCnt.X)), 360);

	daz := Round(Modulus((AztTo - AztFrom) * ClWise, 360));

	AngStep := 1;
	N := floor(daz / AngStep);

	if N = 0 then 		N := 1
	else if N < 10 then N := 10;

	AngStep := daz / N;

	part := TPart.Create;
	part.AddPoint (ptFrom);

	for I := 1 to N - 1 do
	begin
		iInRad := DegToRad(AztFrom + I * AngStep * ClWise);
		Pt.X := PtCnt.X + R * Cos(iInRad);
		Pt.Y := PtCnt.Y + R * Sin(iInRad);
		part.AddPoint (pt);
	end;

	part.AddPoint (ptTo);
	result := TPolyline.Create;
	result.AddPart (part);

	part.Free;
	Pt.Free;
end;

function ConvertPointsToTrackLIne(MultiPoint: TMultiPoint): TPolyline;
var
	I, J, N:		Integer;
	Side:			TSideDirection;
	fTmp, fE:		Double;

	FromPt, CntPt,
	ToPt:			TPoint;

	Part:			TPart;
	arcPolyline:	TPolyline;
	tmpGeometry:	TGeometry;
begin
	fE := DegToRad(0.5);
	N := MultiPoint.Count - 2;

	CntPt := TPoint.Create;
	Part := TPart.Create;

	part.AddPoint(MultiPoint.Point[0]);

	for I := 0 to N do
	begin
		FromPt := MultiPoint.Point[I];
		ToPt := MultiPoint.Point[I + 1];
		fTmp := FromPt.M - ToPt.M;

		if (Abs(Sin(fTmp)) <= fE) and (Cos(fTmp) > 0) then
			part.AddPoint(ToPt)
		else
		begin
			if Abs(Sin(fTmp)) > fE then
			begin
				tmpGeometry := LineLineIntersect(FromPt, FromPt.M + 0.5*PI, ToPt, ToPt.M + 0.5 * PI);
//				if tmpGeometry.GeometryType = gtPoint then
					CntPt.Assign(tmpGeometry);
//				else
				tmpGeometry.Free;
			end
			else
				CntPt.SetCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

			Side := SideDef(FromPt, FromPt.M, ToPt);
			arcPolyline := DrawArcPrj(CntPt, FromPt, ToPt, -Integer(Side));

			for J := 0 to arcPolyline.Part[0].Count - 1 do
				Part.AddPoint (arcPolyline.Part[0].Point [J]);

			arcPolyline.Free;
		end;
	end;

	result := TPolyline.Create;
	result.AddPart(Part);

	Part.Free;
	CntPt.Free
end;

function CalcTrajectoryFromMultiPoint (lineArray: TLineArray): TPolyline;
var
	i, n, j:		Integer;
	sideDir:		TSideDirection;
	fTmp, fE:		Double;
	fromLine,
	toLine:			TLine;
	centrePoint:	TPoint;
	part:			TPart;
	arcPolyline:	TPolyline;
begin
	part := TPart.Create;

	fE := DegToRad(0.5);
	n := Length (lineArray) - 2;

	part.AddPoint (lineArray [0].RefPoint);

	for i := 0 to N do
	begin
		fromLine := lineArray [i];
		toLine := lineArray [i+1];
		fTmp := fromLine.DirVector.Direction - toLine.DirVector.Direction;

		if (Abs (Sin (fTmp)) <= fE) And (Cos (fTmp) > 0) then
			part.AddPoint (toLine.RefPoint)
		else
		begin
			if Abs (Sin(fTmp)) > fE then
				centrePoint := LineLineIntersect (
								fromLine.RefPoint, fromLine.DirVector.Direction + 0.5*Pi,
								toLine.RefPoint, toLine.DirVector.Direction + 0.5 * Pi).AsPoint
			else
				centrePoint := TPoint.Create (
									0.5 * (fromLine.RefPoint.X + toLine.RefPoint.X),
									0.5 * (fromLine.RefPoint.Y + toLine.RefPoint.Y));

			sideDir := SideDef(fromLine.RefPoint, fromLine.DirVector.Direction, toLine.RefPoint);
			arcPolyline := DrawArcPrj (centrePoint, fromLine.RefPoint, toLine.RefPoint, -1 * Integer(sideDir));
			centrePoint.Free;
			for j:=0 to arcPolyline.Part [0].Count - 1 do
			begin
				part.AddPoint (arcPolyline.Part [0].Point [j]);
			end;
			arcPolyline.Free;
		end;
	end;

	result := TPolyline.Create;
	result.AddPart (part);
	part.Free;
end;

function TangentCyrcleIntersectPoint (
			centrePoint: TPoint;
			radius: Double;
			outPoint: TPoint;
			side: TSideDirection): TPoint;
var
	dirLine, dirRadius,
	distance, alpha:	Double;
	turnVal:			Integer;
begin
	result := nil;

	distance := ReturnDistanceInMeters(centrePoint, outPoint);
	if distance < radius then
		exit;

	turnVal := Integer (side);
	alpha := ArcSin (radius / distance);
	dirLine := ReturnAngleInRadians (outPoint, centrePoint);
	dirRadius := dirLine - turnVal * (alpha + 0.5*Pi);
	dirLine := dirLine - turnVal * alpha;
	result := LineLineIntersect (outPoint, dirLine, centrePoint, dirRadius).AsPoint;
end;

function FixToTouchSprial (
			startLine,
			endLine:	TLine;
			coefficient,
			turnRadius:	Double;
			turnSide: 	TSideDirection): Double;
var
	tmpCoef,
	theta0, d, d2, r,
	f, F1, theta1,
	x1, y1, centreTheta,
	fixTheta, dTheta,
	theta1New:			Double;
	sinT, cosT:			Extended;
	turnVal, i:			Integer;
	spiralCentrePoint,
	outPoint:			TPoint;
begin
	result := 4; // greater than PI.
	turnVal := Integer (turnSide);
	turnVal := -turnVal;

	tmpCoef :=  RadToDeg (coefficient);

	theta0 := Modulus(startLine.DirVector.Direction - 0.5*Pi*turnVal, 2*Pi);
	spiralCentrePoint := PointAlongPlane (startLine.RefPoint, startLine.DirVector.Direction + 0.5*Pi*turnVal, turnRadius);
	d := ReturnDistanceInMeters (spiralCentrePoint, endLine.RefPoint);
	fixTheta := ReturnAngleInRadians (spiralCentrePoint, endLine.RefPoint);
	dTheta := Modulus ( (fixTheta - theta0) * turnVal, 2*Pi);
	r := turnRadius + RadToDeg (dTheta) * coefficient;
	if d < r then exit;

	x1 :=  endLine.RefPoint.X - spiralCentrePoint.X;
	y1 := endLine.RefPoint.Y - spiralCentrePoint.Y;
	centreTheta := SpiralTouchAngle (turnRadius, coefficient, startLine.DirVector.Direction, endLine.DirVector.Direction, turnSide);
	centreTheta := Modulus (theta0 + centreTheta * turnVal, 2*Pi);

	//---Variant Firdowsy

	theta1 := centreTheta;
	for i:=0 to 20 do
	begin
		dTheta := Modulus ((theta1 - theta0) * turnVal, 2*Pi);
		SinCos (theta1, sinT, cosT);
		r := turnRadius + RadToDeg (dTheta) * coefficient;
		f := Sqr (r) - (y1 * r + x1 * tmpCoef * turnVal) * sinT - (x1 * r - y1 * tmpCoef * turnVal) * cosT;
		F1 := 2 * r * tmpCoef * turnVal - (y1 * r + 2 * x1 * tmpCoef * turnVal) * cosT + (x1 * r - 2 * y1 * tmpCoef * turnval) * sinT;
		theta1New := theta1 - (f / F1);

		d := SubtractAngles (theta1New, theta1);
		theta1 := theta1New;
		if d < DegToRad (0.0001) then break;
	end;

	dTheta := Modulus ((theta1 - theta0) * turnVal, 2*Pi);
	r := turnRadius + RadToDeg (dTheta) * coefficient;
	outPoint := PointAlongPlane (spiralCentrePoint, theta1, r);

	d := ReturnAngleInRadians (outPoint, endLine.RefPoint);
	centreTheta := SpiralTouchAngle (turnRadius, coefficient, startLine.DirVector.Direction, d, turnSide);
	centreTheta := Modulus (theta0 + centreTheta * turnVal, 2*Pi);
	d2 := SubtractAngles (centreTheta, theta1);
	if d2 < 0.0001 then		result := d;
end;

function SortArray (items: Array of Double): IntegerArray;
var
	i, j:			Integer;
	count:          Integer;
	minVal:			Double;
	resultIndexList:	Array of Integer;
	searchIndexList:	TList;
	minIndex, curIndex:	Integer;
begin
	result := nil;

	if Length (items) = 0 then
		exit;
	count := Length (items);
	SetLength (resultIndexList, count);

	if count  = 1 then
	begin
		resultIndexList[0] := 0;
		exit;
	end;

	searchIndexList := TList.Create();
	searchIndexList.Capacity := count;

	for i := 0 to count-1 do
		searchIndexList.Add(Pointer(i));

	for i := 0 to count-1 do
	begin
		minval := items[Integer(searchIndexList[0])];
		minIndex := Integer(searchIndexList[0]);
		curIndex := 0;
		for j := 0 to searchIndexList.Count-1 do
		begin
			if (items[Integer(searchIndexList[j])]<minVal) then
			begin
				minval := items[Integer(searchIndexList[j])];
				minIndex := Integer(searchIndexList[j]);
				curIndex := j;
			end;
		end;
		searchIndexList.Delete(curIndex);
		resultIndexList[i] := minIndex;
	end;

	result := IntegerArray(resultIndexList);
end;

function SortPoints (basePoint: TPoint; multiPoint: TMultiPoint): TMultiPoint;
var
    i:              Integer;
	distanceList:   Array of Double;
	indexList:      IntegerArray;
begin
	result := nil;

	if multiPoint.Count = 0 then
		exit;

	result := TMultiPoint.Create;

	if multiPoint.Count < 1 then
	begin
//		result.AddPoint (multiPoint.Point [0]);		//Invalid
		exit;
	end;

	SetLength (distanceList, multiPoint.Count);

	for i := 0 to multiPoint.Count - 1 do
		distanceList [i] := ReturnDistanceInMeters (basePoint, multiPoint.Point [i]);

	indexList := SortArray (distanceList);

	if Assigned(indexList)then
		for i := 0 to multiPoint.Count - 1 do
			result.AddPoint (multiPoint.Point [indexList[i]]);
end;

function GeoToPrj (geoGeometry: TGeometry): TGeometry;
begin
	result := GeoOper.geoTransformations (geoGeometry, GGeoSR, GPrjSR);
end;

function PrjToGeo (prjGeometry: TGeometry): TGeometry;
begin
	result := GeoOper.geoTransformations (prjGeometry, GPrjSR, GGeoSR);
end;

procedure FreeObject (var vObject: TObject);
begin
  if (vObject <> nil) then
   begin
	  vObject.Free;
    vObject := nil;
  end;
end;

function IfThen (AValue: Boolean; const ATrue: String; const AFalse: String): String;
begin
  if AValue then
	Result := ATrue
  else
	Result := AFalse;
end;

function ShowHelp (callerHWND: LongWord; helpContextId: Integer): Integer;
begin
	result := HtmlHelp (callerHWND, PChar (Application.HelpFile), $F, helpContextId);
end;

initialization
	_geoOper := TGeometryOperators.Create;

finalization
	FreeObject (TObject (_geoOper));

end.

