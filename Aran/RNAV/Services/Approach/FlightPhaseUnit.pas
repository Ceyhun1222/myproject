unit FlightPhaseUnit;

interface

uses
	AIXMTypes, Geometry, UIContract, ConstantsContract, IntervalUnit, FIXUnit;

type

	TFlightProcedure = (fpEnroute, fpDeparture, fpArrival, fpInitialApproach, fpIntermediateApproach,
					fpFinalApproach, fpInitialMissedApproach, fpIntermediateMissedApproach,
					fpFinalMissedApproach);

	TFlightPhase = record
		Name:				String;		
		FlightProcedure:	TFlightProcedure;
		MOC:				Double;
		IASRange,
		GradientRange,
		TurnRange,
		DistRange:			TInterval;
	end;

	TLeg = class(TObject)
	protected
		FStartFIX,
		FEndFIX:			TWayPoint;
		FUI:				TUIContract;

		FFlightProcedure:	TFlightProcedure;
		FIAS,
		FGradient:			Double;

		FPrimaryAreaMEl,
		FFullAreaMEl,
//		FNNSecondLineEl,
		FNomTrackEl:		Integer;

		FPrimaryArea,
		FFullArea,

		FPrimaryAreaS,
		FFullAreaS,

		FPrimaryAreaM,
		FFullAreaM:			TPolygon;

		FNNSecondLine,
		FNomTrack:			TPolyline;

		FBufferSymbol,
		FProtectSymbol:		TFillSymbol;

		FNNSecondSymbol,
		FNomTrackSymbol:	TLineSymbol;

		procedure SetFullAreaM(Val: TPolygon);
		procedure SetPrimaryAreaM(Val: TPolygon);

		procedure SetFullAreaS(Val: TPolygon);
		procedure SetPrimaryAreaS(Val: TPolygon);

		procedure SetFullArea(Val: TPolygon);
		procedure SetPrimaryArea(Val: TPolygon);

		procedure SetNNSecondLine(Val: TPolyline);
		procedure SetNomTrack(Val: TPolyline);
		procedure CreateNNSecondLine(const PrevLeg: TLeg);
	public
		constructor Create(FIX0, FIX1: TWayPoint; _FlightProcedure: TFlightProcedure; UI: TUIContract; PrevLeg: Tleg = nil);
		destructor Destroy; override;

		procedure DeleteGraphics;
		procedure RefreshGraphics;
		procedure CreateNomTrack(PrevLeg: TLeg);
		procedure CreateProtectionArea(PrevLeg: TLeg; ARP: TAHP);

		property IAS: Double read FIAS write FIAS;
		property Gradient: Double read FGradient write FGradient;

		property StartFIX: TWayPoint read FStartFIX;
		property EndFIX: TWayPoint read FEndFIX;

		property FullAssesmentArea: TPolygon read FFullAreaS write SetFullAreaS;
		property PrimaryAssesmentArea: TPolygon read FPrimaryAreaS write SetPrimaryAreaS;

		property FullTotalArea: TPolygon read FFullAreaM write SetFullAreaM;
		property PrimaryTotalArea: TPolygon read FPrimaryAreaM write SetPrimaryAreaM;

		property FullArea: TPolygon read FFullArea write SetFullArea;
		property PrimaryArea: TPolygon read FPrimaryArea write SetPrimaryArea;

		property NNSecondLine: TPolyline read FNNSecondLine write SetNNSecondLine;
		property NomTrack: TPolyline read FNomTrack write SetNomTrack;
		property FlightProcedure: TFlightProcedure read FFlightProcedure;
	end;

var
	FlightPhases:	Array [TFlightProcedure] of TFlightPhase;

const
	PBNInternalTriggerDistance = 28000;
	PBNTerminalTriggerDistance = 56000;
	SBASTriggerDistance = 46000;


procedure InitModule(PANSOPSConstants: TPANSOPSConstantList;
					AircraftCategoryConstants: TAircraftCategoryList;
					AircraftCategory: TAircraftCategory);


implementation
uses
	Math, ApproachGlobals, ARANFunctions, ARANMath, Functions,
	ARANGlobals, SysUtils;


function RGB(R, G, B: Byte): Cardinal;
begin
	Result := R + (G shl 8) + (B shl 16);
end;

procedure InitModule(PANSOPSConstants: TPANSOPSConstantList;
					AircraftCategoryConstants: TAircraftCategoryList;
					AircraftCategory: TAircraftCategory);
var
	I:	TFlightProcedure;
begin

	for I := Low(TFlightProcedure) to High(TFlightProcedure) do
		FlightPhases[I].FlightProcedure := I;

// InitialMissedApproach =======================================================
	FlightPhases[fpInitialMissedApproach].Name := 'Initial Missed Approach';
	FlightPhases[fpInitialMissedApproach].MOC := PANSOPSConstants.Constant[arMA_InterMOC].Value;

	FlightPhases[fpInitialMissedApproach].IASRange.Left := AircraftCategoryConstants.Constant[cVmaInter].Value[AirCraftCategory];
	FlightPhases[fpInitialMissedApproach].IASRange.Right := AircraftCategoryConstants.Constant[cVmaFaf].Value[AirCraftCategory];

	FlightPhases[fpInitialMissedApproach].GradientRange.Left := PANSOPSConstants.Constant[arMAS_Climb_Min].Value;
	FlightPhases[fpInitialMissedApproach].GradientRange.Right := PANSOPSConstants.Constant[arMAS_Climb_Max].Value;

//	FlightPhases[fpInitialMissedApproach].TurnRange.Left :=
//	FlightPhases[fpInitialMissedApproach].TurnRange.Right :=
//	FlightPhases[fpInitialMissedApproach].DistRange.Left :=
//	FlightPhases[fpInitialMissedApproach].DistRange.Right :=

// IntermediateMissedApproach ==================================================
	FlightPhases[fpIntermediateMissedApproach].Name := 'Intermediate Missed Approach';
	FlightPhases[fpIntermediateMissedApproach].MOC := PANSOPSConstants.Constant[arMA_InterMOC].Value;
	FlightPhases[fpIntermediateMissedApproach].IASRange.Left := AircraftCategoryConstants.Constant[cVmaInter].Value[AirCraftCategory];
	FlightPhases[fpIntermediateMissedApproach].IASRange.Right := AircraftCategoryConstants.Constant[cVmaFaf].Value[AirCraftCategory];

	FlightPhases[fpIntermediateMissedApproach].GradientRange.Left := PANSOPSConstants.Constant[arMAS_Climb_Min].Value;
	FlightPhases[fpIntermediateMissedApproach].GradientRange.Right := PANSOPSConstants.Constant[arMAS_Climb_Max].Value;

//	FlightPhases[fpIntermediateMissedApproach].TurnRange.Left :=
//	FlightPhases[fpIntermediateMissedApproach].TurnRange.Right :=
//	FlightPhases[fpIntermediateMissedApproach].DistRange.Left :=
//	FlightPhases[fpIntermediateMissedApproach].DistRange.Right :=

// FinalMissedApproach =========================================================
	FlightPhases[fpFinalMissedApproach].Name := 'Final Missed Approach';
	FlightPhases[fpFinalMissedApproach].MOC := PANSOPSConstants.Constant[arMA_FinalMOC].Value;
	FlightPhases[fpFinalMissedApproach].IASRange.Left := AircraftCategoryConstants.Constant[cVmaInter].Value[AirCraftCategory];
	FlightPhases[fpFinalMissedApproach].IASRange.Right := AircraftCategoryConstants.Constant[cVmaFaf].Value[AirCraftCategory];

	FlightPhases[fpFinalMissedApproach].GradientRange.Left := PANSOPSConstants.Constant[arMAS_Climb_Min].Value;
	FlightPhases[fpFinalMissedApproach].GradientRange.Right := PANSOPSConstants.Constant[arMAS_Climb_Max].Value;

//	FlightPhases[fpFinalMissedApproach].TurnRange.Left :=
//	FlightPhases[fpFinalMissedApproach].TurnRange.Right :=
//	FlightPhases[fpFinalMissedApproach].DistRange.Left :=
//	FlightPhases[fpFinalMissedApproach].DistRange.Right :=

// Departure ===================================================================
	FlightPhases[fpDeparture].Name := 'Departure';
//	FlightPhases[fpDeparture].MOC :=

	FlightPhases[fpDeparture].IASRange.Left := 1.1 * FlightPhases[fpFinalMissedApproach].IASRange.Left;
	FlightPhases[fpDeparture].IASRange.Right := 1.1 * FlightPhases[fpFinalMissedApproach].IASRange.Right;

	FlightPhases[fpDeparture].GradientRange.Left := PANSOPSConstants.Constant[dpOIS].Value;
	FlightPhases[fpDeparture].GradientRange.Right := PANSOPSConstants.Constant[dpMaxPosPDG].Value;

//	FlightPhases[fpDeparture].TurnRange.Left :=
//	FlightPhases[fpDeparture].TurnRange.Right :=
//	FlightPhases[fpDeparture].DistRange.Left :=
//	FlightPhases[fpDeparture].DistRange.Right :=

// Enroute =====================================================================
	FlightPhases[fpEnroute].Name := 'Enroute';
	FlightPhases[fpEnroute].MOC := PANSOPSConstants.Constant[enrMOC].Value;
	FlightPhases[fpEnroute].IASRange.Left := AircraftCategoryConstants.Constant[enIAS].Value[AirCraftCategory];
	FlightPhases[fpEnroute].IASRange.Right := AircraftCategoryConstants.Constant[enIAS].Value[AirCraftCategory];

//	FlightPhases[fpEnroute].GradientRange.Left :=
//	FlightPhases[fpEnroute].GradientRange.Right :=

	FlightPhases[fpEnroute].TurnRange.Left := 0.0;
	FlightPhases[fpEnroute].TurnRange.Right := PANSOPSConstants.Constant[enrMaxTurnAngle].Value;

//	FlightPhases[fpEnroute].DistRange.Left :=
//	FlightPhases[fpEnroute].DistRange.Right :=

// InitialApproach =============================================================
	FlightPhases[fpInitialApproach].Name := 'Initial Approach';
	FlightPhases[fpInitialApproach].MOC := PANSOPSConstants.Constant[arIASegmentMOC].Value;
	FlightPhases[fpInitialApproach].IASRange.Left := AircraftCategoryConstants.Constant[cViafMin].Value[AirCraftCategory];
	FlightPhases[fpInitialApproach].IASRange.Right := AircraftCategoryConstants.Constant[cViafMax].Value[AirCraftCategory];

	FlightPhases[fpInitialApproach].GradientRange.Left := PANSOPSConstants.Constant[arIADescent_Nom].Value;
	FlightPhases[fpInitialApproach].GradientRange.Right := PANSOPSConstants.Constant[arIADescent_Max].Value;

	FlightPhases[fpInitialApproach].TurnRange.Left := 0.0;
	FlightPhases[fpInitialApproach].TurnRange.Right := PANSOPSConstants.Constant[arIAFMaxTurnAngl].Value;

//	FlightPhases[fpInitialApproach].DistRange.Left :=
//	FlightPhases[fpInitialApproach].DistRange.Right :=

// Arrival =====================================================================
	FlightPhases[fpArrival].Name := 'Arrival';
	FlightPhases[fpArrival].MOC := PANSOPSConstants.Constant[arArrivalMOC].Value;
	FlightPhases[fpArrival].IASRange.Left := FlightPhases[fpEnroute].IASRange.Left;
	FlightPhases[fpArrival].IASRange.Right := FlightPhases[fpEnroute].IASRange.Right;

	FlightPhases[fpArrival].GradientRange.Left := FlightPhases[fpInitialApproach].GradientRange.Left;
	FlightPhases[fpArrival].GradientRange.Right := FlightPhases[fpInitialApproach].GradientRange.Right;

	FlightPhases[fpArrival].TurnRange.Left := FlightPhases[fpInitialApproach].TurnRange.Left;
	FlightPhases[fpArrival].TurnRange.Right := FlightPhases[fpInitialApproach].TurnRange.Right;

//	FlightPhases[fpArrival].DistRange.Left :=
//	FlightPhases[fpArrival].DistRange.Right :=

// IntermediateApproach ========================================================
	FlightPhases[fpIntermediateApproach].Name := 'Intermediate Approach';
	FlightPhases[fpIntermediateApproach].MOC := PANSOPSConstants.Constant[arISegmentMOC].Value;
	FlightPhases[fpIntermediateApproach].IASRange.Left := FlightPhases[fpInitialApproach].IASRange.Left;
	FlightPhases[fpIntermediateApproach].IASRange.Right := FlightPhases[fpInitialApproach].IASRange.Right;

	FlightPhases[fpIntermediateApproach].GradientRange.Left := 0.0;
	FlightPhases[fpIntermediateApproach].GradientRange.Right := PANSOPSConstants.Constant[arImDescent_Max].Value;

	FlightPhases[fpIntermediateApproach].TurnRange.Left := 0.0;
	FlightPhases[fpIntermediateApproach].TurnRange.Right := PANSOPSConstants.Constant[arImMaxIntercept].Value;

//	FlightPhases[fpIntermediateApproach].DistRange.Left :=
//	FlightPhases[fpIntermediateApproach].DistRange.Right :=

// FinalApproach ===============================================================
	FlightPhases[fpFinalApproach].Name := 'Final Approach';
	FlightPhases[fpFinalApproach].MOC := PANSOPSConstants.Constant[arFASeg_FAF_MOC].Value;
	FlightPhases[fpFinalApproach].IASRange.Left := AircraftCategoryConstants.Constant[cVfafMin].Value[AirCraftCategory];
	FlightPhases[fpFinalApproach].IASRange.Right := AircraftCategoryConstants.Constant[cVfafMax].Value[AirCraftCategory];

	FlightPhases[fpFinalApproach].GradientRange.Left := PANSOPSConstants.Constant[arFADescent_Min].Value;
	FlightPhases[fpFinalApproach].GradientRange.Right := AircraftCategoryConstants.Constant[arFADescent_Max].Value[AirCraftCategory];

//	FlightPhases[fpFinalApproach].TurnRange.Left :=
//	FlightPhases[fpFinalApproach].TurnRange.Right :=

//	FlightPhases[fpFinalApproach].DistRange.Left :=
//	FlightPhases[fpFinalApproach].DistRange.Right :=

end;

//TLeg =========================================================================

constructor TLeg.Create(FIX0, FIX1: TWayPoint; _FlightProcedure: TFlightProcedure; UI: TUIContract; PrevLeg: Tleg = nil);
begin
	Inherited Create;
	FUI := UI;

	FStartFIX := FIX0;
	FEndFIX := FIX1;

	FFullAreaMEl := -1;
	FPrimaryAreaMEl := -1;

//	FNNSecondLineEl := -1;
	FNomTrackEl := -1;

	FFullAreaM := TPolygon.Create;
	FPrimaryAreaM := TPolygon.Create;

	FFullAreaS := TPolygon.Create;
	FPrimaryAreaS := TPolygon.Create;

	FFullArea := TPolygon.Create;
	FPrimaryArea := TPolygon.Create;

	FNNSecondLine := TPolyline.Create;

	FFlightProcedure := _FlightProcedure;

	FBufferSymbol := TFillSymbol.Create;
	FBufferSymbol.Color := RGB(0, 0, 255);
	FBufferSymbol.Style := sfsNULL;		//sfsDiagonalCross;	//sfsForwardDiagonal;
	FBufferSymbol.Outline.Color := 0;
	FBufferSymbol.Outline.Style := slsSolid;

	FProtectSymbol := TFillSymbol.Create;
	FProtectSymbol.Color := RGB(0, 255, 0);
	FProtectSymbol.Style := sfsNULL;	//sfsDiagonalCross;	//sfsBackwardDiagonal;
	FProtectSymbol.Outline.Color := 255;
	FProtectSymbol.Outline.Style := slsSolid;

	FNNSecondSymbol := TLineSymbol.Create(slsSolid, RGB(0, 255, 0), 2);
	FNomTrackSymbol := TLineSymbol.Create(slsSolid, 255, 2);

	CreateNomTrack(PrevLeg);
end;

destructor TLeg.Destroy;
begin
//	FStartFIX.Free;
//	FEndFIX.Free;
	DeleteGraphics;

	FFullArea.Free;
	FPrimaryArea.Free;

	FFullAreaM.Free;
	FPrimaryAreaM.Free;

	FFullAreaS.Free;
	FPrimaryAreaS.Free;

	FNNSecondLine.Free;
	FNomTrack.Free;

	FBufferSymbol.Free;
	FProtectSymbol.Free;

	FNNSecondSymbol.Free;
	FNomTrackSymbol.Free;

	inherited;
end;

function CalcDRNomDir(MATF: TMATF; DestFIX: TFIX): Double;
var
	ptCenter:		TPoint;
	dx, dy,
	TurnAngle,
	distDest, dirDest,
	fTurnDirection,
	PilotTolerance,
	BankTolerance,
	r, dist6sec:	Double;
begin
	PilotTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
	BankTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;

	fTurnDirection := Integer(MATF.TurnDirection);
	dist6sec := (MATF.TAS + GPANSOPSConstants.Constant[dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
	r := MATF.CalcTurnRadius;

	ptCenter := LocalToPrj(MATF.PrjPt, MATF.EntryDirection, dist6sec, -r*fTurnDirection);

	dx := DestFIX.PrjPt.X - ptCenter.X;
	dy := DestFIX.PrjPt.Y - ptCenter.Y;

	dirDest := ArcTan2(dy, dx);
	distDest := Hypot(dy, dx);

	TurnAngle := (MATF.EntryDirection - dirDest) * fTurnDirection + C_PI_2 - ArcCos(r/distDest);
	result := Modulus(MATF.EntryDirection - TurnAngle * fTurnDirection, C_2xPI);
end;

procedure TLeg.CreateNNSecondLine(const PrevLeg: TLeg);
var
	TurnAng, theta, d,
	fSide, LineLen,
	Dir0, Dir2,
	EntryDir:			Double;

	TurnDir:			TSideDirection;

	FullPoly:			TPolygon;
	FullRing:			TRing;
	NNLinePart:			TPart;

	PtTmp0,
	PtTmp1, PtTmp2:				TPoint;
begin
	EntryDir := FStartFIX.EntryDirection;
	TurnAng := FStartFIX.TurnAngle;
	TurnDir := FStartFIX.TurnDirection;
	fSide := Integer(TurnDir);

	LineLen := 10 * EndFIX.ASW_R + EndFIX.ASW_L;

	if TurnDir = SideOn then
	begin
		PtTmp1 := LocalToPrj(StartFIX.PrjPt, EntryDir + PI, StartFIX.EPT, 0);
		Dir0 := EntryDir + 1.5* PI;
		Dir2 := EntryDir + 0.5* PI;
	end
	else
	begin
		theta := 0.25 * (TurnAng);
		d := StartFIX.ATT *  Tan(theta);
		PtTmp1 := LocalToPrj(StartFIX.PrjPt, EntryDir + PI, StartFIX.ATT, -d * fSide);

		if TurnDir = SideLeft then
		begin
			Dir0 := EntryDir + 1.5* PI;
			Dir2 := EntryDir + 0.5 * (PI + TurnAng);
		end
		else
		begin
			Dir0 := EntryDir - 0.5 * (PI + TurnAng);
			Dir2 := EntryDir + 0.5* PI;
		end;
	end;

	FullRing := TRing.Create;
	if Assigned(PrevLeg) then
	begin
		FullPoly := GGeoOperators.UnionGeometry(PrevLeg.FullArea, FFullArea).AsPolygon;
		FullRing.Assign(FullPoly.Ring[0]);
		FullPoly.Free;
	end
	else
		FullRing.Assign(FFullArea.Ring[0]);
{
	PtTmp := RingVectorIntersect (FullRing, PtTmp1, Dir0, d, True);
	if Assigned(PtTmp) then
		PtTmp0 := LocalToPrj(PtTmp1, Dir0, d + OverlapDist, 0)
	else
		PtTmp0 := LocalToPrj(PtTmp1, Dir0, LineLen, 0);
	PtTmp.Free;

	PtTmp := RingVectorIntersect (FullRing, PtTmp1, Dir2, d, True);
	if Assigned(PtTmp) then
		PtTmp2 := LocalToPrj(PtTmp1, Dir2, d + OverlapDist, 0)
	else
		PtTmp2 := LocalToPrj(PtTmp1, Dir2, LineLen, 0);
	PtTmp.Free;
}

	PtTmp0 := RingVectorIntersect (FullRing, PtTmp1, Dir0, d, True);
	if not Assigned(PtTmp0) then
		PtTmp0 := LocalToPrj(PtTmp1, Dir0, LineLen, 0);

	PtTmp2 := RingVectorIntersect (FullRing, PtTmp1, Dir2, d, True);
	if not Assigned(PtTmp2) then
		PtTmp2 := LocalToPrj(PtTmp1, Dir2, LineLen, 0);

	FullRing.Free;

	NNLinePart := TPart.Create;
	NNLinePart.AddPoint(PtTmp0);
	NNLinePart.AddPoint(PtTmp1);
	NNLinePart.AddPoint(PtTmp2);

	PtTmp0.Free;	PtTmp1.Free;	PtTmp2.Free;

	FNNSecondLine.Clear;
	FNNSecondLine.AddPart(NNLinePart);

	NNLinePart.Free;
end;

procedure TLeg.CreateNomTrack(PrevLeg: TLeg);
var
	I,
	N:					Integer;
	PilotTolerance,
	BankTolerance,
	Dist6sec,
	fTmp, NomDir,
	DivergenceAngle,
	DivergenceAngle30,
	Bank15, fTurnDir,
	OutDir, EntryDir,
	TurnDist, TurnAngle,
	L1, L2, R1, R2:		Double;

	pt6sec,
	ptTmp, ptFlyBy,
	PtTmp0, PtTmp1,
	PtTmp2, PtTmp3:		TPoint;
	TrackPoints:		TMultiPoint;
//	TurnDir:			TSideDirection;
	MATF:				TMATF;
begin
	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	Bank15 := GPANSOPSConstants.Constant[rnvFlyOInterBank].Value;

	EntryDir := FStartFIX.EntryDirection;
	OutDir := FEndFIX.EntryDirection;

	//TurnDir := FStartFIX.TurnDirection;			//SideDef(FStartFIX.PrjPt, EntryDir, FEndFIX.PrjPt);
	//fTurnDir := Integer(TurnDir);
//	fTurnDir := Integer(SideDef(FStartFIX.PrjPt, EntryDir, FEndFIX.PrjPt));//
	fTurnDir := Integer(FStartFIX.TurnDirection);

	TurnAngle := Modulus((EntryDir - OutDir) * fTurnDir, PI);
//	TurnAngle := (FStartFIX.EntryDirection - NomDIr)* fTurnDir;
//	TurnAngle := Modulus((FStartFIX.EntryDirection - EntryDir)* fTurnDirection, C_2xPI);

	R1 := StartFIX.CalcTurnRadius;
//	R1 := BankToRadius(FStartFIX.Bank, FStartFIX.TAS);
//	R1 := FStartFIX.CalcTurnRadius;	//BankToRadius(FStartFIX.Bank, FStartFIX.TAS);

	fTmp := Modulus(TurnAngle, PI);

	TrackPoints := TMultipoint.Create;

	if Abs(TurnAngle) < EpsilonRadian then
	begin
		ptTmp := StartFIX.PrjPt;
		ptTmp.M := StartFIX.EntryDirection;
		TrackPoints.AddPoint(ptTmp);

		ptTmp := FEndFIX.PrjPt;
		ptTmp.M := OutDir;
		TrackPoints.AddPoint(ptTmp);
	end
	else if StartFIX is TMATF then
	begin
		MATF := TMATF(FStartFIX);

		if MATF.FlyPath = fpDirectToFIX then
			NomDir := CalcDRNomDir(MATF, TFIX(FEndFIX))
		else
			NomDir := MATF.OutDirection;

		TurnAngle := (FStartFIX.EntryDirection - NomDIr)* fTurnDir;

		if (MATF.FlyPath = fpCourseToFIX)and ((MATF.TurnAt = taMApt)or(MATF.FlyMode = fmFlyOver)) then
		begin
			R2 := BankToRadius(Bank15, FStartFIX.TAS);

		//	EntryDir := FStartFIX.EntryDirection;
		//	OutDir := FEndFIX.EntryDirection;
		//TurnDir := SideDef(FStartFIX.PrjPt, FStartFIX.EntryDirection, FEndFIX.PrjPt);
		//fSide := Integer(TurnDir);
		//TurnAngle := Modulus((FStartFIX.EntryDirection - FEndFIX.EntryDirection) * fTurnDir, PI);

			fTmp := ArcCos((1 + R1 * Cos(TurnAngle)/R2)/(1 + R1/R2)) - EpsilonRadian;

			DivergenceAngle := Min(DivergenceAngle30, fTmp);

			if MATF.TurnAt = taMApt then
				ptTmp := MATF.MAPt.PrjPt
			else
				ptTmp := MATF.PrjPt;

			ptTmp.M := FStartFIX.EntryDirection;
			TrackPoints.AddPoint(ptTmp);					//	Point1

			PtTmp0 := TPoint.Create;

			LocalToPrj(ptTmp, FStartFIX.EntryDirection - 0.5*PI * fTurnDir, R1, 0, PtTmp0);

			fTmp := FStartFIX.EntryDirection - (TurnAngle + DivergenceAngle) * fTurnDir;

			PtTmp1 := PointAlongPlane(PtTmp0, fTmp + 0.5*PI * fTurnDir, R1);
			PtTmp1.M := fTmp;
			TrackPoints.AddPoint(PtTmp1);					//	Point2

			PtTmp0.Free;
			PtTmp0 := LineLineIntersect(PtTmp1, PtTmp1.M, EndFIX.PrjPt, FEndFIX.EntryDirection).AsPoint;

			fTmp := R2 * Tan(0.5 * DivergenceAngle);

			PtTmp2 := LocalToPrj(PtTmp0, PtTmp1.M + PI, fTmp, 0);
			PtTmp2.M := PtTmp1.M;

			PtTmp3 := LocalToPrj(PtTmp0, FEndFIX.EntryDirection, fTmp, 0);
			PtTmp3.M := FEndFIX.EntryDirection;

			TrackPoints.AddPoint(PtTmp2);					//	Point3
			TrackPoints.AddPoint(PtTmp3);					//	Point4

			PtTmp0.Assign(FEndFIX.PrjPt);
			PtTmp0.M := FEndFIX.EntryDirection;
			TrackPoints.AddPoint(PtTmp0);

			PtTmp0.Free;
			PtTmp1.Free;
			PtTmp2.Free;
			PtTmp3.Free;
		end
		else if (fTmp > EpsilonRadian) and (PI - fTmp > EpsilonRadian) then
		begin
			ptFlyBy := LineLineIntersect(FStartFIX.PrjPt, FStartFIX.EntryDirection, FEndFIX.PrjPt, NomDir).AsPoint;

			TurnDist := R1/Tan(0.5 * (PI-TurnAngle));
			PtTmp1 := LocalToPrj(ptFlyBy, FStartFIX.EntryDirection, -TurnDist, 0);
			PtTmp1.M := FStartFIX.EntryDirection;

			if PointToLineDistance(PtTmp1, FStartFIX.PrjPt, FStartFIX.EntryDirection + C_PI_2) < 0 then
			begin
				ptTmp := FStartFIX.PrjPt;
				ptTmp.M := FStartFIX.EntryDirection;
				TrackPoints.AddPoint(ptTmp);
			end;

			TrackPoints.AddPoint(PtTmp1);

			LocalToPrj(ptFlyBy, NomDir, TurnDist, 0, PtTmp1);
			PtTmp1.M := NomDir;
			TrackPoints.AddPoint(PtTmp1);

			if PointToLineDistance(PtTmp1, FEndFIX.PrjPt, NomDir + C_PI_2) > 0 then
			begin
				ptTmp := FEndFIX.PrjPt;
				ptTmp.M := NomDir;
				TrackPoints.AddPoint(ptTmp);
			end;

			ptFlyBy.Free;
			PtTmp1.Free;
		end
		else
		begin
			ptTmp := FStartFIX.PrjPt;
			ptTmp.M := FStartFIX.EntryDirection;
			TrackPoints.AddPoint(ptTmp);
			pt6sec := TPoint.Create;

			if (MATF.TurnAt in [taMApt, taHeight]) then
			begin
				PilotTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
				BankTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
				Dist6sec := (MATF.TAS + GPANSOPSConstants.Constant[dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
				LocalToPrj(MATF.PrjPt, MATF.EntryDirection, Dist6sec, 0, pt6sec);
				pt6sec.M := MATF.EntryDirection;
				TrackPoints.AddPoint(pt6sec);
			end
			else
				pt6sec.Assign(MATF.PrjPt);

			PtTmp1 := PrjToLocal(pt6sec, MATF.EntryDirection, FEndFIX.PrjPt);

			if PtTmp1.X > 0 then
			begin
				LocalToPrj(pt6sec, MATF.EntryDirection, PtTmp1.X, 0, PtTmp1);
				PtTmp1.M := NomDir;
				TrackPoints.AddPoint(PtTmp1);
			end
			else
			begin
				LocalToPrj(FEndFIX.PrjPt, NomDir, PtTmp1.X, 0, PtTmp1);
				PtTmp1.M := NomDir;
				TrackPoints.AddPoint(PtTmp1);
			end;

			ptTmp := FEndFIX.PrjPt;
			ptTmp.M := NomDir;
			TrackPoints.AddPoint(ptTmp);

			pt6sec.Free;
			PtTmp1.Free;
		end;
	end
	else
	begin
//		if FStartFIX.TurnDirection = sideOn then	L1 := 0
//		else

		if StartFIX.FlyMode = fmFlyBy then		L1 := R1 * Sin(TurnAngle)
		else									L1 := R1 * Tan(0.5 * TurnAngle);

		if not (FStartFIX is TMATF) then
		begin
			PtTmp0 := PointAlongPlane(FStartFIX.PrjPt, EntryDir - PI, L1);
			PtTmp0.M := EntryDir;
			TrackPoints.AddPoint(PtTmp0);				//	Point1
		end
		else
			PtTmp0 := TPoint.Create;

//		MATF := TMATF(FStartFIX);
//		if (MATF.FlyPath = fpCourseToFIX)and ((MATF.TurnAt = taMApt)or(MATF.FlyMode = fmFlyOver)) then
//		begin
// 		end;

//		if TurnDir = sideOn then
//		begin
//			GUI.DrawPointWithText(StartFIX.PrjPt, 255, '1-ci');
//			GUI.DrawPointWithText(EndFIX.PrjPt, 255, '2-ci');
//		end else

		if StartFIX.FlyMode = fmFlyBy then
		begin
			L2 := R1 * Cos(TurnAngle) * Tan(DivergenceAngle30);

			LocalToPrj(FStartFIX.PrjPt, EntryDir - TurnAngle * fTurnDir, L1, 0, PtTmp0);
			PtTmp0.M := EntryDir - TurnAngle * fTurnDir;
			TrackPoints.AddPoint(PtTmp0);			//	Point2

			PtTmp1 := LocalToPrj(PtTmp0, PtTmp0.M, L2, 0);
			PtTmp1.M := PtTmp0.M;
			TrackPoints.AddPoint(PtTmp1);			//	Point3
			PtTmp1.Free;
		end
		else if StartFIX.FlyMode = fmFlyOver then
		begin
			R2 := BankToRadius(Bank15, StartFIX.TAS);

			fTmp := ArcCos((1 + R1 * Cos(TurnAngle)/R2)/(1 + R1/R2)) - EpsilonRadian;

			DivergenceAngle := Min(DivergenceAngle30, fTmp);

			PtTmp0.Assign(FStartFIX.PrjPt);
			PtTmp0.M := EntryDir;
			TrackPoints.AddPoint(PtTmp0);		//	Point2

			LocalToPrj(FStartFIX.PrjPt, EntryDir - 0.5*PI * fTurnDir, R1, 0, PtTmp0);
			fTmp := EntryDir - (TurnAngle + DivergenceAngle) * fTurnDir;

			PtTmp1 := PointAlongPlane(PtTmp0, fTmp + 0.5*PI * fTurnDir, R1);
			PtTmp1.M := fTmp;
			TrackPoints.AddPoint(PtTmp1);			//	Point3

			PtTmp0.Free;
			PtTmp0 := LineLineIntersect(PtTmp1, PtTmp1.M, FEndFIX.PrjPt, OutDir).AsPoint;

			fTmp := R2 * Tan(0.5 * DivergenceAngle);

			PtTmp2 := LocalToPrj(PtTmp0, PtTmp1.M + PI, fTmp, 0);
			PtTmp2.M := PtTmp1.M;

			PtTmp3 := LocalToPrj(PtTmp0, OutDir, fTmp, 0);
			PtTmp3.M := OutDir;

			TrackPoints.AddPoint(PtTmp2);			//	Point4
			TrackPoints.AddPoint(PtTmp3);			//	Point5
			PtTmp1.Free;
			PtTmp2.Free;
			PtTmp3.Free;
		end;

		PtTmp0.Assign(FEndFIX.PrjPt);
		PtTmp0.M := OutDir;
		TrackPoints.AddPoint(PtTmp0);
		PtTmp0.Free;
	end;

	FNomTrack := ConvertPointsToTrackLIne(TrackPoints);
{
if FEndFIX.Role = MAPt_ then
begin
	GUI.DrawPolyline(FNomTrack, RGB(0,255,0), 2);
	for I:=0 to TrackPoints.Count-1 do
		GUI.DrawPointWithText(TrackPoints.Point[I], RGB(0,0,255), IntToStr(I+1))
end;
}

	TrackPoints.Free;

	if Assigned(PrevLeg) and Assigned(PrevLeg.NomTrack) then
	begin
		N := PrevLeg.NomTrack.Part[0].Count;
		PrevLeg.NomTrack.Part[0].Point[N - 1].Assign(FNomTrack.Part[0].Point[0]);
	end;
end;

procedure TLeg.CreateProtectionArea(PrevLeg: TLeg; ARP: TAHP);
begin
	Functions.CreateProtectionArea(PrevLeg, Self, ARP);
	CreateAssesmentArea(PrevLeg, Self);
	CreateNNSecondLine(PrevLeg);
end;

procedure TLeg.SetFullArea(Val: TPolygon);
begin
	FFullArea.Assign(Val);
	FFullAreaM.Assign(Val);
	FFullAreaS.Assign(Val);
end;

procedure TLeg.SetPrimaryArea(Val: TPolygon);
begin
	FPrimaryArea.Assign(Val);
	FPrimaryAreaM.Assign(Val);
	FPrimaryAreaS.Assign(Val);
end;

procedure TLeg.SetFullAreaM(Val: TPolygon);
begin
	FFullAreaM.Assign(Val);
end;

procedure TLeg.SetPrimaryAreaM(Val: TPolygon);
begin
	FPrimaryAreaM.Assign(Val);
end;

procedure TLeg.SetFullAreaS(Val: TPolygon);
begin
	FFullAreaS.Assign(Val);
end;

procedure TLeg.SetPrimaryAreaS(Val: TPolygon);
begin
	FPrimaryAreaS.Assign(Val);
end;

procedure TLeg.SetNNSecondLine(Val: TPolyline);
begin
	FNNSecondLine.Assign(Val);
end;

procedure TLeg.SetNomTrack(Val: TPolyline);
begin
	FNomTrack.Assign(Val);
end;

procedure TLeg.DeleteGraphics;
begin
//	FUI.DeleteGraphic(FRightBufferMEl);
//	FUI.DeleteGraphic(FLeftBufferMEl);

	FUI.SafeDeleteGraphic(FFullAreaMEl);
	FUI.SafeDeleteGraphic(FPrimaryAreaMEl);
//	FUI.SafeDeleteGraphic(FNNSecondLineEl);
	FUI.SafeDeleteGraphic(FNomTrackEl);

//	FStartFIX.DeleteGraphics;
//	FEndFIX.DeleteGraphics;
end;

procedure TLeg.RefreshGraphics;
begin
	FUI.SafeDeleteGraphic(FFullAreaMEl);
	FUI.SafeDeleteGraphic(FPrimaryAreaMEl);
//	FUI.SafeDeleteGraphic(FNNSecondLineEl);
	FUI.SafeDeleteGraphic(FNomTrackEl);

	FFullAreaMEl := FUI.DrawPolygon(FFullAreaS, FBufferSymbol);
	FPrimaryAreaMEl := FUI.DrawPolygon(FPrimaryAreaS, FProtectSymbol);

//	FNNSecondLineEl := FUI.DrawPolyline(FNNSecondLine, FNNSecondSymbol);
	FNomTrackEl := FUI.DrawPolyline(FNomTrack, FNomTrackSymbol);

//	if RefreshFIXs then
	begin
		FStartFIX.RefreshGraphics;
		FEndFIX.RefreshGraphics;
	end;
end;

end.
