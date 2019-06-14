unit FIXUnit;

interface
uses
	Classes, Controls, Geometry, UIContract, ConstantsContract, UnitConverter,
	IntervalUnit, Parameter, ARANMath; //ARANFunctions,

type
	TSensorType	= (stGNSS, stDME_DME);

	TFlightPhase	= (ffEnroute, ffSTAR, ffIIAP, ffFAFApch, ffFApch, ffMApLT28);
	TPBNType		= (ptRNPAPCH, ptRNAV1, ptRNAV2, ptBASIC_RNP1, ptRNP4, ptRNAV5);

	TFlyMode		= (fmFlyBy, fmFlyOver);
	TFlyPath		= (fpDirectToFIX, fpCourseToFIX, fpTrackToFIX);
	TTurnAt			= (taMApt, taHeight, taTP);

	TTableCell = record
		ATT, FTT,
		XTT, Semiwidth:	double;
	end;


const
	ffSID = ffFApch;
	ffMApGE28 = ffIIAP;
	ffSIDGE28 = ffMApGE28;
	ffSIDGE56 = ffEnroute;

{
	WPTParams : Array [TFlightFase, TSensorType, TPBNType] of TTableCell =
	(
	(((ATT:0;FTT:0;XTT:0;Semiwidth:0), (ATT:2963.2;FTT:926;XTT:3704;Semiwidth:9260),(ATT:2963.2;FTT:1852;XTT:3704;Semiwidth:9260),(ATT:0;FTT:0;XTT:0;Semiwidth:0),(ATT:5926.4;FTT:3704;XTT:7408;Semiwidth:14816),(ATT:3722.52;FTT:4630;XTT:4648.52;Semiwidth:10686.04)),
		((ATT:0;FTT:0;XTT:0;Semiwidth:0), (ATT:0;FTT:926;XTT:0;Semiwidth:0),(ATT:0;FTT:1852;XTT:0;Semiwidth:0),(ATT:0;FTT:0;XTT:0;Semiwidth:0),(ATT:0;FTT:0;XTT:0;Semiwidth:0),(ATT:0;FTT:4630;XTT:0;Semiwidth:0))),

	(((ATT:0;FTT:0;XTT:0;Semiwidth:0),		(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:1852;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4074.4), (ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
		((ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:926;XTT:0;Semiwidth:0),			(ATT:0;FTT:1852;XTT:0;Semiwidth:0),			(ATT:0;FTT:926;XTT:0;Semiwidth:0),				(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0))),

	(((ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),	(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:1852;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4074.4), (ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
		((ATT:0;FTT:0;XTT:1852;Semiwidth:0),	(ATT:0;FTT:926;XTT:0;Semiwidth:0),			(ATT:0;FTT:1852;XTT:0;Semiwidth:0),			(ATT:0;FTT:926;XTT:0;Semiwidth:0),				(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0))),

	(((ATT:444.48;FTT:463;XTT:555.6;Semiwidth:1759.4),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
		((ATT:0;FTT:463;XTT:0;Semiwidth:0),				(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0))),

	(((ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
		((ATT:0;FTT:926;XTT:0;Semiwidth:0),				(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)))
	);
}
	WPTParams : Array [TFlightPhase, TPBNType] of TTableCell =
	((	(ATT:0;FTT:0;XTT:0;Semiwidth:0),					(ATT:2963.2;FTT:926;XTT:3704;Semiwidth:9260),	(ATT:2963.2;FTT:1852;XTT:3704;Semiwidth:9260),(ATT:0;FTT:0;XTT:0;Semiwidth:0),				 (ATT:5926.4;FTT:3704;XTT:7408;Semiwidth:14816),(ATT:3722.52;FTT:4630;XTT:4648.52;Semiwidth:10686.04)),
	(	(ATT:0;FTT:0;XTT:0;Semiwidth:0),					(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),	(ATT:1481.6;FTT:1852;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4074.4), (ATT:0;FTT:0;XTT:0;Semiwidth:0),				(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
	(	(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),		(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:1852;XTT:1852;Semiwidth:4630),(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:4074.4), (ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
	(	(ATT:444.48;FTT:463;XTT:555.6;Semiwidth:1759.4),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
	(	(ATT:444.48;FTT:463;XTT:555.6;Semiwidth:1759.4),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)),
	(	(ATT:1481.6;FTT:926;XTT:1852;Semiwidth:3704),		(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0),	(ATT:0;FTT:0;XTT:0;Semiwidth:0)));

	WPTBuffers:	Array [TFlightPhase] of double = (3704, 1852, 1852, 1852, 926, 926);

type

	TWayPoint = class(TPersistent)
	private
		FPrjPt:			TPoint;
		FGeoPt:			TPoint;

		FTolerArea:		TPolygon;
		FPropagated,
		FTurnDir:		TSideDirection;

		FEntryDir:		Double;
		FOutDir:		Double;
		FTurnAngle:		Double;
		FOCH:			Double;

		FBankAngle,
		FPilotTime,
		FBankTime:		Double;

		FFTE,
		FXXT, FATT:		Double;
		FEPT, FLPT:		Double;

		FASW_L, FASW_2_L,
		FASW_R, FASW_2_R:	Double;

		FSemiWidth:		Double;
//
		FFlightPhase:	TFlightPhase;
		FConstMode,
		FFIXRole:		TFIXRole;
		FID,
		FName:			String;
		FSensorType:	TSensorType;
		FPBNType:		TPBNType;
		FAltitude,
		FGradient:		Double;
//		FBank:			Double;

		FFlyMode:		TFlyMode;
		FImMaxIntercept,
		FFlyOInterBank,
		FFlyByTechTol,
		FFlyOTechTol,
		FISA,
		FIAS, FTAS:		Double;

		FUI:				TUIContract;
		FToleranceSymbol:	TFillSymbol;
		FPointSymbol:		TPointSymbol;

		FMultiCoverage,
		FDrawingEnabled:	Boolean;
		FToleranceElement,
		FPointElement:		Integer;

		function GetName: string;
		procedure SetPoint(NewPoint: TPoint); virtual;
		procedure SetRole(NewRole: TFIXRole);
		procedure SetOutDirection(NewDirection: Double);
		procedure SetEntryDir(NewDirection: Double);

		procedure SetISA(NewValue: Double);
		procedure SetAltitude(NewValue: Double);
		procedure SetIAS(NewValue: Double);

		procedure SetGradient(NewValue: Double);
		procedure SetBank(NewValue: Double);
		procedure SetDrawingEnabled(Value: Boolean);
		procedure SetTurnDir(Val: TSideDirection);
		procedure SetPropagated(Val: TSideDirection);
		procedure SetSensorType(Val: TSensorType);
		procedure SetPBNType(Val: TPBNType);
		procedure SetMultiCoverage(Value: Boolean);
		function GetEffectiveTurnDirection: TSideDirection;
	protected
		procedure CreateTolerArea;	virtual; abstract;
		procedure CalcWPTParams;
	public
		JointFlags:													Integer;
		procedure CalcTurnPoints;
//		constructor Create;											overload;
		constructor Create(UI: TUIContract);						overload;
		constructor Create(InitialRole: TFIXRole; UI: TUIContract); overload;
//		procedure Assign(AnOther: TWayPoint);
		procedure AssignTo(Dest: TPersistent);					override;

		destructor Destroy;			override;

		function CalcFromMinStablizationDistance(TurnAngle: Double): Double;
		function CalcInToMinStablizationDistance(TurnAngle: Double): Double;
		function CalcTurnRadius: Double;

		procedure DeleteGraphics;	virtual;
		procedure RefreshGraphics;	virtual;
		procedure ReCreateArea;

		property ID: String read FID write FID; 
		property Name: String read GetName write FName;

		property PrjPt: TPoint read FPrjPt write SetPoint;
		property GeoPt: TPoint read FGeoPt;

		property Role: TFIXRole read FFIXRole write SetRole;
		property ConstMode: TFIXRole read FConstMode;
		property FlyMode: TFlyMode read FFlyMode write FFlyMode;
		property SensorType: TSensorType read FSensorType write SetSensorType;
		property PBNType: TPBNType read FPBNType write SetPBNType;
		property MultiCoverage: Boolean read FMultiCoverage write SetMultiCoverage;

		property ISA: Double read FISA write SetISA;
		property Altitude: Double read FAltitude write SetAltitude;
		property IAS: Double read FIAS write SetIAS;
		property TAS: Double read FTAS;

		property Gradient: Double read FGradient write SetGradient;
		property Bank: Double read FBankAngle write SetBank;

		property OutDirection: Double read FOutDir write SetOutDirection;
		property EntryDirection: Double read FEntryDir write SetEntryDir;
		property TurnDirection: TSideDirection read FTurnDir write SetTurnDir;
		property PropagatedTurnDirection: TSideDirection read FPropagated write SetPropagated;
		property EffectiveTurnDirection: TSideDirection read GetEffectiveTurnDirection;
		property TurnAngle: Double read FTurnAngle;

		property TolerArea: TPolygon read FTolerArea;

		property DrawingEnabled: Boolean read FDrawingEnabled write SetDrawingEnabled;

		property ToleranceElement: Integer read FToleranceElement;
		property PointElement: Integer read FPointElement;

		property PilotTime: Double read FPilotTime;
		property BankTime: Double read FBankTime;

		property XXT: Double read FXXT;
		property ATT: Double read FATT;
		property EPT: Double read FEPT;
		property LPT: Double read FLPT;
		property SemiWidth: Double read FSemiWidth;
		property ASW_L: Double read FASW_L write FASW_L;
		property ASW_2_L: Double read FASW_2_L write FASW_2_L;
		property ASW_R: Double read FASW_R write FASW_R;
		property ASW_2_R: Double read FASW_2_R write FASW_2_R;
	end;

	TFIX = class(TWayPoint)
	protected
		procedure CreateTolerArea;									override;
	end;
//=======================================
	TMAPt = class(TFIX)
	private
		FSOCDistance:	Double;
		FSOCAreaSymbol:	TFillSymbol;

		FSOCPrjPt:		TPoint;
		FSOCGeoPt:		TPoint;

		FSOCArea:			TPolygon;
		FStMAArea:			TPolygon;

		FSOCAreaElement,
		FSOCElement:	Integer;
		procedure SetPoint(NewPoint: TPoint);						override;
		procedure SetStMAArea(Val: TPolygon);
//		procedure SetOutDirection(Val: Double);
	protected
		procedure CreateTolerArea; 									override;
	public
		constructor Create(UI: TUIContract);						overload;
		constructor Create(InitialRole: TFIXRole; UI: TUIContract); overload;
		procedure AssignTo(Dest: TPersistent);						override;
		destructor Destroy;											override;
		procedure DeleteGraphics;									override;
		procedure RefreshGraphics;									override;

		property SOCDistance:	Double read FSOCDistance write FSOCDistance;
		property SOCPrjPt:		TPoint read FSOCPrjPt;
		property SOCGeoPt:		TPoint read FSOCGeoPt;
		property StraightArea:	TPolygon read FStMAArea write SetStMAArea;
		property SOCArea:		Tpolygon read FSOCArea;
	end;
//=======================================

	TMATF = class(TFIX)
	private
		FTurnDistance:		Double;
//		FNomOutDirection,
		FOutDir1,
		FOutDir2:			Double;
		FFlyPath:			TFlyPath;
		FTurnAt:			TTurnAt;

		FTolerSecArea,
		FStraightInPrimPolygon,
		FStraightInSecPolygon: TPolygon;
		FMAPt:				TMAPt;
		FReferenceGeometry:	TGeometry;

		procedure SetMAPt(Val: TPoint);
		procedure SetTurnAt(Val: TTurnAt);
		procedure SetFlyPath(Val: TFlyPath);
	protected
		procedure CreateTolerArea;									override;
	public
		constructor Create(UI: TUIContract);
		destructor Destroy;											override;

		procedure AssignTo(Dest: TPersistent);						override;
		procedure RefreshGraphics;									override;

//		property NomOutDirection: Double read FNomOutDirection write FNomOutDirection;
		property OutDirection1: Double read FOutDir1 write FOutDir1;
		property OutDirection2: Double read FOutDir2 write FOutDir2;
		property MAPt: TMAPt read FMAPt write FMAPt;//SetMAPt;
		property StraightInPrimaryPolygon: TPolygon read FStraightInPrimPolygon write FStraightInPrimPolygon;
		property StraightInSecondaryPolygon: TPolygon read FStraightInSecPolygon write FStraightInSecPolygon;

		property TurnDistance:	Double read FTurnDistance write FTurnDistance;
		property TurnAt: TTurnAt read FTurnAt write SetTurnAt;
		property FlyPath: TFlyPath read FFlyPath write SetFlyPath;

		property SecondaryTolerArea: TPolygon read FTolerSecArea;
		property ReferenceGeometry: TGeometry read FReferenceGeometry;
	end;
//=======================================

	TFIXInfo = class
	private
		FFIX:					TWayPoint;

		FDistParameter,
		FAltitudeParameter,
		FGradientParameter,
		FAngleParameter,
		FSpeedParameter:		TParameter;

		FPrev:					TFIXInfo;
		FNext:					TFIXInfo;

		function GetDistance: Double;
		function GetAltitude: Double;
		function GetGradient: Double;
		function GetAngle: Double;
		function GetSpeed: Double;

		procedure SetDistance(Val: Double);
		procedure SetAltitude(Val: Double);
		procedure SetGradient(Val: Double);
		procedure SetAngle(Val: Double);
		procedure SetSpeed(Val: Double);

	public
		constructor Create;	overload;
		constructor Create(InitialFIX: TWayPoint); overload;
		destructor Destroy; override;

		property FIX: TWayPoint read FFIX write FFIX;

		property Distance: Double read GetDistance write SetDistance;
		property Altitude: Double read GetAltitude write SetAltitude;
		property Gradient: Double read GetGradient write SetGradient;
		property Angle: Double read GetAngle write SetAngle;
		property Speed: Double read GetSpeed write SetSpeed;

		property DistanceParameter: TParameter read FDistParameter write FDistParameter;
		property AltitudeParameter: TParameter read FAltitudeParameter write FAltitudeParameter;
		property GradientParameter: TParameter read FGradientParameter write FGradientParameter;
		property AngleParameter: TParameter read FAngleParameter write FAngleParameter;
		property SpeedParameter: TParameter read FSpeedParameter write FSpeedParameter;
	end;

const
	FlyModeNames: Array [TFlyMode] of string =
			('Fly By', 'Fly Ower');

implementation

uses
	SysUtils, Math, StdCtrls, ARANGlobals, ARANFunctions, ApproachGlobals;


function RGB(R, G, B: Byte): Cardinal;
begin
	Result := R + (G shl 8) + (B shl 16);
end;

// TFIX ========================================================================

constructor TWayPoint.Create(UI: TUIContract);
begin
	Inherited Create;

	FUI := UI;
	FName := '';
	FID := '0';

	FPrjPt := TPoint.Create;
	FGeoPt := TPoint.Create;

	FTolerArea := TPolygon.Create;

	FFlyByTechTol := GPANSOPSConstants.Constant[rnvFlyByTechTol].Value;
	FFlyOTechTol := GPANSOPSConstants.Constant[rnvFlyOTechTol].Value;

	FImMaxIntercept := GPANSOPSConstants.Constant[arImMaxIntercept].Value;
	FFlyOInterBank := GPANSOPSConstants.Constant[rnvFlyOInterBank].Value;

	FEntryDir := 0.0;
	FOutDir := 0.0;

	FToleranceSymbol := TFillSymbol.Create;
//	FToleranceSymbol.Color := RGB(Random(256), Random(256), Random(256));
//	FToleranceSymbol.Style := TFillStyle(Random(8));

	FToleranceSymbol.Color := RGB(0, 0, 255);
	FToleranceSymbol.Style := TFillStyle(sfsCross);

//	FPointSymbol := TPointSymbol.Create(TPointStyle(Random(5)), RGB(Random(256), Random(256), Random(256)), 6);
	FPointSymbol := TPointSymbol.Create(smsCircle, 255, 6);

	FToleranceElement := -1;
	FPointElement := -1;
	FFIXRole := TFIXRole(-1);
	FConstMode := FFIXRole;

	FSensorType := TSensorType(-1);
	FPBNType := TPBNType(-1);

//	FSensorType := stGNSS;
//	FPBNType := ptRNPAPCH;
	FMultiCoverage := False;

	FPilotTime := 0.0;
	FBankTime := 0.0;

	FXXT := 0.0;
	FATT := 0.0;
	FSemiWidth := 0.0;
	FDrawingEnabled := True;
end;

constructor TWayPoint.Create(InitialRole: TFIXRole; UI: TUIContract);
begin
	Inherited Create;

	FUI := UI;
	FPrjPt := TPoint.Create;
	FGeoPt := TPoint.Create;
	FTolerArea := TPolygon.Create;

	JointFlags := 0;

	FName := '';
	FEntryDir := 0.0;
	FOutDir := 0.0;
	FTurnDir := SideOn;
	FPropagated := SideOn;

	FToleranceSymbol := TFillSymbol.Create;
//	FToleranceSymbol.Color := RGB(Random(256), Random(256), Random(256));
//	FToleranceSymbol.Style := TFillStyle(Random(8));//sfsCross;

	FToleranceSymbol.Color := RGB(0, 255, 255);
	FToleranceSymbol.Style := sfsCross;


//	FPointSymbol := TPointSymbol.Create(TPointStyle(Random(5)), RGB(Random(256), Random(256), Random(256)), 6);
	FPointSymbol := TPointSymbol.Create(smsCircle, RGB(2, 215, 72), 6);

	FToleranceElement := -1;
	FPointElement := -1;

	FFlyByTechTol := GPANSOPSConstants.Constant[rnvFlyByTechTol].Value;
	FFlyOTechTol := GPANSOPSConstants.Constant[rnvFlyOTechTol].Value;

	FImMaxIntercept := GPANSOPSConstants.Constant[arImMaxIntercept].Value;
	FFlyOInterBank := GPANSOPSConstants.Constant[rnvFlyOInterBank].Value;

	FSensorType := stGNSS;
	FPBNType := ptRNPAPCH;

	FMultiCoverage := False;

	FConstMode := TFIXRole(-1);
	SetRole(InitialRole);
	FDrawingEnabled := True;
end;

destructor TWayPoint.Destroy;
begin
	DeleteGraphics;
	FPrjPt.Free;
	FGeoPt.Free;

	FTolerArea.Free;
	FToleranceSymbol.Free;
	FPointSymbol.Free;

	Inherited;
end;

procedure TWayPoint.AssignTo(Dest: TPersistent);
var
	AnOther:	TWayPoint;
begin
	AnOther := TWayPoint(Dest);
	AnOther.FPrjPt.Assign(FPrjPt);
	AnOther.FGeoPt.Assign(FGeoPt);
	AnOther.FTolerArea.Assign(FTolerArea);

	AnOther.FTurnDir := FTurnDir;
	AnOther.FEntryDir := FEntryDir;
	AnOther.FOutDir := FOutDir;
	AnOther.FTurnAngle := FTurnAngle;
	AnOther.FOCH := FOCH;

	AnOther.FBankAngle := FBankAngle;
	AnOther.FPilotTime := FPilotTime;
	AnOther.FBankTime := FBankTime;

	AnOther.FFTE := FFTE;
	AnOther.FXXT := FXXT;
	AnOther.FATT := FATT;
	AnOther.FEPT := FEPT;
	AnOther.FLPT := FLPT;

	AnOther.FASW_L := FASW_L;
	AnOther.FASW_2_L := FASW_2_L;
	AnOther.FASW_R := FASW_R;
	AnOther.FASW_2_R := FASW_2_R;
	AnOther.FSemiWidth := FSemiWidth;

	AnOther.JointFlags := JointFlags;
	AnOther.FFlightPhase := FFlightPhase;
	AnOther.FConstMode := FConstMode;
	AnOther.FFIXRole := FFIXRole;
	AnOther.FID := FID;
	AnOther.FName := FName;

	AnOther.FSensorType := FSensorType;
	AnOther.FPBNType := FPBNType;

	AnOther.FAltitude := FAltitude;
	AnOther.FGradient := FGradient;
//		FBank:			Double;

	AnOther.FFlyMode := FFlyMode;
	AnOther.FImMaxIntercept := FImMaxIntercept;
	AnOther.FFlyOInterBank := FFlyOInterBank;
	AnOther.FFlyByTechTol := FFlyByTechTol;
	AnOther.FFlyOTechTol := FFlyOTechTol;
	AnOther.FISA := FISA;
	AnOther.FIAS := FIAS;
	AnOther.FTAS := FTAS;
	AnOther.FUI := FUI;
	AnOther.FName := FName;

	AnOther.FToleranceSymbol.Assign(FToleranceSymbol);
	AnOther.FPointSymbol.Assign(FPointSymbol);
	AnOther.FDrawingEnabled := FDrawingEnabled;

	AnOther.DeleteGraphics;
	AnOther.FToleranceElement := -1;
	AnOther.FPointElement := -1;

	AnOther.FMultiCoverage := FMultiCoverage;
	AnOther.FDrawingEnabled := FDrawingEnabled;
end;

function TWayPoint.CalcFromMinStablizationDistance(TurnAngle: Double): Double;
var
	R1Turn, R2Turn:			Double;
	SinTurnAngle,
	CosTurnAngle,
	SinFImMaxIntercept,
	CosFImMaxIntercept:		Extended;
begin
	result := 0.0;
	if RadToDeg(TurnAngle) > 2.0 then
	begin
		if RadToDeg(TurnAngle) < 50.0 then TurnAngle := DegToRad(50.0);
		R1Turn := BankToRadius(FBankAngle, FTAS);

		if FFlyMode = fmFlyBy then
			result := R1Turn * Tan(0.5 * TurnAngle) + FFlyByTechTol * FTAS
		else
		begin
			SinCos(TurnAngle, SinTurnAngle, CosTurnAngle);
			SinCos(FImMaxIntercept, SinFImMaxIntercept, CosFImMaxIntercept);
			R2Turn := BankToRadius(FFlyOInterBank, FTAS);

			result := R1Turn *
				(SinTurnAngle + CosTurnAngle * SinFImMaxIntercept/CosFImMaxIntercept +
					1.0/SinFImMaxIntercept - CosTurnAngle/(SinFImMaxIntercept * CosFImMaxIntercept)) +
					R2Turn * Tan(0.5 * FImMaxIntercept) +
					FFlyOTechTol * FTAS
		end
	end;
end;

function TWayPoint.CalcInToMinStablizationDistance(TurnAngle: Double): Double;
begin
	result := 0.0;
	if (FFlyMode = fmFlyBy) and (RadToDeg(TurnAngle) > 2.0) then
	begin
		if RadToDeg(TurnAngle) < 50.0 then TurnAngle := DegToRad(50.0);

		result := Tan(0.5 * TurnAngle) * BankToRadius(FBankAngle, FTAS) +
				FFlyByTechTol * FTAS
	end;
end;

function TWayPoint.CalcTurnRadius: Double;
begin
	result := BankToRadius(FBankAngle, FTAS);
end;

function TWayPoint.GetName: string;
begin
	if FName <> '' then		result := FName
	else					result := FIXRoleStyleNames[FFIXRole];
end;

procedure TWayPoint.SetPoint(NewPoint: TPoint);
var
	tmpPt:	TPoint;
begin
	FPrjPt.Assign(NewPoint);
	if not FPrjPt.IsEmpty then
	begin
		tmpPt := GGeoOperators.geoTransformations (FPrjPt, GPrjSR, GGeoSR).AsPoint;
		FGeoPt.Assign(tmpPt);
		tmpPt.Free;
		ReCreateArea;
	end
	else
		FGeoPt.Assign(NewPoint);
end;

procedure TWayPoint.ReCreateArea;
begin
	if not FPrjPt.IsEmpty then
	begin
		CalcWPTParams;
		CalcTurnPoints;
		CreateTolerArea;
	end;
end;

procedure TWayPoint.DeleteGraphics;
begin
	FUI.SafeDeleteGraphic(FToleranceElement);
	FUI.SafeDeleteGraphic(FPointElement);

	FToleranceElement := -1;
	FPointElement := -1;
end;


procedure TWayPoint.CalcTurnPoints;
var
	R1Turn,
	L1, L1min:		Double;
begin
	FTurnAngle := Modulus((FEntryDir-FOutDir)*Integer(FTurnDir), 2 * PI);
	if FTurnAngle > DegToRad(240) then FTurnAngle := DegToRad(240);

	if Abs(FTurnAngle) < EpsilonRadian then
	begin
		FEPT := FATT;
		if FFlyMode = fmFlyBy then	FLPT := 0	///-FATT
		else						FLPT := FATT;
	end
	else
	begin
		R1Turn := BankToRadius(FBankAngle, FTAS);
		L1 := R1Turn * Tan(0.5 * FTurnAngle);
		L1min := Min(L1, R1Turn);

		if FFlyMode = fmFlyBy then
		begin
			FEPT := L1 + FATT;
			FLPT := L1min - FATT - FTAS * FPilotTime;
		end
		else
		begin
			FEPT := FATT;
			FLPT := FATT + FTAS * (FPilotTime + FBankTime);
		end;
	end;
end;

procedure TWayPoint.CalcWPTParams;
const
	SigmaSis	= 92.6;
	ST			= 463;
var
	dTT,
	SigmaAir:		double;
begin
	FFTE := WPTParams[FFlightPhase, FPBNType].FTT;

	case FSensorType of
	stGNSS:
		begin
			FXXT		:= WPTParams[FFlightPhase, FPBNType].XTT;
			FATT		:= WPTParams[FFlightPhase, FPBNType].ATT;
			FSemiWidth	:= WPTParams[FFlightPhase, FPBNType].SemiWidth;
		end;
	stDME_DME:
		begin
			SigmaAir := Max(4110 * 0.125 / 100 * Sqrt(FAltitude), 157.42);
			dTT := 2 * C_SQRT2 * Sqrt(SigmaSis * SigmaSis + SigmaAir*SigmaAir);

			if not FMultiCoverage then
				dTT := 2 * dTT;

			FXXT := Sqrt(dTT*dTT + FFTE*FFTE + ST*ST);
			FATT := Sqrt(dTT*dTT + ST*ST);
			FSemiWidth	:= 1.5 * FXXT + WPTBuffers[FFlightPhase];
		end;
	end;

	FASW_R := FSemiWidth;
	FASW_L := FSemiWidth;

	FASW_2_R := 0.5 * FSemiWidth;
	FASW_2_L := 0.5 * FSemiWidth;
end;

procedure TWayPoint.SetSensorType(Val: TSensorType);
begin
	if FSensorType <> Val then
	begin
		FSensorType := Val;
		ReCreateArea;
	end
end;

procedure TWayPoint.SetPBNType(Val: TPBNType);
begin
	if FPBNType <> Val then
	begin
		FPBNType := Val;
		ReCreateArea;
	end;
end;

procedure TWayPoint.SetMultiCoverage(Value: Boolean);
begin
	if FMultiCoverage <> Value then
	begin
		FMultiCoverage := Value;

		if FSensorType = stDME_DME then
			ReCreateArea;
	end
end;

function TWayPoint.GetEffectiveTurnDirection: TSideDirection;
begin
	result := FTurnDir;
	if result = SideOn then
		result := FPropagated;
	if result = SideOn then
		result := SideLeft;
end;

procedure TWayPoint.SetTurnDir(Val: TSideDirection);
begin
	if FTurnDir <> Val then
	begin
		FTurnDir := Val;
		if Val <> SideOn then
			FPropagated := Val;
		ReCreateArea;
	end;
end;

procedure TWayPoint.SetPropagated(Val: TSideDirection);
begin
	if FTurnDir <> SideOn then
		FPropagated := FTurnDir
	else
		FPropagated := Val
end;

procedure TWayPoint.SetDrawingEnabled(Value: Boolean);
begin
	if Value <> FDrawingEnabled then
	begin
		FDrawingEnabled := Value;
		if not Value then DeleteGraphics;
	end;
end;

procedure TWayPoint.RefreshGraphics;
var
	Text:	String;
begin
	FUI.SafeDeleteGraphic(FToleranceElement);
	FUI.SafeDeleteGraphic(FPointElement);

	if not FDrawingEnabled then exit;

	FToleranceElement := FUI.DrawPolygon(FTolerArea, FToleranceSymbol);

	Text := FIXRoleStyleNames[FFIXRole];
	if FName <> '' then Text := Text + ' / ' + FName;

	FPointElement := FUI.DrawPointWithText(FPrjPt, FPointSymbol, Text);
end;

procedure TWayPoint.SetRole(NewRole: TFIXRole);
begin
	if FFIXRole <> NewRole then
	begin
		FFIXRole := NewRole;

		if FFIXRole <> TP_ then
		begin
			case FFIXRole of
			PBN_IAF, IAF_GT_56_, IAF_LE_56_:
				begin
					if FFIXRole = IAF_GT_56_ then
						FFlightPhase := ffEnroute
					else
						FFlightPhase := ffIIAP;

					FPilotTime := GPANSOPSConstants.Constant[arIPilotToleranc].Value;
					FBankTime := GPANSOPSConstants.Constant[arIBankTolerance].Value;
					FBankAngle := GPANSOPSConstants.Constant[arBankAngle].Value;
				end;
			PBN_IF, IF_:
				begin
					FFlightPhase := ffIIAP;
					FPilotTime := GPANSOPSConstants.Constant[arIPilotToleranc].Value;
					FBankTime := GPANSOPSConstants.Constant[arIBankTolerance].Value;
					FBankAngle := GPANSOPSConstants.Constant[arBankAngle].Value;
				end;

			PBN_FAF, FAF_:
				begin
					FFlightPhase := ffFAFApch;
					FPilotTime := GPANSOPSConstants.Constant[arIPilotToleranc].Value;
					FBankTime := GPANSOPSConstants.Constant[arIBankTolerance].Value;
					FBankAngle := GPANSOPSConstants.Constant[arBankAngle].Value;
				end;
			MAPt_, PBN_MAPt:
				begin
					FFlightPhase := ffFApch;
					FPilotTime := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
					FBankTime := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
					FBankAngle := GPANSOPSConstants.Constant[dpT_Bank].Value;
				end;
			PBN_MATF_LT_28, PBN_MATF_GE_28,
			MATF_LE_56, MATF_GT_56:
				begin
					if FFIXRole = PBN_MATF_LT_28 then
						FFlightPhase := ffMApLT28
					else
						FFlightPhase := ffIIAP;

					FPilotTime := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
					FBankTime := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
					FBankAngle := GPANSOPSConstants.Constant[dpT_Bank].Value;
				end;
			MAHF_LE_56, MAHF_GT_56:
				begin
					FFlightPhase := ffIIAP;
					FPilotTime := GPANSOPSConstants.Constant[EnTechTolerance].Value;
					FBankTime := GPANSOPSConstants.Constant[EnTechTolerance].Value;
					FBankAngle := GPANSOPSConstants.Constant[dpT_Bank].Value;
				end;
			IDEP_, DEP_:
				begin
					FFlightPhase := ffEnroute;
					FPilotTime := GPANSOPSConstants.Constant[dpPilotTolerance].Value;
					FBankTime := GPANSOPSConstants.Constant[dpBankTolerance].Value;
					FBankAngle := GPANSOPSConstants.Constant[dpT_Bank].Value;
				end;
			end;
			FConstMode := NewRole;
			ReCreateArea;
		end;
	end;
end;

procedure TWayPoint.SetEntryDir(NewDirection: Double);
begin
	FEntryDir := NewDirection;
	ReCreateArea;
end;

procedure TWayPoint.SetOutDirection(NewDirection: Double);
begin
	FOutDir := NewDirection;
	ReCreateArea;
end;

procedure TWayPoint.SetISA(NewValue: Double);
begin
	FISA := NewValue;
	FTAS := IASToTAS(FIAS, FAltitude, FISA);
	ReCreateArea;
end;

procedure TWayPoint.SetAltitude(NewValue: Double);
begin
	FAltitude := NewValue;
	FTAS := IASToTAS(FIAS, FAltitude, FISA);
	ReCreateArea;
end;

procedure TWayPoint.SetIAS(NewValue: Double);
begin
	FIAS := NewValue;
	FTAS := IASToTAS(FIAS, FAltitude, FISA);
	ReCreateArea;
end;

procedure TWayPoint.SetGradient(NewValue: Double);
begin
	FGradient := NewValue;
end;

procedure TWayPoint.SetBank(NewValue: Double);
begin
	FBankAngle := NewValue;
	ReCreateArea;
end;

// TFIX ========================================================================
procedure TFIX.CreateTolerArea;
var
	Ring:			TRing;
	Point:			TPoint;
//	spanWidth,
	dir:			Double;
begin
	Ring := TRing.Create;
//	spanWidth := 0.5*SemiWidth;

	if FFIXRole < MAPt_ then	dir := FOutDir
	else						dir := FEntryDir;

	Point := LocalToPrj(FPrjPt, dir, -FATT, -FXXT);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, dir, -FATT, FXXT, Point);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, dir, FATT, FXXT, Point);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, dir, FATT, -FXXT, Point);
	Ring.AddPoint(Point);

	FTolerArea.Clear;
	FTolerArea.AddRing(Ring);

	Point.Free;
	Ring.Free;
end;

// TMATF =======================================================================

constructor TMATF.Create(UI: TUIContract);
begin
	Inherited Create(MATF_LE_56, UI);
	FTurnDistance	:= 0.0;
	FOutDir2		:= FOutDir;
	FFlyPath		:= fpDirectToFIX;
	FTurnAt			:= TTurnAt(-1);
	FTolerSecArea	:= TPolygon.Create;
	FReferenceGeometry := nil;
end;

destructor TMATF.Destroy;
begin
	if Assigned(FReferenceGeometry) then
		FReferenceGeometry.Free;

	FTolerSecArea.Free;
	inherited
end;

procedure TMATF.SetMAPt(Val: TPoint);
begin
	FMAPt.Assign(Val);
end;

procedure TMATF.RefreshGraphics;
var
	Text:	String;
begin
	FUI.SafeDeleteGraphic(FToleranceElement);
	FUI.SafeDeleteGraphic(FPointElement);

	if not FDrawingEnabled then exit;

	FToleranceElement := FUI.DrawPolygon(FTolerArea, FToleranceSymbol);
	if FTurnAt = taTP then
	begin
		Text := FIXRoleStyleNames[FFIXRole];
		if FName <> '' then Text := Text + ' / ' + FName;
		FPointElement := FUI.DrawPointWithText(FPrjPt, FPointSymbol, Text);
	end
	else
		FPointElement := FUI.DrawPointWithText(FPrjPt, FPointSymbol, '');
end;

procedure TMATF.SetTurnAt(Val: TTurnAt);
begin
	if Val<>FTurnAt then
	begin
		FTurnAt := Val;
		ReCreateArea;
	end
end;

procedure TMATF.SetFlyPath(Val: TFlyPath);
begin
	if Val<>FFlyPath then
	begin
		FFlyPath := Val;
		ReCreateArea;
	end
end;

procedure TMATF.AssignTo(Dest: TPersistent);
var
	AnOther:	TMATF;
begin
	Inherited;
	AnOther := TMATF(Dest);

	AnOther.FTurnDistance	:= FTurnDistance;
	AnOther.FFlyPath := FFlyPath;
	AnOther.FTurnAt := FTurnAt;

	AnOther.FOutDir2 := FOutDir2;
	AnOther.FTolerSecArea := FTolerSecArea;

	AnOther.FStraightInPrimPolygon := FStraightInPrimPolygon;
	AnOther.FStraightInSecPolygon := FStraightInSecPolygon;
	AnOther.FMAPt := FMAPt;

	if Assigned(FReferenceGeometry) then
	begin
		if Assigned(AnOther.FReferenceGeometry) and
			(AnOther.FReferenceGeometry.GeometryType <> FReferenceGeometry.GeometryType) then
		begin
			AnOther.FReferenceGeometry.Free;
			AnOther.FReferenceGeometry := nil;
		end;

		if not Assigned(AnOther.FReferenceGeometry) then
			if FReferenceGeometry.GeometryType = gtPolyline then
				AnOther.FReferenceGeometry := TPolyline.Create
			else
				AnOther.FReferenceGeometry := TPolygon.Create;

		AnOther.FReferenceGeometry.Assign(FReferenceGeometry);
	end
	else
	begin
		if Assigned(AnOther.FReferenceGeometry) then
			AnOther.FReferenceGeometry.Free;
		AnOther.FReferenceGeometry := nil;
	end;
end;

procedure TMATF.CreateTolerArea;
var
	RefPgone,
	Left, TIA:			TPolygon;
	Ring:				TRing;	//,RefRing
	Part:				TPart;
	Point:				TPoint;
	tmpPt:				TPoint;
	CutterPart:			TPart;
	RefPline,
	CutterPline:		TPolyline;
	spanWidth, dir,
	lptF,
	Dist6sec,
	PilotTolerance,
	BankTolerance:		Double;
begin
	if FTurnAt = taTP then
	begin
		spanWidth := 0.5*SemiWidth;

//		if FFIXRole < MAPt_ then	dir := FOutDir
//		else
		dir := FEntryDir;

		Ring := TRing.Create;
		Part := TPart.Create;

		Point := LocalToPrj(FPrjPt, dir, -FEPT, -spanWidth);
		Ring.AddPoint(Point);

		LocalToPrj(FPrjPt, dir, -FEPT, spanWidth, Point);
		Ring.AddPoint(Point);

		lptF := (2*BYTE(FFlyMode <> fmFlyBy)-1)*FLPT;

		LocalToPrj(FPrjPt, dir, lptF, spanWidth, Point);
		Ring.AddPoint(Point);

		LocalToPrj(FPrjPt, dir, lptF, -spanWidth, Point);
		Ring.AddPoint(Point);

		FTolerArea.Clear;
		FTolerArea.AddRing(Ring);
//==
		Ring.Clear;

		LocalToPrj(FPrjPt, dir, -FEPT, -SemiWidth, Point);
		Ring.AddPoint(Point);
		Part.AddPoint(Point);

		LocalToPrj(FPrjPt, dir, -FEPT, SemiWidth, Point);
		Ring.AddPoint(Point);
		Part.AddPoint(Point);


		LocalToPrj(FPrjPt, dir, lptF, SemiWidth, Point);
		Ring.AddPoint(Point);

		LocalToPrj(FPrjPt, dir, lptF, -SemiWidth, Point);
		Ring.AddPoint(Point);

		FTolerSecArea.Clear;
		FTolerSecArea.AddRing(Ring);

		if Assigned(FReferenceGeometry) then
			if FReferenceGeometry.GeometryType = gtPolyline then
			begin
				RefPline := FReferenceGeometry.AsPolyline;
				RefPline.Clear;
			end
			else
			begin
				FReferenceGeometry.Free;
				FReferenceGeometry := nil;
			end;

		if not Assigned(FReferenceGeometry) then
			FReferenceGeometry := TPolyline.Create;

		RefPline := FReferenceGeometry.AsPolyline;
		RefPline.AddPart(Part);
//==
		Part.Free;
		Point.Free;
		Ring.Free;
	end
	else
	begin
		CutterPart := TPart.Create;
		CutterPline := TPolyline.Create;

		PilotTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;
		BankTolerance := GPANSOPSConstants.Constant[arMAPilotToleran].Value;

		Dist6sec := (FTAS + GPANSOPSConstants.Constant[dpWind_Speed].Value) * (PilotTolerance + BankTolerance);

		tmpPt := LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FTurnDistance + Dist6sec, 100000.0);
		CutterPart.AddPoint(tmpPt);

		LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FTurnDistance + Dist6sec, -100000.0, tmpPt);
		CutterPart.AddPoint(tmpPt);

		CutterPline.AddPart(CutterPart);

		Left := nil;	TIA := nil;
		GGeoOperators.Cut(FStraightInPrimPolygon, CutterPline, TGeometry(Left), TGeometry(TIA));

		FTolerArea.Assign(TIA);
		Left.Free;
		TIA.Free;

		GGeoOperators.Cut(FStraightInSecPolygon, CutterPline, TGeometry(Left), TGeometry(TIA));

		FTolerSecArea.Assign(TIA);
		Left.Free;
		TIA.Free;
//====================================================================================================
		CutterPart.Clear;
		CutterPline.Clear;

		LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FTurnDistance, 100000.0, tmpPt);
		CutterPart.AddPoint(tmpPt);

		LocalToPrj(FMAPt.PrjPt, FMAPt.EntryDirection, FTurnDistance, -100000.0, tmpPt);
		CutterPart.AddPoint(tmpPt);			tmpPt.Free;

		CutterPline.AddPart(CutterPart);	CutterPart.Free;

		Left := nil;	TIA := nil;
		GGeoOperators.Cut(FStraightInSecPolygon, CutterPline, TGeometry(Left), TGeometry(TIA));
		CutterPline.Free;


		if Assigned(FReferenceGeometry) then
			if FReferenceGeometry.GeometryType = gtPolygon then
			begin
				RefPgone := FReferenceGeometry.AsPolygon;
				RefPgone.Clear;
			end
			else
			begin
				FReferenceGeometry.Free;
				FReferenceGeometry := nil;
			end;

		if not Assigned(FReferenceGeometry) then
			FReferenceGeometry := TPolygon.Create;

		RefPgone := FReferenceGeometry.AsPolygon;
		RefPgone.Assign(TIA);

		Left.Free;
		TIA.Free;
	end;
end;

// TMAPt =======================================================================
constructor TMAPt.Create(UI: TUIContract);
begin
	Create(MAPt_, UI);
end;

constructor TMAPt.Create(InitialRole: TFIXRole; UI: TUIContract);
begin
	Inherited Create(MAPt_, UI);

	FSOCDistance	:= 0.0;

	FSOCAreaSymbol := TFillSymbol.Create;
//	FSOCAreaSymbol.Color := RGB(Random(256), Random(256), Random(256));
//	FSOCAreaSymbol.Style := TFillStyle(Random(8));

	FSOCAreaSymbol.Color := RGB(255, 0, 255);
	FSOCAreaSymbol.Style := sfsCross;

	FSOCPrjPt		:= TPoint.Create;
	FSOCGeoPt		:= TPoint.Create;

	FStMAArea		:= TPolygon.Create;
	FSOCArea		:= TPolygon.Create;

	FSOCAreaElement	:= -1;
	FSOCElement		:= -1;
end;

procedure TMAPt.AssignTo(Dest: TPersistent);
var
	AnOther:	TMAPt;
begin
	Inherited;
	AnOther := TMAPt(Dest);

	AnOther.FSOCAreaElement	:= -1;
	AnOther.FSOCElement		:= -1;
	AnOther.FSOCDistance	:= FSOCDistance;

	AnOther.FSOCAreaSymbol.Assign(FSOCAreaSymbol);
	AnOther.FSOCPrjPt.Assign(FSOCPrjPt);
	AnOther.FSOCGeoPt.Assign(FSOCGeoPt);
	AnOther.FStMAArea.Assign(FStMAArea);
	AnOther.FSOCArea.Assign(FSOCArea);
end;

destructor TMAPt.Destroy;
begin
	FSOCAreaSymbol.Free;
	FSOCPrjPt.Free;
	FSOCGeoPt.Free;
	FStMAArea.Free;
	FSOCArea.Free;
	Inherited;
end;

procedure TMAPt.SetPoint(NewPoint: TPoint);
var
	tmpPt:	TPoint;
begin
	FPrjPt.Assign(NewPoint);

	if not FPrjPt.IsEmpty then
	begin
		tmpPt := GGeoOperators.geoTransformations (FPrjPt, GPrjSR, GGeoSR).AsPoint;
		FGeoPt.Assign(tmpPt);
		tmpPt.Free;

		LocalToPrj (FPrjPt, FOutDir, FSOCDistance, 0.0, FSOCPrjPt);

		tmpPt := GGeoOperators.geoTransformations (FSOCPrjPt, GPrjSR, GGeoSR).AsPoint;
		FSOCGeoPt.Assign(tmpPt);
		tmpPt.Free;
		ReCreateArea;
	end
	else
	begin
		FGeoPt.Assign(NewPoint);
		FSOCPrjPt.Assign(NewPoint);
		FSOCGeoPt.Assign(NewPoint);
	end;
end;

procedure TMAPt.DeleteGraphics;
begin
	Inherited;
	FUI.SafeDeleteGraphic(FSOCAreaElement);
	FUI.SafeDeleteGraphic(FSOCElement);
	FSOCAreaElement	:= -1;
	FSOCElement		:= -1;
end;

procedure TMAPt.RefreshGraphics;


begin
	if not FDrawingEnabled then
	begin
		DeleteGraphics;
		exit;
	end;

	FUI.SafeDeleteGraphic(FSOCAreaElement);
	FUI.SafeDeleteGraphic(FSOCElement);

	FSOCAreaElement := FUI.DrawPolygon(FSOCArea, FSOCAreaSymbol);
	Inherited;
	FSOCElement := FUI.DrawPointWithText(FSOCPrjPt, FPointSymbol, 'SOC');

end;

procedure TMAPt.SetStMAArea(Val: TPolygon);
begin
	FStMAArea.Assign(Val);
	ReCreateArea;
end;

procedure TMAPt.CreateTolerArea;
var
	Point:			TPoint;
	tmpPt:			TPoint;
	CutterPart:		TPart;
	Ring:			TRing;
	CutterPline:	TPolyline;
	Left, SOCArea:	TPolygon;
	Tan15,
	Tan15_2,
	Width:			Double;
begin
	Inherited;
{	Ring := TRing.Create;

	Point := LocalToPrj(FPrjPt, FOutDir, -FATT, -FXXT);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, FOutDir, -FATT, FXXT, Point);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, FOutDir, FATT, FXXT, Point);
	Ring.AddPoint(Point);

	LocalToPrj(FPrjPt, FOutDir, FATT, -FXXT, Point);
	Ring.AddPoint(Point);

	FTolerance.Clear;
	FTolerance.AddRing(Ring);
}
//SOC Area
	if FStMAArea.Count = 0 then
	begin
		Ring := TRing.Create;

		Point := LocalToPrj(FPrjPt, FOutDir, -FATT, -FASW_2_R);
		Ring.AddPoint(Point);

		LocalToPrj(FPrjPt, FOutDir, -FATT, FASW_2_L, Point);
		Ring.AddPoint(Point);

		Tan15 := Tan(GPANSOPSConstants.Constant[arafTrn_OSplay].Value);

		Tan15_2 := FASW_2_L/FASW_L * Tan15;
		Width := FXXT + (FSOCDistance + FATT) * Tan15_2;

		LocalToPrj(FPrjPt, FOutDir, FSOCDistance, Width, Point);
		Ring.AddPoint(Point);

		Tan15_2 := FASW_2_R/FASW_R * Tan15;
		Width := FXXT + (FSOCDistance + FATT) * Tan15_2;

		LocalToPrj(FPrjPt, FOutDir, FSOCDistance, -Width, Point);
		Ring.AddPoint(Point);

		FSOCArea.Clear;
		FSOCArea.AddRing(Ring);

		Point.Free;
		Ring.Free;
	end
	else
	begin
		Left := nil;	SOCArea := nil;
		tmpPt := LocalToPrj(PrjPt, EntryDirection, FSOCDistance, 100000.0);

		CutterPart := TPart.Create;
		CutterPline := TPolyline.Create;

		CutterPart.AddPoint(tmpPt);
		LocalToPrj(PrjPt, EntryDirection, SOCDistance, -100000.0, tmpPt);
		CutterPart.AddPoint(tmpPt);				tmpPt.Free;
		CutterPline.AddPart(CutterPart);		CutterPart.Free;

		GGeoOperators.Cut(FStMAArea, CutterPline, TGeometry(Left), TGeometry(SOCArea));
		FSOCArea.Assign(SOCArea);

		Left.Free;
		SOCArea.Free;
		CutterPline.Free;
	end;
end;

// TFIXInfo ====================================================================
constructor TFIXInfo.Create;
begin
	Inherited;
	FFIX := nil;
	FPrev := nil;
	FNext := nil;

	FDistParameter := nil;
	FAltitudeParameter := nil;
	FGradientParameter := nil;
	FAngleParameter := nil;
	FSpeedParameter := nil;
end;

constructor TFIXInfo.Create(InitialFIX: TWayPoint);
begin
	Inherited Create;
	FFIX := InitialFIX;
	FPrev := nil;
	FNext := nil;

	FDistParameter := nil;
	FAltitudeParameter := nil;
	FGradientParameter := nil;
	FAngleParameter := nil;
	FSpeedParameter := nil;
end;

destructor TFIXInfo.Destroy;
begin
	Inherited;
end;

function TFIXInfo.GetDistance: Double;
begin
	if Assigned(FDistParameter) then
	begin
		Result := FDistParameter.Value
	end
	else
		Result := NAN;
end;

function TFIXInfo.GetAltitude: Double;
begin
	if Assigned(FAltitudeParameter) then
	begin
		Result := FAltitudeParameter.Value;
		FFIX.Altitude := Result;
	end
	else
		Result := NAN;
end;

function TFIXInfo.GetGradient: Double;
begin
	if Assigned(FGradientParameter) then
	begin
		Result := FGradientParameter.Value;
		FFIX.Gradient := Result
	end
	else
		Result := NAN;
end;

function TFIXInfo.GetAngle: Double;
begin
	if Assigned(FAngleParameter) then
	begin
		Result := FAngleParameter.Value;
		FFIX.OutDirection := Result
//		FFIX.EntryDirection := Result
	end
	else
		Result := NAN;
end;

function TFIXInfo.GetSpeed: Double;
begin
	if Assigned(FSpeedParameter) then
	begin
		Result := FSpeedParameter.Value;
		FFIX.IAS := Result;
	end
	else
		Result := NAN;
end;

procedure TFIXInfo.SetDistance(Val: Double);
begin
	if Assigned(FDistParameter) then
	begin
		FDistParameter.BeginUpdate;
		FDistParameter.Value := Val;
		FDistParameter.EndUpdate;
	end;
end;

procedure TFIXInfo.SetAltitude(Val: Double);
begin
	FFIX.Altitude := Val;
	if Assigned(FAltitudeParameter) then
	begin
		FAltitudeParameter.BeginUpdate;
		FAltitudeParameter.Value := Val;
		FAltitudeParameter.EndUpdate;
	end;
end;

procedure TFIXInfo.SetGradient(Val: Double);
begin
	FFIX.Gradient := Val;
	if Assigned(FGradientParameter) then
	begin
		FGradientParameter.BeginUpdate;
		FGradientParameter.Value := Val;
		FGradientParameter.EndUpdate;
	end;
end;

procedure TFIXInfo.SetAngle(Val: Double);
begin
//	FFIX.EntryDirection := Val;
	FFIX.OutDirection := Val;
	if Assigned(FAngleParameter) then
	begin
		FAngleParameter.BeginUpdate;
		FAngleParameter.Value := Val;
		FAngleParameter.EndUpdate;
	end;
end;

procedure TFIXInfo.SetSpeed(Val: Double);
begin
	FFIX.IAS := Val;
	if Assigned(FSpeedParameter) then
	begin
		FSpeedParameter.BeginUpdate;
		FSpeedParameter.Value := Val;
		FSpeedParameter.EndUpdate;
	end;
end;

end.
