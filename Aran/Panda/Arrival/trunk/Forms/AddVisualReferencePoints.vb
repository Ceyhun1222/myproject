Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Geometry
Imports Aran.AranEnvironment
Imports ESRI.ArcGIS.Carto
Imports System.Collections.Generic
Imports ESRI.ArcGIS.Geodatabase
Imports Aran.Aim.Features

Public Class AddVisualReferencePoints
    Private _isClosed As Boolean = False
    Public Property isClosed() As Boolean
        Get
            Return _isClosed
        End Get
        Set(ByVal value As Boolean)
            '_isClosed = value
        End Set
    End Property

    Private _featureWorkspace As IFeatureWorkspace
    Private _visRefFeatClass As IFeatureClass
    Private _visRefIndexes As Integer()
    Private _fileName As String

    Private pickNewPnt As AranTool

    Dim LonSubFix() As String = {"E", "W"}
    Dim LatSubFix() As String = {"N", "S"}
    Dim gNewPntX As Double
    Dim gNewPntY As Double
    Dim pNewPntX As Double
    Dim pNewPntY As Double
    Dim newPntLatDeg As Double
    Dim newPntLatMin As Double
    Dim newPntLatSec As Double
    Dim newPntLonDeg As Double
    Dim newPntLonMin As Double
    Dim newPntLonSec As Double
    Dim newPntLatSign As Integer
    Dim newPntLonSign As Integer

    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

    Private Sub OnPickNewPnt(ByVal sender As Object, ByVal arg As Aran.AranEnvironment.MapMouseEventArg)
        pNewPntX = arg.X
        pNewPntY = arg.Y

        Dim pPnt As IPoint = New Point()
        pPnt.PutCoords(pNewPntX, pNewPntY)
        Dim gPnt As IPoint = CType(Functions.ToGeo(pPnt), Global.ESRI.ArcGIS.Geometry.IPoint)

        gNewPntX = gPnt.X
        gNewPntY = gPnt.Y

        Dim xDeg As Double
        Dim xMin As Double
        Dim xSec As Double

        Dim yDeg As Double
        Dim yMin As Double
        Dim ySec As Double

        DD2DMS(gNewPntX, xDeg, xMin, xSec, Math.Sign(gNewPntX))
        DD2DMS(gNewPntY, yDeg, yMin, ySec, Math.Sign(gNewPntY))

        txtBox_newPntLatDegree.Text = CStr(yDeg)
        txtBox_newPntLatMinute.Text = CStr(yMin)
        txtBox_newPntLatSecond.Text = CStr(Math.Round(ySec, 2))
        If Math.Sign(pNewPntY) < 0 Then
            cmbBox_newPntLatSide.SelectedIndex = 1
        Else
            cmbBox_newPntLatSide.SelectedIndex = 0
        End If

        txtBox_newPntLonDegree.Text = CStr(xDeg)
        txtBox_newPntLonMinute.Text = CStr(xMin)
        txtBox_newPntLonSecond.Text = CStr(Math.Round(xSec, 2))

        If Math.Sign(pNewPntX) < 0 Then
            cmbBox_newPntLonSide.SelectedIndex = 1
        Else
            cmbBox_newPntLonSide.SelectedIndex = 0
        End If
    End Sub

    Private Sub btn_pickNewPnt_Click(sender As System.Object, e As System.EventArgs) Handles btn_pickNewPnt.Click
        gAranEnv.AranUI.SetCurrentTool(Me.pickNewPnt)
    End Sub

    Private Sub AddVisualReferencePoints_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        pickNewPnt = New AranTool()
        pickNewPnt.Visible = True
        pickNewPnt.Cursor = Cursors.Cross
        pickNewPnt.MouseClickedOnMap = AddressOf Me.OnPickNewPnt
        gAranEnv.AranUI.AddMapTool(pickNewPnt)

        newPntLatSign = 1
        newPntLonSign = 1

        cmbBox_newPntLatSide.Items.Add(LatSubFix(0))
        cmbBox_newPntLatSide.Items.Add(LatSubFix(1))
        cmbBox_newPntLonSide.Items.Add(LonSubFix(0))
        cmbBox_newPntLonSide.Items.Add(LonSubFix(1))
        cmbBox_newPntLatSide.SelectedIndex = 0
        cmbBox_newPntLonSide.SelectedIndex = 0

        cmbBox_newPntType.Items.Add("type 1")
        cmbBox_newPntType.Items.Add("type 2")
        cmbBox_newPntType.Items.Add("type 3")
        cmbBox_newPntType.SelectedIndex = 0

        pGraphics = GetActiveView().GraphicsContainer
    End Sub

    Private Sub btn_addPnt_Click(sender As System.Object, e As System.EventArgs) Handles btn_addPnt.Click
        Dim isFormValid As Boolean = True

        If Not IsNumeric(txtBox_newPntLatDegree.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_newPntLatMinute.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_newPntLatSecond.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_newPntLonDegree.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_newPntLonMinute.Text) Then
            isFormValid = False
        End If
        If Not IsNumeric(txtBox_newPntLonSecond.Text) Then
            isFormValid = False
        End If
        If txtBox_newPntName.Text = "" Then
            isFormValid = False
        End If

        If txtBox_newPntDescription.Text = "" Then
            isFormValid = False
        End If

        If isFormValid Then
            newPntLatDeg = CDbl(txtBox_newPntLatDegree.Text)
            newPntLatMin = CDbl(txtBox_newPntLatMinute.Text)
            newPntLatSec = CDbl(txtBox_newPntLatSecond.Text)

            newPntLonDeg = CDbl(txtBox_newPntLonDegree.Text)
            newPntLonMin = CDbl(txtBox_newPntLonMinute.Text)
            newPntLonSec = CDbl(txtBox_newPntLonSecond.Text)

            gNewPntY = DMS2DD(newPntLatDeg, newPntLatMin, newPntLatSec, newPntLatSign)
            gNewPntX = DMS2DD(newPntLonDeg, newPntLonMin, newPntLonSec, newPntLonSign)

            Dim newgPnt As IPoint = New Point()
            newgPnt.PutCoords(gNewPntX, gNewPntY)
            CType(newgPnt, IZAware).ZAware = True
            newgPnt.Z = 0
            'Dim newpPnt As IPoint = CType(Functions.ToPrj(newgPnt), Global.ESRI.ArcGIS.Geometry.IPoint)

            'Dim newVisRefPnt As VisualReferencePoint = New VisualReferencePoint(newgPnt, Nothing, txtBox_newPntName.Text, CStr(cmbBox_newPntType.SelectedItem), txtBox_newPntDescription.Text)

            VisualManoeuvringDBTool.CreateVisualReferencePoint(txtBox_newPntName.Text, txtBox_newPntDescription.Text, newgPnt)

            VisualManoeuvringDBTool.InsertVisualReferencePoints()
            'VisualManoeuvringDBTool.AddVisualReferencePoint(newVisRefPnt)
            'txtBox_newPntName.Text = ""
            'cmbBox_newPntType.SelectedIndex = 0
            'txtBox_newPntDescription.Text = ""
            MessageBox.Show("The new point is added. Please refresh the layer")
        Else
            MessageBox.Show("Please, fill in the form completely")
        End If
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

    Private Sub txtBox_newPntLatMinute_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_newPntLatMinute.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_startPntLatMinute_Validating(txtBox_newPntLatMinute, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_newPntLatMinute.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_startPntLatMinute_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_newPntLatMinute.Validating
        If Not IsNumeric(txtBox_newPntLatMinute.Text) Then Return
        If CDbl(txtBox_newPntLatMinute.Text) >= 60.0# Then
            txtBox_newPntLatMinute.Text = CStr(59)
        End If
    End Sub

    Private Sub txtBox_startPntLatSecond_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_newPntLatSecond.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            txtBox_newPntLatSecond_Validating(txtBox_newPntLatSecond, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_newPntLatSecond.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtBox_newPntLatSecond_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_newPntLatSecond.Validating
        If Not IsNumeric(txtBox_newPntLatSecond.Text) Then Return
        If CDbl(txtBox_newPntLatSecond.Text) >= 60.0# Then
            txtBox_newPntLatSecond.Text = CStr(59.99)
        End If
    End Sub

    Private Sub cmbBox_newPntLatSide_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_newPntLatSide.SelectedIndexChanged
        If cmbBox_newPntLatSide.SelectedIndex = 1 Then
            newPntLatSign = -1
        Else
            newPntLatSign = 1
        End If
    End Sub

    Private Sub cmbBox_newPntLonSide_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_newPntLonSide.SelectedIndexChanged
        If cmbBox_newPntLonSide.SelectedIndex = 1 Then
            newPntLonSign = -1
        Else
            newPntLonSign = 1
        End If
    End Sub

    Private Sub btn_cancel_Click(sender As System.Object, e As System.EventArgs) Handles btn_cancel.Click
        Me.Close()
    End Sub

    Private Sub AddVisualReferencePoints_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        _isClosed = True
    End Sub

End Class