unit FIXInfoUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Parameter, StdCtrls, FIXUnit;

type
  TFIXInfoForm = class(TForm)
    labAltitude: TLabel;
    editAltitude: TEdit;
	labAltitudeUnit: TLabel;

	lblTAS: TLabel;
	edtTAS: TEdit;
	lblTASUnit: TLabel;

	lblWindSpeed: TLabel;
	edtWindSpeed: TEdit;
	lblWindSpeedUnit: TLabel;

	lblRTurn: TLabel;
	edtRTurn: TEdit;
	lblRTurnUnit: TLabel;

	prmAltitude: TParameter;
	prmTAS: TParameter;
	prmWindSpeed: TParameter;

	prmRTurn: TParameter;
	lblEWindSpiral: TLabel;
	edtEWindSpiral: TEdit;

	lblEWindSpiralUnit: TLabel;
    prmEWindSpiral: TParameter;
	lblLatestTPp: TLabel;

	edtLatestTPp: TEdit;
	lblLatestTPpUnit: TLabel;
	prmLatestTPp: TParameter;
    lblTurnAngle: TLabel;
    edtTurnAngle: TEdit;
    lblTurnAngleUnit: TLabel;
    prmTurnAngle: TParameter;
    procedure FormDeactivate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
	{ Private declarations }
  public
    { Public declarations }
  end;

procedure ShowFixInfo(X, Y: Integer; FIX: TWayPoint);

var
  FIXInfoForm: TFIXInfoForm;

implementation

{$R *.dfm}

uses
	Math, ConstantsContract, ApproachGlobals;

procedure ShowFixInfo(X, Y: Integer; FIX: TWayPoint);
var
	WindSpeed, EWindSpiral, RTurn, Rv:		Double;
begin
	WindSpeed := GPANSOPSConstants.Constant[dpWind_Speed].Value;
	RTurn := FIX.CalcTurnRadius;

	Rv := 1765.27777777777777777 * Tan(FIX.Bank) / (PI * FIX.TAS);
	if Rv > 3 then	Rv := 3;

	EWindSpiral := RTurn + 90 * WindSpeed / Rv;

	FIXInfoForm := TFIXInfoForm.Create(nil);
	FIXInfoForm.prmAltitude.ChangeValue(FIX.Altitude);
	FIXInfoForm.prmTAS.ChangeValue(FIX.TAS);

	FIXInfoForm.prmWindSpeed.ChangeValue(WindSpeed);
	FIXInfoForm.prmLatestTPp.ChangeValue(FIX.LPT * (1 - 2 * Byte(FIX.FlyMode = fmFlyBy)));
	FIXInfoForm.prmRTurn.ChangeValue(RTurn);
	FIXInfoForm.prmEWindSpiral.ChangeValue(EWindSpiral);
	FIXInfoForm.edtTurnAngle.Text := ForMatFloat('0.###', RadToDeg(FIX.TurnAngle));
	//FIXInfoForm.prmTurnAngle.ChangeValue(FIX.TurnAngle);

	FIXInfoForm.Left := X;
	FIXInfoForm.Top := Y;

	FIXInfoForm.Show;
end;

procedure TFIXInfoForm.FormDeactivate(Sender: TObject);
begin
	Close;
end;

procedure TFIXInfoForm.FormClose(Sender: TObject;
  var Action: TCloseAction);
begin
	Action := caFree;
end;

end.

