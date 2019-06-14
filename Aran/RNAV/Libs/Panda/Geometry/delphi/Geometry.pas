unit Geometry;

interface

uses
  SysUtils, Classes, Contract;
//------------------------------------------------------------------------------
type
	TGeometryType =
	(
		gtNull,			// 0
		gtPoint,		// 1
		gtMultiPoint,	// 2
		gtPart,			// 3
		gtRing,			// 4
		gtPolyline,		// 5
		gtPolygon,		// 6

		gtVector,			// 7
		gtGeometrycLine,	// 8
		gtGeometrycPlane	// 9
	);
//------------------------------------------------------------------------------
	TPoint = class;
	TMultiPoint = class;
	TPart = class;
	TRing = class;
	TPolyline = class;
	TPolygon = class;

	TVector = class;
	TLine = class;
	TPlane = class;

	EVectorException = class (Exception)
	end;

	TGeometry = class(TPersistent)
	protected
		function GetGeometryType: TGeometryType; virtual; abstract;
	public
//		procedure Assign (const geometry: TGeometry); virtual; abstract;
//		procedure AssignTo (geometry: TGeometry); overload; virtual; abstract;
		function Clone: TGeometry; virtual; abstract;
		procedure Pack (handle: TRegistryHandle); virtual; abstract;
		procedure Unpack (handle: TRegistryHandle); virtual; abstract;

		function AsPoint: TPoint;
		function AsMultiPoint: TMultiPoint;
		function AsPart: TPart;
		function AsRing: TRing;
		function AsPolyline: TPolyline;
		function AsPolygon: TPolygon;

		function AsVector:	TVector;
		function AsLine: TLine;
		function AsPlane: TPlane;

		property GeometryType: TGeometryType read GetGeometryType;
	end;

	TPGeometry = ^TGeometry;
//------------------------------------------------------------------------------
	TNullGeometry = class (TGeometry)
	protected
		function GetGeometryType: TGeometryType;	override;
	public
//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent);	override;
		function Clone: TGeometry; override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;
	end;
//------------------------------------------------------------------------------
	TPoint = class (TGeometry)
	private
		FX: double;
		FY: double;
		FZ: double;
		FT: double;
		FM: double;
	protected
		function GetGeometryType: TGeometryType; override;
	public
		constructor Create;									overload;
		constructor Create (InitX, InitY: double);			overload;
		constructor Create (InitX, InitY, InitZ: double);	overload;
		constructor Create (const point: TPoint);			overload;

		procedure SetCoords (NewX, NewY: double);			overload;
		procedure SetCoords (NewX, NewY, NewZ: double);		overload;

		procedure SetEmpty;
		function IsEmpty: boolean;

//		procedure Assign (const geometry: TGeometry);		override;
		procedure AssignTo (geometry: TPersistent);			override;
		function Clone: TGeometry;							override;
		procedure Pack (handle: TRegistryHandle);			override;
		procedure Unpack (handle: TRegistryHandle);			override;

		property X: Double read FX write FX;
		property Y: Double read FY write FY;
		property Z: Double read FZ write FZ;
		property T: Double read FT write FT;
		property M: Double read FM write FM;
	end;

	TVector = class (TGeometry)
	private
		FNComponents:	Integer;
		FComponents:	array of Double;

		FDirangle:		Double;
		FZenith:		Double;
		FLength:		Double;
	protected
		function GetGeometryType: TGeometryType; override;

		function CalcDirection: Double;
		procedure SetDirection(Value: Double);

		function CalcZenith: Double;
		procedure SetZenith(Value: Double);

		function CalcLength: Double;
		procedure SetLength(Value: Double);

		function GetComponent(I: Integer): Double;
		procedure SetComponent(I: Integer; Value: Double);
	public
		constructor Create;								overload;
		constructor Create (N: Integer);				overload;
		constructor Create (const InitialValues: Array of double); overload;
		constructor Create (const point: TPoint);		overload;
		constructor Create (const Vector: TVector);		overload;
		constructor Create (const DirAngle: Double);	overload;
		destructor Destroy; override;

//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent);		override;
		function Clone: TGeometry; override;
		procedure Pack (handle: TRegistryHandle);		override;
		procedure Unpack (handle: TRegistryHandle);		override;

		property ComponentsCount: Integer read FNComponents;
		property Component[I: Integer]: Double read GetComponent write SetComponent;
		property Length: Double read FLength write SetLength;
		property Direction: Double read FDirangle write SetDirection;
		property Zenith: Double read FZenith write SetZenith;
	end;
//------------------------------------------------------------------------------
	TMultiPoint = class (TGeometry)
	private
		_pointList: TList;
	protected
		function GetGeometryType: TGeometryType; override;
		function GetPoint (Index: integer): TPoint;
		function GetCount: Integer;
	public
		constructor Create;
		destructor Destroy;							override;
		function ReplacePoint(Index: integer; Val: TPoint): Boolean;
		procedure AddPoint (const point: TPoint);
		procedure AddMultiPoint (const multiPoint: TMultiPoint);
		procedure AddMultiPointInverse(const multiPoint: TMultiPoint);

		procedure InsertPoint (Index: integer; const point: TPoint);
		procedure RemoveAt (Index: integer);
		procedure Clear;

//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent);	override;
		function Clone: TGeometry; override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;
		function Length: Double;

		property Count: Integer read GetCount;
		property Point [I: Integer]: TPoint read GetPoint;
	end;
  //----------------------------------------------------------------------------
	TPart = class (TMultiPoint)
	public
		function GetGeometryType: TGeometryType;	override;
		function Clone: TGeometry;					override;
	end;
//------------------------------------------------------------------------------
	TRing = class (TMultiPoint)
	public
		function GetGeometryType: TGeometryType; override;
		function Clone: TGeometry; override;
	end;
//------------------------------------------------------------------------------
	TPoly = class (TGeometry)
	private
		_multiPointList:	TList;
	protected
		function GetPrototype: TMultiPoint; virtual; abstract;
		function GetCount: integer;
		function GetMultiPoint (index: integer): TMultiPoint;
	public
		constructor Create;
		destructor Destroy; override;

//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent); override;
		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;

		procedure RemoveAt (index: integer);
		procedure Clear;

		procedure AddMultiPoint (const multiPoint: TMultiPoint);
		procedure InsertMultiPoint (index: integer; const multiPoint: TMultiPoint);
		function Length: Double;

		property Count: integer read GetCount;
		property MultiPoint[I: Integer]: TMultiPoint read GetMultiPoint;
	end;
//------------------------------------------------------------------------------
	TPolyline = class (TPoly)
	protected
		function GetPrototype: TMultiPoint; override;
		function GetPart (index: integer): TPart;
	public
		function Clone: TGeometry; override;
		function GetGeometryType: TGeometryType; override;

		procedure AddPart (const part: TPart);
		procedure InsertPart (index: integer; const part: TPart);

		property Part[I: Integer]: TPart read GetPart;
	end;
//------------------------------------------------------------------------------
	TPolygon = class (TPoly)
	protected
		function GetPrototype: TMultiPoint; override;
		function GetRing (index: integer): TRing;
	public
		function Clone: TGeometry; override;
		function GetGeometryType: TGeometryType; override;

		procedure AddRing (const ring: TRing);
		procedure InsertRing (index: integer; const ring: TRing);

		property Ring[I: Integer]: TRing read GetRing;
	end;
//------------------------------------------------------------------------------
	TLine = class (TGeometry)
	private
		FRefPoint:	TPoint;
		FVector:	TVector;
	protected
		function GetGeometryType: TGeometryType; override;
		procedure SetRefPoint(Val: TPoint);
		procedure SetDirVector(Val: TVector);
	public
		constructor Create;											overload;
		constructor Create(refPoint: TPoint; refVector: TVector);	overload;
		constructor Create(refPoint: TPoint; Direction: Double);	overload;
		constructor Create(FromPoint, ToPoint: TPoint);				overload;

		destructor Destroy; 					override;

//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent); override;
		function Clone: TGeometry; override;
		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;

		property RefPoint: TPoint read FRefPoint write SetRefPoint;
		property DirVector: TVector read FVector write SetDirVector;
	end;

	TPlane = class (TGeometry)
	private
		FRefPoint:	TPoint;
		FVector:	TVector;
	protected
		function GetGeometryType: TGeometryType; override;
		procedure setRefPoint(Val: TPoint);
		procedure setNormVector(Val: TVector);
	public
		constructor Create;											overload;
		constructor Create(refPoint: TPoint; refVector: TVector);	overload;
		destructor Destroy; override;

//		procedure Assign (const geometry: TGeometry); override;
		procedure AssignTo (geometry: TPersistent); override;
		function Clone: TGeometry; override;
		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;

		property RefPoint: TPoint read FRefPoint write SetRefPoint;
		property NormVector: TVector read FVector write SetNormVector;
	end;
//------------------------------------------------------------------------------

procedure PackGeometry (handle: TRegistryHandle; const geom: TGeometry);
function UnpackGeometry (handle: TRegistryHandle): TGeometry;

implementation

uses Math;

procedure PackGeometry (handle: TRegistryHandle; const geom: TGeometry);
begin
	Contract.putInt32 (handle, Integer(geom.geometryType));
	geom.pack(handle);
end;

function UnpackGeometry (handle: TRegistryHandle): TGeometry;
var
	geomType:	integer;
begin
	geomType := Contract.getInt32 (handle);
	result := nil;

	case TGeometryType(geomType) of
	gtNull:
		result := TNullGeometry.Create;
	gtPoint:
		result := TPoint.Create;
	gtMultiPoint:
		result := TMultiPoint.Create;
	gtPart:
		result := TPart.Create;
	gtRing:
		result := TRing.Create;
	gtPolyline:
		result := TPolyline.Create;
	gtPolygon:
		result := TPolygon.Create;
	gtVector:
		result := TVector.Create;
	gtGeometrycLine:
		result := TLine.Create;
	gtGeometrycPlane:
		result := TPlane.Create;
	else
		Contract.ThrowException (rcInvalid);
	end;

	try
		result.unpack(handle);
	except
		result.Free;
		raise;
	end;
end;
//------------------------------------------------------------------------------
function TNullGeometry.GetGeometryType: TGeometryType;
begin
	result := gtNull;
end;
//------------------------------------------------------------------------------
{procedure TNullGeometry.Assign (const geometry: TGeometry);
begin
end;
}
//------------------------------------------------------------------------------
procedure TNullGeometry.AssignTo (geometry: TPersistent);
begin
end;
//------------------------------------------------------------------------------
function TNullGeometry.Clone: TGeometry;
begin
	result := TNullGeometry.Create;
end;
//------------------------------------------------------------------------------
procedure TNullGeometry.Pack (handle: TRegistryHandle);
begin
end;
//------------------------------------------------------------------------------
procedure TNullGeometry.Unpack (handle: TRegistryHandle);
begin
end;
//------------------------------------------------------------------------------
function TPart.GetGeometryType: TGeometryType;
begin
	result := gtPart;
end;
//------------------------------------------------------------------------------
function TPart.Clone: TGeometry;
begin
	result := TPart.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
function TRing.GetGeometryType: TGeometryType;
begin
	result := gtRing;
end;
//------------------------------------------------------------------------------
function TRing.Clone: TGeometry;
begin
	result := TRing.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
procedure TPolygon.AddRing (const ring: TRing);
begin
	AddMultiPoint (ring);
end;
//------------------------------------------------------------------------------
procedure TPolygon.InsertRing (index: integer; const ring: TRing);
begin
	InsertMultiPoint (index, ring);
end;
//------------------------------------------------------------------------------
function TPolygon.Clone: TGeometry;
begin
	result := TPolygon.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
function TPolygon.GetPrototype: TMultiPoint;
begin
	result := TRing.Create;
end;
//------------------------------------------------------------------------------
function TPolygon.GetGeometryType: TGeometryType;
begin
	result := gtPolygon;
end;
//------------------------------------------------------------------------------
function TPolygon.GetRing (index: integer): TRing;
begin
	result := TRing (GetMultiPoint (index));
end;
//------------------------------------------------------------------------------
function TPolyline.Clone: TGeometry;
begin
	result := TPolyline.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
function TPolyline.GetPrototype: TMultiPoint;
begin
	result := TPart.Create;
end;
//------------------------------------------------------------------------------
function TPolyline.GetGeometryType: TGeometryType;
begin
	result := gtPolyline;
end;
//------------------------------------------------------------------------------
procedure TPolyline.AddPart (const part: TPart);
begin
	AddMultiPoint (part);
end;
//------------------------------------------------------------------------------
procedure TPolyline.InsertPart (index: integer; const part: TPart);
begin
	InsertMultiPoint (index, part);
end;
//------------------------------------------------------------------------------
function TPolyline.GetPart (index: integer): TPart;
begin
	result := TPart (GetMultiPoint (index));
end;
//------------------------------------------------------------------------------
constructor TPoly.Create;
begin
	inherited;
	_multiPointList := TList.create;
end;
//------------------------------------------------------------------------------
{
procedure TPoly.Assign (const geometry: TGeometry);
var
	i:		integer;
	src:	TPoly;
begin
	src := TPoly (geometry);
	Clear;
	for i:=0 to src.Count-1 do
		AddMultiPoint (src.GetMultiPoint (i));
end;
}
//------------------------------------------------------------------------------
procedure TPoly.AssignTo (geometry: TPersistent);
var
	i:		integer;
	Dst:	TPoly;
begin
	Dst := TPoly (geometry);
	Dst.Clear;
	for i := 0 to Count-1 do
		Dst.AddMultiPoint (GetMultiPoint (i));
end;
//------------------------------------------------------------------------------
procedure TPoly.Pack (handle: TRegistryHandle);
var
	i:	integer;
begin
	Contract.putInt32 (handle, Count);
	for i := 0 to Count-1 do
		GetMultiPoint (i).Pack (handle);
end;
//------------------------------------------------------------------------------
procedure TPoly.Unpack (handle: TRegistryHandle);
var
	i:			integer;
	size:		integer;
	MultiPoint:	TMultiPoint;
begin
	size := Contract.getInt32 (handle);
	Clear;

	if size = 0 then exit;

//	MultiPoint := TMultiPoint.Create;	// if TPart and TRing is same as TMultiPoint
	MultiPoint := GetPrototype;			// in general case

	try
		for i := 0 to size-1 do
		begin
			MultiPoint.Unpack (handle);
			AddMultiPoint (MultiPoint);
		end;
		MultiPoint.Free;
	except
		MultiPoint.Free;
		raise
	end;
end;
//------------------------------------------------------------------------------
destructor TPoly.Destroy;
begin
	Clear;
	_multiPointList.Free;
	inherited;
end;
//------------------------------------------------------------------------------
procedure TPoly.RemoveAt (index: integer);
begin
	GetMultiPoint(index).Free;
	_multiPointList.Delete (index);
end;
//------------------------------------------------------------------------------
procedure TPoly.Clear;
var
	i:	integer;
begin
	for i := 0 to _multiPointList.Count-1 do
		GetMultiPoint (i).Free;

	_multiPointList.Clear;
end;
//------------------------------------------------------------------------------
procedure TPoly.AddMultiPoint (const multiPoint: TMultiPoint);
begin
	_multiPointList.Add (multiPoint.Clone);
end;
//------------------------------------------------------------------------------
procedure TPoly.InsertMultiPoint (index: integer; const multiPoint: TMultiPoint);
begin
	if (index >= 0) and (index < _multiPointList.Count) then
	begin
		_multiPointList.Insert (index, multiPoint.Clone);
	end;
end;
//------------------------------------------------------------------------------
function TPoly.GetCount: integer;
begin
	result := _multiPointList.Count;
end;

function TPoly.Length: Double;
var
	n, i:		integer;
	multiPoint:	TMultiPoint;
begin
	result := 0.0;
	n := _multiPointList.Count - 1;
	for i := 0 to n do
	begin
		multiPoint := TMultiPoint(_multiPointList.Items[i]);
		result := result + multiPoint.Length;
	end;
end;

//------------------------------------------------------------------------------
function TPoly.GetMultiPoint (index: integer): TMultiPoint;
begin
	if (index >= 0) and (index < _multiPointList.Count) then
		result := TMultiPoint (_multiPointList.Items [index])
	else
		result := nil;
end;
//------------------------------------------------------------------------------
constructor TMultiPoint.Create;
begin
	inherited;
	_pointList := TList.Create;
end;
//------------------------------------------------------------------------------
destructor TMultiPoint.Destroy;
begin
	Clear;
	_pointList.Free;
	inherited;
end;
//------------------------------------------------------------------------------
procedure TMultiPoint.AddPoint (const point: TPoint);
begin
	_pointList.Add (point.Clone);
end;
//------------------------------------------------------------------------------

procedure TMultiPoint.AddMultiPoint(const multiPoint: TMultiPoint);
var
	i:	Integer;
begin
	for i:=0 to multiPoint.Count - 1 do
		AddPoint (multiPoint.Point [i]);
end;

procedure TMultiPoint.AddMultiPointInverse(const multiPoint: TMultiPoint);
var
	i:	Integer;
begin
	for i:=multiPoint.Count - 1 downto 0 do
		AddPoint (multiPoint.Point [i]);
end;

//------------------------------------------------------------------------------
function TMultiPoint.ReplacePoint(Index: integer; Val: TPoint): Boolean;
begin
	Result := (Index >= 0) and (Index < _pointList.Count);
	if Result then
		TPoint(_pointList.Items[Index]).Assign(Val);
end;

procedure TMultiPoint.InsertPoint (index: integer; const Point: TPoint);
begin
	if (index >= 0) and (index < _pointList.Count) then
		_pointList.Insert (index, point.Clone);
end;
//------------------------------------------------------------------------------
function TMultiPoint.GetPoint (index: integer): TPoint;
begin
	if (index >= 0) and (index < _pointList.Count) then
		result := TPoint (_pointList.Items [index])
	else
		result := nil;
end;
//------------------------------------------------------------------------------
procedure TMultiPoint.RemoveAt (index: integer);
begin
	GetPoint(index).Free;
	_pointList.Delete (index);
end;
//------------------------------------------------------------------------------
procedure TMultiPoint.Clear;
var
	i:	integer;
begin
	for i := 0 to _pointList.Count-1 do
		GetPoint (i).Free;
	_pointList.Clear;
end;
//------------------------------------------------------------------------------
{procedure TMultiPoint.Assign (const geometry: TGeometry);
var
	i:		integer;
	src:	TMultiPoint;
begin
	src := TMultiPoint (geometry);
	Clear;
	for i:=0 to src.Count-1 do
		AddPoint (src.GetPoint (i));
end;}
//------------------------------------------------------------------------------
procedure TMultiPoint.AssignTo (geometry: TPersistent);
var
	i:		integer;
	Dst:	TMultiPoint;
begin
	Dst := TMultiPoint (geometry);
	Dst.Clear;
	for i := 0 to Count-1 do
		Dst.AddPoint (GetPoint (i));
end;
//------------------------------------------------------------------------------
function TMultiPoint.GetCount: integer;
begin
	result := _pointList.Count;
end;
//------------------------------------------------------------------------------
function TMultiPoint.Clone: TGeometry;
begin
	result := TMultiPoint.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
function TMultiPoint.GetGeometryType: TGeometryType;
begin
	result := gtMultiPoint;
end;
//------------------------------------------------------------------------------
function TMultiPoint.Length: Double;
var
	i, n:	Integer;
	pt0, pt1:	TPoint;
begin
	result := 0;
	n := _pointList.Count - 2;

	for i := 0 to n do
	begin
		pt0 := TPoint(_pointList.Items[i]);
		pt1 := TPoint(_pointList.Items[i + 1]);
		result := result + Hypot(pt1.X - pt0.X, pt1.Y - pt0.Y);
	end;
end;

//------------------------------------------------------------------------------
procedure TMultiPoint.Pack (handle: TRegistryHandle);
var
	i:	integer;
begin
	Contract.putInt32 (handle, Count);
	for i := 0 to Count-1 do
		GetPoint (i).pack (handle);
end;
//------------------------------------------------------------------------------
procedure TMultiPoint.Unpack (handle: TRegistryHandle);
var
	size:	integer;
	i:		integer;
	point:	TPoint;
begin
	size := Contract.getInt32 (handle);

	Clear;
	point := TPoint.Create;

	try
		for i := 0 to size - 1 do
		begin
			point.Unpack (handle);
			AddPoint (point);
		end;
		point.Free;
	except
		point.Free;
		raise;
	end;
end;
//-----------------------  TPoint ----------------------------------------------
constructor TPoint.Create;
begin
	inherited;
	SetEmpty;
end;
//------------------------------------------------------------------------------
constructor TPoint.Create (InitX, InitY: double);
begin
	inherited Create;
	SetEmpty;
	FX := InitX;
	FY := InitY;
end;

constructor TPoint.Create (InitX, InitY, InitZ: double);
begin
	inherited Create;
	SetEmpty;
	FX := InitX;
	FY := InitY;
	FZ := InitZ;
end;

constructor TPoint.Create (const point: TPoint);
begin
	inherited Create;
	Assign (point);
end;
//------------------------------------------------------------------------------
procedure TPoint.SetCoords (NewX, NewY: double);
begin
	FX := NewX;
	FY := NewY;
end;

procedure TPoint.SetCoords (NewX, NewY, NewZ: double);
begin
	FX := NewX;
	FY := NewY;
	FZ := NewZ;
end;

//------------------------------------------------------------------------------
procedure TPoint.SetEmpty;
begin
	FX := NaN;
	FY := NaN;
	FZ := 0.0;
	FM := 0.0;
	FT := 0.0;
end;

//------------------------------------------------------------------------------
{procedure TPoint.Assign (const geometry: TGeometry);
var
	point: TPoint;
begin
	point := TPoint (geometry);
	FX := point.FX;
	FY := point.FY;
	FZ := point.FZ;
	FM := point.FM;
	FT := point.FT;
end;
}
procedure TPoint.AssignTo (geometry: TPersistent);
var
	point: TPoint;
begin
	point := TPoint (geometry);
	point.FX := FX;
	point.FY := FY;
	point.FZ := FZ;
	point.FM := FM;
	point.FT := FT;
end;

//------------------------------------------------------------------------------
function TPoint.GetGeometryType: TGeometryType;
begin
	result := gtPoint;
end;

//------------------------------------------------------------------------------
procedure TPoint.Pack (handle: TRegistryHandle);
begin
	Contract.putDouble (handle, FX);
	Contract.putDouble (handle, FY);
	Contract.putDouble (handle, FZ);
	Contract.putDouble (handle, FM);
	Contract.putDouble (handle, FT);
end;

//------------------------------------------------------------------------------
procedure TPoint.Unpack (handle: TRegistryHandle);
begin
	FX := Contract.getDouble (handle);
	FY := Contract.getDouble (handle);
	FZ := Contract.getDouble (handle);
	FM := Contract.getDouble (handle);
	FT := Contract.getDouble (handle);
end;
//------------------------------------------------------------------------------
function TPoint.IsEmpty: boolean;
begin
	result := IsNan(FX) or IsNan(FY);
end;
//------------------------------------------------------------------------------
function TPoint.Clone: TGeometry;
begin
	result := TPoint.Create (self);
	result.Assign (self);
end;

//----------------------  TGeometry --------------------------------------------
function TGeometry.asPoint: TPoint;
begin
	result := TPoint (self);
end;
//------------------------------------------------------------------------------
function TGeometry.asMultiPoint: TMultiPoint;
begin
	result := TMultiPoint (self);
end;
//------------------------------------------------------------------------------
function TGeometry.asPart: TPart;
begin
	result := TPart (self);
end;
//------------------------------------------------------------------------------
function TGeometry.asRing: TRing;
begin
	result := TRing (self);
end;
//------------------------------------------------------------------------------
function TGeometry.asPolyline: TPolyline;
begin
	result := TPolyline (self);
end;
//------------------------------------------------------------------------------
function TGeometry.asPolygon: TPolygon;
begin
	result := TPolygon (self);
end;
//------------------------------------------------------------------------------
function TGeometry.AsVector:	TVector;
begin
	result := TVector (self);
end;

function TGeometry.AsLine: TLine;
begin
	result := TLine (self);
end;

function TGeometry.AsPlane: TPlane;
begin
	result := TPlane (self);
end;

//----------------------------  TVector  ---------------------------------------

constructor TVector.Create;
begin
	inherited;
	FNComponents := 0;
	FComponents := nil;
end;

constructor TVector.Create (N: Integer);
begin
	inherited Create;
	FNComponents := 5;
	System.SetLength(FComponents, FNComponents);
	FDirangle := NAN;
end;

constructor TVector.Create (const InitialValues: Array of double);
var
	I:	Integer;
begin
	inherited Create;
	FNComponents := System.Length(InitialValues);
	System.SetLength(FComponents, FNComponents);

	for I := 0 to FNComponents-1 do
		FComponents[I] := InitialValues[I];

	FDirangle := CalcDirection;
	FZenith := CalcZenith;
	FLength := Norm(FComponents);
end;

constructor TVector.Create (const point: TPoint);
begin
	Create ([point.X, point.Y, point.Z, point.M, point.T]);
end;

constructor TVector.Create (const Vector: TVector);
begin
	Create (Vector.FComponents);
//	Create (Vector);
end;

constructor TVector.Create (const DirAngle: Double);
var
	SinA, CosA:		Extended;
begin
	SinCos(DirAngle, SinA, CosA);
	Create ([CosA, SinA]);
end;

destructor TVector.Destroy;
begin
	FComponents := nil;
	inherited
end;

function TVector.GetGeometryType: TGeometryType;
begin
	result := gtVector;
end;
{
procedure TVector.Assign (const geometry: TGeometry);
var
	I:	Integer;
begin
	FNComponents := TVector(geometry).FNComponents;
	System.SetLength(FComponents, FNComponents);

	for I := 0 to FNComponents-1 do
		FComponents[I] := TVector(geometry).FComponents[I];

	FDirangle := TVector(geometry).FDirangle;
end;
}
procedure TVector.AssignTo (geometry: TPersistent);
var
	I:		Integer;
	Dst:	TVector;
begin
	Dst := TVector(geometry);
	Dst.FNComponents := FNComponents;
	System.SetLength(Dst.FComponents, FNComponents);

	for I := 0 to FNComponents - 1 do
		Dst.FComponents[I] := FComponents[I];

	Dst.FDirangle := FDirangle;
end;

function TVector.Clone: TGeometry;
begin
	result := TVector.Create (FComponents);
//	result := TVector.Create (self);
end;

procedure TVector.Pack (handle: TRegistryHandle);
var
	I:	Integer;
begin
	Contract.putInt32 (handle, FNComponents);
	for i := 0 to FNComponents-1 do
		Contract.PutDouble(handle, FComponents[I]);
	Contract.PutDouble(handle, FDirangle);
end;

procedure TVector.Unpack (handle: TRegistryHandle);
var
	I:	Integer;
begin
	FNComponents := Contract.GetInt32 (handle);
	for i := 0 to FNComponents-1 do
		FComponents[I] := Contract.GetDouble(handle);
	FDirangle := Contract.GetDouble(handle);
end;

function TVector.CalcDirection: Double;
begin
	if FNComponents > 1 then
		result := ArcTan2(FComponents[1], FComponents[0])
	else
		result := NAN;
end;

procedure TVector.SetDirection(Value: Double);
var
	Len, SinA, CosA:		Extended;
begin
	FDirangle := Value;
	if (FNComponents > 1) and not IsNan(FDirangle) then
	begin
		Len := Hypot(FComponents[0], FComponents[1]);
		SinCos(Value, SinA, CosA);
		FComponents[0] := Len*CosA;
		FComponents[1] := Len*SinA;
	end;
end;

function TVector.CalcZenith: Double;
begin
	if FNComponents > 2 then
		result := ArcCos(FComponents[2]/Norm([FComponents[0], FComponents[1], FComponents[2]]))
	else
		result := NAN;
end;

procedure TVector.SetZenith(Value: Double);
var
	V1, Len:		Extended;
	SinA, CosA:		Extended;
begin
	FZenith := Value;
	if (FNComponents > 2) and not IsNan(FZenith) then
	begin
		V1 := Hypot(FComponents[0], FComponents[1]);
		Len := Hypot(V1, FComponents[2]);
//		Len := Norm([FComponents[0], FComponents[1], FComponents[2]]);
		SinCos(Value, SinA, CosA);
//		V1 := Len*SinA;

		FComponents[0] := SinA*FComponents[0];
		FComponents[1] := SinA*FComponents[1];
		FComponents[2] := Len*CosA;
	end;
end;

function TVector.CalcLength: Double;
begin
	result := Norm(FComponents);
end;

procedure TVector.SetLength(Value: Double);
var
	I:	Integer;
	K:	Extended;
begin
	K := Value / FLength;
	FLength := Value;

	for I := 0 to FNComponents-1 do
		FComponents[I] := FComponents[I] * K;
end;

function TVector.GetComponent(I: Integer): Double;
begin
	if I >= FNComponents then
		raise EVectorException.Create('Invalid index.');
	Result := FComponents[I];
end;

procedure TVector.SetComponent(I: Integer; Value: Double);
begin
	if I >= FNComponents then
		raise EVectorException.Create('Invalid index.');
	FComponents[I] := Value;

	FDirangle := CalcDirection;
	FZenith := CalcZenith;
	FLength := Norm(FComponents);
end;

//---------------------  TGeometrycLine  ---------------------------------------
constructor TLine.Create;
begin
	Inherited;
	FRefPoint := TPoint.Create;
	FVector := TVector.Create;
end;

constructor TLine.Create(refPoint: TPoint; refVector: TVector);
begin
	Create;
	FRefPoint.Assign(refPoint);
	FVector.Assign(refVector);
end;

constructor TLine.Create(refPoint: TPoint; Direction: Double);
begin
	Create;
	FRefPoint.Assign(refPoint);
	FVector := TVector.Create(Direction);
end;

constructor TLine.Create(FromPoint, ToPoint: TPoint);
begin
	Inherited Create;
	FRefPoint := TPoint.Create(FromPoint);
	FVector := TVector.Create([ToPoint.X - FromPoint.X, ToPoint.Y - FromPoint.Y]);
end;

destructor TLine.Destroy;
begin
	FRefPoint.Free;
	FVector.Free;
	Inherited;
end;

procedure TLine.SetRefPoint(Val: TPoint);
begin
	FRefPoint.Assign (Val);
end;

procedure TLine.SetDirVector(Val: TVector);
begin
	FVector.Assign (Val);
end;

function TLine.GetGeometryType: TGeometryType;
begin
	result := gtGeometrycLine
end;
{
procedure TLine.Assign (const geometry: TGeometry);
begin
	inherited;
	FRefPoint.Assign (TLine (geometry).FRefPoint);
	FVector.Assign (TLine (geometry).FVector);
end;
}
procedure TLine.AssignTo (geometry: TPersistent);
Var
	Dst:	TLine;
begin
	Dst := TLine (geometry);
	Dst.FRefPoint.Assign (FRefPoint);
	Dst.FVector.Assign (FVector);
end;

function TLine.Clone: TGeometry;
begin
	result := TLine.Create(FRefPoint, FVector);
end;

procedure TLine.Pack (handle: TRegistryHandle);
begin
	Inherited;
	FRefPoint.Pack(handle);
	FVector.Pack(handle);
end;

procedure TLine.Unpack (handle: TRegistryHandle);
begin
	Inherited;
	FRefPoint.UnPack(handle);
	FVector.UnPack(handle);
end;

//---------------------  TGeometrycPlane  --------------------------------------
constructor TPlane.Create;
begin
	Inherited;
	FRefPoint := TPoint.Create;
	FVector := TVector.Create;
end;

constructor TPlane.Create(refPoint: TPoint; refVector: TVector);
begin
	Create;
	FRefPoint.Assign(refPoint);
	FVector.Assign(refVector);
end;

destructor TPlane.Destroy;
begin
	FRefPoint.Free;
	FVector.Free;
	Inherited;
end;

procedure TPlane.setRefPoint(Val: TPoint);
begin
	FRefPoint.Assign (Val);
end;

procedure TPlane.setNormVector(Val: TVector);
begin
	FVector.Assign (Val);
end;

function TPlane.GetGeometryType: TGeometryType;
begin
	result := gtGeometrycPlane
end;
{
procedure TPlane.Assign (const geometry: TGeometry);
begin
	inherited;
	FRefPoint.Assign(TPlane (geometry).FRefPoint);
	FVector.Assign(TPlane (geometry).FVector);
end;
}
procedure TPlane.AssignTo (geometry: TPersistent);
var
	Dst:	TPlane;
begin
	Dst := TPlane(geometry);
	Dst.FRefPoint.Assign(FRefPoint);
	Dst.FVector.Assign(FVector);
end;

function TPlane.Clone: TGeometry;
begin
	result := TPlane.Create(FRefPoint, FVector);
end;

procedure TPlane.Pack (handle: TRegistryHandle);
begin
	Inherited;
	FRefPoint.Pack(handle);
	FVector.Pack(handle);
end;

procedure TPlane.Unpack (handle: TRegistryHandle);
begin
	Inherited;
	FRefPoint.UnPack(handle);
	FVector.UnPack(handle);
end;

end.

