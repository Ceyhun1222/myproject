unit Common;

interface
uses
  Contract,
  Registry,
  Classes,                                  
  SysUtils;

type

//------------------------------------------------------------------------------
	TPandaItem = class
	protected
		function CloneItem: TPandaItem;				virtual; abstract;
	public
		procedure Pack (handle: TRegistryHandle);	virtual; abstract;
		procedure Unpack (handle: TRegistryHandle);	virtual; abstract;
//		class function Unpack (handle: TRegistryHandle): TPandaItem; overload; virtual; abstract;
	end;
//------------------------------------------------------------------------------

	EPandaListException = class (Exception);

//------------------------------------------------------------------------------
	TPandaList = class
	private
		_list:		TList;
		_template:	TPandaItem;
	protected
		procedure SetItem (index: integer; const _Item: TPandaItem);	//overload; virtual;
		function At (index: integer): TPandaItem;						//overload; virtual;
	public
		constructor Create (template: TPandaItem);
		destructor Destroy;												override;

		procedure Insert (index: integer; const _Item: TPandaItem);
		procedure Remove (index: integer);
		function Count: integer;
		function Size: integer;

		procedure Clear;
		procedure Add (const _Item: TPandaItem);

		procedure Assign (const list: TPandaList);
		function Clone: TPandaList;

		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);
		property Item[I: Integer]: TPandaItem read At write SetItem;
	end;
//------------------------------------------------------------------------------

	TUnicalName = class (TPandaItem)
	public
		ID: String;
		Name: String;
		Tag: String;

		constructor Create ();

		procedure Assign (const value: TUnicalName);
		function Clone (): TUnicalName;

		function CloneItem: TPandaItem; override;

		procedure Pack (handle: TRegistryHandle); override;
		procedure Unpack (handle: TRegistryHandle); override;
	end;

implementation

//========================  TPandaList =========================================
//------------------------------------------------------------------------------
constructor TPandaList.Create (template: TPandaItem);
begin
	Inherited Create;
	_list := TList.Create;
	_template := template;
end;
//------------------------------------------------------------------------------
destructor TPandaList.Destroy;
begin
	Clear;
	_list.Free;
	_template.Free;
	Inherited;
end;
//------------------------------------------------------------------------------
procedure TPandaList.Remove (index: integer);
begin
	At (index).Free;
	_list.Delete (index);
end;
//------------------------------------------------------------------------------
procedure TPandaList.Clear;
var
	i: integer;
begin
	for i:=0 to _list.Count - 1 do
	begin
		At (i).Free;
	end;
	_list.Clear;
end;
//------------------------------------------------------------------------------
procedure TPandaList.Add (const _Item: TPandaItem);
begin
	_list.Add (_Item.CloneItem);
end;
//------------------------------------------------------------------------------
procedure TPandaList.Insert (index: integer; const _Item: TPandaItem);
begin
	_list.Insert (index, _Item.CloneItem);
end;
//------------------------------------------------------------------------------
procedure TPandaList.Assign (const list: TPandaList);
var
	i:	integer;
begin
	Clear;
	for i:=0 to list.Count - 1 do
	begin
		Add (list.At (i));
	end;
end;
//------------------------------------------------------------------------------
procedure TPandaList.Pack (handle: TRegistryHandle);
var
	i: integer;
begin
	Contract.PutInt32 (handle, _list.Count);

	for i:=0 to _list.Count - 1 do
	begin
		At (i).Pack (handle);
	end;
end;
//------------------------------------------------------------------------------
procedure TPandaList.Unpack (handle: TRegistryHandle);
var
	i:		integer;
	count:	integer;
begin
	Clear;
	count := Contract.GetInt32 (handle);
	for i:=0 to count - 1 do
	begin
		_template.Unpack (handle);
		Add (_template);
	end;
end;
//------------------------------------------------------------------------------
function TPandaList.Count: integer;
begin
	result := _list.Count;
end;
//------------------------------------------------------------------------------
function TPandaList.Size: integer;
begin
	result := _list.Count;
end;
//------------------------------------------------------------------------------
procedure TPandaList.SetItem (index: integer; const _Item: TPandaItem);
begin
	TPandaItem(_list.Items [index]).Free;
	_list.Items [index] := _Item.CloneItem;
end;
//------------------------------------------------------------------------------
function TPandaList.At (index: integer): TPandaItem;
begin
	result := TPandaItem (_list.Items [index]);
end;
//------------------------------------------------------------------------------
function TPandaList.Clone: TPandaList;
begin
	result := TPandaList.Create (_template.CloneItem);
	result.Assign (self);
end;
//------------------------------------------------------------------------------
{ TUnicalName }

procedure TUnicalName.Assign (const value: TUnicalName);
begin
	ID := value.ID;
	Name := value.Name;
	Tag := value.Tag;
end;

function TUnicalName.Clone: TUnicalName;
begin
	result := TUnicalName.Create;
	result.Assign (self);
end;

function TUnicalName.CloneItem: TPandaItem;
begin
    result := self.Clone;
end;

constructor TUnicalName.Create;
begin
	ID := '';
	Name := '';
	Tag := '';
end;

procedure TUnicalName.Pack (handle: TRegistryHandle);
begin
	Contract.PutWideString (handle, ID);
	Contract.PutWideString (handle, Name);
	Contract.PutWideString (handle, Tag);
end;

procedure TUnicalName.Unpack (handle: TRegistryHandle);
begin
	ID := Contract.GetWideString (handle);
	Name := Contract.GetWideString (handle);
	Tag := Contract.GetWideString (handle);
end;

end.

