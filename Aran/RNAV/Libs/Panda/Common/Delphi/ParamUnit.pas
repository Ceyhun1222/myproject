unit ParamUnit;

interface

uses
  SysUtils, Classes, Parameter, FIXUnit, APVBaroVNAVForm;
                                       
type
  TParamModule = class(TDataModule)
  private
	{ Private declarations }
  public
	{ Public declarations }
// ===
//	FIF:							TFIX;
//	FIFInfo:						TFIXInfo;
//=== FAF
	FAF:							TFIX;
	FAFInfo:						TFIXInfo;
//=== MAPt
	MAPt:							TMAPt;
	MAPtInfo:						TFIXInfo;
  end;

var
	  ParamModule: TParamModule;


implementation

{$R *.dfm}

end.
