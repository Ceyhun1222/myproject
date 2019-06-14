Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports Aran.Aim.Features
Imports Aran.Aim.Enums
Imports Aran.Aim.DataTypes
Imports Aran.Geometries
Imports Aran.Queries
Imports Aran.Aim
Imports Aran.Converters
Imports ESRI.ArcGIS.Geometry
Imports Aran.Metadata.Utils
Imports Aran.AranEnvironment

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CInitialApproach
    Inherits System.Windows.Forms.Form

    Private Structure IAPArrayItem
        Dim LegIndex As Integer
        Dim pIAP As InstrumentApproachProcedure
        Dim pProcedureTransition As ProcedureTransition
    End Structure

    Private Const BigDist As Double = 10000000.0
    Private Const IntDist As Double = 150000.0

    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

    Private pNominalElement As ESRI.ArcGIS.Carto.IElement
    Private pIAF_IIAreaElement As ESRI.ArcGIS.Carto.IElement
    Private pIAF_IAreaElement As ESRI.ArcGIS.Carto.IElement

    Private pFAFptElement As ESRI.ArcGIS.Carto.IElement
    Private pFAFAreaElement As ESRI.ArcGIS.Carto.IElement

    Private ptElem As ESRI.ArcGIS.Carto.IElement
    Private pPlyElem As ESRI.ArcGIS.Carto.IElement
    '==========================================
    Private StepDownsNum As Integer
    Private UboundFIXs As Integer
    Private StepDownFIXs() As StepDownFIX
    Private LegList() As StepDownFIX

    Private StartPoint As WPT_FIXType
    'Private LocalADHPList() As ADHPType
    'Private CurrADHP As ADHPType

    Private TextBox110Range As Interval
    Private fFIXHeight As Double

    Private pFAFptPrj As ESRI.ArcGIS.Geometry.IPoint
    Private bFirstPointIsIF As Boolean
    Private bIsHolding As Boolean

    Private pIFPoint As TerminalSegmentPoint
    Private pIFptGeo As ESRI.ArcGIS.Geometry.IPoint
    Private pIFptPrj As ESRI.ArcGIS.Geometry.IPoint
    Private pIFTolerArea As ESRI.ArcGIS.Geometry.IPolygon
    Private pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection

    Private MOCLimit As Double
    Private fRefHeight As Double
    Private IAF_PDG As Double
    Private arMinISlen As Double

    Private arHalfWithConst1 As Double
    Private arHalfWithConst2 As Double

    'Private IAFObstList4FIXdMin15() As Double

    Private MinISlensArray(5) As Double
    Private TurnIntervalsL(5) As TurnStruct
    Private TurnIntervalsR(5) As TurnStruct

    Private m_IAF_IIAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private m_IAF_IAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private m_pNominalPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private ptKT1 As ESRI.ArcGIS.Geometry.IPoint

    Private pProcedure As Procedure

    Private IAFNavDat() As NavaidData
    Private SDFInterNavs() As NavaidData
    Private IAFProhibSectors() As IFProhibitionSector

    Private m_IAFObstList As ObstacleContainer
    Private IAFObstList4FIX As ObstacleContainer
    Private IAFObstList4Turn As ObstacleContainer

    Private TurnComboList() As String = {"0-90", "91-96", "97-102", "103-108", "109-114", "115-120"}

    Private dDMin As Double
    Private ProcedureName As String

    Private IAPArray() As InstrumentApproachProcedure
    Private TransArray() As IAPArrayItem
    Private SelectedTransition As IAPArrayItem

    Private Category As Integer
    Private pLandingTakeoff As LandingTakeoffAreaCollection

    Private InitialReportsFrm As CInitialReportsFrm

    Private HelpContextID As Integer = 12100
    Private bFormInitialised As Boolean = False
    Private PageLabel() As System.Windows.Forms.Label

    Private Sub FocusStepCaption(ByVal StIndex As Integer)
        Dim I As Integer
        For I = 0 To 1
            PageLabel(I).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
            PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
        Next

        PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
        PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

        Me.Text = My.Resources.str00003 + " [" + MultiPage1.TabPages.Item(StIndex).Text + "]"   '+ " " + My.Resources.str00818 
    End Sub

#Region "Form"
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
        Me.HelpContextID = 12100        ' SetComboDroppedWidth(ComboBox001, 2 * VB6.PixelsToTwipsX(ComboBox001.Width) / VB6.TwipsPerPixelX)

        ComboBox004.SelectedIndex = 0
        UboundFIXs = 99

        ReDim StepDownFIXs(UboundFIXs)

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim NN As Integer
        Dim IL As Integer
        Dim bTransIsValid As Boolean

        Dim pIAP As InstrumentApproachProcedure
        Dim pIAPList As List(Of InstrumentApproachProcedure)
        Dim pProcedureLeg As SegmentLeg
        Dim pProcedureTransition As ProcedureTransition
        Dim lastMID As Guid

        mocLimitCmbBox.Items.Clear()

        For I = 0 To UBound(EnrouteMOCValues)
            mocLimitCmbBox.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(I), 4)))
        Next I
        mocLimitCmbBox.SelectedIndex = 0

        ComboBox002.Items.Clear()

        pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)

        N = pIAPList.Count
        NN = N + N

        ReDim IAPArray(NN)
        K = -1

        lastMID = New Guid()
        '==========================================================================================

        For I = 0 To N - 1
            pIAP = pIAPList.Item(I)

            If pIAP.Name Is Nothing Then Continue For

            If (InStr(1, pIAP.Name, "RNAV", vbTextCompare) = 0) And (pIAP.Identifier <> lastMID) Then   'Set pProcedureTransitionList = pObjectDir.GetProcedureTranstionList(pIAP.identifier)
                M = pIAP.FlightTransition.Count
                For J = 0 To M - 1
                    pProcedureTransition = pIAP.FlightTransition.Item(J)    'pProcedureTransitionList.Item(J)
                    'Set pProcedureLegList = pObjectDir.GetSegmentLegListByTransition(CStr(pProcedureTransition.ID))
                    L = pProcedureTransition.TransitionLeg.Count            'pProcedureLegList.Count

                    If L > 0 Then
                        If pProcedureTransition.Type.Value = CodeProcedurePhase.APPROACH Then
                            K = K + 1
                            IAPArray(K) = pIAP
                            ComboBox002.Items.Add(pIAP.Name)
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

                                If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
                                    K = K + 1
                                    IAPArray(K) = pIAP
                                    ComboBox002.Items.Add(pIAP.Name)
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
            '==========================================================================================
            'Set pIAP = pIAPList.Item(I)
            'Set pProcedureLegList = pObjectDir.GetProcedureLegListByProcedure(pIAP.Mid)

            'M = pProcedureLegList.Count
            'If M > 0 Then
            '			Set pProcedureLeg = pProcedureLegList.Item(0)
            '			If (pProcedureLeg.CodeRoleFix = "FAF") Or (pProcedureLeg.CodeRoleFix = "FAP") Then  'Or (pProcedureLeg.CodeRoleFix = "IF")
            '				K = K + 1
            '				Set IAPArray(K) = pIAP
            '				ComboBox002.AddItem pIAP.Designator
            '			End If
            'End If
            '==========================================================================================
        Next I

        If K >= 0 Then
            ReDim Preserve IAPArray(K)
        End If

        If ComboBox002.Items.Count > 0 Then
            ComboBox002.SelectedIndex = 0
        End If

        '====================================================

        SetComboDroppedWidth(ComboBox002, 1.5 * ComboBox002.Width)

        pGraphics = GetActiveView().GraphicsContainer
        Frame102.Top = Frame101.Top
        Frame102.Left = Frame101.Left

        '===============================================
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
        arHalfWithUnitLbl.Text = DistanceConverter(DistanceUnit).Unit
        mocLimitUnitLbl.Text = HeightConverter(HeightUnit).Unit

        '===============================================
        PrevBtn.Text = My.Resources.str00150
        NextBtn.Text = My.Resources.str00151
        OkBtn.Text = My.Resources.str00152
        CancelBtn.Text = My.Resources.str00153
        ReportBtn.Text = My.Resources.str00154

        Me.Text = My.Resources.str00003
        MultiPage1.TabPages.Item(0).Text = My.Resources.str40001
        MultiPage1.TabPages.Item(1).Text = My.Resources.str40002
        MultiPage1.TabPages.Item(2).Text = My.Resources.str40003

        Frame001.Text = My.Resources.str40100
        Label001.Text = My.Resources.str40101
        Label002.Text = My.Resources.str40102
        Label003.Text = My.Resources.str40103
        Label004.Text = My.Resources.str10406
        Label006.Text = My.Resources.str00232
        Label008.Text = My.Resources.str40106
        Label010.Text = My.Resources.str40107
        Label012.Text = My.Resources.str40108
        Label014.Text = My.Resources.str10107

        OptionButton101.Text = My.Resources.str40201
        OptionButton102.Text = My.Resources.str40202

        Frame101.Text = My.Resources.str40222
        Frame102.Text = My.Resources.str40222

        Frame103.Text = My.Resources.str40200

        Label101.Text = My.Resources.str40203
        Label105.Text = My.Resources.str40217
        Label107.Text = My.Resources.str40216
        Label108.Text = My.Resources.str40215
        Label110.Text = My.Resources.str40108
        Label113.Text = My.Resources.str10503
        Label115.Text = My.Resources.str40106
        Label117.Text = My.Resources.str40209
        Label118.Text = My.Resources.str40210

        Label124.Text = My.Resources.str10202
        Label126.Text = My.Resources.str10208

        RemoveBtn.Text = My.Resources.str40213
        AddBtn.Text = My.Resources.str40214

        '===========================================================
        MultiPage1.Tag = "1"
        MultiPage1.SelectedIndex = 0
        MultiPage1.Tag = "0"

        ComboBox102.Items.Clear()
        ComboBox102.Items.Add(My.Resources.str40218)
        ComboBox102.Items.Add(My.Resources.str40219)
        ComboBox102.SelectedIndex = 0

        arHalfWithConst1 = 9260.0#
        arHalfWithConst2 = 14816.0#

        arHalfWithCmbBox.Items.Clear()
        arHalfWithCmbBox.Items.Add(CStr(ConvertDistance(arHalfWithConst1, 2)))
        arHalfWithCmbBox.Items.Add(CStr(ConvertDistance(arHalfWithConst2, 2)))
        arHalfWithCmbBox.SelectedIndex = 0


        ReDim IAFProhibSectors(-1)
        'SetFormParented(Handle.ToInt32)

        '====================================================

        pGraphics = GetActiveView().GraphicsContainer

        InitialReportsFrm.SetReportBtn(ReportBtn)

        '' ====== 2007
        PageLabel = {Label1_0, Label1_1, Label1_2}

        PageLabel(0).Text = MultiPage1.TabPages.Item(0).Text
        PageLabel(1).Text = MultiPage1.TabPages.Item(1).Text
        PageLabel(2).Text = MultiPage1.TabPages.Item(2).Text

        FocusStepCaption(0)

        MultiPage1.Top = -21
        Me.Height = Me.Height - 21
        Frame1.Top = Frame1.Top - 21

        ShowPanelBtn.Checked = False
    End Sub

    Private Sub CInitialApproach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        DBModule.CloseDB()
        ClearScr()
        InitialReportsFrm.Close()
        CurrCmd = 0

        pGraphics = Nothing
        pIAF_IIAreaElement = Nothing
        pIAF_IAreaElement = Nothing
        pNominalElement = Nothing

        pFAFptElement = Nothing
        pFAFptPrj = Nothing

        pIFptGeo = Nothing
        pIFptPrj = Nothing
        pGuidPoly = Nothing
        m_IAF_IIAreaPoly = Nothing
        m_IAF_IAreaPoly = Nothing
        m_pNominalPoly = Nothing

        Erase IAFNavDat
        Erase SDFInterNavs
        Erase IAFProhibSectors
        Erase m_IAFObstList.Parts
        Erase IAFObstList4FIX.Parts
        Erase IAFObstList4Turn.Parts
    End Sub

    Private Sub CInitialApproach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F1 Then
            HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
            e.Handled = True
        End If
    End Sub


    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)

        ' Get a handle to a copy of this form's system (window) menu
        Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
        ' Add a separator
        AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
        ' Add the About menu item
        AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
        MyBase.WndProc(m)

        ' Test if the About item was selected from the system menu
        If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
            Dim about As AboutForm = New AboutForm()

            about.ShowDialog(Me)
            about = Nothing
        End If
    End Sub

#End Region

    Private Function CalcL(ByRef Alpha As Double, ByRef Betta As Double, ByRef RDME As Double, ByRef rDMEObs As Double) As Double
        CalcL = System.Math.Sqrt(RDME * RDME + rDMEObs * rDMEObs - 2 * RDME * rDMEObs * System.Math.Cos(Alpha - Betta))
    End Function

    Private Function CalcReffectiv(ByRef Alpha As Double, ByRef hIF As Double, ByRef Hi As Double, ByRef RDME As Double, ByRef PDG As Double, ByRef C155 As Double) As Double
        CalcReffectiv = arIFHalfWidth.Value - C155 * (hIF - Hi + Alpha * RDME * PDG)
    End Function

    Private Function AlphaR(ByRef Reffectiv As Double, ByRef hIF As Double, ByRef Hi As Double, ByRef RDME As Double, ByRef PDG As Double, ByRef C155 As Double) As Double
        AlphaR = ((arIFHalfWidth.Value - Reffectiv) / C155 - hIF + Hi) / (RDME * PDG)
    End Function

    Private Function AlphaL(ByRef Betta As Double, ByRef RDME As Double, ByRef rDMEObs As Double, ByRef L As Double) As Double
        AlphaL = Betta - ArcCos((RDME * RDME + rDMEObs * rDMEObs - L * L) / (2 * RDME * rDMEObs))
    End Function

    Private Function CreateGuidPoly(ByRef GuidNav As NavaidData, ByRef ptFIX As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.Polygon
        Dim fDist As Double
        Dim fTmp As Double
        Dim fSlDist As Double
        Dim fRadius As Double
        Dim fIADir As Double
        Dim TrackToler As Double
        Dim TrackRange As Double

        Dim pBaseLine As ESRI.ArcGIS.Geometry.ILine
        Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        If GuidNav.TypeCode = eNavaidType.DME Then
            pBaseLine = New ESRI.ArcGIS.Geometry.Line
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = ptFIX
            fDist = pBaseLine.Length

            fTmp = ptFIX.Z - GuidNav.pPtPrj.Z
            fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)

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

            TrackRange = GuidNav.Range / System.Math.Cos(DegToRad(TrackToler))

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

    Private Function GetIAFTurnNavs(ByRef ptIF As ESRI.ArcGIS.Geometry.IPoint) As NavaidData()
        Dim I As Integer
        Dim N As Integer
        Dim M As Integer
        Dim K As Integer
        Dim fTmp As Double
        Dim fDist As Double
        Dim ConRad As Double
        Dim NavDists(4) As Double
        Dim Result() As NavaidData

        K = -1
        N = UBound(NavaidList)
        M = UBound(DMEList)
        K = N + M + 1
        If K < 0 Then
            ReDim Result(-1)
            Return Result
        Else
            ReDim Result(K)
        End If

        NavDists(0) = arIFHalfWidth.Value / System.Math.Tan(DegToRad(VOR.TrackingTolerance)) 'Tan(DegToRad(VOR.SplayAngle))
        NavDists(1) = 13000.0
        NavDists(2) = arIFHalfWidth.Value / System.Math.Tan(DegToRad(NDB.TrackingTolerance)) 'Tan(DegToRad(NDB.SplayAngle))
        NavDists(3) = 0.0
        NavDists(4) = NavDists(0)

        K = -1

        For I = 0 To N
            fDist = ReturnDistanceInMeters(ptIF, NavaidList(I).pPtPrj)
            If (fDist < NavDists(NavaidList(I).TypeCode)) And (fDist < NavaidList(I).Range) Then
                If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                    ConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (ptIF.Z - NavaidList(I).pPtPrj.Z + fRefHeight)
                    fTmp = VOR.OnNAVRadius
                ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                    ConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (ptIF.Z - NavaidList(I).pPtPrj.Z + fRefHeight)
                    fTmp = NDB.OnNAVRadius
                ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
                    ConRad = 250000.0
                    fTmp = -10000.0
                End If

                If (fDist > ConRad) Or (fDist < fTmp) Then
                    K = K + 1
                    Result(K) = NavaidList(I)
                End If
            End If
        Next I

        For I = 0 To M
            fDist = ReturnDistanceInMeters(ptIF, DMEList(I).pPtPrj)
            If (fDist > NavDists(DMEList(I).TypeCode)) And (fDist < DMEList(I).Range) Then
                K = K + 1
                Result(K) = DMEList(I)
            End If
        Next I

        If K < 0 Then
            ReDim Result(-1)
        Else
            ReDim Preserve Result(K)
        End If

        Return Result
    End Function

    Private Function CreateFixZone(ByRef ptFIX As ESRI.ArcGIS.Geometry.IPoint, ByRef GuidNav As NavaidData, ByRef InterNav As NavaidData, ByVal bOnNav As Boolean, ByVal fIADir As Double, ByRef dD As Double) As ESRI.ArcGIS.Geometry.Polygon
        Dim I As Integer

        Dim fDir As Double
        Dim hFix As Double
        Dim fTmp As Double
        Dim fDist As Double
        Dim fSlDist As Double
        Dim fRadius As Double
        Dim fInterDir As Double
        Dim fInterToler As Double
        Dim fInterRange As Double

        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pBaseLine As ESRI.ArcGIS.Geometry.ILine
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pIntersectPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pResGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection

        '=======================================================================================
        If InterNav.TypeCode <= eNavaidType.NONE Then Return New ESRI.ArcGIS.Geometry.Polygon

        hFix = fFIXHeight 'ptFIX.Z
        pBaseLine = New ESRI.ArcGIS.Geometry.Line
        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        If GuidNav.TypeCode = eNavaidType.DME Then
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = ptFIX
            fDist = pBaseLine.Length
            fDir = RadToDeg(pBaseLine.Angle)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fDir + 90.0, 3 * fDist)
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fDir - 90.0, 3 * fDist)

            pTopoOper = pGuidPoly
            pTopoOper.Cut(pCutter, pTmpPoly, CreateFixZone)

            pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
            fDist = ReturnDistanceInMeters(InterNav.pPtPrj, ptFIX)
            fInterDir = ReturnAngleInDegrees(InterNav.pPtPrj, ptFIX)

            If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                fInterToler = VOR.IntersectingTolerance
            ElseIf InterNav.TypeCode = eNavaidType.NDB Then
                fInterToler = NDB.IntersectingTolerance
            ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
                fInterToler = LLZ.IntersectingTolerance
            End If

            pIntersectPoly.AddPoint(InterNav.pPtPrj)
            pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir - fInterToler, 3.0 * fDist))
            pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir + fInterToler, 3.0 * fDist))
            pIntersectPoly.AddPoint(InterNav.pPtPrj)

            pTopoOper = pIntersectPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            CreateFixZone = pTopoOper.Intersect(pTmpPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            '=======================================================================================
        Else
            '=======================================================================================
            pBaseLine.FromPoint = InterNav.pPtPrj
            pBaseLine.ToPoint = ptFIX
            fDir = RadToDeg(pBaseLine.Angle)
            fDist = pBaseLine.Length

            pTopoOper = pGuidPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
            '================================================
            If InterNav.TypeCode = eNavaidType.DME Then
                fTmp = SubtractAngles(fIADir, fDir)

                fTmp = hFix - InterNav.pPtPrj.Z
                fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)
                fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
                fRadius = fDist + fTmp
                pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, fDir + 90.0, 2 * fRadius)
                pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, fDir - 90.0, 2 * fRadius)
                '            If Side > 0 Then
                ClipByLine(CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing)
                '            Else
                '                ClipByLine CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, Nothing, pTmpPoly, Nothing
                '            End If
                'DrawPolygon CreatePrjCircle(InterNav.pPtPrj, fRadius), 0
                'DrawPolyLine pCutter, 255, 2

                fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
                fRadius = fDist - fTmp
                pTopoOper = pTmpPoly
                pIntersectPoly = pTopoOper.Difference(CreatePrjCircle(InterNav.pPtPrj, fRadius))
                'DrawPolygon CreatePrjCircle(InterNav.pPtPrj, fRadius), 255
            Else
                '================================================
                If bOnNav Or (fDist < distEps) Then
                    bOnNav = True
                    fDir = fIADir 'ptFIX.M 'fInitDir
                    If InterNav.TypeCode = eNavaidType.NDB Then
                        NDBFIXTolerArea(InterNav.pPtPrj, fDir, hFix, pIntersectPoly)
                    Else
                        VORFIXTolerArea(InterNav.pPtPrj, fDir, hFix, pIntersectPoly)
                    End If
                Else
                    If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                        fInterToler = VOR.IntersectingTolerance
                    ElseIf InterNav.TypeCode = eNavaidType.NDB Then
                        fInterToler = NDB.IntersectingTolerance
                    ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
                        fInterToler = LLZ.IntersectingTolerance
                    End If
                    fInterRange = InterNav.Range / System.Math.Cos(DegToRad(fInterToler))

                    pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
                    pIntersectPoly.AddPoint(InterNav.pPtPrj)
                    pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler, 100.0 * fInterRange))
                    pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler, 100.0 * fInterRange))
                    pIntersectPoly.AddPoint(InterNav.pPtPrj)
                    pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler + 180.0, 100.0 * fInterRange))
                    pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler + 180.0, 100.0 * fInterRange))
                    pIntersectPoly.AddPoint(InterNav.pPtPrj)
                End If
            End If

            If Not bOnNav Then
                pTopoOper = pIntersectPoly
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()
                CreateFixZone = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            Else
                CreateFixZone = pIntersectPoly
            End If
            '================================================
            ptTmp = PointAlongPlane(ptFIX, fIADir + 180.0, 100000.0)
            pCutter.FromPoint = PointAlongPlane(ptTmp, fIADir + 90.0, 100000.0)
            pCutter.ToPoint = PointAlongPlane(ptTmp, fIADir - 90.0, 100000.0)
            pProxi = CreateFixZone
            'DrawPolygon pProxi, 0
            'DrawPolyLine pCutter, 255, 2
            dD = 100000.0 - pProxi.ReturnDistance(pCutter)
        End If
        '=======================================================================================
        'DrawPolygon CreateFixZone, 0
        'DrawPolygon pTmpPoly, 255
        pGeomCollection = CreateFixZone

        If ((GuidNav.TypeCode = eNavaidType.DME) Or (InterNav.TypeCode = eNavaidType.DME)) And (pGeomCollection.GeometryCount > 1) Then
            pResGeomCollection = pTmpPoly
            pRelation = CreateFixZone
            I = 0
            While I < pGeomCollection.GeometryCount
                If pResGeomCollection.GeometryCount > 0 Then
                    pResGeomCollection.RemoveGeometries(0, pResGeomCollection.GeometryCount)
                End If
                pResGeomCollection.AddGeometry(pGeomCollection.Geometry(I))

                If pRelation.Contains(ptFIX) Then
                    I = I + 1
                Else
                    pGeomCollection.RemoveGeometries(I, 1)
                End If
            End While
        End If

        If GuidNav.TypeCode = eNavaidType.DME Then
            dD = 0.0
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = ptFIX
            fDir = pBaseLine.Angle
            pIntersectPoly = CreateFixZone
            For I = 0 To pIntersectPoly.PointCount - 1
                pBaseLine.ToPoint = pIntersectPoly.Point(I)
                fTmp = Modulus((pBaseLine.Angle - fDir) * (2 * ComboBox102.SelectedIndex - 1), 2 * PI)
                If fTmp > PI Then fTmp = fTmp - 2 * PI
                If fTmp > dD Then dD = fTmp
            Next I
            dD = dD * fDist
        End If

        pBaseLine = Nothing
        pCutter = Nothing
        pIntersectPoly = Nothing
        pTmpPoly = Nothing
    End Function

    Private Shared Function CreateArca(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef Direction As Double, ByRef Radius As Double) As ESRI.ArcGIS.Geometry.Polyline
        Dim I As Integer
        Dim iInRad As Double
        Dim Pt As ESRI.ArcGIS.Geometry.IPoint
        Dim pPolyline As ESRI.ArcGIS.Geometry.IPointCollection

        Pt = New ESRI.ArcGIS.Geometry.Point
        CreateArca = New ESRI.ArcGIS.Geometry.Polyline
        pPolyline = CreateArca

        For I = 0 To 179
            iInRad = DegToRad(I + Direction - 90.0)
            Pt.X = ptCnt.X + Radius * System.Math.Cos(iInRad)
            Pt.Y = ptCnt.Y + Radius * System.Math.Sin(iInRad)
            pPolyline.AddPoint(Pt)
        Next I
    End Function

    Private Function GetSDFInterNavs(ByRef PtSDF As ESRI.ArcGIS.Geometry.IPoint, ByRef fIF_SDFDist As Double) As NavaidData()
        Dim Side As Integer
        Dim Ik As Integer
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer

        Dim M As Integer
        Dim N As Integer

        Dim fInterToler As Double
        Dim fInterRange As Double
        Dim fConeAngle As Double
        Dim fOnNavRad As Double
        Dim fInterDir As Double
        Dim svGuyrug As Double
        Dim fRadius As Double
        Dim fRConus As Double
        Dim fSlDist As Double
        Dim fIADir As Double
        Dim ConRad As Double
        Dim fAlpha As Double
        Dim fDist As Double
        Dim dPrec As Double
        Dim fDir As Double
        Dim hFix As Double
        Dim fTmp As Double

        Dim Result() As NavaidData
        Dim GuidNav As NavaidData

        Dim pGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim pIntersectPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly2 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pOuterArc As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pInnerArc As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pFixPoly As ESRI.ArcGIS.Geometry.IPointCollection

        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

        Dim pRightRay As ESRI.ArcGIS.Geometry.IPolyline
        Dim pLeftRay As ESRI.ArcGIS.Geometry.IPolyline
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pBaseLine As ESRI.ArcGIS.Geometry.ILine
        Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

        '================================================
        N = UBound(NavaidList)
        M = UBound(DMEList)
        K = N + M + 1

        If K < 0 Then
            ReDim Result(-1)
            Return Result
        Else
            ReDim Result(K)
        End If

        If IsNumeric(TextBox105.Text) Then
            dPrec = DeConvertDistance(CDbl(TextBox105.Text))
        Else
            dPrec = arIFTolerance.Value
        End If

        If OptionButton101.Checked Then
            GuidNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav
            fIADir = StepDownFIXs(StepDownsNum - 1).InDir
        Else
            GuidNav = IAFNavDat(ComboBox101.SelectedIndex)
            fIADir = Modulus(ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, GuidNav.pPtPrj) + 180.0 * (1 - ComboBox102.SelectedIndex), 360.0)
        End If

        hFix = fFIXHeight 'StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + fIF_SDFDist * IAF_PDG 'StepDownFIXs(StepDownsNum - 1).PDG
        K = -1

        pBaseLine = New ESRI.ArcGIS.Geometry.Line
        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        If GuidNav.TypeCode = eNavaidType.DME Then
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = PtSDF
            fDir = RadToDeg(pBaseLine.Angle)
            fDist = pBaseLine.Length

            fTmp = hFix - GuidNav.pPtPrj.Z
            fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)

            fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
            fRadius = fDist + fTmp
            pOuterArc = CreateArca(GuidNav.pPtPrj, fDir, fRadius)

            fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
            fRadius = fDist - fTmp
            pInnerArc = CreateArca(GuidNav.pPtPrj, fDir, fRadius)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fDir + 90.0, 2 * fRadius)
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fDir - 90.0, 2 * fRadius)

            pTopoOper = pGuidPoly 'pTmpPoly1
            pTopoOper.Cut(pCutter, pTmpPoly2, pTmpPoly1)

            'DrawPolygon pGuidPoly, 255

            fAlpha = RadToDeg(dPrec / fDist)
            pt1 = PointAlongPlane(GuidNav.pPtPrj, fDir + fAlpha, fDist)
            pt2 = PointAlongPlane(GuidNav.pPtPrj, fDir - fAlpha, fDist)

            pCutter.FromPoint = pt1
            pCutter.ToPoint = pt2

            pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
            pLeftRay = New ESRI.ArcGIS.Geometry.Polyline
            pRightRay = New ESRI.ArcGIS.Geometry.Polyline

            For I = 0 To N
                fDist = ReturnDistanceInMeters(PtSDF, NavaidList(I).pPtPrj)
                fInterDir = ReturnAngleInDegrees(NavaidList(I).pPtPrj, PtSDF)

                If (fDist < NavaidList(I).Range) And (NavaidList(I).TypeCode <> eNavaidType.LLZ) Then
                    If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                        ConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                        fOnNavRad = VOR.OnNAVRadius
                        fInterToler = VOR.IntersectingTolerance
                        fConeAngle = VOR.ConeAngle
                    ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                        ConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                        fOnNavRad = NDB.OnNAVRadius
                        fInterToler = NDB.IntersectingTolerance
                        fConeAngle = NDB.ConeAngle
                    End If

                    pLeftRay.FromPoint = NavaidList(I).pPtPrj
                    pRightRay.FromPoint = NavaidList(I).pPtPrj

                    pLeftRay.ToPoint = PointAlongPlane(NavaidList(I).pPtPrj, fInterDir + fInterToler, 3.0 * fDist)
                    pRightRay.ToPoint = PointAlongPlane(NavaidList(I).pPtPrj, fInterDir - fInterToler, 3.0 * fDist)

                    'DrawPolyLine pLeftRay, 0
                    'DrawPolyLine pRightRay, 255

                    pRelation = pCutter
                    If pRelation.Crosses(pLeftRay) And pRelation.Crosses(pRightRay) Then
                        pRelation = pLeftRay
                        If pRelation.Crosses(pInnerArc) And pRelation.Crosses(pOuterArc) Then
                            pRelation = pRightRay
                            If pRelation.Crosses(pInnerArc) And pRelation.Crosses(pOuterArc) Then
                                If pIntersectPoly.PointCount > 0 Then pIntersectPoly.RemovePoints(0, pIntersectPoly.PointCount)

                                pFixPoly = CreateFixZone(PtSDF, GuidNav, NavaidList(I), False, fIADir, svGuyrug)
                                pRelation = pFixPoly
                                'DrawPolygon pFixPoly, 0
                                pGeomCollection = pFixPoly
                                If pGeomCollection.GeometryCount > 0 Then
                                    pProxi = pFixPoly
                                    fTmp = pProxi.ReturnDistance(NavaidList(I).pPtPrj)
                                    If fTmp > ConRad Then

                                        Ik = -1
                                        M = UBound(IAFObstList4FIX.Parts)

                                        For J = 0 To M
                                            If pRelation.Contains(IAFObstList4FIX.Parts(J).pPtPrj) Or (IAFObstList4FIX.Parts(J).Dist < fIF_SDFDist) Then
                                                If IAFObstList4FIX.Parts(J).hPenet > 0.0 Then
                                                    fTmp = fIF_SDFDist + svGuyrug - IAFObstList4FIX.Parts(J).Dist
                                                    If fTmp > arFIX15PlaneRang.Value Then
                                                        Ik = J
                                                        Exit For
                                                    End If

                                                    fTmp = (PtSDF.Z - IAFObstList4FIX.Parts(J).ReqH) / fTmp
                                                    If fTmp < arFixMaxIgnorGrd.Value Then
                                                        Ik = J
                                                        Exit For
                                                    End If
                                                End If
                                            End If

                                            If IAFObstList4FIX.Parts(J).Dist > fIF_SDFDist + svGuyrug Then
                                                Exit For
                                            End If
                                        Next J

                                        If Ik = -1 Then
                                            K += 1
                                            Result(K) = NavaidList(I)
                                            Result(K).IntersectionType = eIntersectionType.ByAngle
                                            'Result(K).Tag = 0
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next I
        Else
            '    fIADir = Modulus(ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, GuidNav.pPtPrj) + 180.0 * ComboBox102.ListIndex, 360.0)
            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                fConeAngle = VOR.ConeAngle
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                fConeAngle = NDB.ConeAngle
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                fConeAngle = 80.0
            End If

            fRConus = System.Math.Tan(DegToRad(fConeAngle)) * (hFix - GuidNav.pPtGeo.Z)
            pProxi = pGuidPoly

            pt1 = PointAlongPlane(PtSDF, fIADir - 180.0, 100000.0)
            pCutter.FromPoint = PointAlongPlane(pt1, fIADir - 90.0, 100000.0)
            pCutter.ToPoint = PointAlongPlane(pt1, fIADir + 90.0, 100000.0)
            '================================================
            For I = 0 To N
                If (NavaidList(I).TypeCode <> eNavaidType.LLZ) And (pProxi.ReturnDistance(NavaidList(I).pPtPrj) > fRConus) Then
                    If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                        ConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                        fOnNavRad = VOR.OnNAVRadius
                        fInterToler = VOR.IntersectingTolerance
                    ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                        ConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * (hFix - NavaidList(I).pPtPrj.Z)
                        fOnNavRad = NDB.OnNAVRadius
                        fInterToler = NDB.IntersectingTolerance
                    End If

                    fDist = ReturnDistanceInMeters(PtSDF, NavaidList(I).pPtPrj)
                    fInterRange = NavaidList(I).Range '/ Cos(DegToRad(fInterToler))

                    If fDist < fInterRange Then
                        If ((fDist > ConRad) Or (fDist < fOnNavRad)) Then
                            fDir = ReturnAngleInDegrees(PtSDF, NavaidList(I).pPtPrj)
                            pConstruct = pt1
                            pConstruct.ConstructAngleIntersection(StepDownFIXs(StepDownsNum - 1).pPtPrj, DegToRad(fIADir), NavaidList(I).pPtPrj, DegToRad(fDir + fInterToler))
                            If ReturnDistanceInMeters(pt1, PtSDF) < dPrec Then
                                pConstruct = pt1
                                pConstruct.ConstructAngleIntersection(StepDownFIXs(StepDownsNum - 1).pPtPrj, DegToRad(fIADir), NavaidList(I).pPtPrj, DegToRad(fDir - fInterToler))
                                If ReturnDistanceInMeters(pt1, PtSDF) < dPrec Then
                                    pFixPoly = CreateFixZone(PtSDF, GuidNav, NavaidList(I), False, fIADir, svGuyrug)
                                    '                            Set pFixProxi = pFixPoly
                                    '                            svGuyrug = 100000# - pFixProxi.ReturnDistance(pCutter)
                                    pRelation = pFixPoly

                                    Ik = -1
                                    M = UBound(IAFObstList4FIX.Parts)
                                    For J = 0 To M
                                        If pRelation.Contains(IAFObstList4FIX.Parts(J).pPtPrj) Or (IAFObstList4FIX.Parts(J).Dist < fIF_SDFDist) Then
                                            If IAFObstList4FIX.Parts(J).hPenet > 0.0 Then
                                                fTmp = fIF_SDFDist + svGuyrug - IAFObstList4FIX.Parts(J).Dist
                                                If fTmp > arFIX15PlaneRang.Value Then
                                                    Ik = J
                                                    Exit For
                                                End If

                                                fTmp = (PtSDF.Z - IAFObstList4FIX.Parts(J).ReqH) / fTmp
                                                If fTmp < arFixMaxIgnorGrd.Value Then
                                                    Ik = J
                                                    Exit For
                                                End If
                                            End If
                                        End If
                                        If IAFObstList4FIX.Parts(J).Dist > fIF_SDFDist + svGuyrug Then
                                            Exit For ' exit next for
                                        End If
                                    Next J

                                    If Ik = -1 Then
                                        K = K + 1
                                        Result(K) = NavaidList(I)
                                        Result(K).IntersectionType = eIntersectionType.ByAngle
                                        'Result(K).Tag = 0
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next I

            M = UBound(DMEList)
            For I = 0 To M
                pBaseLine.FromPoint = DMEList(I).pPtPrj
                pBaseLine.ToPoint = PtSDF
                fDir = RadToDeg(pBaseLine.Angle)
                fTmp = SubtractAngles(fIADir, fDir)
                fDist = pBaseLine.Length

                If (fDist > DME.MinimalError) And ((fTmp <= DME.TP_div) Or (fTmp >= 180.0 - DME.TP_div)) Then
                    fTmp = hFix - DMEList(I).pPtPrj.Z
                    fAlpha = RadToDeg(Math.Atan2(fDist, fTmp))

                    If (fAlpha < 90.0) And (fAlpha > DME.SlantAngle) And (fDist < DMEList(I).Range) Then
                        Side = SideDef(PtSDF, fIADir + 90.0, DMEList(I).pPtPrj)

                        fTmp = hFix - DMEList(I).pPtPrj.Z
                        fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)
                        fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
                        fRadius = fDist - Side * fTmp

                        CircleVectorIntersect(DMEList(I).pPtPrj, fRadius, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 90.0 * (1 + Side), pt1)
                        If (Not pt1.IsEmpty) And (ReturnDistanceInMeters(pt1, PtSDF) < dPrec) Then
                            fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
                            fRadius = fDist + Side * fTmp

                            CircleVectorIntersect(DMEList(I).pPtPrj, fRadius, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 90.0 * (1 + Side), pt1)
                            If (Not pt1.IsEmpty) And (ReturnDistanceInMeters(pt1, PtSDF) < dPrec) Then
                                pFixPoly = CreateFixZone(PtSDF, GuidNav, DMEList(I), False, fIADir, svGuyrug)

                                pRelation = pFixPoly 'svGuyrug = 100000# - pFixProxi.ReturnDistance(pCutter)

                                Ik = -1
                                M = UBound(IAFObstList4FIX.Parts)
                                For J = 0 To M
                                    If pRelation.Contains(IAFObstList4FIX.Parts(J).pPtPrj) Or (IAFObstList4FIX.Parts(J).Dist < fIF_SDFDist) Then
                                        'If pRelation.Contains(IAFObstList4FIX(J).pPtPrj) Then
                                        If IAFObstList4FIX.Parts(J).hPenet > 0.0 Then
                                            fTmp = fIF_SDFDist + svGuyrug - IAFObstList4FIX.Parts(J).Dist
                                            If fTmp > arFIX15PlaneRang.Value Then
                                                Ik = J
                                                Exit For
                                            End If

                                            fTmp = (PtSDF.Z - IAFObstList4FIX.Parts(J).ReqH) / fTmp
                                            If fTmp < arFixMaxIgnorGrd.Value Then
                                                Ik = J
                                                Exit For
                                            End If
                                        End If
                                    End If
                                    If IAFObstList4FIX.Parts(J).Dist > fIF_SDFDist + svGuyrug Then
                                        Exit For 'exit next for	
                                    End If
                                Next J

                                If Ik = -1 Then
                                    K = K + 1
                                    Result(K) = DMEList(I)
                                    Result(K).IntersectionType = eIntersectionType.ByDistance
                                End If
                            End If
                        End If
                    End If
                End If
            Next I
        End If

        For I = 0 To N
            If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                fOnNavRad = VOR.OnNAVRadius
            ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                fOnNavRad = NDB.OnNAVRadius
            Else
                fOnNavRad = -10000.0
            End If

            fDist = Point2LineDistancePrj(GuidNav.pPtPrj, NavaidList(I).pPtPrj, fIADir)
            If (fDist <= fOnNavRad) And (GuidNav.TypeCode = NavaidList(I).TypeCode) And SideDef(StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 90.0, NavaidList(I).pPtPrj) < 0 Then
                pFixPoly = CreateFixZone(NavaidList(I).pPtPrj, GuidNav, NavaidList(I), True, fIADir, svGuyrug)
                pRelation = pFixPoly

                Ik = -1
                M = UBound(IAFObstList4FIX.Parts)
                For J = 0 To M
                    If pRelation.Contains(IAFObstList4FIX.Parts(J).pPtPrj) Or (IAFObstList4FIX.Parts(J).Dist < fIF_SDFDist) Then
                        If IAFObstList4FIX.Parts(J).hPenet > 0.0 Then
                            fTmp = fIF_SDFDist + svGuyrug - IAFObstList4FIX.Parts(J).Dist
                            If fTmp > arFIX15PlaneRang.Value Then
                                Ik = J
                                Exit For
                            End If

                            fTmp = (PtSDF.Z - IAFObstList4FIX.Parts(J).ReqH) / fTmp
                            If fTmp < arFixMaxIgnorGrd.Value Then
                                Ik = J
                                Exit For
                            End If
                        End If
                    End If
                    If IAFObstList4FIX.Parts(J).Dist > fIF_SDFDist + svGuyrug Then
                        Exit For 'exit next for
                    End If
                Next J

                If Ik = -1 Then
                    K = K + 1
                    Result(K) = NavaidList(I)
                    Result(K).IntersectionType = eIntersectionType.OnNavaid
                    'Result(K).Tag = -1
                End If
            End If
        Next I

        If K < 0 Then
            ReDim Result(-1)
        Else
            ReDim Preserve Result(K)
        End If

        Return Result
    End Function

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
                            If System.Math.Abs(L0 - L) < 100.0 Then L = L0 - 100.0
                            Alpha1 = AlphaR(L, hFix, Hi, RDME, PDG, C155)
                            L0 = L
                        Else
                            If System.Math.Abs(Reffectiv0 - Reffectiv) < 100.0 Then Reffectiv = Reffectiv0 - 100.0
                            Alpha1 = AlphaL(Betta, RDME, rDMEObs, Reffectiv)
                            Reffectiv0 = Reffectiv
                        End If
                        J = -CShort(System.Math.Abs(Alpha1 - Alpha0) < degEps)
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
                X9300 = IAFObstList.Parts(I).Dist - System.Math.Sqrt(arIFHalfWidth.Value * arIFHalfWidth.Value - IAFObstList.Parts(I).CLDist * IAFObstList.Parts(I).CLDist)

                FixRange(I, 1).Tag = -1

                B = (a1 * b1 + IAFObstList.Parts(I).Dist)
                C = b1 * b1 - IAFObstList.Parts(I).Dist * IAFObstList.Parts(I).Dist - IAFObstList.Parts(I).CLDist * IAFObstList.Parts(I).CLDist
                D = B * B - A * C
                If D >= 0.0 Then
                    D = System.Math.Sqrt(D)
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

    Private Sub CreateIAF_AreaPoly(ByRef GuidNav As NavaidData, ByRef ForFIX As StepDownFIX, ByRef pIAF_IAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByRef pIAF_IIAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByRef pNomPoly As ESRI.ArcGIS.Geometry.IPolyline)
        Dim bNavCode As Boolean
        Dim TurnDir As Integer
        Dim NavSide As Integer

        Dim L As Double
        Dim fIADir As Double
        Dim fNavDist As Double
        Dim fNextDir As Double

        Dim fDist As Double
        Dim fOnNavRad As Double
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtTo As ESRI.ArcGIS.Geometry.IPoint

        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pCirc As ESRI.ArcGIS.Geometry.IPolygon
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        '====================================================================================
        L = ReturnDistanceInMeters(ForFIX.pPtPrj, GuidNav.pPtPrj)
        fNextDir = ReturnAngleInDegrees(ForFIX.pPtPrj, GuidNav.pPtPrj)

        If GuidNav.TypeCode = eNavaidType.DME Then
            '===============
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + arIFHalfWidth.Value)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - arIFHalfWidth.Value)
            pTopo = pCirc
            pTmpPoly = pTopo.Difference(pTmpPoly)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0, 2 * (L + arIFHalfWidth.Value))
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fNextDir, 2 * (L + arIFHalfWidth.Value))
            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If ForFIX.TurnDir = 0 Then
                If Modulus(fNextDir - ForFIX.OutDir, 360.0) > 180.0 Then
                    pTopo.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
                    pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir, L)
                    pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, 1)
                Else
                    pTopo.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
                    pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir, L)
                    pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, -1)
                End If
            ElseIf ForFIX.TurnDir > 0 Then
                pTopo.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
                pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir, L)
                pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, 1)
            Else
                pTopo.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
                pPtTo = PointAlongPlane(GuidNav.pPtPrj, fNextDir, L)
                pNomPoly = CreateArcPolylinePrj(GuidNav.pPtPrj, ForFIX.pPtStart, pPtTo, -1)
            End If

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arIFHalfWidth.Value)
            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)

            pTopo = pIAF_IIAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()
            pIAF_IIAreaPoly = pTopo.Union(pTmpPoly)
            '===============
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + 0.5 * arIFHalfWidth.Value)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - 0.5 * arIFHalfWidth.Value)

            pTopo = pCirc
            pTmpPoly = pTopo.Difference(pTmpPoly)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fNextDir + 180.0, 2 * (L + arIFHalfWidth.Value))
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fNextDir, 2 * (L + arIFHalfWidth.Value))

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If ForFIX.TurnDir = 0 Then
                If Modulus(fNextDir - ForFIX.OutDir, 360.0) > 180.0 Then
                    pTopo.Cut(pCutter, pCirc, pIAF_IAreaPoly)
                Else
                    pTopo.Cut(pCutter, pIAF_IAreaPoly, pCirc)
                End If
            ElseIf ForFIX.TurnDir > 0 Then
                pTopo.Cut(pCutter, pCirc, pIAF_IAreaPoly)
            Else
                pTopo.Cut(pCutter, pIAF_IAreaPoly, pCirc)
            End If

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopo = pIAF_IAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()
            pIAF_IAreaPoly = pTopo.Union(pTmpPoly)
            '===============
        Else 'VOR NDB
            '====================================================================================
            fOnNavRad = -10000.0

            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                fNavDist = arIFHalfWidth.Value / System.Math.Tan(DegToRad(VOR.SplayAngle))
                fOnNavRad = VOR.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                fNavDist = arIFHalfWidth.Value / System.Math.Tan(DegToRad(NDB.SplayAngle))
                fOnNavRad = NDB.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                fNavDist = LLZ.Range
            End If

            NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)
            TurnDir = ForFIX.TurnDir '2 * ComboBox102.ListIndex - 1

            If ForFIX.TurnDir = 0 Then
                '        fIADir = fNextDir
                '==========================================================================================================
                bNavCode = StepDownsNum >= 1
                If bNavCode Then bNavCode = ForFIX.GuidanceNav.TypeCode = eNavaidType.DME

                If Not bNavCode Then
                    fIADir = ForFIX.InDir
                Else
                    If SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, GuidNav.pPtPrj) > 0 Then
                        If TurnDir = NavSide Then
                            fIADir = fNextDir
                        Else
                            fIADir = fNextDir + 180.0
                        End If
                    Else
                        If TurnDir = NavSide Then
                            fIADir = fNextDir
                        Else
                            fIADir = fNextDir + 180.0
                        End If
                    End If
                End If
                '==========================================================================================================
                If ForFIX.PDG < PDGEps Then
                    L = 100000.0
                Else
                    L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                End If
            Else
                '        fMaxTurnAngle = Abs(CDbl(Right(ComboBox103.Text, 3)))

                '        If SubtractAngles(fNextDir, ForFIX.OutDir) > fMaxTurnAngle Then
                '            fIADir = fNextDir + 180.0
                '            L = Max(L - fNavDist, (curradhp.MinTMA - ForFIX.pPtPrj.Z) / ForFIX.PDG)
                '        Else
                '            fIADir = fNextDir
                '            L = Max(L + fNavDist, (curradhp.MinTMA - ForFIX.pPtPrj.Z) / ForFIX.PDG)
                '        End If
                '        TurnDir = ForFIX.TurnDir
                '        NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)
                fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ForFIX.pPtPrj)

                If fDist < fOnNavRad Then
                    pClone = StepDownFIXs(StepDownsNum).pPtPrj
                    pPtTmp = pClone.Clone
                    pPtTmp.SpatialReference = pSpRefPrj
                    pPtTmp.Project(pSpRefGeo)
                    fIADir = Azt2Dir(pPtTmp, CDbl(TextBox108.Text))

                    L = fNavDist
                Else
                    'Dim bDrawFlg As Boolean
                    'If bDrawFlg Then
                    '    DrawPointWithText ForFIX.pPtPrj, "For FIX"
                    '    DrawPointWithText GuidNav.pPtPrj, "GuidNav"
                    'End If

                    If SideDef(ForFIX.pPtPrj, fNextDir + 90.0, GuidNav.pPtPrj) > 0 Then
                        If TurnDir = NavSide Then
                            fIADir = fNextDir + 180.0
                            If ForFIX.PDG > PDGEps Then L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                        Else
                            fIADir = fNextDir
                            If ForFIX.PDG > PDGEps Then L = Max(L - fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                        End If
                    Else
                        If TurnDir = NavSide Then
                            fIADir = fNextDir + 180.0
                            If ForFIX.PDG > PDGEps Then L = Max(L - fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                        Else
                            fIADir = fNextDir
                            If ForFIX.PDG > PDGEps Then L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                        End If
                    End If
                    If ForFIX.PDG < PDGEps Then L = 100000.0
                    If L < 10000.0 Then L = 10000.0
                End If
            End If

            If L < IntDist Then L = IntDist ' ;(

            pNomPoly = New ESRI.ArcGIS.Geometry.Polyline
            pNomPoly.FromPoint = ForFIX.pPtStart
            pNomPoly.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fIADir + 180.0, L)

            pIAF_IIAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0, arIFHalfWidth.Value))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0, arIFHalfWidth.Value))

            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(1), fIADir + 180.0, L))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(0), fIADir + 180.0, L))

            pTopo = pIAF_IIAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopo = pIAF_IIAreaPoly
            pIAF_IIAreaPoly = pTopo.Union(pTmpPoly)

            pIAF_IAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0, 0.5 * arIFHalfWidth.Value))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0, 0.5 * arIFHalfWidth.Value))

            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(1), fIADir + 180.0, L))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(0), fIADir + 180.0, L))

            pTopo = pIAF_IAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pCutter = New ESRI.ArcGIS.Geometry.Polyline
            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, ForFIX.OutDir - 90.0, arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopo = pIAF_IAreaPoly
            pIAF_IAreaPoly = pTopo.Union(pTmpPoly)
        End If
    End Sub

    Private Function EstimateIAF_AreaObstacles() As Integer
        Dim TurnFlg As Integer
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer

        Dim DistNavToObst As Double
        Dim DirNavToObst As Double
        Dim fDistFIX2Obs As Double
        Dim fNextSegPDG As Double
        Dim DistToObst As Double
        Dim CurrDirect As Double
        Dim fTurnAngle As Double
        Dim DirToObst As Double
        Dim fTurnDist As Double
        Dim AlphaL As Double
        Dim AlphaR As Double
        Dim dMax15 As Double
        Dim dMin15 As Double
        Dim fIADir As Double
        Dim fAlpha As Double
        Dim fRObs As Double
        Dim fTmp1 As Double
        Dim dPrec As Double
        Dim fDist As Double
        Dim Side As Double
        Dim fTmp As Double
        Dim Phi As Double
        Dim dD As Double
        Dim fL As Double
        Dim X As Double

        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim ptMinTurn As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim LastFIX As StepDownFIX
        Dim GuidNav As NavaidData

        ShowPandaBox(Me.Handle.ToInt32)

        'While True
        '	Application.DoEvents()
        'End While

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
        fIADir = LastFIX.InDir

        fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastFIX.pPtPrj)
        fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastFIX.pPtPrj)

        pGuidPoly = CreateGuidPoly(GuidNav, LastFIX.pPtPrj)

        '====================================================================================
        'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, 0
        'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIAreaPoly, 255
        N = CalcIFProhibitions(LastFIX, fRefHeight, StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, StepDownFIXs(StepDownsNum - 1).pIAreaPoly, m_IAFObstList, IAFProhibSectors)
        EstimateIAF_AreaObstacles = N
        fNextSegPDG = IIf(IAF_PDG <= 0, arIADescent_Max.Value, IAF_PDG)
        '   fNextSegPDG = IIf(IAF_PDG <= 0, arIADescent_Nom.Value, IAF_PDG)

        If N >= 0 Then
            If N > 0 Then Sort(m_IAFObstList, 0)
            M = UBound(m_IAFObstList.Obstacles)
            ReDim IAFObstList4FIX.Parts(N)
            ReDim IAFObstList4FIX.Obstacles(M)

            Array.Copy(m_IAFObstList.Parts, IAFObstList4FIX.Parts, N + 1)
            Array.Copy(m_IAFObstList.Obstacles, IAFObstList4FIX.Obstacles, M + 1)

            If GuidNav.TypeCode = eNavaidType.DME Then
                fTmp = ReturnAngleInDegrees(LastFIX.pPtPrj, GuidNav.pPtPrj)
                fTmp = Modulus(LastFIX.InDir - fTmp + 90, 360.0)
                If fTmp > 180.0 Then fTmp = 360.0 - fTmp
                TurnFlg = -CShort(fTmp < 90.0)
            End If

            pRelation = pGuidPoly

            IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsAreaElement = DrawPolygon(IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsArea, RGB(254, 0, 254))
            IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).ObsAreaElement.Locked = True
            '======================================15%====================================================
            IAFObstList4FIX.Parts(0).Rmin = IAFObstList4FIX.Parts(0).hPenet / fNextSegPDG 'arIADescent_Max.Value
            'IAFObstList4FIX(0).
            dMax15 = IAFObstList4FIX.Parts(0).Dist - IAFObstList4FIX.Parts(0).Rmin
            'IAFObstList4FIX(0).
            dMin15 = (arFIX15PlaneRang.Value * (fNextSegPDG - arFixMaxIgnorGrd.Value) - IAFObstList4FIX.Parts(0).hPenet + fNextSegPDG * IAFObstList4FIX.Parts(0).Dist) / fNextSegPDG

            'dD = Max(Min(IAFObstList4FIX(0).dMax15, dPrec), IAFObstList4FIX(0).dMin15)
            dD = Max(Min(dMax15, dPrec), dMin15)

            IAFObstList4FIX.Parts(0).Rmax = (IAFObstList4FIX.Parts(0).hPenet + arFixMaxIgnorGrd.Value * (dD - IAFObstList4FIX.Parts(0).Dist)) / (fNextSegPDG - arFixMaxIgnorGrd.Value)
            IAFObstList4FIX.Parts(0).dMin15 = dD

            IAFObstList4FIX.Parts(0).Flags = IAFObstList4FIX.Parts(0).Flags And Not 2
            If Not pRelation.Contains(IAFObstList4FIX.Parts(0).pPtPrj) Then
                If IAFObstList4FIX.Parts(0).Rmax < IAFObstList4FIX.Parts(0).Dist - 5.0 Then
                    IAFObstList4FIX.Parts(0).Rmax = IAFObstList4FIX.Parts(0).Dist - 5.0
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
                        IAFObstList4FIX.Parts(0).DistStar = 0.0
                    End If
                End If
            Else
                If IAFObstList4FIX.Parts(0).Rmax < dPrec Then
                    IAFObstList4FIX.Parts(0).DistStar = IAFObstList4FIX.Parts(0).hPenet
                Else
                    IAFObstList4FIX.Parts(0).DistStar = 0.0
                End If
            End If

            If GuidNav.TypeCode <> eNavaidType.DME Then
                If CircleVectorIntersect(IAFObstList4FIX.Parts(0).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).rObs, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir, ptTmp) > 0.0 Then
                    IAFObstList4FIX.Parts(0).TurnDistL = Point2LineDistancePrj(ptTmp, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 90.0) * SideDef(ptTmp, fIADir + 90.0, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                    IAFObstList4FIX.Parts(0).TurnDistR = IAFObstList4FIX.Parts(0).TurnDistL

                    IAFObstList4FIX.Parts(0).TurnAngleL = SubtractAngles(ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(0).pPtPrj) + 90.0, fIADir + 180.0)
                    IAFObstList4FIX.Parts(0).TurnAngleR = 180.0 - IAFObstList4FIX.Parts(0).TurnAngleL
                Else
                    IAFObstList4FIX.Parts(0).TurnAngleL = 500.0
                    IAFObstList4FIX.Parts(0).TurnAngleR = 500.0
                    IAFObstList4FIX.Parts(0).TurnDistL = -1.0
                    IAFObstList4FIX.Parts(0).TurnDistR = -1.0
                End If
            Else
                If CircleCircleIntersect(GuidNav.pPtPrj, fL, IAFObstList4FIX.Parts(0).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).rObs, 1 - 2 * TurnFlg, ptTmp) > -1.0 Then
                    IAFObstList4FIX.Parts(0).TurnDistL = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ptTmp))) * fL
                    IAFObstList4FIX.Parts(0).TurnDistR = IAFObstList4FIX.Parts(0).TurnDistL

                    fTmp = ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(0).pPtPrj)
                    fTmp1 = ReturnAngleInDegrees(ptTmp, GuidNav.pPtPrj)

                    IAFObstList4FIX.Parts(0).TurnAngleL = SubtractAngles(fTmp + 90.0, fTmp1 + 90.0 * (2 * TurnFlg - 1))
                    IAFObstList4FIX.Parts(0).TurnAngleR = 180.0 - IAFObstList4FIX.Parts(0).TurnAngleL
                Else
                    IAFObstList4FIX.Parts(0).TurnAngleL = 500.0
                    IAFObstList4FIX.Parts(0).TurnAngleR = 500.0
                    IAFObstList4FIX.Parts(0).TurnDistL = -1.0
                    IAFObstList4FIX.Parts(0).TurnDistR = -1.0
                End If
            End If
            '==========================================================================================
            I = 0

            If IAFObstList4FIX.Parts(0).fTmp <= fNextSegPDG Then
                For J = 1 To N
                    'If IAFObstList4FIX(J).Height + IAFObstList4FIX(J).MOC > IAFObstList4FIX(i).ReqH Then
                    If IAFObstList4FIX.Parts(J).ReqH > IAFObstList4FIX.Parts(I).ReqH Then
                        I = I + 1
                        IAFObstList4FIX.MovePart(I, J)

                        IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsAreaElement = DrawPolygon(IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsArea, RGB(254, 0, 254))
                        IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).ObsAreaElement.Locked = True

                        'Application.DoEvents()
                        '============================================15%==============================================
                        IAFObstList4FIX.Parts(I).Rmin = IAFObstList4FIX.Parts(I).hPenet / fNextSegPDG 'arIADescent_Max.Value
                        'IAFObstList4FIX(I).
                        dMax15 = IAFObstList4FIX.Parts(I).Dist - IAFObstList4FIX.Parts(I).Rmin
                        'IAFObstList4FIX(I).
                        dMin15 = (arFIX15PlaneRang.Value * (fNextSegPDG - arFixMaxIgnorGrd.Value) - IAFObstList4FIX.Parts(I).hPenet + fNextSegPDG * IAFObstList4FIX.Parts(I).Dist) / fNextSegPDG

                        'dD = Max(Min(IAFObstList4FIX(I).dMax15, dPrec), IAFObstList4FIX(I).dMin15)
                        dD = Max(Min(dMax15, dPrec), dMin15)
                        IAFObstList4FIX.Parts(I).Rmax = (IAFObstList4FIX.Parts(I).hPenet + arFixMaxIgnorGrd.Value * (dD - IAFObstList4FIX.Parts(I).Dist)) / (fNextSegPDG - arFixMaxIgnorGrd.Value)
                        IAFObstList4FIX.Parts(I).dMin15 = dD

                        IAFObstList4FIX.Parts(I).Flags = IAFObstList4FIX.Parts(I).Flags And Not 2
                        If Not pRelation.Contains(IAFObstList4FIX.Parts(I).pPtPrj) Then
                            If IAFObstList4FIX.Parts(I).Rmax < IAFObstList4FIX.Parts(I).Dist - 5.0 Then
                                IAFObstList4FIX.Parts(I).Rmax = IAFObstList4FIX.Parts(I).Dist - 5.0
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
                                '                        If IAFObstList4FIX(i).fTmp > arIADescent_Max.Value Then
                                If IAFObstList4FIX.Parts(I).fTmp > fNextSegPDG Then
                                    IAFObstList4FIX.Parts(I).DistStar = IAFObstList4FIX.Parts(I).hPenet - fNextSegPDG * IAFObstList4FIX.Parts(I).Rmax
                                Else
                                    IAFObstList4FIX.Parts(I).DistStar = 0.0
                                End If
                            End If
                        Else
                            If IAFObstList4FIX.Parts(I).Rmax < dPrec Then
                                IAFObstList4FIX.Parts(I).DistStar = IAFObstList4FIX.Parts(I).hPenet
                            Else
                                IAFObstList4FIX.Parts(I).DistStar = 0.0
                            End If
                        End If

                        If GuidNav.TypeCode <> eNavaidType.DME Then
                            If CircleVectorIntersect(IAFObstList4FIX.Parts(I).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(I).SectorIndex).rObs, StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir, ptTmp) > 0.0 Then
                                IAFObstList4FIX.Parts(I).TurnDistL = ReturnDistanceInMeters(ptTmp, StepDownFIXs(StepDownsNum - 1).pPtPrj) * SideDef(ptTmp, fIADir + 90.0, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                                IAFObstList4FIX.Parts(I).TurnDistR = IAFObstList4FIX.Parts(I).TurnDistL
                                IAFObstList4FIX.Parts(I).TurnAngleL = SubtractAngles(ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(I).pPtPrj) + 90.0, fIADir + 180.0)
                                IAFObstList4FIX.Parts(I).TurnAngleR = 180.0 - IAFObstList4FIX.Parts(I).TurnAngleL
                            Else
                                IAFObstList4FIX.Parts(I).TurnAngleL = 500.0
                                IAFObstList4FIX.Parts(I).TurnAngleR = 500.0
                                IAFObstList4FIX.Parts(I).TurnDistL = -1.0
                                IAFObstList4FIX.Parts(I).TurnDistR = -1.0
                            End If
                        Else
                            If CircleCircleIntersect(GuidNav.pPtPrj, fL, IAFObstList4FIX.Parts(I).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(I).Index).rObs, 1 - 2 * TurnFlg, ptTmp) > -1.0 Then
                                IAFObstList4FIX.Parts(I).TurnDistL = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ptTmp))) * fL
                                IAFObstList4FIX.Parts(I).TurnDistR = IAFObstList4FIX.Parts(I).TurnDistL

                                fTmp = ReturnAngleInDegrees(ptTmp, IAFObstList4FIX.Parts(I).pPtPrj)
                                fTmp1 = ReturnAngleInDegrees(ptTmp, GuidNav.pPtPrj)

                                IAFObstList4FIX.Parts(I).TurnAngleL = SubtractAngles(fTmp + 90.0, fTmp1 + 90.0 * (2 * TurnFlg - 1))
                                IAFObstList4FIX.Parts(I).TurnAngleR = 180.0 - IAFObstList4FIX.Parts(I).TurnAngleL
                            Else
                                IAFObstList4FIX.Parts(I).TurnAngleL = 500.0
                                IAFObstList4FIX.Parts(I).TurnAngleR = 500.0
                                IAFObstList4FIX.Parts(I).TurnDistL = -1.0
                                IAFObstList4FIX.Parts(I).TurnDistR = -1.0
                            End If
                        End If
                        'If IAFObstList4FIX(I).fTmp > arIADescent_Max.Value Then Exit For
                        'If IAFObstList4FIX(I).fTmp > fNextSegPDG Then Exit For
                    End If
                Next J
            End If

            If N > I Then
                N = I
                ReDim Preserve IAFObstList4FIX.Parts(I)
            End If

            'InitialReportsFrm.FillPage1 IAFObstList4FIX
            '================================================================================
            M = UBound(IAFObstList4FIX.Obstacles)

            ReDim IAFObstList4Turn.Parts(N)
            ReDim IAFObstList4Turn.Obstacles(M)
            Array.Copy(IAFObstList4FIX.Parts, IAFObstList4Turn.Parts, N + 1)
            Array.Copy(IAFObstList4FIX.Obstacles, IAFObstList4Turn.Obstacles, M + 1)

            Sort(IAFObstList4Turn, 1)

            If SubtractAngles(StepDownFIXs(StepDownsNum - 1).InDir, StepDownFIXs(StepDownsNum - 1).OutDir) < 5.0 Then
                If StepDownFIXs(StepDownsNum - 2).GuidanceNav.TypeCode <> eNavaidType.DME Then
                    fDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 2).pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                Else
                    X = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 2).pPtPrj)
                    fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                    X = DegToRad(SubtractAngles(fTmp, X))
                    fTmp = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 2).pPtPrj, StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj)
                    fDist = fTmp * X
                End If
            Else
                fDist = 0.0
            End If
            'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, 0
            'DrawPolygon StepDownFIXs(StepDownsNum - 1).pIAreaPoly, 255

            If GuidNav.TypeCode <> eNavaidType.DME Then
                Side = SideDef(StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 180.0, IAFObstList4Turn.Parts(0).pPtPrj)
                fDistFIX2Obs = Point2LineDistancePrj(StepDownFIXs(StepDownsNum - 1).pPtPrj, IAFObstList4Turn.Parts(0).pPtPrj, fIADir + 90.0)
            Else
                DirNavToObst = ReturnAngleInDegrees(GuidNav.pPtPrj, IAFObstList4Turn.Parts(0).pPtPrj)

                DistNavToObst = ReturnDistanceInMeters(GuidNav.pPtPrj, IAFObstList4Turn.Parts(0).pPtPrj)
                CircleCircleIntersect(GuidNav.pPtPrj, fL, IAFObstList4FIX.Parts(0).pPtPrj, IAFProhibSectors(IAFObstList4FIX.Parts(0).SectorIndex).rObs, 1 - 2 * TurnFlg, ptTmp)
                fTmp = ReturnAngleInDegrees(GuidNav.pPtPrj, ptTmp) + 90.0 * (2 * TurnFlg - 1)
                Side = SideDef(ptTmp, fTmp, IAFObstList4Turn.Parts(0).pPtPrj)
                ''          Side = 1 + 2 * CInt(fL < DistNavToObst)
                ''          fTmp = ReturnAngleInDegrees(GuidNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)=fAlpha
                X = DegToRad(SubtractAngles(fAlpha, DirNavToObst))
                fDistFIX2Obs = fL * X
            End If

            fDistFIX2Obs = fDistFIX2Obs + fDist
            fRObs = IAFProhibSectors(IAFObstList4Turn.Parts(0).SectorIndex).rObs
            ComboBox103.Items.Clear()

            For I = 0 To 5
                fTmp = MinISlensArray(I)
                If fTmp < fDist Then fTmp = fDist

                'msgbox "IAFObstList4Turn(0).TurnDistL = " + CStr(IAFObstList4Turn(I).TurnDistL) + vbCrLf + _
                '"fTmp = " + CStr(fTmp)

                If IAFObstList4Turn.Parts(0).TurnDistL + fDist < fTmp Then
                    For J = I To 5
                        TurnIntervalsL(J).Tag = 0
                        TurnIntervalsR(J).Tag = 0
                    Next J
                    Exit For
                Else
                    ComboBox103.Items.Add(TurnComboList(I))
                    TurnIntervalsR(I).Tag = 1
                    TurnIntervalsL(I).Tag = 1
                    TurnIntervalsL(I).FromDist = fTmp
                    TurnIntervalsR(I).FromDist = fTmp
                    fTmp = fTmp - fDist
                    If GuidNav.TypeCode <> eNavaidType.DME Then
                        ptMinTurn = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).pPtPrj, fIADir + 180.0, fTmp)
                    Else
                        ptMinTurn = PointAlongPlane(GuidNav.pPtPrj, fAlpha + (1 - 2 * TurnFlg) * RadToDeg(fTmp / fL), fL)
                    End If
                    'DrawPoint ptMinTurn, 255
                    DistToObst = ReturnDistanceInMeters(ptMinTurn, IAFObstList4Turn.Parts(0).pPtPrj)
                    DirToObst = ReturnAngleInDegrees(ptMinTurn, IAFObstList4Turn.Parts(0).pPtPrj)
                    fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, ptMinTurn)

                    fTmp = RadToDeg(ArcSin(fRObs / DistToObst))

                    If GuidNav.TypeCode <> eNavaidType.DME Then
                        AlphaL = SubtractAngles(fIADir + 180.0, DirToObst + fTmp)
                        AlphaR = SubtractAngles(fIADir + 180.0, DirToObst - fTmp)
                    Else
                        CurrDirect = ReturnAngleInDegrees(ptMinTurn, GuidNav.pPtPrj) - (1 - 2 * TurnFlg) * 90.0
                        AlphaL = SubtractAngles(CurrDirect, DirToObst + fTmp)
                        AlphaR = SubtractAngles(CurrDirect, DirToObst - fTmp)
                    End If

                    TurnIntervalsL(I).FromAngle = AlphaL
                    TurnIntervalsR(I).FromAngle = AlphaR
                    '================================================================================
                    fTurnAngle = I * 6.0 + 90.0

                    If (IAFObstList4Turn.Parts(0).TurnAngleL < fTurnAngle) And (IAFObstList4Turn.Parts(0).TurnAngleR < fTurnAngle) Then
                        fTurnDist = IAFObstList4Turn.Parts(0).TurnDistL + fDist
                        If IAFObstList4Turn.Parts(0).TurnAngleL > IAFObstList4Turn.Parts(0).TurnAngleR Then
                            fTurnAngle = IAFObstList4Turn.Parts(0).TurnAngleL
                        Else
                            fTurnAngle = IAFObstList4Turn.Parts(0).TurnAngleR
                        End If
                    Else
                        If GuidNav.TypeCode <> eNavaidType.DME Then
                            fTmp = DegToRad(fTurnAngle)
                            X = (fRObs + Side * IAFObstList4Turn.Parts(0).CLDist * System.Math.Cos(fTmp)) / System.Math.Sin(fTmp)   'Provereno
                            fTurnDist = fDistFIX2Obs - X
                        Else
                            Phi = Side * DegToRad(I * 6.0) + ArcSin((fRObs - Side * fL * System.Math.Sin(DegToRad(I * 6.0))) / DistNavToObst)
                            '	Phi = DegToRad(-Side * I * 6.0) + ArcSin((fL * Sin(DegToRad(I * 6)) + Side * fRObs) / DistNavToObst)
                            ptTmp = PointAlongPlane(GuidNav.pPtPrj, DirNavToObst + (2 * TurnFlg - 1) * RadToDeg(Phi), fL)
                            'DrawPoint ptTmp, 255
                            fTmp = ReturnAngleInDegrees(ptTmp, IAFObstList4Turn.Parts(0).pPtPrj)
                            X = ArcSin(fRObs / ReturnDistanceInMeters(ptTmp, IAFObstList4Turn.Parts(0).pPtPrj))
                            fTmp1 = fTmp - RadToDeg(X)
                            fTurnDist = fDistFIX2Obs - fL * Phi
                        End If
                    End If

                    If Side > 0 Then
                        TurnIntervalsL(I).ToAngle = IAFObstList4Turn.Parts(0).TurnAngleL
                        TurnIntervalsL(I).ToDist = IAFObstList4Turn.Parts(0).TurnDistL + fDist
                        If fTurnAngle > IAFObstList4Turn.Parts(0).TurnAngleR Then
                            TurnIntervalsR(I).ToAngle = IAFObstList4Turn.Parts(0).TurnAngleR
                            TurnIntervalsR(I).ToDist = IAFObstList4Turn.Parts(0).TurnDistR + fDist
                        Else
                            TurnIntervalsR(I).ToDist = fTurnDist
                            TurnIntervalsR(I).ToAngle = fTurnAngle
                        End If
                    Else
                        TurnIntervalsR(I).ToAngle = IAFObstList4Turn.Parts(0).TurnAngleR
                        TurnIntervalsR(I).ToDist = IAFObstList4Turn.Parts(0).TurnDistR + fDist

                        If fTurnAngle > IAFObstList4Turn.Parts(0).TurnAngleL Then
                            TurnIntervalsL(I).ToAngle = IAFObstList4Turn.Parts(0).TurnAngleL
                            TurnIntervalsL(I).ToDist = IAFObstList4Turn.Parts(0).TurnDistL + fDist
                        Else
                            TurnIntervalsL(I).ToDist = fTurnDist
                            TurnIntervalsL(I).ToAngle = fTurnAngle
                        End If
                    End If
                    If TurnIntervalsL(I).ToAngle - 90.0 - 6.0 * I > degEps Then TurnIntervalsL(I).Tag = 0
                    If TurnIntervalsR(I).ToAngle - 90.0 - 6.0 * I > degEps Then TurnIntervalsR(I).Tag = 0
                End If
            Next I
        Else
            ReDim IAFObstList4FIX.Obstacles(-1)
            ReDim IAFObstList4FIX.Parts(-1)
            ReDim IAFObstList4Turn.Obstacles(-1)
            ReDim IAFObstList4Turn.Parts(-1)

            ComboBox103.Items.Clear()
            For J = 0 To 5
                ComboBox103.Items.Add(TurnComboList(J))
                fTurnAngle = I * 6.0 + 90.0
                TurnIntervalsL(J).Tag = 1
                TurnIntervalsL(J).FromDist = 0.0
                TurnIntervalsL(J).ToDist = BigDist
                TurnIntervalsL(J).FromAngle = 0.0
                TurnIntervalsL(J).ToAngle = fTurnAngle

                TurnIntervalsR(J).Tag = 1
                TurnIntervalsR(J).FromDist = 0.0
                TurnIntervalsR(J).ToDist = BigDist
                TurnIntervalsR(J).FromAngle = 0.0
                TurnIntervalsR(J).ToAngle = fTurnAngle
            Next J
        End If

        '   If ComboBox103.ListCount > 0 Then ComboBox103.ListIndex = 0
        HidePandaBox()
        InitialReportsFrm.FillPage2(IAFObstList4FIX)
    End Function

    Private Sub ComboBox001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
        'If Not bFormInitialised Then Return
        'Dim I As Integer
        'Dim J As Integer
        'Dim K As Integer
        'Dim L As Integer
        'Dim M As Integer
        'Dim N As Integer
        'Dim NN As Integer
        'Dim IL As Integer
        'Dim bTransIsValid As Boolean

        'Dim pIAP As InstrumentApproachProcedure
        'Dim pIAPList As List(Of InstrumentApproachProcedure)
        'Dim pProcedureLeg As SegmentLeg
        'Dim pProcedureTransition As ProcedureTransition

        'ComboBox002.Items.Clear()

        'K = ComboBox001.SelectedIndex
        'If K < 0 Then Return

        'CurrADHP = LocalADHPList(K)

        'FillNavaidList(NavaidList, DMEList, CurrADHP, MaxNAVDist)
        'FillWPT_FIXList(WPTList, CurrADHP, MaxNAVDist)
        'pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)

        'N = pIAPList.Count
        'NN = N + N

        'ReDim IAPArray(NN)
        'K = -1

        'Dim lastMID As Guid
        'lastMID = New Guid()

        'For I = 0 To N - 1
        '	'==========================================================================================
        '	pIAP = pIAPList.Item(I)
        '	If (InStr(1, pIAP.Name, "RNAV", vbTextCompare) = 0) And (pIAP.Identifier <> lastMID) Then

        '		'Set pProcedureTransitionList = pObjectDir.GetProcedureTranstionList(pIAP.identifier)
        '		M = pIAP.FlightTransition.Count
        '		For J = 0 To M - 1
        '			pProcedureTransition = pIAP.FlightTransition.Item(J) 'pProcedureTransitionList.Item(J)
        '			'Set pProcedureLegList = pObjectDir.GetSegmentLegListByTransition(CStr(pProcedureTransition.ID))
        '			L = pProcedureTransition.TransitionLeg.Count 'pProcedureLegList.Count
        '			If L > 0 Then
        '				If pProcedureTransition.Type.Value = Aran.Aim.Enums.CodeProcedurePhase.APPROACH Then
        '					K = K + 1

        '					IAPArray(K) = pIAP
        '					ComboBox002.Items.Add(pIAP.Name)
        '					lastMID = pIAP.Identifier
        '					Exit For
        '				Else
        '					bTransIsValid = False
        '					For IL = 0 To L - 1
        '						pProcedureLeg = pProcedureTransition.TransitionLeg.Item(IL).TheSegmentLeg.GetFeature()

        '						If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
        '							K = K + 1
        '							IAPArray(K) = pIAP
        '							ComboBox002.Items.Add(pIAP.Name)
        '							lastMID = pIAP.Identifier
        '							bTransIsValid = True
        '							Exit For
        '						End If
        '					Next IL
        '					If bTransIsValid Then Exit For
        '				End If
        '			End If
        '		Next J
        '	End If
        '	'==========================================================================================
        '	'        Set pIAP = pIAPList.Item(I)
        '	'        Set pProcedureLegList = pObjectDir.GetProcedureLegListByProcedure(pIAP.Mid)
        '	'
        '	'        M = pProcedureLegList.Count
        '	'        If M > 0 Then
        '	'            Set pProcedureLeg = pProcedureLegList.Item(0)
        '	'            If (pProcedureLeg.CodeRoleFix = "FAF") Or (pProcedureLeg.CodeRoleFix = "FAP") Then  'Or (pProcedureLeg.CodeRoleFix = "IF")
        '	'                K = K + 1
        '	'                Set IAPArray(K) = pIAP
        '	'                ComboBox002.AddItem pIAP.Designator
        '	'            End If
        '	'        End If
        '	'==========================================================================================
        'Next I

        'If K >= 0 Then
        '	ReDim Preserve IAPArray(K)
        'End If

        'If ComboBox002.Items.Count > 0 Then
        '	ComboBox002.SelectedIndex = 0
        'End If
    End Sub

    Private Sub ComboBox002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox002.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim M As Integer
        Dim N As Integer
        Dim Count As Integer

        Dim pProcedureLeg As SegmentLeg

        Dim pIAP As InstrumentApproachProcedure
        Dim pProcedureTransition As ProcedureTransition

        ComboBox003.Items.Clear()

        K = ComboBox002.SelectedIndex
        If K < 0 Then Return

        ProcedureName = ComboBox002.Text
        pIAP = IAPArray(K)

        pLandingTakeoff = pIAP.Landing
        'pLandingTakeoff = pIAP.flightTransition(0).departureRunwayTransition
        'Set pAircraftCharacteristic = pIAP.AircraftCharacteristic

        If pIAP.AircraftCharacteristic.Count > 0 Then
            Category = pIAP.AircraftCharacteristic.Item(0).AircraftLandingCategory.Value
        End If
        '======================================================================================

        Count = 1000
        ReDim TransArray(Count)

        K = -1

        'Set pProcedureTransitionList = pObjectDir.GetProcedureTranstionList(pIAP.identifier)
        N = pIAP.FlightTransition.Count

        For I = 0 To N - 1
            pProcedureTransition = pIAP.FlightTransition.Item(I)
            'Set pProcedureLegList = pObjectDir.GetSegmentLegListByTransition(CStr(pProcedureTransition.ID))
            M = pProcedureTransition.TransitionLeg.Count
            If M > 0 Then

                If Not (pProcedureTransition.Type Is Nothing) And pProcedureTransition.Type.HasValue And pProcedureTransition.Type.Value = CodeProcedurePhase.APPROACH Then
                    K += 1
                    If K > Count Then
                        Count = Count + 1000
                        ReDim Preserve TransArray(Count)
                    End If

                    TransArray(K).LegIndex = -1
                    TransArray(K).pIAP = pIAP
                    TransArray(K).pProcedureTransition = pProcedureTransition
                    ComboBox003.Items.Add("Transition " + (I + 1).ToString())
                Else
                    For J = 0 To M - 1
                        pProcedureLeg = pProcedureTransition.TransitionLeg.Item(J).TheSegmentLeg.GetFeature2()

                        If pProcedureLeg Is Nothing Then
                            Continue For
                        End If

                        If pProcedureLeg.EndPoint Is Nothing Then
                            Continue For
                        End If

                        If pProcedureLeg.EndPoint.Role Is Nothing Then
                            Continue For
                        End If

                        If pProcedureLeg.StartPoint Is Nothing Then
                            Continue For
                        End If

                        'If (pProcedureLeg.CodeRoleFix = "FAF") Or (pProcedureLeg.CodeRoleFix = "IF") Then
                        If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
                            K = K + 1
                            If K > Count Then
                                Count = Count + 1000
                                ReDim Preserve TransArray(Count)
                            End If

                            TransArray(K).LegIndex = J
                            TransArray(K).pIAP = pIAP
                            TransArray(K).pProcedureTransition = pProcedureTransition
                            ComboBox003.Items.Add("Transition " + (I + 1).ToString())
                            Exit For
                        End If
                    Next J
                End If
            End If
        Next I

        If K >= 0 Then
            ReDim Preserve TransArray(K)
            ComboBox003.SelectedIndex = 0
        Else
            ReDim TransArray(-1)
        End If
    End Sub

    Private Sub ComboBox003_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox003.SelectedIndexChanged
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

        TextBox004.Text = ""
        TextBox005.Text = ""
        TextBox006.Text = ""
        Label005.Text = ""

        On Error Resume Next
        If Not pFAFptElement Is Nothing Then pGraphics.DeleteElement(pFAFptElement)
        If Not pFAFAreaElement Is Nothing Then pGraphics.DeleteElement(pFAFAreaElement)
        On Error GoTo 0

        K = ComboBox003.SelectedIndex
        If K < 0 Then
            GetActiveView().Refresh() ' .PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            Return
        End If

        SelectedTransition = TransArray(K)
        bFirstPointIsIF = True
        '======================================================================================
        If SelectedTransition.LegIndex <= 0 Then
            pProcedureLegRef = SelectedTransition.pProcedureTransition.TransitionLeg.Item(0).TheSegmentLeg
            If SelectedTransition.LegIndex < 0 Then
                TextBox005.Text = "IAF/TP"
                bFirstPointIsIF = False
            Else
                TextBox005.Text = "IF"
            End If
        Else
            pProcedureLegRef = SelectedTransition.pProcedureTransition.TransitionLeg.Item(SelectedTransition.LegIndex).TheSegmentLeg
            TextBox005.Text = "IF"
        End If

        Identifier = Guid.Empty
        pProcedureLeg = pProcedureLegRef.GetFeature2()
        pIFPoint = pProcedureLeg.StartPoint

        bFound = False
        bOnNav = False

        If Not pIFPoint Is Nothing Then
            If Not pIFPoint.PointChoice Is Nothing Then
                If pIFPoint.PointChoice.Choice = Aran.Aim.SignificantPointChoice.DesignatedPoint Then
                    Identifier = pIFPoint.PointChoice.FixDesignatedPoint.Identifier
                ElseIf pIFPoint.PointChoice.Choice = Aran.Aim.SignificantPointChoice.Navaid Then
                    Identifier = pIFPoint.PointChoice.NavaidSystem.Identifier
                End If

                N = UBound(WPTList)

                If Identifier <> Guid.Empty Then
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

                If pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD Then
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
                            pNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)
                            iFound = iFound Or 1
                        Else
                            pIntersectNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                            If pIntersectNavaid Is Nothing Then Exit For

                            iFound = iFound Or 2
                        End If
                    End If

                    If iFound = 3 Then Exit For
                Next

                If iFound = 3 Then Exit For

                For I = 0 To pPointReference.FacilityDistance.Count - 1
                    pDistanceIndication = pPointReference.FacilityDistance(I).Feature.GetFeature2(FeatureType.DistanceIndication)

                    'If pPointReference.FacilityDistance(I).AlongCourseGuidance Then
                    '	pNavaid = pDistanceIndication.PointChoice.GetFeatureRef().GetFeature()
                    '	GuidanceNavCode = eNavaidType.CodeDME
                    '	iFound = iFound Or 1
                    'Else
                    '	pIntersectNavaid = pDistanceIndication.PointChoice.GetFeatureRef().GetFeature()
                    '	IntersectNavCode = eNavaidType.CodeDME
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

                If (TypeOf pNavaidEquipment Is Aran.Aim.Features.VOR) Or (TypeOf pNavaidEquipment Is Aran.Aim.Features.NDB) Or (TypeOf pNavaidEquipment Is Localizer) Then
                    If (TypeOf pNavaidEquipment Is Localizer) And Not ILSAdded Then
                        If GetILSByNavEq(pNavaid, ILS) And 1 = 1 Then
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
                ElseIf TypeOf pNavaidEquipment Is Aran.Aim.Features.DME Then
                    If GuidanceNavCode = eNavaidType.DME Then   ' pProcedureLeg.LegTypeARINC = CodeSegmentPath.AF
                        For I = 0 To N
                            If DMEList(I).NAV_Ident = pNavaid.Identifier Then
                                StartPoint.GuidanceNav = DMEList(I)
                                bFound = True
                                Exit For
                            End If
                        Next I
                    End If
                ElseIf (TypeOf pNavaidEquipment Is Aran.Aim.Features.TACAN) Then
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
                        If (TypeOf pIntersectNavaidEquipment Is Aran.Aim.Features.VOR) Or (TypeOf pIntersectNavaidEquipment Is Aran.Aim.Features.NDB) Or (TypeOf pIntersectNavaidEquipment Is Localizer) Then
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
                        ElseIf (TypeOf pIntersectNavaidEquipment Is Aran.Aim.Features.TACAN) Then
                            ' ...
                        End If
                    End If

                    If bIntFound Then Exit For
                Next J
            End If
        ElseIf Not pProcedureLeg.Angle Is Nothing Then
            pAngleIndication = pProcedureLeg.Angle.GetFeature2(FeatureType.AngleIndication)

            '============================================================================================
            If pAngleIndication.PointChoice.Choice = Aran.Aim.SignificantPointChoice.Navaid Then
                pNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                For J = 0 To pNavaid.NavaidEquipment.Count - 1
                    pNavaidEquipment = pNavaid.NavaidEquipment(J).TheNavaidEquipment.GetFeature2()

                    If (TypeOf pNavaidEquipment Is Aran.Aim.Features.VOR) Or (TypeOf pNavaidEquipment Is Aran.Aim.Features.NDB) Or (TypeOf pNavaidEquipment Is Localizer) Then
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
                    ElseIf (TypeOf pNavaidEquipment Is Aran.Aim.Features.TACAN) Then
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

        'If StartPoint.GuidanceNav.TypeCode = eNavaidType.CodeDME Then
        '	StartPoint.TrackType = TrackTypeArc
        'Else
        '	StartPoint.TrackType = TrackTypeStraight
        'End If


        'bIsHolding = False
        'If Not pProcedureLeg.LegTypeARINC Is Nothing Then
        '	If (Not bFirstPointIsIF) And (pProcedureLeg.LegTypeARINC >= Aran.Aim.Enums.CodeSegmentPath.HF) And (pProcedureLeg.LegTypeARINC <= Aran.Aim.Enums.CodeSegmentPath.HM) Then
        '		bIsHolding = True
        '	End If
        'End If

        bIsHolding = bOnNav

        TextBox004.Text = StartPoint.GuidanceNav.CallSign
        Label005.Text = GetNavTypeName(StartPoint.GuidanceNav.TypeCode)

        TextBox006.Text = StartPoint.IntersectNav.CallSign
        Label013.Text = GetNavTypeName(StartPoint.IntersectNav.TypeCode)

        Altitude = ConverterToSI.Convert(pProcedureLeg.UpperLimitAltitude, 0.0)
        TextBox002.Text = CStr(ConvertHeight(Altitude, eRoundMode.NEAREST))

        TextBox001.ReadOnly = Not bIsHolding
        If bIsHolding Then
            TextBox001.BackColor = SystemColors.Window
        Else
            TextBox001.BackColor = SystemColors.Control
        End If

        Course = pProcedureLeg.Course.Value
        TextBox001.Text = CStr(System.Math.Round(Course, 4))

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
            pFAFAreaElement = DrawPolygon(pIFTolerArea, , 255)
            pFAFAreaElement.Locked = True
        End If
    End Sub

    Private Sub ComboBox004_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox004.SelectedIndexChanged
        If Not bFormInitialised Then Return
        arMinISlen = MinISlensArray(ComboBox004.SelectedIndex)
        TextBox003.Text = CStr(ConvertDistance(arMinISlen, eRoundMode.NEAREST))
    End Sub

    Private Sub ComboBox101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox101.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer
        Dim Side1 As Integer
        Dim Side2 As Integer
        Dim fDist As Double
        Dim fAzimuth As Double
        Dim fOnNavRad As Double
        Dim fMaxTurnAngle As Double

        Dim ptFIX As ESRI.ArcGIS.Geometry.IPoint
        Dim GuidNav As NavaidData

        K = ComboBox101.SelectedIndex
        If K < 0 Then Return

        GuidNav = CType(ComboBox101.SelectedItem, NavaidData) 'IAFNavDat(K)

        Label106.Text = GetNavTypeName(GuidNav.TypeCode)

        fMaxTurnAngle = System.Math.Abs(CDbl(Strings.Right(ComboBox103.Text, 3)))
        fOnNavRad = -10000.0
        If GuidNav.TypeCode <> eNavaidType.DME Then
            fMaxTurnAngle = 180.0 - fMaxTurnAngle
            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                fOnNavRad = VOR.OnNAVRadius
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                fOnNavRad = NDB.OnNAVRadius
            End If
        Else
            fMaxTurnAngle = fMaxTurnAngle - 90.0
        End If

        ptFIX = StepDownFIXs(StepDownsNum).pPtPrj
        fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ptFIX)

        ComboBox102.Enabled = True

        If (fDist < fOnNavRad) Then 'And (GuidNav.TypeCode = StepDownFIXs(StepDownsNum - 1).GuidanceNav.TypeCode) Then
            TextBox108.ReadOnly = False
            TextBox108.BackColor = System.Drawing.SystemColors.Window

            TextBox101.ReadOnly = True
            TextBox101.BackColor = System.Drawing.SystemColors.ButtonFace

            fAzimuth = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum).OutDir)
            TextBox108.Text = CStr(System.Math.Round(fAzimuth))
            ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
            Return
        End If

        TextBox108.ReadOnly = True
        TextBox108.BackColor = System.Drawing.SystemColors.ButtonFace

        TextBox101.ReadOnly = False
        TextBox101.BackColor = System.Drawing.SystemColors.Window

        Side1 = SideDef(ptFIX, StepDownFIXs(StepDownsNum).OutDir + fMaxTurnAngle, GuidNav.pPtPrj)
        If Side1 = 0 Then Side1 = 1
        Side2 = SideDef(ptFIX, StepDownFIXs(StepDownsNum).OutDir - fMaxTurnAngle, GuidNav.pPtPrj)
        If Side2 = 0 Then Side2 = 1

        If GuidNav.TypeCode = eNavaidType.DME Then
            If Side1 <> Side2 Then
                If ComboBox102.SelectedIndex = 0 Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = 0
                End If
            ElseIf Side1 < 0 Then
                If ComboBox102.SelectedIndex = 0 Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = 0
                End If
                ComboBox102.Enabled = False
            Else
                If ComboBox102.SelectedIndex = 1 Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = 1
                End If
                ComboBox102.Enabled = False
            End If
        Else
            If Side1 = Side2 Then
                If ComboBox102.SelectedIndex = 0 Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = 0
                End If
            ElseIf Side1 < 0 Then
                K = System.Math.Round(0.5 * (1 + SideDef(ptFIX, StepDownFIXs(StepDownsNum).OutDir, GuidNav.pPtPrj)))
                If ComboBox102.SelectedIndex = K Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = K
                End If
                ComboBox102.Enabled = False
            Else
                K = System.Math.Round(0.5 * (1 - SideDef(ptFIX, StepDownFIXs(StepDownsNum).OutDir, GuidNav.pPtPrj)))
                If ComboBox102.SelectedIndex = K Then
                    ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
                Else
                    ComboBox102.SelectedIndex = K
                End If
                ComboBox102.Enabled = False
            End If
        End If
    End Sub

    Private Sub ComboBox102_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox102.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim TurnDir As Integer
        Dim NavSide As Integer
        Dim fDist As Double
        Dim fIADir As Double
        Dim fAzimuth As Double
        Dim fOnNavRad As Double
        Dim fMaxTurnAngle As Double
        Dim fTmp As Double
        Dim fTmpAzt1 As Double
        Dim fTmpAzt0 As Double

        Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint

        Dim pIAF_IAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pIAF_IIAreaPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pNominalPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim GuidNav As NavaidData
        Dim ForFIX As StepDownFIX

        If ComboBox102.SelectedIndex < 0 Then Return
        If ComboBox101.SelectedIndex < 0 Then Return

        '==================================================================
        GuidNav = IAFNavDat(ComboBox101.SelectedIndex)
        ForFIX = StepDownFIXs(StepDownsNum)

        fDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ForFIX.pPtPrj)
        fOnNavRad = -10000.0

        If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
            fOnNavRad = VOR.OnNAVRadius
        ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
            fOnNavRad = NDB.OnNAVRadius
        End If

        TurnDir = 2 * ComboBox102.SelectedIndex - 1
        Dim mmaxStr As String

        mmaxStr = Strings.Right(ComboBox103.Text, 3)
        fMaxTurnAngle = 0.0

        If IsNumeric(mmaxStr) Then
            fMaxTurnAngle = System.Math.Abs(CDbl(mmaxStr))
        End If

        If fDist < fOnNavRad Then
            If TurnDir < 0 Then
                fTmpAzt0 = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum - 1).OutDir)
                fTmpAzt1 = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum - 1).OutDir + fMaxTurnAngle * TurnDir)
            Else
                fTmpAzt1 = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum - 1).OutDir)
                fTmpAzt0 = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum - 1).OutDir + fMaxTurnAngle * TurnDir)
            End If
            fTmp = CDbl(TextBox108.Text)
            fIADir = fTmp
            If Not AngleInSector(fTmp, fTmpAzt0, fTmpAzt1) Then
                'GuidNav.Tag = -1
                GuidNav.IntersectionType = eIntersectionType.OnNavaid

                'Dim f1 As Double
                'Dim f2 As Double
                '        f1 = Modulus(fTmpAzt0 - fTmp, 360.0)
                '        f2 = Modulus(fTmp - fTmpAzt1, 360.0)

                '        If f1 < f2 Then
                '            fTmp = fTmpAzt0
                '        Else
                '            fTmp = fTmpAzt1
                '        End If

                If AnglesSideDef(fTmpAzt0 + 0.5 * fMaxTurnAngle, fTmp) < 0 Then
                    fTmp = fTmpAzt1
                Else
                    fTmp = fTmpAzt0
                End If
            End If

            If fIADir <> fTmp Then
                TextBox108.Text = CStr(System.Math.Round(fTmp))
                TextBox109.Text = CStr(System.Math.Round(Modulus(fTmp - GuidNav.MagVar, 360.0)))
            End If

            pPtTmp = ToGeo(StepDownFIXs(StepDownsNum).pPtPrj)
            StepDownFIXs(StepDownsNum).pPtGeo = pPtTmp

            fIADir = Azt2Dir(pPtTmp, fTmp)
            ToolTip1.SetToolTip(TextBox108, My.Resources.str00210 + My.Resources.str00221 + CStr(Modulus(System.Math.Round(fTmpAzt0 + 0.49999), 360.0)) + "°" + My.Resources.str00222 + CStr(Modulus(System.Math.Round(fTmpAzt1 - 0.49999), 360.0)) + "°")
        Else
            fIADir = ReturnAngleInDegrees(ForFIX.pPtPrj, GuidNav.pPtPrj)
            If GuidNav.TypeCode = eNavaidType.DME Then
                fIADir = fIADir + 90.0 * TurnDir
                '            NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90.0, GuidNav.pPtPrj)
                '            If TurnDir = NavSide Then
                '                fIADir = fIADir + 90.0
                '            Else
                '                fIADir = fIADir - 90.0
                '            End If
            Else
                NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)
                If TurnDir = NavSide Then fIADir = Modulus(fIADir + 180.0, 360.0)

                '    If SubtractAngles(fIADir, ForFIX.OutDir) > fMaxTurnAngle Then fIADir = fIADir + 180.0

                '    NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)
                '    If SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90, GuidNav.pPtPrj) > 0 Then
                '        If TurnDir <> NavSide Then fIADir = fIADir + 180.0
                '    Else
                '        If TurnDir = NavSide Then fIADir = fIADir + 180.0
                '    End If
            End If

            fAzimuth = Dir2Azt(StepDownFIXs(StepDownsNum).pPtPrj, fIADir)
            TextBox108.Text = CStr(System.Math.Round(fAzimuth))
            TextBox109.Text = CStr(System.Math.Round(Modulus(fAzimuth - GuidNav.MagVar, 360.0)))
        End If

        StepDownFIXs(StepDownsNum).InDir = Modulus(fIADir, 360.0)
        '==================================================================
        On Error Resume Next
        If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
        If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
        If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)
        On Error GoTo 0

        'EstimateTRN_AreaObstacles
        'GuidNav = IAFNavDat(ComboBox101.ListIndex)
        'Set StepDownFIXs(StepDownsNum).pIAreaPoly = pIAF_IAreaPoly
        'Set StepDownFIXs(StepDownsNum).pIIAreaPoly = pIAF_IIAreaPoly

        StepDownFIXs(StepDownsNum).TurnDir = TurnDir
        CreateIAF_AreaPoly(GuidNav, StepDownFIXs(StepDownsNum), pIAF_IAreaPoly, pIAF_IIAreaPoly, pNominalPoly)

        pIAF_IIAreaElement = DrawPolygon(pIAF_IIAreaPoly, RGB(0, 255, 96))
        pIAF_IIAreaElement.Locked = True

        pIAF_IAreaElement = DrawPolygon(pIAF_IAreaPoly, RGB(235, 0, 96))
        pIAF_IAreaElement.Locked = True

        pNominalElement = DrawPolyLine(pNominalPoly, 255, 2)
        pNominalElement.Locked = True
    End Sub

    Private Sub ComboBox103_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox103.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim fDist As Double

        Dim fFromDistL As Double
        Dim fToDistL As Double
        Dim fFromDistR As Double
        Dim X As Double
        Dim fTmp As Double

        I = ComboBox103.SelectedIndex
        If I < 0 Then Return
        If MultiPage1.SelectedIndex < 1 Then Return

        If SubtractAngles(StepDownFIXs(StepDownsNum - 1).InDir, StepDownFIXs(StepDownsNum - 1).OutDir) < 5.0 Then
            If StepDownFIXs(StepDownsNum - 2).GuidanceNav.TypeCode <> eNavaidType.DME Then
                fDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 2).pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                'Else
                X = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 2).pPtPrj)
                fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
                X = DegToRad(SubtractAngles(fTmp, X))
                fTmp = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 2).pPtPrj, StepDownFIXs(StepDownsNum - 2).GuidanceNav.pPtPrj)
                fDist = fTmp * X
            End If
        Else
            fDist = 0.0
        End If

        fFromDistL = TurnIntervalsL(I).FromDist - fDist
        If fFromDistL < 0.0 Then fFromDistL = 0.0
        fToDistL = TurnIntervalsL(I).ToDist - fDist

        fFromDistR = TurnIntervalsR(I).FromDist - fDist
        If fFromDistR < 0.0 Then fFromDistR = 0.0

        If StepDownFIXs(StepDownsNum - 1).TurnDir <> 0 Then
            If fFromDistR < MinISlensArray(I) Then fFromDistR = MinISlensArray(I)
            If fFromDistL < MinISlensArray(I) Then fFromDistL = MinISlensArray(I)
        End If

        Label104.Text = ""

        If TurnIntervalsR(I).Tag > 0 Then
            If (fToDistL > fFromDistL) And (fToDistL >= 0) Then
                If fToDistL >= 0.5 * BigDist Then
                    Label103.Text = My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertDistance(fFromDistL, 3))
                Else
                    Label103.Text = My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertDistance(fFromDistL, 3)) + My.Resources.str00222 + CStr(ConvertDistance(fToDistL, 1))
                End If
                TextBox101.Text = CStr(ConvertDistance(fFromDistL, 3))
                TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs(True))
            Else
                Label103.Text = My.Resources.str00210 + My.Resources.str00233
                '            Label103.Caption = "[]"
                '            msgbox
            End If
        Else
            Label103.Text = My.Resources.str00210 + My.Resources.str00233
            '		Label103.Caption = "/-/"
        End If

        '    ComboBox101_Click
        'TextBox101_Validate true
        '    Label104.Caption = "Рекомендуемый интервал: от " + CStr(Round(Rmin + 0.4999)) + " до " + CStr(Round(Rmax - 0.4999))
        '    fTurnMinDist = fDist - MinISlensArray(ComboBox103.ListIndex)
        '    fRObs = IAFProhibSectors(IAFObstList4Turn(0).Index).rObs
    End Sub

    Private Sub ComboBox104_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox104.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim K As Integer

        Dim hFix As Double
        Dim fDir As Double
        Dim fDist As Double
        Dim fTmp As Double
        Dim fSlDist As Double
        Dim fRadius As Double
        Dim fInterDir As Double
        Dim fInterToler As Double
        Dim fIADir As Double
        Dim fInterRange As Double
        Dim dD As Double

        Dim fIF_SDFDist As Double

        Dim GuidNav As NavaidData
        Dim InterNav As NavaidData

        Dim pResGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim pIntersectPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pFixPoly As ESRI.ArcGIS.Geometry.IPointCollection

        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pBaseLine As ESRI.ArcGIS.Geometry.ILine
        Dim PtSDF As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim bDirChanged As Boolean

        On Error Resume Next
        If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)
        On Error GoTo 0

        K = ComboBox104.SelectedIndex
        If K < 0 Then
            Label111.Text = ""
            Return
        End If

        InterNav = SDFInterNavs(K)          'ComboBox104.SelectedItem
        Label111.Text = GetNavTypeName(InterNav.TypeCode)
        bDirChanged = False
        pBaseLine = New ESRI.ArcGIS.Geometry.Line

        If InterNav.IntersectionType = eIntersectionType.OnNavaid Then
            pClone = InterNav.pPtPrj
            PtSDF = pClone.Clone
            bDirChanged = True
            fIF_SDFDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).pPtPrj, InterNav.pPtPrj)
            Label127.Visible = False
            TextBox102.Visible = False
        Else
            Label127.Visible = True
            TextBox102.Visible = True
            Label127.Text = "°"

            fIF_SDFDist = DeConvertDistance(CDbl(TextBox101.Text))
            PtSDF = ptKT1
            If InterNav.IntersectionType = eIntersectionType.ByDistance Then
                Label127.Text = DistanceConverter(DistanceUnit).Unit
                pBaseLine.FromPoint = InterNav.pPtPrj
                pBaseLine.ToPoint = PtSDF
                fDir = RadToDeg(pBaseLine.Angle)
                fDist = pBaseLine.Length
                If fDist < 2.0 * DME.MinimalError Then
                    CircleVectorIntersect(InterNav.pPtPrj, 2.0 * DME.MinimalError, StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + 180.0, ptTmp)
                    If Not ptTmp.IsEmpty() Then
                        PtSDF = ptTmp
                        bDirChanged = True
                        fIF_SDFDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).pPtPrj, ptTmp)
                    Else
                        MessageBox.Show(My.Resources.str40220)
                        Return
                    End If
                End If
            End If
        End If

        hFix = fFIXHeight

        If bDirChanged Then
            TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + (fIF_SDFDist + StepDownFIXs(StepDownsNum - 1).TotalLength) * IAF_PDG 'fIF_SDFDist
            ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit)
            '        TextBox110.Text = CStr(ConvertHeight(ptKT1.Z, eRoundMode.rmNERAEST))   '+ " " + HeightConverter(HeightUnit).Unit
            If fFIXHeight > TextBox110Range.Right Then
                fFIXHeight = TextBox110Range.Right
                hFix = fFIXHeight
                TextBox110.Text = CStr(ConvertHeight(hFix, eRoundMode.NEAREST))
            End If

            PtSDF.Z = hFix
            ptKT1 = PtSDF
            '       TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + fIF_SDFDist * StepDownFIXs(StepDownsNum - 1).PDG

            TextBox101.Text = CStr(ConvertDistance(fIF_SDFDist, eRoundMode.NEAREST))

            StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
            StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
            StepDownFIXs(StepDownsNum).pPtStart = ptKT1
        End If

        '   GuidNav = IAFNavDat(ComboBox101.ListIndex)
        pBaseLine.ToPoint = PtSDF
        GuidNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        If GuidNav.TypeCode = eNavaidType.DME Then
            '=======================================================================================
            pBaseLine.FromPoint = GuidNav.pPtPrj
            fDir = RadToDeg(pBaseLine.Angle)
            fDist = pBaseLine.Length
            '        fTmp = hFIX - GuidNav.pPtPrj.Z
            '        fSlDist = Sqr(fDist * fDist + fTmp * fTmp)

            '        fTmp = fSlDist * (1# + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
            '        fRadius = fDist + fTmp
            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fDir + 90.0, 3 * fDist)
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fDir - 90.0, 3 * fDist)

            '        ClipByLine CreatePrjCircle(GuidNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing
            '        fTmp = fSlDist - fSlDist * (1# - DME.ErrorScalingUp) + DME.MinimalError
            '        fRadius = fDist - fTmp

            pTopoOper = pGuidPoly
            pTopoOper.Cut(pCutter, pTmpPoly, pFixPoly)

            pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
            fDist = ReturnDistanceInMeters(InterNav.pPtPrj, PtSDF)
            fInterDir = ReturnAngleInDegrees(InterNav.pPtPrj, PtSDF)
            fTmp = Dir2Azt(InterNav.pPtPrj, fInterDir)

            If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                Label112.Text = My.Resources.str00227
                TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
                fInterToler = VOR.IntersectingTolerance
            ElseIf InterNav.TypeCode = eNavaidType.NDB Then
                Label112.Text = My.Resources.str00228
                TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp + 180.0, 360.0)))
                fInterToler = NDB.IntersectingTolerance
            ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
                Label112.Text = My.Resources.str00227
                TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
                fInterToler = LLZ.IntersectingTolerance
            End If

            pIntersectPoly.AddPoint(InterNav.pPtPrj)
            pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir - fInterToler, 3.0 * fDist))
            pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir + fInterToler, 3.0 * fDist))
            pIntersectPoly.AddPoint(InterNav.pPtPrj)

            pTopoOper = pIntersectPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pFixPoly = pTopoOper.Intersect(pTmpPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            '        Set pGeomCollection = pFixPoly
            '        Set pResGeomCollection = pTmpPoly
            '        Set pRelation = pTmpPoly
            '
            '        If pGeomCollection.GeometryCount > 1 Then
            '            I = 0
            '            While I < pGeomCollection.GeometryCount
            '                If pResGeomCollection.GeometryCount > 1 Then
            '                    pResGeomCollection.RemoveGeometries 0, pResGeomCollection.GeometryCount
            '                End If
            '                pResGeomCollection.AddGeometry pGeomCollection.Geometry(I)
            '
            '                If pRelation.Contains(PtSDF) Then
            '                    I = I + 1
            '                Else
            '                    pGeomCollection.RemoveGeometries I, 1
            '                End If
            '            Wend
            '        End If
            '=======================================================================================
        Else
            '=======================================================================================
            pBaseLine.FromPoint = InterNav.pPtPrj
            fDir = RadToDeg(pBaseLine.Angle)
            fDist = pBaseLine.Length

            '        fIADir = Modulus(ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, GuidNav.pPtPrj) + 180.0 * (1 - ComboBox102.ListIndex), 360.0)
            fIADir = StepDownFIXs(StepDownsNum - 1).InDir

            '        If GuidNav.TypeCode = 0 Then
            '            TrackToler = VOR.TrackingTolerance
            ''            fConeAngle = VOR.ConeAngle
            '        ElseIf GuidNav.TypeCode = 2 Then
            '            TrackToler = NDB.TrackingTolerance
            ''            fConeAngle = NDB.ConeAngle
            '        End If

            '        TrackRange = GuidNav.Range / Cos(DegToRad(TrackToler))
            ''        fRConus = Tan(DegToRad(fConeAngle)) * (hFIX - GuidNav.pt.Z)

            '        Set pGuidPoly = New Polygon
            '        pGuidPoly.AddPoint GuidNav.pPtPrj
            '        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler, 100.0 * TrackRange)
            '        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler, 100.0 * TrackRange)
            '        pGuidPoly.AddPoint GuidNav.pPtPrj
            '        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler + 180.0, 100.0 * TrackRange)
            '        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler + 180.0, 100.0 * TrackRange)
            '        pGuidPoly.AddPoint GuidNav.pPtPrj

            '        Set pTopoOper = pGuidPoly
            '        pTopoOper.IsKnownSimple_2 = False
            '        pTopoOper.Simplify
            '================================================
            If InterNav.IntersectionType = eIntersectionType.ByDistance Then
                fTmp = SubtractAngles(fIADir, fDir)

                fTmp = hFix - InterNav.pPtPrj.Z
                fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)
                fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
                fRadius = fDist + fTmp

                Label112.Text = My.Resources.str00229    'fSlDist
                TextBox102.Text = CStr(ConvertDistance(fSlDist, eRoundMode.NEAREST))

                pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, fDir + 90.0, 2 * fRadius)
                pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, fDir - 90.0, 2 * fRadius)
                '            If Side > 0 Then
                ClipByLine(CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing)
                '            Else
                '                ClipByLine CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, Nothing, pTmpPoly, Nothing
                '            End If
                'DrawPolygon pTmpPoly, 255

                fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
                fRadius = fDist - fTmp
                pTopoOper = pTmpPoly
                pIntersectPoly = pTopoOper.Difference(CreatePrjCircle(InterNav.pPtPrj, fRadius))
                'DrawPolygon CreatePrjCircle(InterNav.pPtPrj, fRadius), 255
            ElseIf InterNav.IntersectionType = eIntersectionType.OnNavaid Then
                Label112.Text = My.Resources.str00106
                If InterNav.TypeCode = eNavaidType.NDB Then
                    NDBFIXTolerArea(InterNav.pPtPrj, fIADir, hFix, pFixPoly)
                Else
                    VORFIXTolerArea(InterNav.pPtPrj, fIADir, hFix, pFixPoly)
                End If
                '                Set pFixPoly =pIntersectPoly
            Else
                fTmp = Dir2Azt(InterNav.pPtPrj, fDir)
                If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
                    Label112.Text = My.Resources.str00227
                    TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
                    fInterToler = VOR.IntersectingTolerance
                ElseIf InterNav.TypeCode = eNavaidType.NDB Then
                    Label112.Text = My.Resources.str00228
                    TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp + 180.0, 360.0)))
                    fInterToler = NDB.IntersectingTolerance
                ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
                    Label112.Text = My.Resources.str00227
                    TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
                    fInterToler = LLZ.IntersectingTolerance
                End If
                fInterRange = InterNav.Range / System.Math.Cos(DegToRad(fInterToler))

                pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
                pIntersectPoly.AddPoint(InterNav.pPtPrj)
                pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler, 100.0 * fInterRange))
                pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler, 100.0 * fInterRange))
                pIntersectPoly.AddPoint(InterNav.pPtPrj)
                pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler + 180.0, 100.0 * fInterRange))
                pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler + 180.0, 100.0 * fInterRange))
                pIntersectPoly.AddPoint(InterNav.pPtPrj)
            End If

            If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
                pTopoOper = pIntersectPoly
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()
                pFixPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            End If
            '================================================
            ptTmp = PointAlongPlane(PtSDF, fIADir + 180.0, 100000.0)
            pCutter.FromPoint = PointAlongPlane(ptTmp, fIADir + 90.0, 100000.0)
            pCutter.ToPoint = PointAlongPlane(ptTmp, fIADir - 90.0, 100000.0)
            pProxi = pFixPoly
            dD = 100000.0 - pProxi.ReturnDistance(pCutter)
        End If
        '=======================================================================================

        pGeomCollection = pFixPoly

        If ((GuidNav.TypeCode = eNavaidType.DME) Or (InterNav.IntersectionType = eIntersectionType.ByDistance)) And (pGeomCollection.GeometryCount > 1) Then
            pResGeomCollection = pTmpPoly
            pRelation = pTmpPoly
            I = 0
            While I < pGeomCollection.GeometryCount
                If pResGeomCollection.GeometryCount > 0 Then
                    pResGeomCollection.RemoveGeometries(0, pResGeomCollection.GeometryCount)
                End If
                pResGeomCollection.AddGeometry(pGeomCollection.Geometry(I))

                If pRelation.Contains(PtSDF) Then
                    I = I + 1
                Else
                    pGeomCollection.RemoveGeometries(I, 1)
                End If
            End While
        End If

        If GuidNav.TypeCode = eNavaidType.DME Then
            dD = 0.0
            pBaseLine.FromPoint = GuidNav.pPtPrj
            pBaseLine.ToPoint = PtSDF
            fDir = pBaseLine.Angle

            For I = 0 To pFixPoly.PointCount - 1
                pBaseLine.ToPoint = pFixPoly.Point(I)
                fTmp = Modulus((pBaseLine.Angle - fDir) * (2 * ComboBox102.SelectedIndex - 1), 2 * PI)
                If fTmp > PI Then fTmp = fTmp - 2 * PI
                If fTmp > dD Then dD = fTmp
            Next I
            dD = dD * fDist
        End If

        StepDownFIXs(StepDownsNum).pFixPoly = pFixPoly

        TextBox104.Text = CStr(ConvertDistance(dD, eRoundMode.NEAREST))
        StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(pFixPoly, RGB(128, 255, 128))
        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(PtSDF, 0)

        StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
        StepDownFIXs(StepDownsNum).ptElem.Locked = True
    End Sub

    Private Sub CreateApproachGeometry()
        Dim I As Integer
        Dim Side As Integer
        Dim TurnDir As Integer

        Dim fTmp As Double
        Dim fIADir As Double
        Dim InterceptionType As Integer

        Dim pNominalPolyline As ESRI.ArcGIS.Geometry.IPolyline

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
        Dim DirFrom As Double
        Dim DirTo As Double
        Dim pPtWarn As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

        Dim pPtCntr As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtConstructor As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pPtCollection As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPtCollection2 As ESRI.ArcGIS.Geometry.IPointCollection

        pNominalPolyline = New ArcGIS.Geometry.Polyline()

        TurnDir = 2 * ComboBox102.SelectedIndex - 1

        If OptionButton101.Checked Then
            If StepDownFIXs(StepDownsNum - 1).Track = TrackType.Straight Then
                pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum - 1).pPtEnd
            Else
                fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj) '312.262013327147
                I = StepDownFIXs(StepDownsNum - 1).TurnDir
                If I = 0 Then
                    If Modulus(fTmp - StepDownFIXs(StepDownsNum - 1).OutDir, 360.0) > 180.0 Then
                        Side = 1
                    Else
                        Side = -1
                    End If
                ElseIf I > 0 Then
                    Side = 1
                Else
                    Side = -1
                End If

                pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum).pPtPrj, Side)
            End If
        Else
            InterceptionType = 2 * StepDownFIXs(StepDownsNum).Track + StepDownFIXs(StepDownsNum - 1).Track

            Select Case InterceptionType
                ''====================================================================================================
                Case 0
                    TurnAngle = (StepDownFIXs(StepDownsNum).InDir - StepDownFIXs(StepDownsNum - 1).InDir) * StepDownFIXs(StepDownsNum).TurnDir
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus(TurnAngle, 360)
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    Lwarn = arInitialApTurnRadius / System.Math.Tan(DegToRad(0.5 * (180 - TurnAngle)))
                    pPtWarn = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum - 1).InDir, Lwarn)

                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(ptKT1, StepDownFIXs(StepDownsNum).InDir - 180, Lwarn)

                    pPtCntr = New ESRI.ArcGIS.Geometry.Point
                    pPtConstructor = pPtCntr
                    pPtConstructor.ConstructAngleIntersection(pPtWarn, DegToRad(StepDownFIXs(StepDownsNum - 1).InDir + 90), StepDownFIXs(StepDownsNum).pPtStart, DegToRad(StepDownFIXs(StepDownsNum).InDir + 90))

                    pPtCollection = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                    If StepDownFIXs(StepDownsNum).Length > distEps Then
                        pPtCollection.AddPoint(StepDownFIXs(StepDownsNum - 1).pPtStart, 0)
                    End If
                    pNominalPolyline = pPtCollection

                    StepDownFIXs(StepDownsNum - 1).pPtEnd = pPtWarn
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
                Case 1
                    fIADir = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, ptKT1)

                    fTmp = fIADir - 90 * StepDownFIXs(StepDownsNum - 1).ArcDir
                    TurnDir = 2 * CShort(Modulus(StepDownFIXs(StepDownsNum).InDir - fTmp, 360) > 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    fTmp = Modulus(System.Math.Abs(StepDownFIXs(StepDownsNum).InDir - fIADir), 360)
                    If fTmp > 180 Then
                        fTmp = 360 - fTmp
                    End If

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
                    'DrawPoint pPtCntr, 0
                    pPtWarn = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).InDir + Alpha + fTmp, RDME)
                    StepDownFIXs(StepDownsNum - 1).pPtEnd = pPtWarn
                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(pPtCntr, StepDownFIXs(StepDownsNum).InDir + 90 * TurnDir, arInitialApTurnRadius)

                    If StepDownFIXs(StepDownsNum).Length > distEps Then
                        pPtCollection = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, pPtWarn, StepDownFIXs(StepDownsNum - 1).ArcDir)
                        pPtCollection2 = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                        pPtCollection.AddPointCollection(pPtCollection2)
                    Else
                        'If StepDownsNum >= 2 Then
                        '    Set StepDownFIXs(StepDownsNum - 2).pPtWarn = StepDownFIXs(StepDownsNum - 1).pPtWarn
                        'End If
                        pPtCollection = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, StepDownFIXs(StepDownsNum).TurnDir)
                    End If

                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)

                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * StepDownFIXs(StepDownsNum).TurnDir, 360)

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
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr

                    'DrawPoint pPtCntr, 255

                    pPtWarn = PointAlongPlane(pPtCntr, StepDownFIXs(StepDownsNum - 1).InDir + 90 * TurnDir, arInitialApTurnRadius)
                    StepDownFIXs(StepDownsNum - 1).pPtEnd = pPtWarn
                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + Alpha + fTmp, RDME)

                    'DrawPoint StepDownFIXs(StepDownsNum - 1).pPtStart, 255
                    'DrawPoint StepDownFIXs(StepDownsNum - 1).pPtEnd, 0

                    pNominalPolyline = New ESRI.ArcGIS.Geometry.Polyline
                    If StepDownFIXs(StepDownsNum).Length > distEps Then
                        pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                    End If
                    pNominalPolyline.ToPoint = pPtWarn
                    pPtCollection = pNominalPolyline

                    pPtCollection2 = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)

                    pPtCollection.AddPointCollection(pPtCollection2)
                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
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
                    Y = System.Math.Sqrt(fTmp * fTmp - X * X)

                    Side = SideDef(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirDME_DME, ptKT1)
                    ptTmp = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirDME_DME, X)
                    pPtCntr = PointAlongPlane(ptTmp, dirDME_DME - 90 * Side, Y)

                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, pPtCntr)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, pPtCntr)

                    StepDownFIXs(StepDownsNum).pPtStart = PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, dirToDME2, RDME2)
                    pPtWarn = PointAlongPlane(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, dirToDME1, RDME1)
                    StepDownFIXs(StepDownsNum - 1).pPtEnd = pPtWarn

                    If StepDownFIXs(StepDownsNum).Length > distEps Then
                        pPtCollection = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, pPtWarn, StepDownFIXs(StepDownsNum - 1).ArcDir)
                        'DrawPolyLine pPtCollection, , 2
                        'DrawPoint StepDownFIXs(StepDownsNum).pPtStart, RGB(128, 128, 128)
                        'DrawPoint pPtCntr, RGB(255, 128, 128)

                        pPtCollection2 = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)
                        pPtCollection.AddPointCollection(pPtCollection2)
                    Else
                        pPtCollection = CreateArcPolylinePrj(pPtCntr, pPtWarn, StepDownFIXs(StepDownsNum).pPtStart, TurnDir)
                    End If
                    pNominalPolyline = pPtCollection

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
            End Select

            StepDownFIXs(StepDownsNum - 1).pPtEnd.Z = ptKT1.Z
            StepDownFIXs(StepDownsNum).pPtStart.Z = ptKT1.Z
        End If
        '' ''====================================================================================================

        '    Set pTopo = pNominalPolyline
        '    pTopo.Cut pCutPoly, StepDownFIXs(StepDownsNum - 1).pNominalPoly, pTmpPoly
        '    Set pNominalPolyline = StepDownFIXs(StepDownsNum - 1).pNominalPoly
        '    pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum).pPtPrj

        StepDownFIXs(StepDownsNum - 1).pNominalPoly = pNominalPolyline

        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True
    End Sub

    Sub CreateArrivalGeometry()
        Dim I As Integer
        Dim Side As Integer
        Dim TurnDir As Integer
        Dim fTmp As Double
        Dim fIADir As Double
        Dim InterceptionType As Integer
        Dim pNominalPolyline As ESRI.ArcGIS.Geometry.IPolyline
        Dim TurnAngle As Double
        Dim DirFrom As Double
        Dim DirTo As Double

        Dim pPtCntr As ESRI.ArcGIS.Geometry.IPoint

        'DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, -1, 2)
        'ProcessMesages()

        pNominalPolyline = New ArcGIS.Geometry.Polyline()
        TurnDir = 2 * ComboBox102.SelectedIndex - 1

        'DrawPointWithText(ptKT1, "ptKT1")
        'ProcessMesages()

        If OptionButton101.Checked Then
            If StepDownFIXs(StepDownsNum - 1).Track = TrackType.Straight Then
                pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum).pPtStart
                StepDownFIXs(StepDownsNum).pPtCntr = Nothing
            Else
                fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj) '312.262013327147
                I = StepDownFIXs(StepDownsNum - 1).TurnDir
                If I = 0 Then
                    If Modulus(fTmp - StepDownFIXs(StepDownsNum - 1).OutDir, 360.0) > 180.0 Then
                        Side = 1
                    Else
                        Side = -1
                    End If
                ElseIf I > 0 Then
                    Side = 1
                Else
                    Side = -1
                End If

                pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum).pPtStart, Side)
            End If
        Else
            InterceptionType = 2 * StepDownFIXs(StepDownsNum).Track + StepDownFIXs(StepDownsNum - 1).Track

            Select Case InterceptionType
                ''====================================================================================================
                Case 0
                    TurnAngle = (StepDownFIXs(StepDownsNum).InDir - StepDownFIXs(StepDownsNum - 1).InDir) * StepDownFIXs(StepDownsNum).TurnDir
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus(TurnAngle, 360)
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    StepDownFIXs(StepDownsNum).pPtStart = ptKT1
                    pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                    pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum).pPtStart

                    StepDownFIXs(StepDownsNum).pPtCntr = Nothing 'pPtCntr
                Case 1
                    fIADir = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, ptKT1)

                    fTmp = fIADir - 90 * StepDownFIXs(StepDownsNum - 1).ArcDir
                    TurnDir = 2 * CShort(Modulus(StepDownFIXs(StepDownsNum).InDir - fTmp, 360) > 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    StepDownFIXs(StepDownsNum).pPtStart = ptKT1 'PointAlongPlane(pPtCntr, StepDownFIXs(StepDownsNum).InDir + 90 * TurnDir, arInitialApTurnRadius)
                    pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, ptKT1, StepDownFIXs(StepDownsNum - 1).ArcDir)

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)

                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * StepDownFIXs(StepDownsNum).TurnDir, 360)

                    StepDownFIXs(StepDownsNum).pPtCntr = Nothing
                    ''====================================================================================================
                Case 2
                    fIADir = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum).pPtPrj)

                    fTmp = fIADir + 90 * StepDownFIXs(StepDownsNum).ArcDir
                    TurnDir = 2 * CShort(Modulus(StepDownFIXs(StepDownsNum - 1).InDir - fTmp, 360) > 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    StepDownFIXs(StepDownsNum).pPtStart = ptKT1
                    pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
                    pNominalPolyline.ToPoint = ptKT1

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir 'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
                    StepDownFIXs(StepDownsNum).pPtCntr = Nothing
                    ''====================================================================================================
                Case 3
                    fTmp = Modulus(StepDownFIXs(StepDownsNum).OutDir - StepDownFIXs(StepDownsNum).InDir, 360)
                    TurnDir = 2 * CShort(fTmp < 180) + 1
                    StepDownFIXs(StepDownsNum).TurnDir_A = TurnDir

                    StepDownFIXs(StepDownsNum).pPtStart = ptKT1 'PointAlongPlane(StepDownFIXs(StepDownsNum).GuidanceNav.pPtPrj, dirToDME2, RDME2)

                    pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, ptKT1, StepDownFIXs(StepDownsNum - 1).ArcDir)

                    DirFrom = StepDownFIXs(StepDownsNum).OutDir             'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum - 1).pPtWarn)
                    DirTo = StepDownFIXs(StepDownsNum).InDir                'ReturnAngleInDegrees(pPtCntr, StepDownFIXs(StepDownsNum).pPtStart)
                    StepDownFIXs(StepDownsNum).TurnAngle = Modulus((DirTo - DirFrom) * TurnDir, 360)
                    StepDownFIXs(StepDownsNum).pPtCntr = pPtCntr
            End Select

            StepDownFIXs(StepDownsNum - 1).pPtEnd.Z = ptKT1.Z
            StepDownFIXs(StepDownsNum).pPtStart.Z = ptKT1.Z
        End If
        '====================================================================================================

        StepDownFIXs(StepDownsNum - 1).pNominalPoly = pNominalPolyline
        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True
    End Sub

    Private Sub AddBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles AddBtn.Click
        Dim I As Integer
        Dim N As Integer
        Dim Side As Integer
        Dim TurnDir As Integer

        Dim fTmp As Double
        Dim fIADir As Double

        Dim IAF_IAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim IAF_IIAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pNominalPolyline As ESRI.ArcGIS.Geometry.IPolyline

        Dim pCutPoly As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTmpLeftPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTmpRightPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim fDist As Double

        fDist = DeConvertDistance(CDbl(TextBox101.Text))

        If StepDownsNum = 0 Then
            StepDownFIXs(StepDownsNum).Name = TextBox005.Text
            StepDownFIXs(StepDownsNum).Role = pIFPoint.Role
        End If

        If OptionButton101.Checked Then
            If ComboBox104.Items.Count = 0 Then
                MessageBox.Show(My.Resources.str00514)
                Return
            End If

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

            StepDownFIXs(StepDownsNum).IntersectNav = SDFInterNavs(ComboBox104.SelectedIndex)
            StepDownFIXs(StepDownsNum).pPtPrj.Z = DeConvertHeight(CDbl(TextBox110.Text))

            StepDownFIXs(StepDownsNum).PDG = 0.01 * CDbl(TextBox103.Text)
            StepDownFIXs(StepDownsNum).InDir = StepDownFIXs(StepDownsNum).OutDir
            StepDownFIXs(StepDownsNum).Counted = 1
            StepDownFIXs(StepDownsNum).TurnDir = 0
            StepDownFIXs(StepDownsNum).TurnDir_A = 0
            TextBox110Range.Left = StepDownFIXs(StepDownsNum).pPtPrj.Z
            StepDownFIXs(StepDownsNum).TotalLength = 0
        Else
            If ComboBox101.Items.Count = 0 Then
                MessageBox.Show(My.Resources.str00515)
                Return
            End If

            If StepDownsNum <= 2 Then
                StepDownFIXs(StepDownsNum).TotalLength = 0
            Else
                StepDownFIXs(StepDownsNum).TotalLength = StepDownFIXs(StepDownsNum - 1).TotalLength + fDist
            End If

            TurnDir = 2 * ComboBox102.SelectedIndex - 1
            If fDist < distEps Then
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
                ElseIf Not bFirstPointIsIF Then
                    On Error Resume Next
                    If (StepDownsNum > 1) And Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
                    If Not StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
                    If Not StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
                    If Not StepDownFIXs(StepDownsNum - 1).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)
                    On Error GoTo 0

                    StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = StepDownFIXs(StepDownsNum).pIAreaPolyElem
                    StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = StepDownFIXs(StepDownsNum).pIIAreaPolyElem
                    StepDownFIXs(StepDownsNum - 1).pNominalElem = StepDownFIXs(StepDownsNum).pNominalElem

                    StepDownFIXs(StepDownsNum - 1).InDir = StepDownFIXs(StepDownsNum).InDir
                    StepDownFIXs(StepDownsNum - 1).OutDir = StepDownFIXs(StepDownsNum).InDir
                    StepDownsNum = StepDownsNum - 1
                    StepDownFIXs(StepDownsNum - 1).Counted = 0
                Else
                    StepDownFIXs(StepDownsNum - 1).Counted = 2
                End If
            Else
                If StepDownsNum > 0 Then
                    StepDownFIXs(StepDownsNum).Name = "TP"
                    StepDownFIXs(StepDownsNum).Role = CodeProcedureFixRole.TP
                End If

                StepDownFIXs(StepDownsNum).Counted = 1
                StepDownFIXs(StepDownsNum).IntersectNav.Index = -1
                StepDownFIXs(StepDownsNum).pPtPrj.Z = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z
                If StepDownsNum > 0 Then
                    StepDownFIXs(StepDownsNum).PDG = StepDownFIXs(StepDownsNum - 1).PDG
                Else
                    StepDownFIXs(StepDownsNum).PDG = IAF_PDG
                End If
            End If

            StepDownFIXs(StepDownsNum).GuidanceNav = IAFNavDat(ComboBox101.SelectedIndex)

            If StepDownFIXs(StepDownsNum).GuidanceNav.TypeCode = eNavaidType.DME Then
                StepDownFIXs(StepDownsNum).Track = TrackType.Arc

                fIADir = ReturnAngleInDegrees(IAFNavDat(ComboBox101.SelectedIndex).pPtPrj, StepDownFIXs(StepDownsNum).pPtPrj)
                fTmp = Modulus(StepDownFIXs(StepDownsNum).InDir - fIADir - 90.0, 360.0)
                If fTmp > 180.0 Then fTmp = 360.0 - fTmp
                StepDownFIXs(StepDownsNum).ArcDir = 2 * CShort(fTmp < 25.0) + 1
            Else
                StepDownFIXs(StepDownsNum).Track = TrackType.Straight
            End If

            StepDownFIXs(StepDownsNum).TurnDir = TurnDir

            On Error Resume Next
            If Not pIAF_IAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IAreaElement)
            If Not pIAF_IIAreaElement Is Nothing Then pGraphics.DeleteElement(pIAF_IIAreaElement)
            If Not pNominalElement Is Nothing Then pGraphics.DeleteElement(pNominalElement)
            On Error GoTo 0
        End If

        If fDist > distEps Then
            StepDownFIXs(StepDownsNum).Length = fDist
        Else
            '??????????????????????????????????????????????????????????????????????????????????
            '        If StepDownFIXs(StepDownsNum).TrackType = TrackTypeStraight Then
            '            StepDownFIXs(StepDownsNum).Length = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).pPtPrj, ptKT1)
            '        Else
            '            RDME = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum).GuidNav.pPtPrj, ptKT1)
            '            dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtPrj)
            '            dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum).GuidNav.pPtPrj, ptKT1)
            '            StepDownFIXs(StepDownsNum).Length = RDME * DegToRad(Modulus((dirToDME2 - dirToDME1) * StepDownFIXs(StepDownsNum).ArcDir, 360))
            '        End If
            '??????????????????????????????????????????????????????????????????????????????????
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
        'GetActiveView().Refresh() 'PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        If StepDownsNum >= 2 Then
            IAF_IAreaPoly = StepDownFIXs(StepDownsNum - 1).pIAreaPoly
            IAF_IIAreaPoly = StepDownFIXs(StepDownsNum - 1).pIIAreaPoly

            pCutPoly = New ESRI.ArcGIS.Geometry.Polyline
            pCutPoly.FromPoint = PointAlongPlane(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum).OutDir + 90.0, 100000.0)
            pCutPoly.ToPoint = PointAlongPlane(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum).OutDir - 90.0, 100000.0)

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

        If bFirstPointIsIF Then
            CreateApproachGeometry()
        Else
            CreateArrivalGeometry()
        End If

        '' ''====================================================================================================
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

        '================================================================================
        Dim SegObstList As ObstacleContainer
        Dim HPrevFIX As Double

        HPrevFIX = -BigDist

        For I = StepDownsNum - 1 To 0 Step -1
            If StepDownFIXs(I).TurnDir = 0 Then
                If Not StepDownFIXs(I).pPtPrj Is Nothing Then HPrevFIX = StepDownFIXs(I).pPtPrj.Z
                Exit For
            End If
        Next I

        If (StepDownsNum > 2) Or OptionButton101.Checked Then
            FillSegObstList(StepDownFIXs(StepDownsNum - 1), fRefHeight, StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, StepDownFIXs(StepDownsNum - 1).pIAreaPoly, SegObstList)
            InitialReportsFrm.AddSegment(SegObstList, HPrevFIX) ', IAFObstList4Turn
        End If

        RemoveBtn.Enabled = True
        StepDownsNum = StepDownsNum + 1

        If StepDownsNum > UboundFIXs Then
            UboundFIXs = UboundFIXs + 100
            ReDim Preserve StepDownFIXs(UboundFIXs)
        End If

        NextBtn.Enabled = StepDownsNum > 0

        '================================================================================

        EstimateIAF_AreaObstacles()

        If OptionButton101.Checked Then
            OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
        Else
            OptionButton101.Checked = True
        End If
    End Sub

    '---------------------------------------------------------------------------------------
    ' Procedure : InfoBtn_Click
    ' DateTime  : 06.09.2007 15:38
    ' Author    : RuslanA
    ' Purpose   :
    '---------------------------------------------------------------------------------------
    '
    Private Sub InfoBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ShowPanelBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ShowPanelBtn.Checked Then
            Me.Width = 706
            ShowPanelBtn.Image = My.Resources.bmpHIDE_INFO
        Else
            Me.Width = 523
            ShowPanelBtn.Image = My.Resources.bmpSHOW_INFO
        End If

        If NextBtn.Enabled Then
            NextBtn.Focus()
        Else
            PrevBtn.Focus()
        End If
    End Sub

    Private Sub ListView201_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView201.SelectedIndexChanged
        If ListView201.SelectedItems.Count = 0 Then Return

        Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
        Dim I As Integer
        Dim N As Integer

        On Error Resume Next
        If Not ptElem Is Nothing Then pGraphics.DeleteElement(ptElem)
        If Not pPlyElem Is Nothing Then pGraphics.DeleteElement(pPlyElem)
        On Error GoTo 0

        pPlyElem = Nothing

        N = UBound(LegList)
        I = Item.Index

        '    TextBox201.Text = CStr(ConvertHeight(LegList(I).M, eRoundMode.rmNERAEST))
        '    TextBox202.Text = CStr(ConvertHeight(LegList(I).M, eRoundMode.rmNERAEST))

        TextBox201.Text = Item.SubItems.Item(7).Text
        TextBox201.Tag = TextBox201.Text

        TextBox201.ReadOnly = Item.Index + 1 = N
        If TextBox201.ReadOnly Then
            TextBox201.BackColor = System.Drawing.SystemColors.ButtonFace
        Else
            TextBox201.BackColor = System.Drawing.SystemColors.Window
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

    Private Shared Function CreateNominalLine(ByRef ForFIX As StepDownFIX) As ESRI.ArcGIS.Geometry.IPolyline
        If ForFIX.GuidanceNav.TypeCode = eNavaidType.DME Then
            CreateNominalLine = CreateArcPolylinePrj(ForFIX.GuidanceNav.pPtPrj, ForFIX.pPtStart, ForFIX.pPtEnd, ForFIX.ArcDir)
        Else
            CreateNominalLine = New ESRI.ArcGIS.Geometry.Polyline
            CreateNominalLine.FromPoint = ForFIX.pPtStart
            CreateNominalLine.ToPoint = ForFIX.pPtEnd
        End If
    End Function

    Private Sub RemoveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles RemoveBtn.Click
        Dim N As Integer
        Dim I As Integer

        If StepDownsNum < 2 Then Return

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

        StepDownFIXs(StepDownsNum - 1).Counted -= 1

        If StepDownFIXs(StepDownsNum - 1).Counted <= 0 Then
            StepDownFIXs(StepDownsNum - 1).Counted = 0
            If Not StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem)
            If Not StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem)
            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)

            '    pGraphics.AddElement StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem, 0
            '    pGraphics.AddElement StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem, 0

            StepDownFIXs(StepDownsNum - 2).pIAreaPolyElem = Nothing
            StepDownFIXs(StepDownsNum - 2).pIIAreaPolyElem = Nothing
            StepDownFIXs(StepDownsNum - 2).pNominalElem = Nothing

            StepDownsNum = StepDownsNum - 1
        Else
            StepDownFIXs(StepDownsNum - 1).GuidanceNav = StepDownFIXs(StepDownsNum - 2).GuidanceNav
            StepDownFIXs(StepDownsNum - 1).InDir = StepDownFIXs(StepDownsNum - 1).OutDir
            StepDownFIXs(StepDownsNum - 1).TurnDir = 0
            StepDownFIXs(StepDownsNum - 1).TurnDir_A = 0
            StepDownFIXs(StepDownsNum - 1).pPtStart = StepDownFIXs(StepDownsNum - 1).pPtPrj

            If Not StepDownFIXs(StepDownsNum - 2).pNominalElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum - 2).pNominalElem)
            StepDownFIXs(StepDownsNum - 2).pPtEnd = StepDownFIXs(StepDownsNum - 1).pPtPrj

            m_pNominalPoly = CreateNominalLine(StepDownFIXs(StepDownsNum - 2))
            StepDownFIXs(StepDownsNum - 2).pNominalPoly = m_pNominalPoly
            StepDownFIXs(StepDownsNum - 2).pNominalElem = DrawPolyLine(m_pNominalPoly, 255, 2)
        End If

        InitialReportsFrm.DeleteSegment()

        On Error GoTo 0

        NextBtn.Enabled = StepDownsNum > 0

        CreateIAF_AreaPoly(StepDownFIXs(StepDownsNum - 1).GuidanceNav, StepDownFIXs(StepDownsNum - 1), m_IAF_IAreaPoly, m_IAF_IIAreaPoly, m_pNominalPoly)

        StepDownFIXs(StepDownsNum - 1).pIAreaPoly = m_IAF_IAreaPoly
        StepDownFIXs(StepDownsNum - 1).pIIAreaPoly = m_IAF_IIAreaPoly
        StepDownFIXs(StepDownsNum - 1).pNominalPoly = m_pNominalPoly

        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = DrawPolygon(m_IAF_IIAreaPoly, RGB(0, 255, 96))
        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem.Locked = True

        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = DrawPolygon(m_IAF_IAreaPoly, RGB(235, 0, 96))
        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem.Locked = True

        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(m_pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True

        TextBox110Range.Left = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z
        ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit)

        RemoveBtn.Enabled = StepDownsNum > 2

        EstimateIAF_AreaObstacles()

        If StepDownFIXs(StepDownsNum - 1).TurnDir <> 0 Then
            If OptionButton102.Checked Then
                OptionButton102_CheckedChanged(OptionButton102, New System.EventArgs())
            Else
                OptionButton101.Checked = True
            End If
        Else
            If OptionButton101.Checked Then
                OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
            Else
                OptionButton101.Checked = True
            End If
        End If
    End Sub

    'Private Sub MultiPage1_Click(PreviousTab As Integer)
    'If MultiPage1.Tag <> "0" Then Return
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

        NextBtn.Enabled = OptionButton101.Checked

        If OptionButton101.Checked Then
            Frame104.Text = My.Resources.str40223
        Else
            Frame104.Text = My.Resources.str40224
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

        Label108.Enabled = OptionButton102.Checked
        ComboBox103.Enabled = OptionButton102.Checked
        Label109.Enabled = OptionButton102.Checked
    End Sub

    Private Sub OptionButton101_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton101.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim fTmp As Double
        Dim Rmin As Double
        Dim Rmax As Double
        Dim FixRange(,) As LowHigh
        Dim dPrec As Double

        ChangeMode()
        StepDownFIXs(StepDownsNum).TurnDir = 0

        fFIXHeight = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z

        TextBox108.ReadOnly = True
        TextBox108.BackColor = System.Drawing.SystemColors.ButtonFace

        TextBox101.ReadOnly = False
        TextBox101.BackColor = System.Drawing.SystemColors.Window

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

            If (IAFObstList4FIX.Parts(0).Flags And 2) = 2 Then
                dDMin = IAFObstList4FIX.Parts(0).dMin15
            Else
                dDMin = BigDist
            End If

            N = UBound(FixRange, 2)
            fTmp = 0.0
            For I = 0 To N
                If FixRange(0, I).Tag > 0 Then
                    If FixRange(0, I).High > fTmp Then fTmp = FixRange(0, I).High
                End If
            Next I

            If Rmax > fTmp Then
                Rmax = fTmp
            End If

            J = -1
            N = UBound(IAFObstList4FIX.Parts)
            For I = 1 To N
                If Rmax + 9300.0 < IAFObstList4FIX.Parts(I).Dist Then
                    J = I - 1
                    Exit For
                End If

                If (dDMin > IAFObstList4FIX.Parts(I).dMin15) And ((IAFObstList4FIX.Parts(I).Flags And 2) = 2) Then dDMin = IAFObstList4FIX.Parts(I).dMin15
                If (Rmin < IAFObstList4FIX.Parts(I).Rmin) And (Rmax > IAFObstList4FIX.Parts(I).Rmin) Then Rmin = IAFObstList4FIX.Parts(I).Rmin

                K = UBound(FixRange, 2)
                fTmp = 0.0
                For J = 0 To K
                    If FixRange(I, J).Tag > 0 Then
                        If FixRange(I, J).High > fTmp Then fTmp = FixRange(I, J).High
                    End If
                Next J

                If Rmax > fTmp Then
                    Rmax = fTmp
                End If
                If Rmax > IAFObstList4FIX.Parts(I).Rmax Then
                    Rmax = IAFObstList4FIX.Parts(I).Rmax
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
                Label103.Text = My.Resources.str00210 + My.Resources.str00221 + CStr(fTmp) + My.Resources.str00222 + CStr(Rmax)
            Else
                Label103.Text = My.Resources.str00210 + My.Resources.str00233
            End If

            If Rmin <= Rmax Then
                Label104.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(Rmin) + My.Resources.str00222 + CStr(Rmax)
                TextBox101.Text = CStr(Rmin)
            Else
                Label104.Text = My.Resources.str00220 + My.Resources.str00233
                If fTmp <= Rmax Then
                    TextBox101.Text = CStr(Rmax)
                Else
                    TextBox101.Text = CStr(0.0)
                    '            msgbox "Данный маршрут не возможно продолжить в зтом направлении."
                    MessageBox.Show(My.Resources.str00516)
                End If
            End If
        Else
            Label103.Text = My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertDistance(dPrec, eRoundMode.NEAREST))    '+ My.Resources.str00222 + " "
            Label104.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertDistance(dPrec, eRoundMode.NEAREST))    '+ My.Resources.str00222 + " "

            '       Label103.Caption = "Допустимый интервал: от " + CStr(dPrec) '+ " до " + " " "Допустимый интервал: от " + CStr(Round(IAFObstList4FIX(0).Rmin + 0.4999)) + " до " + CStr(Round(Rmax - 0.4999))
            '       Label104.Caption = "Рекомендуемый интервал: от " + CStr(dPrec) ' + " до " + " "'"Рекомендуемый интервал: от " + CStr(Round(Rmin + 0.4999)) + " до " + CStr(Round(Rmax - 0.4999))

            Rmin = dPrec
            Rmax = 1.0E+16
            TextBox101.Text = CStr(ConvertDistance(dPrec, 3))
        End If

        If TextBox110.Tag = "" Then TextBox110.Tag = "a"
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs(True))
    End Sub

    Private Sub OptionButton102_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton102.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return

        ChangeMode()
        If ComboBox103.Items.Count = 0 Then
            Label103.Text = ""
            Label104.Text = ""
            AddBtn.Enabled = False
            MessageBox.Show(My.Resources.str00517)
        Else
            If ComboBox103.SelectedIndex = 0 Then
                ComboBox103_SelectedIndexChanged(ComboBox103, New System.EventArgs())
            Else
                ComboBox103.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub PrevBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click
        NextBtn.Enabled = True

        If MultiPage1.SelectedIndex = 1 Then
            ClearScr()
            MultiPage1.SelectedIndex = 0
            PrevBtn.Enabled = False
            ReportBtn.Enabled = False

            HelpContextID = 12100
            If Visible Then Activate()
        ElseIf MultiPage1.SelectedIndex = 2 Then
            'RemoveBtn_Click
            On Error Resume Next
            If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)
            If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)

            If Not ptElem Is Nothing Then pGraphics.DeleteElement(ptElem)
            If Not pPlyElem Is Nothing Then pGraphics.DeleteElement(pPlyElem)
            On Error GoTo 0
            MultiPage1.SelectedIndex = 1
        End If

        OkBtn.Enabled = False

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
    End Sub

    Private Sub ToStep1()
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone

        IAF_PDG = arIADescent_Nom.Value 'arIADescent_Max.Value
        '    IAF_PDG = 7.5
        StepDownsNum = 2
        '================================================================================
        pClone = pFAFptPrj
        StepDownFIXs(0).pPtPrj = pClone.Clone
        StepDownFIXs(0).pPtStart = pClone.Clone

        StepDownFIXs(0).GuidanceNav = StartPoint.GuidanceNav
        StepDownFIXs(0).IntersectNav = StartPoint.IntersectNav
        If bFirstPointIsIF Then
            StepDownFIXs(0).Role = Aran.Aim.Enums.CodeProcedureFixRole.IF
        Else
            StepDownFIXs(0).Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF ' TP
        End If

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

        StepDownFIXs(0).pPtEnd = ptKT1
        StepDownFIXs(1).pPtPrj = ptKT1
        StepDownFIXs(1).pPtStart = ptKT1

        StepDownFIXs(1).Length = ReturnDistanceInMeters(pFAFptPrj, ptKT1)

        '    If Not GetDefNames(False, True, False, HaveTP, False, _
        ''        ptFAP, Nothing, IFPnt, Nothing, pMAPt, PtSOC, TurnFixPnt, _
        ''        "", FAPName, "", IFName, MAPtName, SOCName, TPName, "") Then
        '        Return
        '    End If

        StepDownFIXs(1).Name = StartPoint.Name
        'StepDownFIXs(1).Role = 'StartPoint.TypeCode

        StepDownFIXs(1).PDG = arIADescent_Nom.Value
        StepDownFIXs(1).GuidanceNav = StepDownFIXs(0).GuidanceNav
        StepDownFIXs(1).IntersectNav = StepDownFIXs(0).IntersectNav

        StepDownFIXs(1).InDir = StepDownFIXs(0).InDir
        StepDownFIXs(1).OutDir = StepDownFIXs(0).InDir
        StepDownFIXs(1).TurnDir = 0

        CreateIAF_AreaPoly(StepDownFIXs(1).GuidanceNav, StepDownFIXs(1), m_IAF_IAreaPoly, m_IAF_IIAreaPoly, m_pNominalPoly)

        StepDownFIXs(1).pIAreaPoly = m_IAF_IAreaPoly
        StepDownFIXs(1).pIIAreaPoly = m_IAF_IIAreaPoly
        StepDownFIXs(1).pNominalPoly = m_pNominalPoly

        StepDownFIXs(1).pIIAreaPolyElem = DrawPolygon(m_IAF_IIAreaPoly, RGB(0, 255, 96))
        StepDownFIXs(1).pIIAreaPolyElem.Locked = True

        StepDownFIXs(1).pIAreaPolyElem = DrawPolygon(m_IAF_IAreaPoly, RGB(235, 0, 96))
        StepDownFIXs(1).pIAreaPolyElem.Locked = True

        StepDownFIXs(1).pNominalElem = DrawPolyLine(m_pNominalPoly, 255, 2)
        StepDownFIXs(1).pNominalElem.Locked = True

        '===============================================
        StepDownFIXs(2).OutDir = StepDownFIXs(1).InDir
        StepDownFIXs(2).GuidanceNav = StepDownFIXs(1).GuidanceNav '?????????????
        '===============================================
        '    TextBox1508_Validate true
        '    Set pGuidPoly = CreateGuidPoly(StepDownFIXs(StepDownsNum - 1).GuidNav, StepDownFIXs(StepDownsNum - 1).pPtPrj)
        '    Set pGuidPoly = CreateGuidPoly(StepDownFIXs(StepDownsNum - 1))
        If IsNumeric(TextBox103.Text) Then
            IAF_PDG = 0.01 * CDbl(TextBox103.Text)
            If IAF_PDG > arIADescent_Max.Value Then
                IAF_PDG = arIADescent_Max.Value
                TextBox103.Text = CStr(100.0 * IAF_PDG)
            End If
        Else
            IAF_PDG = arIADescent_Nom.Value
            TextBox103.Text = CStr(100.0 * IAF_PDG)
        End If
        EstimateIAF_AreaObstacles()

        TextBox110Range.Left = pIFptPrj.Z
        TextBox110Range.Right = pIFptPrj.Z + 100.0
        '===============================================
        If OptionButton101.Checked Then
            OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
        Else
            OptionButton101.Checked = True
        End If

        MultiPage1.Tag = "1"
        MultiPage1.SelectedIndex = 1
        ReportBtn.Enabled = True
        HelpContextID = 12200
    End Sub

    Private Sub CreateLegs()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim Side As Integer

        Dim fTmp As Double
        Dim RDME As Double
        Dim dirToDME1 As Double
        Dim dirToDME2 As Double

        Dim pClone As ArcGIS.esriSystem.IClone
        Dim pPointcollection1 As IPointCollection
        Dim pTmpPoly As IPointCollection
        Dim pPath As IPointCollection
        Dim pPath1 As IPath

        Dim pPolyline As IGeometryCollection
        Dim pTopo As ITopologicalOperator2

        Dim IAF_IIAreaPoly As IPolygon
        Dim IAF_IAreaPoly As IPolygon
        Dim pTmpRightPoly As IPolygon
        Dim pTmpLeftPoly As IPolygon

        Dim pNominalPolyline As IPolyline
        Dim pCutPoly As IPolyline
        Dim pPoly As IPolyline


        'StepDownFIXs(StepDownsNum).Name = "SDF"
        'StepDownFIXs(StepDownsNum).Role = CodeProcedureFixRole.SDF

        StepDownFIXs(StepDownsNum).GuidanceNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav
        StepDownFIXs(StepDownsNum).IntersectNav = SDFInterNavs(ComboBox104.SelectedIndex)
        StepDownFIXs(StepDownsNum).pPtPrj.Z = DeConvertHeight(CDbl(TextBox110.Text))

        StepDownFIXs(StepDownsNum).PDG = 0.01 * CDbl(TextBox103.Text)
        StepDownFIXs(StepDownsNum).InDir = StepDownFIXs(StepDownsNum).OutDir
        StepDownFIXs(StepDownsNum).TurnDir = 0
        StepDownFIXs(StepDownsNum).TurnDir_A = 0

        StepDownFIXs(StepDownsNum).Track = StepDownFIXs(StepDownsNum - 1).Track
        StepDownFIXs(StepDownsNum).ArcDir = StepDownFIXs(StepDownsNum - 1).ArcDir

		'If bFirstPointIsIF Then
		'	StepDownFIXs(StepDownsNum).pPtEnd = StepDownFIXs(StepDownsNum).pPtPrj
		'End If

		'=====================================================================================
		SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem)
        SafeDeleteElement(StepDownFIXs(StepDownsNum - 1).pNominalElem)

		StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = Nothing
        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = Nothing
        StepDownFIXs(StepDownsNum - 1).pNominalElem = Nothing

        N = UBound(IAFProhibSectors)
        For I = 0 To N
            SafeDeleteElement(IAFProhibSectors(I).ObsAreaElement)
            IAFProhibSectors(I).ObsAreaElement = Nothing
        Next I

        '=====================================================================================

        IAF_IAreaPoly = StepDownFIXs(StepDownsNum - 1).pIAreaPoly
        IAF_IIAreaPoly = StepDownFIXs(StepDownsNum - 1).pIIAreaPoly
        pNominalPolyline = StepDownFIXs(StepDownsNum - 1).pNominalPoly

        pCutPoly = New ESRI.ArcGIS.Geometry.Polyline
        pCutPoly.FromPoint = PointAlongPlane(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum).OutDir + 90.0, 100000.0)
        pCutPoly.ToPoint = PointAlongPlane(StepDownFIXs(StepDownsNum).pPtPrj, StepDownFIXs(StepDownsNum).OutDir - 90.0, 100000.0)

        ClipByLine(IAF_IIAreaPoly, pCutPoly, pTmpLeftPoly, pTmpRightPoly, pTmpPoly)

        pTopo = StepDownFIXs(StepDownsNum).pFixPoly
        pTopo.IsKnownSimple_2 = False

        pTopo.Simplify()

        StepDownFIXs(StepDownsNum - 1).pIIAreaPoly = pTopo.Union(pTmpLeftPoly)
        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIIAreaPoly, RGB(0, 96, 255))
        StepDownFIXs(StepDownsNum - 1).pIIAreaPolyElem.Locked = True

        ClipByLine(IAF_IAreaPoly, pCutPoly, pTmpLeftPoly, pTmpRightPoly, pTmpPoly)

        StepDownFIXs(StepDownsNum - 1).pIAreaPoly = pTopo.Union(pTmpLeftPoly)
        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum - 1).pIAreaPoly, RGB(235, 96, 0))
        StepDownFIXs(StepDownsNum - 1).pIAreaPolyElem.Locked = True

        '===================================

        If StepDownFIXs(StepDownsNum).Track = TrackType.Straight Then
            pNominalPolyline.FromPoint = StepDownFIXs(StepDownsNum - 1).pPtStart
            pNominalPolyline.ToPoint = StepDownFIXs(StepDownsNum).pPtPrj
        Else
            fTmp = ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj)

            I = StepDownFIXs(StepDownsNum - 1).TurnDir
            If I = 0 Then
                If Modulus(fTmp - StepDownFIXs(StepDownsNum - 1).OutDir, 360.0) > 180.0 Then
                    Side = 1
                Else
                    Side = -1
                End If
            ElseIf I > 0 Then
                Side = 1
            Else
                Side = -1
            End If

            pNominalPolyline = CreateArcPolylinePrj(StepDownFIXs(StepDownsNum - 1).GuidanceNav.pPtPrj, StepDownFIXs(StepDownsNum - 1).pPtStart, StepDownFIXs(StepDownsNum).pPtPrj, Side)
        End If

        '================================================================================
        StepDownFIXs(StepDownsNum - 1).pNominalPoly = pNominalPolyline
        StepDownFIXs(StepDownsNum - 1).pNominalElem = DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, 255, 2)
        StepDownFIXs(StepDownsNum - 1).pNominalElem.Locked = True
        '================================================================================

        StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(StepDownFIXs(StepDownsNum).pFixPoly, 255)
        StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True

        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(StepDownFIXs(StepDownsNum).pPtPrj, RGB(255, 0, 255))
        StepDownFIXs(StepDownsNum).ptElem.Locked = True
        '=================================================================================

        'DrawPolyLine(StepDownFIXs(StepDownsNum - 1).pNominalPoly, RGB(0, 255, 255), 2)
        'ProcessMesages()

        ReDim LegList(StepDownsNum * 2)
        J = -1

        For I = StepDownsNum To 1 Step -1
            If (I = 1) And (StepDownFIXs(I).Counted = 2) Then

                Exit For
                '            StepDownFIXs(I - 1).TurnAngle = StepDownFIXs(I).TurnAngle
                '            StepDownFIXs(I - 1).TurnDir = StepDownFIXs(I).TurnDir
                '            StepDownFIXs(I - 1).TurnDir_A = StepDownFIXs(I).TurnDir_A
                '            StepDownFIXs(I - 1).InDir = StepDownFIXs(I).InDir
                '            GoTo ContinueForI
            End If

            J += 1
            LegList(J) = StepDownFIXs(I)

            LegList(J).ArcDir = StepDownFIXs(I - 1).ArcDir
            LegList(J).GuidanceNav = StepDownFIXs(I - 1).GuidanceNav
            LegList(J).Track = StepDownFIXs(I - 1).Track

            'DrawPointWithText(StepDownFIXs(I).pPtPrj, "I")
            'DrawPointWithText(StepDownFIXs(I - 1).pPtPrj, "I-1")
            'DrawPolyLine(StepDownFIXs(I - 1).pNominalPoly, , 3)
            'DrawPolyLine(StepDownFIXs(I - 2).pNominalPoly, , 3)
            'ProcessMesages()

            If (StepDownFIXs(I - 1).TurnDir_A <> 0) And (StepDownFIXs(I - 1).TurnAngle > 70) And False Then
                'If (StepDownFIXs(I - 1).TurnDir_A <> 0) And (StepDownFIXs(I - 1).TurnAngle > 70) Then
                J = J + 1
                LegList(J) = LegList(J - 1)
                LegList(J).Tag = 1
                LegList(J).IntersectNav = StepDownFIXs(I - 2).GuidanceNav
                LegList(J).Role = CodeProcedureFixRole.OTHER_WPT '"OTHER"
                LegList(J).TurnDir_A = 0

                'Set LegList(J - 1).pPtGeo = ToGeo(LegList(J - 1).pPtPrj)
                'LegList(J).ArcDir = StepDownFIXs(I).ArcDir
                'LegList(J).TrackType = StepDownFIXs(I).TrackType

                If StepDownFIXs(I - 1).Track = TrackType.Straight Then
                    LegList(J - 1).pNominalPoly = New ESRI.ArcGIS.Geometry.Polyline

                    LegList(J - 1).pNominalPoly.FromPoint = StepDownFIXs(I - 1).pPtEnd
                    LegList(J - 1).pNominalPoly.ToPoint = StepDownFIXs(I - 1).pPtStart

                    LegList(J - 1).Length = ReturnDistanceInMeters(StepDownFIXs(I).pPtPrj, StepDownFIXs(I - 1).pPtStart)
                    LegList(J - 1).OutDir = LegList(J - 1).InDir
                    '================================================================
                    LegList(J).pPtPrj = StepDownFIXs(I - 1).pPtStart
                    LegList(J).pPtGeo = ToGeo(LegList(J).pPtPrj)

                    LegList(J).pNominalPoly = CreateArcPolylinePrj(StepDownFIXs(I - 1).pPtCntr, StepDownFIXs(I - 1).pPtStart, StepDownFIXs(I - 2).pPtEnd, -StepDownFIXs(I - 1).TurnDir_A)
                    LegList(J).Length = ReturnDistanceInMeters(StepDownFIXs(I - 1).pPtStart, StepDownFIXs(I - 1).pPtPrj)
                    ''====================================================================================================
                Else
                    LegList(J - 1).pNominalPoly = CreateArcPolylinePrj(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtEnd, StepDownFIXs(I - 1).pPtStart, -StepDownFIXs(I - 1).ArcDir)

                    fTmp = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtStart)
                    RDME = ReturnDistanceInMeters(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)

                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtStart)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)
                    LegList(J - 1).Length = RDME * DegToRad(Modulus((dirToDME2 - dirToDME1) * StepDownFIXs(I - 1).ArcDir, 360))

                    '================================================================
                    LegList(J).InDir = Modulus(fTmp - 90 * StepDownFIXs(I - 1).ArcDir, 360)
                    LegList(J).OutDir = LegList(J).InDir

                    LegList(J).pPtPrj = StepDownFIXs(I - 1).pPtStart
                    LegList(J).pPtGeo = ToGeo(LegList(J).pPtPrj)
                    LegList(J).pNominalPoly = CreateArcPolylinePrj(StepDownFIXs(I - 1).pPtCntr, StepDownFIXs(I - 1).pPtStart, StepDownFIXs(I - 2).pPtEnd, -StepDownFIXs(I - 1).TurnDir_A)

                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtPrj)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtStart)
                    LegList(J).Length = RDME * DegToRad(Modulus((dirToDME2 - dirToDME1) * StepDownFIXs(I - 1).ArcDir, 360))
                End If
                'LegList(J).pPtPrj.Z = StepDownFIXs(I - 1).pPtPrj.Z
            Else
                LegList(J).pPtGeo = ToGeo(StepDownFIXs(I).pPtPrj)

                '		If bFirstPointIsIF Then
                If bFirstPointIsIF Then
                    If (StepDownFIXs(I - 1).TurnDir_A <> 0) Then
                        If StepDownFIXs(I - 1).Track = TrackType.Straight Then
                            pPointcollection1 = CreateArcPolylinePrj(StepDownFIXs(I - 1).pPtCntr, StepDownFIXs(I - 1).pPtStart, StepDownFIXs(I - 2).pPtEnd, -StepDownFIXs(I - 1).TurnDir_A)
                            pPath = New ESRI.ArcGIS.Geometry.Path
                            pPath.AddPointCollection(pPointcollection1)

                            pPath1 = New ESRI.ArcGIS.Geometry.Path
                            pPath1.FromPoint = StepDownFIXs(I - 1).pPtEnd
                            pPath1.ToPoint = pPointcollection1.Point(0)

                            pPolyline = New ESRI.ArcGIS.Geometry.Polyline
                            pPolyline.AddGeometry(pPath1)
                            pPolyline.AddGeometry(pPath)
                            LegList(J).pNominalPoly = pPolyline
                            '=============================================
                        Else
                            pPolyline = CreateArcPolylinePrj(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtEnd, StepDownFIXs(I - 1).pPtStart, -StepDownFIXs(I - 1).ArcDir)

                            pPath = New ESRI.ArcGIS.Geometry.Path
                            pPointcollection1 = CreateArcPolylinePrj(StepDownFIXs(I - 1).pPtCntr, StepDownFIXs(I - 1).pPtStart, StepDownFIXs(I - 2).pPtEnd, -StepDownFIXs(I - 1).TurnDir_A)
                            pPath.AddPointCollection(pPointcollection1)
                            pPolyline.AddGeometry(pPath)
                            LegList(J).pNominalPoly = pPolyline
                        End If
                    Else
                        If StepDownFIXs(I - 1).Track = TrackType.Straight Then
                            LegList(J).pNominalPoly = New ESRI.ArcGIS.Geometry.Polyline

                            LegList(J).pNominalPoly.FromPoint = StepDownFIXs(I - 1).pPtEnd
                            LegList(J).pNominalPoly.ToPoint = StepDownFIXs(I - 1).pPtStart
                        Else
                            LegList(J).pNominalPoly = CreateArcPolylinePrj(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtEnd, StepDownFIXs(I - 1).pPtStart, -StepDownFIXs(I - 1).ArcDir)
                        End If
                    End If
                ElseIf Not StepDownFIXs(I - 1).pNominalPoly Is Nothing Then
                    pClone = StepDownFIXs(I - 1).pNominalPoly
                    pPoly = pClone.Clone()
                    pPoly.ReverseOrientation()
                    LegList(J).pNominalPoly = pPoly
                End If

                'DrawPolyLine(LegList(J - 1).pNominalPoly, 255, 5)
                'ProcessMesages()

                If StepDownFIXs(I - 1).Track = TrackType.Straight Then
                    LegList(J).Length = ReturnDistanceInMeters(StepDownFIXs(I - 1).pPtPrj, StepDownFIXs(I).pPtPrj)
                Else
                    RDME = ReturnDistanceInMeters(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)
                    dirToDME1 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I - 1).pPtPrj)
                    dirToDME2 = ReturnAngleInDegrees(StepDownFIXs(I - 1).GuidanceNav.pPtPrj, StepDownFIXs(I).pPtPrj)
                    LegList(J).Length = RDME * DegToRad(Modulus((dirToDME2 - dirToDME1) * StepDownFIXs(I - 1).ArcDir, 360))
                End If
            End If

            'If I = 1 Then LegList(J).Role = Aran.Aim.Enums.CodeProcedureFixRole.IF
        Next I

        ReDim Preserve LegList(J)
    End Sub

    Private Sub ToStep2()
        Dim I As Integer
        Dim N As Integer
        Dim fTmp As Double

        'DrawPolyLine(StepDownFIXs(I - 1).pNominalPoly, RGB(0, 255, 255), 2)
        'ProcessMesages()

        CreateLegs()

        ListView201.Items.Clear()
        N = UBound(LegList)
        For I = 0 To N - 1
            Dim itmX As ListViewItem = ListView201.Items.Add(LegList(I).Role.ToString())

            If LegList(I).Track = TrackType.Straight Then
                fTmp = Dir2Azt(LegList(I).pPtPrj, LegList(I).OutDir)
                If itmX.SubItems.Count > 1 Then
                    itmX.SubItems(1).Text = CStr(System.Math.Round(fTmp, 1))
                Else
                    itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(fTmp, 1))))
                End If
            Else
                fTmp = Dir2Azt(LegList(I).GuidanceNav.pPtPrj, ReturnAngleInDegrees(LegList(I).GuidanceNav.pPtPrj, LegList(I).pPtPrj))
                If itmX.SubItems.Count > 1 Then
                    itmX.SubItems(1).Text = CStr(System.Math.Round(fTmp, 1)) + " (Boundary course)"
                Else
                    itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(fTmp, 1)) + " (Boundary course)"))
                End If
            End If

            If itmX.SubItems.Count > 2 Then
                itmX.SubItems(2).Text = CStr(ConvertDistance(LegList(I).Length, eRoundMode.NEAREST))
            Else
                itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(LegList(I).Length, eRoundMode.NEAREST))))
            End If

            If LegList(I).TurnDir_A <> 0 Then
                If itmX.SubItems.Count > 3 Then
                    itmX.SubItems(3).Text = CStr(System.Math.Round(LegList(I).TurnAngle, 1))
                Else
                    itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(LegList(I).TurnAngle, 1))))
                End If
            ElseIf itmX.SubItems.Count <= 3 Then
                itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
            End If

            If itmX.SubItems.Count > 4 Then
                itmX.SubItems(4).Text = LegList(I).GuidanceNav.CallSign + "/" + GetNavTypeName(LegList(I).GuidanceNav.TypeCode)
            Else
                itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, LegList(I).GuidanceNav.CallSign + "/" + GetNavTypeName(LegList(I).GuidanceNav.TypeCode)))
            End If

            fTmp = Dir2Azt(LegList(I).pPtPrj, LegList(I).InDir)
            If itmX.SubItems.Count > 5 Then
                itmX.SubItems(5).Text = Str(System.Math.Round(fTmp, 1))
            Else
                itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Str(System.Math.Round(fTmp, 1))))
            End If

            fTmp = Dir2Azt(LegList(I).pPtPrj, LegList(I).OutDir)
            If itmX.SubItems.Count > 6 Then
                itmX.SubItems(6).Text = Str(System.Math.Round(fTmp, 1))
            Else
                itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Str(System.Math.Round(fTmp, 1))))
            End If

            If I < N Then
                LegList(I).MinAlt = LegList(I + 1).pPtPrj.Z
                fTmp = ConvertHeight(LegList(I).MinAlt, eRoundMode.NEAREST)
                If itmX.SubItems.Count > 7 Then
                    itmX.SubItems(7).Text = Str(System.Math.Round(fTmp, 3))
                Else
                    itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Str(System.Math.Round(fTmp, 3))))
                End If
            Else
                LegList(I).MinAlt = LegList(I).pPtPrj.Z
                If itmX.SubItems.Count > 7 Then
                    itmX.SubItems(7).Text = ""
                Else
                    itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                End If
            End If

            LegList(I).MaxAlt = LegList(I).pPtPrj.Z
            fTmp = ConvertHeight(LegList(I).MaxAlt, eRoundMode.NEAREST)
            If itmX.SubItems.Count > 8 Then
                itmX.SubItems(8).Text = Str(System.Math.Round(fTmp, 3))
            Else
                itmX.SubItems.Insert(8, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Str(System.Math.Round(fTmp, 3))))
            End If

            '        If I < N Then
            '            itmX.SubItems(2) = CStr(ConvertDistance(LegList(I).Length, eRoundMode.rmNERAEST))

            '            If StepDownFIXs(I).TurnDir_A <> 0 Then
            '                itmX.SubItems(3) = CStr(Round(LegList(I).TurnAngle, 1))
            '            Else
            '                'itmX.SubItems(3) = "0"
            '            End If
            '        End If
        Next I

        '    For I = StepDownsNum To 1 Step -1
        '        If I = 1 Then
        '            Set itmX = ListView201.ListItems.Add(, , "IF")
        '        ElseIf I = StepDownsNum Then
        '            Set itmX = ListView201.ListItems.Add(, , "IAF")
        '        Else
        '            Set itmX = ListView201.ListItems.Add(, , StepDownFIXs(I).WPT_Type)
        '        End If
        '
        '        fTmp = Dir2Azt(StepDownFIXs(I).pPtPrj, StepDownFIXs(I).OutDir)
        '        itmX.SubItems(1) = CStr(Round(fTmp, 1))
        '        itmX.SubItems(4) = StepDownFIXs(I - 1).GuidNav.CallSign + "/" + StepDownFIXs(I - 1).GuidNav.TypeName
        '
        '        If I > 1 Then
        '            itmX.SubItems(2) = CStr(ConvertDistance(StepDownFIXs(I).Length, eRoundMode.rmNERAEST))
        '
        '            If StepDownFIXs(I).WPT_Type = "TP" Then
        '                itmX.SubItems(3) = CStr(Round(StepDownFIXs(I).TurnAngle, 1))
        '            Else
        '                'itmX.SubItems(3) = "0"
        '            End If
        '        End If
        '    Next I
        '=================================================================================
        MultiPage1.Tag = "2"
        MultiPage1.SelectedIndex = 2
        HelpContextID = 12200

        NextBtn.Enabled = False
        OkBtn.Enabled = True
    End Sub

    Private Sub NextBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
        If MultiPage1.SelectedIndex = 0 Then
            If (StartPoint.GuidanceNav.CallSign = "") Or (StartPoint.IntersectNav.CallSign = "") Then
                Exit Sub
            End If

            ToStep1()
        ElseIf MultiPage1.SelectedIndex = 1 Then
            If ComboBox104.Items.Count = 0 Then
                MessageBox.Show(My.Resources.str00514)
                Return
            End If

            ToStep2()
        End If

        PrevBtn.Enabled = True
        MultiPage1.Tag = "0"

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
    End Sub

    Private Sub OkBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OkBtn.Click
        Dim pReport As ReportHeader
        Dim RepFileName As String
        Dim RepFileTitle As String

        If ShowSaveDialog(RepFileName, RepFileTitle) Then
            pReport.Aerodrome = ""
            pReport.Database = gAranEnv.ConnectionInfo.Database

            pReport.Procedure = "_______"
            'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition

            pReport.Category = Chr(Category + Asc("A"))

            'If bFirstPointIsIF Then
            '	pReport.Procedure = IAPArray(ComboBox002.SelectedIndex).Name
            'Else
            '	pReport.Procedure = "" 'My.Resources.str00003
            'End If

            SaveLog(RepFileName, RepFileTitle, pReport)
            SaveProtocol(RepFileName, RepFileTitle, pReport)

            If SaveProcedure() Then
                Me.Close()
            End If
        End If
    End Sub

    Private Function InitialApproachLeg(ByVal Index As Integer, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
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

        Dim pApproachLeg As ApproachLeg
        Dim pSegmentLeg As SegmentLeg
        Dim pStartPoint As TerminalSegmentPoint
        Dim pDistanceIndication As DistanceIndication
        Dim pAngleIndication As AngleIndication

        Dim pSegmentPoint As SegmentPoint
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical
        Dim pSpeed As ValSpeed

        Dim mUomSpeed As UomSpeed
        Dim mUomHDistance As UomDistance
        Dim mUomVDistance As UomDistanceVertical

        Dim pLocation As Aran.Geometries.Point

        Dim uomSpeedTab() As UomSpeed
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomDistHorzTab() As UomDistance
        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData
        Dim SttIntersectNav As NavaidData
        Dim EndIntersectNav As NavaidData
        Dim ptStart As ESRI.ArcGIS.Geometry.IPoint
        Dim ptEnd As ESRI.ArcGIS.Geometry.IPoint

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        GuidNav = LegList(Index).GuidanceNav
        SttIntersectNav = LegList(Index).IntersectNav
        ptStart = LegList(Index).pPtPrj
        N = UBound(LegList)

        If Index < N - 1 Then
            EndIntersectNav = LegList(Index + 1).IntersectNav
            ptEnd = LegList(Index + 1).pPtPrj
        End If

        pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)

        pSegmentLeg = pApproachLeg

        '    pSegmentLeg.AltitudeOverrideATC =
        '    pSegmentLeg.AltitudeOverrideReference =
        '    pDuration = New Duration()
        '    pDuration.Uom = UomDuration_MIN
        '    pDuration.Value = CDbl(TextBox0602.Text)
        '    pSegmentLeg.Duration = pDuration
        '    pSegmentLeg.Note
        '    pSegmentLeg.ProcedureTurnRequired =
        '    pSegmentLeg.ReqNavPerformance
        '    pSegmentLeg.SpeedInterpretation =

        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        If LegList(Index).TurnDir_A = 1 Then
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
        ElseIf StepDownFIXs(Index).TurnDir_A = -1 Then
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
        Else
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        End If

        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN

        '    pSegmentLeg.EndConditionDesignator =
        '        pSegmentLeg.CourseDirection =
        '        pSegmentLeg.ProcedureTransition
        '        pSegmentLeg.StartPoint
        'pTransition.Type = ProcedurePhaseType_APPROACH

        '=======================================================================================
        If GuidNav.TypeCode = eNavaidType.DME Then
            Dim pArcCentre As TerminalSegmentPoint

            pSegmentLeg.LegPath = CodeTrajectory.ARC
            pSegmentLeg.LegTypeARINC = CodeSegmentPath.AF

            If SideDef(ptStart, LegList(Index).OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CW
            Else
                pSegmentLeg.CourseDirection = CodeDirectionReference.OTHER_CCW
            End If

            pArcCentre = New TerminalSegmentPoint()
            pArcCentre.Waypoint = False
            pArcCentre.PointChoice = GuidNav.GetSignificantPoint()
            'pArcCentre.PointChoice.NavaidSystem = GuidNav.GetSignificantPoint.NavaidSystem
            pSegmentLeg.ArcCentre = pArcCentre

            pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
        Else
            fCourseDir = LegList(Index).OutDir

            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT
            pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF
            If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = CodeDirectionReference.FROM
            Else
                pSegmentLeg.CourseDirection = CodeDirectionReference.TO
            End If
            pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
        End If

        'If LegList(Index).Track = TrackType.Straight Then
        'Else
        '	If pSegmentLeg.CourseDirection = CodeDirectionReference.FROM Then
        '		pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
        '	Else
        '		pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(ptStart, GuidNav.pPtPrj))
        '	End If
        'End If

        '=======================================================================================
        pSegmentLeg.BankAngle = arInitApprBank
        '=======================================================================================

        If Index < N - 1 Then
            pDistanceVertical = New ValDistanceVertical
            pDistanceVertical.Uom = mUomVDistance

            pDistanceVertical.Value = Math.Round(ConvertHeight(ptEnd.Z, eRoundMode.NEAREST), 4)

            pSegmentLeg.LowerLimitAltitude = pDistanceVertical

            'If Not Aran.Aim.Validation.DataTypeValidator.ValDistanceVerticalType(pSegmentLeg.LowerLimitAltitude) Then
            '	MsgBox("p1")
            'End If
        Else
        End If

        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance

        pDistanceVertical.Value = Math.Round(ConvertHeight(ptStart.Z, eRoundMode.NEAREST), 4)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical

        'If Not Aran.Aim.Validation.DataTypeValidator.ValDistanceVerticalType(pSegmentLeg.UpperLimitAltitude) Then
        '	MsgBox("p2")
        'End If

        '=======================================================================================
        pDistance = New ValDistance
        pDistance.Uom = mUomHDistance
        pDistance.Value = ConvertDistance(LegList(Index).Length, eRoundMode.NEAREST)

        pSegmentLeg.Length = pDistance
        '======
        pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(LegList(Index).PDG))
        '======

        pSpeed = New ValSpeed
        pSpeed.Uom = mUomSpeed
        pSpeed.Value = ConvertSpeed(cViafMax.Values(Category), eRoundMode.SPECIAL)
        pSegmentLeg.SpeedLimit = pSpeed
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        ' StarPoint ========================
        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        If Index > 0 Then
            pStartPoint = pEndPoint
        Else
            pStartPoint = New TerminalSegmentPoint()
            pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF

            pSegmentPoint = pStartPoint
            'pSegmentPoint.FlyOver = False
            pSegmentPoint.RadarGuidance = False
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
            pSegmentPoint.Waypoint = False

            ' ========================
            pPointReference = New PointReference()
            pInterNavSignPt = SttIntersectNav.GetSignificantPoint()

            ' Start Point ==================================================
            If GuidNav.TypeCode = eNavaidType.DME Then
                fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart) +
                 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW))
            End If

            If SttIntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                Dim HorAccuracy As Double = 0.0
                If (GuidNav.TypeCode <> eNavaidType.DME) And (SttIntersectNav.Identifier <> Guid.Empty) Then
                    HorAccuracy = CalcHorisontalAccuracy(ptStart, GuidNav, SttIntersectNav)
                End If

                pFixDesignatedPoint = CreateDesignatedPoint(ptStart, , fCourseDir)
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

                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                    pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                    pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pAngleUse = New AngleUse()
                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = True

                    pPointReference.FacilityAngle.Add(pAngleUse)
                End If

                ' ========================

                If SttIntersectNav.TypeCode = eNavaidType.DME Then
                    fDistToNav = ReturnDistanceInMeters(SttIntersectNav.pPtPrj, ptStart)
                    fAltitudeMin = ptStart.Z - SttIntersectNav.pPtPrj.Z
                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
                    pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                    pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                    pPointReference.Role = CodeReferenceRole.RAD_DME
                Else
                    pAngleUse = New AngleUse
                    fDir = ReturnAngleInDegrees(SttIntersectNav.pPtPrj, ptStart)

                    Angle = Modulus(Dir2Azt(SttIntersectNav.pPtPrj, fDir) - SttIntersectNav.MagVar, 360.0)
                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
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
            Dim pTolerArea As ESRI.ArcGIS.Geometry.IPolygon
            pTolerArea = LegList(Index).pFixPoly

            If Not pTolerArea Is Nothing Then
                PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

                pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                pPointReference.PriorFixTolerance = pDistanceSigned

                pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                pPointReference.PostFixTolerance = pDistanceSigned
                pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
            End If
            '=================================================================

            pStartPoint.FacilityMakeup.Add(pPointReference)
            pSegmentPoint.PointChoice = pFIXSignPt
            ' End Of Start Point ===================================
        End If

        pSegmentLeg.StartPoint = pStartPoint
        ' End Of Start Point ========================

        ' EndPoint ========================
        pEndPoint = Nothing
        If Index < N - 1 Then
            pEndPoint = New TerminalSegmentPoint
            pSegmentPoint = pEndPoint

            If LegList(Index).Tag <> 0 Then
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP '??????????????????????
                'pSegmentPoint.FlyOver = True
            ElseIf LegList(Index).TurnDir_A = 0 Then
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.SDF
                'pSegmentPoint.FlyOver = True
            Else
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP
                'pSegmentPoint.FlyOver = False
            End If

            pSegmentPoint.RadarGuidance = False
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
            pSegmentPoint.Waypoint = False
            '        pEndPoint.IndicatorFACF =      ??????????
            '        pEndPoint.LeadDME =            ??????????
            '        pEndPoint.LeadRadial =         ??????????


            If GuidNav.TypeCode = eNavaidType.DME Then
                fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd) +
                 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW))
                'Azt2DirPrj(ptStart, pSegmentLeg.Course)
            End If

            pPointReference = New PointReference()
            ' Indication ======================================================================

            Dim bOnNav As Boolean
            bOnNav = False

            If (EndIntersectNav.Identifier <> Guid.Empty) Then
                pInterNavSignPt = EndIntersectNav.GetSignificantPoint()

                If ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd) < 1.0 Then EndIntersectNav.IntersectionType = eIntersectionType.OnNavaid
                If EndIntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
                    bOnNav = True
                    pFIXSignPt = pInterNavSignPt
                End If
            End If

            If Not bOnNav Then
                Dim HorAccuracy As Double = 0.0
                If (GuidNav.TypeCode <> eNavaidType.DME) And (EndIntersectNav.Identifier <> Guid.Empty) Then
                    HorAccuracy = CalcHorisontalAccuracy(ptEnd, GuidNav, EndIntersectNav)
                End If

                pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, , fCourseDir)
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

                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
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

            If EndIntersectNav.Identifier <> Guid.Empty Then
                If EndIntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
                    pFIXSignPt = pInterNavSignPt
                    pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
                Else
                    If EndIntersectNav.TypeCode = eNavaidType.DME Then
                        fDistToNav = ReturnDistanceInMeters(EndIntersectNav.pPtPrj, ptEnd)
                        fAltitudeMin = ptEnd.Z - EndIntersectNav.pPtPrj.Z

                        fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
                        pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                        pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                        pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                        pPointReference.Role = CodeReferenceRole.RAD_DME
                    Else
                        pAngleUse = New AngleUse
                        fDir = ReturnAngleInDegrees(EndIntersectNav.pPtPrj, ptEnd)

                        Angle = Modulus(Dir2Azt(EndIntersectNav.pPtPrj, fDir) - EndIntersectNav.MagVar, 360.0)
                        pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                        pAngleIndication.TrueAngle = Dir2Azt(ptEnd, fDir)
                        pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                        pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

                        pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                        pAngleUse.AlongCourseGuidance = False

                        pPointReference.FacilityAngle.Add(pAngleUse)
                        pPointReference.Role = CodeReferenceRole.INTERSECTION
                    End If
                End If
                '=================================================================
                Dim pEndTolerArea As ESRI.ArcGIS.Geometry.IPolygon
                pEndTolerArea = LegList(Index + 1).pFixPoly

                If Not pEndTolerArea Is Nothing Then
                    PriorPostFixTolerance(pEndTolerArea, ptEnd, fCourseDir, PriorFixTolerance, PostFixTolerance)

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                    pDistanceSigned.Uom = mUomHDistance
                    pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                    pPointReference.PriorFixTolerance = pDistanceSigned

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                    pDistanceSigned.Uom = mUomHDistance
                    pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                    pPointReference.PostFixTolerance = pDistanceSigned
                    pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pEndTolerArea))
                End If
                '=================================================================

                pSegmentPoint.PointChoice = pFIXSignPt
            End If

            pEndPoint.FacilityMakeup.Add(pPointReference)
        Else
            pEndPoint = pIFPoint
        End If

        pSegmentLeg.EndPoint = pEndPoint

        ' End of EndPoint ========================

        ' Trajectory ===========================================
        Dim J As Integer
        Dim I As Integer
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

        pCurve = New Curve
        pPolyline = LegList(Index).pNominalPoly

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

        Return pApproachLeg
        ' END ====================================================
    End Function

    Private Function CreateArrivalLeg(ByVal Index As Integer, ByVal pProcedure As StandardInstrumentArrival, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ArrivalLeg
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

        Dim pArrivalLeg As ArrivalLeg
        Dim pSegmentLeg As SegmentLeg
        Dim pStartPoint As TerminalSegmentPoint
        Dim pDistanceIndication As DistanceIndication
        Dim pAngleIndication As AngleIndication

        Dim pSegmentPoint As SegmentPoint
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical
        Dim pSpeed As ValSpeed

        Dim mUomSpeed As UomSpeed
        Dim mUomHDistance As UomDistance
        Dim mUomVDistance As UomDistanceVertical

        Dim pLocation As Aran.Geometries.Point

        Dim uomSpeedTab() As UomSpeed
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomDistHorzTab() As UomDistance
        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData
        Dim SttIntesectNav As NavaidData
        Dim EndIntesectNav As NavaidData
        Dim ptStart As ESRI.ArcGIS.Geometry.IPoint
        Dim ptEnd As ESRI.ArcGIS.Geometry.IPoint

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        GuidNav = LegList(Index).GuidanceNav
        SttIntesectNav = LegList(Index).IntersectNav
        ptStart = LegList(Index).pPtPrj
        N = UBound(LegList)

        If Index < N - 1 Then
            EndIntesectNav = LegList(Index + 1).IntersectNav
            ptEnd = LegList(Index + 1).pPtPrj
        End If

        pArrivalLeg = pObjectDir.CreateFeature(Of ArrivalLeg)()
        'pArrivalLeg.Arrival = pProcedure.GetFeatureRef()
        pArrivalLeg.AircraftCategory.Add(IsLimitedTo)

        pSegmentLeg = pArrivalLeg

        '    pSegmentLeg.AltitudeOverrideATC =
        '    pSegmentLeg.AltitudeOverrideReference =
        '    pDuration = New Duration()
        '    pDuration.Uom = UomDuration_MIN
        '    pDuration.Value = CDbl(TextBox0602.Text)
        '    pSegmentLeg.Duration = pDuration
        '    pSegmentLeg.Note
        '    pSegmentLeg.ProcedureTurnRequired =
        '    pSegmentLeg.ReqNavPerformance
        '    pSegmentLeg.SpeedInterpretation =

        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL

        If LegList(Index).TurnDir_A = 1 Then
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
        ElseIf StepDownFIXs(Index).TurnDir_A = -1 Then
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
        Else
            pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        End If

        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        '    pSegmentLeg.EndConditionDesignator =
        '        pSegmentLeg.CourseDirection =
        '        pSegmentLeg.ProcedureTransition
        '        pSegmentLeg.StartPoint
        'pTransition.Type = ProcedurePhaseType_APPROACH

        '=======================================================================================
        If GuidNav.TypeCode = eNavaidType.DME Then
            Dim pArcCentre As TerminalSegmentPoint

            pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.ARC
            pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.AF

            If SideDef(ptStart, LegList(Index).OutDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW
            Else
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CCW
            End If

            pArcCentre = New TerminalSegmentPoint()
            pArcCentre.Waypoint = False
            pArcCentre.PointChoice = GuidNav.GetSignificantPoint
            pSegmentLeg.ArcCentre = pArcCentre

            pSegmentLeg.Course = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart))
        Else
            fCourseDir = LegList(Index).OutDir

            pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
            pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
            If SideDef(ptStart, fCourseDir + 90.0, GuidNav.pPtPrj) < 0 Then
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
            Else
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
            End If

            pSegmentLeg.Course = Dir2Azt(ptStart, fCourseDir)
        End If

        '=======================================================================================
        pSegmentLeg.BankAngle = arInitApprBank
        '=======================================================================================

        If Index < N - 1 Then
            pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptEnd.Z, eRoundMode.NEAREST), mUomVDistance)
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical

            'If Not Aran.Aim.Validation.DataTypeValidator.ValDistanceVerticalType(pSegmentLeg.LowerLimitAltitude) Then
            '	MsgBox("p1")
            'End If
        End If

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptStart.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical

        'If Not Aran.Aim.Validation.DataTypeValidator.ValDistanceVerticalType(pSegmentLeg.UpperLimitAltitude) Then
        '	MsgBox("p2")
        'End If

        '=======================================================================================
        pDistance = New ValDistance(ConvertDistance(LegList(Index).Length, eRoundMode.NEAREST), mUomHDistance)
        pSegmentLeg.Length = pDistance
        '======
        pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(LegList(Index).PDG))
        '======

        pSpeed = New ValSpeed(ConvertSpeed(cViafMax.Values(Category), eRoundMode.NEAREST), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed

        ' StarPoint ========================
        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        If Index > 0 Then
            pStartPoint = pEndPoint
        Else
            pStartPoint = New TerminalSegmentPoint()
            pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.OTHER_WPT

            pSegmentPoint = pStartPoint
            'pSegmentPoint.FlyOver = False
            pSegmentPoint.RadarGuidance = False
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
            pSegmentPoint.Waypoint = False

            ' ========================
            pInterNavSignPt = SttIntesectNav.GetSignificantPoint()

            ' Start Point ==================================================
            pPointReference = New PointReference()

            If GuidNav.TypeCode = eNavaidType.DME Then
                fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptStart) +
                 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW))    'Azt2DirPrj(ptStart, pSegmentLeg.Course)
            End If

            If ReturnDistanceInMeters(ptStart, SttIntesectNav.pPtPrj) < distEps Then
                SttIntesectNav.IntersectionType = eIntersectionType.OnNavaid
            End If

            If SttIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                Dim HorAccuracy As Double = 0.0
                If (GuidNav.TypeCode <> eNavaidType.DME) And (SttIntesectNav.Identifier <> Guid.Empty) Then
                    HorAccuracy = CalcHorisontalAccuracy(ptStart, GuidNav, SttIntesectNav)
                End If

                pFixDesignatedPoint = CreateDesignatedPoint(ptStart, , fCourseDir)
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

                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                    pAngleIndication.TrueAngle = Dir2Azt(ptStart, fDir)
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                    pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pAngleUse = New AngleUse()
                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = True

                    pPointReference.FacilityAngle.Add(pAngleUse)
                End If

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
                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
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
            Dim pTolerArea As ESRI.ArcGIS.Geometry.IPolygon
            pTolerArea = LegList(Index).pFixPoly

            If Not pTolerArea Is Nothing Then
                PriorPostFixTolerance(pTolerArea, ptStart, fCourseDir, PriorFixTolerance, PostFixTolerance)

                pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                pPointReference.PriorFixTolerance = pDistanceSigned

                pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                pDistanceSigned.Uom = mUomHDistance
                pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                pPointReference.PostFixTolerance = pDistanceSigned
                pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTolerArea))
            End If
            '=================================================================
            pStartPoint.FacilityMakeup.Add(pPointReference)
            pSegmentPoint.PointChoice = pFIXSignPt
        End If

        pSegmentLeg.StartPoint = pStartPoint
        ' End Of Start Point ========================

        ' EndPoint ========================
        If Index < N - 1 Then
            pEndPoint = New TerminalSegmentPoint
            pSegmentPoint = pEndPoint

            If LegList(Index).Tag <> 0 Then
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP '??????????????????????
                'pSegmentPoint.FlyOver = True
            ElseIf LegList(Index).TurnDir_A = 0 Then
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.SDF
                'pSegmentPoint.FlyOver = True
            Else
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP
                'pSegmentPoint.FlyOver = False
            End If

            pSegmentPoint.RadarGuidance = False
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
            pSegmentPoint.Waypoint = False
            '        pEndPoint.IndicatorFACF =      ??????????
            '        pEndPoint.LeadDME =            ??????????
            '        pEndPoint.LeadRadial =         ??????????
            If GuidNav.TypeCode = eNavaidType.DME Then
                fCourseDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd) +
                 90 * (1 + 2 * CLng(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW))     'Azt2DirPrj(ptStart, pSegmentLeg.Course)
            End If

            pPointReference = New PointReference()
            ' Indication ======================================================================

            Dim bOnNav As Boolean
            bOnNav = False

            If EndIntesectNav.Identifier <> Guid.Empty Then
                pInterNavSignPt = EndIntesectNav.GetSignificantPoint()

                If ReturnDistanceInMeters(ptEnd, EndIntesectNav.pPtPrj) < distEps Then
                    EndIntesectNav.IntersectionType = eIntersectionType.OnNavaid
                End If

                If EndIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
                    bOnNav = True
                    pFIXSignPt = pInterNavSignPt
                End If
            End If

            If Not bOnNav Then
                Dim HorAccuracy As Double = 0.0
                If (GuidNav.TypeCode <> eNavaidType.DME) And (EndIntesectNav.Identifier <> Guid.Empty) Then
                    HorAccuracy = CalcHorisontalAccuracy(ptEnd, GuidNav, EndIntesectNav)
                End If

                pFixDesignatedPoint = CreateDesignatedPoint(ptEnd, , fCourseDir)
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

                    pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
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
                        pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
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
                Dim pEndTolerArea As ESRI.ArcGIS.Geometry.IPolygon
                pEndTolerArea = LegList(Index + 1).pFixPoly

                If Not pEndTolerArea Is Nothing Then
                    PriorPostFixTolerance(pEndTolerArea, ptEnd, fCourseDir, PriorFixTolerance, PostFixTolerance)

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                    pDistanceSigned.Uom = mUomHDistance
                    pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                    pPointReference.PriorFixTolerance = pDistanceSigned

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
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
            pSegmentPoint.PointChoice = pFIXSignPt
        Else
            pEndPoint = pIFPoint
        End If

        pSegmentLeg.EndPoint = pEndPoint

        ' End of EndPoint ========================

        ' Trajectory ===========================================
        Dim J As Integer
        Dim I As Integer
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

        pCurve = New Curve
        pPolyline = LegList(Index).pNominalPoly

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

        Return pArrivalLeg
        ' END ====================================================
    End Function

    Private Function SaveProcedure() As Boolean
        Dim I As Integer
        Dim N As Integer
        Dim NO_SEQ As Integer

        Dim pTransition As ProcedureTransition
        Dim pSegmentLeg As SegmentLeg
        Dim ptl As ProcedureTransitionLeg

        Dim pIAP As InstrumentApproachProcedure
        Dim pSTAR As StandardInstrumentArrival


        Dim pEndPoint As TerminalSegmentPoint

        pObjectDir.ClearAllFeatures()
        pIAP = IAPArray(ComboBox002.SelectedIndex)

        If bFirstPointIsIF Then
            pProcedure = New InstrumentApproachProcedure()
            pProcedure.Assign(pIAP)
        Else
            Dim Ident As Guid
            Dim tls As TimeSlice
            Dim sProcName As String

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

            If LegList(LegList.Length - 1).Name Is Nothing Or LegList(LegList.Length - 1).Name.Length = 0 Then
                sProcName = "PSTAR"
            ElseIf LegList(LegList.Length - 1).Name.Length > 5 Then
                sProcName = LegList(LegList.Length - 1).Name.Substring(0, 5)
            Else
                sProcName = LegList(LegList.Length - 1).Name
            End If

            pProcedure.Name = sProcName + " 1A"

            'pProcedure.SafeAltitude.

            pSTAR.Designator = sProcName + "1A"

            pSTAR.Arrival = pIAP.Landing
            pSTAR.CommunicationFailureInstruction = ""

            'added by agsin
            'for link runway to arrival

            'Dim count As Integer = ChkRwyDirections.Items.Count

            'If (ChkRwyDirections.CheckedItems.Count > 0) Then
            '	pSTAR.Arrival = New LandingTakeoffAreaCollection()
            '	For index As Integer = 0 To count - 1
            '		If (ChkRwyDirections.GetItemChecked(index)) Then
            '			pSTAR.Arrival.Runway.Add(New Objects.FeatureRefObject(rwyDirList(index).Identifier))
            '		End If
            '	Next
            'End If
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

        pEndPoint = Nothing
        NO_SEQ = 0

        For I = 0 To N - 1
            NO_SEQ = NO_SEQ + 1
            If bFirstPointIsIF Then
                pSegmentLeg = InitialApproachLeg(I, pProcedure, pProcedure.AircraftCharacteristic(0), pEndPoint)
            Else
                pSegmentLeg = CreateArrivalLeg(I, pProcedure, pProcedure.AircraftCharacteristic(0), pEndPoint)
            End If

            ptl = New ProcedureTransitionLeg()
            ptl.SeqNumberARINC = NO_SEQ
            ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()

            pTransition.TransitionLeg.Add(ptl)
        Next I

        pTransition.DepartureRunwayTransition = pLandingTakeoff
        '    pTransition.TransitionId = TextBox0???.Text

        pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.APPROACH ' ProcedurePhaseType_FINAL

        ' Procedure =================================================================================================

        'pProcedure.TimeSlice.CorrectionNumber = pProcedure.TimeSlice.CorrectionNumber + 1
        'pProcedure.TimeSlice.ValidTime.BeginPosition = DateTime.Now
        'pProcedure.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA
        'If pProcedure.TimeSlice.SequenceNumber = 1 Then
        '	pProcedure.TimeSlice.FeatureLifetime.BeginPosition = pProcedure.TimeSlice.ValidTime.BeginPosition
        'End If

        pProcedure.FlightTransition.Add(pTransition)
        ' Store =================================================================================================

        Try
            If bFirstPointIsIF Then
                pObjectDir.SetFeature(pProcedure)
                pObjectDir.SetRootFeatureType(FeatureType.InstrumentApproachProcedure)
            Else
                pObjectDir.SetRootFeatureType(FeatureType.StandardInstrumentArrival)
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

            Dim metadataFeatures As List(Of FeatureType) = New List(Of FeatureType) From {FeatureType.InstrumentApproachProcedure}

            If gAranEnv.ConnectionInfo.ConnectionType = ConnectionType.TDB And gAranEnv.UseWebApi Then
                SaveProcedure = pObjectDir.CommitWithMetadataViewer(
                    gAranEnv.Graphics.ViewProjection.Name,
                    commitedFeatures.ToArray(),
                    metadataFeatures.ToArray(),
                    GetNumericalData(),
                    False)
            Else
                SaveProcedure = pObjectDir.Commit(commitedFeatures.ToArray())
            End If

            gAranEnv.RefreshAllAimLayers()
        Catch ex As Exception
            MsgBox("Error on commit." + vbCrLf + ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GetNumericalData() As List(Of GeoNumericalDataModel)
        Dim NumericalData As New List(Of GeoNumericalDataModel)

        Return NumericalData
    End Function

    Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
        Me.Close()
    End Sub

    Private Sub ReportBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReportBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ReportBtn.Checked Then
            InitialReportsFrm.Show(_Win32Window)
        Else
            InitialReportsFrm.Hide()
        End If
    End Sub

    Private Sub TextBox001_KeyPress(sender As System.Object, eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox001.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox001_Validating(TextBox001, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox001.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox001_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox001.Validating
        If TextBox001.ReadOnly Then Return

        If Not IsNumeric(TextBox001.Text) Then
            TextBox001.Text = CStr(StartPoint.pPtGeo.M)
            Return
        End If

        On Error Resume Next
        If Not pFAFptElement Is Nothing Then pGraphics.DeleteElement(pFAFptElement)
        If Not pFAFAreaElement Is Nothing Then pGraphics.DeleteElement(pFAFAreaElement)
        On Error GoTo 0

        Dim Course As Double
        Course = CDbl(TextBox001.Text)

        pIFptGeo.M = Course
        pIFptPrj.M = Azt2Dir(pIFptGeo, Course)

        StartPoint.pPtGeo.M = Course
        StartPoint.pPtPrj.M = pIFptPrj.M

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

    Private Sub TextBox002_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox002.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox002_Validating(TextBox002, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox002.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox002_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox002.Validating
        Dim fTmp As Double

        If Not IsNumeric(TextBox002.Text) Then Return
		fTmp = DeConvertHeight(CDbl(TextBox002.Text))

		If fTmp < System.Math.Round(pIFptGeo.Z) Then
            fTmp = System.Math.Round(pIFptGeo.Z)
            TextBox002.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
        End If
        pIFptPrj.Z = fTmp


        '    On Error Resume Next
        '        If Not StepDownFIXs(1).pFixPolyElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(1).pFixPolyElem
        '        If Not StepDownFIXs(1).ptElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(1).ptElem
        '    On Error GoTo 0

        'GetNavData TextBox006.Text, Label013.Caption, InterNav
        '    Set pGuidPoly = CreateGuidPoly(StepDownFIXs(0).GuidNav, pFAFptPrj)

        'fTmp = ReturnDistanceInMeters(InterNav.pPtPrj, StepDownFIXs(0).GuidNav.pPtPrj)
        'fDf = ReturnDistanceInMeters(InterNav.pPtPrj, pIFptPrj)
        'bOnNav = (fTmp < distEps) And (fDf < distEps) And (InterNav.TypeCode = StepDownFIXs(1).GuidNav.TypeCode)

        'Set pFixPoly = CreateFixZone(pIFptPrj, StepDownFIXs(0).GuidNav, InterNav, bOnNav, pFAFptPrj.M, fTmp)

        'Set StepDownFIXs(1).pFixPoly = pFixPoly
        'Set StepDownFIXs(1).pFixPolyElem = DrawPolygon(pFixPoly, 255)
        'Set StepDownFIXs(1).ptElem = DrawPoint(pIFptPrj, RGB(255, 0, 255))

        'StepDownFIXs(1).pFixPolyElem.Locked = True
        'StepDownFIXs(1).ptElem.Locked = True
    End Sub

    Private Sub TextBox101_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox101.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox101.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox101_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox101.Validating
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim TurnFlg As Integer

        Dim fTmp As Double
        Dim fDist As Double
        Dim fIADir As Double
        Dim fIADist As Double
        Dim fIAAngle As Double

        Dim IAFNav As NavaidData
        Dim OldNav As NavaidData
        Dim ForFIX As StepDownFIX

        If Not IsNumeric(TextBox101.Text) Then Return
        fDist = DeConvertDistance(CDbl(TextBox101.Text))
        If fDist < 0.0 Then Return

        ForFIX = StepDownFIXs(StepDownsNum - 1)
        IAFNav = ForFIX.GuidanceNav
        fIADist = ReturnDistanceInMeters(ForFIX.pPtPrj, IAFNav.pPtPrj)

        If IAFNav.TypeCode = eNavaidType.DME Then 'DME
            fIADir = ReturnAngleInDegrees(IAFNav.pPtPrj, ForFIX.pPtPrj)
            fTmp = Modulus(ForFIX.InDir - fIADir - 90.0, 360.0)
            If fTmp > 180.0 Then fTmp = 360.0 - fTmp
            TurnFlg = 2 * CShort(fTmp < 25.0) + 1

            fIAAngle = RadToDeg(fDist / fIADist)
            fTmp = fIADir + fIAAngle * TurnFlg
            ptKT1 = PointAlongPlane(IAFNav.pPtPrj, fTmp, fIADist)

            fTmp = ReturnAngleInDegrees(IAFNav.pPtPrj, ptKT1)
            StepDownFIXs(StepDownsNum).OutDir = Modulus(fTmp - 90.0 * TurnFlg, 360.0)
            StepDownFIXs(StepDownsNum).ArcDir = TurnFlg
            '==========================================================================================================
        Else 'VOR, NDB
            '        bNavCode = StepDownsNum >= 1
            '        If bNavCode Then bNavCode = IAFNav.TypeCode = CodeDME
            '        If Not bNavCode Then
            '        If StepDownsNum < 1 Then
            '            fIADir = ForFIX.InDir
            '        Else
            '       fIADir = ForFIX.InDir + 180.0

            '        TurnFlg = 2 * ComboBox102.ListIndex - 1
            '        NavLeftRightSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, IAFNav.pPtPrj)

            '        If TurnFlg = NavLeftRightSide Then fIADir = fIADir + 180.0

            fIADir = ForFIX.InDir 'ReturnAngleInDegrees(ForFIX.pPtPrj, IAFNav.pPtPrj)
            ptKT1 = PointAlongPlane(ForFIX.pPtPrj, fIADir + 180.0, fDist)
            StepDownFIXs(StepDownsNum).OutDir = ForFIX.InDir
            'DrawPointWithText(ptKT1, "ptKT1", RGB(0, 255, 255))

            StepDownFIXs(StepDownsNum).ArcDir = 0
        End If
        '==========================================================================================================

        'DrawPoint ptKT1, RGB(0, 255, 255)
        '    StepDownFIXs(StepDownsNum).OutDir = StepDownFIXs(StepDownsNum - 1).InDir
        '    StepDownFIXs(StepDownsNum).GuidNav = IAFNav
        On Error Resume Next
        If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
        If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)
        On Error GoTo 0
        '    If Not StepDownFIXs(StepDownsNum).pIAreaPolyElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(StepDownsNum).pIAreaPolyElem
        '    If Not StepDownFIXs(StepDownsNum).pIIAreaPolyElem Is Nothing Then pGraphics.DeleteElement StepDownFIXs(StepDownsNum).pIIAreaPolyElem

        StepDownFIXs(StepDownsNum).ptElem = DrawPoint(ptKT1, 0)
        StepDownFIXs(StepDownsNum).ptElem.Locked = True

        ' КТ снижения ================================================================================
        If OptionButton101.Checked Then
            StepDownFIXs(StepDownsNum).PDG = IAF_PDG 'arIADescent_Nom.Value

            TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + (fDist + StepDownFIXs(StepDownsNum - 1).TotalLength) * IAF_PDG 'fDist
            ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit)

            If TextBox110.Tag = "a" Then
                fFIXHeight = TextBox110Range.Right
                TextBox110.Tag = ""
            Else
                fFIXHeight = TextBox110Range.Left
            End If

            ptKT1.Z = fFIXHeight
            TextBox110.Text = CStr(ConvertHeight(fFIXHeight, eRoundMode.NEAREST))   '+ " " + HeightConverter(HeightUnit).Unit

            ''        ptKT1.Z = fhFix8 'StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + fDist * IAF_PDG 'StepDownFIXs(StepDownsNum - 1).PDG
            ''        dD = dDMin
            ''        N = UBound(IAFObstList4FIX)

            ''        For I = 0 To N
            ''            If IAFObstList4FIX(I).Dist >= fDist Then
            ''                J = I
            ''                Exit For
            ''            End If
            ''        Next I

            ''        For I = J To N
            ''            If IAFObstList4FIX(I).Dist <= fDist + dD Then
            ''                fTmp = IAFObstList4FIX(I).Dist - fDist + (fhFix8 - IAFObstList4FIX(I).ReqH) / arFixMaxIgnorGrd.Value
            ''                LdD = (arFixMaxIgnorGrd.Value * IAFObstList4FIX(I).Dist - IAFObstList4FIX(I).ReqH - (arFixMaxIgnorGrd.Value - IAF_PDG) * fDist) / arFixMaxIgnorGrd.Value
            ''                If fTmp < dD Then dD = fTmp
            ''            Else
            ''                Exit For
            ''            End If
            ''        Next I

            StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
            StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
            StepDownFIXs(StepDownsNum).pPtStart = ptKT1

            OldNav.Index = -1
            If ComboBox104.SelectedIndex > -1 Then OldNav = SDFInterNavs(ComboBox104.SelectedIndex)

            SDFInterNavs = GetSDFInterNavs(ptKT1, fDist)
            ComboBox104.Items.Clear()
            N = UBound(SDFInterNavs)

            If N >= 0 Then
                J = 0
                For I = 0 To N
                    ComboBox104.Items.Add(SDFInterNavs(I))  '+ "/" + SDFInterNavs(I).TypeName
                    If (OldNav.Index > -1) And (SDFInterNavs(I).Index = OldNav.Index) Then
                        J = I
                    End If
                Next I

                ComboBox104.SelectedIndex = J
            End If
        Else 'Разворот ================================================================================
            '        N = UBound(IAFObstList4Turn)
            '        For I = 0 To N
            '            If IAFObstList4Turn(I).Dist >= fDist Then
            '                J = I
            '                Exit For
            '            End If
            '        Next I

            '        For I = J To N
            '            If IAFObstList4Turn(I).Dist <= fDist + dD Then
            '                fTmp = IAFObstList4Turn(I).Dist - fDist + (fhFix8 - IAFObstList4Turn(I).ReqH) / arFixMaxIgnorGrd.Value
            '                LdD = (arFixMaxIgnorGrd.Value * IAFObstList4Turn(I).Dist - IAFObstList4Turn(I).ReqH - (arFixMaxIgnorGrd.Value - IAF_PDG) * fDist) / arFixMaxIgnorGrd.Value
            '                If fTmp < dD Then dD = fTmp
            '            Else
            '                Exit For
            '            End If
            '        Next I

            TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + (fDist + StepDownFIXs(StepDownsNum - 1).TotalLength) * IAF_PDG 'fDist
            ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit)

            fFIXHeight = TextBox110Range.Left
            ptKT1.Z = fFIXHeight 'StepDownFIXs(StepDownsNum - 1).pPtPrj.Z
            TextBox110.Text = CStr(ConvertHeight(fFIXHeight, eRoundMode.NEAREST))   '+ " " + HeightConverter(HeightUnit).Unit

            StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
            StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
            StepDownFIXs(StepDownsNum).pPtStart = ptKT1
            OldNav.Index = -1

            Dim IAFTurnNavs() As NavaidData

            If ComboBox101.SelectedIndex > -1 Then OldNav = IAFNavDat(ComboBox101.SelectedIndex)
            IAFTurnNavs = GetIAFTurnNavs(ptKT1)

            N = UBound(IAFTurnNavs)
            ReDim IAFNavDat(N)

            '    IAFNavDat = GetIAFNavs(IFPnt)
            ComboBox101.Items.Clear()

            J = -1
            K = 0

            For I = 0 To N
                fDist = ReturnDistanceInMeters(ptKT1, IAFTurnNavs(I).pPtPrj)
                If (IAFTurnNavs(I).Identifier <> StepDownFIXs(StepDownsNum - 1).GuidanceNav.Identifier) Or (fDist = 0.0) Then
                    J += 1
                    IAFNavDat(J) = IAFTurnNavs(I)
                    ComboBox101.Items.Add(IAFNavDat(J))

                    If (OldNav.Index > -1) And (IAFNavDat(J).Index = OldNav.Index) Then
                        K = J
                    End If
                End If
            Next I

            If J >= 0 Then
                ReDim Preserve IAFNavDat(J)
                ComboBox101.SelectedIndex = K
            Else
                ReDim IAFNavDat(J)
            End If
        End If
    End Sub

    Private Sub TextBox103_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox103.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox103_Validating(TextBox103, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox103.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox103_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox103.Validating
        Dim NewPDG As Double

        If Not IsNumeric(TextBox103.Text) Then Return
        NewPDG = 0.01 * CDbl(TextBox103.Text)
        If NewPDG < 0.0 Then Return

        If NewPDG > arIADescent_Max.Value Then
            NewPDG = arIADescent_Max.Value
            TextBox103.Text = CStr(100.0 * NewPDG)
        End If

        If NewPDG < 0.01 * PDGEps Then
            NewPDG = 0.01 * PDGEps
            TextBox103.Text = CStr(100.0 * NewPDG)
        End If

        IAF_PDG = NewPDG

        EstimateIAF_AreaObstacles()
        OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
    End Sub

    Private Sub TextBox105_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox105.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox105_Validating(TextBox105, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox105.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox105_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox105.Validating
        Dim fTmp As Double
        Dim fTmp1 As Double

        If Not IsNumeric(TextBox105.Text) Then Return

        fTmp = CDbl(TextBox105.Text)
        If fTmp < 0.0 Then Return

        If IsNumeric(TextBox105.Tag) Then
            fTmp1 = CDbl(TextBox105.Tag)
            If fTmp1 = fTmp Then Return
        End If

        TextBox105.Tag = TextBox105.Text

        EstimateIAF_AreaObstacles()
        If OptionButton101.Checked Then OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
    End Sub

    Private Sub TextBox108_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox108.Validating
        Dim fTmp As Double

        If Not IsNumeric(TextBox108.Text) Then Return
        fTmp = CDbl(TextBox108.Text)
        TextBox109.Text = CStr(System.Math.Round(Modulus(fTmp - StepDownFIXs(StepDownsNum - 1).GuidanceNav.MagVar, 360.0)))
        ComboBox102_SelectedIndexChanged(ComboBox102, New System.EventArgs())
    End Sub

    Private Function ConvertTracToPoints(ByRef TrackPoints() As ReportPoint) As Double
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer

        Dim pPt As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

        'Dim pPtX As ESRI.ArcGIS.Geometry.IPoint
        'Dim pPtGeoX As ESRI.ArcGIS.Geometry.IPoint

        Dim pPtPrev As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtNext As ESRI.ArcGIS.Geometry.IPoint

        Dim pPtC1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPtC2 As ESRI.ArcGIS.Geometry.IPointCollection

        'Dim ptConstr As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline

        Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry

        Dim PDG As Double
        Dim fTmp As Double
        'Dim fE As Double
        Dim pPoints(10) As ESRI.ArcGIS.Geometry.IPoint

        N = 0

        ReDim TrackPoints(10)
        pPt = New ESRI.ArcGIS.Geometry.Point
        pPtC1 = New ESRI.ArcGIS.Geometry.Polyline
        'pPtX = New ESRI.ArcGIS.Geometry.Point
        'pPtGeoX = New ESRI.ArcGIS.Geometry.Point
        'ptConstr = pPtX

        '\============================================================================
        'If _OptionButton0201_0.Value Then
        '    fTmp = CDbl(TextBox0201.Text)
        'IF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        pPt.PutCoords(pIFptPrj.X, pIFptPrj.Y)
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(0).Description = "IF"
        '    TrackPoints(0).Lat = MyDD2Str(pPt.Y, 3)
        '    TrackPoints(0).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(0).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(0).PDG = CDbl("")
        TrackPoints(0).Altitude = CDbl(CStr(System.Math.Round(pIFptPrj.Z)))

        TrackPoints(0).Radius = NO_DATA_VALUE
        TrackPoints(0).Turn = 0

        TrackPoints(0).CenterLat = ""
        TrackPoints(0).CenterLon = ""

        TrackPoints(0).TurnAngle = CDbl("")
        TrackPoints(0).TurnArcLen = CDbl("")
        pPoints(0) = pIFptPrj
        'FAF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    pPt.PutCoords PtFAF.X, PtFAF.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(1).Description = "FAF"
        'TrackPoints(1).Lat = MyDD2Str(pPt.Y, 3)
        'TrackPoints(1).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(1).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(1).PDG = CDbl("")
        '    TrackPoints(1).Height = CStr(Round(PtFAF.Z ))

        TrackPoints(1).Radius = NO_DATA_VALUE
        TrackPoints(1).Turn = 0
        TrackPoints(1).CenterLat = ""
        TrackPoints(1).CenterLon = ""

        TrackPoints(1).TurnAngle = CDbl("")
        TrackPoints(1).TurnArcLen = CDbl("")
        '    Set pPoints(1) = PtFAF
        N = 2
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Else
        '          IAF
        '    Set PtTOB = ptHoldingArea.Point(0)
        '    Set PtSOL = ptHoldingArea.Point(1)
        '    Set PtEOL = ptHoldingArea.Point(2)
        '    Set FarFAF = ptHoldingArea.Point(3)
        'IAF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    Set pPt = NavaidList(ComboBox0102.ListIndex).pt
        '    If OptionButton0601.Value Then
        '        pPt.M = PtTOB.M
        '        FillTurnParams PtTOB, PtSOL, TrackPoints(0)
        '        Set pPoints(0) = PtTOB
        '    Else
        '        pPt.M = PtSOL.M

        TrackPoints(0).Radius = NO_DATA_VALUE
        TrackPoints(0).Turn = 0
        TrackPoints(0).CenterLat = ""
        TrackPoints(0).CenterLon = ""

        TrackPoints(0).TurnAngle = CDbl("")
        TrackPoints(0).TurnArcLen = CDbl("")
        '        Set pPoints(0) = PtSOL
        '    End If

        fTmp = Dir2Azt(pFAFptPrj, pPt.M)

        TrackPoints(0).Description = "IAF"
        '    TrackPoints(0).Lat = MyDD2Str(pPt.Y, 3)
        '    TrackPoints(0).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(0).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(0).PDG = CDbl("")
        '    TrackPoints(0).Height = CStr(Round(DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight))

        N = 1
        'SOL++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    If OptionButton0601.Value Then
        '        pPt.PutCoords PtSOL.X, PtSOL.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        '        fTmp = Dir2Azt(pFAFptPrj, PtSOL.M)
        TrackPoints(1).Description = "SOL"
        '        TrackPoints(1).Lat = MyDD2Str(pPt.Y, 3)
        '        TrackPoints(1).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(1).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(1).PDG = CDbl("")
        TrackPoints(1).Altitude = TrackPoints(0).Altitude 'CStr(Round(CDbl(TextBox0601.Text) + fRefHeight))

        TrackPoints(1).Radius = NO_DATA_VALUE
        TrackPoints(1).Turn = 0
        TrackPoints(1).CenterLat = ""
        TrackPoints(1).CenterLon = ""
        TrackPoints(1).TurnAngle = CDbl("")
        TrackPoints(1).TurnArcLen = CDbl("")
        '        Set pPoints(1) = PtSOL
        N = 2
        '    End If

        'EOL++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    If OptionButton0601.Value Or OptionButton0602.Value Then
        '        pPt.PutCoords PtEOL.X, PtEOL.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)
        ''        fTmp = Dir2Azt(pFAFptPrj, PtEOL.M)

        TrackPoints(N).Description = "EOL"
        ''        TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        ''        TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        ''        TrackPoints(N).Height = CStr(Round(PtEOL.Z))
        ''        FillTurnParams PtEOL, FarFAF, TrackPoints(N)
        '
        ''        TrackPoints(N).Radius = NO_DATA_VALUE
        ''        TrackPoints(N).Turn = 0
        ''        TrackPoints(N).CenterLat = ""
        ''        TrackPoints(N).CenterLon = ""
        ''        TrackPoints(N).TurnAngle = ""
        ''        TrackPoints(N).TurnArcLen = ""
        ''        If OptionButton3.Value Then
        ''            TrackPoints(N).Radius = TrackPoints(0).Radius
        ''        Else
        ''            Set ptConstr = pPt
        ''            ptConstr.ConstructAngleIntersection PtEOL, DegToRad(PtEOL.M + 90.0), FarFAF, DegToRad(FarFAF.M + 90.0)
        ''            TrackPoints(N).Radius = ReturnDistanceInMeters(pPt, PtEOL)
        ''        End If
        '        Set pPoints(N) = PtEOL
        N = N + 1
        '    End If

        'FarFAF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    pPt.PutCoords FarFAF.X, FarFAF.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        '    fTmp = Dir2Azt(pFAFptPrj, FarFAF.M)

        TrackPoints(N).Description = "Start of Inbound Leg (SIL)"
        '    TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        '    TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        '    TrackPoints(N).Height = CStr(Round(FarFAF.Z))

        TrackPoints(N).Radius = NO_DATA_VALUE
        TrackPoints(N).Turn = 0
        TrackPoints(N).CenterLat = ""
        TrackPoints(N).CenterLon = ""
        TrackPoints(N).TurnAngle = CDbl("")
        TrackPoints(N).TurnArcLen = CDbl("")
        '    Set pPoints(N) = FarFAF
        N = N + 1
        'SDF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        '    If CheckBox0701.Value Then
        '        pPt.PutCoords PtFAF.X, PtFAF.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)
        '        fTmp = CDbl(TextBox0201.Text)

        TrackPoints(N).Description = "SDF"
        '        TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        '        TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        '        TrackPoints(N).Height = CStr(Round(PtFAF.Z ))

        TrackPoints(N).Radius = NO_DATA_VALUE
        TrackPoints(N).Turn = 0
        TrackPoints(N).CenterLat = ""
        TrackPoints(N).CenterLon = ""
        TrackPoints(N).TurnAngle = CDbl("")
        TrackPoints(N).TurnArcLen = CDbl("")
        '        Set pPoints(N) = PtFAF
        N = N + 1
        '    End If
        'End If

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'fTmp = Modulus(Dir2Azt(pFAFptPrj, CDbl(TextBox1.Text)), 360.0)
        'fTmp = CDbl(TextBox0201.Text)

        'MAPt++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'pPt.PutCoords MAPtPrj.X, MAPtPrj.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(N).Description = "MAPt"
        'TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        'TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        'TrackPoints(N).Height = CStr(Round(MAPtPrj.Z))

        TrackPoints(N).Radius = NO_DATA_VALUE
        TrackPoints(N).Turn = 0
        TrackPoints(N).CenterLat = ""
        TrackPoints(N).CenterLon = ""
        TrackPoints(N).TurnAngle = CDbl("")
        TrackPoints(N).TurnArcLen = CDbl("")
        'Set pPoints(N) = MAPtPrj
        N = N + 1

        'SOC++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'pPt.PutCoords PtSOC.X, PtSOC.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(N).Description = "SOC"
        'TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        'TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        'TrackPoints(N).Height = CStr(Round(PtSOC.Z))

        TrackPoints(N).Radius = NO_DATA_VALUE
        TrackPoints(N).Turn = 0
        TrackPoints(N).CenterLat = ""
        TrackPoints(N).CenterLon = ""
        TrackPoints(N).TurnAngle = CDbl("")
        TrackPoints(N).TurnArcLen = CDbl("")
        'Set pPoints(N) = PtSOC

        ReDim Preserve TrackPoints(N)

        ConvertTracToPoints = 0.0

        For I = 0 To N - 1
            If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
            pPtC1.AddPoint(pPoints(I))
            pPtC1.AddPoint(pPoints(I + 1))

            pPolyline = pPtC2
            TrackPoints(I).ToNext = pPolyline.Length
            ConvertTracToPoints = ConvertTracToPoints + TrackPoints(I).ToNext
        Next I

        'If CheckBox0801.Value Then
        N = N + 1
        '    M = MPtCollection.PointCount + 1
        ReDim Preserve TrackPoints(N + M)

        'TP++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
        pPtC1.AddPoint(pPoints(N - 1))
        '    pPtC1.AddPoint TurnFixPnt

        pPolyline = pPtC2
        TrackPoints(N - 1).ToNext = pPolyline.Length

        '    pPt.PutCoords TurnFixPnt.X, TurnFixPnt.Y
        pGeometry = pPt
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(N).Description = "TP"
        '    TrackPoints(N).Lat = MyDD2Str(pPt.Y, 3)
        '    TrackPoints(N).Lon = MyDD2Str(pPt.X, 4)
        TrackPoints(N).Direction = CStr(System.Math.Round(fTmp, 2))
        TrackPoints(N).PDG = CDbl("")
        '    TrackPoints(N).Height = CStr(Round(TurnFixPnt.Z))

        TrackPoints(N).Radius = NO_DATA_VALUE
        TrackPoints(N).Turn = 0
        TrackPoints(N).CenterLat = ""
        TrackPoints(N).CenterLon = ""
        TrackPoints(N).TurnAngle = CDbl("")
        TrackPoints(N).TurnArcLen = CDbl("")

        J = N + 1
        '\============================================================================
        '    Pdg = CDbl(TextBox1501.Text) * 0.01

        '    TrackPoints(0).Description = "Начальная точка процедуры вылета"
        '    TrackPoints(0).Lat = MyDD2Str(PtDer.Y, 3)
        '    TrackPoints(0).Lon = MyDD2Str(PtDer.X, 4)
        '    TrackPoints(0).Direction = CStr(Round(Modulus(Dir2Azt(ptDerPrj, AztDir), 360.0), 2))
        '    TrackPoints(0).Height = CStr(Round(dpH_abv_DER.Value + ptDerPrj.Z))
        '    TrackPoints(0).Radius = NO_DATA_VALUE
        '    Set pPtNext = TurnFixPnt
        pPtNext.M = Azt2Dir(pPt, fTmp)

        For J = N + 1 To M + N
            I = J - N
            TrackPoints(J).Radius = NO_DATA_VALUE
            '=========================================================================
            If (I < M) Then
                '            Set pPtPrj = MPtCollection.Point(i - 1)
                '            ToGeographic pPtPrj, pPt
                pPt.PutCoords(pPtPrj.X, pPtPrj.Y)
                pGeometry = pPt
                pGeometry.SpatialReference = pSpRefPrj
                pGeometry.Project(pSpRefGeo)
                If (I And 1) = 1 Then
                    '                FillTurnParams MPtCollection.Point(i - 1), MPtCollection.Point(i), TrackPoints(J)
                    TrackPoints(J).Description = My.Resources.str00521 + CStr((I + 1) \ 2) + My.Resources.str00523
                Else
                    TrackPoints(J).Description = My.Resources.str00522 + CStr((I + 1) \ 2) + My.Resources.str00523
                    TrackPoints(J).Direction = CStr(System.Math.Round(Dir2Azt(pFAFptPrj, pPtPrj.M), 2))
                End If
            Else
                '=========================================================================
                pPtPrj.M = pPtNext.M
                'pPtGeoX.PutCoords(pPtPrj.X, pPtPrj.Y)
                pGeometry = pPt
                pGeometry.SpatialReference = pSpRefPrj
                pGeometry.Project(pSpRefGeo)
                TrackPoints(J).Description = My.Resources.str00524  '"Конечная точка процедуры вылета"
                '            TrackPoints(J).Direction = TrackPoints(J - 1).Direction
            End If
            '=========================================================================

            pPtPrev = pPtNext
            pPtNext = pPtPrj

            pPtC1 = New ESRI.ArcGIS.Geometry.Polyline
            pPtC1.AddPoint(pPtPrev)
            pPtC1.AddPoint(pPtNext)

            pPolyline = pPtC2

            TrackPoints(J - 1).ToNext = pPolyline.Length
            TrackPoints(J).Altitude = TrackPoints(J - 1).Altitude + pPolyline.Length * PDG

            '        TrackPoints(J).Lat = MyDD2Str(pPt.Y, 3)
            '        TrackPoints(J).Lon = MyDD2Str(pPt.X, 4)
            ConvertTracToPoints = ConvertTracToPoints + TrackPoints(J - 1).ToNext
        Next J

        '    For I = 0 To N - 1
        '        ConvertTracToPoints = ConvertTracToPoints + TrackPoints(I).ToNext
        '    Next I
        '\============================================================================
        'End If

    End Function

    Private Sub SaveGeometry(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim I As Integer
        Dim TraceLen As Double
        Dim GeomRep As New ReportFile
        Dim TrackPoints() As ReportPoint

        TraceLen = ConvertTracToPoints(TrackPoints)

        'GeomRep.ThrPtPrj = pFAFptPrj
        'GeomRep.RefHeight = fRefHeight

        GeomRep.OpenFile(RepFileName + "_Geometry", My.Resources.str00811)

        GeomRep.WriteMessage(My.Resources.str00003 + " - " + My.Resources.str00811)
        'GeomRep.WriteMessage()
        'GeomRep.WriteMessage(RepFileTitle)
        'GeomRep.WriteParam(My.Resources.str00813, CStr(Today) + " - " + CStr(TimeOfDay))
        GeomRep.WriteHeader(pReport)

        GeomRep.WriteMessage()
        GeomRep.WriteMessage()

        For I = 0 To UBound(TrackPoints)
            GeomRep.WritePoint(TrackPoints(I))
        Next I
        GeomRep.WriteMessage()

        GeomRep.Param(My.Resources.str00852, CStr(ConvertDistance(TraceLen, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit) '+ " / " + CStr(Round(TraceLen * NMCoeff, 1)), My.Resources.str0019 + "/" + My.Resources.str0020

        GeomRep.CloseFile()
        GeomRep = Nothing
    End Sub

    Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim ProtRep As New ReportFile
        Dim I As Integer

        'ProtRep.ThrPtPrj = pFAFptPrj
        'ProtRep.RefHeight = fRefHeight
        ProtRep.OpenFile(RepFileName + "_Protocol", My.Resources.str00815)

        ProtRep.WriteMessage(My.Resources.str00003 + " - " + My.Resources.str00815)
        'ProtRep.WriteMessage()
        'ProtRep.WriteMessage(RepFileTitle)
        ProtRep.WriteHeader(pReport, True)
        '    ProtRep.WriteParam My.Resources.str00813), CStr(Date) + " - " + CStr(Time)

        ProtRep.WriteMessage()
        ProtRep.WriteMessage()

        ProtRep.WriteMessage("[ " + InitialReportsFrm.MultiPage1.TabPages.Item(1).Text + " ]") '
        ProtRep.WriteMessage()

        ProtRep.lListView = InitialReportsFrm.ListView1

        For I = 0 To InitialReportsFrm.SegmUbnd
            If StepDownFIXs(I + 2).TurnDir = 0 Then
                ProtRep.Param((Label115.Text), CStr(ConvertHeight(StepDownFIXs(I + 2).pPtPrj.Z, eRoundMode.NEAREST)), HeightConverter(HeightUnit).Unit)
                ProtRep.WriteMessage()
            End If
            '        ProtRep.CurrData = InitialReportsFrm.SegmentData(I)
            '        ProtRep.WriteTabData SegmentData(I).FIXObstacles, SegmentData(I).FIXIx
            ProtRep.WriteObstData(SegmentData(I).FIXObstacles, SegmentData(I).FIXIx)
            ProtRep.WriteMessage()
        Next I

        ProtRep.CloseFile()
        ProtRep = Nothing
    End Sub

    Private Sub SaveLog(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim LogRep As New ReportFile
        Dim I As Integer
        Dim D As Double
        Dim A As Double

        LogRep.OpenFile(RepFileName + "_Log", My.Resources.str00817)

        LogRep.WriteMessage(My.Resources.str00003 + " - " + My.Resources.str00817)
        LogRep.WriteHeader(pReport)

        LogRep.WriteMessage()
        LogRep.WriteMessage()

        '===========================================================================
        LogRep.WriteMessage("[ " + MultiPage1.TabPages.Item(0).Text + " ]")
        LogRep.WriteMessage()

        LogRep.Param(Label001.Text, ProcedureName)
        LogRep.Param(Label002.Text, TextBox005.Text)
        LogRep.Param(Label006.Text, TextBox001.Text, Label007.Text)
        LogRep.Param(Label008.Text, TextBox002.Text, Label009.Text)
        LogRep.WriteMessage()

        LogRep.Param(Label003.Text, ComboBox004.Text)

        LogRep.Param(Label010.Text, TextBox003.Text, Label011.Text)
        LogRep.Param(Label004.Text, TextBox004.Text, Label005.Text)
        LogRep.Param(Label012.Text, TextBox006.Text, Label013.Text)
        LogRep.WriteMessage()
        LogRep.WriteMessage()
        ''===========================================================================
        LogRep.WriteMessage("[ " + MultiPage1.TabPages.Item(1).Text + " ]")
        LogRep.WriteMessage()

        For I = 2 To StepDownsNum - 2
            LogRep.WriteMessage("Segment " + CStr(I - 1))
            LogRep.WriteMessage()
            D = ReturnDistanceInMeters(StepDownFIXs(I - 1).pPtPrj, StepDownFIXs(I).pPtPrj)
            If StepDownFIXs(I).TurnDir = 0 Then
                LogRep.WriteMessage((OptionButton101.Text))
                LogRep.Param(My.Resources.str40221, CStr(System.Math.Round(D)), (Label102.Text))
                LogRep.Param(Label105.Text, StepDownFIXs(I).GuidanceNav.CallSign, GetNavTypeName(StepDownFIXs(I).GuidanceNav.TypeCode))
                LogRep.Param(Label110.Text, StepDownFIXs(I).IntersectNav.CallSign, GetNavTypeName(StepDownFIXs(I).IntersectNav.TypeCode))
                LogRep.Param(Label113.Text, CStr(100.0 * StepDownFIXs(I).PDG), (Label114.Text))
                LogRep.Param(Label115.Text, CStr(ConvertHeight(StepDownFIXs(I + 2).pPtPrj.Z, eRoundMode.NEAREST)), HeightConverter(HeightUnit).Unit)
            Else
                LogRep.WriteMessage((OptionButton102.Text))
                LogRep.Param(My.Resources.str40221, CStr(System.Math.Round(D)), (Label102.Text))
                LogRep.Param((Label105.Text), StepDownFIXs(I).GuidanceNav.CallSign, GetNavTypeName(StepDownFIXs(I).GuidanceNav.TypeCode))
                A = StepDownFIXs(I).InDir - StepDownFIXs(I).OutDir

                If A < 0 Then
                    LogRep.Param((Label107.Text), My.Resources.str40218) '"На лево"
                ElseIf A > 0 Then
                    LogRep.Param((Label107.Text), My.Resources.str40219) '"На право"
                Else
                    LogRep.Param((Label107.Text), "")
                End If

                LogRep.Param((Label108.Text), CStr(System.Math.Abs(System.Math.Round(A))), (Label109.Text))
            End If
        Next I

        '===========================================================================
        LogRep.CloseFile()
        LogRep = Nothing
    End Sub

#If False Then

	Private Sub TextBox102_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox102.KeyPress
		Dim KeyAscii As Short = Asc(e.KeyChar)
		If KeyAscii = 13 Then
			TextBox102_Validating(TextBox103, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(KeyAscii, TextBox102.Text)
		End If

		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then e.Handled = True
	End Sub

	Private Sub TextBox102_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox102.Validating
		Dim K As Long
		Dim fDirl As Double
		Dim fIADist As Double
		Dim fIADir As Double
		Dim fTmp As Double
		Dim fIAAngle As Double
		Dim svGuyrug As Double

		Dim ForFIX As StepDownFIX
		Dim GuidNav As NavaidType
		Dim InterNav As NavaidType

		K = ComboBox104.SelectedIndex
		InterNav = SDFInterNavs(K)
		If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
			If Not IsNumeric(TextBox102.Text) Then Return
			If TextBox102.Tag = TextBox102.Text Then Return
			fDirl = CDbl(TextBox102.Text)
		End If

		ForFIX = StepDownFIXs(StepDownsNum - 1)
		GuidNav = ForFIX.GuidanceNav

		On Error Resume Next
		If Not StepDownFIXs(StepDownsNum).ptElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).ptElem)
		If Not StepDownFIXs(StepDownsNum).pFixPolyElem Is Nothing Then pGraphics.DeleteElement(StepDownFIXs(StepDownsNum).pFixPolyElem)
		On Error GoTo 0

		Dim pFixPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim PtSDF As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		If InterNav.IntersectionType = eIntersectionType.OnNavaid Then

		ElseIf InterNav.IntersectionType = eIntersectionType.ByDistance Then

			CircleVectorIntersect(InterNav.pPtPrj, 2.0 * DME.MinimalError, StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + 180.0, ptTmp)
		Else

		End If

		Dim Intersect As NavaidType
		Intersect = InterNav

		pFixPoly = CreateFixZone(PtSDF, GuidNav, Intersect, Intersect.IntersectionType = eIntersectionType.OnNavaid, fIADir, svGuyrug)


		'If GuidNav.TypeCode = eNavaidType.DME Then
		'	dD = 0.0
		'	pBaseLine.FromPoint = GuidNav.pPtPrj
		'	pBaseLine.ToPoint = PtSDF
		'	fDir = pBaseLine.Angle

		'	For I = 0 To pFixPoly.PointCount - 1
		'		pBaseLine.ToPoint = pFixPoly.Point(I)
		'		fTmp = Modulus((pBaseLine.Angle - fDir) * (2 * ComboBox102.SelectedIndex - 1), 2 * PI)
		'		If fTmp > PI Then fTmp = fTmp - 2 * PI
		'		If fTmp > dD Then dD = fTmp
		'	Next I
		'	dD = dD * fDist
		'End If

		'TextBox104.Text = CStr(ConvertDistance(dD, eRoundMode.rmNERAEST))

		StepDownFIXs(StepDownsNum).pFixPoly = pFixPoly

		StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(pFixPoly, RGB(128, 255, 128))
		StepDownFIXs(StepDownsNum).ptElem = DrawPoint(PtSDF, 0)

		StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
		StepDownFIXs(StepDownsNum).ptElem.Locked = True
		'//// le_hex_equiv="33333333F387C3C0"


		Dim pBaseLine As ESRI.ArcGIS.Geometry.Line
		Dim pClone As ArcGIS.esriSystem.IClone
		'Dim bDirChanged As Boolean

		pBaseLine = New ESRI.ArcGIS.Geometry.Line

		If InterNav.IntersectionType = eIntersectionType.OnNavaid Then
		Else
			PtSDF = ptKT1
			If InterNav.IntersectionType = eIntersectionType.ByDistance Then

				Label127.Text = DistanceConverter(DistanceUnit).Unit

				pBaseLine.FromPoint = InterNav.pPtPrj
				pBaseLine.ToPoint = PtSDF
				fDir = RadToDeg(pBaseLine.Angle)
				fDist = pBaseLine.Length
				If fDist < 2.0 * DME.MinimalError Then
					CircleVectorIntersect(InterNav.pPtPrj, 2.0 * DME.MinimalError, StepDownFIXs(StepDownsNum - 1).pPtPrj, StepDownFIXs(StepDownsNum - 1).InDir + 180.0, ptTmp)
					If Not ptTmp.IsEmpty() Then
						PtSDF = ptTmp
						bDirChanged = True
						fIF_SDFDist = ReturnDistanceInMeters(StepDownFIXs(StepDownsNum - 1).pPtPrj, ptTmp)
					Else
						MessageBox.Show(My.Resources.str40220)
						Return
					End If
				End If
			End If
		End If

		hFix = fFIXHeight

		If bDirChanged Then
			TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + (fIF_SDFDist + StepDownFIXs(StepDownsNum - 1).TotalLength) * IAF_PDG 'fIF_SDFDist
			ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.rmNERAEST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.rmNERAEST)) + " " + HeightConverter(HeightUnit).Unit)
			'        TextBox110.Text = CStr(ConvertHeight(ptKT1.Z, eRoundMode.rmNERAEST))   '+ " " + HeightConverter(HeightUnit).Unit
			If fFIXHeight > TextBox110Range.Right Then
				fFIXHeight = TextBox110Range.Right
				hFix = fFIXHeight
				TextBox110.Text = CStr(ConvertHeight(hFix, eRoundMode.rmNERAEST))
			End If

			PtSDF.Z = hFix
			ptKT1 = PtSDF
			'       TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + fIF_SDFDist * StepDownFIXs(StepDownsNum - 1).PDG

			TextBox101.Text = CStr(ConvertDistance(fIF_SDFDist, eRoundMode.rmNERAEST))

			StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
			StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
			StepDownFIXs(StepDownsNum).pPtStart = ptKT1
		End If

		'   GuidNav = IAFNavDat(ComboBox101.ListIndex)
		pBaseLine.ToPoint = PtSDF
		GuidNav = StepDownFIXs(StepDownsNum - 1).GuidanceNav

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		If GuidNav.TypeCode = eNavaidType.DME Then
			'=======================================================================================
			pBaseLine.FromPoint = GuidNav.pPtPrj
			fDir = RadToDeg(pBaseLine.Angle)
			fDist = pBaseLine.Length
			'        fTmp = hFIX - GuidNav.pPtPrj.Z
			'        fSlDist = Sqr(fDist * fDist + fTmp * fTmp)

			'        fTmp = fSlDist * (1# + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
			'        fRadius = fDist + fTmp
			pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fDir + 90.0, 3 * fDist)
			pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fDir - 90.0, 3 * fDist)

			'        ClipByLine CreatePrjCircle(GuidNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing
			'        fTmp = fSlDist - fSlDist * (1# - DME.ErrorScalingUp) + DME.MinimalError
			'        fRadius = fDist - fTmp

			pTopoOper = pGuidPoly
			pTopoOper.Cut(pCutter, pTmpPoly, pFixPoly)

			pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
			fDist = ReturnDistanceInMeters(InterNav.pPtPrj, PtSDF)
			fInterDir = ReturnAngleInDegrees(InterNav.pPtPrj, PtSDF)
			fTmp = Dir2Azt(InterNav.pPtPrj, fInterDir)

			If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
				Label112.Text = My.Resources.str00227
				TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
				fInterToler = VOR.IntersectingTolerance
			ElseIf InterNav.TypeCode = eNavaidType.NDB Then
				Label112.Text = My.Resources.str00228
				TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp + 180.0, 360.0)))
				fInterToler = NDB.IntersectingTolerance
			ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
				Label112.Text = My.Resources.str00227
				TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
				fInterToler = LLZ.IntersectingTolerance
			End If

			pIntersectPoly.AddPoint(InterNav.pPtPrj)
			pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir - fInterToler, 3.0 * fDist))
			pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir + fInterToler, 3.0 * fDist))
			pIntersectPoly.AddPoint(InterNav.pPtPrj)

			pTopoOper = pIntersectPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pFixPoly = pTopoOper.Intersect(pTmpPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			'        Set pGeomCollection = pFixPoly
			'        Set pResGeomCollection = pTmpPoly
			'        Set pRelation = pTmpPoly
			'
			'        If pGeomCollection.GeometryCount > 1 Then
			'            I = 0
			'            While I < pGeomCollection.GeometryCount
			'                If pResGeomCollection.GeometryCount > 1 Then
			'                    pResGeomCollection.RemoveGeometries 0, pResGeomCollection.GeometryCount
			'                End If
			'                pResGeomCollection.AddGeometry pGeomCollection.Geometry(I)
			'
			'                If pRelation.Contains(PtSDF) Then
			'                    I = I + 1
			'                Else
			'                    pGeomCollection.RemoveGeometries I, 1
			'                End If
			'            Wend
			'        End If
			'=======================================================================================
		Else
			'=======================================================================================
			pBaseLine.FromPoint = InterNav.pPtPrj
			fDir = RadToDeg(pBaseLine.Angle)
			fDist = pBaseLine.Length

			'        fIADir = Modulus(ReturnAngleInDegrees(StepDownFIXs(StepDownsNum - 1).pPtPrj, GuidNav.pPtPrj) + 180.0 * (1 - ComboBox102.ListIndex), 360.0)
			fIADir = StepDownFIXs(StepDownsNum - 1).InDir

			'        If GuidNav.TypeCode = 0 Then
			'            TrackToler = VOR.TrackingTolerance
			''            fConeAngle = VOR.ConeAngle
			'        ElseIf GuidNav.TypeCode = 2 Then
			'            TrackToler = NDB.TrackingTolerance
			''            fConeAngle = NDB.ConeAngle
			'        End If

			'        TrackRange = GuidNav.Range / Cos(DegToRad(TrackToler))
			''        fRConus = Tan(DegToRad(fConeAngle)) * (hFIX - GuidNav.pt.Z)

			'        Set pGuidPoly = New Polygon
			'        pGuidPoly.AddPoint GuidNav.pPtPrj
			'        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler, 100.0 * TrackRange)
			'        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler, 100.0 * TrackRange)
			'        pGuidPoly.AddPoint GuidNav.pPtPrj
			'        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler + 180.0, 100.0 * TrackRange)
			'        pGuidPoly.AddPoint PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler + 180.0, 100.0 * TrackRange)
			'        pGuidPoly.AddPoint GuidNav.pPtPrj

			'        Set pTopoOper = pGuidPoly
			'        pTopoOper.IsKnownSimple_2 = False
			'        pTopoOper.Simplify
			'================================================
			If InterNav.IntersectionType = eIntersectionType.ByDistance Then
				fTmp = SubtractAngles(fIADir, fDir)

				fTmp = hFix - InterNav.pPtPrj.Z
				fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)
				fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
				fRadius = fDist + fTmp

				Label112.Text = My.Resources.str00229	 'fSlDist
				TextBox102.Text = CStr(ConvertDistance(fSlDist, eRoundMode.rmNERAEST))

				pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, fDir + 90.0, 2 * fRadius)
				pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, fDir - 90.0, 2 * fRadius)
				'            If Side > 0 Then
				ClipByLine(CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing)
				'            Else
				'                ClipByLine CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, Nothing, pTmpPoly, Nothing
				'            End If
				'DrawPolygon pTmpPoly, 255

				fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
				fRadius = fDist - fTmp
				pTopoOper = pTmpPoly
				pIntersectPoly = pTopoOper.Difference(CreatePrjCircle(InterNav.pPtPrj, fRadius))
				'DrawPolygon CreatePrjCircle(InterNav.pPtPrj, fRadius), 255
			ElseIf InterNav.IntersectionType = eIntersectionType.OnNavaid Then
				Label112.Text = My.Resources.str00106
				If InterNav.TypeCode = eNavaidType.NDB Then
					NDBFIXTolerArea(InterNav.pPtPrj, fIADir, hFix, pFixPoly)
				Else
					VORFIXTolerArea(InterNav.pPtPrj, fIADir, hFix, pFixPoly)
				End If
				'                Set pFixPoly =pIntersectPoly
			Else
				fTmp = Dir2Azt(InterNav.pPtPrj, fDir)
				If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
					Label112.Text = My.Resources.str00227
					TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
					fInterToler = VOR.IntersectingTolerance
				ElseIf InterNav.TypeCode = eNavaidType.NDB Then
					Label112.Text = My.Resources.str00228
					TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp + 180.0, 360.0)))
					fInterToler = NDB.IntersectingTolerance
				ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
					Label112.Text = My.Resources.str00227
					TextBox102.Text = CStr(System.Math.Round(Modulus(fTmp - InterNav.MagVar, 360.0)))
					fInterToler = LLZ.IntersectingTolerance
				End If
				fInterRange = InterNav.Range / System.Math.Cos(DegToRad(fInterToler))

				pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
				pIntersectPoly.AddPoint(InterNav.pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler, 100.0 * fInterRange))
				pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler, 100.0 * fInterRange))
				pIntersectPoly.AddPoint(InterNav.pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir - fInterToler + 180.0, 100.0 * fInterRange))
				pIntersectPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fDir + fInterToler + 180.0, 100.0 * fInterRange))
				pIntersectPoly.AddPoint(InterNav.pPtPrj)
			End If

			If InterNav.IntersectionType <> eIntersectionType.OnNavaid Then
				pTopoOper = pIntersectPoly
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
				pFixPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			End If
			'================================================
			ptTmp = PointAlongPlane(PtSDF, fIADir + 180.0, 100000.0)
			pCutter.FromPoint = PointAlongPlane(ptTmp, fIADir + 90.0, 100000.0)
			pCutter.ToPoint = PointAlongPlane(ptTmp, fIADir - 90.0, 100000.0)
			pProxi = pFixPoly
			dD = 100000.0 - pProxi.ReturnDistance(pCutter)
		End If
		'=======================================================================================

		pGeomCollection = pFixPoly

		If ((GuidNav.TypeCode = eNavaidType.DME) Or (InterNav.IntersectionType = eIntersectionType.ByDistance)) And (pGeomCollection.GeometryCount > 1) Then
			pResGeomCollection = pTmpPoly
			pRelation = pTmpPoly
			I = 0
			While I < pGeomCollection.GeometryCount
				If pResGeomCollection.GeometryCount > 0 Then
					pResGeomCollection.RemoveGeometries(0, pResGeomCollection.GeometryCount)
				End If
				pResGeomCollection.AddGeometry(pGeomCollection.Geometry(I))

				If pRelation.Contains(PtSDF) Then
					I = I + 1
				Else
					pGeomCollection.RemoveGeometries(I, 1)
				End If
			End While
		End If

		If GuidNav.TypeCode = eNavaidType.DME Then
			dD = 0.0
			pBaseLine.FromPoint = GuidNav.pPtPrj
			pBaseLine.ToPoint = PtSDF
			fDir = pBaseLine.Angle

			For I = 0 To pFixPoly.PointCount - 1
				pBaseLine.ToPoint = pFixPoly.Point(I)
				fTmp = Modulus((pBaseLine.Angle - fDir) * (2 * ComboBox102.SelectedIndex - 1), 2 * PI)
				If fTmp > PI Then fTmp = fTmp - 2 * PI
				If fTmp > dD Then dD = fTmp
			Next I
			dD = dD * fDist
		End If

		StepDownFIXs(StepDownsNum).pFixPoly = pFixPoly

		TextBox104.Text = CStr(ConvertDistance(dD, eRoundMode.rmNERAEST))
		StepDownFIXs(StepDownsNum).pFixPolyElem = DrawPolygon(pFixPoly, RGB(128, 255, 128))
		StepDownFIXs(StepDownsNum).ptElem = DrawPoint(PtSDF, 0)

		StepDownFIXs(StepDownsNum).pFixPolyElem.Locked = True
		StepDownFIXs(StepDownsNum).ptElem.Locked = True



















		'Clone = pFullPoly
		'If FinalNav.TypeCode > eNavaidType.NONE Then
		'	If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
		'		TrackToler = VOR.TrackingTolerance
		'	ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
		'		TrackToler = NDB.TrackingTolerance
		'	ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
		'		TrackToler = LLZ.TrackingTolerance
		'	End If

		'	pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
		'	pPolyClone.AddPoint(FinalNav.pPtPrj)
		'	pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, m_ArDir - TrackToler + 180.0, 3.0 * RModel))
		'	pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, m_ArDir + TrackToler + 180.0, 3.0 * RModel))
		'Else
		'	pPolyClone = Clone.Clone
		'End If

		'pTopo = pPolyClone
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'Select Case IFNavDat(K).IntersectionType
		'	Case eIntersectionType.OnNavaid
		'		D = Point2LineDistancePrj(ptTmpFAF, IFNavDat(K).pPtPrj, m_ArDir + 90.0)
		'		Clone = IFNavDat(K).pPtPrj
		'		IFPnt = Clone.Clone
		'		IFPnt.M = m_ArDir

		'		hIFFix = D * arImDescent_Max.Value + CurrhFAF + fRefHeight - IFNavDat(K).pPtPrj.Z
		'		If IFNavDat(K).TypeCode = eNavaidType.NDB Then
		'			NDBFIXTolerArea(IFNavDat(K).pPtPrj, m_ArDir, hIFFix, pTmpPoly)
		'		Else
		'			VORFIXTolerArea(IFNavDat(K).pPtPrj, m_ArDir, hIFFix, pTmpPoly)
		'		End If

		'		pTopo = pTmpPoly
		'		pTopo.IsKnownSimple_2 = False
		'		pTopo.Simplify()
		'	Case eIntersectionType.ByAngle
		'		If IFNavDat(K).TypeCode = eNavaidType.NDB Then
		'			InterToler = NDB.IntersectingTolerance
		'			fDis = NDB.Range
		'			fDirl = fDirl + 180.0
		'		Else
		'			InterToler = VOR.IntersectingTolerance
		'			fDis = VOR.Range
		'		End If
		'		'            fDis = IFNavDat(K).Range
		'		fDirl = fDirl + IFNavDat(K).MagVar
		'		'==============================================================
		'		d0 = Modulus(IFNavDat(K).ValMax(0) - IFNavDat(K).ValMin(0), 360.0)
		'		d1 = Modulus(fDirl - IFNavDat(K).ValMin(0), 360.0)
		'		If d0 < d1 Then
		'			d1 = Modulus(IFNavDat(K).ValMin(0) - fDirl, 360.0)
		'			d2 = Modulus(fDirl - IFNavDat(K).ValMax(0), 360.0)
		'			If d1 < d2 Then
		'				fDirl = IFNavDat(K).ValMin(0)
		'			Else
		'				fDirl = IFNavDat(K).ValMax(0)
		'			End If

		'			If IFNavDat(K).TypeCode = eNavaidType.NDB Then
		'				d0 = Modulus(fDirl + 180.0 - IFNavDat(K).MagVar, 360.0)
		'			Else
		'				d0 = Modulus(fDirl - IFNavDat(K).MagVar, 360.0)
		'			End If
		'			TextBox0403.Text = CStr(d0)
		'		End If
		'		'==============================================================
		'		fDirl = Azt2Dir(IFNavDat(K).pPtGeo, fDirl)

		'		pt1 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl + InterToler, fDis)
		'		pt2 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl - InterToler, fDis)

		'		IFPnt = New ESRI.ArcGIS.Geometry.Point
		'		pConstruct = IFPnt
		'		pConstruct.ConstructAngleIntersection(IFNavDat(K).pPtPrj, DegToRad(fDirl), ptTmpFAF, DegToRad(m_ArDir))
		'		IFPnt.M = m_ArDir

		'		pSect0 = New ESRI.ArcGIS.Geometry.Polygon
		'		pSect0.AddPoint(IFNavDat(K).pPtPrj)
		'		pSect0.AddPoint(pt1)
		'		pSect0.AddPoint(pt2)
		'		pSect0.AddPoint(IFNavDat(K).pPtPrj)

		'		pTopo = pSect0
		'		pTopo.IsKnownSimple_2 = False
		'		pTopo.Simplify()

		'		pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
		'	Case eIntersectionType.ByDistance
		'		fDirl = DeConvertDistance(fDirl)
		'		N = UBound(IFNavDat(K).ValMax)

		'		If (OptionButton0402.Enabled And OptionButton0402.Checked) Or (N = 0) Then
		'			If (fDirl < IFNavDat(K).ValMin(0)) Or (fDirl > IFNavDat(K).ValMax(0)) Then
		'				If fDirl < IFNavDat(K).ValMin(0) Then fDirl = IFNavDat(K).ValMin(0)
		'				If fDirl > IFNavDat(K).ValMax(0) Then fDirl = IFNavDat(K).ValMax(0)
		'				TextBox0403.Text = CStr(ConvertDistance(fDirl, eRoundMode.rmNERAEST))
		'			End If
		'		Else
		'			If (fDirl < IFNavDat(K).ValMin(1)) Or (fDirl > IFNavDat(K).ValMax(1)) Then
		'				If fDirl < IFNavDat(K).ValMin(1) Then fDirl = IFNavDat(K).ValMin(1)
		'				If fDirl > IFNavDat(K).ValMax(1) Then fDirl = IFNavDat(K).ValMax(1)
		'				TextBox0403.Text = CStr(ConvertDistance(fDirl, eRoundMode.rmNERAEST))
		'			End If
		'		End If

		'		If (IFNavDat(K).ValCnt < 0) Or (OptionButton0402.Enabled And OptionButton0402.Checked) Then
		'			CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptTmpFAF, m_ArDir, IFPnt)
		'		Else
		'			CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptTmpFAF, m_ArDir + 180.0, IFPnt)
		'		End If

		'		IFPnt.M = m_ArDir

		'		fDis = Point2LineDistancePrj(IFPnt, ptTmpFAF, m_ArDir + 90.0)
		'		hIFFix = fDis * IntermAreaPDG + ptTmpFAF.Z - IFNavDat(K).pPtPrj.Z + FictTHR.Z

		'		d0 = System.Math.Sqrt(fDirl * fDirl + hIFFix * hIFFix)
		'		TextBox0407.Text = CStr(ConvertDistance(d0, eRoundMode.rmNERAEST))

		'		d0 = d0 * DME.ErrorScalingUp + DME.MinimalError

		'		D = fDirl + d0
		'		pSect0 = CreatePrjCircle(IFNavDat(K).pPtPrj, D)

		'		pCutter = New ESRI.ArcGIS.Geometry.Polyline
		'		pCutter.FromPoint = PointAlongPlane(IFNavDat(K).pPtPrj, m_ArDir - 90.0, D + D)
		'		pCutter.ToPoint = PointAlongPlane(IFNavDat(K).pPtPrj, m_ArDir + 90.0, D + D)

		'		D = fDirl - d0
		'		pSect1 = CreatePrjCircle(IFNavDat(K).pPtPrj, D)

		'		pTopo = pSect0
		'		pTmpPoly = pTopo.Difference(pSect1)

		'		pTopo = pTmpPoly
		'		pTopo.IsKnownSimple_2 = False
		'		pTopo.Simplify()

		'		If SideDef(pCutter.FromPoint, m_ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

		'		If (IFNavDat(K).ValCnt < 0) Or (OptionButton0402.Enabled And OptionButton0402.Checked) Then
		'			pTopo.Cut(pCutter, pSect1, pSect0)
		'		Else
		'			pTopo.Cut(pCutter, pSect0, pSect1)
		'		End If

		'		pTopo = pSect0
		'		pTopo.IsKnownSimple_2 = False
		'		pTopo.Simplify()
		'		pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
		'End Select

		'pTopo = pTmpPoly
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'pIFTolerArea = pTmpPoly
		''===================================================================
		'pIFFIXPoly = New ESRI.ArcGIS.Geometry.Polygon
		'pIFFIXPoly.AddPoint(pFAFLine.FromPoint)
		'pIFFIXPoly.AddPoint(pFAFLine.ToPoint)

		'pIFFIXPoly.AddPoint(PointAlongPlane(IFPnt, m_ArDir - 90.0, arIFHalfWidth.Value))
		'pIFFIXPoly.AddPoint(PointAlongPlane(IFPnt, m_ArDir + 90.0, arIFHalfWidth.Value))

		'pSect0 = New ESRI.ArcGIS.Geometry.Polygon
		'If FinalNav.TypeCode <> eNavaidType.LLZ Then
		'	pSect0.AddPoint(PointAlongPlane(pFAFLine.ToPoint, m_ArDir + 90.0, 0.25 * pFAFLine.Length))
		'End If

		'pSect0.AddPoint(pIFFIXPoly.Point(1))
		'pSect0.AddPoint(pIFFIXPoly.Point(2))
		'pSect0.AddPoint(PointAlongPlane(IFPnt, m_ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
		'pSect0.AddPoint(pSect0.Point(0))

		'pTopo = pSect0
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'pSect1 = New ESRI.ArcGIS.Geometry.Polygon
		'If FinalNav.TypeCode <> eNavaidType.LLZ Then
		'	pSect1.AddPoint(PointAlongPlane(pFAFLine.FromPoint, m_ArDir - 90.0, 0.25 * pFAFLine.Length))
		'End If
		'pSect1.AddPoint(PointAlongPlane(IFPnt, m_ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
		'pSect1.AddPoint(pIFFIXPoly.Point(3))
		'pSect1.AddPoint(pIFFIXPoly.Point(0))
		'pSect1.AddPoint(pSect1.Point(0))

		'pTopo = pIFFIXPoly
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'IntermediateBaseArea = pTopo.Union(pTmpPoly)

		'pTopo = pSect1
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'IntermediateSecondArea = pTopo.Union(pSect0)

		'D = Point2LineDistancePrj(ptTmpFAF, IFPnt, m_ArDir + 90.0)
		''    hIFFix = D * arImDescent_Max.Value + ptTmpFAF.Z    '+ ptDerPrj.Z
		'hDis = DeConvertDistance(CDbl(TextBox0409.Text))
		'hIFFix = (D - hDis) * IntermAreaPDG + ptTmpFAF.Z

		'TextBox0404.Tag = CStr(hIFFix + fRefHeight)
		'ComboBox0402_SelectedIndexChanged(ComboBox0402, New System.EventArgs())	'    TextBox0404.Text = CStr(ConvertHeight(hIFFix + fRefHeight, eRoundMode.rmNERAEST))
		'TextBox0401.Text = CStr(ConvertDistance(D, eRoundMode.rmNERAEST))

		'IFPnt.Z = hIFFix
		'IFPnt.M = m_ArDir

		'On Error Resume Next
		'IntermediateBaseAreaElem = DrawPolygon(IntermediateBaseArea, 255)
		'IntermediateSecAreaElem = DrawPolygon(IntermediateSecondArea, 0)
		'IFFIXElem = DrawPointWithText(IFPnt, "IF", WPTColor)

		'IntermediateBaseAreaElem.Locked = True
		'IntermediateSecAreaElem.Locked = True
		'IFFIXElem.Locked = True

		'GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'''        RefreshCommandBar mTool, 128
		'On Error GoTo 0
		''========================================================
		'Dim Wfaf As Double
		'Dim fIf2FAFDist As Double

		'Wfaf = 0.5 * pFAFLine.Length
		'fIf2FAFDist = Point2LineDistancePrj(ptTmpFAF, IFPnt, m_ArDir + 90.0)

		'GetIntermedObstacles(ObstacleList, ImObstacles, IntermediateBaseArea, IntermediateSecondArea, Wfaf, fIf2FAFDist, m_ArDir, CurrhFAF, FinalNav.TypeCode, ptTmpFAF, True)

		'N = UBound(ImObstacles.Parts)
		'hTurn = arISegmentMOC.Value
		'K = -1
		'For I = 0 To N
		'	If hTurn < ImObstacles.Parts(I).ReqH Then
		'		hTurn = ImObstacles.Parts(I).ReqH
		'		K = I
		'	End If
		'Next I

		'TextBox0402_Validating(TextBox0402, New System.ComponentModel.CancelEventArgs())

		'NonPrecReportFrm.FillPage04(ImObstacles)
		'TextBox0403.Tag = TextBox0403.Text
		''========================================================
		'If ArrivalProfile.PointsNo = 4 Then
		'	ArrivalProfile.RemovePointByIndex(0)
		'	ArrivalProfile.RemovePointByIndex(0)
		'End If

		'fDis = Point2LineDistancePrj(ptTmpFAF, FictTHR, m_ArDir + 90.0)

		'ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, hIFFix, Modulus(ArAzt - FinalNav.MagVar, 360.0), -IntermAreaPDG, -1, 0)
		'If IntermAreaPDG <> 0 Then
		'	ArrivalProfile.InsertPoint(fIf2FAFDist + fDis - (hIFFix - ptTmpFAF.Z) / IntermAreaPDG, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, -1, 1)
		'Else
		'	ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, -1, 1)
		'End If














		' КТ снижения ================================================================================
		'StepDownFIXs(StepDownsNum).PDG = IAF_PDG 'arIADescent_Nom.Value

		'TextBox110Range.Right = StepDownFIXs(StepDownsNum - 1).pPtPrj.Z + (fDist + StepDownFIXs(StepDownsNum - 1).TotalLength) * IAF_PDG 'fDist
		'ToolTip1.SetToolTip(TextBox110, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(TextBox110Range.Left, eRoundMode.rmNERAEST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TextBox110Range.Right, eRoundMode.rmNERAEST)) + " " + HeightConverter(HeightUnit).Unit)

		'If TextBox110.Tag = "a" Then
		'	fFIXHeight = TextBox110Range.Right
		'	TextBox110.Tag = ""
		'Else
		'	fFIXHeight = TextBox110Range.Left
		'End If

		'ptKT1.Z = fFIXHeight
		'TextBox110.Text = CStr(ConvertHeight(fFIXHeight, eRoundMode.rmNERAEST))	'+ " " + HeightConverter(HeightUnit).Unit

		'StepDownFIXs(StepDownsNum - 1).pPtEnd = ptKT1
		'StepDownFIXs(StepDownsNum).pPtPrj = ptKT1
		'StepDownFIXs(StepDownsNum).pPtStart = ptKT1

		'OldNavFx.Index = -1
		'If ComboBox104.SelectedIndex > -1 Then OldNavFx = SDFInterNavs(ComboBox104.SelectedIndex)

		'SDFInterNavs = GetSDFInterNavs(ptKT1, fDist)
		'ComboBox104.Items.Clear()
		'N = UBound(SDFInterNavs)

		'If N >= 0 Then
		'	J = 0
		'	For I = 0 To N
		'		ComboBox104.Items.Add(SDFInterNavs(I).CallSign)	'+ "/" + SDFInterNavs(I).TypeName
		'		If (OldNavFx.Index > -1) And (SDFInterNavs(I).Index = OldNavFx.Index) Then
		'			J = I
		'		End If
		'	Next I

		'	ComboBox104.SelectedIndex = J
		'End If

		''==========================================================================================================

		'Dim fDist As Double

		'If IAFNav.TypeCode = eNavaidType.DME Then 'DME
		'	fIADir = ReturnAngleInDegrees(IAFNav.pPtPrj, ForFIX.pPtPrj)
		'	fTmp = Modulus(ForFIX.InDir - fIADir - 90.0, 360.0)
		'	If fTmp > 180.0 Then fTmp = 360.0 - fTmp
		'	TurnFlg = 2 * CShort(fTmp < 25.0) + 1

		'	fIAAngle = RadToDeg(fDist / fIADist)
		'	fTmp = fIADir + fIAAngle * TurnFlg
		'	ptKT1 = PointAlongPlane(IAFNav.pPtPrj, fTmp, fIADist)

		'	fTmp = ReturnAngleInDegrees(IAFNav.pPtPrj, ptKT1)
		'	StepDownFIXs(StepDownsNum).OutDir = Modulus(fTmp - 90.0 * TurnFlg, 360.0)
		'	StepDownFIXs(StepDownsNum).ArcDir = TurnFlg

		'Else 'VOR, NDB	'==========================================================================================================
		'	fIADir = ForFIX.InDir 'ReturnAngleInDegrees(ForFIX.pPtPrj, IAFNav.pPtPrj)
		'	ptKT1 = PointAlongPlane(ForFIX.pPtPrj, fIADir + 180.0, fDist)
		'	StepDownFIXs(StepDownsNum).OutDir = ForFIX.InDir
		'	'DrawPointWithText(ptKT1, "ptKT1", RGB(0, 255, 255))

		'	StepDownFIXs(StepDownsNum).ArcDir = 0
		'End If
		''==========================================================================================================

	End Sub
#End If

    Private Sub TextBox110_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox110.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox103_Validating(TextBox110, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(e.KeyChar, TextBox110.Text)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox110_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox110.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim fTmp As Double
        Dim fNewHeight As Double

        If Not IsNumeric(TextBox110.Text) Then Exit Sub

        fNewHeight = DeConvertHeight(CDbl(TextBox110.Text))
        fTmp = fNewHeight

        If fNewHeight < TextBox110Range.Left Then
            fNewHeight = TextBox110Range.Left
        ElseIf fNewHeight > TextBox110Range.Right Then
            fNewHeight = TextBox110Range.Right
        End If

        fFIXHeight = fNewHeight

        If fTmp <> fNewHeight Then
            TextBox110.Text = CStr(ConvertHeight(fNewHeight, eRoundMode.NEAREST))
        End If

        TextBox110.Tag = ""
        'If OptionButton101.Value Then OptionButton101_Click
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub TextBox201_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox201.Validating
        Dim I As Integer
        Dim fTmp As Double
        Dim fMin As Double
        Dim fMax As Double
        Dim fNewVal As Double

        'If ListView201.FocusedItem Is Nothing Then Return
        If ListView201.SelectedItems.Count = 0 Then Return
        If TextBox201.Text = TextBox201.Tag Then Return

        Dim Item As System.Windows.Forms.ListViewItem = ListView201.SelectedItems.Item(0)

        'I = ListView201.FocusedItem.Index
        I = Item.Index
        fMin = LegList(I + 1).MinAlt
        fMax = LegList(I).MaxAlt

        If Not IsNumeric(TextBox201.Text) Then
            '   TextBox201.Text = CStr(ConvertHeight(LegList(I).Minalt, eRoundMode.rmNERAEST))
            'TextBox201.Text = ListView201.FocusedItem.SubItems.Item(6).Text
            TextBox201.Text = Item.SubItems.Item(7).Text
            Return
        End If
        fNewVal = DeConvertHeight(CDbl(TextBox201.Text))
        fTmp = fNewVal

        If fNewVal < fMin Then fNewVal = fMin
        If fNewVal > fMax Then fNewVal = fMax
        If (fNewVal <> fTmp) Then
            TextBox201.Text = CStr(ConvertHeight(fNewVal, eRoundMode.NEAREST))
        End If

        LegList(I).MinAlt = fNewVal
        LegList(I + 1).MaxAlt = fNewVal

        ListView201.Items.Item(I).SubItems.Item(7).Text = TextBox201.Text
        ListView201.Items.Item(I + 1).SubItems.Item(8).Text = TextBox201.Text
        TextBox201.Tag = TextBox201.Text
    End Sub

    Private Sub arHalfWithCmbBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles arHalfWithCmbBox.SelectedIndexChanged
        If arHalfWithCmbBox.SelectedIndex = 1 Then
            arIFHalfWidth.Value = arHalfWithConst2
        Else
            arIFHalfWidth.Value = arHalfWithConst1
        End If
    End Sub

    Private Sub mocLimitCmbBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles mocLimitCmbBox.SelectedIndexChanged
        Dim K As Long
        K = mocLimitCmbBox.SelectedIndex
        If K < 0 Then Exit Sub
        MOCLimit = EnrouteMOCValues(K)
    End Sub
End Class
