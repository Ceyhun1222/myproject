Option Strict Off
Option Explicit On
Option Compare Text

Imports ESRI.ArcGIS

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module UIFunctions

	Public Function ConvertDistance(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (eRoundMode.NONE < 0) Or (RoundMode > eRoundMode.CEIL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding - 0.4999) * DistanceConverter(DistanceUnit).Rounding
			Case eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding) * DistanceConverter(DistanceUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding + 0.4999) * DistanceConverter(DistanceUnit).Rounding
		End Select

		Return Val_Renamed * DistanceConverter(DistanceUnit).Multiplier
	End Function

	Public Function ConvertHeight(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (eRoundMode.NONE < 0) Or (RoundMode > eRoundMode.CEIL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding - 0.4999) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding + 0.4999) * HeightConverter(HeightUnit).Rounding
		End Select
		Return Val_Renamed * HeightConverter(HeightUnit).Multiplier
	End Function

	'Public Function ConvertSpeed(ByVal Val_Renamed As Double, ByVal RoundMode As eRoundMode) As Double
	'	If (eRoundMode.rmNONE < 0) Or (RoundMode > eRoundMode.rmCEIL) Then RoundMode = eRoundMode.rmNONE
	'	Select Case RoundMode
	'		Case eRoundMode.rmFLOOR
	'			Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding - 0.4999) * SpeedConverter(SpeedUnit).Rounding
	'		Case eRoundMode.rmNERAEST
	'			Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding) * SpeedConverter(SpeedUnit).Rounding
	'		Case eRoundMode.rmCEIL
	'			Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding + 0.4999) * SpeedConverter(SpeedUnit).Rounding
	'	End Select
	'	Return Val_Renamed * SpeedConverter(SpeedUnit).Multiplier
	'End Function

	'Public Function ConvertDSpeed(ByVal Val_Renamed As Double, ByVal RoundMode As eRoundMode) As Double
	'	If (eRoundMode.rmNONE < 0) Or (RoundMode > eRoundMode.rmCEIL) Then RoundMode = eRoundMode.rmNONE
	'	Select Case RoundMode
	'		Case eRoundMode.rmFLOOR
	'			Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding - 0.4999) * DSpeedConverter(HeightUnit).Rounding
	'		Case eRoundMode.rmNERAEST
	'			Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding) * DSpeedConverter(HeightUnit).Rounding
	'		Case eRoundMode.rmCEIL
	'			Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding + 0.4999) * DSpeedConverter(HeightUnit).Rounding
	'	End Select
	'	Return Val_Renamed * DSpeedConverter(HeightUnit).Multiplier
	'End Function

	Public Function DeConvertDistance(ByVal Val_Renamed As Double) As Double
		DeConvertDistance = Val_Renamed / DistanceConverter(DistanceUnit).Multiplier
	End Function

	Public Function DeConvertHeight(ByVal Val_Renamed As Double) As Double
		DeConvertHeight = Val_Renamed / HeightConverter(HeightUnit).Multiplier
	End Function

	'Public Function DeConvertSpeed(ByVal Val_Renamed As Double) As Double
	'	DeConvertSpeed = Val_Renamed / SpeedConverter(SpeedUnit).Multiplier
	'End Function

	'Public Function DeConvertDSpeed(ByVal Val_Renamed As Double) As Double
	'	DeConvertDSpeed = Val_Renamed / DSpeedConverter(HeightUnit).Multiplier
	'End Function

	Sub TextBoxFloat(ByRef KeyAscii As Char, ByVal BoxText As String)
		Dim N As Integer
		Dim DecSep As Char

		If KeyAscii < Chr(32) Then Exit Sub

		DecSep = Mid(CStr(1.1), 2, 1)

		If ((KeyAscii < "0" Or KeyAscii > "9") And KeyAscii <> DecSep) Then
			KeyAscii = Chr(0)
		ElseIf KeyAscii = DecSep Then
			N = InStr(BoxText, DecSep)
			If (N <> 0) Then KeyAscii = Chr(0)
		End If
	End Sub

	Public Function GetStyleGallery() As ESRI.ArcGIS.Display.IStyleGallery
#If DEBUG__ Then
		'UPGRADE_NOTE: #If #EndIf block was not upgraded because the expression DEBUG__ did not evaluate to True or was not evaluated. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="27EE2C3C-05AF-4C04-B2AF-657B4FB6B5FC"'

		Dim SG              As IStyleGallery
		Dim pScaleBarUp     As IScaleBar
		Dim pScaleBarDn     As IScaleBar
		Dim pSGS            As IStyleGalleryStorage
		Dim pItems          As IEnumStyleGalleryItem
		Dim pStylePath      As String
		Dim pSI             As IStyleGalleryItem

		Set SG = New StyleGallery
		Set pSGS = SG

		pStylePath = pSGS.DefaultStylePath + "ESRI.Style"
		Set pItems = SG.Items("Scale Bars", pStylePath, "")

		Set pScaleBarUp = Nothing
		Set pScaleBarDn = Nothing

		pItems.Reset
		Set pSI = pItems.Next

		While Not (pSI Is Nothing)
		'System.Console.WriteLine (pSI.Name)

		If (pSI.Name = "Scale Line 1") Then
		Set pScaleBarUp = pSI.Item 'as IScaleBar
		End If

		If (pSI.Name = "Scale Line 3") Then
		Set pScaleBarDn = pSI.Item 'as IScaleBar
		End If

		Set pSI = pItems.Next
		Wend

		Set GetStyleGallery = SG
#Else
		Dim pDocument As ESRI.ArcGIS.ArcMapUI.IMxDocument
		pDocument = GetMap() 'pApplication.Document
		GetStyleGallery = pDocument.StyleGallery
#End If
	End Function

	'Function MyDD2Str(ByVal X As Double, ByVal Mode As Integer) As String
	'	Dim xDeg As Double
	'	Dim xMin As Double
	'	Dim xIMin As Double
	'	Dim xSec As Double
	'	Dim lSign As Integer
	'	Dim sSign As String
	'	Dim sTmp As String

	'	lSign = System.Math.Sign(X)
	'	X = System.Math.Abs(X)
	'	X = Modulus(X, 360.0)

	'	sSign = ""
	'	If lSign < 0 Then sSign = "-"
	'	xDeg = Int(X)

	'	Select Case Mode
	'		Case 0
	'			Return sSign + Str(System.Math.Round(X, 6)) + "°"
	'		Case 1
	'			sTmp = sSign + Str(xDeg) + "°"
	'			xMin = System.Math.Round((X - xDeg) * 60.0, 4)
	'			Return sTmp + Str(xMin) + "'"
	'	End Select
	'	xDeg = Int(X)
	'	sTmp = sSign + Str(xDeg) + "°"
	'	xMin = (X - xDeg) * 60.0
	'	xIMin = Int(xMin)
	'	sTmp = sTmp + Str(xIMin) + "'"
	'	xSec = (xMin - xIMin) * 60.0
	'	Return sTmp + Str(System.Math.Round(xSec, 2)) + """"
	'	'End Select
	'End Function

	'Sub DD2Str(ByVal xDeg As Double, ByVal yDeg As Double, ByRef Xstr As String, ByRef Ystr As String, ByRef LonSide As String, ByRef LatSide As String)
	'	Dim xMin As Double
	'	Dim xSec As Double
	'	Dim yMin As Double
	'	Dim ySec As Double

	'	Dim xSign As Short
	'	Dim ySign As Short

	'	xSign = System.Math.Sign(xDeg)
	'	ySign = System.Math.Sign(yDeg)

	'	DD2DMS(xDeg, xMin, xSec, xSign, 360)
	'	DD2DMS(yDeg, yMin, ySec, ySign, 0)

	'	DMS2Str(xDeg, xMin, xSec, yDeg, yMin, ySec, Xstr, Ystr, LonSide, LatSide)
	'End Sub

	'Sub DD2DMS(ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	Dim X As Double
	'	Dim dX As Double

	'	X = System.Math.Abs(xDeg) * Sign
	'	X = System.Math.Round(X, 10)

	'	If (nMod > 0) Then
	'		X = RealMode(X, nMod)
	'	End If

	'	Sign = System.Math.Sign(X)

	'	X = System.Math.Abs(X)
	'	xDeg = Fix(X)
	'	dX = (X - xDeg) * 60
	'	dX = System.Math.Round(dX, 8)
	'	xMin = Fix(dX)
	'	xSec = (dX - xMin) * 60
	'	xSec = System.Math.Round(xSec, 6)
	'End Sub

	'Sub DD2DM(ByRef xDeg As Double, ByRef xMin As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	Dim X As Double
	'	X = System.Math.Abs(xDeg) * Sign
	'	X = System.Math.Round(X, 10)

	'	If (nMod > 0) Then
	'		X = RealMode(X, nMod)
	'	End If
	'	Sign = System.Math.Sign(X)
	'	X = System.Math.Abs(X)
	'	xDeg = Fix(X)
	'	xMin = (X - xDeg) * 60
	'	xMin = System.Math.Round(xMin, 8)
	'End Sub

	'Sub DM2DD(ByRef xDeg As Double, ByRef xMin As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	Dim X As Double
	'	X = System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60)
	'	X = X * Sign
	'	X = System.Math.Round(X, 10)
	'	If (nMod > 0) Then
	'		X = RealMode(X, nMod)
	'	End If
	'	xDeg = System.Math.Abs(X)
	'	Sign = System.Math.Sign(X)
	'End Sub

	'Sub DM2DMS(ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	xDeg = (System.Math.Abs(xDeg) + System.Math.Abs(xMin) / 60)
	'	xDeg = System.Math.Round(xDeg, 10)
	'	DD2DMS(xDeg, xMin, xSec, Sign, nMod)
	'End Sub

	'Sub DMS2DM(ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	xDeg = (System.Math.Abs(xDeg) + System.Math.Abs(xMin) / 60 + System.Math.Abs(xSec) / 3600)
	'	xDeg = System.Math.Round(xDeg, 10)
	'	DD2DM(xDeg, xMin, Sign, nMod)
	'End Sub

	'Sub DMS2DD(ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, ByRef Sign As Short, ByRef nMod As Short)
	'	Dim X As Double
	'	X = System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60) + System.Math.Abs(xSec / 3600)
	'	X = X * Sign
	'	X = System.Math.Round(X, 10)
	'	If (nMod > 0) Then
	'		X = RealMode(X, nMod)
	'	End If
	'	xDeg = System.Math.Abs(X)
	'	Sign = System.Math.Sign(X)
	'End Sub

	'Sub DMS2Str(ByRef xDeg As Double, ByRef xMin As Double, ByVal xSec As Double, ByRef yDeg As Double, ByRef yMin As Double, ByVal ySec As Double, ByRef Xstr As String, ByRef Ystr As String, ByRef LonSide As String, ByRef LatSide As String)
	'	Dim xDegStr As String
	'	Dim xMinStr As String
	'	Dim xSecStr As String
	'	Dim yDegStr As String
	'	Dim yMinStr As String
	'	Dim ySecStr As String

	'	If (xDeg < 10) Then
	'		xDegStr = "00" + CStr(xDeg)
	'	ElseIf (xDeg < 100) Then
	'		xDegStr = "0" + CStr(xDeg)
	'	Else
	'		xDegStr = CStr(xDeg)
	'	End If
	'	xDegStr = xDegStr + "°"

	'	If (xMin < 10) Then
	'		xMinStr = "0" + CStr(xMin)
	'	Else
	'		xMinStr = CStr(xMin)
	'	End If
	'	xMinStr = xMinStr + "'"
	'	xSec = System.Math.Round(xSec * 100) / 100

	'	If (xSec < 10) Then
	'		xSecStr = "0" + CStr(Fix(xSec))
	'	Else
	'		xSecStr = CStr(Fix(xSec))
	'	End If
	'	xSecStr = xSecStr + "."

	'	xSec = System.Math.Round((xSec - Fix(xSec)) * 100)

	'	If (xSec < 10) Then
	'		xSecStr = xSecStr + "0" + CStr(xSec)
	'	Else
	'		xSecStr = xSecStr + CStr(xSec)
	'	End If
	'	xSecStr = xSecStr + "''" + LonSide
	'	Xstr = xDegStr + xMinStr + xSecStr


	'	If (yDeg < 10) Then
	'		yDegStr = "0" + CStr(yDeg)
	'	Else
	'		yDegStr = CStr(yDeg)
	'	End If
	'	yDegStr = yDegStr + "°"

	'	If (yMin < 10) Then
	'		yMinStr = "0" + CStr(yMin)
	'	Else
	'		yMinStr = CStr(yMin)
	'	End If
	'	yMinStr = yMinStr + "'"

	'	ySec = System.Math.Round(ySec * 100) / 100

	'	If (ySec < 10) Then
	'		ySecStr = "0" + CStr(Fix(ySec))
	'	Else
	'		ySecStr = CStr(Fix(ySec))
	'	End If
	'	ySecStr = ySecStr + "."
	'	ySec = System.Math.Round((ySec - Fix(ySec)) * 100)

	'	If (ySec < 10) Then
	'		ySecStr = ySecStr + "0" + CStr(ySec)
	'	Else
	'		ySecStr = ySecStr + CStr(ySec)
	'	End If
	'	ySecStr = ySecStr + "''" + LatSide
	'	Ystr = yDegStr + yMinStr + ySecStr

	'End Sub

	'Function LatNorm(ByRef Y As Double) As String
	'	If (Y < 0.0) Then
	'		LatNorm = "S"
	'		Y = -Y
	'	Else
	'		LatNorm = "N"
	'	End If
	'End Function

	'Function LonNorm(ByRef X As Double) As String
	'	If (X > 180.0) Then
	'		LonNorm = "W"
	'		X = 360.0 - X
	'	Else
	'		LonNorm = "E"
	'	End If
	'End Function

	Public Sub shall_SortfSort(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I

				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).fSort > TempVal.fSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop

				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortfSortD(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I

				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).fSort < TempVal.fSort

					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop

				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSort(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I

				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).sSort > TempVal.sSort

					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop

				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSortD(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I

				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).sSort < TempVal.sSort

					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop

				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	'Private Sub SortByDist(ByRef A() As TypeDefinitions.ObstacleType, ByRef iLo As Short, ByRef iHi As Short)
	'	Dim Midle As Double
	'	Dim Lo As Short
	'	Dim Hi As Short
	'	Dim t As TypeDefinitions.ObstacleType

	'	Lo = iLo
	'	Hi = iHi
	'	Midle = A((Lo + Hi) / 2).Dist
	'	Do
	'		While A(Lo).Dist < Midle
	'			Lo += 1
	'		End While

	'		While A(Hi).Dist > Midle
	'			Hi -= 1
	'		End While

	'		If (Lo <= Hi) Then
	'			t = A(Lo)
	'			A(Lo) = A(Hi)
	'			A(Hi) = t

	'			Lo += 1
	'			Hi -= 1
	'		End If
	'	Loop Until Lo > Hi

	'	If (Hi > iLo) Then SortByDist(A, iLo, Hi)
	'	If (Lo < iHi) Then SortByDist(A, Lo, iHi)
	'End Sub

	'Private Sub SortByTurnDist(ByRef A() As TypeDefinitions.ObstacleType, ByRef iLo As Short, ByRef iHi As Short)
	'	Dim t As TypeDefinitions.ObstacleType
	'	Dim Midle As Double
	'	Dim Lo As Short
	'	Dim Hi As Short

	'	Lo = iLo
	'	Hi = iHi
	'	Midle = A((Lo + Hi) / 2).TurnDist

	'	Do
	'		While A(Lo).TurnDist < Midle
	'			Lo = Lo + 1
	'		End While

	'		While A(Hi).TurnDist > Midle
	'			Hi = Hi - 1
	'		End While

	'		If (Lo <= Hi) Then
	'			t = A(Lo)
	'			A(Lo) = A(Hi)
	'			A(Hi) = t

	'			Lo = Lo + 1
	'			Hi = Hi - 1
	'		End If
	'	Loop Until Lo > Hi

	'	If (Hi > iLo) Then SortByTurnDist(A, iLo, Hi)
	'	If (Lo < iHi) Then SortByTurnDist(A, Lo, iHi)
	'End Sub

	'Private Sub SortByReqH(ByRef A() As TypeDefinitions.ObstacleType, ByRef iLo As Short, ByRef iHi As Short)
	'	Dim t As TypeDefinitions.ObstacleType
	'	Dim Midle As Double
	'	Dim Lo As Short
	'	Dim Hi As Short

	'	Lo = iLo
	'	Hi = iHi
	'	Midle = A((Lo + Hi) / 2).ReqH

	'	Do
	'		While A(Lo).ReqH > Midle
	'			Lo = Lo + 1
	'		End While
	'		While A(Hi).ReqH < Midle
	'			Hi = Hi - 1
	'		End While
	'		If (Lo <= Hi) Then
	'			t = A(Lo)
	'			A(Lo) = A(Hi)
	'			A(Hi) = t

	'			Lo = Lo + 1
	'			Hi = Hi - 1
	'		End If
	'	Loop Until Lo > Hi

	'	If (Hi > iLo) Then SortByReqH(A, iLo, Hi)
	'	If (Lo < iHi) Then SortByReqH(A, Lo, iHi)
	'End Sub

	'Sub Sort(ByRef A() As TypeDefinitions.ObstacleType, ByRef SortIx As Integer)
	'	Dim Lo As Short
	'	Dim Hi As Short

	'	Lo = LBound(A)
	'	Hi = UBound(A)

	'	If (Lo >= Hi) Then Exit Sub

	'	Select Case SortIx
	'		Case 0
	'			SortByDist(A, Lo, Hi)
	'		Case 1
	'			SortByTurnDist(A, Lo, Hi)
	'		Case 2
	'			SortByReqH(A, Lo, Hi)
	'	End Select
	'End Sub

	'Sub GetObstInRange(ByVal ObstSource() As TypeDefinitions.ObstacleType, ByRef ObstDest() As TypeDefinitions.ObstacleType, ByVal Range As Double)
	'	Dim N As Integer
	'	Dim I As Integer
	'	Dim J As Integer

	'	N = UBound(ObstSource)
	'	ReDim ObstDest(N)
	'	If N < 0 Then Return

	'	J = -1
	'	For I = 0 To N
	'		If ObstSource(I).Dist > Range Then Exit For
	'		J = J + 1
	'		ObstDest(J) = ObstSource(I)
	'	Next I

	'	If J < 0 Then
	'		ReDim ObstDest(-1)
	'	Else
	'		ReDim Preserve ObstDest(J)
	'	End If
	'End Sub

	'Sub RefreshCommandBar(ByRef mBar As Framework.ICommandBar, Optional ByRef iFlag As Integer = &HFFFF)
	'	Dim I As Integer
	'	Dim J As Integer

	'	J = 1
	'	For I = 0 To mBar.Count - 1
	'		If (iFlag And J) <> 0 Then
	'			mBar.Item(I).Refresh()
	'		End If
	'		J = J * 2
	'	Next
	'End Sub
End Module