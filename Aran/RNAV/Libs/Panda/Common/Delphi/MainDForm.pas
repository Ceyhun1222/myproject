unit MainDForm;

{$DEFINE DISABLE_SAVE}

{ $DEFINE TEST_PARAMS}
{ $DEFINE SHOW_MESSAGEBOX}
{ $DEFINE NO_LICENSE_CHECK}	//Also in DepartureService.pas

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls,
  Forms, Dialogs, Grids, StdCtrls, Buttons, ComCtrls, AIXMTypes,
  ObjectDirectoryContract, ConstantsContract, ExtCtrls,
  CollectionUnit, Mask, ARANFunctions, GeometryOperatorsContract,
  UIContract, Geometry, DepartureClasses,
  ReportForm, ARANClasses, UnitConverter, Parameter,
  ParameterContainerFrame, DepartureGlobals, IntervalUnit,
  FunctionCallManager, FunctionParams, ReportUnit_, Common, SaveDForm,
  MUILoader;

type
	TDeparturePages = (dpgParameters, dpgStraight, dpgTurn);

  TMainForm = class(TForm)
    procedure bHelpClick(Sender: TObject);
    function FormHelp(Command: Word; Data: Integer;
      var CallHelp: Boolean): Boolean;
    procedure FormKeyUp(Sender: TObject; var Key: Word;
      Shift: TShiftState);
  private
	FParams:			    TParameters;
	FStraight:			    TStraight;
	FTurn:				    TTurn;
	panelMain:			    TPanel;
	FCurrentPage:		    TDeparturePages;
	FReportForm:		    TReportDForm;
	FFormType:			    TDepartureFormType;
	FNextPointGraphic:      TGraphicItem;
	FNextLineGraphic:	    TGraphicItem;
	FLastNextFiltered:	    TInterval;
	FOldNextFilterParams:   TFilterNextPointsParams;
	FNextFilterParams:      TFilterNextPointsParams;
    FDefaultNominalPDG:     Double;
	FDefaultProtectPDG:     Double;
	prevTurnType:           TTurnType;
	FCallManager:         	TFunctionCallManager;
	FPrevStraightAreaParams:    TCreateStraightAreaParams;
	FParamControlUpdateList:	TList;
	FLockedUpdateParamControl:	Boolean;
	FAhpUnicalNameList: TPandaList;
	FApplicationHandle: Integer;

//--- OldValues
	FOldStartChangedParam:	TPointParam;
	FOldNextChangedParam:	TPointParam;
{$IFDEF SHOW_MESSAGEBOX}
	FDebugForm:				TForm;
{$ENDIF}
  published
	procedure FormCreate (Sender: TObject);
	procedure FormClose (Sender: TObject; var Action: TCloseAction);
  private
//--- Init
	procedure OldValuesInit ();
	procedure OldValuesFree ();
	procedure FillControlMUI ();
//--- Common
	procedure DeleteAllGraphics ();
	procedure SetPanels ();
	procedure ChangeParent (oldParent, newParent, control: TWinControl);
	procedure HandleARANError (e: EARANError);
	procedure EnableControl (control: TControl; isEnable: Boolean);
	procedure LoadUISettings ();
	procedure SetUIDefaultValues ();
	procedure AddParamControlUpdateList (parameterControl: TParameter);
	procedure UpdateParameterControl (parameter: TFunctionParam);
	function GetSignificantPoint (id: String): TSignificantPoint;
//--- Parameters
	procedure ParametersToStraightPage ();
	procedure ParametersToTurnPage ();
	procedure FromParameters ();
	procedure FillAerodromes ();
	procedure SetMagneticVariation (magVar: Double);
//--- Straight
	procedure CreateStraightArea (param: TFunctionParam);
//--- Turn
	procedure CreateStraightAreaOnTurnPage ();
	procedure CreateTurnArea ();
	procedure SetRwyEarlestLine ();
	procedure TurnTypeChanged ();
	procedure CodeTypeChanged ();
	procedure GetFilterNextPointsParams ();
	procedure FilterStartPoints (param: TFunctionParam);
	procedure FilterNextPoints (param: TFunctionParam);
	procedure ExtendSegmentForAtHeight ();
	function GetStartMinDistance (vHeight, pdg: Double): Double;
	function GetNextMinDistance (): Double;
	procedure ClearCalculatedInfo ();
	procedure UpdateReportWindow ();
	//--- State
	procedure StartPointSelectTypeState ();
	procedure NextPointSelectTypeState ();
	procedure TurnTypeState ();
	procedure CodeTypeState ();
	//--- Set
	procedure SetProtectRadius (val: Double);
	procedure SetTurnSideAndAngle ();
//	procedure SetNextMinDistance (param: TFunctionParam);
	//--- OnChanged
	procedure StartPointChanged_ (parameter: TFunctionParam);
	procedure NextPointChanged_ (parameter: TFunctionParam);
	procedure StartOrNextPointChanged ();
    procedure OnBankAngleChanged ();
    procedure OnAircraftCategoryChanged ();
	procedure OnHeightChanged ();
	procedure HeightNominalChanged (newValue: Double);
	procedure PDGNominalChanged (newValue: Double);
	procedure PDGProtectChanged (newValue: Double);
	procedure OnHeightProtectChanged ();
	procedure OnPrevPDGChanged ();
{$IFDEF SHOW_MESSAGEBOX}
	procedure AddLogMessage (messageStr: String);
{$ENDIF}
  published
    cbTurnCategory: TComboBox;
    labCategory: TLabel;
	grbPrevPDG: TGroupBox;
    pcTurnPrevPDG: TParamContainer;
    pcTurnPrevReqPDG: TParamContainer;
    grbNextPDG: TGroupBox;
	pcTurnCurPDG: TParamContainer;
    pcTurnCurReqPDG: TParamContainer;  
	pcModelAreaRadius: TParamContainer;
	pcAerodrome: TParamContainer;
	pcRwyDirElev: TParamContainer;
	prRwyDirClearway: TParamContainer;
	pcStraightCourse: TParamContainer;
	pcStraightDistance: TParamContainer;
	pcIAS: TParamContainer;
	pcBankAngle: TParamContainer;
	pcTAS: TParamContainer;
	pcTurnRadius: TParamContainer;
    pcHeightProtect: TParamContainer;
	pcTurnStartCourse: TParamContainer;
	pcTurnStartDistance: TParamContainer;
	pcTurnNextDistance: TParamContainer;
	pcTurnNextCourse: TParamContainer;
	grbDepartureType: TGroupBox;
	rbDepartureTypeStaight: TRadioButton;
	rbDepartureTypeTurn: TRadioButton;
	labTurnStartSignifigantPointsUnit: TLabel;
	cbTurnStartSignificantPoints: TComboBox;
	rbTurnStartSignificantPoints: TRadioButton;
	rbTurnStartCreatePoint: TRadioButton;
	grbTurnType: TGroupBox;
	rbTurnTypeFlyBy: TRadioButton;
	rbTurnTypeFlyOver: TRadioButton;
	rbTurnTypeAtHeight: TRadioButton;
	grbTurnCodeType: TGroupBox;
	rbTurnCodeTypeTF: TRadioButton;
	rbTurnCodeTypeCF: TRadioButton;
	rbTurnCodeTypeDF: TRadioButton;
	grbStraightPoint: TGroupBox;
	labStraightSgfPointType: TLabel;
	cbStraightSgfPoint: TComboBox;
	rbStraigthSelectPoint: TRadioButton;
	rbStraigthCreatePoint: TRadioButton;
    gbSensorType: TGroupBox;
    rbSensorTypeDME: TRadioButton;
    rbSensorTypeGNSS: TRadioButton;
	pageContMain: TPageControl;
	tabParameters: TTabSheet;
	tabStraight: TTabSheet;
	panelParameters: TPanel;
	grbAerodrome: TGroupBox;
	labAerodromes: TLabel;
	Label2: TLabel;
	labAerodromeMagVar: TLabel;
	labAerodromeMagVarUnit: TLabel;
	labAerodromeISA: TLabel;
	labAerodromeISAUnit: TLabel;
	editAerodromeAixmID: TEdit;
	cbAerodromes: TComboBox;
	editAerodromeMagVar: TEdit;
	editAerodromeISA: TEdit;
	grbRWY: TGroupBox;
	labRWYName: TLabel;
	cbRWY: TComboBox;
	grbRwyDirection: TGroupBox;
	labRwyDirectionName: TLabel;
	labRwyDirectionTrueBrg: TLabel;
	labRwyDirectionUnit: TLabel;
	labRwyDirectionMagBrg: TLabel;
	labRwyDirectionMagBrgUnit: TLabel;
	cbRwyDirection: TComboBox;
	editRwyDirectionTrueBrg: TEdit;
	editRwyDirectionMagBrg: TEdit;
	grbSignificantPoint: TGroupBox;
	labSignificantPointName: TLabel;
	labSignificantPointType: TLabel;
	cbSignificant: TComboBox;
	panelStraight: TPanel;
	labStraightInfoElevGrd: TLabel;
	labStraightInfoElevGrdUnit: TLabel;
	labStaightInfoAdjustment: TLabel;
	labStaightInfoAdjustmentUnit: TLabel;
	labStaightInfoMOCObsName: TLabel;
	editStraightInfoElevGrd: TEdit;
	editStaightInfoAdjustment: TEdit;
	editStaightInfoMOCObsName: TEdit;
	panelButtons: TPanel;
	bPrev: TButton;
	bNext: TButton;
	bClose: TButton;
	bReport: TButton;
	bHelp: TButton;
	tabTurn: TTabSheet;
	panelTurn: TPanel;
	grbTurnAircraftParams: TGroupBox;
	grbTurnStartPoint: TGroupBox;
	grbTurnNextPoint: TGroupBox;
	cbTurnNextSignificantPoints: TComboBox;
	rbTurnNextSignificantPoints: TRadioButton;
	labTurnNextSignifigantPointsUnit: TLabel;
	rbTurnNextCreatePoint: TRadioButton;
	cbTurnDFSide: TComboBox;
	labTurnDFSide: TLabel;
	bApply: TButton;
    grbKKLine: TGroupBox;
	pcTurnKKHeight: TParamContainer;
    pcTurnKKDistance: TParamContainer;
    pcStraightReqPDG: TParamContainer;
    pcStraightPDG: TParamContainer;
    grbDME: TGroupBox;
    cbDMEInstalled: TComboBox;
    labDMEInstalled: TLabel;
	labDMECount: TLabel;
	cbDMECount: TComboBox;
	saveDlgReport: TSaveDialog;
	bSave: TButton;

	procedure bSaveClick(Sender: TObject);
	procedure SensorTypeClick(Sender: TObject);
	procedure pcTurnKKHeightPChangeValue(Sender: TCustomParameter);
	procedure pcStraightPDGPChangeValue(Sender: TCustomParameter);
	procedure pcTurnStartCoursePChangeValue(Sender: TCustomParameter);
	procedure pcTurnNextCoursePChangeValue(Sender: TCustomParameter);
	procedure pcTurnNextDistancePChangeValue(Sender: TCustomParameter);
	procedure bApplyClick(Sender: TObject);
	procedure bNextClick(Sender: TObject);
	procedure bPrevClick(Sender: TObject);
	procedure cbAerodromesSelect(Sender: TObject);
	procedure bCloseClick(Sender: TObject);
	procedure cbSignificantSelect(Sender: TObject);
	procedure cbRWYSelect(Sender: TObject);
	procedure cbRwyDirectionSelect(Sender: TObject);
	procedure cbStraightSgfPointSelect(Sender: TObject);
	procedure rbStraigthCreateSelectPointClick(Sender: TObject);
	procedure bReportClick(Sender: TObject);
	procedure cbTurnCategorySelect(Sender: TObject);
	procedure cbTurnNextSignificantPointsSelect(Sender: TObject);
	procedure rbTurnNextPointClick(Sender: TObject);
	procedure TurnTypeClick(Sender: TObject);
	procedure TurnCodeTypeClick(Sender: TObject);
	procedure rbTurnStartPointClick (Sender: TObject);
	procedure cbTurnStartSignificantPointsSelect(Sender: TObject);
	procedure pcIASPChangeValue(Sender: TCustomParameter);
	procedure pcModelAreaRadiusPChangeValue(Sender: TCustomParameter);
	procedure pcStraightCoursePChangeValue(Sender: TCustomParameter);
	procedure pcStraightDistancePChangeValue(Sender: TCustomParameter);
	procedure rbDepartureTypeClick(Sender: TObject);
	procedure pcBankAnglePChangeValue(Sender: TCustomParameter);
	procedure pcHeightProtectPChangeValue (Sender: TCustomParameter);
	procedure pcTurnStartDistancePChangeValue(Sender: TCustomParameter);
	procedure pcTurnPrevPDGPChangeValue (Sender: TCustomParameter);
	procedure pcTurnCurPDGPChangeValue (Sender: TCustomParameter);
	procedure cbTurnDFSideSelect(Sender: TObject);
	procedure ReportForm_bSaveClick (Sender: TObject);
  end;

implementation

uses
	Math, DepartureService, ARANMath, ARANGlobals, Contract, AIXM_TLB,
  ConvUtils, SettingsContract;

{$R *.dfm}


procedure TMainForm.ChangeParent (oldParent, newParent, control: TWinControl);
begin
	control.Visible := False;
	control.Left := 1;
	control.Top := 1;
	control.Width := newParent.Width - 10;
	control.Height := newParent.Height - 30;
	control.Anchors := [akTop, akLeft, akRight, akBottom];
	oldParent.RemoveControl (control);
	newParent.InsertControl (control);
end;

procedure TMainForm.SetPanels ();
begin
	panelMain := TPanel.Create(self);
	panelMain.Left := pageContMain.Left;
	panelMain.Top := pageContMain.Top;
	panelMain.Width := pageContMain.Width;
	panelMain.Height := pageContMain.Height - 15;
	panelMain.Anchors := [akTop, akLeft, akRight, akBottom];
	pageContMain.Visible := False;
	InsertControl (panelMain);

	panelButtons.Top := panelButtons.Top - 5;

	ChangeParent (tabParameters, panelMain, panelParameters);
	ChangeParent (tabStraight, panelMain, panelStraight);
	ChangeParent (tabTurn, panelMain, panelTurn);	
end;

procedure TMainForm.LoadUISettings ();
begin
	labStraightInfoElevGrdUnit.Caption := CConverters [puAltitude].UnitName;
	labTurnStartSignifigantPointsUnit.Caption := '';
	labTurnNextSignifigantPointsUnit.Caption := '';
end;

procedure TMainForm.SetUIDefaultValues ();
begin
	Caption := 'Departure - Parameters';
    labStraightSgfPointType.Caption := '';
end;

{$IFDEF SHOW_MESSAGEBOX}
procedure TMainForm.AddLogMessage (messageStr: String);
begin
	TMemo(FDebugForm.Controls [0]).Text :=
		TEdit(FDebugForm.Controls [0]).Text + #13 + #10 + 
		messageStr;
end;
{$ENDIF}

procedure TMainForm.FormCreate(Sender: TObject);
var
	MainProgram: String;
	info0, info1, info2, info3: Integer;
	tmpPart: TPart;
	tmpPoint: TPoint;
	appFileName: String;
{$IFDEF SHOW_MESSAGEBOX}
	editBox:		TMemo;
{$ENDIF}
	startupPath: String;
begin

	if DecimalSeparator = ',' then
		DecimalSeparator := '.';

	appFileName := GetModuleName (HInstance);
	startupPath := ExtractFilePath (appFileName);
	//Application.HelpFile := startupPath + 'RNAV.chm';
	Application.HelpFile := 'RNAV.chm';

	GMuiLoader := TMUILoader.Create (startupPath + 'DepartureRes.dll');
	GMuiLoader.LangCode := Integer (GAranSettings.GetLanguageCode ());
	GMuiStrings := TMuiStrings.Create;

	FillControlMUI ();
	LoadUISettings ();

	SetPanels ();

{$IFDEF SHOW_MESSAGEBOX}
	FDebugForm := TForm.Create (Self);
	editBox := TMemo.Create (FDebugForm);
	editBox.Left := 5;
	editBox.Top := 5;
	editBox.Width := FDebugForm.Width  - 25;
	editBox.Height := FDebugForm.Height - 25;
	editBox.WantReturns := True;
	editBox.WantTabs := True;
	editBox.Parent := FDebugForm;
	FDebugForm.Show ();
{$ENDIF}

	FNextFilterParams := TFilterNextPointsParams.Create;
	FOldNextFilterParams := TFilterNextPointsParams.Create;
	FCallManager := TFunctionCallManager.Create;
	FParamControlUpdateList := TList.Create;
	FCallManager.DefaultThis := Self;
	FAhpUnicalNameList := nil;


	GAixmProcedure := CoStandardInstrumentDeparture.Create () as IAIXMProcedure;


	SetUIDefaultValues ();

	OldValuesInit ();

	FCurrentPage := dpgParameters;
	FFormType := dftStraight;
	panelParameters.Visible := True;
	FDefaultProtectPDG := 0.1;
	FDefaultNominalPDG := 0.07;
	FLockedUpdateParamControl := False;
	prevTurnType := tuFlyBy;

    FPrevStraightAreaParams := TCreateStraightAreaParams.Create ();

	FParams := TParameters.Create();
	FParams.UI := GUI;

	FStraight := TStraight.Create (FParams);

	FTurn := TTurn.Create (FParams, FStraight);

	FTurn.SegmentSymbol.SymbolPrimary.Style := sfsNull;
	FTurn.SegmentSymbol.SymbolPrimary.Outline.Color := RGB (255, 0, 0);
	FTurn.SegmentSymbol.SymbolAddition.Style := sfsNull;
	FTurn.SegmentSymbol.SymbolAddition.Outline.Color := RGB (0, 0, 255);
	FTurn.SegmentSymbol.SymbolNominal.Color := RGB (0, 0, 0);
	FTurn.SegmentSymbol.SymbolCL.Color := RGB (50, 50, 50);
	FTurn.SegmentSymbol.SymbolEarlestLine.Color := RGB (0, 0, 200);
	FTurn.SegmentSymbol.SymbolLatestLine.Color := RGB (200, 0, 0);

	FNextPointGraphic := TGraphicItem.Create;
	FNextPointGraphic.Symbol := TPointSymbol.Create;
	FNextPointGraphic.Symbol.Color := RGB (0, 0, 255);

	FNextLineGraphic := TGraphicItem.Create;
	FNextLineGraphic.Symbol := TLineSymbol.Create;
	FNextLineGraphic.Symbol.Color := RGB (0, 0, 255);
	FNextLineGraphic.Geometry := TPolyline.Create;
	tmpPart := TPart.Create;
	tmpPoint := TPoint.Create;
	tmpPart.AddPoint (tmpPoint);
	tmpPart.AddPoint (tmpPoint);
	tmpPoint.Free;	
	FNextLineGraphic.Geometry.AsPolyline.AddPart (tmpPart);
	tmpPart.Free;

	pageContMain.ActivePageIndex := 0;

	FReportForm := TReportDForm.Create (Self);
 //	GUi.GetEnvInfo (MainProgram, info0, info1, info2, info3);
	//FApplicationHandle := info2;
	//if IsWindow(info2) then
	//	SetWindowLong(FReportForm.Handle, GWL_HWNDPARENT, FApplicationHandle);
	FReportForm.bSave.OnClick := ReportForm_bSaveClick;


	FLastNextFiltered.Left := GNullRadianValue;
	FLastNextFiltered.Right := GNullRadianValue;

	pcModelAreaRadius.P.ChangeValue (100000, False);

	FillAerodromes ();

{$IFDEF TEST_PARAMS}
	rbDepartureTypeTurn.Checked := True;
	rbDepartureTypeClick (rbDepartureTypeTurn);
	cbAerodromes.ItemIndex := 5;
	cbAerodromesSelect (cbAerodromes);
	cbRwyDirection.ItemIndex := 1;
	cbRwyDirectionSelect (cbRwyDirection);
	bNextClick (bNext);
{$ENDIF}
end;

procedure TMainForm.FormClose(Sender: TObject; var Action: TCloseAction);
var
    i:  Integer;
begin
	DeleteAllGraphics;

	//--- begin: Free.
	if FReportForm <> nil then
	begin
		for i:=0 to FReportForm.ReportCount - 1 do
			FReportForm.GetItem (i).Free;
		FReportForm.Free;
	end;
	FParams.Free;
	FStraight.Free;
	FTurn.Free;
	FNextFilterParams.Free;
	FOldNextFilterParams.Free;
	FParamControlUpdateList.Free;
	FNextPointGraphic.Symbol.Free;
	FNextPointGraphic.Free;
	FNextLineGraphic.Geometry.Free;
	FNextLineGraphic.Symbol.Free;
	FNextLineGraphic.Free;
	FCallManager.Free;
	FPrevStraightAreaParams.Free;
	FAhpUnicalNameList.Free;
	//--- end.

	OldValuesFree ();

	Action := caFree;

	OnFormClose (0);


 //	FreeLibrary (GMuiLoader.Module);
	GMuiLoader.Free;
	GMuiStrings.Free;
end;

procedure TMainForm.DeleteAllGraphics;
begin
	FTurn.SegmentSymbol.EraseArea;
end;

procedure TMainForm.bNextClick(Sender: TObject);
begin
	GUi.ShowAnimation ();

	try
		if FFormType = dftStraight then
		begin
			FCurrentPage := dpgStraight;

			ParametersToStraightPage ();

			Caption := GMuiStrings.Departure + ' - ' + GMuiStrings.Straight;
			panelParameters.Visible := False;
			panelStraight.Visible := True;
		end
		else if FFormType = dftMultiTurn then
		begin
			FCurrentPage := dpgTurn;

			ParametersToTurnPage ();

			Caption := GMuiStrings.Departure + ' - ' + GMuiStrings.Turn;
			panelStraight.Visible := False;
			panelTurn.Visible := True;
		end;
	except
		on e: EARANError do HandleARANError (e);
	end;

	bNext.Enabled := False;
	bPrev.Enabled := True;

	GUi.HideAnimation ();
end;

procedure TMainForm.ParametersToStraightPage ();
var
	rwyDir:		Double;
	i:			Integer;
begin
	FromParameters ();

	rwyDir := FParams.StartRwyDirection.Direction;

	pcStraightCourse.P.BeginUpdate (False);
	pcStraightCourse.P.OutConvertionPoint := FParams.StartRwyDirection.Prj.Clone.AsPoint;
	pcStraightCourse.P.InConvertionPoint := FParams.StartRwyDirection.Geo.Clone.AsPoint;
	pcStraightCourse.P.MinValue := rwyDir - GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;
	pcStraightCourse.P.MaxValue := rwyDir + GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;
	pcStraightCourse.P.Value := rwyDir;
	pcStraightCourse.P.EndUpdate;

    FTurn.TurnInfo.Start.Distance := 56000.0;
	pcStraightDistance.P.ChangeValue (FTurn.TurnInfo.Start.Distance, False);

    FTurn.TurnInfo.PDG := GConst.PANSOPS.Constant [dpPDG_Nom].Value;
    pcStraightPDG.P.ChangeValue (FTurn.TurnInfo.PDG, False);

	FStraight.FillSignificantPointByArea (FParams.SignificantPoints, rwyDir);

	cbStraightSgfPoint.Clear;
	for i:=0 to FStraight.SignificantPointsByArea.Count - 1 do
	begin
		cbStraightSgfPoint.AddItem (
			FStraight.SignificantPointsByArea.Item [i].AixmID,
			FStraight.SignificantPointsByArea.Item [i]);
	end;

	if cbStraightSgfPoint.Items.Count > 0 then
		cbStraightSgfPoint.ItemIndex := 0;


	rbStraigthSelectPoint.Caption :=
		GMuiStrings.SignificantPoints +
		' (' + IntToStr (cbStraightSgfPoint.Items.Count) + ')';
    rbStraigthSelectPoint.Enabled := (cbStraightSgfPoint.Items.Count > 0);

    //To accur update event.
    FTurn.TurnInfo.Start.Distance := 0;

	rbStraigthCreatePoint.Checked := True;
	rbStraigthCreateSelectPointClick (nil);
end;

procedure TMainForm.bPrevClick(Sender: TObject);
var
    showParameterPage:  Boolean;
begin
    showParameterPage := True;

    if FCurrentPage = dpgTurn then
    begin
        if FTurn.TurnInfoList.Count > 0 then
        begin
			showParameterPage := False;
        end;
    end;


    if showParameterPage then
	begin
		FTurn.SegmentSymbol.EraseArea;
		FNextPointGraphic.Erase;
		FNextLineGraphic.Erase;

		Caption := GMuiStrings.Departure + ' - ' + GMuiStrings.Parameters;
	    FCurrentPage := dpgParameters;
    	panelStraight.Visible := False;
        panelTurn.Visible := False;
    	panelParameters.Visible := True;
    	bNext.Enabled := True;
	    bPrev.Enabled := False;
    end;

	bApply.Enabled := False;
	bReport.Enabled := False;
	bSave.Enabled := False;
end;

procedure TMainForm.FillAerodromes ();
var
	i: Integer;
	unicalName: TUnicalName;
	icaoPrefixArray: TStringArray;
	icaoPrefixList: TStringList;
begin
	cbAerodromes.Clear;

	if not GObjectDirectory.isValid then exit;

 //	icaoPrefixList := GAranSettings.GetDBAirportFilterList ();
 //	SetLength (icaoPrefixArray, icaoPrefixList.Count);
 	SetLength (icaoPrefixArray, 0);
 //	for i := 0 to icaoPrefixList.Count - 1 do
	 //	icaoPrefixArray [i] := icaoPrefixList.Strings [i];

	if FAhpUnicalNameList <> nil then
		FAhpUnicalNameList.Free;
	FAhpUnicalNameList := GObjectDirectory.GetAerodromeList (icaoPrefixArray).Clone ();

	for i := 0 to FAhpUnicalNameList.Count - 1 do
	begin
{$IFNDEF NO_LICENSE_CHECK}
 //		if IsPointInPoly (ahpList.Item[i].Prj, GLicenseRect) then
{$ENDIF}
		unicalName := TUnicalName (FAhpUnicalNameList.Item [i]);
		cbAerodromes.AddItem (unicalName.Tag + ' (' + unicalName.Name + ')', unicalName);
	end;

	if cbAerodromes.Items.Count > 0 then
	begin
{$IFDEF TEST_PARAMS}
		cbAerodromes.ItemIndex := 2;
{$ELSE}
		cbAerodromes.ItemIndex := 0;
{$ENDIF}
	end;

    pcModelAreaRadiusPChangeValue (nil);
end;

procedure TMainForm.cbAerodromesSelect(Sender: TObject);
var
	i: Integer;
	rwyUnicalNameList: TPandaList;
	//tmpGeo: TGeometry;
	ahpUnicalName: TUnicalName;
begin
	cbRWY.Clear;
	cbRwyDirection.Clear;
	cbSignificant.Items.Clear;

	if cbAerodromes.ItemIndex < 0 then exit;

	ahpUnicalName := TUnicalName (cbAerodromes.Items.Objects [cbAerodromes.ItemIndex]);

	FParams.AHP := GObjectDirectory.AHP [ahpUnicalName.Id];

{$IFNDEF NO_LICENSE_CHECK}
// i check this as comment.we must look again this
    {
	if not IsPointInPoly (FParams.AHP.Prj, GLicenseRect) then
	begin
		ShowError ('Selected airport not in license area!');
		bNext.Enabled := False;
		exit;
	end;
     }
  //end
	bNext.Enabled := True;
{$ENDIF}	

	if FParams.AHP.ID = '-1' then
	begin
		bNext.Enabled := False;
		raise EARANError.Create ('AHP not loaded', mtError);
	end;
	bNext.Enabled := True;

	//if Abs (FParams.AHP.Geo.X - GPrjSR.Param [srptCentralMeridian]) > 3 then
    //begin
    //    if MessageDlg (
    //        'Difference between given central meridian of projection coordinate systems (' +
    //        FormatFloat ('#.00', GPrjSR.Param [srptCentralMeridian]) + ') and selected aerodrome''s reference point (' +
    //        FormatFloat ('#.00', FParams.AHP.Geo.X) + ') greater than 3 degree.' + #13 +
    //        'Do you want to change central meridian to aerodrome longitude?', mtConfirmation, [mbYes, mbNo, mbCancel], 0)
    //        <> mrYes then
    //    begin
    //        bNext.Enabled := False;
    //        exit;
    //    end;

    //    //tmpGeo := PrjToGeo (GLicenseRect);

    //    GPrjSR.Param [srptCentralMeridian] := FParams.AHP.Geo.X;
    //    GUi.SetViewProjection (GPrjSR);

    //    //tmpGeo2 := GeoToPrj (tmpGeo);
    //    //GLicenseRect.Assign (tmpGeo2);
    //    //tmpGeo2.Free;
    //    //tmpGeo.Free;

    //    {
    //    for i := 0 to cbAerodromes.Items.Count - 1 do
    //    begin
    //        tmpAhp := TAHP (cbAerodromes.Items.Objects [i]);
    //        tmpGeo := GeoToPrj (tmpAhp.Geo).AsPoint;
    //        tmpAhp.Prj.Assign (tmpGeo);
    //        tmpGeo.Free;
    //    end;
    //    }

    //    GObjectDirectory.setToSpatialReference (GPrjSR);

    //    {MessageDlg (
    //        'Difference between given central meridian of projection coordinate systems and ' +
    //        'selected aerodrome''s reference point greater than 3 degree.' + #13 +
    //        'Please, change projection coordinate system and restart application.', mtWarning, [mbOk], 0);
    //    bNext.Enabled := False;
	//    exit;}
	//end;

	bNext.Enabled := True;

	SetMagneticVariation (FParams.AHP.MagVar);
	GMagneticVariation := FParams.AHP.MagVar;

	FParams.MagVar := FParams.AHP.MagVar;

	editAerodromeAixmID.Text := FParams.AHP.AixmID;
	pcAerodrome.P.ChangeValue (FParams.AHP.Elevation);
	editAerodromeMagVar.Text := RoundToStr (FParams.AHP.MagVar, 0.1, rtNear);
	editAerodromeISA.Text := RoundToStr (FParams.AHP.Temperature, 1, rtNear);

	GUi.ShowAnimation ();

	rwyUnicalNameList := GObjectDirectory.GetRWYList (FParams.AHP.AixmID);

	for i := 0 to rwyUnicalNameList.Count - 1 do
		cbRWY.AddItem (
			TUnicalName (rwyUnicalNameList.Item [i]).Name,
			TUnicalName (rwyUnicalNameList.Item [i]));

	if cbRWY.Items.Count > 0 then
		cbRWY.ItemIndex := 0;

	{for i := 0 to FParams.SignificantPoints.Count - 1 do
		cbSignificant.AddItem (
			FParams.SignificantPoints.Item [i].AixmID,
			FParams.SignificantPoints.Item [i]);

	if cbSignificant.Items.Count > 0 then
		cbSignificant.ItemIndex := 0;}

    cbRWYSelect (cbRWY);
	cbSignificantSelect (cbSignificant);

	GUi.HideAnimation ();
end;

procedure TMainForm.SetMagneticVariation (magVar: Double);
begin
	pcStraightCourse.P.Offset := magVar;
	pcTurnNextCourse.P.Offset := magVar;
	pcTurnStartCourse.P.Offset := magVar;
end;

procedure TMainForm.bCloseClick(Sender: TObject);
begin
	self.Close;
end;

procedure TMainForm.cbSignificantSelect(Sender: TObject);
begin
	if cbSignificant.ItemIndex < 0 then
	begin
		labSignificantPointType.Caption := '';
		exit;
	end;
	labSignificantPointType.Caption :=
		AIXMTypeNames [TSignificantPoint (cbSignificant.Items.Objects [cbSignificant.ItemIndex]).AIXMType];
end;

procedure TMainForm.cbRWYSelect(Sender: TObject);
var
	i:			Integer;
	tempPoint:	TPoint;
	x:			Double;
	rwyDirectionList:	TRWYDirectionList;
	rwyUnicalName: TUnicalName;
	rwy: TRWY;
begin
	cbRwyDirection.Clear;

	if cbRWY.ItemIndex < 0 then
	begin
	    cbRwyDirectionSelect (nil);
		exit;
	end;

	rwyUnicalName := TUnicalName (cbRWY.Items.Objects [cbRWY.ItemIndex]);
	rwy := GObjectDirectory.RWY [rwyUnicalName.Id];
	rwyDirectionList := GObjectDirectory.RWYDirectionList [rwy.AixmID, FParams.AHP];

	if rwyDirectionList.Count <> 2 then
	begin
		cbRwyDirectionSelect (nil);	
		bNext.Enabled := False;
		exit;
	end;

	bNext.Enabled := True;

	tempPoint := rwyDirectionList.Item [0].Geo.Clone.AsPoint;
	rwyDirectionList.Item [0].Geo.Assign (rwyDirectionList.Item [1].Geo);
	rwyDirectionList.Item [1].Geo.Assign (tempPoint);
	tempPoint.Free;

	tempPoint := rwyDirectionList.Item [0].Prj.Clone.AsPoint;
	rwyDirectionList.Item [0].Prj.Assign (rwyDirectionList.Item [1].Prj);
	rwyDirectionList.Item [1].Prj.Assign (tempPoint);
	tempPoint.Free;

	x := rwyDirectionList.Item [0].elevTdz;
	rwyDirectionList.Item [0].elevTdz := rwyDirectionList.Item [1].elevTdz;
	rwyDirectionList.Item [1].elevTdz := x;

	x := rwyDirectionList.Item [0].elevTdzAccuracy;
	rwyDirectionList.Item [0].elevTdzAccuracy := rwyDirectionList.Item [1].elevTdzAccuracy;
	rwyDirectionList.Item [1].elevTdzAccuracy := x;

	for i := 0 to rwyDirectionList.Count - 1 do
	begin
		tempPoint := PointAlongGeodesic (rwyDirectionList.Item [i].Geo,
										rwyDirectionList.Item [i].trueBearing,
										rwyDirectionList.Item [i].clearway);
		rwyDirectionList.Item [i].Geo.Assign (tempPoint);
		tempPoint.Free;
		tempPoint := GeoToPrj (rwyDirectionList.Item [i].Geo).AsPoint;
		rwyDirectionList.Item [i].Prj.Assign (tempPoint);
		tempPoint.Free;

		cbRwyDirection.AddItem (rwyDirectionList.Item[i].Name, rwyDirectionList.Item[i]);
	end;

	if cbRwyDirection.Items.Count > 0 then
		cbRwyDirection.ItemIndex := 0;

	cbRwyDirectionSelect (cbRwyDirection);
end;

procedure TMainForm.cbRwyDirectionSelect(Sender: TObject);
var
	rwyDirection:	TRWYDirection;
begin
	if (cbRwyDirection.Items.Count = 0) or (cbRwyDirection.ItemIndex = -1) then
	begin
		editRwyDirectionTrueBrg.Text := '';
		editRwyDirectionMagBrg.Text := '';
		pcRwyDirElev.ValueControl.Text := '';
		prRwyDirClearway.ValueControl.Text := '';
		exit;
	end;

	rwyDirection := TRWYDirection (cbRwyDirection.Items.Objects [cbRwyDirection.ItemIndex]);

	editRwyDirectionTrueBrg.Text := RoundToStr (rwyDirection.TrueBearing, GAngleAccuracy, rtNear);
	editRwyDirectionMagBrg.Text := RoundToStr (rwyDirection.MagBearing, GAngleAccuracy, rtNear);

	pcRwyDirElev.P.ChangeValue (rwyDirection.ElevTdz);
	prRwyDirClearway.P.ChangeValue (rwyDirection.Clearway);
end;

procedure TMainForm.cbStraightSgfPointSelect(Sender: TObject);
var
	distance,
	direction:		Double;
	tmpPoint:		TPoint;
	sigPoint:		TSignificantPoint;
begin

	if (cbStraightSgfPoint.ItemIndex < 0) or (not rbStraigthSelectPoint.Checked) then
	begin
		labStraightSgfPointType.Caption := '';
		exit;
	end;

	sigPoint := TSignificantPoint (cbStraightSgfPoint.Items.Objects [cbStraightSgfPoint.ItemIndex]);
	labStraightSgfPointType.Caption := AIXMTypeNames [sigPoint.AIXMType];

	distance := ReturnDistanceInMeters (FParams.StartRwyDirection.Prj, sigPoint.Prj);
	direction := ReturnAngleInRadians (FParams.StartRwyDirection.Prj, sigPoint.Prj);

	FTurn.TurnInfo.Start.SignificantPointID := sigPoint.ID;
	FTurn.TurnInfo.Start.Direction := direction;
	FTurn.TurnInfo.Start.Distance := distance;
	FTurn.TurnInfo.Start.HeightToKK := distance * FDefaultProtectPDG + FParams.StartRwyDirection.ElevTdz;
	tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj, FTurn.TurnInfo.Start.Direction, distance);
	FTurn.TurnInfo.Start.Point.Assign (tmpPoint);
	tmpPoint.Free;

	FCallManager.Call (CreateStraightArea);

	pcStraightCourse.P.ChangeValue (direction, False);
	pcStraightDistance.P.ChangeValue (distance, False);	
end;

procedure TMainForm.rbStraigthCreateSelectPointClick (Sender: TObject);
var
	isSelectPoint:	Boolean;
begin
	isSelectPoint := rbStraigthSelectPoint.Checked;

	cbStraightSgfPoint.Enabled := isSelectPoint;
	SetControlColor (cbStraightSgfPoint, isSelectPoint);

	pcStraightCourse.P.ReadOnly := isSelectPoint;
	pcStraightDistance.P.ReadOnly := isSelectPoint;

	if isSelectPoint then
		cbStraightSgfPointSelect (cbStraightSgfPoint)
	else
	begin
		FCallManager.BeginSuspend;
		FTurn.TurnInfo.Start.SignificantPointID := '';
		pcStraightCoursePChangeValue (nil);
		pcStraightDistancePChangeValue (nil);
		FCallManager.Call (CreateStraightArea);
		FCallManager.EndSuspend;
	end;
end;

procedure TMainForm.bReportClick(Sender: TObject);
begin
  	if FCurrentPage = dpgParameters then
		exit;

    FReportForm.Show;

    UpdateReportWindow ();
end;

procedure TMainForm.CreateStraightArea (param: TFunctionParam);
var
	side:		        TSideDirection;
	maxRequiredGrd:     Double;
	angle:		        Double;
	obs:		        TObstacleInArea;
	calcuteObstacles:   Boolean;
begin
	calcuteObstacles := False;

	if FParams.SensorType = stGNSS then
    begin
	    if (FPrevStraightAreaParams.Direction <> FTurn.TurnInfo.Start.Direction) or
			(FPrevStraightAreaParams.Distance <> FTurn.TurnInfo.Start.Distance) then
		begin
			FStraight.CreateArea (FTurn.TurnInfo.Start.Direction,
						FTurn.TurnInfo.Start.Distance, False);
	        calcuteObstacles := True;
	    end;
    end
    else
    begin
	    if (FPrevStraightAreaParams.Direction <> FTurn.TurnInfo.Start.Direction) or
	        (FPrevStraightAreaParams.Distance <> FTurn.TurnInfo.Start.Distance) or
            (FPrevStraightAreaParams.PDG <> FTurn.TurnInfo.PDG) then
        begin
            FTurn.CreateStaightAreaDME (False);
            calcuteObstacles := True;
        end;
    end;

	if calcuteObstacles then
	begin
		FStraight.FillObstaclesInStraightArea (FTurn.TurnInfo.Start.Direction,
	                FTurn.TurnInfo.PDG, FStraight.Segment.Area);
    end;

	FTurn.DrawStraightArea ();

	//--- begin: Fill calculated max info.
	side := SideFrom2Angle_new (FTurn.TurnInfo.Start.Direction, FParams.StartRwyDirection.Direction);
	if side = sideRight then
		labStaightInfoAdjustmentUnit.Caption := GMuiStrings.ToRight
	else
		labStaightInfoAdjustmentUnit.Caption := GMuiStrings.ToLeft;
		
	angle := SubtractAngles (FTurn.TurnInfo.Start.Direction, FParams.StartRwyDirection.Direction);
	editStaightInfoAdjustment.Text := RoundToStr (RadToDeg (angle), GAngleAccuracy, rtNear);

	if FStraight.MaxRequiredGrdIndex > -1 then
	begin
		obs := FStraight.Obstacles.Item [FStraight.MaxRequiredGrdIndex];
		maxRequiredGrd := obs.RequiredGrd;
        pcStraightReqPDG.P.ChangeValue (maxRequiredGrd, False);
		editStaightInfoMOCObsName.Text := obs.AixmID;
	end
	else
	begin
		editStaightInfoMOCObsName.Text := '';
		maxRequiredGrd := GConst.PANSOPS.Constant [dpPDG_Nom].Value;
        pcStraightReqPDG.P.ChangeValue (maxRequiredGrd, False);
		editStraightInfoElevGrd.Text := '';
	end;

    if pcStraightReqPDG.P.Value > pcStraightPDG.P.Value then
        pcStraightPDG.ValueControl.Font.Color := 255
    else
        pcStraightPDG.ValueControl.Font.Color := Self.Font.Color;


	if FStraight.MaxDistPdgIndex > -1 then
	begin
		obs := FStraight.Obstacles.Item [FStraight.MaxDistPdgIndex];
		editStraightInfoElevGrd.Text := RoundToStr (obs.DistPDG * maxRequiredGrd, GAltitudeAccuracy, rtCeil)
	end
	else
		editStraightInfoElevGrd.Text := '';
	//--- end.

    FPrevStraightAreaParams.Direction := FTurn.TurnInfo.Start.Direction;
    FPrevStraightAreaParams.Distance := FTurn.TurnInfo.Start.Distance;
	FPrevStraightAreaParams.PDG := FTurn.TurnInfo.PDG;

	bReport.Enabled := True;
	bSave.Enabled := True;
{$IFDEF DISABLE_SAVE}
	bSave.Enabled := False;
{$ENDIF}
end;

procedure TMainForm.cbTurnCategorySelect (Sender: TObject);
begin
	if (cbTurnCategory.ItemIndex < 0) then { or
		(Integer (FTurn.TurnInfo.AircraftCategory) = cbTurnCategory.ItemIndex) then //--- RIGA 22 Jan 2009---//}
		exit;

	FTurn.TurnInfo.AircraftCategory := TAircraftCategory (cbTurnCategory.ItemIndex);

	OnAircraftCategoryChanged ();
end;

procedure TMainForm.OnAircraftCategoryChanged ();
var
	maxIAS: 			Double;
begin
	maxIAS := GConst.AircraftCategory.Constant [cVmaFaf].Value [FTurn.TurnInfo.AircraftCategory];

	FTurn.TurnInfo.FlyBySpiralRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		maxIAS, FTurn.TurnInfo.Start.HeightProtect);

	pcIAS.P.BeginUpdate (False);
	pcIAS.P.MinValue := GConst.AircraftCategory.Constant [cVmaInter].Value [FTurn.TurnInfo.AircraftCategory];
	pcIAS.P.MaxValue := maxIAS;
	pcIAS.P.Value := pcIAS.P.MaxValue;
	pcIAS.P.EndUpdate;
	pcIASPChangeValue (nil);
end;

procedure TMainForm.cbTurnNextSignificantPointsSelect (Sender: TObject);
var
	param:	TPointParam;
begin
	if (cbTurnNextSignificantPoints.ItemIndex < 0) or
		(not rbTurnNextSignificantPoints.Checked) then
	begin
		labTurnNextSignifigantPointsUnit.Caption := '';
		exit;
	end;

	param := TPointParam.Create;

	param.SignifigantPoint := TSignificantPoint (cbTurnNextSignificantPoints.Items.Objects [
					cbTurnNextSignificantPoints.ItemIndex]);
	labTurnNextSignifigantPointsUnit.Caption := AIXMTypeNames [param.SignifigantPoint.AIXMType];

	param.Direction := FTurn.TurnInfo.Next.Direction;
	param.Distance := FTurn.TurnInfo.Next.Distance;

	FCallManager.Call (NextPointChanged_, param);

	param.Free;
end;

procedure TMainForm.rbTurnNextPointClick (Sender: TObject);
var
	param:	TPointParam;
begin
	if rbTurnNextSignificantPoints.Checked and (cbTurnNextSignificantPoints.Items.Count = 0) then
		exit;

	if rbTurnNextSignificantPoints.Checked then
	begin
		if cbTurnNextSignificantPoints.ItemIndex = -1 then
			cbTurnNextSignificantPoints.ItemIndex := 0;
		cbTurnNextSignificantPointsSelect (nil);
	end
	else
	begin
		if cbTurnNextSignificantPoints.ItemIndex <> -1 then
			cbTurnNextSignificantPoints.ItemIndex := -1;
		FTurn.TurnInfo.Next.SignificantPointID := '';

		param := TPointParam.Create;
		param.SignifigantPoint := nil;
		param.Direction := pcTurnNextCourse.P.Value;
		param.Distance := pcTurnNextDistance.P.Value;

		FCallManager.Call (NextPointChanged_, param);

		param.Free;
	end;

	NextPointSelectTypeState ();
end;

procedure TMainForm.CreateTurnArea ();
const
	turnSideName:	Array [TSideDirection] of String = ('Left', 'On', 'Right');
	turnTypeName:	Array [TTurnType] of String = ('FlyBy','FlyOver','AtHeight');
	codeTypeName:	Array [TCodeType] of String = ('TF', 'CF', 'DF');	
var
{$IFDEF SHOW_MESSAGEBOX}
	debugMessage:	String;
{$ENDIF}
	val:            Double;
	tmpPoint:		TPoint;
begin
{$IFDEF SHOW_MESSAGEBOX}

	debugMessage := Format (
		'TurnSide:'#9'%s'#13 +
		'TurnType:'#9'%s'#13 +
		'CodeType:'#9'%s'#13 +

		'HeightToKK:'#9'%f'#13 +
		'HeightNominal:'#9'%f'#13 +
		'HeightProtect:'#9'%f'#13 +

		'PDG:'#9#9'%f'#13 +
		'PDGNominal:'#9'%f'#13 +
		'PDGProtect:'#9'%f'#13 +

		'Start.Distance:'#9'%f'#13 +
		'Start.Direction:'#9'%f'#13 +
		'Next.Direction:'#9'%f'#13 +
		'Next.Distance:'#9'%f'#13 +
		'TurnAngle:'#9'%f'#13,
		[
		turnSideName [FTurn.TurnInfo.TurnSide],
		turnTypeName [FTurn.TurnInfo.TurnType],
		codeTypeName [FTurn.TurnInfo.CodeType],

		FTurn.TurnInfo.Start.HeightToKK,
		FTurn.TurnInfo.Start.HeightNominal,
		FTurn.TurnInfo.Start.HeightProtect,

		FTurn.TurnInfo.PDG,
		FTurn.TurnInfo.PDGNominal,
		FTurn.TurnInfo.PDGProtect,

		FTurn.TurnInfo.Start.Distance,
		RadToDeg (FTurn.TurnInfo.Start.Direction),
		RadToDeg (FTurn.TurnInfo.Next.Direction),
		FTurn.TurnInfo.Start.Distance,
		RadToDeg (FTurn.TurnInfo.TurnAngle)
		]);
	ShowMessage (debugMessage);
    
{$ENDIF}

	if FTurn.TurnInfo.Next.Direction = FTurn.TurnInfo.Start.Direction then
	begin
		FTurn.TurnInfo.Next.Direction := FTurn.TurnInfo.Start.Direction - Integer (FTurn.TurnInfo.TurnSide) *  DegToRad (0.1);
		FTurn.TurnInfo.TurnAngle :=  Modulus ((FTurn.TurnInfo.Start.Direction -
			FTurn.TurnInfo.Next.Direction) * Integer (FTurn.TurnInfo.TurnSide), 2*Pi);
		tmpPoint := PointAlongPlane (FTurn.TurnInfo.Start.Point,
						FTurn.TurnInfo.Next.Direction, FTurn.TurnInfo.Next.Distance);
		FTurn.TurnInfo.Next.Point.Assign (tmpPoint);
		tmpPoint.Free;

		//raise EARANError.Create ('Start and Next Direction must be different!', mtWarning);
	end;

	FTurn.CreateArea ();

    pcTurnKKDistance.P.ChangeValue (FTurn.TurnInfo.Start.DistanceToKK, False);    
    pcTurnKKHeight.P.ChangeValue (FTurn.TurnInfo.Start.HeightToKK, False);

	FNextPointGraphic.Erase;
	FNextLineGraphic.Erase;

	//--- begin: Calculate Report.
	FTurn.CalcMinPDG (True);

	if FStraight.MaxRequiredGrdIndex > -1 then
		val := FStraight.Obstacles.Item [FStraight.MaxRequiredGrdIndex].RequiredGrd
	else
		val := GConst.PANSOPS.Constant [dpPDG_Nom].Value;

    pcTurnPrevReqPDG.P.ChangeValue (val, False);

    if pcTurnPrevReqPDG.P.Value > pcTurnPrevPDG.P.Value then
        pcTurnPrevPDG.ValueControl.Font.Color := 255
    else
        pcTurnPrevPDG.ValueControl.Font.Color := Self.Font.Color;

    //--------------------------------------------------------------------------
    
	if FTurn.TurnInfo.MaxRequiredGrdIndex = -1 then
		val := GConst.PANSOPS.Constant [dpPDG_Nom].Value
	else
		val := FTurn.TurnInfo.Obstacles.Item [FTurn.TurnInfo.MaxRequiredGrdIndex].RequiredGrd;

    pcTurnCurReqPDG.P.ChangeValue (val, False);

    if pcTurnCurReqPDG.P.Value > pcTurnCurPDG.P.Value then
        pcTurnCurPDG.ValueControl.Font.Color := 255
    else
        pcTurnCurPDG.ValueControl.Font.Color := Self.Font.Color;
	//--- end.

	bReport.Enabled := True;
	bSave.Enabled := True;
{$IFDEF DISABLE_SAVE}
	bSave.Enabled := False;
{$ENDIF}
end;

function TMainForm.GetStartMinDistance (vHeight, pdg: Double): Double;
const
	maxTurnAngle: Double = 120 * (PI / 180);
var
	abvDER,
	valATT:	    Double;
begin
	abvDER := GConst.PANSOPS.Constant [dpH_abv_DER].Value;		//const above DER - 5 m.

	if (FTurn.TurnInfo.TurnType = tuFlyBy) or
			(FTurn.TurnInfo.TurnType = tuFlyOver) then
		valATT := GConst.GNSS.Constant [IDEP_].ATT
	else
    	valATT := 0;

	result := (vHeight - abvDER) / pdg + valATT;
end;

procedure TMainForm.UpdateReportWindow ();
var
    reportItem: TReportItem;
begin
//--- begin: TEMPRORARY... NOT FOR MULTI TURN.

	if FReportForm.ReportCount = 0 then
    begin
        reportItem := TReportItem.Create;
		FReportForm.AddItem (reportItem);
    end
    else
        reportItem := FReportForm.GetItem (0);

    reportItem.ObtacleList := FStraight.Obstacles;
    if FFormType = dftStraight then
        reportItem.PDG := FTurn.TurnInfo.PDG
	else
        reportItem.PDG := FTurn.TurnInfo.PrevTurnInfo.PDG;


	if FCurrentPage = dpgTurn then
	begin
		if FReportForm.ReportCount = 1 then
        begin
            reportItem := TReportItem.Create;
			FReportForm.AddItem (reportItem);
        end
        else
            reportItem := FReportForm.GetItem (1);

        reportItem.ObtacleList := FTurn.TurnInfo.Obstacles;
        reportItem.PDG := FTurn.TurnInfo.PDG;
	end;

    if FReportForm.Visible then
        FReportForm.ShowReport (FReportForm.ReportCount - 1);
//--- end.
end;

procedure TMainForm.bApplyClick(Sender: TObject);
begin
	GUi.ShowAnimation ();
	try
		if FCurrentPage = dpgTurn then
		begin
			CreateTurnArea ();
			UpdateReportWindow ();
		end;                      
	except
		on e: EARANError do HandleARANError (e);
	end;
	GUi.HideAnimation ();
end;

procedure TMainForm.HandleARANError (e: EARANError);
begin
	MessageDlg (e.Message, e.ErrorType, [mbOK], 0);
end;

procedure TMainForm.CodeTypeChanged;
begin
	FCallManager.BeginSuspend ();

	if pcTurnNextCourse.P.Active then
		pcTurnNextCourse.P.PerformCheck (True);

	rbTurnStartPointClick (nil);
	FOldNextChangedParam.CheckEqual := False;
	rbTurnNextPointClick (nil);

	FCallManager.EndSuspend ();
end;

procedure TMainForm.ExtendSegmentForAtHeight ();
const
	widthInDER = 150.0;
    distance = 600.0 - 0.05;
var
    dir:        Double;
    polygon:    TPolygon;
    ring:       TRing;
    tmpPoint,
    centrePoint: TPoint;
    geom:       TGeometry;
begin
    polygon := TPolygon.Create;
    ring := TRing.Create;

    dir := FParams.StartRwyDirection.Direction;

    centrePoint := PointAlongPlane (FParams.PairRwyDirection.Prj, dir, distance);

    tmpPoint := PointAlongPlane (centrePoint, dir + 0.5*Pi, widthInDER);
    ring.AddPoint (tmpPoint);
    tmpPoint.Free;

    tmpPoint := PointAlongPlane (centrePoint, dir - 0.5*Pi, widthInDER);
    ring.AddPoint (tmpPoint);
    tmpPoint.Free;

    centrePoint.Free;
    centrePoint := PointAlongPlane (FParams.StartRwyDirection.Prj, FParams.StartRwyDirection.Direction, 1);

    tmpPoint := PointAlongPlane (centrePoint, dir - 0.5*Pi, widthInDER);
    ring.AddPoint (tmpPoint);
    tmpPoint.Free;

    tmpPoint := PointAlongPlane (centrePoint, dir + 0.5*Pi, widthInDER);
    ring.AddPoint (tmpPoint);
    tmpPoint.Free;

    polygon.AddRing (ring);

    geom := GeoOper.unionGeometry (FStraight.Segment.ExtendedArea.PrimaryArea, polygon);

    FStraight.Segment.ExtendedArea.PrimaryArea.Assign (geom);

    centrePoint.Free;
    geom.Free;
    ring.Free;
    polygon.Free;
end;

procedure TMainForm.StartPointSelectTypeState ();
var
    isSelectSigPoint:   Boolean;
begin
    isSelectSigPoint := rbTurnStartSignificantPoints.Checked;

	cbTurnStartSignificantPoints.Enabled := isSelectSigPoint;
	SetControlColor (cbTurnStartSignificantPoints, isSelectSigPoint);

	pcTurnStartCourse.P.ReadOnly := isSelectSigPoint;
	pcTurnStartCourse.P.Active := not pcTurnStartCourse.P.ReadOnly;
	pcTurnStartDistance.P.ReadOnly := (isSelectSigPoint or (FTurn.TurnInfo.TurnType = tuAtHeight));
	pcTurnStartDistance.P.Active := not pcTurnStartDistance.P.ReadOnly;
end;

procedure TMainForm.NextPointSelectTypeState ();
var
    isSelectSigPoint:   Boolean;
begin
	isSelectSigPoint := rbTurnNextSignificantPoints.Checked;

	cbTurnNextSignificantPoints.Enabled := isSelectSigPoint;
	SetControlColor (cbTurnNextSignificantPoints, isSelectSigPoint);

	pcTurnNextCourse.P.ReadOnly := isSelectSigPoint;
	pcTurnNextCourse.P.Active := not pcTurnNextCourse.P.ReadOnly;
	pcTurnNextDistance.P.ReadOnly := isSelectSigPoint;
	pcTurnNextDistance.P.Active := not pcTurnNextDistance.P.ReadOnly;
end;

procedure TMainForm.TurnTypeChanged;
var
	param:			TPointParam;
begin
	FCallManager.BeginSuspend ();

	if FTurn.TurnInfo.TurnType = tuAtHeight then
		pcTurnKKHeight.P.PerformCheck (True)
	else
		pcTurnStartDistance.P.PerformCheck (True);

	OnHeightChanged ();
	OnPrevPDGChanged ();

	param := TPointParam.Create;
	param.SignifigantPoint := nil;
	param.Direction := pcTurnStartCourse.P.Value;
	param.Distance := pcTurnStartDistance.P.Value;

	FOldStartChangedParam.CheckEqual := False;
	FCallManager.Call (StartPointChanged_, param);

	param.Free;

	rbTurnNextPointClick (nil);

	FCallManager.EndSuspend ();
end;

procedure TMainForm.GetFilterNextPointsParams ();
begin
	FNextFilterParams := TFilterNextPointsParams.Create;
    if FTurn.TurnInfo.TurnType <> tuAtHeight then
    	FNextFilterParams.StartMinDistance := pcTurnStartDistance.P.MinValue
    else
        FNextFilterParams.StartMinDistance := 0;
	FNextFilterParams.Distance := FTurn.TurnInfo.Start.Distance;
	FNextFilterParams.Radius := FTurn.TurnInfo.ProtectedRadius;
	FNextFilterParams.Direction := FTurn.TurnInfo.Start.Direction;
	FNextFilterParams.Point := FTurn.TurnInfo.Start.Point;
	FNextFilterParams.TurnType := FTurn.TurnInfo.TurnType;
end;

procedure TMainForm.ClearCalculatedInfo ();
begin
	if FTurn.TurnInfo.TurnType <> tuAtHeight then
		pcTurnKKHeight.P.ChangeValue (0, False);

	pcTurnKKDistance.P.ChangeValue (0, False);
	pcTurnPrevReqPDG.P.ChangeValue (0, False);
	pcTurnCurReqPDG.P.ChangeValue (0, False);

	bReport.Enabled := False;
	bSave.Enabled := False;
end;

procedure TMainForm.TurnTypeState ();
var
	isAtHeight: Boolean;
begin
	isAtHeight := (FTurn.TurnInfo.TurnType = tuAtHeight);

	rbTurnCodeTypeTF.Enabled := not isAtHeight;
	rbTurnCodeTypeCF.Enabled := not isAtHeight;
	pcTurnKKHeight.P.ReadOnly := not isAtHeight;

	rbTurnCodeTypeDF.Enabled := (FTurn.TurnInfo.TurnType <> tuFlyBy);

    //--- begin: Enable or Disable StartPointList ComboBox.
    if isAtHeight and rbTurnStartSignificantPoints.Checked then
    begin
        rbTurnStartCreatePoint.Checked := True;
        StartPointSelectTypeState ();
    end;
    rbTurnStartSignificantPoints.Enabled := not isAtHeight;
    if not rbTurnStartSignificantPoints.Enabled then
    begin
        cbTurnStartSignificantPoints.ItemIndex := -1;
        rbTurnStartCreatePoint.Checked := True;
    end;
    //--- end.

    //--- begin: Set enalbe to KK line distance and height.
    pcTurnKKDistance.Visible := not isAtHeight;

    if isAtHeight then
    begin
        grbKKLine.Height := 45;
        grbKKLine.Caption := '';
    end
    else
    begin
		grbKKLine.Height := 85;

		grbKKLine.Caption := GMuiStrings.ControlKKLine;
    end;
	//--- end.

	if isAtHeight then
	begin
		pcHeightProtect.Top := 200;
		grbTurnAircraftParams.Height := 173;
	end
	else
	begin
		pcHeightProtect.Top := 170;
		grbTurnAircraftParams.Height := 205;
	end;

	if (not rbTurnCodeTypeTF.Enabled) and (rbTurnCodeTypeTF.Checked) then
		rbTurnCodeTypeDF.Checked := True;

	if (not rbTurnCodeTypeDF.Enabled) and (rbTurnCodeTypeDF.Checked) then
		rbTurnCodeTypeTF.Checked := True;

	CodeTypeState ();
	StartPointSelectTypeState ();
end;

procedure TMainForm.TurnTypeClick (Sender: TObject);
begin
	prevTurnType := FTurn.TurnInfo.TurnType;

	if rbTurnTypeFlyBy.Checked then
		FTurn.TurnInfo.TurnType := tuFlyBy
	else if	rbTurnTypeFlyOver.Checked then
		FTurn.TurnInfo.TurnType := tuFlyOver
	else
		FTurn.TurnInfo.TurnType := tuAtHeight;

	pcTurnKKHeight.P.Active := (FTurn.TurnInfo.TurnType = tuAtHeight);
	pcTurnStartDistance.P.Active := (FTurn.TurnInfo.TurnType <> tuAtHeight);

	TurnTypeChanged ();
	TurnTypeState ();
end;

procedure TMainForm.CodeTypeState ();
begin
	pcTurnNextCourse.P.ShowMinMaxHint := pcTurnNextCourse.P.Active;

	cbTurnDFSide.Enabled := ((FTurn.TurnInfo.CodeType = ctDF));
end;

procedure TMainForm.TurnCodeTypeClick(Sender: TObject);
begin
	if rbTurnCodeTypeTF.Checked then
		FTurn.TurnInfo.CodeType := ctTF
	else if rbTurnCodeTypeCF.Checked then
//		FTurn.TurnInfo.CodeType := ctCF
		FTurn.TurnInfo.CodeType := ctTF
	else
		FTurn.TurnInfo.CodeType := ctDF;

	FTurn.TurnInfo.tmp_codeTypeIsCF := rbTurnCodeTypeCF.Checked;

	pcTurnNextCourse.P.Active := (FTurn.TurnInfo.CodeType <> ctDF);

	CodeTypeChanged ();
	CodeTypeState ();
end;

procedure TMainForm.rbTurnStartPointClick (Sender: TObject);
var
	param:	TPointParam;
begin
	if rbTurnStartSignificantPoints.Checked and (cbTurnStartSignificantPoints.Items.Count = 0) then
		exit;

	if rbTurnStartSignificantPoints.Checked then
	begin
		if cbTurnStartSignificantPoints.ItemIndex = -1 then
			cbTurnStartSignificantPoints.ItemIndex := 0;
		cbTurnStartSignificantPointsSelect (cbTurnStartSignificantPoints);
	end
	else
	begin
		if cbTurnStartSignificantPoints.ItemIndex <> -1 then
			cbTurnStartSignificantPoints.ItemIndex := -1;
		FTurn.TurnInfo.Start.SignificantPointID := '';

		param := TPointParam.Create;
		param.SignifigantPoint := nil;
		param.Direction := pcTurnStartCourse.P.Value;
		param.Distance := pcTurnStartDistance.P.Value;

		FCallManager.Call (StartPointChanged_, param);

		param.Free;
	end;

	StartPointSelectTypeState ();	
end;

procedure TMainForm.cbTurnStartSignificantPointsSelect (Sender: TObject);
var
	param:		TPointParam;
begin
	if (cbTurnStartSignificantPoints.ItemIndex < 0) or (not rbTurnStartSignificantPoints.Checked) then
	begin
		labTurnStartSignifigantPointsUnit.Caption := '';
		exit;
	end;

	param := TPointParam.Create;
	param.SignifigantPoint := TSignificantPoint (cbTurnStartSignificantPoints.Items.Objects [cbTurnStartSignificantPoints.ItemIndex]);
	labTurnStartSignifigantPointsUnit.Caption := AIXMTypeNames [param.SignifigantPoint.AIXMType];

	param.Direction := FTurn.TurnInfo.Start.Direction;
	param.Distance := FTurn.TurnInfo.Start.Distance;

	FCallManager.Call (StartPointChanged_, param);

	param.Free;
end;

procedure TMainForm.EnableControl (control: TControl; isEnable: Boolean);
var
	i:			Integer;
	winControl:	TWinControl;
begin
	control.Enabled := isEnable;
	
	if not (control is TWinControl) then
		exit;
	winControl := TWinControl (control);
	for i:=0 to winControl.ControlCount - 1 do
		EnableControl (winControl.Controls [i], isEnable);
end;

procedure TMainForm.pcIASPChangeValue (Sender: TCustomParameter);
begin
	if FTurn.TurnInfo.IAS = pcIAS.P.Value then
		exit;

	FTurn.TurnInfo.IAS := pcIAS.P.Value;

    pcTAS.P.ChangeValue (FParams.ToTAS (
        FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightProtect), False);

	FTurn.TurnInfo.NominalRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightNominal);

    SetProtectRadius (FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightProtect));
end;

procedure TMainForm.pcModelAreaRadiusPChangeValue (Sender: TCustomParameter);
begin
	cbAerodromesSelect (cbAerodromes);

    pcTurnStartDistance.P.BeginUpdate (False);
    pcTurnStartDistance.P.MaxValue := pcModelAreaRadius.P.Value;
    pcTurnStartDistance.P.EndUpdate ();
end;

procedure TMainForm.pcStraightCoursePChangeValue(Sender: TCustomParameter);
var
	direction:	Double;
	tmpPoint:	TPoint;
begin
	direction := pcStraightCourse.P.Value;

	FTurn.TurnInfo.Start.Direction := direction;
	tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj, direction, FTurn.TurnInfo.Start.Distance);
	FTurn.TurnInfo.Start.Point.Assign (tmpPoint);
	tmpPoint.Free;

	FCallManager.Call (CreateStraightArea);
end;

procedure TMainForm.pcStraightDistancePChangeValue (Sender: TCustomParameter);
var
	tmpPoint:	TPoint;
	distance:	Double;
begin
	distance := pcStraightDistance.P.Value;

	FTurn.TurnInfo.Start.Distance := distance;
	FTurn.TurnInfo.Start.HeightToKK := distance * FDefaultProtectPDG + FParams.StartRwyDirection.ElevTdz;
	FTurn.TurnInfo.Start.HeightProtect := FTurn.TurnInfo.Start.Distance *
		FDefaultProtectPDG + FParams.StartRwyDirection.ElevTdz;
	tmpPoint := PointAlongPlane (FParams.StartRwyDirection.Prj, FTurn.TurnInfo.Start.Direction, distance);
	FTurn.TurnInfo.Start.Point.Assign (tmpPoint);
	tmpPoint.Free;

	FCallManager.Call (CreateStraightArea);
end;

procedure TMainForm.rbDepartureTypeClick(Sender: TObject);
begin
	if Sender = rbDepartureTypeStaight then
		FFormType := dftStraight
	else
		FFormType := dftMultiTurn;

	FReportForm.FormType := FFormType;
end;

procedure TMainForm.CreateStraightAreaOnTurnPage ();
var
    changed:    Boolean;
begin
{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - CreateStraightAreaOnTurnPage');
{$ENDIF}

	GUi.ShowAnimation ();

    changed := False;

    if (FPrevStraightAreaParams.Direction <> FTurn.TurnInfo.Start.Direction) or
        (FPrevStraightAreaParams.Distance <> FTurn.TurnInfo.Start.Distance) or
	        ((FPrevStraightAreaParams.TurnType <> FTurn.TurnInfo.TurnType)
	        and (FPrevStraightAreaParams.TurnType = tuAtHeight)
	        and (FTurn.TurnInfo.TurnType = tuAtHeight)) then
    begin
        changed := True;
    end;

    if FParams.SensorType <> stDME then
    begin
		if changed then
	    begin
            FStraight.CreateArea (FTurn.TurnInfo.Start.Direction,
					    FTurn.TurnInfo.Start.Distance, True);
        end;
    end
    else
    begin
        if changed or (FPrevStraightAreaParams.PDG <> FTurn.TurnInfo.PDG) then
        begin
            changed := True;
            FTurn.CreateStaightAreaDME (True);
        end;
    end;

    if changed then
    begin
        if FTurn.TurnInfo.TurnType = tuAtHeight then
	        ExtendSegmentForAtHeight ();
        FTurn.TurnInfo.PrevTurnInfo.Segment.Assign (FStraight.Segment);
	    FTurn.DrawStraightArea ();
    end;

    FPrevStraightAreaParams.Direction := FTurn.TurnInfo.Start.Direction;
    FPrevStraightAreaParams.Distance := FTurn.TurnInfo.Start.Distance;
    FPrevStraightAreaParams.TurnType := FTurn.TurnInfo.TurnType;
    FPrevStraightAreaParams.PDG := FTurn.TurnInfo.PDG;

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - CreateStraightAreaOnTurnPage (changed: ' + BoolToStr (changed) + ')');
{$ENDIF}
	GUi.HideAnimation ();
end;

procedure TMainForm.SetTurnSideAndAngle ();
const
	maxDFTurnAngle = 3 * Pi / 2;
var
	angleLeftDF,
	angleRightDF:	Double;
	enabledCombo:	Boolean;
begin
	if FTurn.TurnInfo.CodeType = ctDF then
	begin
		angleLeftDF := Modulus ((FTurn.TurnInfo.Start.Direction - FTurn.TurnInfo.Next.Direction) * Integer (sideLeft), 2 * Pi);
		angleRightDF := Modulus ((FTurn.TurnInfo.Start.Direction - FTurn.TurnInfo.Next.Direction) * Integer (sideRight), 2 * Pi);
		enabledCombo := True;
		if angleLeftDF >= maxDFTurnAngle then
		begin
			cbTurnDFSide.ItemIndex := 1;
			enabledCombo := False;
			FTurn.TurnInfo.TurnAngle := angleRightDF;
		end
		else if angleRightDF >= maxDFTurnAngle then
		begin
			cbTurnDFSide.ItemIndex := 0;
			enabledCombo := False;
			FTurn.TurnInfo.TurnAngle := angleLeftDF;
		end
		else
		begin
            if angleLeftDF < angleRightDF then
                cbTurnDFSide.ItemIndex := 0
            else
                cbTurnDFSide.ItemIndex := 1;
		end;
		cbTurnDFSide.Enabled := enabledCombo;
		cbTurnDFSideSelect (cbTurnDFSide);
	end
	else
	begin
		FTurn.TurnInfo.TurnSide := SideDef (FTurn.TurnInfo.Start.Point,
			FTurn.TurnInfo.Start.Direction, FTurn.TurnInfo.Next.Point);

		FTurn.TurnInfo.TurnAngle :=  Modulus ((FTurn.TurnInfo.Start.Direction -
			FTurn.TurnInfo.Next.Direction) * Integer (FTurn.TurnInfo.TurnSide), 2*Pi);
		if FTurn.TurnInfo.TurnSide = sideRight then
			cbTurnDFSide.ItemIndex := 1
		else
			cbTurnDFSide.ItemIndex := 0;
	end;
end;

procedure TMainForm.StartOrNextPointChanged;
const
	maxDFTurnAngle = 3 * Pi / 2;
begin
{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - StartOrNextPointChanged');
{$ENDIF}

	ClearCalculatedInfo ();
	
	if not IsGeometryEmpty (FStraight.Segment.Area.PrimaryArea) then
		FTurn.DrawStraightArea ();

	SetTurnSideAndAngle ();

	//--- begin: Draw Start point and line.
	FNextPointGraphic.Geometry := FTurn.TurnInfo.Next.Point;
	FNextLineGraphic.Geometry.AsPolyline.Part [0].Point [0].Assign (FTurn.TurnInfo.Start.Point);
	FNextLineGraphic.Geometry.AsPolyline.Part [0].Point [1].Assign (FTurn.TurnInfo.Next.Point);

	FNextPointGraphic.Draw;
	FNextLineGraphic.Draw;
	//--- end.

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - StartOrNextPointChanged');
{$ENDIF}
end;

procedure TMainForm.FilterStartPoints (param: TFunctionParam);
var
	i,
	selIndex:	Integer;
	selId:		String;
	indexList:  TList;
	adjAngle:   Double;
	prevDir:    Double;
	sigPoint:   TSignificantPoint;
	enable:     Boolean;
begin
{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - FilterStartPoints');
{$ENDIF}

	adjAngle := GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;	//const adjust straight angle.
	prevDir := FTurn.TurnInfo.PrevTurnInfo.Start.Direction;
	indexList := FilterSignificantPoints (FParams.SignificantPoints,
					FTurn.TurnInfo.PrevTurnInfo.Start.Point, prevDir - adjAngle,
					prevDir + adjAngle, pcTurnStartDistance.P.MinValue);


	selId := '';
	selIndex := -1;
	if rbTurnStartSignificantPoints.Checked then
	begin
		if cbTurnStartSignificantPoints.ItemIndex <> -1 then
			selId := TSignificantPoint (cbTurnStartSignificantPoints.Items.Objects [cbTurnStartSignificantPoints.ItemIndex]).ID;
	end;

	cbTurnStartSignificantPoints.Clear;

	for i:=0 to indexList.Count - 1 do
	begin
		sigPoint := FParams.SignificantPoints.Item [Integer (indexList.Items [i])];
		if (selId <> '') and (selId = sigPoint.ID) then
			selIndex := cbTurnStartSignificantPoints.Items.Count;
		cbTurnStartSignificantPoints.AddItem (sigPoint.AixmID, sigPoint);
	end;

	rbTurnStartSignificantPoints.Caption := GMuiStrings.SignificantPoints + ' (' + IntToStr (indexList.Count) + ')';
	enable := (indexList.Count > 0);
	indexList.Free;

	rbTurnStartSignificantPoints.Enabled := enable;

	if (not enable) and (rbTurnStartSignificantPoints.Checked) then
		rbTurnStartCreatePoint.Checked := True;

	if selIndex <> -1 then
		cbTurnStartSignificantPoints.ItemIndex := selIndex;

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - FilterStartPoints');
{$ENDIF}
end;

procedure TMainForm.FilterNextPoints (param: TFunctionParam);
const
	maxTurnAngle: Double = 120 * (PI / 180);
var
	i,
	selIndex:	Integer;
	selId:		String;
	alpha:		Double;
	interval:	TInterval;
	indexList:	TList;
	sigPoint:	TSignificantPoint;
	params:     TFilterNextPointsParams;
	enable:     Boolean;
begin

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - FilterNextPoints');
{$ENDIF}


    GetFilterNextPointsParams;

	if FNextFilterParams.IsEqual (FOldNextFilterParams) then
	begin
{$IFDEF SHOW_MESSAGEBOX}
		AddLogMessage ('Exit - FilterNextPoints');
{$ENDIF}
		exit;
	end;

	FOldNextFilterParams.Assign (FNextFilterParams);
	params := FNextFilterParams;
	alpha := 0; // Assign initial value for remove compiler warning.

	case params.TurnType of
		tuFlyBy:
			begin
				if params.Distance <= (params.StartMinDistance + params.Radius * Tan (maxTurnAngle / 2)) then
					alpha := 2 * ArcTan ( (params.Distance - params.StartMinDistance) / params.Radius)
				else
					alpha := 2 * Pi / 3;
			end;
		tuFlyOver:
				alpha := maxTurnAngle;
		tuAtHeight:
			begin
				interval.Left :=  GNullDataValue;
				interval.Right := GNullDataValue;
			end;
	end;

	if params.TurnType <> tuAtHeight then
	begin
		interval.Left :=  params.Direction -  alpha;
		interval.Right := params.Direction + alpha;

        pcTurnNextCourse.P.BeginUpdate (False);
        pcTurnNextCourse.P.Range := interval;
        pcTurnNextCourse.P.EndUpdate ();
	end;

	selId := '';
	selIndex := -1;
	if rbTurnNextSignificantPoints.Checked then
	begin
		if cbTurnNextSignificantPoints.ItemIndex <> -1 then
			selId := TSignificantPoint (cbTurnNextSignificantPoints.Items.Objects [cbTurnNextSignificantPoints.ItemIndex]).ID;
	end;

	cbTurnNextSignificantPoints.Items.Clear;

	indexList := FilterSignificantPoints (FParams.SignificantPoints, params.Point,
					interval.Left, interval.Right);

	for i:=0 to indexList.Count - 1 do
	begin
		sigPoint := FParams.SignificantPoints.Item [Integer (indexList.Items [i])];
		if (selId <> '') and (selId = sigPoint.ID) then
			selIndex := cbTurnNextSignificantPoints.Items.Count;
		cbTurnNextSignificantPoints.AddItem (sigPoint.AixmID, sigPoint);
	end;

	rbTurnNextSignificantPoints.Caption := GMuiStrings.SignificantPoints + ' (' + IntToStr (indexList.Count) + ')';
    enable := (indexList.Count > 0);
	indexList.Free;

	rbTurnNextSignificantPoints.Enabled := enable;

    if (not enable) and (rbTurnNextSignificantPoints.Checked) then
        rbTurnNextCreatePoint.Checked := True;

	if selIndex <> -1 then
		cbTurnNextSignificantPoints.ItemIndex := selIndex;

    rbTurnNextPointClick (nil);

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - FilterNextPoints');
{$ENDIF}
end;

procedure TMainForm.SetRwyEarlestLine ();
var
    tmpPoint:   TPoint;
begin
    tmpPoint := PointAlongPlane (FTurn.RwyInfo.Start.Point, FTurn.RwyInfo.Start.Direction + 0.5*Pi, 0.25);
    FTurn.RwyInfo.Start.EarlestTolerance.PointList.Point [0].Assign (tmpPoint);
    FTurn.RwyInfo.Start.EarlestTolerance.PointList.Point [1].Assign (tmpPoint);
    FTurn.RwyInfo.Start.EarlestTolerance.PointList.Point [2].Assign (FTurn.RwyInfo.Start.Point);
    tmpPoint.Free;
    tmpPoint := PointAlongPlane (FTurn.RwyInfo.Start.Point, FTurn.RwyInfo.Start.Direction - 0.5*Pi, 0.25);
    FTurn.RwyInfo.Start.EarlestTolerance.PointList.Point [3].Assign (tmpPoint);
    FTurn.RwyInfo.Start.EarlestTolerance.PointList.Point [4].Assign (tmpPoint);
    tmpPoint.Free;
end;

procedure TMainForm.ParametersToTurnPage;
const
	maxTurnAngle: Double = 120 * (PI / 180);
var
	prevDir,
	radius,
	IAS,
	minDist:	Double;
begin
	FCallManager.BeginSuspend ();

	FNextFilterParams.Clear ();
	FTurn.TurnInfo.Clear ();
	FTurn.RwyInfo.Clear ();

	FromParameters ();

	FTurn.RwyInfo.Start.Point.Assign (FTurn.Parameters.StartRwyDirection.Prj);
	FTurn.RwyInfo.Start.Direction := FTurn.Parameters.StartRwyDirection.Direction;
	FTurn.RwyInfo.Start.Distance := 0.0;
	FTurn.RwyInfo.Start.HeightToKK := FTurn.Parameters.StartRwyDirection.ElevTdz +
		FTurn.Parameters.StartRwyDirection.ElevTdzAccuracy;
	FTurn.RwyInfo.Start.HeightProtect := FTurn.RwyInfo.Start.HeightToKK;
	FTurn.RwyInfo.Start.HeightNominal := FTurn.RwyInfo.Start.HeightToKK;
    SetRwyEarlestLine ();
	FTurn.TurnInfo.PrevTurnInfo := FTurn.RwyInfo;

	//--- begin: Set Default Values.
	rbTurnTypeFlyBy.Checked := True;
	rbTurnCodeTypeTF.Checked := True;
	rbTurnStartCreatePoint.Checked := True;
	FTurn.TurnInfo.TurnType := tuFlyBy;
	FTurn.TurnInfo.CodeType := ctTF;
	TurnTypeState ();
	StartPointSelectTypeState ();

	FTurn.TurnInfo.BankAngle := DegToRad (15);
	pcBankAngle.P.BeginUpdate (True);
	pcBankAngle.P.MinValue := FTurn.TurnInfo.BankAngle;
	pcBankAngle.P.MaxValue := DegToRad (25);
	pcBankAngle.P.Value := FTurn.TurnInfo.BankAngle;
	AddParamControlUpdateList (pcBankAngle.P);

	FTurn.TurnInfo.PrevTurnInfo.PDG := 0.033;
	pcTurnPrevPDG.P.BeginUpdate (True);
	pcTurnPrevPDG.P.MinValue := FTurn.TurnInfo.PrevTurnInfo.PDG;
	pcTurnPrevPDG.P.MaxValue := 0.15;
	pcTurnPrevPDG.P.Value := FTurn.TurnInfo.PrevTurnInfo.PDG;
	AddParamControlUpdateList (pcTurnPrevPDG.P);

	FTurn.TurnInfo.PDG := 0.033;
	FTurn.TurnInfo.PDGNominal := FDefaultNominalPDG;
	FTurn.TurnInfo.PDGProtect := FDefaultProtectPDG;
	pcTurnCurPDG.P.BeginUpdate (True);
	pcTurnCurPDG.P.MinValue := 0.033;
	pcTurnCurPDG.P.MaxValue := 0.15;
	pcTurnCurPDG.P.Value := FTurn.TurnInfo.PDG;
	AddParamControlUpdateList (pcTurnCurPDG.P);
	//--- end.

	//--- begin: Calculate appriximate radius. Calculate by IAS.
	IAS := GConst.AircraftCategory.Constant [cVmaFaf].Value [TAircraftCategory (cbTurnCategory.ItemIndex)];
	radius := BankToRadius (FTurn.TurnInfo.BankAngle, IAS);
    FTurn.TurnInfo.ProtectedRadius := radius;
	//--- end.

	prevDir := FTurn.TurnInfo.PrevTurnInfo.Start.Direction;

	FTurn.TurnInfo.Start.Direction := prevDir;
	pcTurnStartCourse.P.BeginUpdate (True);
	pcTurnStartCourse.P.OutConvertionPoint := FTurn.TurnInfo.PrevTurnInfo.Start.Point.Clone.AsPoint;
	pcTurnStartCourse.P.InConvertionPoint := PrjToGeo (FTurn.TurnInfo.PrevTurnInfo.Start.Point).AsPoint;
	pcTurnStartCourse.P.MinValue := prevDir - GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;
	pcTurnStartCourse.P.MaxValue := prevDir + GConst.PANSOPS.Constant [dpTr_AdjAngle].Value;
	pcTurnStartCourse.P.Value := prevDir;
	AddParamControlUpdateList (pcTurnStartCourse.P);

	minDist := GetStartMinDistance (
				GConst.PANSOPS.Constant [dpNGui_Ar1].Value, //const minimum height - 120 m.
				FTurn.TurnInfo.PrevTurnInfo.PDG);

	FTurn.TurnInfo.Start.Distance := minDist + radius * Tan (maxTurnAngle / 2);
	pcTurnStartDistance.P.BeginUpdate (True);
	pcTurnStartDistance.P.MinValue := minDist;
	pcTurnStartDistance.P.Value := FTurn.TurnInfo.Start.Distance;
	AddParamControlUpdateList (pcTurnStartDistance.P);

	FCallManager.Call (FilterStartPoints);

	cbTurnCategorySelect (nil);

    FPrevStraightAreaParams.Direction := 0;
	rbTurnStartPointClick (nil);

	FCallManager.EndSuspend ();

	pcTurnNextDistance.P.BeginUpdate (True);
	pcTurnNextDistance.P.Value := Abs (GetNextMinDistance ());
	//AddParamControlUpdateList (pcTurnNextDistance.P);
	pcTurnNextDistance.P.EndUpdate;	

	bApply.Enabled := True;
end;

procedure TMainForm.FromParameters ();
begin
	if cbRwyDirection.ItemIndex < 0 then
		raise EARANError.Create ('Please, select RWY Direction.', mtWarning);

	FParams.SignificantPoints := TSignificantPointCollection (
		GObjectDirectory.SignificantPoints [GOrgID, FParams.AHP, FParams.AHP.Geo, pcModelAreaRadius.P.Value]);
	FParams.Obstacles := GObjectDirectory.Obstalces [FParams.AHP.Geo, pcModelAreaRadius.P.Value];

	FParams.StartRwyDirection := TRWYDirection (cbRwyDirection.Items.Objects [cbRwyDirection.ItemIndex]);

	if cbRwyDirection.ItemIndex = 0 then
		FParams.PairRwyDirection := TRWYDirection (cbRwyDirection.Items.Objects [1])
	else
		FParams.PairRwyDirection := TRWYDirection (cbRwyDirection.Items.Objects [0]);

    if rbSensorTypeGNSS.Checked then
        FParams.SensorType := stGNSS
    else
        FParams.SensorType := stDME;
        
    FParams.IsDMEtill_1989 := (cbDMEInstalled.ItemIndex = 0);
    FParams.DMECount := StrToInt (cbDMECount.Text);

	if cbSignificant.ItemIndex >= 0 then
		FParams.EndSignificantPoint := TSignificantPoint (cbSignificant.Items.Objects [cbSignificant.ItemIndex]);
//	else
//		raise EARANError.Create ('Please, select End Significatn Point.', mtWarning);


	FParams.ModelAreaRadius := pcModelAreaRadius.P.Value;
end;

procedure TMainForm.OnBankAngleChanged ();
var
    radius: Double;
begin
	FTurn.TurnInfo.FlyBySpiralRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		pcIAS.P.MaxValue, FTurn.TurnInfo.Start.HeightProtect);

	FTurn.TurnInfo.NominalRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightNominal);

    radius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,     
                FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightProtect);

    SetProtectRadius (radius);
end;

procedure TMainForm.pcBankAnglePChangeValue (Sender: TCustomParameter);
begin
	if FTurn.TurnInfo.BankAngle = pcBankAngle.P.Value then
		exit;

	FTurn.TurnInfo.BankAngle := pcBankAngle.P.Value;
    OnBankAngleChanged ();
end;

procedure TMainForm.OnHeightProtectChanged ();
var
	TAS:	Double;
begin
    TAS := FParams.ToTAS (FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightProtect);
    pcTAS.P.ChangeValue (TAS, False);

	FTurn.TurnInfo.FlyBySpiralRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		pcIAS.P.MaxValue, FTurn.TurnInfo.Start.HeightProtect);

    SetProtectRadius (FParams.ToRadius (FTurn.TurnInfo.BankAngle,
        FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightProtect));
end;

procedure TMainForm.pcHeightProtectPChangeValue (Sender: TCustomParameter);
begin
    if FTurn.TurnInfo.Start.HeightProtect = pcHeightProtect.P.Value then
        exit;
    FTurn.TurnInfo.Start.HeightProtect := pcHeightProtect.P.Value;
    OnHeightProtectChanged ();
end;

procedure TMainForm.pcTurnStartDistancePChangeValue (Sender: TCustomParameter);
var
	param:	TPointParam;
begin
	if not rbTurnStartCreatePoint.Checked then
		exit;
		
	param := TPointParam.Create;
	param.SignifigantPoint := nil;
	param.Direction := FTurn.TurnInfo.Start.Direction;
	param.Distance := pcTurnStartDistance.P.Value;

	FCallManager.Call (StartPointChanged_, param);

	param.Free;
end;

procedure TMainForm.pcTurnStartCoursePChangeValue (Sender: TCustomParameter);
var
	param:	TPointParam;
begin
	if not rbTurnStartCreatePoint.Checked then
		exit;
		
	param := TPointParam.Create;
	param.SignifigantPoint := nil;
	param.Direction := pcTurnStartCourse.P.Value;
	param.Distance := FTurn.TurnInfo.Start.Distance;

	FCallManager.Call (StartPointChanged_, param);

	param.Free;
end;

procedure TMainForm.SetProtectRadius (val: Double);
begin
	FTurn.TurnInfo.ProtectedRadius := val;
	pcTurnRadius.P.ChangeValue (FTurn.TurnInfo.ProtectedRadius, False);

    rbTurnNextPointClick (nil);
end;
{
procedure TMainForm.SetNextMinDistance (param: TFunctionParam);
//var
//	minDist:    Double;
begin
{
	exit;
	if FExitSetMinDistanceCount > 0 then
	begin
		FExitSetMinDistanceCount := FExitSetMinDistanceCount + 1;
		exit;
	end;
	minDist := GetNextMinDistance ();

	pcTurnNextDistance.P.BeginUpdate ();
	pcTurnNextDistance.P.MinValue := minDist;
	pcTurnNextDistance.P.EndUpdate ();

	FilterNextPoints (nil);

	rbTurnNextPointClick (nil);
end;
}

procedure TMainForm.pcTurnNextCoursePChangeValue(Sender: TCustomParameter);
var
	param:	TPointParam;
begin
	if not rbTurnNextCreatePoint.Checked then
		exit;

	param := TPointParam.Create;
	param.SignifigantPoint := nil;
	param.Direction := pcTurnNextCourse.P.Value;
	param.Distance := FTurn.TurnInfo.Next.Distance;

	FCallManager.Call (NextPointChanged_, param);

	param.Free;
end;

procedure TMainForm.pcTurnNextDistancePChangeValue (Sender: TCustomParameter);
var
	param:	TPointParam;
begin
	if not rbTurnNextCreatePoint.Checked then
		exit;
		
	param := TPointParam.Create;
	param.SignifigantPoint := nil;
	param.Direction := FTurn.TurnInfo.Next.Direction;
	param.Distance := pcTurnNextDistance.P.Value;

	FCallManager.Call (NextPointChanged_, param);

	param.Free;
end;

procedure TMainForm.OnHeightChanged ();
var
	distance:   Double;
	param:		TPointParam;
begin
	if FTurn.TurnInfo.TurnType = tuAtHeight then
	begin
		distance := (FTurn.TurnInfo.Start.HeightToKK - GConst.PANSOPS.Constant [dpH_abv_DER].Value)/ FTurn.TurnInfo.PrevTurnInfo.PDG;
		pcTurnStartDistance.P.ChangeValue (distance, False);
		pcTurnStartDistancePChangeValue (nil);
		
		param := TPointParam.Create;
		param.SignifigantPoint := nil;
		param.Direction := FTurn.TurnInfo.Start.Direction;
		param.Distance := distance;
		FCallManager.Call (StartPointChanged_, param);
		param.Free;
   	end;
end;

procedure TMainForm.PDGNominalChanged (newValue: Double);
begin
	if FTurn.TurnInfo.PDGNominal = newValue then
		exit;
	FTurn.TurnInfo.PDGNominal := newValue;
	FTurn.TurnInfo.Start.HeightNominal := FTurn.TurnInfo.Start.Distance * FTurn.TurnInfo.PDGNominal;
end;

procedure TMainForm.PDGProtectChanged (newValue: Double);
begin
	if FTurn.TurnInfo.PDGProtect = newValue then
		exit;
	FTurn.TurnInfo.PDGProtect := newValue;
	pcHeightProtect.P.ChangeValue (FTurn.TurnInfo.Start.Distance * FTurn.TurnInfo.PDGProtect, True);	
end;              

procedure TMainForm.OnPrevPDGChanged ();
var
	distance:               Double;
	minDist:                Double;
begin
	FCallManager.BeginSuspend;

	//--- begin: Change PDGNominal.
	if (FTurn.TurnInfo.PrevTurnInfo.PDG > FTurn.TurnInfo.PDGNominal) or
			(FTurn.TurnInfo.TurnType = tuAtHeight) then
	begin
		PDGNominalChanged (FTurn.TurnInfo.PrevTurnInfo.PDG)
	end;
	//--- end.

	//--- begin: Change PDGProtect.
	if (FTurn.TurnInfo.PrevTurnInfo.PDG > FTurn.TurnInfo.PDGProtect) or
		(FTurn.TurnInfo.TurnType = tuAtHeight) then
	begin
		PDGProtectChanged (FTurn.TurnInfo.PrevTurnInfo.PDG);
	end
	else if FTurn.TurnInfo.PDGProtect <> FDefaultProtectPDG then
		PDGProtectChanged (FDefaultProtectPDG);
	//--- end.

	minDist := GetStartMinDistance (pcTurnKKHeight.P.MinValue, FTurn.TurnInfo.PrevTurnInfo.PDG);

  	pcTurnStartDistance.P.BeginUpdate (False);
	pcTurnStartDistance.P.MinValue := minDist;

    if FTurn.TurnInfo.TurnType = tuAtHeight then
    begin
        distance := (FTurn.TurnInfo.Start.HeightToKK - GConst.PANSOPS.Constant [dpH_abv_DER].Value) / FTurn.TurnInfo.PrevTurnInfo.PDG;
		pcTurnStartDistance.P.Value := distance;
		pcTurnStartDistancePChangeValue (nil);
    end
    else
		FCallManager.Call (FilterStartPoints);

    pcTurnStartDistance.P.EndUpdate;

	rbTurnStartPointClick (nil);

	FCallManager.EndSuspend;
end;

procedure TMainForm.pcTurnPrevPDGPChangeValue (Sender: TCustomParameter);
begin
	if FTurn.TurnInfo.PrevTurnInfo.PDG = pcTurnPrevPDG.P.Value then
		exit;

	FTurn.TurnInfo.PrevTurnInfo.PDG := pcTurnPrevPDG.P.Value;

	OnPrevPDGChanged ();
end;

procedure TMainForm.pcTurnCurPDGPChangeValue (Sender: TCustomParameter);
begin
	if FTurn.TurnInfo.PDG = pcTurnCurPDG.P.Value then
		exit;

	FTurn.TurnInfo.PDG := pcTurnCurPDG.P.Value;
end;

procedure TMainForm.cbTurnDFSideSelect (Sender: TObject);
begin
	if FTurn.TurnInfo.CodeType <> ctDF then
		exit;

	FTurn.TurnInfo.TurnSide := TSideDirection (2 * cbTurnDFSide.ItemIndex - 1);

    FTurn.TurnInfo.TurnAngle := Modulus ((FTurn.TurnInfo.Start.Direction - FTurn.TurnInfo.Next.Direction) *
        Integer (FTurn.TurnInfo.TurnSide), 2 * Pi);
end;

function TMainForm.GetNextMinDistance (): Double;
const
	bankTime = 5.0;
	pilotTime = 6.0;
	B2 = 15 * Pi / 180;
var
    R1, R2, H,
    L1, L2, L3, L4, L5,
    alpha, tmpAlpha,
    TAS, minDist:       Double;
begin
    R1 := FTurn.TurnInfo.ProtectedRadius;

	alpha := DegToRad (30.0);

	H := FTurn.TurnInfo.PrevTurnInfo.Start.HeightToKK +
		    FTurn.TurnInfo.Start.Distance * FTurn.TurnInfo.PDGProtect;
	TAS := IASToTAS (FTurn.TurnInfo.IAS, H, FParams.AHP.Temperature);

	if FTurn.TurnInfo.TurnType = tuFlyOver then
	begin
		R2 := BankToRadius (B2, TAS);
		L1 := R1 * Sin (FTurn.TurnInfo.TurnAngle);

		L2 := R1 * Cos (FTurn.TurnInfo.TurnAngle) * Tan (alpha);
		L3 := R1 * (1.0 - Cos (FTurn.TurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha);
		L4 := R2 * Tan (0.5 * alpha);

		If L3 * Cos (alpha) < L4 Then
		begin
			tmpAlpha := RadToDeg (ArcCos ((R2 + R1 * Cos (FTurn.TurnInfo.TurnAngle)) / (R1 + R2)));
			alpha := DegToRad ((AdvancedRound (tmpAlpha, 0, rtFloor)));
			L2 := R1 * Cos (FTurn.TurnInfo.TurnAngle) * Tan (alpha);
			if alpha <> 0 then
				L3 := R1 * (1.0 - Cos (FTurn.TurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha)
			else
			begin
				alpha := DegToRad (tmpAlpha);
				L3 := R1 * (1.0 - Cos (FTurn.TurnInfo.TurnAngle) / Cos(alpha)) / Sin(alpha)
			end;
		end;

		L5 := bankTime * TAS;
		minDist := L1 + L2 + L3 + L4 + L5;
	end
	else if FTurn.TurnInfo.TurnType = tuFlyBy then
	begin
		L1 := R1 * Tan(0.5 * FTurn.TurnInfo.TurnAngle);
		L2 := bankTime * TAS;
		minDist := L1 + L2;
    end
    else //--- FTurn.TurnInfo.TurnType = tuAtHeight
    begin
        minDist := 2 * R1;
    end;

    result := minDist;
end;

procedure TMainForm.pcTurnKKHeightPChangeValue(Sender: TCustomParameter);
begin
    if FTurn.TurnInfo.TurnType <> tuAtHeight then
        exit;
    if FTurn.TurnInfo.Start.HeightToKK = pcTurnKKHeight.P.Value then
        exit;

    FTurn.TurnInfo.Start.HeightToKK := pcTurnKKHeight.P.Value;
    OnHeightChanged ();
end;

procedure TMainForm.pcStraightPDGPChangeValue(Sender: TCustomParameter);
var
    obs:            TObstacleInArea;
    maxRequiredGrd: Double;
begin
	if FTurn.TurnInfo.PDG = pcStraightPDG.P.Value then
        exit;
	FTurn.TurnInfo.PDG := pcStraightPDG.P.Value;

	FTurn.TurnInfo.Start.HeightNominal := FTurn.TurnInfo.Start.Distance *
		FTurn.TurnInfo.PDG + FParams.StartRwyDirection.ElevTdz;

    if FParams.SensorType = stDME then
    begin
		CreateStraightArea (nil);
    end;

    FStraight.FillObstaclesInStraightArea (FTurn.TurnInfo.Start.Direction,
            FTurn.TurnInfo.PDG, FStraight.Segment.Area);

	if FStraight.MaxRequiredGrdIndex > -1 then
	begin
		obs := FStraight.Obstacles.Item [FStraight.MaxRequiredGrdIndex];
		maxRequiredGrd := obs.RequiredGrd;
        pcStraightReqPDG.P.ChangeValue (maxRequiredGrd, False);
		editStaightInfoMOCObsName.Text := obs.AixmID;
	end
	else
	begin
		editStaightInfoMOCObsName.Text := '';
		maxRequiredGrd := GConst.PANSOPS.Constant [dpPDG_Nom].Value;
        pcStraightReqPDG.P.ChangeValue (maxRequiredGrd, False);
		editStraightInfoElevGrd.Text := '';
	end;

    if pcStraightReqPDG.P.Value > pcStraightPDG.P.Value then
        pcStraightPDG.ValueControl.Font.Color := 255
    else
        pcStraightPDG.ValueControl.Font.Color := Self.Font.Color;
    

	if FStraight.MaxDistPdgIndex > -1 then
	begin
		obs := FStraight.Obstacles.Item [FStraight.MaxDistPdgIndex];
		editStraightInfoElevGrd.Text := RoundToStr (obs.DistPDG * maxRequiredGrd, GAltitudeAccuracy, rtCeil);
	end
	else
		editStraightInfoElevGrd.Text := '';
end;

procedure TMainForm.SensorTypeClick(Sender: TObject);
begin
    grbDME.Visible := rbSensorTypeDME.Checked;
end;

procedure TMainForm.StartPointChanged_ (parameter: TFunctionParam);
var
	param:				TPointParam;
	tmpPoint:	        TPoint;
begin
{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - StartPointChanged');
{$ENDIF}

	param := TPointParam (parameter);

	if param.CheckEqual and FOldStartChangedParam.CheckEqual and param.IsEqual (FOldStartChangedParam) then
	begin
{$IFDEF SHOW_MESSAGEBOX}
		AddLogMessage ('Exit - StartPointChanged');
{$ENDIF}
		exit;
	end;

	if param.SignifigantPoint = nil then
	begin
		tmpPoint := PointAlongPlane (FTurn.TurnInfo.PrevTurnInfo.Start.Point,
						param.Direction, param.Distance);
		FTurn.TurnInfo.Start.Point.Assign (tmpPoint);
		tmpPoint.Free;
	end
	else
	begin
		FTurn.TurnInfo.Start.SignificantPointID := param.SignifigantPoint.ID;
		FTurn.TurnInfo.Start.Point.Assign (param.SignifigantPoint.Prj);
		param.Distance := ReturnDistanceInMeters (FTurn.TurnInfo.PrevTurnInfo.Start.Point, param.SignifigantPoint.Prj);
		param.Direction := ReturnAngleInRadians (FTurn.TurnInfo.PrevTurnInfo.Start.Point, param.SignifigantPoint.Prj);

		pcTurnStartCourse.P.BeginUpdate (False);
		pcTurnStartCourse.P.Value := param.Direction;
		AddParamControlUpdateList (pcTurnStartCourse.P);

		pcTurnStartDistance.P.BeginUpdate (False);
		pcTurnStartDistance.P.Value := param.Distance;
		AddParamControlUpdateList (pcTurnStartDistance.P);
	end;

	FTurn.TurnInfo.Start.Direction := param.Direction;
	FTurn.TurnInfo.Start.Distance := param.Distance;

	if FTurn.TurnInfo.TurnType <> tuAtHeight then
	begin
		HeightNominalChanged (param.Distance * FTurn.TurnInfo.PDGNominal);

		pcHeightProtect.P.BeginUpdate (True);
		pcHeightProtect.P.MinValue := GConst.PANSOPS.Constant [dpH_abv_DER].Value +
			param.Distance * FTurn.TurnInfo.PDG;
		pcHeightProtect.P.Value := param.Distance * FTurn.TurnInfo.PDGProtect;
		AddParamControlUpdateList (pcHeightProtect.P);
	end;

	pcTurnNextCourse.P.BeginUpdate (True);
	pcTurnNextCourse.P.InConvertionPoint := PrjToGeo (FTurn.TurnInfo.Start.Point).AsPoint;
	pcTurnNextCourse.P.OutConvertionPoint := FTurn.TurnInfo.Start.Point.Clone.AsPoint;
	AddParamControlUpdateList (pcTurnNextCourse.P);

	CreateStraightAreaOnTurnPage ();

	FOldNextChangedParam.CheckEqual := False;
	FilterNextPoints (nil);

	FOldStartChangedParam.Assign (param);

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - StartPointChanged');
{$ENDIF}
end;

procedure TMainForm.NextPointChanged_ (parameter: TFunctionParam);
var
	param:				TPointParam;
	tmpPoint:			TPoint;
begin
{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('Begin - NextPointChanged');
{$ENDIF}

	param := TPointParam (parameter);

	if param.CheckEqual and FOldNextChangedParam.CheckEqual and param.IsEqual (FOldNextChangedParam) then
	begin
{$IFDEF SHOW_MESSAGEBOX}
		AddLogMessage ('Exit - NextPointChanged');
{$ENDIF}
		exit;
	end;

	if param.SignifigantPoint = nil then
	begin
		tmpPoint := PointAlongPlane (FTurn.TurnInfo.Start.Point,
						param.Direction, param.Distance);
		FTurn.TurnInfo.Next.Point.Assign (tmpPoint);
		tmpPoint.Free;
	end
	else
	begin
		FTurn.TurnInfo.Next.SignificantPointID := param.SignifigantPoint.ID;
		FTurn.TurnInfo.Next.Point.Assign (param.SignifigantPoint.Prj);
		param.Distance := ReturnDistanceInMeters (FTurn.TurnInfo.Start.Point, param.SignifigantPoint.Prj);
		param.Direction := ReturnAngleInRadians (FTurn.TurnInfo.Start.Point, param.SignifigantPoint.Prj);

		pcTurnNextCourse.P.BeginUpdate (False);
		pcTurnNextCourse.P.Value := param.Direction;
		AddParamControlUpdateList (pcTurnNextCourse.P);

		pcTurnNextDistance.P.BeginUpdate (False);
		pcTurnNextDistance.P.Value := param.Distance;
		AddParamControlUpdateList (pcTurnNextDistance.P);
	end;

	FTurn.TurnInfo.Next.Direction := param.Direction;
	FTurn.TurnInfo.Next.Distance := param.Distance;

	StartOrNextPointChanged;

	FOldNextChangedParam.Assign (param);

{$IFDEF SHOW_MESSAGEBOX}
	AddLogMessage ('End - NextPointChanged');
{$ENDIF}
end;

procedure TMainForm.OldValuesInit;
begin
	FOldStartChangedParam := TPointParam.Create;
	FOldNextChangedParam := TPointParam.Create;
end;

procedure TMainForm.OldValuesFree;
begin
	FOldStartChangedParam.Free;
	FOldNextChangedParam.Free;
end;

procedure TMainForm.AddParamControlUpdateList (parameterControl: TParameter);
begin
	FParamControlUpdateList.Remove (parameterControl);
	FParamControlUpdateList.Add (parameterControl);
	FCallManager.Call (UpdateParameterControl, nil);
end;

procedure TMainForm.UpdateParameterControl (parameter: TFunctionParam);
var
	paramControl:	TParameter;
begin

	if FLockedUpdateParamControl then
		exit;

	FLockedUpdateParamControl := True;

	while FParamControlUpdateList.Count > 0 do
	begin
		paramControl := TParameter (FParamControlUpdateList [0]);
		FParamControlUpdateList.Delete (0);
		paramControl.EndUpdate;
	end;

	FParamControlUpdateList.Clear;

	FLockedUpdateParamControl := False;
end;

procedure TMainForm.HeightNominalChanged (newValue: Double);
begin
	if FTurn.TurnInfo.Start.HeightNominal = newValue then
		exit; 
	FTurn.TurnInfo.Start.HeightNominal := newValue;
	FTurn.TurnInfo.NominalRadius := FParams.ToRadius (FTurn.TurnInfo.BankAngle,
		FTurn.TurnInfo.IAS, FTurn.TurnInfo.Start.HeightNominal);
end;

procedure TMainForm.ReportForm_bSaveClick (Sender: TObject);
const
	turnTypeStrArray: Array [TTurnType] of string = ('Fly By', 'Fly Over', 'At Height');
	codeTypeStrArray: Array [TCodeType] of string = ('TF', 'CF', 'DF');
var
	reportFile: TReportFile;
	tabChar: String;
begin
	if not saveDlgReport.Execute then
		exit;
	reportFile := TReportFile.Create (saveDlgReport.FileName, 'RNAV Departure - Calculation report');

	tabChar := '&nbsp;&nbsp;&nbsp;&nbsp;';

	with reportFile do
	begin
		WriteText ('<P><b><big>Current user name:</big></b> ' + GCurrentUserName + '</P>');

		//--- begin: Parameters.
		WriteText ('<P><b><big>Parameters</big></b></P>');

		WriteParam ('Aerodrome', cbAerodromes.Text);
		WriteTextLine (tabChar + 'ICAO Code: ' + FParams.AHP.AixmID);
		WriteTextLine (tabChar + 'Elevation: ' + pcAerodrome.ValueControl.Text + ' ' + pcAerodrome.UnitControl.Caption);
		WriteTextLine (tabChar + 'Mag. var: ' + editAerodromeMagVar.Text + ' ' + labAerodromeMagVarUnit.Caption);
		WriteTextLine (tabChar + 'ISA+: ' + editAerodromeISA.Text + ' ' + labAerodromeISAUnit.Caption);

		WriteParam ('RWY', cbRWY.Text);

		WriteParam ('RWY Direction', cbRwyDirection.Text);
		WriteTextLine (tabChar + 'True Brg: ' + editRwyDirectionTrueBrg.Text + ' ' + labRwyDirectionUnit.Caption);
		WriteTextLine (tabChar + 'Mag Brg: ' + editRwyDirectionMagBrg.Text + ' ' + labRwyDirectionMagBrgUnit.Caption);
		WriteTextLine (tabChar + 'Elevation: ' + pcRwyDirElev.ValueControl.Text + ' ' + pcRwyDirElev.UnitControl.Caption);
		WriteTextLine (tabChar + 'Clearway: ' + prRwyDirClearway.ValueControl.Text + ' ' + prRwyDirClearway.UnitControl.Caption);

		WriteParam ('Departure Type', IfThen (rbDepartureTypeStaight.Checked, 'Straight', 'Turn'));

		WriteParam ('Sensor Type', IfThen (rbSensorTypeGNSS.Checked, 'GNSS', 'DME/DME'));
		if rbSensorTypeDME.Checked then
		begin
			reportFile.WriteTextLine (tabChar + 'DME Installed: ' +
				IfThen ((cbDMEInstalled.ItemIndex = 0), 'prior to 1 Jan 1989', 'after 1 Jan 1989'));
			reportFile.WriteTextLine (tabChar + 'Coverage DME Count: ' + cbDMECount.Text);
		end;

		WriteParam ('Modelling Area Radius', pcModelAreaRadius.ValueControl.Text, pcModelAreaRadius.UnitControl.Caption);
		//--- end.

		if rbDepartureTypeStaight.Checked then
		begin
			//--- begin: Striaght.
			WriteText ('<P><b><big>Straight</big></b></P>');

			WriteParam ('Next Point', IfThen (rbStraigthSelectPoint.Checked, 'Selected', 'Created'));
			if rbStraigthSelectPoint.Checked then
				WriteTextLine (tabChar + 'Signifigant Point: ' + cbStraightSgfPoint.Text + ' (' + labStraightSgfPointType.Caption + ')')
			else
			begin
				WriteTextLine (tabChar + 'Course: ' + pcStraightCourse.ValueControl.Text + ' ' + pcStraightCourse.UnitControl.Caption);
				WriteTextLine (tabChar + 'Distance: ' + pcStraightDistance.ValueControl.Text + ' ' + pcStraightDistance.UnitControl.Caption);
			end;

			WriteParam ('Applicable PDG', pcStraightPDG.ValueControl.Text, pcTurnPrevPDG.UnitControl.Caption);
			WriteParam ('Required PDG', pcStraightReqPDG.ValueControl.Text, pcStraightReqPDG.UnitControl.Caption);
			WriteParam ('Required PDG Obstacle ID', editStaightInfoMOCObsName.Text);			
			WriteParam ('Height at which gradient in excees of 3.3% no required',
				editStraightInfoElevGrd.Text, labStraightInfoElevGrdUnit.Caption);
			WriteParam ('Track adjustment', editStaightInfoAdjustment.Text, labStaightInfoAdjustmentUnit.Caption);			
			//--- end.
		end
		else
		begin
			//--- begin: Turn
			WriteText ('<P><b><big>Turn Segment - 1</big></b></P>');

			WriteParam ('Aircraft Category', cbTurnCategory.Text);
			WriteParam ('IAS', pcIAS.ValueControl.Text, pcIAS.UnitControl.Caption);
			WriteParam ('Bank Angle', pcBankAngle.ValueControl.Text, pcBankAngle.UnitControl.Caption);
			WriteParam ('TAS', pcTAS.ValueControl.Text, pcTAS.UnitControl.Caption);
			WriteParam ('Turn Radius', pcTurnRadius.ValueControl.Text, pcTurnRadius.UnitControl.Caption);
			WriteParam ('Turn Type', turnTypeStrArray [FTurn.TurnInfo.TurnType]);
			if FTurn.TurnInfo.tmp_codeTypeIsCF then
				WriteParam ('Code Type', codeTypeStrArray [ctCF])
			else
				WriteParam ('Code Type', codeTypeStrArray [FTurn.TurnInfo.CodeType]);

			WriteParam ('Turn Point', IfThen (rbTurnStartSignificantPoints.Checked, 'Selected', 'Created'));
			if rbTurnStartSignificantPoints.Checked then
				WriteTextLine (tabChar + 'Signifigant Point: ' + cbTurnStartSignificantPoints.Text + ' (' + labTurnStartSignifigantPointsUnit.Caption + ')')
			else
			begin
				WriteTextLine (tabChar + 'Course: ' + pcTurnStartCourse.ValueControl.Text + ' ' + pcTurnStartCourse.UnitControl.Caption);
				WriteTextLine (tabChar + 'Distance: ' + pcTurnStartDistance.ValueControl.Text + ' ' + pcTurnStartDistance.UnitControl.Caption);
			end;

			WriteParam ('Next Point', IfThen (rbTurnNextSignificantPoints.Checked, 'Selected', 'Created'));
			if rbTurnNextSignificantPoints.Checked then
				WriteTextLine (tabChar + 'Signifigant Point: ' + cbTurnNextSignificantPoints.Text + ' (' + labTurnNextSignifigantPointsUnit.Caption + ')')
			else
			begin
				WriteTextLine (tabChar + 'Course: ' + pcTurnNextCourse.ValueControl.Text + ' ' + pcTurnNextCourse.UnitControl.Caption);
				WriteTextLine (tabChar + 'Distance: ' + pcTurnNextDistance.ValueControl.Text + ' ' + pcTurnNextDistance.UnitControl.Caption);
			end;

			WriteParam ('Turn Side', cbTurnDFSide.Text);

			WriteTextLine ('<b>Initial Area PDG</b>');
			WriteTextLine (tabChar + 'Input: ' + pcTurnPrevPDG.ValueControl.Text + ' ' + pcTurnPrevPDG.UnitControl.Caption);
			WriteTextLine (tabChar + 'Required: ' + pcTurnPrevReqPDG.ValueControl.Text + ' ' + pcTurnPrevReqPDG.UnitControl.Caption);

			WriteTextLine ('<b>Turn Area PDG</b>');
			WriteTextLine (tabChar + 'Input: ' + pcTurnCurPDG.ValueControl.Text + ' ' + pcTurnCurPDG.UnitControl.Caption);
			WriteTextLine (tabChar + 'Required: ' + pcTurnCurReqPDG.ValueControl.Text + ' ' + pcTurnCurReqPDG.UnitControl.Caption);

			if FTurn.TurnInfo.TurnType = tuAtHeight then
				WriteParam ('Height', pcTurnKKHeight.ValueControl.Text, pcTurnKKHeight.UnitControl.Caption)
			else
			begin
				WriteParam ('Altitude by 10%', pcHeightProtect.ValueControl.Text, pcHeightProtect.UnitControl.Caption);
				WriteParam ('KK'' Line Height', pcTurnKKHeight.ValueControl.Text, pcTurnKKHeight.UnitControl.Caption);
				WriteParam ('KK'' Line Distance', pcTurnKKDistance.ValueControl.Text, pcTurnKKDistance.UnitControl.Caption);
			end;


			//--- end.
		end;

		//--- begin: Obstacle report.
		WriteText ('<P><b><big>Obstacles in Area</big></b></P>');
		//--- end.
	end;

	FReportForm.SaveReport (reportFile);

	reportFile.Free;
end;

procedure TMainForm.bSaveClick(Sender: TObject);
var
	procTransition: IProcedureTransition;
	segmentLeg: ISegmentLeg;
	departureLeg: IDepartureLeg;
	saveForm: TSaveForm;
	endSigPointId: String;
	tmpSigPoint: TSignificantPoint;
	endSigPoint: TSignificantPoint;
	emptyLine: TPolyline;
	tmpPart: TPart;
	tmpPoint,
	tmpPoint2: Geometry.TPoint;
	firstLegPDG: Double;
	aixmSegmentPoint: ISegmentPoint;
begin
	GAixmProcedure.Id := '0';

	GAixmProcedure.Note := '';
	GAixmProcedure.Description := '';
	GAixmProcedure.CommunicationFailureDescription := '';
	GAixmProcedure.RNAV := CreateIBoolean (True);
	GAixmProcedure.ServicesAirportId := FParams.AHP.ID;

	if FFormType = dftStraight then
	begin
		endSigPointId := FTurn.TurnInfo.Start.SignificantPointID;
		GAixmProcedure.IsLimitedTo := nil;
		firstLegPDG := FTurn.TurnInfo.PDG;
	end
	else
	begin
		endSigPointId := FTurn.TurnInfo.Next.SignificantPointID;
		GAixmProcedure.IsLimitedTo := CoAircraftCharacteristic.Create () as IAircraftCharacteristic;
		GAixmProcedure.IsLimitedTo.AircraftLandingCategory := Integer (FTurn.TurnInfo.AircraftCategory);
		firstLegPDG := FTurn.TurnInfo.PrevTurnInfo.PDG;
	end;

	GAixmProcedure.TransitionList.Clear ();
	procTransition := CoProcedureTransition.Create () as IProcedureTransition;
	procTransition.Description := '';
	procTransition.RunwayDirectionId := CreateIString (FParams.StartRwyDirection.ID);
	procTransition.type_ := ProcedurePhaseType_RWY;
	procTransition.LegList := CoDepartureLegList.Create () as ISegmentLegList;

	//--- begin: Create Leg with empty geometry. GeomLength = 0
	departureLeg := CoDepartureLeg.Create () as IDepartureLeg;
	segmentLeg := departureLeg as ISegmentLeg;
	segmentLeg.SeqNumberARINC := CreateIInt32 (1);
	segmentLeg.Note := '';
	segmentLeg.LowerLimitAltitude := CreateDefaulIDistanceVertical (0);
	segmentLeg.LowerLimitReference := VerticalReferenceType_W84;
	segmentLeg.UpperLimitAltitude := CreateDefaulIDistance (0);
	segmentLeg.UpperLimitReference := VerticalReferenceType_W84;
	segmentLeg.ProcedureTurnRequired := CreateIBoolean (False);
	emptyLine := TPolyline.Create ();
	tmpPart := TPart.Create ();
	tmpPoint := TPoint.Create (FParams.StartRwyDirection.Prj);
	tmpPart.AddPoint (tmpPoint);
	tmpPoint2 := PointAlongPlane (tmpPoint, FParams.StartRwyDirection.Direction, 1);
	tmpPart.AddPoint (tmpPoint2);
	emptyLine.AddPart (tmpPart);
	segmentLeg.Trajectory := PolylineToIAIXMCurve (emptyLine);
	tmpPoint.Free;
	tmpPoint2.Free;
	tmpPart.Free;
	emptyLine.Free;

	segmentLeg.EndPoint := nil;
	segmentLeg.LegTypeARINC := SegmentPathType_IF;
	segmentLeg.Course := CreateIDouble (FParams.StartRwyDirection.TrueBearing);
	segmentLeg.CourseType := CourseType_TRUE_TRACK;
	segmentLeg.Length := CreateDefaulIDistance (0);

	procTransition.LegList.AsISIDList.Add (departureLeg);
	//--- end.

	//--- begin: Create next segmentLeg.
	departureLeg := CoDepartureLeg.Create () as IDepartureLeg;
	segmentLeg := departureLeg as ISegmentLeg;
	segmentLeg.SeqNumberARINC := CreateIInt32 (2);
	segmentLeg.Note := '';

	segmentLeg.LowerLimitAltitude := CreateDefaulIDistanceVertical (
		FTurn.TurnInfo.Start.Distance * firstLegPDG +
		FParams.StartRwyDirection.ElevTdz);

	segmentLeg.UpperLimitAltitude := CreateDefaulIDistance (
		FTurn.TurnInfo.Start.Distance * FDefaultProtectPDG +
		FParams.StartRwyDirection.ElevTdz);

	segmentLeg.LowerLimitReference := VerticalReferenceType_W84;
	segmentLeg.UpperLimitReference := VerticalReferenceType_W84;
	segmentLeg.ProcedureTurnRequired := CreateIBoolean (False);
	segmentLeg.Course := CreateIDouble (
		DirToAzimuth (FTurn.TurnInfo.Start.Point, FTurn.TurnInfo.Start.Direction, GPrjSR, GGeoSR));
	segmentLeg.CourseType := CourseType_TRUE_TRACK;

	segmentLeg.VerticalAngle := CreateIDouble (RadToDeg (ArcTanh (firstLegPDG)));
	segmentLeg.Length := CreateDefaulIDistance (FTurn.TurnInfo.Start.Distance);

	if FFormType = dftStraight then
	begin
		segmentLeg.Trajectory := PolylineToIAIXMCurve (FStraight.Segment.Area.NominalLine);
		segmentLeg.LegTypeARINC := SegmentPathType_CF;
	end
	else
	begin
		segmentLeg.Trajectory := PolylineToIAIXMCurve (FTurn.TurnInfo.PrevTurnInfo.Segment.AreaToKK.NominalLine);
		segmentLeg.LegTypeARINC := IfThen ((FTurn.TurnInfo.TurnType = tuAtHeight),
			SegmentPathType_CA, SegmentPathType_CF);
		segmentLeg.SpeedLimit := CoSpeed.Create () as ISpeed;
		segmentLeg.SpeedLimit.value := FTurn.TurnInfo.IAS * (3600.0 / 1000.0);
		segmentLeg.SpeedLimit.uom := UomSpeed_KM_H;
		segmentLeg.SpeedReference := SpeedReferenceType_IAS;
	end;

	if FTurn.TurnInfo.TurnType = tuAtHeight then
		segmentLeg.EndPoint := nil
	else
	begin
		//--- --- begin: Create DesignatedPoint.
		segmentLeg.EndPoint := CoTerminalSegmentPoint.Create () as ITerminalSegmentPoint;
		segmentLeg.EndPoint.Role := ProcedureFixRoleType_OTHER;
		aixmSegmentPoint := segmentLeg.EndPoint as ISegmentPoint;
		aixmSegmentPoint.PointChoice := CoSignificantPoint.Create () as ISignificantPoint;
		aixmSegmentPoint.PointChoice.FixDesignatedPoint := CoDesignatedPoint.Create () as IDesignatedPoint;
		if FTurn.TurnInfo.Start.SignificantPointID <> '' then
		begin
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Id := FTurn.TurnInfo.Start.SignificantPointID;
			tmpSigPoint := GetSignificantPoint (FTurn.TurnInfo.Start.SignificantPointID);
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Designator := tmpSigPoint.AixmID;
		end
		else
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Id := '0';
		aixmSegmentPoint.PointChoice.FixDesignatedPoint.Point := CoAIXMPoint.Create () as IAIXMPoint;
		tmpPoint := PrjToGeo (FTurn.TurnInfo.Start.Point).AsPoint;
		aixmSegmentPoint.PointChoice.FixDesignatedPoint.Point.AsIGMLPoint.PutCoord (tmpPoint.X, tmpPoint.Y);
		tmpPoint.Free;
		//--- --- end.
	end;

	if (FFormType <> dftStraight) and (FTurn.TurnInfo.TurnType <> tuAtHeight) then
		aixmSegmentPoint.FlyOver := CreateIBoolean (FTurn.TurnInfo.TurnType = tuFlyOver);

	procTransition.LegList.AsISIDList.Add (departureLeg);
	//--- end.  	

	if FFormType = dftMultiTurn then
	begin
		//--- begin: Next LEG.
		departureLeg := CoDepartureLeg.Create () as IDepartureLeg;
		segmentLeg := departureLeg as ISegmentLeg;
		segmentLeg.SeqNumberARINC := CreateIInt32 (3);
		segmentLeg.Note := '';
		
		segmentLeg.LowerLimitAltitude := CreateDefaulIDistanceVertical (
			FTurn.TurnInfo.Next.Distance * FTurn.TurnInfo.PDG +
			FParams.StartRwyDirection.ElevTdz);

		segmentLeg.UpperLimitAltitude := CreateDefaulIDistance (
			FTurn.TurnInfo.Next.Distance * FTurn.TurnInfo.PDGProtect +
			FParams.StartRwyDirection.ElevTdz);

		segmentLeg.LowerLimitReference := VerticalReferenceType_W84;
		segmentLeg.UpperLimitReference := VerticalReferenceType_W84;

		if FTurn.TurnInfo.CodeType = ctTF then
			segmentLeg.LegTypeARINC := SegmentPathType_TF
		else if FTurn.TurnInfo.CodeType = ctDF then
			segmentLeg.LegTypeARINC := SegmentPathType_DF
		else
			segmentLeg.LegTypeARINC := SegmentPathType_CF;

		segmentLeg.Course := CreateIDouble (
			DirToAzimuth (FTurn.TurnInfo.Next.Point, FTurn.TurnInfo.Next.Direction, GPrjSR, GGeoSR));
		segmentLeg.CourseType := CourseType_TRUE_TRACK;
		segmentLeg.VerticalAngle := CreateIDouble (RadToDeg (ArcTanh (FTurn.TurnInfo.PDG)));
		segmentLeg.Length := CreateDefaulIDistance (FTurn.TurnInfo.Next.Distance);
		segmentLeg.ProcedureTurnRequired := CreateIBoolean (True);
		if FTurn.TurnInfo.TurnSide = sideLeft then
			segmentLeg.TurnDirection := DirectionTurnType_LEFT
		else if FTurn.TurnInfo.TurnSide = sideRight then
			segmentLeg.TurnDirection := DirectionTurnType_RIGHT
		else
			segmentLeg.TurnDirection := DirectionTurnType_EITHER;

		segmentLeg.BankAngle := CreateIDouble (RadToDeg (FTurn.TurnInfo.BankAngle));
		segmentLeg.SpeedLimit := CoSpeed.Create () as ISpeed;
		segmentLeg.SpeedLimit.value := FTurn.TurnInfo.IAS * (3600.0 / 1000.0);
		segmentLeg.SpeedLimit.uom := UomSpeed_KM_H;
		segmentLeg.SpeedReference := SpeedReferenceType_IAS;

		segmentLeg.Trajectory := PolylineToIAIXMCurve (FTurn.TurnInfo.Segment.Area.NominalLine);

		//--- --- begin: Create DesignatedPoint.
		segmentLeg.EndPoint := CoTerminalSegmentPoint.Create () as ITerminalSegmentPoint;
		segmentLeg.EndPoint.Role := ProcedureFixRoleType_OTHER;
		aixmSegmentPoint := segmentLeg.EndPoint as ISegmentPoint;
		aixmSegmentPoint.PointChoice := CoSignificantPoint.Create () as ISignificantPoint;
		aixmSegmentPoint.PointChoice.FixDesignatedPoint := CoDesignatedPoint.Create () as IDesignatedPoint;
		if FTurn.TurnInfo.Next.SignificantPointID <> '' then
		begin
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Id := FTurn.TurnInfo.Next.SignificantPointID;
			tmpSigPoint := GetSignificantPoint (FTurn.TurnInfo.Next.SignificantPointID);
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Designator := tmpSigPoint.AixmID;
		end
		else
			aixmSegmentPoint.PointChoice.FixDesignatedPoint.Id := '0';
		aixmSegmentPoint.PointChoice.FixDesignatedPoint.Point := CoAIXMPoint.Create () as IAIXMPoint;
		tmpPoint := PrjToGeo (FTurn.TurnInfo.Next.Point).AsPoint;
		aixmSegmentPoint.PointChoice.FixDesignatedPoint.Point.AsIGMLPoint.PutCoord (tmpPoint.X, tmpPoint.Y);
		tmpPoint.Free;
		//--- --- end.

		procTransition.LegList.AsISIDList.Add (departureLeg);
		//--- end.
	end;

	endSigPoint := GetSignificantPoint (endSigPointId);

	if endSigPoint = nil then
		GAixmProcedure.Name := ''
	else
		GAixmProcedure.Name := endSigPoint.Name;
	
	GAixmProcedure.TransitionList.Add (procTransition);

	saveForm := TSaveForm.Create (self);
	SetWindowLong(saveForm.Handle, GWL_HWNDPARENT, FApplicationHandle);
	saveForm.AIXMProcedure := GAixmProcedure;

	if saveForm.ShowModal <> mrOK then
	begin
		saveForm.Free;
		exit;
	end;

	GObjectDirectory.SetProcedure (Integer (Pointer (GAixmProcedure)));

	saveForm.Free;
	bSave.Enabled := False;
end;

function TMainForm.GetSignificantPoint (id: String): TSignificantPoint;
var
	i: Integer;
begin
	result := nil;
	if id = '' then
		exit;

	for i := 0 to FParams.SignificantPoints.Count - 1 do
	begin

		if FParams.SignificantPoints.Item [i].ID = id then
		begin
			result := FParams.SignificantPoints.Item [i];
			exit;
		end;
	end;
end;

procedure TMainForm.FillControlMUI ();
begin

//--- begin: Fill Common.
	GMuiLoader.LoadString (GMuiStrings.Departure, 51);
	GMuiLoader.LoadString (GMuiStrings.Parameters, 52);
	GMuiLoader.LoadString (GMuiStrings.Straight, 53);
	GMuiLoader.LoadString (GMuiStrings.Turn, 54);
	GMuiLoader.LoadString (GMuiStrings.ToRight, 55);
	GMuiLoader.LoadString (GMuiStrings.ToLeft, 56);
//--- end.

//--- begin: Fill Parameters page.

	GMuiLoader.LoadString (grbDepartureType, 101);
	GMuiLoader.LoadString (rbDepartureTypeStaight, 102);
	GMuiLoader.LoadString (rbDepartureTypeTurn, 103);
	GMuiLoader.LoadString (gbSensorType, 104);
	GMuiLoader.LoadString (rbSensorTypeGNSS, 105);
	GMuiLoader.LoadString (rbSensorTypeDME, 106);
	GMuiLoader.LoadString (pcModelAreaRadius.DescriptionControl, 107);
	GMuiLoader.LoadString (grbAerodrome, 108);
	GMuiLoader.LoadString (labAerodromes, 109);
	GMuiLoader.LoadString (labRWYName, 109);
	GMuiLoader.LoadString (labRwyDirectionName, 109);
	GMuiLoader.LoadString (Label2, 110);
	GMuiLoader.LoadString (pcAerodrome.DescriptionControl, 111);
	GMuiLoader.LoadString (pcRwyDirElev.DescriptionControl, 111);
	GMuiLoader.LoadString (labAerodromeMagVar, 112);
	GMuiLoader.LoadString (labAerodromeISA, 113);
	GMuiLoader.LoadString (grbRWY, 114);
	GMuiLoader.LoadString (grbDME, 116);
	GMuiLoader.LoadString (labDMEInstalled, 117);
	GMuiLoader.LoadString (GMuiStrings.PriorJan89, 118);
	GMuiLoader.LoadString (GMuiStrings.AfterJan89, 119);
	GMuiLoader.LoadString (labDMECount, 120);
	GMuiLoader.LoadString (grbRwyDirection, 121);
	GMuiLoader.LoadString (labRwyDirectionTrueBrg, 122);
	GMuiLoader.LoadString (labRwyDirectionMagBrg, 123);
	GMuiLoader.LoadString (prRwyDirClearway.DescriptionControl, 124);

	GMuiLoader.LoadString (bHelp, 125);
	GMuiLoader.LoadString (bPrev, 126);
	GMuiLoader.LoadString (bNext, 127);
	GMuiLoader.LoadString (bApply, 128);
	GMuiLoader.LoadString (bReport, 129);
	GMuiLoader.LoadString (bSave, 130);
	GMuiLoader.LoadString (bClose, 131);

	cbDMEInstalled.Items.Clear;
	cbDMEInstalled.Items.Add (GMuiStrings.PriorJan89);
	cbDMEInstalled.Items.Add (GMuiStrings.AfterJan89);
	cbDMEInstalled.ItemIndex := 0;
//--- eng.

//--- begin: Fill Straight page.

	GMuiLoader.LoadString (grbStraightPoint, 201);
	GMuiLoader.LoadString (GMuiStrings.SignificantPoints, 202);
	GMuiLoader.LoadString (rbStraigthCreatePoint, 203);
	GMuiLoader.LoadString (pcStraightCourse.DescriptionControl, 204);
	GMuiLoader.LoadString (pcStraightDistance.DescriptionControl, 205);
	GMuiLoader.LoadString (pcStraightPDG.DescriptionControl, 206);
	GMuiLoader.LoadString (pcStraightReqPDG.DescriptionControl, 207);
	GMuiLoader.LoadString (labStraightInfoElevGrd, 208);
	GMuiLoader.LoadString (labStaightInfoAdjustment, 209);
	GMuiLoader.LoadString (labStaightInfoMOCObsName, 210);
//--- end.

//--- begin: Fill Turn page.

	GMuiLoader.LoadString (grbTurnAircraftParams, 300);
	GMuiLoader.LoadString (labCategory, 301);
	GMuiLoader.LoadString (pcIAS.DescriptionControl, 302);
	GMuiLoader.LoadString (pcBankAngle.DescriptionControl, 303);
	GMuiLoader.LoadString (pcTAS.DescriptionControl, 304);
	GMuiLoader.LoadString (pcTurnRadius.DescriptionControl, 305);
	GMuiLoader.LoadString (pcHeightProtect.DescriptionControl, 306);
	GMuiLoader.LoadString (grbTurnType, 307);
	GMuiLoader.LoadString (rbTurnTypeFlyBy, 308);
	GMuiLoader.LoadString (rbTurnTypeFlyOver, 309);
	GMuiLoader.LoadString (rbTurnTypeAtHeight, 310);
	GMuiLoader.LoadString (grbTurnCodeType, 311);
	GMuiLoader.LoadString (rbTurnCodeTypeTF, 312);
	GMuiLoader.LoadString (rbTurnCodeTypeCF, 313);
	GMuiLoader.LoadString (rbTurnCodeTypeDF, 314);
	GMuiLoader.LoadString (grbTurnStartPoint, 315);
	GMuiLoader.LoadString (rbTurnStartCreatePoint, 203);
	GMuiLoader.LoadString (pcTurnStartCourse.DescriptionControl, 204);
	GMuiLoader.LoadString (pcTurnStartDistance.DescriptionControl, 205);
	GMuiLoader.LoadString (grbTurnNextPoint, 201);
	GMuiLoader.LoadString (rbTurnNextCreatePoint, 203);
	GMuiLoader.LoadString (pcTurnNextCourse.DescriptionControl, 204);
	GMuiLoader.LoadString (pcTurnNextDistance.DescriptionControl, 205);
	GMuiLoader.LoadString (GMuiStrings.ControlKKLine, 316);
	GMuiLoader.LoadString (pcTurnKKHeight.DescriptionControl, 317);
	GMuiLoader.LoadString (pcTurnKKDistance.DescriptionControl, 205);
	GMuiLoader.LoadString (grbPrevPDG, 319);
	GMuiLoader.LoadString (pcTurnPrevReqPDG.DescriptionControl, 320);
	GMuiLoader.LoadString (grbNextPDG, 321);
	GMuiLoader.LoadString (pcTurnCurReqPDG.DescriptionControl, 320);
	GMuiLoader.LoadString (labTurnDFSide, 322);
	GMuiLoader.LoadString (GMuiStrings.Left, 323);
	GMuiLoader.LoadString (GMuiStrings.Right, 324);

	cbTurnDFSide.Items.Clear;
	cbTurnDFSide.Items.Add (GMuiStrings.Left);
	cbTurnDFSide.Items.Add (GMuiStrings.Right);
	cbTurnDFSide.ItemIndex := 0;
//--- end.

end;

procedure TMainForm.bHelpClick(Sender: TObject);
var
	helpId: Integer;
begin
	if FCurrentPage = dpgParameters then
		helpId := 2100
	else if FCurrentPage = dpgStraight then
		helpId := 2200
	else
		helpId := 2300;

	ShowHelp (self.Handle, helpId);
end;

function TMainForm.FormHelp (Command: Word; Data: Integer; var CallHelp: Boolean): Boolean;
begin
	CallHelp := False;
//	bHelpClick (nil);
end;

procedure TMainForm.FormKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
	if Key = VK_F1 then
		bHelpClick (nil);
//
end;

end.
