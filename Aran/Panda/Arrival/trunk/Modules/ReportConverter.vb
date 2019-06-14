Module ReportConverter

    Public Function ConvertReportDistance(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.CEIL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * ReportDistanceConverter(DistanceUnit).Multiplier
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * ReportDistanceConverter(ReportDistanceUnit).Multiplier / ReportDistanceConverter(ReportDistanceUnit).Rounding) * ReportDistanceConverter(ReportDistanceUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertReportHeight(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * ReportHeightConverter(ReportHeightUnit).Multiplier
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * ReportHeightConverter(ReportHeightUnit).Multiplier / ReportHeightConverter(ReportHeightUnit).Rounding) * ReportHeightConverter(ReportHeightUnit).Rounding
			Case eRoundMode.SPECIAL
				If HeightUnit = 0 Then
					Return System.Math.Round(Val_Renamed * ReportHeightConverter(ReportHeightUnit).Multiplier / 50.0) * 50.0
				ElseIf HeightUnit = 1 Then
					Return System.Math.Round(Val_Renamed * ReportHeightConverter(ReportHeightUnit).Multiplier / 100.0) * 100.0
				Else
					Return System.Math.Round(Val_Renamed * ReportHeightConverter(ReportHeightUnit).Multiplier / ReportHeightConverter(ReportHeightUnit).Rounding) * ReportHeightConverter(ReportHeightUnit).Rounding
				End If
		End Select
		Return Val_Renamed
	End Function
	
End Module

