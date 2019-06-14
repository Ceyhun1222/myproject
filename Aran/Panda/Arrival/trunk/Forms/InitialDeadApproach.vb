Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports Aran.Aim
Imports Aran.Aim.Features
Imports Aran.Aim.Enums
Imports Aran.Aim.DataTypes
Imports Aran.Geometries
Imports Aran.Queries
Imports Aran.Converters
Imports ESRI.ArcGIS.esriSystem
Imports System.ComponentModel
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports Aran.AranEnvironment
Imports Aran.Aim.Data
Imports Aran.Metadata.Utils


'If bIFFound Then
'Altitude = IFAltitudeInBase
'End If

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CInitialDeadApproach
    Inherits System.Windows.Forms.Form

    Private pGraphics As IGraphicsContainer
    Private pIFptElement As IElement
    Private pTptElement As IElement

    Private pInitLineElement As IElement
    Private pNomTrackElement As IElement
    '=========================================================
    Private pInitialA_PrimAreaElement As IElement
    Private pInitialA_FullAreaElement As IElement

    Private pIntermAA_FullAreaElement As IElement

    Private pTurnA_FullAreaElement As IElement
    Private pTurnA_SecAreaElement As IElement

    Private pTP_FIXAreaElement As IElement
    Private pIF_FIXAreaElement As IElement
    '=========================================================
    Private pTPtPrj As IPoint
    Private pFAFPptPrj As IPoint

    Private pNomTrack As IPolyline
    Private m_pGuidPoly As IPointCollection
    Private pIntermA_FullAreaPoly As IPointCollection
    Private pInitialA_PrimAreaPoly As IPointCollection
    Private pInitialA_FullAreaPoly As IPointCollection

    Private pTurnA_FullAreaPoly As IPointCollection
    Private pTurnA_SecAreaPoly As IPointCollection

    Private pTP_FIXAreaPoly As IPointCollection
    Private pIF_FIXAreaPoly As IPointCollection
    '=========================================================

    Private formStep As Integer = 0
    Private fIAS As Double
    Private fTAS As Double
    Private fBank As Double
    Private fTurnR As Double
    Private fProcAlt As Double

    Private TurnPtAltitude As Double
    Private IFAltitude As Double
    Private IFAltitudeInBase As Double
    Private FAPAltitude As Double

    Private fminInter As Double
    Private fFAFPDir As Double

    Private fDR As Double
    Private fNomLenght As Double

    Private fBetta As Double
    Private MaxIntermReqH As Double
    Private MaxTurnReqH As Double

    Private fDRDir As Double
    Private m_fInitDir As Double
    Private fTPFcDist As Double
    Private fInterSign As Double
    Private fIF_FAFPDist As Double
    Private fFAPSemiWidth As Double
    Private fDistInterFAFP As Double
    Private fDRInterceptAngle As Double
    Private fTxtInterceptAngle As Double

    Private fFIXMaxToler As Double
    Private TextBox008MinVal As Double

    Private iDRType As Integer
    Private iTurnDir As Integer
    Private iCategory As Integer
    Private iErrorCode As Integer

    Private ComboBox106Locked As Boolean
    Private bIFFound As Boolean

    Private pProcedure As InstrumentApproachProcedure

    Private SDFIX0 As StepDownFIX
    Private SDFIX1 As StepDownFIX

    Private ComboBox104List() As WPT_FIXType
    Private ComboBox108List() As WPT_FIXType

    Private GuidanceNavs() As NavaidData
    Private IFIntersectNavs() As NavaidData
    Private TPIntersectNavs() As NavaidData

    Private TextBox101_Interval As LowHigh
    Private TextBox102_Interval As LowHigh
    Private TextBox107_Interval As LowHigh
    Private TextBox108_Interval As LowHigh
    Private TextBox109_Interval(1) As LowHigh

    Private InitialAAObstList As ObstacleContainer
    Private TurnAObstList As ObstacleContainer
    Private IntermAAObstList As ObstacleContainer

    Private StartPoint As WPT_FIXType
    Private IFPoint As WPT_FIXType

    Private ProcedureName As String
    Private IAPArray() As InstrumentApproachProcedure
    Private pLandingTakeoff As LandingTakeoffAreaCollection

    Private HelpContextID As Integer
    Private DeadApproachReport As CDeadApproachReport

    Private bFormInitialised As Boolean = False
    Private PageLabel() As System.Windows.Forms.Label

    Private screenCapture As IScreenCapture

    Private AccurRep As ReportFile
    Private ProtRep As ReportFile
    Private GeomRep As ReportFile

    Private nomTrackIsChanged As Boolean = True
    Dim pPtFIXBegin As IPoint

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        bFormInitialised = True

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '		''====================================================''	'
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.InstrumentApproachProcedure.ToString())
        DeadApproachReport = New CDeadApproachReport()
        HelpContextID = 13100
        pGraphics = GetActiveView().GraphicsContainer

        fDRInterceptAngle = 45.0
        fFAPSemiWidth = 1500.0
        fFIXMaxToler = 3700.0
        TextBox013.Text = CStr(ConvertDistance(fFIXMaxToler, eRoundMode.NEAREST))

        SetComboDroppedWidth(ComboBox001, 1.5 * ComboBox001.Width)

        MultiPage1.SelectedIndex = 0

        TextBox006.Text = CStr(ConvertDistance(3700.0, 3))

        TextBox016.Text = CStr(ConvertDistance(28000.0, 3)) 'CStr(28#)
        TextBox017.Text = CStr(ConvertDistance(6000.0, 3))  'CStr(6#)
        TextBox018.Text = CStr(ConvertDistance(19000.0, 3)) 'CStr(19#)

        Label007.Text = DistanceConverter(DistanceUnit).Unit
        Label009.Text = SpeedConverter(SpeedUnit).Unit
        Label013.Text = HeightConverter(HeightUnit).Unit
        Label015.Text = HeightConverter(HeightUnit).Unit
        Label022.Text = SpeedConverter(SpeedUnit).Unit
        Label024.Text = DistanceConverter(DistanceUnit).Unit
        Label030.Text = DistanceConverter(DistanceUnit).Unit
        Label035.Text = DistanceConverter(DistanceUnit).Unit
        Label036.Text = DistanceConverter(DistanceUnit).Unit
        Label038.Text = DistanceConverter(DistanceUnit).Unit
        Label040.Text = DistanceConverter(DistanceUnit).Unit

        Label101.Text = DistanceConverter(DistanceUnit).Unit
        Label102.Text = DistanceConverter(DistanceUnit).Unit
        Label103.Text = DistanceConverter(DistanceUnit).Unit
        Label105.Text = HeightConverter(HeightUnit).Unit
        Label107.Text = HeightConverter(HeightUnit).Unit

        ComboBox004.Items.Clear()
        ComboBox004.Items.Add(My.Resources.str50127)
        ComboBox004.Items.Add(My.Resources.str50128)
        ComboBox004.SelectedIndex = 0

        ComboBox106.Items.Clear()
        ComboBox106.Items.Add(My.Resources.str50230)
        ComboBox106.Items.Add(My.Resources.str50231)
        ComboBox106.SelectedIndex = 0

        '==================================================
        Text = My.Resources.str50000
        MultiPage1.TabPages.Item(0).Text = My.Resources.str50100
        MultiPage1.TabPages.Item(1).Text = My.Resources.str50200

        PrevBtn.Text = My.Resources.str00150
        NextBtn.Text = My.Resources.str00151
        OkBtn.Text = My.Resources.str00152
        CancelBtn.Text = My.Resources.str00153
        ReportBtn.Text = My.Resources.str00154
        '==================================================
        Frame001.Text = My.Resources.str50101
        Label001.Text = My.Resources.str50102
        Label002.Text = My.Resources.str50103
        Label003.Text = My.Resources.str10107
        Label004.Text = My.Resources.str50104
        Label006.Text = My.Resources.str50105
        Label008.Text = My.Resources.str50107
        Label010.Text = My.Resources.str00232
        Label012.Text = My.Resources.str50117
        Label014.Text = My.Resources.str50118

        Label018.Text = My.Resources.str50121
        Label021.Text = My.Resources.str50122
        Label020.Text = My.Resources.str50106
        Label023.Text = My.Resources.str50108
        Label027.Text = My.Resources.str50120
        Label029.Text = My.Resources.str50112

        Label031.Text = My.Resources.str50109
        Label032.Text = My.Resources.str50124
        Label033.Text = My.Resources.str50125
        Label034.Text = My.Resources.str50126
        Label037.Text = My.Resources.str50110
        Label039.Text = My.Resources.str50111
        '==================================================
        Label104.Text = My.Resources.str00223 + ":"
        Label106.Text = My.Resources.str00223 + ":"

        Frame101.Text = My.Resources.str50201
        OptionButton101.Text = My.Resources.str50202
        OptionButton102.Text = My.Resources.str50203
        OptionButton103.Text = My.Resources.str50204
        Frame104.Text = My.Resources.str50205
        OptionButton104.Text = My.Resources.str50206
        OptionButton105.Text = My.Resources.str50207
        Frame105.Text = My.Resources.str50208
        Label112.Text = My.Resources.str50211

        Frame106.Text = My.Resources.str50209
        OptionButton106.Text = My.Resources.str50210
        OptionButton107.Text = My.Resources.str50213
        Label115.Text = My.Resources.str50212
        Frame1.Text = My.Resources.str50214
        Label117.Text = My.Resources.str50215
        Label123.Text = My.Resources.str50216
        Label118.Text = My.Resources.str50217
        Frame2.Text = My.Resources.str50218
        Label119.Text = My.Resources.str50219
        Label125.Text = My.Resources.str50216

        '==================================================
        DeadApproachReport.SetReportBtn(ReportBtn)
        'SetFormParented(Handle.ToInt32)

        '' ====== 2007
        PageLabel = {Label1_0, Label1_1}
        PageLabel(0).Text = MultiPage1.TabPages.Item(0).Text
        PageLabel(1).Text = MultiPage1.TabPages.Item(1).Text

        FocusStepCaption(0)
        MultiPage1.Top = -21
        Me.Height = Me.Height - 21
        Frame3.Top = Frame3.Top - 21

        ShowPanelBtn.Checked = False

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer

        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim P As Integer

        Dim pIAP As InstrumentApproachProcedure
        Dim pIAPList As List(Of InstrumentApproachProcedure)
        Dim pProcedureLeg As SegmentLeg
        Dim pProcedureTransition As ProcedureTransition
        Dim Break As Boolean

        ComboBox001.Items.Clear()

        pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)

        N = pIAPList.Count
        ReDim IAPArray(N - 1)
        K = -1

        For I = 0 To N - 1
            pIAP = pIAPList.Item(I)
            Break = False

            M = pIAP.FlightTransition.Count
            For J = 0 To M - 1
                pProcedureTransition = pIAP.FlightTransition.Item(J)

                L = pProcedureTransition.TransitionLeg.Count
                If L > 0 Then
                    For P = 0 To L - 1
                        pProcedureLeg = pProcedureTransition.TransitionLeg.Item(P).TheSegmentLeg.GetFeature2()
                        If pProcedureLeg Is Nothing Then
                            Continue For
                        End If

                        If pProcedureLeg.EndPoint Is Nothing Then
                            Continue For
                        End If

                        If pProcedureLeg.EndPoint.Role Is Nothing Then Continue For

                        If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
                            K = K + 1
                            IAPArray(K) = pIAP
                            ComboBox001.Items.Add(pIAP.Name)
                            Break = True
                            Exit For
                        End If
                    Next P

                    If Break Then Exit For
                End If
            Next J
        Next I

        If ComboBox001.Items.Count > 0 Then ComboBox001.SelectedIndex = 0
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

    Private Function CreateGuidPoly(ByRef GuidNav As NavaidData, ByRef ptFIX As IPoint) As IPolygon
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

        pBaseLine = New ESRI.ArcGIS.Geometry.Line
        pBaseLine.FromPoint = GuidNav.pPtPrj
        pBaseLine.ToPoint = ptFIX

        If GuidNav.TypeCode = eNavaidType.DME Then
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
            '===============================================================
        Else
            '=================================================================
            If pBaseLine.Length > distEps Then
                fIADir = ReturnAngleInDegrees(ptFIX, GuidNav.pPtPrj)
            Else
                fIADir = m_fInitDir
            End If

            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                TrackToler = VOR.TrackingTolerance
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                TrackToler = NDB.TrackingTolerance
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                TrackToler = LLZ.TrackingTolerance
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

            pTopoOper = pPoly
            pTopoOper.IsKnownSimple_2 = False

            pTopoOper.Simplify()
            '=================================================================
        End If

        Return pPoly
    End Function

    Private Function CreateFixZone(ByRef ptFIX As IPoint, ByRef GuidNav As NavaidData, ByRef InterNav As NavaidData, ByVal bOnNav As Boolean, ByRef pGuidPoly As IPolygon, ByRef fIADir As Double, ByRef dD As Double) As IPolygon
        Dim pBaseLine As ILine
        Dim pCutter As IPolyline
        Dim ptTmp As IPoint
        Dim pProxi As IProximityOperator

        Dim I As Integer
        Dim fDist As Double
        Dim fDir As Double
        Dim fInterDir As Double
        Dim fInterToler As Double
        Dim fInterRange As Double

        Dim hFix As Double
        Dim fTmp As Double

        Dim pTopoOper As ITopologicalOperator2
        Dim pIntersectPoly As IPointCollection
        Dim pGeomCollection As IGeometryCollection
        Dim pResGeomCollection As IGeometryCollection
        Dim pRelation As IRelationalOperator

        Dim pTmpPoly As IPolygon
        Dim fSlDist As Double
        Dim fRadius As Double
        '=======================================================================================
        hFix = ptFIX.Z
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

            CreateFixZone = pTopoOper.Intersect(pTmpPoly, esriGeometryDimension.esriGeometry2Dimension)
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
            If InterNav.IntersectionType = eIntersectionType.ByDistance Then
                fTmp = SubtractAngles(fIADir, fDir)
                'Side = SideDef(ptFIX, fIADir + 90.0, InterNav.pPtPrj)

                fTmp = hFix - InterNav.pPtPrj.Z
                fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)
                fTmp = fSlDist * (1.0 + DME.ErrorScalingUp) + DME.MinimalError - fSlDist
                fRadius = fDist + fTmp
                pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, fDir + 90.0, 2 * fRadius)
                pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, fDir - 90.0, 2 * fRadius)
                'If Side > 0 Then
                ClipByLine(CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, pTmpPoly, Nothing, Nothing)
                'Else
                '	ClipByLine(CreatePrjCircle(InterNav.pPtPrj, fRadius), pCutter, Nothing, pTmpPoly, Nothing)
                'End If

                fTmp = fSlDist - fSlDist * (1.0 - DME.ErrorScalingUp) + DME.MinimalError
                fRadius = fDist - fTmp
                pTopoOper = pTmpPoly
                pIntersectPoly = pTopoOper.Difference(CreatePrjCircle(InterNav.pPtPrj, fRadius))
                'DrawPolygon CreatePrjCircle(InterNav.pPtPrj, fRadius), 255
            ElseIf InterNav.IntersectionType = eIntersectionType.OnNavaid Then
                bOnNav = True
                fDir = m_fInitDir
                If InterNav.TypeCode = 0 Then
                    VORFIXTolerArea(InterNav.pPtPrj, fDir, hFix, pIntersectPoly)
                ElseIf InterNav.TypeCode = 2 Then
                    NDBFIXTolerArea(InterNav.pPtPrj, fDir, hFix, pIntersectPoly)
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

            If Not bOnNav Then
                pTopoOper = pIntersectPoly
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()

                CreateFixZone = pTopoOper.Intersect(pGuidPoly, esriGeometryDimension.esriGeometry2Dimension)
            Else
                CreateFixZone = pIntersectPoly
            End If
            '================================================
            ptTmp = PointAlongPlane(ptFIX, fIADir + 180.0, 100000.0)
            pCutter.FromPoint = PointAlongPlane(ptTmp, fIADir + 90.0, 100000.0)
            pCutter.ToPoint = PointAlongPlane(ptTmp, fIADir - 90.0, 100000.0)
            pProxi = CreateFixZone
            dD = 100000.0 - pProxi.ReturnDistance(pCutter)
            '================================================
        End If
        '=======================================================================================
        pGeomCollection = CreateFixZone

        If (pGeomCollection.GeometryCount > 1) And (Not pTmpPoly Is Nothing) Then
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
                fTmp = Modulus((pBaseLine.Angle - fDir) * (2 * ComboBox004.SelectedIndex - 1), 2 * PI)
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

    Private Sub CreateIAF_AreaPoly(ByRef GuidNav As NavaidData, ByRef ForFIX As StepDownFIX, ByRef pIAF_IAreaPoly As IPointCollection, ByRef pIAF_IIAreaPoly As IPointCollection)
        Dim TurnDir As Integer
        Dim NavSide As Integer

        Dim L As Double
        Dim fTmp As Double
        Dim fIADir As Double
        Dim fNavDist As Double
        Dim fNextDir As Double

        Dim pTopoOper As ITopologicalOperator2
        Dim pTmpPoly As IPointCollection
        Dim pCirc As IPolygon
        Dim pCutter As IPolyline

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        L = ReturnDistanceInMeters(ForFIX.pPtPrj, GuidNav.pPtPrj)
        fTmp = ReturnAngleInDegrees(ForFIX.pPtPrj, GuidNav.pPtPrj)
        fNextDir = ForFIX.OutDir

        '===============
        If GuidNav.TypeCode = eNavaidType.DME Then
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + arIFHalfWidth.Value)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - arIFHalfWidth.Value)
            pTopoOper = pCirc
            pTmpPoly = pTopoOper.Difference(pTmpPoly)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fTmp + 180.0, 2 * (L + arIFHalfWidth.Value))
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fTmp, 2 * (L + arIFHalfWidth.Value))

            pTopoOper = pTmpPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            If ForFIX.TurnDir = 0 Then
                If Modulus(fTmp - fNextDir, 360.0) > 180.0 Then
                    pTopoOper.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
                Else
                    pTopoOper.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
                End If
            ElseIf ForFIX.TurnDir > 0 Then
                pTopoOper.Cut(pCutter, pCirc, pIAF_IIAreaPoly)
            Else
                pTopoOper.Cut(pCutter, pIAF_IIAreaPoly, pCirc)
            End If

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arIFHalfWidth.Value)
            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)

            pTopoOper = pIAF_IIAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pIAF_IIAreaPoly = pTopoOper.Union(pTmpPoly)
            '===============
            pCirc = CreatePrjCircle(GuidNav.pPtPrj, L + 0.5 * arIFHalfWidth.Value)
            pTmpPoly = CreatePrjCircle(GuidNav.pPtPrj, L - 0.5 * arIFHalfWidth.Value)

            pTopoOper = pCirc
            pTmpPoly = pTopoOper.Difference(pTmpPoly)

            pCutter.FromPoint = PointAlongPlane(GuidNav.pPtPrj, fTmp + 180.0, 2 * (L + arIFHalfWidth.Value))
            pCutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, fTmp, 2 * (L + arIFHalfWidth.Value))

            pTopoOper = pTmpPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            If ForFIX.TurnDir = 0 Then
                If Modulus(fTmp - fNextDir, 360.0) > 180 Then
                    pTopoOper.Cut(pCutter, pCirc, pIAF_IAreaPoly)
                Else
                    pTopoOper.Cut(pCutter, pIAF_IAreaPoly, pCirc)
                End If
            ElseIf ForFIX.TurnDir > 0 Then
                pTopoOper.Cut(pCutter, pCirc, pIAF_IAreaPoly)
            Else
                pTopoOper.Cut(pCutter, pIAF_IAreaPoly, pCirc)
            End If

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)

            pTopoOper = pIAF_IAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pIAF_IAreaPoly = pTopoOper.Union(pTmpPoly)
            '===============
        Else
            If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
                fNavDist = arIFHalfWidth.Value / System.Math.Tan(DegToRad(VOR.SplayAngle))
            ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
                fNavDist = arIFHalfWidth.Value / System.Math.Tan(DegToRad(NDB.SplayAngle))
            ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
                fNavDist = LLZ.Range
            End If

            If ForFIX.TurnDir = 0 Then
                fIADir = fTmp
                L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
            Else
                TurnDir = 2 * ComboBox004.SelectedIndex - 1
                NavSide = SideDef(ForFIX.pPtPrj, ForFIX.OutDir, GuidNav.pPtPrj)

                If SideDef(ForFIX.pPtPrj, ForFIX.OutDir + 90, GuidNav.pPtPrj) > 0 Then
                    If TurnDir = NavSide Then
                        fIADir = fTmp + 180.0
                        L = Max(L - fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                    Else
                        fIADir = fTmp
                        L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                    End If
                Else
                    If TurnDir = NavSide Then
                        fIADir = fTmp + 180.0
                        L = Max(L - fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                    Else
                        fIADir = fTmp
                        L = Max(L + fNavDist, arIASegmentMOC.Value / ForFIX.PDG)
                    End If
                End If
            End If

            pIAF_IIAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0, arIFHalfWidth.Value))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0, arIFHalfWidth.Value))

            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(1), fIADir + 180.0, L))
            pIAF_IIAreaPoly.AddPoint(PointAlongPlane(pIAF_IIAreaPoly.Point(0), fIADir + 180.0, L))

            pTopoOper = pIAF_IIAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir + 90.0, 2 * arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir - 90.0, 2 * arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopoOper = pIAF_IIAreaPoly
            pIAF_IIAreaPoly = pTopoOper.Union(pTmpPoly)

            pIAF_IAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir + 90.0, 0.5 * arIFHalfWidth.Value))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(ForFIX.pPtPrj, fIADir - 90.0, 0.5 * arIFHalfWidth.Value))

            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(1), fIADir + 180.0, L))
            pIAF_IAreaPoly.AddPoint(PointAlongPlane(pIAF_IAreaPoly.Point(0), fIADir + 180.0, L))

            pTopoOper = pIAF_IAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pCutter = New ESRI.ArcGIS.Geometry.Polyline
            pCutter.FromPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir + 90.0, arIFHalfWidth.Value)
            pCutter.ToPoint = PointAlongPlane(ForFIX.pPtPrj, fNextDir - 90.0, arIFHalfWidth.Value)
            pCirc = CreatePrjCircle(ForFIX.pPtPrj, 0.5 * arIFHalfWidth.Value)

            ClipByLine(pCirc, pCutter, Nothing, pTmpPoly, Nothing)
            pTopoOper = pIAF_IAreaPoly
            pIAF_IAreaPoly = pTopoOper.Union(pTmpPoly)
        End If
    End Sub

    Private Sub FillGuidanceNavs()
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer

        Dim fIF_FAFPMin As Double
        Dim fIF_FAFPMax As Double

        Dim fDRMinDist As Double
        Dim fDRMaxDist As Double
        Dim fDist As Double
        Dim fAngle As Double
        Dim fDir As Double
        Dim fMaxTurnAngle As Double
        Dim fTmp As Double

        Dim pt0 As IPoint
        Dim pt1 As IPoint
        Dim pt2 As IPoint
        Dim pt3 As IPoint

        Dim ptIFMin As IPoint
        Dim ptIFMax As IPoint
        Dim FromArray(3) As Double
        Dim ToArray(3) As Double
        Dim pDRBasePoints As IPointCollection

        Dim FromInterval As Interval
        Dim ToInterval As Interval
        Dim AvailInterval As Interval
        Dim ResInterval As Interval
        Dim fInvDRDir As Double

        pDRBasePoints = New ESRI.ArcGIS.Geometry.Multipoint

        fInvDRDir = fDRDir + 180.0

        'fIFMinDist = DeConvertDistance(CDbl(TextBox011.Text))
        'fIFMaxDist = DeConvertDistance(CDbl(TextBox016.Text))

        If bIFFound And CheckBox001.Checked Then
            fIF_FAFPMin = ReturnDistanceInMeters(IFPoint.pPtPrj, StartPoint.pPtPrj)
            fIF_FAFPMax = fIF_FAFPMin
        Else
            fIF_FAFPMin = DeConvertDistance(CDbl(TextBox011.Text))
            fIF_FAFPMax = DeConvertDistance(CDbl(TextBox016.Text))
        End If

        fDRMinDist = DeConvertDistance(CDbl(TextBox017.Text))
        fDRMaxDist = DeConvertDistance(CDbl(TextBox018.Text))

        ptIFMin = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPMin)
        ptIFMax = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPMax)

        pt0 = PointAlongPlane(ptIFMin, fInvDRDir, fDRMinDist)
        pt1 = PointAlongPlane(ptIFMin, fInvDRDir, fDRMaxDist)
        pt2 = PointAlongPlane(ptIFMax, fInvDRDir, fDRMaxDist)
        pt3 = PointAlongPlane(ptIFMax, fInvDRDir, fDRMinDist)

        pDRBasePoints.AddPoint(PointAlongPlane(pt0, fInvDRDir + 90.0 * iDRType, fTurnR))
        pDRBasePoints.AddPoint(PointAlongPlane(pt1, fInvDRDir + 90.0 * iDRType, fTurnR))
        pDRBasePoints.AddPoint(PointAlongPlane(pt2, fInvDRDir + 90.0 * iDRType, fTurnR))
        pDRBasePoints.AddPoint(PointAlongPlane(pt3, fInvDRDir + 90.0 * iDRType, fTurnR))

        N = UBound(NavaidList)
        ReDim GuidanceNavs(N)

        fMaxTurnAngle = CDbl(TextBox105.Text)
        AvailInterval.Left = Modulus(fDRDir, 360.0)
        AvailInterval.Right = Modulus(AvailInterval.Left + fMaxTurnAngle * iDRType, 360.0)

        If Modulus(AvailInterval.Right - AvailInterval.Left, 360.0) > 180.0 Then
            fTmp = AvailInterval.Right
            AvailInterval.Right = AvailInterval.Left
            AvailInterval.Left = fTmp
        End If

        K = 0
        For I = 0 To N
            If NavaidList(I).TypeCode <> eNavaidType.LLZ Then
                J = 0
                Do While J <= 3
                    fDist = ReturnDistanceInMeters(pDRBasePoints.Point(J), NavaidList(I).pPtPrj)
                    fDir = ReturnAngleInDegrees(pDRBasePoints.Point(J), NavaidList(I).pPtPrj)

                    If fDist < fTurnR Then
                        FromArray(0) = 0.0
                        FromArray(1) = 360.0
                        FromArray(2) = 0.0
                        FromArray(3) = 0.0
                        ToArray(0) = 0.0
                        ToArray(1) = 360.0
                        ToArray(2) = 0.0
                        ToArray(3) = 0.0
                        Exit Do
                    End If
                    fAngle = RadToDeg(ArcCos(fTurnR / fDist))
                    FromArray(J) = Modulus(fDir - (90.0 + fAngle) * iDRType, 360.0)
                    ToArray(J) = Modulus(fDir - (90.0 - fAngle) * iDRType, 360.0)

                    J = J + 1
                Loop

                GuidanceNavs(K) = NavaidList(I)

                ReDim GuidanceNavs(K).ValMin(1)
                ReDim GuidanceNavs(K).ValMax(1)
                GuidanceNavs(K).ValCnt = 0
                '===========================================================================================
                If Modulus(FromArray(1) - FromArray(0), 360.0) < 180.0 Then
                    FromInterval.Left = FromArray(0)
                    FromInterval.Right = FromArray(1)
                Else
                    FromInterval.Left = FromArray(1)
                    FromInterval.Right = FromArray(0)
                End If

                If Not AngleInSector(FromArray(2), FromInterval.Left, FromInterval.Right) Then
                    If AnglesSideDef(FromArray(2), FromInterval.Left) < 0 Then
                        FromInterval.Left = FromArray(2)
                    Else
                        FromInterval.Right = FromArray(2)
                    End If
                End If

                If Not AngleInSector(FromArray(3), FromInterval.Left, FromInterval.Right) Then
                    If AnglesSideDef(FromArray(3), FromInterval.Left) < 0 Then
                        FromInterval.Left = FromArray(3)
                    Else
                        FromInterval.Right = FromArray(3)
                    End If
                End If

                If Modulus(FromInterval.Right - FromInterval.Left, 360.0) > 180.0 Then
                    FromInterval.Left = 0.0
                    FromInterval.Right = 360.0
                End If
                '===========================================================================================
                If Modulus(ToArray(1) - ToArray(0), 360.0) < 180.0 Then
                    ToInterval.Left = ToArray(0)
                    ToInterval.Right = ToArray(1)
                Else
                    ToInterval.Left = ToArray(1)
                    ToInterval.Right = ToArray(0)
                End If

                If Not AngleInSector(ToArray(2), ToInterval.Left, ToInterval.Right) Then
                    If AnglesSideDef(ToArray(2), ToInterval.Left) < 0 Then
                        ToInterval.Left = ToArray(2)
                    Else
                        ToInterval.Right = ToArray(2)
                    End If
                End If

                If Not AngleInSector(ToArray(3), ToInterval.Left, ToInterval.Right) Then
                    If AnglesSideDef(ToArray(3), ToInterval.Left) < 0 Then
                        ToInterval.Left = ToArray(3)
                    Else
                        ToInterval.Right = ToArray(3)
                    End If
                End If

                If Modulus(ToInterval.Right - ToInterval.Left, 360.0) > 180.0 Then
                    ToInterval.Left = 0.0
                    ToInterval.Right = 360.0
                End If
                '===========================================================================================
                If (FromInterval.Right = 360.0) And (FromInterval.Left = 0.0) Then
                    GuidanceNavs(K).ValMin(0) = AvailInterval.Left
                    GuidanceNavs(K).ValMax(0) = AvailInterval.Right
                    GuidanceNavs(K).ValCnt = 1
                ElseIf AngleIntervalIntersection(AvailInterval, FromInterval, ResInterval) > 0 Then
                    GuidanceNavs(K).ValMin(0) = ResInterval.Left
                    GuidanceNavs(K).ValMax(0) = ResInterval.Right
                    GuidanceNavs(K).ValCnt = 1
                End If

                If (ToInterval.Right = 360.0) And (ToInterval.Left = 0.0) Then
                    GuidanceNavs(K).ValMin(1) = AvailInterval.Left
                    GuidanceNavs(K).ValMax(1) = AvailInterval.Right
                    GuidanceNavs(K).ValCnt = GuidanceNavs(K).ValCnt Or 2
                ElseIf AngleIntervalIntersection(AvailInterval, ToInterval, ResInterval) > 0 Then
                    GuidanceNavs(K).ValMin(1) = ResInterval.Left
                    GuidanceNavs(K).ValMax(1) = ResInterval.Right
                    GuidanceNavs(K).ValCnt = GuidanceNavs(K).ValCnt Or 2
                End If

                If (GuidanceNavs(K).ValCnt > 0) And (AnglesSideDef(Math.Round(ResInterval.Left + 0.4999), Math.Round(ResInterval.Right - 0.4999)) < 0) Then
                    K = K + 1
                End If
            End If
        Next I

        ComboBox103.Items.Clear()

        If K > 0 Then
            ReDim Preserve GuidanceNavs(K - 1)
            For I = 0 To K - 1
                ComboBox103.Items.Add(GuidanceNavs(I).CallSign)
            Next I
            ComboBox103.SelectedIndex = 0
        Else
            ReDim GuidanceNavs(-1)
        End If
    End Sub

    Private Sub GetValidIFIntersectFacilities()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer
        Dim T As Integer
        Dim Side As Integer

        Dim fTmp As Double
        Dim fDir As Double
        Dim fHFix As Double
        Dim fDist As Double
        Dim fDist0 As Double
        Dim fDist1 As Double
        Dim InterToler As Double

        Dim pProxi As IProximityOperator
        Dim pTopoOper As ITopologicalOperator2

        Dim pCutter As IPolyline
        Dim pNavLine As IPolyline
        Dim pTolerLine As IPolyline
        Dim pSect0 As IPointCollection
        Dim pSect1 As IPointCollection
        Dim pTmpPoly As IPointCollection
        Dim pIntesectPoly As IPointCollection

        ComboBox105.Items.Clear()
        N = UBound(NavaidList)
        M = UBound(DMEList)
        T = N + M + 1

        If T < 0 Then
            ReDim IFIntersectNavs(-1)
        Else
            ReDim IFIntersectNavs(T)
            pProxi = m_pGuidPoly
            pNavLine = New ESRI.ArcGIS.Geometry.Polyline
            pIntesectPoly = New ESRI.ArcGIS.Geometry.Polygon
            pNavLine.FromPoint = PointAlongPlane(IFPoint.pPtPrj, fFAFPDir, 100000.0)
            pNavLine.ToPoint = PointAlongPlane(IFPoint.pPtPrj, fFAFPDir + 180.0, 100000.0)

            J = -1

            For I = 0 To N
                If pProxi.ReturnDistance(NavaidList(I).pPtPrj) > 0 Then
                    fDist = ReturnDistanceInMeters(NavaidList(I).pPtPrj, IFPoint.pPtPrj)
                    fDir = ReturnAngleInDegrees(NavaidList(I).pPtPrj, IFPoint.pPtPrj)
                    If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                        InterToler = VOR.IntersectingTolerance
                    ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                        InterToler = NDB.IntersectingTolerance
                    ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
                        InterToler = LLZ.IntersectingTolerance
                    End If

                    If pIntesectPoly.PointCount > 0 Then pIntesectPoly.RemovePoints(0, pIntesectPoly.PointCount)

                    pIntesectPoly.AddPoint(NavaidList(I).pPtPrj)
                    pIntesectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir + InterToler, fDist + 100000.0))
                    pIntesectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir - InterToler, fDist + 100000.0))
                    pIntesectPoly.AddPoint(NavaidList(I).pPtPrj)

                    pTopoOper = pIntesectPoly
                    pTopoOper.IsKnownSimple_2 = False
                    pTopoOper.Simplify()

                    pTolerLine = pTopoOper.Intersect(pNavLine, esriGeometryDimension.esriGeometry1Dimension)
                    If Not pTolerLine.IsEmpty() Then
                        fDist0 = ReturnDistanceInMeters(IFPoint.pPtPrj, pTolerLine.FromPoint)
                        fDist1 = ReturnDistanceInMeters(IFPoint.pPtPrj, pTolerLine.ToPoint)
                        If (fDist0 <= fFIXMaxToler) And (fDist1 <= fFIXMaxToler) Then
                            J = J + 1
                            IFIntersectNavs(J) = NavaidList(I)
                            IFIntersectNavs(J).IntersectionType = eIntersectionType.ByAngle
                            IFIntersectNavs(J).ValCnt = 0
                        End If
                    End If
                End If
            Next I

            pCutter = New ESRI.ArcGIS.Geometry.Polyline

            For I = 0 To M
                fDist = ReturnDistanceInMeters(DMEList(I).pPtPrj, IFPoint.pPtPrj)
                fDir = ReturnAngleInDegrees(DMEList(I).pPtPrj, IFPoint.pPtPrj)
                '=====================================================================================
                fHFix = fProcAlt - DMEList(I).pPtGeo.Z 'IFPoint.pPtPrj.Z
                fTmp = System.Math.Sqrt(fDist * fDist + fHFix * fHFix)
                fTmp = fTmp * DME.ErrorScalingUp + DME.MinimalError

                fDist0 = fDist + fTmp
                pSect0 = CreatePrjCircle(DMEList(I).pPtPrj, fDist0)

                fDist1 = fDist - fTmp
                pSect1 = CreatePrjCircle(DMEList(I).pPtPrj, fDist1)

                pTopoOper = pSect0
                pTmpPoly = pTopoOper.Difference(pSect1)
                '=====================================================================================
                pCutter.FromPoint = PointAlongPlane(DMEList(I).pPtPrj, fDir - 90.0, fDist + fDist)
                pCutter.ToPoint = PointAlongPlane(DMEList(I).pPtPrj, fDir + 90.0, fDist + fDist)

                If SideDef(pCutter.FromPoint, fDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

                pTopoOper = pTmpPoly

                If SideDef(DMEList(I).pPtPrj, fDir + 90.0, IFPoint.pPtPrj) > 0 Then
                    pTopoOper.Cut(pCutter, pSect1, pIntesectPoly)
                Else
                    pTopoOper.Cut(pCutter, pIntesectPoly, pSect1)
                End If

                pTopoOper = pIntesectPoly
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()

                pTolerLine = pTopoOper.Intersect(pNavLine, esriGeometryDimension.esriGeometry1Dimension)

                If Not pTolerLine.IsEmpty() Then
                    fDist0 = ReturnDistanceInMeters(IFPoint.pPtPrj, pTolerLine.FromPoint)
                    fDist1 = ReturnDistanceInMeters(IFPoint.pPtPrj, pTolerLine.ToPoint)
                    If (fDist0 <= fFIXMaxToler) And (fDist1 <= fFIXMaxToler) Then
                        J = J + 1
                        IFIntersectNavs(J) = DMEList(I)
                        IFIntersectNavs(J).IntersectionType = eIntersectionType.ByDistance
                        IFIntersectNavs(J).ValCnt = 0
                    End If
                End If
            Next I

            For I = 0 To N
                If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.DME) Then
                    fDist0 = VOR.OnNAVRadius
                ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                    fDist0 = NDB.OnNAVRadius
                Else 'If NavaidList(I).TypeCode = CodeLLZ Then
                    fDist0 = -10000.0#
                End If

                fDist1 = Point2LineDistancePrj(NavaidList(I).pPtPrj, IFPoint.pPtPrj, fFAFPDir)
                If fDist1 <= fDist0 Then
                    Side = SideDef(NavaidList(I).pPtPrj, fFAFPDir + 90.0, pFAFPptPrj)
                    If Side = 0 Then Side = 1
                    fDist0 = Side * Point2LineDistancePrj(NavaidList(I).pPtPrj, pFAFPptPrj, fFAFPDir + 90.0)

                    If (fDist0 >= TextBox107_Interval.Low) And (fDist0 <= TextBox107_Interval.High) Then
                        J = J + 1
                        IFIntersectNavs(J) = NavaidList(I)
                        IFIntersectNavs(J).IntersectionType = eIntersectionType.OnNavaid
                        IFIntersectNavs(J).ValCnt = -1
                    End If
                End If
            Next I

            If J >= 0 Then
                ReDim Preserve IFIntersectNavs(J)
                For I = 0 To J
                    ComboBox105.Items.Add(IFIntersectNavs(I).CallSign)
                Next I
                ComboBox105.SelectedIndex = 0
            Else
                ReDim IFIntersectNavs(-1)
            End If

            pCutter = Nothing
            pNavLine = Nothing
            pIntesectPoly = Nothing
        End If
    End Sub

    Private Sub GetValidTPIntersectFacilities()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer
        Dim T As Integer

        Dim fTmp As Double
        Dim fDir As Double
        Dim fHFix As Double
        Dim fDist As Double
        Dim fDist0 As Double
        Dim fDist1 As Double
        Dim InterToler As Double
        Dim pInitGuidPoly As IPointCollection

        Dim pProxi As IProximityOperator
        Dim pTopoOper As ITopologicalOperator2
        Dim InitGuidNav As NavaidData
        Dim pCutter As IPolyline
        Dim pNavLine As IPolyline
        Dim pTolerLine As IPolyline
        Dim pSect0 As IPointCollection
        Dim pSect1 As IPointCollection
        Dim pTmpPoly As IPointCollection
        Dim pIntesectPoly As IPointCollection

        ComboBox107.Items.Clear()

        InitGuidNav = GuidanceNavs(ComboBox103.SelectedIndex)

        pInitGuidPoly = CreateGuidPoly(InitGuidNav, pTPtPrj)

        N = UBound(NavaidList)
        M = UBound(DMEList)
        T = N + M + 1

        If T < 0 Then
            ReDim TPIntersectNavs(-1)
        Else
            ReDim TPIntersectNavs(T)
            pProxi = pInitGuidPoly
            pNavLine = New ESRI.ArcGIS.Geometry.Polyline

            pIntesectPoly = New ESRI.ArcGIS.Geometry.Polygon
            pNavLine.FromPoint = PointAlongPlane(pTPtPrj, m_fInitDir, 100000.0)
            pNavLine.ToPoint = PointAlongPlane(pTPtPrj, m_fInitDir + 180.0, 100000.0)

            J = -1

            For I = 0 To N
                If pProxi.ReturnDistance(NavaidList(I).pPtPrj) > 0 Then
                    fDist = ReturnDistanceInMeters(NavaidList(I).pPtPrj, pTPtPrj)
                    fDir = ReturnAngleInDegrees(NavaidList(I).pPtPrj, pTPtPrj)
                    If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                        InterToler = VOR.IntersectingTolerance
                    ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                        InterToler = NDB.IntersectingTolerance
                    ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
                        InterToler = LLZ.IntersectingTolerance
                    End If

                    If pIntesectPoly.PointCount > 0 Then pIntesectPoly.RemovePoints(0, pIntesectPoly.PointCount)

                    pIntesectPoly.AddPoint(NavaidList(I).pPtPrj)
                    pIntesectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir + InterToler, fDist + 100000.0))
                    pIntesectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir - InterToler, fDist + 100000.0))
                    pIntesectPoly.AddPoint(NavaidList(I).pPtPrj)
                    pTopoOper = pIntesectPoly
                    pTopoOper.IsKnownSimple_2 = False
                    pTopoOper.Simplify()

                    pTolerLine = pTopoOper.Intersect(pNavLine, esriGeometryDimension.esriGeometry1Dimension)
                    If Not pTolerLine.IsEmpty() Then
                        fDist0 = ReturnDistanceInMeters(pTPtPrj, pTolerLine.FromPoint)
                        fDist1 = ReturnDistanceInMeters(pTPtPrj, pTolerLine.ToPoint)
                        If (fDist0 <= fFIXMaxToler) And (fDist1 <= fFIXMaxToler) Then
                            J = J + 1
                            TPIntersectNavs(J) = NavaidList(I)
                            TPIntersectNavs(J).IntersectionType = eIntersectionType.ByAngle
                            TPIntersectNavs(J).ValCnt = 0
                        End If
                    End If
                End If
            Next I

            pCutter = New ESRI.ArcGIS.Geometry.Polyline

            For I = 0 To M
                fDist = ReturnDistanceInMeters(DMEList(I).pPtPrj, pTPtPrj)
                fDir = ReturnAngleInDegrees(DMEList(I).pPtPrj, pTPtPrj)
                '=====================================================================================
                fHFix = fProcAlt - DMEList(I).pPtGeo.Z 'pTPtPrj.Z
                fTmp = System.Math.Sqrt(fDist * fDist + fHFix * fHFix)
                fTmp = fTmp * DME.ErrorScalingUp + DME.MinimalError

                fDist0 = fDist + fTmp
                pSect0 = CreatePrjCircle(DMEList(I).pPtPrj, fDist0)

                fDist1 = fDist - fTmp
                pSect1 = CreatePrjCircle(DMEList(I).pPtPrj, fDist1)

                pTopoOper = pSect0
                pTmpPoly = pTopoOper.Difference(pSect1)
                '=====================================================================================
                pCutter.FromPoint = PointAlongPlane(DMEList(I).pPtPrj, fDir - 90.0, fDist + fDist)
                pCutter.ToPoint = PointAlongPlane(DMEList(I).pPtPrj, fDir + 90.0, fDist + fDist)

                If SideDef(pCutter.FromPoint, fDir, pCutter.ToPoint) > 0 Then
                    pCutter.ReverseOrientation()
                End If

                pTopoOper = pTmpPoly

                If SideDef(DMEList(I).pPtPrj, fDir + 90.0, pTPtPrj) > 0 Then
                    pTopoOper.Cut(pCutter, pSect1, pIntesectPoly)
                Else
                    pTopoOper.Cut(pCutter, pIntesectPoly, pSect1)
                End If

                pTopoOper = pIntesectPoly
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()

                pTolerLine = pTopoOper.Intersect(pNavLine, esriGeometryDimension.esriGeometry1Dimension)

                If Not pTolerLine.IsEmpty() Then
                    fDist0 = ReturnDistanceInMeters(pTPtPrj, pTolerLine.FromPoint)
                    fDist1 = ReturnDistanceInMeters(pTPtPrj, pTolerLine.ToPoint)
                    If (fDist0 <= fFIXMaxToler) And (fDist1 <= fFIXMaxToler) Then
                        J = J + 1
                        TPIntersectNavs(J) = DMEList(I)
                        TPIntersectNavs(J).IntersectionType = eIntersectionType.ByDistance
                        TPIntersectNavs(J).ValCnt = 0
                    End If
                End If
            Next I

            '===============================================================================
            'Dim Ix As Long
            'Ix = 1 - ComboBox106.ListIndex
            For I = 0 To N
                If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
                    fDist0 = VOR.OnNAVRadius
                ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
                    fDist0 = NDB.OnNAVRadius
                Else 'If NavaidList(I).TypeCode = CodeLLZ Then
                    fDist0 = -10000.0
                End If

                fDist1 = ReturnDistanceInMeters(NavaidList(I).pPtPrj, pTPtPrj)
                If fDist1 <= fDist0 Then
                    'Side = SideDef(NavaidList(I).pPtPrj, fInitDir + 90.0, pFAFPptPrj)
                    'If Side = 0 Then Side = 1
                    '	fDist0 = Side * Point2LineDistancePrj(NavaidList(I).pPtPrj, pFAFPptPrj, fInitDir + 90.0)
                    '	If (fDist0 >= TextBox109_Interval(Ix).Low) And (fDist0 <= TextBox109_Interval(Ix).High) Then
                    J = J + 1
                    TPIntersectNavs(J) = NavaidList(I)
                    TPIntersectNavs(J).IntersectionType = eIntersectionType.OnNavaid
                    TPIntersectNavs(J).ValCnt = -1
                    'End If
                End If
            Next I

            pCutter = Nothing
            pNavLine = Nothing
            pIntesectPoly = Nothing

            If J >= 0 Then
                ReDim Preserve TPIntersectNavs(J)

                For I = 0 To J
                    ComboBox107.Items.Add(TPIntersectNavs(I).CallSign)
                Next I
                ComboBox107.SelectedIndex = 0
            Else
                ReDim TPIntersectNavs(-1)
            End If
        End If
    End Sub

    Private Sub FillComboBox104()
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim Idx As Integer
        Dim fTmp0 As Double
        Dim fTmp1 As Double

        Dim pPtTmp0 As IPoint
        Dim pPtTmp1 As IPoint
        Dim pSector0 As IPointCollection
        Dim pSector1 As IPointCollection

        Dim pSector As IPointCollection
        Dim pSectProxi As IProximityOperator
        Dim pNavProxi As IProximityOperator
        Dim pTopoOper As ITopologicalOperator2
        Dim oldWPT As WPT_FIXType

        If ComboBox103.Items.Count < 0 Then Return
        If ComboBox103.SelectedIndex < 0 Then Return
        K = ComboBox103.SelectedIndex

        L = ComboBox104.SelectedIndex
        If L >= 0 Then
            oldWPT = CType(ComboBox104List(L), WPT_FIXType)
        End If

        If GuidanceNavs(K).ValCnt = 3 Then
            If OptionButton104.Checked Then
                fTmp0 = GuidanceNavs(K).ValMax(0)
                fTmp1 = GuidanceNavs(K).ValMin(0)
            Else
                fTmp0 = GuidanceNavs(K).ValMax(1)
                fTmp1 = GuidanceNavs(K).ValMin(1)
            End If
        ElseIf GuidanceNavs(K).ValCnt And 1 Then
            fTmp0 = GuidanceNavs(K).ValMax(0)
            fTmp1 = GuidanceNavs(K).ValMin(0)
        ElseIf GuidanceNavs(K).ValCnt And 2 Then
            fTmp0 = GuidanceNavs(K).ValMax(1)
            fTmp1 = GuidanceNavs(K).ValMin(1)
        End If

        pPtTmp0 = PointAlongPlane(GuidanceNavs(K).pPtPrj, fTmp0, 150000.0)
        pPtTmp1 = PointAlongPlane(GuidanceNavs(K).pPtPrj, fTmp1, 150000.0)

        pSector0 = CreateArcPrj(GuidanceNavs(K).pPtPrj, pPtTmp0, pPtTmp1, -1)
        pSector0.AddPoint(GuidanceNavs(K).pPtPrj)

        pTopoOper = pSector0
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pPtTmp0 = PointAlongPlane(GuidanceNavs(K).pPtPrj, fTmp0 + 180.0, 150000.0)
        pPtTmp1 = PointAlongPlane(GuidanceNavs(K).pPtPrj, fTmp1 + 180.0, 150000.0)

        pSector1 = CreateArcPrj(GuidanceNavs(K).pPtPrj, pPtTmp0, pPtTmp1, -1)
        pSector1.AddPoint(GuidanceNavs(K).pPtPrj)

        pTopoOper = pSector1
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pSector = pTopoOper.Union(pSector0)

        pTopoOper = pSector
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        '==========================================================================================
        ComboBox104.Items.Clear()
        N = UBound(WPTList)
        ComboBox104.Enabled = False
        OptionButton107.Enabled = False

        If N < 0 Then
            OptionButton106.Checked = True
            ReDim ComboBox104List(-1)
            Return
        End If

        ReDim ComboBox104List(N)

        'DrawPolygon(pSector, , ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
        'Application.DoEvents()
        pSectProxi = pSector
        pNavProxi = GuidanceNavs(K).pPtPrj
        J = 0

        For I = 0 To N
            If (pSectProxi.ReturnDistance(WPTList(I).pPtPrj) = 0.0) And (pNavProxi.ReturnDistance(WPTList(I).pPtPrj) > distEps) Then
                ComboBox104List(J) = WPTList(I)
                ComboBox104.Items.Add(ComboBox104List(J).Name)
                J = J + 1
            End If
        Next I

        If J = 0 Then
            OptionButton106.Checked = True
            ReDim ComboBox104List(-1)
            Return
        End If

        ReDim Preserve ComboBox104List(J - 1)

        OptionButton107.Enabled = True
        ComboBox104.Enabled = True

        If L < 0 Or formStep = 0 Then
            ComboBox104.SelectedIndex = 0
        Else
            Idx = -1
            For M = 0 To ComboBox104.Items.Count - 1
                If ComboBox104List(M).Identifier = oldWPT.Identifier Then
                    Idx = M
                    ComboBox104.SelectedIndex = M
                    Exit For
                End If
            Next
            If Idx < 0 Then
                ComboBox104.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub FillComboBox108(ByVal InitialDir As Double)
        Dim I As Long
        Dim J As Long
        Dim K As Long
        Dim N As Long

        Dim fTmp As Double
        Dim fDir As Double
        Dim fDist As Double
        Dim fAlpha As Double
        Dim fDistPtCenter As Double

        Dim pPtCenter As IPoint

        ComboBox108.Items.Clear()
        N = UBound(WPTList)

        CheckBox101.Enabled = False
        ComboBox108.Enabled = False

        If N < 0 Then
            ReDim ComboBox108List(-1)
            Return
        End If

        ReDim ComboBox108List(0 To N)

        K = ComboBox103.SelectedIndex
        fDistPtCenter = iDRType * fTurnR

        J = -1
        For I = 0 To N
            fDist = Point2LineDistancePrj(GuidanceNavs(K).pPtPrj, WPTList(I).pPtPrj, InitialDir)

            If fDist < 30.0 Then
                pPtCenter = PointAlongPlane(WPTList(I).pPtPrj, InitialDir - 90.0, fDistPtCenter)

                fDist = ReturnDistanceInMeters(pPtCenter, IFPoint.pPtPrj)
                fDir = ReturnAngleInDegrees(pPtCenter, IFPoint.pPtPrj)
                fAlpha = RadToDeg(ArcSin(fTurnR / fDist))

                fTmp = Modulus((fDir - iDRType * fAlpha - fFAFPDir) * iTurnDir, 360.0)
                If fTmp < 90.0 Then
                    J = J + 1
                    ComboBox108List(J) = WPTList(I)
                    ComboBox108.Items.Add(ComboBox108List(J).Name)
                End If
            End If
        Next I

        If J < 0 Then
            ReDim ComboBox108List(-1)
            Return
        End If

        ReDim Preserve ComboBox108List(0 To J)
        CheckBox101.Enabled = True
        ComboBox108.Enabled = CheckBox101.Checked
        ComboBox108.SelectedIndex = 0
    End Sub

    Private Sub CreateNomTrack(Optional ByVal bFillIntersect As Boolean = True)
        Dim bVa As Boolean
        Dim K As Integer

        Dim fdL As Double
        Dim delta As Double
        Dim fCurrR As Double
        Dim fTmpDir0 As Double
        Dim fCurrPhi As Double
        Dim fIF_FAF15 As Double
        Dim fFIXWidth As Double
        Dim fInvDRDir As Double

        Dim pPt0 As IPoint
        Dim pPt1 As IPoint
        Dim pPt2 As IPoint
        Dim pPt3 As IPoint
        Dim pPtY As IPoint
        Dim pPtA0 As IPoint
        Dim pPtA1 As IPoint
        Dim pPtC0 As IPoint
        Dim pPtC1 As IPoint
        Dim pPtTmp As IPoint
        Dim pPtCntr As IPoint

        Dim pPtInitBegin As IPoint

        Dim pClone As IClone
        Dim pConstructor As IConstructPoint
        Dim pTopoOper As ITopologicalOperator3

        Dim pCircle As IPointCollection
        Dim pTmpPoly0 As IPointCollection
        Dim pTmpPoly1 As IPointCollection
        Dim pTrackPoints As IPointCollection

        Try
            If Not pIFptElement Is Nothing Then pGraphics.DeleteElement(pIFptElement)
        Catch ex As Exception
        End Try
        Try
            If Not pTptElement Is Nothing Then pGraphics.DeleteElement(pTptElement)
        Catch ex As Exception
        End Try
        Try
            If Not pNomTrackElement Is Nothing Then pGraphics.DeleteElement(pNomTrackElement)
        Catch ex As Exception
        End Try

        Try
            If Not pIntermAA_FullAreaElement Is Nothing Then pGraphics.DeleteElement(pIntermAA_FullAreaElement)
        Catch ex As Exception
        End Try
        Try
            If Not pInitialA_PrimAreaElement Is Nothing Then pGraphics.DeleteElement(pInitialA_PrimAreaElement)
        Catch ex As Exception
        End Try
        Try
            If Not pInitialA_FullAreaElement Is Nothing Then pGraphics.DeleteElement(pInitialA_FullAreaElement)
        Catch ex As Exception
        End Try

        Try
            If Not pTurnA_FullAreaElement Is Nothing Then pGraphics.DeleteElement(pTurnA_FullAreaElement)
        Catch ex As Exception
        End Try
        Try
            If Not pTurnA_SecAreaElement Is Nothing Then pGraphics.DeleteElement(pTurnA_SecAreaElement)
        Catch ex As Exception
        End Try
        Try
            If Not pTP_FIXAreaElement Is Nothing Then pGraphics.DeleteElement(pTP_FIXAreaElement)
        Catch ex As Exception
        End Try
        Try
            If Not pIF_FIXAreaElement Is Nothing Then pGraphics.DeleteElement(pIF_FIXAreaElement)
        Catch ex As Exception
        End Try

        nomTrackIsChanged = True

        fInvDRDir = fDRDir + 180.0

        If Not CheckBox001.Checked Then
            IFPoint.pPtPrj = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPDist)
            IFPoint.pPtPrj.M = fDRDir
            IFPoint.pPtPrj.Z = IFAltitude '	fProcAlt	'FAPAltitude
        End If

        pPt1 = PointAlongPlane(IFPoint.pPtPrj, fInvDRDir, fDR)
        pPt1.M = fDRDir

        pPtCntr = PointAlongPlane(pPt1, fInvDRDir + 90.0 * iDRType, fTurnR)
        pPt0 = PointAlongPlane(pPtCntr, m_fInitDir + 90.0 * iDRType, fTurnR)
        pPt0.M = m_fInitDir

        If bIFFound And CheckBox001.Checked Then
            pPt0.Z = IFAltitudeInBase
        Else
            pPt0.Z = IFPoint.pPtPrj.Z
        End If

        delta = fTurnR * Math.Tan(DegToRad(0.5 * fDRInterceptAngle))

        pPt2 = PointAlongPlane(IFPoint.pPtPrj, fInvDRDir, delta)
        pPt2.M = fDRDir

        pPt3 = PointAlongPlane(IFPoint.pPtPrj, fFAFPDir, delta)
        pPt3.M = fFAFPDir
        pPt3.Z = TurnPtAltitude

        '=============================================
        pTrackPoints = New ESRI.ArcGIS.Geometry.Multipoint

        pTrackPoints.AddPoint(pPt0)
        pTrackPoints.AddPoint(pPt1)

        pTrackPoints.AddPoint(pPt2)
        pTrackPoints.AddPoint(pPt3)

        pNomTrack = CalcTrajectoryFromMultiPoint(pTrackPoints)

        Dim pZAware As IZAware
        Dim pZ As IZ
        pZAware = pNomTrack
        pZAware.ZAware = True

        pZ = pNomTrack
        pZ.CalculateNonSimpleZs()

        pClone = pNomTrack

        pTmpPoly0 = pClone.Clone
        pTmpPoly0.AddPoint(pFAFPptPrj)
        pTmpPoly0.AddPoint(PointAlongPlane(pTPtPrj, m_fInitDir + 180.0, 38000.0), 0)

        pNomTrackElement = DrawPolyLine(pTmpPoly0, 255, 2)
        pNomTrackElement.Locked = True

        If bFillIntersect Then GetValidIFIntersectFacilities()
        '==============================================================================
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        K = ComboBox105.SelectedIndex
        If K < 0 Then Return

        If IFIntersectNavs(K).IntersectionType = eIntersectionType.OnNavaid Then
            fFIXWidth = 1900.0
            If fTAS < 335.0 Then
                fCurrR = 9300.0
                fCurrPhi = 22.0
            Else
                fCurrPhi = 14.0
                If fProcAlt <= 1500.0 Then
                    fCurrR = 10200.0
                Else
                    fCurrR = 12000.0
                End If
            End If
        ElseIf IFIntersectNavs(K).IntersectionType = eIntersectionType.ByAngle Then
            fFIXWidth = 4600.0

            If fTAS < 335.0 Then
                fCurrPhi = 22.0
                If fProcAlt <= 1500.0 Then
                    fCurrR = 10200.0
                Else
                    fCurrR = 11100.0
                End If
            Else
                fCurrPhi = 14.0
                If fProcAlt <= 1500.0 Then
                    fCurrR = 12000.0
                Else
                    fCurrR = 13900.0
                End If
            End If
        ElseIf IFIntersectNavs(K).IntersectionType = eIntersectionType.ByDistance Then
            fFIXWidth = 1900.0
            If fTAS < 335.0 Then
                fCurrPhi = 22.0
                fCurrR = 9300.0
            Else
                fCurrPhi = 14.0
                If fProcAlt <= 1500.0 Then
                    fCurrR = 10200.0
                Else
                    fCurrR = 12000.0
                End If
            End If
        Else 'If IFIntersectNavs(K).TypeCode = 3 Then
            Return
        End If

        'Interm Aproach Area ==============================================================================
        pIntermA_FullAreaPoly = Nothing
        pIntermA_FullAreaPoly = New ESRI.ArcGIS.Geometry.Polygon

        fIF_FAF15 = fIF_FAFPDist / System.Math.Cos(DegToRad(15.0))

        pPtA0 = PointAlongPlane(pFAFPptPrj, fFAFPDir + 90.0 * iTurnDir, fFAPSemiWidth)
        pPtA1 = PointAlongPlane(pFAFPptPrj, fFAFPDir - 90.0 * iTurnDir, fFAPSemiWidth)
        pPtFIXBegin = PointAlongPlane(pTPtPrj, m_fInitDir + 180.0, fFIXWidth)

        pPtC0 = New ESRI.ArcGIS.Geometry.Point
        pConstructor = pPtC0
        If ComboBox003.SelectedIndex = 0 Then
            pPtTmp = pPtFIXBegin
            fTmpDir0 = ReturnAngleInDegrees(pPtFIXBegin, pPtA1)

            pPt0 = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iTurnDir, 9200.0)

            bVa = SideDef(pPtTmp, fTmpDir0, pPt0) = iTurnDir

            If bVa Then
                pPtTmp = pPt0
                fTmpDir0 = ReturnAngleInDegrees(pPtTmp, pPtA1)
            End If
        Else
            pPtTmp = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iTurnDir, 9200.0)
            fTmpDir0 = ReturnAngleInDegrees(pPtTmp, pPtA1)
        End If

        pConstructor.ConstructAngleIntersection(IFPoint.pPtPrj, DegToRad(fFAFPDir + 90.0), pPtTmp, DegToRad(fTmpDir0))
        pPtC1 = PointAlongPlane(pPtA0, fFAFPDir + 180.0 - 15.0 * iTurnDir, fIF_FAF15)

        pIntermA_FullAreaPoly.AddPoint(pPtA0)
        pIntermA_FullAreaPoly.AddPoint(pPtA1)
        pIntermA_FullAreaPoly.AddPoint(pPtC0)
        pIntermA_FullAreaPoly.AddPoint(pPtC1)

        pTopoOper = pIntermA_FullAreaPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        'Initial Aproach Area ==============================================================================

        pInitialA_PrimAreaPoly = Nothing
        pInitialA_FullAreaPoly = Nothing
        pInitialA_PrimAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
        pInitialA_FullAreaPoly = New ESRI.ArcGIS.Geometry.Polygon

        pPtTmp = PointAlongPlane(pTPtPrj, m_fInitDir + 180.0, 38000.0)

        pPt0 = PointAlongPlane(pTPtPrj, m_fInitDir + 90.0, 4600.0)
        pPt1 = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0, 4600.0)

        pPt2 = PointAlongPlane(pPtTmp, m_fInitDir - 90.0, 4600.0)
        pPt3 = PointAlongPlane(pPtTmp, m_fInitDir + 90.0, 4600.0)

        pInitialA_PrimAreaPoly.AddPoint(pPt0)
        pInitialA_PrimAreaPoly.AddPoint(pPt1)
        pInitialA_PrimAreaPoly.AddPoint(pPt2)
        pInitialA_PrimAreaPoly.AddPoint(pPt3)

        pTopoOper = pInitialA_PrimAreaPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        'Initial Aproach Area ====================================================================
        pPt0 = PointAlongPlane(pTPtPrj, m_fInitDir + 90.0, 9200.0)
        pPt1 = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0, 9200.0)
        pPtInitBegin = PointAlongPlane(pTPtPrj, m_fInitDir + 180.0, 38000.0)

        pPt2 = PointAlongPlane(pPtInitBegin, m_fInitDir - 90.0, 9200.0)
        pPt3 = PointAlongPlane(pPtInitBegin, m_fInitDir + 90.0, 9200.0)

        pInitialA_FullAreaPoly.AddPoint(pPt0)
        pInitialA_FullAreaPoly.AddPoint(pPt1)
        pInitialA_FullAreaPoly.AddPoint(pPt2)
        pInitialA_FullAreaPoly.AddPoint(pPt3)

        pTopoOper = pInitialA_FullAreaPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        'DrawPolygon pInitialA_FullAreaPoly

        'Turn Area ======================================================================
        'pCutter = New ESRI.ArcGIS.Geometry.Polyline
        pTurnA_FullAreaPoly = Nothing
        pTurnA_SecAreaPoly = Nothing

        pTurnA_FullAreaPoly = New ESRI.ArcGIS.Geometry.Polygon
        pTurnA_SecAreaPoly = New ESRI.ArcGIS.Geometry.Polygon

        pTmpPoly0 = New ESRI.ArcGIS.Geometry.Polygon
        pTmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon

        pPt0 = PointAlongPlane(pPtFIXBegin, m_fInitDir + 90.0 * iTurnDir, 9200.0)
        pPt1 = PointAlongPlane(pPtFIXBegin, m_fInitDir - 90.0 * iTurnDir, 9200.0)

        If ComboBox003.SelectedIndex = 0 Then 'U
            fdL = System.Math.Sqrt(fCurrR * fCurrR - 9200.0 * 9200.0)

            pTmpPoly0.AddPoint(pPt0)
            pConstructor = pPt2
            pConstructor.ConstructAngleIntersection(pPt1, DegToRad(m_fInitDir), pPtA1, DegToRad(fTmpDir0))

            pTmpPoly0.AddPoint(pPtFIXBegin)
            pTmpPoly0.AddPoint(pPt1)

            pPt3 = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iTurnDir, 9200.0)
            pConstructor = pPtTmp
            pConstructor.ConstructAngleIntersection(IFPoint.pPtPrj, DegToRad(fFAFPDir + 90.0), pPt1, DegToRad(m_fInitDir))
            If ReturnDistanceInMeters(pPt2, pPt3) > ReturnDistanceInMeters(pPt2, pPtTmp) Then pPt3 = pPtTmp

            pTmpPoly0.AddPoint(pPt3)
            pTmpPoly0.AddPoint(IFPoint.pPtPrj)
            pTmpPoly0.AddPoint(pTPtPrj)

            pPt3 = PointAlongPlane(pPt0, m_fInitDir, fFIXWidth + fdL)
            pTmpPoly0.AddPoint(pPt3)

            pTopoOper = pTmpPoly0
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pPtY = New ESRI.ArcGIS.Geometry.Point
            pConstructor = pPtY
            pConstructor.ConstructAngleDistance(pTPtPrj, DegToRad(fInvDRDir - 90.0 * iTurnDir), fCurrR)

            If SideDef(pPt3, fInvDRDir - 90.0 * iTurnDir, pPtY) = iTurnDir Then
                pCircle = CreateArcPrj(pTPtPrj, pPt3, pPtY, -iTurnDir)
                pCircle.AddPoint(pTPtPrj)

                pTopoOper = pCircle
                pTopoOper.IsKnownSimple_2 = False
                pTopoOper.Simplify()

                pTmpPoly0 = pTopoOper.Union(pTmpPoly0)
            Else
                pPtY = pPt3
            End If

            pTmpPoly1.AddPoint(pPtFIXBegin)
            pTmpPoly1.AddPoint(pTPtPrj)
            pTmpPoly1.AddPoint(pPtY)

            pConstructor = pPt0
            pConstructor.ConstructAngleIntersection(pPtY, DegToRad(fDRDir + fCurrPhi * iTurnDir), pPtC1, DegToRad(fFAFPDir))

            pTmpPoly1.AddPoint(pPt0)
            pTmpPoly1.AddPoint(pPtC1)
            pTmpPoly1.AddPoint(pPtC0)

            pConstructor = pPtTmp
            pConstructor.ConstructAngleIntersection(pPtC0, DegToRad(fTmpDir0), pPt2, DegToRad(m_fInitDir)) 'DegToRad(fInvDRDir + 180.0 + fCurrPhi * iTurnDir), pPtC1, DegToRad(fFAFPDir)

            If SideDef(IFPoint.pPtPrj, fFAFPDir + 90.0, pPtTmp) < 0 Then
                pTmpPoly1.AddPoint(pPtTmp)
            End If

            pTopoOper = pTmpPoly1
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pTurnA_FullAreaPoly = pTopoOper.Union(pTmpPoly0)

            pTopoOper = pTurnA_FullAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
            ' Secondary ==============================================
            pPt0 = PointAlongPlane(pPtFIXBegin, m_fInitDir + 90.0 * iTurnDir, 92000.0)
            pPt1 = PointAlongPlane(pPtFIXBegin, m_fInitDir + 90.0 * iTurnDir, 4600.0)
            pPtTmp = PointAlongPlane(pTPtPrj, m_fInitDir, 100000.0)
            pPt2 = PointAlongPlane(pPtTmp, m_fInitDir + 90.0 * iTurnDir, 4600.0)
            pPt3 = PointAlongPlane(pPtTmp, m_fInitDir + 90.0 * iTurnDir, 92000.0)

            If pTmpPoly0.PointCount > 0 Then pTmpPoly0.RemovePoints(0, pTmpPoly0.PointCount)

            pTmpPoly0.AddPoint(pPt0)
            pTmpPoly0.AddPoint(pPt1)
            pTmpPoly0.AddPoint(pPt2)
            pTmpPoly0.AddPoint(pPt3)

            pTopoOper = pTmpPoly0
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pTurnA_SecAreaPoly = pTopoOper.Intersect(pTurnA_FullAreaPoly, esriGeometryDimension.esriGeometry2Dimension)

            pTopoOper = pTurnA_SecAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
        Else 'S
            pPt2 = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iTurnDir, 9200.0)

            pTurnA_FullAreaPoly.AddPoint(pPt1)
            pTurnA_FullAreaPoly.AddPoint(pPt2)
            pTurnA_FullAreaPoly.AddPoint(pPtC0)

            pTurnA_FullAreaPoly.AddPoint(pPtC1)

            pPt0 = PointAlongPlane(pPtFIXBegin, m_fInitDir + 90.0 * iTurnDir, 9200.0)
            pConstructor = pPtTmp
            pConstructor.ConstructAngleIntersection(pPtC1, DegToRad(fFAFPDir), pPt0, DegToRad(fInvDRDir + 22.0 * iTurnDir))
            pTurnA_FullAreaPoly.AddPoint(pPtTmp)

            pPt3 = PointAlongPlane(pPtFIXBegin, m_fInitDir + 90.0 * iTurnDir, 4600.0)
            pConstructor = pPt0
            pConstructor.ConstructAngleIntersection(pPt3, DegToRad(m_fInitDir), pPtTmp, DegToRad(fInvDRDir + 22.0 * iTurnDir))
            pTurnA_FullAreaPoly.AddPoint(pPt0)

            pConstructor = pPt2
            pConstructor.ConstructAngleIntersection(pPtFIXBegin, DegToRad(m_fInitDir), pPt0, DegToRad(m_fInitDir + 90.0))

            pTurnA_FullAreaPoly.AddPoint(pPt2)
            pTurnA_FullAreaPoly.AddPoint(pPtFIXBegin)

            pTopoOper = pTurnA_FullAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
            '==============================================
            fdL = Point2LineDistancePrj(pPt1, IFPoint.pPtPrj, m_fInitDir + 90.0)
            pPt2 = PointAlongPlane(pPtFIXBegin, m_fInitDir - 90.0 * iTurnDir, 4600.0)

            pTmpPoly0.AddPoint(pPt1)
            pTmpPoly0.AddPoint(PointAlongPlane(pPt1, m_fInitDir, 2 * fdL))
            pTmpPoly0.AddPoint(PointAlongPlane(pPt2, m_fInitDir, 2 * fdL))
            pTmpPoly0.AddPoint(pPt2)

            pTopoOper = pTurnA_FullAreaPoly
            pTmpPoly1 = pTopoOper.Union(pIntermA_FullAreaPoly)

            pTopoOper = pTmpPoly1
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pTopoOper = pTmpPoly0
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pTurnA_SecAreaPoly = pTopoOper.Intersect(pTmpPoly1, esriGeometryDimension.esriGeometry2Dimension)

            pTopoOper = pTurnA_SecAreaPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
            '===========================================
        End If

        pInitialA_FullAreaElement = DrawPolygon(pInitialA_FullAreaPoly, RGB(0, 255, 0))
        pInitialA_FullAreaElement.Locked = True

        pInitialA_PrimAreaElement = DrawPolygon(pInitialA_PrimAreaPoly, RGB(255, 0, 0))
        pInitialA_PrimAreaElement.Locked = True

        pIntermAA_FullAreaElement = DrawPolygon(pIntermA_FullAreaPoly, RGB(255, 0, 0))
        pIntermAA_FullAreaElement.Locked = True

        pTurnA_FullAreaElement = DrawPolygon(pTurnA_FullAreaPoly, RGB(0, 0, 255))
        pTurnA_FullAreaElement.Locked = True

        pTurnA_SecAreaElement = DrawPolygon(pTurnA_SecAreaPoly, RGB(127, 255, 127))
        pTurnA_SecAreaElement.Locked = True

        pTP_FIXAreaElement = DrawPolygon(pTP_FIXAreaPoly, RGB(255, 127, 0))
        pTP_FIXAreaElement.Locked = True

        pIF_FIXAreaElement = DrawPolygon(pIF_FIXAreaPoly, RGB(255, 127, 0))
        pIF_FIXAreaElement.Locked = True

        pIFptElement = DrawPointWithText(IFPoint.pPtPrj, "IF", WPTColor)
        pIFptElement.Locked = True

        pTptElement = DrawPointWithText(pTPtPrj, "TP", WPTColor)
        pTptElement.Locked = True

        'Application.DoEvents()

        ''Obstacles =====================================================================================
        'Dim I As Integer
        'Dim J As Integer
        'Dim M As Integer
        'Dim N As Integer
        'Dim Side As Integer
        ''Dim IxInReqH As Integer
        ''Dim IxTrnReqH As Integer
        'Dim pCurrPt As IPoint

        'Dim pTurnASecRelation As IRelationalOperator
        'Dim pInitialAPrimRelation As IRelationalOperator

        'Dim pCalcPoly As IPolyline
        'Dim pSecAreaPoly As IPolyline
        'Dim pResPoly As IPolyline

        'pInitialAPrimRelation = pInitialA_PrimAreaPoly

        ''FillSegObstList()

        ''GetArObstaclesByPoly(InitialAAObstList, pInitialA_FullAreaPoly, 0)
        ''N = CreateObstacleParts(InitialAAObstList, pInitialA_PrimAreaPoly, pInitialA_FullAreaPoly)
        'N = FillInitialObstList(InitialAAObstList, pInitialA_PrimAreaPoly, pInitialA_FullAreaPoly)

        'For I = 0 To N
        '    pCurrPt = InitialAAObstList.Parts(I).pPtPrj
        '    fdL = Point2LineDistancePrj(pCurrPt, pPtFIXBegin, m_fInitDir)

        '    If Not pInitialAPrimRelation.Contains(pCurrPt) Then
        '        InitialAAObstList.Parts(I).fTmp = (9200.0 - fdL) / 4600.0
        '        InitialAAObstList.Parts(I).Flags = 0
        '    Else
        '        InitialAAObstList.Parts(I).fTmp = 1.0
        '        InitialAAObstList.Parts(I).Flags = 1
        '    End If
        '    InitialAAObstList.Parts(I).MOC = 300.0 * InitialAAObstList.Parts(I).fTmp
        '    InitialAAObstList.Parts(I).ReqH = InitialAAObstList.Parts(I).Height + InitialAAObstList.Parts(I).MOC
        'Next I

        'pTurnASecRelation = pTurnA_SecAreaPoly
        'pCalcPoly = New ESRI.ArcGIS.Geometry.Polyline
        'pTopoOper = pTurnASecRelation

        ''GetArObstaclesByPoly(TurnAObstList, pTurnA_FullAreaPoly, 0)
        ''N = CreateObstacleParts(TurnAObstList, pTurnA_SecAreaPoly, pTurnA_FullAreaPoly)
        'N = FillInitialObstList(TurnAObstList, pTurnA_SecAreaPoly, pTurnA_FullAreaPoly)

        'MaxTurnReqH = NO_DATA_VALUE
        ''IxTrnReqH = -1
        'For I = 0 To N
        '    pCurrPt = TurnAObstList.Parts(I).pPtPrj
        '    fdL = Point2LineDistancePrj(pCurrPt, pPtFIXBegin, m_fInitDir)
        '    Side = SideDef(pPtFIXBegin, m_fInitDir, pCurrPt)

        '    If pTurnASecRelation.Contains(pCurrPt) Then
        '        pCalcPoly.FromPoint = pCurrPt
        '        pCalcPoly.ToPoint = PointAlongPlane(pCurrPt, m_fInitDir + 90.0 * Side, fdL)

        '        pResPoly = pTopoOper.Intersect(pCalcPoly, esriGeometryDimension.esriGeometry1Dimension)
        '        pCalcPoly.FromPoint = PointAlongPlane(pCurrPt, m_fInitDir - 90.0 * Side, 100000.0)

        '        pSecAreaPoly = pTopoOper.Intersect(pCalcPoly, esriGeometryDimension.esriGeometry1Dimension)

        '        TurnAObstList.Parts(I).fTmp = (pSecAreaPoly.Length - pResPoly.Length) / pSecAreaPoly.Length
        '        TurnAObstList.Parts(I).Flags = 0
        '    Else
        '        TurnAObstList.Parts(I).fTmp = 1.0
        '        TurnAObstList.Parts(I).Flags = 1
        '    End If

        '    TurnAObstList.Parts(I).MOC = 300.0 * TurnAObstList.Parts(I).fTmp
        '    TurnAObstList.Parts(I).ReqH = TurnAObstList.Parts(I).Height + TurnAObstList.Parts(I).MOC

        '    If TurnAObstList.Parts(I).ReqH > MaxTurnReqH Then
        '        MaxTurnReqH = TurnAObstList.Parts(I).ReqH
        '        'IxTrnReqH = I
        '    End If
        'Next I

        ''GetArObstaclesByPoly(IntermAAObstList, pIntermA_FullAreaPoly, 0)
        ''N = CreateObstacleParts(IntermAAObstList, pIntermA_FullAreaPoly)
        'N = FillInitialObstList(IntermAAObstList, pIntermA_FullAreaPoly)

        'MaxIntermReqH = NO_DATA_VALUE
        'For I = 0 To N
        '    IntermAAObstList.Parts(I).fTmp = 1.0
        '    IntermAAObstList.Parts(I).MOC = 150.0
        '    IntermAAObstList.Parts(I).ReqH = IntermAAObstList.Parts(I).Height + IntermAAObstList.Parts(I).MOC

        '    If IntermAAObstList.Parts(I).ReqH > MaxIntermReqH Then
        '        MaxIntermReqH = IntermAAObstList.Parts(I).ReqH
        '    End If
        '    IntermAAObstList.Parts(I).Flags = 1
        'Next I

        'OkBtn.Enabled = (MaxIntermReqH <= pFAFPptPrj.Z) And (MaxTurnReqH < IFAltitude)

        'DeadApproachReport.FillPage1(IntermAAObstList, pFAFPptPrj.Z)
        'DeadApproachReport.FillPage2(TurnAObstList, IFAltitude)
        'DeadApproachReport.FillPage3(InitialAAObstList)
        'DrawPolyLine(pNomTrack, 0, 3)
        'Application.DoEvents()
    End Sub

    Private Sub CalculateObstacles()
        'Obstacles =====================================================================================
        Dim fdL As Double
        Dim I As Integer
        'Dim J As Integer
        'Dim M As Integer
        Dim N As Integer
        Dim Side As Integer
        'Dim IxInReqH As Integer
        'Dim IxTrnReqH As Integer
        Dim pCurrPt As IPoint

        Dim pTurnASecRelation As IRelationalOperator
        Dim pInitialAPrimRelation As IRelationalOperator

        Dim pCalcPoly As IPolyline
        Dim pSecAreaPoly As IPolyline
        Dim pResPoly As IPolyline

        Dim pTopoOper As ITopologicalOperator3

        pInitialAPrimRelation = pInitialA_PrimAreaPoly

        'FillSegObstList()

        'GetArObstaclesByPoly(InitialAAObstList, pInitialA_FullAreaPoly, 0)
        'N = CreateObstacleParts(InitialAAObstList, pInitialA_PrimAreaPoly, pInitialA_FullAreaPoly)
        N = FillInitialObstList(InitialAAObstList, pInitialA_PrimAreaPoly, pInitialA_FullAreaPoly)

        For I = 0 To N
            pCurrPt = InitialAAObstList.Parts(I).pPtPrj
            fdL = Point2LineDistancePrj(pCurrPt, pPtFIXBegin, m_fInitDir)

            If Not pInitialAPrimRelation.Contains(pCurrPt) Then
                InitialAAObstList.Parts(I).fTmp = (9200.0 - fdL) / 4600.0
                InitialAAObstList.Parts(I).Flags = 0
            Else
                InitialAAObstList.Parts(I).fTmp = 1.0
                InitialAAObstList.Parts(I).Flags = 1
            End If
            InitialAAObstList.Parts(I).MOC = 300.0 * InitialAAObstList.Parts(I).fTmp
            InitialAAObstList.Parts(I).ReqH = InitialAAObstList.Parts(I).Height + InitialAAObstList.Parts(I).MOC
        Next I

        pTurnASecRelation = pTurnA_SecAreaPoly
        pCalcPoly = New ESRI.ArcGIS.Geometry.Polyline
        pTopoOper = pTurnASecRelation

        'GetArObstaclesByPoly(TurnAObstList, pTurnA_FullAreaPoly, 0)
        'N = CreateObstacleParts(TurnAObstList, pTurnA_SecAreaPoly, pTurnA_FullAreaPoly)
        N = FillInitialObstList(TurnAObstList, pTurnA_SecAreaPoly, pTurnA_FullAreaPoly)

        MaxTurnReqH = NO_DATA_VALUE
        'IxTrnReqH = -1
        For I = 0 To N
            pCurrPt = TurnAObstList.Parts(I).pPtPrj
            fdL = Point2LineDistancePrj(pCurrPt, pPtFIXBegin, m_fInitDir)
            Side = SideDef(pPtFIXBegin, m_fInitDir, pCurrPt)

            If pTurnASecRelation.Contains(pCurrPt) Then
                pCalcPoly.FromPoint = pCurrPt
                pCalcPoly.ToPoint = PointAlongPlane(pCurrPt, m_fInitDir + 90.0 * Side, fdL)

                pResPoly = pTopoOper.Intersect(pCalcPoly, esriGeometryDimension.esriGeometry1Dimension)
                pCalcPoly.FromPoint = PointAlongPlane(pCurrPt, m_fInitDir - 90.0 * Side, 100000.0)

                pSecAreaPoly = pTopoOper.Intersect(pCalcPoly, esriGeometryDimension.esriGeometry1Dimension)

                TurnAObstList.Parts(I).fTmp = (pSecAreaPoly.Length - pResPoly.Length) / pSecAreaPoly.Length
                TurnAObstList.Parts(I).Flags = 0
            Else
                TurnAObstList.Parts(I).fTmp = 1.0
                TurnAObstList.Parts(I).Flags = 1
            End If

            TurnAObstList.Parts(I).MOC = 300.0 * TurnAObstList.Parts(I).fTmp
            TurnAObstList.Parts(I).ReqH = TurnAObstList.Parts(I).Height + TurnAObstList.Parts(I).MOC

            If TurnAObstList.Parts(I).ReqH > MaxTurnReqH Then
                MaxTurnReqH = TurnAObstList.Parts(I).ReqH
                'IxTrnReqH = I
            End If
        Next I

        'GetArObstaclesByPoly(IntermAAObstList, pIntermA_FullAreaPoly, 0)
        'N = CreateObstacleParts(IntermAAObstList, pIntermA_FullAreaPoly)
        N = FillInitialObstList(IntermAAObstList, pIntermA_FullAreaPoly)

        MaxIntermReqH = NO_DATA_VALUE
        For I = 0 To N
            IntermAAObstList.Parts(I).fTmp = 1.0
            IntermAAObstList.Parts(I).MOC = 150.0
            IntermAAObstList.Parts(I).ReqH = IntermAAObstList.Parts(I).Height + IntermAAObstList.Parts(I).MOC

            If IntermAAObstList.Parts(I).ReqH > MaxIntermReqH Then
                MaxIntermReqH = IntermAAObstList.Parts(I).ReqH
            End If
            IntermAAObstList.Parts(I).Flags = 1
        Next I

        OkBtn.Enabled = (MaxIntermReqH <= pFAFPptPrj.Z) And (MaxTurnReqH < IFAltitude)

        DeadApproachReport.FillPage1(IntermAAObstList, pFAFPptPrj.Z)
        DeadApproachReport.FillPage2(TurnAObstList, IFAltitude)
        DeadApproachReport.FillPage3(InitialAAObstList)

        nomTrackIsChanged = False
    End Sub

    Private Sub CheckBox001_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox001.CheckedChanged
        Label033.Enabled = Not CheckBox001.Checked
        Label034.Enabled = Not CheckBox001.Checked
        Label035.Enabled = Not CheckBox001.Checked
        Label036.Enabled = Not CheckBox001.Checked

        TextBox011.Enabled = Not CheckBox001.Checked
        TextBox016.Enabled = Not CheckBox001.Checked
    End Sub

    Private Sub CheckBox101_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox101.CheckedChanged
        Dim fTmp As Double
        If MultiPage1.SelectedIndex = 0 Then Return

        ComboBox108.Enabled = CheckBox101.Checked

        If ComboBox108.Enabled Then
            'TextBox109.ReadOnly = True
            If ComboBox108.SelectedIndex < 0 Then
                ComboBox108.SelectedIndex = 0
            Else
                ComboBox108_SelectedIndexChanged(ComboBox108, New EventArgs())
            End If
        ElseIf Not CheckBox101.Checked Then
            TextBox109.ReadOnly = (TPIntersectNavs(ComboBox107.SelectedIndex).ValCnt < 0) Or (Not OptionButton103.Checked)
            fDRInterceptAngle = fTxtInterceptAngle

            TextBox110.Text = CStr(Math.Round(fDRInterceptAngle, 2))
            fDRDir = fFAFPDir + fDRInterceptAngle * iTurnDir
            IFPoint.pPtPrj.M = fDRDir

            fTmp = Dir2Azt(pTPtPrj, fDRDir) - CurrADHP.MagVar
            TextBox111.Text = CStr(Math.Round(fTmp, 2))

            'ComboBox104_Click
            'TextBox106_Validate False
        End If

        If TextBox109.ReadOnly Then
            TextBox109.BackColor = SystemColors.Control
        Else
            TextBox109.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub ComboBox002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox002.SelectedIndexChanged
        'Dim I As Integer
        'Dim J As Integer
        'Dim K As Integer

        'Dim L As Integer
        'Dim M As Integer
        'Dim N As Integer
        'Dim P As Integer

        'Dim pIAP As InstrumentApproachProcedure
        'Dim pIAPList As List(Of InstrumentApproachProcedure)

        'Dim pProcedureLeg As SegmentLeg
        ''Dim pProcedureLegList               As SegmentLegComList

        'Dim pProcedureTransition As ProcedureTransition
        ''Dim pProcedureTransitionList    As ProcedureTransitionComList
        'Dim Break As Boolean
        'If Not bFormInitialised Then Return
        'K = ComboBox002.SelectedIndex
        'If K < 0 Then Return
        'CurrADHP = LocalADHPList(K)

        'ComboBox001.Items.Clear()

        'pIAPList = pObjectDir.GetIAPList(CurrADHP.pAirportHeliport.Identifier)

        'N = pIAPList.Count
        'ReDim IAPArray(N - 1)
        'K = -1

        'For I = 0 To N - 1
        '	pIAP = pIAPList.Item(I)
        '	'Set pProcedureLegList = GetSegmentLegListByProcedure(pIAP)

        '	Break = False

        '	M = pIAP.FlightTransition.Count
        '	For J = 0 To M - 1
        '		pProcedureTransition = pIAP.FlightTransition.Item(J)

        '		L = pProcedureTransition.TransitionLeg.Count
        '		If L > 0 Then
        '			For P = 0 To L - 1
        '				pProcedureLeg = pProcedureTransition.TransitionLeg.Item(P).TheSegmentLeg.GetFeature()
        '				If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
        '					K = K + 1
        '					IAPArray(K) = pIAP
        '					ComboBox001.Items.Add(pIAP.Name)
        '					Break = True
        '					Exit For
        '				End If
        '			Next P

        '			If Break Then Exit For
        '		End If
        '	Next J
        'Next I

        'If ComboBox001.Items.Count > 0 Then
        '	ComboBox001.SelectedIndex = 0
        'End If
    End Sub

    Private Sub ComboBox001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox001.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer

        Dim Identifier As Guid
        Dim IFIdentifier As Guid
        Dim bFound As Boolean

        Dim pIAP As InstrumentApproachProcedure
        Dim pProcedureLeg As SegmentLeg
        Dim pProcedureTransition As ProcedureTransition

        Label005.Text = ""
        TextBox004.Text = ""
        TextBox014.Text = ""

        K = ComboBox001.SelectedIndex
        If K < 0 Then Return

        ProcedureName = ComboBox001.Text

        'FillNavaidList(NavaidList, DMEList, CurrADHP, MaxNAVDist)
        'FillWPT_FIXList(WPTList, CurrADHP, MaxNAVDist)

        pIAP = IAPArray(K)

        pLandingTakeoff = pIAP.Landing
        iCategory = pIAP.AircraftCharacteristic.Item(0).AircraftLandingCategory.Value
        TextBox015.Text = Chr(iCategory + Asc("A"))

        '===========================

        N = pIAP.FlightTransition.Count

        bFound = False

        For I = 0 To N - 1
            pProcedureTransition = pIAP.FlightTransition.Item(I)
            IFIdentifier = Guid.Empty

            For J = 0 To pProcedureTransition.TransitionLeg.Count - 1
                pProcedureLeg = pProcedureTransition.TransitionLeg.Item(J).TheSegmentLeg.GetFeature2()

                If pProcedureLeg.StartPoint Is Nothing Then Continue For
                If pProcedureLeg.EndPoint Is Nothing Then Continue For

                If Not pProcedureTransition.Type Is Nothing AndAlso pProcedureTransition.Type = CodeProcedurePhase.FINAL Then
                    If (J = 0) AndAlso pProcedureLeg.StartPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.IF Then
                        IFIdentifier = pProcedureLeg.StartPoint.PointChoice.GetFeatureRef().Identifier
                        IFAltitudeInBase = ConverterToSI.Convert(pProcedureLeg.UpperLimitAltitude, 0.0)
                    End If
                End If

                If pProcedureLeg.EndPoint.Role.Value = Aran.Aim.Enums.CodeProcedureFixRole.FAF Then
                    Identifier = pProcedureLeg.EndPoint.PointChoice.GetFeatureRef().Identifier
                    'K = J
                    bFound = True
                    Exit For
                End If
            Next J
            If bFound Then Exit For
        Next I

        If Not bFound Then
            MessageBox.Show("Invalid 'FINAL' Transition")
            Return
        End If

        bFound = False

        N = UBound(WPTList)
        For I = 0 To N
            If WPTList(I).NAV_Ident = Identifier Then
                StartPoint = WPTList(I)
                bFound = True
                Exit For
            End If
        Next I

        If Not bFound Then
            MessageBox.Show("Invalid FAF point")
            Return
        End If

        'IF Point =========================================================

        bIFFound = False

        If IFIdentifier <> Guid.Empty Then
            For I = 0 To N
                If WPTList(I).NAV_Ident = IFIdentifier Then
                    IFPoint = WPTList(I)
                    bIFFound = True
                    Exit For
                End If
            Next I
        Else

        End If

        '==================================================================================================
        Dim Course As Double
        Dim ILS As NavaidData
        Dim uomDistVerNameTab() As String

        'uomDistHorzNameTab = New String() {"NM", "KM", "M", "FT", "MI", "OTHER"}
        uomDistVerNameTab = New String() {"FT", "M", "FL", "SM", "OTHER"}

        CheckBox001.Enabled = bIFFound

        If Not bIFFound Then
            CheckBox001.Checked = False
            '    PrAlt = ReturnDistanceInMeters(IFPoint.pPtPrj, StartPoint.pPtPrj)
            '    DrawPointWithText IFPoint.pPtPrj, "IF: " + CStr(ConvertDistance(PrAlt, eRoundMode.rmNERAEST))
        End If

        TextBox014.Text = "FAF"

        Course = pProcedureLeg.Course.Value
        TextBox001.Text = CStr(System.Math.Round(Course))

        Dim pAngleIndication As AngleIndication

        If Not (pProcedureLeg.Angle Is Nothing) Then
            pAngleIndication = pProcedureLeg.Angle.GetFeature2(FeatureType.AngleIndication)
        Else
            bFound = False
            For I = 0 To pProcedureLeg.StartPoint.FacilityMakeup.Count - 1
                For J = 0 To pProcedureLeg.StartPoint.FacilityMakeup.Item(I).FacilityAngle.Count - 1
                    If pProcedureLeg.StartPoint.FacilityMakeup.Item(I).FacilityAngle.Item(J).AlongCourseGuidance Then
                        pAngleIndication = pProcedureLeg.StartPoint.FacilityMakeup.Item(I).FacilityAngle.Item(J).TheAngleIndication.GetFeature2(FeatureType.AngleIndication)
                        bFound = True
                        Exit For
                    End If
                Next
                If bFound Then Exit For
            Next
        End If

        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Dim pNavaid As Navaid
        Dim pNavaidEquipment As NavaidEquipment
        Dim ILSAdded As Boolean
        Dim PrAlt As Double

        bFound = False
        ILSAdded = False

        If Not (pAngleIndication Is Nothing) Then
            If pAngleIndication.PointChoice.Choice = SignificantPointChoice.Navaid Then
                pNavaid = pAngleIndication.PointChoice.GetFeatureRef().GetFeature2(FeatureType.Navaid)

                If pNavaid Is Nothing Then
                    MessageBox.Show("Invalid guidance navaid")
                    Return
                End If

                For J = 0 To pNavaid.NavaidEquipment.Count - 1
                    pNavaidEquipment = pNavaid.NavaidEquipment(J).TheNavaidEquipment.GetFeature2()

                    If (TypeOf pNavaidEquipment Is Aran.Aim.Features.VOR) Or (TypeOf pNavaidEquipment Is Aran.Aim.Features.NDB) Or (TypeOf pNavaidEquipment Is Localizer) Then
                        If (TypeOf pNavaidEquipment Is Localizer) Then
                            TextBox014.Text = "FAP"

                            If Not ILSAdded Then
                                If GetILSByNavEq(pNavaid, ILS) >= 1 Then
                                    'If GetILSByName(ProcedureName, CurrADHP, ILS) >= 1 Then
                                    AddILSToNavList(ILS, NavaidList)
                                    ILSAdded = True
                                End If
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
                        'N = UBound(DMEList)
                        'For I = 0 To N
                        '	If DMEList(I).pFeature.Identifier = pNavaid.Identifier Then
                        '		StartPoint.GuidanceNav = DMEList(I)
                        '		bFound = True
                        '		Exit For
                        '	End If
                        'Next I
                    ElseIf (TypeOf pNavaidEquipment Is Aran.Aim.Features.TACAN) Then
                        ' ...
                    End If

                    If bFound Then Exit For
                Next J
            End If
        End If

        If Not bFound Then
            MessageBox.Show("Invalid guidance navaid")
            Return
        End If

        Label005.Text = GetNavTypeName(StartPoint.GuidanceNav.TypeCode)
        TextBox004.Text = StartPoint.GuidanceNav.CallSign

        FAPAltitude = ConverterToSI.Convert(pProcedureLeg.UpperLimitAltitude, 0.0)
        TextBox008MinVal = ConverterToSI.Convert(pProcedureLeg.UpperLimitAltitude, 0.0)
        TextBox002.Text = CStr(ConvertHeight(FAPAltitude, eRoundMode.NEAREST))
        TextBox101_Interval.Low = FAPAltitude
        TextBox101_Interval.High = FAPAltitude

        StartPoint.pPtGeo.Z = FAPAltitude
        StartPoint.pPtPrj.Z = FAPAltitude

        StartPoint.pPtGeo.M = Course
        StartPoint.pPtPrj.M = Azt2Dir(StartPoint.pPtGeo, Course)

        pFAFPptPrj = StartPoint.pPtPrj
        fFAFPDir = pFAFPptPrj.M

        PrAlt = arDRNomAltitude.Value
        If PrAlt < TextBox008MinVal Then PrAlt = TextBox008MinVal

        TextBox008.Text = CStr(ConvertHeight(PrAlt, eRoundMode.NEAREST))
        TextBox007.Text = CStr(ConvertSpeed(cViafMax.Values(iCategory), eRoundMode.SPECIAL))

        ToolTip1.SetToolTip(TextBox007, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertSpeed(cViafMin.Values(iCategory), eRoundMode.SPECIAL)) + " " + SpeedConverter(SpeedUnit).Unit + My.Resources.str00222 + TextBox007.Text + " " + SpeedConverter(SpeedUnit).Unit)

        TextBox007_Validating(TextBox007, New System.ComponentModel.CancelEventArgs())

        SDFIX0.GuidanceNav = StartPoint.GuidanceNav
        SDFIX1.GuidanceNav = StartPoint.GuidanceNav
        m_pGuidPoly = CreateGuidPoly(SDFIX1.GuidanceNav, pFAFPptPrj)

        On Error Resume Next
        If Not SDFIX1.ptElem Is Nothing Then pGraphics.DeleteElement(SDFIX1.ptElem)
        On Error GoTo 0

        SDFIX1.ptElem = DrawPoint(pFAFPptPrj, RGB(255, 0, 255))
        SDFIX1.ptElem.Locked = True
        NextBtn.Enabled = True
    End Sub

    Private Sub ComboBox003_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox003.SelectedIndexChanged
        If Not bFormInitialised Then Return

        iDRType = iTurnDir * (1 - 2 * ComboBox003.SelectedIndex)
        If ComboBox003.SelectedIndex = 0 Then
            TextBox105.Text = "120"
        Else
            TextBox105.Text = "50"
        End If
    End Sub

    Private Sub ComboBox004_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox004.SelectedIndexChanged
        If Not bFormInitialised Then Return

        iTurnDir = 2 * ComboBox004.SelectedIndex - 1
        If ComboBox003.SelectedIndex < 0 Then
            ComboBox003.SelectedIndex = 0
        Else
            ComboBox003_SelectedIndexChanged(ComboBox003, New System.EventArgs())
        End If
    End Sub

    Private Sub ComboBox103_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox103.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer
        Dim fTmp0 As Double
        Dim fTmp1 As Double
        Dim fTmp2 As Double
        Dim fTmp3 As Double

        If ComboBox103.SelectedIndex < 0 Then Return
        K = ComboBox103.SelectedIndex
        Label111.Text = GetNavTypeName(GuidanceNavs(K).TypeCode)

        If GuidanceNavs(K).ValCnt = 3 Then
            OptionButton104.Enabled = True
            OptionButton105.Enabled = True
            fTmp0 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(0))
            fTmp1 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(0))

            fTmp2 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(1))
            fTmp3 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(1))

            Label114.Text = My.Resources.str50232 + My.Resources.str00221 + CStr(System.Math.Round(fTmp0 + 0.4999)) + My.Resources.str00222 + CStr(System.Math.Round(fTmp1 - 0.4999)) + vbCrLf +
            My.Resources.str00221 + CStr(System.Math.Round(fTmp2 + 0.4999)) + My.Resources.str00222 + CStr(System.Math.Round(fTmp3 - 0.4999))

            If OptionButton104.Checked Then
                TextBox106.Text = CStr(System.Math.Round(fTmp0 + 0.4999))
            Else
                TextBox106.Text = CStr(System.Math.Round(fTmp2 + 0.4999))
            End If
        ElseIf GuidanceNavs(K).ValCnt And 1 Then
            OptionButton105.Enabled = True
            OptionButton105.Checked = True
            OptionButton104.Enabled = False

            fTmp0 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(0))
            fTmp1 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(0))

            Label114.Text = My.Resources.str50232 + My.Resources.str00221 + CStr(System.Math.Round(fTmp0 + 0.4999)) + My.Resources.str00222 + CStr(System.Math.Round(fTmp1 - 0.4999))
            TextBox106.Text = CStr(System.Math.Round(fTmp0 + 0.4999))
        ElseIf GuidanceNavs(K).ValCnt And 2 Then
            OptionButton105.Enabled = False
            OptionButton104.Enabled = True
            OptionButton104.Checked = True
            fTmp0 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(1))
            fTmp1 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(1))

            Label114.Text = My.Resources.str50232 + My.Resources.str00221 + CStr(System.Math.Round(fTmp0 + 0.4999)) + My.Resources.str00222 + CStr(System.Math.Round(fTmp1 - 0.4999))
            TextBox106.Text = CStr(System.Math.Round(fTmp0 + 0.4999))
        End If
        FillComboBox104()
        TextBox106_Validating(TextBox106, New System.ComponentModel.CancelEventArgs())
    End Sub

    Private Sub ComboBox104_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox104.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer
        Dim L As Integer
        Dim fInitAzt As Double
        Dim fTmp0 As Double
        Dim fTmp1 As Double

        Label122.Text = ""
        K = ComboBox104.SelectedIndex
        If K < 0 Then Return
        Label122.Text = GetNavTypeName(ComboBox104List(K).TypeCode)

        If OptionButton106.Checked Then Return
        L = ComboBox103.SelectedIndex

        m_fInitDir = ReturnAngleInDegrees(GuidanceNavs(L).pPtPrj, ComboBox104List(K).pPtPrj)
        fTmp0 = GuidanceNavs(L).ValMin(1 + CShort(OptionButton105.Checked))
        fTmp1 = GuidanceNavs(L).ValMax(1 + CShort(OptionButton105.Checked))

        If Not AngleInSector(m_fInitDir, fTmp0, fTmp1) Then m_fInitDir = m_fInitDir + 180.0
        fInitAzt = Dir2Azt(GuidanceNavs(L).pPtPrj, m_fInitDir)

        TextBox106.Text = CStr(System.Math.Round(fInitAzt, 2))
        InitialCourseChanged(m_fInitDir)
    End Sub

    Private Sub ComboBox105_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox105.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer
        Dim Side As Integer

        Dim fD As Double
        Dim fdL As Double
        Dim fDist As Double
        Dim fInvDRDir As Double

        Dim pPt0 As IPoint
        Dim pPt2 As IPoint
        Dim pPtCntr As IPoint

        On Error Resume Next
        If Not pIF_FIXAreaElement Is Nothing Then pGraphics.DeleteElement(pIF_FIXAreaElement)
        On Error GoTo 0

        K = ComboBox105.SelectedIndex
        If K < 0 Then Return
        Label124.Text = GetNavTypeName(IFIntersectNavs(K).TypeCode)

        TextBox107.ReadOnly = (IFIntersectNavs(K).ValCnt < 0) Or (Not OptionButton101.Checked)
        If TextBox107.ReadOnly Then
            TextBox107.BackColor = System.Drawing.SystemColors.Control
        Else
            TextBox107.BackColor = System.Drawing.SystemColors.Window
        End If

        If IFIntersectNavs(K).ValCnt < 0 Then
            Label124.Text = Label124.Text + vbCrLf + My.Resources.str00106 '"Над средством"
            fDist = Point2LineDistancePrj(IFIntersectNavs(K).pPtPrj, pFAFPptPrj, fFAFPDir + 90.0)
            TextBox107.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))
            fIF_FAFPDist = fDist

            fD = fDistInterFAFP - fInterSign * fIF_FAFPDist
            If ComboBox003.SelectedIndex = 0 Then
                fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fBetta)))
            Else
                fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fInterSign * fBetta)))
            End If
            fDR = fD * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fInterSign * fBetta)) - fdL
            TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

            TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
            TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
            '====================================================================================================
            fInvDRDir = fDRDir + 180.0
            pPt2 = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPDist)

            pPt0 = PointAlongPlane(pPt2, fInvDRDir, fDR)

            pPtCntr = PointAlongPlane(pPt0, fInvDRDir + 90.0 * iDRType, fTurnR)
            pTPtPrj = PointAlongPlane(pPtCntr, m_fInitDir + 90.0 * iDRType, fTurnR)
            pTPtPrj.Z = TurnPtAltitude

            fTPFcDist = ReturnDistanceInMeters(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, pTPtPrj)
            TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

            '        Side = SideDef(GuidanceNavs(ComboBox103.ListIndex).pPtGeoPrj, fInitDir + 90.0 * iDRType, pTPtPrj)
            Side = SideDef(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, m_fInitDir - 90.0, pTPtPrj)

            If Side = 0 Then Side = 1
            ComboBox106.SelectedIndex = 1 - System.Math.Round(0.5 * (Side + 1))
            GetValidTPIntersectFacilities()
            '====================================================================================================
            CreateNomTrack(False)
        End If

        Dim IntersectNav As NavaidData

        IntersectNav = IFIntersectNavs(K)
        pIF_FIXAreaPoly = CreateFixZone(IFPoint.pPtPrj, SDFIX1.GuidanceNav, IntersectNav, IFIntersectNavs(K).ValCnt < 0, m_pGuidPoly, fFAFPDir, 1600.0)
        pIF_FIXAreaElement = DrawPolygon(pIF_FIXAreaPoly, RGB(255, 255, 0))
        pIF_FIXAreaElement.Locked = True
    End Sub

    Private Sub ComboBox106_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox106.SelectedIndexChanged
        If Not bFormInitialised Then Return

        If OptionButton103.Checked Then TextBox109_Validating(TextBox109, New System.ComponentModel.CancelEventArgs())
    End Sub

    Private Sub ComboBox107_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox107.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer

        Dim fDir As Double
        Dim fDist As Double
        Dim fInvDRDir As Double

        Dim pPt0 As IPoint

        Dim pPtCntr As IPoint
        Dim pConstruct As IConstructPoint
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone

        K = ComboBox107.SelectedIndex
        If K < 0 Then Return
        Label126.Text = GetNavTypeName(TPIntersectNavs(K).TypeCode)

        'If CheckBox001.Checked Then Return

        On Error Resume Next
        If Not pTP_FIXAreaElement Is Nothing Then pGraphics.DeleteElement(pTP_FIXAreaElement)
        On Error GoTo 0

        TextBox109.ReadOnly = CheckBox101.Checked Or (TPIntersectNavs(K).ValCnt < 0) Or (Not OptionButton103.Checked)

        If TextBox109.ReadOnly Then
            TextBox109.BackColor = System.Drawing.SystemColors.Control
        Else
            TextBox109.BackColor = System.Drawing.SystemColors.Window
        End If

        LockControl(ComboBox106, ComboBox106Locked Or TextBox109.ReadOnly)

        If TPIntersectNavs(K).IntersectionType = eIntersectionType.OnNavaid Then
            Label126.Text = Label126.Text + vbCrLf + My.Resources.str00106  '"Над средством"
            fDist = 0.0 'Point2LineDistancePrj(TPIntersectNavs(K).pPtPrj, pFAFPptPrj, fFAFPDir + 90.0)
            TextBox109.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))

            fTPFcDist = fDist
            fInvDRDir = fDRDir + 180.0

            pClone = TPIntersectNavs(K).pPtPrj
            pTPtPrj = pClone.Clone
            pTPtPrj.Z = TurnPtAltitude

            fTPFcDist = ReturnDistanceInMeters(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, pTPtPrj)
            TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

            pPtCntr = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iDRType, fTurnR)
            pPt0 = PointAlongPlane(pPtCntr, fInvDRDir - 90.0 * iDRType, fTurnR)

            'pPtIF = New ESRI.ArcGIS.Geometry.Point
            pConstruct = IFPoint.pPtPrj
            pConstruct.ConstructAngleIntersection(pFAFPptPrj, DegToRad(fFAFPDir), pPt0, DegToRad(fInvDRDir))

            fIF_FAFPDist = ReturnDistanceInMeters(pFAFPptPrj, IFPoint.pPtPrj)
            TextBox107.Text = CStr(ConvertDistance(fIF_FAFPDist, eRoundMode.NEAREST))

            fDR = ReturnDistanceInMeters(pPt0, IFPoint.pPtPrj)
            TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

            TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
            TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())

            CreateNomTrack(False)
        End If

        Dim fTrackingToler As Double
        Dim GuidanceNav As NavaidData
        Dim IntersectNav As NavaidData
        Dim pGuidPoly As IPointCollection
        Dim pTopoOper As ITopologicalOperator2

        GuidanceNav = GuidanceNavs(ComboBox103.SelectedIndex)
        IntersectNav = TPIntersectNavs(K)
        If (GuidanceNav.TypeCode = eNavaidType.VOR) Or (GuidanceNav.TypeCode = eNavaidType.TACAN) Then
            fTrackingToler = VOR.TrackingTolerance
        ElseIf GuidanceNav.TypeCode = eNavaidType.NDB Then
            fTrackingToler = NDB.TrackingTolerance
        ElseIf GuidanceNav.TypeCode = eNavaidType.LLZ Then
            fTrackingToler = LLZ.TrackingTolerance
        End If
        fDist = ReturnDistanceInMeters(GuidanceNav.pPtPrj, pTPtPrj)
        fDir = ReturnAngleInDegrees(GuidanceNav.pPtPrj, pTPtPrj)

        pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon

        pGuidPoly.AddPoint(GuidanceNav.pPtPrj)
        pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir + fTrackingToler, 10.0 * fDist + 100000.0))
        pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir - fTrackingToler, 10.0 * fDist + 100000.0))
        pGuidPoly.AddPoint(GuidanceNav.pPtPrj)
        pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir - fTrackingToler + 180.0, 10.0 * fDist + 100000.0))
        pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir + fTrackingToler + 180.0, 10.0 * fDist + 100000.0))
        pGuidPoly.AddPoint(GuidanceNav.pPtPrj)

        pTopoOper = pGuidPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pTP_FIXAreaPoly = CreateFixZone(pTPtPrj, GuidanceNav, IntersectNav, TPIntersectNavs(K).ValCnt < 0, pGuidPoly, m_fInitDir, 1600.0)
        pTP_FIXAreaElement = DrawPolygon(pTP_FIXAreaPoly, RGB(255, 127, 0))
        '???????????????????????????????????????????????????????????????????????????????????????????????????????????????
        'ProcessMesages()
        '???????????????????????????????????????????????????????????????????????????????????????????????????????????????
        pTP_FIXAreaElement.Locked = True
    End Sub

    Private Sub ComboBox108_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox108.SelectedIndexChanged
        Dim K As Long
        Dim fInvDRDir As Double

        Dim pPtCenter As IPoint
        Dim pPtCntr As IPoint
        Dim pPtFc As IPoint
        Dim pPt0 As IPoint
        Dim pClone As IClone

        Dim fDistPtCenter As Double
        Dim fAlpha As Double
        Dim fDist As Double
        Dim fDir As Double

        'Dim GuidanceNav As NavaidType
        'Dim IntersectNav As NavaidType
        Dim fTmp As Double
        Dim pConstruct As IConstructPoint

        If Not ComboBox108.Enabled Then Return

        K = ComboBox108.SelectedIndex
        If K < 0 Then Return

        pClone = ComboBox108List(K).pPtPrj
        pTPtPrj = pClone.Clone
        pTPtPrj.Z = TurnPtAltitude

        pPtFc = GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj
        fTPFcDist = ReturnDistanceInMeters(pPtFc, pTPtPrj)
        TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))


        '        Set pPtInter = New Point
        '        Set pConstruct = pPtInter
        '        pConstruct.ConstructAngleIntersection GuidanceNavs(K).pPtPrj, DegToRad(fInitDir), pFAFPptPrj, DegToRad(fFAFPDir)
        '        fDistInterFAFP = ReturnDistanceInMeters(pFAFPptPrj, pPtInter)
        'DrawPointWithText pTPtPrj, "pTPtPrj"

        If Not CheckBox001.Checked Then
            fInvDRDir = fDRDir + 180.0
            pPtCntr = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iDRType, fTurnR)
            pPt0 = PointAlongPlane(pPtCntr, fInvDRDir - 90.0 * iDRType, fTurnR)

            pConstruct = IFPoint.pPtPrj
            pConstruct.ConstructAngleIntersection(pFAFPptPrj, DegToRad(fFAFPDir), pPt0, DegToRad(fInvDRDir))
            IFPoint.pPtPrj.Z = FAPAltitude  'IFAltitude	'fProcAlt

            fIF_FAFPDist = ReturnDistanceInMeters(pFAFPptPrj, IFPoint.pPtPrj)
            TextBox107.Text = CStr(ConvertDistance(fIF_FAFPDist, eRoundMode.NEAREST))
        Else
            fDistPtCenter = iDRType * fTurnR
            pPtCenter = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0, fDistPtCenter)

            fDist = ReturnDistanceInMeters(pPtCenter, IFPoint.pPtPrj)
            fDir = ReturnAngleInDegrees(pPtCenter, IFPoint.pPtPrj)

            fAlpha = RadToDeg(ArcSin(fTurnR / fDist))

            fDRDir = Modulus(fDir - iDRType * fAlpha, 360.0)
            IFPoint.pPtPrj.M = fDRDir

            fInvDRDir = fDRDir + 180.0

            fDRInterceptAngle = Modulus((fDRDir - fFAFPDir) * iTurnDir, 360.0)

            TextBox110.Text = CStr(Math.Round(fDRInterceptAngle, 2))

            fTmp = Dir2Azt(pTPtPrj, fDRDir) - CurrADHP.MagVar
            TextBox111.Text = CStr(Math.Round(fTmp, 2))

            pPt0 = PointAlongPlane(pPtCenter, fInvDRDir - 90.0 * iDRType, fTurnR)
        End If

        fDR = ReturnDistanceInMeters(pPt0, IFPoint.pPtPrj)
        TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

        'If Not CheckBox001.Checked Then
        TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
        TextBox101_Validating(TextBox101, New CancelEventArgs())
        GetValidTPIntersectFacilities()
        'End If

        CreateNomTrack(True)

        'GuidanceNav = FixableNavaidType2NavaidType(GuidanceNavs(ComboBox103.SelectedIndex))
        'IntersectNav = FixableNavaidType2NavaidType(TPIntersectNavs(K))
        'If (GuidanceNav.TypeCode = eNavaidType.CodeVOR) Or (GuidanceNav.TypeCode = eNavaidType.CodeTACAN) Then
        '	fTrackingToler = VOR.TrackingTolerance
        'ElseIf GuidanceNav.TypeCode = eNavaidType.CodeNDB Then
        '	fTrackingToler = NDB.TrackingTolerance
        'ElseIf GuidanceNav.TypeCode = eNavaidType.CodeLLZ Then
        '	fTrackingToler = LLZ.TrackingTolerance
        'End If
        'fDist = ReturnDistanceInMeters(GuidanceNav.pPtPrj, pTPtPrj)
        'fDir = ReturnAngleInDegrees(GuidanceNav.pPtPrj, pTPtPrj)

        'pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon

        'pGuidPoly.AddPoint(GuidanceNav.pPtPrj)
        'pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir + fTrackingToler, 10.0# * fDist + 100000.0#))
        'pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir - fTrackingToler, 10.0# * fDist + 100000.0#))
        'pGuidPoly.AddPoint(GuidanceNav.pPtPrj)
        'pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir - fTrackingToler + 180.0#, 10.0# * fDist + 100000.0#))
        'pGuidPoly.AddPoint(PointAlongPlane(GuidanceNav.pPtPrj, fDir + fTrackingToler + 180.0#, 10.0# * fDist + 100000.0#))
        'pGuidPoly.AddPoint(GuidanceNav.pPtPrj)

        'pTopoOper = pGuidPoly
        'pTopoOper.IsKnownSimple_2 = False
        'pTopoOper.Simplify()

        'On Error Resume Next
        'If Not pTP_FIXAreaElement Is Nothing Then pGraphics.DeleteElement(pTP_FIXAreaElement)
        'On Error GoTo 0

        'pTP_FIXAreaPoly = CreateFixZone(pTPtPrj, GuidanceNav, IntersectNav, TPIntersectNavs(K).ValCnt < 0, pGuidPoly, m_fInitDir, 1600.0#)
        'pTP_FIXAreaElement = DrawPolygon(pTP_FIXAreaPoly, RGB(255, 255, 0))
        'pTP_FIXAreaElement.Locked = True
    End Sub

    Private Sub CInitialDeadApproach_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        screenCapture.Rollback()
        DBModule.CloseDB()
        CurrCmd = 0
        ClearScr()
        DeadApproachReport.Close()

        '    Set pGraphics = Nothing
        '    Set pIAF_IIAreaElement = Nothing
        '    Set pIAF_IAreaElement = Nothing
        '    Set pFAFptElement = Nothing
        '    Set pFAFAreaElement = Nothing
        '    Set pDefinitionsTable = Nothing

        '    Set pFAFpt = Nothing
        '    Set pFAFPptPrj = Nothing

        '    Set pIFpt = Nothing
        '    Set IFPoint.pPtPrj = Nothing
        '    Set pGuidPoly = Nothing
        '    Set IAF_IIAreaPoly = Nothing
        '    Set IAF_IAreaPoly = Nothing

        '    Erase IAFTurnNavs
        '    Erase IAFNavDat
        '    Erase SDFInterNavs
        '    Erase IAFProhibSectors
        '    Erase IAFObstList
        '    Erase IAFObstList4FIX
        '    Erase IAFObstList4Turn
        '    Erase wPoints
    End Sub

    Private Sub InfoBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ShowPanelBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ShowPanelBtn.Checked() Then
            Me.Width = 772
            ShowPanelBtn.Image = Arrival.My.Resources.bmpHIDE_INFO
        Else
            Me.Width = 587
            ShowPanelBtn.Image = Arrival.My.Resources.bmpSHOW_INFO
        End If

        If NextBtn.Enabled Then
            NextBtn.Focus()
        Else
            PrevBtn.Focus()
        End If
    End Sub

    '    Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
    '        Static PreviousTab As Short = MultiPage1.SelectedIndex()
    '        Dim e As New EventArgs()
    '        If PreviousTab = 0 Then NextBtn_ClickEvent(NextBtn, e) 'NextBtn_Click()
    '        If PreviousTab = 1 Then PrevBtn_ClickEvent(PrevBtn, e)
    '        PreviousTab = MultiPage1.SelectedIndex()
    '    End Sub

    Private Sub OptionButton101_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton101.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            TextBox107.ReadOnly = False
            TextBox107.BackColor = System.Drawing.SystemColors.Window

            TextBox108.ReadOnly = True
            TextBox108.BackColor = System.Drawing.SystemColors.Control
            TextBox109.ReadOnly = True
            TextBox109.BackColor = System.Drawing.SystemColors.Control

            CheckBox101.Enabled = False '?
            ComboBox108.Enabled = False '?

            LockControl(ComboBox106, True)
            '    ComboBox106.Locked = TextBox109.Locked
            '    ComboBox106.BackColor = TextBox109.BackColor
        End If
    End Sub

    Private Sub OptionButton102_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton102.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            TextBox107.ReadOnly = True
            TextBox107.BackColor = System.Drawing.SystemColors.Control

            TextBox108.ReadOnly = False
            TextBox108.BackColor = System.Drawing.SystemColors.Window

            TextBox109.ReadOnly = True
            TextBox109.BackColor = System.Drawing.SystemColors.Control

            CheckBox101.Enabled = False '?
            CheckBox101.Checked = False '?
            ComboBox108.Enabled = False '?

            LockControl(ComboBox106, True)
            '    ComboBox106.Locked = TextBox109.Locked
            '    ComboBox106.BackColor = TextBox109.BackColor
        End If
    End Sub

    Private Sub OptionButton103_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton103.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            TextBox107.ReadOnly = True
            TextBox107.BackColor = System.Drawing.SystemColors.Control

            TextBox108.ReadOnly = True
            TextBox108.BackColor = System.Drawing.SystemColors.Control

            TextBox109.ReadOnly = False
            TextBox109.BackColor = System.Drawing.SystemColors.Window
            'CheckBox101.Enabled = True

            LockControl(ComboBox106, ComboBox106Locked) 'Or TextBox109.Locked

            FillComboBox104()
            '    ComboBox106.Locked = TextBox109.Locked
            '    ComboBox106.BackColor = TextBox109.BackColor
        End If
    End Sub

    Private Sub OptionButton104_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton104.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            FillComboBox104()
        End If
    End Sub

    Private Sub OptionButton105_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton105.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            FillComboBox104()
        End If
    End Sub

    Private Sub OptionButton106_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton106.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            Label115.Enabled = True
            Label116.Enabled = True
            TextBox106.Enabled = True

            Label121.Enabled = False
            Label122.Enabled = False

            ComboBox104.Enabled = False
            TextBox106_Validating(TextBox106, New System.ComponentModel.CancelEventArgs())
            CheckBox101.Enabled = False '?
            ComboBox108.Enabled = False '?
        End If
    End Sub

    Private Sub OptionButton107_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton107.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            Label115.Enabled = False
            Label116.Enabled = False
            TextBox106.Enabled = False

            Label121.Enabled = True
            Label122.Enabled = True
            ComboBox104.Enabled = True
            ComboBox104_SelectedIndexChanged(ComboBox104, New System.EventArgs())
        End If
    End Sub

    Private Sub PrevBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click
        screenCapture.Delete()
        ClearScr()
        MultiPage1.SelectedIndex = 0
        NextBtn.Enabled = True
        PrevBtn.Enabled = False
        ReportBtn.Enabled = False
        fDRInterceptAngle = CDbl(TextBox012.Text)
        OptionButton101.Checked = True
        OptionButton102.Checked = False
        OptionButton103.Checked = False
        'fTxtInterceptAngle = fDRInterceptAngle
        'TextBox012.Text = CStr(fTxtInterceptAngle)

        HelpContextID = 13100
        OkBtn.Enabled = False
        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
        formStep = 0
    End Sub

    Private Sub NextBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
        CheckBox101.Checked = False
        TextBox110.Text = CStr(Math.Round(fDRInterceptAngle, 2))

        Dim fTmp As Double
        fDRDir = fFAFPDir + fDRInterceptAngle * iTurnDir
        IFPoint.pPtPrj.M = fDRDir

        fTmp = Dir2Azt(pFAFPptPrj, fDRDir) - CurrADHP.MagVar
        TextBox111.Text = CStr(Math.Round(fTmp, 2))

        TextBox101.Text = TextBox002.Text
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())

        If OptionButton101.Checked Then
            OptionButton101_CheckedChanged(OptionButton101, New System.EventArgs())
        ElseIf OptionButton102.Checked Then
            OptionButton102_CheckedChanged(OptionButton102, New System.EventArgs())
        ElseIf OptionButton103.Checked Then
            OptionButton103_CheckedChanged(OptionButton103, New System.EventArgs())
        End If

        iErrorCode = 0
        FillGuidanceNavs()

        If (iErrorCode < 0) And (iErrorCode > -5) Then Return

        If CheckBox001.Checked Then
            OptionButton101.Enabled = False
            OptionButton102.Checked = True
        Else
            OptionButton101.Enabled = True
            OptionButton101.Checked = True
        End If
        screenCapture.Save(Me)
        MultiPage1.SelectedIndex = 1
        NextBtn.Enabled = False
        PrevBtn.Enabled = True
        ReportBtn.Enabled = True
        HelpContextID = 13200
        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
        formStep = 1

        Me.Visible = False
        Me.Show(_Win32Window)
    End Sub

    Private Function InitialDeadApproachLeg0(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic) As ApproachLeg
        Dim fDir As Double

        Dim fDistToNav As Double
        Dim fAltitude As Double
        Dim fDist As Double
        Dim Angle As Double

        Dim PostFixTolerance As Double
        Dim PriorFixTolerance As Double

        Dim pTSegPoint As TerminalSegmentPoint
        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As ApproachLeg
        Dim pInterNavSignPt As SignificantPoint

        Dim pFIXSignPt As SignificantPoint
        Dim pGuidNavSignPt As SignificantPoint
        Dim pPointReference As PointReference

        Dim pSegmentPoint As SegmentPoint
        Dim pStAngleIndication As AngleIndication
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistanceIndication As DistanceIndication
        Dim pAngleIndication As AngleIndication
        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData
        Dim IntersectNav As NavaidData

        Dim pSpeed As ValSpeed
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical

        Dim mUomSpeed As UomSpeed
        Dim mUomHDistance As UomDistance
        Dim mUomVDistance As UomDistanceVertical

        Dim pLocation As Aran.Geometries.Point

        Dim uomDistHorzTab() As UomDistance
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomSpeedTab() As UomSpeed

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT}           ', UomDistance.OTHER, UomDistance.OTHER, UomDistance.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}


        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)

        pSegmentLeg = pApproachLeg

        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
        pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

        GuidNav = GuidanceNavs(ComboBox103.SelectedIndex)
        IntersectNav = TPIntersectNavs(ComboBox107.SelectedIndex)

        pGuidNavSignPt = GuidNav.GetSignificantPoint()
        pInterNavSignPt = IntersectNav.GetSignificantPoint()

        '=======================================================================================
        pSegmentLeg.Course = Dir2Azt(pTPtPrj, m_fInitDir)

        If SideDef(pTPtPrj, m_fInitDir + 90.0, GuidNav.pPtPrj) < 0 Then
            pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
        Else
            pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        End If

        '=======================================================================================
        pSegmentLeg.BankAngle = fBank
        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN

        '=======================================================================================
        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox102.Text)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical
        '===
        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox102.Text)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical
        '=======================================================================================
        pSpeed = New ValSpeed
        pSpeed.Uom = mUomSpeed
        pSpeed.Value = CDbl(TextBox007.Text)
        pSegmentLeg.SpeedLimit = pSpeed
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        ' Start Point ========================
        ' EndPoint ========================
        pTSegPoint = New TerminalSegmentPoint()
        pTSegPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF

        pSegmentPoint = pTSegPoint
        'pSegmentPoint.FlyOver = True
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        If IntersectNav.ValCnt < 0 Then
            pFIXSignPt = pInterNavSignPt
            pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
        Else
            Dim HorAccuracy As Double = 0.0
            If (GuidNav.TypeCode <> eNavaidType.DME) And (IntersectNav.Identifier <> Guid.Empty) Then
                HorAccuracy = CalcHorisontalAccuracy(pTPtPrj, GuidNav, IntersectNav)
            End If

            pFixDesignatedPoint = CreateDesignatedPoint(pTPtPrj, , Azt2DirPrj(pTPtPrj, pSegmentLeg.Course))
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            '== Angle Use ==========================================================
            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, pTPtPrj)
            Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(pTPtPrj, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference = New PointReference()
            pPointReference.FacilityAngle.Add(pAngleUse)
            '== Angle Use ==========================================================

            If IntersectNav.TypeCode = eNavaidType.DME Then
                fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, pTPtPrj)
                'fAltitude = pTPtPrj.Z - IntersectNav.pPtPrj.Z ' + fRefHeight
                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
                pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                pPointReference.Role = CodeReferenceRole.RAD_DME
            Else
                pAngleUse = New AngleUse
                fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, pTPtPrj)

                Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
                pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                pStAngleIndication.TrueAngle = Dir2Azt(pTPtPrj, fDir)
                pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = False

                pPointReference.FacilityAngle.Add(pAngleUse)
                pPointReference.Role = CodeReferenceRole.INTERSECTION
            End If
        End If
        '=======================
        PriorPostFixTolerance(pTP_FIXAreaPoly, pTPtPrj, m_fInitDir, PriorFixTolerance, PostFixTolerance)

        pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
        pPointReference.PriorFixTolerance = pDistanceSigned

        pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

        pPointReference.PostFixTolerance = pDistanceSigned
        pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTP_FIXAreaPoly))
        '========
        pTSegPoint.FacilityMakeup.Add(pPointReference)
        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.StartPoint = pTSegPoint
        pSegmentLeg.EndPoint = pTSegPoint
        ' End of EndPoint ========================
        ' End Of Start Point ========================

        ' Trajectory ==============================================
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim ptTPGeo As IPoint

        ptTPGeo = ToGeo(pTPtPrj)
        pLineStringSegment = New LineString

        pLocation = ESRIPointToARANPoint(ptTPGeo)
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(ptTPGeo)
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve
        ' Trajectory ==============================================

        Return pApproachLeg
        ' END =====================================================
    End Function

    Private Function InitialDeadApproachLeg1(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic) As ApproachLeg
        Dim I As Integer
        Dim fTmp As Double

        Dim pApproachLeg As ApproachLeg
        Dim pSegmentLeg As SegmentLeg
        Dim pDistance As ValDistance
        Dim pDistanceVertical As ValDistanceVertical

        Dim pSpeed As ValSpeed

        Dim mUomHDistance As UomDistance
        Dim mUomVDistance As UomDistanceVertical

        Dim mUomSpeed As UomSpeed

        Dim pLocation As Aran.Geometries.Point

        Dim uomDistHorzTab() As UomDistance
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomSpeedTab() As UomSpeed

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pApproachLeg

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL

        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VI
        '=======================================================================================
        pSegmentLeg.Course = Dir2Azt(IFPoint.pPtPrj, fDRDir)
        pSegmentLeg.BankAngle = fBank
        '=======================================================================================

        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox101.Text)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical
        '===
        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox102.Text)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical
        '=======================================================================================
        pDistance = New ValDistance
        pDistance.Uom = mUomHDistance
        pDistance.Value = ConvertDistance(fNomLenght)                   'fDr
        pSegmentLeg.Length = pDistance

        '===
        Dim H1 As Double
        Dim H2 As Double
        H1 = DeConvertHeight(CDbl(TextBox102.Text))     ' fTurnPtAlt
        H2 = DeConvertHeight(CDbl(TextBox101.Text))     ' fIFPtAlt

        fTmp = (H1 - H2) / fDR
        If fTmp < 0.04 Then fTmp = 0.04 'If fTmp > 0.08 Then fTmp = 0.08

        pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(fTmp))
        '===
        pSpeed = New ValSpeed
        pSpeed.Uom = mUomSpeed
        pSpeed.Value = CDbl(TextBox007.Text)
        pSegmentLeg.SpeedLimit = pSpeed
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        ' Trajectory =====================================================
        Dim J As Integer
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim pPolyline As IGeometryCollection
        Dim pPath As IPointCollection
        Dim pPtTmp As IPoint
        Dim pZAware As IZAware
        Dim pZ As IZ

        pPtTmp = pNomTrack.FromPoint
        If bIFFound And CheckBox001.Checked Then
            pPtTmp.Z = IFAltitudeInBase
        Else
            pPtTmp.Z = IFPoint.pPtPrj.Z
        End If
        pNomTrack.FromPoint = pPtTmp

        pPtTmp = pNomTrack.ToPoint
        pPtTmp.Z = TurnPtAltitude
        pNomTrack.ToPoint = pPtTmp

        pZAware = pNomTrack
        pZAware.ZAware = True

        pZ = pNomTrack
        pZ.CalculateNonSimpleZs()

        pCurve = New Curve
        pPolyline = pNomTrack

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
        ' Trajectory =====================================================

        ' I Protection Area 
        Dim pTopo As ITopologicalOperator2

        Dim pPolygon As IPolygon
        pTopo = pTurnA_FullAreaPoly
        pPolygon = pTopo.Difference(pTurnA_SecAreaPoly)
        pTopo = pPolygon
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        Dim pSurface As Surface
        Dim pPrimProtectedArea As ObstacleAssessmentArea

        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

        ' I Protection Area ========================================

        ' II Protection Area 

        Dim pSecProtectedArea As ObstacleAssessmentArea
        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pTurnA_SecAreaPoly))

        pSecProtectedArea = New ObstacleAssessmentArea
        pSecProtectedArea.Surface = pSurface
        pSecProtectedArea.SectionNumber = 1
        pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

        ' II Protection Area ========================================

        'Protection Area Obstructions list ==================================================

        AddObstacles(TurnAObstList, mUomVDistance, pPrimProtectedArea, pSecProtectedArea)

        pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
        pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

        Return pApproachLeg
        ' END =====================================================
    End Function

    Private Function InitialDeadApproachLeg2(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic) As ApproachLeg
        Dim fDir As Double
        Dim fDist As Double
        Dim Angle As Double
        Dim fAltitude As Double
        Dim fDistToNav As Double
        Dim PriorFixTolerance As Double
        Dim PostFixTolerance As Double

        Dim GuidNav As NavaidData
        Dim IntersectNav As NavaidData

        Dim pTSegPoint As TerminalSegmentPoint
        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As ApproachLeg

        Dim pDistanceIndication As DistanceIndication

        Dim pSegmentPoint As SegmentPoint
        Dim pAngleIndication As AngleIndication
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistanceVertical As ValDistanceVertical
        Dim pDistanceSigned As ValDistanceSigned
        Dim pSpeed As ValSpeed

        Dim mUomVDistance As UomDistanceVertical
        Dim mUomHDistance As UomDistance
        Dim mUomSpeed As UomSpeed

        Dim pFIXSignPt As SignificantPoint
        Dim pGuidNavSignPt As SignificantPoint

        Dim pLocation As Aran.Geometries.Point
        Dim pIFptGeo As IPoint

        Dim pAngleUse As AngleUse
        Dim pPointReference As PointReference
        Dim pInterNavSignPt As SignificantPoint
        Dim pStAngleIndication As AngleIndication

        Dim uomDistHorzTab() As UomDistance
        Dim uomDistVerTab() As UomDistanceVertical
        Dim uomSpeedTab() As UomSpeed

        uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
        uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        mUomHDistance = uomDistHorzTab(DistanceUnit)
        mUomVDistance = uomDistVerTab(HeightUnit)
        mUomSpeed = uomSpeedTab(SpeedUnit)

        pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pApproachLeg

        GuidNav = StartPoint.GuidanceNav
        IntersectNav = IFIntersectNavs(ComboBox105.SelectedIndex)

        pGuidNavSignPt = GuidNav.GetSignificantPoint()
        pInterNavSignPt = IntersectNav.GetSignificantPoint()

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF

        '=======================================================================================
        pSegmentLeg.Course = Dir2Azt(IFPoint.pPtPrj, fFAFPDir)
        pSegmentLeg.BankAngle = fBank
        '=======================================================================================

        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox101.Text)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical
        '===
        pDistanceVertical = New ValDistanceVertical
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = CDbl(TextBox101.Text)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical
        '=======================================================================================
        pSpeed = New ValSpeed
        pSpeed.Uom = mUomSpeed
        pSpeed.Value = CDbl(TextBox007.Text)
        pSegmentLeg.SpeedLimit = pSpeed
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        pIFptGeo = ToGeo(IFPoint.pPtPrj)

        ' Start Point ========================
        ' EndPoint ========================
        pTSegPoint = New TerminalSegmentPoint()
        pTSegPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IF

        'If (CheckBox001.Checked) Then		    pTSegPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF

        pSegmentPoint = pTSegPoint
        'pSegmentPoint.FlyOver = True
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        If IntersectNav.ValCnt < 0 Then
            pFIXSignPt = pInterNavSignPt
            pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
        Else
            Dim HorAccuracy As Double = 0.0
            If (GuidNav.TypeCode <> eNavaidType.DME) And (IntersectNav.Identifier <> Guid.Empty) Then
                HorAccuracy = CalcHorisontalAccuracy(IFPoint.pPtPrj, GuidNav, IntersectNav)
            End If

            pFixDesignatedPoint = CreateDesignatedPoint(IFPoint.pPtPrj, , Azt2DirPrj(IFPoint.pPtPrj, pSegmentLeg.Course))
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            '== Angle Use ==========================================================
            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, IFPoint.pPtPrj)
            Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(IFPoint.pPtPrj, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference = New PointReference()
            pPointReference.FacilityAngle.Add(pAngleUse)
            '== Angle Use ==========================================================

            If IntersectNav.TypeCode = eNavaidType.DME Then
                fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, IFPoint.pPtPrj)
                'fAltitude = IFPoint.pPtPrj.Z - IntersectNav.pPtPrj.Z ' + fRefHeight

                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
                pDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                ' ========================pStDistanceIndication.
                pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                pPointReference.Role = CodeReferenceRole.RAD_DME
            Else
                pAngleUse = New AngleUse
                fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, IFPoint.pPtPrj)
                Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)

                pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                pStAngleIndication.TrueAngle = Dir2Azt(IFPoint.pPtPrj, fDir)
                pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = False

                pPointReference.FacilityAngle.Add(pAngleUse)
                pPointReference.Role = CodeReferenceRole.INTERSECTION
            End If
        End If

        '==============================================================================
        PriorPostFixTolerance(pIF_FIXAreaPoly, IFPoint.pPtPrj, m_fInitDir, PriorFixTolerance, PostFixTolerance)

        If PriorFixTolerance > 0 Then
            pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
            pPointReference.PriorFixTolerance = pDistanceSigned
        End If

        If PostFixTolerance > 0 Then
            pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))
            pPointReference.PostFixTolerance = pDistanceSigned
        End If

        If (PriorFixTolerance > 0) Or (PostFixTolerance > 0) Then
            pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pIF_FIXAreaPoly))
        End If
        '========
        pTSegPoint.FacilityMakeup.Add(pPointReference)
        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.StartPoint = pTSegPoint
        pSegmentLeg.EndPoint = pTSegPoint
        ' End of EndPoint ========================
        ' End of Start Point ========================

        ' Trajectory =====================================================
        Dim pLineStringSegment As LineString
        Dim pCurve As Curve

        pLineStringSegment = New LineString

        pLocation = ESRIPointToARANPoint(pIFptGeo)
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(pIFptGeo)
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve
        ' Trajectory =====================================================

        Return pApproachLeg
        ' END =====================================================
    End Function

    Private Function SaveProcedure() As Boolean
        Dim K As Integer
        Dim NO_SEQ As Integer

        Dim pEndPoint As TerminalSegmentPoint = Nothing
        Dim pTransition As ProcedureTransition
        Dim pSegmentLeg As SegmentLeg
        Dim ptl As ProcedureTransitionLeg

        pObjectDir.ClearAllFeatures()

        K = ComboBox001.SelectedIndex
        pProcedure = IAPArray(K)

        'Transition ==========================================================================
        pTransition = New ProcedureTransition

        'Legs ======================================================================================================
        NO_SEQ = 0


        gAranEnv.GetLogger("DEAD RECKONING").Info("Initial Dead Approach Leg 0")
        ' Initial Dead Approach Leg 0 ===============================================================================
        NO_SEQ = NO_SEQ + 1
        pSegmentLeg = InitialDeadApproachLeg0(pProcedure, pProcedure.AircraftCharacteristic(0))

        ptl = New ProcedureTransitionLeg()
        ptl.SeqNumberARINC = NO_SEQ
        ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
        pTransition.TransitionLeg.Add(ptl)

        gAranEnv.GetLogger("DEAD RECKONING").Info("Initial Dead Approach Leg 1")
        ' Initial Dead Approach Leg 1 ===============================================================================
        NO_SEQ = NO_SEQ + 1
        pSegmentLeg = InitialDeadApproachLeg1(pProcedure, pProcedure.AircraftCharacteristic(0))

        ptl = New ProcedureTransitionLeg()
        ptl.SeqNumberARINC = NO_SEQ
        ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
        pTransition.TransitionLeg.Add(ptl)

        gAranEnv.GetLogger("DEAD RECKONING").Info("Initial Dead Approach Leg 2")
        ' Initial Dead Approach Leg 2 ===============================================================================
        NO_SEQ = NO_SEQ + 1
        pSegmentLeg = InitialDeadApproachLeg2(pProcedure, pProcedure.AircraftCharacteristic(0))

        ptl = New ProcedureTransitionLeg()
        ptl.SeqNumberARINC = NO_SEQ
        ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
        pTransition.TransitionLeg.Add(ptl)
        'End of Legs ================================================================================================

        pTransition.DepartureRunwayTransition = pLandingTakeoff
        pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.APPROACH

        gAranEnv.GetLogger("DEAD RECKONING").Info("Procedure set")
        ' Procedure =================================================================================================
        pProcedure.FlightTransition.Add(pTransition)
        pObjectDir.SetFeature(pProcedure)

        Dim result As Boolean = False

        gAranEnv.GetLogger("DEAD RECKONING").Info("Dead reckoning Procedure save/commit start")
        Try
            pObjectDir.SetRootFeatureType(FeatureType.InstrumentApproachProcedure)

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
                result = pObjectDir.
                    CommitWithMetadataViewer(
                    gAranEnv.Graphics.ViewProjection.Name,
                    commitedFeatures.ToArray(),
                    metadataFeatures.ToArray(),
                    GetNumericalData(),
                    False)
            Else
                result = pObjectDir.Commit(commitedFeatures.ToArray())
            End If

            gAranEnv.RefreshAllAimLayers()

            gAranEnv.GetLogger("DEAD RECKONING").Info("Dead reckoning Procedure save/commit end")
        Catch ex As Exception
            gAranEnv.GetLogger("DEAD RECKONING").Error(ex, "Save procedure")
            MsgBox("Error on commit." + vbCrLf + ex.Message)
            Return False
        End Try

        Return result
    End Function

    Private Function GetNumericalData() As List(Of GeoNumericalDataModel)
        Dim NumericalData As New List(Of GeoNumericalDataModel)

        Dim GuidanceNav As NavaidData
        Dim IntersectNav As NavaidData

        'Leg 0 =================================================================================

        GuidanceNav = GuidanceNavs(ComboBox103.SelectedIndex)
        IntersectNav = TPIntersectNavs(ComboBox107.SelectedIndex)

        Dim IafHorAccuracy As Double = CalcHorisontalAccuracy(pTPtPrj, GuidanceNav, IntersectNav)
        NumericalData.Add(New GeoNumericalDataModel With
        {
            .Role = "IAF",
            .Accuracy = IafHorAccuracy,
            .Resolution = 0.0,
            .DesignatorDescription = GetDesignatedPointDescription(pTPtPrj),
            .LegType = "Initial"
        })

        'Leg 2 =================================================================================
        GuidanceNav = StartPoint.GuidanceNav
        IntersectNav = IFIntersectNavs(ComboBox105.SelectedIndex)

        Dim IfHorAccuracy As Double = CalcHorisontalAccuracy(IFPoint.pPtPrj, GuidanceNav, IntersectNav)
        NumericalData.Add(New GeoNumericalDataModel With
        {
            .Role = "IF",
            .Accuracy = IfHorAccuracy,
            .Resolution = 0.0,
            .DesignatorDescription = GetDesignatedPointDescription(IFPoint.pPtPrj),
            .LegType = "Initial"
        })

        Return NumericalData
    End Function

	Private Sub OkBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OkBtn.Click
		Dim RepFileName As String
		Dim RepFileTitle As String
		Dim pReport As ReportHeader

		If (nomTrackIsChanged) Then
			CalculateObstacles()
		End If

		screenCapture.Save(Me)
		If Not ShowSaveDialog(RepFileName, RepFileTitle) Then Return

		pReport.Aerodrome = CurrADHP.Name
		pReport.Database = gAranEnv.ConnectionInfo.Database

		pReport.Procedure = IAPArray(ComboBox001.SelectedIndex).Name
		'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition

		pReport.Category = TextBox015.Text      'IAPArray(ComboBox001.ListIndex).CodeCatAircraft
		'pReport.RWY = ""

		SaveAccuracy(RepFileName, RepFileTitle, pReport)
		SaveProtocol(RepFileName, RepFileTitle, pReport)
		SaveGeometry(RepFileName, RepFileTitle, pReport)

		DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml")

		Try
			If SaveProcedure() Then
				saveReportToDB()
				saveScreenshotToDB()
				Me.Close()
			End If
		Catch ex As Exception
			gAranEnv.GetLogger("DEAD RECKONING").Error(ex, "Ok button click")
			Throw
		End Try
	End Sub

	Private Sub saveReportToDB(rp As ReportFile, type As FeatureReportType)
        If (rp.IsFinished) Then
            Dim report As FeatureReport = Nothing
            report = New FeatureReport()
            report.Identifier = pProcedure.Identifier
            report.ReportType = type
            report.HtmlZipped = rp.Report
            pObjectDir.SetFeatureReport(report)
        End If
    End Sub

    Private Sub saveReportToDB()
        saveReportToDB(ProtRep, FeatureReportType.Protocol)
        saveReportToDB(GeomRep, FeatureReportType.Geometry)
    End Sub

    Private Sub saveScreenshotToDB()
        Dim screenshot As Screenshot = Nothing
        screenshot = New Screenshot()
        screenshot.DateTime = DateTime.Now
        screenshot.Identifier = pProcedure.Identifier
        screenshot.Images = screenCapture.Commit(pProcedure.Identifier)
        pObjectDir.SetScreenshot(screenshot)
    End Sub

    Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
        Me.Close()
    End Sub

    Private Sub ReportBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReportBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ReportBtn.Checked Then
            If nomTrackIsChanged Then
                NativeMethods.ShowPandaBox(gAranEnv.Win32Window.Handle.ToInt32())
                CalculateObstacles()
                NativeMethods.HidePandaBox()
            End If

            DeadApproachReport.Show(_Win32Window)
        Else
            DeadApproachReport.Hide()
        End If
    End Sub

    Private Sub TextBox006_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox006.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox006_Validating(TextBox006, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox006.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox006_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox006.Validating
        Dim fTmp As Double

        If TextBox006.ReadOnly Then Return
        If Not IsNumeric(TextBox006.Text) Then Return

        fTmp = DeConvertDistance(CDbl(TextBox006.Text))
        fFAPSemiWidth = fTmp
        If fTmp < 1000.0 Then fTmp = 1000.0
        If fTmp > 6000.0 Then fTmp = 6000.0
        If fFAPSemiWidth <> fTmp Then
            fFAPSemiWidth = fTmp
            TextBox006.Text = CStr(ConvertDistance(fFAPSemiWidth, 3))
        End If
    End Sub

    Private Sub TextBox007_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox007.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox007_Validating(TextBox007, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox007.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox007_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox007.Validating
        Dim fTmp As Double

        If TextBox007.ReadOnly Then Return
        If Not IsNumeric(TextBox007.Text) Then Return
        fTmp = DeConvertSpeed(CDbl(TextBox007.Text))
        fIAS = fTmp

        If fTmp > cViafMax.Values(iCategory) Then fTmp = cViafMax.Values(iCategory)
        If fTmp < cViafMin.Values(iCategory) Then fTmp = cViafMin.Values(iCategory)

        If fIAS <> fTmp Then
            fIAS = fTmp
            TextBox007.Text = CStr(ConvertSpeed(fIAS, 2))
        End If

        fIAS = 3.6 * fIAS
        TextBox008_Validating(TextBox008, New System.ComponentModel.CancelEventArgs())
    End Sub

    Private Sub TextBox008_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox008.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox008_Validating(TextBox008, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox008.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox008_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox008.Validating
        Dim fTmp As Double
        Dim L0 As Double
        Dim L1 As Double

        If TextBox008.ReadOnly Then Return
        If Not IsNumeric(TextBox008.Text) Then Return
        fTmp = DeConvertHeight(CDbl(TextBox008.Text))
        fProcAlt = fTmp

        If fTmp < TextBox008MinVal Then fTmp = TextBox008MinVal
        If fTmp > 4500.0 Then fTmp = 4500.0

        If fProcAlt <> fTmp Then
            fProcAlt = fTmp
            TextBox008.Text = CStr(ConvertHeight(fProcAlt, eRoundMode.NEAREST))
        End If

        If fIAS <= 335.0 Then
            L0 = 11000.0
            L1 = 12000.0
        Else
            L0 = 17000.0
            L1 = 20000.0
        End If

        fminInter = L0 + (fProcAlt - 1500.0) / 1500.0 * (L1 - L0)
        TextBox011.Text = CStr(ConvertDistance(fminInter, eRoundMode.NEAREST))

        fTAS = IAS2TAS(fIAS, fProcAlt, fAirportISAtC.Value)

        fTurnR = Bank2Radius(25.0, fTAS)
        fBank = Radius2Bank(fTurnR, fTAS)

        TextBox009.Text = CStr(ConvertSpeed(fTAS * 0.27777777777777779, 2))
        TextBox010.Text = CStr(ConvertDistance(fTurnR, eRoundMode.NEAREST))
        ToolTip1.SetToolTip(TextBox010, My.Resources.str50123 + " " + CStr(System.Math.Round(fBank, 1)) + "°")
    End Sub

    Private Sub TextBox012_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox012.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox012_Validating(TextBox012, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox012.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox012_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox012.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim fTmp As Double

        If Not IsNumeric(TextBox012.Text) Then TextBox012.Text = CStr(fTxtInterceptAngle)
        fTmp = CDbl(TextBox012.Text)

        fTxtInterceptAngle = fTmp
        If fTmp > 80.0 Then fTmp = 80.0
        If fTmp < 10.0 Then fTmp = 10.0

        If fTxtInterceptAngle <> fTmp Then
            fTxtInterceptAngle = fTmp
            TextBox012.Text = CStr(System.Math.Round(fTxtInterceptAngle))
        End If
        fDRInterceptAngle = fTxtInterceptAngle
        TextBox017_Validating(TextBox017, New System.ComponentModel.CancelEventArgs())
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub TextBox013_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox013.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox013_Validating(TextBox013, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox013.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox013_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox013.Validating
        Dim fTmp As Double
        If Not IsNumeric(TextBox013.Text) Then Return
        fTmp = DeConvertDistance(CDbl(TextBox013.Text))

        If fTmp < 3700.0 Then fTmp = 3700.0
        If fTmp > 5000.0 Then fTmp = 5000.0
        fFIXMaxToler = fTmp
        TextBox013.Text = CStr(ConvertDistance(fFIXMaxToler, eRoundMode.NEAREST))
    End Sub

    Private Sub TextBox011_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox011.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox011_Validating(TextBox011, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox011.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox011_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox011.Validating
        Dim fTmp As Double
        Dim fNewValue As Double

        If TextBox011.ReadOnly Then Return
        If Not IsNumeric(TextBox011.Text) Then TextBox011.Text = CStr(ConvertDistance(fminInter, eRoundMode.NEAREST))

        fTmp = DeConvertDistance(CDbl(TextBox011.Text))
        fNewValue = fTmp

        If fTmp > fminInter Then fTmp = fminInter
        If fTmp < 2800.0 Then fTmp = 2800.0

        If fNewValue <> fTmp Then
            fNewValue = fTmp
            TextBox011.Text = CStr(ConvertDistance(fNewValue, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub TextBox016_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox016.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox016_Validating(TextBox016, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox016.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox016_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox016.Validating
        Dim fTmp As Double
        Dim fNewValue As Double

        If TextBox016.ReadOnly Then Return
        If Not IsNumeric(TextBox016.Text) Then TextBox016.Text = CStr(ConvertDistance(2800.0, eRoundMode.NEAREST))

        fTmp = DeConvertDistance(CDbl(TextBox016.Text))
        fNewValue = fTmp
        'If fTmp > 42000.0 Then fTmp = 42000.0
        'If fTmp < 28000.0 Then fTmp = 28000.0

        If fTmp > 28000.0# Then fTmp = 28000.0#

        If fNewValue <> fTmp Then
            fNewValue = fTmp
            TextBox016.Text = CStr(ConvertDistance(fNewValue, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub TextBox017_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox017.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox017_Validating(TextBox017, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox017.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox017_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox017.Validating
        Dim fTmp As Double
        Dim fMinVal As Double
        Dim fNewValue As Double

        If TextBox017.ReadOnly Then Return
        If Not IsNumeric(TextBox017.Text) Then TextBox017.Text = CStr(ConvertDistance(6000.0, eRoundMode.NEAREST))

        fMinVal = 0.001 * fTurnR * System.Math.Tan(DegToRad(0.5 * fDRInterceptAngle))
        fTmp = DeConvertDistance(CDbl(TextBox017.Text))

        fNewValue = fTmp
        If fTmp > 6000.0 Then fTmp = 6000.0
        If fTmp < fMinVal Then fTmp = fMinVal

        If fNewValue <> fTmp Then
            fNewValue = fTmp
            TextBox017.Text = CStr(ConvertDistance(fNewValue, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub TextBox018_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox018.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox018_Validating(TextBox018, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox018.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox018_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox018.Validating
        Dim fTmp As Double
        Dim fNewValue As Double

        If TextBox018.ReadOnly Then Return
        If Not IsNumeric(TextBox018.Text) Then TextBox018.Text = CStr(ConvertDistance(19000.0, eRoundMode.NEAREST))

        fTmp = DeConvertDistance(CDbl(TextBox018.Text))
        fNewValue = fTmp
        If fTmp > 28000.0 Then fTmp = 28000.0
        If fTmp < 19000.0 Then fTmp = 19000.0

        If fNewValue <> fTmp Then
            fNewValue = fTmp
            TextBox018.Text = CStr(ConvertDistance(fNewValue, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub TextBox101_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox101.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(e.KeyChar)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox101_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox101.Validating
        Dim fTmp As Double

        If Not IsNumeric(TextBox101.Text) Then
            If Not IsNumeric(TextBox101.Tag) Then TextBox101.Text = TextBox101.Tag

            Return
        End If

        fTmp = DeConvertHeight(CDbl(TextBox101.Text))
        IFAltitude = fTmp
        If IFAltitude < TextBox101_Interval.Low Then IFAltitude = TextBox101_Interval.Low
        If IFAltitude > TextBox101_Interval.High Then IFAltitude = TextBox101_Interval.High

        If IFAltitude <> fTmp Then
            IFPoint.pPtPrj.Z = IFAltitude
            TextBox101.Text = CStr(ConvertHeight(IFAltitude, eRoundMode.NEAREST))
        End If

        TextBox101.Tag = TextBox101.Text
        '====================================================================================

        DeadApproachReport.RecalcPage2(IFAltitude)

        If MaxTurnReqH > IFAltitude Then
            TextBox101.ForeColor = Color.Red
        Else
            TextBox101.ForeColor = SystemColors.WindowText
        End If

        OkBtn.Enabled = (MaxIntermReqH <= pFAFPptPrj.Z) And (MaxTurnReqH < IFAltitude)

        '====================================================================================
        TextBox102_Interval.Low = IFAltitude
        TextBox102_Interval.High = TextBox102_Interval.Low + fDR * arIADescent_Max.Value

        If TurnPtAltitude < TextBox102_Interval.Low Then TurnPtAltitude = TextBox102_Interval.Low
        If TurnPtAltitude > TextBox102_Interval.High Then TurnPtAltitude = TextBox102_Interval.High

        If Not (pTPtPrj Is Nothing) Then
            pTPtPrj.Z = TurnPtAltitude
        End If
        TextBox102.Text = CStr(ConvertHeight(TurnPtAltitude, eRoundMode.NEAREST))

        TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs())
    End Sub

    Private Sub TextBox102_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox102.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox102_Validating(TextBox102, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(e.KeyChar)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox102_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox102.Validating
        Dim fTmp As Double

        If Not IsNumeric(TextBox102.Text) Then
            If (IsNumeric(TextBox102.Tag)) Then
                TextBox102.Text = TextBox102.Tag
            End If
            Return
        End If

        fTmp = DeConvertHeight(CDbl(TextBox102.Text))
        TurnPtAltitude = fTmp

        If TurnPtAltitude < TextBox102_Interval.Low Then TurnPtAltitude = TextBox102_Interval.Low
        If TurnPtAltitude > TextBox102_Interval.High Then TurnPtAltitude = TextBox102_Interval.High

        If TurnPtAltitude <> fTmp Then
            If Not (pTPtPrj Is Nothing) Then pTPtPrj.Z = TurnPtAltitude
            TextBox102.Text = CStr(ConvertHeight(TurnPtAltitude, eRoundMode.NEAREST))
        End If

        TextBox102.Tag = TextBox102.Text
    End Sub

    Private Sub TextBox105_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox105.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox105_Validating(TextBox105, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(eventArgs.KeyChar)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox105_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox105.Validating
        If Not TextBox105.ReadOnly Then FillGuidanceNavs()
    End Sub

    Private Sub TextBox106_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox106.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox106_Validating(TextBox106, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(eventArgs.KeyChar)
        End If

        If eventArgs.KeyChar = Chr(0) Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextBox106_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox106.Validating
        Dim K As Integer
        Dim fInitAzt As Double
        Dim fTmp0 As Double
        Dim fTmp1 As Double

        If TextBox106.ReadOnly Then Return
        If Not IsNumeric(TextBox106.Text) Then Return
        If ComboBox103.Items.Count < 0 Then Return
        If ComboBox103.SelectedIndex < 0 Then Return
        K = ComboBox103.SelectedIndex

        If OptionButton104.Checked Then
            fTmp0 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(1))
            fTmp1 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(1))
        ElseIf OptionButton105.Checked Then
            fTmp0 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMin(0))
            fTmp1 = Dir2Azt(GuidanceNavs(K).pPtPrj, GuidanceNavs(K).ValMax(0))
        Else
            Return
        End If

        fInitAzt = CDbl(TextBox106.Text)

        If Not AngleInSector(fInitAzt, fTmp1, fTmp0) Then
            fInitAzt = System.Math.Round(fTmp1 + 0.4999)
            TextBox106.Text = CStr(fInitAzt)
        End If

        m_fInitDir = Azt2Dir(GuidanceNavs(K).pPtGeo, fInitAzt)
        InitialCourseChanged(m_fInitDir)
    End Sub

    Private Sub InitialCourseChanged(ByRef fInitDir As Double)
        Dim K As Integer
        Dim Side0 As Integer
        Dim Side1 As Integer

        Dim fdL As Double
        Dim fTmp As Double
        Dim fDMin As Double
        Dim fDMax As Double

        Dim fDRMin As Double
        Dim fDRMax As Double

        Dim fAvaTPMin As Double
        Dim fAvaTPMax As Double

        Dim fAvaDRMin As Double
        Dim fAvaDRMax As Double

        Dim fAvaIF_FAFPMin As Double
        Dim fAvaIF_FAFPMax As Double

        Dim fIF_FAFPMin As Double
        Dim fIF_FAFPMax As Double

        Dim fL0Min As Double
        Dim fInvDRDir As Double

        Dim pPtInter As IPoint
        Dim pPtIFMin As IPoint
        Dim pPtIFMax As IPoint
        Dim pConstruct As IConstructPoint

        ToolTip1.SetToolTip(TextBox107, "")
        ToolTip1.SetToolTip(TextBox108, "")
        ToolTip1.SetToolTip(TextBox109, "")

        On Error Resume Next
        If Not pInitLineElement Is Nothing Then pGraphics.DeleteElement(pInitLineElement)
        On Error GoTo 0

        K = ComboBox103.SelectedIndex

        If bIFFound And CheckBox001.Checked Then
            fIF_FAFPMin = ReturnDistanceInMeters(IFPoint.pPtPrj, StartPoint.pPtPrj)
            fIF_FAFPMax = fIF_FAFPMin
        Else
            fIF_FAFPMin = DeConvertDistance(CDbl(TextBox011.Text))
            fIF_FAFPMax = DeConvertDistance(CDbl(TextBox016.Text))
        End If

        fDRMin = DeConvertDistance(CDbl(TextBox017.Text))
        fDRMax = DeConvertDistance(CDbl(TextBox018.Text))

        If System.Math.Abs(System.Math.Sin(DegToRad(fFAFPDir - fInitDir))) > radEps Then
            pPtInter = New ESRI.ArcGIS.Geometry.Point
            pConstruct = pPtInter
            pConstruct.ConstructAngleIntersection(GuidanceNavs(K).pPtPrj, DegToRad(fInitDir), pFAFPptPrj, DegToRad(fFAFPDir))

            pPtIFMin = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPMin)
            pPtIFMax = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPMax)

            Side0 = SideDef(pPtInter, fFAFPDir + 90.0, pPtIFMin)
            Side1 = SideDef(pPtInter, fFAFPDir + 90.0, pPtIFMax)

            fDistInterFAFP = ReturnDistanceInMeters(pFAFPptPrj, pPtInter)
            If ComboBox003.SelectedIndex = 0 Then
                If Side0 > 0 Then
                    fInterSign = 1.0
                    fBetta = Modulus((fFAFPDir + 180.0 - fInitDir) * iDRType, 360.0)
                    If fBetta > 180.0 Then fBetta = 360.0 - fBetta

                    fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fBetta + fDRInterceptAngle)))
                    fL0Min = fTurnR / System.Math.Tan(DegToRad(0.5 * fBetta))

                    If Side1 > 0 Then
                        fTmp = (fL0Min - fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                        fDMin = Max(ReturnDistanceInMeters(pPtIFMax, pPtInter), fTmp)
                    Else
                        fDMin = (fL0Min - fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                    End If

                    fTmp = (fDRMin + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fBetta))
                    fDMin = Max(fDMin, fTmp)

                    fAvaIF_FAFPMax = Min(fDistInterFAFP - fDMin, fIF_FAFPMax)
                    fDMin = fDistInterFAFP - fAvaIF_FAFPMax
                    '========================
                    fTmp = (fDRMax + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fBetta))
                    fDMax = Min(ReturnDistanceInMeters(pPtIFMin, pPtInter), fTmp)

                    fAvaIF_FAFPMin = Max(fDistInterFAFP - fDMax, fIF_FAFPMin)
                    fDMax = fDistInterFAFP - fAvaIF_FAFPMin

                    fAvaDRMin = fDMin * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) - fdL
                    fAvaDRMax = fDMax * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) - fdL
                Else
                    iErrorCode = -1
                    MessageBox.Show(My.Resources.str00303)
                    Return
                End If
            Else
                fBetta = Modulus((fInitDir - fFAFPDir) * iDRType, 360.0)

                If fBetta < 180.0 Then
                    fInterSign = 1.0
                    If Side0 > 0 Then
                        fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fBetta + fDRInterceptAngle)))

                        fL0Min = fTurnR / System.Math.Tan(DegToRad(0.5 * fBetta))

                        If Side1 > 0 Then
                            fTmp = (fL0Min + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                            fDMin = Max(ReturnDistanceInMeters(pPtIFMax, pPtInter), fTmp)
                        Else
                            fDMin = (fL0Min + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                        End If

                        fTmp = (fDRMin + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fBetta))
                        fDMin = Max(fDMin, fTmp)

                        fAvaIF_FAFPMax = Min(fDistInterFAFP - fDMin, fIF_FAFPMax)
                        fDMin = fDistInterFAFP - fAvaIF_FAFPMax
                        '========================
                        fTmp = (fDRMax + fdL) * System.Math.Sin(DegToRad(fBetta + fDRInterceptAngle)) / System.Math.Sin(DegToRad(fBetta))
                        fDMax = Min(ReturnDistanceInMeters(pPtIFMin, pPtInter), fTmp)

                        fAvaIF_FAFPMin = Max(fDistInterFAFP - fDMax, fIF_FAFPMin)
                        fDMax = fDistInterFAFP - fAvaIF_FAFPMin

                        fAvaDRMin = fDMin * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fBetta)) - fdL
                        fAvaDRMax = fDMax * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fBetta)) - fdL
                    Else
                        iErrorCode = -2
                        MessageBox.Show(My.Resources.str00303)
                        Return
                    End If
                Else
                    fInterSign = -1.0
                    If Side1 < 0 Then
                        fDistInterFAFP = fDistInterFAFP * SideDef(pPtInter, fFAFPDir - 90.0, pFAFPptPrj)

                        fBetta = 360.0 - fBetta
                        fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle - fBetta)))

                        fL0Min = fTurnR / System.Math.Tan(DegToRad(0.5 * fBetta))

                        If Side0 < 0 Then
                            fTmp = (fL0Min - fdL) * System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                            fDMin = Max(ReturnDistanceInMeters(pPtIFMin, pPtInter), fTmp)
                        Else
                            fDMin = (fL0Min - fdL) * System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle))
                        End If

                        fTmp = (fDRMin + fdL) * System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) / System.Math.Sin(DegToRad(fBetta))
                        fDMin = fTmp

                        fAvaIF_FAFPMin = Max(fDMin - fDistInterFAFP, fIF_FAFPMin)
                        fDMin = fAvaIF_FAFPMin + fDistInterFAFP
                        '========================
                        fTmp = (fDRMax + fdL) * System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) / System.Math.Sin(DegToRad(fBetta))
                        fDMax = fTmp

                        fAvaIF_FAFPMax = Min(fDMax - fDistInterFAFP, fIF_FAFPMax)
                        fDMax = fAvaIF_FAFPMax + fDistInterFAFP
                        fAvaDRMin = fDMin * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) - fdL
                        fAvaDRMax = fDMax * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle - fBetta)) - fdL
                    Else
                        iErrorCode = -3
                        MessageBox.Show(My.Resources.str00303)
                        Return
                    End If
                End If
            End If
        Else
            iErrorCode = -4
            MessageBox.Show(My.Resources.str00303)
            Return
        End If

        If Math.Abs(fDMax - fDMin) < distEps Then
            If fDMax < fDMin Then
                fTmp = fDMin
                fDMin = fDMax
                fDMax = fTmp
            End If
        ElseIf fDMax < fDMin Then
            iErrorCode = -5
            MessageBox.Show(My.Resources.str00303)
            Return
        End If

        TextBox107_Interval.Low = fAvaIF_FAFPMin
        TextBox107_Interval.High = fAvaIF_FAFPMax
        TextBox107.Enabled = fAvaIF_FAFPMax - fAvaIF_FAFPMin > distEps

        TextBox108_Interval.Low = fAvaDRMin
        TextBox108_Interval.High = fAvaDRMax
        TextBox108.Enabled = fAvaDRMax - fAvaDRMin > distEps

        ToolTip1.SetToolTip(TextBox107, My.Resources.str50233 + My.Resources.str00221 + CStr(ConvertDistance(TextBox107_Interval.Low, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TextBox107_Interval.High, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit)
        TextBox107.Text = CStr(ConvertDistance(TextBox107_Interval.Low, eRoundMode.NEAREST))

        ToolTip1.SetToolTip(TextBox108, My.Resources.str50234 + My.Resources.str00221 + CStr(ConvertDistance(TextBox108_Interval.Low, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TextBox108_Interval.High, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit)
        TextBox108.Text = CStr(ConvertDistance(TextBox108_Interval.Low, eRoundMode.NEAREST))
        '================================================================================================
        pPtIFMin = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fAvaIF_FAFPMin)
        pPtIFMax = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fAvaIF_FAFPMax)

        fInvDRDir = fDRDir + 180.0

        '=======================================================================================================
        Dim Ix As Integer
        Dim pPtCntr As IPoint
        Dim pPtFc As IPoint
        Dim pPt0 As IPoint
        Dim pPt1 As IPoint

        pPtFc = GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj

        '=======================================================================================================
        fTmp = fDistInterFAFP - fInterSign * fAvaIF_FAFPMin
        If ComboBox003.SelectedIndex = 0 Then
            fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fBetta)))
        Else
            fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fInterSign * fBetta)))
        End If

        fDR = fTmp * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fInterSign * fBetta)) - fdL
        '=======================================================================================================
        pPt0 = PointAlongPlane(pPtIFMin, fInvDRDir, fDR)
        pPtCntr = PointAlongPlane(pPt0, fInvDRDir + 90.0 * iDRType, fTurnR)
        pPt1 = PointAlongPlane(pPtCntr, fInitDir + 90.0 * iDRType, fTurnR)

        fAvaTPMin = ReturnDistanceInMeters(pPt1, pPtFc) - 200       '?????????
        Side0 = SideDef(pPt1, fInitDir - 90.0, pPtFc)

        '=======================================================================================================
        fTmp = fDistInterFAFP - fInterSign * fAvaIF_FAFPMax
        If ComboBox003.SelectedIndex = 0 Then
            fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fBetta)))
        Else
            fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fInterSign * fBetta)))
        End If
        fDR = fTmp * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fInterSign * fBetta)) - fdL

        TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
        '=======================================================================================================
        pPt0 = PointAlongPlane(pPtIFMax, fInvDRDir, fDR)
        pPtCntr = PointAlongPlane(pPt0, fInvDRDir + 90.0 * iDRType, fTurnR)
        pPt1 = PointAlongPlane(pPtCntr, fInitDir + 90.0 * iDRType, fTurnR)

        fAvaTPMax = ReturnDistanceInMeters(pPt1, pPtFc) + 200       '?????????
        Side1 = SideDef(pPt1, fInitDir - 90.0, pPtFc)

        If Side0 = 0 Then Side0 = -1
        If Side1 = 0 Then Side1 = 1
        Ix = System.Math.Round(0.5 * (Side0 + 1))

        If Side0 = Side1 Then
            Select Case Side0
                Case -1
                    ComboBox106.SelectedIndex = 0
                Case 1
                    ComboBox106.SelectedIndex = 1
            End Select
            ComboBox106Locked = True
            LockControl(ComboBox106, ComboBox106Locked)

            TextBox109_Interval(1 - Ix).Low = 0.0
            TextBox109_Interval(1 - Ix).High = 0.0
            If fAvaTPMin < fAvaTPMax Then
                ToolTip1.SetToolTip(TextBox109, My.Resources.str50235 + My.Resources.str00221 +
                 CStr(ConvertDistance(fAvaTPMin, 3)) + " " + DistanceConverter(DistanceUnit).Unit +
                 My.Resources.str00222 + CStr(ConvertDistance(fAvaTPMax, 1)) + " " + DistanceConverter(DistanceUnit).Unit)

                TextBox109_Interval(Ix).Low = System.Math.Round(fAvaTPMin + 0.4999)
                TextBox109_Interval(Ix).High = System.Math.Round(fAvaTPMax - 0.4999)
            Else
                ToolTip1.SetToolTip(TextBox109, My.Resources.str50235 + My.Resources.str00221 +
                 CStr(ConvertDistance(fAvaTPMax, 3)) + " " + DistanceConverter(DistanceUnit).Unit +
                 My.Resources.str00222 + CStr(ConvertDistance(fAvaTPMin, 1)) + " " + DistanceConverter(DistanceUnit).Unit)

                TextBox109_Interval(Ix).Low = System.Math.Round(fAvaTPMax + 0.4999)
                TextBox109_Interval(Ix).High = System.Math.Round(fAvaTPMin - 0.4999)
            End If
        Else
            ComboBox106Locked = False
            LockControl(ComboBox106, ComboBox106Locked)
            If ComboBox003.SelectedIndex = 1 Then Ix = 1 - Ix

            If Side0 > 0 Then
                TextBox109_Interval(1 - Ix).Low = 0.0
                TextBox109_Interval(1 - Ix).High = System.Math.Round(fAvaTPMin - 0.4999)
                TextBox109_Interval(Ix).Low = 0.0
                TextBox109_Interval(Ix).High = System.Math.Round(fAvaTPMax - 0.4999)
            Else
                TextBox109_Interval(Ix).Low = 0.0
                TextBox109_Interval(Ix).High = System.Math.Round(fAvaTPMin - 0.4999)
                TextBox109_Interval(1 - Ix).Low = 0.0
                TextBox109_Interval(1 - Ix).High = System.Math.Round(fAvaTPMax - 0.4999)
            End If

            ToolTip1.SetToolTip(TextBox109, My.Resources.str50235 +
            My.Resources.str00221 + CStr(ConvertDistance(TextBox109_Interval(0).High, 3)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str50236 +
            My.Resources.str00222 + CStr(ConvertDistance(TextBox109_Interval(1).High, 1)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str50237)
        End If
        '================================================================================================
        If OptionButton101.Checked Then
            TextBox107_Validating(TextBox107, New System.ComponentModel.CancelEventArgs())
        ElseIf OptionButton102.Checked Then
            TextBox108_Validating(TextBox108, New System.ComponentModel.CancelEventArgs())
        ElseIf CheckBox101.Checked Then
            TextBox109_Validating(TextBox109, New System.ComponentModel.CancelEventArgs())
            FillComboBox108(fInitDir)
        Else
            FillComboBox108(fInitDir)
            TextBox109_Validating(TextBox109, New System.ComponentModel.CancelEventArgs())
        End If
    End Sub

    Private Sub TextBox107_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox107.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox107_Validating(TextBox107, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox107.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox107_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox107.Validating
        Dim Side As Integer
        Dim fD As Double
        Dim fdL As Double
        Dim fTmp As Double
        Dim fInvDRDir As Double
        Dim pPt0 As IPoint
        Dim pPt2 As IPoint
        Dim pPtCntr As IPoint

        If TextBox107.ReadOnly Then Return
        If Not OptionButton101.Checked Then Return
        If Not IsNumeric(TextBox107.Text) Then Return

        fTmp = DeConvertDistance(CDbl(TextBox107.Text))
        fIF_FAFPDist = fTmp
        If fIF_FAFPDist < TextBox107_Interval.Low Then fIF_FAFPDist = TextBox107_Interval.Low
        If fIF_FAFPDist > TextBox107_Interval.High Then fIF_FAFPDist = TextBox107_Interval.High
        If fIF_FAFPDist <> fTmp Then TextBox107.Text = CStr(ConvertDistance(fIF_FAFPDist, eRoundMode.NEAREST))

        fD = fDistInterFAFP - fInterSign * fIF_FAFPDist
        If ComboBox003.SelectedIndex = 0 Then
            fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fBetta)))
        Else
            fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fInterSign * fBetta)))
        End If
        fDR = fD * System.Math.Sin(DegToRad(fBetta)) / System.Math.Sin(DegToRad(fDRInterceptAngle + fInterSign * fBetta)) - fdL
        TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

        TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
        '====================================================================================================
        fInvDRDir = fDRDir + 180.0
        pPt2 = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPDist)
        pPt0 = PointAlongPlane(pPt2, fInvDRDir, fDR)

        pPtCntr = PointAlongPlane(pPt0, fInvDRDir + 90.0 * iDRType, fTurnR)
        pTPtPrj = PointAlongPlane(pPtCntr, m_fInitDir + 90.0 * iDRType, fTurnR)
        pTPtPrj.Z = TurnPtAltitude

        fTPFcDist = ReturnDistanceInMeters(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, pTPtPrj)

        TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

        Side = SideDef(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, m_fInitDir - 90.0, pTPtPrj)
        If Side = 0 Then Side = 1

        ComboBox106.SelectedIndex = 1 - System.Math.Round(0.5 * (Side + 1))
        GetValidTPIntersectFacilities()
        '====================================================================================================
        CreateNomTrack()
    End Sub

    Private Sub TextBox108_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox108.Validating
        Dim Side As Integer
        Dim fD As Double
        Dim fdL As Double
        Dim fTmp As Double
        Dim fInvDRDir As Double
        Dim pPt0 As IPoint
        Dim pPt2 As IPoint
        Dim pPtCntr As IPoint

        If TextBox108.ReadOnly Then Return
        If Not OptionButton102.Checked Then Return
        If Not IsNumeric(TextBox108.Text) Then Return
        fDR = DeConvertDistance(CDbl(TextBox108.Text))

        fTmp = fDR
        If fDR < TextBox108_Interval.Low Then fDR = TextBox108_Interval.Low
        If fDR > TextBox108_Interval.High Then fDR = TextBox108_Interval.High
        If fDR <> fTmp Then TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

        If ComboBox003.SelectedIndex = 0 Then
            fdL = fTurnR / System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fBetta)))
        Else
            fdL = fTurnR * System.Math.Tan(DegToRad(0.5 * (fDRInterceptAngle + fInterSign * fBetta)))
        End If
        fD = (fDR + fdL) * System.Math.Sin(DegToRad(fDRInterceptAngle + fInterSign * fBetta)) / System.Math.Sin(DegToRad(fBetta))

        fIF_FAFPDist = fInterSign * (fDistInterFAFP - fD)
        TextBox107.Text = CStr(ConvertDistance(fIF_FAFPDist, eRoundMode.NEAREST))

        TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())
        '====================================================================================================
        fInvDRDir = fDRDir + 180.0
        pPt2 = PointAlongPlane(pFAFPptPrj, fFAFPDir + 180.0, fIF_FAFPDist)
        pPt0 = PointAlongPlane(pPt2, fInvDRDir, fDR)

        pPtCntr = PointAlongPlane(pPt0, fInvDRDir + 90.0 * iDRType, fTurnR)
        pTPtPrj = PointAlongPlane(pPtCntr, m_fInitDir + 90.0 * iDRType, fTurnR)
        pTPtPrj.Z = TurnPtAltitude

        fTPFcDist = ReturnDistanceInMeters(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, pTPtPrj)
        TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

        Side = SideDef(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, m_fInitDir - 90.0, pTPtPrj)
        If Side = 0 Then Side = 1
        ComboBox106.SelectedIndex = 1 - System.Math.Round(0.5 * (Side + 1))
        '====================================================================================================
        GetValidTPIntersectFacilities()
        CreateNomTrack()
    End Sub

    Private Sub TextBox109_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox109.Validating
        Dim Ix As Long
        Dim fTmp As Double
        Dim fDir As Double
        Dim fDist As Double
        Dim fAlpha As Double
        Dim fInvDRDir As Double

        Dim pPt0 As IPoint
        Dim pPtFc As IPoint
        Dim pPtCntr As IPoint
        Dim pConstruct As IConstructPoint

        If TextBox109.ReadOnly Then Return
        If Not OptionButton103.Checked Then Return
        If Not IsNumeric(TextBox109.Text) Then Return
        If ComboBox103.SelectedIndex < 0 Then Return

        fTPFcDist = DeConvertDistance(CDbl(TextBox109.Text))
        fTmp = fTPFcDist

        Ix = ComboBox106.SelectedIndex

        If fTPFcDist < TextBox109_Interval(Ix).Low Then fTPFcDist = TextBox109_Interval(Ix).Low
        If fTPFcDist > TextBox109_Interval(Ix).High Then fTPFcDist = TextBox109_Interval(Ix).High
        If fTPFcDist <> fTmp Then TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

        fInvDRDir = fDRDir + 180.0

        pPtFc = GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj
        pTPtPrj = PointAlongPlane(pPtFc, m_fInitDir + 180.0 * (1 - Ix), fTPFcDist)
        pTPtPrj.Z = TurnPtAltitude

        fTPFcDist = ReturnDistanceInMeters(GuidanceNavs(ComboBox103.SelectedIndex).pPtPrj, pTPtPrj)
        TextBox109.Text = CStr(ConvertDistance(fTPFcDist, eRoundMode.NEAREST))

        pPtCntr = PointAlongPlane(pTPtPrj, m_fInitDir - 90.0 * iDRType, fTurnR)
        pPt0 = PointAlongPlane(pPtCntr, fInvDRDir - 90.0 * iDRType, fTurnR)

        If Not CheckBox001.Checked Then
            pConstruct = IFPoint.pPtPrj
            pConstruct.ConstructAngleIntersection(pFAFPptPrj, DegToRad(fFAFPDir), pPt0, DegToRad(fInvDRDir))

            IFPoint.pPtPrj.Z = IFAltitude ' FAPAltitude	'fProcAlt
            fIF_FAFPDist = ReturnDistanceInMeters(pFAFPptPrj, IFPoint.pPtPrj)
            TextBox107.Text = CStr(ConvertDistance(fIF_FAFPDist, eRoundMode.NEAREST))
        Else
            fDist = ReturnDistanceInMeters(pPtCntr, IFPoint.pPtPrj)
            fDir = ReturnAngleInDegrees(pPtCntr, IFPoint.pPtPrj)

            fAlpha = RadToDeg(ArcSin(fTurnR / fDist))

            fDRDir = Modulus(fDir - iDRType * fAlpha, 360.0#)
            IFPoint.pPtPrj.M = fDRDir

            fDRInterceptAngle = Modulus((fDRDir - fFAFPDir) * iTurnDir, 360.0#)
            TextBox110.Text = CStr(Math.Round(fDRInterceptAngle, 2))

            'fAvaTPMin = ReturnDistanceInMeters(pPt1, pPtFc)
            'ComboBox104_Click
            'TextBox106_Validate False

            fTmp = Dir2Azt(pTPtPrj, fDRDir) - CurrADHP.MagVar
            TextBox111.Text = CStr(Math.Round(fTmp, 2))
        End If

        fDR = ReturnDistanceInMeters(pPt0, IFPoint.pPtPrj)
        TextBox108.Text = CStr(ConvertDistance(fDR, eRoundMode.NEAREST))

        TextBox101_Interval.High = TextBox101_Interval.Low + fIF_FAFPDist * arImDescent_Max.Value
        TextBox101_Validating(TextBox101, New System.ComponentModel.CancelEventArgs())

        GetValidTPIntersectFacilities()
        CreateNomTrack()
    End Sub

    Private Sub FocusStepCaption(ByVal StIndex As Integer)
        Dim I As Integer

        For I = 0 To 1
            PageLabel(I).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
            PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
        Next

        PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
        PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

        Me.Text = My.Resources.str50000 + "  [" + MultiPage1.TabPages.Item(StIndex).Text + "]"      '+ " " + My.Resources.str00818
    End Sub

    Private Sub SaveAccuracy(RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader)
        AccurRep = New ReportFile()

        AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + My.Resources.str00805)
        'AccurRep.H1(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00805)
        AccurRep.WriteMessage(My.Resources.str00159 + " " + My.Resources.str00160 + " - " + RepFileTitle + ": " + My.Resources.str00805)
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteHeader(pReport)
        AccurRep.Param("Distance accuracy", _settings.DistancePrecision, DistanceConverter(DistanceUnit).Unit)
        AccurRep.Param("Angle accuracy", _settings.AnglePrecision, "degrees")

        AccurRep.WriteMessage()
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteMessage("=================================================")
        AccurRep.WriteMessage()

        Dim GuidNav As NavaidData
        Dim IntersectNav As NavaidData

        'Leg 0 =================================================================================

        GuidNav = GuidanceNavs(ComboBox103.SelectedIndex)
        IntersectNav = TPIntersectNavs(ComboBox107.SelectedIndex)

        SaveFixAccurasyInfo(AccurRep, pTPtPrj, "IAF", GuidNav, IntersectNav)

        'Leg 2 =================================================================================
        GuidNav = StartPoint.GuidanceNav
        IntersectNav = IFIntersectNavs(ComboBox105.SelectedIndex)

        'SaveFixAccurasyInfo(AccurRep, IFPoint.pPtPrj, IIf(CheckBox001.Checked, "IAF", "IF"), GuidNav, IntersectNav, True)
        SaveFixAccurasyInfo(AccurRep, IFPoint.pPtPrj, "IF", GuidNav, IntersectNav, True)

        '=============================================================================================================

        AccurRep.CloseFile()
    End Sub

    Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        ProtRep = New ReportFile()

        'ProtRep.RefHeight = 0.0
        ProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + My.Resources.str00815)
        ProtRep.WriteMessage(My.Resources.str00159 + " " + My.Resources.str00160 + " - " + RepFileTitle + ": " + My.Resources.str00815)
        'ProtRep.WriteMessage()
        'ProtRep.WriteMessage(RepFileTitle)
        ProtRep.WriteHeader(pReport, True)
        ProtRep.WriteMessage()
        ProtRep.WriteMessage()

        '=====================================================================================================
        ProtRep.Page(DeadApproachReport.MultiPage1.TabPages.Item(0).Text)
        ProtRep.HTMLMessage("[ " + DeadApproachReport.MultiPage1.TabPages.Item(0).Text + " ]") '
        ProtRep.HTMLMessage()
        ProtRep.lListView = DeadApproachReport.ListView1
        ProtRep.WriteTabData(IntermAAObstList)
        ProtRep.WriteMessage()
        '=====================================================================================================
        ProtRep.Page(DeadApproachReport.MultiPage1.TabPages.Item(1).Text)
        ProtRep.HTMLMessage("[ " + DeadApproachReport.MultiPage1.TabPages.Item(1).Text + " ]") '
        ProtRep.HTMLMessage()
        ProtRep.lListView = DeadApproachReport.ListView2
        ProtRep.WriteTabData(TurnAObstList)
        ProtRep.WriteMessage()
        '=====================================================================================================
        ProtRep.Page(DeadApproachReport.MultiPage1.TabPages.Item(2).Text)
        ProtRep.HTMLMessage("[ " + DeadApproachReport.MultiPage1.TabPages.Item(2).Text + " ]") '
        ProtRep.HTMLMessage()
        ProtRep.lListView = DeadApproachReport.ListView3
        ProtRep.WriteTabData(InitialAAObstList)
        ProtRep.WriteMessage()
        '=====================================================================================================

        ProtRep.CloseFile()
    End Sub

    Private Sub SaveGeometry(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim TrackPoint1 As ReportPoint
        Dim TrackPoint2 As ReportPoint
        GeomRep = New ReportFile()

        GeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + My.Resources.str00811)

        GeomRep.WriteMessage(My.Resources.str00159 + " " + My.Resources.str00160 + " - " + RepFileTitle + ": " + My.Resources.str00811)
        'GeomRep.WriteMessage()
        'GeomRep.WriteMessage(RepFileTitle)
        GeomRep.WriteHeader(pReport)
        GeomRep.WriteMessage()
        GeomRep.WriteMessage()

        'Turn Pt++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        Dim fInvDRDir As Double
        Dim pPt As IPoint
        Dim pPt0 As IPoint
        Dim pPt1 As IPoint
        Dim pPtCntr As IPoint
        Dim pPolyline As IPolyline
        Dim pTrackPoints As IPointCollection

        fInvDRDir = fDRDir + 180.0

        pPt1 = PointAlongPlane(IFPoint.pPtPrj, fInvDRDir, fDR)
        pPt1.M = fDRDir
        pPtCntr = PointAlongPlane(pPt1, fInvDRDir + 90.0# * iDRType, fTurnR)

        pPt0 = PointAlongPlane(pPtCntr, m_fInitDir + 90.0# * iDRType, fTurnR)
        pPt0.M = m_fInitDir

        pTrackPoints = New ESRI.ArcGIS.Geometry.Multipoint
        pTrackPoints.AddPoint(pPt0)
        pTrackPoints.AddPoint(pPt1)
        pTrackPoints.AddPoint(IFPoint.pPtPrj)
        pPolyline = CalcTrajectoryFromMultiPoint(pTrackPoints)

        fNomLenght = pPolyline.Length

        'DrawPolyLine(pPolyline, RGB(0, 255, 0), 2)
        'Application.DoEvents()

        pPt = ToGeo(pTPtPrj)

        TrackPoint1.Description = "TP"
        TrackPoint1.Lat = DegreeToString(pPt.Y, Degree2StringMode.DMSLat)
        TrackPoint1.Lon = DegreeToString(pPt.X, Degree2StringMode.DMSLon)

        TrackPoint1.Direction = TextBox106.Text
        TrackPoint1.PDG = NO_DATA_VALUE
        TrackPoint1.Altitude = TurnPtAltitude         ' pTPtPrj.Z

        TrackPoint1.Radius = DeConvertDistance(CDbl(TextBox010.Text))
        TrackPoint1.Turn = 0

        TrackPoint1.CenterLat = ""
        TrackPoint1.CenterLon = ""

        TrackPoint1.ToNext = fNomLenght             ' fDR '
        TrackPoint1.TurnAngle = NO_DATA_VALUE
        TrackPoint1.TurnArcLen = NO_DATA_VALUE

        'IF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        pPt = ToGeo(IFPoint.pPtPrj)

        TrackPoint2.Description = "IF"
        TrackPoint2.Lat = DegreeToString(pPt.Y, Degree2StringMode.DMSLat)
        TrackPoint2.Lon = DegreeToString(pPt.X, Degree2StringMode.DMSLon)

        TrackPoint2.Direction = TextBox001.Text
        TrackPoint2.PDG = NO_DATA_VALUE
        TrackPoint2.Altitude = IFAltitude             ' IFPoint.pPtPrj.Z

        TrackPoint2.Radius = NO_DATA_VALUE
        TrackPoint2.Turn = 0

        TrackPoint2.CenterLat = ""
        TrackPoint2.CenterLon = ""

        TrackPoint2.ToNext = NO_DATA_VALUE
        TrackPoint2.TurnAngle = NO_DATA_VALUE
        TrackPoint2.TurnArcLen = NO_DATA_VALUE

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        GeomRep.WritePoint(TrackPoint1)
        GeomRep.WritePoint(TrackPoint2)

        GeomRep.CloseFile()
    End Sub

    Private Sub CInitialDeadApproach_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F1 Then
            HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
            e.Handled = True
        End If
    End Sub

End Class
