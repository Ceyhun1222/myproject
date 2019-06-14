Option Strict Off

Imports Aran.AranEnvironment
Imports ESRI.ArcGIS.Geometry
Imports System.Collections.Generic
Imports ESRI.ArcGIS.Carto

Friend Class VisualManoeuvringTrackConstructor
    Private _isClosed As Boolean = False
    Public Property isClosed() As Boolean
        Get
            Return _isClosed
        End Get
        Set(ByVal value As Boolean)
            _isClosed = value
        End Set
    End Property

    Dim LonSubFix() As String = {"E", "W"}
    Dim LatSubFix() As String = {"N", "S"}
    Dim targetPntLatDeg As Double
    Dim targetPntLatMin As Double
    Dim targetPntLatSec As Double
    Dim targetPntLonDeg As Double
    Dim targetPntLonMin As Double
    Dim targetPntLonSec As Double
    Dim targetPntLatSign As Integer
    Dim targetPntLonSign As Integer

    Dim pickTargetPnt As AranTool

    Dim targetPntX As Double
    Dim targetPntY As Double
    Dim targetPntGeoAngle As Double

    Dim targetPrjPnt As IPoint
    Dim targetPntPrjAngle As Double
    Dim targetPntElement As IElement
    Dim divergenceAngle As Double
    Dim convergenceAngle As Double
    Dim prevDivergenceAngle As Double
    Dim prevConvergenceAngle As Double
    Dim divergencePnt As IPoint
    Dim convergencePnt As IPoint

    'Dim startPrjPnt As IPoint
    'Dim startPrjAngle As Double
    'Dim targetPntSide As Integer

    Private _trackConstructor As TrackConstructor
    Public Property trackConstructor() As TrackConstructor
        Get
            Return _trackConstructor
        End Get
        Set(ByVal value As TrackConstructor)
            _trackConstructor = value
        End Set
    End Property

    Private _manoeuvringArea As IPointCollection
    Public Property manoeuvringArea() As IPointCollection
        Get
            Return _manoeuvringArea
        End Get
        Set(ByVal value As IPointCollection)
            _manoeuvringArea = value
        End Set
    End Property
    Dim pTopo As ITopologicalOperator2

    Private _leftPickablePoly As IGeometry
    Private _rightPickablePoly As IGeometry

    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

    Public Sub New(ByVal convexPoly As IPointCollection)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _manoeuvringArea = convexPoly
        _trackConstructor = New TrackConstructor
        pGraphics = GetActiveView().GraphicsContainer
    End Sub

    Private Sub VisualManoeuvringTrackConstructor_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cmbBox_targetPntLatSide.Items.Add(LatSubFix(0))
        cmbBox_targetPntLatSide.Items.Add(LatSubFix(1))
        cmbBox_targetPntLonSide.Items.Add(LonSubFix(0))
        cmbBox_targetPntLonSide.Items.Add(LonSubFix(1))
        cmbBox_targetPntLatSide.SelectedIndex = 0
        cmbBox_targetPntLonSide.SelectedIndex = 0

        pickTargetPnt = New AranTool()
        pickTargetPnt.Visible = True
        pickTargetPnt.Cursor = Cursors.Cross
        pickTargetPnt.MouseClickedOnMap = AddressOf Me.OnPickTargetPnt
        gAranEnv.AranUI.AddMapTool(pickTargetPnt)

        targetPntLatSign = 1
        targetPntLonSign = 1

        _trackConstructor.ConstructPickableArea(_leftPickablePoly, _rightPickablePoly, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
    End Sub

    Private Sub btn_pickTargetPnt_Click(sender As System.Object, e As System.EventArgs) Handles btn_pickTargetPnt.Click
        gAranEnv.AranUI.SetCurrentTool(Me.pickTargetPnt)
    End Sub

    Private Sub btn_targetPntDone_Click(sender As System.Object, e As System.EventArgs) Handles btn_targetPntDone.Click
        Dim isFormValid As Boolean = True
        If Not IsNumeric(txtBox_targetPntLatDegree.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_targetPntLatMinute.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_targetPntLatSecond.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_targetPntLonDegree.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_targetPntLonMinute.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_targetPntLonSecond.Text) Then
            isFormValid = False
        End If

        If isFormValid Then

            targetPntLatDeg = CDbl(txtBox_targetPntLatDegree.Text)
            targetPntLatMin = CDbl(txtBox_targetPntLatMinute.Text)
            targetPntLatSec = CDbl(txtBox_targetPntLatSecond.Text)

            targetPntLonDeg = CDbl(txtBox_targetPntLonDegree.Text)
            targetPntLonMin = CDbl(txtBox_targetPntLonMinute.Text)
            targetPntLonSec = CDbl(txtBox_targetPntLonSecond.Text)

            targetPntY = DMS2DD(targetPntLatDeg, targetPntLatMin, targetPntLatSec, targetPntLatSign)
            targetPntX = DMS2DD(targetPntLonDeg, targetPntLonMin, targetPntLonSec, targetPntLonSign)

            targetPntGeoAngle = nmrcUpDown_TargetPntAngle.Value

            Dim geoPnt As IPoint = New Point()
            geoPnt.PutCoords(targetPntX, targetPntY)
            Dim targetPrjPnt As IPoint = CType(Functions.ToPrj(geoPnt), Global.ESRI.ArcGIS.Geometry.IPoint)
            Dim targetPntPrjAngle As Double = Functions.Azt2DirPrj(targetPrjPnt, targetPntGeoAngle)

            
            _trackConstructor.TargetPointDoneButtonClicked(targetPrjPnt, targetPntPrjAngle, chkBox_finalStep.Checked)

            cmbBox_divergencePnt.Items.Clear()
            If _trackConstructor.DivergenceSegmentVisualReferencePoints.Count > 0 Then
                For i As Integer = 0 To _trackConstructor.DivergenceSegmentVisualReferencePoints.Count - 1
                    cmbBox_divergencePnt.Items.Add(_trackConstructor.DivergenceSegmentVisualReferencePoints(i).Name)
                Next
            Else
                cmbBox_divergencePnt.Items.Add("No visual point...")
            End If
            cmbBox_divergencePnt.SelectedIndex = 0
        Else
            MessageBox.Show("Please, fill in the form...")
        End If
    End Sub

    Private Sub cmbBox_divergencePnt_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_divergencePnt.SelectedIndexChanged
        If cmbBox_divergencePnt.SelectedItem Is "No visual point..." Then
            If Not chkBox_isOnCirclingBox.Checked OrElse _trackConstructor.startTargetAngleDiff <= 178 OrElse _trackConstructor.startTargetAngleDiff >= 182 Then
                cmbBox_convergencePnt.Items.Add("No visual point...")
                cmbBox_convergencePnt.SelectedIndex = 0
                Return
            End If
        End If
        If cmbBox_divergencePnt.SelectedItem IsNot "No visual point..." Then
            If _trackConstructor.divergenceVisRefPntElement IsNot Nothing Then
                _trackConstructor.pGraphics.DeleteElement(_trackConstructor.divergenceVisRefPntElement)
                _trackConstructor.divergenceVisRefPntElement = Nothing
            End If
            _trackConstructor.divergenceVisRefPntElement = Functions.DrawPoint(_trackConstructor.DivergenceSegmentVisualReferencePoints(cmbBox_divergencePnt.SelectedIndex).pShape, Color.Purple.ToArgb, True)
            _trackConstructor.divergenceVisRefPntIndx = cmbBox_divergencePnt.SelectedIndex
        Else
            _trackConstructor.divergenceVisRefPntIndx = -1
        End If

        If _trackConstructor.convergenceVisRefPntElement IsNot Nothing Then
            _trackConstructor.pGraphics.DeleteElement(_trackConstructor.convergenceVisRefPntElement)
            _trackConstructor.convergenceVisRefPntElement = Nothing
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        cmbBox_convergencePnt.Items.Clear()
        Dim pntNames As List(Of String) = New List(Of String)
        _trackConstructor.GetSelectableConvergencePoints(chkBox_finalStep.Checked, pntNames)
        For i As Integer = 0 To pntNames.Count - 1
            cmbBox_convergencePnt.Items.Add(pntNames(i))
        Next

        If cmbBox_convergencePnt.Items.Count = 0 Then
            cmbBox_convergencePnt.Items.Add("No visual point...")
        End If
        cmbBox_convergencePnt.SelectedIndex = 0
    End Sub

    Private Sub cmbBox_convergencePnt_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_convergencePnt.SelectedIndexChanged
        If _trackConstructor.convergenceVisRefPntElement IsNot Nothing Then
            _trackConstructor.pGraphics.DeleteElement(_trackConstructor.convergenceVisRefPntElement)
            _trackConstructor.convergenceVisRefPntElement = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If
        If cmbBox_convergencePnt.SelectedItem IsNot "No visual point..." Then
            For i As Integer = 0 To _trackConstructor.ConvergenceSegmentVisualReferencePoints.Count - 1
                If _trackConstructor.ConvergenceSegmentVisualReferencePoints(i).Name = cmbBox_convergencePnt.SelectedItem.ToString Then
                    _trackConstructor.convergenceVisRefPntElement = Functions.DrawPoint(_trackConstructor.ConvergenceSegmentVisualReferencePoints(i).pShape, Color.Purple.ToArgb, True)
                    _trackConstructor.convergenceVisRefPntIndx = i
                    Exit For
                End If
            Next
        Else
            _trackConstructor.convergenceVisRefPntIndx = -1
        End If
    End Sub

    Private Sub btn_AddStep_Click(sender As System.Object, e As System.EventArgs) Handles btn_AddStep.Click
        If cmbBox_divergencePnt.Items.Count = 0 Then
            MessageBox.Show("There is no visual points to diverge.")
            Return
        End If
        If cmbBox_convergencePnt.Items.Count = 0 Then
            MessageBox.Show("There is no visual points to converge.")
            Return
        End If
        If cmbBox_convergencePnt.SelectedItem IsNot "No visual point..." Then
            _trackConstructor.AddStepButtonClicked(chkBox_isOnCirclingBox.Checked, chkBox_finalStep.Checked, cmbBox_convergencePnt.SelectedItem.ToString, cmbBox_divergencePnt.SelectedIndex)
            cmbBox_divergencePnt.Items.Clear()
            cmbBox_convergencePnt.Items.Clear()
            chkBox_leftGE180turn.Checked = False
            chkBox_rightGE180turn.Checked = False
            _trackConstructor.ConstructPickableArea(_leftPickablePoly, _rightPickablePoly, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
        Else
            MessageBox.Show("Cannot construct the step...")
        End If
    End Sub

    Private Sub btn_RemoveStep_Click(sender As System.Object, e As System.EventArgs) Handles btn_RemoveStep.Click
        _trackConstructor.RemoveButtonCkicked()
        cmbBox_divergencePnt.Items.Clear()
        cmbBox_convergencePnt.Items.Clear()
        _trackConstructor.ConstructPickableArea(_leftPickablePoly, _rightPickablePoly, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
    End Sub

    Private Sub btn_SaveTrack_Click(sender As System.Object, e As System.EventArgs) Handles btn_SaveTrack.Click
        'VisualManoeuvringDBTool.AddTrackStepCentreline(_trackConstructor.trackSteps)
        'Me.Close()
        If _trackConstructor.pickableAreaElements IsNot Nothing Then
            For i As Integer = 0 To _trackConstructor.pickableAreaElements.ElementCount - 1
                pGraphics.DeleteElement(_trackConstructor.pickableAreaElements.Element(i))
            Next
            _trackConstructor.pickableAreaElements = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Sub

    Private Sub btn_Close_Click(sender As System.Object, e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub VisualManoeuvringTrackConstructor_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        _trackConstructor.RemoveTrack()
        If targetPntElement IsNot Nothing Then
            pGraphics.DeleteElement(targetPntElement)
            targetPntElement = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If
        _isClosed = True
    End Sub

    Private Sub OnPickTargetPnt(ByVal sender As Object, ByVal arg As Aran.AranEnvironment.MapMouseEventArg)
        If targetPntElement IsNot Nothing Then
            pGraphics.DeleteElement(targetPntElement)
            targetPntElement = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If
        cmbBox_divergencePnt.Items.Clear()
        cmbBox_convergencePnt.Items.Clear()

        targetPntX = arg.X
        targetPntY = arg.Y

        Dim pPnt As IPoint = New Point()
        pPnt.PutCoords(targetPntX, targetPntY)

        Dim poly As IPolygon = CType(_manoeuvringArea, IPolygon)
        Dim relatOper As IRelationalOperator
        pTopo = CType(poly, ITopologicalOperator2)
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        relatOper = CType(_manoeuvringArea, IRelationalOperator)

        If relatOper.Contains(pPnt) Then
            If _leftPickablePoly IsNot Nothing Then
                pTopo = CType(_leftPickablePoly, ITopologicalOperator2)
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                relatOper = CType(_leftPickablePoly, IRelationalOperator)
                If Not relatOper.Contains(pPnt) Then
                    If _rightPickablePoly IsNot Nothing Then
                        pTopo = CType(_rightPickablePoly, ITopologicalOperator2)
                        pTopo.IsKnownSimple_2 = False
                        pTopo.Simplify()
                        relatOper = CType(_rightPickablePoly, IRelationalOperator)
                        If Not relatOper.Contains(pPnt) Then
                            relatOper = CType(_leftPickablePoly, IRelationalOperator)
                            If Not relatOper.Contains(pPnt) Then
                                txtBox_targetPntLatDegree.Text = ""
                                txtBox_targetPntLatMinute.Text = ""
                                txtBox_targetPntLatSecond.Text = ""
                                txtBox_targetPntLonDegree.Text = ""
                                txtBox_targetPntLonMinute.Text = ""
                                txtBox_targetPntLonSecond.Text = ""
                                MessageBox.Show("The point is out of the reachable area.")
                                Return
                            End If
                        End If
                    End If
                End If
            End If
        Else
            txtBox_targetPntLatDegree.Text = ""
            txtBox_targetPntLatMinute.Text = ""
            txtBox_targetPntLatSecond.Text = ""
            txtBox_targetPntLonDegree.Text = ""
            txtBox_targetPntLonMinute.Text = ""
            txtBox_targetPntLonSecond.Text = ""
            MessageBox.Show("The point is out of the manoeuvring area.")
            Return
        End If

        Dim gPnt As IPoint = CType(Functions.ToGeo(pPnt), Global.ESRI.ArcGIS.Geometry.IPoint)

        targetPntX = gPnt.X
        targetPntY = gPnt.Y

        Dim xDeg As Double
        Dim xMin As Double
        Dim xSec As Double

        Dim yDeg As Double
        Dim yMin As Double
        Dim ySec As Double

        DD2DMS(targetPntX, xDeg, xMin, xSec, Math.Sign(targetPntX))
        DD2DMS(targetPntY, yDeg, yMin, ySec, Math.Sign(targetPntY))

        txtBox_targetPntLatDegree.Text = CStr(yDeg)
        txtBox_targetPntLatMinute.Text = CStr(yMin)
        txtBox_targetPntLatSecond.Text = CStr(Math.Round(ySec, 2))
        If Math.Sign(targetPntY) < 0 Then
            cmbBox_targetPntLatSide.SelectedIndex = 1
        Else
            cmbBox_targetPntLatSide.SelectedIndex = 0
        End If

        txtBox_targetPntLonDegree.Text = CStr(xDeg)
        txtBox_targetPntLonMinute.Text = CStr(xMin)
        txtBox_targetPntLonSecond.Text = CStr(Math.Round(xSec, 2))

        If Math.Sign(targetPntX) < 0 Then
            cmbBox_targetPntLonSide.SelectedIndex = 1
        Else
            cmbBox_targetPntLonSide.SelectedIndex = 0
        End If



        'New
        Dim minPrjAngle As Double
        Dim maxPrjAngle As Double

        targetPntLatDeg = CDbl(txtBox_targetPntLatDegree.Text)
        targetPntLatMin = CDbl(txtBox_targetPntLatMinute.Text)
        targetPntLatSec = CDbl(txtBox_targetPntLatSecond.Text)

        targetPntLonDeg = CDbl(txtBox_targetPntLonDegree.Text)
        targetPntLonMin = CDbl(txtBox_targetPntLonMinute.Text)
        targetPntLonSec = CDbl(txtBox_targetPntLonSecond.Text)

        targetPntY = DMS2DD(targetPntLatDeg, targetPntLatMin, targetPntLatSec, targetPntLatSign)
        targetPntX = DMS2DD(targetPntLonDeg, targetPntLonMin, targetPntLonSec, targetPntLonSign)

        Dim geoPnt As IPoint = New Point()
        geoPnt.PutCoords(targetPntX, targetPntY)
        targetPrjPnt = CType(Functions.ToPrj(geoPnt), Global.ESRI.ArcGIS.Geometry.IPoint)
        targetPntElement = Functions.DrawPoint(targetPrjPnt, 255, True)
        trackConstructor.GetAngleRange_light(targetPrjPnt, minPrjAngle, maxPrjAngle, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
        minPrjAngle = Math.Ceiling(minPrjAngle)
        maxPrjAngle = Math.Floor(maxPrjAngle)

        Dim minGeoAngle As Double = Dir2Azt(targetPrjPnt, maxPrjAngle)
        Dim maxGeoAngle As Double = Dir2Azt(targetPrjPnt, minPrjAngle)
        If maxGeoAngle < minGeoAngle Then
            maxGeoAngle += 360
        End If

        'maxGeoAngle = Dir2Azt(targetPrjPnt, maxPrjAngle)
        'minGeoAngle = Dir2Azt(targetPrjPnt, minPrjAngle)

        lbl_TargetPntAngleRange.Text = CStr("(" & Math.Round(minGeoAngle, 0) & " - " & Math.Round(maxGeoAngle, 0) & ")")


        nmrcUpDown_TargetPntAngle.Enabled = True
        nmrcUpDown_TargetPntAngle.Minimum = CDec(minGeoAngle)
        nmrcUpDown_TargetPntAngle.Maximum = CDec(maxGeoAngle)

        nmrcUpDown_TargetPntAngle.Value = CDec(minGeoAngle)
        targetPntPrjAngle = maxPrjAngle

        nmrcUpDown_DivergenceAngle.Minimum = 0
        nmrcUpDown_DivergenceAngle.Maximum = CDec(45)

        nmrcUpDown_ConvergenceAngle.Minimum = 0
        nmrcUpDown_ConvergenceAngle.Maximum = CDec(90)
        'End New

    End Sub

    Private Function DMS2DD(Deg As Double, Min As Double, Sec As Double, Sign As Integer) As Double
        Dim X As Double
        X = Deg + Min / 60 + Sec / 3600
        Return X * Sign
    End Function

    Private Sub DD2DMS(val As Double, ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, Sign As Integer)
        Dim x As Double
        Dim dx As Double

        x = Math.Abs(Math.Round(Math.Abs(val) * Sign, 10))

        xDeg = Fix(x)
        dx = (x - xDeg) * 60
        dx = Math.Round(dx, 8)
        xMin = Fix(dx)
        xSec = (dx - xMin) * 60
        xSec = Math.Round(xSec, 6)
    End Sub

    Private Sub txtBox_targetPntLatDegree_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLatDegree.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLatDegree_Validating(txtBox_targetPntLatDegree, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLatDegree.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLatDegree_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLatDegree.Validating
        If Not IsNumeric(txtBox_targetPntLatDegree.Text) Then Return
        If CDbl(txtBox_targetPntLatDegree.Text) >= 90.0# Then
            txtBox_targetPntLatDegree.Text = CStr(89)
        End If
    End Sub

    Private Sub txtBox_targetPntLatMinute_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLatMinute.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLatMinute_Validating(txtBox_targetPntLatMinute, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLatMinute.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLatMinute_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLatMinute.Validating
        If Not IsNumeric(txtBox_targetPntLatMinute.Text) Then Return
        If CDbl(txtBox_targetPntLatMinute.Text) >= 60.0# Then
            txtBox_targetPntLatMinute.Text = CStr(59)
        End If
    End Sub

    Private Sub txtBox_targetPntLatSecond_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLatSecond.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLatSecond_Validating(txtBox_targetPntLatSecond, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLatSecond.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLatSecond_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLatSecond.Validating
        If Not IsNumeric(txtBox_targetPntLatSecond.Text) Then Return
        If CDbl(txtBox_targetPntLatSecond.Text) >= 60.0# Then
            txtBox_targetPntLatSecond.Text = CStr(59.99)
        End If
    End Sub

    Private Sub txtBox_targetPntLonDegree_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLonDegree.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLonDegree_Validating(txtBox_targetPntLonDegree, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLonDegree.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLonDegree_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLonDegree.Validating
        If Not IsNumeric(txtBox_targetPntLonDegree.Text) Then Return
        If CDbl(txtBox_targetPntLonDegree.Text) >= 180.0# Then
            txtBox_targetPntLonDegree.Text = CStr(179)
        End If
    End Sub

    Private Sub txtBox_targetPntLonMinute_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLonMinute.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLonMinute_Validating(txtBox_targetPntLonMinute, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLonMinute.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLonMinute_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLonMinute.Validating
        If Not IsNumeric(txtBox_targetPntLonMinute.Text) Then Return
        If CDbl(txtBox_targetPntLonMinute.Text) >= 60.0# Then
            txtBox_targetPntLonMinute.Text = CStr(59)
        End If
    End Sub

    Private Sub txtBox_targetPntLonSecond_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_targetPntLonSecond.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_targetPntLonSecond_Validating(txtBox_targetPntLonSecond, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_targetPntLonSecond.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_targetPntLonSecond_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_targetPntLonSecond.Validating
        If Not IsNumeric(txtBox_targetPntLonSecond.Text) Then Return
        If CDbl(txtBox_targetPntLonSecond.Text) >= 60.0# Then
            txtBox_targetPntLonSecond.Text = CStr(59.99)
        End If
    End Sub

    Private Sub cmbBox_targetPntLatSide_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If cmbBox_targetPntLatSide.SelectedIndex = 1 Then
            targetPntLatSign = -1
        Else
            targetPntLatSign = 1
        End If
    End Sub

    Private Sub cmbBox_targetPntLonSide_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If cmbBox_targetPntLonSide.SelectedIndex = 1 Then
            targetPntLonSign = -1
        Else
            targetPntLonSign = 1
        End If
    End Sub

    Private Sub chkBox_isOnCirclingBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_isOnCirclingBox.CheckedChanged
        If chkBox_isOnCirclingBox.Checked Then
            chkBox_finalStep.Checked = True
            'chkBox_180turn.Checked = True
        Else
            chkBox_finalStep.Checked = False
            'chkBox_180turn.Checked = False
        End If
    End Sub

    Private Sub chkBox_leftGE180turn_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_leftGE180turn.CheckedChanged
        chkBox_rightGE180turn.Checked = False
        _trackConstructor.ConstructPickableArea(_leftPickablePoly, _rightPickablePoly, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
    End Sub

    Private Sub chkBox_rightGE180turn_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_rightGE180turn.CheckedChanged
        chkBox_leftGE180turn.Checked = False
        _trackConstructor.ConstructPickableArea(_leftPickablePoly, _rightPickablePoly, chkBox_leftGE180turn.Checked, chkBox_rightGE180turn.Checked)
    End Sub

    Private Sub nmrcUpDown_TargetPntAngle_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_TargetPntAngle.ValueChanged
        'targetPntPrjAngle = Azt2DirPrj(targetPrjPnt, nmrcUpDown_TargetPntAngle.Value)
        'trackConstructor.GetDivergenceConvergenceAngles(targetPrjPnt, targetPntPrjAngle, divergenceAngle, convergenceAngle)
        'divergencePnt = Nothing
        'convergencePnt = Nothing
        'nmrcUpDown_DivergenceAngle.Value = CDec(divergenceAngle)
        'nmrcUpDown_ConvergenceAngle.Value = CDec(convergenceAngle)
        'trackConstructor.DrawStep_2(targetPrjPnt, targetPntPrjAngle, divergenceAngle, convergenceAngle, divergencePnt, convergencePnt)
    End Sub

    Private Sub nmrcUpDown_DivergenceAngle_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_DivergenceAngle.ValueChanged
        'If divergencePnt IsNot Nothing Then
        '    divergenceAngle = nmrcUpDown_DivergenceAngle.Value
        '    convergencePnt = Nothing
        '    convergenceAngle = trackConstructor.GetConvergenceAngle(targetPrjPnt, targetPntPrjAngle, divergenceAngle)
        '    If convergenceAngle <= 90 Then
        '        nmrcUpDown_ConvergenceAngle.Value = CDec(convergenceAngle)
        '        trackConstructor.DrawStep_2(targetPrjPnt, targetPntPrjAngle, divergenceAngle, convergenceAngle, divergencePnt, convergencePnt)
        '        prevDivergenceAngle = divergenceAngle
        '    Else
        '        divergenceAngle = prevDivergenceAngle
        '        nmrcUpDown_DivergenceAngle.Value = CDec(divergenceAngle)
        '    End If
        'End If
    End Sub

    Private Sub nmrcUpDown_ConvergenceAngle_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_ConvergenceAngle.ValueChanged
        'If convergencePnt IsNot Nothing Then
        '    convergenceAngle = nmrcUpDown_ConvergenceAngle.Value
        '    divergencePnt = Nothing
        '    divergenceAngle = trackConstructor.GetDivergenceAngle(targetPrjPnt, targetPntPrjAngle, convergencePnt, convergenceAngle)
        '    If divergenceAngle <= 45 Then
        '        nmrcUpDown_DivergenceAngle.Value = CDec(divergenceAngle)
        '        trackConstructor.DrawStep_2(targetPrjPnt, targetPntPrjAngle, divergenceAngle, convergenceAngle, divergencePnt, convergencePnt)
        '        prevConvergenceAngle = convergenceAngle
        '    Else
        '        convergenceAngle = prevConvergenceAngle
        '        nmrcUpDown_ConvergenceAngle.Value = CDec(convergenceAngle)
        '    End If
        'End If
    End Sub

    
End Class