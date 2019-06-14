Option Strict Off
Option Explicit On
Friend Class ReportFile
	
	Const ReportFileExt As String = ".htm"
	Const FootCoeff As Double = 0.3048
	
	Public lListView As System.Windows.Forms.ListView
	Public ThrPtPrj As ESRI.ArcGIS.Geometry.IPoint
	Public RefHeight As Double
	
	Private pFileName As String
	Private pFileNum As Integer
	
	Public Sub OpenFile(ByRef Name As String, ByRef ReportTitle As String)
		If (Name = "") Then Exit Sub
		If (pFileNum > 0) Then Exit Sub
		
		pFileName = Name
		pFileName = pFileName & ReportFileExt
		pFileNum = FreeFile
		FileOpen(pFileNum, pFileName, OpenMode.Output, OpenAccess.Write, OpenShare.LockReadWrite)
		
		PrintLine(pFileNum, "<html>")
		PrintLine(pFileNum, "<head>")
		PrintLine(pFileNum, "<title>PANDA - " & ReportTitle & "</title>")
		PrintLine(pFileNum, "<style>")
		PrintLine(pFileNum, "body {font-family: Arial, Sans-Serif; font-size:12;}")
		PrintLine(pFileNum, "table {font-family: Arial, Sans-Serif; font-size:10;}")
		PrintLine(pFileNum, "</style>")
		PrintLine(pFileNum, "</head>")
		PrintLine(pFileNum, "<body>")
	End Sub
	
	Public Sub CloseFile()
		If (pFileNum <= 0) Then Exit Sub
		
		PrintLine(pFileNum, "</body>")
		PrintLine(pFileNum, "</html>")
		
		FileClose(pFileNum)
		pFileNum = -1
	End Sub
	
	Public Sub Out(ByRef Message As String)
		PrintLine(pFileNum, Message)
	End Sub
	
	Public Sub WriteString(Optional ByRef Message As String = "")
		PrintLine(pFileNum, Message & "<br>")
	End Sub
	
	Public Sub WriteMessage(Optional ByRef Message As String = "")
		Dim temp As String
		temp = ""
		If (Message <> "") Then temp = "<b>" & Message & "</b>"
		PrintLine(pFileNum, temp & "<br>")
	End Sub
	
	Public Sub WriteParam(ByRef ParamName As String, ByRef ParamValue As String, Optional ByRef ParamUnit As String = "")
		Dim temp As String
		
		If (ParamName = "") Or (ParamValue = "") Then Exit Sub
		
		temp = "<b>" & ParamName & ":</b> " & ParamValue
		If (ParamUnit <> "") Then temp = temp & " " & ParamUnit
		
		PrintLine(pFileNum, temp & "<br>")
	End Sub
	
	'Public Sub WriteLayerInfo(LI As LayerInfo)
	'    Print #pFileNum, LI.LayerName + " source: " + LI.Source + "<br>"
	'    Print #pFileNum, LI.LayerName + " date: " + CStr(LI.FileDate) + "<br>"
	'    Print #pFileNum, "<br>"
	'End Sub
	
	Public Sub WriteTab(Optional ByRef TabComment As String = "")
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim iColor As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		
		If (lListView Is Nothing) Then Exit Sub
		If (lListView.Items.Count = 0) Then Exit Sub
		
		If (TabComment <> "") Then
			WriteMessage(TabComment)
			WriteMessage()
		End If
		
		PrintLine(pFileNum, "<table border='1' cellspacing='0' cellpadding='1'>")
		
		N = lListView.Columns.Count
		M = lListView.Items.Count
		
		PrintLine(pFileNum, "<tr>")
		For I = 1 To N
			'UPGRADE_WARNING: Lower bound of collection lListView.ColumnHeaders has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			PrintLine(pFileNum, "<td><b>" & lListView.Columns.Item(I).Text & "</b></td>")
		Next I
		PrintLine(pFileNum, "</tr>")
		
		'=================================================
		For I = 1 To M
			'UPGRADE_WARNING: Lower bound of collection lListView.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			itmX = lListView.Items.Item(I)
			If System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) = 0 Then
				ColorStr = Chr(34) & "000000" & Chr(34)
			ElseIf System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) = 255 Then 
				ColorStr = Chr(34) & "FF0000" & Chr(34)
			ElseIf System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) = RGB(0, 0, 255) Then 
				ColorStr = Chr(34) & "0000FF" & Chr(34)
			Else
				If (System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) >= 0) And (System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) <= 16777215) Then
					iColor = CShort(CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 255) * 65536 + CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 65280)) + CShort(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor) And 16711680) * CShort(1.52587890625E-05)
					ColorStr = CStr(System.Drawing.ColorTranslator.ToOle(itmX.ForeColor))
				Else
					ColorStr = Chr(34) & "000000" & Chr(34) 'CStr(Not itmX.ForeColor)
				End If
			End If
			
			If itmX.Font.Bold Then
				Face = "<b>"
				EndFace = "</b>"
			Else
				Face = ""
				EndFace = ""
			End If
			'        FontStr = "<Font Color=" + Chr(34) + ColorStr + Chr(34) + ">" + Face
			FontStr = "<Font Color=" & ColorStr & ">" & Face
			
			PrintLine(pFileNum, "<Tr><Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & itmX.Text & EndFace & "</Td>")
			For J = 1 To lListView.Columns.Count - 1
				'UPGRADE_WARNING: Lower bound of collection itmX has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
				PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & itmX.SubItems(J).Text & EndFace & "</Td>")
			Next 
			'        Print #pFileNum, "<Tr>" + FontStr + "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + itmX.Text + "</Td>"
			'        For J = 1 To lListView.ColumnHeaders.Count - 1
			'            Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + itmX.SubItems(J) + "</Td>"
			'        Next
			
			PrintLine(pFileNum, EndFace & "</Tr>")
		Next I
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
		
		PrintLine(pFileNum, "</table>")
		PrintLine(pFileNum, "<br><br>")
		
	End Sub
	
	Public Sub WriteObstData(ByRef Obstacles() As TypeDefinitions.ObstacleType, Optional ByRef Ix As Integer = -1)
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer

        Dim Headers() As String
		
		PrintLine(pFileNum, "<table border='1' cellspacing='0' cellpadding='1'>")

        Headers = New String() {My.Resources.str1405, My.Resources.str1406, My.Resources.str1407, My.Resources.str1408, My.Resources.str1409}
		'=================================================================
		N = UBound(Headers)
		
		PrintLine(pFileNum, "<tr>")
		For I = 0 To N
            PrintLine(pFileNum, "<td><b>" + Headers(I) + "</b></td>")
		Next I
		PrintLine(pFileNum, "</tr>")
		
		M = UBound(Obstacles)
		'=================================================
		For I = 0 To M
			If I = Ix Then
				ColorStr = Chr(34) & "FF0000" & Chr(34)
				Face = "<b>"
				EndFace = "</b>"
			Else
				ColorStr = Chr(34) & "000000" & Chr(34)
				Face = ""
				EndFace = ""
			End If
			
			FontStr = "<Font Color=" & ColorStr & ">" & Face
			
			PrintLine(pFileNum, "<Tr><Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & Obstacles(I).Name & EndFace & "</Td>")
			PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & Obstacles(I).ID & EndFace & "</Td>")
			PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & CStr(ConvertHeight(Obstacles(I).Height, 2)) & EndFace & "</Td>")
			PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & CStr(ConvertHeight(Obstacles(I).MOC, 2)) & EndFace & "</Td>")
			PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & CStr(ConvertHeight(Obstacles(I).ReqH, 2)) & EndFace & "</Td>")
			PrintLine(pFileNum, "</Tr>")
			'        Print #pFileNum, EndFace + "</Tr>"
		Next I
		'=======================================================================================================================
		PrintLine(pFileNum, "</table>")
		PrintLine(pFileNum, "<br><br>")
	End Sub
	
	Public Sub WriteTabData(ByRef Obstacles() As TypeDefinitions.ObstacleType, Optional ByRef Ix As Integer = -1)
		Dim ColorStr As String
		Dim Face As String
		Dim EndFace As String
		Dim FontStr As String
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer

		Dim itmX As System.Windows.Forms.ListViewItem
		
		If (lListView Is Nothing) Then Exit Sub
		M = lListView.Items.Count
		If M = 0 Then Exit Sub
		
		PrintLine(pFileNum, "<table border='1' cellspacing='0' cellpadding='1'>")
		
		N = lListView.Columns.Count
		
		PrintLine(pFileNum, "<tr>")
		For I = 1 To N
			'UPGRADE_WARNING: Lower bound of collection lListView.ColumnHeaders has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			PrintLine(pFileNum, "<td><b>" & lListView.Columns.Item(I).Text & "</b></td>")
		Next I
		PrintLine(pFileNum, "</tr>")
		
		'=================================================
		For I = 1 To M
			If I - 1 = Ix Then
				ColorStr = Chr(34) & "FF0000" & Chr(34)
				Face = "<b>"
				EndFace = "</b>"
			Else
				ColorStr = Chr(34) & "000000" & Chr(34)
				Face = ""
				EndFace = ""
			End If
			
			FontStr = "<Font Color=" & ColorStr & ">" & Face
			
			'UPGRADE_WARNING: Lower bound of collection lListView.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			itmX = lListView.Items.Item(I)
			'
			PrintLine(pFileNum, "<tr>")
			PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & itmX.Text & EndFace & "</Td>")
			'        Print #pFileNum, "<td>" + itmX + "</td>"
			For J = 1 To N - 1
				'UPGRADE_WARNING: Lower bound of collection itmX has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
				PrintLine(pFileNum, "<Td Width=" & Chr(34) & "9%" & Chr(34) & ">" & FontStr & itmX.SubItems(J).Text & EndFace & "</Td>")
			Next J
			PrintLine(pFileNum, "</Tr>")
		Next I
		'=================================================
		'    For I = 0 To M
		'        If I = Ix Then
		'            ColorStr = Chr(34) + "FF0000" + Chr(34)
		'            Face = "<b>"
		'            EndFace = "</b>"
		'        Else
		'            ColorStr = Chr(34) + "000000" + Chr(34)
		'            Face = ""
		'            EndFace = ""
		'        End If
		'
		'        FontStr = "<Font Color=" + ColorStr + ">" + Face
		'
		'        Print #pFileNum, "<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + Obstacles(I).Name + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + Obstacles(I).ID + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(Obstacles(I).Height, 1)) + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(Obstacles(I).MOC, 1)) + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(Obstacles(I).ReqH, 1)) + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(100# * Obstacles(I).fTmp + 0.04999999, 1)) + EndFace + "</Td>"
		'        Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + CStr(Round(Obstacles(I).Dist, 1)) + EndFace + "</Td>"
		'
		'        If (Obstacles(I).Flags And 1) = 1 Then
		'            Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Primary" + EndFace + "</Td>"
		'        Else
		'            Print #pFileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + "Secondary" + EndFace + "</Td>"
		'        End If
		'
		'        Print #pFileNum, "</Tr>"
		''        Print #pFileNum, EndFace + "</Tr>"
		'    Next I
		'=======================================================================================================================
		PrintLine(pFileNum, "</table>")
		PrintLine(pFileNum, "<br><br>")
		
	End Sub
	
	Public Sub WriteImage(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
		If (ImageName = "") Then Exit Sub
		
		If (ImageComment <> "") Then
			WriteMessage(ImageComment)
			WriteMessage()
		End If
		
		PrintLine(pFileNum, "<img src='" & ImageName & "' border='0'>")
	End Sub
	
	Public Sub WriteImageLink(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
		If (ImageName = "") Then Exit Sub
		
		PrintLine(pFileNum, "<a href='" & ImageName & "'>" & ImageComment & "</a>")
	End Sub
	
	'Public Sub WritePoint(DepPoint As ReportPoint)
	'    If (DepPoint.Description = "") Then Exit Sub
	'
	'    WriteMessage DepPoint.Description
	'
	'    WriteParam LoadResString(209), DepPoint.Lat
	'    WriteParam LoadResString(210), DepPoint.Lon
	'    WriteParam LoadResString(209) + LoadResString(300), DepPoint.CenterLat
	'    WriteParam LoadResString(210) + LoadResString(300), DepPoint.CenterLon
	'
	'    If DepPoint.Height <> NO_DATA_VALUE Then
	'        WriteParam LoadResString(207), CStr(ConvertHeight(DepPoint.Height, 2)), HeightConverter(HeightUnit).Unit
	'        WriteParam LoadResString(208), CStr(ConvertHeight(DepPoint.Height - RefHeight, 2)), HeightConverter(HeightUnit).Unit
	'    End If
	'
	'    If DepPoint.Raidus > 0 Then
	'        WriteParam LoadResString(305), CStr(ConvertDistance(DepPoint.Raidus, 2)), DistanceConverter(DistanceUnit).Unit
	'        If DepPoint.Turn > 0 Then
	'            WriteParam LoadResString(306), LoadResString(308)
	'        Else
	'            WriteParam LoadResString(306), LoadResString(309)
	'        End If
	'    End If
	'
	'    If DepPoint.TurnAngle <> NO_DATA_VALUE Then WriteParam LoadResString(307), CStr(DepPoint.TurnAngle), "°"
	'    WriteParam LoadResString(200), DepPoint.Direction, "°"
	'
	'    If DepPoint.ToNext <> NO_DATA_VALUE Then
	'        WriteParam LoadResString(501), CStr(ConvertDistance(DepPoint.ToNext, 2)), DistanceConverter(DistanceUnit).Unit
	'    End If
	'
	'    If (DepPoint.Raidus > 0) And (DepPoint.TurnArcLen <> NO_DATA_VALUE) Then
	'        WriteParam LoadResString(310), CStr(ConvertDistance(DepPoint.TurnArcLen, 2)), DistanceConverter(DistanceUnit).Unit
	'    End If
	'
	'    WriteMessage
	'End Sub
	
	'Public Sub WriteTraceSegmentOld(Segment As TraceSegment)
	'
	'    If (Segment.RepComment = "") Then Exit Sub
	'
	'    WriteMessage Segment.RepComment
	'
	'    WriteParam "Координаты начала сегмента", Segment.StCoords
	'    WriteParam "Координаты конца сегмента", Segment.FinCoords
	'    WriteParam "Начальный курс", CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'    If (Segment.BetweenTurns <> 0) Then
	'        WriteParam "Промежуточный курс", CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirBetween), 360#), 2)), "°"
	'    End If
	'    WriteParam "Конечный курс", CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirOut), 360#), 2)), "°"
	'    WriteParam "Начальная высота", CStr(Round(Segment.HStart)), "м"
	'    WriteParam "Конечная высота", CStr(Round(Segment.HFinish)), "м"
	'    WriteParam "Длина сегмента", CStr(Round(Segment.Length)), "м"
	'    If (Segment.TurnR > 0) Then
	'        WriteParam LoadResString(305), CStr(Round(Segment.TurnR)), "м"
	'    End If
	'    WriteParam "Координаты точки входа в 1 разворот", Segment.St1Coords
	'    WriteParam "Координаты точки выхода из 1 разворота", Segment.Fin1Coords
	'    WriteParam "Координаты центра 1 разворота", Segment.Center1Coords
	'    If (Segment.Turn1Dir <> 0) Then
	'        WriteParam "Угол 1 разворота", CStr(Round(Segment.Turn1Angle, 2)), "°"
	'        If (Segment.Turn1Dir > 0) Then
	'            WriteParam "Направление 1 разворота", LoadResString(308)
	'        Else
	'            WriteParam "Направление 1 разворота", LoadResString(309)
	'        End If
	'    End If
	'    WriteParam "Координаты точки входа во 2 разворот", Segment.St2Coords
	'    WriteParam "Координаты точки выхода из 2 разворота", Segment.Fin2Coords
	'    WriteParam "Координаты центра 2 разворота", Segment.Center2Coords
	'    If (Segment.Turn2Dir <> 0) Then
	'        WriteParam "Угол 2 разворота", CStr(Round(Segment.Turn2Angle, 2)), "°"
	'        If (Segment.Turn2Dir > 0) Then
	'            WriteParam "Направление 2 разворота", LoadResString(308)
	'        Else
	'            WriteParam "Направление 2 разворота", LoadResString(309)
	'        End If
	'    End If
	'    If (Segment.BetweenTurns <> 0) Then
	'        WriteParam "Длина линии пути счисления", CStr(Round(Segment.BetweenTurns)), "м"
	'    End If
	'
	'    WriteMessage
	'
	'End Sub
	
	'Public Sub WriteTraceSegment(Segment As TraceSegment, ByVal IsLastSegment As Boolean)
	'
	'    Select Case Segment.SegmentType
	'    Case 1
	'        WriteMessage "[Начальная точка прямого сегмента] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.StCoords
	'
	'        If Segment.HStart <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HStart, 2)), HeightConverter(HeightUnit).Unit
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HStart - RefHeight, 2)), HeightConverter(HeightUnit).Unit
	'        End If
	'
	'        If Segment.DirIn <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'        End If
	'        If Segment.Length <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.Length, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'
	'        If (IsLastSegment) Then
	'            WriteMessage "[Конечная точка прямого сегмента] - " + Segment.RepComment
	'            WriteParam "Координаты", Segment.FinCoords
	'
	'            If Segment.HFinish <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HFinish, 2)), HeightConverter(HeightUnit).Unit
	'                WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HFinish - RefHeight, 2)), HeightConverter(HeightUnit).Unit
	'            End If
	'
	'            If Segment.DirOut <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirOut), 360#), 2)), "°"
	'            End If
	'            WriteMessage
	'        End If
	'    Case 2, 3
	'        WriteMessage "[Точка входа в разворот] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.StCoords
	'
	'        If Segment.HStart <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HStart, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.HStart / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HStart - RefHeight, 2)), HeightConverter(HeightUnit).Unit   '+ " / " + CStr(Round((Segment.HStart - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirIn <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.Length <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.Length, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'        WriteParam "Центр разворота", Segment.Center1Coords
	'
	'        If Segment.TurnR <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(305), CStr(ConvertDistance(Segment.TurnR, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        If (Segment.Turn1Dir > 0) Then
	'            WriteParam LoadResString(306), LoadResString(308)
	'        Else
	'            WriteParam LoadResString(306), LoadResString(309)
	'        End If
	'
	'        If Segment.Turn1Angle <> NO_DATA_VALUE Then WriteParam LoadResString(307), CStr(Round(Segment.Turn1Angle, 2)), "°"
	'        WriteMessage
	'
	'        If (IsLastSegment) Then
	'            WriteMessage "[Точка выхода из разворота] - " + Segment.RepComment
	'            WriteParam "Координаты", Segment.FinCoords
	'
	'            If Segment.HFinish <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HFinish, 2)), HeightConverter(HeightUnit).Unit   '+ " / " + CStr(Round(Segment.HFinish / FootCoeff)), "м/фт"
	'                WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HFinish - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.HFinish - RefHeight) / FootCoeff)), "м/фт"
	'            End If
	'
	'            If Segment.DirOut <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirOut), 360#), 2)), "°"
	'            End If
	'            WriteMessage
	'        End If
	'    Case 4
	'        WriteMessage "[Начальная точка прямого сегмента] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.StCoords
	'
	'        If Segment.HStart <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HStart, 2)), HeightConverter(HeightUnit).Unit   '+ " / " + CStr(Round(Segment.HStart / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HStart - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.HStart - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirIn <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.BetweenTurns <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.BetweenTurns, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'
	'        WriteMessage "[Точка входа в разворот] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.St1Coords
	'
	'        If Segment.H1 <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.H1, 2)), HeightConverter(HeightUnit).Unit   '+ " / " + CStr(Round(Segment.H1 / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.H1 - RefHeight, 2)), HeightConverter(HeightUnit).Unit   '+ " / " + CStr(Round((Segment.H1 - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirIn <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.Turn1Length <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.Turn1Length, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'        WriteParam "Центр разворота", Segment.Center1Coords
	'
	'        If Segment.TurnR <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(305), CStr(ConvertDistance(Segment.TurnR, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        If (Segment.Turn1Dir > 0) Then
	'            WriteParam LoadResString(306), LoadResString(308)
	'        Else
	'            WriteParam LoadResString(306), LoadResString(309)
	'        End If
	'
	'        If Segment.Turn1Angle <> NO_DATA_VALUE Then WriteParam LoadResString(307), CStr(Round(Segment.Turn1Angle, 2)), "°"
	'
	'
	'        WriteMessage
	'
	'        If (IsLastSegment) Then
	'            WriteMessage "[Точка выхода из разворота] - " + Segment.RepComment
	'            WriteParam "Координаты", Segment.FinCoords
	'
	'            If Segment.HFinish <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HFinish, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.HFinish / FootCoeff)), "м/фт"
	'                WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HFinish - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.HFinish - RefHeight) / FootCoeff)), "м/фт"
	'            End If
	'
	'            If Segment.DirOut <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirOut), 360#), 2)), "°"
	'            End If
	'            WriteMessage
	'        End If
	'    Case 5
	'        WriteMessage "[Точка входа в 1 разворот] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.StCoords
	'
	'        If Segment.HStart <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HStart, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.HStart / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HStart - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.HStart - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirIn <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirIn), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.Turn1Length <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.Turn1Length, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'
	'        WriteParam "Центр 1 разворота", Segment.Center1Coords
	'
	'        If Segment.TurnR <> NO_DATA_VALUE Then
	'            WriteParam "Радиус 1 разворота", CStr(ConvertDistance(Segment.TurnR, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        If (Segment.Turn1Dir > 0) Then
	'            WriteParam "Направление 1 разворота", LoadResString(308)
	'        Else
	'            WriteParam "Направление 1 разворота", LoadResString(309)
	'        End If
	'
	'        If Segment.Turn1Angle <> NO_DATA_VALUE Then
	'            WriteParam "Угол 1 разворота", CStr(Round(Segment.Turn1Angle, 2)), "°"
	'        End If
	'
	'        WriteMessage
	'
	'        WriteMessage "[Точка выхода из 1 разворота] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.Fin1Coords
	'
	'        If Segment.H1 <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.H1, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.H1 / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.H1 - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.H1 - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirBetween <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirBetween), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.BetweenTurns <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.BetweenTurns, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'        WriteMessage
	'
	'        WriteMessage "[Точка входа во 2 разворот] - " + Segment.RepComment
	'        WriteParam "Координаты", Segment.St2Coords
	'
	'        If Segment.H2 <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(207), CStr(ConvertHeight(Segment.H2, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.H2 / FootCoeff)), "м/фт"
	'            WriteParam LoadResString(208), CStr(ConvertHeight(Segment.H2 - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.H2 - RefHeight) / FootCoeff)), "м/фт"
	'        End If
	'
	'        If Segment.DirBetween <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirBetween), 360#), 2)), "°"
	'        End If
	'
	'        If Segment.Turn2Length <> NO_DATA_VALUE Then
	'            WriteParam LoadResString(501), CStr(ConvertDistance(Segment.Turn2Length, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        WriteMessage
	'        WriteParam "Центр 2 разворота", Segment.Center2Coords
	'
	'        If Segment.TurnR <> NO_DATA_VALUE Then
	'            WriteParam "Радиус 2 разворота", CStr(ConvertDistance(Segment.TurnR, 2)), DistanceConverter(DistanceUnit).Unit
	'        End If
	'
	'        If (Segment.Turn2Dir > 0) Then
	'            WriteParam "Направление 2 разворота", LoadResString(308)
	'        Else
	'            WriteParam "Направление 2 разворота", LoadResString(309)
	'        End If
	'
	'        If Segment.Turn2Angle <> NO_DATA_VALUE Then WriteParam "Угол 2 разворота", CStr(Round(Segment.Turn2Angle, 2)), "°"
	'
	'        WriteMessage
	'
	'        If (IsLastSegment) Then
	'            WriteMessage "[Точка выхода из 2 разворота] - " + Segment.RepComment
	'            WriteParam "Координаты", Segment.FinCoords
	'
	'            If Segment.HFinish <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(207), CStr(ConvertHeight(Segment.HFinish, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round(Segment.HFinish / FootCoeff)), "м/фт"
	'                WriteParam LoadResString(208), CStr(ConvertHeight(Segment.HFinish - RefHeight, 2)), HeightConverter(HeightUnit).Unit  ' + " / " + CStr(Round((Segment.HFinish - RefHeight) / FootCoeff)), "м/фт"
	'            End If
	'
	'            If Segment.DirOut <> NO_DATA_VALUE Then
	'                WriteParam LoadResString(200), CStr(Round(Modulus(Dir2Azt(ThrPtPrj, Segment.DirOut), 360#), 2)), "°"
	'            End If
	'            WriteMessage
	'        End If
	'
	'    End Select
	'
	'End Sub
End Class