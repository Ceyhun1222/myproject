Option Strict Off
Option Explicit On



Module Categories_DATABase
	
	Structure CategoriesData
		Dim Name As String
		Dim Values() As Double
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double
		Dim E As Double
		Dim H As Double
		Dim Multiplier As Double
		Dim Unit As String
		Dim Comment As String
		Dim DefinedIn As String
		
		Public Sub Initialize()
			ReDim Values(5)
		End Sub
	End Structure
	
	'Public ErrorCode As Integer
	
	Public cVatMin As CategoriesData
	Public cVatMax As CategoriesData
	Public cViafMin As CategoriesData
	Public cViafMax As CategoriesData
	Public cViafStar As CategoriesData
	Public cVfafMin As CategoriesData
	Public cVfafMax As CategoriesData
	Public cVva As CategoriesData
	Public cVmaInter As CategoriesData
	Public cVmaFaf As CategoriesData
	Public carStraightSegmen As CategoriesData
	Public arObsClearance As CategoriesData
	Public arMinOCH As CategoriesData
	Public arMinVisibility As CategoriesData
	Public arFAMinOCH15 As CategoriesData
	Public arFAMinOCH30 As CategoriesData
	Public arMaxInterAngle As CategoriesData
	Public arT45_180 As CategoriesData
	Public arMaxOutBoundDes As CategoriesData
	Public arMaxInBoundDesc As CategoriesData
	Public arMinHV_FAS As CategoriesData
	Public arMaxHV_FAS As CategoriesData
	Public arFADescent_Max As CategoriesData
	Public arImHorSegLen As CategoriesData
	Public arMinISlen00_15p As CategoriesData
	Public arMinISlen16_30p As CategoriesData
	Public arMinISlen31_60p As CategoriesData
	Public arMinISlen61_90p As CategoriesData
	Public arSemiSpan As CategoriesData
	Public arVerticalSize As CategoriesData
	
	Private Data() As Byte
	Private Index As Integer
	
	Function InitModule() As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim DataSize As Integer

		Dim FileSign As String
		Dim FileName As String
		Dim FileNumber As Short

		Dim BoolField As Boolean
		Dim SmallIntField As Integer
		Dim intField As Integer
		Dim charField As String
		Dim floatField As Double
		Dim CurrencyField As Decimal
		Dim DateTimeField As Date
		Dim FieldType As Short
		Dim Records As Integer
		Dim Fields As Integer
		Dim FieldName As String

		Dim TmpData As CategoriesData

		On Error GoTo ErrorHandler
		'ErrorCode = 0
		
		FileName = ConstDir + "\categories.dat"

		FileNumber = FreeFile()
		FileOpen(FileNumber, FileName, OpenMode.Binary, OpenAccess.Read)
		FileSign = InputString(FileNumber, 20)
		FileClose(FileNumber)

		If FileSign <> "Anplan DATABASE file" Then
			'ErrorCode = -1
			Return False
		End If
		
		Data = My.Computer.FileSystem.ReadAllBytes(FileName)
		DataSize = Data.Length()
		Index = 20

		Data(DataSize - 1) = Data(DataSize - 1) Xor Asc("R")

		For I = DataSize - 2 To Index Step -1
			Data(I) = Data(I) Xor Data(I + 1)
		Next I
		
		GetIntData(Data, Index, SmallIntField, 2)
		Fields = SmallIntField
		GetIntData(Data, Index, SmallIntField, 2)
		Records = SmallIntField
		
		
		For I = 0 To Fields - 1
			GetIntData(Data, Index, SmallIntField, 2)
			GetStrData(Data, Index, charField, SmallIntField)
		Next I

		TmpData.Initialize()

		For I = 0 To Records - 1
			For J = 0 To Fields - 1
				GetIntData(Data, Index, SmallIntField, 2)
				GetStrData(Data, Index, FieldName, SmallIntField)
				GetIntData(Data, Index, SmallIntField, 2)
				FieldType = SmallIntField
				
				Select Case FieldType
					'Fixed character field
					'Wide string field
					'Text memo field
					'Formatted text memo field
					'Character or string field
					Case 1, 16, 18, 23, 24
						GetIntData(Data, Index, SmallIntField, 2)
						GetStrData(Data, Index, charField, SmallIntField)
						Select Case FieldName
							Case "COMMENT"
								TmpData.Comment = charField
							Case "DEFINED_IN"
								TmpData.DefinedIn = charField
							Case "NAME"
								TmpData.Name = charField
							Case "UNIT"
								TmpData.Unit = charField
						End Select
						'16-bit integer field
						'16-bit unsigned integer field
					Case 2, 4
						GetIntData(Data, Index, SmallIntField, 2)
						'Large integer field
						'32-bit integer field
					Case 3, 25
						GetIntData(Data, Index, intField, 4)
					Case 5 'Boolean field
						GetData(Data, Index, BoolField, 2)
					Case 6, 8 'Floating-point numeric field
						GetDoubleData(Data, Index, floatField, 8)
						If FieldName = "A" Then TmpData.A = floatField
						If FieldName = "B" Then TmpData.B = floatField
						If FieldName = "C" Then TmpData.C = floatField
						If FieldName = "D" Then TmpData.D = floatField
						If FieldName = "E" Then TmpData.E = floatField
						If FieldName = "H" Then TmpData.H = floatField
						If FieldName = "MULTIPLIER" Then TmpData.Multiplier = floatField
					Case 7 'Money field
						GetData(Data, Index, CurrencyField, 8)
						'Date field
						'Time field
						'Date and time field
					Case 9, 10, 11
						GetData(Data, Index, DateTimeField, 8)
				End Select
			Next J
			
			TmpData.A = TmpData.A * TmpData.Multiplier
			TmpData.B = TmpData.B * TmpData.Multiplier
			TmpData.C = TmpData.C * TmpData.Multiplier
			TmpData.D = TmpData.D * TmpData.Multiplier
			TmpData.E = TmpData.E * TmpData.Multiplier
            TmpData.H = TmpData.H * TmpData.Multiplier

            If (TmpData.Unit = "rad") Or (TmpData.Unit = "°") Then
                TmpData.A = Math.Round(RadToDeg(TmpData.A), 2)
                TmpData.B = Math.Round(RadToDeg(TmpData.B), 2)
                TmpData.C = Math.Round(RadToDeg(TmpData.C), 2)
                TmpData.D = Math.Round(RadToDeg(TmpData.D), 2)
                TmpData.E = Math.Round(RadToDeg(TmpData.E), 2)
                TmpData.H = Math.Round(RadToDeg(TmpData.H), 2)
            End If

            TmpData.Values(0) = TmpData.A
            TmpData.Values(1) = TmpData.B
            TmpData.Values(2) = TmpData.C
            TmpData.Values(3) = TmpData.D
            TmpData.Values(4) = TmpData.E
            TmpData.Values(5) = TmpData.H

            Select Case TmpData.Name
                Case "VatMin"
					cVatMin = TmpData
					cVatMin.Initialize()
					System.Array.Copy(TmpData.Values, cVatMin.Values, 6)
				Case "VatMax"
					cVatMax = TmpData
					cVatMax.Initialize()
					System.Array.Copy(TmpData.Values, cVatMax.Values, 6)
				Case "ViafMin"
					cViafMin = TmpData
					cViafMin.Initialize()
					System.Array.Copy(TmpData.Values, cViafMin.Values, 6)
                Case "ViafMax"
                    cViafMax = TmpData
                    cViafMax.Initialize()
                    System.Array.Copy(TmpData.Values, cViafMax.Values, 6)
                Case "Viaf*"
                    cViafStar = TmpData
                    cViafStar.Initialize()
                    System.Array.Copy(TmpData.Values, cViafStar.Values, 6)
                Case "VfafMin"
                    cVfafMin = TmpData
                    cVfafMin.Initialize()
                    System.Array.Copy(TmpData.Values, cVfafMin.Values, 6)
                Case "VfafMax"
                    cVfafMax = TmpData
                    cVfafMax.Initialize()
                    System.Array.Copy(TmpData.Values, cVfafMax.Values, 6)
                Case "Vva"
                    cVva = TmpData
                    cVva.Initialize()
                    System.Array.Copy(TmpData.Values, cVva.Values, 6)
                Case "VmaInter"
                    cVmaInter = TmpData
                    cVmaInter.Initialize()
                    System.Array.Copy(TmpData.Values, cVmaInter.Values, 6)
                Case "VmaFaf"
                    cVmaFaf = TmpData
                    cVmaFaf.Initialize()
                    System.Array.Copy(TmpData.Values, cVmaFaf.Values, 6)
                Case "arStraightSegmen"
                    carStraightSegmen = TmpData
                    carStraightSegmen.Initialize()
                    System.Array.Copy(TmpData.Values, carStraightSegmen.Values, 6)
                Case "arObsClearance"
                    arObsClearance = TmpData
                    arObsClearance.Initialize()
                    System.Array.Copy(TmpData.Values, arObsClearance.Values, 6)
                Case "arMinOCH"
                    arMinOCH = TmpData
                    arMinOCH.Initialize()
                    System.Array.Copy(TmpData.Values, arMinOCH.Values, 6)
                Case "arMinVisibility"
                    arMinVisibility = TmpData
                    arMinVisibility.Initialize()
                    System.Array.Copy(TmpData.Values, arMinVisibility.Values, 6)
                Case "arFAMinOCH 15°"
                    arFAMinOCH15 = TmpData
                    arFAMinOCH15.Initialize()
                    System.Array.Copy(TmpData.Values, arFAMinOCH15.Values, 6)
                Case "arFAMinOCH 30°"
                    arFAMinOCH30 = TmpData
                    arFAMinOCH30.Initialize()
                    System.Array.Copy(TmpData.Values, arFAMinOCH30.Values, 6)
                Case "arMaxInterAngle"
                    arMaxInterAngle = TmpData
                    arMaxInterAngle.Initialize()
                    System.Array.Copy(TmpData.Values, arMaxInterAngle.Values, 6)
                Case "arT45-180"
                    arT45_180 = TmpData
                    arT45_180.Initialize()
                    System.Array.Copy(TmpData.Values, arT45_180.Values, 6)
                Case "arMaxOutBoundDes"
                    arMaxOutBoundDes = TmpData
                    arMaxOutBoundDes.Initialize()
                    System.Array.Copy(TmpData.Values, arMaxOutBoundDes.Values, 6)
                Case "arMaxInBoundDesc"
                    arMaxInBoundDesc = TmpData
                    arMaxInBoundDesc.Initialize()
                    System.Array.Copy(TmpData.Values, arMaxInBoundDesc.Values, 6)
                Case "arMinHV_FAS"
                    arMinHV_FAS = TmpData
                    arMinHV_FAS.Initialize()
                    System.Array.Copy(TmpData.Values, arMinHV_FAS.Values, 6)
                Case "arMaxHV_FAS"
                    arMaxHV_FAS = TmpData
                    arMaxHV_FAS.Initialize()
                    System.Array.Copy(TmpData.Values, arMaxHV_FAS.Values, 6)
                Case "arFADescent_Max"
                    arFADescent_Max = TmpData
                    arFADescent_Max.Initialize()
                    System.Array.Copy(TmpData.Values, arFADescent_Max.Values, 6)
                Case "arImHorSegLen"
                    arImHorSegLen = TmpData
                    arImHorSegLen.Initialize()
                    System.Array.Copy(TmpData.Values, arImHorSegLen.Values, 6)
                Case "arMinISlen00_15p"
                    arMinISlen00_15p = TmpData
                    arMinISlen00_15p.Initialize()
                    System.Array.Copy(TmpData.Values, arMinISlen00_15p.Values, 6)
                Case "arMinISlen16_30p"
                    arMinISlen16_30p = TmpData
                    arMinISlen16_30p.Initialize()
                    System.Array.Copy(TmpData.Values, arMinISlen16_30p.Values, 6)
                Case "arMinISlen31_60p"
                    arMinISlen31_60p = TmpData
                    arMinISlen31_60p.Initialize()
                    System.Array.Copy(TmpData.Values, arMinISlen31_60p.Values, 6)
                Case "arMinISlen61_90p"
                    arMinISlen61_90p = TmpData
                    arMinISlen61_90p.Initialize()
                    System.Array.Copy(TmpData.Values, arMinISlen61_90p.Values, 6)
                Case "arSemiSpan"
                    arSemiSpan = TmpData
                    arSemiSpan.Initialize()
                    System.Array.Copy(TmpData.Values, arSemiSpan.Values, 6)
                Case "arVerticalSize"
                    arVerticalSize = TmpData
                    arVerticalSize.Initialize()
                    System.Array.Copy(TmpData.Values, arVerticalSize.Values, 6)
            End Select
        Next I

		On Error GoTo 0

		Return True

		'dpNGui_Ar1_Dist = dpNGui_Ar1
		'dpNGui_Ar1_Dist.Value = (dpNGui_Ar1.Value - dpH_abv_DER.Value) / dpPDG_Nom.Value
ErrorHandler: 
		FileClose(FileNumber)
		ErrorStr = Err.Number + "  " + Err.Description
		'ErrorCode = -2
		Return False
	End Function

End Module
