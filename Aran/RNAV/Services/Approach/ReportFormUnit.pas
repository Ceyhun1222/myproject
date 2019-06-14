unit ReportFormUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls, ImgList, ExtCtrls, AIXMTypes, ObstacleInfoUnit,
  FlightPhaseUnit, Parameter;

const
	cStraightMA = 3;
	cStraightSegment_TIA = 4;
	cMATurnArea = 5;

type

	TLegData = record
		Leg:			TLeg;
		LegIx,
		ObsIx:			Integer;
		Obstacles:		TList;
		SortT,
		SortF:			Integer;
		ListView:		TListView;
	end;

	PLegData = ^TLegData;

	TPhaseData = record
		Count:			Integer;
		LegData:		Array of PLegData;
	end;

	TBranchData = record
		Count:			Integer;
		PhaseData:		Array [TFlightProcedure] of TPhaseData;
		LegData:		Array of TLegData;
		Branch:			TList;
		Obstacles:		TList;
	end;

	TReportForm = class(TForm)
	ImageList1: TImageList;
	PageControl1: TPageControl;
    tsInitialApproach: TTabSheet;
    tsIntermediateApproach: TTabSheet;
	ListView1: TListView;
	Panel1: TPanel;
	Label1: TLabel;
	UpDown1: TUpDown;
	cbBranch: TComboBox;
    tsFinalApproach: TTabSheet;
	UpDown2: TUpDown;
	cbLeg: TComboBox;
	Label2: TLabel;
	ListView2: TListView;
	ListView3: TListView;
    tsStraightMA: TTabSheet;
	ListView4: TListView;
    tsMAstraightsegment: TTabSheet;
    tsMAturnArea: TTabSheet;
    ListView5: TListView;
    ListView6: TListView;
	procedure ListView1ColumnClick(Sender: TObject; Column: TListColumn);
	procedure ListView1SelectItem(Sender: TObject; Item: TListItem;
	  Selected: Boolean);
	procedure UpDown1Changing(Sender: TObject; var AllowChange: Boolean);
	procedure FormDestroy(Sender: TObject);
	procedure UpDown2Changing(Sender: TObject; var AllowChange: Boolean);
	procedure PageControl1Change(Sender: TObject);
	procedure ListView1Editing(Sender: TObject; Item: TListItem;
	  var AllowEdit: Boolean);
	procedure ListView1Compare(Sender: TObject; Item1, Item2: TListItem;
	  Data: Integer; var Compare: Integer);
	procedure cbBranchChange(Sender: TObject);
	procedure cbLegChange(Sender: TObject);
	procedure ListView4ColumnClick(Sender: TObject; Column: TListColumn);
	procedure FormCreate(Sender: TObject);
    function FormHelp(Command: Word; Data: Integer;
      var CallHelp: Boolean): Boolean;
    procedure FormKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
    procedure ListView5ColumnClick(Sender: TObject; Column: TListColumn);
    procedure ListView6ColumnClick(Sender: TObject; Column: TListColumn);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
	{ Private declarations }
//	IxID:			String;
//	SortF:			Integer;
//	CurrLegs:		Array [TFlightProcedure] of PLegData;

	ReportBtn:		TControl;
	pPointElem:		Integer;
	CurrLeg:		PLegData;

//	CurrLeg:		Integer;

//	LegCnt:			Integer;
//	LegData:		Array of TLegData;

	CurrBranch:		Integer;
	BranchCount:	Integer;
	BranchData:		Array of TBranchData;
	SortF:			Array [0..15] of Integer;
	SortT:			Array [0..15] of Integer;
	ObstT:			Array [0..15] of TList;
  public
	{ Public declarations }
	procedure SetLegObst(LegObstacles: TList; Leg: TLeg; LegIx: LongInt = -1);
	procedure FillPage(LegData: TLegData);

	procedure AddBranch(Obstacles: TList; Branch: TList);
	procedure AddMissedApproach(ObstacleList: TList);
	procedure AddMIAS_TIA(ObstacleList: TList; TabCaption: String);
	procedure AddMITurn(ObstacleList: TList);

	procedure ModifyBranch(Obstacles: TList; Branch: TList; Index: Integer);
	procedure DeleteBranch(Index: Integer);
	procedure ClearAllBranchs;
//	procedure DeleteLeg;
	procedure ClearAll;
  end;

var
  ReportForm: TReportForm;

implementation

{$R *.dfm}

uses
	Math, ARANFunctions, ARANMath, ARANGlobals, ApproachGlobals, UnitConverter,
	ConstantsContract;

procedure TReportForm.ClearAll;
begin
//-
end;

procedure TReportForm.ClearAllBranchs;
var
	I:	Integer;
begin
	for I := 0 to BranchCount - 1 do
		DeleteBranch(I);
end;

procedure TReportForm.DeleteBranch(Index: Integer);
var
	I, N, ListI,
	LegIx:			Integer;
	Branch:			TList;
begin
	Branch := BranchData[Index].Branch;

	N := Branch.Count;


	SetLength(BranchData[Index].PhaseData[fpEnroute].LegData, 0);
	SetLength(BranchData[Index].PhaseData[fpDeparture].LegData, 0);
	SetLength(BranchData[Index].PhaseData[fpArrival].LegData, 0);
	SetLength(BranchData[Index].PhaseData[fpInitialApproach].LegData, 0);

//	SetLength(BranchData[Index].PhaseData[fpIntermediateApproach].LegData, 0);
//	SetLength(BranchData[Index].PhaseData[fpFinalApproach].LegData, 0);

	SetLength(BranchData[Index].PhaseData[fpInitialMissedApproach].LegData, 0);
//	SetLength(BranchData[Index].PhaseData[fpIntermediateMissedApproach].LegData, 0);
//	SetLength(BranchData[Index].PhaseData[fpFinalMissedApproach].LegData, 0);

	ListI := cbBranch.Items.IndexOfName(TLeg(Branch.Items[0]).StartFIX.Name);

	for LegIx := 0 to N - 1 do
		TList(BranchData[Index].LegData[LegIx].Obstacles).Free;

	for I := Index to BranchCount - 2 do
	begin
		BranchData[I] := BranchData[I + 1];
	end;

	SetLength(BranchData[CurrBranch].LegData, 0);

	Dec(BranchCount);
	if ListI >= 0 then
		cbBranch.Items.Delete(ListI);
		
	UpDown1.Max := BranchCount - 1;
	SetLength(BranchData, BranchCount);
end;

procedure TReportForm.ModifyBranch(Obstacles: TList; Branch: TList; Index: Integer);
var
	I, LegIx,
	N, M:			Integer;
//	AllowChange:	Boolean;
	Leg:			TLeg;
//	ObstList:		TList;
begin
	N := BranchData[Index].Count;

	for LegIx := 0 to N - 1 do
		TList(BranchData[Index].LegData[LegIx].Obstacles).Free;

	N := Branch.Count;
	M := Obstacles.Count;

	BranchData[Index].Branch := Branch;
	BranchData[Index].Count := N;
	BranchData[Index].Obstacles := Obstacles;
	SetLength(BranchData[Index].LegData, Branch.Count);

	BranchData[Index].PhaseData[fpEnroute].Count := 0;
	BranchData[Index].PhaseData[fpDeparture].Count := 0;
	BranchData[Index].PhaseData[fpArrival].Count := 0;
	BranchData[Index].PhaseData[fpInitialApproach].Count := 0;
	BranchData[Index].PhaseData[fpIntermediateApproach].Count := 0;
	BranchData[Index].PhaseData[fpFinalApproach].Count := 0;
	BranchData[Index].PhaseData[fpInitialMissedApproach].Count := 0;

//	BranchData[Index].PhaseData[fpIntermediateMissedApproach].Count := 0;
//	BranchData[Index].PhaseData[fpFinalMissedApproach].Count := 0;

	SetLength(BranchData[Index].PhaseData[fpEnroute].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpDeparture].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpArrival].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpInitialApproach].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpIntermediateApproach].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpFinalApproach].LegData, Branch.Count);
	SetLength(BranchData[Index].PhaseData[fpInitialMissedApproach].LegData, Branch.Count);
//	SetLength(BranchData[Index].PhaseData[fpIntermediateMissedApproach].LegData, Branch.Count);
//	SetLength(BranchData[Index].PhaseData[fpFinalMissedApproach].LegData, Branch.Count);

	for LegIx := 0 to N - 1 do
	begin
		BranchData[Index].LegData[LegIx].Obstacles := TList.Create;
		BranchData[Index].LegData[LegIx].LegIx := -1;
		BranchData[Index].LegData[LegIx].ObsIx := -1;
		Leg := Branch.Items[LegIx];
		BranchData[Index].LegData[LegIx].Leg := Leg;

		BranchData[Index].LegData[LegIx].SortF := 0;
		BranchData[Index].LegData[LegIx].SortT := 0;
		BranchData[Index].LegData[LegIx].ListView := nil;

		case Leg.FlightProcedure of
		fpArrival:
			begin
				BranchData[Index].PhaseData[fpArrival].LegData[BranchData[Index].PhaseData[fpArrival].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpArrival].Count);
			end;
		fpInitialApproach:
			begin
				BranchData[Index].PhaseData[fpInitialApproach].LegData[BranchData[Index].PhaseData[fpInitialApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpInitialApproach].Count);
				BranchData[Index].LegData[LegIx].ListView := ListView1;
			end;
		fpIntermediateApproach:
			begin
				BranchData[Index].PhaseData[fpIntermediateApproach].LegData[BranchData[Index].PhaseData[fpIntermediateApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpIntermediateApproach].Count);
				BranchData[Index].LegData[LegIx].ListView := ListView2;
			end;
		fpFinalApproach:
			begin
				BranchData[Index].PhaseData[fpFinalApproach].LegData[BranchData[Index].PhaseData[fpFinalApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpFinalApproach].Count);
				BranchData[Index].LegData[LegIx].ListView := ListView3;
			end;
		fpInitialMissedApproach:
			begin
				BranchData[Index].PhaseData[fpInitialMissedApproach].LegData[BranchData[Index].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpInitialMissedApproach].Count);
			end;
		fpIntermediateMissedApproach:
			begin
				BranchData[Index].PhaseData[fpInitialMissedApproach].LegData[BranchData[Index].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpInitialMissedApproach].Count);
			end;
		fpFinalMissedApproach:
			begin
				BranchData[Index].PhaseData[fpInitialMissedApproach].LegData[BranchData[Index].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[Index].LegData[LegIx];
				Inc(BranchData[Index].PhaseData[fpInitialMissedApproach].Count);
			end;
		end;
	end;

	for I := 0 to M - 1 do
	begin
		LegIx := TObstacleInfo(Obstacles.Items[I]).Leg;
		BranchData[Index].LegData[LegIx].Obstacles.Add(Obstacles.Items[I]);
	end;

	SetLength(BranchData[Index].PhaseData[fpEnroute].LegData, BranchData[Index].PhaseData[fpEnroute].Count);
	SetLength(BranchData[Index].PhaseData[fpDeparture].LegData, BranchData[Index].PhaseData[fpDeparture].Count);
	SetLength(BranchData[Index].PhaseData[fpArrival].LegData, BranchData[Index].PhaseData[fpArrival].Count);
	SetLength(BranchData[Index].PhaseData[fpInitialApproach].LegData, BranchData[Index].PhaseData[fpInitialApproach].Count);
//	SetLength(BranchData[Index].PhaseData[fpIntermediateApproach].LegData, BranchData[Index].PhaseData[fpIntermediateApproach].Count);
//	SetLength(BranchData[Index].PhaseData[fpFinalApproach].LegData, BranchData[Index].PhaseData[fpFinalApproach].Count);

	SetLength(BranchData[Index].PhaseData[fpInitialMissedApproach].LegData, BranchData[Index].PhaseData[fpInitialMissedApproach].Count);
//	SetLength(BranchData[Index].PhaseData[fpIntermediateMissedApproach].LegData, BranchData[Index].PhaseData[fpIntermediateMissedApproach].Count);
//	SetLength(BranchData[Index].PhaseData[fpFinalMissedApproach].LegData, BranchData[Index].PhaseData[fpFinalMissedApproach].Count);

//	cbBranch.ItemIndex := CurrBranch;
	cbBranchChange(cbBranch);
{
	cbBranch.Clear;

	cbBranch.Items.Add(TLeg(Branch.Items[0]).StartFIX.Name);
	if cbBranch.Items.Count > 0 then
	begin
		cbBranch.ItemIndex := CurrBranch;
		cbBranchChange(cbBranch);
	end;
}
end;

procedure TReportForm.SetLegObst(LegObstacles: TList; Leg: TLeg; LegIx: LongInt = -1);
//var
//	AllowChange:	Boolean;
begin
	BranchData[CurrBranch].LegData[BranchData[CurrBranch].Count - 1].Obstacles := LegObstacles;
	BranchData[CurrBranch].LegData[BranchData[CurrBranch].Count - 1].LegIx := LegIx;
	BranchData[CurrBranch].LegData[BranchData[CurrBranch].Count - 1].Leg := Leg;
	UpDown2.Position := BranchData[CurrBranch].Count - 1;
//	if UpDown2.Max = 0 then
//		UpDown2Changing(UpDown2, AllowChange)
end;

procedure TReportForm.AddBranch(Obstacles: TList; Branch: TList);
var
	I, LegIx,
	N, M:			Integer;
//	AllowChange:	Boolean;
	Leg:			TLeg;
begin
	Inc(BranchCount);
	CurrBranch := BranchCount - 1;
	UpDown1.Max := CurrBranch;
	N := Branch.Count;
	M := Obstacles.Count;

	SetLength(BranchData, BranchCount);

	BranchData[CurrBranch].Branch := Branch;
	BranchData[CurrBranch].Count := N;
	BranchData[CurrBranch].Obstacles := Obstacles;
	SetLength(BranchData[CurrBranch].LegData, Branch.Count);

	BranchData[CurrBranch].PhaseData[fpEnroute].Count := 0;
	BranchData[CurrBranch].PhaseData[fpDeparture].Count := 0;
	BranchData[CurrBranch].PhaseData[fpArrival].Count := 0;
	BranchData[CurrBranch].PhaseData[fpInitialApproach].Count := 0;
	BranchData[CurrBranch].PhaseData[fpIntermediateApproach].Count := 0;
	BranchData[CurrBranch].PhaseData[fpFinalApproach].Count := 0;
	BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count := 0;

//	BranchData[CurrBranch].PhaseData[fpIntermediateMissedApproach].Count := 0;
//	BranchData[CurrBranch].PhaseData[fpFinalMissedApproach].Count := 0;

	SetLength(BranchData[CurrBranch].PhaseData[fpEnroute].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpDeparture].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpArrival].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpInitialApproach].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpIntermediateApproach].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpFinalApproach].LegData, Branch.Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].LegData, Branch.Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpIntermediateMissedApproach].LegData, Branch.Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpFinalMissedApproach].LegData, Branch.Count);

	for LegIx := 0 to N - 1 do
	begin
		BranchData[CurrBranch].LegData[LegIx].Obstacles := TList.Create;
		BranchData[CurrBranch].LegData[LegIx].LegIx := -1;
		BranchData[CurrBranch].LegData[LegIx].ObsIx := -1;
		Leg := Branch.Items[LegIx];
		BranchData[CurrBranch].LegData[LegIx].Leg := Leg;

		BranchData[CurrBranch].LegData[LegIx].SortF := 0;
		BranchData[CurrBranch].LegData[LegIx].SortT := 0;
		BranchData[CurrBranch].LegData[LegIx].ListView := nil;

		case Leg.FlightProcedure of
		fpArrival:
			begin
				BranchData[CurrBranch].PhaseData[fpArrival].LegData[BranchData[CurrBranch].PhaseData[fpArrival].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpArrival].Count);
			end;
		fpInitialApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpInitialApproach].LegData[BranchData[CurrBranch].PhaseData[fpInitialApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpInitialApproach].Count);
				BranchData[CurrBranch].LegData[LegIx].ListView := ListView1;
			end;
		fpIntermediateApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpIntermediateApproach].LegData[BranchData[CurrBranch].PhaseData[fpIntermediateApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpIntermediateApproach].Count);
				BranchData[CurrBranch].LegData[LegIx].ListView := ListView2;
			end;
		fpFinalApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpFinalApproach].LegData[BranchData[CurrBranch].PhaseData[fpFinalApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpFinalApproach].Count);
				BranchData[CurrBranch].LegData[LegIx].ListView := ListView3;
			end;
		fpInitialMissedApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].LegData[BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count);
			end;
		fpIntermediateMissedApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].LegData[BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count);
			end;
		fpFinalMissedApproach:
			begin
				BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].LegData[BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count]
							:= @BranchData[CurrBranch].LegData[LegIx];
				Inc(BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count);
			end;
		end;
	end;

	for I := 0 to M - 1 do
	begin
		LegIx := TObstacleInfo(Obstacles.Items[I]).Leg;
		BranchData[CurrBranch].LegData[LegIx].Obstacles.Add(Obstacles.Items[I]);
	end;

	SetLength(BranchData[CurrBranch].PhaseData[fpEnroute].LegData, BranchData[CurrBranch].PhaseData[fpEnroute].Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpDeparture].LegData, BranchData[CurrBranch].PhaseData[fpDeparture].Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpArrival].LegData, BranchData[CurrBranch].PhaseData[fpArrival].Count);
	SetLength(BranchData[CurrBranch].PhaseData[fpInitialApproach].LegData, BranchData[CurrBranch].PhaseData[fpInitialApproach].Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpIntermediateApproach].LegData, BranchData[CurrBranch].PhaseData[fpIntermediateApproach].Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpFinalApproach].LegData, BranchData[CurrBranch].PhaseData[fpFinalApproach].Count);

	SetLength(BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].LegData, BranchData[CurrBranch].PhaseData[fpInitialMissedApproach].Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpIntermediateMissedApproach].LegData, BranchData[CurrBranch].PhaseData[fpIntermediateMissedApproach].Count);
//	SetLength(BranchData[CurrBranch].PhaseData[fpFinalMissedApproach].LegData, BranchData[CurrBranch].PhaseData[fpFinalMissedApproach].Count);


	cbBranch.Items.Add(TLeg(Branch.Items[0]).StartFIX.Name);
	if cbBranch.Items.Count > 0 then
	begin
		cbBranch.ItemIndex := 0;
		cbBranchChange(cbBranch);
	end;

//	if UpDown1.Max = 0 then
//		UpDown1Changing(UpDown1, AllowChange)
end;

{
	CurrBranch:		Integer;
	BranchCnt:		Integer;
	Branch:			Array of TBranchData;
procedure TReportForm.DeleteLeg;
begin
	if LegCnt > 0 then
	begin
		Dec(LegCnt);
		SetLength(LegData, LegCnt);
		UpDown1.Max := LegCnt - 1;
	end;
end;
}

procedure TReportForm.FillPage(LegData: TLegData);
const Ignored : Array [Boolean] of string = ('No','Yes');
var
	I, N:			LongInt;
	itmX:			TListItem;

//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;

	Obstacle:		TObstacleInfo;
begin
	LegData.ListView.Clear;
	N := LegData.Obstacles.Count;
	if N = 0 then exit;

//	if HPrevFIX > -10000.0 then
//		Label3.Caption := FormatFloat(GVDistConvertString, ConvertAltitude(HPrevFIX, cdToOuter))
//	else
//		Label3.Caption := '';

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(LegData.Obstacles.Items[I]);
		
//		ItemBolt := I = LegData.ObsIx;
//		if ItemBolt then		ItemForeColor := 255
//		else					ItemForeColor := 0;

		itmX := LegData.ListView.Items.Add;
		itmX.ImageIndex := -1;
		itmX.Data := Obstacle;

		itmX.Caption := Obstacle.Name;
		itmX.SubItems.Add(Obstacle.AID);

		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.Elevation, cdToOuter), GAltitudeAccuracy, rtCeil));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.MOC, cdToOuter), GAltitudeAccuracy));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.ReqH, cdToOuter), GAltitudeAccuracy));
		itmX.SubItems.Add(RoundToStr(ConvertDistance(Obstacle.Dist, cdToOuter), GDistanceAccuracy));

		if Obstacle.Flags and 1 = 1 then	itmX.SubItems.Add('Primary')
		else								itmX.SubItems.Add('Secondary');

		itmX.SubItems.Add(Ignored[Obstacle.Ignored]);

//		if I = Ix then
//		begin
//			itmX.Bold := ItemBolt;
//			itmX.ForeColor := ItemForeColor;
//			for J := 1 to 7 do
//			begin
//				itmX.SubItems[1].Bold := ItemBolt;
//				itmX.SubItems[1].ForeColor := ItemForeColor;
//			end;
//		end;
	end;

//If ReportBtn Then Show 0
end;

procedure TReportForm.AddMissedApproach(ObstacleList: TList);
var
	I, N:			LongInt;
	itmX:			TListItem;
	Obstacle:		TObstacleInfo;
	fMOC30, ReqOCA:	Double;
//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
begin
	ListView4.Clear;
	N := ObstacleList.Count;
	if N = 0 then exit;

//	if HPrevFIX > -10000.0 then
//		Label3.Caption := FormatFloat(GVDistConvertString, ConvertAltitude(HPrevFIX, cdToOuter))
//	else
//		Label3.Caption := '';

	ObstT[cStraightMA] := ObstacleList;
	fMOC30 := GPANSOPSConstants.Constant[arMA_InterMOC].Value;
//	fMOC50 := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(ObstacleList.Items[I]);
//		ItemBolt := I = MaxI;
//		if ItemBolt then		ItemForeColor := 255
//		else					ItemForeColor := 0;

		itmX := ListView4.Items.Add;
		itmX.ImageIndex := -1;
		itmX.Data := Obstacle;

		itmX.Caption := Obstacle.Name;
		itmX.SubItems.Add(Obstacle.AID);
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.Elevation, cdToOuter), GAltitudeAccuracy, rtCeil));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.fTmp * fMOC30, cdToOuter), GAltitudeAccuracy));

		ReqOCA := Obstacle.ReqOCA;
		if ReqOCA < 0 then	ReqOCA := 0;
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(ReqOCA, cdToOuter), GAltitudeAccuracy));
		itmX.SubItems.Add(RoundToStr(ConvertDistance(Obstacle.Dist, cdToOuter), GDistanceAccuracy));

		if Obstacle.Flags and 1 = 1 then	itmX.SubItems.Add('Primary')
		else								itmX.SubItems.Add('Secondary');

		if Obstacle.FlightProcedure >= fpEnroute then
											itmX.SubItems.Add(FlightPhases[Obstacle.FlightProcedure].Name)
		else								itmX.SubItems.Add('-')
	end;
end;

procedure TReportForm.AddMIAS_TIA(ObstacleList: TList; TabCaption: String);
var
	I, N:			LongInt;
	itmX:			TListItem;
	Obstacle:		TObstacleInfo;
	fMOC30, fMOC50,
	ReqOCA, fTA:	Double;

//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
begin
	tsMAstraightsegment.Caption := TabCaption;
	ListView5.Clear;

	N := ObstacleList.Count;
	if N = 0 then exit;

	ObstT[cStraightSegment_TIA] := ObstacleList;

	fMOC30 := GPANSOPSConstants.Constant[arMA_InterMOC].Value;
	fMOC50 := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;

	for I := 0 to N - 1 do
	begin
//		ItemBolt := I = MaxI;
//		if ItemBolt then		ItemForeColor := 255
//		else					ItemForeColor := 0;

		Obstacle := TObstacleInfo(ObstacleList.Items[I]);

		itmX := ListView5.Items.Add;
		itmX.ImageIndex := -1;
		itmX.Data := Obstacle;

		itmX.Caption := Obstacle.Name;
		itmX.SubItems.Add(Obstacle.AID);

		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.Elevation, cdToOuter), GAltitudeAccuracy, rtCeil));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.fTmp * fMOC30, cdToOuter), GAltitudeAccuracy));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.fTmp * fMOC50, cdToOuter), GAltitudeAccuracy));

		ReqOCA := Obstacle.ReqOCA;
		if ReqOCA < 0 then	ReqOCA := 0;
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(ReqOCA, cdToOuter), GAltitudeAccuracy));

		fTA := Obstacle.Elevation + Obstacle.fTmp * fMOC50;
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(fTA, cdToOuter), GAltitudeAccuracy));

		itmX.SubItems.Add(RoundToStr(ConvertDistance(Obstacle.Dist, cdToOuter), GDistanceAccuracy));

		if Obstacle.Flags and 1 = 1 then	itmX.SubItems.Add('Primary')
		else								itmX.SubItems.Add('Secondary');
	end;
end;

procedure TReportForm.AddMITurn(ObstacleList: TList);
var
	I, N:			LongInt;
	itmX:			TListItem;
	fMOC50, ReqOCA:	Double;
	Obstacle:		TObstacleInfo;
//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
begin
	ListView6.Clear;
	N := ObstacleList.Count;
	if N = 0 then exit;

	ObstT[cMATurnArea] := ObstacleList;
	fMOC50 := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(ObstacleList.Items[I]);
//		ItemBolt := I = MaxI;
//		if ItemBolt then		ItemForeColor := 255
//		else					ItemForeColor := 0;

		itmX := ListView6.Items.Add;
		itmX.ImageIndex := -1;
		itmX.Data := Obstacle;

		itmX.Caption := Obstacle.Name;
		itmX.SubItems.Add(Obstacle.AID);

		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.Elevation, cdToOuter), GAltitudeAccuracy, rtCeil));
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(Obstacle.fTmp * fMOC50, cdToOuter), GAltitudeAccuracy));

		ReqOCA := Obstacle.ReqOCA;
		if ReqOCA < 0 then		ReqOCA := 0;
		itmX.SubItems.Add(RoundToStr(ConvertAltitude(ReqOCA, cdToOuter), GAltitudeAccuracy));

		itmX.SubItems.Add(RoundToStr(ConvertDistance(Obstacle.Dist, cdToOuter), GDistanceAccuracy));

		if Obstacle.Flags and 1 = 1 then	itmX.SubItems.Add('Primary')
		else								itmX.SubItems.Add('Secondary');
	end;
end;

procedure TReportForm.ListView1ColumnClick(Sender: TObject; Column: TListColumn);
const cIgnored : Array [Boolean] of string = ('No','Yes');
var
	I, N:			LongInt;	//, J, M
	itmX:			TListItem;
	ListView:		TListView;
//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
begin
	ListView := CurrLeg.ListView;
	ListView.SortType := stNone;
	N := CurrLeg.Obstacles.Count;

	if Abs(CurrLeg.SortF)-1 = Column.Index then
		CurrLeg.SortF := -CurrLeg.SortF
	else
	begin
		if CurrLeg.SortF <> 0 then
			ListView.Columns.Items[Abs(CurrLeg.SortF)-1].ImageIndex := -1;
		CurrLeg.SortF := Column.Index + 1
	end;

	if CurrLeg.SortF > 0 then	Column.ImageIndex := 0
	else						Column.ImageIndex := 1;

{	itmX.Data := Obstacle;	}

	if (Column.Index < 6) and (Column.Index >= 2) then
	begin
		for I := 0 to N -1 do with TObstacleInfo(CurrLeg.Obstacles.Items[I]) do
			case Column.Index of
			2:	fSort := Elevation;
			3:	fSort := MOC;
			4:	fSort := ReqH;
			5:	fSort := Dist;
			end;
		CurrLeg.SortT := 0
	end
	else
	begin

		for I := 0 to N-1 do with TObstacleInfo(CurrLeg.Obstacles.Items[I]) do
			case Column.Index of
			0:	sSort := Name;
			1:	sSort := ID;
			6:	if Flags and 1 = 1 then		sSort := 'Primary'
				else						sSort := 'Secondary';
			7:	sSort := cIgnored[Ignored];
			end;

		CurrLeg.SortT := 1
	end;

	ListView.SortType := stData;
	itmX := ListView.Selected;
	ListView.OnSelectItem(ListView, itmX, True);
end;

procedure TReportForm.ListView4ColumnClick(Sender: TObject;
  Column: TListColumn);
var
	I, N, Ix:		LongInt;	//, J, M
	itmX:			TListItem;
	ListView:		TListView;

//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
//	CurrLeg:		PLegData;
begin
	ListView := TListView(Sender);
	Ix := ListView.Tag;
	ListView.SortType := stNone;
	if not Assigned(ObstT[Ix]) then
		exit;

	N := ObstT[Ix].Count;
	if Abs(SortF[Ix]) - 1 = Column.Index then
		SortF[Ix] := -SortF[Ix]
	else
	begin
		if SortF[Ix] <> 0 then
			ListView.Columns.Items[Abs(SortF[Ix])-1].ImageIndex := -1;
		SortF[Ix] := Column.Index + 1
	end;

	if SortF[Ix] > 0 then	Column.ImageIndex := 0
	else					Column.ImageIndex := 1;
	
{	itmX.Data := Obstacle;	}

	if (Column.Index <= 5) and (Column.Index >= 2) then
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			2:	fSort := Elevation;
			3:	fSort := fTmp;
			4:	if ReqOCA > 0 then	fSort := ReqOCA
				else				fSort := 0;
			5:	fSort := Dist;
			end;
		SortT[Ix] := 0
	end
	else
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			0:	sSort := Name;
			1:	sSort := ID;
			6:	if Flags and 1 = 1 then		sSort := 'Primary'
				else						sSort := 'Secondary';
			7:	if FlightProcedure >= fpEnroute then
											sSort := FlightPhases[FlightProcedure].Name
				else						sSort := '-';
			end;
		SortT[Ix] := 1
	end;

	ListView.SortType := stData;
	itmX := ListView.Selected;
//	ListView.Selected := itmX;
	ListView.OnSelectItem(ListView, itmX, True);
end;

procedure TReportForm.ListView5ColumnClick(Sender: TObject;
  Column: TListColumn);
var
	I, N, Ix:		LongInt;	//, J, M
	itmX:			TListItem;
	ListView:		TListView;
	fMOC50:			Double;

//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
//	CurrLeg:		PLegData;
begin
	ListView := TListView(Sender);
	Ix := ListView.Tag;
	ListView.SortType := stNone;
	if not Assigned(ObstT[Ix]) then
		exit;

	N := ObstT[Ix].Count;
	if Abs(SortF[Ix]) - 1 = Column.Index then
		SortF[Ix] := -SortF[Ix]
	else
	begin
		if SortF[Ix] <> 0 then
			ListView.Columns.Items[Abs(SortF[Ix])-1].ImageIndex := -1;
		SortF[Ix] := Column.Index + 1
	end;

	if SortF[Ix] > 0 then	Column.ImageIndex := 0
	else					Column.ImageIndex := 1;

	fMOC50 := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;

{	itmX.Data := Obstacle;	}

	if (Column.Index <= 7) and (Column.Index >= 2) then
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			2:	fSort := Elevation;
			3, 4:	fSort := fTmp;
			5:	if ReqOCA > 0 then	fSort := ReqOCA
				else				fSort := 0;
			6:	fSort :=  Elevation + fTmp * fMOC50;

			7:	fSort := Dist;
			end;
		SortT[Ix] := 0
	end
	else
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			0:	sSort := Name;
			1:	sSort := ID;
			7:	if Flags and 1 = 1 then		sSort := 'Primary'
				else						sSort := 'Secondary';
			end;
		SortT[Ix] := 1
	end;

	ListView.SortType := stData;
	itmX := ListView.Selected;
//	ListView.Selected := itmX;
	ListView.OnSelectItem(ListView, itmX, True);
end;

procedure TReportForm.ListView6ColumnClick(Sender: TObject;
  Column: TListColumn);
var
	I, N, Ix:		LongInt;	//, J, M
	itmX:			TListItem;
	ListView:		TListView;

//	ItemForeColor:	TColor;
//	ItemBolt:		Boolean;
//	CurrLeg:		PLegData;
begin
	ListView := TListView(Sender);
	Ix := ListView.Tag;
	ListView.SortType := stNone;
	if not Assigned(ObstT[Ix]) then
		exit;

	N := ObstT[Ix].Count;
	if Abs(SortF[Ix]) - 1 = Column.Index then
		SortF[Ix] := -SortF[Ix]
	else
	begin
		if SortF[Ix] <> 0 then
			ListView.Columns.Items[Abs(SortF[Ix])-1].ImageIndex := -1;
		SortF[Ix] := Column.Index + 1
	end;

	if SortF[Ix] > 0 then	Column.ImageIndex := 0
	else					Column.ImageIndex := 1;

{	itmX.Data := Obstacle;	}

	if (Column.Index <= 5) and (Column.Index >= 2) then
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			2:	fSort := Elevation;
			3:	fSort := fTmp;
			4:	if ReqOCA > 0 then	fSort := ReqOCA
				else				fSort := 0;
			5:	fSort := Dist;
			end;
		SortT[Ix] := 0
	end
	else
	begin
		for I := 0 to N - 1 do with TObstacleInfo(ObstT[Ix].Items[I]) do
			case Column.Index of
			0:	sSort := Name;
			1:	sSort := ID;
			6:	if Flags and 1 = 1 then		sSort := 'Primary'
				else						sSort := 'Secondary';
			end;
		SortT[Ix] := 1
	end;

	ListView.SortType := stData;
	itmX := ListView.Selected;
//	ListView.Selected := itmX;
	ListView.OnSelectItem(ListView, itmX, True);
end;

procedure TReportForm.ListView1SelectItem(Sender: TObject; Item: TListItem; Selected: Boolean);
var
	ObstacleInfo:	TObstacleInfo;
begin
	if not Assigned(Item) then exit;

	GUI.SafeDeleteGraphic(pPointElem);
	pPointElem := -1;

	ObstacleInfo := TObstacleInfo(Item.Data);
	pPointElem := GUI.DrawPointWithText(ObstacleInfo.ptPrj, 255, ObstacleInfo.AID + ' (' + ObstacleInfo.Name + ')');
end;

procedure TReportForm.ListView1Editing(Sender: TObject; Item: TListItem;
  var AllowEdit: Boolean);
begin
	AllowEdit := False;
end;

procedure TReportForm.ListView1Compare(Sender: TObject; Item1,
  Item2: TListItem; Data: Integer; var Compare: Integer);
var
	Obstacle1,
	Obstacle2:		TObstacleInfo;
	Ix,
	ISort,
	SSort:			Integer;
	ListView:		TListView;
begin
	ListView := TListView(Sender);
	Ix := ListView.Tag;
	Obstacle1 := Item1.Data;
	Obstacle2 := Item2.Data;

	if PageControl1.ActivePageIndex > 2 then
	begin
		SSort := Sign(SortF[Ix]);
		ISort := SortT[Ix];
	end
	else
	begin
		SSort := Sign(CurrLeg.SortF);
		ISort := CurrLeg.SortT;
	end;

	if ISort = 0 then
	begin
			 if Obstacle1.fSort < Obstacle2.fSort then	Compare := SSort
		else if Obstacle1.fSort > Obstacle2.fSort then	Compare := -SSort
		else											Compare := 0
	end
	else
		Compare := SSort*CompareText(Obstacle2.sSort, Obstacle1.sSort);
end;

procedure TReportForm.FormCreate(Sender: TObject);
var
	I:		Integer;
begin
	pPointElem := -1;
	for I := 0 to 15 do
	begin
		SortF[I] := 0;
		SortT[I] := 0;
	end;
	PageControl1.ActivePageIndex := 0;
end;

procedure TReportForm.FormDestroy(Sender: TObject);
begin
	ReportForm := nil;
end;

procedure TReportForm.PageControl1Change(Sender: TObject);
begin
	cbBranchChange(cbBranch);
	GUI.SafeDeleteGraphic(pPointElem);
	pPointElem := -1;
end;

procedure TReportForm.UpDown1Changing(Sender: TObject; var AllowChange: Boolean);
//var
//	K:	Integer;
begin
{	K := UpDown1.Position;
	if (K >= 0) and (K <= BranchCount) then
	begin
		CurrBranch := K;
		FillPage(BranchData[CurrBranch].PhaseData[fpInitialApproach].LegData[0]^);
		FillPage(BranchData[CurrBranch].PhaseData[fpIntermediateApproach].LegData[0]^);
		FillPage(BranchData[CurrBranch].PhaseData[fpFinalApproach].LegData[0]^);
	end
}
end;

procedure TReportForm.cbBranchChange(Sender: TObject);
var
	I, N, K:		Integer;
	Leg:			TLeg;
	Branch:			TBranchData;
	Phase:			TPhaseData;
begin
	K := cbBranch.ItemIndex;
	if K < 0 then exit;

	CurrBranch := cbBranch.ItemIndex;
	Branch := BranchData[CurrBranch];

//	fpInitialMissedApproach, fpIntermediateMissedApproach, fpFinalMissedApproach

	cbLeg.Clear;
	case PageControl1.ActivePageIndex of
	0:	begin
			Phase := Branch.PhaseData[fpInitialApproach];
		end;
	1:	begin
			Phase := Branch.PhaseData[fpIntermediateApproach];
		end;
	2:	begin
			Phase := Branch.PhaseData[fpFinalApproach];
		end;
	3:	begin
			Phase := Branch.PhaseData[fpInitialMissedApproach];
		end;
	4:	begin
			exit;
		end;
	5:	begin
			exit;
		end
		else
			exit;
	end;

	N := Phase.Count;

	for I := 0 to  N - 1 do
	begin
		Leg := TLeg(Phase.LegData[I].Leg);
		cbLeg.Items.Add(Leg.StartFIX.Name + ' - ' + Leg.EndFIX.Name);
	end;

	if cbLeg.Items.Count>0 then
	begin
		cbLeg.ItemIndex := 0;
		cbLegChange(cbLeg);
	end;

end;

procedure TReportForm.UpDown2Changing(Sender: TObject;
  var AllowChange: Boolean);
var
	K:	Integer;
begin
	K := UpDown2.Position;
	if (K >= 0) and (K <= BranchCount) then
	begin
//		BranchData[K].
//		CurLeg := K;
//		FillPage(LegData[K].FIXObstacles, LegData[K].Branch, LegData[K].FIXIx);
	end
end;

procedure TReportForm.cbLegChange(Sender: TObject);
var
	K:				Integer;	//I, N,
	Branch:			TBranchData;
	Phase:			TPhaseData;
//	Leg:			TLeg;
begin
	K := cbLeg.ItemIndex;
	if K < 0 then exit;

	Branch := BranchData[CurrBranch];

	case PageControl1.ActivePageIndex of
	0:	begin
			Phase := Branch.PhaseData[fpInitialApproach];
		end;
	1:	begin
			Phase := Branch.PhaseData[fpIntermediateApproach];
		end;
	2:	begin
			Phase := Branch.PhaseData[fpFinalApproach];
		end;
	3:	begin
			Phase := Branch.PhaseData[fpInitialMissedApproach];
		end;
	end;

	CurrLeg := Phase.LegData[K];
	FillPage(CurrLeg^);
end;


procedure TReportForm.FormClose(Sender: TObject; var Action: TCloseAction);
begin
	GUI.SafeDeleteGraphic(pPointElem);
	pPointElem := -1;
end;

function TReportForm.FormHelp(Command: Word; Data: Integer;
  var CallHelp: Boolean): Boolean;
begin
	CallHelp := False;
	Result := True;
//	ShowHelp(Handle,  3700);
end;

procedure TReportForm.FormKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
	if Key = VK_F1 then
		ShowHelp(Handle,  3700);
end;

initialization
	ReportForm := nil;

end.

