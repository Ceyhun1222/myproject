unit FunctionParams;

interface

uses
	FunctionCallManager,
	AIXMTypes,
	Parameter;

type
	TChangedFunctionParam = class (TFunctionParam)
	public
		function IsEqual (other: TChangedFunctionParam): Boolean; virtual; abstract;
	end;

	TPointParam = class (TChangedFunctionParam)
	public
		CheckEqual:			Boolean;
		SignifigantPoint:	TSignificantPoint;
		Direction:			Double;
		Distance:			Double;

		constructor Create ();

		function Clone (): TFunctionParam; override;
		procedure Assign (source: TFunctionParam); override;
		function IsEqual (other: TChangedFunctionParam): Boolean; override;
	end;

implementation

{ TPointParam }

procedure TPointParam.Assign(source: TFunctionParam);
var
	src:	TPointParam;
begin
	src := TPointParam (source);
	CheckEqual := src.CheckEqual;
	SignifigantPoint := src.SignifigantPoint;
	Direction := src.Direction;
	Distance := src.Distance;
end;

function TPointParam.Clone: TFunctionParam;
begin
	result := TPointParam.Create ();
	result.Assign (self);
end;

constructor TPointParam.Create;
begin
	CheckEqual := True;
	Direction := 0;
	Distance := 0;
end;

function TPointParam.IsEqual (other: TChangedFunctionParam): Boolean;
var
	pp:	TPointParam;
begin
	pp := TPointParam (other);

	result :=
		(SignifigantPoint = pp.SignifigantPoint) and
		(Direction = pp.Direction) and
		(Distance = pp.Distance);
end;

end.
