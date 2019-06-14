unit ParameterUnit;

interface

uses
	Classes, Controls, StdCtrls, UnitConverter, IntervalUnit, Geometry,
	ARANFunctions, ARANMath;

type

	TParameter = class;

	TOnChangeValueEvent = procedure (Sender: TParameter) of object;

	TParameter = class
	private
		FParameterType:			TParameterUnit;
		FOldValue,
		FValue:					Double;
		FValidRange:			TInterval;

		FInConvertionPoint:		TPoint;
		FOutConvertionPoint:	TPoint;

		FAutoArrange,
		FUpdating,
		FVisible,
		FReadOnly,
		FPerformCheck:			Boolean;

		FDescriptionControl,
		FValueControl,
		FChangerControl,
		FUnitControl:			TControl;
		FOnChangeValue:			TOnChangeValueEvent;

		FUserOnExitEvent:		TNotifyEvent;
		FUserOnKeyPressEvent: 	TKeyPressEvent;

		FOffset:				Double;
		FRoundMode:				TRoundTo;
		FPrecision:				Integer;
		FShowMinMaxHint:		Boolean;

		procedure EditOnExit (Sender: TObject);
		procedure EditOnKeyPress (Sender: TObject; var Key: Char);

		function DoCheckValue: Boolean;
		function CheckControlText: Double;

		procedure SetInConvertionPoint(Val: TPoint);
		procedure SetOutConvertionPoint(Val: TPoint);

		procedure SetAutoArrange(Val: Boolean);
		procedure SetValue(Val: Double);
		procedure SetValidRange(Val: TInterval);
		procedure SetOffset (Val: Double);
		procedure SetVisible(Val: Boolean);
		procedure SetReadOnly(Val: Boolean);
		procedure SetPerformCheck(Val: Boolean);
		procedure SetRoundMode (Val: TRoundTo);
		procedure SetPrecision (Val: Integer);

		procedure SetValueControl(Val: TControl);
		procedure SetUnitControl (Val: TControl);
	public
		constructor Create (ConvertingParameter: TParameterUnit);
		destructor Destroy; override;
		procedure ChangeValue (Val: Double);
		procedure Reset;
		function ReadFromControl: Double;
		function ChekRange: Boolean;
		function InRange(Val: Double): Boolean;
		function BeginUpdate: Boolean;
		function EndUpdate: Boolean;

		property ParameterType: TParameterUnit read FParameterType;
		property AutoArrange: Boolean read FAutoArrange write SetAutoArrange;

		property Value: Double read FValue write SetValue;
		property Visible: Boolean read FVisible write SetVisible;
		property ReadOnly: Boolean read FReadOnly write SetReadOnly;

		property PerformCheck: Boolean read FPerformCheck write SetPerformCheck;
		property Offset: Double read FOffset write SetOffset;

		property InConvertionPoint: TPoint read FInConvertionPoint write SetInConvertionPoint;
		property OutConvertionPoint: TPoint read FOutConvertionPoint write SetOutConvertionPoint;

		property Range: TInterval read FValidRange write SetValidRange;
		property Updating: Boolean read FUpdating;

		property DescriptionControl: TControl read FDescriptionControl write FDescriptionControl;
		property ValueControl: TControl read FValueControl write SetValueControl;
		property UnitControl: TControl read FUnitControl write SetUnitControl;
		property ChangerControl: TControl read FChangerControl write FChangerControl;
		property RoundMode: TRoundTo read FRoundMode write SetRoundMode;
		property Precision: Integer read FPrecision write SetPrecision;
		property ShowMinMaxHint: Boolean read FShowMinMaxHint write FShowMinMaxHint;

		property OnChangeValue: TOnChangeValueEvent read FOnChangeValue write FOnChangeValue;
	end;

procedure SetControlColor (Control: TControl);						overload;
procedure SetControlColor (Control: TControl; isEnabled: Boolean);	overload;

implementation

uses
	SysUtils, ComCtrls, Graphics, Math;

type
	TMyControl = class(TControl)
	public
		property Caption;
		property Color;
	end;

const
	Colors: array [Boolean] of TColor = (clBtnFace, clWindow);

procedure SetControlColor (Control: TControl);
begin
	TMyControl (Control).Color := Colors[Control.Enabled]
end;

procedure SetControlColor (Control: TControl; isEnabled: Boolean);
begin
	TMyControl (Control).Color := Colors[isEnabled]
end;

function TParameter.CheckControlText: Double;
var
	fNewVal:		Double;
	Code:			Integer;
	Text:			String;
	checkInRange:	Boolean;
begin
	result := FValue;

	Text := TMyControl(FValueControl).Caption;
	Val(Text, fNewVal, Code);
	if Code > 0 then	exit;

	fNewVal := Converters[FParameterType].ConvertFunction (StrToFloat(Text) + FOffset,
				cdToInner, FInConvertionPoint);

	checkInRange := (FPerformCheck and not FReadOnly);

	if checkInRange then
		result := CheckValue(fNewVal, FValidRange, FParameterType = puAngle)
	else
		result := fNewVal;

	if result <> fNewVal then
	begin
		fNewVal := Converters[FParameterType].ConvertFunction(result, cdToOuter, FOutConvertionPoint);
		fNewVal := fNewVal - FOffset;
		if FParameterType = puAngle then
			fNewVal := Modulus (fNewVal, 360);
		fNewVal := AdvancedRound (fNewVal, FPrecision, FRoundMode);
		TMyControl(FValueControl).Caption := FormatFloat (Converters [FParameterType].ConvertString, fNewVal);
	end;
end;

procedure TParameter.Reset;
begin
//	FUpdating := False;
//	FVisible := True;
//	FReadOnly := False;
//	FPerformCheck := True;

	FValue := NAN;
	FillChar(FValidRange, SizeOf(TInterval), 0);
end;

constructor TParameter.Create(ConvertingParameter: TParameterUnit);
begin
	Inherited Create;
	FParameterType := ConvertingParameter;
	FUpdating := False;
	FVisible := True;
	FReadOnly := False;
	FPerformCheck := False;

	FValue := NAN;
	FillChar(FValidRange, SizeOf(TInterval), 0);

	FInConvertionPoint := TPoint.Create;
	FOutConvertionPoint := TPoint.Create;

	FDescriptionControl := nil;
	FValueControl := nil;
	FUnitControl := nil;
	FChangerControl := nil;

	FPrecision := GPrecisions [ConvertingParameter];
	FRoundMode := rtNear;
	FShowMinMaxHint := False;
end;

destructor TParameter.Destroy;
begin
	FInConvertionPoint.Free;
	FOutConvertionPoint.Free;
	Inherited;
end;

function TParameter.ReadFromControl: Double;
var
	OldValue:	Double;
begin
	OldValue := FValue;

	FValue := CheckControlText ();

	if Assigned(FChangerControl) and FChangerControl.Visible then
	begin
		if FChangerControl is TUpDown then
			TUpDown(FChangerControl).Position := Round(StrToFloat(TMyControl(FValueControl).Caption))
//		else
	end;

	if Assigned(FOnChangeValue)and((OldValue <> FValue)or(IsNAN(OldValue)and(not IsNAN(FValue)))) then
	begin
		FOldValue := FValue;
		FOnChangeValue (Self);
	end;
	result := FValue;
end;

function TParameter.DoCheckValue: Boolean;
var
	fTmp:		Double;
	TmpBoolean:	Boolean;
	minS, maxS:	String;
begin
	result := False;

	if (not FReadOnly) and FPerformCheck then
	begin
		FValue := CheckValue(FValue, FValidRange, FParameterType = puAngle);

		if FValidRange.Left = FValidRange.Right then
		begin
			TmpBoolean := FReadOnly;
			SetReadOnly(True);
			FReadOnly := TmpBoolean;
		end
		else
			SetReadOnly(FReadOnly);
	end;

	if (not FReadOnly) and Assigned(FOnChangeValue)and((FOldValue <> FValue)or(IsNAN(FOldValue)and(not IsNAN(FValue)))) then
	begin
		FOldValue := FValue;
		FOnChangeValue(Self);
	end;


	if Assigned(FValueControl) then
	begin
		fTmp := Converters[FParameterType].ConvertFunction(FValue, cdToOuter, FOutConvertionPoint);
		fTmp := fTmp - FOffset;
		if FParameterType = puAngle then
			fTmp := Modulus (fTmp, 360);
		fTmp := AdvancedRound (fTmp, FPrecision, FRoundMode);
		TMyControl(FValueControl).Caption := FormatFloat(Converters[FParameterType].ConvertString, fTmp);

		if Assigned(FChangerControl) and FChangerControl.Visible then
		begin
			if FChangerControl.ClassType = TUpDown then
				TUpDown(FChangerControl).Position := Round(StrToFloat(TMyControl(FValueControl).Caption))
		end;
	end;

	if (FShowMinMaxHint) and (FValueControl is TEdit) then
	begin
		fTmp := Converters[FParameterType].ConvertFunction(FValidRange.Left, cdToOuter, FOutConvertionPoint);
		fTmp := fTmp - FOffset;
		if FParameterType = puAngle then
			fTmp := Modulus (fTmp, 360);
		fTmp := AdvancedRound (fTmp, FPrecision, FRoundMode);
		minS := 'Min: ' + FormatFloat(Converters[FParameterType].ConvertString, fTmp);
		if Assigned (FUnitControl) then
			minS := minS + ' ' + TMyControl(FUnitControl).Caption;

		fTmp := Converters[FParameterType].ConvertFunction(FValidRange.Right, cdToOuter, FOutConvertionPoint);
		fTmp := fTmp - FOffset;
		if FParameterType = puAngle then
			fTmp := Modulus (fTmp, 360);
		fTmp := AdvancedRound (fTmp, FPrecision, FRoundMode);
		maxS := 'Max: ' + FormatFloat(Converters[FParameterType].ConvertString, fTmp);
		if Assigned (FUnitControl) then
			maxS := maxS + ' ' + TMyControl(FUnitControl).Caption;

		TEdit (FValueControl).Hint := minS + #13 + maxS;
	end;
end;

procedure TParameter.SetValue(Val: Double);
begin
	if FUpdating then
		FValue := Val;
end;

function TParameter.InRange(Val: Double): Boolean;
begin
	result := Val = CheckValue(Val, FValidRange, FParameterType = puAngle);
end;

function TParameter.BeginUpdate: Boolean;
begin
	if not FUpdating then
		FOldValue := FValue;
	result := FUpdating;
	FUpdating := True;
end;

function TParameter.EndUpdate: Boolean;
begin
	result := FUpdating;
	if FUpdating then
		DoCheckValue;
	FUpdating := False;
end;

function TParameter.ChekRange: Boolean;
var
	Min, Max:		Double;
	MinRounded,
	MaxRounded:		Integer;
begin
	Min := Converters[FParameterType].ConvertFunction(FValidRange.Right, cdToOuter, FOutConvertionPoint);
	Max := Converters[FParameterType].ConvertFunction(FValidRange.Left, cdToOuter, FOutConvertionPoint);

	if (FParameterType = puAngle) and (Max < Min) then Max := Max + 360.0;

	MinRounded := Round(Min + 0.4999);
	MaxRounded := Round(Max - 0.4999);

	result := MinRounded < MaxRounded;
end;

procedure TParameter.SetValidRange(Val: TInterval);
begin
	if FUpdating then
	begin
		FPerformCheck := True;
		FValidRange := Val;
	end;
end;

procedure TParameter.SetInConvertionPoint(Val: TPoint);
begin
	if FUpdating then
		FInConvertionPoint.Assign(Val);
end;

procedure TParameter.SetOutConvertionPoint(Val: TPoint);
begin
	if FUpdating then
		FOutConvertionPoint.Assign(Val);
end;

procedure TParameter.EditOnExit (Sender: TObject);
begin
	ReadFromControl;

	if Assigned (FUserOnExitEvent) then
		FUserOnExitEvent (Sender);
end;

procedure TParameter.EditOnKeyPress (Sender: TObject; var Key: Char);
begin
	if Key = #13 then
	begin
		ReadFromControl;
		exit;
	end;

	if Key in [#8, #9, '-', DecimalSeparator] then
		exit;

	if not (Key in ['0'..'9']) then	key := #0;
	if (Key < '0') or (Key > '9') then
		Key := #0;

	if (Key <> #0) and Assigned (FUserOnKeyPressEvent) then
		FUserOnKeyPressEvent (Sender, Key);

//	if (not (key in ['0'..'9']))or(key <> '-')or( Length(TEdit(Sender).Text)) then	key := #0;

end;

procedure TParameter.SetValueControl(Val: TControl);
var
	edit:	TEdit;
begin
	FValueControl := Val;

	if FValueControl is TEdit then
	begin
		edit := TEdit(FValueControl);

		FUserOnExitEvent := edit.OnExit;
		FuserOnKeyPressEvent := edit.OnKeyPress;

		edit.OnExit := EditOnExit;
		edit.OnKeyPress := EditOnKeyPress;
	end;
end;

procedure TParameter.SetUnitControl(Val: TControl);
begin
	FUnitControl := Val;
	TMyControl(FUnitControl).Caption := GParameterUnitNames [FParameterType];
end;

procedure TParameter.SetAutoArrange(Val: Boolean);
begin
	if Val then
	begin

	end;
	FAutoArrange := Val;
end;

procedure TParameter.SetVisible(Val: Boolean);
begin
	FVisible := Val;
	if Assigned(FDescriptionControl) then FDescriptionControl.Visible := FVisible;
	if Assigned(FValueControl) then FValueControl.Visible := FVisible;
	if Assigned(FUnitControl) then FUnitControl.Visible := FVisible;
	if Assigned(FChangerControl) then FChangerControl.Visible := FVisible;
end;

procedure TParameter.SetReadOnly(Val: Boolean);
begin
	FReadOnly := Val;

	if Assigned(FDescriptionControl)and (FDescriptionControl is TEdit) then
		TEdit(FDescriptionControl).ReadOnly := FReadOnly;
	if Assigned(FValueControl) and (FValueControl is TEdit) then
		TEdit(FValueControl).ReadOnly := FReadOnly;
	if Assigned(FUnitControl) and (FUnitControl is TEdit) then
		TEdit(FUnitControl).ReadOnly := FReadOnly;

	if Assigned(FValueControl) then SetControlColor (FValueControl, not Val);
	if Assigned(FChangerControl) then
		if FReadOnly then	FChangerControl.Visible := False
		else				FChangerControl.Visible := FVisible;
end;

procedure TParameter.SetPerformCheck(Val: Boolean);
begin
	FPerformCheck := Val;
	if FPerformCheck then	DoCheckValue;
end;

procedure TParameter.SetOffset (Val: Double);
begin
	if FUpdating then
	begin
		FOffset := Val;
	end;
end;

procedure TParameter.ChangeValue(Val: Double);
begin
	BeginUpdate ();
	Value := Val;
	EndUpdate ();
end;

procedure TParameter.SetRoundMode(Val: TRoundTo);
begin
	if FUpdating then
		FRoundMode := Val;
end;

procedure TParameter.SetPrecision(Val: Integer);
begin
	if FUpdating then
		FPrecision := Val;
end;

end.
