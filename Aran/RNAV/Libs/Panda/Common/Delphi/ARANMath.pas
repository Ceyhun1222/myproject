unit ARANMath;

interface
const
	C_E				= 2.71828182845904523536;
	C_LOG2E			= 1.44269504088896340736;
	C_LOG10E		= 0.434294481903251827651;
	C_LN2			= 0.693147180559945309417;
	C_LN10			= 2.30258509299404568402;

	C_PI			= 3.14159265358979323846;
	C_2xPI			= 2*3.14159265358979323846;
	C_PI_2			= 1.57079632679489661923;
	C_PI_4			= 0.785398163397448309616;
	C_1_PI			= 0.318309886183790671538;
	C_2_PI			= 0.636619772367581343076;
	C_1_SQRTPI		= 0.564189583547756286948;
	C_2_SQRTPI		= 1.12837916709551257390;
	C_SQRT2			= 1.41421356237309504880;
	C_SQRT_2		= 0.707106781186547524401;

	C_SQRT3			= 1.7320508075688772;
	C_SQRT_3		= 0.57735026918962584;

	Epsilon = 10e-8;
	EpsilonDistance = 0.001;
	Epsilon_2Distance = EpsilonDistance * EpsilonDistance;
	EpsilonDegree = 1.0 / 3600.0;
	EpsilonRadian = EpsilonDegree / C_2xPI;

type
	TRoundTo = (rtNone, rtFloor, rtNear, rtCeil);
	TSideDirection = (sideLeft = -1, sideOn = 0, sideRight = 1);

	TVariants = 0..2;

	TIntDouble = record
		case TVariants of
		0: (AsDouble:	Double);
		1: (AsInteger:	Packed array [0..1] of integer);
		2: (AsInt64: 	Int64);
	end;

	TIntSingle = record
		case Boolean of
		True:	(AsSingle:	Single);
		False:	(AsInteger:	Integer);
	end;

function Modulus (X, Y: Double): Double;
function AdvancedRound (Value, Accuracy: Double; RoundMode: TRoundTo): Double;
function RoundToStr(Value, Accuracy: Double; roundMode: TRoundTo = rtNear): String;

function InversDirection(Direction: TSideDirection): TSideDirection;
function MultiplyDirections(Direction1, Direction2: TSideDirection): TSideDirection;
function IASToTAS (IAS, H, dT: Double): Double;
function BankToRadius(radianBank, VInMetrsInSec: Double): Double;
function RadiusToBank(radius: Double; VInMetrsInSec: Double): Double;

function SideFrom2Angle (DirInRadian0, DirInRadian1: Double): Integer;
function SideFrom2Angle_new (DirInRadian0, DirInRadian1: Double): TSideDirection;
function SubtractAngles (X, Y: Double): Double;
function SubtractAnglesWithSign (startAngle, endAngle: Double; side: TSideDirection): Double;

implementation

uses
	Math, SysUtils;

function Modulus (X, Y: Double): Double;
begin
	x := x - floor (x / y) * y;
	if x < 0.0 then	x := x + y;
	if x = y then	x := 0.0;
	Result := x;
end;

function AdvancedRound (Value, Accuracy: Double; RoundMode: TRoundTo): Double;
var
	nValue:	Double;
begin
	result := value;

	if RoundMode in [rtFloor, rtNear, rtCeil] then
	begin
		nValue := value / Accuracy;

		if Abs(nValue-Round(nValue)) < Epsilon then
			roundMode := rtNear;

		case roundMode of
			rtFloor:	result := Floor (nValue) * Accuracy;
			rtNear:		result := Round (nValue) * Accuracy;
			rtCeil:		result := Ceil (nValue) * Accuracy;
		end
	end
end;

function RoundToStr (Value, Accuracy: Double; roundMode: TRoundTo = rtNear): String;
var
    s:  String;
    n:  Double;
begin
    s := '0';
    n := Log10 (Accuracy);
    if n < 0 then
        s := '0.' + StringOfChar ('#', Ceil (-n));

	result := FormatFloat (s, AdvancedRound (value, Accuracy, roundMode));
end;

function InversDirection(Direction: TSideDirection): TSideDirection;
begin
	Result := TSideDirection(-Integer(Direction));
end;

function MultiplyDirections(Direction1, Direction2: TSideDirection): TSideDirection;
begin
	Result := TSideDirection(Integer(Direction1)*Integer(Direction2));
end;

function IASToTAS(IAS, H, dT: Double): Double;
begin
	result := IAS * 171233.0 * Sqrt(288.0 + dT - 0.006496 * H) / Power (288.0 - 0.006496 * H, 2.628);
end;

function BankToRadius(radianBank, VInMetrsInSec: Double): Double;
var
	Rv:		Double;
begin
	Rv := 1.76527777777777777777 * Tan(radianBank) / (Pi * VInMetrsInSec);
	if Rv > 0.003 then	Rv := 0.003;

	if Rv > 0.0 then	result := VInMetrsInSec / (5.555555555555555555555 * PI * Rv)
	else				result := -1;
end;

function RadiusToBank(radius: Double; VInMetrsInSec: Double): Double;
begin
	if radius > 0.0 then
		result := ArcTan (Sqr(VInMetrsInSec) / (9.807098765432098 * radius ))
	else
		result := -1;
end;

function SideFrom2Angle (DirInRadian0, DirInRadian1: Double): Integer;
var
	rAngle:	Double;
begin
	rAngle := SubtractAngles (DirInRadian0, DirInRadian1);

	if ((C_2xPI - rAngle) < EpsilonRadian) or (rAngle < EpsilonRadian) then
		result := 0
	else
	begin
		rAngle := Modulus(DirInRadian1 - DirInRadian0, C_2xPI);
		if rAngle < C_PI then	result := 1
		else					result := -1
	end
end;

function SideFrom2Angle_new (DirInRadian0, DirInRadian1: Double): TSideDirection;
var
	rAngle:	Double;
begin
	rAngle := SubtractAngles (DirInRadian0, DirInRadian1);

	if ((C_2xPI - rAngle) < EpsilonRadian) or (rAngle < EpsilonRadian) then
		result := sideOn
	else
	begin
		rAngle := Modulus (DirInRadian1 - DirInRadian0, C_2xPI);
		if rAngle < C_PI then
			result := sideRight
		else
			result := sideLeft;
	end
end;

function SubtractAngles (X, Y: Double): Double;
begin
	X := Modulus(X, C_2xPI);
	Y := Modulus(Y, C_2xPI);
	result := Modulus(X - Y, C_2xPI);
	if result > Pi then result := C_2xPI - result;
end;

function SubtractAnglesWithSign (startAngle, endAngle: Double; side: TSideDirection): Double;
begin
    result := Modulus ((endAngle - startAngle) * Integer (side), C_2xPI);
    if result > Pi then
        result := result  - C_2xPI;
end;

end.
