{$INCLUDE Settings}

unit ApproachService;
{ $DEFINE RIGA}
{ $DEFINE SWEDEN}
{ $DEFINE ESTONIA}
{ $DEFINE FINLAND}

interface

uses
	Contract, UIContract, Geometry, GeometryOperatorsContract,Loader;

const
	ModuleName = 'Approach';

function Approach_EntryPoint(PrivateData: TRegistryHandle; Command: Integer; InOut: TRegistryHandle): Integer; stdcall;
procedure OnFormClose(Task: Integer);
function InitDataFrame: Integer;

var
	GApproachButton:	TCommandButton;
	GReportButton:		TCommandButton;
//	P_LicenseRect:		TPolygon;

implementation

uses
	Windows, Classes, ARANGlobals, ApproachGlobals, UnitConverter, Parameter,
	ARANFunctions, ApproachForm, ReportFormUnit, SettingsContract, Dialogs,
	Registry, en, SysUtils;

var
	ToolBar:		TToolBar;

procedure OnFormClose(Task: Integer);
begin
	case Task of
	0:	begin
			GApproachButton.SetChecked(False);
			GApproachButton.SetEnabled(True);
			GReportButton.SetEnabled(False);

			FreeObject(TObject(GAranSettings));
			FreeObject(TObject(GPrjSR));
//			FreeObject(TObject(GGeoSR));
{$IFNDEF NOLIC}
			GLicenseRect.Free;
			GLicenseRect := nil;
{$ENDIF}
			FinitGlobals;
		end;

	1:	begin

		end;
	end;
end;

procedure OnApproachHandler(this: Pointer; Sender: TObject);
{$IFNDEF NOLIC}
var
	Reg:		TRegistry;
	KeyValue:	String;
{$ENDIF}
begin
	GPrjSR := nil;
	GGeoSR := nil;

	GAranSettings := TARANSettings.Create;

	if GAranSettings.IsFileNameEmpty then
	begin
		MessageDlg ('Please, save document first.', mtWarning, [mbOk], 0);
		GAranSettings.Free;
		exit;
	end;

	InitDataFrame;
//==============================================================================
{$IFNDEF NOLIC}
	Reg := TRegistry.Create(KEY_READ);
	try
		Reg.RootKey := HKEY_CURRENT_USER;
		Reg.OpenKey(PandaRegKeyPath + '\' + ModuleName, False);
		KeyValue := Reg.ReadString(KeyName);
	finally
		Reg.CloseKey;
		Reg.Free;
	end;

	GLicenseRect := DecodeLCode(KeyValue, ModuleName);

	if LstStDtWriter(ModuleName) <> 0 then
	begin
		MessageDlg('CRITICAL ERROR!!', mtError, [mbOk], 0);
		GAranSettings.Free;
		GLicenseRect.Free;
		GLicenseRect := nil;
		exit;
	end;

	if GLicenseRect.Count < 1 then
	begin
		MessageDlg('ERROR #10LR512', mtError, [mbOk], 0);
		GAranSettings.Free;
		GLicenseRect.Free;
		GLicenseRect := nil;
		exit;
	end;
{$ENDIF}
//==============================================================================

	InitGlobals;
  //MainForm.Free;
	MainForm := TMainForm.Create(nil);

	if IsWindow(GHandle) then
		SetWindowLong(MainForm.Handle, GWL_HWNDPARENT, GHandle);
	MainForm.Show;
 //	GApproachButton.SetChecked(True);
	//GApproachButton.SetEnabled(False);
end;

procedure OnReportHandler(this: Pointer; Sender: TObject);
begin
	if Assigned(ReportForm) then
	begin
		if IsWindow(GHandle) then
			SetWindowLong(ReportForm.Handle, GWL_HWNDPARENT, GHandle);
		ReportForm.Show;
	end;
end;

function SetOrganisation: Integer;
begin
//	GOrgID := GetInt(InOut);

{$IFDEF RIGA}
	GOrgID := '4478555';			//Lat
{$ELSE}
	{$IFDEF SWEDEN}
		GOrgID := '7104';			//SW
	{$ELSE}
		{$IFDEF ESTONIA}
			GOrgID := '4478535';	//ESTONIA
		{$ELSE}
			{$IFDEF FINLAND}
				GOrgID := '4478537';	//FINLAND
			{$ELSE}
				result := rcInvalid;
				exit;
			{$ENDIF}
		{$ENDIF}
	{$ENDIF}
{$ENDIF}
	result := rcOK
end;

function SetProjectedSpatialReference: Integer;
var
	defSpatialRef:  TSpatialReference;
begin
	result := rcInvalid;
	if not Assigned(GAranSettings) then
		exit;

	defSpatialRef := GUI.ViewProjection;
 //	GPrjSR := GAranSettings.GetPrjSpatialReference (defSpatialRef);
   GPrjSR := defSpatialRef;
	defSpatialRef.Free;
	//GUI.ViewProjection := GPrjSR;

	result := rcOK
end;

function SetGeoSpatialReference: Integer;
begin
	GGeoSR := TSpatialReference.Create;
//	GGeoSR.unpack(InOut);

	GGeoSR.name := 'WGS1984';
	GGeoSR.spatialReferenceType := srtGeographic;
	GGeoSR.spatialReferenceUnit := sruMeter;
	GGeoSR.ellipsoid.srGeoType := srgtWGS1984;
	GGeoSR.ellipsoid.semiMajorAxis := 6378137.0;
	GGeoSR.ellipsoid.flattening := 1 / 298.25722356300003;

	result := rcOK
end;

function InitDataFrame: Integer;
begin
	result := rcInvalid;
	if not Assigned(GAranSettings) then
		exit;

	SetOrganisation;
 	SetProjectedSpatialReference;
	SetGeoSpatialReference;

	InitEllipsoid (GGeoSR.ellipsoid.semiMajorAxis, 1.0 / GGeoSR.ellipsoid.flattening);

	GHorisontalDistIn := hduKM;
	GHorisontalDistOut := GAranSettings.GetDistanceUnit;	// hduKM;

	GVerticalDistIn := vduMeter;
	GVerticalDistOut := GAranSettings.GetElevationUnit;		// vduMeter;

	GHorisontalSpeedIn := hsuKMInHour;
	GHorisontalSpeedOut := GAranSettings.GetSpeedUnit;		// hsuKMInHour;

	GVerticalSpeedIn := vsuMeterInMin;
	GVerticalSpeedOut := vsuMeterInMin;

	GGradientUnitName	:= '%';
	GAngleUnitName		:= '°';

	GDistanceUnitName	:= GHorisontalDistanceUnitName [GHorisontalDistOut];
	GAltitudeUnitName	:= GVerticalDistanceUnitName [GVerticalDistOut];
	GHSpeedUnitName		:= GHorisontalSpeedUnitName [GHorisontalSpeedOut];
	GVSpeedUnitName		:= GVerticalSpeedUnit [GVerticalSpeedOut];

	GDistanceAccuracy	:= GAranSettings.GetDistanceAccuracy;
	GAltitudeAccuracy	:= GAranSettings.GetElevationAccuracy;
	GGradientAccuracy	:= GAranSettings.GetGradientAccuracy;

	GDirAngleAccuracy	:= GAranSettings.GetAngleAccuracy;
	GAngleAccuracy		:= GAranSettings.GetAngleAccuracy;

	GHSpeedAccuracy		:= GAranSettings.GetSpeedAccuracy;
	GVSpeedAccuracy		:= 1;

	NULLConverter.UnitName				:= '';
	DistanceConverter.UnitName			:= GDistanceUnitName;
	AltitudeConverter.UnitName			:= GAltitudeUnitName;
	GradientConverter.UnitName			:= GGradientUnitName;

	DirAngleConverter.UnitName			:= GAngleUnitName;
	AngleConverter.UnitName				:= GAngleUnitName;

	HSpeedConverter.UnitName			:= GHSpeedUnitName;
	VSpeedConverter.UnitName			:= GVSpeedUnitName;

	NULLConverter.Accuracy				:= 1;
	DistanceConverter.Accuracy			:= GDistanceAccuracy;
	AltitudeConverter.Accuracy			:= GAltitudeAccuracy;
	GradientConverter.Accuracy			:= GGradientAccuracy;

	DirAngleConverter.Accuracy			:= GDirAngleAccuracy;
	AngleConverter.Accuracy				:= GAngleAccuracy;

	HSpeedConverter.Accuracy			:= GHSpeedAccuracy;
	VSpeedConverter.Accuracy			:= GVSpeedAccuracy;

	result := rcOK
end;

function Approach_EntryPoint(PrivateData: TRegistryHandle; Command: Integer; InOut: TRegistryHandle): Integer; stdcall;
var
	MainProgram:	String;
	info0, info1,
	info2, info3:	integer;
begin
	result := rcOK;
	case command of
		svcGetInstance:
			begin
				GUI := TUIContract.Create;
				try
					//Rasim m. Men bunu telem-telesik yaziram. Burda mueyyen
					//* qerar qebul etmek lazimdir.
					//* Anar Niftili.
					DecimalSeparator := '.';
          OnApproachHandler(nil,nil);
          {
					GUI.GetEnvInfo(MainProgram, info0, info1, info2, info3);
					GHandle := info2;
             }
					GApproachButton := GUI.CreateCommandButton('Approach');
         {
					GApproachButton.SetStyle(csTextOnly);
					GApproachButton.SetCaption ('Approach');
					GApproachButton.SetToolTip ('Approach');
					GApproachButton.SetMessage('Approach');
					GApproachButton.SetCategory('Approach');
					GApproachButton.SetOnClickHandler(nil, OnApproachHandler);
					//GApproachButton.
					//3000
           }
					GReportButton := GUI.CreateCommandButton('Report');
          {
					GReportButton.SetStyle(csTextOnly);
					GReportButton.SetCaption ('Report');
					GReportButton.SetToolTip ('Report');
					GReportButton.SetMessage('Report');
					GReportButton.SetCategory('Report');
					GReportButton.SetOnClickHandler(nil, OnReportHandler);
					GReportButton.SetEnabled(False);

					ToolBar := GUI.CreateToolBar('Approach Bar');

					ToolBar.AddCommandButton(GApproachButton);
					ToolBar.AddCommandButton(GReportButton);
                                        }
					result := rcOK
				except
					FreeObject(TObject(GUI));
				end;
			end;
		svcFreeInstance:
			begin
				FreeObject(TObject(GUI));
				result := rcOK
			end;
		Integer(lcInitDataFrame):
			result := InitDataFrame;
		Integer(lcSetOrganisation):
			result := SetOrganisation;
		Integer(lcSetProjectedSpatialReference):
			result := SetProjectedSpatialReference;
		Integer(lcSetGeoSpatialReference):
			result := SetGeoSpatialReference;
	end;
end;

exports
	Approach_EntryPoint Name 'Approach_EntryPoint';

end.

