unit HelperClasses;

interface
uses
	Classes, ConstantsContract, AIXMTypes, CollectionUnit, Geometry, IntervalUnit;

type

	TPageHolder = Class
	end;

	TPage1 = Class(TPageHolder)
		iChecked:					Integer;

		ProcedureType:				Integer;
		ApproachType:				Integer;
		FAirCraftCategory:			TAircraftCategory;

		Aerodrome:					Integer;
		RWY:						Integer;
		RWYDirection:				Integer;
//	TrueBrg:					Double;
		fRWYCLDir:					Double;

		FRWYDirection:				TRWYDirection;
	end;

	TPage2 = Class(TPageHolder)
		FAlignedSgfPoint:			TPandaCollection;
		FNotAlignedSgfPoint:		TPandaCollection;
		FNotAlignedSgfPointRange:	TList;

		FFarMAPt,
		FAnchorPoint1,
		FAnchorPoint2:				TPoint;

		FFarMAPtHandle,
		FTrackHandle,
		FCLHandle:					Integer;
		arMaxInterDist:				Double;

		FAligned:					Boolean;
		FExistingAnchorPoint:		Boolean;
		FAbeam_AlongDistance:		Double;
		FApproachDir:				Double;
		FTrackIntervals:			Array [0..1] of TInterval;
//		FTrackCourses:				TInterval;

		procedure SetAligned(Aligned: Boolean);
		procedure SetAnchorPointType(Existing: Boolean);
		procedure SetAnchorPoint(AnchorPoint: TPoint; Index: Integer);
		procedure SetAlignedAnchorPoint(AnchorPoint: TPoint);
		procedure SetNotAlignedAnchorPoint(AnchorPoint: TPoint; Index: Integer);
		procedure SetTrackInterval(TrackInterval: TInterval);
		function FillSecondPoints(TrackIntervalList: TIntervalList): Integer;

		procedure SetAbeam_AlongDistance(Distance: Double);

		procedure SetDirection(fTrackDir: Double);
//		procedure SetAlignedDirection(fTrackDir: Double);
//		procedure SetNotAlignedDirection(fTrackDir: Double);

		function CreateMAPt(fTrackDir: Double): Boolean;
		procedure FillFirstAnchorPoints;
	end;

implementation

procedure TPage2.SetAligned(Aligned: Boolean);
begin
end;

procedure TPage2.SetAnchorPointType(Existing: Boolean);
begin
end;

procedure TPage2.SetAnchorPoint(AnchorPoint: TPoint; Index: Integer);
begin
end;

procedure TPage2.SetAlignedAnchorPoint(AnchorPoint: TPoint);
begin
end;

procedure TPage2.SetNotAlignedAnchorPoint(AnchorPoint: TPoint; Index: Integer);
begin
end;

procedure TPage2.SetTrackInterval(TrackInterval: TInterval);
begin
end;

function TPage2.FillSecondPoints(TrackIntervalList: TIntervalList): Integer;
begin
	result := 0;
end;

procedure TPage2.SetAbeam_AlongDistance(Distance: Double);
begin
end;

procedure TPage2.SetDirection(fTrackDir: Double);
begin
end;

//procedure TPage2.SetAlignedDirection(fTrackDir: Double);
//procedure TPage2.SetNotAlignedDirection(fTrackDir: Double);

function TPage2.CreateMAPt(fTrackDir: Double): Boolean;
begin
	result := False;
end;

procedure TPage2.FillFirstAnchorPoints;
begin
end;

end.

