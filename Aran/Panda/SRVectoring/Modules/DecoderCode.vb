#Const OldGen = False

Option Strict Off
Option Explicit On

Imports Microsoft.Win32
Imports VB = Microsoft.VisualBasic
Imports ESRI.ArcGIS.Geometry

Module DecoderCode
	Private Const c_NGnerators As UInteger = 6
	Private Const c_CodeLen As UInteger = 36
	Private Const c_InputRange As UInteger = 11

	Private Const c_CodeScale As Double = 0.305555555555556	'm_InputRange / m_CodeLen
	Private Const c_Scale1 As Double = 36.0 / 4294967296.0
	Private Const c_Scale2 As Double = 36.0 / 65536.0

	Private Const c_SSTableSize As UInteger = 128
	Private Const c_SSTableMask As UInteger = c_SSTableSize - 1
	Private Const c_SSTableBits As UInteger = 7

	Private s_MinV As Integer
	Private s_MaxV As Integer

	Private s_SeedValue As UInteger
	Private s_GneratorIndex As UInteger
	Private s_SSArray(127) As UInteger
	Private s_bFillSS As Boolean

	Private Function DeCodeString(ByVal strVal As String) As String
		Dim I As Integer
		Dim N As Integer
		Dim RetVal As String

		RetVal = ""
		N = Len(strVal)

		For I = 1 To N
			RetVal = RetVal + DeCodeChar(Mid(strVal, I, 1))
		Next
		Return RetVal
	End Function

	Private Function DeCodeChar(ByVal strVal As Char) As Char
		Dim C As Integer
		C = Asc(strVal)
		If C > Asc("9") Then
			C = C + 10 - Generator(s_GneratorIndex) - Asc("A")
		Else
			C = C - Generator(s_GneratorIndex) - Asc("0")
		End If

		C = System.Math.Round((C - c_CodeLen * CShort(C < 0)) * c_CodeScale)
		Return IIf(C > 0, Chr(C + s_MaxV + s_MinV * CShort(C < 11)), "-")
	End Function

	Private Sub SetNewSeedL(ByVal initSeed As UInteger)
		Dim I As Integer

		s_bFillSS = True
		s_SeedValue = initSeed
		For I = 0 To 127
			Generator(s_GneratorIndex)
			s_SSArray(I) = (s_SeedValue And &HFFFFFFFEUI) Or (I And 1)
		Next
		s_bFillSS = False
		s_SeedValue = initSeed
	End Sub

	Private Sub SetNewSeedS(ByVal initString As String)
		Dim D As Integer
		Dim I As Integer
		Dim L As Integer
		Dim initValue As UInteger
		Dim Ch As Char

		Ch = Mid(initString, 1, 1)
		If Ch <= "9" Then
			I = Asc(Ch) - Asc("0")
		Else
			I = Asc(Ch) + 10 - Asc("A")
		End If

		s_GneratorIndex = I - c_CodeLen \ 2
		If s_GneratorIndex < 0 Then s_GneratorIndex = s_GneratorIndex + c_CodeLen
		If s_GneratorIndex >= c_NGnerators Then Err.Raise(1000, "", "Unsupported format string reached.")

		initValue = 0
		L = Len(initString)
		For I = 2 To L
			Ch = Mid(initString, I, 1)
			If Ch <= "9" Then
				D = Asc(Ch) - Asc("0")
			Else
				D = Asc(Ch) + 10 - Asc("A")
			End If
			initValue = initValue * c_CodeLen + D
		Next I
		SetNewSeedL(initValue)
	End Sub

#If OldGen Then
	Private Function Exp2(ByVal IntVal As Integer) As Double
		Dim I As Integer
		Dim RetVal As Double

		RetVal = 1.0
		For I = 0 To IntVal - 1
			RetVal = RetVal + RetVal
		Next
		Return RetVal
	End Function

	Private Function Unchecked(ByVal Val_Renamed As Double) As UInteger
		Dim I As Integer
		Dim N As Integer
		Dim fExp As Double

		If Val_Renamed < 2147483648.0 Then Return System.Math.Floor(Val_Renamed)

		N = System.Math.Round(System.Math.Log(Val_Renamed) * 1.44269504088896 - 0.49999)

		For I = N To 32 Step -1
			fExp = Exp2(I)
			If Val_Renamed >= fExp Then Val_Renamed = Val_Renamed - fExp
		Next I

		If Val_Renamed < 2147483648.0 Then Return System.Math.Floor(Val_Renamed)

		Val_Renamed = Val_Renamed - 2147483648.0
		Return System.Math.Floor(Val_Renamed) Or &H80000000UI
	End Function

	Private Function ShiftDWORD24(ByVal val_Renamed As UInteger) As UInteger
		Dim I As Integer
		Dim Mask As Integer
		Dim AddValue As Integer
		Dim RetVal As UInteger

		RetVal = 0
		If (val_Renamed And &H80000000UI) = &H80000000UI Then RetVal = 128

		Mask = &H40000000UI
		AddValue = 64
		For I = 0 To 6
			If (val_Renamed And Mask) <> 0 Then RetVal = RetVal + AddValue
			Mask = Mask \ 2
			AddValue = AddValue \ 2
		Next I

		Return RetVal
	End Function

	Private Function ShiftDWORD16(ByVal val_Renamed As UInteger) As UInteger
		Dim I As Integer
		Dim Mask As UInteger
		Dim AddValue As UInteger
		Dim RetVal As UInteger

		RetVal = 0
		If (val_Renamed And &H80000000UI) = &H80000000UI Then RetVal = 32768

		Mask = &H40000000UI
		AddValue = 16384
		For I = 0 To 14
			If (val_Renamed And Mask) = Mask Then RetVal = RetVal + AddValue
			Mask = Mask \ 2
			AddValue = AddValue \ 2
		Next I

		Return RetVal
	End Function

	'Private Function DWORD2Double(ByVal Val_Renamed As Integer) As Double
	'	If Val_Renamed < 0 Then Return Val_Renamed + 4294967296.0
	'	Return Val_Renamed
	'End Function

	'Private Function DWORD2Single(ByVal Val As Long) As Single
	'	If Val < 0 Then Return Val + 4294967296.0
	'	Return Val
	'End Function

	Private Function Generator0() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim K As UInteger
		Dim J0 As UInteger
		Dim J1 As UInteger
		fTmp = CDbl(s_SeedValue) * 22695477.0 + 37.0

		If s_bFillSS Then
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			K = 0
			Do While (s_SSArray(J1) = I) And (K < 128)
				K = K + 1
				J1 = (J1 + 1) And 127
			Loop

			If s_SSArray(J1) = I Then s_SSArray(J1) = Unchecked(I + 22695477.0)

			J0 = J1 - 23
			If J0 < 0 Then J0 = J0 + 128
			s_SeedValue = (s_SSArray(J0) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J1)))
			s_SSArray(J1) = I
		End If

		Return CByte(System.Math.Floor(s_SeedValue * c_Scale1))
	End Function

	Private Function Generator1() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim J0 As UInteger
		Dim J1 As UInteger

		'fTmp = (0.5 * DWORD2Double(SeedValue) + 1.4142135623731) * 2.23606797749979
		fTmp = CDbl(s_SeedValue) * 22695461.0 + 3.14159265358979

		If s_bFillSS Then
			'SeedValue = Unchecked((fTmp - Int(fTmp)) * 4706870449.79926)
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			J0 = ShiftDWORD24(s_SeedValue) And 127 'J1 - 21

			s_SeedValue = (s_SSArray(J0) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J1)))
			s_SSArray(J0) = I
		End If

		Return CByte(System.Math.Floor(CDbl(s_SeedValue) * c_Scale1))
	End Function

	Private Function Generator2() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim J0 As UInteger
		Dim J1 As UInteger

		'    fTmp = DWORD2Double(SeedValue) * 0.318309886183791 + 3.14159265358979
		'    SeedValue = Unchecked((fTmp - Int(fTmp)) * 4896968389.19523)
		'    Generator2 = CByte(Int(ShiftDWORD24(SeedValue) * m_RangeScale))
		fTmp = CDbl(s_SeedValue)
		fTmp = fTmp * (fTmp + 3.0) '+ 3.4142135623731
		If s_bFillSS Then
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			J0 = ShiftDWORD24(s_SeedValue) And 127
			s_SeedValue = (s_SSArray(J1) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J0)))
			s_SSArray(J1) = I
		End If

		Return CByte(System.Math.Floor(CDbl(s_SeedValue) * c_Scale1))
	End Function

	Private Function Generator3() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim K As UInteger
		Dim J0 As Integer
		Dim J1 As UInteger
		fTmp = CDbl(s_SeedValue) * 22695477.0 + 37.0

		If s_bFillSS Then
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			K = 0
			Do While (s_SSArray(J1) = I) And (K < 128)
				K = K + 1
				J1 = (J1 + 63) And 127
			Loop
			If s_SSArray(J1) = I Then s_SSArray(J1) = Unchecked(I + 22695477.0)

			J0 = CLng(J1) - 23
			If J0 < 0 Then J0 = J0 + 128
			s_SeedValue = (s_SSArray(J0) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J1)))
			s_SSArray(J1) = I
		End If

		Return CByte(System.Math.Floor((s_SeedValue And 65535) * c_Scale2))
	End Function

	Private Function Generator4() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim J0 As UInteger
		Dim J1 As UInteger
		fTmp = CDbl(s_SeedValue) * 22695461.0 + 3.14159265358979

		If s_bFillSS Then
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			J0 = ShiftDWORD24(s_SeedValue) And 127 'J1 - 21

			s_SeedValue = (s_SSArray(J0) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J1)))
			s_SSArray(J0) = I
		End If

		Return CByte(System.Math.Floor((s_SeedValue And 65535) * c_Scale2))
	End Function

	Private Function Generator5() As Byte
		Dim fTmp As Double
		Dim I As UInteger
		Dim J0 As UInteger
		Dim J1 As UInteger

		fTmp = CDbl(s_SeedValue)
		fTmp = fTmp * (fTmp + 3.0)
		If s_bFillSS Then
			s_SeedValue = Unchecked(fTmp)
		Else
			I = Unchecked(fTmp)
			J1 = ShiftDWORD24(I) And 127
			J0 = ShiftDWORD24(s_SeedValue) And 127
			s_SeedValue = (s_SSArray(J1) And &HFFFF0000UI) Or (ShiftDWORD16(s_SSArray(J0)))
			s_SSArray(J1) = I
		End If

		Return CByte(System.Math.Floor((s_SeedValue And 65535) * c_Scale2))
	End Function
#Else
	Private Function Generator0() As Byte
		Dim i, k, j1 As UInteger
		Dim j0 As Integer

		i = 22695477 * s_SeedValue + 37

		If s_bFillSS Then
			s_SeedValue = i
		Else
			j1 = (i >> 24) And c_SSTableMask
			k = 0

			While ((s_SSArray(j1) = i) And (k < 128))
				j1 = (j1 + 1) And c_SSTableMask
				k += 1
			End While

			If s_SSArray(j1) = i Then s_SSArray(j1) = 22695477 + i

			j0 = CLng(j1) - 23
			If (j0 < 0) Then j0 += c_SSTableSize

			s_SeedValue = (s_SSArray(j0) And &HFFFF0000UI) Or (s_SSArray(j1) >> 16)
			s_SSArray(j1) = i
		End If

		Return CByte(Math.Floor(c_Scale1 * s_SeedValue))
	End Function

	Private Function Generator1() As Byte
		Dim i, j0, j1 As UInteger

		i = 22695461 * s_SeedValue + 3

		If s_bFillSS Then
			s_SeedValue = i
		Else

			j1 = (i >> 24) And c_SSTableMask
			j0 = (s_SeedValue >> 24) And c_SSTableMask

			s_SeedValue = (s_SSArray(j0) And &HFFFF0000UI) Or (s_SSArray(j1) >> 16)
			s_SSArray(j0) = i
		End If

		Return CByte(Math.Floor(c_Scale1 * s_SeedValue))
	End Function

	Private Function Generator2() As Byte
		Dim i, j0, j1 As UInteger

		i = s_SeedValue * (s_SeedValue + 3)

		If s_bFillSS Then
			s_SeedValue = i
		Else

			j1 = (i >> 24) And c_SSTableMask
			j0 = (s_SeedValue >> 24) And c_SSTableMask
			s_SeedValue = (s_SSArray(j1) And &HFFFF0000UI) Or (s_SSArray(j0) >> 16)
			s_SSArray(j1) = i
		End If

		Return CByte(Math.Floor(c_Scale1 * s_SeedValue))
	End Function

	Private Function Generator3() As Byte
		Dim i, k, j1 As UInteger
		Dim j0 As Integer

		i = 22695477 * s_SeedValue + 37

		If s_bFillSS Then
			s_SeedValue = i
		Else

			j1 = (i >> 24) And c_SSTableMask
			k = 0
			While (s_SSArray(j1) = i) And (k < 128)
				j1 = (j1 + 63) And c_SSTableMask
				k += 1
			End While

			If (s_SSArray(j1) = i) Then
				s_SSArray(j1) = 22695477 + i
			End If

			j0 = CLng(j1) - 23
			If (j0 < 0) Then
				j0 += c_SSTableSize
			End If

			s_SeedValue = (s_SSArray(j0) And &HFFFF0000UI) Or (s_SSArray(j1) >> 16)
			s_SSArray(j1) = i
		End If

		Return CByte(Math.Floor(c_Scale2 * (s_SeedValue And &HFFFF)))
	End Function

	Private Function Generator4() As Byte
		Dim i, j0, j1 As UInteger

		i = 22695461 * s_SeedValue + 3

		If s_bFillSS Then
			s_SeedValue = i
		Else

			j1 = (i >> 24) And c_SSTableMask
			j0 = (s_SeedValue >> 24) And c_SSTableMask	'J1 - 21

			s_SeedValue = (s_SSArray(j0) And &HFFFF0000UI) Or ((s_SSArray(j1) >> 16))
			s_SSArray(j0) = i
		End If

		Return CByte(Math.Floor(c_Scale2 * (s_SeedValue And &HFFFF)))
	End Function

	Private Function Generator5() As Byte
		Dim i, j0, j1 As UInteger


		i = s_SeedValue * (s_SeedValue + 3)

		If s_bFillSS Then
			s_SeedValue = i
		Else
			j1 = (i >> 24) And c_SSTableMask
			j0 = (s_SeedValue >> 24) And c_SSTableMask
			s_SeedValue = (s_SSArray(j1) And &HFFFF0000UI) Or (s_SSArray(j0) >> 16)
			s_SSArray(j1) = i
		End If

		Return CByte(Math.Floor(c_Scale2 * (s_SeedValue And &HFFFF)))
	End Function
#End If

	Private Function Generator(ByVal Index As Integer) As Byte
		Select Case Index
			Case 0
				Return Generator0()
			Case 1
				Return Generator1()
			Case 2
				Return Generator2()
			Case 3
				Return Generator3()
			Case 4
				Return Generator4()
			Case 5
				Return Generator5()
		End Select
		Return 0
	End Function

	Private Sub InitModule()
		s_SeedValue = &HFFFFFFFFUI
		s_GneratorIndex = c_NGnerators
		s_MaxV = 9 + Asc("A")
		s_MinV = s_MaxV - Asc("0") + 1
	End Sub

	Private Function LstStDtReader(ByVal LST_DT As String, ByVal key As String, Optional ByVal Multpl As Integer = -1) As String
		Dim intTab() As Integer = New Integer() _
		  {26, 27, 28, 29, 30, 31, 32, 33, 34, 35, _
		   0, 23, 21, 2, 11, 3, 4, 5, 16, 6, _
		   7, 8, 25, 24, 17, 18, 9, 12, 1, 13, _
		   15, 22, 10, 20, 14, 19}

		Dim Res As String = ""
		Dim I As Integer

		For I = 0 To Len(key) - 1
			Dim J As Integer

			If key(I) <= "9" Then
				J = Asc(key(I)) - Asc("0")
			Else
				J = Asc(key(I)) - Asc("A") + 10
			End If

			Dim ind As String = CStr(intTab(J))

			If intTab(J) < 10 Then
				ind = "0" + ind
			End If
			Res = Res + ind
		Next

		Dim retval As String

		retval = CStr(CDbl(LST_DT) + Multpl * CDbl(Res))
		While Len(retval) < 10
			retval = "0" + retval
		End While

		Return retval
	End Function

	Public Function LstStDtWriter(ByVal LCode As String, ByVal ModuleName As String) As Integer
		'            /**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][LastStart][CRC]
		'             * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
		'             * PrgNL -кол-во букв в названии программы (3 позиции)
		'             * PRG_NAME - название программы
		'             *R - код страны (3 позиции)
		'             FC - кол-во фигур (3 позиции)
		'             СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
		'             PC - кол-во точек фигуры (6 позиций)
		'             P - точки (формат ZAAA.BBB)
		'                    p.X (7 позиций формат ZAAA.BBB)
		'                    p.Y (7 позиций формат ZAAA.BBB)
		'                    p.R (8 позиций формат AAAAAA.BB)
		'             K - ключ кодировки (5 позиций)
		'             LastStart ЗАКОДИРОВАННАЯ дата последнего запуска программы 10 позиций (после раскодирования формат DDMMYY)
		'             CRC - контрольная последовательность (8 позиций)
		'             **/

		Dim LastStart As String
		Dim CRCCode As String
		Dim mesNow As String
		Dim yrNow As String
		Dim key As String
		Dim dNow As String

		If Len(LCode) <= 0 Then Return -1

		' получили CRC код
		CRCCode = Mid(LCode, Len(LCode) - 7, 8)
		LCode = Mid(LCode, 1, Len(LCode) - 8)

		'получили дату последнего запуска
		LastStart = Mid(LCode, Len(LCode) - 9, 10)
		LCode = Mid(LCode, 1, Len(LCode) - 10)

		' проверили CRC код
		If CRCCode <> CalcCRC32(LCode) Then Return -1

		'получили ключ
		key = Mid(LCode, Len(LCode) - 4, 5)

		' сформируем новую дату "последнего запуска"
		dNow = CStr(VB.Day(Now))
		mesNow = CStr(Month(Now))
		yrNow = CStr(Year(Now) - 2000)

		If Len(dNow) < 2 Then dNow = "0" + dNow
		If Len(mesNow) < 2 Then mesNow = "0" + mesNow
		If Len(yrNow) < 2 Then yrNow = "0" + yrNow

		LastStart = LstStDtReader(dNow + mesNow + yrNow, key, 1)

		CRCCode = CalcCRC32(LCode)
		LCode = LCode + LastStart + CRCCode

        Common.RegFuncs.WriteConfig(ModuleCategory + "\" + ModuleName + "\" + LicenseKeyName, LCode)
        Return 0
	End Function

	Public Function DecodeLCode(ByVal CodeString As String, ByVal ModuleName As String) As ESRI.ArcGIS.Geometry.Polygon
		'            /**[Tarix][PrgNL][PRG_NAME] [R][FC][CF][PC][P1]...[Pn][K][CRC]
		'             * Tarix - "срок годности" (8 позиций формат DDMMYYYY)
		'             * PrgNL -кол-во букв в названии программы (3 позиции)
		'             * PRG_NAME - название программы
		'             *R - код страны (3 позиции)
		'             FC - кол-во фигур (3 позиции)
		'             СF - код фигуры ( 1-окружность 0-полигон) (1 позиция)
		'             PC - кол-во точек фигуры (6 позиций)
		'             P - точки (формат ZAAA.BBB)
		'                    p.X (7 позиций формат ZAAA.BBB)
		'                    p.Y (7 позиций формат ZAAA.BBB)
		'                    p.R (8 позиций формат AAAAAA.BB)
		'             K - ключ кодировки (5 позиций)
		'             CRC - контрольная последовательность (8 позиций)
		'             **/

		'Dim CountryCode As String
		Dim LastStart As String
		Dim CRCCode As String
		Dim PRG_NAME As String
		Dim FigCode As String
		Dim Drobnoe As String
		Dim Celoe As String
		Dim tempS As String
		Dim Tarix As String
		Dim key As String
		Dim X As String
		Dim Y As String
		Dim R As String

		Dim yearKey As String
		Dim mesKey As String
		Dim dKey As String

		Dim FigCount As Integer
		Dim PointsCount As Integer
		Dim mesNow As Integer
		Dim yrNow As Integer
		Dim dNow As Integer
		Dim PrgNL As Integer
		Dim K As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer

		Dim TL1 As Double
		Dim TL2 As Double
		Dim TL As Double

		Dim Xc As Double
		Dim Yc As Double
		Dim Rc As Double

		Dim fDrobnoe As Double
		Dim fCeloe As Double

		Dim T As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim Pol As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGeometry As IGeometry
		Dim pResult As ESRI.ArcGIS.Geometry.IPolygon
		Dim p As ESRI.ArcGIS.Geometry.IPoint
		Dim LCode As String

		pResult = New ESRI.ArcGIS.Geometry.Polygon

		If Len(CodeString) <= 0 Then Return pResult

		CRCCode = Mid(CodeString, Len(CodeString) - 7, 8)	' получили CRC код
		LCode = Mid(CodeString, 1, Len(CodeString) - 8)

		LastStart = Mid(LCode, Len(LCode) - 9, 10) 'получили дату последнего запуска
		LCode = Mid(LCode, 1, Len(LCode) - 10)

		If CRCCode <> CalcCRC32(LCode) Then Return pResult ' проверили его

		key = Mid(LCode, Len(LCode) - 4, 5)	'получили ключ
		LCode = Mid(LCode, 1, Len(LCode) - 5)

		InitModule()
		SetNewSeedS(key)
		LCode = DeCodeString(LCode)

		'==================================================================================================
		Tarix = Mid(LCode, 1, 8) ' получили срок "годности"
		LCode = Mid(LCode, 9, Len(LCode))

		dKey = Mid(Tarix, 1, 2)
		mesKey = Mid(Tarix, 3, 2)
		yearKey = Mid(Tarix, 5, 4)
		TL = CInt(dKey) + CInt(mesKey) * 30.4375 + (CInt(yearKey) - 1899) * 365.25
		'==================================================================================================
		LastStart = LstStDtReader(LastStart, key)

		dKey = Mid(LastStart, 5, 2)
		mesKey = Mid(LastStart, 7, 2)
		yearKey = Mid(LastStart, 9, 4)
		TL1 = CInt(dKey) + CInt(mesKey) * 30.4375 + (CInt(yearKey) + (2000 - 1899)) * 365.25

		dNow = VB.Day(Now)
		mesNow = Month(Now)
		yrNow = Year(Now) - 1899
		TL2 = (dNow + mesNow * 30.4375 + yrNow * 365.25)

		TL1 = IIf(TL1 > TL2, TL1, TL2)
		'==================================================================================================
		' сравним дату последнего запуска LastStart и текущую. если Текущая МЕНЬШЕ даты LastStart - вылетаем
		If TL < TL1 Then Return pResult
		'==================================================================================================

		PrgNL = CInt(Mid(LCode, 1, 3)) ' получили длину названия модуля
		LCode = Mid(LCode, 4, Len(LCode))

		J = 1
		PRG_NAME = ""
		tempS = Mid(LCode, 1, PrgNL * 3) 'получили название модуля в символьном виде
		For I = 1 To PrgNL Step 1	' теперь сконвертируем его в строку
			PRG_NAME = PRG_NAME + Chr(CInt(Mid(tempS, J, 3)))
			J = J + 3
		Next I

		If UCase(PRG_NAME) <> UCase(ModuleName) Then Return pResult

		LCode = Mid(LCode, PrgNL * 3 + 1, Len(LCode))

		'CountryCode = Mid(LCode, 1, 3) ' получили код страны
		LCode = Mid(LCode, 4, Len(LCode))
		FigCount = CInt(Mid(LCode, 1, 3))	' получили число фигур
		LCode = Mid(LCode, 4, Len(LCode))

		p = New ESRI.ArcGIS.Geometry.Point
		I = 1
		For K = 0 To FigCount - 1
			FigCode = Mid(LCode, I, 1)	' получили код фигуры
			I = I + 1

			If FigCode <> "1" Then
				Pol = New ESRI.ArcGIS.Geometry.Polygon
				PointsCount = CInt(Mid(LCode, I, 6)) ' получили число точек для данной фигуры
				I = I + 6

				For M = 0 To PointsCount - 1
					X = Mid(LCode, I, 7)
					Y = Mid(LCode, I + 7, 7)
					I = I + 14 '7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

					Celoe = Mid(X, 1, 4)
					Drobnoe = Mid(X, 5, 3)
					fCeloe = CDbl(Celoe)
					fDrobnoe = CDbl(Drobnoe)
					Xc = fCeloe + 0.001 * IIf(fCeloe < 0.0, -fDrobnoe, fDrobnoe) ' получили координату X

					Celoe = Mid(Y, 1, 4)
					Drobnoe = Mid(Y, 5, 3)
					fCeloe = CDbl(Celoe)
					fDrobnoe = CDbl(Drobnoe)
					Yc = fCeloe + 0.001 * IIf(fCeloe < 0.0, -fDrobnoe, fDrobnoe) ' получили координату Y

					p.PutCoords(Xc, Yc)
					p = ToPrj(p) 'спроецировали точки
					'GeographicToProjection Xc, Yc, XX, YY
					'p.PutCoords XX, YY
					If Not p.IsEmpty Then Pol.AddPoint(p)
				Next M
			Else
				X = Mid(LCode, I, 7)
				Y = Mid(LCode, I + 7, 7)
				I = I + 14		'7 позиций X (ZAAA.BBB)+ 7 позиций Y (ZAAA.BBB)

				R = Mid(LCode, I, 8)
				I = I + 8		'8 позиций R (AAAAAA.BB)

				Celoe = Mid(X, 1, 4)
				Drobnoe = Mid(X, 5, 3)
				fCeloe = CDbl(Celoe)
				fDrobnoe = CDbl(Drobnoe)
				Xc = fCeloe + 0.001 * IIf(fCeloe < 0.0, -fDrobnoe, fDrobnoe) ' получили координату X

				Celoe = Mid(Y, 1, 4)
				Drobnoe = Mid(Y, 5, 3)
				fCeloe = CDbl(Celoe)
				fDrobnoe = CDbl(Drobnoe)
				Yc = fCeloe + 0.001 * IIf(fCeloe < 0.0, -fDrobnoe, fDrobnoe) ' получили координату Y

				'GeographicToProjection Xc, Yc, XX, YY
				'p.PutCoords XX, YY

				p.PutCoords(Xc, Yc)
				p = ToPrj(p) 'спроецировали точки
				If Not p.IsEmpty Then
					Celoe = Mid(R, 1, 6)
					Drobnoe = Mid(R, 7, 2)

					Rc = CDbl(Celoe) * 1000.0 + CDbl(Drobnoe) ' получили значение радиуса если фигура круг
					Pol = CreatePrjCircle(p, Rc)
				Else
					Pol = New ESRI.ArcGIS.Geometry.Polygon
				End If
			End If

			pGeometry = Pol
			If Not pGeometry.IsEmpty Then
				T = Pol
				T.IsKnownSimple_2 = False
				T.Simplify()
				pResult = T.Union(pResult)

				T = pResult
				T.IsKnownSimple_2 = False
				T.Simplify()
			End If
		Next K

		'DrawPolygon pResult, 255

		Return pResult
	End Function
End Module
