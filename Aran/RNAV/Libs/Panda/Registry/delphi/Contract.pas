unit Contract;

interface

uses
	SysUtils;

//	Standart service commands
//	TRegistryCommand
const
	svcGetInstance		= $10000;
	svcFreeInstance		= $10001;
	svcAttachClass		= $10002;
	svcDetachClass		= $10003;

type
	ERegistryError = class(Exception);
	TRegistryHandle = integer;

	TStringArray = array of string;

// Standart registry results
{
	TRegistryResult =
	(
		rcClassNotFound				= -$10000,
		rcClassInUse,
		rcErrorLoadingDll,
		rcEntryPointNotFound,
		rcObjectNotFound,
		rcException,
		rcClassAlreadyRegistered,
		rcInvalid,
		rcInvalidParameter,
		rcNotImplemented,
		rcOK = 0
	);
}

const
	rcClassNotFound				= -$10000;
	rcClassInUse				= rcClassNotFound + 01;
	rcErrorLoadingDll			= rcClassNotFound + 02;
	rcEntryPointNotFound		= rcClassNotFound + 03;
	rcObjectNotFound			= rcClassNotFound + 04;
	rcException					= rcClassNotFound + 05;
	rcClassAlreadyRegistered	= rcClassNotFound + 06;
	rcInvalid					= rcClassNotFound + 07;
	rcInvalidParameter			= rcClassNotFound + 08;
	rcNotImplemented			= rcClassNotFound + 09;
	rcOK						= 0;

type
  // Entry point for services

	Method = function (privateData: TRegistryHandle; command: integer; inout: TRegistryHandle): integer; stdcall;

	function Registry_getInstance (const className: PChar; var handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_getInstance';
	function Registry_FreeInstance (handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_freeInstance';

	function Registry_SetHandler (handle: TRegistryHandle; handlerData: integer; method: Method): integer; stdcall; external 'Registry.dll' name 'Registry_setHandler';
	function Registry_UnsetHandler (handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_unsetHandler';

	function Registry_RegisterClass (const className: PChar; priority: integer; method: Method; hModule: Integer; var handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_registerClass';
	function Registry_UnregisterClass (const serviceName: PChar): integer; stdcall; external 'Registry.dll' name 'Registry_unregisterClass';
	function Registry_LoadService (const className: PChar; const path: PChar; const entryName: PChar; priority: integer; var handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_loadService';
	function Registry_IsInProcess (handle: TRegistryHandle; var result: integer): integer; stdcall; external 'Registry.dll' name 'Registry_isInProcess';

	function Registry_BeginMessage (handle: TRegistryHandle; command: integer): integer; stdcall; external 'Registry.dll' name 'Registry_beginMessage';
	function Registry_EndMessage (handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_endMessage';
	function Registry_BeginEvent (handle: TRegistryHandle; command: integer): integer; stdcall; external 'Registry.dll' name 'Registry_beginEvent';
	function Registry_EndEvent (handle: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_endEvent';

	function Registry_PutByte (handle: TRegistryHandle; value: integer): integer; stdcall; external 'Registry.dll' name 'Registry_putByte';
	function Registry_PutShort (handle: TRegistryHandle; value: integer): integer; stdcall; external 'Registry.dll' name 'Registry_putShort';
	function Registry_PutInt32 (handle: TRegistryHandle; value: integer): integer; stdcall; external 'Registry.dll' name 'Registry_putInt32';
	function Registry_PutInt64 (handle: TRegistryHandle; value: Int64): integer; stdcall; external 'Registry.dll' name 'Registry_putInt64';
	function Registry_PutFloat (handle: TRegistryHandle; value: single): integer; stdcall; external 'Registry.dll' name 'Registry_putFloat';
	function Registry_PutDouble (handle: TRegistryHandle; value: double): integer; stdcall; external 'Registry.dll' name 'Registry_putDouble';
	function Registry_PutString (handle: TRegistryHandle; const value: PInteger): integer; stdcall; external 'Registry.dll' name 'Registry_putString';
	function Registry_PutArray (handle: TRegistryHandle; const dataPtr: Pointer; size: LongWord): integer; stdcall; external 'Registry.dll' name 'Registry_putArray';
	function Registry_PutHandle (handle: TRegistryHandle; h: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_putHandle';
	function Registry_PutWideString (handle: TRegistryHandle; const str: WideString): integer;
	function Registry_PutBool (handle: TRegistryHandle; value: boolean): integer;

	function Registry_GetByte (handle: TRegistryHandle; var value: integer): integer; stdcall; external 'Registry.dll'  name 'Registry_getByte';
	function Registry_GetShort (handle: TRegistryHandle; var value: integer): integer; stdcall; external 'Registry.dll' name 'Registry_getShort';
	function Registry_GetInt32 (handle: TRegistryHandle; var value: integer): integer; stdcall; external 'Registry.dll' name 'Registry_getInt32';
	function Registry_GetInt64 (handle: TRegistryHandle; var value: Int64): integer; stdcall; external 'Registry.dll' name 'Registry_getInt64';
	function Registry_GetFloat (handle: TRegistryHandle; var value: single): integer; stdcall; external 'Registry.dll' name 'Registry_getFloat';
	function Registry_GetDouble (handle: TRegistryHandle; var value: double): integer; stdcall; external 'Registry.dll' name 'Registry_getDouble';
	function Registry_GetString (handle: TRegistryHandle; var str: PInteger): integer; stdcall; external 'Registry.dll' name 'Registry_getString';

	function Registry_GetArray (handle: TRegistryHandle; var dataPtr: Pointer; var size: LongWord): integer; stdcall; external 'Registry.dll' name 'Registry_getArray';

	function Registry_GetHandle (handle: TRegistryHandle; var h: TRegistryHandle): integer; stdcall; external 'Registry.dll' name 'Registry_getHandle';
	function Registry_GetWideString (handle: TRegistryHandle; var str: WideString): integer;
	function Registry_GetBool (handle: TRegistryHandle; var value: boolean): integer;

	function GetInstance (const className: PChar): TRegistryHandle;
	procedure FreeInstance (handle: TRegistryHandle);

	procedure SetHandler (handle: TRegistryHandle; handlerData: integer; method: Method);
	procedure UnsetHandler (handle: TRegistryHandle);

	function RegisterClass (const className: PChar; priority: integer; method: Method; hModule: Integer): TRegistryHandle;
	procedure UnregisterClass (const serviceName: PChar);
	function LoadService (const className: PChar; const path: PChar; const entryName: PChar; priority: integer): TRegistryHandle;
	function IsInProcess (handle: TRegistryHandle): boolean;

	procedure BeginMessage (handle: TRegistryHandle; command: integer);
	procedure EndMessage (handle: TRegistryHandle);
	procedure BeginEvent (handle: TRegistryHandle; command: integer);
	procedure EndEvent (handle: TRegistryHandle);

	procedure PutBool (handle: TRegistryHandle; value: boolean);
	procedure PutByte (handle: TRegistryHandle; value: integer);
	procedure PutShort (handle: TRegistryHandle; value: integer);
	procedure PutInt32 (handle: TRegistryHandle; value: integer);
	procedure PutInt64 (handle: TRegistryHandle; value: Int64);
	procedure PutFloat (handle: TRegistryHandle; value: single);
	procedure PutDouble (handle: TRegistryHandle; value: double);
	procedure PutString (handle: TRegistryHandle; const value: PInteger);
	procedure PutArray (handle: TRegistryHandle; const dataPtr: Pointer; size: LongWord);
	procedure PutHandle (handle: TRegistryHandle; h: TRegistryHandle);
	procedure PutWideString (handle: TRegistryHandle; const str: WideString);
	procedure PutWideStringArray (handle: TRegistryHandle; strArray: TStringArray);

	function GetBool (handle: TRegistryHandle): boolean;
	function GetByte (handle: TRegistryHandle): byte;
	function GetShort (handle: TRegistryHandle): word;
	function GetInt32 (handle: TRegistryHandle): integer;
	function GetInt64 (handle: TRegistryHandle): Int64;
	function GetFloat (handle: TRegistryHandle): single;
	function GetDouble (handle: TRegistryHandle): double;
	function GetString (handle: TRegistryHandle): PInteger;
	procedure GetArray (handle: TRegistryHandle; var dataPtr: Pointer; var size: LongWord);
	function GetWideStringArray (handle: TRegistryHandle): TStringArray;

	function GetHandle (handle: TRegistryHandle): TRegistryHandle;
	function GetWideString (handle: TRegistryHandle): WideString;
	procedure ThrowException(errorCode: integer);
//------------------------------------------------------------------------------
implementation
//uses	Geometry;		//included For debug purposes

//------------------------------------------------------------------------------
function Registry_GetWideString (handle: TRegistryHandle; var str: WideString): integer;
var
	inputString:	PInteger;
	len:			integer;
begin
	result := Registry_GetString (handle, inputString);
	if result <> rcOk then exit;

	len := inputString^;

	str := ''; 

	while len > 0 do
	begin
		inc (inputString);
		str := str + WideChar (inputString^);
		dec (len);
	end;

	result := rcOk;
end;
//------------------------------------------------------------------------------
function Registry_PutWideString (handle: TRegistryHandle; const str: WideString): integer;
var
	len:			integer;
	i:				integer;
	inputString:	array of integer;
begin
	len := length (str);
	setLength (inputString, len + 1);
	inputString [0] := len;

	for i := 1 to len do
	begin
		inputString [i] := ord (str [i]);
	end;

	result := Registry_putString (handle, @inputString [0]);
end;
//------------------------------------------------------------------------------
function Registry_PutBool (handle: TRegistryHandle; value: boolean): integer;
begin
	result := Registry_putByte (handle, Byte(value));
end;
//------------------------------------------------------------------------------
function Registry_GetBool (handle: TRegistryHandle; var value: boolean): integer;
var
	i:	integer;
begin
	result := Registry_GetByte (handle, i);
	if result <> rcOk then exit;

	value := (i <> 0);
end;
//------------------------------------------------------------------------------
procedure ThrowException(errorCode: integer);
	function ReturnAddr: Pointer;
	asm
		MOV     EAX, [EBP+4]
	end;

var
	msg:	string;
begin

	case errorCode of
	rcClassNotFound:
		msg := 'Class not found.';
	rcClassInUse:
		msg := 'Class in use.';
	rcErrorLoadingDll:
		msg := 'Error loading dll.';
	rcEntryPointNotFound:
		msg := 'Entry point not found.';
	rcClassAlreadyRegistered:
		msg := 'Class is already registered.';
	rcObjectNotFound:
		msg := 'Object not found.';
	rcException:
		msg := 'Exception occured during execution.';
	rcInvalid:
		msg := 'Invalid call.';
	rcInvalidParameter:
		msg := 'Invalid parameter.';
	rcNotImplemented:
		msg := 'Not implemented.';
	else
		msg := 'Unknown error.';
	end;

	raise ERegistryError.CreateFmt('Error (%x): '+ Msg, [errorCode]) at ReturnAddr;
end;
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function GetInstance (const className: PChar): TRegistryHandle;
var
	error:	integer;
begin
	error := Registry_GetInstance (className, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure FreeInstance (handle: TRegistryHandle);
var
	error:	integer;
begin
	error := Registry_FreeInstance (handle);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure SetHandler (handle: TRegistryHandle; handlerData: integer; method: Method);
var
	error:	integer;
begin
	error := Registry_SetHandler (handle, handlerData, method);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure UnsetHandler (handle: TRegistryHandle);
var
	error:	integer;
begin
	error := Registry_UnsetHandler (handle);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function RegisterClass (const className: PChar; priority: integer; method: Method; hModule: Integer): TRegistryHandle;
var
	error:	integer;
begin
	error := Registry_RegisterClass (className, priority, method, hModule, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure UnregisterClass (const serviceName: PChar);
var
	error:	integer;
begin
	error := Registry_UnregisterClass (serviceName);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function LoadService (const className: PChar; const path: PChar; const entryName: PChar; priority: integer): TRegistryHandle;
var
	error:	integer;
begin
	error := Registry_LoadService (className, path, entryName, priority, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure BeginMessage (handle: TRegistryHandle; command: integer);
var
	error:	integer;
begin
	error := Registry_BeginMessage (handle, command);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure EndMessage (handle: TRegistryHandle);
var
	error:	integer;
begin
	error := Registry_EndMessage (handle);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure BeginEvent (handle: TRegistryHandle; command: integer);
var
	error:	integer;
begin
	error := Registry_BeginEvent (handle, command);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure EndEvent (handle: TRegistryHandle);
var
	error:	integer;
begin
	error := Registry_EndEvent (handle);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutBool (handle: TRegistryHandle; value: boolean);
var
	error:	integer;
begin
	error := Registry_putByte (handle, Byte(value));

	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutByte (handle: TRegistryHandle; value: integer);
var
	error:	integer;
begin
	error := Registry_PutByte (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutShort (handle: TRegistryHandle; value: integer);
var
	error:	integer;
begin
	error := Registry_PutShort (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutInt32 (handle: TRegistryHandle; value: integer);
var
	error:	integer;
begin
	error := Registry_PutInt32 (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutInt64 (handle: TRegistryHandle; value: Int64);
var
	error:	integer;
begin
	error := Registry_PutInt64 (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutFloat (handle: TRegistryHandle; value: single);
var
	error:	integer;
begin
	error := Registry_PutFloat (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutDouble (handle: TRegistryHandle; value: double);
var
	error:	integer;
begin
	error := Registry_PutDouble (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutString (handle: TRegistryHandle; const value: PInteger);
var
	error:	integer;
begin
	error := Registry_PutString (handle, value);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutArray (handle: TRegistryHandle; const dataPtr: Pointer; size: LongWord);
var
	error:	integer;
begin
	error := Registry_PutArray (handle, dataPtr, size);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutHandle (handle: TRegistryHandle; h: TRegistryHandle);
var
	error: integer;
begin
	error := Registry_PutHandle (handle, h);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutWideString (handle: TRegistryHandle; const str: WideString);
var
	error:			integer;
begin
	error := Registry_putWideString (handle, str);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure PutWideStringArray (handle: TRegistryHandle; strArray: TStringArray);
var
	strLen: Integer;
	i: Integer;
begin
	strLen := Length (strArray);
	Contract.PutInt32 (handle, strLen);
	for i := 0 to strLen - 1 do
		Contract.PutWideString (handle, strArray [i]);
end;
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function GetBool (handle: TRegistryHandle): boolean;
var
	error:	integer;
	i:		integer;
begin
	error := Registry_GetByte (handle, i);
	if (error <> rcOk) then
		ThrowException (error);
	result := (i <> 0);
end;
//------------------------------------------------------------------------------
function GetByte (handle: TRegistryHandle): byte;
var
	error:	integer;
	b:		integer;
begin
	error := Registry_GetByte (handle, b);
	if (error <> rcOk) then
		ThrowException (error);

	result := b;
end;
//------------------------------------------------------------------------------
function GetShort (handle: TRegistryHandle): word;
var
	error:	integer;
	b:		integer;
begin
	error := Registry_GetShort (handle, b);
	if (error <> rcOk) then
		ThrowException (error);

	result := b;
end;
//------------------------------------------------------------------------------
function GetInt32 (handle: TRegistryHandle): integer;
var
	error:	integer;
begin
	error := Registry_GetInt32 (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetInt64 (handle: TRegistryHandle): Int64;
var
	error:	integer;
begin
	error := Registry_GetInt64 (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetFloat (handle: TRegistryHandle): single;
var
	error:	integer;
begin
	error := Registry_GetFloat (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetDouble (handle: TRegistryHandle): double;
var
	error:	integer;
begin
	error := Registry_GetDouble (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetString (handle: TRegistryHandle): PInteger;
var
	error:	integer;
begin
	error := Registry_GetString (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
procedure GetArray (handle: TRegistryHandle; var dataPtr: Pointer; var size: LongWord);
var
	error:	integer;
begin
	error := Registry_GetArray (handle, dataPtr, size);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetHandle (handle: TRegistryHandle): TRegistryHandle;
var
	error:	integer;
begin
	error := Registry_GetHandle (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetWideString (handle: TRegistryHandle): WideString;
var
	error:			integer;

begin
	error := Registry_GetWideString (handle, result);
	if (error <> rcOk) then
		ThrowException (error);
end;
//------------------------------------------------------------------------------
function GetWideStringArray (handle: TRegistryHandle): TStringArray;
var
	strLen: Integer;
	i: Integer;
begin
	strLen := Contract.GetInt32 (handle);
	SetLength (result, strLen);

	for i := 0 to strLen - 1 do
		result [i] := Contract.GetWideString (handle);
end;

//------------------------------------------------------------------------------
function IsInProcess (handle: TRegistryHandle): boolean;
var
	error:	integer;
	i:		integer;
begin
	error := Registry_isInProcess (handle, i);
	if (error <> rcOk) then
		ThrowException (error);

	result := (i <> 0);
end;

end.

