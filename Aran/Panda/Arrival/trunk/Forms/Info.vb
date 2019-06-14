Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ComVisible(False)> Friend Class CInfoFrm
	Inherits System.Windows.Forms.Form

	Private THRValuses(4) As Double
	Private THRFields As Integer

	Private TurnValuses(7) As Double
	Private TurnFields As Integer

	Public Sub SetLocAbeamDist(ByVal LocAbeamDist As Double)
		THRValuses(0) = LocAbeamDist
		THRFields = THRFields Or 1
	End Sub

	Public Sub SetLocAlongDist(ByVal LocAlongDist As Double)
		THRValuses(1) = LocAlongDist
		THRFields = THRFields Or 2
	End Sub

	Public Sub SetDeltaAngle(ByVal dAngle As Double)
		THRValuses(2) = dAngle
		THRFields = THRFields Or 4
	End Sub

	Public Sub SetIntersectDistance(ByVal IntersectDistance As Double)
		THRValuses(3) = IntersectDistance
		THRFields = THRFields Or 8
	End Sub

	Public Sub SetOCHLimit(ByVal OCHLimit As Double)
		THRValuses(4) = OCHLimit
		THRFields = THRFields Or 16
	End Sub

	Public Sub ResetTHRFields()
		THRFields = 0
	End Sub

	Public Sub ShowTHRInfo(ByVal X As Integer, ByVal Y As Integer)
		Dim I As Integer
		Dim F As Integer
		Dim strRes As String

		Dim Str_Renamed As String
		Dim FieldNam() As String = {"THR abeam distance from Localizer course: ", "THR along distance from Localizer: ", "Offset angle: ", "Intersect point distance from THR: ", "OCH lower limit: "}
		Dim FieldUnit() As String = {" m", " m", " °", " m", " " + HeightConverter(HeightUnit).Unit}

		F = 1
		strRes = ""
		For I = 0 To 4
			If THRFields And F Then
				If I = 4 Then
					Str_Renamed = FieldNam(I) + CStr(ConvertHeight(THRValuses(I), eRoundMode.NEAREST)) + FieldUnit(I)
				Else
					Str_Renamed = FieldNam(I) + CStr(System.Math.Round(THRValuses(I), 2)) + FieldUnit(I)
				End If
				strRes = strRes + Str_Renamed + vbCrLf
			End If
			F = F + F
		Next I

		txtInfo.Text = strRes
		txtInfo.Select(0, 0)

		lblInfo.Text = strRes
		lblInfo.AutoSize = False
		lblInfo.AutoSize = True

		Me.Width = lblInfo.Width + 2
		Me.Height = lblInfo.Height + 1

		Me.Show(_Win32Window)
		Me.Left = X
		Me.Top = Y + Height
		'Me.Activate()
	End Sub

	Public Sub SetTAS(ByVal TAS As Double)
		TurnValuses(0) = TAS
		TurnFields = TurnFields Or 1
	End Sub

	Public Sub SetWindSpeed(ByVal WindSpeed As Double)
		TurnValuses(1) = WindSpeed
		TurnFields = TurnFields Or 2
	End Sub

	Public Sub SetRadius(ByVal Radius As Double)
		TurnValuses(2) = Radius
		TurnFields = TurnFields Or 4
	End Sub

	Public Sub SetE(ByVal E As Double)
		TurnValuses(3) = E
		TurnFields = TurnFields Or 8
	End Sub

	Public Sub SetAltitude(ByVal Altitude As Double)
		TurnValuses(4) = Altitude
		TurnFields = TurnFields Or 16
	End Sub

	Public Sub ResetTurnFields()
		TurnFields = 0
	End Sub

	Public Sub ShowTurnInfo(ByVal X As Integer, ByVal Y As Integer)
		Dim I As Integer
		Dim F As Integer
		Dim strRes As String

		Dim Str_Renamed As String
		Dim FieldNam() As String = {"TAS: ", "Wind speed: ", "Radius: ", "E: ", "Altitude: "}

		Dim FieldUnit() As String = {" " + SpeedConverter(SpeedUnit).Unit, " " + SpeedConverter(SpeedUnit).Unit, " " + DistanceConverter(DistanceUnit).Unit, " " + DistanceConverter(DistanceUnit).Unit + "/°", " " + HeightConverter(HeightUnit).Unit}

		F = 1
		strRes = ""
		For I = 0 To 4
			If TurnFields And F Then
				If I < 2 Then
					Str_Renamed = FieldNam(I) + CStr(ConvertSpeed(TurnValuses(I), eRoundMode.NEAREST)) + FieldUnit(I)
				ElseIf I < 4 Then
					Str_Renamed = FieldNam(I) + CStr(ConvertDistance(TurnValuses(I), eRoundMode.NEAREST)) + FieldUnit(I)
				Else
					Str_Renamed = FieldNam(I) + CStr(ConvertHeight(TurnValuses(I), eRoundMode.NEAREST)) + FieldUnit(I)
				End If
				strRes = strRes + Str_Renamed + vbCrLf
			End If
			F = F + F
		Next I

		txtInfo.Text = strRes
		txtInfo.Select(0, 0)

		lblInfo.Text = strRes
		lblInfo.AutoSize = False
		lblInfo.AutoSize = True

		Me.Width = lblInfo.Width + 2
		Me.Height = lblInfo.Height + 1

		Me.Show(_Win32Window)
		Me.Left = X
		Me.Top = Y + Height
		'Me.Activate()
	End Sub

	Private Sub InfoFrm_Deactivate(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Deactivate
		Me.Hide()
	End Sub
End Class
