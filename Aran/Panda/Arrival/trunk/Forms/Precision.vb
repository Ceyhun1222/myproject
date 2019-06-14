Option Strict Off
Option Explicit On

Imports System.Collections.Generic
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Features
Imports Aran.Aim.Objects
Imports Aran.Aim.Enums
Imports Aran.Geometries
Imports Aran.Queries
Imports Aran.Aim
Imports System.ComponentModel
Imports ESRI.ArcGIS.Geometry
Imports Aran.AranEnvironment
Imports Aran.Aim.Data
Imports Aran.Metadata.Utils

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CPrecisionFrm
    Inherits System.Windows.Forms.Form
    Implements IProcedureForm

    Private Const MinCalcAreaRadius As Double = 150000.0
    Const MaxTraceSegments As Integer = 100
    Const InnerSurfaceHeightMin As Double = 45.0
    Const InnerSurfaceHeightMax As Double = 60.0

#Region "Variables"
    'Graphic managment ======================================
    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

    Private FAPElem As ESRI.ArcGIS.Carto.IElement
    Private SOCElem As ESRI.ArcGIS.Carto.IElement
    Private Sec0Elem As ESRI.ArcGIS.Carto.IElement
    Private IFFIXElem As ESRI.ArcGIS.Carto.IElement
    Private pMAPtElem As ESRI.ArcGIS.Carto.IElement
    Private MAPtCLineElem As ESRI.ArcGIS.Carto.IElement

    Private Plane15Elem As ESRI.ArcGIS.Carto.IElement
    Private FAP15FIXElem As ESRI.ArcGIS.Carto.IElement
    Private ZContinuedElem As ESRI.ArcGIS.Carto.IElement
    Private ReducedTIAElem As ESRI.ArcGIS.Carto.IElement
    Private TerminationFIXElem As ESRI.ArcGIS.Carto.IElement

    Private IntermediateFullAreaElem As ESRI.ArcGIS.Carto.IElement
    Private IntermediatePrimAreaElem As ESRI.ArcGIS.Carto.IElement

    Public pTraceElem As ArcGIS.Carto.IElement
    Public TracElement As ESRI.ArcGIS.Carto.IElement
    Public pProtectElem As ArcGIS.Carto.IElement

    'Point objects ======================================
    Private ptLH As ESRI.ArcGIS.Geometry.IPoint
    Private FlyBy As ESRI.ArcGIS.Geometry.IPoint
    Private ptFAP As ESRI.ArcGIS.Geometry.IPoint
    Private IFprj As ESRI.ArcGIS.Geometry.IPoint
    Private pMAPt As ESRI.ArcGIS.Geometry.IPoint
    Private PtSOC As ESRI.ArcGIS.Geometry.IPoint
    Private m_OutPt As ESRI.ArcGIS.Geometry.IPoint
    Private NOutPt As ESRI.ArcGIS.Geometry.IPoint
    Private FOutPt As ESRI.ArcGIS.Geometry.IPoint
    Private NJoinPt As ESRI.ArcGIS.Geometry.IPoint
    Private FJoinPt As ESRI.ArcGIS.Geometry.IPoint
    Private TerFixPnt As ESRI.ArcGIS.Geometry.IPoint
    Private FicTHRprj As ESRI.ArcGIS.Geometry.IPoint
    Private TurnFixPnt As ESRI.ArcGIS.Geometry.IPoint
    Private PtCoordCntr As ESRI.ArcGIS.Geometry.IPoint

    Private pTermPt As ESRI.ArcGIS.Geometry.IPoint
    Private pStraightNomLine As IPolyline

    'polylines ==========================================
    Private KK As ESRI.ArcGIS.Geometry.IPolyline
    Private K1K1 As ESRI.ArcGIS.Geometry.IPolyline
    Private pIFLine As ESRI.ArcGIS.Geometry.IPolyline
    Private KKFixMax As ESRI.ArcGIS.Geometry.IPolyline
    Private pFAPLine As ESRI.ArcGIS.Geometry.IPolyline
    Private mPoly As ESRI.ArcGIS.Geometry.IPolyline
    Private LeftLine As ESRI.ArcGIS.Geometry.IPointCollection
    Private RightLine As ESRI.ArcGIS.Geometry.IPointCollection

    'polygons and others ======================================
    Private pIFTolerArea As ESRI.ArcGIS.Geometry.IPolygon
    Private pFAPTolerArea As ESRI.ArcGIS.Geometry.IPolygon
    Private pTPTolerArea As ESRI.ArcGIS.Geometry.IPolygon
    Private pTermFIXTolerArea As ESRI.ArcGIS.Geometry.IPolygon

    Private FAP15FIX As ESRI.ArcGIS.Geometry.IPolygon
    Private pFIXPoly_6 As ESRI.ArcGIS.Geometry.IPolygon
    Private pReducedTIA As ESRI.ArcGIS.Geometry.IPolygon
    Private pDistPolygon As ESRI.ArcGIS.Geometry.IPolygon
    Private SecL As ESRI.ArcGIS.Geometry.IPointCollection
    Private SecR As ESRI.ArcGIS.Geometry.IPointCollection
    Private Prim As ESRI.ArcGIS.Geometry.IPointCollection
    Private pFixedTIAPart As ESRI.ArcGIS.Geometry.IPolygon
    Private pIFPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private SecPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private pCircle As ESRI.ArcGIS.Geometry.IPointCollection
    Private ZNR_Poly As ESRI.ArcGIS.Geometry.IPointCollection
    Private m_TurnArea As ESRI.ArcGIS.Geometry.IPointCollection
    Private BaseArea As ESRI.ArcGIS.Geometry.IPointCollection
    Private pFixPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private pFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private pIFFIXPoly As ESRI.ArcGIS.Geometry.IPointCollection
    Private ZContinued As ESRI.ArcGIS.Geometry.IPointCollection
    Private m_BasePoints As ESRI.ArcGIS.Geometry.IPointCollection
    Private MPtCollection As ESRI.ArcGIS.Geometry.IPointCollection
    Private IntermediateFullArea As IPointCollection
    Private IntermediateWorkArea As IPointCollection
    Private IntermediatePrimArea As IPointCollection

    ' Integrals =====================================
    Private bNavFlg As Boolean
    Private CheckState As Boolean
    'Private bUnloadByOk As Boolean
    Private OkBtnEnabled As Boolean
    Private HaveExcluded As Boolean
    Private bHaveSolution As Boolean

    Private IxOCH As Integer
    Private TurnDir As Integer
    Private Category As Integer
    Private CurrPage As Integer
    Private m_IxMinOCH As Integer
    Private IxMaxOCH As Integer
    Private ArrayNum As Integer
    Private PrevCmbRWY As Integer
    Private NReqCorrAngle As Integer
    Private FReqCorrAngle As Integer

    Private Ss As Double
    Private Vs As Double
    Private m_fMOC As Double
    Private m_fIAS As Double
    Private hTMA As Double
    Private fTNH As Double
    Private _hFAP As Double
    Private _hCons As Double
    Private MinDR As Double
    Private _ArDir As Double
    Private XptLH As Double
    Private XptSOC As Double
    Private m_OutDir As Double
    Private _InterMOCH As Double
    Private InnerSurfaceHeight As Double
    Private finalAssessedAltitude As Double

    Private hTurn As Double
    Private fHTurn As Double
    Private fTA_OCH As Double
    Private fRDHOCH As Double
    Private _CurrFAPOCH As Double
    Private fFAPDist As Double
    Private fFarDist As Double
    Private m_DirToNav As Double
    Private fNearDist As Double
    Private fMisAprOCH As Double
    Private fCorrAngle As Double
    Private NCorrAngle As Double
    Private arMinISlen As Double
    Private fBankAngle As Double
    Private fMissAprPDG As Double
    Private DistLLZ2THR As Double
    Private EarlierTPDir As Double
    Private fHalfFAPWidth As Double
    Private TurnAreaMaxd0 As Double
    Private dMAPt2SOC As Double
    Private FAPEarlierToler As Double
    Private fMaxInterLenght As Double
    Private arImDescent_PDG As Double
    Private _TerminationAltitude As Double
    Private fStraightMissedTermHght As Double

    Private pProcedure As InstrumentApproachProcedure

    Private prevText As String
    Private prevCnt As Integer
    Private _IsClosing As Boolean
    Private pTraceSelectElem As ArcGIS.Carto.IElement
    Private pProtectSelectElem As ArcGIS.Carto.IElement

    Private segPDG As Double
    Private TSC As Integer
    Private Trace() As TraceSegment

    'user type ======================================
    Private ILS As NavaidData
    Private Plane15 As D3DPolygone
    Private SelectedRWY As RWYType
    Private WPt0602 As WPT_FIXType
    Private TurnDirector As WPT_FIXType

    'user arrays ==================================================
    Private FixBox0602() As WPT_FIXType
    Private FixBox0901() As WPT_FIXType

    Private RWYList() As RWYType
    Private lADHPList() As ADHPType
    Private FixAngl() As WPT_FIXType
    Private m_TurnIntervals() As LowHigh
    Private InSectList() As NavaidData
    Private hOASPlanes() As D3DPolygone
    Private IFNavDat() As NavaidData
    Private SelectedFixAll() As WPT_FIXType
    Private SelectedFixAngl() As WPT_FIXType
    Private FAPNavDat() As NavaidData
    Private TextBox0507Intervals() As LowHigh
    Private TPInterNavDat(,) As NavaidData
    Private TerInterNavDat() As NavaidData

    'obstacles ====================================================
    Private ObstacleList As ObstacleContainer
    Private MAObstacleList As ObstacleContainer
    Private DetTNHObstacles As ObstacleContainer
    Private ZNRObstacleList As ObstacleContainer
    Private OASObstacleList As ObstacleContainer
    Private ILSObstacleList As ObstacleContainer

    Private IntermObstacleList As ObstacleContainer
    Private lIntermObstacleList As ObstacleContainer
    Private PrecisionObstacleList As ObstacleContainer

    Private WorkObstacleList As ObstacleContainer
    Private TurnAreaObstacleList As ObstacleContainer

    Private HelpContextID As Integer
    Private PrecReportFrm As CPrecReportFrm
    Private ArrivalProfile As CArrivalProfile
    Private ExcludeObstFrm As CExcludeObstFrm
    Private MrkInfoForm As CMrkInfoForm
    Private AddSegmentFrm As AddSegmentForm
    Private _InfoFrm As CInfoFrm
    Private bFormInitialised As Boolean = False

    Private PageLabel() As Label

    Private AccurRep As ReportFile
    Private ProtRep As ReportFile
    Private LogRep As ReportFile
    Private GeomRep As ReportFile

    Dim mUomSpeed As UomSpeed
    Dim mUomHDistance As UomDistance
    Dim mUomVDistance As UomDistanceVertical

    Private screenCapture As IScreenCapture
#End Region


#Region "Utilities"

    Private Sub FocusStepCaption(ByVal StIndex As Integer)
        Dim I As Integer

        For I = 0 To 9
            PageLabel(I).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
            PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
        Next

        PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
        PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

        Me.Text = My.Resources.str00002 + "  [" + MultiPage1.TabPages.Item(StIndex).Text + "]"  '+ " " + My.Resources.str00818 
    End Sub

    Private Sub ClearGraphics()
        Dim I As Integer
        Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement

        On Error Resume Next

        For I = 0 To UBound(OASPlanesCat23Element)
            If Not OASPlanesCat23Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat23Element(I))
        Next I

        For I = 0 To UBound(OASPlanesCat1Element)
            If Not OASPlanesCat1Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat1Element(I))
        Next I

        For I = 0 To UBound(ILSPlanesElement)
            If Not ILSPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(ILSPlanesElement(I))
        Next I

        For I = 0 To UBound(OFZPlanesElement)
            If Not OFZPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(OFZPlanesElement(I))
        Next I

        If Not IntermediatePrimAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediatePrimAreaElem)
        If Not IntermediateFullAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateFullAreaElem)

        If Not TerminationFIXElem Is Nothing Then
            pGroupElement = TerminationFIXElem
            For I = 0 To pGroupElement.ElementCount - 1
                If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
            Next I
        End If

        If Not ReducedTIAElem Is Nothing Then pGraphics.DeleteElement(ReducedTIAElem)
        If Not ZContinuedElem Is Nothing Then pGraphics.DeleteElement(ZContinuedElem)
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
        If Not FAP15FIXElem Is Nothing Then pGraphics.DeleteElement(FAP15FIXElem)
        If Not Plane15Elem Is Nothing Then pGraphics.DeleteElement(Plane15Elem)
        If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
        If Not pMAPtElem Is Nothing Then pGraphics.DeleteElement(pMAPtElem)
        If Not IFFIXElem Is Nothing Then pGraphics.DeleteElement(IFFIXElem)
        If Not K1K1Elem Is Nothing Then pGraphics.DeleteElement(K1K1Elem)
        If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
        If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
        If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
        If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
        If Not SOCElem Is Nothing Then pGraphics.DeleteElement(SOCElem)
        If Not FAPElem Is Nothing Then pGraphics.DeleteElement(FAPElem)
        If Not KKElem Is Nothing Then pGraphics.DeleteElement(KKElem)

        If Not FIXElem Is Nothing Then
            pGroupElement = FIXElem
            For I = 0 To pGroupElement.ElementCount - 1
                If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
            Next I
        End If

		Functions.SafeDeleteElement(pTraceSelectElem)
		Functions.SafeDeleteElement(pProtectSelectElem)
		Functions.SafeDeleteElement(pTraceElem)
		Functions.SafeDeleteElement(pProtectElem)

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        On Error GoTo 0
    End Sub

    Private Function FAPDist2hFAP(ByVal Dist As Double) As Double
        Dim TanGPA As Double
        Dim fCorrectedDist As Double
        Dim dD As Double

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        dD = arCurvatureCoeff.Value * (arAbv_Treshold.Value - ILS.GP_RDH) / TanGPA
        fCorrectedDist = Dist + dD
        Return ILS.GP_RDH + TanGPA * fCorrectedDist + 0.0785 * 0.000001 * fCorrectedDist * fCorrectedDist
    End Function

    Private Function hFAP2FAPDist(ByVal Hrel As Double) As Double
        Dim TanGPA As Double
        Dim kA As Double
        Dim dD As Double

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        dD = arCurvatureCoeff.Value * (arAbv_Treshold.Value - ILS.GP_RDH) / TanGPA
        kA = 0.0785 * 0.000001

        Return (System.Math.Sqrt(TanGPA * TanGPA + 4 * kA * (Hrel - ILS.GP_RDH)) - TanGPA) / (2 * kA) - dD
    End Function

    Private Sub ApplayOptions()
        Dim NewPoly As IPointCollection
        Dim tmpPoly1 As IPointCollection
        Dim pTopoOper As ITopologicalOperator2
        Dim TurnWPT_Nav As NavaidData

        If MultiPage1.SelectedIndex <> 7 Then Return

        If OptionButton0601.Checked Then Return

        TurnWPT_Nav = WPT_FIXToNavaid(TurnDirector)

        CreateNavaidZone(TurnWPT_Nav, m_OutDir, FicTHRprj, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

        If OptionButton0603.Checked And ((TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.NDB) Or (TurnDirector.TypeCode = eNavaidType.TACAN)) Then
            tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon

            tmpPoly1.AddPoint(m_TurnArea.Point(0))
            tmpPoly1.AddPoint(m_TurnArea.Point(m_TurnArea.PointCount - 1))

            NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, m_TurnArea, m_OutPt, m_OutDir, tmpPoly1)
            EarlierTPDir = ReturnAngleInDegrees(NewPoly.Point(NewPoly.PointCount - 1), NewPoly.Point(NewPoly.PointCount - 2))

            NewPoly = RemoveAgnails(NewPoly)

            pTopoOper = tmpPoly1
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            pTopoOper = NewPoly
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()
            'DrawPolygon tmpPoly1, 0
            'DrawPolygon BasePoints, 0

            NewPoly = pTopoOper.Union(m_BasePoints)
            pTopoOper = NewPoly

            If Modulus((m_OutDir - _ArDir) * TurnDir, 360.0) > 90.0 Then
                '    If SideFrom2Angle(ArDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
                NewPoly = pTopoOper.Union(tmpPoly1)
                pTopoOper = NewPoly
            End If

            tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon()
            tmpPoly1.AddPointCollection(m_TurnArea)

            pTopoOper = tmpPoly1
            pTopoOper.IsKnownSimple_2 = False
            pTopoOper.Simplify()

            BaseArea = RemoveHoles(pTopoOper.Union(NewPoly))

            '    If OptionButton601 Then
            '        Set pTopoOper = BaseArea
            '        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
            '    End If

            '    Set pTopoOper = pCircle
            '    Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
            BaseArea = PolygonIntersection(pCircle, BaseArea)
            '================================================================
            On Error Resume Next
            If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
            If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
            If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
            If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
            If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
            On Error GoTo 0

            TurnAreaElem = DrawPolygon(BaseArea, 255, , False)

            tmpPoly1 = PolygonIntersection(pCircle, Prim)
            '    Set tmpPoly1 = pTopoOper.Intersect(Prim, esriGeometry2Dimension)
            PrimElem = DrawPolygon(tmpPoly1, RGB(128, 128, 0), , False)


            tmpPoly1 = PolygonIntersection(pCircle, SecL)
            '    Set tmpPoly1 = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
            SecLElem = DrawPolygon(tmpPoly1, RGB(0, 0, 255), , False)

            tmpPoly1 = PolygonIntersection(pCircle, SecR)
            '    Set tmpPoly1 = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
            SecRElem = DrawPolygon(tmpPoly1, RGB(0, 0, 255), , False)

            '    If ButtonControl3State Then
            pGraphics.AddElement(TurnAreaElem, 0)
            TurnAreaElem.Locked = True
            '    End If

            '    If ButtonControl4State Then
            pGraphics.AddElement(PrimElem, 0)
            PrimElem.Locked = True
            pGraphics.AddElement(SecLElem, 0)
            SecLElem.Locked = True
            pGraphics.AddElement(SecRElem, 0)
            SecRElem.Locked = True
            '    End If

            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            '    RefreshCommandBar mTool, 4
        ElseIf OptionButton0602.Checked Then
            m_OutDir = Azt2Dir(TurnDirector.pPtGeo, CDbl(TextBox0603.Text) + TurnDirector.MagVar)
            UpdateToNavCourse(m_OutDir)
        End If
    End Sub

    Private Sub SecondArea(ByVal iTurnDir As Integer, ByVal pTurnArea As IPointCollection)
        Dim bFlg As Boolean

        Dim SideIn As Integer
        Dim Side0 As Integer
        Dim Side1 As Integer

        Dim TmpDepDir As Double
        Dim OutDist As Double
        Dim InDist As Double
        Dim TmpDir As Double
        Dim DrDir As Double
        Dim Dist0 As Double
        Dim Dist1 As Double

        Dim ptCutInner0 As IPoint
        Dim ptCutInner1 As IPoint
        Dim ptCutOuter As IPoint
        Dim ptCutInner As IPoint
        Dim ptFar As IPoint
        Dim ptOut As IPoint
        Dim ptTmp As IPoint
        Dim ptCut As IPoint
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone

        Dim GeneralArea As IPointCollection
        Dim pConstruct As IConstructPoint
        Dim pIArea As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim TurnPT As NavaidData

        TurnPT = WPT_FIXToNavaid(TurnDirector)

        If OptionButton0603.Checked Or OptionButton0602.Checked Then
            ptOut = MPtCollection.Point(MPtCollection.PointCount - 1)
            If OptionButton0602.Checked Then DrDir = MPtCollection.Point(1).M
        Else
            MessageBox.Show("Invalid options")
            Return
        End If

        Prim = New ESRI.ArcGIS.Geometry.Polygon
        SecL = New ESRI.ArcGIS.Geometry.Polygon
        SecR = New ESRI.ArcGIS.Geometry.Polygon

        CreateNavaidZone(TurnPT, m_OutDir, FicTHRprj, Ss, Vs, 5.0 * VOR.Range, 5.0 * VOR.Range, SecL, SecR, Prim)

        pClone = pTurnArea
        GeneralArea = pClone.Clone

        pIArea = New ESRI.ArcGIS.Geometry.Polygon

        pIArea.AddPoint(SecR.Point(5))
        pIArea.AddPoint(SecR.Point(0))
        pIArea.AddPoint(SecR.Point(1))

        pIArea.AddPoint(SecL.Point(2))
        pIArea.AddPoint(SecL.Point(3))
        pIArea.AddPoint(SecL.Point(4))

        pTopo = pIArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTopo = GeneralArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        If TurnDirector.TypeCode = eNavaidType.NDB Then
            Dist0 = NDB.Range * 10.0
        Else 'If (TurnWPT.TypeCode = eNavaidType.CodeVOR) Or (TurnWPT.TypeCode = eNavaidType.CodeTACAN) Then
            Dist0 = VOR.Range * 10.0
        End If

        ptFar = PointAlongPlane(TurnDirector.pPtPrj, m_OutDir, Dist0)
        pCutter.ToPoint = ptFar
        pCutter.FromPoint = PointAlongPlane(TurnDirector.pPtPrj, m_OutDir + 180.0, Dist0)
        ClipByLine(GeneralArea, pCutter, SecL, SecR, pTmpPoly)

        pTopo = SecR
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        'DrawPolygon SecR, 0
        'DrawPolygon SecL, 128
        'DrawPolygon IArea, 255

        SecR = pTopo.Difference(pIArea)

        pTopo = SecL
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        SecL = pTopo.Difference(pIArea)

        SecR = RemoveFars(SecR, ptFar)
        SecL = RemoveFars(SecL, ptFar)

        If CheckBox0801.Checked Then
            If OptionButton0603.Checked Then
                'Na FIX
                '==================== Outer ==================
                ptCutOuter = New ESRI.ArcGIS.Geometry.Point
                pConstruct = ptCutOuter
                pConstruct.ConstructAngleIntersection(ptOut, DegToRad(m_OutDir), FOutPt, DegToRad(m_OutDir + 90.0))
                SideIn = SideDef(ptOut, m_OutDir + 90.0, ptCutOuter)

                If SideIn < 0 Then
                    ptCutOuter.PutCoords(ptOut.X, ptOut.Y)
                End If

                OutDist = SideIn * ReturnDistanceInMeters(ptOut, ptCutOuter)
                '==================== Inner ==================
                ptCutInner0 = New ESRI.ArcGIS.Geometry.Point
                ptCutInner1 = New ESRI.ArcGIS.Geometry.Point

                pConstruct = ptCutInner0
                pConstruct.ConstructAngleIntersection(ptOut, DegToRad(m_OutDir), NOutPt, DegToRad(m_OutDir + 90.0))

                If ptCutInner0.IsEmpty() Then ptCutInner0.PutCoords(ptCutOuter.X, ptCutOuter.Y)

                pConstruct = ptCutInner1
                pConstruct.ConstructAngleIntersection(ptOut, DegToRad(m_OutDir), NOutPt, DegToRad(_ArDir))

                If ptCutInner1.IsEmpty() Then ptCutInner1.PutCoords(ptCutInner0.X, ptCutInner0.Y)

                Side0 = SideDef(ptOut, m_OutDir + 90.0, ptCutInner0)
                Dist0 = Side0 * ReturnDistanceInMeters(ptOut, ptCutInner0)

                Side1 = SideDef(ptOut, m_OutDir + 90.0, ptCutInner1)
                Dist1 = Side1 * ReturnDistanceInMeters(ptOut, ptCutInner1)

                TmpDir = ReturnAngleInDegrees(ptCutInner0, ptCutInner1)

                If Side0 < 0 Then ptCutInner0.PutCoords(ptOut.X, ptOut.Y)

                If Side1 < 0 Then ptCutInner1.PutCoords(ptOut.X, ptOut.Y)

                bFlg = SubtractAngles(m_OutDir, TmpDir) > 0.5
                If bFlg Then
                    ptCutInner = ptCutInner0 'outazt+90
                    InDist = Dist0
                Else
                    ptCutInner = ptCutInner1 'depdir
                    InDist = Dist1
                End If
                '==================== Select suitable point ==================
                If OutDist > InDist Then
                    ptCut = ptCutInner
                Else
                    ptCut = ptCutOuter
                End If

                '==================== Outer ==================
                pCutter.FromPoint = ptCut
                pCutter.ToPoint = PointAlongPlane(ptCut, m_OutDir - 90.0 * TurnDir, 2.0 * RModel)

                If iTurnDir > 0 Then
                    ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecR = RemoveFars(pTmpPoly, ptFar)
                    End If
                Else
                    ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecL = RemoveFars(pTmpPoly, ptFar)
                    End If
                End If

                '==================== Inner ==================
                If bFlg Then
                    pCutter.ToPoint = PointAlongPlane(ptCut, m_OutDir + 90.0 * TurnDir, 2.0 * RModel)
                Else
                    pCutter.ToPoint = PointAlongPlane(ptCut, _ArDir + 180.0, 2.0 * RModel)
                    pCutter.FromPoint = PointAlongPlane(ptCut, _ArDir, 2.0 * RModel)

                    If SideDef(pCutter.ToPoint, m_OutDir, pCutter.FromPoint) <> TurnDir Then
                        pCutter.ReverseOrientation()
                    End If
                End If

                If iTurnDir > 0 Then
                    ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecL = RemoveFars(pTmpPoly, ptFar)
                    End If
                Else
                    ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecR = RemoveFars(pTmpPoly, ptFar)
                    End If
                End If
            Else 'Na curs NAV
                '==================== Outer ==================
                pCutter.FromPoint = FlyBy

                If SideDef(FlyBy, DrDir, ptOut) = TurnDir Then
                    pCutter.ToPoint = PointAlongPlane(FlyBy, m_OutDir - 90.0 * TurnDir, 2.0 * RModel)
                Else
                    pCutter.ToPoint = PointAlongPlane(FlyBy, DrDir - 90.0 * TurnDir, 2.0 * RModel)
                End If

                If iTurnDir > 0 Then
                    ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecR = RemoveFars(pTmpPoly, ptFar)
                    End If
                Else
                    ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecL = RemoveFars(pTmpPoly, ptFar)
                    End If
                End If
                '==================== Inner ==================
                ptTmp = PointAlongPlane(FlyBy, _ArDir, 2.0 * RModel)

                If SideDef(FlyBy, m_OutDir, ptTmp) = TurnDir Then
                    TmpDepDir = _ArDir + 180.0
                Else
                    TmpDepDir = _ArDir
                End If

                If SideDef(FlyBy, DrDir, ptOut) <> TurnDir Then
                    TmpDir = m_OutDir + 90.0 * TurnDir
                Else
                    TmpDir = DrDir + 90.0 * TurnDir
                End If

                If SideFrom2Angle(TmpDir, TmpDepDir) = TurnDir Then
                    TmpDir = TmpDepDir
                End If

                pCutter.ToPoint = PointAlongPlane(FlyBy, TmpDir, 2.0 * RModel)

                If iTurnDir > 0 Then
                    ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecL = RemoveFars(pTmpPoly, ptFar)
                    End If
                Else
                    ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                    If Not pTmpPoly.IsEmpty() Then
                        SecR = RemoveFars(pTmpPoly, ptFar)
                    End If
                End If
            End If
        End If

        If CheckBox0802.Checked Then
            ptCut = New ESRI.ArcGIS.Geometry.Point
            pConstruct = ptCut

            TmpDir = m_OutDir - arSecAreaCutAngl.Value * TurnDir
            pConstruct.ConstructAngleIntersection(TurnDirector.pPtPrj, DegToRad(m_OutDir), NJoinPt, DegToRad(TmpDir))

            pCutter.FromPoint = ptCut
            pCutter.ToPoint = PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * RModel)

            If iTurnDir > 0 Then
                ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecL = RemoveFars(pTmpPoly, ptFar)
                End If
            Else
                ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecR = RemoveFars(pTmpPoly, ptFar)
                End If
            End If
        End If

        If CheckBox0805.Checked Then
            ptCut = New ESRI.ArcGIS.Geometry.Point
            pConstruct = ptCut

            TmpDir = m_OutDir + arSecAreaCutAngl.Value * TurnDir
            pConstruct.ConstructAngleIntersection(TurnDirector.pPtPrj, DegToRad(m_OutDir), FJoinPt, DegToRad(TmpDir))

            pCutter.FromPoint = ptCut
            pCutter.ToPoint = PointAlongPlane(ptCut, TmpDir + 180.0, 2.0 * RModel)

            If iTurnDir > 0 Then
                ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecR = RemoveFars(pTmpPoly, ptFar)
                End If
            Else
                ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecL = RemoveFars(pTmpPoly, ptFar)
                End If
            End If
        End If

        ptCut = TurnDirector.pPtPrj

        If CheckBox0803.Checked Then
            TmpDir = ReturnAngleInDegrees(ptCut, NJoinPt)

            pCutter.FromPoint = ptCut
            pCutter.ToPoint = PointAlongPlane(ptCut, TmpDir, 2.0 * RModel)

            If iTurnDir > 0 Then
                ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecL = RemoveFars(pTmpPoly, ptFar)
                End If
            Else
                ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecR = RemoveFars(pTmpPoly, ptFar)
                End If
            End If
        End If

        If CheckBox0806.Checked Then
            TmpDir = ReturnAngleInDegrees(ptCut, FJoinPt)

            pCutter.FromPoint = ptCut
            pCutter.ToPoint = PointAlongPlane(ptCut, TmpDir, 2.0 * RModel)

            If iTurnDir > 0 Then
                ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecR = RemoveFars(pTmpPoly, ptFar)
                End If
            Else
                ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
                If Not pTmpPoly.IsEmpty() Then
                    SecL = RemoveFars(pTmpPoly, ptFar)
                End If
            End If
        End If

        If OptionButton0402.Checked Then
            pTopo = SecR
            SecR = pTopo.Difference(pFixPoly)

            pTopo = SecL
            SecL = pTopo.Difference(pFixPoly)
        End If

        '============================================
        pClone = m_TurnArea
        pTmpPoly = pClone.Clone

        pTopo = pTmpPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTopo = SecL
        SecL = pTopo.Difference(pTmpPoly)

        pTopo = SecR
        SecR = pTopo.Difference(pTmpPoly)
        '============================================

        SecL = RemoveFars(SecL, ptFar)
        SecR = RemoveFars(SecR, ptFar)

        SecL = RemoveAgnails(SecL)
        SecR = RemoveAgnails(SecR)

        On Error Resume Next
        If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
        PrimElem = Nothing
        If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
        If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)

        SecLElem = DrawPolygon(SecL, RGB(0, 0, 255))
        SecLElem.Locked = True

        SecRElem = DrawPolygon(SecR, RGB(0, 0, 255))
        SecRElem.Locked = True
        On Error GoTo 0

        pTopo = SecR
        SecPoly = pTopo.Union(SecL)

        'Dim pBag As IGeometryCollection
        'Set pBag = New GeometryBag
        '    pBag.AddGeometry SecL
        '    pBag.AddGeometry SecR

        'Set SecPoly = New Polygon
        'Set Topo = SecPoly
        '    Topo.ConstructUnion pBag
    End Sub

    Private Function CreateMAPolygon() As Double
        Dim C As Integer
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim O As Integer
        Dim P As Integer
        Dim MAObCnt As Integer
        Dim MAPrCnt As Integer
        Dim WrObCnt As Integer
        Dim WrPrCnt As Integer

        Dim Ix As Integer

        Dim fTmp As Double
        Dim Dist As Double
        Dim lDist As Double
        Dim CoTanZ As Double
        Dim TanGPA As Double
        Dim CoTanGPA As Double

        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolygon1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        CoTanGPA = 1.0 / TanGPA
        CoTanZ = 1.0 / fMissAprPDG

        pPolygon = ReArrangePolygon(wOASPlanes(eOAS.ZPlane).Poly, FicTHRprj, _ArDir)
        ZContinued = New ESRI.ArcGIS.Geometry.Polygon

        'fTmp = CurrADHP.MinTMA - ptLHPrj.Z
        'If fTmp < 600.0 Then fTmp = 600.0
        '    Dist = (fTmp - hCons) / (fMissAprPDG * Cos(DegToRad(arMA_SplayAngle.Value)))

        lDist = (600.0 - _hCons) / fMissAprPDG

        If lDist < MinCalcAreaRadius Then lDist = MinCalcAreaRadius
        Dist = lDist / System.Math.Cos(DegToRad(arMA_SplayAngle.Value))

        ZContinued.AddPoint(pPolygon.Point(1))
        ZContinued.AddPoint(PointAlongPlane(pPolygon.Point(1), _ArDir - arMA_SplayAngle.Value, Dist))
        ZContinued.AddPoint(PointAlongPlane(pPolygon.Point(2), _ArDir + arMA_SplayAngle.Value, Dist))
        ZContinued.AddPoint(pPolygon.Point(2))
        ZContinued.AddPoint(pPolygon.Point(1))

        pTopo = ZContinued
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pPolygon = ReArrangePolygon(ZContinued, FicTHRprj, _ArDir)
        ptTmp = PointAlongPlane(FicTHRprj, _ArDir, RModel)
        pPolygon1 = ReArrangePolygon(wOASPlanes(eOAS.CommonPlane).Poly, ptTmp, _ArDir + 180.0)

        LeftLine = New ESRI.ArcGIS.Geometry.Polyline
        RightLine = New ESRI.ArcGIS.Geometry.Polyline

        LeftLine.AddPoint(pPolygon1.Point(1))
        LeftLine.AddPoint(pPolygon.Point(3))
        LeftLine.AddPoint(pPolygon.Point(2))

        RightLine.AddPoint(pPolygon1.Point(pPolygon1.PointCount - 2))
        RightLine.AddPoint(pPolygon.Point(0))
        RightLine.AddPoint(pPolygon.Point(1))
        '============================================================
        pFullPoly = New ESRI.ArcGIS.Geometry.Polygon
        pFullPoly.AddPointCollection(RightLine)
        pFullPoly.AddPoint(LeftLine.Point(2))
        pFullPoly.AddPoint(LeftLine.Point(1))
        pFullPoly.AddPoint(LeftLine.Point(0))

        pTopo = pFullPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        PtCoordCntr = New ESRI.ArcGIS.Geometry.Point
        PtCoordCntr.PutCoords(0.5 * (LeftLine.Point(0).X + RightLine.Point(0).X), 0.5 * (LeftLine.Point(0).Y + RightLine.Point(0).Y))
        '    PtCoordCntr.Z = 300#
        XptLH = ReturnDistanceInMeters(FicTHRprj, PtCoordCntr)
        '============================================================
        On Error Resume Next
        If Not ZContinuedElem Is Nothing Then pGraphics.DeleteElement(ZContinuedElem)
        On Error GoTo 0

        ZContinuedElem = DrawPolygon(ZContinued, 0)
        ZContinuedElem.Locked = True
        GetIntermObstacleList(ObstacleList, MAObstacleList, ZContinued, FicTHRprj, _ArDir)

        Dim ZSurfaceOrigin As Double
        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        MAObCnt = UBound(MAObstacleList.Obstacles)
        MAPrCnt = UBound(MAObstacleList.Parts)
        For I = 0 To MAPrCnt
            MAObstacleList.Parts(I).Plane = eOAS.NonPrec
            MAObstacleList.Parts(I).Dist = -MAObstacleList.Parts(I).Dist
            MAObstacleList.Parts(I).EffectiveHeight = (MAObstacleList.Parts(I).Height * CoTanZ + (ZSurfaceOrigin + MAObstacleList.Parts(I).Dist)) / (CoTanZ + CoTanGPA)
            MAObstacleList.Parts(I).fTmp = -(MAObstacleList.Parts(I).Dist + ZSurfaceOrigin) * fMissAprPDG
            MAObstacleList.Parts(I).hPenet = MAObstacleList.Parts(I).Height - MAObstacleList.Parts(I).fTmp
            'MAObstacleList.Parts(I).Flags = 1
            'MAObstacleList.Parts(I).ReqH = MAObstacleList.Parts(I).Height + arMA_FinalMOC.Value
        Next I
        '===========================================
        WrObCnt = UBound(WorkObstacleList.Obstacles)
        WrPrCnt = UBound(WorkObstacleList.Parts)

        If MAPrCnt >= 0 Then
            ReDim Preserve MAObstacleList.Parts(WrPrCnt + MAPrCnt + 1)
            ReDim Preserve MAObstacleList.Obstacles(WrObCnt + MAObCnt + 1)
        ElseIf WrPrCnt >= 0 Then
            ReDim MAObstacleList.Parts(WrPrCnt)
            ReDim MAObstacleList.Obstacles(WrObCnt)
        Else
            ReDim MAObstacleList.Parts(-1)
            ReDim MAObstacleList.Obstacles(-1)
        End If

        For I = 0 To WrObCnt
            WorkObstacleList.Obstacles(I).NIx = -1
        Next

        For I = 0 To WrPrCnt
            If WorkObstacleList.Parts(I).EffectiveHeight >= WorkObstacleList.Parts(I).Height Then Continue For
            MAPrCnt += 1
            MAObstacleList.Parts(MAPrCnt) = WorkObstacleList.Parts(I)

            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                MAObCnt += 1
                MAObstacleList.Obstacles(MAObCnt) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                MAObstacleList.Obstacles(MAObCnt).PartsNum = 0
                ReDim MAObstacleList.Obstacles(MAObCnt).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = MAObCnt
            End If

            MAObstacleList.Parts(MAPrCnt).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
            MAObstacleList.Parts(MAPrCnt).Index = MAObstacleList.Obstacles(MAObstacleList.Parts(MAPrCnt).Owner).PartsNum
            MAObstacleList.Obstacles(MAObstacleList.Parts(MAPrCnt).Owner).Parts(MAObstacleList.Obstacles(MAObstacleList.Parts(MAPrCnt).Owner).PartsNum) = MAPrCnt
            MAObstacleList.Obstacles(MAObstacleList.Parts(MAPrCnt).Owner).PartsNum += 1
        Next I

        If MAPrCnt < 0 Then
            ReDim MAObstacleList.Obstacles(-1)
            ReDim MAObstacleList.Parts(-1)
        Else
            ReDim Preserve MAObstacleList.Parts(MAPrCnt)
            ReDim Preserve MAObstacleList.Obstacles(MAObCnt)
        End If
        '===========================================

        '        Dist = MAObstacleList(i).Dist - ZSurfaceOrigin
        '        Hz = fMissAprPDG * Dist
        '        MAObstacleList(i).hPent = MAObstacleList(i).Height - Hz
        '        MAObstacleList(i).hPent =MAObstacleList(i).Height-
        '        If MAObstacleList(i).hPent > 0.0 Then
        fMisAprOCH = _CurrFAPOCH
        Ix = -1

        For I = 0 To MAPrCnt
            If (MAObstacleList.Parts(I).hPenet > 0.0) And (MAObstacleList.Parts(I).ReqH <= _hFAP) Then
                fTmp = (MAObstacleList.Parts(I).Height * CoTanZ + (ZSurfaceOrigin + MAObstacleList.Parts(I).Dist)) / (CoTanZ + CoTanGPA)
                MAObstacleList.Parts(I).EffectiveHeight = fTmp

                If CheckBox0001.Checked And (MAObstacleList.Parts(I).Dist >= -ZSurfaceOrigin) Then
                    MAObstacleList.Parts(I).ReqOCH = MAObstacleList.Parts(I).Height + m_fMOC
                Else
                    MAObstacleList.Parts(I).ReqOCH = Min(MAObstacleList.Parts(I).Height, MAObstacleList.Parts(I).EffectiveHeight) + m_fMOC
                End If

                If MAObstacleList.Parts(I).ReqOCH > fMisAprOCH Then
                    fMisAprOCH = MAObstacleList.Parts(I).ReqOCH
                    Ix = I
                End If
            End If
        Next I

        Dim socDist As Double
        Dim maptDist As Double

        socDist = (fMisAprOCH - m_fMOC) * CoTanGPA - ZSurfaceOrigin
        PtSOC = PointAlongPlane(FicTHRprj, _ArDir, -socDist)
        PtSOC.Z = fMisAprOCH - m_fMOC
        PtSOC.M = _ArDir
        XptSOC = ReturnDistanceInMeters(PtSOC, PtCoordCntr) * SideDef(PtCoordCntr, _ArDir + 90.0, PtSOC)
        TextBox0307.Text = CStr(ConvertDistance(socDist, eRoundMode.NEAREST))

        maptDist = (fMisAprOCH - ILS.GP_RDH) * CoTanGPA
        pMAPt = PointAlongPlane(FicTHRprj, _ArDir, -maptDist)
        pMAPt.Z = fMisAprOCH
        pMAPt.M = _ArDir

        dMAPt2SOC = (m_fMOC - ILS.GP_RDH) * CoTanGPA + ZSurfaceOrigin

        ComboBox0302_SelectedIndexChanged(ComboBox0302, New System.EventArgs()) ''"""""""

        On Error Resume Next
        If Not SOCElem Is Nothing Then pGraphics.DeleteElement(SOCElem)
        If Not pMAPtElem Is Nothing Then pGraphics.DeleteElement(pMAPtElem)
        '    Set SOCElem = DrawPoint(PtSOC, RGB(128, 128, 0))
        SOCElem = DrawPointWithText(PtSOC, "SOC", WPTColor)
        SOCElem.Locked = True

        '    Set pMAPtElem = DrawPoint(pMAPt, RGB(192, 127, 192))
        pMAPtElem = DrawPointWithText(pMAPt, "MAPt", WPTColor)
        pMAPtElem.Locked = True
        On Error GoTo 0

        CreateMAPolygon = fMisAprOCH
        TextBox0303.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.NEAREST))
        If Ix >= 0 Then
            TextBox0306.Text = MAObstacleList.Obstacles(MAObstacleList.Parts(Ix).Owner).UnicalName
        Else
            TextBox0306.Text = "-"
        End If

        If (fMisAprOCH > _CurrFAPOCH) Or (fMisAprOCH > _hFAP) Then
            TextBox0303.ForeColor = Color.FromArgb(&HFF0000)
        Else
            TextBox0303.ForeColor = Color.FromArgb(0)
        End If

        PrecReportFrm.FillPage06(MAObstacleList)

        '    If N > -1 Then
        '        Sort MAObstacleList, 0
        '        ReDim DetOCHObstacles(0 To N)
        '        fTmp = fCurrOCH
        '        J = -1
        '
        '        For I = N To 0 Step -1
        '            If MAObstacleList(I).ReqOCH > fTmp Then
        '                fTmp = MAObstacleList(I).ReqOCH
        '                J = J + 1
        '                DetOCHObstacles(J) = MAObstacleList(I)
        '                DetOCHObstacles(J).Dist = XptLH - DetOCHObstacles(J).Dist
        '            End If
        '        Next I
        '
        '        If J >= 0 Then
        '            ReDim Preserve DetOCHObstacles(0 To J)
        '        Else
        '            ReDim DetOCHObstacles(-1)
        '        End If
        '    Else
        '        ReDim DetOCHObstacles(-1)
        '    End If

    End Function

    Private Sub CalcTurnRange(ByRef OCHObstacles As ObstacleContainer)
        Dim TurnRange As Interval

        Dim HAbsTurn As Double
        Dim RTurn As Double
        Dim E As Double
        Dim lTAS As Double
        Dim I As Integer
        Dim N As Integer

        HAbsTurn = FicTHRprj.Z + arMATurnAlt.Value

        lTAS = IAS2TAS(m_fIAS, HAbsTurn, CurrADHP.ISAtC)
        RTurn = Bank2Radius(fBankAngle, lTAS)
        E = PI * 3.6 * depWS.Value * lTAS / (System.Math.Tan(DegToRad(fBankAngle)) * 254.168) / 90.0
        N = UBound(OCHObstacles.Parts)

        For I = 0 To N
            If TurnDir > 0 Then
                TurnRange = CalcSpiralStartPoint(RightLine, OCHObstacles.Parts(I), E, RTurn, _ArDir, TurnDir)
            Else
                TurnRange = CalcSpiralStartPoint(LeftLine, OCHObstacles.Parts(I), E, RTurn, _ArDir, TurnDir)
            End If

            OCHObstacles.Parts(I).TurnDistL = TurnRange.Left
            OCHObstacles.Parts(I).TurnAngleL = TurnRange.Right
        Next I

        PrecReportFrm.FillPage07(OCHObstacles)
    End Sub

#End Region

#Region "Form"
    Public Sub New()
        'MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        HaveExcluded = False
        _IsClosing = False
        ReDim Trace(MaxTraceSegments)
        screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.InstrumentApproachProcedure.ToString())

        Dim I As Integer

        bFormInitialised = True
        PrecReportFrm = New CPrecReportFrm
        ArrivalProfile = New CArrivalProfile
        ExcludeObstFrm = New CExcludeObstFrm
        MrkInfoForm = New CMrkInfoForm
        _InfoFrm = New CInfoFrm
        AddSegmentFrm = New AddSegmentForm(screenCapture)

        OASPlanesCat1State = True
        ILSPlanesState = False

        'Application.HelpFile = "Panda.chm"
        Me.HelpContextID = 10100

        InnerSurfaceHeight = InnerSurfaceHeightMin
        TextBox0012.Text = CStr(ConvertHeight(InnerSurfaceHeight, eRoundMode.NEAREST))
        'bUnloadByOk = False
        arImDescent_PDG = arImDescent_Max.Value
        TextBox0203.Text = CStr(100.0 * arImDescent_PDG)

        TextBox0206.Text = CStr(ConvertDistance(arIFHalfWidth.Value, eRoundMode.NEAREST))

        RModel = MinCalcAreaRadius
        _hCons = arMATurnAlt.Value

        pGraphics = GetActiveView().GraphicsContainer
        CurrPage = 0

        PrevCmbRWY = -1
        'PrevCmb002 = -1
        ReDim TextBox0507Intervals(-1)
        '===========================================================
        PrevBtn.Text = My.Resources.str00150
        NextBtn.Text = My.Resources.str00151
        OkBtn.Text = My.Resources.str00152
        CancelBtn.Text = My.Resources.str00153
        ReportButton.Text = My.Resources.str00154
        ProfileBtn.Text = My.Resources.str00155

        ComboBox0005.Items.Add(CStr(2.0))
        ComboBox0005.Items.Add(CStr(2.5))
        ComboBox0005.Items.Add(CStr(3.0))
        ComboBox0005.Items.Add(CStr(4.0))
        ComboBox0005.Items.Add(CStr(5.0))

        ComboBox0103.Items.Add(My.Resources.str00223 + ":")
        ComboBox0103.Items.Add(My.Resources.str00224 + ":")

        ComboBox0104.Items.Add(My.Resources.str10113)
        ComboBox0104.Items.Add(My.Resources.str10118)

        ComboBox0202.Items.Add(My.Resources.str00223 + ":")
        ComboBox0202.Items.Add(My.Resources.str00224 + ":")

        ComboBox0301.Items.Add(My.Resources.str00223 + ":")
        ComboBox0301.Items.Add(My.Resources.str00224 + ":")

        ComboBox0401.Items.Add(My.Resources.str00226)
        ComboBox0401.Items.Add(My.Resources.str00225)

        ComboBox0502.Items.Add(My.Resources.str10113)
        ComboBox0502.Items.Add(My.Resources.str10118)

        ComboBox0503.Items.Add(My.Resources.str16008)   '???????
        ComboBox0503.Items.Add(My.Resources.str16010)

        ComboBox0903.Items.Add(My.Resources.str10113)
        ComboBox0903.Items.Add(My.Resources.str10118)

        FIXElem = Nothing
        '===========================================================
        'N = UBound(ADHPList)
        'If N >= 0 Then
        '    ReDim lADHPList(N)
        'Else
        '    ReDim lADHPList(-1)
        'End If
        'J = -1
        'ComboBox0006.Items.Clear()

        'For I = 0 To N
        '    '    Set AIXMILSList = pObjectDir.GetILSNavaidEquipmentList(ADHPList(I).ID)
        '    '    If AIXMILSList.Count > 0 Then
        '    '    If ADHPList(I).Index = 3 Then
        '    J = J + 1

        '    lADHPList(J) = ADHPList(I)
        '    ComboBox0006.Items.Add(lADHPList(J).Name)
        '    '    End If
        'Next I

        'If J >= 0 Then
        '    ReDim Preserve lADHPList(J)
        'Else
        '    MsgBox("There is no ILS for the specified ADHP list.", MsgBoxStyle.Critical, "PANDA")
        '    Me.Close()
        '    Return
        'End If

        ComboBox0601.Items.Clear()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        MultiPage1.Tag = "1"
        MultiPage1.SelectedIndex = 0
        MultiPage1.Tag = "0"

        'SetFormParented(Handle.ToInt32)

        PrecReportFrm.SetBtn(ReportButton)
        'PrecReportFrm.SetUnVisible -1
        '===========================================================
        Text = My.Resources.str00002
        MultiPage1.TabPages.Item(0).Text = My.Resources.str00201
        MultiPage1.TabPages.Item(1).Text = My.Resources.str00202
        MultiPage1.TabPages.Item(2).Text = My.Resources.str00203
        MultiPage1.TabPages.Item(3).Text = My.Resources.str00109
        MultiPage1.TabPages.Item(4).Text = My.Resources.str00110
        MultiPage1.TabPages.Item(5).Text = My.Resources.str00111
        MultiPage1.TabPages.Item(6).Text = My.Resources.str00113
        MultiPage1.TabPages.Item(7).Text = My.Resources.str00114
        MultiPage1.TabPages.Item(8).Text = My.Resources.str00115
        MultiPage1.TabPages.Item(9).Text = My.Resources.str00116
        MultiPage1.TabPages.Item(10).Text = My.Resources.str00117
        '=======================================================
        CheckBox0001.Text = My.Resources.str20123
        Label0001_0.Text = My.Resources.str20101
        _Label0001_1.Text = My.Resources.str20102
        _Label0001_2.Text = My.Resources.str20104
        _Label0001_3.Text = My.Resources.str20103
        _Label0001_4.Text = My.Resources.str20105
        _Label0001_5.Text = My.Resources.str20106
        _Label0001_6.Text = My.Resources.str20107
        _Label0001_7.Text = My.Resources.str20115
        _Label0001_12.Text = My.Resources.str00021
        _Label0001_13.Text = My.Resources.str00021
        _Label0001_14.Text = My.Resources.str20109
        _Label0001_15.Text = My.Resources.str20116
        _Label0001_16.Text = My.Resources.str20110
        _Label0001_17.Text = My.Resources.str20111
        _Label0001_18.Text = My.Resources.str20112
        _Label0001_19.Text = My.Resources.str20113
        _Label0001_20.Text = My.Resources.str20114
        Label0001_21.Text = My.Resources.str20108
        _Label0001_25.Text = My.Resources.str00021
        label0001_27.Text = My.Resources.str20120

        '===============================================
        _Label0101_0.Text = My.Resources.str20201

        _Label0101_5.Text = My.Resources.str10405
        CheckBox0101.Text = My.Resources.str20206
        _Label0101_2.Text = My.Resources.str20207

        Frame0103.Text = My.Resources.str20203
        _Label0101_3.Text = My.Resources.str20210
        _Label0101_6.Text = My.Resources.str20211
        '==================================================
        Frame0201.Text = My.Resources.str10509
        Frame0202.Text = My.Resources.str20306
        Frame0203.Text = My.Resources.str20304

        OptionButton0201.Text = My.Resources.str00230
        OptionButton0202.Text = My.Resources.str00231

        label0201_01.Text = My.Resources.str10503
        label0201_03.Text = My.Resources.str20307
        label0201_05.Text = My.Resources.str20308
        label0201_07.Text = My.Resources.str10516   '    IF designator:
        label0201_08.Text = My.Resources.str00613
        label0201_10.Text = My.Resources.str10203
        label0201_15.Text = My.Resources.str20305
        label0201_17.Text = My.Resources.str20316
        label0201_19.Text = My.Resources.str10405

        '=================================================
        _Label0301_1.Text = My.Resources.str20402
        _Label0301_2.Text = My.Resources.str20403
        CheckBox0301.Text = My.Resources.str20404
        _Label0301_4.Text = My.Resources.str10911
        _Label0301_5.Text = My.Resources.str11057
        _Label0301_6.Text = My.Resources.str11057
        _Label0301_7.Text = My.Resources.str20407
        _Label0301_3.Text = My.Resources.str20408

        '_Label0301_13.Text = My.Resources.str16005
        '=================================================
        _Label0401_0.Text = My.Resources.str20501
        _Label0401_2.Text = My.Resources.str10101
        _Label0401_4.Text = My.Resources.str20502
        _Label0401_5.Text = My.Resources.str20102
        _Label0401_8.Text = My.Resources.str10920

        CheckBox0401.Text = My.Resources.str20503
        CheckBox0402.Text = My.Resources.str20507

        Frame0401.Text = My.Resources.str11005  '20504
        OptionButton0401.Text = My.Resources.str11006
        OptionButton0402.Text = My.Resources.str11007
        '=================================================
        _Label0501_0.Text = My.Resources.str20606
        _Label0501_1.Text = My.Resources.str20607
        '_Label0501_2.Text = My.Resources.str20601
        _Label0501_3.Text = My.Resources.str20608
        _Label0501_4.Text = My.Resources.str20609
        _Label0501_5.Text = My.Resources.str20610
        _Label0501_6.Text = My.Resources.str20611
        '_Label0501_7.Text = "THR-TP Dist:"

        _Label0501_8.Text = My.Resources.str20602
        _Label0501_9.Text = My.Resources.str10203

        _Label0501_10.Text = ""
        _Label0501_18.Text = ""

        Frame0501.Text = My.Resources.str12007
        OptionButton0501.Text = My.Resources.str00230
        OptionButton0502.Text = My.Resources.str00231
        '=====================================================
        OptionButton0601.Text = My.Resources.str13001
        OptionButton0602.Text = My.Resources.str13002
        OptionButton0603.Text = My.Resources.str13003
        CheckBox0601.Text = My.Resources.str13007

        _Label0601_0.Text = My.Resources.str13004
        _Label0601_1.Text = My.Resources.str13011
        _Label0601_2.Text = My.Resources.str13010
        _Label0601_5.Text = My.Resources.str13005
        _Label0601_6.Text = My.Resources.str13006
        _Label0601_11.Text = My.Resources.str13012
        _Label0601_12.Text = My.Resources.str00015
        _Label0601_13.Text = My.Resources.str13016

        ComboBox0603.Items.Clear()
        ComboBox0603.Items.Add(My.Resources.str13014)
        ComboBox0603.Items.Add(My.Resources.str13015)
        '======================================================
        Frame0701.Text = My.Resources.str14001
        Frame0702.Text = My.Resources.str14006
        _Label0701_0.Text = My.Resources.str20902
        OptionButton0701.Text = My.Resources.str20903
        OptionButton0702.Text = My.Resources.str20904
        OptionButton0703.Text = My.Resources.str20905
        OptionButton0704.Text = My.Resources.str20903
        OptionButton0705.Text = My.Resources.str20904
        OptionButton0706.Text = My.Resources.str20905
        _Label0701_1.Text = My.Resources.str20907
        '=======================================================
        Frame0801.Text = My.Resources.str21001
        Frame0802.Text = My.Resources.str21005
        CheckBox0801.Text = My.Resources.str21002
        CheckBox0802.Text = My.Resources.str21003
        CheckBox0803.Text = My.Resources.str21004
        CheckBox0804.Text = My.Resources.str21002
        CheckBox0805.Text = My.Resources.str21003
        CheckBox0806.Text = My.Resources.str21006
        '========================================================
        _Label0901_0.Text = My.Resources.str16006       '	'
        _Label0901_2.Text = My.Resources.str16009       '	'
        _Label0901_6.Text = My.Resources.str16005
        _Label0901_8.Text = My.Resources.str10720
        _Label0901_11.Text = My.Resources.str10720
        _Label0901_12.Text = My.Resources.str16007
        _Label0901_14.Text = My.Resources.str12002
        _Label0901_18.Text = My.Resources.str21103
        _Label0901_23.Text = My.Resources.str13016 'LoadResString(13013)+":"

        CheckBox0901.Text = My.Resources.str16011
        OptionButton0901.Text = My.Resources.str10512
        OptionButton0902.Text = My.Resources.str10513
        Frame0901.Text = My.Resources.str16003
        '========================================================

        AddSegmentBtn.Text = My.Resources.str17021
        RemoveSegmentBtn.Text = My.Resources.str17022
        SaveGeometryBtn.Text = My.Resources.str17023

        'CreateLog(My.Resources.str1)
        For I = 0 To 11
            ListView501.Columns(I).Text = Resources.ResourceManager.GetString("str" + (60602 + I).ToString())
        Next

        '========================================================

        _Label0001_8.Text = HeightConverter(HeightUnit).Unit
        _Label0001_10.Text = DistanceConverter(DistanceUnit).Unit
        _Label0001_11.Text = HeightConverter(HeightUnit).Unit
        _Label0001_22.Text = HeightConverter(HeightUnit).Unit
        label0001_28.Text = HeightConverter(HeightUnit).Unit

        _Label0101_4.Text = HeightConverter(HeightUnit).Unit
        _Label0101_7.Text = HeightConverter(HeightUnit).Unit
        _Label0101_8.Text = DistanceConverter(DistanceUnit).Unit
        _Label0101_9.Text = HeightConverter(HeightUnit).Unit
        _Label0101_10.Text = DistanceConverter(DistanceUnit).Unit

        label0201_00.Text = HeightConverter(HeightUnit).Unit
        label0201_04.Text = DistanceConverter(DistanceUnit).Unit
        label0201_06.Text = DistanceConverter(DistanceUnit).Unit
        label0201_09.Text = HeightConverter(HeightUnit).Unit
        label0201_13.Text = DistanceConverter(DistanceUnit).Unit
        label0201_16.Text = DistanceConverter(DistanceUnit).Unit
        label0201_18.Text = HeightConverter(HeightUnit).Unit

        _Label0301_0.Text = HeightConverter(HeightUnit).Unit
        _Label0301_8.Text = HeightConverter(HeightUnit).Unit
        _Label0301_9.Text = HeightConverter(HeightUnit).Unit
        _Label0301_11.Text = DistanceConverter(DistanceUnit).Unit
        _Label0301_14.Text = HeightConverter(HeightUnit).Unit
        _Label0301_15.Text = HeightConverter(HeightUnit).Unit
        label0301_17.Text = DistanceConverter(DistanceUnit).Unit

        _Label0401_7.Text = SpeedConverter(SpeedUnit).Unit
        _Label0401_9.Text = HeightConverter(HeightUnit).Unit

        _Label0501_13.Text = HeightConverter(HeightUnit).Unit
        _Label0501_14.Text = HeightConverter(HeightUnit).Unit
        _Label0501_15.Text = HeightConverter(HeightUnit).Unit
        _Label0501_16.Text = HeightConverter(HeightUnit).Unit
        _Label0501_17.Text = HeightConverter(HeightUnit).Unit

        _Label0601_10.Text = DistanceConverter(DistanceUnit).Unit

        _Label0901_1.Text = DistanceConverter(DistanceUnit).Unit
        _Label0901_3.Text = HeightConverter(HeightUnit).Unit
        _Label0901_5.Text = HeightConverter(HeightUnit).Unit
        _Label0901_7.Text = HeightConverter(HeightUnit).Unit
        _Label0901_10.Text = HeightConverter(HeightUnit).Unit
        _Label0901_13.Text = HeightConverter(HeightUnit).Unit
        _Label0901_21.Text = DistanceConverter(DistanceUnit).Unit
        _Label0901_22.Text = HeightConverter(HeightUnit).Unit

        'CreateLog(My.Resources.str2)

        '' ====== 2007
        PageLabel = {Label1_0, Label1_1, Label1_2, Label1_3, Label1_4, Label1_5, Label1_6, Label1_7, Label1_8, Label1_9, Label1_10}
        PageLabel(0).Text = MultiPage1.TabPages.Item(0).Text
        PageLabel(1).Text = MultiPage1.TabPages.Item(1).Text
        PageLabel(2).Text = MultiPage1.TabPages.Item(2).Text
        PageLabel(3).Text = MultiPage1.TabPages.Item(3).Text
        PageLabel(4).Text = MultiPage1.TabPages.Item(4).Text
        PageLabel(5).Text = MultiPage1.TabPages.Item(5).Text
        PageLabel(6).Text = MultiPage1.TabPages.Item(6).Text
        PageLabel(7).Text = MultiPage1.TabPages.Item(7).Text
        PageLabel(8).Text = MultiPage1.TabPages.Item(8).Text
        PageLabel(9).Text = MultiPage1.TabPages.Item(9).Text
        PageLabel(10).Text = MultiPage1.TabPages.Item(10).Text

        FocusStepCaption(0)

        MultiPage1.Top = -21
        Frame1.Top = Frame1.Top - 21        'VB6.TwipsToPixelsY(5760)
        Me.Height = Me.Height - 21      'VB6.TwipsToPixelsY(7005)

        ShowPanelBtn.Checked = False

        'If ComboBox0006.Items.Count > 0 Then ComboBox0006.SelectedIndex = 0

        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim RWYDATA() As RWYType
        Dim lILS As NavaidData

        ComboBox0001.Items.Clear()
        FillRWYList(RWYDATA, CurrADHP, True)

        N = UBound(RWYDATA)

        If N < 0 Then
            MsgBox(My.Resources.str00300, MsgBoxStyle.Critical, "PANDA")
            _IsClosing = True
            Me.Close()
            Return
        End If

        ReDim RWYList(N)

        J = -1

        For I = 0 To N
            K = GetILS(RWYDATA(I), lILS, CurrADHP)
            If K = 3 Then
                J = J + 1
                RWYList(J) = RWYDATA(I)
                ComboBox0001.Items.Add(RWYDATA(I).Name)
            End If
        Next I

        NextBtn.Enabled = J >= 0

        If J >= 0 Then
            ReDim Preserve RWYList(J)
            ComboBox0001.SelectedIndex = 0
        Else
            ReDim RWYList(-1)
            MsgBox("There is no ILS for the specified ADHP.", MsgBoxStyle.Critical, "PANDA")
            _IsClosing = True
            Me.Close()
            Return
        End If

        If ComboBox0003.SelectedIndex >= 0 Then
            ComboBox0003_SelectedIndexChanged(ComboBox0003, New System.EventArgs())
        Else
            ComboBox0003.SelectedIndex = 0
        End If

        ComboBox0401.SelectedIndex = 0

        ComboBox0302.Items.Clear()
        ComboBox0402.Items.Clear()

        For I = 0 To UBound(EnrouteMOCValues)
            ComboBox0302.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(I), 4)))
            ComboBox0402.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(I), 4)))
        Next I
        ComboBox0302.SelectedIndex = 0
        ComboBox0402.SelectedIndex = 0
    End Sub

    Private Sub CPrecisionFrm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        screenCapture.Rollback()
        DBModule.CloseDB()
        ClearGraphics()
        'If Not bUnloadByOk Then ClearScr()

        PrecReportFrm.Close()
        ArrivalProfile.Close()
        'AddSegmentFrm.Close()

        'CloseLog()
        CurrCmd = 0
    End Sub

    Private Sub CPrecisionFrm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
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

    Property IsClosing As Boolean Implements IProcedureForm.IsClosing
        Get
            Return _IsClosing
        End Get

        Set(value As Boolean)

        End Set
    End Property

    Private Sub ShowPanelBtn_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ShowPanelBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ShowPanelBtn.Checked() Then
            Me.Width = 702
            ShowPanelBtn.Image = My.Resources.bmpHIDE_INFO
        Else
            Me.Width = 517
            ShowPanelBtn.Image = My.Resources.bmpSHOW_INFO
        End If

        If NextBtn.Enabled Then
            NextBtn.Focus()
        Else
            PrevBtn.Focus()
        End If
    End Sub

    Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
        Dim e As New System.EventArgs
        If MultiPage1.Tag = "1" Then Return
        MultiPage1.Tag = "1"

        If CurrPage > MultiPage1.SelectedIndex Then
            MultiPage1.SelectedIndex = CurrPage
            PrevBtn_Click(PrevBtn, e)
        ElseIf CurrPage < MultiPage1.SelectedIndex Then
            MultiPage1.SelectedIndex = CurrPage
            NextBtn_Click(NextBtn, e)
        End If
        MultiPage1.Tag = "0"
    End Sub

    Private Sub PrevBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click
        CurrPage = MultiPage1.SelectedIndex - 1
        screenCapture.Delete()
		Dim I As Integer
		Dim K As Integer
		Dim L As Integer
        Dim M As Integer
        Dim N As Integer

		Select Case CurrPage
			Case 0
				ArrivalProfile.ClearPoints()
				ProfileBtn.Checked = False
				ProfileBtn.Enabled = False
			Case 1
				SafeDeleteElement(IntermediatePrimAreaElem)
				SafeDeleteElement(IFFIXElem)
				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				'    GetActiveView().PartialRefresh esriViewGraphics, Nothing, Nothing
				arImDescent_PDG = arImDescent_Max.Value
				TextBox0203.Text = CStr(100.0 * arImDescent_PDG)
				_hFAP = 0.0
				TextBox0102_Validating(TextBox0102, New System.ComponentModel.CancelEventArgs())
			Case 2
				SafeDeleteElement(MAPtCLineElem)
				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
			Case 3
				N = UBound(WorkObstacleList.Parts)
				M = UBound(WorkObstacleList.Obstacles)
				If N >= 0 Then
					ReDim PrecisionObstacleList.Parts(N)
					ReDim PrecisionObstacleList.Obstacles(M)
				Else
					ReDim PrecisionObstacleList.Parts(-1)
					ReDim PrecisionObstacleList.Obstacles(-1)
				End If

				For I = 0 To M
					WorkObstacleList.Obstacles(I).NIx = -1
				Next

				K = -1
				L = -1

				For I = 0 To N
					If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).IgnoredByUser Then Continue For
					If CheckBox0101.Enabled And CheckBox0101.Checked And (WorkObstacleList.Parts(I).Dist > FAPEarlierToler) Then Continue For
					If (WorkObstacleList.Parts(I).ReqH > _hFAP) Then Continue For

					K = K + 1
					PrecisionObstacleList.Parts(K) = WorkObstacleList.Parts(I)

					If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
						L += 1
						PrecisionObstacleList.Obstacles(L) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
						PrecisionObstacleList.Obstacles(L).PartsNum = 0
						ReDim PrecisionObstacleList.Obstacles(L).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
						WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = L
					End If

					PrecisionObstacleList.Parts(K).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
					PrecisionObstacleList.Parts(K).Index = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum
					PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum) = K
					PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum += 1
				Next I

				If K >= 0 Then
					ReDim Preserve PrecisionObstacleList.Parts(K)
					ReDim Preserve PrecisionObstacleList.Obstacles(L)
				Else
					ReDim PrecisionObstacleList.Parts(-1)
					ReDim PrecisionObstacleList.Obstacles(-1)
				End If

				PrecReportFrm.FillPage04(PrecisionObstacleList)
			Case 6
				CheckState = True
			Case 8
				Dim pGroupElement As ESRI.ArcGIS.Carto.GroupElement


				If Not TerminationFIXElem Is Nothing Then
					pGroupElement = TerminationFIXElem
					For I = 0 To pGroupElement.ElementCount - 1
						SafeDeleteElement(pGroupElement.Element(I))
					Next I
				End If
				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				If OptionButton0601.Checked Or (TurnDirector.TypeCode <= eNavaidType.NONE) Then
					CurrPage = CurrPage - 2
					CheckState = True
				End If
				ArrivalProfile.RemovePoint()
			Case 9
				SafeDeleteElement(pTraceElem)
				SafeDeleteElement(pProtectElem)
				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End Select

		NextBtn.Enabled = CurrPage < MultiPage1.TabPages.Count() - 1
        PrevBtn.Enabled = CurrPage > 0
        MultiPage1.SelectedIndex = CurrPage
        Me.HelpContextID = 10000 + 100 * (MultiPage1.SelectedIndex + 1)
        Me.Activate()

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))
    End Sub

    Private Sub NextBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
        Dim X As Double
        Dim Y As Double
        Dim Dt As Double
        Dim fTmp As Double
        Dim hMax As Double
        Dim CurrOCH As Double
        Dim NextOCH As Double
        Dim fTmpDist As Double
        Dim ZSurfaceOrigin As Double

        Dim I As Integer
        Dim J As Integer
        Dim M As Integer
        Dim N As Integer
		'Dim Side As Integer

		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim OCHInterv As LowHigh

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        ShowPandaBox(Me.Handle.ToInt32)

        Select Case CurrPage
            Case 0
                'FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, MaxNAVDist)
                '    FillNavLists CurrADHP
                '    I = UBound(NavaidList) 'FillNavLists()
                '    If I < 0 Then
                '        HidePandaBox
                '        MessageBox.Show( My.Resources.str505) '"База данных навигационных средств (NAVAID) пуст"
                '        Return
                '    End If

                '==============================================================

                N = UBound(WPTList)
                If N >= 0 Then
                    ReDim FixAngl(N)
                    J = -1
                    For I = 0 To N
                        If (WPTList(I).TypeCode = eNavaidType.VOR) Or (WPTList(I).TypeCode = eNavaidType.NDB) Or (WPTList(I).TypeCode = eNavaidType.TACAN) Then
                            J = J + 1
                            FixAngl(J) = WPTList(I)
                        End If
                    Next I

                    If J >= 0 Then
                        ReDim Preserve FixAngl(J)
                    Else
                        ReDim FixAngl(-1)
                        OptionButton0602.Enabled = False
                        '	ComboBox0601.Enabled = False
                    End If
                Else
                    OptionButton0603.Enabled = False
                    OptionButton0602.Enabled = False
                    ComboBox0601.Enabled = False
                    ReDim FixAngl(-1)
                End If

                '==============================================================
                ArrivalProfile.InitWOFAF(SelectedRWY.Length, 1 + 2 * CShort(Modulus(SelectedRWY.TrueBearing, 360.0) > 180.0), FicTHRprj.Z, FicTHRprj.Z, ProfileBtn)
                ProfileBtn.Enabled = True

                Ss = CDbl(TextBox0005.Text)
                '    If Ss < arSemiSpan.Values(K) Then
                '        If msgbox(My.Resources.str20100, 1) = vbCancel Then '"Полуразмах крыла ВС меньше стандартной. Произвести корректировку?"
                '            Ss = arSemiSpan.Values(K)
                '        End If
                '    End If

                Vs = CDbl(TextBox0006.Text)
                '    If Vs < arVerticalSize.Values(K) Then
                '        If msgbox(My.Resources.str20119, 1) = vbCancel Then '"Вертикальный размер ВС меньше стандартной. Произвести корректировку?"
                '            Vs = arVerticalSize.Values(K)
                '        End If
                '    End If

                CheckBox0101.Checked = False
                ClearScr()

                CreateILSPlanes(FicTHRprj, _ArDir, ILSPlanes)

                AnaliseObstacles(ObstacleList, ILSObstacleList, FicTHRprj, _ArDir, ILSPlanes)

                CalcEffectiveHeights(ILSObstacleList, ILS.GPAngle, fMissAprPDG, ILS.GP_RDH, True) 'arMAS_Climb_Nom.Value
                '	PrecReportFrm.FillPage9 ILSObstacleList

                OAS_DATABase(DistLLZ2THR, ILS.GPAngle, fMissAprPDG, 1, ILS.GP_RDH, Ss, Vs, OASPlanes)
                hMax = arFAPMaxRange.Value * System.Math.Tan(DegToRad(ILS.GPAngle)) + ILS.GP_RDH - arISegmentMOC.Value
                '	CreateOASPlanes ptLHprj, ArDir, GPAngle, ILS.GP_RDH, hMax, OASPlanes, ILSCategory
                CreateOASPlanes(FicTHRprj, _ArDir, hMax, OASPlanes, 1)

                AnaliseObstacles(ObstacleList, OASObstacleList, FicTHRprj, _ArDir, OASPlanes)

                'On Error Resume Next
                'N = UBound(OASPlanes)
                'For I = 0 To N
                '	If Not OASPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesElement(I))
                '	OASPlanesElement(I) = DrawPolygon(OASPlanes(I).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, OASPlanesState)
                '	OASPlanesElement(I).Locked = True
                'Next I
                'CommandBar.isEnabled(CommandBar.OAS) = True
                'On Error GoTo 0

                CommandBar.isEnabled(CommandBar.OAS23) = False
                If ComboBox0002.SelectedIndex > 0 Then
                    OAS_DATABase(DistLLZ2THR, ILS.GPAngle, fMissAprPDG, 2, ILS.GP_RDH, Ss, Vs, OASPlanesCat23)
                    CreateOASPlanes(FicTHRprj, _ArDir, arHOASPlaneCat23, OASPlanesCat23, 2)
                    AnaliseCat23Obstacles(OASObstacleList, FicTHRprj, _ArDir, OASPlanesCat23)

                    'On Error Resume Next
                    N = UBound(OASPlanesCat23)
                    For I = 0 To N
                        SafeDeleteElement(OASPlanesCat23Element(I))
                        OASPlanesCat23Element(I) = DrawPolygon(OASPlanesCat23(I).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, OASPlanesCat23State)
                        OASPlanesCat23Element(I).Locked = True
                    Next I
                    CommandBar.isEnabled(CommandBar.OAS23) = True
                    'On Error GoTo 0
                End If

                'PrecReportFrm.FillPage05(OASObstacleList)
                CalcEffectiveHeights(OASObstacleList, ILS.GPAngle, fMissAprPDG, ILS.GP_RDH) 'arMAS_Climb_Nom.Value
                'PrecReportFrm.FillPage10 OASObstacleList

                M = OASObstacleList.Obstacles.Length
                ReDim WorkObstacleList.Obstacles(M - 1)

                N = OASObstacleList.Parts.Length
                ReDim WorkObstacleList.Parts(N - 1)

                Array.Copy(OASObstacleList.Obstacles, WorkObstacleList.Obstacles, M)
                Array.Copy(OASObstacleList.Parts, WorkObstacleList.Parts, N)

                Sort(WorkObstacleList, 2)
                'PrecReportFrm.FillPage05(WorkObstacleList)
                'NavInSector(ptLHPrj, NavaidList, InSectList)

                WPTInSector(FicTHRprj, WPTList, InSectList)
                bNavFlg = False

                fTmp = FAPDist2hFAP(arOptimalFAFRang.Value)

                If ComboBox0103.SelectedIndex = 0 Then
                    TextBox0102.Text = CStr(ConvertHeight(fTmp + FicTHRprj.Z, eRoundMode.NEAREST))
                Else
                    TextBox0102.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
                End If

                If ComboBox0102.SelectedIndex = 0 Then
                    ComboBox0102_SelectedIndexChanged(ComboBox0102, New System.EventArgs())
                Else
                    ComboBox0102.SelectedIndex = 0
                End If

                If Not bHaveSolution Then
                    HidePandaBox()
                    Return
                End If

                'TextBox0102.Tag = ""    'CStr(arMATurnAlt.Value)
                'TextBox0102_Validate True

                'ComboBox0102.ListIndex = 0
                'TextBox0101.Text = CStr(ConvertDistance(arImRange_Min.Value, eRoundMode.NERAEST))
                'TextBox0101_Validate True

                'TextBox0102.Text = CStr(ConvertHeight(arMATurnAlt.Value, eRoundMode.NERAEST))
                'TextBox0102.Tag = ""
                'fhFAP = -100000.0#

                'MinFAPDist = (arISegmentMOC.Value - ILS.GP_RDH) / Tan(DegToRad(ILS.GPAngle))

                'ComboBox0101.Clear
                'M = -1
                'N = UBound(InSectList)
                'If N >= 0 Then
                '	ReDim FAPNavDat(0 To N)
                '	For I = 0 To N
                '		If (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then Continue For
                '		Side = SideDef(ptLHPrj, ArDir - 90.0, InSectList(I).pPtPrj)
                '		fTmp = Point2LineDistancePrj(ptLHPrj, InSectList(I).pPtPrj, ArDir + 90.0)

                '		If (Side > 0) And (fTmp < arFAPMaxRange.Value) And (fTmp > MinFAPDist) Then
                '                Set lNavDat.pPtPrj = InSectList(I).pPtPrj
                '                Set lNavDat.pPtGeo = InSectList(I).pPtGeo
                '                lNavDat.Name = InSectList(I).Name
                '			lNavDat.Callsign = InSectList(I).CallSign
                '			lNavDat.ID = InSectList(I).ID

                '			lNavDat.TypeCode = InSectList(I).TypeCode
                '			lNavDat.TypeName = InSectList(I).TypeName
                '			'                lNavDat.Index = InSectList(I).Index
                '			lNavDat.ValCnt = -2

                '			lPlane15 = CreatePlane15(lNavDat, fTmp)

                '			M = M + 1
                '			FAPNavDat(M) = lNavDat
                '			ComboBox0101.AddItem FAPNavDat(M).CallSign
                '			End If
                '	Next I
                'Else
                '	ReDim FAPNavDat(-1)
                'End If

                'CheckBox0101.Enabled = M > -1
                'If M > -1 Then
                '	ComboBox0101.ListIndex = 0
                '	ReDim Preserve FAPNavDat(0 To M)
                'Else
                '	ReDim FAPNavDat(-1)
                'End If

                fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), arHOASPlaneCat1, 0.0)

                ComboBox0105.Items.Clear()
                N = UBound(InSectList)

                For I = 0 To N
                    If InSectList(I).TypeCode <> eNavaidType.NONE Then Continue For

                    PrjToLocal(FicTHRprj, _ArDir + 180.0, InSectList(I).pPtPrj, X, Y)

                    If X < fTmpDist Then Continue For
                    If X > arMaxRangeFAS.Value Then Continue For

                    ComboBox0105.Items.Add(InSectList(I))
                Next

                If ComboBox0105.Items.Count > 0 Then
                    CheckBox0102.Enabled = True
                    ComboBox0105.SelectedIndex = 0
                Else
                    CheckBox0102.Enabled = False
                    CheckBox0102.Checked = False
                End If
            Case 1
                ComboBox0201.Items.Clear()
                ComboBox0203.Items.Clear()

                IFNavDat = GetValidIFNavs(ptFAP, FicTHRprj.Z, fNearDist, fFarDist, _ArDir, arImDescent_Max.Value, eNavaidType.LLZ, ILS.pPtPrj)
                'IFNavDat = GetValidIFNavs(ptPlaneFAP, ptLHPrj.Z, fNearDist, fFarDist, ArDir, arImDescent_Max.Value, eNavaidType.LLZ, ILS.pPtPrj)
                N = UBound(IFNavDat)
                M = UBound(InSectList)

                If N >= 0 Then
                    ReDim Preserve IFNavDat(N + M + 1)
                ElseIf M >= 0 Then
                    ReDim IFNavDat(M)
                Else
                    ReDim IFNavDat(-1)
                    HidePandaBox()
                    MessageBox.Show(My.Resources.str00303)  '"IF Nav Array пуст"
                    Return
                End If

                For I = 0 To M
                    fTmp = Point2LineDistanceSigned(ptFAP, InSectList(I).pPtPrj, _ArDir - 90.0)
                    If (fTmp < fNearDist) Or (fTmp > fFarDist) Then Continue For

                    If InSectList(I).TypeCode = eNavaidType.NONE Then
                        ComboBox0203.Items.Add(InSectList(I))
                    ElseIf (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then
                        N += 1
                        IFNavDat(N) = InSectList(I)
                        IFNavDat(N).IntersectionType = eIntersectionType.OnNavaid
                        IFNavDat(N).ValCnt = -2
                    End If

                    'Side = SideDef(ptFAP, _ArDir - 90.0, InSectList(I).pPtPrj)
                    'If (InSectList(I).TypeCode = eNavaidType.NONE) And (fTmp >= fNearDist) And (fTmp <= fFarDist) Then
                    '	ComboBox0203.Items.Add(InSectList(I))
                    'End If

                    'If (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then Continue For
                    ''Side = SideDef(ptPlaneFAP, ArDir - 90.0, InSectList(I).pPtPrj)
                    ''fTmp = Point2LineDistancePrj(ptPlaneFAP, InSectList(I).pPtPrj, ArDir + 90.0)
                    'If (fTmp >= fNearDist) And (fTmp <= fFarDist) Then
                    '	N += 1
                    '	IFNavDat(N) = InSectList(I)
                    '	IFNavDat(N).IntersectionType = eIntersectionType.OnNavaid
                    '	IFNavDat(N).ValCnt = -2
                    'End If
                Next I

                If N < 0 Then
                    ReDim IFNavDat(-1)
                    HidePandaBox()
                    MessageBox.Show(My.Resources.str00303)  '"IF Nav Array пуст"
                    Return
                End If

                ReDim Preserve IFNavDat(N)

                For I = 0 To N
                    ComboBox0201.Items.Add(IFNavDat(I).CallSign)
                Next I

                If ComboBox0203.Items.Count > 0 Then
                    CheckBox0201.Enabled = True
                    ComboBox0203.SelectedIndex = 0
                Else
                    CheckBox0201.Checked = False
                    CheckBox0201.Enabled = False
                End If

                ComboBox0201.SelectedIndex = 0
            Case 2
                CreateMAPolygon()
                TextBox0304.Text = CStr(fMissAprPDG * 100.0)
                TextBox0301.Tag = TextBox0102.Tag
                ComboBox0301_SelectedIndexChanged(ComboBox0301, New System.EventArgs())

                TextBox0302.Text = CStr(ConvertHeight(_CurrFAPOCH, eRoundMode.NEAREST))
                TextBox0305.Text = TextBox0104.Text

                ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo

                _TerminationAltitude = fStraightMissedTermHght + FicTHRprj.Z
                TextBox0308.Text = CStr(ConvertHeight(_TerminationAltitude, eRoundMode.NEAREST))
                TextBox0308_Validating(TextBox0308, Nothing)

                'TextBox0308.Tag = TextBox0308.Text
                'TextBox0309.Text = ConvertDistance(5.0 * 1852.0, eRoundMode.NEAREST).ToString()

                'fTmp = (fStraightMissedTermHght - _hFAP) / fMissAprPDG + dMAPt2SOC
                'TextBox0309.Text = ConvertDistance(fTmp).ToString()

            Case 3
                If OptionButton0401.Checked Then
                    OptionButton0401_CheckedChanged(OptionButton0401, New System.EventArgs())
                ElseIf OptionButton0402.Checked Then
                    OptionButton0402_CheckedChanged(OptionButton0402, New System.EventArgs())
                Else
                    OptionButton0401.Checked = True
                End If

            Case 4
                fBankAngle = CDbl(TextBox0401.Text)
                m_fIAS = 3.6 * DeConvertSpeed(CDbl(TextBox0402.Text))
                CalcTNHIntervals()

                If OptionButton0401.Checked And CheckBox0402.Checked Then
                    Dt = (m_fMOC - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle)) + ZSurfaceOrigin
                    pFixedTIAPart = CreateReducedTIA(OASPlanes, Dt, FicTHRprj, _ArDir + 180.0)

                    pRelational = pFixedTIAPart
                    N = UBound(DetTNHObstacles.Obstacles)
                    hMax = 0.0
                    For I = 0 To N
                        If (Not DetTNHObstacles.Obstacles(I).IgnoredByUser) And pRelational.Contains(DetTNHObstacles.Obstacles(I).pGeomPrj) Then
                            If hMax < DetTNHObstacles.Obstacles(I).Height Then hMax = DetTNHObstacles.Obstacles(I).Height
                        End If
                    Next I

                    If CheckBox0401.Checked Then
                        fTNH = hMax + arMA_FinalMOC.Value
                    Else
                        fTNH = hMax + arMA_InterMOC.Value
                    End If

                    CurrOCH = PrelimOCH4Reduced(DetTNHObstacles, m_fMOC, ILS.GPAngle, m_IxMinOCH)
                    fTmp = CurrOCH - m_fMOC
                    If fTmp > fTNH Then fTNH = fTmp

                    CalcReducedTNH(DetTNHObstacles, ILS.GPAngle, fMissAprPDG, m_fMOC, CurrOCH, fTNH, m_IxMinOCH)
                    If Not bHaveSolution Then
                        HidePandaBox()
                        Return
                    End If

                    If m_IxMinOCH > -1 Then
                        TextBox0501.Text = DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(m_IxMinOCH).Owner).UnicalName
                    Else
                        TextBox0501.Text = ""
                    End If
                    TextBox0507.Tag = "a"
                Else
                    If IxOCH > -1 Then
                        If SideDef(PtCoordCntr, _ArDir + 90.0, WorkObstacleList.Parts(IxOCH).pPtPrj) < 0 Then
                            NextOCH = _CurrFAPOCH
                        Else
                            NextOCH = IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
                        End If
                    Else
                        NextOCH = IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
                    End If

                    m_IxMinOCH = -1
                    IxMaxOCH = -1
                    Do
                        CurrOCH = NextOCH
                        OCHInterv = CalcOCHRange(CurrOCH, NextOCH, m_IxMinOCH, IxMaxOCH)
                        m_TurnIntervals = CalcTurnInterval(CurrOCH, NextOCH, OCHInterv, m_IxMinOCH)
                    Loop While (CurrOCH > NextOCH) And (IxMaxOCH >= 0)

                    If m_IxMinOCH > -1 Then
                        TextBox0501.Text = DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(m_IxMinOCH).Owner).UnicalName
                    ElseIf IxOCH > -1 Then
                        TextBox0501.Text = WorkObstacleList.Obstacles(WorkObstacleList.Parts(IxOCH).Owner).UnicalName
                    Else
                        TextBox0501.Text = ""
                    End If
                End If

                fMisAprOCH = CurrOCH
                TextBox0505.Text = CStr(ConvertHeight(CurrOCH, eRoundMode.NEAREST))

                If ComboBox0502.SelectedIndex < 0 Then
                    ComboBox0502.SelectedIndex = 0
                Else
                    ComboBox0502_SelectedIndexChanged(ComboBox0502, New System.EventArgs())
                End If

                'TextBox0503.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.rmNERAEST))
                fHTurn = fTNH
                DrawMAPtSOC(fMisAprOCH, NextOCH, m_TurnIntervals, IxMaxOCH) ', fTNH
            Case 5
                If OptionButton0402.Checked Then
                    If ComboBox0501.Items.Count < 1 Then
                        HidePandaBox()
                        MessageBox.Show(My.Resources.str20604) '"Отсутствует средство для создания КТ разворота.", 1
                        Return
                    End If
                End If
                '    CurrPage = CurrPage
                CheckState = True
                hTMA = CalcHTMA(ObstacleList, TurnFixPnt, DeConvertHeight(CDbl(ComboBox0402.Text)), FicTHRprj.Z, fMissAprPDG)

                If OptionButton0601.Checked Then
                    OptionButton0601_CheckedChanged(OptionButton0601, New System.EventArgs())
                ElseIf OptionButton0602.Checked Then
                    OptionButton0602_CheckedChanged(OptionButton0602, New System.EventArgs())
                ElseIf OptionButton0603.Checked Then
                    OptionButton0603_CheckedChanged(OptionButton0603, New System.EventArgs())
                End If
            Case 6
                If Not (OptionButton0603.Checked Or OptionButton0602.Checked Or OptionButton0601.Checked) Then
                    HidePandaBox()
                    MessageBox.Show(My.Resources.str00503)
                    Return
                End If

                If (OptionButton0602.Checked Or OptionButton0601.Checked) And Not IsNumeric(TextBox0603.Text) Then
                    TextBox0603.Focus()
                    HidePandaBox()
                    MessageBox.Show(_Label0601_5.Text + My.Resources.str00302)  '"Поле 'Курс следующего сегмента' заполнено некорректно."
                    Return
                End If

                If OptionButton0602.Checked Then
                    If Not IsNumeric(TextBox0604.Text) Then
                        TextBox0604.Focus()
                        HidePandaBox()
                        MessageBox.Show(_Label0601_6.Text + My.Resources.str00302)  '"Поле 'Угол сопряжения' заполнено некорректно."
                        Return
                    End If

                    If Not IsNumeric(TextBox0605.Text) Then
                        TextBox0605.Focus()
                        HidePandaBox()
                        MessageBox.Show(_Label0601_11.Text + My.Resources.str00302) '"Поле 'Время стабилизации' заполнено некорректно."
                        Return
                    End If
                End If

                Frame0901.Visible = OptionButton0602.Checked

                CheckState = False
                ApplayOptions()

                If OptionButton0601.Checked Or (TurnDirector.TypeCode <= eNavaidType.NONE) Then
                    CurrPage = CurrPage + 2
                    CalcTAParams()
                    'PrecReportFrm.FillPage09(TurnAreaObstacleList, IxDh)
                End If
            Case 7
                CheckBox0803.Enabled = OptionButton0703.Checked And OptionButton0703.Enabled
                If Not CheckBox0803.Enabled Then CheckBox0803.Checked = False

                CheckBox0806.Enabled = OptionButton0706.Checked And OptionButton0706.Enabled
                If Not CheckBox0806.Enabled Then CheckBox0806.Checked = False
                SecondArea(TurnDir, BaseArea)
            Case 8
                If Not ((CheckBox0801.Checked) Or (CheckBox0802.Checked) Or (CheckBox0803.Checked)) Then
                    HidePandaBox()
                    MessageBox.Show(My.Resources.str00503)
                    Return
                End If

                If Not ((CheckBox0804.Checked) Or (CheckBox0805.Checked) Or (CheckBox0806.Checked)) Then
                    HidePandaBox()
                    MessageBox.Show(My.Resources.str00503)
                    Return
                End If

                Frame0901.Visible = OptionButton0602.Checked

                CalcTAParams()
                'PrecReportFrm.FillPage09(TurnAreaObstacleList, IxDh)
            Case 9
                segPDG = fMissAprPDG
                Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline

                If OptionButton0602.Checked Then
                    pPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
                    CType(pPoly, IPointCollection).AddPoint(TerFixPnt)
                Else
                    TerFixPnt = mPoly.ToPoint
                    TerFixPnt.M = m_OutDir
                    '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    If OptionButton0401.Checked Then
                        pProxi = ZNR_Poly
                    Else
                        pProxi = KK
                    End If

                    Dim fDis As Double
                    Dim hKK As Double

                    fDis = pProxi.ReturnDistance(TerFixPnt)
                    hKK = DeConvertHeight(CDbl(TextBox0908.Text))
                    TerFixPnt.Z = hKK + fDis * fMissAprPDG

                    '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    'TerFixPnt.Z = DeConvertHeight(CDbl(TextBox0903.Text))
                    '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    mPoly.ToPoint = TerFixPnt
                    pPoly = mPoly
                End If

                '**************************************************************
                Dim pIZ As IZ
                Dim pIZAware As IZAware

                pIZAware = pPoly
                pIZAware.ZAware = True

                pIZ = pPoly
                pIZ.CalculateNonSimpleZs()
                '**************************************************************

                'DrawPolyLine(pPoly, 255, 2)
                'DrawPointWithText(TerFixPnt, "TerFixPnt")
                'Application.DoEvents()

                Trace(0) = AddSegmentFrm.CreateInitialSegment(TerFixPnt, pPoly, BaseArea, m_OutDir, fMissAprPDG, TurnFixPnt.Z)
                If OptionButton0602.Checked Then
                    Trace(0).GuidanceNav = WPT_FIXToNavaid(TurnDirector)
                    Trace(0).InterceptionNav = TerInterNavDat(ComboBox0901.SelectedIndex)
                End If

                TSC = 1

                RemoveSegmentBtn.Enabled = False

                ReDrawTrace()
                ReListTrace()
        End Select

        HidePandaBox()
        screenCapture.Save(Me)
        CurrPage = CurrPage + 1
        '    NextBtn.Enabled = CurrPage < MultiPage1.Tabs - 1    'NextBtn.Enabled and (
        NextBtn.Enabled = NextBtn.Enabled And (CurrPage < MultiPage1.TabPages.Count() - 1)

        PrevBtn.Enabled = CurrPage > 0
        MultiPage1.SelectedIndex = CurrPage
        OkBtn.Enabled = (CurrPage = MultiPage1.TabPages.Count() - 1) And OkBtnEnabled
        Me.HelpContextID = 10000 + 100 * (MultiPage1.SelectedIndex + 1)
        Me.Activate()

        ' 2007
        FocusStepCaption((MultiPage1.SelectedIndex))

        Me.Visible = False
        Me.Show(_Win32Window)
    End Sub

    Private Sub ReportButton_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ReportButton.CheckedChanged
        If Not bFormInitialised Then Return

        If ReportButton.Checked Then
            PrecReportFrm.Show(_Win32Window)
        Else
            PrecReportFrm.Hide()
        End If
    End Sub

    Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
        _IsClosing = True
        Me.Close()
    End Sub

    Private Sub OkBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OkBtn.Click
        Dim RepFileName As String
        Dim RepFileTitle As String

        screenCapture.Save(Me)
        If Not ShowSaveDialog(RepFileName, RepFileTitle) Then Return

        Dim pReport As ReportHeader
		'Dim sProcName As String
		'Dim ProcCat() As String
		'ProcCat = New String() {"CAT_I", "CAT_II", "CAT_II_AUTO"}
		'sProcName = "ILS_" + ProcCat(ComboBox0002.SelectedIndex) + "_RWY" + ComboBox0001.Text + "_CAT_" + ComboBox0003.Text

		'pReport.Procedure = sProcName
		pReport.Procedure = RepFileTitle

		pReport.Aerodrome = CurrADHP.Name
        pReport.Database = gAranEnv.ConnectionInfo.Database

		'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition
		pReport.Category = ComboBox0003.Text

        'pReport.RWY = ComboBox0001.Text
        'pReport.iProcedure = 1

        PrecReportFrm.SortForSave()

        SaveAccuracy(RepFileName, RepFileTitle, pReport)
        SaveLog(RepFileName, RepFileTitle, pReport)
        SaveProtocol(RepFileName, RepFileTitle, pReport)
        SaveGeometry(RepFileName, RepFileTitle, pReport)
		DBModule.pObjectDir.SaveAsXml(RepFileName + "_InputData.xml")

		Try
			If Not SaveProcedure(RepFileTitle) Then Return
		Catch ex As Exception
            gAranEnv.GetLogger("Precision Arrival").Error(ex, "Ok button click")
            Throw
        End Try

        _IsClosing = True
        saveReportToDB()
        saveScreenshotToDB()
        'bUnloadByOk = True
        Me.Close()
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
        saveReportToDB(LogRep, FeatureReportType.Log)
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
#End Region

    Private Sub DrawMAPtSOC(ByRef OCH As Double, ByRef NextOCH As Double, ByRef TurnIntervals() As LowHigh, ByRef IxNextOCH As Integer) ', Optional fHTurn As Double
        Dim N As Integer
        Dim M As Integer
        Dim L As Integer
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim MaxM As Integer

        Dim fTmp As Double
        Dim TNHLow As Double
        Dim TNHHigh As Double
        Dim TipText As String
        Dim ItemCaption As String

        Dim fLinePtDist As Double
        Dim fDist As Double
        Dim fHrel As Double
        Dim dMin As Double
        Dim dMax As Double
        Dim fConRad As Double

        Dim pNomTP As ESRI.ArcGIS.Geometry.IPoint
        Dim itmX As System.Windows.Forms.ListViewItem
        Dim tmpNavDat() As NavaidData
        Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

        PtSOC = PointAlongPlane(PtCoordCntr, _ArDir, XptSOC)
        PtSOC.Z = OCH - m_fMOC
        PtSOC.M = _ArDir

        fTmp = (OCH - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))
        pMAPt = PointAlongPlane(FicTHRprj, _ArDir, -fTmp)
        pMAPt.Z = OCH 'PtSOC.z
        pMAPt.M = _ArDir

        SafeDeleteElement(SOCElem)
        SafeDeleteElement(pMAPtElem)

        SOCElem = DrawPointWithText(PtSOC, "SOC", WPTColor)
        SOCElem.Locked = True

        pMAPtElem = DrawPointWithText(pMAPt, "MAPt", WPTColor)
        pMAPtElem.Locked = True

        '======================================================================

        If OptionButton0401.Checked And CheckBox0402.Checked Then
            TextBox0506.Text = "-"
            TextBox0502.Text = "-"
            TipText = ""

            If ComboBox0503.SelectedIndex < 0 Then
                ComboBox0503.SelectedIndex = 0
            Else
                ComboBox0503_SelectedIndexChanged(ComboBox0503, New System.EventArgs())
            End If
            'TextBox0507.Text = CStr(ConvertHeight(fHTurn, eRoundMode.rmNERAEST))
        Else
            If IxNextOCH > -1 Then
                TextBox0506.Text = CStr(ConvertHeight(NextOCH, eRoundMode.NEAREST))
                TextBox0502.Text = DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(IxNextOCH).Owner).UnicalName
            Else
                TextBox0506.Text = "-"
                TextBox0502.Text = "-"
            End If
            'DrawPoint DetTNHObstacles(IxNextOCH).pPtPrj, 255
            N = UBound(TurnIntervals)
            ListView0501.Items.Clear()

            If N < 0 Then
                MessageBox.Show(My.Resources.str00303)  '"??????????????????????????????"
                Return
            End If

            fTNH = (TurnIntervals(0).Low - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
            'If fTNH < fMisAprOCH Then fTNH = fMisAprOCH
            MaxM = -1

            If OptionButton0401.Checked Then
                TipText = My.Resources.str00210  '"Допустимый интервал высот:"
                fHTurn = fTNH
                If ComboBox0503.SelectedIndex < 0 Then
                    ComboBox0503.SelectedIndex = 0
                Else
                    ComboBox0503_SelectedIndexChanged(ComboBox0503, New System.EventArgs())
                End If
                'TextBox0507.Text = CStr(ConvertHeight(fTNH, eRoundMode.rmNERAEST))
            Else
                ReDim TPInterNavDat(N, 20)
                TipText = My.Resources.str20603  '"Допустимый интервал расстояний от THR:"
                pNomTP = New ESRI.ArcGIS.Geometry.Point
                pConstruct = pNomTP
                TextBox0507.Text = CStr(ConvertDistance(XptLH - TurnIntervals(0).Low, 3))
            End If

            For I = 0 To N
                If TurnIntervals(I).Tag < 0 Then Continue For

                TNHLow = (TurnIntervals(I).Low - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
                TNHHigh = (TurnIntervals(I).High - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC

                If OptionButton0402.Checked Then 'Над FIX    'DrawPolygon pTmpPoly, 0
                    'DrawPolygon(pFullPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)

                    'While True
                    '	Application.DoEvents()
                    'End While

                    tmpNavDat = GetValidNavs(pFullPoly, FicTHRprj.Z, TNHLow, TNHHigh, fMisAprOCH, _ArDir, fMissAprPDG, PtSOC, pTmpPoly, , , False)

                    M = UBound(tmpNavDat) 'If M >= 0 Then DrawPolygon pTmpPoly, 0
                    '========================================================== ??????????????????????
                    K = UBound(InSectList)

                    If M >= 0 Then
                        If K >= 0 Then ReDim Preserve tmpNavDat(K + M + 1)
                    Else
                        If K >= 0 Then ReDim Preserve tmpNavDat(K)
                    End If

                    dMin = (TNHLow - PtSOC.Z) / fMissAprPDG '0# '(hMin - arAbv_Treshold.Value) / PDG
                    dMax = (TNHHigh - PtSOC.Z) / fMissAprPDG

                    For J = 0 To K
                        If (InSectList(J).TypeCode <> eNavaidType.NDB) And (InSectList(J).TypeCode <> eNavaidType.VOR) And (InSectList(J).TypeCode <> eNavaidType.TACAN) Then Continue For
                        '===============================================
                        fLinePtDist = Point2LineDistancePrj(InSectList(J).pPtPrj, PtSOC, _ArDir)
                        fDist = Point2LineDistancePrj(InSectList(J).pPtPrj, PtSOC, _ArDir + 90.0) * SideDef(InSectList(J).pPtPrj, _ArDir - 90.0, PtSOC)
                        fHrel = Point2LineDistancePrj(PtSOC, InSectList(J).pPtPrj, _ArDir + 90.0) * SideDef(PtSOC, _ArDir + 90.0, InSectList(J).pPtPrj)
                        If fHrel < 0.0 Then fHrel = 0.0

                        fHrel = fHrel * fMissAprPDG + fMisAprOCH + FicTHRprj.Z - InSectList(J).pPtGeo.Z
                        If fHrel < arMATurnAlt.Value Then fHrel = arMATurnAlt.Value
                        '            fHrel = fRefHeight + CurrOCH - InSectList(I).pPtGeo.Z
                        '            If fHrel < arMATurnAlt.Value Then fHrel = arMATurnAlt.Value
                        fTmp = OnNAVShift(InSectList(J).TypeCode, fHrel)

                        If (fDist > dMin) And (fDist < dMax) And (fLinePtDist <= fTmp) Then
                            If InSectList(J).TypeCode = eNavaidType.NDB Then
                                fConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * fHrel
                            Else
                                fConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * fHrel
                            End If

                            If dMax - fDist >= fConRad Then
                                M = M + 1
                                tmpNavDat(M) = InSectList(J)
                                tmpNavDat(M).ValCnt = -2
                                tmpNavDat(M).IntersectionType = eIntersectionType.OnNavaid
                            End If
                        End If
                    Next J

                    If M >= 0 Then
                        ReDim Preserve tmpNavDat(M)
                    Else
                        ReDim tmpNavDat(-1)
                        'CurrPage = MultiPage1.Tab
                        'HidePandaBox
                        'MsgBox LoadResString(304) '"Нет подходящего средства для создания FIX"
                        'Return
                    End If
                    '========================================================== ??????????????????????

                    If M >= 0 Then
                        TNHLow = XptLH - TurnIntervals(I).Low
                        TNHHigh = XptLH - TurnIntervals(I).High
                        If M > 20 Then M = 20
                        If M > MaxM Then MaxM = M
                    Else
                        TNHLow = XptLH - TurnIntervals(I).High
                        TNHHigh = XptLH - TurnIntervals(I).Low
                    End If

                    For J = 0 To M
                        TPInterNavDat(I, J) = tmpNavDat(J)
                        'TPInterNavDat(I, J).Index = 0
                        If tmpNavDat(J).IntersectionType <> eIntersectionType.ByDistance Then
                            If tmpNavDat(J).IntersectionType = eIntersectionType.OnNavaid Then
                                fTmp = ReturnDistanceInMeters(FicTHRprj, tmpNavDat(J).pPtPrj) * SideDef(FicTHRprj, _ArDir - 90.0, tmpNavDat(J).pPtPrj)
                                If fTmp < TNHLow Then TNHLow = fTmp
                                If fTmp > TNHHigh Then TNHHigh = fTmp
                            Else
                                pConstruct.ConstructAngleIntersection(PtSOC, DegToRad(_ArDir), tmpNavDat(J).pPtPrj, DegToRad(tmpNavDat(J).ValMin(0)))
                                fTmp = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)
                                If fTmp < TNHLow Then TNHLow = fTmp
                                If fTmp > TNHHigh Then TNHHigh = fTmp

                                pConstruct.ConstructAngleIntersection(PtSOC, DegToRad(_ArDir), tmpNavDat(J).pPtPrj, DegToRad(tmpNavDat(J).ValMax(0)))
                                fTmp = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)
                                If fTmp < TNHLow Then TNHLow = fTmp
                                If fTmp > TNHHigh Then TNHHigh = fTmp
                            End If
                        Else
                            L = UBound(tmpNavDat(J).ValMin)

                            For K = 0 To L
                                If ((L = 0) And (tmpNavDat(J).ValCnt <= 0)) Or ((L <> 0) And (K > 0)) Then
                                    CircleVectorIntersect(tmpNavDat(J).pPtPrj, tmpNavDat(J).ValMax(K), PtSOC, _ArDir, pNomTP)
                                Else
                                    CircleVectorIntersect(tmpNavDat(J).pPtPrj, tmpNavDat(J).ValMin(K), PtSOC, _ArDir + 180.0, pNomTP)
                                End If

                                fTmp = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)
                                If fTmp < TNHLow Then TNHLow = fTmp
                                If fTmp > TNHHigh Then TNHHigh = fTmp

                                If ((L = 0) And (tmpNavDat(J).ValCnt <= 0)) Or ((L <> 0) And (K > 0)) Then
                                    CircleVectorIntersect(tmpNavDat(J).pPtPrj, tmpNavDat(J).ValMin(K), PtSOC, _ArDir, pNomTP)
                                Else
                                    CircleVectorIntersect(tmpNavDat(J).pPtPrj, tmpNavDat(J).ValMax(K), PtSOC, _ArDir + 180.0, pNomTP)
                                End If
                                'DrawPolygon CreatePrjCircle(tmpNavDat(J).pPtPrj, ReturnDistanceInMeters(tmpNavDat(J).pPtPrj, pNomTP))

                                fTmp = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)
                                If fTmp < TNHLow Then TNHLow = fTmp
                                If fTmp > TNHHigh Then TNHHigh = fTmp
                            Next K
                        End If
                    Next J

                    For J = M + 1 To 20
                        TPInterNavDat(I, J).Index = -1
                    Next J

                    TurnIntervals(I).High = XptLH - TNHLow
                    TurnIntervals(I).Low = XptLH - TNHHigh
                    If TurnIntervals(I).Low < 10.0 Then TurnIntervals(I).Low = 10.0

                    ItemCaption = CStr(ConvertDistance(TNHHigh, 1)) + ".." + CStr(ConvertDistance(TNHLow, 3))
                    itmX = ListView0501.Items.Add(ItemCaption)
                    '            itmX.SubItems(3) = CStr(M + 1)
                    TipText = TipText + My.Resources.str00221 + CStr(ConvertDistance(TNHHigh, 1)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TNHLow, 3)) + " " + DistanceConverter(DistanceUnit).Unit

                    fTmp = (XptLH - TNHLow - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
                    TNHLow = (XptLH - TNHHigh - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
                    TNHHigh = fTmp
                Else
                    ItemCaption = CStr(ConvertDistance(XptLH - TurnIntervals(I).Low, 3)) + ".." + CStr(ConvertDistance(XptLH - TurnIntervals(I).High, 1))
                    itmX = ListView0501.Items.Add(ItemCaption)
                    '            itmX.SubItems(3) = "-"
                    TipText = TipText + My.Resources.str00221 + CStr(ConvertHeight(TNHLow, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(TNHHigh, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
                End If

                ItemCaption = CStr(ConvertHeight(TNHLow, eRoundMode.NEAREST)) + ".." + CStr(ConvertHeight(TNHHigh, eRoundMode.NEAREST))
                itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemCaption))

                ItemCaption = CStr(ConvertHeight(TNHLow + FicTHRprj.Z, eRoundMode.NEAREST)) + ".." + CStr(ConvertHeight(TNHHigh + FicTHRprj.Z, eRoundMode.NEAREST))
                itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemCaption))

                If TurnIntervals(I).Tag > 0 Then
                    itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(TurnIntervals(I).Tag - 1).Owner).UnicalName))
                End If

                If I <> N Then TipText = TipText + "; "
            Next I

            If MaxM >= 0 Then
                ReDim Preserve TPInterNavDat(N, MaxM)
            Else
                ReDim TPInterNavDat(-1, -1)
            End If
        End If

        ToolTip1.SetToolTip(TextBox0507, TipText)
        TextBox0507_Validating(TextBox0507, New System.ComponentModel.CancelEventArgs())
        PrecReportFrm.FillPage07(DetTNHObstacles, 1)
    End Sub

    Private Function CalcTurnInterval(ByRef OCH As Double, ByRef NextOCH As Double, ByRef OCHRange As LowHigh, ByRef IxMinOCH As Integer) As LowHigh()
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim lIxOCH As Integer

        Dim fTmp As Double
        Dim lTAS As Double
        Dim L6Sec As Double
        Dim TanGPA As Double
        Dim CoTanZ As Double

        Dim CurrMOC As Double
        Dim fNewOCH As Double
        Dim CoTanGPA As Double

        Dim fTmpDist As Double
        Dim TurnDist As Double
        Dim Result() As LowHigh
        Dim FullInterval() As LowHigh
        Dim ObsInterval(,) As LowHigh
        Dim ZSurfaceOrigin As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        If CheckBox0401.Checked Then
            CurrMOC = arMA_FinalMOC.Value
        Else
            CurrMOC = arMA_InterMOC.Value
        End If

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        CoTanGPA = 1.0 / TanGPA
        CoTanZ = 1.0 / fMissAprPDG

        lTAS = IAS2TAS(m_fIAS, FicTHRprj.Z + arMATurnAlt.Value, CurrADHP.ISAtC)
        L6Sec = arT_TechToleranc.Value * (lTAS + CurrADHP.WindSpeed) * 0.277777777777778

        N = UBound(DetTNHObstacles.Parts)
        fNewOCH = m_fMOC
        'PrevH = Max(OCH - fMOC, CurrMOC)

        ReDim FullInterval(0)
        FullInterval(0) = OCHRange
        FullInterval(0).Tag = 0

        If N < 0 Then Return FullInterval

        ReDim ObsInterval(1, N)
        J = -1

        For I = 0 To N
            If ((Not DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(I).Owner).IgnoredByUser) And (DetTNHObstacles.Parts(I).Flags And 16) = 0) And (DetTNHObstacles.Parts(I).Dist > 0.0) Then
                TurnDist = DetTNHObstacles.Parts(I).TurnDistL - L6Sec
                If TurnDist < OCHRange.High Then
                    ObsInterval(1, I).Low = TanGPA * (XptLH - TurnDist + ZSurfaceOrigin) + m_fMOC   ' If ObsInterval(1, I).Low < 10.0 Then ObsInterval(1, I).Low = 10.0
                    ObsInterval(1, I).Tag = I + 1

                    If OptionButton0401.Checked Then
                        '                   ObsInterval(1, I).High = (XptLH - Max(OCHRange.High, DetTNHObstacles(I).Dist) + ZSurfaceOrigin + DetTNHObstacles(I).ReqH * CoTanZ) / (CoTanZ + CoTanGPA) + fMOC
                        ObsInterval(1, I).High = (XptLH - Max(OCHRange.High, DetTNHObstacles.Parts(I).Dist) + ZSurfaceOrigin + DetTNHObstacles.Parts(I).Height * CoTanZ) / (CoTanZ + CoTanGPA) + m_fMOC
                    Else
                        If DetTNHObstacles.Parts(I).Dist < OCHRange.High Then
                            ObsInterval(1, I).High = 0.0
                        Else
                            '                       ObsInterval(1, I).High = (XptLH - DetTNHObstacles(I).Dist + ZSurfaceOrigin + DetTNHObstacles(I).ReqH * CoTanZ) / (CoTanZ + CoTanGPA) + fMOC
                            ObsInterval(1, I).High = (XptLH - DetTNHObstacles.Parts(I).Dist + ZSurfaceOrigin + DetTNHObstacles.Parts(I).Height * CoTanZ) / (CoTanZ + CoTanGPA) + m_fMOC
                        End If
                    End If

                    ObsInterval(1, I).High = Min(ObsInterval(1, I).High, DetTNHObstacles.Parts(I).ReqOCH)

                    '               PrevH = DetTNHObstacles(I).Height
                    fTmp = Min(ObsInterval(1, I).Low, ObsInterval(1, I).High)
                    If fNewOCH < fTmp Then
                        fNewOCH = fTmp
                        lIxOCH = I
                    ElseIf OptionButton0402.Checked Then
                        ObsInterval(1, I).Tag = -1
                        ObsInterval(0, I).Tag = -1
                    End If
                    J = I
                Else
                    ObsInterval(1, I).Tag = -1
                    ObsInterval(0, I).Tag = -1
                End If
            Else
                ObsInterval(1, I).Tag = -1
                ObsInterval(0, I).Tag = -1
            End If
        Next I

        N = J

        '    ReDim FullInterval(0 To 0)
        '    FullInterval(0) = OCHRange
        '    FullInterval(0).Tag = 0
        '    CalcTurnInterval = FullInterval

        If N < 0 Then Return FullInterval

        'fNewOCH = 5.0 * Round(0.2 * fNewOCH + 0.4999)

        If (NextOCH > OCH) And (fNewOCH >= NextOCH) Then
            OCH = fNewOCH
            Return FullInterval
        End If

        ReDim Preserve ObsInterval(1, N)
        If OCH < fNewOCH Then
            OCH = fNewOCH
            IxMinOCH = lIxOCH
            fTmpDist = (OCH - m_fMOC) / TanGPA - ZSurfaceOrigin
            XptSOC = XptLH - fTmpDist
            OCHRange.Low = XptLH - Max(OCH - m_fMOC, CurrMOC) / TanGPA + ZSurfaceOrigin
        End If

        For I = 0 To N
            If (Not DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(I).Owner).IgnoredByUser) And (ObsInterval(1, I).Tag >= 0) Then
                TurnDist = DetTNHObstacles.Parts(I).TurnDistL - L6Sec
                If TurnDist < OCHRange.High Then
                    fTmpDist = XptSOC + (DetTNHObstacles.Parts(I).ReqH - OCH + m_fMOC) * CoTanZ

                    If fTmpDist < DetTNHObstacles.Parts(I).Dist Then
                        ObsInterval(0, I).Tag = -1
                    Else
                        ObsInterval(0, I).Low = TurnDist
                        If OptionButton0401.Checked Then
                            ObsInterval(0, I).High = fTmpDist
                        Else
                            ObsInterval(0, I).High = DetTNHObstacles.Parts(I).Dist + CoTanZ
                        End If

                        ObsInterval(0, I).Tag = I + 1
                    End If
                End If
            End If
        Next I

        '==================================================================================================

        For I = 0 To N
            If ObsInterval(0, I).Tag >= 0 Then
                M = UBound(FullInterval)
                J = 0
                Do While J <= M
                    Result = LowHighDifference(FullInterval(J), ObsInterval(0, I))
                    L = UBound(Result)

                    If L >= 0 Then
                        If Result(0).High < FullInterval(J).High Then Result(0).Tag = I + 1
                    End If

                    If L = 0 Then
                        FullInterval(J) = Result(0)
                        '                    FullInterval(J).Tag = 1
                    ElseIf L > 0 Then
                        FullInterval(J) = Result(0)
                        '                    FullInterval(J).Tag = 1
                        ReDim Preserve FullInterval(M + L)

                        For K = M + 1 To M + L
                            FullInterval(K) = Result(K - M)
                            FullInterval(K).Tag = I + 1
                        Next K
                    ElseIf L < 0 Then
                        If M = 0 Then
                            NextOCH = OCH + 0.5
                            OCH = NextOCH + 0.5
                            Return Result
                        Else
                            For K = J + 1 To M - 1
                                FullInterval(K) = Result(K + 1)
                            Next K

                            ReDim Preserve FullInterval(M - 1)
                            J = J - 1
                        End If
                    End If
                    M = UBound(FullInterval)
                    J = J + 1
                Loop
            End If
        Next I

        Return FullInterval
    End Function

    Private Sub CalcTNHIntervals()
        Dim I As Integer
        Dim DtObCnt As Integer
        Dim DtPrCnt As Integer
        Dim WrObCnt As Integer
        Dim WrPrCnt As Integer

        Dim fCutDist As Double
        Dim TanGPA As Double
        Dim CoTanZ As Double
        Dim TurnMOC As Double
        Dim CoTanGPA As Double

        Dim pYPlaneRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim ZSurfaceOrigin As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        CoTanGPA = 1.0 / TanGPA
        CoTanZ = 1.0 / fMissAprPDG

        If CheckBox0401.Checked Then
            TurnMOC = arMA_FinalMOC.Value
        Else
            TurnMOC = arMA_InterMOC.Value
        End If
        '=============================================================================
        GetIntermObstacleList(ObstacleList, DetTNHObstacles, ZContinued, FicTHRprj, _ArDir)
        'Dim WrObCnt As Integer
        'Dim WrPrCnt As Integer

        DtObCnt = UBound(DetTNHObstacles.Obstacles)
        DtPrCnt = UBound(DetTNHObstacles.Parts)

        For I = 0 To DtPrCnt
            DetTNHObstacles.Parts(I).ReqOCH = (DetTNHObstacles.Parts(I).Height * CoTanZ + (ZSurfaceOrigin - DetTNHObstacles.Parts(I).Dist)) / (CoTanZ + CoTanGPA) + m_fMOC
            If DetTNHObstacles.Parts(I).ReqOCH < 0.0 Then DetTNHObstacles.Parts(I).ReqOCH = 0.0

            DetTNHObstacles.Parts(I).ReqH = DetTNHObstacles.Parts(I).Height + TurnMOC
            DetTNHObstacles.Parts(I).Dist = XptLH + DetTNHObstacles.Parts(I).Dist
            DetTNHObstacles.Parts(I).Plane = eOAS.NonPrec
        Next I

        WrObCnt = UBound(WorkObstacleList.Obstacles)
        WrPrCnt = UBound(WorkObstacleList.Parts)

        If DtPrCnt >= 0 Then
            ReDim Preserve DetTNHObstacles.Obstacles(WrObCnt + DtObCnt + 1)
            ReDim Preserve DetTNHObstacles.Parts(WrPrCnt + DtPrCnt + 1)
        ElseIf WrPrCnt >= 0 Then
            ReDim DetTNHObstacles.Obstacles(WrObCnt)
            ReDim DetTNHObstacles.Parts(WrPrCnt)
        Else
            ReDim DetTNHObstacles.Obstacles(-1)
            ReDim DetTNHObstacles.Parts(-1)
        End If

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            fCutDist = FAPEarlierToler
        Else
            fCutDist = ReturnDistanceInMeters(IFprj, FicTHRprj)
        End If
        pYPlaneRelation = OASPlanes(eOAS.ZPlane + TurnDir).Poly


        For I = 0 To WrObCnt
            WorkObstacleList.Obstacles(I).NIx = -1
        Next

        For I = 0 To WrPrCnt
            'If (WorkObstacleList.Parts(I).Dist < XptLH) And ((Not pYPlaneRelation.Contains(WorkObstacleList.Parts(I).pPtPrj)) Or (WorkObstacleList.Parts(I).hPent > 0.0)) Then
            If (Not pYPlaneRelation.Contains(WorkObstacleList.Parts(I).pPtPrj) Or (WorkObstacleList.Parts(I).hPenet > 0.0)) And (WorkObstacleList.Parts(I).ReqH < _hFAP) And (WorkObstacleList.Parts(I).Dist < fCutDist) Then
            Else
                Continue For
            End If
            DtPrCnt += 1
            DetTNHObstacles.Parts(DtPrCnt) = WorkObstacleList.Parts(I)

            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                DtObCnt += 1
                DetTNHObstacles.Obstacles(DtObCnt) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                DetTNHObstacles.Obstacles(DtObCnt).PartsNum = 0
                ReDim DetTNHObstacles.Obstacles(DtObCnt).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = DtObCnt
            End If

            DetTNHObstacles.Parts(DtPrCnt).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
            DetTNHObstacles.Parts(DtPrCnt).Index = DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(DtPrCnt).Owner).PartsNum
            DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(DtPrCnt).Owner).Parts(DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(DtPrCnt).Owner).PartsNum) = DtPrCnt
            DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(DtPrCnt).Owner).PartsNum += 1

            If DetTNHObstacles.Parts(DtPrCnt).hPenet > 0.0 Then
                If CheckBox0001.Checked And (DetTNHObstacles.Parts(DtPrCnt).Dist >= -ZSurfaceOrigin) Then
                    DetTNHObstacles.Parts(DtPrCnt).ReqOCH = DetTNHObstacles.Parts(DtPrCnt).Height + m_fMOC
                Else
                    DetTNHObstacles.Parts(DtPrCnt).ReqOCH = Min(DetTNHObstacles.Parts(DtPrCnt).Height, DetTNHObstacles.Parts(DtPrCnt).EffectiveHeight) + m_fMOC
                End If

                'DetTNHObstacles.Parts(DtPrCnt).ReqOCH = Min(DetTNHObstacles.Parts(DtPrCnt).Height, DetTNHObstacles.Parts(DtPrCnt).EffectiveHeight) + m_fMOC
            Else
                DetTNHObstacles.Parts(DtPrCnt).ReqOCH = 0.0
            End If

            DetTNHObstacles.Parts(DtPrCnt).Dist = XptLH - DetTNHObstacles.Parts(DtPrCnt).Dist
            DetTNHObstacles.Parts(DtPrCnt).ReqH = DetTNHObstacles.Parts(DtPrCnt).Height + TurnMOC
        Next I

        If DtPrCnt < 0 Then
            ReDim DetTNHObstacles.Obstacles(-1)
            ReDim DetTNHObstacles.Parts(-1)
        Else
            ReDim Preserve DetTNHObstacles.Parts(DtPrCnt)
            ReDim Preserve DetTNHObstacles.Obstacles(DtObCnt)
        End If

        Sort(DetTNHObstacles, 0)
        '=============================================================================
        '    N = UBound(DetTNHObstacles)
        '    PrevH = Max(fCurrOCH - fMOC - TurnMOC, 0.0)

        '    PrevH = 0.0
        '    J = -1

        '    For I = 0 To N
        '        lFlag = DetTNHObstacles(I).Flags - IIf(DetTNHObstacles(I).Flags > 100, 100, 0)
        '        bHFlg = DetTNHObstacles(I).Height > PrevH
        '        bOCHFlg = ((lFlag = eOAS.NonPrec) And (DetTNHObstacles(I).ReqOCH > PrevOCH)) Or ((lFlag <> eOAS.NonPrec) And (DetTNHObstacles(I).hPent > 0.0))

        '        If bHFlg Or bOCHFlg Then
        '            J = J + 1
        '            DetTNHObstacles(J) = DetTNHObstacles(I)
        '            If bHFlg Then PrevH = DetTNHObstacles(I).Height
        '            If bOCHFlg And (lFlag = eOAS.NonPrec) Then PrevOCH = DetTNHObstacles(I).ReqOCH
        '        End If
        '    Next I
        '    J = N

        '    If J < 0 Then
        '        ReDim DetTNHObstacles(-1)
        '    Else
        '        ReDim Preserve DetTNHObstacles(0 To J)
        '    End If

        CalcTurnRange(DetTNHObstacles)

        PrecReportFrm.FillPage07(DetTNHObstacles)   '????????????????

        Sort(DetTNHObstacles, 1)
    End Sub

    Private Function CalcOCHRange(ByRef OCH As Double, ByRef NextOCH As Double, ByRef IxMinOCH As Integer, ByRef IxNextOCH As Integer) As LowHigh
        Dim I As Integer
        Dim N As Integer

        Dim lFlag As Integer

        Dim fTmp As Double
        Dim lTAS As Double
        Dim L6Sec As Double
        Dim TanGPA As Double

        Dim CurrMOC As Double
        Dim fNewOCH As Double
        Dim fTmpDist As Double
        Dim TurnDist As Double
        Dim fMinHDist As Double
        Dim ZSurfaceOrigin As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        If CheckBox0401.Checked Then
            CurrMOC = arMA_FinalMOC.Value
        Else
            CurrMOC = arMA_InterMOC.Value
        End If

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))

        'fTmpDist = (OCH - fMOC) / TanGPA - ZSurfaceOrigin
        XptSOC = XptLH - (OCH - m_fMOC) / TanGPA + ZSurfaceOrigin
        fMinHDist = XptSOC '+ Max(CurrMOC - OCH + fMOC, 0.0) / fMissAprPDG
        If fMinHDist < 10.0 Then fMinHDist = 10.0

        lTAS = IAS2TAS(m_fIAS, FicTHRprj.Z + arMATurnAlt.Value, CurrADHP.ISAtC)
        L6Sec = arT_TechToleranc.Value * (lTAS + CurrADHP.WindSpeed) * 0.277777777777778

        CalcOCHRange.Low = fMinHDist
        CalcOCHRange.High = RModel

        'NextOCH = 0.0

        N = UBound(DetTNHObstacles.Parts)
        'If ((DetTNHObstacles(I).Flags = eOAS.NonPrec) And (DetTNHObstacles(I).ReqOCH > PrevOCH)) Or (DetTNHObstacles(I).hPent > 0.0) Then
        '	If DetTNHObstacles(I).Flags = eOAS.NonPrec Then PrevOCH = DetTNHObstacles(I).ReqOCH
        '	J += 1
        '	DetTNHObstacles(J) = DetTNHObstacles(I)
        'End If

        For I = 0 To N
            lFlag = DetTNHObstacles.Parts(I).Plane And 15

            If (Not DetTNHObstacles.Obstacles(DetTNHObstacles.Parts(I).Owner).IgnoredByUser) And ((lFlag = eOAS.NonPrec) Or (DetTNHObstacles.Parts(I).hPenet > 0.0)) And (DetTNHObstacles.Parts(I).ReqOCH > OCH) And ((DetTNHObstacles.Parts(I).Plane And 16) = 0) Then
                TurnDist = DetTNHObstacles.Parts(I).TurnDistL - L6Sec
                If TurnDist > fMinHDist Then
                    CalcOCHRange.Low = fMinHDist
                    CalcOCHRange.High = TurnDist
                    NextOCH = DetTNHObstacles.Parts(I).ReqOCH
                    IxNextOCH = I
                    Exit Function
                Else
                    fTmp = XptLH + ZSurfaceOrigin - TurnDist
                    fNewOCH = fTmp * TanGPA + m_fMOC

                    If fNewOCH > DetTNHObstacles.Parts(I).ReqOCH Then
                        OCH = DetTNHObstacles.Parts(I).ReqOCH
                        XptSOC = XptLH - (OCH - m_fMOC) / TanGPA + ZSurfaceOrigin
                        fMinHDist = XptSOC                  'XptLH - Max((OCH - fMOC), CurrMOC) / TanGPA + ZSurfaceOrigin 'XptSOC
                        IxMinOCH = I
                    Else
                        OCH = fNewOCH

                        fTmpDist = (OCH - m_fMOC) / TanGPA - ZSurfaceOrigin
                        XptSOC = XptLH - fTmpDist
                        fMinHDist = XptLH - Max(OCH - m_fMOC, CurrMOC) / TanGPA + ZSurfaceOrigin 'XptSOC

                        NextOCH = DetTNHObstacles.Parts(I).ReqOCH
                        IxNextOCH = I

                        CalcOCHRange.Low = TurnDist
                        CalcOCHRange.High = TurnDist
                        IxMinOCH = I
                        Exit Function
                    End If
                End If
            End If
        Next I
    End Function

    Private Function CalcImIntervals(ByRef fhFAP As Double, ByRef IntermObstacles As ObstacleContainer, ByRef Ix As Integer) As LowHigh()
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim P As Integer
        Dim O As Integer

        Dim fDh As Double
        Dim fTmp As Double
        Dim fTmp1 As Double
        Dim fDist As Double
        Dim MaxLocDist As Double

        Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
        Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTmpPolygon As ESRI.ArcGIS.Geometry.IPolygon
        Dim pLPolyLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pRPolyLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim pCutLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim ptTmpFAP As ESRI.ArcGIS.Geometry.IPoint

        N = UBound(OASPlanes)
        'ReDim wOASPlanes(N)
        System.Array.Copy(OASPlanes, wOASPlanes, N + 1)

        CreateOASPlanes(FicTHRprj, _ArDir, fhFAP - arISegmentMOC.Value, wOASPlanes, 1)

        pLPolyLine = IntersectPlanes(OASPlanes(eOAS.XlPlane).Plane, OASPlanes(eOAS.YlPlane).Plane, fhFAP - arISegmentMOC.Value, fhFAP)
        pRPolyLine = IntersectPlanes(OASPlanes(eOAS.YrPlane).Plane, OASPlanes(eOAS.XrPlane).Plane, fhFAP - arISegmentMOC.Value, fhFAP)

        pTransform = pLPolyLine
        pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
        pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

        pTransform = pRPolyLine
        pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
        pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

        '======================================================
        '  Set ptPlaneFAP = New Point
        '  ptPlaneFAP.PutCoords 0.5 * (pRPolyLine.ToPoint.X + pLPolyLine.ToPoint.X), 0.5 * (pRPolyLine.ToPoint.Y + pLPolyLine.ToPoint.Y)
        '  ptPlaneFAP.Z = ptFAP.Z
        '  ptPlaneFAP.M = ptFAP.M
        '======================================================

        fDist = hFAP2FAPDist(fhFAP)
        ptTmpFAP = PointAlongPlane(FicTHRprj, _ArDir + 180.0, fDist)

        pCutLine = New ESRI.ArcGIS.Geometry.Polyline
        pCutLine.FromPoint = PointAlongPlane(ptTmpFAP, _ArDir + 90.0, 100000.0)
        pCutLine.ToPoint = PointAlongPlane(ptTmpFAP, _ArDir - 90.0, 100000.0)

        Dim Vb As Double
        Dim Ub As Double
        Dim Nd As Double
        Dim Xr As Double
        Dim Yr As Double

        Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt4 As ESRI.ArcGIS.Geometry.IPoint

        Dim ptLeft As ESRI.ArcGIS.Geometry.IPoint
        Dim ptRight As ESRI.ArcGIS.Geometry.IPoint
        Dim bFlg As Boolean

        pt1 = pCutLine.FromPoint
        pt2 = pCutLine.ToPoint

        pt3 = pRPolyLine.FromPoint
        pt4 = pRPolyLine.ToPoint

        'Va = (pt4.X - pt3.X) * (pt1.Y - pt3.Y) - (pt4.Y - pt3.Y) * (pt1.X - pt3.X)
        Vb = (pt2.X - pt1.X) * (pt1.Y - pt3.Y) - (pt2.Y - pt1.Y) * (pt1.X - pt3.X)
        Nd = (pt4.Y - pt3.Y) * (pt2.X - pt1.X) - (pt4.X - pt3.X) * (pt2.Y - pt1.Y)
        Ub = Vb / Nd

        bFlg = (Ub >= 0) And (Ub <= 1)

        If bFlg Then
            Xr = pt3.X + Ub * (pt4.X - pt3.X)
            Yr = pt3.Y + Ub * (pt4.Y - pt3.Y)
            ptRight = New ESRI.ArcGIS.Geometry.Point
            ptRight.PutCoords(Xr, Yr)

            pt3 = pLPolyLine.FromPoint
            pt4 = pLPolyLine.ToPoint

            'Va = (pt4.X - pt3.X) * (pt1.Y - pt3.Y) - (pt4.Y - pt3.Y) * (pt1.X - pt3.X)
            Vb = (pt2.X - pt1.X) * (pt1.Y - pt3.Y) - (pt2.Y - pt1.Y) * (pt1.X - pt3.X)
            Nd = (pt4.Y - pt3.Y) * (pt2.X - pt1.X) - (pt4.X - pt3.X) * (pt2.Y - pt1.Y)

            Ub = Vb / Nd

            Xr = pt3.X + Ub * (pt4.X - pt3.X)
            Yr = pt3.Y + Ub * (pt4.Y - pt3.Y)
            ptLeft = New ESRI.ArcGIS.Geometry.Point
            ptLeft.PutCoords(Xr, Yr)
            I = 2
        Else
            ptRight = pRPolyLine.FromPoint
            ptLeft = pLPolyLine.FromPoint
            I = 1
        End If

        'MaxLocDist = Max(46000.0, fDist + 28000.0)
        MaxLocDist = fDist + arImRange_Max.Value '28000.0

        IntermediateFullArea = New ESRI.ArcGIS.Geometry.Polygon

        pPoint = PointAlongPlane(FicTHRprj, _ArDir + 180.0, fDist + arMinISlen)

        If bFlg Then IntermediateFullArea.AddPoint(pRPolyLine.FromPoint)

        IntermediateFullArea.AddPoint(ptRight)

        IntermediateFullArea.AddPoint(PointAlongPlane(pPoint, _ArDir - 90.0, arIFHalfWidth.Value))
        IntermediateFullArea.AddPoint(PointAlongPlane(IntermediateFullArea.Point(I), _ArDir - 180.0, MaxLocDist - fDist - arMinISlen - ILS.LLZ_THR))

        fMaxInterLenght = Point2LineDistancePrj(ptTmpFAP, IntermediateFullArea.Point(I + 1), _ArDir + 90.0)

        IntermediateFullArea.AddPoint(PointAlongPlane(IntermediateFullArea.Point(I + 1), _ArDir + 90.0, 2.0 * arIFHalfWidth.Value))
        IntermediateFullArea.AddPoint(PointAlongPlane(pPoint, _ArDir + 90.0, arIFHalfWidth.Value))
        IntermediateFullArea.AddPoint(ptLeft)
        If bFlg Then IntermediateFullArea.AddPoint(pLPolyLine.FromPoint)

        '============================================================================

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            pCutLine = New ESRI.ArcGIS.Geometry.Polyline
            pPoint = PointAlongPlane(FicTHRprj, _ArDir + 180.0, FAPEarlierToler)
            pCutLine.FromPoint = PointAlongPlane(pPoint, _ArDir + 90.0, 100000.0)
            pCutLine.ToPoint = PointAlongPlane(pPoint, _ArDir - 90.0, 100000.0)

            pTmpPolygon = Nothing
            pTopo = wOASPlanes(eOAS.CommonPlane).Poly
            On Error Resume Next
            pTopo.Cut(pCutLine, pTmpPolygon, pPolygon)
            On Error GoTo 0
            If pTmpPolygon Is Nothing Then pTmpPolygon = wOASPlanes(eOAS.CommonPlane).Poly
        Else
            pTmpPolygon = wOASPlanes(eOAS.CommonPlane).Poly
        End If

        pTopo = IntermediateFullArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        IntermediateWorkArea = pTopo.Difference(pTmpPolygon)
        pTopo = IntermediateWorkArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        fHalfFAPWidth = 0.5 * ReturnDistanceInMeters(ptRight, ptLeft)
        GetIntermObstacleList(ObstacleList, IntermObstacles, IntermediateWorkArea, FicTHRprj, _ArDir)

        'fDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), fhFAP, 0.0)

        M = UBound(IntermObstacles.Obstacles)
        N = UBound(IntermObstacles.Parts)
        I = 0

        While I <= N
            fTmp = System.Math.Round(IntermObstacles.Parts(I).Dist - fDist + 0.4999) + 20.0
            If fTmp > arMinISlen Then
                IntermObstacles.Parts(I).Rmin = fTmp
                IntermObstacles.Parts(I).MOC = 2.0 * arISegmentMOC.Value * (1.0 - IntermObstacles.Parts(I).DistStar / arIFHalfWidth.Value)
            ElseIf fTmp > 0.0 Then
                IntermObstacles.Parts(I).Rmin = arMinISlen
                IntermObstacles.Parts(I).MOC = 2.0 * arISegmentMOC.Value * (arIFHalfWidth.Value - fHalfFAPWidth - arMinISlen * (IntermObstacles.Parts(I).DistStar - fHalfFAPWidth) / fTmp) / arIFHalfWidth.Value
            Else
                IntermObstacles.Parts(I).Rmin = arMinISlen
                IntermObstacles.Parts(I).MOC = arISegmentMOC.Value
            End If

            If IntermObstacles.Parts(I).MOC > arISegmentMOC.Value Then IntermObstacles.Parts(I).MOC = arISegmentMOC.Value
            IntermObstacles.Parts(I).hPenet = System.Math.Round(IntermObstacles.Parts(I).Height + IntermObstacles.Parts(I).MOC - fhFAP + 0.4999)

            If IntermObstacles.Parts(I).hPenet > 0.0 Then
                fTmp1 = IntermObstacles.Parts(I).DistStar - fHalfFAPWidth
                If (fTmp <= 0.0) Or (fTmp1 <= 0.0) Then
                    IntermObstacles.Parts(I).Rmax = fMaxInterLenght
                Else
                    fDh = fhFAP - IntermObstacles.Parts(I).Height

                    If fDh < 0.0 Then
                        IntermObstacles.Parts(I).Rmax = fTmp * (arIFHalfWidth.Value - fHalfFAPWidth) / fTmp1
                    Else
                        IntermObstacles.Parts(I).Rmax = fTmp * (arIFHalfWidth.Value * (1.0 - 0.5 * fDh / arISegmentMOC.Value) - fHalfFAPWidth) / fTmp1
                    End If

                    If IntermObstacles.Parts(I).Rmax > fMaxInterLenght Then IntermObstacles.Parts(I).Rmax = fMaxInterLenght
                End If
                I += 1
            Else
                L = IntermObstacles.Parts(I).Owner
                K = IntermObstacles.Parts(I).Index
                IntermObstacles.Obstacles(L).PartsNum -= 1
                IntermObstacles.Obstacles(L).Parts(K) = IntermObstacles.Obstacles(L).Parts(IntermObstacles.Obstacles(L).PartsNum)
                IntermObstacles.Parts(IntermObstacles.Obstacles(L).Parts(K)).Index = K

                IntermObstacles.Parts(I) = IntermObstacles.Parts(N)
                IntermObstacles.Obstacles(IntermObstacles.Parts(I).Owner).Parts(IntermObstacles.Parts(I).Index) = I
                N -= 1
            End If
        End While

        Dim ImRange() As LowHigh
        Dim ResRange() As LowHigh
        Dim ObsRange As LowHigh

        ReDim ImRange(0)

        ImRange(0).Low = arMinISlen
        ImRange(0).High = fMaxInterLenght
        ImRange(0).Tag = -1

        Ix = -1

        If N < 0 Then
            ReDim IntermObstacles.Parts(-1)
            Return ImRange
        Else
            ReDim Preserve IntermObstacles.Parts(N)
        End If

        Sort(IntermObstacles, 0)

        For I = 0 To N

            If IntermObstacles.Parts(I).hPenet <= 0.0 Then Continue For
            P = UBound(ImRange)

            ObsRange.Low = IntermObstacles.Parts(I).Rmin
            ObsRange.High = IntermObstacles.Parts(I).Rmax

            K = 0
            While K <= P
                ResRange = LowHighDifference(ImRange(K), ObsRange)
                L = UBound(ResRange)
                If L >= 0 Then
                    If (ImRange(K).Low <> ResRange(0).Low) Or (ImRange(K).High <> ResRange(0).High) Then
                        ImRange(K) = ResRange(0)
                        ImRange(K).Tag = I
                    End If

                    If L > 0 Then
                        P = P + 1
                        ReDim Preserve ImRange(P)
                        For O = P To K + 2 Step -1
                            ImRange(O) = ImRange(O - 1)
                        Next O
                        ImRange(K + 1) = ResRange(1)
                        ImRange(K + 1).Tag = I
                        K = K + 2
                    Else
                        K = K + 1
                    End If
                ElseIf L < 0 Then
                    For O = K To P - 1
                        ImRange(O) = ImRange(O + 1)
                    Next O

                    P = P - 1
                    If P < 0 Then
                        ReDim ImRange(-1)
                        Ix = I
                        Exit For
                    Else
                        ReDim Preserve ImRange(P)
                    End If
                End If
            End While
        Next I

        Return ImRange
    End Function

    Private Function CalcNewHFAP(ByRef Ix As Integer, ByRef ObstList As ObstacleContainer, ByRef hFAP As Double) As Double
        Dim fTmp As Double
        Dim MinNewRange As Double
        Dim OldFAPRange As Double
        Dim A As Double
        Dim B As Double
        Dim C As Double
        Dim D As Double

        Dim Zx As Double
        Dim Zw As Double

        Dim Ap As Double
        Dim Bp As Double
        Dim Cp As Double

        Dim X0 As Double
        Dim X1 As Double
        'Dim X2 As Double
        Dim X4 As Double
        Dim X5 As Double

        Dim I As Integer
        Dim Sol1 As Integer
        Dim Sol2 As Integer

        Dim TanGPA As Double
        Dim Coef300 As Double

        TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
        Coef300 = 0.5 / arISegmentMOC.Value
        OldFAPRange = hFAP2FAPDist(hFAP)
        '    OldFAPRange = (hFAP - ILS.GP_RDH) / TanGPA

        MinNewRange = 40000000.0
        Ap = wOASPlanes(eOAS.XrPlane).Plane.A
        Bp = wOASPlanes(eOAS.XrPlane).Plane.B
        Cp = wOASPlanes(eOAS.XrPlane).Plane.D

        For I = 0 To Ix
            Zx = wOASPlanes(eOAS.XrPlane).Plane.A * ObstList.Parts(I).Dist + wOASPlanes(eOAS.XrPlane).Plane.B * ObstList.Parts(I).DistStar + wOASPlanes(eOAS.XrPlane).Plane.D
            Zw = wOASPlanes(eOAS.WPlane).Plane.A * ObstList.Parts(I).Dist + wOASPlanes(eOAS.WPlane).Plane.B * ObstList.Parts(I).DistStar + wOASPlanes(eOAS.WPlane).Plane.D

            If Zx > Zw Then
                X4 = OldFAPRange + (Zx + arISegmentMOC.Value - hFAP) / TanGPA
            Else
                X4 = OldFAPRange + (Zw + arISegmentMOC.Value - hFAP) / TanGPA
            End If

            X5 = (ObstList.Parts(I).MOC + ObstList.Parts(I).Height - hFAP) / TanGPA + OldFAPRange

            If hFAP > ObstList.Parts(I).Height Then
                A = TanGPA * Coef300 + (TanGPA - Ap) / (Bp * arIFHalfWidth.Value)
                B = ObstList.Parts(I).Rmin * (TanGPA - Ap) / (Bp * arIFHalfWidth.Value) - ObstList.Parts(I).Dist * A + (ILS.GP_RDH - ObstList.Parts(I).Height) * Coef300 + (ILS.GP_RDH - Cp) / (Bp * arIFHalfWidth.Value) - 1.0
                C = -ObstList.Parts(I).Rmin * (ObstList.Parts(I).DistStar - (ILS.GP_RDH - Cp) / Bp) / arIFHalfWidth.Value - ObstList.Parts(I).Dist * ((ILS.GP_RDH - ObstList.Parts(I).Height) * Coef300 + (ILS.GP_RDH - Cp) / (Bp * arIFHalfWidth.Value) - 1.0)
            Else
                A = (TanGPA - Ap) / (Bp * arIFHalfWidth.Value)
                B = (ObstList.Parts(I).Dist - ObstList.Parts(I).Rmin) * A + (ILS.GP_RDH - Cp) / (Bp * arIFHalfWidth.Value) - 1.0
                C = ObstList.Parts(I).Dist - ObstList.Parts(I).Rmin * ObstList.Parts(I).DistStar / arIFHalfWidth.Value - (ObstList.Parts(I).Dist - ObstList.Parts(I).Rmin) * (ILS.GP_RDH - Cp) / (Bp * arIFHalfWidth.Value)
            End If

            Sol1 = Quadric(A, B, C, X0, X1)
            If (Sol1 = 2) And (X0 <= OldFAPRange) Then Sol1 = 0

            Sol2 = 0

            Select Case 3 * Sol2 + Sol1
                Case 0, 1, 3, 4
                    fTmp = Min(X4, X5)
                Case 2, 5
                    fTmp = Min(X0, Min(X4, X5))
                Case 6, 7
                    'fTmp = Min(X2, Min(X4, X5))
                    fTmp = Min(X4, X5)
                Case 8
                    'fTmp = Min(X0, Min(X2, Min(X4, X5)))
                    fTmp = Min(X0, Min(X4, X5))
            End Select

            If fTmp <= OldFAPRange Then
                D = 0.5 / TanGPA
                fTmp = OldFAPRange + D '0.5
            End If
            If MinNewRange > fTmp Then MinNewRange = fTmp + 4.0
        Next I

        'CalcNewHFAP = MinNewRange * TanGPA + ILS.GP_RDH
        CalcNewHFAP = FAPDist2hFAP(MinNewRange)
    End Function

    Private Function CalcFAPOCH(ByRef fhFAP As Double, ByRef f_MOC As Double, ByRef Ix As Integer) As Double
        Dim I As Integer
        Dim N As Integer
        Dim fTmp As Double
        Dim bUnder15 As Integer
        Dim ZSurfaceOrigin As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        N = UBound(WorkObstacleList.Parts)
        CalcFAPOCH = IIf(f_MOC > fRDHOCH, f_MOC, fRDHOCH)
        Ix = -1

        For I = N To 0 Step -1
            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).IgnoredByUser Then Continue For
            If CheckBox0101.Enabled And CheckBox0101.Checked And (WorkObstacleList.Parts(I).Dist > FAPEarlierToler) Then Continue For

            If (WorkObstacleList.Parts(I).hPenet > 0.0) And (WorkObstacleList.Parts(I).ReqH <= fhFAP) Then 'And (WorkObstacleList(I).Dist >= -ZSurfaceOrigin)
                If CheckBox0001.Checked And (WorkObstacleList.Parts(I).Dist >= -ZSurfaceOrigin) Then
                    WorkObstacleList.Parts(I).ReqOCH = WorkObstacleList.Parts(I).Height + f_MOC
                Else
                    WorkObstacleList.Parts(I).ReqOCH = Min(WorkObstacleList.Parts(I).Height, WorkObstacleList.Parts(I).EffectiveHeight) + f_MOC
                End If

                bUnder15 = 0
                If CheckBox0101.Enabled And CheckBox0101.Checked Then
                    fTmp = FAPEarlierToler - WorkObstacleList.Parts(I).Dist
                    If fTmp < arFIX15PlaneRang.Value Then
                        fTmp = fTmp * arFixMaxIgnorGrd.Value
                        bUnder15 = CInt(fhFAP - arISegmentMOC.Value - fTmp > WorkObstacleList.Parts(I).Height) And 16
                    End If
                End If

                WorkObstacleList.Parts(I).Plane = (WorkObstacleList.Parts(I).Plane And (Not 16)) Or bUnder15

                If bUnder15 = 0 Then
                    If WorkObstacleList.Parts(I).ReqOCH > CalcFAPOCH Then
                        CalcFAPOCH = WorkObstacleList.Parts(I).ReqOCH
                        Ix = I
                    End If

                    If WorkObstacleList.Parts(I).ReqOCH > fhFAP Then fhFAP = WorkObstacleList.Parts(I).ReqOCH
                End If
            Else
                WorkObstacleList.Parts(I).ReqOCH = 0.0
            End If
        Next I

        'TextBox0103.Text = CStr(ConvertHeight(CalcFAPOCH, eRoundMode.rmNERAEST))

        'If Ix > -1 Then
        '	TextBox0104.Text = WorkObstacleList(Ix).ID
        'Else
        '	TextBox0104.Text = ""
        'End If
    End Function

    Private Sub Caller(ByRef OCH As Double, ByRef hFAP As Double)
        Dim I As Integer
        Dim J As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim Ix As Integer

        Dim fTmp As Double
        Dim FAPDist As Double
        Dim distEps2 As Double

        Dim tmpStr As String

        Dim hRange As LowHigh
        Dim ImIntervals() As LowHigh

        Dim itmX As System.Windows.Forms.ListViewItem

        Dim pTmpPolygon As ESRI.ArcGIS.Geometry.IPolygon
        Dim OldNav As NavaidData
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        ShowPandaBox(Me.Handle.ToInt32)
        '================ Initial
        FAPDist = hFAP2FAPDist(hFAP)

        ptFAP = PointAlongPlane(FicTHRprj, _ArDir + 180.0, FAPDist)
        ptFAP.Z = hFAP
        ptFAP.M = _ArDir                 'DrawPointWithText PtFAP, "ptFAP"
        OldNav.CallSign = ""

        If CheckBox0101.Enabled And CheckBox0101.Checked And (ComboBox0101.SelectedIndex >= 0) Then
            OldNav = FAPNavDat(ComboBox0101.SelectedIndex)
            CreatePlane15(OldNav, -1)
        End If

        IxOCH = -1
        OCH = CalcFAPOCH(hFAP, m_fMOC, IxOCH)

        ExcludeBtn.Enabled = (IxOCH >= 0) Or HaveExcluded
        '================ Initial

        distEps2 = distEps * distEps
        hRange.Low = 0.0
        bHaveSolution = True
        N = UBound(WorkObstacleList.Parts)

        Do
            Ix = -1

            For I = N To 0 Step -1
                'If (WorkObstacleList(i).ReqH > hFAP) And (WorkObstacleList(i).Height + fMOC > OCH) Then
                If (WorkObstacleList.Parts(I).ReqH > hFAP) And (WorkObstacleList.Parts(I).hPenet > 0) Then
                    Ix = I
                    Exit For
                End If
            Next

            If Ix < 0 Then
                hRange.High = FAPDist2hFAP(arFAPMaxRange.Value)
            Else
                hRange.High = WorkObstacleList.Parts(Ix).ReqH
            End If

            ImIntervals = CalcImIntervals(hFAP, IntermObstacleList, Ix)

            If Ix >= 0 Then
                '            fTmp = CalcNewHFAP(Ix, WorkObstacleList, hFAP)
                fTmp = CalcNewHFAP(Ix, IntermObstacleList, hFAP)
                If System.Math.Abs(fTmp - hFAP) < distEps2 Then
                    bHaveSolution = False
                    HidePandaBox()
                    MsgBox(My.Resources.str00303, MsgBoxStyle.Critical, "PANDA")
                    Return
                End If

                '            hFAP = CalcNewHFAP(Ix, WorkObstacleList, hFAP)
                hFAP = fTmp

                FAPDist = hFAP2FAPDist(hFAP)
                ptFAP = PointAlongPlane(FicTHRprj, _ArDir + 180.0, FAPDist)
                ptFAP.Z = hFAP
                ptFAP.M = _ArDir

                If CheckBox0101.Enabled And CheckBox0101.Checked And (ComboBox0101.SelectedIndex >= 0) Then
                    OldNav = FAPNavDat(ComboBox0101.SelectedIndex)
                    CreatePlane15(OldNav, -1)
                End If

                OCH = CalcFAPOCH(hFAP, m_fMOC, IxOCH)
            End If
        Loop While Ix >= 0

        '========================================================
        If ComboBox0104.SelectedIndex < 0 Then
            ComboBox0104.SelectedIndex = 0
        Else
            ComboBox0104_SelectedIndexChanged(ComboBox0104, New System.EventArgs())
        End If

        If IxOCH > -1 Then
            TextBox0104.Text = WorkObstacleList.Obstacles(WorkObstacleList.Parts(IxOCH).Owner).UnicalName
            ToolTip1.SetToolTip(_Label0101_5, WorkObstacleList.Obstacles(WorkObstacleList.Parts(IxOCH).Owner).UnicalName)
        Else
            TextBox0104.Text = ""
            ToolTip1.SetToolTip(_Label0101_5, "")
        End If
        '========================================================

        If (UBound(ImIntervals) = 0) And (ImIntervals(0).High < ImIntervals(0).Low) Then
            bHaveSolution = False
            HidePandaBox()
            MsgBox(My.Resources.str00303, MsgBoxStyle.Critical, "PANDA")
            Return
        End If

        FAPDist = hFAP2FAPDist(hFAP)
        fFAPDist = FAPDist

        '========================================================
        'TextBox0103.Text = CStr(ConvertHeight(OCH, eRoundMode.rmNERAEST))

        'If IxOCH > -1 Then
        '	TextBox0104.Text = WorkObstacleList(IxOCH).ID
        'Else
        '	TextBox0104.Text = ""
        'End If
        '========================================================

        If Ix >= 0 Then
            TextBox0105.Text = CStr(ConvertHeight(hRange.High, eRoundMode.NEAREST))
        Else
            TextBox0105.Text = ""
        End If

        '============================================================================
        '    N = UBound(wOASPlanes)
        '    Set pTopo = wOASPlanes(eOAS.CommonPlane).Poly
        '    Set pTmpPolygon = pTopo.Difference(wOASPlanes(eOAS.YrPlane).Poly)
        '    Set pTopo = pTmpPolygon
        '    pTopo.IsKnownSimple_2 = False
        '    pTopo.Simplify
        '    Set pTmpPolygon = pTopo.Difference(wOASPlanes(eOAS.YlPlane).Poly)
        '    Set pTopo = pTmpPolygon
        '    pTopo.IsKnownSimple_2 = False
        '    pTopo.Simplify
        '============================================================================
        '
        '    Set pTopo = IntermediateFullArea
        '    Set pTmpPoly = pTopo.Difference(pTmpPolygon)

        On Error Resume Next

        If Not IntermediateFullAreaElem Is Nothing Then If Not IntermediateFullAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateFullAreaElem)

        IntermediateFullAreaElem = DrawPolygon(IntermediateWorkArea, 255)
        IntermediateFullAreaElem.Locked = True

        N = UBound(wOASPlanes)
        For I = 0 To N
            If Not OASPlanesCat1Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat1Element(I))

            pTopo = wOASPlanes(I).Poly
            pTmpPolygon = pTopo.Difference(IntermediateWorkArea)

            If Not pTmpPolygon.IsEmpty() Then
                OASPlanesCat1Element(I) = DrawPolygon(pTmpPolygon, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, OASPlanesCat1State)
                OASPlanesCat1Element(I).Locked = True
            End If
        Next I
        CommandBar.isEnabled(CommandBar.wOAS) = True
        '==============================================================================================================================
        Dim pRightPolygon As ESRI.ArcGIS.Geometry.IPolygon
        Dim pCutPoly As ESRI.ArcGIS.Geometry.IPolyline

        pCutPoly = New ESRI.ArcGIS.Geometry.Polyline()
        pCutPoly.FromPoint = PointAlongPlane(ptFAP, _ArDir + 90.0, 2 * RModel + 10000.0)
        pCutPoly.ToPoint = PointAlongPlane(ptFAP, _ArDir - 90.0, 2 * RModel + 10000.0)

        N = UBound(ILSPlanes)
        For I = 0 To N
            If Not ILSPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(ILSPlanesElement(I))
            pRightPolygon = Nothing
            pTmpPolygon = Nothing
            pTopo = ILSPlanes(I).Poly
            pTopo.Cut(pCutPoly, pTmpPolygon, pRightPolygon)

            If (pTmpPolygon Is Nothing) Or pTmpPolygon.IsEmpty Then
                pTmpPolygon = ILSPlanes(I).Poly
            End If

            ILSPlanesElement(I) = DrawPolygon(pTmpPolygon, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, ILSPlanesState)
            ILSPlanesElement(I).Locked = True
        Next I
        CommandBar.isEnabled(CommandBar.ILS) = True
        '==============================================================================================================================

        If Not FAPElem Is Nothing Then pGraphics.DeleteElement(FAPElem)
        'Set FAPElem = DrawPoint(ptFAP, 255)
        FAPElem = DrawPointWithText(ptFAP, "FAP", WPTColor)
        FAPElem.Locked = True

        On Error GoTo 0

        Dim fFlg As Boolean

        ' ILS Obstacles ========================================================
        Dim K As Integer
        M = UBound(ILSObstacleList.Obstacles)
        N = UBound(ILSObstacleList.Parts)

        If N >= 0 Then
            ReDim PrecisionObstacleList.Obstacles(M)
            ReDim PrecisionObstacleList.Parts(N)
        Else
            ReDim PrecisionObstacleList.Obstacles(-1)
            ReDim PrecisionObstacleList.Parts(-1)
        End If

        For I = 0 To M
            ILSObstacleList.Obstacles(I).NIx = -1
        Next

        K = -1
        L = -1
        For I = 0 To N
            If ILSObstacleList.Parts(I).ReqH > hFAP Then Continue For
            K = K + 1
            PrecisionObstacleList.Parts(K) = ILSObstacleList.Parts(I)

            If ILSObstacleList.Obstacles(ILSObstacleList.Parts(I).Owner).NIx < 0 Then
                L += 1
                PrecisionObstacleList.Obstacles(L) = ILSObstacleList.Obstacles(ILSObstacleList.Parts(I).Owner)
                PrecisionObstacleList.Obstacles(L).PartsNum = 0
                ReDim PrecisionObstacleList.Obstacles(L).Parts(ILSObstacleList.Obstacles(ILSObstacleList.Parts(I).Owner).PartsNum - 1)
                ILSObstacleList.Obstacles(ILSObstacleList.Parts(I).Owner).NIx = L
            End If

            PrecisionObstacleList.Parts(K).Owner = ILSObstacleList.Obstacles(ILSObstacleList.Parts(I).Owner).NIx
            PrecisionObstacleList.Parts(K).Index = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum) = K
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum += 1
        Next I

        'For I = 0 To M
        '	If ILSObstacleList.Obstacles(I).PartsNum = 0 Then
        '		J = I
        '	End If
        'Next

        If K >= 0 Then
            ReDim Preserve PrecisionObstacleList.Parts(K)
            ReDim Preserve PrecisionObstacleList.Obstacles(L)
        Else
            ReDim PrecisionObstacleList.Parts(-1)
            ReDim PrecisionObstacleList.Obstacles(-1)
        End If

        PrecReportFrm.FillPage02(PrecisionObstacleList)

        'OAS Obstacles ========================================================
        'I = 52
        'DrawPolygon(OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).pGeomPrj, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSVertical)
        'DrawPointWithText(OASObstacleList.Parts(I).pPtPrj, OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).Name + " / " + OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).ID)

        'I = 0
        'While I = 0
        '	Application.DoEvents()
        'End While

        M = UBound(OASObstacleList.Obstacles)
        N = UBound(OASObstacleList.Parts)

        If N >= 0 Then
            ReDim PrecisionObstacleList.Obstacles(M)
            ReDim PrecisionObstacleList.Parts(N)
        Else
            ReDim PrecisionObstacleList.Obstacles(-1)
            ReDim PrecisionObstacleList.Parts(-1)
        End If

        For I = 0 To M
            OASObstacleList.Obstacles(I).NIx = -1
        Next

        K = -1
        L = -1

        For I = 0 To N
            If OASObstacleList.Parts(I).ReqH > hFAP Then Continue For

            K += 1
            PrecisionObstacleList.Parts(K) = OASObstacleList.Parts(I)

            If OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).NIx < 0 Then
                L += 1
                PrecisionObstacleList.Obstacles(L) = OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner)
                PrecisionObstacleList.Obstacles(L).PartsNum = 0
                ReDim PrecisionObstacleList.Obstacles(L).Parts(OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).PartsNum - 1)
                OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).NIx = L
            End If

            PrecisionObstacleList.Parts(K).Owner = OASObstacleList.Obstacles(OASObstacleList.Parts(I).Owner).NIx
            PrecisionObstacleList.Parts(K).Index = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum) = K
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum += 1
        Next I

        'For I = 0 To M
        '	If OASObstacleList.Obstacles(I).PartsNum = 0 Then
        '		J = I
        '	End If
        'Next

        If K >= 0 Then
            ReDim Preserve PrecisionObstacleList.Parts(K)
            ReDim Preserve PrecisionObstacleList.Obstacles(L)
        Else
            ReDim PrecisionObstacleList.Parts(-1)
            ReDim PrecisionObstacleList.Obstacles(-1)
        End If

        'PrecReportFrm.FillPage03(OASObstacleList)
        PrecReportFrm.FillPage03(PrecisionObstacleList)
        ' Work Obstacles ========================================================

        M = UBound(WorkObstacleList.Obstacles)
        N = UBound(WorkObstacleList.Parts)

        If N >= 0 Then
            ReDim PrecisionObstacleList.Obstacles(M)
            ReDim PrecisionObstacleList.Parts(N)
        Else
            ReDim PrecisionObstacleList.Obstacles(-1)
            ReDim PrecisionObstacleList.Parts(-1)
        End If

        For I = 0 To M
            WorkObstacleList.Obstacles(I).NIx = -1
        Next

        K = -1
        L = -1
        finalAssessedAltitude = 0

        For I = 0 To N
            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).IgnoredByUser Then Continue For
            If CheckBox0101.Enabled And CheckBox0101.Checked And (WorkObstacleList.Parts(I).Dist > FAPEarlierToler) Then Continue For
            If (WorkObstacleList.Parts(I).ReqH > hFAP) Then Continue For

            K += 1
            PrecisionObstacleList.Parts(K) = WorkObstacleList.Parts(I)

            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                L += 1
                PrecisionObstacleList.Obstacles(L) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                PrecisionObstacleList.Obstacles(L).PartsNum = 0
                ReDim PrecisionObstacleList.Obstacles(L).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = L
            End If

            PrecisionObstacleList.Parts(K).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
            PrecisionObstacleList.Parts(K).Index = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum) = K
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum += 1

            If WorkObstacleList.Parts(I).Height < WorkObstacleList.Parts(I).EffectiveHeight Then
                If finalAssessedAltitude < WorkObstacleList.Parts(I).ReqOCH Then
                    finalAssessedAltitude = WorkObstacleList.Parts(I).ReqOCH
                End If
            End If


            'bUnder15 = 0
            'If CheckBox0101.Enabled And CheckBox0101.Checked Then
            '	fTmp = FAPEarlierToler - WorkObstacleList.Parts(I).Dist
            '	If fTmp < arFIX15PlaneRang.Value Then
            '		fTmp = fTmp * arFixMaxIgnorGrd.Value
            '		bUnder15 = CInt(fhFAP - arISegmentMOC.Value - fTmp > WorkObstacleList.Parts(I).Height) And 16
            '	End If
            'End If
        Next I

        If K >= 0 Then
            ReDim Preserve PrecisionObstacleList.Parts(K)
            ReDim Preserve PrecisionObstacleList.Obstacles(L)
        Else
            ReDim PrecisionObstacleList.Parts(-1)
            ReDim PrecisionObstacleList.Obstacles(-1)
        End If

        PrecReportFrm.FillPage04(PrecisionObstacleList)

        '========================================================
        N = UBound(ImIntervals)

        ListView0101.Items.Clear()

        For I = 0 To N
            tmpStr = CStr(ConvertDistance(ImIntervals(I).Low, 3))
            itmX = ListView0101.Items.Add(tmpStr)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(ImIntervals(I).High, 1))))
            If ImIntervals(I).Tag >= 0 Then
                itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, IntermObstacleList.Obstacles(IntermObstacleList.Parts(ImIntervals(I).Tag).Owner).UnicalName))
            End If
        Next I

        If N >= 0 Then
            fNearDist = ImIntervals(0).Low
            fFarDist = ImIntervals(0).High

            itmX = ListView0101.Items.Item(0)
            itmX.Checked = True

            ListView0101.SelectedIndices.Clear()
            ListView0101.SelectedIndices.Add(itmX.Index)
            ListView0101_ItemChecked(ListView0101, New System.Windows.Forms.ItemCheckedEventArgs(itmX))
        End If

        '==================================================================================================

        Dim Side As Integer
        Dim fDist As Double
        Dim MinFAPDist As Double
        Dim lNavDat As NavaidData

        FAPNavDat = GetValidFAPNavs(FicTHRprj, 2.0 * fFAPDist, _ArDir, ptFAP, ptFAP.Z, 3, ILS.pPtPrj)
        K = 0

        M = UBound(FAPNavDat)
        ComboBox0101.Items.Clear()
        For I = 0 To M
            ComboBox0101.Items.Add(FAPNavDat(I))
            If FAPNavDat(I).Identifier.Equals(OldNav.Identifier) And (FAPNavDat(I).TypeCode = OldNav.TypeCode) Then
                K = I
            End If
        Next I

        N = UBound(InSectList)
        If N >= 0 Then
            If M >= 0 Then
                ReDim Preserve FAPNavDat(N + M + 2)
            Else
                ReDim FAPNavDat(N)
            End If

            MinFAPDist = (arISegmentMOC.Value - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))

            For I = 0 To N
                If (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then Continue For

                Side = SideDef(FicTHRprj, _ArDir - 90.0, InSectList(I).pPtPrj)
                fTmp = Point2LineDistancePrj(FicTHRprj, InSectList(I).pPtPrj, _ArDir + 90.0)

                If (Side > 0) And (fTmp < arFAPMaxRange.Value) And (fTmp > MinFAPDist) Then
                    lNavDat = InSectList(I)

                    lNavDat.ValCnt = -2
                    lNavDat.IntersectionType = eIntersectionType.OnNavaid

                    M = M + 1
                    FAPNavDat(M) = lNavDat
                    ComboBox0101.Items.Add(lNavDat.CallSign)

                    If (lNavDat.CallSign = OldNav.CallSign) And (lNavDat.TypeCode = OldNav.TypeCode) Then
                        K = M
                    End If
                End If
            Next I
        End If

        ArrivalProfile.ClearPoints()
        ArrivalProfile.AddPoint(FAPDist, hFAP, ILS.Course - ILS.MagVar, -ILS.GPAngle, CodeProcedureDistance.PFAF)
        fDist = FAPDist - (hFAP - OCH) / System.Math.Tan(DegToRad(ILS.GPAngle))
        ArrivalProfile.AddPoint(fDist, OCH, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), CodeProcedureDistance.MAP)

        CheckBox0101.Enabled = M > -1
        If M > -1 Then
            ReDim Preserve FAPNavDat(M)
            ComboBox0101.SelectedIndex = K
        Else
            ReDim FAPNavDat(-1)
        End If
        bNavFlg = False
        HidePandaBox()
        HidePandaBox()
    End Sub

    Private Sub NavInSector(ByRef pRefPt As ESRI.ArcGIS.Geometry.IPoint, ByRef InList() As NavaidData, ByRef OutList() As NavaidData)
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim d0 As Double
        Dim tmpDist As Double

        N = UBound(InList)
        If (N < 0) Then
            ReDim OutList(-1)
            Return
        Else
            ReDim OutList(N + 1)
        End If

        J = 0

        For I = 0 To N
            If (InList(I).TypeCode = eNavaidType.VOR) Or (InList(I).TypeCode = eNavaidType.TACAN) Then
                d0 = VOR.OnNAVRadius
            ElseIf InList(I).TypeCode = eNavaidType.NDB Then
                d0 = NDB.OnNAVRadius
            Else
                d0 = -10000.0
            End If

            tmpDist = Point2LineDistancePrj(InList(I).pPtPrj, pRefPt, _ArDir)

            If tmpDist <= d0 Then
                OutList(J) = InList(I)
                OutList(J).Distance = Point2LineDistancePrj(OutList(J).pPtPrj, pRefPt, _ArDir - 90.0)
                J = J + 1
            End If
        Next I

        If J = 0 Then
            ReDim OutList(-1)
        Else
            ReDim Preserve OutList(J - 1)
        End If

    End Sub

    Private Sub WPTInSector(ByRef pRefPt As ESRI.ArcGIS.Geometry.IPoint, ByRef InList() As WPT_FIXType, ByRef OutList() As NavaidData)
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer

        Dim tmpDist As Double
        Dim d0 As Double
        Dim X As Double
        Dim Y As Double

        N = InList.Length
        ReDim OutList(N - 1)
        If N <= 0 Then Return

        'DrawPolygon(pSector, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal)
        'Application.DoEvents()

        J = -1

        For I = 0 To N - 1
            PrjToLocal(pRefPt, _ArDir, InList(I).pPtPrj, X, Y)  'pRefPt.M
            tmpDist = Math.Abs(Y)

            If InList(I).TypeCode = eNavaidType.NDB Then
                d0 = NDB.OnNAVRadius
            ElseIf (InList(I).TypeCode = eNavaidType.VOR) Or (InList(I).TypeCode = eNavaidType.TACAN) Then
                d0 = VOR.OnNAVRadius
            Else
                d0 = 20.0
            End If

            d0 = 0.1
            If tmpDist > d0 Then Continue For

            'If X > 0 Then Continue For

            J = J + 1

            If (InList(I).TypeCode = eNavaidType.VOR) Or (InList(I).TypeCode = eNavaidType.NDB) Or (InList(I).TypeCode = eNavaidType.TACAN) Then
                FindNavaid(InList(I).Name, InList(I).TypeCode, OutList(J))
            Else
                OutList(J) = WPT_FIXToNavaid(InList(I))
            End If
            OutList(J).Distance = -X
        Next I

        If J < 0 Then
            ReDim OutList(-1)
        Else
            ReDim Preserve OutList(J)
        End If
    End Sub

#Region "Page 0 events"

    Private Sub ComboBox0001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0001.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim K As Integer
        Dim M As Integer
        'Dim N As Integer

        Dim fTmp As Double
        Dim ResX As Double
        Dim ResY As Double
        Dim RWYDir As Double
        Dim ILSDir As Double

        Dim pPtBase As ESRI.ArcGIS.Geometry.IPoint
        Dim BaseSight As Integer
        Dim BaseDist As Double
        Dim Dist55 As Double
        Dim Dist200 As Double

        Dim bFlg As Boolean

        Dim RUM() As String = {"I", "II", "III"}

        _InfoFrm.ResetTHRFields()
        K = ComboBox0001.SelectedIndex
        If K < 0 Then Return

        SelectedRWY = RWYList(K)

        M = GetILS(SelectedRWY, ILS, CurrADHP)
        If M <> 3 Then Return

        fTmp = ILS.Course - SelectedRWY.pPtGeo(eRWY.PtTHR).M
        If fTmp < 0 Then fTmp = fTmp + 360.0
        If fTmp > 180.0 Then fTmp = 360.0 - fTmp
        _InfoFrm.SetDeltaAngle(fTmp)

        RWYDir = SelectedRWY.pPtPrj(eRWY.PtTHR).M
        ILSDir = ILS.pPtPrj.M

        'GetMRKList(MkrList, ILS)
        For I = 0 To UBound(ILS.MkrList)
            ILS.MkrList(I).DistFromTHR = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.MkrList(I).pPtPrj, ILSDir + 90.0)
            ILS.MkrList(I).Height = FAPDist2hFAP(ILS.MkrList(I).DistFromTHR)
            ILS.MkrList(I).Altitude = SelectedRWY.pPtGeo(eRWY.PtTHR).Z + ILS.MkrList(I).Height
        Next

        Dist55 = (MinGPIntersectHeight - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))
        Dist200 = (MaxGPIntersectHeight - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))
        bFlg = True
        fRDHOCH = 0.0

        pPtBase = LineLineIntersect(SelectedRWY.pPtPrj(eRWY.PtTHR), RWYDir, ILS.pPtPrj, ILSDir)

        If fTmp < OffsetTreshold Then
            ResY = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.pPtPrj, ILSDir)
            BaseSight = SideDef(ILS.pPtPrj, ILSDir - 90.0, SelectedRWY.pPtPrj(eRWY.PtTHR))
            ResX = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.pPtPrj, ILSDir + 90.0)
            _InfoFrm.SetLocAlongDist(ResX * BaseSight)

            If (ResY / ResX < System.Math.Tan(DegToRad(OffsetTreshold))) Then
                BaseSight = SideDef(SelectedRWY.pPtPrj(eRWY.PtTHR), RWYDir + 90.0, ILS.pPtPrj)
                BaseDist = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.pPtPrj, ILSDir + 90.0) * BaseSight
                FicTHRprj = PointAlongPlane(ILS.pPtPrj, ILSDir + 180.0, BaseDist)
                bFlg = False
            ElseIf pPtBase.IsEmpty() Then
                MessageBox.Show("Invalid ILS")
                ComboBox0001.SelectedIndex = PrevCmbRWY
                Return
            End If

            _InfoFrm.SetLocAbeamDist(ResY * SideDef(ILS.pPtPrj, ILSDir + 180.0, SelectedRWY.pPtPrj(eRWY.PtTHR)))
        End If

        If bFlg Then
            If pPtBase.IsEmpty() Then
                MessageBox.Show("Invalid ILS")
                ComboBox0001.SelectedIndex = PrevCmbRWY
                Return
            End If

            BaseSight = SideDef(SelectedRWY.pPtPrj(eRWY.PtTHR), RWYDir - 90.0, pPtBase)
            BaseDist = ReturnDistanceInMeters(SelectedRWY.pPtPrj(eRWY.PtTHR), pPtBase) * BaseSight
            _InfoFrm.SetIntersectDistance(BaseDist)

            If (BaseDist >= Dist55) And (BaseDist <= Dist200) Then
                FicTHRprj = PointAlongPlane(pPtBase, ILSDir, BaseDist)
            Else
                MessageBox.Show("Invalid ILS")
                ComboBox0001.SelectedIndex = PrevCmbRWY
                Return
            End If

            ResY = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.pPtPrj, ILSDir)
            BaseSight = SideDef(ILS.pPtPrj, ILSDir - 90.0, SelectedRWY.pPtPrj(eRWY.PtTHR))
            ResX = Point2LineDistancePrj(SelectedRWY.pPtPrj(eRWY.PtTHR), ILS.pPtPrj, ILSDir + 90) * BaseSight
            _InfoFrm.SetLocAlongDist(ResX)
            _InfoFrm.SetLocAbeamDist(ResY * SideDef(ILS.pPtPrj, ILSDir + 180.0, SelectedRWY.pPtPrj(eRWY.PtTHR)))

            fRDHOCH = ILS.GP_RDH + BaseDist * System.Math.Tan(DegToRad(ILS.GPAngle)) + LocOffsetOCHAdd
            _InfoFrm.SetOCHLimit(fRDHOCH)
            ILS.Category = 1
        End If

        FicTHRprj.Z = SelectedRWY.pPtPrj(eRWY.PtTHR).Z
        ptLH = ToGeo(FicTHRprj)
        ptLH.M = ILS.Course
        FicTHRprj.M = ILSDir 'Azt2Dir(ptLH, ptLH.M)

        ILS.LLZ_THR = ReturnDistanceInMeters(ILS.pPtPrj, FicTHRprj)
        'ILS.SecWidth = ILS.LLZ_THR * Tan(DegToRad(ILS.AngleWidth))
        '=====================================================================

        If ComboBox0005.SelectedIndex < 0 Then
            ComboBox0005.SelectedIndex = 1
            Return
        End If

        TextBox0011.Text = CStr(System.Math.Round(ILS.SecWidth))

        If ILS.SecWidth > arILSSectorWidth.Value Then
            MessageBox.Show(My.Resources.str20121 + arILSSectorWidth.Value.ToString() + " " + My.Resources.str00021 + My.Resources.str20122)
            '210 m at threshold.
        End If

        PrevCmbRWY = K

        TextBox0009.Text = RUM(ILS.Category - 1)
        ComboBox0002.Items.Clear()

        Select Case ILS.Category
            Case 1
                ComboBox0002.Items.Add("Cat I")
            Case 2
                ComboBox0002.Items.Add("Cat I")
                ComboBox0002.Items.Add("Cat II")
            Case 3
                ComboBox0002.Items.Add("Cat I")
                ComboBox0002.Items.Add("Cat II")
                ComboBox0002.Items.Add("Cat II AUTO")
        End Select

        DistLLZ2THR = ILS.LLZ_THR

        TextBox0003.Text = CStr(ConvertDistance(DistLLZ2THR, eRoundMode.NEAREST))
        TextBox0010.Text = CStr(System.Math.Round(ILS.GPAngle, 2))
        TextBox0004.Text = CStr(ConvertHeight(ILS.GP_RDH, eRoundMode.NEAREST))

        _ArDir = FicTHRprj.M


        If GlobalVars._settings.AnnexObstalce = True Then
            GetAnnexObstacles(ObstacleList, CurrADHP.Identifier, FicTHRprj.Z)
        Else
            GetObstaclesByDist(ObstacleList, CurrADHP.pPtPrj, RModel, FicTHRprj.Z)
        End If


        pCircle = CreatePrjCircle(FicTHRprj, RModel)

        TextBox0002.Text = CStr(System.Math.Round(Modulus(ptLH.M, 360.0), 2))
        TextBox0008.Text = CStr(System.Math.Round(Modulus(ptLH.M - ILS.MagVar, 360.0), 2))
        TextBox0001.Text = CStr(ConvertHeight(ptLH.Z, eRoundMode.NEAREST))

        'ClearScr()
        'CreateILS23Planes(ptLHPrj, ArDir, InnerSurfaceHeight, ILS23Planes)

        'N = UBound(ILS23Planes)
        'For I = 0 To N
        '	If Not ILS23PlanesElement(I) Is Nothing Then pGraphics.DeleteElement(ILS23PlanesElement(I))
        '	ILS23PlanesElement(I) = DrawPolygon(ILS23Planes(I).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, ILS23PlaneState)
        '	ILS23PlanesElement(I).Locked = True
        'Next I
        'CommandBar.isEnabled(CommandBar.ILS23) = True

        ComboBox0002.SelectedIndex = 0
        ComboBox0103.SelectedIndex = 0
        ComboBox0202.SelectedIndex = 0
        ComboBox0301.SelectedIndex = 0
    End Sub

    Private Sub ComboBox0002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0002.SelectedIndexChanged
        If ComboBox0002.SelectedIndex < 0 Then Return

        Dim I As Integer
        Dim N As Integer
        Dim ILS23ObstacleList As ObstacleContainer
        Dim ErrorStr As String = "The value is outside the range of GP angle."
        ComboBox0004.Items.Clear()

        TextBox0010.ForeColor = SystemColors.WindowText
        ToolTip1.SetToolTip(TextBox0010, "")

        TextBox0012.Enabled = ComboBox0002.SelectedIndex > 0
        label0001_27.Enabled = ComboBox0002.SelectedIndex > 0
        label0001_28.Enabled = ComboBox0002.SelectedIndex > 0

        Select Case ComboBox0002.SelectedIndex
            Case 0
                If ILS.GPAngle < arMinGPAngle.Value Then
                    TextBox0010.ForeColor = Color.Red
                    ToolTip1.SetToolTip(TextBox0010, ErrorStr)
                    MessageBox.Show(ErrorStr)
                ElseIf ILS.GPAngle > arMaxGPAngleCat1.Value Then
                    TextBox0010.ForeColor = Color.Red
                    ToolTip1.SetToolTip(TextBox0010, ErrorStr)
                    MessageBox.Show(ErrorStr)
                End If

                ComboBox0004.Items.Add(My.Resources.str20117) '"Рад. Высотомер"
                ComboBox0004.Items.Add(My.Resources.str20118) '"Бар. Высотомер"
                ComboBox0004.SelectedIndex = 1

                ReDim ILS23ObstacleList.Obstacles(-1)
                ReDim ILS23ObstacleList.Parts(-1)
                PrecReportFrm.FillPage01(ILS23ObstacleList)
            Case 1, 2
                If arMinGPAngle.Value > ILS.GPAngle Then
                    TextBox0010.ForeColor = Color.Red
                    ToolTip1.SetToolTip(TextBox0010, ErrorStr)
                    MessageBox.Show(ErrorStr)
                ElseIf arMaxGPAngleCat2.Value < ILS.GPAngle Then
                    TextBox0010.ForeColor = Color.Red
                    ToolTip1.SetToolTip(TextBox0010, ErrorStr)
                    MessageBox.Show(ErrorStr)
                End If

                ComboBox0004.Items.Add(My.Resources.str20117) '"Рад. Высотомер"
                CreateOFZPlanes(FicTHRprj, _ArDir, InnerSurfaceHeight, OFZPlanes)

                N = UBound(OFZPlanes)

                For I = 0 To N
                    Try
                        If Not OFZPlanesElement(I) Is Nothing Then pGraphics.DeleteElement(OFZPlanesElement(I))
                    Catch
                    End Try

                    OFZPlanesElement(I) = DrawPolygon(OFZPlanes(I).Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross, OFZPlanesState)
                    OFZPlanesElement(I).Locked = True
                Next I

                CommandBar.isEnabled(CommandBar.OFZ) = True


                If AnaliseObstacles(ObstacleList, ILS23ObstacleList, FicTHRprj, _ArDir, OFZPlanes) Then
                    MessageBox.Show(My.Resources.str20302 + vbCrLf + My.Resources.str20303)
                    '        PrecReportFrm.FillPage8 ILS23ObstacleList
                    '        ComboBox0002.ListIndex = PrevCmb002
                    '        Return
                End If
                PrecReportFrm.FillPage01(ILS23ObstacleList)

                ComboBox0004.SelectedIndex = 0
        End Select

        'PrevCmb002 = ComboBox0002.SelectedIndex
        TextBox0404.Text = ComboBox0002.Text
    End Sub

    Private Sub ComboBox0003_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0003.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim fMOCCorrH As Double
        Dim fMOCCorrGP As Double
        Dim fMargin As Double
        Dim fRadMargin As Double
        Dim K As Integer

        Category = ComboBox0003.SelectedIndex
        K = ComboBox0003.SelectedIndex
        If K < 0 Then Return

        TextBox0403.Text = ComboBox0003.Text

        TextBox0005.Text = CStr(arSemiSpan.Values(K))
        TextBox0006.Text = CStr(arVerticalSize.Values(K))

        _Label0401_3.Text = My.Resources.str00220 + vbCrLf + My.Resources.str00221 + CStr(ConvertSpeed(cVmaInter.Values(K), eRoundMode.SPECIAL)) + My.Resources.str00222 + CStr(ConvertSpeed(cVmaFaf.Values(K), eRoundMode.SPECIAL)) + " " + SpeedConverter(SpeedUnit).Unit
        TextBox0402.Text = CStr(ConvertSpeed(cVmaFaf.Values(Category), eRoundMode.SPECIAL))

        fRadMargin = 0.34406047516199 * cVatMax.Values(K) - 3.2
        If ComboBox0004.SelectedIndex = 0 Then                      'Radio	'	fMargin = 0.096 / 0.277777777777778 * cVatMax.Values(k) - 3.2
            fMargin = fRadMargin                                                    ' 0.3456
        Else                                                        'Baro	'	fMargin = 0.068 / 0.277777777777778 * cVatMax.Values(k) + 28.3
            fMargin = 0.24298056155508 * cVatMax.Values(K) + 28.3                   ' 0.2448
        End If

        fMOCCorrH = 0.0
        If CurrADHP.pPtGeo.Z > 900.0 Then fMOCCorrH = CurrADHP.pPtGeo.Z * fRadMargin / 15000.0

        fMOCCorrGP = 0.0
        If ILS.GPAngle > 3.2 Then fMOCCorrGP = (ILS.GPAngle - 3.2) * fRadMargin * 0.5


        m_fMOC = fMargin + fMOCCorrGP + fMOCCorrH

        TextBox0007.Text = CStr(ConvertHeight(m_fMOC, eRoundMode.NEAREST))
    End Sub

    Private Sub ComboBox0004_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0004.SelectedIndexChanged
        If Not bFormInitialised Then Return

        ComboBox0003_SelectedIndexChanged(ComboBox0003, New System.EventArgs())
    End Sub

    Private Sub ComboBox0005_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0005.SelectedIndexChanged
        If Not bFormInitialised Then Return

        fMissAprPDG = 0.01 * CDbl(ComboBox0005.Text)

        RModel = (600.0 - _hCons) / (fMissAprPDG * System.Math.Cos(DegToRad(arMA_SplayAngle.Value)))
        If RModel < MinCalcAreaRadius Then RModel = MinCalcAreaRadius
        ComboBox0001_SelectedIndexChanged(ComboBox0001, New System.EventArgs())

        '    If OptionButton101.Value Then
        '        CalcEffectiveHeights ILSObstacleList, GPAngle, arMAS_Climb_Nom.Value, ILS.GP_RDH, True
        '        WorkObstacleList = ILSObstacleList
        '    ElseIf OptionButton102.Value Then
        '        CalcEffectiveHeights OASObstacleList, GPAngle, fMissAprPDG, ILS.GP_RDH
        '        WorkObstacleList = OASObstacleList
        '    End If
        '    Sort WorkObstacleList, 2
        '    PrecReportFrm.FillPage11 WorkObstacleList, OptionButton101
    End Sub

    Private Sub ComboBox0006_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)  'Handles ComboBox0006.SelectedIndexChanged
        'If Not bFormInitialised Then Return

        'Dim I As Integer
        'Dim J As Integer
        'Dim K As Integer
        'Dim N As Integer
        'Dim RWYDATA() As RWYType
        'Dim lILS As ILSType


        'I = ComboBox0006.SelectedIndex
        'If I < 0 Then Return
        'CurrADHP = lADHPList(I)

        'FillADHPFields(CurrADHP)
        'If CurrADHP.pPtGeo Is Nothing Then
        '	MsgBox("Error reading ADHP.", MsgBoxStyle.Critical, "PANDA")
        '	Return
        'End If


        'ComboBox0001.Items.Clear()
        'FillRWYList(RWYDATA, CurrADHP, True)

        'N = UBound(RWYDATA)

        'If N < 0 Then
        '	MsgBox(My.Resources.str300, MsgBoxStyle.Critical, "PANDA")
        '	Return
        'End If

        'ReDim RWYList(N)

        'J = -1

        'For I = 0 To N
        '	K = GetILS(RWYDATA(I), lILS, CurrADHP)
        '	If K = 3 Then
        '		J = J + 1
        '		RWYList(J) = RWYDATA(I)
        '		ComboBox0001.Items.Add(RWYDATA(I).Name)
        '	End If
        'Next I

        'NextBtn.Enabled = J >= 0

        'If J >= 0 Then
        '	ReDim Preserve RWYList(J)
        '	ComboBox0001.SelectedIndex = 0
        'Else
        '	ReDim RWYList(-1)
        '	MsgBox("There is no ILS for the specified ADHP.", MsgBoxStyle.Critical, "PANDA")
        '	Return
        'End If

        'If ComboBox0003.SelectedIndex >= 0 Then
        '	ComboBox0003_SelectedIndexChanged(ComboBox0003, New System.EventArgs())
        'Else
        '	ComboBox0003.SelectedIndex = 0
        'End If
    End Sub

    Private Sub TextBox0005_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0005.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0005_Validating(TextBox0005, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0005.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0005_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0005.Validating
        Dim lSs As Double
        Dim fTmp As Double

        If Not IsNumeric(TextBox0005.Text) Then Return

        lSs = CDbl(TextBox0005.Text)
        fTmp = lSs
        'K = ComboBox0003.ListIndex

        If lSs < arSemiSpan.Values(Category) Then lSs = arSemiSpan.Values(Category)
        If lSs > 60.0 Then lSs = 60.0
        If lSs <> fTmp Then TextBox0005.Text = CStr(lSs)
    End Sub

    Private Sub TextBox0006_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0006.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0006_Validating(TextBox0006, Nothing)
        Else
            TextBoxInteger(eventArgs.KeyChar)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0006_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0006.Validating
        Dim lVs As Double
        Dim fTmp As Double
        If Not IsNumeric(TextBox0006.Text) Then Return
        lVs = CDbl(TextBox0006.Text)
        fTmp = lVs
        '    K = ComboBox0003.ListIndex

        If lVs < arVerticalSize.Values(Category) Then lVs = arVerticalSize.Values(Category)
        If lVs > 10.0 Then lVs = 10.0
        If lVs <> fTmp Then TextBox0006.Text = CStr(lVs)
    End Sub

    Private Sub TextBox0012_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0012.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox0012_Validating(TextBox0012, Nothing)
        Else
            TextBoxFloat(e.KeyChar, TextBox0012.Text)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox0012_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0012.Validating
        If Not IsNumeric(TextBox0012.Text) Then Return

        Dim lISH As Double
        Dim fTmp As Double

        fTmp = DeConvertHeight(CDbl(TextBox0012.Text))

        lISH = fTmp

        If lISH < InnerSurfaceHeightMin Then lISH = InnerSurfaceHeightMin
        If lISH > InnerSurfaceHeightMax Then lISH = InnerSurfaceHeightMax

        If lISH <> fTmp Then
            TextBox0012.Text = CStr(ConvertHeight(lISH, eRoundMode.NEAREST))
        End If

        If InnerSurfaceHeight <> lISH Then
            InnerSurfaceHeight = lISH
            'ComboBox0001_SelectedIndexChanged(ComboBox0001, Nothing)
            If ComboBox0002.SelectedIndex > 0 Then ComboBox0002_SelectedIndexChanged(ComboBox0002, Nothing)
        End If
    End Sub

#End Region

#Region "Page 1 events"

    Private Sub CheckBox0101_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0101.CheckedChanged
        If Not bFormInitialised Then Return
        If Not CheckBox0101.Enabled Then Return

        ComboBox0101.Enabled = CheckBox0101.Checked

        On Error Resume Next
        If Not Plane15Elem Is Nothing Then pGraphics.DeleteElement(Plane15Elem)
        If Not FAP15FIXElem Is Nothing Then pGraphics.DeleteElement(FAP15FIXElem)
        Plane15Elem = Nothing
        FAP15FIXElem = Nothing
        On Error GoTo 0

        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        If CheckBox0101.Checked Then
            bNavFlg = False
            ComboBox0101_SelectedIndexChanged(ComboBox0101, New System.EventArgs())
        Else
            TextBox0101.ReadOnly = False
            TextBox0102.ReadOnly = False
            TextBox0101.BackColor = System.Drawing.SystemColors.Window
            TextBox0102.BackColor = System.Drawing.SystemColors.Window

            _CurrFAPOCH = m_fMOC
            bNavFlg = True
            Caller(_CurrFAPOCH, _hFAP)
        End If
    End Sub

    Private Sub CheckBox0102_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0102.CheckedChanged
        ComboBox0105.Enabled = CheckBox0102.Checked
        TextBox0101.ReadOnly = CheckBox0102.Checked
        TextBox0102.ReadOnly = CheckBox0102.Checked

        If CheckBox0102.Checked Then
            TextBox0101.BackColor = SystemColors.ButtonFace
            TextBox0102.BackColor = SystemColors.ButtonFace
            ComboBox0105_SelectedIndexChanged(ComboBox0105, New System.EventArgs())
        Else
            TextBox0101.BackColor = SystemColors.Window
            TextBox0102.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub ComboBox0101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0101.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim K As Integer
        Dim D As Double

        If Not ComboBox0101.Enabled Then Return

        K = ComboBox0101.SelectedIndex
        If K < 0 Then Return

        Plane15 = CreatePlane15(FAPNavDat(K), D)

        If FAPNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
            TextBox0101.ReadOnly = True
            TextBox0102.ReadOnly = True
            TextBox0101.BackColor = System.Drawing.SystemColors.Control
            TextBox0102.BackColor = System.Drawing.SystemColors.Control
        Else
            TextBox0101.ReadOnly = False
            TextBox0102.ReadOnly = False
            TextBox0101.BackColor = System.Drawing.SystemColors.Window
            TextBox0102.BackColor = System.Drawing.SystemColors.Window
        End If

        If Not bNavFlg Then
            If FAPNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
                TextBox0101.Text = CStr(ConvertDistance(D, eRoundMode.NEAREST))
                TextBox0102.Tag = CStr(Plane15.Plane.pPt.Z)
                ComboBox0103_SelectedIndexChanged(ComboBox0103, New System.EventArgs())
            End If

            bNavFlg = True
            TextBox0101.Tag = ""
            TextBox0101_Validating(TextBox0101, New System.ComponentModel.CancelEventArgs(True))
        End If

        On Error Resume Next
        If Not Plane15Elem Is Nothing Then pGraphics.DeleteElement(Plane15Elem)
        If Not FAP15FIXElem Is Nothing Then pGraphics.DeleteElement(FAP15FIXElem)
        On Error GoTo 0

        Plane15Elem = DrawPolygon(Plane15.Poly, 255 * 256.0)
        FAP15FIXElem = DrawPolygon(FAP15FIX, 128)

        Plane15Elem.Locked = True
        FAP15FIXElem.Locked = True
    End Sub

    Private Sub ComboBox0102_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0102.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Select Case ComboBox0102.SelectedIndex
            Case 0
                arMinISlen = arMinISlen00_15p.Values(ComboBox0003.SelectedIndex)
            Case 1
                arMinISlen = arMinISlen16_30p.Values(ComboBox0003.SelectedIndex)
            Case 2
                arMinISlen = arMinISlen31_60p.Values(ComboBox0003.SelectedIndex)
            Case 3
                arMinISlen = arMinISlen61_90p.Values(ComboBox0003.SelectedIndex)
        End Select
        CheckBox0101.Checked = False
        TextBox0106.Text = CStr(ConvertDistance(arMinISlen, eRoundMode.NEAREST))

        _hFAP = -100.0
        TextBox0102.Tag = ""
        TextBox0102_Validating(TextBox0102, New System.ComponentModel.CancelEventArgs())
    End Sub

    Private Sub ComboBox0103_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0103.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim fTmp As Double

        If Not IsNumeric(TextBox0102.Tag) Then Return
        If ComboBox0103.SelectedIndex < 0 Then ComboBox0103.SelectedIndex = 0

        fTmp = CDbl(TextBox0102.Tag)

        If ComboBox0103.SelectedIndex = 0 Then
            TextBox0102.Text = CStr(ConvertHeight(fTmp + FicTHRprj.Z, eRoundMode.NEAREST))
        Else
            TextBox0102.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ComboBox0104_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0104.SelectedIndexChanged
        If ComboBox0104.SelectedIndex = 0 Then
            TextBox0103.Text = CStr(ConvertHeight(_CurrFAPOCH, eRoundMode.NEAREST))
        Else
            TextBox0103.Text = CStr(ConvertHeight(_CurrFAPOCH + FicTHRprj.Z, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ComboBox0105_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0105.SelectedIndexChanged
        If Not bFormInitialised Then Return

        If Not CheckBox0102.Checked Then Return
        If ComboBox0105.SelectedIndex < 0 Then Return

        Dim fTmpDist As Double
        Dim WPT As NavaidData

        WPT = ComboBox0105.SelectedItem

        fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), arHOASPlaneCat1, 0.0)
        fFAPDist = Point2LineDistancePrj(WPT.pPtPrj, FicTHRprj, _ArDir - 90.0)                       ' * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, FAFNavDat(i).pPtGeo)

        If fFAPDist < fTmpDist Then fFAPDist = fTmpDist

        _hFAP = FAPDist2hFAP(fFAPDist)

        'fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), m_fhFAP, fFAPDist)
        'FAPto150Distance = arMinISlen + fTmpDist

        _CurrFAPOCH = IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
        bNavFlg = True

        Caller(_CurrFAPOCH, _hFAP)

        TextBox0102.Tag = CStr(_hFAP)
        ComboBox0103_SelectedIndexChanged(ComboBox0103, New System.EventArgs())

        TextBox0101.Tag = CStr(ConvertDistance(fFAPDist, 1))
        TextBox0101.Text = TextBox0101.Tag
    End Sub

    Private ListView0101_InUse As Boolean = False

    Private Sub ListView0101_ItemChecked(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView0101.ItemChecked
        If Not bFormInitialised Then Return

        If ListView0101_InUse Then Return
        ListView0101_InUse = True

        Dim I As Integer
        Dim N As Integer
        Dim Item As System.Windows.Forms.ListViewItem = eventArgs.Item
        Dim itmX As System.Windows.Forms.ListViewItem

        Try
            N = ListView0101.Items.Count

            If Item.Checked Then
                fNearDist = DeConvertDistance(CDbl(Item.Text))
                If Item.SubItems.Count < 2 Then Return
                fFarDist = DeConvertDistance(CDbl(Item.SubItems(1).Text))

                For I = 0 To N - 1
                    If I <> Item.Index Then
                        itmX = ListView0101.Items.Item(I)
                        itmX.Checked = False
                    End If
                Next I
            Else
                For I = 0 To N - 1
                    If I <> Item.Index Then
                        itmX = ListView0101.Items.Item(I)
                        If itmX.Checked Then Return
                    End If
                Next I
                ListView0101_InUse = False
                Item.Checked = True
            End If
        Finally
            ListView0101_InUse = False
        End Try
    End Sub

    Private Sub ListView0101_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView0101.SelectedIndexChanged
        If ListView0101.SelectedItems.Count = 0 Then Return

        Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems(0)
        Dim fNear As Double
        Dim fFar As Double

        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pLineTmp As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon

        fNear = DeConvertDistance(CDbl(Item.Text))
        fFar = DeConvertDistance(CDbl(Item.SubItems(1).Text))

        pClone = IntermediateFullArea
        pIFPoly = pClone.Clone

        pLineTmp = New ESRI.ArcGIS.Geometry.Polyline

        ptTmp = PointAlongPlane(ptFAP, _ArDir + 180.0, fNear)
        pLineTmp.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 2.0 * RModel)
        pLineTmp.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 2.0 * RModel)

        ClipByLine(pIFPoly, pLineTmp, Nothing, pTmpPoly, Nothing)

        If Not pTmpPoly.IsEmpty() Then pIFPoly = pTmpPoly

        ptTmp = PointAlongPlane(ptFAP, _ArDir + 180.0, fFar)

        pLineTmp.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 2.0 * RModel)
        pLineTmp.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 2.0 * RModel)

        ClipByLine(pIFPoly, pLineTmp, pTmpPoly, Nothing, Nothing)
        If Not pTmpPoly.IsEmpty() Then pIFPoly = pTmpPoly
    End Sub

    Private Sub TextBox0101_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0101.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0101_Validating(TextBox0101, Nothing)
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0101.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0101_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0101.Validating
        If TextBox0101.Tag = TextBox0101.Text Then Return

        Dim fTmpDist As Double
        fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), arHOASPlaneCat1, 0.0)

        If IsNumeric(TextBox0101.Text) Then
            fFAPDist = DeConvertDistance(CDbl(TextBox0101.Text))
        Else
            fFAPDist = 0.0
        End If

        If fFAPDist < fTmpDist Then fFAPDist = fTmpDist
        'TextBox0101.Text = ConvertDistance(fFAPDist, eRoundMode.rmNERAEST).ToString()
        'End If

        _hFAP = FAPDist2hFAP(fFAPDist)

        'fTmpDist = StartPointDist(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), m_fhFAP, fFAPDist)
        'FAPto150Distance = arMinISlen + fTmpDist

        _CurrFAPOCH = IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
        bNavFlg = True

        Caller(_CurrFAPOCH, _hFAP)

        TextBox0102.Tag = CStr(_hFAP)
        ComboBox0103_SelectedIndexChanged(ComboBox0103, New System.EventArgs())

        TextBox0101.Tag = CStr(ConvertDistance(fFAPDist, 1))
        TextBox0101.Text = TextBox0101.Tag
    End Sub

    Private Sub TextBox0102_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0102.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0102_Validating(TextBox0102, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0102.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0102_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0102.Validating
        Dim fTmp As Double

        If IsNumeric(TextBox0102.Text) Then
            fTmp = DeConvertHeight(CDbl(TextBox0102.Text))
            If ComboBox0103.SelectedIndex = 0 Then fTmp = fTmp - FicTHRprj.Z
        Else
            fTmp = arHOASPlaneCat1 'arMATurnAlt.Value
        End If

        If fTmp < arHOASPlaneCat1 Then fTmp = arHOASPlaneCat1 'arISegmentMOC.Value'arISegmentMOC.Value 

        If _hFAP = fTmp Then Return

        _hFAP = fTmp
        fFAPDist = hFAP2FAPDist(_hFAP)

        _CurrFAPOCH = IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
        bNavFlg = True
        Caller(_CurrFAPOCH, _hFAP)

        TextBox0102.Tag = CStr(_hFAP)
        ComboBox0103_SelectedIndexChanged(ComboBox0103, New System.EventArgs())

        TextBox0101.Tag = CStr(ConvertDistance(fFAPDist, 1))
        TextBox0101.Text = TextBox0101.Tag
    End Sub


#End Region

#Region "Page 2 events"

    Private Sub CheckBox0201_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0201.CheckedChanged
        ComboBox0203.Enabled = CheckBox0201.Checked
        TextBox0201.ReadOnly = CheckBox0201.Checked

        If CheckBox0201.Checked Then
            TextBox0201.BackColor = SystemColors.ButtonFace
            ComboBox0203_SelectedIndexChanged(ComboBox0203, New System.EventArgs())
        Else
            TextBox0201.BackColor = SystemColors.Window
        End If
    End Sub

    Private Sub OptionButton0201_CheckedChanged(sender As Object, e As EventArgs) Handles OptionButton0201.CheckedChanged
        If (Not bFormInitialised) Or (Not OptionButton0201.Enabled) Then
            Return
        End If

        TextBox0201_Validating(TextBox0201, Nothing)
    End Sub

    Private Sub ComboBox0201_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0201.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim N As Integer
        Dim K As Integer
        Dim Kmin As Integer
        Dim Kmax As Integer
        Dim tipStr As String

        K = ComboBox0201.SelectedIndex
        If K < 0 Then Return

        label0201_11.Text = GetNavTypeName(IFNavDat(K).TypeCode)
        TextBox0201.Visible = True

        If IFNavDat(K).IntersectionType = eIntersectionType.ByDistance Then
            N = UBound(IFNavDat(K).ValMin)
            _label0201_12.Text = DistanceConverter(DistanceUnit).Unit
            OptionButton0201.Enabled = N > 0
            OptionButton0202.Enabled = N > 0

            If CheckBox0201.Checked Then

            Else
                If OptionButton0202.Checked Or (N = 0) Then
                    TextBox0201.Text = CStr(ConvertDistance(IFNavDat(K).ValMin(0) - IFNavDat(K).Disp, eRoundMode.NEAREST))
                Else
                    TextBox0201.Text = CStr(ConvertDistance(IFNavDat(K).ValMin(1) - IFNavDat(K).Disp, eRoundMode.NEAREST))
                End If
            End If

            If N = 0 Then
                If IFNavDat(K).ValCnt > 0 Then
                    OptionButton0201.Checked = True
                Else
                    OptionButton0202.Checked = True
                End If
            End If

            label0201_12.Text = My.Resources.str00229
            tipStr = My.Resources.str00220 + vbCrLf '"Рекомендуемый интервал расстояний:"

            For I = N To 0 Step -1
                tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(IFNavDat(K).ValMin(I) - IFNavDat(K).Disp, eRoundMode.NEAREST)) + " " +
                    DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(IFNavDat(K).ValMax(I) - IFNavDat(K).Disp, eRoundMode.NEAREST)) + " " +
                    DistanceConverter(DistanceUnit).Unit
                If I > 0 Then
                    tipStr = tipStr + vbCrLf
                End If
            Next I
        Else
            OptionButton0201.Enabled = False
            OptionButton0202.Enabled = False

            If IFNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
                CheckBox0201.Checked = False
                label0201_12.Text = My.Resources.str00106    '"Над средством"
                tipStr = ""
                _label0201_12.Text = ""
                TextBox0201.Visible = False
            Else
                If IFNavDat(K).TypeCode = eNavaidType.NDB Then
                    label0201_12.Text = My.Resources.str00228    '"Пеленг:"
                    Kmax = Modulus(IFNavDat(K).ValMax(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
                    Kmin = Modulus(IFNavDat(K).ValMin(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
                    tipStr = My.Resources.str00220 + vbCrLf + My.Resources.str00221  '"Рекомендуемый интервал пеленгов:"
                Else
                    label0201_12.Text = My.Resources.str00227    '"Радиал:"
                    Kmax = Modulus(IFNavDat(K).ValMax(0) - IFNavDat(K).MagVar, 360.0)
                    Kmin = Modulus(IFNavDat(K).ValMin(0) - IFNavDat(K).MagVar, 360.0)
                    tipStr = My.Resources.str00220 + vbCrLf + My.Resources.str00221  '"Рекомендуемый интервал радиалов:"
                End If

                _label0201_12.Text = "°"
                tipStr = tipStr + CStr(Kmin) + " °" + My.Resources.str00222 + CStr(Kmax) + " °"

                If Not CheckBox0201.Checked Then
                    If IFNavDat(K).ValCnt > 0 Then
                        TextBox0201.Text = CStr(Kmin)
                    Else
                        TextBox0201.Text = CStr(Kmax)
                    End If
                End If
            End If
        End If

        TextBox0201.Visible = IFNavDat(K).IntersectionType <> eIntersectionType.OnNavaid

        label0201_14.Text = tipStr
        tipStr = Replace(tipStr, vbCrLf, "   ")
        ToolTip1.SetToolTip(TextBox0201, tipStr)

        TextBox0201.Tag = "A"
        TextBox0201_Validating(TextBox0201, New System.ComponentModel.CancelEventArgs(True))
    End Sub

    Private Sub ComboBox0202_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0202.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim fTmp As Double

        If Not IsNumeric(TextBox0202.Tag) Then Return
        If ComboBox0202.SelectedIndex < 0 Then ComboBox0202.SelectedIndex = 0

        fTmp = CType(TextBox0202.Tag, Double)

        If ComboBox0202.SelectedIndex = 0 Then
            TextBox0202.Text = CStr(ConvertHeight(fTmp + FicTHRprj.Z, eRoundMode.NEAREST))
        Else
            TextBox0202.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ComboBox0203_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0203.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim WPT As NavaidData
        Dim fDist As Double
        Dim fDir As Double
        Dim fAzt As Double
        Dim K As Integer
        Dim N As Integer

        If Not CheckBox0201.Checked Then Return
        If ComboBox0203.SelectedIndex < 0 Then Return

        WPT = ComboBox0203.SelectedItem

        K = ComboBox0201.SelectedIndex
        If K < 0 Then Return

        N = UBound(IFNavDat)
        If N < 0 Then Return

        'Label0401_25.Text = GetNavTypeName(WPT.TypeCode)
        'PtFAF

        If (IFNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (IFNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
            fDist = ReturnDistanceInMeters(IFNavDat(K).pPtPrj, WPT.pPtPrj)
            TextBox0201.Text = CStr(ConvertDistance(fDist - IFNavDat(K).Disp, eRoundMode.NONE))
            If SideDef(IFNavDat(K).pPtPrj, _ArDir + 90.0, WPT.pPtPrj) < 0 Then
                OptionButton0201.Checked = True
            Else
                OptionButton0202.Checked = True
            End If
        Else
            fDir = ReturnAngleInDegrees(IFNavDat(K).pPtPrj, WPT.pPtPrj)
            fAzt = Dir2Azt(IFNavDat(K).pPtPrj, fDir)
            If (IFNavDat(K).TypeCode = eNavaidType.NDB) Then
                TextBox0201.Text = CStr(Modulus(fAzt + 180.0 - IFNavDat(K).MagVar, 360.0))
            Else
                TextBox0201.Text = CStr(Modulus(fAzt - IFNavDat(K).MagVar, 360.0))
            End If
        End If

        TextBox0201.Tag = "-"
        TextBox0201_Validating(TextBox0201, New CancelEventArgs())
    End Sub

    Private Sub TextBox0201_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0201.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0201_Validating(TextBox0201, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, (TextBox0201.Text))
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0201_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0201.Validating
        Dim I As Integer
		'Dim J As Integer
		Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer

        Dim D As Double
        Dim d0 As Double
        Dim Kmax As Double
        Dim Kmin As Double
        Dim fTmp As Double
        Dim fDis As Double
        Dim hDis As Double
        Dim fDirl As Double
        Dim fDist As Double
        Dim hIFFix As Double
        Dim InterToler As Double
        Dim TrackToler As Double

        Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt3 As ESRI.ArcGIS.Geometry.IPoint

        Dim Clone As ESRI.ArcGIS.esriSystem.IClone
        Dim pGeo As ESRI.ArcGIS.Geometry.IGeometry
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPolygon
        Dim pLPolyLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pRPolyLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
        Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpLine As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

        K = ComboBox0201.SelectedIndex
        If Not IsNumeric(TextBox0201.Text) Then Return
        If TextBox0201.Tag = TextBox0201.Text Then Return

        If (IFNavDat(K).IntersectionType <> eIntersectionType.OnNavaid) And Not CheckBox0201.Checked Then
            fDirl = CDbl(TextBox0201.Text)

            Select Case IFNavDat(K).IntersectionType
                Case eIntersectionType.ByAngle
                    fTmp = fDirl
                    If IFNavDat(K).TypeCode = eNavaidType.NDB Then
                        Kmax = Modulus(IFNavDat(K).ValMax(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
                        Kmin = Modulus(IFNavDat(K).ValMin(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
                    Else
                        Kmax = Modulus(IFNavDat(K).ValMax(0) - IFNavDat(K).MagVar, 360.0)
                        Kmin = Modulus(IFNavDat(K).ValMin(0) - IFNavDat(K).MagVar, 360.0)
                    End If

                    If IFNavDat(K).ValCnt > 0 Then
                        If Not AngleInSector(fDirl, Kmin, Kmax) Then fDirl = Kmin
                    Else
                        If Not AngleInSector(fDirl, Kmin, Kmax) Then fDirl = Kmax
                    End If

                    If fDirl <> fTmp Then
                        TextBox0201.Text = CStr(System.Math.Round(fDirl))
                    End If
                Case eIntersectionType.ByDistance
                    fDirl = DeConvertDistance(fDirl) + IFNavDat(K).Disp
                    fTmp = fDirl

                    N = UBound(IFNavDat(K).ValMin)

                    If OptionButton0202.Checked Or (N = 0) Then
                        If fDirl < IFNavDat(K).ValMin(0) Then fDirl = IFNavDat(K).ValMin(0)
                        If fDirl > IFNavDat(K).ValMax(0) Then fDirl = IFNavDat(K).ValMax(0)
                    Else
                        If fDirl < IFNavDat(K).ValMin(1) Then fDirl = IFNavDat(K).ValMin(1)
                        If fDirl > IFNavDat(K).ValMax(1) Then fDirl = IFNavDat(K).ValMax(1)
                    End If

                    If fDirl <> fTmp Then
                        TextBox0201.Text = CStr(ConvertDistance(fDirl - IFNavDat(K).Disp, eRoundMode.NEAREST))
                    End If
            End Select
        End If

        SafeDeleteElement(IFFIXElem)
        SafeDeleteElement(IntermediateFullAreaElem)
        SafeDeleteElement(IntermediatePrimAreaElem)

        'N = UBound(OASPlanesElement)
        'For I = 0 To N
        '	SafeDeleteElement(OASPlanesElement(I))
        'Next I

        TrackToler = LLZ.TrackingTolerance
        pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
        pPolyClone.AddPoint(ILS.pPtPrj)
        pPolyClone.AddPoint(PointAlongPlane(ILS.pPtPrj, _ArDir - TrackToler + 180.0, 3.0 * RModel))
        pPolyClone.AddPoint(PointAlongPlane(ILS.pPtPrj, _ArDir + TrackToler + 180.0, 3.0 * RModel))

        pTopo = pPolyClone
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        '    DrawPolygon pPolyClone, 0
        '    Set Clone = IntermediateFullArea 'pFullPoly

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        Select Case IFNavDat(K).IntersectionType
            Case eIntersectionType.OnNavaid
                '            D = Point2LineDistancePrj(ptPlaneFAP, IFNavDat(K).pPtPrj, ArDir + 90.0)
                D = Point2LineDistancePrj(ptFAP, IFNavDat(K).pPtPrj, _ArDir + 90.0)
                Clone = IFNavDat(K).pPtPrj
                IFprj = Clone.Clone

                hIFFix = D * arImDescent_PDG + ptFAP.Z + FicTHRprj.Z - IFNavDat(K).pPtPrj.Z
                If IFNavDat(K).TypeCode = eNavaidType.NDB Then
                    NDBFIXTolerArea(IFNavDat(K).pPtPrj, _ArDir, hIFFix, pTmpPoly)
                Else
                    VORFIXTolerArea(IFNavDat(K).pPtPrj, _ArDir, hIFFix, pTmpPoly)
                End If
                pTopo = pTmpPoly
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
            Case eIntersectionType.ByAngle
                If IFNavDat(K).TypeCode = eNavaidType.NDB Then
                    InterToler = NDB.IntersectingTolerance
                    fDis = NDB.Range
                    fDirl = fDirl + 180.0
                Else
                    InterToler = VOR.IntersectingTolerance
                    fDis = VOR.Range
                End If

                If CheckBox0201.Checked And (ComboBox0203.SelectedIndex >= 0) Then
                    pt3 = CType(ComboBox0203.SelectedItem, NavaidData).pPtPrj
                    Clone = pt3
                    IFprj = Clone.Clone

                    fDirl = ReturnAngleInDegrees(IFNavDat(K).pPtPrj, pt3)

                    fTmp = Dir2Azt(IFNavDat(K).pPtPrj, fDirl) - IFNavDat(K).MagVar
                    If IFNavDat(K).TypeCode = eNavaidType.NDB Then fTmp += 180.0
                    fTmp = Modulus(fTmp)

                    TextBox0201.Text = System.Math.Round(fTmp, 4).ToString()
                    TextBox0201.Tag = TextBox0201.Text
                Else
                    fDirl = Azt2Dir(IFNavDat(K).pPtGeo, fDirl + IFNavDat(K).MagVar)
                    IFprj = New ESRI.ArcGIS.Geometry.Point
                    pConstruct = IFprj  '	pConstruct.ConstructAngleIntersection IFNavDat(K).pPtPrj, DegToRad(fDirl), ptPlaneFAP, DegToRad(ArDir)
                    pConstruct.ConstructAngleIntersection(IFNavDat(K).pPtPrj, DegToRad(fDirl), ptFAP, DegToRad(_ArDir))
                End If

                pt1 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl + InterToler, fDis)
                pt2 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl - InterToler, fDis)

                'DrawPoint IFPnt, 0
                'DrawPoint ptFAP, 255
                pSect0 = New ESRI.ArcGIS.Geometry.Polygon
                pSect0.AddPoint(IFNavDat(K).pPtPrj)
                pSect0.AddPoint(pt1)
                pSect0.AddPoint(pt2)
                pSect0.AddPoint(IFNavDat(K).pPtPrj)

                pTopo = pSect0
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                'DrawPolygon pPolyClone, 0
                'DrawPolygon pSect0, 255

                pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
                'DrawPolygon pTmpPoly, RGB(128, 128, 0)
            Case eIntersectionType.ByDistance
                If CheckBox0201.Checked And (ComboBox0203.SelectedIndex >= 0) Then
                    pt3 = CType(ComboBox0203.SelectedItem, NavaidData).pPtPrj
                    Clone = pt3
                    IFprj = Clone.Clone

                    fDirl = ReturnDistanceInMeters(IFNavDat(K).pPtPrj, pt3)
                    TextBox0201.Text = System.Math.Round(ConvertDistance(fDirl - IFNavDat(K).Disp, eRoundMode.NONE), 4).ToString()
                    TextBox0201.Tag = TextBox0201.Text
                Else
                    If (IFNavDat(K).ValCnt < 0) Or (OptionButton0202.Enabled And OptionButton0202.Checked) Then
                        CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptFAP, _ArDir, IFprj)
                    Else
                        CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptFAP, _ArDir + 180.0, IFprj)
                    End If
                End If

                'fDis = Point2LineDistancePrj(IFPnt, ptPlaneFAP, ArDir + 90.0)
                fDis = Point2LineDistancePrj(IFprj, ptFAP, _ArDir + 90.0)
                hIFFix = fDis * arImDescent_Max.Value + ptFAP.Z + FicTHRprj.Z - IFNavDat(K).pPtPrj.Z

                d0 = System.Math.Sqrt(fDirl * fDirl + hIFFix * hIFFix) * DME.ErrorScalingUp + DME.MinimalError

                D = fDirl + d0
                pSect0 = CreatePrjCircle(IFNavDat(K).pPtPrj, D)

                pCutter.FromPoint = PointAlongPlane(IFNavDat(K).pPtPrj, _ArDir - 90.0, D + D)
                pCutter.ToPoint = PointAlongPlane(IFNavDat(K).pPtPrj, _ArDir + 90.0, D + D)

                D = fDirl - d0
                pSect1 = CreatePrjCircle(IFNavDat(K).pPtPrj, D)

                pTopo = pSect0
                pTmpPoly = pTopo.Difference(pSect1)

                pTopo = pTmpPoly
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                If SideDef(pCutter.FromPoint, _ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

                If (IFNavDat(K).ValCnt < 0) Or (OptionButton0202.Enabled And OptionButton0202.Checked) Then
                    pTopo.Cut(pCutter, pSect1, pSect0)
                Else
                    pTopo.Cut(pCutter, pSect0, pSect1)
                End If
                pTopo = pSect0
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
                '            DrawPolygon pTmpPoly, 0
        End Select

        pTopo = pTmpPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        pIFTolerArea = pTmpPoly
        '===================================================================
        pTopo = IntermediateFullArea

        pRelation = IntermediateFullArea
        If pRelation.Contains(ptFAP) Then
            pCutter.FromPoint = PointAlongPlane(ptFAP, _ArDir + 90.0, 50000.0)
            pCutter.ToPoint = PointAlongPlane(ptFAP, _ArDir - 90.0, 50000.0)
            pFAPLine = pTopo.Intersect(pCutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
        Else
            pLPolyLine = IntersectPlanes(OASPlanes(eOAS.XlPlane).Plane, OASPlanes(eOAS.YlPlane).Plane, _hFAP - arISegmentMOC.Value, _hFAP)
            pRPolyLine = IntersectPlanes(OASPlanes(eOAS.YrPlane).Plane, OASPlanes(eOAS.XrPlane).Plane, _hFAP - arISegmentMOC.Value, _hFAP)

            pTransform = pLPolyLine
            pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
            pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

            pTransform = pRPolyLine
            pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
            pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

            pFAPLine = New ESRI.ArcGIS.Geometry.Polyline
            pFAPLine.FromPoint = pLPolyLine.FromPoint
            pFAPLine.ToPoint = pRPolyLine.FromPoint
        End If

        If SideDef(pFAPLine.FromPoint, _ArDir, pFAPLine.ToPoint) < 0 Then pFAPLine.ReverseOrientation()
        ''================================================

        'DrawPolygon IntermediateFullArea
        'DrawPolyLine pCutter, RGB(0, 0, 255), 2
        'DrawPoint ptFAP, 255
        'DrawPolyLine pFAPLine, RGB(0, 0, 255), 2

        pIFLine = New ESRI.ArcGIS.Geometry.Polyline
        pIFLine.FromPoint = PointAlongPlane(IFprj, _ArDir + 90.0, arIFHalfWidth.Value)
        pIFLine.ToPoint = PointAlongPlane(IFprj, _ArDir - 90.0, arIFHalfWidth.Value)
        '===================================================================
        'DrawPolyLine pIFLine, RGB(0, 0, 255), 2

        pCutter.FromPoint = PointAlongPlane(pFAPLine.FromPoint, _ArDir, 5000.0)
        pCutter.ToPoint = PointAlongPlane(pFAPLine.FromPoint, _ArDir + 180.0, 100000.0)
        pTopo.Cut(pCutter, pSect0, pSect1)

        pTopo = pSect0
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pCutter.FromPoint = PointAlongPlane(pFAPLine.ToPoint, _ArDir, 5000.0)
        pCutter.ToPoint = PointAlongPlane(pFAPLine.ToPoint, _ArDir + 180.0, 100000.0)
        pTopo.Cut(pCutter, pSect1, pIFFIXPoly)

        pTopo = pIFFIXPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pCutter.FromPoint = PointAlongPlane(IFprj, _ArDir + 90.0, 50000.0)
        pCutter.ToPoint = PointAlongPlane(IFprj, _ArDir - 90.0, 50000.0)
        'DrawPolyLine pCutter, 255, 2
        'DrawPolygon pIFFIXPoly, 255
        pTopo.Cut(pCutter, pSect0, pSect1)

        pSect1 = New ESRI.ArcGIS.Geometry.Polygon
        pSect1.AddPoint(pFAPLine.FromPoint)
        pSect1.AddPoint(pIFLine.FromPoint)
        pSect1.AddPoint(pIFLine.ToPoint)
        pSect1.AddPoint(pFAPLine.ToPoint)

        pTopo = pSect0
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTopo = pSect1
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pIFFIXPoly = pTopo.Union(pSect0)
        pTopo = pIFFIXPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        'DrawPolygon pIFFIXPoly, 0

        pIFFIXPoly = pTopo.Union(pTmpPoly)
        pTopo = pIFFIXPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        '===================================================================
        pTmpLine = New ESRI.ArcGIS.Geometry.Polyline
        pTmpLine.AddPoint(PointAlongPlane(pFAPLine.FromPoint, ReturnAngleInDegrees(pIFLine.FromPoint, pFAPLine.FromPoint) + 90.0, 500.0))
        pTmpLine.AddPoint(pFAPLine.FromPoint)
        pTmpLine.AddPoint(PointAlongPlane(pIFLine.FromPoint, _ArDir - 90.0, 0.25 * pIFLine.Length))
        pTmpLine.AddPoint(PointAlongPlane(pTmpLine.Point(2), _ArDir + 180.0, 500.0))

        pTopo.Cut(pTmpLine, pSect0, pSect1)
        pTopo = pSect0
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        '===================================================================
        pTmpLine.RemovePoints(0, 4)
        pTmpLine.AddPoint(PointAlongPlane(pFAPLine.ToPoint, ReturnAngleInDegrees(pIFLine.ToPoint, pFAPLine.ToPoint) - 90.0, 500.0))
        pTmpLine.AddPoint(pFAPLine.ToPoint)
        pTmpLine.AddPoint(PointAlongPlane(pIFLine.ToPoint, _ArDir + 90.0, 0.25 * pIFLine.Length))
        pTmpLine.AddPoint(PointAlongPlane(pTmpLine.Point(2), _ArDir + 180.0, 500.0))

        pTopo.Cut(pTmpLine, pSect1, IntermediatePrimArea)

        pTopo = IntermediatePrimArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        '===================================================================

        'On Error Resume Next

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            pCutter = New ESRI.ArcGIS.Geometry.Polyline
            pt1 = PointAlongPlane(FicTHRprj, _ArDir + 180.0, FAPEarlierToler)
            pCutter.FromPoint = PointAlongPlane(pt1, _ArDir + 90.0, 100000.0)
            pCutter.ToPoint = PointAlongPlane(pt1, _ArDir - 90.0, 100000.0)
            'DrawPolyLine pCutter, 0, 2

            pTopo = wOASPlanes(eOAS.CommonPlane).Poly
            pTmpPoly1 = Nothing
            Try
                pTopo.Cut(pCutter, pTmpPoly1, pSect1)
            Catch ex As Exception

            End Try

            If pTmpPoly1 Is Nothing Then pTmpPoly1 = wOASPlanes(eOAS.CommonPlane).Poly
        Else
            pTmpPoly1 = wOASPlanes(eOAS.CommonPlane).Poly
        End If

        pTopo = pTmpPoly1
        pTmpPoly1 = pTopo.Difference(wOASPlanes(eOAS.YrPlane).Poly)

        pTopo = pTmpPoly1
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTmpPoly1 = pTopo.Difference(wOASPlanes(eOAS.YlPlane).Poly)
        pTmpPoly1 = RemoveAgnails(pTmpPoly1)

        ClipByLine(wOASPlanes(eOAS.CommonPlane).Poly, pCutter, pTmpPoly, Nothing, Nothing)
        pTopo = pIFFIXPoly
        pGeo = pTmpPoly

        If pGeo.IsEmpty() Then
            pSect0 = pTopo.Difference(pTmpPoly1)
        Else
            pSect0 = pTopo.Difference(pTmpPoly)
        End If

        pIFFIXPoly = pSect0
        pTopo = pIFFIXPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        IntermediateFullAreaElem = DrawPolygon(pSect0, 255)
        'IntermediateFullAreaElem = DrawPolygon(pIFFIXPoly, 255)

        pTopo = IntermediatePrimArea

        If pGeo.IsEmpty() Then
            pSect0 = pTopo.Difference(pTmpPoly1)
        Else
            pSect0 = pTopo.Difference(pTmpPoly)
        End If
        IntermediatePrimArea = pSect0

        IntermediatePrimAreaElem = DrawPolygon(IntermediatePrimArea, 0)
        '        Set IFFIXElem = DrawPoint(IFPnt, 255)
        IFFIXElem = DrawPointWithText(IFprj, "IF", WPTColor)

        IntermediateFullAreaElem.Locked = True
        IntermediatePrimAreaElem.Locked = True
        IFFIXElem.Locked = True

        N = UBound(wOASPlanes)

        'On Error Resume Next
        For I = 0 To N
            SafeDeleteElement(OASPlanesCat1Element(I))
            'If Not OASPlanesCat1Element(I) Is Nothing Then pGraphics.DeleteElement(OASPlanesCat1Element(I))

            pGeo = wOASPlanes(I).Poly
            If Not pGeo.IsEmpty() Then
                ClipByLine(wOASPlanes(I).Poly, pCutter, pTmpPoly1, Nothing, Nothing)
                If pTmpPoly1.IsEmpty() Then pTmpPoly1 = wOASPlanes(I).Poly

                pTopo = pTmpPoly1
                pTmpPoly = pTopo.Difference(pIFFIXPoly)
                pGeo = pTmpPoly

                If Not pGeo.IsEmpty() Then
                    OASPlanesCat1Element(I) = DrawPolygon(pTmpPoly, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSHollow, OASPlanesCat1State)
                    OASPlanesCat1Element(I).Locked = True
                End If
            End If
        Next I
        CommandBar.isEnabled(CommandBar.wOAS) = True

        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        ''       RefreshCommandBar mTool, 128
        'On Error GoTo 0 '==========================================================================

        '====================================================================================================
        Dim fCutDist As Double

        M = UBound(WorkObstacleList.Obstacles)
        N = UBound(WorkObstacleList.Parts)
        If N > -1 Then
            ReDim PrecisionObstacleList.Obstacles(M)
            ReDim PrecisionObstacleList.Parts(N)

            If CheckBox0101.Enabled And CheckBox0101.Checked Then
                fCutDist = FAPEarlierToler
            Else
                fCutDist = ReturnDistanceInMeters(IFprj, FicTHRprj)
            End If
        Else
            ReDim PrecisionObstacleList.Obstacles(-1)
            ReDim PrecisionObstacleList.Parts(-1)
        End If

        For I = 0 To M
            WorkObstacleList.Obstacles(I).NIx = -1
        Next

        K = -1
        L = -1
        For I = 0 To N
            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).IgnoredByUser Then Continue For
            If (WorkObstacleList.Parts(I).Dist > fCutDist) Then Continue For
            If (WorkObstacleList.Parts(I).ReqH > _hFAP) Then Continue For

            K = K + 1
            PrecisionObstacleList.Parts(K) = WorkObstacleList.Parts(I)

            If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                L += 1
                PrecisionObstacleList.Obstacles(L) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                PrecisionObstacleList.Obstacles(L).PartsNum = 0
                ReDim PrecisionObstacleList.Obstacles(L).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = L
            End If

            PrecisionObstacleList.Parts(K).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
            PrecisionObstacleList.Parts(K).Index = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum) = K
            PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(K).Owner).PartsNum += 1
        Next I

        If K >= 0 Then
            ReDim Preserve PrecisionObstacleList.Parts(K)
            ReDim Preserve PrecisionObstacleList.Obstacles(L)
        Else
            ReDim PrecisionObstacleList.Parts(-1)
            ReDim PrecisionObstacleList.Obstacles(-1)
        End If

        PrecReportFrm.FillPage04(PrecisionObstacleList)
        '====================================================================================================

        TextBox0201.Tag = TextBox0201.Text

        fDist = Point2LineDistancePrj(FicTHRprj, ptFAP, _ArDir + 90.0)   'hFAP2FAPDist(fhFAP) '(fhFAP - ILS.GP_RDH) / TanGPA
        fDis = Point2LineDistancePrj(IFprj, ptFAP, _ArDir + 90.0)

        GetIntermObstacleList(ObstacleList, lIntermObstacleList, pIFFIXPoly, FicTHRprj, _ArDir)
        N = UBound(lIntermObstacleList.Parts)
        _InterMOCH = arISegmentMOC.Value '_hFAP
        K = -1
        For I = 0 To N
            fTmp = lIntermObstacleList.Parts(I).Dist - fDist

            If fTmp > 0.0 Then
                lIntermObstacleList.Parts(I).Rmin = arMinISlen
                lIntermObstacleList.Parts(I).MOC = 2.0 * arISegmentMOC.Value * (arIFHalfWidth.Value - fHalfFAPWidth - fDis * (lIntermObstacleList.Parts(I).DistStar - fHalfFAPWidth) / fTmp) / arIFHalfWidth.Value
            Else
                lIntermObstacleList.Parts(I).Rmin = arMinISlen
                lIntermObstacleList.Parts(I).MOC = arISegmentMOC.Value
            End If

            If lIntermObstacleList.Parts(I).MOC > arISegmentMOC.Value Then lIntermObstacleList.Parts(I).MOC = arISegmentMOC.Value
            lIntermObstacleList.Parts(I).hPenet = lIntermObstacleList.Parts(I).Height + lIntermObstacleList.Parts(I).MOC - _hFAP

            lIntermObstacleList.Parts(I).ReqH = lIntermObstacleList.Parts(I).Height + lIntermObstacleList.Parts(I).MOC
            lIntermObstacleList.Parts(I).Flags = lIntermObstacleList.Parts(I).MOC >= arISegmentMOC.Value

            If _InterMOCH < lIntermObstacleList.Parts(I).ReqH Then
                _InterMOCH = lIntermObstacleList.Parts(I).ReqH
                K = I
            End If
        Next I

        PrecReportFrm.FillPage05(lIntermObstacleList)
        NextBtn.Enabled = _InterMOCH <= _hFAP
        If _InterMOCH > _hFAP Then MessageBox.Show(My.Resources.str00520)

        TextBox0209.Text = ConvertHeight(_InterMOCH + FicTHRprj.Z).ToString()

        If K >= 0 Then
            TextBox0210.Text = lIntermObstacleList.Obstacles(lIntermObstacleList.Parts(K).Owner).UnicalName
        Else
            TextBox0210.Text = Resources.str10520
        End If

        IFprj.M = _ArDir
        D = Point2LineDistancePrj(ptFAP, IFprj, _ArDir + 90.0)
        hDis = arImHorSegLen.Values(ComboBox0003.SelectedIndex)
        hIFFix = (D - hDis) * arImDescent_PDG + ptFAP.Z
        '======================================
        If ArrivalProfile.PointsNo = 4 Then
            ArrivalProfile.RemovePointByIndex(0)
            ArrivalProfile.RemovePointByIndex(0)
        End If

        fDis = Point2LineDistancePrj(ptFAP, FicTHRprj, _ArDir + 90.0)
        ArrivalProfile.InsertPoint(D + fDis, hIFFix, ILS.Course - ILS.MagVar, -RadToDeg(System.Math.Atan(arImDescent_PDG)), -1, 0)
        If arImDescent_PDG <> 0 Then
            ArrivalProfile.InsertPoint(D + fDis - (hIFFix - ptFAP.Z) / arImDescent_PDG, ptFAP.Z, ILS.Course - ILS.MagVar, 0, -1, 1)
        Else
            ArrivalProfile.InsertPoint(D + fDis, ptFAP.Z, ILS.Course - ILS.MagVar, 0, -1, 1)
        End If
        '======================================
        TextBox0202.Tag = hIFFix

        ComboBox0202_SelectedIndexChanged(ComboBox0202, New System.EventArgs())
        TextBox0204.Text = CStr(ConvertDistance(hDis, eRoundMode.NEAREST))
        TextBox0205.Text = CStr(ConvertDistance(D, eRoundMode.NEAREST))
        IFprj.Z = hIFFix
        TextBox0206_Validating(TextBox0206, New System.ComponentModel.CancelEventArgs(True))
    End Sub

    Private Sub TextBox0202_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0202.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0202_Validating(TextBox0202, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0202.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0202_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0202.Validating
        Dim D As Double
        Dim fTmp As Double
        Dim hDis As Double
        Dim fDis As Double
        Dim hIFFix As Double
        Dim hIFMin As Double
        Dim hIFMax As Double

        If Not IsNumeric(TextBox0202.Text) Then Return

        D = Point2LineDistancePrj(ptFAP, IFprj, _ArDir + 90.0)
        hDis = DeConvertDistance(CDbl(TextBox0204.Text))
        hIFMin = ptFAP.Z
        hIFMax = (D - hDis) * arImDescent_PDG + ptFAP.Z

        If ComboBox0202.SelectedIndex = 0 Then
            fTmp = DeConvertHeight(CDbl(TextBox0202.Text)) - FicTHRprj.Z
        Else
            fTmp = DeConvertHeight(CDbl(TextBox0202.Text))
        End If

        hIFFix = fTmp

        If fTmp < hIFMin Then fTmp = hIFMin
        If fTmp > hIFMax Then fTmp = hIFMax

        'If CType(TextBox0202.Tag, Double) = fTmp Then
        '	hIFFix = fTmp
        '	ComboBox0202_SelectedIndexChanged(ComboBox0202, New System.EventArgs())
        '	Return
        'End If

        TextBox0202.Tag = fTmp
        If fTmp <> hIFFix Then
            hIFFix = fTmp
            ComboBox0202_SelectedIndexChanged(ComboBox0202, New System.EventArgs())
        End If

        IFprj.Z = hIFFix

        If arImDescent_PDG = 0.0 Then
            hDis = D
        Else
            hDis = D - (hIFFix - ptFAP.Z) / arImDescent_PDG
        End If

        TextBox0204.Text = CStr(ConvertDistance(hDis, eRoundMode.NEAREST))

        '======================================
        If ArrivalProfile.PointsNo = 4 Then
            ArrivalProfile.RemovePointByIndex(0)
            ArrivalProfile.RemovePointByIndex(0)
        End If

        fDis = Point2LineDistancePrj(ptFAP, FicTHRprj, _ArDir + 90.0)
        ArrivalProfile.InsertPoint(D + fDis, hIFFix, ILS.Course - ILS.MagVar, -RadToDeg(System.Math.Atan(arImDescent_PDG)), -1, 0)

        If arImDescent_PDG <> 0.0 Then
            ArrivalProfile.InsertPoint(D + fDis - (hIFFix - ptFAP.Z) / arImDescent_PDG, ptFAP.Z, ILS.Course - ILS.MagVar, 0, -1, 1)
        Else
            ArrivalProfile.InsertPoint(D + fDis, ptFAP.Z, ILS.Course - ILS.MagVar, 0, -1, 1)
        End If
    End Sub

    Private Sub TextBox0203_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0203.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0203_Validating(TextBox0203, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0203.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0203_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0203.Validating
        Dim D As Double
        Dim fTmp As Double
        Dim hDis As Double
        Dim hIFFix As Double

        If IsNumeric(TextBox0203.Text) Then
            fTmp = 0.01 * CDbl(TextBox0203.Text)
            If fTmp > arImDescent_Max.Value Then
                arImDescent_PDG = arImDescent_Max.Value
                TextBox0203.Text = CStr(100.0 * arImDescent_PDG)
            ElseIf fTmp < 0.0 Then
                arImDescent_PDG = 0.0
                TextBox0203.Text = "0"
            Else
                arImDescent_PDG = fTmp
            End If

            TextBox0203.Tag = TextBox0203.Text

            D = Point2LineDistancePrj(ptFAP, IFprj, _ArDir + 90.0)
            hDis = CDbl(DeConvertDistance(CDbl(TextBox0204.Text)))
            hIFFix = (D - hDis) * arImDescent_PDG + ptFAP.Z
            IFprj.Z = hIFFix

            TextBox0202.Tag = hIFFix
            ComboBox0202_SelectedIndexChanged(ComboBox0202, New System.EventArgs())
        Else
            TextBox0203.Text = TextBox0203.Tag
        End If
    End Sub

    Private Sub TextBox0204_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0204.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0204_Validating(TextBox0204, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0204.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0204_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0204.Validating
        Dim D As Double
        Dim fTmp As Double
        Dim hDis As Double
        Dim hDisMin As Double
        Dim hDisMax As Double
        Dim hIFFix As Double

        If Not IsNumeric(TextBox0204.Text) Then Return

        D = Point2LineDistancePrj(ptFAP, IFprj, _ArDir + 90.0)
        hDisMin = 0.0
        hDisMax = D

        fTmp = DeConvertDistance(CDbl(TextBox0204.Text))
        hDis = fTmp

        If fTmp < hDisMin Then fTmp = hDisMin
        If fTmp > hDisMax Then fTmp = hDisMax

        If fTmp <> hDis Then
            hDis = fTmp
            TextBox0204.Text = CStr(ConvertDistance(hDis, eRoundMode.NEAREST))
        End If

        hIFFix = (D - hDis) * arImDescent_PDG + ptFAP.Z
        IFprj.Z = hIFFix

        TextBox0202.Tag = hIFFix
        ComboBox0202_SelectedIndexChanged(ComboBox0202, New System.EventArgs())
    End Sub

    Private Sub TextBox0206_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0206.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0206_Validating(TextBox0206, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0206.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0206_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0206.Validating
        Dim I As Integer
        Dim N As Integer

        Dim K As Double
        Dim fTmp As Double
        Dim fDist As Double
        Dim fMinHeight As Double
        Dim fObstHeight As Double

        Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pFullRelat As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pPrimRelat As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pInitialFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pInitialPrimPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pIfLine As ESRI.ArcGIS.Geometry.IPolyline

        If Not IsNumeric(TextBox0206.Text) Then Return
        fDist = DeConvertDistance(CDbl(TextBox0206.Text))

        If fDist < arIFHalfWidth.Value Then
            fDist = arIFHalfWidth.Value
            TextBox0206.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))
        End If

        If fDist > 60000.0 Then
            fDist = 60000.0
            TextBox0206.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))
        End If

        pInitialFullPoly = New ESRI.ArcGIS.Geometry.Polygon
        pInitialPrimPoly = New ESRI.ArcGIS.Geometry.Polygon
        pPtTmp = PointAlongPlane(IFprj, _ArDir + 180.0, fDist)

        pInitialFullPoly.AddPoint(PointAlongPlane(IFprj, _ArDir + 90.0, arIFHalfWidth.Value))
        pInitialFullPoly.AddPoint(PointAlongPlane(IFprj, _ArDir - 90.0, arIFHalfWidth.Value))

        pInitialFullPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir - 90.0, arIFHalfWidth.Value))
        pInitialFullPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir + 90.0, arIFHalfWidth.Value))

        pTopo = pInitialFullPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pInitialPrimPoly.AddPoint(PointAlongPlane(IFprj, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
        pInitialPrimPoly.AddPoint(PointAlongPlane(IFprj, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))

        pInitialPrimPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
        pInitialPrimPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))

        pTopo = pInitialPrimPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pFullRelat = pInitialFullPoly
        pPrimRelat = pInitialPrimPoly
        N = UBound(ObstacleList.Obstacles)
        fMinHeight = arIASegmentMOC.Value

        pIfLine = New ESRI.ArcGIS.Geometry.Polyline()
        pIfLine.FromPoint = PointAlongPlane(IFprj, _ArDir, -5000.0)
        pIfLine.ToPoint = PointAlongPlane(ptFAP, _ArDir, 5000.0) 'PointAlongPlane(FicTHRprj, ArDir, 5000.0)
        pProxi = pIfLine

        'DrawPolyLine(pIfLine)
        'While True
        '	Application.DoEvents()
        'End While

        For I = 0 To N
            If Not pFullRelat.Disjoint(ObstacleList.Obstacles(I).pGeomPrj) Then
                If Not pFullRelat.Disjoint(ObstacleList.Obstacles(I).pGeomPrj) Then
                    K = 1.0
                Else
                    'K = 2 * (arIFHalfWidth.Value - Point2LineDistancePrj(IFprj, ObstacleList.Obstacles(I).pGeomPrj, ArDir)) / arIFHalfWidth.Value
                    K = 2 * (arIFHalfWidth.Value - pProxi.ReturnDistance(ObstacleList.Obstacles(I).pGeomPrj)) / arIFHalfWidth.Value

                    'DrawPolygon(ObstacleList.Obstacles(I).pGeomPrj, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
                    'Application.DoEvents()
                End If

                fObstHeight = ObstacleList.Obstacles(I).Height + arIASegmentMOC.Value * K
                If fObstHeight > fMinHeight Then
                    fMinHeight = fObstHeight
                End If
            End If
        Next I

        fTmp = ConvertHeight(fMinHeight + FicTHRprj.Z, 0)
        If System.Math.Abs(HeightConverter(HeightUnit).Multiplier - 1.0 / 0.3048) < mEps Then
            fTmp = 100.0 * (System.Math.Round(0.01 * fTmp + 0.499999))
        Else
            fTmp = 50.0 * (System.Math.Round(0.02 * fTmp + 0.499999))
        End If
        TextBox0207.Text = CStr(fTmp)
    End Sub

    Private Sub TextBox0208_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0208.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0208_Validating(TextBox0208, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxLimitCount(eventArgs.KeyChar, TextBox0208.Text, 5)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0208_Validating(eventSender As System.Object, eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0208.Validating
        '
    End Sub

#End Region

#Region "Page 3 events"

    Private Sub CheckBox0301_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0301.CheckedChanged
        If Not bFormInitialised Then Return

        OkBtn.Enabled = Not CheckBox0301.Checked
        NextBtn.Enabled = CheckBox0301.Checked
        _Label0301_3.Enabled = Not CheckBox0301.Checked

        TextBox0308.ReadOnly = CheckBox0301.Checked
        label0301_16.Visible = Not CheckBox0301.Checked
        TextBox0309.Visible = Not CheckBox0301.Checked
        label0301_17.Visible = Not CheckBox0301.Checked

        If CheckBox0301.Checked Then
            On Error Resume Next
            If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
            On Error GoTo 0
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        Else
            TextBox0308_Validating(TextBox0308, Nothing)
        End If
    End Sub

    Private Sub ComboBox0301_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0301.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim fTmp As Double
        If Not IsNumeric(TextBox0301.Tag) Then Return
        If ComboBox0301.SelectedIndex < 0 Then ComboBox0301.SelectedIndex = 0

        fTmp = CDbl(TextBox0301.Tag)

        If ComboBox0301.SelectedIndex = 0 Then
            TextBox0301.Text = CStr(ConvertHeight(fTmp + FicTHRprj.Z, eRoundMode.NEAREST))
        Else
            TextBox0301.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ComboBox0302_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0302.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim I As Integer
        Dim N As Integer
        Dim fMOC As Double
        Dim fReqH As Double

        If (MultiPage1.SelectedIndex < 2) Or (ComboBox0302.SelectedIndex < 0) Then Return

        fMOC = DeConvertHeight(CDbl(ComboBox0302.Text))
        fStraightMissedTermHght = fMOC 'arIASegmentMOC.Value

        N = UBound(MAObstacleList.Parts)
        For I = 0 To N
            fReqH = MAObstacleList.Parts(I).Height + fMOC

            If fReqH > fStraightMissedTermHght Then
                fStraightMissedTermHght = fReqH
            End If
        Next I

        _TerminationAltitude = fStraightMissedTermHght + FicTHRprj.Z

        TextBox0308.Text = CStr(ConvertHeight(_TerminationAltitude, eRoundMode.NEAREST))
        TextBox0308_Validating(TextBox0308, Nothing)
        'TextBox0308.Tag = TextBox0308.Text
    End Sub

    Private Sub TextBox0308_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0308.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox0308_Validating(TextBox0308, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(e.KeyChar, TextBox0308.Text)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox0308_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0308.Validating
        If CheckBox0301.Checked Then Return
        Dim fTmp As Double

        If Not Double.TryParse(TextBox0308.Text, fTmp) Then
            If Not Double.TryParse(TextBox0308.Tag, fTmp) Then TextBox0308.Text = TextBox0308.Tag
            Return
        End If

        Dim fNevValue As Double = DeConvertHeight(fTmp)
        Dim fMinVal As Double = fStraightMissedTermHght + FicTHRprj.Z
        Dim fMaxVal As Double = fMissAprPDG * 20.0 * 1852.0 + pMAPt.Z + FicTHRprj.Z

        If fMaxVal < fMinVal Then fMaxVal = fMinVal

        If fNevValue < fMinVal Then fNevValue = fMinVal
        If fNevValue > fMaxVal Then fNevValue = fMaxVal

        If fNevValue <> fTmp Then TextBox0308.Text = ConvertHeight(fNevValue, eRoundMode.NEAREST).ToString()
        _TerminationAltitude = fNevValue

        fTmp = (_TerminationAltitude - fMisAprOCH - FicTHRprj.Z) / fMissAprPDG + dMAPt2SOC
        TextBox0309.Text = ConvertDistance(fTmp).ToString()

        TextBox0308.Tag = TextBox0308.Text

        pStraightNomLine = New Polyline()

        pTermPt = PointAlongPlane(pMAPt, _ArDir, fTmp)
        pTermPt.Z = _TerminationAltitude

        pStraightNomLine.FromPoint = pMAPt
        pStraightNomLine.ToPoint = pTermPt

        On Error Resume Next
        If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
        On Error GoTo 0

        MAPtCLineElem = DrawPolyLine(pStraightNomLine, RGB(255, 0, 128), 2)
    End Sub

    Private Sub TextBox0309_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0309.KeyPress
        Return
        If e.KeyChar = Chr(13) Then
            TextBox0309_Validating(TextBox0309, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(e.KeyChar, TextBox0309.Text)
        End If

        If e.KeyChar = Chr(0) Then e.Handled = True
    End Sub

    Private Sub TextBox0309_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0309.Validating
        Return
        If CheckBox0301.Checked Then Return

        Dim fTmp As Double

        If Not Double.TryParse(TextBox0309.Text, fTmp) Then
            If Not Double.TryParse(TextBox0309.Tag, fTmp) Then
                TextBox0309.Text = TextBox0309.Tag
            End If

            Return
        End If

        Dim fNevValue As Double = fTmp
        Dim fMinVal As Double = 2.0
        Dim fMaxVal As Double = 10.0

        If fNevValue < fMinVal Then fNevValue = fMinVal
        If fNevValue > fMaxVal Then fNevValue = fMaxVal

        If fNevValue <> fTmp Then
            fTmp = DeConvertDistance(fNevValue)
            TextBox0309.Text = ConvertDistance(fTmp, eRoundMode.NEAREST).ToString()
        End If

        TextBox0309.Tag = TextBox0309.Text

        Dim pTermPt As ESRI.ArcGIS.Geometry.IPoint
        Dim pPolyline As IPolyline

        pPolyline = New Polyline()

        fTmp = DeConvertDistance(fNevValue)

        'pTermPt = PointAlongPlane(PtSOC, _ArDir, fTmp)
        'fTmp = ConvertDistance(fTmp + dMAPt2SOC, eRoundMode.NEAREST)

        pTermPt = PointAlongPlane(pMAPt, _ArDir, fTmp)
        fTmp = ConvertDistance(fTmp, eRoundMode.NEAREST)

        pPolyline.FromPoint = pMAPt
        pPolyline.ToPoint = pTermPt

        On Error Resume Next
        If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
        On Error GoTo 0

        MAPtCLineElem = DrawPolyLine(pPolyline, RGB(255, 0, 128), 2)
    End Sub

#End Region

#Region "Page 4 events"
    Private Sub OptionButton0401_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0401.CheckedChanged
        If Not bFormInitialised Then Return

        If Not eventSender.Checked Then Return
        Dim H As Integer

        CheckBox0402.Enabled = True
        _Label0501_7.Visible = False
        ComboBox0503.Visible = True
        'ComboBox0503.ListIndex = 0

        _Label0501_16.Text = HeightConverter(HeightUnit).Unit

        Frame0501.Visible = False
        _Label0501_18.Visible = False

        _Label0501_3.Visible = True
        TextBox0504.Visible = True

        _Label0501_18.ForeColor = System.Drawing.SystemColors.ControlText
        _Label0501_8.Text = My.Resources.str20602

        H = System.Math.Round(TextBox0504.Top + 0.5 * (TextBox0504.Height - _Label0501_8.Height))
        _Label0501_8.Top = H

        _Label0901_4.Text = My.Resources.str16008
    End Sub

    Private Sub OptionButton0402_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0402.CheckedChanged
        If Not bFormInitialised Then Return

        If eventSender.Checked Then
            CheckBox0402.Enabled = False

            _Label0501_7.Visible = True
            ComboBox0503.Visible = False

            _Label0501_16.Text = DistanceConverter(DistanceUnit).Unit
            Frame0501.Visible = True
            _Label0501_18.Visible = True

            _Label0501_3.Visible = False
            TextBox0504.Visible = False

            _Label0501_8.Text = My.Resources.str20612
            _Label0501_8.Top = System.Math.Round(TextBox0504.Top + 0.5 * (TextBox0504.Height - _Label0501_8.Height))

            _Label0901_4.Text = My.Resources.str16002
        End If
    End Sub

    Private Sub ComboBox0401_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0401.SelectedIndexChanged
        If Not bFormInitialised Then Return

        TurnDir = 1 - 2 * ComboBox0401.SelectedIndex
    End Sub

    Private Sub TextBox0401_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0401.KeyPress
        TextBoxFloat(eventArgs.KeyChar, TextBox0401.Text)

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0402_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0402.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0402_Validating(TextBox0402, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0402.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0402_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0402.Validating
        Dim K As Integer
        Dim fTmp As Double
        Dim fMinVal As Double
        Dim fMaxVal As Double

        K = ComboBox0003.SelectedIndex
        If (K < 0) Or Not IsNumeric(TextBox0402.Text) Then Return
        fMinVal = ConvertSpeed(cVmaInter.Values(K), eRoundMode.SPECIAL)
        fMaxVal = ConvertSpeed(cVmaFaf.Values(K), eRoundMode.SPECIAL)
        fTmp = CDbl(TextBox0402.Text)
        If fTmp < fMinVal Then TextBox0402.Text = CStr(fMinVal)
        If fTmp > fMaxVal Then TextBox0402.Text = CStr(fMaxVal)
    End Sub

#End Region

#Region "Page 5 events"
    Private Sub ComboBox0501_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0501.SelectedIndexChanged
        If Not bFormInitialised Then Return

        Dim bInRange As Boolean

        Dim I As Integer
        Dim K As Integer
        Dim L As Integer
        'Dim N As Integer

        Dim D As Double
        Dim d0 As Double
        Dim Ls As Double
        Dim Azt As Double
        Dim fTmp As Double
        Dim hFix As Double
        Dim fDis As Double
        Dim fMin As Double
        Dim fMax As Double
        Dim fDirl As Double
        Dim fTASl As Double
        Dim VTotal As Double
        Dim FixDist As Double
        Dim InRange As Double
        Dim fDistMin As Double
        Dim fDistMax As Double
        Dim InterToler As Double

        Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
        Dim pNomTP As ESRI.ArcGIS.Geometry.IPoint
        'Dim pPtNAV As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
        'Dim pFIXArea As ESRI.ArcGIS.Geometry.IPolygon 'pPIXInterPoly

        Dim NavDat As NavaidData

        Dim pPolyClone As ESRI.ArcGIS.Geometry.IPolygon
        Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        If Not FIXElem Is Nothing Then
            pGroupElement = FIXElem
            For I = 0 To pGroupElement.ElementCount - 1
                Try
                    If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
                Catch
                End Try
            Next I
        End If

        _Label0501_10.Text = ""
        _Label0501_18.Text = ""
        pGroupElement = New ESRI.ArcGIS.Carto.GroupElement()

        K = ComboBox0501.SelectedIndex
        If K < 0 Then Return
        NavDat = TPInterNavDat(ArrayNum, K)

        _Label0501_11.Text = GetNavTypeName(NavDat.TypeCode)

        pClone = pFullPoly
        pPolyClone = pClone.Clone
        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        TextBox0509.Visible = True
        _Label0501_12.Visible = True
        TextBox0507.ReadOnly = False
        TextBox0507.BackColor = System.Drawing.SystemColors.Window

        FixDist = DeConvertDistance(CDbl(TextBox0507.Text))

        Dim sStr As String
        If NavDat.IntersectionType = eIntersectionType.ByDistance Then
            'DME/RadarFIX ===============================================================================
            OptionButton0501.Visible = True
            OptionButton0502.Visible = True

            L = UBound(NavDat.ValMin)
            ReDim TextBox0507Intervals(L)

            If (L = 0) And (NavDat.ValCnt <= 0) Then
                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMax(0), PtSOC, _ArDir, pNomTP)
            Else
                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMin(0), PtSOC, _ArDir + 180.0, pNomTP)
            End If

            fMin = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)

            If (L = 0) And (NavDat.ValCnt <= 0) Then
                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMin(0), PtSOC, _ArDir, pNomTP)
            Else
                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMax(0), PtSOC, _ArDir + 180.0, pNomTP)
            End If

            fMax = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)
            If fMin > fMax Then
                fTmp = fMin
                fMin = fMax
                fMax = fTmp
            End If
            TextBox0507Intervals(0).Low = fMin
            TextBox0507Intervals(0).High = fMax

            sStr = My.Resources.str20312 + ":" + My.Resources.str00221 + CStr(ConvertDistance(fMin, 3)) + DistanceConverter(DistanceUnit).Unit + ", " + My.Resources.str00222 + CStr(ConvertDistance(fMax, 1)) + DistanceConverter(DistanceUnit).Unit

            If L > 0 Then
                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMax(1), PtSOC, _ArDir, pNomTP)
                fMin = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)

                CircleVectorIntersect(NavDat.pPtPrj, NavDat.ValMin(1), PtSOC, _ArDir, pNomTP)
                fMax = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)

                If fMin > fMax Then
                    fTmp = fMin
                    fMin = fMax
                    fMax = fTmp
                End If
                TextBox0507Intervals(1).Low = fMin
                TextBox0507Intervals(1).High = fMax

                sStr = sStr + vbCrLf + My.Resources.str20312 + ":" + My.Resources.str00221 + CStr(ConvertDistance(fMin, 3)) + DistanceConverter(DistanceUnit).Unit + ", " + My.Resources.str00222 + CStr(ConvertDistance(fMax, 1)) + DistanceConverter(DistanceUnit).Unit
            End If

            '===============================================================================
            D = RModel
            InRange = FixDist

            For I = 0 To L
                bInRange = (FixDist >= TextBox0507Intervals(I).Low) And (FixDist <= TextBox0507Intervals(I).High)
                If bInRange Then Exit For

                fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(I).Low)
                If fTmp < D Then
                    InRange = ConvertDistance(TextBox0507Intervals(I).Low, 3)
                    D = fTmp
                End If

                fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(I).High)
                If fTmp < D Then
                    InRange = ConvertDistance(TextBox0507Intervals(I).High, 1)
                    D = fTmp
                End If
            Next I

            'If TextBox0507.Text = CStr(InRange) Then
            '	Return
            'End If

            If Not bInRange Then
                TextBox0507.Tag = ""
                TextBox0507.Text = CStr(InRange) '- NavDat.Disp
                TextBox0507_Validating(TextBox0507, New System.ComponentModel.CancelEventArgs(True))
                Return
            End If

            _Label0501_18.Text = sStr
            '===============================================================================
            If SideDef(NavDat.pPtPrj, _ArDir + 90.0, TurnFixPnt) < 0 Then
                OptionButton0501.Checked = True
                OptionButton0502.Checked = False
            Else
                OptionButton0501.Checked = False
                OptionButton0502.Checked = True
            End If

            _Label0501_10.Text = My.Resources.str10511  'Verify
            _Label0501_12.Text = DistanceConverter(DistanceUnit).Unit

            '============================================================================+++++++++++++++++++
            'fDis = Point2LineDistancePrj(FicTHRprj, KK.FromPoint, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, KK.FromPoint)
            'hTurn = (XptLH - fDis - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
            'TurnFixPnt.Z = hTurn + FicTHRprj.Z
            '============================================================================+++++++++++++++++++

            fDis = ReturnDistanceInMeters(NavDat.pPtPrj, TurnFixPnt)

            d0 = ReturnDistanceInMeters(PtSOC, TurnFixPnt)
            hFix = d0 * fMissAprPDG + PtSOC.Z + FicTHRprj.Z

            Dim fdH As Double = hFix - NavDat.pPtPrj.Z

            d0 = System.Math.Sqrt(fDis * fDis + fdH * fdH)

            TextBox0509.Text = CStr(ConvertDistance(d0 - NavDat.Disp, eRoundMode.NEAREST))

            d0 = d0 * DME.ErrorScalingUp + DME.MinimalError

            D = fDis + d0
            pSect0 = CreatePrjCircle(NavDat.pPtPrj, D)

            D = fDis - d0
            pSect1 = CreatePrjCircle(NavDat.pPtPrj, D)

            pTopo = pSect0
            pTmpPoly = pTopo.Difference(pSect1)

            pCutter.FromPoint = PointAlongPlane(NavDat.pPtPrj, _ArDir - 90.0, DME.Range + fDis + fDis)
            pCutter.ToPoint = PointAlongPlane(NavDat.pPtPrj, _ArDir + 90.0, DME.Range + fDis + fDis)

            If SideDef(pCutter.FromPoint, _ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If OptionButton0502.Checked Then
                pTopo.Cut(pCutter, pSect1, pSect0)
            Else
                pTopo.Cut(pCutter, pSect0, pSect1)
            End If

            pTopo = pSect0
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pTPTolerArea = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
        Else
            pNomTP = New ESRI.ArcGIS.Geometry.Point
            OptionButton0501.Visible = False
            OptionButton0502.Visible = False

            If NavDat.IntersectionType = eIntersectionType.OnNavaid Then
                d0 = ReturnDistanceInMeters(NavDat.pPtPrj, PtSOC)
                hFix = d0 * fMissAprPDG + PtSOC.Z + FicTHRprj.Z

                If NavDat.TypeCode = eNavaidType.NDB Then
                    NDBFIXTolerArea(NavDat.pPtPrj, _ArDir, hFix, pTPTolerArea)
                Else
                    VORFIXTolerArea(NavDat.pPtPrj, _ArDir, hFix, pTPTolerArea)
                End If

                TurnFixPnt = New ESRI.ArcGIS.Geometry.Point
                TurnFixPnt.PutCoords(NavDat.pPtPrj.X, NavDat.pPtPrj.Y)

                TurnFixPnt.M = _ArDir

                FixDist = ReturnDistanceInMeters(FicTHRprj, NavDat.pPtPrj) * SideDef(FicTHRprj, _ArDir - 90.0, NavDat.pPtPrj)
                TextBox0507.Text = CStr(ConvertDistance(FixDist, eRoundMode.NEAREST))

                _Label0501_10.Text = My.Resources.str00106
                _Label0501_12.Text = ""

                TextBox0509.Visible = False
                _Label0501_12.Visible = False
                TextBox0507.ReadOnly = True
                TextBox0507.BackColor = System.Drawing.SystemColors.Control
            Else
                pConstruct = pNomTP
                pConstruct.ConstructAngleIntersection(PtSOC, DegToRad(_ArDir), NavDat.pPtPrj, DegToRad(NavDat.ValMin(0)))
                fMin = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)

                pConstruct.ConstructAngleIntersection(PtSOC, DegToRad(_ArDir), NavDat.pPtPrj, DegToRad(NavDat.ValMax(0)))
                fMax = ReturnDistanceInMeters(FicTHRprj, pNomTP) * SideDef(FicTHRprj, _ArDir - 90.0, pNomTP)

                If fMin > fMax Then
                    fTmp = fMin
                    fMin = fMax
                    fMax = fTmp
                End If

                ReDim TextBox0507Intervals(0)
                TextBox0507Intervals(0).Low = fMin
                TextBox0507Intervals(0).High = fMax

                '===============================================================================
                D = RModel
                InRange = FixDist

                bInRange = (FixDist >= TextBox0507Intervals(0).Low) And (FixDist <= TextBox0507Intervals(0).High)

                If Not bInRange Then
                    fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(0).Low)
                    If fTmp < D Then
                        InRange = ConvertDistance(TextBox0507Intervals(0).Low, 3)
                        D = fTmp
                    End If

                    fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(0).High)
                    If fTmp < D Then
                        InRange = ConvertDistance(TextBox0507Intervals(0).High, 1)
                        D = fTmp
                    End If

                    '_Label0501_10.Text = ""
                    'TextBox0507.Tag = ""
                    TextBox0507.Text = CStr(ConvertDistance(InRange, eRoundMode.NEAREST))
                    TextBox0507_Validating(TextBox0507, Nothing)
                    Return
                End If
                '===============================================================================

                _Label0501_18.Text = My.Resources.str20312 + ":" + My.Resources.str00221 + CStr(ConvertDistance(fMin, 3)) + DistanceConverter(DistanceUnit).Unit + ", " + My.Resources.str00222 + CStr(ConvertDistance(fMax, 1)) + DistanceConverter(DistanceUnit).Unit
                _Label0501_12.Text = "°"

                fDirl = ReturnAngleInDegrees(NavDat.pPtPrj, TurnFixPnt)

                If NavDat.TypeCode = eNavaidType.NDB Then
                    InterToler = NDB.IntersectingTolerance
                    _Label0501_10.Text = My.Resources.str00228
                    Azt = Modulus(Dir2Azt(NavDat.pPtPrj, fDirl) - TPInterNavDat(ArrayNum, K).MagVar + 180.0, 360.0)
                    TextBox0509.Text = CStr(System.Math.Round(Azt, 2))
                Else
                    InterToler = VOR.IntersectingTolerance
                    _Label0501_10.Text = My.Resources.str00227
                    Azt = Modulus(Dir2Azt(NavDat.pPtPrj, fDirl) - TPInterNavDat(ArrayNum, K).MagVar, 360.0)
                    TextBox0509.Text = CStr(System.Math.Round(Azt, 2))
                End If

                fDis = NavDat.Range
                pt1 = PointAlongPlane(NavDat.pPtPrj, fDirl + InterToler, fDis)
                pt2 = PointAlongPlane(NavDat.pPtPrj, fDirl - InterToler, fDis)

                pSect0 = New ESRI.ArcGIS.Geometry.Polygon
                pSect0.AddPoint(NavDat.pPtPrj)
                pSect0.AddPoint(pt1)
                pSect0.AddPoint(pt2)
                pSect0.AddPoint(NavDat.pPtPrj)

                pTopo = pSect0
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                pTPTolerArea = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            End If
        End If
        '================================================================
        ToolTip1.SetToolTip(TextBox0507, _Label0501_18.Text)

        '================================================================

        pTopo = pTPTolerArea
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pPtTmp = PointAlongPlane(TurnFixPnt, _ArDir - 180.0, 500000.0)
        pCutter.FromPoint = PointAlongPlane(pPtTmp, _ArDir - 90.0, RModel)
        pCutter.ToPoint = PointAlongPlane(pPtTmp, _ArDir + 90.0, RModel)

        pProxi = pCutter
        fDistMin = 500000.0 - pProxi.ReturnDistance(pTPTolerArea) 'fTmp = 500000# - pProxi.ReturnDistance(pFullPoly)'If fTmp < fDistMin Then fDistMin = fTmp

        pPtTmp = PointAlongPlane(TurnFixPnt, _ArDir, 500000.0)
        pCutter.FromPoint = PointAlongPlane(pPtTmp, _ArDir - 90.0, RModel)
        pCutter.ToPoint = PointAlongPlane(pPtTmp, _ArDir + 90.0, RModel)

        pProxi = pCutter
        fDistMax = 500000.0 - pProxi.ReturnDistance(pTPTolerArea) 'fTmp = 500000# - pProxi.ReturnDistance(pFullPoly)'If fTmp < fDistMax Then fDistMax = fTmp

        'KK ===============================================================================
        pPtTmp = PointAlongPlane(TurnFixPnt, _ArDir - 180.0, fDistMin)
        pCutter.FromPoint = PointAlongPlane(pPtTmp, _ArDir - 90.0, RModel)
        pCutter.ToPoint = PointAlongPlane(pPtTmp, _ArDir + 90.0, RModel)
        pTopo = pPolyClone

        'DrawPointWithText TurnFixPnt, "TurnFixPnt"
        'DrawPolygon pPolyClone, 0
        'DrawPolygon pTopo, 255
        'DrawPolyLine pCutter, 255, 2
        'DrawPointWithText pPtTmp, "KK", 0

        KK = pTopo.Intersect(pCutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
        If SideDef(KK.ToPoint, _ArDir, KK.FromPoint) < 0 Then KK.ReverseOrientation()

        fDis = Point2LineDistancePrj(FicTHRprj, KK.FromPoint, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, KK.FromPoint)
        hTurn = (XptLH - fDis - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
        TurnFixPnt.Z = hTurn + FicTHRprj.Z
        TurnFixPnt.M = _ArDir

        TextBox0508.Text = CStr(ConvertHeight(hTurn, eRoundMode.NEAREST))

        'DrawPolygon pTopo, 0

        pTopo.Cut(pCutter, pTmpPoly, pSect1)
        '    pTopo.Cut pCutter, pSect1, pTmpPoly
        'DrawPolygon pSect1, 255
        'DrawPolygon pTmpPoly, RGB(0, 255, 0)

        pTopo = pSect1 '   pTmpPoly    '
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        'Set pClone = pTmpPoly
        'Set pSect1 = pClone.Clone
        'KK max(-6) ====================================================================================
        pPtTmp = PointAlongPlane(TurnFixPnt, _ArDir, fDistMax)
        pCutter.FromPoint = PointAlongPlane(pPtTmp, _ArDir - 90.0, RModel)
        pCutter.ToPoint = PointAlongPlane(pPtTmp, _ArDir + 90.0, RModel)

        'DrawPolyLine pCutter, 255, 2
        'DrawPointWithText pPtTmp, "KK max(K1K1-6sec)", 0

        'DrawPolygon pSect1, 255
        'DrawPolyLine pCutter, 255, 2
        'DrawPolygon pSect1, 255

        KKFixMax = pTopo.Intersect(pCutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
        If SideDef(KKFixMax.ToPoint, _ArDir, KKFixMax.FromPoint) < 0 Then KKFixMax.ReverseOrientation()
        pTopo.Cut(pCutter, pFIXPoly_6, pSect0)

        'DrawPolygon pFIXPoly_6, 255
        'K1K1 ===========================================================================================

        fTASl = IAS2TAS(m_fIAS, hTurn, CurrADHP.ISAtC)
        VTotal = fTASl + CurrADHP.WindSpeed
        Ls = VTotal * arT_TechToleranc.Value * 0.277777777777778

        pPtTmp = PointAlongPlane(TurnFixPnt, _ArDir, fDistMax + Ls)
        pCutter.FromPoint = PointAlongPlane(pPtTmp, _ArDir - 90.0, 100000.0)
        pCutter.ToPoint = PointAlongPlane(pPtTmp, _ArDir + 90.0, 100000.0)

        'DrawPolyLine pCutter, 255, 2
        'DrawPointWithText pPtTmp, "K1K1", 0

        '    Set pTopo = pSect1
        '    pTopo.IsKnownSimple_2 = False
        '    pTopo.Simplify

        'DrawPolygon pSect1, 0
        'DrawPolyLine pCutter, 0, 2
        'DrawPolygon pFIXArea, RGB(0, 0, 255)
        K1K1 = pTopo.Intersect(pCutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
        If SideDef(K1K1.ToPoint, _ArDir, K1K1.FromPoint) < 0 Then K1K1.ReverseOrientation()

        'DrawPolyLine pCutter, 255, 2
        'DrawPointWithText pPtTmp, "KK max", 0

        pTopo.Cut(pCutter, pFixPoly, pSect0)
        pTopo = pFixPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        '================================================================
        While ArrivalProfile.PointsNo > ArrivalProfile.MAPtIndex
            ArrivalProfile.RemovePoint()
        End While

        d0 = Point2LineDistancePrj(FicTHRprj, TurnFixPnt, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, TurnFixPnt)
        ArrivalProfile.AddPoint(d0, TurnFixPnt.Z - FicTHRprj.Z, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
        '================================================================
        pGroupElement.AddElement(DrawPolygon(pFixPoly, 255, , False))
        pGroupElement.AddElement(DrawPolygon(pTPTolerArea, RGB(198, 198, 198), , False))
        pGroupElement.AddElement(DrawPointWithText(TurnFixPnt, "TP", WPTColor, False))
        FIXElem = pGroupElement

        For I = 0 To pGroupElement.ElementCount - 1
            pGroupElement.Element(I).Locked = True
            pGraphics.AddElement(pGroupElement.Element(I), 0)
        Next I
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub ComboBox0502_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0502.SelectedIndexChanged
        If ComboBox0502.SelectedIndex = 0 Then
            TextBox0503.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.NEAREST))
        Else
            TextBox0503.Text = CStr(ConvertHeight(fMisAprOCH + FicTHRprj.Z, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ComboBox0503_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0503.SelectedIndexChanged
        If ComboBox0503.SelectedIndex = 0 Then
            TextBox0507.Text = CStr(ConvertHeight(fHTurn + FicTHRprj.Z, eRoundMode.NEAREST))
        Else
            TextBox0507.Text = CStr(ConvertHeight(fHTurn, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub ListView501_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView501.SelectedIndexChanged
        ReSelectTrace()
    End Sub

    Private Sub TextBox0503_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0503.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0503_Validating(TextBox0503, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0503.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0503_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0503.Validating
        Dim N As Integer
        Dim d0 As Double
        Dim fMinOCH As Double
        Dim CurrOCH As Double
        Dim NextOCH As Double
        Dim fMAPtDist As Double
        Dim OCHInterv As LowHigh

        If Not IsNumeric(TextBox0503.Text) Then Return

        fMisAprOCH = DeConvertHeight(CDbl(TextBox0503.Text))
        If ComboBox0502.SelectedIndex = 1 Then
            fMisAprOCH = fMisAprOCH - FicTHRprj.Z
        End If

        If IxOCH > -1 Then
            If SideDef(PtCoordCntr, _ArDir + 90.0, WorkObstacleList.Parts(IxOCH).pPtPrj) < 0 Then
                fMinOCH = _CurrFAPOCH
            Else
                fMinOCH = Max(m_fMOC, fRDHOCH) 'IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
            End If
        Else
            fMinOCH = Max(m_fMOC, fRDHOCH) 'IIf(m_fMOC > fRDHOCH, m_fMOC, fRDHOCH)
        End If

        'DrawPointWithText(PtCoordCntr, "CoordCntr")
        'Application.DoEvents()

        If fMisAprOCH < fMinOCH Then fMisAprOCH = fMinOCH

        If OptionButton0401.Checked And CheckBox0402.Checked Then
            CurrOCH = fMisAprOCH
            fHTurn = hTurn - FicTHRprj.Z
            CalcReducedTNH(DetTNHObstacles, ILS.GPAngle, fMissAprPDG, m_fMOC, CurrOCH, fHTurn, m_IxMinOCH)
        Else
            NextOCH = fMisAprOCH
            IxMaxOCH = -1
            m_IxMinOCH = -1

            Do
                CurrOCH = NextOCH
                OCHInterv = CalcOCHRange(CurrOCH, NextOCH, m_IxMinOCH, IxMaxOCH)
                m_TurnIntervals = CalcTurnInterval(CurrOCH, NextOCH, OCHInterv, m_IxMinOCH)
            Loop While (CurrOCH > NextOCH) And (IxMaxOCH >= 0)
        End If

        fMisAprOCH = CurrOCH
        DrawMAPtSOC(fMisAprOCH, NextOCH, m_TurnIntervals, IxMaxOCH) ', fHTurn

        '======================================================================

        N = ArrivalProfile.MAPtIndex - 1
        While ArrivalProfile.PointsNo > N
            ArrivalProfile.RemovePoint()
        End While

        'fMAPtDist = FAPDist - (hFAP - fMisAprOCH) / Tan(DegToRad(ILS.GPAngle))
        fMAPtDist = (fMisAprOCH - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))
        ArrivalProfile.AddPoint(fMAPtDist, fMisAprOCH, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), CodeProcedureDistance.MAP)

        '================================================================
        d0 = Point2LineDistancePrj(FicTHRprj, TurnFixPnt, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, TurnFixPnt)
        ArrivalProfile.AddPoint(d0, TurnFixPnt.Z - FicTHRprj.Z, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
        '======================================================================

        If ComboBox0502.SelectedIndex < 0 Then
            ComboBox0502.SelectedIndex = 0
        Else
            ComboBox0502_SelectedIndexChanged(ComboBox0502, New System.EventArgs())
        End If
        'TextBox0503.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.rmNERAEST))

        If TextBox0507.Tag <> "a" Then
            TextBox0507.Tag = ""
            TextBox0507_Validating(TextBox0507, New System.ComponentModel.CancelEventArgs())
        End If
    End Sub

    Private Sub TextBox0507_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0507.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0507_Validating(TextBox0507, New System.ComponentModel.CancelEventArgs())
        Else
            If OptionButton0402.Checked Then
                TextBoxFloatWithMinus(eventArgs.KeyChar, TextBox0507.Text)
            Else
                TextBoxFloat(eventArgs.KeyChar, TextBox0507.Text)
            End If
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0507_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0507.Validating
        Dim bInRange As Boolean

        Dim I As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim Ix As Integer

        Dim IL As Integer
        Dim IK As Integer
        Dim IH As Integer
        Dim Indx As Boolean
        Dim PrevNavIx As Integer

        Dim E As Double
        Dim Ls As Double
        Dim L0 As Double
        Dim fTmp As Double
        Dim fTASl As Double
        Dim Range As Double
        Dim VTotal As Double
        Dim TNHLow As Double
        Dim InRange As Double
        Dim TNHHigh As Double
        Dim FixDist As Double
        Dim CurrOCH As Double

        Dim pt300 As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pLeftPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pRightPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
        Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
        Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim PrevNav As NavaidData

        If Not IsNumeric(TextBox0507.Text) Then Return
        If TextBox0507.Tag = TextBox0507.Text Then Return

        If OptionButton0401.Checked Then
            '==================== На абсолютной высоте
            pLine = New ESRI.ArcGIS.Geometry.Polyline
            fHTurn = DeConvertHeight(CDbl(TextBox0507.Text))

            If ComboBox0503.SelectedIndex = 0 Then fHTurn = fHTurn - FicTHRprj.Z

            On Error Resume Next
            If Not FIXElem Is Nothing Then
                pGroupElement = FIXElem
                For I = 0 To pGroupElement.ElementCount - 1
                    If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
                Next I
            End If
            If Not ReducedTIAElem Is Nothing Then pGraphics.DeleteElement(ReducedTIAElem)
            On Error GoTo 0

            pGroupElement = New ESRI.ArcGIS.Carto.GroupElement

            If CheckBox0402.Checked Then
                CurrOCH = fMisAprOCH
                CalcReducedTNH(DetTNHObstacles, ILS.GPAngle, fMissAprPDG, m_fMOC, CurrOCH, fHTurn, m_IxMinOCH)
                If CurrOCH > fMisAprOCH Then
                    fMisAprOCH = CurrOCH
                    TextBox0505.Text = CStr(ConvertHeight(fMisAprOCH, eRoundMode.NEAREST))

                    DrawMAPtSOC(fMisAprOCH, 0, m_TurnIntervals, IxMaxOCH)   ', fHTurn
                    Return
                End If

                pTopo = pFixedTIAPart

                N = OASPlanes.Length
                ReDim hOASPlanes(N - 1)
                Array.Copy(OASPlanes, hOASPlanes, N)

                CreateOASPlanes(FicTHRprj, _ArDir, fHTurn, hOASPlanes, 0)

                pLeftPoly = pTopo.Union(hOASPlanes(eOAS.CommonPlane).Poly)

                '======================
                fTmp = -(OASPlanes(eOAS.WPlane).Plane.C * fHTurn + OASPlanes(eOAS.WPlane).Plane.D) / OASPlanes(eOAS.WPlane).Plane.A
                pt300 = IntersectPlanesAtHeight(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), 300.0)
                pTopo = pLeftPoly

                If pt300.X < fTmp - 0.01 Then
                    pTopo.IsKnownSimple_2 = False
                    pTopo.Simplify()

                    pTransform = pt300
                    pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
                    pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

                    pLine.FromPoint = PointAlongPlane(pt300, _ArDir - 90.0, MaxModelRadius)
                    pLine.ToPoint = PointAlongPlane(pt300, _ArDir + 90.0, MaxModelRadius)

                    On Error GoTo ErrorM
                    pTopo.Cut(pLine, pRightPoly, pReducedTIA)
                    pTopo = pReducedTIA
ErrorM:
                    On Error GoTo 0
                End If

                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                pReducedTIA = pTopo
                '======================
                ReducedTIAElem = DrawPolygon(pReducedTIA, RGB(0, 168, 176))
                ReducedTIAElem.Locked = True

                pLeftPoly = pTopo.Union(OASPlanes(eOAS.ZPlane).Poly)
                pTopo = pLeftPoly
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                pClone = pLeftPoly '= pReducedTIA
            Else '   Reduced
                N = UBound(m_TurnIntervals)
                If N < 0 Then Return

                TNHLow = (m_TurnIntervals(0).Low - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
                TNHHigh = (m_TurnIntervals(N).High - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC

                'fMinTNH = TNHLow
                'fMaxTNH = TNHHigh

                If fHTurn < TNHLow Then
                    fHTurn = TNHLow
                ElseIf fHTurn > TNHHigh Then
                    fHTurn = TNHHigh
                Else
                    For I = 0 To N
                        TNHLow = (m_TurnIntervals(I).Low - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
                        TNHHigh = (m_TurnIntervals(I).High - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC

                        If (fHTurn >= TNHLow) And (fHTurn <= TNHHigh) Then
                            IL = I
                            IH = I
                            Exit For
                        End If
                        If fHTurn <= TNHLow Then
                            IL = I
                            Exit For
                        End If
                        If fHTurn >= TNHHigh Then IH = I
                    Next I


                    If IL <> IH Then fHTurn = System.Math.Round(TNHLow + 0.4999)
                End If
                pClone = pFullPoly
            End If
        Else    '==================== Над FIX
            N = UBound(m_TurnIntervals)
            If N < 0 Then Return

            M = UBound(TPInterNavDat)
            If (ArrayNum < 0) Or (ArrayNum > M) Then Return

            M = UBound(TPInterNavDat, 2)
            I = ComboBox0501.SelectedIndex
            If (M > -1) And (I >= 0) And (I <= M) Then
                PrevNav = TPInterNavDat(ArrayNum, I)
            End If

            FixDist = DeConvertDistance(CDbl(TextBox0507.Text))
            M = UBound(TextBox0507Intervals)
            E = RModel
            Range = FixDist

            For I = 0 To M
                bInRange = (FixDist >= TextBox0507Intervals(I).Low) And (FixDist <= TextBox0507Intervals(I).High)
                If bInRange Then Exit For

                fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(I).Low)
                If fTmp < E Then
                    Range = TextBox0507Intervals(I).Low
                    InRange = ConvertDistance(FixDist, 3)
                    E = fTmp
                End If

                fTmp = System.Math.Abs(FixDist - TextBox0507Intervals(I).High)
                If fTmp < E Then
                    Range = TextBox0507Intervals(I).High
                    InRange = ConvertDistance(FixDist, 1)
                    E = fTmp
                End If
            Next I

            If prevText <> TextBox0507.Text Then
                prevText = TextBox0507.Text
                prevCnt = 0
            Else
                prevCnt += 1
            End If

            If Not bInRange Then
                FixDist = Range
                TextBox0507.Text = CStr(InRange)
            End If

            fHTurn = (XptLH - FixDist - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC

            ArrayNum = -1
            TNHLow = XptLH - m_TurnIntervals(N).High
            TNHHigh = XptLH - m_TurnIntervals(0).Low

            If FixDist < TNHLow Then
                FixDist = TNHLow
                ArrayNum = N
                IL = N
                IH = N
            ElseIf FixDist > TNHHigh Then
                FixDist = TNHHigh
                ArrayNum = 0
                IL = 0
                IH = 0
            Else
                For I = 0 To N
                    TNHLow = XptLH - m_TurnIntervals(I).Low
                    TNHHigh = XptLH - m_TurnIntervals(I).High

                    If (FixDist >= TNHHigh) And (FixDist <= TNHLow) Then
                        IL = I
                        IH = I
                        Exit For
                    End If

                    If FixDist <= TNHLow Then IL = I
                    If FixDist >= TNHHigh Then
                        IH = I
                        Exit For
                    End If
                Next I
                ArrayNum = IL
                If IL <> IH Then FixDist = TNHLow
            End If

            fHTurn = (XptLH - FixDist - XptSOC) * fMissAprPDG + fMisAprOCH - m_fMOC
            pClone = pFullPoly

            If prevCnt < 2 Then
                N = UBound(TPInterNavDat, 2)
                ComboBox0501.Items.Clear()
                PrevNavIx = 0

                If (ArrayNum > -1) And (N >= ArrayNum) Then
                    N = UBound(TPInterNavDat, 2)
                    For I = 0 To N
                        If TPInterNavDat(ArrayNum, I).Index = -1 Then
                            IH = I
                            Exit For
                        End If

                        ComboBox0501.Items.Add(TPInterNavDat(ArrayNum, I).CallSign)
                        If (TPInterNavDat(ArrayNum, I).CallSign = PrevNav.CallSign) And (TPInterNavDat(ArrayNum, I).Identifier = PrevNav.Identifier) And (TPInterNavDat(ArrayNum, I).IntersectionType = PrevNav.IntersectionType) Then
                            PrevNavIx = I
                        End If
                    Next I
                End If
            End If
        End If

        '============================================================
        hTurn = fHTurn + FicTHRprj.Z

        L0 = (fHTurn - PtSOC.Z) / fMissAprPDG

        TurnFixPnt = PointAlongPlane(PtSOC, _ArDir, L0)
        TurnFixPnt.Z = hTurn
        TurnFixPnt.M = _ArDir

        fTASl = IAS2TAS(m_fIAS, hTurn, CurrADHP.ISAtC)
        VTotal = fTASl + CurrADHP.WindSpeed
        Ls = VTotal * arT_TechToleranc.Value * 0.277777777777778

        _InfoFrm.ResetTurnFields()

        _InfoFrm.SetAltitude(hTurn)
        _InfoFrm.SetTAS(fTASl * 0.277777777777778)
        _InfoFrm.SetWindSpeed(depWS.Value)

        Dim Rv As Double
        Dim r0 As Double

        Rv = 6355.0 * System.Math.Tan(GlobalVars.DegToRadValue * fBankAngle) / (GlobalVars.PI * fTASl)
        If (Rv > 3.0) Then Rv = 3.0

        '_InfoFrm.SetRv(Rv)

        If (Rv > 0.0) Then
            r0 = fTASl / (0.02 * GlobalVars.PI * Rv)
            _InfoFrm.SetRadius(r0)
        End If

        E = depWS.Value / Rv '25.0 * 
        _InfoFrm.SetE(E)

        '==================== На абсолютной высоте
        Dim TanGPA As Double
        Dim CoTanZ As Double
        Dim TurnMOC As Double
        Dim CoTanGPA As Double
        Dim ZSurfaceOrigin As Double

        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
        Dim pYPlaneRelation As ESRI.ArcGIS.Geometry.IRelationalOperator


        If OptionButton0401.Checked Or KK Is Nothing Then
            pPolyClone = pClone.Clone

            pTopo = pPolyClone
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If pLine Is Nothing Then pLine = New ESRI.ArcGIS.Geometry.Polyline

            pLine.FromPoint = PointAlongPlane(TurnFixPnt, _ArDir - 90.0, RModel)
            pLine.ToPoint = PointAlongPlane(TurnFixPnt, _ArDir + 90.0, RModel)

            KK = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
            If SideDef(KK.ToPoint, _ArDir, KK.FromPoint) < 0 Then KK.ReverseOrientation()

            'DrawPolyLine(KK, -1, 2)
            'Application.DoEvents()
        End If

        If OptionButton0401.Checked Then
            pFixPoly = pClone.Clone

            pClone = pFixPoly
            pFIXPoly_6 = pClone.Clone


            If CheckBox0402.Checked Then
                pTopo = hOASPlanes(eOAS.CommonPlane).Poly
                On Error GoTo Error0
                pTopo.Cut(pLine, pLeftPoly, pRightPoly)
                pTopo = pLeftPoly
Error0:
                On Error GoTo 0
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                pDistPolygon = pTopo
                pTopo = pPolyClone
            End If

            ptTmp = PointAlongPlane(PtSOC, _ArDir, L0 + Ls)

            pLine.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)
            pLine.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)

            'fTurnDist = ReturnDistanceInMeters(TurnFixPnt, ptLHPrj) * SideDef(ptLHPrj, ArDir - 90.0, TurnFixPnt)
            CutPoly(pFixPoly, pLine, 1)
            K1K1 = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

            If SideDef(K1K1.ToPoint, _ArDir, K1K1.FromPoint) < 0 Then K1K1.ReverseOrientation()
            'DrawPolyLine K1K1, 255, 2
            KKFixMax = K1K1
            '============================================================
            ptTmp = PointAlongPlane(PtSOC, _ArDir, L0)
            pLine.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)
            pLine.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
            CutPoly(pFIXPoly_6, pLine, 1)

            If ComboBox0503.SelectedIndex < 0 Then
                ComboBox0503.SelectedIndex = 0
            Else
                ComboBox0503_SelectedIndexChanged(ComboBox0503, New System.EventArgs())
            End If

            TextBox0507.Tag = TextBox0507.Text
            '=============================================================================
            ZSurfaceOrigin = OASZOrigin
            If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

            TanGPA = System.Math.Tan(DegToRad(ILS.GPAngle))
            CoTanGPA = 1.0 / TanGPA
            CoTanZ = 1.0 / fMissAprPDG

            If CheckBox0401.Checked Then
                TurnMOC = arMA_FinalMOC.Value 'CurrMOC
            Else
                TurnMOC = arMA_InterMOC.Value 'CurrMOC
            End If
            '=============================================================================
            Ix = -1

            'GetIntermObstacleList(ObstacleList, ZNRObstacleList, ZContinued, ptLHPrj, ArDir)

            'pRelation = pFIXPoly_6
            'L = -1

            'K = -1
            'Ix = -1
            'fTmp = -10000.0

            'For I = 0 To M
            '	ZNRObstacleList.Obstacles(I).NIx = -1
            'Next


            'For I = 0 To N
            '	If pRelation.Disjoint(ZNRObstacleList.Parts(I).pPtPrj) Then Continue For
            '	K += 1
            '	ZNRObstacleList.MovePart(K, I)
            '	'ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(I).Owner).NIx = I
            '	If ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(I).Owner).NIx < 0 Then
            '		L += 1
            '		ZNRObstacleList.Obstacles(L) = ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(I).Owner)
            '		ZNRObstacleList.Obstacles(L).PartsNum = 0
            '		ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(I).Owner).NIx = L
            '	End If

            '	ZNRObstacleList.Parts(K).Dist = -ZNRObstacleList.Parts(K).Dist
            '	ZNRObstacleList.Parts(K).ReqOCH = (ZNRObstacleList.Parts(K).Height * CoTanZ + (ZSurfaceOrigin + ZNRObstacleList.Parts(K).Dist)) / (CoTanZ + CoTanGPA) + m_fMOC
            '	ZNRObstacleList.Parts(K).ReqH = ZNRObstacleList.Parts(K).Height + TurnMOC 'CurrMOC
            '	If ZNRObstacleList.Parts(K).ReqH > fTmp Then
            '		fTmp = ZNRObstacleList.Parts(K).ReqH
            '		Ix = K
            '	End If
            '	ZNRObstacleList.Parts(K).Plane = eOAS.NonPrec
            'Next I



            Dim pIntersection As ArcGIS.Geometry.IPolygon
            'Dim pTopo As ArcGIS.Geometry.ITopologicalOperator2
            pIntersection = Nothing
            pTopo = ZContinued
            On Error Resume Next
            pIntersection = pTopo.Intersect(pFIXPoly_6, ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
            On Error GoTo 0

            Ix = -1

            If (Not (pIntersection Is Nothing)) And Not pIntersection.IsEmpty Then
                pTopo = pIntersection
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
                'DrawPolygon(pIntersection, RGB(200, 200, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
                'Application.DoEvents()

                GetIntermObstacleList(ObstacleList, ZNRObstacleList, pIntersection, FicTHRprj, _ArDir)

                L = UBound(ZNRObstacleList.Obstacles)
                K = UBound(ZNRObstacleList.Parts)
                fTmp = -10000.0

                For I = 0 To K
                    ZNRObstacleList.Parts(I).Dist = -ZNRObstacleList.Parts(I).Dist
                    ZNRObstacleList.Parts(I).ReqOCH = (ZNRObstacleList.Parts(I).Height * CoTanZ + (ZSurfaceOrigin + ZNRObstacleList.Parts(I).Dist)) / (CoTanZ + CoTanGPA) + m_fMOC
                    ZNRObstacleList.Parts(I).ReqH = ZNRObstacleList.Parts(I).Height + TurnMOC 'CurrMOC
                    If ZNRObstacleList.Parts(I).ReqH > fTmp Then
                        fTmp = ZNRObstacleList.Parts(I).ReqH
                        Ix = K
                    End If
                    ZNRObstacleList.Parts(I).Plane = eOAS.NonPrec
                Next I

            Else
                'ReDim ZNRObstacleList.Obstacles(-1)
                'ReDim ZNRObstacleList.Parts(-1)
                L = -1
                K = -1
            End If


            pRelation = pFIXPoly_6

            M = UBound(WorkObstacleList.Obstacles)
            N = UBound(WorkObstacleList.Parts)

            'DrawPolygon(pFIXPoly_6, RGB(0, 200, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
            'DrawPolygon(ZContinued, RGB(180, 0, 200), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
            'Application.DoEvents()

            If K >= 0 Then
                ReDim Preserve ZNRObstacleList.Obstacles(M + L + 1)
                ReDim Preserve ZNRObstacleList.Parts(N + K + 1)
            ElseIf M >= 0 Then
                L = -1
                ReDim ZNRObstacleList.Obstacles(M)
                ReDim ZNRObstacleList.Parts(N)
            Else
                ReDim ZNRObstacleList.Obstacles(-1)
                ReDim ZNRObstacleList.Parts(-1)
            End If

            For I = 0 To M
                WorkObstacleList.Obstacles(I).NIx = -1
            Next

            pYPlaneRelation = OASPlanes(eOAS.ZPlane + TurnDir).Poly

            For I = 0 To N
                If pRelation.Disjoint(WorkObstacleList.Parts(I).pPtPrj) Then Continue For

                K = K + 1
                ZNRObstacleList.Parts(K) = WorkObstacleList.Parts(I)

                If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                    L += 1
                    ZNRObstacleList.Obstacles(L) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                    ZNRObstacleList.Obstacles(L).PartsNum = 0
                    ReDim ZNRObstacleList.Obstacles(L).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                    WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = L
                End If

                ZNRObstacleList.Parts(K).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
                ZNRObstacleList.Parts(K).Index = ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum
                ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).Parts(ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum) = K
                ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum += 1

                ZNRObstacleList.Parts(K).Flags = IIf(pYPlaneRelation.Contains(WorkObstacleList.Parts(I).pPtPrj), 1, 0) 'And (WorkObstacleList.Parts(I).hPenet <= 0.0))
                ZNRObstacleList.Parts(K).ReqH = ZNRObstacleList.Parts(K).Height + TurnMOC   'CurrMOC

                Indx = pYPlaneRelation.Contains(ZNRObstacleList.Parts(K).pPtPrj) And (ZNRObstacleList.Parts(K).hPenet <= 0.0)

                If (Not Indx) And (ZNRObstacleList.Parts(K).ReqH > fTmp) Then
                    fTmp = ZNRObstacleList.Parts(K).ReqH
                    Ix = K
                End If
            Next I


            If K >= 0 Then
                ReDim Preserve ZNRObstacleList.Obstacles(L)
                ReDim Preserve ZNRObstacleList.Parts(K)
            Else
                ReDim ZNRObstacleList.Obstacles(-1)
                ReDim ZNRObstacleList.Parts(-1)
            End If


            If Ix >= 0 Then
                TextBox0504.Text = ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(Ix).Owner).UnicalName
                'TextBox0508.Text = CStr(ConvertHeight(ZNRObstacleList(Ix).ReqH, eRoundMode.rmNERAEST))
                TextBox0508.Text = CStr(ConvertHeight(ZNRObstacleList.Parts(Ix).Height, eRoundMode.NEAREST))
            End If

            Sort(ZNRObstacleList, 0)
            PrecReportFrm.FillPage08(ZNRObstacleList)
            pGroupElement.AddElement(DrawPolygon(pFIXPoly_6, RGB(0, 0, 255), , False))
            pGroupElement.AddElement(DrawPointWithText(TurnFixPnt, "TP", WPTColor, False))
            FIXElem = pGroupElement


            For I = 0 To pGroupElement.ElementCount - 1
                pGroupElement.Element(I).Locked = True
                pGraphics.AddElement(pGroupElement.Element(I), 0)
            Next I
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            '        If ButtonControl5State Then
            '        If True Then
            '            pGraphics.AddElement StrTrackElem, 0
            '            StrTrackElem.Locked = True
            '        End If
            '        RefreshCommandBar mTool, 128

        Else            '==================== Над FIX
            M = UBound(PrecisionObstacleList.Obstacles)
            N = UBound(PrecisionObstacleList.Parts)

            For I = 0 To M
                PrecisionObstacleList.Obstacles(I).NIx = -1
            Next

            ReDim ZNRObstacleList.Obstacles(M)
            ReDim ZNRObstacleList.Parts(N)
            K = -1
            L = -1

            Dim NewFullPoly As IPolygon = ReArrangePolygon(pFullPoly, PtSOC, _ArDir)
            ZNR_Poly = CreateBasePoints(NewFullPoly, KK, _ArDir, TurnDir)

            '=================================================================================================================
            pRelation = ZNR_Poly

            For I = 0 To N
                If pRelation.Disjoint(PrecisionObstacleList.Parts(I).pPtPrj) Then Continue For

                K = K + 1
                ZNRObstacleList.Parts(K) = PrecisionObstacleList.Parts(I)

                If PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(I).Owner).NIx < 0 Then
                    L += 1
                    ZNRObstacleList.Obstacles(L) = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(I).Owner)
                    ZNRObstacleList.Obstacles(L).PartsNum = 0
                    ReDim ZNRObstacleList.Obstacles(L).Parts(PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(I).Owner).PartsNum - 1)
                    PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(I).Owner).NIx = L
                End If

                ZNRObstacleList.Parts(K).Owner = PrecisionObstacleList.Obstacles(PrecisionObstacleList.Parts(I).Owner).NIx
                ZNRObstacleList.Parts(K).Index = ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum
                ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).Parts(ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum) = K
                ZNRObstacleList.Obstacles(ZNRObstacleList.Parts(K).Owner).PartsNum += 1

                ZNRObstacleList.Parts(K).Flags = 0 '-CShort(ZNRObstacleList.Parts(I).hPenet <= 0.0)

                ZNRObstacleList.Parts(K).ReqH = ZNRObstacleList.Parts(K).Height + TurnMOC   'CurrMOC
                'Indx = -CShort((pYPlaneRelation.Contains(ZNRObstacleList.Parts(K).pPtPrj)) And (ZNRObstacleList.Parts(K).hPenet <= 0.0))

                'If (Indx = 0) And (ZNRObstacleList.Parts(K).ReqH > fTmp) Then
                '	fTmp = ZNRObstacleList.Parts(K).ReqH
                '	Ix = K
                'End If
            Next I

            If K >= 0 Then
                ReDim Preserve ZNRObstacleList.Obstacles(L)
                ReDim Preserve ZNRObstacleList.Parts(K)
            Else
                ReDim ZNRObstacleList.Obstacles(-1)
                ReDim ZNRObstacleList.Parts(-1)
            End If
            '=================================================================================================================
            'DrawPolygon(ZNR_Poly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
            'Application.DoEvents()

            'fTurnDist = FixDist

            'Sort(ZNRObstacleList, 0)

            PrecReportFrm.FillPage08(ZNRObstacleList)

            TextBox0507.Tag = CStr(ConvertDistance(FixDist, eRoundMode.NEAREST))
            TextBox0507.Text = TextBox0507.Tag
            NextBtn.Enabled = ComboBox0501.Items.Count > 0


            If Not NextBtn.Enabled Then
                _Label0501_18.ForeColor = Color.Red
                _Label0501_18.Text = My.Resources.str20604
            Else
                _Label0501_18.ForeColor = System.Drawing.SystemColors.ControlText
            End If

            If prevCnt < 2 Then
                If ComboBox0501.Items.Count > PrevNavIx Then ComboBox0501.SelectedIndex = PrevNavIx
            End If
        End If

        While ArrivalProfile.PointsNo > ArrivalProfile.MAPtIndex + 1
            ArrivalProfile.RemovePoint()
        End While

        Dim dD As Double
        dD = Point2LineDistancePrj(FicTHRprj, TurnFixPnt, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, TurnFixPnt)
        ArrivalProfile.AddPoint(dD, TurnFixPnt.Z - FicTHRprj.Z, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
    End Sub

#End Region

#Region "Page 6 events"
    Private Sub CheckBox0601_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0601.CheckedChanged
        If Not bFormInitialised Then Return

        ComboBox0601_SelectedIndexChanged(ComboBox0601, New System.EventArgs())
    End Sub

    Private Sub OptionButton0601_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0601.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return

        TextBox0603.ReadOnly = False
        TextBox0603.BackColor = System.Drawing.SystemColors.Window
        ToolTip1.SetToolTip(TextBox0603, "")
        TextBox0603.Tag = "a"
        TurnDirector.TypeCode = eNavaidType.NONE

        TextBox0908.ReadOnly = False
        TextBox0908.BackColor = System.Drawing.SystemColors.Window

        TextBox0901.ReadOnly = False
        TextBox0901.BackColor = System.Drawing.SystemColors.Window

        _Label0601_1.Visible = False
        _Label0601_2.Visible = False
        _Label0601_4.Visible = False
        _Label0601_6.Visible = False
        _Label0601_7.Visible = False
        _Label0601_9.Visible = False
        _Label0601_10.Visible = False
        _Label0601_11.Visible = False
        _Label0601_12.Visible = False
        _Label0601_13.Visible = False

        ComboBox0601.Visible = False
        ComboBox0602.Visible = False
        ComboBox0603.Visible = False
        CheckBox0601.Visible = False

        TextBox0605.Visible = False
        TextBox0604.Visible = False
        TextBox0602.Visible = False
        OptionButton0706.Visible = False

        OptionButton0702.Visible = False
        TextBox0603_Validating(TextBox0603, New System.ComponentModel.CancelEventArgs(True))
    End Sub

    Private Sub OptionButton0602_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0602.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return

        Dim I As Integer
        Dim J As Integer
        Dim N As Integer

        _Label0601_1.Text = My.Resources.str13011  'Verify
        _Label0601_1.Visible = True
        _Label0601_2.Visible = True
        _Label0601_4.Visible = True
        _Label0601_6.Visible = True
        _Label0601_7.Visible = True
        _Label0601_9.Visible = True
        _Label0601_10.Visible = True
        _Label0601_11.Visible = True
        _Label0601_12.Visible = True
        _Label0601_13.Visible = True

        TextBox0603.ReadOnly = False
        TextBox0603.BackColor = System.Drawing.SystemColors.Window
        ToolTip1.SetToolTip(TextBox0603, "")
        TextBox0603.Tag = "a"

        TextBox0908.ReadOnly = False
        TextBox0908.BackColor = System.Drawing.SystemColors.Window

        TextBox0901.ReadOnly = False
        TextBox0901.BackColor = System.Drawing.SystemColors.Window

        ComboBox0601.Visible = True
        ComboBox0602.Visible = True
        ComboBox0603.Visible = True
        ComboBox0603.Enabled = UBound(WPTList) > 0

        CheckBox0601.Visible = False

        TextBox0605.Visible = True
        TextBox0604.Visible = True
        TextBox0602.Visible = True

        OptionButton0702.Visible = True
        OptionButton0705.Visible = True
        OptionButton0706.Visible = False
        '==================================
        ComboBox0601.Items.Clear()
        N = UBound(FixAngl)
        J = -1
        If N >= 0 Then
            ReDim SelectedFixAngl(N)
            For I = 0 To N
                If ReturnDistanceInMeters(TurnFixPnt, FixAngl(I).pPtPrj) < RModel Then
                    J = J + 1
                    SelectedFixAngl(J) = FixAngl(I)
                    ComboBox0601.Items.Add(SelectedFixAngl(J).Name)
                End If
            Next I

            If J >= 0 Then
                ReDim Preserve SelectedFixAngl(J)
                ComboBox0601.Enabled = True
                ComboBox0601.SelectedIndex = 0
            Else
                ComboBox0601.Enabled = False
            End If
        End If
    End Sub

    Private Sub OptionButton0603_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0603.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return

        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        ComboBox0601.Visible = True
        CheckBox0601.Visible = True
        ComboBox0602.Visible = False
        ComboBox0603.Visible = False

        '=====
        _Label0601_1.Text = My.Resources.str13013  'Verify
        _Label0601_1.Visible = True
        _Label0601_2.Visible = False
        _Label0601_4.Visible = True
        _Label0601_6.Visible = False
        _Label0601_9.Visible = False
        _Label0601_7.Visible = False
        _Label0601_10.Visible = False
        _Label0601_11.Visible = False
        _Label0601_12.Visible = False
        _Label0601_13.Visible = False

        TextBox0603.ReadOnly = True
        TextBox0603.BackColor = System.Drawing.SystemColors.Control
        ToolTip1.SetToolTip(TextBox0603, "")
        TextBox0603.Tag = "a"

        TextBox0908.ReadOnly = True
        TextBox0908.BackColor = System.Drawing.SystemColors.Control

        TextBox0901.ReadOnly = True
        TextBox0901.BackColor = System.Drawing.SystemColors.Control

        TextBox0605.Visible = False
        TextBox0604.Visible = False
        TextBox0602.Visible = False
        '==
        OptionButton0702.Visible = False
        OptionButton0705.Visible = False
        OptionButton0706.Visible = True

        OptionButton0702.Checked = False
        OptionButton0705.Checked = False

        _Label0701_1.Visible = True
        TextBox0702.Visible = True
        '==
        ComboBox0601.Items.Clear()
        N = UBound(WPTList)
        J = -1
        If N >= 0 Then
            ReDim SelectedFixAll(N)
            For I = 0 To N
                If (WPTList(I).TypeCode <= eNavaidType.NONE) Or (ReturnDistanceInMeters(TurnFixPnt, WPTList(I).pPtPrj) < RModel) Then
                    J = J + 1
                    SelectedFixAll(J) = WPTList(I)
                    ComboBox0601.Items.Add(SelectedFixAll(J).Name)
                End If
            Next I

            If J >= 0 Then
                ReDim Preserve SelectedFixAll(J)
                ComboBox0601.Enabled = True
                ComboBox0601.SelectedIndex = 0
                '            ComboBox0601_Click
            Else
                ComboBox0601.Enabled = False
            End If
        End If
    End Sub

    Private Sub ComboBox0601_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0601.SelectedIndexChanged
        If Not bFormInitialised Then Return

        If ComboBox0601.Items.Count < 1 Then Return
        If MultiPage1.SelectedIndex < 6 Then Return

        If OptionButton0603.Checked Then
            TurnDirector = SelectedFixAll(ComboBox0601.SelectedIndex)
            'Dim Clone As ESRI.ArcGIS.esriSystem.IClone
            'Clone = TurnDirector.pPtPrj
            'TerFixPnt = Clone.Clone
        ElseIf OptionButton0602.Checked Then
            TurnDirector = SelectedFixAngl(ComboBox0601.SelectedIndex)
        Else
            TurnDirector.TypeCode = eNavaidType.NONE
            Return
        End If

        _Label0601_4.Text = GetNavTypeName(TurnDirector.TypeCode)

        'If TurnWPT.TypeCode >= 0 Then
        '    fTmp = ReturnDistanceInMeters(TurnFixPnt, TurnWPT.pPtPrj)
        '
        '    If fTmp >= RModel Then
        '        msgbox "Navaid out of range."
        '        Return
        '    End If
        'End If

        If OptionButton0603.Checked And ((TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.NDB) Or (TurnDirector.TypeCode = eNavaidType.TACAN)) Then
            CheckBox0601.Enabled = True
        Else
            CheckBox0601.Checked = False
            CheckBox0601.Enabled = False
        End If

        If OptionButton0602.Checked Then
            UpdateIntervals(True)
            If ComboBox0603.SelectedIndex = 0 Then
                ComboBox0603_SelectedIndexChanged(ComboBox0603, New EventArgs())
            Else
                ComboBox0603.SelectedIndex = 0
            End If
        ElseIf OptionButton0603.Checked Then
            UpdateToFix()
        End If
    End Sub

    Private Sub ComboBox0602_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0602.SelectedIndexChanged
        Dim K As Integer
        Dim BoolRes As Boolean

        K = ComboBox0602.SelectedIndex
        If K < 0 Then Return
        If K = CInt(ComboBox0602.Tag) Then Return

        WPt0602 = FixBox0602(K)

        m_OutDir = ReturnAngleInDegrees(TurnDirector.pPtPrj, WPt0602.pPtPrj)
        BoolRes = UpdateToNavCourse(m_OutDir) = -1

        If Not BoolRes Then
            TextBox0603.Tag = CStr(Math.Round(Modulus(Dir2Azt(TurnDirector.pPtPrj, m_OutDir) - TurnDirector.MagVar, 360.0)))
            TextBox0603.Text = TextBox0603.Tag
        End If

        If (Not BoolRes) Or (CInt(ComboBox0602.Tag) = -1) Then
            ComboBox0602.Tag = ComboBox0602.SelectedIndex
        Else
            ComboBox0602.SelectedIndex = ComboBox0602.Tag
        End If
    End Sub

    Private Sub ComboBox0603_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0603.SelectedIndexChanged
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer

        If OptionButton0602.Checked Then
            If (ComboBox0603.SelectedIndex = 0) Then
                TextBox0603.ReadOnly = False
                'TextBox0603.BackColor = System.Drawing.SystemColors.Window

                ComboBox0602.Enabled = False
                TextBox0603.Tag = "a"
                TextBox0603_Validating(TextBox0603, New CancelEventArgs())
            Else
                N = UBound(WPTList)
                If N >= 0 Then
                    Dim OldWPT As WPT_FIXType
                    Dim k As Integer
                    k = ComboBox0602.SelectedIndex
                    If k >= 0 Then
                        OldWPT.Identifier = WPTList(k).Identifier
                    End If

                    TextBox0603.ReadOnly = True
                    'TextBox0603.BackColor = System.Drawing.SystemColors.Control
                    ComboBox0602.Enabled = True

                    ComboBox0602.Tag = "-1"
                    ComboBox0602.Items.Clear()

                    ReDim FixBox0602(N)
                    J = -1
                    k = 0
                    For I = 0 To N
                        If WPTList(I).Name <> ComboBox0601.Text Then
                            J = J + 1
                            FixBox0602(J) = WPTList(I)
                            ComboBox0602.Items.Add(WPTList(I).Name)
                            If OldWPT.Identifier = WPTList(k).Identifier Then k = J
                        End If
                    Next I

                    If J >= 0 Then
                        ReDim Preserve FixBox0602(J)
                        ComboBox0602.SelectedIndex = k
                    Else
                        ComboBox0603.Enabled = False
                        ComboBox0603.SelectedIndex = 0
                    End If
                Else
                    ComboBox0603.Enabled = False
                    ComboBox0603.SelectedIndex = 0
                End If
            End If
        End If
    End Sub

    Private Sub TextBox0603_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0603.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0603_Validating(TextBox0603, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(eventArgs.KeyChar)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0603_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0603.Validating
        If (Not IsNumeric(TextBox0603.Text)) And IsNumeric(TextBox0603.Tag) Then TextBox0603.Text = TextBox0603.Tag
        If (MultiPage1.SelectedIndex < 5) Or (TextBox0603.Tag = TextBox0603.Text) Or (Not IsNumeric(TextBox0603.Text)) Then Return

        If OptionButton0601.Checked Then
            m_OutDir = Azt2Dir(ToGeo(TurnFixPnt), CDbl(TextBox0603.Text) + CurrADHP.MagVar)
        Else
            m_OutDir = Azt2Dir(TurnDirector.pPtGeo, CDbl(TextBox0603.Text) + TurnDirector.MagVar)
        End If

        If OptionButton0602.Checked Then
            UpdateToNavCourse(m_OutDir)
        ElseIf OptionButton0601.Checked Then
            UpdateToCourse(m_OutDir)
        End If

        TextBox0603.Tag = TextBox0603.Text
    End Sub

    Private Sub TextBox0604_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0604.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0604_Validating(TextBox0604, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxInteger(eventArgs.KeyChar)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0604_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0604.Validating
        If Not IsNumeric(TextBox0604.Text) Then Return
        Dim fTmp As Double

        fTmp = CDbl(TextBox0604.Text)

        If fTmp < 5.0 Then
            TextBox0604.Text = "2"
        ElseIf fTmp > 75.0 Then       'arImMaxIntercept.Value
            TextBox0604.Text = arImMaxIntercept.Value.ToString()
        End If

        If TextBox0604.Text <> TextBox0604.Tag Then
            UpdateIntervals((Not OptionButton0602.Checked) Or (ComboBox0603.SelectedIndex = 0))
            If OptionButton0602.Checked And (ComboBox0603.SelectedIndex = 1) Then
                ComboBox0602.Tag = "-1"
                ComboBox0602_SelectedIndexChanged(ComboBox0602, New EventArgs())
            End If

            TextBox0604.Tag = TextBox0604.Text
        End If
    End Sub

    Private Sub TextBox0605_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0605.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0605_Validating(TextBox0605, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0605.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0605_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBox0605.Validating
        If Not IsNumeric(TextBox0605.Text) Then Return

        If TextBox0605.Text <> TextBox0605.Tag Then
            TextBox0605.Tag = TextBox0605.Text
            If OptionButton0602.Checked Then
                UpdateIntervals(True)
            End If
        End If
    End Sub

#End Region

#Region "Page 7 events"
    Private Sub OptionButton0701_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0701.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

    Private Sub OptionButton0702_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0702.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

    Private Sub OptionButton0703_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0703.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

    Private Sub OptionButton0704_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0704.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

    Private Sub OptionButton0705_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0705.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

    Private Sub OptionButton0706_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0706.CheckedChanged
        If Not bFormInitialised Then Return
        If Not eventSender.Checked Then Return
        If CurrPage <> 7 Then Return

        ApplayOptions()
    End Sub

#End Region

#Region "Page 8 events"
    Private Sub CheckBox0801_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0801.CheckedChanged
        If Not bFormInitialised Then Return

        CheckBox0804.Checked = CheckBox0801.Checked
        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

    Private Sub CheckBox0802_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0802.CheckedChanged
        If Not bFormInitialised Then Return

        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

    Private Sub CheckBox0803_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0803.CheckedChanged
        If Not bFormInitialised Then Return

        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

    Private Sub CheckBox0804_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0804.CheckedChanged
        If Not bFormInitialised Then Return

        CheckBox0801.Checked = CheckBox0804.Checked
        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

    Private Sub CheckBox0805_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0805.CheckedChanged
        If Not bFormInitialised Then Return

        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

    Private Sub CheckBox0806_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckBox0806.CheckedChanged
        If Not bFormInitialised Then Return

        If CurrPage <> 8 Then Return
        SecondArea(TurnDir, BaseArea)
    End Sub

#End Region

#Region "Page 9 events"

    Private Sub OptionButton0901_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptionButton0901.CheckedChanged, OptionButton0902.CheckedChanged
        If Not bFormInitialised Then Return
        If sender.Checked And sender.Enabled Then
            '        TextBox0909.Tag = ""
            '        TextBox0909_Validate True
            ComboBox0902_SelectedIndexChanged(ComboBox0902, New System.EventArgs())
        End If
    End Sub

    Private Sub CheckBox0901_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0901.CheckedChanged
        If Not bFormInitialised Then Return

        OkBtn.Enabled = Not CheckBox0901.Checked

        _Label0901_18.Enabled = Not CheckBox0901.Checked
        NextBtn.Enabled = CheckBox0901.Checked
    End Sub

    Private Sub ComboBox0901_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0901.SelectedIndexChanged
        Dim pInterceptPt As ArcGIS.Geometry.IPoint
        Dim fDist As Double
        Dim fDir As Double
        Dim fAzt As Double
        Dim I As Integer
        Dim K As Integer
        Dim N As Integer

        I = ComboBox0901.SelectedIndex - 1
        If I < 0 Then
            TextBox0909.ReadOnly = False
            'TextBox0909.BackColor = System.Drawing.SystemColors.Window
        Else
            If Not OptionButton0602.Checked Then Return

            TextBox0909.ReadOnly = True
            'TextBox0909.BackColor = System.Drawing.SystemColors.ButtonFace

            K = ComboBox0902.SelectedIndex
            If K < 0 Then Return

            N = UBound(TerInterNavDat)
            If N < 0 Then Return

            _Label0901_24.Text = GetNavTypeName(FixBox0901(I).TypeCode)

            pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

            If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
                fDist = ReturnDistanceInMeters(TerInterNavDat(K).pPtPrj, FixBox0901(I).pPtPrj)
                TextBox0909.Text = CStr(ConvertDistance(fDist - TerInterNavDat(K).Disp, 0))
                If SideDef(TerInterNavDat(K).pPtPrj, pInterceptPt.M + 90.0, FixBox0901(I).pPtPrj) < 0 Then
                    OptionButton0901.Checked = True
                Else
                    OptionButton0902.Checked = True
                End If
            Else
                fDir = ReturnAngleInDegrees(TerInterNavDat(K).pPtPrj, FixBox0901(I).pPtPrj)
                fAzt = Dir2Azt(TerInterNavDat(K).pPtPrj, fDir)
                If TerInterNavDat(K).TypeCode = eNavaidType.NDB Then
                    TextBox0909.Text = CStr(Modulus(fAzt + 180.0 - TerInterNavDat(K).MagVar, 360.0))
                Else
                    TextBox0909.Text = CStr(Modulus(fAzt - TerInterNavDat(K).MagVar, 360.0))
                End If
            End If
        End If

        TextBox0909.Tag = "-"
        TextBox0909_Validating(TextBox0909, New CancelEventArgs())
    End Sub

    Private Sub ComboBox0902_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0902.SelectedIndexChanged
        If Not bFormInitialised Then Return
        Dim I As Integer
        Dim N As Integer
        Dim K As Integer
        Dim Kmin As Integer
        Dim Kmax As Integer
        Dim tipStr As String

        If Not OptionButton0602.Checked Then Return
        K = ComboBox0902.SelectedIndex
        If K < 0 Then Return

        N = UBound(TerInterNavDat)

        If N < 0 Then
            Return
        End If

        _Label0901_15.Text = GetNavTypeName(TerInterNavDat(K).TypeCode)

        If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
            TextBox0909.Visible = True

            _Label0901_17.Text = DistanceConverter(DistanceUnit).Unit

            N = UBound(TerInterNavDat(K).ValMin)

            OptionButton0901.Enabled = N > 0
            OptionButton0902.Enabled = N > 0

            If OptionButton0901.Checked Or (N = 0) Then
                TextBox0909.Text = CStr(ConvertDistance(TerInterNavDat(K).ValMin(0) - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
            Else
                TextBox0909.Text = CStr(ConvertDistance(TerInterNavDat(K).ValMin(1) - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
            End If

            If N = 0 Then
                If TerInterNavDat(K).ValCnt > 0 Then
                    OptionButton0901.Checked = True
                Else
                    OptionButton0902.Checked = True
                End If
            End If

            _Label0901_16.Text = My.Resources.str00229
            tipStr = My.Resources.str10514

            For I = 0 To N
                tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(TerInterNavDat(K).ValMin(I) - TerInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TerInterNavDat(K).ValMax(I) - TerInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
                If I < N Then
                    tipStr = My.Resources.str00230 + " - " + tipStr + vbCrLf + My.Resources.str00231 + " - "
                End If
            Next I
            '===
        Else
            OptionButton0901.Enabled = False
            OptionButton0902.Enabled = False

            TextBox0909.Visible = TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid
            If TerInterNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
                TextBox0909.Text = ""
                tipStr = "FIX " + My.Resources.str00106
                _Label0901_16.Text = tipStr
                _Label0901_17.Text = ""
            Else
                _Label0901_17.Text = "°"
                If TerInterNavDat(K).TypeCode = eNavaidType.VOR Then
                    _Label0901_16.Text = My.Resources.str00227
                    Kmax = Modulus(TerInterNavDat(K).ValMax(0) - TerInterNavDat(K).MagVar, 360.0)
                    Kmin = Modulus(TerInterNavDat(K).ValMin(0) - TerInterNavDat(K).MagVar, 360.0)
                Else
                    _Label0901_16.Text = My.Resources.str00228
                    Kmax = Modulus(TerInterNavDat(K).ValMax(0) + 180.0 - TerInterNavDat(K).MagVar, 360.0)
                    Kmin = Modulus(TerInterNavDat(K).ValMin(0) + 180.0 - TerInterNavDat(K).MagVar, 360.0)
                End If
                tipStr = My.Resources.str00220 + My.Resources.str00221

                If TerInterNavDat(K).ValCnt > 0 Then
                    TextBox0909.Text = CStr(Kmin)
                Else
                    TextBox0909.Text = CStr(Kmax)
                End If
                tipStr = tipStr + CStr(Kmin) + " °" + My.Resources.str00222 + CStr(Kmax) + " °"
            End If
        End If

        If TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
            tipStr = Replace(tipStr, vbCrLf, "   ")
            ToolTip1.SetToolTip(TextBox0909, tipStr)
        End If

        FillCombo0901()
        'TextBox0909.Tag = "-"
        'TextBox0909_Validating(TextBox0909, New System.ComponentModel.CancelEventArgs(True))
    End Sub

    Private Sub ComboBox0903_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0903.SelectedIndexChanged
        If ComboBox0903.SelectedIndex = 0 Then
            TextBox0906.Text = CStr(ConvertHeight(fTA_OCH, eRoundMode.NEAREST))
        Else
            TextBox0906.Text = CStr(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST))
        End If
    End Sub

    Private Sub TextBox0901_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0901.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0901_Validating(TextBox0901, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0901.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0901_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0901.Validating
        Dim fVal As Double
        Dim fTmp As Double

        If Not Double.TryParse(TextBox0901.Text, fVal) Then fVal = -1.0

        fVal = DeConvertDistance(fVal)
        fTmp = fVal

        If fVal > TurnAreaMaxd0 Then fVal = TurnAreaMaxd0
        If fVal < arBufferMSA.Value Then fVal = arBufferMSA.Value
        If fTmp <> fVal Then TextBox0901.Text = ConvertDistance(fVal, eRoundMode.NEAREST).ToString()

        TerminationOCH()
    End Sub

    Private Sub TextBox0908_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0908.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0908_Validating(TextBox0908, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0908.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0908_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0908.Validating
        Dim MinVal As Double
        Dim MaxVal As Double
        Dim NevVal As Double
        Dim fTmp As Double

        If Not IsNumeric(TextBox0908.Text) Then Return
        MinVal = CDbl(TextBox0904.Text)
        MaxVal = CDbl(TextBox0902.Text)
        NevVal = CDbl(TextBox0908.Text)
        fTmp = NevVal

        If NevVal < MinVal Then NevVal = MinVal
        If NevVal > MaxVal Then NevVal = MaxVal

        If fTmp <> NevVal Then
            TextBox0908.Text = CStr(NevVal)
        End If
    End Sub

    Private Sub TextBox0909_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0909.KeyPress
        If eventArgs.KeyChar = Chr(13) Then
            TextBox0909_Validating(TextBox0909, New System.ComponentModel.CancelEventArgs())
        Else
            TextBoxFloat(eventArgs.KeyChar, TextBox0909.Text)
        End If

        If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
    End Sub

    Private Sub TextBox0909_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0909.Validating
        Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection

        Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement

        Dim Clone As ESRI.ArcGIS.esriSystem.IClone

        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

        Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

        Dim InterToler As Double
        Dim TrackToler As Double
        Dim hFix As Double
        Dim fDirl As Double
        Dim fTmp As Double
        Dim fDis As Double
        Dim dMin As Double
        Dim dMax As Double
        Dim hKK As Double
        Dim d0 As Double
        Dim D As Double
        Dim I As Integer
        Dim N As Integer
        Dim K As Integer
        Dim pInterceptPt As ESRI.ArcGIS.Geometry.IPoint
        Dim TurnNav As NavaidData

        If Not OptionButton0602.Checked Then Return

        K = ComboBox0902.SelectedIndex
        If K < 0 Then Return

        If TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
            If TextBox0909.Tag = TextBox0909.Text Then Return
            If IsNumeric(TextBox0909.Text) Then
                fDirl = CDbl(TextBox0909.Text)
            Else 'If (TerInterNavDat(K).ValCnt <> -2) Or (TerInterNavDat(K).TypeCode = 1) Then
                Return
            End If
        End If

        On Error Resume Next
        If Not TerminationFIXElem Is Nothing Then
            pGroupElement = TerminationFIXElem
            For I = 0 To pGroupElement.ElementCount - 1
                If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
            Next I
        End If

        pGroupElement = New ESRI.ArcGIS.Carto.GroupElement

        pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

        On Error GoTo 0

        If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
            fDirl = DeConvertDistance(fDirl) + TerInterNavDat(K).Disp
            fTmp = fDirl

            N = UBound(TerInterNavDat(K).ValMin)

            If OptionButton0901.Checked Or (N = 0) Then
                If fDirl < TerInterNavDat(K).ValMin(0) Then
                    fDirl = TerInterNavDat(K).ValMin(0)
                ElseIf fDirl > TerInterNavDat(K).ValMax(0) Then
                    fDirl = TerInterNavDat(K).ValMax(0)
                End If
            Else
                If fDirl < TerInterNavDat(K).ValMin(1) Then
                    fDirl = TerInterNavDat(K).ValMin(1)
                ElseIf fDirl > TerInterNavDat(K).ValMax(1) Then
                    fDirl = TerInterNavDat(K).ValMax(1)
                End If
            End If

            If fTmp <> fDirl Then
                TextBox0909.Text = CStr(ConvertDistance(fDirl - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
            End If

            If fDirl < 1 Then fDirl = 1

            If (TerInterNavDat(K).ValCnt < 0) Or (OptionButton0901.Enabled And OptionButton0901.Checked) Then
                CircleVectorIntersect(TerInterNavDat(K).pPtPrj, fDirl, pInterceptPt, pInterceptPt.M + 180, TerFixPnt)
            Else
                CircleVectorIntersect(TerInterNavDat(K).pPtPrj, fDirl, pInterceptPt, (pInterceptPt.M), TerFixPnt)
            End If
        ElseIf TerInterNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
            Clone = TerInterNavDat(K).pPtPrj
            TerFixPnt = Clone.Clone
        Else
            fDirl = fDirl + TerInterNavDat(K).MagVar
            If (TerInterNavDat(K).TypeCode = eNavaidType.VOR) Or (TerInterNavDat(K).TypeCode = eNavaidType.TACAN) Then
                InterToler = VOR.IntersectingTolerance
            ElseIf (TerInterNavDat(K).TypeCode = eNavaidType.NDB) Then
                InterToler = NDB.IntersectingTolerance
                fDirl = fDirl - 180.0
            ElseIf (TerInterNavDat(K).TypeCode = eNavaidType.LLZ) Then
                InterToler = LLZ.IntersectingTolerance
            End If

            fTmp = fDirl

            If Not AngleInSector(fDirl, TerInterNavDat(K).ValMin(0), TerInterNavDat(K).ValMax(0)) Then
                If SubtractAngles(fDirl, TerInterNavDat(K).ValMin(0)) < SubtractAngles(fDirl, TerInterNavDat(K).ValMax(0)) Then
                    fDirl = TerInterNavDat(K).ValMin(0)
                Else
                    fDirl = TerInterNavDat(K).ValMax(0)
                End If
            End If

            If fTmp <> fDirl Then
                If (TerInterNavDat(K).TypeCode = eNavaidType.NDB) Then
                    TextBox0909.Text = CStr(System.Math.Round(Modulus(fDirl + 180.0 - TerInterNavDat(K).MagVar, 360.0)))
                Else
                    TextBox0909.Text = CStr(System.Math.Round(Modulus(fDirl - TerInterNavDat(K).MagVar, 360.0)))
                End If
            End If

            fDirl = Azt2Dir(TerInterNavDat(K).pPtGeo, fDirl)

            TerFixPnt = New ESRI.ArcGIS.Geometry.Point
            pConstruct = TerFixPnt

            pConstruct.ConstructAngleIntersection(TerInterNavDat(K).pPtPrj, DegToRad(fDirl), pInterceptPt, DegToRad(pInterceptPt.M))
        End If

        TerFixPnt.M = pInterceptPt.M
        TerminationOCH()

        If OptionButton0401.Checked Then
            pProxi = ZNR_Poly
        Else
            pProxi = KK
        End If

        fDis = pProxi.ReturnDistance(TerFixPnt)
        hKK = DeConvertHeight(CDbl(TextBox0903.Text))
        TerFixPnt.Z = hKK + fDis * fMissAprPDG

        '************************************************************************
        Dim pIZ As IZ
        Dim pIZAware As IZAware

        mPoly.ToPoint = TerFixPnt

        pIZAware = mPoly
        pIZAware.ZAware = True

        pIZ = mPoly
        pIZ.CalculateNonSimpleZs()
        '************************************************************************

        Select Case TerInterNavDat(K).IntersectionType
            Case eIntersectionType.ByDistance, eIntersectionType.RadarFIX
                hFix = TerFixPnt.Z - TerInterNavDat(K).pPtPrj.Z

                d0 = System.Math.Sqrt(fDirl * fDirl + hFix * hFix) * DME.ErrorScalingUp + DME.MinimalError

                D = fDirl + d0
                pSect0 = CreatePrjCircle(TerInterNavDat(K).pPtPrj, D)

                pCutter = New ESRI.ArcGIS.Geometry.Polyline
                pCutter.FromPoint = PointAlongPlane(TerInterNavDat(K).pPtPrj, pInterceptPt.M + 90.0, D + D)
                pCutter.ToPoint = PointAlongPlane(TerInterNavDat(K).pPtPrj, pInterceptPt.M - 90.0, D + D)

                D = fDirl - d0
                pSect1 = CreatePrjCircle(TerInterNavDat(K).pPtPrj, D)

                pTopo = pSect0
                pPoly = pTopo.Difference(pSect1)

                pTopo = pPoly
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                If SideDef(pCutter.FromPoint, pInterceptPt.M + 180, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

                If (TerInterNavDat(K).ValCnt < 0) Or (OptionButton0901.Enabled And OptionButton0901.Checked) Then
                    pTopo.Cut(pCutter, pSect1, pSect0)
                Else
                    pTopo.Cut(pCutter, pSect0, pSect1)
                End If

                pGroupElement.AddElement(DrawPolygon(pSect0, RGB(0, 0, 255), , False))
            Case eIntersectionType.ByAngle
                fDis = TerInterNavDat(K).Range

                pSect0 = New ESRI.ArcGIS.Geometry.Polyline
                pt1 = PointAlongPlane(TerInterNavDat(K).pPtPrj, fDirl + InterToler, fDis)
                pt2 = PointAlongPlane(TerInterNavDat(K).pPtPrj, fDirl - InterToler, fDis)

                pSect0.AddPoint(pt1)
                pSect0.AddPoint(TerInterNavDat(K).pPtPrj)
                pSect0.AddPoint(pt2)

                pGroupElement.AddElement(DrawPolyLine(pSect0, RGB(0, 0, 255), 1, False))
            Case eIntersectionType.OnNavaid
                If TerInterNavDat(K).TypeCode = eNavaidType.NDB Then
                    NDBFIXTolerArea(TerInterNavDat(K).pPtPrj, pInterceptPt.M, TerFixPnt.Z, pSect0)
                Else
                    VORFIXTolerArea(TerInterNavDat(K).pPtPrj, pInterceptPt.M, TerFixPnt.Z, pSect0)
                End If

                pTopo = pSect0
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()

                pGroupElement.AddElement(DrawPolygon(pSect0, RGB(0, 0, 255), , False))
        End Select

        pTopo = pSect0
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        Clone = Prim

        TurnNav = WPT_FIXToNavaid(TurnDirector)

        If TurnNav.TypeCode > eNavaidType.NONE Then
            If (TurnNav.TypeCode = eNavaidType.VOR) Or (TurnNav.TypeCode = eNavaidType.TACAN) Then
                TrackToler = VOR.TrackingTolerance
            ElseIf TurnNav.TypeCode = eNavaidType.NDB Then
                TrackToler = NDB.TrackingTolerance
            ElseIf TurnNav.TypeCode = eNavaidType.LLZ Then
                TrackToler = LLZ.TrackingTolerance
            End If

            pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
            pPolyClone.AddPoint(TurnNav.pPtPrj)
            pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180 - TrackToler, 3.0 * RModel))
            pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180 + TrackToler, 3.0 * RModel))
            pPolyClone.AddPoint(TurnNav.pPtPrj)
            pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180 - TrackToler + 180.0, 3.0 * RModel))
            pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180 + TrackToler + 180.0, 3.0 * RModel))
            pPolyClone.AddPoint(TurnNav.pPtPrj)
            '    Set pFIXPoly__ = Clone.Clone
        Else
            '    Set Clone = pPolygon
            '    Set pPolygon1 = Clone.Clone
            pPolyClone = Clone.Clone
        End If

        pTermFIXTolerArea = Clone.Clone

        pTopo = pPolyClone
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

        If (pLine.PointCount = 0) And (TerInterNavDat(K).IntersectionType <> eIntersectionType.ByAngle) Then
            pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
        End If

        dMax = -RModel
        dMin = RModel

        For I = 0 To pLine.PointCount - 1
            D = Point2LineDistancePrj(pInterceptPt, pLine.Point(I), pInterceptPt.M - 90.0) * SideDef(pInterceptPt, pInterceptPt.M - 90.0, pLine.Point(I))
            If D < dMin Then
                dMin = D
                pt1 = pLine.Point(I)
            End If
            If D > dMax Then
                dMax = D
                pt2 = pLine.Point(I)
            End If
        Next

        pLine = New ESRI.ArcGIS.Geometry.Polyline

        pLine.AddPoint(PointAlongPlane(pt1, pInterceptPt.M - 90.0, RModel))
        pLine.AddPoint(PointAlongPlane(pt1, pInterceptPt.M + 90.0, RModel))

        CutPoly(pTermFIXTolerArea, pLine, 1)

        pLine.RemovePoints(0, pLine.PointCount)
        pLine.AddPoint(PointAlongPlane(pt2, pInterceptPt.M - 90.0, RModel))
        pLine.AddPoint(PointAlongPlane(pt2, pInterceptPt.M + 90.0, RModel))

        CutPoly(pTermFIXTolerArea, pLine, -1)
        '=============================================
        pGroupElement.AddElement(DrawPolygon(pTermFIXTolerArea, 0, , False))
        pGroupElement.AddElement(DrawPointWithText(TerFixPnt, "TerPt", WPTColor, False))
        TerminationFIXElem = pGroupElement

        On Error Resume Next
        '        If ButtonControl8State Then
        For I = 0 To pGroupElement.ElementCount - 1
            pGraphics.AddElement(pGroupElement.Element(I), 0)
            pGroupElement.Element(I).Locked = True
        Next I
        '        End If

        '        If ButtonControl5State Then
        '            pGraphics.AddElement StrTrackElem, 0
        '            StrTrackElem.Locked = True
        '        End If

        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        '        RefreshCommandBar mTool, 128
        On Error GoTo 0
        '==========================================================================
        TextBox0909.Tag = TextBox0909.Text
    End Sub

#End Region

    Private Function CreatePlane15(ByRef NavDat As NavaidData, ByRef Dist As Double) As D3DPolygone
        Dim Hinter As Double
        Dim Tan15 As Double
        Dim hFix As Double
        Dim fDis As Double
        Dim d0 As Double
        Dim D As Double

        Dim pLeftLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pRightLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim pTrackPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pSect0 As ESRI.ArcGIS.Geometry.IPolygon
        Dim pSect1 As ESRI.ArcGIS.Geometry.IPolygon
        Dim pPtNAV As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

        Dim Side As Integer

        pPtNAV = NavDat.pPtPrj
        hFix = ptFAP.Z + FicTHRprj.Z

        pCutter = New ESRI.ArcGIS.Geometry.Polyline

        If NavDat.IntersectionType = eIntersectionType.OnNavaid Then
            If NavDat.TypeCode = eNavaidType.VOR Then
                VORFIXTolerArea(pPtNAV, _ArDir, hFix, pTmpPoly)
            Else
                NDBFIXTolerArea(pPtNAV, _ArDir, hFix, pTmpPoly)
            End If
        Else
            Side = SideDef(pPtNAV, _ArDir + 90.0, ptFAP)

            fDis = ReturnDistanceInMeters(pPtNAV, ptFAP)
            d0 = System.Math.Sqrt(fDis * fDis + (hFix - pPtNAV.Z) * (hFix - pPtNAV.Z))
            'FAPRho = d0

            d0 = d0 * DME.ErrorScalingUp + DME.MinimalError

            D = fDis + d0
            pSect0 = CreatePrjCircle(pPtNAV, D)

            D = fDis - d0
            pSect1 = CreatePrjCircle(pPtNAV, D)

            pTopo = pSect0
            pTmpPoly = pTopo.Difference(pSect1)

            pCutter.FromPoint = PointAlongPlane(pPtNAV, _ArDir - 90.0, MaxModelRadius + 10 * (fDis + fDis))
            pCutter.ToPoint = PointAlongPlane(pPtNAV, _ArDir + 90.0, MaxModelRadius + 10 * (fDis + fDis))

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If Side > 0 Then
                pTopo.Cut(pCutter, pSect1, pSect0)
            Else
                pTopo.Cut(pCutter, pSect0, pSect1)
            End If

            pTopo = pSect0
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pTrackPoly = New ESRI.ArcGIS.Geometry.Polygon
            pTrackPoly.AddPoint(ILS.pPtPrj)

            pTrackPoly.AddPoint(PointAlongPlane(ILS.pPtPrj, _ArDir - LLZ.TrackingTolerance + 180.0, 3.0 * MaxModelRadius))
            pTrackPoly.AddPoint(PointAlongPlane(ILS.pPtPrj, _ArDir + LLZ.TrackingTolerance + 180.0, 3.0 * MaxModelRadius))

            pTopo = pTrackPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pTmpPoly = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
        End If
        '================================================================
        pTopo = pTmpPoly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pFAPTolerArea = pTmpPoly
        'DrawPolygon pFAPTolerArea, 255

        ptTmp = PointAlongPlane(ptFAP, _ArDir + 180.0, 100000.0)
        pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, MaxModelRadius)
        pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, MaxModelRadius)

        pProxi = pCutter
        fDis = 100000.0 - pProxi.ReturnDistance(pTmpPoly)

        If (NavDat.IntersectionType = eIntersectionType.ByDistance) Or (NavDat.IntersectionType = eIntersectionType.RadarFIX) Then
            Dist = ReturnDistanceInMeters(FicTHRprj, ptFAP)
        Else
            Dist = ReturnDistanceInMeters(FicTHRprj, pPtNAV)
        End If

        FAPEarlierToler = Dist + fDis
        FAP15FIX = pTmpPoly
        '================================================================

        Tan15 = arFixMaxIgnorGrd.Value

        CreatePlane15.Poly = New ESRI.ArcGIS.Geometry.Polygon
        ptTmp = PointAlongPlane(pPtNAV, _ArDir + 180.0, fDis)
        ptTmp.Z = FAPDist2hFAP(Dist) - 150.0

        CreatePlane15.Plane.pPt = ptTmp
        CreatePlane15.Plane.B = 0.0
        CreatePlane15.Plane.C = -1.0
        CreatePlane15.Plane.D = (ptTmp.Z - Tan15 * FAPEarlierToler)
        CreatePlane15.Plane.A = Tan15

        Hinter = Det(CreatePlane15.Plane.A, CreatePlane15.Plane.D, wOASPlanes(eOAS.WPlane).Plane.A, wOASPlanes(eOAS.WPlane).Plane.D) / (CreatePlane15.Plane.A - wOASPlanes(eOAS.WPlane).Plane.A)

        pLeftLine = IntersectPlanes(CreatePlane15.Plane, wOASPlanes(eOAS.XlPlane).Plane, ptTmp.Z, Hinter)
        pRightLine = IntersectPlanes(CreatePlane15.Plane, wOASPlanes(eOAS.XrPlane).Plane, ptTmp.Z, Hinter)

        pTransform = pLeftLine
        pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
        pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

        pTransform = pRightLine
        pTransform.Move(FicTHRprj.X, FicTHRprj.Y)
        pTransform.Rotate(FicTHRprj, DegToRad(_ArDir + 180.0))

        CreatePlane15.Poly.AddPoint(pLeftLine.FromPoint)
        CreatePlane15.Poly.AddPoint(pLeftLine.ToPoint)
        CreatePlane15.Poly.AddPoint(pRightLine.ToPoint)
        CreatePlane15.Poly.AddPoint(pRightLine.FromPoint)
        CreatePlane15.Poly.AddPoint(pLeftLine.FromPoint)

        pTopo = CreatePlane15.Poly
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
    End Function

    Private Sub FillCombo0901()
        Dim I As Integer
        Dim J As Integer
        Dim K As Integer
        Dim N As Integer
        Dim nN As Integer

        Dim iSide As Integer
        Dim fDist As Double
        Dim fDir As Double
        Dim fAzt As Double
        Dim pInterceptPt As ArcGIS.Geometry.IPoint

        ComboBox0901.Items.Clear()
        ComboBox0901.Items.Add("Create new")

        If Not OptionButton0602.Checked Then Return
        K = ComboBox0902.SelectedIndex
        If K < 0 Then Return

        pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

        N = UBound(WPTList)
        ReDim FixBox0901(N)

        J = -1

        For I = 0 To N
            If SideDef(pInterceptPt, pInterceptPt.M + 90, WPTList(I).pPtPrj) > 0 Then
                fDir = ReturnAngleInDegrees(pInterceptPt, WPTList(I).pPtPrj)

                If (SubtractAngles(fDir, pInterceptPt.M) < 0.5) Then
                    If (fDist < RModel) Then
                        If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
                            fDist = ReturnDistanceInMeters(TerInterNavDat(K).pPtPrj, WPTList(I).pPtPrj)
                            iSide = SideDef(TerInterNavDat(K).pPtPrj, pInterceptPt.M + 90, WPTList(I).pPtPrj)
                            nN = UBound(TerInterNavDat(K).ValMin)

                            If (iSide < 0) Or (nN = 0) Then
                                If (fDist >= TerInterNavDat(K).ValMin(0)) And (fDist < TerInterNavDat(K).ValMax(0)) Then
                                    J = J + 1
                                    FixBox0901(J) = WPTList(I)
                                    ComboBox0901.Items.Add(FixBox0901(J).Name)
                                End If
                            ElseIf (fDist >= TerInterNavDat(K).ValMin(1)) And (fDist < TerInterNavDat(K).ValMax(1)) Then
                                J = J + 1
                                FixBox0901(J) = WPTList(I)
                                ComboBox0901.Items.Add(FixBox0901(J).Name)
                            End If
                        ElseIf TerInterNavDat(K).IntersectionType = eIntersectionType.ByAngle Then
                            fDir = ReturnAngleInDegrees(TerInterNavDat(K).pPtPrj, WPTList(I).pPtPrj)
                            fAzt = Dir2Azt(TerInterNavDat(K).pPtPrj, fDir)

                            If AngleInSector(fDir, TerInterNavDat(K).ValMin(0), TerInterNavDat(K).ValMax(0)) Then
                                J = J + 1
                                FixBox0901(J) = WPTList(I)
                                ComboBox0901.Items.Add(FixBox0901(J).Name)
                            End If
                        End If
                    End If
                End If
            End If
        Next I

        ComboBox0901.SelectedIndex = 0
    End Sub

    Private Sub ExcludeBtn_Click(sender As System.Object, e As System.EventArgs) Handles ExcludeBtn.Click
        Dim I As Integer
        Dim K As Integer
        Dim L As Integer
        Dim M As Integer
        Dim N As Integer
        Dim Side As Integer
        Dim Dist As Double
        Dim Dist1 As Double
        Dim LocalObstacleList As ObstacleContainer

        M = UBound(WorkObstacleList.Obstacles)
        N = UBound(WorkObstacleList.Parts)

        ReDim LocalObstacleList.Obstacles(M)
        ReDim LocalObstacleList.Parts(N)

        For I = 0 To M
            WorkObstacleList.Obstacles(I).NIx = -1
            'WorkObstacleList.Obstacles(I).IgnoredByUser = False
            WorkObstacleList.Obstacles(I).Index = I
        Next

        K = -1
        L = -1
        For I = 0 To N
            Side = SideDef(FicTHRprj, _ArDir + 90.0, WorkObstacleList.Parts(I).pPtPrj)
            Dist = Point2LineDistancePrj(WorkObstacleList.Parts(I).pPtPrj, FicTHRprj, _ArDir + 90.0)
            Dist1 = Point2LineDistancePrj(WorkObstacleList.Parts(I).pPtPrj, FicTHRprj, _ArDir)

            'If (Side = 1) And (Dist < 900.0) And (Dist1 < 150.0) And (WorkObstacleList.Parts(I).ReqOCH > 0.0) Then
            If (WorkObstacleList.Parts(I).ReqOCH > 0.0) Then
                K += 1
                LocalObstacleList.Parts(K) = WorkObstacleList.Parts(I)

                If WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx < 0 Then
                    L += 1
                    LocalObstacleList.Obstacles(L) = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner)
                    LocalObstacleList.Obstacles(L).PartsNum = 0
                    ReDim LocalObstacleList.Obstacles(L).Parts(WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).PartsNum - 1)
                    WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx = L
                End If

                LocalObstacleList.Parts(K).Owner = WorkObstacleList.Obstacles(WorkObstacleList.Parts(I).Owner).NIx
                LocalObstacleList.Parts(K).Index = LocalObstacleList.Obstacles(LocalObstacleList.Parts(K).Owner).PartsNum
                LocalObstacleList.Obstacles(LocalObstacleList.Parts(K).Owner).Parts(LocalObstacleList.Obstacles(LocalObstacleList.Parts(K).Owner).PartsNum) = K
                LocalObstacleList.Obstacles(LocalObstacleList.Parts(K).Owner).PartsNum += 1
            End If
        Next I

        If K < 0 Then
            ReDim LocalObstacleList.Obstacles(-1)
            ReDim LocalObstacleList.Parts(-1)
        Else
            ReDim Preserve LocalObstacleList.Parts(K)
            ReDim Preserve LocalObstacleList.Obstacles(L)

			If ExcludeObstFrm.ExcludeObstacles(LocalObstacleList, Me) Then
				HaveExcluded = False

				For I = 0 To L
					WorkObstacleList.Obstacles(LocalObstacleList.Obstacles(I).Index).IgnoredByUser = LocalObstacleList.Obstacles(I).IgnoredByUser
					If LocalObstacleList.Obstacles(I).IgnoredByUser Then HaveExcluded = True
				Next

				'ComboBox0102_SelectedIndexChanged(ComboBox0102, New System.EventArgs())
				'TextBox0106.Text = CStr(ConvertDistance(arMinISlen, eRoundMode.rmNERAEST))
				_hFAP = -100.0
				TextBox0102.Tag = ""
				TextBox0102_Validating(TextBox0102, New System.ComponentModel.CancelEventArgs())
			End If
		End If
    End Sub

    Private Sub TurnInfoBtn_Click(sender As System.Object, e As System.EventArgs) Handles THRInfoBtn.Click, TurnInfoBtn.Click
        If sender.Equals(THRInfoBtn) Then
            _InfoFrm.ShowTHRInfo(Me.Left + MultiPage1.Left + sender.Left, Me.Top + MultiPage1.Top + sender.Top + sender.Height)
        Else
            _InfoFrm.ShowTurnInfo(Me.Left + MultiPage1.Left + sender.Left, Me.Top + MultiPage1.Top + sender.Top) '+ sender.Height
        End If
    End Sub

    Private Sub MarkerBtn_Click(sender As System.Object, e As System.EventArgs) Handles MarkerBtn.Click
        MrkInfoForm.ShowMrkInfo(ILS.MkrList)
    End Sub

    Private Sub ProfileBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ProfileBtn.CheckedChanged
        If Not bFormInitialised Then Return

        If ProfileBtn.Checked() Then
            ArrivalProfile.Show()
        Else
            ArrivalProfile.Hide()
        End If
    End Sub

    Private Function IntermediateApproachLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim K As Integer
        Dim fDistToNav As Double
        Dim fDist As Double
        Dim fAltitudeDif As Double

        Dim fDir As Double
        Dim Angle As Double
        Dim PriorFixTolerance As Double
        Dim PostFixTolerance As Double

        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As ApproachLeg

        Dim pSegmentPoint As SegmentPoint
        Dim pFixDesignatedPoint As DesignatedPoint
        Dim pStartPoint As TerminalSegmentPoint
        Dim pPointReference As PointReference
        Dim pInterNavSignPt As SignificantPoint
        Dim pFIXSignPt As SignificantPoint

        Dim pAngleIndication As AngleIndication
        Dim pStDistanceIndication As DistanceIndication

        Dim pStAngleIndication As AngleIndication
        Dim pGuidNavSignPt As SignificantPoint

        Dim pSpeed As ValSpeed
        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical

        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData
        Dim IFIntesectNav As NavaidData
        Dim FAPIntesectNav As NavaidData

        pApproachLeg = pObjectDir.CreateFeature(Of IntermediateLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)

        pSegmentLeg = pApproachLeg

        '	pSegmentLeg.AltitudeOverrideATC =
        '	pSegmentLeg.AltitudeOverrideReference =
        '	pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
        '	pSegmentLeg.Note
        '	pSegmentLeg.ProcedureTurnRequired
        '	pSegmentLeg.ReqNavPerformance
        '	pSegmentLeg.SpeedInterpretation =

        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN

        GuidNav = ILS
        IFIntesectNav = IFNavDat(ComboBox0201.SelectedIndex)

        'pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.TF

        '=======================================================================================
        pSegmentLeg.Course = Dir2Azt(IFprj, _ArDir)
        pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        '=======================================================================================
        '    pSegmentLeg.BankAngle.Value = fBankAngle
        '=======================================================================================

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptFAP.Z + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(IFprj.Z + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical
        '======
        pDistance = New ValDistance(ConvertDistance(ReturnDistanceInMeters(IFprj, ptFAP), eRoundMode.NEAREST), mUomHDistance)
        pSegmentLeg.Length = pDistance
        '======
        pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(arImDescent_PDG))
        '======
        pSpeed = New ValSpeed(ConvertSpeed(cViafMax.Values(Category), eRoundMode.SPECIAL), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed

        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        ' Start Point ==========================================================================
        pStartPoint = New TerminalSegmentPoint()
        '	pStartPoint.IndicatorFACF =      ??????????
        '	pStartPoint.LeadDME =            ??????????
        '	pStartPoint.LeadRadial =         ??????????
        pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IF

        '==

        pSegmentPoint = pStartPoint
        'pSegmentPoint.FlyOver = False
        pSegmentPoint.RadarGuidance = False

        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        ' ========================
        pPointReference = New PointReference()
        pInterNavSignPt = IFIntesectNav.GetSignificantPoint()

        If (IFNavDat(K).IntersectionType = eIntersectionType.OnNavaid) Then
            pFIXSignPt = pInterNavSignPt
            pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
        Else
            Dim HorAccuracy As Double = CalcHorisontalAccuracy(IFprj, GuidNav, IFIntesectNav)

            pFixDesignatedPoint = CreateDesignatedPoint(IFprj, "IF", Azt2DirPrj(IFprj, pSegmentLeg.Course))
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, IFprj)
            Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(IFprj, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference.FacilityAngle.Add(pAngleUse)

            ' ========================

            If IFIntesectNav.TypeCode = eNavaidType.DME Then
                fDistToNav = ReturnDistanceInMeters(IFIntesectNav.pPtPrj, IFprj)
                fAltitudeDif = IFprj.Z - IFIntesectNav.pPtPrj.Z + FicTHRprj.Z

                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeDif * fAltitudeDif)
                pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
                pPointReference.Role = CodeReferenceRole.RAD_DME
            Else
                pAngleUse = New AngleUse
                fDir = ReturnAngleInDegrees(IFIntesectNav.pPtPrj, IFprj)

                Angle = Modulus(Dir2Azt(IFIntesectNav.pPtPrj, fDir) - IFIntesectNav.MagVar, 360.0)
                pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                pStAngleIndication.TrueAngle = Dir2Azt(IFprj, fDir)
                pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = False

                pPointReference.FacilityAngle.Add(pAngleUse)
                pPointReference.Role = CodeReferenceRole.INTERSECTION
            End If
        End If
        '=============================
        PriorPostFixTolerance(pIFTolerArea, IFprj, _ArDir, PriorFixTolerance, PostFixTolerance)

        pDistanceSigned = New ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
        pPointReference.PriorFixTolerance = pDistanceSigned

        pDistanceSigned = New ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

        pPointReference.PostFixTolerance = pDistanceSigned
        pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pIFTolerArea))
        '========
        pStartPoint.FacilityMakeup.Add(pPointReference)
        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.StartPoint = pStartPoint
        ' End Of Start Point ========================

        ' EndPoint ========================
        pEndPoint = New TerminalSegmentPoint
        '        pTerminalSegmentPoint.IndicatorFACF =      ??????????
        '        pTerminalSegmentPoint.LeadDME =            ??????????
        '        pTerminalSegmentPoint.LeadRadial =         ??????????
        pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF

        pSegmentPoint = pEndPoint

        'pSegmentPoint.FlyOver = False
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        ' ========================
        pPointReference = New PointReference()

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            FAPIntesectNav = FAPNavDat(ComboBox0101.SelectedIndex)

            pInterNavSignPt = FAPIntesectNav.GetSignificantPoint()

            If FAPIntesectNav.IntersectionType = eIntersectionType.OnNavaid Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                Dim HorAccuracy As Double = CalcHorisontalAccuracy(ptFAP, GuidNav, FAPIntesectNav)

                pFixDesignatedPoint = CreateDesignatedPoint(ptFAP, "FAP", Azt2DirPrj(ptFAP, pSegmentLeg.Course))
                pFIXSignPt = New SignificantPoint()
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

                fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptFAP)
                Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

                pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                pAngleIndication.TrueAngle = Dir2Azt(ptFAP, fDir)
                pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pAngleUse = New AngleUse()
                pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = True

                pPointReference.FacilityAngle.Add(pAngleUse)

                ' ========================

                'If FAPIntesectNav.TypeCode = eNavaidType.CodeDME Then

                fDistToNav = ReturnDistanceInMeters(FAPIntesectNav.pPtPrj, ptFAP)
                fAltitudeDif = ptFAP.Z - FAPIntesectNav.pPtPrj.Z + FicTHRprj.Z
                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeDif * fAltitudeDif)
                pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                ' ========================pStDistanceIndication.
                pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
                pPointReference.Role = CodeReferenceRole.RAD_DME

                'Else
                '	pAngleUse = New AngleUse
                '	fDir = ReturnAngleInDegrees(FAPIntesectNav.pPtPrj, ptFAP)

                '	Angle = Modulus(Dir2Azt(FAPIntesectNav.pPtPrj, fDir) - FAPIntesectNav.MagVar, 360.0)
                '	pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                '	pStAngleIndication.TrueAngle = Dir2Azt(ptFAP, fDir)

                '	pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                '	pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                '	pAngleUse.AlongCourseGuidance = False
                '	' ========================
                '	pPointReference.FacilityAngle.Add(pAngleUse)
                '	pPointReference.Role = CodeReferenceRole.INTERSECTION
                'End If

            End If

            PriorPostFixTolerance(pFAPTolerArea, ptFAP, _ArDir, PriorFixTolerance, PostFixTolerance)

            pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
            pPointReference.PriorFixTolerance = pDistanceSigned

            pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

            pPointReference.PostFixTolerance = pDistanceSigned
            pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pFAPTolerArea))
        Else
            pFixDesignatedPoint = CreateDesignatedPoint(ptFAP, "FAP", Azt2DirPrj(ptFAP, pSegmentLeg.Course))
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, ptFAP)
            Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(ptFAP, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference.FacilityAngle.Add(pAngleUse)
        End If

        pEndPoint.FacilityMakeup.Add(pPointReference)
        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.EndPoint = pEndPoint
        ' End of EndPoint ==========================================================================

        'Trajectory
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        Dim pLocation As Aran.Geometries.Point

        pLineStringSegment = New LineString()

        pLocation = ESRIPointToARANPoint(ToGeo(IFprj))
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(ToGeo(ptFAP))
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve

        ' protected Area =======================================================

        Dim pSurface As Surface = ESRIPolygonToAIXMSurface(ToGeo(IntermediatePrimArea))

        Dim pPrimProtectedArea As ObstacleAssessmentArea
        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY
        '===============================================================
        Dim pTopo As ITopologicalOperator2
        Dim pPolygon As IPolygon

        pTopo = pIFFIXPoly
        pPolygon = pTopo.Difference(IntermediatePrimArea)
        pTopo = pPolygon
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

        Dim pSecProtectedArea As ObstacleAssessmentArea
        pSecProtectedArea = New ObstacleAssessmentArea
        pSecProtectedArea.Surface = pSurface
        pSecProtectedArea.SectionNumber = 1
        pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(_InterMOCH + FicTHRprj.Z), mUomVDistance)
        pPrimProtectedArea.AssessedAltitude = pDistanceVertical
        pSecProtectedArea.AssessedAltitude = pDistanceVertical

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = lIntermObstacleList
		Dim pFullReleational As IRelationalOperator = pIFFIXPoly
        Dim pPrimReleational As IRelationalOperator = IntermediatePrimArea
        Dim i As Integer
		'Dim j As Integer
		'sort by Req.H
		'Added by Agshin
		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim l As Integer
		For l = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(l).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim requiredClearance As Double = 0
			Dim isPrimary As Integer = 0

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			requiredClearance = obstacles.Parts(i).MOC

			If Not pPrimReleational.Disjoint(obstacles.Parts(i).pPtPrj) Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim minimumAltitude As Double = tmpObstacle.Height + requiredClearance + FicTHRprj.Z

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(minimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = requiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			saveCount -= 1
		Next

		'//  END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
        pSegmentLeg.DesignSurface.Add(pSecProtectedArea)
        '======================================================================
        Return pApproachLeg
    End Function

    Private Function FinalApproachLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim fDir As Double
        Dim Angle As Double

        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As ApproachLeg
        Dim pFIXSignPt As SignificantPoint

        Dim pPointReference As PointReference
        Dim pGuidNavSignPt As SignificantPoint

        Dim pSegmentPoint As SegmentPoint
        Dim pAngleIndication As AngleIndication
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pSpeed As ValSpeed
        Dim pDistance As ValDistance
        Dim pDistanceVertical As ValDistanceVertical
        Dim pAngleUse As AngleUse

        Dim GuidNav As NavaidData

        Dim pLocation As Aran.Geometries.Point

        GuidNav = ILS

        pApproachLeg = pObjectDir.CreateFeature(Of FinalLeg)()

        Dim pFinalLeg As FinalLeg
        pFinalLeg = pApproachLeg

        If ILS.Category = 1 Then
            pFinalLeg.LandingSystemCategory = CodeApproachGuidance.ILS_PRECISION_CAT_I
        Else
            pFinalLeg.LandingSystemCategory = CodeApproachGuidance.ILS_PRECISION_CAT_II
        End If

        pFinalLeg.GuidanceSystem = CodeFinalGuidance.ILS

        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)

        pSegmentLeg = pApproachLeg
        '	pSegmentLeg.AltitudeOverrideATC =
        '	pSegmentLeg.AltitudeOverrideReference =
        '	pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
        '    pSegmentLeg.Note
        '    pSegmentLeg.ProcedureTurnRequired
        '    pSegmentLeg.ReqNavPerformance
        '	pSegmentLeg.SpeedInterpretation =

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        pSegmentLeg.Course = Dir2Azt(ptFAP, _ArDir) 'CDbl(TextBox0002.Text)

        '====================================================================================================

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(pMAPt.Z + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(ptFAP.Z + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.UpperLimitAltitude = pDistanceVertical

        pDistance = New ValDistance(ConvertDistance(ReturnDistanceInMeters(ptFAP, pMAPt), eRoundMode.NEAREST), mUomHDistance)
        pSegmentLeg.Length = pDistance

        pSegmentLeg.VerticalAngle = -ILS.GPAngle

        pSpeed = New ValSpeed(ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed

        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        ' Start Point ========================
        pSegmentLeg.StartPoint = pEndPoint
        ' End Of Start Point ========================

        ' EndPoint ========================
        pEndPoint = New TerminalSegmentPoint
        'pEndPoint.IndicatorFACF =      ??????????
        'pEndPoint.LeadDME =            ??????????
        'pEndPoint.LeadRadial =         ??????????
        pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.MAPT

        pSegmentPoint = pEndPoint

        'pSegmentPoint.FlyOver = True
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        ' ========================

        pPointReference = New PointReference()

        pFixDesignatedPoint = CreateDesignatedPoint(pMAPt, "MAPT", Azt2DirPrj(pMAPt, pSegmentLeg.Course))
        pFIXSignPt = New SignificantPoint()
        pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

        fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, pMAPt)
        Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

        pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
        pAngleIndication.TrueAngle = Dir2Azt(pMAPt, fDir)
        pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
        pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

        pAngleUse = New AngleUse()
        pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
        pAngleUse.AlongCourseGuidance = True

        ' ========================
        pPointReference.FacilityAngle.Add(pAngleUse)
        pEndPoint.FacilityMakeup.Add(pPointReference)

        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.EndPoint = pEndPoint

        ' End Of EndPoint =====================================================================================
        ' Trajectory =====================================================

        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        pLineStringSegment = New LineString

        pLocation = ESRIPointToARANPoint(ToGeo(ptFAP))
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(ToGeo(pMAPt))
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve

        ' Trajectory =====================================================
        ' protected Area =================================================

        Dim n As Integer = UBound(wOASPlanes)
        Dim i As Integer
        Dim j As Integer

        Dim pSrcPolygon As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pPrecisionPolygon As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pTurnCutter As ESRI.ArcGIS.Geometry.IPolyline = Nothing
        Dim pIfCutter As ESRI.ArcGIS.Geometry.IPolyline

        Dim pLeftGeom As ESRI.ArcGIS.Geometry.IGeometry
        Dim pRightGeom As ESRI.ArcGIS.Geometry.IGeometry

        Dim pRelation As IRelationalOperator
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2


        If CheckBox0301.Checked Then
            pTurnCutter = New ESRI.ArcGIS.Geometry.Polyline
            pTurnCutter.FromPoint = PointAlongPlane(TurnFixPnt, ILS.pPtPrj.M - 90.0, 10.0 * RModel)
            pTurnCutter.ToPoint = PointAlongPlane(TurnFixPnt, ILS.pPtPrj.M + 90.0, 10.0 * RModel)
        End If

        pIfCutter = New ESRI.ArcGIS.Geometry.Polyline
        pIfCutter.FromPoint = PointAlongPlane(IFprj, ILS.pPtPrj.M - 90.0, 10.0 * RModel)
        pIfCutter.ToPoint = PointAlongPlane(IFprj, ILS.pPtPrj.M + 90.0, 10.0 * RModel)

        pPrecisionPolygon = New ESRI.ArcGIS.Geometry.Polygon()

        For i = 0 To n - 1
            pSrcPolygon = wOASPlanes(i).Poly

            pRelation = pIfCutter
            If Not pRelation.Disjoint(pSrcPolygon) Then
                pTopo = pSrcPolygon
                pTopo.Cut(pIfCutter, pLeftGeom, pRightGeom)

                pSrcPolygon = pRightGeom
                pTopo = pSrcPolygon
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
            End If

            If Not pTurnCutter Is Nothing Then
                pRelation = pTurnCutter
                If Not pRelation.Disjoint(pSrcPolygon) Then
                    pTopo = pSrcPolygon
                    pTopo.Cut(pTurnCutter, pLeftGeom, pRightGeom)

                    pSrcPolygon = pLeftGeom
                    pTopo = pSrcPolygon
                    pTopo.IsKnownSimple_2 = False
                    pTopo.Simplify()
                End If
            End If

            pSrcPolygon = ToGeo(pSrcPolygon)
            For j = 0 To pSrcPolygon.GeometryCount - 1
                pPrecisionPolygon.AddGeometry(pSrcPolygon.Geometry(j))
            Next
        Next

        Dim pSurface As Surface
        Dim pPrimProtectedArea As ObstacleAssessmentArea

        pTopo = pPrecisionPolygon
        pTopo.IsKnownSimple_2 = True

        pSurface = ESRIPolygonToAIXMSurface(pPrecisionPolygon)

        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY


		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = PrecisionObstacleList
		Dim pFullReleational As IRelationalOperator = ToPrj(pPrecisionPolygon)

		'DrawPolygon(pFullReleational, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'While True
		'	Application.DoEvents()
		'End While

		'sort by ReqOCH
		'Added by Agshin
		Functions.Sort(obstacles, 3)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		'For i = 0 To obstacles.Obstacles.Length - 1
		'          If (saveCount = 0) Then Exit For

		'	Dim obs As Obstruction = New Obstruction()
		'	obs.VerticalStructureObstruction = New FeatureRef(obstacles.Obstacles(i).Identifier)



		'	Dim RequiredClearance As Double = 0
		'	Dim inArea As Boolean = False



		'	For j = 0 To obstacles.Obstacles(i).PartsNum - 1
		'		If pFullReleational.Disjoint(obstacles.Parts(obstacles.Obstacles(i).Parts(j)).pPtPrj) Then Continue For
		'		inArea = True
		'		RequiredClearance = Math.Max(RequiredClearance, obstacles.Parts(obstacles.Obstacles(i).Parts(j)).MOC)
		'	Next


		'	If Not inArea Then Continue For


		'	'ReqH
		'	Dim MinimumAltitude As Double = obstacles.Obstacles(i).Height + RequiredClearance + FicTHRprj.Z

		'	pDistanceVertical = New ValDistanceVertical()
		'	pDistanceVertical.Uom = mUomVDistance
		'	pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
		'	obs.MinimumAltitude = pDistanceVertical

		'	'MOC
		'	pDistance = New ValDistance()
		'	pDistance.Uom = UomDistance.M
		'	pDistance.Value = RequiredClearance
		'	obs.RequiredClearance = pDistance

		'	pPrimProtectedArea.SignificantObstacle.Add(obs)

		'          saveCount-=1
		'Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim requiredClearance As Double = 0

			If (obstacles.Parts(i).ReqOCH <= 0) Then Exit For

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			If pFullReleational.Disjoint(obstacles.Parts(i).pPtPrj) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			requiredClearance = obstacles.Parts(i).MOC

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim minimumAltitude As Double = obstacles.Parts(i).ReqOCH + FicTHRprj.Z

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(minimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = requiredClearance
			obs.RequiredClearance = pDistance

			pPrimProtectedArea.SignificantObstacle.Add(obs)

			saveCount -= 1
		Next



		'//  END ======================================================
		pDistanceVertical = New ValDistanceVertical(ConvertHeight(finalAssessedAltitude + FicTHRprj.Z), mUomVDistance)
        pPrimProtectedArea.AssessedAltitude = pDistanceVertical

        pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)

        '=================================================================================================================
        Dim pCondition As ApproachCondition = New ApproachCondition()
        pCondition.AircraftCategory.Add(IsLimitedTo)
        'pCondition.ClimbGradient = -ILS.GPAngle
        'pCondition.DesignSurface.Add(pPrimProtectedArea)

        Dim pMinimumSet As Minima = New Minima()

        'If Not CheckBox0301.Checked Then
        '	fOCH = fMisAprOCH 'DeConvertHeight(CDbl(TextBox0303.Text))
        'Else
        '	fOCH = fTA_OCH 'DeConvertHeight(CDbl(TextBox0906.Text))
        'End If

        'fOCA = fOCH + FicTHRprj.Z

        pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA
        pMinimumSet.AltitudeReference = CodeVerticalReference.MSL

        pMinimumSet.HeightCode = CodeMinimumHeight.OCH
        pMinimumSet.HeightReference = CodeHeightReference.HAT

        If CheckBox0301.Checked Then
            pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
            pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), mUomVDistance)
        Else
            pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), mUomVDistance)
            pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), mUomVDistance)
        End If


        pCondition.MinimumSet = pMinimumSet

        pFinalLeg.Condition.Add(pCondition)

        ' protected Area =================================================

        Return pApproachLeg
    End Function

    Private Function StraightMissedApproachLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim fDir As Double
        Dim fDist As Double
        Dim Angle As Double
        Dim fAltitude As Double
        Dim fDistToNav As Double
        Dim fCourseDir As Double
        Dim PostFixTolerance As Double
        Dim PriorFixTolerance As Double

        Dim pStDistanceIndication As DistanceIndication
        Dim pStAngleIndication As AngleIndication

        Dim IntersectNav As NavaidData
        Dim pFIXSignPt As SignificantPoint
        Dim pGuidNavSignPt As SignificantPoint
        Dim pInterNavSignPt As SignificantPoint
        Dim pAngleUse As AngleUse

        Dim pSpeed As ValSpeed
        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As MissedApproachLeg

        Dim pPointReference As PointReference

        Dim GuidNav As NavaidData

        Dim pSegmentPoint As SegmentPoint
        Dim pAngleIndication As AngleIndication
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical


        Dim pLocation As Aran.Geometries.Point

        GuidNav = ILS

        pApproachLeg = pObjectDir.CreateFeature(Of MissedApproachLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pApproachLeg

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.ABOVE_LOWER
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        'If ComboBox0401.SelectedIndex = 0 Then
        '	pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
        'Else
        '	pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
        'End If

        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT

        If OptionButton0401.Checked Then
            pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
            pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VA
        Else
            pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

            IntersectNav = TPInterNavDat(ArrayNum, ComboBox0501.SelectedIndex)

            If IntersectNav.IntersectionType = eIntersectionType.ByDistance Then
                pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VD
            ElseIf IntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
                pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
            Else
                pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VR
            End If
        End If

        '        pSegmentLeg.AltitudeOverrideATC =
        '        pSegmentLeg.AltitudeOverrideReference =
        '    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
        '    pSegmentLeg.EndConditionDesignator =
        '    pSegmentLeg.Note
        '    pSegmentLeg.ProcedureTurnRequired
        '    pSegmentLeg.ReqNavPerformance
        '    pSegmentLeg.SpeedInterpretation =
        '    pSegmentLeg.ReqNavPerformance
        '        pSegmentLeg.ProcedureTransition

        '=======================================================================================

        pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        pSegmentLeg.Course = Dir2Azt(pMAPt, _ArDir) 'CDbl(TextBox0002.Text)
        '===
        pSegmentLeg.BankAngle = fBankAngle
        '===

        pDistanceVertical = New ValDistanceVertical(ConvertHeight(TurnFixPnt.Z, eRoundMode.NEAREST), mUomVDistance)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        '===
        '    Set pDistanceVertical = New ValDistanceTypeCom
        '    pDistanceVertical.Uom = mUomVDistance
        '    fTmp = ConvertHeight(ptFAP.Z + ptLHPrj.Z, eRoundMode.rmNERAEST)
        '    pDistanceVertical.Value = CStr(fTmp)
        '    Set pSegmentLeg.UpperLimitAltitude = pDistanceVertical
        '    Set pSegmentLeg.upperLimitReference = VerticalReferenceType_MSL
        '===
        pDistance = New ValDistance(ConvertDistance(ReturnDistanceInMeters(TurnFixPnt, pMAPt), eRoundMode.NEAREST), mUomHDistance)
        pSegmentLeg.Length = pDistance
        '===
        pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
        '===
        pSpeed = New ValSpeed(CDbl(TextBox0402.Text), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed
        '===

        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        ' Start Point ======================================================
        pSegmentLeg.StartPoint = pEndPoint
        ' End Of Start Point ===============================================

        ' EndPoint =========================================================
        pEndPoint = New TerminalSegmentPoint
        'pTerminalSegmentPoint.IndicatorFACF =      ??????????
        'pTerminalSegmentPoint.LeadDME =            ??????????
        'pTerminalSegmentPoint.LeadRadial =         ??????????
        pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP
        '==
        pSegmentPoint = pEndPoint

        'pSegmentPoint.FlyOver = True
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = False

        '=======================================================================
        fCourseDir = Azt2DirPrj(TurnFixPnt, pSegmentLeg.Course)
        fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, TurnFixPnt)
        Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)
        '=======================================================================
        pPointReference = New PointReference()

        If OptionButton0401.Checked Then
            pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP
            pFixDesignatedPoint = CreateDesignatedPoint(TurnFixPnt, "MATF", fCourseDir)
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(TurnFixPnt, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference.FacilityAngle.Add(pAngleUse)
        Else
            pInterNavSignPt = IntersectNav.GetSignificantPoint()

            If (IntersectNav.IntersectionType = eIntersectionType.OnNavaid) Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                Dim HorAccuracy As Double = CalcHorisontalAccuracy(TurnFixPnt, GuidNav, IntersectNav)

                pFixDesignatedPoint = CreateDesignatedPoint(TurnFixPnt, "MATF", fCourseDir)
                pFIXSignPt = New SignificantPoint()
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

                pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                pAngleIndication.TrueAngle = Dir2Azt(TurnFixPnt, fDir)
                pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pAngleUse = New AngleUse()
                pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = True

                pPointReference.FacilityAngle.Add(pAngleUse)

                ' ========================

                If (IntersectNav.IntersectionType = eIntersectionType.ByDistance) Or (IntersectNav.IntersectionType = eIntersectionType.RadarFIX) Then
                    fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, TurnFixPnt)
                    fAltitude = TurnFixPnt.Z - IntersectNav.pPtPrj.Z

                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
                    pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                    pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    ' ========================pStDistanceIndication.
                    pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
                    pPointReference.Role = CodeReferenceRole.RAD_DME
                Else
                    pAngleUse = New AngleUse
                    fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, TurnFixPnt)

                    Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
                    pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                    pStAngleIndication.TrueAngle = Dir2Azt(TurnFixPnt, fDir)
                    pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                    pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = False

                    pPointReference.FacilityAngle.Add(pAngleUse)
                    pPointReference.Role = CodeReferenceRole.INTERSECTION
                End If
            End If

            PriorPostFixTolerance(pTPTolerArea, TurnFixPnt, _ArDir, PriorFixTolerance, PostFixTolerance)

            pDistanceSigned = New ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
            pPointReference.PriorFixTolerance = pDistanceSigned

            pDistanceSigned = New ValDistanceSigned()
            pDistanceSigned.Uom = mUomHDistance
            pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

            pPointReference.PostFixTolerance = pDistanceSigned
            pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTPTolerArea))

            pEndPoint.FacilityMakeup.Add(pPointReference)
        End If

        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.EndPoint = pEndPoint


        ' End Of EndPoint =================================================================
        ' Trajectory =====================================================

        Dim pCurve As Curve
        Dim pLineStringSegment As LineString
        pLineStringSegment = New LineString

        pLocation = ESRIPointToARANPoint(ToGeo(pMAPt))
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(ToGeo(TurnFixPnt))
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve

        ' Trajectory =====================================================
        ' protected Area =================================================

        'DrawPolygon(pFullPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
        'Application.DoEvents()

        'While True
        '	Application.DoEvents()
        'End While

        'DrawPolyLine(pCutter, 255, 2)
        'Application.DoEvents()

        Dim pSurface As Surface
        Dim pPrimProtectedArea As ObstacleAssessmentArea
        Dim i As Integer
        'Dim j As Integer

        'If CheckBox0301.Checked Then
        pSurface = ESRIPolygonToAIXMSurface(ToGeo(ZNR_Poly))
        'Else
        '	pSurface = ESRIPolygonToAIXMSurface(ToGeo(pFullPoly))
        'End If

        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = ZNRObstacleList


		'sort by Req.H
		'Added by Agshin
		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim requiredClearance As Double = 0

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			requiredClearance = obstacles.Parts(i).MOC

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + requiredClearance + FicTHRprj.Z

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = requiredClearance
			obs.RequiredClearance = pDistance

			pPrimProtectedArea.SignificantObstacle.Add(obs)

			saveCount -= 1
		Next

		'//  END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)

        ' protected Area =================================================

        Return pApproachLeg
    End Function

    Private Function MissedApproachTermination(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim fTmp As Double
        Dim fDir As Double
        Dim PostFixTolerance As Double
        Dim PriorFixTolerance As Double

        Dim HaveTP As Boolean
        Dim HaveIntercept As Boolean

        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As MissedApproachLeg

        Dim pSegmentPoint As SegmentPoint
        Dim pFIXSignPt As SignificantPoint
        Dim pPointReference As PointReference
        Dim pLocation As Aran.Geometries.Point

        Dim pSpeed As ValSpeed
        Dim pDistance As ValDistance
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistanceVertical As ValDistanceVertical

        HaveTP = CheckBox0301.Checked
        HaveIntercept = OptionButton0602.Checked

        pApproachLeg = pObjectDir.CreateFeature(Of MissedApproachLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pApproachLeg

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.AT_LOWER
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        ' Start Point ========================
        pSegmentLeg.StartPoint = pEndPoint
        ' End Of Start Point ========================

        '====================================================================================================
        Dim i As Integer
        Dim j As Integer

        Dim sTermAlt As String
        Dim fTermAlt As Double
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        If HaveTP Then
            If HaveIntercept Then
                sTermAlt = TextBox0903.Text
            Else
                sTermAlt = TextBox0908.Text
            End If
        Else
            sTermAlt = TextBox0308.Text
        End If

        pDistanceVertical = New ValDistanceVertical(CDbl(sTermAlt), mUomVDistance)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        fTermAlt = DeConvertHeight(pDistanceVertical.Value)

        If HaveTP Then
            If OptionButton0601.Checked Then
                pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VA
                pSegmentLeg.Course = Modulus(CDbl(TextBox0603.Text) + CurrADHP.MagVar, 360.0)
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
            ElseIf OptionButton0602.Checked Then
                pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VI
                pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

                pSegmentLeg.Course = System.Math.Round(Dir2Azt(MPtCollection.Point(1), MPtCollection.Point(1).M), 3)
                pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
            Else 'If OptionButton0603.Value Then
                If CheckBox0601.Checked Or (TurnDirector.TypeCode = eNavaidType.NONE) Then
                    pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.DF
                Else
                    pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
                End If

                pSegmentLeg.Course = Modulus(CDbl(TextBox0603.Text) + CurrADHP.MagVar, 360.0)
                pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO

                'IntersectNav = WPT_FIXToFixableNavaid(TurnWPT)

                ' EndPoint ========================
                pEndPoint = New TerminalSegmentPoint
                '        pTerminalSegmentPoint.IndicatorFACF =      ??????????
                '        pTerminalSegmentPoint.LeadDME =            ??????????
                '        pTerminalSegmentPoint.LeadRadial =         ??????????
                pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.MAHF

                pSegmentPoint = pEndPoint

                'pSegmentPoint.FlyOver = True
                pSegmentPoint.RadarGuidance = False
                pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
                pSegmentPoint.Waypoint = False
                '=======================================================================
                'Dim bOnNav As Boolean
                'bOnNav = False
                'If TurnWPT.TypeCode <> eNavaidType.CodeNONE Then
                'bOnNav = True
                'If ReturnDistanceInMeters(TurnWPT.pPtPrj, TerFixPnt) < 2.0 Then
                '	Angle = Modulus(pSegmentLeg.Course - TurnWPT.MagVar, 360.0)
                '	bOnNav = True
                'Else
                '	fDir = ReturnAngleInDegrees(TurnWPT.pPtPrj, TerFixPnt)
                '	Angle = Modulus(Dir2Azt(TurnWPT.pPtPrj, fDir) - TurnWPT.MagVar, 360.0)
                'End If

                'If Not bOnNav Then
                '	pGuidNavSignPt = New SignificantPoint()
                '	pGuidNavSignPt.NavaidSystem = TurnWPT.pFeature.GetFeatureRef()

                '	pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                '	pAngleIndication.TrueAngle = Dir2Azt(TerFixPnt, fDir)

                '	pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

                '	pAngleUse = New AngleUse()
                '	pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                '	pAngleUse.AlongCourseGuidance = True
                '	pPointReference = New PointReference()
                '	pPointReference.FacilityAngle.Add(pAngleUse)
                'End If
                'End If
                '=======================================================================

                If TurnDirector.TypeCode <> eNavaidType.NONE Then
                    fDir = Azt2DirPrj(TurnDirector.pPtPrj, pSegmentLeg.Course)
                    If TurnDirector.TypeCode = eNavaidType.NDB Then
                        NDBFIXTolerArea(TurnDirector.pPtPrj, fDir, fTermAlt, pTermFIXTolerArea)
                    Else
                        VORFIXTolerArea(TurnDirector.pPtPrj, fDir, fTermAlt, pTermFIXTolerArea)
                    End If

                    pTopo = pTermFIXTolerArea
                    pTopo.IsKnownSimple_2 = False
                    pTopo.Simplify()

                    pPointReference = New PointReference()

                    pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
                    pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF

                    pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD


                    PriorPostFixTolerance(pTermFIXTolerArea, TurnDirector.pPtPrj, fDir, PriorFixTolerance, PostFixTolerance)

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                    pDistanceSigned.Uom = mUomHDistance
                    pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
                    pPointReference.PriorFixTolerance = pDistanceSigned

                    pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
                    pDistanceSigned.Uom = mUomHDistance
                    pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

                    pPointReference.PostFixTolerance = pDistanceSigned
                    pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTermFIXTolerArea))
                    ' ========================
                    pEndPoint.FacilityMakeup.Add(pPointReference)
                Else
                    pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
                End If

                pFIXSignPt = TurnDirector.GetSignificantPoint()
                pSegmentPoint.PointChoice = pFIXSignPt
                pSegmentLeg.EndPoint = pEndPoint
                ' End Of EndPoint ========================
            End If
            pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        Else
            pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CA
            pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
            pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
            pSegmentLeg.Course = Dir2Azt(pMAPt, _ArDir)

            'Dim pCondition As ApproachCondition = New ApproachCondition()
            'pCondition.AircraftCategory.Add(IsLimitedTo)
            ''pCondition.ClimbGradient = -ILS.GPAngle
            ''pCondition.DesignSurface.Add(pPrimProtectedArea)

            'Dim pMinimumSet As Minima = New Minima()

            'pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fMisAprOCH + FicTHRprj.Z, eRoundMode.rmNERAEST), mUomVDistance)
            'pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA
            'pMinimumSet.AltitudeReference = CodeVerticalReference.MSL

            'pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fMisAprOCH, eRoundMode.rmNERAEST), mUomVDistance)
            'pMinimumSet.HeightCode = CodeMinimumHeight.OCH
            'pMinimumSet.HeightReference = CodeHeightReference.HAT

            'pCondition.MinimumSet = pMinimumSet

            'Dim pMALeg As MissedApproachLeg = pApproachLeg
            'pMALeg.Condition.Add(pCondition)
        End If

        '====================================================================================================

        pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
        '======
        pSpeed = New ValSpeed
        pSpeed.Uom = mUomSpeed

        If HaveTP Then
            pSpeed.Value = CDbl(TextBox0402.Text)
        Else
            pSpeed.Value = ConvertSpeed(cVmaFaf.Values(Category), eRoundMode.SPECIAL)
        End If
        pSegmentLeg.SpeedLimit = pSpeed

        '======
        Dim pLineStringSegment As LineString
        Dim pCurve As Curve

        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pPrimeAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
        Dim pPrimProtectedArea As ObstacleAssessmentArea
        Dim pSecProtectedArea As ObstacleAssessmentArea = Nothing
        Dim pSurface As Surface

		Dim obstacles As ObstacleContainer

		pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY
        pPrimProtectedArea.SectionNumber = 0

        pCurve = New Curve()
        pCutter = New Polyline()

        If HaveTP Then
            Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline

            If HaveIntercept Then
                pPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
                '**************************************************************
                Dim pIZ As IZ
                Dim pIZAware As IZAware

                pIZAware = pPoly
                pIZAware.ZAware = True

                pIZ = pPoly
                pIZ.CalculateNonSimpleZs()
                '**************************************************************
            Else
                pPoly = mPoly
            End If

            'Set pPointCollection = pPoly
            fTmp = ConvertDistance(pPoly.Length, eRoundMode.NEAREST)

            ' Trajectory =====================================================

            Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
            Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection

            pPolyline = pPoly
            For j = 0 To pPolyline.GeometryCount - 1
                pPath = pPolyline.Geometry(j)
                pLineStringSegment = New LineString

                For i = 0 To pPath.PointCount - 1
                    pLocation = ESRIPointToARANPoint(ToGeo(pPath.Point(i)))
                    pLineStringSegment.Add(pLocation)
                Next i
                pCurve.Geo.Add(pLineStringSegment)
            Next j

			' Trajectory =====================================================
			' Protection Area ================================================
			obstacles = TurnAreaObstacleList

			Dim pSecndAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
            Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
            Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
            Dim fCurrX, fCurrY, dBuffer As Double

            If Not Double.TryParse(TextBox0901.Text, dBuffer) Then
                dBuffer = -1.0
            Else
                dBuffer = Functions.DeConvertDistance(dBuffer)
            End If

            If dBuffer > TurnAreaMaxd0 Then dBuffer = TurnAreaMaxd0
            If dBuffer < arBufferMSA.Value Then dBuffer = arBufferMSA.Value

            If OptionButton0601.Checked Then                        'Or (TurnDirector.TypeCode <= eNavaidType.NONE)
                Dim pClone As ESRI.ArcGIS.esriSystem.IClone
                Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection
                Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint

                pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)

                pClone = mPoly
                pPointCollection = pClone.Clone
                pPointCollection.AddPoint(PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, 10.0 * RModel))

                'pPoly = pTopo.Intersect(pPointCollection, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
                'pPointCollection = pPoly

                ptCnt = pTurnComplitPt

                For i = 0 To pPointCollection.PointCount - 1
                    PrjToLocal(ptCnt, m_OutDir, pPointCollection.Point(i), fCurrX, fCurrY)
                    If Math.Abs(fCurrY) > distEps Then Continue For
                    If fCurrX > 0.0 Then
                        ptCnt = pPointCollection.Point(i)
                    End If
                Next
            ElseIf OptionButton0602.Checked Then
                ptCnt = TerFixPnt
            Else 'If OptionButton0603.Checked Then
                ptCnt = TurnDirector.pPtPrj
            End If

            pPath.AddPoint(ptCnt)
            pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
            pPolyline.AddGeometry(pPath)

            For i = 0 To m_BasePoints.PointCount - 1
                PrjToLocal(ptCnt, m_OutDir, m_BasePoints.Point(i), fCurrX, fCurrY)
                If fCurrX > 0.0 Then ptCnt = m_BasePoints.Point(i)
            Next

            PrjToLocal(ptCnt, m_OutDir, TurnFixPnt, fCurrX, fCurrY)
            If fCurrX > 0.0 Then ptCnt = TurnFixPnt

            ' =====================================================================================
            pCutter = New ESRI.ArcGIS.Geometry.Polyline()

            If OptionButton0601.Checked Then
                pCutter.FromPoint = Functions.LocalToPrj(ptCnt, m_OutDir, 0.0, -10.0 * RModel)
                pCutter.ToPoint = Functions.LocalToPrj(ptCnt, m_OutDir, 0.0, 10.0 * RModel)
            Else
                pCutter.FromPoint = Functions.LocalToPrj(ptCnt, m_OutDir, dBuffer, -10.0 * RModel)
                pCutter.ToPoint = Functions.LocalToPrj(ptCnt, m_OutDir, dBuffer, 10.0 * RModel)
            End If

            'pCutter.FromPoint = PointAlongPlane(TerFixPnt, m_OutDir - 90.0, 10.0 * RModel)
            'pCutter.ToPoint = PointAlongPlane(TerFixPnt, m_OutDir + 90.0, 10.0 * RModel)
            '==============================================================================================++++++++++++++++++++++++++++++++++++
            pSecndAreaPoly = Nothing

            Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator
            pRelational = pCutter
            If Not pRelational.Disjoint(BaseArea) Then
                pTopo = BaseArea
                pTopo.Cut(pCutter, pPrimeAreaPoly, pTmpPoly)
            Else
                Dim pClone As ESRI.ArcGIS.esriSystem.IClone = BaseArea
                pPrimeAreaPoly = pClone.Clone()
            End If

            pTopo = pPrimeAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            If OptionButton0602.Checked Then
                pPrimeAreaPoly = pTopo.Union(pTermFIXTolerArea)
                pTopo = pPrimeAreaPoly
                pTopo.IsKnownSimple_2 = False
                pTopo.Simplify()
            End If

            If (Not OptionButton0601.Checked) And (TurnDirector.TypeCode > eNavaidType.NONE) Then
                pTmpPoly = SecPoly

                If Not pTmpPoly.IsEmpty Then
                    ClipByLine(SecPoly, pCutter, pSecndAreaPoly, pTmpPoly, pPolygon)
                End If

                If Not (pSecndAreaPoly Is Nothing) Then
                    pTopo = pPrimeAreaPoly
                    pPrimeAreaPoly = pTopo.Difference(pSecndAreaPoly)

                    pTopo = pPrimeAreaPoly
                    pTopo.IsKnownSimple_2 = False
                    pTopo.Simplify()
                End If
            End If

            If (Not pSecndAreaPoly Is Nothing) Then
                If (Not pSecndAreaPoly.IsEmpty) Then
                    pSurface = ESRIPolygonToAIXMSurface(ToGeo(pSecndAreaPoly))
                    pSecProtectedArea = New ObstacleAssessmentArea
                    pSecProtectedArea.Surface = pSurface
                    pSecProtectedArea.SectionNumber = 1
                    pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY
                End If
            End If
            ' protected Area =================================================
        Else
			' Trajectory =====================================================
			obstacles = MAObstacleList

			''Dim dMAPt2SOC As Double
			''Dim fMAPtOCH As Double
			'         Dim pTermPt As ESRI.ArcGIS.Geometry.IPoint

			''dMAPt2SOC = ReturnDistanceInMeters(PtSOC, pMAPt)
			''fMAPtOCH = fMisAprOCH 'DeConvertHeight(CDbl(TextBox0303.Text))
			''fTmp = (fTermAlt - fMAPtOCH - FicTHRprj.Z + m_fMOC) / fMissAprPDG
			'fTmp = DeConvertDistance(CDbl(TextBox0309.Text))

			''pTermPt = PointAlongPlane(PtSOC, _ArDir, fTmp)
			''fTmp = ConvertDistance(fTmp + dMAPt2SOC, eRoundMode.NEAREST)

			'pTermPt = PointAlongPlane(pMAPt, _ArDir, fTmp)
			'fTmp = ConvertDistance(fTmp, eRoundMode.NEAREST)

			fTmp = ConvertDistance(pStraightNomLine.Length, eRoundMode.NEAREST)

            pLineStringSegment = New LineString

            pLocation = ESRIPointToARANPoint(ToGeo(pMAPt))
            pLineStringSegment.Add(pLocation)

            pLocation = ESRIPointToARANPoint(ToGeo(pTermPt))
            pLineStringSegment.Add(pLocation)

            pCurve.Geo.Add(pLineStringSegment)
            ' Trajectory =====================================================
            ' protected Area =================================================

            pCutter.FromPoint = PointAlongPlane(pTermPt, _ArDir - 90.0, 10.0 * RModel)
            pCutter.ToPoint = PointAlongPlane(pTermPt, _ArDir + 90.0, 10.0 * RModel)

            pTopo = pFullPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            'DrawPolygon(pFullPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
            'DrawPolyLine(pCutter, -1, 2)
            'Application.DoEvents()

            pTopo.Cut(pCutter, pPrimeAreaPoly, pTmpPoly)
            pTopo = pPrimeAreaPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            ' protected Area =================================================
        End If

        pDistance = New ValDistance(fTmp, mUomHDistance)
        pSegmentLeg.Length = pDistance
        pSegmentLeg.Trajectory = pCurve

        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPrimeAreaPoly))
        pPrimProtectedArea.Surface = pSurface

        ' Protection Area Obstructions list ==================================================
        'Dim i As Integer
        'Dim j As Integer

        'sort by Req.H
        'Added by Agshin

        'we have to look again
        If (HaveTP) Then
			Functions.Sort(obstacles, 2)
		Else
			Functions.Sort(obstacles, 3)
		End If

		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim requiredClearance As Double = 0
			Dim isPrimary As Integer = 0

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			requiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags = 0 Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim minimumAltitude As Double = tmpObstacle.Height + requiredClearance + FicTHRprj.Z

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(minimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = requiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			saveCount -= 1
		Next

		' END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
        If Not (pSecProtectedArea Is Nothing) Then pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

        ' END =================================================
        Return pApproachLeg
        ' END of MissedApproachTermination =================================================
    End Function

    Private Function MissedApproachTerminationContinued(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim fDir As Double
        Dim Angle As Double
        Dim fDist As Double

        Dim fAltitude As Double
        Dim fDistToNav As Double
        Dim PostFixTolerance As Double
        Dim PriorFixTolerance As Double

        Dim pFIXSignPt As SignificantPoint
        Dim pInterNavSignPt As SignificantPoint
        Dim pGuidNavSignPt As SignificantPoint
        Dim pPointReference As PointReference

        Dim pSegmentLeg As SegmentLeg
        Dim pApproachLeg As MissedApproachLeg

        Dim pStDistanceIndication As DistanceIndication
        Dim pStAngleIndication As AngleIndication

        Dim pSegmentPoint As SegmentPoint
        Dim pAngleIndication As AngleIndication
        Dim pFixDesignatedPoint As DesignatedPoint

        Dim GuidNav As NavaidData
        Dim IntersectNav As NavaidData
        Dim pAngleUse As AngleUse

        Dim pDistanceVertical As ValDistanceVertical
        Dim pDistanceSigned As ValDistanceSigned
        Dim pDistance As ValDistance
        Dim pSpeed As ValSpeed

        Dim pLocation As Aran.Geometries.Point
        Dim pInterceptPt As ESRI.ArcGIS.Geometry.IPoint

        GuidNav = WPT_FIXToNavaid(TurnDirector)
        IntersectNav = TerInterNavDat(ComboBox0902.SelectedIndex)
        pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)
        '===================================================================

        pApproachLeg = pObjectDir.CreateFeature(Of MissedApproachLeg)()
        'pApproachLeg.Approach = pProcedure.GetFeatureRef()
        pApproachLeg.AircraftCategory.Add(IsLimitedTo)
        pSegmentLeg = pApproachLeg

        pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.ABOVE_LOWER
        pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
        'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
        pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
        pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
        pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
        pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
        pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

        ' Course ===============
        If SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0 Then
            pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
        Else
            pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        End If

        pSegmentLeg.Course = Modulus(CDbl(TextBox0603.Text) + GuidNav.MagVar, 360.0)

        ' LowerLimitAltitude =======================================================================================
        pDistanceVertical = New ValDistanceVertical(CDbl(TextBox0908.Text), mUomVDistance)
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        ' VerticalAngle ====================================================================================================
        pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
        ' SpeedLimit ======

        pSpeed = New ValSpeed(CDbl(TextBox0402.Text), mUomSpeed)
        pSegmentLeg.SpeedLimit = pSpeed

        ' Length ======
        pDistance = New ValDistance
        pDistance.Uom = mUomHDistance
        pDistance.Value = ConvertDistance(ReturnDistanceInMeters(pInterceptPt, TerFixPnt), eRoundMode.NEAREST)
        pSegmentLeg.Length = pDistance

        ' Start Point ===============================
        pSegmentLeg.StartPoint = pEndPoint
        ' End Of Start Point ========================

        pGuidNavSignPt = GuidNav.GetSignificantPoint()

        ' EndPoint ========================
        pEndPoint = New TerminalSegmentPoint
        '        pEndPoint.IndicatorFACF =      ??????????
        '        pEndPoint.LeadDME =            ??????????
        '        pEndPoint.LeadRadial =         ??????????
        pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.MAHF
        '==
        pSegmentPoint = pEndPoint

        'pSegmentPoint.FlyOver = False
        pSegmentPoint.RadarGuidance = False
        pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
        pSegmentPoint.Waypoint = True
        '=======================================================================
        pPointReference = New PointReference()
        pInterNavSignPt = IntersectNav.GetSignificantPoint()

        If IntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
            pFIXSignPt = pInterNavSignPt
            pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
        Else
            Dim HorAccuracy As Double = CalcHorisontalAccuracy(TerFixPnt, GuidNav, IntersectNav)

            pFixDesignatedPoint = CreateDesignatedPoint(TerFixPnt, "MAHF", Azt2DirPrj(TerFixPnt, pSegmentLeg.Course))
            pFIXSignPt = New SignificantPoint()
            pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

            fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, TerFixPnt)
            Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

            pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
            pAngleIndication.TrueAngle = Dir2Azt(TerFixPnt, fDir)
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
            pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

            pAngleUse = New AngleUse()
            pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
            pAngleUse.AlongCourseGuidance = True

            pPointReference.FacilityAngle.Add(pAngleUse)

            ' ========================

            If IntersectNav.TypeCode = eNavaidType.DME Then
                fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, TerFixPnt)
                fAltitude = TerFixPnt.Z - IntersectNav.pPtPrj.Z + FicTHRprj.Z
                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
                pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                ' ========================pStDistanceIndication.
                pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
                pPointReference.Role = CodeReferenceRole.RAD_DME
            Else
                pAngleUse = New AngleUse
                fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, TerFixPnt)

                Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
                pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                pStAngleIndication.TrueAngle = Dir2Azt(TerFixPnt, fDir)
                pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
                pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
                pAngleUse.AlongCourseGuidance = False
                ' ========================
                pPointReference.FacilityAngle.Add(pAngleUse)
                pPointReference.Role = CodeReferenceRole.INTERSECTION
            End If
        End If

        fDir = Azt2DirPrj(TerFixPnt, pSegmentLeg.Course)
        PriorPostFixTolerance(pTermFIXTolerArea, TerFixPnt, fDir, PriorFixTolerance, PostFixTolerance)

        pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
        pPointReference.PriorFixTolerance = pDistanceSigned

        pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
        pDistanceSigned.Uom = mUomHDistance
        pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

        pPointReference.PostFixTolerance = pDistanceSigned
        pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTermFIXTolerArea))
        ' ========================
        pEndPoint.FacilityMakeup.Add(pPointReference)
        pSegmentPoint.PointChoice = pFIXSignPt
        pSegmentLeg.EndPoint = pEndPoint
        ' End Of EndPoint ========================

        'Dim pCondition As ApproachCondition = New ApproachCondition()
        'pCondition.AircraftCategory.Add(IsLimitedTo)
        ''pCondition.ClimbGradient = -ILS.GPAngle
        ''pCondition.DesignSurface.Add(pPrimProtectedArea)

        'Dim pMinimumSet As Minima = New Minima()

        'pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.rmNERAEST), mUomVDistance)
        'pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA
        'pMinimumSet.AltitudeReference = CodeVerticalReference.MSL

        'pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.rmNERAEST), mUomVDistance)
        'pMinimumSet.HeightCode = CodeMinimumHeight.OCH
        'pMinimumSet.HeightReference = CodeHeightReference.HAT

        'pCondition.MinimumSet = pMinimumSet

        'Dim pMALeg As MissedApproachLeg = pApproachLeg
        'pMALeg.Condition.Add(pCondition)

        ' Trajectory =====================================================
        Dim pCurve As Curve
        Dim pLineStringSegment As LineString

        pLineStringSegment = New LineString

        pLocation = ESRIPointToARANPoint(ToGeo(pInterceptPt))
        pLineStringSegment.Add(pLocation)

        pLocation = ESRIPointToARANPoint(ToGeo(TerFixPnt))
        pLineStringSegment.Add(pLocation)

        pCurve = New Curve
        pCurve.Geo.Add(pLineStringSegment)

        pSegmentLeg.Trajectory = pCurve
        ' Trajectory =====================================================
        Return pApproachLeg
        ' END of MissedApproachTermination =================================================
    End Function

    Private Function MissedApproachLegs(segment As TraceSegment, ByRef pProcedure As InstrumentApproachProcedure, IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
        Dim uomSpeedTab() As UomSpeed
        Dim uomDistHorzTab() As UomDistance
        Dim uomDistVerTab() As UomDistanceVertical

        uomDistHorzTab = {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
        uomDistVerTab = {UomDistanceVertical.M, UomDistanceVertical.FT} ', UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER}
        uomSpeedTab = {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

        Dim mUomHDistance As UomDistance = uomDistHorzTab(GlobalVars.DistanceUnit)
        Dim mUomVDistance As UomDistanceVertical = uomDistVerTab(GlobalVars.HeightUnit)
        Dim mUomSpeed As UomSpeed = uomSpeedTab(GlobalVars.SpeedUnit)

        Dim result As ApproachLeg = DBModule.pObjectDir.CreateFeature(Of MissedApproachLeg)()
        'result.Approach = pProcedure.GetFeatureRef()
        result.AircraftCategory.Add(IsLimitedTo)

        Dim pSegmentLeg As SegmentLeg = result

        pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER
        'pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL
        pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL
        pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK

        '=================================
        'FIXableNavaidType IntersectNav
        Dim SttIntersectNav As NavaidData = segment.InterceptionNav

        'Dim EndIntersectNav as NavaidData
        Dim ptStart As ESRI.ArcGIS.Geometry.IPoint = segment.ptIn
        Dim ptEnd As ESRI.ArcGIS.Geometry.IPoint = segment.ptOut

        '  ======================================================================
        pSegmentLeg.LegTypeARINC = segment.LegType

        'if (Functions.SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0)
        '    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
        'else
        '    pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
        Dim fCourse, fCourseDir As Double

        Select Case (segment.SegmentCode)
            'Case eSegmentType.straight					' "Прямой сегмент"
            Case eSegmentType.courseIntercept           ' "На заданную WPT"
            Case eSegmentType.arcIntercept
                fCourseDir = segment.DirIn
                fCourse = Functions.Dir2Azt(segment.ptIn, segment.DirIn)
            Case eSegmentType.turnAndIntercept
                fCourseDir = segment.DirBetween
                fCourse = Functions.Dir2Azt(segment.PtCenter1, fCourseDir)
            Case Else
                fCourseDir = segment.DirOut
                fCourse = Functions.Dir2Azt(segment.ptOut, segment.DirOut)
        End Select

        pSegmentLeg.Course = fCourse

        '  LowerLimitAltitude ========================================================
        Dim pDistanceVertical As ValDistanceVertical = New ValDistanceVertical()
        pDistanceVertical.Uom = mUomVDistance
        pDistanceVertical.Value = Functions.ConvertHeight(segment.HFinish, eRoundMode.NEAREST)        'Trace(index).HStart
        pSegmentLeg.LowerLimitAltitude = pDistanceVertical

        '  UpperLimitAltitude ========================================================
        'pDistanceVertical = new ValDistanceVertical();
        'pDistanceVertical.Uom = mUomVDistance;
        'pDistanceVertical.Value = Functions.ConvertHeight(Trace[index].HFinish, eRoundMode.rmNERAEST);
        'pSegmentLeg.UpperLimitAltitude = pDistanceVertical;
        '=================================
        pSegmentLeg.BankAngle = segment.BankAngle
        pSegmentLeg.VerticalAngle = Functions.RadToDeg(System.Math.Atan(segment.PDG))
        ' =================

        Dim pDistance As ValDistance = New ValDistance()
        pDistance.Uom = mUomHDistance
        pDistance.Value = Functions.ConvertDistance(segment.Length, eRoundMode.NEAREST)
        pSegmentLeg.Length = pDistance

        Dim pSpeed As ValSpeed = New ValSpeed()
        pSpeed.Uom = mUomSpeed
        pSpeed.Value = Double.Parse(TextBox0402.Text)
        pSegmentLeg.SpeedLimit = pSpeed
        pSegmentLeg.SpeedReference = CodeSpeedReference.IAS

        ' Start Point ========================
        pSegmentLeg.StartPoint = pEndPoint

        ' End Of Start Point ===================================

        ' Course Indication =====================================================
        Dim GuidNav As NavaidData = segment.GuidanceNav
        Dim fDistToNav, fAltitudeMin As Double
        Dim Angle, fDist, fDir As Double

        Dim pAngleIndication As AngleIndication = Nothing
        Dim pDistanceIndication As DistanceIndication = Nothing

        If (GuidNav.TypeCode > eNavaidType.NONE) Then  'And (segment.SegmentCode <> eSegmentType.turnAndIntercept)
            If (GuidNav.TypeCode = eNavaidType.DME) Then
                fDistToNav = Functions.ReturnDistanceInMeters(GuidNav.pPtPrj, ptEnd)
                fAltitudeMin = segment.HFinish - GuidNav.pPtPrj.Z
                fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)

                pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, GuidNav.GetSignificantPoint())
                pSegmentLeg.Distance = pDistanceIndication.GetFeatureRef()
            ElseIf (GuidNav.TypeCode <> eNavaidType.NONE) Then
                Dim pGuidNavSignPt As SignificantPoint = GuidNav.GetSignificantPoint()

                fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd)
                Angle = NativeMethods.Modulus(fCourse - GuidNav.MagVar)

                pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
                pAngleIndication.TrueAngle = Functions.Dir2Azt(ptEnd, fDir)
                pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

                pAngleIndication.TrueAngle = pSegmentLeg.Course
                pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

                pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
            End If
        End If
        ' End Of Indication ======================================================================

        ' Leg Points ========================
        pEndPoint = Nothing

        If (segment.LegType = CodeSegmentPath.AF) Or (segment.LegType = CodeSegmentPath.HF) Or (segment.LegType = CodeSegmentPath.IF) Or
            (segment.LegType = CodeSegmentPath.TF) Or (segment.LegType = CodeSegmentPath.CF) Or (segment.LegType = CodeSegmentPath.DF) Or
            (segment.LegType = CodeSegmentPath.RF) Or (segment.LegType = CodeSegmentPath.CD) Or (segment.LegType = CodeSegmentPath.CR) Or
            (segment.LegType = CodeSegmentPath.VD) Or (segment.LegType = CodeSegmentPath.VR) Then

            pEndPoint = New TerminalSegmentPoint()
            pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.OTHER_WPT
            Dim pSegmentPoint As SegmentPoint = pEndPoint
            'pSegmentPoint.FlyOver = True

            pSegmentPoint.RadarGuidance = False
            pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
            pSegmentPoint.Waypoint = False

            'pEndPoint.IndicatorFACF =      ??????????
            'pEndPoint.LeadDME =            ??????????
            'pEndPoint.LeadRadial =         ??????????
            'Dim GuidNav As NavaidData = segment.GuidanceNav
            'segment.tu
            'If GuidNav.TypeCode = eNavaidType.DME Then
            ''	If (Functions.SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0) Then
            ''		pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW
            ''	Else
            ''		pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CCW
            ''	End If

            '	fCourseDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd) + IIf(pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.OTHER_CW, 270.0, 90.0)
            'End If
            'fCourseDir = Azt2DirPrj(ptStart, pSegmentLeg.Course)

            Dim bOnNav As Boolean = False
            Dim pInterNavSignPt As SignificantPoint = Nothing

            If (SttIntersectNav.TypeCode <> eNavaidType.NONE) And (SttIntersectNav.Identifier <> Guid.Empty) Then
                pInterNavSignPt = SttIntersectNav.GetSignificantPoint()

                'if(SttIntersectNav.Identifier = GuidNav.Identifier)
                If (SttIntersectNav.Tag = -1) Then bOnNav = True
            End If

            ' Guidance Indication =====================================================================
            Dim pAngleUse As AngleUse
            Dim pPointReference As PointReference = New PointReference()
            Dim pFIXSignPt As SignificantPoint

            If (bOnNav) Then
                pFIXSignPt = pInterNavSignPt
                pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
            Else
                Dim HorAccuracy As Double = 0.0

                If (GuidNav.Identifier <> Guid.Empty) And (SttIntersectNav.TypeCode <> eNavaidType.NONE) Then
                    HorAccuracy = CalcHorisontalAccuracy(TerFixPnt, GuidNav, SttIntersectNav)
                End If

                Dim pFixDesignatedPoint As DesignatedPoint = DBModule.CreateDesignatedPoint(ptEnd, "COORD", fCourseDir)

                If GuidNav.Identifier <> Guid.Empty Then
                    If GuidNav.TypeCode = eNavaidType.DME Then
                        pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                        pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                    ElseIf GuidNav.TypeCode <> eNavaidType.NONE Then
                        pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                        pAngleUse = New AngleUse()
                        pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                        pAngleUse.AlongCourseGuidance = True

                        pPointReference.FacilityAngle.Add(pAngleUse)
                    End If
                End If

                ' End Of Indication ============================================

                pFIXSignPt = New SignificantPoint()
                pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

                If SttIntersectNav.TypeCode = eNavaidType.DME Then
                    fDistToNav = Functions.ReturnDistanceInMeters(SttIntersectNav.pPtPrj, ptEnd)
                    fAltitudeMin = segment.HFinish - SttIntersectNav.pPtPrj.Z

                    fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeMin * fAltitudeMin)
                    pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
                    pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

                    pPointReference.FacilityDistance.Add(pDistanceIndication.GetFeatureRefObject())
                    pPointReference.Role = CodeReferenceRole.RAD_DME
                ElseIf SttIntersectNav.TypeCode <> eNavaidType.NONE Then
                    pAngleUse = New AngleUse()
                    fDir = Functions.ReturnAngleInDegrees(SttIntersectNav.pPtPrj, ptEnd)

                    Angle = NativeMethods.Modulus(Functions.Dir2Azt(SttIntersectNav.pPtPrj, fDir) - SttIntersectNav.MagVar, 360.0)
                    pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
                    pAngleIndication.TrueAngle = Functions.Dir2Azt(ptEnd, fDir)
                    pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
                    pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

                    pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
                    pAngleUse.AlongCourseGuidance = False

                    pPointReference.FacilityAngle.Add(pAngleUse)
                    pPointReference.Role = CodeReferenceRole.INTERSECTION
                End If
            End If

            pSegmentPoint.PointChoice = pFIXSignPt
            pEndPoint.FacilityMakeup.Add(pPointReference)
        End If

        pSegmentLeg.EndPoint = pEndPoint

        ' End of EndPoint ========================

        ' Trajectory ===========================================
        Dim pPolyline As IGeometryCollection = segment.PathPrj
        Dim pCurve As Curve = New Curve()

        For j As Integer = 0 To pPolyline.GeometryCount - 1
            Dim pPath As IPointCollection = pPolyline.Geometry(j)
            Dim pLineStringSegment As LineString = New LineString()

            For i As Integer = 0 To pPath.PointCount - 1
                Dim pLocation As Aran.Geometries.Point = Converters.ESRIPointToARANPoint(Functions.ToGeo(pPath.Point(i)))
                pLineStringSegment.Add(pLocation)
            Next
            pCurve.Geo.Add(pLineStringSegment)
        Next
        pSegmentLeg.Trajectory = pCurve

        ' Trajectory =============================================
        ' Protection Area ========================================
        'DrawPolygon(pMAStraightPrimPoly, RGB(128, 128, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
        'DrawPolygon(pMAStraightSecPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)

        'DrawPolygon(pMAStraightSecLPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
        'DrawPolygon(pMAStraightSecRPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
        'DrawPolyLine(pCutter, 255, 2)
        'Application.DoEvents()

        Dim pPolygon As IPolygon = segment.pProtectArea
        Dim pSurface As Surface
        Dim pPrimProtectedArea As ObstacleAssessmentArea

        pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

        pPrimProtectedArea = New ObstacleAssessmentArea
        pPrimProtectedArea.Surface = pSurface
        pPrimProtectedArea.SectionNumber = 0
        pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

        pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)

        'Dim pSecProtectedArea As ObstacleAssessmentArea
        'pSurface = ESRIPolygonToAIXMSurface(ToGeo(pMAStraightSecRPoly))

        'pSecProtectedArea = New ObstacleAssessmentArea
        'pSecProtectedArea.Surface = pSurface
        'pSecProtectedArea.SectionNumber = 1
        'pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

        'pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

        ' END ====================================================

        Return result
    End Function

	Private Function SaveProcedure(sProcName As String) As Boolean
		Dim NO_SEQ As Integer

		'Dim sProcName As String

		Dim pTransition As ProcedureTransition
		Dim pLandingTakeoffAreaCollection As LandingTakeoffAreaCollection
		Dim pEndPoint As TerminalSegmentPoint = Nothing
		Dim IsLimitedTo As AircraftCharacteristic
		Dim featureRef As FeatureRef
		Dim featureRefObject As FeatureRefObject
		Dim pGuidanceServiceChose As New GuidanceService

		Dim HaveTP As Boolean
		Dim HaveIntercept As Boolean
		Dim pSegmentLeg As SegmentLeg
		Dim ptl As ProcedureTransitionLeg

		Dim uomDistHorzTab() As UomDistance
		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomSpeedTab() As UomSpeed

		uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT}   ', UomDistanceVertical.OTHER, UomDistanceVertical.OTHER, UomDistanceVertical.OTHER
		uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		pObjectDir.ClearAllFeatures()

		'Dim ProcCat() As String
		'ProcCat = New String() {"CAT_I", "CAT_II", "CAT_II_AUTO"}
		'sProcName = "ILS_" + ProcCat(ComboBox0002.SelectedIndex) + "_RWY" + ComboBox0001.Text + "_CAT_" + ComboBox0003.Text

		HaveTP = CheckBox0301.Checked
		HaveIntercept = OptionButton0602.Checked

		pProcedure = DBModule.pObjectDir.CreateFeature(Of InstrumentApproachProcedure)()
		' Procedure =================================================================================================
		pLandingTakeoffAreaCollection = New LandingTakeoffAreaCollection
		pLandingTakeoffAreaCollection.Runway.Add(SelectedRWY.GetFeatureRefObject())

		pProcedure.RNAV = False
		pProcedure.FlightChecked = False
		pProcedure.CodingStandard = Aran.Aim.Enums.CodeProcedureCodingStandard.PANS_OPS
		pProcedure.DesignCriteria = Aran.Aim.Enums.CodeDesignStandard.PANS_OPS
		pProcedure.Landing = pLandingTakeoffAreaCollection
		pProcedure.Name = sProcName
		'pProcedure.CommunicationFailureDescription
		'pProcedure.Description =
		'pProcedure.ID
		'pProcedure.Note =
		'pProcedure.ProtectsSafeAltitudeAreaId = `

		IsLimitedTo = New AircraftCharacteristic

		Select Case Category
			Case 0
				IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.A
			Case 1
				IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.B
			Case 2
				IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.C
			Case 3
				IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.D
			Case 4
				IsLimitedTo.AircraftLandingCategory = Aran.Aim.Enums.CodeAircraftCategory.E
		End Select

		pProcedure.AircraftCharacteristic.Add(IsLimitedTo)

		featureRefObject = New FeatureRefObject
		featureRef = New FeatureRef
		featureRef.Identifier = CurrADHP.pAirportHeliport.Identifier
		featureRefObject.Feature = featureRef
		pProcedure.AirportHeliport.Add(featureRefObject)

		'ILS.GetSignificantPoint()
		pGuidanceServiceChose.Navaid = ILS.GetFeatureRef()
		pProcedure.GuidanceFacility.Add(pGuidanceServiceChose)

		gAranEnv.GetLogger("Precision Arrival").Info("Create transition")
		'Transition =========================================================================================
		pTransition = New ProcedureTransition
		'	pTransition.Description =
		'	pTransition.ID =
		'	pTransition.Procedure =
		'pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection
		pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.FINAL


		'Legs ===============================================================================================
		'    Set pSegmentLegList = New SegmentLegComList
		NO_SEQ = 0

		gAranEnv.GetLogger("Precision Arrival").Info("Leg 1 Intermediate Approach")
		'Leg 1 Intermediate Approach ========================================================================
		NO_SEQ = NO_SEQ + 1
		pSegmentLeg = IntermediateApproachLeg(pProcedure, IsLimitedTo, pEndPoint)

		ptl = New ProcedureTransitionLeg()
		ptl.SeqNumberARINC = NO_SEQ
		ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
		pTransition.TransitionLeg.Add(ptl)

		gAranEnv.GetLogger("Precision Arrival").Info("Leg 2 Final Approach")
		'Leg 2 Final Approach ===============================================================================
		NO_SEQ = NO_SEQ + 1
		pSegmentLeg = FinalApproachLeg(pProcedure, IsLimitedTo, pEndPoint)

		ptl = New ProcedureTransitionLeg()
		ptl.SeqNumberARINC = NO_SEQ
		ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
		pTransition.TransitionLeg.Add(ptl)

		gAranEnv.GetLogger("Precision Arrival").Info("Leg 3 Straight Missed Approach")
		'Leg 3 Straight Missed Approach =====================================================================
		If HaveTP Then
			NO_SEQ = NO_SEQ + 1
			pSegmentLeg = StraightMissedApproachLeg(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		gAranEnv.GetLogger("Precision Arrival").Info("Leg 4 Missed Approach Termination")
		'Leg 4 Missed Approach Termination ==================================================================
		NO_SEQ = NO_SEQ + 1
		pSegmentLeg = MissedApproachTermination(pProcedure, IsLimitedTo, pEndPoint)

		ptl = New ProcedureTransitionLeg()
		ptl.SeqNumberARINC = NO_SEQ
		ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
		pTransition.TransitionLeg.Add(ptl)

		gAranEnv.GetLogger("Precision Arrival").Info("Leg 5 Missed Approach Termination Continued")
		'Leg 5 Missed Approach Termination Continued ========================================================
		If HaveIntercept Then
			NO_SEQ = NO_SEQ + 1
			pSegmentLeg = MissedApproachTerminationContinued(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		'====================================================================================================
		'==============================================
		Dim i As Integer

		If CheckBox0901.Checked Then
			'Dim pEndPoint As TerminalSegmentPoint = Nothing		  '??????????????????????????????????
			'  End Of Start Point ====================================================================================
			i = 1

			Do While (i < TSC)
				Dim currSegment As TraceSegment = Trace(i)
				If (i < TSC - 1) Then
					Dim nextSegment As TraceSegment = Trace(i + 1)
					If (nextSegment.SegmentCode = eSegmentType.straight) And (currSegment.LegType = CodeSegmentPath.DF) Then
						'If (nextSegment.SegmentCode = eSegmentType.straight) And
						'	(((i = 0) And (currSegment.LegType = CodeSegmentPath.VA)) Or
						'	 (currSegment.LegType = CodeSegmentPath.DF)) Then
						nextSegment.HStart = currSegment.HStart

						nextSegment.ptIn = currSegment.ptIn
						nextSegment.DirIn = currSegment.DirIn
						nextSegment.Length += currSegment.Length

						nextSegment.StCoords = currSegment.StCoords

						If (Not (nextSegment.PathPrj Is Nothing)) And (Not (currSegment.PathPrj Is Nothing)) Then

							Dim pTopo As ITopologicalOperator2 = currSegment.PathPrj
							pTopo.IsKnownSimple_2 = False
							pTopo.Simplify()

							nextSegment.PathPrj = pTopo.Union(nextSegment.PathPrj)

							pTopo = nextSegment.PathPrj
							pTopo.IsKnownSimple_2 = False
							pTopo.Simplify()

							If Not nextSegment.PathPrj.IsEmpty Then
								nextSegment.pProtectArea = pTopo.Buffer(nextSegment.SeminWidth)
								pTopo = nextSegment.pProtectArea
								pTopo.IsKnownSimple_2 = False
								pTopo.Simplify()
							End If
						End If
						currSegment = nextSegment
						i += 1
					End If
				End If

				pSegmentLeg = MissedApproachLegs(currSegment, pProcedure, IsLimitedTo, pEndPoint)

				ptl = New ProcedureTransitionLeg()
				NO_SEQ += 1
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)

				i += 1
			Loop

		End If

		pProcedure.FlightTransition.Add(pTransition)

		gAranEnv.GetLogger("Precision Arrival").Info("IAPProfile")
		'IAPProfile =========================================================================================
		Dim pIAPProfile As FinalProfile
		Dim pApproachAltitudeTable As ApproachAltitudeTable
		Dim pApproachDistanceTable As ApproachDistanceTable

		pIAPProfile = New FinalProfile

		Dim distance As Double
		Dim tmpY As Double

		'===================================================================================================================================
		PrjToLocal(FicTHRprj, _ArDir + 180.0, ptFAP, distance, tmpY)
		pApproachDistanceTable = New ApproachDistanceTable
		pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.PFAF
		pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.THLD
		pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
		pIAPProfile.Distance.Add(pApproachDistanceTable)

		pApproachAltitudeTable = New ApproachAltitudeTable
		pApproachAltitudeTable.AltitudeReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.PFAF
		pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(ptFAP.Z + FicTHRprj.Z, eRoundMode.NEAREST))
		pIAPProfile.Altitude.Add(pApproachAltitudeTable)
		'===================================================================================================================================
		PrjToLocal(FicTHRprj, _ArDir + 180.0, pMAPt, distance, tmpY)
		pApproachDistanceTable = New ApproachDistanceTable
		pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.MAP
		pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.THLD
		pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
		pIAPProfile.Distance.Add(pApproachDistanceTable)

		pApproachAltitudeTable = New ApproachAltitudeTable
		pApproachAltitudeTable.AltitudeReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.MAP
		pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(pMAPt.Z + FicTHRprj.Z, eRoundMode.NEAREST))
		pIAPProfile.Altitude.Add(pApproachAltitudeTable)

		pProcedure.FinalProfile = pIAPProfile
		'pObjectDir.Store(pProcedure)
		Dim Result As Boolean

		Try
			Dim commitedFeatures As List(Of FeatureType) = New List(Of FeatureType) From {
					FeatureType.DesignatedPoint,
					FeatureType.AngleIndication,
					FeatureType.DistanceIndication,
					FeatureType.ArrivalFeederLeg,
					FeatureType.ArrivalLeg,
					FeatureType.DepartureLeg,
					FeatureType.IntermediateLeg,
					FeatureType.FinalLeg,
					FeatureType.MissedApproachLeg,
					FeatureType.StandardInstrumentDeparture,
					FeatureType.StandardInstrumentArrival,
					FeatureType.InstrumentApproachProcedure}

			Dim metadataFeatures As List(Of FeatureType) = New List(Of FeatureType) From {FeatureType.InstrumentApproachProcedure}

			pObjectDir.SetRootFeatureType(FeatureType.InstrumentApproachProcedure)

			gAranEnv.GetLogger("Precision Arrival").Info("Commit start")
			If gAranEnv.ConnectionInfo.ConnectionType = ConnectionType.TDB And gAranEnv.UseWebApi Then
				Result = pObjectDir.CommitWithMetadataViewer(
					gAranEnv.Graphics.ViewProjection.Name,
					commitedFeatures.ToArray(),
					metadataFeatures.ToArray(),
					GetNumericalData(),
					False)
			Else
				Result = pObjectDir.Commit(commitedFeatures.ToArray())
			End If

			gAranEnv.RefreshAllAimLayers()

			gAranEnv.GetLogger("Precision Arrival").Info("Precision Arrival Procedure Save/Commit End")
		Catch ex As Exception
			gAranEnv.GetLogger("Precision Arrival").Error(ex, "Save procedure")
			MsgBox("Error on commit." + vbCrLf + ex.Message)
			Result = False
		End Try

		If (Not Result) Then
			pObjectDir.ClearAllFeatures()
		End If

		Return Result
	End Function

	'
	' All distance measured from start point of TIA
	'
	Sub CalcReducedTNH(ByRef ObstList As ObstacleContainer, ByVal GPA As Double, ByVal MAPDG As Double, ByVal HL As Double, ByRef MinOCH As Double, ByRef MinTNH As Double, ByRef IxMinOCH As Integer)
        Dim I As Integer
        Dim N As Integer
        Dim fTmp As Double
        Dim xSOC As Double
        Dim xSOC1 As Double
        Dim xTNH As Double
        Dim xTNH1 As Double
        Dim NewTNH As Double

        Dim CoTanZ As Double
        Dim CoTanGPA As Double
        Dim bDone As Boolean
        Dim ZSurfaceOrigin As Double

        bHaveSolution = True

        ZSurfaceOrigin = OASZOrigin
        If GPA > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (GPA - MaxRefGPAngle))

        CoTanZ = 1.0 / MAPDG
        CoTanGPA = 1.0 / System.Math.Tan(DegToRad(GPA))
        IxMinOCH = -1
        N = UBound(ObstList.Parts)
        bDone = True

        Do
            fTmp = MinOCH - HL
            NewTNH = IIf(fTmp > MinTNH, fTmp, MinTNH)

            xSOC = XptLH - (MinOCH - HL) * CoTanGPA + ZSurfaceOrigin
            fTmp = (MinTNH - MinOCH + HL)
            If fTmp < 0.0 Then fTmp = 0.0
            xTNH = xSOC + fTmp * CoTanZ

            For I = 0 To N
                If MinOCH > 300.0 Then
                    bHaveSolution = False
                    MessageBox.Show(My.Resources.str00303)
                    Return
                End If

                If (Not ObstList.Obstacles(ObstList.Parts(I).Owner).IgnoredByUser) And (ObstList.Parts(I).Dist <= xTNH) Then
                    If (ObstList.Parts(I).fTmp < NewTNH) And (ObstList.Parts(I).ReqH > NewTNH) Then
                        NewTNH = ObstList.Parts(I).ReqH
                        If NewTNH >= 300.0 Then
                            MinOCH = MinOCH + 1.0
                            IxMinOCH = -1
                            Exit Do
                        End If

                        fTmp = (NewTNH - MinOCH + HL)
                        If fTmp < 0.0 Then fTmp = 0.0
                        xTNH = xSOC + fTmp * CoTanZ
                    End If

                    If ObstList.Parts(I).ReqOCH > MinOCH Then
                        xSOC1 = XptLH - (ObstList.Parts(I).ReqOCH - HL) * CoTanGPA + ZSurfaceOrigin

                        fTmp = (MinTNH - ObstList.Parts(I).ReqOCH + HL)
                        If fTmp < 0.0 Then fTmp = 0.0

                        xTNH1 = xSOC1 + fTmp * CoTanZ

                        If ObstList.Parts(I).Dist <= xTNH1 Then
                            MinOCH = ObstList.Parts(I).ReqOCH
                            IxMinOCH = I
                            Exit Do
                        End If
                    End If
                End If
            Next I
        Loop While Not bDone

        MinTNH = NewTNH
        XptSOC = xSOC
    End Sub

    Function PrelimOCH4Reduced(ByRef ObstList As ObstacleContainer, ByVal HL As Double, ByVal GPA As Double, ByRef IxMin As Integer) As Double
        Dim I As Integer
        Dim N As Integer
        Dim xSOC As Double
        Dim CoTanGPA As Double
        Dim ZSurfaceOrigin As Double
        Dim Result As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        Result = HL
        CoTanGPA = 1.0 / System.Math.Tan(DegToRad(GPA))

        N = UBound(ObstList.Parts)
        For I = 0 To N
            If (Not ObstList.Obstacles(ObstList.Parts(I).Owner).IgnoredByUser) And (ObstList.Parts(I).ReqOCH > Result) Then
                xSOC = XptLH - (ObstList.Parts(I).ReqOCH - HL) * CoTanGPA + ZSurfaceOrigin
                If ObstList.Parts(I).Dist < xSOC Then
                    Result = ObstList.Parts(I).ReqOCH
                    IxMin = I
                End If
            End If
        Next I

        Return Result
    End Function

    Private Sub CalcTAParams()
        Dim ptAreaEnd As ESRI.ArcGIS.Geometry.IPoint
        Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

        If OptionButton0401.Checked Then
            pProxi = ZNR_Poly
        Else
            pProxi = KK
        End If

        pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)
        CircleVectorIntersect(CurrADHP.pPtPrj, RModel, pTurnComplitPt, pTurnComplitPt.M, ptAreaEnd)

        TurnAreaMaxd0 = pProxi.ReturnDistance(ptAreaEnd)
        If OptionButton0603.Checked Then
            TurnAreaMaxd0 = Functions.ReturnDistanceInMeters(ptAreaEnd, TurnDirector.pPtPrj)
        End If

        'If OptionButton0603.Checked Then
        '	d0 = pProxi.ReturnDistance(TurnWPT.pPtPrj)
        'Else
        '	d0 = pProxi.ReturnDistance(pTurnComplitPt)
        'End If

        'If TurnAreaMaxd0 <= d0 Then TurnAreaMaxd0 = d0
        'N = UBound(TurnAreaObstacleList.Parts)
        'For I = 0 To N
        '	If TurnAreaMaxd0 < TurnAreaObstacleList.Parts(I).DistStar Then TurnAreaMaxd0 = TurnAreaObstacleList.Parts(I).DistStar
        'Next I

        TextBox0901.Text = ConvertDistance(arBufferMSA.Value, eRoundMode.NEAREST).ToString()

        If OptionButton0602.Checked Then
            Dim I As Integer
            Dim N As Integer

            Dim DistMin As Double
            Dim DistMax As Double
            DistMin = 0.0
            DistMax = ReturnDistanceInMeters(ptAreaEnd, pTurnComplitPt)
            pTurnComplitPt.Z = TurnFixPnt.Z

            TerInterNavDat = GetValidIFNavs(pTurnComplitPt, FicTHRprj.Z, DistMin, DistMax, pTurnComplitPt.M + 180.0, fMissAprPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj)

            ComboBox0902.Items.Clear()
            N = UBound(TerInterNavDat)

            For I = 0 To N
                ComboBox0902.Items.Add(TerInterNavDat(I))
            Next I

            'DrawPointWithText(pTurnComplitPt, "PtTurnEnd")
            'DrawPointWithText(TurnWPT.pPtPrj, "TurnWPT")


            If SideDef(pTurnComplitPt, pTurnComplitPt.M + 90, TurnDirector.pPtPrj) > 0 Then
                N = N + 1
                ReDim Preserve TerInterNavDat(N)
                TerInterNavDat(N) = WPT_FIXToNavaid(TurnDirector)
                TerInterNavDat(N).ValCnt = -2
                TerInterNavDat(N).IntersectionType = eIntersectionType.OnNavaid

                ComboBox0902.Items.Add(TurnDirector)
            End If

            If N < 0 Then
                Dim Side0 As Integer
                Dim Side1 As Integer
                Dim Dist0 As Double
                Dim Dist1 As Double
                Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
                Dim pt1 As ESRI.ArcGIS.Geometry.IPoint

                ReDim TerInterNavDat(0)
                If SideDef(pTurnComplitPt, pTurnComplitPt.M + 90, TurnDirector.pPtPrj) > 0 Then
                    TerInterNavDat(0).pPtGeo = ToGeo(pTurnComplitPt)
                    TerInterNavDat(0).pPtPrj = pTurnComplitPt
                Else
                    TerInterNavDat(0).pPtGeo = TurnDirector.pPtGeo
                    TerInterNavDat(0).pPtPrj = TurnDirector.pPtPrj
                End If

                TerInterNavDat(0).CallSign = "Radar FIX"
                TerInterNavDat(0).Name = "Radar FIX"
                TerInterNavDat(0).Identifier = Guid.Empty
                TerInterNavDat(0).NAV_Ident = Guid.Empty
                TerInterNavDat(0).TypeCode = eNavaidType.RadarFIX
                TerInterNavDat(0).MagVar = TurnDirector.MagVar
                TerInterNavDat(0).Range = DME.Range
                TerInterNavDat(0).IntersectionType = eIntersectionType.RadarFIX
                '===============================================================

                pt0 = PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, DistMin)
                pt1 = PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, TurnAreaMaxd0)

                Side0 = SideDef(pt0, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj)
                Side1 = SideDef(pt1, pTurnComplitPt.M + 90.0, TurnDirector.pPtPrj)

                Dist0 = ReturnDistanceInMeters(pt0, TurnDirector.pPtPrj)
                Dist1 = ReturnDistanceInMeters(pt1, TurnDirector.pPtPrj)

                If Side0 <> Side1 Then
                    TerInterNavDat(0).ValCnt = 0
                    ReDim TerInterNavDat(0).ValMax(1)
                    ReDim TerInterNavDat(0).ValMin(1)

                    TerInterNavDat(0).ValMin(0) = 1
                    TerInterNavDat(0).ValMin(1) = 1
                    If Side0 > 0 Then
                        TerInterNavDat(0).ValMax(0) = Dist0
                        TerInterNavDat(0).ValMax(1) = Dist1
                    Else
                        TerInterNavDat(0).ValMax(0) = Dist1
                        TerInterNavDat(0).ValMax(1) = Dist0
                    End If
                Else
                    TerInterNavDat(0).ValCnt = Side0
                    ReDim TerInterNavDat(0).ValMax(0)
                    ReDim TerInterNavDat(0).ValMin(0)

                    TerInterNavDat(0).ValMin(0) = Min(Dist0, Dist1)
                    TerInterNavDat(0).ValMax(0) = Max(Dist0, Dist1)
                End If
                '===============================================================
                ComboBox0902.Items.Add("Radar FIX")
            End If
            ComboBox0902.SelectedIndex = 0
        Else
            TerminationOCH()
        End If
    End Sub

    Private Sub TerminationOCH()
        Dim I As Integer

        Dim fCurrX As Double
        Dim fCurrY As Double
        Dim dBuffer As Double

        Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
        Dim DistPoly As ESRI.ArcGIS.Geometry.IGeometry
        Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection

        If OptionButton0401.Checked Then
            DistPoly = ZNR_Poly
        Else
            DistPoly = KK
        End If

        If Not Double.TryParse(TextBox0901.Text, dBuffer) Then
            dBuffer = -1.0
        Else
            dBuffer = Functions.DeConvertDistance(dBuffer)
        End If

        If dBuffer > TurnAreaMaxd0 Then dBuffer = TurnAreaMaxd0
        If dBuffer < arBufferMSA.Value Then dBuffer = arBufferMSA.Value

        pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)

        mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
        pPolyline = mPoly
        pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

        If OptionButton0601.Checked Then
            Dim pClone As ESRI.ArcGIS.esriSystem.IClone
            Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline
            Dim pBufferPolygon As ESRI.ArcGIS.Geometry.IPolygon
            Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

            pTopo = DistPoly
            pBufferPolygon = pTopo.Buffer(dBuffer)
            pTopo = pBufferPolygon
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pClone = mPoly
            pPointCollection = pClone.Clone
            pPointCollection.AddPoint(PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, 10.0 * RModel))

            pPoly = pTopo.Intersect(pPointCollection, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
            pPointCollection = pPoly
            ptCnt = pTurnComplitPt

            For I = 0 To pPointCollection.PointCount - 1
                PrjToLocal(ptCnt, m_OutDir, pPointCollection.Point(I), fCurrX, fCurrY)
                If Math.Abs(fCurrY) > distEps Then Continue For
                If fCurrX > 0.0 Then ptCnt = pPointCollection.Point(I)
            Next

            'If Not pPoly.IsEmpty() Then
            '	If ReturnDistanceInMeters(pPoly.FromPoint, mPoly.FromPoint) > distEps Then pPoly.ReverseOrientation()
            '	ptCnt = pPoly.ToPoint
            'End If
        ElseIf OptionButton0602.Checked Then
            ptCnt = TerFixPnt
        Else 'If OptionButton0603.Checked Then
            ptCnt = TurnDirector.pPtPrj
        End If

        pPath.AddPoint(ptCnt)
        pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
        pPolyline.AddGeometry(pPath)

        If Not OptionButton0603.Checked Then
            For I = 0 To m_BasePoints.PointCount - 1
                PrjToLocal(ptCnt, m_OutDir, m_BasePoints.Point(I), fCurrX, fCurrY)
                If fCurrX > 0.0 Then ptCnt = m_BasePoints.Point(I)
            Next

            PrjToLocal(ptCnt, m_OutDir, TurnFixPnt, fCurrX, fCurrY)
            If fCurrX > 0.0 Then ptCnt = TurnFixPnt
        End If

        ' =====================================================================================
        Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
        Dim pFullArea As ESRI.ArcGIS.Geometry.IPolygon
        Dim pTmpPoly As ESRI.ArcGIS.Geometry.IGeometry
        Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

        pCutter = New ESRI.ArcGIS.Geometry.Polyline()

        If OptionButton0601.Checked Then
            pCutter.FromPoint = Functions.LocalToPrj(ptCnt, m_OutDir, 0.0, 100000.0)    '2.0 * GlobalVars.RModel
            pCutter.ToPoint = Functions.LocalToPrj(ptCnt, m_OutDir, 0.0, -100000.0)
        Else
            pCutter.FromPoint = Functions.LocalToPrj(ptCnt, m_OutDir, dBuffer, 100000.0)    '2.0 * GlobalVars.RModel
            pCutter.ToPoint = Functions.LocalToPrj(ptCnt, m_OutDir, dBuffer, -100000.0)
        End If

        pRelation = pCutter

        If (pRelation.Disjoint(BaseArea) Or pRelation.Disjoint(pCircle)) Then
            pFullArea = BaseArea
        Else
            pTopo = BaseArea
            pTopo.Cut(pCutter, pTmpPoly, pFullArea)
        End If

        ' =====================================================================================
        Dim IxDh As Integer
        Dim CurrTNH As Double
        Dim TurnAngle As Double

        CurrTNH = TurnFixPnt.Z - FicTHRprj.Z
        TurnAngle = CDbl(TextBox0601.Text)

        If OptionButton0401.Checked Then
            If CheckBox0402.Checked Then
                GetReducedTurnAreaObstacles(ObstacleList, TurnAreaObstacleList, pFullArea, m_BasePoints, pDistPolygon, KK, TurnAngle, CurrTNH, fMissAprPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj, SecPoly, m_OutDir, TurnDir, EarlierTPDir, _ArDir, FicTHRprj, OASPlanes, IxDh)
            Else
                GetTurnAreaObstacles(ObstacleList, TurnAreaObstacleList, pFullArea, ZNR_Poly, TurnAngle, CurrTNH, fMissAprPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj, SecPoly, m_OutDir, IxDh)
            End If
        Else
            GetTurnAreaObstacles(ObstacleList, TurnAreaObstacleList, pFullArea, KK, TurnAngle, CurrTNH, fMissAprPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj, SecPoly, m_OutDir, IxDh)
        End If
        '=============================================================================================================================================================================================================
        Dim N As Integer
        Dim IxMaxReq As Integer

        Dim MaxDh As Double
        Dim MaxReq As Double
        Dim CurrReq As Double
        Dim fEnrouteMOC As Double

        fEnrouteMOC = DeConvertHeight(CDbl(ComboBox0402.Text))
        IxDh = -1
        IxMaxReq = -1

        MaxDh = 0.0
        MaxReq = fEnrouteMOC

        N = UBound(TurnAreaObstacleList.Parts)

        For I = 0 To N
            If TurnAreaObstacleList.Parts(I).hPenet > MaxDh Then
                MaxDh = TurnAreaObstacleList.Parts(I).hPenet
                IxDh = I
            End If

            CurrReq = TurnAreaObstacleList.Parts(I).Height + TurnAreaObstacleList.Parts(I).fTmp * fEnrouteMOC
            If MaxReq < CurrReq Then
                MaxReq = CurrReq
                IxMaxReq = I
            End If
        Next I

        MaxReq = MaxReq + FicTHRprj.Z
        PrecReportFrm.FillPage09(TurnAreaObstacleList, IxDh)
        '=============================================================================================================================================================================================================
        Dim d0 As Double
        Dim fAltKK As Double
        Dim fReachedA As Double
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

        pProxi = DistPoly
        If OptionButton0601.Checked Then
            d0 = dBuffer
        Else
            d0 = pProxi.ReturnDistance(ptCnt)
        End If

        fAltKK = TurnFixPnt.Z - FicTHRprj.Z + MaxDh
        fReachedA = fAltKK + d0 * fMissAprPDG + FicTHRprj.Z
        '*********************************************************************************
        Dim pIZAware As IZAware
        pIZAware = mPoly
        pIZAware.ZAware = True

        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pIZ As IZ

        ptTmp = mPoly.ToPoint
        ptTmp.Z = MaxReq - FicTHRprj.Z
        mPoly.ToPoint = ptTmp

        pIZ = mPoly
        pIZ.CalculateNonSimpleZs()

        '*********************************************************************************

        TextBox0903.Text = CStr(ConvertHeight(fAltKK + FicTHRprj.Z, eRoundMode.NEAREST))
        TextBox0904.Text = CStr(ConvertHeight(MaxReq, eRoundMode.NEAREST))
        TextBox0908.Text = TextBox0904.Text
        TextBox0902.Text = CStr(ConvertHeight(fReachedA, eRoundMode.NEAREST))

        If MaxReq > fReachedA Then
            TextBox0908.ReadOnly = True
            TextBox0908.BackColor = System.Drawing.SystemColors.Control
            OkBtnEnabled = False
        Else
            TextBox0908.ReadOnly = False
            TextBox0908.BackColor = System.Drawing.SystemColors.Window
            OkBtnEnabled = True
        End If

        If IxDh > -1 Then
            TextBox0907.Text = TurnAreaObstacleList.Obstacles(TurnAreaObstacleList.Parts(IxDh).Owner).UnicalName
        Else
            TextBox0907.Text = "-"
        End If

        If IxMaxReq > -1 Then
            TextBox0905.Text = TurnAreaObstacleList.Obstacles(TurnAreaObstacleList.Parts(IxMaxReq).Owner).UnicalName
        Else
            TextBox0905.Text = "-"
        End If

        OkBtnEnabled = True '?????????????????????????????????????????????????????????
        '==================================================================
        Dim dD As Double
        Dim CoTanZ As Double
        Dim CoTanGPA As Double
        Dim fMAPtDist As Double
        Dim DistTHR_KK As Double
        Dim ZSurfaceOrigin As Double

        ZSurfaceOrigin = OASZOrigin
        If ILS.GPAngle > MaxRefGPAngle Then ZSurfaceOrigin = (OASZOrigin + 500.0 * (ILS.GPAngle - MaxRefGPAngle))

        CoTanGPA = 1.0 / System.Math.Tan(DegToRad(ILS.GPAngle))
        CoTanZ = 1.0 / fMissAprPDG
        DistTHR_KK = Point2LineDistancePrj(FicTHRprj, KK.FromPoint, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, KK.FromPoint)

        fTA_OCH = (fAltKK * CoTanZ + (DistTHR_KK + ZSurfaceOrigin)) / (CoTanZ + CoTanGPA) + m_fMOC
        If ComboBox0903.SelectedIndex < 0 Then
            ComboBox0903.SelectedIndex = 0
        Else
            ComboBox0903_SelectedIndexChanged(ComboBox0903, New System.EventArgs())
        End If

        N = ArrivalProfile.MAPtIndex - 1
        While ArrivalProfile.PointsNo > N
            ArrivalProfile.RemovePoint()
        End While

        fMAPtDist = (fTA_OCH - ILS.GP_RDH) / System.Math.Tan(DegToRad(ILS.GPAngle))

        ArrivalProfile.AddPoint(fMAPtDist, fTA_OCH, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), CodeProcedureDistance.MAP)

        dD = Point2LineDistancePrj(FicTHRprj, TurnFixPnt, _ArDir + 90.0) * SideDef(FicTHRprj, _ArDir - 90.0, TurnFixPnt)
        ArrivalProfile.AddPoint(dD, fAltKK, ILS.Course - ILS.MagVar, RadToDeg(System.Math.Atan(fMissAprPDG)), -1, 1)
        '==================================================================

        On Error Resume Next
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        On Error GoTo 0

        NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        pGraphics.AddElement(NomTrackElem, 0)
        NomTrackElem.Locked = True
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        TextBox0910.Text = CStr(ConvertDistance(mPoly.Length, eRoundMode.NEAREST))
        TextBox0911.Text = CStr(ConvertHeight(TurnFixPnt.Z + mPoly.Length * fMissAprPDG, eRoundMode.NEAREST))
    End Sub

    Private Sub DrawTrail()
        Dim d0 As Double
        Dim fTmpDist As Double
        Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint
        Dim pBufferPolygon As ESRI.ArcGIS.Geometry.IPolygon
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

        mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
        pPolyline = mPoly
        pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

        If OptionButton0603.Checked Then
            pPath.AddPoint(TurnDirector.pPtPrj)
        Else
            pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)

            If OptionButton0401.Checked Then
                pTopo = ZNR_Poly
            Else
                pTopo = KK
            End If

            pProxi = pTopo
            fTmpDist = pProxi.ReturnDistance(pTurnComplitPt) + 0.5

            d0 = (hTMA + FicTHRprj.Z - TurnFixPnt.Z) / fMissAprPDG
            If d0 <= fTmpDist Then d0 = fTmpDist

            pBufferPolygon = pTopo.Buffer(d0)

            pTopo = pBufferPolygon
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()

            pClone = mPoly
            pPointCollection = pClone.Clone
            pPointCollection.AddPoint(PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, 10 * RModel))

            pPoly = pTopo.Intersect(pPointCollection, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
            pPolyline = pPoly

            If pPolyline.GeometryCount = 1 Then
                If ReturnDistanceInMeters(pPoly.FromPoint, mPoly.FromPoint) > distEps Then pPoly.ReverseOrientation()
                pPath.AddPoint(pPoly.ToPoint)
            End If
            pPolyline = mPoly
        End If
        '========================
        pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
        pPolyline.AddGeometry(pPath)
        '=========================

        On Error Resume Next
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        On Error GoTo 0

        NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        pGraphics.AddElement(NomTrackElem, 0)
        NomTrackElem.Locked = True
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        TextBox0910.Text = CStr(ConvertDistance(mPoly.Length, eRoundMode.NEAREST))
        TextBox0911.Text = CStr(ConvertHeight(TurnFixPnt.Z + mPoly.Length * fMissAprPDG, eRoundMode.NEAREST))
    End Sub

    Private Function CreateTurnArea(ByRef WSpeed As Double, ByRef TurnR As Double, ByRef V As Double, ByRef AztEnter As Double, ByRef AztOut As Double, ByRef iTurnDir As Integer, ByRef BasePoints As ESRI.ArcGIS.Geometry.IPointCollection) As ESRI.ArcGIS.Geometry.IPointCollection
        Dim Constructor As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim TmpSpiral As ESRI.ArcGIS.Geometry.IPointCollection
        Dim PtIntersect As ESRI.ArcGIS.Geometry.IPoint

        Dim Rv As Double
        Dim dAng As Double
        Dim Bank As Double
        Dim coef As Double
        Dim azt12 As Double
        Dim dAng0 As Double
        Dim AztCur As Double
        Dim AztNext As Double
        Dim TurnAng As Double
        Dim wAztOut As Double

        Dim I As Integer
        Dim N As Integer
        Dim K As Integer

        Bank = Radius2Bank(TurnR, V)
        Rv = 6355.0 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
        If (Rv > 3.0) Then
            Rv = 3.0
        End If
        coef = WSpeed / Rv

        AztOut = Modulus(AztOut, 360.0)
        wAztOut = AztOut

        AztEnter = Modulus(AztEnter, 360.0)

        If SubtractAngles(wAztOut, AztEnter) < 1.0 Then
            TurnAng = 0.0
        Else
            TurnAng = Modulus((wAztOut - AztEnter) * iTurnDir, 360.0)
        End If

        CreateTurnArea = New ESRI.ArcGIS.Geometry.Polygon

        AztCur = AztEnter
        N = BasePoints.PointCount
        K = 0

        Dim ThetaTouch As Double
        I = 0
        AztNext = ReturnAngleInDegrees(BasePoints.Point(0), BasePoints.Point(1))
        TmpSpiral = New ESRI.ArcGIS.Geometry.Polyline

        Do
            '======================================
            If OptionButton0603.Checked And CheckBox0601.Checked Then
                ThetaTouch = FixToTouchSpiral(BasePoints.Point(I), coef, TurnR, iTurnDir, AztOut, TurnDirector.pPtPrj, _ArDir)
                If ThetaTouch < -370.0 Then
                    MessageBox.Show(My.Resources.str00303)  '"С заданными параметрами разворота выйти в указанную точку невозможно.", vbCritical

                    wAztOut = AztOut
                Else
                    wAztOut = ThetaTouch
                End If

                If SubtractAngles(wAztOut, AztEnter) < 1.0 Then
                    TurnAng = 0.0
                Else
                    TurnAng = Modulus((wAztOut - AztEnter) * iTurnDir, 360.0)
                End If
            End If
            '======================================
            azt12 = ReturnAngleInDegrees(BasePoints.Point(I), BasePoints.Point((I + 1) Mod N))

            If System.Math.Abs(Modulus(azt12, 360.0) - Modulus(AztCur, 360.0)) < 1.0 Then
                AztNext = azt12
            ElseIf SideFrom2Angle(AztNext, azt12) * TurnDir < 0 Then
                dAng0 = Modulus((AztCur - azt12) * iTurnDir, 360.0)
                dAng = dAng - dAng0
                AztNext = azt12

                CreateWindSpiral(BasePoints.Point(I), AztEnter, azt12 - 90.0 * TurnDir, azt12, TurnR, coef, iTurnDir, TmpSpiral)

                If TmpSpiral.PointCount > 0 Then
                    PtIntersect = New ESRI.ArcGIS.Geometry.Point
                    Constructor = PtIntersect
                    Constructor.ConstructAngleIntersection(CreateTurnArea.Point(CreateTurnArea.PointCount - 1), DegToRad(AztCur), TmpSpiral.Point(TmpSpiral.PointCount - 1), DegToRad(azt12))
                    CreateTurnArea.AddPoint(PtIntersect)
                End If
            Else
                dAng0 = Modulus((azt12 - AztCur) * iTurnDir, 360.0)
                dAng = dAng + dAng0

                If dAng < TurnAng Then
                    AztNext = azt12
                Else
                    AztNext = wAztOut
                End If

                CreateWindSpiral(BasePoints.Point(I), AztEnter, AztCur, AztNext, TurnR, coef, iTurnDir, CreateTurnArea)
            End If

            I = (I + 1) Mod N
            AztCur = AztNext
            K = K + 1
        Loop While SubtractAngles(AztNext, wAztOut) > degEps

        If OptionButton0603.Checked And CheckBox0601.Checked Then
            AztOut = wAztOut
        End If

    End Function

    Private Function CalcAreaIntersection(ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutAzt As Double, ByRef DirToNav As Double, ByRef iTurnDir As Integer, ByRef iTurnDir2 As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
        Dim RightPolys As ESRI.ArcGIS.Geometry.IPointCollection
        Dim LeftPolys As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

        Dim PtInterP As ESRI.ArcGIS.Geometry.IPoint
        Dim PtInterE As ESRI.ArcGIS.Geometry.IPoint
        Dim PtInter15 As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

        Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
        Dim ptIn As ESRI.ArcGIS.Geometry.IPoint

        Dim SplayAngle As Double
        Dim OutDir As Double
        Dim InDir As Double
        Dim Dist1 As Double
        Dim Dist2 As Double
        Dim Dir2Int As Double
        Dim Dir2Out As Double
        Dim ExtAngle As Double

        Dim OutLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim InLine As ESRI.ArcGIS.Geometry.IPolyline

        Dim TopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

        Dim I As Integer
        Dim Side As Integer
        Dim SideE As Integer
        Dim SideP As Integer

        CalcAreaIntersection = New ESRI.ArcGIS.Geometry.Polygon
        CalcAreaIntersection.AddPointCollection(TurnArea)

        LeftPolys = New ESRI.ArcGIS.Geometry.Polyline
        RightPolys = New ESRI.ArcGIS.Geometry.Polyline

        OutLine = New ESRI.ArcGIS.Geometry.Polyline
        InLine = New ESRI.ArcGIS.Geometry.Polyline

        PtInterE = New ESRI.ArcGIS.Geometry.Point
        PtInterP = New ESRI.ArcGIS.Geometry.Point

        TextBox0701.Text = ""
        TextBox0702.Text = ""

        LeftPolys.AddPoint(SecL.Point(5)) '0
        LeftPolys.AddPoint(SecL.Point(0)) '1
        LeftPolys.AddPoint(SecL.Point(1)) '2

        RightPolys.AddPoint(SecR.Point(2)) '3
        RightPolys.AddPoint(SecR.Point(3)) '4
        RightPolys.AddPoint(SecR.Point(4)) '5

        ptIn = TurnArea.Point(0)
        ptOut = TurnArea.Point(TurnArea.PointCount - 1)

        'InDir = OutAzt + arafTrn_OSplay.Value * TurnDir
        'If iTurnDir * iTurnDir2 < 0 Then
        '    OutDir = OutAzt - arafTrn_OSplay.Value * TurnDir
        'Else
        '    OutDir = MPtCollection.Point(1).M - arafTrn_OSplay.Value * TurnDir
        'End If

        'If iTurnDir * iTurnDir2 < 0 Then
        '	InDir = MPtCollection.Point(1).M + arafTrn_OSplay.Value * TurnDir
        'Else
        '	InDir = DirToNav + arafTrn_OSplay.Value * TurnDir
        'End If

        'If iTurnDir * iTurnDir2 < 0 Then
        '	OutDir = DirToNav - arafTrn_OSplay.Value * TurnDir
        'Else
        '	OutDir = MPtCollection.Point(1).M - arafTrn_OSplay.Value * TurnDir
        'End If

        InDir = OutAzt + arafTrn_OSplay.Value * TurnDir
        OutDir = OutAzt - arafTrn_OSplay.Value * TurnDir

        InLine.FromPoint = ptIn
        ptTmp = PointAlongPlane(ptIn, InDir, 10.0 * RModel)
        InLine.ToPoint = ptTmp

        OutLine.FromPoint = ptOut
        ptTmp = PointAlongPlane(ptOut, OutDir, 10.0 * RModel)
        OutLine.ToPoint = ptTmp

        tmpPoly = New ESRI.ArcGIS.Geometry.Polygon

        If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
            SplayAngle = VOR.SplayAngle
        ElseIf TurnDirector.TypeCode = eNavaidType.NDB Then
            SplayAngle = NDB.SplayAngle
        End If

        If iTurnDir > 0 Then
            tmpPoly.AddPoint(ptIn)
            tmpPoly.AddPoint(ptOut)

            TopoOper = LeftPolys
            pPoints = TopoOper.Intersect(InLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)

            If pPoints.PointCount > 0 Then
                If CheckState Then
                    OptionButton0701.Enabled = False
                    OptionButton0702.Enabled = False
                    OptionButton0703.Enabled = False
                    Frame0701.Enabled = False
                End If

                If pPoints.PointCount > 1 Then
                    Dist1 = 10.0 * RModel
                    For I = 0 To pPoints.PointCount - 1
                        Dist2 = ReturnDistanceInMeters(ptIn, pPoints.Point(I))
                        If Dist2 < Dist1 Then
                            Dist1 = Dist2
                            NJoinPt = pPoints.Point(I)
                        End If
                    Next I
                Else
                    NJoinPt = pPoints.Point(0)
                End If

                tmpPoly.AddPoint(NJoinPt, 0)

                Side = SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)
                If Side < 0 Then
                    ptTmp = PointAlongPlane(NJoinPt, DirToNav + SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(LeftPolys.Point(1), 0)
                    ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp, 0)
            Else
                If CheckState Then
                    Frame0701.Enabled = True
                End If

                SideE = -1
                Side = -1
                SideP = -1

                If RayPolylineIntersect(LeftPolys, ptIn, DirToNav, PtInterP) Then
                    SideP = SideDef(ptIn, DirToNav + 90.0, PtInterP)
                End If

                Dir2Int = ReturnAngleInDegrees(ptIn, LeftPolys.Point(1))
                ExtAngle = SubtractAngles(Dir2Int, DirToNav)
                TextBox0701.Text = CStr(System.Math.Round(ExtAngle))

                If ExtAngle <= 75.0 Then
                    PtInterE = New ESRI.ArcGIS.Geometry.Point
                    PtInterE.PutCoords(ptIn.X, ptIn.Y)
                    SideE = 1
                End If

                If RayPolylineIntersect(LeftPolys, ptIn, DirToNav + arafTrn_OSplay.Value, PtInter15) Then
                    Side = SideDef(ptIn, DirToNav + arafTrn_OSplay.Value + 90.0, PtInter15)
                End If

                OptionButton0701.Enabled = (SideP > 0)
                OptionButton0702.Enabled = (Side > 0)
                OptionButton0703.Enabled = (SideE > 0)
                '======================================================================================
                If CheckState Then
                    If OptionButton0703.Enabled Then
                        OptionButton0703.Checked = True
                        ptTmp = PtInterE
                    ElseIf OptionButton0701.Enabled Then
                        OptionButton0701.Checked = True
                        ptTmp = PtInterP
                    ElseIf OptionButton0702.Enabled Then
                        OptionButton0702.Checked = True
                        ptTmp = PtInter15
                    End If
                Else
                    If OptionButton0703.Checked Then
                        ptTmp = PtInterE
                    ElseIf OptionButton0701.Checked Then
                        ptTmp = PtInterP
                    Else
                        ptTmp = PtInter15
                    End If
                End If

                NJoinPt = New ESRI.ArcGIS.Geometry.Point
                NJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

                tmpPoly.AddPoint(ptTmp, 0)
                Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(LeftPolys.Point(1), 0)
                    ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp, 0)
            End If
            '======================================================================================

            TopoOper = RightPolys
            pPoints = TopoOper.Intersect(OutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
            If pPoints.PointCount > 0 Then
                If CheckState Then
                    Frame0702.Enabled = False
                    OptionButton0704.Enabled = False
                    OptionButton0705.Enabled = False
                    OptionButton0706.Enabled = False
                End If

                If pPoints.PointCount > 1 Then
                    Dist1 = 10.0 * RModel
                    For I = 0 To pPoints.PointCount - 1
                        Dist2 = ReturnDistanceInMeters(ptOut, pPoints.Point(I))
                        If Dist2 < Dist1 Then
                            Dist1 = Dist2
                            FJoinPt = pPoints.Point(I)
                        End If
                    Next I
                Else
                    FJoinPt = pPoints.Point(0)
                End If

                tmpPoly.AddPoint(FJoinPt)
                Side = SideDef(FJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(FJoinPt, DirToNav - SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(RightPolys.Point(1))
                    ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp)
            Else
                If CheckState Then
                    Frame0702.Enabled = True
                End If

                SideP = -1
                Side = -1
                SideE = -1

                If RayPolylineIntersect(RightPolys, ptOut, DirToNav, PtInterP) Then
                    Dist1 = ReturnDistanceInMeters(PtInterP, ptOut)
                    SideP = SideDef(ptOut, DirToNav + 90.0, PtInterP)
                End If

                Dir2Out = ReturnAngleInDegrees(ptOut, RightPolys.Point(1))
                ExtAngle = SubtractAngles(Dir2Out, DirToNav)
                TextBox0702.Text = CStr(System.Math.Round(ExtAngle))

                If ExtAngle <= 75.0 Then
                    PtInterE = New ESRI.ArcGIS.Geometry.Point
                    PtInterE.PutCoords(ptOut.X, ptOut.Y)
                    SideE = 1
                End If

                If RayPolylineIntersect(RightPolys, ptOut, DirToNav - arafTrn_OSplay.Value, PtInter15) Then
                    Dist2 = ReturnDistanceInMeters(PtInter15, ptOut)
                    Side = SideDef(ptOut, DirToNav - arafTrn_OSplay.Value + 90.0, PtInter15)
                End If

                OptionButton0704.Enabled = SideP > 0
                OptionButton0705.Enabled = Side > 0
                OptionButton0706.Enabled = SideE > 0

                '        Frame0702.Enabled = OptionButton0704.Enabled And OptionButton0705.Enabled And OptionButton0706.Enabled

                '======================================================================================
                If CheckState Then
                    If OptionButton0704.Enabled Then
                        OptionButton0704.Checked = True
                        ptTmp = PtInterP
                    ElseIf OptionButton0705.Enabled Then
                        OptionButton0705.Checked = True
                        ptTmp = PtInter15
                    ElseIf OptionButton0706.Enabled Then
                        OptionButton0706.Checked = True
                        ptTmp = PtInterE
                    End If
                Else
                    If OptionButton0704.Checked Then
                        ptTmp = PtInterP
                    ElseIf OptionButton0705.Checked Then
                        ptTmp = PtInter15
                    Else 'If OptionButton0706.Value Then
                        ptTmp = PtInterE
                    End If
                End If

                FJoinPt = New ESRI.ArcGIS.Geometry.Point
                FJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

                tmpPoly.AddPoint(ptTmp)
                Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(RightPolys.Point(1))
                    ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp)
            End If
            '======================================================================================
        Else
            tmpPoly.AddPoint(ptIn)
            tmpPoly.AddPoint(ptOut)

            TopoOper = RightPolys
            pPoints = TopoOper.Intersect(InLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
            If pPoints.PointCount > 0 Then
                If CheckState Then
                    OptionButton0701.Enabled = False
                    OptionButton0702.Enabled = False
                    OptionButton0703.Enabled = False
                    Frame0701.Enabled = False
                End If

                If pPoints.PointCount > 1 Then
                    Dist1 = 10.0 * RModel
                    For I = 0 To pPoints.PointCount - 1
                        Dist2 = ReturnDistanceInMeters(ptIn, pPoints.Point(I))
                        If Dist2 < Dist1 Then
                            Dist1 = Dist2
                            NJoinPt = pPoints.Point(I)
                        End If
                    Next I
                Else
                    NJoinPt = pPoints.Point(0)
                End If

                tmpPoly.AddPoint(NJoinPt, 0)
                Side = SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(NJoinPt, DirToNav - SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(RightPolys.Point(1), 0)
                    ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp, 0)
            Else
                If CheckState Then
                    Frame0701.Enabled = True
                    '                OptionButton0701.Enabled = True
                    '                OptionButton0702.Enabled = True
                    '                OptionButton0703.Enabled = True
                End If

                SideP = -1
                Side = -1
                SideE = -1

                If RayPolylineIntersect(RightPolys, ptIn, DirToNav, PtInterP) Then
                    Dist1 = ReturnDistanceInMeters(PtInterP, ptIn)
                    SideP = SideDef(ptIn, DirToNav + 90.0, PtInterP)
                End If

                Dir2Int = ReturnAngleInDegrees(ptIn, RightPolys.Point(1))
                ExtAngle = SubtractAngles(Dir2Int, DirToNav)
                TextBox0701.Text = CStr(System.Math.Round(ExtAngle))

                If ExtAngle <= 75.0 Then
                    PtInterE = New ESRI.ArcGIS.Geometry.Point
                    PtInterE.PutCoords(ptIn.X, ptIn.Y)
                    SideE = 1
                End If

                If RayPolylineIntersect(RightPolys, ptIn, DirToNav - arafTrn_OSplay.Value, PtInter15) Then
                    '                Dist2 = ReturnDistanceInMeters(PtInter15, ptIn)
                    '                msgbox  "15° Dist = " + CStr(Round(Dist2)) + " Side = " + CStr(SideE)
                    Side = SideDef(ptIn, DirToNav - arafTrn_OSplay.Value + 90.0, PtInter15)
                End If

                OptionButton0701.Enabled = SideP > 0
                OptionButton0702.Enabled = Side > 0
                OptionButton0703.Enabled = SideE > 0
                '        Frame0701.Enabled = OptionButton0701.Enabled And OptionButton0703.Enabled
                '======================================================================================
                If CheckState Then
                    If OptionButton0703.Enabled Then
                        OptionButton0703.Checked = True
                        ptTmp = PtInterE
                    ElseIf OptionButton0701.Enabled Then
                        OptionButton0701.Checked = True
                        ptTmp = PtInterP
                    ElseIf OptionButton0702.Enabled Then
                        OptionButton0702.Checked = True
                        ptTmp = PtInter15
                    End If
                Else
                    If OptionButton0703.Checked Then
                        ptTmp = PtInterE
                    ElseIf OptionButton0701.Checked Then
                        ptTmp = PtInterP
                    Else
                        ptTmp = PtInter15
                    End If
                End If

                NJoinPt = New ESRI.ArcGIS.Geometry.Point
                NJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

                tmpPoly.AddPoint(ptTmp, 0)
                Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(RightPolys.Point(1), 0)
                    ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp, 0)
            End If
            '======================================================================================
            TopoOper = LeftPolys

            pPoints = TopoOper.Intersect(OutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
            If pPoints.PointCount > 0 Then
                If CheckState Then
                    OptionButton0704.Enabled = False
                    OptionButton0705.Enabled = False
                    OptionButton0706.Enabled = False
                    Frame0702.Enabled = False
                End If

                If pPoints.PointCount > 1 Then
                    Dist1 = 10.0 * RModel
                    For I = 0 To pPoints.PointCount - 1
                        Dist2 = ReturnDistanceInMeters(ptOut, pPoints.Point(I))
                        If Dist2 < Dist1 Then
                            Dist1 = Dist2
                            FJoinPt = pPoints.Point(I)
                        End If
                    Next I
                Else
                    FJoinPt = pPoints.Point(0)
                End If

                tmpPoly.AddPoint(pPoints.Point(0))
                Side = SideDef(pPoints.Point(0), DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(pPoints.Point(0), DirToNav + SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(LeftPolys.Point(1))
                    ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp)
            Else
                If CheckState Then
                    Frame0702.Enabled = True
                End If

                SideP = -1
                Side = -1
                SideE = -1

                If RayPolylineIntersect(LeftPolys, ptOut, DirToNav, PtInterP) Then
                    Dist1 = ReturnDistanceInMeters(PtInterP, ptOut)
                    SideP = SideDef(ptOut, DirToNav + 90.0, PtInterP)
                End If

                Dir2Out = ReturnAngleInDegrees(ptOut, LeftPolys.Point(1))
                ExtAngle = SubtractAngles(Dir2Out, DirToNav)
                TextBox0702.Text = CStr(System.Math.Round(ExtAngle))

                If ExtAngle <= 75.0 Then
                    PtInterE = New ESRI.ArcGIS.Geometry.Point
                    PtInterE.PutCoords(ptOut.X, ptOut.Y)
                    SideE = 1
                End If

                If RayPolylineIntersect(LeftPolys, ptOut, DirToNav + arafTrn_OSplay.Value, PtInter15) Then
                    Dist2 = ReturnDistanceInMeters(PtInter15, ptOut)
                    Side = SideDef(ptOut, DirToNav + arafTrn_OSplay.Value + 90.0, PtInter15)
                End If

                OptionButton0704.Enabled = SideP > 0
                OptionButton0705.Enabled = Side > 0
                OptionButton0706.Enabled = SideE > 0

                '        Frame0702.Enabled = OptionButton0704.Enabled And OptionButton0705.Enabled And OptionButton0706.Enabled
                '======================================================================================
                If CheckState Then
                    If OptionButton0704.Enabled Then
                        OptionButton0704.Checked = True
                        ptTmp = PtInterP
                    ElseIf OptionButton0705.Enabled Then
                        OptionButton0705.Checked = True
                        ptTmp = PtInter15
                    ElseIf OptionButton0706.Enabled Then
                        OptionButton0706.Checked = True
                        ptTmp = PtInterE
                    End If
                Else
                    If OptionButton0704.Checked Then
                        ptTmp = PtInterP
                    ElseIf OptionButton0705.Checked Then
                        ptTmp = PtInter15
                    Else 'If OptionButton0706.Value Then
                        ptTmp = PtInterE
                    End If
                End If

                FJoinPt = New ESRI.ArcGIS.Geometry.Point
                FJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

                tmpPoly.AddPoint(ptTmp)
                Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

                If Side < 0 Then
                    ptTmp = PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * RModel)
                Else
                    tmpPoly.AddPoint(LeftPolys.Point(1))
                    ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
                End If

                tmpPoly.AddPoint(ptTmp)
            End If
            '======================================================================================
        End If

        TopoOper = tmpPoly
        TopoOper.IsKnownSimple_2 = False
        TopoOper.Simplify()

        CalcAreaIntersection = RemoveAgnails(CalcAreaIntersection)
        CalcAreaIntersection = TopoOper.Union(CalcAreaIntersection)

        TopoOper = CalcAreaIntersection
        TopoOper.IsKnownSimple_2 = False
        TopoOper.Simplify()
    End Function

    Private Sub CalcJoiningParams(ByRef NavType As Integer, ByRef iTurnDir As Integer, ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutPt As ESRI.ArcGIS.Geometry.IPoint, ByRef OutAzt As Double)
        Dim AllPolys As ESRI.ArcGIS.Geometry.IPointCollection

        Dim votPovorot As ESRI.ArcGIS.Geometry.IPoint

        Dim Side As Integer
        Dim Side1 As Integer
        Dim KendineGayidis As Boolean
        Dim fTmpAzt As Double
        Dim TurnPT As NavaidData

        Prim = New ESRI.ArcGIS.Geometry.Polygon
        SecL = New ESRI.ArcGIS.Geometry.Polygon
        SecR = New ESRI.ArcGIS.Geometry.Polygon
        AllPolys = New ESRI.ArcGIS.Geometry.Polygon

        KendineGayidis = (TurnDirector.pPtPrj.X - ILS.pPtPrj.X) ^ 2 + (TurnDirector.pPtPrj.Y - ILS.pPtPrj.Y) ^ 2 < distEps * distEps
        TurnPT = WPT_FIXToNavaid(TurnDirector)

        CreateNavaidZone(TurnPT, OutAzt, FicTHRprj, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

        votPovorot = TurnArea.Point(TurnArea.PointCount - 1)

        AllPolys.AddPoint(SecL.Point(5)) '0
        AllPolys.AddPoint(SecL.Point(0)) '1
        AllPolys.AddPoint(SecL.Point(1)) '2

        AllPolys.AddPoint(SecR.Point(2)) '3
        AllPolys.AddPoint(SecR.Point(3)) '4
        AllPolys.AddPoint(SecR.Point(4)) '5

        'TextBox1001 = ""
        TextBox0906.Text = ""

        NReqCorrAngle = -1.0
        FReqCorrAngle = -1.0

        If iTurnDir < 0 Then 'sag
            '================== xarici ========================
            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(1), fTmpAzt, votPovorot)

            If Side * Side1 < 0 Then
                If CheckState Then
                    Frame0702.Enabled = False
                    'FState = eExpansion.noChange
                    OptionButton0704.Enabled = False
                    OptionButton0706.Enabled = False
                End If
            Else
                If CheckState Then
                    Frame0702.Enabled = True
                    OptionButton0704.Enabled = True
                    OptionButton0706.Enabled = True
                End If

                FReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(1), votPovorot)
                FReqCorrAngle = Modulus(FReqCorrAngle - fTmpAzt, 360.0)
                If FReqCorrAngle > 180.0 Then FReqCorrAngle = 360.0 - FReqCorrAngle
                TextBox0702.Text = CStr(System.Math.Round(FReqCorrAngle + 0.0499999, 1))

                If FReqCorrAngle > fCorrAngle Then
                    'FState = eExpansion.goParalel
                    OptionButton0704.Checked = True
                Else
                    'FState = eExpansion.expandAngle
                    OptionButton0706.Checked = True
                End If
            End If
            '================== daxili ========================
            If KendineGayidis Then
                If CheckState Then
                    Frame0701.Enabled = False
                    OptionButton0701.Enabled = False
                    OptionButton0703.Enabled = False
                    'FState = eExpansion.selfExpand
                    OptionButton0703.Checked = True
                End If
            Else
                fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))
                Side = SideFrom2Angle(OutAzt, fTmpAzt)
                Side1 = SideDef(AllPolys.Point(4), fTmpAzt, TurnArea.Point(0))

                If Side * Side1 < 0 Then
                    If CheckState Then
                        'NState = eExpansion.noChange
                        OptionButton0701.Enabled = False
                        OptionButton0703.Enabled = False
                        Frame0701.Enabled = False
                    End If
                Else
                    If CheckState Then
                        Frame0701.Enabled = True
                        OptionButton0701.Enabled = True
                        OptionButton0703.Enabled = True
                    End If

                    NReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(4), TurnArea.Point(0))
                    NReqCorrAngle = Modulus(fTmpAzt - NReqCorrAngle, 360.0)
                    If NReqCorrAngle > 180.0 Then NReqCorrAngle = 360.0 - NReqCorrAngle
                    TextBox0701.Text = CStr(System.Math.Round(NReqCorrAngle + 0.0499999, 1))

                    If CheckState Then
                        If NReqCorrAngle > NCorrAngle Then
                            'NState = eExpansion.goParalel
                            OptionButton0701.Checked = True
                        Else
                            'NState = eExpansion.expandAngle
                            OptionButton0703.Checked = True
                        End If
                    End If
                End If
            End If
        Else 'sol
            '================== xarici ========================
            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))
            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(4), fTmpAzt, votPovorot)

            If Side * Side1 < 0 Then
                If CheckState Then
                    'FState = eExpansion.noChange
                    OptionButton0704.Enabled = False
                    OptionButton0706.Enabled = False
                    Frame0702.Enabled = False
                End If
            Else
                If CheckState Then
                    Frame0702.Enabled = True
                    OptionButton0704.Enabled = True
                    OptionButton0706.Enabled = True
                End If

                FReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(4), votPovorot)
                FReqCorrAngle = Modulus(FReqCorrAngle - fTmpAzt, 360.0)
                If FReqCorrAngle > 180.0 Then FReqCorrAngle = 360.0 - FReqCorrAngle
                TextBox0702.Text = CStr(System.Math.Round(FReqCorrAngle + 0.0499999, 1))

                If CheckState Then
                    If FReqCorrAngle > fCorrAngle Then
                        'FState = eExpansion.goParalel
                        OptionButton0704.Checked = True
                    Else
                        'FState = eExpansion.expandAngle
                        OptionButton0706.Checked = True
                    End If
                End If
            End If
            '================== daxili ========================
            If KendineGayidis Then
                If CheckState Then
                    Frame0701.Enabled = False
                    OptionButton0701.Enabled = False
                    OptionButton0703.Enabled = False
                    'FState = eExpansion.selfExpand
                    OptionButton0703.Checked = True
                End If
            Else
                fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
                Side = SideFrom2Angle(OutAzt, fTmpAzt)
                Side1 = SideDef(AllPolys.Point(1), fTmpAzt, TurnArea.Point(0))

                If Side * Side1 < 0 Then
                    If CheckState Then
                        'NState = eExpansion.noChange
                        OptionButton0701.Enabled = False
                        OptionButton0703.Enabled = False
                        Frame0701.Enabled = False
                    End If
                Else
                    If CheckState Then
                        Frame0701.Enabled = True
                        OptionButton0701.Enabled = True
                        OptionButton0703.Enabled = True
                    End If

                    NReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(1), TurnArea.Point(0))
                    NReqCorrAngle = Modulus(fTmpAzt - NReqCorrAngle, 360.0)
                    If NReqCorrAngle > 180.0 Then NReqCorrAngle = 360.0 - NReqCorrAngle
                    TextBox0701.Text = CStr(System.Math.Round(NReqCorrAngle + 0.0499999, 1))

                    If CheckState Then
                        If NReqCorrAngle > NCorrAngle Then
                            'NState = eExpansion.goParalel
                            OptionButton0701.Checked = True
                        Else
                            'NState = eExpansion.expandAngle
                            OptionButton0703.Checked = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function ApplayJoining(ByRef NavType As Integer, ByRef iTurnDir As Integer, ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutPt As ESRI.ArcGIS.Geometry.IPoint, ByRef OutAzt As Double, ByRef tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection) As ESRI.ArcGIS.Geometry.Polygon
        Dim ApplayJoining1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim AllPolys As ESRI.ArcGIS.Geometry.IPointCollection

        Dim votPovorot As ESRI.ArcGIS.Geometry.IPoint
        Dim antiPovorot As ESRI.ArcGIS.Geometry.IPoint

        Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim Clone As ESRI.ArcGIS.esriSystem.IClone

        Dim Side1 As Integer
        Dim Side As Integer

        Dim NavOuterAzt As Double
        Dim fTmpAzt As Double
        Dim TurnPT As NavaidData

        AllPolys = New ESRI.ArcGIS.Geometry.Polygon

        Clone = TurnArea
        ApplayJoining1 = Clone.Clone

        votPovorot = TurnArea.Point(TurnArea.PointCount - 1)
        antiPovorot = TurnArea.Point(0)

        NJoinPt = antiPovorot
        FJoinPt = votPovorot

        TurnPT = WPT_FIXToNavaid(TurnDirector)

        CreateNavaidZone(TurnPT, OutAzt, FicTHRprj, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

        AllPolys.AddPoint(SecL.Point(5)) '0
        AllPolys.AddPoint(SecL.Point(0)) '1
        AllPolys.AddPoint(SecL.Point(1)) '2

        AllPolys.AddPoint(SecR.Point(2)) '3
        AllPolys.AddPoint(SecR.Point(3)) '4
        AllPolys.AddPoint(SecR.Point(4)) '5

        NOutPt = New ESRI.ArcGIS.Geometry.Point
        FOutPt = New ESRI.ArcGIS.Geometry.Point

        Construct = FOutPt

        If iTurnDir < 0 Then 'sag
            '================== xarici ========================
            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(1), fTmpAzt, votPovorot)

            If Side * Side1 < 0 Then
                NavOuterAzt = OutAzt - arafTrn_OSplay.Value * iTurnDir
                Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(1), DegToRad(fTmpAzt))
                fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(1))

                Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, FOutPt)

                If (Side < 0) Then
                    ApplayJoining1.AddPoint(FOutPt)
                    ApplayJoining1.AddPoint(AllPolys.Point(1))
                    tmpPoly1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(AllPolys.Point(1))
                Else
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(0), AllPolys.Point(2))
                    Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(0), DegToRad(fTmpAzt))
                    ApplayJoining1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(FOutPt)
                End If

                ApplayJoining1.AddPoint(AllPolys.Point(2))
                tmpPoly1.AddPoint(AllPolys.Point(2))
            Else
                If OptionButton0704.Checked Then
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
                    Construct.ConstructAngleIntersection(votPovorot, DegToRad(OutAzt), AllPolys.Point(1), DegToRad(fTmpAzt))

                    ApplayJoining1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(FOutPt)

                    Side = SideDef(FOutPt, OutAzt, AllPolys.Point(2))
                    If Side < 0 Then
                        ApplayJoining1.AddPoint(AllPolys.Point(2))
                        tmpPoly1.AddPoint(AllPolys.Point(2))
                    End If
                Else 'Genishlanir
                    FOutPt.PutCoords(votPovorot.X, votPovorot.Y)

                    ApplayJoining1.AddPoint(AllPolys.Point(1))
                    ApplayJoining1.AddPoint(AllPolys.Point(2))
                    tmpPoly1.AddPoint(AllPolys.Point(1))
                    tmpPoly1.AddPoint(AllPolys.Point(2))
                End If
            End If
            '================== daxili ========================
            Construct = NOutPt

            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))

            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(4), fTmpAzt, antiPovorot)

            If Side * Side1 < 0 Then
                NavOuterAzt = OutAzt + arafTrn_ISplay.Value * iTurnDir
                Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

                ApplayJoining1.AddPoint(AllPolys.Point(3))
                tmpPoly1.AddPoint(AllPolys.Point(3))

                fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(4))
                Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt)
                If (Side * iTurnDir < 0) Then
                    ApplayJoining1.AddPoint(AllPolys.Point(4))
                    tmpPoly1.AddPoint(AllPolys.Point(4))
                Else
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
                    Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))
                End If

                ApplayJoining1.AddPoint(NOutPt)
                tmpPoly1.AddPoint(NOutPt)
            Else
                If OptionButton0701.Checked Then
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
                    Construct.ConstructAngleIntersection(antiPovorot, DegToRad(OutAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

                    Side = SideDef(NOutPt, OutAzt, AllPolys.Point(3))
                    If Side * iTurnDir < 0 Then
                        ApplayJoining1.AddPoint(AllPolys.Point(3))
                        tmpPoly1.AddPoint(AllPolys.Point(3))
                    End If
                    ApplayJoining1.AddPoint(NOutPt)
                    tmpPoly1.AddPoint(NOutPt)
                Else 'Genishlanir
                    NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y)

                    ApplayJoining1.AddPoint(AllPolys.Point(3))
                    ApplayJoining1.AddPoint(AllPolys.Point(4))
                    tmpPoly1.AddPoint(AllPolys.Point(3))
                    tmpPoly1.AddPoint(AllPolys.Point(4))
                    '            ApplayJoining1.AddPoint NOutPt
                    '            tmpPoly1.AddPoint NOutPt
                    '        Set NJoinPt = NOutPt
                End If
            End If
        Else 'sol
            '================== xarici ========================
            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))
            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(4), fTmpAzt, votPovorot)

            If Side * Side1 < 0 Then
                NavOuterAzt = OutAzt - arafTrn_OSplay.Value * iTurnDir
                Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

                fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(4))
                Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, FOutPt)
                If (Side * iTurnDir > 0) Then
                    ApplayJoining1.AddPoint(FOutPt)
                    ApplayJoining1.AddPoint(AllPolys.Point(4))
                    tmpPoly1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(AllPolys.Point(4))
                Else
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
                    Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))
                    ApplayJoining1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(FOutPt)
                End If

                ApplayJoining1.AddPoint(AllPolys.Point(3))
                tmpPoly1.AddPoint(AllPolys.Point(3))
            Else
                If OptionButton0704.Checked Then 'Genishlanmir
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
                    Construct.ConstructAngleIntersection(votPovorot, DegToRad(OutAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

                    ApplayJoining1.AddPoint(FOutPt)
                    tmpPoly1.AddPoint(FOutPt)
                    Side = SideDef(FOutPt, OutAzt, AllPolys.Point(3))
                    If Side > 0 Then
                        ApplayJoining1.AddPoint(AllPolys.Point(3))
                        tmpPoly1.AddPoint(AllPolys.Point(3))
                    End If
                Else 'Genishlanir
                    FOutPt.PutCoords(votPovorot.X, votPovorot.Y)

                    ApplayJoining1.AddPoint(AllPolys.Point(4))
                    ApplayJoining1.AddPoint(AllPolys.Point(3))
                    tmpPoly1.AddPoint(AllPolys.Point(4))
                    tmpPoly1.AddPoint(AllPolys.Point(3))
                End If
            End If
            '================== daxili ========================

            fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
            Side = SideFrom2Angle(OutAzt, fTmpAzt)
            Side1 = SideDef(AllPolys.Point(1), fTmpAzt, antiPovorot)
            Construct = NOutPt

            If Side * Side1 < 0 Then
                NavOuterAzt = OutAzt + arafTrn_ISplay.Value * iTurnDir
                Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(1), DegToRad(fTmpAzt))

                ApplayJoining1.AddPoint(AllPolys.Point(2))
                tmpPoly1.AddPoint(AllPolys.Point(2))
                fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(1))
                Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt)
                If (Side * iTurnDir < 0) Then
                    ApplayJoining1.AddPoint(AllPolys.Point(1))
                    tmpPoly1.AddPoint(AllPolys.Point(1))
                Else
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
                    Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(1), DegToRad(fTmpAzt))
                End If

                ApplayJoining1.AddPoint(NOutPt)
                tmpPoly1.AddPoint(NOutPt)
            Else
                If OptionButton0701.Checked Then 'Genishlanmir
                    fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
                    Construct.ConstructAngleIntersection(antiPovorot, DegToRad(OutAzt), AllPolys.Point(1), DegToRad(fTmpAzt))

                    Side = SideDef(NOutPt, OutAzt, AllPolys.Point(2))
                    If Side * iTurnDir < 0 Then
                        ApplayJoining1.AddPoint(AllPolys.Point(2))
                        tmpPoly1.AddPoint(AllPolys.Point(2))
                    End If
                    ApplayJoining1.AddPoint(NOutPt)
                    tmpPoly1.AddPoint(NOutPt)
                Else 'Genishlanir
                    NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y)

                    ApplayJoining1.AddPoint(AllPolys.Point(2))
                    ApplayJoining1.AddPoint(AllPolys.Point(1))
                    tmpPoly1.AddPoint(AllPolys.Point(2))
                    tmpPoly1.AddPoint(AllPolys.Point(1)) '''''
                End If
            End If
        End If

        ApplayJoining1.AddPoint(ApplayJoining1.Point(0))
        tmpPoly1.AddPoint(tmpPoly1.Point(0))
        ApplayJoining = ApplayJoining1
    End Function

    Private Sub UpdateIntervals(Optional ChangeCurDir As Boolean = False)
        Dim Intervals() As Interval
        Dim CurPnt As ESRI.ArcGIS.Geometry.IPoint
        Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
        Dim RecommendStr As String
        Dim tmpStr As String

        Dim VTotal As Double
        Dim lMinDR As Double
        Dim tStabl As Double
        Dim TurnR As Double
        Dim fTASl As Double
        Dim delta As Double
        Dim Alpha As Double
        Dim xMin As Double
        Dim xMax As Double
        Dim Snap As Double
        Dim fTmp As Double
        Dim ddr As Double
        Dim bAz As Double
        Dim dr As Double
        Dim L0 As Double
        Dim D As Double
        Dim R As Double

        Dim N As Integer
        Dim I As Integer
        Dim J As Integer

        If MultiPage1.SelectedIndex < 6 Then Return

        Snap = CDbl(TextBox0604.Text)
        tStabl = CDbl(TextBox0605.Text)
        fTASl = IAS2TAS(m_fIAS, (TurnFixPnt.Z), CurrADHP.ISAtC)

        VTotal = fTASl + CurrADHP.WindSpeed

        If OptionButton0401.Checked Then
            L0 = Point2LineDistancePrj(PtSOC, KK.FromPoint, _ArDir + 90.0)
        Else
            L0 = Point2LineDistancePrj(PtSOC, KKFixMax.FromPoint, _ArDir + 90.0)
        End If

        If OptionButton0401.Checked Then
            CurPnt = PointAlongPlane(PtSOC, _ArDir, L0)
        Else
            CurPnt = New ESRI.ArcGIS.Geometry.Point
            CurPnt.PutCoords(TurnFixPnt.X, TurnFixPnt.Y)
        End If

        TurnR = Bank2Radius(fBankAngle, fTASl)
        '==========================================
        ddr = TurnR / (System.Math.Tan(DegToRad((180.0 - Snap) * 0.5)))
        dr = arT_Gui_dist.Value + ddr
        lMinDR = VTotal * tStabl * 0.277777777777778 + ddr
        MinDR = System.Math.Round(lMinDR - ddr)
        ToolTip1.SetToolTip(TextBox0605, My.Resources.str13009 + CStr(ConvertDistance(MinDR, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit)
        '==========================================

        ptCnt = PointAlongPlane(CurPnt, _ArDir + 90.0 * TurnDir, TurnR)
        bAz = ReturnAngleInDegrees(TurnDirector.pPtPrj, ptCnt)
        D = ReturnDistanceInMeters(ptCnt, TurnDirector.pPtPrj)

        R = System.Math.Sqrt(dr * dr + TurnR * TurnR)

        delta = RadToDeg(System.Math.Atan(TurnR / dr))
        Alpha = Snap - TurnDir * delta

        fTmp = R * System.Math.Sin(DegToRad(Alpha)) / D
        If fTmp > 1.0 Then
            xMin = 90.0
        ElseIf fTmp < -1.0 Then
            xMin = -90.0
        Else
            xMin = RadToDeg(ArcSin(fTmp))
        End If

        Alpha = Snap + TurnDir * delta
        fTmp = R * System.Math.Sin(DegToRad(Alpha)) / D
        If fTmp > 1.0 Then
            xMax = 90.0
        ElseIf fTmp < -1.0 Then
            xMax = -90.0
        Else
            xMax = RadToDeg(ArcSin(fTmp))
        End If

        ReDim Intervals(3)

        Intervals(0).Left = Modulus(Dir2Azt(PtSOC, bAz + xMin) - CurrADHP.MagVar, 360.0)
        Intervals(1).Right = Modulus(Dir2Azt(PtSOC, bAz - xMax) - CurrADHP.MagVar, 360.0)
        Intervals(2).Left = Modulus(Dir2Azt(PtSOC, bAz + xMax) + 180.0 - CurrADHP.MagVar, 360.0)
        Intervals(3).Right = Modulus(Dir2Azt(PtSOC, bAz - xMin) + 180.0 - CurrADHP.MagVar, 360.0)

        R = System.Math.Sqrt(lMinDR * lMinDR + TurnR * TurnR)

        delta = RadToDeg(System.Math.Atan(TurnR / lMinDR))
        Alpha = Snap - TurnDir * delta

        fTmp = R * System.Math.Sin(DegToRad(Alpha)) / D
        If fTmp > 1.0 Then
            xMin = 90.0
        ElseIf fTmp < -1.0 Then
            xMin = -90.0
        Else
            xMin = RadToDeg(ArcSin(fTmp))
        End If

        Alpha = Snap + TurnDir * delta
        fTmp = R * System.Math.Sin(DegToRad(Alpha)) / D
        If fTmp > 1.0 Then
            xMax = 90.0
        ElseIf fTmp < -1.0 Then
            xMax = -90.0
        Else
            xMax = RadToDeg(ArcSin(fTmp))
        End If

        Intervals(0).Right = Modulus(Dir2Azt(PtSOC, bAz + xMin) - CurrADHP.MagVar, 360.0)
        Intervals(1).Left = Modulus(Dir2Azt(PtSOC, bAz - xMax) - CurrADHP.MagVar, 360.0)
        Intervals(2).Right = Modulus(Dir2Azt(PtSOC, bAz + xMax) + 180.0 - CurrADHP.MagVar, 360.0)
        Intervals(3).Left = Modulus(Dir2Azt(PtSOC, bAz - xMin) + 180.0 - CurrADHP.MagVar, 360.0)

        N = UBound(Intervals)
        J = 0
        'SortIntervals Intervals

        While J < N
            If SubtractAngles(Intervals(J).Right, Intervals(J + 1).Left) <= 1.0 Then
                Intervals(J).Right = Intervals(J + 1).Right
                For I = J + 1 To N - 1
                    Intervals(I) = Intervals(I + 1)
                Next
                N = N - 1
                If N > -1 Then
                    ReDim Preserve Intervals(N)
                Else
                    ReDim Intervals(-1)
                End If
            Else
                J = J + 1
            End If
        End While

        N = UBound(Intervals)

        If N > 0 Then
            If SubtractAngles(Intervals(0).Left, Intervals(N).Right) <= 1.0 Then
                Intervals(0).Left = Intervals(N).Left
                N = N - 1
                ReDim Preserve Intervals(N)
            End If
        End If

        SortIntervals(Intervals)

        RecommendStr = My.Resources.str13008 '"Рекомендуемые интервалы курса: "
        tmpStr = RecommendStr + vbCrLf

        For I = 0 To N
            If SubtractAngles(System.Math.Round(Intervals(I).Left), System.Math.Round(Intervals(I).Right)) <= degEps Then
                RecommendStr = RecommendStr + CStr(System.Math.Round(Intervals(I).Left)) + "°"
                tmpStr = tmpStr + CStr(System.Math.Round(Intervals(I).Left)) + "°"

                If (I = 0) And ChangeCurDir Then TextBox0603.Text = CStr(System.Math.Round(Intervals(0).Left))
            Else
                RecommendStr = RecommendStr + My.Resources.str00221 + CStr(System.Math.Round(Intervals(I).Left + 0.4999)) + "°" + My.Resources.str00222 + CStr(Int(Intervals(I).Right)) + "°"
                tmpStr = tmpStr + My.Resources.str00221 + CStr(System.Math.Round(Intervals(I).Left + 0.4999)) + "°" + My.Resources.str00222 + CStr(Int(Intervals(I).Right)) + "°"
                If (I = 0) And ChangeCurDir Then TextBox0603.Text = CStr(System.Math.Round(Intervals(0).Left + 0.4999))
            End If

            If I <> N Then
                RecommendStr = RecommendStr + "; "
                tmpStr = tmpStr + vbCrLf
            End If
        Next
        '    i = _Label0601_7.Width
        _Label0601_7.Text = tmpStr
        '    _Label0601_7.Width = i

        If OptionButton0602.Checked Then
            ToolTip1.SetToolTip(TextBox0603, RecommendStr)
        Else
            ToolTip1.SetToolTip(TextBox0603, "")
        End If

        If ChangeCurDir Then
            TextBox0603.Tag = ""
            TextBox0603_Validating(TextBox0603, New System.ComponentModel.CancelEventArgs(True))
        End If
    End Sub

    Private Function UpdateToNavCourse(ByVal OutAzt As Double) As Integer
        Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim InterAngle As Double
        Dim curDist0 As Double
        Dim curDist As Double
        Dim AztCur As Double
        Dim TurnR As Double
        Dim hCalc As Double
        Dim fTASl As Double
        Dim fTmp As Double
        Dim dDir As Double
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pRPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pLPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim DrawPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolygon1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim NewPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSource As ESRI.ArcGIS.esriSystem.IClone
        Dim TurnDir2 As Integer
        Dim iMax As Integer
        Dim K As Integer
        Dim I As Integer
        Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D

        UpdateToNavCourse = 0

        If Not OptionButton0401.Checked Then 'не на Высоте
            pSource = pFullPoly
            pRPolygon = pSource.Clone
            tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
            tmpPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
            tmpPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

            CutPoly(pRPolygon, tmpPoly, -1)
            pRPolygon = ReArrangePolygon(pRPolygon, PtSOC, _ArDir)
        Else
            '        Set pSource = NewFullPoly
            pRPolygon = ReArrangePolygon(pFullPoly, PtSOC, _ArDir) 'pSource.Clone
        End If

        hCalc = Max(arMATurnAlt.Value + FicTHRprj.Z, TurnFixPnt.Z)
        fTASl = IAS2TAS(m_fIAS, hCalc, CurrADHP.ISAtC)
        TurnR = Bank2Radius(fBankAngle, fTASl)
        ''=========  End Params
        fTmp = Modulus(Dir2Azt(TurnDirector.pPtPrj, OutAzt) - TurnDirector.MagVar, 360.0)

        TextBox0603.Tag = CStr(Math.Round(fTmp))
        TextBox0603.Text = TextBox0603.Tag
        InterAngle = CDbl(TextBox0604.Text)

        '=====
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection

        MPtCollection = CalcTouchByFixDir(TurnFixPnt, TurnDirector.pPtPrj, TurnR, _ArDir, OutAzt, TurnDir, TurnDir2, InterAngle, dDir, FlyBy)
        mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
        ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)

        pPolyline = mPoly
        pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

        If ComboBox0603.SelectedIndex = 1 Then
            If SideDef(ptCur, OutAzt + 90.0, WPt0602.pPtPrj) > 0 Then
                pPath.AddPoint(WPt0602.pPtPrj)
            Else
                pProxi = pCircle
                If pProxi.ReturnDistance(ptCur) = 0.0 Then
                    CircleVectorIntersect(CurrADHP.pPtPrj, RModel, ptCur, ptCur.M, ptTmp)
                    pPath.AddPoint(ptTmp)
                End If
            End If
        Else
            pProxi = pCircle
            If pProxi.ReturnDistance(ptCur) = 0.0 Then
                CircleVectorIntersect(CurrADHP.pPtPrj, RModel, ptCur, ptCur.M, ptTmp)
                pPath.AddPoint(ptTmp)
            End If
        End If

        pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
        pPolyline.AddGeometry(pPath)
        '=====

        TextBox0602.Text = CStr(ConvertDistance(dDir, eRoundMode.NEAREST))

        If (System.Math.Round(dDir) > arT_Gui_dist.Value) Or (System.Math.Round(dDir) < MinDR) Then
            TextBox0602.ForeColor = Color.Red
        Else
            TextBox0602.ForeColor = Color.Black
        End If

        If dDir > RModel Then MessageBox.Show(My.Resources.str00303) '"При данном значении угла выход на курс средства невозможен."

        Dim NewFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
        ''=========  End Params
        If OptionButton0401.Checked And CheckBox0402.Checked Then
            pTopoOper = pFIXPoly_6
            ZNR_Poly = pTopoOper.ConvexHull
            pPolygon1 = New ESRI.ArcGIS.Geometry.Multipoint
            pPolygon1.AddPointCollection(ZNR_Poly)

            pSource = K1K1
            NewPoly = pSource.Clone
            pTransform2D = NewPoly
            pTransform2D.Move(3 * System.Math.Cos(DegToRad(_ArDir)), 3 * System.Math.Sin(DegToRad(_ArDir)))

            pPolygon1.AddPointCollection(NewPoly)
            pTopoOper = pPolygon1

            tmpPoly1 = ReArrangePolygon(pTopoOper.ConvexHull, IFprj, _ArDir)

            m_BasePoints = CreateBasePoints(tmpPoly1, K1K1, _ArDir, TurnDir)
        Else
            m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)
            NewFullPoly = ReArrangePolygon(pFullPoly, PtSOC, _ArDir)
            ZNR_Poly = CreateBasePoints(NewFullPoly, KK, _ArDir, TurnDir)
        End If

        pTopoOper = ZNR_Poly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        ''=========  End Params

        'If TurnDir * TurnDir2 < 0 Then
        '	fTmp = OutAzt
        'Else
        fTmp = MPtCollection.Point(1).M
        'End If

        m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, fTmp, TurnDir, m_BasePoints)

        I = System.Math.Round(Modulus((OutAzt - _ArDir) * TurnDir, 360.0))
        TextBox0601.Text = CStr(I)

        If I > 255 Then
            TextBox0601.ForeColor = Color.Red
        Else
            TextBox0601.ForeColor = Color.Black
        End If

        Dim Side As Integer
        If False Then
            If TurnDir > 0 Then
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.ToPoint)
                    m_TurnArea.AddPoint(KK.FromPoint)
                End If
            Else
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.FromPoint)
                    m_TurnArea.AddPoint(KK.ToPoint)
                End If
            End If

            '    If CheckBox601 Then
            '        If Turndir > 0 Then
            '            TurnArea.AddPoint pFullPoly.Point(pFullPoly.PointCount - 2), 0
            '        Else
            '            TurnArea.AddPoint pFullPoly.Point(1), 0
            '        End If
            '    End If
        Else
            K = m_BasePoints.PointCount - 1

            If m_TurnArea.PointCount > 0 Then
                pLPolygon = New ESRI.ArcGIS.Geometry.Polygon
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        pLPolygon.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I

                pLPolygon.AddPointCollection(m_TurnArea)
                m_TurnArea = pLPolygon
                ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
            Else
                ptCur = m_BasePoints.Point(0)
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        m_TurnArea.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I
                m_TurnArea.AddPoint(ptCur)
            End If
        End If

        ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
        'If TurnDir * TurnDir2 < 0 Then
        '	fTmp = MPtCollection.Point(1).M
        'Else
        '	fTmp = OutAzt
        'End If
        AztCur = fTmp + arafTrn_OSplay.Value * TurnDir

        curDist0 = 0.0
        K = pFixPoly.PointCount - 1
        For I = 0 To K
            curDist = Point2LineDistancePrj(pFixPoly.Point(I), ptCur, AztCur)
            If (curDist0 < curDist) Then
                iMax = I
                curDist0 = curDist
            End If
        Next I
        ptTmp = pFixPoly.Point(iMax)

        '====================================================================
        Dim LeftPolys As ESRI.ArcGIS.Geometry.IPointCollection
        Dim RightPolys As ESRI.ArcGIS.Geometry.IPointCollection
        Dim InLine As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
        Dim TurnPT As NavaidData

        Prim = New ESRI.ArcGIS.Geometry.Polygon
        SecL = New ESRI.ArcGIS.Geometry.Polygon
        SecR = New ESRI.ArcGIS.Geometry.Polygon

        TurnPT = WPT_FIXToNavaid(TurnDirector)
        m_DirToNav = MPtCollection.Point(MPtCollection.PointCount - 1).M
        CreateNavaidZone(TurnPT, m_DirToNav, FicTHRprj, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

        LeftPolys = New ESRI.ArcGIS.Geometry.Polyline
        RightPolys = New ESRI.ArcGIS.Geometry.Polyline
        InLine = New ESRI.ArcGIS.Geometry.Polyline

        LeftPolys.AddPoint(SecL.Point(5)) '0
        LeftPolys.AddPoint(SecL.Point(0)) '1
        LeftPolys.AddPoint(SecL.Point(1)) '2

        RightPolys.AddPoint(SecR.Point(2)) '3
        RightPolys.AddPoint(SecR.Point(3)) '4
        RightPolys.AddPoint(SecR.Point(4)) '5

        InLine.AddPoint(ptTmp)
        InLine.AddPoint(PointAlongPlane(ptTmp, AztCur, 10.0 * RModel))

        If TurnDir > 0 Then
            pTopoOper = LeftPolys
        Else
            pTopoOper = RightPolys
        End If
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pPoints = pTopoOper.Intersect(InLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)

        If pPoints.PointCount <= 0 Then
            curDist0 = 0.0
            K = pFixPoly.PointCount - 1
            For I = 0 To K
                curDist = Point2LineDistancePrj(pFixPoly.Point(I), ptCur, OutAzt)
                If (curDist0 < curDist) Then
                    iMax = I
                    curDist0 = curDist
                End If
            Next I
            ptTmp = pFixPoly.Point(iMax)
        End If
        '===========================================================================

        m_TurnArea.AddPoint(ptTmp, 0)

        NewPoly = CalcAreaIntersection(m_TurnArea, fTmp, m_DirToNav, TurnDir, TurnDir2)
        EarlierTPDir = ReturnAngleInDegrees(ptTmp, NJoinPt)

        pTopoOper = m_BasePoints
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pTopoOper = NewPoly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        BaseArea = RemoveHoles(pTopoOper.Union(m_BasePoints))

        '    If OptionButton601 Then
        '        Set pTopoOper = BaseArea
        '        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
        '    End If

        pTopoOper = pCircle
        '    Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
        'Set ge = New GeometryEnvironment
        'ge.UseAlternativeTopoOps = True
        'DrawPolygon BaseArea, 0
        BaseArea = PolygonIntersection(pCircle, BaseArea)

        On Error Resume Next
        If Not KKElem Is Nothing Then pGraphics.DeleteElement(KKElem)
        If Not K1K1Elem Is Nothing Then pGraphics.DeleteElement(K1K1Elem)
        If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
        If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
        If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
        If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
        If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        On Error GoTo hErr

        '    Set DrawPoly = pTopoOper.Intersect(Prim, esriGeometry2Dimension)
        DrawPoly = PolygonIntersection(pCircle, Prim)
        PrimElem = DrawPolygon(DrawPoly, RGB(128, 128, 0), , False)

        '    Set DrawPoly = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
        DrawPoly = PolygonIntersection(pCircle, SecL)
        SecLElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

        '    Set TurnAreaElem = DrawPolygon(BaseArea, 255, False)
        '    Set NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        '    Set DrawPoly = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
        DrawPoly = PolygonIntersection(pCircle, SecR)
        SecRElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

        TurnAreaElem = DrawPolygon(BaseArea, 255, , False)
        KKElem = DrawPolyLine(KK, 0, 1.0, False)
        K1K1Elem = DrawPolyLine(K1K1, 0, 1.0, False)
        NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        '    If ButtonControl3State Then
        pGraphics.AddElement(TurnAreaElem, 0)
        TurnAreaElem.Locked = True
        '    End If

        '    If ButtonControl4State Then
        pGraphics.AddElement(PrimElem, 0)
        PrimElem.Locked = True
        pGraphics.AddElement(SecLElem, 0)
        SecLElem.Locked = True
        pGraphics.AddElement(SecRElem, 0)
        SecRElem.Locked = True
        '    End If

        '    If ButtonControl6State Then
        pGraphics.AddElement(KKElem, 0)
        KKElem.Locked = True
        pGraphics.AddElement(K1K1Elem, 0)
        K1K1Elem.Locked = True
        '    End If

        '    If ButtonControl5State Then
        pGraphics.AddElement(NomTrackElem, 0)
        NomTrackElem.Locked = True
        '    End If

        DrawTrail()

        '    RefreshCommandBar mTool, 124
        ' CSErrorHandler begin - please do not modify or remove this line
hErr:
    End Function

    Private Function UpdateToFix() As Integer
        Dim iMax As Integer

        Dim I As Integer
        Dim K As Integer


        Dim curDist0 As Double
        Dim curDist As Double
        Dim AztCur As Double
        Dim TurnR As Double
        Dim fTASl As Double
        Dim fTmp As Double

        Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

        Dim pLPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pRPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolygon1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim NewFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pSource As ESRI.ArcGIS.esriSystem.IClone
        Dim NewPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D
        Dim DrawPoly As ESRI.ArcGIS.Geometry.IPointCollection


        NewFullPoly = ReArrangePolygon(pFullPoly, PtSOC, _ArDir)

        If OptionButton0402.Checked Then 'не на Высоте
            pSource = pFullPoly
            pRPolygon = pSource.Clone
            tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
            tmpPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
            tmpPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

            CutPoly(pRPolygon, tmpPoly, -1)
            pRPolygon = ReArrangePolygon(pRPolygon, PtSOC, _ArDir)
        Else
            pSource = NewFullPoly
            pRPolygon = pSource.Clone
        End If

        fTmp = Max(FicTHRprj.Z + arMATurnAlt.Value, TurnFixPnt.Z)
        fTASl = IAS2TAS(m_fIAS, fTmp, CurrADHP.ISAtC)
        TurnR = Bank2Radius(fBankAngle, fTASl)

        MPtCollection = TurnToFixPrj(TurnFixPnt, TurnR, TurnDir, TurnDirector.pPtPrj)

        If MPtCollection.PointCount = 0 Then
            MessageBox.Show(My.Resources.str00303)  '"С заданным направлением разворота и " + vbCrLf |        + "радиусом выход на данный пункт невозможен" + vbCrLf |        + "В заданном направлении Rmax =" + CStr(Fix(TurnR)) + "метр"
            Return -1
        End If

        m_OutPt = MPtCollection.Point(1)

        m_DirToNav = MPtCollection.Point(MPtCollection.PointCount - 1).M
        m_OutDir = MPtCollection.Point(1).M

        fTmp = Modulus(Dir2Azt(TurnDirector.pPtPrj, m_OutDir) - TurnDirector.MagVar, 360.0)
        TextBox0603.Tag = CStr(System.Math.Round(fTmp))
        TextBox0603.Text = TextBox0603.Tag

        If (TurnDirector.TypeCode <> eNavaidType.VOR) And (TurnDirector.TypeCode <> eNavaidType.NDB) And (TurnDirector.TypeCode <> eNavaidType.TACAN) Then
            Return UpdateToCourse(m_OutDir)
        End If

        If OptionButton0401.Checked And CheckBox0402.Checked Then
            pTopoOper = pFIXPoly_6
            ZNR_Poly = pTopoOper.ConvexHull
            pPolygon1 = New ESRI.ArcGIS.Geometry.Multipoint
            pPolygon1.AddPointCollection(ZNR_Poly)

            pSource = K1K1
            NewPoly = pSource.Clone
            pTransform2D = NewPoly
            pTransform2D.Move(3 * System.Math.Cos(DegToRad(_ArDir)), 3 * System.Math.Sin(DegToRad(_ArDir)))

            pPolygon1.AddPointCollection(NewPoly)
            pTopoOper = pPolygon1

            tmpPoly1 = ReArrangePolygon(pTopoOper.ConvexHull, IFprj, _ArDir)
            m_BasePoints = CreateBasePoints(tmpPoly1, K1K1, _ArDir, TurnDir)
        Else
            m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)
            ZNR_Poly = CreateBasePoints(NewFullPoly, KK, _ArDir, TurnDir)
        End If

        pTopoOper = ZNR_Poly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        '            DrawPolygon BasePoints
        '            DrawPolygon ZNR_Poly
        'DrawPolygon ZNR_Poly, 0

        m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, m_OutDir, TurnDir, m_BasePoints)

        '    DrawPolygon TurnArea

        I = System.Math.Round(Modulus((m_OutDir - _ArDir) * TurnDir, 360.0))
        TextBox0601.Text = CStr(I)

        If I > 255 Then
            TextBox0601.ForeColor = Color.Red
        Else
            TextBox0601.ForeColor = Color.Black
        End If

        mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
        If Not CheckBox0601.Checked Then
            '        mPoly.AddPoint TurnWPT.pPtPrj
            Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
            Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

            pPolyline = mPoly
            pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)
            pPath.AddPoint(TurnDirector.pPtPrj)
            '=====
            pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
            pPolyline.AddGeometry(pPath)
        End If

        '        Set ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
        'Dim pProxi As IProximityOperator
        '        Set pProxi = pCircle
        '        If pProxi.ReturnDistance(ptCur) = 0.0 Then
        '            CircleVectorIntersect PtAirportPrj, RModel, ptCur, ptCur.M, ptTmp
        '            mPoly.AddPoint ptTmp
        '        End If
        '    End If
        '    MPtCollection.Point(1).M = OutAzt

        Dim Side As Integer
        If False Then
            If TurnDir > 0 Then
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.ToPoint)
                    m_TurnArea.AddPoint(KK.FromPoint)
                End If
            Else
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.FromPoint)
                    m_TurnArea.AddPoint(KK.ToPoint)
                End If
            End If

            '    If (CheckBox601 ) And OptionButton601 Then
            '        If TurnDir > 0 Then
            '            TurnArea.AddPoint pPolygon.Point(pPolygon.PointCount - 2), 0
            '        Else
            '            TurnArea.AddPoint pPolygon.Point(1), 0
            '        End If
            '    End If
        Else
            K = m_BasePoints.PointCount - 1

            If m_TurnArea.PointCount > 0 Then
                pLPolygon = New ESRI.ArcGIS.Geometry.Polygon
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        pLPolygon.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I

                'DrawPolygon pLPolygon, 0
                'DrawPoint pLPolygon.Point(0), 0
                pLPolygon.AddPointCollection(m_TurnArea)
                m_TurnArea = pLPolygon
                'DrawPolygon TurnArea, 0
                '        Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
                'DrawPoint ptCur, 0
            Else
                ptCur = m_BasePoints.Point(0)
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        m_TurnArea.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I
                m_TurnArea.AddPoint(ptCur)
            End If

            '    Dim Side As Long
            '    k = BasePoints.PointCount - 1
            '
            '    Set pLPolygon = New Polygon
            '    For i = 1 To k
            '        Side = SideDef(ptDerPrj, ArDir, BasePoints.Point(i))
            '        If Side = TurnDir Then
            '            pLPolygon.AddPoint BasePoints.Point(i)
            '        End If
            '    Next i
            '    pLPolygon.AddPointCollection TurnArea
            '    Set TurnArea = pLPolygon
        End If

        ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
        AztCur = m_OutDir + arafTrn_OSplay.Value * TurnDir

        'If OptionButton601 Then
        '    Set tmpPoly1 = ZNR_Poly
        'Else
        tmpPoly1 = pFixPoly
        'End If

        curDist0 = 0.0
        K = tmpPoly1.PointCount - 1
        For I = 0 To K
            curDist = Point2LineDistancePrj(tmpPoly1.Point(I), ptCur, AztCur)
            If (curDist0 < curDist) Then
                iMax = I
                curDist0 = curDist
            End If
        Next I
        ptTmp = tmpPoly1.Point(iMax)

        m_TurnArea.AddPoint(ptTmp, 0)   '???????????????????????????????

        pSource = m_TurnArea
        pRPolygon = pSource.Clone

        tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
        tmpPoly1.AddPoint(ptTmp)
        tmpPoly1.AddPoint(m_TurnArea.Point(m_TurnArea.PointCount - 1))

        'DrawPoint OutPt, 0

        '============= To FIX ============
        CalcJoiningParams(TurnDirector.TypeCode, TurnDir, m_TurnArea, m_OutPt, m_OutDir)
        NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, m_TurnArea, m_OutPt, m_OutDir, tmpPoly1)
        EarlierTPDir = ReturnAngleInDegrees(NewPoly.Point(NewPoly.PointCount - 1), NewPoly.Point(NewPoly.PointCount - 2))
        '================
        'DrawPolygon tmpPoly1, RGB(128, 128, 0)
        'DrawPolygon NewPoly, RGB(255, 0, 255)
        NewPoly = RemoveAgnails(NewPoly)

        pTopoOper = tmpPoly1
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pTopoOper = pRPolygon
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pTopoOper = m_BasePoints
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        '    Set pTopoOper = NewPoly

        NewPoly = pTopoOper.Union(NewPoly)
        pTopoOper = NewPoly

        NewPoly = pTopoOper.Union(pRPolygon)
        pTopoOper = NewPoly

        'DrawPolygon NewPoly, 255

        '    If SideFrom2Angle(ArDir + 90.0 * Turndir, EarlierTPDir) * Turndir < 0 Then
        If Modulus((m_OutDir - _ArDir) * TurnDir, 360.0) > 90.0 Then
            NewPoly = pTopoOper.Union(tmpPoly1)
            pTopoOper = NewPoly
        End If

        tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
        tmpPoly1.AddPointCollection(m_TurnArea)

        pTopoOper = tmpPoly1
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        BaseArea = RemoveHoles(pTopoOper.Union(NewPoly)) '    Set BaseArea = RemoveHoles(NewPoly)
        'DrawPolygon BaseArea, 0
        '    If OptionButton401 Then
        '        Set pTopoOper = BaseArea
        '        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
        '    End If

        pTopoOper = pCircle
        BaseArea = PolygonIntersection(pCircle, BaseArea)
        '    Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
        'DrawPolygon BaseArea, 0

        On Error Resume Next
        If Not KKElem Is Nothing Then pGraphics.DeleteElement(KKElem)
        If Not K1K1Elem Is Nothing Then pGraphics.DeleteElement(K1K1Elem)
        If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
        If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
        If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
        If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
        If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        On Error GoTo 0

        KKElem = DrawPolyLine(KK, 0, 1.0, False)
        K1K1Elem = DrawPolyLine(K1K1, 0, 1.0, False)

        DrawPoly = PolygonIntersection(pCircle, Prim)
        '    Set DrawPoly = pTopoOper.Intersect(Prim, esriGeometry2Dimension)
        PrimElem = DrawPolygon(DrawPoly, RGB(128, 128, 0), , False)

        DrawPoly = PolygonIntersection(pCircle, SecL)
        '    Set DrawPoly = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
        SecLElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

        DrawPoly = PolygonIntersection(pCircle, SecR)
        '    Set DrawPoly = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
        SecRElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

        TurnAreaElem = DrawPolygon(BaseArea, 255, , False)
        NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        '    If ButtonControl3State Then
        pGraphics.AddElement(TurnAreaElem, 0)
        TurnAreaElem.Locked = True
        '    End If

        '    If ButtonControl4State Then
        pGraphics.AddElement(PrimElem, 0)
        PrimElem.Locked = True
        pGraphics.AddElement(SecLElem, 0)
        SecLElem.Locked = True
        pGraphics.AddElement(SecRElem, 0)
        SecRElem.Locked = True
        '    End If

        '    If ButtonControl6State Then
        pGraphics.AddElement(KKElem, 0)
        KKElem.Locked = True
        pGraphics.AddElement(K1K1Elem, 0)
        K1K1Elem.Locked = True
        '    End If

        '    If ButtonControl5State Then
        pGraphics.AddElement(NomTrackElem, 0)
        NomTrackElem.Locked = True
        '    End If

        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        '    RefreshCommandBar mTool, 124
        '=========== End To FIX ==========
        DrawTrail()
        Return 1
    End Function

    Private Function UpdateToCourse(ByVal OutAzt As Double) As Integer
        Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
        Dim curDist0 As Double
        Dim curDist As Double
        Dim AztCur As Double
        Dim TurnR As Double
        Dim fTASl As Double
        Dim fTmp As Double

        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pLPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pRPolygon As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolygon1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim NewFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pSource As ESRI.ArcGIS.esriSystem.IClone

        Dim iMax As Integer
        Dim K As Integer
        Dim I As Integer

        Dim NewPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim DrawPoly As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D

        NewFullPoly = ReArrangePolygon(pFullPoly, IFprj, _ArDir)
        'DrawPolygon NewFullPoly, 0
        If Not OptionButton0401.Checked Then 'на Высоте
            pSource = pFullPoly
            pRPolygon = pSource.Clone

            tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
            tmpPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
            tmpPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

            CutPoly(pRPolygon, tmpPoly, -1)
            pRPolygon = ReArrangePolygon(pRPolygon, PtSOC, _ArDir)
        Else
            pSource = NewFullPoly
            pRPolygon = pSource.Clone
        End If

        fTmp = Max(FicTHRprj.Z + arMATurnAlt.Value, TurnFixPnt.Z)
        fTASl = IAS2TAS(m_fIAS, fTmp, CurrADHP.ISAtC)
        TurnR = Bank2Radius(fBankAngle, fTASl)
        ''=========  End Params

        ptTmp = PointAlongPlane(TurnFixPnt, _ArDir + 90.0 * TurnDir, TurnR)
        pt0 = PointAlongPlane(ptTmp, OutAzt - 90.0 * TurnDir, TurnR)
        pt0.M = OutAzt

        MPtCollection = New ESRI.ArcGIS.Geometry.Multipoint
        MPtCollection.AddPoint(TurnFixPnt)
        MPtCollection.AddPoint(pt0)

        mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
        '=========================================================================================================
        Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
        '=====
        pPolyline = mPoly
        pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

        If OptionButton0603.Checked Then
            pPath.AddPoint(TurnDirector.pPtPrj)
        Else
            ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
            pProxi = pCircle
            If pProxi.ReturnDistance(ptCur) = 0.0 Then
                CircleVectorIntersect(ILS.pPtPrj, RModel, ptCur, (ptCur.M), ptTmp)
                pPath.AddPoint(ptTmp)
            End If
        End If

        pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
        pPolyline.AddGeometry(pPath)
        '=========================================================================================================
        If OptionButton0401.Checked And CheckBox0402.Checked Then
            pTopoOper = pFIXPoly_6
            ZNR_Poly = pTopoOper.ConvexHull
            pPolygon1 = New ESRI.ArcGIS.Geometry.Multipoint
            pPolygon1.AddPointCollection(ZNR_Poly)

            pSource = K1K1
            NewPoly = pSource.Clone
            pTransform2D = NewPoly
            pTransform2D.Move(3 * System.Math.Cos(DegToRad(_ArDir)), 3 * System.Math.Sin(DegToRad(_ArDir)))

            pPolygon1.AddPointCollection(NewPoly)
            pTopoOper = pPolygon1

            tmpPoly1 = ReArrangePolygon(pTopoOper.ConvexHull, IFprj, _ArDir)

            m_BasePoints = CreateBasePoints(tmpPoly1, K1K1, _ArDir, TurnDir)
        Else
            m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)
            ZNR_Poly = CreateBasePoints(NewFullPoly, KK, _ArDir, TurnDir)
        End If
        'Set ZNR_Poly = CreateBasePoints(ReArrangePolygon(NewFullPoly, PtSOC, ArDir), KK, ArDir, TurnDir)

        pTopoOper = ZNR_Poly
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, OutAzt, TurnDir, m_BasePoints)

        I = System.Math.Round(Modulus((OutAzt - _ArDir) * TurnDir, 360.0))
        TextBox0601.Text = CStr(I)

        If I > 255 Then
            TextBox0601.ForeColor = Color.Red
        Else
            TextBox0601.ForeColor = Color.Black
        End If
        'TextBox0601.Text = CStr(Round(Modulus((OutAzt - ArDir) * TurnDir, 360.0)))

        '    If OptionButton601 Then
        Dim Side As Integer
        If False Then
            If TurnDir > 0 Then
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.ToPoint)
                    m_TurnArea.AddPoint(KK.FromPoint)
                End If
            Else
                If m_TurnArea.PointCount > 0 Then
                    m_TurnArea.AddPoint(KK.ToPoint, 0)
                    m_TurnArea.AddPoint(KK.FromPoint, 0)
                Else
                    m_TurnArea.AddPoint(KK.FromPoint)
                    m_TurnArea.AddPoint(KK.ToPoint)
                End If
            End If
            'DrawPolygon TurnArea, RGB(0, 255, 255)

            '        If CheckBox601 Then
            '            If TurnDir > 0 Then
            '                TurnArea.AddPoint pFIXPoly.Point(pFIXPoly.PointCount - 2), 0
            '            Else
            '                TurnArea.AddPoint pFIXPoly.Point(1), 0
            '            End If
            '        End If
        Else 'If False Then
            K = m_BasePoints.PointCount - 1

            If m_TurnArea.PointCount > 0 Then
                pLPolygon = New ESRI.ArcGIS.Geometry.Polygon
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        pLPolygon.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I
                pLPolygon.AddPointCollection(m_TurnArea)
                m_TurnArea = pLPolygon
                ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
            Else
                ptCur = m_BasePoints.Point(0)
                For I = 1 To K
                    Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
                    If Side = TurnDir Then
                        m_TurnArea.AddPoint(m_BasePoints.Point(I))
                    End If
                Next I
                m_TurnArea.AddPoint(ptCur)
            End If
        End If

        AztCur = OutAzt + arafTrn_OSplay.Value * TurnDir

        'If OptionButton601 Then
        '    Set tmpPoly1 = ZNR_Poly
        'Else
        tmpPoly1 = pFixPoly
        'End If

        curDist0 = 0.0
        K = tmpPoly1.PointCount - 1
        For I = 0 To K
            curDist = Point2LineDistancePrj(tmpPoly1.Point(I), ptCur, AztCur)
            If (curDist0 < curDist) Then
                iMax = I
                curDist0 = curDist
            End If
        Next I
        ptTmp = tmpPoly1.Point(iMax)

        m_TurnArea.AddPoint(ptTmp, 0)

        tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
        tmpPoly1.AddPoint(ptTmp)
        tmpPoly1.AddPoint(m_TurnArea.Point(m_TurnArea.PointCount - 1))

        '============= To Course ============
        curDist0 = CircleVectorIntersect(ptTmp, RModel + RModel, ptCur, AztCur)

        ptCur = PointAlongPlane(ptCur, OutAzt - arafTrn_OSplay.Value * TurnDir, curDist0)
        tmpPoly1.AddPoint(ptCur)
        m_TurnArea.AddPoint(ptCur)

        ptCur = PointAlongPlane(ptTmp, OutAzt + arafTrn_ISplay.Value * TurnDir, RModel + RModel)
        tmpPoly1.AddPoint(ptCur)
        m_TurnArea.AddPoint(ptCur)
        EarlierTPDir = ReturnAngleInDegrees(m_TurnArea.Point(0), ptCur)
        '================================================================
        'DrawPolygon TurnArea, 0
        'DrawPolygon tmpPoly1, RGB(0, 255, 255)
        pTopoOper = tmpPoly1
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        pTopoOper = m_BasePoints
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        BaseArea = m_BasePoints

        If Modulus((OutAzt - _ArDir) * TurnDir, 360.0) > 90.0 Then
            '    If SideFrom2Angle(ArDir + 90.0 * TurnDir, EarlierTPDir) * TurnDir < 0 Then
            BaseArea = pTopoOper.Union(tmpPoly1)
        End If

        tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
        tmpPoly1.AddPointCollection(m_TurnArea)

        pTopoOper = tmpPoly1
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()

        tmpPoly1 = pTopoOper.Union(BaseArea)
        BaseArea = RemoveHoles(tmpPoly1)

        If OptionButton0601.Checked Then
            pTopoOper = BaseArea
            '        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
        End If

        pTopoOper = pCircle
        '    DrawPolygon BaseArea, 0
        '    DrawPolygon pCircle, 255

        '    Set DrawPoly = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
        DrawPoly = PolygonIntersection(pCircle, BaseArea)
        '================================================================
        On Error Resume Next
        If Not KKElem Is Nothing Then pGraphics.DeleteElement(KKElem)
        If Not K1K1Elem Is Nothing Then pGraphics.DeleteElement(K1K1Elem)
        If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
        If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
        If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
        If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
        If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
        If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
        On Error GoTo 0

        PrimElem = Nothing
        SecRElem = Nothing
        SecLElem = Nothing
        Sec0Elem = Nothing

        '    Set KKElem = DrawLine(KK, 0, 1.0, False)
        '    Set K1K1Elem = DrawLine(K1K1, 0, 1.0, False)
        KKElem = DrawPolyLine(KK, 0, 1.0, False)
        K1K1Elem = DrawPolyLine(K1K1, 0, 1.0, False)

        TurnAreaElem = DrawPolygon(DrawPoly, 255, , False)

        '    DrawPolygon DrawPoly, 0
        NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

        '    If ButtonControl3State Then
        pGraphics.AddElement(TurnAreaElem, 0)
        TurnAreaElem.Locked = True
        '    End If

        '    If ButtonControl6State Then
        pGraphics.AddElement(KKElem, 0)
        KKElem.Locked = True
        pGraphics.AddElement(K1K1Elem, 0)
        K1K1Elem.Locked = True
        '    End If

        '    If ButtonControl5State Then
        pGraphics.AddElement(NomTrackElem, 0)
        NomTrackElem.Locked = True
        '    End If

        DrawTrail()

        '    RefreshCommandBar mTool, 124
    End Function

    Private Function ConvertTracToPoints(ByRef TrackPoints() As ReportPoint) As Double
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer

        Dim PDG As Double
        Dim course As Double

        Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtPrev As ESRI.ArcGIS.Geometry.IPoint
        Dim pPtNext As ESRI.ArcGIS.Geometry.IPoint

        Dim pPtC1 As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline
        Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry
        Dim pPoints(10) As ESRI.ArcGIS.Geometry.IPoint

        N = 0
        ReDim TrackPoints(10)

        pPtGeo = New ESRI.ArcGIS.Geometry.Point
        pPtC1 = New ESRI.ArcGIS.Geometry.Polyline
        course = CDbl(TextBox0002.Text)

        'IF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        pPtGeo.PutCoords(IFprj.X, IFprj.Y)
        pGeometry = pPtGeo
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(0).Description = "IF"
        TrackPoints(0).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
        TrackPoints(0).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
        TrackPoints(0).Direction = System.Math.Round(course, 2)
        TrackPoints(0).PDG = NO_DATA_VALUE
        TrackPoints(0).Altitude = IFprj.Z + FicTHRprj.Z

        TrackPoints(0).Radius = NO_DATA_VALUE
        TrackPoints(0).Turn = 0
        TrackPoints(0).CenterLat = ""
        TrackPoints(0).CenterLon = ""

        TrackPoints(0).TurnAngle = NO_DATA_VALUE
        TrackPoints(0).TurnArcLen = NO_DATA_VALUE
        pPoints(0) = IFprj

        'FAP++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        pPtGeo.PutCoords(ptFAP.X, ptFAP.Y)
        pGeometry = pPtGeo
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(1).Description = "FAP"
        TrackPoints(1).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
        TrackPoints(1).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
        TrackPoints(1).Direction = System.Math.Round(course, 2)
        TrackPoints(1).PDG = NO_DATA_VALUE
        TrackPoints(1).Altitude = ptFAP.Z + FicTHRprj.Z

        TrackPoints(1).Radius = NO_DATA_VALUE
        TrackPoints(1).Turn = 0

        TrackPoints(1).CenterLat = ""
        TrackPoints(1).CenterLon = ""

        TrackPoints(1).TurnAngle = NO_DATA_VALUE
        TrackPoints(1).TurnArcLen = NO_DATA_VALUE
        pPoints(1) = ptFAP

        'MAPt++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        pPtGeo.PutCoords(pMAPt.X, pMAPt.Y)
        pGeometry = pPtGeo
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(2).Description = "MAPt"
        TrackPoints(2).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
        TrackPoints(2).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
        TrackPoints(2).Direction = System.Math.Round(course, 2)
        TrackPoints(2).PDG = NO_DATA_VALUE
        TrackPoints(2).Altitude = pMAPt.Z + FicTHRprj.Z

        TrackPoints(2).Radius = NO_DATA_VALUE
        TrackPoints(2).Turn = 0
        TrackPoints(2).CenterLat = ""
        TrackPoints(2).CenterLon = ""
        TrackPoints(2).TurnAngle = NO_DATA_VALUE
        TrackPoints(2).TurnArcLen = NO_DATA_VALUE
        pPoints(2) = pMAPt

        'SOC++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        pPtGeo.PutCoords(PtSOC.X, PtSOC.Y)
        pGeometry = pPtGeo
        pGeometry.SpatialReference = pSpRefPrj
        pGeometry.Project(pSpRefGeo)

        TrackPoints(3).Description = "SOC"
        TrackPoints(3).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
        TrackPoints(3).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
        TrackPoints(3).Direction = System.Math.Round(course, 2)
        TrackPoints(3).PDG = NO_DATA_VALUE
        TrackPoints(3).Altitude = PtSOC.Z + FicTHRprj.Z

        TrackPoints(3).Radius = NO_DATA_VALUE
        TrackPoints(3).Turn = 0
        TrackPoints(3).CenterLat = ""
        TrackPoints(3).CenterLon = ""
        TrackPoints(3).TurnAngle = NO_DATA_VALUE
        TrackPoints(3).TurnArcLen = NO_DATA_VALUE
        pPoints(3) = PtSOC

        ConvertTracToPoints = 0.0

        For I = 0 To 2
            If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
            pPtC1.AddPoint(pPoints(I))
            pPtC1.AddPoint(pPoints(I + 1))
            pPolyline = CalcTrajectoryFromMultiPoint(pPtC1)
            TrackPoints(I).ToNext = pPolyline.Length
            ConvertTracToPoints = ConvertTracToPoints + TrackPoints(I).ToNext
        Next I
        N = 4
        'TP++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        If Not CheckBox0301.Checked Then
            pPtGeo.PutCoords(pTermPt.X, pTermPt.Y)
            pGeometry = pPtGeo
            pGeometry.SpatialReference = pSpRefPrj
            pGeometry.Project(pSpRefGeo)

            TrackPoints(N).Description = "Straight MA termination point"
            TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
            TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
            TrackPoints(N).Direction = System.Math.Round(course, 2)
            TrackPoints(N).PDG = fMissAprPDG
            TrackPoints(N).Altitude = pTermPt.Z
            TrackPoints(N - 1).ToNext = pStraightNomLine.Length - TrackPoints(N - 2).ToNext

            TrackPoints(N).Radius = NO_DATA_VALUE
            TrackPoints(N).Turn = 0
            TrackPoints(N).CenterLat = ""
            TrackPoints(N).CenterLon = ""
            TrackPoints(N).TurnAngle = NO_DATA_VALUE
            TrackPoints(N).TurnArcLen = NO_DATA_VALUE
            'pPoints(N) = pTermPt

            ConvertTracToPoints += TrackPoints(N - 1).ToNext
        Else
            If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
            pPtC1.AddPoint(pPoints(N - 1))
            pPtC1.AddPoint(TurnFixPnt)

            pPolyline = CalcTrajectoryFromMultiPoint(pPtC1)
            TrackPoints(N - 1).ToNext = pPolyline.Length

            pPtGeo.PutCoords(TurnFixPnt.X, TurnFixPnt.Y)
            pGeometry = pPtGeo
            pGeometry.SpatialReference = pSpRefPrj
            pGeometry.Project(pSpRefGeo)

            TrackPoints(N).Description = "TP"
            TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
            TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
            TrackPoints(N).Direction = System.Math.Round(course, 2)
            TrackPoints(N).PDG = NO_DATA_VALUE
            TrackPoints(N).Altitude = TurnFixPnt.Z

            TrackPoints(N).Radius = NO_DATA_VALUE
            TrackPoints(N).Turn = 0
            TrackPoints(N).CenterLat = ""
            TrackPoints(N).CenterLon = ""
            TrackPoints(N).TurnAngle = NO_DATA_VALUE
            TrackPoints(N).TurnArcLen = NO_DATA_VALUE
            J = N + 1
            '\============================================================================
            M = MPtCollection.PointCount + 1
            PDG = CDbl(ComboBox0005.Text) * 0.01

            ReDim Preserve TrackPoints(N + M)

            pPtNext = TurnFixPnt            'pPtNext.M = Azt2Dir(pPtGeo, fTmp)

            For J = N + 1 To M + N
                I = J - N
                TrackPoints(J).Radius = NO_DATA_VALUE

                If (I < M) Then
                    pPtPrj = MPtCollection.Point(I - 1)
                    pPtGeo.PutCoords(pPtPrj.X, pPtPrj.Y)
                    pGeometry = pPtGeo
                    pGeometry.SpatialReference = pSpRefPrj
                    pGeometry.Project(pSpRefGeo)
                    '=========================================================================
                    If (I And 1) = 1 Then
                        FillTurnParams(MPtCollection.Point(I - 1), MPtCollection.Point(I), TrackPoints(J))
                        TrackPoints(J).Description = My.Resources.str00521 + CStr((I + 1) \ 2) + My.Resources.str00523
                    Else
                        TrackPoints(J).Description = My.Resources.str00522 + CStr((I + 1) \ 2) + My.Resources.str00523
                        TrackPoints(J).Direction = System.Math.Round(Dir2Azt(pPtPrj, pPtPrj.M), 2)
                    End If
                Else
                    pPtPrj = mPoly.ToPoint
                    pPtPrj.M = pPtNext.M
                    pPtGeo.PutCoords(pPtPrj.X, pPtPrj.Y)
                    pGeometry = pPtGeo
                    pGeometry.SpatialReference = pSpRefPrj
                    pGeometry.Project(pSpRefGeo)
                    '=========================================================================

                    TrackPoints(J).Description = My.Resources.str00524 '"Конечная точка процедуры вылета"
                    '                TrackPoints(J).Direction = TrackPoints(J - 1).Direction
                End If

                pPtPrev = pPtNext
                pPtNext = pPtPrj

                If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
                pPtC1.AddPoint(pPtPrev)
                pPtC1.AddPoint(pPtNext)

                pPolyline = CalcTrajectoryFromMultiPoint(pPtC1)

                TrackPoints(J - 1).ToNext = pPolyline.Length
                TrackPoints(J).ToNext = NO_DATA_VALUE
                TrackPoints(J).Altitude = TrackPoints(J - 1).Altitude + pPolyline.Length * PDG

                TrackPoints(J).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
                TrackPoints(J).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
                ConvertTracToPoints = ConvertTracToPoints + TrackPoints(J - 1).ToNext
            Next J

            '    For I = 0 To N - 1
            '        ConvertTracToPoints = ConvertTracToPoints + TrackPoints(I).ToNext
            '    Next I
            '\============================================================================
        End If

        pPtC1 = Nothing
        pPtPrj = Nothing
        pPtGeo = Nothing
    End Function

    Private Function GetNumericalData() As List(Of GeoNumericalDataModel)
        Dim NumericalData As New List(Of GeoNumericalDataModel)

        Dim GuidanceNav As NavaidData = ILS
        Dim IntersectNav As NavaidData

        Dim HaveTP = CheckBox0301.Checked
        Dim HaveIntercept = OptionButton0602.Checked

        IntersectNav = IFNavDat(ComboBox0201.SelectedIndex)
        Dim IfHorAccuracy As Double = CalcHorisontalAccuracy(IFprj, GuidanceNav, IntersectNav)

        NumericalData.Add(New GeoNumericalDataModel With
        {
            .Role = "IF",
            .Accuracy = IfHorAccuracy,
            .Resolution = 0.0,
            .DesignatorDescription = GetDesignatedPointDescription(IFprj),
            .LegType = "Intermediate"
        })

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            IntersectNav = FAPNavDat(ComboBox0101.SelectedIndex)
            Dim FapHorAccuracy As Double = CalcHorisontalAccuracy(ptFAP, GuidanceNav, IntersectNav)

            NumericalData.Add(New GeoNumericalDataModel With
            {
                .Role = "FAP",
                .Accuracy = FapHorAccuracy,
                .Resolution = 0.0,
                .DesignatorDescription = GetDesignatedPointDescription(ptFAP),
                .LegType = "Intermediate"
            })
        End If

        If HaveTP Then
            If OptionButton0402.Checked Then
                IntersectNav = TPInterNavDat(ArrayNum, ComboBox0501.SelectedIndex)
                Dim MatfHorAccuracy As Double = CalcHorisontalAccuracy(TurnFixPnt, GuidanceNav, IntersectNav)

                NumericalData.Add(New GeoNumericalDataModel With
                {
                    .Role = "MATF",
                    .Accuracy = MatfHorAccuracy,
                    .Resolution = 0.0,
                    .DesignatorDescription = GetDesignatedPointDescription(TurnFixPnt),
                    .LegType = "MissedApproach"
                })
            End If

            If HaveIntercept Then
                GuidanceNav = WPT_FIXToNavaid(TurnDirector)
                IntersectNav = TerInterNavDat(ComboBox0902.SelectedIndex)
                Dim MatfMahfHorAccuracy As Double = CalcHorisontalAccuracy(TerFixPnt, GuidanceNav, IntersectNav)

                NumericalData.Add(New GeoNumericalDataModel With
                {
                    .Role = IIf(CheckBox0901.Checked, "MATF", "MAHF"),
                    .Accuracy = MatfMahfHorAccuracy,
                    .Resolution = 0.0,
                    .DesignatorDescription = GetDesignatedPointDescription(TerFixPnt),
                    .LegType = "MissedApproach"
                })
            End If


            If CheckBox0901.Checked Then
                Dim j As Integer = 1

                Do While (j < TSC)
                    Dim currSegment As TraceSegment = Trace(j)
                    If (j < TSC - 1) Then
                        Dim nextSegment As TraceSegment = Trace(j + 1)
                        If (nextSegment.SegmentCode = eSegmentType.straight) And (currSegment.LegType = CodeSegmentPath.DF) Then
                            currSegment = nextSegment
                            j += 1
                        End If
                    End If
                    j += 1

                    IntersectNav = currSegment.InterceptionNav
                    GuidanceNav = currSegment.GuidanceNav
                    Dim MatfMahfHorAccuracy As Double = CalcHorisontalAccuracy(currSegment.ptOut, GuidanceNav, IntersectNav)

                    NumericalData.Add(New GeoNumericalDataModel With
                    {
                        .Role = IIf(j < TSC, "MATF", "MAHF"),
                        .Accuracy = MatfMahfHorAccuracy,
                        .Resolution = 0.0,
                        .DesignatorDescription = GetDesignatedPointDescription(currSegment.ptOut),
                        .LegType = "MissedApproach"
                    })
                Loop
            End If
        End If

        Return NumericalData
    End Function

    Private Sub SaveGeometry(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
        Dim I As Integer
        Dim TraceLen As Double

        Dim TrackPoints() As ReportPoint

        GeomRep = New ReportFile()
        TraceLen = ConvertTracToPoints(TrackPoints)

        'GeomRep.RefHeight = FicTHRprj.Z

        GeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + My.Resources.str00811)

        GeomRep.WriteMessage(My.Resources.str00002 + " - " + RepFileTitle + ": " + My.Resources.str00811)
        GeomRep.WriteHeader(pReport)

        GeomRep.WriteMessage()
        GeomRep.WriteMessage()

        For I = 0 To UBound(TrackPoints)
            GeomRep.WritePoint(TrackPoints(I))
        Next I
        GeomRep.WriteMessage()

        GeomRep.Param(My.Resources.str00851, CStr(ConvertDistance(TraceLen, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit)

        GeomRep.CloseFile()

    End Sub

    Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader) 'Precision
        ProtRep = New ReportFile()

        'ProtRep.RefHeight = FicTHRprj.Z

        ProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + My.Resources.str00815)

        ProtRep.WriteMessage(My.Resources.str00002 + " - " + RepFileTitle + ": " + My.Resources.str00815)
        ProtRep.WriteHeader(pReport, True)

        ProtRep.WriteMessage()
        ProtRep.WriteMessage()

        ProtRep.lListView = PrecReportFrm.ListView01
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(0).Text)

        ProtRep.lListView = PrecReportFrm.ListView02
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(1).Text)

        ProtRep.lListView = PrecReportFrm.ListView03
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(2).Text)

        ProtRep.lListView = PrecReportFrm.ListView04
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(3).Text)

        ProtRep.lListView = PrecReportFrm.ListView05
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(4).Text)

        ProtRep.lListView = PrecReportFrm.ListView06
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(5).Text)

        ProtRep.lListView = PrecReportFrm.ListView07
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(6).Text)

        ProtRep.lListView = PrecReportFrm.ListView08
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(7).Text)

        ProtRep.lListView = PrecReportFrm.ListView09
        ProtRep.WriteTab(PrecReportFrm.MultiPage1.TabPages.Item(8).Text)

        ProtRep.CloseFile()
    End Sub

    Private Sub SaveAccuracy(RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader)
        AccurRep = New ReportFile()

        AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + My.Resources.str00805)
        AccurRep.WriteMessage(My.Resources.str00002 + " - " + RepFileTitle + ": " + My.Resources.str00805)
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteHeader(pReport)
        AccurRep.Param("Distance accuracy", _settings.DistancePrecision, DistanceConverter(DistanceUnit).Unit)
        AccurRep.Param("Angle accuracy", _settings.AnglePrecision, "degrees")

        AccurRep.WriteMessage()
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        AccurRep.WriteMessage("=================================================")
        AccurRep.WriteMessage()

        Dim HaveTP As Boolean = CheckBox0301.Checked
        Dim HaveIntercept As Boolean = OptionButton0602.Checked

        Dim GuidNav As NavaidData = ILS
        Dim IntersectNav As NavaidData

        'Leg 1 Initial to Final Approach =================================================================================
        IntersectNav = IFNavDat(ComboBox0201.SelectedIndex)
        SaveFixAccurasyInfo(AccurRep, IFprj, "IF", GuidNav, IntersectNav)

        If CheckBox0101.Enabled And CheckBox0101.Checked Then
            IntersectNav = FAPNavDat(ComboBox0101.SelectedIndex)
            SaveFixAccurasyInfo(AccurRep, ptFAP, "FAP", GuidNav, IntersectNav)
        End If

        If Not HaveTP Then
            AccurRep.CloseFile()
            Return
        End If

        'Leg 5 Straight Missed Approach ============================================================================
        If OptionButton0402.Checked Then
            IntersectNav = TPInterNavDat(ArrayNum, ComboBox0501.SelectedIndex)
            SaveFixAccurasyInfo(AccurRep, TurnFixPnt, "MATF", GuidNav, IntersectNav)
        End If

        'Leg 6-7 Missed Approach Termination ==========================================================================

        If HaveIntercept Then
            GuidNav = WPT_FIXToNavaid(TurnDirector)
            IntersectNav = TerInterNavDat(ComboBox0902.SelectedIndex)

            SaveFixAccurasyInfo(AccurRep, TerFixPnt, IIf(CheckBox0901.Checked, "MATF", "MAHF"), GuidNav, IntersectNav, Not CheckBox0901.Checked)
        End If

        '=============================================================================================================

        If CheckBox0901.Checked Then
            Dim i As Integer = 1

            Do While (i < TSC)
                Dim currSegment As TraceSegment = Trace(i)
                If (i < TSC - 1) Then
                    Dim nextSegment As TraceSegment = Trace(i + 1)
                    If (nextSegment.SegmentCode = eSegmentType.straight) And (currSegment.LegType = CodeSegmentPath.DF) Then
                        currSegment = nextSegment
                        i += 1
                    End If
                End If
                i += 1

                IntersectNav = currSegment.InterceptionNav
                GuidNav = currSegment.GuidanceNav
                SaveFixAccurasyInfo(AccurRep, currSegment.ptOut, IIf(i < TSC, "MATF", "MAHF"), GuidNav, IntersectNav, i >= TSC)
            Loop
        End If

        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        AccurRep.CloseFile()
    End Sub

    Private Sub SaveLog(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)   'PrecisionForm

        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem
        Dim fOCH As Double
        Dim fOCA As Double
        Dim fTmp As Double
        Dim sTmp As String
        LogRep = New ReportFile()
        'LogRep.RefHeight = FicTHRprj.Z

        LogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + My.Resources.str00817)

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        LogRep.H1(My.Resources.str00002 + " - " + RepFileTitle + ": " + My.Resources.str00817)
        LogRep.WriteHeader(pReport)
        LogRep.WriteMessage()
        '=============================================================================
        LogRep.WriteMessage(My.Resources.str00002 + " - " + My.Resources.str00817)
        LogRep.WriteMessage()

        LogRep.WriteText("<TABLE border=1 style= ""font-family: Arial, Sans-Serif; font-size:12"">")
        LogRep.WriteText("<TD><CAPTION><STRONG>Publication data</STRONG></CAPTION></TD>")
        LogRep.WriteText("<TBODY>")
        LogRep.WriteText("<TR align=center><TD><b>Straight-in approach</b></TD></TR>")
        LogRep.WriteText("<TR><TD>")

        LogRep.Page("Publication data")
        LogRep.ExH1("Straight-in approach")

        If Not CheckBox0301.Checked Then
            fOCH = fMisAprOCH 'DeConvertHeight(CDbl(TextBox0303.Text))
        Else
            fOCH = fTA_OCH 'DeConvertHeight(CDbl(TextBox0906.Text))
        End If

        fOCA = fOCH + FicTHRprj.Z

        '=============================================================================
        sTmp = CStr(ConvertHeight(fOCA, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
        LogRep.WriteString("OCA calculated  - " + sTmp) '(расчетная)

        'If HeightUnit = 0 Then
        '    fTmp = Round(fOCA * 0.2 + 0.4999) * 5
        'ElseIf HeightUnit = 1 Then
        '    fTmp = Round(ConvertHeight(fOCA, 0) * 0.1 + 0.4999) * 10
        'End If

        fTmp = ConvertHeight(fOCA, 0)
        sTmp = CStr(System.Math.Round(fTmp + 0.499999)) + " " + HeightConverter(HeightUnit).Unit

        LogRep.WriteString("OCA rounded  - " + sTmp) '(округленная по правилам PANS-OPS)
        LogRep.WriteString("OCA for publication - " + sTmp)
        LogRep.WriteString()
        '=============================================================================
        sTmp = CStr(ConvertHeight(FicTHRprj.Z, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
        LogRep.WriteString("OCH reference point  - " + sTmp) '(уровень отчета OCH)

        sTmp = CStr(ConvertHeight(fOCH, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
        LogRep.WriteString("OCH calculated  - " + sTmp) '(расчетная)

        'If HeightUnit = 0 Then
        '    fTmp = Round(fOCH * 0.2 + 0.4999) * 5
        'ElseIf HeightUnit = 1 Then
        '    fTmp = Round(ConvertHeight(fOCH, 0) * 0.1 + 0.4999) * 10
        'End If

        fTmp = ConvertHeight(fOCH, 0)

        sTmp = CStr(System.Math.Round(fTmp + 0.499999)) + " " + HeightConverter(HeightUnit).Unit
        LogRep.WriteString("OCH rounded  - " + sTmp) '(округленная по правилам PANS-OPS)

        LogRep.WriteString("OCH for publication - " + sTmp)
        LogRep.WriteString("Missed approach climb gradient - " + System.Net.WebUtility.HtmlEncode(ComboBox0005.Text + " %"))

        LogRep.WriteText("</TD></TR></TBODY></TABLE>")

        LogRep.WriteString()
        LogRep.WriteString()
        '=============================================================================
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        'LogRep.WriteMessage(RepFileTitle)
        'LogRep.WriteParam(My.Resources.str813, CStr(Today) + " - " + CStr(TimeOfDay))

        LogRep.WriteMessage()
        LogRep.WriteMessage()

        '===========================================================================
        LogRep.Page("Log")

        LogRep.ExH2(MultiPage1.TabPages.Item(0).Text)

        LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(0).Text + " ]")
        LogRep.WriteMessage()

        LogRep.Param(Label0001_0.Text, ComboBox0001.Text)
        LogRep.Param(_Label0001_1.Text, ComboBox0002.Text)
        LogRep.Param(_Label0001_14.Text, ComboBox0003.Text)
        LogRep.Param(_Label0001_15.Text, ComboBox0004.Text)
        LogRep.WriteMessage()

        LogRep.Param(_Label0001_2.Text, TextBox0001.Text, _Label0001_8.Text)
        LogRep.Param(_Label0001_16.Text, TextBox0007.Text, _Label0001_22.Text)
        LogRep.Param(_Label0001_3.Text, TextBox0002.Text, _Label0001_9.Text)
        LogRep.Param(_Label0001_17.Text, TextBox0008.Text, _Label0001_23.Text)
        LogRep.WriteMessage()

        LogRep.Param(_Label0001_18.Text, TextBox0009.Text)
        LogRep.Param(_Label0001_4.Text, TextBox0003.Text, _Label0001_10.Text)
        LogRep.Param(_Label0001_5.Text, TextBox0004.Text, _Label0001_11.Text)
        LogRep.Param(_Label0001_19.Text, TextBox0010.Text, _Label0001_24.Text)
        LogRep.Param(_Label0001_20.Text, TextBox0011.Text, _Label0001_25.Text)
        LogRep.WriteMessage()

        LogRep.Param(_Label0001_6.Text, TextBox0005.Text, _Label0001_12.Text)
        LogRep.Param(_Label0001_7.Text, TextBox0006.Text, _Label0001_13.Text)
        LogRep.Param(Label0001_21.Text, ComboBox0005.Text, _Label0001_26.Text)
        LogRep.WriteMessage()
        LogRep.WriteMessage()
        '===========================================================================
        LogRep.ExH2(MultiPage1.TabPages.Item(1).Text)
        LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(1).Text + " ]")
        LogRep.WriteMessage()
        LogRep.Param(_Label0101_0.Text, TextBox0101.Text, DistanceConverter(DistanceUnit).Unit)
        LogRep.Param(ComboBox0103.Text, TextBox0102.Text, HeightConverter(HeightUnit).Unit)
        LogRep.WriteMessage(Frame0101.Text)
        LogRep.Param(ComboBox0104.Text, TextBox0103.Text, HeightConverter(HeightUnit).Unit)
        LogRep.Param(_Label0101_5.Text, TextBox0104.Text)
        LogRep.Param(_Label0101_2.Text, TextBox0105.Text, HeightConverter(HeightUnit).Unit)

        If CheckBox0101.Enabled And CheckBox0101.Checked Then LogRep.Param(CheckBox0101.Text, ComboBox0101.Text)

        LogRep.WriteMessage((Frame0103.Text))

        LogRep.Param(_Label0101_3.Text, ComboBox0102.Text)
        LogRep.Param(_Label0101_6.Text, TextBox0106.Text, DistanceConverter(DistanceUnit).Unit)
        For I = 0 To ListView0101.Items.Count - 1
            itmX = ListView0101.Items.Item(I)
            LogRep.WriteMessage((itmX.Text)) ', itmX.SubItems(1)
        Next I
        LogRep.WriteMessage()
        LogRep.WriteMessage()
        '===========================================================================
        LogRep.ExH2(MultiPage1.TabPages.Item(2).Text)
        LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(2).Text + " ]")
        LogRep.WriteMessage()
        LogRep.WriteMessage((Frame0201.Text))
        LogRep.WriteMessage()
        LogRep.Param(label0201_10.Text, ComboBox0201.Text, label0201_11.Text)
        LogRep.Param(label0201_12.Text, TextBox0201.Text, _label0201_12.Text)
        LogRep.WriteMessage(label0201_14.Text)

        If IFNavDat(ComboBox0201.SelectedIndex).TypeCode = eNavaidType.DME Then
            If OptionButton0201.Checked Then
                LogRep.WriteMessage(OptionButton0201.Text)
            Else
                LogRep.WriteMessage(OptionButton0202.Text)
            End If
        End If

        LogRep.WriteMessage(Frame0202.Text)
        LogRep.WriteMessage()
        LogRep.Param(ComboBox0202.Text, TextBox0202.Text, label0201_00.Text)
        LogRep.Param(label0201_01.Text, TextBox0203.Text, label0201_02.Text)
        LogRep.Param(label0201_03.Text, TextBox0204.Text, label0201_04.Text)
        LogRep.Param(label0201_05.Text, TextBox0205.Text, label0201_06.Text)
        LogRep.Param(label0201_08.Text, TextBox0209.Text, label0201_09.Text)
        LogRep.Param(label0201_19.Text, TextBox0210.Text)

        LogRep.WriteMessage()
        LogRep.WriteMessage()
        '===========================================================================
        LogRep.ExH2(MultiPage1.TabPages.Item(3).Text)
        LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(3).Text + " ]")
        LogRep.WriteMessage()
        LogRep.Param(ComboBox0301.Text, TextBox0301.Text, HeightConverter(HeightUnit).Unit)
        LogRep.WriteMessage()
        LogRep.Param(_Label0301_1.Text, TextBox0302.Text, HeightConverter(HeightUnit).Unit)
        LogRep.Param(_Label0301_5.Text, TextBox0305.Text)
        LogRep.WriteMessage()

        LogRep.Param(_Label0301_2.Text, TextBox0303.Text, HeightConverter(HeightUnit).Unit)
        LogRep.Param(_Label0301_6.Text, TextBox0306.Text)
        LogRep.WriteMessage()

        LogRep.Param(_Label0301_4.Text, TextBox0304.Text, "%")
        LogRep.Param(_Label0301_7.Text, TextBox0307.Text, DistanceConverter(DistanceUnit).Unit)

        LogRep.WriteMessage()
        LogRep.Param(_Label0301_13.Text, TextBox0308.Text, _Label0301_15.Text)
        LogRep.Param(label0301_16.Text, TextBox0309.Text, label0301_17.Text)

        LogRep.WriteMessage()
        LogRep.WriteMessage()


        If CheckBox0301.Checked Then
            '===========================================================================
            LogRep.ExH2(MultiPage1.TabPages.Item(4).Text)
            LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(4).Text + " ]")
            LogRep.WriteMessage()
            LogRep.Param(_Label0401_4.Text, ComboBox0401.Text)
            LogRep.Param(_Label0401_0.Text, TextBox0401.Text, "°")
            LogRep.Param(_Label0401_1.Text, TextBox0402.Text, SpeedConverter(SpeedUnit).Unit)
            LogRep.Param(_Label0401_2.Text, TextBox0403.Text)
            LogRep.Param(_Label0401_5.Text, TextBox0404.Text)
            LogRep.WriteMessage(_Label0401_3.Text)

            LogRep.WriteMessage()
            If OptionButton0401.Checked Then
                LogRep.Param(Frame0401.Text, OptionButton0401.Text)
            Else
                LogRep.Param(Frame0401.Text, OptionButton0402.Text)
            End If
            LogRep.WriteMessage()
            LogRep.WriteMessage()
            '===========================================================================
            LogRep.ExH2(MultiPage1.TabPages.Item(5).Text)
            LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(5).Text + " ]")
            LogRep.WriteMessage()
            LogRep.Param(_Label0501_5.Text, TextBox0505.Text, HeightConverter(HeightUnit).Unit)
            LogRep.Param(_Label0501_0.Text, TextBox0501.Text)
            LogRep.WriteMessage()

            LogRep.Param(_Label0501_6.Text, TextBox0506.Text, HeightConverter(HeightUnit).Unit)
            LogRep.Param(_Label0501_1.Text, TextBox0502.Text)

            LogRep.Param(ComboBox0502.Text, TextBox0503.Text, HeightConverter(HeightUnit).Unit)

            If OptionButton0401.Checked Then
                LogRep.Param(ComboBox0503.Text, TextBox0507.Text, HeightConverter(HeightUnit).Unit)
            Else
                LogRep.Param(_Label0501_7.Text, TextBox0507.Text, DistanceConverter(DistanceUnit).Unit)
            End If

            LogRep.Param(_Label0501_3.Text, TextBox0504.Text)
            LogRep.Param(_Label0501_8.Text, TextBox0508.Text, HeightConverter(HeightUnit).Unit)
            LogRep.WriteMessage()

            LogRep.WriteMessage(_Label0501_4.Text)
            For I = 0 To ListView0501.Items.Count - 1
                itmX = ListView0501.Items.Item(I)
                LogRep.WriteMessage((itmX.Text)) ', itmX.SubItems(1)
            Next I

            '        If Frame0501.Visible Then
            LogRep.WriteMessage((Frame0501.Text))
            LogRep.WriteMessage()
            '        End If
            LogRep.Param(_Label0501_9.Text, ComboBox0501.Text, _Label0501_11.Text)
            LogRep.Param(_Label0501_10.Text, TextBox0509.Text, _Label0501_12.Text)

            If OptionButton0402.Checked Then
                If TPInterNavDat(ArrayNum, ComboBox0501.SelectedIndex).TypeCode = eNavaidType.DME Then
                    If OptionButton0501.Checked Then
                        LogRep.WriteMessage((OptionButton0501.Text))
                    Else
                        LogRep.WriteMessage((OptionButton0502.Text))
                    End If
                End If
            End If

            LogRep.WriteMessage()
            LogRep.WriteMessage()
            '===========================================================================
            LogRep.ExH2(MultiPage1.TabPages.Item(6).Text)
            LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(6).Text + " ]")
            LogRep.WriteMessage()

            If OptionButton0601.Checked Then LogRep.WriteMessage(OptionButton0601.Text)
            If OptionButton0602.Checked Then LogRep.WriteMessage(OptionButton0602.Text)
            If OptionButton0603.Checked Then LogRep.WriteMessage(OptionButton0603.Text)

            LogRep.Param(_Label0601_0.Text, TextBox0601.Text, _Label0601_3.Text)
            LogRep.Param(_Label0601_5.Text, TextBox0603.Text, _Label0601_8.Text)

            If _Label0601_1.Visible Then LogRep.Param(_Label0601_1.Text, ComboBox0601.Text, _Label0601_4.Text)
            If _Label0601_6.Visible Then LogRep.Param(_Label0601_6.Text, TextBox0604.Text, _Label0601_9.Text)
            If _Label0601_11.Visible Then LogRep.Param(_Label0601_11.Text, TextBox0605.Text, _Label0601_12.Text)
            If _Label0601_2.Visible Then LogRep.Param(_Label0601_2.Text, TextBox0602.Text, _Label0601_10.Text)
            If _Label0601_7.Visible Then LogRep.WriteMessage(_Label0601_7.Text)

            If CheckBox0601.Visible And CheckBox0601.Enabled And CheckBox0601.Checked Then
                LogRep.WriteMessage((CheckBox0601.Text))
            End If
            LogRep.WriteMessage()
            LogRep.WriteMessage()

            If OptionButton0601.Checked Or (CheckBox0601.Visible And CheckBox0601.Enabled) Then
                '===========================================================================
                LogRep.ExH2(MultiPage1.TabPages.Item(7).Text)
                LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(7).Text + " ]")
                LogRep.WriteMessage()

                LogRep.WriteMessage((Frame0701.Text))
                If TextBox0701.Text <> "" Then LogRep.Param(_Label0701_0.Text, TextBox0701.Text)
                If OptionButton0701.Checked Then
                    LogRep.WriteMessage((OptionButton0701.Text))
                ElseIf OptionButton0702.Checked Then
                    LogRep.WriteMessage((OptionButton0702.Text))
                Else
                    LogRep.WriteMessage((OptionButton0703.Text))
                End If
                LogRep.WriteMessage()

                LogRep.WriteMessage((Frame0702.Text))
                If TextBox0702.Text <> "" Then LogRep.Param(_Label0701_1.Text, TextBox0702.Text)
                If OptionButton0704.Checked Then
                    LogRep.WriteMessage((OptionButton0704.Text))
                ElseIf OptionButton0705.Checked Then
                    LogRep.WriteMessage((OptionButton0705.Text))
                Else
                    LogRep.WriteMessage((OptionButton0706.Text))
                End If
                LogRep.WriteMessage()
                LogRep.WriteMessage()
                '===========================================================================
                LogRep.ExH2(MultiPage1.TabPages.Item(8).Text)
                LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(8).Text + " ]")
                LogRep.WriteMessage()
                LogRep.WriteMessage((Frame0801.Text))
                If CheckBox0801.Checked Then LogRep.WriteMessage((CheckBox0801.Text))
                If CheckBox0802.Checked Then LogRep.WriteMessage((CheckBox0802.Text))
                If CheckBox0802.Checked Then LogRep.WriteMessage((CheckBox0802.Text))
                LogRep.WriteMessage()

                LogRep.WriteMessage((Frame0802.Text))
                If CheckBox0804.Checked Then LogRep.WriteMessage((CheckBox0804.Text))
                If CheckBox0805.Checked Then LogRep.WriteMessage((CheckBox0805.Text))
                If CheckBox0806.Checked Then LogRep.WriteMessage((CheckBox0806.Text))
                LogRep.WriteMessage()
                LogRep.WriteMessage()
            End If
            '===========================================================================
            LogRep.ExH2(MultiPage1.TabPages.Item(9).Text)
            LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(9).Text + " ]")
            LogRep.Param(_Label0901_11.Text, TextBox0907.Text)
            LogRep.WriteMessage()
            LogRep.WriteMessage()

            LogRep.Param(_Label0901_4.Text, TextBox0903.Text, _Label0901_5.Text)
            LogRep.Param(ComboBox0903.Text, TextBox0906.Text, _Label0901_10.Text)
            LogRep.WriteMessage()
            LogRep.Param(_Label0901_12.Text, TextBox0908.Text, _Label0901_13.Text)
        End If
        '===========================================================================
        LogRep.CloseFile()

    End Sub

    'Private Sub TextBox0504_Validate(Cancel As Boolean)
    'Dim TNH As Double
    '
    'If TextBox0504.Text = TextBox0504.Tag Then Return
    '
    'If IsNumeric(TextBox0504.Text) Then
    '    TNH = CDbl(TextBox0504.Text)
    '    fTNH = CalcTNH(SOCObstacleMOCList, PtCoordCntr, ArDir, Turndir, TNH, fMisAprOCH)
    '    TextBox0504.Text = CStr(Round(fTNH + 0.4999))
    '    TextBox0504.Tag = TextBox0504.Text
    'End If
    '
    'End Sub

    Private Sub ReDrawTrace()
		Functions.SafeDeleteElement(pTraceElem)
		Functions.SafeDeleteElement(pProtectElem)

		pTraceElem = Nothing
        pProtectElem = Nothing

        Dim pAllTracks As IPointCollection = New ESRI.ArcGIS.Geometry.Polyline()
        Dim pAllProtections As IPolygon = New ESRI.ArcGIS.Geometry.Polygon()

        For i As Integer = 0 To TSC - 1

            Dim pTopo As ITopologicalOperator2 = pAllProtections
            Dim pTmpPoly As IPolygon = pTopo.Union(Trace(i).pProtectArea)

            pTopo = pTmpPoly
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()
            pAllProtections = pTmpPoly

            pAllTracks.AddPointCollection(Trace(i).PathPrj)
        Next
        pTraceElem = Functions.DrawPolyLine(pAllTracks, RGB(0, 0, 255), 1)
        pTraceElem.Locked = True

        pProtectElem = Functions.DrawPolygon(pAllProtections, RGB(0, 255, 0))
        pProtectElem.Locked = True
    End Sub

    Private Sub ReListTrace()
        ListView501.Items.Clear()

        For i As Integer = 0 To TSC - 1
            Dim itmX As ListViewItem = ListView501.Items.Add((i + 1).ToString())
            itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Trace(i).Comment))
            'Select Case Trace(i).SegmentCode
            '	Case eSegmentType.straight			'	"Прямой сегмент"
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Resources.str00154))
            '	Case eSegmentType.toHeading			'	" На заданный курс"
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Resources.str00155))
            '	Case eSegmentType.courseIntercept	'	"На заданную WPT"
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Resources.str00156))
            '	Case eSegmentType.directToFIX		'	"???????? ?????"
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Resources.str00157))
            '	Case eSegmentType.turnAndIntercept	'	"На курс заданной РНС"
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, Resources.str00158))
            '	Case eSegmentType.arcIntercept
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, "Arc intercept"))
            '	Case eSegmentType.arcPath
            '		itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, "Arc path"))	'	Resources.str160
            'End Select

            'If i = 0 Then
            '	itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HStart, eRoundMode.rmNERAEST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HStart - fRefHeight, eRoundMode.rmNERAEST).ToString()))
            'Else
            '	itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HStart, eRoundMode.rmNERAEST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HStart - fRefHeight, eRoundMode.rmNERAEST).ToString()))
            'End If
            itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HStart, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HStart - FicTHRprj.Z, eRoundMode.NEAREST).ToString()))

            itmX.SubItems(2).Text = itmX.SubItems(2).Text + " " + GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit

            itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HFinish, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HFinish - FicTHRprj.Z, eRoundMode.NEAREST).ToString()))
            itmX.SubItems(3).Text = itmX.SubItems(3).Text + " " + GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit

            itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertDistance(Trace(i).Length, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit))
            itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertDistance(Trace(i).Turn1R, eRoundMode.NEAREST).ToString() + " " + GlobalVars.DistanceConverter(GlobalVars.DistanceUnit).Unit))
            itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, (100.0 * Trace(i).PDG).ToString("0.00")))

            'itmX.SubItems.Insert(7, new ListViewItem.ListViewSubItem(Nothing, NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtEnd], Trace[i].DirIn) - GlobalVars.CurrADHP.MagVar).ToString("0.00")))
            'itmX.SubItems.Insert(8, new ListViewItem.ListViewSubItem(Nothing, NativeMethods.Modulus(Functions.Dir2Azt(DER.pPtPrj[eRWY.PtEnd], Trace[i].DirOut) - GlobalVars.CurrADHP.MagVar).ToString("0.00")))

            itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, NativeMethods.Modulus(Functions.Dir2Azt(Trace(i).ptIn, Trace(i).DirIn) - GlobalVars.CurrADHP.MagVar).ToString("0.00")))
            itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, NativeMethods.Modulus(Functions.Dir2Azt(Trace(i).ptOut, Trace(i).DirOut) - GlobalVars.CurrADHP.MagVar).ToString("0.00")))
            itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, Trace(i).StCoords))
            itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, Trace(i).FinCoords))
            itmX.SubItems.Insert(11, New ListViewItem.ListViewSubItem(Nothing, Trace(i).Comment))
        Next
    End Sub

    Private Sub ReSelectTrace()
		Functions.SafeDeleteElement(pTraceSelectElem)
		Functions.SafeDeleteElement(pProtectSelectElem)
		pTraceSelectElem = Nothing
        pProtectSelectElem = Nothing

        Dim i As Integer
        Dim j As Integer = -1

        For i = 0 To ListView501.Items.Count - 1
            If ListView501.Items(i).Selected Then
                j = i
                Exit For
            End If
        Next

        If j > -1 Then
            pTraceSelectElem = Functions.DrawPolyLine(Trace(j).PathPrj, 255)    '		Functions.RGB(0, 255, 255)
            pTraceSelectElem.Locked = True

            pProtectSelectElem = Functions.DrawPolygon(Trace(j).pProtectArea, 255)  '	Functions.RGB(0, 255, 255))
            pProtectSelectElem.Locked = True
        End If
    End Sub

    Private Sub AddSegmentBtn_Click(sender As Object, e As EventArgs) Handles AddSegmentBtn.Click
        For i As Integer = 0 To ListView501.Items.Count - 1
            ListView501.Items(i).Selected = False
        Next
        ReSelectTrace()

        If TSC = MaxTraceSegments Then
            MessageBox.Show(Resources.str00151, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return
        End If

        Me.Hide()

        AddSegmentFrm.CreateNextSegment(GlobalVars.CurrADHP, FicTHRprj.Z, m_fIAS, segPDG, Trace(TSC - 1), Me)
    End Sub

    Private Sub RemoveSegmentBtn_Click(sender As Object, e As EventArgs) Handles RemoveSegmentBtn.Click
        screenCapture.Delete()
        For i As Integer = 0 To ListView501.Items.Count - 1
            ListView501.Items(i).Selected = False
        Next

        ReSelectTrace()

        If (TSC > 1) Then
            TSC -= 1

            ReDrawTrace()
            ReListTrace()
        End If

        RemoveSegmentBtn.Enabled = TSC > 1
    End Sub

    Private Sub SaveGeometryBtn_Click(sender As Object, e As EventArgs) Handles SaveGeometryBtn.Click
        Dim RepFileName, RepFileTitle As String
        Dim pReport As ReportHeader = New ReportHeader()

        If Functions.ShowSaveDialog(RepFileName, RepFileTitle) Then
            'pReport.Procedure = Resources.str11
            pReport.Procedure = RepFileTitle

            'pReport.RWY = ComboBox001.Text
            pReport.Category = ComboBox0001.Text
            SaveGeometry(RepFileName, RepFileTitle, pReport)
        End If
    End Sub

    Sub DialogHook(result As Integer, ByRef NSegment As TraceSegment, Optional NewPDG As Double = 0) Implements IProcedureForm.DialogHook
        If result = 1 Then
            RemoveSegmentBtn.Enabled = True
            Trace(TSC) = NSegment
            TSC += 1

            segPDG = NewPDG
            ReDrawTrace()
            ReListTrace()
        End If

        Me.Show(GlobalVars._Win32Window)
    End Sub

End Class