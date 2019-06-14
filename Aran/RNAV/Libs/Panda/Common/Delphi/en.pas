unit en;

interface

uses
	Windows, Geometry;

const
	PandaRegKeyPath = 'SOFTWARE\RISK\RNAV';
	KeyName = 'Acar';

procedure InitModule;
procedure SetNewSeedL(initSeed: Cardinal);
procedure SetNewSeedS(initString: String);
function LstStDtWriter(ModuleName: String): Integer;
function DecodeLCode(LCode, ModuleName: String):	TPolygon;

implementation

uses
	SysUtils, Math, Registry, CRC32Unit,
	GeometryOperatorsContract, ARANFunctions, //;
	ARANGlobals, UIContract;					//For drawing

const
	m_CodeLen		= 36;
	m_InputRange	= 11;
	m_NGnerators	= 6;

	m_CodeScale		= 0.305555555555555555;		//m_InputRange / m_CodeLen
//	m_InvCodeScale	= 3.272727272727272727;		//m_CodeLen / m_InputRange
	m_Scale1		= 36.0 / 4294967296.0;
	m_Scale2		= 36.0 / 65536.0;

	SSTableSize		= 128;
	SSTableMask		= SSTableSize - 1;
	SSTableBits		= 7;

var
	SeedValue:			Cardinal;
	MinV, MaxV,
	GeneratorIndex:		Integer;
	FillSSMode:			Boolean;
	SSArray:			Array [0 .. SSTableSize-1] of Cardinal;

function Generator(Index: Integer): Byte;
	function Generator0: Byte;
	var
		I:			Cardinal;
		K, J0, J1:	Integer;
	begin
		I := 22695477 * SeedValue + 37;

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			K := 0;
			while (SSArray[J1] = I) and (K < 128) do
			begin
				J1 := (J1 + 1) and SSTableMask;
				Inc(K);
			end;

			if SSArray[J1] = I then
				SSArray[J1] := 22695477 + I;

			J0 := J1 - 23;
			if J0 < 0 then	Inc(J0, SSTableSize);

			SeedValue := (SSArray[J0] and $FFFF0000) or (SSArray[J1] shr 16);
			SSArray[J1] := I;
		end;

		Result := Byte(Floor(m_Scale1 * SeedValue))
	end;

	function Generator1: Byte;
	var
		I:			Cardinal;
		J0, J1:		Integer;
	begin
		I := 22695461 * SeedValue + 3;

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			J0 := (SeedValue shr 24) and SSTableMask;

			SeedValue := (SSArray[J0] and $FFFF0000) or (SSArray[J1] shr 16);
			SSArray[J0] := I
		end;

		Result := Byte(Floor(m_Scale1 * SeedValue));
	end;

	function Generator2: Byte;
	var
		I:			Cardinal;
		J0, J1:		Integer;
	begin
		I := SeedValue * (SeedValue + 3);

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			J0 := (SeedValue shr 24) and SSTableMask;
			SeedValue := (SSArray[J1] and $FFFF0000) or (SSArray[J0] shr 16);
			SSArray[J1] := I;
		end;

		Result := Byte(Floor(m_Scale1 * SeedValue));
	end;

	function Generator3: Byte;
	var
		I:				Cardinal;
		K, J0, J1:		Integer;
	begin
		I := 22695477 * SeedValue + 37;

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			K := 0;
			while (SSArray[J1] = I) and (K < 128) do
			begin
				J1 := (J1 + 63) and SSTableMask;
				Inc(K);
			end;

			if SSArray[J1] = I then
				SSArray[J1] := 22695477 + I;

			J0 := J1 - 23;
			if J0 < 0 then Inc(J0, SSTableSize);

			SeedValue := (SSArray[J0] and $FFFF0000) or (SSArray[J1] shr 16);
			SSArray[J1] := I;
		end;

		Result := Byte(Floor(m_Scale2 * (SeedValue and $FFFF)));
	end;

	function Generator4: Byte;
	var
		I,
		J0, J1:		Cardinal;
	begin
		I := 22695461 * SeedValue + 3;

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			J0 := (SeedValue shr 24) and SSTableMask;	//J1 - 21

			SeedValue := (SSArray[J0] and $FFFF0000) or ((SSArray[J1] shr 16));
			SSArray[J0] := I;
		end;

		Result := Byte(Floor(m_Scale2 * (SeedValue and $FFFF)));
	end;

	function Generator5: Byte;
	var
		I,
		J0, J1:		Cardinal;
	begin
		I := SeedValue * (SeedValue + 3);

		if FillSSMode then
			SeedValue := I
		else
		begin
			J1 := (I shr 24) and SSTableMask;
			J0 := (SeedValue shr 24) and SSTableMask;
			SeedValue := (SSArray[J1] and $FFFF0000) or (SSArray[J0] shr 16);
			SSArray[J1] := I;
		end;

		Result := Byte(Floor(m_Scale2 * (SeedValue and $FFFF)));
	end;

begin
	Result := 0;
	Case Index of
	0:	Result := Generator0;
	1:	Result := Generator1;
	2:	Result := Generator2;
	3:	Result := Generator3;
	4:	Result := Generator4;
	5:	Result := Generator5;
	end;
end;

function DeCodeString(Val: String): String;
	function DeCodeChar(Val: Char): Char;
	var
		C:	Integer;
	begin
		C := Ord(Val);
		if C > Ord('9') then	C := C - Generator(GeneratorIndex) - Ord('A') + 10
		else					C := C - Generator(GeneratorIndex) - Ord('0');

		C := Round((C + m_CodeLen * Integer(C < 0)) * m_CodeScale);
		Result := Chr(IfThen(C > 0, C + MaxV - MinV * Integer(C < 11), Ord('-')))
	end;
var
	I, N:	Integer;
begin
	Result := '';
	N := Length(Val);

	for I := 1 to N do
		Result := Result + DeCodeChar(Val[I])
end;

procedure SetNewSeedL(initSeed: Cardinal);
var
	I:	Cardinal;
begin
	FillSSMode := True;
	SeedValue := initSeed;
	for I := 0 to 127 do
	begin
		Generator(GeneratorIndex);
		SSArray[I] := (SeedValue and $FFFFFFFE) or (I and 1);
	end;
	SeedValue := initSeed;
	FillSSMode := False;
end;

procedure SetNewSeedS(initString: String);
var
	D, I, L, Ch:	Cardinal;
	initValue:		Cardinal;
begin
	I := Ord(initString[1]);
	if I <= Ord('9') then		I := I - Ord('0')
	else						I := I - Ord('A') + 10;

	GeneratorIndex := I - (m_CodeLen shr 1);
	if GeneratorIndex < 0 then
		GeneratorIndex := GeneratorIndex + m_CodeLen;
	if GeneratorIndex >= m_NGnerators then
		raise EInvalidCast.Create('Unsupported format string reached.');

	L := Length(initString);
	initValue := 0;
	for I := 2 to L do
	begin
		Ch := Ord(initString[I]);
		if Ch <= Ord('9') then		D := Ch - Ord('0')
		else						D := Ch - Ord('A') + 10;

		initValue := initValue * m_CodeLen + D
	end;

	SetNewSeedL(initValue)
end;

procedure InitModule;
begin
	SeedValue := $FFFFFFFF;
	GeneratorIndex := m_NGnerators;
	MaxV := 9 + Ord('A');
	MinV := MaxV - Ord('0') + 1;
end;

const
	C: Array[0..m_CodeLen-1] of char =('A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Q', 'W',
							'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'Z', 'X', 'C',
							'V', 'B', 'N', 'M', '0', '1', '2', '3', '4', '5', '6',
							'7', '8', '9');

function LstStDtReader(LST_DT, key: String): String;
var
	ind, sDbl:		String;
	I, J, N:	Integer;
begin
	N := Length(key);
	sDbl := '';

	for I := 1 to N do
	begin
		for J := 0 to m_CodeLen-1 do
			if key[I] = C[J] then
			begin
				ind := IntToStr(J);
				if Length(ind) < 2 then ind := '0' + ind;
				sDbl := sDbl + ind;
				break;
			end;
	end;

	Result := IntToStr(StrToInt64(LST_DT) - StrToInt64(sDbl));

	while Length(Result)<6 do
		Result := '0' + Result;
end;

function LstStDtWriter(ModuleName: String): Integer;
			(**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][LastStart][CRC]
			 * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
			 * PrgNL -кол-во букв в названии программы (3 позиции)
			 * PRG_NAME - название программы
			 *R - код страны (3 позиции)
			 FC - кол-во фигур (3 позиции)
			 СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
			 PC - кол-во точек фигуры (6 позиций)
			 P - точки (формат ZAAA.BBB)
					P.X (7 позиций формат ZAAA.BBB)
					P.Y (7 позиций формат ZAAA.BBB)
					P.R (8 позиций формат AAAAAA.BB)
			 K - ключ кодировки (5 позиций)
			 LastStart ЗАКОДИРОВАННАЯ дата последнего запуска программы 10 позиций (после раскодирования формат DDMMYY)
			 CRC - контрольная последовательность (8 позиций)
			**)
var
	LastStart, CRCCode,
	mesNow, yrNow, LCode,
	key, dNow,
	ind, Res, str:		String;

	Reg:		TRegistry;
	I,  J:		Integer;
	CurrDate:	TDateTime;
	Year, Month,
	Day:		Word;
begin

	LCode := '';
	Reg := TRegistry.Create(KEY_READ);
	try
		Reg.RootKey := HKEY_CURRENT_USER;
		Reg.OpenKey(PandaRegKeyPath + '\' + ModuleName, False);
		LCode := Reg.ReadString(KeyName);
	finally
		Reg.CloseKey;
		Reg.Free;
	end;

	if LCode = '' then
	begin
		Result := -1;
		exit
	end;

	CRCCode := Copy(LCode, Length(LCode) - 7, 8);	//	получили CRC код
	LCode := Copy(LCode, 1, Length(LCode) - 8);

	LastStart := Copy(LCode, Length(LCode) - 9, 10);	//	получили дату последнего запуска
	LCode := Copy(LCode, 1, Length(LCode) - 10);

	if CRCCode <> CalcCRC32(LCode) then		//	проверили его
	begin
		Result := -1;
		exit
	end;

	str := LCode;

	key := Copy(LCode, Length(LCode) - 4, 5);		//	получили ключ
	LCode := Copy(LCode, 1, Length(LCode) - 5);

	SetNewSeedS(key);

	LCode := DeCodeString(LCode);

	// сформируем новую дату "последнего запуска"

	for I := 1 to Length(key) do
		for J := 0 to m_CodeLen-1 do
			if key[I] = C[J] then
			begin
				ind := IntToStr(J);
				if Length(ind) < 2 then ind := '0' + ind;
				Res := Res + ind;
				break;
			end;

	CurrDate := Date;
	DecodeDate(CurrDate, Year, Month, Day);
	dNow := IntToStr(Day);
	mesNow := IntToStr(Month);
	yrNow := IntToStr(Year - 2000);

	If Length(dNow) < 2 then	dNow	:= '0' + dNow;
	If Length(mesNow) < 2 then	mesNow	:= '0' + mesNow;
	If Length(yrNow) < 2 then	yrNow	:= '0' + yrNow;

	LastStart := dNow + mesNow + yrNow;
	LastStart := IntToStr(StrToInt64(LastStart) + StrToInt64(Res));

	while Length(LastStart) < 10 do
		LastStart := '0' + LastStart;

	LCode := str;
	CRCCode := CalcCRC32(str);

	LCode := LCode + LastStart + CRCCode;

	Reg := TRegistry.Create((STANDARD_RIGHTS_WRITE or
							KEY_SET_VALUE) and not SYNCHRONIZE);
	result := 0;

	try
		try
			Reg.RootKey := HKEY_CURRENT_USER;
			Reg.OpenKey(PandaRegKeyPath + '\' + ModuleName, False);
			Reg.WriteString(KeyName, LCode);
		except
			result := -1;
		end;
	finally
		Reg.CloseKey;
		Reg.Free;
	end;
end;

function DecodeLCode(LCode, ModuleName: String):	TPolygon;
			(**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][CRC]
			 * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
			 * PrgNL -кол-во букв в названии программы (3 позиции)
			 * PRG_NAME - название программы
			 *R - код страны (3 позиции)
			 FC - кол-во фигур (3 позиции)
			 СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
			 PC - кол-во точек фигуры (6 позиций)
			 P - точки (формат ZAAA.BBB)
					P.X (7 позиций формат ZAAA.BBB)
					P.Y (7 позиций формат ZAAA.BBB)
					P.R (8 позиций формат AAAAAA.BB)
			 K - ключ кодировки (5 позиций)
			 CRC - контрольная последовательность (8 позиций)
			**)
	procedure CreatePrjCircle(PtCnt: TPoint; Radius: Double; Ring: TRing);
	var
		I:			LongInt;
		iInRad:		Double;
		pt:			TPoint;
	begin
		pt := TPoint.Create;

		for I := 0 to 359 do
		begin
			iInRad := DegToRad(I);
			pt.X := PtCnt.X + Radius * Cos(iInRad);
			pt.Y := PtCnt.Y + Radius * Sin(iInRad);
			Ring.AddPoint(pt);
		end;
		pt.Free;
	end;

var
	CountryCode,
	PointsCount,
	LastStart,
	CRCCode,
	PRG_NAME,
	FigCount,
	FigCode,
	Drobnoe, Celoe,
	tempS, Tarix,
	PrgNL, key,
	X, Y, R:	String;

	yearKey,
	mesKey,
	dKey:		String;

	Year,
	mesNow,
	yrNow,
	dNow:		WORD;

	I, J, K, M:	Integer;

	TL1, TL2, TL,
	Xc,
	Yc, Rc:		Double;
	fCeloe,
	fDrobnoe:	Double;
	CurrDate:	TDateTime;

	Pol:		TRing;
	P, Pprj:	TPoint;

//	T:		TGeometryOperators;
begin
	Result := TPolygon.Create;

	if Length(LCode) < 1 then
		exit;

	CRCCode := Copy(LCode, Length(LCode) - 7, 8);			//	получили CRC код
	LCode := Copy(LCode, 1, Length(LCode) - 8);

	LastStart := Copy(LCode, Length(LCode) - 9, 10);		//	получили дату последнего запуска
	LCode := Copy(LCode, 1, Length(LCode) - 10);

	if CRCCode <> CalcCRC32(LCode) then						//	проверили его
		exit;

	key := Copy(LCode, Length(LCode) - 4, 5);				//	получили ключ
	LCode := Copy(LCode, 1, Length(LCode) - 5);

	InitModule;
	SetNewSeedS(key);
	LCode := DeCodeString(LCode);

//==================================================================================================
	Tarix := Copy(LCode, 1, 8);								//	получили срок "годности"
	LCode := Copy(LCode, 9, Length(LCode));

	dKey := Copy(Tarix, 1, 2);
	mesKey := Copy(Tarix, 3, 2);
	yearKey := Copy(Tarix, 5, 4);
	TL := StrToInt(dKey) + StrToInt(mesKey) * 30.4375 + (StrToInt(yearKey) - 1899) * 365.25;
//==================================================================================================
	LastStart := LstStDtReader(LastStart, key);

	dKey := Copy(LastStart, 1, 2);
	mesKey := Copy(LastStart, 3, 2);
	yearKey := Copy(LastStart, 5, 4);
	TL1 := StrToInt(dKey) + StrToInt(mesKey) * 30.4375 + (StrToInt(yearKey) + (2000 - 1899)) * 365.25;

	CurrDate := Date;
	DecodeDate(CurrDate, Year, mesNow, dNow);
	yrNow := Year - 1899;

	TL2 := (dNow + mesNow * 30.4375 + yrNow * 365.25);

	TL1 := IfThen(TL1 < TL2, TL1, TL2);
//==================================================================================================
// сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
	if TL < TL1 then
		exit;
//==================================================================================================

	PrgNL := Copy(LCode, 1, 3);							//	получили длину названия модуля
	LCode := Copy(LCode, 4, Length(LCode));

	J := 1;
	tempS := Copy(LCode, 1, StrToInt(PrgNL) * 3);		//	получили название модуля в символьном виде
	for I := 1 to StrToInt(PrgNL) do					//	теперь сконвертируем его в строку
	begin
		PRG_NAME := PRG_NAME + Chr(StrToInt(Copy(tempS, J, 3)));
		Inc(J, 3)
	end;

	if UpperCase(PRG_NAME) <> UpperCase(ModuleName) then
		exit;

	LCode := Copy(LCode, StrToInt(PrgNL) * 3 + 1, Length(LCode));

	CountryCode := Copy(LCode, 1, 3);					//	получили код страны
	LCode := Copy(LCode, 4, Length(LCode));
	FigCount := Copy(LCode, 1, 3);						//	получили число фигур
	LCode := Copy(LCode, 4, Length(LCode));

	P := TPoint.Create;
	Pol := TRing.Create;

	I := 1;
	for K := 0 to StrToInt(FigCount) - 1 do
	begin
		FigCode := Copy(LCode, 1, 1);					//	получили код фигуры
		Inc(I);
		Pol.Clear;

		if FigCode <> '1' then
		begin
			PointsCount := Copy(LCode, I, 6);			//	получили число точек для данной фигуры
			Inc(I, 6);

			for M := 0 to StrToInt(PointsCount) - 1 do
			begin
				X := Copy(LCode, I, 7);
				Y := Copy(LCode, I + 7, 7);
				Inc(I, 14);								//	7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

				Celoe := Copy(X, 1, 4);
				Drobnoe := Copy(X, 5, 3);
				fCeloe := StrToFloat(Celoe);
				fDrobnoe := StrToFloat(Drobnoe);
				Xc := fCeloe + 0.001 * IfThen(fCeloe < 0, -fDrobnoe, fDrobnoe);		//	получили координату X

				Celoe := Copy(Y, 1, 4);
				Drobnoe := Copy(Y, 5, 3);
				fCeloe := StrToFloat(Celoe);
				fDrobnoe := StrToFloat(Drobnoe);
				Yc := fCeloe + 0.001 * IfThen(fCeloe < 0, -fDrobnoe, fDrobnoe);		//	получили координату Y

				P.X := Xc;		P.Y := Yc;

				Pprj := GeoToPrj(P).AsPoint;										//	спроецировали точки
				if not Pprj.IsEmpty then
					Pol.AddPoint(Pprj);

				Pprj.Free
			end
		end
		else
		begin
			X := Copy(LCode, I, 7);
			Y := Copy(LCode, I + 7, 7);
			Inc(I, 14);								//	7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

			R := Copy(LCode, I, 8);
			Inc(I, 8);								//	8 позиций R (AAAAAA.BB)

			Celoe := Copy(X, 1, 4);
			Drobnoe := Copy(X, 5, 3);
			fCeloe := StrToFloat(Celoe);
			fDrobnoe := StrToFloat(Drobnoe);
			Xc := fCeloe + 0.001 * IfThen(fCeloe < 0, -fDrobnoe, fDrobnoe);		//	получили координату X

			Celoe := Copy(Y, 1, 4);
			Drobnoe := Copy(Y, 5, 3);
			fCeloe := StrToFloat(Celoe);
			fDrobnoe := StrToFloat(Drobnoe);
			Yc := fCeloe + 0.001 * IfThen(fCeloe < 0, -fDrobnoe, fDrobnoe);		//	получили координату Y

			P.X := Xc;		P.Y := Yc;

			Celoe := Copy(R, 1, 6);
			Drobnoe := Copy(R, 7, 2);
			Rc := StrToFloat(Celoe) * 1000.0 + StrToFloat(Drobnoe);				//	получили значение радиуса если фигура круг

			Pprj := GeoToPrj(P).AsPoint;										//	спроецировали точки
			if not Pprj.IsEmpty then
				CreatePrjCircle(Pprj, Rc, Pol);
			Pprj.Free;
		end;

		if Pol.Count > 2 then
			Result.AddRing(Pol);
	end;

	P.Free;
	Pol.Free;

//	GUI.DrawPolygon(Result, 255, sfsDiagonalCross);
end;

end.
