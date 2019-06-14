unit UISettings;

interface
	uses ARANFunctions, UnitConverter, ARANMath;

type
	TUISettings = class
	public
		InHorDistanceUnit,
		OutHorDistanceUnit:		THorisontalDistanceUnit;
		InVerDistanceUnit,
		OutVerDistanceUnit:		TVerticalDistanceUnit;

		InHorSpeedUnit,
		OutHorSpeedUnit:		THorisontalSpeedUnit;
		InVerSpeedUnit,
		OutVerSpeedUnit:		TVerticalSpeedUnit;

		HorDistancePrecision:	Integer;
		VerDistancePrecision:	Integer;
	public
		constructor Create;
		destructor Destroy; overload;
		 
		function HorDistanceOutToIn (value: Double): Double; overload;
		function HorDistanceInToOut (value: Double): Double;
		function VerDistanceOutToIn (value: Double): Double; overload;
		function VerDistanceInToOut (value: Double): Double;

		function HorSpeedOutToIn (value: Double): Double;
		function HorSpeedInToOut (value: Double): Double;
		function VerSpeedOutToIn (value: Double): Double;
		function VerSpeedInToOut (value: Double): Double;

		function HorDistanceInToOutStr (value: Double; roundMode: TRoundTo): String;
		function VerDistanceInToOutStr (value: Double; roundMode: TRoundTo): String;
		function HorDistanceOutToIn (strValue: String): Double; overload;
		function VerDistanceOutToIn (strValue: String): Double; overload;
		function PDGOutToIn (value: Double): Double; overload;
		function PDGOutToIn (strValue: String): Double; overload;
		function PDGInToOut (value: Double): String;
	end;

implementation

uses ConvUtils, SysUtils;

constructor TUISettings.Create;
begin
	inherited;

	InHorDistanceUnit := hduMeter;
	InVerDistanceUnit := vduMeter;
	OutHorDistanceUnit := hduMeter;
	OutVerDistanceUnit := vduMeter;
	InHorSpeedUnit := hsuMeterInSec;
	OutHorSpeedUnit := hsuKMInHour;
	InVerSpeedUnit := vsuMeterInMin;
	OutVerSpeedUnit := vsuMeterInMin;

	HorDistancePrecision := 1;
	VerDistancePrecision := 1;
end;

destructor TUISettings.Destroy;
begin
	inherited;
end;

function TUISettings.HorDistanceOutToIn (value: Double): Double;
begin
	result := ConvertHorDistance (value, OutHorDistanceUnit, InHorDistanceUnit);
end;

function TUISettings.HorDistanceInToOut (value: Double): Double;
begin
	result := ConvertHorDistance (value, InHorDistanceUnit, OutHorDistanceUnit);
end;

function TUISettings.VerDistanceOutToIn (value: Double): Double;
begin
	result := ConvertVerDistance (value, OutVerDistanceUnit, InVerDistanceUnit);
end;

function TUISettings.VerDistanceInToOut (value: Double): Double;
begin
	result := ConvertVerDistance (value, InVerDistanceUnit, OutVerDistanceUnit);
end;

function TUISettings.HorSpeedOutToIn (value: Double): Double;
begin
	result := ConvertHorSpeed (value, OutHorSpeedUnit, InHorSpeedUnit);
end;

function TUISettings.HorSpeedInToOut (value: Double): Double;
begin
	result := ConvertHorSpeed (value, InHorSpeedUnit, OutHorSpeedUnit);
end;

function TUISettings.VerSpeedOutToIn (value: Double): Double;
begin
	result := ConvertVerSpeed (value, OutVerSpeedUnit, InVerSpeedUnit);
end;

function TUISettings.VerSpeedInToOut (value: Double): Double;
begin
	result := ConvertVerSpeed (value, InVerSpeedUnit, OutVerSpeedUnit);
end;

function TUISettings.HorDistanceInToOutStr (value: Double; roundMode: TRoundTo): String;
begin
	result := RoundToStr (value, HorDistancePrecision, roundMode);
end;

function TUISettings.VerDistanceInToOutStr (value: Double; roundMode: TRoundTo): String;
begin
	result := RoundToStr (value, VerDistancePrecision, roundMode);
end;

function TUISettings.HorDistanceOutToIn (strValue: String): Double;
begin
	result := HorDistanceOutToIn (StrToFloat (strValue));
end;

function TUISettings.VerDistanceOutToIn (strValue: String): Double;
begin
	result := VerDistanceOutToIn (StrToFloat (strValue));
end;

function TUISettings.PDGOutToIn (strValue: String): Double;
begin
	result := PDGOutToIn (StrToFloat (strValue));
end;

function TUISettings.PDGOutToIn (value: Double): Double;
begin
	result := value / 100;
end;

function TUISettings.PDGInToOut (value: Double): String;
begin
	result := FloatToStr (value * 100);
end;

end.
