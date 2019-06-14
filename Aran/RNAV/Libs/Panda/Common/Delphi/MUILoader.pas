unit MUILoader;

interface

uses
	Classes,
	Controls;

type
	TMyControl = class(TControl)
	public
		property Caption;
		property Color;
	end;

	TMUILoader = class
	private
		FLangCode: Integer;
		FModule: Cardinal;
		FFreeLib: Boolean;
	public
		constructor Create (moduleFileName: String); overload;
		constructor Create (hModule: Cardinal); overload;
		destructor Destroy; override;

		function LoadString (var text: String; strId: Integer): Boolean; overload;
		function LoadString (myControl: TControl; strId: Integer): Boolean; overload;
		function LoadString (var text: String; strId: Integer; addStr: String): Boolean; overload;
		function LoadString (myControl: TControl; strId: Integer; addStr: String): Boolean; overload;

		property LangCode: Integer read FLangCode write FLangCode;
		property Module: Cardinal read FModule;
	end;

function GetStringFromTable (hModule: Cardinal; uID: Integer; lpBuffer: PChar;
	nBufferMax: Integer; wLanguage: Integer): Integer; stdcall; external 'MathRNAV.dll';

implementation

uses
	SysUtils,
	Windows;

{ FMUILoader }

constructor TMUILoader.Create (moduleFileName: String);
begin
	FLangCode := 1033;
	FFreeLib := True;
	FModule := LoadLibrary (PAnsiChar (moduleFileName));
end;

constructor TMUILoader.Create (hModule: Cardinal);
begin
	FLangCode := 1033;
	FFreeLib := False;
	FModule := hModule;
end;

destructor TMUILoader.Destroy;
begin
	if FFreeLib then
        FreeLibrary (FModule);

 	inherited;
end;

function TMUILoader.LoadString (var text: String; strId: Integer): Boolean;
var
	buffer: array [0 .. 4096] of char;
	nCopy: Integer;
begin
	nCopy := GetStringFromTable (FModule, strId, buffer, 4096, FLangCode);
	result := (nCopy >= 0);
	if result then
		text := buffer;
end;

function TMUILoader.LoadString (myControl: TControl; strId: Integer): Boolean;
var
	text: String;
begin
	result := LoadString (text, strId);
	if result then
		TMyControl (myControl).Caption := text;
end;

function TMUILoader.LoadString (var text: String; strId: Integer; addStr: String): Boolean;
var
	text_: String;
begin
	result := LoadString (text_, strId);
	if result then
		text := text_ + addStr;
end;

function TMUILoader.LoadString (myControl: TControl; strId: Integer; addStr: String): Boolean;
var
	text: String;
begin
	result := LoadString (text, strId);
	if result then
		TMyControl (myControl).Caption := text + addStr;
end;

end.
