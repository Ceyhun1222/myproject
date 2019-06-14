unit ARANClasses;

interface

uses
	Geometry,
    ARANMath,
	UIContract,
	CollectionUnit,
	Classes,
	SysUtils,
	Dialogs;

type

	EARANError = class (Exception)
	private
		FErrorType:	TMsgDlgType;
	public
		constructor Create (message: String; errorType: TMsgDlgType); overload;
		property ErrorType: TMsgDlgType read FErrorType write FErrorType;
	end;

	TMUIStrings = class
	public
		PriorJan89: String;
		AfterJan89: String;
		SignificantPoints: String;
		ControlKKLine: String;
		Left: String;
		Right: String;
		Lat: String;
		Long: String;
		ProcNameNotCorrect: String;
		Departure: String;
		Parameters: String;
		Straight: String;
		Turn: String;
		ToRight: String;
		ToLeft: String;

		constructor Create;
	end;

	TProtectedArea = class
	private
		FLeftArea:		TPolygon;
		FRightArea:		TPolygon;
		FPrimaryArea:	TPolygon;
		FNominalLine:	TPolyline;

        function GetSideArea (side: TSideDirection): TPolygon;
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TProtectedArea);
		function Clone (): TProtectedArea;
        procedure Clear ();

		property LeftArea: TPolygon read FLeftArea;
		property PrimaryArea: TPolygon read FPrimaryArea;
		property RightArea: TPolygon read FRightArea;
		property NominalLine: TPolyline read FNominalLine;
        property SideArea [side: TSideDirection]: TPolygon read GetSideArea;
	end;

	TProtectedSegment = class
	private
		FArea,
		FExtendedArea,
        FAreaToKK:		    TProtectedArea;
		FLinesForMOCCoef:	TPolyline; // part 0 is addition outline line ... 3 is addition inner line.
	public
		constructor Create;
		destructor Destroy; override;

		procedure Assign (src: TProtectedSegment);
		function Clone (): TProtectedSegment;
        procedure Clear ();

		property Area: TProtectedArea read FArea;
        property AreaToKK: TProtectedArea read FAreaToKK;
		property ExtendedArea: TProtectedArea read FExtendedArea;
		property LinesForMOCCoef: TPolyline read FLinesForMOcCoef;
	end;

	TProtectedSegmentList = class
	private
		FSegments:	TList;

		FAdditionGraphicHandle,
		FPrimaryGraphicHandle,
		FCLGraphicHandle,
		FNominalGraphicHandle:	Integer;

		FUI:	TUIContract;

		FSymbolPrimary:		TFillSymbol;
		FSymbolAddition:	TFillSymbol;
		FSymbolCL:			TLineSymbol;
		FSymbolNominal:		TLineSymbol;		
	private
		function GetSegment (index: Integer): TProtectedSegment;
		function GetCurrentSegment (): TProtectedSegment;
		procedure DeleteGraphic (var handle: Integer);
		function GetCount (): Integer;
	public
		constructor Create (aUI: TUIContract); overload;
		destructor Destroy; override;

		procedure Add (value: TProtectedSegment);
		procedure Clear ();
		procedure DrawArea (staightCutLine: TPolyline);
		procedure EraseArea ();

		property UI: TUIContract read FUI;
		property SymbolPrimary: TFillSymbol read FSymbolPrimary;
		property SymbolAddition: TFillSymbol read FSymbolAddition;
		property SymbolCL: TLineSymbol read FSymbolCL;
		property SymbolNominal: TLineSymbol read FSymbolNominal;
		property Item [index: Integer]: TProtectedSegment read GetSegment;
		property CurrentSegment: TProtectedSegment read GetCurrentSegment;
		property Count: Integer read GetCount;
	end;

implementation

uses
	ARANFunctions,
	GeometryOperatorsContract,
	Windows;

constructor EARANError.Create (message: String; errorType: TMsgDlgType);
begin
	inherited Create(message);
	FErrorType := errorType;
end;

constructor TProtectedSegment.Create;
var
	part:	TPart;
begin
	inherited;
	FArea := TProtectedArea.Create;
    FAreaToKK := TProtectedArea.Create;
	FExtendedArea := TProtectedArea.Create;

	FLinesForMOCCoef := TPolyline.Create;
	part := TPart.Create;
	FLinesForMOCCoef.AddPart (part);
	FLinesForMOCCoef.AddPart (part);
	FLinesForMOCCoef.AddPart (part);
	FLinesForMOCCoef.AddPart (part);
	part.Free;
end;

destructor TProtectedSegment.Destroy;
begin
	inherited;
	FArea.Free;
    FAreaToKK.Free;
	FExtendedArea.Free;
	FLinesForMOCCoef.Free;
end;

procedure TProtectedSegment.Assign (src: TProtectedSegment);
begin
	FArea.Assign (src.Area);
    FAreaToKK.Assign (src.AreaToKK);
	FExtendedArea.Assign (src.ExtendedArea); 
	FLinesForMOCCoef.Assign (src.LinesForMOCCoef);
end;

function TProtectedSegment.Clone (): TProtectedSegment;
begin
	result := TprotectedSegment.Create;
	result.Assign (self);
end;

procedure TProtectedSegment.Clear;
begin
	FArea.Clear;
    FAreaToKK.Clear;
    FExtendedArea.Clear;

    // part 0 is addition outline line ... 3 is addition inner line.
    FLinesForMOCCoef.Part [0].Clear;
    FLinesForMOCCoef.Part [1].Clear;
    FLinesForMOCCoef.Part [2].Clear;
    FLinesForMOCCoef.Part [3].Clear;
end;

constructor TProtectedSegmentList.Create (aUI: TUIContract);
begin
	inherited Create;
	FSegments := TList.Create;
	
	FUI := aUI;

	FAdditionGraphicHandle := -1;
	FPrimaryGraphicHandle := -1;
	FCLGraphicHandle := -1;
	FNominalGraphicHandle := -1;

	FSymbolPrimary := TFillSymbol.Create;
	FSymbolAddition := TFillsymbol.Create;
	FSymbolCl := TLineSymbol.Create;
	FSymbolNominal := TLineSymbol.Create;	
end;

destructor TProtectedSegmentList.Destroy;
var
	i:	Integer;
begin
	inherited;

	for i := 0 to FSegments.Count-1 do
	begin
		Item [i].Free;
	end;

	FSegments.Free;

	SymbolPrimary.Free;
	SymbolAddition.Free;
	SymbolCl.Free;
	SymbolNominal.Free;	
end;

procedure TProtectedSegmentList.Add (value: TProtectedSegment);
begin
	FSegments.Add (value.Clone);
end;

procedure TProtectedSegmentList.Clear;
begin
	FSegments.Clear;
end;

function TProtectedSegmentList.GetSegment (index: Integer): TProtectedSegment;
begin
	result := TProtectedSegment (FSegments [index]);
end;

function TProtectedSegmentList.GetCount: Integer;
begin
	result := FSegments.Count;
end;

function TProtectedSegmentList.GetCurrentSegment (): TProtectedSegment;
begin
	result := nil;
	if Count = 0 then exit;
	result := GetSegment (FSegments.Count - 1);
end;

procedure TProtectedSegmentList.DrawArea (staightCutLine: TPolyline);
var
	leftArea,
	rightArea,
	leftAreaNext,
	rightAreaNext,
	additionArea,
	primaryArea,
	primaryAreaNext:	TPolygon;
	nominalLine,
	nominalLineNext:	TPolyline;
	seg:				TProtectedSegment;
	i:					Integer;
	geom,
	leftGeom,
	rightGeom:			TGeometry;
	ring:				TRing;
	part:				TPart;
begin
	EraseArea ();

	if FSegments.Count = 0 then
		exit;

	seg := GetSegment (0);

	
	ring := TRing.Create;
	part := TPart.Create;

	leftArea := TPolygon.Create;
	rightArea := TPolygon.Create;
	leftArea.AddRing (seg.Area.LeftArea.Ring [0]);
	rightArea.AddRing (seg.Area.RightArea.Ring [0]);

	leftAreaNext := TPolygon.Create;
	rightAreaNext := TPolygon.Create;
	leftAreaNext.AddRing (ring);
	rightAreaNext.AddRing (ring);

	primaryArea := TPolygon.Create;
	primaryArea.Assign (seg.Area.PrimaryArea);

	primaryAreaNext := TPolygon.Create;
	primaryAreaNext.AddRing (ring);

	nominalLine := TPolyline.Create;
	nominalLine.Assign (seg.Area.NominalLine);

	nominalLineNext := TPolyline.Create;
	nominalLineNext.AddPart (part);

	ring.Free;
	part.Free;

	// --- begin: Cut straight area by latestToleranceLine.
	if (staightCutLine <> nil) and (FSegments.Count > 1) then
	begin
		GeoOper.cut (primaryArea, staightCutLine, leftGeom, rightGeom);
		primaryArea.Assign (rightGeom);
		leftGeom.Free;
		rightGeom.Free;
	end;
	// --- end.


	for i:=1 to FSegments.Count - 1 do
	begin
		seg := GetSegment (i);

		leftAreaNext.Ring [0].Assign (seg.Area.LeftArea.Ring [0]);
		rightAreaNext.Ring [0].Assign (seg.Area.RightArea.Ring [0]);

		geom := GeoOper.unionGeometry (leftArea, leftAreaNext);
		leftArea.Assign (geom);
		geom.Free;

		geom := GeoOper.unionGeometry (rightArea, rightAreaNext);
		rightArea.Assign (geom);
		geom.Free;

		primaryAreaNext.Ring [0].Assign (seg.Area.PrimaryArea.Ring [0]);
		geom := GeoOper.unionGeometry (primaryArea, primaryAreaNext);
		primaryArea.Assign (geom);
		geom.Free;

		nominalLineNext.Part [0].Assign (seg.Area.NominalLine.Part [0]);

		nominalLine.Part [0].Point [nominalLine.Part [0].Count - 1].Assign (
			nominalLineNext.Part [0].Point [0]);

		geom := GeoOper.unionGeometry(nominalLine, nominalLineNext);
		nominalLine.Assign (geom);
		geom.Free;
	end;

	geom := GeoOper.difference (leftArea, primaryArea);
	if (geom.GeometryType = gtPolygon) and (geom.AsPolygon.Count > 0) then
		leftArea.Assign (geom);
	geom.Free;

	geom := GeoOper.difference (rightArea, primaryArea);
	if (geom.GeometryType = gtPolygon) and (geom.AsPolygon.Count > 0) then
		rightArea.Assign (geom);
	geom.Free;

	additionArea := TPolygon.Create;

	if leftArea.Count = 1 then
		ring := leftArea.Ring [0]
	else
	begin
		if leftArea.Ring [0].Count > leftArea.Ring [1].Count then
			ring := leftArea.Ring [0]
		else
			ring := leftArea.Ring [1];
	end;
	additionArea.AddRing (ring);

	if rightArea.Count = 1 then
		ring := rightArea.Ring [0]
	else
	begin
		if rightArea.Ring [0].Count > rightArea.Ring [1].Count then
			ring := rightArea.Ring [0]
		else
			ring := rightArea.Ring [1];
	end;
	additionArea.AddRing (ring);
		
	FAdditionGraphicHandle := FUi.DrawPolygon (additionArea, SymbolAddition);
	FPrimaryGraphicHandle := FUi.DrawPolygon (primaryArea, SymbolPrimary);
	FNominalGraphicHandle := FUi.DrawPolyline (nominalLine, SymbolNominal);

	additionArea.Free;
	primaryArea.Free;
	primaryAreaNext.Free;
	nominalLine.Free;
	nominalLineNext.Free;
end;

procedure TProtectedSegmentList.EraseArea ();
begin
	DeleteGraphic (FAdditionGraphicHandle);
	DeleteGraphic (FPrimaryGraphicHandle);
	DeleteGraphic (FCLGraphicHandle);
	DeleteGraphic (FNominalGraphicHandle);
end;

procedure TProtectedSegmentList.DeleteGraphic (var handle: Integer);
begin
	if handle = -1 then
		exit;
	FUi.DeleteGraphic (handle);
	handle := -1;
end;

constructor TProtectedArea.Create;
begin
	inherited;
	FLeftArea := TPolygon.Create;
	FPrimaryArea := TPolygon.Create;
	FRightArea := TPolygon.Create;
	FNominalLine := TPolyline.Create;
end;

destructor TProtectedArea.Destroy;
begin
	inherited;
	FLeftArea.Free;
	FPrimaryArea.Free;
	FRightArea.Free;
	FNominalLine.Free;
end;

procedure TProtectedArea.Assign(src: TProtectedArea);
begin
	FLeftArea.Assign (src.LeftArea);
	FPrimaryArea.Assign (src.PrimaryArea);
	FRightArea.Assign (src.RightArea);
	FNominalLine.Assign (src.NominalLine);
end;

function TProtectedArea.Clone (): TProtectedArea;
begin
	result := TProtectedArea.Create;
	result.Assign (self); 
end;

procedure TProtectedArea.Clear;
begin
	FLeftArea.Clear;
	FRightArea.Clear;
	FPrimaryArea.Clear;
	FNominalLine.Clear;
end;

function TProtectedArea.GetSideArea (side: TSideDirection): TPolygon;
begin
    if side = sideLeft then
        result := FLeftArea
    else if side = sideRight then
        result := FRightArea
    else
        result := FPrimaryArea
end;

{ TMUIString }

constructor TMUIStrings.Create;
begin
	PriorJan89 := 'prior to 1 Jan 1989';
	AfterJan89 := 'after 1 Jan 1989';
	SignificantPoints := 'Significant Points';
	ControlKKLine := 'KK'' Line';
	Left := 'Left';
	Right := 'Right';
	Lat := 'Lat.';
	Long := 'Long.';

	ProcNameNotCorrect := 'Procedure name not correct.';

	Departure := 'Departure';
	Parameters := 'Parameters';
	Straight := 'Straight';
	Turn := 'Turn';
	ToRight := '° To Right';
	ToLeft := '° To Left';
end;
{
initialization
	GeoOper := TGeometryOperators.Create;

finalization
	GeoOper.Free;
}
end.
