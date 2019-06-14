unit ListUnit;

interface

uses Classes, Contract, AIXMObjectTypes;

type

	TPANDAList = class(TList)
	private
		_aixmType:	TAixmType;
	public
		constructor Create(ObjType: TAIXMType);
		procedure Clear; override;
		procedure Pack (handle: TRegistryHandle);
		procedure Unpack (handle: TRegistryHandle);
	end;

implementation

uses
	AIXMTypes;

constructor TPANDAList.Create(ObjType: TAIXMType);
begin
	Inherited Create;
	_aixmType := ObjType;
end;

procedure TPANDAList.Clear;
var
	I:	Integer;
begin
	for I := 0 to Count-1 do
		TAIXM(List[I]).Free;
	inherited;
end;

procedure TPANDAList.Pack (handle: TRegistryHandle);
var
	sz, i:		Integer;
begin
	sz := Count;
	Registry_putInt(handle, sz);

	for i := 0 to sz -1 do
	begin
//		TAIXM(List[I]).pack(handle);

//		case TAIXM(List[I]).GetAixmType of
		case _aixmType of
		atDME:
			TDME(List[I]).pack(handle);
		atVOR:
			TVOR(List[I]).pack(handle);
		atObstacle:
			TObstacle(List[I]).pack(handle);
		atAHP:
			TAHP(List[I]).pack(handle);
		atRWY:
			TRWY(List[I]).pack(handle);
		atDesignatedPoint:
			TDesignatedPoint(List[I]).pack(handle);
		end;
	end;
end;

procedure TPANDAList.Unpack (handle: TRegistryHandle);
var
	sz, i:		Integer;
	obj:		TAIXM;
begin
	Clear;
	Registry_getInt (handle, sz);
	obj := nil;
	for i := 0 to sz -1 do
	begin
		case _aixmType of
		atDME:
			obj := TDME.Create;
		atVOR:
			obj := TVOR.Create;
		atObstacle:
			obj := TObstacle.Create;
		atAHP:
			obj := TAHP.Create;
		atRWY:
			obj := TRWY.Create;
		atDesignatedPoint:
			obj := TDesignatedPoint.Create;
		end;

		if Assigned(obj) then
			obj.unpack (handle);
		add (obj);
		obj := nil;
	end;
end;

end.

