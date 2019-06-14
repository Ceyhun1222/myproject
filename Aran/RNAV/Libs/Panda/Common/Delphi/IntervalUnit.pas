unit IntervalUnit;

interface

type

	TInterval = record
		Left, Right:	Double;
//		Ciclic:			Boolean;
	end;

	TIntervalList = class
	private
		FCount:	Integer;
		FRange:	Array of TInterval;
		function GetValue(I: Integer): TInterval;
		procedure SetValue(I: Integer; Value: TInterval);
	public
		constructor Create;						overload;
		constructor Create(Range: TInterval);	overload;
		destructor Destroy;						override;
		procedure Assign(Val: TIntervalList);
		function Clone: TIntervalList;
		function Contains(Val: Double): Integer;

		function AddInterval(Val: TInterval): Boolean;	overload;
		procedure Union(Val: TIntervalList);		overload;

		procedure Intersect(Val: TIntervalList);	overload;
		procedure Intersect(Val: TInterval);		overload;


		procedure Subtract(Val: TIntervalList);	overload;
		procedure Subtract(Val: TInterval);		overload;

		property Count: Integer read FCount;
		property Interval[I: Integer]: TInterval read GetValue write SetValue;
	end;

function CheckValue(Val: Double; ValidRange: TInterval; Circular: Boolean): Double;

implementation

uses
	SysUtils, ARANMath;

function CheckValue(Val: Double; ValidRange: TInterval; Circular: Boolean): Double;
var
	Range, fTmp:		Double;
begin
	if not Circular then
	begin
		if Val < ValidRange.Left then Val := ValidRange.Left;
		if Val > ValidRange.Right then Val := ValidRange.Right;
	end
	else
	begin
		Range := Modulus(ValidRange.Right - ValidRange.Left, 2*PI);
		fTmp := Modulus(ValidRange.Right - Val, 2*PI);
		if fTmp > Range then
		begin
			if Modulus(fTmp - 0.5 * Range, 2*PI) < PI then
								Val := ValidRange.Left
			else				Val := ValidRange.Right;
		end;
	end;
	result := Val;
end;

function IntersectIntervals(A, B: TInterval; var C: TInterval): boolean;
begin
	if (A.Left > B.Right) Or (A.Right < B.Left) then
		Result := False
	else
	begin
		if A.Right < B.Right then
			C.Right := A.Right
		else
			C.Right := B.Right;

		if A.Left > B.Left then
			C.Left := A.Left
		else
			C.Left := B.Left;

		Result := True
	end;
end;

function SubtractInterval(A, B: TInterval; var C: TIntervalList): Boolean;
var
	TmpRange:	TInterval;
begin
	result := True;

	if (B.Left = B.Right) or (B.Right < A.Left) or (A.Right < B.Left) then
		C.AddInterval(A)
	else if (A.Left < B.Left) and (A.Right > B.Right) then
	begin
		TmpRange.Left := A.Left;
		TmpRange.Right := B.Left;
		C.AddInterval(TmpRange);

		TmpRange.Left := B.Right;
		TmpRange.Right := A.Right;
		C.AddInterval(TmpRange);
	end
	else if A.Right > B.Right then
	begin
		TmpRange.Left := B.Right;
		TmpRange.Right := A.Right;
		C.AddInterval(TmpRange);
	end
	else if (A.Left < B.Left) then
	begin
		TmpRange.Left := A.Left;
		TmpRange.Right := B.Left;
		C.AddInterval(TmpRange);
	end
	else
		result := False;
end;

constructor TIntervalList.Create;
begin
	FCount := 0;
	FRange := nil;
end;

constructor TIntervalList.Create(Range: TInterval);
var
	fTmp:	Double;
begin
	FCount := 1;
	SetLength(FRange, 1);
	if Range.Right < Range.Left then
	begin
		fTmp := Range.Left;
		Range.Left := Range.Right;
		Range.Right := fTmp;
	end;

	FRange[0] := Range;
end;

destructor TIntervalList.Destroy;
begin
	SetLength(FRange, 0);
	FRange := nil;
end;

procedure TIntervalList.Assign(Val: TIntervalList);
var
	I:	Integer;
begin
	FCount := Val.FCount;
	SetLength(FRange, FCount);
	for I := 0 to FCount - 1 do
		FRange[I] := Val.FRange[I];
end;

function TIntervalList.Clone: TIntervalList;
begin
	result := TIntervalList.Create;
	result.Assign(Self);
end;

function TIntervalList.GetValue(I: Integer): TInterval;
begin
	if(I >= 0) and(I < FCount) then
		result := FRange[I]
	else
		raise ERangeError.Create('Interval inindex out of bound.');
end;

procedure TIntervalList.SetValue(I: Integer; Value: TInterval);
begin
	if(I >= 0) and(I < FCount) then
		FRange[I] := Value
	else
		raise ERangeError.Create('Interval inindex out of bound.');
end;

function TIntervalList.AddInterval(Val: TInterval): Boolean;
var
	I, R, L: Integer;
begin
	result := False;
	if Val.Right < Val.Left then exit;

	if FCount = 0 then
	begin
		FCount := 1;
		SetLength(FRange, FCount);
		FRange[0] := Val;
	end
	else
	begin
		R := 0;
		while (R < FCount)and(FRange[R].Right < Val.Left) do Inc(R);

		if R = FCount then
		begin
			if FRange[R-1].Right >= Val.Left then
			begin
				if FRange[R-1].Left > Val.Left then
					FRange[R-1].Left := Val.Left;

				if FRange[R-1].Right < Val.Right then
					FRange[R-1].Right := Val.Right;
			end
			else
			begin
				Inc(FCount);
				SetLength(FRange, FCount);
				FRange[FCount-1] := Val;
			end
		end
		else if FRange[R].Left > Val.Right then
		begin
			Inc(FCount);
			SetLength(FRange, FCount);
			for I := FCount - 1 downto R do
				FRange[I] := FRange[I - 1];
			FRange[R] := Val;
		end
		else
		begin
			if FRange[R].Left > Val.Left then
				FRange[R].Left := Val.Left;

			if FRange[R].Right < Val.Right then
				FRange[R].Right := Val.Right;

			L := R + 1;
			while(L < FCount)and(FRange[L].Left <= Val.Right) do Inc(L);

			if FRange[L-1].Right > Val.Right then
				FRange[R].Right := FRange[L-1].Right;

			if L = FCount then
			begin
				if FRange[R].Right < Val.Right then
					FRange[R].Right := Val.Right;

				FCount := R+1;
				SetLength(FRange, FCount);
			end
			else
			begin
				for I := R + 1 to FCount - 2 do
					FRange[I] := FRange[I+1];

				Dec(FCount, L-R);
				SetLength(FRange, FCount);
			end;
		end;
	end;
end;

procedure TIntervalList.Union(Val: TIntervalList);
var
	I:		Integer;
begin
	for I := 0 to Val.FCount -1 do
		AddInterval(Val.FRange[I]);
end;

procedure TIntervalList.Intersect(Val: TIntervalList);
var
	C:			TInterval;
	I, J:		Integer;
	TmpInter:	TIntervalList;
begin
	TmpInter := TIntervalList.Create;
	for J := 0 to Val.FCount -1 do
	begin
		for I := 0 to FCount -1 do
		begin
			if IntersectIntervals(Val.FRange[J], FRange[I], C) then
				TmpInter.AddInterval(C);
		end;
	end;
	Assign(TmpInter);
	TmpInter.Free;
end;

procedure TIntervalList.Intersect(Val: TInterval);
var
	I:			Integer;
	C:			TInterval;
	TmpInter:	TIntervalList;
begin
	TmpInter := TIntervalList.Create;

	for I := 0 to FCount -1 do
	begin
		if IntersectIntervals(Val, FRange[I], C) then
			TmpInter.AddInterval(C);
	end;
	Assign(TmpInter);
	TmpInter.Free;
end;

procedure TIntervalList.Subtract(Val: TInterval);
var
	I:			Integer;
	TmpInter:	TIntervalList;
begin
	TmpInter := TIntervalList.Create;

	for I := 0 to FCount -1 do
		SubtractInterval(Val, FRange[I], TmpInter);

	Assign(TmpInter);
	TmpInter.Free;
end;

procedure TIntervalList.Subtract(Val: TIntervalList);
var
	I, J:		Integer;
	TmpInter:	TIntervalList;
begin
	TmpInter := TIntervalList.Create;
	for J := 0 to Val.FCount -1 do
		for I := 0 to FCount -1 do
			SubtractInterval(Val.FRange[J], FRange[I], TmpInter);

	Assign(TmpInter);
	TmpInter.Free;
end;

function TIntervalList.Contains(Val: Double): Integer;
var
	I:	Integer;
begin
	for I := 0 to FCount -1 do
		if (FRange[I].Left <= Val) and(FRange[I].Right >= Val) then
		begin
			result := I;
			exit;
		end;
	result := -1;
end;

end.

