Option Strict Off
Option Explicit On
Imports System.Reflection
Imports Aran.PANDA.Reports

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Friend Class ReportFile : Inherits ReportBase

	Public Sub WriteHeader(ByRef pReport As ReportHeader)
		Dim currentAssem As Assembly = Assembly.GetExecutingAssembly()

		WriteString(currentAssem.GetName().Version.ToString(), True, True)
		WriteString(GlobalVars.PANSOPSVersion, True, True)
		WriteString("AIM Environment: " + pReport.Database, True, True)

		WriteString("Obstacle source: All vertical structures from DB.", True)

		If pReport.Aerodrome <> "" Then
			WriteString(My.Resources.str0135 + pReport.Aerodrome, True)
		End If

		If pReport.Procedure <> "" Then
			WriteString(My.Resources.str0130 + pReport.Procedure, True)
		End If

		If pReport.Category <> "" Then
			WriteString(My.Resources.str1051 + pReport.Category, True)
		End If

		If UserName <> "" Then
			WriteString(My.Resources.str0136 + UserName, True)
		End If

		WriteString()

		_excellExporter.CreateRow().Text(My.Resources.str0128, True, False, 12).Text(Today.ToString("d"))
		_stwr.WriteLine("<p><b>" + My.Resources.str0128 + "</b>    " + Today.ToString("d") + "</br>")
		_excellExporter.CreateRow().Text(My.Resources.str0129, True, False, 12).Text(TimeOfDay.ToString("T"))
		_stwr.WriteLine("<b>" + My.Resources.str0129 + "</b>    " + TimeOfDay.ToString("T") + "</p>" + "</br>")

		'pReport.EffectiveDate = New Date(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0)
		'WriteString("<b>" + My.Resources.str853 + pReport.EffectiveDate.ToString("d") + "</p>")

		WriteString()
		WriteString()

		WriteString(My.Resources.str0131 + DistanceConverter(DistanceUnit).Unit + ".", True, False)
		WriteString(My.Resources.str0132 + HeightConverter(HeightUnit).Unit, True, False)
		WriteString(My.Resources.str0133 + SpeedConverter(SpeedUnit).Unit + ".", True, False)

		WriteString()
	End Sub

	Public Sub WriteObstData(ByRef Obstacles() As TypeDefinitions.ObstacleType, Optional Ix As Integer = -1)
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String

		Dim Bold As Boolean
		Dim Color As Color

		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim Headers() As String

		_stwr.WriteLine("<Table border='1' cellspacing='0' cellpadding='1'>")

		Headers = New String() {My.Resources.str2021, My.Resources.str2022, My.Resources.str2023 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2024 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2025 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2026 + " (" + DistanceConverter(DistanceUnit).Unit + ")", My.Resources.str2027}

		N = UBound(Headers)

		_stwr.WriteLine("<Tr>")
		_excellExporter.CreateRow()

		For I = 0 To N
			_excellExporter.Text(Headers(I), True)
			_stwr.WriteLine("<Td><b>" + System.Net.WebUtility.HtmlEncode(Headers(I)) + "</b></Td>")
		Next I
		_stwr.WriteLine("</Tr>")

		M = UBound(Obstacles)

		For I = 0 To M
			If I = Ix Then
				ColorStr = Chr(34) + "FF0000" + Chr(34)
				Face = "<b>"
				EndFace = "</b>"
				Color = Color.Red
				Bold = True
			Else
				ColorStr = Chr(34) + "000000" + Chr(34)
				Face = ""
				EndFace = ""
				Color = Color.Black
				Bold = False
			End If

			FontStr = "<Font Color=" + ColorStr + ">" + Face

			_stwr.WriteLine("<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + Obstacles(I).TypeName + EndFace + "</Td>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + Obstacles(I).UnicalName + EndFace + "</Td>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(Obstacles(I).Height)) + EndFace + "</Td>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(Obstacles(I).MOC)) + EndFace + "</Td>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(Obstacles(I).ReqH)) + EndFace + "</Td>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertDistance(Obstacles(I).Dist)) + EndFace + "</Td>")

			_excellExporter.CreateRow()
			_excellExporter.Text(Obstacles(I).TypeName, Color, Bold)
			_excellExporter.Text(Obstacles(I).UnicalName, Color, Bold)
			_excellExporter.Text(CStr(ConvertHeight(Obstacles(I).Height, eRoundMode.NEAREST)), Color, Bold)
			_excellExporter.Text(CStr(ConvertHeight(Obstacles(I).MOC, eRoundMode.NEAREST)), Color, Bold)
			_excellExporter.Text(CStr(ConvertHeight(Obstacles(I).ReqH, eRoundMode.NEAREST)), Color, Bold)
			_excellExporter.Text(CStr(ConvertDistance(Obstacles(I).Dist, eRoundMode.NEAREST)), Color, Bold)

			If Obstacles(I).Prima = 1 Then
				_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Primary" + EndFace + "</Td>")
				_excellExporter.Text("Primary", Color, Bold)
			Else
				_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Secondary" + EndFace + "</Td>")
				_excellExporter.Text("Secondary", Color, Bold)
			End If
			_stwr.WriteLine("</Tr>")

		Next I

		_stwr.WriteLine("</Table>")
		_stwr.WriteLine("<br><br>")
	End Sub

	Public Sub WriteSegmentData(ByRef SegmentData As TypeDefinitions.SegmentInfo) ', ByRef Ix As Integer
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim ColorStr As String

		Dim Bold As Boolean
		Dim Color As Color

		Dim N As Integer
		Dim I As Integer
		Dim fTmp As Double
		Dim Headers() As String

		_stwr.WriteLine("<Table border='1' cellspacing='0' cellpadding='1'>")

		Headers = New String() {My.Resources.str2028, My.Resources.str2001, My.Resources.str2029, My.Resources.str2002, My.Resources.str2029, My.Resources.str2003, My.Resources.str2004 + " (" + DistanceConverter(DistanceUnit).Unit + ")", My.Resources.str2006 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2007 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2008 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2018 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2019 + " (" + HeightConverter(HeightUnit).Unit + ")", My.Resources.str2020}
		'My.Resources.str2005,
		N = UBound(Headers)

		_stwr.WriteLine("<Tr>")
		_excellExporter.CreateRow()

		For I = 0 To N
			_excellExporter.Text(Headers(I), True)
			_stwr.WriteLine("<Td><b>" + Headers(I) + "</b></Td>")
		Next I
		_stwr.WriteLine("</Tr>")

		ColorStr = Chr(34) + "000000" + Chr(34)
		Face = ""
		EndFace = ""
		FontStr = "<Font Color=" + ColorStr + ">" + Face

		Color = Color.Black
		Bold = False

		_stwr.WriteLine("<Tr>")
		_excellExporter.CreateRow()


		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.EndNav.CallSign + "-" + GetNavTypeName(SegmentData.EndNav.TypeCode) + EndFace + "</Td>")
		_excellExporter.Text(SegmentData.EndNav.CallSign + "-" + GetNavTypeName(SegmentData.EndNav.TypeCode), Color, Bold)

		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.StartFIX.Name + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.StartInter.CallSign + "-" + GetNavTypeName(SegmentData.StartInter.TypeCode) + EndFace + "</Td>")

		_excellExporter.Text(SegmentData.StartFIX.Name, Color, Bold)
		_excellExporter.Text(SegmentData.StartInter.CallSign + "-" + GetNavTypeName(SegmentData.StartInter.TypeCode), Color, Bold)

		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.EndFIX.Name + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.EndInter.CallSign + "-" + GetNavTypeName(SegmentData.EndInter.TypeCode) + EndFace + "</Td>")
		_excellExporter.Text(SegmentData.EndFIX.Name, Color, Bold)
		_excellExporter.Text(SegmentData.EndInter.CallSign + "-" + GetNavTypeName(SegmentData.EndInter.TypeCode), Color, Bold)

		fTmp = Dir2Azt(SegmentData.StartFIX.pPtPrj, SegmentData.fDirection)
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(System.Math.Round(fTmp, 1)) + "°" + EndFace + "</Td>")
		_excellExporter.Text(CStr(System.Math.Round(fTmp, 1)) + "°", Color, Bold)

		fTmp = ReturnDistanceInMeters(SegmentData.StartFIX.pPtPrj, SegmentData.EndFIX.pPtPrj)
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertDistance(fTmp)) + EndFace + "</Td>")
		_excellExporter.Text(CStr(ConvertDistance(fTmp)), Color, Bold)

		'Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(SegmentData.fTurnAngle, 1)) + "°" + EndFace + "</Td>"
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(SegmentData.fHInterS)) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(SegmentData.fHInterE)) + EndFace + "</Td>")
		'_excellExporter.Text(CStr(System.Math.Round(SegmentData.fTurnAngle, 1)) + "°", Color, Bold)
		_excellExporter.Text(CStr(ConvertHeight(SegmentData.fHInterS)), Color, Bold)
		_excellExporter.Text(CStr(ConvertHeight(SegmentData.fHInterE)), Color, Bold)


		fTmp = Math.Max(SegmentData.fHGuidE, SegmentData.fHGuidS)
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(fTmp)) + EndFace + "</Td>")
		_excellExporter.Text(CStr(ConvertHeight(fTmp)), Color, Bold)

		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(SegmentData.DominantObstacle.ReqH)) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(ConvertHeight(SegmentData.fMOC)) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + SegmentData.DominantObstacle.UnicalName + EndFace + "</Td>")
		_excellExporter.Text(CStr(ConvertHeight(SegmentData.DominantObstacle.ReqH)), Color, Bold)
		_excellExporter.Text(CStr(ConvertHeight(SegmentData.fMOC)), Color, Bold)
		_excellExporter.Text(SegmentData.DominantObstacle.UnicalName, Color, Bold)

		_stwr.WriteLine("</Tr>")
		_stwr.WriteLine("</Table>")
		_stwr.WriteLine("<br><br>")
	End Sub

End Class