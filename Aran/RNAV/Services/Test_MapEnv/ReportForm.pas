unit ReportForm;

interface

uses
  ARANFunctions, Windows, Messages, SysUtils, Variants,
  Classes, Graphics, Controls, Forms, Dialogs, StdCtrls,
  ComCtrls, DepartureClasses, CollectionUnit, Grids,
  UIContract, ConstantsContract, DepartureGlobals, ReportUnit_;

type

  TReportDForm = class(TForm)
    bHelp: TButton;
    procedure bHelpClick(Sender: TObject);
    function FormHelp(Command: Word; Data: Integer;
      var CallHelp: Boolean): Boolean;
  private
	FColumnType,
	FColumnIndex,
	FSortAscColumnIndex,
	FSortAsc,
	FPenetrationBackColor,
	FObstacleGraphicHandle: Integer;
	FObstacleSymbol: TPointSymbol;
	FFormType: TDepartureFormType;
	FColumnHeaders: Array [0..16] of String;
  private
	procedure FillStraightColumns (listView: TListView);
	procedure FillTurnColumns (listView: TListView);
	procedure SetFormType (vFormType: TDepartureFormType);
	procedure FillStaightItems (listView: TListView; obsInAreaList: TObstacleInAreaList);
	procedure FillTurnItems (listView: TListView; obsInAreaList: TObstacleInAreaList);
	function GetReportCount (): Integer;
	procedure FillControlMUI ();
  public
	destructor Destroy; override;

	procedure Show;
	procedure AddItem (reportItem: TReportItem);
	procedure SetItem (index: Integer; reportItem: TReportItem);
    function GetItem (index: Integer): TReportItem;
	procedure ShowReport (index: Integer);
	procedure SaveReport (reportFile: TReportFile);

	property ReportCount: Integer read GetReportCount;
	property FormType: TDepartureFormType read FFormType write SetFormType;
  published
	bClose: TButton;
	labObstacleCount: TLabel;
	editObtacleCount: TEdit;
	cbTurnSegments: TComboBox;
	labTurnSegments: TLabel;
	lv: TListView;
	labPDG: TLabel;
	editPDG: TEdit;
	bSave: TButton;
	lvSaveReport: TListView;
	UnitControl: TLabel;	

	procedure bCloseClick(Sender: TObject);
	procedure FormClose(Sender: TObject; var Action: TCloseAction);
	procedure FormCreate(Sender: TObject);
	procedure lvCompare(Sender: TObject; Item1, Item2: TListItem; Data: Integer; var Compare: Integer);
	procedure lvColumnClick(Sender: TObject; Column: TListColumn);
	procedure lvSelectItem (Sender: TObject; Item: TListItem; Selected: Boolean);
	procedure cbTurnSegmentsSelect (Sender: TObject);
	procedure lvCustomDrawItem(Sender: TCustomListView; Item: TListItem; State: TCustomDrawState; var DefaultDraw: Boolean);
  end;

var
  ReportDForm: TReportDForm;

implementation

uses
	Math,
	DepartureService,
	ARANMath,
	ARANGlobals,
	Parameter, StrUtils;

{$R *.dfm}

destructor TReportDForm.Destroy;      
begin
	inherited;
	DeleteGraphic (FObstacleGraphicHandle);

	FObstacleSymbol.Free;
end;


procedure TReportDForm.FillStraightColumns (listView: TListView);
var
	lvColumn:	TListColumn;
begin
	listView.Columns.Clear;
                        
	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [0];//'ID';
	lvColumn.Alignment := taLeftJustify;
	lvColumn.Tag := 0;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [1];//'Name';
	lvColumn.Width := 80;
	lvColumn.Alignment := taLeftJustify;
	lvColumn.Tag := 0;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [2];//'H penetration';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [3] + ' (%)';//'Gradient to the obstacle top';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [4] + ' (%)'; //'Req. gradient';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [5]; //'Req. TNH';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [6]; //'Lateral distance from DNT';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [7]; //'Distance from DER along DNT';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [8]; //'Height above DER';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [9]; //'MOC 0.8%';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [10]; //'Req. H 0.8%';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [11] + ' (%)'; //'PDG';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [12]; //'Track length of climbing at PDG';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [13]; //'Req. height reached using PDG';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [14]; //'Req. H > 60 m';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 0;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [15]; //'Hor. accuracy';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [16]; //'Ver. accuracy';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;
end;

procedure TReportDForm.FillTurnColumns (listView: TListView);
var
	lvColumn:	TListColumn;
begin
	listView.Columns.Clear;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [0]; //'ID';
	lvColumn.Alignment := taLeftJustify;
	lvColumn.Tag := 0;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [1]; //'Name';
	lvColumn.Width := 80;
	lvColumn.Alignment := taLeftJustify;
	lvColumn.Tag := 0;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [4] + ' (%)'; //'Req. gradient';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [2]; //'H penetration';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [8]; //'Height above DER';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := 'd0';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := 'Dr*';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := 'MOC';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [15]; //'Hor. accuracy';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;

	lvColumn := listView.Columns.Add;
	lvColumn.Caption := FColumnHeaders [16]; //'Ver. accuracy';
	lvColumn.Alignment := taRightJustify;
	lvColumn.Tag := 1;
end;

procedure TReportDForm.AddItem (reportItem: TReportItem);
var
	name:	String;
begin
	if cbTurnSegments.Items.Count = 0 then
		name := 'Obstacles To KK Line'
	else
		name := 'Turn Segment - ' + IntToStr (cbTurnSegments.Items.Count);

	cbTurnSegments.AddItem (name, reportItem);
end;

function TReportDForm.GetItem (index: Integer): TReportItem;
begin
    result := TReportItem (cbTurnSegments.Items.Objects [index]);
end;

function TReportDForm.GetReportCount (): Integer;
begin
    result := cbTurnSegments.Items.Count;
end;

procedure TReportDForm.SetItem (index: Integer; reportItem: TReportItem);
begin
	cbTurnSegments.Items.Objects [index] := reportItem;
	ShowReport (index);	
end;

procedure TReportDForm.FillStaightItems (listView: TListView; obsInAreaList: TObstacleInAreaList);
var
	i:			Integer;
	lvItem:		TListItem;
	obs:		TObstacleInArea;
	prevDistanceAccuracy:  	Double;
begin
	listView.Items.Clear;

	for i := 0 to obsInAreaList.Count - 1 do
	begin
		obs := obsInAreaList.Item [i];
		obs.Tag := -1;

		lvItem := listView.Items.Add;
		lvItem.Data := obs;

		lvItem.Caption := obs.AixmID;
		lvItem.SubItems.Add (obs.Name);

		if (obs.HPenetration > 0) and (not obs.Ignored) then
		begin
			obs.Tag := FPenetrationBackColor
		end
		else
			obs.Tag := -1;

prevDistanceAccuracy := CConverters[puDistance].Accuracy;

CConverters[puDistance].Accuracy := 0.001;

		lvItem.SubItems.Add (ConvertUnitStr (obs.HPenetration, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.PDGToTop, cdToOuter, puGradient, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.RequiredGrd, cdToOuter, puGradient, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.RequiredTNH, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.TransformY, cdToOuter, puDistance, rtNear));
		lvItem.SubItems.Add (ConvertUnitStr (obs.TransformX, cdToOuter, puDistance, rtNear));
		lvItem.SubItems.Add (ConvertUnitStr (obs.AboveDER, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.MOC, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.RequiredH, cdToOuter, puAltitude, rtCeil));

CConverters[puDistance].Accuracy := prevDistanceAccuracy;

		if obs.Pdg < 0.033 then
			lvItem.SubItems.Add ('3.3')
		else
			lvItem.SubItems.Add (ConvertUnitStr (obs.PDG, cdToOuter, puGradient, rtCeil));

		if obs.DistPDG <= 0 then
		begin
			lvItem.SubItems.Add ('0');
			lvItem.SubItems.Add ('5');
		end
		else
		begin
			lvItem.SubItems.Add (ConvertUnitStr (obs.DistPDG, cdToOuter, puDistance, rtCeil));
			lvItem.SubItems.Add (ConvertUnitStr (obs.DistPDG * obs.Pdg, cdToOuter, puAltitude, rtCeil));
		end;

		if obs.Pdg > GConst.PANSOPS.Constant [dpPDG_Nom].Value then
		begin
			if obs.Ignored then
				lvItem.SubItems.Add ('Close in')
			else
				lvItem.SubItems.Add ('Yes');
		end
		else
			lvItem.SubItems.Add ('No');

		lvItem.SubItems.Add (ConvertUnitStr (obs.GeoAccuracy, cdToOuter, puAltitude, rtNear));
		lvItem.SubItems.Add (ConvertUnitStr (obs.ElevationAccuracy, cdToOuter, puAltitude, rtNear));
	end;
end;

procedure TReportDForm.FillTurnItems (listView: TListView; obsInAreaList: TObstacleInAreaList);
var
	i:			Integer;
	lvItem:		TListItem;
	obs:		TObstacleInArea;
	prevDistanceAccuracy:  	Double;
begin
	listView.Items.Clear;

	for i := 0 to obsInAreaList.Count - 1 do
	begin
		obs := obsInAreaList.Item [i];
		lvItem := listView.Items.Add;
		lvItem.Data := obs;

		lvItem.Caption := obs.AixmID;
		lvItem.SubItems.Add (obs.Name);

		if obs.HPenetration > 0 then obs.Tag := FPenetrationBackColor
		else obs.Tag := -1;

prevDistanceAccuracy := CConverters[puDistance].Accuracy;

CConverters[puDistance].Accuracy := 0.001;

		lvItem.SubItems.Add (ConvertUnitStr (obs.RequiredGrd, cdToOuter, puGradient, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.HPenetration, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.AboveDER, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.TransformX - obs.Dr, cdToOuter, puDistance, rtFloor)); // d0
		lvItem.SubItems.Add (ConvertUnitStr (obs.Dr, cdToOuter, puDistance, rtFloor));
		lvItem.SubItems.Add (ConvertUnitStr (obs.MOC, cdToOuter, puAltitude, rtCeil));
		lvItem.SubItems.Add (ConvertUnitStr (obs.GeoAccuracy, cdToOuter, puAltitude, rtNear));
		lvItem.SubItems.Add (ConvertUnitStr (obs.ElevationAccuracy, cdToOuter, puAltitude, rtNear));

CConverters[puDistance].Accuracy := prevDistanceAccuracy;
	end;
end;

procedure TReportDForm.bCloseClick(Sender: TObject);
begin
	Close;
end;

procedure TReportDForm.FormClose(Sender: TObject; var Action: TCloseAction);
begin
	DeleteGraphic (FObstacleGraphicHandle);

	Action := caHide;
end;

procedure TReportDForm.FormCreate(Sender: TObject);
begin
	FObstacleGraphicHandle := -1;
	FSortAscColumnIndex := -1;
	FSortAsc := 1;

	FObstacleSymbol := TPointSymbol.Create;
	FObstacleSymbol.Color := 255;
	FObstacleSymbol.Size := 5;

	FPenetrationBackColor := RGB (255, 50, 50);

	FFormType := dftStraight;
//	lv.Height := lv.Height + (lv.Top - 7);
//	lv.Top := 7;
	labTurnSegments.Visible := False;
	cbTurnSegments.Visible := False;

	lv.Tag := -1;

	FillControlMUI ();
end;

procedure TReportDForm.lvCompare(
			Sender: TObject;
			Item1,
			Item2: TListItem;
			Data: Integer;
			var Compare: Integer);
var
	strItem1,
	strItem2:		String;
begin

	if FColumnIndex = 0 then
	begin
		strItem1 := Item1.Caption;
		strItem2 := Item2.Caption;
	end
	else
	begin
		strItem1 := Item1.SubItems.Strings [FColumnIndex - 1];
		strItem2 := Item2.SubItems.Strings [FColumnIndex - 1];
	end;

	if FColumnType = 0 then
	begin
		Compare := AnsiCompareStr (strItem1, strItem2);
	end
	else
	begin
		if (strItem1 = '') or (strItem2 = '') then
		begin
			Compare := 0;
			exit;
		end;
		Compare := Sign(StrToFloat (strItem1) - StrToFloat (strItem2));
	end;

	Compare := Compare * FSortAsc;
end;

procedure TReportDForm.lvColumnClick(Sender: TObject;  Column: TListColumn);
begin
	if FSortAscColumnIndex = Column.Index then
		FSortAsc := -1 * FSortAsc;
		
	FColumnType := Column.Tag;
	FColumnIndex := Column.Index;
	FSortAscColumnIndex := Column.Index;
	TListView (Sender).AlphaSort ();
end;

procedure TReportDForm.lvSelectItem (Sender: TObject; Item: TListItem; Selected: Boolean);
begin
	DeleteGraphic (FObstacleGraphicHandle);

	if (Item = nil) or (Selected = False) then
		exit;

	FObstacleGraphicHandle := GUi.DrawPointWithText (
			TObstacleInArea (Item.Data).Prj,
			FObstacleSymbol,
			TObstacleInArea (Item.Data).AixmID);
end;

procedure TReportDForm.cbTurnSegmentsSelect(Sender: TObject);
var
	cb:				TComboBox;
    reportItem:     TReportItem;
	lvi:			TListItem;
begin
	cb := cbTurnSegments;
	if cb.ItemIndex < 0 then
    begin
        editPDG.Text := '';
    	editObtacleCount.Text := '0';        
		exit;
    end;

	reportItem := TReportItem (cb.Items.Objects [cb.ItemIndex]);

	lv.Columns.BeginUpdate;
	lv.Items.BeginUpdate;
	if cb.ItemIndex = 0 then
	begin
		if lv.Tag <> 0 then
		begin
			FillStraightColumns (lv);
			lv.Tag := 0;
		end;
		FillStaightItems (lv, reportItem.ObtacleList);
		editObtacleCount.Text := IntToStr (lv.Items.Count);
	end
	else
	begin
		if lv.Tag <> 1 then
		begin
			FillTurnColumns (lv);
			lv.Tag := 1;
		end;
		FillTurnItems (lv, reportItem.ObtacleList);
		editObtacleCount.Text := IntToStr (lv.Items.Count);		
	end;
	lv.Columns.EndUpdate;
	lv.Items.EndUpdate;

	lvi := nil;
	if lv.Items.Count > 0 then
	begin
		lv.ItemIndex := 0;
		lvi := lv.Items [0];
	end;

    editPDG.Text := ConvertUnitStr (reportItem.PDG, cdToOuter, puGradient, rtCeil);

	lvSelectItem (lv, lvi, True);
end;

procedure TReportDForm.ShowReport (index: Integer);
begin
	cbTurnSegments.ItemIndex := index;
	cbTurnSegmentsSelect (nil);
end;

procedure TReportDForm.lvCustomDrawItem(Sender: TCustomListView;
  Item: TListItem; State: TCustomDrawState; var DefaultDraw: Boolean);
begin
	if TObstacleInArea (Item.Data).Tag = -1 then
		exit;
    sender.Canvas.Font.Color :=  TColor (TObstacleInArea (Item.Data).Tag);
    sender.Canvas.Font.Style := [fsBold];
end;

procedure TReportDForm.SetFormType(vFormType: TDepartureFormType);
begin
	if FFormType = vFormType then
		exit;
	FFormType := vFormType;

	if FFormType = dftStraight then
	begin
//		lv.Height := lv.Height + (lv.Top - 7);
//		lv.Top := 7;
		labTurnSegments.Visible := False;
		cbTurnSegments.Visible := False;
	end
	else
	begin
//		lv.Height := lv.Height - 40;
//		lv.Top := 40;
		labTurnSegments.Visible := True;
		cbTurnSegments.Visible := True;
	end;
end;

procedure TReportDForm.Show;
begin
	inherited;
	cbTurnSegmentsSelect (cbTurnSegments);
end;

procedure TReportDForm.SaveReport (reportFile: TReportFile);
var
	i: Integer;
begin
	if cbTurnSegments.Items.Count = 0 then
		exit;

	//--- begin: Save straight report.
	FillStraightColumns (lvSaveReport);
	FillStaightItems (lvSaveReport, TReportItem (cbTurnSegments.Items.Objects [0]).ObtacleList);

	reportFile.WriteMessage ('');
	if cbTurnSegments.Items.Count = 1 then
		reportFile.WriteMessage ('Obstacle in Straight Segment (Count: ' + IntToStr (lvSaveReport.Items.Count) + ')')
	else
		reportFile.WriteMessage ('Obstacle in Segment To KK'' Line (Count: ' + IntToStr (lvSaveReport.Items.Count) + ')');
	reportFile.WriteMessage ('');

	for i := 0 to lvSaveReport.Items.Count - 1 do
		lvSaveReport.Items [i].Data := Pointer (TObstacleInArea (lvSaveReport.Items [i].Data).Tag);

	reportFile.WriteTab (lvSaveReport);
	//--- end.

	//--- begin: Save turn report.
	if cbTurnSegments.Items.Count > 1 then
	begin
		FillTurnColumns (lvSaveReport);
		FillTurnItems (lvSaveReport, TReportItem (cbTurnSegments.Items.Objects [1]).ObtacleList);

		reportFile.WriteMessage ('');
		reportFile.WriteMessage ('Obstacle in Turn Segment 1 (Count: ' + IntToStr (lvSaveReport.Items.Count) + ')');
		reportFile.WriteMessage ('');

		for i := 0 to lvSaveReport.Items.Count - 1 do
			lvSaveReport.Items [i].Data := Pointer (TObstacleInArea (lvSaveReport.Items [i].Data).Tag);

		reportFile.WriteTab (lvSaveReport);			
	end;
	//--- end.
end;

procedure TReportDForm.FillControlMUI ();
var
	i: Integer;
	captionText: String;
begin
	GMuiLoader.LoadString (captionText, 129);
	GMuiLoader.LoadString (labTurnSegments, 401);
	GMuiLoader.LoadString (labObstacleCount, 402);
	GMuiLoader.LoadString (labPDG, 415, ':');
	GMuiLoader.LoadString (bSave, 130);
	GMuiLoader.LoadString (bClose, 131);

	for i := 0 to 16 do
		GMuiLoader.LoadString (FColumnHeaders [i], 404 + i);


	if (Length (captionText) > 0) and (captionText [1] = '&') then
		captionText := MidStr(captionText, 2, Length (captionText) - 1);

	self.Caption := captionText;
end;

procedure TReportDForm.bHelpClick(Sender: TObject);
begin
	ShowHelp (self.Handle, 2400);
end;

function TReportDForm.FormHelp (Command: Word; Data: Integer; var CallHelp: Boolean): Boolean;
begin
	CallHelp := False;
	bHelpClick (nil); 
end;

end.
