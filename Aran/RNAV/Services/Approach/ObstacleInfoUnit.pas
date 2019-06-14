unit ObstacleInfoUnit;

interface

uses
	AIXMTypes, FIXUnit, FlightPhaseUnit;
	
type

	TObstacleInfo = class
		Host:				TObstacle;
		FlightProcedure:	TFlightProcedure;
		Leg,
		Flags:				Integer;
		Elevation,
		Dist,
		fTmp,
		ReqH,
		ReqOCA,
		MOC, fSort:			Double;
		Name, ID, sSort:	String;
	end;

	TObstacleInfoList = Array of TObstacleInfo;
	
implementation

end.
