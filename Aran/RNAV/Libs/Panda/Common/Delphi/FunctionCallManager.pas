unit FunctionCallManager;

interface

uses
    Classes;

type

	TFunctionParam = class
	public
		function Clone (): TFunctionParam; virtual; abstract;
		procedure Assign (source: TFunctionParam); virtual; abstract;
	end;

	TFunctionEvent = procedure (this: TObject; param: TFunctionParam);
	TFunctionEventOfObject = procedure (param: TFunctionParam) of object;

	TFunctionInfo = class
	private
		FParameter:	TFunctionParam;

		procedure SetParameter (value: TFunctionParam);
	public
		Event:		TFunctionEvent;
		This:		TObject;

		constructor Create (vEvent: TFunctionEvent; vThis: TObject; vParameter: TFunctionParam);
		destructor Destroy (); override;

		procedure DoFunction ();

		property Parameter: TFunctionParam read FParameter write SetParameter;
	end;

	TFunctionCallManager = class
	private
		FCallCount:        	Integer;
		FFunctionList:		TList;
		FUpdating:         	Boolean;
		FLockedEndSuspend:	Boolean;
		FDefaultThis:		TObject;

		function AddFunction (functionEvent: TFunctionEvent; this: TObject; parameter: TFunctionParam): Boolean;
	public
		constructor Create ();
		destructor Destroy (); override;

		procedure Call (functionEvent: TFunctionEventOfObject; this: TObject; parameter: TFunctionParam); overload;
		procedure Call (functionEvent: TFunctionEventOfObject; parameter: TFunctionParam); overload;
		procedure Call (functionEvent: TFunctionEventOfObject); overload;
		procedure BeginSuspend ();
		procedure EndSuspend ();

		property DefaultThis: TObject read FDefaultThis write FDefaultThis;
	end;

implementation

//------------------------------------------------------------------------------
//--- TFunctionInfo ------------------------------------------------------------
//------------------------------------------------------------------------------

constructor TFunctionInfo.Create (vEvent: TFunctionEvent; vThis: TObject; vParameter: TFunctionParam);
begin
	Event := vEvent;
	This := vThis;

	if vParameter = nil then
		FParameter := nil
	else
		FParameter := vParameter.Clone ();
end;

destructor TFunctionInfo.Destroy;
begin
	FParameter.Free;
	Event := nil;
	inherited;
end;

procedure TFunctionInfo.DoFunction ();
begin
	Event (This, FParameter);
end;

procedure TFunctionInfo.SetParameter (value: TFunctionParam);
begin
	if value = nil then
	begin
		FParameter.Free;
		FParameter := nil;
	end
	else
	begin
		if FParameter = nil then
			FParameter := value.Clone ()
		else
			FParameter.Assign (value);
	end;
end;

//------------------------------------------------------------------------------
//--- TFunctionCallManager -----------------------------------------------------
//------------------------------------------------------------------------------

constructor TFunctionCallManager.Create ();
begin
	FCallCount := 0;
	FUpdating := False;
	FLockedEndSuspend := False;
	FFunctionList := TList.Create ();
end;

destructor TFunctionCallManager.Destroy;
begin
	FFunctionList.Free;
end;

procedure TFunctionCallManager.BeginSuspend ();
begin
	FCallCount := FCallCount + 1;
	FUpdating := True;
end;

procedure TFunctionCallManager.EndSuspend ();
var
	funcInfo:	TFunctionInfo;
begin
	if not FUpdating then
		exit;

	FCallCount := FCallCount - 1;

	if FCallCount > 0 then
		exit;

	if FLockedEndSuspend then
		exit;
	FLockedEndSuspend := True;

	while FFunctionList.Count > 0 do
	begin
		funcInfo := TFunctionInfo (FFunctionList.Items [0]);
		FFunctionList.Delete (0);		
		funcInfo.DoFunction ();
		funcInfo.Free;
	end;

	FUpdating := False;
	FLockedEndSuspend := False;
end;

function TFunctionCallManager.AddFunction (functionEvent: TFunctionEvent; this: TObject; parameter: TFunctionParam): Boolean;
var
	i:				Integer;
	functionInfo:	TFunctionInfo;
begin
	result := False;

	if not FUpdating then
		exit;
		
	result := True;

	for i :=0 to FFunctionList.Count - 1 do
	begin
		functionInfo := TFunctionInfo (FFunctionList.Items [i]);
		if @functionInfo.Event = @functionEvent then
		begin
			functionInfo.Free;
			FFunctionList.Delete (i);
			break;
		end
	end;

	functionInfo := TFunctionInfo.Create (functionEvent, this, parameter);
	FFunctionList.Add (functionInfo);
end;

procedure TFunctionCallManager.Call (functionEvent: TFunctionEventOfObject);
begin
	Call (functionEvent, FDefaultThis, nil);
end;

procedure TFunctionCallManager.Call (functionEvent: TFunctionEventOfObject; parameter: TFunctionParam);
begin
	Call (functionEvent, FDefaultThis, parameter);
end;

procedure TFunctionCallManager.Call (functionEvent: TFunctionEventOfObject; this: TObject; parameter: TFunctionParam);
	function Assgn(f: TFunctionEventOfObject): TFunctionEvent;
	asm
		mov edx, DWORD PTR f
		mov DWORD PTR result, edx
	end;
var
	p:	TFunctionEvent;
	b:	Boolean;
begin
	p := Assgn (functionEvent);
	b := AddFunction (p, this, parameter);

	if b then
		exit;

	p (this, parameter);
end;


end.
