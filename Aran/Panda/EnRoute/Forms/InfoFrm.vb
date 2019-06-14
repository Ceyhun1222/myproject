Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Friend Class CInfoFrm
	Inherits System.Windows.Forms.Form
	Private fD0 As Double
	Private fNearTol As Double
	'Private fTurnAngle As Double
	Private fSegLenght As Double
	Private fStabLenght As Double
	Private fReqStabLenght As Double

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		Label1.Text = ""
		Label2.Text = ""
		Label3.Text = ""
		Label4.Text = ""
		Label5.Text = ""
		Label6.Text = ""
		Label7.Text = ""
		Label8.Text = ""
		Label9.Text = ""
		Label10.Text = ""
	End Sub

	Sub SetGuidNavMsg(ByRef sMessage As String)
		Label2.Text = sMessage
	End Sub

	Sub SetInterNav(ByRef InterNav As TypeDefinitions.NavaidType)
		If InterNav.Index >= 0 Then
			Label4.Text = My.Resources.str2009 + InterNav.Name + "-" + GetNavTypeName(InterNav.TypeCode)
		Else
			Label4.Text = "-"
		End If
	End Sub

	'Sub SetD0(ByVal D0 As Double, ByVal bIsStraight As Boolean)
	'	fD0 = D0
	'	SetStabLenght(bIsStraight)
	'	Label3.Text = My.Resources.str2010 + CStr(ConvertDistance(fD0, 2)) + " " + DistanceConverter(DistanceUnit).Unit
	'End Sub

	Sub SetNearTol(ByVal NearTol As Double, ByVal bIsStraight As Boolean)
		If NearTol >= 0 Then
			fNearTol = NearTol
			SetStabLenght(bIsStraight)
			Label1.Text = My.Resources.str2011 + CStr(ConvertDistance(fNearTol, 2)) + " " + DistanceConverter(DistanceUnit).Unit
		Else
			Label1.Text = "-"
		End If
	End Sub

	Sub SetSegLenght(ByRef SegLenght As Double, ByVal bIsStraight As Boolean)
		fSegLenght = SegLenght
		SetStabLenght(bIsStraight)
		Label5.Text = My.Resources.str2012 + CStr(ConvertDistance(SegLenght, 2)) + " " + DistanceConverter(DistanceUnit).Unit
	End Sub

	Sub SetTurnAngle(ByVal TurnAngle As Double)
		'fTurnAngle = TurnAngle
		Label8.Text = My.Resources.str2013 + CStr(System.Math.Round(TurnAngle, 1)) + " °"
	End Sub

	Sub SetStabLenght(ByVal bIsStraight As Boolean)
		fStabLenght = IIf(bIsStraight, 0.0, fSegLenght - fNearTol - fD0)
		Label6.Text = My.Resources.str2014 + CStr(ConvertDistance(fStabLenght, 2)) + " " + DistanceConverter(DistanceUnit).Unit
	End Sub

	Sub SetReqStabLenght(ByVal ReqStabLenght As Double)
		fReqStabLenght = ReqStabLenght
		Label7.Text = My.Resources.str2015 + CStr(ConvertDistance(fReqStabLenght, 2)) + " " + DistanceConverter(DistanceUnit).Unit
	End Sub

	Sub SetGuidNavReqH(ByVal ReqH As Double)
		Label9.Text = My.Resources.str2016 + CStr(ConvertHeight(ReqH, 2)) + " " + HeightConverter(HeightUnit).Unit
	End Sub

	Sub SetInterNavReqH(ByVal ReqH As Double)
		Label10.Text = My.Resources.str2017 + CStr(ConvertHeight(ReqH, 2)) + " " + HeightConverter(HeightUnit).Unit
	End Sub

	Sub ShowInfo(ByVal X As Integer, ByVal Y As Integer)
		Me.Left = X
		Me.Top = Y
		Me.Show(s_Win32Window)
	End Sub

	Private Sub InfoFrm_Deactivate(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Deactivate
		Me.Hide()
	End Sub

	Private Sub InfoFrm_Paint(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
		Dim points(4) As Point

		points(0).X = 0
		points(0).Y = 0

		points(1).X = ClientRectangle.Width - 1
		points(1).Y = 0

		points(2).X = ClientRectangle.Width - 1
		points(2).Y = ClientRectangle.Height - 1

		points(3).X = 0
		points(3).Y = ClientRectangle.Height - 1

		points(4).X = 0
		points(4).Y = 0

		eventArgs.Graphics.DrawLines(Pens.Black, points)
	End Sub

	Private Sub InfoFrm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim CloseMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		If CloseMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			Me.Visible = False
		End If
		eventArgs.Cancel = Cancel
	End Sub
End Class