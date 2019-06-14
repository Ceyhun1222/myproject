unit SettingsContract;

interface

uses
	Contract,
	GeometryOperatorsContract,
	SysUtils,
	Classes;

type
  TSettingsCommands =
    (
    settingsGetFileName,
    settingsIsFileNameEmpty,
    settingsGetLanguageCode,
    settingsGetDistanceUnit,
    settingsGetElevationUnit,
    settingsGetSpeedUnit,
    settingsGetDistanceAccuracy,
    settingsGetElevationAccuracy,
    settingsGetSpeedAccuracy,
    settingsGetAngleAccuracy,
    settingsGetGradientAccuracy,
    settingsGetDBAirportFilterList
    );
	THorisontalDistanceUnit = (hduKM, hduNM);
	TVerticalDistanceUnit = (vduMeter, vduFeet);
	THorisontalSpeedUnit = (hsuKMInHour, hsuKnot);
	TVerticalSpeedUnit = (vsuMeterInMin, vsuFeetInMin);

	TLanguageCode = (langCodeEng = 1033, langCodeRus = 1049, langCodePortuguese = 1046);

const
	GHorisontalDistanceUnitName:Array [THorisontalDistanceUnit] of string = ('km', 'NM');
	GVerticalDistanceUnitName:	Array [TVerticalDistanceUnit] of string = ('m', 'ft');
	GHorisontalSpeedUnitName:	Array [THorisontalSpeedUnit] of string = ('km/h', 'knot');

	GVerticalSpeedUnit:			Array [TVerticalSpeedUnit] of string = ('m/min', 'feet/min');


type
    TARANSettings = class
    private
    	  FHandle:	THandle;
        function GetIsFileNameEmpty (): Boolean;
        function GetFileName (): String;
    public
    	constructor Create;
      destructor Destroy; override;

		  function IsValid (): Boolean;
		  function GetLanguageCode (): TLanguageCode;
		  function GetDistanceUnit (): THorisontalDistanceUnit;
		  function GetElevationUnit (): TVerticalDistanceUnit;
		  function GetSpeedUnit (): THorisontalSpeedUnit;
	  	function GetDistanceAccuracy (): Double;
		  function GetElevationAccuracy (): Double;
		  function GetSpeedAccuracy (): Double;
		  function GetAngleAccuracy (): Double;
		  function GetGradientAccuracy (): Double;
  		property IsFileNameEmpty: Boolean read GetIsFileNameEmpty;
	  	property FileName: String read GetFileName;
	end;

implementation


constructor TARANSettings.Create;
begin
	FHandle := Contract.GetInstance ('SettingsService');
end;

destructor TARANSettings.Destroy;
begin
    Contract.FreeInstance (FHandle);
	inherited;
end;

function TARANSettings.GetIsFileNameEmpty (): Boolean;
begin
    Contract.BeginMessage (FHandle, Integer (settingsIsFileNameEmpty));
    Contract.EndMessage (FHandle);
	result := Contract.GetBool (FHandle);
end;

function TARANSettings.GetFileName: String;
begin
    Contract.BeginMessage (FHandle, Integer (settingsGetFileName));
    Contract.EndMessage (FHandle);
    result := Contract.GetWideString (FHandle);
end;

function TARANSettings.GetDistanceAccuracy: Double;
var
 distanceAccuracy:Integer;
begin
  	Contract.BeginMessage (FHandle, Integer (settingsGetDistanceAccuracy));
    Contract.EndMessage (FHandle);
	  result := Contract.GetDouble (FHandle)
end;

function TARANSettings.GetDistanceUnit: THorisontalDistanceUnit;
begin
	try
			Contract.BeginMessage (FHandle, Integer (settingsGetDistanceUnit));
      Contract.EndMessage (FHandle);
	    result :=THorisontalDistanceUnit(Contract.GetInt32 (FHandle));
	except
    	result := hduKM;
    end;
end;

function TARANSettings.GetElevationAccuracy: Double;
begin
	try
	    	Contract.BeginMessage (FHandle, Integer (settingsGetElevationAccuracy));
        Contract.EndMessage (FHandle);
	      result := Contract.GetDouble (FHandle)
    except
    	result := 1;
    end;
end;

function TARANSettings.GetElevationUnit: TVerticalDistanceUnit;
begin
	try
		  Contract.BeginMessage (FHandle, Integer (settingsGetElevationUnit));
      Contract.EndMessage (FHandle);
	    result :=TVerticalDistanceUnit(Contract.GetInt32 (FHandle));
    except
    	result := vduMeter;
	end;
end;

function TARANSettings.GetSpeedAccuracy: Double;
begin
	try
			Contract.BeginMessage (FHandle, Integer (settingsGetSpeedAccuracy));
      Contract.EndMessage (FHandle);
	    result := Contract.GetDouble (FHandle);
    except
    	result := 1;
    end;
end;

function TARANSettings.GetSpeedUnit: THorisontalSpeedUnit;
begin
	try
		  Contract.BeginMessage (FHandle, Integer (settingsGetSpeedUnit));
      Contract.EndMessage (FHandle);
	    result :=THorisontalSpeedUnit(Contract.GetInt32 (FHandle));
    except
    	result := hsuKMInHour;
    end;
end;

function TARANSettings.GetAngleAccuracy: Double;
begin
	try
			Contract.BeginMessage (FHandle, Integer (settingsGetAngleAccuracy));
      Contract.EndMessage (FHandle);
	    result := Contract.GetDouble (FHandle);
    except
    	result := 1;
    end;
end;

function TARANSettings.GetLanguageCode: TLanguageCode;
begin
  Contract.BeginMessage(FHandle,Integer(settingsGetLanguageCode));
  Contract.EndMessage(FHandle);
  result:= TLanguageCode(Contract.GetInt32(FHandle));
	//result := TLanguageCode (GetInt32 ('General/LangCode', 1033));
end;

function TARANSettings.GetGradientAccuracy: Double;
begin
	try
		  Contract.BeginMessage (FHandle, Integer (settingsGetGradientAccuracy));
      Contract.EndMessage (FHandle);
	    result := Contract.GetDouble (FHandle);
    except
    	result := 0.1;
    end;
end;

function TARANSettings.IsValid: Boolean;
begin
	result := (FHandle <> 0);
end;
end.
