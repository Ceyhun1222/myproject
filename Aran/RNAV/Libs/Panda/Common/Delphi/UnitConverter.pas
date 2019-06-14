unit UnitConverter;

interface

uses
	Parameter, SettingsContract;

{    
type
	THorisontalDistanceUnit = (hduMeter, hduKM, hduNM);
	TVerticalDistanceUnit = (vduMeter, vduFeet, vduFL, vduSM);
	THorisontalSpeedUnit = (hsuMeterInSec, hsuKMInHour, hsuKnot);
	TVerticalSpeedUnit = (vsuMeterInMin, vsuFeetInMin);

}

var
	GHorisontalDistIn,
	GHorisontalDistOut:		THorisontalDistanceUnit;

	GVerticalDistIn,
	GVerticalDistOut:		TVerticalDistanceUnit;

	GHorisontalSpeedIn,
	GHorisontalSpeedOut:	THorisontalSpeedUnit;

	GVerticalSpeedIn,
	GVerticalSpeedOut:		TVerticalSpeedUnit;

	GDistanceUnitName,
	GAltitudeUnitName,
	GGradientUnitName,
	GAngleUnitName,
	GHSpeedUnitName,
	GVSpeedUnitName:		string;

	GDistanceAccuracy,
	GAltitudeAccuracy,
	GGradientAccuracy,
	GDirAngleAccuracy,
	GAngleAccuracy,
	GHSpeedAccuracy,
	GVSpeedAccuracy:		Double;


function ConvertHorDistance (value: Double; fromUnit, toUnit: THorisontalDistanceUnit): Double;
function ConvertVerDistance (value: Double; fromUnit, toUnit: TVerticalDistanceUnit): Double;
function ConvertHorSpeed (value: Double; fromUnit, toUnit: THorisontalSpeedUnit): Double;
function ConvertVerSpeed (value: Double; fromUnit, toUnit: TVerticalSpeedUnit): Double;

function ConvertNULL(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertDistance(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertAltitude(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertGradient(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertAngle(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertHSpeed(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
function ConvertVSpeed(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;

implementation

uses
	Math, ARANMath, ARANGlobals, ARANFunctions, Geometry, GeometryOperatorsContract;

function ConvertHorSpeed (value: Double; fromUnit, toUnit: THorisontalSpeedUnit): Double;
const
	ConvertTable: Array [THorisontalSpeedUnit] of Double = (1000.0 / 3600.0, 1852.0 / 3600.0);
var
	kFrom, kTo:	Double;
begin
	kFrom := ConvertTable [fromUnit];
	kTo := 1.0 / ConvertTable [toUnit];
	result := value * kFrom * kTo;
end;

function ConvertVerSpeed (value: Double; fromUnit, toUnit: TVerticalSpeedUnit): Double;
const
	ConvertTable: Array [TVerticalSpeedUnit] of Double = (1.0, 0.3048);
var
	kFrom, kTo:	Double;
begin
	kFrom := ConvertTable [fromUnit];
	kTo := 1.0 / ConvertTable [toUnit];

	result := value * kFrom * kTo;
end;

function ConvertHorDistance (value: Double; fromUnit, toUnit: THorisontalDistanceUnit): Double;
const
	ConvertTable: Array [THorisontalDistanceUnit] of Double = (1000.0, 1852.0);
var
	kFrom, kTo:	Double;
begin
	kFrom := ConvertTable [fromUnit];
	kTo := 1.0 / ConvertTable [toUnit];

	result := value * kFrom * kTo;
end;

function ConvertVerDistance (value: Double; fromUnit, toUnit: TVerticalDistanceUnit): Double;
const
	ConvertTable: Array [TVerticalDistanceUnit] of Double = (1.0, 0.3048);
var
	kFrom, kTo:	Double;
begin
	kFrom := ConvertTable [fromUnit];
	kTo := 1.0 / ConvertTable [toUnit];

	result := value * kFrom * kTo;
end;

function ConvertNULL(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	Result := Val;
end;

function ConvertDistance(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	if Direction = cdToInner then
		Result := ConvertHorDistance (Val, GHorisontalDistOut, GHorisontalDistIn)
	else
		Result := ConvertHorDistance (Val, GHorisontalDistIn, GHorisontalDistOut)
end;

function ConvertAltitude(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	if Direction = cdToInner then
		Result := ConvertVerDistance (Val, GVerticalDistOut, GVerticalDistIn)
	else
		Result := ConvertVerDistance (Val, GVerticalDistIn, GVerticalDistOut)
end;

function ConvertGradient(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	if Direction = cdToInner then	Result := 0.01 * Val
	else							Result := 100.0 * Val
end;

function ConvertDirAngle(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
var
	Point:	TPoint;
begin
	Point := TPoint(ByPt);
	if (Point=nil)or(Point.IsEmpty) then
	begin
		if Direction = cdToInner then	Result := Modulus(DegToRad (Val), C_2xPI)
		else							Result := Modulus(RadToDeg (Val), 360.0);
	end
	else if Direction = cdToInner then
		Result := Modulus(AztToDirection(Point, Val, GGeoSR, GPrjSR), C_2xPI)
	else
		Result := Modulus(DirToAzimuth (Point, Val, GPrjSR, GGeoSR), 360.0);
end;

function ConvertAngle(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
var
	Point:	TPoint;
begin
	Point := TPoint(ByPt);
	if (Point=nil)or(Point.IsEmpty) then
	begin
		if Direction = cdToInner then	Result := Modulus(DegToRad (Val), C_2xPI)
		else							Result := Modulus(RadToDeg (Val), 360.0);
	end
	else if Direction = cdToInner then
		Result := Modulus(AztToDirection(Point, Val, GGeoSR, GPrjSR), C_2xPI)
	else
		Result := Modulus(DirToAzimuth (Point, Val, GPrjSR, GGeoSR), 360.0);
end;

function ConvertHSpeed(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	if Direction = cdToInner then
		Result := ConvertHorSpeed (Val, GHorisontalSpeedOut, GHorisontalSpeedIn)
	else
		Result := ConvertHorSpeed (Val, GHorisontalSpeedIn, GHorisontalSpeedOut)
end;

function ConvertVSpeed(Val: Double; Direction: TConvrtDirection; ByPt: TObject = nil): Double;
begin
	if Direction = cdToInner then
		Result := ConvertVerSpeed (Val, GVerticalSpeedOut, GVerticalSpeedIn)
	else
		Result := ConvertVerSpeed (Val, GVerticalSpeedIn, GVerticalSpeedOut)
end;

initialization
//======== Temperory initializations (Must be loaded from project)====================

	GDistanceUnitName		:= 'kM';
	GAltitudeUnitName		:= 'm';
	GGradientUnitName		:= '%';
	GAngleUnitName			:= '°';
	GHSpeedUnitName			:= 'km/h';
	GVSpeedUnitName			:= 'm/min';

	GDistanceAccuracy		:= 1;
	GAltitudeAccuracy		:= 1;
	GGradientAccuracy		:= 1;
	GDirAngleAccuracy		:= 1;
	GAngleAccuracy			:= 0.1;
	GHSpeedAccuracy			:= 1;
	GVSpeedAccuracy			:= 1;


	NULLConverter.ConvertFunction		:= ConvertNULL;
	DistanceConverter.ConvertFunction	:= ConvertDistance;
	AltitudeConverter.ConvertFunction	:= ConvertAltitude;
	GradientConverter.ConvertFunction	:= ConvertGradient;
	DirAngleConverter.ConvertFunction	:= ConvertDirAngle;
	AngleConverter.ConvertFunction		:= ConvertAngle;
	HSpeedConverter.ConvertFunction		:= ConvertHSpeed;
	VSpeedConverter.ConvertFunction		:= ConvertVSpeed;

	NULLConverter.Capability			:= [];
	DistanceConverter.Capability		:= [];
	AltitudeConverter.Capability		:= [];
	GradientConverter.Capability		:= [];
	DirAngleConverter.Capability		:= [ccvReversiveDependence];
	AngleConverter.Capability			:= [];
	HSpeedConverter.Capability			:= [];
	VSpeedConverter.Capability			:= [];

	NULLConverter.Accuracy				:= 0;
	DistanceConverter.Accuracy			:= GDistanceAccuracy;
	AltitudeConverter.Accuracy			:= GAltitudeAccuracy;
	GradientConverter.Accuracy			:= GGradientAccuracy;
	DirAngleConverter.Accuracy			:= GDirAngleAccuracy;
	AngleConverter.Accuracy				:= GAngleAccuracy;
	HSpeedConverter.Accuracy			:= GHSpeedAccuracy;
	VSpeedConverter.Accuracy			:= GVSpeedAccuracy;

	NULLConverter.UnitName				:= '';
	DistanceConverter.UnitName			:= GDistanceUnitName;
	AltitudeConverter.UnitName			:= GAltitudeUnitName;
	GradientConverter.UnitName			:= GGradientUnitName;
	DirAngleConverter.UnitName			:= GAngleUnitName;
	AngleConverter.UnitName				:= GAngleUnitName;
	HSpeedConverter.UnitName			:= GHSpeedUnitName;
	VSpeedConverter.UnitName			:= GVSpeedUnitName;

end.
