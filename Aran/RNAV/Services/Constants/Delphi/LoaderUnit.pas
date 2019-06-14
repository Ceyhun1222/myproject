unit LoaderUnit;

interface
uses
	SysUtils, ConstantsContract;

const
	Sign: PChar = 'Anplan DATABASE file';
	InValidFile: string = 'Invalid constant file.';

type
	// Helper classes

{	TNamedConstantObjectLoader = class(TNamedConstantObject)
	public
		constructor Create;
		procedure LoadFromFile(FileName: TFileName);
	end;
}
//======================================================
{	TPANSOPS_Constant = class(TNamedConstantObject)
	protected
		FValue:		Double;
	public
		constructor Create;
		function CloneItem: TPandaItem;				override;
		procedure Pack (handle: TRegistryHandle);	override;
		procedure Unpack (handle: TRegistryHandle);	override;

		property Value: Double read FValue;
	end;
}
	TPANSOPSConstantLoader = class(TPANSOPSConstant)
		procedure SetName(Value: string);
		procedure SetUnit(Value: string);
		procedure SetDefinedIn(Value: string);
		procedure SetComment(Value: string);
		procedure SetMultiplier(Value: Double);
		procedure SetValue(Value: Double);
	end;

	TGNSSConstantLoader = class(TGNSSConstant)
		procedure SetName(Value: string);
		procedure SetUnit(Value: string);
		procedure SetDefinedIn(Value: string);
		procedure SetComment(Value: string);
		procedure SetMultiplier(Value: Double);

		procedure SetFIXRole(Value: string);
		procedure SetAccuracy(Value: Double);
		procedure SetThreshold(Value: Double);
		procedure SetFTT(Value: Double);
		procedure SetATT(Value: Double);
		procedure SetXTT(Value: Double);
		procedure SetSemiWidth(Value: Double);
	end;

	TCategoryData = Array [TAircraftCategory] of Double;

	TAircraftCategoryConstantLoader = class(TAircraftCategoryConstant)
		procedure SetName(Value: string);
		procedure SetUnit(Value: string);
		procedure SetDefinedIn(Value: string);
		procedure SetComment(Value: string);
		procedure SetMultiplier(Value: Double);
		procedure SetValue(Value: TCategoryData);
		procedure SetCategoryValue(Category: TAircraftCategory; Value: Double);
	end;
//======================================================
	TPANSOPSConstantListLoader = class(TPANSOPSConstantList)
		procedure LoadFromFile(FileName: TFileName);
		procedure Merge(List: TPANSOPSConstantList);
	end;

	TGNSSConstantListLoader = class(TGNSSConstantList)
		procedure LoadFromFile(FileName: TFileName; PBNClass: TPBNClass);
	end;

	TAircraftCategoryListLoader = class(TAircraftCategoryList)
		procedure LoadFromFile(FileName: TFileName);
	end;
    
implementation
uses
	Math;	//, DB;
//======================== TPANSOPS_ConstantLoader =============================

procedure TPANSOPSConstantLoader.SetName(Value: string);
begin
	FName := Value;
end;

procedure TPANSOPSConstantLoader.SetUnit(Value: string);
begin
	FUnit := Value;
end;

procedure TPANSOPSConstantLoader.SetDefinedIn(Value: string);
begin
	FDefinedIn := Value;
end;

procedure TPANSOPSConstantLoader.SetComment(Value: string);
begin
	FComment := Value;
end;

procedure TPANSOPSConstantLoader.SetMultiplier(Value: Double);
begin
	FMultiplier := Value;
end;

procedure TPANSOPSConstantLoader.SetValue(Value: Double);
begin
	FValue := Value;
end;

//=========================== TGNSS_ConstantLoader =============================
procedure TGNSSConstantLoader.SetName(Value: string);
begin
	FName := Value;
end;

procedure TGNSSConstantLoader.SetUnit(Value: string);
begin
	FUnit := Value;
end;

procedure TGNSSConstantLoader.SetDefinedIn(Value: string);
begin
	FDefinedIn := Value;
end;

procedure TGNSSConstantLoader.SetComment(Value: string);
begin
	FComment := Value;
end;

procedure TGNSSConstantLoader.SetMultiplier(Value: Double);
begin
	FMultiplier := Value;
end;

procedure TGNSSConstantLoader.SetFIXRole(Value: string);
begin
	FFIXRole := Value;
end;

procedure TGNSSConstantLoader.SetAccuracy(Value: Double);
begin
	FAccuracy := Value;
end;

procedure TGNSSConstantLoader.SetThreshold(Value: Double);
begin
	FThreshold := Value;
end;

procedure TGNSSConstantLoader.SetFTT(Value: Double);
begin
	FFTT := Value;
end;

procedure TGNSSConstantLoader.SetATT(Value: Double);
begin
	FATT := Value;
end;

procedure TGNSSConstantLoader.SetXTT(Value: Double);
begin
	FXTT := Value;
end;

procedure TGNSSConstantLoader.SetSemiWidth(Value: Double);
begin
	FSemiWidth := Value;
end;

//===================== TAircraftCategory_ConstantLoader =======================

procedure TAircraftCategoryConstantLoader.SetName(Value: string);
begin
	FName := Value;
end;

procedure TAircraftCategoryConstantLoader.SetUnit(Value: string);
begin
	FUnit := Value;
end;

procedure TAircraftCategoryConstantLoader.SetDefinedIn(Value: string);
begin
	FDefinedIn := Value;
end;

procedure TAircraftCategoryConstantLoader.SetComment(Value: string);
begin
	FComment := Value;
end;

procedure TAircraftCategoryConstantLoader.SetMultiplier(Value: Double);
begin
	FMultiplier := Value;
end;

procedure TAircraftCategoryConstantLoader.SetValue(Value: TCategoryData);
var
	ac:	TAircraftCategory;
begin
	for ac := acA to acH do
		FValue[ac] := Value[ac]
end;

procedure TAircraftCategoryConstantLoader.SetCategoryValue(Category: TAircraftCategory; Value: Double);
begin
	FValue[Category] := Value
end;

//================================ Loaders =====================================
procedure GetData(Data: PByteArray; var index: integer; var Vara;  size: integer);
var
	i:	integer;
begin
	for i := 0 to size-1 do
		TByteArray(Vara)[i] := Data[index+i];

	inc(index, size);
end;

//======================= TPANSOPS_ConstantListLoader ==========================
procedure TPANSOPSConstantListLoader.Merge(List: TPANSOPSConstantList);
var
	pd:		TPANSOPSData;
begin
	for pd := Low(TPANSOPSData) to High(TPANSOPSData) do
		if (self.Constant[pd] = nil) and (List.Constant[pd]<> nil) then
			self.AddItem(List.Constant[pd]);
end;

procedure TPANSOPSConstantListLoader.LoadFromFile(FileName: TFileName);
var
	I, J:					Integer;
	Transferred, DataSize:	Integer;
	Fields, Records, Index:	Integer;
//	PANS_OPS_File
	F:						file of Byte;
	Data:					PByteArray;

	BoolField:				Boolean;
//	WordField:				Word;
	SmallIntField:			Smallint;
	intField:				Longint;
//	largintField:			Int64;
	charField:				array [0..1023] of char;
	FieldName:				array [0..1023] of char;

	floatField:				double;
	CurrencyField:			Currency;
	DateTimeField:			TDateTime;
	FieldType:				Integer;	//TFieldType;
	PANSOPSConstant:		TPANSOPSConstantLoader;
begin
	AssignFile(F, FileName);
	FileMode := 0;

	Reset(F);
	try
		BlockRead(F, charField, 20, Transferred);
		charField[Transferred] := Chr(0);

		if (Transferred<>20) or(StrComp(charField, Sign)<>0) then
			Exception.Create(InValidFile);
	except
		CloseFile(F);
		raise;
	end;

	DataSize := FileSize(F) - 20;
	GetMem(Data, DataSize);

	try
		BlockRead(F, Data^, DataSize, Transferred);
		if Transferred<>DataSize then
			Exception.Create(InValidFile);
	except
		FreeMem(Data);
		CloseFile(F);
		raise;
	end;
	CloseFile(F);

//==============================================================================
	Data^[DataSize-1] := Data^[DataSize-1] xor Ord('R');
	for i := DataSize-2 downto 0 do
		Data^[i] := Data^[i] xor Data^[i+1];

	Index := 0;
	GetData(Data, Index, SmallIntField, 2);
	fields := SmallIntField;
	GetData(Data, Index, SmallIntField, 2);
	records := SmallIntField;

	for i := 0 to fields - 1 do
	begin
		GetData(Data, Index, SmallIntField, 2);
		GetData(Data, Index, charField, SmallIntField);
		charField[SmallIntField] := char(0);
	end;

	PANSOPSConstant := TPANSOPSConstantLoader.Create;
	try
		for I := 0 to Records - 1 do
		begin
			PANSOPSConstant.SetName('');
			PANSOPSConstant.SetUnit('');
			PANSOPSConstant.SetDefinedIn('');
			PANSOPSConstant.SetComment('');
			PANSOPSConstant.SetMultiplier(NaN);
			PANSOPSConstant.SetValue(NaN);

			for J := 0 to Fields - 1 do
			begin
				GetData(Data, Index, SmallIntField, 2);
				GetData(Data, Index, FieldName, SmallIntField);
				FieldName[SmallIntField] := char(0);

				GetData(Data, Index, SmallIntField, 2);
				FieldType := SmallIntField;

				case FieldType of
//				ftFixedChar,	//Fixed character field
//				ftWideString,	//Wide string field
//				ftMemo,			//Text memo field
//				ftFmtMemo,		//Formatted text memo field
//				ftString:		//Character or string field
				1, 16, 18, 23, 24:
					begin
						GetData(Data, Index, SmallIntField, 2);
						GetData(Data, Index, charField, SmallIntField);
						charField[SmallIntField] := char(0);
							 if StrComp(FieldName, 'NAME') = 0 then			PANSOPSConstant.SetName(string(charField))
						else if StrComp(FieldName, 'UNIT') = 0 then			PANSOPSConstant.SetUnit(string(charField))
						else if StrComp(FieldName, 'DEFINED_IN') = 0 then	PANSOPSConstant.SetDefinedIn(string(charField))
						else if StrComp(FieldName, 'COMMENT') = 0 then		PANSOPSConstant.SetComment(string(charField))
					end;
//				ftSmallint:		//16-bit integer field
//				ftWord:			//16-bit unsigned integer field
				// 16-bit integer field
				// 16-bit unsigned integer field
				2, 4:
					GetData(Data, Index, SmallIntField, 2);
//					GetData(Data, Index, WordField, 2);

//				ftLargeInt,		//Large integer field
//				ftInteger:		//32-bit integer field
				3, 25:
					GetData(Data, Index, intField, sizeOf(Longint));

{				ftLargeInt:		//Large integer field
					begin
						largintField := Table1.Fields[j].AsInteger;
						PutData(intField, sizeOf(Int64));
					end;
}

//				ftBoolean:		//Boolean field
				5:		//Boolean field
					GetData(Data, Index, BoolField, sizeOf(Boolean));

//				ftBCD, ftFloat:		//Floating-point numeric field
				6, 8:	//Floating-point numeric field
					begin
						GetData(Data, Index, floatField, sizeOf(double));
							 if StrComp(FieldName, 'MULTIPLIER') = 0 then	PANSOPSConstant.SetMultiplier(floatField)
						else if StrComp(FieldName, 'VALUE') = 0 then		PANSOPSConstant.SetValue(floatField)
					end;
//				ftCurrency:		//Money field
				7:	//Money field
					GetData(Data, Index, CurrencyField, sizeOf(Currency));
//				ftDate,			//Date field
//				ftTime,			//Time field
//				ftDateTime:		//Date and time field
				9, 10, 11:
					GetData(Data, Index, DateTimeField, sizeOf(TDateTime));
				end;
			end;
			PANSOPSConstant.FValue := PANSOPSConstant.FValue * PANSOPSConstant.FMultiplier;
			AddItem(PANSOPSConstant);
		end;
	finally
		FreeMem(Data);
		PANSOPSConstant.Free;
	end;
end;

//========================= TGNSS_ConstantListLoader ===========================

procedure TGNSSConstantListLoader.LoadFromFile(FileName: TFileName; PBNClass: TPBNClass);
var
	I, J:					Integer;
	Transferred, DataSize:	Integer;
	Fields, Records, Index:	Integer;

//	GNSSFile
	F:						file of Byte;
	Data:					PByteArray;

	BoolField:				Boolean;
//	WordField:				Word;
	SmallIntField:			Smallint;
	intField:				Integer;

	charField:				array [0..1023] of char;
	FieldName:				array [0..1023] of char;

	floatField:				Double;
	CurrencyField:			Currency;
	DateTimeField:			TDateTime;
	FieldType:				Integer;

	GNSS_Constant:			TGNSSConstantLoader;
begin
	AssignFile(F, FileName);
	FileMode := 0;
	Reset(F);

	try
		BlockRead(F, charField, 20, Transferred);
		charField[Transferred] := Chr(0);

		if (Transferred<>20) or(StrComp(charField, Sign)<>0) then
			Exception.Create(InValidFile);
	except
		CloseFile(F);
		raise;
	end;

	DataSize := FileSize(F) - 20;
	GetMem(Data, DataSize);

	try
		BlockRead(F, Data^, DataSize, Transferred);
		if Transferred<>DataSize then
			Exception.Create(InValidFile);
	except
		FreeMem(Data);
		CloseFile(F);
		raise;
	end;
	CloseFile(F);

//==============================================================================
	Data^[DataSize-1] := Data^[DataSize-1] xor Ord('R');
	for i := DataSize-2 downto 0 do
		Data^[i] := Data^[i] xor Data^[i+1];

	Index := 0;
	GetData(Data, Index, SmallIntField, 2);
	fields := SmallIntField;
	GetData(Data, Index, SmallIntField, 2);
	records := SmallIntField;
	GNSS_Constant := TGNSSConstantLoader.Create(PBNClass);

	try
		for I := 0 to Fields - 1 do
		begin
			GetData(Data, Index, SmallIntField, 2);
			GetData(Data, Index, charField, SmallIntField);
			charField[SmallIntField] := char(0);
		end;

		for I := 0 to Records - 1 do
		begin
			GNSS_Constant.SetName('');
			GNSS_Constant.SetUnit('');
			GNSS_Constant.SetDefinedIn('');
			GNSS_Constant.SetComment('');
			GNSS_Constant.SetFIXRole('');

			GNSS_Constant.SetMultiplier(NaN);
			GNSS_Constant.SetAccuracy(NaN);
			GNSS_Constant.SetThreshold(NaN);
			GNSS_Constant.SetFTT(NaN);
			GNSS_Constant.SetATT(NaN);
			GNSS_Constant.SetXTT(NaN);
			GNSS_Constant.SetSemiWidth(NaN);

			for J := 0 to Fields - 1 do
			begin
				GetData(Data, Index, SmallIntField, 2);
				GetData(Data, Index, FieldName, SmallIntField);
				FieldName[SmallIntField] := char(0);

				GetData(Data, Index, SmallIntField, 2);
				FieldType := SmallIntField;

				Case FieldType of
				// Fixed character field
				// Wide string field
				// Text memo field
				// Formatted text memo field
				// Character or string field
				1, 16, 18, 23, 24:
					begin
						GetData(Data, Index, SmallIntField, 2);
						GetData(Data, Index, charField, SmallIntField);
						charField[SmallIntField] := char(0);

							 if StrComp(FieldName, 'NAME') = 0 then			GNSS_Constant.SetName(string(charField))
						else if StrComp(FieldName, 'UNIT') = 0 then			GNSS_Constant.SetUnit(string(charField))
						else if StrComp(FieldName, 'DEFINED_IN') = 0 then	GNSS_Constant.SetDefinedIn(string(charField))
						else if StrComp(FieldName, 'COMMENT') = 0 then		GNSS_Constant.SetComment(string(charField))
						else if StrComp(FieldName, 'FIX') = 0 then			GNSS_Constant.SetFIXRole(string(charField))
				end;

				// 16-bit integer field
				// 16-bit unsigned integer field
				2, 4:
					GetData(Data, Index, SmallIntField, 2);
				//Large integer field
				//32-bit integer field
				3, 25:
					GetData(Data, Index, intField, 4);
				5:		//Boolean field
					GetData(Data, Index, BoolField, 2);
				6, 8:	//Floating-point numeric field
					begin
						GetData(Data, Index, floatField, 8);
							 if StrComp(FieldName, 'MULTIPLIER') = 0 then	GNSS_Constant.SetMultiplier(floatField)
						else if StrComp(FieldName, 'ACCURACY')=0 then		GNSS_Constant.SetAccuracy(floatField)
						else if StrComp(FieldName, 'THRESHOLD')=0 then		GNSS_Constant.SetThreshold(floatField)
						else if StrComp(FieldName, 'FTT')=0 then			GNSS_Constant.SetFTT(floatField)
						else if StrComp(FieldName, 'ATT')=0 then			GNSS_Constant.SetATT(floatField)
						else if StrComp(FieldName, 'XTT')=0 then			GNSS_Constant.SetXTT(floatField)
						else if StrComp(FieldName, 'SEMIWIDTH')=0 then		GNSS_Constant.SetSemiWidth(floatField)
					end;
				7:	//Money field
					GetData(Data, Index, CurrencyField, 8);
				//Date field
				//Time field
				//Date and time field
				9, 10, 11:
					GetData(Data, Index, DateTimeField, 8);
				end;
			end;		//J
			if GNSS_Constant.FName = '' then
			begin
				if PBNClass = pcGNSS then
				begin
						 if GNSS_Constant.FIXRole = 'IAF>56'	then	GNSS_Constant.FName := 'IAF_GT_56'
					else if GNSS_Constant.FIXRole = 'IAF<=56'	then	GNSS_Constant.FName := 'IAF_LE_56'
					else if GNSS_Constant.FIXRole = 'IF'		then	GNSS_Constant.FName := 'IF'
					else if GNSS_Constant.FIXRole = 'FAF'		then	GNSS_Constant.FName := 'FAF'
					else if GNSS_Constant.FIXRole = 'MAPt'		then	GNSS_Constant.FName := 'MAPt'
					else if GNSS_Constant.FIXRole = 'MATF<=56'	then	GNSS_Constant.FName := 'MATF_LE_56'
					else if GNSS_Constant.FIXRole = 'MATF>56'	then	GNSS_Constant.FName := 'MATF_GT_56'
					else if GNSS_Constant.FIXRole = 'MAHF<=56'	then	GNSS_Constant.FName := 'MAHF_LE_56'
					else if GNSS_Constant.FIXRole = 'MAHF>56'	then	GNSS_Constant.FName := 'MAHF_GT_56'
					else if GNSS_Constant.FIXRole = 'IDEP'		then	GNSS_Constant.FName := 'IDEP'
					else if GNSS_Constant.FIXRole = 'DEP'		then	GNSS_Constant.FName := 'DEP'
				end
				else //if PBNClass = pcRNP_APCH then
				begin
						 if GNSS_Constant.FIXRole = 'IAF'		then	GNSS_Constant.FName := 'PBN_IAF'
					else if GNSS_Constant.FIXRole = 'IF'		then	GNSS_Constant.FName := 'PBN_IF'
					else if GNSS_Constant.FIXRole = 'FAF'		then	GNSS_Constant.FName := 'PBN_FAF'
					else if GNSS_Constant.FIXRole = 'MAPt'		then	GNSS_Constant.FName := 'PBN_MAPt'
					else if GNSS_Constant.FIXRole = 'MATF<28'	then	GNSS_Constant.FName := 'PBN_MATF_LT_28'
					else if GNSS_Constant.FIXRole = 'MATF>=28'	then	GNSS_Constant.FName := 'PBN_MATF_GE_28'
				end
			end;
			GNSS_Constant.FAccuracy := GNSS_Constant.FAccuracy * GNSS_Constant.FMultiplier;
			GNSS_Constant.FThreshold := GNSS_Constant.FThreshold * GNSS_Constant.FMultiplier;
			GNSS_Constant.FFTT := GNSS_Constant.FFTT * GNSS_Constant.FMultiplier;
			GNSS_Constant.FATT := GNSS_Constant.FATT * GNSS_Constant.FMultiplier;
			GNSS_Constant.FXTT := GNSS_Constant.FXTT * GNSS_Constant.FMultiplier;
			GNSS_Constant.FSemiWidth := GNSS_Constant.FSemiWidth * GNSS_Constant.FMultiplier;
			AddItem(GNSS_Constant);
		end;		//I
	finally
		FreeMem(Data);
		GNSS_Constant.Free;
	end;
end;

//========================= TAircraftCategoryListLoader ========================

procedure TAircraftCategoryListLoader.LoadFromFile(FileName: TFileName);
var
	I, J:					Integer;
	Transferred, DataSize:	Integer;
	Fields, Records, Index:	Integer;

//	GNSSFile
	F:						file of Byte;
	Data:					PByteArray;

	BoolField:				Boolean;
//	WordField:				Word;
	SmallIntField:			Smallint;
	intField:				Integer;

	charField:				array [0..1023] of char;
	FieldName:				array [0..1023] of char;

	floatField:				Double;
	CurrencyField:			Currency;
	DateTimeField:			TDateTime;
	FieldType:				Integer;

	AircraftCategoryConstant:	TAircraftCategoryConstantLoader;
begin
	AssignFile(F, FileName);
	FileMode := 0;
	Reset(F);

	try
		BlockRead(F, charField, 20, Transferred);
		charField[Transferred] := Chr(0);

		if (Transferred<>20) or(StrComp(charField, Sign)<>0) then
			Exception.Create(InValidFile);
	except
		CloseFile(F);
		raise;
	end;

	DataSize := FileSize(F) - 20;
	GetMem(Data, DataSize);

	try
		BlockRead(F, Data^, DataSize, Transferred);
		if Transferred<>DataSize then
			Exception.Create(InValidFile);
	except
		FreeMem(Data);
		CloseFile(F);
		raise;
	end;
	CloseFile(F);

//==============================================================================
	Data^[DataSize-1] := Data^[DataSize-1] xor Ord('R');
	for i := DataSize-2 downto 0 do
		Data^[i] := Data^[i] xor Data^[i+1];

	Index := 0;
	GetData(Data, Index, SmallIntField, 2);
	fields := SmallIntField;
	GetData(Data, Index, SmallIntField, 2);
	records := SmallIntField;
	AircraftCategoryConstant := TAircraftCategoryConstantLoader.Create;

	try
		for I := 0 to Fields - 1 do
		begin
			GetData(Data, Index, SmallIntField, 2);
			GetData(Data, Index, charField, SmallIntField);
			charField[SmallIntField] := char(0);
		end;

		for I := 0 to Records - 1 do
		begin
			AircraftCategoryConstant.SetName('');
			AircraftCategoryConstant.SetUnit('');
			AircraftCategoryConstant.SetDefinedIn('');
			AircraftCategoryConstant.SetComment('');

			AircraftCategoryConstant.SetMultiplier(floatField);
			AircraftCategoryConstant.SetCategoryValue(acA, NaN);
			AircraftCategoryConstant.SetCategoryValue(acB, NaN);
			AircraftCategoryConstant.SetCategoryValue(acC, NaN);
			AircraftCategoryConstant.SetCategoryValue(acD, NaN);
			AircraftCategoryConstant.SetCategoryValue(acDL, NaN);
			AircraftCategoryConstant.SetCategoryValue(acE, NaN);
			AircraftCategoryConstant.SetCategoryValue(acH, NaN);

			for J := 0 to Fields - 1 do
			begin
				GetData(Data, Index, SmallIntField, 2);
				GetData(Data, Index, FieldName, SmallIntField);
				FieldName[SmallIntField] := char(0);

				GetData(Data, Index, SmallIntField, 2);
				FieldType := SmallIntField;

				Case FieldType of
				// Fixed character field
				// Wide string field
				// Text memo field
				// Formatted text memo field
				// Character or string field
				1, 16, 18, 23, 24:
					begin
						GetData(Data, Index, SmallIntField, 2);
						GetData(Data, Index, charField, SmallIntField);
						charField[SmallIntField] := char(0);

							 if StrComp(FieldName, 'NAME') = 0 then			AircraftCategoryConstant.SetName(string(charField))
						else if StrComp(FieldName, 'UNIT') = 0 then			AircraftCategoryConstant.SetUnit(string(charField))
						else if StrComp(FieldName, 'DEFINED_IN') = 0 then	AircraftCategoryConstant.SetDefinedIn(string(charField))
						else if StrComp(FieldName, 'COMMENT') = 0 then		AircraftCategoryConstant.SetComment(string(charField))
				end;

				// 16-bit integer field
				// 16-bit unsigned integer field
				2, 4:
					GetData(Data, Index, SmallIntField, 2);
				//Large integer field
				//32-bit integer field
				3, 25:
					GetData(Data, Index, intField, 4);
				5:		//Boolean field
					GetData(Data, Index, BoolField, 2);
				6, 8:	//Floating-point numeric field
					begin
						GetData(Data, Index, floatField, 8);
							 if StrComp(FieldName, 'MULTIPLIER') = 0 then	AircraftCategoryConstant.SetMultiplier(floatField)
						else if StrComp(FieldName, 'A')=0 then				AircraftCategoryConstant.SetCategoryValue(acA, floatField)
						else if StrComp(FieldName, 'B')=0 then				AircraftCategoryConstant.SetCategoryValue(acB, floatField)
						else if StrComp(FieldName, 'C')=0 then				AircraftCategoryConstant.SetCategoryValue(acC, floatField)
						else if StrComp(FieldName, 'D')=0 then				AircraftCategoryConstant.SetCategoryValue(acD, floatField)
						else if StrComp(FieldName, 'DL')=0 then				AircraftCategoryConstant.SetCategoryValue(acDL, floatField)
						else if StrComp(FieldName, 'E')=0 then				AircraftCategoryConstant.SetCategoryValue(acE, floatField)
						else if StrComp(FieldName, 'H')=0 then				AircraftCategoryConstant.SetCategoryValue(acH, floatField)
					end;
				7:	//Money field
					GetData(Data, Index, CurrencyField, 8);
				//Date field
				//Time field
				//Date and time field
				9, 10, 11:
					GetData(Data, Index, DateTimeField, 8);
				end;
			end;	//J

			AircraftCategoryConstant.FValue[acA] := AircraftCategoryConstant.FValue[acA] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acB] := AircraftCategoryConstant.FValue[acB] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acC] := AircraftCategoryConstant.FValue[acC] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acD] := AircraftCategoryConstant.FValue[acD] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acDL] := AircraftCategoryConstant.FValue[acDL] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acE] := AircraftCategoryConstant.FValue[acE] * AircraftCategoryConstant.FMultiplier;
			AircraftCategoryConstant.FValue[acH] := AircraftCategoryConstant.FValue[acH] * AircraftCategoryConstant.FMultiplier;
			AddItem(AircraftCategoryConstant);
		end;		//I
	finally
		FreeMem(Data);
		AircraftCategoryConstant.Free;
	end;
end;

//======================================================

end.
