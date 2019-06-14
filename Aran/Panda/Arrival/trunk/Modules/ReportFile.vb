Option Strict Off
Option Explicit On
Imports System.Reflection
Imports Aran.PANDA.Reports

Friend Class ReportFile : Inherits ReportBase
	'Public RefHeight As Double

	Public Sub WriteHeader(ByRef pReport As ReportHeader, Optional isProtocol As Boolean = False)
		Dim currentAssem As Assembly = Assembly.GetExecutingAssembly()
		'Dim titleAttrib As AssemblyTitleAttribute = currentAssem.GetCustomAttribute(TypeOf (AssemblyTitleAttribute))
		'Dim name As String = titleAttrib.Title + " v." + currentAssem.GetName().Version.ToString()

		WriteString(currentAssem.GetName().Version.ToString(), True, True)

		WriteString(PANSOPSVersion, True, True)
		WriteString("AIM Environment: " + pReport.Database, True, True)

		If (GlobalVars._settings.AnnexObstalce) Then
			WriteString("Obstacle source: Obstacle area obstacles.", True)
		Else
			WriteString("Obstacle source: All vertical structures from DB.", True)
		End If

		If pReport.Aerodrome <> "" Then
			WriteString(My.Resources.str10107 + pReport.Aerodrome, True)
		End If

		If pReport.Procedure <> "" Then
			WriteString(My.Resources.str00821 + pReport.Procedure, True)
		End If

		'If pReport.RWY <> "" Then WriteString("<p>" + My.Resources.str822 + pReport.RWY)

		If pReport.Category <> "" Then
			WriteString(My.Resources.str00823 + pReport.Category, True)
		End If

		If UserName <> "" Then
			WriteString("User name: " + UserName, True)
		End If

		WriteString()

		_excellExporter.CreateRow().Text(My.Resources.str00813, True, False, 12).Text(Today.ToString("d"))
		WriteText("<p><b>" + My.Resources.str00813 + "</b>    " + Today.ToString("d") + "</br>")
		_excellExporter.CreateRow().Text(My.Resources.str00824, True, False, 12).Text(TimeOfDay.ToString("T"))
		WriteText("<b>" + My.Resources.str00824 + "</b>    " + TimeOfDay.ToString("T") + "</p>" + "</br>")

		'pReport.EffectiveDate = New Date(pReport.EffectiveDate.Year, pReport.EffectiveDate.Month, pReport.EffectiveDate.Day, 0, 0, 0)
		'WriteString("<b>" + My.Resources.str853 + pReport.EffectiveDate.ToString("d") + "</p>")

		WriteString()
		WriteString()
		If (isProtocol = False) Then
			WriteString(My.Resources.str00806 + DistanceConverter(DistanceUnit).Unit + ".", True, False)
			WriteString(My.Resources.str00807 + HeightConverter(HeightUnit).Unit, True, False)
			WriteString(My.Resources.str00808 + SpeedConverter(SpeedUnit).Unit + ".", True, False)
		Else
			WriteString(My.Resources.str60521 + ReportDistanceConverter(ReportDistanceUnit).Unit + ".", True, False)
			WriteString(My.Resources.str60522 + ReportHeightConverter(ReportHeightUnit).Unit, True, False)
			WriteString(My.Resources.str60523 + ReportSpeedConverter(ReportSpeedUnit).Unit + ".", True, False)
		End If
		WriteString()
	End Sub

	Public Sub WriteText(ByRef Text As String)
		_stwr.WriteLine(Text)
	End Sub

	Public Sub WriteTab(Optional ByRef TabComment As String = "")
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		'Dim iColor As Integer

		If (lListView Is Nothing) Then Return
		'If (lListView.Items.Count = 0) Then Return

		If (TabComment <> "") Then
			_excellExporter.CreateSheet(TabComment)
			HTMLMessage(System.Net.WebUtility.HtmlEncode(TabComment))
			HTMLMessage()
		Else
			_excellExporter.CreateSheet()
		End If
		_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>")

		N = lListView.Columns.Count
		M = lListView.Items.Count 'Math.Min(lListView.Items.Count, 1000)

		_excellExporter.CreateRow()
		_stwr.WriteLine("<tr>")
		For I = 0 To N - 1
			_excellExporter.Text(lListView.Columns.Item(I).Text, True, False, 14)
			_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns.Item(I).Text) + "</b></td>")
		Next I
		_stwr.WriteLine("</tr>")

		'=================================================
		If M = 0 Then
			_excellExporter.CreateRow()
			_stwr.WriteLine("<tr>")

			For I = 0 To N - 1
				_excellExporter.Text("-", True, False, 14)
				_stwr.WriteLine("<td align=center>-</td>")
			Next I
			_stwr.WriteLine("</tr>")
		Else
			For I = 0 To M - 1
				itmX = lListView.Items.Item(I)
				If itmX.ForeColor = Color.Black Then
					ColorStr = Chr(34) + "000000" + Chr(34)
				ElseIf itmX.ForeColor = Color.Red Then
					ColorStr = Chr(34) + "FF0000" + Chr(34)
				ElseIf itmX.ForeColor = Color.Blue Then
					ColorStr = Chr(34) + "0000FF" + Chr(34)
				Else
					If (System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) >= 0) And (System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) <= 16777215) Then
						'iColor = CShort(CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 255) * 65536 + 
						' CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 65280)) +
						' CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 16711680) * CShort(0.0000152587890625)
						ColorStr = CStr(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor))
					Else
						ColorStr = Chr(34) + "000000" + Chr(34) 'CStr(Not itmX.ForeColor)
					End If
				End If

				If itmX.Font.Bold Then
					Face = "<b>"
					EndFace = "</b>"
				Else
					Face = ""
					EndFace = ""
				End If
				'	FontStr = "<Font Color=" + Chr(34) + ColorStr + Chr(34) + ">" + Face
				FontStr = "<Font Color=" + ColorStr + ">" + Face

				_stwr.WriteLine("<tr><td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(itmX.Text) + EndFace + "</td>")

				If I < 500 Then
					_excellExporter.CreateRow()
					_excellExporter.Text(itmX.Text, itmX.ForeColor)
				End If

				For J = 1 To lListView.Columns.Count - 1
					_stwr.WriteLine("<td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(itmX.SubItems(J).Text) + EndFace + "</td>")
					If I < 500 Then
						_excellExporter.Text(itmX.SubItems(J).Text, itmX.ForeColor)
					End If
				Next
				'	Print #pFileNum, "<Tr>" + FontStr + "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + itmX.Text + "</Td>"
				'	For J = 1 To lListView.ColumnHeaders.Count - 1
				'		Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + itmX.SubItems(J) + "</Td>"
				'	Next
				_stwr.WriteLine(EndFace + "</tr>")
			Next I
		End If
		'=================================================
		'    For I = 1 To M
		'        Set itmX = lListView.ListItems.Item(I)
		'
		'        Print #pFileNum, "<tr>"
		'        Print #pFileNum, "<td>" + itmX + "</td>"
		'        For J = 1 To N - 1
		'            Print #pFileNum, "<td>" + itmX.SubItems(J) + "</td>"
		'        Next J
		'        Print #pFileNum, "</tr>"
		'    Next I

		_stwr.WriteLine("</table>")
		_stwr.WriteLine("<br><br>")
	End Sub

	Public Sub WriteTabData(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1)
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim Bold As Boolean
		Dim Color As Color

		If (lListView Is Nothing) Then Return
		M = lListView.Items.Count

		_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>")

		N = lListView.Columns.Count

		_excellExporter.CreateRow()
		_stwr.WriteLine("<tr>")
		For I = 0 To N - 1
			_excellExporter.Text(lListView.Columns.Item(I).Text, True)
			_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(lListView.Columns.Item(I).Text) + "</b></td>")
		Next I
		_stwr.WriteLine("</tr>")

		'=================================================

		For I = 0 To M - 1
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

			itmX = lListView.Items.Item(I)

			_excellExporter.CreateRow()
			_stwr.WriteLine("<tr>")
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(itmX.Text) + EndFace + "</Td>")
			_excellExporter.Text(itmX.Text, Color, Bold)
			For J = 1 To N - 1
				_excellExporter.Text(itmX.SubItems(J).Text, Color, Bold)
				_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(itmX.SubItems(J).Text) + EndFace + "</Td>")
			Next J
			_stwr.WriteLine("</Tr>")
		Next I

		If M = 0 Then
			WriteNullLeg(Bold, Color, Face, EndFace, FontStr, ColorStr, N)
		End If

		_stwr.WriteLine("</table>")
		_stwr.WriteLine("<br><br>")

	End Sub

	Public Sub WriteObstData(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1)
		Dim I As Integer
		Dim N As Integer

		Dim Bold As Boolean
		Dim Color As Color
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim ColorStr As String
		Dim Headers() As String

		_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>")

		Headers = New String() {My.Resources.str30011, My.Resources.str50225, My.Resources.str00234, My.Resources.str30036, My.Resources.str40243, My.Resources.str40033, My.Resources.str40035, My.Resources.str40034}
		'=================================================================
		N = UBound(Headers)

		_stwr.WriteLine("<tr>")
		_excellExporter.CreateRow()
		For I = 0 To N
			_excellExporter.Text(Headers(I), True)
			_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(Headers(I)) + "</b></td>")
		Next I
		_stwr.WriteLine("</tr>")

		If Not Obstacles.Parts Is Nothing Then
			N = UBound(Obstacles.Parts)
			'=================================================
			If N > -1 Then
				For I = 0 To N
					If I = Ix Then
						ColorStr = Chr(34) + "FF0000" + Chr(34)
						Color = Color.Red
						Bold = True
						Face = "<b>"
						EndFace = "</b>"
					Else
						ColorStr = Chr(34) + "000000" + Chr(34)
						Color = Color.Black
						Bold = False
						Face = ""
						EndFace = ""
					End If

					_excellExporter.CreateRow()
					FontStr = "<Font Color=" + ColorStr + ">" + Face
					_stwr.WriteLine("<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(CStr(System.Math.Round(100.0 * Obstacles.Parts(I).fTmp + 0.04999999, 1))) + EndFace + "</Td>")
					_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(CStr(ConvertReportDistance(Obstacles.Parts(I).Dist, eRoundMode.NEAREST))) + EndFace + "</Td>")

					_excellExporter.Text(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName, Color, Bold)
					_excellExporter.Text(Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName, Color, Bold)
					_excellExporter.Text(CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST)), Color, Bold)
					_excellExporter.Text(CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST)), Color, Bold)
					_excellExporter.Text(CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST)), Color, Bold)
					_excellExporter.Text(CStr(System.Math.Round(100.0 * Obstacles.Parts(I).fTmp + 0.04999999, 1)), Color, Bold)
					_excellExporter.Text(CStr(ConvertReportDistance(Obstacles.Parts(I).Dist, eRoundMode.NEAREST)), Color, Bold)

					If (Obstacles.Parts(I).Flags And 1) = 1 Then
						_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Primary" + EndFace + "</Td>")
						_excellExporter.Text("Primary", Color, Bold)
					Else
						_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Secondary" + EndFace + "</Td>")
						_excellExporter.Text("Secondary", Color, Bold)
					End If

					_stwr.WriteLine("</Tr>")
				Next I
			Else
				WriteNullLeg(Bold, Color, Face, EndFace, FontStr, ColorStr)
			End If
		Else
			WriteNullLeg(Bold, Color, Face, EndFace, FontStr, ColorStr)
		End If
		'=======================================================================================================================

		_stwr.WriteLine("</table>")
		_stwr.WriteLine("<br><br>")
	End Sub

	Private Sub WriteNullLeg(ByRef Bold As Boolean, ByRef Color As Color, ByRef Face As String, ByRef EndFace As String, ByRef FontStr As String, ByRef ColorStr As String)
		ColorStr = Chr(34) + "000000" + Chr(34)
		Color = Color.Black
		Bold = False
		Face = ""
		EndFace = ""
		_excellExporter.CreateRow()
		FontStr = "<Font Color=" + ColorStr + ">" + Face
		_stwr.WriteLine("<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		_stwr.WriteLine("</Tr>")

		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
		_excellExporter.Text("---", Color, Bold)
	End Sub

	Private Sub WriteNullLeg(ByRef Bold As Boolean, ByRef Color As Color, ByRef Face As String, ByRef EndFace As String, ByRef FontStr As String, ByRef ColorStr As String, ByVal N As Int32)
		Dim I As Integer

		ColorStr = Chr(34) + "000000" + Chr(34)
		Color = Color.Black
		Bold = False
		Face = ""
		EndFace = ""
		_excellExporter.CreateRow()
		FontStr = "<Font Color=" + ColorStr + ">" + Face

		For I = 0 To N - 1
			_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode("---") + EndFace + "</Td>")
		Next

		_stwr.WriteLine("</Tr>")

		For I = 0 To N - 1
			_excellExporter.Text("---", Color, Bold)
		Next
	End Sub

	Public Sub WriteLegs(ByRef Legs As StepDownFIX(), ByRef IfPointReport As StepDownFIXReport, Headers As String(), Optional ByRef Ix As Integer = -1)
		Dim I As Integer
		Dim N As Integer
		Dim Count As Integer

		Dim Bold As Boolean
		Dim Color As Color

		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim ColorStr As String

		_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>")
		'=================================================================
		N = Headers.Length

		_excellExporter.CreateRow()
		_stwr.WriteLine("<tr>")
		For I = 0 To N - 1
			_excellExporter.Text(Headers(I), True)
			_stwr.WriteLine("<td><b>" + System.Net.WebUtility.HtmlEncode(Headers(I)) + "</b></td>")
		Next I
		_stwr.WriteLine("</tr>")

		N = Legs.Length - 1
		'=================================================
		Count = 1

		ColorStr = Chr(34) + "000000" + Chr(34)
		Face = ""
		EndFace = ""
		FontStr = "<Font Color=" + ColorStr + ">" + Face
		WriteLeg(IfPointReport, FontStr, "", EndFace)
		WriteLegExcel(IfPointReport, "", Color.Black, False)
		Count = Count + 1

		For I = 0 To N - 1
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
			WriteLeg(Legs(I).Report, FontStr, (Count - 1).ToString(), EndFace)
			WriteLegExcel(Legs(I).Report, (Count - 1).ToString(), Color, Bold)
			Count = Count + 1
		Next I
		'=======================================================================================================================

		_stwr.WriteLine("</table>")
		_stwr.WriteLine("<br><br>")
	End Sub

	Private Sub WriteLegExcel(ByVal Leg As StepDownFIXReport, Count As String, color As Color, bold As Boolean)
		_excellExporter.CreateRow()
		_excellExporter.Text(Count, color, bold)
		_excellExporter.Text(Leg.LegType, color, bold)
		_excellExporter.Text(Leg.WayPoint, color, bold)
		_excellExporter.Text(Leg.FlyOver, color, bold)
		_excellExporter.Text(Leg.Course, color, bold)
		_excellExporter.Text(Leg.MagVariation, color, bold)
		_excellExporter.Text(Leg.Distance, color, bold)
		_excellExporter.Text(Leg.TurnDirection, color, bold)
		_excellExporter.Text(Leg.Altitude, color, bold)
		_excellExporter.Text(Leg.Speed, color, bold)
		_excellExporter.Text(Leg.Latitude, color, bold)
		_excellExporter.Text(Leg.Longitude, color, bold)
	End Sub

	Private Sub WriteLeg(ByVal Leg As StepDownFIXReport, FontStr As String, Count As String, EndFace As String)
		_stwr.WriteLine("<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Count) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.LegType) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.WayPoint) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.FlyOver) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Course) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.MagVariation) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Distance) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.TurnDirection) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Altitude) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Speed) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Latitude) + EndFace + "</Td>")
		_stwr.WriteLine("<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + System.Net.WebUtility.HtmlEncode(Leg.Longitude) + EndFace + "</Td>")
		_stwr.WriteLine("</Tr>")
	End Sub

	'Public Sub WriteImage(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
	'	If (ImageName = "") Then Return
	'	If (ImageComment <> "") Then
	'		WriteMessage(ImageComment)
	'		WriteMessage()
	'	End If
	'	PrintLine(pFileNum, "<img src='" + ImageName + "' border='0'>")
	'End Sub

	'Public Sub WriteImageLink(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
	'	If (ImageName = "") Then Return
	'	PrintLine(pFileNum, "<a href='" + ImageName + "'>" + ImageComment + "</a>")
	'End Sub

	Public Sub WritePoint(ByRef DepPoint As ReportPoint)
		If (DepPoint.Description = "") Then Return

		_excellExporter.CreateRow()
		_excellExporter.H2(DepPoint.Description)
		HTMLMessage("[" + DepPoint.Description + "]")

		Param(My.Resources.str00841, DepPoint.Lat)
		Param(My.Resources.str00842, DepPoint.Lon)
		Param(My.Resources.str00843, DepPoint.CenterLat)
		Param(My.Resources.str00844, DepPoint.CenterLon)

		If DepPoint.Altitude <> NO_DATA_VALUE Then
			Param(My.Resources.str00223, CStr(ConvertHeight(DepPoint.Altitude, eRoundMode.NEAREST)), HeightConverter(HeightUnit).Unit)
		End If

		If DepPoint.Radius <> NO_DATA_VALUE Then
			Param(My.Resources.str00845, CStr(ConvertDistance(DepPoint.Radius, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit)
			If DepPoint.Turn > 0 Then
				Param(My.Resources.str00846, My.Resources.str00225)
			ElseIf DepPoint.Turn < 0 Then
				Param(My.Resources.str00846, My.Resources.str00226)
			End If
		End If

		If DepPoint.TurnAngle <> NO_DATA_VALUE Then
			Param(My.Resources.str20702, CStr(DepPoint.TurnAngle), "°")
		End If

		Param(My.Resources.str00847, DepPoint.Direction, "°")
		If DepPoint.ToNext <> NO_DATA_VALUE Then
			Param(My.Resources.str00848, CStr(ConvertDistance(DepPoint.ToNext, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit)
		End If

		If (DepPoint.Radius <> NO_DATA_VALUE) And (DepPoint.TurnArcLen <> NO_DATA_VALUE) Then
			Param(My.Resources.str00849, CStr(ConvertDistance(DepPoint.TurnArcLen, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit)
		End If

		WriteMessage()
	End Sub

End Class
