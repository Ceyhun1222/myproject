unit SaveDForm;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, AIXM_TLB, ParameterContainerFrame, Parameter, ComCtrls,
  Geometry;

type
  TSaveForm = class(TForm)
	bCancel: TButton;
	bOK: TButton;
    grbProc: TGroupBox;
    labProcName: TLabel;
    editProcReadOnlyName: TEdit;
    labProcNote: TLabel;
    grbTransition: TGroupBox;
    labTransitionName: TLabel;
    editTransitionName: TEdit;
    grbSegmentLeg: TGroupBox;
    labSegmentLegList: TLabel;
    lbSegmentLeg: TListBox;
    labSegmentLegNote: TLabel;
    pcUpperAlt: TParamContainer;
    pcLowerAlt: TParamContainer;
    grbDesignatedPoint: TGroupBox;
    labDesignatedPointName: TLabel;
    labDesPointREPATC: TLabel;
    labDesPointMagVarDate: TLabel;
    editDesignatedPointName: TEdit;
    cbDesPointREPATC: TComboBox;
    pcDesPointMagVar: TParamContainer;
    dtpDesPointMagVar: TDateTimePicker;
    chbDesPointOnShore: TCheckBox;
    labDesPointNote: TLabel;
    labDesPointDesignator: TLabel;
    editDesPointDesignator: TEdit;
    labDesPointPosition: TLabel;
    editProcedureName: TEdit;
    editProcNote: TMemo;
    editSegmentLegNote: TMemo;
    editDesPointNote: TMemo;
	procedure bOKClick(Sender: TObject);
	procedure bCancelClick(Sender: TObject);
	procedure lbSegmentLegClick(Sender: TObject);
    procedure editSegmentLegNoteChange(Sender: TObject);
	procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure pcUpperAltPChangeValue(Sender: TCustomParameter);
    procedure editDesignatedPointNameChange(Sender: TObject);
    procedure cbDesPointREPATCSelect(Sender: TObject);
    procedure pcDesPointMagVarPChangeValue(Sender: TCustomParameter);
    procedure dtpDesPointMagVarChange(Sender: TObject);
    procedure chbDesPointOnShoreClick(Sender: TObject);
    procedure editDesPointDesignatorChange(Sender: TObject);
	procedure editDesPointNoteChange(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
	FAIXMProcedure: IAIXMProcedure;
	procedure SetAIXMProcedure (value: IAIXMProcedure);
	function CheckProcedureName (): Boolean;
	function SegmentPathTypeToStr (legTypeArinc: Integer) : String;
	procedure FillControlMUI ();
  public

	property AIXMProcedure: IAIXMProcedure read FAIXMProcedure write SetAIXMProcedure;
  end;

	TLocalSegmentLeg = class
	private
		FPosition: Geometry.TPoint;
	public
		Number: Integer;
		Note: String;
		Lower: Double;
		Uppwer: Double;
		UppwerValue: Double;
		CreatedPoint: Boolean;

		PointDesignator: String;
		PointName: String;
		PointNote: String;
		PointREPATC: Integer;
		MagVar: Double;
		MagVarDate: TDate;
		OnShore: Boolean;
		property Position: Geometry.TPoint read FPosition;

		constructor Create ();
		destructor Destroy (); override;
	end;

var
  SaveForm: TSaveForm;

implementation

uses
	StrUtils,
	ARANGlobals;

{$R *.dfm}

procedure TSaveForm.bOKClick(Sender: TObject);
var
	i: Integer;
	procTransition: IProcedureTransition;
	localLeg: TLocalSegmentLeg;
	departureLeg: IDepartureLeg;
	segmentLeg: ISegmentLeg;
	segmentPoint: ISegmentPoint;
	designatedTag: String;
begin
	if not CheckProcedureName () then
	begin
		MessageDlg (GMuiStrings.ProcNameNotCorrect, mtWarning, [mbOK], 0);
		exit;
	end;
	for i := 0 to lbSegmentLeg.Items.Count - 1 do
	begin
		localLeg := TLocalSegmentLeg (lbSegmentLeg.Items.Objects [i]);
		if (localLeg.CreatedPoint) and (localLeg.PointDesignator = '') then
		begin
			MessageDlg ('Leg - ' + IntToStr (i + 1) + ' has not Designator.', mtWarning, [mbOK], 0);
			exit;
		end;
	end;


	FAIXMProcedure.Name := editProcReadOnlyName.Text + editProcedureName.Text;
	FAIXMProcedure.Note := editProcNote.Text;

	procTransition := FAIXMProcedure.TransitionList.GetItem (0);
	procTransition.Description := editTransitionName.Text;

	for i := 0 to lbSegmentLeg.Items.Count - 1 do
	begin
		localLeg := TLocalSegmentLeg (lbSegmentLeg.Items.Objects [i]);
		departureLeg := procTransition.LegList.AsISIDList.GetItem (i);
		segmentLeg := departureLeg as ISegmentLeg;
		segmentLeg.Note := localLeg.Note;
		segmentLeg.UpperLimitAltitude.value := FloatToStr (localLeg.Uppwer);

		if localLeg.CreatedPoint then
		begin
			designatedTag := '';
			segmentPoint := segmentLeg.EndPoint as ISegmentPoint;
			segmentPoint.PointChoice.FixDesignatedPoint.Designator := localLeg.PointDesignator;
			segmentPoint.PointChoice.FixDesignatedPoint.Note := localLeg.PointNote;
			segmentPoint.PointChoice.FixDesignatedPoint.Name := localLeg.PointName;

			if localLeg.PointREPATC = 0 then designatedTag := 'C'
			else if localLeg.PointREPATC = 0 then designatedTag := 'R'
			else designatedTag := 'N';

			designatedTag := designatedTag + ';' +
				FloatToStr (localLeg.MagVar) + ';' +
				FormatDateTime ('mm-dd-yyyy', localLeg.MagVarDate) + ';' +
				IfThen (localLeg.OnShore, '1', '0');

			segmentPoint.PointChoice.FixDesignatedPoint.Tag := designatedTag;
		end;
	end;

	ModalResult := mrOK;
end;

procedure TSaveForm.bCancelClick(Sender: TObject);
begin
	ModalResult := mrCancel;
end;

procedure TSaveForm.SetAIXMProcedure (value: IAIXMProcedure);
var
	i: Integer;
	procTransition: IProcedureTransition;
	departureLegList: IDepartureLegList;
	departureLeg: IDepartureLeg;
	segmentLeg: ISegmentLeg;
	segmentPoint: ISegmentPoint;
	localLeg: TLocalSegmentLeg;
begin
	FAIXMProcedure := value;
	procTransition := FAIXMProcedure.TransitionList.GetItem (0);

	editProcReadOnlyName.Text := FAIXMProcedure.Name;
	editTransitionName.Text := procTransition.Description;

	departureLegList := procTransition.LegList.AsISIDList;

	for i := 0 to lbSegmentLeg.Items.Count - 1 do
		lbSegmentLeg.Items.Objects [i].Free;
	lbSegmentLeg.Items.Clear ();

	for i := 0 to departureLegList.Count - 1 do
	begin
		departureLeg := departureLegList.GetItem (i);
		segmentLeg := departureLeg as ISegmentLeg;

		localLeg := TLocalSegmentLeg.Create ();
		localLeg.Number := departureLeg.AsISegmentLeg.SeqNumberARINC.value;
		localLeg.Note := departureLeg.AsISegmentLeg.Note;

		localLeg.Lower := StrToFloat (segmentLeg.LowerLimitAltitude.Value);
		localLeg.Uppwer := StrToFloat (segmentLeg.UpperLimitAltitude.Value);
		localLeg.UppwerValue := localLeg.Uppwer;

		if segmentLeg.EndPoint <> nil then
		begin
			segmentPoint := segmentLeg.EndPoint as ISegmentPoint;
			localLeg.CreatedPoint := (segmentPoint.PointChoice.FixDesignatedPoint.Id = '0');
			localLeg.PointDesignator := segmentPoint.PointChoice.FixDesignatedPoint.Designator;
			localLeg.Position.SetCoords (segmentPoint.PointChoice.FixDesignatedPoint.Point.AsIGMLPoint.X,
				segmentPoint.PointChoice.FixDesignatedPoint.Point.AsIGMLPoint.Y);
		end;

		lbSegmentLeg.Items.AddObject (
			'Leg - ' + IntToStr (localLeg.Number) + ' (' + SegmentPathTypeToStr (segmentLeg.LegTypeARINC) + ')',
			localLeg);
	end;

	if lbSegmentLeg.Items.Count > 0 then
	begin
		lbSegmentLeg.Selected [0] := True;
		lbSegmentLegClick (nil);
	end;
end;

procedure TSaveForm.lbSegmentLegClick (Sender: TObject);
var
	localLeg: TLocalSegmentLeg;
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;

	localLeg := TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]);
	editSegmentLegNote.Text := localLeg.Note;

	pcLowerAlt.P.ChangeValue (localLeg.Lower, False);
	
	pcUpperAlt.P.BeginUpdate (False);
	pcUpperAlt.P.MinValue := localLeg.Lower;
	pcUpperAlt.P.MaxValue := localLeg.Uppwer;
	pcUpperAlt.P.Value := localLeg.UppwerValue;
	pcUpperAlt.P.EndUpdate;

	editDesPointDesignator.Text := localLeg.PointDesignator;

	if not localLeg.CreatedPoint then
	begin
		grbDesignatedPoint.Height := 73;
		editDesPointDesignator.ReadOnly := True;
		editDesPointDesignator.Color := clBtnFace;
	end
	else
	begin
		grbDesignatedPoint.Height := 266;
		editDesPointDesignator.ReadOnly := False;
		editDesPointDesignator.Color := clWindow;
		cbDesPointREPATC.ItemIndex := localLeg.PointREPATC;
		pcDesPointMagVar.P.ChangeValue (localLeg.MagVar);
		dtpDesPointMagVar.Date := localLeg.MagVarDate;
		chbDesPointOnShore.Checked := localLeg.OnShore;
		editDesignatedPointName.Text := localLeg.PointName;
		editDesPointNote.Text := localLeg.PointNote;
	end;

	if localLeg.Position.IsEmpty then
		labDesPointPosition.Caption := ''
	else
		labDesPointPosition.Caption := GMuiStrings.Lat + ': ' + FormatFloat ('0.0000', localLeg.Position.Y) + '   ' +
			GMuiStrings.Long + ': ' + FormatFloat ('0.0000', localLeg.Position.X);

end;

procedure TSaveForm.editSegmentLegNoteChange (Sender: TObject);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).Note := editSegmentLegNote.Text;
end;

procedure TSaveForm.FormClose(Sender: TObject; var Action: TCloseAction);
var
	i: Integer;
begin
	for i := 0 to lbSegmentLeg.Items.Count - 1 do
		lbSegmentLeg.Items.Objects [i].Free;
end;

procedure TSaveForm.pcUpperAltPChangeValue(Sender: TCustomParameter);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).UppwerValue := pcUpperAlt.P.Value;
end;

function TSaveForm.CheckProcedureName (): Boolean;
var
	editText: String;
	editTextLen: Integer;
	charVal: Char;
begin
	result := False;
	if editProcReadOnlyName.Text = '' then
		exit;
	if Length (editProcedureName.Text) <> 2 then
		exit;
	charVal := editProcedureName.Text [1];
	if (charVal < '0') or (charVal > '9') then
		exit;
	charVal := editProcedureName.Text [2];
	if ((charVal < 'A') or (charVal > 'z')) then
		exit;
	result := True;
end;

procedure TSaveForm.editDesignatedPointNameChange (Sender: TObject);
begin
	if (lbSegmentLeg.ItemIndex = -1) or (editDesignatedPointName.ReadOnly) then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).PointName := editDesignatedPointName.Text;
end;

procedure TSaveForm.cbDesPointREPATCSelect(Sender: TObject);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).PointREPATC := cbDesPointREPATC.ItemIndex;
end;

procedure TSaveForm.pcDesPointMagVarPChangeValue (Sender: TCustomParameter);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).MagVar := pcDesPointMagVar.P.Value;
end;

procedure TSaveForm.dtpDesPointMagVarChange(Sender: TObject);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).MagVarDate := dtpDesPointMagVar.Date;
end;

procedure TSaveForm.chbDesPointOnShoreClick(Sender: TObject);
begin
	if lbSegmentLeg.ItemIndex = -1 then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).OnShore := chbDesPointOnShore.Checked;
end;

procedure TSaveForm.editDesPointDesignatorChange(Sender: TObject);
begin
	if (lbSegmentLeg.ItemIndex = -1) or (editDesignatedPointName.ReadOnly) then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).PointDesignator := editDesPointDesignator.Text;
	if (lbSegmentLeg.ItemIndex = lbSegmentLeg.Items.Count - 1) and (FAIXMProcedure.Name = '') then
		editProcReadOnlyName.Text := editDesPointDesignator.Text;
end;

procedure TSaveForm.editDesPointNoteChange(Sender: TObject);
begin
	if (lbSegmentLeg.ItemIndex = -1) or (editDesignatedPointName.ReadOnly) then
		exit;
	TLocalSegmentLeg (lbSegmentLeg.Items.Objects [lbSegmentLeg.ItemIndex]).PointNote := editDesPointNote.Text;
end;

function TSaveForm.SegmentPathTypeToStr (legTypeArinc: Integer) : String;
begin
	if legTypeArinc = SegmentPathType_IF then
		result := 'IF'
	else if legTypeArinc = SegmentPathType_CF then
		result := 'CF'
	else if legTypeArinc = SegmentPathType_CA then
		result := 'CA'
	else if legTypeArinc = SegmentPathType_DF then
		result := 'DF'
	else if legTypeArinc = SegmentPathType_TF then
		result := 'TF'
	else
		result := '';
end;

{ TLocalSegmentLeg }

constructor TLocalSegmentLeg.Create ();
begin
	FPosition := TPoint.Create ();
	PointDesignator := '';
	PointName := '';
	PointNote := '';
	CreatedPoint := False;
	PointREPATC := 0;
	MagVar := 0;
	MagVarDate := Now;
	OnShore := False;
end;

destructor TLocalSegmentLeg.Destroy ();
begin
	FPosition.Free;
  inherited;
end;

procedure TSaveForm.FormCreate(Sender: TObject);
begin
	FillControlMUI ();
end;

procedure TSaveForm.FillControlMUI ();
begin
	GMuiLoader.LoadString (self, 501);
	GMuiLoader.LoadString (grbProc, 502);
	GMuiLoader.LoadString (labProcName, 109);
	GMuiLoader.LoadString (labProcNote, 503);
	GMuiLoader.LoadString (labSegmentLegNote, 503);
	GMuiLoader.LoadString (labDesPointNote, 503);
	GMuiLoader.LoadString (grbTransition, 504);
	GMuiLoader.LoadString (labTransitionName, 505);
	GMuiLoader.LoadString (grbSegmentLeg, 506);
	GMuiLoader.LoadString (labSegmentLegList, 507);
	GMuiLoader.LoadString (pcLowerAlt.DescriptionControl, 508);
	GMuiLoader.LoadString (pcUpperAlt.DescriptionControl, 509);
	GMuiLoader.LoadString (grbDesignatedPoint, 510);
	GMuiLoader.LoadString (labDesPointDesignator, 511);
	GMuiLoader.LoadString (GMuiStrings.Lat, 512);
	GMuiLoader.LoadString (GMuiStrings.Long, 513);
	GMuiLoader.LoadString (labDesPointREPATC, 514);
	GMuiLoader.LoadString (pcDesPointMagVar.DescriptionControl, 515);
	GMuiLoader.LoadString (labDesPointMagVarDate, 516);
	GMuiLoader.LoadString (chbDesPointOnShore, 517);
	GMuiLoader.LoadString (bCancel, 518);
end;

end.
