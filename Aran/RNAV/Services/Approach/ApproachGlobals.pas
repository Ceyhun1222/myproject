unit ApproachGlobals;

interface

uses
	Windows, ConstantsContract, ObjectDirectoryContract, GeometryOperatorsContract,
	AIXMTypes, CollectionUnit, SettingsContract, ARANGlobals;//, UIContract

var
	GGeoOperators: 				TGeometryOperators;
//	GConstants:					TConstants;	GConst:		TConstants;


	GPANSOPSConstants:			TPANSOPSConstantList;
	GGNSSConstants:				TGNSSConstantList;
	GAircraftCategoryConstants:	TAircraftCategoryList;
	GHandle:				HWND;

{	GHDistConvertString:	String;
	GVDistConvertString:	String;
	GHSpeedConvertString:	String;
	GVSpeedConvertString:	String;	// = '0.#'
}
	GAranSettings:			TARANSettings;
//const
//	GAngleConvertString = '0';
//	GGradientConvertString = '0.0';

function InitGlobals: Boolean;
function FinitGlobals: Boolean;


implementation

uses
	ARANFunctions, UnitConverter;


function InitGlobals: Boolean;
//var
 //	ConnectInfo:	TConnectionInfo;
begin
 //	ConnectInfo := GAranSettings.GetConnectionInfo;
{
	connectInfo := TConnectionInfo.Create;
(*
	ConnectInfo.connectionType := connectionTypePostgres;
	ConnectInfo.hostName := '172.30.31.54';
	ConnectInfo.dbName := 'aran_new';
	ConnectInfo.userName := 'aran';
	ConnectInfo.password := 'aran';
*)

	connectInfo.connectionType := connectionTypeMDB;
	connectInfo.hostName := 'C:\WORK\Panda\ARAN\WorkFolder\DB\ARAN.mdb';
	connectInfo.dbName := '';
	connectInfo.userName := 'Admin';
	connectInfo.password := '';
}
	GObjectDirectory := TObjectDirectory.Create;
	if GObjectDirectory.isValid then
	begin
	//	GCurrentUserName := GObjectDirectory.Connect(ConnectInfo);
	 //	GObjectDirectory.ToSpatialReference := GPrjSR;
	end;

	//ConnectInfo.Free;

	GPANSOPSConstants := GConst.PANSOPS;
	GGNSSConstants := GConst.GNSS;
	GAircraftCategoryConstants := GConst.AircraftCategory;
	GGeoOperators := TGeometryOperators.Create;
	result := True;
end;

function FinitGlobals: Boolean;
begin
	FreeObject(TObject(GObjectDirectory));
	FreeObject(TObject(GGeoOperators));
	result := True;
end;

end.
