unit UIContract;

interface

uses
	Windows, Contract, Geometry, Classes, Controls, Graphics, GeometryOperatorsContract;

type
	IntArray = array of Integer;
	//----------------------------------------------------------------------------
	TUIBitmap = class
	private
		FWidth:		integer;
		FHeight:	integer;
		FBits:		IntArray;
		function GetBits: PInteger;
	public
		constructor Create;									overload;
		constructor Create (const from: Graphics.TBitmap);	overload;

		destructor Destroy;	override;

		procedure SetSize (NewWidth, NewHeight: integer);
		procedure Clear;

		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);
		procedure Assign (const src: TUIBitmap);
		function Clone: TUIBitmap;

		property Width: Integer read FWidth;
		property Height: Integer read FHeight;
		property Bits: PInteger read GetBits;
	end;
  //----------------------------------------------------------------------------
	TUICursor = class
	private
		FX:				Integer;
		FY:				Integer;
		FWidth:			Integer;
		FHeight:		Integer;
		FHaveXorMask:	Boolean;
		FAndMask:		array of Integer;
		FXorMask:		array of Integer;
		function GetAndMask: PInteger;
		function GetXorMask: PInteger;
	public
		constructor Create;							overload;
		constructor Create(IsColored: boolean);		overload;
		constructor Create(const from: THandle);	overload;

		destructor Destroy;	override;
		procedure SetSpot (SpotX, SpotY: integer);
		procedure SetSize (NewWidth, NewHeight: integer);
		procedure Clear;

		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);
		procedure Assign (const src: TUICursor);
		function Clone: TUICursor;

		property Width: Integer read FWidth;
		property Height: Integer read FHeight;
		property X: Integer read FX;
		property Y: Integer read FY;

		property Colored: Boolean read FHaveXorMask;
		property AndMask: PInteger read GetAndMask;
		property XorMask: PInteger read GetXorMask;
  end;
  //----------------------------------------------------------------------------
	TPointStyle =
	(
		smsCircle,
		smsSquare,
		smsCross,
		smsX,
		smsDiamond
	);
  //----------------------------------------------------------------------------
	TSymbol = class
	private
		FColor:	Integer;
		FSize:	Integer;
	protected
		constructor Create (InitColor, InitSize: Integer); overload;
		property Size:	Integer read FSize write FSize;
	public
		constructor Create; overload;

		procedure Pack (handle: TRegistryHandle); virtual;
		procedure Unpack (handle: TRegistryHandle); virtual;
		procedure Assign (const src: TSymbol); virtual;
		function Clone: TSymbol; virtual;

		property Color:	Integer read FColor write FColor;
	end;
  //----------------------------------------------------------------------------
	TPointSymbol = class (TSymbol)
	private
		FStyle:	TPointStyle;
	public
		constructor Create; overload;
		constructor Create (InitStyle: TPointStyle; InitColor, InitSize: Integer); overload;

		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;
		procedure Assign (const src: TSymbol); override;
		function Clone: TSymbol; override;

		property Style:	TPointStyle read FStyle write FStyle;
		property Size;
  end;
  //----------------------------------------------------------------------------
  TLineStyle =
  (
	  slsSolid,
	  slsDash,
	  slsDot,
	  slsDashDot,
	  slsDashDotDot,
	  slsNull,
	  slsInsideFrame
  );
  //----------------------------------------------------------------------------
	TLineSymbol = class (TSymbol)
	private
		FStyle:	TLineStyle;
	public
		constructor Create; overload;
		constructor Create (InitStyle: TLineStyle; InitColor, InitSize: integer); overload;

		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;
		procedure Assign (const src: TSymbol); override;
		function Clone: TSymbol; override;

		property Style:	TLineStyle read FStyle write FStyle;
		property Size;
  end;
  //----------------------------------------------------------------------------
	TFillStyle =
	(
		sfsSolid,
		sfsNull,
		sfsHorizontal,
		sfsVertical,
		sfsForwardDiagonal,
		sfsBackwardDiagonal,
		sfsCross,
		sfsDiagonalCross
	);

const
	sfsHollow = sfsNull;

  //----------------------------------------------------------------------------
type
	TFillSymbol = class (TSymbol)
	private
		FStyle:		TFillStyle;
		FOutline:	TLineSymbol;
	private
		procedure SetOutline(Val: TLineSymbol);
	public
		constructor Create;
		destructor Destroy; override;

		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;
		procedure Assign (const src: TSymbol); override;
		function Clone: TSymbol; override;

		property Style:	TFillStyle read FStyle write FStyle;
		property Outline: TLineSymbol read FOutline write SetOutline;
  end;
  //----------------------------------------------------------------------------
  TUICommand =
  (
	uiGetEnvInfo = 0,		// AppName, AppVersion, MainWindowHandle
	uiGetRelatedFileName,

	uiGetViewProjection = 10,
	uiSetViewProjection,	//11

	uiGetExtent,
	uiSetExtent,
	uiGetDocumentLayerList,
	uiGetDocumentMap,

	//uiSetMainWindowListener,	//19

	uiDisplayMessage = 20,
	uiDrawPoint,
	uiDrawPointWithText,
	uiDrawPolyline,
	uiDrawPolygon,
	uiSetVisible,
	uiDeleteGraphic,
	uiShowAnimation,

	uiCreateToolBar = 30,
	uiCreateMenuBar,
	uiCreateToolButton,
	uiCreateCommandButton,
	uiDeleteControl,

	uiControlSetStyle = 40,
	uiControlSetCategory,
	uiControlSetCaption,

	uiControlSetChecked = 50,
	uiControlSetEnabled,
	uiToolButtonSetDeactivate,
	uiToolButtonSetDown,

	uiControlSetBitmap = 60,
	uiControlSetCursor,

	uiControlSetTooltip = 70,
	uiControlSetMessage,
	uiControlSetHelpID,
	uiControlSetHelpFile,

	uiControlSetOnClickHandler = 80,
	uiControlSetOnMouseMoveHandler,
	uiControlSetOnMouseDownHandler,
	uiControlSetOnMouseUpHandler,
	uiControlSetOnDblClickHandler,

	uiToolBarAddToolButton = 90,
	uiToolBarAddCommandButton,
	uiToolBarGetVisible,
	uiToolBarSetVisible,

	uiGetVisiblePoints = 100,
	uiGetVisiblePolylines,
	uiGetVisiblePolygons,

	uiGetSelectedPoints,
	uiGetSelectedPolylines,
	uiGetSelectedPolygons,

	uiControlOnClick = 1000,
	uiControlOnMouseMove,
	uiControlOnMouseDown,
	uiControlOnMouseUp,
	uiControlOnDblClick,
	uiControlOnKeyDown,
	uiControlOnKeyUp,

	uiControlOnDeactivate,
	uiControlOnContextMenu,

	uiMainWindowOnClick = 1100,
	uiMainWindowOnMouseMove,
	uiMainWindowOnMouseDown,
	uiMainWindowOnMouseUp,
	uiMainWindowOnDblClick,
	uiMainWindowOnKeyDown,
	uiMainWindowOnKeyUp,
	uiMainWindowOnDeactivate,
	uiMainWindowOnContextMenu,

  uiCreateNewToolItem = 1200
  );
//----------------------------------------------------------------------------
	TToolBar = class;
	TMenuBar = class;
	TToolButton = class;
	TCommandButton = class;
	TUIControl = class;

	TUIContract = class
	private
		FHandle:	TRegistryHandle;
		FControls:	TList;

		function CreateControl (const name: WideString; command: TUICommand): Integer;
		procedure AddControl (control: TUIControl);
		function FindControl (id: integer): TUIControl;
		procedure RemoveControl (control: TUIControl);
		procedure ShowAnimation (const show: Boolean); overload;
		function GetViewProjection: TSpatialReference;
		procedure SetViewProjection (SpatialReference: TSpatialReference);
		function GetDocumentMap (): Cardinal;
	public
		constructor Create;
		destructor Destroy; override;

		function IsValid: boolean;
		procedure GetEnvInfo(var ProgName: String; var info0, info1, info2, info3: Integer);
		function GetRelatedFileName: WideString;

		procedure DisplayMessage (pos: integer; const msg: WideString);

		procedure GetExtent (var xmin, ymin, xmax, ymax: double);
		procedure SetExtent (xmin, ymin, xmax, ymax: double);

		function DrawPoint (const point: TPoint; const symbol: TPointSymbol): Integer; overload;
		function DrawPoint (const point: TPoint; const Color: Integer): Integer; overload;
		function DrawPoint (const point: TPoint; const Color: Integer; const Style: TPointStyle): Integer; overload;

		function DrawPointWithText (const point: TPoint; const symbol: TPointSymbol; const text: WideString): Integer; overload;
		function DrawPointWithText (const point: TPoint; const Color: Integer; const text: WideString): Integer; overload;

		function DrawPolyline (const polyline: TPolyline; const symbol: TLineSymbol): Integer; overload;
		function DrawPolyline (const polyline: TPolyline; const Color, Width: Integer): Integer; overload;

		function DrawPolygon (const polygon: TPolygon; const symbol: TFillSymbol): Integer; overload;
		function DrawPolygon (const polygon: TPolygon; const Color: Integer; const Style: TFillStyle): Integer; overload;

		function DrawRing (const ring: TRing; const symbol: TFillSymbol): Integer; overload;
		function DrawRing (const ring: TRing; const Color: Integer; const Style: TFillStyle): Integer; overload;

		procedure SetVisible (graphicHandle: integer; isVisible: boolean);
		function DeleteGraphic (const Handle: Integer): Integer;
		procedure ShowAnimation (); overload;
		procedure HideAnimation ();
		function SafeDeleteGraphic (var Handle: Integer): Integer;

		procedure GetVisiblePoints(var Points, Handlers: TList);		overload;
		procedure GetVisiblePolylines(var Polylines, Handlers: TList);	overload;
		procedure GetVisiblePolygons(var Polygons, Handlers: TList);	overload;

		function GetVisiblePoints: TList;								overload;
		function GetVisiblePolylines: TList;							overload;
		function GetVisiblePolygons: TList;								overload;

		procedure GetSelectedPoints(var Points, Handlers: TList);		overload;
		procedure GetSelectedPolylines(var Polylines, Handlers: TList);	overload;
		procedure GetSelectedPolygons(var Polygons, Handlers: TList);	overload;

		function GetSelectedPoints: TList;								overload;
		function GetSelectedPolylines: TList;							overload;
		function GetSelectedPolygons: TList;							overload;


		function CreateToolBar (const name: WideString): TToolBar;
		function CreateMenuBar (const name: WideString): TMenubar;
		function CreateToolButton (const name: WideString): TToolButton;
		function CreateCommandButton (const name: WideString): TCommandButton;

		function GetDocumentLayerList (): TStringArray;

		property ViewProjection: TSpatialReference read GetViewProjection write SetViewProjection;
		property DocumentMap: Cardinal read GetDocumentMap;

    procedure CreateNewToolItem (name: WideString);
	end;
//----------------------------------------------------------------------------
	TControlStyle =
	(
		csTextOnly,		// Display text only.
		csIconOnly,		// Display icon only.
		csIconAndText,	// Display icon and text.
		csMenuBar		// Display bar as main menu.
	);
	TToolDeactivate = (tdFalse = 0, tdTrue = 1, tdAsk = 2);
//----------------------------------------------------------------------------
	TNotifyHandler = procedure (handlerData: Pointer; sender: TObject);
	TUIMouseEvent = procedure (Sender: TObject; button: TMouseButton; shift: TShiftState; x, y: integer; fx, fy: double) of object;
	TMouseHandler = procedure (handlerData: Pointer; sender: TObject; button: TMouseButton; shift: TShiftState; x, y: integer; fx, fy: double);

	TUIControl = class
	public
		destructor Destroy; override;

		procedure SetStyle (style: TControlStyle);
		procedure SetCaption (const value: WideString);
		procedure SetBitmap (const value: TUIBitmap);
		procedure SetCursor (const value: TUICursor);
		procedure SetToolTip (const value: WideString);

		procedure SetCategory (const value: WideString);
		procedure SetChecked (const value: Boolean);
		procedure SetEnabled (const value: Boolean);
		procedure SetMessage (const value: WideString);
		procedure SetHelpID (const value: Integer);
		procedure SetHelpFile (const value: WideString);

		procedure SetOnClickHandler (handler: TNotifyEvent); overload;
		procedure SetOnClickHandler (handlerData: Pointer; handler: TNotifyHandler); overload;
	public
		id: integer;
	protected
		constructor Create (uiContract: TUIContract; id: integer);
	protected
		_onClick:	TNotifyEvent;
		_onClickHandler: TNotifyHandler;
		_onClickHandlerData: Pointer;

		_uiContract: TUIContract;
	end;
//----------------------------------------------------------------------------

	TToolBar = class (TUIControl)
	private
		function GetVisible: Boolean;
		procedure SetVisible(Value: Boolean);
	public
		procedure AddToolButton (toolButton: TToolButton);
		procedure AddCommandButton (commandButton: TCommandButton);
		property Visible: Boolean read GetVisible write SetVisible;
	end;
//----------------------------------------------------------------------------
	TMenuBar = class (TUIControl)
	//public
	//	procedure Refresh;
	end;
//----------------------------------------------------------------------------
	TCommandButton = class (TUIControl)
	end;
//----------------------------------------------------------------------------
	TToolButton = class (TCommandButton)
	public
		procedure SetDownState(value: Boolean);
		procedure SetDeactivate (const value: TToolDeactivate);

		procedure SetOnMouseMoveHandler (handler: TUIMouseEvent); overload;
		procedure SetOnMouseMoveHandler (handlerData: Pointer; handler: TMouseHandler); overload;

		procedure SetOnMouseDownHandler (handler: TUIMouseEvent); overload;
		procedure SetOnMouseDownHandler (handlerData: Pointer; handler: TMouseHandler); overload;

		procedure SetOnMouseUpHandler (handler: TUIMouseEvent); overload;
		procedure SetOnMouseUpHandler (handlerData: Pointer; handler: TMouseHandler); overload;

		procedure SetDblOnClickHandler (handler: TNotifyEvent); overload;
		procedure SetDblOnClickHandler (handlerData: Pointer; handler: TNotifyHandler); overload;

	protected
		constructor Create (uiContract: TUIContract; id: integer);
	protected
		_onMouseMove:	TUIMouseEvent;
		_onMouseMoveHandler: TMouseHandler;
		_onMouseMoveHandlerData: Pointer;

		_onMouseDown:	TUIMouseEvent;
		_onMouseDownHandler: TMouseHandler;
		_onMouseDownHandlerData: Pointer;

		_onMouseUp:	TUIMouseEvent;
		_onMouseUpHandler: TMouseHandler;
		_onMouseUpHandlerData: Pointer;

		_onDblClick:	TNotifyEvent;
		_onDblClickHandler: TNotifyHandler;
		_onDblClickHandlerData: Pointer;
	end;
//------------------------------------------------------------------------------
implementation
//------------------------------------------------------------------------------
function onEvent (privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): integer; stdcall;
var
	uiContract:		TUIContract;
	control:		TUIControl;
	id:				integer;
	button:			integer;
	shift:			integer;
	x, y:			integer;
	fx, fy:			Double;
	sh:				TShiftState;
begin
	try
		result := Integer (rcOk);
		uiContract := TUIContract (privateData);

		case TUICommand (command) of
			uiControlOnClick:
			begin
				id := Contract.GetInt32 (inout);
				control := uiContract.FindControl (id);
				if control = nil then exit;
				if Assigned (control._onClick) then
					control._onClick (control);
				if Assigned (control._onClickHandler) then
					control._onClickHandler (control._onClickHandlerData, control);
			end;
			uiControlOnMouseMove:
			begin
				id := Contract.GetInt32 (inout);
				button := Contract.GetInt32 (inout);
				shift := Contract.GetInt32 (inout);
				x := Contract.GetInt32 (inout);
				y := Contract.GetInt32 (inout);
				fx := Contract.GetDouble(inout);
				fy := Contract.GetDouble(inout);

				control := uiContract.FindControl (id);
				if control = nil then exit;

				if (shift and 1) <> 0 then sh := sh + [ssShift];
				if (shift and 2) <> 0 then sh := sh + [ssCtrl];
				if (shift and 4) <> 0 then sh := sh + [ssAlt];

				if Assigned (TToolButton(control)._onMouseMove) then
					TToolButton(control)._onMouseMove (control, TMouseButton (button), sh, x, y, fx, fy);
				if Assigned (TToolButton(control)._onMouseMoveHandler) then
					TToolButton(control)._onMouseMoveHandler (TToolButton(control)._onMouseMoveHandlerData, control, TMouseButton (button), sh, x, y, fx, fy);
			end;
			uiControlOnMouseDown:
			begin
				id := Contract.GetInt32 (inout);
				button := Contract.GetInt32 (inout);
				shift := Contract.GetInt32 (inout);
				x := Contract.GetInt32 (inout);
				y := Contract.GetInt32 (inout);
				fx := Contract.GetDouble(inout);
				fy := Contract.GetDouble(inout);

				control := uiContract.FindControl (id);
				if control = nil then exit;

				if (shift and 1) <> 0 then sh := sh + [ssShift];
				if (shift and 2) <> 0 then sh := sh + [ssCtrl];
				if (shift and 4) <> 0 then sh := sh + [ssAlt];

				if Assigned (TToolButton(control)._onMouseDown) then
					TToolButton(control)._onMouseDown (control, TMouseButton (button), sh, x, y, fx, fy);
				if Assigned (TToolButton(control)._onMouseDownHandler) then
					TToolButton(control)._onMouseDownHandler (TToolButton(control)._onMouseDownHandlerData, control, TMouseButton (button), sh, x, y, fx, fy);
			end;
			uiControlOnMouseUp:
			begin
				id := Contract.GetInt32 (inout);
				button := Contract.GetInt32 (inout);
				shift := Contract.GetInt32 (inout);
				x := Contract.GetInt32 (inout);
				y := Contract.GetInt32 (inout);
				fx := Contract.GetDouble(inout);
				fy := Contract.GetDouble(inout);

				control := uiContract.FindControl (id);
				if control = nil then exit;

				if (shift and 1) <> 0 then sh := sh + [ssShift];
				if (shift and 2) <> 0 then sh := sh + [ssCtrl];
				if (shift and 4) <> 0 then sh := sh + [ssAlt];

				if Assigned (TToolButton(control)._onMouseUp) then
					TToolButton(control)._onMouseUp (control, TMouseButton (button), sh, x, y, fx, fy);
				if Assigned (TToolButton(control)._onMouseUpHandler) then
					TToolButton(control)._onMouseUpHandler (TToolButton(control)._onMouseUpHandlerData, control, TMouseButton (button), sh, x, y, fx, fy);
			end;
			uiControlOnDblClick:
			begin
				id := Contract.GetInt32 (inout);
				control := uiContract.FindControl (id);
				if control = nil then exit;
				if Assigned (TToolButton(control)._onDblClick) then
					TToolButton(control)._onDblClick (control);
				if Assigned (TToolButton(control)._onDblClickHandler) then
					TToolButton(control)._onDblClickHandler (TToolButton(control)._onDblClickHandlerData, control);
			end;
		end;
	except
		result := Integer (rcException);
	end;
end;

//------------------------------------------------------------------------------
function ControlsCompareFunction (Item1, Item2: Pointer): integer;
var
	control1, control2: TUIControl;
begin
	control1 := Item1;
	control2 := Item2;

	if control1.id < control2.id then
	begin
		result := -1;
		exit;
	end;

	if control1.id > control2.id then
	begin
		result := 1;
		exit;
	end;

	result := 0;
end;
//------------------------------------------------------------------------------
procedure TUIContract.AddControl (control: TUIControl);
begin
	FControls.Add (control);
	FControls.Sort (ControlsCompareFunction);
end;
//------------------------------------------------------------------------------
procedure TUIContract.RemoveControl (control: TUIControl);
begin
	FControls.Remove (control)
end;
//------------------------------------------------------------------------------
function TUIContract.FindControl (id: integer): TUIControl;
var
	index, beginIndex,
	endIndex:			integer;
	control:			TUIControl;
begin
	beginIndex := 0;
	endIndex := FControls.Count;
	result := nil;

	while true do
	begin
		if beginIndex = endIndex then exit;
		index := (endIndex + beginIndex) shr 1;
		control := FControls.Items [index];

		if control.id = id then
		begin
			result := control;
			exit;
		end;

		if id < control.id then
			endIndex := index
		else
			beginIndex := index;
	end;
end;
//------------------------------------------------------------------------------
constructor TUIContract.Create;
begin
	Inherited;
	FHandle := 0;
	FControls := TList.Create;
	FHandle := Contract.GetInstance ('UIService');
	Contract.setHandler (FHandle, integer (self), onEvent);
end;
//------------------------------------------------------------------------------
destructor TUIContract.Destroy;
var
	i:			integer;
	control:	TUIControl;
begin
	for i := FControls.Count-1 downto 0 do
	begin
		control := TUIControl(FControls.Items[i]);
		control.Free;
	end;
	FControls.Free;

	Contract.freeInstance (FHandle);
	Inherited;
end;

//------------------------------------------------------------------------------
function TUIContract.IsValid: boolean;
begin
	result := FHandle <> 0;
end;

//------------------------------------------------------------------------------
procedure TUIContract.GetEnvInfo(var ProgName: String; var info0, info1, info2, info3: Integer);
begin
	Contract.BeginMessage (FHandle, Integer (uiGetEnvInfo));
	Contract.EndMessage (FHandle);

	ProgName := Contract.GetWideString(FHandle);
	info0 := Contract.GetInt32 (FHandle);
	info1 := Contract.GetInt32 (FHandle);
	info2 := Contract.GetInt32 (FHandle);
	info3 := Contract.GetInt32 (FHandle);
end;

function TUIContract.GetRelatedFileName: WideString;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetRelatedFileName));
	Contract.EndMessage (FHandle);
	result := Contract.GetWideString(FHandle);
end;

//------------------------------------------------------------------------------
procedure TUIContract.DisplayMessage (pos: integer; const msg: WideString);
begin
	Contract.BeginMessage (FHandle, Integer (uiDisplayMessage));
	Contract.PutInt32 (FHandle, pos);
	Contract.PutWideString (FHandle, msg);
	Contract.EndMessage (FHandle);
end;
//------------------------------------------------------------------------------
function TUIContract.GetViewProjection: TSpatialReference;
var
  notNull: Boolean;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetViewProjection));
	Contract.EndMessage (FHandle);

  notNull := Contract.GetBool (FHandle);

  result := nil;

  if notNull then
  begin
    result := TSpatialReference.Create;
	  result.Unpack(FHandle);
  end;
end;

function TUIContract.GetDocumentLayerList (): TStringArray;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetDocumentLayerList));
	Contract.EndMessage (FHandle);
	result := Contract.GetWideStringArray (FHandle);
end;

procedure TUIContract.SetViewProjection(SpatialReference: TSpatialReference);
begin
	Contract.BeginMessage (FHandle, Integer (uiSetViewProjection));
	SpatialReference.Pack(FHandle);
	Contract.EndMessage (FHandle);
end;

procedure TUIContract.GetExtent (var xmin, ymin, xmax, ymax: double);
begin
	Contract.BeginMessage (FHandle, Integer (uiGetExtent));
	Contract.EndMessage (FHandle);
	xmin := Contract.GetDouble (FHandle);
	ymin := Contract.GetDouble (FHandle);
	xmax := Contract.GetDouble (FHandle);
	ymax := Contract.GetDouble (FHandle);
end;

procedure TUIContract.SetExtent (xmin, ymin, xmax, ymax: double);
begin
	Contract.BeginMessage (FHandle, Integer (uiSetExtent));
	Contract.PutDouble(FHandle, xmin);
	Contract.PutDouble(FHandle, ymin);
	Contract.PutDouble(FHandle, xmax);
	Contract.PutDouble(FHandle, ymax);
	Contract.EndMessage (FHandle);
end;

//------------------------------------------------------------------------------
function TUIContract.DrawPoint (const point: TPoint; const symbol: TPointSymbol): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (uiDrawPoint));
	point.Pack (FHandle);
	symbol.Pack (FHandle);
	Contract.EndMessage (FHandle);
	result := Contract.GetInt32 (FHandle);
end;

function TUIContract.DrawPoint (const point: TPoint; const Color: Integer): Integer;
var
	symbol:	TPointSymbol;
begin
	symbol := TPointSymbol.Create(Color, 8);
	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPoint));
		point.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free;
	end;
end;

function TUIContract.DrawPoint(const point: TPoint; const Color: Integer; const Style: TPointStyle): Integer;
var
	symbol:	TPointSymbol;
begin
	symbol := TPointSymbol.Create(Style, Color, 8);
	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPoint));
		point.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free;
	end;
end;

function TUIContract.DrawPointWithText (const point: TPoint; const symbol: TPointSymbol; const text: WideString): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (uiDrawPointWithText));
	point.Pack (FHandle);
	symbol.Pack (FHandle);
	Contract.PutWideString (FHandle, text);
	Contract.EndMessage (FHandle);
	result := Contract.GetInt32 (FHandle);
end;

function TUIContract.DrawPointWithText (const point: TPoint; const Color: Integer; const text: WideString): Integer;
var
	symbol:	TPointSymbol;
begin
	symbol := TPointSymbol.Create(Color, 8);
	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPointWithText));
		point.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.PutWideString (FHandle, text);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free;
	end;

end;

//------------------------------------------------------------------------------
function TUIContract.DrawPolyline (const polyline: TPolyline; const symbol: TLineSymbol): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (uiDrawPolyline));
	polyline.Pack (FHandle);
	symbol.Pack (FHandle);
	Contract.EndMessage (FHandle);
	result := Contract.GetInt32 (FHandle);
end;

function TUIContract.DrawPolyline (const polyline: TPolyline; const Color, Width: Integer): Integer;
var
	symbol: TLineSymbol;
begin
	symbol := TLineSymbol.Create(slsSolid, Color, Width);

	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPolyline));
		polyline.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free;
	end;
end;

//------------------------------------------------------------------------------
function TUIContract.DrawPolygon (const polygon: TPolygon; const symbol: TFillSymbol): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (uiDrawPolygon));
	polygon.Pack (FHandle);
	symbol.Pack (FHandle);
	Contract.EndMessage (FHandle);
	result := Contract.GetInt32 (FHandle);
end;

function TUIContract.DrawPolygon (const polygon: TPolygon; const Color: Integer; const Style: TFillStyle): Integer;
var
	symbol: TFillSymbol;
begin
	symbol := TFillSymbol.Create;
	symbol.Color := Color;
	symbol.Style := Style;
	symbol.Outline.Color := Color;
	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPolygon));
		polygon.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free
	end;
end;

function TUIContract.DrawRing (const ring: TRing; const symbol: TFillSymbol): Integer;
var
	polygon: TPolygon;
begin
	polygon := TPolygon.Create;
	polygon.AddRing(ring);
	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPolygon));
		polygon.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		polygon.Free;
	end;
end;

function TUIContract.DrawRing (const ring: TRing; const Color: Integer; const Style: TFillStyle): Integer;
var
	symbol: TFillSymbol;
	polygon: TPolygon;
begin
	polygon := TPolygon.Create;
	polygon.AddRing(ring);

	symbol := TFillSymbol.Create;
	symbol.Color := Color;
	symbol.Style := Style;
	symbol.Outline.Color := Color;
	result := rcException;

	try
		Contract.BeginMessage (FHandle, Integer (uiDrawPolygon));
		polygon.Pack (FHandle);
		symbol.Pack (FHandle);
		Contract.EndMessage (FHandle);
		result := Contract.GetInt32 (FHandle);
	finally
		symbol.Free;
		polygon.Free;
	end;
end;

//------------------------------------------------------------------------------
procedure TUIContract.SetVisible (graphicHandle: integer; isVisible: boolean);
begin
	Contract.BeginMessage (FHandle, Integer (uiSetVisible));
	Contract.PutInt32 (FHandle, graphicHandle);
	Contract.EndMessage (FHandle);
end;
//------------------------------------------------------------------------------
function TUIContract.DeleteGraphic (const Handle: Integer): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (uiDeleteGraphic));
	Contract.PutInt32 (FHandle, Handle);
	Contract.EndMessage (FHandle);
//	result := Registry_EndMessage (handle);
	result := 0;
end;

//------------------------------------------------------------------------------

procedure TUIContract.ShowAnimation (const show: Boolean);
begin
	Contract.BeginMessage (FHandle, Integer (uiShowAnimation));
	Contract.PutBool (FHandle, show);
	Contract.EndMessage (FHandle);
end;

//------------------------------------------------------------------------------

procedure TUIContract.HideAnimation;
begin
    ShowAnimation (False);
end;

//------------------------------------------------------------------------------

procedure TUIContract.ShowAnimation;
begin
    ShowAnimation (True);
end;

//------------------------------------------------------------------------------

function TUIContract.SafeDeleteGraphic (var Handle: Integer): Integer;
begin
	if Handle<>-1 then
	begin
		try
			Contract.BeginMessage (FHandle, Integer (uiDeleteGraphic));
			Contract.PutInt32 (FHandle, Handle);
			Contract.EndMessage (FHandle);
		except
		end;
	end;
	Handle := -1;
	result := 0;
end;

procedure TUIContract.GetVisiblePoints(var Points, Handlers: TList);
var
	I, N:	Integer;
	Point:	TPoint;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePoints));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Points := TList.Create;
	Handlers := TList.Create;

	for I := 0 to N - 1 do
	begin
		Point := TPoint.Create;
		Point.Unpack(FHandle);
		Points.Add(Point);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

procedure TUIContract.GetVisiblePolylines(var Polylines, Handlers: TList);
var
	I, N:		Integer;
	Polyline:	TPolyline;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePolylines));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Polylines := TList.Create;
	Handlers := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polyline := TPolyline.Create;
		Polyline.Unpack(FHandle);
		Polylines.Add(Polyline);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

procedure TUIContract.GetVisiblePolygons(var Polygons, Handlers: TList);
var
	I, N:		Integer;
	Polygon:	TPolygon;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePolygons));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Polygons := TList.Create;
	Handlers := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polygon := TPolygon.Create;
		Polygon.Unpack(FHandle);
		Polygons.Add(Polygon);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

function TUIContract.GetVisiblePoints: TList;
var
	I, N:	Integer;
	Point:	TPoint;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePoints));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Point := TPoint.Create;
		Point.Unpack(FHandle);
		result.Add(Point)
	end;
end;

function TUIContract.GetVisiblePolylines: TList;
var
	I, N:		Integer;
	Polyline:	TPolyline;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePolylines));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polyline := TPolyline.Create;
		Polyline.Unpack(FHandle);
		result.Add(Polyline)
	end;
end;

function TUIContract.GetVisiblePolygons: TList;
var
	I, N:		Integer;
	Polygon:	TPolygon;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetVisiblePolygons));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polygon := TPolygon.Create;
		Polygon.Unpack(FHandle);
		result.Add(Polygon)
	end;
end;

procedure TUIContract.GetSelectedPoints(var Points, Handlers: TList);
var
	I, N:	Integer;
	Point:	TPoint;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPoints));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Points := TList.Create;
	Handlers := TList.Create;
	for I := 0 to N - 1 do
	begin
		Point := TPoint.Create;
		Point.Unpack(FHandle);
		Points.Add(Point);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

procedure TUIContract.GetSelectedPolylines(var Polylines, Handlers: TList);
var
	I, N:		Integer;
	Polyline:	TPolyline;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPolylines));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Polylines := TList.Create;
	Handlers := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polyline := TPolyline.Create;
		Polyline.Unpack(FHandle);
		Polylines.Add(Polyline);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

procedure TUIContract.GetSelectedPolygons(var Polygons, Handlers: TList);
var
	I, N:		Integer;
	Polygon:	TPolygon;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPolygons));
	Contract.PutBool(FHandle, True);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	Polygons := TList.Create;
	Handlers := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polygon := TPolygon.Create;
		Polygon.Unpack(FHandle);
		Polygons.Add(Polygon);
		Handlers.Add(Pointer(Contract.GetInt32(FHandle)));
	end;
end;

function TUIContract.GetSelectedPoints: TList;
var
	I, N:	Integer;
	Point:	TPoint;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPoints));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Point := TPoint.Create;
		Point.Unpack(FHandle);
		result.Add(Point)
	end;
end;

function TUIContract.GetSelectedPolylines: TList;
var
	I, N:		Integer;
	Polyline:	TPolyline;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPolylines));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polyline := TPolyline.Create;
		Polyline.Unpack(FHandle);
		result.Add(Polyline)
	end;
end;

function TUIContract.GetSelectedPolygons: TList;
var
	I, N:		Integer;
	Polygon:	TPolygon;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetSelectedPolygons));
	Contract.PutBool(FHandle, False);
	Contract.EndMessage (FHandle);

	N := Contract.GetInt32(FHandle);
	result := TList.Create;
	for I := 0 to N - 1 do
	begin
		Polygon := TPolygon.Create;
		Polygon.Unpack(FHandle);
		result.Add(Polygon)
	end;
end;

//------------------------------------------------------------------------------

function TUIContract.CreateControl (const name: WideString; command: TUICommand): Integer;
begin
	Contract.BeginMessage (FHandle, Integer (command));
	Contract.PutWideString (FHandle, name);
	Contract.EndMessage (FHandle);
	result := Contract.GetInt32 (FHandle);
end;
//------------------------------------------------------------------------------
function TUIContract.CreateToolBar (const name: WideString): TToolBar;
var
	id: integer;
begin
	id := CreateControl (name, uiCreateToolBar);
	result := TToolBar.Create (self, id);
end;
//------------------------------------------------------------------------------
function TUIContract.CreateMenuBar (const name: WideString): TMenuBar;
var
	id: integer;
begin
	id := CreateControl (name, uiCreateMenuBar);
	result := TMenuBar.Create (self, id);
end;
//------------------------------------------------------------------------------
function TUIContract.CreateToolButton (const name: WideString): TToolButton;
var
	id: integer;
begin
	id := CreateControl (name, uiCreateToolButton);
	result := TToolButton.Create(self, id);
end;
//------------------------------------------------------------------------------
function TUIContract.CreateCommandButton (const name: WideString): TCommandButton;
var
	id: integer;
begin
	id := CreateControl (name, uiCreateCommandButton);
	result := TCommandButton.Create (self, id);
end;

//------------------------------------------------------------------------------

procedure TUIContract.CreateNewToolItem (name: WideString);
begin
  Contract.BeginMessage (FHandle, Integer (uiCreateNewToolItem));
	Contract.PutWideString (FHandle, name);
	Contract.EndMessage (FHandle);
end;

//------------------------------------------------------------------------------

constructor TUIControl.Create (uiContract: TUIContract; id: integer);
begin
	Inherited Create;
	_uiContract := uiContract;
	self.id := id;
	_onClick := nil;
	_onClickHandler := nil;
	_onClickHandlerData := nil;
	uiContract.AddControl(self);
end;
//------------------------------------------------------------------------------
destructor TUIControl.Destroy;
var
	handle: TRegistryHandle;
begin
	_uiContract.RemoveControl (self);
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiDeleteControl));
	Contract.PutInt32 (handle, id);
	Contract.EndMessage (handle);
	Inherited;
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetOnClickHandler (handler: TNotifyEvent);
var
	handle: TRegistryHandle;
begin
	_onClick := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnClickHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetOnClickHandler (handlerData: Pointer; handler: TNotifyHandler);
var
	handle: TRegistryHandle;
begin
	_onClickHandlerData := handlerData;
	_onClickHandler := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnClickHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetCaption (const value: WideString);
var
	handle:		TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetCaption));
	Contract.PutInt32 (handle, id);
	Contract.PutWideString (handle, value);
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetBitmap (const value: TUIBitmap);
var
	handle:	TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetBitmap));
	Contract.PutInt32 (handle, id);
	value.Pack (handle);
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetTooltip (const value: WideString);
var
	handle:	TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetTooltip));
	Contract.PutInt32 (handle, id);
	Contract.PutWideString (handle, value);
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetCursor (const value: TUICursor);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetCursor));
	Contract.PutInt32 (handle, id);
	value.Pack (handle);
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetStyle (style: TControlStyle);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetStyle));
	Contract.PutInt32 (handle, id);
	Contract.PutInt32 (handle, Integer (style));
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TUIControl.SetCategory (const value: WideString);
var
	handle:		TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetCategory));
	Contract.PutInt32 (handle, id);
	Contract.PutWideString (handle, value);
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetChecked (const value: Boolean);
var
	handle:		TRegistryHandle;
begin

	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetChecked));

	Contract.PutInt32 (handle, id);
	Contract.PutBool(handle, value);
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetEnabled (const value: Boolean);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetEnabled));
	Contract.PutInt32 (handle, id);
	Contract.PutBool(handle, value);
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetMessage (const value: WideString);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetMessage));
	Contract.PutInt32 (handle, id);
	Contract.PutWideString (handle, value);
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetHelpID (const value: Integer);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetHelpID));
	Contract.PutInt32 (handle, id);
	Contract.PutInt32 (handle, value);
	Contract.EndMessage (handle);
end;

procedure TUIControl.SetHelpFile (const value: WideString);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetHelpFile));
	Contract.PutInt32 (handle, id);
	Contract.PutWideString (handle, value);
	Contract.EndMessage (handle);
end;

//------------------------------------------------------------------------------
constructor TToolButton.Create (uiContract: TUIContract; id: integer);
begin
	Inherited;
	_onMouseMove := nil;
	_onMouseMoveHandler := nil;
	_onMouseMoveHandlerData := nil;

	_onMouseDown := nil;
	_onMouseDownHandler := nil;
	_onMouseDownHandlerData := nil;

	_onMouseUp := nil;
	_onMouseUpHandler := nil;
	_onMouseUpHandlerData := nil;

	_onDblClick := nil;
	_onDblClickHandler := nil;
	_onDblClickHandlerData := nil;
end;

procedure TToolButton.SetDeactivate (const value: TToolDeactivate);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolButtonSetDeactivate));
	Contract.PutInt32 (handle, id);
	Contract.PutInt32 (handle, Integer (value));
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetDownState(value: Boolean);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolButtonSetDown));
	Contract.PutInt32 (handle, id);
	Contract.PutBool(handle, value);
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetOnMouseMoveHandler (handler: TUIMouseEvent);
var
	handle: TRegistryHandle;
begin
	_onMouseMove := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseMoveHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetOnMouseMoveHandler (handlerData: Pointer; handler: TMouseHandler);
var
	handle: TRegistryHandle;
begin
	_onMouseMoveHandlerData := handlerData;
	_onMouseMoveHandler := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseMoveHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TToolButton.SetOnMouseDownHandler (handler: TUIMouseEvent);
var
	handle: TRegistryHandle;
begin
	_onMouseMove := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseDownHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetOnMouseDownHandler (handlerData: Pointer; handler: TMouseHandler);
var
	handle: TRegistryHandle;
begin
	_onMouseMoveHandlerData := handlerData;
	_onMouseMoveHandler := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseDownHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------

procedure TToolButton.SetOnMouseUpHandler (handler: TUIMouseEvent);
var
	handle: TRegistryHandle;
begin
	_onMouseMove := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseUpHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetOnMouseUpHandler (handlerData: Pointer; handler: TMouseHandler);
var
	handle: TRegistryHandle;
begin
	_onMouseMoveHandlerData := handlerData;
	_onMouseMoveHandler := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnMouseUpHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

//------------------------------------------------------------------------------

procedure TToolButton.SetDblOnClickHandler (handler: TNotifyEvent);
var
	handle: TRegistryHandle;
begin
	_onClick := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnDblClickHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

procedure TToolButton.SetDblOnClickHandler (handlerData: Pointer; handler: TNotifyHandler);
var
	handle: TRegistryHandle;
begin
	_onClickHandlerData := handlerData;
	_onClickHandler := handler;
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiControlSetOnDblClickHandler));
	Contract.PutInt32 (handle, id);
	Contract.PutBool (handle, Assigned (handler));
	Contract.EndMessage (handle);
end;

//------------------------------------------------------------------------------

function TToolBar.GetVisible: Boolean;
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolBarGetVisible));
	Contract.PutInt32 (handle, id);
	Contract.EndMessage (handle);
	result := Contract.GetBool(handle);
end;

procedure TToolBar.SetVisible(Value: Boolean);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolBarSetVisible));
	Contract.PutInt32 (handle, id);
	Contract.PutBool(handle, Value);
	Contract.EndMessage (handle);
end;

procedure TToolBar.AddToolButton (toolButton: TToolButton);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolBarAddToolButton));
	Contract.PutInt32 (handle, id);
	Contract.PutInt32 (handle, toolButton.id);
	Contract.EndMessage (handle);
end;
//------------------------------------------------------------------------------
procedure TToolBar.AddCommandButton (commandButton: TCommandButton);
var
	handle: TRegistryHandle;
begin
	handle := _uiContract.FHandle;
	Contract.BeginMessage (handle, Integer (uiToolBarAddCommandButton));
	Contract.PutInt32 (handle, id);
	Contract.PutInt32 (handle, commandButton.id);
	Contract.EndMessage (handle);
end;
//---------------- TSymbol -----------------------------------------------------
constructor TSymbol.Create;
begin
	Inherited;
	FColor := RGB(Random(256), Random(256), Random(256));
	FSize := 1;
end;
//------------------------------------------------------------------------------
constructor TSymbol.Create (InitColor, InitSize: integer);
begin
	Inherited Create;
	FColor := InitColor;
	FSize := InitSize;
end;
//------------------------------------------------------------------------------
procedure TSymbol.Assign (const src: TSymbol);
begin
	FColor := src.FColor;
	FSize := src.FSize;
end;
//------------------------------------------------------------------------------
procedure TSymbol.Pack (handle: TRegistryHandle);
begin
	Contract.PutInt32 (handle, FColor);
	Contract.PutInt32 (handle, FSize);
end;
//------------------------------------------------------------------------------
procedure TSymbol.Unpack (handle: TRegistryHandle);
begin
	FColor := Contract.GetInt32 (handle);
	FSize := Contract.GetInt32 (handle);
end;
//------------------------------------------------------------------------------
function TSymbol.Clone: TSymbol;
begin
	result := TSymbol.Create (FColor, FSize);
end;
//------------------------------------------------------------------------------
constructor TPointSymbol.Create;
begin
	Inherited;
	FStyle := smsSquare;
	FSize := 5;
end;
//------------------------------------------------------------------------------
constructor TPointSymbol.Create (InitStyle: TPointStyle; InitColor, InitSize: integer);
begin
	Inherited Create (InitColor, InitSize);
	FStyle := InitStyle;
end;
//------------------------------------------------------------------------------
procedure TPointSymbol.Assign (const src: TSymbol);
begin
	inherited;
	FStyle := TPointSymbol (src).FStyle;
end;
//------------------------------------------------------------------------------
procedure TPointSymbol.Pack (handle: TRegistryHandle);
begin
	inherited;
	Contract.PutInt32 (handle, Integer (FStyle));
end;
//------------------------------------------------------------------------------
procedure TPointSymbol.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FStyle := TPointStyle (Contract.GetInt32 (handle));
end;
//------------------------------------------------------------------------------
function TPointSymbol.Clone: TSymbol;
begin
	result := TPointSymbol.Create (FStyle, FColor, FSize);
end;
//----------- TLineSymbol ------------------------------------------------------
constructor TLineSymbol.Create;
begin
	Inherited;
	FStyle := slsSolid;
end;
//------------------------------------------------------------------------------
constructor TLineSymbol.Create (InitStyle: TLineStyle; InitColor, InitSize: integer);
begin
	Inherited Create (InitColor, InitSize);
	FStyle := InitStyle;
end;
//------------------------------------------------------------------------------

procedure TLineSymbol.Assign (const src: TSymbol);
begin
	inherited;
	FStyle := TLineSymbol (src).FStyle;
end;
//------------------------------------------------------------------------------
procedure TLineSymbol.Pack (handle: TRegistryHandle);
begin
	inherited;
	Contract.PutInt32 (handle, Integer (FStyle));
end;
//------------------------------------------------------------------------------
procedure TLineSymbol.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FStyle := TLineStyle (Contract.GetInt32 (handle));
end;
//------------------------------------------------------------------------------
function TLineSymbol.Clone: TSymbol;
begin
	result := TLineSymbol.Create (FStyle, FColor, FSize);
end;
//-------- TFillSymbol ---------------------------------------------------------
constructor TFillSymbol.Create;
begin
	Inherited;
	FStyle := sfsSolid;
	FOutline := TLineSymbol.Create;
end;
//------------------------------------------------------------------------------
destructor TFillSymbol.Destroy;
begin
	Inherited;
	FOutline.Free;
end;
//------------------------------------------------------------------------------
procedure TFillSymbol.SetOutline(Val: TLineSymbol);
begin
	FOutline.Assign (Val);
end;
//------------------------------------------------------------------------------
procedure TFillSymbol.Assign (const src: TSymbol);
begin
	inherited;
	FStyle := TFillSymbol (src).FStyle;
	FOutline.Assign (TFillSymbol (src).FOutline);
end;
//------------------------------------------------------------------------------
procedure TFillSymbol.Pack (handle: TRegistryHandle);
begin
	inherited;
	Contract.PutInt32 (handle, Integer (FStyle));
	FOutline.Pack (handle);
end;
//------------------------------------------------------------------------------
procedure TFillSymbol.Unpack (handle: TRegistryHandle);
begin
	inherited;
	FStyle := TFillStyle (Contract.GetInt32 (handle));
	FOutline.Unpack (handle);
end;
//------------------------------------------------------------------------------
function TFillSymbol.Clone: TSymbol;
begin
	result := TFillSymbol.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
constructor TUIBitmap.Create;
begin
	Inherited;
	FWidth := 0;
	FHeight := 0;
end;

constructor TUIBitmap.Create (const from: Graphics.TBitmap);
var
	pd, ps:		PInteger;
	x, y:		Integer;
begin
	Inherited Create;
	SetSize (from.Width, from.Height);

	pd := GetBits;
	from.PixelFormat := pf32bit;

	for y := 0 to from.Height-1 do
	begin
		ps := from.ScanLine[y];
		for x := 0 to from.Width-1 do
		begin
			pd^ := ps^;
			Inc (pd);
			Inc (ps);
		end;
	end;
end;

destructor TUIBitmap.Destroy;
begin
	SetSize(0, 0);
	FBits := nil;
	Inherited;
end;

//------------------------------------------------------------------------------
procedure TUIBitmap.SetSize(NewWidth, NewHeight: Integer);
var
	size:		integer;
begin
	FWidth := NewWidth;
	FHeight := NewHeight;
	size := FWidth * FHeight;

//	if size >= 0 then
		SetLength (FBits, size);
end;
//------------------------------------------------------------------------------
procedure TUIBitmap.Clear;
begin
	SetSize(0, 0)
end;
//------------------------------------------------------------------------------
function TUIBitmap.GetBits: PInteger;
begin
	result := @FBits [0];
end;
//------------------------------------------------------------------------------
procedure TUIBitmap.Assign (const src: TUIBitmap);
var
	i: integer;
begin
	SetSize (src.FWidth, src.FHeight);
	for i := 0 to FWidth * FHeight - 1 do
		FBits[i] := src.FBits[i];
end;
//------------------------------------------------------------------------------
function TUIBitmap.Clone: TUIBitmap;
begin
	result := TUIBitmap.Create;
	result.Assign (self);
end;
//------------------------------------------------------------------------------
procedure TUIBitmap.Pack (handle: TRegistryHandle);
var
	i:	Integer;
begin
	Contract.PutInt32 (handle, FWidth);
	Contract.PutInt32 (handle, FHeight);
	for i := 0 to FWidth * FHeight - 1 do
		Contract.PutInt32 (handle, FBits [i]);
end;
//------------------------------------------------------------------------------
procedure TUIBitmap.Unpack (handle: TRegistryHandle);
var
	w:	integer;
	h:	integer;
	i:	integer;
begin
	w := Contract.GetInt32 (handle);
	h := Contract.GetInt32 (handle);
	SetSize (w, h);

	for i := 0 to w*h-1 do
		FBits [i] := Contract.GetInt32 (handle);
end;
//------------------------------------------------------------------------------
constructor TUICursor.Create;
begin
	Inherited;
	FX := 0;
	FY := 0;
	FWidth := 0;
	FHeight := 0;
	FHaveXorMask := True;
end;

constructor TUICursor.Create(IsColored: Boolean);
begin
	Inherited Create;
	FX := 0;
	FY := 0;
	FWidth := 0;
	FHeight := 0;
	FHaveXorMask := IsColored;
end;

constructor TUICursor.Create(const from: THandle);
var
	pd, ps:			PInteger;
	cx, cy, x, y:	integer;
	cursorPicture:	Graphics.TBitmap;
	cursorInfo:		TIconInfo;
begin
	Inherited Create;
//	FX := 0;
//	FY := 0;
//	FWidth := 0;
//	FHeight := 0;

	GetIconInfo (from, cursorInfo);

	FHaveXorMask := CursorInfo.hbmColor <> 0;

	SetSpot (cursorInfo.xHotspot, cursorInfo.yHotspot);

	CursorPicture := Graphics.TBitmap.Create;

	cursorPicture.Handle :=  cursorInfo.hbmMask;
	cursorPicture.PixelFormat := pf32bit;
	cx := cursorPicture.Width;
	cy := cursorPicture.Height;
	SetSize (cx, cy);

	pd := AndMask;
	for y := 0 to cy-1 do
	begin
		ps := cursorPicture.ScanLine[y];
		for x := 0 to cx-1 do
		begin
			Pd^ := Ps^;
			Inc(Pd);
			Inc(Ps);
		end;
	end;

	if Colored then
	begin
		cursorPicture.Handle :=  cursorInfo.hbmColor;
		cursorPicture.PixelFormat := pf32bit;

		pd := XorMask;
		for y := 0 to cy-1 do
		begin
			Ps := CursorPicture.ScanLine[y];
			for x := 0 to cx-1 do
			begin
				Pd^ := Ps^;
				Inc(Pd);
				Inc(Ps);
			end;
		end;
	end;

	CursorPicture.Free;
end;

destructor TUICursor.Destroy;
begin
	SetSize (0, 0);
	FAndMask := nil;
	FXorMask := nil;
	Inherited;
end;

//------------------------------------------------------------------------------
procedure TUICursor.SetSpot (SpotX, SpotY: integer);
begin
	FX := SpotX;
	FY := SpotY;
end;
//------------------------------------------------------------------------------
procedure TUICursor.SetSize (NewWidth, NewHeight: Integer);
var
	size: integer;
begin
	FWidth := NewWidth;
	FHeight := NewHeight;
	size := FWidth * FHeight;

	if size >= 0 then
	begin
		SetLength (FAndMask, size);
		if FHaveXorMask then
			SetLength (FXorMask, size);
	end;
end;
//------------------------------------------------------------------------------
procedure TUICursor.Assign (const src: TUICursor);
var
	i: integer;
begin
	FHaveXorMask := src.FHaveXorMask;
	SetSize (src.FWidth, src.FHeight);
	FX := src.FX;
	FY := src.FY;

	for i := 0 to FWidth * FHeight - 1 do
	begin
		FAndMask [i] := src.FAndMask [i];
		if FHaveXorMask then
			FXorMask [i] := src.FXorMask [i];
	end;
end;
//------------------------------------------------------------------------------
procedure TUICursor.Clear;
begin
	FWidth := 0;
	FHeight := 0;
	FX := 0;
	FY := 0;
end;
//------------------------------------------------------------------------------
function TUICursor.GetAndMask: PInteger;
begin
	result := @FAndMask [0];
end;
//------------------------------------------------------------------------------
function TUICursor.GetXorMask: PInteger;
begin
	result := nil;
	if FHaveXorMask then
		result := @FXorMask [0];
end;
//------------------------------------------------------------------------------
procedure TUICursor.Pack (handle: TRegistryHandle);
var
	i:		integer;
	Size:	integer;
begin
	Contract.PutInt32 (handle, FWidth);
	Contract.PutInt32 (handle, FHeight);
	Contract.PutInt32 (handle, FX);
	Contract.PutInt32 (handle, FY);
	Contract.PutBool  (handle, FHaveXorMask);

	Size := FWidth * FHeight;

	for i := 0 to Size - 1 do
		Contract.PutInt32 (handle, FAndMask [i]);

	if FHaveXorMask then
		for i := 0 to Size - 1 do
			Contract.PutInt32 (handle, FXorMask [i]);
end;
//------------------------------------------------------------------------------
procedure TUICursor.Unpack (handle: TRegistryHandle);
var
	i:		integer;
	Size:	integer;
begin
	FWidth := Contract.GetInt32 (handle);
	FHeight := Contract.GetInt32 (handle);
	FX := Contract.GetInt32 (handle);
	FY := Contract.GetInt32 (handle);
	FHaveXorMask := Contract.GetBool(handle);

	SetSize (FWidth, FHeight);
	Size := FWidth * FHeight;

	for i := 0 to Size - 1 do
		FAndMask [i] := Contract.GetInt32 (handle);

	if FHaveXorMask then
		for i := 0 to Size - 1 do
			FXorMask [i] := Contract.GetInt32 (handle);
end;

//------------------------------------------------------------------------------

function TUICursor.Clone: TUICursor;
begin
	result := TUICursor.Create;
	result.Assign (self);
end;

function TUIContract.GetDocumentMap (): Cardinal;
begin
	Contract.BeginMessage (FHandle, Integer (uiGetDocumentMap));
	Contract.EndMessage (FHandle);
	result := Cardinal (Contract.GetInt32 (FHandle));
end;

end.
