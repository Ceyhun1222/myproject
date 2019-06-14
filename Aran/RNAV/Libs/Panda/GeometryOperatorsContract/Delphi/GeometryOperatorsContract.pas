unit GeometryOperatorsContract;

interface

uses
	Contract,
	Registry,
	Geometry, Common;

type

	TSpatialReferenceGeoType = (
		srgtWGS1984 = 1,
		srgtKrasovsky1940,
		srgtNAD1983);

	TSpatialReferenceType = (
		srtGeographic = 1,
		srtMercator,
		srtTransverse_Mercator,
		srtGauss_Krueger);

	TSpatialReferenceParamType = (
		srptFalseEasting = 1,
		srptFalseNorthing,
		srptScaleFactor,
		srptAzimuth,
		srptCentralMeridian,
		srptLatitudeOfOrigin,
		srptLongitudeOfCenter);

	TSpatialReferenceUnit = (
		sruMeter = 1,
		sruFoot,
		sruNauticalMile,
		sruKilometer);

	TGeometryCommands =(
		gcUnion = 1,
		gcConvexHull,		// 2
		gcCut,				// 3
		gcIntersect,		// 4
		gcBoundary,			// 5
		gcBuffer,			// 6

		gcDifference,		// 7

		gcGetNearestPoint,	// 8
		gcGetDistance,		// 9

		gcContains,			// 10
		gcCrosses,			// 11
		gcDisjoint,			// 12
		gcEquals,			// 13
		gcGeoTransformations
	);

	TEllipsoid = class
	public
		constructor Create;

		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);

	public
		srGeoType: TSpatialReferenceGeoType;
		semiMajorAxis: Double;
		flattening: Double;
		isValid: boolean;
	end;

	TSpatialReferenceParam = class (TPandaItem)
	public
		constructor Create; overload;
		constructor Create (aSRParamType: TSpatialReferenceParamType; aValue: double); overload;

		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;

		procedure Assign (const src: TSpatialReferenceParam);
		function Clone: TSpatialReferenceParam;

	public
		srParamType:	TSpatialReferenceParamType;
		value: 			Double;

	protected
		function CloneItem: TPandaItem; override;
	end;

	TSpatialReference = class
	private
		_ellipsoid: TEllipsoid;
		_paramList: TPandaList;

        function GetParameter (param: TSpatialReferenceParamType): Double;
        procedure SetParameter (param: TSpatialReferenceParamType; val: Double);
	public
		constructor Create;
		destructor Destroy; override;

		procedure pack (handle: TRegistryHandle);
		procedure unpack (handle: TRegistryHandle);

	public
		Name:	WideString;
		spatialReferenceType: TSpatialReferenceType;
		spatialReferenceUnit: TSpatialReferenceUnit;

		property ellipsoid: TEllipsoid read _ellipsoid;
		property paramList: TPandaList read _paramList;
        property Param [param: TSpatialReferenceParamType]: Double read GetParameter write SetParameter;
	end;

	TGeometryOperators = class
	private
		_handle: TRegistryHandle;
	public
		constructor Create;
		destructor Destroy; override;
		function isValid: boolean;

		// #region Topological Operators
		function UnionGeometry(const geom1, geom2: TGeometry): TGeometry;
		function ConvexHull (const geom: TGeometry): TGeometry;
		procedure Cut (const Geometry: TGeometry; const cutter: TPolyline; var geomLeft, geomRight: TGeometry);
		function Intersect (const geom1, geom2: TGeometry): TGeometry;
		function Boundary (const geom: TGeometry): TGeometry;
		function Buffer(const geom: TGeometry; width: double): TGeometry;
		function Difference (const geom, other: TGeometry): TGeometry;
		// #endregion

		// #region Proximity Operators
		function GetNearestPoint (const geom: TGeometry; const point: TPoint): TPoint;
		function GetDistance (const geom, other: TGeometry): double;
		// #endregion

		// #region Relational Operators
		function Contains (const geom, other: TGeometry): boolean;
		function Crosses (const geom, other: TGeometry): boolean;
		function Disjoint (const geom, other: TGeometry): boolean;
		function Equals (const geom, other: TGeometry): boolean;
		// #endregion

		function GeoTransformations (const geom: TGeometry; const fromSR: TSpatialReference; const toSR: TSpatialReference): TGeometry;
	end;

implementation

constructor TEllipsoid.Create;
begin
	srGeoType := srgtWGS1984;
	semiMajorAxis := 0;
	flattening := 0;
	isValid := false;
end;

procedure TEllipsoid.Pack (handle: TRegistryHandle);
begin
	Contract.putInt32 (handle, Integer (srGeoType));
	Contract.putDouble (handle, semiMajorAxis);
	Contract.putDouble (handle, flattening);
	Contract.putBool (handle, isValid);
end;

procedure TEllipsoid.Unpack (handle: TRegistryHandle);
begin
	srGeoType := TSpatialReferenceGeoType (getInt32 (handle));
	semiMajorAxis := Contract.getDouble (handle);
	flattening := Contract.getDouble (handle);
	isValid := Contract.getBool (handle);
end;

constructor TSpatialReferenceParam.Create;
begin
	srParamType := TSpatialReferenceParamType(0);
	value := 0;
end;

constructor TSpatialReferenceParam.Create (aSRParamType: TSpatialReferenceParamType; aValue: Double);
begin
	srParamType := aSRParamType;
	value := aValue;
end;

procedure TSpatialReferenceParam.Pack (handle: TRegistryHandle);
begin
	Contract.putInt32 (handle, Integer (srParamType));
	Contract.putDouble (handle, value);
end;

procedure TSpatialReferenceParam.Unpack (handle: TRegistryHandle);
begin
	srParamType := TSpatialReferenceParamType(Contract.getInt32 (handle));
	value := Contract.getDouble(handle);
end;

procedure TSpatialReferenceParam.Assign (const src: TSpatialReferenceParam);
begin
	srParamType := src.srParamType;
	value := src.value;
end;

function TSpatialReferenceParam.Clone: TSpatialReferenceParam;
begin
	result := TSpatialReferenceParam.Create;
	result.Assign (self);
end;

function TSpatialReferenceParam.CloneItem: TPandaItem;
begin
  result := Clone;
end;

constructor TSpatialReference.Create;
begin
    _ellipsoid := TEllipsoid.Create;
    _paramList := TPandaList.Create (TSpatialReferenceParam.Create);
	spatialReferenceType := srtGeographic;
	spatialReferenceUnit := sruMeter;
end;

destructor TSpatialReference.Destroy;
begin
	_ellipsoid.Free;
	_paramList.Free;
end;

function TSpatialReference.GetParameter (param: TSpatialReferenceParamType): Double;
var
	i:	Integer;
begin
	result := 0;
	for i:=0 to paramList.Count - 1 do
    begin
    	if TSpatialReferenceParam (paramList.Item [i]).srParamType = param then
        begin
        	result := TSpatialReferenceParam (paramList.Item [i]).value;
        	exit;
        end;
    end;
end;

procedure TSpatialReference.SetParameter (param: TSpatialReferenceParamType; val: Double);
var
	i:	Integer;
begin
	for i:=0 to paramList.Count - 1 do
    begin
    	if TSpatialReferenceParam (paramList.Item [i]).srParamType = param then
        begin
        	TSpatialReferenceParam (paramList.Item [i]).value := val;
        	exit;
        end;
    end;
end;

procedure TSpatialReference.Pack (handle: TRegistryHandle);
begin
	Contract.PutWideString (handle, name);
	Contract.putInt32 (handle, Integer (spatialReferenceType));
	Contract.putInt32 (handle, Integer (spatialReferenceUnit));

	ellipsoid.pack (handle);
	paramList.pack (handle);
end;

procedure TSpatialReference.Unpack (handle: TRegistryHandle);
begin
	name := Contract.GetWideString( handle);
	spatialReferenceType := TSpatialReferenceType( Contract.getInt32 (handle));
	spatialReferenceUnit := TSpatialReferenceUnit( Contract.getInt32 (handle));
	ellipsoid.unpack(handle);
	paramList.unpack(handle);
end;
                                                //bu nece olacaq??
constructor TGeometryOperators.Create;
begin
	Inherited;
	try
		_handle := Contract.getInstance ('GeometryOperatorsService');
	except
		_handle := 0;
	end;
end;

destructor TGeometryOperators.Destroy;
begin
	try
		Contract.freeInstance(_handle);
	except
	end;
	Inherited;
end;

function TGeometryOperators.isValid: boolean;
begin
	result := _handle <> 0;
end;

// #region Topological Operators
function TGeometryOperators.UnionGeometry (const geom1, geom2: TGeometry): TGeometry;
begin
	Contract.beginMessage (_handle, Integer(gcUnion));
	packGeometry (_handle, geom1);
	packGeometry (_handle, geom2);
	Contract.endMessage (_handle);
	result := unpackGeometry (_handle);
end;

function TGeometryOperators.convexHull (const geom: TGeometry): TGeometry;
begin
	Contract.beginMessage (_handle, Integer(gcConvexHull));
	packGeometry (_handle, geom);
	Contract.endMessage (_handle);
	result := unpackGeometry (_handle);
end;

procedure TGeometryOperators.Cut (const geometry: TGeometry; const cutter: TPolyline; var geomLeft, geomRight: TGeometry);
begin
	Contract.beginMessage (_handle, Integer(gcCut));
	packGeometry (_handle, geometry);
	packGeometry (_handle, cutter);
	Contract.endMessage (_handle);

	try
		geomLeft := unpackGeometry(_handle);
		geomRight := unpackGeometry(_handle);
	except
		geomLeft.Free;
		geomRight.Free;
    raise;
	end;
end;

function TGeometryOperators.Intersect (const geom1, geom2: TGeometry): TGeometry;
begin
	Contract.BeginMessage (_handle, Integer(gcIntersect));
	packGeometry (_handle, geom1);
	packGeometry (_handle, geom2);
	Contract.EndMessage (_handle);
	result := unpackGeometry(_handle);
end;

function TGeometryOperators.boundary (const geom: TGeometry): TGeometry;
begin
	Contract.BeginMessage (_handle, Integer(gcBoundary));
	packGeometry (_handle, geom);
	Contract.EndMessage (_handle);
	result := unpackGeometry(_handle);
end;

function TGeometryOperators.Buffer (const geom: TGeometry; width: double): TGeometry;
begin
	Contract.BeginMessage (_handle, Integer(gcBuffer));
	packGeometry (_handle, geom);
	Contract.PutDouble (_handle, width);
	Contract.EndMessage (_handle);
  result := unpackGeometry (_handle);
end;

function TGeometryOperators.Difference (const geom, other: TGeometry): TGeometry;
begin
	Contract.BeginMessage (_handle, Integer(gcDifference));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := unpackGeometry(_handle);
end;
// #endregion

// #region Proximity Operators
function TGeometryOperators.getNearestPoint(const geom: TGeometry; const point: TPoint): TPoint;
var
	resultGeom:	TGeometry;
begin
	Contract.BeginMessage (_handle, Integer(gcGetNearestPoint));
	packGeometry (_handle, geom);
	packGeometry (_handle, point);
	Contract.EndMessage (_handle);

	resultGeom := unpackGeometry(_handle);
	result := resultGeom.AsPoint;
end;

function TGeometryOperators.getDistance (const geom, other: TGeometry): double;
begin
	Contract.BeginMessage (_handle, Integer(gcGetDistance));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := Contract.GetDouble(_handle);
end;
// #endregion

// #region Relational Operators
function TGeometryOperators.contains (const geom, other: TGeometry): boolean;
begin
	Contract.BeginMessage (_handle, Integer(gcContains));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := Contract.GetBool (_handle);
end;

function TGeometryOperators.crosses (const geom, other: TGeometry): boolean;
begin
	Contract.BeginMessage (_handle, Integer(gcCrosses));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := Contract.GetBool (_handle);
end;

function TGeometryOperators.disjoint (const geom, other: TGeometry): boolean;
begin
	Contract.BeginMessage (_handle, Integer(gcDisjoint));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := Contract.GetBool (_handle);
end;

function TGeometryOperators.equals (const geom, other: TGeometry): boolean;
begin
	Contract.BeginMessage (_handle, Integer(gcEquals));
	packGeometry (_handle, geom);
	packGeometry (_handle, other);
	Contract.EndMessage (_handle);
	result := Contract.GetBool(_handle);
end;

function TGeometryOperators.geoTransformations (const geom: TGeometry; const fromSR: TSpatialReference; const toSR: TSpatialReference): TGeometry;
begin
	Contract.beginMessage (_handle, Integer (gcGeoTransformations));
	packGeometry (_handle, geom);
	fromSR.pack (_handle);
	toSR.pack(_handle);
	Contract.endMessage (_handle);

	result := unpackGeometry (_handle);
end;

end.
 
