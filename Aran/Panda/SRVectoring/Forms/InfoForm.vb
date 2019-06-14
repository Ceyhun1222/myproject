Option Strict Off
Option Explicit On
Friend Class CInfoForm
	Inherits System.Windows.Forms.Form

	Private stValid As Boolean
	Private stHeight As Double
	Private stCourse As Double

	Private stX As Double
	Private stY As Double

	Private endValid As Boolean
	Private endHeight As Double
	Private endCourse As Double
	Private endX As Double
	Private endY As Double

	Private segLen As Double

	Public Sub AddStartPoint(ByRef pPtGeo As ESRI.ArcGIS.Geometry.IPoint)
		stValid = True
		stHeight = pPtGeo.Z
		stCourse = pPtGeo.M
		stX = pPtGeo.X
		stY = pPtGeo.Y
	End Sub

	Public Sub AddEndPoint(ByVal pt As ESRI.ArcGIS.Geometry.IPoint, ByVal Course As Double, ByVal fsLen As Double)
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		endValid = True
		endHeight = pt.Z
		endCourse = Course

		ptTmp = ToGeo(pt)
		endX = ptTmp.X
		endY = ptTmp.Y
		segLen = fsLen

		ptTmp = Nothing
	End Sub

	Public Sub NewSegment()
		stValid = False
		endValid = False
	End Sub

	Public Sub ShowInfo(ByVal MagVar As Double)
		Dim Xstr As String
		Dim Ystr As String

		If Not (stValid And endValid) Then
			MsgBox(My.Resources.str1403)
			Exit Sub
		End If

		If stCourse >= -360.0 Then
			TextBox02.Text = CStr(System.Math.Round(Modulus(stCourse - MagVar), 1))
		Else
			TextBox02.Text = "NO DATA"
		End If

		If endCourse >= -360.0 Then
			TextBox05.Text = CStr(System.Math.Round(Modulus(endCourse - MagVar), 1))
		Else
			TextBox05.Text = "NO DATA"
		End If

		TextBox07.Text = CStr(ConvertDistance(segLen, 2))

		DD2Str(stX, stY, Xstr, Ystr, "E", "N")
		TextBox3.Text = Ystr
		Text1.Text = Xstr

		DD2Str(endX, endY, Xstr, Ystr, "E", "N")
		TextBox06.Text = Ystr
		Text2.Text = Xstr

		Me.Show(s_Win32Window)
		Activate()
	End Sub

	Private Sub InfoForm_Deactivate(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Deactivate
		Me.Hide()
	End Sub

	Private Sub InfoForm_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
		Me.Visible = False
	End Sub

	Private Sub InfoForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		stValid = False
		endValid = False
		Label3.Text = DistanceConverter(DistanceUnit).Unit
	End Sub
End Class