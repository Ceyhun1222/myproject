unit TestService;

{ $DEFINE LATVIA}
{$DEFINE ESTONIA}
{ $DEFINE SWEDEN}
{ $DEFINE FINLAND}

{$DEFINE WITHOUT_TOOLITEM}	//Also in MainDForm.pas
{ $DEFINE NO_LICENSE_CHECK}  //Also in MainDForm.pas

interface

uses
  Contract,
  MainDForm;

const
	ModuleName = 'Test';


function TestService_EntryPoint (privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): Integer; stdcall;


implementation

uses
	Windows,
  Forms;

var
    GForm:  TForm = nil;

function OnTestServiceClick (): Integer;
var
  form1:  TForm1;
begin

  if GForm = nil then
  begin
    form1 := TForm1.Create(nil);
    GForm := form1;
  end;

  GForm.Show;

  result := rcOK
end;

procedure TestServiceTerminate ();
begin
end;

function TestService_EntryPoint(privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): Integer; stdcall;
begin
	result := rcOK;

	case command of
	//	svcGetInstance:
		//	begin
		 //	end;

		svcFreeInstance:
			begin
        TestServiceTerminate ();
			end;

    svcGetInstance:
      begin
        result:= OnTestServiceClick ();
      end;
	end;
end;

exports
	TestService_EntryPoint Name 'TestService_EntryPoint';

end.
