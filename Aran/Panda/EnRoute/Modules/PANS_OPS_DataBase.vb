Option Strict Off
Option Explicit On

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module PANS_OPS_DataBase

	<System.Runtime.InteropServices.ComVisibleAttribute(False)> Structure PansOpsData
		Dim Name As String
		Dim Value As Double
		Dim Multiplier As Double
		Dim Unit As String
		Dim Comment As String
		Dim DefinedIn As String
	End Structure

	'Public ErrorCode As Integer

	Public erISA As PansOpsData
	Public TurnIAS As PansOpsData
	Public SplayAngl As PansOpsData
	Public SnapAngle As PansOpsData
	Public BankAngle As PansOpsData
	Public ToBankTime As PansOpsData
	Public MaxTurnAngle As PansOpsData
	Public TrackAccuracy As PansOpsData
	Public MaxPilotToler As PansOpsData
	Public OverHeadToler As PansOpsData
	Public SecAreaCutAngl As PansOpsData
	'Public EnRouteSplayAngl As PansOpsData

	Function InitModule() As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim Index As Integer
		Dim Fields As Integer
		Dim Records As Integer
		Dim intField As Integer
		Dim DataSize As Integer
		Dim FieldType As Integer
		Dim FileNumber As Integer
		Dim SmallIntField As Integer

		Dim FileSign As String
		Dim FileName As String
		Dim FieldName As String = ""
		Dim charField As String = ""

		Dim BoolField As Boolean
		Dim floatField As Double
		Dim DateTimeField As Date
		Dim CurrencyField As Decimal

		Dim data() As Byte
		Dim TmpData As PansOpsData

		InitModule = True
		'ErrorCode = 0

		FileName = ConstDir + "\enroute.dat"

		FileNumber = FreeFile()
		FileOpen(FileNumber, FileName, OpenMode.Binary, OpenAccess.Read)
		FileSign = InputString(FileNumber, 20)
		FileClose(FileNumber)

		If FileSign <> "Anplan DATABASE file" Then
			'ErrorCode = -1
			Return False
		End If

		data = My.Computer.FileSystem.ReadAllBytes(FileName)
		DataSize = data.Length()
		Index = 20

		data(DataSize - 1) = data(DataSize - 1) Xor Asc("R")

		For I = DataSize - 2 To Index Step -1
			data(I) = data(I) Xor data(I + 1)
		Next I

		GetIntData(data, Index, SmallIntField, 2)
		Fields = SmallIntField
		GetIntData(data, Index, SmallIntField, 2)
		Records = SmallIntField

		For I = 0 To Fields - 1
			GetIntData(data, Index, SmallIntField, 2)
			GetStrData(data, Index, charField, SmallIntField)
		Next I

		TmpData.Name = ""
		TmpData.Unit = ""
		TmpData.Comment = ""
		TmpData.DefinedIn = ""

		For I = 0 To Records - 1
			For J = 0 To Fields - 1
				GetIntData(data, Index, SmallIntField, 2)
				GetStrData(data, Index, FieldName, SmallIntField)
				GetIntData(data, Index, SmallIntField, 2)
				FieldType = SmallIntField

				Select Case FieldType
					'Fixed character field
					'Wide string field
					'Text memo field
					'Formatted text memo field
					'Character or string field
					Case 1, 16, 18, 23, 24
						GetIntData(data, Index, SmallIntField, 2)
						GetStrData(data, Index, charField, SmallIntField)
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
						GetIntData(data, Index, SmallIntField, 2)
						'Large integer field
						'32-bit integer field
					Case 3, 25
						GetIntData(data, Index, intField, 4)
					Case 5 'Boolean field
						GetData(data, Index, BoolField, 2)
					Case 6, 8 'Floating-point numeric field
						GetDoubleData(data, Index, floatField, 8)
						If FieldName = "VALUE" Then TmpData.Value = floatField
						If FieldName = "MULTIPLIER" Then TmpData.Multiplier = floatField
					Case 7 'Money field
						GetData(data, Index, CurrencyField, 8)
						'Date field
						'Time field
						'Date and time field
					Case 9, 10, 11
						GetData(data, Index, DateTimeField, 8)
				End Select
			Next J

			TmpData.Value = TmpData.Value * TmpData.Multiplier

			If (TmpData.Unit = "rad") Or (TmpData.Unit = "°") Then
				TmpData.Value = Math.Round(RadToDeg(TmpData.Value), 2)
			End If

			Select Case TmpData.Name
				Case "MaxTurnAngle"
					MaxTurnAngle = TmpData
				Case "erISA"
					erISA = TmpData
				Case "TurnIAS"
					TurnIAS = TmpData
				Case "BankAngle"
					BankAngle = TmpData
				Case "MaxPilotToler"
					MaxPilotToler = TmpData
				Case "ToBankTime"
					ToBankTime = TmpData
				Case "SplayAngl"
					SplayAngl = TmpData
				Case "SnapAngle"
					SnapAngle = TmpData
				Case "SecAreaCutAngl"
					SecAreaCutAngl = TmpData
				Case "TrackAccuracy"
					TrackAccuracy = TmpData
				Case "OverHeadToler"
					OverHeadToler = TmpData
			End Select
		Next I

		On Error GoTo 0

		Return True
EH:
		MessageBox.Show(Err.Number + ":  " + Err.Description, "Constants")
		'ErrorCode = Err.Number
		FileClose(FileNumber)
		ErrorStr = Err.Number + "  " + Err.Description
		'ErrorCode = -2
		Return False
	End Function
End Module