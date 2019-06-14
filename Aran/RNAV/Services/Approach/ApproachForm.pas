{$INCLUDE Settings}

unit ApproachForm;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls,
  Forms, Dialogs, Grids, StdCtrls, ComCtrls, ExtCtrls,
  Mask, Parameter, ConstantsContract, AIXM_TLB,
  UIContract, Geometry, FIXUnit, HelperClasses, CollectionUnit,
  AIXMTypes, IntervalUnit, FlightPhaseUnit, ObstacleInfoUnit,
  AddBranchFUnit, Buttons, Common;

type

  TMainForm = class(TForm)
	pageContMain: TPageControl;
	tabInitialParameters: TTabSheet;
	tabFinalApproachCourse: TTabSheet;
    tabApproach_ArrivalSegments: TTabSheet;
    tabFinalAndIntermediateApproach: TTabSheet;
    tabStraightMissedApproach: TTabSheet;
    tabTurningMissedApproach: TTabSheet;
	udIFCourse: TUpDown;
    udApproacCourse_OLD: TUpDown;
    bOK: TButton;
	bReport: TButton;
	bHelp: TButton;
    btnAddBranch: TButton;

	PrevBtn: TBitBtn;
	NextBtn: TBitBtn;
	rgSelectFirstAncorPoint: TRadioGroup;
	rgChooseTrack: TRadioGroup;
	rgAlignment: TRadioGroup;
    rbMAPtByDist: TRadioButton;
	rbMAPtFromList: TRadioButton;
	rbFAFFromList: TRadioButton;
    rbFAFByDist: TRadioButton;
    rbIFFromList: TRadioButton;
	rbIFByDistAndAzimuth: TRadioButton;

	grbAerodrome: TGroupBox;
	grbRwyDirection: TGroupBox;
	grbIF: TGroupBox;
	grbStraightCreatePoint: TGroupBox;
	gb0203: TGroupBox;
	grbMAPt: TGroupBox;
	grbFAF: TGroupBox;
	GroupBox1: TGroupBox;

	cbAerodromes: TComboBox;
	cbRwyDirection: TComboBox;
	cbCategory: TComboBox;
	ComboTrackIntervals: TComboBox;
	cbMAPtList: TComboBox;
	cbFAFList: TComboBox;
	cbIFList: TComboBox;
	cbFirstAnchorPoint: TComboBox;
	cbSecondAnchorPoint: TComboBox;
	cbMAclimbGradient: TComboBox;

	editAerodromeAixmID: TEdit;
	editAerodromeElev: TEdit;
	editAerodromeISA: TEdit;
	editAerodromeMagVar: TEdit;
	editAbeamDistance: TEdit;
	editAlongDistance: TEdit;
	editCourse: TEdit;
	editRwyDirectionTrueBrg: TEdit;
	editRwyDirectionMagBrg: TEdit;
	editRwyDirectionElev: TEdit;
	editRwyDirectionClearway: TEdit;
	editMAPtDistance: TEdit;
	editFAFDistance: TEdit;
	editIFDistance: TEdit;
	editMAPtCourse: TEdit;
	editFAFGradient: TEdit;
	editFAFAltitude: TEdit;
	editIFAltitude: TEdit;
	editModelAreaRadius: TEdit;
	editIFCourse: TEdit;
    editSOCFromMAPtDistance: TEdit;
	editOCA: TEdit;
    editRwyDirectionDisplacement: TEdit;
	editMAOCA: TEdit;
	Edit3: TEdit;
	Edit4: TEdit;
	Edit5: TEdit;
	Edit6: TEdit;
	Edit7: TEdit;
	Edit8: TEdit;
    editMinimalOCA: TEdit;

	labAerodromes: TLabel;
	labAerodromeElev: TLabel;
	labAerodromeElevUnit: TLabel;
	labAerodromeMagVar: TLabel;
	labAerodromeMagVarUnit: TLabel;
	labAerodromeISA: TLabel;
	labAerodromeISAUnit: TLabel;
	labCategory: TLabel;
	labRwyDirectionName: TLabel;
	labRwyDirectionTrueBrg: TLabel;
	labRwyDirectionUnit: TLabel;
	labRwyDirectionMagBrg: TLabel;
	labRwyDirectionMagBrgUnit: TLabel;
	labRwyDirectionElev: TLabel;
	labRwyDirectionElevUnit: TLabel;
	labRwyDirectionClearwayUnit: TLabel;
	labRwyDirectionClearway: TLabel;

	labAbeamDistance: TLabel;
	labAbeamDistanceUnit: TLabel;
	labAlongDistance: TLabel;
	labAlongDistanceUnit: TLabel;
	labCourse: TLabel;
    labCourseUnit: TLabel;
	labIFDistance: TLabel;
	labIFCourseUnit: TLabel;
	labIFCourse: TLabel;
	Label0201: TLabel;
	Label0202: TLabel;
	Label0203: TLabel;
	labMAPtCourse: TLabel;
    labMAPtCourseUnit: TLabel;
	labMAPtDistanceUnit: TLabel;
	labFAFDistanceUnit: TLabel;
	labIFDistanceUnit: TLabel;
	labFAFGradient: TLabel;
	labOCA: TLabel;
	labOCAUnit: TLabel;
	labFAFGradientUnit: TLabel;
    labFAFDistance: TLabel;
	labFAFAltitudeUnit: TLabel;
	labIFAltitude: TLabel;
	labIFAltitudeUnit: TLabel;
	labFirstAnchorSgfPoint: TLabel;
	labStraightSgfPointType: TLabel;
	labSecondAnchorSgfPoint: TLabel;
	labModelAreaRadius: TLabel;
	labModelAreaRadiusUnit: TLabel;
    labSOCFromMAPtDistance: TLabel;
	labSOCFromMAPtDistanceUnit: TLabel;
	labMAclimbGradient: TLabel;
	labMAclimbGradientUnit: TLabel;
    labRwyDirectionDisplacement: TLabel;
	Label2: TLabel;
	labRwyDirectionDisplacementUnit: TLabel;
	Label5: TLabel;
    labMAOCA: TLabel;
	Label11: TLabel;
	Label12: TLabel;
	Label13: TLabel;
	Label14: TLabel;
	Label15: TLabel;
	Label16: TLabel;
    labMAOCAUnit: TLabel;
	Label18: TLabel;
	Label19: TLabel;
	Label20: TLabel;
	labMinimalOCA: TLabel;
	labMinimalOCAUnit: TLabel;

	prmAbeam: TParameter;
	prmAlong: TParameter;
	prmCourse: TParameter;
    prmSOCFromMAPtDistance: TParameter;
	prmOCA: TParameter;
	prmMAClimb: TParameter;
	prmMAPtDistance: TParameter;
	prmMAPtCourse: TParameter;
	prmFAFDistance: TParameter;
	prmFAFGradient: TParameter;
	prmFAFAltitude: TParameter;
	prmIFDistance: TParameter;
	prmIFCourse: TParameter;
	prmIFIAS: TParameter;
	prmIFAltitude: TParameter;
    prmPreTurnAltitude: TParameter;
    labelFAOcaCaused: TLabel;
    cbGoToTurn: TCheckBox;
    prmMAIAS: TParameter;
	grbNextWPT: TGroupBox;
	labelTurnWPT: TLabel;
    cbTurnWPT: TComboBox;
    labelTurnWPTType: TLabel;
	rbExistingPoint: TRadioButton;
	rbCreateNew: TRadioButton;
    prmPreOCA: TParameter;
    prmTurnFromMAPtDist: TParameter;
    labNWPTFromTPDist: TLabel;
    labNWPTFromTPDistUnit: TLabel;
	editNWPTFromTPDist: TEdit;
	prmNWPTFromTPDist: TParameter;
    prmTurnCourse: TParameter;
	labTurnCourse: TLabel;
    editTurnCourse: TEdit;
    udTurnCourse: TUpDown;
    labTurnCourseUnit: TLabel;
	rgMAPtSensorType: TRadioGroup;
    labTurnFromMAPtDist: TLabel;
	editTurnFromMAPtDist: TEdit;
    labTurnFromMAPtDistUnit: TLabel;
	rgFlyMode: TRadioGroup;
    rgToFIX: TRadioGroup;
	prmMATBankAngle: TParameter;
    labMATBankAngle: TLabel;
	editMATBankAngle: TEdit;
    labMATBankAngleUnit: TLabel;
	GroupBox2: TGroupBox;
    prmTurn1: TParameter;
    prmTurn2: TParameter;
    prmTurn3: TParameter;
	grbCourses: TGroupBox;
    labelTurn1: TLabel;
    editTurn1: TEdit;
    labelTurn1Unit: TLabel;
    labelTurn2: TLabel;
    editTurn2: TEdit;
    labelTurn2Unit: TLabel;
	labelTurn3: TLabel;
    editTurn3: TEdit;
    labelTurn3Unit: TLabel;
	labFinalTurnAltitude: TLabel;
	labFinalTurnAltitudeUnit: TLabel;
    labFinalOCA: TLabel;
	labFinalOCAUnit: TLabel;
    editFinalTurnAltitude: TEdit;
    editFinalOCA: TEdit;
	labTurnOn: TLabel;
    cbTurnAt: TComboBox;
	labelTurnSide: TLabel;
    cbTurnSide: TComboBox;
    labSignificiantObstacle: TLabel;
    editSignificiantObstacle: TEdit;
	labMinimalOCH: TLabel;
    labMinimalOCHUnit: TLabel;
    editMinimalOCH: TEdit;
    SaveReportDialog: TSaveDialog;
    labOCH: TLabel;
    labOCHUnit: TLabel;
	editOCH: TEdit;
    labPlannedAngle: TLabel;
    editMaxTurnAngle: TEdit;
	labPlannedAngleUnit: TLabel;
    prmIFMaxAngle: TParameter;
    cbBranch: TComboBox;
    cbLeg: TComboBox;
    CheckBoxTest: TCheckBox;
    rgMAHFSensorType: TRadioGroup;
    labMAHFFromMAPtDist: TLabel;
	editMAHFFromMAPtDist: TEdit;
    labMAHFFromMAPtDistUnit: TLabel;
    prmMAHFFromMAPtDist: TParameter;
    labMAHFAltitude: TLabel;
    editMAHFAltitude: TEdit;
    labMAHFAltitudeUnit: TLabel;
    editPreTurnAltitude: TEdit;
	labPreTurnAltitude: TLabel;
	labPreTurnAltitudeUnit: TLabel;
    Button1: TButton;
    AddBranchFrame1: TAddBranchFrame;
	grbIntermediate: TGroupBox;
    rbIAPCH_GNSS: TRadioButton;
    rbIAPCH_DME_DME: TRadioButton;
	lblPBNTypeIF: TLabel;
	cbPBNTypeIF: TComboBox;
	editIFVelocity: TEdit;
	labIFVelocity: TLabel;
	labIFVelocityUnit: TLabel;
    cbMoreDMEIF: TCheckBox;
    cbMoreDMEMA: TCheckBox;
    cbPBNTypeMA: TComboBox;
	labMAPBNType: TLabel;
    labRWYName: TLabel;
    cbRWY: TComboBox;
    Label1: TLabel;
    Edit1: TEdit;
    Parameter1: TParameter;

	procedure FormClose(Sender: TObject; var Action: TCloseAction);
	procedure NextBtnClick(Sender: TObject);
	procedure PrevBtnClick(Sender: TObject);
	procedure pageContMainChange(Sender: TObject);
	procedure FormCreate(Sender: TObject);
	procedure cbAerodromesSelect(Sender: TObject);
	procedure bOKClick(Sender: TObject);
	procedure cbRWYSelect(Sender: TObject);
	procedure cbRwyDirectionSelect(Sender: TObject);

	procedure FormDestroy(Sender: TObject);
	procedure rgApproachTypeClick(Sender: TObject);
	procedure ListView1Change(Sender: TObject; Item: TListItem; Change: TItemChange);
	procedure rgAlignmentClick(Sender: TObject);
	procedure rgSelectFirstAncorPointClick(Sender: TObject);
	procedure rgChooseTrackClick(Sender: TObject);
	procedure cbSecondAnchorPointChange(Sender: TObject);
	procedure cbFirstAnchorPointChange(Sender: TObject);
	procedure ComboTrackIntervalsChange(Sender: TObject);
	procedure rbMAPtClick(Sender: TObject);
	procedure rbFAFClick(Sender: TObject);
	procedure rbIFClick(Sender: TObject);
	procedure editIFVelocityExit(Sender: TObject);
	procedure udIFCourseClick(Sender: TObject; Button: TUDBtnType);
	procedure btnAddBranchClick(Sender: TObject);
	procedure cbMAclimbGradientChange(Sender: TObject);
	procedure OnAbeamChangeValue(Sender: TCustomParameter);
	procedure OnAlongChangeValue(Sender: TCustomParameter);
    procedure OnCourseChangeValue(Sender: TCustomParameter);
    procedure OnFAFDistanceChangeValue(Sender: TCustomParameter);
    procedure OnIFDistanceChangeValue(Sender: TCustomParameter);
	procedure OnIFCourseChangeValue(Sender: TCustomParameter);
	procedure cbTurnAtChange(Sender: TObject);
	procedure AddBranchFrame1spBtnPlusClick(Sender: TObject);
	procedure AddBranchFrame1cbDirListChange(Sender: TObject);
	procedure AddBranchFrame1rBtnAFromListClick(Sender: TObject);
    procedure AddBranchFrame1rBtnAzimuthClick(Sender: TObject);
    procedure AddBranchFrame1rBtnDistanceClick(Sender: TObject);
    procedure AddBranchFrame1rBtnDFromListClick(Sender: TObject);
	procedure AddBranchFrame1cbDistListChange(Sender: TObject);
	procedure AddBranchFrame1cbFlyModeChange(Sender: TObject);
    procedure AddBranchFrame1prmCourseChangeValue(
	  Sender: TCustomParameter);
	procedure AddBranchFrame1prmDistanceChangeValue(
	  Sender: TCustomParameter);
	procedure AddBranchFrame1prmIASChangeValue(Sender: TCustomParameter);
	procedure AddBranchFrame1prmAltitudeChangeValue(
	  Sender: TCustomParameter);
	procedure AddBranchFrame1prmGradientChangeValue(
	  Sender: TCustomParameter);
	procedure prmMAPtDistanceChangeValue(Sender: TCustomParameter);
	procedure prmSOCFromMAPtDistanceChangeValue(Sender: TCustomParameter);
	procedure rbExistingPointClick(Sender: TObject);
	procedure cbTurnSideChange(Sender: TObject);
	procedure cbTurnWPTChange(Sender: TObject);
	procedure rgToFIXClick(Sender: TObject);
	procedure prmNWPTFromTPDistChangeValue(Sender: TCustomParameter);
	procedure prmTurnCourseChangeValue(Sender: TCustomParameter);
	procedure prmPreTurnAltitudeChangeValue(Sender: TCustomParameter);
	procedure prmTurnFromMAPtDistChangeValue(Sender: TCustomParameter);
	procedure prmMATBankAngleChangeValue(Sender: TCustomParameter);
	procedure rgFlyModeClick(Sender: TObject);
	procedure prmMAIASChangeValue(Sender: TCustomParameter);
	procedure cbIFListChange(Sender: TObject);
	procedure cbFAFListChange(Sender: TObject);
	procedure cbMAPtListChange(Sender: TObject);
	procedure prmIFIASChangeValue(Sender: TCustomParameter);
	procedure prmIFMaxAngleChangeValue(Sender: TCustomParameter);
	procedure prmIFAltitudeChangeValue(Sender: TCustomParameter);
	procedure cbBranchChange(Sender: TObject);
	procedure cbLegChange(Sender: TObject);
	procedure cbGoToTurnClick(Sender: TObject);
	procedure prmMAHFFromMAPtDistChangeValue(Sender: TCustomParameter);
	procedure bHelpClick(Sender: TObject);
	procedure Button1Click(Sender: TObject);
	function FormHelp(Command: Word; Data: Integer;
	  var CallHelp: Boolean): Boolean;
	procedure FormKeyUp(Sender: TObject; var Key: Word;
	  Shift: TShiftState);
    procedure rgIntermediatSensorTypeClick(Sender: TObject);
    procedure cbPBNTypeIFChange(Sender: TObject);
    procedure cbMoreDMEIFClick(Sender: TObject);
    procedure rgMAHFSensorTypeClick(Sender: TObject);
    procedure cbPBNTypeMAChange(Sender: TObject);
    procedure cbMoreDMEMAClick(Sender: TObject);
    procedure prmFAFAltitudeChangeValue(Sender: TCustomParameter);
    procedure prmFAFGradientChangeValue(Sender: TCustomParameter);
    procedure AddBranchFrame1spbtnInfoClick(Sender: TObject);
  private
	procedure DeleteAllGraphics;
	function InitialApproachLeg(NO_SEQ: Integer; Leg: Tleg): IApproachLeg;
	function IntermediateApproachLeg(NO_SEQ: Integer): IApproachLeg;
	function FinalApproachLeg(NO_SEQ: Integer; ptFAFDescent: TPoint; var Continued: Integer): IApproachLeg;

	function FinalApproachContinuedLeg(NO_SEQ, Continued: Integer): IApproachLeg;
	function MissedApproachStraight(NO_SEQ, Continued: Integer): IApproachLeg;
	function MissedApproachTermination(NO_SEQ, Continued: Integer): IApproachLeg;
//	function MissedApproachTerminationContinued(NO_SEQ: Integer): IApproachLeg;

	procedure SaveProcedure;

// Prev ============================================================
	procedure BackToInitialParameters;
	procedure BackToApproachCoursePage;
	procedure BackToFinalAndIntermediateApproach;

// Next ============================================================
	procedure ShowFinalApproachCourse;
	procedure ShowFinalAndIntermediateApproach;
	procedure ShowApproach_ArrivalSegments;
	procedure ShowStraightMissedApproach;
	procedure ShowTurnMissedApproach;
	procedure ShowMATurnPage;
//============================================================ Page 1

//============================================================ Page 2
	procedure SetAligned(Aligned: Boolean);
	procedure SetAlignedAnchorPoint(AnchorPoint: TPoint);
	procedure SetNotAlignedAnchorPoint(AnchorPoint: TPoint; Index: Integer);
	procedure SetTrackInterval(TrackInterval: TInterval);

//	procedure SetDirection(fTrackDir: Double);

	function CreateMAPt(fTrackDir: Double; AnchorPoint: TPoint; Aligned: Boolean): Boolean;
	procedure DrawCLine;

	procedure FillFirstAnchorPoints;
	function FillSecondPoints(TrackIntervalList: TIntervalList): Integer;
//============================================================ Page 3
	procedure SetMAPt(NewMAPt: TPoint);
	procedure ApplyIFDistanceAndDirection(Dist, Dir: Double);
	procedure CalcFAFALtitude;

//============================================================ Page 4
	procedure CreateStraightInSegment(Num: Integer);
	procedure CreateMATIASegment(Num: Integer; FIX: TFIX);

//============================================================ Page 6
	function CalcFAOCA(MinOCA: Double; var MaxIx, MaxL: Integer): Double;
	function GetMAObstAndMinOCA(ObstacleList: TList; FullPoly,
			PrimaryPoly: TPolygon; Gradient: Double; var MaxIx: Integer): Double;

	procedure CalcCourseInterval;
	function FillMATurnPoints: Integer;
	procedure TurnOnTP;
	procedure TurnOnMAPt;
	procedure ReCreateMATurnArea;
	procedure GetTurnObstacles;
	procedure GetStraightMAHFObstacles;
//============================================================ Data
  private
	FCurrPage:						Integer;
	FMSA:							Double;
	FRange:							Double;
	FSignificantCollection:			TPandaCollection;
//	FRWYList:						TRWYList;
	FRWYList:						TPandaList;
	FRWYDirectionList:				TRwyDirectionList;
//	FAerportList:					TAhpList;
//	FObstacleList:					TPandaCollection;

//============================================================ Page 1
//Input
	iChecked:						Integer;
	FApproachType:					Integer;
	FAirCraftCategory:				TAircraftCategory;

//Local
	FCirclingMinOCH,
	fRWYCLDir:						Double;

	FAerodromeList:					TPandaList;
	FAerodrome:						TAHP;
	FRWYDirection:					TRWYDirection;
//Out
//	FSensorType:					TSensorType;

//============================================================ Page 2
//Input
	FFarMAPtHandle,
	FTrackHandle,
	FCLHandle:						Integer;

	FAlignedSgfPoint:				TPandaCollection;
	FNotAlignedSgfPoint:			TPandaCollection;
	FNotAlignedSgfPointRange:		TList;

	FIABranchs:						TList;

	FFarMAPt,
	FAlignedAnchorPoint1,
	FAlignedAnchorPoint2:			TPoint;

	FNotAlignedAnchorPoint1,
	FNotAlignedAnchorPoint2:		TPoint;

	arMaxInterDist:					Double;
//Local
	FAligned:						Boolean;
//Aligned
	FAlignedExistingAnchorPoint:	Boolean;
//	FAbeamDistance:					Double;
	FAlignedApproachDir:			Double;

	FAlignedTrackInterval:			TInterval;
//Not Aligned
	FNotAlignedExistingAnchorPoint:	Boolean;
//	FAlongDistance:					Double;
	FNotAlignedApproachDir:			Double;

//	FComboTrackIntervalsVisible:	Boolean;
//	FNotAlignedTrackInterval:		TInterval;
	FNotAlignedTrackIntervals:		Array [0..1] of TInterval;
//===============
//============================================================ Page 3
//Input
//	FTHRToMAPtDistance,
//	FTHRToFAFDistance:				Double;

	F3ApproachDir:					Double;

	FMAPt:							TMAPt;
	FMAPtInfo:						TFIXInfo;

	FMATF:							TMATF;
	FMATFInfo:						TFIXInfo;

	FMAHF:							TFIX;
//	FMAHFInfo:						TFIXInfo;

//=== FAF
	FFAF:							TFIX;
	FFAFInfo:						TFIXInfo;

//=== IF
	FIF:							TFIX;
	FIFInfo:						TFIXInfo;

	FIFCourseSgfPoint:				TPandaCollection;
	FIFDistanceSgfPoint:			TPandaCollection;
//Local
	FObstalces:						TObstacleList;

	FullPolyList,
	PrimaryPolyList:				TList;
	FObstacleListList:				TList;
//============================================================ Page 4
//	FBranchPoints:	TList;
//============================================================ Page 5
	bTurnExistingPoint:				Boolean;
//	FMATurnPoint:					TPoint;

	FMAFullPolyGr,
	FMAPrimPolyGr:					Integer;

//SOC
	FMAMinOCA:						Double;
	FFAAligOCA,
	FFAMAPtPosOCA,
//	FFAObstOCA,

	FFAOCA:							Double;
	FIMALenght,
	FMAHFLenght:					Double;

	FSOC:							TFIX;
//	FptSOC:							TPoint;
	FSOCInfo:						TFIXInfo;

	FMissedApproach:				TList;

	SMASFullPolyList,
	SMASPrimaryPolyList:			TList;
	SMASFObstacleListList:			TList;

	SMATFullPolyList,
	SMATPrimaryPolyList:			TList;
//	SMATFObstacleListList:			TList;

	MATAObstacleList:				TList;
	TIAObstacleList:				TList;
//============================================================ Page 5
	kkLineElem,
	primePolyEl,
	secondPolyEl:					integer;

	primeTAPoly,
	secondTAPoly:					TPolygon;
	FMAPtLeg:						TLeg;
	FMATFLeg:						TLeg;
//============================================================ Page ??
	FMATIAOCA,
	FMATAOCA, FMAFinalOCA:			Double;

	FMATIAOCA_CO:					String;
//	FMATAOCA_CO:					String;
	FAppCaption: 					String;
  end;

//==============================================================================

var
	MainForm:		TMainForm;

implementation

uses
	Math, ObjectDirectoryContract, ARANMath, ARANFunctions, ARANGlobals,
	ApproachGlobals, ApproachService, UnitConverter, Functions, ReportFormUnit,
	ReportUnit, contract, SettingsContract;	//FIXParameterUnit,

const
	GrTable: Array	[0..8] of double =
					(0.02, 0.025, 0.03, 0.035, 0.04,
					 0.045, 0.05, 0.055, 0.06);
{$R *.dfm}

// ================================================================= Form Events
procedure TMainForm.FormCreate(Sender: TObject);
var
	i:					Integer;
	Range:				TInterval;
	Tan_5:				Double;
	icaoPrefixArray:	TStringArray;
	appFileName: String;
	startupPath: String;
begin
	FAppCaption := Caption;

	appFileName := GetModuleName (HInstance);
	startupPath := ExtractFilePath (appFileName);
	//Application.HelpFile := startupPath + 'RNAV_Manual.chm';
	Application.HelpFile := 'RNAV.chm';

	MATAObstacleList := nil;
	TIAObstacleList := nil;

	kkLineElem := -1;
	primePolyEl := -1;
	secondPolyEl := -1;

	FFarMAPtHandle := -1;
	FTrackHandle := -1;
	FCLHandle := -1;
	FMAFullPolyGr := -1;
	FMAPrimPolyGr := -1;
	FRange := 0;
	FMSA := 300;

	bTurnExistingPoint := True;
	AddBranchFrame1.Initialize;
	FMAPtLeg := nil;
	FMATFLeg := nil;

//	FSignificantCollection := nil;
//	FRWYList := nil;
//	FRWYDirectionList := nil;
//	FAerportList := nil;

// =======================================================================
	labAerodromeElevUnit.Caption := CConverters[puAltitude].UnitName;
	labOCHUnit.Caption := CConverters[puAltitude].UnitName;
	labMAOCAUnit.Caption := CConverters[puAltitude].UnitName;

	labRwyDirectionElevUnit.Caption := CConverters[puAltitude].UnitName;
	labRwyDirectionClearwayUnit.Caption := CConverters[puAltitude].UnitName;
	labRwyDirectionDisplacementUnit.Caption := CConverters[puAltitude].UnitName;

	labMinimalOCAUnit.Caption := CConverters[puAltitude].UnitName;
	labMinimalOCHUnit.Caption := CConverters[puAltitude].UnitName;

	labFinalTurnAltitudeUnit.Caption := CConverters[puAltitude].UnitName;
	labFinalOCAUnit.Caption := CConverters[puAltitude].UnitName;
	labMAHFAltitudeUnit.Caption := CConverters[puAltitude].UnitName;

	Label18.Caption := CConverters[puDistance].UnitName;
	Label19.Caption := CConverters[puDistance].UnitName;
	Label20.Caption := CConverters[puAltitude].UnitName;


//Page 1 =======================================================================

//Page 2 =======================================================================
	FAligned						:= True;
	FAlignedExistingAnchorPoint		:= False;
	FNotAlignedExistingAnchorPoint	:= False;

	FAlignedAnchorPoint1			:= TPoint.Create;
	FAlignedAnchorPoint2			:= TPoint.Create;

	FNotAlignedAnchorPoint1			:= TPoint.Create;
	FNotAlignedAnchorPoint2			:= TPoint.Create;
//=======================
	Range.Left						:= -GPANSOPSConstants.Constant[arMinInterToler].Value;
	Range.Right						:= GPANSOPSConstants.Constant[arMinInterToler].Value;

	prmAbeam.BeginUpdate;
	prmAbeam.Range					:= Range;
//=======================
	Tan_5							:= Tan(GPANSOPSConstants.Constant[arStrInAlignment].Value);
	arMaxInterDist					:= GPANSOPSConstants.Constant[arMinInterDist].Value +
											GPANSOPSConstants.Constant[arMinInterToler].Value/Tan_5;

	Range.Left						:= GPANSOPSConstants.Constant[arMinInterDist].Value;
	Range.Right						:= arMaxInterDist;

	prmAlong.BeginUpdate(False);
	prmAlong.Range					:= Range;
	prmAlong.EndUpdate;
//Page 3 =======================================================================
	FullPolyList					:= TList.Create;
	PrimaryPolyList					:= TList.Create;
	FObstacleListList				:= TList.Create;
//FAF ==========================================================================
	FFAF							:= TFIX.Create(FAF_, GUI);
	FFAF.FlyMode					:= fmFlyBy;
	FFAF.SensorType					:= stGNSS;
	FFAF.PBNType					:= ptRNPAPCH;

	FFAFInfo						:= TFIXInfo.Create(FFAF);

	prmFAFDistance.BeginUpdate;
	prmFAFAltitude.BeginUpdate;
	prmFAFGradient.BeginUpdate;

	FFAFInfo.DistanceParameter		:= prmFAFDistance;
	FFAFInfo.AltitudeParameter		:= prmFAFAltitude;
	FFAFInfo.GradientParameter		:= prmFAFGradient;

//IF ===========================================================================
	FIF := TFIX.Create(IF_, GUI);
	FIF.FlyMode := fmFlyBy;
	FIF.SensorType					:= stGNSS;
	FIF.PBNType						:= ptRNPAPCH;

	FIFInfo := TFIXInfo.Create(FIF);

	FIFCourseSgfPoint := TPandaCollection.Create;
	FIFDistanceSgfPoint := TPandaCollection.Create;

//=========================

	Range.Left := GPANSOPSConstants.Constant[arImRange_Min].Value;
	Range.Right := GPANSOPSConstants.Constant[arImRange_Max].Value;
	prmIFDistance.BeginUpdate;
	prmIFDistance.Range := Range;

//=========================
	prmIFCourse.BeginUpdate;
//=========================
	prmIFAltitude.BeginUpdate;
//=========================
	prmIFIAS.BeginUpdate;
//==========================================================================
{	prmIFGradient.BeginUpdate;
	Range.Left := 0;
	Range.Right := GPANSOPSConstants.Constant[arImDescent_Max].Value;
	prmIFGradient.Range := Range;
}
//==========================================================================
	FIFInfo.DistanceParameter := prmIFDistance;
	FIFInfo.AltitudeParameter := prmIFAltitude;
	FIFInfo.AngleParameter := prmIFCourse;
	FIFInfo.SpeedParameter := prmIFIAS;
	FIFInfo.GradientParameter := prmFAFGradient;
//	FIFInfo.GradientParameter := prmIFGradient;

// Page 4 ======================================================================
	SMASFullPolyList		:= TList.Create;
	SMASPrimaryPolyList		:= TList.Create;
	SMASFObstacleListList	:= TList.Create;

	SMATFullPolyList		:= TList.Create;
	SMATPrimaryPolyList		:= TList.Create;
//	SMATFObstacleListList	:= TList.Create;

	FSOC					:= TFIX.Create(MAPt_, GUI);
	FSOC.FlyMode			:= fmFlyBy;
	FSOCInfo				:= TFIXInfo.Create(FSOC);

	prmSOCFromMAPtDistance.BeginUpdate;
	prmOCA.BeginUpdate;
	prmMAClimb.BeginUpdate;
//==============================================================================
	pageContMain.Top := -21;
	pageContMain.ActivePageIndex := 0;
	pageContMain.OnChange (pageContMain);

	FAlignedSgfPoint			:= TPandaCollection.Create;
	FNotAlignedSgfPoint			:= TPandaCollection.Create;
	FNotAlignedSgfPointRange	:= TList.Create;
	FIABranchs					:= TList.Create;
	FMissedApproach				:= TList.Create;
//	FptSOC						:= TPoint.Create;

// Page 5 ======================================================================
//MAPt =========================================================================
	FMAPt						:= TMAPt.Create(MAPt_, GUI);
	FMAPt.FlyMode				:= fmFlyOver;
	FMAPt.SensorType			:= stGNSS;
	FMAPt.PBNType				:= ptRNPAPCH;

	FMAPtInfo					:= TFIXInfo.Create(FMAPt);

	FMAPtInfo.DistanceParameter	:= prmMAPtDistance;

//	FMAPtCourseParam := TParameter.Create(cpAngle);

	FMATF						:= TMATF.Create(GUI);
	FMATF.SensorType			:= stGNSS;
	FMATF.PBNType				:= ptRNPAPCH;

	FMATF.MAPt					:= FMAPt;
	FMATFInfo					:= TFIXInfo.Create;
	FMATFInfo.AltitudeParameter	:= prmPreTurnAltitude;
//	FMATFInfo.DistanceParameter := prmTPFromMAPtMAPtDist;
	FMATFInfo.SpeedParameter	:= prmMAIAS;
//	FMATFInfo.AngleParameter	:= prmTurnCourse;

	FMAHF						:= TFIX.Create(MAHF_LE_56, GUI);
	FMAHF.SensorType			:= stGNSS;
	FMAHF.PBNType				:= ptRNPAPCH;

//	FMAHFInfo					:= TFIXInfo.Create(FMAHF);

{
//	FMAHFInfo.DistanceParameter	:= prmIFDistance;
//	FMAHFInfo.AngleParameter	:= prmIFCourse;
//	FMAHFInfo.SpeedParameter	:= prmIFIAS;
//	FMAHFInfo.GradientParameter	:= prmFAFGradient;
}
///////////////////////////////////////////////////////////////////////
	cbAerodromes.Clear;

	if not GObjectDirectory.isValid then exit;
	//SetLength (icaoPrefixArray, 1);
 //	icaoPrefixArray [0] := '-1';
	//icaoPrefixArray [1] := 'SW';
 //	icaoPrefixArray [2] := 'SB';

{	SetLength(icaoPrefixArray, 6);
	icaoPrefixArray[0] := 'SB';
	icaoPrefixArray[1] := 'KC';
	icaoPrefixArray[2] := 'KB';
	icaoPrefixArray[3] := 'SN';
	icaoPrefixArray[4] := 'SS';
	icaoPrefixArray[5] := 'SW';
}
	FAerodromeList := GObjectDirectory.GetAerodromeList(icaoPrefixArray);

//	FAerportList := GObjectDirectory.AerodromeList [GOrgID];
	for i := 0 to FAerodromeList.Count-1 do
	begin
{$IFNDEF NOLIC}
//		if IsPointInPoly(FAerportList.Item[i].Prj, GLicenseRect) then
{$ENDIF}
		//cbAerodromes.AddItem (TUnicalName(FAerodromeList.Item[i]).Name, FAerodromeList.Item[i]);
		cbAerodromes.AddItem (
			TUnicalName(FAerodromeList.Item[i]).Name,	// + ' (' + TUnicalName(FAerodromeList.Item[i]).Name +')',
			FAerodromeList.Item[i]);
	end;

	if cbAerodromes.Items.Count > 0 then
	begin
		cbAerodromes.ItemIndex := 0;
		cbAerodromesSelect (cbAerodromes);
	end;

{
	if cbAerodromes.Items.Count > 2 then
	begin
		cbAerodromes.ItemIndex := 2;
		cbAerodromesSelect (cbAerodromes);
	end
	else if cbAerodromes.Items.Count > 0 then
	begin
		cbAerodromes.ItemIndex := cbAerodromes.Items.Count-1;
		cbAerodromesSelect (cbAerodromes);
	end;
}
//	FIXParameterForm := TFIXParameterForm.Create(self);
//	AddBranchForm := TAddBranchForm.Create(self);
end;

procedure TMainForm.FormClose(Sender: TObject; var Action: TCloseAction);
begin
	Action := caFree;
	if AddBranchFrame1.State = fsRunned then
		AddBranchFrame1.Hide;
end;

procedure TMainForm.FormDestroy(Sender: TObject);
var
	I, J:				Integer;
	Leg:				TLeg;
	LegList,
	ObstalceInfoList:	TList;
begin
	try
		FMAPtLeg.Free;
		FMATFLeg.Free;

//		FSignificantCollection.Free;
//		FRWYList.Free;
//		FRWYDirectionList.Free;
//		FAerportList.Free;

		if Assigned(MATAObstacleList) then		MATAObstacleList.Free;
		if Assigned(TIAObstacleList) then		TIAObstacleList.Free;

		AddBranchFrame1.Finalize;

		DeleteAllGraphics;

		for I := 0 to FIABranchs.Count - 1 do
		begin
			LegList := TList(FIABranchs.Items[I]);
			for J := 0 to LegList.Count - 1 do
			begin
				Leg := TLeg(LegList.Items[J]);
				Leg.StartFIX.Free;
				if J = LegList.Count - 1 then
					Leg.EndFIX.Free;

				Leg.Free;
			end;
			LegList.Free;
		end;
		FIABranchs.Free;

		for I := 0 to FMissedApproach.Count - 1 do
		begin
			Leg := TLeg(FMissedApproach.Items[I]);
			Leg.Free;
		end;
		FMissedApproach.Free;

//		FIXParameterForm.Free;
//		AddBranchForm.Free;

//Page 2 ======================================
		FAlignedSgfPoint.Free;
		FNotAlignedSgfPoint.Free;

		for I := 0 to FNotAlignedSgfPointRange.Count - 1 do
			TIntervalList(FNotAlignedSgfPointRange.Items[I]).Free;
		FNotAlignedSgfPointRange.Free;

		FAlignedAnchorPoint1.Free;
		FAlignedAnchorPoint2.Free;
		FNotAlignedAnchorPoint1.Free;
		FNotAlignedAnchorPoint2.Free;
//Page 3 ======================================

		for I := 0 to FullPolyList.Count - 1 do
			TPolygon(FullPolyList.Items[I]).Free;
		FullPolyList.Free;

		for I := 0 to PrimaryPolyList.Count - 1 do
			TPolygon(PrimaryPolyList.Items[I]).Free;
		PrimaryPolyList.Free;

		for I := 0 to FObstacleListList.Count - 1 do
		begin
			ObstalceInfoList := TList(FObstacleListList.Items[I]);
			for J := 0 to ObstalceInfoList.Count - 1 do
				TObstacleInfo(ObstalceInfoList.Items[J]).Free;
			ObstalceInfoList.Free;
		end;
		FObstacleListList.Free;

		FMAHF.Free;
		FMATF.Free;
		FMAPt.Free;
		FFAF.Free;
		FIF.Free;
		FIFCourseSgfPoint.Free;
		FIFDistanceSgfPoint.Free;
// = IF

//		FMAHFInfo.Free;
		FMATFInfo.Free;
		FMAPtInfo.Free;
		FFAFInfo.Free;
		FIFInfo.Free;

//		FFAFTreshold.Free;
//Page 4 ======================================
		for I := 0 to SMASFullPolyList.Count - 1 do
			TPolygon(SMASFullPolyList.Items[I]).Free;
		SMASFullPolyList.Free;

		for I := 0 to SMASPrimaryPolyList.Count - 1 do
			TPolygon(SMASPrimaryPolyList.Items[I]).Free;
		SMASPrimaryPolyList.Free;

		for I := 0 to SMASFObstacleListList.Count - 1 do
		begin
			ObstalceInfoList := TList(SMASFObstacleListList.Items[I]);
			for J := 0 to ObstalceInfoList.Count - 1 do
				TObstacleInfo(ObstalceInfoList.Items[J]).Free;
			ObstalceInfoList.Free;
		end;

		SMASFObstacleListList.Free;

		for I := 0 to SMATFullPolyList.Count - 1 do
			TPolygon(SMATFullPolyList.Items[I]).Free;
		SMATFullPolyList.Free;

		for I := 0 to SMATPrimaryPolyList.Count - 1 do
			TPolygon(SMATPrimaryPolyList.Items[I]).Free;
		SMATPrimaryPolyList.Free;

{		for I := 0 to SMATFObstacleListList.Count - 1 do
		begin
			ObstalceInfoList := TList(SMATFObstacleListList.Items[I]);
			for J := 0 to ObstalceInfoList.Count - 1 do
				TObstacleInfo(ObstalceInfoList.Items[J]).Free;
			ObstalceInfoList.Free;
		end;

		SMATFObstacleListList.Free;
}
		FSOC.Free;
		FSOCInfo.Free;
//		FptSOC.Free;
//=============================================
	finally
		OnFormClose(0);
	end
end;

function formLat(Val: Double): string;
var
	deg, min:	Integer;
	sec, fTmp:	Double;
	Fix, sTmp:	string;
begin
	if Val < 0 then	Fix := 'S'
	else			Fix := 'N';

	Val := Abs(Val);
	deg := Trunc(Val);
	fTmp := (Val-deg)*60.0;
	min := Trunc(fTmp);
	sec := (fTmp - min)*60.0;

	if sec >= 59.995 then
	begin
		sec := 0.0;
		inc(min);
	end;

	if min >59 then
	begin
		min := 0;
		inc(deg);
	end;

	sTmp := IntToStr(deg);
	if Length(sTmp) < 2 then
		sTmp := '0' + sTmp;
	result := sTmp + '° ';

	sTmp := IntToStr(min);
	if Length(sTmp) < 2 then
		sTmp := '0' + sTmp;

	result := result + sTmp + ''' ';

	sTmp := FormatFloat('00.00', sec);
	if Length(sTmp) < 4 then
		sTmp := '0' + sTmp;

	result := result + sTmp + '" ' + Fix;
end;

function formLong(Val: Double): string;
var
	deg, min:	Integer;
	sec, fTmp:	Double;
	Fix, sTmp:	string;
begin
	if Val < 0 then	Fix := 'W'
	else			Fix := 'E';

	Val := Abs(Val);
	deg := Trunc(Val);
	fTmp := (Val-deg)*60.0;
	min := Trunc(fTmp);
	sec := (fTmp - min)*60.0;

	if sec >= 59.995 then
	begin
		sec := 0.0;
		inc(min);
	end;

	if min >59 then
	begin
		min := 0;
		inc(deg);
	end;

	sTmp := IntToStr(deg);
	while Length(sTmp) < 3 do
		sTmp := '0' + sTmp;

	result := sTmp + '° ';

	sTmp := IntToStr(min);
	if Length(sTmp) < 2 then
		sTmp := '0' + sTmp;

	result := result + sTmp + ''' ';

	sTmp := FormatFloat('00.00', sec);
	if Length(sTmp) < 4 then
		sTmp := '0' + sTmp;

	result := result + sTmp + '" ' + Fix;
end;

function CreateLegTable(Leg: TLeg; TabComment: string = ''; StoreHeight: Boolean = True): string;
const
	CrLf = #13#10;
var
	sTmp:	String;
	fDist,
	fDir,
	fTmp:	Double;
begin
	if TabComment <> '' then
		sTmp := '<b>' + TabComment + '</b>'+CrLf+'<br>' + CrLf;

	sTmp :=  sTmp + '<table border=''1'' cellspacing=''0'' cellpadding=''1''>' + CrLf;

	fDist := Hypot(Leg.EndFIX.PrjPt.Y - Leg.StartFIX.PrjPt.Y, Leg.EndFIX.PrjPt.X - Leg.StartFIX.PrjPt.X);
	fTmp := CConverters[puDistance].ConvertFunction (fDist, cdToOuter, nil);
	sTmp :=  sTmp + '<tr><td><b>Length</b></td>' + '<td><b>' + RoundToStr(fTmp, GDistanceAccuracy) +'</b></td>'+'<td><b>' + CConverters[puDistance].UnitName + '</b></td></tr>' + CrLf;

	fDir := ArcTan2(Leg.EndFIX.PrjPt.Y - Leg.StartFIX.PrjPt.Y, Leg.EndFIX.PrjPt.X - Leg.StartFIX.PrjPt.X);
	fTmp := CConverters[puAngle].ConvertFunction (fDir, cdToOuter, Leg.StartFIX.PrjPt);
	sTmp :=  sTmp + '<tr><td><b>True bearing</b></td>' + '<td><b>' + RoundToStr(fTmp, GAngleAccuracy) +'</b></td>'+'<td><b>' + CConverters[puAngle].UnitName + '</b></td></tr>' + CrLf;

	sTmp :=  sTmp + '<tr><td><b>Start Point:</b></td></tr>' + CrLf;
	sTmp :=  sTmp + '<tr><td><b>Name</b></td>' + '<td><b>' + Leg.StartFIX.Name +'</b></td> </tr>' + CrLf;

	fTmp := CConverters[puAltitude].ConvertFunction (Leg.StartFIX.Altitude, cdToOuter, nil);
	sTmp :=  sTmp + '<tr><td><b>Altitude</b></td>' + '<td><b>' + RoundToStr(fTmp, GAltitudeAccuracy) + '</b></td>' + '<td><b>' + CConverters[puAltitude].UnitName + '</b></td></tr>' + CrLf;

	sTmp :=  sTmp + '<tr><td><b>Latitude</b></td>' + '<td><b>' + formLat(Leg.StartFIX.GeoPt.Y) +'</b></td> </tr>' + CrLf;
	sTmp :=  sTmp + '<tr><td><b>Longitude</b></td>' + '<td><b>' + formLong(Leg.StartFIX.GeoPt.X) +'</b></td> </tr>' + CrLf;

	sTmp :=  sTmp + '<tr><td><b>End Point:</b></td></tr>' + CrLf;

	sTmp :=  sTmp + '<tr><td><b>Name</b></td>' + '<td><b>' + Leg.EndFIX.Name +'</b></td> </tr>' + CrLf;

	if StoreHeight then
	begin
		fTmp := CConverters[puAltitude].ConvertFunction (Leg.EndFIX.Altitude, cdToOuter, nil);
		sTmp :=  sTmp + '<tr><td><b>Altitude</b></td>' + '<td><b>' + RoundToStr(fTmp, GAltitudeAccuracy) + '</b></td>' + '<td><b>' + CConverters[puAltitude].UnitName + '</b></td></tr>' + CrLf;
	end;

	sTmp :=  sTmp + '<tr><td><b>Latitude</b></td>' + '<td><b>' + formLat(Leg.EndFIX.GeoPt.Y) +'</b></td> </tr>' + CrLf;
	sTmp :=  sTmp + '<tr><td><b>Longitude</b></td>' + '<td><b>' + formLong(Leg.EndFIX.GeoPt.X) +'</b></td> </tr>' + CrLf;

	sTmp :=  sTmp + '</table>' + CrLf;
	sTmp :=  sTmp + '<br><br>' + CrLf;
	result := sTmp;
end;

procedure TMainForm.bOKClick(Sender: TObject);
var
	ReportFile:			TReportFile;
	ObstalceInfoList,
	LegList:			TList;
	Leg:				TLeg;
	I, J:				Integer;

	ObstalceInfo:		TObstacleInfo;
	InitialApproachList,
	IntermediateApproachList,
	FinalApproachList,
	InitialMissedApproachList:	TList;
begin
	if cbGoToTurn.Checked then
		FMAPt.IAS := prmMAIAS.Value
	else
		FMAPt.IAS :=  prmMAIAS.MaxValue;

	if((pageContMain.ActivePageIndex = 5) or((pageContMain.ActivePageIndex = 4)and(not cbGoToTurn.Checked)))and
	 SaveReportDialog.Execute then
	begin
		ReportFile := TReportFile.Create(SaveReportDialog.FileName, 'RNAV Approach - Calculation report');
//==============================================================================
		ReportFile.WriteMessage;
		ReportFile.WriteMessage('RNAV Approach - Obstacle report');	//RepFileTitle

		ReportFile.WriteText ('<P><b><big>Current user name:</big></b> ' + GCurrentUserName + '</P>');

		ReportFile.WriteParam('Date', DateToStr(Date) + ' - ' + TimeToStr(Time));

		ReportFile.WriteMessage;
		ReportFile.WriteMessage;

		for I := 0 to FIABranchs.Count - 1 do
		begin
			ReportFile.WriteMessage('Branch ' + IntToStr(I + 1) + ' legs');
			ReportFile.WriteMessage;
			LegList := TList(FIABranchs.Items[I]);

			for J := 0 to LegList.Count - 1 do
			begin
				Leg := TLeg(LegList.Items[J]);
				if Leg.EndFIX.Name = 'MAPt' then
					Leg.EndFIX.Altitude := FMAPt.Altitude;
				ReportFile.WriteMessage(CreateLegTable(Leg, 'Leg ' + IntToStr(J)));
			end;
			ReportFile.WriteMessage;
		end;

		ReportFile.WriteMessage;
		if Assigned(FMAPtLeg) then
			ReportFile.WriteMessage(CreateLegTable(FMAPtLeg, 'Straight missed approach'));
		if Assigned(FMATFLeg) then
			ReportFile.WriteMessage(CreateLegTable(FMATFLeg, 'Missed approach turn area', False));

		InitialApproachList := TList.Create;
		IntermediateApproachList := TList.Create;
		FinalApproachList := TList.Create;
		InitialMissedApproachList := TList.Create;

		for I := 0 to FObstacleListList.Count - 1 do
		begin
			ReportFile.WriteMessage('Branch ' + IntToStr(I + 1));
			ReportFile.WriteMessage;
			ObstalceInfoList := TList(FObstacleListList.Items[I]);
			LegList := TList(FIABranchs.Items[I]);

			InitialApproachList.Clear;
			IntermediateApproachList.Clear;
			FinalApproachList.Clear;
			InitialMissedApproachList.Clear;
			for J := 0 to ObstalceInfoList.Count - 1 do
			begin
//try
				ObstalceInfo := TObstacleInfo(ObstalceInfoList.Items[J]);
				case TLeg(LegList.Items[ObstalceInfo.Leg]).FlightProcedure of
				fpInitialApproach:
					InitialApproachList.Add(ObstalceInfo);
				fpIntermediateApproach:
					IntermediateApproachList.Add(ObstalceInfo);
				fpFinalApproach:
					FinalApproachList.Add(ObstalceInfo);
				fpInitialMissedApproach:
					InitialMissedApproachList.Add(ObstalceInfo);
				end;
//except
//	MessageDlg('Index = ' + IntToStr(J), mtInformation, [mbYes], 0);
//end;
			end;

			ReportFile.WriteMessage('Initial Approach ' + IntToStr(I + 1));
			ReportFile.WriteObstData(InitialApproachList);
			ReportFile.WriteMessage;

			ReportFile.WriteMessage('Intermediate Approach ' + IntToStr(I + 1));
			ReportFile.WriteObstData(IntermediateApproachList);
			ReportFile.WriteMessage;

			ReportFile.WriteMessage('Final Approach ' + IntToStr(I + 1));
			ReportFile.WriteObstData(FinalApproachList);
			ReportFile.WriteMessage;

			if InitialMissedApproachList.Count>0 then
			begin
				ReportFile.WriteMessage('Initial Missed Approach ' + IntToStr(I + 1));
				ReportFile.WriteObstData(InitialMissedApproachList);
				ReportFile.WriteMessage;
			end;
		end;

		InitialApproachList.Free;
		IntermediateApproachList.Free;
		FinalApproachList.Free;
		InitialMissedApproachList.Free;

		for I := 0 to SMASFObstacleListList.Count - 1 do
		begin
			if SMASFObstacleListList.Count > 1 then
				ReportFile.WriteMessage('Straight misssed approach branch ' + IntToStr(I + 1))
			else
				ReportFile.WriteMessage('Straight misssed approach');

			ObstalceInfoList := TList(SMASFObstacleListList.Items[I]);
			ReportFile.WriteStraightMissedApproachObstData(ObstalceInfoList);
		end;

		ReportFile.WriteTab(ReportForm.ListView5, 'Missed Approach - TIA');
		//ReportFile.WriteMessage('Missed Approach - TIA');
		//ReportFile.WriteMATIAreaObstData(TIAObstacleList);

		ReportFile.WriteTab(ReportForm.ListView6, 'Missed Approach - Turn Area');
		//ReportFile.WriteMessage('Missed Approach - Turn Area');
		//ReportFile.WriteMATurnAreaObstData(MATAObstacleList);

		ReportFile.Free;

		SaveProcedure;
	end;

	Close;
end;

procedure TMainForm.PrevBtnClick(Sender: TObject);
begin
	exit;
	
	if pageContMain.ActivePageIndex > 0 Then
		FCurrPage := pageContMain.ActivePageIndex - 1
	else
		FCurrPage := 0;

	HelpContext := 3100 + 100*FCurrPage;

	case FCurrPage of
	0:	begin
			BackToInitialParameters;
		end;
	1:	begin
			BackToApproachCoursePage;
		end;
	2:	begin
			NextBtn.Enabled := True;
			BackToFinalAndIntermediateApproach;
		end;
	3:	begin

		end;
	4:	begin

		end;
	5:	begin

		end;
	end;

	pageContMain.ActivePageIndex := FCurrPage;
	pageContMain.OnChange (pageContMain);
end;

procedure TMainForm.NextBtnClick(Sender: TObject);
begin
	if pageContMain.ActivePageIndex < pageContMain.PageCount - 1 then
		FCurrPage := pageContMain.ActivePageIndex + 1
	else
		FCurrPage := pageContMain.PageCount - 1;

	HelpContext := 3100 + 100*FCurrPage;

	case FCurrPage of
	1:	begin
  			ShowFinalApproachCourse;
		end;
	2:	begin
			ShowFinalAndIntermediateApproach;
		end;
	3:	begin
			ShowApproach_ArrivalSegments;
		end;
	4:	begin
			ShowStraightMissedApproach;
		end;
	5:	begin
			ShowTurnMissedApproach;
		end;
	6:	begin
			ShowMATurnPage;
		end;
	end;

	pageContMain.ActivePageIndex := FCurrPage;
	pageContMain.OnChange (pageContMain);
	bOK.Enabled := (pageContMain.ActivePageIndex = 5)or((pageContMain.ActivePageIndex = 4)and(not cbGoToTurn.Checked))
end;

// END ============================================================= Form Events
// ================================================================= Common procedures

procedure TMainForm.DeleteAllGraphics;
begin
	GUI.SafeDeleteGraphic(kkLineElem);
	GUI.SafeDeleteGraphic(primePolyEl);
	GUI.SafeDeleteGraphic(secondPolyEl);
	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);

	GUI.SafeDeleteGraphic(FFarMAPtHandle);
	GUI.SafeDeleteGraphic(FCLHandle);
	GUI.SafeDeleteGraphic(FTrackHandle);
end;

procedure TMainForm.pageContMainChange(Sender: TObject);
begin
	FCurrPage := pageContMain.ActivePageIndex;
	PrevBtn.Enabled := false;//FCurrPage > 0;
	if FCurrPage > pageContMain.PageCount - 3 then
		NextBtn.Enabled := FCurrPage < pageContMain.PageCount - 1;

	Caption := FAppCaption + ' - ' + pageContMain.ActivePage.Caption;
end;

procedure TMainForm.BackToInitialParameters;
begin
	GUI.SafeDeleteGraphic(FFarMAPtHandle);
	GUI.SafeDeleteGraphic(FCLHandle);
	GUI.SafeDeleteGraphic(FTrackHandle);
//	prmCourse.Active := False;
	prmCourse.ChangeValue(prmCourse.MaxValue, False);
//	prmCourse.Active := True;
end;

procedure TMainForm.BackToApproachCoursePage;
begin
	FMAPt.DeleteGraphics;
	FFAF.DeleteGraphics;
	FIF.DeleteGraphics;
end;

procedure TMainForm.BackToFinalAndIntermediateApproach;
var
	I, J:				Integer;
	ObstalceInfoList:	TList;
	BranchLegs:			TList;
	Leg:				TLeg;
begin
	AddBranchFrame1.Cancel;
	if Assigned(ReportForm) then
		ReportForm.ClearAllBranchs;

	for I := 0 to FullPolyList.Count - 1 do
		TPolygon(FullPolyList.Items[I]).Free;
	FullPolyList.Clear;

	for I := 0 to PrimaryPolyList.Count - 1 do
		TPolygon(PrimaryPolyList.Items[I]).Free;
	PrimaryPolyList.Clear;


	for I := 0 to FIABranchs.Count - 1 do
	begin
		BranchLegs := FIABranchs.Items[I];
		for J := 0 to BranchLegs.Count - 1 do
		begin
			Leg := TLeg(BranchLegs.Items[J]);
			Leg.StartFIX.Free;
			if J = BranchLegs.Count - 1 then
				Leg.EndFIX.Free;
			Leg.Free;
		end;
		BranchLegs.Free;
	end;
	FIABranchs.Clear;

	for I := 0 to FObstacleListList.Count - 1 do
	begin
		ObstalceInfoList := TList(FObstacleListList.Items[I]);

		for J := 0 to ObstalceInfoList.Count - 1 do
			TObstacleInfo(ObstalceInfoList.Items[J]).Free;
		ObstalceInfoList.Free;
	end;
	FObstacleListList.Clear;

end;

procedure TMainForm.ShowFinalApproachCourse;
{
var
	I:			Integer;
}
begin
	FApproachType := 0;//rgApproachType.ItemIndex;
	FAirCraftCategory := TAirCraftCategory(cbCategory.ItemIndex);

	if cbAerodromes.ItemIndex < 0 then
	begin
		//There is not selected Aerodrome
		exit;
	end;
//	FAerodrome := TAHP(cbAerodromes.Items.Objects[cbAerodromes.ItemIndex]);

	if cbRwyDirection.ItemIndex < 0 then
	begin
		//There is not selected RwyDirection
		exit;
	end;

	FRWYDirection := FRWYDirectionList.Item [cbRwyDirection.ItemIndex]; //?????????

	fRWYCLDir := FRWYDirection.Direction;

	FAlignedApproachDir := fRWYCLDir;
	FNotAlignedApproachDir := fRWYCLDir;

	prmCourse.BeginUpdate;
	prmCourse.InConvertionPoint := FRWYDirection.Geo.Clone;
	prmCourse.OutConvertionPoint := FRWYDirection.Prj.Clone;

	FCirclingMinOCH := 300.0;

	FillFirstAnchorPoints;

	prmAbeam.ChangeValue(0);
	SetAligned(rgAlignment.ItemIndex = 0);
//=============================================================
//	FObstalces := GObjectDirectory.Obstalces[FAerodrome.Prj, 100000.0];
end;

procedure TMainForm.ShowFinalAndIntermediateApproach;
var
	AppAzt, fTAS,
	Dist0, Dist1:	Double;
	TmpRange:		TInterval;
	point:			TPoint;
begin
	rgIntermediatSensorTypeClick(rbIAPCH_GNSS);

	FFAF.SensorType := stGNSS;
	FMapt.SensorType := stGNSS;

	if FAligned then
	begin
		F3ApproachDir := FAlignedApproachDir;
		FMAPtInfo.Distance := 0.0
	end
	else
	begin
		F3ApproachDir := FNotAlignedApproachDir;
		FMAPtInfo.Distance := prmAlong.Value;
	end;

// MAPt & SOC
	FMAPt.OutDirection := F3ApproachDir;
	FMAPt.EntryDirection := F3ApproachDir;

	fTAS := IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory],
			FAerodrome.Elevation, FAerodrome.Temperature) +
				GPANSOPSConstants.Constant[arNearTerrWindSp].Value;

	Dist0 := fTAS * GPANSOPSConstants.Constant[arSOCdelayTime].Value;
	Dist1 := fTAS * GPANSOPSConstants.Constant[dpPilotTolerance].Value;

	FMapt.SOCDistance := Dist0 + Dist1 + FMapt.ATT;
	prmSOCFromMAPtDistance.ChangeValue(FMapt.SOCDistance);

//	FMAPt.Bank := GPANSOPSConstants.Constant[dpT_Bank].Value;
	FMAPt.ISA := FAerodrome.Temperature;
	SetMAPt(FFarMAPt);
// FAF
	TmpRange.Left := GPANSOPSConstants.Constant[arFADescent_Min].Value;
	TmpRange.Right := GAircraftCategoryConstants.Constant[arFADescent_Max].Value[FAirCraftCategory];

	FFAFInfo.GradientParameter.BeginUpdate;
	FFAFInfo.GradientParameter.Range := TmpRange;
//	FFAFInfo.GradientParameter.Value := TmpRange.Left;
//	FFAFInfo.GradientParameter.EndUpdate;

	prmMAPtCourse.BeginUpdate(False);
	prmMAPtCourse.InConvertionPoint := FMAPt.GeoPt.Clone;
	prmMAPtCourse.OutConvertionPoint := FMAPt.PrjPt.Clone;
	prmMAPtCourse.Value := F3ApproachDir;
	prmMAPtCourse.EndUpdate;

	FFAF.OutDirection := F3ApproachDir;
	FFAF.EntryDirection := F3ApproachDir;
	FFAF.Gradient := FFAFInfo.GradientParameter.Range.Left;
	FFAF.IAS := prmIFIAS.Range.Right;
//	FFAF.Bank := GPANSOPSConstants.Constant[arBankAngle].Value;
	FFAF.ISA := FAerodrome.Temperature;

	TmpRange.Left := GPANSOPSConstants.Constant[arMinRangeFAS].Value;
	TmpRange.Right := GPANSOPSConstants.Constant[arMaxRangeFAS].Value;

	if not FAligned then
		TmpRange.Left := TmpRange.Left + Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

	FFAFInfo.DistanceParameter.BeginUpdate;
	FFAFInfo.DistanceParameter.Range := TmpRange;

	if FApproachType = 0 then
	begin
		TmpRange.Left := TmpRange.Left * FFAFInfo.GradientParameter.Value +
								GPANSOPSConstants.Constant[arAbv_Treshold].Value + FRWYDirection.ElevTdz;
		TmpRange.Right := TmpRange.Right * FFAFInfo.GradientParameter.Value +
								GPANSOPSConstants.Constant[arAbv_Treshold].Value + FRWYDirection.ElevTdz;
	end
	else
	begin
		TmpRange.Left := TmpRange.Left * FFAFInfo.GradientParameter.Value +
								FCirclingMinOCH + FAerodrome.Elevation;
		TmpRange.Right := TmpRange.Right * FFAFInfo.GradientParameter.Value +
								FCirclingMinOCH + FAerodrome.Elevation;
	end;

	FFAFInfo.AltitudeParameter.BeginUpdate;
	FFAFInfo.AltitudeParameter.Range := TmpRange;

	point := LocalToPrj(FMAPt.PrjPt, F3ApproachDir + PI, TmpRange.Left, 0.0);
	FFAF.PrjPt := point;
	point.Free;

//	FFAFInfo.DistanceParameter.EndUpdate;

	prmIFCourse.BeginUpdate;
	prmIFCourse.InConvertionPoint := FFAF.GeoPt.Clone;
	prmIFCourse.OutConvertionPoint := FFAF.PrjPt.Clone;

// IF
	TmpRange.Left := GAircraftCategoryConstants.Constant[cViafMin].Value[FAirCraftCategory];
	TmpRange.Right := GAircraftCategoryConstants.Constant[cViafMax].Value[FAirCraftCategory];
	FIFInfo.SpeedParameter.BeginUpdate;
	FIFInfo.SpeedParameter.Range := TmpRange;
	FIFInfo.SpeedParameter.EndUpdate;

	FIF.ISA := FAerodrome.Temperature;
//	FIF.Bank := GPANSOPSConstants.Constant[arBankAngle].Value;
	FIF.IAS := FIFInfo.SpeedParameter.Range.Right;

	AppAzt := DirToAzimuth(FFarMAPt, F3ApproachDir, GPrjSR, GGeoSR);
	udIFCourse.Min := Round(Modulus(Ceil(AppAzt - RadToDeg(GPANSOPSConstants.Constant[arImMaxIntercept].Value)), 360.0));
	udIFCourse.Max := Round(Modulus(Ceil(AppAzt + RadToDeg(GPANSOPSConstants.Constant[arImMaxIntercept].Value)), 360.0));

//	seIFCourse.MinValue := Round(Modulus(Ceil(AppAzt - RadToDeg(FPANSOPSConstants.Constant[arImMaxIntercept].Value)), 360.0));
//	seIFCourse.MaxValue := Round(Modulus(Ceil(AppAzt + RadToDeg(FPANSOPSConstants.Constant[arImMaxIntercept].Value)), 360.0));

	TmpRange.Left	:= Modulus(F3ApproachDir - GPANSOPSConstants.Constant[arImMaxIntercept].Value, 2*PI);
	TmpRange.Right	:= Modulus(F3ApproachDir + GPANSOPSConstants.Constant[arImMaxIntercept].Value, 2*PI);

	FIFInfo.AngleParameter.BeginUpdate;
	FIFInfo.AngleParameter.Range := TmpRange;
	FIFInfo.AngleParameter.EndUpdate;
//===========================================================================

//	FFAFInfo.DistanceParameter.Value = FFAFInfo.DistanceParameter.Range.Left;
//	FFAFInfo.DistanceParameter.EndUpdate;

	FFAFInfo.Distance := FFAFInfo.DistanceParameter.Range.Left;
	FFAFInfo.Gradient := FFAFInfo.GradientParameter.Range.Left;

	FIFInfo.DistanceParameter.BeginUpdate;
	FIFInfo.DistanceParameter.Value := FIFInfo.DistanceParameter.Range.Left;

//	FIFInfo.AngleParameter.BeginUpdate;
//	FIFInfo.AngleParameter.Value := F3ApproachDir;
//	FIFInfo.Distance := FIFInfo.DistanceParameter.Range.Left;
//	FIFInfo.Angle := F3ApproachDir;

	FIFInfo.Gradient := FFAFInfo.Gradient;//FIFInfo.GradientParameter.Value;
	FIFInfo.Altitude := FIFInfo.AltitudeParameter.Value;
	FIFInfo.Speed := FIFInfo.SpeedParameter.Range.Right;
	FIFInfo.Angle := F3ApproachDir;
	FIFInfo.DistanceParameter.EndUpdate;

	prmIFMaxAngle.BeginUpdate;
	prmIFMaxAngle.MaxValue := 0.5 * PI;
	prmIFMaxAngle.EndUpdate;

	FFAFInfo.AltitudeParameter.EndUpdate;

	FlightPhaseUnit.InitModule(GPANSOPSConstants, GAircraftCategoryConstants,
					FAircraftCategory);
	FFAOCA := GPANSOPSConstants.Constant[arFASeg_FAF_MOC].Value + FAerodrome.Elevation;
end;

procedure TMainForm.ShowApproach_ArrivalSegments;
var
	Axis:		TLine;
	LocalPoint:	TPoint;
	sigPoint:	TSignificantPoint;
	I, N:		Integer;
begin
	prmMAPtDistance.BeginUpdate;
	prmMAPtDistance.MaxValue := prmFAFDistance.Value;
	prmMAPtDistance.MinValue := GPANSOPSConstants.Constant[arMinRangeFAS].Value;
	prmMAPtDistance.Value := prmFAFDistance.Value;
	prmMAPtDistance.EndUpdate;

//==============================================================================
	cbMAPtList.Clear;
	LocalPoint := TPoint.Create;
	Axis := TLine.Create(FFAF.PrjPt, F3ApproachDir);

//	MinDist := GPANSOPSConstants.Constant[arMinRangeFAS].Value;
//	MaxDist := GPANSOPSConstants.Constant[arMaxRangeFAS].Value;

	N := FSignificantCollection.Count;
	for I := 0 to N-1 do
	begin
		sigPoint := FSignificantCollection.Item [i].asSignificantPoint;
		PrjToLocal(Axis, sigPoint.Prj, LocalPoint);
		if (LocalPoint.X >= prmMAPtDistance.MinValue) and (LocalPoint.X <= prmMAPtDistance.MaxValue)
			and (Abs(LocalPoint.Y) <= 0.25*GGNSSConstants.Constant[MAPt_].XTT) then
			cbMAPtList.AddItem(sigPoint.AixmID, sigPoint);
	end;

	rbMAPtFromList.Enabled := cbMAPtList.Items.Count > 0;

	if cbMAPtList.Items.Count > 0 then
		cbMAPtList.ItemIndex := 0
	else
		rbMAPtByDist.Checked := True;

	LocalPoint.Free;
	Axis.Free;

//==============================================================================

	AddBranchFrame1.Execute(FSignificantCollection, FIFInfo, fpInitialApproach, FAerodrome.Prj, 150000.0);
	NextBtn.Enabled := False;
end;

//==============================================================================

function TMainForm.CalcFAOCA(MinOCA: Double; var MaxIx, MaxL: Integer): Double;
var
	OCA:				Double;
	I, J, N, M:			Integer;
	ObstalceInfoList:	TList;
	ObstacleInfo:		TObstacleInfo;
begin
	MaxIx := -1;
	MaxL := -1;
	OCA := MinOCA;

	N := FObstacleListList.Count;
	for I := 0 to N - 1 do
	begin
		ObstalceInfoList := TList(FObstacleListList.Items[I]);
		M := ObstalceInfoList.Count;

		for J := 0 to M - 1 do
		begin
			ObstacleInfo := TObstacleInfo(ObstalceInfoList.Items[J]);
			if(not ObstacleInfo.Ignored) and (ObstacleInfo.FlightProcedure = fpFinalApproach) then
				if ObstacleInfo.ReqH > OCA then
				begin
					OCA := ObstacleInfo.ReqH;
					MaxIx := J;
					MaxL := I;
				end;
		end;
	end;
	result := OCA;
end;

function TMainForm.GetMAObstAndMinOCA(ObstacleList: TList; FullPoly, PrimaryPoly: TPolygon; Gradient: Double; var MaxIx: Integer): Double;
var
	Obstacle:		TObstacle;
	ObstacleInfo:	TObstacleInfo;
	FAF2THRDist,
	FullFAMOC,
	AddMOC, MinOCA,
	Dist0, Dist1:	Double;
	I, N:			Integer;
begin
	N := ObstacleList.Count;

	MinOCA := prmOCA.Value;
	MaxIx := -1;

	FAF2THRDist := Hypot(FFAF.PrjPt.X - FFarMAPt.X , FFAF.PrjPt.Y - FFarMAPt.Y);
	if not FAligned then
		FAF2THRDist := FAF2THRDist +
			Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

	Dist0 := FAF2THRDist - GPANSOPSConstants.Constant[arMOCChangeDist].Value;
	AddMOC := (Dist0 * GPANSOPSConstants.Constant[arAddMOCCoef].Value) * Integer(Dist0 > 0);
	FullFAMOC := FlightPhases[fpFinalApproach].MOC + AddMOC;

	for I := 0 To N - 1 do
	begin
		Obstacle := TObstacle(ObstacleList.Items[I]);
		ObstacleInfo := TObstacleInfo.Create(Obstacle);

		ObstacleInfo.Flags := Byte(IsPointInPoly(Obstacle.Prj, PrimaryPoly));
		if ObstacleInfo.Flags and 1 = 0 then
		begin
			Dist0 := PointToRingDistance(Obstacle.Prj, PrimaryPoly.Ring[0]);
			Dist1 := PointToRingDistance(Obstacle.Prj, FullPoly.Ring[0]);
			ObstacleInfo.fTmp := Dist1/(Dist0 + Dist1);
		end
		else
			ObstacleInfo.fTmp := 1.0;

		ObstacleInfo.Dist := PointToLineDistance(Obstacle.Prj, FSOC.PrjPt, prmCourse.Value - 0.5 * PI);

		if ObstacleInfo.Dist >= 0 then
			ObstacleInfo.FlightProcedure := fpIntermediateMissedApproach
		else
			ObstacleInfo.FlightProcedure := fpInitialMissedApproach;

		ObstacleInfo.Leg := -1;
		ObstacleInfo.MOC := FlightPhases[ObstacleInfo.FlightProcedure].MOC * ObstacleInfo.fTmp;
		ObstacleInfo.ReqH := ObstacleInfo.Elevation + ObstacleInfo.MOC;

		if ObstacleInfo.Dist >= 0 then
			ObstacleInfo.ReqOCA := ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradient
		else
			ObstacleInfo.ReqOCA := Min(ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradient,
										ObstacleInfo.Elevation + FullFAMOC*ObstacleInfo.fTmp);

		if MinOCA < ObstacleInfo.ReqOCA then
		begin
			MaxIx := I;
			MinOCA := ObstacleInfo.ReqOCA;
		end;

		ObstacleList.Items[I] := ObstacleInfo;
	end;
	result := MinOCA;
end;

procedure TMainForm.ShowStraightMissedApproach;
var
	I, N,
	MaxIx,
	MaxFAIx, MaxL,
	IMALenghtIx,
	MAHFLenghtIx:		Integer;

//	Dist0, Dist1, SOCDist,
	fOCH,
	fTmp, TurnAngle,
	MaxTurnAngle,
	R, Gradient,
	fTAS:				Double;

	FullPoly,
	PrimaryPoly:		TPolygon;

	ObstacleList:		TList;
//	Branch:				TList;
	ObstacleInfo:		TObstacleInfo;

//	Left:				TPolygon;
//	TIA:				TPolygon;
//	tmpPt:				TPoint;
//	CutterPart:			TPart;
//	CutterPline:		TPolyline;
begin
	N := FIABranchs.Count;
	if N < 1 then
	begin
		exit;
	end;

	AddBranchFrame1.Cancel;
	
	if SMATFullPolyList.Count > 0 then
	begin
		TPolygon(SMATFullPolyList.Items[0]).Clear;
		TPolygon(SMATPrimaryPolyList.Items[0]).Clear;
	end
	else
	begin
		SMATFullPolyList.Add(TPolygon.Create);
		SMATPrimaryPolyList.Add(TPolygon.Create);
	end;

//	cbBranch.Clear;
{
	for I := 0 to N - 1 do
	begin
		Branch := TList(FIABranchs.Items[I]);
		cbBranch.Items.Add(TLeg(Branch.Items[0]).StartFIX.Name);
	end;
}
{	if cbBranch.Items.Count > 0 then
	begin
		cbBranch.ItemIndex := 0;
		cbBranchChange(cbBranch);
	end;
}
	if rgAlignment.ItemIndex = 1 then
	begin
		fTAS := IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory],
				FAerodrome.Elevation, FAerodrome.Temperature) +
					GPANSOPSConstants.Constant[arNearTerrWindSp].Value;
		R := BankToRadius(GPANSOPSConstants.Constant[arBankAngle].Value, fTAS);
		TurnAngle := prmCourse.Value - FRWYDirection.Direction;
		if TurnAngle < 0 then TurnAngle := TurnAngle + 2*PI;
		if TurnAngle > PI then TurnAngle := 2*PI - TurnAngle;

		MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAircraftCategory];
		if (FAircraftCategory <= acB)and (TurnAngle < GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC]) then
				MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC];

		FFAAligOCA := GPANSOPSConstants.Constant[arAbv_Treshold].Value +
				(prmAlong.Value + R * Tan(0.5 * MaxTurnAngle) +
				5.0 * fTAS) * GPANSOPSConstants.Constant[arFADescent_Nom].Value;
	end
	else
		FFAAligOCA := FlightPhases[fpFinalApproach].MOC;

	FFAAligOCA := FFAAligOCA + FAerodrome.Elevation;
	FFAMAPtPosOCA := prmFAFAltitude.Value - prmMAPtDistance.Value*prmFAFGradient.Value;

	FFAOCA := CalcFAOCA(Max(FFAAligOCA, FFAMAPtPosOCA), MaxFAIx, MaxL);
	prmOCA.ReadOnly := True;
	prmOCA.ChangeValue(FFAOCA);
	FMAPt.Altitude := FFAOCA;

	fOCH := FFAOCA - FAerodrome.Elevation;
	fTmp := CConverters[puAltitude].ConvertFunction (fOCH, cdToOuter, nil);
	editOCH.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	if MaxFAIx>=0 then
		labelFAOcaCaused.Caption := 'Caused by obstacle ' + 
			TObstacleInfo(TList(FObstacleListList.Items[MaxL]).Items[MaxFAIx]).AID
	else if FFAAligOCA > FFAMAPtPosOCA  then
		labelFAOcaCaused.Caption := 'Caused by Track alignment.'
	else
		labelFAOcaCaused.Caption := 'Caused by MAPt position.';
{
	Dist0 := fTAS * GPANSOPSConstants.Constant[arSOCdelayTime].Value;
	Dist1 := fTAS * GPANSOPSConstants.Constant[dpPilotTolerance].Value;
	SOCDist := Dist0 + Dist1 + FMapt.ATT;
	FMapt.SOCDistance := SOCDist;
	prmSOCDistance.ChangeValue(SOCDist);
}

	prmMAClimb.BeginUpdate;
	prmMAClimb.Value := 1;
	prmMAClimb.EndUpdate;
//=================================================================
	if SMASFullPolyList.Count >0 then
	begin
		SMASFullPolyList.Items[0] := TPolygon.Create;
		SMASPrimaryPolyList.Items[0] := TPolygon.Create;
	end
	else
	begin
		SMASFullPolyList.Add(TPolygon.Create);
		SMASPrimaryPolyList.Add(TPolygon.Create);
	end;

	CreateStraightInSegment(0);

	FullPoly := SMASFullPolyList.Items[0];
	PrimaryPoly := SMASPrimaryPolyList.Items[0];

	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);
	FMAPt.DeleteGraphics;

	FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
	FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);

//
//	Center: TPoint; DirInRadian, X, Y: Double
	FMAPt.StraightArea := PrimaryPoly;
{
	CutterPart := TPart.Create;
	CutterPline := TPolyline.Create;
	Left := nil;	TIA := nil;
	tmpPt := LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FMapt.SOCDistance, 100000.0);
	CutterPart.AddPoint(tmpPt);
	LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FMapt.SOCDistance, -100000.0, tmpPt);
	CutterPart.AddPoint(tmpPt);				tmpPt.Free;
	CutterPline.AddPart(CutterPart);		CutterPart.Free;

	GGeoOperators.Cut(PrimaryPoly, CutterPline, TGeometry(Left), TGeometry(TIA));
	FMAPt.TIArea.Assign(TIA);

	Left.Free;
	TIA.Free;
}
	FMAPt.RefreshGraphics;
	ObstacleList := GetObstacles(FullPoly, FObstalces);

	I := cbMAclimbGradient.ItemIndex;
	if I < 0 then
	begin
		I := 0;
		cbMAclimbGradient.ItemIndex := I;
	end;

	Gradient := GrTable[I];
	FMAMinOCA := GetMAObstAndMinOCA(ObstacleList, FullPoly, PrimaryPoly, Gradient, MaxIx);
	FMATF.Gradient := Gradient;

	FIMALenght := 0;

	FMAHFLenght := (FAerodrome.Elevation + FlightPhases[fpEnroute].MOC - FMAMinOCA)/ Gradient;
	if FMAHFLenght < 0 then FMAHFLenght := 0;

	IMALenghtIx := -1;
	MAHFLenghtIx := -1;

	N := ObstacleList.Count;
	for I := 0 To N - 1 do
	begin
		ObstacleInfo := ObstacleList.Items[I];

		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpFinalMissedApproach].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if (fTmp > ObstacleInfo.Dist) and (fTmp > FIMALenght) then
		begin
			FIMALenght := fTmp;
			IMALenghtIx := I
		end;

		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpEnroute].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if FMAHFLenght < fTmp then
		begin
			FMAHFLenght := fTmp;
			MAHFLenghtIx := I
		end;
	end;

	ReportForm.AddMissedApproach(ObstacleList);
	SMASFObstacleListList.Add(ObstacleList);

	fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA, cdToOuter, nil);
	editMAOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FIMALenght, cdToOuter, nil);
	Edit3.Text := RoundToStr(fTmp, GDistanceAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FMAHFLenght + FMAPt.SOCDistance, cdToOuter, nil);
	Edit4.Text := RoundToStr(fTmp, GDistanceAccuracy);

	prmMAHFFromMAPtDist.BeginUpdate;
	prmMAHFFromMAPtDist.MinValue := FMAHFLenght + FMAPt.SOCDistance;
	prmMAHFFromMAPtDist.MaxValue := 200000;
	prmMAHFFromMAPtDist.EndUpdate;

	Edit5.Text := '';
	Edit6.Text := '';
	Edit7.Text := '';

	if MaxIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MaxIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 2;
		Edit5.Text := ObstacleInfo.AID;
	end;

	if IMALenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[IMALenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 4;
		Edit6.Text := ObstacleInfo.AID;
	end;

	if MAHFLenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MAHFLenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 8;
		Edit7.Text := ObstacleInfo.AID;
	end;

	fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA + FMAHFLenght * Gradient, cdToOuter, nil);
	Edit8.Text := RoundToStr(fTmp, GAltitudeAccuracy);

{	prmPreTurnAltitude.BeginUpdate;
	prmPreTurnAltitude.MinValue := fTmp;
	prmPreTurnAltitude.MaxValue := FMAMinOCA + FMSA;
	prmPreTurnAltitude.Value := fTmp;
	prmPreTurnAltitude.EndUpdate;
}
end;

const
	MinMATurnDist: Double = 10000.0;
	MaxMATurnDist = 64000.0;

procedure TMainForm.ShowTurnMissedApproach;
var
	limit,
	delta:		Double;
begin
	prmMATBankAngle.BeginUpdate(False);
	prmMATBankAngle.MinValue := GPANSOPSConstants.Constant[arMABankAngle].Value;
	prmMATBankAngle.MaxValue := GPANSOPSConstants.Constant[arBankAngle].Value;
	prmMATBankAngle.Value := prmMATBankAngle.MinValue;
	prmMATBankAngle.EndUpdate;

	FMATF.TurnAt			:= TTurnAt(cbTurnAt.ItemIndex);
	FMATF.TurnDirection		:= TSideDirection(2*cbTurnSide.ItemIndex-1);
	FMATF.StraightInPrimaryPolygon		:= TPolygon(SMASPrimaryPolyList.Items[0]);
	FMATF.StraightInSecondaryPolygon	:= TPolygon(SMASFullPolyList.Items[0]);

	FMATF.ISA				:= FAerodrome.Temperature;
	FMATF.Bank				:= prmMATBankAngle.Value;
	FMATF.EntryDirection	:= FMAPt.EntryDirection;
	FMATF.SensorType		:= FMAPt.SensorType;			//TSensorType(rgMAPtSensorType.ItemIndex);

	prmPreOCA.ChangeValue(prmOCA.Value, False);

	prmTurnFromMAPtDist.Active := False;
	prmTurnFromMAPtDist.BeginUpdate;
	prmTurnFromMAPtDist.MinValue := FMAPt.SOCDistance + IfThen(cbTurnAt.ItemIndex = 2, FMATF.ATT, 0);
	prmTurnFromMAPtDist.MaxValue :=  80000.0;
//	prmTurnFromSOCDist.PerformCheck(cbTurnAt.ItemIndex = 2);
	prmTurnFromMAPtDist.EndUpdate;
	prmTurnFromMAPtDist.Active := cbTurnAt.ItemIndex = 2;
	prmTurnFromMAPtDist.ReadOnly := cbTurnAt.ItemIndex < 2;
	prmTurnFromMAPtDist.ShowMinMaxHint := prmTurnFromMAPtDist.Active;

	prmPreTurnAltitude.Active := False;
	prmPreTurnAltitude.BeginUpdate;
	prmPreTurnAltitude.MinValue := prmOCA.Value;
	prmPreTurnAltitude.MaxValue := prmOCA.Value + 2000;
	prmPreTurnAltitude.Value := prmOCA.Value;
	prmPreTurnAltitude.EndUpdate;
	prmPreTurnAltitude.Active := cbTurnAt.ItemIndex = 1;
	prmPreTurnAltitude.ReadOnly := cbTurnAt.ItemIndex <> 1;
	prmPreTurnAltitude.ShowMinMaxHint := prmPreTurnAltitude.Active;

	prmMAIAS.Active := False;
	prmMAIAS.BeginUpdate;
	prmMAIAS.MinValue := GAircraftCategoryConstants.Constant[cVmaInter].Value[FAircraftCategory];
	prmMAIAS.MaxValue := GAircraftCategoryConstants.Constant[cVmaFaf].Value[FAircraftCategory];
	prmMAIAS.Value := prmMAIAS.MaxValue;
	prmMAIAS.EndUpdate;
	prmMAIAS.Active := True;

//	MinMATurnDist := FIMALenght +

	prmNWPTFromTPDist.Active := False;
	prmNWPTFromTPDist.BeginUpdate;
	prmNWPTFromTPDist.MinValue := MinMATurnDist;
	prmNWPTFromTPDist.MaxValue := MaxMATurnDist;
	prmNWPTFromTPDist.Value := MinMATurnDist;
	prmNWPTFromTPDist.EndUpdate;
	prmNWPTFromTPDist.Active := True;
	prmNWPTFromTPDist.ReadOnly := rbExistingPoint.Checked;

	delta := DegToRad(240 - 120*rgToFIX.ItemIndex);
	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.InConvertionPoint := FMAPt.GeoPt.Clone;
	prmTurnCourse.OutConvertionPoint := FMAPt.PrjPt.Clone;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex*delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1-cbTurnSide.ItemIndex)*delta;
	prmTurnCourse.Value := FMAPt.EntryDirection;
	prmTurnCourse.EndUpdate;
	prmTurnCourse.Active := True;
	prmTurnCourse.ReadOnly := rbExistingPoint.Checked;

	prmTurn1.BeginUpdate;
	prmTurn2.BeginUpdate;
	prmTurn3.BeginUpdate;

	prmTurn1.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn2.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn3.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;

	prmTurn1.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn2.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn3.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;

	prmTurn1.EndUpdate;
	prmTurn2.EndUpdate;
	prmTurn3.EndUpdate;

	prmMATBankAngle.ChangeValue(GPANSOPSConstants.Constant[arMABankAngle].Value);

//	rbExistingPoint.Checked := True;
//	FreeObject(TObject(FMAPtLeg));
//	FMAPtLeg := TLeg.Create(FMApt, FMATF, fpIntermediateMissedApproach);

	cbTurnAtChange(cbTurnAt);
end;

procedure TMainForm.ShowMATurnPage;
begin

end;

// END ======================================================= Common procedures

procedure TMainForm.CalcCourseInterval;
var
	limit,
	delta:		Double;
	N:			Integer;
begin
	FMATF.TurnDirection := TSideDirection(2 * cbTurnSide.ItemIndex - 1);

	delta := DegToRad(240 - 120 * rgToFIX.ItemIndex);

	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex * delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1 - cbTurnSide.ItemIndex) * delta;
	prmTurnCourse.EndUpdate;
	prmTurnCourse.Active := True;

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then	rbCreateNew.Checked := True;

//	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
//	else								prmTurnCourseChangeValue(prmTurnCourse);
end;

function TMainForm.FillMATurnPoints: Integer;
var
	i, j:			Integer;
	prevPoint,
	sigPoint:		TSignificantPoint;
	fTurn, fMinDist,
	dX, dY,
	fTurnAngle,
	delta, limit,
	fDist:			Double;
begin
	if cbTurnWPT.ItemIndex >= 0 then
		prevPoint := TSignificantPoint(cbTurnWPT.Items.Objects[cbTurnWPT.ItemIndex])
	else
		prevPoint := nil;

	cbTurnWPT.Clear;
	fTurn := 1 - 2*cbTurnSide.ItemIndex;		//Right = -1;	Left = 1

	delta := DegToRad(240 - 120 * rgToFIX.ItemIndex);

	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	j := 0;
	for i := 0 to FSignificantCollection.Count - 1 do
	begin
		sigPoint := FSignificantCollection.Item [i].asSignificantPoint;
		dX := sigPoint.Prj.X - FMATF.PrjPt.X;
		dY := sigPoint.Prj.Y - FMATF.PrjPt.Y;
		fDist := Hypot(dX, dY);
		if (fDist < prmNWPTFromTPDist.MinValue)or(fDist > prmNWPTFromTPDist.MaxValue) then continue;

		fTurnAngle := Modulus((ArcTan2(dY, dX) - FMAPt.EntryDirection) * fTurn, C_2xPI);
		if fTurnAngle > delta then continue;


		if (rgFlyMode.ItemIndex = 0) or (rgToFIX.ItemIndex = 0) then
			fMinDist := MinMATurnDist
		else
			fMinDist := FMATF.CalcFromMinStablizationDistance(fTurnAngle);

		if fMinDist > fDist then		continue;

		if sigPoint = prevPoint then
			j := cbTurnWPT.Items.Count;
		cbTurnWPT.AddItem(sigPoint.AixmID, sigPoint);
	end;

	result := cbTurnWPT.Items.Count;
	if result > 0 then
		cbTurnWPT.ItemIndex := j;
end;

procedure TMainForm.cbAerodromesSelect(Sender: TObject);
var
	I, N, K:			Integer;
//	ListItem:			TListItem;
begin
	K := cbAerodromes.ItemIndex;
	editRwyDirectionTrueBrg.Text := '';
	editRwyDirectionMagBrg.Text := '';
	editRwyDirectionElev.Text := '';
	editRwyDirectionClearway.Text := '';
	editRwyDirectionDisplacement.Text := '';
	cbRWY.Clear;
	cbRwyDirection.Clear;

//	ListView1.Items.Clear;

	NextBtn.Enabled := K >= 0;
	if K < 0 then exit;
	FAerodrome := GObjectDirectory.AHP[TUnicalName(cbAerodromes.Items.Objects[K]).ID];

  editAerodromeAixmID.Text := FAerodrome.AixmID;
	editAerodromeElev.Text := RoundToStr(CConverters[puAltitude].ConvertFunction(FAerodrome.Elevation, cdToOuter, nil), GAltitudeAccuracy);
	editAerodromeMagVar.Text := FormatFloat ('0.0', FAerodrome.MagVar);
	editAerodromeISA.Text :=  FormatFloat ('00', FAerodrome.Temperature);

	FSignificantCollection := GObjectDirectory.SignificantPoints [GOrgID, FAerodrome, FAerodrome.Geo, 1000000];

	FRWYList := GObjectDirectory.RWYList[FAerodrome.AixmID];
	N := FRWYList.Count;

	iChecked := -1;

	NextBtn.Enabled := N > 0;

	if N > 0 then
	begin
		for i := 0 to N-1 do
		begin
			cbRWY.AddItem(TUnicalName(FRWYList.Item[I]).Name, FRWYList.Item[I]);
//			ListItem := ListView1.Items.Add;
//			ListItem.Caption := TUnicalName(FRWYList.Item[I]).Name;
		end;
//		ListView1.Items[0].Checked := True;

		cbRWY.ItemIndex := 0;
		cbRWYSelect (cbRWY)
	end;
end;

procedure TMainForm.cbRWYSelect(Sender: TObject);
var
	I:		Integer;
	RWY:	TRWY;
begin
	cbRwyDirection.Clear;

	I := cbRWY.ItemIndex;

	NextBtn.Enabled := I >= 0;

	if I < 0 then exit;

	RWY := GObjectDirectory.RWY[TUnicalName(FRWYList.Item[I]).ID];
//	TRWY(cbRWY.Items.Objects[cbRWY.ItemIndex]);

	FRWYDirectionList := GObjectDirectory.RWYDirectionList [RWY.AixmID, FAerodrome];

	for i := 0 to FRWYDirectionList.Count - 1 do
		cbRwyDirection.AddItem (FRWYDirectionList.Item[i].Name, FRWYDirectionList.Item[i]);

	if cbRwyDirection.Items.Count > 0 then
		cbRwyDirection.ItemIndex := 0;

	cbRwyDirectionSelect (cbRwyDirection);
end;

procedure TMainForm.cbRwyDirectionSelect(Sender: TObject);
var
	K:		Integer;
	fTmp:	Double;
begin
	K := cbRwyDirection.ItemIndex;
	editRwyDirectionTrueBrg.Text := '';
	editRwyDirectionMagBrg.Text := '';
	editRwyDirectionElev.Text := '';
	editRwyDirectionClearway.Text := '';
	editRwyDirectionDisplacement.Text := '';

	NextBtn.Enabled := K >= 0;

	if K < 0 then exit;
	FRWYDirection := FRWYDirectionList.Item[K];

	editRwyDirectionTrueBrg.Text := FormatFloat ('00.0', FRWYDirection.trueBearing);
	editRwyDirectionMagBrg.Text := FormatFloat ('00.0', FRWYDirection.magBearing);

	fTmp := CConverters[puAltitude].ConvertFunction (FRWYDirection.elevTdz, cdToOuter, nil);
	editRwyDirectionElev.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FRWYDirection.clearway, cdToOuter, nil);
	editRwyDirectionClearway.Text := RoundToStr(fTmp, GDistanceAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FRWYDirection.displacement, cdToOuter, nil);
	editRwyDirectionDisplacement.Text := RoundToStr(fTmp, GDistanceAccuracy);

//	editRwyDirectionClearway.Text := FormatFloat ('00.00', FRWYDirectionList.Item[K].clearway);
//	editRwyDirectionDisplacement.Text := FormatFloat ('00.00', FRWYDirectionList.Item[K].displacement);
end;

// ================================================================= Page 1
procedure TMainForm.rgApproachTypeClick(Sender: TObject);
begin
	FApproachType := 0;//rgApproachType.ItemIndex;
//	grbRWY.Visible := rgApproachType.ItemIndex = 0;
//	grbRwyDirection.Visible := rgApproachType.ItemIndex = 0;

//	ListView1.Visible := rgApproachType.ItemIndex = 1;

//	if rgApproachType.ItemIndex = 0 then
//	begin
		labFAFDistance.Caption := 'FAF-THR Distance:'
//	end
//	else
//	begin
//		labFAFDistance.Caption := 'FAF-MAPt Distance:'
//	end;
end;

procedure TMainForm.ListView1Change(Sender: TObject; Item: TListItem; Change: TItemChange);
begin
{var
	I, N:		Integer;
	nChecked:	Integer;
begin
	if Change = ctState then
	begin
		nChecked := 0;
		N := ListView1.Items.Count;
		for I := 0 to N-1 do
			if ListView1.Items[I].Checked then
			begin
				Inc(nChecked);
				iChecked := I;
				Break;
			end;

		if nChecked = 0 then
		begin
			if iChecked < 0 then	iChecked := 0;
			ListView1.Items[iChecked].Checked := True;
		end;
	end;
}
end;

// ================================================================= Page 2

procedure TMainForm.FillFirstAnchorPoints;
var
	I:						Integer;
	Tan_5,
	fDist, d,
	Denom, Num, K:			Double;
	sigPoint:				TSignificantPoint;

	Local_0_0,
	LocalsigPoint:			TPoint;
	LeftPoint,
	RightPoint:				TGeometry;
	Axis, Line5, LineD:		TLine;
	IntervalList1,
	IntervalList2,
	tmpIntervalList:		TIntervalList;
	Vector:					TVector;
	FullInterval,
	TmpInterval:			TInterval;
begin
	Tan_5 := Tan(GPANSOPSConstants.Constant[arStrInAlignment].Value);
	d := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAirCraftCategory];

//	Treshold15_30 := Tan(DegToRad(d));

	FullInterval.Left := GPANSOPSConstants.Constant[arMinInterDist].Value;
	FullInterval.Right := arMaxInterDist;
	tmpIntervalList := TIntervalList.Create(FullInterval);

	for I := 0 to FNotAlignedSgfPointRange.Count - 1 do
		TIntervalList(FNotAlignedSgfPointRange.Items[I]).Free;

	FNotAlignedSgfPointRange.Clear;

//	cbOnTrackSgfPoint.Items.Clear;
	Local_0_0 := TPoint.Create(0.0, 0.0);
	Axis := TLine.Create(Local_0_0, 0.0);
	Local_0_0.Free;

	LocalsigPoint := TPoint.Create;
	Line5 := TLine.Create;
	LineD := TLine.Create;
	Vector := TVector.Create(d);

	for i := 0 to FSignificantCollection.Count - 1 do
	begin
		sigPoint := FSignificantCollection.Item [i].asSignificantPoint;
		fDist := Hypot(sigPoint.Prj.X - FRWYDirection.Prj.X, sigPoint.Prj.Y - FRWYDirection.Prj.Y);

		if fDist <= 100000.0 then
		begin
			PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, sigPoint.Prj, LocalsigPoint);

			Denom := Abs(LocalsigPoint.X - GPANSOPSConstants.Constant[arMinInterDist].Value);
			Num := (Abs(LocalsigPoint.Y) - GPANSOPSConstants.Constant[arMinInterToler].Value);

			if Denom > 0.0 then		K := Num / Denom
			else if Num = 0 then	K := 0
			else					K := 10.0;

			if K <= Tan_5 then		FAlignedSgfPoint.Add(sigPoint);

			Line5.RefPoint := LocalsigPoint;
			LineD.RefPoint := LocalsigPoint;
//=====================================================================================================
			Vector.Direction := GPANSOPSConstants.Constant[arStrInAlignment].Value;
			Line5.DirVector := Vector;
			Vector.Direction := d;
			LineD.DirVector := Vector;
			IntervalList1 := TIntervalList.Create(FullInterval);

			LeftPoint := LineLineIntersect(Axis, Line5);
			RightPoint := LineLineIntersect(Axis, LineD);

//			with LeftPoint.asPoint do
//			begin
				TmpInterval.Left := LeftPoint.asPoint.X;
				LeftPoint.asPoint.Free;
//			end;

			TmpInterval.Right := RightPoint.asPoint.X;
			RightPoint.asPoint.Free;

			if TmpInterval.Left > TmpInterval.Right then
			begin
				K := TmpInterval.Left;
				TmpInterval.Left := TmpInterval.Right;
				TmpInterval.Right := K;
			end;
//			tmpIntervalList := TIntervalList.Create(TmpInterval);
			tmpIntervalList.Interval[0] := TmpInterval;
			IntervalList1.Intersect(tmpIntervalList);
//=====================================================================================================

			Line5.DirVector.Direction :=  -GPANSOPSConstants.Constant[arStrInAlignment].Value;
			LineD.DirVector.Direction := -d;
			IntervalList2 := TIntervalList.Create(FullInterval);

			LeftPoint := LineLineIntersect(Axis, Line5);
			RightPoint := LineLineIntersect(Axis, LineD);

			TmpInterval.Left := LeftPoint.asPoint.X;
			TmpInterval.Right := RightPoint.asPoint.X;
			LeftPoint.asPoint.Free;
			RightPoint.asPoint.Free;

			if TmpInterval.Left > TmpInterval.Right then
			begin
				K := TmpInterval.Left;
				TmpInterval.Left := TmpInterval.Right;
				TmpInterval.Right := K;
			end;
//			tmpIntervalList := TIntervalList.Create(TmpInterval);
			tmpIntervalList.Interval[0] := TmpInterval;
			IntervalList2.Intersect(tmpIntervalList);
//=====================================================================================================
			IntervalList1.Union(IntervalList2);
			if IntervalList1.Count > 0 then
			begin
				FNotAlignedSgfPoint.Add(sigPoint);
				FNotAlignedSgfPointRange.Add(IntervalList1)
			end
			else
				IntervalList1.Free;

			IntervalList2.Free;
		end;
	end;

	tmpIntervalList.Free;
	LocalsigPoint.Free;
	Axis.Free;
	Line5.Free;
	LineD.Free;
	Vector.Free;
end;

function TMainForm.FillSecondPoints(TrackIntervalList: TIntervalList): Integer;
var
	I, N, Ix:		Integer;
	TresholdL, TresholdR,
	fTmp, Denom,
	Num, K:				Double;
	SigPoint:			TSignificantPoint;

	LocalBasePoint,
	LocalPoint:			TPoint;
	AxisLine,
	TrackLine:			TLine;
	tmpGeom:			TGeometry;
	AnchorPoint1:		TPoint;
	Suitable:			Boolean;
begin
	N := cbFirstAnchorPoint.Items.Count;

	if FAligned then
	begin
		AxisLine := nil;
		TresholdL := Tan(TrackIntervalList.Interval[0].Left - fRWYCLDir);
		TresholdR := Tan(TrackIntervalList.Interval[0].Right - fRWYCLDir);
		AnchorPoint1 := FAlignedAnchorPoint1;
	end
	else
	begin
		AxisLine := Tline.Create(FRWYDirection.Prj, fRWYCLDir);
		TresholdL := Tan(GPANSOPSConstants.Constant[arStrInAlignment].Value);
		TresholdR := Tan(GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAirCraftCategory]);
		AnchorPoint1 := FNotAlignedAnchorPoint1;
	end;

	cbSecondAnchorPoint.Clear;

	LocalBasePoint := PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, AnchorPoint1);
	LocalPoint := TPoint.Create;

	for I := 0 to N-1 do
	begin
		sigPoint := TSignificantPoint(cbFirstAnchorPoint.Items.Objects[I]);
		fTmp := Sqr(sigPoint.Prj.X - AnchorPoint1.X) + Sqr(sigPoint.Prj.Y - AnchorPoint1.Y);

		if fTmp > EpsilonDistance then
		begin
			PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, sigPoint.Prj, LocalPoint);

			if FAligned then
			begin
				Denom	:= LocalPoint.X - LocalBasePoint.X;
				Num		:= LocalPoint.Y - LocalBasePoint.Y;

				if Abs(Denom) > 0.0 then	K := Num / Denom
				else if Num = 0 then		K := 0
				else						K := 10.0;

				if ((Abs(TresholdL-TresholdR) < EpsilonRadian)and
					(Abs(0.5*(TresholdL+TresholdR)-K) < EpsilonRadian))
					or ((K >= TresholdL) and (K <= TresholdR)) then
					cbSecondAnchorPoint.AddItem (sigPoint.AixmID, sigPoint);
			end
			else
			begin
				if Abs(LocalBasePoint.Y) < EpsilonDistance then
				begin
					Denom	:= LocalPoint.X - LocalBasePoint.X;
					Num		:= LocalPoint.Y - LocalBasePoint.Y;

					if Abs(Denom) > 0.0 then	K := Abs(Num / Denom)
					else if Num = 0 then		K := 0
					else						K := 10.0;

					Suitable := (TresholdL <= K)and(TresholdR >= K);
				end
				else
				begin
					Ix := -1;
					TrackLine := TLine.Create(sigPoint.Prj, AnchorPoint1);
					tmpGeom := LineLineIntersect(AxisLine, TrackLine);
					TrackLine.Free;

					if tmpGeom.GeometryType = gtPoint then
					begin
						PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, tmpGeom.AsPoint, LocalPoint);
						Ix := TrackIntervalList.Contains(LocalPoint.X);
					end;
					Suitable := Ix >= 0;
					tmpGeom.Free;
				end;

				if Suitable then
				begin


					cbSecondAnchorPoint.AddItem (sigPoint.AixmID, sigPoint);
				end;

			end
		end;
	end;

	AxisLine.Free;
	LocalBasePoint.Free;
	LocalPoint.Free;
	result := cbSecondAnchorPoint.Items.Count;
end;

procedure TMainForm.SetAligned(Aligned: Boolean);
var
	I:			Integer;
	Range:		TInterval;
	fTmp,
	Val, d,
	fTAS, R,
	MaxTurnAngle:	Double;
	TurnAngle:	Double;
begin
	FAligned := Aligned;
	cbFirstAnchorPoint.Clear;

	ComboTrackIntervals.Visible := not FAligned;

	if FAligned then
	begin
		prmAlong.Visible := False;
		prmAlong.Active := False;

		prmAbeam.Visible := True;
		prmAbeam.Active := True;

		labMinimalOCA.Visible := False;
		editMinimalOCA.Visible := False;
		labMinimalOCAUnit.Visible := False;

		labMinimalOCH.Visible := False;
		editMinimalOCH.Visible := False;
		labMinimalOCHUnit.Visible := False;

		rgSelectFirstAncorPoint.Items[0] := 'Point Abeam C/L on 1400m from THR';

		Val := FRWYDirection.TrueBearing;
		d := RadToDeg(GPANSOPSConstants.Constant[arStrInAlignment].Value);

		//udApproacCourse.Min := Round(Val - d + 0.4999);
		//udApproacCourse.Max := Round(Val + d - 0.4999);
		//Range.Left := CConverters[puAngle].ConvertFunction(udApproacCourse.Max, cdToInner, prmCourse.InConvertionPoint);
		//Range.Right := CConverters[puAngle].ConvertFunction(udApproacCourse.Min, cdToInner, prmCourse.InConvertionPoint);

		Range.Left := CConverters[puAngle].ConvertFunction(Val + d, cdToInner, prmCourse.InConvertionPoint);
		Range.Right := CConverters[puAngle].ConvertFunction(Val - d, cdToInner, prmCourse.InConvertionPoint);
		FAlignedTrackInterval := Range;


		prmCourse.BeginUpdate;
		prmCourse.Range := Range;
		prmCourse.Value := FAlignedApproachDir;

		rgSelectFirstAncorPoint.Enabled := FAlignedSgfPoint.Count > 0;

		if rgSelectFirstAncorPoint.Enabled then
		begin
			for i := 0 to FAlignedSgfPoint.Count - 1 do
				cbFirstAnchorPoint.AddItem (FAlignedSgfPoint.Item[i].asSignificantPoint.AixmID, FAlignedSgfPoint.Item[i]);

			cbFirstAnchorPoint.ItemIndex := 0;
			cbFirstAnchorPointChange(cbFirstAnchorPoint);
//			if rgSelectFirstAncorPoint.ItemIndex = 0 then
//				prmAbeam.ChangeValue(0.0);
		end;
//		else
//			prmAbeam.ChangeValue(0.0);
//		prmAbeam.PerformCheck := True;

		prmCourse.EndUpdate;
//		FFAOCA := GPANSOPSConstants.Constant[arFASeg_FAF_MOC].Value + FAerodrome.Elevation;
	end
	else
	begin
		prmAbeam.Visible := False;
		prmAbeam.Active := False;

		prmAlong.Visible := True;
		prmAlong.Active := True;

		labMinimalOCA.Visible := True;
		editMinimalOCA.Visible := True;
		labMinimalOCAUnit.Visible := True;

		labMinimalOCH.Visible := True;
		editMinimalOCH.Visible := True;
		labMinimalOCHUnit.Visible := True;

		prmCourse.Value := FNotAlignedApproachDir;

		rgSelectFirstAncorPoint.Items[0] := 'Point Along C/L further than 1400m from THR';

		rgSelectFirstAncorPoint.Enabled := FNotAlignedSgfPoint.Count > 0;

		if rgSelectFirstAncorPoint.Enabled then
		begin
			for i := 0 to FNotAlignedSgfPoint.Count - 1 do
				cbFirstAnchorPoint.AddItem (FNotAlignedSgfPoint.Item[i].asSignificantPoint.AixmID, FNotAlignedSgfPoint.Item[i]);

			cbFirstAnchorPoint.ItemIndex := 0;
			cbFirstAnchorPointChange(cbFirstAnchorPoint);
//			if rgSelectFirstAncorPoint.ItemIndex = 0 then
//				prmAlong.ChangeValue(prmAlong.MinValue);
		end;
//		else
//			prmAlong.ChangeValue(prmAlong.MinValue);

		prmCourse.BeginUpdate;
//		prmCourse.Range :=

//		prmCourse.Active := True;
		OnAlongChangeValue(prmAlong);
//		prmAlong.PerformCheck := True;

		SetTrackInterval(FNotAlignedTrackIntervals[0]);

		prmCourse.EndUpdate;

		fTAS := IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory],
				FAerodrome.Elevation, FAerodrome.Temperature) +
				GPANSOPSConstants.Constant[arNearTerrWindSp].Value;

		R := BankToRadius(GPANSOPSConstants.Constant[arBankAngle].Value, fTAS);
		TurnAngle := prmCourse.Value - FRWYDirection.Direction;
		if TurnAngle < 0 then TurnAngle := TurnAngle + 2*PI;
		if TurnAngle > PI then TurnAngle := 2*PI - TurnAngle;

		MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAircraftCategory];
		if (FAircraftCategory <= acB)and (TurnAngle < GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC]) then
				MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC];

		FFAOCA := GPANSOPSConstants.Constant[arAbv_Treshold].Value +
					(prmAlong.Value + R * Tan(0.5 * MaxTurnAngle) + 5.0 * fTAS) *
						GPANSOPSConstants.Constant[arFADescent_Nom].Value;

		fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA, cdToOuter, nil);
		editMinimalOCH.Text := RoundToStr(fTmp, GAltitudeAccuracy);

		fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA + FAerodrome.Elevation, cdToOuter, nil);
		editMinimalOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);
	end;
end;

procedure TMainForm.SetTrackInterval(TrackInterval: TInterval);
var
	//MinRounded, MaxRounded:	Integer;
	Range:					TInterval;
begin
		//MinRounded := Round(TrackInterval.Left);
		//MaxRounded := Round(TrackInterval.Right);

		//udApproacCourse.Min := MinRounded;
		//udApproacCourse.Max := MaxRounded;

		//Range.Left := CConverters[puAngle].ConvertFunction(MaxRounded, cdToInner,
		//				prmCourse.InConvertionPoint);
		//Range.Right := CConverters[puAngle].ConvertFunction(MinRounded, cdToInner,
		//				prmCourse.InConvertionPoint);


		Range.Left := CConverters[puAngle].ConvertFunction(TrackInterval.Right, cdToInner,
						prmCourse.InConvertionPoint);
		Range.Right := CConverters[puAngle].ConvertFunction(TrackInterval.Left, cdToInner,
						prmCourse.InConvertionPoint);

		prmCourse.BeginUpdate;
		prmCourse.Range := Range;
		prmCourse.EndUpdate;
{
	if TrackInterval.Left = TrackInterval.Right then
	begin
//		prmCourse.ChangerControl.Visible := False;
		prmCourse.Value := AztToDirection(FRWYDirection.Geo, TrackInterval.Right, GGeoSR, GPrjSR);
//		prmCourse.ReadOnly := True;
//		if TrackInterval.Left = Round(TrackInterval.Left) then
//			editCourse.Text := IntToStr(Round(TrackInterval.Right))
//		else
//			editCourse.Text := FloatToStr(TrackInterval.Right);

//		prmCourse.ReadFromControl;
//		prmCourse.OnChangeValue(prmCourse);
//		SetDirection(prmCourse.ReadFromControl);
	end
	else
	begin
//		prmCourse.ChangerControl.Visible := True;
//		prmCourse.ReadOnly := False;

		MinRounded := Round(TrackInterval.Left);
		MaxRounded := Round(TrackInterval.Right);

		udApproacCourse.Min := MinRounded;
		udApproacCourse.Max := MaxRounded;
		Range.Left := Converters[puAngle].ConvertFunction(MaxRounded, cdToInner,
						prmCourse.InConvertionPoint);
		Range.Right := Converters[puAngle].ConvertFunction(MinRounded, cdToInner,
						prmCourse.InConvertionPoint);
		prmCourse.BeginUpdate;
		prmCourse.Range := Range;
		prmCourse.EndUpdate;
	end;}
end;

procedure TMainForm.SetNotAlignedAnchorPoint(AnchorPoint: TPoint; Index: Integer);
var
	Count, I, gr:	Integer;
	Interval:		TInterval;
	IntervalList:	TIntervalList;
	LocalAnchor:	TPoint;
//	Line1, Line2:	TLine;

	fTmp, d, Value,
	dAngL, dAngR:	Double;
begin
	FNotAlignedAnchorPoint1.Assign(AnchorPoint);

	ComboTrackIntervals.Clear;
	LocalAnchor := 	PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, AnchorPoint);
	ComboTrackIntervals.Visible := True;

	if Abs(LocalAnchor.Y) < EpsilonDistance then //not FExistingAnchorPoint
	begin
		FNotAlignedTrackIntervals[0].Left := Modulus(Ceil(FRWYDirection.TrueBearing + RadToDeg(GPANSOPSConstants.Constant[arStrInAlignment].Value)), 360.0);
		FNotAlignedTrackIntervals[0].Right := Modulus(Floor(FRWYDirection.TrueBearing + RadToDeg(GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAirCraftCategory])), 360.0);

		FNotAlignedTrackIntervals[1].Left := Modulus(Ceil(FRWYDirection.TrueBearing - RadToDeg(GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAirCraftCategory])), 360.0);
		FNotAlignedTrackIntervals[1].Right := Modulus(Floor(FRWYDirection.TrueBearing - RadToDeg(GPANSOPSConstants.Constant[arStrInAlignment].Value)), 360.0);

		ComboTrackIntervals.Items.Add(IntToStr(Round(FNotAlignedTrackIntervals[0].Left))+ '..' +IntToStr(Round(FNotAlignedTrackIntervals[0].Right)));
		ComboTrackIntervals.Items.Add(IntToStr(Round(FNotAlignedTrackIntervals[1].Left))+ '..' +IntToStr(Round(FNotAlignedTrackIntervals[1].Right)));

		IntervalList := TIntervalList.Create(FNotAlignedTrackIntervals[0]);
		IntervalList.AddInterval(FNotAlignedTrackIntervals[1]);

		Count := FillSecondPoints(IntervalList);
	end
	else
	begin
		if Index < 0 then exit;
		IntervalList := TIntervalList(FNotAlignedSgfPointRange.Items[Index]);

//		Count := IntervalList.Count;
		for I := 0 to IntervalList.Count-1 do
		begin
			Interval := IntervalList.Interval[I];
			d := LocalAnchor.X - Interval.Left;
			if Abs(d) < EpsilonDistance then
				raise EDivByZero.CreateFmt('LocalAnchor.X - Interval.Left(%d) is less than EpsilonDistance(%d)',
										  [d, EpsilonDistance]);

			dAngL := -RadToDeg(Arctan(LocalAnchor.Y/d));

			d := LocalAnchor.X - Interval.Right;
			if Abs(d) < EpsilonDistance then
				raise EDivByZero.CreateFmt('LocalAnchor.X - Interval.Right(%d) is less than EpsilonDistance(%d)',
										  [d, EpsilonDistance]);

			dAngR := -RadToDeg(Arctan(LocalAnchor.Y/d));
			if dAngL > dAngR then
			begin
				fTmp := dAngR;
				dAngR := dAngL;
				dAngL := fTmp;
			end;

			FNotAlignedTrackIntervals[I].Left := Modulus(Ceil(FRWYDirection.TrueBearing + dAngL), 360.0);
			FNotAlignedTrackIntervals[I].Right := Modulus(Floor(FRWYDirection.TrueBearing + dAngR), 360.0);

			if FNotAlignedTrackIntervals[I].Left = FNotAlignedTrackIntervals[I].Right then
				ComboTrackIntervals.Items.Add(FloatToStr(FNotAlignedTrackIntervals[I].Left))
			else if Modulus(FNotAlignedTrackIntervals[I].Right - FNotAlignedTrackIntervals[I].Left, 360.0) > 90.0 then
			begin
				gr := Floor(Log10(dAngR - dAngL));
				Value := RoundTo(FRWYDirection.TrueBearing + 0.5*(dAngR + dAngL), gr);

				FNotAlignedTrackIntervals[I].Left := Value;
				FNotAlignedTrackIntervals[I].Right := Value;

				ComboTrackIntervals.Items.Add(FloatToStr(Value));
				ComboTrackIntervals.Visible := False;
			end
			else
				ComboTrackIntervals.Items.Add(IntToStr(Round(FNotAlignedTrackIntervals[I].Left)) +
					'..' + IntToStr(Round(FNotAlignedTrackIntervals[I].Right)));
		end;

		Count := FillSecondPoints(IntervalList);
	end;
//	ComboTrackIntervals.Visible := Count > 1;
//	if Count > 1 then

	ComboTrackIntervals.ItemIndex := 0;

	Interval.Left := CConverters[puAngle].ConvertFunction(FNotAlignedTrackIntervals[0].Right, cdToInner, prmCourse.InConvertionPoint);
	Interval.Right := CConverters[puAngle].ConvertFunction(FNotAlignedTrackIntervals[0].Left, cdToInner, prmCourse.InConvertionPoint);

	SetTrackInterval(FNotAlignedTrackIntervals[0]);

	if Count > 0 then
	begin
		rgChooseTrack.Enabled := True;
		cbSecondAnchorPoint.ItemIndex := 0;
		cbSecondAnchorPointChange(cbSecondAnchorPoint)
	end
	else
	begin
		rgChooseTrack.Enabled := False;
		rgChooseTrack.ItemIndex := 0;
	end;

	LocalAnchor.Free;
{
	if rg0203.ItemIndex <> 0 then
		cbSecondAnchorPointChange(cbSecondAnchorPoint)
	else
		prmCourse.OnChangeValue(prmCourse);
//		prmCourse.ReadFromControl
//		SetDirection(StrToFloat(edCourse.Text))
}
end;

procedure TMainForm.SetAlignedAnchorPoint(AnchorPoint: TPoint);
var
	fTmp, Inter1,
	MinAngle, dist,
	MaxAngle,
	Inter2, Rad5,
	OldCourse, Val:		Double;
	HasSolution:		Boolean;

	//MinRounded,
	//MaxRounded,
	//gr:				Integer;
	pt1400,
	LocalPoint:			TPoint;

	Line1, Line2:		TLine;
	ptInter1,
	ptInter2:			TGeometry;
	tmpIntervalList:	TIntervalList;

	Range:				TInterval;
begin
	if Assigned(FAlignedAnchorPoint1) then
		dist := Hypot(FAlignedAnchorPoint1.Y-AnchorPoint.Y, FAlignedAnchorPoint1.X-AnchorPoint.X)
	else
		dist := 10000000.0;

	FAlignedAnchorPoint1.Assign(AnchorPoint);
	Rad5 := GPANSOPSConstants.Constant[arStrInAlignment].Value;

	LocalPoint := PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, FAlignedAnchorPoint1);
	pt1400 := TPoint.Create(GPANSOPSConstants.Constant[arMinInterDist].Value, 0);

	Line1 := TLine.Create(pt1400, 0.5*PI);
	Line2 := TLine.Create(LocalPoint, Rad5);

	ptInter1 := LineLineIntersect(Line1, Line2);
	Inter1 := 0.0;
	if ptInter1.GeometryType = gtPoint then		Inter1 := RoundTo(ptInter1.AsPoint.Y, -3);

	Line2.DirVector.Direction := -Rad5;
	ptInter2 := LineLineIntersect(Line1, Line2);
	Inter2 := 0.0;
	if ptInter2.GeometryType = gtPoint then		Inter2 := RoundTo(ptInter2.AsPoint.Y, -3);

	Line1.Free;
	Line2.Free;

	if Inter1 > Inter2 then
	begin
		fTmp := Inter2;	Inter2 := Inter1;	Inter1 := fTmp;
	end;

	HasSolution := ( Inter2 >= -GPANSOPSConstants.Constant[arMinInterToler].Value) and
					(Inter1 <= GPANSOPSConstants.Constant[arMinInterToler].Value);

	if HasSolution then
	begin
		if Inter1 < -GPANSOPSConstants.Constant[arMinInterToler].Value then
			Inter1 := -GPANSOPSConstants.Constant[arMinInterToler].Value;

		if Inter2 > GPANSOPSConstants.Constant[arMinInterToler].Value then
			Inter2 := GPANSOPSConstants.Constant[arMinInterToler].Value;

		if Abs(pt1400.X - LocalPoint.X) < EpsilonDistance then
		begin
			MinAngle :=  -Rad5;
			MaxAngle :=  Rad5;
		end
		else
		begin
			MinAngle :=  ArcTan((LocalPoint.Y - Inter1)/(LocalPoint.X - pt1400.X));
			MaxAngle :=  ArcTan((LocalPoint.Y - Inter2)/(LocalPoint.X - pt1400.X));

			if MinAngle > MaxAngle then
			begin
				fTmp := MaxAngle;	MaxAngle := MinAngle;	MinAngle := fTmp;
			end;
		end;

		fTmp		:= DirToAzimuth(FRWYDirection.Prj, fRWYCLDir + MinAngle, GPrjSR, GGeoSR);
		MinAngle	:= DirToAzimuth(FRWYDirection.Prj, fRWYCLDir + MaxAngle, GPrjSR, GGeoSR);
		MaxAngle	:= fTmp;

		{MinRounded := Round(MinAngle + 0.49999);
		MaxRounded := Round(MaxAngle - 0.49999);

		if MinRounded >= MaxRounded then
		begin
			if MinRounded = MaxRounded then
			begin
				Range.Left := CConverters[puAngle].ConvertFunction(MaxRounded, cdToInner, prmCourse.InConvertionPoint);
				Range.Right := CConverters[puAngle].ConvertFunction(MinRounded, cdToInner, prmCourse.InConvertionPoint);
				Val := AztToDirection(FRWYDirection.Geo, MaxRounded, GGeoSR, GPrjSR);
			end
			else
			begin
				gr := Round(Log10(MaxAngle - MinAngle) - 0.49999);
				Val := AztToDirection(FRWYDirection.Geo, 0.5*(MaxAngle + MinAngle), GGeoSR, GPrjSR);
				Range.Left := Val;
				Range.Left := Val;
				MinRounded := MaxRounded;
			end;
		end}

		if MinAngle >= MaxAngle then
		begin
			if MinAngle = MaxAngle then
			begin
				Range.Left := CConverters[puAngle].ConvertFunction(MaxAngle, cdToInner, prmCourse.InConvertionPoint);
				Range.Right := CConverters[puAngle].ConvertFunction(MinAngle, cdToInner, prmCourse.InConvertionPoint);
				Val := AztToDirection(FRWYDirection.Geo, MaxAngle, GGeoSR, GPrjSR);
			end
			else
			begin
				Val := AztToDirection(FRWYDirection.Geo, 0.5*(MaxAngle + MinAngle), GGeoSR, GPrjSR);
				Range.Left := Val;
				Range.Left := Val;
				//MinAngle := MaxAngle;
			end;
		end
		else
		begin
			Range.Left := CConverters[puAngle].ConvertFunction(MaxAngle, cdToInner, prmCourse.InConvertionPoint);
			Range.Right := CConverters[puAngle].ConvertFunction(MinAngle, cdToInner, prmCourse.InConvertionPoint);
			Val := AztToDirection(FRWYDirection.Geo, 0.5*(MaxAngle + MinAngle), GGeoSR, GPrjSR);
		end;

		//udApproacCourse.Min := MinRounded;
		//udApproacCourse.Max := MaxRounded;

		FAlignedTrackInterval := Range;
		OldCourse := prmCourse.Value;
		prmCourse.BeginUpdate;

		prmCourse.Range := Range;
		prmCourse.Value := Val;

		tmpIntervalList := TIntervalList.Create(FAlignedTrackInterval);

		if FillSecondPoints(tmpIntervalList) > 0 then
		begin
			rgChooseTrack.Enabled := True;
			cbSecondAnchorPoint.ItemIndex := 0;
			cbSecondAnchorPointChange(cbSecondAnchorPoint)
		end
		else
		begin
			rgChooseTrack.Enabled := False;
			rgChooseTrack.ItemIndex := 0;
		end;

		tmpIntervalList.Free;
		prmCourse.EndUpdate;
		if (OldCourse = prmCourse.Value) and(dist>EpsilonDistance) then
			OnCourseChangeValue(prmCourse);
	end;

	LocalPoint.Free;
	ptInter1.Free;
	ptInter2.Free;
	pt1400.Free;
end;

function TMainForm.CreateMAPt(fTrackDir: Double; AnchorPoint: TPoint; Aligned: Boolean): Boolean;
var
	Line1, Line2: 		TLine;
	Geometry:			TGeometry;
begin
	FFarMAPt.Free;
	FFarMAPt := nil;

	Line1 := TLine.Create(AnchorPoint, fTrackDir);

	if Aligned then
		Line2 := TLine.Create(FRWYDirection.Prj, fTrackDir + 0.5 * PI)
	else
		Line2 := TLine.Create(FRWYDirection.Prj, fRWYCLDir);

	Geometry := LineLineIntersect(Line1, Line2);
	Line1.Free;
	Line2.Free;

	result := Geometry.GeometryType = gtPoint;
	if result then		FFarMAPt := Geometry.AsPoint
	else				Geometry.Free;
end;

procedure TMainForm.TurnOnMAPt;
var
//	TriggerDistance,
	DistTPFromARP,
	LatestDist:			Double;
	tmpPt:				TPoint;
	FullPoly,
	PrimaryPoly:		TPolygon;
	N:					Integer;
begin
	if cbTurnAt.ItemIndex = 0 then
		LatestDist := FMapt.SOCDistance
	else
		LatestDist := FMapt.SOCDistance + (prmPreTurnAltitude.Value - prmOCA.Value)/FMATF.Gradient;

	tmpPt := LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, LatestDist, 0.0);
	FMATF.PrjPt				:= tmpPt;
	tmpPt.Free;

	FMATF.Altitude			:= prmPreTurnAltitude.Value;
	FMATF.TurnDistance		:= LatestDist;
	FMATF.TurnAt			:= TTurnAt(cbTurnAt.ItemIndex);
	FMATF.FlyPath			:= TFlyPath(rgToFIX.ItemIndex);
//	FMATF.SensorType		:= FMAPt.SensorType;		//TSensorType(rgMAPtSensorType.ItemIndex);
	FMATF.IAS				:= prmMAIAS.Value;

	prmTurnFromMAPtDist.ChangeValue(LatestDist);
//	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.InConvertionPoint := FMATF.GeoPt.Clone;
	prmTurnCourse.OutConvertionPoint := FMATF.PrjPt.Clone;
	prmTurnCourse.EndUpdate;
//	prmTurnCourse.Active := True;

	prmTurn1.BeginUpdate;
	prmTurn2.BeginUpdate;
	prmTurn3.BeginUpdate;

	prmTurn1.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn2.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn3.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;

	prmTurn1.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn2.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn3.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;

	prmTurn1.EndUpdate;
	prmTurn2.EndUpdate;
	prmTurn3.EndUpdate;

//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
//	TriggerDistance := PBNInternalTriggerDistance;

	DistTPFromARP := Hypot(FMATF.PrjPt.X - FAerodrome.Prj.X, FMATF.PrjPt.Y - FAerodrome.Prj.Y);

	if DistTPFromARP >= PBNTerminalTriggerDistance		then	FMATF.Role := MATF_GT_56
	else if DistTPFromARP >= PBNInternalTriggerDistance	then	FMATF.Role := PBN_MATF_GE_28
	else														FMATF.Role := PBN_MATF_LT_28;

	CreateMATIASegment(0, FMATF);

//==============================================================================

	FullPoly := SMATFullPolyList.Items[0];
	PrimaryPoly := SMATPrimaryPolyList.Items[0];

	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);

	FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
	FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);

	FMAPt.StraightArea := PrimaryPoly;
	FMATF.ReCreateArea;

	FMAPt.RefreshGraphics;
	FMATF.RefreshGraphics;

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then	rbCreateNew.Checked := True;

	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
	else								prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.TurnOnTP;
var
//	TriggerDistance,
	DistTPFromARP,
	LatestDist:			Double;
	tmpPt:				TPoint;
	FullPoly,
	PrimaryPoly:		TPolygon;
	N:					Integer;
begin
	LatestDist := prmTurnFromMAPtDist.Value;

	tmpPt := LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, LatestDist, 0.0);
	FMATF.PrjPt				:= tmpPt;
	tmpPt.Free;

	FMATF.TurnDistance		:= LatestDist;
	FMATF.TurnAt			:= TTurnAt(cbTurnAt.ItemIndex);
	FMATF.FlyPath			:= TFlyPath(rgToFIX.ItemIndex);
//	FMATF.SensorType		:= TSensorType(rgMAPtSensorType.ItemIndex);
	FMATF.IAS				:= prmMAIAS.Value;

	FMATF.Altitude			:= prmPreTurnAltitude.Value;

//	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.InConvertionPoint := FMATF.GeoPt.Clone;
	prmTurnCourse.OutConvertionPoint := FMATF.PrjPt.Clone;
	prmTurnCourse.EndUpdate;
//	prmTurnCourse.Active := True;

	prmTurn1.BeginUpdate;
	prmTurn2.BeginUpdate;
	prmTurn3.BeginUpdate;

	prmTurn1.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn2.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;
	prmTurn3.InConvertionPoint := TPoint(prmTurnCourse.InConvertionPoint).Clone;

	prmTurn1.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn2.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;
	prmTurn3.OutConvertionPoint := TPoint(prmTurnCourse.OutConvertionPoint).Clone;

	prmTurn1.EndUpdate;
	prmTurn2.EndUpdate;
	prmTurn3.EndUpdate;

//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
//	TriggerDistance := PBNTerminalTriggerDistance;

	DistTPFromARP := Hypot(FMATF.PrjPt.X - FAerodrome.Prj.X, FMATF.PrjPt.Y - FAerodrome.Prj.Y);

	if DistTPFromARP >= PBNTerminalTriggerDistance		then	FMATF.Role := MATF_GT_56
	else if DistTPFromARP >= PBNInternalTriggerDistance	then	FMATF.Role := PBN_MATF_GE_28
	else														FMATF.Role := PBN_MATF_LT_28;

	CreateMATIASegment(0, FMATF);

	FullPoly := SMATFullPolyList.Items[0];
	PrimaryPoly := SMATPrimaryPolyList.Items[0];

	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);

	FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
	FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);

	FMAPt.StraightArea := PrimaryPoly;
	FMATF.ReCreateArea;

	FMAPt.RefreshGraphics;
	FMATF.RefreshGraphics;

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then
		rbCreateNew.Checked := True;

	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
	else								prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.GetTurnObstacles;
var
	Dist, fGradient,
	fMOC30, fMOC50,
	FAF2THRDist,
	DistKKToSOC,
	AddMOC, FullFAMOC,
	TIAMOC,
	fMOC, fTmp, X:		Double;
	Point:				TPoint;
	Left,
	FullPoly,
	PrimaryPoly,

	TIAFullPoly,
	TIAPrimaryPoly:	TPolygon;

	Part:				TPart;
	CutterPline:		TPolyline;
	I, N, K,
	IxTIA, IxTA:		Integer;
	ObstacleInfo:		TObstacleInfo;
begin
	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then exit;
	fGradient := GrTable[K];

	FullPoly := SMATFullPolyList.Items[0];
	PrimaryPoly := SMATPrimaryPolyList.Items[0];

	CutterPline := TPolyline.Create;
	Part := TPart.Create;
	Point := TPoint.Create;
	X := 0;
	if FMATF.TurnAt = taTP then
		X := -FMATF.EPT;

	LocalToPrj(FMATF.PrjPt, FMATF.EntryDirection, X, 100000.0, Point);
	Part.AddPoint(Point);

	LocalToPrj(FMATF.PrjPt, FMATF.EntryDirection, X, -100000.0, Point);
	Part.AddPoint(Point);
	Point.Free;

	CutterPline.AddPart(Part);
	Part.Free;
//	GGeoOperators.Cut(FullPoly, CutterPline, TGeometry(Left), TGeometry(TIAFullPoly));

try
	GGeoOperators.Cut(FullPoly, CutterPline, TGeometry(Left), TGeometry(TIAFullPoly));
except
	GUI.DrawPolygon(FullPoly, 255, sfsSolid);
	GUI.DrawPolyline(CutterPline, 0, 2);
end;

	Left.Free;

	GGeoOperators.Cut(PrimaryPoly, CutterPline, TGeometry(Left), TGeometry(TIAPrimaryPoly));
	Left.Free;
	CutterPline.Free;
//===========================================
	if Assigned(MATAObstacleList) then
	begin
		for I := 0 to MATAObstacleList.Count - 1 do
			TObstacleInfo(MATAObstacleList.Items[I]).Free;
		MATAObstacleList.Free;
	end;

	if Assigned(TIAObstacleList) then
	begin
		for I := 0 to TIAObstacleList.Count - 1 do
			TObstacleInfo(TIAObstacleList.Items[I]).Free;
		TIAObstacleList.Free;
	end;

	if FMATF.TurnAt = taTP then	Point := TPolyLine(FMATF.ReferenceGeometry).Part[0].Point[0]
	else						Point := FMATF.PrjPt;

	DistKKToSOC := PointToLineDistance(Point, FSOC.PrjPt, prmCourse.Value - 0.5 * PI);
	fMOC30 := GPANSOPSConstants.Constant[arMA_InterMOC].Value;
	fMOC50 := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;

	if FMATF.TurnAngle <= GPANSOPSConstants.Constant[arMATurnTrshAngl].Value then
		fMOC := fMOC30
	else
		fMOC := fMOC50;

	if cbTurnAt.ItemIndex = 2 then
		TIAMOC := fMOC30
	else
		TIAMOC := fMOC50;

	FAF2THRDist := Hypot(FFAF.PrjPt.X - FFarMAPt.X , FFAF.PrjPt.Y - FFarMAPt.Y);
	if not FAligned then
		FAF2THRDist := FAF2THRDist +
			Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

	Dist := FAF2THRDist - GPANSOPSConstants.Constant[arMOCChangeDist].Value;
	AddMOC := (Dist * GPANSOPSConstants.Constant[arAddMOCCoef].Value) * Integer(Dist > 0);
	FullFAMOC := FlightPhases[fpFinalApproach].MOC + AddMOC;

	TIAObstacleList := GetObstacleInfoList(TIAPrimaryPoly, TIAFullPoly, TIAMOC, FObstalces);
	MATAObstacleList := GetObstacleInfoList(primeTAPoly, secondTAPoly, fMOC, FObstalces);

	N := TIAObstacleList.Count;

	FMATIAOCA := prmPreOCA.Value;
	IxTIA := -1;

	for I := 0 to N - 1 do
	begin
		ObstacleInfo := TObstacleInfo(TIAObstacleList.Items[I]);
		Dist := PointToLineDistance(ObstacleInfo.ptPrj, FSOC.PrjPt, prmCourse.Value - 0.5 * PI);
		ObstacleInfo.Dist := Dist;

		ObstacleInfo.ReqOCA := ObstacleInfo.Elevation  + ObstacleInfo.fTmp * fMOC30 - Dist * fGradient;
		fTmp := ObstacleInfo.Elevation + ObstacleInfo.fTmp * FullFAMOC;

		if (ObstacleInfo.Dist < 0) and (fTmp < ObstacleInfo.ReqOCA) then
			ObstacleInfo.ReqOCA := fTmp;

//		ObstacleInfo.ReqOCA := ObstacleInfo.Elevation +
//									Min(	ObstacleInfo.fTmp * TIAMOC - Dist * fGradient,
//											ObstacleInfo.fTmp * FullFAMOC);

		if ObstacleInfo.ReqOCA > FMATIAOCA then
		begin
			FMATIAOCA := ObstacleInfo.ReqOCA;
			IxTIA := I;
		end;
	end;

	FMATAOCA := prmPreOCA.Value;
	FMAFinalOCA := prmPreOCA.Value;
	IxTA := -1;

	N := MATAObstacleList.Count;
	for I := 0 to N - 1 do
	begin
		ObstacleInfo := TObstacleInfo(MATAObstacleList.Items[i]);
		Dist := GGeoOperators.getDistance(ObstacleInfo.PtPrj, FMATF.ReferenceGeometry);
		ObstacleInfo.Dist := Dist;

		ObstacleInfo.ReqOCA := ObstacleInfo.Elevation + ObstacleInfo.fTmp * fMOC -
								(DistKKToSOC + Dist) * fGradient;
		if ObstacleInfo.ReqOCA > FMATAOCA then
		begin
			FMATAOCA := ObstacleInfo.ReqOCA;
			IxTA := I;
		end;
	end;

	FMATIAOCA_CO := '';

	if FMATAOCA > FMATIAOCA then
	begin
		FMAFinalOCA := FMATAOCA;
		FMATIAOCA_CO := TObstacleInfo(MATAObstacleList.Items[IxTA]).AID;
	end
	else if FMATIAOCA > prmPreOCA.Value then
	begin
		FMAFinalOCA := FMATIAOCA;
		FMATIAOCA_CO := TObstacleInfo(TIAObstacleList.Items[IxTIA]).AID;
	end;

	fTmp := CConverters[puAltitude].ConvertFunction (FMAFinalOCA, cdToOuter, nil);
	editFinalOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := FMAFinalOCA + DistKKToSOC * fGradient;

	prmPreTurnAltitude.ChangeValue(prmOCA.Value + DistKKToSOC * fGradient, False);
	fTmp := CConverters[puAltitude].ConvertFunction (fTmp, cdToOuter, nil);
	editFinalTurnAltitude.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	editSignificiantObstacle.Text := FMATIAOCA_CO;

	ReportForm.AddMIAS_TIA(TIAObstacleList, 'MA straight segment/TIA');
	ReportForm.AddMITurn(MATAObstacleList);

//===========================================
	TIAFullPoly.Free;
	TIAPrimaryPoly.Free;
end;

procedure TMainForm.ReCreateMATurnArea;
var
	ptDst:		TPoint;
	K, N:		Integer;
//	TriggerDistance,
	DistMAHFFromARP, Dir:	Double;
	ptTmp0,
	ptTmp1,
	ptTmp2:				TPoint;

	CutterPart:			TPart;
	CutterPline:		TPolyline;

	pLeft, pRight,
	PrimaryPoly,
	FullPoly:			TPolygon;
begin
	ptDst := TPoint.Create;

//	CalcFromMinStablizationDistance
// cbTurnWPT

	if bTurnExistingPoint then
	begin
		K := cbTurnWPT.ItemIndex;
		if K < 0 then exit;
		ptDst.Assign(TSignificantPoint(cbTurnWPT.Items.Objects[K]).Prj);
	end
	else
		LocalToPrj(FMATF.PrjPt, prmTurnCourse.Value, prmNWPTFromTPDist.Value, 0, ptDst);
	Dir := prmTurnCourse.Value;			//ArcTan2(ptDst.Y - FMATF.PrjPt.Y, ptDst.X - FMATF.PrjPt.X);

	FMAHF.PrjPt := ptDst;
	FMAHF.EntryDirection := Dir;

	DistMAHFFromARP := Hypot(ptDst.X - FAerodrome.Prj.X, ptDst.Y - FAerodrome.Prj.Y);
	ptDst.Free;


//	if FMAHF.SensorType = stGNSS then	TriggerDistance := GNSSTriggerDistance
//	else								TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
//	TriggerDistance := PBNTerminalTriggerDistance;
//????????????????????????????????????????????????
{	if rbIAPCH_DME_DME.Checked then
		FMAHF.SensorType := stDME_DME
	else
		FMAHF.SensorType := stGNSS;
}
//????????????????????????????????????????????????

	if rgMAPtSensorType.ItemIndex = 0 then
		FMAHF.SensorType := stGNSS
	else
		FMAHF.SensorType := stDME_DME;

	FMATF.OutDirection := Dir;
	FMATF.FlyPath := TFlyPath(rgToFIX.ItemIndex);

	if DistMAHFFromARP >= PBNTerminalTriggerDistance then	FMAHF.Role := MAHF_GT_56
	else													FMAHF.Role := MAHF_LE_56;

	GUI.SafeDeleteGraphic(kkLineElem);
	GUI.SafeDeleteGraphic(secondPolyEl);
	GUI.SafeDeleteGraphic(primePolyEl);

	FreeObject(TObject(FMAPtLeg));
	FreeObject(TObject(FMATFLeg));

	N := TList(FIABranchs.Items[0]).Count;

	FMAPtLeg := TLeg.Create(FMApt, FMATF, fpIntermediateMissedApproach, GUI, TLeg(TList(FIABranchs.Items[0]).Items[N - 1]));
	CreateMATIASegment(0, FMATF);

	if cbTurnAt.ItemIndex = 2 then
	begin
		ptTmp2 := LocalToPrj(FMATF.PrjPt, prmCourse.Value, -FMATF.EPT, 0);
		ptTmp0 := LocalToPrj(ptTmp2, prmCourse.Value, 0, 100000);
		ptTmp1 := LocalToPrj(ptTmp2, prmCourse.Value, 0, -100000);

		CutterPart := TPart.Create;
		CutterPart.AddPoint(ptTmp0);
		CutterPart.AddPoint(ptTmp1);

		CutterPline := TPolyline.Create;
		CutterPline.AddPart(CutterPart);
		ptTmp2.Free;
		ptTmp0.Free;
		ptTmp1.Free;
		CutterPart.Free;

		GGeoOperators.Cut(TPolygon(SMATFullPolyList.Items[0]), CutterPline, TGeometry(pLeft), TGeometry(pRight));
		FMATF.SecondaryTolerArea.Assign(pLeft);
		pLeft.Free;
		pRight.Free;

		GGeoOperators.Cut(TPolygon(SMATPrimaryPolyList.Items[0]), CutterPline, TGeometry(pLeft), TGeometry(pRight));
		FMATF.TolerArea.Assign(pLeft);
		pLeft.Free;
		pRight.Free;
		CutterPline.Free;
	end;

	FMATFLeg := TLeg.Create(FMATF, FMAHF, fpFinalMissedApproach, GUI, FMAPtLeg);

	if FMATF.FlyPath = fpDirectToFIX then
	begin
		Dir := CalcDFDirection(FMATF, FMAHF);

		if Dir > 1000.0 then
		begin
			MessageDlg('Next WPT situated too close to turn point.', mtConfirmation, [mbOK], 0);
			exit;
		end;

		prmTurn1.ChangeValue(Dir);
		prmTurn2.ChangeValue(CalcDRNomDir(FMATF, FMAHF));
		prmTurn3.ChangeValue(FMATF.OutDirection2);
	end
	else
	begin
		prmTurn2.ChangeValue(Dir);
	end;

	PrimaryPoly := CreateMATurnArea(FMATF, FMAHF, Dir, True, FMATF.FlyPath = fpDirectToFIX);
	FullPoly := CreateMATurnArea(FMATF, FMAHF, Dir, False, FMATF.FlyPath = fpDirectToFIX);

	primeTAPoly := GGeoOperators.UnionGeometry(FMATF.TolerArea, PrimaryPoly).AsPolygon;
	secondTAPoly := GGeoOperators.UnionGeometry(FMATF.SecondaryTolerArea, FullPoly).AsPolygon;

	FullPoly.Free;
	PrimaryPoly.Free;

	secondPolyEl := GUI.DrawPolygon(secondTAPoly, RGB(0, 255, 255), sfsNull);
	primePolyEl := GUI.DrawPolygon(primeTAPoly, RGB(168, 168, 0), sfsNull);

	if Assigned(FMATF.ReferenceGeometry) then
		if FMATF.ReferenceGeometry.GeometryType = gtPolyline then
			kkLineElem := GUI.DrawPolyline(FMATF.ReferenceGeometry.AsPolyline, RGB(255, 0, 255), 2)
		else
			kkLineElem := GUI.DrawPolygon(FMATF.ReferenceGeometry.AsPolygon, RGB(255, 0, 255), sfsNull);

	FullPoly := SMATFullPolyList.Items[0];
	PrimaryPoly := SMATPrimaryPolyList.Items[0];
	FMAPt.StraightArea := PrimaryPoly;

	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);

	FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
	FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);

	FMAPtLeg.RefreshGraphics;
	FMATFLeg.RefreshGraphics;
	FMAPt.RefreshGraphics;
	FMATF.RefreshGraphics;
	FMAHF.RefreshGraphics;

	GetTurnObstacles;
end;

procedure TMainForm.rgAlignmentClick(Sender: TObject);
begin
	SetAligned(rgAlignment.ItemIndex = 0);
end;

procedure TMainForm.rgSelectFirstAncorPointClick(Sender: TObject);
var
	Existing:	Boolean;
begin
	Existing := rgSelectFirstAncorPoint.ItemIndex = 1;

	if FAligned then
	begin
		FAlignedExistingAnchorPoint := Existing;
		prmAbeam.ReadOnly := Existing;
		prmAbeam.Active := not Existing;
	end
	else
	begin
		FNotAlignedExistingAnchorPoint := Existing;
		prmAlong.ReadOnly := Existing;
		prmAlong.Active := not Existing;
	end;

	cbFirstAnchorPoint.Enabled := Existing;
	SetControlColor (cbFirstAnchorPoint);

	if Existing then
		cbFirstAnchorPointChange(cbFirstAnchorPoint)
	else
	begin
		SetAligned(FAligned);
	end;
end;

procedure TMainForm.rgChooseTrackClick(Sender: TObject);
var
	Existing:		Boolean;
begin
	Existing := rgChooseTrack.ItemIndex = 1;

	cbSecondAnchorPoint.Enabled := Existing;
	SetControlColor (cbSecondAnchorPoint);
	prmCourse.ReadOnly := Existing;
	ComboTrackIntervals.Enabled := not Existing;

	if not Existing then
	begin
		if FAligned then
		begin
			prmCourse.BeginUpdate;
			prmCourse.Range := FAlignedTrackInterval;
			prmCourse.EndUpdate;
		end
		else
		begin
			if rgSelectFirstAncorPoint.ItemIndex = 0 then
				OnAlongChangeValue(prmAlong)
			else
				SetNotAlignedAnchorPoint(FNotAlignedAnchorPoint1, -1);

//			prmCourse.BeginUpdate;
//			prmCourse.Value := FNotAlignedApproachDir;
//			prmCourse.EndUpdate;

{			if ComboTrackIntervals.ItemIndex >= 0 then
				SetTrackInterval(FNotAlignedTrackIntervals[ComboTrackIntervals.ItemIndex])
			else if ComboTrackIntervals.Items.Count > 0 then
				ComboTrackIntervals.ItemIndex := 0
			else
				SetTrackInterval(FNotAlignedTrackIntervals[0]);
}
		end
	end
	else
		cbSecondAnchorPointChange(cbSecondAnchorPoint);
end;

procedure TMainForm.cbSecondAnchorPointChange(Sender: TObject);
var
	K:			Integer;
	sigPoint2:	TSignificantPoint;
	fTrackDir:	Double;
begin
	K := cbSecondAnchorPoint.ItemIndex;
	if K < 0 then exit;

	sigPoint2 := TSignificantPoint(cbSecondAnchorPoint.Items.Objects[K]);
	Label5.Caption := AIXMTypeNames[sigPoint2.AIXMType];
	if rgChooseTrack.ItemIndex <> 1 then exit;

	if FAligned then
		fTrackDir := ReturnAngleInRadians(FAlignedAnchorPoint1, sigPoint2.Prj)
	else
		fTrackDir := ReturnAngleInRadians(FNotAlignedAnchorPoint1, sigPoint2.Prj);

	if Modulus(Abs(fTrackDir - fRWYCLDir), 2.0 * PI) > 0.5 * PI then
		fTrackDir := Modulus(fTrackDir + PI, 2.0 * PI);

	prmCourse.ChangeValue(fTrackDir);
end;

procedure TMainForm.cbFirstAnchorPointChange(Sender: TObject);
var
	sigPoint:		TSignificantPoint;
begin
	if cbFirstAnchorPoint.ItemIndex < 0 then exit;
	sigPoint := TSignificantPoint(cbFirstAnchorPoint.Items.Objects [cbFirstAnchorPoint.ItemIndex]);
	labStraightSgfPointType.Caption := AIXMTypeNames[sigPoint.AIXMType];
	if rgSelectFirstAncorPoint.ItemIndex <> 1 then	exit;

	if FAligned then
	begin
		SetAlignedAnchorPoint(sigPoint.Prj)
	end
	else
	begin
		SetNotAlignedAnchorPoint(sigPoint.Prj, cbFirstAnchorPoint.ItemIndex);
	end;
end;

procedure TMainForm.ComboTrackIntervalsChange(Sender: TObject);
begin
	if ComboTrackIntervals.ItemIndex < 0 then exit;
	SetTrackInterval(FNotAlignedTrackIntervals[ComboTrackIntervals.ItemIndex]);
end;

//======================================================================= Page 3
procedure TMainForm.SetMAPt(NewMAPt: TPoint);
var
	I, N:				Integer;
	MinDist, MaxDist:	Double;
	LocalPoint:			TPoint;
	sigPoint:			TSignificantPoint;
	Axis:				TLine;
begin
	FMAPt.PrjPt := NewMAPt;

	cbFAFList.Clear;
	LocalPoint := TPoint.Create;
	Axis := TLine.Create(FMAPt.PrjPt, F3ApproachDir + PI);

	MinDist := GPANSOPSConstants.Constant[arMinRangeFAS].Value;
	MaxDist := GPANSOPSConstants.Constant[arMaxRangeFAS].Value;

	N := FSignificantCollection.Count;
	for I := 0 to N-1 do
	begin
		sigPoint := FSignificantCollection.Item [i].asSignificantPoint;
		PrjToLocal(Axis, sigPoint.Prj, LocalPoint);
		if (LocalPoint.X >= MinDist) and (LocalPoint.X <= MaxDist) and
			(Abs(LocalPoint.Y) <= 0.25*GGNSSConstants.Constant[FAF_].XTT) then
			cbFAFList.AddItem(sigPoint.AixmID, sigPoint);
	end;

	rbFAFFromList.Enabled := cbFAFList.Items.Count > 0;

	if cbFAFList.Items.Count > 0 then
		cbFAFList.ItemIndex := 0
	else
		rbFAFByDist.Checked := True;

	FMAPt.RefreshGraphics;
	LocalPoint.Free;
	Axis.Free;
end;

procedure TMainForm.rbMAPtClick(Sender: TObject);
var
	bFromList:		Boolean;
begin
	bFromList := TRadioButton(Sender).Tag = 1;

//	labMAPtDistance.Visible := not bFromList;
	editMAPtDistance.Visible := not bFromList;
	labMAPtDistanceUnit.Visible := not bFromList;

	cbMAPtList.Enabled := bFromList;
	Label0201.Enabled := bFromList;

	if bFromList then
	begin
	end
	else
	begin
	end;
end;

procedure TMainForm.rbFAFClick(Sender: TObject);
var
	bFromList:		Boolean;
begin
	bFromList := TRadioButton(Sender).Tag = 1;

	prmFAFAltitude.ReadOnly := bFromList;

	cbFAFList.Enabled := bFromList;
	Label0202.Enabled := bFromList;

	if bFromList then
		cbFAFListChange(cbFAFList)
	else
	begin

	end;
end;

procedure TMainForm.rbIFClick(Sender: TObject);
var
	bFromList:		Boolean;
begin
	bFromList := TRadioButton(Sender).Tag = 1;

	prmIFDistance.ReadOnly := bFromList;
	prmIFCourse.ReadOnly := bFromList;

	cbIFList.Enabled := bFromList;
	Label0203.Enabled := bFromList;

	if bFromList then
		cbIFListChange(cbIFList)
	else
	begin

	end;
end;

procedure TMainForm.cbFAFListChange(Sender: TObject);
var
	K:			Integer;
	Dist:		Double;
	sigPoint:	TSignificantPoint;
begin
	K := cbFAFList.ItemIndex;
	if K < 0 then
		exit;

	sigPoint := TSignificantPoint(cbFAFList.Items.Objects[K]);
	Label0202.Caption := AIXMTypeNames[sigPoint.AIXMType];

	if not cbFAFList.Enabled then
		exit;

//	Dist := Hypot(sigPoint.Prj.Y - FMAPt.PrjPt.Y, sigPoint.Prj.X - FMAPt.PrjPt.X);
	Dist := Abs(PointToLineDistance(FMAPt.PrjPt, sigPoint.Prj, F3ApproachDir + 0.5 * PI));
	prmFAFDistance.ChangeValue(Dist);
end;

procedure TMainForm.cbIFListChange(Sender: TObject);
var
	K:			Integer;
	Dist,
	Angle:		Double;
	sigPoint:	TSignificantPoint;
begin
	K := cbIFList.ItemIndex;
	if K < 0 then
		exit;

	sigPoint := TSignificantPoint(cbIFList.Items.Objects[K]);
	Label0203.Caption := AIXMTypeNames[sigPoint.AIXMType];

	if not cbIFList.Enabled then
		exit;

	Angle := ArcTan2(FFAF.PrjPt.Y - sigPoint.Prj.Y, FFAF.PrjPt.X - sigPoint.Prj.X);
	Dist := Hypot(FFAF.PrjPt.Y - sigPoint.Prj.Y, FFAF.PrjPt.X - sigPoint.Prj.X);

	prmIFCourse.ChangeValue(Angle);
	prmIFDistance.ChangeValue(Dist);
end;

procedure TMainForm.cbMAPtListChange(Sender: TObject);
var
	K:			Integer;
	Dist:		Double;
	sigPoint:	TSignificantPoint;
begin
	K := cbMAPtList.ItemIndex;
	if K < 0 then
		exit;

	sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[K]);
	Label0201.Caption := AIXMTypeNames[sigPoint.AIXMType];

	if not cbMAPtList.Enabled then
		exit;

//	Dist := Hypot(sigPoint.Prj.Y - FMAPt.PrjPt.Y, sigPoint.Prj.X - FMAPt.PrjPt.X);
	Dist := Abs(PointToLineDistance(FFAF.PrjPt, sigPoint.Prj, F3ApproachDir + 0.5*PI));
	prmMAPtDistance.ChangeValue(Dist);
end;

// IF ======================================================

procedure TMainForm.ApplyIFDistanceAndDirection(Dist, Dir: Double);
var
	Angle,
	fTurnAngle,
	MinStabFAF,
	MinStabIF:			Double;
	point:				TPoint;
	tmpRange:			TInterval;
begin
	point := LocalToPrj(FFAF.PrjPt, Dir + PI, Dist, 0.0);
	FIF.PrjPt := point;
	point.Free;

	Parameter1.BeginUpdate(False);
	Parameter1.InConvertionPoint := FIF.GeoPt.Clone;
	Parameter1.OutConvertionPoint := FIF.PrjPt.Clone;
	Parameter1.EndUpdate;

	Angle := ArcTan2(FFAF.PrjPt.Y - FIF.PrjPt.Y, FFAF.PrjPt.X - FIF.PrjPt.X);
	Parameter1.ChangeValue(Angle, False);
//	FIF.IAS :=
//=========================================
	FFAF.TurnDirection := AngleSideDef(Dir, FFAF.OutDirection);

	FIF.OutDirection := Dir;
	FIF.EntryDirection := Dir;

	FIFInfo.Angle := Dir;
	FFAF.EntryDirection := Dir;

	fTurnAngle := Modulus(FFAF.OutDirection - FIF.OutDirection, 2*PI);
	if fTurnAngle > PI then	fTurnAngle := 2*PI - fTurnAngle;

	MinStabFAF := FFAF.CalcInToMinStablizationDistance(fTurnAngle);
	MinStabIF := FIF.CalcFromMinStablizationDistance(prmIFMaxAngle.Value);

	tmpRange := FIFInfo.DistanceParameter.Range;
	tmpRange.Left := MinStabFAF + MinStabIF + GPANSOPSConstants.Constant[rnvImMinDist].Value;

	FIFInfo.DistanceParameter.BeginUpdate;
	FIFInfo.DistanceParameter.Range := tmpRange;
	FIFInfo.DistanceParameter.EndUpdate;
//=========================================
	FIF.RefreshGraphics;
end;

procedure TMainForm.udIFCourseClick(Sender: TObject; Button: TUDBtnType);
begin
	FIFInfo.AngleParameter.BeginUpdate;
	FIFInfo.AngleParameter.Value := CConverters[puAngle].ConvertFunction(udIFCourse.Position, cdToInner, FIFInfo.AngleParameter.InConvertionPoint);
	FIFInfo.AngleParameter.EndUpdate;
end;

procedure TMainForm.editIFVelocityExit(Sender: TObject);
begin
	FFAF.IAS := FIFInfo.Speed;
end;

procedure TMainForm.btnAddBranchClick(Sender: TObject);
var
	I, J, N:	Integer;
	bInSide:		Boolean;
	PrevTurnDir:	TSideDirection;

	ObstacleList:	TList;
	BranchPoints:	TList;
	BranchLegs:		TList;

	NewIFFIX:		TFIX;
	NewFAFFIX:		TFIX;
	NewMAptFIX:		TMAPt;

	PrevLeg,
	Leg:			TLeg;
	tmpPoint:		TPoint;
	tmpPoly,
	FullPoly,
	CheckPoly,
	PrimaryPoly:	TPolygon;
	tmpRing:		TRing;
	Range, Dist0,
	Dist1:			Double;
	Obstacle:		TObstacle;
	ObstacleInfo:	TObstacleInfo;
begin
	BranchPoints := AddBranchFrame1.GetResult;
	if Assigned(BranchPoints) then
	begin
		BranchLegs := TList.Create;
		NewIFFIX := TFIX.Create(GUI);
		NewIFFIX.Assign(FIF);
		NewIFFIX.DrawingEnabled := False;
		NewIFFIX.EntryDirection := TFIX(BranchPoints.Items[BranchPoints.Count - 2]).OutDirection;

		NewFAFFIX := TFIX.Create(GUI);
		NewFAFFIX.Assign(FFAF);
		NewFAFFIX.DrawingEnabled := False;
		NewFAFFIX.EntryDirection := NewIFFIX.OutDirection;
		NewFAFFIX.IAS := NewIFFIX.IAS;

		NewMAptFIX := TMAPt.Create(GUI);
		NewMAptFIX.Assign(FMApt);
		NewMAptFIX.DrawingEnabled := False;
		NewMAptFIX.EntryDirection := NewFAFFIX.OutDirection;
		NewMAptFIX.IAS := GAircraftCategoryConstants.Constant[cVmaInter].Value[FAirCraftCategory];
{
		FCurrFIX := TFIX(BranchPoints.Items[0]);
		if FSensorType = stGNSS then
			fDistTreshol := GNSSTriggerDistance
		else if FSensorType = stSBAS then
			fDistTreshol := SBASTriggerDistance
		else
			fDistTreshol := SBASTriggerDistance + GNSSTriggerDistance;

		fDist := Hypot(FCurrFIX.PrjPt.X - FAerodrome.Prj.X, FCurrFIX.PrjPt.Y - FAerodrome.Prj.Y);
		if fDist <= fDistTreshol then	FCurrFIX.Role := IAF_LE_56_
		else							FCurrFIX.Role := IAF_GT_56_;
		FCurrFIX.RefreshGraphics;
}

		PrevLeg := nil;
		Range := 0;
		PrevTurnDir := SideOn;

		for I := 0 to BranchPoints.Count - 3 do
		begin
			TFIX(BranchPoints.Items[I]).PropagatedTurnDirection := PrevTurnDir;
			PrevTurnDir := TFIX(BranchPoints.Items[I]).EffectiveTurnDirection;

			Leg := TLeg.Create(TFIX(BranchPoints.Items[I]), TFIX(BranchPoints.Items[I+1]), fpInitialApproach, GUI, PrevLeg);
			Leg.IAS := TFIX(BranchPoints.Items[I]).IAS;
			Leg.Gradient := TFIX(BranchPoints.Items[I]).Gradient;

//if I  = BranchPoints.Count - 3 then
//	GUI.DrawPolyline(Leg.NomTrack, 0, 3);

//	Areas  ==========================================================
			Leg.CreateProtectionArea(PrevLeg, FAerodrome);
//			GUI.DrawPolyline(Leg.NNSecondLine, 255, 2);
// ==================================================================

			Dist0 := Hypot(Leg.StartFIX.PrjPt.Y - FAerodrome.Prj.Y, Leg.StartFIX.PrjPt.X - FAerodrome.Prj.X);
			if Range < Dist0 then Range := Dist0;

			BranchLegs.Add(Leg);
			PrevLeg := Leg;
		end;

		I := BranchPoints.Count - 2;
		TFIX(BranchPoints.Items[I]).PropagatedTurnDirection := PrevTurnDir;
		PrevTurnDir := TFIX(BranchPoints.Items[I]).EffectiveTurnDirection;

		Leg := TLeg.Create(TFIX(BranchPoints.Items[I]), NewIFFIX, fpInitialApproach, GUI, PrevLeg);
		Leg.IAS := TFIX(BranchPoints.Items[I]).IAS;
		Leg.Gradient := TFIX(BranchPoints.Items[I]).Gradient;
		Leg.CreateProtectionArea(PrevLeg, FAerodrome);

		BranchLegs.Add(Leg);
		PrevLeg := Leg;
//GUI.DrawPolyline(Leg.NomTrack, 0, 3);

		BranchPoints.Free;

		NewIFFIX.PropagatedTurnDirection := PrevTurnDir;
		PrevTurnDir := NewIFFIX.EffectiveTurnDirection;

		Leg := TLeg.Create(NewIFFIX, NewFAFFIX, fpIntermediateApproach, GUI, PrevLeg);
		Leg.IAS := NewIFFIX.IAS;
		Leg.Gradient := NewIFFIX.Gradient;

		Leg.CreateProtectionArea(PrevLeg, FAerodrome);
		BranchLegs.Add(Leg);
		PrevLeg := Leg;
//GUI.DrawPolyline(Leg.NomTrack, 128, 3);

		NewFAFFIX.PropagatedTurnDirection := PrevTurnDir;
//		PrevTurnDir := NewFAFFIX.EffectiveTurnDirection;

		Leg := TLeg.Create(NewFAFFIX, NewMAptFIX, fpFinalApproach, GUI, PrevLeg);
		Leg.IAS := NewFAFFIX.IAS;
		Leg.Gradient := NewFAFFIX.Gradient;
		Leg.CreateProtectionArea(PrevLeg, FAerodrome);

//GUI.DrawPolyline(Leg.NomTrack, 0, 3);

		BranchLegs.Add(Leg);
//		PrevLeg := Leg;

		FIABranchs.Add(BranchLegs);

		N := BranchLegs.Count;

		for I := N - 3 to N - 1 do
		begin
			Leg := TLeg(BranchLegs.Items[I]);
			Dist0 := Hypot(Leg.StartFIX.PrjPt.Y - FAerodrome.Prj.Y, Leg.StartFIX.PrjPt.X - FAerodrome.Prj.X);
			if Range < Dist0 then Range := Dist0;
		end;

		Range := Range + 46000.0;
		if Range > FRange then
		begin
			FRange := Range;
			FObstalces := GObjectDirectory.Obstalces[FAerodrome.Geo, Range];
		end;

		if not Assigned(ReportForm) then
			ReportForm := TReportForm.Create(self);

		FullPoly := TPolygon.Create;
		PrimaryPoly := TPolygon.Create;

		for I := 0 to N - 1 do
		begin
			Leg := TLeg(BranchLegs.Items[I]);
			Leg.RefreshGraphics;

			if I > 0 then
			begin
				tmpPoly := GGeoOperators.UnionGeometry(FullPoly, Leg.FullArea).AsPolygon;
				FullPoly.Assign(tmpPoly);
				tmpPoly.Free;

				tmpPoly := GGeoOperators.UnionGeometry(PrimaryPoly, Leg.PrimaryArea).AsPolygon;
				PrimaryPoly.Assign(tmpPoly);
				tmpPoly.Free;
			end
			else
			begin
				FullPoly.Assign(Leg.FullArea);
				PrimaryPoly.Assign(Leg.PrimaryArea);
			end;
		end;

		tmpRing := TRing.Create;

		tmpPoint := LocalToPrj(FMApt.PrjPt, FMApt.OutDirection, 0.0, 10000.0);
		tmpRing.AddPoint(tmpPoint);

		LocalToPrj(FMApt.PrjPt, FMApt.OutDirection, 10000.0, 10000.0, tmpPoint);
		tmpRing.AddPoint(tmpPoint);

		LocalToPrj(FMApt.PrjPt, FMApt.OutDirection, 10000.0, -10000.0, tmpPoint);
		tmpRing.AddPoint(tmpPoint);

		LocalToPrj(FMApt.PrjPt, FMApt.OutDirection, 0.0, -10000.0, tmpPoint);
		tmpRing.AddPoint(tmpPoint);

		tmpPoint.Free;

		tmpPoly := TPolygon.Create;
		tmpPoly.AddRing(tmpRing);
		tmpRing.Free;


		CheckPoly := GGeoOperators.UnionGeometry(FullPoly, tmpPoly).AsPolygon;
		tmpPoly.Free;
//GUI.DrawPolygon(CheckPoly, 255, sfsCross);

		FIF.RefreshGraphics;
		FFAF.RefreshGraphics;
		FMApt.RefreshGraphics;

		ObstacleList := GetObstacles(FullPoly, FObstalces);


		FullPolyList.Add(FullPoly);
		PrimaryPolyList.Add(PrimaryPoly);

//GUI.DrawPolygon(FullPoly, 255, sfsCross);
//GUI.DrawPolygon(PrimaryPoly, RGB(0, 0, 255), sfsCross);

		//for I := 0 To M - 1 do
		I := 0;
		while I < ObstacleList.Count do
		begin
			Obstacle := TObstacle(ObstacleList.Items[I]);

			bInSide := False;

			for J := 0 to N - 1 do
			begin
				Leg := TLeg(BranchLegs.Items[J]);

				if IsPointInPoly(Obstacle.Prj, Leg.FullAssesmentArea) then
				begin
					ObstacleInfo := TObstacleInfo.Create(Obstacle);

					ObstacleInfo.FlightProcedure := Leg.FlightProcedure;
					ObstacleInfo.Leg := J;
					if IsPointInPoly(Obstacle.Prj, Leg.PrimaryAssesmentArea) then
					begin
						ObstacleInfo.fTmp := 1.0;
						ObstacleInfo.Flags := 1;
					end
					else
					begin
						ObstacleInfo.Flags := 0;
						Dist0 := PointToRingDistance(Obstacle.Prj, PrimaryPoly.Ring[0]);
						Dist1 := PointToRingDistance(Obstacle.Prj, CheckPoly.Ring[0]);
						ObstacleInfo.fTmp := Dist1/(Dist0 + Dist1);
					end;

					ObstacleInfo.MOC := FlightPhases[Leg.FlightProcedure].MOC * ObstacleInfo.fTmp;
					ObstacleInfo.ReqH := ObstacleInfo.MOC + ObstacleInfo.Elevation;

					Dist0 := PointToPolylineDistance(Obstacle.Prj, Leg.NNSecondLine);
					ObstacleInfo.Dist := Dist0;
					ObstacleInfo.Ignored := (Dist0 < GPANSOPSConstants.Constant[arFIX15PlaneRang].Value) and
								((Leg.StartFIX.Altitude	 - FlightPhases[Leg.FlightProcedure].MOC - Dist0 * 0.15) > ObstacleInfo.Elevation);

					ObstacleList.Items[I] := ObstacleInfo;
					bInSide := True;
					break;
				end
			end;

			if bInSide then		Inc(I)
			else				ObstacleList.Delete(I);
		end;

		CheckPoly.Free;
		ReportForm.AddBranch(ObstacleList, BranchLegs);
		FObstacleListList.Add(ObstacleList);
		GReportButton.SetEnabled(True);
		NextBtn.Enabled := True;
	end;

	AddBranchFrame1.Execute(FSignificantCollection, FIFInfo, fpInitialApproach, FAerodrome.Prj, 150000.0);
//==============================================================================

	cbBranch.Clear;
    for I := 0 to FIABranchs.Count - 1 do
    begin
    	BranchLegs := TList(FIABranchs.Items[I]);
        cbBranch.Items.Add(TLeg(BranchLegs.Items[0]).StartFIX.Name);
    end;

	if cbBranch.Items.Count > 0 then
	begin
		cbBranch.ItemIndex := 0;
		cbBranchChange(cbBranch);
	end;
end;

//======================================================================= Page 4
procedure TMainForm.CreateStraightInSegment(Num: Integer);
var
	FullPolyMAPt,
	PrimaryPolyMAPt,

	FullPoly,
	PrimaryPoly:	TPolygon;
	tpmRing:		TRing;

	ptSOC, refPt,
	ptFar28, ptFar56,
	ptASWL28, ptASWR28,
	ptASWL56, ptASWR56,
	ptLE00, ptRE00,
	ptLE01, ptRE01,
	ptLE02, ptRE02,
	ptLE03, ptRE03,
	ptLE04, ptRE04,
	ptLE05, ptRE05:	TPoint;

	Toler28, Toler56,
	SplayAngle15,
	ASW_C28,ASW_C56,
	fTmp:	Double;
	Branch:			TList;
	Leg:			TLeg;
begin
	FullPoly		:= TPolygon.Create;
	PrimaryPoly		:= TPolygon.Create;
	tpmRing			:= TRing.Create;
	SplayAngle15	:= GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
//	ASW_C			:= GGNSSConstants.Constant[MAHF_LE_56].SemiWidth;

	Branch := TList(FIABranchs.Items[Num]);
	Leg := TLeg(Branch.Items[Branch.Count - 1]);

	FullPolyMAPt := Leg.FullAssesmentArea;
	PrimaryPolyMAPt := Leg.PrimaryAssesmentArea;

	refPt := LocalToPrj(FMAPt.PrjPt, prmCourse.Value + PI, FMAPt.ATT, 0);

	ptSOC := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, 0);
	FSOC.PrjPt := ptSOC;
	ptSOC.Free;

//	SocDist := DistTPFromARP := Hypot(ptSOC.X - FAerodrome.Prj.X, ptSOC.Y - FAerodrome.Prj.Y);

	Toler28 := WPTParams[ffMApLT28, ptRNPAPCH].ATT;
	ptFar28 := CircleVectorIntersect(FAerodrome.Prj, PBNInternalTriggerDistance, FMAPt.PrjPt, prmCourse.Value, fTmp);
	LocalToPrj(ptFar28, prmCourse.Value, -Toler28, 0, ptFar28);

	ASW_C28 := WPTParams[ffMApLT28, ptRNPAPCH].Semiwidth;
	ptASWL28 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, ASW_C28);
	ptASWR28 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, -ASW_C28);

	Toler56 := WPTParams[ffMApGE28, ptRNPAPCH].ATT;
	ptFar56 := CircleVectorIntersect(FAerodrome.Prj, PBNTerminalTriggerDistance, FMAPt.PrjPt, prmCourse.Value, fTmp);
	LocalToPrj(ptFar56, prmCourse.Value, -Toler56, 0, ptFar56);

	ASW_C56 := WPTParams[ffMApGE28, ptRNPAPCH].Semiwidth;
	ptASWL56 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, ASW_C56);
	ptASWR56 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, -ASW_C56);
//==========================================================================================================
	ptRE00 := RingVectorIntersect (FullPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
	FMAPt.ASW_R := fTmp;

	ptRE01 := LineLineIntersect(ptRE00, prmCourse.Value - SplayAngle15, FSOC.PrjPt, prmCourse.Value + 0.5 * PI).AsPoint;
	ptRE02 := LineLineIntersect(ptRE00, prmCourse.Value - SplayAngle15, ptASWR28, prmCourse.Value).AsPoint;
	ptRE03 := LineLineIntersect(ptRE02, prmCourse.Value,				ptFar28, prmCourse.Value + 0.5*PI).AsPoint;

	ptRE04 := LineLineIntersect(ptRE03, prmCourse.Value - SplayAngle15, ptASWR56, prmCourse.Value).AsPoint;
	ptRE05 := LineLineIntersect(ptRE04, prmCourse.Value,				ptFar56, prmCourse.Value + 0.5*PI).AsPoint;

//==========================================================================================================
	ptLE00 := RingVectorIntersect (FullPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);
	FMAPt.ASW_L := fTmp;

	ptLE01 := LineLineIntersect(ptLE00, prmCourse.Value + SplayAngle15, FSOC.PrjPt, prmCourse.Value + 0.5 * PI).AsPoint;
	ptLE02 := LineLineIntersect(ptLE00, prmCourse.Value + SplayAngle15, ptASWL28, prmCourse.Value).AsPoint;
	ptLE03 := LineLineIntersect(ptLE02, prmCourse.Value, ptFar28, prmCourse.Value + 0.5*PI).AsPoint;

	ptLE04 := LineLineIntersect(ptLE03, prmCourse.Value + SplayAngle15, ptASWL56, prmCourse.Value).AsPoint;
	ptLE05 := LineLineIntersect(ptLE04, prmCourse.Value, ptFar56, prmCourse.Value + 0.5*PI).AsPoint;

	tpmRing.AddPoint(ptRE00);
	tpmRing.AddPoint(ptRE01);
	tpmRing.AddPoint(ptRE02);
	tpmRing.AddPoint(ptRE03);
	tpmRing.AddPoint(ptRE04);
	tpmRing.AddPoint(ptRE05);

	tpmRing.AddPoint(ptLE05);
	tpmRing.AddPoint(ptLE04);
	tpmRing.AddPoint(ptLE03);
	tpmRing.AddPoint(ptLE02);
	tpmRing.AddPoint(ptLE01);
	tpmRing.AddPoint(ptLE00);

	FullPoly.AddRing(tpmRing);
	tpmRing.Clear;
	ptRE00.Free;
	ptLE00.Free;
//==========================================================================================================
	ptLE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);

	FMAPt.ASW_2_L := fTmp;

	ptRE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
	FMAPt.ASW_2_R := fTmp;

	LocalToPrj(ptRE02, prmCourse.Value, 0, 0.5*ASW_C28, ptRE01);
	LocalToPrj(ptRE03, prmCourse.Value, 0, 0.5*ASW_C28, ptRE02);
	LocalToPrj(ptRE04, prmCourse.Value, 0, 0.5*ASW_C56, ptRE03);
	LocalToPrj(ptRE05, prmCourse.Value, 0, 0.5*ASW_C56, ptRE04);
//==========================================================================================================
	FMAPt.ReCreateArea;
	FMAPt.RefreshGraphics;

	LocalToPrj(ptLE02, prmCourse.Value, 0, -0.5*ASW_C28, ptLE01);
	LocalToPrj(ptLE03, prmCourse.Value, 0, -0.5*ASW_C28, ptLE02);
	LocalToPrj(ptLE04, prmCourse.Value, 0, -0.5*ASW_C56, ptLE03);
	LocalToPrj(ptLE05, prmCourse.Value, 0, -0.5*ASW_C56, ptLE04);

	tpmRing.AddPoint(ptRE00);
	tpmRing.AddPoint(ptRE01);
	tpmRing.AddPoint(ptRE02);
	tpmRing.AddPoint(ptRE03);
	tpmRing.AddPoint(ptRE04);

	tpmRing.AddPoint(ptLE04);
	tpmRing.AddPoint(ptLE03);
	tpmRing.AddPoint(ptLE02);
	tpmRing.AddPoint(ptLE01);
	tpmRing.AddPoint(ptLE00);

	PrimaryPoly.AddRing(tpmRing);

	TPolygon(SMASFullPolyList.Items[Num]).Assign(FullPoly);
	TPolygon(SMASPrimaryPolyList.Items[Num]).Assign(PrimaryPoly);

	tpmRing.Free;
	refPt.Free;
	ptFar28.Free;
	ptASWL28.Free;
	ptASWR28.Free;

	ptFar56.Free;
	ptASWL56.Free;
	ptASWR56.Free;

	ptRE00.Free;
	ptRE01.Free;
	ptRE02.Free;
	ptRE03.Free;
	ptRE04.Free;
	ptRE05.Free;

	ptLE00.Free;
	ptLE01.Free;
	ptLE02.Free;
	ptLE03.Free;
	ptLE04.Free;
	ptLE05.Free;

	FullPoly.Free;
	PrimaryPoly.Free;
end;

procedure TMainForm.CreateMATIASegment(Num: Integer; FIX: TFIX);
var
	FullPolyMAPt,
	PrimaryPolyMAPt,

	FullPoly,
	PrimaryPoly:	TPolygon;
	tpmRing:		TRing;

	ptTmp,
	refPt, ptFar,
	ptEnd,
	ptASWL28, ptASWR28,
	ptASWL56, ptASWR56,
	ptLE00, ptRE00,
	ptLE01, ptRE01,
	ptLE02, ptRE02,
	ptLE03, ptRE03,
	ptLE04, ptRE04:	TPoint;

	Dist6sec,
	IAS, TAS,
	PilotTolerance,
	BankTolerance,

	TriggerDistance28,
	DistFullWidth28,
	DistFullWidth56,
	DistTP, LPT,
	DistToFar,
	SOCWidth,
	DistTurn,
	SplayAngle15,
	TanSplayAngle15,
	ASW_C28,
	ASW_C56, fTmp:	Double;
	SpecialCase:	Boolean;
	Branch:			TList;
	Leg:			TLeg;
begin
	FullPoly		:= TPolygon.Create;
	PrimaryPoly		:= TPolygon.Create;
	tpmRing			:= TRing.Create;
	SplayAngle15	:= GPANSOPSConstants.Constant[arafTrn_OSplay].Value;
	TanSplayAngle15 := Tan(SplayAngle15);

	ASW_C28 := WPTParams[ffMApLT28, ptRNPAPCH].Semiwidth;
	ASW_C56 := WPTParams[ffMApGE28, ptRNPAPCH].Semiwidth;

	LPT := (2*BYTE(FIX.FlyMode <> fmFlyBy)-1)*FIX.LPT;

	Branch := TList(FIABranchs.Items[Num]);
	Leg := TLeg(Branch.Items[Branch.Count - 1]);

	FullPolyMAPt := Leg.FullAssesmentArea;
	PrimaryPolyMAPt := Leg.PrimaryAssesmentArea;

	refPt := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, -FMAPt.ATT, 0);

	ptTmp := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, 0);
	FSOC.PrjPt := ptTmp;

	ptASWL28 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, ASW_C28);
	ptASWR28 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, -ASW_C28);

	ptASWL56 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, ASW_C56);
	ptASWR56 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, 0, -ASW_C56);

	DistTP := Hypot(FIX.PrjPt.X - refPt.X, FIX.PrjPt.Y - refPt.Y);

	CircleVectorIntersect(FAerodrome.Prj, PBNInternalTriggerDistance, FMAPt.PrjPt, prmCourse.Value, fTmp, ptTmp);
	TriggerDistance28 := Hypot(ptTmp.X - refPt.X, ptTmp.Y - refPt.Y) - WPTParams[ffMApGE28, ptRNPAPCH].ATT;

//==========================================================================================================
	ptRE00 := RingVectorIntersect (FullPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
	FMAPt.ASW_R := fTmp;

	ptLE00 := RingVectorIntersect (FullPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);
	FMAPt.ASW_L := fTmp;

	fTmp := Max(FMAPt.ASW_R, FMAPt.ASW_L);
	DistFullWidth28 := (ASW_C28 - fTmp)/TanSplayAngle15;
	DistFullWidth56 := (ASW_C56 - ASW_C28)/TanSplayAngle15;
	SOCWidth := fTmp + (prmSOCFromMAPtDistance.Value + FMAPt.ATT)*TanSplayAngle15;

	ptRE02 := nil;	ptLE02 := nil;
	ptRE03 := nil;	ptLE03 := nil;
	ptRE04 := nil;	ptLE04 := nil;

	SpecialCase := (cbTurnAt.ItemIndex = 2)and((SOCWidth > FIX.SemiWidth)or(DistFullWidth28 > DistTP - FIX.EPT));

	if SpecialCase then
	begin
		if SOCWidth > FIX.SemiWidth then
		begin
			ptRE01 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, -SOCWidth);
			ptRE02 := LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, -FIX.SemiWidth);
			ptLE02 := LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, FIX.SemiWidth);
			ptLE01 := LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, SOCWidth);

			tpmRing.AddPoint(ptRE00);
			tpmRing.AddPoint(ptRE01);
			tpmRing.AddPoint(ptRE02);
			tpmRing.AddPoint(ptLE02);
			tpmRing.AddPoint(ptLE01);
			tpmRing.AddPoint(ptLE00);
			FullPoly.AddRing(tpmRing);
//==================================================================
			tpmRing.Clear;
			ptRE00.Free;
			ptLE00.Free;

			ptLE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);
			FMAPt.ASW_2_L := fTmp;

			ptRE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
			FMAPt.ASW_2_R := fTmp;

			LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, -0.5*SOCWidth, ptRE01);
			LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, -0.5*FIX.SemiWidth, ptRE02);
			LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, 0.5*FIX.SemiWidth, ptLE02);
			LocalToPrj(FMAPt.PrjPt, prmCourse.Value, prmSOCFromMAPtDistance.Value, 0.5*SOCWidth, ptLE01);

			tpmRing.AddPoint(ptRE00);
			tpmRing.AddPoint(ptRE01);
			tpmRing.AddPoint(ptRE02);
			tpmRing.AddPoint(ptLE02);
			tpmRing.AddPoint(ptLE01);
			tpmRing.AddPoint(ptLE00);
			PrimaryPoly.AddRing(tpmRing);
		end
		else if DistFullWidth28 > DistTP - FIX.EPT then
		begin
			ptRE01 := LocalToPrj(FIX.PrjPt, prmCourse.Value, -FIX.EPT, -FIX.SemiWidth);
			ptRE02 := LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, -FIX.SemiWidth);

			ptLE02 := LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, FIX.SemiWidth);
			ptLE01 := LocalToPrj(FIX.PrjPt, prmCourse.Value, -FIX.EPT, FIX.SemiWidth);

			tpmRing.AddPoint(ptRE00);
			tpmRing.AddPoint(ptRE01);
			tpmRing.AddPoint(ptRE02);
			tpmRing.AddPoint(ptLE02);
			tpmRing.AddPoint(ptLE01);
			tpmRing.AddPoint(ptLE00);
			FullPoly.AddRing(tpmRing);
//==================================================================
			tpmRing.Clear;
			ptRE00.Free;
			ptLE00.Free;

			ptLE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);
			FMAPt.ASW_2_L := fTmp;

			ptRE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
			FMAPt.ASW_2_R := fTmp;

			LocalToPrj(FIX.PrjPt, prmCourse.Value, -FIX.EPT, -0.5*FIX.SemiWidth, ptRE01);
			LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, -0.5*FIX.SemiWidth, ptRE02);
			LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, 0.5*FIX.SemiWidth, ptLE02);
			LocalToPrj(FIX.PrjPt, prmCourse.Value, -FIX.EPT, 0.5*FIX.SemiWidth, ptLE01);

			tpmRing.AddPoint(ptRE00);
			tpmRing.AddPoint(ptRE01);
			tpmRing.AddPoint(ptRE02);
			tpmRing.AddPoint(ptLE02);
			tpmRing.AddPoint(ptLE01);
			tpmRing.AddPoint(ptLE00);
			PrimaryPoly.AddRing(tpmRing);
		end
	end
	else
	begin
		if cbTurnAt.ItemIndex = 2 then
		begin
			DistTurn := DistTP - FIX.EPT;
			ptEnd := LocalToPrj(FIX.PrjPt, prmCourse.Value, LPT, 0)
		end
		else
		begin
			DistTurn := DistTP;

			PilotTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
			BankTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;

			TAS := IASToTAS(prmMAIAS.Value, prmPreTurnAltitude.Value, FAerodrome.Temperature);

			Dist6sec := (TAS + GPANSOPSConstants.Constant[dpWind_Speed].Value) * (PilotTolerance + BankTolerance);
			ptEnd := LocalToPrj(FIX.PrjPt, prmCourse.Value, Dist6sec, 0)
		end;

		if DistTP >= TriggerDistance28 then
			ptFar := LocalToPrj(refPt, prmCourse.Value, TriggerDistance28, 0)
		else
			ptFar := ptEnd.Clone.AsPoint;

//==========================================================================================================
		if DistTP < DistFullWidth28 then
			ptRE01 := LineLineIntersect(ptRE00, prmCourse.Value - SplayAngle15, ptFar, prmCourse.Value + 0.5*PI).AsPoint
		else
		begin
			ptRE01 := LineLineIntersect(ptRE00, prmCourse.Value - SplayAngle15, ptASWR28, prmCourse.Value).AsPoint;
			ptRE02 := LineLineIntersect(ptRE01, prmCourse.Value, ptFar, prmCourse.Value + 0.5*PI).AsPoint;

			if DistTP >= TriggerDistance28 then
			begin
				if DistTP < TriggerDistance28 + DistFullWidth56 then
					ptRE03 := LineLineIntersect(ptRE02, prmCourse.Value - SplayAngle15, ptEnd, prmCourse.Value + 0.5*PI).AsPoint
				else
				begin
					ptRE03 := LineLineIntersect(ptRE02, prmCourse.Value - SplayAngle15, ptASWR56, prmCourse.Value).AsPoint;
					ptRE04 := LineLineIntersect(ptRE03, prmCourse.Value, ptEnd, prmCourse.Value + 0.5*PI).AsPoint;
				end;
			end
		end;
//==========================================================================================================
		if DistTP < DistFullWidth28 then
			ptLE01 := LineLineIntersect(ptLE00, prmCourse.Value + SplayAngle15, ptFar, prmCourse.Value + 0.5*PI).AsPoint
		else
		begin
			ptLE01 := LineLineIntersect(ptLE00, prmCourse.Value + SplayAngle15, ptASWL28, prmCourse.Value).AsPoint;
			ptLE02 := LineLineIntersect(ptLE01, prmCourse.Value, ptFar, prmCourse.Value + 0.5*PI).AsPoint;

			if DistTP >= TriggerDistance28 then
			begin
				if DistTP < TriggerDistance28 + DistFullWidth56 then
					ptLE03 := LineLineIntersect(ptLE02, prmCourse.Value + SplayAngle15, ptEnd, prmCourse.Value + 0.5*PI).AsPoint
				else
				begin
					ptLE03 := LineLineIntersect(ptLE02, prmCourse.Value + SplayAngle15, ptASWL56, prmCourse.Value).AsPoint;
					ptLE04 := LineLineIntersect(ptLE03, prmCourse.Value, ptEnd, prmCourse.Value - 0.5*PI).AsPoint;
				end;
			end;
		end;
		ptFar.Free;
//==========================================================================================================

		tpmRing.AddPoint(ptRE00);
		tpmRing.AddPoint(ptRE01);

		if DistTP >= DistFullWidth28 then
		begin
			tpmRing.AddPoint(ptRE02);
			if DistTP >= TriggerDistance28 then
			begin
				tpmRing.AddPoint(ptRE03);
				if DistTP >= TriggerDistance28 + DistFullWidth56 then
				begin
					tpmRing.AddPoint(ptRE04);
					tpmRing.AddPoint(ptLE04);
				end;
				tpmRing.AddPoint(ptLE03);
			end;
			tpmRing.AddPoint(ptLE02);
		end;

		tpmRing.AddPoint(ptLE01);
		tpmRing.AddPoint(ptLE00);

		FullPoly.AddRing(tpmRing);
		tpmRing.Clear;
		ptRE00.Free;
		ptLE00.Free;
//==========================================================================================================
		ptLE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value + 0.5 * PI, fTmp, True);
		FMAPt.ASW_2_L := fTmp;

		ptRE00 := RingVectorIntersect (PrimaryPolyMAPt.Ring[0], refPt, prmCourse.Value - 0.5 * PI, fTmp, True);
		FMAPt.ASW_2_R := fTmp;
//
		fTmp := PointToLineDistance(ptRE01, FMAPt.PrjPt, prmCourse.Value);
		LocalToPrj(ptRE01, prmCourse.Value, 0, -0.5*fTmp, ptRE01);

		fTmp := PointToLineDistance(ptLE01, FMAPt.PrjPt, prmCourse.Value);
		LocalToPrj(ptLE01, prmCourse.Value, 0, -0.5*fTmp, ptLE01);

		if DistTP >= DistFullWidth28 then
		begin
			fTmp := PointToLineDistance(ptRE02, FMAPt.PrjPt, prmCourse.Value);
			LocalToPrj(ptRE02, prmCourse.Value, 0, -0.5*fTmp, ptRE02);

			fTmp := PointToLineDistance(ptLE02, FMAPt.PrjPt, prmCourse.Value);
			LocalToPrj(ptLE02, prmCourse.Value, 0, -0.5*fTmp, ptLE02);

			if DistTP >= TriggerDistance28 then
			begin
				fTmp := PointToLineDistance(ptRE03, FMAPt.PrjPt, prmCourse.Value);
				LocalToPrj(ptRE03, prmCourse.Value, 0, -0.5*fTmp, ptRE03);

				fTmp := PointToLineDistance(ptLE03, FMAPt.PrjPt, prmCourse.Value);
				LocalToPrj(ptLE03, prmCourse.Value, 0, -0.5*fTmp, ptLE03);

				if DistTP >= TriggerDistance28 + DistFullWidth56 then
				begin
					fTmp := PointToLineDistance(ptRE04, FMAPt.PrjPt, prmCourse.Value);
					LocalToPrj(ptRE04, prmCourse.Value, 0, -0.5*fTmp, ptRE04);

					fTmp := PointToLineDistance(ptLE04, FMAPt.PrjPt, prmCourse.Value);
					LocalToPrj(ptLE04, prmCourse.Value, 0, -0.5*fTmp, ptLE04);
				end;
			end;
		end;

//==========================================================================================================
		tpmRing.AddPoint(ptRE00);
		tpmRing.AddPoint(ptRE01);
		if DistFullWidth28 < DistTurn then
		begin
			tpmRing.AddPoint(ptRE02);
			if DistTP >= TriggerDistance28 then
			begin
				tpmRing.AddPoint(ptRE03);
				if DistTP >= TriggerDistance28 + DistFullWidth56 then
				begin
					tpmRing.AddPoint(ptRE04);
					tpmRing.AddPoint(ptLE04);
				end;
				tpmRing.AddPoint(ptLE03);
			end;
			tpmRing.AddPoint(ptLE02);
		end;

		tpmRing.AddPoint(ptLE01);
		tpmRing.AddPoint(ptLE00);

		PrimaryPoly.AddRing(tpmRing);
		ptEnd.Free;
	end;

	TPolygon(SMATFullPolyList.Items[Num]).Assign(FullPoly);
	TPolygon(SMATPrimaryPolyList.Items[Num]).Assign(PrimaryPoly);

	ptTmp.Free;
	tpmRing.Free;
	refPt.Free;
	ptASWL28.Free;
	ptASWR28.Free;
	ptASWL56.Free;
	ptASWR56.Free;

	ptRE00.Free;
	ptRE01.Free;
	ptRE02.Free;
	ptRE03.Free;

	ptLE00.Free;
	ptLE01.Free;
	ptLE02.Free;
	ptLE03.Free;

	FullPoly.Free;
	PrimaryPoly.Free;
end;
//==============================================================================

procedure TMainForm.cbMAclimbGradientChange(Sender: TObject);
var
	I, N, K,
	MaxIx,
	IMALenghtIx,
	MAHFLenghtIx:	Integer;
	fTmp,
	Gradient:		Double;
	ObstacleList:	TList;
	ObstacleInfo:	TObstacleInfo;
begin
	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then exit;

	ObstacleList := SMASFObstacleListList.Items[0];
	Gradient := GrTable[K];
	FMAMinOCA := CalcMAMinOCA(ObstacleList, Gradient, prmOCA.Value, MaxIx, 1);

	FIMALenght := 0;
	FMAHFLenght := (FAerodrome.Elevation + FlightPhases[fpEnroute].MOC - FMAMinOCA)/ Gradient;
	if FMAHFLenght < 0 then FMAHFLenght := 0;

	IMALenghtIx := -1;
	MAHFLenghtIx := -1;
	N := ObstacleList.Count - 1;

	for I := 0 To N - 1 do
	begin
		ObstacleInfo := ObstacleList.Items[I];
		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpFinalMissedApproach].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if (fTmp > ObstacleInfo.Dist) and (fTmp > FIMALenght) then
		begin
			FIMALenght := fTmp;
			IMALenghtIx := I
		end;

		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpEnroute].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if FMAHFLenght < fTmp then
		begin
			FMAHFLenght := fTmp;
			MAHFLenghtIx := I
		end;
	end;

	ReportForm.AddMissedApproach(ObstacleList);
	SMASFObstacleListList.Add(ObstacleList);

	fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA, cdToOuter, nil);
	editMAOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FIMALenght, cdToOuter, nil);
	Edit3.Text := RoundToStr(fTmp, GDistanceAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FMAHFLenght + FMAPt.SOCDistance, cdToOuter, nil);
	Edit4.Text := RoundToStr(fTmp, GDistanceAccuracy);

	prmMAHFFromMAPtDist.BeginUpdate;
	prmMAHFFromMAPtDist.MinValue := FMAHFLenght + FMAPt.SOCDistance;
	prmMAHFFromMAPtDist.MaxValue := 200000;
	prmMAHFFromMAPtDist.EndUpdate;

	Edit5.Text := '';
	Edit6.Text := '';
	Edit7.Text := '';

	if MaxIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MaxIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 2;
		Edit5.Text := ObstacleInfo.AID;
	end;

	if IMALenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[IMALenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 4;
		Edit6.Text := ObstacleInfo.AID;
	end;

	if MAHFLenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MAHFLenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 8;
		Edit7.Text := ObstacleInfo.AID;
	end;

	fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA + FMAHFLenght * Gradient, cdToOuter, nil);
	Edit8.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	ReportForm.AddMissedApproach(SMASFObstacleListList.Items[0]);
end;

procedure TMainForm.OnAbeamChangeValue(Sender: TCustomParameter);
var
	tmpPoint:		TPoint;
	ApproachDir:	Double;
begin
	ApproachDir := FAlignedApproachDir;

	tmpPoint := LocalToPrj(FRWYDirection.Prj, fRWYCLDir + PI,
			GPANSOPSConstants.Constant[arMinInterDist].Value, Sender.Value);

	SetAlignedAnchorPoint(tmpPoint);
	tmpPoint.Free;

	if ApproachDir = prmCourse.Value then	DrawCLine;
end;

procedure TMainForm.OnAlongChangeValue(Sender: TCustomParameter);
var
	tmpPoint:	TPoint;
	fTmp,
	ApproachDir,
	R, fTAS,
	MaxTurnAngle,
	TurnAngle:	Double;
begin
	ApproachDir := FNotAlignedApproachDir;

	tmpPoint := LocalToPrj(FRWYDirection.Prj, fRWYCLDir + PI, Sender.Value, 0.0);
	SetNotAlignedAnchorPoint(tmpPoint, -1);
	tmpPoint.Free;

	if ApproachDir = prmCourse.Value then	DrawCLine;

	fTAS := IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory],
			FAerodrome.Elevation, FAerodrome.Temperature) +
				GPANSOPSConstants.Constant[arNearTerrWindSp].Value;

	R := BankToRadius(GPANSOPSConstants.Constant[arBankAngle].Value, fTAS);
	TurnAngle := prmCourse.Value - FRWYDirection.Direction;
	if TurnAngle < 0 then TurnAngle := TurnAngle + 2*PI;
	if TurnAngle > PI then TurnAngle := 2*PI - TurnAngle;

	MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAircraftCategory];
	if (FAircraftCategory <= acB)and (TurnAngle < GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC]) then
			MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC];

	FFAOCA := GPANSOPSConstants.Constant[arAbv_Treshold].Value +
			(prmAlong.Value + R * Tan(0.5 * MaxTurnAngle) +	5.0 * fTAS) *
				GPANSOPSConstants.Constant[arFADescent_Nom].Value;

	fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA, cdToOuter, nil);
	editMinimalOCH.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA + FAerodrome.Elevation, cdToOuter, nil);
	editMinimalOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);
end;

procedure TMainForm.DrawCLine;
var
	fTrackDir:		Double;
	ptSymbol:		TPointSymbol;
	LineSymbol:		TLineSymbol;

	AnchorPoint:	TPoint;
	tmpPt,
	FarFAP:			TPoint;
	pTrackPolyline,
	pCLPolyline:	TPolyline;
	pLeg:			TPart;
begin
	if FAligned then
	begin
		fTrackDir := FAlignedApproachDir;
		AnchorPoint := FAlignedAnchorPoint1;
	end
	else
	begin
		fTrackDir := FNotAlignedApproachDir;
		AnchorPoint := FNotAlignedAnchorPoint1;
	end;

	GUI.SafeDeleteGraphic(FFarMAPtHandle);
	GUI.SafeDeleteGraphic(FCLHandle);
	GUI.SafeDeleteGraphic(FTrackHandle);

	if CreateMAPt(fTrackDir, AnchorPoint, FAligned) then
	begin
		ptSymbol := TPointSymbol.Create(smsSquare, 255, 8);
		FFarMAPtHandle := GUI.DrawPoint(FFarMAPt, ptSymbol);
		ptSymbol.Free;
	end;

	pCLPolyline := TPolyline.Create;
	pTrackPolyline := TPolyline.Create;

	pLeg := TPart.Create;
	FarFAP := LocalToPrj(FRWYDirection.Prj, fRWYCLDir + PI, GPANSOPSConstants.Constant[arFAPMaxRange].Value, 0.0);
//==============================================================
	pLeg.AddPoint(FRWYDirection.Prj);
	pLeg.AddPoint(FarFAP);
	pCLPolyline.AddPart(pLeg);

	LineSymbol := TLineSymbol.Create(slsSolid, 255, 1);
	FCLHandle := GUI.DrawPolyline(pCLPolyline, LineSymbol);
	FarFAP.Free;
//==============================================================

	pLeg.Clear;
	tmpPt := LocalToPrj(FFarMAPt, fTrackDir + PI, GPANSOPSConstants.Constant[arFAPMaxRange].Value, 0.0);

	pLeg.AddPoint(FFarMAPt);
	pLeg.AddPoint(tmpPt);
	pTrackPolyline.AddPart(pLeg);

	LineSymbol.Color := 0;
	FTrackHandle := GUI.DrawPolyline(pTrackPolyline, LineSymbol);

	tmpPt.Free;
	pLeg.Free;
	pTrackPolyline.Free;
	pCLPolyline.Free;
	LineSymbol.Free;
end;

procedure TMainForm.OnCourseChangeValue(Sender: TCustomParameter);
var
	fTmp,
	R, fTAS,
	MaxTurnAngle,
	TurnAngle:		Double;
	pt1400, pt00,
	LocalSigPt: 	TPoint;
	ptInter:		TGeometry;
begin
	if FAligned then
	begin
		FAlignedApproachDir := Sender.Value;
		if FAlignedExistingAnchorPoint then
		begin
			pt1400 := LocalToPrj(FRWYDirection.Prj, fRWYCLDir + PI, GPANSOPSConstants.Constant[arMinInterDist].Value, 0.0);
			LocalSigPt := PrjToLocal(pt1400, fRWYCLDir + PI, FAlignedAnchorPoint1);
			pt00 := TPoint.Create(0, 0);
			ptInter := LineLineIntersect(pt00, 0.5 * PI, LocalSigPt, Sender.Value - fRWYCLDir);

			prmAbeam.BeginUpdate;
			prmAbeam.Value := 0.0;
			if ptInter.GeometryType = gtPoint then		prmAbeam.Value := ptInter.AsPoint.Y;
			prmAbeam.EndUpdate;

			pt1400.Free;			pt00.Free;
			LocalSigPt.Free;		ptInter.Free;
		end
	end
	else
	begin
		FNotAlignedApproachDir := Sender.Value;
		if FNotAlignedExistingAnchorPoint then
		begin
			pt00 := TPoint.Create(0, 0);
			LocalSigPt := PrjToLocal(FRWYDirection.Prj, fRWYCLDir + PI, FNotAlignedAnchorPoint1);
			ptInter := LineLineIntersect(pt00, 0, LocalSigPt, Sender.Value - fRWYCLDir);

			prmAlong.BeginUpdate;
			prmAlong.Value := 0.0;
			if ptInter.GeometryType = gtPoint then
				prmAlong.Value := ptInter.AsPoint.X;
			prmAlong.EndUpdate;

			pt00.Free;		ptInter.Free;
			LocalSigPt.Free;
		end;

		fTAS := IASToTAS(GAircraftCategoryConstants.Constant[cVfafMax].Value[FAircraftCategory],
							FAerodrome.Elevation, FAerodrome.Temperature) +
									GPANSOPSConstants.Constant[arNearTerrWindSp].Value;

		R := BankToRadius(GPANSOPSConstants.Constant[arBankAngle].Value, fTAS);
		TurnAngle := prmCourse.Value - FRWYDirection.Direction;
		if TurnAngle < 0 then TurnAngle := TurnAngle + 2*PI;
		if TurnAngle > PI then TurnAngle := 2*PI - TurnAngle;

		MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[FAircraftCategory];
		if (FAircraftCategory <= acB)and (TurnAngle < GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC]) then
				MaxTurnAngle := GAircraftCategoryConstants.Constant[arMaxInterAngle].Value[acC];

		FFAOCA := GPANSOPSConstants.Constant[arAbv_Treshold].Value +
				(prmAlong.Value + R * Tan(0.5 * MaxTurnAngle) +
				5.0 * fTAS) * GPANSOPSConstants.Constant[arFADescent_Nom].Value;

		fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA, cdToOuter, nil);
		editMinimalOCH.Text := RoundToStr(fTmp, GAltitudeAccuracy);

		fTmp := CConverters[puAltitude].ConvertFunction (FFAOCA + FAerodrome.Elevation, cdToOuter, nil);
		editMinimalOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);
	end;

	DrawCLine;
end;

procedure TMainForm.CalcFAFALtitude;
var
	FAF2THRDist:		Double;
	tmpRange:			TInterval;
begin
	FAF2THRDist := Hypot(FFAF.PrjPt.X - FFarMAPt.X , FFAF.PrjPt.Y - FFarMAPt.Y);
	if not FAligned then
		FAF2THRDist := FAF2THRDist +
			Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

	if FApproachType = 0 then
		FFAFInfo.Altitude := FAF2THRDist * FFAF.Gradient +
					GPANSOPSConstants.Constant[arAbv_Treshold].Value + FRWYDirection.ElevTdz
	else
		FFAFInfo.Altitude := FAF2THRDist * FFAF.Gradient +
							 FCirclingMinOCH + FAerodrome.Elevation;

	FFAFInfo.AltitudeParameter.ChangeValue(FFAF.Altitude);

	tmpRange.Left := FFAF.Altitude;
	tmpRange.Right := tmpRange.Left + (FIFInfo.DistanceParameter.Value -
			GAircraftCategoryConstants.Constant[arImHorSegLen].Value[FAircraftCategory]) *
									  FIFInfo.GradientParameter.Range.Left;

	FIFInfo.AltitudeParameter.BeginUpdate;
	FIFInfo.AltitudeParameter.Range := tmpRange;
	FIFInfo.Altitude := FFAFInfo.AltitudeParameter.Value;
	FIFInfo.AltitudeParameter.EndUpdate;
end;

procedure TMainForm.OnFAFDistanceChangeValue(Sender: TCustomParameter);
var
	I:					Integer;
	fAngle, fDist,
	fAngle1, fAngle2,
	fMinDist, fFAFAlt,
	fdX, fdY,
	fDistTreshol1,
	fDistTreshol2:		Double;
	point:				TPoint;
	sigPoint:			TSignificantPoint;
begin
	fDist := Sender.Value;

	if FApproachType = 0 then
	begin
		if FAligned then
			point := LocalToPrj(FMAPt.PrjPt, F3ApproachDir + PI, fDist, 0.0)
		else
			point := LocalToPrj(FMAPt.PrjPt, F3ApproachDir + PI, fDist-prmAlong.Value, 0.0);

		fFAFAlt := fDist * FFAFInfo.GradientParameter.Value + FRWYDirection.ElevTdz + GPANSOPSConstants.Constant[arAbv_Treshold].Value;
	end
	else
	begin
		point := LocalToPrj(FMAPt.PrjPt, F3ApproachDir + PI, fDist, 0.0);
		fFAFAlt := fDist * FFAFInfo.GradientParameter.Value + FCirclingMinOCH + FAerodrome.Elevation;
	end;

	prmFAFAltitude.ChangeValue(fFAFAlt);

{
		if FApproachType = 0 then
			 (FFAFInfo.AltitudeParameter.Value - FRWYDirection.ElevTdz - GPANSOPSConstants.Constant[arAbv_Treshold].Value)/FFAFInfo.GradientParameter.Value)
		else
			H = fDist * FFAFInfo.GradientParameter.Value + FCirclingMinOCH + FAerodrome.Elevation
			(FFAFInfo.AltitudeParameter.Value - FCirclingMinOCH - FAerodrome.Elevation)/FFAFInfo.GradientParameter.Value;
}
	FFAF.PrjPt := point;

//if bflg then
//	gui.DrawPointWithText(point, 'FAF Point');

	prmIFCourse.BeginUpdate;
	prmIFCourse.OutConvertionPoint := FFAF.PrjPt.Clone;
	prmIFCourse.InConvertionPoint := FFAF.GeoPt.Clone;
	prmIFCourse.EndUpdate;

//	CalcFAFALtitude;
	cbIFList.Clear;

	FFAF.RefreshGraphics;

	fAngle1 := DegToRad(2.0);
	fAngle2 := DegToRad(50.0);

	fDist := FIF.CalcFromMinStablizationDistance(0.5*PI);
	fDistTreshol1 := FFAF.CalcInToMinStablizationDistance(fAngle1) + fDist;
	fDistTreshol2 := FFAF.CalcInToMinStablizationDistance(fAngle2) + fDist;

	FIFCourseSgfPoint.Clear;
	FIFDistanceSgfPoint.Clear;

	for I := 0 to FSignificantCollection.Count - 1 do
	begin
		sigPoint := FSignificantCollection.Item [i].asSignificantPoint;
		PrjToLocal(FFAF.PrjPt, F3ApproachDir + PI, sigPoint.Prj, point);

		if point.X > 0 then
		begin
			fDist := Hypot(point.Y, point.X);
			fAngle := Abs(ArcTan2(point.Y, point.X));

			fMinDist := fDistTreshol1;
			if fAngle > fAngle1 then
				fMinDist := fDistTreshol2;

			if (fDist <= GPANSOPSConstants.Constant[arImRange_Max].Value)and
				(fDist > fMinDist)and
				(fAngle <= GPANSOPSConstants.Constant[arImMaxIntercept].Value) then
				cbIFList.AddItem(sigPoint.AixmID, sigPoint);
		end
	end;

	point.Free;

	rbIfFromList.Enabled := cbIFList.Items.Count > 0;

	if rbIFFromList.Checked and rbIfFromList.Enabled then
		cbIFList.ItemIndex := 0
	else
	begin
		if rbIFByDistAndAzimuth.Checked then
			rbIFClick(rbIFByDistAndAzimuth)
		else
		begin
			rbIFByDistAndAzimuth.Checked := True;
		end;
	end;

{	if cbIFList.Items.Count > 0 then
		cbIFList.ItemIndex := 0
	else
		rbIFByDistAndAzimuth.Checked := True;
}
//====================================================================

//	fdX := fDist;//FFAFInfo.Distance;
//if bflg then
//begin
//	gui.DrawPointWithText(FFAFInfo.FIX.PrjPt, 128, 'FAF Point - -');
//	gui.DrawPointWithText(FIFInfo.FIX.PrjPt, 128, 'IF Point - -');
//end;

	fdX := FFAFInfo.FIX.PrjPt.X - FIFInfo.FIX.PrjPt.X;
	fdY := FFAFInfo.FIX.PrjPt.Y - FIFInfo.FIX.PrjPt.Y;

	fAngle := ArcTan2(fdY, fdX);
	if SubtractAngles(FIFInfo.Angle, fAngle) < C_PI_2 then
	begin
		FIFInfo.Angle := fAngle;
		FIFInfo.Distance := Hypot(fdY, fdX);
	end
	else
	begin
//		FIFInfo.Angle := fIFAngle;			//ArcTan2(fdY, fdX);
//		FIFInfo.Distance := fIFDist;		//Hypot(fdY, fdX);
		OnIFDistanceChangeValue(prmIFDistance);
	end;
end;

// FAF =====================================================

procedure TMainForm.OnIFDistanceChangeValue(Sender: TCustomParameter);
var
	tmpRange:	TInterval;
begin
	tmpRange.Left := FFAF.Altitude;
	tmpRange.Right := tmpRange.Left + (FIFInfo.DistanceParameter.Value -
			GAircraftCategoryConstants.Constant[arImHorSegLen].Value[FAircraftCategory]) *
									  FIFInfo.GradientParameter.Range.Left;

	FIFInfo.AltitudeParameter.BeginUpdate;
	FIFInfo.AltitudeParameter.Range := tmpRange;
//	FIFInfo.Altitude := FFAFInfo.AltitudeParameter.Value;
	FIFInfo.AltitudeParameter.EndUpdate;

	ApplyIFDistanceAndDirection(Sender.Value, FIFInfo.AngleParameter.Value);
end;

procedure TMainForm.OnIFCourseChangeValue(Sender: TCustomParameter);
begin
	ApplyIFDistanceAndDirection(FIFInfo.DistanceParameter.Value, Sender.Value);
end;

procedure TMainForm.prmMAPtDistanceChangeValue(Sender: TCustomParameter);
var
	I, J, K,
	L, N, M,
	MaxFAIx,
	MaxL, MaxIx,
	IMALenghtIx,
	MAHFLenghtIx:	Integer;

	point:			TPoint;
	PrevLeg,
	Leg:			TLeg;

	Branch,
	ObstacleList,
	ObstalceInfoList:	TList;

	StartFIX,
	EndFIX:			TWayPoint;
	TmpPolyLine:	TPolyLine;
	tmpPoly,
	FullPoly,
	PrimaryPoly:	TPolygon;
	Obstacle:		TObstacle;
	ObstacleInfo:	TObstacleInfo;

	fOCH,
	Gradient, fTmp,
	Dist0, Dist1:	Double;
begin
	point := LocalToPrj(FFAF.PrjPt, F3ApproachDir, Sender.Value, 0.0);
	FMAPt.PrjPt := point;
	point.Free;
// MAPT Shifting ======================================================================

	N := FIABranchs.Count;
	if N<1 then
	begin
		exit;
	end;

	M := FObstacleListList.Count;

	for I := 0 to M - 1 do
	begin
		ObstacleList := FObstacleListList.Items[I];
		L := ObstacleList.Count;
		for J := 0 to L - 1 do
			TObstacleInfo(ObstacleList.Items[J]).Free;
		ObstacleList.Free;
	end;
	FObstacleListList.Clear;

	for I := 0 to N - 1 do
	begin
		Branch := TList(FIABranchs.Items[I]);

		M := Branch.Count;
		Leg := TLeg(Branch.Items[M-1]);
		EndFIX := Leg.EndFIX;
		EndFIX.Assign(FMAPt);

		PrevLeg := nil;
		for J := 0 to M - 1 do
		begin
			Leg := TLeg(Branch.Items[J]);

			Leg.CreateNomTrack(PrevLeg);
			Leg.CreateProtectionArea(PrevLeg, FAerodrome);
			PrevLeg := Leg;
		end;

		FullPoly := TPolygon.Create;
		PrimaryPoly := TPolygon.Create;

		for J := 0 to M - 1 do
		begin
			Leg := TLeg(Branch.Items[J]);
			Leg.RefreshGraphics;

			if J > 0 then
			begin
				tmpPoly := GGeoOperators.UnionGeometry(FullPoly, Leg.FullArea).AsPolygon;
				FullPoly.Assign(tmpPoly);
				tmpPoly.Free;

				tmpPoly := GGeoOperators.UnionGeometry(PrimaryPoly, Leg.PrimaryArea).AsPolygon;
				PrimaryPoly.Assign(tmpPoly);
				tmpPoly.Free;
			end
			else
			begin
				FullPoly.Assign(Leg.FullArea);
				PrimaryPoly.Assign(Leg.PrimaryArea);
			end;
		end;

		TPolygon(FullPolyList.Items[I]).Assign(FullPoly);
		TPolygon(PrimaryPolyList.Items[I]).Assign(PrimaryPoly);

		FullPoly.Free;
		PrimaryPoly.Free;

		FullPoly := TPolygon(FullPolyList.Items[I]);
		PrimaryPoly := TPolygon(PrimaryPolyList.Items[I]);

		ObstacleList := GetObstacles(FullPoly, FObstalces);
		L := ObstacleList.Count;

//==============================================================================
		for J := 0 To L - 1 do
		begin
			Obstacle := TObstacle(ObstacleList.Items[J]);
			ObstacleInfo := TObstacleInfo.Create(Obstacle);

			ObstacleInfo.Flags := Byte(IsPointInPoly(Obstacle.Prj, PrimaryPoly));
			if ObstacleInfo.Flags and 1 = 0 then
			begin
				Dist0 := PointToRingDistance(Obstacle.Prj, PrimaryPoly.Ring[0]);
				Dist1 := PointToRingDistance(Obstacle.Prj, FullPoly.Ring[0]);
				ObstacleInfo.fTmp := Dist1/(Dist0 + Dist1);
			end
			else
				ObstacleInfo.fTmp := 1.0;

			for K := 0 to M - 1 do
			begin
				Leg := TLeg(Branch.Items[K]);
				if IsPointInPoly(Obstacle.Prj, Leg.FullAssesmentArea) then
				begin
					ObstacleInfo.FlightProcedure := Leg.FlightProcedure;
					ObstacleInfo.Leg := K;
					ObstacleInfo.MOC := FlightPhases[Leg.FlightProcedure].MOC * ObstacleInfo.fTmp;
					ObstacleInfo.ReqH := ObstacleInfo.MOC + ObstacleInfo.Elevation;
					break;
				end
			end;

			ObstacleList.Items[J] := ObstacleInfo;
		end;

		ReportForm.ModifyBranch(ObstacleList, Branch, I);
		FObstacleListList.Add(ObstacleList);
//==============================================================================
	end;

	FFAMAPtPosOCA := prmFAFAltitude.Value - prmMAPtDistance.Value*prmFAFGradient.Value;
	FFAOCA := CalcFAOCA(Max(FFAAligOCA, FFAMAPtPosOCA), MaxFAIx, MaxL);
	prmOCA.ChangeValue(FFAOCA);
	FMAPt.Altitude := FFAOCA;

	fOCH := FFAOCA - FAerodrome.Elevation;
	fTmp := CConverters[puAltitude].ConvertFunction (fOCH, cdToOuter, nil);
	editOCH.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	if MaxFAIx>=0 then
		labelFAOcaCaused.Caption := 'Caused by obstacle '+
			TObstacleInfo(TList(FObstacleListList.Items[MaxL]).Items[MaxFAIx]).AID
	else if FFAAligOCA > FFAMAPtPosOCA  then
		labelFAOcaCaused.Caption := 'Caused by Track alignment.'
	else
		labelFAOcaCaused.Caption := 'Caused by MAPt position.';

// MAPT Shifting ===============================================================
	FMAPt.RefreshGraphics;

	CreateStraightInSegment(0);

	GUI.SafeDeleteGraphic(FMAFullPolyGr);
	GUI.SafeDeleteGraphic(FMAPrimPolyGr);

	FullPoly := SMASFullPolyList.Items[0];
	PrimaryPoly := SMASPrimaryPolyList.Items[0];

	FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
	FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);
//==============================================================================

	if SMASFObstacleListList.Count>0 then
	begin
		ObstacleList := SMASFObstacleListList.Items[0];
		N := ObstacleList.Count;
		for I := 0 To N - 1 do
		begin
			ObstacleInfo := TObstacleInfo(ObstacleList.Items[I]);
			ObstacleInfo.Free;
		end;
		ObstacleList.Free;
		SMASFObstacleListList.Clear;
	end;

	I := cbMAclimbGradient.ItemIndex;
	if I < 0 then
	begin
		I := 0;
		cbMAclimbGradient.ItemIndex := I;
	end;

	Gradient := GrTable[I];

	ObstacleList := GetObstacles(FullPoly, FObstalces);
	FMAMinOCA := GetMAObstAndMinOCA(ObstacleList, FullPoly, PrimaryPoly, Gradient, MaxIx);

{
	N := ObstacleList.Count;

	FMAMinOCA := prmOCA.Value;
	MaxIx := -1;

	for I := 0 To N - 1 do
	begin
		Obstacle := TObstacle(ObstacleList.Items[I]);
		ObstacleInfo := TObstacleInfo.Create(Obstacle);

		ObstacleInfo.Flags := Byte(IsPointInPoly(Obstacle.Prj, PrimaryPoly));
		if ObstacleInfo.Flags and 1 = 0 then
		begin
			Dist0 := PointToRingDistance(Obstacle.Prj, PrimaryPoly.Ring[0]);
			Dist1 := PointToRingDistance(Obstacle.Prj, FullPoly.Ring[0]);
			ObstacleInfo.fTmp := Dist1/(Dist0 + Dist1);
		end
		else
			ObstacleInfo.fTmp := 1.0;

		ObstacleInfo.Dist := PointToLineDistance(Obstacle.Prj, FSOC.PrjPt, prmCourse.Value - 0.5 * PI);

		if ObstacleInfo.Dist >= 0 then
			ObstacleInfo.FlightProcedure := fpInitialMissedApproach
		else
			ObstacleInfo.FlightProcedure := fpIntermediateMissedApproach;

		ObstacleInfo.Leg := -1;
		ObstacleInfo.MOC := FlightPhases[ObstacleInfo.FlightProcedure].MOC * ObstacleInfo.fTmp;
		ObstacleInfo.ReqH := ObstacleInfo.Elevation + ObstacleInfo.MOC;

		if ObstacleInfo.Dist >= 0 then
			ObstacleInfo.ReqOCA := ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradient
		else
			ObstacleInfo.ReqOCA := Min(ObstacleInfo.ReqH - ObstacleInfo.Dist * Gradient,
										ObstacleInfo.Elevation + FlightPhases[fpFinalApproach].MOC*ObstacleInfo.fTmp);

		if FMAMinOCA < ObstacleInfo.ReqOCA then
		begin
			MaxIx := I;
			FMAMinOCA := ObstacleInfo.ReqOCA;
		end;

		ObstacleList.Items[I] := ObstacleInfo;
	end;
}
	SMASFObstacleListList.Add(ObstacleList);
	ReportForm.AddMissedApproach(ObstacleList);

//==============================================================================
	N := ObstacleList.Count;

	FIMALenght := 0;
	FMAHFLenght := (FAerodrome.Elevation + FlightPhases[fpEnroute].MOC - FMAMinOCA)/ Gradient;
	if FMAHFLenght < 0 then FMAHFLenght := 0;

	IMALenghtIx := -1;
	MAHFLenghtIx := -1;

	for I := 0 To N - 1 do
	begin
		ObstacleInfo := ObstacleList.Items[I];

		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpFinalMissedApproach].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if (fTmp > ObstacleInfo.Dist) and (fTmp > FIMALenght) then
		begin
			FIMALenght := fTmp;
			IMALenghtIx := I
		end;

		fTmp := (ObstacleInfo.Elevation + FlightPhases[fpEnroute].MOC * ObstacleInfo.fTmp - FMAMinOCA)/Gradient;
		if FMAHFLenght < fTmp then
		begin
			FMAHFLenght := fTmp;
			MAHFLenghtIx := I
		end;
	end;

//===============================================================================
	fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA, cdToOuter, nil);
	editMAOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FIMALenght, cdToOuter, nil);
	Edit3.Text := RoundToStr(fTmp, GDistanceAccuracy);

	fTmp := CConverters[puDistance].ConvertFunction (FMAHFLenght + FMAPt.SOCDistance, cdToOuter, nil);
	Edit4.Text := RoundToStr(fTmp, GDistanceAccuracy);

	prmMAHFFromMAPtDist.BeginUpdate;
	prmMAHFFromMAPtDist.MinValue := FMAHFLenght + FMAPt.SOCDistance;
	prmMAHFFromMAPtDist.MaxValue := 200000;
	prmMAHFFromMAPtDist.EndUpdate;

	Edit5.Text := '';
	Edit6.Text := '';
	Edit7.Text := '';

	if MaxIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MaxIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 2;
		Edit5.Text := ObstacleInfo.AID;
	end;

	if IMALenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[IMALenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 4;
		Edit6.Text := ObstacleInfo.AID;
	end;

	if MAHFLenghtIx >= 0 then
	begin
		ObstacleInfo := TObstacleInfo(ObstacleList.Items[MAHFLenghtIx]);
		ObstacleInfo.Flags := ObstacleInfo.Flags or 8;
		Edit7.Text := ObstacleInfo.AID;
	end;

	fTmp := prmOCA.Value + FMAHFLenght * Gradient;
	prmPreTurnAltitude.BeginUpdate;
	prmPreTurnAltitude.MinValue := fTmp;
	prmPreTurnAltitude.MaxValue := prmOCA.Value + FMSA;
	prmPreTurnAltitude.Value := fTmp;
	prmPreTurnAltitude.EndUpdate;

	fTmp := CConverters[puAltitude].ConvertFunction (fTmp, cdToOuter, nil);
	Edit8.Text := RoundToStr(fTmp, GAltitudeAccuracy);
end;

// Page 4 ======================================================================

procedure TMainForm.cbGoToTurnClick(Sender: TObject);
var
	List:	TList;
begin
	bOK.Enabled := not cbGoToTurn.Checked;
	prmMAHFFromMAPtDist.Enabled := not cbGoToTurn.Checked;
	labMAHFAltitude.Enabled := not cbGoToTurn.Checked;
	editMAHFAltitude.Enabled := not cbGoToTurn.Checked;
	labMAHFAltitudeUnit.Enabled := not cbGoToTurn.Checked;
	NextBtn.Enabled := cbGoToTurn.Checked;

	if prmMAHFFromMAPtDist.Enabled then
		prmMAHFFromMAPtDistChangeValue(prmMAHFFromMAPtDist)
	else
	begin
		List := TList.Create;
		ReportForm.AddMIAS_TIA(List, 'MA straight segment till MAHF');
		List.Free;
		FMAHF.DeleteGraphics;
		prmMAPtDistanceChangeValue(prmMAPtDistance);
	end;
end;

procedure TMainForm.cbTurnAtChange(Sender: TObject);
var
	limit, delta:	Double;
begin
	prmTurnFromMAPtDist.Active := cbTurnAt.ItemIndex = 2;

	prmTurnFromMAPtDist.BeginUpdate;
	prmTurnFromMAPtDist.MinValue := FMAPt.SOCDistance + IfThen(cbTurnAt.ItemIndex > 1, FMATF.ATT, 0);
//	prmTurnFromSOCDist.MaxValue :=  80000.0;
	prmTurnFromMAPtDist.EndUpdate;

	rgFlyMode.Enabled := cbTurnAt.ItemIndex > 1;
	if cbTurnAt.ItemIndex <= 1 then
		rgFlyMode.ItemIndex := 1;

	prmPreTurnAltitude.Active := cbTurnAt.ItemIndex = 1;
	prmPreTurnAltitude.ReadOnly := cbTurnAt.ItemIndex <> 1;
	prmPreTurnAltitude.ShowMinMaxHint := cbTurnAt.ItemIndex = 1;

	prmTurnFromMAPtDist.Active := cbTurnAt.ItemIndex > 1;
	prmTurnFromMAPtDist.ReadOnly := cbTurnAt.ItemIndex <= 1;
	prmTurnFromMAPtDist.ShowMinMaxHint := cbTurnAt.ItemIndex = 2;


	if cbTurnAt.ItemIndex = 0 then
		prmPreTurnAltitude.ChangeValue(prmPreTurnAltitude.MinValue);

	if cbTurnAt.ItemIndex = 1 then
	begin
		rgToFIX.ItemIndex := 0;
		rgToFIX.Enabled := False;
	end
	else
		rgToFIX.Enabled := True;

	FMATF.TurnAt := TTurnAt(cbTurnAt.ItemIndex);
	FMATF.FlyMode := TFlyMode(rgFlyMode.ItemIndex);
	FMATF.FlyPath := TFlyPath(rgToFIX.ItemIndex);

	delta := DegToRad(240 - 120*rgToFIX.ItemIndex);

	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex*delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1-cbTurnSide.ItemIndex)*delta;
	prmTurnCourse.EndUpdate;
	prmTurnCourse.Active := True;

{
	prmTurn1.Visible := rgToFIX.ItemIndex = 0;
	prmTurn3.Visible := rgToFIX.ItemIndex = 0;
}
{
	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;
	if N = 0 then
		rbCreateNew.Checked := True;
}
	if cbTurnAt.ItemIndex < 2 then
		TurnOnMAPt
	else if cbTurnAt.ItemIndex = 2 then
		TurnOnTP
end;

procedure TMainForm.AddBranchFrame1spBtnPlusClick(Sender: TObject);
begin
  AddBranchFrame1.spBtnPlusClick(Sender);
end;

procedure TMainForm.AddBranchFrame1cbDirListChange(Sender: TObject);
begin
  AddBranchFrame1.cbDirListChange(Sender);
end;

procedure TMainForm.AddBranchFrame1rBtnAFromListClick(Sender: TObject);
begin
  AddBranchFrame1.rButtonDirectionClick(Sender);
end;

procedure TMainForm.AddBranchFrame1rBtnAzimuthClick(Sender: TObject);
begin
  AddBranchFrame1.rButtonDirectionClick(Sender);
end;

procedure TMainForm.AddBranchFrame1rBtnDistanceClick(Sender: TObject);
begin
  AddBranchFrame1.rBtnDistanceClick(Sender);
end;

procedure TMainForm.AddBranchFrame1rBtnDFromListClick(Sender: TObject);
begin
  AddBranchFrame1.rBtnDistanceClick(Sender);
end;

procedure TMainForm.AddBranchFrame1cbDistListChange(Sender: TObject);
begin
  AddBranchFrame1.cbDistListChange(Sender);
end;

procedure TMainForm.AddBranchFrame1cbFlyModeChange(Sender: TObject);
begin
  AddBranchFrame1.cbFlyModeChange(Sender);
end;

procedure TMainForm.AddBranchFrame1prmCourseChangeValue(
  Sender: TCustomParameter);
begin
  AddBranchFrame1.OnCourseChangeValue(Sender);
end;

procedure TMainForm.AddBranchFrame1prmDistanceChangeValue(
  Sender: TCustomParameter);
begin
  AddBranchFrame1.OnChangeDistance(Sender);
end;

procedure TMainForm.AddBranchFrame1prmIASChangeValue(
  Sender: TCustomParameter);
begin
  AddBranchFrame1.prmIASChangeValue(Sender);
end;

procedure TMainForm.AddBranchFrame1prmAltitudeChangeValue(
  Sender: TCustomParameter);
begin
  AddBranchFrame1.prmAltitudeChangeValue(Sender);
end;

procedure TMainForm.AddBranchFrame1prmGradientChangeValue(
  Sender: TCustomParameter);
begin
  AddBranchFrame1.prmGradientChangeValue(Sender);
end;

procedure TMainForm.prmSOCFromMAPtDistanceChangeValue(Sender: TCustomParameter);
begin
	FMAPt.SOCDistance := Sender.Value;
end;

procedure TMainForm.rbExistingPointClick(Sender: TObject);
begin
	labelTurnWPT.Enabled := TRadioButton(Sender).Tag = 0;
	cbTurnWPT.Enabled := TRadioButton(Sender).Tag = 0;
	labelTurnWPTType.Enabled := TRadioButton(Sender).Tag = 0;

	prmNWPTFromTPDist.ReadOnly := TRadioButton(Sender).Tag = 0;
	prmTurnCourse.ReadOnly := TRadioButton(Sender).Tag = 0;

	prmNWPTFromTPDist.Active := not prmNWPTFromTPDist.ReadOnly;
//	prmTurnCourse.Active := not prmTurnCourse.ReadOnly;
	bTurnExistingPoint := TRadioButton(Sender).Tag = 0;

	if pageContMain.ActivePageIndex <> 5 then
		exit;

	if bTurnExistingPoint then	cbTurnWPTChange(cbTurnWPT)
	else						prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.cbTurnSideChange(Sender: TObject);
var
	limit,
	delta:	Double;
	N:		Integer;
begin
	FMATF.TurnDirection := TSideDirection(2*cbTurnSide.ItemIndex-1);
//	FMATF.ReCreateArea;

	delta := DegToRad(240 - 120*rgToFIX.ItemIndex);

	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

//	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex*delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1-cbTurnSide.ItemIndex)*delta;
	prmTurnCourse.EndUpdate;
//	prmTurnCourse.Active := True;
//	prmTurnCourse.PerformCheck(True);

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then
		rbCreateNew.Checked := True;

	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
	else if not prmTurnCourse.ValueChanged then prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.cbTurnWPTChange(Sender: TObject);
var
	K:				Integer;
	sigPoint:		TSignificantPoint;
	dX, dY:			Double;
	fDist, fAngle:	Double;
begin
	K := cbTurnWPT.ItemIndex;
	if K < 0 then exit;

	sigPoint := TSignificantPoint(cbTurnWPT.Items.Objects[K]);
	labelTurnWPTType.Caption := AIXMTypeNames[sigPoint.AIXMType];

	if cbTurnAt.ItemIndex = 0 then
	begin
		dX := sigPoint.Prj.X - FMAPt.PrjPt.X;
		dY := sigPoint.Prj.Y - FMAPt.PrjPt.Y;
	end
	else
	begin
		dX := sigPoint.Prj.X - FMATF.PrjPt.X;
		dY := sigPoint.Prj.Y - FMATF.PrjPt.Y;
	end;

	fDist := Hypot(dX, dY);
	prmNWPTFromTPDist.ChangeValue(fDist);

	fAngle := ArcTan2(dY, dX);
	prmTurnCourse.ChangeValue(fAngle);

	if not prmTurnCourse.ValueChanged then
		ReCreateMATurnArea;
end;

procedure TMainForm.rgToFIXClick(Sender: TObject);
var
	limit,
	delta:			Double;
	N:				Integer;
begin
	delta := DegToRad(240 - 120*rgToFIX.ItemIndex);

	if (cbTurnAt.ItemIndex = 2)and(rgToFIX.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	FMATF.FlyPath			:= TFlyPath(rgToFIX.ItemIndex);

	prmTurn1.Visible := rgToFIX.ItemIndex = 0;
	prmTurn3.Visible := rgToFIX.ItemIndex = 0;

	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex*delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1-cbTurnSide.ItemIndex)*delta;
	prmTurnCourse.Active := True;
	prmTurnCourse.EndUpdate;

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then
		rbCreateNew.Checked := True;

	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
	else								prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.prmNWPTFromTPDistChangeValue(Sender: TCustomParameter);
begin
	ReCreateMATurnArea;
end;

procedure TMainForm.prmTurnCourseChangeValue(Sender: TCustomParameter);
var
	fTurn,
	fTurnAngle,
	fMinDist,
	DistTPFromARP:			Double;
begin
	FMATF.OutDirection		:= prmTurnCourse.Value;

//	if FMATF.SensorType = stGNSS then		TriggerDistance := GNSSTriggerDistance
//	else									TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;
//	TriggerDistance := PBNInternalTriggerDistance;

	DistTPFromARP := Hypot(FMATF.PrjPt.X - FAerodrome.Prj.X, FMATF.PrjPt.Y - FAerodrome.Prj.Y);

	if DistTPFromARP >= PBNTerminalTriggerDistance then			FMATF.Role := MATF_GT_56
	else if DistTPFromARP >= PBNInternalTriggerDistance then	FMATF.Role := PBN_MATF_GE_28
	else														FMATF.Role := PBN_MATF_LT_28;

	prmNWPTFromTPDist.BeginUpdate(False);

	if (rgFlyMode.ItemIndex = 0) or (rgToFIX.ItemIndex = 0) then
		prmNWPTFromTPDist.MinValue := MinMATurnDist
	else
	begin
		fTurn := 1 - 2*cbTurnSide.ItemIndex;		//Right = -1;	Left = 1
		fTurnAngle := Modulus((prmTurnCourse.Value - FMAPt.EntryDirection) * fTurn, C_2xPI);
		fMinDist := FMATF.CalcFromMinStablizationDistance(fTurnAngle);
		prmNWPTFromTPDist.MinValue := Max(MinMATurnDist, fMinDist)
	end;
	prmNWPTFromTPDist.EndUpdate;

	ReCreateMATurnArea;
end;

procedure TMainForm.prmPreTurnAltitudeChangeValue(Sender: TCustomParameter);
begin
	FMATF.Altitude := prmPreTurnAltitude.Value;
	TurnOnMAPt
end;

procedure TMainForm.prmTurnFromMAPtDistChangeValue(
  Sender: TCustomParameter);
begin
	CalcCourseInterval;
	TurnOnTP
end;

procedure TMainForm.prmMATBankAngleChangeValue(Sender: TCustomParameter);
begin
	FMATF.Bank	:= prmMATBankAngle.Value;
	cbTurnAtChange(cbTurnAt)
//	ReCreateMATurnArea;
end;

procedure TMainForm.prmMAIASChangeValue(Sender: TCustomParameter);
begin
	FMATF.IAS := prmMAIAS.Value;
	cbTurnAtChange(cbTurnAt)
end;

procedure TMainForm.rgFlyModeClick(Sender: TObject);
var
	limit,
	delta:			Double;
	N:				Integer;
begin
	if not rgFlyMode.Enabled then
		exit;

	FMATF.FlyMode := TFlyMode(rgFlyMode.ItemIndex);

	if (cbTurnAt.ItemIndex < 2)or(rgFlyMode.ItemIndex > 0) then
		rgToFIX.Enabled := True
	else
	begin
		rgToFIX.ItemIndex := 1;
		rgToFIX.Enabled := False;
	end;

	delta := DegToRad(240 - 120*rgToFIX.ItemIndex);
	if (cbTurnAt.ItemIndex = 2)and(rgFlyMode.ItemIndex = 0) then
	begin
		limit := 2 * ArcTan((prmTurnFromMAPtDist.Value - FMATF.ATT - FMAPt.SOCDistance)/FMATF.CalcTurnRadius);
		if delta > limit then delta := limit
	end;

	prmTurnCourse.Active := False;
	prmTurnCourse.BeginUpdate;
	prmTurnCourse.MinValue := FMAPt.EntryDirection - cbTurnSide.ItemIndex*delta;
	prmTurnCourse.MaxValue := FMAPt.EntryDirection + (1-cbTurnSide.ItemIndex)*delta;
	prmTurnCourse.EndUpdate;
	prmTurnCourse.Active := True;

	N := FillMATurnPoints;
	rbExistingPoint.Enabled := N > 0;
	rbCreateNew.Enabled := N > 0;

	if N = 0 then
		rbCreateNew.Checked := True;

	if rbExistingPoint.Checked then		cbTurnWPTChange(cbTurnWPT)
	else								prmTurnCourseChangeValue(prmTurnCourse);
end;

procedure TMainForm.prmIFIASChangeValue(Sender: TCustomParameter);
begin
	FIF.IAS := FIFInfo.SpeedParameter.Value;
	FFAF.IAS := FIF.IAS;
	ApplyIFDistanceAndDirection(prmIFDistance.Value, prmIFCourse.Value);
end;

procedure TMainForm.prmIFAltitudeChangeValue(Sender: TCustomParameter);
begin
	FIF.Altitude := FIFInfo.AltitudeParameter.Value;
//	FIF.RefreshGraphics;
	ApplyIFDistanceAndDirection(prmIFDistance.Value, prmIFCourse.Value);
end;

procedure TMainForm.prmIFMaxAngleChangeValue(Sender: TCustomParameter);
begin
	ApplyIFDistanceAndDirection(prmIFDistance.Value, prmIFCourse.Value);
end;

procedure TMainForm.cbBranchChange(Sender: TObject);
var
	I, N, K:	Integer;
	Branch:		TList;
	Leg:		TLeg;
begin
	K := cbBranch.ItemIndex;
	if K < 0 then exit;

	Branch := TList(FIABranchs.Items[K]);
	cbLeg.Clear;
	N := Branch.Count;

	for I := 0 to N - 1 do
	begin
		Leg := TLeg(Branch.Items[I]);
		cbLeg.Items.Add(Leg.StartFIX.Name + ' - ' + Leg.EndFIX.Name);
	end;

	if cbLeg.Items.Count>0 then cbLeg.ItemIndex := 0;
end;


procedure TMainForm.cbLegChange(Sender: TObject);
var
	K:				Integer;
	Branch:			TList;
	PrevLeg,
	ProtoLeg,
	Leg:			TLeg;
	test:			Boolean;
begin
	K := cbLeg.ItemIndex;
	if K < 0 then exit;
	Branch := TList(FIABranchs.Items[cbBranch.ItemIndex]);
	if K >= Branch.Count then exit;

	test := CheckBoxTest.Checked;
	if test then
	begin
		if k = 0 then	PrevLeg := nil
		else			PrevLeg := TLeg(Branch.Items[K-1]);

		ProtoLeg := TLeg(Branch.Items[K]);
		Leg := TLeg.Create(ProtoLeg.StartFIX, ProtoLeg.EndFix, ProtoLeg.FlightProcedure, GUI, PrevLeg);

		Leg.CreateProtectionArea(PrevLeg, FAerodrome);
		GUI.DrawPolygon(Leg.FullArea, RGB(0, 255, 0), sfsDiagonalCross);
		Leg.Free;
	end
	else
	begin
		Leg := TLeg(Branch.Items[K]);
		Leg.RefreshGraphics;
	end;
end;

procedure TMainForm.GetStraightMAHFObstacles;
var
	Dist, fGradient,
	TIAMOC, fTmp,
	FAF2THRDist,
	AddMOC, FullFAMOC:	Double;

	FullPoly,
	PrimaryPoly:		TPolygon;

	I, N, K, IxTIA:		Integer;
	ObstacleInfo:		TObstacleInfo;
begin
	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then exit;
	fGradient := GrTable[K];

	FullPoly := SMATFullPolyList.Items[0];
	PrimaryPoly := SMATPrimaryPolyList.Items[0];
	
//===========================================
	if Assigned(TIAObstacleList) then
	begin
		for I := 0 to TIAObstacleList.Count - 1 do
			TObstacleInfo(TIAObstacleList.Items[I]).Free;
		TIAObstacleList.Free;
	end;

	if cbTurnAt.ItemIndex < 2 then
		TIAMOC := GPANSOPSConstants.Constant[arMA_InterMOC].Value
	else
		TIAMOC := GPANSOPSConstants.Constant[arMA_FinalMOC].Value;


	FAF2THRDist := Hypot(FFAF.PrjPt.X - FFarMAPt.X , FFAF.PrjPt.Y - FFarMAPt.Y);
	if not FAligned then
		FAF2THRDist := FAF2THRDist +
			Hypot(FRWYDirection.Prj.X - FFarMAPt.X , FRWYDirection.Prj.Y - FFarMAPt.Y);

	Dist := FAF2THRDist - GPANSOPSConstants.Constant[arMOCChangeDist].Value;
	AddMOC := (Dist * GPANSOPSConstants.Constant[arAddMOCCoef].Value) * Integer(Dist > 0);
	FullFAMOC := FlightPhases[fpFinalApproach].MOC + AddMOC;

	TIAObstacleList := GetObstacleInfoList(PrimaryPoly, FullPoly, TIAMOC, FObstalces);

	N := TIAObstacleList.Count;

	FMATIAOCA := prmOCA.Value;
	IxTIA := -1;

	for I := 0 to N - 1 do
	begin
		ObstacleInfo := TObstacleInfo(TIAObstacleList.Items[I]);
		Dist := PointToLineDistance(ObstacleInfo.PtPrj, FSOC.PrjPt, prmCourse.Value - 0.5 * PI);
		ObstacleInfo.Dist := Dist;

		ObstacleInfo.ReqOCA := ObstacleInfo.Elevation  + ObstacleInfo.fTmp * GPANSOPSConstants.Constant[arMA_InterMOC].Value - Dist * fGradient;
		fTmp := ObstacleInfo.Elevation + ObstacleInfo.fTmp * FullFAMOC;

		if (ObstacleInfo.Dist < 0) and (fTmp < ObstacleInfo.ReqOCA) then
			ObstacleInfo.ReqOCA := fTmp;

//		ObstacleInfo.ReqOCA := ObstacleInfo.Elevation +
//									Min(	ObstacleInfo.fTmp * TIAMOC - Dist * fGradient,
//											ObstacleInfo.fTmp * FullFAMOC);

		if ObstacleInfo.ReqOCA > FMATIAOCA then
		begin
			FMATIAOCA := ObstacleInfo.ReqOCA;
			IxTIA := I;
		end;
	end;

	FMATIAOCA_CO := '';

	if FMATIAOCA > prmOCA.Value then
		FMATIAOCA_CO := TObstacleInfo(TIAObstacleList.Items[IxTIA]).AID;


	fTmp := CConverters[puAltitude].ConvertFunction (FMATIAOCA, cdToOuter, nil);
	editMAOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);
	Edit5.Text := FMATIAOCA_CO;

	ReportForm.AddMIAS_TIA(TIAObstacleList, 'MA straight segment till MAHF');
end;

procedure TMainForm.prmMAHFFromMAPtDistChangeValue(Sender: TCustomParameter);
var
	I:					Integer;
	TriggerDistance,
	fTmp, DistTPFromARP,
	Gradient, MAHFDist:	Double;
	tmpPt:				TPoint;
	FullPoly,
	PrimaryPoly:		TPolygon;
begin
	if not cbGoToTurn.Checked then
	begin
		MAHFDist := Sender.Value;

		tmpPt := LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, MAHFDist, 0.0);
		FMAHF.PrjPt				:= tmpPt;
		tmpPt.Free;

		FMAHF.EntryDirection	:= FMAPt.EntryDirection;
		FMAHF.OutDirection		:= FMAPt.EntryDirection;
		FMAHF.SensorType		:= TSensorType(rgMAHFSensorType.ItemIndex);

		I := cbMAclimbGradient.ItemIndex;
		if I < 0 then
		begin
			I := 0;
			cbMAclimbGradient.ItemIndex := I;
		end;
		Gradient := GrTable[I];

		FMAHF.Altitude := FMAMinOCA +  (MAHFDist - FMAPt.SOCDistance) * Gradient;

		fTmp := CConverters[puAltitude].ConvertFunction (FMAHF.Altitude, cdToOuter, nil);
		editMAHFAltitude.Text := RoundToStr(fTmp, GAltitudeAccuracy);

//		if FMAHF.SensorType = stGNSS then	TriggerDistance := GNSSTriggerDistance
//		else								TriggerDistance := SBASTriggerDistance + GNSSTriggerDistance;

		TriggerDistance := PBNTerminalTriggerDistance;

		DistTPFromARP := Hypot(FMAHF.PrjPt.X - FAerodrome.Prj.X, FMAHF.PrjPt.Y - FAerodrome.Prj.Y);
		if DistTPFromARP >= TriggerDistance then	FMAHF.Role := MAHF_GT_56
		else										FMAHF.Role := MAHF_LE_56;

		CreateMATIASegment(0, FMAHF);

		FullPoly := SMATFullPolyList.Items[0];
		PrimaryPoly := SMATPrimaryPolyList.Items[0];

		GUI.SafeDeleteGraphic(FMAFullPolyGr);
		GUI.SafeDeleteGraphic(FMAPrimPolyGr);

		FMAFullPolyGr := GUI.DrawPolygon(FullPoly, RGB(0, 255, 0), sfsNull);
		FMAPrimPolyGr := GUI.DrawPolygon(PrimaryPoly, RGB(255, 0, 0), sfsNull);

		FMAPt.StraightArea := PrimaryPoly;
		FMATF.ReCreateArea;

		FMAPt.RefreshGraphics;
		FMAHF.RefreshGraphics;
		GetStraightMAHFObstacles;
	end;
end;

procedure TMainForm.bHelpClick(Sender: TObject);
begin
	ShowHelp(Handle, 3100 + 100*pageContMain.ActivePageIndex);
end;

procedure TMainForm.Button1Click(Sender: TObject);
begin
	Close
end;

function TMainForm.FormHelp(Command: Word; Data: Integer;
  var CallHelp: Boolean): Boolean;
begin
	CallHelp := False;
	result := True;
//	ShowHelp(Handle, 3100 + 100*pageContMain.ActivePageIndex);
end;

procedure TMainForm.FormKeyUp(Sender: TObject; var Key: Word;
  Shift: TShiftState);
begin
	if Key = VK_F1 then
		ShowHelp(Handle, 3100 + 100*pageContMain.ActivePageIndex);
end;

const
	THRAccuracy = 259.28;

	FIXRoles: Array [TFIXRole] of Integer =
	(
		ProcedureFixRoleType_IAF, ProcedureFixRoleType_IAF,
		ProcedureFixRoleType_IF,
		ProcedureFixRoleType_FAF,
		ProcedureFixRoleType_MAPT,
		ProcedureFixRoleType_OTHER, ProcedureFixRoleType_OTHER,
		ProcedureFixRoleType_MAHF, ProcedureFixRoleType_MAHF,
		ProcedureFixRoleType_OTHER, ProcedureFixRoleType_OTHER,
		ProcedureFixRoleType_TP,
		ProcedureFixRoleType_IAF,
		ProcedureFixRoleType_IF,
		ProcedureFixRoleType_FAF,
		ProcedureFixRoleType_MAPT,
		ProcedureFixRoleType_OTHER, ProcedureFixRoleType_OTHER
	);


	uomDistHorzTab:				Array[THorisontalDistanceUnit] of Integer = (UomDistance_KM, UomDistance_NM);
	uomDistVerH_Tab:			Array[TVerticalDistanceUnit] of Integer = (UomDistance_M, UomDistance_FT);
	uomDistVerTab:				Array[TVerticalDistanceUnit] of Integer = (UomDistanceVertical_M, UomDistanceVertical_FT);
	uomSpeedTab:				Array[THorisontalSpeedUnit] of Integer = (UomSpeed_KM_H, UomSpeed_KT);
	uomVerSpeedTab:				Array[TVerticalSpeedUnit] of Integer = (UomSpeed_M_MIN, UomSpeed_FT_MIN);

function TMainForm.InitialApproachLeg(NO_SEQ: Integer; Leg: Tleg): IApproachLeg;
var
	I, J:			Integer;
	fTmp:			Double;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pTerminalSegmentPoint:	ITerminalSegmentPoint;


	pcDistance:				Distance;
	pDistance:				IDistance;

	pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pSegmentPoint:			ISegmentPoint;

	pcFixDesignatedPoint:	DesignatedPoint;
	pFixDesignatedPoint:	IDesignatedPoint;

	pcSignificantPoint:		SignificantPoint;
	pSignificantPoint:		ISignificantPoint;

	pcInt32:				Int32Class;
	pInt32:					IInt32;

	pcDouble:				DoubleClass;
	pDouble:				IDouble;

//	pcString:				StringClass;
//	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pcSpeed:				Speed;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	ptTmp:					TPoint;


	pcAixmPoint:			AixmPoint;
	pAixmPoint:				IAixmPoint;

	pcAIXMCurve:			AIXMCurve;
	pAIXMCurve:				IAIXMCurve;

	pcGMLPolyline:			GMLPolyline;
	pGMLPolyline:			IGMLPolyline;

	pcGMLPart:				GMLPart;
	pGMLPart:				IGMLPart;

	pcGMLPoint:				GMLPoint;
	pGMLPoint:				IGMLPoint;
begin
	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

//	pApproachLeg := New ApproachLeg;
	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_BETWEEN;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;
	pSegmentLeg.TurnDirection := DirectionTurnType_EITHER;
	pSegmentLeg.LegPath := TrajectoryType_STRAIGHT;
	pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

//    pSegmentLeg.AltitudeOverrideATC =
//    pSegmentLeg.AltitudeOverrideReference =
//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
//    pSegmentLeg.EndConditionDesignator =
//    pSegmentLeg.Note
//    pSegmentLeg.ProcedureTurnRequired
//    pSegmentLeg.ReqNavPerformance
//    pSegmentLeg.SpeedInterpretation =
//    pSegmentLeg.ReqNavPerformance

//        pSegmentLeg.CourseDirection =
//        pSegmentLeg.ProcedureTransition
//        pSegmentLeg.StartPoint
//=======================================================================================

	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	fTmp := CConverters[puDirAngle].ConvertFunction (Leg.StartFIX.OutDirection, cdToOuter, Leg.StartFIX.PrjPt);
	pDouble.Value := fTmp;
	pSegmentLeg.Course := pDouble;
//=======================================================================================
//    Set pDouble = New DoubleClass
//    pDouble.Value = fBankAngle
//    Set pSegmentLeg.BankAngle = pDouble
//=======================================================================================
///////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//	fTmp := CConverters[puAltitude].ConvertFunction (FRWYDirection.elevTdz, cdToOuter, nil);
//	editRwyDirectionElev.Text := RoundToStr(fTmp, GAltitudeAccuracy);
///////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//	fTmp := CConverters[puDistance].ConvertFunction (FRWYDirection.clearway, cdToOuter, nil);
//	editRwyDirectionClearway.Text := RoundToStr(fTmp, GDistanceAccuracy);
///////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);

	pDistanceVertical.Uom := mVUomDistanceV;
	fTmp := CConverters[puAltitude].ConvertFunction (Leg.EndFIX.Altitude, cdToOuter, nil);
	pDistanceVertical.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;

	fTmp := CConverters[puAltitude].ConvertFunction (Leg.StartFIX.Altitude, cdToOuter, nil);

	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);

	pDistance.Uom := mVUomDistance;
	pDistance.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.UpperLimitAltitude := pDistance;
//==============================================================================
	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);

	pDistance.Uom := mHUomDistance;

	fTmp := CConverters[puDistance].ConvertFunction(ReturnDistanceInMeters(Leg.StartFIX.PrjPt, Leg.EndFIX.PrjPt), cdToOuter, nil);

	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	pDouble.Value := -RadToDeg(ArcTan(Leg.Gradient));
	pSegmentLeg.VerticalAngle := pDouble;

	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	pSpeed.Value := CConverters[puHSpeed].ConvertFunction(Leg.IAS, cdToOuter, nil);

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;

// Start Point ========================
	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//        pTerminalSegmentPoint.IndicatorFACF =      ??????????
//        pTerminalSegmentPoint.LeadDME =            ??????????
//        pTerminalSegmentPoint.LeadRadial =         ??????????


	pTerminalSegmentPoint.Role := FIXRoles[Leg.StartFIX.Role];	//ProcedureFixRoleType_IF;

//?????????????????????????		----ptTmp = ToGeo(IFPnt)
//==
	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := Leg.StartFIX.FlyMode = fmFlyOver;//
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//==

	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);


//	if (IFNavDat(K).TypeCode <> CodeDME) and (IFNavDat(K).ValCnt = -2) then
//	begin
//		pString = New StringClass
//		pString.Value = IFNavDat(K).ID + ";" + IFNavDat(K).TypeName
//		pSignificantPoint.NavaidSystemId = pString

//		pAixmPoint = New AixmPoint
//		pAixmPoint.AsIGMLPoint.PutCoord IFNavDat(K).pPtGeo.X, IFNavDat(K).pPtGeo.Y
//		pSignificantPoint.Position = pAixmPoint
//	end
//	else
//	begin

		pcFixDesignatedPoint := CoDesignatedPoint.Create;
		pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := Leg.StartFIX.ID;
		pFixDesignatedPoint.Name := Leg.StartFIX.Name;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(Leg.StartFIX.GeoPt.X, Leg.StartFIX.GeoPt.Y);

		pFixDesignatedPoint.Point := pAixmPoint;

//            pFixDesignatedPoint.Note
//            pFixDesignatedPoint.Tag
		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
//	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;

// End Of Start Point ========================

// EndPoint ========================

	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????
	pTerminalSegmentPoint.Role := FIXRoles[Leg.EndFIX.Role];		//ProcedureFixRoleType_FAF;
	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);


	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);


//	NavDat = FAFNavDat(K)

//	if (NavDat.TypeCode <> CodeDME) and (FAFTheta = NO_DATA_VALUE) then
//	begin
//		pString = New StringClass;
//		pString.Value = NavDat.ID + ";" + NavDat.TypeName;
//		pSignificantPoint.NavaidSystemId = pString;

//		pAixmPoint = New AixmPoint;
//		pAixmPoint.AsIGMLPoint.PutCoord(NavDat.pPtGeo.X, NavDat.pPtGeo.Y);
//		pSignificantPoint.Position = pAixmPoint;
//	end
//	else
//	begin
	pcFixDesignatedPoint := CoDesignatedPoint.Create;
	pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := Leg.EndFIX.ID;
		pFixDesignatedPoint.Name := Leg.EndFIX.Name;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(Leg.EndFIX.GeoPt.X, Leg.EndFIX.GeoPt.Y);

		pFixDesignatedPoint.Point := pAixmPoint;

//            pFixDesignatedPoint.Note
//            pFixDesignatedPoint.Tag
		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;
		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
//	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint ========================

// Trajectory		a===================================================
	pcAIXMCurve := CoAIXMCurve.Create;
	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

	pcGMLPolyline := CoGMLPolyline.Create;
	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

	for J := 0 to Leg.NomTrack.Count - 1 do
	begin
		pcGMLPart := CoGMLPart.Create;
		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

		for I := 0 to Leg.NomTrack.Part[J].Count - 1 do
		begin
			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

			ptTmp := GGeoOperators.geoTransformations (Leg.NomTrack.Part[J].Point[I], GPrjSR, GGeoSR).AsPoint;
			pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
			pGMLPart.Add(pGMLPoint);
			ptTmp.Free;
		end;

		pGMLPolyline.Add(pGMLPart);
	end;

	pAIXMCurve.Polyline := pGMLPolyline;
	pGMLPolyline := nil;
	pSegmentLeg.Trajectory := pAIXMCurve;
	pAIXMCurve := nil;
// END						a===================================================
end;

function TMainForm.IntermediateApproachLeg(NO_SEQ: Integer): IApproachLeg;
var
	K:				Integer;
	fTmp:			Double;

	sigPoint:			TSignificantPoint;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pcDistance:				Distance;
	pcFixDesignatedPoint:	DesignatedPoint;
	pcSignificantPoint:		SignificantPoint;
	pcSpeed:				Speed;
	pcAixmPoint:			AixmPoint;
	pcAIXMCurve:			AIXMCurve;
	pcGMLPolyline:			GMLPolyline;
	pcGMLPart:				GMLPart;
	pcGMLPoint:				GMLPoint;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pTerminalSegmentPoint:	ITerminalSegmentPoint;
	pDistance:				IDistance;

	pSegmentPoint:			ISegmentPoint;
	pFixDesignatedPoint:	IDesignatedPoint;
	pSignificantPoint:		ISignificantPoint;
    pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pInt32:					IInt32;
	pcInt32:				Int32Class;
	pcDouble:				DoubleClass;
	pDouble:				IDouble;

//	pcString:				StringClass;
//	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pAixmPoint:				IAixmPoint;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	pAIXMCurve:				IAIXMCurve;
	pGMLPolyline:			IGMLPolyline;
	pGMLPart:				IGMLPart;
	pGMLPoint:				IGMLPoint;
begin
	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_BETWEEN;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;
	pSegmentLeg.TurnDirection := DirectionTurnType_EITHER;
	pSegmentLeg.LegPath := TrajectoryType_STRAIGHT;
	pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

//    pSegmentLeg.AltitudeOverrideATC =
//    pSegmentLeg.AltitudeOverrideReference =
//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
//    pSegmentLeg.EndConditionDesignator =
//    pSegmentLeg.Note
//    pSegmentLeg.ProcedureTurnRequired
//    pSegmentLeg.ReqNavPerformance
//    pSegmentLeg.SpeedInterpretation =
//    pSegmentLeg.ReqNavPerformance

//        pSegmentLeg.CourseDirection =
//        pSegmentLeg.ProcedureTransition
//        pSegmentLeg.StartPoint
//=======================================================================================

	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	fTmp := CConverters[puDirAngle].ConvertFunction (FIF.OutDirection, cdToOuter, FIF.PrjPt);
	pDouble.Value := fTmp;
	pSegmentLeg.Course := pDouble;
//=======================================================================================
//    Set pDouble = New DoubleClass
//    pDouble.Value = fBankAngle
//    Set pSegmentLeg.BankAngle = pDouble
//=======================================================================================

	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);
	pDistanceVertical.Uom := mVUomDistanceV;

	fTmp := CConverters[puAltitude].ConvertFunction (FFAF.Altitude, cdToOuter, nil);
	pDistanceVertical.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;

	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mVUomDistance;

	fTmp := CConverters[puAltitude].ConvertFunction (FIF.Altitude, cdToOuter, nil);
	pDistance.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.UpperLimitAltitude := pDistance;
//==============================================================================
	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);

	pDistance.Uom := mHUomDistance;

	fTmp := CConverters[puDistance].ConvertFunction(ReturnDistanceInMeters(FFAF.PrjPt, FIF.PrjPt), cdToOuter, nil);
	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;
//====================
	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	pDouble.Value := -RadToDeg(ArcTan(FIF.Gradient));
	pSegmentLeg.VerticalAngle := pDouble;

	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FIF.IAS, cdToOuter, nil);

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;

////////////////////////////////////////////////////////////////////////////////
// Start Point =================================================================
////////////////////////////////////////////////////////////////////////////////
	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//        pTerminalSegmentPoint.IndicatorFACF =      ??????????
//        pTerminalSegmentPoint.LeadDME =            ??????????
//        pTerminalSegmentPoint.LeadRadial =         ??????????
	pTerminalSegmentPoint.Role := ProcedureFixRoleType_IF;

//?????????????????????????		----ptTmp = ToGeo(IFPnt)
//==
	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := FIF.FlyMode = fmFlyOver;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//==

	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

	pcFixDesignatedPoint := CoDesignatedPoint.Create;
	pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

    K := cbIFList.ItemIndex;
	if rbIFFromList.Checked and (K >= 0)then
	begin
		sigPoint := TSignificantPoint(cbIFList.Items.Objects[K]);
		//pFixDesignatedPoint.Designator := sigPoint.Designator;
		pFixDesignatedPoint.ID := sigPoint.ID;
		pFixDesignatedPoint.Name := sigPoint.Name;
	end
	else
	begin
		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'COORD';
	end;

	pcAixmPoint := CoAixmPoint.Create;
	pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
	pAixmPoint.AsIGMLPoint.PutCoord(FIF.GeoPt.X, FIF.GeoPt.Y);

	pFixDesignatedPoint.Point := pAixmPoint;

	//	pFixDesignatedPoint.Note
	//	pFixDesignatedPoint.Tag
	pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

	pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
//	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;

// End Of Start Point ========================

// EndPoint ========================

	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????
	pTerminalSegmentPoint.Role := ProcedureFixRoleType_FAF;
	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);


	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

	pcFixDesignatedPoint := CoDesignatedPoint.Create;
	pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

	K := cbFAFList.ItemIndex;

   	if rbFAFFromList.Checked and (K >= 0) then
	begin
		sigPoint := TSignificantPoint(cbFAFList.Items.Objects[K]);
		pFixDesignatedPoint.ID := sigPoint.ID;
			//pFixDesignatedPoint.Designator := sigPoint.Designator;
		pFixDesignatedPoint.Name := sigPoint.Name;
    end
    else
    begin
    	pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'FAF';
    end;

	pcAixmPoint := CoAixmPoint.Create;
	pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
	pAixmPoint.AsIGMLPoint.PutCoord(FFAF.GeoPt.X, FFAF.GeoPt.Y);

	pFixDesignatedPoint.Point := pAixmPoint;

	//	pFixDesignatedPoint.Note
	//	pFixDesignatedPoint.Tag
	pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;
	pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint	========================

// Trajectory				a===================================================
	pcAIXMCurve := CoAIXMCurve.Create;
	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

	pcGMLPolyline := CoGMLPolyline.Create;
	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

		pcGMLPart := CoGMLPart.Create;
		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

			pGMLPoint.PutCoord(FIF.GeoPt.X, FIF.GeoPt.Y);
			pGMLPart.Add(pGMLPoint);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

			pGMLPoint.PutCoord(FFAF.GeoPt.X, FFAF.GeoPt.Y);
			pGMLPart.Add(pGMLPoint);

		pGMLPolyline.Add(pGMLPart);

	pAIXMCurve.Polyline := pGMLPolyline;
    pGMLPolyline := nil;
	pSegmentLeg.Trajectory := pAIXMCurve;
    pAIXMCurve := nil;
// END						a===================================================
end;

function TMainForm.FinalApproachLeg(NO_SEQ: Integer; ptFAFDescent: TPoint; var Continued: Integer): IApproachLeg;
var
	K:				Integer;
	fFinalAprLength,
    fRefHeight,
    MAPtDistVerLower,
    MAPtDistToTHR,
	fTmp:				Double;
    ptTmp, pToPoint:	TPoint;
	bOnNav:				Boolean;

	sigPoint:			TSignificantPoint;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pcDistance:				Distance;
	pcFixDesignatedPoint:	DesignatedPoint;
	pcSignificantPoint:		SignificantPoint;
	pcSpeed:				Speed;
	pcAixmPoint:			AixmPoint;
	pcAIXMCurve:			AIXMCurve;
	pcGMLPolyline:			GMLPolyline;
	pcGMLPart:				GMLPart;
	pcGMLPoint:				GMLPoint;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pTerminalSegmentPoint:	ITerminalSegmentPoint;
	pDistance:				IDistance;

	pSegmentPoint:			ISegmentPoint;
	pFixDesignatedPoint:	IDesignatedPoint;
	pSignificantPoint:		ISignificantPoint;
    pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pInt32:					IInt32;
	pcInt32:				Int32Class;
	pcDouble:				DoubleClass;
	pDouble:				IDouble;

	pcString:				StringClass;
	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pAixmPoint:				IAixmPoint;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	pAIXMCurve:				IAIXMCurve;
	pGMLPolyline:			IGMLPolyline;
	pGMLPart:				IGMLPart;
	pGMLPoint:				IGMLPoint;
begin
//	HaveFAF := False;

	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_BETWEEN;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;
	pSegmentLeg.TurnDirection := DirectionTurnType_EITHER;
	pSegmentLeg.LegPath := TrajectoryType_STRAIGHT;
	pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

//    pSegmentLeg.AltitudeOverrideATC =
//    pSegmentLeg.AltitudeOverrideReference =
//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
//    pSegmentLeg.EndConditionDesignator =
//    pSegmentLeg.Note
//    pSegmentLeg.ProcedureTurnRequired
//    pSegmentLeg.ReqNavPerformance
//    pSegmentLeg.SpeedInterpretation =
//    pSegmentLeg.ReqNavPerformance

//        pSegmentLeg.CourseDirection =
//        pSegmentLeg.ProcedureTransition
//        pSegmentLeg.StartPoint
//=======================================================================================

	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	fTmp := CConverters[puDirAngle].ConvertFunction (FFAF.OutDirection, cdToOuter, FFAF.PrjPt);
	pDouble.Value := fTmp;
	pSegmentLeg.Course := pDouble;
//=======================================================================================
//    Set pDouble = New DoubleClass
//    pDouble.Value = fBankAngle
//    Set pSegmentLeg.BankAngle = pDouble
//=======================================================================================

	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);
	pDistanceVertical.Uom := mVUomDistanceV;

	fTmp := CConverters[puAltitude].ConvertFunction (FFAF.Altitude, cdToOuter, nil);
	pDistanceVertical.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;

	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mVUomDistance;

	fTmp := CConverters[puAltitude].ConvertFunction (FFAF.Altitude, cdToOuter, nil);
	pDistance.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.UpperLimitAltitude := pDistance;
//==============================================================================
	MAPtDistToTHR := Point2LineDistancePrj(FRWYDirection.Prj, FMAPt.PrjPt, FMAPt.OutDirection + 90);
//	fMAPtPDG = FinalAreaPDG
//	pFromPoint = PtFAF

//   0 ' Far To THR
//   1 ' In THR Range
//   2 ' Ahead THR

	if FApproachType = 0 then
	begin
		fRefHeight := FRWYDirection.ElevTdz;
		MAPtDistVerLower := GPANSOPSConstants.Constant[arAbv_Treshold].Value + fRefHeight;
	end
	else
	begin
		fRefHeight := FAerodrome.Elevation;
		MAPtDistVerLower := FCirclingMinOCH + fRefHeight;
	end;

//fTmp := CConverters[puAltitude].ConvertFunction (FMAMinOCA, cdToOuter, nil);
//editMAOCA.Text := RoundToStr(fTmp, GAltitudeAccuracy);

	Continued := 1;							// In THR Range
	pToPoint := FMAPt.PrjPt;

	if MAPtDistToTHR > THRAccuracy then     // Far To THR
    begin
		if cbGoToTurn.Checked then
			MAPtDistVerLower := FMAFinalOCA + fRefHeight
		else
        	MAPtDistVerLower := FMAMinOCA + fRefHeight;

        Continued := 0;
    end
    else if MAPtDistToTHR < -THRAccuracy then
    begin
        pToPoint := FRWYDirection.Prj;
        Continued := 2;						// Ahead THR
    end;

	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);
	pDistanceVertical.Uom := mVUomDistanceV;

	fTmp := CConverters[puAltitude].ConvertFunction (MAPtDistVerLower, cdToOuter, nil);
	pDistanceVertical.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;
//======
	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mHUomDistance;

	fFinalAprLength := ReturnDistanceInMeters(FFAF.PrjPt, pToPoint);
	fTmp := CConverters[puDistance].ConvertFunction(fFinalAprLength, cdToOuter, nil);
	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;
//======

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	pDouble.Value := -RadToDeg(ArcTan(FFAF.Gradient));
	pSegmentLeg.VerticalAngle := pDouble;

	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FFAF.IAS, cdToOuter, nil);

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;
//======
////////////////////////////////////////////////////////////////////////////////
// Start Point =================================================================
////////////////////////////////////////////////////////////////////////////////

	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????
	pTerminalSegmentPoint.Role := ProcedureFixRoleType_FAF;
	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);


	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

	pcFixDesignatedPoint := CoDesignatedPoint.Create;
	pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

    K := cbFAFList.ItemIndex;

   	if rbFAFFromList.Checked and (K >= 0) then
	begin
		sigPoint := TSignificantPoint(cbFAFList.Items.Objects[K]);
		pFixDesignatedPoint.ID := sigPoint.ID;
			//pFixDesignatedPoint.Designator := sigPoint.Designator;
		pFixDesignatedPoint.Name := sigPoint.Name;
	end
    else
	begin
    	pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'FAF';
    end;

	pcAixmPoint := CoAixmPoint.Create;
	pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
	pAixmPoint.AsIGMLPoint.PutCoord(FFAF.GeoPt.X, FFAF.GeoPt.Y);

	pFixDesignatedPoint.Point := pAixmPoint;

	//	pFixDesignatedPoint.Note
	//	pFixDesignatedPoint.Tag
	pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;
	pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;
// End Of Start Point ========================


////////////////////////////////////////////////////////////////////////////////
// EndPoint ====================================================================
////////////////////////////////////////////////////////////////////////////////

	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

    if Continued <> 2 then
	begin
		pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAPT;

		bOnNav := rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

		If bOnNav Then
	       	sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

		if bOnNav and ((sigPoint.AIXMType = atVOR )or(sigPoint.AIXMType = atNDB)) then
        begin
			pcString := CoStringClass.Create;
			pcString.QueryInterface(IString, pString);

            pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
            pSignificantPoint.NavaidSystemId := pString;

            CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
			pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
            pSignificantPoint.Position := pAixmPoint;
        end
		else
        begin
			pcFixDesignatedPoint := CoDesignatedPoint.Create;
			pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		   	if bOnNav then
			begin
				pFixDesignatedPoint.ID := sigPoint.ID;
				pFixDesignatedPoint.Name := sigPoint.Name;
		    end
		    else
		    begin
    			pFixDesignatedPoint.ID := '0';
				pFixDesignatedPoint.Designator := 'COORD';
				pFixDesignatedPoint.Name := 'MAPT';
		    end;

			pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

			pcAixmPoint := CoAixmPoint.Create;
			pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

			ptTmp := GGeoOperators.geoTransformations (pToPoint, GPrjSR, GGeoSR).AsPoint;
			pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
            ptTmp.Free;

			pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
			pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
        end
	end
	else
	begin
        pTerminalSegmentPoint.Role := ProcedureFixRoleType_FTP;
		pcFixDesignatedPoint := CoDesignatedPoint.Create;
		pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'FicTHR';

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

		ptTmp := GGeoOperators.geoTransformations (pToPoint, GPrjSR, GGeoSR).AsPoint;
		pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
		ptTmp.Free;

		pFixDesignatedPoint.Point := pAixmPoint;

	//	pFixDesignatedPoint.Note
	//	pFixDesignatedPoint.Tag

		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint ========================


// Trajectory				a===================================================
	pcAIXMCurve := CoAIXMCurve.Create;
	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

	pcGMLPolyline := CoGMLPolyline.Create;
	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

		pcGMLPart := CoGMLPart.Create;
		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

			pGMLPoint.PutCoord(FFAF.GeoPt.X, FFAF.GeoPt.Y);
			pGMLPart.Add(pGMLPoint);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

            ptTmp := GGeoOperators.geoTransformations (pToPoint, GPrjSR, GGeoSR).AsPoint;
			pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
            ptTmp.Free;
			pGMLPart.Add(pGMLPoint);

		pGMLPolyline.Add(pGMLPart);

	pAIXMCurve.Polyline := pGMLPolyline;
    pGMLPolyline := nil;
	pSegmentLeg.Trajectory := pAIXMCurve;
    pAIXMCurve := nil;
// END						a===================================================
end;

function TMainForm.FinalApproachContinuedLeg(NO_SEQ, Continued: Integer): IApproachLeg;
var
	K:						Integer;
//	MAPtDistToTHR,
	fFinalAprLength,
	fRefHeight,
	MAPtDistVerLower,
	Gradient, fTmp:			Double;
	ptTmp, pFromPoint,
	pToPoint:				TPoint;
//	HaveTP,
	bOnNav:					Boolean;

	sigPoint:				TSignificantPoint;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pcDistance:				Distance;
	pcFixDesignatedPoint:	DesignatedPoint;
	pcSignificantPoint:		SignificantPoint;
	pcSpeed:				Speed;
	pcAixmPoint:			AixmPoint;
	pcAIXMCurve:			AIXMCurve;
	pcGMLPolyline:			GMLPolyline;
	pcGMLPart:				GMLPart;
	pcGMLPoint:				GMLPoint;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pTerminalSegmentPoint:	ITerminalSegmentPoint;
	pDistance:				IDistance;

	pSegmentPoint:			ISegmentPoint;
	pFixDesignatedPoint:	IDesignatedPoint;
	pSignificantPoint:		ISignificantPoint;
    pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pInt32:					IInt32;
	pcInt32:				Int32Class;
	pcDouble:				DoubleClass;
	pDouble:				IDouble;

	pcString:				StringClass;
	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pAixmPoint:				IAixmPoint;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
//	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	pAIXMCurve:				IAIXMCurve;
	pGMLPolyline:			IGMLPolyline;
	pGMLPart:				IGMLPart;
	pGMLPoint:				IGMLPoint;
begin
//    HaveSDF = CheckBox0701.Value = 1
//    HaveFAF = OptionButton0201(0).Value
//	HaveTP := cbGoToTurn.Checked;

	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
//	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_BETWEEN;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;
	pSegmentLeg.TurnDirection := DirectionTurnType_EITHER;
	pSegmentLeg.LegPath := TrajectoryType_STRAIGHT;

//	pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

//    pSegmentLeg.AltitudeOverrideATC =
//    pSegmentLeg.AltitudeOverrideReference =
//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
//    pSegmentLeg.EndConditionDesignator =
//    pSegmentLeg.Note
//    pSegmentLeg.ProcedureTurnRequired
//    pSegmentLeg.ReqNavPerformance
//    pSegmentLeg.SpeedInterpretation =
//    pSegmentLeg.ReqNavPerformance

//        pSegmentLeg.CourseDirection =
//        pSegmentLeg.ProcedureTransition
//        pSegmentLeg.StartPoint
//=======================================================================================

	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	fTmp := CConverters[puDirAngle].ConvertFunction (FFAF.OutDirection, cdToOuter, FFAF.PrjPt);
	pDouble.Value := fTmp;
	pSegmentLeg.Course := pDouble;
//=======================================================================================
//    Set pDouble = New DoubleClass
//    pDouble.Value = fBankAngle
//    Set pSegmentLeg.BankAngle = pDouble
//=======================================================================================
//   0 ' Far To THR
//   1 ' In THR Range
//   2 ' Ahead THR

	if FApproachType = 0 then
		fRefHeight := FRWYDirection.ElevTdz
	else
    	fRefHeight := FAerodrome.Elevation;

	if Continued = 2 then
    begin
    	pFromPoint := FRWYDirection.Prj;
		pToPoint := FMAPt.PrjPt;
        pSegmentLeg.LegTypeARINC := SegmentPathType_CF
    end
    else                            //2
    begin
    	pFromPoint := FMAPt.PrjPt;
		pToPoint := FMAPt.SOCPrjPt;
		pSegmentLeg.LegTypeARINC := SegmentPathType_CA;
	end;

	if cbGoToTurn.Checked then
		MAPtDistVerLower := FMAFinalOCA + fRefHeight
	else
       	MAPtDistVerLower := FMAMinOCA + fRefHeight;


	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);
	pDistanceVertical.Uom := mVUomDistanceV;

	fTmp := CConverters[puAltitude].ConvertFunction (MAPtDistVerLower, cdToOuter, nil);
	pDistanceVertical.Value := RoundToStr(fTmp, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;
//======
//	pcDistance := CoDistance.Create;
//	pcDistance.QueryInterface(IDistance, pDistance);
//	pDistance.Uom := mVUomDistance;

//	fTmp := CConverters[puAltitude].ConvertFunction (fTmp, cdToOuter, nil);
//	pDistance.Value := RoundToStr(fTmp, GAltitudeAccuracy);
//	pSegmentLeg.UpperLimitAltitude := pDistance;
//======
	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mHUomDistance;

	fFinalAprLength := ReturnDistanceInMeters(pFromPoint, pToPoint);
	fTmp := CConverters[puDistance].ConvertFunction(fFinalAprLength, cdToOuter, nil);
	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;

//====================================================================================================
	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then
		K := 0;

	Gradient := GrTable[K];

	pDouble.Value := RadToDeg(ArcTan(Gradient));
	pSegmentLeg.VerticalAngle := pDouble;
//====================================================================================================
	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FMAPt.IAS, cdToOuter, nil);

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;
//======
////////////////////////////////////////////////////////////////////////////////
// StartPoint ==================================================================
////////////////////////////////////////////////////////////////////////////////

	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

    if Continued <> 2 then
    begin
		pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAPT;

		bOnNav := rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

		If bOnNav Then
			sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

		if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
		begin
			pcString := CoStringClass.Create;
			pcString.QueryInterface(IString, pString);

			pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
			pSignificantPoint.NavaidSystemId := pString;

			CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
			pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
			pSignificantPoint.Position := pAixmPoint;
		end
		else
		begin

			pcFixDesignatedPoint := CoDesignatedPoint.Create;
			pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

			if bOnNav then
			begin
				pFixDesignatedPoint.ID := sigPoint.ID;
				pFixDesignatedPoint.Name := sigPoint.Name;
			end
			else
			begin
				pFixDesignatedPoint.ID := '0';
				pFixDesignatedPoint.Designator := 'COORD';
				pFixDesignatedPoint.Name := 'MAPT';
			end;

			pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

			pcAixmPoint := CoAixmPoint.Create;
			pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

			ptTmp := GGeoOperators.geoTransformations (pFromPoint, GPrjSR, GGeoSR).AsPoint;
			pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
            ptTmp.Free;

			pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag

			pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
		end
	end
	else
	begin
        pTerminalSegmentPoint.Role := ProcedureFixRoleType_FTP;
		pcFixDesignatedPoint := CoDesignatedPoint.Create;
		pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'FicTHR';

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

		ptTmp := GGeoOperators.geoTransformations (pFromPoint, GPrjSR, GGeoSR).AsPoint;
		pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
		ptTmp.Free;

		pFixDesignatedPoint.Point := pAixmPoint;

	//	pFixDesignatedPoint.Note
	//	pFixDesignatedPoint.Tag

		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;
// End of StartPoint ========================


////////////////////////////////////////////////////////////////////////////////
// EndPoint ====================================================================
////////////////////////////////////////////////////////////////////////////////
	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);
	pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAPT;

	bOnNav := (Continued = 2) and rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

	if bOnNav Then
    	sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

	if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
	begin
		pcString := CoStringClass.Create;
        pcString.QueryInterface(IString, pString);
        pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
		pSignificantPoint.NavaidSystemId := pString;

		CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
        pSignificantPoint.Position := pAixmPoint;
    end
    else
    begin
    	pcFixDesignatedPoint := CoDesignatedPoint.Create;
        pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		if bOnNav then
        begin
        	pFixDesignatedPoint.ID := sigPoint.ID;
			pFixDesignatedPoint.Name := sigPoint.Name;
        end
        else
        begin
        	pFixDesignatedPoint.ID := '0';
			pFixDesignatedPoint.Designator := 'COORD';
			if Continued <> 2 then
			begin
				pFixDesignatedPoint.Name := '';
				pTerminalSegmentPoint.Role := 14;//ProcedureFixRoleType_OTHER;
			end
			else
				pFixDesignatedPoint.Name := 'MAPT';
		end;

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

		ptTmp := GGeoOperators.geoTransformations (pToPoint, GPrjSR, GGeoSR).AsPoint;
		pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
		ptTmp.Free;

		pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
        pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint ========================

// Trajectory				a===================================================
	pcAIXMCurve := CoAIXMCurve.Create;
	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

	pcGMLPolyline := CoGMLPolyline.Create;
	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

		pcGMLPart := CoGMLPart.Create;
		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

            ptTmp := GGeoOperators.geoTransformations (pFromPoint, GPrjSR, GGeoSR).AsPoint;
			pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
            ptTmp.Free;

			pGMLPart.Add(pGMLPoint);

			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

            ptTmp := GGeoOperators.geoTransformations (pToPoint, GPrjSR, GGeoSR).AsPoint;
			pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
            ptTmp.Free;
			pGMLPart.Add(pGMLPoint);

		pGMLPolyline.Add(pGMLPart);

	pAIXMCurve.Polyline := pGMLPolyline;
	pGMLPolyline := nil;
	pSegmentLeg.Trajectory := pAIXMCurve;
    pAIXMCurve := nil;
// END						a===================================================
end;

function TMainForm.MissedApproachStraight(NO_SEQ, Continued: Integer): IApproachLeg;
var
	I, J, K:				Integer;

	Gradient,
//	hMATF,
//	fFinalAprLength,
//	fRefHeight,
//	MAPtDistVerLower,
//	MAPtDistToTHR,
	fTmp:					Double;
	ptTmp, pFromPoint:		TPoint;
//	HaveTP,
	bOnNav:					Boolean;

	sigPoint:				TSignificantPoint;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pcDistance:				Distance;
	pcFixDesignatedPoint:	DesignatedPoint;
	pcSignificantPoint:		SignificantPoint;
	pcSpeed:				Speed;
	pcAixmPoint:			AixmPoint;
	pcAIXMCurve:			AIXMCurve;
	pcGMLPolyline:			GMLPolyline;
	pcGMLPart:				GMLPart;
	pcGMLPoint:				GMLPoint;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pTerminalSegmentPoint:	ITerminalSegmentPoint;
	pDistance:				IDistance;

	pSegmentPoint:			ISegmentPoint;
	pFixDesignatedPoint:	IDesignatedPoint;
	pSignificantPoint:		ISignificantPoint;
    pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pInt32:					IInt32;
	pcInt32:				Int32Class;
	pcDouble:				DoubleClass;
	pDouble:				IDouble;

	pcString:				StringClass;
	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pAixmPoint:				IAixmPoint;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
//	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	pAIXMCurve:				IAIXMCurve;
	pGMLPolyline:			IGMLPolyline;
	pGMLPart:				IGMLPart;
	pGMLPoint:				IGMLPoint;
begin
	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
//	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_ABOVE_LOWER;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;

	if cbTurnSide.ItemIndex = 0 then
		pSegmentLeg.TurnDirection := DirectionTurnType_LEFT
	else
		pSegmentLeg.TurnDirection := DirectionTurnType_RIGHT;

	pSegmentLeg.LegPath := TrajectoryType_OTHER;

//	MAPt
//	Altitude
//	MATF

	if cbTurnAt.ItemIndex = 1 then
		pSegmentLeg.LegTypeARINC := SegmentPathType_CA
	else
		pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

//    pSegmentLeg.AltitudeOverrideATC =
//    pSegmentLeg.AltitudeOverrideReference =
//    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
//    pSegmentLeg.EndConditionDesignator =
//    pSegmentLeg.Note
//    pSegmentLeg.ProcedureTurnRequired
//    pSegmentLeg.ReqNavPerformance
//    pSegmentLeg.SpeedInterpretation =
//    pSegmentLeg.ReqNavPerformance

//        pSegmentLeg.CourseDirection =
//        pSegmentLeg.ProcedureTransition
//        pSegmentLeg.StartPoint
//=======================================================================================

	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	fTmp := CConverters[puDirAngle].ConvertFunction (FFAF.OutDirection, cdToOuter, FFAF.PrjPt);
	pDouble.Value := fTmp;
	pSegmentLeg.Course := pDouble;
//=======================================================================================
	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);
	pDouble.Value := DegToRad(FMATF.Bank);
	pSegmentLeg.BankAngle := pDouble;
//=======================================================================================
	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);

	pDistanceVertical.Uom := mVUomDistanceV;
	pDistanceVertical.Value := editFinalTurnAltitude.Text;
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;
//======
//	pcDistance := CoDistance.Create;
//	pcDistance.QueryInterface(IDistance, pDistance);
//	pDistance.Uom := mVUomDistance;

//	fTmp := CConverters[puAltitude].ConvertFunction (fTmp, cdToOuter, nil);
//	pDistance.Value := RoundToStr(fTmp, GAltitudeAccuracy);
//	pSegmentLeg.UpperLimitAltitude := pDistance;
//======
	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mHUomDistance;

	//GUI.DrawPolyline(FMAPtLeg.NomTrack, 0, 3);	//?????????????????????????

	//fTmp := CConverters[puDistance].ConvertFunction(FMAPtLeg.NomTrack.Length, cdToOuter, nil);
	fTmp := CConverters[puDistance].ConvertFunction(ReturnDistanceInMeters(FMAPt.PrjPt, FMATF.PrjPt), cdToOuter, nil);

	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;
//======
	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then exit;
	Gradient := GrTable[K];

//FMAPt.Gradient := Gradient;

	pDouble.Value := RadToDeg(ArcTan(Gradient));
	pSegmentLeg.VerticalAngle := pDouble;
//======

	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FMATF.IAS, cdToOuter, nil);	//prmMAIAS.Value

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;

////////////////////////////////////////////////////////////////////////////////
// StartPoint ==================================================================
////////////////////////////////////////////////////////////////////////////////
//   0 ' Far To THR
//   1 ' In THR Range
//   2 ' Ahead THR

//    If Continued <> 2 Then
//        Set pFromPoint = PtSOC
//    Else
//        Set pFromPoint = MAPtPrj
//    End If

//	if Continued = 2 then
//		pFromPoint := FMAPt.GeoPt
//	else
//		pFromPoint := FMAPt.SOCGeoPt;


	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

	pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAPT;

	bOnNav := (Continued = 2) and rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

	if bOnNav Then
		sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

	if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
	begin
		pcString := CoStringClass.Create;
		pcString.QueryInterface(IString, pString);
		pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
		pSignificantPoint.NavaidSystemId := pString;

		CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
		pSignificantPoint.Position := pAixmPoint;
		pFromPoint := sigPoint.Prj;
	end
	else
	begin
		pcFixDesignatedPoint := CoDesignatedPoint.Create;
		pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

		if bOnNav then
		begin
			ptTmp := sigPoint.Geo;
			pFromPoint := sigPoint.Prj;
			pFixDesignatedPoint.ID := sigPoint.ID;
			pFixDesignatedPoint.Name := sigPoint.Name;
		end
		else
		begin
			pFixDesignatedPoint.ID := '0';
			pFixDesignatedPoint.Designator := 'COORD';
			if Continued = 2 then
			begin
				ptTmp := FMAPt.GeoPt;
				pFromPoint := FMAPt.PrjPt;
				pFixDesignatedPoint.Name := 'MAPT';
			end
			else
			begin
				ptTmp := FMAPt.SOCGeoPt;
				pFromPoint := FMAPt.SOCPrjPt;
				pFixDesignatedPoint.Name := '';
				pTerminalSegmentPoint.Role := 14;//ProcedureFixRoleType_OTHER;
			end
		end;

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

		pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
		pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;
// End of StartPoint ========================

////////////////////////////////////////////////////////////////////////////////
// EndPoint ====================================================================
////////////////////////////////////////////////////////////////////////////////
	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

	pTerminalSegmentPoint.Role := ProcedureFixRoleType_TP;
//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := FMATF.FlyMode = fmFlyOver;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);
{
	bOnNav := (Continued = 1)and rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

	if bOnNav Then
		sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

	if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
	begin
		pcString := CoStringClass.Create;
		pcString.QueryInterface(IString, pString);
		pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
		pSignificantPoint.NavaidSystemId := pString;

		CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
		pSignificantPoint.Position := pAixmPoint;
	end
	else
	begin
}
		pcFixDesignatedPoint := CoDesignatedPoint.Create;
		pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

{		if bOnNav then
		begin
			pFixDesignatedPoint.ID := sigPoint.ID;
			pFixDesignatedPoint.Name := sigPoint.Name;
		end
		else }
		begin
			pFixDesignatedPoint.ID := '0';
			pFixDesignatedPoint.Designator := 'COORD';
			pFixDesignatedPoint.Name := 'MATF';
		end;

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
		pAixmPoint.AsIGMLPoint.PutCoord(FMATF.GeoPt.X, FMATF.GeoPt.Y);

		pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
//	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint ========================

// Trajectory				a===================================================
	pcAIXMCurve := CoAIXMCurve.Create;
	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

	pcGMLPolyline := CoGMLPolyline.Create;
	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

	FMAPtLeg.NomTrack.Part[0].ReplacePoint(0, pFromPoint);

	for J := 0 to FMAPtLeg.NomTrack.Count - 1 do
	begin
		pcGMLPart := CoGMLPart.Create;
		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

		for I := 0 to FMAPtLeg.NomTrack.Part[J].Count - 1 do
		begin
			pcGMLPoint := CoGMLPoint.Create;
			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

			ptTmp := GGeoOperators.geoTransformations (FMAPtLeg.NomTrack.Part[J].Point[I], GPrjSR, GGeoSR).AsPoint;
			pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
			pGMLPart.Add(pGMLPoint);
			ptTmp.Free;
		end;

		pGMLPolyline.Add(pGMLPart);
	end;

	pAIXMCurve.Polyline := pGMLPolyline;
	pGMLPolyline := nil;
	pSegmentLeg.Trajectory := pAIXMCurve;
	pAIXMCurve := nil;
// END						a===================================================

// Trajectory				a===================================================
//	pcAIXMCurve := CoAIXMCurve.Create;
//	pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

//	pcGMLPolyline := CoGMLPolyline.Create;
//	pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

//		pcGMLPart := CoGMLPart.Create;
//		pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

//			pcGMLPoint := CoGMLPoint.Create;
//			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

////			if bOnNav then
////				pGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y)
////			else if Continued = 1 then
////				pGMLPoint.PutCoord(FMAPt.SOCGeoPt.X, FMAPt.SOCGeoPt.Y)
////			else
////				pGMLPoint.PutCoord(FMAPt.GeoPt.X, FMAPt.GeoPt.Y);

//			pGMLPart.Add(pGMLPoint);

//			pcGMLPoint := CoGMLPoint.Create;
//			pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

//			pGMLPoint.PutCoord(FMATF.GeoPt.X, FMATF.GeoPt.Y);
//			pGMLPart.Add(pGMLPoint);

//		pGMLPolyline.Add(pGMLPart);

//	pAIXMCurve.Polyline := pGMLPolyline;
//		pGMLPolyline := nil;
//	pSegmentLeg.Trajectory := pAIXMCurve;
//		pAIXMCurve := nil;
// END						a===================================================
end;

function TMainForm.MissedApproachTermination(NO_SEQ, Continued: Integer): IApproachLeg;
var
	I, J, K:				Integer;

	fTermAlt,
	Gradient,
//	hMATF,
//	fFinalAprLength,
//	fRefHeight,
//	MAPtDistVerLower,
//	MAPtDistToTHR,
	fTmp:					Double;
	ptTmp:					TPoint;
	HaveTP,
	bOnNav:					Boolean;

	sigPoint:				TSignificantPoint;

	pcTerminalSegmentPoint:	TerminalSegmentPoint;
	pcDistance:				Distance;
	pcFixDesignatedPoint:	DesignatedPoint;
	pcSignificantPoint:		SignificantPoint;
	pcSpeed:				Speed;
	pcAixmPoint:			AixmPoint;
	pcAIXMCurve:			AIXMCurve;
	pcGMLPolyline:			GMLPolyline;
	pcGMLPart:				GMLPart;
	pcGMLPoint:				GMLPoint;

	pApproachLeg:			ApproachLeg;
	pSegmentLeg:			ISegmentLeg;

	pTerminalSegmentPoint:	ITerminalSegmentPoint;
	pDistance:				IDistance;

	pSegmentPoint:			ISegmentPoint;
	pFixDesignatedPoint:	IDesignatedPoint;
	pSignificantPoint:		ISignificantPoint;
    pcDistanceVertical:		DistanceVertical;
	pDistanceVertical:		IDistanceVertical;

	pInt32:					IInt32;
	pcInt32:				Int32Class;
	pcDouble:				DoubleClass;
	pDouble:				IDouble;

	pcString:				StringClass;
	pString:				IString;

	pcBoolean:				BooleanClass;
	pBoolean:				IBoolean;

	pAixmPoint:				IAixmPoint;
	pSpeed:					ISpeed;

	mHUomDistance:			UomDistance;
//	mVUomDistance:			UomDistance;
	mVUomDistanceV:			UomDistanceVertical;
	mUomSpeed:				UomSpeed;
//	mVUomSpeed:				UomSpeed;

	pAIXMCurve:				IAIXMCurve;
	pGMLPolyline:			IGMLPolyline;
	pGMLPart:				IGMLPart;
	pGMLPoint:				IGMLPoint;
begin
	HaveTP := cbGoToTurn.Checked;
//	HaveIntercept :=

	mHUomDistance := uomDistHorzTab[GHorisontalDistOut];
//	mVUomDistance := uomDistVerH_Tab[GVerticalDistOut];
	mVUomDistanceV := uomDistVerTab[GVerticalDistOut];
	mUomSpeed := uomSpeedTab[GHorisontalSpeedOut];
//	mVUomSpeed := uomVerSpeedTab[GVerticalSpeedOut];

	pApproachLeg := CoApproachLeg.Create;
	pApproachLeg.QueryInterface(ISegmentLeg, pSegmentLeg);
	pApproachLeg.QueryInterface(IApproachLeg, Result);

	//pSegmentLeg := pApproachLeg.AsISegmentLeg;

	pSegmentLeg.AltitudeInterpretation := AltitudeUseType_ABOVE_LOWER;
	pSegmentLeg.UpperLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.LowerLimitReference := VerticalReferenceType_MSL;
	pSegmentLeg.TurnDirection := DirectionTurnType_EITHER;
	pSegmentLeg.CourseType := CourseType_TRUE_TRACK;

//	pSegmentLeg.LegPath := TrajectoryType_OTHER;
// ======================================================================
	pcInt32 := CoInt32Class.Create;
	pcInt32.QueryInterface(IInt32, pInt32);
	pInt32.Value := NO_SEQ;
	pSegmentLeg.SeqNumberARINC := pInt32;

////////////////////////////////////////////////////////////////////////////////
// StartPoint ==================================================================
////////////////////////////////////////////////////////////////////////////////
	pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
	pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);

//		pTerminalSegmentPoint.IndicatorFACF =      ??????????
//		pTerminalSegmentPoint.LeadDME =            ??????????
//		pTerminalSegmentPoint.LeadRadial =         ??????????

	pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := FMATF.FlyMode = fmFlyOver;
	pSegmentPoint.FlyOver := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.RadarGuidance := pBoolean;

	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	pBoolean.Value := False;
	pSegmentPoint.Waypoint := pBoolean;

	pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;
//////=====
	pcSignificantPoint := CoSignificantPoint.Create;
	pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);
	pcFixDesignatedPoint := CoDesignatedPoint.Create;
	pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

	if HaveTP then
	begin
		pTerminalSegmentPoint.Role := ProcedureFixRoleType_TP;

		pFixDesignatedPoint.ID := '0';
		pFixDesignatedPoint.Designator := 'COORD';
		pFixDesignatedPoint.Name := 'MATF';

		pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

		pcAixmPoint := CoAixmPoint.Create;
		pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

		pAixmPoint.AsIGMLPoint.PutCoord(FMATF.GeoPt.X, FMATF.GeoPt.Y);

		pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
		pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
	end
	else
	begin
		pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAPT;

		bOnNav := (Continued = 2)and rbMAPtFromList.Checked and (cbMAPtList.ItemIndex >=0);

		if bOnNav Then
			sigPoint := TSignificantPoint(cbMAPtList.Items.Objects[cbMAPtList.ItemIndex]);

		if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
		begin
			pcString := CoStringClass.Create;
			pcString.QueryInterface(IString, pString);
			pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
			pSignificantPoint.NavaidSystemId := pString;

			CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
			pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
			pSignificantPoint.Position := pAixmPoint;
		end
		else
		begin
			pcFixDesignatedPoint := CoDesignatedPoint.Create;
			pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

			if bOnNav then
			begin
				pFixDesignatedPoint.ID := sigPoint.ID;
				pFixDesignatedPoint.Name := sigPoint.Name;
				ptTmp := sigPoint.Geo;
			end
			else
			begin
				pFixDesignatedPoint.ID := '0';
				pFixDesignatedPoint.Designator := 'COORD';
				if Continued = 2 then
				begin
					ptTmp := FMAPt.GeoPt;
					pFixDesignatedPoint.Name := 'MAPT';
				end
				else
				begin
					ptTmp := FMAPt.SOCGeoPt;
					pFixDesignatedPoint.Name := '';
					pTerminalSegmentPoint.Role := ProcedureFixRoleType_OTHER;
				end
			end;

			pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

			pcAixmPoint := CoAixmPoint.Create;
			pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);

			pAixmPoint.AsIGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
			pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
			pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
		end;
	end;

	pSegmentPoint.PointChoice := pSignificantPoint;
	pSegmentLeg.StartPoint := pTerminalSegmentPoint;
// End of StartPoint ========================

	if HaveTP then
	begin
		pSegmentLeg.LegPath := TrajectoryType_OTHER;

		if rgToFIX.ItemIndex = 0 then
		begin
			pSegmentLeg.LegTypeARINC := SegmentPathType_DF;
		end
		else
		begin
			pSegmentLeg.LegTypeARINC := SegmentPathType_CF;

			pcDouble := CoDoubleClass.Create;
			pcDouble.QueryInterface(IDouble, pDouble);

			pDouble.Value := CConverters[puDirAngle].ConvertFunction (prmTurnCourse.Value, cdToOuter, FMAHF.PrjPt);
			pSegmentLeg.Course := pDouble;
		end;

////////////////////////////////////////////////////////////////////////////////
// EndPoint ====================================================================
////////////////////////////////////////////////////////////////////////////////

		pcTerminalSegmentPoint := CoTerminalSegmentPoint.Create;
		pcTerminalSegmentPoint.QueryInterface(ITerminalSegmentPoint, pTerminalSegmentPoint);
		pTerminalSegmentPoint.Role := ProcedureFixRoleType_MAHF;

//			pTerminalSegmentPoint.IndicatorFACF =      ??????????
//			pTerminalSegmentPoint.LeadDME =            ??????????
//			pTerminalSegmentPoint.LeadRadial =         ??????????

		pcTerminalSegmentPoint.QueryInterface(ISegmentPoint, pSegmentPoint);

		pcBoolean := CoBooleanClass.Create;
		pcBoolean.QueryInterface(IBoolean, pBoolean);
		pBoolean.Value := False;
		pSegmentPoint.FlyOver := pBoolean;

		pcBoolean := CoBooleanClass.Create;
		pcBoolean.QueryInterface(IBoolean, pBoolean);
		pBoolean.Value := False;
		pSegmentPoint.RadarGuidance := pBoolean;

		pcBoolean := CoBooleanClass.Create;
		pcBoolean.QueryInterface(IBoolean, pBoolean);
		pBoolean.Value := False;
		pSegmentPoint.Waypoint := pBoolean;

		pSegmentPoint.ReportingATC := ATCReportingType_NO_REPORT;
//////=====//===================================================================
		pcSignificantPoint := CoSignificantPoint.Create;
		pcSignificantPoint.QueryInterface(ISignificantPoint, pSignificantPoint);

		bOnNav := rbExistingPoint.Checked and (cbTurnWPT.ItemIndex >=0);

		if bOnNav Then
			sigPoint := TSignificantPoint(cbTurnWPT.Items.Objects[cbTurnWPT.ItemIndex]);

		if bOnNav and ((sigPoint.AIXMType = atVOR)or(sigPoint.AIXMType = atNDB)) then
		begin
			pcString := CoStringClass.Create;
			pcString.QueryInterface(IString, pString);
			pString.Value := sigPoint.ID + ';' + AIXMTypeNames[sigPoint.AIXMType];
			pSignificantPoint.NavaidSystemId := pString;

			CoAixmPoint.Create.QueryInterface(IAixmPoint, pAixmPoint);
			pAixmPoint.AsIGMLPoint.PutCoord(sigPoint.Geo.X, sigPoint.Geo.Y);
			pSignificantPoint.Position := pAixmPoint;
		end
		else
		begin
			pcFixDesignatedPoint := CoDesignatedPoint.Create;
			pcFixDesignatedPoint.QueryInterface(IDesignatedPoint, pFixDesignatedPoint);

			if bOnNav then
			begin
				pFixDesignatedPoint.ID := sigPoint.ID;
				pFixDesignatedPoint.Name := sigPoint.Name;
			end
			else
			begin
				pFixDesignatedPoint.ID := '0';
				pFixDesignatedPoint.Designator := 'COORD';
				pFixDesignatedPoint.Name := 'MAHF';
			end;

			pFixDesignatedPoint.Type_ := DesignatedPointType_DESIGNED;

			pcAixmPoint := CoAixmPoint.Create;
			pcAixmPoint.QueryInterface(IAixmPoint, pAixmPoint);
			pAixmPoint.AsIGMLPoint.PutCoord(FMAHF.GeoPt.X, FMAHF.GeoPt.Y);

			pFixDesignatedPoint.Point := pAixmPoint;

		//	pFixDesignatedPoint.Note
		//	pFixDesignatedPoint.Tag
			pSignificantPoint.FixDesignatedPoint := pFixDesignatedPoint;
		end;

		pSegmentPoint.PointChoice := pSignificantPoint;
		pSegmentLeg.EndPoint := pTerminalSegmentPoint;
// End of EndPoint ========================
	end
	else
	begin
		pSegmentLeg.LegTypeARINC := SegmentPathType_CA;
		pSegmentLeg.LegPath := TrajectoryType_STRAIGHT;

		pcDouble := CoDoubleClass.Create;
		pcDouble.QueryInterface(IDouble, pDouble);

		pDouble.Value := CConverters[puDirAngle].ConvertFunction (FFAF.OutDirection, cdToOuter, FFAF.PrjPt);
		pSegmentLeg.Course := pDouble;
	end;

//=======================================================================================
	if HaveTP then
	begin
		fTermAlt := StrToFloat(editFinalTurnAltitude.Text);
	end
	else
	begin
		fTermAlt := CConverters[puAltitude].ConvertFunction (FMAHF.Altitude, cdToOuter, nil);
	end;

	pcDistanceVertical := CoDistanceVertical.Create;
	pcDistanceVertical.QueryInterface(IDistanceVertical, pDistanceVertical);
	pDistanceVertical.Uom := mVUomDistanceV;

	pDistanceVertical.Value := RoundToStr(fTermAlt, GAltitudeAccuracy);
	pSegmentLeg.LowerLimitAltitude := pDistanceVertical;
//====================================================================================================
	pcDouble := CoDoubleClass.Create;
	pcDouble.QueryInterface(IDouble, pDouble);

	K := cbMAclimbGradient.ItemIndex;
	if K < 0 then
		K := 0;

	Gradient := GrTable[K];

	pDouble.Value := RadToDeg(ArcTan(Gradient));
	pSegmentLeg.VerticalAngle := pDouble;
//====================================================================================================

	pcSpeed := CoSpeed.Create;
	pcSpeed.QueryInterface(ISpeed, pSpeed);
	pSpeed.Uom := mUomSpeed;

	if HaveTP then	//FMAHF.IAS
		pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FMATF.IAS, cdToOuter, nil)
	else
		pSpeed.Value := CConverters[puHSpeed].ConvertFunction(FMAPt.IAS, cdToOuter, nil);

	pSegmentLeg.SpeedLimit := pSpeed;
	pSegmentLeg.SpeedReference := SpeedReferenceType_IAS;

//====================================================================================================

	if HaveTP then
	begin
		fTmp := CConverters[puDistance].ConvertFunction(prmNWPTFromTPDist.Value, cdToOuter, nil);
	end
	else
	begin
		fTmp := CConverters[puDistance].ConvertFunction(prmMAPtDistance.Value, cdToOuter, nil);
	end;

	pcDistance := CoDistance.Create;
	pcDistance.QueryInterface(IDistance, pDistance);
	pDistance.Uom := mHUomDistance;

	pDistance.Value := RoundToStr(fTmp, GDistanceAccuracy);
	pSegmentLeg.Length := pDistance;

// Trajectory				a===================================================
	if HaveTP then
	begin
		pcAIXMCurve := CoAIXMCurve.Create;
		pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

		pcGMLPolyline := CoGMLPolyline.Create;
		pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

		for J := 0 to FMATFLeg.NomTrack.Count - 1 do
		begin
			pcGMLPart := CoGMLPart.Create;
			pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

			for I := 0 to FMATFLeg.NomTrack.Part[J].Count - 1 do
			begin
				pcGMLPoint := CoGMLPoint.Create;
				pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

				ptTmp := GGeoOperators.geoTransformations (FMATFLeg.NomTrack.Part[J].Point[I], GPrjSR, GGeoSR).AsPoint;
				pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
				pGMLPart.Add(pGMLPoint);
				ptTmp.Free;
			end;

			pGMLPolyline.Add(pGMLPart);
		end;

		pAIXMCurve.Polyline := pGMLPolyline;
		pGMLPolyline := nil;
		pSegmentLeg.Trajectory := pAIXMCurve;
		pAIXMCurve := nil;
// END						a===================================================
	end
	else
	begin
// Trajectory				a===================================================
		pcAIXMCurve := CoAIXMCurve.Create;
		pcAIXMCurve.QueryInterface(IAIXMCurve, pAIXMCurve);

		pcGMLPolyline := CoGMLPolyline.Create;
		pcGMLPolyline.QueryInterface(IGMLPolyline, pGMLPolyline);

			pcGMLPart := CoGMLPart.Create;
			pcGMLPart.QueryInterface(IGMLPart, pGMLPart);

				pcGMLPoint := CoGMLPoint.Create;
				pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

				if Continued = 1 then
					pGMLPoint.PutCoord(FMAPt.SOCGeoPt.X, FMAPt.SOCGeoPt.Y)
				else
					pGMLPoint.PutCoord(FMAPt.GeoPt.X, FMAPt.GeoPt.Y);

				pGMLPart.Add(pGMLPoint);

				pcGMLPoint := CoGMLPoint.Create;
				pcGMLPoint.QueryInterface(IGMLPoint, pGMLPoint);

				pGMLPoint.PutCoord(FMAHF.GeoPt.X, FMAHF.GeoPt.Y);
				pGMLPart.Add(pGMLPoint);

			pGMLPolyline.Add(pGMLPart);

		pAIXMCurve.Polyline := pGMLPolyline;
		pGMLPolyline := nil;
		pSegmentLeg.Trajectory := pAIXMCurve;
		pAIXMCurve := nil;
// END						a===================================================
	end;
end;
{
function TMainForm.MissedApproachTerminationContinued(NO_SEQ: Integer): IApproachLeg;
begin
	result := nil;
end;
}
procedure TMainForm.SaveProcedure;
var
	I, J, N, NO_SEQ,
	Continued:					Integer;
	HaveTP:						Boolean;

	pApproachLeg:				IApproachLeg;
//	pSegmentLeg:				ISegmentLeg;

	pApproachLegList:			ApproachLegList;
	pSegmentLegList:			ISegmentLegList;

	pcTransition:				ProcedureTransition;
	pTransition:				IProcedureTransition;

	pcProcedure:				InstrumentApproachProcedure;
	pProcedure:					IAIXMProcedure;

	cIsLimitedTo:				AircraftCharacteristic;
	IsLimitedTo:				IAircraftCharacteristic;

	pcString:					StringClass;
	pString:					IString;

	pcBoolean:					BooleanClass;
	pBoolean:					IBoolean;

	ptFAFDescent:				TPoint;

//	pIAPProfile:				IAPProfile;
//	pDistanceVertical:			IDistanceVertical;
//	pDouble:					IDouble;

//bu hisseye yeniden baxilmalidi
{
	pcObjectDir:				ARANObjectDirectory_;
	pObjectDir:					IObjectDirectory;
 }

 //end
	BranchLegs:					TList;
	Leg:						TLeg;

	sTmp,
	sProcName:					string;
	mapFileName:				string;
//	mHUomDistance:			string;
//	mVUomDistance:			string;
begin
	if rbIAPCH_GNSS.Checked then
		sTmp := rbIAPCH_GNSS.Caption
	else
		sTmp := rbIAPCH_DME_DME.Caption;

	sProcName :=	'RNAV '	+ sTmp +
					' RWY'	+ cbRwyDirection.Text + ' Cat ' + cbCategory.Text;

//	HaveSDF := cbSDF.Checked;
	HaveTP := cbGoToTurn.Checked;

////////////////////////////////////////////////////////////////////////////////
// Procedure ===================================================================
////////////////////////////////////////////////////////////////////////////////
	pcProcedure := CoInstrumentApproachProcedure.Create;    //CoAIXMProcedure.Create;
	pcProcedure.QueryInterface(IAIXMProcedure, pProcedure);

//	pProcedure.CommunicationFailureDescription
//	pProcedure.Description =        Robot Building for Dummies Book
//	pProcedure.FlightChecked =
//	pProcedure.ID

	pProcedure.CodingStandard := ProcedureCodingStandardType_ARINC_424_15;
	pProcedure.DesignCriteria := DesignStandardType_PANS_OPS;

	cIsLimitedTo := CoAircraftCharacteristic.Create;
	cIsLimitedTo.QueryInterface(IAircraftCharacteristic, IsLimitedTo);

	case FAirCraftCategory of
	acA:
		IsLimitedTo.AircraftLandingCategory := AircraftCategoryType_A;
	acB:
		IsLimitedTo.AircraftLandingCategory := AircraftCategoryType_B;
	acC:
		IsLimitedTo.AircraftLandingCategory := AircraftCategoryType_C;
	acD, acDL:
		IsLimitedTo.AircraftLandingCategory := AircraftCategoryType_D;
	acE:
		IsLimitedTo.AircraftLandingCategory := AircraftCategoryType_E;
	end;

	pProcedure.IsLimitedTo := IsLimitedTo;

	pProcedure.Name := sProcName;
	//	pProcedure.Note =
	//	pProcedure.ProtectsSafeAltitudeAreaId =
	pcBoolean := CoBooleanClass.Create;
	pcBoolean.QueryInterface(IBoolean, pBoolean);
	//	pBoolean := CoBooleanClass.Create;
	pBoolean.Value := False;

	pProcedure.RNAV := pBoolean;
	pProcedure.ServicesAirportId := FAerodrome.ID;
    
////////////////////////////////////////////////////////////////////////////////
// Initial transitions /////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

	for J := 0 to FIABranchs.Count - 1 do
	begin
		BranchLegs :=  FIABranchs.Items[J];

		pApproachLegList := CoApproachLegList.Create;
		pApproachLegList.QueryInterface(ISegmentLegList, pSegmentLegList);

		N := BranchLegs.Count - 2;

		for I := 0 to N do
		begin

			Leg := TLeg(BranchLegs.Items[I]);
			pApproachLeg := InitialApproachLeg(I + 1, Leg);
			pSegmentLegList.AsIIAPList.Add(pApproachLeg);
		end;

		pcTransition := CoProcedureTransition.Create;
		pcTransition.QueryInterface(IProcedureTransition, pTransition);
		pTransition.LegList := pSegmentLegList;

		pcString := CoStringClass.Create;
		pcString.QueryInterface(IString, pString);
		pString.Value := FRWYDirection.ID;
		pTransition.RunwayDirectionId := pString;

		//	pTransition.TransitionId := TextBox0410.Text

		pTransition.type_ := ProcedurePhaseType_APPROACH;
		pProcedure.TransitionList.Add(pTransition);
	end;

////////////////////////////////////////////////////////////////////////////////
// Final Approach Legs =========================================================
////////////////////////////////////////////////////////////////////////////////

	pApproachLegList := CoApproachLegList.Create;
	pApproachLegList.QueryInterface(ISegmentLegList, pSegmentLegList);
	NO_SEQ := 0;

////////////////////////////////////////////////////////////////////////////////
//Leg 1 Intermediate Approach ==================================================
////////////////////////////////////////////////////////////////////////////////

//	Inc(NO_SEQ);
//	pApproachLeg := IntermediateApproachLeg(NO_SEQ);
//	pSegmentLegList.AsIIAPList.Add(pApproachLeg);

//Leg 2 Final Approach To SDF :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=
	ptFAFDescent := nil;
{	If HaveSDF Then
		NO_SEQ := NO_SEQ + 1
		Set pApproachLeg := FinalApproachToSDFLeg(NO_SEQ, ptFAFDescent)
		pSegmentLegList.AsIIAPList.Add pApproachLeg
	End If
}
//Leg 3 Final Approach :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
	Inc(NO_SEQ);
	pApproachLeg := FinalApproachLeg(NO_SEQ, ptFAFDescent, Continued);
	pSegmentLegList.AsIIAPList.Add(pApproachLeg);

//Leg 4 Straight Missed Approach :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:

	if Continued <> 0 then
	begin
		Inc(NO_SEQ);
		pApproachLeg := FinalApproachContinuedLeg(NO_SEQ, Continued);
		pSegmentLegList.AsIIAPList.Add(pApproachLeg);
	end;

// Final Approach Transition :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
	pcTransition := CoProcedureTransition.Create;
	pcTransition.QueryInterface(IProcedureTransition, pTransition);

//    pTransition.Description :=
//    pTransition.ID :=

	pTransition.LegList := pSegmentLegList;

//    pTransition.Procedure :=

//	ProcedureTransition

	pcString := CoStringClass.Create;
	pcString.QueryInterface(IString, pString);
	pString.Value := FRWYDirection.ID;
	pTransition.RunwayDirectionId := pString;

//    pTransition.TransitionId := TextBox0410.Text

	pTransition.type_ := ProcedurePhaseType_FINAL;
	pProcedure.TransitionList.Add(pTransition);

// Missed Approach Legs :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=
	pApproachLegList := CoApproachLegList.Create;
	pApproachLegList.QueryInterface(ISegmentLegList, pSegmentLegList);

	NO_SEQ := 0;

//Leg 1 Straight Missed Approach :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:

//	if HaveTP then
	if HaveTP and (cbTurnAt.ItemIndex <> 0) then
	begin
		Inc(NO_SEQ);
		pApproachLeg := MissedApproachStraight(NO_SEQ, Continued);
		pSegmentLegList.AsIIAPList.Add(pApproachLeg);
	end;

//Leg 2 Missed Approach Termination :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=
	Inc(NO_SEQ);
	pApproachLeg := MissedApproachTermination(NO_SEQ, Continued);
	pSegmentLegList.AsIIAPList.Add(pApproachLeg);
//Leg 3 Missed Approach Termination Continued :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=

{
	HaveIntercept := OptionButton1202.Value
	if HaveIntercept then
	begin
		Inc(NO_SEQ);
		pApproachLeg := MissedApproachTerminationContinued(NO_SEQ);
		pSegmentLegList.AsIIAPList.Add(pApproachLeg);
	end;
}
// Missed Transition :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
	pcTransition := CoProcedureTransition.Create;
	pcTransition.QueryInterface(IProcedureTransition, pTransition);

//	pTransition.Description :=
//	pTransition.ID :=
//	pTransition.TransitionId := TextBox0???.Text

	pTransition.LegList := pSegmentLegList;

//	pTransition.Procedure :=
	pcString := CoStringClass.Create;
	pcString.QueryInterface(IString, pString);

	pString.Value := FRWYDirection.ID;
	pTransition.RunwayDirectionId := pString;

	pTransition.Type_ := ProcedurePhaseType_MISSED;
	pProcedure.TransitionList.Add(pTransition);


//	SaveTrackPoints(FileName, pTransition);
{
//Transition III :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
	If Not HaveFAF Then
//Legs :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
		Set pSegmentLegList := New ApproachLegList
		NO_SEQ := 0

//Leg 1 Intermediate Approach :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=
		If OptionButton0601.Value Then
			Inc(NO_SEQ);
			Set pApproachLeg := RacetrackLeg(NO_SEQ)
			pSegmentLegList.AsIIAPList.Add pApproachLeg
		ElseIf OptionButton0602.Value Then
			Inc(NO_SEQ);

			Set pApproachLeg := BaseTurnOutboundLeg(NO_SEQ)
			pSegmentLegList.AsIIAPList.Add pApproachLeg

			Inc(NO_SEQ);
			Set pApproachLeg := BaseTurnInbound(NO_SEQ, ptFAFDescent)
			pSegmentLegList.AsIIAPList.Add pApproachLeg
		Else
			Inc(NO_SEQ);

			Set pApproachLeg := ProcTurnOutboundLeg(NO_SEQ)
			pSegmentLegList.AsIIAPList.Add pApproachLeg

			Inc(NO_SEQ);
			Set pApproachLeg := ProcTurnInbound(NO_SEQ, ptFAFDescent)
			pSegmentLegList.AsIIAPList.Add pApproachLeg
		End If
//Transition III :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
		Set pTransition := New ProcedureTransition

	//    pTransition.Description :=
	//    pTransition.ID :=
	//    pTransition.TransitionId := TextBox0???.Text

		Set pTransition.LegList := pSegmentLegList

	//    pTransition.Procedure :=
		Set pString := New StringClass
		pString.Value := SelectedRWY.ID
		Set pTransition.RunwayDirectionId := pString

		pTransition.Type := ProcedurePhaseType_APPROACH

		pProcedure.TransitionList.Add pTransition
		SaveTrackPoints FileName, pTransition
	End If
}

  //bu hisseye yeniden baxilmalidi
  {
	pcObjectDir := CoARANObjectDirectory_.Create;
	pcObjectDir.QueryInterface(IObjectDirectory, pObjectDir);
//    Set pObjectDir.map = pMap

	mapFileName := GUI.GetRelatedFileName();
	pObjectDir.ConnectUsingRNAVSettings(mapFileName);

	pObjectDir.SetProcedure(pProcedure);
   }
   //end
//Profile Legs :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
//IAPProfile :=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:=:
{
	mHUomDistance := uomDistHorzTab[DistanceUnit];
	mVUomDistance := uomDistVerTab[HeightUnit];

	for I := 0 to ArrivalProfile.PointsNo - 1 do
	begin
		pIAPProfile := New IAPProfile
		pPoint := ArrivalProfile.GetPoint(I)

		pDouble := New DoubleClass
		pDouble.Value := ConvertDistance(pPoint.X, 2)
		pIAPProfile.Distance.Uom := mHUomDistance
		pIAPProfile.Distance.Value := pDouble

		pDouble := New DoubleClass
		pDouble.Value := ConvertHeight(pPoint.Z + fRefHeight, 2)
		pIAPProfile.Altitude.Uom := mVUomDistance
		pIAPProfile.Altitude.Value := pDouble

		pDouble := New DoubleClass
		pDouble.Value := pPoint.Y
		pIAPProfile.Course := pDouble

		pDouble := New DoubleClass
		pDouble.Value := pPoint.M
		pIAPProfile.Gradient := pDouble

		pIAPProfile.IAPMid := pProcedure.ID
		pIAPProfile.SeqNumber := I
		pIAPProfile.FIXRole := ArrivalProfile.GetPointRole(I)

		pIAPProfile.FirRoleFlag := HaveFAF Or (pIAPProfile.FIXRole <> "FAF")
		pObjectDir.SetIAPProfile pIAPProfile
	end;

	if not HaveTP then
	begin
		fDis := SelectedRWY.Length + 1852;
		pPoint := ArrivalProfile.GetPoint(ArrivalProfile.MAPtIndex);
		fHeight := pPoint.Z + (pPoint.X + fDis) * fMissAprPDG;

		pIAPProfile := New IAPProfile;

		pDouble := New DoubleClass;
		pDouble.Value := ConvertDistance(fDis, 2);

		pIAPProfile.Distance.Uom := mHUomDistance;
		pIAPProfile.Distance.Value := pDouble;

		pDouble := New DoubleClass;
		pDouble.Value := ConvertHeight(fHeight + fRefHeight, 2);
		pIAPProfile.Altitude.Uom := mVUomDistance;
		pIAPProfile.Altitude.Value := pDouble;

		pDouble := New DoubleClass;
		pDouble.Value := pPoint.Y;
		pIAPProfile.Course := pDouble;

		pDouble := New DoubleClass;
		pDouble.Value := pPoint.M;
		pIAPProfile.Gradient := pDouble;

		pIAPProfile.IAPMid := pProcedure.ID;
		pIAPProfile.SeqNumber := ArrivalProfile.PointsNo;
		pIAPProfile.FIXRole := '';

		pIAPProfile.FirRoleFlag := True;
		pObjectDir.SetIAPProfile pIAPProfile;
	end
}
end;

procedure TMainForm.rgIntermediatSensorTypeClick(Sender: TObject);
begin
	cbPBNTypeIF.Clear;
	if TControl(Sender).Tag = 0 then
	begin
		FIF.SensorType := stGNSS;
		cbMoreDMEIF.Enabled := False;
		cbPBNTypeIF.Tag := 0;
		cbPBNTypeIF.Items.Add('RNP APCH');
		cbPBNTypeIF.Items.Add('RNAV 1');
		cbPBNTypeIF.Items.Add('RNAV 2');
	end
	else
	begin
		FIF.SensorType := stDME_DME;
		cbMoreDMEIF.Enabled := True;
		cbPBNTypeIF.Tag := 1;
		cbPBNTypeIF.Items.Add('RNAV 1');
		cbPBNTypeIF.Items.Add('RNAV 2');
	end;
	cbPBNTypeIF.ItemIndex := 0;
	cbPBNTypeIFChange(cbPBNTypeIF);
end;

procedure TMainForm.cbPBNTypeIFChange(Sender: TObject);
begin
	FIF.PBNType := TPBNType(cbPBNTypeIF.ItemIndex + cbPBNTypeIF.Tag);
  FIF.RefreshGraphics;
end;

procedure TMainForm.cbMoreDMEIFClick(Sender: TObject);
begin
	FIF.MultiCoverage := cbMoreDMEIF.Checked;
	FIF.RefreshGraphics;
end;

procedure TMainForm.rgMAHFSensorTypeClick(Sender: TObject);
begin
	cbPBNTypeMA.Clear;
	if rgMAHFSensorType.ItemIndex = 0 then
	begin
		FMAPt.SensorType := stGNSS;
		cbMoreDMEMA.Enabled := False;
		cbPBNTypeMA.Tag := 0;
		cbPBNTypeMA.Items.Add('RNP APCH');
		cbPBNTypeMA.Items.Add('RNAV 1');
		cbPBNTypeMA.Items.Add('RNAV 2');
	end
	else
	begin
		FMAPt.SensorType := stDME_DME;
		cbMoreDMEMA.Enabled := True;
		cbPBNTypeMA.Tag := 1;
		cbPBNTypeMA.Items.Add('RNAV 1');
		cbPBNTypeMA.Items.Add('RNAV 2');
	end;
	cbPBNTypeMA.ItemIndex := 0;
	cbPBNTypeMAChange(cbPBNTypeMA);
end;

procedure TMainForm.cbPBNTypeMAChange(Sender: TObject);
begin
	FMAPt.PBNType := TPBNType(cbPBNTypeMA.ItemIndex + cbPBNTypeMA.Tag);
	FMAPt.RefreshGraphics;
end;

procedure TMainForm.cbMoreDMEMAClick(Sender: TObject);
begin
	FMAPt.MultiCoverage := cbMoreDMEMA.Checked;
	FMAPt.RefreshGraphics;
end;

procedure TMainForm.prmFAFAltitudeChangeValue(Sender: TCustomParameter);
var
	tmpRange:	TInterval;
begin
	FFAF.Altitude := FFAFInfo.AltitudeParameter.Value;
	tmpRange.Left := FFAF.Altitude;
	tmpRange.Right := tmpRange.Left + (FIFInfo.DistanceParameter.Value -
			GAircraftCategoryConstants.Constant[arImHorSegLen].Value[FAircraftCategory]) *
									  FIFInfo.GradientParameter.Range.Left;

	FIFInfo.AltitudeParameter.BeginUpdate;
	FIFInfo.AltitudeParameter.Range := tmpRange;
	FIFInfo.Altitude := FFAFInfo.AltitudeParameter.Value;
	FIFInfo.AltitudeParameter.EndUpdate;

	if rbFAFByDist.Checked then
	begin
		if FApproachType = 0 then
			FFAFInfo.DistanceParameter.ChangeValue((FFAFInfo.AltitudeParameter.Value
					- FRWYDirection.ElevTdz - GPANSOPSConstants.Constant[arAbv_Treshold].Value)/FFAFInfo.GradientParameter.Value)
		else
			FFAFInfo.DistanceParameter.ChangeValue((FFAFInfo.AltitudeParameter.Value
					- FCirclingMinOCH - FAerodrome.Elevation)/FFAFInfo.GradientParameter.Value);
	end;
end;

procedure TMainForm.prmFAFGradientChangeValue(Sender: TCustomParameter);
var
	TmpRange:	TInterval;
begin
	if FApproachType = 0 then
	begin
		TmpRange.Left := FFAFInfo.DistanceParameter.Range.Left * FFAFInfo.GradientParameter.Value +
								GPANSOPSConstants.Constant[arAbv_Treshold].Value + FRWYDirection.ElevTdz;
		TmpRange.Right := FFAFInfo.DistanceParameter.Range.Right * FFAFInfo.GradientParameter.Value +
								GPANSOPSConstants.Constant[arAbv_Treshold].Value + FRWYDirection.ElevTdz;
	end
	else
	begin
		TmpRange.Left := FFAFInfo.DistanceParameter.Range.Left * FFAFInfo.GradientParameter.Value +
								FCirclingMinOCH + FAerodrome.Elevation;
		TmpRange.Right := FFAFInfo.DistanceParameter.Range.Right * FFAFInfo.GradientParameter.Value +
								FCirclingMinOCH + FAerodrome.Elevation;
	end;

	FFAFInfo.AltitudeParameter.BeginUpdate;
	FFAFInfo.AltitudeParameter.Range := TmpRange;
	FFAFInfo.AltitudeParameter.EndUpdate;

	if rbFAFByDist.Checked then
	begin
		if FApproachType = 0 then
			FFAFInfo.DistanceParameter.ChangeValue((FFAFInfo.AltitudeParameter.Value
					- FRWYDirection.ElevTdz - GPANSOPSConstants.Constant[arAbv_Treshold].Value)/FFAFInfo.GradientParameter.Value)
		else
			FFAFInfo.DistanceParameter.ChangeValue((FFAFInfo.AltitudeParameter.Value
					- FCirclingMinOCH - FAerodrome.Elevation)/FFAFInfo.GradientParameter.Value);
	end
	else
	begin
		if FApproachType = 0 then
			prmFAFAltitude.ChangeValue(FFAFInfo.DistanceParameter.Value * FFAFInfo.GradientParameter.Value
					+ FRWYDirection.ElevTdz + GPANSOPSConstants.Constant[arAbv_Treshold].Value)
		else
			prmFAFAltitude.ChangeValue(FFAFInfo.DistanceParameter.Value * FFAFInfo.GradientParameter.Value
					+ FCirclingMinOCH + FAerodrome.Elevation);
	end;
end;

procedure TMainForm.AddBranchFrame1spbtnInfoClick(Sender: TObject);
begin
  AddBranchFrame1.spbtnInfoClick(Sender);
end;

end.

{
procedure TMainForm.Button3Click(Sender: TObject);
var
	I, N:			Integer;
	ObstacleList:	TList;
	K, TimeInSec,
	Dist0, Dist1:	Double;

	PrimaryPolygon,
	FullPolygon:	TPolygon;
//	ObstacleInfo:	TObstacleInfo;
	Obstacle:		TObstacleInfo;	//TObstacle;
	T0, T1:			int64;
begin
test: Boolean;
//test:= False;
(*
	QueryPerformanceFrequency(T0);
	K := 1.0/T0;
	PrimaryPolygon := TPolygon(PrimaryPolyList.Items[0]);
	FullPolygon := TPolygon(FullPolyList.Items[0]);

	ObstacleList := FObstacleListList.Items[0];
	N := ObstacleList.Count;

if Test then
begin
	GUI.DrawPolygon(FullPolygon, 255, sfsNull);
	GUI.DrawPolygon(PrimaryPolygon, 0, sfsNull);
end;

if RadioGroup1.ItemIndex = 0 then
begin
	QueryPerformanceCounter(T0);

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(ObstacleList.Items[I]);
		if Obstacle.Flags and 1 = 0 then
		begin
			Dist0 := PointToRingDistanceF(Obstacle.Host.Prj, PrimaryPolygon.Ring[0]);
			Dist1 := PointToRingDistanceF(Obstacle.Host.Prj, FullPolygon.Ring[0]);
			Obstacle.fTmp := Dist1/(Dist0 + Dist1);
		end
		else
			Obstacle.fTmp := 1.0;

		Obstacle.MOC := 300.0 * Obstacle.fTmp;
	end;

	QueryPerformanceCounter(T1);
	TimeInSec := (T1 - T0)*K;
	Label7.Caption := FormatFloat('0.000', 1000 * TimeInSec);
end
else if RadioGroup1.ItemIndex = 1 then
begin
	QueryPerformanceCounter(T0);

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(ObstacleList.Items[I]);
		if Obstacle.Flags and 1 = 0 then
		begin
			Dist0 := PointToRingDistance(Obstacle.Host.Prj, PrimaryPolygon.Ring[0]);
			Dist1 := PointToRingDistance(Obstacle.Host.Prj, FullPolygon.Ring[0]);
			Obstacle.fTmp := Dist1/(Dist0 + Dist1);
		end
		else
			Obstacle.fTmp := 1.0;

		Obstacle.MOC := 300.0 * Obstacle.fTmp;
	end;

	QueryPerformanceCounter(T1);
	TimeInSec := (T1 - T0)*K;
	Label8.Caption := FormatFloat('0.000', 1000 * TimeInSec);
end
else
begin
	QueryPerformanceCounter(T0);

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacleInfo(ObstacleList.Items[I]);
		if Obstacle.Flags and 1 = 0 then
		begin
			Dist0 := PointToRingDistanceOld(Obstacle.Host.Prj, PrimaryPolygon.Ring[0]);
			Dist1 := PointToRingDistanceOld(Obstacle.Host.Prj, FullPolygon.Ring[0]);
			Obstacle.fTmp := Dist1/(Dist0 + Dist1);
		end
		else
			Obstacle.fTmp := 1.0;

		Obstacle.MOC := 300.0 * Obstacle.fTmp;
	end;

	QueryPerformanceCounter(T1);
	TimeInSec := (T1 - T0)*K;
	Label9.Caption := FormatFloat('0.000', 1000 * TimeInSec);
end;

	ReportForm.AddBranch(ObstacleList, TList(FIABranchs.Items[0]));
*)

//	ReportForm.FillPage(ObstacleList, nil);
//	PrimaryPolygon := TPolygon(PrimaryPolyList.Items[0]);
(*
	FullPolygon := TPolygon(FullPolyList.Items[0]);
	if CheckBoxTest.Checked then
		GUI.DrawPolygon(FullPolygon, 255, sfsNull);

	ObstacleList := GetObstacles(FullPolygon, FObstalces);
	ObstacleList.Free;
*)
(*
	Polygon := TPolygon(FullPolyList.Items[0]);
	N := FObstalces.Count;

	for I := 0 to N - 1 do
	begin
		Obstacle := TObstacle(FObstalces.Item[I]);
		if (Obstacle.AIXMID = 'CZ347') or (Obstacle.AIXMID = 'CZ367') then
		begin
			if IsPointInPoly(Obstacle.Prj, Polygon) then
				GUI.DrawPoint(Obstacle.Prj, 255)
		end;
	end;
*)
end;
}

