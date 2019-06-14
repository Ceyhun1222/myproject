unit ObjectDirectoryContract;

interface

uses
  Contract,
  AixmTypes,
  GeometryOperatorsContract,
  CollectionUnit,
  Geometry,
  SettingsContract,
  Common;

type
	TObjectDirectoryCommands =
	(
		objectDirectoryGetAedromeList = 0,
		objectDirectoryGetAedrome,
		objectDirectoryGetRWYList,
		objectDirectoryGetRWY,
		objectDirectorySetToSpatialReference,
		objectDirectoryGetToSpatialReference,
		objectDirectoryConnect,
		objectDirectoryDisConnect,
		objectDirectoryGetConnectionInfo,
		objectDirectoryGetRWYDirectionList,
		objectDirectoryGetSignificantPoints,
		objectDirectoryGetObstacles,

		objectDirectorySetProcedure
	);

	TObjectDirectory = class
	private
		FHandle: THandle;
		FAHP: TAHP;
		FRWY: TRWY;
		FRWYDirectionList: TRWYDirectionList;
		FSignificantPoints: TPandaCollection;
		FDMEList: TDMEList;
		FObstacles: TObstacleList;

		function GetAerodrome (aerodromeId: String): TAHP;
		function GetRWY (rwyId: String): TRWY;
		function getRWYDirectionList (RWY_ID: String; ahp: TAHP): TRwyDirectionList;
		function getSignificantPoints (orgID: String; ahp: TAHP; ptCentre: TPoint; range: Double): TPandaCollection;
		function getObstacleList (ptCentre: TPoint; range: Double): TObstacleList;
		function getToSpatialReference (): TSpatialReference;
		procedure setToSpatialReference (toSpacRef: TSpatialReference);
	public
		constructor Create;
		destructor Destroy; override;

		function isValid: boolean;

		function GetAerodromeList (icaoPrefixArray: TStringArray): TPandaList;
		function GetRWYList (aerodromeID: String): TPandaList;
		function SetProcedure (aixmProcedureHandle: Integer): Boolean;
	public
		property RWYList[aerodromeID: String]: TPandaList read GetRWYList;
		property RWY [rwyID: String]: TRWY read GetRWY;
		property RWYDirectionList [RWY_ID: String; ahp: TAHP]: TRwyDirectionList read getRWYDirectionList;
		property AHP [aerodromeId: String]: TAHP read GetAerodrome;

		property SignificantPoints [orgID: String; ahp: TAHP; ptCentre: TPoint; range: Double]: TPandaCollection read getSignificantPoints;
		property Obstalces [ptCentre: TPoint; range: Double]: TObstacleList read getObstacleList;
		property ToSpatialReference: TSpatialReference read getToSpatialReference write setToSpatialReference;
	end;

implementation



constructor TObjectDirectory.Create;
begin
	inherited;

	FHandle := Contract.GetInstance ('ObjectDirectory');

	FRWYDirectionList := TRWYDirectionList.Create;
	FSignificantPoints := TPandaCollection.Create;
	FObstacles := TObstacleList.Create;
	FDMEList := TDMEList.Create;
	FAHP := TAHP.Create;
	FRWY := TRWY.Create;
end;

destructor TObjectDirectory.Destroy;
begin
 //	DisConnect;
	FRWYDirectionList.Free;
	FSignificantPoints.Free;
	FObstacles.Free;
	FDMEList.Free;
	FAHP.Free;
	FRWY.Free;

	inherited;
end;

function TObjectDirectory.isValid: boolean;
begin
	result := FHandle <> 0;
end;

function TObjectDirectory.GetAerodromeList (icaoPrefixArray: TStringArray): TPandaList;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetAedromeList));
	PutWideStringArray (FHandle, icaoPrefixArray);
	Contract.endMessage (FHandle);

	result := TPandaList.Create (TUnicalName.Create);
	result.unpack (FHandle);
end;

function TObjectDirectory.GetAerodrome (aerodromeId: String): TAHP;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetAedrome));
	PutWideString (FHandle, aerodromeId);
	Contract.endMessage (FHandle);
	FAHP.Unpack (FHandle);

	result := FAHP;
end;

function TObjectDirectory.GetRWYList (aerodromeID: String): TPandaList;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetRWYList));
	PutWideString (FHandle, aerodromeID);
	Contract.endMessage (FHandle);

	result := TPandaList.Create (TUnicalName.Create);
	result.unpack (FHandle);
end;

function TObjectDirectory.GetRWY (rwyId: String): TRWY;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetRWY));
	PutWideString (FHandle, rwyId);
	Contract.endMessage (FHandle);

	FRWY.unpack (FHandle);

	result := FRWY;
end;

function TObjectDirectory.getRWYDirectionList (RWY_ID: String; ahp: TAHP): TRwyDirectionList;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetRWYDirectionList));
	PutWideString (FHandle, RWY_ID);
 	ahp.Pack (FHandle);
	Contract.endMessage (FHandle);

	FRWYDirectionList.unpack (FHandle);

	result := FRWYDirectionList;
end;

function TObjectDirectory.getSignificantPoints (orgID: String; ahp: TAHP; ptCentre: TPoint; range: Double): TPandaCollection;
begin
	Contract.beginMessage (FHandle, Integer (objectDirectoryGetSignificantPoints));
	Contract.PutWideString (FHandle, orgID);
	ahp.Pack (FHandle);
	ptCentre.Pack(FHandle);
	Contract.PutDouble(FHandle, range);
	Contract.endMessage (FHandle);

	FSignificantPoints.Unpack (FHandle);
	result := FSignificantPoints;
end;

function TObjectDirectory.getObstacleList (ptCentre: TPoint; range: Double): TObstacleList;
begin
//	if not ...cached then
	begin
		Contract.beginMessage (FHandle, Integer (objectDirectoryGetObstacles));
		ptCentre.Pack(FHandle);
		Contract.PutDouble(FHandle, range);
		Contract.endMessage (FHandle);

		FObstacles.Unpack (FHandle);
		//...cached := true;
	end;
	result := FObstacles;
end;

procedure TObjectDirectory.SetToSpatialReference(toSpacRef: TSpatialReference);
begin
	Contract.BeginMessage(FHandle, Integer(objectDirectorySetToSpatialReference));
    toSpacRef.Pack(FHandle);
	Contract.EndMessage(FHandle);
end;
   {
function TObjectDirectory.Connect (connectionInfo: TConnectionInfo): String;
begin
	Contract.BeginMessage (FHandle, Integer(objectDirectoryConnect));
	connectionInfo.Pack (FHandle);
	Contract.EndMessage (FHandle);
	result := Contract.GetWideString (FHandle);
end;
    }
    {
procedure TObjectDirectory.DisConnect;
begin
	Contract.BeginMessage(FHandle, Integer(objectDirectoryDisConnect));
	Contract.EndMessage(FHandle);
end;
     }
function TObjectDirectory.getToSpatialReference(): TSpatialReference;
begin
	Contract.BeginMessage(FHandle, Integer(objectDirectoryGetConnectionInfo));
    Contract.EndMessage(FHandle);
	result := TSpatialReference.Create;
    result.unpack(FHandle);
end;

function TObjectDirectory.SetProcedure (aixmProcedureHandle: Integer): Boolean;
begin
	Contract.BeginMessage (FHandle, Integer (objectDirectorySetProcedure));
	Contract.PutInt32 (FHandle, aixmProcedureHandle);
	Contract.EndMessage (FHandle);
	result := Contract.GetBool (FHandle);
end;

end.