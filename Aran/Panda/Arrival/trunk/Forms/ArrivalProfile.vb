Option Strict Off
Option Explicit On

Imports Aran.Aim.Enums

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CArrivalProfile
	Inherits System.Windows.Forms.Form

	Private Const LeftMargin As Integer = 10
	Private Const RightMargin As Integer = 10
	Private Const TopMargin As Integer = 10
	Private Const BottomMargin As Integer = 10

	Private Const InnerTopMargin As Integer = 30
	Private Const InnerBottomMargin As Integer = 30
	Private Const InnerLeftMargin As Integer = 100
	Private Const InnerRightMargin As Integer = 100

	Private Const MaxPoints As Integer = 10

	Private Const ArrowLen As Integer = 30
	Private Const ArrowAngle As Integer = 20
	Private Const ArrowWingLen As Integer = 15
	Private Const ArrowWingAngle As Integer = 20

	Private Const TextPadding As Integer = 2
	Private Const LegendWidth As Integer = 60

	Private RWYDir As Integer
	Private RWYLen As Double
	Private RWYAlt As Double
	Private fRefHeight As Double

	Private LeftX As Double
	Private RightX As Double
	Private TopY As Double
	Private BottomY As Double

	Private ScaleWidth As Single
	Private ScaleHeight As Single

	Private ContainerCheck As System.Windows.Forms.CheckBox
	Private Points(MaxPoints - 1) As ProfilePoint

	Public PointsNo As Integer
	Public MAPtIndex As Integer
	Private imgGraphics As Graphics
	Private bFormInitialised As Boolean = False

	'Private Sub Form_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
	'    If (Button = 1) Then
	'        Select Case PointsNo
	'        Case 0
	'            AddPoint 800, 1000
	'        Case 1
	'            AddPoint 1500, 600
	'        Case 2
	'            AddPoint 1100, 600
	'        Case 3
	'            AddPoint 500, 150
	'        Case Else
	'            AddPoint -200, 150, 1
	'        End Select
	'    Else
	'        RemovePoint
	'    End If
	'End Sub
#Region "Form"

	Public Sub New()
		MyBase.New()
		InitializeComponent()

		PictureBox1.Image = New Bitmap(PictureBox1.ClientSize.Width, PictureBox1.ClientSize.Height)
	End Sub

	Private Sub ArrivalProfile_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		bFormInitialised = True
		Text = My.Resources.str00004
		'SetFormParented(Handle.ToInt32)
	End Sub

	Private Sub ArrivalProfile_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		If eventArgs.CloseReason = System.Windows.Forms.CloseReason.UserClosing Then
			eventArgs.Cancel = True
			Hide()
			ContainerCheck.Checked = False
		End If
	End Sub

	Private Sub ArrivalProfile_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
        If Not bFormInitialised Then Return
        Dim wd As Integer
        Dim hg As Integer
        wd = PictureBox1.ClientSize.Width
        hg = PictureBox1.ClientSize.Height

		PictureBox1.Image = New Bitmap(wd, hg)
		ReDrawGraphics()
	End Sub

	'Private Sub CArrivalProfile_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
	'	If e.KeyCode = Keys.F1 Then
	'		HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
	'		e.Handled = True
	'	End If
	'End Sub
#End Region

#Region "Point management"
	Public Sub InitWOFAF(ByRef RWY_Len As Double, ByRef RWY_Dir As Integer, ByRef RWY_H As Double, ByRef RefHeight As Double, ByRef MyCheck As System.Windows.Forms.CheckBox)
		RWYLen = RWY_Len
		RWYAlt = RWY_H - RefHeight
		RWYDir = RWY_Dir

		MAPtIndex = 0
		PointsNo = 0

		LeftX = -RWYLen
		RightX = RWYLen
		TopY = 100
		BottomY = 0

		ContainerCheck = MyCheck
		fRefHeight = RefHeight
		ReDrawGraphics()
	End Sub

	Public Function AddPoint(X As Double, Y As Double, Course As Double, PDG As Double, Role As CodeProcedureDistance, Optional IsFinal As Integer = 0) As Boolean
		AddPoint = False
		If (PointsNo >= MaxPoints) Then Exit Function

		Points(PointsNo).X = -RWYDir * X
		Points(PointsNo).Y = Y
		Points(PointsNo).Course = Course
		Points(PointsNo).PDG = PDG
		Points(PointsNo).Role = Role

		PointsNo = PointsNo + 1

		AddPoint = True
		ReDrawGraphics()
	End Function

	Public Function InsertPoint(X As Double, Y As Double, Course As Double, PDG As Double, Role As CodeProcedureDistance, Optional Index As Integer = MaxPoints - 1, Optional ByRef IsFinal As Integer = 0) As Boolean
		Dim I As Integer

		If (PointsNo >= MaxPoints) Then Return False

		If Index > PointsNo Then
			Index = PointsNo
		End If

		If Index < PointsNo Then
			For I = PointsNo To Index + 1 Step -1
				Points(I) = Points(I - 1)
			Next I
		End If

		Points(Index).X = -RWYDir * X
		Points(Index).Y = Y
		Points(Index).Course = Course
		Points(Index).PDG = PDG
		Points(Index).Role = Role

		PointsNo = PointsNo + 1
		ReDrawGraphics()

		Return True
	End Function

	Public Function ReplacePoint(X As Double, Y As Double, Course As Double, PDG As Double, Role As CodeProcedureDistance, Optional Index As Integer = MaxPoints - 1, Optional IsFinal As Integer = 0) As Boolean
		If Index >= PointsNo Then Return False

		Points(Index).X = -RWYDir * X
		Points(Index).Y = Y
		Points(Index).Course = Course
		Points(Index).PDG = PDG
		Points(Index).Role = Role

		ReDrawGraphics()

		Return True
	End Function

	Public Function GetPoint(Index As Integer) As ProfilePoint
		GetPoint = Points(Index)
		GetPoint.X = -RWYDir * Points(Index).X
	End Function

	Public Function GetPointRole(Index As Integer) As String
		Dim strArray() As String

		If Points(Index).Role < CodeProcedureDistance.HAT Then
			Return ""
		Else
			strArray = {"HAT", "OM", "MM", "IM", "PFAF", "GSANT", "FAF", "MAP", "THLD", "VDP", "RECH", "SDF"}
			Return strArray(Points(Index).Role)
		End If
	End Function

	Public Function RemovePoint() As Boolean
		RemovePoint = False
		If (PointsNo <= 0) Then Exit Function

		PointsNo = PointsNo - 1

		ReDrawGraphics()
		RemovePoint = True
	End Function

	Public Function RemovePointByIndex(ByRef Index As Integer) As Boolean
		Dim I As Integer
		RemovePointByIndex = False
		If (PointsNo <= 0) Then Exit Function

		For I = Index To PointsNo - 2
			Points(I) = Points(I + 1)
		Next I

		PointsNo = PointsNo - 1

		ReDrawGraphics()
		RemovePointByIndex = True
	End Function

	Public Sub ClearPoints()
		PointsNo = 0
		ReDrawGraphics()
	End Sub

#End Region

#Region "Drawing"
	Private Sub UpdateScales()
		Dim I As Integer
		Dim dLeft As Single
		Dim dRight As Single
		Dim dTop As Single
		Dim dBottom As Single

		' Matrix mx = g.Transform;
		' mx.RotateAt(Angle, new PointF(Location.X + (Area.Right - Location.X) / 2, Location.Y + (Area.Bottom - Location.Y) / 2), MatrixOrder.Append);
		' g.Transform = mx;
		' g.FillRectangle(new SolidBrush(BackColor), Rect);
		' g.DrawString(Data, TextFont, new SolidBrush(ForeColor), Rect, TextFormat);
		' g.DrawRectangle(Pen, Rect.X, Rect.Y, Rect.Width, Rect.Height);

		LeftX = -RWYLen
		RightX = RWYLen
		TopY = 100
		BottomY = RWYAlt

		For I = 0 To PointsNo - 1
			If (Points(I).X < LeftX) Then LeftX = Points(I).X
			If (Points(I).X > RightX) Then RightX = Points(I).X
			If (Points(I).Y < BottomY) Then BottomY = Points(I).Y
			If (Points(I).Y > TopY) Then TopY = Points(I).Y
		Next I

		dLeft = System.Math.Abs(LeftMargin + InnerLeftMargin)
		dRight = System.Math.Abs(RightMargin + InnerRightMargin)

		dBottom = System.Math.Abs(BottomMargin + InnerBottomMargin)
		dTop = System.Math.Abs(TopMargin + InnerTopMargin)

		ScaleWidth = (PictureBox1.ClientSize.Width - dLeft - dRight) / (RightX - LeftX)
		ScaleHeight = (PictureBox1.ClientSize.Height - dBottom - dTop) / (TopY - BottomY)
	End Sub

	Private Function TransformX(ByVal X As Double) As Single
		Return InnerLeftMargin + LeftMargin + ScaleWidth * (X - LeftX) 'RWYDir * 
	End Function

	Private Function TransformY(ByVal Y As Double) As Single
		Return PictureBox1.ClientSize.Height - (InnerBottomMargin + BottomMargin + ScaleHeight * (Y - BottomY))
	End Function

	Private Sub ReDrawGraphics()
		Dim I As Integer
		Dim dX As Double
		Dim PrevH As Double
        Dim TextToDraw As String

		Dim pPoint As New PointF
		'Dim pBrush As SolidBrush = Brushes.Black 

		imgGraphics = Graphics.FromImage(PictureBox1.Image)

		'Dim rect As RectangleF
		'rect = New RectangleF(0, 0, 1200, 900)
		'imgGraphics.Clip = New Region(rect)

		imgGraphics.Clear(Color.White)

		pPoint.Y = TransformY(0)

		DrawScreen()
		UpdateScales()
		'DrawZeroLine()
		imgGraphics.DrawLine(Pens.Black, LeftMargin, TransformY(0), PictureBox1.ClientSize.Width - RightMargin, TransformY(0))

		DrawRWY()

		ForeColor = System.Drawing.Color.FromArgb(255)

		For I = 1 To PointsNo - 1
			DrawSegment(Points(I - 1), Points(I))

			dX = ConvertDistance(Points(I - 1).X - Points(I).X, eRoundMode.NEAREST)
			If System.Math.Abs(dX) > DistanceConverter(DistanceUnit).Rounding Then
				Dim stringSize As SizeF

				TextToDraw = CStr(System.Math.Abs(dX))

				stringSize = imgGraphics.MeasureString(TextToDraw, Font)
				pPoint.X = TransformX(0.5 * (Points(I).X + Points(I - 1).X)) - 0.5 * stringSize.Width
				imgGraphics.DrawString(TextToDraw, Me.Font, Brushes.Black, pPoint)
			End If
		Next I

		PrevH = Points(0).Y - 2
		For I = 0 To PointsNo - 1
			DrawPoint(Points(I), 5)
			DrawTick(Points(I))
			If System.Math.Abs(PrevH - Points(I).Y) >= 1 Then
				DrawHLegend(Points(I), RWYDir)
				PrevH = Points(I).Y
			End If
		Next I

		''    If IsFinalPoint Then DrawArrow Points(PointsNo - 1)

		'    DrawHLegend Points(0), -RWYDir
		'    If PointsNo > 1 Then
		'        If (ProfileType = 1) Then DrawHLegend Points(1), RWYDir
		'        DrawHLegend Points(PointsNo - 1), -RWYDir
		'    End If

		imgGraphics.Dispose()

		PictureBox1.Refresh()
	End Sub

	Private Sub DrawRWY()
		Dim pPtTHR As ProfilePoint
		Dim pPen As New Pen(Color.Cyan, 4)
		pPen.DashStyle = Drawing2D.DashStyle.Solid
		pPtTHR.X = 0
		pPtTHR.Y = RWYAlt
		imgGraphics.DrawLine(pPen, TransformX(pPtTHR.X), TransformY(pPtTHR.Y), TransformX(pPtTHR.X + RWYDir * RWYLen), TransformY(pPtTHR.Y))

		DrawHLegend(pPtTHR, RWYDir, True)
	End Sub

	'Private Sub DrawZeroLine()
	'	imgGraphics.DrawLine(Pens.Black, LeftMargin, TransformY(0), PictureBox1.ClientSize.Width - RightMargin, TransformY(0))
	'End Sub

	Private Sub DrawTick(ByRef pPt As ProfilePoint)
		Dim pPen As Pen = New Pen(Color.DarkGray, 1)
		pPen.DashStyle = Drawing2D.DashStyle.Dash ' .Dot

		imgGraphics.DrawLine(pPen, TransformX(pPt.X), TransformY(pPt.Y), TransformX(pPt.X), TransformY(0))
	End Sub

	Private Sub DrawScreen()
		Dim rectangle1 As New Rectangle(LeftMargin, TopMargin, PictureBox1.ClientSize.Width - LeftMargin - RightMargin, PictureBox1.ClientSize.Height - BottomMargin - TopMargin)
		Dim pPen As New Pen(Color.Black, 2)
		pPen.DashStyle = Drawing2D.DashStyle.Solid

		imgGraphics.DrawRectangle(pPen, rectangle1)
	End Sub

	Private Sub DrawSegment(ByRef pPt1 As ProfilePoint, ByRef pPt2 As ProfilePoint)
		Dim pPen As New Pen(Color.Black, 2)
		pPen.DashStyle = Drawing2D.DashStyle.Solid

		imgGraphics.DrawLine(pPen, TransformX(pPt1.X), TransformY(pPt1.Y), TransformX(pPt2.X), TransformY(pPt2.Y))
	End Sub

	Private Sub DrawPoint(ByRef pPt As ProfilePoint, ByRef R As Single)
		Dim pBrush As SolidBrush = Brushes.Black
		Dim X As Single = TransformX(pPt.X)
		Dim Y As Single = TransformY(pPt.Y)
		Dim rect As New Rectangle(X - R, Y - R, 2 * R, 2 * R)

		imgGraphics.FillEllipse(pBrush, rect)
	End Sub

	Private Sub DrawHLegend(ByRef pPtFrom As ProfilePoint, ByVal Direction As Integer, Optional ByRef IsThr As Boolean = False)
		Dim sHeight As String
		Dim sElevation As String
		Dim Margin_Renamed As Integer

		Dim pPen As Pen = Pens.Black
		Dim pBrush As SolidBrush = Brushes.Black
		Dim stringSize As SizeF
		Dim pPoint As New PointF

		sElevation = CStr(ConvertHeight(pPtFrom.Y + fRefHeight, eRoundMode.NEAREST))
		sHeight = "(" + CStr(ConvertHeight(pPtFrom.Y, eRoundMode.NEAREST)) + ")"
		stringSize = imgGraphics.MeasureString(sElevation, Font)

		If Not IsThr Then
			imgGraphics.DrawLine(pPen, TransformX(pPtFrom.X), TransformY(pPtFrom.Y), TransformX(pPtFrom.X) - Direction * LegendWidth, TransformY(pPtFrom.Y))
		End If

		If (Direction = -1) Then
			If IsThr Then
				Margin_Renamed = 0
			Else
				Margin_Renamed = 5 * TextPadding
			End If
		Else
			If IsThr Then
				Margin_Renamed = -System.Math.Abs(stringSize.Width) '- 9 * TextPadding 
			Else
				Margin_Renamed = -System.Math.Abs(stringSize.Width) - 14 * TextPadding
			End If
		End If

		pPoint.X = TransformX(pPtFrom.X) + Margin_Renamed

		pPoint.Y = TransformY(pPtFrom.Y) - System.Math.Abs(stringSize.Height) - TextPadding
		imgGraphics.DrawString(sElevation, Me.Font, pBrush, pPoint)

		pPoint.Y = pPoint.Y + System.Math.Abs(stringSize.Height) + 2 * TextPadding
		imgGraphics.DrawString(sHeight, Me.Font, pBrush, pPoint)
	End Sub

	'Private Sub DrawArrow(ByRef pPtFrom As ProfilePoint)
	'	Dim ArrowX As Double
	'	Dim ArrowY As Double
	'	Dim ArrowAngleInGrad As Double
	'	Dim imgGraphics As Graphics = Graphics.FromImage(PictureBox1.Image)
	'	Dim pPen As New Pen(Color.Black, 2)
	'	Dim pBrush As SolidBrush = New SolidBrush(Color.Black)
	'	Dim pPoint As New PointF

	'	pPen.DashStyle = Drawing2D.DashStyle.Solid

	'	ArrowAngleInGrad = ArrowAngle * PI / 180.0

	'	ArrowX = pPtFrom.X + RWYDir * ArrowLen * System.Math.Cos(ArrowAngleInGrad)
	'	ArrowY = pPtFrom.Y + ArrowLen * System.Math.Sin(ArrowAngleInGrad)

	'	imgGraphics.DrawLine(pPen, TransformX(pPtFrom.X), TransformY(pPtFrom.Y), TransformX(ArrowX), TransformY(ArrowY))
	'	imgGraphics.DrawLine(pPen, TransformX(ArrowX), TransformY(ArrowY), TransformX(ArrowX - RWYDir * ArrowWingLen), TransformX(ArrowY))
	'	imgGraphics.DrawLine(pPen, TransformX(ArrowX), TransformY(ArrowY), TransformX(ArrowX), TransformY(ArrowY + 60.0 - ArrowWingLen))
	'End Sub

	'Private Sub DrawText(ByRef X As Double, ByRef Y As Double, ByRef S As String)
	'	Dim imgGraphics As Graphics = Graphics.FromImage(PictureBox1.Image)
	'	Dim pBrush As SolidBrush = New SolidBrush(Color.Black)
	'	Dim pPoint As New PointF

	'	pPoint.X = TransformX(X)
	'	pPoint.Y = TransformY(Y)

	'	imgGraphics.DrawString(S, Me.Font, pBrush, pPoint)
	'End Sub
#End Region

End Class