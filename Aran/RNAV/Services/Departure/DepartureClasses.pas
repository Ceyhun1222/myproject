unit DepartureClasses;

interface

uses
	Windows,
	Contract,
	AIXMTypes,
	ARANFunctions,
	ARANClasses,
	Controls,
	CollectionUnit,
	ConstantsContract,
	GeometryOperatorsContract,
	UIContract,
	Geometry,
	Dialogs,
	SysUtils,
	Classes,
	ARANMath;

const
	DeltaDist = 25000;

type
	TWhichArea = (waLeft = -1, waPrimary=0, waRight = 1);
    TSensorType = (stGNSS, stDME);
	TTurnType = (tuFlyBy, tuFlyOver, tuAtHeight);
	TCodeType = (ctTF, ctCF, ctDF);

//----------------------------------

	TGraphicItem = class
	private

		FGeometry:	TGeometry;
		FSymbol:	TSymbol;
		FHandle:	Integer;
	public
		constructor Create ();
		destructor Destroy (); override;

		procedure Draw ();
		procedure Erase ();

		property Geometry: TGeometry read FGeometry write FGeometry;
		property Symbol: TSymbol read FSymbol write FSymbol;
		property Handle: Integer read FHandle;
	end;

//----------------------------------

	TObstacleInArea = class (TObstacle)
	public
		WhichArea:		TWhichArea;
		TransformX,
		TransformY,
		MOC,
		MOCCoefficient,
		AboveDER,
		Pdg,
		PdgToTop,
		RequiredGrd,
		RequiredTNH,
		RequiredH,
		DistPDG,
		HPenetration,
		Dr:				Double;
		Ignored:		Boolean;
		Tag:			Integer;

		procedure Assign (const src: TObstacleInArea); overload;
		function Clone: TObstacleInArea; overload;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;
	end;

//----------------------------------

	TObstacleInAreaList = class (TList)
	private
		function GetItem (index: Integer): TObstacleInArea;
	public
		destructor Destroy; override;
		procedure Add (value: TObstacleInArea); overload;

		property Item [index: Integer]: TObstacleInArea read GetItem;
	end;

    TReportItem = class
    private
        FObstacleList:  TObstacleInAreaList;
    public
        PDG:    Double;

        property ObtacleList: TObstacleInAreaList read FObstacleList write FObstacleList;
    end;
//----------------------------------

	TDepartureControl = class (TControl)
		public
		property Color;
	end;

//----------------------------------

	TParameters = class
	private
		FStartRwyDirection:		TRwyDirection;
		FPairRwyDirection:		TRwyDirection;
		FUI:					TUIContract;
        FModelAreaRadius,
		FMagVar:				Double;
		FObstacles:				TObstacleList;
		FSignificantsPoints:	TSignificantPointCollection;
		FAHP:					TAHP;
		FEndSignificantPoint: 	TSignificantPoint;
        FSensorType:            TSensorType;
        FIsDMEtill_1989:        Boolean;
        FDMECount:              Integer;
	public
		constructor Create;
		destructor Destroy; override;

		function RadianToMagAzimuth (prjPoint: TPoint; radian: Double): Double;
		function RadianToAzimuth (prjPoint: TPoint; radian: Double): Double;
		function MagAzimuthToRadian (geoPoint: TPoint; azimuth: Double): Double;
		function AzimuthToRadian (geoPoint: TPoint; azimuth: Double): Double;
		function ToRadius (bankAngle, IAS, elevation: Double): Double;
        function ToTAS (IAS, elevation: Double): Double;        

		property StartRwyDirection:	TRwyDirection read FStartRwyDirection write FStartRwyDirection;
		property PairRwyDirection: TRwyDirection read FPairRwyDirection write FPairRwyDirection;
		property UI: TUIContract read FUI write FUI;
		property MagVar: Double read FMagVar write FMagVar;
		property Obstacles: TObstacleList read FObstacles write FObstacles;
		property SignificantPoints: TSignificantPointCollection read FSignificantsPoints write FSignificantsPoints;
		property AHP: TAHP read FAHP write FAHP;
		property EndSignificantPoint: TSignificantPoint read FEndSignificantPoint write FEndSignificantPoint;
		property ModelAreaRadius: Double read FModelAreaRadius write FModelAreaRadius;
        property SensorType: TSensorType read FSensorType write FSensorType;
        property IsDMEtill_1989: Boolean read FIsDMEtill_1989 write FIsDMEtill_1989;
        property DMECount: Integer read FDMECount write FDMECount;
	end;

//----------------------------------

	TStraight = class
	private
		FParams:					TParameters;
		FSignificantPointsByArea:	TPandaCollection;
		FSegment:					TProtectedSegment;
		FMaxRequiredGrdIndex:       Integer;
		FMaxDistPdgIndex:		    Integer;
		FObstacles:				    TObstacleInAreaList;
		FCL:                        TPolyline;
		FExpandPoints:				TMultiPoint;
	private
		procedure CreateStZoneRNAV (startPoint: TPoint; startDir, range, dist: Double);
		procedure FillObstaclesDistPdg (userInputPDG: Double);
		procedure FillObstaclesRequiredTNH_PANDA ();
	public
		constructor Create (params: TParameters);
		destructor Destroy; override;

		procedure FillObstaclesInStraightArea (nominalRadianDir, userInputPDG: Double; area: TProtectedArea);
		procedure FillSignificantPointByArea (significantPtCol: TPandaCollection; radianRWYDir: Double);
		procedure CreateArea (shiftDir, distance: Double; createExtededArea: Boolean);
        procedure CutZoneByLine (startPoint: TPoint; startDir: Double; alphaShift: Double; StDepRang: Double; area: TProtectedArea);

		property Parameters: TParameters read FParams ;
		property SignificantPointsByArea: TPandaCollection read FSignificantPointsByArea;
		property Segment: TProtectedSegment read FSegment;
		property MaxRequiredGrdIndex: Integer read FMaxRequiredGrdIndex;
		property MaxDistPdgIndex: Integer read FMaxDistPdgIndex;
		property ExpandPoints: TMultiPoint read FExpandPoints;
		property Obstacles: TObstacleInAreaList read FObstacles;
	end;
	
//----------------------------------

	// By default TurnSide = sideRight.
	TToleranceLineInfo = class
	private
		FPointCount:	Integer; // = 5; constant.
		FIndexes:		Array [0..4] of Integer;
		FWidths:		Array [0..4] of Double;

		FTolerancePoints:	TMultiPoint;
		FSideDirection:		TSideDirection;

		procedure SetSideDirection (vSideDirection: TSideDirection);
		function GetOuterPoint: TPoint;
		function GetPrimaryPoint (index: Integer): TPoint;
		function GetInnerPoint: TPoint;
		function GetHasOuter: Boolean;
		function GetHasInner: Boolean;
		function GetOuterWidth: Double;
		function GetPrimOuterWidth: Double;
		function GetPrimInnerWidth: Double;
		function GetInnerWidth: Double;
		function GetTolerancePoint (index: Integer): TPoint;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TToleranceLineInfo);
		function Clone: TToleranceLineInfo;
		procedure Clear;

		procedure SetAccuracyLine (middlePoint: TPoint; nominalDir: Double; cutLine: TPolyline; area: TProtectedArea);

		property OuterWidth: Double read GetOuterWidth;
		property PrimOuterWidth: Double read GetPrimOuterWidth;
		property PrimInnerWidth: Double read GetPrimInnerWidth;
		property InnerWidth: Double read GetInnerWidth;
		property SideDirection: TSideDirection read FSideDirection write SetSideDirection;
		property OuterPoint: TPoint read GetOuterPoint;
		property PrimaryPoint [i: Integer]: TPoint read GetPrimaryPoint;
		property InnerPoint: TPoint read GetInnerPoint;
		property HasOuter: Boolean read GetHasOuter;
		property HasInner: Boolean read GetHasInner;
		property PointList: TMultiPoint read FTolerancePoints;
	end;

//---------------------------------------

	TSegmentPoint = class
	private
		FPoint:					TPoint;
		FSignificantPointID:	String;
		FDirection,
		FDistance,
		FATT:					Double;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TSegmentPoint); virtual;
		function Clone: TSegmentPoint; virtual;
		procedure Clear; virtual;

		property Point: TPoint read FPoint;
		property SignificantPointID: String read FSignificantPointID write FSignificantPointID;
		property Direction: Double read FDirection write FDirection;
		property Distance: Double read FDistance write FDistance;
		property ATT: Double read FATT write FATT;		
	end;

//---------------------------------------

	TNextPoint = class (TSegmentPoint)
	private
		FLatestDirection,
		FEarlestDirection,
		FWidth:					Double;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TSegmentPoint); override;
		function Clone (): TSegmentPoint; override;
		procedure Clear (); override;

		property Width: Double read FWidth write FWidth;
		property LatestDirection: Double read FLatestDirection write FLatestDirection;
		property EarlestDirection: Double read FEarlestDirection write FEarlestDirection;	
	end;
	
//---------------------------------------

	TStartPoint = class (TSegmentPoint)
	private
		FETP:               Double;
		FLTP:               Double;
		FXTT:               Double;
        FHeightToKK:        Double;
        FDistanceToKK:      Double;
		FHeightProtect:     Double;
		FHeightNominal:		Double;
		FEarlestTolerance:  TToleranceLineInfo;
		FNominalTolerance:  TToleranceLineInfo;
		FLatestTolerance:	TToleranceLineInfo;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TSegmentPoint); override;
		function Clone (): TSegmentPoint; override;
		procedure Clear (); override;

		property EarlestTolerance: TToleranceLineInfo read FEarlestTolerance;
		property NominalTolerance: TToleranceLineInfo read FNominalTolerance;
		property LatestTolerance: TToleranceLineInfo read FLatestTolerance;
		property ETP: Double read FETP write FETP;
		property LTP: Double read FLTP write FLTP;
		property XTT: Double read FXTT write FXTT;
        property HeightToKK: Double read FHeightToKK write FHeightToKK;
        property DistanceToKK: Double read FDistanceToKK write FDistanceToKK;
		property HeightNominal: Double read FHeightNominal write FHeightNominal;
		property HeightProtect: Double read FHeightProtect write FHeightProtect;
	end;

//---------------------------------------

	TTurnInfo = class
	private
		FStart:				TStartPoint;
		FNext:				TNextPoint;
		FMaxRequiredGrdIndex: Integer;
		FNominalRadius,
		FProtectedRadius,
		FFlyBySpiralRadius,
		FIAS,
		FBankAngle,
		FPDG,
		FPDGNominal,
		FPDGProtect,
		FTurnAngle:			Double;
		FAircraftCategory:	TAircraftCategory;
		FPrevDFSideOn:		Boolean;
		FTurnType:			TTurnType;
		FCodeType:			TCodeType;
		FTurnSide:			TSideDirection;
		FSegment:			TProtectedSegment;
		FObstacles:			TObstacleInAreaList;
		FPrevTurnInfo:		TTurnInfo;
		FInnerStartPoint:	TPoint;
	public
		tmp_codeTypeIsCF:	Boolean;

		constructor Create ();
		destructor Destroy; override;

		procedure Assign (src: TTurnInfo);
		function Clone (): TTurnInfo;
		procedure Clear;

		property Start: TStartPoint read FStart;
		property Next: TNextPoint read FNext;
		property MaxRequiredGrdIndex: Integer read FMaxRequiredGrdIndex write FMaxRequiredGrdIndex;
		property NominalRadius: Double read FNominalRadius write FNominalRadius;
		property ProtectedRadius: Double read FProtectedRadius write FProtectedRadius;
		property FlyBySpiralRadius: Double read FFlyBySpiralRadius write FFlyBySpiralRadius;
		property IAS: Double read FIAS write FIAS;
		property BankAngle: Double read FBankAngle write FBankAngle;
		property PDG: Double read FPDG write FPDG;
		property PDGNominal: Double read FPDGNominal write FPDGNominal;
		property PDGProtect: Double read FPDGProtect write FPDGProtect;
		property AircraftCategory: TAircraftCategory read FAircraftCategory write FAircraftCategory;
		property PrevDFSideOn: Boolean read FPrevDFSideOn write FPrevDFSideOn;
		property TurnAngle: Double read FTurnAngle write FTurnAngle;
		property TurnType: TTurnType read FTurnType write FTurnType;
		property CodeType: TCodeType read FCodeType write FCodeType;
		property TurnSide: TSideDirection read FTurnSide write FTurnSide;
		property Segment: TProtectedSegment read FSegment;
		property Obstacles: TObstacleInAreaList read FObstacles;
		property PrevTurnInfo: TTurnInfo read FPrevTurnInfo write FPrevTurnInfo;
		property InnerStartPoint: TPoint read FInnerStartPoint;
	end;

//---------------------------------------

	TTurnInfoList = class
	private
		FTurnInfoList:	TList;
	private
		function GetItem (index: Integer): TTurnInfo;
		function GetCount (): Integer;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Add (value: TTurnInfo);
		procedure Remove (index: Integer);
		procedure Clear ();

		property Item [index: Integer]: TTurnInfo read GetItem;
		property Count: Integer read GetCount;
	end;

//---------------------------------------

	TSegmentSymbol = class
	private
		FAdditionGraphicHandle,
		FPrimaryGraphicHandle,
		FCLGraphicHandle,
		FNominalGraphicHandle,
		FEarlestLineHandle,
		FLatestLineHandle:	Integer;

		FUI:	TUIContract;

		FSymbolEarlestLine,
		FSymbolLatestLine:	TLineSymbol;

		FSymbolPrimary:		TFillSymbol;
		FSymbolAddition:	TFillSymbol;
		FSymbolCL:			TLineSymbol;
		FSymbolNominal:		TLineSymbol;
	private
		procedure DeleteGraphic (var handle: Integer);
	public
		constructor Create (aUI: TUIContract); overload;
		destructor Destroy; override;

		procedure DrawArea (protectArea: TProtectedArea; earlestToleranceLine, latestToleranceLine: TPolyline);
		procedure DrawArea2 (protectArea: TProtectedArea; earlestToleranceLine, latestToleranceLine: TPolyline);
		procedure DrawArea3 (protectArea: TProtectedArea; earlestToleranceLine, latestToleranceLine: TPolyline);
		procedure EraseArea ();

		property UI: TUIContract read FUI;
		property SymbolPrimary: TFillSymbol read FSymbolPrimary;
		property SymbolAddition: TFillSymbol read FSymbolAddition;
		property SymbolCL: TLineSymbol read FSymbolCL;
		property SymbolNominal: TLineSymbol read FSymbolNominal;
		property SymbolEarlestLine: TLineSymbol read FSymbolEarlestLine;
		property SymbolLatestLine: TLineSymbol read FSymbolLatestLine;
	end;

//---------------------------------------

	TTurn = class
	private
		FParams:			TParameters;
		FStraight:			TStraight;
		FRwyInfo:       	TTurnInfo;
		FTurnInfo:			TTurnInfo;
		FTurnInfoList:		TTurnInfoList;
		FSegmentSymbol:		TSegmentSymbol;

		FAddToleranceRectHandle: Integer;
		FPrimaryToleranceRectHandle: Integer;
		FStartPointHandle: Integer;

		FAddToleranceSymbol: TFillSymbol;
		FPrimaryToleranceSymbol: TFillSymbol;

		FNextXTT_tmp: Double; //--- just added in Rio.
	private
		function GetEdges (): TPolyline;
		function CreateNominalTF (L1, L2, R2, alpha: Double): TPolyline;
		function CreateNominalCF (R2: Double): TPolyline;
		function CreateNominalDF (): TPolyline;
		function GetOuterSpiral (edgeLine: TPolyline; radius: Double; spiralStartPoints: TMultiPoint; ATTValue: Double; var paralelPointIndex: Integer;
					var edgeIntersectPtIndex: Integer; var paralelSideByEdge: TSideDirection): TMultiPoint;
		function GetInnerAddArea (edgeLine: TPolyline; startPoint: TPoint; var startPointSide: TSideDirection; ATTValue: Double): TMultiPoint;
		function GetSpiralSideSplitIndex (spiralPoints: TMultiPoint; edgeLine: TPolyline; turnSide: TSideDirection; var intersectPoint: TPoint): Integer;
		function DeleteSpiralOutsidePoints (spiralPoints: TMultiPoint; edgeLine: TPolyline; turnSide: TSideDirection): Boolean;
		function EdgeSideDef (edgeLine: TPolyline; outPoint: TPoint): TSideDirection;
		function GetOuterSpiralDF (radius, nextSemiWidth, ATTValue: Double; var outerNominalDir: Double; basePoints: TMultiPoint;
					latestNominalPart: TPart; var intersectPointIndex: Integer): TMultiPoint;
		function CalcVerHorTolerance (var L1, L2, R2, alpha: Double): Double;
		function GetDFSegmentInner (startPoint: TPoint; innerNominalDir, innerNextWidth, ATTValue: Double): TMultiPoint;
		function CutAreaByToleranceLine (polygon: TPolygon; tolereanceLine: TToleranceLineInfo; getSideGeom: TSideDirection; var cutLine: TPolyline): TPolygon;
		function CheckInnersIntersect (primaryInnerPoints, additionInnerPoints: TMultiPoint): Boolean;
		procedure CreateSegment (edgePoly: TPolyline; ATTValue: Double);
		procedure CreateSegmentDF (ATTValue: Double);
		procedure CreateProtectedArea (L1, L2, R2, alpha: Double; ATTValue: Double);
		procedure CalcTolerance ();
        procedure CalcKKParams ();
		procedure JoinAreaToPrev (additionOuterLine, primaryOuterLine: TPolyline; var addAreaIndexOnPrimOuter: Integer);
        function CreateCutterLineBaseToleranceLine (tolLine: TToleranceLineInfo): TPolyline;
		procedure AtHeightMakeAreaToKK ();
		procedure AddToArea ();        
	public
		constructor Create (AParams: TParameters; AStraight: TStraight);
		destructor Destroy; override;

		procedure CreateArea ();
		procedure CreateStaightAreaDME (createExtendedArea: Boolean);
		procedure DrawStraightArea ();
		procedure DrawAreaAtHeight ();
		procedure DrawArea ();
		procedure DrawToleranceRectangle ();
		procedure CalcMinPDG (calculateStraight: Boolean);

		property Parameters: TParameters read FParams ;
		property TurnInfo: TTurnInfo read FTurnInfo;
		property RwyInfo: TTurnInfo read FRwyInfo;
		property TurnInfoList: TTurnInfoList read FTurnInfoList;
		property Straight: TStraight read FStraight write FStraight;
		property SegmentSymbol: TSegmentSymbol read FSegmentSymbol;
	end;

//---------------------------------------

	TFilterNextPointsParams = class
	public
		StartMinDistance:   Double;
		Distance:           Double;
		Radius:             Double;
		Direction:		    Double;
		Point:			    TPoint;
		TurnType:		    TTurnType;

		constructor Create;

		procedure Clear ();
		function IsEqual (other: TFilterNextPointsParams): Boolean;
		procedure Assign (src: TFilterNextPointsParams);
	end;

    TCreateStraightAreaParams = class
    public
        Direction:  Double;
        Distance:   Double;
        TurnType:   TTurnType;
        PDG:        Double;

        constructor Create;        
    end;

//-------------
//--- Functions
//-------------

function IsObstacleExists (obstacleID: String; obstacleList: TObstacleInAreaList): Boolean;


implementation

uses
	Math,
	UnitConverter,
	ARANGlobals,
	DepartureGlobals,
    Parameter;

//-------------
//--- Functions
//-------------

function IsObstacleExists (obstacleID: String; obstacleList: TObstacleInAreaList): Boolean;
var
	i:	Integer;
begin
	for i:=0 to obstacleList.Count - 1 do
	begin
		if obstacleID = obstacleList.Item [i].ID then
		begin
			result := True;
			exit;
		end;
	end;
	result := False;
end;

//------------------
//---TGraphicItem
//------------------

constructor TGraphicItem.Create ();
begin
	inherited;
	FHandle := -1;
end;

destructor TGraphicItem.Destroy;
begin
	inherited;
	DeleteGraphic(FHandle);
end;

procedure TGraphicItem.Draw ();
begin
	DeleteGraphic (FHandle);
	FHandle := DrawGeometry (FGeometry, FSymbol);
end;

procedure TGraphicItem.Erase;
begin
	DeleteGraphic (FHandle);
end;

//------------------
//---TObstacleInStraightArea
//------------------

procedure TObstacleInArea.Assign (const src: TObstacleInArea);
begin
	Inherited Assign (TObstacle (src));

	WhichArea := src.WhichArea;
	TransformX := src.TransformX;
	TransformY := src.TransformY;
	MOC := src.MOC;
	MOCCoefficient := src.MOCCoefficient;
	AboveDER := src.AboveDER;
	PDG := src.PDG;
	PDGToTop := src.PDGToTop;
	RequiredGrd := src.RequiredGrd;
	RequiredTNH := src.RequiredTNH;
	RequiredH := src.RequiredH;
	DistPDG := src.DistPDG;
	HPenetration := src.HPenetration;
	Dr := src.Dr;
	Ignored := src.Ignored;
	Tag := src.Tag;	
end;

function TObstacleInArea.Clone: TObstacleInArea;
begin
	result := TObstacleInArea.Create;
	try
		result.Assign(self);
	except
		result.Free;
		raise;
	end;
end;

procedure TObstacleInArea.Pack (handle: TRegistryHandle);
begin
	Inherited;

	Contract.PutInt32 (handle, ord(WhichArea));
	Contract.PutDouble (handle, TransformX);
	Contract.PutDouble (handle, TransformY);
	Contract.PutDouble (handle, MOC);
	Contract.PutDouble (handle, MOCCoefficient);
	Contract.PutDouble (handle, AboveDER);
	Contract.PutDouble (handle, PDG);
	Contract.PutDouble (handle, PDGToTop);
	Contract.PutDouble (handle, RequiredGrd);
	Contract.PutDouble (handle, RequiredTNH);
	Contract.PutDouble (handle, RequiredH);
	Contract.PutDouble (handle, DistPDG);
	Contract.PutDouble (handle, HPenetration);
	Contract.PutDouble (handle, Dr);
	Contract.PutBool (handle, Ignored);
	Contract.PutInt32 (handle, Tag);
end;

procedure TObstacleInArea.Unpack (handle: TRegistryHandle);
begin
	Inherited;

	WhichArea := TWHichArea (Contract.GetInt32 (handle));
	TransformX := Contract.GetDouble (handle);
	TransformY := Contract.GetDouble (handle);
	MOC := Contract.GetDouble (handle);
	MOCCoefficient := Contract.GetDouble (handle);
	AboveDER := Contract.GetDouble (handle);
	PDG := Contract.GetDouble (handle);
	PDGToTop := Contract.GetDouble (handle);
	RequiredGrd := Contract.GetDouble (handle);
	RequiredTNH := Contract.GetDouble (handle);
	RequiredH := Contract.GetDouble (handle);
	DistPDG := Contract.GetDouble (handle);
	HPenetration := Contract.GetDouble (handle);
	Dr := Contract.GetDouble (handle);	
	Ignored := Contract.GetBool (handle);
	Tag := Contract.GetInt32 (handle);
end;


//------------------
//--- TOstacleInAreList
//------------------

procedure TObstacleInAreaList.Add(value: TObstacleInArea);
begin
	Add (Pointer (value.Clone));
end;

destructor TObstacleInAreaList.Destroy;
var
	i:	Integer;
begin
	for i:=0 to Count-1 do
		GetItem (i).Free;
	inherited;
end;

function TObstacleInAreaList.GetItem(index: Integer): TObstacleInArea;
begin
	result := TObstacleInArea (Items [index]);	
end;

//------------------
//--- TParameters
//------------------

constructor TParameters.Create;
begin
	inherited;
end;

destructor TParameters.Destroy;
begin
	inherited;
end;

function TParameters.RadianToMagAzimuth (prjPoint: TPoint; radian: Double): Double;
begin
	result := DirToAzimuth (prjPoint, radian, GPrjSR, GGeoSR) - FMagVar;
end;

function TParameters.RadianToAzimuth (prjPoint: TPoint; radian: Double): Double;
begin
	result := DirToAzimuth (prjPoint, radian, GPrjSR, GGeoSR);
end;

function TParameters.MagAzimuthToRadian (geoPoint: TPoint; azimuth: Double): Double;
begin
	result := AztToDirection (geoPoint, azimuth + FMagVar, GGeoSR, GPrjSR)
end;

function TParameters.AzimuthToRadian (geoPoint: TPoint; azimuth: Double): Double;
begin
	result := AztToDirection (geoPoint, azimuth, GGeoSR, GPrjSR);
end;

//------------------
//--- TStaight
//------------------

constructor TStraight.Create (params: TParameters);
begin
	inherited Create;

	FMaxRequiredGrdIndex := -1;
	FMaxDistPdgIndex := -1;

	FParams := params;
	FSignificantPointsByArea := TPandaCollection.Create;
	FSegment := TProtectedSegment.Create;
	FCL := TPolyline.Create;

	FObstacles := TObstacleInAreaList.Create;
	FExpandPoints := TMultiPoint.Create;
end;

destructor TStraight.Destroy;
begin
	inherited;

	FSignificantPointsByArea.Free;
	FSegment.Free;
	FCL.Free;

	FObstacles.Free;
	ExpandPoints.Free;
end;

procedure TStraight.FillSignificantPointByArea (significantPtCol: TPandaCollection; radianRWYDir: Double);
var
	i:			Integer;
	sigPoint:	TSignificantPoint;
	minVal,
	maxVal:		Double;
	side:		TSideDirection;
begin
	FSignificantPointsByArea.Clear;
       
	minVal := radianRWYDir - GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;
	maxVal := radianRWYDir + GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;

	for i:=0 to significantPtCol.Count - 1 do
	begin
		sigPoint := significantPtCol.Item [i].asSignificantPoint;

		if (sigPoint.AIXMType = atDesignatedPoint) or (sigPoint.AIXMType = atVOR) then
		begin
			side := SideDef (FParams.StartRwyDirection.Prj, minVal, sigPoint.Prj);
			if  (side = sideLeft) or (side = sideOn) then
			begin
				side := SideDef (FParams.StartRwyDirection.Prj, maxVal, sigPoint.Prj);
				if (side = sideRight) or (side = sideOn) then
					FSignificantPointsByArea.Add (sigPoint);
			end;
		end;
	end;
end;

procedure TStraight.CutZoneByLine (
			startPoint:		TPoint;
			startDir: 		Double;
			alphaShift:		Double;
			StDepRang:		Double;
            area:			TProtectedArea);
const
	widthInDER = 150.0;
	alpha = 15 * Pi / 180;
	firstZoneLenght = 3500.0;
var
	ptLeft150,
	ptRight150,
	tempPoint,
	tempPoint2:		TPoint;
	cutLine:		TPolyline;
	partCutLine:	TPart;
	geomLeft,
	geomRight,
	tempGeo:		TGeometry;

	isLeftPolygonNotIntersect,
	isRightPolygonNotIntersect:	Boolean;
begin
	tempPoint := nil;

	cutLine := TPolyline.Create;
	partCutLine := TPart.Create;

	ptLeft150 := PointAlongPlane (startPoint, startDir + 0.5 * Pi, widthInDER);
	ptRight150 := PointAlongPlane (startPoint, startDir - 0.5 * Pi, widthInDER);

	isLeftPolygonNotIntersect := False;
	isRightPolygonNotIntersect := False;

	try
		if AlphaShift >= 0 then
	    begin
	        //---cut line for LEFT polygon
			tempPoint := PointAlongPlane (ptLeft150, startDir + alpha + alphaShift + Pi, StDepRang);
	        partCutLine.AddPoint (tempPoint);
			tempPoint.Free;
			tempPoint := PointAlongPlane (ptLeft150, startDir + alpha + AlphaShift, StDepRang * 2.0);
	        partCutLine.AddPoint (tempPoint);
			cutLine.AddPart (partCutLine);

			//---checking for existing points in LEFT polygon AND cutting LEFT polygon
			tempGeo := GeoOper.intersect (area.LeftArea, cutLine);
	        if not IsGeometryEmpty (tempGeo) then
	        begin
				GeoOper.cut (area.LeftArea, cutLine, geomLeft, geomRight);
				area.LeftArea.Assign (geomRight);
	            geomLeft.Free;
	            geomRight.Free;
	        end
	        else
			begin
				isLeftPolygonNotIntersect := True;
	        end;
	        tempGeo.Free;

	       	//---cutting PRIMARY polygon
			GeoOper.cut (area.PrimaryArea, cutLine, geomLeft, geomRight);
			area.PrimaryArea.Assign (geomRight);
	       	geomLeft.Free;
	       	geomRight.Free;

	       	cutLine.Clear;
	        partCutLine.Clear;

	       	//---cut line for RIGHT polygon
	        tempPoint.Free;
			tempPoint := PointAlongPlane (ptRight150, startDir - alpha - Pi, StDepRang);
	        partCutLine.AddPoint (tempPoint);
			tempPoint.Free;
			tempPoint := PointAlongPlane (ptRight150, startDir - alpha, (firstZoneLenght / Cos (DegToRad (alpha))));
	        partcutLine.AddPoint (tempPoint);
	        tempPoint2 := tempPoint;
			tempPoint := PointAlongPlane (tempPoint2, startDir - alpha + AlphaShift, StDepRang * 2.0);
	        tempPoint2.Free;
	        partCutLine.AddPoint (tempPoint);
	        cutLine.AddPart (partCutLine);

	       	//---checking for existing points in RIGHT polygon AND cutting RIGHT polygon
			tempGeo := GeoOper.intersect (area.RightArea, cutLine);
	        if not IsGeometryEmpty (tempGeo) then
	        begin
				GeoOper.cut (area.RightArea, cutLine, geomLeft, geomRight);
				area.RightArea.Assign (geomLeft);
				geomLeft.Free;
				geomRight.Free;
	        end
	        else
			begin
				isRightPolygonNotIntersect := True;
	        end;
	        tempGeo.Free;
	     
	        //---cutting PRIMARY polygon
			GeoOper.cut (area.PrimaryArea, cutLine, geomLeft, geomRight);
			area.PrimaryArea.Assign (geomLeft);
			geomLeft.Free;
			geomRight.Free;
	    end
	    else
	    begin
	        //---cut line for LEFT polygon
			tempPoint := PointAlongPlane (ptLeft150, startDir + alpha + Pi, StDepRang);
	        partCutLine.AddPoint (tempPoint);
			tempPoint.Free;
			tempPoint := PointAlongPlane (ptLeft150, startDir + alpha, (firstZoneLenght / Cos (alpha)));
	        partCutLine.AddPoint (tempPoint);
			tempPoint2 := tempPoint;
			tempPoint := PointAlongPlane (tempPoint2, startDir + alpha + AlphaShift, StDepRang * 2.0);
	        tempPoint2.Free;
	        partCutLine.AddPoint (tempPoint);
	        cutLine.addPart (partCutLine);
	        
	        //---checking for existing points in LEFT polygon AND cutting LEFT polygon
			tempGeo := GeoOper.intersect (area.LeftArea, cutLine);
	        if not IsGeometryEmpty (tempGeo) then
	        begin
				GeoOper.cut (area.LeftArea, cutLine, geomLeft, geomRight);
				area.LeftArea.Assign (geomRight);
	            geomLeft.Free;
	            geomRight.Free;
	        end
	        else
			begin
				isLeftPolygonNotIntersect := True;
			end;
			tempGeo.Free;

	       	//---cutting PRIMARY polygon
			GeoOper.cut (area.PrimaryArea, cutLine, geomLeft, geomRight);
			area.PrimaryArea.Assign (geomRight);
	        geomLeft.Free;
	        geomRight.Free;

	        cutLine.Clear;
	        partCutLine.Clear;

	       	//---creating cut line for RIGHT polygon
	        tempPoint.Free;
			tempPoint := PointAlongPlane (ptRight150, startDir - alpha + AlphaShift - Pi, StDepRang);
	        partCutLine.AddPoint (tempPoint);
	        tempPoint.Free;
			tempPoint := PointAlongPlane (ptRight150, startDir - alpha + AlphaShift, StDepRang * 2.0);
	        partCutLine.AddPoint (tempPoint);
	        cutLine.addPart (partCutLine);
	        
	       	//---checking for existing points in RIGHT polygon AND cutting RIGHT polygon
			tempGeo := GeoOper.intersect (area.RightArea, cutLine);
	        if not IsGeometryEmpty (tempGeo) then
	        begin
				GeoOper.cut (area.RightArea, cutLine, geomLeft, geomRight);
				area.RightArea.Assign (geomLeft);
	            geomLeft.Free;
	            geomRight.Free;
	        end
	        else
			begin
				isRightPolygonNotIntersect := True;
	        end;
	        tempGeo.Free;
	        
	        //---cutting PRIMARY polygon
			GeoOper.cut (area.PrimaryArea, cutLine, geomLeft, geomRight);
			area.PrimaryArea.Assign (geomLeft);
	        geomLeft.Free;
	        geomRight.Free;
		end;

		if isLeftPolygonNotIntersect then
			area.LeftArea.Ring [0].Clear;
		if isRightPolygonNotIntersect then
			area.RightArea.Ring [0].Clear;

		area.NominalLine.Clear;
		partCutLine.Clear;
		partCutLine.AddPoint (startPoint);
		tempPoint.Free;
		tempPoint := PointAlongPlane (startPoint, startDir + alphaShift, StDepRang);
		partCutLine.AddPoint (tempPoint);
		tempPoint.Free;
		area.NominalLine.AddPart (partCutLine);

		FCL.Clear;
		partCutLine.Clear;
		partCutLine.AddPoint (startPoint);
		tempPoint := PointAlongPlane (startPoint, startDir, StDepRang);
		partCutLine.AddPoint (tempPoint);
		FCL.AddPart (partCutLine);
				
	finally
		tempPoint.Free;
		ptLeft150.Free;
		ptRight150.Free;
		cutLine.Free;
		partCutLine.Free;
	end;
end;

procedure TStraight.CreateArea (shiftDir: Double; distance: Double; createExtededArea: Boolean);
const
	AHPCircleRadius = 56000.0;
var
	range,
	alphaShift:	Double;
	line:      	TLine;
	tmpPoint:	TPoint;
begin
	line := TLine.Create (FParams.StartRwyDirection.Prj, FParams.StartRwyDirection.Direction);
	tmpPoint := CircleVectorIntersect (FParams.AHP.Prj, AHPCircleRadius, line, range);
	line.Free;

	if tmpPoint = nil then
		raise EARANError.Create (
			'Circle with centre point AHP and radius 56 km not intersect with line based RwyDirection points.', mtError);

	range := ReturnDistanceInMeters (tmpPoint, FParams.StartRwyDirection.Prj);
	tmpPoint.Free;

    alphaShift := SubtractAngles (shiftDir, FParams.StartRwyDirection.Direction) *
        Integer (SideFrom2Angle_new (FParams.StartRwyDirection.Direction, shiftDir));

    if createExtededArea then
    begin
		//--- begin: Create exteded area.

		CreateStZoneRNAV (
			FParams.StartRwyDirection.Prj,
			FParams.StartRwyDirection.Direction + alphaShift,
			range,
			distance + deltaDist);

		CutZoneByLine (
			FParams.StartRwyDirection.Prj,
			FParams.StartRwyDirection.Direction,
			alphaShift,
			distance + deltaDist,
            FSegment.Area);

        FSegment.ExtendedArea.Assign (FSegment.Area);            
	    //--- end.
    end;

	CreateStZoneRNAV (
		FParams.StartRwyDirection.Prj,
		FParams.StartRwyDirection.Direction + alphaShift,
		range,
		distance);

	CutZoneByLine (
		FParams.StartRwyDirection.Prj,
		FParams.StartRwyDirection.Direction,
		alphaShift,
		distance,
        FSegment.Area);
end;
       
procedure TStraight.CreateStZoneRNAV (
						startPoint: TPoint;
						startDir,
						range,
						dist: Double);
var
	alpha,
	L1,
	mdlDist,
	widthInDER,
	d,w :			Double;

	Pt0, Pt1, Pt2, Pt3,
	Pt4, Pt5, pt6, pt7,
	ptRng,
	ptcntr1,
	ptcntr2,
	leftArm,
	rightArm:	  	TPoint;

	ringLeft,
	ringRight,
	ringPrim:		TRing;

	__UseConstants__:	Integer;
begin
	w := GConst.GNSS.Constant [IDEP_].SemiWidth;

    pt2 := nil;
    pt3 := nil;
    pt4 := nil;
    pt5 := nil;
    pt6 := nil;
    pt7 := nil;
    ptRng := nil;
    ptcntr1 := nil;

	alpha := DegToRad (15.0);
//	L1 := 14800.0;
	L1 := GConst.GNSS.Constant [DEP_].SemiWidth;
    widthInDER := 150.0;

	pt0 := PointAlongPlane (startPoint, startDir + 0.5*Pi, 0.5 * W);
	pt1 := PointAlongPlane (startPoint, startDir + 0.5*Pi, W);

	leftArm := PointAlongPlane (startPoint, FParams.StartRwyDirection.Direction + 0.5*Pi, widthInDER);
	rightArm := PointAlongPlane (startPoint, FParams.StartRwyDirection.Direction - 0.5*Pi, widthInDER);

	mdlDist := (L1 - W) / Tan (alpha);

	ringLeft := TRing.Create;
	ringRight := TRing.Create;
	ringPrim := TRing.Create;

	ExpandPoints.Clear ();


	if Dist <= range then
	begin
		//---LeftPoly
		Pt2 := PointAlongPlane (Pt1, startDir, Dist);
		Pt3 := PointAlongPlane (Pt0, startDir, Dist);

        ringLeft.AddPoint (pt0);
        ringLeft.AddPoint (pt1);
        ringLeft.AddPoint (pt2);
        ringLeft.AddPoint (pt3);
    
		//---RightPoly
        pt0.Free;
        pt1.Free;
        pt2.Free;
        pt3.Free;

		Pt0 := PointAlongPlane (startPoint, startDir - 0.5*Pi, 0.5 * W);
		Pt1 := PointAlongPlane (startPoint, startDir - 0.5*Pi, W);
		Pt2 := PointAlongPlane (Pt1, startDir, Dist);
		Pt3 := PointAlongPlane (Pt0, startDir, Dist);

        ringRight.AddPoint (pt0);
        ringRight.AddPoint (pt3);
        ringRight.AddPoint (pt2);
        ringRight.AddPoint (pt1);

		//---PrimPoly
        pt0.Free;
        pt1.Free;
        pt2.Free;
        pt3.Free;
                
		Pt0 := PointAlongPlane (startPoint, startDir + Pi/2, 0.5 * W);
		Pt1 := PointAlongPlane (Pt0, startDir, Dist);
		Pt3 := PointAlongPlane (startPoint, startDir - Pi/2, 0.5 * W);
		Pt2 := PointAlongPlane (Pt3, startDir, Dist);

        ringPrim.AddPoint (leftArm);
        ringPrim.AddPoint (pt0);
        ringPrim.AddPoint (pt1);
        ringPrim.AddPoint (pt2);
        ringPrim.AddPoint (pt3);
    	ringPrim.AddPoint (rightArm);
	end
	else
    begin
		if Dist <= range + (L1 - W) / Tan (alpha) then
        begin
	    	//---LeftPoly
            Pt2.Free;
            pt3.Free;

			Pt2 := PointAlongPlane (Pt1, startDir, range);
			Pt3 := PointAlongPlane (Pt2, startDir + alpha, (Dist - range) / Cos(alpha));
			ptRng := PointAlongPlane (startPoint, startDir, Dist);
			d := Sqrt ( Sqr (Pt3.X - ptRng.X) + Sqr (Pt3.Y - ptRng.Y));
			pt4 := PointAlongPlane (Pt3, startDir - 0.5*Pi, 0.5*d);

			ptcntr1 := pt4.Clone.AsPoint;
			pt5 := PointAlongPlane (Pt0, startDir, range);

			ringLeft.AddPoint (pt0);
			ringLeft.AddPoint (pt1);
			ringLeft.AddPoint (pt2);
			ringLeft.AddPoint (pt3);
			ringLeft.AddPoint (pt4);
	        ringLeft.AddPoint (pt5);
        
   			//---RightPoly
            pt0.Free;
            pt1.Free;
            pt2.Free;
            pt3.Free;
            pt4.Free;
			pt5.Free;

			pt0 := PointAlongPlane (startPoint, startDir - 0.5*Pi, 0.5 * W);
			pt1 := PointAlongPlane (startPoint, startDir - 0.5*Pi, W);
			pt2 := PointAlongPlane (pt1, startDir, range);

			pt3 := PointAlongPlane (pt2, startDir - alpha, (Dist - range) / Cos (alpha));

	        d := Sqrt ( Sqr (Pt3.X - ptRng.X) + Sqr (Pt3.Y - ptRng.Y));
			pt4 := PointAlongPlane (pt3, startDir + Pi/2, d / 2);

			ptcntr2 := Pt4.Clone.AsPoint;
			pt5 := PointAlongPlane (pt0, startDir, range);

            ringRight.AddPoint (Pt0);
            ringRight.AddPoint (Pt5);
            ringRight.AddPoint (Pt4);
            ringRight.AddPoint (Pt3);
            ringRight.AddPoint (Pt2);
            ringRight.AddPoint (Pt1);
        
    		//---PrimPoly
            pt0.Free;
            pt1.Free;
            pt4.Free;
			pt3.Free;

			pt0 := PointAlongPlane (startPoint, startDir + Pi/2, 0.5 * W);
			pt1 := PointAlongPlane (pt0, startDir, range);
			pt4 := PointAlongPlane (startPoint, startDir - Pi/2, 0.5 * W);
			pt3 := PointAlongPlane (pt4, startDir, range);

			ExpandPoints.AddPoint (pt1);
			ExpandPoints.AddPoint (pt3);

			ringPrim.AddPoint (leftArm);
            ringPrim.AddPoint (Pt0);
            ringPrim.AddPoint (Pt1);
            ringPrim.AddPoint (ptcntr1);
            ringPrim.AddPoint (ptcntr2);
            ringPrim.AddPoint (Pt3);
            ringPrim.AddPoint (Pt4);
            ringPrim.AddPoint (rightArm);
    	end
		else
		begin
			
			//---LeftPoly
			pt2.Free;
			pt3.Free;
			pt4.Free;
			ptRng.Free;
			ptcntr1.Free;
			pt5.Free;

			pt2 := PointAlongPlane (pt1, startDir, range);
			pt3 := PointAlongPlane (pt2, startDir + alpha, (L1 - W) / Sin (alpha));
			pt4 := PointAlongPlane (pt3, startDir, Dist - range - mdlDist);
			pt7 := PointAlongPlane (pt0, startDir, range);
			ptRng := PointAlongPlane (startPoint, startDir, range + mdlDist);
			d := Sqrt ( Sqr(pt3.X - ptRng.X) + Sqr (pt3.Y - ptRng.Y));
			pt6 := PointAlongPlane (pt3, startDir - 0.5*Pi, 0.5*d);

			ptcntr1 := pt6.Clone.AsPoint;
			pt5 := PointAlongPlane (pt6, startDir, Dist - range - mdlDist);

            ringLeft.AddPoint (Pt0);
            ringLeft.AddPoint (Pt1);
            ringLeft.AddPoint (Pt2);
            ringLeft.AddPoint (Pt3);
            ringLeft.AddPoint (Pt4);
            ringLeft.AddPoint (Pt5);
            ringLeft.AddPoint (pt6);
            ringLeft.AddPoint (pt7);

    		//---RightPoly
            pt0.Free;
            pt1.Free;
            pt2.Free;
            pt3.Free;
            pt4.Free;
            pt7.Free;
            pt6.Free;
            //ptcntr2.Free;
            pt5.Free;

			pt0 := PointAlongPlane (startPoint, startDir - Pi/2, 0.5 * W);
			pt1 := PointAlongPlane (startPoint, startDir - Pi/2, W);
			pt2 := PointAlongPlane (pt1, startDir, range);
			pt3 := PointAlongPlane (pt2, startDir - alpha, (L1 - W) / Sin(alpha));
			pt4 := PointAlongPlane (pt3, startDir, Dist - range - mdlDist);
			pt7 := PointAlongPlane (pt0, startDir, range);

        	d := Sqrt ( Sqr (Pt3.X - ptRng.X) + Sqr (Pt3.Y - ptRng.Y));
			pt6 := PointAlongPlane (pt3, startDir + Pi/2, d / 2);

			ptcntr2 := pt6.Clone.AsPoint;
			Pt5 := PointAlongPlane (pt6, startDir, Dist - range - mdlDist);

            ringRight.AddPoint (Pt0);
            ringRight.AddPoint (pt7);
            ringRight.AddPoint (pt6);
            ringRight.AddPoint (Pt5);
            ringRight.AddPoint (Pt4);
            ringRight.AddPoint (Pt3);
            ringRight.AddPoint (Pt2);
            ringRight.AddPoint (Pt1);
        
    		//---PrimePoly
        	pt0.Free;
        	pt1.Free;
            pt2.Free;
            pt3.Free;
            pt7.Free;
			pt6.Free;
			pt5.Free;
			pt4.Free;

			pt0 := PointAlongPlane (startPoint, startDir + 0.5*Pi, 0.5 * W);
			pt1 := PointAlongPlane (pt0, startDir, range);

			ExpandPoints.AddPoint (pt1);

			pt2 := ptcntr1;
			pt3 := PointAlongPlane (pt2, startDir, Dist - range - mdlDist);

			pt7 := PointAlongPlane (startPoint, startDir - 0.5*Pi, 0.5 * W);
			pt6 := PointAlongPlane (pt7, startDir, range);

			ExpandPoints.AddPoint (pt6);

			pt5 := ptcntr2;
			pt4 := PointAlongPlane (Pt5, startDir, Dist - range - mdlDist);

            ringPrim.AddPoint (leftArm);
            ringPrim.AddPoint (Pt0);
            ringPrim.AddPoint (Pt1);
            ringPrim.AddPoint (Pt2);
            ringPrim.AddPoint (Pt3);
            ringPrim.AddPoint (Pt4);
			ringPrim.AddPoint (Pt5);
            ringPrim.AddPoint (pt6);
            ringPrim.AddPoint (pt7);
            ringPrim.AddPoint (rightArm);
    	end;
	end;
		
	FSegment.Area.LeftArea.Clear;
	FSegment.Area.LeftArea.AddRing (ringLeft);
	FSegment.Area.PrimaryArea.Clear;
	FSegment.Area.PrimaryArea.AddRing (ringPrim);
	FSegment.Area.RightArea.Clear;
	FSegment.Area.RightArea.AddRing (ringRight);
    
    ringLeft.Free;
    ringRight.Free;
    ringPrim.Free;  

    pt0.Free;
    pt1.Free;
    pt2.Free;
    pt3.Free;
    pt4.Free;
    pt5.Free;
    pt6.Free;
    pt7.Free;
    leftArm.Free;
    rightArm.Free;
	ptRng.Free;
end;

procedure TStraight.FillObstaclesInStraightArea (
            nominalRadianDir, userInputPDG: Double; area: TProtectedArea);
var
	i:					Integer;
	obsInArea:			TObstacleInArea;
	maxRequiredGrd,
	d1, d2, rH:			Double;
	isInside:			Boolean;
	cutLine:			TPolyline;
	part:				TPart;
    sortPoint,
	point:				TPoint;
    multiPoint:         TMultiPoint;
	polyLeftRight:		TPolygon;
    geom:               TGeometry;
begin
	if FParams.Obstacles.Count = 0 then
		exit;

	cutLine := TPolyline.Create;
	part := TPart.Create;
	point := TPoint.Create (0.0, 0.0);

	maxRequiredGrd := GConst.PANSOPS.Constant [dpPDG_Nom].Value;
	FMaxRequiredGrdIndex := -1;

	part.AddPoint (point);
	part.AddPoint (point);
	point.Free;
	cutLine.AddPart (part);

	FObstacles.Clear;

	obsInArea := TObstacleInArea.Create;

	for i:=0 to FParams.Obstacles.Count - 1 do
	begin
		if SideDef (FParams.StartRwyDirection.Prj, nominalRadianDir + Pi/2,
                FParams.Obstacles.Item [i].AsObstacle.Prj) = sideLeft then
			continue;

		isInside := True;

		if IsPointInPoly (FParams.Obstacles.Item [i].AsObstacle.Prj, area.PrimaryArea) then
		begin
			obsInArea.Assign (FParams.Obstacles.Item [i]);
			obsInArea.WhichArea := waPrimary;
		end
		else if IsPointInPoly (FParams.Obstacles.Item [i].AsObstacle.Prj, area.LeftArea) then
		begin
			obsInArea.Assign (FParams.Obstacles.Item [i]);
			obsInArea.WhichArea := waLeft;
		end
		else if IsPointInPoly (FParams.Obstacles.Item [i].AsObstacle.Prj, area.RightArea) then
		begin
			obsInArea.Assign (FParams.Obstacles.Item [i]);
			obsInArea.WhichArea := waRight;
        end
		else
		begin
			isInside := False;
		end;

		if isInside then
		begin

			obsInArea.TransformX := Point2LineDistancePrj (obsInArea.Prj, FParams.StartRwyDirection.Prj, nominalRadianDir + Pi/2) - obsInArea.GeoAccuracy;
			obsInArea.TransformY := Point2LineDistancePrj (obsInArea.Prj, FParams.StartRwyDirection.Prj, nominalRadianDir) - obsInArea.GeoAccuracy;

			polyLeftRight := nil;

			if obsInArea.WhichArea = waLeft then
				polyLeftRight := area.LeftArea
			else if obsInArea.WhichArea = waRight then
				polyLeftRight := area.RightArea
			else
				obsInArea.MOCCoefficient := 1;

			if polyLeftRight <> nil then
			begin
				sortPoint := PointAlongPlane (obsInArea.Prj, nominalRadianDir + 0.5*Pi, 15000.0);
				cutLine.Part [0].Point [0].Assign (sortPoint);
				point := PointAlongPlane (obsInArea.Prj, nominalRadianDir - 0.5*Pi, 15000.0);
				cutLine.Part [0].Point [1].Assign (point);
				point.Free;

                geom := GeoOper.intersect (polyLeftRight, cutLine);

                if IsGeometryEmpty (geom) or (geom.AsPolyline.Part [0].Count < 2) then
                    continue;

                multiPoint := SortPoints (sortPoint, geom.AsPolyline.Part [0].AsMultiPoint);

				d1 := Point2LineDistancePrj (multiPoint.Point [0], FParams.StartRwyDirection.Prj, nominalRadianDir);
				d2 := Point2LineDistancePrj (multiPoint.Point [1], FParams.StartRwyDirection.Prj, nominalRadianDir);

                geom.Free;
                multiPoint.Free;
                sortPoint.Free;

				if d1 < d2 then
					obsInArea.MOCCoefficient := (d2 - obsInArea.TransformY) / (d2 - d1)
				else
					obsInArea.MOCCoefficient := (d1 - obsInArea.TransformY) / (d1 - d2);
			end;

			obsInArea.AboveDER := obsInArea.ElevationWithAccuracy - FParams.StartRwyDirection.elevTdz;
			obsInArea.MOC := GConst.PANSOPS.Constant [dpMOC].Value * obsInArea.TransformX * obsInArea.MOCCoefficient;
			rH := Ceil (obsInArea.AboveDER + obsInArea.MOC - GConst.PANSOPS.Constant [dpOIS_abv_DER].Value);
			obsInArea.PdgToTop := (rH - obsInArea.MOC) / obsInArea.TransformX;
			obsInArea.RequiredGrd := obsInArea.PdgToTop + GConst.PANSOPS.Constant [dpMOC].Value; //0.008
			obsInArea.Pdg := rH / obsInArea.TransformX;
			obsInArea.Ignored := (rH + GConst.PANSOPS.Constant [dpOIS_abv_DER].Value <= GConst.PANSOPS.Constant [dpPDG_60].Value);
            obsInArea.HPenetration := rH - (userInputPDG * obsInArea.TransformX);
			obsInArea.RequiredH := obsInArea.MOC + obsInArea.AboveDER;
            if obsInArea.AboveDER < (GConst.PANSOPS.Constant [dpGui_Ar1].Value - GConst.PANSOPS.Constant [dpObsClr].Value) then
                obsInArea.RequiredTNH := GConst.PANSOPS.Constant [dpGui_Ar1].Value
            else
                obsInArea.RequiredTNH := obsInArea.AboveDER + GConst.PANSOPS.Constant [dpObsClr].Value;

			if not obsInArea.Ignored then
			begin
				if maxRequiredGrd < obsInArea.RequiredGrd then
				begin
					maxRequiredGrd := obsInArea.RequiredGrd;
					FMaxRequiredGrdIndex := FObstacles.Count;
				end;
			end;

			FObstacles.Add (obsInArea);
		end;
	end;

//	FillObstaclesRequiredTNH ();
	FillObstaclesDistPdg (userInputPDG);


	part.Free;
	cutLine.Free;
	obsInArea.Free;
end;

procedure TStraight.FillObstaclesDistPdg (userInputPDG: Double);
var
	i:				Integer;
	vMaxDistPdg:     Double;
	vMaxRequiredGrd: Double;
	rH:				Double;
	obs:			TObstacleInArea;
begin
	vMaxDistPdg := 0;//dPDGNomHeight;
	FMaxDistPdgIndex := -1;

	if FMaxRequiredGrdIndex = -1 then
		exit;
	vMaxRequiredGrd := FObstacles.Item [FMaxRequiredGrdIndex].RequiredGrd;
    if vMaxRequiredGrd > userInputPDG then
        exit;

	for i:=0 to FObstacles.Count - 1 do
	begin
		obs := FObstacles.Item [i];

		if not obs.Ignored then
		begin
            rH := obs.AboveDER + obs.MOC - GConst.PANSOPS.Constant [dpOIS_abv_DER].Value;
			obs.DistPDG := (rh - GConst.PANSOPS.Constant [dpPDG_Nom].Value * obs.TransformX) /
								(userInputPDG - GConst.PANSOPS.Constant [dpPDG_Nom].Value);

			if vMaxDistPdg < obs.DistPDG then
			begin
				vMaxDistPdg := obs.DistPDG;
				FMaxDistPdgIndex := i;
			end;
		end;
	end;
end;


procedure TStraight.FillObstaclesRequiredTNH_PANDA ();
var
	i:					Integer;
	maxRequiredGrd,
	x, temp,
	TNIA_MOC_Bound:		Double;
	obs:				TObstacleInArea;
begin
	if (FObstacles.Count = 0) or
		(FMaxRequiredGrdIndex = -1) then exit;

	TNIA_MOC_Bound := GConst.PANSOPS.Constant [dpObsClr].Value / GConst.PANSOPS.Constant [dpMOC].Value;
	maxRequiredGrd := FObstacles.Item [FMaxRequiredGrdIndex].RequiredGrd;

	for i := 0 to FObstacles.Count - 1 do
	begin
		obs := FObstacles.Item [i];

		if obs.TransformX > TNIA_MOC_Bound then
		begin
			obs.RequiredTNH := GConst.PANSOPS.Constant [dpNGui_Ar1].Value;
		end
		else
		begin
			if maxRequiredGrd = GConst.PANSOPS.Constant [dpPDG_Nom].Value then
			begin
				temp := obs.AboveDER + GConst.PANSOPS.Constant [dpObsClr].Value;
				if (GConst.PANSOPS.Constant [dpOIS_abv_DER].Value + maxRequiredGrd * obs.TransformX) >= temp then
					temp := 0;
			end
			else
			begin
				x := (GConst.PANSOPS.Constant [dpObsClr].Value -
						GConst.PANSOPS.Constant [dpOIS_abv_DER].Value + obs.AboveDER -
						GConst.PANSOPS.Constant [dpPDG_Nom].Value * obs.TransformX) /
						(maxRequiredGrd - GConst.PANSOPS.Constant [dpPDG_Nom].Value);

				if x >= obs.TransformX then
					temp := obs.AboveDER + GConst.PANSOPS.Constant [dpObsClr].Value
				else
					temp := GConst.PANSOPS.Constant [dpOIS_abv_DER].Value + maxRequiredGrd * x;
			end;

			if temp < GConst.PANSOPS.Constant [dpNGui_Ar1].Value then
				obs.RequiredTNH := GConst.PANSOPS.Constant [dpNGui_Ar1].Value
			else
				obs.RequiredTNH := temp;
		end;
	end;
end;

//-------------------------------
//--- TTurn
//-------------------------------

constructor TTurn.Create (AParams: TParameters; AStraight: TStraight);
begin
	inherited Create;

	FAddToleranceRectHandle := -1;
	FPrimaryToleranceRectHandle := -1;
	FStartPointHandle := -1;

	FAddToleranceSymbol := TFillSymbol.Create;
	FAddToleranceSymbol.Style := sfsCross;
	FAddToleranceSymbol.Color := RGB (20, 200, 20);
	FAddToleranceSymbol.Outline.Color := RGB (20, 200, 20);

	FPrimaryToleranceSymbol := TFillSymbol.Create;
	FPrimaryToleranceSymbol.Style := sfsCross;
	FPrimaryToleranceSymbol.Color := RGB (20, 20, 200);
	FPrimaryToleranceSymbol.Outline.Color := RGB (20, 20, 200);

	FTurnInfo := TTurnInfo.Create ();
	FRwyInfo := TTurnInfo.Create ();
	FTurnInfoList := TTurnInfoList.Create ();
	FParams := AParams;
	FStraight := AStraight;
	FSegmentSymbol := TSegmentSymbol.Create (FParams.UI);
end;

destructor TTurn.Destroy;
begin
	FAddToleranceSymbol.Free;
	FPrimaryToleranceSymbol.Free;

	DeleteGraphic (FAddToleranceRectHandle);
	DeleteGraphic (FPrimaryToleranceRectHandle);
	DeleteGraphic (FStartPointHandle);

	FTurnInfo.Free;
	FRwyInfo.Free;
	FTurnInfoList.Free;
	FSegmentSymbol.Free;
	inherited;
end;

function TTurn.GetDFSegmentInner (startPoint: TPoint; innerNominalDir, innerNextWidth, ATTValue: Double): TMultiPoint;
var
	turnVal:	Integer;
	tmpPoint,
	tmpPoint2,
	tmpPoint3:	TPoint;
	side:		TSideDirection;
begin
	turnVal := Integer (FTurnInfo.TurnSide);

	tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, innerNominalDir - 0.5*Pi*turnVal, innerNextWidth);
	tmpPoint2 := PointAlongPlane (tmpPoint, innerNominalDir, -ATTValue);
	tmpPoint3 := LineLineIntersect (startPoint, innerNominalDir - (Pi/12)*turnVal, tmpPoint2, innerNominalDir).AsPoint;

	side := SideDef (tmpPoint2, innerNominalDir + 0.5*Pi, tmpPoint3);

	result := TMultiPoint.Create;
	result.AddPoint (startPoint);

	if side = sideRight then
		result.AddPoint (tmpPoint2)
	else
		result.AddPoint (tmpPoint3);

	result.AddPoint (tmpPoint);

	tmpPoint3.Free;
	tmpPoint2.Free;
	tmpPoint.Free;
end;

procedure TTurn.CreateSegmentDF (ATTValue: Double);
const
	widthInDER = 150.0;
var
	i, startIndex,
	endIndex,
	addAreaIndexOnPrimOuter,
	spiralIntIndexPrimOut,
	spiralIntIndexAddOut,
	turnVal:			Integer;
	radius, dir,
	spiralEndDir,
	innerNominalDir,
	baseDir:			Double;
	basePoints,
	primOutPoints,
	addOutPoints,
	primInnPoints,
	addInnPoints,
	innerStartPoints: 	TMultiPoint;
	side:				TSideDirection;
	earlestNominalPart,
	latestNominalPart:	TPart;
	tmpPoint,
	tmpPoint2,
	innerNomStartPt:	TPoint;
	nextPointPosition:	Integer; // must be 0, 1 or 2.
	outerArea,
	primaryArea,
	innerArea:			TPolygon;
	tmpRing:			TRing;
	tmpLine,
	addOutLine,
	primOutLine,
	primInnLine,
	addInnLine:			TPolyline;
	tmpPart:            TPart;
begin

	tmpLine := TPolyline.Create;
	tmpPart := TPart.Create;
	tmpLine.AddPart (tmpPart);
	//tmpPart.Free;

	turnVal := Integer (FTurnInfo.TurnSide);

	outerArea := TPolygon.Create;
	primaryArea := TPolygon.Create;
	innerArea := TPolygon.Create;
	tmpRing := TRing.Create;
	outerArea.AddRing (tmpRing);
	primaryArea.AddRing (tmpRing);
	innerArea.AddRing (tmpRing);
	tmpRing.Free;

	addOutLine := TPolyline.Create;
	primOutLine := TPolyline.Create;
	primInnLine := TPolyline.Create;
	addInnLine := TPolyline.Create;
	//tmpPart := TPart.Create;
	addOutLine.AddPart (tmpPart);
	primOutLine.AddPart (tmpPart);
	primInnLine.AddPart (tmpPart);
	addInnLine.AddPart (tmpPart);
	tmpPart.Free;

	//--- begin: Fill base points.
	basePoints := TMultiPoint.Create;
	basePoints.AddPoint (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0]);
	basePoints.AddPoint (FTurnInfo.Start.LatestTolerance.PrimaryPoint [2]);
	basePoints.AddPoint (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2]);
	basePoints.AddPoint (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0]);
	//--- end.

	spiralEndDir := GNullRadianValue;

	latestNominalPart := TPart.Create;

	primOutPoints := GetOuterSpiralDF (FTurnInfo.ProtectedRadius,
						FTurnInfo.Next.Width, ATTValue,
						spiralEndDir, basePoints, latestNominalPart,
						spiralIntIndexPrimOut);

	dir := ReturnAngleInRadians (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2],
			FTurnInfo.Next.Point);

	earlestNominalPart := TPart.Create;

	//--- future must change this. Checked modulus by 90 and substract with sign.
	nextPointPosition := 0;

	if Modulus (SubtractAnglesWithSign (FTurnInfo.Start.Direction,
		dir, TSideDirection (-turnVal)), 2*Pi) >= (5*Pi/12) then
	begin
		dir := ReturnAngleInRadians (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0],
					FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0]) - turnVal * Pi/12;
		side := SideDef (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0], dir, FTurnInfo.Next.Point);

		if not IsSameSide (side, FTurnInfo.TurnSide) then
			nextPointPosition := 1
		else
			nextPointPosition := 2;
	end;

	if nextPointPosition = 0 then
		innerNomStartPt := FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2]
	else if nextPointPosition = 1 then
		innerNomStartPt := FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0]
	else
		innerNomStartPt := FTurnInfo.Start.LatestTolerance.PrimaryPoint [0];

	earlestNominalPart.AddPoint (innerNomStartPt);
	earlestNominalPart.AddPoint (FTurnInfo.Next.Point);
	FTurnInfo.Next.EarlestDirection := ReturnAngleInRadians (
        earlestNominalPart.Point [0], earlestNominalPart.Point [1]);
	innerNominalDir := FTurnInfo.Next.EarlestDirection;

	radius := FTurnInfo.ProtectedRadius;

	if FTurnInfo.Start.LatestTolerance.HasOuter then
	begin
		baseDir := ReturnAngleInRadians (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0],
						FTurnInfo.Start.LatestTolerance.PrimaryPoint [1]);

        basePoints.Point [0].Assign (FTurnInfo.Start.LatestTolerance.OuterPoint);
		tmpPoint := PointAlongPlane (FTurnInfo.Start.LatestTolerance.PrimaryPoint [2],
						baseDir, -FTurnInfo.Start.LatestTolerance.OuterWidth);
        basePoints.Point [1].Assign (tmpPoint);
		tmpPoint.Free;
		tmpPoint := PointAlongPlane (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2],
						baseDir, -FTurnInfo.Start.LatestTolerance.OuterWidth);
        basePoints.Point [2].Assign (tmpPoint);

		tmpPoint.Free;
		basePoints.Point [3].Assign (FTurnInfo.Start.EarlestTolerance.OuterPoint);

		radius := FTurnInfo.ProtectedRadius + FTurnInfo.Start.LatestTolerance.OuterWidth;
	end;

	addOutPoints := GetOuterSpiralDF (radius,
								2 * FTurnInfo.Next.Width,
								ATTValue, spiralEndDir, basePoints, nil,
								spiralIntIndexAddOut);

	//--- begin: Create addition area.
	if FTurnInfo.Start.LatestTolerance.OuterWidth > 0 then
	begin
		startIndex := 0;
		endIndex := 0;
	end
	else
	begin
		startIndex := spiralIntIndexAddOut;
		endIndex := spiralIntIndexPrimOut;
	end;
	
	for i:=startIndex to addOutPoints.Count - 1 do
	begin
		outerArea.Ring [0].AddPoint (addOutPoints.Point [i]);
		addOutLine.Part [0].AddPoint (addOutPoints.Point [i]);
	end;
	for i:=primOutPoints.Count - 1 downto endIndex do
		outerArea.Ring [0].AddPoint (primOutPoints.Point [i]);
	//--- end.

	innerStartPoints := TMultiPoint.Create;

	if nextPointPosition = 0 then
	begin
		innerStartPoints.AddPoint (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2]);
		innerStartPoints.AddPoint (FTurnInfo.Start.EarlestTolerance.InnerPoint)
	end
	else if nextPointPosition = 1 then
	begin
		innerStartPoints.AddPoint (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0]);
		innerStartPoints.AddPoint (FTurnInfo.Start.EarlestTolerance.OuterPoint)
	end
	else
	begin
		innerStartPoints.AddPoint (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0]);
		innerStartPoints.AddPoint (FTurnInfo.Start.LatestTolerance.OuterPoint)
	end;

	FTurnInfo.InnerStartPoint.Assign (innerStartPoints.Point [1]);

	primInnPoints := GetDFSegmentInner (innerStartPoints.Point [0], innerNominalDir, FTurnInfo.Next.Width, ATTValue);
	addInnPoints := GetDFSegmentInner (innerStartPoints.Point [1], innerNominalDir, 2 * FTurnInfo.Next.Width, ATTValue);

	//--- begin: Create primary area.
	//--- --- begin: Add straight area extend point. ExpandPoints [0] is left and [1] is right.
	if (FTurnInfo.TurnType = tuAtHeight) and (FStraight.ExpandPoints.Count > 0) then
		primaryArea.Ring [0].AddPoint (FStraight.ExpandPoints.Point [Round (0.5*turnVal - 0.5) * -1]);
	//--- --- end.	primaryArea.Ring [0].AddMultiPoint (primOutPoints);
	primaryArea.Ring [0].AddMultiPoint (primOutPoints);
	primaryArea.Ring [0].AddPoint (FTurnInfo.Next.Point);
	primOutLine.Part [0].AddMultiPoint (primOutPoints);
	primaryArea.Ring [0].AddMultiPointInverse (primInnPoints);
	primInnLine.Part [0].AddMultiPointInverse (primInnPoints);
	//--- end.

	//--- begin: Create inner area.
	if FTurnInfo.Start.EarlestTolerance.InnerWidth > 0 then
	begin
		startIndex := 0;
		endIndex := 0;
	end
	else
	begin
		startIndex := 1;
		endIndex := 1;
	end;

	for i:=startIndex to primInnPoints.Count - 1 do
		innerArea.Ring [0].AddPoint (primInnPoints.Point [i]);
	for i:=addInnPoints.Count - 1 downto endIndex do
	begin
		innerArea.Ring [0].AddPoint (addInnPoints.Point [i]);
		addInnLine.Part [0].AddPoint (addInnPoints.Point [i]);
	end;
	//--- end.

	if FTurnInfo.TurnSide = sideRight then
	begin
		FTurnInfo.Segment.LinesForMOCCoef.Part [0].Assign (addOutLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [1].Assign (primOutLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [2].Assign (primInnLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [3].Assign (addInnLine.Part [0]);

		FTurnInfo.Segment.Area.LeftArea.Assign (outerArea);
		FTurnInfo.Segment.Area.RightArea.Assign (innerArea);
	end
	else
	begin
		FTurnInfo.Segment.LinesForMOCCoef.Part [0].Assign (addInnLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [1].Assign (primInnLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [2].Assign (primOutLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [3].Assign (addOutLine.Part [0]);

		FTurnInfo.Segment.Area.LeftArea.Assign (innerArea);
		FTurnInfo.Segment.Area.RightArea.Assign (outerArea);
	end;

	FTurnInfo.Segment.Area.PrimaryArea.Assign (primaryArea);

	tmpLine.Free;
	
	outerArea.Free;
	primaryArea.Free;
	innerArea.Free;

	addOutLine.Free;
	primOutLine.Free;
	primInnLine.Free;
	addInnLine.Free;

	basePoints.Free;

	primOutPoints.Free;
	addOutPoints.Free;
	primInnPoints.Free;
	addInnPoints.Free;
end;

function TTurn.CalcVerHorTolerance (var L1, L2, R2, alpha: Double): Double;
const
	bankTime = 3.3;
	pilotTime = 3.3;
	AHPCircleRadius = 56000.0;
	B2 = 15 * Pi / 180;
var
	constIndex:     TFIXRole;
	tmpAlpha,
	minDist,
	R1, L3, L4,
	oldL4, L5,
	H, TAS:	        Double;
    tmpPoint:       TPoint;
begin
	alpha := DegToRad (30.0);

	H := FTurnInfo.PrevTurnInfo.Start.HeightToKK +
		    FTurnInfo.Start.Distance * FTurnInfo.PDGProtect;
	TAS := IASToTAS (FTurnInfo.IAS, H, FParams.AHP.Temperature);

    if FParams.SensorType = stGNSS then
    begin
		if ReturnDistanceInMeters(FParams.AHP.Prj, FTurnInfo.Start.Point) < AHPCircleRadius then constIndex := IDEP_
		else constIndex := DEP_;

		FTurnInfo.Start.ATT := GConst.GNSS.Constant [constIndex].ATT;
		FTurnInfo.Start.XTT := GConst.GNSS.Constant [constIndex].XTT;

		if ReturnDistanceInMeters(FParams.AHP.Prj, FTurnInfo.Next.Point) < AHPCircleRadius then constIndex := IDEP_
		else constIndex := DEP_;

		FTurnInfo.Next.ATT := GConst.GNSS.Constant [constIndex].ATT;
		FNextXTT_tmp := GConst.GNSS.Constant [constIndex].XTT;
    end;

	if (FTurnInfo.TurnType <> tuAtHeight) and (FTurnInfo.CodeType = ctDF) then
	begin
		minDist := 2 * FTurnInfo.ProtectedRadius;
		FTurnInfo.Start.ETP := FTurnInfo.Start.ATT;
		FTurnInfo.Start.LTP := FTurnInfo.Start.ATT + TAS * (pilotTime + bankTime);
	end
	else
	begin
		R1 := FTurnInfo.ProtectedRadius;

		if FTurnInfo.TurnType = tuFlyOver then
		begin
			R2 := BankToRadius (B2, TAS);
			L1 := R1 * Sin (FTurnInfo.TurnAngle);

			L2 := R1 * Cos (FTurnInfo.TurnAngle) * Tan (alpha);
			L3 := R1 * (1.0 - Cos(FTurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha);
			oldL4 := R2 * Tan (0.5 * alpha);
			L4 := oldL4;

			If L3 * Cos (alpha) < L4 Then
			begin
				tmpAlpha := RadToDeg (ArcCos ((R2 + R1 * Cos (FTurnInfo.TurnAngle)) / (R1 + R2)));
				alpha := DegToRad ((AdvancedRound (tmpAlpha, 0, rtFloor)));
				L2 := R1 * Cos (FTurnInfo.TurnAngle) * Tan (alpha);
				if alpha <> 0 then
					L3 := R1 * (1.0 - Cos(FTurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha)
				else
				begin
					alpha := DegToRad (tmpAlpha);
					L3 := R1 * (1.0 - Cos(FTurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha)
				end;
			end;

			L5 := bankTime * TAS;
			minDist := L1 + L2 + L3 + oldL4 + L5;

			FTurnInfo.Start.ETP := FTurnInfo.Start.ATT;
			FTurnInfo.Start.LTP := FTurnInfo.Start.ATT + TAS * (pilotTime + bankTime);
		end
		else if FTurnInfo.TurnType = tuFlyBy then
		begin
			L1 := R1 * Tan(0.5 * FTurnInfo.TurnAngle);
			L2 := bankTime * TAS;
			minDist := L1 + L2;

			FTurnInfo.Start.ETP := L1 + FTurnInfo.Start.ATT;
			FTurnInfo.Start.LTP := -(Min (L1, R1) - FTurnInfo.Start.ATT - pilotTime * TAS);
		end
		else //--- FTurnInfo.TurnType = tuAtHeight
		begin
			R2 := BankToRadius (B2, TAS);
            tmpPoint := PointAlongPlane (FParams.PairRwyDirection.Prj,
                FParams.StartRwyDirection.Direction, 600.0);
			FTurnInfo.Start.ETP := ReturnDistanceInMeters (FTurnInfo.Start.Point, tmpPoint);
			FTurnInfo.Start.LTP := TAS * (pilotTime + bankTime);
            tmpPoint.Free;
            minDist := 2 * FTurnInfo.ProtectedRadius;
		end;
	end;

	result := minDist;
end;

function TTurn.CreateCutterLineBaseToleranceLine (tolLine: TToleranceLineInfo): TPolyline;
var
    line:   TPolyline;
    part:   TPart;
    point:  TPoint;
    dir,
    dist:   Double;
begin
    line := TPolyline.Create;
    part := TPart.Create;

    dist := tolLine.PrimOuterWidth * 4;
    dir := ReturnAngleInRadians (tolLine.PrimaryPoint [1], tolLine.PointList.Point [0] {Left Point} );
    point := PointAlongPlane (tolLine.PointList.Point [0], dir, dist);
    part.AddPoint (point);
    point.Free;
    part.AddMultiPoint (tolLine.PointList);
    dir := ReturnAngleInRadians (tolLine.PrimaryPoint [1], tolLine.PointList.Point [4] {Right Point});
    point := PointAlongPlane (tolLine.PointList.Point [4], dir, dist);
    part.AddPoint (point);
    point.Free;

    line.AddPart (part);
    part.Free;
    result := line;
end;

function TTurn.CutAreaByToleranceLine (polygon: TPolygon; tolereanceLine: TToleranceLineInfo; getSideGeom: TSideDirection; var cutLine: TPolyline): TPolygon;
var
	part:				TPart;
	dir, w:				Double;
	toleranceCentrePoint,
	tmpPoint:			TPoint;
	resultGeomLeft,
	resultGeomRight:	TGeometry;
begin
	if cutLine = nil then
	begin
		cutLine := TPolyline.Create;
		part := TPart.Create;
		cutLine.AddPart (part);
		part.Free;

		w := ReturnDistanceInMeters (tolereanceLine.PointList.Point [2],
						tolereanceLine.PointList.Point [3]);
		dir := ReturnAngleInRadians (tolereanceLine.PointList.Point [2],
						tolereanceLine.PointList.Point [3]);

		toleranceCentrePoint := tolereanceLine.PointList.Point [2];
		tmpPoint := PointAlongPlane (toleranceCentrePoint, dir + 0.5 * Pi, 150);
		toleranceCentrePoint := tmpPoint;

		tmpPoint := PointAlongPlane (toleranceCentrePoint, dir, w * -4);
		cutLine.Part [0].AddPoint (tmpPoint);
		tmpPoint.Free;
		tmpPoint := PointAlongPlane (toleranceCentrePoint, dir, w * 4);
		cutLine.Part [0].AddPoint (tmpPoint);
		tmpPoint.Free;
		toleranceCentrePoint.Free;
	end;

	GeoOper.Cut (polygon, cutLine, resultGeomLeft, resultGeomRight);
	if getSideGeom = sideRight then
	begin
		result := resultGeomRight.AsPolygon;
		resultGeomLeft.Free;
	end
	else
	begin
		result := resultGeomLeft.AsPolygon;
		resultGeomRight.Free
	end;
end;

procedure TTurn.CalcMinPDG (calculateStraight: Boolean);
var
	i:                  Integer;
	addOutLine,
	primOutLine,
    kkLine,

	linesForMOCCoef0,
	linesForMOCCoef1,
	linesForMOCCoef2,
	linesForMOCCoef3:	TPolyline;
	tmpPart:		    TPart;
	dr, d0,	outDist,
	inDist,
    hTIA,
	maxRequiredGrd: 	Double;
	obstacle:			TObstacle;
	obsInArea:			TObstacleInArea;
	isInside:			Boolean;
	tmpPoint:	        TPoint;
    areaToKK:           TProtectedArea;
	straightAreaPoly:   TPolygon;
    geom:               TGeometry;
begin
	dr := 0;
	straightAreaPoly := nil;
	kkLine := nil;
	addOutLine := nil;
	primOutLine := nil;

    if calculateStraight then
    begin
        FStraight.FillObstaclesInStraightArea (FTurnInfo.Start.Direction,
            FTurnInfo.PrevTurnInfo.PDG, FTurnInfo.PrevTurnInfo.Segment.AreaToKK);
    end;

	FTurnInfo.MaxRequiredGrdIndex := -1;
	maxRequiredGrd := GNullDataValue;

	linesForMOCCoef0 := TPolyline.Create;
	linesForMOCCoef1 := TPolyline.Create;
	linesForMOCCoef2 := TPolyline.Create;
	linesForMOCCoef3 := TPolyline.Create;

	linesForMOCCoef0.AddPart (FTurnInfo.Segment.LinesForMOCCoef.Part [0]);
	linesForMOCCoef1.AddPart (FTurnInfo.Segment.LinesForMOCCoef.Part [1]);
	linesForMOCCoef2.AddPart (FTurnInfo.Segment.LinesForMOCCoef.Part [2]);
	linesForMOCCoef3.AddPart (FTurnInfo.Segment.LinesForMOCCoef.Part [3]);

	if FTurnInfo.TurnType <> tuAtHeight then
	begin
		kkLine := TPolyline.Create;
		tmpPart := TPart.Create;
		tmpPart.AddMultiPoint (FTurnInfo.Start.EarlestTolerance.PointList);
		kkLine.AddPart (tmpPart);
		tmpPart.Free;
		dr := GeoOper.getDistance (kkLine, FStraight.Parameters.FStartRwyDirection.Prj);
        hTIA := dr * FTurnInfo.PrevTurnInfo.PDG + GConst.PANSOPS.Constant [dpOIS_abv_DER].Value;
    end
    else
    begin
        straightAreaPoly := TPolygon.Create;
        areaToKK := FTurnInfo.PrevTurnInfo.Segment.AreaToKK;
        if (not IsGeometryEmpty (areaToKK.LeftArea) ) or (not IsGeometryEmpty (areaToKK.PrimaryArea)) then
        begin
            geom := GeoOper.unionGeometry (areaToKK.LeftArea, areaToKK.PrimaryArea);
            straightAreaPoly.Assign (geom);
            geom.Free;
            geom := GeoOper.unionGeometry (straightAreaPoly, areaToKK.RightArea);
            straightAreaPoly.Assign (geom);
            geom.Free;
        end;

        hTIA := FTurnInfo.Start.HeightToKK;
    end;

	FTurnInfo.Obstacles.Clear;
    obsInArea := TObstacleInArea.Create;

	for i:=0 to FParams.Obstacles.Count - 1 do
	begin
		obstacle := FParams.Obstacles.Item [i].asObstacle;
		isInside := True;

		if IsPointInPoly (obstacle.Prj, FTurnInfo.Segment.Area.PrimaryArea) then
		begin
			obsInArea.Assign (obstacle);
			obsInArea.WhichArea := waPrimary;
		end
		else if IsPointInPoly (obstacle.Prj, FTurnInfo.Segment.Area.LeftArea) then
		begin
			obsInArea.Assign (obstacle);
			obsInArea.WhichArea := waLeft;
		end
		else if IsPointInPoly (obstacle.Prj, FTurnInfo.Segment.Area.RightArea) then
		begin
			obsInArea.Assign (obstacle);
			obsInArea.WhichArea := waRight;
		end
		else
		begin
			isInside := False;
		end;

		if isInside then
		begin
			obsInArea.MOCCoefficient := 0;

			if obsInArea.WhichArea = waLeft then
			begin
				addOutLine := linesForMOCCoef0;
				primOutLine := linesForMOCCoef1;
			end
			else if obsInArea.WhichArea = waRight then
			begin
				addOutLine := linesForMOCCoef3;
				primOutLine := linesForMOCCoef2;
			end
			else
				obsInArea.MOCCoefficient := 1;

			if obsInArea.MOCCoefficient <> 1 then
			begin
				outDist := GeoOper.getDistance (obsInArea.Prj, addOutLine);
				inDist := GeoOper.getDistance (obsInArea.Prj, primOutLine);

				obsInArea.MOCCoefficient := outDist / (inDist + outDist)
			end;

			if FTurnInfo.TurnType = tuAtHeight then
			begin
				tmpPoint := GeoOper.getNearestPoint (straightAreaPoly, obsInArea.Prj);
				d0 := ReturnDistanceInMeters (tmpPoint, obsInArea.Prj);
				dr := PointToLineDistance (tmpPoint, FStraight.Parameters.StartRwyDirection.Prj,
						FTurnInfo.Start.Direction - 0.5*Pi);
				if dr < 0 then
					dr := 0;
				tmpPoint.Free;
			end
			else
				d0 := GeoOper.getDistance (kkLine, obsInArea.Prj);

			obsInArea.TransformX := dr + d0;
			obsInArea.MOC := Max ((dr + d0) * GConst.PANSOPS.Constant [dpMOC].Value, 90) * obsInArea.MOCCoefficient ;
			obsInArea.AboveDER := obsInArea.ElevationWithAccuracy - FParams.StartRwyDirection.elevTdz;
			obsInArea.RequiredH := obsInArea.MOC + obsInArea.AboveDER;
            obsInArea.HPenetration := (obsInarea.AboveDER + obsInArea.MOC) - (hTIA + d0 * FTurnInfo.PDG);
            obsInArea.RequiredGrd := (obsInArea.AboveDER + obsInArea.MOC - hTIA) / d0;
			obsInArea.Dr := dr;

			if maxRequiredGrd < obsInArea.RequiredGrd then
			begin
				maxRequiredGrd := obsInArea.RequiredGrd;
				FTurnInfo.MaxRequiredGrdIndex := FTurnInfo.FObstacles.Count;
			end;

			FTurnInfo.Obstacles.Add (obsInArea);
		end;
	end;

	linesForMOCCoef0.Free;
	linesForMOCCoef1.Free;
	linesForMOCCoef2.Free;
	linesForMOCCoef3.Free;

    kkLine.Free;
	straightAreaPoly.Free;
    obsInArea.Free;
end; 

procedure TTurn.DrawStraightArea ();
begin
	FSegmentSymbol.EraseArea ();

	DeleteGraphic (FAddToleranceRectHandle);
	DeleteGraphic (FPrimaryToleranceRectHandle);
	DeleteGraphic (FStartPointHandle);


	FSegmentSymbol.DrawArea (FStraight.Segment.Area, nil, nil);
end;

procedure TTurn.DrawAreaAtHeight ();
var
    leftArea,
    primaryArea,
    rightArea:              TPolygon;
    nominalLine,
    earlestToleranceLine,
    latestToleranceLine:    TPolyline;
    tmpPart:                TPart;
begin
    SegmentSymbol.EraseArea ();

    leftArea := FTurnInfo.Segment.Area.LeftArea.Clone.AsPolygon;
    primaryArea := FTurnInfo.Segment.Area.PrimaryArea.Clone.AsPolygon;
    rightArea := FTurnInfo.Segment.Area.RightArea.Clone.AsPolygon;
    nominalLine :=FTurnInfo.Segment.Area.NominalLine.Clone.AsPolyline;
{
    if FTurnInfo.PrevTurnInfo.Segment.Area.LeftArea.Count > 0 then
        leftArea.AddRing (FTurnInfo.PrevTurnInfo.Segment.Area.LeftArea.Ring [0]);

    if FTurnInfo.PrevTurnInfo.Segment.Area.PrimaryArea.Count > 0 then
        primaryArea.AddRing (FTurnInfo.PrevTurnInfo.Segment.Area.PrimaryArea.Ring [0]);

	if FTurnInfo.PrevTurnInfo.Segment.Area.RightArea.Count > 0 then
		primaryArea.AddRing (FTurnInfo.PrevTurnInfo.Segment.Area.RightArea.Ring [0]);
}

    tmpPart := TPart.Create;
    earlestToleranceLine := TPolyline.Create;
    latestToleranceLine := TPolyline.Create;
    earlestToleranceLine.AddPart (tmpPart);
    latestToleranceLine.AddPart (tmpPart);
    tmpPart.Free;

    earlestToleranceLine.Part [0].AddMultiPoint (FTurnInfo.Start.EarlestTolerance.PointList);
    latestToleranceLine.Part [0].AddMultiPoint (FTurnInfo.Start.LatestTolerance.PointList);

//	SegmentSymbol.DrawArea (leftArea, primaryArea, rightArea, nominalLine,
//                    earlestToleranceLine, latestToleranceLine);

    leftArea.Free;
    primaryArea.Free;
    rightArea.Free;
    nominalLine.Free;
    earlestToleranceLine.Free;
    latestToleranceLine.Free;
end;

procedure TTurn.DrawArea ();
var
    allArea:        TProtectedArea;
	curTurnInfo:    TTurnInfo;
    nextIsTurnType: Boolean;
    geom:           TGeometry;
    turnSide:           TSideDirection;
    oppositeTurnSide:   TSideDirection;
begin
	SegmentSymbol.EraseArea ();

    allArea := TProtectedArea.Create;
	nextIsTurnType := False;
	turnSide := sideLeft; 			//for remove compiler warning.
	oppositeTurnSide := turnSide;	//for remove compiler warning.

	curTurnInfo := FTurnInfo;

//	if FTurnInfo.TurnType <> tuAtHeight then
//		DrawToleranceRectangle ();

    while (curTurnInfo <> nil) and (not IsGeometryEmpty (curTurnInfo.Segment.AreaToKK.PrimaryArea)) do
    begin
        if not nextIsTurnType then
        begin
	        if curTurnInfo.Segment.AreaToKK.LeftArea.Count > 0 then
	            allArea.LeftArea.AddRing (curTurnInfo.Segment.AreaToKK.LeftArea.Ring [0]);
	        if curTurnInfo.Segment.AreaToKK.PrimaryArea.Count > 0 then
	            allArea.PrimaryArea.AddRing (curTurnInfo.Segment.AreaToKK.PrimaryArea.Ring [0]);
	        if curTurnInfo.Segment.AreaToKK.RightArea.Count > 0 then
	            allArea.RightArea.AddRing (curTurnInfo.Segment.AreaToKK.RightArea.Ring [0]);

            //if curTurnInfo.Segment.AreaToKK.NominalLine.Count > 0 then
            //    allArea.NominalLine.AddPart (curTurnInfo.Segment.AreaToKK.NominalLine.Part [0]);
        end
        else
        begin
            geom := GeoOper.unionGeometry (curTurnInfo.Segment.AreaToKK.PrimaryArea,
                        curTurnInfo.Segment.AreaToKK.SideArea [turnSide]);
            if (geom.GeometryType = gtPolygon) and (geom.AsPolygon.Count > 0) then
                allArea.PrimaryArea.AddRing (geom.AsPolygon.Ring [0]);
            geom.Free;
			if curTurnInfo.Segment.AreaToKK.SideArea [oppositeTurnSide].Count > 0 then
                allArea.SideArea [oppositeTurnSide].AddRing (
                    curTurnInfo.Segment.AreaToKK.SideArea [oppositeTurnSide].Ring [0]);
            nextIsTurnType := False;
        end;

        if curTurnInfo.Segment.AreaToKK.NominalLine.Count > 0 then
            allArea.NominalLine.AddPart (curTurnInfo.Segment.AreaToKK.NominalLine.Part [0]);


		if curTurnInfo.TurnType = tuAtHeight then
		begin
			nextIsTurnType := True;
			turnSide := curTurnInfo.TurnSide;
			oppositeTurnSide := TSideDirection (-1 * Integer (turnSide));
		end;

        curTurnInfo := curTurnInfo.PrevTurnInfo;
    end;

	SegmentSymbol.DrawArea3 (allArea, nil, nil);
end;

procedure TTurn.AtHeightMakeAreaToKK ();
var
    areaLeft,
    areaRight,
    prevArea:       TProtectedArea;
    nominalCutLine: TPolyline;
begin
    prevArea := FTurnInfo.PrevTurnInfo.Segment.ExtendedArea.Clone;
    prevArea.NominalLine.Clear;

    nominalCutLine := CreateCutterLineBaseToleranceLine (FTurnInfo.Start.NominalTolerance);
	CutProtectedArea (FTurnInfo.InnerStartPoint, prevArea, nominalCutLine, sideRight, areaLeft, areaRight);

    FTurnInfo.PrevTurnInfo.Segment.AreaToKK.LeftArea.Assign (areaRight.LeftArea);
    FTurnInfo.PrevTurnInfo.Segment.AreaToKK.PrimaryArea.Assign (areaRight.PrimaryArea);
    FTurnInfo.PrevTurnInfo.Segment.AreaToKK.RightArea.Assign (areaRight.RightArea);

    areaRight.Free;
    prevArea.Free;
    nominalCutLine.Free;
end;

procedure TTurn.CreateArea ();
var
	turnVal:        	Integer;
	oldNextPoint,
	tmpPoint:			TPoint;
	minDist,
	tmpDist,
	deltaX,
	alpha,
	L1, L2, R2:			Double;
	tmpLine:			TPolyline;
	tmpPart:        	TPart;
	areaLeft,
	areaRight:      	TProtectedArea;
begin
	turnVal := Integer (FTurnInfo.TurnSide);

	minDist := CalcVerHorTolerance (L1, L2, R2, alpha);

	if FTurnInfo.Next.Distance < minDist then
	begin
			raise EARANError.Create (
			'The distance between two points must be greater than ' +
			ConvertUnitStr (minDist, cdToOuter, puDistance, rtCeil) + ' ' + GDistanceUnitName + '.',
			mtWarning);
	end;

	CalcTolerance ();

	CalcKKParams ();

	if FTurnInfo.CodeType <> ctDF then
	begin
		//--- begin: Create ProtectedArea (Extended Area).
		deltaX := 15000.0;
		FTurnInfo.Next.Distance := FTurnInfo.Next.Distance + deltaX;
		tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction, deltaX);

		oldNextPoint := TPoint.Create (FTurnInfo.Next.Point);
		FTurnInfo.Next.Point.Assign (tmpPoint);
		tmpPoint.Free;

		CreateProtectedArea (L1, L2, R2, alpha, FTurnInfo.Next.ATT + deltaX);

		FTurnInfo.Segment.ExtendedArea.Assign (FTurnInfo.Segment.Area);
		//--- end.

		FTurnInfo.Next.Point.Assign (oldNextPoint);
		oldNextPoint.Free;
		FTurnInfo.Next.Distance := FTurnInfo.Next.Distance - deltaX;

		//--- begin: Create cutterLine.
		tmpDist := FTurnInfo.ProtectedRadius * 4;
		tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction - 0.5*Pi, tmpDist);

		tmpPart := TPart.Create;
		tmpPart.AddPoint (tmpPoint);
		tmpPoint.Free;
		tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction + 0.5*Pi, tmpDist);
		tmpPart.AddPoint (tmpPoint);
		tmpPoint.Free;
		tmpLine := TPolyline.Create;
		tmpLine.AddPart (tmpPart);
		//--- end.

		CutProtectedArea (FTurnInfo.InnerStartPoint, FTurnInfo.Segment.Area, tmpLine, sideLeft, areaLeft, areaRight);
		FTurnInfo.Segment.Area.Assign (areaLeft);
		FTurnInfo.Segment.Area.NominalLine.Assign (FTurnInfo.Segment.ExtendedArea.NominalLine);
		if not IsGeometryEmpty (FTurnInfo.Segment.Area.NominalLine) then
		begin
			FTurnInfo.Segment.Area.NominalLine.Part [0].Point
					[FTurnInfo.Segment.Area.NominalLine.Part [0].Count - 1].Assign (FTurnInfo.Next.Point);
		end;
		areaLeft.Free;
		tmpLine.Free;
	end
    	else
		begin
			CreateProtectedArea (L1, L2, R2, alpha, FTurnInfo.Next.ATT);

    		if (FTurnInfo.TurnType = tuAtHeight) and
				(FTurnInfo.Segment.Area.PrimaryArea.Count > 0) then
			FTurnInfo.Segment.Area.PrimaryArea.Ring [0].AddPoint (FParams.StartRwyDirection.Prj);
	end;

	AddToArea ();
end;

procedure TTurn.AddToArea ();
var
	side:           	TSideDirection;
	latestCutLine,
	earlestCutLine:		TPolyline;
	geom:				TGeometry;
	prevArea,
	curArea,
	middleToleranceArea,
	areaLeft,
	areaRight:      	TProtectedArea;
begin
	try
		FTurnInfo.PrevTurnInfo.Segment.AreaToKK.Clear;

		//--- begin: Union current area with prev extended area cut by KK and NN lines.
		prevArea := FTurnInfo.PrevTurnInfo.Segment.ExtendedArea.Clone;
		curArea := FTurnInfo.Segment.Area.Clone;

		prevArea.NominalLine.Clear;
		//curArea.SideArea [FTurnInfo.TurnSide].Clear;
		curArea.NominalLine.Clear;

//DrawProtectedArea_ForDebug (curArea);

        latestCutLine := CreateCutterLineBaseToleranceLine (FTurnInfo.Start.LatestTolerance);
		MoveMultiPoint (latestCutLine.Part [0].AsMultiPoint, FTurnInfo.Start.Direction, 0.025);
		earlestCutLine := CreateCutterLineBaseToleranceLine (FTurnInfo.Start.EarlestTolerance);
        MoveMultiPoint (earlestCutLine.Part [0].AsMultiPoint, FTurnInfo.Start.Direction, -0.025);

		CutProtectedArea (FTurnInfo.InnerStartPoint, prevArea, latestCutLine, sideRight, areaLeft, areaRight);

		middleToleranceArea := areaRight;
        areaRight := nil;

        if FTurnInfo.TurnType <> tuAtHeight then
			side := sideOn
		else
            side := sideLeft;

		CutProtectedArea (FTurnInfo.InnerStartPoint, middleToleranceArea, earlestCutLine, side, areaLeft, areaRight);

        if FTurnInfo.TurnType <> tuAtHeight then
			FTurnInfo.PrevTurnInfo.Segment.AreaToKK.Assign (areaRight)
        else
            AtHeightMakeAreaToKK ();
		areaRight.Free;

		FTurnInfo.PrevTurnInfo.Segment.AreaToKK.NominalLine.Assign (FTurnInfo.PrevTurnInfo.Segment.Area.NominalLine);
        if (not IsGeometryEmpty (FTurnInfo.PrevTurnInfo.Segment.AreaToKK.NominalLine)) and
            (not IsGeometryEmpty (FTurnInfo.PrevTurnInfo.Segment.Area.NominalLine)) then
        begin
            FTurnInfo.PrevTurnInfo.Segment.AreaToKK.NominalLine.Part [0].Point [
                FTurnInfo.PrevTurnInfo.Segment.AreaToKK.NominalLine.Part [0].Count - 1].Assign (
                FTurnInfo.Segment.Area.NominalLine.Part [0].Point [0]);
		end;

		if not IsGeometryEmpty (areaLeft.LeftArea) then
			middleToleranceArea.LeftArea.Assign (areaLeft.LeftArea);
		middleToleranceArea.PrimaryArea.Assign (areaLeft.PrimaryArea);
		if not IsGeometryEmpty (areaLeft.RightArea) then
			middleToleranceArea.RightArea.Assign (areaLeft.RightArea);

//DrawProtectedArea_ForDebug (middleToleranceArea);
//ShowMessage ('1');

		areaLeft.Free;

//DrawProtectedArea_ForDebug (FTurnInfo.Segment.Area);
//DrawProtectedArea_ForDebug (curArea);
//DrawProtectedArea_ForDebug (middleToleranceArea);

//DrawGeometry (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide]);
//DrawGeometry (FTurnInfo.PrevTurnInfo.Segment.Area.PrimaryArea);


		areaRight := UnionProtectedArea (FTurnInfo.InnerStartPoint, curArea, middleToleranceArea);

		if not IsGeometryEmpty (areaRight.LeftArea) then
			FTurnInfo.Segment.Area.LeftArea.Assign (areaRight.LeftArea);
		FTurnInfo.Segment.Area.PrimaryArea.Assign (areaRight.PrimaryArea);
		if not IsGeometryEmpty (areaRight.RightArea) then
			FTurnInfo.Segment.Area.RightArea.Assign (areaRight.RightArea);

		areaRight.Free;

//DrawProtectedArea_ForDebug (FTurnInfo.Segment.Area);
//ShowMessage ('1');

//DrawGeometry (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide]);
//DrawGeometry (FTurnInfo.PrevTurnInfo.Segment.Area.PrimaryArea);

		geom := GeoOper.difference (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide],
					FTurnInfo.PrevTurnInfo.Segment.Area.PrimaryArea);
		if not IsGeometryEmpty (geom) then
		begin
			RemoveNearestRing (FTurnInfo.InnerStartPoint, geom.AsPolygon);
			FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide].Assign (geom);
		end;
		geom.Free;

//DrawGeometry (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide]);
//DrawGeometry (FTurnInfo.Segment.Area.PrimaryArea);

		geom := GeoOper.difference (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide],
					FTurnInfo.Segment.Area.PrimaryArea);
		if not IsGeometryEmpty (geom) then
		begin
			RemoveNearestRing (FTurnInfo.InnerStartPoint, geom.AsPolygon);
			FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide].Assign (geom);
		end;
		geom.Free;

//DrawGeometry (FTurnInfo.Segment.Area.SideArea [FTurnInfo.TurnSide]);

		latestCutLine.Free;
		earlestCutLine.Free;
        prevArea.Free;
        curArea.Free;
        middleToleranceArea.Free;
		//--- end.

        FTurnInfo.Segment.AreaToKK.Assign (FTurnInfo.Segment.Area);

		DrawArea ();
    except
        on ex: Exception do
		begin
			raise;
		end
	end;
end;

procedure TTurn.CalcKKParams ();
var
    earlestLine:        TPolyline;
    prevEarlestLine:    TPolyline;
    tmpPart:            TPart;
begin
	if FTurnInfo.TurnType = tuAtHeight then
    begin
        FTurnInfo.Start.DistanceToKK := TurnInfo.Start.Distance;
    end
    else
	begin
		earlestLine := TPolyline.Create;
		prevEarlestLine := TPolyline.Create;

		tmpPart := TPart.Create;
		tmpPart.AddMultiPoint (FTurnInfo.Start.EarlestTolerance.PointList);
		earlestLine.AddPart (tmpPart);
		tmpPart.Clear;
		tmpPart.AddMultiPoint (FTurnInfo.PrevTurnInfo.Start.EarlestTolerance.PointList);
		prevEarlestLine.AddMultiPoint (tmpPart);
		tmpPart.Free;

		FTurnInfo.Start.DistanceToKK := GeoOper.getDistance (prevEarlestLine, earlestLine);

		earlestLine.Free;
		prevEarlestLine.Free;

        FTurnInfo.Start.HeightToKK := FTurnInfo.PrevTurnInfo.PDG * FTurnInfo.Start.DistanceToKK +
            GConst.PANSOPS.Constant [dpH_abv_DER].Value;
	end;
end;

procedure TTurn.CalcTolerance ();
var
	cutLine:	TPolyline;
    middlePoint:  TPoint;
    tmpPoint:   TPoint;
    tmpD:       Double;
begin
    cutLine := LineAlong (FTurnInfo.Start.Point, FTurnInfo.Start.Direction - 0.5*Pi,
			        GConst.GNSS.Constant [DEP_].SemiWidth * 2);

    FTurnInfo.Start.NominalTolerance.SetAccuracyLine (FTurnInfo.Start.Point,
        FTurnInfo.Start.Direction, cutLine, FStraight.Segment.ExtendedArea);

    MoveMultiPoint (cutLine.Part [0].AsMultiPoint, FTurnInfo.Start.Direction, Abs(FTurnInfo.Start.LTP));
    middlePoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction, Abs(FTurnInfo.Start.LTP));

    FTurnInfo.Start.LatestTolerance.SetAccuracyLine (middlePoint, FTurnInfo.Start.Direction,
        cutLine, FStraight.Segment.ExtendedArea);

    middlePoint.Free;

    if FTurnInfo.TurnType = tuAtHeight then
    begin
        cutLine.Free;
        middlePoint := PointAlongPlane (FParams.PairRwyDirection.Prj, FParams.StartRwyDirection.Direction, 600.0);
        cutLine := LineAlong (middlePoint, FParams.StartRwyDirection.Direction - 0.5*Pi,
			            GConst.GNSS.Constant [DEP_].SemiWidth * 2);
        tmpD := FParams.FStartRwyDirection.Direction;
    end
    else
    begin
        MoveMultiPoint (cutLine.Part [0].AsMultiPoint, FTurnInfo.Start.Direction, -(FTurnInfo.Start.ETP + Abs(FTurnInfo.Start.LTP)));
        middlePoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction, -FTurnInfo.Start.ETP);
        tmpD := FTurnInfo.Start.Direction;
    end;

   	FTurnInfo.Start.EarlestTolerance.SetAccuracyLine (middlePoint,
        tmpD, cutLine, FStraight.Segment.ExtendedArea);

	cutLine.Free;

	FTurnInfo.Start.EarlestTolerance.SetSideDirection (FTurnInfo.TurnSide);
	FTurnInfo.Start.NominalTolerance.SetSideDirection (FTurnInfo.TurnSide);
	FTurnInfo.Start.LatestTolerance.SetSideDirection (FTurnInfo.TurnSide);
end;

procedure TTurn.CreateProtectedArea (L1, L2, R2, alpha: Double; ATTValue: Double);
const
	splayAngle = 15.0 * Pi/180.0;
var
	edgePoly,
	nominalPoly:	TPolyline;
begin
	if FTurnInfo.CodeType = ctTF then
		nominalPoly := CreateNominalTF (L1, L2, R2, alpha)
	else if FTurnInfo.CodeType = ctCF then
		nominalPoly := CreateNominalCF (R2)
	else
		nominalPoly := CreateNominalDF ();

	if FTurnInfo.TurnAngle = GNullRadianValue then
		FTurnInfo.TurnAngle :=  Modulus ((TurnInfo.Start.Direction - TurnInfo.Next.Direction) * Integer (FTurnInfo.TurnSide), 2*Pi);

	FTurnInfo.Segment.Area.NominalLine.Assign (nominalPoly);

	//--- if Code Type id DF then this function only is to calculate Next.Width
    edgePoly := GetEdges ();

	if FTurnInfo.CodeType = ctDF then
		CreateSegmentDF (ATTValue)
	else
	begin
		CreateSegment (edgePoly, ATTValue);
	end;

	edgePoly.Free;
	nominalPoly.Free;
end;

function TTurn.GetOuterSpiralDF (
					radius,
					nextSemiWidth,
					ATTValue: Double;
					var outerNominalDir: Double;
					basePoints: TMultiPoint;
					latestNominalPart: TPart;
                    var intersectPointIndex: Integer): TMultiPoint;
var
	spiralCount,
	turnVal, i:		Integer;
	startLine,
	endLine:		TLine;
	bank, rv,
	coef, TAS, H,
	windSpeed,
	spiralStartDir,
	spiralEndDir,
    middleNominalDir:	Double;
	tmpMultiPoint,
	spiralPoints:	TMultiPoint;
	tmpPoint,
	tmpPoint2,
	earlestPoint:	TPoint;
	side:			TSideDirection;
	tmpLine:		TPolyline;
	part:			TPart;
	createLastSpiral:	Boolean;

	__UseConstant__: Integer;
begin
	part := TPart.Create;
	tmpLine := TPolyline.Create;
	tmpLine.AddPart (part);
	part.Free;

	windSpeed := 56.0;
	turnVal := Integer (FTurnInfo.TurnSide);

	H := FTurnInfo.PrevTurnInfo.Start.HeightProtect +
		FTurnInfo.Start.Distance * FTurnInfo.PDGProtect;

	TAS := IASToTAS (FTurnInfo.IAS, H, FParams.AHP.Temperature);

	bank := RadiusToBank (radius, TAS);
	rv := 6355.0 * Tan (bank) / (Pi * TAS * 3.6);
	if rv > 3.0 then
		rv := 3.0;
	coef := windSpeed / (rv * 3.6);

	spiralCount := Ceil (FTurnInfo.TurnAngle / (0.5 * Pi));
	if spiralCount > 3 then
		raise EARANError.Create ('Turn angle must be less than 270');

	spiralPoints := TMultiPoint.Create;
	spiralStartDir := FTurnInfo.Start.Direction;

	for i:=0 to spiralCount -2 do
	begin
		spiralEndDir := ReturnAngleInRadians (basePoints.Point [i], basePoints.Point [i+1]);
		tmpMultiPoint := CreateWindSpiral (basePoints.Point [i], FTurnInfo.Start.Direction,
							spiralStartDir, spiralEndDir, radius, coef, FTurnInfo.TurnSide);
		spiralPoints.AddMultiPoint (tmpMultiPoint);
		tmpMultiPoint.Free;

		spiralStartDir := spiralEndDir;
	end;

	if outerNominalDir = GNullRadianValue then
	begin
		middleNominalDir := ReturnAngleInRadians (
			FTurnInfo.Segment.Area.NominalLine.Part [0].Point [FTurnInfo.Segment.Area.NominalLine.Part [0].Count - 2],
			FTurnInfo.Segment.Area.NominalLine.Part [0].Point [FTurnInfo.Segment.Area.NominalLine.Part [0].Count - 1]);

		startLine := TLine.Create (basePoints.Point [spiralCount - 1], FTurnInfo.Start.Direction);
		endLine := TLine.Create (FTurnInfo.Next.Point, middleNominalDir);
		outerNominalDir := FixToTouchSprial (startLine, endLine, coef, radius, FTurnInfo.TurnSide);

		if outerNominalDir = GNullRadianValue then
			raise EARANError.Create ('Next point is inside wind spiral.', mtWarning);

		startLine.Free;
		endLine.Free;
	end;

	createLastSpiral := True;

	if spiralCount > 1 then
	begin
		if SubtractAngles (outerNominalDir, spiralStartDir) < (Pi/12) then
			createLastSpiral := False
		else
			spiralEndDir := outerNominalDir;
	end
	else
	begin
		spiralEndDir := outerNominalDir;
	end;		

	if createLastSpiral then
	begin
		tmpMultiPoint := CreateWindSpiral (basePoints.Point [spiralCount - 1], FTurnInfo.Start.Direction,
							spiralStartDir, spiralEndDir, radius, coef, FTurnInfo.TurnSide);
		spiralPoints.AddMultiPoint (tmpMultiPoint);
		tmpMultiPoint.Free;
	end;

	if latestNominalPart <> nil then
	begin
		latestNominalPart.AddMultiPoint (spiralPoints);
		latestNominalPart.AddPoint (FTurnInfo.Next.Point);
		FTurnInfo.Next.LatestDirection := ReturnAngleInRadians (latestNominalPart.Point [latestNominalPart.Count - 2],
												latestNominalPart.Point [latestNominalPart.Count - 1]);
	end;

	tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, outerNominalDir + 0.5*Pi*turnVal, nextSemiWidth);

	tmpPoint2 := LineLineIntersect (
					spiralPoints.Point [spiralPoints.Count - 1], spiralEndDir + (Pi/12)*turnVal,
					tmpPoint, outerNominalDir).AsPoint;

	earlestPoint := PointAlongPlane (FTurnInfo.Next.Point, outerNominalDir, -ATTValue);
	side := SideDef (earlestPoint, outerNominalDir + 0.5*Pi, tmpPoint2);
	if side = sideRight then
    begin
		tmpPoint2.Free;
		tmpPoint2 := PointAlongPlane (earlestPoint, outerNominalDir + 0.5*Pi*turnVal, nextSemiWidth);
	end;
	earlestPoint.Free;
	
	spiralPoints.AddPoint (tmpPoint2);
    intersectPointIndex := spiralPoints.Count - 2;
	spiralPoints.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint2.Free;
    
	result := spiralPoints;
end;

function TTurn.CreateNominalTF (L1, L2, R2, alpha: Double): TPolyline;
var
	lineArray:	TLineArray;
	tempPoint,
	tempPoint2:	TPoint;
	turnVal,
	i:			Integer;
	tempD,
	tempD2:		Double;
begin

	turnVal := Integer (FTurnInfo.TurnSide);

	if TurnInfo.TurnType = tuFlyBy then
	begin
		SetLength (lineArray, 4);
		tempPoint := PointAlongPlane (TurnInfo.Start.Point, TurnInfo.Start.Direction + Pi, L1);
		lineArray [0] := TLine.Create (tempPoint, TurnInfo.Start.Direction);
		tempPoint.Free;
		tempPoint := PointAlongPlane (TurnInfo.Start.Point, TurnInfo.Next.Direction, L1);
		lineArray [1] := TLine.Create (tempPoint, TurnInfo.Next.Direction);
		tempPoint.Free;
		tempPoint := PointAlongPlane (lineArray [1].RefPoint, TurnInfo.Next.Direction, L2);
		lineArray [2] := TLine.Create (tempPoint, TurnInfo.Next.Direction);
		tempPoint.Free;
	end
	else if TurnInfo.TurnType = tuFlyOver then
	begin
		SetLength (lineArray, 5);
		lineArray [0] := TLine.Create (TurnInfo.Start.Point, TurnInfo.Start.Direction);

		tempPoint := PointAlongPlane (TurnInfo.Start.Point, TurnInfo.Start.Direction - 0.5*Pi*turnVal, TurnInfo.NominalRadius);
		tempD := TurnInfo.Start.Direction - (FTurnInfo.TurnAngle + alpha) * turnVal;

		tempPoint2 := PointAlongPlane (tempPoint, tempD + 0.5*Pi*turnVal, TurnInfo.NominalRadius);
		tempPoint.Free;
		lineArray [1] := TLine.Create (tempPoint2, tempD);

		tempPoint := LineLineIntersect (tempPoint2, tempD, TurnInfo.Next.Point, TurnInfo.Next.Direction).AsPoint;
		tempPoint2.Free;
		tempD2 := R2 * Tan(0.5 * alpha);
		tempPoint2 := PointAlongPlane (tempPoint, tempD + Pi, tempD2);
		lineArray [2] := TLine.Create (tempPoint2, tempD);
		tempPoint2.Free;
		tempPoint2 := PointAlongPlane (tempPoint, TurnInfo.Next.Direction, tempD2);
		tempPoint.Free;
		lineArray [3] := TLine.Create (tempPoint2, TurnInfo.Next.Direction);
		tempPoint2.Free;
	end;

	lineArray [Length (lineArray) - 1] := TLine.Create (TurnInfo.Next.Point, TurnInfo.Next.Direction);

	result := CalcTrajectoryFromMultiPoint (lineArray);

	for i := 0 to Length (lineArray) - 1 do
		lineArray [i].Free;
end;

function TTurn.CreateNominalDF (): TPolyline;
var
	i:				Integer;
	lineArray:		TLineArray;
	intersectPt,
	centrePt:		TPoint;
    nextDirection:  Double;
begin
	setLength (lineArray, 3);

	lineArray [0] := TLine.Create (TurnInfo.Start.Point, TurnInfo.Start.Direction);

	centrePt := PointAlongPlane (FTurnInfo.Start.Point,
        FTurnInfo.Start.Direction - 0.5*Pi*Integer(FTurnInfo.TurnSide), FTurnInfo.NominalRadius);

	intersectPt := TangentCyrcleIntersectPoint (centrePt, FTurnInfo.NominalRadius,
        FTurnInfo.Next.Point, FTurnInfo.TurnSide);
	centrePt.Free;
	nextDirection := ReturnAngleInRadians (intersectPt, FTurnInfo.Next.Point);
	lineArray [1] := TLine.Create (intersectPt, nextDirection);
	intersectPt.Free;
	lineArray [2] := TLine.Create (TurnInfo.Next.Point, nextDirection);

    result := CalcTrajectoryFromMultiPoint (lineArray); 

	for i := 0 to Length (lineArray) - 1 do
		lineArray [i].Free;
end;

function TTurn.CreateNominalCF (R2: Double): TPolyline;
var
	i, turnVal:		Integer;
	tmpPt,
	tmpCentrePt,
	tmpIntersectPt:	TPoint;
	tmpLine:		TLine;
	d, dir,
	dirCntToInt:	Double;
	lineArray:		TLineArray;
begin
	turnVal := Integer (FTurnInfo.TurnSide);
	tmpCentrePt := PointAlongPlane (TurnInfo.Start.Point, TurnInfo.Start.Direction - 0.5*Pi*turnVal, FTurnInfo.NominalRadius);
	tmpPt := PointAlongPlane (TurnInfo.Next.Point, TurnInfo.Next.Direction + 0.5*Pi*turnVal, R2);
	tmpLine := TLine.Create(tmpPt, TurnInfo.Next.Direction);
	tmpPt.Free;

	tmpIntersectPt := CircleVectorIntersect (tmpCentrePt, TurnInfo.NominalRadius + R2, tmpLine, d);
	if tmpIntersectPt = nil then
		raise EARANError.Create ('Could not turn with given direction', mtWarning);
	tmpLine.Free;

	SetLength (lineArray, 4);
	lineArray [0] := TLine.Create (TurnInfo.Start.Point, TurnInfo.Start.Direction);

	dirCntToInt := ReturnAngleInRadians (tmpCentrePt, tmpIntersectPt);
	tmpPt := PointAlongPlane (tmpCentrePt, dirCntToInt, TurnInfo.NominalRadius);
	dir := dirCntToInt - 0.5*Pi*turnVal;
	lineArray [1] := TLine.Create (tmpPt, dir);
	tmpPt.Free;

	tmpPt := PointAlongPlane (tmpIntersectPt, TurnInfo.Next.Direction - 0.5*Pi*turnVal, R2);
	lineArray [2] := TLine.Create (tmpPt, TurnInfo.Next.Direction);
	tmpPt.Free;

	lineArray [3] := TLine.Create (TurnInfo.Next.Point, TurnInfo.Next.Direction);

	result := CalcTrajectoryFromMultiPoint (lineArray);

	tmpIntersectPt.Free;
	tmpCentrePt.Free;
	for i := 0 to Length (lineArray) - 1 do
		lineArray [i].Free;
end;

function TTurn.GetEdges (): TPolyline;
const
	AHPCircleR = 56000; alpha =  15*(PI/180);
var
	i, turnVal:			Integer;
    nextDistance,
    h, w, d, ATT:       Double;
	tmpPoint,
	startPoint,
	endPoint,
	expandStartPoint,
	expandEndPoint:		TPoint;
	line:				TLine;
	sideSS, sideSE,
	sideES, sideEE:		TSideDirection;
	part:				TPart;
	mp:					TMultiPoint;
    earlestLine:        TPolyline;
begin
	turnVal := Integer (FTurnInfo.TurnSide);
	
	result := TPolyline.Create;
	part := TPart.Create;
	result.AddPart (part);
	result.AddPart (part);
	result.AddPart (part);
	result.AddPart (part);
	part.Free;

	mp := TMultiPoint.Create;
	expandEndPoint := nil;

	startPoint := LineLineIntersect (FTurnInfo.Start.Point, FTurnInfo.Start.Direction,
					FTurnInfo.Next.Point, FTurnInfo.Next.Direction).AsPoint;
	endPoint := FTurnInfo.Next.Point.Clone.AsPoint;

	line := TLine.Create (TurnInfo.Next.Point, TurnInfo.Next.Direction);
	expandStartPoint := CircleVectorIntersect (FParams.AHP.Prj, AHPCircleR, line, d);
	line.Free;

	if (expandStartPoint = nil) or (FParams.SensorType = stDME) then
	begin
        if FParams.SensorType = stDME then
        begin
            h := FTurnInfo.Start.Distance * FTurnInfo.PrevTurnInfo.PDG + FTurnInfo.PrevTurnInfo.Start.HeightToKK;
            GetDMEToleranceParams (False, h, FParams.IsDMEtill_1989, FParams.DMECount, d, ATT, w);
            startPoint.T := w;

            //--- begin: Calculate distance between next point and KK line.
            earlestLine := TPolyline.Create ();
            earlestLine.AddMultiPoint (FTurnInfo.Start.EarlestTolerance.PointList);
            nextDistance := GeoOper.getDistance (FTurnInfo.Next.Point, earlestLine);
            earlestLine.Free;
            //--- end.

            h := nextDistance * FTurnInfo.PDG + FTurnInfo.Start.HeightToKK;
            GetDMEToleranceParams (False, h, FParams.IsDMEtill_1989, FParams.DMECount, d, ATT, w);
            endPoint.T := w;
            FTurnInfo.Next.ATT := ATT;
        end
        else
        begin
		    startPoint.T := GConst.GNSS.Constant [DEP_].SemiWidth ;
		    endPoint.T := startPoint.T;
        end;
		mp.AddPoint (startPoint);
		mp.AddPoint (endPoint);
	end
	else
	begin
		expandStartPoint.T := GConst.GNSS.Constant [IDEP_].SemiWidth;
		expandEndPoint := PointAlongPlane (expandStartPoint,
							FTurnInfo.Next.Direction,
							(GConst.GNSS.Constant [DEP_].SemiWidth - GConst.GNSS.Constant [IDEP_].SemiWidth) / Tan (alpha));
		expandEndPoint.T := GConst.GNSS.Constant [DEP_].SemiWidth;

		sideSS := SideDef (expandStartPoint, FTurnInfo.Next.Direction + 0.5*Pi, FTurnInfo.Start.Point);
		sideSE := SideDef (expandEndPoint, FTurnInfo.Next.Direction + 0.5*Pi, FTurnInfo.Start.Point);

		if sideSS = sideLeft then
			startPoint.T := GConst.GNSS.Constant [IDEP_].SemiWidth
		else
		begin
			if sideSE = sideRight then
				startPoint.T := GConst.GNSS.Constant [DEP_].SemiWidth
			else
			begin
				d := ReturnDistanceInMeters (expandStartPoint, startPoint);
				startPoint.T := d * Tan (alpha) + GConst.GNSS.Constant [IDEP_].SemiWidth;
			end;
		end;

		sideES := SideDef (expandStartPoint, FTurnInfo.Next.Direction + 0.5*Pi, FTurnInfo.Next.Point);
		sideEE := SideDef (expandEndPoint, FTurnInfo.Next.Direction + 0.5*Pi, FTurnInfo.Next.Point);

		if sideES = sideLeft then
			endPoint.T := GConst.GNSS.Constant [IDEP_].SemiWidth
		else
		begin
			if sideEE = sideRight then
				endPoint.T := GConst.GNSS.Constant [DEP_].SemiWidth
			else
			begin
				d := ReturnDistanceInMeters (expandStartPoint, endPoint);
				endPoint.T := d * Tan (alpha) + GConst.GNSS.Constant [IDEP_].SemiWidth;
			end;
		end;

		mp.AddPoint (startPoint);

		if sideSS <> sideES then
			mp.AddPoint (expandStartPoint);
		if sideSE <> sideEE then
			mp.AddPoint (expandEndPoint);

		mp.AddPoint (endPoint);
	end;

	for i:=0 to mp.Count - 1 do
	begin
		tmpPoint := PointAlongPlane (mp.Point [i], FTurnInfo.Next.Direction + 0.5*Pi*turnVal, mp.Point [i].T);
		result.Part [0].AddPoint (tmpPoint);
		tmpPoint.Free;

		tmpPoint := PointAlongPlane (mp.Point [i], FTurnInfo.Next.Direction + 0.5*Pi*turnVal, mp.Point [i].T / 2);
		result.Part [1].AddPoint (tmpPoint);
		tmpPoint.Free;

		tmpPoint := PointAlongPlane (mp.Point [i], FTurnInfo.Next.Direction - 0.5*Pi*turnVal, mp.Point [i].T / 2);
		result.Part [2].AddPoint (tmpPoint);
		tmpPoint.Free;

		tmpPoint := PointAlongPlane (mp.Point [i], FTurnInfo.Next.Direction - 0.5*Pi*turnVal, mp.Point [i].T);
		result.Part [3].AddPoint (tmpPoint);
		tmpPoint.Free;
	end;

	FTurnInfo.Next.Width := endPoint.T / 2;
	if FTurnInfo.Next.Width < FTurnInfo.Start.NominalTolerance.OuterWidth then
		FTurnInfo.Next.Width := FTurnInfo.Start.NominalTolerance.OuterWidth;
	if FTurnInfo.Next.Width < FTurnInfo.Start.NominalTolerance.InnerWidth then
		FTurnInfo.Next.Width := FTurnInfo.Start.NominalTolerance.InnerWidth;

	startPoint.Free;
	endPoint.Free;
	expandStartPoint.Free;
	expandEndPoint.Free;
	mp.Free;
end;

function TTurn.GetSpiralSideSplitIndex (spiralPoints: TMultiPoint; edgeLine: TPolyline; turnSide: TSideDirection; var intersectPoint: TPoint): Integer;
var
	i:				Integer;
	dir:			Double;
	edgeDir:		Double;
	side:			TSideDirection;
    tmpPoint:		TPoint;
	spiralLine,
	longerEdgeLine:	TPolyline;
	part:			TPart;
	geom:			TGeometry;
begin

	result := -1;

	if (edgeLine.Count = 0) or (edgeLine.Part[0].Count < 2) or (spiralPoints.Count = 0)then
		exit;

	edgeDir := ReturnAngleInRadians (edgeLine.Part [0].Point [0], edgeLine.Part [0].Point [1]);

	longerEdgeLine := edgeLine.Clone.AsPolyline;
	tmpPoint := PointAlongPlane (edgeLine.Part [0].Point [0], edgeDir, - FTurnInfo.Start.NominalTolerance.PrimOuterWidth);
	longerEdgeLine.Part [0].InsertPoint (0, tmpPoint);
	tmpPoint.Free;

	side := EdgeSideDef(edgeLine, spiralPoints.Point [spiralPoints.Count-1]);

	if Integer (side) * Integer (turnSide) < 0 then exit;

	spiralLine := TPolyline.Create;
	part := TPart.Create;
	part.AddMultiPoint (spiralPoints);	
	spiralLine.AddPart (part);
	part.Free;

	geom := GeoOper.intersect (spiralLine, longerEdgeLine);
	spiralLine.Free;

	if IsGeometryEmpty (geom) then
        exit;

	if geom.AsMultiPoint.Count = 1 then
		intersectPoint := geom.asMultiPoint.Point [0].Clone().AsPoint
	else
	begin
		if ReturnDistanceInMeters (geom.asMultiPoint.Point [0], spiralPoints.Point [spiralPoints.Count - 1]) <
			ReturnDistanceInMeters (geom.asMultiPoint.Point [1], spiralPoints.Point [spiralPoints.Count - 1]) then
			intersectPoint := geom.asMultiPoint.Point [0].Clone().AsPoint
		else
			intersectPoint := geom.asMultiPoint.Point [1].Clone().AsPoint;
	end;

	dir := 4.0;

	for i:=1 to edgeLine.Part [0].Count - 1 do
	begin
		edgeDir := ReturnAngleInRadians (edgeLine.Part [0].Point [i-1], edgeLine.Part [0].Point [i]);
		side := SideDef (edgeLine.Part [0].Point [i], edgeDir + 0.5*Pi, intersectPoint);
		if (side = sideLeft) or (side = sideOn) then
		begin
			dir := edgeDir;
			break;
		end;
	end;

	if dir = 4.0 then exit;

	for i:=(spiralPoints.Count - 1) downto 0 do
	begin
		side := SideDef (intersectPoint, dir, spiralPoints.Point [i]);
		if Integer (side) * Integer (turnSide) <= 0 then
		begin
			result := i + 1;
			break;
		end;
	end;

	geom.Free;
end;

function TTurn.DeleteSpiralOutsidePoints (spiralPoints: TMultiPoint; edgeLine: TPolyline; turnSide: TSideDirection): Boolean;
var
	i:				Integer;
	intersectPoint:	TPoint;
	splitIndex:		Integer;
begin
	result := False;

	intersectPoint := nil;

	splitIndex := GetSpiralSideSplitIndex (spiralPoints, edgeLine, turnSide, intersectPoint);

	if splitIndex = -1 then exit;

	for i := spiralPoints.Count - 1 downto splitIndex do
		spiralPoints.RemoveAt (i);

	spiralPoints.AddPoint (intersectPoint);
	intersectPoint.Free;
	result := True;
end;

function TTurn.EdgeSideDef (edgeLine: TPolyline; outPoint: TPoint): TSideDirection;
var
	i:			Integer;
	side:		TSideDirection;
	dir:		Double;
begin
	dir := 0;
	if (edgeLine.Count = 0) or (edgeLine.Part [0].Count < 2) then
		raise EARANError.Create ('Incorrect Edge Line');

	for i:=1 to edgeLine.Part [0].Count - 1 do
	begin
		dir := ReturnAngleInRadians (edgeLine.Part [0].Point [i-1], edgeLine.Part [0].Point [i]);
		side := SideDef (edgeLine.Part [0].Point [i], dir + 0.5*Pi, outPoint);
		if (side = sideLeft) or (side = sideOn) then
		begin
			result := SideDef (edgeLine.Part [0].Point [i], dir, outPoint);
			exit;
		end;
	end;
	result := SideDef (edgeLine.Part [0].Point [edgeLine.Part [0].Count - 1], dir, outPoint);
end;

function TTurn.GetInnerAddArea (edgeLine: TPolyline; startPoint: TPoint; var startPointSide: TSideDirection; ATTValue: Double): TMultiPoint;
var
	side:			TSideDirection;
	tmpD, dir,
	edgeDir,
	newAlpha:		Double;
	i, index,
	turnVal:		Integer;
	longerEdgeLine,
	tmpLine:		TPolyline;
	part:			TPart;
	intPoint,
	tmpPoint,
	nextEarlestPoint:	TPoint;
	geom:			TGeometry;
	getEdgePoints,
	notIntersected:	Boolean;
begin
	result := TMultiPoint.Create;
	turnVal := Integer (FTurnInfo.TurnSide);

	tmpLine := TPolyLine.Create;
	part := TPart.Create;
	tmpLine.AddPart (part);
	part.Free;

	intPoint := nil;
	getEdgePoints := True;

	tmpD := ReturnDistanceInMeters (startPoint, FTurnInfo.Next.Point) * 2;
	tmpLine.Part [0].AddPoint (startPoint);

	side := EdgeSideDef (edgeLine, startPoint);

	startPointSide := side;

	longerEdgeLine := edgeLine.Clone.AsPolyline;
	tmpPoint := PointAlongPlane (edgeLine.Part [0].Point [0],
					ReturnAngleInRadians (edgeLine.Part [0].Point [0], edgeLine.Part [0].Point [1]),
					-FTurnInfo.Start.NominalTolerance.PrimOuterWidth);
	longerEdgeLine.Part [0].InsertPoint (0, tmpPoint);
	tmpPoint.Free;

	if Integer (side) * turnVal > 0 then //--- Alpha / 2
	begin
		dir := FTurnInfo.Next.Direction + (FTurnInfo.TurnAngle / 2)*turnVal;

		tmpPoint := PointAlongPlane (startPoint, dir, tmpD);
		tmpLine.Part [0].AddPoint (tmpPoint);
		tmpPoint.Free;

		geom := GeoOper.intersect (tmpLine, longerEdgeLine);
		if IsGeometryEmpty (geom) then
		begin
			intPoint := LineLineIntersect (startPoint, dir, FTurnInfo.Next.Point, FTurnInfo.Next.Direction + 0.5*Pi).AsPoint;
			result.AddPoint (startPoint);
			result.AddPoint (intPoint);
			getEdgePoints := False;
		end
		else
		begin
			intPoint := geom.AsMultiPoint.Point [0].Clone.AsPoint;
			getEdgePoints := True;
		end;
		geom.Free;
	end
	else //----------------------------------- 15 degree
	begin
		nextEarlestPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction, -ATTValue);
		dir := FTurnInfo.Next.Direction - (Pi/12)*turnVal;

		tmpPoint := PointAlongPlane (startPoint, dir, tmpD);
		tmpLine.Part [0].AddPoint (tmpPoint);

		geom := GeoOper.intersect (tmpLine, longerEdgeLine);
		if IsGeometryEmpty (geom) then
			notIntersected := True
		else
		begin
			side := SideDef (nextEarlestPoint, FTurnInfo.Next.Direction - 0.5*Pi, geom.AsMultiPoint.Point [0]);
			if side = sideLeft then
				notIntersected := True
			else
			begin
				notIntersected := False;
				getEdgePoints := True;
				intPoint := geom.AsMultiPoint.Point [0].Clone.AsPoint;
			end;
		end;
		geom.Free;

		if notIntersected then
		begin
			tmpLine.Part [0].Clear;
			tmpLine.Part [0].AddPoint (nextEarlestPoint);
			tmpPoint := PointAlongPlane (nextEarlestPoint, FTurnInfo.Next.Direction - 0.5*Pi*turnVal,
							4 * FTurnInfo.Next.Width);
			tmpLine.Part [0].AddPoint (tmpPoint);
			tmpPoint.Free;

			geom := GeoOper.intersect (tmpLine, edgeLine);
			if IsGeometryEmpty (geom) then
				raise EARANError.Create ('Geometry Error: Next KK line and edgeLine not intersected')
			else
			begin
				newAlpha := ReturnAngleInRadians (startPoint, geom.AsMultiPoint.Point [0]);
				tmpLine.Part [0].Clear;
				tmpLine.Part [0].AddPoint (startPoint);
				tmpPoint := PointAlongPlane (startPoint, newAlpha, tmpD);
				tmpLine.Part [0].AddPoint (tmpPoint);
				tmpPoint.Free;
				geom.Free;

				geom := GeoOper.intersect (tmpLine, edgeLine);
				if IsGeometryEmpty (geom) then
					raise EARANError.Create ('Geometry Error: New alpha degreed line and edgeLine not intersected')
				else
					intPoint := geom.AsMultiPoint.Point [0].Clone.AsPoint;
				getEdgePoints := True;
			end;
			geom.Free;
		end;

		nextEarlestPoint.Free;
	end;

	if getEdgePoints then
	begin
		result.AddPoint (startPoint);
		result.AddPoint (intPoint);

		edgeDir := ReturnAngleInRadians (edgeLine.Part [0].Point [0], edgeLine.Part [0].Point [1]);
		
		index := edgeLine.Part [0].Count;
		for i:=0 to edgeLine.Part [0].Count - 1 do
		begin
			side := SideDef (intPoint, edgeDir + 0.5*Pi, edgeLine.Part [0].Point [i]);
			if side = sideRight then
			begin
				index := i;
				break;
			end;
		end;

		for i:= index to edgeLine.Part [0].Count - 1 do
			result.AddPoint (edgeLine.Part [0].Point [i]);
	end;

	tmpLine.Free;
	intPoint.Free;
end;

function TTurn.GetOuterSpiral (
				edgeLine: TPolyline;
				radius: Double;
				spiralStartPoints: TMultiPoint;
				ATTValue: Double;
				var paralelPointIndex: Integer;
				var edgeIntersectPtIndex: Integer;
				var paralelSideByEdge: TSideDirection): TMultiPoint;
const
	windSpeed = 56.0;
var
	turnVal, i:			Integer;
	side:				TSideDirection;
	theta, bank,
	rv, coef, H,
	endDir, tmpD,
	cutWidth, TAS,
	nextSemiWidth:		Double;
	spiralPoints,
	tmpSpiralPoints:	TMultiPoint;
	part:				TPart;
	tmpPoint,
	tmpPoint2,
	intPoint:			TPoint;
	tmpLine,
	tmpLine2,
	spiralLine:			TPolyline;
	geom:				TGeometry;
	addEdgePoints:		Boolean;

	__UseConstants__:	Integer;
begin

	nextSemiWidth := ReturnDistanceInMeters (FTurnInfo.Next.Point,
						edgeLine.Part [0].Point [edgeLine.Part [0].Count - 1]);

	addEdgePoints := True;
	cutWidth := -1;

	H := FTurnInfo.PrevTurnInfo.Start.HeightProtect +
		FTurnInfo.Start.Distance * FTurnInfo.PDGProtect;

	TAS := IASToTAS (FTurnInfo.IAS, H, FParams.AHP.Temperature);

	spiralLine := TPolyline.Create;
	tmpLine := TPolyline.Create;

	part := TPart.Create;
	tmpLine.AddPart (part);
	spiralLine.AddPart (part);
	part.Free;

	turnVal := Integer (FTurnInfo.TurnSide);
	theta := FTurnInfo.TurnAngle;

	if theta < DegToRad (1) then theta := 0;
	bank := RadiusToBank (FTurnInfo.ProtectedRadius, TAS);
	rv := 6355.0 * Tan (bank) / (Pi * TAS * 3.6);
	if rv > 3.0 then
		rv := 3.0;
	coef := windSpeed / (rv * 3.6);

	if theta < (0.5 * Pi) then
		endDir := TurnInfo.Next.Direction
	else
		endDir := TurnInfo.Start.Direction - 0.5*Pi*turnVal;

	if FTurnInfo.TurnType = tuFlyBy then
	begin
		spiralPoints := CreateWindSpiral (spiralStartPoints.Point [0], FTurnInfo.Start.Direction,
							FTurnInfo.Start.Direction, FTurnInfo.Start.Direction,
							radius, coef, FTurnInfo.TurnSide);

		tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [0], FTurnInfo.Start.Direction,
							FTurnInfo.Start.Direction, endDir,
							radius, coef, FTurnInfo.TurnSide);

		if (spiralPoints.Count = 0) and (tmpSpiralPoints.Count = 0) then
			raise EARANError.Create ('Spiral points count must be greater than zero');

		tmpPoint := LineLineIntersect (spiralPoints.Point [spiralPoints.Count-1], FTurnInfo.Start.Direction, tmpSpiralPoints.Point [tmpSpiralPoints.Count - 1], endDir).AsPoint;

		spiralPoints.AddPoint (tmpPoint);
		tmpPoint.Free;
		spiralPoints.AddPoint (tmpSpiralPoints.Point [tmpSpiralPoints.Count - 1]);
		tmpSpiralPoints.Free;
	end
	else //--- FlyOver
	begin
		spiralPoints := CreateWindSpiral (spiralStartPoints.Point [0], FTurnInfo.Start.Direction,
							FTurnInfo.Start.Direction, endDir,
							radius, coef, FTurnInfo.TurnSide);

		if spiralPoints.Count = 0 then
			raise EARANError.Create ('Spiral points count must be greater than zero');
	end;

	if theta < (0.5 * Pi) then
	begin
		paralelPointIndex := spiralPoints.Count - 1;

		side := EdgeSideDef (edgeLine, spiralPoints.Point [spiralPoints.Count-1]);

		paralelSideByEdge := side;

		if Integer (side) * turnVal >= 0 then  //--- 15 degree
		begin
			tmpD := ReturnDistanceInMeters (FTurnInfo.Next.Point, spiralPoints.Point [spiralPoints.Count-1]);
			tmpPoint := PointAlongPlane (spiralPoints.Point [spiralPoints.Count-1],
								FTurnInfo.Next.Direction + (Pi/12) * turnVal, tmpD);
			tmpLine.Part [0].Clear;
			tmpLine.Part [0].AddPoint (spiralPoints.Point [spiralPoints.Count-1]);
			tmpLine.Part [0].AddPoint (tmpPoint);
			tmpPoint.Free;

			spiralLine.Part [0].Clear;
			spiralLine.Part [0].AddMultiPoint (spiralPoints);

			geom := GeoOper.intersect (tmpLine, edgeLine);
			intPoint := nil;

			if not IsGeometryEmpty (geom) then
				intPoint := geom.asMultiPoint.Point [0].Clone.AsPoint;
			geom.Free;

			tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction, -ATTValue);

			if intPoint <> nil then
				side := SideDef (tmpPoint, FTurnInfo.Next.Direction + 0.5*Pi, intPoint)
			else
				side := sideRight;

			if side = sideRight then
			begin
				tmpPoint2 := PointAlongPlane (tmpPoint, FturnInfo.Next.Direction + 0.5*Pi*turnVal, nextSemiWidth);
				intPoint.Free;
				intPoint := tmpPoint2;
			end;

			spiralPoints.AddPoint (intPoint);

			tmpPoint.Free;
			intPoint.Free;
		end
		else  // side <> sideRight   //--- 30 degree
		begin
			if theta > (Pi/3) then
			begin
				tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [0], FTurnInfo.Start.Direction,
										endDir, FTurnInfo.Start.Direction - 0.5*Pi*turnVal,
										radius, coef, FTurnInfo.TurnSide);
				spiralPoints.AddMultiPoint (tmpSpiralPoints);
				tmpSpiralPoints.Free;
				tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [1], FTurnInfo.Start.Direction,
										FTurnInfo.Start.Direction - 0.5*Pi*turnVal, FTurnInfo.Next.Direction - (Pi/6)*turnVal,
										radius, coef, FTurnInfo.TurnSide);
				spiralPoints.AddMultiPoint (tmpSpiralPoints);
				tmpSpiralPoints.Free;
			end
			else //--- theta <= (Pi/3)
			begin
				tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [0], FTurnInfo.Start.Direction,
										endDir, FTurnInfo.Next.Direction - (Pi/6)*turnVal,
										radius, coef, FTurnInfo.TurnSide);
				spiralPoints.AddMultiPoint (tmpSpiralPoints);
				tmpSpiralPoints.Free;
			end;

			if not DeleteSpiralOutsidePoints (spiralPoints, edgeLine, FTurnInfo.TurnSide) then
			begin
				tmpLine.Part [0].Clear;
				tmpLine.Part [0].AddPoint (spiralPoints.Point [spiralPoints.Count-1]);
				tmpD := ReturnDistanceInMeters (spiralPoints.Point [spiralPoints.Count-1], FTurnInfo.Next.Point);
				tmpPoint := PointAlongPlane (spiralPoints.Point [spiralPoints.Count-1],
								FTurnInfo.Next.Direction - (Pi/6)*turnVal, tmpD);
				tmpLine.Part [0].AddPoint (tmpPoint);
				tmpPoint.Free;

				geom := GeoOper.intersect (tmpLine, edgeLine);

				if IsGeometryEmpty (geom) then
				begin
					tmpLine2 := tmpLine.Clone.AsPolyline;
					tmpLine2.Part [0].Clear;
					tmpLine2.Part [0].AddPoint (FTurnInfo.Next.Point);
					tmpPoint := PointAlongPlane (FTurnInfo.Next.Point,
						FTurnInfo.Next.Direction + 0.5*Pi*turnVal, 4 * FTurnInfo.Next.Width);
					tmpLine2.Part [0].AddPoint (tmpPoint);
					tmpPoint.Free;

					spiralLine.Part [0].Clear;
					spiralLine.Part [0].AddMultiPoint (spiralPoints);
					spiralLIne.Part [0].AddPoint (tmpLine.Part [0].Point [1]);

					geom.Free;

					geom := GeoOper.intersect (spiralLine, tmpLine2);
					tmpLine2.Free;

					if IsGeometryEmpty (geom) then
						raise EARANError.Create ('Geometry Error: Nominal perpendicular and 30 degree lines not intersected');

					cutWidth := ReturnDistanceInMeters (FTurnInfo.Next.Point, geom.asMultiPoint.Point[0]);

					tmpD := FTurnInfo.Next.Direction +0.5*Pi;
					for i:= spiralPoints.Count - 1 downto 0 do
					begin
						side := SideDef (geom.asMultiPoint.Point[0], tmpD, spiralPoints.Point [i]);
						if side = sideRight then
							spiralPoints.RemoveAt (i);
					end;

					spiralPoints.AddPoint (geom.asMultiPoint.Point[0]);
					addEdgePoints := False;
				end
				else
					spiralPoints.AddPoint (geom.asMultiPoint.Point[0]);
				geom.Free;
			end;
		end;
	end
	else // theta >= (0.5 * Pi)
	begin
		tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [1], FTurnInfo.Start.Direction,
								FTurnInfo.Start.Direction - 0.5*Pi*turnVal, FTurnInfo.Next.Direction,
								radius, coef, FTurnInfo.TurnSide);
		spiralPoints.AddMultiPoint (tmpSpiralPoints);
		tmpSpiralPoints.Free;

		paralelPointIndex := spiralPoints.Count - 1;

		side := EdgeSideDef (edgeLine, spiralPoints.Point [spiralPoints.Count-1]);

		paralelSideByEdge := side;
		
		if Integer (side) * turnVal >= 0 then //--- 15 degree
		begin
			tmpD := ReturnDistanceInMeters (FTurnInfo.Next.Point, spiralPoints.Point [spiralPoints.Count-1]);
			tmpPoint := PointAlongPlane (spiralPoints.Point [spiralPoints.Count-1],
								FTurnInfo.Next.Direction + (Pi/12) * turnVal, tmpD);

			tmpLine.Part [0].Clear;
			tmpLine.Part [0].AddPoint (spiralPoints.Point [spiralPoints.Count-1]);
			tmpLine.Part [0].AddPoint (tmpPoint);
			tmpPoint.Free;

			spiralLine.Part [0].Clear;
			spiralLine.Part [0].AddMultiPoint (spiralPoints);

			geom := GeoOper.intersect (tmpLine, edgeLine);
			intPoint := nil;

			if not IsGeometryEmpty (geom) then
				intPoint := geom.asMultiPoint.Point [0].Clone.AsPoint;
			geom.Free;

			tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction, -ATTValue);

			if intPoint <> nil then
				side := SideDef (tmpPoint, FTurnInfo.Next.Direction + 0.5*Pi, intPoint)
			else
				side := sideRight;

			if side = sideRight then
			begin
				tmpPoint2 := PointAlongPlane (tmpPoint, FturnInfo.Next.Direction + 0.5*Pi*turnVal, nextSemiWidth);
				intPoint.Free;
				intPoint := tmpPoint2;
			end;

			spiralPoints.AddPoint (intPoint);

			tmpPoint.Free;
			intPoint.Free;
		end
		else //--- side <> sideRight  //--- 30 degree
		begin
			tmpSpiralPoints := CreateWindSpiral (spiralStartPoints.Point [1], FTurnInfo.Start.Direction,
									FTurnInfo.Next.Direction, FTurnInfo.Next.Direction - (Pi/6)*turnVal,
									radius, coef, FTurnInfo.TurnSide);
			spiralPoints.AddMultiPoint (tmpSpiralPoints);
			tmpSpiralPoints.Free;

			if not DeleteSpiralOutsidePoints (spiralPoints, edgeLine, FTurnInfo.TurnSide) then
			begin
				tmpLine.Part [0].Clear;
				tmpLine.Part [0].AddPoint (spiralPoints.Point [spiralPoints.Count-1]);
				tmpD := ReturnDistanceInMeters (spiralPoints.Point [spiralPoints.Count-1], FTurnInfo.Next.Point);
				tmpPoint := PointAlongPlane (spiralPoints.Point [spiralPoints.Count-1],
								FTurnInfo.Next.Direction - (Pi/6)*turnVal, tmpD);
				tmpLine.Part [0].AddPoint (tmpPoint);
				tmpPoint.Free;

				geom := GeoOper.intersect (tmpLine, edgeLine);

				if IsGeometryEmpty (geom) then
				begin
					tmpLine2 := tmpLine.Clone.AsPolyline;
					tmpLine2.Part [0].Clear;
					tmpLine2.Part [0].AddPoint (FTurnInfo.Next.Point);
					tmpPoint := PointAlongPlane (FTurnInfo.Next.Point, FTurnInfo.Next.Direction +0.5*Pi*turnVal, 4 * FTurnInfo.Next.Width);
					tmpLine2.Part [0].AddPoint (tmpPoint);
					tmpPoint.Free;

					spiralLine.Part [0].Clear;
					spiralLine.Part [0].AddMultiPoint (spiralPoints);
					spiralLIne.Part [0].AddPoint (tmpLine.Part [0].Point [1]);

					geom.Free;
					geom := GeoOper.intersect (spiralLine, tmpLine2);

					tmpLine2.Free;

					if IsGeometryEmpty (geom) then
						raise EARANError.Create ('Geometry Error: Nominal perpendicular and 30 degree lines not intersected');

					cutWidth := ReturnDistanceInMeters (FTurnInfo.Next.Point, geom.asMultiPoint.Point[0]);

					tmpD := FTurnInfo.Next.Direction +0.5*Pi;
					for i:= spiralPoints.Count - 1 downto 0 do
					begin
						side := SideDef (geom.asMultiPoint.Point[0], tmpD, spiralPoints.Point [i]);
						if side = sideRight then
							spiralPoints.RemoveAt (i);
					end;

					spiralPoints.AddPoint (geom.asMultiPoint.Point[0]);
					addEdgePoints := False;
				end
				else
					spiralPoints.AddPoint (geom.asMultiPoint.Point[0]);
				geom.Free;
			end;
		end;
	end;

	if cutWidth <> -1 then
	begin
		FTurnInfo.Next.Width := cutWidth;
	end;

	if addEdgePoints then
	begin
		edgeIntersectPtIndex := spiralPoints.Count - 1;
		
		tmpD := ReturnAngleInRadians (edgeLine.Part [0].Point[0], edgeLine.Part [0].Point [1]);
		tmpPoint := spiralPoints.Point [spiralPoints.Count - 1].Clone.AsPoint;
		for i:= 1 to edgeLine.Part [0].Count - 1 do
		begin
			side := SideDef (tmpPoint, tmpD + 0.5*Pi, edgeLine.Part [0].Point [i]);
			if side = sideRight then
				spiralPoints.AddPoint (edgeLine.Part [0].Point [i]);
		end;
	end
	else
		edgeIntersectPtIndex := spiralPoints.Count;

	tmpLine.Free;
	spiralLine.Free;

	result := spiralPoints;
end;

procedure TTurn.CreateSegment (edgePoly: TPolyline; ATTValue: Double);
const
	widthInDER = 150.0;
var
	turnVal, i,
	primOutParalelPointIndex,
	primOutEdgeIntersectPtIndex,
	addOutParalelPointIndex,
	addOutEdgeIntersectPtIndex,
	addStartIndex,
	primEndIndex:				Integer;
	side,
	primOutParalelSideByEdge,
	addOutParalelSideByEdge:	TSideDirection;
	dir, radius, outerWidth:	Double;
	primOutSpiralPoints,
	addOutSpiralPoints,
	primeInnerPoints,
	additionInnerPoints,
	basePoints:					TMultiPoint;
	part:						TPart;
	tmpPoint, intPoint,
	primaryInnerStartPoint,
	additionInnerStartPoint:	TPoint;
	primaryOuterEdge,
	primaryInnerEdge,
	additionOuterEdge,
	additionInnerEdge,
	tmpLine,
	additionOuterLine,
	additionInnerLine,
	primaryOuterLine,
	primaryInnerLine:			TPolyline;
	nextPointIsBottomThanKK:	Boolean;
	innerStartPointSide,
	outerStartPointSide:		TSideDirection;
	additionOuterArea,
	primaryArea,
	additionInnerArea:			TPolygon;
	ring:						TRing;
begin

	turnVal := Integer (FTurnInfo.TurnSide);

	if FTurnInfo.TurnType = tuFlyBy then
		radius := FTurnInfo.FlyBySpiralRadius
	else
		radius := FTurnInfo.ProtectedRadius;

	additionOuterLine := TPolyline.Create;
	additionInnerLine := TPolyline.Create;
	primaryOuterLine := TPolyline.Create;
	primaryInnerLine := TPolyline.Create;

	tmpLine := TPolyline.Create;
	part := TPart.Create;
	tmpLine.AddPart(part);

	primaryOuterLine.AddPart (part);
	primaryInnerLine.AddPart (part);
	additionOuterLine.AddPart (part);
	additionInnerLine.AddPart (part);

	part.Free;

	primaryOuterEdge := TPolyline.Create;
	primaryInnerEdge := TPolyline.Create;
	additionOuterEdge := TPolyline.Create;
	additionInnerEdge := TPolyline.Create;

	additionOuterEdge.AddPart (edgePoly.Part [0]);
	primaryOuterEdge.AddPart (edgePoly.Part [1]);
	primaryInnerEdge.AddPart (edgePoly.Part [2]);
	additionInnerEdge.AddPart (edgePoly.Part [3]);

	basePoints := TMultiPoint.Create;
	basePoints.AddPoint (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0]);
	basePoints.AddPoint (FTurnInfo.Start.LatestTolerance.PrimaryPoint [2]);

	//--- begin: Get primary outer spiral points.
	primOutSpiralPoints := GetOuterSpiral (primaryOuterEdge, radius, basePoints, ATTValue,
			primOutParalelPointIndex, primOutEdgeIntersectPtIndex, primOutParalelSideByEdge);
	//--- end.

	outerWidth := 0;
	if FTurnInfo.Start.LatestTolerance.OuterWidth > 0 then // prev segment has additon outer area.
	begin
		radius := radius + FTurnInfo.Start.LatestTolerance.OuterWidth;
		dir := ReturnAngleInRadians (FTurnInfo.Start.LatestTolerance.PrimaryPoint [0],
					FTurnInfo.Start.LatestTolerance.PrimaryPoint [1]);
		tmpPoint := PointAlongPlane (basePoints.Point [0], dir, -FTurnInfo.Start.LatestTolerance.OuterWidth);
		basePoints.Point [0].Assign (tmpPoint);
		tmpPoint.Free;
		tmpPoint := PointAlongPlane (basePoints.Point [1], dir, -FTurnInfo.Start.LatestTolerance.OuterWidth);
		basePoints.Point [1].Assign (tmpPoint);
		tmpPoint.Free;

		outerWidth := FTurnInfo.Start.LatestTolerance.OuterWidth;
	end;

	//--- begin: Get addition outer spiral points.
	addOutSpiralPoints := GetOuterSpiral (additionOuterEdge, radius, basePoints, ATTValue,
		addOutParalelPointIndex, addOutEdgeIntersectPtIndex, addOutParalelSideByEdge);
	//--- end.

	if FTurnInfo.Start.LatestTolerance.OuterWidth > 0 then // prev segment has additon outer area.
	begin
		addStartIndex := 0;
		primEndIndex := 0;
	end
	else
	begin
		if Integer (addOutParalelSideByEdge) * Integer (primOutParalelSideByEdge) < 0 then
		begin
			addStartIndex := addOutParalelPointIndex;
			primEndIndex := primOutParalelPointIndex + 1;
		end
		else
		begin
			addStartIndex := addOutEdgeIntersectPtIndex;
			primEndIndex := primOutEdgeIntersectPtIndex;
		end;
	end;

	additionOuterArea := TPolygon.Create;
	primaryArea := TPolygon.Create;
	additionInnerArea := TPolygon.Create;

	ring := TRing.Create;
	additionOuterArea.AddRing (ring);
	primaryArea.AddRing (ring);
	additionInnerArea.AddRing (ring);
	ring.Free;

	//--- begin: Create Addition Outer Area.
	for i := addStartIndex to addOutSpiralPoints.Count - 1 do
	begin
		additionOuterArea.Ring [0].AddPoint (addOutSpiralPoints.Point [i]);
		additionOuterLine.Part [0].AddPoint (addOutSpiralPoints.Point [i]);
	end;
	for i := primOutSpiralPoints.Count - 1 downto primEndIndex do
		additionOuterArea.Ring [0].AddPoint (primOutSpiralPoints.Point [i]);
	//--- end.

	//--- begin: Get inner points.
	dir := ReturnAngleInRadians(FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0],
				FTurnInfo.Start.EarlestTolerance.PrimaryPoint [1]);
	side := SideDef (FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0], dir, FTurnInfo.Next.Point);
	nextPointIsBottomThanKK := IsSameSide (side, FTurnInfo.TurnSide);

	if nextPointIsBottomThanKK then
	begin
		primaryInnerStartPoint := FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0];
		additionInnerStartPoint := FTurnInfo.Start.EarlestTolerance.OuterPoint
	end
	else
	begin
		primaryInnerStartPoint := FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2];
		additionInnerStartPoint := FTurnInfo.Start.EarlestTolerance.InnerPoint
	end;

	FTurnInfo.InnerStartPoint.Assign (additionInnerStartPoint);

	primeInnerPoints := GetInnerAddArea (primaryInnerEdge, primaryInnerStartPoint, innerStartPointSide, ATTValue);
	additionInnerPoints := GetInnerAddArea (additionInnerEdge, additionInnerStartPoint, outerStartPointSide, ATTValue);

	if CheckInnersIntersect (primeInnerPoints, additionInnerPoints) then
	begin
		primeInnerPoints.Free;
		additionInnerPoints.Free;

		primeInnerPoints := GetInnerAddArea (primaryInnerEdge, additionInnerStartPoint, innerStartPointSide, ATTValue);
//		primeInnerPoints.InsertPoint (0, FTurnInfo.Start.EarlestTolerance.PrimaryPoint [2]);
		if primeInnerPoints.Count > 0 then
			primeInnerPoints.Point [0].Assign (primaryInnerStartPoint);
		additionInnerPoints := GetInnerAddArea (additionInnerEdge, primaryInnerStartPoint, outerStartPointSide, ATTValue);
	end;

	//if prev segment has not inner area and inner lines are same side by edge.
	if (not FTurnInfo.Start.EarlestTolerance.HasInner) and
			IsSameSide (innerStartPointSide, outerStartPointSide) then
	begin
		addStartIndex := 1;
		primEndIndex := 1;
	end
	else
	begin
		addStartIndex := 0;
		primEndIndex := 0;
	end;
	//--- end.

	//--- begin: Create Primary Area.
	primaryArea.Ring [0].AddMultiPoint (primOutSpiralPoints);
	primaryOuterLine.Part [0].AddMultiPoint (primOutSpiralPoints);
	primaryArea.Ring [0].AddMultiPointInverse (primeInnerPoints);
	primaryInnerLine.Part [0].AddMultiPointInverse (primeInnerPoints);

	//--- --- begin: Add straight area extend point. ExpandPoints [0] is left and [1] is right.
//	if (FTurnInfo.TurnType = tuAtHeight) and (FStraight.ExpandPoints.Count > 0) then
	if FStraight.ExpandPoints.Count > 0 then
		primaryArea.Ring [0].AddPoint (FStraight.ExpandPoints.Point [Round (0.5*turnVal - 0.5) * -1]);
	//--- --- end.

	//--- end.

	//--- begin: Create Inner Addition Area.
	additionInnerLine.Part [0].AddPoint (
		primaryInnerLine.Part [0].Point [primaryInnerLine.Part [0].Count - 2]);

	for i := addStartIndex to additionInnerPoints.Count - 1 do
	begin
		additionInnerArea.Ring [0].AddPoint (additionInnerPoints.Point [i]);
		additionInnerLine.Part [0].AddPoint (additionInnerPoints.Point [i]);
	end;

	for i := primeInnerPoints.Count - 1 downto primEndIndex do
		additionInnerArea.Ring [0].AddPoint (primeInnerPoints.Point [i]);
	//--- end.

	//JoinAreaToPrev (additionOuterLine, primaryOuterLine, addAreaIndexOnPrimOuter);

	if FTurnInfo.TurnSide = sideRight then
	begin
		FTurnInfo.Segment.LinesForMOCCoef.Part [0].Assign (additionOuterLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [1].Assign (primaryOuterLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [2].Assign (primaryInnerLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [3].Assign (additionInnerLine.Part [0]);

		FTurnInfo.Segment.Area.LeftArea.Assign (additionOuterArea);
		FTurnInfo.Segment.Area.RightArea.Assign (additionInnerArea);
	end
	else
	begin
		FTurnInfo.Segment.LinesForMOCCoef.Part [0].Assign (additionInnerLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [1].Assign (primaryInnerLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [2].Assign (primaryOuterLine.Part [0]);
		FTurnInfo.Segment.LinesForMOCCoef.Part [3].Assign (additionOuterLine.Part [0]);

		FTurnInfo.Segment.Area.LeftArea.Assign (additionInnerArea);
		FTurnInfo.Segment.Area.RightArea.Assign (additionOuterArea);
	end;

	FTurnInfo.Segment.Area.PrimaryArea.Assign (primaryArea);

	basePoints.Free;

	primaryOuterEdge.Free;
	primaryInnerEdge.Free;
	additionOuterEdge.Free;
	additionInnerEdge.Free;

	additionOuterLine.Free;
	additionInnerLine.Free;
	primaryOuterLine.Free;
	primaryInnerLine.Free;

	primOutSpiralPoints.Free;
	addOutSpiralPoints.Free;

	primeInnerPoints.Free;
	additionInnerPoints.Free;

	additionOuterArea.Free;
	primaryArea.Free;
	additionInnerArea.Free;

	tmpLine.Free;
end;

procedure TTurn.JoinAreaToPrev (
					additionOuterLine,
					primaryOuterLine: TPolyline;
					var addAreaIndexOnPrimOuter: Integer);
const
	AHPCirleR = 56000.0; splayAngle = 15.0 * Pi/180.0;
var
	i,
	turnVal:			Integer;
	addOutPoints,
	primOutPoints:		TMultiPoint;
	intersectPoint,
	intersectPoint2,
	latestCentrePoint,
	tmpPoint:			TPoint;
	dir, d,
	startOuterWidth,
	extendedEdgeLen,
	startOuterSemiWidth,
	startInnerSemiWidth,
	distS, distL,
	startInnerWidth: 	Double;
	line:           	TLine;
	sideS, sideL:		TSideDirection;
begin
	turnVal := Integer (FTurnInfo.TurnSide);
	extendedEdgeLen := (GConst.GNSS.Constant [DEP_].SemiWidth - GConst.GNSS.Constant [IDEP_].SemiWidth) / Tan (splayAngle);

	addOutPoints := TMultiPoint.Create;
	primOutPoints := TMultiPoint.Create;

	latestCentrePoint := FTurnInfo.Start.LatestTolerance.PrimaryPoint [1];

	startOuterWidth := FTurnInfo.Start.NominalTolerance.OuterWidth;
	startInnerWidth := FTurnInfo.Start.NominalTolerance.InnerWidth;

	startOuterSemiWidth := FTurnInfo.Start.NominalTolerance.PrimOuterWidth + startOuterWidth;
	startInnerSemiWidth := FTurnInfo.Start.NominalTolerance.PrimInnerWidth + startInnerWidth;

	if not FTurnInfo.Start.LatestTolerance.HasOuter then
	begin
		addAreaIndexOnPrimOuter := 1;
		
		tmpPoint := PointAlongPlane (FTurnInfo.Start.Point,
			FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterSemiWidth);
		addOutPoints.AddPoint (tmpPoint);
		tmpPoint.Free;

		tmpPoint := PointAlongPlane (FTurnInfo.Start.Point,
			FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterWidth);
		primOutPoints.AddPoint (tmpPoint);
		tmpPoint.Free;
	end
	else
	begin
		if startOuterWidth = 0 then
		begin
			dir := ReturnAngleInRadians (
					FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0],
					FTurnInfo.Start.LatestTolerance.OuterPoint);
			intersectPoint := LineLineIntersect (
								FTurnInfo.Start.EarlestTolerance.PrimaryPoint [0],
								dir,
								FTurnInfo.Start.LatestTolerance.PrimaryPoint [0],
								FTurnInfo.Start.Direction).AsPoint;
			addOutPoints.AddPoint (intersectPoint);

			tmpPoint := PointAlongPlane (FTurnInfo.Start.Point,
				FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterWidth);
			primOutPoints.AddPoint (tmpPoint);
			tmpPoint.Free;
						
			primOutPoints.AddPoint (intersectPoint);

			intersectPoint.Free;
			addAreaIndexOnPrimOuter := 2;
		end
		else
		begin
			tmpPoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterSemiWidth);
			addOutPoints.AddPoint (tmpPoint);
			tmpPoint.Free;

			tmpPoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterWidth);
			primOutPoints.AddPoint (tmpPoint);
			tmpPoint.Free;

			line := TLine.Create (FTurnInfo.Start.Point, FTurnInfo.Start.Direction);
			intersectPoint := CircleVectorIntersect (FParams.AHP.Prj, AHPCirleR, line, d);
			line.Free;

			distS := ReturnDistanceInMeters (FTurnInfo.Start.Point, FParams.AHP.Prj);
			if distS < AHPCirleR then
			begin
				distL := ReturnDistanceInMeters (latestCentrePoint, FParams.AHP.Prj);
				if distL > AHPCirleR then
				begin
					tmpPoint := PointAlongPlane (intersectPoint, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterSemiWidth);
					addOutPoints.AddPoint (tmpPoint);
					tmpPoint.Free;

					tmpPoint := PointAlongPlane (intersectPoint, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterWidth);
					primOutPoints.AddPoint (tmpPoint);
					tmpPoint.Free;
				end;
			end
			else
			begin
				intersectPoint2 := PointAlongPlane (intersectPoint, FTurnInfo.Start.Direction, extendedEdgeLen);
				
				sideS := SideDef (intersectPoint2, FTurnInfo.Start.Direction + 0.5*Pi, FTurnInfo.Start.Point);
				sideL := sideDef (intersectPoint2, FTurnInfo.Start.Direction + 0.5*Pi, latestCentrePoint);

				if sideS <> sideL then
				begin
					tmpPoint := PointAlongPlane (intersectPoint2, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startOuterSemiWidth);
					addOutPoints.AddPoint (tmpPoint);
					tmpPoint.Free;

					tmpPoint := PointAlongPlane (intersectPoint2, FTurnInfo.Start.Direction + 0.5*Pi*turnVal, startInnerSemiWidth);
					addOutPoints.AddPoint (tmpPoint);
					tmpPoint.Free;
				end;
				intersectPoint2.Free;
			end;
			intersectPoint.Free;
		end;
	end;

	for i:=0 to addOutPoints.Count - 1 do
		additionOuterLine.Part [0].InsertPoint (i, addOutPoints.Point [i]);
	for i:=0 to primOutPoints.Count - 1 do
		primaryOuterLine.Part [0].InsertPoint (i, primOutPoints.Point [i]);

	addOutPoints.Free;
	primOutPoints.Free;
end;

//-----------------
//--- TSegmentPoint
//-----------------

constructor TSegmentPoint.Create;
begin
	inherited;
	FPoint := TPoint.Create;
	Clear;
end;

destructor TSegmentPoint.Destroy;
begin
	inherited;
	FPoint.Free;
end;

procedure TSegmentPoint.Assign (src: TSegmentPoint);
begin
	FPoint := src.FPoint;
	FSignificantPointID := src.FSignificantPointID;
	FDirection := src.Direction;
	FDistance := src.Distance;
	FATT := src.FATT;
end;

function TSegmentPoint.Clone (): TSegmentPoint;
begin
	result := TSegmentPoint.Create;
	result.Assign (self);
end;

procedure TSegmentPoint.Clear ();
begin
	FPoint.SetEmpty;
	FSignificantPointID := '';
	FDirection := 0.0;
	FDistance := 0.0;
	FATT := 0.0;	
end;

//--------------
//--- TNextPoint
//--------------

constructor TNextPoint.Create;
begin
	inherited;
end;

destructor TNextPoint.Destroy;
begin
	inherited;
end;

procedure TNextPoint.Assign (src: TSegmentPoint);
begin
	inherited;
	FLatestDirection := TNextPoint (src).FLatestDirection;
	FEarlestDirection := TNextPoint (src).FEarlestDirection;
	FWidth := TNextPoint (src).FWidth;
end;

function TNextPoint.Clone (): TSegmentPoint;
begin
	result := TNextPoint.Create;
	result.Assign (self);
end;

procedure TNextPoint.Clear ();
begin
	inherited;
	FLatestDirection := 0.0;
	FEarlestDirection := 0.0;
	FWidth := 0.0;
end;

//---------------
//--- TStartPoint
//---------------

constructor TStartPoint.Create;
begin
	FEarlestTolerance := TToleranceLineInfo.Create;
	FNominalTolerance := TToleranceLineInfo.Create;
	FLatestTolerance :=	TToleranceLineInfo.Create;
	inherited;
end;

destructor TStartPoint.Destroy;
begin
	inherited;
	FEarlestTolerance.Free;
	FNominalTolerance.Free;
	FLatestTolerance.Free;
end;

procedure TStartPoint.Assign (src: TSegmentPoint);
var
	source:	TStartPoint;
begin
	inherited;
	source := TStartPoint (src);
	
	FEarlestTolerance.Assign (source.FEarlestTolerance);
	FNominalTolerance.Assign (source.FNominalTolerance);
	FLatestTolerance.Assign (source.FLatestTolerance);
	FETP := source.FETP;
	FLTP := source.FLTP;
	FXTT := source.FXTT;
	FHeightNominal := source.FHeightNominal;
	FHeightProtect := source.FHeightProtect;
    FHeightToKK := source.FHeightToKK;
    FDistanceToKK := source.FDistanceToKK;
end;

function TStartPoint.Clone () : TSegmentPoint;
begin
	result := TStartPoint.Create;
	result.Assign (self);
end;

procedure TStartPoint.Clear ();
begin
	inherited;
	FEarlestTolerance.Clear;
	FNominalTolerance.Clear;
	FLatestTolerance.Clear;
	FETP := 0.0;
	FLTP := 0.0;
	FXTT := 0.0;
	FHeightNominal := 0.0;
	FHeightProtect := 0.0;
    FHeightToKK := 0.0;
    FDistanceToKK := 0.0;
end;

//----------------------
//--- TTurnInfo
//----------------------

constructor TTurnInfo.Create;
begin
	inherited;
	FStart := TStartPoint.Create;
	FNext := TNextPoint.Create;
	FSegment := TProtectedSegment.Create;
	FObstacles := TObstacleInAreaList.Create;
	FInnerStartPoint := TPoint.Create;
end;

destructor TTurnInfo.Destroy;
begin
	FStart.Free;
	FNext.Free;
	FSegment.Free;
	FObstacles.Free;
	FInnerStartPoint.Free;
	inherited;
end;

procedure TTurnInfo.Clear;
begin
	FStart.Clear;
	FNext.Clear;
	MaxRequiredGrdIndex := 0;
	FNominalRadius := 0.0;
	FProtectedRadius := 0.0;
	FFlyBySpiralRadius := 0.0;
	FIAS := 0.0;
	FBankAngle := 0.0;
	FPDG := 0.0;
	FPDGNominal := 0.0;
	FPDGProtect := 0.0;
	FTurnAngle := 0.0;

	FAircraftCategory := acA;
	FPrevDFSideOn := False;
	FTurnType := tuFlyBy;
	FCodeType := ctTF;
	FTurnSide := sideRight;
	FSegment.Clear;
	FObstacles.Clear;
	FPrevTurnInfo := nil;

	tmp_codeTypeIsCF := False;
end;

procedure TTurnInfo.Assign(src: TTurnInfo);
begin
	FStart.Assign (src.Start);
	FNext.Assign (src.Next);
	FSegment.Assign (src.Segment);
	FNominalRadius := src.NominalRadius;
	FProtectedRadius := src.ProtectedRadius;
	FFlyBySpiralRadius := src.FlyBySpiralRadius;
	FTurnType := src.TurnType;
	FIAS := src.FIAS;
	FBankAngle := src.FBankAngle;
	FPDG := src.FPDG;
	FPDGNominal := src.FPDGNominal;
	FPDGProtect := src.FPDGProtect;
	FPDG := src.FPDG;
	FCodeType := src.CodeType;
	FTurnSide := src.TurnSide;
	FTurnAngle := src.TurnAngle;
	FPrevDFSideOn := src.PrevDFSideOn;
	MaxRequiredGrdIndex := src.MaxRequiredGrdIndex;
	FAircraftCategory := src.FAircraftCategory;
end;

function TTurnInfo.Clone (): TTurnInfo;
begin
	result := TTurnInfo.Create();
	result.Assign (self);
end;

//-------------------------------
//--- TToleranceLineInfo
//-------------------------------

procedure TToleranceLineInfo.Assign(src: TToleranceLineInfo);
var
	i:	Integer;
begin
	for i := 0 to Length (FIndexes) do
	begin
		FIndexes [i] := src.FIndexes [i];
		FWidths [i] := src.FWidths [i];
	end;

	FTolerancePoints.Assign (src.FTolerancePoints);
	FSideDirection := src.FSideDirection;
end;

function TToleranceLineInfo.Clone: TToleranceLineInfo;
begin
	result := TToleranceLineInfo.Create;
	result.Assign (self);
end;


constructor TToleranceLineInfo.Create;
var
	i:			Integer;
	tmpPoint:	TPoint;
begin
	inherited;
	FPointCount := 5;
	FTolerancePoints := TMultiPoint.Create;
	tmpPoint := TPoint.Create;
	for i:=0 to FPointCount - 1 do
	begin
		FTolerancePoints.AddPoint (tmpPoint);
		FIndexes [i] := i;
		FWidths [i] := 0;
	end;
	tmpPoint.Free;
	FSideDirection := sideRight;
end;

destructor TToleranceLineInfo.Destroy;
begin
	inherited;
	FTolerancePoints.Free;
end;

procedure TToleranceLineInfo.SetSideDirection (vSideDirection: TSideDirection);
var
	i, n, len, tmpIndex:	Integer;
begin
	if FSideDirection = vSideDirection then
		exit;

	len := Length (FIndexes);
	n := Round (len / 2);

	for i:=0 to n - 1 do
	begin
		tmpIndex := FIndexes [i];
		FIndexes [i] := FIndexes [len - i - 1];
		FIndexes [len - i - 1] := tmpIndex;
	end;
end;

function TToleranceLineInfo.GetInnerPoint (): TPoint;
begin
	result := FTolerancePoints.Point [FIndexes [4]];
end;

function TToleranceLineInfo.GetOuterPoint (): TPoint;
begin
	result := FTolerancePoints.Point [FIndexes [0]];
end;

function TToleranceLineInfo.GetPrimaryPoint (index: Integer): TPoint;
begin
	if (index < 0) or (index > 2) then
		raise EARANError.Create ('index must be in [0, 2]', mtError);
	result := FTolerancePoints.Point [FIndexes [index + 1]];
end;

function TToleranceLineInfo.GetHasOuter: Boolean;
begin
	result := (GetOuterWidth () > 0);
end;

function TToleranceLineInfo.GetHasInner: Boolean;
begin
	result := (GetInnerWidth () > 0);
end;

function TToleranceLineInfo.GetInnerWidth: Double;
begin
	result := FWidths [FIndexes [4]];
end;

function TToleranceLineInfo.GetTolerancePoint (index: Integer): TPoint;
begin
	result := FTolerancePoints.Point [index];
end;

function TToleranceLineInfo.GetOuterWidth: Double;
begin
	result := FWidths [FIndexes [0]];
end;

function TToleranceLineInfo.GetPrimInnerWidth: Double;
begin
	result := FWidths [FIndexes [3]];
end;

function TToleranceLineInfo.GetPrimOuterWidth: Double;
begin
	result := FWidths [FIndexes [1]];
end;


procedure TToleranceLineInfo.SetAccuracyLine (middlePoint: TPoint; nominalDir: Double; cutLine: TPolyline; area: TProtectedArea);
var
	i, index0,
	index1:			Integer;
	geom:			TGeometry;
    mp,
	multiPoint:		TMultiPoint;
	part:			TPart;
	startPoint,
	nominalPoint:	TPoint;
	side:			TSideDirection;
	w:              Double;
begin
	w := GConst.GNSS.Constant [IDEP_].SemiWidth;

	startPoint := area.NominalLine.Part [0].Point [0];

	for i:=0 to Length (FWidths) - 1 do
		FWidths [i] := 0.0;

	geom := GeoOper.intersect (cutLine, area.PrimaryArea);

    if not IsGeometryEmpty (geom) then
	begin
        multiPoint := SortPoints (cutLine.Part [0].Point [0], geom.AsPolyline.Part [0].AsMultiPoint);
		FTolerancePoints.Point [1].Assign (multiPoint.Point [0]);
		FTolerancePoints.Point [2].Assign (middlePoint);
		FTolerancePoints.Point [3].Assign (multiPoint.Point [1]);
        multiPoint.Free;
    end
	else
		raise EARANError.Create ('Primary Area and Tolerance Line not intersected', mtError);
    geom.Free;

	geom := GeoOper.intersect (cutLine, area.LeftArea);
	if not IsGeometryEmpty (geom) then
	begin
        multiPoint := SortPoints (cutLine.Part [0].Point [0], geom.AsPolyline.Part [0].AsMultiPoint);
		FTolerancePoints.Point [0].Assign (multiPoint.Point [0]);
        multiPoint.Free;
	end
	else
	begin
		FTolerancePoints.Point [0].Assign (FTolerancePoints.Point [1]);
		FWidths [0] := -1;
	end;
	geom.Free;

	geom := GeoOper.intersect (cutLine, area.RightArea);
	if not IsGeometryEmpty (geom) then
	begin
        multiPoint := SortPoints (cutLine.Part [0].Point [0], geom.AsPolyline.Part [0].AsMultiPoint);
		FTolerancePoints.Point [4].Assign (multiPoint.Point [1]);
        multiPoint.Free;
	end
	else
	begin
		FTolerancePoints.Point [4].Assign (FTolerancePoints.Point [3]);
		FWidths [4] := -1;
	end;
	geom.Free;

	index0 := 0;
	for i:=0 to FTolerancePoints.Count - 2 do
	begin
		if FWidths [index0] = -1 then
			FWidths [index0] := 0
		else
			FWidths [index0] := ReturnDistanceInMeters (FTolerancePoints.Point [i], FTolerancePoints.Point [i+1]);
		index0 := index0 + 1;
		if index0 = 2 then index0 := 3;
	end;

	side := FSideDirection;
	FSideDirection := sideRight;
	for i:=0 to FPointCount-1 do
		FIndexes [i] := i;
	SetSideDirection (side);

end;

procedure TToleranceLineInfo.Clear;
var
	i:	Integer;
begin
	for i:=0 to FPointCount - 1 do
	begin
		FIndexes [i] := i;
		FWidths [i] := 0.0;
		FTolerancePoints.Point [i].SetEmpty;
	end;
	FSideDirection := sideRight;
end;

//-------------------------------
//--- TTurnInfoList
//-------------------------------

procedure TTurnInfoList.Add(value: TTurnInfo);
begin
	FTurnInfoList.Add (value.Clone);
end;

procedure TTurnInfoList.Remove (index: Integer);
begin
	FTurnInfoList.Delete (index);
end;

procedure TTurnInfoList.Clear;
begin
	FTurnInfoList.Clear;
end;

constructor TTurnInfoList.Create;
begin
	FTurnInfoList := TList.Create;
end;

destructor TTurnInfoList.Destroy;
var
	i:	Integer;
begin
	for i:=0 to FTurnInfoList.Count - 1 do
		GetItem (i).Free;
		
	FTurnInfoList.Free;
	inherited;
end;

function TTurnInfoList.GetCount: Integer;
begin
	result := FTurnInfoList.Count;
end;

function TTurnInfoList.GetItem(index: Integer): TTurnInfo;
begin
	result := TTurnInfo (FTurnInfoList.Items [index]);
end;

//------------------
//--- TSegmentSymbol
//------------------

constructor TSegmentSymbol.Create(aUI: TUIContract);
begin
	inherited Create;

	FUI := aUI;

	FAdditionGraphicHandle := -1;
	FPrimaryGraphicHandle := -1;
	FCLGraphicHandle := -1;
	FNominalGraphicHandle := -1;
	FEarlestLineHandle := -1;
	FLatestLineHandle := -1;

	FSymbolPrimary := TFillSymbol.Create;
	FSymbolAddition := TFillsymbol.Create;
	FSymbolCl := TLineSymbol.Create;
	FSymbolNominal := TLineSymbol.Create;
	FSymbolEarlestLine := TLineSymbol.Create;
	FSymbolLatestLine := TLineSymbol.Create;
end;

procedure TSegmentSymbol.DeleteGraphic(var handle: Integer);
begin
	if handle = -1 then
		exit;
	FUi.DeleteGraphic (handle);
	handle := -1;
end;

destructor TSegmentSymbol.Destroy;
begin
	inherited;
	FSymbolPrimary.Free;
	FSymbolAddition.Free;
	FSymbolCl.Free;
	FSymbolNominal.Free;
	FSymbolEarlestLine.Free;
	FSymbolLatestLine.Free;
end;

procedure TSegmentSymbol.DrawArea (protectArea: TProtectedArea;
                earlestToleranceLine, latestToleranceLine: TPolyline);
var
	additionArea:	TPolygon;
	ring:			TRing;
begin
	additionArea := TPolygon.Create;

    if protectArea.LeftArea.Count > 0 then
    begin
		if protectArea.LeftArea.Count = 1 then
			ring := protectArea.LeftArea.Ring [0]
		else
		begin
			if protectArea.LeftArea.Ring [0].Count > protectArea.LeftArea.Ring [1].Count then
				ring := protectArea.LeftArea.Ring [0]
			else
				ring := protectArea.LeftArea.Ring [1];
		end;
		additionArea.AddRing (ring);
    end;

    if protectArea.RightArea.Count > 0 then
    begin
		if protectArea.RightArea.Count = 1 then
			ring := protectArea.RightArea.Ring [0]
		else
		begin
			if protectArea.RightArea.Ring [0].Count > protectArea.RightArea.Ring [1].Count then
				ring := protectArea.RightArea.Ring [0]
			else
				ring := protectArea.RightArea.Ring [1];
		end;
		additionArea.AddRing (ring);
    end;

	FAdditionGraphicHandle := FUi.DrawPolygon (additionArea, SymbolAddition);
	FPrimaryGraphicHandle := FUi.DrawPolygon (protectArea.PrimaryArea, SymbolPrimary);
	FNominalGraphicHandle := FUi.DrawPolyline (protectArea.NominalLine, SymbolNominal);
	if Assigned (earlestToleranceLine) then
		FEarlestLineHandle := FUi.DrawPolyline (earlestToleranceLine, FSymbolEarlestLine);
	if Assigned (latestToleranceLine) then
		FLatestLineHandle := FUi.DrawPolyline (latestToleranceLine, FSymbolLatestLine);

	additionArea.Free;
end;

procedure TSegmentSymbol.DrawArea3 (protectArea: TProtectedArea;
                earlestToleranceLine, latestToleranceLine: TPolyline);
var
    i:              Integer;
	additionArea:	TPolygon;
begin
	additionArea := TPolygon.Create;
    
    if not IsGeometryEmpty (protectArea.LeftArea) then
    begin
        for i:=0 to protectArea.LeftArea.Count - 1 do
        	additionArea.AddRing (protectArea.LeftArea.Ring [i]);
    end;
    if not IsGeometryEmpty (protectArea.RightArea) then
    begin
        for i:=0 to protectArea.RightArea.Count - 1 do
            additionArea.AddRing (protectArea.RightArea.Ring [i]);
    end;

	FAdditionGraphicHandle := FUi.DrawPolygon (additionArea, SymbolAddition);
	FPrimaryGraphicHandle := FUi.DrawPolygon (protectArea.PrimaryArea, SymbolPrimary);
	FNominalGraphicHandle := FUi.DrawPolyline (protectArea.NominalLine, SymbolNominal);

	if Assigned (earlestToleranceLine) then
		FEarlestLineHandle := FUi.DrawPolyline (earlestToleranceLine, FSymbolEarlestLine);
	if Assigned (latestToleranceLine) then
		FLatestLineHandle := FUi.DrawPolyline (latestToleranceLine, FSymbolLatestLine);

	additionArea.Free;        
end;

procedure TSegmentSymbol.DrawArea2 (protectArea: TProtectedArea;
                earlestToleranceLine, latestToleranceLine: TPolyline);
var
	additionArea:	TPolygon;
	ring:			TRing;
begin
	additionArea := TPolygon.Create;

    if not IsGeometryEmpty (protectArea.LeftArea) then
    	additionArea.AddRing (protectArea.LeftArea.Ring [0]);
    if not IsGeometryEmpty (protectArea.RightArea) then
        additionArea.AddRing (protectArea.RightArea.Ring [0]);

	FAdditionGraphicHandle := FUi.DrawPolygon (additionArea, SymbolAddition);
	FPrimaryGraphicHandle := FUi.DrawPolygon (protectArea.PrimaryArea, SymbolPrimary);
	FNominalGraphicHandle := FUi.DrawPolyline (protectArea.NominalLine, SymbolNominal);
	if Assigned (earlestToleranceLine) then
		FEarlestLineHandle := FUi.DrawPolyline (earlestToleranceLine, FSymbolEarlestLine);
	if Assigned (latestToleranceLine) then
		FLatestLineHandle := FUi.DrawPolyline (latestToleranceLine, FSymbolLatestLine);

	additionArea.Free;
end;

procedure TSegmentSymbol.EraseArea;
begin
	DeleteGraphic (FAdditionGraphicHandle);
	DeleteGraphic (FPrimaryGraphicHandle);
	DeleteGraphic (FCLGraphicHandle);
	DeleteGraphic (FNominalGraphicHandle);
	DeleteGraphic (FEarlestLineHandle);
	DeleteGraphic (FLatestLineHandle);		
end;


function TTurn.CheckInnersIntersect (primaryInnerPoints, additionInnerPoints: TMultiPoint): Boolean;
var
	primLine,
	addLine:	TPolyline;
	part:		TPart;
	geom:		TGeometry;
begin
	primLine := TPolyline.Create;
	addLine := TPolyline.Create;
	part := TPart.Create;
	part.AddMultiPoint (primaryInnerPoints);
	primLine.AddPart (part);
	part.Clear;
	part.AddMultiPoint (additionInnerPoints);
	addLine.AddPart (part);
	part.Free;

	geom := GeoOper.intersect (primLine, addLine);
	result := not IsGeometryEmpty (geom);
	geom.Free;

	primLine.Free;
	addLine.Free;
end;


{ TFilterNextPointsParams }

procedure TFilterNextPointsParams.Assign (src: TFilterNextPointsParams);
begin
	StartMinDistance := src.StartMinDistance;
	Distance := src.Distance;
	Radius := src.Radius;
	Direction := src.Direction;
	Point := src.Point;
	TurnType := src.TurnType;
end;

procedure TFilterNextPointsParams.Clear;
begin
	StartMinDistance := 0.0;
	Distance := 0.0;
	Radius := 0.0;
	Direction := 0.0;
	Point := nil;
	TurnType := tuFlyBy;
end;

constructor TFilterNextPointsParams.Create;
begin
	inherited;
	Clear ();
end;

function TFilterNextPointsParams.IsEqual (other: TFilterNextPointsParams): Boolean;
begin
	result := (
		(StartMinDistance = other.StartMinDistance) and
		(Distance = other.Distance) and
		(Radius = other.Radius) and
		(Direction = other.Direction) and
		(TurnType = other.TurnType) and
        ((self.Point <> nil) and (other.Point <> nil) and
        (self.Point.X = other.Point.X) and (self.Point.Y = other.Point.Y)
        ));
end;

function TParameters.ToRadius(bankAngle, IAS, elevation: Double): Double;
var
	TAS:	Double;
begin
	TAS := IASToTAS (IAS, elevation, FAHP.Temperature);
	result := BankToRadius (bankAngle, TAS);
end;

function TParameters.ToTAS (IAS, elevation: Double): Double;
begin
	result := IASToTAS (IAS, elevation, FAHP.Temperature);
end;

{ TCreateStraightAreaParams }

constructor TCreateStraightAreaParams.Create;
begin
    Self.Direction := 0;
    Self.Distance := 0;
    Self.TurnType := tuFlyBy;
    PDG := 0;
end;

procedure TTurn.CreateStaightAreaDME (createExtendedArea: Boolean);
const
	widthInDER = 150.0;
var
    derSemiWidth,
    startPointSemiWidth,
    XTT, ATT,
	dir,
	alphaShift:     Double;
	leftRing,
	primaryRing,
	rightRing:      TRing;
	nominalPart:    TPart;
	exLeftRing,
	exPrimaryRing,
	exRightRing:    TRing;
	exNominalPart:  TPart;
	tmpPoint:       TPoint;
begin
	FStraight.ExpandPoints.Clear ();

    FTurnInfo.Start.HeightNominal := FTurnInfo.Start.Distance * FTurnInfo.PDG;

    GetDMEToleranceParams (True, FParams.StartRwyDirection.ElevTdz,
        FParams.IsDMEtill_1989, FParams.DMECount, XTT, ATT, derSemiWidth);

    GetDMEToleranceParams (False,
        FTurnInfo.Start.HeightNominal + FParams.FStartRwyDirection.ElevTdz,
        FParams.IsDMEtill_1989, FParams.DMECount, XTT, ATT, startPointSemiWidth);

    FTurnInfo.Start.ATT := ATT;
    FTurnInfo.Start.XTT := XTT;

    leftRing := TRing.Create ();
    primaryRing := TRing.Create ();
    rightRing := TRing.Create ();
    nominalPart := TPart.Create ();

    exLeftRing := TRing.Create ();
    exPrimaryRing := TRing.Create ();
    exRightRing := TRing.Create ();
    exNominalPart := TPart.Create ();

    //--- begin: Fill primary area points.
    tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj, FTurnInfo.Start.Direction + 0.5*Pi,
                    derSemiWidth / 2);
    primaryRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction + 0.5*Pi,
                    startPointSemiWidth / 2);
    primaryRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction - 0.5*Pi,
                    startPointSemiWidth / 2);
    primaryRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj, FTurnInfo.Start.Direction - 0.5*Pi,
                    derSemiWidth / 2);
    primaryRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    //--- end.

    //--- begin: Fill left area points.
    leftRing.AddPoint (primaryRing.Point [0]);
    tmpPoint := PointAlongPlane (primaryRing.Point [0], FTurnInfo.Start.Direction + 0.5*Pi,
                    derSemiWidth / 2);
    leftRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (primaryRing.Point [1], FTurnInfo.Start.Direction + 0.5*Pi,
                    startPointSemiWidth / 2);
    leftRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    leftRing.AddPoint (primaryRing.Point [1]);
    //--- end.

    //--- begin: Fill right area points.
    rightRing.AddPoint (primaryRing.Point [3]);
    tmpPoint := PointAlongPlane (primaryRing.Point [3], FTurnInfo.Start.Direction - 0.5*Pi,
                    derSemiWidth / 2);
    rightRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (primaryRing.Point [2], FTurnInfo.Start.Direction - 0.5*Pi,
                    startPointSemiWidth / 2);
    rightRing.AddPoint (tmpPoint);
    tmpPoint.Free;
    rightRing.AddPoint (primaryRing.Point [2]);
    //--- end.

    if createExtendedArea then
    begin
	    //--- begin: Fill extended primary area points.
	    exPrimaryRing.AddPoint (primaryRing.Point [0]);
	    dir := ReturnAngleInRadians (primaryRing.Point [0], primaryRing.Point [1]);
		tmpPoint := PointAlongPlane (primaryRing.Point [1], dir, deltaDist);
	    exPrimaryRing.AddPoint (tmpPoint);
	    tmpPoint.Free;
	    dir := ReturnAngleInRadians (primaryRing.Point [3], primaryRing.Point [2]);
		tmpPoint := PointAlongPlane (primaryRing.Point [2], dir, deltaDist);
	    exPrimaryRing.AddPoint (tmpPoint);
	    tmpPoint.Free;
	    exPrimaryRing.AddPoint (primaryRing.Point [3]);
	    //--- end.

	    //--- begin: Fill extended left area points.
	    exLeftRing.AddPoint (leftRing.Point [0]);
	    exLeftRing.AddPoint (leftRing.Point [1]);
	    dir := ReturnAngleInRadians (leftRing.Point [1], leftRing.Point [2]);
		tmpPoint := PointAlongPlane (leftRing.Point [2], dir, deltaDist);
	    exLeftRing.AddPoint (tmpPoint);
	    tmpPoint.Free;
	    exLeftRing.AddPoint (exPrimaryRing.Point [1]);
	    //--- end.

	    //--- begin: Fill exteded right area points.
	    exRightRing.AddPoint (rightRing.Point [0]);
	    exRightRing.AddPoint (rightRing.Point [1]);
		dir := ReturnAngleInRadians (rightRing.Point [1], rightRing.Point [2]);
		tmpPoint := PointAlongPlane (rightRing.Point [2], dir, deltaDist);
	    exRightRing.AddPoint (tmpPoint);
	    tmpPoint.Free;
	    exRightRing.AddPoint (exPrimaryRing.Point [2]);
	    //--- end.

        exNominalPart.AddPoint (FParams.StartRwyDirection.Prj);
        tmpPoint := PointAlongPlane (FTurnInfo.Start.Point, FTurnInfo.Start.Direction, deltaDist);
        exNominalPart.AddPoint (tmpPoint);
        tmpPoint.Free;
    end;

    //---begin: Add 2 points for DER Line.
    tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj,
                    FParams.StartRwyDirection.Direction + 0.5*Pi, widthInDER);
    primaryRing.InsertPoint (0, tmpPoint);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj,
                    FParams.StartRwyDirection.Direction - 0.5*Pi, widthInDER);
    primaryRing.AddPoint (tmpPoint);
    tmpPoint.Free;

    if createExtendedArea then
    begin
        tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj,
                        FParams.StartRwyDirection.Direction + 0.5*Pi, widthInDER);
        exPrimaryRing.InsertPoint (0, tmpPoint);
        tmpPoint.Free;
        tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj,
                        FParams.StartRwyDirection.Direction - 0.5*Pi, widthInDER);
        exPrimaryRing.AddPoint (tmpPoint);
        tmpPoint.Free;
    end;
    //---end.

    nominalPart.AddPoint (FParams.StartRwyDirection.Prj);
    nominalPart.AddPoint (FTurnInfo.Start.Point);

    FStraight.Segment.Area.LeftArea.Clear ();
    FStraight.Segment.Area.PrimaryArea.Clear ();
    FStraight.Segment.Area.RightArea.Clear ();
    FStraight.Segment.Area.NominalLine.Clear ();

    FStraight.Segment.Area.LeftArea.AddRing (leftRing);
    FStraight.Segment.Area.PrimaryArea.AddRing (primaryRing);
    FStraight.Segment.Area.RightArea.AddRing (rightRing);
    FStraight.Segment.Area.NominalLine.AddPart (nominalPart);

    if createExtendedArea then
    begin
	    FStraight.Segment.ExtendedArea.LeftArea.Clear ();
	    FStraight.Segment.ExtendedArea.PrimaryArea.Clear ();
	    FStraight.Segment.ExtendedArea.RightArea.Clear ();
	    FStraight.Segment.ExtendedArea.NominalLine.Clear ();

	    FStraight.Segment.ExtendedArea.LeftArea.AddRing (exLeftRing);
	    FStraight.Segment.ExtendedArea.PrimaryArea.AddRing (exPrimaryRing);
	    FStraight.Segment.ExtendedArea.RightArea.AddRing (exRightRing);
	    FStraight.Segment.ExtendedArea.NominalLine.AddPart (exNominalPart);
    end;

    alphaShift := SubtractAngles (FTurnInfo.Start.Direction, FParams.StartRwyDirection.Direction) *
        Integer (SideFrom2Angle_new (FParams.StartRwyDirection.Direction, FTurnInfo.Start.Direction));

    FStraight.CutZoneByLine (
                FParams.StartRwyDirection.Prj,
                FParams.StartRwyDirection.Direction,
                alphaShift,
                FTurnInfo.Start.Distance,
                FStraight.Segment.Area);

    if createExtendedArea then
    begin
        FStraight.CutZoneByLine (
                FParams.StartRwyDirection.Prj,
                FParams.StartRwyDirection.Direction,
                alphaShift,
				FTurnInfo.Start.Distance + deltaDist,
                FStraight.Segment.ExtendedArea);
    end;

    leftRing.Free;
    primaryRing.Free;
    rightRing.Free;
    nominalPart.Free;

    exLeftRing.Free;
    exPrimaryRing.Free;
    exRightRing.Free;
    exNominalPart.Free;
end;

procedure TTurn.DrawToleranceRectangle ();
var
	tmpPolygon: TPolygon;
	tmpRing: TRing;
	tmpPoint: TPoint;
begin

	DeleteGraphic (FAddToleranceRectHandle);
	DeleteGraphic (FPrimaryToleranceRectHandle);
	DeleteGraphic (FStartPointHandle);

	tmpPolygon := TPolygon.Create ();
	tmpRing := TRing.Create ();

	tmpPoint := LocalToPrj (FTurnInfo.Start.Point, FTurnInfo.Start.Direction,
		FTurnInfo.Start.ATT, -FTurnInfo.Start.XTT);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Start.Point, FTurnInfo.Start.Direction,
		FTurnInfo.Start.ATT, FTurnInfo.Start.XTT);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Start.Point, FTurnInfo.Start.Direction,
		-FTurnInfo.Start.ATT, FTurnInfo.Start.XTT);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Start.Point, FTurnInfo.Start.Direction,
		-FTurnInfo.Start.ATT, -FTurnInfo.Start.XTT);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;

	tmpPolygon.AddRing (tmpRing);


	{tmpRing.AddPoint (FTurnInfo.Start.EarlestTolerance.GetOuterPoint ());
	tmpRing.AddPoint (FTurnInfo.Start.NominalTolerance.GetOuterPoint ());
	tmpRing.AddPoint (FTurnInfo.Start.LatestTolerance.GetOuterPoint ());
	tmpRing.AddPoint (FTurnInfo.Start.LatestTolerance.GetPrimaryPoint (0));
	tmpRing.AddPoint (FTurnInfo.Start.NominalTolerance.GetPrimaryPoint (0));
	tmpRing.AddPoint (FTurnInfo.Start.EarlestTolerance.GetPrimaryPoint (0));
	tmpPolygon.AddRing (tmpRing);

	tmpRing.Point [0].Assign (FTurnInfo.Start.EarlestTolerance.GetInnerPoint ());
	tmpRing.Point [1].Assign (FTurnInfo.Start.NominalTolerance.GetInnerPoint ());
	tmpRing.Point [2].Assign (FTurnInfo.Start.LatestTolerance.GetInnerPoint ());
	tmpRing.Point [3].Assign (FTurnInfo.Start.LatestTolerance.GetPrimaryPoint (2));
	tmpRing.Point [4].Assign (FTurnInfo.Start.NominalTolerance.GetPrimaryPoint (2));
	tmpRing.Point [5].Assign (FTurnInfo.Start.EarlestTolerance.GetPrimaryPoint (2));
	tmpPolygon.AddRing (tmpRing);

	FAddToleranceRectHandle := DrawGeometry (tmpPolygon, FAddToleranceSymbol);

	tmpRing.Point [0].Assign (FTurnInfo.Start.EarlestTolerance.GetPrimaryPoint (0));
	tmpRing.Point [1].Assign (FTurnInfo.Start.NominalTolerance.GetPrimaryPoint (0));
	tmpRing.Point [2].Assign (FTurnInfo.Start.LatestTolerance.GetPrimaryPoint (0));
	tmpRing.Point [3].Assign (FTurnInfo.Start.LatestTolerance.GetPrimaryPoint (2));
	tmpRing.Point [4].Assign (FTurnInfo.Start.NominalTolerance.GetPrimaryPoint (2));
	tmpRing.Point [5].Assign (FTurnInfo.Start.EarlestTolerance.GetPrimaryPoint (2));
	tmpPolygon.Ring [0].Assign (tmpRing);
	tmpPolygon.RemoveAt (1);}


	tmpRing.Clear;
	tmpPoint := LocalToPrj (FTurnInfo.Next.Point, FTurnInfo.Next.Direction,
		FTurnInfo.Next.ATT, -FNextXTT_tmp);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Next.Point, FTurnInfo.Next.Direction,
		FTurnInfo.Next.ATT, FNextXTT_tmp);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Next.Point, FTurnInfo.Next.Direction,
		-FTurnInfo.Next.ATT, FNextXTT_tmp);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;
	tmpPoint := LocalToPrj (FTurnInfo.Next.Point, FTurnInfo.Next.Direction,
		-FTurnInfo.Next.ATT, -FNextXTT_tmp);
	tmpRing.AddPoint (tmpPoint);
	tmpPoint.Free;

	tmpPolygon.AddRing (tmpRing);


	FPrimaryToleranceRectHandle := DrawGeometry (tmpPolygon, FPrimaryToleranceSymbol);

	FStartPointHandle := DrawGeometry (FTurnInfo.Start.Point, RGB (20, 200, 20));

	tmpRing.Free;
	tmpPolygon.Free;
end;

end.
