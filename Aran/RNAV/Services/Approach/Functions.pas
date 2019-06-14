unit Functions;

interface
uses
	Windows, Classes, CollectionUnit, AIXMTypes, FIXUnit, FlightPhaseUnit, Geometry,
	ARANMath, ObstacleInfoUnit;

const
	OverlapDist = 0.5;

function GetBasePoints(refPoint: TPoint; refDirection: Double; TolerPoly: TPolygon; TurnSide: TSideDirection): TList;
function CreateNomTrack(PrevLeg, Leg: TLeg): TPolyline;

function CreateNNSecondLine(const PrevLeg, Leg: TLeg): TPolyline;
function CreateProtectionArea(const PrevLeg: TLeg; var Leg: TLeg; ARP: TAHP): Boolean;
procedure CreateAssesmentArea(PrevLeg, Leg: TLeg);
function CalcMAMinOCA(ObstacleList: TList; Gradiend: Double; MinValue: Double; var MaxIx: Integer; AndMask: Integer = -1): Double;

function GetObstacles(Polygon: TPolygon; Obstalces: TObstacleList): TList;		overload;
function GetObstacles(Polygon: TPolygon; ObstacleInfos: TList): TList;			overload;

function GetObstacleInfoList(PrimPolygon, SecondPolygon: TPolygon; MOC: Double; Obstalces: TObstacleList): TList;

function SpiralTouchAngleOld (r0, coef, nominalDir, touchDir: Double; turnDir: TSideDirection; AllowBulge: Boolean = True): Double;

function SpiralTouchAngle(r0, coeff, SpStartRadial, StartTouch, EndTouch: Double; turnDir: TSideDirection): Double;

function SpiralTouchToPoint(ptCnt: TPoint; r0, coef, EntryDir: Double;
							turnSide: TSideDirection; ptDst: TPoint): Double;

procedure AddSpiralToRing(ptCnt: TPoint; r0, coef, StartRadial, TurnAngle: Double;
							turnSide: TSideDirection; Ring: TRing);

function CalcDRNomDir(MATF: TMATF; DestFIX: TFIX): Double;
function CreateTrackPoints(OrignPT, DestPT: TWayPoint; EntryDir: Double): TMultipoint;

function CalcDFDirection(MATF: TMATF; MAHF: TFIX): Double;
function CreateMATurnArea(MATF: TMATF; MAHF: TFIX; OutDir: Double; Primary, Direct: Boolean): TPolygon;
function CalcMinRange(FirstFIX, SecondFIX: TWayPoint; NextAngle: Double): Double;

var
	GlDRAWFLG:	Boolean = False;

implementation

uses
	Math, SysUtils, UIContract, ConstantsContract,
	ARANFunctions, UnitConverter, ARANGlobals, ApproachGlobals;

var
	Counter: integer = 0;

function CreatePrjCircle(PtCnt: TPoint; Radius: Double): TPolygon;
var
	I:			LongInt;
	iInRad:		Double;
	pt:			TPoint;
	Ring:		TRing;
begin
	Ring := TRing.Create;
	pt := TPoint.Create;

	for I := 0 to 359 do
	begin
		iInRad := DegToRad(I);
		pt.X := PtCnt.X + Radius * Cos(iInRad);
		pt.Y := PtCnt.Y + Radius * Sin(iInRad);
		Ring.AddPoint(pt);
	end;

	result := TPolygon.Create;
	result.AddRing(Ring);

	pt.Free;
	Ring.Free;
end;

function GetBasePoints(refPoint: TPoint; refDirection: Double; TolerPoly: TPolygon; TurnSide: TSideDirection): TList;
var
	Ring:			TRing;
	pt0, pt1,
	pt2, ptTmp:		TPoint;

	I, I0, I1, I2,
	N, sg:			Integer;

	Eps, fTmp,
	dx0, dy0,
	dx1, dy1,

	MaxDist:		Double;
	TmpList:		TList;
begin
	result := TList.Create;
	TmpList := TList.Create;

	Ring := TolerPoly.Ring[0];
	N := Ring.Count;

	for I := 0 to N - 1 do
		TmpList.Add(Ring.Point[I]);

	Eps := EpsilonDistance*EpsilonDistance;
//==============================================================================
	pt0 := TmpList.Items[N-1];
	pt1 := TmpList.Items[0];

	I1 := 0;

	while  I1 < N do
	begin
		I2 := (I1 + 1) and (0 - Integer(I1 + 1 < N));
		pt2 := TmpList.Items[I2];

		dx0 := pt1.X - pt0.X;
		dy0 := pt1.Y - pt0.Y;

		dx1 := pt2.X - pt1.X;
		dy1 := pt2.Y - pt1.Y;

		if Abs(Abs(dx0*dy1) - Abs(dx1*dy0)) < Eps then
		begin
			TmpList.Delete(I1);
			N := TmpList.Count;
			if I1 < N then	pt1 := TmpList.Items[I1];
		end
		else
		begin
			pt0 := pt1;
			pt1 := pt2;
			Inc(I1);
		end;
	end;
//==============================================================================
	N := TmpList.Count;
	MaxDist := -300000.0;

	ptTmp := TPoint.Create;
	pt0 := nil;

	for I := 0 to N - 1 do
	begin
		PrjToLocal(refPoint, refDirection, TPoint(TmpList.Items[I]), ptTmp);
		if (ptTmp.X > MaxDist) and (Sign(ptTmp.Y) = Integer(TurnSide)) then
		begin
			MaxDist := ptTmp.X;
			pt0 := TmpList.Items[I];
			I0 := I;
		end;
	end;

	ptTmp.Free;

	result.Add(pt0);

	I := (I0 + 1) and(0 - Integer(I0 + 1 < N));
	ptTmp := TmpList.Items[I];

	fTmp := Modulus((ArcTan2(ptTmp.Y - pt0.Y, ptTmp.X - pt0.X) - refDirection), C_2xPI);
	if fTmp > PI then
		fTmp :=  C_2xPI - fTmp;

	sg := (2 * Integer(fTmp > C_2_PI)-1) * Integer(TurnSide);

	for I := 1 to N - 1 do
	begin
		I1 := (I0 + N + sg * I) Mod N;
		result.Add(TmpList.Items[I1]);
	end;

	TmpList.Free;
end;

function GetBasePointsOld(refPoint: TPoint; refDirection: Double; TolerPoly: TPolygon; TurnSide: TSideDirection): TList;
var
	Ring:			TRing;
	pt0, pt1,
	pt2, ptTmp:		TPoint;

	I, I0, I1, I2,
	N, sg:			Integer;

	Eps,
	dx0, dy0,
	dx1, dy1,
	fTurnSide,
	MaxDist:		Double;
begin
	result := TList.Create;
	fTurnSide := Integer(TurnSide);

	Ring := TolerPoly.Ring[0];
	N := Ring.Count;

	pt0 := Ring.Point[0];
	ptTmp := Ring.Point[N-1];
	if Hypot(pt0.Y - ptTmp.Y, pt0.X - ptTmp.X) < EpsilonDistance then
		Dec(N);

	MaxDist := -300000.0;
	ptTmp := TPoint.Create;
	pt0 := nil;

	for I := 0 to N-1 do
	begin
		PrjToLocal(refPoint, refDirection, Ring.Point[I], ptTmp);
		if (ptTmp.X > MaxDist) and (Sign(ptTmp.Y) = Integer(TurnSide)) then
		begin
			MaxDist := ptTmp.X;
			pt0 := Ring.Point[I];
			I0 := I;
		end;
	end;

	ptTmp.Free;

//	if not Assigned(pt0) then
//	begin
//	end;

	result.Add(pt0);

	I := (I0 + 1) and(0 - Integer(I0 + 1 < N));
	ptTmp := Ring.Point[I];

	sg := 1 - 2 * Integer(Modulus(refDirection - ArcTan2(ptTmp.Y - pt0.Y,
					ptTmp.X - pt0.X)*fTurnSide, 0.55*C_2xPI) > PI);

	I2 := (I0 + N + sg) Mod N;
	pt2 := Ring.Point[I2];
	Eps := EpsilonDistance*EpsilonDistance;

	I := 1;
	while I < N do
	begin
		I1 := I2;
		I2 := (I1 + N + sg) Mod N;

		pt1 := pt2;
		pt2 := Ring.Point[I2];

		dx0 := pt1.X - pt0.X;
		dy0 := pt1.Y - pt0.Y;

		dx1 := pt2.X - pt1.X;
		dy1 := pt2.Y - pt1.Y;

		if Abs(Abs(dx0*dy1) - Abs(dx1*dy0)) > Eps then
		begin
			result.Add(pt1);
			pt0 := pt1;
		end;

		Inc(I);
	end;
end;

function CalcDFDirection(MATF: TMATF; MAHF: TFIX): Double;
var
	ptCnt:			TPoint;
	Ring:			TRing;
	BasePoints:		TList;
	I, J, N:		Integer;

	WSpeed, TurnR,
	TurnRCurr,
	SpTurnAngle,
	SpStartRadial,
	fTurnSide, dirSpCenter,
	Rv, coeff, fTmp,
	dirTouch,
	dirCurr, dirRef,
	dirCntToPtDst:	Double;
begin
	dirTouch := 9999.0;
	result := dirTouch;

	if MATF.TolerArea.Count = 0 then	exit;
	BasePoints := GetBasePoints(MATF.PrjPt, MATF.EntryDirection, MATF.TolerArea, MATF.TurnDirection);
//==============================================================================

	WSpeed := GPANSOPSConstants.Constant[dpWind_Speed].Value;

	Rv := (1.76527777777777777777 / PI) * Tan(MATF.Bank) /  MATF.TAS;
	if Rv > 0.003 then	Rv := 0.003;

	if Rv > 0.0 then	TurnR := MATF.TAS / ((5.555555555555555555555 * PI) * Rv)
	else				TurnR := -1;

	coeff := WSpeed / DegToRad(1000.0*Rv);

	fTurnSide := Integer(MATF.TurnDirection);

	ptCnt := TPoint.Create;
	Ring := TRing.Create;

	dirSpCenter :=  MATF.EntryDirection - C_PI_2*fTurnSide;
	SpStartRadial := MATF.EntryDirection + C_PI_2*fTurnSide;

	TurnRCurr := TurnR;
	dirCurr := MATF.EntryDirection + ArcTan2(coeff, TurnRCurr)*fTurnSide;
	N := BasePoints.Count;

//	for I := 0 to N - 1 do
//GUI.DrawPointWithText(TPoint(BasePoints.Items[I]), RGB(0, 255, 128+127*integer(MATF.TurnDirection)), IntToStr(I+1));


	for I := 0 to N - 1 do
	begin
		LocalToPrj(TPoint(BasePoints.Items[I]), dirSpCenter, TurnR, 0, ptCnt);
//GUI.DrawPoint(TPoint(BasePoints.Items[I]), RGB(0, 255, 0));

		J := (I + 1) and(0 - Integer(I + 1 < N));

		dirRef := ArcTan2(TPoint(BasePoints.Items[J]).Y - TPoint(BasePoints.Items[I]).Y,
							TPoint(BasePoints.Items[J]).X - TPoint(BasePoints.Items[I]).X);
		dirCntToPtDst := ArcTan2(MAHF.PrjPt.Y - ptCnt.Y, MAHF.PrjPt.X - ptCnt.X);

		fTmp := Modulus((dirCntToPtDst - dirRef)*fTurnSide, C_2xPI);

		if fTmp < PI then
		begin
			SpTurnAngle := Functions.SpiralTouchToPoint(ptCnt, TurnRCurr, coeff, dirCurr,
									MATF.TurnDirection, MAHF.PrjPt);

			if SpTurnAngle > 100 then
				exit;

			Rv := TurnRCurr + coeff * SpTurnAngle;
			dirTouch := SpStartRadial - (SpTurnAngle + C_PI_2 - ArcTan2(coeff, Rv))*fTurnSide;

			fTmp := Modulus((dirTouch - dirRef)*fTurnSide, C_2xPI);

			if fTmp < PI then
			begin
//GUI.DrawPoint(TPoint(BasePoints.Items[I]), RGB(0,0,255));
				break;
			end;
		end;

		SpTurnAngle := Functions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, MATF.TurnDirection);

		if SpTurnAngle <= PI then
		begin
			TurnRCurr := TurnRCurr + coeff*SpTurnAngle;
			SpStartRadial := SpStartRadial - SpTurnAngle*fTurnSide;
		end;

		dirCurr := dirRef;
	end;

	ptCnt.Free;
	BasePoints.Free;
	Ring.Free;

	result := Modulus(dirTouch, C_2xPI);
end;

function CreateMATurnArea(MATF: TMATF; MAHF: TFIX; OutDir: Double; Primary, Direct: Boolean): TPolygon;
var
	ptCurr, ptRef,
	ptCnt, //ptPrev,
	ptTmp:				TPoint;
	Ring:				TRing;
	PrimaryPoly,
	FullPoly:			TPolygon;
	Geom:				TGeometry;

	BasePoints,
	PrimBasePoints,
	SecBasePoints:		TList;

	I, Ist, c30, 
	J, N, M, sg:		Integer;
	refFlg, bFlag:		Boolean;

	R, K, dR, AreaWidth,
	SplayAngle15,
	DivergenceAngle30,
	PrevX, PrevY, CurrY,
	MaxDist, MaxDir, TurnAng,
	WSpeed, TurnR, OutDir2,
	TurnRCurr, dAlpha,
	SpTurnAngle,
	SpStartRadial,
	fTurnSide, dirSpCenter,
	Rv, coeff, fTmp,
	dirTouch, dirDst,
	dirDst15, dirDst30,
	dirCurr, dirRef,
	dirCntToPtDst:		Double;
begin
	result := nil;
	if MATF.TolerArea.Count = 0 then	exit;

	PrimBasePoints := GetBasePoints(MATF.PrjPt, MATF.EntryDirection, MATF.TolerArea, MATF.TurnDirection);
	SecBasePoints := GetBasePoints(MATF.PrjPt, MATF.EntryDirection, MATF.SecondaryTolerArea, MATF.TurnDirection);
//==============================================================================

	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	fTurnSide := Integer(MATF.TurnDirection);
	dirDst15 := OutDir + SplayAngle15 * fTurnSide;

	WSpeed := GPANSOPSConstants.Constant[dpWind_Speed].Value;

	Rv := (1.76527777777777777777 / PI) * Tan(MATF.Bank) /  MATF.TAS;
	if Rv > 0.003 then	Rv := 0.003;

	if Rv > 0.0 then	TurnR := MATF.TAS / ((5.555555555555555555555 * PI) * Rv)
	else				TurnR := -1;

	coeff := WSpeed / DegToRad(1000.0*Rv);

//Outer side
//===================================================================================

	ptTmp := TPoint.Create;
	ptCnt := TPoint.Create;
	Ring := TRing.Create;
	dirSpCenter :=  MATF.EntryDirection - C_PI_2*fTurnSide;
	SpStartRadial := MATF.EntryDirection + C_PI_2*fTurnSide;

	if Primary then
	begin
		dR := 0.0;
		AreaWidth := 0.5 * MAHF.SemiWidth;
		BasePoints := PrimBasePoints;
//		for I := 0 to BasePoints.Count-1 do
//			GUI.DrawPointWithText(TPoint(BasePoints.Items[I]), 0, 'BP-'+ IntToStr(I+1))
	end
	else
	begin
		dR := Hypot(TPoint(PrimBasePoints.Items[0]).Y - TPoint(SecBasePoints.Items[0]).Y,
					TPoint(PrimBasePoints.Items[0]).X - TPoint(SecBasePoints.Items[0]).X);
		AreaWidth := MAHF.SemiWidth;
		BasePoints := SecBasePoints;
	end;

	TurnRCurr := TurnR + dR;
	dirCurr := MATF.EntryDirection + ArcTan2(coeff, TurnRCurr)*fTurnSide;
	N := BasePoints.Count;

	if (Modulus((dirCurr-dirDst15)*fTurnSide, C_2xPI) < PI)or(Modulus((dirCurr - OutDir)*fTurnSide, C_2xPI) > 2*SplayAngle15) then
	begin
		for I := 0 to N-1 do
		begin
			if Primary then
				LocalToPrj(TPoint(BasePoints.Items[I]), dirSpCenter, TurnR, 0, ptCnt)
			else
			begin
				PrjToLocal(MATF.PrjPt, MATF.EntryDirection, TPoint(BasePoints.Items[I]), ptTmp);
				LocalToPrj(TPoint(BasePoints.Items[I]), dirSpCenter, TurnR +sign(ptTmp.Y * fTurnSide)*dR, 0, ptCnt);
			end;

			J := (I + 1) and(0 - Integer(I + 1 < N));

			dirRef := ArcTan2(TPoint(BasePoints.Items[J]).Y - TPoint(BasePoints.Items[I]).Y,
								TPoint(BasePoints.Items[J]).X - TPoint(BasePoints.Items[I]).X);

			dAlpha := Modulus((dirCurr - dirDst15)*fTurnSide, C_2xPI);
			fTmp := Modulus((dirCurr - dirRef)*fTurnSide, C_2xPI);
//			fTmp := Modulus((dirDst15 - dirRef)*fTurnSide, C_2xPI);
//			if fTmp < PI then
			if dAlpha < fTmp then
			begin
				SpTurnAngle := Functions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst15, MATF.TurnDirection);

				AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
										SpTurnAngle, MATF.TurnDirection, Ring);

				dirCurr := dirDst15;
				TurnRCurr := TurnRCurr + coeff*SpTurnAngle;
				SpStartRadial := SpStartRadial - SpTurnAngle*fTurnSide;
				break;
			end;

			SpTurnAngle := Functions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, MATF.TurnDirection);

			if SpTurnAngle > PI then
			begin
				Geom := LineLineIntersect(Ring.Point[Ring.Count-1], dirCurr, TPoint(BasePoints.Items[J]), dirSpCenter);
				if Geom.GeometryType = gtPoint then
					Ring.AddPoint(Geom.AsPoint);
				Geom.Free;
			end
			else
			begin
				AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
										SpTurnAngle, MATF.TurnDirection, Ring);

				TurnRCurr := TurnRCurr + coeff*SpTurnAngle;
				SpStartRadial := SpStartRadial - SpTurnAngle*fTurnSide;
			end;
			dirCurr := dirRef;
		end;
		ptCurr := Ring.Point[Ring.Count-1];
	end
	else
	begin
		if Primary then
			LocalToPrj(TPoint(BasePoints.Items[0]), dirSpCenter, TurnR, 0, ptCnt)
		else
		begin
			PrjToLocal(MATF.PrjPt, MATF.EntryDirection, TPoint(BasePoints.Items[0]), ptTmp);
			LocalToPrj(TPoint(BasePoints.Items[0]), dirSpCenter, TurnR +sign(ptTmp.Y * fTurnSide)*dR, 0, ptCnt);
		end;

		ptCurr := BasePoints.Items[0];
		Ring.AddPoint(ptCurr);
		J := 1;
		dirRef := ArcTan2(TPoint(BasePoints.Items[1]).Y - TPoint(BasePoints.Items[0]).Y,
						TPoint(BasePoints.Items[1]).X - TPoint(BasePoints.Items[0]).X);
	end;

	ptRef := LocalToPrj(MAHF.PrjPt, OutDir, 0, AreaWidth * fTurnSide);

//GUI.DrawPoint(ptRef, 255);


	PrjToLocal(ptRef, OutDir, ptCurr, ptTmp);
	refFlg := True;

	if ptTmp.Y * fTurnSide < 0 then
	begin
		Geom := LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir);
		if Geom.GeometryType = gtPoint then
		begin
			PrjToLocal(MAHF.PrjPt, OutDir, Geom.AsPoint, ptTmp);
			if ptTmp.X > 0 then
			begin
				Geom.Free;
				Geom := LineLineIntersect(ptCurr, dirDst15, ptRef, OutDir + C_PI_2);
				refFlg := False;
			end;

			if Geom.GeometryType = gtPoint then
				Ring.AddPoint(Geom.AsPoint);
		end;

		Geom.Free;
	end
	else
	begin
		dirDst30 := OutDir - DivergenceAngle30 * fTurnSide;

		if Modulus((dirDst30 - dirRef) * fTurnSide, C_2xPI) > PI then
		begin
			SpTurnAngle := Functions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirRef, MATF.TurnDirection);

			AddSpiralToRing(ptCnt, TurnRCurr, coeff, SpStartRadial,
									SpTurnAngle, MATF.TurnDirection, Ring);
			dirCurr := dirRef;
			TurnRCurr := TurnRCurr + coeff*SpTurnAngle;
			SpStartRadial := SpStartRadial - SpTurnAngle*fTurnSide;

			if Primary then
				LocalToPrj(TPoint(BasePoints.Items[J]), dirSpCenter, TurnR, 0, ptCnt)
			else
			begin
				PrjToLocal(MATF.PrjPt, MATF.EntryDirection, TPoint(BasePoints.Items[J]), ptTmp);
				LocalToPrj(TPoint(BasePoints.Items[J]), dirSpCenter, TurnR +sign(ptTmp.Y * fTurnSide)*dR, 0, ptCnt);
			end
		end;

		SpTurnAngle := Functions.SpiralTouchAngle(TurnRCurr, coeff, SpStartRadial, dirCurr, dirDst30, MATF.TurnDirection);
		if SpTurnAngle >= 0 then
		begin
			ptCurr := TPoint.Create;
			M := Round(RadToDeg(SpTurnAngle));
			dAlpha := 0;
			if M > 0 then
			begin
				if M < 5 then	M := 5
				else if M < 10 then M := 10;
				dAlpha := SpTurnAngle / M;
			end;
			bFlag := False;

			for I := 0 to M do
			begin
				R := TurnRCurr + I * dAlpha * coeff;
				LocalToPrj(ptCnt, SpStartRadial - I * dAlpha * fTurnSide, R, 0, ptCurr);

				PrjToLocal(ptRef, OutDir, ptCurr, ptTmp);

				if (I > 0) and((ptTmp.Y * fTurnSide <= 0)or(ptTmp.X >= 0)) then
				begin
					refFlg := ptTmp.X <= 0;

					if ptTmp.X < 0 then		K := -PrevY/(ptTmp.Y - PrevY)
					else					K := -PrevX/(ptTmp.X - PrevX);

					ptCurr.X := Ring.Point[Ring.Count-1].X + K *(ptCurr.X - Ring.Point[Ring.Count-1].X);
					ptCurr.Y := Ring.Point[Ring.Count-1].Y + K *(ptCurr.Y - Ring.Point[Ring.Count-1].Y);

					Ring.AddPoint(ptCurr);
					bFlag := True;
					break;
				end;

				PrevX := ptTmp.X;
				PrevY := ptTmp.Y;
				Ring.AddPoint(ptCurr);
			end;

			if (not bFlag) and refFlg then
			begin
				Geom := LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir);
				if Geom.GeometryType = gtPoint then
				begin
					PrjToLocal(MAHF.PrjPt, OutDir, Geom.AsPoint, ptTmp);
					if ptTmp.X > 0 then
					begin
						Geom.Free;
						Geom := LineLineIntersect(ptCurr, dirDst30, ptRef, OutDir + C_PI_2);
						refFlg := False;
					end;

					if Geom.GeometryType = gtPoint then
						Ring.AddPoint(Geom.AsPoint);
				end;

				Geom.Free;
			end;

			ptCurr.Free;
		end;
	end;

	if refFlg then
		Ring.AddPoint(ptRef);

	Ring.AddPoint(MAHF.PrjPt);
//Inner side
//===================================================================================
	if Direct and Primary then
	begin
		for I := 0 to N-1 do
		begin
			bFlag := False;
			ptCurr := BasePoints.Items[I];
			dirDst15 := ArcTan2(MAHF.PrjPt.Y - ptCurr.Y, MAHF.PrjPt.X - ptCurr.X) - SplayAngle15 * fTurnSide;

			for J := 0 to N-1 do
			begin
				if J <> I then
				begin
					PrjToLocal(ptCurr, dirDst15, TPoint(BasePoints.Items[J]), ptTmp);
					if ptTmp.Y * fTurnSide < 0 then
					begin
						bFlag := True;
						break;
					end;
				end;
			end;
			if not bFlag then	break;
		end;

		dirDst := dirDst15;
		OutDir2 := dirDst15 + SplayAngle15 * fTurnSide;
		MATF.OutDirection2 := OutDir2;

		LocalToPrj(MAHF.PrjPt, OutDir2, 0, -AreaWidth * fTurnSide, ptRef);

//==
		Ring.AddPoint(ptRef);

		Geom := LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2);
		if Geom.GeometryType = gtPoint then
		begin
			PrjToLocal(MAHF.PrjPt, OutDir2, Geom.AsPoint, ptTmp);
			if ptTmp.X > 0 then
			begin
				Geom.Free;
				Geom := LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2 + C_PI_2);
				Ring.RemoveAt(Ring.Count-1);
			end;

			if Geom.GeometryType = gtPoint then
				Ring.AddPoint(Geom.AsPoint);
		end;

		Geom.Free;
		Ring.AddPoint(ptCurr);
	end
	else
	begin
		if Direct then
			OutDir2 := MATF.OutDirection2
		else
			OutDir2 := OutDir;

		dirDst15 := OutDir2 - SplayAngle15 * fTurnSide;
		dirDst30 := OutDir2 + DivergenceAngle30 * fTurnSide;
		LocalToPrj(MAHF.PrjPt, OutDir2, 0, -AreaWidth * fTurnSide, ptRef);
		c30 := 0;

		for I := 0 to N-1 do
		begin
			bFlag := False;
			ptCurr := BasePoints.Items[I];
			PrjToLocal(ptRef, OutDir2, ptCurr, ptTmp);

			if ptTmp.Y * fTurnSide >0 then
				dirDst := dirDst15
			else
			begin
				dirDst := dirDst30;
				Ist := I;
				Inc(c30);
			end;

			for J := 0 to N-1 do
			begin
				if J <> I then
				begin
					PrjToLocal(ptCurr, dirDst, TPoint(BasePoints.Items[J]), ptCnt);
					if ptCnt.Y * fTurnSide < 0 then
					begin
						bFlag := True;
						break;
					end;
				end;
			end;

			if not bFlag then	break;
		end;

//		if bFlag then
//		begin
//			if bFlag then
			if c30 = 1 then
			begin
				ptCurr := BasePoints.Items[Ist];
				PrjToLocal(ptRef, OutDir2, ptCurr, ptTmp);
				dirDst := dirDst30;
			end;

//		end;
//==
		Ring.AddPoint(ptRef);

		if Abs(ptTmp.Y) > EpsilonDistance then
		begin
			Geom := LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2);
			if Geom.GeometryType = gtPoint then
			begin
				PrjToLocal(MAHF.PrjPt, OutDir2, Geom.AsPoint, ptTmp);
				if ptTmp.X > 0 then
				begin
					Geom.Free;
					Geom := LineLineIntersect(ptCurr, dirDst, ptRef, OutDir2 + C_PI_2);
					Ring.RemoveAt(Ring.Count-1);
				end;

				if Geom.GeometryType = gtPoint then
					Ring.AddPoint(Geom.AsPoint);
			end;
			Geom.Free;
		end;

		Ring.AddPoint(ptCurr);
	end;

	result := TPolygon.Create;
	result.AddRing(Ring);

	ptCnt.Free;
	ptRef.Free;
	ptTmp.Free;
	PrimBasePoints.Free;
	SecBasePoints.Free;
	Ring.Free;
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

function CalcMinRange(FirstFIX, SecondFIX: TWayPoint; NextAngle: Double): Double;
var
	fTurnAngle,
	MinStabNext,
	MinStabCurr:	Double;
begin
//	fTurnAngle := Modulus(NextFIX.OutDirection - FCurrFIX.OutDirection, 2*PI);
	fTurnAngle := Modulus(SecondFIX.OutDirection - SecondFIX.EntryDirection, C_2xPI);

	if fTurnAngle > PI then
		fTurnAngle := 2*PI - fTurnAngle;

	MinStabNext := FirstFIX.CalcInToMinStablizationDistance(fTurnAngle);
	MinStabCurr := SecondFIX.CalcFromMinStablizationDistance(NextAngle);

	Result := MinStabNext + MinStabCurr;// + GPANSOPSConstants.Constant[rnvImMinDist].Value;
end;

function CreateTrackPoints(OrignPT, DestPT: TWayPoint; EntryDir: Double): TMultipoint;
var
	ptFlyBy,
	pt6sec,
	ptTmp, nPt:		TPoint;
	fTmp, r,
	TurnAngle,
	TurnDist,
	PilotTolerance,
	BankTolerance,
	Dist6sec,
	fTurnDirection:	Double;
begin
	result := TMultipoint.Create;

	if (OrignPT is TMATF) and(TMATF(OrignPT).FlyPath = fpCourseToFIX)and
	((TMATF(OrignPT).TurnAt = taMApt)or(OrignPT.FlyMode = fmFlyOver)) then
	begin
		if TMATF(OrignPT).TurnAt = taMApt then
			ptTmp := TMATF(OrignPT).MAPt.PrjPt
		else
			ptTmp := OrignPT.PrjPt;

		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);

		ptTmp := DestPT.PrjPt;
		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);
		exit;
	end;
{
	if OrignPT.FlyMode = fmFlyOver then
	begin
		ptTmp := OrignPT.PrjPt;
		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);

		ptTmp := DestPT.PrjPt;
		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);
		exit;
	end;
}
	fTurnDirection := Integer(OrignPT.TurnDirection);
	r := OrignPT.CalcTurnRadius;

//	TurnAngle := Modulus((OrignPT.EntryDirection - EntryDir)* fTurnDirection, C_2xPI);

	TurnAngle := (OrignPT.EntryDirection - EntryDir)* fTurnDirection;
	fTmp := Modulus(TurnAngle, PI);

	if Abs(TurnAngle) < EpsilonRadian then
	begin
		ptTmp := OrignPT.PrjPt;
		ptTmp.M := OrignPT.EntryDirection;
		result.AddPoint(ptTmp);

		ptTmp := DestPT.PrjPt;
		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);
	end
	else if (fTmp > EpsilonRadian) and (PI - fTmp > EpsilonRadian) then
	begin
		ptFlyBy := LineLineIntersect(OrignPT.PrjPt, OrignPT.EntryDirection, DestPT.PrjPt, EntryDir).AsPoint;

		TurnDist := r/Tan(0.5 * (PI-TurnAngle));
		nPt := LocalToPrj(ptFlyBy, OrignPT.EntryDirection, -TurnDist, 0);
		nPt.M := OrignPT.EntryDirection;

		if PointToLineDistance(nPt, OrignPT.PrjPt, OrignPT.EntryDirection + C_PI_2) < 0 then
		begin
			ptTmp := OrignPT.PrjPt;
			ptTmp.M := OrignPT.EntryDirection;
			result.AddPoint(ptTmp);
		end;

		result.AddPoint(nPt);

		LocalToPrj(ptFlyBy, EntryDir, TurnDist, 0, nPt);
		nPt.M := EntryDir;
		result.AddPoint(nPt);

		if PointToLineDistance(nPt, DestPT.PrjPt, EntryDir + C_PI_2) > 0 then
		begin
			ptTmp := DestPT.PrjPt;
			ptTmp.M := EntryDir;
			result.AddPoint(ptTmp);
		end;

		ptFlyBy.Free;
		nPt.Free;
	end
	else
	begin
		ptTmp := OrignPT.PrjPt;
		ptTmp.M := OrignPT.EntryDirection;
		result.AddPoint(ptTmp);
		pt6sec := TPoint.Create;

		if (OrignPT is TMATF) and (TMATF(OrignPT).TurnAt in [taMApt, taHeight]) then
		begin
			PilotTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
			BankTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
			Dist6sec := (OrignPT.TAS + GPANSOPSConstants.Constant[dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
			LocalToPrj(OrignPT.PrjPt, OrignPT.EntryDirection, Dist6sec, 0, pt6sec);
			pt6sec.M := OrignPT.EntryDirection;
			result.AddPoint(pt6sec);
		end
		else
			pt6sec.Assign(OrignPT.PrjPt);

		nPt := PrjToLocal(pt6sec, OrignPT.EntryDirection, DestPT.PrjPt);

		if nPt.X > 0 then
		begin
			LocalToPrj(pt6sec, OrignPT.EntryDirection, nPt.X, 0, nPt);
			nPt.M := EntryDir;
			result.AddPoint(nPt);
		end
		else
		begin
			LocalToPrj(DestPT.PrjPt, EntryDir, nPt.X, 0, nPt);
			nPt.M := EntryDir;
			result.AddPoint(nPt);
		end;

		ptTmp := DestPT.PrjPt;
		ptTmp.M := EntryDir;
		result.AddPoint(ptTmp);
		pt6sec.Free;
		nPt.Free;
	end;
end;

function SpiralTouchAngleOld (r0, coef, nominalDir, touchDir: Double; turnDir: TSideDirection; AllowBulge: Boolean): Double;
var
	I:				Integer;
	turnAngle,
	TouchAngle,
	rCurr, g, d, delta:	Double;
begin
	TouchAngle := Modulus ((nominalDir - touchDir) * Integer (turnDir), C_2xPI);
	turnAngle := TouchAngle + 20*EpsilonRadian*Integer (turnDir);

	for I := 0 to 9 do
	begin
		rCurr := r0 + coef * turnAngle;
		d := coef / rCurr;

		if AllowBulge then	g := ArcTan2(coef, rCurr)
		else				g := 0.0;

		delta := (turnAngle - TouchAngle - g) / (2.0 - 1.0 / (d * d + 1.0));
		turnAngle := turnAngle - delta;
		If (Abs(delta) < EpsilonRadian) then
			break;
	end;
	result := turnAngle;
	if result < 0.0 then	result := Modulus (result, C_2xPI);
//	result := Modulus (turnAngle, C_2xPI);
end;

function SpiralTouchAngle(r0, coeff, SpStartRadial, StartTouch, EndTouch: Double; turnDir: TSideDirection): Double;
var
	I:				Integer;
	fTurnSide,
	turnAngle,
	EndTouchAppr,
	rEnd, delta:	Double;
begin
	fTurnSide := Integer(turnDir);
	EndTouch := Modulus(EndTouch, C_2xPI);
	turnAngle := Modulus((StartTouch - EndTouch) * fTurnSide, C_2xPI);

	for I := 0 to 9 do
	begin
		rEnd := r0 + coeff*turnAngle;

		EndTouchAppr := Modulus(SpStartRadial - (turnAngle + C_PI_2 - ArcTan2(coeff, rEnd)) * fTurnSide, C_2xPI);
		delta := (EndTouchAppr - EndTouch) * fTurnSide;
		turnAngle := turnAngle + delta;

		If (Abs(delta) < EpsilonRadian) then
			break;
	end;

//	result := turnAngle;
//	if result < 0.0 then	result := Modulus (turnAngle, C_2xPI);
	result := Modulus (turnAngle, C_2xPI);
end;

function SpiralTouchAngleR(r0, coeff, SpStartRadial, StartTouch, EndTouch: Double; turnDir: TSideDirection): Double;
var
	I:				Integer;
	fTurnSide,
	turnAngle,
	turnAngleNew,
	delta:			Double;
begin
	fTurnSide := Integer(turnDir);
	EndTouch := Modulus(EndTouch, C_2xPI);
	turnAngle := Modulus((StartTouch - EndTouch) * fTurnSide, C_2xPI);

//	EndTouch := SpStartRadial - (turnAngle + C_PI_2 - ArcTan2(coeff, REnd))*fTurnSide;
//	(turnAngle + C_PI_2 - ArcTan2(coeff, REnd))*fTurnSide := SpStartRadial - EndTouch;
//	turnAngle + C_PI_2 - ArcTan2(coeff, REnd) := (SpStartRadial - EndTouch)*fTurnSide;
//	turnAngle := (SpStartRadial - EndTouch)*fTurnSide - C_PI_2 + ArcTan2(coeff, REnd);

	for I := 0 to 9 do
	begin
		turnAngleNew := (SpStartRadial - EndTouch) * fTurnSide - C_PI_2 + ArcTan2(coeff, r0 + coeff*turnAngle);

		delta := turnAngleNew - turnAngle;
		turnAngle := turnAngleNew;

		If (Abs(delta) < EpsilonRadian) then
			break;
	end;

//	result := turnAngle;
//	if result < 0.0 then	result := Modulus (turnAngle, C_2xPI);
	result := Modulus (turnAngle, C_2xPI);
end;

function SpiralTouchToPoint(ptCnt: TPoint; r0, coef, EntryDir: Double;
							turnSide: TSideDirection; ptDst: TPoint): Double;
var
	DirToPnt, DistToPnt,
	dX, dY, Phi, R,
	fTmp, dPhi, Xsp, Ysp,
	SpAngle, fTurn:			Double;
	SpStartRadial:			Double;
	I:						Integer;
begin
	result := 9999.0;
	fTurn := Integer(turnSide);
	SpStartRadial := Modulus(EntryDir + (C_PI_2 - ArcTan2(coef, r0)) * fTurn, C_2xPI);

	dX := ptDst.X - ptCnt.X;
	dY := ptDst.Y - ptCnt.Y;

	DirToPnt := ArcTan2(dY, dX);
	DistToPnt := Hypot(dY, dX);

	SpAngle := Modulus((SpStartRadial - DirToPnt) * fTurn, C_2xPI);
	if (SpAngle > PI) and (r0 = 0.0) then    SpAngle := SpAngle - C_2xPI;

	R := r0 + coef * SpAngle;

	if Abs(R - DistToPnt) < EpsilonDistance then
	begin
		result := SpAngle;
		exit;
	end;

	if R > DistToPnt then	exit;

	fTmp := DirToPnt;

	for I := 0 to 30 do
	begin
		Phi := fTmp;
		SpAngle := SpiralTouchAngle(r0, coef, SpStartRadial, EntryDir, Phi, turnSide);

		R := r0 + coef * SpAngle;
		fTmp := SpStartRadial - SpAngle * fTurn;

		Xsp := ptCnt.X + R * Cos(fTmp);
		Ysp := ptCnt.Y + R * Sin(fTmp);

		fTmp := ArcTan2(ptDst.Y - Ysp, ptDst.X - Xsp);
		dPhi := Abs(Phi - fTmp);

		if dPhi < EpsilonRadian then
		begin
			result := SpAngle;
			break;
		end;
	end;
end;

procedure AddSpiralToRing(ptCnt: TPoint; r0, coef, StartRadial, TurnAngle: Double;
							turnSide: TSideDirection; Ring: TRing);
var
	I, N:			Integer;
	dAlpha, R,
	fSide, fTmp:	Double;
	ptCur:			TPoint;
begin
	fSide := Integer(turnSide);

	N := Round(RadToDeg(TurnAngle));
	dAlpha := 0;

	if N > 0 then
	begin
		if N < 5 then		N := 5
		else if N < 10 then	N := 10;
		dAlpha := TurnAngle / N;
	end;

	ptCur := TPoint.Create;

	for I := 0 to N do
	begin
		fTmp := I * dAlpha;
		R := r0 + fTmp * coef;
		LocalToPrj(ptCnt, StartRadial - fTmp * fSide, R, 0, ptCur);
		Ring.AddPoint(ptCur);
	end;
	ptCur.Free;
end;

function CreateNomTrack(PrevLeg, Leg: TLeg): TPolyline;
var
	N:					Integer;
	DivergenceAngle,
	DivergenceAngle30,
	Bank15, fSide,
	OutDir, EntryDir,
	TurnAng, fTmp,
	L1, L2, R1, R2:		Double;

	PtTmp0, PtTmp1,
	PtTmp2, PtTmp3:		TPoint;
	NomTrack:			TPart;
	StartFIX, EndFIX:	TWayPoint;
	TurnDir:			TSideDirection;
begin
	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	Bank15 := GPANSOPSConstants.Constant[rnvFlyOInterBank].Value;

	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := EndFIX.EntryDirection;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	R1 := BankToRadius(StartFIX.Bank, StartFIX.TAS);

	if TurnDir = sideOn then					L1 := 0
	else if StartFIX.FlyMode = fmFlyBy then		L1 := R1 * Sin(TurnAng)
	else										L1 := R1 * Tan(0.5 * TurnAng);

	NomTrack := TPart.Create;

	PtTmp0 := PointAlongPlane(StartFIX.PrjPt, EntryDir - PI, L1);
	PtTmp0.M := EntryDir;
	NomTrack.AddPoint(PtTmp0);				//	Point1

	if TurnDir = sideOn then
	begin
//		GUI.DrawPointWithText(StartFIX.PrjPt, 255, '1-ci');
//		GUI.DrawPointWithText(EndFIX.PrjPt, 255, '2-ci');
	end
	else if StartFIX.FlyMode = fmFlyBy then
	begin
		L1 := R1 * Sin(TurnAng);
		L2 := R1 * Cos(TurnAng) * Tan(DivergenceAngle30);

		LocalToPrj(StartFIX.PrjPt, EntryDir - TurnAng * fSide, L1, 0, PtTmp0);
		PtTmp0.M := EntryDir - TurnAng * fSide;
		NomTrack.AddPoint(PtTmp0);			//	Point2

		PtTmp1 := LocalToPrj(PtTmp0, PtTmp0.M, L2, 0);
		PtTmp1.M := PtTmp0.M;
		NomTrack.AddPoint(PtTmp1);			//	Point3
		PtTmp1.Free;
	end
	else if StartFIX.FlyMode = fmFlyOver then
	begin
		R2 := BankToRadius(Bank15, StartFIX.TAS);

		fTmp := ArcCos((1 + R1 * Cos(TurnAng)/R2)/(1 + R1/R2)) - EpsilonRadian;

//		DivergenceAngle := Min(DivergenceAngle30, 0.5 * TurnAng);

		DivergenceAngle := Min(DivergenceAngle30, fTmp);

		PtTmp0.Assign(StartFIX.PrjPt);
		PtTmp0.M := EntryDir;
		NomTrack.AddPoint(PtTmp0);		//	Point2

		LocalToPrj(StartFIX.PrjPt, EntryDir - 0.5*PI * fSide, R1, 0, PtTmp0);
		fTmp := EntryDir - (TurnAng + DivergenceAngle) * fSide;

		PtTmp1 := PointAlongPlane(PtTmp0, fTmp + 0.5*PI * fSide, R1);
		PtTmp1.M := fTmp;
		NomTrack.AddPoint(PtTmp1);			//	Point3

		PtTmp0.Free;
		PtTmp0 := LineLineIntersect(PtTmp1, PtTmp1.M, EndFIX.PrjPt, OutDir).AsPoint;
		if PtTmp0.GeometryType <> gtPoint then
		begin
			GUI.DrawPointWithText(PtTmp1, 255, '1-ci');
			GUI.DrawPointWithText(EndFIX.PrjPt, 255, '2-ci');
		end;

		fTmp := R2 * Tan(0.5 * DivergenceAngle);

		PtTmp2 := LocalToPrj(PtTmp0, PtTmp1.M + PI, fTmp, 0);
		PtTmp2.M := PtTmp1.M;

		PtTmp3 := LocalToPrj(PtTmp0, OutDir, fTmp, 0);
		PtTmp3.M := OutDir;

		NomTrack.AddPoint(PtTmp2);			//	Point4
		NomTrack.AddPoint(PtTmp3);			//	Point5
		PtTmp1.Free;
		PtTmp2.Free;
		PtTmp3.Free;
	end;

	PtTmp0.Assign(EndFIX.PrjPt);
	PtTmp0.M := OutDir;
	NomTrack.AddPoint(PtTmp0);

	result := ConvertPointsToTrackLIne(NomTrack);

	if Assigned(PrevLeg) and Assigned(PrevLeg.NomTrack) then
	begin
		N := PrevLeg.NomTrack.Part[0].Count;
		PrevLeg.NomTrack.Part[0].Point[N - 1].Assign(result.Part[0].Point[0]);
	end;

	PtTmp0.Free;
	NomTrack.Free;
end;

function CreateNNSecondLine(const PrevLeg, Leg: TLeg): TPolyline;
var
	TurnAng, theta, d,
	fSide, LineLen,
	Dir0, Dir2,
	EntryDir:			Double;

	TurnDir:			TSideDirection;

	FullPoly:			TPolygon;
	FullRing:			TRing;
	NNLinePart:			TPart;
	StartFIX, EndFIX:	TWayPoint;

	PtTmp0,
	PtTmp1, PtTmp2:				TPoint;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	TurnAng := StartFIX.TurnAngle;
	TurnDir := StartFIX.TurnDirection;
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
		FullPoly := GGeoOperators.UnionGeometry(PrevLeg.FullArea, Leg.FullArea).AsPolygon;
		FullRing.Assign(FullPoly.Ring[0]);
		FullPoly.Free;
	end
	else
		FullRing.Assign(Leg.FullArea.Ring[0]);
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

	Result := TPolyline.Create;
	Result.AddPart(NNLinePart);

	NNLinePart.Free;
end;

{ $DEFINE OLDVARIANT}

{$IFDEF OLDVARIANT}

procedure JoinSegments(const Leg: TLeg; LegRing: TRing; ARP: TAHP; IsOuter, IsPrimary: Boolean);
var
	EntryDir, OutDir,
	BaseDir,
	ASW_0F, ASW_0C, ASW_1C,
	SplayAngle15, LegLenght,
	DivergenceAngle30,


	fDistTreshold, DistToEndFIX,
	fSide, fTmp, DistToCenter,
	TransitionDist, DistToLPT,
	Dist0, Dist1, dPhi1, Delta,
	dPhi2, Direction, Dist56000:						Double;

	ptBase,
	ptTmp, ptCur,
	ptCenter,
	ptInter:			TPoint;

	TurnDir:			TSideDirection;

	StartFIX,
	EndFIX:				TWayPoint;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	if TurnDir = SideOn then
		TurnDir := SideLeft;
		
	fSide := (1.0 - 2.0 * Byte(IsOuter)) * Integer(TurnDir);

	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

	ptTmp := TPoint.Create;
	ptCur := TPoint.Create;
	ptCur.Assign(LegRing.Point[LegRing.Count-1]);

	ASW_0C := (1 - 0.5*Byte(IsPrimary))*StartFIX.SemiWidth;
	ASW_1C := (1 - 0.5*Byte(IsPrimary))*EndFIX.SemiWidth;

//=================================================================================
	if (RadToDeg(EndFIX.TurnAngle) <= 10)or((EndFIX.ConstMode <> FAF_)and (RadToDeg(EndFIX.TurnAngle) <= 30)) then
//	if (EndFIX.ConstMode = FAF_)or(RadToDeg(EndFIX.TurnAngle) <= 30) then
	begin
		if (EndFIX.TurnDirection = SideOn)or (EndFIX.TurnDirection <> TSideDirection(Round(fSide))) then
			DistToEndFIX := -OverlapDist
		else
		begin
			LocalToPrj(EndFIX.PrjPt, EndFIX.OutDirection - 0.5 * PI * fSide, ASW_1C, 0, ptTmp);
			DistToEndFIX := PointToLineDistance(ptTmp, EndFIX.PrjPt, OutDir + 0.5 * PI) - OverlapDist;
		end;
	end
	else if EndFIX.TurnDirection = TSideDirection(Round(fSide)) then
		DistToEndFIX := EndFIX.EPT - OverlapDist
	else
		DistToEndFIX := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - OverlapDist;

	ptCenter := LocalToPrj(EndFIX.PrjPt, OutDir + PI, DistToEndFIX, 0);

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCenter, 0, 'ptCenter');
//=================================================================================

	DistToCenter := PointToLineDistance(ptCur, ptCenter, OutDir + 0.5*PI);
	if DistToCenter > EpsilonDistance then
	begin
		LegLenght := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		if (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_) then
		begin
			dPhi1 := ArcTan2(ASW_0C - ASW_1C, LegLenght);
			if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;




			BaseDir := OutDir + dPhi1 * fSide;
			ptBase := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_1C, 0);
		end
		else
		begin

		

			BaseDir := OutDir;
			ptBase := LocalToPrj(ptCenter, OutDir - 0.5 * PI * fSide, ASW_1C, 0);
		end;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCur, 0, 'ptCur-1');

		Delta := -fside * PointToLineDistance(ptCur, ptBase, BaseDir);

		if Abs(Delta)> EpsilonDistance then
		begin
			TransitionDist := -(LegLenght + 100) * 100;

			if (EndFIX.SensorType = stGNSS) and (EndFIX.ConstMode = FAF_) then
				TransitionDist := GPANSOPSConstants.Constant[rnvImMinDist].Value;

			fDistTreshold := PBNTerminalTriggerDistance;

			Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
			Dist1 := Hypot(EndFIX.PrjPt.X - ARP.Prj.X, EndFIX.PrjPt.Y - ARP.Prj.Y);

			if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
			begin
				dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
				dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
				Direction := dPhi2 - dPhi1;
				Dist0 := Dist1 * Cos(Direction) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(Direction)));
				if Dist0 > TransitionDist then	TransitionDist := Dist0;
			end;

			if TransitionDist > 0 then
			begin
				if TransitionDist < DistToEndFIX then TransitionDist := DistToEndFIX;
				if TransitionDist < DistToCenter then
				begin
					ASW_0F := PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir);
					LocalToPrj(EndFIX.PrjPt, OutDir - PI, TransitionDist, -ASW_0F, ptCur);
					LegRing.AddPoint(ptCur);
					DistToCenter := PointToLineDistance(ptCur, ptCenter, OutDir + 0.5*PI);
				end
			end;

			if DistToCenter > EpsilonDistance then
			begin
				if Delta > EpsilonDistance then
					Direction := OutDir + DivergenceAngle30 * fSide
				else
					Direction := OutDir - SplayAngle15 * fSide;

				ptInter := LineLineIntersect(ptCur, Direction, ptBase, BaseDir).AsPoint;
				DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);

				if DistToCenter < 0 then
				begin
					ptInter.Free;
					ptInter := LineLineIntersect(ptCur, Direction, ptCenter, OutDir + 0.5*PI).AsPoint;
					DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);
				end;

				ptCur.Assign(ptInter);
				ptInter.Free;

				LegRing.AddPoint(ptCur);
			end;
		end;
		ptBase.Free;

		if DistToCenter > EpsilonDistance then
		begin
			LocalToPrj(ptCenter, OutDir, 0, -ASW_1C * fSide, ptCur);
			LegRing.AddPoint(ptCur);
		end;
	end;

	ptCenter.Free;
	ptCur.Free;
	PtTmp.Free;
end;

function CreateInnerTurnAreaLT(const PrevLeg: Tleg; var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	OutDir, EntryDir,
	TurnAng, SplayAngle15,
	DivergenceAngle30,


	fSide, fTmp,
	ptDist, ptDir,
	ASW_IN0F, ASW_IN0C:		Double;

	TurnDir:				TSideDirection;
	StartFIX, EndFIX:		TWayPoint;

	ptFrom, ptTo,
	ptTmp:			TPoint;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	if TurnDir = SideOn then TurnDir := SideLeft;
	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

	ptFrom := nil;

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_2_R;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end
		else
		begin
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_R;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end;
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_2_L;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end
		else
		begin
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_L;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end;
	end;

	result := TRing.Create;
	ptTmp := TPoint.Create;

	if not Assigned(ptFrom) then
	begin
		LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT + 10.0, ASW_IN0F * fSide, ptTmp);
		result.AddPoint(ptTmp);

		ptFrom := LocalToPrj(StartFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN0F, 0);
	end;
	result.AddPoint(ptFrom);

	fTmp := ASW_IN0C/Cos(TurnAng);
	ptTo := LocalToPrj(StartFIX.PrjPt, EntryDir - 0.5 * PI * fSide, fTmp, 0);//ASW_IN0C

	ptDist := Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
	ptDir := ArcTan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

	fTmp := Modulus((ptDir - OutDir) * fSide, 2 * PI);
	if fTmp > PI then fTmp := fTmp - 2 * PI;

	if(ptDist > 1.0)and(fTmp > -SplayAngle15)and(fTmp < DivergenceAngle30) then
		result.AddPoint(ptTo);
{
if IsPrimary then
begin
	GUI.DrawPointWithText(ptFrom, 255, 'ptFrom');
	GUI.DrawPointWithText(ptTo, 255, 'ptTo');
	GUI.DrawPointWithText(PtTmp, 0, 'ptTmp');
end;
}
	ptFrom.Free;
	ptTo.Free;
	PtTmp.Free;

	JoinSegments(Leg, Result, ARP, False, IsPrimary);
end;

function CreateInnerTurnArea(var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	fDistTreshold,
	OutDir, EntryDir,
	Dist0, Dist1, Dist2,
	dPhi1, dPhi2, azt0,
	TurnAng,
	Dist56000, Dist3700,
	CurDist, MaxDist, TransitionDist,
	CurWidth, ptInterDist, LPTYDist,

	DivergenceAngle30,
	BaseDir,
	SplayAngle15, LegLenght,
	SplayAngle, fSide,
	ASW_INMax, ASW_IN0C,
	ASW_IN0F, ASW_IN1:	Double;

	TurnDir:			TSideDirection;
	StartFIX, EndFIX:	TWayPoint;

	InnerBasePoint,
	ptTmp, ptBase,
	ptCur, ptCut,
	ptInter:			TPoint;
	ReCalcASW_IN0C:		Boolean;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;
	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);

	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	PtTmp := LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT, 0);
	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_IN0F := StartFIX.ASW_2_L;
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			ASW_IN0F := StartFIX.ASW_L;
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, ASW_IN0F, 0);
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_IN0F := StartFIX.ASW_2_R;
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			ASW_IN0F := StartFIX.ASW_R;
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, ASW_IN0F, 0);
	end;

	ASW_INMax := Max(ASW_IN0C, ASW_IN1);
	result := TRing.Create;
	ptCur := InnerBasePoint;
if GlDRAWFLG then
	GUI.DrawPointWithText(ptCur, 0, 'ptCur-0');

	result.AddPoint(ptCur);
	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);

//MAX DISTANCE =================================================================
	if (EndFIX.ConstMode = FAF_)or(RadToDeg(EndFIX.TurnAngle) <= 30) then
												LPTYDist := - 0.5
	else if EndFIX.TurnDirection = TurnDir then	LPTYDist := EndFIX.EPT - 0.5
	else										LPTYDist := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - 0.5;

	fDistTreshold := PBNTerminalTriggerDistance;
	//if EndFIX.SensorType = stGNSS then		fDistTreshol := GNSSTriggerDistance
	//else if EndFIX.SensorType = stSBAS then	fDistTreshol := SBASTriggerDistance
	//else									fDistTreshol := SBASTriggerDistance + GNSSTriggerDistance;

	Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
	Dist1 := Hypot(ARP.Prj.X - EndFIX.PrjPt.X, ARP.Prj.Y - EndFIX.PrjPt.Y);

	Dist56000 := LPTYDist;
	TransitionDist := LPTYDist;

	if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
	begin
		dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
		dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
		azt0 := dPhi2 - dPhi1;
		Dist56000 := Dist1 * Cos(azt0) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(azt0)));
		TransitionDist := Dist56000;
	end;

	if (StartFIX.ConstMode = IF_) and (EndFIX.ConstMode = FAF_) then
	begin
		Dist3700 := GPANSOPSConstants.Constant[rnvImMinDist].Value;
		TransitionDist := Max(Dist56000, Dist3700);
	end;

	MaxDist := Max(LPTYDist, TransitionDist);

	if CurDist < TransitionDist then TransitionDist := CurDist;

//	if CurDist > MaxDist then
	begin
//==============================================================================
		LegLenght := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		if (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_) then
		begin
			DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

			dPhi1 := ArcTan2(ASW_IN0C - ASW_IN1, LegLenght);
			if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

			BaseDir := OutDir + dPhi1 * fSide;
			ptBase := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN1, 0);
		end
		else
		begin
			BaseDir := OutDir;
			ptBase := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN0C, 0);
		end;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptBase, 0, 'ptBase');


		CurWidth := -fside * PointToLineDistance(ptCur, ptBase, BaseDir);
		if Abs(CurWidth)> EpsilonDistance then
		begin
			if CurWidth > 0 then
				SplayAngle := EntryDir - 0.5 * TurnAng * fSide
			else
			begin
				SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
				SplayAngle := OutDir - SplayAngle15 * fSide;
			end;

			ptInter := LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir).AsPoint;
if GlDRAWFLG then
	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 1');

			ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

			if ptInterDist < TransitionDist then
			begin
				ptCut := LocalToPrj(EndFIX.PrjPt, OutDir + PI, TransitionDist, 0);

				ptInter.Free;
				ptInter := LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * PI).AsPoint;
if GlDRAWFLG then
	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');

				ptCut.Free;
			end;
			Result.AddPoint(ptInter);
			ptInter.Free;
			ptBase.Free;
		end;
	end;

	PtTmp.Free;
	ptCur.Free;

	JoinSegments(Leg, Result, ARP, False, IsPrimary);
end;

function CreateOuterTurnAreaLT(const PrevLeg: Tleg; var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	ptDir,
	fTmp, Dist0, Dist1,
	ptDist,
	EntryDir, OutDir,

	SplayAngle15,
	DivergenceAngle30,
	fSide,

	ASW_OUT0F, ASW_OUT0C:	Double;

	TurnDir:			TSideDirection;

	ptCnt, ptTmp,
	ptFrom, ptTo:		TPoint;

	Geom0, Geom1:		TGeometry;
	tmpRing:			TRing;
	StartFIX, EndFIX:	TWayPoint;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	if TurnDir = SideOn then TurnDir := SideLeft;
	fSide := Integer(TurnDir);

	ptFrom := nil;

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_OUT0C := 0.5* StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_2_R;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end
		else
		begin
			ASW_OUT0C := StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_R;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end;
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_OUT0C := 0.5* StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_2_L;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end
		else
		begin
			ASW_OUT0C := StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_L;
			if Assigned(PrevLeg) then
			begin
				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end;
	end;

	result := TRing.Create;
	ptTmp := TPoint.Create;

	if not Assigned(ptFrom) then
	begin
		LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT + 10.0, -ASW_OUT0F * fSide, ptTmp);
		result.AddPoint(ptTmp);
		ptFrom := LocalToPrj(StartFIX.PrjPt, EntryDir + 0.5 * PI * fSide, ASW_OUT0F, 0);
	end;

	result.AddPoint(ptFrom);


	ptTo := LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0);

	ptDist := Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
	ptDir := ArcTan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

	fTmp := Modulus((OutDir - ptDir) * fSide, 2 * PI);
	if fTmp > PI then fTmp := fTmp - 2 * PI;

	if(ptDist > 1.0)and(fTmp > -SplayAngle15)and(fTmp < DivergenceAngle30) then
	begin
		LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0, ptTmp);

		Geom0 := LineLineIntersect(ptTmp, ptDir + 0.5 * PI, ptFrom, EntryDir + 0.5 * PI);
		Geom1 := LineLineIntersect(ptTmp, ptDir + 0.5 * PI, ptTo, OutDir + 0.5 * PI);
		ptCnt := nil;

		if Geom0.GeometryType = gtPoint then
		begin
			ptCnt := Geom0.AsPoint;
			if Geom1.GeometryType = gtPoint then
			begin
				Dist0 := Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
				Dist1 := Hypot(ptFrom.Y - Geom1.AsPoint.Y, ptFrom.X - Geom1.AsPoint.X);
				if (ASW_OUT0F > ASW_OUT0C)and(Dist1 < Dist0) then	ptCnt.Assign(Geom1.AsPoint);
			end;
		end
		else if Geom1.GeometryType = gtPoint then
			ptCnt := Geom1.AsPoint;

		if Assigned(ptCnt) then
		begin
			tmpRing := CreateArcPrj(PtCnt, ptFrom, ptTo, TurnDir);
			result.AddMultiPoint(tmpRing);
			tmpRing.Free;
		end;

		Geom0.Free;
		Geom1.Free;
	end;
//=============================================================================
	ptFrom.Free;
	ptTo.Free;
	ptTmp.Free;

	JoinSegments(Leg, Result, ARP, True, IsPrimary);
end;

function CreateOuterTurnArea(var Leg: TLeg; WSpeed: Double; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	I, N: 					LongInt;
	FlyMode:				TFlyMode;

	Rv, Bank, coef,
	R, K, V, dAlpha,
	AztEnd1, AztEnd2, azt0,
	SpAngle, fTmp,
	OutDir, EntryDir,
	SpStartDir,
	SpStartRad,
	SpTurnAng,
	SpFromAngle,
	SpToAngle,

	dPhi1, dPhi2,
	dPhi3, dPhi4,

	SplayAngle,
	CurWidth, CurDist,
	Dist0, Dist1,
	MaxDist,
	TransitionDist,
	Dist3700,
	Dist56000,
	LPTYDist,
	fDistTreshold,
	ptInterDist,
	InterSectAngle,
	TurnAng, fSide,
	TurnR, dRad,
	SpAbeamDist,
	SplayAngle15,
	CurrY, PrevY,

	BulgeAngle,
	BaseDir, Delta,
	DivergenceAngle30,
	ASW_OUT0C, ASW_OUT0F,
	ASW_OUTMax, ASW_OUT1:	Double;

	TurnDir:				TSideDirection;

	OuterBasePoint,
	InnerBasePoint,
	InterSectPoint,
	ptTmp, ptLocal,
	ptInter, ptBase,

	ptCut, ptCnt,
	ptCur, ptCur1:			TPoint;

	bFlag, IsMAPt,
	HaveIntersection,
	Splay15,
	HaveSecondSP:			Boolean;
	StartFIX, EndFIX:		TWayPoint;
polygon:				TPolygon;

begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	FlyMode := StartFIX.FlyMode;
	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;
	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	TurnR := BankToRadius(StartFIX.Bank, StartFIX.TAS);

	Rv := 1765.27777777777777777 * Tan(StartFIX.Bank) / (PI * StartFIX.TAS);
	if Rv > 3 then	Rv := 3;
	coef := WSpeed / DegToRad(Rv);

	PtTmp := LocalToPrj(StartFIX.PrjPt, EntryDir - PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			dRad := 0;
			ASW_OUT0F := StartFIX.ASW_2_R;
			ASW_OUT0C := 0.5 * StartFIX.SemiWidth;

			ASW_OUT1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			dRad := StartFIX.ASW_R - StartFIX.ASW_2_R;
			ASW_OUT0F := StartFIX.ASW_R;
			ASW_OUT0C := StartFIX.SemiWidth;

			ASW_OUT1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, StartFIX.ASW_2_L, 0);
		OuterBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, StartFIX.ASW_2_R, 0);
	end
	else
	begin
		if IsPrimary then
		begin
			dRad := 0;
			ASW_OUT0F := StartFIX.ASW_2_L;
			ASW_OUT0C := 0.5 * StartFIX.SemiWidth;

			ASW_OUT1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			dRad := StartFIX.ASW_L - StartFIX.ASW_2_L;
			ASW_OUT0F := StartFIX.ASW_L;
			ASW_OUT0C := StartFIX.SemiWidth;

			ASW_OUT1 := EndFIX.SemiWidth;
		end;

		InnerBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, StartFIX.ASW_2_R, 0);
		OuterBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, StartFIX.ASW_2_L, 0);
	end;

	TurnR := TurnR + dRad;

	ASW_OUTMax := Max(ASW_OUT0C, ASW_OUT1);

	result := TRing.Create;
	ptCur := TPoint.Create;
	ptCur1 := TPoint.Create;

//MAX DISTANCE =================================================================

	if EndFIX.TurnDirection = TurnDir then	LPTYDist := EndFIX.EPT - 0.5
	else									LPTYDist := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - 0.5;

	fDistTreshold := PBNTerminalTriggerDistance;
//	if EndFIX.SensorType = stGNSS then		fDistTreshold := GNSSTriggerDistance
//	else if EndFIX.SensorType = stSBAS then	fDistTreshold := SBASTriggerDistance
//	else									fDistTreshold := SBASTriggerDistance + GNSSTriggerDistance;

	Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
	Dist1 := Hypot(ARP.Prj.X - EndFIX.PrjPt.X, ARP.Prj.Y - EndFIX.PrjPt.Y);

	Dist56000 := LPTYDist;
	TransitionDist := LPTYDist;

	if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
	begin
		dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
		dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
		fTmp := dPhi2 - dPhi1;
		Dist56000 := Dist1 * Cos(fTmp) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(fTmp)));
		TransitionDist := Dist56000;
	end;

	if (StartFIX.ConstMode = IF_) and (EndFIX.ConstMode = FAF_) then
	begin
		Dist3700 := GPANSOPSConstants.Constant[rnvImMinDist].Value;
		TransitionDist := Max(TransitionDist, Dist3700);
	end;

	IsMAPt := (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_);
//=============================================================================
	ptCnt := LocalToPrj(OuterBasePoint, EntryDir - 0.5 * PI * fSide, TurnR - dRad, 0);

	SpStartDir := EntryDir + C_PI_2 * fSide;
	SpStartRad := SpStartDir;
	BulgeAngle := ArcTan2(coef, TurnR)*fSide;

	SpTurnAng := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir, TurnDir);
//	SpTurnAng := SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

	Dist0 := TurnR + SpTurnAng * coef;
	LocalToPrj(ptCnt, SpStartDir - SpTurnAng * fSide, Dist0, 0, PtTmp);
	SpAbeamDist := fSide * PointToLineDistance(PtTmp, StartFIX.PrjPt, OutDir);

	HaveSecondSP := (RadToDeg(TurnAng) >= 105) or((RadToDeg(TurnAng) >= 60)and(SpAbeamDist > ASW_OUT0C));
	SpFromAngle := 0;

	if TurnAng > 0.5 * PI then	AztEnd1 := EntryDir - 0.5 * PI * fSide
	else						AztEnd1 := OutDir;

	if HaveSecondSP then
		AztEnd2 := EntryDir - 0.5 * PI * fSide
	else
		AztEnd2 := AztEnd1;

	SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd1, TurnDir);
//	SpToAngle := SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);

	if  FlyMode = fmFlyBy then
		SpTurnAng := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, EntryDir, TurnDir)
//		SpTurnAng := SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
	else
		SpTurnAng := SpToAngle;

	N := Round(RadToDeg(SpTurnAng));
	if N = 0 then		N := 1
	else if N < 5 then	N := 5
	else if N < 10 then	N := 10;

	dAlpha := SpTurnAng / N;

	for I := 0 to N do
	begin
		R := TurnR + I * dAlpha * coef;
		LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);
		Result.AddPoint(ptCur);
	end;

	if  FlyMode = fmFlyBy then
	begin
		Dist0 := TurnR + SpToAngle * coef;
		LocalToPrj(ptCnt, EntryDir - (SpToAngle - 0.5 * PI) * fSide, Dist0, 0, ptCur1);
		ptInter := LineLineIntersect(ptCur, EntryDir, ptCur1, AztEnd1).AsPoint;

		ptCur.Assign(ptCur1);
		Result.AddPoint(ptInter);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter');

		Result.AddPoint(ptCur);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur, 0, 'ptCur');

		ptInter.Free;
	end;
{
if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 255, sfsSolid);
	Polygon.Free;
end;
}
	ptBase := LocalToPrj(EndFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUTMax, 0);
	BaseDir := OutDir;

	SpStartDir := SpStartDir - SpToAngle * fSide;
	SpFromAngle := SpToAngle;

	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);
	CurWidth := PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
	MaxDist := Max(LPTYDist, TransitionDist);

	if IsMAPt then
	begin
		Dist0 := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		dPhi1 := ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
		if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

		SpAngle := OutDir - dPhi1 * fSide;

		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

		if (CurWidth < ASW_OUTMax) then
			SplayAngle := OutDir + SplayAngle15 * fSide
		else
			SplayAngle := OutDir - DivergenceAngle30 * fSide;

		ptInter := LineLineIntersect(ptCur, SplayAngle, ptBase, SpAngle).AsPoint;
		ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

		MaxDist := Max(MaxDist, ptInterDist);

		BaseDir := SpAngle;
		ASW_OUTMax := fSide * PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
		ptInter.Free;
	end;

//																	fTmp := Modulus((AztEnd2 - AztEnd1) * fSide, 2*PI);
											//	 (HaveSecondSP or ((fTmp > EpsilonRadian) and (fTmp < PI)))then
	if(CurDist - LPTYDist > EpsilonDistance) and (HaveSecondSP or (Modulus(AztEnd2 - AztEnd1, 2*PI)> EpsilonRadian))then
	begin
		if TransitionDist > CurDist then	ASW_OUTMax := ASW_OUT1;

		if CurWidth - ASW_OUTMax > EpsilonDistance then
		begin
			if Modulus(AztEnd2 - AztEnd1, 2*PI)> EpsilonRadian then
//			if (fTmp > EpsilonRadian) and (fTmp < PI) then
			begin
				SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd2, TurnDir);
//				SpToAngle := SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);

				SpTurnAng := SpToAngle - SpFromAngle;
				if SpTurnAng >= 0 then
				begin
					N := Round(RadToDeg(SpTurnAng));
					if N = 0 then 		N := 1
					else if N < 5 then	N := 5
					else if N < 10 then N := 10;

					dAlpha := SpTurnAng / N;
					bFlag := False;

					for I := 0 to N do
					begin
						R := TurnR + (SpFromAngle + I * dAlpha) * coef;
						LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);

						CurrY := PointToLineDistance(ptCur, ptBase, BaseDir);

						if (not bFlag)and (CurrY * fSide >= 0)then bFlag := True;

						if bFlag and (CurrY * fSide <= 0)and(I > 0) then
						begin
							K := -PrevY/(CurrY-PrevY);
							ptCur.X := Result.Point[Result.Count-1].X + K *(ptCur.X - Result.Point[Result.Count-1].X);
							ptCur.Y := Result.Point[Result.Count-1].Y + K *(ptCur.Y - Result.Point[Result.Count-1].Y);
							Result.AddPoint(ptCur);
							break;
						end;
						PrevY := CurrY;
						Result.AddPoint(ptCur);
					end;
					SpStartDir := SpStartDir - SpTurnAng * fSide;
					SpFromAngle := SpToAngle;
				end;
			end;
{
if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 0, sfsForwardDiagonal);
	Polygon.Free;
end;
}
			if HaveSecondSP then
			begin
				LocalToPrj(InnerBasePoint, EntryDir - 0.5 * PI * fSide, TurnR - dRad, 0, ptCnt);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCnt, 0, 'ptCnt');

				R := TurnR + SpFromAngle * coef;
				LocalToPrj(ptCnt, SpStartDir - SpFromAngle * fSide, R, 0, ptCur1);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur1, 0, 'ptCur1');

				Delta := -fside * PointToLineDistance(ptCur1, ptBase, BaseDir);

				if Abs(Delta) > EpsilonDistance then
				begin
					ptInter := LineLineIntersect(ptCur, EntryDir + 0.5 * PI, ptBase, BaseDir).AsPoint;
					fTmp := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter-0');

					if fTmp < 0 then
					begin
						ptInter.Free;
						ptInter := LineLineIntersect(ptCur, EntryDir + 0.5 * PI, EndFIX.PrjPt, OutDir + 0.5*PI).AsPoint;
					end;
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter--1');

					fTmp := PointToLineDistance(ptInter, ptCur, OutDir + 0.5 * PI);
					if fTmp < 0 then
					begin
						ptCur.Assign(ptInter);
						ptInter.Free;
						Result.AddPoint(ptCur);
					end;
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur, 0, 'ptCur');

				end;
			end;
		end
	end;
{
if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 0, sfsBackwardDiagonal);
	Polygon.Free;
end;
}
	CurWidth := PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);

	if IsMAPt then
	begin
//		Dist0 := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
//		dPhi1 := ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
//		if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

//		SpAngle := OutDir - dPhi1 * fSide;
//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

//		if (CurWidth < ASW_OUTMax) then
//			SplayAngle := OutDir + SplayAngle15 * fSide
//		else
//			SplayAngle := OutDir - DivergenceAngle30 * fSide;

//		ptInter := LineLineIntersect(ptCur, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
//		ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

//		TransitionDist := Max(TransitionDist, ptInterDist);
//		if TransitionDist = ptInterDist then
//		begin
//			IsMAPt := True;
//			BaseDir := SpAngle;
//			ASW_OUTMax := fSide * PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
//		end;
	end;

//=============================================================================
	if TransitionDist > CurDist    then
		ASW_OUTMax := ASW_OUT1;

	if (CurDist - LPTYDist > EpsilonDistance)and(Abs(CurWidth - ASW_OUTMax) > EpsilonDistance) then
	begin
		if CurWidth - ASW_OUTMax > EpsilonDistance then
		begin
			SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir-DivergenceAngle30 * fSide, TurnDir);
//			SpToAngle := SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir - DivergenceAngle30 * fSide, TurnDir);

			SpTurnAng := SpToAngle - SpFromAngle;
			if SpTurnAng >= 0 then
			begin
				N := Round(RadToDeg(SpTurnAng));
				if N = 0 then 		N := 1
				else if N < 5 then	N := 5
				else if N < 10 then N := 10;

				dAlpha := SpTurnAng / N;
				bFlag := False;

				for I := 0 to N do
				begin
					R := TurnR + (SpFromAngle + I * dAlpha) * coef;
					LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);
					CurrY := PointToLineDistance(ptCur, ptBase, BaseDir);

					if (not bFlag)and (CurrY * fSide >= 0)then bFlag := True;

					if bFlag and (CurrY * fSide <= 0)and(I > 0) then
					begin
						K := -PrevY/(CurrY-PrevY);
						ptCur.X := Result.Point[Result.Count-1].X + K *(ptCur.X - Result.Point[Result.Count-1].X);
						ptCur.Y := Result.Point[Result.Count-1].Y + K *(ptCur.Y - Result.Point[Result.Count-1].Y);
						Result.AddPoint(ptCur);
						break;
					end;
					PrevY := CurrY;
					Result.AddPoint(ptCur);
				end;
			end;
		end
		else
		begin
			SplayAngle := OutDir + SplayAngle15 * fSide;
			ptInter := LineLineIntersect(ptCur, SplayAngle, ptBase, BaseDir).AsPoint;

			ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

			if ptInterDist < TransitionDist then
			begin
				ptCut := LocalToPrj(EndFIX.PrjPt, OutDir + PI, TransitionDist, 0);
				ptInter.Free;
				ptInter := LineLineIntersect(ptCur, SplayAngle, ptCut, OutDir + 0.5 * PI).AsPoint;
//if GlDRAWFLG then
//		GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');

				ptCut.Free;
			end;
			Result.AddPoint(ptInter);
			ptInter.Free;

		end;
	end;

if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, RGB(0, 0, 255), sfsDiagonalCross);
	Polygon.Free;
end;

	OuterBasePoint.Free;
	InnerBasePoint.Free;

	ptBase.Free;
	ptCur.Free;
	ptCur1.Free;
	ptCnt.Free;
	PtTmp.Free;

	JoinSegments(Leg, Result, ARP, True, IsPrimary);
end;

{$ELSE}

procedure JoinSegments(const Leg: TLeg; LegRing: TRing; ARP: TAHP; IsOuter, IsPrimary: Boolean);
var
	EntryDir, OutDir,
	BaseDir0, BaseDir1,
	ASW_0F, ASW_0C, ASW_1C,
	SplayAngle15, LegLenght,
	DivergenceAngle30,
	SpiralDivergenceAngle,

	fDistTreshold, DistToEndFIX,
	fSide, fTmp, DistToCenter,
	TransitionDist, DistToLPT,
	Dist0, Dist1, dPhi1, Delta,
	dPhi2, Direction, Dist56000:						Double;
	ptBase0, ptBase1,
	ptTmp, ptCurr,
	ptCenter,
	ptInter:			TPoint;
	TurnDirO,
	TurnDir:			TSideDirection;
	JointFlag:			Integer;

	StartFIX,
	EndFIX:				TWayPoint;
	txt:				String;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;
	if IsOuter then
		JointFlag := 1
	else
		JointFlag := 2;

//GlDRAWFLG := IsOuter and IsPrimary and (StartFIX.ConstMode = FAF_);

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	TurnDirO := TurnDir;

	if TurnDir = SideOn then
		TurnDir := StartFIX.EffectiveTurnDirection;

	fSide := (1.0 - 2.0 * Byte(IsOuter)) * Integer(TurnDir);

	SpiralDivergenceAngle := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	if IsPrimary then
	begin
		DivergenceAngle30 := ArcTan(0.5*Tan(SpiralDivergenceAngle));
		fTmp := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		SplayAngle15 := ArcTan(0.5*Tan(fTmp));
	end
	else
	begin
		SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		DivergenceAngle30 := SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	end;

//	SpiralDivergenceAngle := DivergenceAngle30;


	
	ptTmp := TPoint.Create;
	ptCurr := TPoint.Create;
	ptCurr.Assign(LegRing.Point[LegRing.Count-1]);

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCurr, 0, 'ptCurr-0');

	ASW_0C := (1 - 0.5*Byte(IsPrimary))*StartFIX.SemiWidth;
	ASW_1C := (1 - 0.5*Byte(IsPrimary))*EndFIX.SemiWidth;

//	if IsPrimary then	txt := ' Pr'
//	else				txt := '     Se';

//	if IsOuter then		txt := txt + ' Out'
//	else				txt := txt + '      Inn';

//=================================================================================
//	if (RadToDeg(EndFIX.TurnAngle) <= 10)or((EndFIX.ConstMode <> FAF_)and (RadToDeg(EndFIX.TurnAngle) <= 30)) then
//	if (EndFIX.ConstMode = FAF_)or(RadToDeg(EndFIX.TurnAngle) <= 30) then

	if	(RadToDeg(EndFIX.TurnAngle) <= 10) or
		((EndFIX.FlyMode = fmFlyBy) and(EndFIX.ConstMode <> FAF_) and (RadToDeg(EndFIX.TurnAngle) <= 30))  then
	begin
		if (EndFIX.TurnDirection = SideOn)or (EndFIX.TurnDirection <> TSideDirection(Round(fSide))) then
			DistToEndFIX := -OverlapDist
		else
		begin
			LocalToPrj(EndFIX.PrjPt, EndFIX.OutDirection - 0.5 * PI * fSide, ASW_1C, 0, ptTmp);
//if Counter = 5 then

//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptTmp, 0, 'ptTmp' + txt);

			DistToEndFIX := PointToLineDistance(ptTmp, EndFIX.PrjPt, OutDir + 0.5 * PI) - OverlapDist;
		end;
	end
	else if EndFIX.TurnDirection = TSideDirection(Round(fSide)) then
		DistToEndFIX := EndFIX.EPT - OverlapDist
	else
		DistToEndFIX := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - OverlapDist;

	ptCenter := LocalToPrj(EndFIX.PrjPt, OutDir + PI, DistToEndFIX, 0);

//if GlDRAWFLG then
//if GlDRAWFLG then

//	txt := 'ptCenter-'+ IntToStr(Counter) + txt;

//if Counter = 5 then
//if Test then
//	GUI.DrawPointWithText(ptCenter, 255, txt);
//=================================================================================

	DistToCenter := PointToLineDistance(ptCurr, ptCenter, OutDir + 0.5*PI);
	if DistToCenter > EpsilonDistance then
	begin
		LegLenght := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		if (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_) then
		begin
			dPhi1 := ArcTan2(ASW_0C - ASW_1C, LegLenght);
			if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

			BaseDir0 := OutDir + dPhi1 * fSide;
			ptBase0 := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_0C, 0);

			BaseDir1 := OutDir + dPhi1 * fSide;
			ptBase1 := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_1C, 0);
		end
		else
		begin
			BaseDir0 := OutDir;
			ptBase0 := LocalToPrj(ptCenter, OutDir - 0.5 * PI * fSide, ASW_0C, 0);

			BaseDir1 := OutDir;
			ptBase1 := LocalToPrj(ptCenter, OutDir - 0.5 * PI * fSide, ASW_1C, 0);
		end;

//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-1');

if GlDRAWFLG then
	GUI.DrawPointWithText(ptBase1, RGB(0, 0, 255), 'ptBase1');
if GlDRAWFLG then
	GUI.DrawPointWithText(ptBase0, RGB(0, 0, 255), 'ptBase0');

		TransitionDist := -(LegLenght + 100) * 100;

		if (EndFIX.SensorType = stGNSS) and (EndFIX.ConstMode = FAF_) then
			TransitionDist :=  1.5 * (StartFIX.XXT - EndFIX.XXT)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);

		fDistTreshold := PBNTerminalTriggerDistance;

		Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
		Dist1 := Hypot(EndFIX.PrjPt.X - ARP.Prj.X, EndFIX.PrjPt.Y - ARP.Prj.Y);

		if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
		begin
			dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
			dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
			Direction := dPhi2 - dPhi1;
			Dist0 := Dist1 * Cos(Direction) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(Direction)));
			if Dist0 > TransitionDist then	TransitionDist := Dist0;
		end;

		if (TransitionDist > 0) and (TransitionDist < DistToEndFIX) then
			TransitionDist := DistToEndFIX;

//		Delta := -fside * PointToLineDistance(ptCur, ptBase0, BaseDir0);
		ASW_0F := -fside * PointToLineDistance(ptCurr, StartFIX.PrjPt, OutDir);	//Abs ????????
		if IsOuter and (Abs(ASW_0F - ASW_0C) > EpsilonDistance) then
		begin
			Dist0 := (LegLenght + 100) * 100;
			LocalToPrj(ptCenter, OutDir + PI, TransitionDist, fside * ASW_0C, ptTmp);

if GlDRAWFLG then
	GUI.DrawPointWithText(ptTmp, 255, 'ptTmp');

			if TransitionDist > DistToCenter then
			begin
				if ASW_0C > ASW_1C then
					Direction := OutDir + DivergenceAngle30 * fSide
				else
					Direction := OutDir - SplayAngle15 * fSide;

				ptInter := LineLineIntersect(ptTmp, Direction, ptBase1, BaseDir1).AsPoint;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptInter, 255, 'ptInter-M');

				Dist0 := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5*PI);
				ptInter.Free;
			end;

//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptBase0, 255, 'ptBase0-0');

{
			if Dist0 < DistToCenter then
			begin
				ptBase0.Assign(ptTmp);
				BaseDir0 := OutDir + DivergenceAngle30 * fSide;
if GlDRAWFLG then
	GUI.DrawPointWithText(ptBase0, 255, 'ptBase0-1');
			end;
}
			if ASW_0F > ASW_0C then
			begin
				if ((StartFIX.JointFlags  ) <> 0) or (not IsPrimary)then
					Direction := OutDir + SpiralDivergenceAngle * fSide
				else
					Direction := OutDir + DivergenceAngle30 * fSide;

//				Direction := OutDir + SpiralDivergenceAngle * fSide
			end
			else
				Direction := OutDir - SplayAngle15 * fSide;

			ptInter := LineLineIntersect(ptCurr, Direction, ptBase0, BaseDir0).AsPoint;
			DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);

if GlDRAWFLG then
	GUI.DrawPointWithText(ptInter, 255, 'ptInter-1');

			Dist0 := PointToLineDistance(ptInter, ptCurr, OutDir - 0.5 * PI);
			if (DistToCenter < 0) or(Dist0 < 0) then
			begin
				ptInter.Free;
				ptInter := LineLineIntersect(ptCurr, Direction, ptCenter, OutDir + 0.5*PI).AsPoint;
				DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);
			end
			else
				EndFIX.JointFlags := EndFIX.JointFlags and (not JointFlag);

			ptCurr.Assign(ptInter);
			ptInter.Free;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-2');

			LegRing.AddPoint(ptCurr);
			ASW_0F := -fside * PointToLineDistance(ptCurr, StartFIX.PrjPt, OutDir);
		end;

		Delta := -fside * PointToLineDistance(ptCurr, ptBase1, BaseDir1);

		if Abs(Delta)> EpsilonDistance then
		begin
			if TransitionDist > 0 then
			begin
				if TransitionDist < DistToEndFIX then TransitionDist := DistToEndFIX;

				if TransitionDist < DistToCenter then
				begin
					LocalToPrj(EndFIX.PrjPt, OutDir - PI, TransitionDist, fSide * ASW_0F, ptCurr);
if GlDRAWFLG then
	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-3');

					LegRing.AddPoint(ptCurr);

					DistToCenter := PointToLineDistance(ptCurr, ptCenter, OutDir + 0.5*PI);
				end
			end;

			if DistToCenter > EpsilonDistance then
			begin
				if Delta > EpsilonDistance then
					Direction := OutDir + DivergenceAngle30 * fSide
				else
					Direction := OutDir - SplayAngle15 * fSide;

				ptInter := LineLineIntersect(ptCurr, Direction, ptBase1, BaseDir1).AsPoint;
				DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);
				Dist0 := PointToLineDistance(ptInter, ptCurr, OutDir - 0.5 * PI);

				if (DistToCenter < 0) or(Dist0 < 0) then
				//if DistToCenter < 0 then
				begin
if GlDRAWFLG then
	GUI.DrawPointWithText(ptInter, 255, 'ptInter-2');
//					if IsPrimary then
//						Leg.EndFIX.JointFlags := Leg.EndFIX.JointFlags or JointFlag;

					ptInter.Free;
					ptInter := LineLineIntersect(ptCurr, Direction, ptCenter, OutDir + 0.5*PI).AsPoint;
					DistToCenter := PointToLineDistance(ptInter, ptCenter, OutDir + 0.5 * PI);
				end;

				ptCurr.Assign(ptInter);
				ptInter.Free;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-4');

				LegRing.AddPoint(ptCurr);
			end;
		end;
		ptBase0.Free;
		ptBase1.Free;

		if DistToCenter > EpsilonDistance then
		begin
			LocalToPrj(ptCenter, OutDir, 0, -ASW_1C * fSide, ptCurr);

if GlDRAWFLG then
	GUI.DrawPointWithText(ptCurr, 255, 'ptCur-5');

			LegRing.AddPoint(ptCurr);
		end;
	end;

	ptCenter.Free;
	ptCurr.Free;
	PtTmp.Free;
	
//GlDRAWFLG := False;
end;

function CreateInnerTurnAreaLT(const PrevLeg: Tleg; var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	OutDir, EntryDir,
	TurnAng, SplayAngle15,
	DivergenceAngle30,
	SpiralDivergenceAngle,

	fSide, fTmp,
	ptDist, ptDir,
	ASW_IN0F, ASW_IN0C:		Double;

	TurnDir:				TSideDirection;
	StartFIX, EndFIX:		TWayPoint;

	ptFrom, ptTo,
	ptTmp:			TPoint;
begin
//GlDRAWFLG := Counter mod 4 = 1;

	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	if TurnDir = SideOn then
		TurnDir := StartFIX.EffectiveTurnDirection;
	// TurnDir := SideLeft;	//	if TurnDir = SideOn then TurnDir := SideRight;

	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

//	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
//	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;

	SpiralDivergenceAngle := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	if IsPrimary then
	begin
//		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		DivergenceAngle30 := ArcTan(0.5*Tan(SpiralDivergenceAngle));

		fTmp := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		SplayAngle15 := ArcTan(0.5*Tan(fTmp));
	end
	else
	begin
		SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		DivergenceAngle30 := SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
	end;

	ptFrom := nil;

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_2_R;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, OutDir, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end
		else
		begin
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_R;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5 * PI, OutDir, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end;
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_2_L;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, OutDir, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end
		else
		begin
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN0F := StartFIX.ASW_L;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5 * PI, OutDir, fTmp);
				if Assigned(ptFrom) then	ASW_IN0F := fTmp;
			end;
		end;
	end;

//if GlDRAWFLG and Assigned(ptFrom) then
//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-I0');

	result := TRing.Create;
	ptTmp := TPoint.Create;

	if not Assigned(ptFrom) then
	begin
		LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT + 10.0, ASW_IN0F * fSide, ptTmp);
		result.AddPoint(ptTmp);

		ptFrom := LocalToPrj(StartFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN0F, 0);
	end;

if GlDRAWFLG then
	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-I1');

	result.AddPoint(ptFrom);

	fTmp := ASW_IN0C/Cos(TurnAng);
	ptTo := LocalToPrj(StartFIX.PrjPt, EntryDir - 0.5 * PI * fSide, fTmp, 0);//ASW_IN0C

	ptDist := Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
	ptDir := ArcTan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

	fTmp := Modulus((ptDir - OutDir) * fSide, 2 * PI);
	if fTmp > PI then fTmp := fTmp - 2 * PI;

	if(ptDist > 1.0)and(fTmp > -SplayAngle15)and(fTmp < DivergenceAngle30) then
		result.AddPoint(ptTo);
{
if IsPrimary then
begin
	GUI.DrawPointWithText(ptFrom, 255, 'ptFrom');
	GUI.DrawPointWithText(ptTo, 255, 'ptTo');
	GUI.DrawPointWithText(PtTmp, 0, 'ptTmp');
end;
}
	ptFrom.Free;
	ptTo.Free;
	PtTmp.Free;

	JoinSegments(Leg, Result, ARP, False, IsPrimary);
end;

function CreateInnerTurnArea(var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	fDistTreshold,
	OutDir, EntryDir,
	Dist0, Dist1, Dist2,
	dPhi1, dPhi2, azt0,
	TurnAng,
	Dist56000, Dist3700,
	CurDist, MaxDist, TransitionDist,
	CurWidth, ptInterDist, LPTYDist,

	fTmp,
	DivergenceAngle30,
	SpiralDivergenceAngle,
	SplayAngle15,
	BaseDir, LegLenght,
	SplayAngle, fSide,
	ASW_INMax, ASW_IN0C,
	ASW_IN0F, ASW_IN1:	Double;

	TurnDir:			TSideDirection;
	StartFIX, EndFIX:	TWayPoint;

	InnerBasePoint,
	ptTmp, ptBase,
	ptCur, ptCut,
	ptInter:			TPoint;
	ReCalcASW_IN0C:		Boolean;
begin
	SpiralDivergenceAngle := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	if IsPrimary then
	begin
//		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		DivergenceAngle30 := ArcTan(0.5*Tan(SpiralDivergenceAngle));

		fTmp := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		SplayAngle15 := ArcTan(0.5*Tan(fTmp));
	end
	else
	begin
		DivergenceAngle30 := SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	end;

	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;
	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);

	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	PtTmp := LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT, 0);
	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_IN0F := StartFIX.ASW_2_L;
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			ASW_IN0F := StartFIX.ASW_L;
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, ASW_IN0F, 0);
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_IN0F := StartFIX.ASW_2_R;
			ASW_IN0C := 0.5 * StartFIX.SemiWidth;
			ASW_IN1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			ASW_IN0F := StartFIX.ASW_R;
			ASW_IN0C := StartFIX.SemiWidth;
			ASW_IN1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, ASW_IN0F, 0);
	end;

	ASW_INMax := Max(ASW_IN0C, ASW_IN1);
	result := TRing.Create;
	ptCur := InnerBasePoint;
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur, 0, 'ptCur-0');

	result.AddPoint(ptCur);
	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);

//MAX DISTANCE =================================================================
	if (EndFIX.ConstMode = FAF_)or(RadToDeg(EndFIX.TurnAngle) <= 30) then
												LPTYDist := - 0.5
	else if EndFIX.TurnDirection = TurnDir then	LPTYDist := EndFIX.EPT - 0.5
	else										LPTYDist := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - 0.5;

//	if EndFIX.SensorType = stGNSS then		fDistTreshold := GNSSTriggerDistance
//	else									fDistTreshold := SBASTriggerDistance + GNSSTriggerDistance;
	fDistTreshold := PBNTerminalTriggerDistance;

	Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
	Dist1 := Hypot(ARP.Prj.X - EndFIX.PrjPt.X, ARP.Prj.Y - EndFIX.PrjPt.Y);

	Dist56000 := LPTYDist;
	TransitionDist := LPTYDist;

	if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
	begin
		dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
		dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
		azt0 := dPhi2 - dPhi1;
		Dist56000 := Dist1 * Cos(azt0) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(azt0)));
		TransitionDist := Dist56000;
	end;

	if (StartFIX.ConstMode = IF_) and (EndFIX.ConstMode = FAF_) then
	begin
		Dist3700 := 1.5 * (StartFIX.XXT - EndFIX.XXT)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);

//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
//		Dist3700 := GPANSOPSConstants.Constant[rnvImMinDist].Value;
		TransitionDist := Max(Dist56000, Dist3700);
	end;

	MaxDist := Max(LPTYDist, TransitionDist);

	if CurDist < TransitionDist then TransitionDist := CurDist;

//	if CurDist > MaxDist then
	begin
//==============================================================================
		LegLenght := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		if (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_) then
		begin
			dPhi1 := ArcTan2(ASW_IN0C - ASW_IN1, LegLenght);
			if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

			BaseDir := OutDir + dPhi1 * fSide;
			ptBase := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN1, 0);
		end
		else
		begin
			BaseDir := OutDir;
			ptBase := LocalToPrj(EndFIX.PrjPt, OutDir - 0.5 * PI * fSide, ASW_IN0C, 0);
		end;

//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptBase, 0, 'ptBase');


		CurWidth := -fside * PointToLineDistance(ptCur, ptBase, BaseDir);
		if Abs(CurWidth)> EpsilonDistance then
		begin
			if CurWidth > 0 then
				SplayAngle := EntryDir - 0.5 * TurnAng * fSide
			else
			begin
//				SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
				SplayAngle := OutDir - SplayAngle15 * fSide;
			end;

			ptInter := LineLineIntersect(InnerBasePoint, SplayAngle, ptBase, BaseDir).AsPoint;
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 1');

			ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

			if ptInterDist < TransitionDist then
			begin
				ptCut := LocalToPrj(EndFIX.PrjPt, OutDir + PI, TransitionDist, 0);

				ptInter.Free;
				ptInter := LineLineIntersect(InnerBasePoint, SplayAngle, ptCut, OutDir + 0.5 * PI).AsPoint;
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');

				ptCut.Free;
			end;
			Result.AddPoint(ptInter);
			ptInter.Free;
			ptBase.Free;
		end;
	end;

	PtTmp.Free;
	ptCur.Free;

	JoinSegments(Leg, Result, ARP, False, IsPrimary);
end;

function CreateOuterTurnAreaLT(const PrevLeg: Tleg; var Leg: TLeg; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	ptDir,
	fTmp, Dist0, Dist1,
	ptDist,
	EntryDir, OutDir,

	SplayAngle15,
	SpiralDivergenceAngle,
	DivergenceAngle30,
	fSide,

	ASW_OUT0F, ASW_OUT0C:	Double;

	TurnDir:			TSideDirection;

	ptCnt, ptTmp,
	ptFrom, ptTo:		TPoint;

	Geom0, Geom1:		TGeometry;
	tmpRing:			TRing;
	StartFIX, EndFIX:	TWayPoint;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

//	SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
//	DivergenceAngle30 := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	SpiralDivergenceAngle := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	if IsPrimary then
	begin
//		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		DivergenceAngle30 := ArcTan(0.5*Tan(SpiralDivergenceAngle));

		fTmp := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		SplayAngle15 := ArcTan(0.5*Tan(fTmp));
	end
	else
	begin
		DivergenceAngle30 := SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	end;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
//	if TurnDir = SideOn then TurnDir := SideLeft;
	if TurnDir = SideOn then
		TurnDir := StartFIX.EffectiveTurnDirection;

//	if TurnDir = SideOn then TurnDir := SideRight;

	fSide := Integer(TurnDir);

	ptFrom := nil;

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			ASW_OUT0C := 0.5* StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_2_R;
			if Assigned(PrevLeg) then
			begin

//				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5*PI, OutDir, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end
		else
		begin
			ASW_OUT0C := StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_R;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir - 0.5*PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir - 0.5*PI, OutDir, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end;
	end
	else
	begin
		if IsPrimary then
		begin
			ASW_OUT0C := 0.5* StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_2_L;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.PrimaryArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5*PI, OutDir, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end
		else
		begin
			ASW_OUT0C := StartFIX.SemiWidth;
			ASW_OUT0F := StartFIX.ASW_L;
			if Assigned(PrevLeg) then
			begin
//				ptFrom := RingVectorIntersect (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, EntryDir + 0.5*PI, fTmp);
				ptFrom := FindRingLatestPoint (PrevLeg.FullArea.Ring[0], StartFIX.PrjPt, OutDir + 0.5*PI, OutDir, fTmp);
				if Assigned(ptFrom) then ASW_OUT0F := fTmp
			end;
		end;
	end;

//if(not IsPrimary) and Assigned(ptFrom)and Assigned(PrevLeg) then
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-O0');

	result := TRing.Create;

	ptTmp := TPoint.Create;

	if not Assigned(ptFrom) then
	begin
		LocalToPrj(StartFIX.PrjPt, EntryDir - PI, StartFIX.EPT + 10.0, -ASW_OUT0F * fSide, ptTmp);
		result.AddPoint(ptTmp);
		ptFrom := LocalToPrj(StartFIX.PrjPt, EntryDir + 0.5 * PI * fSide, ASW_OUT0F, 0);
	end;

//if(not IsPrimary) and Assigned(ptFrom)and Assigned(PrevLeg) then
if GlDRAWFLG then
	GUI.DrawPointWithText(ptFrom, 0, 'ptFrom-O1');

	result.AddPoint(ptFrom);


	ptTo := LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0);

	ptDist := Hypot(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);
	ptDir := ArcTan2(ptTo.Y - ptFrom.Y, ptTo.X - ptFrom.X);

	fTmp := Modulus((OutDir - ptDir) * fSide, 2 * PI);
	if fTmp > PI then fTmp := fTmp - 2 * PI;

	if(ptDist > 1.0)and(fTmp > -SplayAngle15)and(fTmp < DivergenceAngle30) then
	begin
		LocalToPrj(ptFrom, ptDir, 0.5 * ptDist, 0, ptTmp);

		Geom0 := LineLineIntersect(ptTmp, ptDir + 0.5 * PI, ptFrom, EntryDir + 0.5 * PI);
		Geom1 := LineLineIntersect(ptTmp, ptDir + 0.5 * PI, ptTo, OutDir + 0.5 * PI);
		ptCnt := nil;

		if Geom0.GeometryType = gtPoint then
		begin
			ptCnt := Geom0.AsPoint;
			if Geom1.GeometryType = gtPoint then
			begin
				Dist0 := Hypot(ptFrom.Y - ptCnt.Y, ptFrom.X - ptCnt.X);
				Dist1 := Hypot(ptFrom.Y - Geom1.AsPoint.Y, ptFrom.X - Geom1.AsPoint.X);
				if (ASW_OUT0F > ASW_OUT0C)and(Dist1 < Dist0) then	ptCnt.Assign(Geom1.AsPoint);
			end;
		end
		else if Geom1.GeometryType = gtPoint then
			ptCnt := Geom1.AsPoint;

		if Assigned(ptCnt) then
		begin
			tmpRing := CreateArcPrj(PtCnt, ptFrom, ptTo, TurnDir);
			result.AddMultiPoint(tmpRing);
			tmpRing.Free;
		end;

		Geom0.Free;
		Geom1.Free;
	end;
//=============================================================================
	ptFrom.Free;
	ptTo.Free;
	ptTmp.Free;

	JoinSegments(Leg, Result, ARP, True, IsPrimary);
end;

function CreateOuterTurnArea(var Leg: TLeg; WSpeed: Double; ARP: TAHP; IsPrimary: Boolean): TRing;
var
	I, N: 					LongInt;
	FlyMode:				TFlyMode;

	Rv, Bank, coef,
	R, K, V, dAlpha,
	AztEnd1, AztEnd2, azt0,
	SpAngle, fTmp,
	OutDir, EntryDir,
	SpStartDir,
	SpStartRad,
	SpTurnAng,
	SpFromAngle,
	SpToAngle,

	dPhi1, dPhi2,
	dPhi3, dPhi4,

	SplayAngle,
	CurWidth, CurDist,
	Dist0, Dist1,
	MaxDist,
	TransitionDist,
	Dist3700,
	Dist56000,
	LPTYDist,
	fDistTreshold,
	ptInterDist,
	InterSectAngle,
	TurnAng, fSide,
	TurnR, dRad,
	SpAbeamDist,
	SplayAngle15,
	CurrY,
	PrevX, PrevY,

	BulgeAngle,
	BaseDir, Delta,
	DivergenceAngle30,
	SpiralDivergenceAngle,

	ASW_OUT0C, ASW_OUT0F,
	ASW_OUTMax, ASW_OUT1:	Double;

	TurnDir:				TSideDirection;

	OuterBasePoint,
	InnerBasePoint,
	InterSectPoint,
	ptTmp, ptLocal,
	ptInter, ptBase,

	ptCut, ptCnt,
	ptCur, ptCur1:			TPoint;

	bFlag, IsMAPt,
	HaveIntersection,
	Splay15,
	HaveSecondSP:			Boolean;
	StartFIX, EndFIX:		TWayPoint;

polygon:				TPolygon;
begin
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	FlyMode := StartFIX.FlyMode;
	EntryDir := StartFIX.EntryDirection;
	OutDir := StartFIX.OutDirection;

	SpiralDivergenceAngle := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;

	if IsPrimary then
	begin
		fTmp := GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		DivergenceAngle30 := ArcTan(0.5*Tan(SpiralDivergenceAngle));

		fTmp := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
		SplayAngle15 := ArcTan(0.5*Tan(fTmp));
	end
	else
	begin
		DivergenceAngle30 := SpiralDivergenceAngle;//GPANSOPSConstants.Constant[arSecAreaCutAngl].Value;
		SplayAngle15 := GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	end;

	TurnDir := SideDef(StartFIX.PrjPt, EntryDir, EndFIX.PrjPt);
	fSide := Integer(TurnDir);
	TurnAng := Modulus((EntryDir - OutDir) * fSide, PI);

	TurnR := BankToRadius(StartFIX.Bank, StartFIX.TAS);

	Rv := 1765.27777777777777777 * Tan(StartFIX.Bank) / (PI * StartFIX.TAS);
	if Rv > 3 then	Rv := 3;
	coef := WSpeed / DegToRad(Rv);

	PtTmp := LocalToPrj(StartFIX.PrjPt, EntryDir - PI * BYTE(FlyMode = fmFlyBy), StartFIX.LPT, 0);

	if TurnDir = SideLeft then
	begin
		if IsPrimary then
		begin
			dRad := 0;
			ASW_OUT0F := StartFIX.ASW_2_R;
			ASW_OUT0C := 0.5 * StartFIX.SemiWidth;

			ASW_OUT1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			dRad := StartFIX.ASW_R - StartFIX.ASW_2_R;
			ASW_OUT0F := StartFIX.ASW_R;
			ASW_OUT0C := StartFIX.SemiWidth;

			ASW_OUT1 := EndFIX.SemiWidth;
		end;
		InnerBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, StartFIX.ASW_2_L, 0);
		OuterBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, StartFIX.ASW_2_R, 0);
	end
	else
	begin
		if IsPrimary then
		begin
			dRad := 0;
			ASW_OUT0F := StartFIX.ASW_2_L;
			ASW_OUT0C := 0.5 * StartFIX.SemiWidth;

			ASW_OUT1 := 0.5 * EndFIX.SemiWidth;
		end
		else
		begin
			dRad := StartFIX.ASW_L - StartFIX.ASW_2_L;
			ASW_OUT0F := StartFIX.ASW_L;
			ASW_OUT0C := StartFIX.SemiWidth;

			ASW_OUT1 := EndFIX.SemiWidth;
		end;

		InnerBasePoint := LocalToPrj(PtTmp, EntryDir - 0.5 * PI, StartFIX.ASW_2_R, 0);
		OuterBasePoint := LocalToPrj(PtTmp, EntryDir + 0.5 * PI, StartFIX.ASW_2_L, 0);
	end;

	TurnR := TurnR + dRad;

	ASW_OUTMax := Max(ASW_OUT0C, ASW_OUT1);

	result := TRing.Create;

	ptCur := TPoint.Create;
	ptCur1 := TPoint.Create;

//MAX DISTANCE =================================================================

	if EndFIX.TurnDirection = TurnDir then	LPTYDist := EndFIX.EPT - 0.5
	else									LPTYDist := (2*BYTE(EndFIX.FlyMode = fmFlyBy)-1)*EndFIX.LPT - 0.5;

//	if EndFIX.SensorType = stGNSS then		fDistTreshold := GNSSTriggerDistance
//	else									fDistTreshold := SBASTriggerDistance + GNSSTriggerDistance;
	fDistTreshold := PBNTerminalTriggerDistance;

	Dist0 := Hypot(ARP.Prj.X - StartFIX.PrjPt.X, ARP.Prj.Y - StartFIX.PrjPt.Y);
	Dist1 := Hypot(ARP.Prj.X - EndFIX.PrjPt.X, ARP.Prj.Y - EndFIX.PrjPt.Y);

	Dist56000 := LPTYDist;
	TransitionDist := LPTYDist;

	if (Dist0 >= fDistTreshold)and (Dist1 < fDistTreshold) then
	begin
		dPhi1 := ArcTan2(ARP.Prj.Y - EndFIX.PrjPt.Y, ARP.Prj.X - EndFIX.PrjPt.X);
		dPhi2 := ArcTan2(StartFIX.PrjPt.Y - EndFIX.PrjPt.Y, StartFIX.PrjPt.X - EndFIX.PrjPt.X);
		fTmp := dPhi2 - dPhi1;
		Dist56000 := Dist1 * Cos(fTmp) + Sqrt(Sqr(fDistTreshold) - Sqr(Dist1 * Sin(fTmp)));
		TransitionDist := Dist56000;
	end;

	if (StartFIX.ConstMode = IF_) and (EndFIX.ConstMode = FAF_) then
	begin
		Dist3700 := 1.5 * (StartFIX.XXT - EndFIX.XXT)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);

//		Dist3700 := (StartFIX.SemiWidth - EndFIX.SemiWidth)/Tan(GPANSOPSConstants.Constant[arSecAreaCutAngl].Value);
		//GPANSOPSConstants.Constant[rnvImMinDist].Value;
		TransitionDist := Max(TransitionDist, Dist3700);
	end;

	IsMAPt := (StartFIX.ConstMode = FAF_) and (EndFIX.ConstMode = MAPt_);
//=============================================================================
	ptCnt := LocalToPrj(OuterBasePoint, EntryDir - 0.5 * PI * fSide, TurnR - dRad, 0);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCnt, 0, 'ptCnt-0');

	SpStartDir := EntryDir + C_PI_2 * fSide;
	SpStartRad := SpStartDir;
	BulgeAngle := ArcTan2(coef, TurnR)*fSide;

	SpTurnAng := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir, TurnDir);
//	SpTurnAng := SpiralTouchAngleOld(TurnR, coef, EntryDir, OutDir, TurnDir);

	Dist0 := TurnR + SpTurnAng * coef;
	LocalToPrj(ptCnt, SpStartDir - SpTurnAng * fSide, Dist0, 0, PtTmp);
	SpAbeamDist := fSide * PointToLineDistance(PtTmp, StartFIX.PrjPt, OutDir);

	HaveSecondSP := (RadToDeg(TurnAng) >= 105) or((RadToDeg(TurnAng) >= 60)and(SpAbeamDist > ASW_OUT0C));
	SpFromAngle := 0;

	if TurnAng > 0.5 * PI then	AztEnd1 := EntryDir - 0.5 * PI * fSide
	else						AztEnd1 := OutDir;

	if HaveSecondSP then
		AztEnd2 := EntryDir - 0.5 * PI * fSide
	else
		AztEnd2 := AztEnd1;

	SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd1, TurnDir);
//	SpToAngle := SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd1, TurnDir);

	if  FlyMode = fmFlyBy then
		SpTurnAng := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, EntryDir, TurnDir)
//		SpTurnAng := SpiralTouchAngleOld(TurnR, coef, EntryDir, EntryDir, TurnDir)
	else
		SpTurnAng := SpToAngle;

	N := Round(RadToDeg(SpTurnAng));
	if N = 0 then		N := 1
	else if N < 5 then	N := 5
	else if N < 10 then	N := 10;

	dAlpha := SpTurnAng / N;

	for I := 0 to N do
	begin
		R := TurnR + I * dAlpha * coef;
		LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);
		Result.AddPoint(ptCur);
	end;

	if FlyMode = fmFlyBy then
	begin
		Dist0 := TurnR + SpToAngle * coef;
		LocalToPrj(ptCnt, EntryDir - (SpToAngle - 0.5 * PI) * fSide, Dist0, 0, ptCur1);
		ptInter := LineLineIntersect(ptCur, EntryDir, ptCur1, AztEnd1).AsPoint;

		ptCur.Assign(ptCur1);
		Result.AddPoint(ptInter);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptInter, 0, 'ptInter');

		Result.AddPoint(ptCur);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur, 0, 'ptCur-0');

		ptInter.Free;
	end;

if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 255, sfsSolid);
	Polygon.Free;
end;

	ptBase := LocalToPrj(EndFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUTMax, 0);
	BaseDir := OutDir;

	SpStartDir := SpStartDir - SpToAngle * fSide;
	SpFromAngle := SpToAngle;

	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);
	CurWidth := PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
	MaxDist := Max(LPTYDist, TransitionDist);

	if IsMAPt then
	begin
		Dist0 := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
		dPhi1 := ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
		if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

		SpAngle := OutDir - dPhi1 * fSide;

		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

		if (CurWidth < ASW_OUTMax) then
			SplayAngle := OutDir + SplayAngle15 * fSide
		else
			SplayAngle := OutDir - DivergenceAngle30 * fSide;

		ptInter := LineLineIntersect(ptCur, SplayAngle, ptBase, SpAngle).AsPoint;
		ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

		MaxDist := Max(MaxDist, ptInterDist);

		BaseDir := SpAngle;
		ASW_OUTMax := fSide * PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
		ptInter.Free;
	end;

//																	fTmp := Modulus((AztEnd2 - AztEnd1) * fSide, 2*PI);
											//	 (HaveSecondSP or ((fTmp > EpsilonRadian) and (fTmp < PI)))then
	if(CurDist - LPTYDist > EpsilonDistance) and (HaveSecondSP or (Modulus(AztEnd2 - AztEnd1, 2*PI)> EpsilonRadian))then
	begin
		if TransitionDist > CurDist then	ASW_OUTMax := ASW_OUT1;

		if CurWidth - ASW_OUTMax > EpsilonDistance then
		begin
			if Modulus(AztEnd2 - AztEnd1, 2*PI)> EpsilonRadian then
//			if (fTmp > EpsilonRadian) and (fTmp < PI) then
			begin
				SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, AztEnd2, TurnDir);
//				SpToAngle := SpiralTouchAngleOld(TurnR, coef, EntryDir, AztEnd2, TurnDir);

				SpTurnAng := SpToAngle - SpFromAngle;
				if SpTurnAng >= 0 then
				begin
					N := Round(RadToDeg(SpTurnAng));
					if N = 0 then 		N := 1
					else if N < 5 then	N := 5
					else if N < 10 then N := 10;

					dAlpha := SpTurnAng / N;
					bFlag := False;

					for I := 0 to N do
					begin
						R := TurnR + (SpFromAngle + I * dAlpha) * coef;
						LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);

						CurrY := PointToLineDistance(ptCur, ptBase, BaseDir);

						if (not bFlag)and (CurrY * fSide >= 0)then bFlag := True;

						if bFlag and (CurrY * fSide <= 0)and(I > 0) then
						begin
							K := -PrevY/(CurrY-PrevY);
							ptCur.X := Result.Point[Result.Count-1].X + K *(ptCur.X - Result.Point[Result.Count-1].X);
							ptCur.Y := Result.Point[Result.Count-1].Y + K *(ptCur.Y - Result.Point[Result.Count-1].Y);
							Result.AddPoint(ptCur);
							break;
						end;
						PrevY := CurrY;
						Result.AddPoint(ptCur);
					end;
					SpStartDir := SpStartDir - SpTurnAng * fSide;
					SpFromAngle := SpToAngle;
				end;
			end;

if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 0, sfsForwardDiagonal);
	Polygon.Free;
end;

			if HaveSecondSP then
			begin
				LocalToPrj(InnerBasePoint, EntryDir - 0.5 * PI * fSide, TurnR - dRad, 0, ptCnt);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCnt, 0, 'ptCnt-1');

				R := TurnR + SpFromAngle * coef;
				LocalToPrj(ptCnt, SpStartDir - SpFromAngle * fSide, R, 0, ptCur1);
//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptCur1, 0, 'ptCurSp1');

				Delta := -fside * PointToLineDistance(ptCur1, ptBase, BaseDir);

				fTmp := Modulus (EntryDir + 0.5 * PI - BaseDir, 2 * Pi);

				if (Abs (fTmp) < EpsilonRadian) or (Abs (fTmp - C_2xPi) < EpsilonRadian) or (Abs (fTmp - Pi) < EpsilonRadian) then
				else if Abs(Delta) > EpsilonDistance then
				begin
					ptInter := LineLineIntersect(ptCur, EntryDir + 0.5 * PI, ptBase, BaseDir).AsPoint;
					fTmp := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);
					Dist0 := PointToLineDistance(ptInter, ptCur, OutDir + 0.5 * PI);

if GlDRAWFLG then
begin
	GUI.DrawPointWithText(ptCur, 255, 'ptCur-2');
	if (fTmp > -100000) then
		GUI.DrawPointWithText(ptInter, 255, 'ptInter-0');
	GUI.DrawPointWithText(ptBase, 255, 'ptBase');
end;

					if (fTmp > 0)and(Dist0 < 0) then
					begin
						ptCur.Assign(ptInter);
						Result.AddPoint(ptCur);
					end;

					ptInter.Free;
{
					if fTmp < 0 then
					begin
						ptInter.Free;
						ptInter := LineLineIntersect(ptCur, EntryDir + 0.5 * PI, EndFIX.PrjPt, OutDir + 0.5*PI).AsPoint;
if GlDRAWFLG then
		GUI.DrawPointWithText(ptInter, 255, 'ptInter--1');
if GlDRAWFLG then
		GUI.DrawPointWithText(EndFIX.PrjPt, 255, 'EndFIX.PrjPt');
					end;

					fTmp := PointToLineDistance(ptInter, ptCur, OutDir + 0.5 * PI);
					if false then
//					if fTmp < 0 then
					begin
						ptCur.Assign(ptInter);
						ptInter.Free;
						Result.AddPoint(ptCur);
					end;
if GlDRAWFLG then
	GUI.DrawPointWithText(ptCur, 0, 'ptCur-3');
}
				end;
			end;
		end
	end;

if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, 0, sfsBackwardDiagonal);
	Polygon.Free;
end;

	CurWidth := PointToLineDistance(ptCur, StartFIX.PrjPt, OutDir) * fSide;
	CurDist := PointToLineDistance(ptCur, EndFIX.PrjPt, OutDir + 0.5 * PI);

	if IsMAPt then
	begin
//		Dist0 := Hypot(EndFIX.PrjPt.X - StartFIX.PrjPt.X, EndFIX.PrjPt.Y - StartFIX.PrjPt.Y);
//		dPhi1 := ArcTan2(ASW_OUT0C - ASW_OUT1, Dist0);
//		if dPhi1 > DivergenceAngle30 then dPhi1 := DivergenceAngle30;

//		SpAngle := OutDir - dPhi1 * fSide;
//		LocalToPrj(StartFIX.PrjPt, OutDir + 0.5 * PI * fSide, ASW_OUT0C, 0, ptBase);

//		if (CurWidth < ASW_OUTMax) then
//			SplayAngle := OutDir + SplayAngle15 * fSide
//		else
//			SplayAngle := OutDir - DivergenceAngle30 * fSide;

//		ptInter := LineLineIntersect(ptCur, SplayAngle, InnerBasePoint, SpAngle).AsPoint;
//		ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

//		TransitionDist := Max(TransitionDist, ptInterDist);
//		if TransitionDist = ptInterDist then
//		begin
//			IsMAPt := True;
//			BaseDir := SpAngle;
//			ASW_OUTMax := fSide * PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir);
//		end;
	end;

//=============================================================================
	if TransitionDist > CurDist    then
		ASW_OUTMax := ASW_OUT1;

	if (CurDist - LPTYDist > EpsilonDistance)and(Abs(CurWidth - ASW_OUTMax) > EpsilonDistance) then
	begin
		if CurWidth - ASW_OUTMax > EpsilonDistance then
		begin
			SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir - SpiralDivergenceAngle * fSide, TurnDir);
//			SpToAngle := SpiralTouchAngle(TurnR, coef, SpStartRad, EntryDir + BulgeAngle, OutDir-DivergenceAngle30 * fSide, TurnDir);

			SpTurnAng := SpToAngle - SpFromAngle;
			if SpTurnAng >= 0 then
			begin
				N := Round(RadToDeg(SpTurnAng));
				if N = 0 then 		N := 1
				else if N < 5 then	N := 5
				else if N < 10 then N := 10;

				dAlpha := SpTurnAng / N;
				bFlag := False;

//if GlDRAWFLG then
//	GUI.DrawPointWithText(ptBase, 255, 'ptBase');

				for I := 0 to N do
				begin
					R := TurnR + (SpFromAngle + I * dAlpha) * coef;
					LocalToPrj(ptCnt, SpStartDir - I * dAlpha * fSide, R, 0, ptCur);

					PrjToLocal(ptBase, BaseDir, ptCur, ptTmp);
//					CurrY := PointToLineDistance(ptCur, ptBase, BaseDir);

					if (not bFlag)and (ptTmp.Y * fSide >= 0)then
						bFlag := True;

					if (I > 0) then
					begin
{						if ptTmp.X > 0 then
						begin
							K := -PrevX/(ptTmp.X-PrevX);
							ptCur.X := Result.Point[Result.Count-1].X + K *(ptCur.X - Result.Point[Result.Count-1].X);
							ptCur.Y := Result.Point[Result.Count-1].Y + K *(ptCur.Y - Result.Point[Result.Count-1].Y);
							Result.AddPoint(ptCur);
							break;
						end;
}
						if bFlag and (ptTmp.Y * fSide <= 0) then
						begin
							K := -PrevY/(ptTmp.Y-PrevY);
							ptCur.X := Result.Point[Result.Count-1].X + K *(ptCur.X - Result.Point[Result.Count-1].X);
							ptCur.Y := Result.Point[Result.Count-1].Y + K *(ptCur.Y - Result.Point[Result.Count-1].Y);
							Result.AddPoint(ptCur);
							break;
						end;
					end;

					if (I = N) and IsPrimary then
						Leg.EndFIX.JointFlags := Leg.EndFIX.JointFlags or 1;

					PrevX := ptTmp.X;
					PrevY := ptTmp.Y;
					Result.AddPoint(ptCur);
				end;
			end;
		end
		else
		begin
			SplayAngle := OutDir + SplayAngle15 * fSide;
			ptInter := LineLineIntersect(ptCur, SplayAngle, ptBase, BaseDir).AsPoint;

			ptInterDist := PointToLineDistance(ptInter, EndFIX.PrjPt, OutDir + 0.5 * PI);

			if ptInterDist < TransitionDist then
			begin
				ptCut := LocalToPrj(EndFIX.PrjPt, OutDir + PI, TransitionDist, 0);
				ptInter.Free;
				ptInter := LineLineIntersect(ptCur, SplayAngle, ptCut, OutDir + 0.5 * PI).AsPoint;
//if GlDRAWFLG then
//		GUI.DrawPointWithText(ptInter, 0, 'ptInter - 2');
				ptCut.Free;
			end;
			Result.AddPoint(ptInter);
			ptInter.Free;
		end;
	end;

if GlDRAWFLG then
begin
	Polygon := TPolygon.Create;
	Polygon.AddRing(Result);
	GUI.DrawPolygon(Polygon, RGB(0, 0, 255), sfsDiagonalCross);
	Polygon.Free;
end;

	OuterBasePoint.Free;
	InnerBasePoint.Free;

	ptBase.Free;
	ptCur.Free;
	ptCur1.Free;
	ptCnt.Free;
	PtTmp.Free;

GlDRAWFLG := False;//(EndFIX.ConstMode = IF_) and IsPrimary;
	JoinSegments(Leg, Result, ARP, True, IsPrimary);
GlDRAWFLG := False;

end;
{$ENDIF}

function CreateProtectionArea(Const PrevLeg: TLeg; var Leg: TLeg; ARP: TAHP): Boolean;
var
	WSpeed:						Double;

	OutDir, fSide,
	TurnAngle,
	ASW_OUT1F, 	ASW_IN1F:		Double;

	TurnDir:					TSideDirection;
	ptTmp, ptLPTMeasure,
	ptEPTMeasure:				TPoint;

	InnerRingP,
	OuterRingP,
	InnerRingB,
	OuterRingB:					TRing;

	PrimaryArePolygon,
	BufferPolygon,
	FullArePolygon:				TPolygon;

	StartFIX, EndFIX:			TWayPoint;

	TestPolygon:				TPolygon;
	Test:						Boolean;
begin
	Test := False;

//	if Assigned(PrevLeg) then
//		Leg.StartFIX.JointFlags := PrevLeg.EndFIX.JointFlags;

	WSpeed := GPANSOPSConstants.Constant[dpWind_Speed].Value;
	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	StartFIX.CalcTurnPoints;
	EndFIX.CalcTurnPoints;

	OutDir := EndFIX.EntryDirection;	//StartFIX.OutDirection;//
	TurnAngle := StartFIX.TurnAngle;

	if	(RadToDeg(TurnAngle) <= 10) or
		((StartFIX.FlyMode = fmFlyBy) and(StartFIX.ConstMode <> FAF_) and (RadToDeg(TurnAngle) <= 30))  then
	begin
{		if RadToDeg(TurnAngle) <= 0.1 then
		begin
			OuterRingP := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, True);
			InnerRingP := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, True);

			OuterRingB := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, False);
			InnerRingB := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, False);


			OuterRingP := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, True);
			InnerRingP := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, True);

			OuterRingB := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, False);
			InnerRingB := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, False);
		end
		else}

		begin
			OuterRingP := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, True);
			InnerRingP := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, True);

			OuterRingB := CreateOuterTurnAreaLT(PrevLeg, Leg, ARP, False);
			InnerRingB := CreateInnerTurnAreaLT(PrevLeg, Leg, ARP, False);
		end;

		ptTmp := TPoint.Create;
		if Assigned(PrevLeg) then
		begin
			LocalToPrj(StartFIX.PrjPt, StartFIX.OutDirection, -0.5, 0, ptTmp);
			OuterRingP.InsertPoint(0, ptTmp);	//StartFIX.PrjPt
			OuterRingB.InsertPoint(0, ptTmp);	//StartFIX.PrjPt
		end;

		if (EndFIX.ConstMode <> MAPt_)and( (RadToDeg(EndFIX.TurnAngle) <= 10) or
			((EndFIX.FlyMode = fmFlyBy) and(EndFIX.ConstMode <> FAF_) and (RadToDeg(EndFIX.TurnAngle) <= 30))) then
		begin
			LocalToPrj(EndFIX.PrjPt, OutDir, 0.5, 0, ptTmp);
			OuterRingP.AddPoint(ptTmp);		//EndFIX.PrjPt
			OuterRingB.AddPoint(ptTmp);		//EndFIX.PrjPt
		end;
		ptTmp.Free;
	end
	else
	begin

//GlDRAWFLG := EndFIX.ConstMode = IF_;
		OuterRingP := CreateOuterTurnArea(Leg, WSpeed, ARP, True);
//GlDRAWFLG := False;

		InnerRingP := CreateInnerTurnArea(Leg, ARP, True);

		OuterRingB := CreateOuterTurnArea(Leg, WSpeed, ARP, False);
		InnerRingB := CreateInnerTurnArea(Leg, ARP, False);

		ptTmp := TPoint.Create;
		if (EndFIX.ConstMode <> MAPt_)and( (RadToDeg(EndFIX.TurnAngle) <= 10) or
			((EndFIX.FlyMode = fmFlyBy) and(EndFIX.ConstMode <> FAF_) and (RadToDeg(EndFIX.TurnAngle) <= 30))) then
		begin
			LocalToPrj(EndFIX.PrjPt, OutDir, 0.5, 0, ptTmp);
			OuterRingP.AddPoint(ptTmp);		//EndFIX.PrjPt
			OuterRingB.AddPoint(ptTmp);		//EndFIX.PrjPt
		end;
		ptTmp.Free;
	end;

//if Counter mod 4 = 1 then
if Test then
begin
	TestPolygon := TPolygon.Create;
	TestPolygon.AddRing(OuterRingP);
	GUI.DrawPolygon(TestPolygon, RGB(0, 0, 255), sfsSolid);

	TestPolygon.Clear;
	TestPolygon.AddRing(InnerRingP);
	GUI.DrawPolygon(TestPolygon, 255, sfsSolid);
end;

	OuterRingP.AddMultiPointInverse(InnerRingP);
	OuterRingB.AddMultiPointInverse(InnerRingB);

	PrimaryArePolygon := TPolygon.Create;
	BufferPolygon := TPolygon.Create;

	PrimaryArePolygon.AddRing(OuterRingP);
	BufferPolygon.AddRing(OuterRingB);

	OuterRingP.Free;
	InnerRingP.Free;
	InnerRingB.Free;
	OuterRingB.Free;

//if Counter = 5 then
if False then
begin
	GUI.DrawPolygon(BufferPolygon, RGB(0, 0, 255), sfsSolid);
	GUI.DrawPolygon(PrimaryArePolygon, 255, sfsSolid);
end;

	FullArePolygon := GGeoOperators.UnionGeometry(BufferPolygon, PrimaryArePolygon).AsPolygon;

	Leg.PrimaryArea := PrimaryArePolygon;
	Leg.FullArea := FullArePolygon;

	TurnDir := EndFIX.TurnDirection;
	fSide := Integer(TurnDir);

	if TurnDir = SideOn then
	begin
		ptLPTMeasure := LocalToPrj(EndFIX.PrjPt, OutDir, 0, 0);
		ptEPTMeasure := LocalToPrj(EndFIX.PrjPt, OutDir, 0, 0);
	end
	else
	begin
		ptLPTMeasure := LocalToPrj(EndFIX.PrjPt, OutDir - PI * BYTE(EndFIX.FlyMode = fmFlyBy), EndFIX.LPT, 0);
		ptEPTMeasure := LocalToPrj(EndFIX.PrjPt, OutDir - PI, EndFIX.EPT, 0);
	end;

//====================================================================================================================
	ptTmp := RingVectorIntersect (PrimaryArePolygon.Ring[0], ptEPTMeasure, OutDir - 0.5 * PI * fSide, ASW_IN1F);

//if EndFIX.Role = MAPt_ then
//if Counter = 5 then
//if Test then
if False then
begin
	GUI.DrawPolygon(PrimaryArePolygon, RGB(0,255,0), sfsCross);
	GUI.DrawPointWithText(ptEPTMeasure, 0, 'ptEPTMeasure');
	GUI.DrawPointWithText(ptTmp, 0, 'ptEPTMeasure');
end;

	if Assigned(ptTmp) then
	begin
//GUI.DrawPointWithText(ptTmp, 0, 'ptEPTM' + IntToStr(Counter));
		if TurnDir = SideLeft then	EndFIX.ASW_2_L := ASW_IN1F
		else						EndFIX.ASW_2_R := ASW_IN1F;
		ptTmp.Free;
	end;

	ptTmp := RingVectorIntersect (PrimaryArePolygon.Ring[0], ptLPTMeasure, OutDir + 0.5 * PI * fSide, ASW_OUT1F);
	if Assigned(ptTmp) then
	begin
		if TurnDir = SideLeft then	EndFIX.ASW_2_R := ASW_OUT1F
		else						EndFIX.ASW_2_L := ASW_OUT1F;
		ptTmp.Free;
	end;
//====================================================================================================================
	ptTmp := RingVectorIntersect (FullArePolygon.Ring[0], ptEPTMeasure, OutDir - 0.5 * PI * fSide, ASW_IN1F);
	if Assigned(ptTmp) then
	begin
		if TurnDir = SideLeft then	EndFIX.ASW_L := ASW_IN1F
		else						EndFIX.ASW_R := ASW_IN1F;
		ptTmp.Free;
	end;

	ptTmp := RingVectorIntersect (FullArePolygon.Ring[0], ptLPTMeasure, OutDir + 0.5 * PI * fSide, ASW_OUT1F);
	if Assigned(ptTmp) then
	begin
		if TurnDir = SideLeft then	EndFIX.ASW_R := ASW_OUT1F
		else						EndFIX.ASW_L := ASW_OUT1F;
		ptTmp.Free;
	end;
//====================================================================================================================

	PrimaryArePolygon.Free;
	BufferPolygon.Free;
	FullArePolygon.Free;

	result:= True;
	Inc(Counter);
end;

procedure CreateAssesmentArea(PrevLeg, Leg: TLeg);
var
	TurnAng, theta, d,
	fSide, LineLen,
	Dir0, Dir2,
	EntryDir:			Double;

	TurnDir:			TSideDirection;

	FullPoly, PrimPoly:	TPolygon;
	PrevPrimaryRing,
	PrevFullRing,

	PrimaryRing,
	FullRing:			TRing;

	NNLinePart:			TPart;
	StartFIX, EndFIX:	TWayPoint;

	PtTmp0,
	PtTmp1, PtTmp2:		TPoint;

//test:				Boolean;
begin
//test := False;

	if Assigned(PrevLeg) then
	begin
		FullPoly := GGeoOperators.UnionGeometry(PrevLeg.FullAssesmentArea, Leg.FullAssesmentArea).AsPolygon;
		PrimPoly := GGeoOperators.UnionGeometry(PrevLeg.PrimaryAssesmentArea, Leg.PrimaryAssesmentArea).AsPolygon;
	end
	else
	begin
		FullPoly := GGeoOperators.UnionGeometry(Leg.FullAssesmentArea, Leg.FullAssesmentArea).AsPolygon;
		PrimPoly := GGeoOperators.UnionGeometry(Leg.PrimaryAssesmentArea, Leg.PrimaryAssesmentArea).AsPolygon;
	end;

	StartFIX := Leg.StartFIX;
	EndFIX := Leg.EndFIX;

	EntryDir := StartFIX.EntryDirection;
	TurnAng := StartFIX.TurnAngle;
	TurnDir := StartFIX.TurnDirection;
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

	PtTmp0 := LocalToPrj(PtTmp1, Dir0, LineLen, 0);
	PtTmp2 := LocalToPrj(PtTmp1, Dir2, LineLen, 0);

	NNLinePart := TPart.Create;
	NNLinePart.AddPoint(PtTmp0);
	NNLinePart.AddPoint(PtTmp1);
	NNLinePart.AddPoint(PtTmp2);

	PtTmp0.Free;	PtTmp1.Free;	PtTmp2.Free;

	CutRingByNNLine(PrimPoly.Ring[0], NNLinePart, PrevPrimaryRing, PrimaryRing);
	CutRingByNNLine(FullPoly.Ring[0], NNLinePart, PrevFullRing, FullRing);

{
if test and (EndFIX.Role = MAPt_) then
begin
	GUI.DrawPolygon(FullPoly, 255, sfsSolid);
	GUI.DrawPolygon(PrimPoly, 0, sfsSolid);

	GUI.DrawRing(PrevFullRing, 0, sfsCross);
	GUI.DrawRing(FullRing, 0, sfsCross);
end;
}

	if Assigned(PrevLeg) then
	begin
		if Assigned(PrevFullRing) then
		begin
			FullPoly.Clear;
			FullPoly.AddRing(PrevFullRing);
			PrevLeg.FullAssesmentArea := FullPoly;
		end;

		if Assigned(PrevPrimaryRing) then
		begin
			PrimPoly.Clear;
			PrimPoly.AddRing(PrevPrimaryRing);
			PrevLeg.PrimaryAssesmentArea := PrimPoly;
		end;
	end;

	if Assigned(FullRing) then
	begin
		FullPoly.Clear;
		FullPoly.AddRing(FullRing);
		Leg.FullAssesmentArea := FullPoly;
	end;

	if Assigned(PrimaryRing) then
	begin
		PrimPoly.Clear;
		PrimPoly.AddRing(PrimaryRing);
		Leg.PrimaryAssesmentArea := PrimPoly;
	end;

	PrimPoly.Free;
	FullPoly.Free;
end;

function CalcMAMinOCA(ObstacleList: TList; Gradiend: Double; MinValue: Double; var MaxIx: Integer; AndMask: Integer = -1): Double;
var
	I, N:		Integer;
	ObstacleInfo:	TObstacleInfo;
begin
	result := MinValue;

	N := ObstacleList.Count;
	MaxIx := -1;

	for I := 0 To N - 1 do
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[I]);
		ObstacleInfo.Flags := ObstacleInfo.Flags and AndMask;

		if ObstacleInfo.Dist >= 0 then
			ObstacleInfo.ReqOCA := ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradiend
		else
			ObstacleInfo.ReqOCA := Min(ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradiend,
										ObstacleInfo.Elevation + FlightPhases[fpFinalApproach].MOC*ObstacleInfo.fTmp);

		if result < ObstacleInfo.ReqOCA then
		begin
			MaxIx := I;
			result := ObstacleInfo.ReqOCA;
		end;
	end;

	if MaxIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MaxIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 2;
	end;
end;

function GetObstacles(Polygon: TPolygon; Obstalces: TObstacleList): TList;
var
	I, N:		Integer;
	Obstacle:	TObstacle;
begin
	Result := TList.Create;
	N := Obstalces.Count;

	for I := 0 to N - 1 do
	begin
		Obstacle := Obstalces.Item[I];
		if IsPointInPoly(Obstacle.Prj, Polygon) then
			Result.Add(Obstacle);
	end;
end;

function GetObstacleInfoList(PrimPolygon, SecondPolygon: TPolygon; MOC: Double; Obstalces: TObstacleList): TList;
var
	I, N:			Integer;
	Obstacle:		TObstacle;
	ObstacleInfo:	TObstacleInfo;
	Dist0, Dist1:	Double;
begin
	Result := TList.Create;
	N := Obstalces.Count;

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacle(Obstalces.Item[I]);
		if IsPointInPoly(Obstacle.Prj, SecondPolygon) then
		begin
			ObstacleInfo := TObstacleInfo.Create(Obstacle);
			ObstacleInfo.Flags := Byte(IsPointInPoly(Obstacle.Prj, PrimPolygon));

			ObstacleInfo.fTmp := 1.0;
			if ObstacleInfo.Flags and 1 = 0 then
			begin
				Dist0 := PointToRingDistance(Obstacle.Prj, PrimPolygon.Ring[0]);
				Dist1 := PointToRingDistance(Obstacle.Prj, SecondPolygon.Ring[0]);
				ObstacleInfo.fTmp := Dist1/(Dist0 + Dist1);
			end;

			ObstacleInfo.MOC := MOC*ObstacleInfo.fTmp;
			Result.Add(ObstacleInfo);
		end;
	end;
end;

function GetObstacles(Polygon: TPolygon; ObstacleInfos: TList): TList;
var
	I, N:			Integer;
	ObstacleInfo:	TObstacleInfo;
begin
	Result := TList.Create;
	N := ObstacleInfos.Count;

	for I := 0 to N - 1 do
	begin
		ObstacleInfo := TObstacleInfo(ObstacleInfos.Items[I]);
		if IsPointInPoly(ObstacleInfo.ptPrj, Polygon) then
			Result.Add(ObstacleInfo);
	end;
end;

end.
