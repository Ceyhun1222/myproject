unit FIXParameterUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ComCtrls, StdCtrls, Parameter, IntervalUnit, UnitConverter,
  CollectionUnit, Geometry, FIXUnit;

type
  TFIXParameterForm = class(TForm)
	gBoxDirection: TGroupBox;
	labCourseUnit: TLabel;
	labCourse: TLabel;
	rBtnAFromList: TRadioButton;
	rBtnAzimuth: TRadioButton;
	cbDirList: TComboBox;
    editCourse: TEdit;
    udCourse: TUpDown;
	gBoxPosition: TGroupBox;
	labDistance: TLabel;
	labDistanceUnit: TLabel;
    editDistance: TEdit;
	cbDistList: TComboBox;
	rBtnDistance: TRadioButton;
	rBtnDFromList: TRadioButton;
	Button1: TButton;
	Button2: TButton;
	labAPtType: TLabel;
	labDPtType: TLabel;
	gBoxFlight: TGroupBox;
	labIAS: TLabel;
	labVelocityUnit: TLabel;
	labAltitude: TLabel;
	labAltitudeUnit: TLabel;
	editVelocity: TEdit;
	editAltitude: TEdit;
	labGradient: TLabel;
	editGradient: TEdit;
	labGradientUnit: TLabel;
	procedure FormCreate(Sender: TObject);
	procedure FormDestroy(Sender: TObject);
	procedure rButtonDirectionClick(Sender: TObject);
	procedure rBtnDistanceClick(Sender: TObject);

	procedure editCourseExit(Sender: TObject);
	procedure udCourseClick(Sender: TObject; Button: TUDBtnType);
	procedure editDistanceExit(Sender: TObject);
	procedure editVelocityExit(Sender: TObject);
	procedure editAltitudeExit(Sender: TObject);
	procedure editGradientExit(Sender: TObject);
	procedure cbDirListChange(Sender: TObject);
	procedure cbDistListChange(Sender: TObject);
	procedure FormDeactivate(Sender: TObject);
 private
	{ Private declarations }
	FCourseParam,
	FDistanceParam,
	FIASParam,
	FAltitudeParam,
	FGradientParam:				TParameter;

	FCourseSgfPoint:			TPandaCollection;
	FDistanceSgfPoint:			TPandaCollection;

	FFIX, FReferenceFIX:		TFIX;

	procedure ChangeDirection(CourseParam: TParameter);
	procedure ChangeDistance(DistParam: TParameter);
  public
	{ Public declarations }


	procedure SetCourseCurr(Range: TInterval; DefaultValue: Double; PtList: TPandaCollection;
							InConvertionPoint, OutConvertionPoint: TPoint);

	procedure SetDistCurr(Range: TInterval; DefaultValue: Double; PtList: TPandaCollection);
	procedure SetIASCurr(Range: TInterval; DefaultValue: Double);
	procedure SetAltitudeCurr(Range: TInterval; DefaultValue: Double);
	procedure SetGradientCurr(Range: TInterval; DefaultValue: Double);

	procedure SetSignificantPoints(SignificantCollection: TPandaCollection; CurrFIX, ReferenceFIX: TFIX; MaxDist: Double);

	function Execute: Integer;

	property CourseParam: TParameter read FCourseParam;
	property DistanceParam: TParameter read FDistanceParam;
	property IASParam: TParameter read FIASParam;
	property AltitudeParam:	TParameter read FAltitudeParam;
	property GradientParam:	TParameter read FGradientParam;
  end;

var
  FIXParameterForm: TFIXParameterForm;

implementation

{$R *.dfm}
uses
	Math, AIXMTypes, ARANFunctions;

procedure TFIXParameterForm.FormCreate(Sender: TObject);
begin
	FCourseParam := TParameter.Create(puAngle);
	FCourseParam.DescriptionControl := labCourse;
	FCourseParam.ValueControl := editCourse;
	FCourseParam.UnitControl := labCourseUnit;
	FCourseParam.ChangerControl := udCourse;
	FCourseParam.OnChangeValue := ChangeDirection;

	FDistanceParam := TParameter.Create(puDistance);
	FDistanceParam.DescriptionControl := labDistance;
	FDistanceParam.ValueControl := editDistance;
	FDistanceParam.UnitControl := labDistanceUnit;
	FDistanceParam.OnChangeValue := ChangeDistance;

	FIASParam := TParameter.Create(puHSpeed);
	FIASParam.DescriptionControl := labIAS;
	FIASParam.ValueControl := editVelocity;
	FIASParam.UnitControl := labVelocityUnit;

	FAltitudeParam := TParameter.Create(puAltitude);
	FAltitudeParam.DescriptionControl := labAltitude;
	FAltitudeParam.ValueControl := editAltitude;
	FAltitudeParam.UnitControl := labAltitudeUnit;

	FGradientParam := TParameter.Create(puGradient);
	FGradientParam.DescriptionControl := labGradient;
	FGradientParam.ValueControl := editGradient;
	FGradientParam.UnitControl := labGradientUnit;

	FCourseSgfPoint := TPandaCollection.Create;
	FDistanceSgfPoint := TPandaCollection.Create;

end;

procedure TFIXParameterForm.FormDestroy(Sender: TObject);
begin
	FCourseParam.Free;
	FDistanceParam.Free;
	FIASParam.Free;
	FAltitudeParam.Free;
	FGradientParam.Free;
	FCourseSgfPoint.Free;
	FDistanceSgfPoint.Free;
end;

procedure TFIXParameterForm.SetCourseCurr(Range: TInterval; DefaultValue: Double; PtList: TPandaCollection;
							InConvertionPoint, OutConvertionPoint: TPoint);
begin
	FCourseParam.BeginUpdate;
	FCourseParam.Range := Range;
	FCourseParam.Value := DefaultValue;

	FCourseParam.InConvertionPoint := InConvertionPoint;
	FCourseParam.OutConvertionPoint := OutConvertionPoint;
end;

procedure TFIXParameterForm.SetDistCurr(Range: TInterval; DefaultValue: Double; PtList: TPandaCollection);
begin
	FDistanceParam.BeginUpdate;
	FDistanceParam.Range := Range;
	FDistanceParam.Value := DefaultValue;
end;

procedure TFIXParameterForm.SetIASCurr(Range: TInterval; DefaultValue: Double);
begin
	FIASParam.BeginUpdate;
	FIASParam.Range := Range;
	FIASParam.Value := DefaultValue;
end;

procedure TFIXParameterForm.SetAltitudeCurr(Range: TInterval; DefaultValue: Double);
begin
	FAltitudeParam.BeginUpdate;
	FAltitudeParam.Range := Range;
	FAltitudeParam.Value := DefaultValue;
end;

procedure TFIXParameterForm.SetGradientCurr(Range: TInterval; DefaultValue: Double);
begin
	FGradientParam.BeginUpdate;
	FGradientParam.Range := Range;
	FGradientParam.Value := DefaultValue;
end;

procedure TFIXParameterForm.SetSignificantPoints(SignificantCollection: TPandaCollection; CurrFIX, ReferenceFIX: TFIX; MaxDist: Double);
var
	dX, dY,
	fDist, fAngle:	Double;
	SigPoint:		TSignificantPoint;
	I:				Integer;
begin
	FFIX := CurrFIX;
	FReferenceFIX := ReferenceFIX;

	FCourseSgfPoint.Clear;
	cbDirList.Clear;

	for I := 0 to SignificantCollection.Count - 1 do
	begin
		SigPoint := SignificantCollection.Item [i].asSignificantPoint;
		dX := FReferenceFIX.PrjPt.X - sigPoint.Prj.X;
		dY := FReferenceFIX.PrjPt.Y - sigPoint.Prj.Y;
		fDist := Hypot(dX, dY);

		if fDist <= MaxDist then
		begin
			fAngle := ArcTan2(dY, dX);
			if CheckValue(fAngle, FCourseParam.Range, True) = fAngle then
			begin
				FCourseSgfPoint.Add(SigPoint);
				cbDirList.AddItem(SigPoint.AixmID, SigPoint);
			end;
		end
	end;

	rBtnAzimuth.Checked := True;
	if cbDirList.Items.Count = 0 then
	begin
		cbDirList.Enabled := False;
		rBtnAFromList.Enabled := False;
		labAPtType.Caption := '';
	end
	else
	begin
		cbDirList.Enabled := True;
		rBtnAFromList.Enabled := True;
		cbDirList.ItemIndex := 0;
	end;
end;

function TFIXParameterForm.Execute: Integer;
var
	Min, Max, Val:	Double;
	gr, MinRounded,
	MaxRounded:		Integer;
begin
	Min := Converters[puAngle].ConvertFunction(FCourseParam.Range.Right, cdToOuter, FCourseParam.OutConvertionPoint);
	Max := Converters[puAngle].ConvertFunction(FCourseParam.Range.Left, cdToOuter, FCourseParam.OutConvertionPoint);

	if Max < Min then Max := Max + 360.0;

	MinRounded := Round(Min + 0.4999);
	MaxRounded := Round(Max - 0.4999);

	if MinRounded >= MaxRounded then
	begin
		FCourseParam.ReadOnly := True;
		if MinRounded = MaxRounded then
			editCourse.Text := IntToStr(MaxRounded)
		else
		begin
			gr := Round(Log10(Max - Min) - 0.49999);
			Val := RoundTo(0.5*(Max + Min), gr);
			editCourse.Text := FloatToStr(Val);
		end;
	end
	else
	begin
		FCourseParam.ReadOnly := False;
		udCourse.Min := MinRounded;
		udCourse.Max := MaxRounded;
	end;

	FCourseParam.EndUpdate;
	FDistanceParam.EndUpdate;
	FIASParam.EndUpdate;
	FAltitudeParam.EndUpdate;
	FGradientParam.EndUpdate;

	result := ShowModal;
end;

procedure TFIXParameterForm.ChangeDirection(CourseParam: TParameter);
var
	fAngle2_0, fAngle50_0,
	MinDist,
	MaxDist,
	fDist, fAngle,
	fDistTreshol1,
	fDistTreshol2:	Double;
	FIXPoint:		TPoint;
	LocalSigPoint:	TPoint;
	SigPoint:		TSignificantPoint;
	I:				Integer;
begin
	fAngle2_0 := DegToRad(2.0);
	fAngle50_0 := DegToRad(50.0);
	FFIX.OutDirection := CourseParam.Value;

	fDist := FFIX.CalcFromMinStablizationDistance(0.5*PI);
	fDistTreshol1 := FReferenceFIX.CalcInToMinStablizationDistance(fAngle2_0) + fDist;
	fDistTreshol2 := FReferenceFIX.CalcInToMinStablizationDistance(fAngle50_0) + fDist;
	FDistanceSgfPoint.Clear;

	FDistanceSgfPoint.Clear;
	cbDistList.Clear;

	MaxDist := FDistanceParam.Range.Right;
	LocalSigPoint := TPoint.Create;

	for I := 0 to FCourseSgfPoint.Count - 1 do
	begin
		SigPoint := FCourseSgfPoint.Item [i].asSignificantPoint;
		PrjToLocal(FReferenceFIX.PrjPt, FFIX.OutDirection + Pi, SigPoint.Prj, LocalSigPoint);

		if(LocalSigPoint.X > 0)and(LocalSigPoint.Y <= 0.25*FFIX.XXT) then
		begin
			fAngle := ArcTan(LocalSigPoint.Y / LocalSigPoint.X);
			if fAngle<=fAngle2_0 then		MinDist := fDistTreshol1
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
		cbDistList.Enabled := False;
		rBtnDFromList.Enabled := False;
		labDPtType.Caption := '';
		ChangeDistance(FDistanceParam)
	end
	else
	begin
		cbDistList.Enabled := True;
		rBtnDFromList.Enabled := True;
		cbDistList.ItemIndex := 0;
	end;

	FIXPoint := LocalToPrj(FReferenceFIX.PrjPt, FFIX.OutDirection + PI, FDistanceParam.Value, 0.0);
	FFIX.PrjPt := FIXPoint;
	FIXPoint.Free;

	FFIX.RefreshGraphics;
end;

procedure TFIXParameterForm.ChangeDistance(DistParam: TParameter);
var
	FIXPoint:	TPoint;
begin
	FIXPoint := LocalToPrj(FReferenceFIX.PrjPt, FFIX.OutDirection + PI, DistParam.Value, 0.0);
	FFIX.PrjPt := FIXPoint;
	FIXPoint.Free;
	FFIX.RefreshGraphics;
end;

//========= Course
procedure TFIXParameterForm.rButtonDirectionClick(Sender: TObject);
begin
	FCourseParam.ReadOnly := (TRadioButton(Sender).Tag <> 0) or not FCourseParam.ChekRange;
	cbDirList.Enabled := (TRadioButton(Sender).Tag <> 0) and (cbDirList.Items.Count > 0);
end;

procedure TFIXParameterForm.editCourseExit(Sender: TObject);
begin
	FCourseParam.ReadFromControl;
end;

procedure TFIXParameterForm.udCourseClick(Sender: TObject;
  Button: TUDBtnType);
begin
	FCourseParam.BeginUpdate;
	FCourseParam.Value := Converters[puAngle].ConvertFunction(udCourse.Position, cdToInner, FCourseParam.InConvertionPoint);
	FCourseParam.EndUpdate;
end;

procedure TFIXParameterForm.cbDirListChange(Sender: TObject);
var
	sigPoint:		TSignificantPoint;
	Value:			Double;
begin
	sigPoint := TSignificantPoint(cbDirList.Items.Objects [cbDirList.ItemIndex]);
	labAPtType.Caption := AIXMTypeNames[sigPoint.AIXMType];
	Value := ArcTan2(FReferenceFIX.PrjPt.Y - sigPoint.Prj.Y, FReferenceFIX.PrjPt.X - sigPoint.Prj.X);

	FCourseParam.BeginUpdate;
	FCourseParam.Value := Value;
	FCourseParam.EndUpdate;
end;
//========= Distance

procedure TFIXParameterForm.rBtnDistanceClick(Sender: TObject);
begin
	FDistanceParam.ReadOnly := (TRadioButton(Sender).Tag <> 0) or not FCourseParam.ChekRange;
	cbDistList.Enabled := (TRadioButton(Sender).Tag <> 0) and (cbDistList.Items.Count > 0);
end;

procedure TFIXParameterForm.editDistanceExit(Sender: TObject);
begin
	FDistanceParam.ReadFromControl;
end;

procedure TFIXParameterForm.cbDistListChange(Sender: TObject);
var
	sigPoint:		TSignificantPoint;
	Value:			Double;
begin
	sigPoint := TSignificantPoint(cbDistList.Items.Objects [cbDistList.ItemIndex]);
	labDPtType.Caption := AIXMTypeNames[sigPoint.AIXMType];

	Value := Hypot(FReferenceFIX.PrjPt.Y - sigPoint.Prj.Y, FReferenceFIX.PrjPt.X - sigPoint.Prj.X);

	FDistanceParam.BeginUpdate;
	FDistanceParam.Value := Value;
	FDistanceParam.EndUpdate;
end;

// ========= Velocity
procedure TFIXParameterForm.editVelocityExit(Sender: TObject);
begin
	FIASParam.ReadFromControl
end;

procedure TFIXParameterForm.editAltitudeExit(Sender: TObject);
begin
	FAltitudeParam.ReadFromControl
end;

procedure TFIXParameterForm.editGradientExit(Sender: TObject);
begin
	FGradientParam.ReadFromControl
end;

procedure TFIXParameterForm.FormDeactivate(Sender: TObject);
begin
	hide
end;

end.


