Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports Aran.Aim
Imports Aran.Aim.Data
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Enums
Imports Aran.Aim.Features
Imports Aran.AranEnvironment
Imports Aran.Converters
Imports Aran.Geometries
Imports Aran.Metadata.Utils
Imports Aran.PANDA.Common
Imports Aran.Queries
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geometry

<ComVisible(False)> Friend Class InitialApproach
    Inherits Form

    Private Structure IAPArrayItem
        Dim LegIndex As Integer
        Dim pIAP As InstrumentApproachProcedure
        Dim pProcedureTransition As ProcedureTransition
    End Structure

    Private Const MaxStraightSegLength As Double = 650000.0#
    Private Const IntDist As Double = 650000.0#
    Private Const BigDist As Double = 10000000.0#
    Private Const distError As Double = 30.0

    Private ptElem As IElement
    Private pPlyElem As IElement
    Private pNomLineElem As IElement
    Private pFAFptElement As IElement
    Private pFAFAreaElement As IElement
    Private pNominalElement As IElement
    Private pIAF_IAreaElement As IElement
    Private pIAF_IIAreaElement As IElement
    Private pReportIAreaElement As IElement
    Private pReportIIAreaElement As IElement
    Private pGraphics As IGraphicsContainer
    Private screenCapture As IScreenCapture

    '==========================================
    Private bUnloadByOk As Boolean

    Private iCategory As Integer
    Private UboundFIXs As Integer
    Private StepDownsNum As Integer

    Private IAF_PDG As Double
    Private MOCLimit As Double
    Private fFIXHeight As Double
    Private fRefHeight As Double
    Private arMinISlen As Double
    Private arAreaHalfWidth As Double
    Private MinISlensArray(5) As Double
    Private EnrouteMOCValues As Object

    Private ptKT1 As IPoint

    Private pIFptPrj As IPoint
    Private pFAFptPrj As IPoint
    Private pIFPoint As TerminalSegmentPoint
    Private pIFptGeo As IPoint
    Private pIFTolerArea As IPolygon
    Private pGuidPoly As IPointCollection
    Private pNominalPoly As IPointCollection
    Private IAF_IAreaPoly As IPointCollection
    Private IAF_IIAreaPoly As IPointCollection

    Private StartPoint As WPT_FIXType
    Private CurrInterval As Interval
    Private TextBox110Range As Interval
    Private SelectedTransition As IAPArrayItem

    Private ProtRep As ReportFile
    Private AccurRep As ReportFile
    Private pProcedure As Procedure

    Private LegList() As StepDownFIX
    Private StepDownFIXs() As StepDownFIX

    Private TransArray As List(Of IAPArrayItem)

    Private Category As Integer
    Private pLandingTakeoff As LandingTakeoffAreaCollection

    Private bFirstPointIsIF As Boolean
    Private bIsHolding As Boolean

	Private RepFileTitle As String

	Private ComboBox101_Intervals00() As Interval
    Private ComboBox101_Intervals01() As Interval
    Private ComboBox101_Intervals10() As Interval
    Private ComboBox101_Intervals11() As Interval

    Private IAFObstList4FIX As ObstacleContainer
    Private IAFObstList4Turn As ObstacleContainer
    Private IAFProhibSectors() As IFProhibitionSector

    Private InitialReportsFrm As CInitialReportsFrm
    Private HelpContextID As Integer = 12100
    Private bFormInitialised As Boolean = False

    Private turnMagBearing As Double
    Private segmentLegs As List(Of SegmentLeg)
    Private rwyDirList As List(Of RunwayDirection)

    Private PageLabel() As Label

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        MinISlensArray(0) = arImRange_Min.Value
        MinISlensArray(1) = arMinISlen91_96.Value
        MinISlensArray(2) = arMinISlen97_02.Value
        MinISlensArray(3) = arMinISlen03_08.Value
        MinISlensArray(4) = arMinISlen09_14.Value
        MinISlensArray(5) = arMinISlen15_20.Value

        bFormInitialised = True
        InitialReportsFrm = New CInitialReportsFrm()
        Me.HelpContextID = 12100

        pGraphics = GetActiveView().GraphicsContainer
        UboundFIXs = 99

        ReDim StepDownFIXs(UboundFIXs)

        'FillADHPList()
        FillProcedureList()


        FillOtherControls()

        LoadRwyList()


        ReDim IAFProhibSectors(-1)

        pGraphics = GetActiveView().GraphicsContainer
        InitialReportsFrm.SetReportBtn(ReportBtn)

        PageLabel = {_Label11_0, _Label11_1, _Label11_2}

        PageLabel(0).Text = MultiPage1.TabPages.Item(0).Text
        PageLabel(1).Text = MultiPage1.TabPages.Item(1).Text
        PageLabel(2).Text = MultiPage1.TabPages.Item(2).Text

        FocusStepCaption(0)

        MultiPage1.Top = -21
        Me.Height = Me.Height - 21
        Frame1.Top = Frame1.Top - 21

        InfoBtn.Checked = False
        InfoBtn_CheckedChanged(InfoBtn, New EventArgs())
        OkBtn.Enabled = False
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)

        Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
        AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
        AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
        MyBase.WndProc(m)

        If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
            Dim about As AboutForm = New AboutForm()
            about.ShowDialog(Me)
            about = Nothing
        End If
    End Sub

    Private Sub LoadRwyList()
        Dim runwayList As List(Of Descriptor) = DBModule.pObjectDir.GetRunwayList(CurrADHP.Identifier)
        If (runwayList Is Nothing) Then
            Exit Sub
        End If

        rwyDirList = New List(Of RunwayDirection)
        For Each runway As Descriptor In runwayList
            Dim tmpRwyDirList = DBModule.pObjectDir.GetRunwayDirectionList(runway.Identifier)

            If (Not (rwyDirList Is Nothing)) Then

                For Each rwyDir As RunwayDirection In tmpRwyDirList
                    ChkRwyDirections.Items.Add(rwyDir.Designator)
                    rwyDirList.Add(rwyDir)
                Next
            End If
        Next
    End Sub

    Private Sub FillOtherControls()
        Dim I As Integer

        ComboBox004.SelectedIndex = 0
        SetComboDroppedWidth(ComboBox002, 1.5 * ComboBox002.Width)
        Frame102.Top = Frame101.Top
        Frame102.Left = Frame101.Left

        TextBox105.Text = CStr(ConvertDistance(arIFTolerance.Value, eRoundMode.NEAREST))
        TextBox105.Tag = TextBox105.Text

        Label009.Text = HeightConverter(HeightUnit).Unit
        Label011.Text = DistanceConverter(DistanceUnit).Unit
        Label102.Text = DistanceConverter(DistanceUnit).Unit
        Label119.Text = DistanceConverter(DistanceUnit).Unit
        Label128.Text = HeightConverter(HeightUnit).Unit
        Label129.Text = DistanceConverter(DistanceUnit).Unit

        Label202.Text = HeightConverter(HeightUnit).Unit
        Label204.Text = HeightConverter(HeightUnit).Unit

        Label018.Text = DistanceConverter(DistanceUnit).Unit
        Label013.Text = HeightConverter(HeightUnit).Unit
        '===============================================
        PrevBtn.Text = str00150
        NextBtn.Text = str00151
        OkBtn.Text = str40229
        CancelBtn.Text = str00153
        ReportBtn.Text = str00154
        SaveReportBtn.Text = str40230
        SaveReportBtn.Enabled = False

        Me.Text = str00003
        MultiPage1.TabPages.Item(0).Text = str40001
        MultiPage1.TabPages.Item(1).Text = str40002
        MultiPage1.TabPages.Item(2).Text = str40003
        MultiPage1.ItemSize = New Size(0, 1)

        Frame001.Text = str40100
        Frame002.Text = str40100

        OptionButton001.Text = str40109
        OptionButton002.Text = str40110

        Frame101.Text = str40222
        Frame102.Text = str40222
        Frame103.Text = str40200

        Label001.Text = str40101
        Label002.Text = str40102
        Label003.Text = str40103
        Label004.Text = str10406
        Label006.Text = str00232
        Label008.Text = str40106
        Label010.Text = str40107
        Label012.Text = str00612
        Label016.Text = str50106
        Label017.Text = str40104

        OptionButton101.Text = str40201
        OptionButton102.Text = str40202

        OptionButton103.Text = str13014
        OptionButton104.Text = str13015

        OptionButton105.Text = str10512
        OptionButton106.Text = str10513

        Label101.Text = str40203
        Label105.Text = str40217
        Label107.Text = str40216
        Label108.Text = str40215
        Label110.Text = str40108
        Label113.Text = str10503
        Label115.Text = str40106
        Label117.Text = str40209
        Label118.Text = str40210
        Label130.Text = str13016
        Label132.Text = str13016
        Label133.Text = ""

        CheckBox101.Text = str13015

        EnrouteMOCValues = New Object() {300.0#, 450.0#, 600.0#}

        ComboBox006.Items.Clear()
        For I = 0 To UBound(EnrouteMOCValues)

            ComboBox006.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(I), 4)))
        Next I
        ComboBox006.SelectedIndex = 0

        'MOCLimit

        ComboBox005.Items.Clear()
        ComboBox005.Items.Add(CStr(ConvertDistance(9260, 2)))
        ComboBox005.Items.Add(CStr(ConvertDistance(14816, 2)))
        ComboBox005.SelectedIndex = 0

        ComboBox102.Items.Clear()
        ComboBox102.Items.Add(str40218)
        ComboBox102.Items.Add(str40219)
        ComboBox102.SelectedIndex = 0

        ComboBox106.Items.AddRange(New Object() {str50206, str50207})
        Label121.Text = str40225

        Me.ComboBox105.Items.AddRange(New Object() {str40227, str40228})
        Label116.Text = str40226

        RemoveBtn.Text = str40213
        AddBtn.Text = str40214


        MultiPage1.Tag = "1"
        MultiPage1.SelectedIndex = 0
        MultiPage1.Tag = "0"
    End Sub

    'Private Sub FillADHPList()

    '	ComboBox001.Items.Clear()
    '	ComboBox001.Items.Add(CurrADHP.pAirportHeliport.Designator)
    '	ComboBox001.SelectedIndex = 0
    '	ComboBox001.Enabled = False
    'End Sub

    Private Sub FillProcedureList()
        Dim pIAPList As List(Of InstrumentApproachProcedure)
        Dim I As Integer
        Dim pIAP As InstrumentApproachProcedure
        Dim lastMID As Guid
        Dim M As Integer
        Dim J As Integer
        Dim pProcedureTransition As ProcedureTransition
        Dim L As Integer
        Dim IL As Integer
        Dim pProcedureLeg As SegmentLeg
        Dim bTransIsValid As Boolean

        ComboBox002.Items.Clear()

        pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)



        lastMID = New Guid()
        '==========================================================================================
        For I = 0 To pIAPList.Count - 1
            pIAP = pIAPList.Item(I)

            If pIAP.Name Is Nothing Then Continue For

            If (InStr(1, pIAP.Name, "RNAV", vbTextCompare) = 0) And (pIAP.Identifier <> lastMID) Then
                M = pIAP.FlightTransition.Count
                For J = 0 To M - 1
                    pProcedureTransition = pIAP.FlightTransition.Item(J)

                    L = pProcedureTransition.TransitionLeg.Count
                    If L > 0 Then
                        If pProcedureTransition.Type.Value = CodeProcedurePhase.APPROACH Then
                            ComboBox002.Items.Add(pIAP)
                            lastMID = pIAP.Identifier
                            Exit For
                        Else
                            bTransIsValid = False
                            For IL = 0 To L - 1
                                pProcedureLeg = pProcedureTransition.TransitionLeg.Item(IL).TheSegmentLeg.GetFeature()

                                If pProcedureLeg Is Nothing Then Continue For
                                If pProcedureLeg.EndPoint Is Nothing Then Continue For
                                If pProcedureLeg.EndPoint.Role Is Nothing Then Continue For
                                If pProcedureLeg.StartPoint Is Nothing Then Continue For

                                If pProcedureLeg.EndPoint.Role.Value = CodeProcedureFixRole.FAF Then
                                    ComboBox002.Items.Add(pIAP)
                                    lastMID = pIAP.Identifier
                                    bTransIsValid = True
                                    Exit For
                                End If
                            Next IL

                            If bTransIsValid Then Exit For
                        End If
                    End If
                Next J
            End If
        Next I


        If ComboBox002.Items.Count > 0 Then
            ComboBox002.SelectedIndex = 0
        End If
    End Sub

    Private Function CalcL(ByRef Alpha As Double, ByRef Betta As Double, ByRef RDME As Double, ByRef rDMEObs As Double) As Double
        CalcL = Math.Sqrt(RDME * RDME + rDMEObs * rDMEObs - 2 * RDME * rDMEObs * Math.Cos(Alpha - Betta))
    End Function

    Private Function CalcReffectiv(ByRef Alpha As Double, ByRef hIF As Double, ByRef Hi As Double, ByRef RDME As Double, ByRef PDG As Double, ByRef C155 As Double) As Double
        CalcReffectiv = arAreaHalfWidth - C155 * (hIF - Hi + Alpha * RDME * PDG)
    End Function

    Private Function AlphaR(ByRef Reffectiv As Double, ByRef hIF As Double, ByRef Hi As Double, ByRef RDME As Double, ByRef PDG As Double, ByRef C155 As Double) As Double
        AlphaR = ((arAreaHalfWidth - Reffectiv) / C155 - hIF + Hi) / (RDME * PDG)
    End Function

    Private Function AlphaL(ByRef Betta As Double, ByRef RDME As Double, ByRef rDMEObs As Double, ByRef L As Double) As Double
        AlphaL = Betta - ArcCos((RDME * RDME + rDMEObs * rDMEObs - L * L) / (2 * RDME * rDMEObs))
    End Function

    Private Function CreateGuidPoly(ByRef GuidNav As NavaidData, ByRef ptFIX As IPoint) As ESRI.ArcGIS.Geometry.Polygon
        Dim fDist As Double
        Dim fTmp As Double
        Dim fSlDist As Double
        Dim fRadius As Double
        Dim fIADir As Double
        Dim TrackToler As Double
        Dim TrackRange As Double

        Dim pBaseLine As ILine
        Dim pPoly As IPointCollection
        Dim pTmpPoly1 As IPointCollection
        Dim pTopoOper As ITopologicalOperator2

        If GuidNav.TypeCode = eNavaidType.DME Then
            pBaseLine = New ESRI.ArcGIS.Geometry.Line
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = ptFIX
            fDist = pBaseLine.Length

            fTmp = ptFIX.Z - GuidNav.pPtPrj.Z
            fSlDist = Math.Sqrt(fDist * fDist + fTmp * fTmp)

            fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
            fRadius = fDist + fTmp

            pTmpPoly1 = CreatePrjCircle(GuidNav.pPtPrj, fRadius)

            fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
            fRadius = fDist - fTmp

            pTopoOper = pTmpPoly1
            pPoly = pTopoOper.Difference(CreatePrjCircle(GuidNav.pPtPrj, fRadius))
            '=================================================================
        Else
            '=================================================================
            fRadius = -100000.0
            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                TrackToler = VOR.TrackingTolerance
                fRadius = VOR.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                TrackToler = NDB.TrackingTolerance
                fRadius = NDB.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                TrackToler = LLZ.TrackingTolerance
            End If

            fTmp = ReturnDistanceInMeters(ptFIX, GuidNav.pPtPrj)

            If fTmp < TrackToler Then
                fIADir = StepDownFIXs(StepDownsNum - 1).InDir
            Else
                fIADir = ReturnAngleInDegrees(ptFIX, GuidNav.pPtPrj)
            End If

            TrackRange = GuidNav.Range / Math.Cos(DegToRad(TrackToler))

            pPoly = New ESRI.ArcGIS.Geometry.Polygon
            pPoly.AddPoint(GuidNav.pPtPrj)
            pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler, 100.0 * TrackRange))
            pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler, 100.0 * TrackRange))
            pPoly.AddPoint(GuidNav.pPtPrj)
            pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler + 180.0, 100.0 * TrackRange))
            pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler + 180.0, 100.0 * TrackRange))
            pPoly.AddPoint(GuidNav.pPtPrj)
        End If
        '=================================================================
        pTopoOper = pPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        Return pPoly
    End Function

    Function FillComboBox101() As Integer
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim M As Integer
        Dim N As Integer

        Dim dH As Double
        Dim fDist As Double
        Dim fRDME As Double
        Dim fADME As Double
        Dim fTurnR As Double
        Dim MaxDist As Double
        Dim maxLegLenght As Double
        Dim DirNavToObst As Double
        Dim AircraftSpeed As Double
        Dim maxPosLegLenght As Double

        Dim pPtNAV As IPoint
        Dim NextFIX As StepDownFIX

        Dim Interval00 As Interval
        Dim Interval01 As Interval
        Dim Interval10 As Interval
        Dim Interval11 As Interval
        Dim TmpInterval As List(Of Interval)

        ComboBox101.Items.Clear()

        N = UBound(NavaidList)
        M = UBound(DMEList)
        K = N + M + 1

        If K < 0 Then

            ReDim ComboBox101_Intervals00(-1)

            ReDim ComboBox101_Intervals01(-1)

            ReDim ComboBox101_Intervals10(-1)

            ReDim ComboBox101_Intervals11(-1)


            Return 0
        End If

        '==================================================================

        ReDim ComboBox101_Intervals00(K)
        ReDim ComboBox101_Intervals01(K)
        ReDim ComboBox101_Intervals10(K)
        ReDim ComboBox101_Intervals11(K)


        NextFIX = StepDownFIXs(StepDownsNum - 1)

        AircraftSpeed = cViafMax.Values(iCategory)
        fTurnR = Bank2Radius(arInitApprBank, IAS2TAS(3.6 * AircraftSpeed, (NextFIX.pPtPrj.Z), C_ISAtC))
        maxLegLenght = 15000000.0#

        If NextFIX.GuidanceNav.TypeCode <> eNavaidType.DME Then
            maxPosLegLenght = MaxStraightSegLength
        Else
            fRDME = ReturnDistanceInMeters(NextFIX.GuidanceNav.pPtPrj, NextFIX.pPtPrj)
            maxPosLegLenght = 120.0# * DegToRadValue * fRDME
        End If

        If UBound(IAFObstList4Turn.Parts) >= 0 Then
            If NextFIX.GuidanceNav.TypeCode <> eNavaidType.DME Then
                maxLegLenght = Point2LineDistancePrj(NextFIX.pPtPrj, IAFObstList4Turn.Parts(0).pPtPrj, NextFIX.InDir + 90.0#) - IAFProhibSectors(IAFObstList4Turn.Parts(0).SectorIndex).rObs
            Else
                fADME = ReturnAngleInDegrees(NextFIX.GuidanceNav.pPtPrj, NextFIX.pPtPrj)
                DirNavToObst = ReturnAngleInDegrees(NextFIX.GuidanceNav.pPtPrj, IAFObstList4Turn.Parts(0).pPtPrj)
                fRDME = ReturnDistanceInMeters(NextFIX.GuidanceNav.pPtPrj, NextFIX.pPtPrj)
                maxLegLenght = fRDME * DegToRad(Modulus((DirNavToObst - fADME) * NextFIX.ArcDir)) - IAFProhibSectors(IAFObstList4Turn.Parts(0).SectorIndex).rObs
            End If
        End If

        maxLegLenght = Min(maxLegLenght, maxPosLegLenght)

        J = -1

        Try

            For I = 0 To N
                If NavaidList(I).TypeCode = eNavaidType.LLZ Then Continue For

                pPtNAV = NavaidList(I).pPtPrj

                fDist = ReturnDistanceInMeters(NextFIX.pPtPrj, pPtNAV)

                'If fDist > 0.5 * MaxNAVDist Then Continue For
                If fDist > MaxNAVDist Then Continue For

                dH = Math.Abs(NextFIX.pPtPrj.Z - pPtNAV.Z)
                MaxDist = 4130.0# * 2.0# * Math.Sqrt(dH)

                If fDist > MaxDist Then Continue For

                If NextFIX.Track = TrackType.Straight Then

                    Interval00 = CalcNavInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, eSide.Left)
                    Interval01.Tag = -1

                    Interval10 = CalcNavInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, eSide.Right)
                    Interval11.Tag = -1
                Else
                    TmpInterval = CalcLeaveArcInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, (eSide.Right))
                    If TmpInterval.Count = 2 Then

                        Interval00 = TmpInterval(0)

                        Interval01 = TmpInterval(1)
                    ElseIf TmpInterval.Count = 1 Then

                        Interval00 = TmpInterval(0)
                        Interval01.Tag = -1
                    Else
                        Interval00.Tag = -1
                        Interval01.Tag = -1
                    End If

                    TmpInterval = CalcLeaveArcInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, (eSide.Left))
                    If TmpInterval.Count = 2 Then

                        Interval10 = TmpInterval(0)

                        Interval11 = TmpInterval(1)
                    ElseIf TmpInterval.Count = 1 Then

                        Interval10 = TmpInterval(0)
                        Interval11.Tag = -1
                    Else
                        Interval10.Tag = -1
                        Interval11.Tag = -1
                    End If
                End If
                J = CalcInterSectionVariants(I, Interval00, Interval01, Interval10, Interval11, J, NavaidList)

            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        If NextFIX.Track = TrackType.Straight Then
            For I = 0 To M
                pPtNAV = DMEList(I).pPtPrj


                Interval00 = CalcDMEInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, -1, -1) 'to left from inside

                Interval01 = CalcDMEInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, -1, 1) 'to left from outside


                Interval10 = CalcDMEInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, 1, -1) 'to right from inside

                Interval11 = CalcDMEInterval(NextFIX, maxLegLenght, pPtNAV, fTurnR, 1, 1) 'to right from outside

                J = CalcInterSectionVariants(I, Interval00, Interval01, Interval10, Interval11, J, DMEList)
            Next I
        End If

        If J < 0 Then

            ReDim ComboBox101_Intervals00(-1)

            ReDim ComboBox101_Intervals01(-1)

            ReDim ComboBox101_Intervals10(-1)

            ReDim ComboBox101_Intervals11(-1)
            Return 0
        End If

        ReDim Preserve ComboBox101_Intervals00(J)
        ReDim Preserve ComboBox101_Intervals01(J)
        ReDim Preserve ComboBox101_Intervals10(J)
        ReDim Preserve ComboBox101_Intervals11(J)


        Return J + 1
    End Function

    Private Function CalcInterSectionVariants(I As Integer, Interval00 As Interval, Interval01 As Interval, Interval10 As Interval, Interval11 As Interval, J As Integer, NavList As NavaidData()) As Integer
        Dim C As Integer

        If (Interval00.Tag >= 0) Or (Interval01.Tag >= 0) Or (Interval10.Tag >= 0) Or (Interval11.Tag >= 0) Then
            J = J + 1

            ComboBox101_Intervals00(J) = Interval00

            ComboBox101_Intervals01(J) = Interval01

            ComboBox101_Intervals10(J) = Interval10

            ComboBox101_Intervals11(J) = Interval11

            C = 0
            If (Interval00.Tag >= 0) Then C = 1
            If (Interval01.Tag >= 0) Then C = C + 2

            If (Interval10.Tag >= 0) Then C = C + 4
            If (Interval11.Tag >= 0) Then C = C + 8


            Dim tmpNavaid As NavaidData = NavList(I)
            tmpNavaid.Tag = C
            ComboBox101.Items.Add(tmpNavaid)
        End If
        Return J
    End Function

    Function FillComboBox104() As Integer
        Dim C As Integer
        Dim I As Integer
        ' Dim J As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer

        Dim ApplicableNavDist As Double
        Dim maxPosLegLenght As Double
        Dim maxLegLenght As Double
        Dim DirNavToObst As Double
        Dim fInterToler As Double
        'Dim fInterRange As Double

        Dim fConeAngle As Double
        Dim NavArcDist As Double
        Dim fOnNavRad As Double
        Dim MaxTheta As Double
        Dim maxInter As Double
        Dim ConRad As Double
        'Dim fmADME As Double
        Dim fADME As Double
        Dim fRDME As Double
        Dim numer As Double
        Dim dPrec As Double
        Dim fDist As Double
        Dim dAng0 As Double
        Dim dAng1 As Double
        Dim fDir As Double
        Dim hFix As Double
        Dim absY As Double
        Dim Dir1 As Double
        Dim Dir2 As Double
        Dim Xnav As Double
        Dim Ynav As Double
        Dim dX0 As Double
        Dim dX1 As Double
        Dim dX As Double

        Dim FullRange As Interval
        Dim CurrRange As Interval
        Dim Interval0 As Interval
        Dim Interval1 As Interval
        Dim NavRange As Interval
        Dim DMEInterval() As Interval

        Dim OnNavList() As NavaidData
        Dim NextFIX As StepDownFIX
        Dim GuidNav As NavaidData

        Dim OldNav As NavaidData

        Dim pt1 As IPoint
        Dim pt2 As IPoint

        N = UBound(NavaidList)
        M = UBound(DMEList)

        K = N + M + 1

        If K < 0 Then
            Return 0
        End If

        'ReDim ComboBox104_Intervals(0 To K)
        ReDim OnNavList(K)
        '================================================
        OldNav.Index = -1

        If ComboBox104.SelectedIndex > -1 Then OldNav = ComboBox104.SelectedItem


        NextFIX = StepDownFIXs(StepDownsNum - 1)

        GuidNav = NextFIX.GuidanceNav
        hFix = fFIXHeight

        maxLegLenght = 100000000.0#

        If GuidNav.TypeCode <> eNavaidType.DME Then
            maxPosLegLenght = MaxStraightSegLength
        Else
            fRDME = ReturnDistanceInMeters(GuidNav.pPtPrj, NextFIX.pPtPrj)
            fADME = ReturnAngleInDegrees(GuidNav.pPtPrj, NextFIX.pPtPrj)
            maxPosLegLenght = 179.0# * DegToRadValue * fRDME
        End If

        If UBound(IAFObstList4FIX.Parts) >= 0 Then
            If GuidNav.TypeCode <> eNavaidType.DME Then
                maxLegLenght = Point2LineDistancePrj(NextFIX.pPtPrj, IAFObstList4FIX.Parts(0).pPtPrj, NextFIX.InDir + 90.0#) '- IAFProhibSectors(IAFObstList4FIX(0).Index).rObs
            Else
                DirNavToObst = ReturnAngleInDegrees(GuidNav.pPtPrj, IAFObstList4FIX.Parts(0).pPtPrj)
                maxLegLenght = fRDME * DegToRad(Modulus((DirNavToObst - fADME) * NextFIX.ArcDir)) '- IAFProhibSectors(IAFObstList4FIX(0).Index).rObs
            End If
        End If

        maxLegLenght = Min(maxLegLenght, maxPosLegLenght)

        If IsNumeric(TextBox105.Text) Then
            dPrec = DeConvertDistance(CDbl(TextBox105.Text))
        Else
            dPrec = arIFTolerance.Value
        End If

        '================================================

        K = -1

        ComboBox104.Items.Clear()

        If GuidNav.TypeCode = eNavaidType.DME Then
            '        FullRange.Left = Max(dPrec, Point2PointArcDistance(NextFIX.pPtPrj, NextFIX.pPtStart, GuidNav.pPtPrj, fRDME, NextFIX.ArcDir))
            '        FullRange.Right = maxLegLenght - dPrec
            If NextFIX.ArcDir > 0 Then
                FullRange.Left = fADME + DistToArcAngle(Max(dPrec, Point2PointArcDistance(NextFIX.pPtPrj, NextFIX.pPtStart, GuidNav.pPtPrj, fRDME, NextFIX.ArcDir)), fRDME)
                FullRange.Right = fADME + DistToArcAngle(maxLegLenght - dPrec, fRDME)
            Else
                FullRange.Left = fADME - DistToArcAngle(maxLegLenght - dPrec, fRDME)
                FullRange.Right = fADME - DistToArcAngle(Max(dPrec, Point2PointArcDistance(NextFIX.pPtPrj, NextFIX.pPtStart, GuidNav.pPtPrj, fRDME, NextFIX.ArcDir)), fRDME)
            End If
            FullRange.Tag = 1

            For I = 0 To N
                If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                    ConRad = Math.Tan(DegToRad(VOR.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                    fOnNavRad = VOR.OnNAVRadius
                    fConeAngle = VOR.ConeAngle

                    fInterToler = DegToRadValue * VOR.IntersectingTolerance
                ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                    ConRad = Math.Tan(DegToRad(NDB.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                    fOnNavRad = NDB.OnNAVRadius
                    fConeAngle = NDB.ConeAngle

                    fInterToler = DegToRadValue * NDB.IntersectingTolerance
                ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
                    '                ConRad = 250000#
                    '                fOnNavRad = -10000#
                    '                fInterToler = DegToRadValue * LLZ.IntersectingTolerance
                    Continue For
                End If

                ApplicableNavDist = (fRDME + dPrec) / fInterToler
                fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, NavaidList(I).pPtPrj)
                If fDist > ApplicableNavDist Then Continue For

                fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, NavaidList(I).pPtPrj)

                NavArcDist = Modulus(NextFIX.ArcDir * (fDir - fADME)) * DegToRadValue * fRDME

                absY = Math.Abs(fDist - fRDME)
                If absY < fOnNavRad Then
                    If (NavArcDist < maxLegLenght) And (FullRange.Left + fOnNavRad < Xnav) And (FullRange.Right - fOnNavRad > Xnav) Then
                        K = K + 1

                        OnNavList(K) = NavaidList(I) 'NavaidData2FixableNavaid(NavaidList(I))
                        Continue For
                    End If
                End If

                If (ConRad > absY) And (NavArcDist < maxLegLenght) Then Continue For

                If (fDist > 0.5 * fRDME) Then
                    'MaxTheta = (ArcSin(0.5 * fRDME / fDist) - fInterToler) * RadToDegValue
                    'If MaxTheta < 0# Then GoTo ContinueDMENav
                    MaxTheta = (ArcSin(0.5 * fRDME / fDist)) * RadToDegValue

                    dAng0 = 90.0# - MaxTheta - ArcCos(fDist * Math.Sin(MaxTheta * DegToRadValue) / fRDME) * RadToDegValue
                    dAng1 = 90.0# - MaxTheta + ArcCos(fDist * Math.Sin(MaxTheta * DegToRadValue) / fRDME) * RadToDegValue

                    NavRange.Left = fDir - dAng0
                    NavRange.Right = fDir + dAng0
                    AngleIntervalIntersection(NavRange, FullRange, Interval0)

                    NavRange.Left = fDir + dAng1
                    NavRange.Right = fDir - dAng1
                    AngleIntervalIntersection(NavRange, FullRange, Interval1)

                    C = Interval0.Tag + Interval1.Tag
                    L = Interval0.Tag + 2 * Interval1.Tag

                    If (C = 1) And (Interval0.Tag = 0) Then

                        Interval0 = Interval1
                    End If
                Else
                    C = 1
                    L = 0

                    Interval0 = FullRange
                End If
                If C = 0 Then Continue For


                Dim navaidTmp As NavaidData = NavaidList(I)
                navaidTmp.IntersectionType = eIntersectionType.ByAngle

                ReDim navaidTmp.ValMin(C - 1)
                ReDim navaidTmp.ValMax(C - 1)
                navaidTmp.ValCnt = L

                pt1 = PointAlongPlane(GuidNav.pPtPrj, Interval0.Left, fRDME)
                pt2 = PointAlongPlane(GuidNav.pPtPrj, Interval0.Right, fRDME)

                'DrawPointWithText pt1, "pt1"
                'DrawPointWithText pt2, "pt2"

                Dir1 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt1)
                Dir2 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt2)

                If AnglesSideDef(Dir1, Dir2) < 0 Then
                    navaidTmp.ValMin(0) = Dir1
                    navaidTmp.ValMax(0) = Dir2
                Else
                    navaidTmp.ValMin(0) = Dir2
                    navaidTmp.ValMax(0) = Dir1
                End If

                If C > 1 Then
                    pt1 = PointAlongPlane(GuidNav.pPtPrj, Interval1.Left, fRDME)
                    pt2 = PointAlongPlane(GuidNav.pPtPrj, Interval1.Right, fRDME)

                    Dir1 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt1)
                    Dir2 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt2)

                    If AnglesSideDef(Dir1, Dir2) < 0 Then
                        navaidTmp.ValMin(1) = Dir1
                        navaidTmp.ValMax(1) = Dir2
                    Else
                        navaidTmp.ValMin(1) = Dir2
                        navaidTmp.ValMax(1) = Dir1
                    End If
                End If
                ComboBox104.Items.Add(navaidTmp)
            Next
        Else '================================================
            FullRange.Left = Max(dPrec, Point2LineDistancePrj(NextFIX.pPtPrj, NextFIX.pPtStart, NextFIX.InDir + 90.0#))
            FullRange.Right = maxLegLenght - dPrec

            For I = 0 To N
                If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                    ConRad = Math.Tan(DegToRad(VOR.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                    fOnNavRad = VOR.OnNAVRadius
                    fInterToler = VOR.IntersectingTolerance
                ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                    ConRad = Math.Tan(DegToRad(NDB.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                    fOnNavRad = NDB.OnNAVRadius
                    fInterToler = NDB.IntersectingTolerance
                ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
                    '                ConRad = 250000#
                    '                fOnNavRad = -10000#
                    '                fInterToler = LLZ.IntersectingTolerance
                    Continue For
                End If

                PrjToLocal(NextFIX.pPtPrj, NextFIX.InDir + 180.0#, NavaidList(I).pPtPrj, Xnav, Ynav)

                absY = Math.Abs(Ynav)
                If absY < fOnNavRad Then
                    If (FullRange.Left + fOnNavRad < Xnav) And (FullRange.Right - fOnNavRad > Xnav) Then
                        K = K + 1

                        OnNavList(K) = NavaidList(I) 'NavaidData2FixableNavaid(NavaidList(I))
                    End If
                    Continue For
                End If
                If ConRad > absY Then Continue For

                numer = 2.0# * absY * Math.Sin(0.5 * DegToRad(fInterToler))
                If numer > dPrec Then Continue For

                maxInter = 0.5 * PI - (ArcSin(Math.Sqrt(numer / dPrec))) - 0.5 * DegToRad(fInterToler)
                dX = absY * Math.Tan(maxInter)

                NavRange.Left = Xnav - dX
                NavRange.Right = Xnav + dX


                IntervalsIntersection(NavRange, FullRange, CurrRange)
                If CurrRange.Tag = 0 Then Continue For

                Dim navaidTmp As NavaidData = NavaidList(I)
                navaidTmp.IntersectionType = eIntersectionType.ByAngle

                ReDim navaidTmp.ValMin(0)
                ReDim navaidTmp.ValMax(0)

                pt1 = PointAlongPlane(NextFIX.pPtPrj, NextFIX.InDir + 180.0#, CurrRange.Left)
                pt2 = PointAlongPlane(NextFIX.pPtPrj, NextFIX.InDir + 180.0#, CurrRange.Right)

                Dir1 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt1)
                Dir2 = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pt2)


                If AnglesSideDef(Dir1, Dir2) < 0 Then
                    navaidTmp.ValMin(0) = Dir1
                    navaidTmp.ValMax(0) = Dir2
                Else
                    navaidTmp.ValMin(0) = Dir2
                    navaidTmp.ValMax(0) = Dir1
                End If
                ComboBox104.Items.Add(navaidTmp)
            Next I

            Dir1 = NextFIX.InDir + 180.0#
            For I = 0 To M
                PrjToLocal(NextFIX.pPtPrj, Dir1, DMEList(I).pPtPrj, Xnav, Ynav)
                absY = Math.Abs(Ynav)

                dX0 = absY / Math.Tan(DegToRad(DME.TP_div))
                dX1 = (hFix - DMEList(I).pPtPrj.Z) * Math.Tan(DegToRad(DME.SlantAngle))

                dX = Max(dX0, dX1)
                NavRange.Left = Xnav - dX
                NavRange.Right = Xnav + dX

                DMEInterval = IntervalsDifference(FullRange, NavRange)
                C = UBound(DMEInterval)
                If C < 0 Then Continue For


                Dim dmeTmp As NavaidData = DMEList(I)
                dmeTmp.IntersectionType = eIntersectionType.ByDistance

                ReDim dmeTmp.ValMin(C)
                ReDim dmeTmp.ValMax(C)

                pt1 = PointAlongPlane(NextFIX.pPtPrj, Dir1, DMEInterval(0).Left)
                pt2 = PointAlongPlane(NextFIX.pPtPrj, Dir1, DMEInterval(0).Right)

                dmeTmp.ValCnt = SideDef(DMEList(I).pPtPrj, Dir1 - 90.0#, pt1)
                If dmeTmp.ValCnt < 0 Then
                    dmeTmp.ValMin(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt1)
                    dmeTmp.ValMax(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt2)
                Else
                    dmeTmp.ValMin(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt2)
                    dmeTmp.ValMax(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt1)
                End If

                If C > 0 Then
                    pt1 = PointAlongPlane(NextFIX.pPtPrj, Dir1, DMEInterval(1).Left)
                    pt2 = PointAlongPlane(NextFIX.pPtPrj, Dir1, DMEInterval(1).Right)

                    If dmeTmp.ValCnt > 0 Then
                        dmeTmp.ValMin(1) = dmeTmp.ValMin(0)
                        dmeTmp.ValMax(1) = dmeTmp.ValMax(0)

                        dmeTmp.ValMin(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt1)
                        dmeTmp.ValMax(0) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt2)
                    Else
                        dmeTmp.ValMin(1) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt1)
                        dmeTmp.ValMax(1) = ReturnDistanceInMeters(DMEList(I).pPtPrj, pt2)
                    End If
                    dmeTmp.ValCnt = 2
                End If
                ComboBox104.Items.Add(dmeTmp)
            Next I
        End If

        For I = 0 To K

            Dim navadTmp As NavaidData = OnNavList(I)
            navadTmp.IntersectionType = eIntersectionType.OnNavaid
            ComboBox104.Items.Add(navadTmp)
        Next I

        K = 0
        For I = 0 To ComboBox104.Items.Count - 1
            If (OldNav.Index > -1) And (ComboBox104.Items(I).Index = OldNav.Index) Then
                K = I
                Exit For
            End If
        Next I
        ComboBox104.SelectedIndex = K
        Return ComboBox104.Items.Count
    End Function

    Private Sub FillComboBox107(flag As Boolean)
        ComboBox107.Items.Clear()
        CheckBox101.Enabled = False
        If (ComboBox104.SelectedItem Is Nothing) Then
            Return
        End If

        If Not flag Then
            Return
        End If

        If WPTList.Length = 0 Then
            Return
        End If

        For I As Integer = 0 To WPTList.Length - 1
            If (IsValid(WPTList(I))) Then
                ComboBox107.Items.Add(WPTList(I))
            End If
        Next I
        If (ComboBox107.Items.Count > 0) Then
            ComboBox107.SelectedIndex = 0
            CheckBox101.Enabled = True
        Else
            CheckBox101.Enabled = False
        End If
    End Sub

    Private Sub CalcMaxTurnRangeWFIX(ByRef IAFObstList As ObstacleContainer, ByRef FixRange(,) As LowHigh, ByVal PDG As Double)
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer

        Dim fAlpha As Double
        Dim fTmp As Double
        Dim C155 As Double
        Dim a1 As Double
        Dim a11 As Double
        Dim b1 As Double

        Dim High As Double
        Dim Low As Double

        Dim X9300 As Double
        Dim X4650 As Double
        Dim X0 As Double
        Dim X1 As Double
        Dim X2 As Double
        Dim Y0 As Double

        Dim A As Double
        Dim B As Double
        Dim C As Double
        Dim D As Double
        Dim DirToObs As Double
        Dim RDME As Double
        Dim rDMEObs As Double
        Dim Betta As Double
        Dim Alpha0 As Double
        Dim Alpha1 As Double
        Dim Reffectiv As Double
        Dim Reffectiv0 As Double
        Dim L As Double
        Dim L0 As Double
        Dim hFix As Double
        Dim Hi As Double

        Dim GuidNav As NavaidData

        If PDG = 0.0 Then PDG = PDGEps

        GuidNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav

        C155 = arIFHalfWidth.Value / (2 * arIASegmentMOC.Value)
        a1 = -C155 * PDG 'StepDownFIXs(StepDownsNum - 1).PDG

        hFix = fFIXHeight 'StepDownFIXs(StepDownsNum - 1).pPtPrj.Z

        If GuidNav.TypeCode = eNavaidType.DME Then
            RDME = ReturnDistanceInMeters(GuidNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
            fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
        Else
            A = a1 * a1 - 1.0
            a11 = 1.0 / a1
        End If

        N = UBound(IAFObstList.Parts)
        If N >= 0 Then
            ReDim FixRange(N, 3)
        Else
            ReDim FixRange(-1, -1)
        End If

        For I = 0 To N
            Hi = IAFObstList.Parts(I).Height
            b1 = C155 * (Hi + 2 * arIASegmentMOC.Value - hFix)

            If GuidNav.TypeCode = eNavaidType.DME Then
                rDMEObs = ReturnDistanceInMeters(GuidNav.pPtPrj, IAFObstList.Parts(I).pPtPrj)
                C = b1 * b1 - RDME * RDME - rDMEObs * rDMEObs
                DirToObs = ReturnAngleInDegrees(GuidNav.pPtPrj, IAFObstList.Parts(I).pPtPrj)

                Betta = DegToRad(fAlpha - DirToObs)
                X1 = ArcCos((rDMEObs * rDMEObs + RDME * RDME - 0.25 * arIFHalfWidth.Value * arIFHalfWidth.Value) / (2.0 * rDMEObs * RDME))
                X2 = ArcCos((rDMEObs * rDMEObs + RDME * RDME - arIFHalfWidth.Value * arIFHalfWidth.Value) / (2.0 * rDMEObs * RDME))

                '        Y0 = 0.5 * arIFHalfWidth.Value
                '        Y1 = arIFHalfWidth.Value
                If ComboBox102.SelectedIndex = 0 Then
                    Alpha0 = (Betta - X2) 'DegToRad(fAlpha) -
                    Alpha1 = (Betta - X1) 'DegToRad(fAlpha)-
                Else
                    Alpha0 = (Betta - X1) 'DegToRad(fAlpha) +
                    Alpha1 = (Betta + X2) 'DegToRad(fAlpha) +
                End If

                X4650 = Alpha1 * RDME
                X9300 = Alpha0 * RDME
                FixRange(I, 0).Tag = -1
                FixRange(I, 1).Tag = -1
                FixRange(I, 2).Tag = -1
                FixRange(I, 3).Tag = -1

                Alpha1 = 0.0
                J = 0
                K = 0
                X0 = CalcReffectiv(Alpha1, hFix, Hi, RDME, PDG, C155)
                If Reffectiv > arIFHalfWidth.Value Then Reffectiv = arIFHalfWidth.Value
                Y0 = CalcL(Alpha1, Betta, RDME, rDMEObs)
                Do
                    L0 = 0.0
                    Reffectiv0 = 0.0
                    Do
                        Alpha0 = Alpha1
                        Reffectiv = CalcReffectiv(Alpha0, hFix, Hi, RDME, PDG, C155)
                        If Reffectiv > arIFHalfWidth.Value Then Reffectiv = arIFHalfWidth.Value
                        L = CalcL(Alpha0, Betta, RDME, rDMEObs)
                        If (X0 - Y0) * (Reffectiv - L) < 0 Then
                            J = 1
                            Exit Do
                        End If

                        If Reffectiv < 0.5 * arIFHalfWidth.Value Then
                            J = 2
                            Exit Do
                        End If

                        If L < Reffectiv Then
                            If Math.Abs(L0 - L) < 100.0 Then L = L0 - 100.0
                            Alpha1 = AlphaR(L, hFix, Hi, RDME, PDG, C155)
                            L0 = L
                        Else
                            If Math.Abs(Reffectiv0 - Reffectiv) < 100.0 Then Reffectiv = Reffectiv0 - 100.0
                            Alpha1 = AlphaL(Betta, RDME, rDMEObs, Reffectiv)
                            Reffectiv0 = Reffectiv
                        End If
                        J = -CShort(Math.Abs(Alpha1 - Alpha0) < degEps)
                    Loop While J <> 1

                    If J = 1 Then
                        If X0 > Y0 Then
                            FixRange(I, K).Tag = 1
                            FixRange(I, K).Low = Alpha1 * RDME
                            FixRange(I, K).High = BigDist
                        Else
                            FixRange(I, K).Tag = 1
                            FixRange(I, K).Low = -BigDist
                            FixRange(I, K).High = Alpha1 * RDME
                        End If
                    Else
                        If X0 > Y0 Then
                            FixRange(I, K).Tag = -1
                        Else
                            FixRange(I, K).Tag = 1
                            FixRange(I, K).Low = -BigDist
                            FixRange(I, K).High = BigDist
                        End If
                    End If

                    K = K + 1
                    If J = 2 Then Exit Do
                    Alpha1 = Alpha1 + 120.0 / RDME
                Loop While K < 2

                If J = 2 Then
                    If L > Reffectiv Then
                    Else
                    End If
                End If

                FixRange(I, K).Tag = 1
                FixRange(I, K).Low = -BigDist
                FixRange(I, K).High = X9300

                K = K + 1
                FixRange(I, K).Tag = 1
                FixRange(I, K).Low = X4650
                FixRange(I, K).High = BigDist
            Else 'VOR/NDB
                X0 = -b1 * a11
                X4650 = -(b1 - 0.5 * arIFHalfWidth.Value) * a11
                X9300 = IAFObstList.Parts(I).Dist - Math.Sqrt(arIFHalfWidth.Value * arIFHalfWidth.Value - IAFObstList.Parts(I).CLDist * IAFObstList.Parts(I).CLDist)

                FixRange(I, 1).Tag = -1

                B = (a1 * b1 + IAFObstList.Parts(I).Dist)
                C = b1 * b1 - IAFObstList.Parts(I).Dist * IAFObstList.Parts(I).Dist - IAFObstList.Parts(I).CLDist * IAFObstList.Parts(I).CLDist
                D = B * B - A * C
                If D >= 0.0 Then
                    D = Math.Sqrt(D)
                    X1 = (-B + D) / A
                    X2 = (-B - D) / A
                    If X1 > X2 Then
                        fTmp = X1
                        X1 = X2
                        X2 = fTmp
                    End If

                    If X2 >= IAFObstList.Parts(I).Dist Then X2 = IAFObstList.Parts(I).Rmax + 5.0

                    FixRange(I, 1).Low = X1
                    FixRange(I, 1).High = X2
                    FixRange(I, 1).Tag = 1
                    'End If
                    '=======================================================================================
                    'If FixRange(I, 1).Ix = 1 Then
                    If X1 > X0 Then
                        FixRange(I, 0).Tag = 1
                        FixRange(I, 0).Low = -BigDist
                        FixRange(I, 0).High = BigDist

                        FixRange(I, 1).Tag = -1
                        FixRange(I, 2).Tag = -1
                        FixRange(I, 3).Tag = -1
                    ElseIf X2 < X0 Then
                        FixRange(I, 0).Tag = 1
                        FixRange(I, 0).Low = -BigDist
                        FixRange(I, 0).High = X1

                        FixRange(I, 1).Tag = 1
                        FixRange(I, 1).Low = X2
                        FixRange(I, 1).High = BigDist

                        FixRange(I, 2).Tag = 1
                        FixRange(I, 2).Low = -BigDist
                        FixRange(I, 2).High = X9300

                        FixRange(I, 3).Tag = 1
                        FixRange(I, 3).Low = X4650
                        FixRange(I, 3).High = BigDist
                    Else 'If (X1 <= X0) And (X2 >= X0) Then
                        FixRange(I, 0).Tag = 1
                        FixRange(I, 0).Low = X1
                        FixRange(I, 0).High = BigDist

                        FixRange(I, 1).Tag = 1
                        FixRange(I, 1).Low = -BigDist
                        FixRange(I, 1).High = X9300

                        FixRange(I, 2).Tag = 1
                        FixRange(I, 2).Low = X4650
                        FixRange(I, 2).High = BigDist

                        FixRange(I, 3).Tag = -1
                    End If
                Else
                    FixRange(I, 0).Tag = 1
                    FixRange(I, 0).Low = -BigDist
                    FixRange(I, 0).High = BigDist

                    FixRange(I, 1).Tag = -1
                    FixRange(I, 2).Tag = -1
                    FixRange(I, 3).Tag = -1
                End If
            End If
            '=======================================================================================
            'Squaze
            Low = 0.0
            High = IAFObstList.Parts(I).Rmax
            '    If pRelation.Contains(IAFObstList(I).pPtPrj) Then
            '        High = IAFObstList(I).Dist - dPrec
            '    Else
            '        High = IAFObstList(I).Dist - 5.0
            '    End If

            For J = 0 To 2
                If FixRange(I, J).High <= Low Then FixRange(I, J).Tag = -1
                If FixRange(I, J).Low >= High Then FixRange(I, J).Tag = -1

                If FixRange(I, J).Low < Low Then FixRange(I, J).Low = Low
                If FixRange(I, J).High > High Then FixRange(I, J).High = High

                If FixRange(I, J).Tag > 0 Then
                    For K = J + 1 To 3
                        If FixRange(I, K).High <= Low Then FixRange(I, K).Tag = -1
                        If FixRange(I, K).Low >= High Then FixRange(I, K).Tag = -1

                        If FixRange(I, K).Low < Low Then FixRange(I, K).Low = Low
                        If FixRange(I, K).High > High Then FixRange(I, K).High = High

                        If FixRange(I, K).Tag > 0 Then
                            If FixRange(I, J).Low <= FixRange(I, K).Low Then
                                If FixRange(I, J).High >= FixRange(I, K).High Then
                                    FixRange(I, K).Tag = -1
                                ElseIf FixRange(I, J).High >= FixRange(I, K).Low Then
                                    FixRange(I, J).High = FixRange(I, K).High
                                    FixRange(I, K).Tag = -1
                                End If
                            ElseIf FixRange(I, J).High >= FixRange(I, K).High Then
                                If FixRange(I, J).Low <= FixRange(I, K).Low Then
                                    FixRange(I, K).Tag = -1
                                ElseIf FixRange(I, J).Low <= FixRange(I, K).High Then
                                    FixRange(I, J).Low = FixRange(I, K).Low
                                    FixRange(I, K).Tag = -1
                                End If
                            Else
                                FixRange(I, J).High = FixRange(I, K).High
                                FixRange(I, J).Low = FixRange(I, K).Low
                                FixRange(I, K).Tag = -1
                            End If
                        End If
                    Next K
                End If
            Next J
            '=======================================================================================
            K = 0
            For J = 1 To 3
                If FixRange(I, K).Tag < 0 Then
                    FixRange(I, K) = FixRange(I, J)
                    FixRange(I, J).Tag = -1
                Else
                    K = K + 1
                End If
            Next J
            '=======================================================================================
        Next I
    End Sub

    Private Sub CreateIAF_AreaPoly(ByRef GuidNav As NavaidData, ByRef ForFIX As StepDownFIX, ByRef pIAF_IAreaPoly As IPointCollection, ByRef pIAF_IIAreaPoly As IPointCollection, ByRef pNomPoly As IPolyline)
        Dim bNavCode As Boolean
        Dim TurnDir As Integer
        Dim TurnFlg As Integer
        Dim NavSide As Integer

        Dim L As Double
        Dim fDist As Double
        Dim fIADir As Double
        Dim fNavDist As Double
        Dim fNextDir As Double
        Dim fOnNavRad As Double
        'Dim fMaxTurnAngle As Double


        Dim pPtTo As IPoint
        Dim pPtTmp As IPoint
        Dim pClone As IClone

        Dim pCirc As IPolygon
        Dim pCutter As IPointCollection
        Dim pTmpPoly As IPointCollection
        Dim pTopo As ITopologicalOperator2

        pCutter = New Polyline

        'If ComboBox101.ListIndex < 0 Then Exit Sub

        '==========================================================================================================
        L = ReturnDistanceInMeters(ForFIX.pPtPrj, GuidNav.pPtPrj)
        fNextDir = ReturnAngleInDegrees(ForFIX.pPtPrj, GuidNav.pPtPrj)
        '===============
        If GuidNav.TypeCode = eNavaidType.DME Then 'DME
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + arAreaHalfWidth)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - arAreaHalfWidth)
            pTopo = pCirc

            pTmpPoly = pTopo.Difference(pTmpPoly)

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()

            'DrawPolygon pIAF_IIAreaPoly, 255
            'Set pIAF_IIAreaPoly = pTmpPoly
            'Set pNomPoly = CreatePolylineCircle(GuidNav.pPtPrj, L)

            TurnFlg = 2 * ComboBox105.SelectedIndex - 1 '???????????????????????

            If ForFIX.TurnDir = 0 Then
                pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir, L)
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0#, 2 * (L + arAreaHalfWidth)))
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir, 2 * (L + arAreaHalfWidth)))
            Else
                fIADir = 3.0# * RadToDeg(ArcSin(0.5 * arAreaHalfWidth / L))
                If fIADir < 90.0# Then fIADir = 90.0#

                pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0# - ForFIX.TurnDir * TurnFlg * fIADir, L)
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0#, 2 * (L + arAreaHalfWidth)))
                pCutter.AddPoint(GuidNav.pPtPrj)
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0# - ForFIX.TurnDir * TurnFlg * fIADir, 2 * (L + arAreaHalfWidth)))
            End If

            'DrawPointWithText pPtTo, "pPtTo"
            'pCutter.RemovePoints 0, pCutter.PointCount
            'DrawPointWithText PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180#, 2 * (L + arAreaHalfWidth)), "2*L"


            If ForFIX.TurnDir = 0 Then
                If Modulus(fNextDir - ForFIX.OutDir, 360.0#) > 180.0# Then

                    pTopo.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
                    pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, TurnFlg)
                Else

                    pTopo.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
                    pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, -TurnFlg)
                End If
            Else
                If ForFIX.TurnDir * TurnFlg > 0 Then

                    pTopo.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
                Else

                    pTopo.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
                End If

                pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, ForFIX.TurnDir * TurnFlg)
            End If

            'DrawPolygon pIAF_IIAreaPoly, 0
            pCutter.RemovePoints(0, pCutter.PointCount)
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0#, 2 * arAreaHalfWidth))
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0#, 2 * arAreaHalfWidth))
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arAreaHalfWidth)
            'DrawPolygon pCirc, 0




            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)

            pTopo = pIAF_IIAreaPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()

            pIAF_IIAreaPoly = pTopo.Union(pTmpPoly)
            'DrawPolygon pIAF_IIAreaPoly, 0
            '===============
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + 0.5 * arAreaHalfWidth)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - 0.5 * arAreaHalfWidth)
            'DrawPolygon pTmpPoly, 0

            pTopo = pCirc

            pTmpPoly = pTopo.Difference(pTmpPoly)

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()

            'Set pIAF_IAreaPoly = pTmpPoly
            'DrawPolygon pTmpPoly, RGB(0, 255, 0)
            pCutter.RemovePoints(0, pCutter.PointCount)

            If ForFIX.TurnDir = 0 Then
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0#, 2 * (L + arAreaHalfWidth)))
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir, 2 * (L + arAreaHalfWidth)))
            Else
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0#, 2 * (L + arAreaHalfWidth)))
                pCutter.AddPoint(GuidNav.pPtPrj)
                pCutter.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0# - ForFIX.TurnDir * TurnFlg * fIADir, 2 * (L + arAreaHalfWidth)))
            End If

            If ForFIX.TurnDir = 0 Then
                If Modulus(fNextDir - ForFIX.OutDir, 360.0#) > 180.0# Then

                    pTopo.Cut(pCutter, pCirc, pIAF_IAreaPoly)
                Else

                    pTopo.Cut(pCutter, pIAF_IAreaPoly, pCirc)
                End If
            ElseIf ForFIX.TurnDir * TurnFlg > 0 Then

                pTopo.Cut(pCutter, pCirc, pIAF_IAreaPoly)
            Else

                pTopo.Cut(pCutter, pIAF_IAreaPoly, pCirc)
            End If

            pCutter.RemovePoints(0, pCutter.PointCount)
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0#, 2 * arAreaHalfWidth))
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0#, 2 * arAreaHalfWidth))
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arAreaHalfWidth)




            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopo = pIAF_IAreaPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()

            pIAF_IAreaPoly = pTopo.Union(pTmpPoly)
            '==========================================================================================================
        Else 'VOR NDB
            '==========================================================================================================
            fOnNavRad = -10000.0#

            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                fNavDist = arAreaHalfWidth / Math.Tan(DegToRad(VOR.SplayAngle))
                fOnNavRad = VOR.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                fNavDist = arAreaHalfWidth / Math.Tan(DegToRad(NDB.SplayAngle))
                fOnNavRad = NDB.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                fNavDist = LLZ.Range
            End If

            NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90.0#, GuidNav.pPtPrj)
            TurnDir = ForFIX.TurnDir

            If ForFIX.TurnDir = 0 Then
                '==========================================================================================================
                bNavCode = StepDownsNum >= 1
                If bNavCode Then bNavCode = ForFIX.GuidanceNav.TypeCode = eNavaidType.DME

                '            If Not bNavCode Then
                '                fIADir = ForFIX.InDir
                '            Else
                '                If SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90#, GuidNav.pPtPrj) > 0 Then
                '                    If TurnDir = NavSide Then
                '                        fIADir = fNextDir
                '                    Else
                '                        fIADir = fNextDir + 180#
                '                    End If
                '                Else
                '                    If TurnDir = NavSide Then
                '                        fIADir = fNextDir
                '                    Else
                '                        fIADir = fNextDir + 180#
                '                    End If
                '                End If
                '            End If

                fIADir = ForFIX.InDir

                If ForFIX.PDG < PDGEps Then
                    L = 100000.0#
                Else
                    L = Max(L + fNavDist, MOCLimit / ForFIX.PDG)
                End If
                '==========================================================================================================
            Else
                '           fMaxTurnAngle = Abs(CDbl(Right(ComboBox103.Text, 3)))
                '           If SubtractAngles(fNextDir, ForFIX.OutDir) > fMaxTurnAngle Then
                '               fIADir = fNextDir + 180#
                '               L = Max(L - fNavDist, (CurrADHP.MinTMA - ForFIX.pPtPrj.Z) / ForFIX.PDG)
                '           Else
                '               fIADir = fNextDir
                '               L = Max(L + fNavDist, (CurrADHP.MinTMA - ForFIX.pPtPrj.Z) / ForFIX.PDG)
                '           End If

                '           TurnDir = ForFIX.TurnDir
                '           NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)

                fDist = L 'ReturnDistanceInMeters(GuidNav.pPtPrj, ForFIX.pPtPrj)

                If L < fOnNavRad Then
                    pClone = StepDownFIXs(StepDownsNum).pPtPrj
                    pPtTmp = pClone.Clone

                    pPtTmp.SpatialReference = pSpRefPrj

                    pPtTmp.Project(pSpRefGeo)
                    'fIADir = Azt2Dir(pPtTmp, CDbl(TextBox109.Text) + GuidNav.MagVar)
                    fIADir = Azt2Dir(pPtTmp, turnMagBearing + GuidNav.MagVar)

                    L = fNavDist
                Else
                    '    DrawPointWithText ForFIX.pPtPrj, "ForFIX"
                    '    DrawPointWithText GuidNav.pPtPrj, "GuidNav"

                    If NavSide < 0 Then
                        If TurnDir = NavSide Then
                            If ForFIX.PDG > PDGEps Then L = Max(L + fNavDist, MOCLimit / ForFIX.PDG)
                        Else
                            If ForFIX.PDG > PDGEps Then L = Max(L - fNavDist, MOCLimit / ForFIX.PDG)
                        End If
                    Else
                        If TurnDir = NavSide Then
                            If ForFIX.PDG > PDGEps Then L = Max(L - fNavDist, MOCLimit / ForFIX.PDG)
                        Else
                            If ForFIX.PDG > PDGEps Then L = Max(L + fNavDist, MOCLimit / ForFIX.PDG)
                        End If
                    End If

                    fIADir = ForFIX.InDir
                    If ForFIX.PDG < PDGEps Then L = 100000.0#
                    If L < 10000.0# Then L = 10000.0#
                End If
            End If

            If L < IntDist Then L = IntDist ' ;(

            pNomPoly = New Polyline


            pNomPoly.FromPoint = ForFIX.pPtStart


            pNomPoly.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fIADir + 180.0#, L)

            'DrawPolyLine pNomPoly, , 2

            pIAF_IIAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            'DrawPoint ForFIX.pPtPrj, RGB(0, 255, 0)
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0#, arAreaHalfWidth))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0#, arAreaHalfWidth))

            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(1), fIADir + 180.0#, L))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(0), fIADir + 180.0#, L))
            pTopo = pIAF_IIAreaPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()
            'DrawPolygon pIAF_IIAreaPoly, 255
            If pCutter.PointCount > 0 Then pCutter.RemovePoints(0, pCutter.PointCount)
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0#, 2 * arAreaHalfWidth))
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0#, 2 * arAreaHalfWidth))

            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arAreaHalfWidth)
            'DrawPolygon pCirc, 255
            'DrawPolyLine pCutter, 0, 2




            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            'DrawPolygon pTmpPoly, 0
            pTopo = pIAF_IIAreaPoly

            pIAF_IIAreaPoly = pTopo.Union(pTmpPoly)

            'DrawPolygon pIAF_IIAreaPoly, 255

            pIAF_IAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0#, 0.5 * arAreaHalfWidth))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0#, 0.5 * arAreaHalfWidth))

            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(1), fIADir + 180.0#, L))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(0), fIADir + 180.0#, L))
            pTopo = pIAF_IAreaPoly
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()

            pCutter.RemovePoints(0, pCutter.PointCount)
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0#, arAreaHalfWidth))
            pCutter.AddPoint(PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0#, arAreaHalfWidth))
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arAreaHalfWidth)




            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopo = pIAF_IAreaPoly

            pIAF_IAreaPoly = pTopo.Union(pTmpPoly)
        End If
    End Sub

    Private Function EstimateIAF_AreaObstacles() As Integer

        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer
        Dim TurnFlg As Integer

        Dim L As Double
        Dim dD As Double
        Dim fL As Double
        Dim fTmp As Double
        Dim dPrec As Double
        Dim fTmp1 As Double
        Dim dMax15 As Double
        Dim fIADir As Double
        Dim fAlpha As Double
        Dim fNextSegPDG As Double

        Dim GuidNav As NavaidData
        Dim LastFIX As StepDownFIX

        Dim IFNav As NavaidData

        Dim ptTmp As IPoint
        Dim ptIFTmp As IPoint
        Dim pConstructor As IConstructPoint
        Dim pRelation As IRelationalOperator

        ShowPandaBox(Me.Handle.ToInt32)

        If IsNumeric(TextBox105.Text) Then
            dPrec = DeConvertDistance(CDbl(TextBox105.Text))
        Else
            dPrec = arIFTolerance.Value
        End If

        On Error Resume Next
        '    If Not StepDownFIXs(StepDownsNum).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(StepDownsNum).pIAreaPolyElem
        '    If Not StepDownFIXs(StepDownsNum).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(StepDownsNum).pIIAreaPolyElem

        N = UBound(IAFProhibSectors)
        For I = 0 To N
            If Not IAFProhibSectors(I).ObsAreaElement Is Nothing Then pGraphics.DeleteElement(IAFProhibSectors(I).ObsAreaElement)
        Next I
        On Error GoTo 0


        LastFIX = StepDownFIXs(StepDownsNum - 1)

        GuidNav = LastFIX.GuidanceNav
        IFNav = LastFIX.IntersectNav
        fIADir = LastFIX.InDir

        fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastFIX.pPtPrj)
        fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastFIX.pPtPrj)

        pGuidPoly = CreateGuidPoly(GuidNav, LastFIX.pPtPrj)

        '====================================================================================
        'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, 0
        'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIAreaPoly, 255

        N = CalcIFProhibitions(LastFIX, fRefHeight, arAreaHalfWidth, MOCLimit, StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, StepDownFIXs(StepDownsNum - 1).pIAreaPoly, IAFObstList4FIX, IAFProhibSectors)
        'N = -1
        EstimateIAF_AreaObstacles = N
        fNextSegPDG = IIf(IAF_PDG <= 0, arIADescent_Max.Value, IAF_PDG)

        If N >= 0 Then
            If N > 0 Then Sort(IAFObstList4FIX, 0)

            If GuidNav.TypeCode = eNavaidType.DME Then
                fTmp = ReturnAngleInDegrees(LastFIX.pPtPrj, GuidNav.pPtPrj)
                fTmp = Modulus(LastFIX.InDir - fTmp + 90, 360.0#)
                If fTmp > 180.0# Then fTmp = 360.0# - fTmp
                TurnFlg = -CShort(fTmp < 90.0#)
            End If

            pRelation = pGuidPoly


            IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsAreaElement = DrawPolygon(IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsArea, RGB(254, 0, 254))
            IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsAreaElement.Locked = True
            '======================================15%====================================================
            IAFObstList4FIX.Parts(0).Rmin = IAFObstList4FIX.Parts(0).hPenet / fNextSegPDG 'arIADescent_Max.Value
            dMax15 = IAFObstList4FIX.Parts(0).Dist - IAFObstList4FIX.Parts(0).Rmin
            IAFObstList4FIX.Parts(0).dMin15 = (arFIX15PlaneRang.Value * (fNextSegPDG - arFixMaxIgnorGrd.Value) - IAFObstList4FIX.Parts(0).hPenet + fNextSegPDG * IAFObstList4FIX.Parts(0).Dist) / fNextSegPDG

            dD = Max(Min(dMax15, dPrec), IAFObstList4FIX.Parts(0).dMin15)
            IAFObstList4FIX.Parts(0).Rmax = (IAFObstList4FIX.Parts(0).hPenet + arFixMaxIgnorGrd.Value * (dD - IAFObstList4FIX.Parts(0).Dist)) / (fNextSegPDG - arFixMaxIgnorGrd.Value)
            IAFObstList4FIX.Parts(0).dMin15 = dD

            IAFObstList4FIX.Parts(0).Flags = IAFObstList4FIX.Parts(0).Flags And Not 2

            If Not pRelation.Contains(IAFObstList4FIX.Parts(0).pPtPrj) Then
                If IAFObstList4FIX.Parts(0).Rmax < IAFObstList4FIX.Parts(0).Dist - 5.0# Then
                    IAFObstList4FIX.Parts(0).Rmax = IAFObstList4FIX.Parts(0).Dist - 5.0#
                End If
                If IAFObstList4FIX.Parts(0).Rmax > IAFObstList4FIX.Parts(0).Dist Then
                    IAFObstList4FIX.Parts(0).Flags = IAFObstList4FIX.Parts(0).Flags Or 2
                End If
            End If
            '==========================================================================================
            If IAFObstList4FIX.Parts(0).Rmin > IAFObstList4FIX.Parts(0).Rmax Then
                If IAFObstList4FIX.Parts(0).Rmax < dPrec Then
                    IAFObstList4FIX.Parts(0).DistStar = IAFObstList4FIX.Parts(0).hPenet
                Else
                    IAFObstList4FIX.Parts(0).fTmp = IAFObstList4FIX.Parts(0).hPenet / IAFObstList4FIX.Parts(0).Rmax

                    If IAFObstList4FIX.Parts(0).fTmp > fNextSegPDG Then
                        IAFObstList4FIX.Parts(0).DistStar = IAFObstList4FIX.Parts(0).hPenet - fNextSegPDG * IAFObstList4FIX.Parts(0).Rmax
                    Else
                        IAFObstList4FIX.Parts(0).DistStar = 0.0#
                    End If
                End If
            Else
                If IAFObstList4FIX.Parts(0).Rmax < dPrec Then
                    IAFObstList4FIX.Parts(0).DistStar = IAFObstList4FIX.Parts(0).hPenet
                Else
                    IAFObstList4FIX.Parts(0).DistStar = 0.0#
                End If
            End If

            If GuidNav.TypeCode <> eNavaidType.DME Then
                If CircleVectorIntersect(IAFObstList4FIX.Parts(0).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).rObs, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir, ptTmp) >= 0.0# Then
                    IAFObstList4FIX.Parts(0).TurnDistL = Point2LineDistancePrj(ptTmp, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 90.0#) * SideDef(ptTmp, fIADir + 90.0#, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                    IAFObstList4FIX.Parts(0).TurnDistR = IAFObstList4FIX.Parts(0).TurnDistL

                    IAFObstList4FIX.Parts(0).TurnAngleL = SubtractAngles(ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(0).pPtPrj) + 90.0#, fIADir + 180.0#)
                    IAFObstList4FIX.Parts(0).TurnAngleR = 180.0# - IAFObstList4FIX.Parts(0).TurnAngleL
                Else
                    IAFObstList4FIX.Parts(0).TurnAngleL = 500.0#
                    IAFObstList4FIX.Parts(0).TurnAngleR = 500.0#
                    IAFObstList4FIX.Parts(0).TurnDistL = -1.0#
                    IAFObstList4FIX.Parts(0).TurnDistR = -1.0#
                End If
            Else
                If CircleCircleIntersect(GuidNav.pPtPrj, fL, IAFObstList4FIX.Parts(0).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).rObs, 1 - 2 * TurnFlg, ptTmp) > -1.0# Then
                    IAFObstList4FIX.Parts(0).TurnDistL = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ptTmp))) * fL
                    IAFObstList4FIX.Parts(0).TurnDistR = IAFObstList4FIX.Parts(0).TurnDistL

                    fTmp = ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(0).pPtPrj)
                    fTmp1 = ReturnAngleInDegrees(ptTmp, GuidNav.pPtPrj)

                    IAFObstList4FIX.Parts(0).TurnAngleL = SubtractAngles(fTmp + 90.0#, fTmp1 + 90.0# * (2 * TurnFlg - 1))
                    IAFObstList4FIX.Parts(0).TurnAngleR = 180.0# - IAFObstList4FIX.Parts(0).TurnAngleL
                Else
                    IAFObstList4FIX.Parts(0).TurnAngleL = 500.0#
                    IAFObstList4FIX.Parts(0).TurnAngleR = 500.0#
                    IAFObstList4FIX.Parts(0).TurnDistL = -1.0#
                    IAFObstList4FIX.Parts(0).TurnDistR = -1.0#
                End If
            End If
            '==========================================================================================
            I = 0

            If IAFObstList4FIX.Parts(0).fTmp <= fNextSegPDG Then
                For J = 1 To N
                    '                If IAFObstList4FIX.Parts(J).Height + IAFObstList4FIX.Parts(J).MOC > IAFObstList4FIX.Parts(i).ReqH Then
                    If IAFObstList4FIX.Parts(J).ReqH > IAFObstList4FIX.Parts(I).ReqH Then
                        I = I + 1

                        IAFObstList4FIX.Parts(I) = IAFObstList4FIX.Parts(J)

                        IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsAreaElement = DrawPolygon(IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsArea, RGB(254, 0, 254))
                        IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsAreaElement.Locked = True
                        '============================================15%==============================================
                        IAFObstList4FIX.Parts(I).Rmin = IAFObstList4FIX.Parts(I).hPenet / fNextSegPDG 'arIADescent_Max.Value
                        dMax15 = IAFObstList4FIX.Parts(I).Dist - IAFObstList4FIX.Parts(I).Rmin
                        IAFObstList4FIX.Parts(I).dMin15 = (arFIX15PlaneRang.Value * (fNextSegPDG - arFixMaxIgnorGrd.Value) - IAFObstList4FIX.Parts(I).hPenet + fNextSegPDG * IAFObstList4FIX.Parts(I).Dist) / fNextSegPDG

                        dD = Max(Min(dMax15, dPrec), IAFObstList4FIX.Parts(I).dMin15)
                        IAFObstList4FIX.Parts(I).Rmax = (IAFObstList4FIX.Parts(I).hPenet + arFixMaxIgnorGrd.Value * (dD - IAFObstList4FIX.Parts(I).Dist)) / (fNextSegPDG - arFixMaxIgnorGrd.Value)
                        IAFObstList4FIX.Parts(I).dMin15 = dD

                        IAFObstList4FIX.Parts(I).Flags = IAFObstList4FIX.Parts(I).Flags And Not 2

                        If Not pRelation.Contains(IAFObstList4FIX.Parts(I).pPtPrj) Then
                            If IAFObstList4FIX.Parts(I).Rmax < IAFObstList4FIX.Parts(I).Dist - 5.0# Then
                                IAFObstList4FIX.Parts(I).Rmax = IAFObstList4FIX.Parts(I).Dist - 5.0#
                            End If
                            If IAFObstList4FIX.Parts(I).Rmax > IAFObstList4FIX.Parts(I).Dist Then
                                IAFObstList4FIX.Parts(I).Flags = IAFObstList4FIX.Parts(I).Flags Or 2
                            End If
                        End If
                        '=============================================15%=============================================
                        If IAFObstList4FIX.Parts(I).Rmin > IAFObstList4FIX.Parts(I).Rmax Then
                            If IAFObstList4FIX.Parts(I).Rmax < dPrec Then
                                IAFObstList4FIX.Parts(I).DistStar = IAFObstList4FIX.Parts(I).hPenet
                            Else
                                '                            If IAFObstList4FIX.Parts(i).fTmp > arIADescent_Max.Value Then
                                If IAFObstList4FIX.Parts(I).fTmp > fNextSegPDG Then
                                    IAFObstList4FIX.Parts(I).DistStar = IAFObstList4FIX.Parts(I).hPenet - fNextSegPDG * IAFObstList4FIX.Parts(I).Rmax
                                Else
                                    IAFObstList4FIX.Parts(I).DistStar = 0.0#
                                End If
                            End If
                        Else
                            If IAFObstList4FIX.Parts(I).Rmax < dPrec Then
                                IAFObstList4FIX.Parts(I).DistStar = IAFObstList4FIX.Parts(I).hPenet
                            Else
                                IAFObstList4FIX.Parts(I).DistStar = 0.0#
                            End If
                        End If

                        If GuidNav.TypeCode <> eNavaidType.DME Then
                            If CircleVectorIntersect(IAFObstList4FIX.Parts(I).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).rObs, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir, ptTmp) >= 0.0# Then
                                IAFObstList4FIX.Parts(I).TurnDistL = ReturnDistanceInMeters(ptTmp, StepDownFIXs(StepDownsNum - 1).pPtPrj) * SideDef(ptTmp, fIADir + 90.0#, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                                IAFObstList4FIX.Parts(I).TurnDistR = IAFObstList4FIX.Parts(I).TurnDistL
                                IAFObstList4FIX.Parts(I).TurnAngleL = SubtractAngles(ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(I).pPtPrj) + 90.0#, fIADir + 180.0#)
                                IAFObstList4FIX.Parts(I).TurnAngleR = 180.0# - IAFObstList4FIX.Parts(I).TurnAngleL
                            Else
                                IAFObstList4FIX.Parts(I).TurnAngleL = 500.0#
                                IAFObstList4FIX.Parts(I).TurnAngleR = 500.0#
                                IAFObstList4FIX.Parts(I).TurnDistL = -1.0#
                                IAFObstList4FIX.Parts(I).TurnDistR = -1.0#
                            End If
                        Else
                            If CircleCircleIntersect(GuidNav.pPtPrj, fL, IAFObstList4FIX.Parts(I).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).rObs, 1 - 2 * TurnFlg, ptTmp) > -1.0# Then
                                IAFObstList4FIX.Parts(I).TurnDistL = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ptTmp))) * fL
                                IAFObstList4FIX.Parts(I).TurnDistR = IAFObstList4FIX.Parts(I).TurnDistL

                                fTmp = ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(I).pPtPrj)
                                fTmp1 = ReturnAngleInDegrees(ptTmp, GuidNav.pPtPrj)

                                IAFObstList4FIX.Parts(I).TurnAngleL = SubtractAngles(fTmp + 90.0#, fTmp1 + 90.0# * (2 * TurnFlg - 1))
                                IAFObstList4FIX.Parts(I).TurnAngleR = 180.0# - IAFObstList4FIX.Parts(I).TurnAngleL
                            Else
                                IAFObstList4FIX.Parts(I).TurnAngleL = 500.0#
                                IAFObstList4FIX.Parts(I).TurnAngleR = 500.0#
                                IAFObstList4FIX.Parts(I).TurnDistL = -1.0#
                                IAFObstList4FIX.Parts(I).TurnDistR = -1.0#
                            End If
                        End If
                        '                   If IAFObstList4FIX.Parts(I).fTmp > arIADescent_Max.Value Then Exit For
                        '                   If IAFObstList4FIX.Parts(I).fTmp > fNextSegPDG Then Exit For
                    End If
                Next J
            End If

            If N > I Then
                N = I
                ReDim Preserve IAFObstList4FIX.Parts(I)
            End If

            'InitialReportsFrm.FillPage1 IAFObstList4FIX
            M = UBound(IAFObstList4FIX.Obstacles)

            ReDim IAFObstList4Turn.Parts(N)
            ReDim IAFObstList4Turn.Obstacles(M)
            System.Array.Copy(IAFObstList4FIX.Parts, IAFObstList4Turn.Parts, N + 1)
            System.Array.Copy(IAFObstList4FIX.Obstacles, IAFObstList4Turn.Obstacles, M + 1)

            Sort(IAFObstList4Turn, 1)

        Else

            ReDim IAFObstList4FIX.Obstacles(-1)
            ReDim IAFObstList4FIX.Parts(-1)
            ReDim IAFObstList4Turn.Obstacles(-1)
            ReDim IAFObstList4Turn.Parts(-1)
        End If

        HidePandaBox()

        InitialReportsFrm.FillPage2(IAFObstList4FIX)
    End Function

    Private Sub ComboBox002_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox002.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim M As Integer
        Dim N As Integer

        Dim pProcedureLeg As SegmentLeg

        Dim pIAP As InstrumentApproachProcedure
        Dim pProcedureTransition As ProcedureTransition

        ComboBox003.Items.Clear()

        K = ComboBox002.SelectedIndex
        If K < 0 Then Return

        pIAP = ComboBox002.SelectedItem

        pLandingTakeoff = pIAP.Landing
        'pLandingTakeoff = pIAP.flightTransition(0).departureRunwayTransition
        'Set pAircraftCharacteristic = pIAP.AircraftCharacteristic

        If pIAP.AircraftCharacteristic.Count > 0 Then
            Category = pIAP.AircraftCharacteristic.Item(0).AircraftLandingCategory.Value
            TextBox007.Text = pIAP.AircraftCharacteristic.Item(0).AircraftLandingCategory.ToString()
        End If
        '======================================================================================

        TransArray = New List(Of IAPArrayItem)

        'Set pProcedureTransitionList = pObjectDir.GetProcedureTranstionList(pIAP.identifier)
        N = pIAP.FlightTransition.Count

        For I = 0 To N - 1
            pProcedureTransition = pIAP.FlightTransition.Item(I)
            'Set pProcedureLegList = pObjectDir.GetSegmentLegListByTransition(CStr(pProcedureTransition.ID))
            M = pProcedureTransition.TransitionLeg.Count
            If M > 0 Then
                Dim bflag As Boolean = Not pProcedureTransition.Type Is Nothing
                If bflag Then bflag = pProcedureTransition.Type.HasValue
				If bflag Then bflag = pProcedureTransition.Type.Value = CodeProcedurePhase.APPROACH

				If bflag Then
					Dim Transition As IAPArrayItem

					If pProcedureTransition.TransitionLeg(0).TheSegmentLeg Is Nothing Then Continue For

					Transition.LegIndex = -1
                    Transition.pIAP = pIAP
                    Transition.pProcedureTransition = pProcedureTransition
                    TransArray.Add(Transition)
                    ComboBox003.Items.Add("Transition " + (I + 1).ToString())
                Else
                    For J = 0 To M - 1
						If pProcedureTransition.TransitionLeg(J).TheSegmentLeg Is Nothing Then Continue For

						pProcedureLeg = pProcedureTransition.TransitionLeg(J).TheSegmentLeg.GetFeature2()

						If pProcedureLeg Is Nothing Then Continue For
						If pProcedureLeg.EndPoint Is Nothing Then Continue For
						If pProcedureLeg.EndPoint.Role Is Nothing Then Continue For
						If pProcedureLeg.StartPoint Is Nothing Then Continue For
						'If (pProcedureLeg.CodeRoleFix = "FAF") Or (pProcedureLeg.CodeRoleFix = "IF") Then
						If pProcedureLeg.EndPoint.Role.Value = CodeProcedureFixRole.FAF Then
							Dim Transition As IAPArrayItem
							Transition.LegIndex = J
                            Transition.pIAP = pIAP
                            Transition.pProcedureTransition = pProcedureTransition
                            TransArray.Add(Transition)
                            ComboBox003.Items.Add("Transition " + (I + 1).ToString())
                            Exit For
                        End If
                    Next J
                End If
            End If
        Next I

        If ComboBox003.Items.Count > 0 Then
            ComboBox003.SelectedIndex = 0
        End If

    End Sub

    Private Sub TextBox001_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox001.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox001_Validating(TextBox001, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox001.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

	Private Sub TextBox001_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox001.Validating
		If TextBox001.ReadOnly Then Return
		If (StartPoint.TypeCode <> eNavaidType.VOR) And (StartPoint.TypeCode <> eNavaidType.NDB) Then Return

		If Not IsNumeric(TextBox001.Text) Then
			TextBox001.Text = CStr(StartPoint.pPtGeo.M)
			Return
		End If

		Dim Course As Double
		Course = CDbl(TextBox001.Text)

		If (Course >= 360.0) Or (Course <= -360.0) Then
			Course = 0.0
			TextBox001.Text = CStr(Course)
		End If

		'SafeDeleteElement(pFAFptElement)
		'SafeDeleteElement(pFAFAreaElement)

		StartPoint.pPtGeo.M = Course
		StartPoint.pPtPrj.M = Azt2Dir(StartPoint.pPtGeo, Course)

		pIFptPrj.M = StartPoint.pPtPrj.M
		pFAFptPrj.M = StartPoint.pPtPrj.M
	End Sub

	Private Sub ComboBox003_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox003.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim Course As Double
        Dim Altitude As Double
        Dim bFound As Boolean
        Dim bOnNav As Boolean
        Dim ILS As NavaidData
        Dim Identifier As Guid
        Dim pProcedureLeg As SegmentLeg
        Dim pProcedureLegRef As AbstractSegmentLegRef
        Dim pAngleIndication As AngleIndication
        Dim pDistanceIndication As DistanceIndication

        'Dim uomDistVerNameTab() As String
        'uomDistVerNameTab = New String() {"FT", "M", "FL", "SM", "OTHER"}
        'Dim roleNames() As String = New String() {"IAF", "IF", "IF_IAF", "FAF", "VDP", "SDF", "FPAP", "FTP", "FROP", "TP", "MAPT", "MAHF", "OTHER_WPT"}

        TextBox004.Text = ""
        TextBox005.Text = ""
        Label005.Text = ""

		SafeDeleteElement(pFAFptElement)
		SafeDeleteElement(pFAFAreaElement)

		K = ComboBox003.SelectedIndex
        If K < 0 Then
            GetActiveView().Refresh() ' .PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Return
        End If

        SelectedTransition = TransArray(K)
		bFirstPointIsIF = OptionButton002.Checked
		TextBox005.Text = "IF"

        '======================================================================================
        If SelectedTransition.LegIndex <= 0 Then
            pProcedureLegRef = SelectedTransition.pProcedureTransition.TransitionLeg.Item(0).TheSegmentLeg
            If SelectedTransition.LegIndex < 0 Then
                TextBox005.Text = "IAF"
                '	bFirstPointIsIF = False
            Else
                TextBox005.Text = "IF"
            End If
        Else
            pProcedureLegRef = SelectedTransition.pProcedureTransition.TransitionLeg.Item(SelectedTransition.LegIndex).TheSegmentLeg
            TextBox005.Text = "IF"
        End If

        pProcedureLeg = pProcedureLegRef.GetFeature2()
        pIFPoint = pProcedureLeg.StartPoint

		If Not (pIFPoint.Role Is Nothing) Then
			TextBox005.Text = pIFPoint.Role.ToString()
		End If

		If screenCapture Is Nothing Then
			If bFirstPointIsIF Then
				screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.InstrumentApproachProcedure.ToString())
			Else
				screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.StandardInstrumentArrival.ToString())
			End If
		End If

		Identifier = Guid.Empty
        bFound = False
        bOnNav = False

        If Not pIFPoint Is Nothing Then
            If Not pIFPoint.PointChoice Is Nothing Then
                If pIFPoint.PointChoice.Choice = SignificantPointChoice.DesignatedPoint Then
                    Identifier = pIFPoint.PointChoice.FixDesignatedPoint.Identifier
                ElseIf pIFPoint.PointChoice.Choice = SignificantPointChoice.Navaid Then
                    Identifier = pIFPoint.PointChoice.NavaidSystem.Identifier
                End If

				If Identifier <> Guid.Empty Then
                    N = UBound(WPTList)

                    For I = 0 To N
                        If (WPTList(I).NAV_Ident = Identifier) Then
                            StartPoint = WPTList(I)
                            bFound = True
                            Exit For
                        End If
                    Next I
                End If
            End If
        End If

        If Not bFound Then
            GetActiveView().Refresh() ' .PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            MessageBox.Show("Invalid " + TextBox005.Text + " point")
            Return
        End If
        '=================================================================

        Dim pNavaid As Navaid
        Dim pNavaidEquipment As NavaidEquipment
        Dim pIntersectNavaid As Navaid
        Dim pIntersectNavaidEquipment As NavaidEquipment

        Dim ILSAdded As Boolean
        Dim iFound As Integer
        Dim bIntFound As Boolean
        Dim GuidanceNavCode As eNavaidType
        Dim IntersectNavCode As eNavaidType
        Dim pPointReference As PointReference

        iFound = 0
        ILSAdded = False
        IntersectNavCode = eNavaidType.NONE
        GuidanceNavCode = eNavaidType.NONE

        pNavaid = Nothing

        If Not (pIFPoint.FacilityMakeup Is Nothing) Then
            For K = 0 To pIFPoint.FacilityMakeup.Count - 1
                pPointReference = pIFPoint.FacilityMakeup(K)

                If Not (pPointReference.FixToleranceArea Is Nothing) And Not (pPointReference.FixToleranceArea.Geo Is Nothing) Then
                    pIFTolerArea = ToPrj(ConvertToEsriGeom.FromMultiPolygon(pPointReference.FixToleranceArea.Geo))
                End If
                'pIFPoint.FacilityMakeup.Add(pPointReference)
                'pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pEndTolerArea))

                If pIFPoint.PointChoice.Choice = SignificantPointChoice.Navaid Then
                    'If pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD Then
                    bOnNav = True
                    pNavaid = pIFPoint.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)
                    pIntersectNavaid = pIFPoint.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                    If pIntersectNavaid Is Nothing Then Exit For

                    iFound = 3
                    Exit For
                End If

                For I = 0 To pPointReference.FacilityAngle.Count - 1
                    pAngleIndication = pPointReference.FacilityAngle(I).TheAngleIndication.GetFeature2(FeatureType.AngleIndication)

                    If Not pAngleIndication Is Nothing Then
						If pPointReference.FacilityAngle(I).AlongCourseGuidance Then
							If Not pAngleIndication.PointChoice Is Nothing Then
								pNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)
								iFound = iFound Or 1
							End If
						Else
							If Not pAngleIndication.PointChoice Is Nothing Then
								pIntersectNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)
								If pIntersectNavaid Is Nothing Then Continue For
								iFound = iFound Or 2
							End If
						End If
					End If

                    If iFound = 3 Then Exit For
                Next

                If iFound = 3 Then Exit For

                For I = 0 To pPointReference.FacilityDistance.Count - 1
                    pDistanceIndication = pPointReference.FacilityDistance(I).Feature.GetFeature2(FeatureType.DistanceIndication)

                    'If pPointReference.FacilityDistance(I).AlongCourseGuidance Then
                    '	pNavaid = pDistanceIndication.PointChoice.GetFeatureRef().GetFeature()
                    '	GuidanceNavCode = eNavaidType.eNavaidType.DME
                    '	iFound = iFound Or 1
                    'Else
                    '	pIntersectNavaid = pDistanceIndication.PointChoice.GetFeatureRef().GetFeature()
                    '	IntersectNavCode = eNavaidType.eNavaidType.DME
                    '	iFound = iFound Or 2
                    'End If

                    If Not pDistanceIndication Is Nothing Then
                        pIntersectNavaid = pDistanceIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                        If pIntersectNavaid Is Nothing Then Exit For

                        IntersectNavCode = eNavaidType.DME
                        iFound = iFound Or 2
                        Exit For
                    End If
                Next

                If iFound = 3 Then Exit For
            Next
        End If

        bFound = False
        If Not (pNavaid Is Nothing) Then
            For J = 0 To pNavaid.NavaidEquipment.Count - 1
                pNavaidEquipment = pNavaid.NavaidEquipment(J).TheNavaidEquipment.GetFeature2()

                If (TypeOf pNavaidEquipment Is VOR) Or (TypeOf pNavaidEquipment Is NDB) Or (TypeOf pNavaidEquipment Is Localizer) Then
                    If (TypeOf pNavaidEquipment Is Localizer) And Not ILSAdded Then
                        If (GetILSByNavEq(pNavaid, ILS) And 1) = 1 Then
                            'If GetILSByName(ProcedureName, CurrADHP, ILS) >= 1 Then
                            AddILSToNavList(ILS, NavaidList)
                            ILSAdded = True
                        End If
                    End If

                    N = UBound(NavaidList)
                    For I = 0 To N
                        If NavaidList(I).NAV_Ident = pNavaid.Identifier Then
                            StartPoint.GuidanceNav = NavaidList(I)
                            bFound = True
                            Exit For
                        End If
                    Next I
                ElseIf TypeOf pNavaidEquipment Is DME Then
					If GuidanceNavCode = eNavaidType.DME Then   ' pProcedureLeg.LegTypeARINC = CodeSegmentPath.AF
						For I = 0 To N
							If DMEList(I).NAV_Ident = pNavaid.Identifier Then
								StartPoint.GuidanceNav = DMEList(I)
								bFound = True
								Exit For
							End If
						Next I
					End If
				ElseIf (TypeOf pNavaidEquipment Is TACAN) Then
						' ...
					End If

                If bFound Then Exit For
            Next J

            If (iFound And 2) <> 0 Then
                bIntFound = False
                For J = 0 To pIntersectNavaid.NavaidEquipment.Count - 1
                    pIntersectNavaidEquipment = pIntersectNavaid.NavaidEquipment(J).TheNavaidEquipment.GetFeature2()
                    If IntersectNavCode = eNavaidType.DME Then
                        N = UBound(DMEList)
                        For I = 0 To N
                            If DMEList(I).NAV_Ident = pIntersectNavaid.Identifier Then
                                StartPoint.IntersectNav = DMEList(I)
                                bIntFound = True
                                Exit For
                            End If
                        Next I
                    Else
                        If (TypeOf pIntersectNavaidEquipment Is VOR) Or (TypeOf pIntersectNavaidEquipment Is NDB) Or (TypeOf pIntersectNavaidEquipment Is Localizer) Then
                            If (TypeOf pIntersectNavaidEquipment Is Localizer) And Not ILSAdded Then
                                If GetILSByNavEq(pNavaid, ILS) And 1 = 1 Then
                                    'If GetILSByName(ProcedureName, CurrADHP, ILS) >= 1 Then
                                    AddILSToNavList(ILS, NavaidList)
                                    ILSAdded = True
                                End If
                            End If

                            N = UBound(NavaidList)
                            For I = 0 To N
                                If NavaidList(I).NAV_Ident = pIntersectNavaid.Identifier Then
                                    StartPoint.IntersectNav = NavaidList(I)

                                    If (Not bOnNav) And (StartPoint.GuidanceNav.TypeCode = StartPoint.IntersectNav.TypeCode) Then
                                        bOnNav = ((StartPoint.GuidanceNav.TypeCode = eNavaidType.VOR) Or (StartPoint.GuidanceNav.TypeCode = eNavaidType.NDB)) And (ReturnDistanceInMeters(StartPoint.GuidanceNav.pPtPrj, StartPoint.IntersectNav.pPtPrj) < distEps)
                                        StartPoint.TypeCode = StartPoint.GuidanceNav.TypeCode
                                    End If

                                    If bOnNav Then StartPoint.IntersectNav.IntersectionType = eIntersectionType.OnNavaid

                                    bIntFound = True
                                    Exit For
                                End If
                            Next I
                        ElseIf (TypeOf pIntersectNavaidEquipment Is TACAN) Then
                            ' ...
                        End If
                    End If

                    If bIntFound Then Exit For
                Next J
            End If
        ElseIf Not pProcedureLeg.Angle Is Nothing Then
            pAngleIndication = pProcedureLeg.Angle.GetFeature2(FeatureType.AngleIndication)

            '============================================================================================
            If pAngleIndication.PointChoice.Choice = SignificantPointChoice.Navaid Then
                pNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                For J = 0 To pNavaid.NavaidEquipment.Count - 1
                    pNavaidEquipment = pNavaid.NavaidEquipment(J).TheNavaidEquipment.GetFeature2()

                    If (TypeOf pNavaidEquipment Is VOR) Or (TypeOf pNavaidEquipment Is NDB) Or (TypeOf pNavaidEquipment Is Localizer) Then
                        If (TypeOf pNavaidEquipment Is Localizer) And Not ILSAdded Then
                            If GetILSByNavEq(pNavaid, ILS) >= 1 Then
                                'If GetILSByName(ProcedureName, CurrADHP, ILS) >= 1 Then
                                AddILSToNavList(ILS, NavaidList)
                                ILSAdded = True
                            End If
                        End If

                        N = UBound(NavaidList)
                        For I = 0 To N
                            If NavaidList(I).NAV_Ident = pNavaid.Identifier Then
                                StartPoint.GuidanceNav = NavaidList(I)
                                bFound = True
                                Exit For
                            End If
                        Next I
                        'ElseIf TypeOf pNavaidEquipment Is Aran.Aim.Features.DME Then
                        '	For I = 0 To N
                        '		If DMEList(I).pFeature.GetFeatureRef.Identifier = pNavaid.Identifier Then
                        '			StartPoint.GuidanceNav = DMEList(I)
                        '			bFound = True
                        '			Exit For
                        '		End If
                        '	Next I
                    ElseIf (TypeOf pNavaidEquipment Is TACAN) Then
                        ' ...
                    End If

                    If bFound Then Exit For
                Next J
            End If
        End If
        '============================================================================================

        If Not bFound Then
            GetActiveView().Refresh() ' .PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            MessageBox.Show("Invalid guidance navaid")
            Return
        End If

        'If StartPoint.GuidanceNav.TypeCode = eNavaidType.eNavaidType.DME Then
        '	StartPoint.TrackType = TrackTypeArc
        'Else
        '	StartPoint.TrackType = TrackType.Straight
        'End If


        'bIsHolding = False
        'If Not pProcedureLeg.LegTypeARINC Is Nothing Then
        '	If (Not bFirstPointIsIF) And (pProcedureLeg.LegTypeARINC >= CodeSegmentPath.HF) And (pProcedureLeg.LegTypeARINC <= CodeSegmentPath.HM) Then
        '		bIsHolding = True
        '	End If
        'End If

        bIsHolding = bOnNav

        TextBox004.Text = StartPoint.GuidanceNav.CallSign
        Label005.Text = GetNavTypeName(StartPoint.GuidanceNav.TypeCode)

		'Label013.Text = GetNavTypeName(StartPoint.IntersectNav.TypeCode)

		If Not pProcedureLeg.UpperLimitAltitude Is Nothing Then
			Altitude = ConverterToSI.Convert(pProcedureLeg.UpperLimitAltitude, 0.0)
		Else
			Altitude = ConverterToSI.Convert(pProcedureLeg.LowerLimitAltitude, 0.0)
		End If


		TextBox002.Text = CStr(ConvertHeight(Altitude, eRoundMode.NEAREST))

        TextBox001.ReadOnly = Not bIsHolding
        If bIsHolding Then
            TextBox001.BackColor = SystemColors.Window
        Else
            TextBox001.BackColor = SystemColors.Control
        End If

		If Not pProcedureLeg.Course Is Nothing Then
			Course = pProcedureLeg.Course.Value
		Else
			pProcedureLegRef = SelectedTransition.pProcedureTransition.TransitionLeg.Item(1).TheSegmentLeg
			pProcedureLeg = pProcedureLegRef.GetFeature2()
			Course = pProcedureLeg.Course.Value
		End If

		TextBox001.Text = CStr(Math.Round(Course, 4))

		StartPoint.pPtGeo.M = Course
        StartPoint.pPtPrj.M = Azt2Dir(StartPoint.pPtGeo, Course)

        pIFptGeo = StartPoint.pPtGeo
        pIFptGeo.Z = Altitude

        pIFptPrj = StartPoint.pPtPrj
        pIFptPrj.Z = Altitude

        pFAFptPrj = PointAlongPlane(pIFptPrj, pIFptPrj.M, arMinISlen)
        pFAFptPrj.M = pIFptPrj.M
        pFAFptPrj.Z = pIFptGeo.Z

        pFAFptElement = DrawPoint(pIFptPrj, 255)
        pFAFptElement.Locked = True

        If Not pIFTolerArea Is Nothing Then
            pFAFAreaElement = DrawPolygon(pIFTolerArea, 255)
            pFAFAreaElement.Locked = True
        End If

    End Sub

    Private Sub OptionButton001_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton001.CheckedChanged, OptionButton002.CheckedChanged
        If Not sender.Checked Then Return

		bFirstPointIsIF = OptionButton002.Checked

		If bFirstPointIsIF Then
			screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.InstrumentApproachProcedure.ToString())
		Else
			screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.StandardInstrumentArrival.ToString())
		End If

		'If sender Is OptionButton001 Then
		'	TextBox005.Text = "IF"
		'Else
		'	TextBox005.Text = "IAF"
		'End If
	End Sub

    Private Sub ComboBox004_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox004.SelectedIndexChanged
        arMinISlen = MinISlensArray(ComboBox004.SelectedIndex)
        TextBox003.Text = CStr(ConvertDistance(arMinISlen, 2))
    End Sub

    Private Function FillComboBox103List() As Integer
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim fDir As Double
        Dim fDist As Double
        'Dim fTurnAngle As Double

        Dim bPass As Boolean
        Dim OldWPT As WPT_FIXType
        Dim GuidNav As NavaidData
        Dim NextFIX As StepDownFIX

        If ComboBox103.SelectedIndex >= 0 Then
            OldWPT = ComboBox103.SelectedItem
        End If

        ComboBox103.Items.Clear()

        K = ComboBox101.SelectedIndex
        If K < 0 Then
            Return 0
        End If

        GuidNav = ComboBox101.SelectedItem

        N = UBound(WPTList)
        If N < 0 Then
            Return 0
        End If

        NextFIX = StepDownFIXs(StepDownsNum - 1)
        J = -1
        K = 0

        For I = 0 To N
            bPass = False
            If GuidNav.Identifier <> WPTList(I).Identifier Then
                fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, WPTList(I).pPtPrj)

                If GuidNav.TypeCode = eNavaidType.DME Then
                    bPass = (fDist >= CurrInterval.Left) And (fDist <= CurrInterval.Right)
                ElseIf (fDist > 1.0#) And (fDist <= MaxNAVDist) Then
                    If (CurrInterval.Left = 0.0#) And (CurrInterval.Right = 360.0#) Then
                        bPass = True
                    Else
                        fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, WPTList(I).pPtPrj)
                        bPass = AngleInSector(fDir, CurrInterval.Left, CurrInterval.Right) Or AngleInSector(fDir + 180.0#, CurrInterval.Left, CurrInterval.Right)
                    End If

                Else
                    bPass = False
                End If

                If bPass Then
                    J = J + 1
                    If WPTList(I).Identifier = OldWPT.Identifier Then K = J
                    ComboBox103.Items.Add(WPTList(I))
                End If
            End If
        Next I

        If J >= 0 Then
            OptionButton104.Enabled = True
            K = 0
            For I = 0 To ComboBox103.Items.Count - 1
                If (ComboBox103.Items(I).Identifier = OldWPT.Identifier) Then
                    K = I
                    Exit For
                End If
            Next I
            ComboBox103.SelectedIndex = K
        Else
            OptionButton103.Checked = True
            OptionButton104.Enabled = False
        End If

        If OptionButton103.Checked Then TextBox109_Validating(TextBox109, New CancelEventArgs(False))
        Return ComboBox103.Items.Count
    End Function

    Private Sub GetValidRange()
        Dim K As Integer
        Dim NavSide As Integer
        Dim TurnDir As Double
        Dim RecommendStr As String

        Dim pPtTmp As IPoint
        Dim pPtIntr As IPoint
        Dim GuidNav As NavaidData
        Dim NextFIX As StepDownFIX
        'Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
        'Dim pInnerCircle As ESRI.ArcGIS.Geometry.IPointCollection
        'Dim pOuterCircle As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pConstructPoint As IConstructPoint
        'Dim pTopologic As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        K = ComboBox101.SelectedIndex
        If K < 0 Then Exit Sub


        GuidNav = ComboBox101.SelectedItem


        NextFIX = StepDownFIXs(StepDownsNum - 1)
        TurnDir = 2 * ComboBox102.SelectedIndex - 1

        If GuidNav.TypeCode = eNavaidType.DME Then
            If ComboBox102.SelectedIndex > 0 Then
                If ComboBox105.SelectedIndex > 0 Then

                    CurrInterval = ComboBox101_Intervals00(K)
                Else

                    CurrInterval = ComboBox101_Intervals01(K)
                End If
            Else
                If ComboBox105.SelectedIndex > 0 Then

                    CurrInterval = ComboBox101_Intervals10(K)
                Else

                    CurrInterval = ComboBox101_Intervals11(K)
                End If
            End If

            RecommendStr = str20312 & Chr(13) & str00221 & CStr(ConvertDistance(CurrInterval.Left, 2)) & DistanceConverter(DistanceUnit).Unit & str00222 & CStr(ConvertDistance(CurrInterval.Right, 2)) & DistanceConverter(DistanceUnit).Unit
        Else
            NavSide = SideDef(NextFIX.pPtPrj, NextFIX.InDir, GuidNav.pPtPrj)
            pPtIntr = New ESRI.ArcGIS.Geometry.Point
            pConstructPoint = pPtIntr


            pPtTmp = NextFIX.pPtPrj

            RecommendStr = str13008 & Chr(13) '"Рекомендуемые интервалы курса: "

            If ComboBox102.SelectedIndex > 0 Then
                If ComboBox106.SelectedIndex > 0 Then

                    CurrInterval = ComboBox101_Intervals00(K)
                Else

                    CurrInterval = ComboBox101_Intervals01(K)
                End If

                If (CurrInterval.Left = 0.0#) And (CurrInterval.Right = 360.0#) Then
                    RecommendStr = RecommendStr & " 360°"
                ElseIf SubtractAngles(CurrInterval.Left, CurrInterval.Right) <= degEps Then
                    RecommendStr = RecommendStr & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Left) - GuidNav.MagVar), 1)) & "°"
                Else
                    RecommendStr = RecommendStr & str00221 & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Right) - GuidNav.MagVar), 2)) & "°" & str00222 & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Left) - GuidNav.MagVar), 2)) & "°"
                End If
            Else
                If ComboBox106.SelectedIndex > 0 Then

                    CurrInterval = ComboBox101_Intervals10(K)
                Else

                    CurrInterval = ComboBox101_Intervals11(K)
                End If

                If (CurrInterval.Left = 0.0#) And (CurrInterval.Right = 360.0#) Then
                    RecommendStr = RecommendStr & " 360°"
                ElseIf SubtractAngles(CurrInterval.Left, CurrInterval.Right) <= degEps Then
                    RecommendStr = RecommendStr & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Left) - GuidNav.MagVar), 1)) & "°"
                Else
                    RecommendStr = RecommendStr & str00221 & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Right) - GuidNav.MagVar), 2)) & "°" & str00222 & CStr(Math.Round(Modulus(Dir2Azt(pPtTmp, CurrInterval.Left) - GuidNav.MagVar), 2)) & "°"
                End If
            End If

        End If

        ToolTip1.SetToolTip(TextBox109, RecommendStr)
        Label120.Text = RecommendStr
        If CurrInterval.Tag >= 0 Then FillComboBox103List()

    End Sub

    Private Sub ComboBox005_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox005.SelectedIndexChanged
        If ComboBox005.SelectedIndex = 1 Then
            arAreaHalfWidth = 14816.0# '8* 1852.0
        Else
            arAreaHalfWidth = 9260.0# '5 * 1852.0
        End If

        'arAreaHalfWidth = DeConvertHeight(CDbl(ComboBox005.Text))
    End Sub

    Private Sub ComboBox006_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox006.SelectedIndexChanged
        Dim K As Integer
        K = ComboBox006.SelectedIndex
        If K < 0 Then Exit Sub

        MOCLimit = EnrouteMOCValues(K)
    End Sub

    Private Sub ComboBox101_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox101.SelectedIndexChanged
        'Dim C As Integer
        Dim K As Integer
        Dim GuidNav As NavaidData

        K = ComboBox101.SelectedIndex
        If K < 0 Then Exit Sub

        Try
            GuidNav = ComboBox101.SelectedItem
            Label106.Text = GetNavTypeName(GuidNav.TypeCode)

            If GuidNav.TypeCode = eNavaidType.DME Then
                Label108.Text = str00229
                Label109.Text = DistanceConverter(DistanceUnit).Unit
                Label116.Visible = True
                ComboBox105.Visible = True

                Label121.Visible = False
                ComboBox106.Visible = False

                ComboBox105.Enabled = ((GuidNav.Tag And 5) > 0) And ((GuidNav.Tag And 10) > 0)
                If ComboBox105.Enabled Then
                    If ComboBox105.SelectedIndex < 0 Then ComboBox105.SelectedIndex = 0
                Else
                    If (GuidNav.Tag And 10) > 0 Then
                        ComboBox105.SelectedIndex = 0
                    Else
                        ComboBox105.SelectedIndex = 1
                    End If
                End If

                ComboBox102.Enabled = ((GuidNav.Tag And 3) > 0) And ((GuidNav.Tag And 12) > 0)
                If ComboBox102.Enabled Then
                    If ComboBox102.SelectedIndex < 0 Then ComboBox102.SelectedIndex = 0
                Else
                    If (GuidNav.Tag And 12) > 0 Then
                        ComboBox102.SelectedIndex = 0
                    Else
                        ComboBox102.SelectedIndex = 1
                    End If
                End If
            Else
                Label108.Text = str10208
                Label109.Text = "°"

                Label116.Visible = False
                ComboBox105.Visible = False

                Label121.Visible = True
                ComboBox106.Visible = True

                'ComboBox106.Enabled = (ComboBox101_Intervals01(K).Tag >= 0) Or (ComboBox101_Intervals11(K).Tag >= 0)
                'ComboBox106.Enabled = ((GuidNav.Tag And 2) > 0) Or ((GuidNav.Tag And 8) > 0)
                ComboBox106.Enabled = ((GuidNav.Tag And 5) > 0) And ((GuidNav.Tag And 10) > 0)
                If ComboBox106.Enabled Then
                    If ComboBox106.SelectedIndex < 0 Then ComboBox106.SelectedIndex = 0
                Else
                    If (GuidNav.Tag And 10) > 0 Then
                        ComboBox106.SelectedIndex = 0
                    Else
                        ComboBox106.SelectedIndex = 1
                    End If
                End If

                'ComboBox102.Enabled = ((ComboBox101_Intervals00(K).Tag >= 0) Or (ComboBox101_Intervals01(K).Tag >= 0)) And ((ComboBox101_Intervals10(K).Tag >= 0) Or (ComboBox101_Intervals11(K).Tag >= 0))
                ComboBox102.Enabled = ((GuidNav.Tag And 3) > 0) And ((GuidNav.Tag And 12) > 0)

                If ComboBox102.Enabled Then
                    If ComboBox102.SelectedIndex < 0 Then ComboBox102.SelectedIndex = 0
                Else
                    If (GuidNav.Tag And 12) > 0 Then
                        'If ComboBox101_Intervals10(K).Tag >= 0 Then
                        ComboBox102.SelectedIndex = 0
                    Else
                        ComboBox102.SelectedIndex = 1
                    End If
                End If
            End If

            GetValidRange()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub ComboBox102_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox102.SelectedIndexChanged
        GetValidRange()
    End Sub

    Private Sub DoTurn(ByVal course As Double)
        Dim K As Integer
        Dim N As Integer
        Dim TurnDir As Integer
        Dim NavSide As Integer


        Dim bNavCode As Boolean
        Dim fDist As Double
        Dim fIADir As Double

        Dim fTmpAzt1 As Double
        Dim fTmpAzt0 As Double
        Dim fOnNavRad As Double
        Dim fMaxTurnAngle As Double

        Dim pClone As IClone
        Dim pPtTmp As IPoint
        Dim pPtWarn As IPoint
        Dim IAFNav As NavaidData
        Dim GuidNav As NavaidData

        Dim NextFIX As StepDownFIX
        Dim pNominalPoly As IPointCollection
        Dim pConstructPoint As IConstructPoint
        Dim pIAF_IAreaPoly As IPointCollection
        Dim pIAF_IIAreaPoly As IPointCollection

        On Error Resume Next
        If Not (pNomLineElem Is Nothing) Then pGraphics.DeleteElement(pNomLineElem)

        pNomLineElem = Nothing
        On Error GoTo 0

        K = ComboBox101.SelectedIndex
        If K < 0 Then Exit Sub

        GuidNav = ComboBox101.SelectedItem

        NextFIX = StepDownFIXs(StepDownsNum - 1)
        TurnDir = 2 * ComboBox102.SelectedIndex - 1

        IAFNav = NextFIX.GuidanceNav

        'Set pNominalPoly = New Polyline
        'pNominalPoly.AddPoint GuidNav.pPtPrj
        'pNominalPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, Val + 180#, 350000#)
        'DrawPolyLine pNominalPoly, , 2

        If GuidNav.TypeCode = eNavaidType.DME Then
            If IAFNav.TypeCode = eNavaidType.DME Then
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            Else
                CircleVectorIntersect(GuidNav.pPtPrj, course, NextFIX.pPtPrj, NextFIX.InDir + 180.0# * ComboBox105.SelectedIndex, ptKT1)
            End If
        Else
            If IAFNav.TypeCode = eNavaidType.DME Then
                fDist = ReturnDistanceInMeters(IAFNav.pPtPrj, NextFIX.pPtPrj)
                CircleVectorIntersect(IAFNav.pPtPrj, fDist, GuidNav.pPtPrj, course + 90.0# * (TurnDir * NextFIX.ArcDir - 1), ptKT1)
            Else
                ptKT1 = New ESRI.ArcGIS.Geometry.Point
                'If ReturnDistanceInMeters(NextFIX.pPtPrj, GuidNav.pPtPrj) < distEps Then
                If Point2LineDistancePrj(NextFIX.pPtPrj, GuidNav.pPtPrj, NextFIX.InDir) < distEps Then
                    ptKT1.PutCoords(GuidNav.pPtPrj.X, GuidNav.pPtPrj.Y)
                Else
                    pConstructPoint = ptKT1
                    pConstructPoint.ConstructAngleIntersection(NextFIX.pPtPrj, DegToRadValue * NextFIX.InDir, GuidNav.pPtPrj, DegToRadValue * course)
                End If
            End If
        End If

        '    Set StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
        '    Set StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
        '    Set StepDownFIXs(StepDownsNum).pPtStart = ptKT1

        'DrawPointWithText ptKT1, "ptKT1"

        '==========================================================================================================

        If IAFNav.TypeCode = eNavaidType.DME Then 'DME
            fTmpAzt0 = ReturnAngleInDegrees(IAFNav.pPtPrj, NextFIX.pPtPrj)
            fTmpAzt1 = ReturnAngleInDegrees(IAFNav.pPtPrj, ptKT1)
            NavSide = SideDef(IAFNav.pPtPrj, course + 90.0#, ptKT1)

            StepDownFIXs(StepDownsNum).OutDir = fTmpAzt1 - 90.0# * TurnDir * NavSide
            'StepDownFIXs(StepDownsNum).ArcDir = TurnFlg

            fDist = ReturnDistanceInMeters(IAFNav.pPtPrj, NextFIX.pPtPrj) * DegToRad(Modulus(NextFIX.ArcDir * (fTmpAzt1 - fTmpAzt0)))
        Else 'VOR, NDB
            StepDownFIXs(StepDownsNum).OutDir = NextFIX.InDir
            StepDownFIXs(StepDownsNum).ArcDir = 0
            fDist = ReturnDistanceInMeters(NextFIX.pPtPrj, ptKT1)
        End If

        StepDownFIXs(StepDownsNum).Length = fDist
        TextBox101.Text = CStr(ConvertDistance(fDist, 3))

        '==========================================================================================================
        SafeDeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)

        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(ptKT1, 0)
        StepDownFIXs(StepDownsNum).ptElem.Locked = True

        'Разворот ================================================================================
        ptKT1.Z = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z

        StepDownFIXs(StepDownsNum - 1).pPtWarn = ptKT1
        StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
        StepDownFIXs(StepDownsNum).pPtStart = ptKT1
        ''==================================================================
        fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ptKT1)
        fOnNavRad = -10000.0#

        If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
            fOnNavRad = VOR.OnNAVRadius
        ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
            fOnNavRad = NDB.OnNAVRadius
        End If

        fMaxTurnAngle = MaxInterceptAngle 'Abs(CDbl(Right(ComboBox103.Text, 3)))

        If fDist < fOnNavRad Then
            If TurnDir < 0 Then
                fTmpAzt0 = Dir2Azt(ptKT1, StepDownFIXs(StepDownsNum).OutDir)
                fTmpAzt1 = Dir2Azt(ptKT1, StepDownFIXs(StepDownsNum).OutDir + fMaxTurnAngle * TurnDir)
            Else
                fTmpAzt1 = Dir2Azt(ptKT1, StepDownFIXs(StepDownsNum).OutDir)
                fTmpAzt0 = Dir2Azt(ptKT1, StepDownFIXs(StepDownsNum).OutDir + fMaxTurnAngle * TurnDir)
            End If

            'fIADir = CDbl(TextBox109.Text) + GuidNav.MagVar
            fIADir = turnMagBearing + GuidNav.MagVar

            If Not AngleInSector(fIADir, fTmpAzt0, fTmpAzt1) Then
                GuidNav.Tag = -1

                If AnglesSideDef(fTmpAzt0 + 0.5 * fMaxTurnAngle, fIADir) < 0 Then
                    fIADir = fTmpAzt1
                Else
                    fIADir = fTmpAzt0
                End If
            End If


            pPtTmp = ToGeo(ptKT1)
            fIADir = Azt2Dir(pPtTmp, fIADir)
        Else
            If GuidNav.TypeCode = eNavaidType.DME Then
                fIADir = ReturnAngleInDegrees(ptKT1, GuidNav.pPtPrj) + 90.0# * TurnDir
            Else
                fIADir = course
            End If
        End If

        '==================================================================
        On Error Resume Next
        If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
        If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
        If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)
        On Error GoTo 0

        StepDownFIXs(StepDownsNum).InDir = fIADir
        StepDownFIXs(StepDownsNum).TurnDir = TurnDir


        CreateIAF_AreaPoly(GuidNav, StepDownFIXs(StepDownsNum), pIAF_IAreaPoly, pIAF_IIAreaPoly, pNominalPoly)

        'EstimateTRN_AreaObstacles
        'GuidNav = ComboBox101NavList(ComboBox101.ListIndex)
        'Set StepDownFIXs(StepDownsNum).pIAreaPoly = pIAF_IAreaPoly
        'Set StepDownFIXs(StepDownsNum).pIIAreaPoly = pIAF_IIAreaPoly


        pIAF_IIAreaElement = DrawPolygon(pIAF_IIAreaPoly, RGB(0, 255, 96))
        pIAF_IIAreaElement.Locked = True


        pIAF_IAreaElement = DrawPolygon(pIAF_IAreaPoly, RGB(235, 0, 96))
        pIAF_IAreaElement.Locked = True


        pNominalElement = DrawPolyLine(pNominalPoly, 255, 2)
        pNominalElement.Locked = True
    End Sub

    Private Sub ComboBox103_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox103.SelectedIndexChanged
        Dim K As Integer
        Dim TurnDir As Integer

        Dim fRDME As Double
        Dim fDir As Double
        Dim fDist As Double
        Dim fDistToNav As Double
        'Dim fTurnAngle As Double

        Dim GuidNav As NavaidData
        Dim NextFIX As StepDownFIX
        Dim SelectedWPT As WPT_FIXType

        K = ComboBox103.SelectedIndex
        If K < 0 Then Exit Sub
        If Not OptionButton104.Checked Then Exit Sub


        SelectedWPT = ComboBox103.SelectedItem
        Label131.Text = GetNavTypeName(SelectedWPT.TypeCode)

        K = ComboBox101.SelectedIndex
        If K < 0 Then Exit Sub

        GuidNav = ComboBox101.SelectedItem


        NextFIX = StepDownFIXs(StepDownsNum - 1)

        If GuidNav.TypeCode = eNavaidType.DME Then
            fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, SelectedWPT.pPtPrj)
            TextBox109.Text = CStr(ConvertDistance(fDist, 2))
            DoTurn(fDist)
        Else
            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, SelectedWPT.pPtPrj)
            If Not AngleInSector(fDir, CurrInterval.Left, CurrInterval.Right) Then
                fDir = Modulus(fDir + 180.0#)
            End If
            'If StepDownFIXs(StepDownsNum - 1).GuidanceNav.TypeCode = eNavaidType.DME Then
            '    fRDME = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
            '    fDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, SelectedWPT.pPtPrj)
            '    fDistToNav = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, GuidNav.pPtPrj)

            '    If fDist > fDistToNav Then
            '        If fDist < fRDME Then
            '            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, SelectedWPT.pPtPrj)
            '        Else
            '            fDir = ReturnAngleInDegrees(SelectedWPT.pPtPrj, GuidNav.pPtPrj)
            '        End If
            '    Else
            '        If fDist < fRDME Then
            '            fDir = ReturnAngleInDegrees(SelectedWPT.pPtPrj, GuidNav.pPtPrj)
            '        Else
            '            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, SelectedWPT.pPtPrj)
            '        End If
            '    End If
            'Else
            '    fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, SelectedWPT.pPtPrj)
            '    TurnDir = 2 * ComboBox102.SelectedIndex - 1
            '    If Not AngleInSector(fDir, CurrInterval.Left, CurrInterval.Right) Then
            '        fDir = Modulus(fDir + 180.0#)
            '    End If
            'End If

            turnMagBearing = Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar
            TextBox109.Text = CStr(Math.Round(turnMagBearing, 2))

            DoTurn(fDir)
        End If
    End Sub

    Private Sub ComboBox105_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox105.SelectedIndexChanged
        GetValidRange()
    End Sub

    Private Sub ComboBox106_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox106.SelectedIndexChanged
        GetValidRange()
    End Sub

    Private Sub ComboBox104_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ComboBox104.SelectedIndexChanged
        Dim I As Integer
        Dim N As Integer
        Dim K As Integer
        Dim Kmin As Double
        Dim Kmax As Double
        Dim tipStr As String
        Dim InterNav As NavaidData

        SafeDeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)

        K = ComboBox104.SelectedIndex
        If K < 0 Then
            Label111.Text = ""
            Label127.Text = ""
            Label112.Text = ""
            Exit Sub
        End If

        InterNav = ComboBox104.SelectedItem
        Label111.Text = GetNavTypeName(InterNav.TypeCode)

        If MultiPage1.SelectedIndex = 0 Then Exit Sub

        '    Label127.Visible = True
        '    TextBox102.Visible = True
        '    If InterNav.IntersectionType = ByDistance Then
        '        Label127.Caption = DistanceConverter(DistanceUnit).Unit
        '    Else 'If InterNav.IntersectionType = ByAngle Then
        '        Label127.Caption = "°"
        '    End If
        '=============================================================================================================

        If (InterNav.IntersectionType = eIntersectionType.ByDistance) Or (InterNav.IntersectionType = eIntersectionType.RadarFIX) Then
            TextBox102.Visible = True

            Label127.Text = DistanceConverter(DistanceUnit).Unit

            N = UBound(InterNav.ValMin)

            OptionButton105.Enabled = N > 0
            OptionButton106.Enabled = N > 0

            If OptionButton105.Checked() Or (N = 0) Then
                TextBox102.Text = CStr(ConvertDistance(InterNav.ValMin(0), 2))
            Else
                TextBox102.Text = CStr(ConvertDistance(InterNav.ValMin(1), 2))
            End If

            If N = 0 Then
                If InterNav.ValCnt < 0 Then
                    OptionButton105.Checked = True
                Else
                    OptionButton106.Checked = True
                End If
            Else
                OptionButton105.Checked = True
            End If

            Label112.Text = str00229
            tipStr = str10514

            For I = 0 To N
                tipStr = tipStr & str00221 & CStr(ConvertDistance(InterNav.ValMin(I), 2)) & " " & DistanceConverter(DistanceUnit).Unit & str00222 & CStr(ConvertDistance(InterNav.ValMax(I), 2)) & " " & DistanceConverter(DistanceUnit).Unit
                If I < N Then
                    tipStr = str00230 & " - " & tipStr & Chr(13) & str00231 & " - "
                End If
            Next I
            '===
        Else
            OptionButton105.Enabled = False
            OptionButton106.Enabled = False

            TextBox102.Visible = InterNav.IntersectionType <> eIntersectionType.OnNavaid
            If InterNav.IntersectionType = eIntersectionType.OnNavaid Then
                TextBox102.Text = ""
                tipStr = "FIX " & str00106
                Label112.Text = tipStr
                Label127.Text = ""
            Else
                Label127.Text = "°"
                Kmin = Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))
                Kmax = Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))

                If InterNav.TypeCode = eNavaidType.NDB Then
                    Label112.Text = str00228
                    Kmax = Modulus(Kmax + 180.0# - InterNav.MagVar)
                    Kmin = Modulus(Kmin + 180.0# - InterNav.MagVar)
                Else
                    Label112.Text = str00227
                    Kmax = Modulus(Kmax - InterNav.MagVar)
                    Kmin = Modulus(Kmin - InterNav.MagVar)
                End If
                tipStr = str00220 & str00221

                If InterNav.ValCnt > 0 Then
                    TextBox102.Text = CStr(Math.Round(Kmin + 0.4999))
                Else
                    TextBox102.Text = CStr(Math.Round(Kmax - 0.4999))
                End If
                tipStr = tipStr & CStr(Math.Round(Kmin, 1)) & " °" & str00222 & CStr(Math.Round(Kmax, 1)) & " °"
            End If
        End If

        'Label_111.Caption = tipStr
        If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
            tipStr = Replace(tipStr, Chr(13), "   ")
            ToolTip1.SetToolTip(TextBox102, tipStr)
        End If

        CheckBox101.Checked = False
        FillComboBox107(TextBox102.Visible)

        TextBox102.Tag = "-"
        TextBox102_Validating(TextBox102, New CancelEventArgs(True))
    End Sub

    Private Sub AddBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles AddBtn.Click
        Dim I As Integer
        Dim N As Integer
        Dim Side As Integer
        Dim TurnDir As Integer
        Dim TurnFlg As Integer
        Dim fTmp As Double
        Dim fIADir As Double
        Dim InterceptionType As Integer

        Dim IAF_IAreaPoly As IPolygon
        Dim IAF_IIAreaPoly As IPolygon
        Dim pNominalPolyline As IPolyline

        Dim pCutPoly As IPolyline
        Dim pTmpLeftPoly As IPolygon
        Dim pTmpRightPoly As IPolygon
        Dim pTmpPoly As IPointCollection
        Dim pTopo As ITopologicalOperator2


        StepDownFIXs(0).Name = TextBox005.Text
        StepDownFIXs(0).Role = pIFPoint.Role

        If OptionButton101.Checked Then
            If ComboBox104.Items.Count = 0 Then
                MsgBox(str00514)
                Exit Sub
            End If

            screenCapture.Save(Me)
            If StepDownsNum > 0 Then
                StepDownFIXs(StepDownsNum).Name = "SDF"
                StepDownFIXs(StepDownsNum).Role = CodeProcedureFixRole.SDF
            End If

            StepDownFIXs(StepDownsNum).GuidanceNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav

            If StepDownFIXs(StepDownsNum).GuidanceNav.TypeCode = eNavaidType.DME Then
                StepDownFIXs(StepDownsNum).Track = TrackType.Arc
            Else
                StepDownFIXs(StepDownsNum).Track = TrackType.Straight
            End If

            StepDownFIXs(StepDownsNum).IntersectNav = ComboBox104.SelectedItem
            StepDownFIXs(StepDownsNum).pPtPrj.Z = DeConvertHeight(CDbl(TextBox110.Text))

            StepDownFIXs(StepDownsNum).PDG = 0.01 * CDbl(TextBox103.Text)
            StepDownFIXs(StepDownsNum).InDir = StepDownFIXs(StepDownsNum).OutDir
            StepDownFIXs(StepDownsNum).Counted = 1
            StepDownFIXs(StepDownsNum).TurnDir = 0
            StepDownFIXs(StepDownsNum).TurnDir_A = 0
            StepDownFIXs(StepDownsNum).ArcDir = StepDownFIXs(StepDownsNum - 1).ArcDir
            StepDownFIXs(StepDownsNum).TotalLength = 0
            TextBox110Range.Left = StepDownFIXs(StepDownsNum).pPtPrj.Z
        Else
            If ComboBox101.Items.Count = 0 Then
                MsgBox(str00515)
                Exit Sub
            End If

            screenCapture.Save()
            '===========================================================================================================
            TurnDir = 2 * ComboBox102.SelectedIndex - 1

            If StepDownFIXs(StepDownsNum).Length < distError Then
                If StepDownsNum > 2 Then
                    On Error Resume Next

                    If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)

                    If Not StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
                    If Not StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
                    If Not StepDownFIXs(StepDownsNum - 1).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)
                    On Error GoTo 0

                    StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = StepDownFIXs(StepDownsNum).pIAreaPolyElem
                    StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = StepDownFIXs(StepDownsNum).pIIAreaPolyElem
                    StepDownFIXs(StepDownsNum - 1).pNominalElem = StepDownFIXs(StepDownsNum).pNominalElem

                    ptKT1 = StepDownFIXs(StepDownsNum - 1).pPtPrj

                    StepDownFIXs(StepDownsNum - 1).InDir = StepDownFIXs(StepDownsNum).InDir
                    StepDownsNum = StepDownsNum - 1
                    StepDownFIXs(StepDownsNum).Counted = 2
                Else
                    StepDownFIXs(StepDownsNum - 1).Counted = 2
                    CreateTPPoint()
                    'StepDownFIXs(0).Name = TextBox005.Text
                    'StepDownFIXs(0).Role = pIFPoint.Role

                End If
            Else
                CreateTPPoint()
            End If

            If StepDownsNum < 2 Then
                StepDownFIXs(StepDownsNum).TotalLength = 0
            Else
                If StepDownFIXs(StepDownsNum).Role = CodeProcedureFixRole.SDF Then
                    StepDownFIXs(StepDownsNum).TotalLength = 0
                Else
                    StepDownFIXs(StepDownsNum).TotalLength = StepDownFIXs(StepDownsNum - 1).TotalLength + StepDownFIXs(StepDownsNum).Length
                End If
            End If


            StepDownFIXs(StepDownsNum).GuidanceNav = ComboBox101.SelectedItem

            If StepDownFIXs(StepDownsNum).GuidanceNav.TypeCode = eNavaidType.DME Then
                StepDownFIXs(StepDownsNum).Track = TrackType.Arc
                TurnFlg = 2 * ComboBox105.SelectedIndex - 1

                StepDownFIXs(StepDownsNum).ArcDir = TurnDir * TurnFlg
            Else
                StepDownFIXs(StepDownsNum).Track = TrackType.Straight
            End If

            StepDownFIXs(StepDownsNum).TurnDir = TurnDir

            SafeDeleteElement(pIAF_IAreaElement)
            SafeDeleteElement(pIAF_IIAreaElement)
            SafeDeleteElement(pNominalElement)

            GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

        '================================================================================
        SafeDeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)

        SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)

        N = UBound(IAFProhibSectors)
        For I = 0 To N
            SafeDeleteElement(IAFProhibSectors(I).ObsAreaElement)
        Next I

        GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        If StepDownsNum >= 2 Then
            IAF_IAreaPoly = StepDownFIXs(StepDownsNum - 1).pIAreaPoly
            IAF_IIAreaPoly = StepDownFIXs(StepDownsNum - 1).pIIAreaPoly
            pNominalPolyline = StepDownFIXs(StepDownsNum - 1).pNominalPoly

            pCutPoly = New Polyline

            If StepDownFIXs(StepDownsNum - 1).Track = TrackType.Arc Then
                If StepDownFIXs(StepDownsNum - 1).ArcDir > 0 Then
                    pCutPoly.FromPoint = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).OutDir + 90.0#, 100000.0#)
                    pCutPoly.ToPoint = StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj
                Else
                    pCutPoly.FromPoint = StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj
                    pCutPoly.ToPoint = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).OutDir - 90.0#, 100000.0#)
                End If
            Else
                pCutPoly.FromPoint = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).OutDir + 90.0#, 100000.0#)
                pCutPoly.ToPoint = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).OutDir - 90.0#, 100000.0#)
            End If

            ClipByLine(IAF_IIAreaPoly, pCutPoly, pTmpLeftPoly, pTmpRightPoly, pTmpPoly)

            If OptionButton101.Checked Then
                pTopo = StepDownFIXs(StepDownsNum).pFixPoly
                pTopo.IsKnownSimple_2 = False

                pTopo.Simplify()
            Else
                pTopo = New ESRI.ArcGIS.Geometry.Polygon
            End If


            StepDownFIXs(StepDownsNum - 1).pIIAreaPoly = pTopo.Union(pTmpLeftPoly)

            StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, RGB(0, 96, 255))
            StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem.Locked = True

            ClipByLine(IAF_IAreaPoly, pCutPoly, pTmpLeftPoly, pTmpRightPoly, pTmpPoly)

            StepDownFIXs(StepDownsNum - 1).pIAreaPoly = pTopo.Union(pTmpLeftPoly)
            StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIAreaPoly, RGB(235, 96, 0))
            StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem.Locked = True
        End If
        '===================================

        CreateApproachGeometry()

        '================================================================================

        CreateIAF_AreaPoly(StepDownFIXs(StepDownsNum).GuidanceNav, StepDownFIXs(StepDownsNum), IAF_IAreaPoly, IAF_IIAreaPoly, pNominalPolyline)
        '====================================================================================

        StepDownFIXs(StepDownsNum).pIAreaPoly = IAF_IAreaPoly
        StepDownFIXs(StepDownsNum).pIIAreaPoly = IAF_IIAreaPoly
        StepDownFIXs(StepDownsNum).pNominalPoly = pNominalPolyline

        StepDownFIXs(StepDownsNum).pIIAreaPolyElem = DrawPolygon(IAF_IIAreaPoly, RGB(0, 255, 96))
        StepDownFIXs(StepDownsNum).pIIAreaPolyElem.Locked = True

        StepDownFIXs(StepDownsNum).pIAreaPolyElem = DrawPolygon(IAF_IAreaPoly, RGB(235, 0, 96))
        StepDownFIXs(StepDownsNum).pIAreaPolyElem.Locked = True

        StepDownFIXs(StepDownsNum).pNominalElem = DrawPolyLine(pNominalPolyline, 255, 2)
        StepDownFIXs(StepDownsNum).pNominalElem.Locked = True

        '================================================================================
        If OptionButton101.Checked Then

            StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum).pFixPoly, 255)
            StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
        End If

        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(StepDownFIXs(StepDownsNum).pPtPrj, RGB(255, 0, 255))
        StepDownFIXs(StepDownsNum).ptElem.Locked = True
        AddSegmentToReportForm()

        '================================================================================

        RemoveBtn.Enabled = True
        StepDownsNum = StepDownsNum + 1

        If StepDownsNum > UboundFIXs Then
            UboundFIXs = UboundFIXs + 100
            ReDim Preserve StepDownFIXs(UboundFIXs)
        End If

        changetNextButtonMode()

        '================================================================================
        EstimateIAF_AreaObstacles()
        FillComboBox101()
        FillComboBox104()

        If OptionButton101.Checked Then
            OptionButton101_CheckedChanged(OptionButton101, New EventArgs())
        Else
            OptionButton101.Checked = True
        End If

    End Sub

    Private Sub CreateTPPoint()
        StepDownFIXs(StepDownsNum).Counted = 1

        If (StepDownsNum > 0) Then
            StepDownFIXs(StepDownsNum).Name = "TP"
            StepDownFIXs(StepDownsNum).Role = CodeProcedureFixRole.TP
        End If

        StepDownFIXs(StepDownsNum).IntersectNav.Index = -1
        StepDownFIXs(StepDownsNum).pPtPrj.Z = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z

        If StepDownsNum > 0 Then
            StepDownFIXs(StepDownsNum).PDG = StepDownFIXs(StepDownsNum - 1).PDG
        Else
            StepDownFIXs(StepDownsNum).PDG = IAF_PDG
        End If
    End Sub

    Private Sub AddSegmentToReportForm()
        Dim I As Integer

        Dim SegObstList As ObstacleContainer
        Dim HPrevFIX As Double
        HPrevFIX = -BigDist

        For I = StepDownsNum - 1 To 0 Step -1
            If StepDownFIXs(I).TurnDir = 0 Then
                If Not StepDownFIXs(I).pPtPrj Is Nothing Then HPrevFIX = StepDownFIXs(I).pPtPrj.Z
                Exit For
            End If
        Next I

        If (StepDownsNum > 2 And StepDownFIXs(StepDownsNum).Counted < 2) Or (StepDownsNum = 2 And StepDownFIXs(StepDownsNum - 1).Counted < 2 And StepDownFIXs(StepDownsNum).Counted < 2) Or OptionButton101.Checked Then
            FillSegObstList(StepDownFIXs(StepDownsNum - 1), fRefHeight, arAreaHalfWidth, MOCLimit, StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, StepDownFIXs(StepDownsNum - 1).pIAreaPoly, SegObstList)
            StepDownFIXs(StepDownsNum).Obstacles = SegObstList
            InitialReportsFrm.AddSegment(SegObstList, HPrevFIX) ', IAFObstList4Turn
        End If
    End Sub

    Private Sub RemoveSegmentFromReportForm()
        StepDownFIXs(StepDownsNum).Obstacles = New ObstacleContainer()
        InitialReportsFrm.DeleteSegment()
    End Sub

    '---------------------------------------------------------------------------------------
    ' Procedure : InfoBtn_Click
    ' DateTime  : 06.09.2007 15:38
    ' Author    : RuslanA
    ' Purpose   :
    '---------------------------------------------------------------------------------------
    Private Sub InfoBtn_CheckedChanged(sender As Object, e As EventArgs) Handles InfoBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If InfoBtn.Checked Then
            Me.Width += TabInfoFrame.Width
            InfoBtn.Image = bmpHIDE_INFO
        Else
            Me.Width -= TabInfoFrame.Width
            InfoBtn.Image = bmpSHOW_INFO
        End If

        If NextBtn.Enabled Then
            NextBtn.Focus()
        Else
            PrevBtn.Focus()
        End If
    End Sub

    Private Sub OptionButton103_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton103.Click
        TextBox109.ReadOnly = False
        TextBox109.BackColor = SystemColors.Window

        Label130.Enabled = False
        Label131.Enabled = False
        ComboBox103.Enabled = False
    End Sub

    Private Sub OptionButton104_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton104.Click
        TextBox109.ReadOnly = True
        TextBox109.BackColor = SystemColors.Control

        Label130.Enabled = True
        Label131.Enabled = True
        ComboBox103.Enabled = ComboBox103.Items.Count > 0

        If ComboBox103.Enabled Then
            If ComboBox103.SelectedIndex >= 0 Then
                ComboBox103_SelectedIndexChanged(ComboBox103, New EventArgs())
            Else
                ComboBox103.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub OptionButton105_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton105.Click
        TextBox102.Tag = "a"
        TextBox102_Validating(TextBox102, New CancelEventArgs(True))
    End Sub

    Private Sub OptionButton106_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton106.Click
        TextBox102.Tag = "a"
        TextBox102_Validating(TextBox102, New CancelEventArgs(True))
    End Sub

    Private Sub Picture1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        ReportBtn.CheckState = 1 - ReportBtn.CheckState
    End Sub

    Private Function CreateNominalLine(ByRef ForFIX As StepDownFIX) As IPolyline
        If ForFIX.GuidanceNav.TypeCode = eNavaidType.DME Then
            CreateNominalLine = CreateArcPolylinePrj(ForFIX.GuidanceNav.pPtPrj, ForFIX.pPtStart, ForFIX.pPtWarn, ForFIX.ArcDir)
        Else
            CreateNominalLine = New Polyline
            CreateNominalLine.FromPoint = ForFIX.pPtStart
            CreateNominalLine.ToPoint = ForFIX.pPtWarn
        End If
    End Function

    Private Sub RemoveBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles RemoveBtn.Click
        Dim N As Integer
        Dim I As Integer
        Dim onFix As Boolean

        If StepDownsNum < 2 Then Exit Sub

        screenCapture.Delete()

        On Error Resume Next
        If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
        If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
        If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)


        pIAF_IAreaElement = Nothing

        pIAF_IIAreaElement = Nothing

        pNominalElement = Nothing

        If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)

        StepDownFIXs(StepDownsNum).ptElem = Nothing

        StepDownFIXs(StepDownsNum).pFixPolyElem = Nothing

        If Not StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum - 1).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)



        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = Nothing

        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = Nothing

        StepDownFIXs(StepDownsNum - 1).pNominalElem = Nothing

        If Not StepDownFIXs(StepDownsNum - 1).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).ptElem)
        If Not StepDownFIXs(StepDownsNum - 1).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pFixPolyElem)

        '    pGraphics.AddElement StepDownFIXs(StepDownsNum - 1).ptElem, 0
        '    pGraphics.AddElement StepDownFIXs(StepDownsNum - 1).pFixPolyElem, 0


        StepDownFIXs(StepDownsNum - 1).ptElem = Nothing

        StepDownFIXs(StepDownsNum - 1).pFixPolyElem = Nothing

        N = UBound(IAFProhibSectors)
        For I = 0 To N
            If Not IAFProhibSectors(I).ObsAreaElement Is Nothing Then pGraphics.DeleteElement(IAFProhibSectors(I).ObsAreaElement)

            IAFProhibSectors(I).ObsAreaElement = Nothing
        Next I

        If StepDownFIXs(StepDownsNum - 1).Counted = 2 Then
            onFix = True
        End If
        StepDownFIXs(StepDownsNum - 1).Counted = StepDownFIXs(StepDownsNum - 1).Counted - 1

        If StepDownFIXs(StepDownsNum - 1).Counted <= 0 Then
            StepDownFIXs(StepDownsNum - 1).Counted = 0
            If Not StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem)
            If Not StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem)
            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)



            StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem = Nothing
            StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem = Nothing
            StepDownFIXs(StepDownsNum - 2).pNominalElem = Nothing
            StepDownsNum = StepDownsNum - 1

            If (StepDownFIXs(StepDownsNum - 1).PropagateToPrevious = True) Then
                pNominalPoly = CreateNominalLine(StepDownFIXs(StepDownsNum - 2))
                StepDownFIXs(StepDownsNum - 2).pNominalPoly = pNominalPoly
                StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(pNominalPoly, 255, 2)
                StepDownFIXs(StepDownsNum - 1).PropagateToPrevious = False
            End If
        Else

            StepDownFIXs(StepDownsNum - 1).GuidanceNav = StepDownFIXs(StepDownsNum - 2).GuidanceNav
            StepDownFIXs(StepDownsNum - 1).InDir = StepDownFIXs(StepDownsNum - 1).OutDir
            StepDownFIXs(StepDownsNum - 1).TurnDir = 0
            StepDownFIXs(StepDownsNum - 1).TurnDir_A = 0
            StepDownFIXs(StepDownsNum - 1).pPtStart = StepDownFIXs(StepDownsNum - 1).pPtPrj

            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
            StepDownFIXs(StepDownsNum - 2).pPtWarn = StepDownFIXs(StepDownsNum - 1).pPtPrj

            pNominalPoly = CreateNominalLine(StepDownFIXs(StepDownsNum - 2))
            StepDownFIXs(StepDownsNum - 2).pNominalPoly = pNominalPoly

            StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(pNominalPoly, 255, 2)
        End If

        If onFix = False Then
            RemoveSegmentFromReportForm()
        End If


        On Error GoTo 0

        NextBtn.Enabled = StepDownsNum > 0



        CreateIAF_AreaPoly(StepDownFIXs(StepDownsNum - 1).GuidanceNav, StepDownFIXs(StepDownsNum - 1), IAF_IAreaPoly, IAF_IIAreaPoly, pNominalPoly)

        If StepDownsNum = 2 Then
            StepDownFIXs(StepDownsNum - 1).Counted = 0
        End If
        StepDownFIXs(StepDownsNum - 1).pIAreaPoly = IAF_IAreaPoly
        StepDownFIXs(StepDownsNum - 1).pIIAreaPoly = IAF_IIAreaPoly
        StepDownFIXs(StepDownsNum - 1).pNominalPoly = pNominalPoly


        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = DrawPolygon(IAF_IIAreaPoly, RGB(0, 255, 96))
        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem.Locked = True


        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = DrawPolygon(IAF_IAreaPoly, RGB(235, 0, 96))
        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem.Locked = True


        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True

        TextBox110Range.Left = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z
        ToolTip1.SetToolTip(TextBox110, str00210 & str00221 & CStr(ConvertHeight(TextBox110Range.Left, 2)) & " " & HeightConverter(HeightUnit).Unit & str00222 & CStr(ConvertHeight(TextBox110Range.Right, 2)) & " " & HeightConverter(HeightUnit).Unit)

        RemoveBtn.Enabled = StepDownsNum > 2
        changetNextButtonMode()

        EstimateIAF_AreaObstacles()
        FillComboBox101()
        FillComboBox104()

        If StepDownFIXs(StepDownsNum - 1).TurnDir <> 0 Then
            If OptionButton102.Checked Then
                OptionButton102_CheckedChanged(OptionButton102, New EventArgs())
            Else
                OptionButton101.Checked = True
            End If
        Else
            If OptionButton101.Checked Then
                OptionButton101_CheckedChanged(OptionButton101, New EventArgs())
            Else
                OptionButton101.Checked = True
            End If
        End If
    End Sub

    Private Sub changetNextButtonMode()

        NextBtn.Enabled = (StepDownsNum > 2) And (StepDownFIXs(StepDownsNum - 1).TurnDir = 0)
    End Sub

    'Private Sub MultiPage1_Click(PreviousTab As Integer)
    'If MultiPage1.Tag <> "0" Then Exit Sub
    'MultiPage1.Tag = "1"
    'MultiPage1.Tab = PreviousTab
    'If PreviousTab = 1 Then
    '    PrevBtn_Click
    'ElseIf PreviousTab = 0 Then
    '    NextBtn_Click
    'End If
    'MultiPage1.Tag = "0"
    'End Sub

    Private Sub ChangeMode()
        AddBtn.Enabled = True


        If OptionButton101.Checked Then
            Frame104.Text = str40223
        Else
            Frame104.Text = str40224
        End If

        '==================================================
        Frame102.Visible = OptionButton101.Checked
        Label113.Enabled = OptionButton101.Checked
        Label110.Enabled = OptionButton101.Checked
        Label115.Enabled = OptionButton101.Checked

        TextBox103.Enabled = OptionButton101.Checked
        Label114.Enabled = OptionButton101.Checked
        ComboBox104.Enabled = OptionButton101.Checked
        Label111.Enabled = OptionButton101.Checked

        Label112.Enabled = OptionButton101.Checked
        TextBox102.Enabled = OptionButton101.Checked

        Label117.Enabled = OptionButton101.Checked
        TextBox104.Enabled = OptionButton101.Checked

        Label118.Enabled = OptionButton101.Checked
        TextBox105.Enabled = OptionButton101.Checked
        '==================================================
        Frame101.Visible = OptionButton102.Checked

        Label105.Enabled = OptionButton102.Checked
        ComboBox101.Enabled = OptionButton102.Checked
        Label106.Enabled = OptionButton102.Checked

        Label107.Enabled = OptionButton102.Checked
        ComboBox102.Enabled = OptionButton102.Checked

        CkbVirtualDP.Visible = OptionButton102.Checked

        'Label108.Enabled = OptionButton102.Value
        'ComboBox103.Enabled = OptionButton102.Value
        'Label109.Enabled = OptionButton102.Value
    End Sub

    Private Sub OptionButton101_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton101.CheckedChanged
        If eventSender.Checked Then
            Dim I As Integer
            Dim J As Integer
            Dim K As Integer
            Dim L As Integer
            Dim N As Integer
            Dim M As Integer
            Dim fTmp As Double
            Dim Rmin As Double
            Dim Rmax As Double
            Dim dPrec As Double
            Dim LocalMax As Double
            'Dim dDMin As Double
            'Dim bMinMax As Boolean

            Dim FixRange(,) As LowHigh


            ChangeMode()
            CheckBox101.Checked = False
            ComboBox107.Enabled = False

            StepDownFIXs(StepDownsNum).TurnDir = 0
            fFIXHeight = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z

            'TextBox108.Locked = True
            'TextBox108.BackColor = &H8000000F

            '    dPrec = DeConvertDistance(CDbl(TextBox105.Text))

            If IsNumeric(TextBox105.Text) Then
                dPrec = DeConvertDistance(CDbl(TextBox105.Text))
            Else
                dPrec = arIFTolerance.Value
            End If

            On Error Resume Next
            If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
            If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
            If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)
            On Error GoTo 0

            N = UBound(IAFObstList4FIX.Parts)
            If N >= 0 Then
                CalcMaxTurnRangeWFIX(IAFObstList4FIX, FixRange, IAF_PDG) 'arIADescent_Max.Value

                Rmin = Max(IAFObstList4FIX.Parts(0).Rmin, dPrec)
                Rmax = IAFObstList4FIX.Parts(0).Rmax
                'bMinMax = False

                '        If (IAFObstList4FIX.Parts(0).Flags And 2) = 2 Then
                '            dDMin = IAFObstList4FIX.Parts(0).dMin15
                '        Else
                '            dDMin = BigDist
                '        End If

                N = UBound(FixRange, 2)
                fTmp = 0.0#
                For I = 0 To N
                    If FixRange(0, I).Tag > 0 Then
                        If FixRange(0, I).High > fTmp Then fTmp = FixRange(0, I).High
                    End If
                Next I

                If Rmax > fTmp Then
                    Rmax = fTmp
                    'bMinMax = True
                End If

                J = -1
                N = UBound(IAFObstList4FIX.Parts)
                For I = 1 To N
                    If Rmax + 9300.0# < IAFObstList4FIX.Parts(I).Dist Then
                        J = I - 1
                        Exit For
                    End If

                    'If (dDMin > IAFObstList4FIX.Parts(I).dMin15) And ((IAFObstList4FIX.Parts(I).Flags And 2) = 2) Then dDMin = IAFObstList4FIX.Parts(I).dMin15
                    If (Rmin < IAFObstList4FIX.Parts(I).Rmin) And (Rmax > IAFObstList4FIX.Parts(I).Rmin) Then Rmin = IAFObstList4FIX.Parts(I).Rmin

                    K = UBound(FixRange, 2)
                    fTmp = 0.0#
                    For J = 0 To K
                        If FixRange(I, J).Tag > 0 Then
                            If FixRange(I, J).High > fTmp Then fTmp = FixRange(I, J).High
                        End If
                    Next J

                    If Rmax > fTmp Then
                        Rmax = fTmp
                        L = I - 1
                        'bMinMax = True
                    End If
                    If Rmax > IAFObstList4FIX.Parts(I).Rmax Then
                        Rmax = IAFObstList4FIX.Parts(I).Rmax
                        'bMinMax = False
                    End If
                Next I
            Else
                '?????????????????????????????????????????????????????????????
            End If

            If N >= 0 Then
                fTmp = ConvertDistance(IAFObstList4FIX.Parts(0).Rmin, 3)
                Rmin = ConvertDistance(Rmin, 3)
                Rmax = ConvertDistance(Rmax, 1)

                If fTmp <= Rmax Then '        Label103.Caption = "Допустимый интервал: от " + CStr(fTmp) + " до " + CStr(Rmax)
                    Label103.Text = str00210 & str00221 & CStr(fTmp) & str00222 & CStr(Rmax)
                Else
                    Label103.Text = str00210 & str00233
                End If

                If Rmin <= Rmax Then
                    Label104.Text = str00220 & str00221 & CStr(Rmin) & str00222 & CStr(Rmax)
                Else
                    Label104.Text = str00220 & str00233
                    If fTmp > Rmax Then
                        MsgBox(str00516) '            msgbox "Данный маршрут не возможно продолжить в зтом направлении."
                        Exit Sub
                    End If
                End If
            Else
                Label103.Text = str00210 & str00221 & CStr(ConvertDistance(dPrec, 2)) '+ LoadResString(222) + " "
                Label104.Text = str00220 & str00221 & CStr(ConvertDistance(dPrec, 2)) '+ LoadResString(222) + " "

                '       Label103.Caption = "Допустимый интервал: от " + CStr(dPrec) '+ " до " + " " "Допустимый интервал: от " + CStr(Round(IAFObstList4FIX.Parts(0).Rmin + 0.4999)) + " до " + CStr(Round(Rmax - 0.4999))
                '       Label104.Caption = "Рекомендуемый интервал: от " + CStr(dPrec) ' + " до " + " "'"Рекомендуемый интервал: от " + CStr(Round(Rmin + 0.4999)) + " до " + CStr(Round(Rmax - 0.4999))

                Rmin = dPrec
                Rmax = 1.0E+16
            End If

            If TextBox110.Tag = "" Then TextBox110.Tag = "a"
            If ComboBox104.Items.Count = 0 Then
                MsgBox(str00304) '            msgbox "Нет подходящего средства для создания FIX."
                Exit Sub
            End If

            If ComboBox104.SelectedIndex < 0 Then
                ComboBox104.SelectedIndex = 0
            Else
                ComboBox104_SelectedIndexChanged(ComboBox104, New EventArgs())
            End If

        End If
    End Sub

    Private Sub OptionButton102_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptionButton102.CheckedChanged
        If eventSender.Checked Then
            ChangeMode()

            fFIXHeight = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z
            OptionButton103.Checked = True

            If ComboBox101.Items.Count = 0 Then
                Label103.Text = ""
                Label104.Text = ""
                AddBtn.Enabled = False
                MsgBox(str00517)
            ElseIf ComboBox101.SelectedIndex = 0 Then
                ComboBox101_SelectedIndexChanged(ComboBox101, New EventArgs())
            Else
                ComboBox101.SelectedIndex = 0
            End If
            OptionButton103_ClickEvent(OptionButton103, New EventArgs())
        End If
    End Sub

    Private Sub PrevBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles PrevBtn.Click
        NextBtn.Enabled = True
        screenCapture.Delete()

        If MultiPage1.SelectedIndex = 1 Then
            ClearScr()
            InitialReportsFrm.ClearSegments()
            MultiPage1.SelectedIndex = 0
            PrevBtn.Enabled = False
            ReportBtn.Enabled = False

            HelpContextID = 12100
            If Visible Then Activate()
        ElseIf MultiPage1.SelectedIndex = 2 Then

            On Error Resume Next

            If Not ptElem Is Nothing Then pGraphics.DeleteElement(ptElem)
            If Not pPlyElem Is Nothing Then pGraphics.DeleteElement(pPlyElem)
            If Not pReportIAreaElement Is Nothing Then pGraphics.DeleteElement(pReportIAreaElement)
            If Not pReportIIAreaElement Is Nothing Then pGraphics.DeleteElement(pReportIIAreaElement)

            StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, RGB(0, 255, 96))
            StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem.Locked = True


            StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIAreaPoly, RGB(235, 0, 96))
            StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem.Locked = True


            StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, 255, 2)
            StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True

            StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum).pFixPoly, RGB(128, 255, 128))
            StepDownFIXs(StepDownsNum).ptElem = DrawPoint(ptKT1, 0)
            StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
            StepDownFIXs(StepDownsNum).ptElem.Locked = True

            If OptionButton102.Checked Then
                If Not pIAF_IAreaElement Is Nothing Then pGraphics.AddElement(pIAF_IAreaElement, 0)
                If Not pIAF_IIAreaElement Is Nothing Then pGraphics.AddElement(pIAF_IIAreaElement, 0)
                If Not pNominalElement Is Nothing Then pGraphics.AddElement(pNominalElement, 0)
                GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            End If

            On Error GoTo 0
            MultiPage1.SelectedIndex = 1
            ProtRep = Nothing
            AccurRep = Nothing
        End If

        OkBtn.Enabled = False
        SaveReportBtn.Enabled = False

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
    End Sub

    Private Sub ToStep1()
        Dim pClone As IClone

        IAF_PDG = arIADescent_Nom.Value 'arIADescent_Max.Value
        '    IAF_PDG = 7.5
        StepDownsNum = 2
        '================================================================================
        pClone = pFAFptPrj
        StepDownFIXs(0).pPtPrj = pClone.Clone
        StepDownFIXs(0).pPtStart = pClone.Clone


        StepDownFIXs(0).GuidanceNav = StartPoint.GuidanceNav
        StepDownFIXs(0).IntersectNav = StartPoint.IntersectNav
        If StartPoint.GuidanceNav.TypeCode = eNavaidType.DME Then
            StepDownFIXs(0).Track = TrackType.Arc
        Else
            StepDownFIXs(0).Track = TrackType.Straight
        End If

        StepDownFIXs(0).InDir = pIFptPrj.M
        StepDownFIXs(0).OutDir = pIFptPrj.M
        StepDownFIXs(0).TurnDir = 0

        '================================================================================
        pClone = pIFptPrj
        ptKT1 = pClone.Clone

        StepDownFIXs(0).pPtWarn = ptKT1
        StepDownFIXs(1).pPtPrj = ptKT1
        StepDownFIXs(1).pPtStart = ptKT1

        StepDownFIXs(1).Length = Math.Max(ReturnDistanceInMeters(pFAFptPrj, ptKT1), arMinISlen)

        '    If Not GetDefNames(False, True, False, HaveTP, False, _
        ''        ptFAP, Nothing, IFPnt, Nothing, pMAPt, PtSOC, TurnFixPnt, _
        ''        "", FAPName, "", IFName, MAPtName, SOCName, TPName, "") Then
        '        Exit Sub
        '    End If

        StepDownFIXs(1).Name = StartPoint.Name
        'StepDownFIXs(1).Role = StartPoint.GuidanceNav.

        StepDownFIXs(1).PDG = arIADescent_Nom.Value

        StepDownFIXs(1).GuidanceNav = StepDownFIXs(0).GuidanceNav
        StepDownFIXs(1).IntersectNav = StepDownFIXs(0).IntersectNav

        StepDownFIXs(1).InDir = StepDownFIXs(0).InDir
        StepDownFIXs(1).OutDir = StepDownFIXs(0).InDir
        StepDownFIXs(1).TurnDir = 0
        StepDownFIXs(1).Counted = 0

        CreateIAF_AreaPoly(StepDownFIXs(1).GuidanceNav, StepDownFIXs(1), IAF_IAreaPoly, IAF_IIAreaPoly, pNominalPoly)

        StepDownFIXs(1).pIAreaPoly = IAF_IAreaPoly
        StepDownFIXs(1).pIIAreaPoly = IAF_IIAreaPoly
        StepDownFIXs(1).pNominalPoly = pNominalPoly


        StepDownFIXs(1).pIIAreaPolyElem = DrawPolygon(IAF_IIAreaPoly, RGB(0, 255, 96))
        StepDownFIXs(1).pIIAreaPolyElem.Locked = True


        StepDownFIXs(1).pIAreaPolyElem = DrawPolygon(IAF_IAreaPoly, RGB(235, 0, 96))
        StepDownFIXs(1).pIAreaPolyElem.Locked = True


        StepDownFIXs(1).pNominalElem = DrawPolyLine(pNominalPoly, 255, 2)
        StepDownFIXs(1).pNominalElem.Locked = True

        '===============================================
        StepDownFIXs(2).OutDir = StepDownFIXs(1).InDir

        StepDownFIXs(2).GuidanceNav = StepDownFIXs(1).GuidanceNav '?????????????
        '===============================================
        '    TextBox1508_Validate true
        '    Set pGuidPoly = CreateGuidPoly(StepDownFIXs(StepDownsNum - 1).GuidanceNav, StepDownFIXs(StepDownsNum - 1).pPtPrj)
        '    Set pGuidPoly = CreateGuidPoly(StepDownFIXs(StepDownsNum - 1))
        If IsNumeric(TextBox103.Text) Then
            IAF_PDG = 0.01 * CDbl(TextBox103.Text)
            If IAF_PDG > arIADescent_Max.Value Then
                IAF_PDG = arIADescent_Max.Value
                TextBox103.Text = CStr(100.0# * IAF_PDG)
            End If
        Else
            IAF_PDG = arIADescent_Nom.Value
            TextBox103.Text = CStr(100.0# * IAF_PDG)
        End If

        '===============================================
        TextBox110Range.Left = pIFptPrj.Z
        TextBox110Range.Right = pIFptPrj.Z + 100.0#

        EstimateIAF_AreaObstacles()
        FillComboBox101()
        FillComboBox104()

        MultiPage1.Tag = "1"
        MultiPage1.SelectedIndex = 1

        If OptionButton101.Checked Then
            OptionButton101_CheckedChanged(OptionButton101, New EventArgs())
        Else
            OptionButton101.Checked = True
        End If

        ReportBtn.Enabled = True
        NextBtn.Enabled = False

        HelpContextID = 12200

    End Sub

    Private Sub CreateLegs()
        Dim I As Integer
        Dim J As Integer

        'Dim fTmp As Double
        Dim RDME As Double
        Dim dirToDME1 As Double
        Dim dirToDME2 As Double

        Dim pPointcollection1 As IPointCollection
        Dim pPointcollection2 As IPointCollection
        Dim pNominalPolyline As IPolyline

        Dim pPoly As IPolyline
        Dim pClone As IClone


        ReDim LegList(StepDownsNum * 2)
        J = -1

        For I = StepDownsNum - 1 To 1 Step -1
            If (I = 2) And (StepDownFIXs(I - 1).Counted = 2) Then
                pClone = StepDownFIXs(I).pNominalPoly
                pNominalPolyline = pClone.Clone()
                pNominalPolyline.ReverseOrientation()
                pPointcollection1 = pNominalPolyline
                pPointcollection2 = CreateArcPolylinePrj(StepDownFIXs(I).pPtCntr, StepDownFIXs(I).pPtStart, StepDownFIXs(I - 1).pPtWarn, -StepDownFIXs(I).TurnDir_A)
                pPointcollection1.AddPointCollection(pPointcollection2)
                pNominalPolyline = pPointcollection1
                LegList(J).pNominalPoly = pNominalPolyline
            Else
                J += 1
                LegList(J) = StepDownFIXs(I)

                LegList(J).TurnDir = -LegList(J).TurnDir
                LegList(J).TurnDir_A = -LegList(J).TurnDir_A

                LegList(J).ArcDir = StepDownFIXs(I - 1).ArcDir
                LegList(J).GuidanceNav = StepDownFIXs(I - 1).GuidanceNav
                LegList(J).Track = StepDownFIXs(I - 1).Track

                'If I = StepDownsNum Then
                '	If bFirstPointIsIF Then
                '		LegList(J).Role = CodeProcedureFixRole.IAF
                '	Else
                '		LegList(J).Role = CodeProcedureFixRole.SDF
                '	End If
                'End If

                LegList(J).pPtGeo = ToGeo(StepDownFIXs(I).pPtPrj)

                If (I > 1) Then
                    pClone = StepDownFIXs(I - 1).pNominalPoly
                    pPoly = pClone.Clone()
                    pPoly.ReverseOrientation()
                    LegList(J).pNominalPoly = pPoly

                    pClone = StepDownFIXs(I - 1).pIAreaPoly
                    LegList(J).pIAreaPoly = pClone.Clone()

                    pClone = StepDownFIXs(I - 1).pIIAreaPoly
                    LegList(J).pIIAreaPoly = pClone.Clone()

                    'LegList(J).Obstacles = StepDownFIXs(I - 1).Obstacles
                End If

                If StepDownFIXs(I - 1).Track = TrackType.Straight Then
                    LegList(J).Length = ReturnDistanceInMeters(StepDownFIXs(I - 1).pPtPrj, StepDownFIXs(I).pPtPrj)
                Else
                    RDME = ReturnDistanceInMeters(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)
                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtPrj)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)
                    LegList(J).Length = RDME * DegToRad(Modulus((dirToDME2 - dirToDME1) * StepDownFIXs(I - 1).ArcDir, 360))
                End If
            End If

            If I = 1 Then
                'LegList(J).Role = CodeProcedureFixRole.IF
                LegList(J).pNominalPoly = Nothing
                LegList(J).pIAreaPoly = Nothing
                LegList(J).pIAreaPoly = Nothing
            End If
        Next I

        ReDim Preserve LegList(J)
        CreaterLegsCodes()
        CreateLegsFixPoints()
    End Sub

    Private Sub CreateLegsFixPoints()
        Dim I As Integer

        Dim N As Integer = LegList.Length - 1
        Dim currLeg As StepDownFIX
        Dim nextLeg As StepDownFIX
        Dim courseDirection As CodeDirectionReference
        Dim fCourseDir As Double

		For I = 0 To N '- 1
			currLeg = LegList(I)
			nextLeg = Nothing

			If I < N Then
				nextLeg = LegList(I + 1)
			End If

			If currLeg.GuidanceNav.TypeCode = eNavaidType.DME Then
				If SideDef(currLeg.pPtPrj, currLeg.OutDir + 90.0, currLeg.GuidanceNav.pPtPrj) < 0 Then
					courseDirection = CodeDirectionReference.OTHER_CW
				Else
					courseDirection = CodeDirectionReference.OTHER_CCW
				End If
			Else
				fCourseDir = currLeg.OutDir
				If SideDef(currLeg.pPtPrj, fCourseDir + 90.0, currLeg.GuidanceNav.pPtPrj) < 0 Then
					courseDirection = CodeDirectionReference.FROM
				Else
					courseDirection = CodeDirectionReference.TO
				End If
			End If

			If I > 0 Then
				currLeg.FixStartPoint = LegList(I - 1).FixEndPoint
			Else
				If currLeg.GuidanceNav.TypeCode = eNavaidType.DME Then
					fCourseDir = ReturnAngleInDegrees(currLeg.GuidanceNav.pPtPrj, currLeg.pPtPrj) +
								 90 * (1 + 2 * CLng(courseDirection = CodeDirectionReference.OTHER_CW))

				End If

				If currLeg.IntersectNav.IntersectionType = eIntersectionType.OnNavaid Or ReturnDistanceInMeters(currLeg.pPtPrj, currLeg.IntersectNav.pPtPrj) < distError Then
					currLeg.FixStartPoint.OnNavaid = True
				End If

				If currLeg.FixStartPoint.OnNavaid = True Then
					SetFixPointValues(currLeg.FixStartPoint, currLeg.IntersectNav)
				Else
					If currLeg.WPT.Identifier = Guid.Empty Then
						Dim wpt As WPT_FIXType
						Dim bExist As Boolean = FindDesignatedPoint(currLeg.pPtPrj, wpt, fCourseDir)
						If bExist Then
							SetFixPointValues(currLeg.FixStartPoint, wpt)
						Else
							SetFixPointValues(currLeg.FixEndPoint, currLeg.pPtPrj)
						End If
					Else
						SetFixPointValues(currLeg.FixStartPoint, currLeg.WPT, True)
					End If
				End If
			End If

			If currLeg.LegTypeARINC <> CodeSegmentPath.CI Then
				If I < N Then
					If currLeg.GuidanceNav.TypeCode = eNavaidType.DME Then
						fCourseDir = ReturnAngleInDegrees(currLeg.GuidanceNav.pPtPrj, nextLeg.pPtPrj) +
									 90 * (1 + 2 * CLng(courseDirection = CodeDirectionReference.OTHER_CW))     'Azt2DirPrj(ptStart, pSegmentLeg.Course)
					End If

					If nextLeg.IntersectNav.Identifier <> Guid.Empty Then
						If nextLeg.IntersectNav.IntersectionType = eIntersectionType.OnNavaid Or ReturnDistanceInMeters(nextLeg.pPtPrj, nextLeg.IntersectNav.pPtPrj) < distError Then
							currLeg.FixEndPoint.OnNavaid = True
						End If

						If currLeg.FixEndPoint.OnNavaid = True Then
							SetFixPointValues(currLeg.FixEndPoint, nextLeg.IntersectNav)
						End If
					End If

					If Not currLeg.FixEndPoint.OnNavaid Then
						If nextLeg.WPT.Identifier = Guid.Empty Then
							Dim wpt As WPT_FIXType
							Dim bExist As Boolean = FindDesignatedPoint(nextLeg.pPtPrj, wpt, fCourseDir)
							If bExist Then
								SetFixPointValues(currLeg.FixEndPoint, wpt)
							Else
								SetFixPointValues(currLeg.FixEndPoint, nextLeg.pPtPrj)
							End If
						Else
							SetFixPointValues(currLeg.FixEndPoint, nextLeg.WPT, True)
						End If
					End If
				End If
			End If
			LegList(I).FixStartPoint = currLeg.FixStartPoint
			LegList(I).FixEndPoint = currLeg.FixEndPoint
		Next I
	End Sub

    Private Sub SetFixPointValues(ByRef Point As StepDownFIXPoint, ByVal WPT As WPT_FIXType, Optional ByVal Snaped As Boolean = False)
        Point.Name = WPT.Name
        Point.Created = True
        Point.Inited = True
        Point.Identifier = WPT.Identifier
        Point.NAV_Ident = WPT.NAV_Ident

        'Point.Feature = WPT.pFeature
        SetGeo(Point, WPT.pPtPrj)
        Point.Snaped = Snaped
    End Sub

    Private Sub SetFixPointValues(ByRef Point As StepDownFIXPoint, ByVal pPrj As IPoint)
        Point.Name = "COORD"
        Point.Inited = True
        SetGeo(Point, pPrj)
    End Sub

    Private Sub SetFixPointValues(ByRef Point As StepDownFIXPoint, ByVal Nav As NavaidData, Optional ByVal Snaped As Boolean = False)
        Point.Name = Nav.CallSign
        Point.Created = True
        Point.Inited = True
        Point.Identifier = Nav.Identifier
        Point.NAV_Ident = Nav.NAV_Ident

        'Point.Feature = Nav.pFeature
        SetGeo(Point, Nav.pPtPrj)
        Point.Snaped = Snaped
    End Sub

    Private Function SetGeo(ByRef Point As StepDownFIXPoint, pt As IPoint) As StepDownFIXPoint
        Dim pPoint As IPoint
        pPoint = ToGeo(pt)
        Point.X = pPoint.X
        Point.Y = pPoint.Y
        Return Point
    End Function

    Private Sub CreaterLegsCodes()
        Dim I As Integer
        Dim N As Integer = LegList.Length - 1

        For I = 0 To N - 1
            If LegList(I).GuidanceNav.TypeCode = eNavaidType.DME Then
                LegList(I).LegTypeARINC = CodeSegmentPath.AF
            Else
                If (I < N And LegList(I + 1).Role = CodeProcedureFixRole.TP) Then
                    LegList(I).LegTypeARINC = CodeSegmentPath.CI
                ElseIf (LegList(I).Role = CodeProcedureFixRole.SDF) Then
                    LegList(I).LegTypeARINC = CodeSegmentPath.TF
                ElseIf LegList(I).GuidanceNav.TypeCode = eNavaidType.NDB Then
                    LegList(I).LegTypeARINC = CodeSegmentPath.VI
                Else
                    LegList(I).LegTypeARINC = CodeSegmentPath.CF
                    If (I > 0) Then
                        If (LegList(I - 1).LegTypeARINC = CodeSegmentPath.AF) Then
                            LegList(I).LegTypeARINC = CodeSegmentPath.TF
                        End If
                    End If
                End If
            End If

        Next I
		'LegList(N).LegTypeARINC = CodeSegmentPath.IF
	End Sub

    Private Sub ToStep2()

        CleanGraphics()
        'AddSegmentToReportForm()
        CreateLegs()
        CalculateLegsReports()
        FillTransitionCoding()

        MultiPage1.Tag = "2"
        MultiPage1.SelectedIndex = 2

        HelpContextID = 12200

        NextBtn.Enabled = False
        OkBtn.Enabled = True
        SaveReportBtn.Enabled = True
    End Sub

    Private Sub CleanGraphics()
        Dim I As Integer
        Dim N As Integer

        On Error Resume Next
        If Not StepDownFIXs(StepDownsNum).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pIIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pNominalElem)
        If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)

        If Not StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
        If Not StepDownFIXs(StepDownsNum - 1).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)

        If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
        If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
        If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)

        GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        N = UBound(IAFProhibSectors)
        For I = 0 To N
            If Not IAFProhibSectors(I).ObsAreaElement Is Nothing Then pGraphics.DeleteElement(IAFProhibSectors(I).ObsAreaElement)

            IAFProhibSectors(I).ObsAreaElement = Nothing
        Next I
        On Error GoTo 0
    End Sub

    Private Sub FillTransitionCoding()
        Dim N As Integer
        Dim I As Integer

        Dim itmX As ListViewItem

        ListView201.Items.Clear()

        N = LegList.Length - 1

        For I = 0 To N - 1
            itmX = ListView201.Items.Add(LegList(I).Report.LegType)
            itmX.SubItems.Add(LegList(I + 1).Report.Role)

            itmX.SubItems.Add(LegList(I).Report.FullCourse)
            itmX.SubItems.Add(LegList(I).Report.Distance)
            itmX.SubItems.Add(LegList(I).Report.TurnAngle)
            itmX.SubItems.Add(LegList(I).Report.GuidNav)
            itmX.SubItems.Add(LegList(I).Report.InDir)
            itmX.SubItems.Add(LegList(I).Report.OutDir)
            itmX.SubItems.Add(LegList(I).Report.MinAltitude)
            itmX.SubItems.Add(LegList(I).Report.MaxAltitude)
        Next I
    End Sub

    Private Sub CalculateLegsReports()
        Dim N As Integer
        Dim I As Integer

        N = LegList.Length - 1
        For I = 0 To N - 1
            LegList(I).Report = CalculateLegReport(LegList(I), LegList(I + 1))
        Next I
        LegList(N).Report = CalculateLastLegReport(LegList(N))
    End Sub

    Private Function CalculateLastLegReport(ByRef Leg As StepDownFIX) As StepDownFIXReport
        Dim fTmp As Double
        Dim Report As StepDownFIXReport = New StepDownFIXReport()

        Report.LegType = Leg.LegTypeARINC.ToString()
        Report.Role = Leg.Role.ToString()

        Leg.MinAlt = Leg.pPtPrj.Z
        Leg.MaxAlt = Leg.pPtPrj.Z

        fTmp = ConvertReportHeight(Leg.MaxAlt, eRoundMode.NEAREST)
        Report.MaxAltitude = Str(Math.Round(fTmp, 3))
        Report.MinAltitude = Report.MaxAltitude
        Report.Altitude = Report.MaxAltitude
        Report.AltitudeM = Str(Leg.MaxAlt)

        Report.WayPoint = Leg.FixEndPoint.Name

        If (Leg.FixEndPoint.Inited) Then
            SetReportsGeoCoord(Report, Leg.FixEndPoint)
        End If

        Report.FlyOver = "---"
        Report.MagVariation =
        Report.Speed = ""
        Report.Course = ""
        Report.DistanceKM = ""
        Report.Distance = ""
        Report.TurnDirection = ""

        Return Report
    End Function

    Private Function CalculateFirstLegReport(ByRef Leg As StepDownFIX) As StepDownFIXReport
        Dim fTmp As Double
        Dim Report As StepDownFIXReport = New StepDownFIXReport()

        Report.LegType = CodeSegmentPath.IF.ToString()
        Report.Role = Leg.Role.ToString()
        Report.GuidNav = Leg.GuidanceNav.CallSign & "/" & GetNavTypeName(Leg.GuidanceNav.TypeCode)
        SetLegsReportMagVar(Report, Leg)

        Report.MinAltitude = ""
        fTmp = ConvertReportHeight(Leg.pPtPrj.Z, eRoundMode.NEAREST)
        Report.MaxAltitude = Str(Math.Round(fTmp, 3))
        Report.MinAltitude = Report.MaxAltitude
        Report.Altitude = Report.MaxAltitude
        Report.AltitudeM = Str(Leg.pPtPrj.Z)

        Report.WayPoint = Leg.FixStartPoint.Name

        If (Leg.FixStartPoint.Inited) Then
            SetReportsGeoCoord(Report, Leg.FixStartPoint)
        End If

        Report.FlyOver = "---"
        Report.Speed = ""
        Report.Course = ""
        Report.DistanceKM = ""
        Report.Distance = ""
        Report.TurnDirection = ""

        Return Report
    End Function

    Private Function CalculateLegReport(ByRef Leg As StepDownFIX, ByRef NextLeg As StepDownFIX) As StepDownFIXReport
        Dim fTmp As Double
        Dim Report As StepDownFIXReport = New StepDownFIXReport()
        Report.LegType = Leg.LegTypeARINC.ToString()
        Report.Role = Leg.Role.ToString()
        Report.GuidNav = Leg.GuidanceNav.CallSign & "/" & GetNavTypeName(Leg.GuidanceNav.TypeCode)

        If Leg.Track = TrackType.Straight Then
            fTmp = Dir2Azt(Leg.pPtPrj, Leg.OutDir)
            Report.Course = CStr(Math.Round(fTmp, 1))
            Report.FullCourse = Report.Course
        Else
            fTmp = Dir2Azt(Leg.GuidanceNav.pPtPrj, ReturnAngleInDegrees(Leg.GuidanceNav.pPtPrj, Leg.pPtPrj))
            Report.Course = CStr(Math.Round(fTmp, 1))
            Report.FullCourse = Report.Course & " (Boundary course)"
        End If
        Report.Course = CStr(Math.Round(fTmp, 1) - Leg.GuidanceNav.MagVar) + " (" + Report.Course + ")"
        SetLegsReportMagVar(Report, Leg)

        Dim distance = ConvertReportDistance(Leg.Length, eRoundMode.NEAREST)
        Report.Distance = CStr(distance)
        Report.DistanceKM = CStr(Math.Round(Leg.Length / 1000, 1))

        If Leg.TurnDir_A <> 0 Then
            Report.TurnAngle = CStr(Math.Round(Leg.TurnAngle, 1))
        Else
            Report.TurnAngle = ""
        End If

        If Leg.TurnDir_A = 1 Then
            Report.TurnDirection = "L"
        ElseIf Leg.TurnDir_A = -1 Then
            Report.TurnDirection = "R"
        Else
            Report.TurnDirection = ""
        End If

        fTmp = Dir2Azt(Leg.pPtPrj, Leg.InDir)
        Report.InDir = Str(Math.Round(fTmp, 1))

        fTmp = Dir2Azt(Leg.pPtPrj, Leg.OutDir)
        Report.OutDir = Str(Math.Round(fTmp, 1))

        Leg.MinAlt = NextLeg.pPtPrj.Z
        fTmp = ConvertReportHeight(Leg.MinAlt, 2)
        Report.MinAltitude = Str(Math.Round(fTmp, 3))

        Leg.MaxAlt = Leg.pPtPrj.Z
        fTmp = ConvertReportHeight(Leg.MaxAlt, 2)
        Report.MaxAltitude = Str(Math.Round(fTmp, 3))

        Report.Altitude = Report.MinAltitude
        Report.AltitudeM = Str(Leg.MinAlt)

        Report.WayPoint = Leg.FixEndPoint.Name

        If (Leg.FixEndPoint.Inited) Then
            SetReportsGeoCoord(Report, Leg.FixEndPoint)
        End If

        Report.FlyOver = "---"
        Report.Speed = ""
        Return Report
    End Function

    Private Sub SetLegsReportMagVar(ByRef Report As StepDownFIXReport, Leg As StepDownFIX)
        Report.MagVariation = Leg.GuidanceNav.MagVar.ToString() + " " + Report.GuidNav
        If (Leg.GuidanceNav.MagVar > 0) Then
            Report.MagVariation = "+" + Report.MagVariation
        Else
            Report.MagVariation = "-" + Report.MagVariation
        End If
    End Sub

    Private Sub SetReportsGeoCoord(ByRef Report As StepDownFIXReport, Point As StepDownFIXPoint)
        Report.Latitude = DegreeToString(Point.Y, Degree2StringMode.DMSLat)
        Report.Longitude = DegreeToString(Point.X, Degree2StringMode.DMSLon)
    End Sub

    Private Sub NextBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles NextBtn.Click
        screenCapture.Save(Me)

        If MultiPage1.SelectedIndex = 0 Then
            If (StartPoint.GuidanceNav.CallSign = "") Or (StartPoint.IntersectNav.CallSign = "") Then
                screenCapture.Delete()
                Exit Sub
            End If

            ToStep1()
        ElseIf MultiPage1.SelectedIndex = 1 Then
            If ComboBox104.Items.Count = 0 Then
                MessageBox.Show(str00514)
                Return
            End If

            ToStep2()
        End If

        PrevBtn.Enabled = True
        MultiPage1.Tag = "0"

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))

        Me.Visible = False
        Me.Show(_Win32Window)
    End Sub

    Private Sub OkBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OkBtn.Click
		screenCapture.Save(Me)

		If ProtRep Is Nothing Then
			SaveReports()
		End If

		CreateProcedure()
		Try
			If SaveProcedure() Then
				saveReportToDB()
				saveScreenshotToDB()
				Me.Close()
			Else
				screenCapture.Delete()
			End If
		Catch ex As Exception
			gAranEnv.GetLogger("INITIAL APPROACH").Error(ex, "Ok button click")
            Throw
        End Try

    End Sub

	'Private Sub SaveLog(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader) 'PrecisionForm
	'Dim LogRep As New ReportFile
	'Dim I As Long
	'Dim J As Long
	'Dim N As Long
	'Dim itmX As ListItem
	'Dim fOCH As Double
	'Dim fOCA As Double
	'Dim fTmp As Double
	'Dim sTmp As String

	'          Set LogRep.ThrPtPrj = ptLHPrj
	'          LogRep.RefHeight = ptLHPrj.Z

	'LogRep.OpenFile RepFileName + "_Log", LoadResString(817)
	''=============================================================================
	'LogRep.WriteHeader pReport

	'	LogRep.Out "<TABLE border=1 style= ""font-family: Arial, Sans-Serif; font-size:12"">"
	'	LogRep.Out "<TD><CAPTION><STRONG>Publication data</STRONG></CAPTION></TD>"
	'	LogRep.Out "<TBODY>"
	'	LogRep.Out "<TR align=center><TD><b>Initial approach</b></TD></TR>"
	'	LogRep.Out "<TR><TD>"

	'	If CheckBox0301.Value = 0 Then
	'	fOCH = DeConvertHeight(CDbl(TextBox0303.Text))
	'Else
	'	fOCH = DeConvertHeight(CDbl(TextBox0906.Text))
	'End If

	'fOCA = fOCH + ptLHPrj.Z

	''=============================================================================
	'sTmp = CStr(ConvertHeight(fOCA, 2)) + " " + HeightConverter(HeightUnit).Unit
	'LogRep.WriteString "OCA calculated  - " + sTmp '(расчетная)

	'fTmp = ConvertHeight(fOCA, 0)
	'sTmp = CStr(Round(fTmp + 0.499999)) + " " + HeightConverter(HeightUnit).Unit

	'LogRep.WriteString "OCA rounded  - " + sTmp '(округленная по правилам PANS-OPS)
	'LogRep.WriteString "OCA for publication - " + sTmp
	'	LogRep.WriteString()
	''=============================================================================
	'sTmp = CStr(ConvertHeight(ptLHPrj.Z, 2)) + " " + HeightConverter(HeightUnit).Unit
	'LogRep.WriteString "OCH reference point  - " + sTmp '(уровень отчета OCH)

	'sTmp = CStr(ConvertHeight(fOCH, 2)) + " " + HeightConverter(HeightUnit).Unit
	'LogRep.WriteString "OCH calculated  - " + sTmp '(расчетная)

	'fTmp = ConvertHeight(fOCH, 0)

	'sTmp = CStr(Round(fTmp + 0.499999)) + " " + HeightConverter(HeightUnit).Unit
	'LogRep.WriteString "OCH rounded  - " + sTmp '(округленная по правилам PANS-OPS)

	'LogRep.WriteString "OCH for publication - " + sTmp
	'	LogRep.WriteString "Missed approach climb gradient - " + ComboBox0005.Text + " %"

	'	LogRep.Out "</TD></TR></TBODY></TABLE>"

	'	LogRep.WriteString()
	'LogRep.WriteString()
	''=============================================================================
	'LogRep.WriteMessage LoadResString(2) + " - " + LoadResString(817)
	'	LogRep.WriteMessage()
	'LogRep.WriteMessage RepFileTitle
	'	LogRep.WriteParam LoadResString(813), CStr(Date) + " - " + CStr(Time)

	'	LogRep.WriteMessage()
	'LogRep.WriteMessage()

	''===========================================================================
	'LogRep.WriteMessage "[ " + MultiPage1.TabCaption(0) + " ]"
	''===========================================================================
	'LogRep.WriteMessage "[ " + MultiPage1.TabCaption(1) + " ]"
	''===========================================================================
	'LogRep.WriteMessage "[ " + MultiPage1.TabCaption(2) + " ]"
	''===========================================================================
	'LogRep.WriteMessage "[ " + MultiPage1.TabCaption(3) + " ]"
	''===========================================================================
	'LogRep.CloseFile()
	'          Set LogRep = Nothing
	'End Sub

	Private Function ConvertTracToPoints(ByRef TrackPoints() As ReportPoint) As Double
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer

        Dim pPt As IPoint
        Dim pPtPrj As IPoint

        N = UBound(LegList)
        ReDim TrackPoints(2 * N)

        ConvertTracToPoints = 0.0#
        J = -1
        M = 0
        I = 0

        While I <= N
            J = J + 1

            TrackPoints(J).Description = LegList(I).Role

            If LegList(I).pPtGeo Is Nothing Then LegList(I).pPtGeo = ToGeo(LegList(I).pPtPrj)

            TrackPoints(J).Lat = DegreeToString((LegList(I).pPtGeo.Y), Degree2StringMode.DMSLat)
            TrackPoints(J).Lon = DegreeToString((LegList(I).pPtGeo.X), Degree2StringMode.DMSLon)

            TrackPoints(J).Direction = CDbl(CStr(Math.Round(Dir2Azt(LegList(I).pPtPrj, LegList(I).OutDir), 2)))

            TrackPoints(J).PDG = LegList(I).PDG
            TrackPoints(J).Altitude = LegList(I).MinAlt 'CStr(Round(pPtPrj.Z))
            TrackPoints(J).Turn = LegList(I).TurnDir

            If LegList(I).Role = CodeProcedureFixRole.OTHER_WPT Then
                TrackPoints(J).Radius = NO_DATA_VALUE
                TrackPoints(J).CenterLat = ""
                TrackPoints(J).CenterLon = ""
                '               TrackPoints(J).Description = "TP"
                ''            Set ptConstr = pPt
                ''            ptConstr.ConstructAngleIntersection PtEOL, DegToRad(PtEOL.M + 90#), FarFAF, DegToRad(FarFAF.M + 90#)
                ''            TrackPoints(J).Radius = ReturnDistanceInMeters(pPt, PtEOL)
                '               FillTurnParams MPtCollection.Point(I - 1), MPtCollection.Point(I), TrackPoints(J)

                M = M + 1
                TrackPoints(J).Description = str00521 & CStr(M) & str00523

                TrackPoints(J).TurnAngle = LegList(I).TurnAngle
                TrackPoints(J).TurnArcLen = LegList(I).Length

                TrackPoints(J).ToNext = LegList(I).Length
                ConvertTracToPoints = ConvertTracToPoints + TrackPoints(J).ToNext


                pPtPrj = LegList(I).pNominalPoly.ToPoint

                pPt = ToGeo(pPtPrj)
                If I < N Then
                    If LegList(I + 1).ArcDir <> 0 Then
                        J = J + 1
                        I = I + 1

                        TrackPoints(J).Lat = DegreeToString((pPt.Y), Degree2StringMode.DMSLat)
                        TrackPoints(J).Lon = DegreeToString((pPt.X), Degree2StringMode.DMSLon)

                        TrackPoints(J).Direction = CDbl(CStr(Math.Round(Dir2Azt(LegList(I).pPtPrj, LegList(I).OutDir), 2)))

                        TrackPoints(J).PDG = LegList(I).PDG
                        TrackPoints(J).Altitude = LegList(I).MinAlt 'CStr(Round(pPtPrj.Z))
                        TrackPoints(J).Turn = LegList(I).TurnDir

                        TrackPoints(J).Radius = NO_DATA_VALUE
                        TrackPoints(J).CenterLat = ""
                        TrackPoints(J).CenterLon = ""

                        TrackPoints(J).Description = str00522 & CStr(M) & str00523
                        TrackPoints(J).Direction = CDbl(CStr(Math.Round(Dir2Azt(LegList(I).pPtPrj, LegList(I).OutDir), 2)))
                    End If
                End If
            ElseIf LegList(I).ArcDir <> 0 Then

                TrackPoints(J).Radius = NO_DATA_VALUE
                TrackPoints(J).CenterLat = ""
                TrackPoints(J).CenterLon = ""

                M = M + 1
                TrackPoints(J).Description = str00521 & CStr(M) & str00523

                TrackPoints(J).TurnAngle = LegList(I).TurnAngle
                TrackPoints(J).TurnArcLen = LegList(I).Length

                TrackPoints(J).ToNext = LegList(I).Length
                ConvertTracToPoints = ConvertTracToPoints + TrackPoints(J).ToNext
            Else

                TrackPoints(J).Radius = NO_DATA_VALUE
                TrackPoints(J).CenterLat = ""
                TrackPoints(J).CenterLon = ""
                If I <> 0 Then TrackPoints(J).Description = "SDF"
            End If

            If I < N Then
                TrackPoints(J).ToNext = LegList(I).Length
                ConvertTracToPoints = ConvertTracToPoints + TrackPoints(J).ToNext
            End If

            I = I + 1
        End While

        If J > 0 Then
            ReDim Preserve TrackPoints(J)
        Else

            ReDim TrackPoints(-1)
        End If
    End Function

    Private Sub SaveGeometry(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim I As Integer
        Dim TraceLen As Double
        Dim GeomRep As New ReportFile
        Dim TrackPoints() As ReportPoint

        TraceLen = ConvertTracToPoints(TrackPoints)

        'GeomRep.ThrPtPrj = pFAFptPrj
        'GeomRep.RefHeight = fRefHeight

        GeomRep.OpenFile(RepFileName + "_Geometry", str00811)

        GeomRep.WriteMessage(str00003 + " - " + str00811)
        'GeomRep.WriteMessage()
        'GeomRep.WriteMessage(RepFileTitle)
        'GeomRep.WriteParam(My.Resources.str813, CStr(Today) + " - " + CStr(TimeOfDay))
        GeomRep.WriteHeader(pReport)

        GeomRep.WriteMessage()
        GeomRep.WriteMessage()

        For I = 0 To UBound(TrackPoints)
            GeomRep.WritePoint(TrackPoints(I))
        Next I
        GeomRep.WriteMessage()

        GeomRep.Param(str00852, CStr(ConvertDistance(TraceLen, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit) '+ " / " + CStr(Round(TraceLen * NMCoeff, 1)), My.Resources.str19 + "/" + My.Resources.str20

        GeomRep.CloseFile()
        GeomRep = Nothing
    End Sub

    Private Sub TextBox102_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox102.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox102_Validating(TextBox102, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox102.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox102_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox102.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        ValidateDMEDistance(OptionButton105.Enabled And OptionButton105.Checked)
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub ValidateDMEDistance(beforeNavaid As Boolean)

        Dim pPolyClone As IPointCollection
        Dim pSect0 As IPointCollection
        Dim pSect1 As IPointCollection
        Dim pLine As IPointCollection
        Dim pPoly As IPointCollection

        Dim pTopo As ITopologicalOperator2
        Dim pConstruct As IConstructPoint
        Dim pProxi As IProximityOperator
        Dim pCutter As IPolyline

        Dim Clone As IClone
        Dim ptTmp As IPoint
        Dim pt1 As IPoint
        Dim pt2 As IPoint

        Dim InterToler As Double
        Dim LegLength As Double
        Dim fTmpDir0 As Double
        Dim fTmpDir1 As Double
        Dim fDirl As Double
        Dim InDir As Double
        Dim hFix As Double
        Dim fTmp As Double
        Dim dFar As Double
        Dim fDis As Double
        Dim hDis As Double
        Dim dMin As Double
        Dim dMax As Double

        Dim d0 As Double
        Dim dD As Double
        Dim D As Double

        Dim Side1 As Integer
        Dim Side As Integer
        Dim I As Integer
        Dim N As Integer
        Dim K As Integer


        Dim InterNav As NavaidData
        Dim NextFIX As StepDownFIX
        Dim GuidNav As NavaidData
        Dim pFixPoly As IPolygon

        On Error Resume Next
        If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)
        On Error GoTo 0

        K = ComboBox104.SelectedIndex
        If K < 0 Then Return

        InterNav = ComboBox104.SelectedItem

        If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
            TextBox102.Visible = True
            If TextBox102.Tag = TextBox102.Text Then Return
            If Not IsNumeric(TextBox102.Text) Then Return
            fDirl = CDbl(TextBox102.Text)
        End If


        NextFIX = StepDownFIXs(StepDownsNum - 1)

        GuidNav = NextFIX.GuidanceNav
        InDir = NextFIX.InDir

        If InterNav.IntersectionType = eIntersectionType.OnNavaid Then
            TextBox102.Visible = False

            Clone = InterNav.pPtPrj
            ptKT1 = Clone.Clone

            If InterNav.TypeCode = eNavaidType.NDB Then
                NDBFIXTolerArea(InterNav.pPtPrj, InDir, fFIXHeight, pSect0)
            Else
                VORFIXTolerArea(InterNav.pPtPrj, InDir, fFIXHeight, pSect0)
            End If
        End If

        If (GuidNav.TypeCode = eNavaidType.DME) Then 'DME
            If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
                fDirl = fDirl + InterNav.MagVar

                If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                    Label112.Text = str00227
                    InterToler = VOR.IntersectingTolerance '                fDis = VOR.Range
                ElseIf (InterNav.TypeCode = eNavaidType.NDB) Then
                    Label112.Text = str00228
                    InterToler = NDB.IntersectingTolerance '                fDis = NDB.Range
                    fDirl = fDirl - 180.0#
                ElseIf (InterNav.TypeCode = eNavaidType.LLZ) Then
                    Label112.Text = str00227
                    InterToler = LLZ.IntersectingTolerance
                End If

                fTmp = fDirl
                dMin = Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))
                dMax = Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))

                If Not AngleInSector(fDirl, dMin, dMax) Then
                    If SubtractAngles(fDirl, dMin) < SubtractAngles(fDirl, dMax) Then
                        fDirl = dMin
                    Else
                        fDirl = dMax
                    End If
                End If

                If SubtractAngles(fTmp, fDirl) > degEps Then
                    If InterNav.TypeCode = eNavaidType.NDB Then
                        TextBox102.Text = CStr(Math.Round(Modulus(fDirl + 180.0# - InterNav.MagVar), 1))
                    Else
                        TextBox102.Text = CStr(Math.Round(Modulus(fDirl - InterNav.MagVar), 1))
                    End If
                End If

                fDirl = Azt2Dir(InterNav.pPtGeo, fDirl)
                fDis = ReturnDistanceInMeters(GuidNav.pPtPrj, InterNav.pPtPrj)
                D = ReturnDistanceInMeters(GuidNav.pPtPrj, NextFIX.pPtPrj)
                If fDis < D Then fDis = InterNav.Range

                pSect0 = New ESRI.ArcGIS.Geometry.Polygon
                pt1 = PointAlongPlane(InterNav.pPtPrj, fDirl + InterToler, fDis + 2.0# * D)
                pt2 = PointAlongPlane(InterNav.pPtPrj, fDirl - InterToler, fDis + 2.0# * D)

                pSect0.AddPoint(InterNav.pPtPrj)
                pSect0.AddPoint(pt2)
                pSect0.AddPoint(pt1)
                pSect0.AddPoint(InterNav.pPtPrj)

                pTopo = pSect0
                pTopo.IsKnownSimple_2 = False

                pTopo.Simplify()

                pCutter = New Polyline


                pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fDirl + 90.0#, D + D)


                pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fDirl - 90.0#, D + D)

                If (InterNav.ValCnt And 1) = 0 Then
                    CircleVectorIntersect(GuidNav.pPtPrj, D, InterNav.pPtPrj, fDirl, ptKT1)

                    If InterNav.ValCnt <> 0 Then pTopo.Cut(pCutter, pSect0, pSect1)
                Else
                    CircleVectorIntersect(GuidNav.pPtPrj, D, InterNav.pPtPrj, fDirl + 180.0#, ptKT1)

                    pTopo.Cut(pCutter, pSect1, pSect0)
                End If
            End If

            fTmpDir0 = ReturnAngleInDegrees(InterNav.pPtPrj, ptKT1)
            StepDownFIXs(StepDownsNum).OutDir = fTmpDir0 - 90.0# * NextFIX.ArcDir
            StepDownFIXs(StepDownsNum).ArcDir = NextFIX.ArcDir

            fTmpDir1 = ReturnAngleInDegrees(InterNav.pPtPrj, NextFIX.pPtPrj)
            LegLength = ReturnDistanceInMeters(InterNav.pPtPrj, NextFIX.pPtPrj) * DegToRad(Modulus(NextFIX.ArcDir * (fTmpDir0 - fTmpDir1)))
        Else 'VOR, NDB
            Select Case InterNav.IntersectionType
                Case eIntersectionType.ByAngle
                    fDirl = fDirl + InterNav.MagVar
                    If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                        Label112.Text = str00227
                        InterToler = VOR.IntersectingTolerance '                fDis = VOR.Range
                    ElseIf (InterNav.TypeCode = eNavaidType.NDB) Then
                        Label112.Text = str00228
                        InterToler = NDB.IntersectingTolerance '                fDis = NDB.Range
                        fDirl = fDirl - 180.0#
                    ElseIf (InterNav.TypeCode = eNavaidType.LLZ) Then
                        Label112.Text = str00227
                        InterToler = LLZ.IntersectingTolerance
                    End If

                    fTmp = fDirl
                    dMin = Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))
                    dMax = Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))

                    If Not AngleInSector(fDirl, dMin, dMax) Then
                        If SubtractAngles(fDirl, dMin) < SubtractAngles(fDirl, dMax) Then
                            fDirl = dMin
                        Else
                            fDirl = dMax
                        End If
                    End If

                    If SubtractAngles(fTmp, fDirl) > degEps Then
                        If InterNav.TypeCode = eNavaidType.NDB Then
                            TextBox102.Text = CStr(Math.Round(Modulus(fDirl + 180.0# - InterNav.MagVar), 1))
                        Else
                            TextBox102.Text = CStr(Math.Round(Modulus(fDirl - InterNav.MagVar), 1))
                        End If
                    End If

                    fDirl = Azt2Dir(InterNav.pPtGeo, fDirl)
                    ptKT1 = New ESRI.ArcGIS.Geometry.Point
                    pConstruct = ptKT1

                    pConstruct.ConstructAngleIntersection(InterNav.pPtPrj, DegToRad(fDirl), NextFIX.pPtPrj, DegToRad(InDir))

                    fDis = InterNav.Range

                    pSect0 = New ESRI.ArcGIS.Geometry.Polygon
                    pt1 = PointAlongPlane(InterNav.pPtPrj, fDirl + InterToler, fDis)
                    pt2 = PointAlongPlane(InterNav.pPtPrj, fDirl - InterToler, fDis)

                    pSect0.AddPoint(InterNav.pPtPrj)
                    pSect0.AddPoint(pt2)
                    pSect0.AddPoint(pt1)
                    pSect0.AddPoint(InterNav.pPtPrj)
                Case eIntersectionType.ByDistance, eIntersectionType.RadarFIX
                    fDirl = DeConvertDistance(fDirl)
                    fTmp = fDirl

                    N = UBound(InterNav.ValMin)

                    If OptionButton105.Checked Or (N = 0) Then
                        If fDirl < InterNav.ValMin(0) Then
                            fDirl = InterNav.ValMin(0)
                        ElseIf fDirl > InterNav.ValMax(0) Then
                            fDirl = InterNav.ValMax(0)
                        End If
                    Else
                        If fDirl < InterNav.ValMin(1) Then
                            fDirl = InterNav.ValMin(1)
                        ElseIf fDirl > InterNav.ValMax(1) Then
                            fDirl = InterNav.ValMax(1)
                        End If
                    End If

                    If fTmp <> fDirl Then
                        TextBox102.Text = CStr(ConvertDistance(fDirl, 2))
                    End If

                    If fDirl < 1 Then fDirl = 1

                    If (InterNav.IntersectionType <> eIntersectionType.RadarFIX) And ((InterNav.ValCnt < 0) Or beforeNavaid) Then
                        CircleVectorIntersect(InterNav.pPtPrj, fDirl, NextFIX.pPtPrj, InDir + 180.0#, ptKT1)
                    Else
                        CircleVectorIntersect(InterNav.pPtPrj, fDirl, NextFIX.pPtPrj, InDir, ptKT1)
                    End If

                    hFix = fFIXHeight - InterNav.pPtPrj.Z

                    d0 = Math.Sqrt(fDirl * fDirl + hFix * hFix) * DME.ErrorScalingUp + DME.MinimalError

                    D = fDirl + d0
                    pSect0 = CreatePrjCircle(InterNav.pPtPrj, D)

                    pCutter = New Polyline


                    pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, InDir + 90.0#, D + D)


                    pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, InDir - 90.0#, D + D)

                    D = fDirl - d0
                    pSect1 = CreatePrjCircle(InterNav.pPtPrj, D)

                    pTopo = pSect0

                    pPoly = pTopo.Difference(pSect1)

                    pTopo = pPoly
                    pTopo.IsKnownSimple_2 = False

                    pTopo.Simplify()




                    If SideDef(pCutter.FromPoint, InDir + 180.0#, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

                    If (InterNav.IntersectionType <> eIntersectionType.RadarFIX) And ((InterNav.ValCnt < 0) Or beforeNavaid) Then

                        pTopo.Cut(pCutter, pSect1, pSect0)
                    Else

                        pTopo.Cut(pCutter, pSect0, pSect1)
                    End If
            End Select

            '============================================================================
            StepDownFIXs(StepDownsNum).OutDir = InDir
            StepDownFIXs(StepDownsNum).ArcDir = 0
            LegLength = ReturnDistanceInMeters(NextFIX.pPtPrj, ptKT1)
        End If

        StepDownFIXs(StepDownsNum).Length = LegLength
        TextBox101.Text = CStr(ConvertDistance(LegLength, 2))

        TextBox110Range.Right = NextFIX.pPtPrj.Z + (LegLength + NextFIX.TotalLength) * IAF_PDG
        If fFIXHeight > TextBox110Range.Right Then fFIXHeight = TextBox110Range.Right

        TextBox110.Text = CStr(ConvertHeight(fFIXHeight, 2))
        ToolTip1.SetToolTip(TextBox110, str00210 & str00221 & CStr(ConvertHeight(TextBox110Range.Left, 2)) & " " & HeightConverter(HeightUnit).Unit & str00222 & CStr(ConvertHeight(TextBox110Range.Right, 2)) & " " & HeightConverter(HeightUnit).Unit)

        ptKT1.Z = fFIXHeight
        ptKT1.M = InDir

        pTopo = pSect0
        pTopo.IsKnownSimple_2 = False

        pTopo.Simplify()

        If InterNav.IntersectionType = eIntersectionType.OnNavaid Then
            pFixPoly = pSect0
        Else
            Clone = pGuidPoly
            pTopo = Clone.Clone
            pTopo.IsKnownSimple_2 = False

            pTopo.Simplify()


            pFixPoly = pTopo.Intersect(pSect0, esriGeometryDimension.esriGeometry2Dimension)
        End If
        dFar = 1000000.0#

        '====================================================
        pCutter = New Polyline
        ptTmp = PointAlongPlane(ptKT1, InDir + 180.0#, dFar)


        pCutter.FromPoint = PointAlongPlane(ptTmp, InDir + 90.0#, dFar)


        pCutter.ToPoint = PointAlongPlane(ptTmp, InDir - 90.0#, dFar)
        pProxi = pFixPoly

        dD = dFar - pProxi.ReturnDistance(pCutter)

        TextBox104.Text = CStr(ConvertDistance(dD, 2))
        '====================================================

        StepDownFIXs(StepDownsNum - 1).pPtWarn = ptKT1
        StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
        StepDownFIXs(StepDownsNum).pPtStart = ptKT1

        StepDownFIXs(StepDownsNum).pFixPoly = pFixPoly

        StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(pFixPoly, RGB(128, 255, 128))

        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(ptKT1, 0)

        StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
        StepDownFIXs(StepDownsNum).ptElem.Locked = True

        '==========================================================================
        TextBox102.Tag = TextBox102.Text
    End Sub

    Private Sub TextBox109_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox109.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox109_Validating(TextBox109, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox109.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox109_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox109.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim K As Integer
        Dim fTmp As Double
        Dim fValue As Double
        Dim GuidNav As NavaidData
        Dim NextFIX As StepDownFIX
        'Dim pNomPoly As ESRI.ArcGIS.Geometry.IPointCollection
        'Dim pConstructPoint As ESRI.ArcGIS.Geometry.IConstructPoint

        If Not IsNumeric(TextBox109.Text) Then GoTo EventExitSub

        K = ComboBox101.SelectedIndex
        If K < 0 Then GoTo EventExitSub

        GuidNav = ComboBox101.SelectedItem

        fTmp = CDbl(TextBox109.Text)

        If GuidNav.TypeCode = eNavaidType.DME Then
            fValue = DeConvertDistance(fTmp)
            fTmp = fValue
            If fValue < CurrInterval.Left Then
                fValue = CurrInterval.Left
            ElseIf fValue > CurrInterval.Right Then
                fValue = CurrInterval.Right
            End If

            If fValue <> fTmp Then
                TextBox109.Text = CStr(ConvertDistance(fValue, 2))
            End If
        Else

            NextFIX = StepDownFIXs(StepDownsNum - 1)


            fValue = Azt2Dir(ToGeo(NextFIX.pPtPrj), fTmp + GuidNav.MagVar)
            fTmp = fValue

            If (CurrInterval.Left = 0.0#) And (CurrInterval.Right = 360.0#) Then

            ElseIf Not AngleInSector(fValue, CurrInterval.Left, CurrInterval.Right) Then
                If SubtractAngles(fValue, CurrInterval.Left) < SubtractAngles(fValue, CurrInterval.Right) Then
                    fValue = CurrInterval.Left
                Else
                    fValue = CurrInterval.Right
                End If
            End If

            turnMagBearing = Dir2Azt(NextFIX.pPtPrj, fValue) - GuidNav.MagVar
            If fValue <> fTmp Then
                TextBox109.Text = CStr(Math.Round(turnMagBearing, 2))
            End If
        End If
        DoTurn(fValue)
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub TextBox110_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox110.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox110_Validating(TextBox110, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox110.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox110_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox110.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim fTmp As Double
        Dim fNewHeight As Double
        If Not IsNumeric(TextBox110.Text) Then GoTo EventExitSub

        fNewHeight = DeConvertHeight(CDbl(TextBox110.Text))
        fTmp = fNewHeight

        If fNewHeight < TextBox110Range.Left Then
            fNewHeight = TextBox110Range.Left
        ElseIf fNewHeight > TextBox110Range.Right Then
            fNewHeight = TextBox110Range.Right
        End If

        fFIXHeight = fNewHeight
        ptKT1.Z = fFIXHeight

        If fTmp <> fNewHeight Then
            TextBox110.Text = CStr(ConvertHeight(fNewHeight, 2))
        End If
        TextBox110.Tag = ""
        '    If OptionButton101.Value Then OptionButton101_Click
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub SaveAccuracy(RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader)
        AccurRep = New ReportFile()

        AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + My.Resources.str00805)
        'AccurRep.H1(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00805)
        AccurRep.WriteMessage(My.Resources.str00003 + " - " + RepFileTitle + ": " + My.Resources.str00805)
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteHeader(pReport)
        AccurRep.Param("Distance accuracy", _settings.DistancePrecision, DistanceConverter(DistanceUnit).Unit)
        AccurRep.Param("Angle accuracy", _settings.AnglePrecision, "degrees")

        AccurRep.WriteMessage()
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteMessage("=================================================")
        AccurRep.WriteMessage()

        'Legs =================================================================================

        Dim i, n As Integer
        Dim currLeg As StepDownFIX
        Dim GuidNav As NavaidData
        Dim IntersectNav As NavaidData

        n = UBound(LegList)

        currLeg = LegList(0)
        GuidNav = currLeg.GuidanceNav
        IntersectNav = currLeg.IntersectNav

        If (Not currLeg.FixStartPoint.Created) And (Not currLeg.FixStartPoint.OnNavaid) Then
            SaveFixAccurasyInfo(AccurRep, currLeg.pPtPrj, IIf(bFirstPointIsIF, "IAF", "SDF"), GuidNav, IntersectNav, n = 0)
            '		currLeg.Name
        End If
        '==================================================================================

        For i = 1 To n
            currLeg = LegList(i)

            If (currLeg.TurnDir = 0) And (currLeg.LegTypeARINC <> CodeSegmentPath.CI) Then
                Dim Role As String
                IntersectNav = currLeg.IntersectNav
                GuidNav = currLeg.GuidanceNav

                If i = n Then
                    If bFirstPointIsIF Then
                        Role = "IF"
                    Else
                        Role = "IAF"
                    End If
                Else
                    Role = "SDF"
                End If

                If (Not currLeg.FixStartPoint.Created) And (Not currLeg.FixStartPoint.OnNavaid) Then
                    SaveFixAccurasyInfo(AccurRep, currLeg.pPtPrj, Role, GuidNav, IntersectNav, i = n)
                End If
            End If

        Next i

        '=============================================================================================================

        AccurRep.CloseFile()
    End Sub

	Private Sub SaveProtocol(ByRef RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader) 'Initial Approach
		Dim Headers() As String

		ProtRep = New ReportFile()

		Headers = New String() {str40231, str40232, str40233, str40234, str40235,
			str40236, str40237 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")", str40238,
			str40239 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")", str40240 + " (" + ReportSpeedConverter(ReportSpeedUnit).Unit + ")", str40241, str40242}
		'ProtRep.ThrPtPrj = pFAFptPrj
		'ProtRep.RefHeight = fRefHeight

		If RepFileName IsNot Nothing Then
			ProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + str00815)
		Else
			ProtRep.Open(RepFileTitle + ": " + str00815)
		End If

		WriteProtocol(pReport, RepFileTitle, Headers)

		ProtRep.CloseFile()
	End Sub

	Private Sub SaveProtocol(RepFileTitle As String, ByRef pReport As ReportHeader) 'Initial Approach
		SaveProtocol(Nothing, RepFileTitle, pReport)
	End Sub

	Private Sub WriteProtocol(pReport As ReportHeader, RepFileTitle As String, Headers() As String)
        Dim I As Integer
        'ProtRep.H1(str00003 + " - " + RepFileTitle + ": " + str00815)
        'ProtRep.WriteMessage()
        'ProtRep.WriteMessage(RepFileTitle)
        ProtRep.WriteHeader(pReport, True)
        '    ProtRep.WriteParam My.Resources.str813), CStr(Date) + " - " + CStr(Time)

        ProtRep.WriteMessage()
        ProtRep.WriteMessage()

        ProtRep.Page("FORMAL DESCRIPTION OF THE PROCEDURE")
        ProtRep.HTMLMessage("FORMAL DESCRIPTION OF THE PROCEDURE")
        ProtRep.HTMLMessage()

        ProtRep.WriteLegs(LegList, CalculateFirstLegReport(LegList(0)), Headers)

        ProtRep.WriteMessage()
        ProtRep.WriteMessage()

        ProtRep.HTMLMessage("[ " + InitialReportsFrm.MultiPage1.TabPages.Item(1).Text + " ]") '
        ProtRep.HTMLMessage()

        ProtRep.lListView = InitialReportsFrm.ListView1

        For I = 0 To InitialReportsFrm.SegmUbnd
            ProtRep.Page(InitialReportsFrm.MultiPage1.TabPages.Item(1).Text + " " + (InitialReportsFrm.SegmUbnd - I + 1).ToString())
            'If StepDownFIXs(I + 2).TurnDir = 0 Then
            '	ProtRep.Param((Label115.Text), CStr(ConvertHeight(StepDownFIXs(I + 2).pPtPrj.Z, eRoundMode.NERAEST)), HeightConverter(HeightUnit).Unit)
            '	ProtRep.WriteMessage()
            'End If
            ProtRep.WriteObstData(SegmentData(I).FIXObstacles, SegmentData(I).FIXIx)
            ProtRep.WriteMessage()
        Next I

    End Sub

	Private Function FirstLeg(ByVal pProcedure As Procedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint, ByVal isApproach As Boolean) As SegmentLeg
		Dim fDir As Double
		Dim Angle As Double
		Dim fDist As Double
		Dim fCourseDir As Double

		Dim fDistToNav As Double
		Dim fAltitudeMin As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim pFIXSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pArrivalLeg As SegmentLeg
		Dim pSegmentLeg As SegmentLeg
		Dim pStartPoint As TerminalSegmentPoint
		Dim pDistanceIndication As DistanceIndication
		Dim pAngleIndication As AngleIndication

		Dim pFixDesignatedPoint As DesignatedPoint

		'Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical
		Dim pSpeed As ValSpeed

		Dim currLeg As StepDownFIX

		Dim mUomSpeed As UomSpeed
		Dim mUomHDistance As UomDistance
		Dim mUomVDistance As UomDistanceVertical

		Dim uomSpeedTab() As UomSpeed
		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomDistHorzTab() As UomDistance
		Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidData
		Dim SttIntesectNav As NavaidData
		Dim ptStart As IPoint

		uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT}
		uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		currLeg = LegList(0)
		GuidNav = currLeg.GuidanceNav
		SttIntesectNav = currLeg.IntersectNav
		ptStart = currLeg.pPtPrj

		If isApproach Then
			pArrivalLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		Else
			pArrivalLeg = pObjectDir.CreateFeature(Of ArrivalLeg)()
		End If

		pArrivalLeg.AircraftCategory.Add(IsLimitedTo)
		pSegmentLeg = pArrivalLeg

		'If currLeg.TurnDir_A = 1 Then
		'	pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT
		'ElseIf currLeg.TurnDir_A = -1 Then
		'	pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT
		'Else
		'	'pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER
		'End If

		'pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK
		'pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT
		pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER
		pSegmentLeg.SpeedReference = CodeSpeedReference.IAS

		'=======================================================================================
		'If GuidNav.TypeCode = eNavaidType.DME Then
		'	Dim pArcCentre As TerminalSegmentPoint

		'	pSegmentLeg.LegPath = CodeTrajectory.ARC

		'	If SideDef(ptStart, currLeg.OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CCW
		'	End If

		'	pArcCentre = New TerminalSegmentPoint()
		'	pArcCentre.Waypoint = False
		'	pArcCentre.PointChoice = GuidNav.GetSignificantPoint
		'	pSegmentLeg.ArcCentre = pArcCentre

		'	pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
		'Else
		'	fCourseDir = currLeg.OutDir

		'	pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT

		'	If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.FROM
		'	Else
		'		pSegmentLeg.CourseDirection = CodeDirectionReference.TO
		'	End If

		'	pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
		'End If
		pSegmentLeg.LegTypeARINC = CodeSegmentPath.IF 'currLeg.LegTypeARINC

		'=======================================================================================
		pSegmentLeg.BankAngle = arInitApprBank
		'=======================================================================================

		pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptStart.Z, eRoundMode.NEAREST), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'=======================================================================================
		'pDistance = New ValDistance(ConvertDistance(currLeg.Length, eRoundMode.NEAREST), mUomHDistance)
		'pSegmentLeg.Length = pDistance
		'======
		'pSegmentLeg.VerticalAngle = -RadToDeg(Math.Atan(currLeg.PDG))
		'======

		pSpeed = New ValSpeed(ConvertSpeed(cViafMax.Values(Category), eRoundMode.NEAREST), mUomSpeed)
		pSegmentLeg.SpeedLimit = pSpeed

		' StarPoint ========================
		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		pStartPoint = New TerminalSegmentPoint()
		If isApproach Then pStartPoint.Role = CodeProcedureFixRole.IAF
		'Else
		'pStartPoint.Role = CodeProcedureFixRole.OTHER_WPT

		'pStartPoint.FlyOver = False

		pStartPoint.RadarGuidance = False
		pStartPoint.ReportingATC = CodeATCReporting.COMPULSORY
		pStartPoint.Waypoint = False

		' ========================
		pInterNavSignPt = SttIntesectNav.GetSignificantPoint()

		' Start Point ==================================================
		pPointReference = New PointReference()

		If GuidNav.TypeCode = eNavaidType.DME Then
			fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart) +
				 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW))    'Azt2DirPrj(ptStart, pSegmentLeg.Course)
		Else
			fCourseDir = currLeg.OutDir
		End If

		If currLeg.FixStartPoint.Created And currLeg.FixStartPoint.OnNavaid Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			If currLeg.FixStartPoint.Created Then
				pFixDesignatedPoint = New DesignatedPoint()
				pFixDesignatedPoint.Identifier = currLeg.FixStartPoint.NAV_Ident
			Else
				Dim HorAccuracy As Double = 0.0
				If (GuidNav.TypeCode <> eNavaidType.DME) And (SttIntesectNav.Identifier <> Guid.Empty) Then
					HorAccuracy = CalcHorisontalAccuracy(ptStart, GuidNav, SttIntesectNav)
				End If

				pFixDesignatedPoint = CreateDesignatedPoint(ptStart, "PSTAR", fCourseDir)
			End If

			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			If GuidNav.TypeCode = eNavaidType.DME Then
				fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptStart)
				fAltitudeMin = ptStart.Z - GuidNav.pPtPrj.Z
				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

				pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, GuidNav.GetSignificantPoint)
				pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
				pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
			Else
				fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart)
				Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

				pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pGuidNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse = New AngleUse()
				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = True

				pPointReference.FacilityAngle.Add(pAngleUse)
			End If

			' ========================
			If SttIntesectNav.TypeCode = eNavaidType.DME Then
				fDistToNav = ReturnDistanceInMeters(SttIntesectNav.pPtPrj, ptStart)
				fAltitudeMin = ptStart.Z - SttIntesectNav.pPtPrj.Z

				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
				pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
				pPointReference.Role = CodeReferenceRole.RAD_DME
			Else
				pAngleUse = New AngleUse
				fDir = ReturnAngleInDegrees(SttIntesectNav.pPtPrj, ptStart)

				Angle = Modulus(Dir2Azt(SttIntesectNav.pPtPrj, fDir) - SttIntesectNav.MagVar, 360.0)
				pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pInterNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = False

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If
		End If
		'=================================================================
		Dim pTolerArea As IPolygon
		pTolerArea = currLeg.pFixPoly

		If Not pTolerArea Is Nothing Then
			PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

			pDistanceSigned = New ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
			pPointReference.PriorFixTolerance = pDistanceSigned

			pDistanceSigned = New ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

			pPointReference.PostFixTolerance = pDistanceSigned
			pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
		End If
		'=================================================================
		pStartPoint.FacilityMakeup.Add(pPointReference)
		pStartPoint.PointChoice = pFIXSignPt
		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ========================

		Return pArrivalLeg
		' END ====================================================
	End Function

	Private Function ArrivalAndApproachLeg(ByVal Index As Integer, ByVal pProcedure As Procedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint,
                                           ByVal isApproach As Boolean) As SegmentLeg
        Dim N As Integer

        Dim fDir As Double
        Dim Angle As Double
        Dim fDist As Double
        Dim fCourseDir As Double

        Dim fDistToNav As Double
        Dim fAltitudeMin As Double
        Dim PostFixTolerance As Double
        Dim PriorFixTolerance As Double

        Dim pFIXSignPt As SignificantPoint
        Dim pInterNavSignPt As SignificantPoint
        Dim pGuidNavSignPt As SignificantPoint
        Dim pPointReference As PointReference

        Dim pArrivalLeg As SegmentLeg
        Dim pSegmentLeg As SegmentLeg
        Dim pStartPoint As TerminalSegmentPoint
        Dim pDistanceIndication As DistanceIndication
        Dim pAngleIndication As AngleIndication

        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical
        Dim pSpeed As ValSpeed

        Dim currLeg As StepDownFIX
        Dim nextLeg As StepDownFIX

        Dim mUomSpeed As UomSpeed
        Dim mUomHDistance As UomDistance
        Dim mUomVDistance As UomDistanceVertical

        Dim pLocation As Geometries.Point

        Dim uomSpeedTab() As UomSpeed
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomDistHorzTab() As UomDistance
        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData
        Dim SttIntesectNav As NavaidData
        Dim EndIntesectNav As NavaidData
        Dim ptStart As IPoint
        Dim ptEnd As IPoint

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        currLeg = LegList(Index)
        GuidNav = currLeg.GuidanceNav
        SttIntesectNav = currLeg.IntersectNav
        ptStart = currLeg.pPtPrj
        N = UBound(LegList)

        If Index < N - 1 Then
            nextLeg = LegList(Index + 1)
            EndIntesectNav = nextLeg.IntersectNav
            ptEnd = nextLeg.pPtPrj
        End If

        If isApproach Then
            pArrivalLeg = pObjectDir.CreateFeature(Of InitialLeg)()
        Else
            pArrivalLeg = pObjectDir.CreateFeature(Of ArrivalLeg)()
        End If

        pArrivalLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pArrivalLeg

        pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL

        If currLeg.TurnDir_A = 1 Then
            pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT
        ElseIf currLeg.TurnDir_A = -1 Then
            pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT
        Else
            'pSegmentLeg.TurnDirection = CodeDirectionTurn.EITHER
        End If

        pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK
        pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT
        pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN
        pSegmentLeg.SpeedReference = CodeSpeedReference.IAS

        '=======================================================================================
        If GuidNav.TypeCode = eNavaidType.DME Then
            Dim pArcCentre As TerminalSegmentPoint

            pSegmentLeg.LegPath = CodeTrajectory.ARC

            If SideDef(ptStart, currLeg.OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW
            Else
                pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CCW
            End If

            pArcCentre = New TerminalSegmentPoint()
            pArcCentre.Waypoint = False
            pArcCentre.PointChoice = GuidNav.GetSignificantPoint
            pSegmentLeg.ArcCentre = pArcCentre

            pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
        Else
            fCourseDir = currLeg.OutDir

            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT

            If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = CodeDirectionReference.FROM
            Else
                pSegmentLeg.CourseDirection = CodeDirectionReference.TO
            End If

            pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
        End If
        pSegmentLeg.LegTypeARINC = currLeg.LegTypeARINC

        '=======================================================================================
        pSegmentLeg.BankAngle = arInitApprBank
		'=======================================================================================

		If Index < N - 1 Then
			pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptEnd.Z, eRoundMode.NEAREST), mUomVDistance)
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical
		Else
			pDistanceVertical = New ValDistanceVertical(ConvertHeight(pIFptPrj.Z, eRoundMode.NEAREST), mUomVDistance)
			pSegmentLeg.LowerLimitAltitude = pDistanceVertical
		End If

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptStart.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical

        '=======================================================================================
        pDistance = New ValDistance(ConvertDistance(currLeg.Length, eRoundMode.NEAREST), mUomHDistance)
        pSegmentLeg.Length = pDistance
        '======
        pSegmentLeg.VerticalAngle = -RadToDeg(Math.Atan(currLeg.PDG))
        '======

        pSpeed = New ValSpeed(ConvertSpeed(cViafMax.Values(Category), eRoundMode.NEAREST), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed

        ' StarPoint ========================
        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        If Index > 0 Then
            pStartPoint = pEndPoint
        Else
            pStartPoint = New TerminalSegmentPoint()
            If isApproach Then
                pStartPoint.Role = CodeProcedureFixRole.IAF
                'Else
                'pStartPoint.Role = CodeProcedureFixRole.OTHER_WPT
            End If

            'pStartPoint.FlyOver = False

            pStartPoint.RadarGuidance = False
            pStartPoint.ReportingATC = CodeATCReporting.NO_REPORT
            pStartPoint.Waypoint = False

            ' ========================
            pInterNavSignPt = SttIntesectNav.GetSignificantPoint()

            ' Start Point ==================================================
            pPointReference = New PointReference()

            If GuidNav.TypeCode = eNavaidType.DME Then
                fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart) +
                 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW))    'Azt2DirPrj(ptStart, pSegmentLeg.Course)
            End If

            If currLeg.FixStartPoint.Created And currLeg.FixStartPoint.OnNavaid Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                If currLeg.FixStartPoint.Created Then
                    pFixDesignatedPoint = New DesignatedPoint()
                    pFixDesignatedPoint.Identifier = currLeg.FixStartPoint.NAV_Ident

                    'pFixDesignatedPoint = currLeg.FixStartPoint.
                Else
                    Dim HorAccuracy As Double = 0.0
                    If (GuidNav.TypeCode <> eNavaidType.DME) And (SttIntesectNav.Identifier <> Guid.Empty) Then
                        HorAccuracy = CalcHorisontalAccuracy(ptStart, GuidNav, SttIntesectNav)
                    End If

                    pFixDesignatedPoint = CreateDesignatedPoint(ptStart, "PSTAR", fCourseDir)
                End If

                pFIXSignPt = New SignificantPoint()
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

                If GuidNav.TypeCode = eNavaidType.DME Then
                    fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptStart)
                    fAltitudeMin = ptStart.Z - GuidNav.pPtPrj.Z
                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

                    pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, GuidNav.GetSignificantPoint)
                    pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                    pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                Else
                    fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart)
                    Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

                    pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pGuidNavSignPt)
                    pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                    pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pAngleUse = New AngleUse()
                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = True

                    pPointReference.FacilityAngle.Add(pAngleUse)
                End If

                ' ========================
                If SttIntesectNav.TypeCode = eNavaidType.DME Then
                    fDistToNav = ReturnDistanceInMeters(SttIntesectNav.pPtPrj, ptStart)
                    fAltitudeMin = ptStart.Z - SttIntesectNav.pPtPrj.Z

                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
                    pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                    pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                    pPointReference.Role = CodeReferenceRole.RAD_DME
                Else
                    pAngleUse = New AngleUse
                    fDir = ReturnAngleInDegrees(SttIntesectNav.pPtPrj, ptStart)

                    Angle = Modulus(Dir2Azt(SttIntesectNav.pPtPrj, fDir) - SttIntesectNav.MagVar, 360.0)
                    pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pInterNavSignPt)
                    pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                    pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = False

                    pPointReference.FacilityAngle.Add(pAngleUse)
                    pPointReference.Role = CodeReferenceRole.INTERSECTION
                End If
            End If
            '=================================================================
            Dim pTolerArea As IPolygon
            pTolerArea = currLeg.pFixPoly

            If Not pTolerArea Is Nothing Then
                PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

                pDistanceSigned = New ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                pPointReference.PriorFixTolerance = pDistanceSigned

                pDistanceSigned = New ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                pPointReference.PostFixTolerance = pDistanceSigned
                pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
            End If
            '=================================================================
            pStartPoint.FacilityMakeup.Add(pPointReference)
            pStartPoint.PointChoice = pFIXSignPt
        End If

        pSegmentLeg.StartPoint = pStartPoint
        ' End Of Start Point ========================

        ' EndPoint ========================
        pEndPoint = Nothing
        'If (pSegmentLeg.TurnDir = 0) And (pSegmentLeg.LegTypeARINC <> CodeSegmentPath.CI) Then
        If pSegmentLeg.LegTypeARINC <> CodeSegmentPath.CI Then
            If Index < N - 1 Then
                pEndPoint = New TerminalSegmentPoint

                If currLeg.Tag <> 0 Then
                    pEndPoint.Role = CodeProcedureFixRole.TP '??????????????????????
                    'pEndPoint.FlyOver = True
                ElseIf currLeg.TurnDir_A = 0 Then
                    pEndPoint.Role = CodeProcedureFixRole.SDF
                    'pEndPoint.FlyOver = True
                Else
                    pEndPoint.Role = CodeProcedureFixRole.TP
                    'pEndPoint.FlyOver = False
                End If

                pEndPoint.RadarGuidance = False
                pEndPoint.ReportingATC = CodeATCReporting.NO_REPORT
                pEndPoint.Waypoint = False
                '        pEndPoint.IndicatorFACF =      ??????????
                '        pEndPoint.LeadDME =            ??????????
                '        pEndPoint.LeadRadial =         ??????????
                If GuidNav.TypeCode = eNavaidType.DME Then
                    fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd) +
                     90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW))     'Azt2DirPrj(ptStart, pSegmentLeg.Course)
                End If

                pPointReference = New PointReference()
                ' Indication ======================================================================


                Dim bOnNav As Boolean
                bOnNav = False

                If EndIntesectNav.Identifier <> Guid.Empty Then
                    pInterNavSignPt = EndIntesectNav.GetSignificantPoint()

                    If currLeg.FixEndPoint.Created And currLeg.FixEndPoint.OnNavaid Then
                        bOnNav = True
                        pFIXSignPt = pInterNavSignPt
                    End If
                End If

                If Not bOnNav Then
                    If currLeg.FixEndPoint.Created Then
                        pFixDesignatedPoint = New DesignatedPoint()
                        pFixDesignatedPoint.Identifier = currLeg.FixEndPoint.NAV_Ident
                    Else
                        Dim HorAccuracy As Double = 0.0
                        If (GuidNav.TypeCode <> eNavaidType.DME) And (EndIntesectNav.Identifier <> Guid.Empty) Then
                            HorAccuracy = CalcHorisontalAccuracy(ptEnd, GuidNav, EndIntesectNav)
                        End If

                        pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, "PSTAR", fCourseDir)
                    End If

                    pFIXSignPt = New SignificantPoint()
                    pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

                    If GuidNav.TypeCode = eNavaidType.DME Then
                        fDistToNav = ReturnDistanceInMeters(GuidNav.pPtPrj, ptEnd)
                        fAltitudeMin = ptEnd.Z - GuidNav.pPtPrj.Z
                        fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

                        pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, GuidNav.GetSignificantPoint)
                        pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                        pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                    Else
                        fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd)
                        Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

                        pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pGuidNavSignPt)
                        pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
                        pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                        pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                        pAngleUse = New AngleUse()
                        pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                        pAngleUse.AlongCourseGuidance = True

                        pPointReference.FacilityAngle.Add(pAngleUse)
                    End If
                End If
                ' End Of Indication ============================================

                If EndIntesectNav.Identifier <> Guid.Empty Then
                    ' End Point ==================================================
                    If EndIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
                        pFIXSignPt = pInterNavSignPt
                        pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
                    Else
                        If EndIntesectNav.TypeCode = eNavaidType.DME Then
                            fDistToNav = ReturnDistanceInMeters(EndIntesectNav.pPtPrj, ptEnd)
                            fAltitudeMin = ptEnd.Z - EndIntesectNav.pPtPrj.Z

                            fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
                            pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                            pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                            pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                            pPointReference.Role = CodeReferenceRole.RAD_DME
                        Else
                            pAngleUse = New AngleUse
                            fDir = ReturnAngleInDegrees(EndIntesectNav.pPtPrj, ptEnd)

                            Angle = Modulus(Dir2Azt(EndIntesectNav.pPtPrj, fDir) - EndIntesectNav.MagVar, 360.0)
                            pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pInterNavSignPt)
                            pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
                            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                            pAngleUse.AlongCourseGuidance = False

                            pPointReference.FacilityAngle.Add(pAngleUse)
                            pPointReference.Role = CodeReferenceRole.INTERSECTION
                        End If
                    End If
                    '=================================================================
                    Dim pEndTolerArea As IPolygon
                    pEndTolerArea = LegList(Index + 1).pFixPoly

                    If Not pEndTolerArea Is Nothing Then
                        PriorPostFixTolerance(pEndTolerArea, ptEnd, fCourseDir, PriorFixTolerance, PostFixTolerance)

                        pDistanceSigned = New ValDistanceSigned()
                        pDistanceSigned.Uom = mUomHDistance
                        pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                        pPointReference.PriorFixTolerance = pDistanceSigned

                        pDistanceSigned = New ValDistanceSigned()
                        pDistanceSigned.Uom = mUomHDistance
                        pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                        pPointReference.PostFixTolerance = pDistanceSigned
                        pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pEndTolerArea))
                    End If
                    '=================================================================
                Else
                    pPointReference.Role = CodeReferenceRole.RECNAV
                End If

                pEndPoint.FacilityMakeup.Add(pPointReference)
                pEndPoint.PointChoice = pFIXSignPt
            Else
                pEndPoint = pIFPoint
            End If
        End If

        pSegmentLeg.EndPoint = pEndPoint

        ' End of EndPoint ========================

        ' Trajectory ===========================================
        Dim J As Integer
        Dim I As Integer
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim pPolyline As IGeometryCollection
        Dim pPath As IPointCollection

        pCurve = New Curve
        pPolyline = currLeg.pNominalPoly

        For J = 0 To pPolyline.GeometryCount - 1
            pPath = pPolyline.Geometry(J)
            pLineStringSegment = New LineString

            For I = 0 To pPath.PointCount - 1
                pLocation = ESRIPointToARANPoint(ToGeo(pPath.Point(I)))
                pLineStringSegment.Add(pLocation)
            Next I
            pCurve.Geo.Add(pLineStringSegment)
        Next J

        pSegmentLeg.Trajectory = pCurve
        ' Trajectory =============================================

        ' I Protection Area 
        Dim pSurface As Surface
        Dim pPrimProtectedArea As ObstacleAssessmentArea

        pSurface = ESRIPolygonToAIXMSurface(ToGeo(currLeg.pIAreaPoly))

        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

        ' I Protection Area ========================================

        ' II Protection Area 
        Dim pTopo As ITopologicalOperator2
        Dim pPolygon As IPolygon
        pTopo = currLeg.pIIAreaPoly
        pPolygon = pTopo.Difference(currLeg.pIAreaPoly)
        pTopo = pPolygon
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        Dim pSecProtectedArea As ObstacleAssessmentArea
        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

        pSecProtectedArea = New ObstacleAssessmentArea
        pSecProtectedArea.Surface = pSurface
        pSecProtectedArea.SectionNumber = 1
        pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

        ' II Protection Area ========================================

        'Protection Area Obstructions list ==================================================
        AddObstacles(currLeg.Obstacles, mUomVDistance, pPrimProtectedArea, pSecProtectedArea)
        'Dim ostacles As ObstacleContainer = currLeg.Obstacles

        'For I = 0 To ostacles.Obstacles.Length - 1
        '	Dim obs As Obstruction = New Obstruction
        '	obs.VerticalStructureObstruction = ostacles.Obstacles(I).GetFeatureRef()

        '	Dim MinimumAltitude As Double = 0
        '	Dim RequiredClearance As Double = 0
        '	Dim isPrimary As Integer = 0

        '	For J = 0 To ostacles.Obstacles(I).PartsNum - 1
        '		MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts(ostacles.Obstacles(I).Parts(J)).ReqH)
        '		RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts(ostacles.Obstacles(I).Parts(J)).MOC)
        '		If ((ostacles.Parts(ostacles.Obstacles(I).Parts(J)).Flags And 1) = 1) Then
        '			isPrimary = isPrimary Or 1
        '		Else
        '			isPrimary = isPrimary Or 2
        '		End If
        '	Next

        '	'ReqH
        '	pDistanceVertical = New ValDistanceVertical()
        '	pDistanceVertical.Uom = mUomVDistance
        '	pDistanceVertical.Value = ConvertHeight(MinimumAltitude, eRoundMode.NEAREST)
        '	obs.MinimumAltitude = pDistanceVertical

        '	'MOC
        '	pDistance = New ValDistance()
        '	pDistance.Uom = UomDistance.M
        '	pDistance.Value = RequiredClearance
        '	obs.RequiredClearance = pDistance

        '	If ((isPrimary And 1) <> 0) Then
        '		pPrimProtectedArea.SignificantObstacle.Add(obs)
        '	End If

        '	If ((isPrimary And 2) <> 0) Then
        '		pSecProtectedArea.SignificantObstacle.Add(obs)
        '	End If
        'Next

        pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
        pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

        Return pArrivalLeg
        ' END ====================================================
    End Function

    Private Sub CreateProcedure()
        Dim I As Integer
        Dim N As Integer

        Dim pTransition As ProcedureTransition
        Dim pSegmentLeg As SegmentLeg
        Dim ptl As ProcedureTransitionLeg

        Dim pIAP As InstrumentApproachProcedure
        Dim pSTAR As StandardInstrumentArrival

        Dim pEndPoint As TerminalSegmentPoint

        If segmentLegs Is Nothing Then
            segmentLegs = New List(Of SegmentLeg)
        End If

        segmentLegs.Clear()

        pObjectDir.ClearAllFeatures()
        pIAP = ComboBox002.SelectedItem

        If bFirstPointIsIF Then
            pProcedure = New InstrumentApproachProcedure()
            pProcedure.Assign(pIAP)
        Else
            Dim Ident As Guid
            Dim tls As TimeSlice
            Dim name As String

            pProcedure = pObjectDir.CreateFeature(Of StandardInstrumentArrival)()
            pSTAR = pProcedure

            Ident = pProcedure.Identifier
            tls = pProcedure.TimeSlice

            pProcedure.Assign(pIAP)
            pProcedure.Identifier = Ident
            pProcedure.TimeSlice = tls

            pProcedure.Annotation.Clear()
            pProcedure.Availability.Clear()

            pProcedure.CommunicationFailureInstruction = ""
            pProcedure.FlightChecked = False
            pProcedure.FlightTransition.Clear()
            pProcedure.GuidanceFacility.Clear()
            pProcedure.Instruction = ""

			'         If LegList(LegList.Length - 1).Name Is Nothing Or LegList(LegList.Length - 1).Name.Length = 0 Then
			'             name = "PSTAR"
			'         ElseIf LegList(LegList.Length - 1).Name.Length > 5 Then
			'             name = LegList(LegList.Length - 1).Name.Substring(0, 5)
			'         Else
			'             name = LegList(LegList.Length - 1).Name
			'         End If

			'         pProcedure.Name = name + " 1A"

			''pProcedure.SafeAltitude.
			'pSTAR.Designator = name + "1A"

			pProcedure.Name = RepFileTitle

			pSTAR.Arrival = pIAP.Landing
            pSTAR.CommunicationFailureInstruction = ""
            pSTAR.Instruction = ""

            'added by agsin
            'for link runway to arrival

            Dim count As Integer = ChkRwyDirections.Items.Count

            If (ChkRwyDirections.CheckedItems.Count > 0) Then
                pSTAR.Arrival = New LandingTakeoffAreaCollection()
                For index As Integer = 0 To count - 1
                    If (ChkRwyDirections.GetItemChecked(index)) Then
                        pSTAR.Arrival.Runway.Add(New Objects.FeatureRefObject(rwyDirList(index).Identifier))
                    End If
                Next
            End If
        End If

        'Transition ==========================================================================
        pTransition = New ProcedureTransition
        '    pTransition.Description =
        '    pTransition.ID =
        '    pTransition.Procedure =

        'Legs ======================================================================================================
        'pSegmentLegList = New List(Of SegmentLeg)

        ' Initial Approach Legs ===============================================================================
        N = UBound(LegList)

		'pEndPoint = Nothing
		'segmentLegs.Add(ifcode)

		pSegmentLeg = FirstLeg(pProcedure, pProcedure.AircraftCharacteristic(0), pEndPoint, bFirstPointIsIF)
		segmentLegs.Add(pSegmentLeg)

		For I = 0 To N - 1
            pSegmentLeg = ArrivalAndApproachLeg(I, pProcedure, pProcedure.AircraftCharacteristic(0), pEndPoint, bFirstPointIsIF)
            segmentLegs.Add(pSegmentLeg)
        Next I

        For I = 0 To segmentLegs.Count - 1
            pSegmentLeg = segmentLegs(I)

            ptl = New ProcedureTransitionLeg()
            ptl.SeqNumberARINC = I + 1
            ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
            pTransition.TransitionLeg.Add(ptl)
        Next I

        pTransition.DepartureRunwayTransition = pLandingTakeoff
        '    pTransition.TransitionId = TextBox0???.Text

        pTransition.Type = CodeProcedurePhase.APPROACH ' ProcedurePhaseType_FINAL

        ' Procedure =================================================================================================

        'pProcedure.TimeSlice.CorrectionNumber = pProcedure.TimeSlice.CorrectionNumber + 1
        'pProcedure.TimeSlice.ValidTime.BeginPosition = DateTime.Now
        'pProcedure.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA
        'If pProcedure.TimeSlice.SequenceNumber = 1 Then
        '	pProcedure.TimeSlice.FeatureLifetime.BeginPosition = pProcedure.TimeSlice.ValidTime.BeginPosition
        'End If

        pProcedure.FlightTransition.Add(pTransition)
    End Sub

    Private Function SaveProcedure() As Boolean
        Dim result As Boolean = False
        Dim metadataFeatures As List(Of FeatureType) = New List(Of FeatureType)

        Try
            gAranEnv.GetLogger("INITIAL APPROACH").Info("Initial Approach Save Procedure Start")
            If bFirstPointIsIF Then
                pObjectDir.SetFeature(pProcedure)
                pObjectDir.SetRootFeatureType(FeatureType.InstrumentApproachProcedure)
                metadataFeatures.Add(FeatureType.InstrumentApproachProcedure)
            Else
                pObjectDir.SetRootFeatureType(FeatureType.StandardInstrumentArrival)
                metadataFeatures.Add(FeatureType.StandardInstrumentArrival)
            End If

            Dim commitedFeatures As List(Of FeatureType) = New List(Of FeatureType) From {
                    FeatureType.DesignatedPoint,
                    FeatureType.AngleIndication,
                    FeatureType.DistanceIndication,
                    FeatureType.ArrivalFeederLeg,
                    FeatureType.ArrivalLeg,
                    FeatureType.DepartureLeg,
                    FeatureType.InitialLeg,
                    FeatureType.IntermediateLeg,
                    FeatureType.FinalLeg,
                    FeatureType.MissedApproachLeg,
                    FeatureType.StandardInstrumentDeparture,
                    FeatureType.StandardInstrumentArrival,
                    FeatureType.InstrumentApproachProcedure}

            gAranEnv.GetLogger("INITIAL APPROACH").Info("Initial Approach Procedure Commit Start")
            If gAranEnv.ConnectionInfo.ConnectionType = ConnectionType.TDB And gAranEnv.UseWebApi Then
                result = pObjectDir.CommitWithMetadataViewer(
                    gAranEnv.Graphics.ViewProjection.Name,
                    commitedFeatures.ToArray(),
                    metadataFeatures.ToArray(),
                    GetNumericalData(),
                    False)
            Else
                result = pObjectDir.Commit(commitedFeatures.ToArray())
            End If

            gAranEnv.RefreshAllAimLayers()

            gAranEnv.GetLogger("INITIAL APPROACH").Info("Initial Approach Procedure Save/Commit End")
        Catch ex As Exception
            gAranEnv.GetLogger("INITIAL APPROACH").Error(ex, "Save procedure")
            MsgBox("Error on commit." + vbCrLf + ex.Message)
            Return False
        End Try

        Return result
    End Function

    Private Sub saveReportToDB()
        If (Not ProtRep Is Nothing) Then
            If (ProtRep.IsFinished) Then

                Dim report As FeatureReport = Nothing
                report = New FeatureReport()
                report.Identifier = pProcedure.Identifier
                report.ReportType = FeatureReportType.Obstacle
                report.HtmlZipped = ProtRep.Report
                pObjectDir.SetFeatureReport(report)
            End If
        End If
    End Sub

    Private Sub saveScreenshotToDB()
        Dim screenshot As Screenshot = Nothing
        screenshot = New Screenshot()
        screenshot.DateTime = DateTime.Now
        screenshot.Identifier = pProcedure.Identifier
        screenshot.Images = screenCapture.Commit(pProcedure.Identifier)
        pObjectDir.SetScreenshot(screenshot)
    End Sub

    Private Sub CancelBtn_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CancelBtn.Click
        Me.Close()
    End Sub

    Private Sub ReportBtn_CheckedChanged(sender As Object, e As EventArgs) Handles ReportBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ReportBtn.Checked Then
            InitialReportsFrm.Show(_Win32Window)
        Else
            InitialReportsFrm.Hide()
        End If
    End Sub

    Private Sub TextBox002_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox002.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox002_Validating(TextBox002, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox002.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

	Private Sub TextBox002_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox002.Validating
		Dim fTmp As Double

		If Not IsNumeric(TextBox002.Text) Then Return
		fTmp = DeConvertHeight(CDbl(TextBox002.Text))

		If fTmp < Math.Round(pIFptPrj.Z) Then
			fTmp = Math.Round(pIFptPrj.Z)
			TextBox002.Text = CStr(ConvertHeight(fTmp, 2))
		End If
		pIFptPrj.Z = fTmp
	End Sub

	Private Sub TextBox103_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox103.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox103_Validating(TextBox103, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox103.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox103_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox103.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim NewPDG As Double

        If Not IsNumeric(TextBox103.Text) Then GoTo EventExitSub
        NewPDG = 0.01 * CDbl(TextBox103.Text)
        If NewPDG < 0.0# Then GoTo EventExitSub

        If NewPDG > arIADescent_Max.Value Then
            NewPDG = arIADescent_Max.Value
            TextBox103.Text = CStr(100.0# * NewPDG)
        End If

        If NewPDG < 0.01 * PDGEps Then
            NewPDG = 0.01 * PDGEps
            TextBox103.Text = CStr(100.0# * NewPDG)
        End If

        IAF_PDG = NewPDG

        EstimateIAF_AreaObstacles()
        OptionButton101_CheckedChanged(OptionButton101, New EventArgs())
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub TextBox105_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles TextBox105.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox105_Validating(TextBox105, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox105.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox105_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox105.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim fTmp As Double
        Dim fTmp1 As Double

        If Not IsNumeric(TextBox105.Text) Then GoTo EventExitSub

        fTmp = CDbl(TextBox105.Text)
        If fTmp < 0.0# Then GoTo EventExitSub

        If IsNumeric(TextBox105.Tag) Then
            fTmp1 = CDbl(TextBox105.Tag)
            If fTmp1 = fTmp Then GoTo EventExitSub
        End If

        TextBox105.Tag = TextBox105.Text

        EstimateIAF_AreaObstacles()
        If OptionButton101.Checked Then
            FillComboBox104()
            OptionButton101_CheckedChanged(OptionButton101, New EventArgs())
        End If
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub FocusStepCaption(ByRef StIndex As Object)
        Dim I As Integer
        For I = 0 To 1
            PageLabel(I).ForeColor = Color.FromArgb(&HC0C0C0)
            PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
        Next

        PageLabel(StIndex).ForeColor = Color.FromArgb(&HFF8000)
        PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

        Me.Text = str00003 + " [" + MultiPage1.TabPages.Item(StIndex).Text + "]"       '+ " " + str00818
    End Sub

    Private Sub InitialApproach_FormClosed(ByVal eventSender As Object, ByVal eventArgs As FormClosedEventArgs) Handles Me.FormClosed
        If Not bUnloadByOk Then ClearScr()
        If Not screenCapture Is Nothing Then
            screenCapture.Rollback()
        End If

        InitialReportsFrm.Close()
        CurrCmd = 0


        pGraphics = Nothing

        pIAF_IIAreaElement = Nothing

        pIAF_IAreaElement = Nothing

        pNominalElement = Nothing


        pFAFptElement = Nothing

        pFAFAreaElement = Nothing
        'Set pDefinitionsTable = Nothing


        ptElem = Nothing

        pPlyElem = Nothing


        pFAFptPrj = Nothing


        pIFptPrj = Nothing

        pGuidPoly = Nothing

        IAF_IIAreaPoly = Nothing

        IAF_IAreaPoly = Nothing

        pNominalPoly = Nothing

        'Erase IAFTurnNavs
        Erase IAFProhibSectors
        Erase IAFObstList4FIX.Parts
        Erase IAFObstList4Turn.Parts
    End Sub

    Private Sub TextBox201_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles TextBox201.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim I As Integer
        Dim fTmp As Double
        Dim fMin As Double
        Dim fMax As Double
        Dim fNewVal As Double

        If ListView201.FocusedItem Is Nothing Then GoTo EventExitSub
        If TextBox201.Text = TextBox201.Tag Then GoTo EventExitSub

        I = ListView201.FocusedItem.Index
        If I < ListView201.Items.Count - 1 Then
            fMin = LegList(I + 1).MinAlt
        Else
            fMin = 0
        End If
        fMax = LegList(I).MaxAlt

        If Not IsNumeric(TextBox201.Text) Then
            TextBox201.Text = ListView201.FocusedItem.SubItems.Item(8).Text
            GoTo EventExitSub
        End If
        fNewVal = DeConvertHeight(CDbl(TextBox201.Text))
        fTmp = fNewVal

        If fNewVal < fMin Then fNewVal = fMin
        If fNewVal > fMax Then fNewVal = fMax
        If (fNewVal <> fTmp) Then
            TextBox201.Text = CStr(ConvertHeight(fNewVal, 2))
        End If


        LegList(I).MinAlt = fNewVal
        LegList(I).Report.CalculateMinAlt(LegList(I).MinAlt)

        ListView201.FocusedItem.SubItems.Item(8).Text = TextBox201.Text
        If I < ListView201.Items.Count - 1 Then
            LegList(I + 1).MaxAlt = fNewVal
            LegList(I + 1).Report.CalculateMaxAlt(LegList(I + 1).MaxAlt)

            ListView201.Items.Item(I + 1).SubItems.Item(9).Text = TextBox201.Text
        End If
        TextBox201.Tag = TextBox201.Text
EventExitSub:
        eventArgs.Cancel = Cancel
    End Sub

    Public Function DistToArcAngle(Dist As Double, ByVal fRadius As Double) As Double
        DistToArcAngle = RadToDegValue * Dist / fRadius
    End Function

    Public Function Point2PointArcDistance(ByVal pt0 As IPoint, ByVal pt1 As IPoint, ByVal ptCenter As IPoint, ByVal fRadius As Double, ByVal ArcDir As Long) As Double
        Dim dir0 As Double
        Dim Dir1 As Double

        dir0 = ReturnAngleInDegrees(ptCenter, pt0)
        Dir1 = ReturnAngleInDegrees(ptCenter, pt1)

        Point2PointArcDistance = Modulus((Dir1 - dir0) * ArcDir) * DegToRadValue * fRadius
    End Function

    Private Sub ListView201_ItemClick(ByVal Item As ListViewItem)
        Dim I As Integer
        Dim N As Integer

        SafeDeleteElement(ptElem)
        SafeDeleteElement(pPlyElem)

        pPlyElem = Nothing

        N = UBound(LegList)
        I = Item.Index - 1

        '    TextBox201.Text = CStr(ConvertHeight(LegList(I).M))
        '    TextBox202.Text = CStr(ConvertHeight(LegList(I).M))


        TextBox201.Text = Item.SubItems.Item(7).Text
        TextBox201.Tag = TextBox201.Text

        TextBox201.ReadOnly = Item.Index = N - 1
        If TextBox201.ReadOnly Then
            TextBox201.BackColor = ColorTranslator.FromOle(&H8000000F)
        Else
            TextBox201.BackColor = ColorTranslator.FromOle(&H80000005)
        End If

        '    TextBox202.Text = Item.ListSubItems(8)
        '
        '    TextBox202.Locked = I = 0
        '    If TextBox202.Locked Then
        '        TextBox202.BackColor = &H8000000F
        '    Else
        '        TextBox202.BackColor = &H80000005
        '    End If


        ptElem = DrawPoint(LegList(I).pPtPrj, RGB(0, 255, 0))

        If I < N Then
            pPlyElem = DrawPolyLine(LegList(I).pNominalPoly, RGB(0, 0, 255), 2)
            pPlyElem.Locked = True
        End If

        ptElem.Locked = True
    End Sub

    Private Sub ListView201_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles ListView201.ItemSelectionChanged
        Dim I As Integer
        Dim N As Integer

        SafeDeleteElement(ptElem)
        SafeDeleteElement(pPlyElem)
        SafeDeleteElement(pReportIAreaElement)
        SafeDeleteElement(pReportIIAreaElement)

        ptElem = Nothing
        pPlyElem = Nothing
        pReportIAreaElement = Nothing
        pReportIIAreaElement = Nothing

        'ToStep2()
        'FillTransitionCoding()

        N = UBound(LegList)
        I = e.ItemIndex

        TextBox201.Text = e.Item.SubItems(8).Text
        TextBox201.Tag = TextBox201.Text

        TextBox201.ReadOnly = e.ItemIndex = N - 1
        If TextBox201.ReadOnly Then
            TextBox201.BackColor = ColorTranslator.FromOle(&H8000000F)
        Else
            TextBox201.BackColor = ColorTranslator.FromOle(&H80000005)
        End If

        ptElem = DrawPoint(LegList(I).pPtPrj, RGB(0, 255, 0))
        pPlyElem = DrawPolyLine(LegList(I).pNominalPoly, RGB(0, 0, 255), 2)

        ptElem.Locked = True
        pPlyElem.Locked = True

        pReportIAreaElement = DrawPolygon(LegList(I).pIAreaPoly, RGB(0, 255, 96))
        pReportIAreaElement.Locked = True

        pReportIIAreaElement = DrawPolygon(LegList(I).pIIAreaPoly, RGB(0, 150, 96))
        pReportIIAreaElement.Locked = True

    End Sub

    Private Sub CheckBox101_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox101.CheckedChanged
        If (CheckBox101.Checked) Then
            TextBox102.ReadOnly = True
            ComboBox107.Enabled = True
            ComboBox107_SelectedIndexChanged(ComboBox107, New EventArgs())
        Else
            StepDownFIXs(StepDownsNum).WPT = New WPT_FIXType()
            Dim InterNav As NavaidData = ComboBox104.SelectedItem
            If (InterNav.IntersectionType = eIntersectionType.ByDistance) Or (InterNav.IntersectionType = eIntersectionType.RadarFIX) Then
                OptionButton105.Enabled = InterNav.ValMin.Length > 1
                OptionButton106.Enabled = InterNav.ValMin.Length > 1
            Else
                OptionButton105.Enabled = False
                OptionButton106.Enabled = False
            End If
            TextBox102.ReadOnly = False
            ComboBox107.Enabled = False
        End If

    End Sub

    Private Sub ComboBox107_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox107.SelectedIndexChanged
        If Not CheckBox101.Checked Then
            Return
        End If

        If (ComboBox107.SelectedItem Is Nothing) Then
            Label113.Text = ""
            Return
        End If

        Dim WPT As WPT_FIXType = ComboBox107.SelectedItem
        Label133.Text = GetNavTypeName(WPT.TypeCode)

        Dim InterNav As NavaidData
        Dim NextFIX As StepDownFIX
        Dim GuidNav As NavaidData



        InterNav = ComboBox104.SelectedItem
        NextFIX = StepDownFIXs(StepDownsNum - 1)
        GuidNav = NextFIX.GuidanceNav

        If (GuidNav.TypeCode = eNavaidType.DME) Then 'DME
            If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
                SetTexBox102Angle(NextFIX, InterNav, WPT)
                StepDownFIXs(StepDownsNum).WPT = WPT
            End If
        Else 'VOR, NDB
            Select Case InterNav.IntersectionType
                Case eIntersectionType.ByAngle
                    SetTexBox102Angle(NextFIX, InterNav, WPT)
                    StepDownFIXs(StepDownsNum).WPT = WPT
                Case eIntersectionType.ByDistance, eIntersectionType.RadarFIX
                    SetTextBox102Distance(NextFIX, InterNav, WPT)
                    StepDownFIXs(StepDownsNum).WPT = WPT
            End Select
        End If

    End Sub

    Private Sub SetTextBox102Distance(NextFIX As StepDownFIX, InterNav As NavaidData, WPT As WPT_FIXType)
        Dim x, y As Double
        PrjToLocal(InterNav.pPtPrj, NextFIX.InDir + 180.0, WPT.pPtPrj, x, y)
        OptionButton105.Enabled = False
        OptionButton106.Enabled = False
        If (x < 0) Then
            OptionButton105.Checked = False
            OptionButton106.Checked = True
        Else
            OptionButton105.Checked = True
            OptionButton106.Checked = False
        End If
        TextBox102.Text = CStr(ConvertDistance(ReturnDistanceInMeters(InterNav.pPtPrj, WPT.pPtPrj), 2))
        ValidateDMEDistance(x > 0)
    End Sub

    Private Sub SetTexBox102Angle(NextFIX As StepDownFIX, InterNav As NavaidData, WPT As WPT_FIXType)

        If InterNav.TypeCode = eNavaidType.NDB Then
            TextBox102.Text = CStr(Math.Round(Dir2Azt(InterNav.pPtPrj, ReturnAngleInDegrees(InterNav.pPtPrj, WPT.pPtPrj)) + 180.0# - InterNav.MagVar, 1))
        Else
            TextBox102.Text = CStr(Math.Round(Dir2Azt(InterNav.pPtPrj, ReturnAngleInDegrees(InterNav.pPtPrj, WPT.pPtPrj)) - InterNav.MagVar, 1))
        End If
        TextBox102_Validating(TextBox102, New CancelEventArgs())
    End Sub

    Private Function IsValid(WPT As WPT_FIXType) As Boolean
        Dim InterNav As NavaidData
        Dim NextFIX As StepDownFIX
        Dim GuidNav As NavaidData

        InterNav = ComboBox104.SelectedItem
        NextFIX = StepDownFIXs(StepDownsNum - 1)
        GuidNav = NextFIX.GuidanceNav

        If (GuidNav.TypeCode = eNavaidType.DME) Then 'DME
            If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
                Return IsInSector(InterNav, NextFIX, WPT)
            End If
            Return False
        Else 'VOR, NDB
            Select Case InterNav.IntersectionType
                Case eIntersectionType.ByAngle
                    Return IsInSector(InterNav, NextFIX, WPT)
                Case eIntersectionType.ByDistance, eIntersectionType.RadarFIX
                    Return IsInInterval(NextFIX, InterNav, WPT)
            End Select
            Return False
        End If
    End Function

    Private Function IsInInterval(NextFIX As StepDownFIX, InterNav As NavaidData, WPT As WPT_FIXType) As Boolean

        Dim x, y As Double
        Dim x1, y1 As Double
        Dim dist As Double
        Dim minDist, maxDist As Double

        PrjToLocal(NextFIX.pPtPrj, NextFIX.InDir + 180.0, WPT.pPtPrj, x, y)
        PrjToLocal(InterNav.pPtPrj, NextFIX.InDir + 180.0, WPT.pPtPrj, x1, y1)
        dist = ReturnDistanceInMeters(InterNav.pPtPrj, WPT.pPtPrj)
        If x1 > 0 Or InterNav.ValMin.Length = 1 Then
            minDist = InterNav.ValMin(0)
            maxDist = InterNav.ValMax(0)
        Else
            minDist = InterNav.ValMin(1)
            maxDist = InterNav.ValMax(1)
        End If

        If (dist > maxDist) Or (dist < minDist) Or (Math.Abs(y) > distError) Then
            Return False
        End If
        Return True
    End Function

    Private Function IsInSector(InterNav As NavaidData, NextFIX As StepDownFIX, WPT As WPT_FIXType) As Boolean
        Dim fDirl As Double
        Dim fTmp As Double
        Dim dMin As Double
        Dim dMax As Double
        Dim R As Double
        Dim squareDist As Double


        R = ReturnDistanceInMeters(InterNav.pPtPrj, NextFIX.pPtPrj)
        squareDist = ReturnSquareDistanceInMeters(InterNav.pPtPrj, WPT.pPtPrj)


        If ((squareDist > (R + distError) ^ 2) Or (squareDist < (R - distError) ^ 2)) Then
            Return False
        End If

        fDirl = Dir2Azt(InterNav.pPtPrj, ReturnAngleInDegrees(InterNav.pPtPrj, WPT.pPtPrj))

        dMin = Dir2Azt(InterNav.pPtPrj, InterNav.ValMax(0))
        dMax = Dir2Azt(InterNav.pPtPrj, InterNav.ValMin(0))
        fTmp = fDirl
        If Not AngleInSector(fDirl, dMin, dMax) Then
            If SubtractAngles(fDirl, dMin) < SubtractAngles(fDirl, dMax) Then
                fDirl = dMin
            Else
                fDirl = dMax
            End If
            If SubtractAngles(fTmp, fDirl) > degEps Then
                Return False
            End If
            Return True
        End If

        Return True
    End Function

    Private Sub CreateApproachGeometry()
        Dim I As Integer
        Dim Side As Integer
        Dim TurnDir As Integer

        Dim fTmp As Double
        Dim fIADir As Double
        Dim InterceptionType As Integer

        Dim pNominalPolyline As IPolyline
        Dim pNominalPolyline2 As IPolyline

        Dim TurnAngle As Double
        Dim LPerpend As Double
        Dim Lwarn As Double

        Dim Alpha As Double
        Dim SinA As Double
        Dim RDME As Double
        Dim dDME_DME As Double
        Dim dirDME_DME As Double
        Dim dirToDME1 As Double
        Dim dirToDME2 As Double
        Dim RDME1 As Double
        Dim RDME2 As Double
        Dim X As Double
        Dim Y As Double
        Dim distX As Double
        Dim distY As Double
        Dim dFrom As Double
        Dim dTo As Double
        Dim dReal As Double

        Dim DirFrom As Double
        Dim DirTo As Double
        Dim ptTmp As IPoint

        Dim pPtCntr As IPoint
        Dim pPtConstructor As IConstructPoint
        Dim pPtCollection As IPointCollection
        Dim pPtCollection2 As IPointCollection

        pNominalPolyline = New Polyline()

        TurnDir = 2 * ComboBox102.SelectedIndex - 1


        If OptionButton101.Checked Then
            If StepDownFIXs(StepDownsNum - 1).Track = TrackType.Straight Then


                pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart


                pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum - 1).pPtWarn
            Else
                fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj) '312.262013327147
                I = StepDownFIXs(StepDownsNum - 1).TurnDir
                If I = 0 Then
                    If Modulus(fTmp - StepDownFIXs(StepDownsNum - 1).OutDir, 360.0#) > 180.0# Then
                        Side = 1
                    Else
                        Side = -1
                    End If
                ElseIf I > 0 Then
                    Side = 1
                Else
                    Side = -1
                End If

                pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum - 1).pPtWarn, Side)
            End If
        Else
            InterceptionType = 2 * StepDownFIXs(StepDownsNum).Track + StepDownFIXs(StepDownsNum - 1).Track

            Select Case InterceptionType
                ''====================================================================================================
                Case 0
                    TurnAngle = (StepDownFIXs(StepDownsNum).InDir - StepDownFIXs(StepDownsNum - 1).InDir) * StepDownFIXs(StepDownsNum).TurnDir
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus(TurnAngle, 360)
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    Lwarn = arInitialApTurnRadius / Math.Tan(DegToRad(0.5 * (180 - TurnAngle)))
                    StepDownFIXs(StepDownsNum - 1).pPtWarn = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum - 1).InDir, Lwarn)
                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).InDir - 180, Lwarn)


                    pPtCntr = New ESRI.ArcGIS.Geometry.Point
                    pPtConstructor = pPtCntr
                    pPtConstructor.ConstructAngleIntersection(StepDownFIXs(StepDownsNum - 1).pPtWarn, DegToRad(StepDownFIXs(StepDownsNum - 1).InDir + 90), StepDownFIXs(StepDownsNum).pPtStart, DegToRad(StepDownFIXs(StepDownsNum).InDir + 90))

                    pPtCollection = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)

                    If StepDownFIXs(StepDownsNum - 1).Role = CodeProcedureFixRole.TP Or StepDownFIXs(StepDownsNum).Counted = 2 Then
                        pPtCollection.AddPoint(StepDownFIXs(StepDownsNum - 1).pPtStart, 0)
                    Else
                        PrjToLocal(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + 180.0, StepDownFIXs(StepDownsNum - 1).pPtWarn, distX, distY)
                        If (StepDownFIXs(StepDownsNum).Length >= distError And distX > 0) Then
                            pPtCollection.AddPoint(StepDownFIXs(StepDownsNum - 1).pPtStart, 0)
                        ElseIf (StepDownFIXs(StepDownsNum).Length >= distError) And StepDownsNum > 2 Then
                            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
                            pNominalPolyline2 = CreateNominalLineInWrongTurn()

                            StepDownFIXs(StepDownsNum - 2).pNominalPoly = pNominalPolyline2
                            StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 2).pNominalPoly, 255, 2)
                            StepDownFIXs(StepDownsNum - 2).pNominalElem.Locked = True
                            StepDownFIXs(StepDownsNum - 1).PropagateToPrevious = True
                        End If
                    End If

                    pNominalPolyline = pPtCollection
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
                Case 1
                    fIADir = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, ptKT1)

                    fTmp = fIADir - 90.0# * StepDownFIXs(StepDownsNum - 1).ArcDir
                    'TurnDir = 2 * (Modulus(StepDownFIXs(StepDownsNum).InDir - fTmp) > 180#) + 1
                    TurnDir = IIf(Modulus(StepDownFIXs(StepDownsNum).InDir - fTmp) > 180.0#, -1, 1)
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    fTmp = Modulus(Math.Abs(StepDownFIXs(StepDownsNum).InDir - fIADir))
                    If fTmp > 180.0# Then fTmp = 360.0# - fTmp

                    RDME = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, ptKT1)

                    Lwarn = RDME - arInitialApTurnRadius * StepDownFIXs(StepDownsNum - 1).ArcDir * TurnDir
                    LPerpend = Point2LineDistancePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, ptKT1, StepDownFIXs(StepDownsNum).InDir)

                    If LPerpend < distEps Then
                        SinA = -arInitialApTurnRadius / Lwarn
                        Alpha = RadToDeg(ArcSin(SinA)) * StepDownFIXs(StepDownsNum).ArcDir

                        If fTmp < 90 Then
                            fTmp = 0
                        Else
                            fTmp = 180
                        End If
                    Else
                        Side = SideDef(ptKT1, StepDownFIXs(StepDownsNum).InDir, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj)
                        SinA = (LPerpend - arInitialApTurnRadius * Side * TurnDir) / Lwarn

                        If fTmp < 90 Then
                            fTmp = 0
                            Alpha = RadToDeg(ArcSin(SinA)) * Side
                        Else
                            fTmp = 0
                            Alpha = 180 - RadToDeg(ArcSin(SinA)) * Side
                        End If
                    End If

                    pPtCntr = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).InDir + Alpha + fTmp, RDME - arInitialApTurnRadius * StepDownFIXs(StepDownsNum - 1).ArcDir * TurnDir)
                    StepDownFIXs(StepDownsNum - 1).pPtWarn = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).InDir + Alpha + fTmp, RDME)
                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(pPtCntr, StepDownFIXs(StepDownsNum).InDir + 90 * TurnDir, arInitialApTurnRadius)

                    If StepDownFIXs(StepDownsNum - 1).Role = CodeProcedureFixRole.TP Or StepDownFIXs(StepDownsNum).Counted = 2 Then
                        pPtCollection = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum - 1).ArcDir)
                        pPtCollection2 = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                        pPtCollection.AddPointCollection(pPtCollection2)
                    Else
                        If StepDownFIXs(StepDownsNum).Length >= distError Then
                            Dim turnsBefore As Boolean = False
                            If StepDownsNum > 2 Then
                                dTo = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 2).pPtPrj)
                                dFrom = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                                dReal = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                                If AngleInSector(dReal, dFrom, dTo) Then
                                    turnsBefore = True
                                End If
                            End If
                            If turnsBefore = False Then
                                pPtCollection = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum - 1).ArcDir)
                                pPtCollection2 = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                                pPtCollection.AddPointCollection(pPtCollection2)
                            Else
                                If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
                                pNominalPolyline2 = CreateNominalLineInWrongTurn()
                                StepDownFIXs(StepDownsNum - 2).pNominalPoly = pNominalPolyline2
                                StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 2).pNominalPoly, 255, 2)
                                StepDownFIXs(StepDownsNum - 2).pNominalElem.Locked = True
                                StepDownFIXs(StepDownsNum - 1).PropagateToPrevious = True
                                pPtCollection = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                            End If
                        Else

                            pPtCollection = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                        End If
                    End If

                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtEnd)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)

                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * StepDownFIXs(StepDownsNum).TurnDir)

                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
                    ''====================================================================================================
                Case 2
                    fIADir = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).pPtPrj)

                    fTmp = fIADir + 90 * StepDownFIXs(StepDownsNum).ArcDir
                    TurnDir = 2 * CShort(Modulus(StepDownFIXs(StepDownsNum - 1).InDir - fTmp, 360) > 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    fTmp = Modulus(StepDownFIXs(StepDownsNum - 1).InDir - fIADir, 360)
                    If fTmp > 180 Then
                        fTmp = 360 - fTmp
                    End If

                    RDME = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, ptKT1)
                    Lwarn = RDME - arInitialApTurnRadius * StepDownFIXs(StepDownsNum).ArcDir * TurnDir
                    LPerpend = Point2LineDistancePrj(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, ptKT1, StepDownFIXs(StepDownsNum - 1).InDir)

                    If LPerpend < distEps Then
                        SinA = arInitialApTurnRadius / Lwarn
                        Alpha = RadToDeg(ArcSin(SinA)) * StepDownFIXs(StepDownsNum).ArcDir

                        If fTmp < 90 Then
                            fTmp = 0
                        Else
                            fTmp = 180
                        End If
                    Else
                        Side = SideDef(ptKT1, StepDownFIXs(StepDownsNum - 1).InDir, StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj)
                        SinA = (LPerpend - arInitialApTurnRadius * Side * TurnDir) / Lwarn

                        If fTmp < 90 Then
                            fTmp = 0
                            Alpha = RadToDeg(ArcSin(SinA)) * Side
                        Else
                            fTmp = 0
                            Alpha = 180 - RadToDeg(ArcSin(SinA)) * Side
                        End If
                    End If

                    pPtCntr = PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + Alpha + fTmp, RDME - StepDownFIXs(StepDownsNum).ArcDir * TurnDir * arInitialApTurnRadius)

                    StepDownFIXs(StepDownsNum - 1).pPtWarn = PointAlongPlane(pPtCntr, StepDownFIXs(StepDownsNum - 1).InDir + 90 * TurnDir, arInitialApTurnRadius)
                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + Alpha + fTmp, RDME)


                    pPtCollection = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)

                    If StepDownFIXs(StepDownsNum - 1).Role = CodeProcedureFixRole.TP Or StepDownFIXs(StepDownsNum).Counted = 2 Then
                        pPtCollection.AddPoint(StepDownFIXs(StepDownsNum - 1).pPtStart, 0)
                    Else
                        PrjToLocal(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + 180, StepDownFIXs(StepDownsNum - 1).pPtWarn, distX, distY)
                        If (StepDownFIXs(StepDownsNum).Length >= distError And distX > 0) Then
                            pPtCollection.AddPoint(StepDownFIXs(StepDownsNum - 1).pPtStart, 0)
                        ElseIf (StepDownFIXs(StepDownsNum).Length >= distError) And StepDownsNum > 2 Then
                            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
                            pNominalPolyline2 = CreateNominalLineInWrongTurn()

                            StepDownFIXs(StepDownsNum - 2).pNominalPoly = pNominalPolyline2
                            StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 2).pNominalPoly, 255, 2)
                            StepDownFIXs(StepDownsNum - 2).pNominalElem.Locked = True
                            StepDownFIXs(StepDownsNum - 1).PropagateToPrevious = True
                        End If
                    End If

                    'If StepDownFIXs(StepDownsNum).Length > distEps Then
                    '    pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                    'End If


                    'pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum - 1).pPtWarn
                    'pPtCollection = pNominalPolyline

                    'pPtCollection2 = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)

                    'pPtCollection.AddPointCollection(pPtCollection2)
                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtEnd)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
                    ''====================================================================================================
                Case 3
                    fTmp = Modulus(StepDownFIXs(StepDownsNum).OutDir - StepDownFIXs(StepDownsNum).InDir, 360)
                    TurnDir = 2 * CShort(fTmp < 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    dDME_DME = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj)
                    dirDME_DME = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj)
                    RDME1 = ReturnDistanceInMeters(ptKT1, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj)
                    RDME2 = ReturnDistanceInMeters(ptKT1, StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj)

                    X = (RDME1 * RDME1 - RDME2 * RDME2 - 2 * arInitialApTurnRadius * TurnDir * (RDME1 * StepDownFIXs(StepDownsNum - 1).ArcDir - RDME2 * StepDownFIXs(StepDownsNum).ArcDir) + dDME_DME * dDME_DME) / (2 * dDME_DME)
                    fTmp = (RDME1 - arInitialApTurnRadius * TurnDir * StepDownFIXs(StepDownsNum - 1).ArcDir)
                    Y = Math.Sqrt(fTmp * fTmp - X * X)

                    Side = SideDef(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirDME_DME, ptKT1)
                    ptTmp = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirDME_DME, X)
                    pPtCntr = PointAlongPlane(ptTmp, dirDME_DME - 90 * Side, Y)

                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, pPtCntr)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, pPtCntr)

                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, dirToDME2, RDME2)
                    StepDownFIXs(StepDownsNum - 1).pPtWarn = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirToDME1, RDME1)

                    If StepDownFIXs(StepDownsNum).Length > distEps Then
                        pPtCollection = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum - 1).ArcDir)

                        pPtCollection2 = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)
                        pPtCollection.AddPointCollection(pPtCollection2)
                    Else
                        pPtCollection = CreateArcPolylinePrj(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)
                    End If
                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtEnd)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
            End Select

            StepDownFIXs(StepDownsNum - 1).pPtWarn.Z = ptKT1.Z
            StepDownFIXs(StepDownsNum).pPtStart.Z = ptKT1.Z
        End If

        ''''====================================================================================================
        StepDownFIXs(StepDownsNum - 1).pNominalPoly = pNominalPolyline
        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True
    End Sub

    Private Function CreateNominalLineInWrongTurn() As IPolyline
        Dim pNominalPolyline2 As IPolyline

        If StepDownFIXs(StepDownsNum - 2).GuidanceNav.TypeCode = eNavaidType.DME Then
            pNominalPolyline2 = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 2).pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtWarn, StepDownFIXs(StepDownsNum - 2).ArcDir)
        Else
            pNominalPolyline2 = New Polyline
            pNominalPolyline2.FromPoint = StepDownFIXs(StepDownsNum - 2).pPtPrj
            pNominalPolyline2.ToPoint = StepDownFIXs(StepDownsNum - 1).pPtWarn
        End If
        Return pNominalPolyline2
    End Function

    Private Function GetNumericalData() As List(Of GeoNumericalDataModel)
        Dim NumericalData As New List(Of GeoNumericalDataModel)

        Dim i, n As Integer
        Dim currLeg As StepDownFIX
        Dim GuidanceNav As NavaidData
        Dim IntersectNav As NavaidData

        n = UBound(LegList)

        currLeg = LegList(0)
        GuidanceNav = currLeg.GuidanceNav
        IntersectNav = currLeg.IntersectNav

        If (Not currLeg.FixStartPoint.Created) And (Not currLeg.FixStartPoint.OnNavaid) Then
            Dim IafSdfHorAccuracy As Double = CalcHorisontalAccuracy(currLeg.pPtPrj, GuidanceNav, IntersectNav)
            NumericalData.Add(New GeoNumericalDataModel With
            {
                .Role = IIf(bFirstPointIsIF, "IAF", "SDF"),
                .Accuracy = IafSdfHorAccuracy,
                .Resolution = 0.0,
                .DesignatorDescription = GetDesignatedPointDescription(currLeg.pPtPrj),
                .LegType = IIf(bFirstPointIsIF, "Initial", "Arrival")
            })
        End If
        '==================================================================================

        For i = 1 To n
            currLeg = LegList(i)

            If (currLeg.TurnDir = 0) And (currLeg.LegTypeARINC <> CodeSegmentPath.CI) Then
                Dim Role As String
                IntersectNav = currLeg.IntersectNav
                GuidanceNav = currLeg.GuidanceNav

                If i = n Then
                    If bFirstPointIsIF Then
                        Role = "IF"
                    Else
                        Role = "IAF"
                    End If
                Else
                    Role = "SDF"
                End If

                If (Not currLeg.FixStartPoint.Created) And (Not currLeg.FixStartPoint.OnNavaid) Then
                    Dim PtHorAccuracy As Double = CalcHorisontalAccuracy(currLeg.pPtPrj, GuidanceNav, IntersectNav)
                    NumericalData.Add(New GeoNumericalDataModel With
                    {
                        .Role = Role,
                        .Accuracy = PtHorAccuracy,
                        .Resolution = 0.0,
                        .DesignatorDescription = GetDesignatedPointDescription(currLeg.pPtPrj),
                        .LegType = IIf(bFirstPointIsIF, "Initial", "Arrival")
                    })
                End If
            End If

        Next i

        Return NumericalData
    End Function

    Private Sub TextBox201_KeyPress(sender As Object, eventArgs As KeyPressEventArgs) Handles TextBox201.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox201_Validating(TextBox201, New CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox201.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub saveRepBtn_Click(sender As Object, e As EventArgs) Handles SaveReportBtn.Click
        SaveReports()
    End Sub

    Private Sub SaveReports()
        Dim pReport As ReportHeader
        Dim RepFileName As String

		If ShowSaveDialog(RepFileName, RepFileTitle) Then
			pReport.Aerodrome = ""

			pReport.Database = gAranEnv.ConnectionInfo.Database
			pReport.Procedure = IO.Path.GetFileName(RepFileName)
			pReport.Category = Chr(Category + Asc("A"))

			SaveAccuracy(RepFileName, RepFileTitle, pReport)
			'SaveLog(RepFileName, RepFileTitle, pReport)
			SaveProtocol(RepFileName, RepFileTitle, pReport)

			DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml")
		End If
	End Sub

    Private Sub CkbVirtualDP_CheckedChanged(sender As Object, e As EventArgs) Handles CkbVirtualDP.CheckedChanged
        Dim i As Integer
        Dim N As Integer = WPTList.Length
        Dim navaidLength As Integer = NavaidList.Length
        If (CkbVirtualDP.Checked) Then

            ReDim Preserve NavaidList(navaidLength + N - 1)
            For i = 0 To N - 1
                Dim wpt As WPT_FIXType = WPTList(i)
                If (wpt.pPtPrj Is Nothing) Then Continue For
                Dim tmpNavaid As NavaidData = New NavaidData()
                tmpNavaid.TypeCode = eNavaidType.NDB
                tmpNavaid.Name = wpt.Name
                tmpNavaid.CallSign = wpt.Name
                tmpNavaid.pPtGeo = wpt.pPtGeo
                tmpNavaid.pPtPrj = wpt.pPtPrj
                tmpNavaid.Range = 15000
                NavaidList(navaidLength + i) = tmpNavaid
            Next
        Else
            ReDim Preserve NavaidList(navaidLength - N - 1)
        End If

        FillComboBox101()
        If (ComboBox101.Items.Count > -1) Then
            ComboBox101.SelectedIndex = 0
        End If
    End Sub

End Class
