unit DepartureService;

{ $DEFINE LATVIA}
{$DEFINE ESTONIA}
{ $DEFINE SWEDEN}
{ $DEFINE FINLAND}

{$DEFINE WITHOUT_TOOLITEM}	//Also in MainDForm.pas
{ $DEFINE NO_LICENSE_CHECK}  //Also in MainDForm.pas

interface

uses
	Contract,
    UIContract,
    DepartureGlobals,
    MainDForm,
    SettingsContract;

const
	ModuleName = 'Departure';


function Departure_EntryPoint (privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): Integer; stdcall;
procedure OnFormClose (Task: Integer);
procedure InitDataFrame ();

var
    GAranSettings:  TARANSettings;
    LIsFormLoaded:  Boolean = False;
    LMainForm:      TMainForm;

implementation

uses
	Windows, Classes, ARANGlobals, GeometryOperatorsContract,
	ARANFunctions, UnitConverter, ObjectDirectoryContract,
	Dialogs, Parameter, Registry, en, SysUtils;


{$IFNDEF WITHOUT_TOOLITEM}
var
	LToolBar:			TToolBar;
	LDepartureButton:	TCommandButton;
{$ENDIF}	

procedure OnFormClose (Task: Integer);
begin

{$IFNDEF WITHOUT_TOOLITEM}

	LDepartureButton.SetChecked(False);
	LDepartureButton.SetEnabled(True);

{$ENDIF}

  FreeObject (TObject (GPrjSR));
	FreeObject (TObject (GGeoSR));
	FreeObject (TObject (GObjectDirectory));
	FreeObject (TObject (GAranSettings));
	FreeObject (TObject (GLicenseRect));

    LIsFormLoaded := False;
end;

{
procedure Connect ();
var
	connectInfo:	TConnectionInfo;
begin
	if GObjectDirectory <> nil then
		exit;

	connectInfo := GAranSettings.GetConnectionInfo ();


	connectInfo := TConnectionInfo.Create ();
	connectInfo.ConnectionType := connectionTypePLTS;
	connectInfo.OtherConnection.ConnectionString :=
		'INSTANCE=sde:sqlServer:172.30.31.54\sqlExpress; DATABASE=ASPn2; AUTHENTICATION_MODE=OSA; VERSION=dbo.DEFAULT';


	GObjectDirectory := TObjectDirectory.Create;


	if GObjectDirectory.isValid then
	begin
		GCurrentUserName := GObjectDirectory.Connect (connectInfo);
		GObjectDirectory.ToSpatialReference := GPrjSR;
	end;

	connectInfo.Free;
end;
  }

procedure LoadLicenseRect ();
var
	Reg: TRegistry;
	KeyValue: String;
begin

{$IFDEF NO_LICENSE_CHECK}
	exit;
{$ENDIF}

	//--- begin: Create license area.
	Reg := TRegistry.Create(KEY_READ);
	try
		Reg.RootKey := HKEY_CURRENT_USER;
		Reg.OpenKey(PandaRegKeyPath + '\' + ModuleName, False);
		KeyValue := Reg.ReadString(KeyName);
	finally
		Reg.CloseKey;
		Reg.Free;
	end;

	try
		GLicenseRect := DecodeLCode (KeyValue, ModuleName);
	except
		GLicenseRect := nil;
	end;

	if LstStDtWriter(ModuleName) <> 0 then
	begin
		MessageDlg('CRITICAL ERROR!!', mtError, [mbOk], 0);
		GAranSettings.Free;
		FreeObject (TObject (GLicenseRect));
		exit;
	end;

	if (GLicenseRect = nil) or (GLicenseRect.Count < 1) then
	begin
		MessageDlg('ERROR #10LR512', mtError, [mbOk], 0);
		GAranSettings.Free;
		FreeObject (TObject (GLicenseRect));
		exit;
	end;
	//--- end.
end;

procedure OnDepartureClick (this: Pointer; Sender: TObject);
var
	MainProgram:	String;
	info0, info1,
	info2, info3:	integer;
begin

	GAranSettings := TAranSettings.Create ();

	if GAranSettings.IsFileNameEmpty then
	begin
		GAranSettings.Free;
		MessageDlg ('Please, save ArcMap document first.', mtWarning, [mbOk], 0);
		exit;
	end;

  GObjectDirectory := TObjectDirectory.Create;

	InitDataFrame ();
	LoadLicenseRect ();


	//Connect ();

	LMainForm := TMainForm.Create (nil);
	LIsFormLoaded := True;


  {
	GUi.GetEnvInfo (MainProgram, info0, info1, info2, info3);
	if IsWindow(info2) then
		SetWindowLong (LMainForm.Handle, GWL_HWNDPARENT, info2);

}
LMainForm.Show;
{$IFNDEF WITHOUT_TOOLITEM}
 //	LDepartureButton.SetChecked(True);
	//LDepartureButton.SetEnabled(False);
{$ENDIF}

end;

procedure SetOrganisation ();
begin
{$IFDEF LATVIA}
    GOrgID := '4478555';
{$ENDIF}

{$IFDEF ESTONIA}
	GOrgID := '4478535';
{$ENDIF}

{$IFDEF SWEDEN}
	GOrgID := '7104';
{$ENDIF}

{$IFDEF FINLAND}
	GOrgID := '4478537';
{$ENDIF}
end;


procedure SetGeoSpatialReference ();
begin
	GGeoSR := TSpatialReference.Create;

	GGeoSR.name := 'WGS1984';
	GGeoSR.spatialReferenceType := srtGeographic;
	GGeoSR.spatialReferenceUnit := sruMeter;
	GGeoSR.ellipsoid.srGeoType := srgtWGS1984;
	GGeoSR.ellipsoid.semiMajorAxis := 6378137.0;
	GGeoSR.ellipsoid.flattening := 1 / 298.25722356300003;
end;

procedure InitDataFrame ();
begin
	SetOrganisation ();
  GPrjSR:= GUI.ViewProjection;
	SetGeoSpatialReference ();

	InitEllipsoid (GGeoSR.ellipsoid.semiMajorAxis, 1.0 / GGeoSR.ellipsoid.flattening);

	GHorisontalDistIn   := hduKM;
	GHorisontalDistOut  := GAranSettings.GetDistanceUnit;	// hduKM;

	GVerticalDistIn     := vduMeter;
	GVerticalDistOut    := GAranSettings.GetElevationUnit;		// vduMeter;

	GHorisontalSpeedIn  := hsuKMInHour;
	GHorisontalSpeedOut := GAranSettings.GetSpeedUnit;		// hsuKMInHour;

	GVerticalSpeedIn    := vsuMeterInMin;
	GVerticalSpeedOut   := vsuMeterInMin;

	GGradientUnitName   := '%';
	GAngleUnitName      := '°';

	GDistanceUnitName   := GHorisontalDistanceUnitName [GHorisontalDistOut];
	GAltitudeUnitName   := GVerticalDistanceUnitName [GVerticalDistOut];
	GHSpeedUnitName     := GHorisontalSpeedUnitName [GHorisontalSpeedOut];
	GVSpeedUnitName     := GVerticalSpeedUnit [GVerticalSpeedOut];

	GDistanceAccuracy   := GAranSettings.GetDistanceAccuracy;
	GAltitudeAccuracy	:= GAranSettings.GetElevationAccuracy;
	GGradientAccuracy	:= GAranSettings.GetGradientAccuracy;
	GAngleAccuracy		:= GAranSettings.GetAngleAccuracy;
	GHSpeedAccuracy		:= GAranSettings.GetSpeedAccuracy;
	GVSpeedAccuracy		:= 1;

	NULLConverter.UnitName      := '';
	DistanceConverter.UnitName	:= GDistanceUnitName;
	AltitudeConverter.UnitName	:= GAltitudeUnitName;
	GradientConverter.UnitName	:= GGradientUnitName;
	AngleConverter.UnitName		:= GAngleUnitName;
	HSpeedConverter.UnitName	:= GHSpeedUnitName;
	VSpeedConverter.UnitName	:= GVSpeedUnitName;

	NULLConverter.Accuracy		:= 1;
	DistanceConverter.Accuracy	:= GDistanceAccuracy;
	AltitudeConverter.Accuracy	:= GAltitudeAccuracy;
	GradientConverter.Accuracy	:= GGradientAccuracy;
	AngleConverter.Accuracy		:= GAngleAccuracy;
	HSpeedConverter.Accuracy	:= GHSpeedAccuracy;
	VSpeedConverter.Accuracy	:= GVSpeedAccuracy;
end;

procedure DepartureServiceInitialize ();
begin
	DecimalSeparator := '.';
	
  if LIsFormLoaded then
    begin
        if LMainForm <> nil then
            LMainForm.Show ();
        exit;
	end;

	GObjectDirectory := nil;

	GUi := TUIContract.Create;

{$IFDEF WITHOUT_TOOLITEM}
	OnDepartureClick (Pointer (nil), nil);
{$ELSE}
	try
		LToolBar := GUi.CreateToolBar ('Departure Bar');

		LDepartureButton := GUi.CreateCommandButton('depDeparture');
 {
		LDepartureButton.SetStyle(csTextOnly);
		LDepartureButton.SetCaption ('Departure');
		LDepartureButton.SetToolTip ('Departure');
		LDepartureButton.SetMessage('Departure');
		LDepartureButton.SetCategory('Departure');
		LDepartureButton.SetOnClickHandler (nil, OnDepartureClick);
		LToolBar.AddCommandButton (LDepartureButton);
    }
	except
		FreeObject (TObject (GUi));
	end;
{$ENDIF}
end;

procedure DepartureServiceTerminate ();
begin
	FreeObject (TObject (GUi));
end;

function Departure_EntryPoint(privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): Integer; stdcall;
begin
	result := rcInvalid;

	case command of
		svcGetInstance:
			begin
				DepartureServiceInitialize ();
			end;

		svcFreeInstance:
			begin
            	DepartureServiceTerminate ();
			end;
	end;
end;

exports
	Departure_EntryPoint Name 'Departure_EntryPoint';

end.
