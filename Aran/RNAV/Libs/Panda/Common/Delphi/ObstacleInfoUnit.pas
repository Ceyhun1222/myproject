unit ObstacleInfoUnit;

interface

uses
	AIXMTypes, FlightPhaseUnit, Geometry;

type

	TObstacleInfo = class
		ptGeo:				TPoint;
		ptPrj:				TPoint;
		Local:				TPoint;
		FlightProcedure:	TFlightProcedure;
		Leg,
		Flags:				Integer;
		Elevation,
		Height,
		Dist,
		fTmp,
		ReqH,
		ReqOCA,
		hPent, coeff,
		MOC, fSort:			Double;
		Ignored:			Boolean;
		Name, AID,
		ID, sSort:			String;

		constructor Create;						overload;
		constructor Create(Host: TObstacle);	overload;
		destructor Destroy;						override;
	end;

	TObstacleInfoList = Array of TObstacleInfo;

implementation

constructor TObstacleInfo.Create;
begin
	Inherited Create;

	ptGeo := TPoint.Create;
	ptPrj := TPoint.Create;
	Local := TPoint.Create;

	Elevation := 0;
	Name := '';
	ID := '';
	AID := '';

	FlightProcedure := TFlightProcedure(-1);
	Leg	:= -1;
	Flags := 0;
	Height := 0;
	Dist := 0;
	fTmp := 0;
	ReqH := 0;
	ReqOCA := 0;
	hPent := 0;
	coeff := 0;
	MOC := 0;
	fSort := 0;
	Ignored := False;
	sSort := '';
end;

constructor TObstacleInfo.Create(Host: TObstacle);
begin
	Inherited Create;

	ptGeo := TPoint.Create(Host.Geo);
	ptPrj := TPoint.Create(Host.Prj);
	Local := TPoint.Create;

	Elevation := Host.Elevation;
	Name := Host.Name;
	ID := Host.ID;
	AID := Host.AixmID;

	FlightProcedure := TFlightProcedure(-1);
	Leg	:= -1;
	Flags := 0;
	Height := 0;
	Dist := 0;
	fTmp := 0;
	ReqH := 0;
	ReqOCA := 0;
	hPent := 0;
	coeff := 0;
	MOC := 0;
	fSort := 0;
	sSort := '';
end;

destructor TObstacleInfo.Destroy;
begin
	ptGeo.Free;
	ptPrj.Free;
	Local.Free;
	inherited;
end;

end.
