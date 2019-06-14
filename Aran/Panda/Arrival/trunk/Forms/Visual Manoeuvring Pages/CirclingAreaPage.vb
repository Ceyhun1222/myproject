Option Strict Off
Option Explicit On
Option Compare Text

Friend Class CirclingAreaPage

    Private fVisAprOCH As Double

    Private _MSAObstacleList As ObstacleMSA()
    Public Property MSAObstacleList() As ObstacleMSA()
        Get
            Return _MSAObstacleList
        End Get
        Set(ByVal value As ObstacleMSA())
            _MSAObstacleList = value
        End Set
    End Property

    Public VisualAreaElement As ESRI.ArcGIS.Carto.IElement
    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
    Private _ConvexPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Public Property ConvexPoly() As ESRI.ArcGIS.Geometry.IPointCollection
        Get
            Return _ConvexPoly
        End Get
        Set(ByVal value As ESRI.ArcGIS.Geometry.IPointCollection)
            _ConvexPoly = value
        End Set
    End Property

    Private _RWYDATA As RWYType()
    Public Property RWYDATA() As RWYType()
        Get
            Return _RWYDATA
        End Get
        Set(ByVal value As RWYType())
            _RWYDATA = value
        End Set
    End Property
    Private _RWYIndex As Integer()
    Public Property RWYIndex() As Integer()
        Get
            Return _RWYIndex
        End Get
        Set(ByVal value As Integer())
            _RWYIndex = value
        End Set
    End Property
    Private _ObstacleList As ObstacleAr()
    Public Property ObstacleList() As ObstacleAr()
        Get
            Return _ObstacleList
        End Get
        Set(ByVal value As ObstacleAr())
            _ObstacleList = value
        End Set
    End Property

    'Private NonPrecReportFrm As CNonPrecReportFrm

    Private fRefHeight As Double

    Private _Category As Integer
    Public Property Category() As Integer
        Get
            Return _Category
        End Get
        Set(ByVal value As Integer)
            _Category = value
        End Set
    End Property
    Private _TAS As Double
    Public Property TAS() As Double
        Get
            Return _TAS
        End Get
        Set(ByVal value As Double)
            _TAS = value
        End Set
    End Property
    Private _ADElevation As Double
    Public Property ADElevation() As Double
        Get
            Return _ADElevation
        End Get
        Set(ByVal value As Double)
            _ADElevation = value
        End Set
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub CirclingAreaPage_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Label15.Text = SpeedConverter(SpeedUnit).Unit
        Label18.Text = HeightConverter(HeightUnit).Unit
        Label19.Text = HeightConverter(HeightUnit).Unit
        Label20.Text = HeightConverter(HeightUnit).Unit
        Label21.Text = SpeedConverter(SpeedUnit).Unit
        Label22.Text = DistanceConverter(DistanceUnit).Unit
        Label23.Text = DistanceConverter(DistanceUnit).Unit
        Label24.Text = DistanceConverter(DistanceUnit).Unit
        Label25.Text = HeightConverter(HeightUnit).Unit
        Label26.Text = HeightConverter(HeightUnit).Unit

        Dim I As Integer
        Dim N As Integer

        N = UBound(RWYDATA)
        pGraphics = GetActiveView().GraphicsContainer

        VisualAreaState = True

        _Category = 0

        fRefHeight = CurrADHP.pPtGeo.Z

        lstVw_RWY.Items.Clear()

        For I = 0 To N
            RWYDATA(I).Selected = True
            If (I And 1) = 0 Then
                lstVw_RWY.Items.Add(RWYDATA(I).Name & " / " & RWYDATA(I).PairName)
                lstVw_RWY.Items.Item(I \ 2).Checked = True
            End If
        Next I

        If cmbBox_AircraftCat.SelectedIndex >= 0 Then
            cmbBox_AircraftCat_SelectedIndexChanged(cmbBox_AircraftCat, New System.EventArgs())
        Else
            cmbBox_AircraftCat.SelectedIndex = 0
        End If


    End Sub

    Private Sub VsManev(Optional ByRef newH As Double = -1.0)
        Dim Ix As Integer
        Dim OIx As Integer

        Dim H As Double
        Dim Res As Double
        Dim R__ As Double
        Dim Bank As Double
        Dim lTAS As Double

        Ix = cmbBox_AircraftCat.SelectedIndex
        txtBox_IAS.Text = CStr(ConvertSpeed(cVva.Values(Ix), eRoundMode.rmSPECIAL))

        lTAS = IAS2TAS(3.6 * cVva.Values(Ix), (CurrADHP.pPtGeo.Z), CurrADHP.ISAtC)
        'Bank = System.Math.Round(RadToDeg(System.Math.Atan(0.003 * PI * (lTAS + arVisualWS.Value / arVisualWS.Multiplier) / 6.355)) - 0.4999) 'System.Math.Atan(0.003 * PI * lTAS / 6.355) '
        Bank = RadToDeg(System.Math.Atan(0.003 * PI * (lTAS + arVisualWS.Value / arVisualWS.Multiplier) / 6.355))
        If Bank > arVisAverBank.Value Then Bank = arVisAverBank.Value

        txtBox_BankAngle.Text = CStr(System.Math.Round(Bank - 0.4999))
        'txtBox_BankAngle.Text = CStr(Bank)
        txtBox_RateOfTurn.Text = CStr(System.Math.Round(6355.0 * System.Math.Tan(DegToRad(Bank)) / (PI * (lTAS + arVisualWS.Value / arVisualWS.Multiplier)), 1))
        _ADElevation = CurrADHP.pPtGeo.Z

        '    RWShapeField = pRWYTable.FindField("Shape")
        H = CalcMOC(_ADElevation, Bank, Ix, OIx)
        fVisAprOCH = H

        _TAS = lTAS + arVisualWS.Value / arVisualWS.Multiplier
        txtBox_TASWind.Text = CStr(ConvertSpeed(lTAS * 0.277777777777778 + arVisualWS.Value, eRoundMode.rmNERAEST))
        Res = Bank2Radius(Bank, lTAS + arVisualWS.Value / arVisualWS.Multiplier)

        txtBox_RadiusOfTurn.Text = CStr(ConvertDistance(Res, eRoundMode.rmNERAEST))
        txtBox_StraightSegment.Text = CStr(ConvertDistance(carStraightSegmen.Values(Ix), eRoundMode.rmNERAEST))

        R__ = 2.0 * Res + carStraightSegmen.Values(Ix)
        txtBox_RadiusFromTHR.Text = CStr(ConvertDistance(R__, eRoundMode.rmNERAEST))

        '=============================================================
        txtBox_ADElev.Text = CStr(ConvertHeight(_ADElevation, eRoundMode.rmNERAEST))
        txtBox_MOC.Text = CStr(ConvertHeight(arObsClearance.Values(Ix), eRoundMode.rmNERAEST))
        txtBox_MinOCH.Text = CStr(ConvertHeight(arMinOCH.Values(Ix), eRoundMode.rmNERAEST))

        txtBox_OCH.Text = CStr(ConvertHeight(fVisAprOCH, eRoundMode.rmCEIL))
        txtBox_OCA.Text = CStr(ConvertHeight(fVisAprOCH + CurrADHP.pPtGeo.Z, eRoundMode.rmCEIL))
        If OIx >= 0 Then
            txtBox_ObstacleID.Text = _MSAObstacleList(OIx).ID
        Else
            txtBox_ObstacleID.Text = ""
        End If

        On Error Resume Next
        If Not VisualAreaElement Is Nothing Then pGraphics.DeleteElement(VisualAreaElement)
        VisualAreaElement = DrawPolygon(_ConvexPoly, RGB(0, 0, 255), , False)
        If VisualAreaState Then
            pGraphics.AddElement(VisualAreaElement, 0)
            VisualAreaElement.Locked = True
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If
        On Error GoTo 0
    End Sub

    Private Function CalcMOC(ByVal H As Double, ByVal Bank As Double, ByVal cIx As Integer, ByRef OIx As Integer) As Double
        Dim I As Integer

        Dim newH As Double
        Dim lTAS As Double
        Dim fTmp As Double
        Dim Res As Double
        Dim R__ As Double

        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim sumPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry
        Dim ptCentr As ESRI.ArcGIS.Geometry.IPoint

        lTAS = IAS2TAS(3.6 * cVva.Values(cIx), H, CurrADHP.ISAtC)
        '=============================================================
        sumPoly = New ESRI.ArcGIS.Geometry.Polygon
        fTmp = lTAS + arVisualWS.Value / arVisualWS.Multiplier
        Res = Bank2Radius(Bank, fTmp)
        R__ = 2.0 * Res + carStraightSegmen.Values(cIx)

        For I = 0 To UBound(RWYDATA)
            If RWYDATA(I).Selected Then
                ptCentr = RWYDATA(I).pPtPrj(eRWY.PtTHR)

                pGeometry = sumPoly
                If pGeometry.IsEmpty() Then
                    sumPoly = CreatePrjCircle(ptCentr, R__)
                Else
                    pTopo = sumPoly
                    pTopo.IsKnownSimple_2 = False

                    pTopo.Simplify()
                    sumPoly = pTopo.Union(CreatePrjCircle(ptCentr, R__))
                End If
            End If
        Next I
        pTopo = sumPoly
        _ConvexPoly = pTopo.ConvexHull

        pTopo = _ConvexPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        newH = MaxObstacleHeightInPoly(ObstacleList, _MSAObstacleList, _ConvexPoly, fRefHeight, OIx)
        newH = newH + arObsClearance.Values(cIx)

        If newH < arMinOCH.Values(cIx) Then
            newH = arMinOCH.Values(cIx)
            OIx = -1
        End If
        CalcMOC = newH

        'NonPrecReportFrm.FillPageVMA(_MSAObstacleList, arObsClearance.Values(cIx), OIx)
    End Function

    Private Sub cmbBox_AircraftCat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBox_AircraftCat.SelectedIndexChanged
        _Category = cmbBox_AircraftCat.SelectedIndex
        VsManev()
    End Sub

    Private Sub lstVw_RWY_ItemChecked(sender As System.Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles lstVw_RWY.ItemChecked
        If cmbBox_AircraftCat.SelectedIndex < 0 Then Return
        Static Dim InUse As Boolean = False
        If InUse Then Return
        InUse = True

        Dim Item As System.Windows.Forms.ListViewItem = e.Item 'ListView0001.Items(eventArgs.Index)
        Dim I As Integer
        Dim N As Integer
        Dim Checked As Boolean

        Try
            N = 0
            For I = 0 To lstVw_RWY.Items.Count - 1
                Checked = lstVw_RWY.Items.Item(I).Checked
                RWYDATA(I * 2).Selected = Checked
                RWYDATA(I * 2 + 1).Selected = Checked
                If Checked Then N = N + 2
            Next I

            If N = 0 Then
                Item.Checked = True
                I = Item.Index
                RWYDATA(I * 2).Selected = True
                RWYDATA(I * 2 + 1).Selected = True
            Else
                VsManev()
            End If
        Finally
            InUse = False
        End Try
    End Sub
End Class
