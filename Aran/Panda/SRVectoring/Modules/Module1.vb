Option Strict Off
Option Explicit On
Module Module1

	Public Const EnabledColor As Integer = &H80000005
	Public Const DisabledColor As Integer = &H8000000F

	Public Const WritableColor As Integer = &H80000005
	Public Const ReadOnlyColor As Integer = &H8000000F
	
	'	Public Function GetSelectedOptionButton(ByRef vOptionButton As Object) As Short
	'        On Error GoTo A
	'        Dim I As Short

	'        For I = vOptionButton.LBound To vOptionButton.UBound
	'            If vOptionButton(I).Value Then
	'                GetSelectedOptionButton = I
	'                Exit Function
	'            End If
	'        Next I
	'A: 
	'		GetSelectedOptionButton = -1
	'    End Function
	
	'Public Function GetEnableColor(ByVal vEnable As Boolean) As OLE_COLOR
	'    If vEnable Then
	'        GetEnableColor = EnabledColor
	'    Else
	'        GetEnableColor = DisabledColor
	'    End If
	'
	'End Function
	
	'Public Sub SetReadOnly(tb As VB.TextBox, ByVal isReadOnly As Boolean)
	'    tb.Locked = isReadOnly
	'    tb.BackColor = IIf(vEnable, EnabledColor, DisabledColor)
	'End Sub
	'
	'Public Sub SetComboBoxReadOnly(cb As VB.ComboBox, ByVal isReadOnly As Boolean)
	'    cb.Locked = isReadOnly
	'    cb.BackColor = IIf(isReadOnly, WritableColor, ReadOnlyColor)
	'    'cb.Style = IIf(isReadOnly, 1, 2)
	'End Sub
	
	Public Sub SetReadOnly(ByRef ctrl As System.Windows.Forms.Control, ByVal isReadOnly As Boolean)
		ctrl.Enabled = Not isReadOnly
		If ctrl.Enabled Then
			ctrl.BackColor = System.Drawing.ColorTranslator.FromOle(IIf(isReadOnly, ReadOnlyColor, WritableColor))
		Else
			ctrl.BackColor = System.Drawing.ColorTranslator.FromOle(DisabledColor)
		End If
		'    If TypeOf ctrl Is ComboBox Then ctrl.Style = IIf(isReadOnly, 1, 2)
	End Sub
	
	Public Sub EnableControl(ByRef ctrl As System.Windows.Forms.Control, ByVal vEnable As Boolean)
		ctrl.Enabled = vEnable

		If Not ((TypeOf ctrl Is System.Windows.Forms.Label) Or (TypeOf ctrl Is System.Windows.Forms.RadioButton)) Then
			ctrl.BackColor = System.Drawing.ColorTranslator.FromOle(IIf(vEnable, EnabledColor, DisabledColor))
		End If

		'    If TypeOf ctrl Is TextBox Then ctrl.BackColor = IIf(vEnable, EnabledColor, DisabledColor)
	End Sub
	
	'Public Function ExitEvent(ByRef ctrl As Object) As Boolean
	'       ExitEvent = ctrl.Tag = "N"
	'       If ExitEvent Then ctrl.Tag = ""
	'End Function
	
	'Public Function GetAngleInterval_Last( _
	''            ByVal ptLine As IPoint, _
	''            ByVal dirLine As Double, _
	''            ByVal maxDist As Double, _
	''            ByVal r As Double, _
	''            ByVal ptNav As IPoint, _
	''            ByVal turnSide As Long) As MinMaxDist
	'
	'    On Error GoTo ErrHnd
	'
	'    If (Abs(ptLine.X - ptNav.X) < distEps) And (Abs(ptLine.Y - ptNav.Y) < distEps) Then
	'        GetAngleInterval_Last.Count = 0
	'        Exit Function
	'    End If
	'
	'Dim ptC1 As IPoint
	'Dim ptC2 As IPoint
	'Dim min1 As Double, min2  As Double, max1  As Double, max2 As Double, D As Double
	'
	'    Set ptC1 = PointAlongPlane(ptLine, dirLine - turnSide * 90.0, r)
	'    Set ptC2 = PointAlongPlane( _
	''                    PointAlongPlane(ptLine, dirLine, maxDist), _
	''                    dirLine - turnSide * 90.0, r)
	'
	''------
	'    If ReturnDistanceInMeters(ptC1, ptNav) < r Then
	'        D = CircleVectorIntersect(ptNav, r + 1, ptC1, dirLine, ptC1)
	'        If D > maxDist Then
	'        End If
	'    End If
	'
	'    If ReturnDistanceInMeters(ptC2, ptNav) < r Then
	'        D = CircleVectorIntersect(ptNav, r + 1, ptC2, Modulus(dirLine + 180.0, 360.0), ptC2)
	'        If D > maxDist Then
	'        End If
	'    End If
	''------
	'
	'Dim sideL As Long
	'    sideL = SideDef(ptLine, dirLine, ptNav)
	'    If sideL = 0 Then sideL = 1
	'
	'    min1 = A(ptC1, r, ptNav, turnSide, True, turnSide, sideL)
	'    max1 = A(ptC2, r, ptNav, turnSide, False, turnSide, sideL)
	'
	'    min2 = A(ptC1, r, ptNav, turnSide, True, -turnSide, sideL)
	'    max2 = A(ptC2, r, ptNav, turnSide, False, -turnSide, sideL)
	'
	'Dim t As MinMaxDist
	''    t.Count = 2
	'    t.MinFrom = AddMagSc(min1)
	'    t.MaxFrom = AddMagSc(max1)
	'    t.MinTo = AddMagSc(min2)
	'    t.MaxTo = AddMagSc(max2)
	''    t.isClockwise = (sideL > 0)
	'
	'    GetAngleInterval_Last = t
	'    Exit Function
	'ErrHnd:
	'    GetAngleInterval_Last.Count = 0
	'End Function
	
	'Private Function GetAngleFromTanglet_Azmt(ByRef ptCen As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint, ByVal tangSide As Integer, ByVal TurnSide As Integer) As Double
	'	Dim B As Integer
	'	B = IIf(TurnSide * tangSide > 0, 1, 0)

	'	GetAngleFromTanglet_Azmt = Dir2Azt(ptOut, Modulus(GetTangentAngle(ptCen, R, ptOut, tangSide) + B * 180, 360.0))
	'End Function
	
	'Public Function GetAngleInside(ByVal minAngle As Double, ByVal maxAngle As Double, ByVal valAngle As Double) As Boolean
	'	minAngle = Modulus(minAngle, 360.0)
	'	maxAngle = Modulus(maxAngle, 360.0)

	'	If minAngle < maxAngle Then
	'		GetAngleInside = (valAngle >= minAngle And valAngle <= maxAngle)
	'	Else
	'		GetAngleInside = (valAngle >= minAngle And valAngle <= 0) Or (valAngle >= 0 And valAngle <= maxAngle)
	'	End If
	'End Function
	
	'Public Function A(ByRef ptCen As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint, ByVal TurnSide As Integer, ByVal near As Boolean, ByVal tangSide As Integer, ByVal sideL As Integer) As Double
	'	Dim ptI As ESRI.ArcGIS.Geometry.IPoint
	'	Dim crySide As Integer
	'	Dim dirTang As Double
	'	Dim B As Integer

	'	ptI = TangentCrycleIntersectPoint(ptCen, R, ptOut, tangSide)

	'	If ptI Is Nothing Then Return 0.0

	'	crySide = SideDef(ptI, ReturnAngleInDegrees(ptCen, ptI), ptOut)
	'	B = IIf((crySide * TurnSide) = 1, 1, 0)
	'	dirTang = Modulus(ReturnAngleInDegrees(ptOut, ptI) + B * 180.0, 360.0)
	'	B = IIf(near, sideL, -sideL)
	'	Return System.Math.Round(Dir2Azt(ptOut, dirTang) + (B * 0.4999999))
	'End Function
	
	'Public Sub GetAngleInterval(ByVal ptLine As ESRI.ArcGIS.Geometry.IPoint, ByVal azmt As Double, ByVal L As Double, ByVal R As Double, ByVal ptNav As ESRI.ArcGIS.Geometry.IPoint, ByVal sd As Integer, ByRef a0 As Double, ByRef a1 As Double)
	'	Dim sid As Integer
	'	Dim sid2 As Integer
	'	Dim pt As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptC1 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptC2 As ESRI.ArcGIS.Geometry.IPoint

	'	If sd >= 1 Then
	'		sd = 1
	'	Else
	'		sd = -1
	'	End If

	'	sid = SideDef(ptLine, azmt, ptNav)
	'	ptC1 = PointAlongPlane(ptLine, azmt - sd * 90.0, R)
	'	ptC2 = PointAlongPlane(PointAlongPlane(ptLine, azmt, L), azmt - sd * 90.0, R)
	'	pt = PointAlongPlane(ptLine, azmt - sd * 90.0, 2 * R)
	'	sid2 = SideDef(pt, azmt, ptNav)

	'	If sd = 1 Then
	'		If sid = -1 Then
	'			a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, -1), 360.0))
	'			a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC2, R, ptNav, -1), 360.0))
	'		Else
	'			If sid2 = -1 Then
	'				a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, 1) - 180.0, 360.0))
	'				a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, -1), 360.0))
	'			Else
	'				a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, 1) - 180.0, 360.0))
	'				a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC2, R, ptNav, 1) - 180.0, 360.0))
	'			End If
	'		End If
	'	Else
	'		If sid = 1 Then
	'			a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, 1), 360.0))
	'			a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC2, R, ptNav, 1), 360.0))
	'		Else
	'			If sid2 = 1 Then
	'				a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, -1) - 180.0, 360.0))
	'				a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, 1), 360.0))
	'			Else
	'				a1 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC1, R, ptNav, -1) - 180.0, 360.0))
	'				a0 = Dir2Azt(ptLine, Modulus(GetTangentAngle(ptC2, R, ptNav, -1) - 180.0, 360.0))
	'			End If
	'		End If
	'	End If

	'	a0 = System.Math.Round(a0 + 0.4999999)
	'	a1 = System.Math.Round(a1 - 0.4999999)
	'End Sub
	
	'Public Function GetInsideInterval(ByVal MinVal As Double, ByVal MaxVal As Double, ByRef Val_Renamed As Double) As Boolean
	'	GetInsideInterval = True
	'	If MinVal <= MaxVal Then
	'		If Val_Renamed < MinVal Or Val_Renamed > MaxVal Then
	'			GetInsideInterval = False
	'			If Val_Renamed < MinVal Then
	'				Val_Renamed = MinVal
	'			Else
	'				Val_Renamed = MaxVal
	'			End If
	'		End If
	'	Else
	'		If Val_Renamed < MinVal And Val_Renamed > MaxVal Then
	'			GetInsideInterval = False
	'			If (MinVal - Val_Renamed) < (Val_Renamed - MaxVal) Then
	'				Val_Renamed = MinVal
	'			Else
	'				Val_Renamed = MaxVal
	'			End If
	'		End If
	'	End If
	'End Function
	
	'Private Function GetTangentAngle(ByRef ptCen As ESRI.ArcGIS.Geometry.IPoint, ByRef R As Double, ByRef pPoint As ESRI.ArcGIS.Geometry.IPoint, ByVal pSide As Integer) As Double
	'       Dim a1, dir2, dir1, D As Double

	'	D = ReturnDistanceInMeters(pPoint, ptCen)

	'	If D < R Then Err.Raise(55055)

	'	a1 = RadToDeg(ArcSin(R / D))
	'	dir1 = ReturnAngleInDegrees(pPoint, ptCen)

	'       dir2 = dir1 - pSide * (a1 + 90.0)
	'	dir1 = dir1 - pSide * a1

	'	Dim pt As ESRI.ArcGIS.Geometry.IPoint
	'       pt = LineLineIntersect(pPoint, dir1, ptCen, dir2)

	'	GetTangentAngle = ReturnAngleInDegrees(pPoint, pt)
	'End Function
	
	'Private Function TangentCrycleIntersectPoint(ByRef ptCen As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByRef pPoint As ESRI.ArcGIS.Geometry.IPoint, ByVal pSide As Integer) As ESRI.ArcGIS.Geometry.IPoint
	'	Dim D, dir1, dir2, a1 As Double

	'	D = ReturnDistanceInMeters(pPoint, ptCen)

	'	If D < R Then Return Nothing

	'	a1 = RadToDeg(ArcSin(R / D))
	'	dir1 = ReturnAngleInDegrees(pPoint, ptCen)
	'	dir2 = dir1 - pSide * (a1 + 90.0)
	'	dir1 = dir1 - pSide * a1

	'	Return LineLineIntersect(pPoint, dir1, ptCen, dir2)
	'   End Function
	
	'Private Sub IsOutPointInsideCrycle(ByRef ptCen As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByVal dirLine As Double, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint, ByVal back As Boolean)
	'	If ReturnDistanceInMeters(ptCen, ptOut) <= R Then
	'		Call CircleVectorIntersect(ptOut, R + 1, ptCen, IIf(back, Modulus(dirLine + 180.0, 360.0), dirLine), ptCen)
	'	End If
	'End Sub
	
	'Public Function GetInclineMinMaxDisFromDME( _
	''            ByVal ptCur As IPoint, _
	''            ByVal dirCur As Double, _
	''            ByVal GRD As Double, _
	''            ByVal ptDME As IPoint, _
	''            ByVal dmeMaxDis As Double, _
	''            ByVal dmeMinAngle As Double, _
	''            ByVal MaxAltitude As Double, _
	''            ByVal MinAltitude As Double) As
	'
	'Dim t As tt
	'Dim MinAngleDis As Double
	'Dim DmePtLineDis As Double
	'
	'Dim D As Double
	'Dim h As Double
	'Dim h1 As Double
	'Dim h2 As Double
	'    t.Count = 0
	'
	'    Dim side As Long
	'    side = SideDef(ptCur, dirCur, ptDME)
	'    If side = 0 Then side = 1
	'
	'    Dim ptNear As IPoint
	'    Dim ptFar As IPoint
	'
	'    Set ptNear = LineLineIntersect(ptCur, dirCur, ptDME, dirCur - side * dmeMinAngle)
	'    MinAngleDis = ReturnDistanceInMeters(ptDME, ptNear)
	'    If MinAngleDis > dmeMaxDis Then
	'        GetInclineMinMaxDisFromDME = t
	'        Exit Function
	'    End If
	'    Set ptFar = LineLineIntersect(ptCur, dirCur, ptDME, dirCur + side * dmeMinAngle)
	'
	'    Dim dH As Double
	'    dH = ptCur.Z - ptDME.Z
	'
	'    Dim DistCurDME As Double
	'    Dim DistCurDME3D As Double
	'
	'    DistCurDME = ReturnDistanceInMeters(ptCur, ptDME)
	'    DistCurDME3D = Sqr(Kvd(DistCurDME) + Kvd(dH))
	'
	'    side = SideDef(ptCur, dirCur + 90.0, ptDME)
	'    Dim p As Double
	'
	'    If DistCurDME3D > dmeMaxDis Then
	'        p = IIf(side < 0, 1, 5)
	'    Else
	'        If DistCurDME > MinAngleDis Then
	'            p = IIf(side > 0, 2, 4)
	'        Else
	'            p = 3
	'        End If
	'    End If
	'
	'    Dim MinAngleDisNear As Double
	'    Dim MinAngleDisFar As Double
	'
	'    If p <> 4 Or p <> 5 Then
	'        h = ReturnDistanceInMeters(ptNear, ptCur) * GRD + ptCur.Z
	'        If GRD > 0 Then
	'            If h > MaxAltitude Then h = MaxAltitude
	'        Else
	'            If h < MinAltitude Then h = MinAltitude
	'        End If
	'        h = h - ptDME.Z
	'
	'        MinAngleDisNear = Sqr(Kvd(MinAngleDis) + Kvd(h))
	'
	'        h = ReturnDistanceInMeters(ptFar, ptCur) * GRD + ptCur.Z
	'        If GRD > 0 Then
	'            If h > MaxAltitude Then h = MaxAltitude
	'        Else
	'            If h < MinAltitude Then h = MinAltitude
	'        End If
	'        h = h - ptDME.Z
	'
	'        MinAngleDisFar = Sqr(Kvd(MinAngleDis) + Kvd(h))
	'    End If
	'
	'    Select Case p
	'        Case 1:
	'            t.Count = 2
	'            t.tillMin = ConvertDistance(dmeMaxDis, 1)
	'            t.tillMax = ConvertDistance(MinAngleDisNear, 3)
	'            t.afterMin = ConvertDistance(MinAngleDisFar, 3)
	'            t.afterMax = ConvertDistance(dmeMaxDis, 1)
	'        Case 2:
	'            t.Count = 2
	'            t.tillMin = ConvertDistance(DistCurDME3D, 1)
	'            t.tillMax = ConvertDistance(MinAngleDisNear, 3)
	'            t.afterMin = ConvertDistance(MinAngleDisFar, 3)
	'            t.afterMax = ConvertDistance(dmeMaxDis, 1)
	'        Case 3:
	'            t.Count = 1
	'            t.afterMin = ConvertDistance(MinAngleDisFar, 3)
	'            t.afterMax = ConvertDistance(dmeMaxDis, 1)
	'        Case 4:
	'            t.Count = 1
	'            t.afterMin = ConvertDistance(DistCurDME3D, 3)
	'            t.afterMax = ConvertDistance(dmeMaxDis, 1)
	'        Case 5:
	'            t.Count = 0
	'    End Select
	'
	'    GetInclineMinMaxDisFromDME = t
	'
	'End Function
	
	'Public Function MinMaxDisFromDme( _
	''            ByVal ptLine As IPoint, _
	''            ByVal dir As Double, _
	''            ByVal ptDME As IPoint, _
	''            ByVal dmeMaxDis As Double, _
	''            ByVal dmeMinAngle As Double) As tt
	'
	'Dim t As tt
	'    t.Count = 0
	'
	'Dim side As Long
	'Dim MinAngleDis As Double
	'Dim DmePtLineDis As Double
	'Dim Pos As Long
	'
	''    'if minumum distanse ptDme and Line > dmeMaxDis
	''    If ReturnDistanceInMeters(ptDME, _
	'' '                LineLineIntersect(ptLine, dir, ptDME, dir + 90.0)) > dmeMaxDis Then
	''        MinMaxDisFromDme = t
	''        Exit Function
	''    End If
	'
	'    MinAngleDis = ReturnDistanceInMeters(ptDME, _
	''                LineLineIntersect(ptLine, dir, ptDME, dir + dmeMinAngle))
	'
	'    If MinAngleDis > dmeMaxDis Then
	'        MinMaxDisFromDme = t
	'        Exit Function
	'    End If
	'
	'    DmePtLineDis = ReturnDistanceInMeters(ptLine, ptDME)
	'
	'    If DmePtLineDis > dmeMaxDis Then
	'        Pos = 2
	'    ElseIf DmePtLineDis > MinAngleDis Then
	'        Pos = 1
	'    Else
	'        Pos = 0
	'    End If
	'
	'    side = SideDef(ptLine, dir + 90.0, ptDME)
	'
	'    If Pos = 0 Then
	'        t.Count = 1
	'        t.afterMin = MinAngleDis
	'        t.afterMax = dmeMaxDis
	'    ElseIf Pos = 1 Then
	'        If side = 1 Then
	'            t.Count = 2
	'            t.tillMin = DmePtLineDis
	'            t.tillMax = MinAngleDis
	'            t.afterMin = MinAngleDis
	'            t.afterMax = dmeMaxDis
	'        Else
	'            t.Count = 1
	'            t.afterMin = DmePtLineDis
	'            t.afterMax = dmeMaxDis
	'        End If
	'    Else
	'        If side = 1 Then
	'            t.Count = 2
	'            t.tillMin = dmeMaxDis
	'            t.tillMax = MinAngleDis
	'            t.afterMin = MinAngleDis
	'            t.afterMax = dmeMaxDis
	'        Else
	'            t.Count = 0
	'        End If
	'    End If
	'
	'    If t.Count > 0 Then
	'        side = IIf((t.afterMin < t.afterMax), 1, -1)
	'
	'        If side < 0 Then
	'            t.afterMin = ConvertDistance(t.afterMin, 1)
	'            t.afterMax = ConvertDistance(t.afterMax, 3)
	'        Else
	'            t.afterMin = ConvertDistance(t.afterMin, 3)
	'            t.afterMax = ConvertDistance(t.afterMax, 1)
	'        End If
	'
	'        If t.Count = 2 Then
	'            side = IIf((t.tillMin < t.tillMax), 1, -1)
	'
	'            If side < 0 Then
	'                t.tillMin = ConvertDistance(t.tillMin, 1)
	'                t.tillMax = ConvertDistance(t.tillMax, 3)
	'            Else
	'                t.tillMin = ConvertDistance(t.tillMin, 3)
	'                t.tillMax = ConvertDistance(t.tillMax, 1)
	'            End If
	'        End If
	'    End If
	'
	'    MinMaxDisFromDme = t
	'End Function
	
	'Public Sub MinMaxDisFromDme2( _
	''            ByVal ptLine As IPoint, _
	''            ByVal dir As Double, _
	''            ByVal ptDME As IPoint, _
	''            ByVal dmeMaxDis As Double, _
	''            ByVal dmeMinAngle As Double, _
	''            t As tt)
	'
	'    Dim side As Long
	'    Dim MinAngleDis As Double
	'    Dim DmePtLineDis As Double
	'    Dim Pos As Long
	'
	'    side = SideDef(ptLine, dir + 90.0, ptDME)
	'
	'    'if minumum distanse ptDme and Line > dmeMaxDis
	'    If ReturnDistanceInMeters(ptDME, _
	''                LineLineIntersect(ptLine, dir, ptDME, dir + 90.0)) > dmeMaxDis Then
	'        Exit Sub
	'    End If
	'
	'    MinAngleDis = ReturnDistanceInMeters(ptDME, _
	''                LineLineIntersect(ptLine, dir, ptDME, dir + dmeMinAngle))
	'
	'    If MinAngleDis > dmeMaxDis Then
	'        Exit Sub
	'    End If
	'
	'    DmePtLineDis = ReturnDistanceInMeters(ptLine, ptDME)
	'
	'    If DmePtLineDis > dmeMaxDis Then
	'        Pos = 2
	'    ElseIf DmePtLineDis > MinAngleDis Then
	'        Pos = 1
	'    Else
	'        Pos = 0
	'    End If
	'
	'    If Pos = 0 Then
	'        t.Count = 1
	'        t.afterMin = MinAngleDis
	'        t.afterMax = dmeMaxDis
	'    ElseIf Pos = 1 Then
	'        If side = 1 Then
	'            t.Count = 2
	'            t.tillMin = DmePtLineDis
	'            t.tillMax = MinAngleDis
	'            t.afterMin = MinAngleDis
	'            t.afterMax = dmeMaxDis
	'        Else
	'            t.Count = 1
	'            t.afterMin = DmePtLineDis
	'            t.afterMax = dmeMaxDis
	'        End If
	'    Else
	'        If side = 1 Then
	'            t.Count = 2
	'            t.tillMin = dmeMaxDis
	'            t.tillMax = MinAngleDis
	'            t.afterMin = MinAngleDis
	'            t.afterMax = dmeMaxDis
	'        Else
	'            t.Count = 0
	'        End If
	'    End If
	'
	'    If t.Count > 0 Then
	'        side = IIf((t.afterMin < t.afterMax), 1, -1)
	'
	'        If side < 0 Then
	'            t.afterMin = ConvertDistance(t.afterMin, 1)
	'            t.afterMax = ConvertDistance(t.afterMax, 3)
	'        Else
	'            t.afterMin = ConvertDistance(t.afterMin, 3)
	'            t.afterMax = ConvertDistance(t.afterMax, 1)
	'        End If
	'
	'        If t.Count = 2 Then
	'            side = IIf((t.tillMin < t.tillMax), 1, -1)
	'
	'            If side < 0 Then
	'                t.tillMin = ConvertDistance(t.tillMin, 1)
	'                t.tillMax = ConvertDistance(t.tillMax, 3)
	'            Else
	'                t.tillMin = ConvertDistance(t.tillMin, 3)
	'                t.tillMax = ConvertDistance(t.tillMax, 1)
	'            End If
	'        End If
	'    End If
	'End Sub
	
	'Public Function GetValidAnglesAs_Azmt( _
	''            ptLine As IPoint, _
	''            ByVal dir As Double, _
	''            ptOut As IPoint, _
	''            ByVal Dist As Double) As tt
	'
	'    Dim t As tt
	'    Dim dis As Double
	'    Dim sideX As Long
	'    Dim side As Long
	'    Dim ptRes As IPoint
	'    Dim D As Double
	'    Dim vMin As Double
	'    Dim vMax As Double
	'
	'    dis = ReturnDistanceInMeters(ptLine, ptOut)
	'    sideX = SideDef(ptLine, dir + 90.0, ptOut)
	'
	'    D = CircleVectorIntersect(ptOut, Dist, ptLine, dir, ptRes)
	'
	'    If D = -1 Then
	'        t.Count = 0
	'        GetValidAnglesAs_Azmt = t
	'        Exit Function
	'    End If
	'
	'    vMax = ReturnAngleInDegrees(ptOut, ptRes)
	'    Call CircleVectorIntersect(ptOut, Dist, ptLine, dir + 180.0, ptRes)
	'    vMin = ReturnAngleInDegrees(ptOut, ptRes)
	'
	'    If dis > Dist Then
	'        If sideX = -1 Then
	'            t.Count = 0
	'            GetValidAnglesAs_Azmt = t
	'            Exit Function
	'        Else
	'            t.Count = 1
	'            t.afterMin = vMin
	'            t.afterMax = vMax
	'        End If
	'    Else
	'        vMin = ReturnAngleInDegrees(ptOut, ptLine)
	'        t.Count = 1
	'        t.afterMin = vMin
	'        t.afterMax = vMax
	'    End If
	'
	'    side = SideDef(ptLine, dir, ptOut)
	'    If side = 0 Then side = -1
	'
	'    t.isClockwise = (side > 0)
	'
	'    t.afterMin = Round(AddMagSc(Dir2Azt(ptOut, t.afterMin) + (side * 0.4999999)))
	'    t.afterMax = Round(AddMagSc(Dir2Azt(ptOut, t.afterMax) - (side * 0.4999999)))
	'
	'    GetValidAnglesAs_Azmt = t
	'End Function
	
	'Public Function ttToString(t As tt, ByVal til_aft As Long, MinVal As String) As String
	'
	'    If til_aft = 0 Then
	'        MinVal = t.tillMin
	'        ttToString = FromToText(t.tillMin, t.tillMax)
	'    Else
	'        MinVal = t.afterMin
	'        ttToString = FromToText(t.afterMin, t.afterMax)
	'    End If
	'End Function
	
	'Public Function IsInsidePlantyWithDirection(ByVal min1 As Double, ByVal max1 As Double, ByVal min2 As Double, ByVal max2 As Double, ByVal isClockwise As Boolean, ByVal Val_Renamed As Double, Optional ByRef NewVal As Double = 0.0) As Boolean
	'	Dim nv1 As Double
	'	Dim nv2 As Double

	'	If Not IsInsideWithDirection(min1, max1, Val_Renamed, isClockwise, nv1) Then
	'		If Not IsInsideWithDirection(min2, max2, Val_Renamed, isClockwise, nv2) Then
	'			If System.Math.Abs(Val_Renamed - nv1) < System.Math.Abs(Val_Renamed - nv2) Then
	'				NewVal = nv1
	'			Else
	'				NewVal = nv2
	'			End If
	'			Return False
	'		End If
	'	End If

	'	Return True
	'End Function
	
	'Public Function IsInsidePlanty(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal Val_Renamed As Double, Optional ByRef NewVal As Double = 0.0) As Boolean
	'	Dim nv1 As Double
	'	Dim nv2 As Double

	'	If Not IsInside(x1, y1, Val_Renamed, nv1) Then
	'		If Not IsInside(x2, y2, Val_Renamed, nv2) Then
	'			If System.Math.Abs(Val_Renamed - nv1) < System.Math.Abs(Val_Renamed - nv2) Then
	'				NewVal = nv1
	'			Else
	'				NewVal = nv2
	'			End If
	'			Return False
	'		End If
	'	End If

	'	Return True
	'End Function
	
	'Public Function IsInside(ByVal MinVal As Double, ByVal MaxVal As Double, ByVal Val_Renamed As Double, Optional ByRef NewVal As Double = 0.0) As Boolean
	'	Dim x0, x1 As Double

	'	If MinVal < MaxVal Then
	'		x0 = MinVal
	'		x1 = MaxVal
	'	Else
	'		x0 = MaxVal
	'		x1 = MinVal
	'	End If

	'	If (Val_Renamed >= x0 And Val_Renamed <= x1) Then Return True

	'	If System.Math.Abs(Val_Renamed - x0) > System.Math.Abs(Val_Renamed - x1) Then
	'		NewVal = x1
	'	Else
	'		NewVal = x0
	'	End If

	'	Return False
	'End Function

	'Public Function IsInsideWithDirection(ByVal fromRad As Double, ByVal toRad As Double, ByVal Val_Renamed As Double, ByVal isClockwise As Boolean, Optional ByRef NewVal As Double = 0.0) As Boolean
	'	Dim a1 As Double
	'	Dim a2 As Double
	'	Dim Direction As Integer
	'	Direction = IIf(isClockwise, 1, -1)

	'	a1 = Modulus(SubtractAnglesWithSign(toRad, fromRad, -Direction), 360.0)
	'	a2 = Modulus(SubtractAnglesWithSign(toRad, Val_Renamed, -Direction), 360.0)

	'	Dim a3 As Double
	'	If a2 < 0 Or a1 < a2 Then
	'		a3 = Modulus(SubtractAnglesWithSign(Val_Renamed, fromRad, Direction), 360.0)
	'		If a3 < a2 Then
	'			NewVal = fromRad
	'		Else
	'			NewVal = toRad
	'		End If

	'		Return False
	'	End If

	'	Return True
	'End Function
	
	'Public Function FromToText(ByVal vFrom As Double, ByVal vTo As Double, Optional ByVal symbol As String = "") As String
	'    FromToText = My.Resources.str1110 + " " + CStr(vFrom) + symbol + " " + My.Resources.str1111 + " " + CStr(vTo) + symbol
	'End Function


	'Public Function ConvertCordinate(ByRef ptCordStart As ESRI.ArcGIS.Geometry.IPoint, ByVal dir_Renamed As Double, ByRef pPoint As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.IPoint
	'	Dim sdY As Integer
	'	Dim sdX As Integer
	'	Dim X As Double
	'	Dim Y As Double
	'	Dim pClone As ESRI.ArcGIS.esriSystem.IClone

	'	sdX = SideDef(ptCordStart, dir_Renamed + 90.0, pPoint)
	'	sdY = SideDef(ptCordStart, dir_Renamed + 180.0, pPoint)

	'	Y = sdY * Point2LineDistancePrj(pPoint, ptCordStart, dir_Renamed)
	'	X = sdX * Point2LineDistancePrj(pPoint, ptCordStart, dir_Renamed + 90.0)

	'	pClone = pPoint
	'	ConvertCordinate = pClone.Clone
	'	ConvertCordinate.PutCoords(X, Y)
	'End Function
	
	'Public Function Square(ByVal X As Double) As Double
	'	Return X * X
	'End Function

	'Public Function DME_InclineToProject(ByRef ptCur As ESRI.ArcGIS.Geometry.IPoint, ByVal dirCur As Double, ByVal curH As Double, ByVal GRD As Double, ByRef ptDME As ESRI.ArcGIS.Geometry.Point, ByVal dmeH As Double, ByVal distDME As Double, ByVal isAfter As Boolean, Optional ByRef newDistDME As Double = -1) As Double
	'	Dim Result As Double
	'	Dim dH As Double
	'	Dim tg_a As Double
	'	Dim xND As Double
	'	Dim yND As Double
	'	Dim L As Double
	'	Dim kc As Integer
	'	Dim x0 As Double
	'	Dim x1 As Double

	'	Dim pt As ESRI.ArcGIS.Geometry.IPoint

	'	pt = ConvertCordinate(ptCur, dirCur, ptDME)
	'	xND = pt.X
	'	yND = pt.Y

	'	dH = curH - dmeH
	'	tg_a = GRD
	'	L = distDME

	'	kc = Quadric(1 + Square(tg_a), (2 * dH * tg_a) - (2 * xND), Square(xND) + Square(yND) + Square(dH) - Square(L), x0, x1)

	'	If kc = 0 Then
	'		Result = -1
	'	ElseIf kc = 1 Then
	'		Result = x0
	'	Else
	'		If isAfter Then
	'			Result = Min(x0, x1)
	'		Else
	'			Result = Max(x0, x1)
	'		End If
	'	End If

	'	If newDistDME <> -1 And kc <> 0 Then
	'		xND = pt.X - Result
	'		newDistDME = System.Math.Sqrt(xND * xND + pt.Y * pt.Y)
	'	End If

	'	Return Result
	'End Function
	
	'Public Function GetPoint(ByVal X As Double, ByVal Y As Double) As ESRI.ArcGIS.Geometry.IPoint
	'	GetPoint = New ESRI.ArcGIS.Geometry.Point
	'	GetPoint.PutCoords(X, Y)
	'End Function
End Module