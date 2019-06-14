unit AddBranchFUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Buttons, Mask, Grids, StdCtrls, ComCtrls, Parameter,

  IntervalUnit,  UnitConverter, CollectionUnit, Geometry, FIXUnit,
  FlightPhaseUnit, GeometryOperatorsContract, ExtCtrls;

type
	TBranchFrameState = (fsStart, fsInitialized, fsRunned, fsFinalized, fsCancelled);

  TAddBranchFrame = class(TFrame)
    Label1: TLabel;
	Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    gBoxDirection: TGroupBox;
    labCourseUnit: TLabel;
    labAPtType: TLabel;
    rBtnAFromList: TRadioButton;
	rBtnAzimuth: TRadioButton;
    cbDirList: TComboBox;
    editCourse: TEdit;
    udCourse: TUpDown;
    gBoxPosition: TGroupBox;
    labDistanceUnit: TLabel;
    labDPtType: TLabel;
    editDistance: TEdit;
	cbDistList: TComboBox;
    rBtnDistance: TRadioButton;
    rBtnDFromList: TRadioButton;
	gBoxFlight: TGroupBox;
    labIAS: TLabel;
	labVelocityUnit: TLabel;
	labAltitude: TLabel;
    labAltitudeUnit: TLabel;
    labGradient: TLabel;
	labGradientUnit: TLabel;
	editVelocity: TEdit;
	editAltitude: TEdit;
	editGradient: TEdit;
	StringGrid1: TStringGrid;
	cbFlyMode: TComboBox;
	editMaxTurnAngle: TEdit;
	editFIXName: TMaskEdit;
	spBtnPlus: TBitBtn;
	spBtnMinus: TBitBtn;

	prmCourse:		TParameter;
	prmDistance:	TParameter;
	prmIAS:			TParameter;
	prmAltitude:	TParameter;
	prmGradient:	TParameter;
	prmMaxAngle:	TParameter;
	prmName:		TParameter;
	rgSensorType:	TRadioGroup;
    lblTAS: TLabel;
    edtTAS: TEdit;
    lblTASUnit: TLabel;
    prmTAS: TParameter;
    lblPBNTypeIF: TLabel;
    cbPBNType: TComboBox;
    cbMoreDME: TCheckBox;
    spbtnInfo: TSpeedButton;

	procedure rButtonDirectionClick(Sender: TObject);
	procedure cbDirListChange(Sender: TObject);
	procedure rBtnDistanceClick(Sender: TObject);
	procedure cbDistListChange(Sender: TObject);
	procedure spBtnPlusClick(Sender: TObject);
	procedure spBtnMinusClick(Sender: TObject);
	procedure cbFlyModeChange(Sender: TObject);
	procedure editFIXNameExit(Sender: TObject);
	//procedure Button1Click(Sender: TObject);
	procedure OnCourseChangeValue(Sender: TCustomParameter);
	procedure OnChangeDistance(Sender: TCustomParameter);
	procedure prmIASChangeValue(Sender: TCustomParameter);
	procedure prmAltitudeChangeValue(Sender: TCustomParameter);
	procedure prmGradientChangeValue(Sender: TCustomParameter);
	procedure prmMaxAngleChangeValue(Sender: TCustomParameter);
    procedure rgSensorTypeClick(Sender: TObject);
    procedure cbPBNTypeChange(Sender: TObject);
    procedure cbMoreDMEClick(Sender: TObject);
	procedure cbPBNTypeDrawItem(Control: TWinControl; Index: Integer;
      Rect: TRect; State: TOwnerDrawState);
    procedure spbtnInfoClick(Sender: TObject);
  private
	{ Private declarations }
	FN, FLineElem:					Integer;
	runNum:							Integer;
	FFIXName:						string;
	FbNameChanged:					boolean;
{	FCourseParam,
	FDistanceParam,
	FIASParam,
	FAltitudeParam,
	FGradientParam:				TParameter;
}
	FBranchFrameState:			TBranchFrameState;
	FSignificantCollection:		TPandaCollection;
	FARP:						TPoint;
	FSensorType:				TSensorType;
	FFlightProcedure:			TFlightProcedure;
	FLegPoints:					TList;
	FLegs:						TList;
	FCourseSgfPoint:			TPandaCollection;
	FDistanceSgfPoint:			TPandaCollection;

	FMaxDist:					Double;
	FFirstFIX:					TFIXInfo;
	FReferenceFIX,
	FCurrFIX:					TWayPoint;

	function CheckName(NewName: String): Boolean;
	procedure InitNewPoint(Prev: TWayPoint);
	procedure UpDateFIX;
	procedure CalcDistRange;

//	procedure ChangeDirection(CourseParam: TParameter);
//	procedure ChangeDistance(DistanceParam: TParameter);
//	procedure ChangeIAS(IASParam: TParameter);
//	procedure ChangeAltitude(AltitudeParam: TParameter);
//	procedure ChangeGradient(GradientParam: TParameter);
  public
	{ Public declarations }
	procedure Initialize;
	procedure Finalize;
	procedure Execute(SignificantCollection: TPandaCollection;
			ReferenceFIX: TFIXInfo; FlightProcedure: TFlightProcedure; ARP: TPoint; MaxDist: Double);
	function GetResult: TList;
	procedure Cancel;
	procedure Hide;
	property State: TBranchFrameState read FBranchFrameState;
  end;

implementation

{$R *.dfm}
uses
	Math, ARANMath, AIXMTypes, ARANFunctions, ConstantsContract, ARANGlobals,
	ApproachGlobals, Functions, FIXInfoUnit;

procedure TAddBranchFrame.Initialize;
begin
	FCurrFIX := nil;

	FCourseSgfPoint := TPandaCollection.Create;
	FDistanceSgfPoint := TPandaCollection.Create;
	StringGrid1.Cells[0, 0] := 'Num.';
	StringGrid1.Cells[1, 0] := 'Role';
	StringGrid1.Cells[2, 0] := 'Out Course';
	StringGrid1.Cells[3, 0] := 'Dist to next';
	StringGrid1.Cells[4, 0] := 'Altitude';
	StringGrid1.Cells[5, 0] := 'Fly mode';
	FLineElem := -1;
	FBranchFrameState := fsInitialized;
	runNum := 0;
end;

procedure TAddBranchFrame.Finalize;
begin
	GUI.SafeDeleteGraphic(FLineElem);
	if Assigned(FCurrFIX) then
		FCurrFIX.DeleteGraphics;
	FCourseSgfPoint.Free;
	FDistanceSgfPoint.Free;
	FBranchFrameState := fsFinalized;
end;

function TAddBranchFrame.GetResult: TList;
var
	sNumP:		string;
begin
	FCurrFIX.IAS := prmIAS.Value;
	FCurrFIX.Gradient := prmGradient.Value;

	FCurrFIX.MultiCoverage := cbMoreDME.Checked;
	FCurrFIX.FlyMode := TFlyMode(cbFlyMode.ItemIndex);
	FCurrFIX.SensorType := FSensorType;
	FCurrFIX.PBNType := TPBNType(cbPBNType.ItemIndex + cbPBNType.Tag);

	FCurrFIX.EntryDirection := prmCourse.Value;

	FCurrFIX.Name := editFIXName.Text;

	if not CheckName(FCurrFIX.Name) then
	begin
		sNumP := IntTostr(runNum);
		if runNum<10 then
			sNumP := '0' + sNumP;

		MessageDlg('Invalid name.', mtError, [mbOk], 0);
		FCurrFIX.Name := 'IAF' + sNumP
	end;
	FLegPoints.Insert(0, FCurrFIX);
	FCurrFIX.RefreshGraphics;
	result := FLegPoints;	//	result := FLegs;
	FLegPoints := nil;		FLegs := nil;
	FCurrFIX := nil;
	Inc(runNum);
end;

procedure TAddBranchFrame.Cancel;
var
	I:		Integer;
begin
	GUI.SafeDeleteGraphic(FLineElem);
	if Assigned(FCurrFIX) then
		FCurrFIX.Free;

	FCurrFIX := nil;

	if Assigned(FLegPoints) then
	begin
		for I := 0 to FLegPoints.Count - 2 do
			TFIX(FLegPoints.Items[I]).Free;
		FLegPoints.Free;
		FLegPoints := nil;
	end;

	if Assigned(FLegs) then
	begin
		for I := 1 to FLegs.Count - 1 do
		begin
			TLeg(FLegs.Items[I]).StartFIX.Free;
			TLeg(FLegs.Items[I]).EndFIX.Free;
			TLeg(FLegs.Items[I]).Free;
		end;
		FLegs.Free;
		FLegs := nil;
	end;

	FBranchFrameState := fsCancelled;
end;

procedure TAddBranchFrame.Hide;
begin
	GUI.SafeDeleteGraphic(FLineElem);
	if Assigned(FCurrFIX) then
		FCurrFIX.DeleteGraphics;
	FBranchFrameState := fsCancelled;
end;

procedure TAddBranchFrame.Execute(SignificantCollection: TPandaCollection;
			ReferenceFIX: TFIXInfo; FlightProcedure: TFlightProcedure; ARP: TPoint; MaxDist: Double);
var
	Range:			TInterval;
begin
//	FFIXName := 'TP' + IntToStr(runNum) + '00';
//	editFIXName.Text := FFIXName;
//	editFIXName.Hint := FFIXName;
//	FbNameChanged := False;

	FSignificantCollection := SignificantCollection;
	FARP := ARP;

	FMaxDist := MaxDist;
	FFirstFIX := ReferenceFIX;
	FFlightProcedure := FlightProcedure;
	FN := 0;

{
	prmMaxAngle.BeginUpdate;
	prmMaxAngle.MaxValue := GPANSOPSConstants.Constant[rnvIFMaxTurnAngl].Value;	//90;
	prmMaxAngle.EndUpdate;
}

	prmCourse.BeginUpdate;
	prmDistance.BeginUpdate;
	prmIAS.BeginUpdate;

	Range.Left := ReferenceFIX.AngleParameter.Value - GPANSOPSConstants.Constant[rnvIFMaxTurnAngl].Value;
	Range.Right := ReferenceFIX.AngleParameter.Value + GPANSOPSConstants.Constant[rnvIFMaxTurnAngl].Value;

	prmCourse.Range := Range;		//ReferenceFIX.AngleParameter.Range;
	prmCourse.Value := ReferenceFIX.AngleParameter.Value;

	prmCourse.InConvertionPoint := TPoint(ReferenceFIX.AngleParameter.InConvertionPoint).Clone;
	prmCourse.OutConvertionPoint := TPoint(ReferenceFIX.AngleParameter.OutConvertionPoint).Clone;

	Range := ReferenceFIX.DistanceParameter.Range;
	Range.Right := Range.Right*5;

	prmDistance.Range := Range;//ReferenceFIX.DistanceParameter.Range;
	prmDistance.Value := ReferenceFIX.DistanceParameter.Value;

	prmIAS.Range := ReferenceFIX.SpeedParameter.Range;
	prmIAS.Value := ReferenceFIX.SpeedParameter.Value;

	prmAltitude.BeginUpdate;
	prmAltitude.MinValue := ReferenceFIX.AltitudeParameter.Value;
	prmAltitude.MaxValue := prmAltitude.MinValue + prmGradient.Value*prmDistance.Value;
	prmAltitude.Value := prmAltitude.MaxValue;
	prmGradient.BeginUpdate;

{
	if Assigned(ReferenceFIX.GradientParameter) then
	begin
		prmGradient.Range := ReferenceFIX.GradientParameter.Range;
		prmGradient.Value := ReferenceFIX.GradientParameter.Value;
	end
	else
	begin
		prmGradient.Range := ReferenceFIX.GradientParameter.Range;
		prmGradient.Value := ReferenceFIX.GradientParameter.Value;
	end;
}

	prmGradient.Range := FlightPhases[FFlightProcedure].GradientRange;
	prmGradient.Value := prmGradient.MinValue;

	FLegPoints := TList.Create;
	FLegs := TList.Create;
	FFirstFIX.FIX.EntryDirection := FFirstFIX.FIX.OutDirection;		//prmCourse.Value

	InitNewPoint(FFirstFIX.FIX);

//	FFirstFIX.FIX.SensorType := FSensorType;
//	FFirstFIX.FIX.FlyMode := TFlyMode(cbFlyMode.ItemIndex);

	prmAltitude.EndUpdate;
	prmGradient.EndUpdate;

	prmMaxAngle.BeginUpdate;
	prmMaxAngle.MaxValue := GPANSOPSConstants.Constant[rnvIFMaxTurnAngl].Value;	//90;
	prmMaxAngle.EndUpdate;

//	ModalResult := ShowModal;

{	if ModalResult <> mrOK then
	begin
		for I := 0 to FLegPoints.Count - 2 do
			TFIX(FLegPoints.Items[I]).Free;

		FLegPoints.Free;
		FLegPoints := nil;
	end;
	result := FLegPoints;
}
//	FBranchFrameState := fsStart;
	FBranchFrameState := fsRunned;
	rgSensorTypeClick(rgSensorType);
end;

function TAddBranchFrame.CheckName(NewName: String): Boolean;
var
	I, N:	Integer;	//	Leg:	TLeg;
begin
	NewName := UpperCase(NewName);
	N := FLegPoints.Count;
	result := False;

	for I := 0 to N-1 do
		if TFIX(FLegPoints.Items[I]).Name = NewName then
			exit;

	result := True;
end;

procedure TAddBranchFrame.InitNewPoint(Prev: TWayPoint);
var
	dX, dY, Val,
	fDist, fAngle,
	MinVal, MaxVal:		Double;
	SigPoint:			TSignificantPoint;
	I, gr,	//N,
	MinRounded,
	MaxRounded:			Integer;
	sNumP:				string;
	EndFIX:				TWayPoint;
begin
	prmDistance.BeginUpdate;
	prmCourse.BeginUpdate;

	sNumP := IntToStr(FN);
	if FN<10 then
		sNumP := '0'+ sNumP;

	FFIXName := 'TP' + IntToStr(runNum) + sNumP;
	editFIXName.Text := FFIXName;
	editFIXName.Hint := FFIXName;
	FbNameChanged := False;

	prmIAS.BeginUpdate;
//	prmGradient.BeginUpdate;
//	FLegPoints.Add(Prev);

	FLegPoints.Insert(0, Prev);
	EndFIX := FReferenceFIX;
	FReferenceFIX := Prev;

	FCurrFIX := TFIX.Create(IAF_LE_56_, GUI);

	FCurrFIX.SensorType := FSensorType;
	FCurrFIX.FlyMode := TFlyMode(cbFlyMode.ItemIndex);

	FCurrFIX.Bank := FFirstFIX.FIX.Bank;
	FCurrFIX.ISA := FFirstFIX.FIX.ISA;
	FCurrFIX.Altitude := prmAltitude.Value;
	FCurrFIX.OutDirection := Prev.EntryDirection;		//FCourseParam.Value;

	FCurrFIX.IAS := FFirstFIX.FIX.IAS;

	Inc(FN);	//	N := FLegPoints.Count;
	StringGrid1.RowCount := FN + 1;

	if FN > 1 then
	begin
		for I := FN downto 2 do
		begin
			StringGrid1.Cells[0, I] := IntToStr(I);		//StringGrid1.Cells[0, I - 1];
			StringGrid1.Cells[1, I] := StringGrid1.Cells[1, I - 1];
			StringGrid1.Cells[2, I] := StringGrid1.Cells[2, I - 1];
			StringGrid1.Cells[3, I] := StringGrid1.Cells[3, I - 1];
			StringGrid1.Cells[4, I] := StringGrid1.Cells[4, I - 1];
			StringGrid1.Cells[5, I] := StringGrid1.Cells[5, I - 1];
		end;
	end;

	StringGrid1.Cells[0, 1] := '1';//IntToStr(I);
	StringGrid1.Cells[1, 1] := FIXRoleStyleNames[Prev.Role];

	Val := CConverters[puAngle].ConvertFunction(Prev.OutDirection, cdToOuter, prmCourse.OutConvertionPoint);
	StringGrid1.Cells[2, 1] := RoundToStr(Val, GAngleAccuracy);//FormatFloat(GAngleConvertString, Val);

	if FN > 1 then
	begin
		Prev.Role := TP_;
		Prev.RefreshGraphics;

//		fDist := Hypot(TFIX(FLegPoints.Items[N-2]).PrjPt.X - Prev.PrjPt.X,
//						TFIX(FLegPoints.Items[N-2]).PrjPt.Y - Prev.PrjPt.Y);

		fDist := Hypot(EndFIX.PrjPt.X - Prev.PrjPt.X, EndFIX.PrjPt.Y - Prev.PrjPt.Y);

		Val := CConverters[puDistance].ConvertFunction(fDist, cdToOuter, nil);
		StringGrid1.Cells[3, 1] := RoundToStr(Val, GDistanceAccuracy);
		if FN > 2 then
			StringGrid1.Cells[1, 2] := FIXRoleStyleNames[Prev.Role];
	end;

	Val := CConverters[puAltitude].ConvertFunction(Prev.Altitude, cdToOuter, nil);
	StringGrid1.Cells[4, 1] := RoundToStr(Val, GAltitudeAccuracy);
	StringGrid1.Cells[5, 1] := FlyModeNames[Prev.FlyMode];
//=================================================================================

	MinVal := CConverters[puAngle].ConvertFunction(prmCourse.MaxValue, cdToOuter, prmCourse.OutConvertionPoint);
	MaxVal := CConverters[puAngle].ConvertFunction(prmCourse.MinValue, cdToOuter, prmCourse.OutConvertionPoint);

	if MaxVal < MinVal then MaxVal := MaxVal + 360.0;

	MinRounded := Round(MinVal + 0.4999);
	MaxRounded := Round(MaxVal - 0.4999);

	if MinRounded >= MaxRounded then
	begin
		prmCourse.ReadOnly := True;
		if MinRounded = MaxRounded then
			editCourse.Text := IntToStr(MaxRounded)
		else
		begin
			gr := Round(Log10(MaxVal - MinVal) - 0.49999);
			Val := RoundTo(0.5*(MaxVal + MinVal), gr);
			editCourse.Text := FloatToStr(Val);
		end;
		editCourse.Hint := editCourse.Text;
		udCourse.Hint := '';
	end
	else
	begin
		prmCourse.ReadOnly := False;
		udCourse.Min := MinRounded;
		udCourse.Max := MaxRounded;

//		MinRounded := Round(Modulus(MinRounded, 360.0));
//		MaxRounded := Round(Modulus(MaxRounded, 360.0));
//		editCourse.Hint := 'Range: ' + IntToStr(MinRounded) + ' .. ' + IntToStr(MaxRounded);
//		udCourse.Hint := editCourse.Hint;
	end;

//==============================================================================
	FCourseSgfPoint.Clear;
	cbDirList.Clear;

	for I := 0 to FSignificantCollection.Count - 1 do
	begin
		SigPoint := FSignificantCollection.Item [I].asSignificantPoint;
		dX := Prev.PrjPt.X - sigPoint.Prj.X;
		dY := Prev.PrjPt.Y - sigPoint.Prj.Y;
		fDist := Hypot(dX, dY);

		if fDist <= FMaxDist then
		begin
			fAngle := ArcTan2(dY, dX);
			if CheckValue(fAngle, prmCourse.Range, True) = fAngle then
			begin
				FCourseSgfPoint.Add(SigPoint);
				cbDirList.AddItem(SigPoint.AixmID, SigPoint);
			end;
		end
	end;

	rBtnAzimuth.Checked := True;
	if cbDirList.Items.Count = 0 then
	begin
//		cbDirList.Enabled := False;
		rBtnAFromList.Enabled := False;
		labAPtType.Caption := '';
	end
	else
	begin
//		cbDirList.Enabled := rBtnAFromList.Checked;
		rBtnAFromList.Enabled := True;
		cbDirList.ItemIndex := 0;
	end;

//==============================================================================
	prmCourse.EndUpdate;
	prmDistance.EndUpdate;
	prmIAS.EndUpdate;
//	prmGradient.EndUpdate;
	UpDateFIX;
end;

//==============================================================================
procedure TAddBranchFrame.UpdateFIX;
var
	FIXPoint:				TPoint;
	fDist, fDistTreshol:	Double;
begin
	FIXPoint := LocalToPrj(FReferenceFIX.PrjPt, FCurrFIX.OutDirection + PI, prmDistance.Value, 0.0);
	FCurrFIX.PrjPt := FIXPoint;
	FIXPoint.Free;

//	if FSensorType = stGNSS then		fDistTreshol := GNSSTriggerDistance
//	else								fDistTreshol := SBASTriggerDistance + GNSSTriggerDistance;
	fDistTreshol := PBNTerminalTriggerDistance;

	fDist := Hypot(FIXPoint.X - FARP.X, FIXPoint.Y - FARP.Y);
	if fDist <= fDistTreshol then	FCurrFIX.Role := IAF_LE_56_
	else							FCurrFIX.Role := IAF_GT_56_;

	if(rgSensorType.ItemIndex = 0)and(cbPBNType.ItemIndex = 0)and(FCurrFIX.Role = IAF_GT_56_) then
	begin
		cbPBNType.ItemIndex := 1;
		cbPBNTypeChange(cbPBNType);
	end
	else
		FCurrFIX.RefreshGraphics;
end;

procedure TAddBranchFrame.CalcDistRange;
var
	NextFIX:		TWayPoint;
	tmpRange:		TInterval;
	fTurnAngle,
	MinStabNext,
	MinStabCurr,
	fLeft, fRight:	Double;
begin
	NextFIX := FFirstFIX.FIX;
//	NextFIX := TFIX(FLegPoints.Items[FLegPoints.Count - 1]);
//	NextFIX := TFIX(FLegPoints.Items[0]);

	fTurnAngle := Modulus(FReferenceFIX.OutDirection - FReferenceFIX.EntryDirection, C_2xPI);
//	fTurnAngle := Modulus(NextFIX.OutDirection - FCurrFIX.OutDirection, 2*PI);
	if fTurnAngle > PI then fTurnAngle := 2*PI - fTurnAngle;

	MinStabNext := NextFIX.CalcInToMinStablizationDistance(fTurnAngle);
	MinStabCurr := FCurrFIX.CalcFromMinStablizationDistance(prmMaxAngle.Value);

	tmpRange := prmDistance.Range;
	tmpRange.Left := MinStabNext + MinStabCurr;// + GPANSOPSConstants.Constant[rnvImMinDist].Value;

	prmDistance.BeginUpdate;
	prmDistance.Range := tmpRange;
	prmDistance.EndUpdate;

	fLeft := CConverters[puDistance].ConvertFunction(prmDistance.MinValue, cdToOuter, nil);
	fRight := CConverters[puDistance].ConvertFunction(prmDistance.MaxValue, cdToOuter, nil);

	editDistance.Hint := 'Range: ' + RoundToStr(fLeft, GDistanceAccuracy, rtCeil) +
							' .. ' + RoundToStr(fRight, GDistanceAccuracy, rtFloor);
end;
//==============================================================================
procedure TAddBranchFrame.rButtonDirectionClick(Sender: TObject);
begin
//	prmCourse.ReadOnly := (TRadioButton(Sender).Tag <> 0) or not prmCourse.ChekRange;
	cbDirList.Enabled := (TRadioButton(Sender).Tag <> 0) and (cbDirList.Items.Count > 0);
	if cbDirList.Enabled then
		cbDirListChange(cbDirList);
end;

procedure TAddBranchFrame.cbDirListChange(Sender: TObject);
var
	sigPoint:		TSignificantPoint;
	Value:			Double;
begin
	sigPoint := TSignificantPoint(cbDirList.Items.Objects [cbDirList.ItemIndex]);
	labAPtType.Caption := AIXMTypeNames[sigPoint.AIXMType];
	Value := ArcTan2(FReferenceFIX.PrjPt.Y - sigPoint.Prj.Y, FReferenceFIX.PrjPt.X - sigPoint.Prj.X);

	prmCourse.BeginUpdate;
	prmCourse.Value := Value;
	prmCourse.EndUpdate;
end;

procedure TAddBranchFrame.rBtnDistanceClick(Sender: TObject);
begin
//	FDistanceParam.ReadOnly := (TRadioButton(Sender).Tag <> 0) or not prmCourse.ChekRange;
	cbDistList.Enabled := (TRadioButton(Sender).Tag <> 0) and (cbDistList.Items.Count > 0);
	if cbDistList.Enabled then
		cbDistListChange(cbDistList);
end;

procedure TAddBranchFrame.cbDistListChange(Sender: TObject);
var
	sigPoint:		TSignificantPoint;
	Value:			Double;
begin
	sigPoint := TSignificantPoint(cbDistList.Items.Objects [cbDistList.ItemIndex]);
	labDPtType.Caption := AIXMTypeNames[sigPoint.AIXMType];

	Value := Hypot(FReferenceFIX.PrjPt.Y - sigPoint.Prj.Y, FReferenceFIX.PrjPt.X - sigPoint.Prj.X);

//	if sigPoint.Name <> '' then
//		editFIXName.Text := sigPoint.Name;

	prmDistance.BeginUpdate;
	prmDistance.Value := Value;
	prmDistance.EndUpdate;
end;

procedure TAddBranchFrame.cbFlyModeChange(Sender: TObject);
begin
	FCurrFIX.FlyMode := TFlyMode(cbFlyMode.ItemIndex);
	CalcDistRange;
end;

procedure TAddBranchFrame.spBtnPlusClick(Sender: TObject);
var
	Range:			TInterval;
	fLeft, fRight:	Double;
	Leg:			TLeg;
	IAS,
	Gradient:		Double;
	sigPoint:		TSignificantPoint;
//	TmpPolyLine:	TPolyLine;
begin
//	prmCourse.Reset;
//	FDistanceParam.Reset;
//	prmIAS.Reset;
//	prmAltitude.Reset;
//	prmGradient.Reset;
	FCurrFIX.Name := editFIXName.Text;

	if rBtnDFromList.Checked then
	begin
		sigPoint := TSignificantPoint(cbDistList.Items.Objects [cbDistList.ItemIndex]);
		FCurrFIX.ID := sigPoint.ID;
		FCurrFIX.Name := sigPoint.Name;
	end;

	if not CheckName(FCurrFIX.Name) then
	begin
		MessageDlg('Invalid name.', mtError, [mbOk], 0);
		exit;
	end;

{
	prmMaxAngle.BeginUpdate;
	prmMaxAngle.MaxValue := GPANSOPSConstants.Constant[enrMaxTurnAngle].Value;	//120
	prmMaxAngle.EndUpdate;
}
//	sigPoint := TSignificantPoint(cbDistList.Items.Objects [cbDistList.ItemIndex]);
//	FCurrFIX.Name

	IAS := prmIAS.Value;
	Gradient := prmGradient.Value;

	FCurrFIX.IAS := IAS;
	FCurrFIX.Gradient := Gradient;

	FCurrFIX.MultiCoverage := cbMoreDME.Checked;
	FCurrFIX.FlyMode := TFlyMode(cbFlyMode.ItemIndex);
	FCurrFIX.SensorType := FSensorType;
	FCurrFIX.PBNType := TPBNType(cbPBNType.ItemIndex + cbPBNType.Tag);

	FCurrFIX.EntryDirection := prmCourse.Value;

	prmCourse.BeginUpdate;
	prmDistance.BeginUpdate;
	prmIAS.BeginUpdate;
	prmAltitude.BeginUpdate;
	prmGradient.BeginUpdate;

	Range.Left := Modulus(prmCourse.Value - FlightPhases[FFlightProcedure].TurnRange.Right, 2*PI);
	Range.Right := Modulus(prmCourse.Value + FlightPhases[FFlightProcedure].TurnRange.Right, 2*PI);

	prmCourse.Range := Range;

	prmCourse.OutConvertionPoint := FCurrFIX.PrjPt.Clone;
	prmCourse.InConvertionPoint := FCurrFIX.GeoPt.Clone;

	Range.Left := GPANSOPSConstants.Constant[rnvImMinDist].Value;
	Range.Right := 500000.0;

	prmDistance.Range := Range;		//FlightPhases[FFlightProcedure].DistRange;
	prmDistance.Value := 9300.0;	//prmDistance.Range.Right;

	prmIAS.Range := FlightPhases[FFlightProcedure].IASRange;
	prmIAS.Value := prmIAS.MaxValue;

//	Range.Left := FCurrFIX.Altitude;
//	Range.Right := FCurrFIX.Altitude + prmGradient.Value*prmDistance.Value;;
//	prmAltitude.Range := Range;
//	prmAltitude.Value := Range.Left;

	prmAltitude.MinValue := FCurrFIX.Altitude;
	prmAltitude.MaxValue := prmAltitude.MinValue + prmGradient.Value*prmDistance.Value;
	prmAltitude.Value := prmAltitude.MaxValue;

	fLeft := CConverters[puAltitude].ConvertFunction(Range.Left, cdToOuter, nil);
	fRight := CConverters[puAltitude].ConvertFunction(Range.Right, cdToOuter, nil);

	editAltitude.Hint := 'Range: ' + RoundToStr(fLeft, GAltitudeAccuracy, rtCeil) +
							' .. ' + RoundToStr(fRight, GAltitudeAccuracy, rtFloor);

	prmGradient.Range := FlightPhases[FFlightProcedure].GradientRange;
	prmGradient.Value := prmGradient.MinValue;
	prmGradient.EndUpdate;

	spBtnMinus.Enabled := True;


	InitNewPoint(FCurrFIX);

	Leg := TLeg.Create(FCurrFIX, FReferenceFIX, FFlightProcedure, GUI);
	Leg.Gradient := Gradient;
	Leg.IAS := IAS;

	FLegs.Insert(0, Leg);//	FLegs.Add(Leg);

	prmMaxAngle.BeginUpdate;
	prmMaxAngle.MaxValue := GPANSOPSConstants.Constant[enrMaxTurnAngle].Value;	//120
	prmMaxAngle.EndUpdate;
end;

procedure TAddBranchFrame.spBtnMinusClick(Sender: TObject);
begin
//
end;

procedure TAddBranchFrame.editFIXNameExit(Sender: TObject);
//var
//	I, N:	Integer;
begin
//	editFIXName.Width := cbFlyMode.Width;

	if not CheckName(editFIXName.Text) then
		editFIXName.Text := editFIXName.Hint;

//	FbNameChanged := editFIXName.Text <> FFIXName;

	editFIXName.Hint := editFIXName.Text;
end;

{
procedure TAddBranchFrame.Button1Click(Sender: TObject);
begin
	FCurrFIX.EntryDirection := prmCourse.Value;
	FCurrFIX.Name := editFIXName.Text;
	if not CheckName(FCurrFIX.Name) then
	begin
		MessageDlg('Invalid name.', mtError, [mbOk], 0);
//		exit;
		FCurrFIX.Name := 'IAF00'
	end;
	FLegPoints.Insert(0, FCurrFIX);
	FCurrFIX.RefreshGraphics;
end;
}
procedure TAddBranchFrame.OnCourseChangeValue(Sender: TCustomParameter);
var
	fAngle2_0, fAngle50_0,
	MinDist, MaxDist,
//	fTurnAngle,
//	MinStabNext,
//	MinStabCurr,
	fDist, fAngle,
	fDistTreshol1,
	fDistTreshol2:	Double;

	LocalSigPoint:	TPoint;
	SigPoint:		TSignificantPoint;
	I:				Integer;

	BridgeLinePart:	TPart;
	BridgeLine:		TPolyline;
begin
	FCurrFIX.OutDirection := prmCourse.Value;
{
	fAngle := Modulus(FCurrFIX.OutDirection - FCurrFIX.EntryDirection, C_2xPI);
	if Abs(fAngle) < EpsilonRadian then
		FCurrFIX.TurnDirection := SideOn
	else if fAngle > PI then
		FCurrFIX.TurnDirection := SideRight
	else
		FCurrFIX.TurnDirection := SideLeft;
}
	FReferenceFIX.EntryDirection := FCurrFIX.OutDirection;

	fAngle := Modulus(FReferenceFIX.OutDirection - FReferenceFIX.EntryDirection, C_2xPI);
	if Abs(fAngle) < EpsilonRadian then
		FReferenceFIX.TurnDirection := SideOn
	else if fAngle > PI then
		FReferenceFIX.TurnDirection := SideRight
	else
		FReferenceFIX.TurnDirection := SideLeft;

	fAngle2_0 := DegToRad(2.0);
	fAngle50_0 := DegToRad(50.0);

	fDist := FCurrFIX.CalcFromMinStablizationDistance(C_PI_2);
	fDistTreshol1 := FReferenceFIX.CalcInToMinStablizationDistance(fAngle2_0) + fDist;
	fDistTreshol2 := FReferenceFIX.CalcInToMinStablizationDistance(fAngle50_0) + fDist;

	CalcDistRange;

	FDistanceSgfPoint.Clear;
	cbDistList.Clear;

	MaxDist := prmDistance.MaxValue;
	LocalSigPoint := TPoint.Create;

	for I := 0 to FCourseSgfPoint.Count - 1 do
	begin
		SigPoint := FCourseSgfPoint.Item [i].asSignificantPoint;
		PrjToLocal(FReferenceFIX.PrjPt, FCurrFIX.OutDirection + Pi, SigPoint.Prj, LocalSigPoint);

		if(LocalSigPoint.X > 0)and(Abs(LocalSigPoint.Y) <= 0.25*FReferenceFIX.XXT) then
		begin
			fAngle := ArcTan(LocalSigPoint.Y / LocalSigPoint.X);
			if fAngle <= fAngle2_0 then		MinDist := fDistTreshol1
			else							MinDist := fDistTreshol2;

			if (LocalSigPoint.X >= MinDist)and(LocalSigPoint.X <= MaxDist) then
			begin
				FDistanceSgfPoint.Add(SigPoint);
				cbDistList.AddItem(SigPoint.AixmID, SigPoint);
			end
		end
	end;

	LocalSigPoint.Free;

	rBtnDistance.Checked := True;
	if cbDistList.Items.Count = 0 then
	begin
//		cbDistList.Enabled := False;
		rBtnDFromList.Enabled := False;
		labDPtType.Caption := '';
		OnChangeDistance(prmDistance)
	end
	else
	begin
//		cbDistList.Enabled := rBtnDFromList.Checked;
		rBtnDFromList.Enabled := True;
		cbDistList.ItemIndex := 0;
		UpDateFIX;
	end;

	BridgeLinePart := TPart.Create;
	BridgeLinePart.AddPoint(FReferenceFIX.PrjPt);
	BridgeLinePart.AddPoint(FCurrFIX.PrjPt);

	BridgeLine := TPolyline.Create;
	BridgeLine.AddPart(BridgeLinePart);
	BridgeLinePart.Free;

	if FLineElem <> -1 then GUI.DeleteGraphic(FLineElem);	FLineElem := -1;
	FLineElem := GUI.DrawPolyline(BridgeLine, 0, 1);

	BridgeLine.Free;
end;

procedure TAddBranchFrame.OnChangeDistance(Sender: TCustomParameter);
begin
	prmAltitude.BeginUpdate;
	prmAltitude.MaxValue := prmAltitude.MinValue + prmGradient.Value*prmDistance.Value;
	prmAltitude.EndUpdate;

//var
//	Range:				TInterval;
//	fLeft, fRight:		Double;

//	Range := prmAltitude.Range;
//	Range.Right := Range.Left + prmGradient.Value * Sender.Value;	//GPANSOPSConstants.Constant[arIADescent_Max].Value;
//	prmAltitude.Range := Range;
//	fLeft := CConverters[puAltitude].ConvertFunction(Range.Left, cdToOuter, nil);
//	fRight := CConverters[puAltitude].ConvertFunction(Range.Right, cdToOuter, nil);
//	editAltitude.Hint := 'Range: ' + RoundToStr(fLeft, GAltitudeAccuracy, rtCeil) +
//							' .. ' + RoundToStr(fRight, GAltitudeAccuracy, rtFloor);

	UpDateFIX;
end;

procedure TAddBranchFrame.prmIASChangeValue(Sender: TCustomParameter);
begin
	FCurrFIX.IAS := Sender.Value;
	prmTAS.ChangeValue(FCurrFIX.TAS, False);
	CalcDistRange;
end;

procedure TAddBranchFrame.prmAltitudeChangeValue(Sender: TCustomParameter);
begin
	FCurrFIX.Altitude := Sender.Value;
	prmTAS.ChangeValue(FCurrFIX.TAS, False);
	CalcDistRange;
	FCurrFIX.RefreshGraphics;
end;

procedure TAddBranchFrame.prmGradientChangeValue(Sender: TCustomParameter);
begin
	prmAltitude.BeginUpdate;
	prmAltitude.MaxValue := prmAltitude.MinValue + Sender.Value*prmDistance.Value;
	prmAltitude.EndUpdate;
	FCurrFIX.Gradient := Sender.Value;
end;

procedure TAddBranchFrame.prmMaxAngleChangeValue(Sender: TCustomParameter);
begin
	CalcDistRange;
end;

procedure TAddBranchFrame.rgSensorTypeClick(Sender: TObject);
begin
	FSensorType := TSensorType(rgSensorType.ItemIndex);
	cbPBNType.Clear;
	if rgSensorType.ItemIndex = 0 then
	begin
		FCurrFIX.SensorType := stGNSS;
		cbMoreDME.Enabled := False;
		cbPBNType.Tag := 0;
		cbPBNType.Items.Add('RNP APCH');
		cbPBNType.Items.Add('RNAV 1');
		cbPBNType.Items.Add('RNAV 2');
	end
	else
	begin
		FCurrFIX.SensorType := stDME_DME;
		cbMoreDME.Enabled := True;
		cbPBNType.Tag := 1;
		cbPBNType.Items.Add('RNAV 1');
		cbPBNType.Items.Add('RNAV 2');
	end;
	cbPBNType.ItemIndex := 0;
	cbPBNTypeChange(cbPBNType);
end;

procedure TAddBranchFrame.cbPBNTypeChange(Sender: TObject);
begin
	if not Assigned(FCurrFIX) then
		exit;

	if (rgSensorType.ItemIndex = 0) and (FCurrFIX.Role = IAF_GT_56_)and(cbPBNType.ItemIndex = 0) then
	begin
		cbPBNType.ItemIndex := 1
	end
	else
	begin
		FCurrFIX.PBNType := TPBNType(cbPBNType.ItemIndex + cbPBNType.Tag);
	end;

	FCurrFIX.RefreshGraphics;
end;

procedure TAddBranchFrame.cbMoreDMEClick(Sender: TObject);
begin
	FCurrFIX.MultiCoverage := cbMoreDME.Checked;
	FCurrFIX.RefreshGraphics;
end;

procedure TAddBranchFrame.cbPBNTypeDrawItem(Control: TWinControl;
  Index: Integer; Rect: TRect; State: TOwnerDrawState);
var
	Combo:		TComboBox;
	Color:		TColor;
	CY:			Integer;
	str:		string;
begin
	Combo := TComboBox(Control);

	str := Combo.Items[Index];

	CY := Combo.Canvas.TextHeight(str);
	Color := Combo.Canvas.Font.Color;
	Combo.Canvas.FillRect(Rect);

	if Assigned(FCurrFIX)and (Index = 0) and (rgSensorType.ItemIndex = 0) and (FCurrFIX.Role = IAF_GT_56_) then
		Combo.Canvas.Font.Color := RGB(192, 192, 192);

	Combo.Canvas.TextRect(Rect, 2, Rect.Top + ((Rect.Bottom - Rect.Top - CY) div 2), str);
	Combo.Canvas.Font.Color := Color;
end;

procedure TAddBranchFrame.spbtnInfoClick(Sender: TObject);
var
	I:			Integer;
	pt:			Windows.TPoint;
begin
	I := StringGrid1.Selection.Top;
	pt := ClientToScreen(Point(spbtnInfo.Left, spbtnInfo.Top + spbtnInfo.Height));
	ShowFixInfo(pt.X, pt.Y, FLegPoints.Items[I - 1]);
end;

end.

{
640*480 = 307200
640+480 = 1120

sqrt(640*480) = 554.25625842204079
0.5*(640+480) = 560

570*540 = 307800
0.5*(153600+2) = 76801
}
