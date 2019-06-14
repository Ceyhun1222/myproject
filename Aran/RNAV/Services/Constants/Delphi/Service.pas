unit Service;

interface

implementation

uses
	Windows, SysUtils, Contract, ConstantsContract, LoaderUnit, Registry;

var
	PANS_OPS_Path1,
	PANS_OPS_Path2,
	GNSS_Path,
	PBN_RNP_APCH_Path,
	AircraftCategoryPath,
	FTE_Path: String;

{
	PANS_OPS_Path1: String = 'C:\Program Files\Panda\RNAV\pansops.dat';
	PANS_OPS_Path2: String = 'C:\Program Files\Panda\RNAV\arpansops.dat';
	GNSS_Path: String = 'C:\Program Files\Panda\RNAV\gnss.dat';
	AircraftCategoryPath: String = 'C:\Program Files\Panda\RNAV\categories.dat';
}

var
	PANSOPS_ConstantListLoader:			TPANSOPSConstantListLoader;
	GNSS_ConstantListLoader:			TGNSSConstantListLoader;
	PBN_RNP_APCH_ConstantListLoader:	TGNSSConstantListLoader;
	AircraftCategoryListLoader:			TAircraftCategoryListLoader;
	FTEConstantListLoader:				TPANSOPSConstantListLoader;

function InitService: Integer;
var
	Reg:	TRegistry;
	Path:	String;
	Filename:	Array [0..1023] of char;
	TmpPANSOPS_ConstantListLoader:	TPANSOPSConstantListLoader;
begin
	Path := '';

	Reg := TRegistry.Create;
	try
		Reg.RootKey := HKEY_CURRENT_USER;
		if Reg.OpenKey('\Software\RISK\PANDA\RNAV', False) then
		begin
			Path := Reg.ReadString('Path');
			Reg.CloseKey;
		end;
	finally
		Reg.Free;
	end;

	if Path = '' then
	begin
		GetModuleFileName(hInstance, Filename, 1024);
		Path := ExtractFilePath(Filename);
	end;

	if Path <> '' then
	begin
		PANS_OPS_Path1 := Path + 'Constants\pansops.dat';
		PANS_OPS_Path2 := Path + 'Constants\arpansops.dat';
		GNSS_Path := Path + 'Constants\gnss.dat';
		PBN_RNP_APCH_Path := Path + 'Constants\PBN RNP APCH.dat';
		AircraftCategoryPath := Path + 'Constants\categories.dat';
		FTE_Path := Path + 'Constants\FTE.dat';
	end;

	PANSOPS_ConstantListLoader	:= TPANSOPSConstantListLoader.Create;
	GNSS_ConstantListLoader		:= TGNSSConstantListLoader.Create;
	AircraftCategoryListLoader	:= TAircraftCategoryListLoader.Create;
	PBN_RNP_APCH_ConstantListLoader := TGNSSConstantListLoader.Create;
	FTEConstantListLoader :=TPANSOPSConstantListLoader.Create;

	try
		PANSOPS_ConstantListLoader.LoadFromFile(PANS_OPS_Path1);
	except
	end;

	TmpPANSOPS_ConstantListLoader := TPANSOPSConstantListLoader.Create;
	try
		TmpPANSOPS_ConstantListLoader.LoadFromFile(PANS_OPS_Path2);
		PANSOPS_ConstantListLoader.Merge(TmpPANSOPS_ConstantListLoader);
	except
	end;
	TmpPANSOPS_ConstantListLoader.Free;

	try
		GNSS_ConstantListLoader.LoadFromFile(GNSS_Path, pcGNSS);
	except
	end;

	try
		AircraftCategoryListLoader.LoadFromFile(AircraftCategoryPath);
	except
	end;

	try
		FTEConstantListLoader.LoadFromFile(FTE_Path);
	except;
	end;

	try
		PBN_RNP_APCH_ConstantListLoader.LoadFromFile(PBN_RNP_APCH_Path, pcRNP_APCH);
	except
	end;


end;

procedure FinitService;
begin
	PANSOPS_ConstantListLoader.Free;
	GNSS_ConstantListLoader.Free;
	PBN_RNP_APCH_ConstantListLoader.Free;
	AircraftCategoryListLoader.Free;
	FTEConstantListLoader.Free;
end;

procedure GetPANS_OPSConstants(InOut: TRegistryHandle);
begin
	PANSOPS_ConstantListLoader.Pack(InOut);
end;

procedure GetGNSSConstants(InOut: TRegistryHandle);
begin
	GNSS_ConstantListLoader.Pack(InOut);
end;

procedure GetPBN_RNP_APCHConstants(InOut: TRegistryHandle);
begin
	PBN_RNP_APCH_ConstantListLoader.Pack(InOut);
end;

procedure GetAircraftCategoryConstants(InOut: TRegistryHandle);
begin
	AircraftCategoryListLoader.Pack(InOut);
end;

procedure GetFTEConstants(InOut:TRegistryHandle);
begin
	FTEConstantListLoader.Pack(InOut);
end;

function Constants_EntryPoint (PrivateData: TRegistryHandle; Command: Integer; InOut: TRegistryHandle): Integer; stdcall;
begin
	try
		Result := rcOk;

		case command of
		svcGetInstance:
			Result := InitService;

		svcFreeInstance:
			FinitService;

		Integer(ccGetPANS_OPSConstants):
			GetPANS_OPSConstants(inout);

		Integer(ccGetGNSSConstants):
			GetGNSSConstants(inout);

		Integer(ccPBN_RNP_APCHConstants):
			GetPBN_RNP_APCHConstants(inout);

		Integer(ccGetAircraftCategoryConstants):
			GetAircraftCategoryConstants(inout);

//		Integer(ccGetFTEContants):
//			GetFTEConstants(inout);
		else
			Result := rcInvalid;
		end;

	except
		Result := rcException;
	end;
end;

exports
	Constants_EntryPoint;

end.
