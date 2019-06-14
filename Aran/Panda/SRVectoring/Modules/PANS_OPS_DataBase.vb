Option Strict Off
Option Explicit On

Imports System.IO

Module PANS_OPS_DataBase

	Structure PansOpsData
		Dim Name As String
		Dim Value As Double
		Dim Multiplier As Double
		Dim Unit As String
		Dim Comment As String
		Dim DefinedIn As String
	End Structure

	'Public ErrorCode As Integer

	Public Const arHOASPlaneCat1 As Double = 300.0
	Public Const arHOASPlaneCat23 As Double = 150.0

	Public arBufferMSA As PansOpsData
	Public arMSARoundThreshold As PansOpsData
	Public arMinInterDist As PansOpsData
	Public arMinInterToler As PansOpsData
	Public arStrInAlignment As PansOpsData

	Public arMaxRangeFAS As PansOpsData
	Public arCirclAprShift As PansOpsData
	Public arHoldAreaEdge As PansOpsData
	Public arHoldingBuffer As PansOpsData
	Public arIASegmentMOC As PansOpsData
	Public arISegmentMOC As PansOpsData
	Public arFASeg_FAF_MOC As PansOpsData
	Public arFASegmentMOC As PansOpsData
	Public arSOCdelayTime As PansOpsData
	Public arNearTerrWindSp As PansOpsData
	Public arISAmax As PansOpsData
	Public arISAmin As PansOpsData
	Public arMA_InterMOC As PansOpsData
	Public arMA_FinalMOC As PansOpsData
	Public arMAS_Climb_Min As PansOpsData
	Public arMAS_Climb_Max As PansOpsData
	Public arMAS_Climb_Nom As PansOpsData
	Public arMATurnAlt As PansOpsData
	Public arT_TechToleranc As PansOpsData
	Public arTP_by_DME_div As PansOpsData
	Public arT_Gui_dist As PansOpsData
	Public arafTrn_OSplay As PansOpsData
	Public arafTrn_ISplay As PansOpsData
	Public arSecAreaCutAngl As PansOpsData
	Public arMATurnTrshAngl As PansOpsData
	Public arFAFLenght As PansOpsData
	Public arAbv_Treshold As PansOpsData
	Public arFADescent_Min As PansOpsData
	Public arFADescent_Nom As PansOpsData
	Public arImRange_Min As PansOpsData
	Public arImRange_Nom As PansOpsData
	Public arImRange_Max As PansOpsData
	Public arIFHalfWidth As PansOpsData
	Public arMinISlen00_90 As PansOpsData
	Public arMinISlen91_96 As PansOpsData
	Public arMinISlen97_02 As PansOpsData
	Public arMinISlen03_08 As PansOpsData
	Public arMinISlen09_14 As PansOpsData
	Public arMinISlen15_20 As PansOpsData
	Public arFAFTolerance As PansOpsData
	Public arIFTolerance As PansOpsData
	Public arImDescent_Max As PansOpsData
	Public arFAPMaxRange As PansOpsData
	Public arOverHeadToler As PansOpsData
	Public arTrackAccuracy As PansOpsData
	Public arPilotTolerance As PansOpsData
	Public arMA_SplayAngle As PansOpsData
	Public arOptimalFAFRang As PansOpsData

	Public arCurvatureCoeff As PansOpsData
	Public arFixMaxIgnorGrd As PansOpsData
	Public arMOCChangeDist As PansOpsData
	Public arAddMOCCoef As PansOpsData
	Public arFIX15PlaneRang As PansOpsData
	Public arFAFOptimalDist As PansOpsData
	Public arIAFMinDMERange As PansOpsData
	Public arIAFMaxTurnAngl As PansOpsData
	Public arIAFMinTurnAngl As PansOpsData
	Public arIAFMinGuadLen As PansOpsData
	Public arVisAverBank As PansOpsData
	Public EnRouteSplayAngl As PansOpsData
	Public arIADescent_Nom As PansOpsData
	Public arIADescent_Max As PansOpsData
	Public arITechTolerance As PansOpsData
	Public arIBankTolerance As PansOpsData
	Public arIPilotToleranc As PansOpsData
	Public arMSAMOC As PansOpsData

	Private Data() As Byte
	Private Index As Integer

	Function InitModule() As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim BoolField As Boolean
		Dim WordField As Short
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
		Dim TmpData As PansOpsData

		Dim FileSign As String
		Dim FileName As String
		Dim L As Integer
		Dim Pos As Integer
		Dim DataSize As Integer
		Dim FileNumber As Short

		On Error GoTo ErrorHandler

		InitModule = True
		'ErrorCode = 0

		FileName = ConstDir + "\ArPansops.dat"

		Dim b() As Byte
		ReDim b(19)

		Dim fs As FileStream = File.OpenRead(FileName)
		fs.Read(b, 0, 20)

		FileSign = System.Text.Encoding.Default.GetString(b)
		If (FileSign <> "Anplan DATABASE file") Then
			'ErrorCode = -1
			Return False
			'Throw New Exception("Invalid PANSOPS constants DATABASE file")
		End If

		DataSize = fs.Length - 20
		ReDim Data(DataSize - 1)

		fs.Read(Data, 0, DataSize)

		Index = 0

		Data(DataSize - 1) = Data(DataSize - 1) Xor Asc("R")

		For I = DataSize - 2 To 0 Step -1
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

		'TmpData.Name = ""
		'TmpData.Unit = ""
		'TmpData.Comment = ""
		'TmpData.DefinedIn = ""

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
						If FieldName = "VALUE" Then TmpData.Value = floatField
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

			TmpData.Value = TmpData.Value * TmpData.Multiplier

			If (TmpData.Unit = "rad") Or (TmpData.Unit = "°") Then
				TmpData.Value = Math.Round(RadToDeg(TmpData.Value), 2)
			End If

			Select Case TmpData.Name
				Case "arBufferMSA"
					arBufferMSA = TmpData
				Case "arMSARoundThresh"
					arMSARoundThreshold = TmpData
				Case "arMinInterDist"
					arMinInterDist = TmpData
				Case "arMinInterToler"
					arMinInterToler = TmpData
				Case "arStrInAlignment"
					arStrInAlignment = TmpData
				Case "arMaxRangeFAS"
					arMaxRangeFAS = TmpData
				Case "arCirclAprShift"
					arCirclAprShift = TmpData
				Case "arHoldAreaEdge"
					arHoldAreaEdge = TmpData
				Case "arHoldingBuffer"
					arHoldingBuffer = TmpData
				Case "arIASegmentMOC"
					arIASegmentMOC = TmpData
				Case "arISegmentMOC"
					arISegmentMOC = TmpData
				Case "arFASeg_FAF_MOC"
					arFASeg_FAF_MOC = TmpData
				Case "arFASegmentMOC"
					arFASegmentMOC = TmpData
				Case "arSOCdelayTime"
					arSOCdelayTime = TmpData
				Case "arNearTerrWindSp"
					arNearTerrWindSp = TmpData
				Case "arISAmax"
					arISAmax = TmpData
				Case "arISAmin"
					arISAmin = TmpData
				Case "arMA_InterMOC"
					arMA_InterMOC = TmpData
				Case "arMA_FinalMOC"
					arMA_FinalMOC = TmpData
				Case "arMAS_Climb_Min"
					arMAS_Climb_Min = TmpData
				Case "arMAS_Climb_Max"
					arMAS_Climb_Max = TmpData
				Case "arMAS_Climb_Nom"
					arMAS_Climb_Nom = TmpData
				Case "arMATurnAlt"
					arMATurnAlt = TmpData
				Case "arT_TechToleranc"
					arT_TechToleranc = TmpData
				Case "arTP_by_DME_div"
					arTP_by_DME_div = TmpData
				Case "arT_Gui_dist"
					arT_Gui_dist = TmpData
				Case "arafTrn_OSplay"
					arafTrn_OSplay = TmpData
				Case "arafTrn_ISplay"
					arafTrn_ISplay = TmpData
				Case "arSecAreaCutAngl"
					arSecAreaCutAngl = TmpData
				Case "arMATurnTrshAngl"
					arMATurnTrshAngl = TmpData
				Case "arFAFLenght"
					arFAFLenght = TmpData
				Case "arAbv_Treshold"
					arAbv_Treshold = TmpData
				Case "arFADescent_Min"
					arFADescent_Min = TmpData
				Case "arFADescent_Nom"
					arFADescent_Nom = TmpData
				Case "arImRange_Min"
					arImRange_Min = TmpData
				Case "arImRange_Nom"
					arImRange_Nom = TmpData
				Case "arImRange_Max"
					arImRange_Max = TmpData
				Case "arIFHalfWidth"
					arIFHalfWidth = TmpData
				Case "arMinISlen00_90"
					arMinISlen00_90 = TmpData
				Case "arMinISlen91_96"
					arMinISlen91_96 = TmpData
				Case "arMinISlen97_02"
					arMinISlen97_02 = TmpData
				Case "arMinISlen03_08"
					arMinISlen03_08 = TmpData
				Case "arMinISlen09_14"
					arMinISlen09_14 = TmpData
				Case "arMinISlen15_20"
					arMinISlen15_20 = TmpData
				Case "arFAFTolerance"
					arFAFTolerance = TmpData
				Case "arIFTolerance"
					arIFTolerance = TmpData
				Case "arImDescent_Max"
					arImDescent_Max = TmpData
				Case "arFAPMaxRange"
					arFAPMaxRange = TmpData
				Case "arOverHeadToler"
					arOverHeadToler = TmpData
				Case "arTrackAccuracy"
					arTrackAccuracy = TmpData
				Case "arPilotTolerance"
					arPilotTolerance = TmpData
				Case "arMA_SplayAngle"
					arMA_SplayAngle = TmpData
				Case "arOptimalFAFRang"
					arOptimalFAFRang = TmpData
				Case "arCurvatureCoeff"
					arCurvatureCoeff = TmpData
				Case "arFixMaxIgnorGrd"
					arFixMaxIgnorGrd = TmpData
				Case "arMOCChangeDist"
					arMOCChangeDist = TmpData
				Case "arAddMOCCoef"
					arAddMOCCoef = TmpData
				Case "arFIX15PlaneRang"
					arFIX15PlaneRang = TmpData
				Case "arFAFOptimalDist"
					arFAFOptimalDist = TmpData
				Case "arIAFMinDMERange"
					arIAFMinDMERange = TmpData
				Case "arIAFMaxTurnAngl"
					arIAFMaxTurnAngl = TmpData
				Case "arIAFMinTurnAngl"
					arIAFMinTurnAngl = TmpData
				Case "arIAFMinGuadLen"
					arIAFMinGuadLen = TmpData
				Case "arVisAverBank"
					arVisAverBank = TmpData
				Case "EnRouteSplayAngl"
					EnRouteSplayAngl = TmpData
				Case "arIADescent_Nom"
					arIADescent_Nom = TmpData
				Case "arIADescent_Max"
					arIADescent_Max = TmpData
				Case "arMSAMOC"
					arMSAMOC = TmpData
			End Select
		Next I

		On Error GoTo 0

		Return True

		'dpNGui_Ar1_Dist = dpNGui_Ar1
		'dpNGui_Ar1_Dist.Value = (dpNGui_Ar1.Value - dpH_abv_DER.Value) / dpPDG_Nom.Value
ErrorHandler:
		ErrorStr = Err.Number + "  " + Err.Description
		'ErrorCode = -2
		Return False
	End Function
End Module