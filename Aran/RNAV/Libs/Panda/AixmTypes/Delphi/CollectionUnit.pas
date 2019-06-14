unit CollectionUnit;

interface

uses
	Classes, Contract, AIXMTypes;

type
	TPandaCollection = class
	private
		FList:		TList;
//		FTemplate:	TPandaItem;
	protected
		procedure SetItem (index: integer; const Value: TAIXM);	//overload; virtual;
		function GetItem (index: integer): TAIXM;				//overload; virtual;
	public
//		constructor Create (template: TPandaItem);
		constructor Create;
		destructor Destroy;												override;

		procedure Insert (index: integer; const Value: TAIXM);
		procedure Remove (index: integer);
		function Count: integer;
		function Size: integer;

		procedure Clear;
		procedure Add (const Value: TAIXM);

		procedure Assign (const Value: TPandaCollection);
		function Clone: TPandaCollection;

		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);
		property Item[I: Integer]: TAIXM read GetItem write SetItem;
	end;

	TSignificantPointCollection = class (TPandaCollection)
	private
		procedure SetItem (index: integer; const Value: TSignificantPoint);
		function GetItem(index: integer): TSignificantPoint;
	public
		constructor Create;
		property Item [I: Integer]: TSignificantPoint read GetItem write SetItem;
	end;

implementation

//========================  TPandaCollection =========================================

constructor TPandaCollection.Create;
begin
	Inherited;
	FList := TList.Create;

end;

destructor TPandaCollection.Destroy;
begin
	Clear;
	FList.Free;

	Inherited;
end;

procedure TPandaCollection.Remove (index: integer);
begin
	GetItem(index).Free;
	FList.Delete (index);
end;

procedure TPandaCollection.Clear;
var
	i: integer;
begin
	for i:=0 to FList.Count - 1 do
	begin
		GetItem (i).Free;
	end;
	FList.Clear;
end;

procedure TPandaCollection.Add (const Value: TAIXM);
begin
	FList.Add (Value.Clone);
end;

procedure TPandaCollection.Insert (index: integer; const Value: TAIXM);
begin
	FList.Insert (index, Value.Clone);
end;

procedure TPandaCollection.Assign (const Value: TPandaCollection);
var
	i:	integer;
begin
	Clear;
	for i := 0 to Value.Count - 1 do
	begin
		Add (Value.GetItem (i));
	end;
end;

procedure TPandaCollection.Pack (handle: TRegistryHandle);
var
	i: integer;
begin
	Contract.PutInt32 (handle, FList.Count);

	for i:=0 to FList.Count - 1 do
	begin
		GetItem(i).AIXMPack (handle);
	end;
end;

procedure TPandaCollection.Unpack (handle: TRegistryHandle);
var
	i:			integer;
	count:		integer;
	NextItem:	TAIXM;
begin
	Clear;
	count := Contract.GetInt32 (handle);

	for i := 0 to count - 1 do
	begin

		NextItem := TAIXM.AIXMUnpack(handle);
		try
			Add (NextItem);
		finally
			NextItem.Free;
		end;
	end;
end;

function TPandaCollection.Count: integer;
begin
	result := FList.Count;
end;

function TPandaCollection.Size: integer;
begin
	result := FList.Count;
end;

procedure TPandaCollection.SetItem (index: integer; const Value: TAIXM);
begin
	TAIXM(FList.Items [index]).Free;
	FList.Items [index] := Value.Clone;
end;

function TPandaCollection.GetItem (index: integer): TAIXM;
begin
	result := TAIXM (FList.Items [index]);
end;

function TPandaCollection.Clone: TPandaCollection;
begin
	result := TPandaCollection.Create;
	result.Assign (self);
end;

//================== TSignificantPointCollection =============================
constructor TSignificantPointCollection.Create;
begin
	inherited Create;
end;

procedure TSignificantPointCollection.SetItem (index: integer; const Value: TSignificantPoint);
begin
	inherited SetItem (index, Value);
end;

function TSignificantPointCollection.GetItem (index: integer): TSignificantPoint;
begin
	result := TSignificantPoint (inherited GetItem (index));
end;

end.
