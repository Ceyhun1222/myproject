unit AIXMTypes;

interface

uses Contract, Geometry, Common;

type
	TAIXMType =
	(
		atNull,				//0
		atDME,				//1
		atVOR,				//2
		atObstacle,			//3
		atAHP,				//4
		atRWY,				//5
		atRwyDirection,		//6
		atNDB,				//7
		atDesignatedPoint,	//8
		atTACAN				//9
	);

const
	AIXMTypeNames: Array [TAIXMType] of string = ('Null', 'DME', 'VOR', 'Obstacle',
													'AHP', 'RWY', 'RWY Dir.',
													'NDB', 'Dsgn Pnt.', 'TACAN');

type

	TVORNorthType = (ntOther, ntTrueNorth, ntMagneticNorth, ntGrid);

	TDME = class;
	TVOR = class;
	TObstacle = class;
	TAHP = class;
	TRWY = class;
	TRWYDirection = class;         
	TNDB = class;
	TDesignatedPoint = class;
	TTACAN = class;
    TSignificantPoint = class;

//------------------------------------------------------------------------------

	TAIXM = class (TPandaItem)
	private
		FAixmType:	TAixmType;
		FID:		String;
		FAixmID:	WideString;
		FName:		Widestring;
		FRemark:	Widestring;
		FTag:		Longint;
	protected
		constructor Create (objType: TAIXMType);	overload;
		constructor Create;							overload; virtual;
		function CloneItem: TPandaItem;				override;

	public
		procedure Assign (const value: TAIXM);		virtual;
		function Clone: TAIXM;						virtual; abstract;

		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		procedure AIXMPack (handle: TRegistryHandle);
		class function  AIXMUnpack (handle: TRegistryHandle): TAIXM;

		function asDme: TDME;
		function asVor: TVOR;
		function asObstacle: TObstacle;
		function asAhp: TAHP;
		function asRwy: TRWY;
		function asRwyDirection: TRWYDirection;
		function asNDB: TNDB;
		function asDesignatedPoint: TDesignatedPoint;
		function asSignificantPoint: TSignificantPoint;
        function asTACAN: TTACAN;

		property AIXMType: TAIXMType read FAixmType;
		property Name: WideString read FName write FName;
		property ID: String read FID write FID;
		property AixmID: WideString read FAixmID write FAixmID;
		property Remark: WideString read FRemark write FRemark;
		property Tag: Longint read FTag write FTag default 0;
	end;

	TAIXMClass = Class of TAIXM;

//------------------------------------------------------------------------------

	TAIXMGeometry = class (TAIXM)
	protected
		FGeoGeometry:		TGeometry;
		FPrjGeometry:		TGeometry;
		constructor Create (objType: TAIXMType; geo: TGeometry; prj: TGeometry);

	public
		destructor Destroy; override;

		procedure Unpack (handle: TRegistryHandle);		override;
		procedure Pack (handle: TRegistryHandle);		override;

		procedure Assign (const Value: TAIXM);	override;

		property Geo: TGeometry read FGeoGeometry;
		property Prj: TGeometry read FPrjGeometry;
	end;

//------------------------------------------------------------------------------

	TAHP = class(TAIXM)
	private
		ptGeo:			TPoint;
		ptPrj:			TPoint;
	public
		Elevation:		double;
		ElevAccuracy:	double;
		MagVar:			double;
		Temperature:	double;
		CountryId:		String;

		property Geo: TPoint read ptGeo;
		property Prj: TPoint read ptPrj;
	public
		constructor Create;	override;
		destructor Destroy; override;

		function Clone: TAixm;	override;
		procedure Unpack (handle: TRegistryHandle);	override;
		procedure Pack (handle: TRegistryHandle);	override;

		procedure Assign (const Value: TAIXM); override;
	end;

//------------------------------------------------------------------------------

	TAHPList = class(TPANDAList)
	private
		procedure SetItem (index: integer; const _Item: TAHP);	//overload;
		function At (index: integer): TAHP;						//overload;
	public
		constructor Create;
		property Item[I: Integer]: TAHP read At write SetItem;
	end;

//------------------------------------------------------------------------------

	TRWYList = class (TPandaList)
	private
		procedure SetItem (index: integer; const _Item: TRWY);	//overload;
		function At (index: integer): TRWY;						//overload;
	public
		constructor Create;
		property Item[I: Integer]: TRWY read At write SetItem;
	end;

//------------------------------------------------------------------------------

	TRWYDirectionList = class (TPandaList)
	private
		procedure SetItem (index: integer; const _Item: TRWYDirection);	//overload;
		function At (index: integer): TRWYDirection;					//overload;
	public
		constructor Create;
		property Item[I: Integer]: TRWYDirection read At write SetItem;
	end;

//------------------------------------------------------------------------------

	TDMEList = class (TPandaList)
	private
		procedure SetItem (index: integer; const _Item: TDME);
		function At (index: integer): TDME;
	public
		constructor Create;
		property Item[I: Integer]: TDME read At write SetItem;
	end;

//------------------------------------------------------------------------------

	TObstacleList = class (TPandaList)
	private
		procedure SetItem (index: integer; const _Item: TObstacle);
		function At (index: integer): TObstacle;
	public
		constructor Create;
		property Item[I: Integer]: TObstacle read At write SetItem;
	end;

//------------------------------------------------------------------------------

	TObstacle = class(TAIXM)
	private
		ptGeo:			TPoint;
		ptPrj:			TPoint;

		FGeoAccuracy,
		FElevation,
		FElevationWithAccuracy,
		FHeight:							Double;

		function GetElevationAccuracy():	Double;
	public
		constructor Create;	override;
		destructor Destroy; override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAixm;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property Geo: TPoint read ptGeo;
		property Prj: TPoint read ptPrj;

		property GeoAccuracy: Double read FGeoAccuracy write FGeoAccuracy;
		property Elevation: Double read FElevation write FElevation;
		property ElevationAccuracy: Double read GetElevationAccuracy;
		property ElevationWithAccuracy: Double read FElevationWithAccuracy write FElevationWithAccuracy;
		property Height: Double read FHeight write FHeight;        
	end;

//------------------------------------------------------------------------------

	TRWY = class(TAIXM)
	public
		AerodromeID:		String;
		Width:				double;
		Length:				double;
	public
		constructor Create;	override;
		//destructor Destroy; override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAixm;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;
	end;

//------------------------------------------------------------------------------

	TRWYDirection = class(TAIXM)
	private
		ptGeo:				TPoint;
		ptPrj:				TPoint;
		FRwyID:  			String;

		FTrueBearing:		double;
		FMagBearing:		double;
		FDirection:			double;

		FElevTdz:			double;
		FElevTdzAccuracy:	double;
		FClearway:			Double;
		FDisplacement:		Double;
	public
		constructor Create;							override;
		destructor Destroy;							override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAIXM;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property Geo: TPoint read ptGeo;
		property Prj: TPoint read ptPrj;
		property RWY_ID: String read FRwyID write FRwyID;

		property TrueBearing: double read FTrueBearing write FTrueBearing;
		property MagBearing: double read FMagBearing write FMagBearing;
		property Direction:	double read FDirection write FDirection;
		property ElevTdz: double read FElevTdz write FElevTdz;
		property ElevTdzAccuracy: double read FElevTdzAccuracy write FElevTdzAccuracy;
		property Clearway: Double read FClearway write FClearway;
		property Displacement: Double read FDisplacement write FDisplacement;
	end;

//------------------------------------------------------------------------------
//	TFrameType = (ftNULL, ftDesignatedPoint, ftTACAN, ftVOR, ftNDB, ftDME);

	TSignificantPoint = class(TAIXM)
	private
		ptGeo:  			TPoint;
		ptPrj:  			TPoint;

		FFrameID:			String;
		FGeoAccuracy:		Double;
		FElevation,
		FElevationAccuracy:	Double;
	public
		constructor Create (objType: TAIXMType);	overload;
		constructor Create;							overload; override;
		destructor Destroy; 						override;

		procedure Assign (const value: TAIXM);		override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property Geo: TPoint read ptGeo;
		property Prj: TPoint read ptPrj;

		property FrameID: String read FFrameID write FFrameID;
		property GeoAccuracy: Double read FGeoAccuracy write FGeoAccuracy;

		property Elevation: Double read FElevation write FElevation;
		property ElevationAccuracy: Double read FElevationAccuracy write FElevationAccuracy;
	end;

//------------------------------------------------------------------------------

	TVOR = class(TSignificantPoint)
	private
		FOrgID:				String;
		FFrequency:			String;
		FType:				String;
		FMagneticVariation:	Double;

		FDeclination:		Double;
		FNorthType:			TVORNorthType;
	public
		constructor Create;							override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAIXM;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property OrgID: String read FOrgID write FOrgID;
		property Frequency: String read FFrequency write FFrequency;
		property VORType: String read FType write FType;
		property MagneticVariation: Double read FMagneticVariation write FMagneticVariation;

		property Declination: Double read FDeclination write FDeclination;
		property NorthType: TVORNorthType read FNorthType write FNorthType;
	end;

//------------------------------------------------------------------------------

	TNDB = class(TSignificantPoint)
	private
		FOrgID:				String;
		FFrequency:			String;
		FClass:				String;
		FMagneticVariation:	Double;

		FILSPosition:		Double;
	public
		constructor Create;							override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAIXM;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property OrgID: String read FOrgID write FOrgID;
		property Frequency: String read FFrequency write FFrequency;
		property NDBClass: String read FClass write FClass;
		property MagneticVariation: Double read FMagneticVariation write FMagneticVariation;
		property ILSPosition: Double read FILSPosition write FILSPosition;
	end;

//------------------------------------------------------------------------------

	TDME = class(TSignificantPoint)
	private
		FOrgID:			String;
		FChannel:		String;
		FType:			String;
		FDispalcement:	Double;

		FVOR_ID:		String;
	public
		constructor Create;							override;

		procedure Assign (const Value: TAIXM);		override;
		function Clone: TAIXM;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property OrgID: String read FOrgID write FOrgID;
		property Channel: String read FChannel write FChannel;
		property DMEType: String read FType write FType;
		property Dispalcement: Double read FDispalcement write FDispalcement;

		property VOR_ID: String read FVOR_ID write FVOR_ID;
	end;

//------------------------------------------------------------------------------

	TTACAN = class(TSignificantPoint)
	private
		FOrgID:				String;
		FChannel:			ShortString;

		FMagneticVariation:	Double;
		FDeclination:		Double;
	public
		constructor Create;							override;

		procedure Assign (const value: TAIXM);		override;
		function Clone: TAIXM;						override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property OrgID: String read FOrgID write FOrgID;
		property Channel: ShortString read FChannel write FChannel;
		property MagneticVariation: Double read FMagneticVariation write FMagneticVariation;
		property Declination: Double read FDeclination write FDeclination;

	end;

//------------------------------------------------------------------------------

	TDesignatedPoint = class (TSignificantPoint)
	public
		constructor Create;		override;
		function Clone: TAIXM;	override;
	end;

//------------------------------------------------------------------------------


const
	ItemType: Array [TAIXMType] of TAIXMClass =
			(TAIXM, TDME, TVOR, TObstacle, TAHP, TRWY,
			 TRWYDirection, TNDB, TDesignatedPoint, TTACAN);

implementation

uses
	SysUtils;
//================== TAIXM ==============================

constructor TAIXM.Create(ObjType: TAixmType);
begin
	inherited Create;
	FAixmType := ObjType;
	FID := '';
	FTag := 0
end;

constructor TAIXM.Create;
begin
	Create(atNull);
end;

procedure TAIXM.Assign (const value: TAIXM);
begin
	FAixmType := Value.FAixmType;
	FID := Value.FID;
	FAixmID := Value.FAixmID;
	FName := Value.FName;
	FRemark := Value.FRemark;
	FTag := Value.FTag;
end;

function TAIXM.CloneItem: TPandaItem;
begin
  result := Clone;
end;

procedure TAIXM.Pack (handle: TRegistryHandle);
begin
	inherited;
//	Contract.PutInt (handle, Integer(FAixmType));
	Contract.PutWideString (handle, FID);
	Contract.PutWideString (handle, FAixmID);
	Contract.PutWideString (handle, FName);
	Contract.PutWideString (handle, FRemark);
end;

procedure TAIXM.Unpack (handle: TRegistryHandle);
begin
	inherited;
//	FAixmtype := TAixmType (Contract.GetInt (handle));
	FID := Contract.GetWideString (handle);
	FAixmID := Contract.GetWideString (handle);
	FName := Contract.GetWideString (handle);
	FRemark := Contract.GetWideString (handle);
	FTag := 0;
end;

procedure TAIXM.AIXMPack (handle: TRegistryHandle);
begin
	Contract.PutInt32 (handle, Integer(FAixmType));
	Pack(handle);
end;

class function  TAIXM.AIXMUnpack (handle: TRegistryHandle): TAIXM;
var
	AixmType:	TAIXMType;
begin
	AixmType := TAixmType (Contract.GetInt32 (handle));
	result := ItemType[AixmType].Create;
	result.Unpack(handle);
end;

function TAIXM.asDme: TDME;
begin
	if FAixmType<>atDME then
		raise EConvertError.Create(FName + ' is not a valid DME.');
	result := TDME(self);
end;

function TAIXM.asNDB: TNDB;
begin
	if FAixmType<>atNDB then
		raise EConvertError.Create(FName + ' is not a valid NDB.');
	result := TNDB(self);
end;

function TAIXM.asVor: TVOR;
begin
	if FAixmType<>atVOR then
		raise EConvertError.Create(FName + ' is not a valid VOR.');
	result := TVOR(self);
end;

function TAIXM.asAhp: TAHP;
begin
	if FAixmType<>atAHP then
		raise EConvertError.Create(FName + ' is not a valid AHP.');
	result := TAHP(self);
end;

function TAIXM.asRwy: TRWY;
begin
	if FAixmType<>atRWY then
		raise EConvertError.Create(FName + ' is not a valid RWY.');
	result := TRWY(self);
end;

function TAIXM.asRwyDirection: TRWYDirection;
begin
	result := TRWYDirection(self);
end;

function TAIXM.asDesignatedPoint: TDesignatedPoint;
begin
	result := TDesignatedPoint (self);
end;

function TAIXM.asObstacle: TObstacle;
begin
	result := TObstacle (self);
end;

function TAIXM.asSignificantPoint: TSignificantPoint;
begin
	result := TSignificantPoint (self);
end;

function TAIXM.asTACAN: TTACAN;
begin
	result := TTACAN (self);
end;

//============== TAIXMGeometry =====================================

constructor TAIXMGeometry.Create (objType: TAIXMType; geo: TGeometry; prj: TGeometry);
begin
	inherited Create (objType);
	FGeoGeometry := geo;
	FPrjGeometry := prj;
end;

destructor TAIXMGeometry.Destroy;
begin
	FGeoGeometry.Free;
    FPrjGeometry.Free;
    inherited;
end;

procedure TAIXMGeometry.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FGeoGeometry.unpack(handle);
	FPrjGeometry.unpack(handle);
end;

procedure TAIXMGeometry.Pack (handle: TRegistryHandle);
begin
	inherited;
	FGeoGeometry.pack(handle);
	FPrjGeometry.pack(handle);
end;

procedure TAIXMGeometry.Assign (const Value: TAIXM);
var
	aixmGeometry: TAIXMGeometry;
begin
	Inherited Assign (Value);
    aixmGeometry := TAIXMGeometry (value);
	FGeoGeometry.Assign (aixmGeometry.FGeoGeometry);
	FPrjGeometry.Assign (aixmGeometry.FPrjGeometry);
end;

//============== TAHP ============================
constructor TAHP.Create;
begin
	Inherited Create(atAHP);

	ptGeo := TPoint.Create;
	ptPrj := TPoint.Create;

	elevation := 0;
	elevAccuracy := 0;
	magVar := 0;
	temperature := 0;
end;

destructor TAHP.Destroy;
begin
	ptGeo.Free;
	ptPrj.Free;

	inherited;
end;

procedure TAHP.Assign (const Value: TAIXM);
var
	src:	TAHP;
begin
	Inherited;

	src := Value.asAHP;
	ptGeo.Assign (src.ptGeo);
	ptPrj.Assign (src.ptPrj);

	Elevation := src.Elevation;
	ElevAccuracy := src.ElevAccuracy;
	MagVar := src.MagVar;
	Temperature := src.Temperature;
	CountryId := src.CountryId;
end;

function TAHP.Clone: TAIXM;
begin
	result := TAHP.Create;
	try
		result.Assign (self);
	except
		result.Free;
		raise;
	end;
end;

procedure TAHP.Pack (handle: TRegistryHandle);
begin
	Inherited;
	ptGeo.pack (handle);
	ptPrj.pack (handle);

	Contract.PutDouble(handle, elevation);
	Contract.PutDouble(handle, elevAccuracy);
	Contract.PutDouble(handle, magVar);
	Contract.PutDouble(handle, temperature);
	Contract.PutWideString (handle, CountryId);
end;

procedure TAHP.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.unpack(handle);
	ptPrj.unpack(handle);

	elevation := Contract.GetDouble(handle);
	elevAccuracy := Contract.GetDouble(handle);
	magVar := Contract.GetDouble(handle);
	temperature := Contract.GetDouble(handle);
	countryId := Contract.GetWideString (handle);
end;

//================== TSignificantPoint ==============================
constructor TSignificantPoint.Create (objType: TAIXMType);
begin
	Inherited;

	ptGeo := TPoint.Create;
	ptPrj := TPoint.Create;

	FFrameID := '';

	FGeoAccuracy := 0;
	FElevation := -9999;
	FElevationAccuracy := 0;
end;

constructor TSignificantPoint.Create;
begin
	Create (atNull);
end;

destructor TSignificantPoint.Destroy;
begin
	ptGeo.Free;
	ptPrj.Free;
	inherited
end;

procedure TSignificantPoint.Assign (const value: TAIXM);
begin
	Inherited Assign(Value);

	ptGeo.Assign(TSignificantPoint(value).ptGeo);
	ptPrj.Assign(TSignificantPoint(value).ptPrj);

//	FFrameType := TSignificantPoint(value).FFrameType;
	FFrameID := TSignificantPoint(value).FFrameID;

	FGeoAccuracy := TSignificantPoint(value).FGeoAccuracy;
	FElevation := TSignificantPoint(value).FElevation;
	FElevationAccuracy := TSignificantPoint(value).FElevationAccuracy;
end;

procedure TSignificantPoint.Pack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.Pack(handle);
	ptPrj.Pack(handle);

//	Contract.PutInt(handle, Integer(FFrameType));
	Contract.PutWideString (handle, FFrameID);
	Contract.PutDouble(handle, FGeoAccuracy);
	Contract.PutDouble(handle, FElevation);
	Contract.PutDouble(handle, FElevationAccuracy);
end;

procedure TSignificantPoint.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.unpack(handle);
	ptPrj.unpack(handle);

	FFrameID := Contract.GetWideString (handle);

	FGeoAccuracy := Contract.GetDouble(handle);
	FElevation := Contract.GetDouble(handle);
	FElevationAccuracy := Contract.GetDouble(handle);
end;

//========== TVOR ================
constructor TVOR.Create;
begin
	Inherited Create(atVOR);
	FOrgID := '';
	FFrequency := '';
	FType := '';
	FMagneticVariation := 0;
	FDeclination := 0;
	FNorthType := ntOther;
end;

{
destructor TVOR.Destroy;
begin
	inherited;
end;
}

procedure TVOR.Assign (const Value: TAIXM);
var
	src:	TVOR;
begin
	Inherited;

	src := Value.asVor;

	FOrgID := src.FOrgID;
	FFrequency := src.FFrequency;
	FType := src.FType;
	FMagneticVariation := src.FMagneticVariation;
	FDeclination := src.FDeclination;
	FNorthType := src.FNorthType;
end;

function TVOR.Clone: TAIXM;
begin
	result := TVOR.Create;
	try
		result.Assign (self);
	except
		result.Free;
		raise;
	end;
end;

procedure TVOR.Pack (handle: TRegistryHandle);
begin
	Inherited;
	Contract.PutWideString (handle, FOrgID);
	Contract.PutWideString(handle, FFrequency);
	Contract.PutWideString(handle, FType);
	Contract.PutDouble(handle, FMagneticVariation);
	Contract.PutDouble(handle, FDeclination);
	Contract.PutInt32 (handle, Integer(FNorthType));
end;

procedure TVOR.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FOrgID := Contract.GetWideString (handle);
	FFrequency := Contract.GetWideString(handle);
	FType := Contract.GetWideString(handle);
	FMagneticVariation := Contract.GetDouble(handle);
	FDeclination := Contract.GetDouble(handle);
	FNorthType := TVORNorthType(Contract.GetInt32 (handle));
end;

//================ TNDB ============================
constructor TNDB.Create;
begin
	inherited Create(atNDB);

	FOrgID := '';
	FFrequency := '';
	FClass := '';
	FMagneticVariation := 0;
	FILSPosition := 0;
end;
{
destructor TNDB.Destroy;
begin
	inherited;
end;
}
procedure TNDB.Assign (const Value: TAIXM);
var
	src:	TNDB;
begin
	Inherited;

	src := Value.asNDB;

	FOrgID := src.FOrgID;
	FFrequency := src.FFrequency;
	FClass := src.FClass;
	FMagneticVariation := src.FMagneticVariation;
	FILSPosition := src.FILSPosition;
end;

function TNDB.Clone: TAIXM;
begin
	result := TNDB.Create;
	try
		result.Assign (self);
	except
		result.Free;
		raise;
	end;
end;

procedure TNDB.Pack (handle: TRegistryHandle);
begin
	Inherited;

	Contract.PutWideString (handle, FOrgID);
	Contract.PutWideString (handle, FFrequency);
	Contract.PutWideString (handle, FClass);
	Contract.PutDouble (handle, FMagneticVariation);
	Contract.PutDouble (handle, FILSPosition);
end;

procedure TNDB.Unpack (handle: TRegistryHandle);
begin
	inherited;

	FOrgID := Contract.GetWideString (handle);
	FFrequency := Contract.GetWideString (handle);
	FClass := Contract.GetWideString (handle);
	FMagneticVariation := Contract.GetDouble (handle);
	FILSPosition := Contract.GetDouble (handle);
end;

//==================== TDME =============================
constructor TDME.Create;
begin
	Inherited Create(atDME);

	FOrgID := '';
	FChannel := '';
	FType := '';
	FDispalcement := 0;
	FVOR_ID := '';
//	FCodeID := '';
end;

{
destructor TDME.Destroy;
begin
	inherited;
end;
}
procedure TDME.Assign (const Value: TAIXM);
var
	src:	TDME;
begin
	Inherited;

	src := Value.asDme;

	FOrgID := src.FOrgID;
	FChannel := src.FChannel;
	FType := src.FType;
	FDispalcement := src.FDispalcement;
	FVOR_ID := src.FVOR_ID;
//	FCodeID := src.FCodeID;
end;

function TDME.Clone: TAIXM;
begin
	result := TDME.Create;
	try
		result.Assign (self);
	except
		result.Free;
		raise;
	end;
end;

procedure TDME.Pack (handle: TRegistryHandle);
begin
	Inherited;

	Contract.PutWideString (handle, FOrgID);
	Contract.PutWideString (handle, FChannel);
	Contract.PutWideString (handle, FType);
	Contract.PutDouble (handle, FDispalcement);
	Contract.PutWideString (handle, FVOR_ID);
//	Contract.PutWideString (handle, FCodeID);
end;

procedure TDME.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	FOrgID := Contract.GetWideString (handle);
	FChannel := Contract.GetWideString (handle);
	FType := Contract.GetWideString (handle);
	FDispalcement := Contract.GetDouble (handle);
	FVOR_ID := Contract.GetWideString (handle);
end;

//==================== TTACAN =============================
constructor TTACAN.Create;
begin
	inherited Create (atTACAN);
	FOrgID := '';
	FChannel := '';

	FMagneticVariation := 0;
	FDeclination := 0;
end;
{
destructor TTACAN.Destroy;
begin
	inherited;
end;
}
procedure TTACAN.Assign (const value: TAIXM);
var
	source:	TTACAN;
begin
	inherited;

    source := value.asTACAN;

	FOrgID := source.FOrgID;
	FChannel := source.FChannel;

	FMagneticVariation := source.FMagneticVariation;
	FDeclination := source.FDeclination;
end;

function TTACAN.Clone: TAIXM;
begin
	result := TTACAN.Create;
	try
		result.Assign (self);
	except
		result.Free;
		raise;
	end;
end;

procedure TTACAN.Pack (handle: TRegistryHandle);
begin
	inherited;
	Contract.PutWideString (handle, FOrgID);
	Contract.PutWideString (handle, FChannel);
	Contract.PutDouble (handle, FMagneticVariation);
	Contract.PutDouble (handle, FDeclination);
end;

procedure TTACAN.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FOrgID := Contract.GetWideString (handle);
	FChannel := Contract.GetWideString (handle);
	FMagneticVariation := Contract.GetDouble (handle);
	FDeclination := Contract.GetDouble (handle);
end;

//==================== TWPT =============================
constructor TDesignatedPoint.Create;
begin
	inherited Create (atDesignatedPoint)
end;

function TDesignatedPoint.Clone: TAIXM;
begin
	result := TDesignatedPoint.Create;
	result.Assign (self);
end;

//================== TObstacle ===============================

constructor TObstacle.Create;
begin
	Inherited Create(atObstacle);
	ptGeo := TPoint.Create;
	ptPrj := TPoint.Create;

	elevation := 0;
	height := 0;
end;

destructor TObstacle.Destroy;
begin
	ptGeo.Free;
	ptPrj.Free;
	inherited;
end;

function TObstacle.GetElevationAccuracy(): Double;
begin
	result := FElevationWithAccuracy - FElevation;
end;

procedure TObstacle.Assign (const Value: TAIXM);
var
	src:	TObstacle;
begin
	Inherited;

	src := Value.asObstacle;

	ptGeo.assign(src.ptGeo);
	ptPrj.assign(src.ptPrj);

	FGeoAccuracy := src.FGeoAccuracy;
	FElevation := src.FElevation;
	FElevationWithAccuracy := src.FElevationWithAccuracy;
	FHeight := src.FHeight;
end;

function TObstacle.Clone: TAIXM;
begin
	result := TObstacle.Create;
	try
		result.Assign(self);
	except
		result.Free;
		raise;
	end;
end;

procedure TObstacle.Pack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.pack(handle);
	ptPrj.pack(handle);

	Contract.PutDouble (handle, FGeoAccuracy);
	Contract.PutDouble (handle, FElevation);
	Contract.PutDouble (handle, FElevationWithAccuracy);
	Contract.PutDouble (handle, FHeight);
end;

procedure TObstacle.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.unpack(handle);
	ptPrj.unpack(handle);

	FGeoAccuracy := Contract.GetDouble (handle);
	FElevation := Contract.GetDouble (handle);
	FElevationWithAccuracy := Contract.GetDouble (handle);
	FHeight := Contract.GetDouble (handle);
end;

//================ TRWY ============================
constructor TRWY.Create;
begin
	Inherited Create(atRWY);
end;

procedure TRWY.Assign (const Value: TAIXM);
var
	source:		TRWY;
begin
	Inherited;
	source := Value.asRwy;

	AerodromeID := source.aerodromeID;
	Length := source.Length;
	Width := source.Width;
end;

function TRWY.Clone: TAIXM;
begin
	result := TRWY.Create;
	try
		result.Assign(self);
	except
		result.Free;
		raise;
	end;
end;

procedure TRWY.Pack (handle: TRegistryHandle);
begin
	Inherited;

	Contract.PutWideString (handle, AerodromeID);
	Contract.PutDouble(handle, Length);
	Contract.PutDouble(handle, Width);
end;

procedure TRWY.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	AerodromeID := Contract.GetWideString (handle);
	Length := Contract.GetDouble (handle);
	Width := Contract.GetDouble (handle);
end;

//================ TRWYDirection ============================
constructor TRWYDirection.Create;
begin
	Inherited Create(atRWY);

	ptGeo := TPoint.Create;
	ptPrj := TPoint.Create;

	FTrueBearing := 0;
	FMagBearing := 0;
	FDirection := 0;
	FElevTdz := 0;
	FElevTdzAccuracy := 0;
	FClearway := 0;
	FDisplacement := 0;
end;

destructor TRWYDirection.Destroy;
begin
	ptGeo.Free;
	ptPrj.Free;
end;

procedure TRWYDirection.Assign (const Value: TAIXM);
var
	source:		TRWYDirection;
begin
	Inherited;
	source := Value.asRwyDirection;

	ptGeo.assign(source.ptGeo);
	ptPrj.assign(source.ptPrj);

	FTrueBearing := source.FTrueBearing;
	FDirection := source.FDirection;
	FMagBearing := source.FMagBearing;
	FElevTdz := source.FElevTdz;
	FElevTdzAccuracy := source.FElevTdzAccuracy;
	FRwyID := source.FRwyID;
	FClearway := source.FClearway;
	FDisplacement := source.FDisplacement;
end;

function TRWYDirection.Clone: TAIXM;
begin
	result := TRWYDirection.Create;
	try
		result.Assign(self);
	except
		result.Free;
		raise;
	end;
end;

procedure TRWYDirection.Pack (handle: TRegistryHandle);
begin
	Inherited;
	ptGeo.pack (handle);
	ptPrj.pack (handle);

	Contract.PutWideString (handle, FRwyID);
	Contract.putDouble (handle, FTrueBearing);
	Contract.putDouble (handle, FMagBearing);
	Contract.putDouble (handle, FDirection);

	Contract.putDouble (handle, FElevTdz);
	Contract.putDouble (handle, FElevTdzAccuracy);
	Contract.putDouble (handle, FClearway);
	Contract.putDouble (handle, FDisplacement);
end;

procedure TRWYDirection.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	ptGeo.unpack(handle);
	ptPrj.unpack(handle);

	FRwyID := Contract.GetWideString (handle);
	FTrueBearing := Contract.getDouble (handle);
	FMagBearing := Contract.getDouble (handle);
	FDirection := Contract.getDouble (handle);

	FElevTDZ := Contract.getDouble (handle);
	FElevTdzAccuracy := Contract.getDouble (handle);
	FClearway := Contract.GetDouble (handle);
	FDisplacement := Contract.GetDouble (handle);
end;

constructor TAHPList.Create;
begin
	inherited Create (TAhp.Create);
end;

//================ TAHPList ============================
procedure TAHPList.SetItem (index: integer; const _Item: TAHP);
begin
	inherited SetItem(index, _Item);
end;

function TAHPList.At (index: integer): TAHP;
begin
	result := TAHP(inherited At(index));
end;

//================ TRWYList ============================
constructor TRWYList.Create;
begin
	inherited Create (TRWY.Create);
end;

procedure TRWYList.SetItem (index: integer; const _Item: TRWY);
begin
	inherited SetItem(index, _Item);
end;

function TRWYList.At (index: integer): TRWY;
begin
	result := TRWY(inherited At(index));
end;

//================ TRWYDirectionList ============================
constructor TRWYDirectionList.Create;
begin
	inherited Create (TRWYDirection.Create);
end;

procedure TRWYDirectionList.SetItem (index: integer; const _Item: TRWYDirection);
begin
	inherited SetItem(index, _Item);
end;

function TRWYDirectionList.At (index: integer): TRWYDirection;
begin
	result := TRWYDirection(inherited At(index));
end;

{ TDMEList }

function TDMEList.At(index: integer): TDME;
begin
	result := TDME(inherited At(index));
end;

constructor TDMEList.Create;
begin
	inherited Create ( TDME.Create);
end;

procedure TDMEList.SetItem(index: integer; const _Item: TDME);
begin
	inherited SetItem(index, _Item);
end;

{ TObstacleList }

function TObstacleList.At(index: integer): TObstacle;
begin
	result := TObstacle(inherited At(index));
end;

constructor TObstacleList.Create;
begin
	inherited Create (TObstacle.Create);
end;

procedure TObstacleList.SetItem(index: integer; const _Item: TObstacle);
begin
	inherited SetItem(index, _Item);
end;

end.
