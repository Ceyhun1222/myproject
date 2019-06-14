Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports System.ComponentModel
Imports Aran.Aim
Imports Aran.Aim.Data
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Enums
Imports Aran.Aim.Features
Imports Aran.Aim.Objects
Imports Aran.AranEnvironment
Imports Aran.Geometries
Imports Aran.Queries
Imports ESRI.ArcGIS.Geometry
Imports Aran.Metadata.Utils

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CArrivalFrm
	Inherits System.Windows.Forms.Form
	Implements IProcedureForm

#Region "variables"

	Const MaxTraceSegments As Integer = 100
	Const MaxDistStraightInApproach8000 As Double = 8000.0
	' ID for the About item on the system menu

	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
	Public IntermediateBaseAreaElem As ESRI.ArcGIS.Carto.IElement
	Public IntermediateSecAreaElem As ESRI.ArcGIS.Carto.IElement
	Public TerminationFIXElem As ESRI.ArcGIS.Carto.IElement
	Public PrimeMAPolyElement As ESRI.ArcGIS.Carto.IElement
	Public VisualAreaElement As ESRI.ArcGIS.Carto.IElement
	Public FinalBaseAreaElem As ESRI.ArcGIS.Carto.IElement
	Public FinalSecAreaElem As ESRI.ArcGIS.Carto.IElement
	Public RightPolyElement As ESRI.ArcGIS.Carto.IElement
	Public PrimePolyElement As ESRI.ArcGIS.Carto.IElement
	Public LeftPolyElement As ESRI.ArcGIS.Carto.IElement
	Public FullPolyElement As ESRI.ArcGIS.Carto.IElement
	Public NavAreaElement As ESRI.ArcGIS.Carto.IElement
	'Public FictThrElement As ESRI.ArcGIS.Carto.IElement
	Public FASegmElement As ESRI.ArcGIS.Carto.IElement
	Public IIAreaElement As ESRI.ArcGIS.Carto.IElement
	Public IFPolyElement As ESRI.ArcGIS.Carto.IElement
	Public IAreaElement As ESRI.ArcGIS.Carto.IElement
	Public MAPtCLineElem As ESRI.ArcGIS.Carto.IElement
	Public MAPtAreaElem As ESRI.ArcGIS.Carto.IElement
	Public MAPtFixElem As ESRI.ArcGIS.Carto.IElement
	Public TracElement As ESRI.ArcGIS.Carto.IElement
	Public IFFIXElem As ESRI.ArcGIS.Carto.IElement
	Public CLElement As ESRI.ArcGIS.Carto.IElement

	Public pShablonElem As ESRI.ArcGIS.Carto.IElement
	Public MAPtElem As ESRI.ArcGIS.Carto.IElement

	Public Sec0Elem As ESRI.ArcGIS.Carto.IElement
	Public SOCElem As ESRI.ArcGIS.Carto.IElement

	Public PtSDFElem As ESRI.ArcGIS.Carto.IElement
	Public SDFElem As ESRI.ArcGIS.Carto.IElement

	Public FAFAreaElem As ESRI.ArcGIS.Carto.IElement
	Public FAFFIXElem As ESRI.ArcGIS.Carto.IElement

	Public FarFAFElem As ESRI.ArcGIS.Carto.IElement
	Public pProtectElem As ArcGIS.Carto.IElement
	Public pTraceElem As ArcGIS.Carto.IElement

	Private pProtectSelectElem As ArcGIS.Carto.IElement
	Private pTraceSelectElem As ArcGIS.Carto.IElement

	Private BufferedStandart45_180Area As ESRI.ArcGIS.Geometry.IPointCollection
	Private BufferedStandart80_260Area As ESRI.ArcGIS.Geometry.IPointCollection
	Private IntermediateSecondArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private IntermediateBaseArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private BufferedBaseTurnArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private BufferedHoldingArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private Standart80_260Area As ESRI.ArcGIS.Geometry.IPointCollection
	Private Standart45_180Area As ESRI.ArcGIS.Geometry.IPointCollection
	Private FinalSecondArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private FinalBaseArea As ESRI.ArcGIS.Geometry.IPointCollection

	Private SDFFinalSecondArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private SDFFinalBaseArea As ESRI.ArcGIS.Geometry.IPointCollection

	Private MPtCollection As ESRI.ArcGIS.Geometry.IPointCollection
	Private pMAPtFIXPoly As ESRI.ArcGIS.Geometry.IPointCollection
	Private NavGuadArea2 As ESRI.ArcGIS.Geometry.IPointCollection
	Private BaseTurnArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private NavGuadArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private HoldingArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private ConvexPoly As ESRI.ArcGIS.Geometry.IPointCollection
	Private VsArea As ESRI.ArcGIS.Geometry.IPolygon

	Private trHoldingArea As ESRI.ArcGIS.Geometry.IPolyline
	Private trTurn45_180 As ESRI.ArcGIS.Geometry.IPolyline
	Private trTurn80_260 As ESRI.ArcGIS.Geometry.IPolyline
	Private trBaseTurn As ESRI.ArcGIS.Geometry.IPolyline
	Private mPoly As ESRI.ArcGIS.Geometry.IPolyline

	Private pFIXAreaPolygon As ESRI.ArcGIS.Geometry.IPointCollection
	Private pPolyPrime As ESRI.ArcGIS.Geometry.IPointCollection
	Private pPolyRight As ESRI.ArcGIS.Geometry.IPointCollection
	Private pPolyLeft As ESRI.ArcGIS.Geometry.IPointCollection

	Private m_BasePoints As ESRI.ArcGIS.Geometry.IPointCollection
	Private pFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
	Private pFullPolyCopy As ESRI.ArcGIS.Geometry.IPolygon

	Private pMAStraightPrimPoly As ESRI.ArcGIS.Geometry.IPolygon
	Private pMAStraightSecLPoly As ESRI.ArcGIS.Geometry.IPolygon
	Private pMAStraightSecRPoly As ESRI.ArcGIS.Geometry.IPolygon
	Private pMAStraightSecPoly As ESRI.ArcGIS.Geometry.IPolygon

	Private pIFTolerArea As ESRI.ArcGIS.Geometry.IPolygon
	Private pFAFTolerArea As ESRI.ArcGIS.Geometry.IPolygon
	Private pSDFTolerArea As ESRI.ArcGIS.Geometry.IPolygon
	Private pTPTolerArea As ESRI.ArcGIS.Geometry.IPolygon
	Private pTermFIXTolerArea As ESRI.ArcGIS.Geometry.IPolygon
	Private ReckoningFIXToleranceArea As ESRI.ArcGIS.Geometry.IPolygon

	Private PrimaryArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private BufferedArea As ESRI.ArcGIS.Geometry.IPointCollection

	Private RightLine As ESRI.ArcGIS.Geometry.IPointCollection
	Private pMissAproachArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private pMAPtArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private SecPoly0 As ESRI.ArcGIS.Geometry.IPointCollection
	Private LeftLine As ESRI.ArcGIS.Geometry.IPointCollection
	Private m_pFixPoly As ESRI.ArcGIS.Geometry.IPointCollection
	Private ZNR_Poly As ESRI.ArcGIS.Geometry.IPointCollection
	Private m_TurnArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private BaseArea As ESRI.ArcGIS.Geometry.IPointCollection
	Private SecPoly As ESRI.ArcGIS.Geometry.IPointCollection
	Private pCircle As ESRI.ArcGIS.Geometry.IPointCollection
	Private SecL As ESRI.ArcGIS.Geometry.IPointCollection
	Private SecR As ESRI.ArcGIS.Geometry.IPointCollection
	Private Prim As ESRI.ArcGIS.Geometry.IPointCollection
	Private KKFixMax As ESRI.ArcGIS.Geometry.IPolyline
	Private pFAFLine As ESRI.ArcGIS.Geometry.IPolyline
	Private pIFPoly As ESRI.ArcGIS.Geometry.IPolygon
	Private KKhMin As ESRI.ArcGIS.Geometry.IPolyline
	Private K1K1 As ESRI.ArcGIS.Geometry.IPolyline
	Private KK As ESRI.ArcGIS.Geometry.IPolyline
	Private pStraightNomLine As IPolyline
	Private pTermPt As ESRI.ArcGIS.Geometry.IPoint

	Private _bTurnFIXSameMAPtFIX As Boolean
	Private _CheckState As Boolean
	Private _IsClosing As Boolean
	Private _bHaveMSA As Boolean
	Private _OnAero As Boolean

	Private IxFinalOCH As Integer
	Private JxFinalOCH As Integer
	Private SchemeType As Integer
	Private IxWorkJO As Integer
	Private JxWorkJO As Integer
	Private IxMinOCH As Integer
	Private JxMinOCH As Integer
	Private IxMaxOCH As Integer
	Private JxMaxOCH As Integer
	Private Category As Integer
	Private CurrPage As Integer
	Private TurnDir As Integer
	Private NavIx As Integer
	Private NavJx As Integer
	Private MinDR As Integer

	Private fStraightMissedTermHght As Double
	Private fFAF2NEARMAPt As Double
	Private NReqCorrAngle As Double
	Private FReqCorrAngle As Double
	Private IntermAreaPDG As Double
	Private FinalAreaPDG As Double
	Private fFAF2FARMAPt As Double
	Private fMissAprPDG As Double
	Private fIF2FAFMin As Double
	Private fIF2FAFMax As Double
	Private arMinISlen As Double
	Private _DirToNav As Double
	Private CurrhFAF As Double
	Private CPDGmin As Double
	Private segPDG As Double

	Private prevPDGmin As Double
	Private prevPDGmax As Double
	Private nextPDGmin As Double
	Private nextPDGmax As Double
	Private fNearDist As Double
	Private MinHeight As Double
	Private MaxHeight As Double
	Private dNearMAPt As Double
	Private fFarDist As Double
	Private dFarMAPt As Double
	Private lFAFmin As Double
	Private dMinSDF As Double
	Private dMaxSDF As Double
	Private _CurrPDG As Double
	Private dMaxFAF As Double
	Private OptRad As Double
	Private SDFOCH As Double
	Private fReachedA As Double
	Private _TerminationAltitude As Double
	Private fdLimit As Double

	Private fhTHR As Double
	Private fDesV As Double
	Private ArAzt As Double
	Private _ArDir As Double
	Private hTMA As Double
	Private _initialIAS As Double
	Private _initialTAS As Double
	Private _missedIAS As Double

	Private _InterMOCH As Double

	Private _visualIAS As Double
	Private _visualTAS As Double
	Private _finalIAS As Double

	Private Ss As Double
	Private Vs As Double

	Private TurnAreaMaxd0 As Double
	Private fMAInterRange As Double
	Private fRefHeight As Double
	Private fBankAngle As Double
	Private NCorrAngle As Double
	Private fCorrAngle As Double
	Private fVisAprOCH As Double
	Private fNewVisAprOCH As Double

	Private resultOCH As Double
	Private dMAPt2SOC As Double
	Private dMAPt2FAF As Double
	Private fAlignOCH As Double
	Private _FinalMOC As Double
	Private hMinTurn As Double
	Private hMaxTurn As Double
	Private fFinalOCH As Double
	Private fFinalOCA As Double
	Private _FinalOCH As Double
	Private CurrOCH As Double

	Private SDFOCH13 As Double
	Private MAPtOCH As Double

	Private _OutDir As Double
	Private fMinOCH As Double
	Private OverOCH As Double
	Private MinOCH As Double
	Private AvdOCH As Double

	Private RWYCollection As ESRI.ArcGIS.Geometry.IPointCollection
	Private TurnFixPnt As ESRI.ArcGIS.Geometry.IPoint
	Private RWYTHRPrj As ESRI.ArcGIS.Geometry.IPoint
	Private _pCentroid As ESRI.ArcGIS.Geometry.IPoint
	Private TerFixPnt As ESRI.ArcGIS.Geometry.IPoint
	Private NJoinPt As ESRI.ArcGIS.Geometry.IPoint
	Private FJoinPt As ESRI.ArcGIS.Geometry.IPoint
	Private m_OutPt As ESRI.ArcGIS.Geometry.IPoint

	Private FictTHR As ESRI.ArcGIS.Geometry.IPoint
	Private FarFAF As ESRI.ArcGIS.Geometry.IPoint
	Private FOutPt As ESRI.ArcGIS.Geometry.IPoint
	Private NOutPt As ESRI.ArcGIS.Geometry.IPoint

	Private PtFAF As ESRI.ArcGIS.Geometry.IPoint
	Private PtSDF As ESRI.ArcGIS.Geometry.IPoint

	Private pMAPt As ESRI.ArcGIS.Geometry.IPoint
	Private PtSOC As ESRI.ArcGIS.Geometry.IPoint

	Private PtTOB As ESRI.ArcGIS.Geometry.IPoint
	Private PtSOL As ESRI.ArcGIS.Geometry.IPoint
	Private PtEOL As ESRI.ArcGIS.Geometry.IPoint

	Private FlyBy As ESRI.ArcGIS.Geometry.IPoint
	Private IFPnt As ESRI.ArcGIS.Geometry.IPoint

	Private AddSegmentFrm As AddSegmentForm

	Private pProcedure As InstrumentApproachProcedure
	Private SelectedRWY As RWYType
	Private RecommendStr As String
	Private FinalNav As NavaidData
	Private WPt1202 As WPT_FIXType
	Private TurnDirector As WPT_FIXType

	Private TSC As Integer
	Private Trace() As TraceSegment

	Private SelectedFixAngl() As WPT_FIXType
	Private SelectedFixAll() As WPT_FIXType
	Private FixBox1202() As WPT_FIXType
	Private FixBox1501() As WPT_FIXType
	Private FixAngl() As WPT_FIXType
	Private RWYDATA() As RWYType
	Private RWYIndex() As Integer

	Private Solutions() As LowHigh
	Private MSAObstacleList() As ObstacleMSA
	Private MSAList() As MSAType
	Private FAFRange As LowHigh

	Private SDFDominikObstacle As Integer
	Private ObstacleFinalMOCList As ObstacleContainer
	Private FAFWorkObstacleList As ObstacleContainer
	Private BuffObstacleList As ObstacleContainer
	Private NavObstacleList As ObstacleContainer
	Private DetOCHObstacles As ObstacleContainer
	Private FAFObstacleList As ObstacleContainer
	Private MAPtObstacles As ObstacleContainer
	Private ObstacleList As ObstacleContainer
	Private ImObstacles As ObstacleContainer

	Private TurnInterNavDat() As NavaidData
	Private TerInterNavDat() As NavaidData
	Private MAPtNavDat() As NavaidData
	Private FAFNavDat() As NavaidData
	Private SDFNavDat() As NavaidData
	Private IFNavDat() As NavaidData
	Private InSectList() As NavaidData
	Private ArNavList() As NavaidData

	Private MinISlensArray(5) As Double
	Private InSectListIx() As Integer

	Private LimitTextBox0701 As LowHigh
	Private LimitTextBox0702 As LowHigh
	Private LimitTextBox0605 As LowHigh

	Private OkBtnEnabled As Boolean
	Private HelpContextID As Integer = 8100

	Private NonPrecReportFrm As CNonPrecReportFrm
	Private ArrivalProfile As CArrivalProfile
	Private bFormInitialised As Boolean = False
	Private PageLabel() As Label

	Private AccurRep As ReportFile
	Private ProtRep As ReportFile
	Private LogRep As ReportFile
	Private GeomRep As ReportFile

	Private mUomHDistance As UomDistance
	Private mUomVDistance As UomDistanceVertical
	Private mUomSpeed As UomSpeed

	Private screenCapture As IScreenCapture

	Private pSubtrahend As List(Of ESRI.ArcGIS.Geometry.Polygon)
	Private pSubtrahendElem As List(Of ESRI.ArcGIS.Carto.IElement)

#End Region

#Region "Form"
	Public Sub New()
		'MyBase.New()
		' This call is required by the Windows Form Designer.
		InitializeComponent()

		_IsClosing = False
		'bUnloadByOk = False
		ReDim Trace(MaxTraceSegments)
		screenCapture = GlobalVars.gAranEnv.GetScreenCapture(FeatureType.InstrumentApproachProcedure.ToString())

		Try
			Dim i As Integer
			Dim n As Integer

			bFormInitialised = True

			AddSegmentFrm = New AddSegmentForm(screenCapture)
			ArrivalProfile = New CArrivalProfile
			NonPrecReportFrm = New CNonPrecReportFrm
			NonPrecReportFrm.SetBtn(ReportButton)

			SchemeType = -1
			CurrPage = 0
			OkBtnEnabled = False
			'hTMA = 1850#
			'Set CLElement = New LineElement

			pGraphics = GetActiveView().GraphicsContainer

			TextBox0402.Text = CStr(ConvertDistance(arIFHalfWidth.Value, eRoundMode.NEAREST))

			ReDim FixAngl(-1)
			ReDim FAFWorkObstacleList.Parts(-1)
			ReDim RWYDATA(-1)
			ReDim RWYIndex(-1)
			ReDim MSAObstacleList(-1)

			pSubtrahend = New List(Of ESRI.ArcGIS.Geometry.Polygon)
			pSubtrahendElem = New List(Of ArcGIS.Carto.IElement)

			'VisualAreaState = True
			'CLState = True

			MinISlensArray(0) = arMinISlen00_90.Value
			MinISlensArray(1) = arMinISlen91_96.Value
			MinISlensArray(2) = arMinISlen97_02.Value
			MinISlensArray(3) = arMinISlen03_08.Value
			MinISlensArray(4) = arMinISlen09_14.Value
			MinISlensArray(5) = arMinISlen15_20.Value

			'ComboBox0305.Tag = ""

			FinalAreaPDG = arFADescent_Nom.Value
			IntermAreaPDG = arImDescent_Max.Value
			fMissAprPDG = arMAS_Climb_Nom.Value
			'IAF_PDG = arIADescent_Max.Value 'IAF_PDG = arIADescent_Nom.Value

			TextBox0306.Text = CStr(100.0 * FinalAreaPDG)
			TextBox0408.Text = CStr(100.0 * IntermAreaPDG)

			'TextBox503.Text = CStr(100# * fMissAprPDG)

			ToolTip1.SetToolTip(TextBox0301, My.Resources.str00506 + CStr(ConvertDistance(arFAFOptimalDist.Value, eRoundMode.NEAREST)) + DistanceConverter(DistanceUnit).Unit)
			Label0001_13.Text = My.Resources.str10109 + vbCrLf + CStr(ConvertSpeed(arVisualWS.Value, eRoundMode.NEAREST)) + " " + SpeedConverter(SpeedUnit).Unit + ":"
			'_Label0001_13.Width = 900

			NCorrAngle = arafTrn_ISplay.Value
			fCorrAngle = arafTrn_OSplay.Value

			pPolyLeft = New ESRI.ArcGIS.Geometry.Polygon
			pPolyRight = New ESRI.ArcGIS.Geometry.Polygon
			pPolyPrime = New ESRI.ArcGIS.Geometry.Polygon

			LeftLine = New ESRI.ArcGIS.Geometry.Polyline
			RightLine = New ESRI.ArcGIS.Geometry.Polyline
			FIXElem = Nothing
			'=======================================================================================================================================
			'ComboBox0303.AddItem (My.Resources.str10416)
			'ComboBox0303.AddItem (My.Resources.str10417)
			ComboBox0303.Items.Add(My.Resources.str00223)
			ComboBox0303.Items.Add(My.Resources.str00224)

			ComboBox0304.Items.Add(My.Resources.str10113)
			ComboBox0304.Items.Add(My.Resources.str10118)

			ComboBox0604.Items.Clear()
			ComboBox0604.Items.Add(ConvertHeight(90.0, eRoundMode.NEAREST).ToString())
			ComboBox0604.Items.Add(ConvertHeight(150.0, eRoundMode.NEAREST).ToString())
			ComboBox0604.SelectedIndex = 0

			ComboBox0605.Items.Add(My.Resources.str10113)
			ComboBox0605.Items.Add(My.Resources.str10118)

			ComboBox0704.Items.Add(My.Resources.str10113)
			ComboBox0704.Items.Add(My.Resources.str10118)

			ComboBox0705.Items.Add(My.Resources.str10113)
			ComboBox0705.Items.Add(My.Resources.str10118)

			ComboBox0803.Items.Add(My.Resources.str10118)
			ComboBox0803.Items.Add(My.Resources.str10113)

			ComboBox0303.SelectedIndex = 0

			ComboBox0402.Items.Add(My.Resources.str00223)
			ComboBox0402.Items.Add(My.Resources.str00224)

			ComboBox0402.SelectedIndex = 0

			'====================================================================
			Category = 0
			'MAPtDef = 1
			fMAInterRange = 0.0

			RModel = MaxModelRadius

			'N = UBound(ADHPList)
			'ComboBox0002.Items.Clear()

			'For I = 0 To N
			'	ComboBox0002.Items.Add(ADHPList(I).Name)
			'Next I

			'If N >= 0 Then ComboBox0002.SelectedIndex = 0

			'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			ComboBox1201.Items.Clear()
			ComboBox1201.SelectedIndex = -1
			'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			MultiPage1.Tag = "1"
			MultiPage1.SelectedIndex = 0
			MultiPage1.Tag = "0"

			'SetFormParented(Handle.ToInt32)

			PrevBtn.Text = My.Resources.str00150
			NextBtn.Text = My.Resources.str00151
			OkBtn.Text = My.Resources.str00152
			CancelBtn.Text = My.Resources.str00153
			ReportButton.Text = My.Resources.str00154
			ProfileBtn.Text = My.Resources.str00155

			'NonPrecReportFrm.SetUnVisible -1
			'Page 1 ====================================================================
			Label0001_07.Text = SpeedConverter(SpeedUnit).Unit
			Label0001_10.Text = HeightConverter(HeightUnit).Unit
			Label0001_11.Text = HeightConverter(HeightUnit).Unit
			Label0001_12.Text = HeightConverter(HeightUnit).Unit
			Label0001_20.Text = SpeedConverter(SpeedUnit).Unit
			Label0001_21.Text = DistanceConverter(DistanceUnit).Unit
			Label0001_22.Text = DistanceConverter(DistanceUnit).Unit
			Label0001_23.Text = DistanceConverter(DistanceUnit).Unit
			Label0001_24.Text = HeightConverter(HeightUnit).Unit
			Label0001_25.Text = HeightConverter(HeightUnit).Unit

			_Label0101_10.Text = DistanceConverter(DistanceUnit).Unit
			'Page 2 ====================================================================
			_Label0201_13.Text = DistanceConverter(DistanceUnit).Unit
			_Label0201_15.Text = HeightConverter(HeightUnit).Unit
			_Label0201_16.Text = HeightConverter(HeightUnit).Unit
			_Label0201_17.Text = HeightConverter(HeightUnit).Unit
			'Page 3 ====================================================================
			_Label0301_11.Text = DistanceConverter(DistanceUnit).Unit
			_Label0301_12.Text = HeightConverter(HeightUnit).Unit
			_Label0301_13.Text = HeightConverter(HeightUnit).Unit
			_Label0301_14.Text = DistanceConverter(DistanceUnit).Unit
			_Label0301_15.Text = DistanceConverter(DistanceUnit).Unit
			_Label0301_17.Text = DistanceConverter(DistanceUnit).Unit
			'Page 4 ====================================================================
			label0401_06.Text = HeightConverter(HeightUnit).Unit
			label0401_13.Text = DistanceConverter(DistanceUnit).Unit
			label0401_14.Text = DistanceConverter(DistanceUnit).Unit
			label0401_15.Text = DistanceConverter(DistanceUnit).Unit
			label0401_17.Text = DistanceConverter(DistanceUnit).Unit
			label0401_18.Text = HeightConverter(HeightUnit).Unit
			label0401_19.Text = DistanceConverter(DistanceUnit).Unit
			label0401_21.Text = HeightConverter(HeightUnit).Unit
			'Page 5 ====================================================================
			_Label0501_0.Text = HeightConverter(HeightUnit).Unit

			'Page 6 ====================================================================
			_Label0601_8.Text = HeightConverter(HeightUnit).Unit
			_Label0601_10.Text = SpeedConverter(SpeedUnit).Unit

			_Label0601_11.Text = DSpeedConverter(HeightUnit).Unit
			_Label0601_15.Text = SpeedConverter(SpeedUnit).Unit
			_Label0601_19.Text = DSpeedConverter(HeightUnit).Unit
			_Label0601_23.Text = HeightConverter(HeightUnit).Unit
			_Label0601_24.Text = HeightConverter(HeightUnit).Unit
			_Label0601_25.Text = HeightConverter(HeightUnit).Unit
			_Label0601_27.Text = HeightConverter(HeightUnit).Unit
			_Label0601_29.Text = HeightConverter(HeightUnit).Unit
			Label0601_31.Text = DistanceConverter(DistanceUnit).Unit
			Label0601_33.Text = SpeedConverter(SpeedUnit).Unit
			'Page 7 ====================================================================
			_Label0701_13.Text = HeightConverter(HeightUnit).Unit
			_Label0701_14.Text = HeightConverter(HeightUnit).Unit
			_Label0701_17.Text = DistanceConverter(DistanceUnit).Unit
			_Label0701_18.Text = DistanceConverter(DistanceUnit).Unit
			_Label0701_25.Text = HeightConverter(HeightUnit).Unit
			_Label0701_26.Text = DistanceConverter(DistanceUnit).Unit
			_Label0701_28.Text = HeightConverter(HeightUnit).Unit
			'Page 8 ====================================================================
			_Label0801_8.Text = DistanceConverter(DistanceUnit).Unit
			_Label0801_15.Text = HeightConverter(HeightUnit).Unit
			_Label0801_17.Text = DistanceConverter(DistanceUnit).Unit
			_Label0801_18.Text = HeightConverter(HeightUnit).Unit
			_Label0801_20.Text = HeightConverter(HeightUnit).Unit
			label0801_22.Text = DistanceConverter(DistanceUnit).Unit
			'Page 9 ====================================================================
			_Label0901_6.Text = SpeedConverter(SpeedUnit).Unit
			_Label0901_8.Text = HeightConverter(HeightUnit).Unit
			'Page 10 ====================================================================
			_Label1001_11.Text = HeightConverter(HeightUnit).Unit
			_Label1001_12.Text = HeightConverter(HeightUnit).Unit
			_Label1001_13.Text = HeightConverter(HeightUnit).Unit
			_Label1001_14.Text = HeightConverter(HeightUnit).Unit
			_Label1001_15.Text = HeightConverter(HeightUnit).Unit
			_Label1001_16.Text = HeightConverter(HeightUnit).Unit
			_Label1001_17.Text = DistanceConverter(DistanceUnit).Unit
			_Label1001_18.Text = DistanceConverter(DistanceUnit).Unit
			'Page 11 ====================================================================
			_Label1101_4.Text = HeightConverter(HeightUnit).Unit
			_Label1101_13.Text = DistanceConverter(DistanceUnit).Unit
			_Label1101_14.Text = SpeedConverter(SpeedUnit).Unit
			_Label1101_15.Text = SpeedConverter(SpeedUnit).Unit
			_Label1101_16.Text = My.Resources.str00014   'DistanceConverter(DistanceUnit).Unit
			_Label1101_17.Text = DistanceConverter(DistanceUnit).Unit
			_Label1101_18.Text = HeightConverter(HeightUnit).Unit
			_Label1101_19.Text = DistanceConverter(DistanceUnit).Unit
			_Label1101_20.Text = DistanceConverter(DistanceUnit).Unit
			_Label1101_21.Text = DistanceConverter(DistanceUnit).Unit
			'Page 12 ====================================================================
			_Label1201_10.Text = DistanceConverter(DistanceUnit).Unit
			'Page 15 ====================================================================
			_Label1501_1.Text = HeightConverter(HeightUnit).Unit
			_Label1501_7.Text = HeightConverter(HeightUnit).Unit
			_Label1501_8.Text = HeightConverter(HeightUnit).Unit
			_Label1501_10.Text = HeightConverter(HeightUnit).Unit
			_Label1501_12.Text = DistanceConverter(DistanceUnit).Unit
			_Label1501_20.Text = HeightConverter(HeightUnit).Unit
			_Label1501_23.Text = DistanceConverter(DistanceUnit).Unit
			_Label1501_24.Text = HeightConverter(HeightUnit).Unit

			'====================================================================
			Me.Text = My.Resources.str00001

			MultiPage1.TabPages.Item(0).Text = My.Resources.str00101
			MultiPage1.TabPages.Item(1).Text = My.Resources.str00102
			MultiPage1.TabPages.Item(2).Text = My.Resources.str00103
			MultiPage1.TabPages.Item(3).Text = My.Resources.str00104
			MultiPage1.TabPages.Item(4).Text = My.Resources.str00105
			MultiPage1.TabPages.Item(5).Text = "IAF " + My.Resources.str00106
			MultiPage1.TabPages.Item(6).Text = My.Resources.str00107
			MultiPage1.TabPages.Item(7).Text = My.Resources.str00108
			MultiPage1.TabPages.Item(8).Text = My.Resources.str00109
			MultiPage1.TabPages.Item(9).Text = My.Resources.str00110
			MultiPage1.TabPages.Item(10).Text = My.Resources.str00111
			MultiPage1.TabPages.Item(11).Text = My.Resources.str00112
			MultiPage1.TabPages.Item(12).Text = My.Resources.str00113
			MultiPage1.TabPages.Item(13).Text = My.Resources.str00114
			MultiPage1.TabPages.Item(14).Text = My.Resources.str00115
			MultiPage1.TabPages.Item(15).Text = My.Resources.str00116
			MultiPage1.TabPages.Item(16).Text = My.Resources.str00117
			'==============================================
			Label0001_00.Text = My.Resources.str10101
			Label0001_01.Text = My.Resources.str11002
			Label0001_02.Text = My.Resources.str10103
			Label0001_03.Text = My.Resources.str10104
			Label0001_04.Text = My.Resources.str10105
			Label0001_05.Text = My.Resources.str10106
			Label0001_06.Text = My.Resources.str10114

			Label0001_09.Text = My.Resources.str00014
			Label0001_14.Text = My.Resources.str10110
			Label0001_15.Text = My.Resources.str10111
			Label0001_16.Text = My.Resources.str10112
			Label0001_17.Text = My.Resources.str10910
			Label0001_18.Text = My.Resources.str10118
			Label0001_19.Text = My.Resources.str10405
			'============================================
			Label0101_0.Text = My.Resources.str10201
			Label0101_1.Text = My.Resources.str10202
			_Label0101_2.Text = My.Resources.str10406
			_Label0101_6.Text = My.Resources.str10208
			Frame0101.Text = My.Resources.str10205
			OptionButton0101.Text = My.Resources.str10206
			OptionButton0102.Text = My.Resources.str10207
			Frame0102.Text = My.Resources.str10226
			_Label0101_11.Text = My.Resources.str20107
			_Label0101_12.Text = My.Resources.str20115
			_Label0101_13.Text = My.Resources.str00021
			_Label0101_14.Text = My.Resources.str00021
			'============================================
			_Label0201_0.Text = My.Resources.str10301
			_Label0201_1.Text = My.Resources.str10302
			_Label0201_2.Text = My.Resources.str10303
			'_Label0201_3.Text  = "Смещение трека на удалении " + CStr(arMinInterDist.Value) + " м до порога ВПП"
			'_Label0201_3.Text  = "Смещение трека на удалении " + CStr(ConvertDistance(arMinInterDist.Value, eRoundMode.rmNERAEST)) + " " + DistanceConverter(DistanceUnit).Unit + " до порога ВПП"
			'_Label0201_3.Text = My.Resources.str10304 + CStr(ConvertDistance(arMinInterDist.Value, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str10310
			_Label0201_4.Text = My.Resources.str10305
			ToolTip1.SetToolTip(_Label0201_4, "OCH визуального маневрирования не должна быть меньше, чем OCH захода на посадку")

			_Label0201_5.Text = My.Resources.str10306
			OptionButton0201.Text = My.Resources.str10307
			OptionButton0202.Text = My.Resources.str10308

			'CheckBox0201.Text = My.Resources.str10320
			'CheckBox0202.Text = My.Resources.str10321

			_Label0201_6.Text = My.Resources.str10309
			Frame0201.Text = My.Resources.str10319

			'============================================

			_Label0301_0.Text = My.Resources.str10401
			_Label0301_2.Text = My.Resources.str10408
			_Label0301_3.Text = My.Resources.str10410
			_Label0301_5.Text = My.Resources.str10405
			_Label0301_6.Text = My.Resources.str10503
			_Label0301_7.Text = My.Resources.str00513 + ":"
			_Label0301_8.Text = My.Resources.str10411
			_Label0301_16.Text = My.Resources.str10505

			CheckBox0301.Text = My.Resources.str10517   '    FAF designator:
			Frame0301.Text = My.Resources.str10404
			Frame0302.Text = My.Resources.str10409

			'============================================

			label0401_00.Text = My.Resources.str10501
			label0401_01.Text = My.Resources.str20305

			label0401_02.Text = My.Resources.str10510
			label0401_07.Text = My.Resources.str20316
			label0401_09.Text = My.Resources.str10511
			label0401_10.Text = My.Resources.str10503
			label0401_11.Text = My.Resources.str10505
			label0401_12.Text = My.Resources.str10516  '    IF designator:
			label0401_20.Text = My.Resources.str00613
			label0401_22.Text = My.Resources.str10405

			Frame0401.Text = My.Resources.str10509
			Frame0402.Text = My.Resources.str20306
			Frame0403.Text = My.Resources.str20304

			OptionButton0401.Text = My.Resources.str10512
			OptionButton0402.Text = My.Resources.str10513
			'CheckBox0401.Text = My.Resources.str10517
			'===================================================
			_Label0501_2.Text = My.Resources.str10614
			'ComboBox0501.Clear
			'=================================================
			_Label0601_0.Text = My.Resources.str10701
			_Label0601_3.Text = My.Resources.str11002
			_Label0601_4.Text = My.Resources.str10722
			_Label0601_6.Text = My.Resources.str10723
			_Label0601_7.Text = My.Resources.str10724
			_Label0601_9.Text = My.Resources.str00011
			_Label0601_14.Text = My.Resources.str11002
			_Label0601_16.Text = My.Resources.str10713
			_Label0601_17.Text = My.Resources.str10725
			_Label0601_18.Text = My.Resources.str10726
			_Label0601_20.Text = My.Resources.str10720
			_Label0601_21.Text = My.Resources.str10720
			_Label0601_22.Text = My.Resources.str10106
			_Label0601_26.Text = My.Resources.str00011

			Frame0605.Text = My.Resources.str10727

			Frame0602.Text = My.Resources.str10704
			OptionButton0601.Text = My.Resources.str10705
			OptionButton0602.Text = My.Resources.str10706
			OptionButton0603.Text = My.Resources.str10707
			OptionButton0604.Text = My.Resources.str10708
			CheckBox0601.Text = My.Resources.str10709
			CheckBox0602.Text = My.Resources.str10710
			Frame0603.Text = My.Resources.str10715
			Frame0604.Text = My.Resources.str10718
			Frame0606.Text = My.Resources.str10733

			ComboBox0601.Items.Add(My.Resources.str00225)
			ComboBox0601.Items.Add(My.Resources.str00226)
			ComboBox0601.SelectedIndex = 0

			ComboBox0602.Items.Add(My.Resources.str10728)
			ComboBox0602.Items.Add(My.Resources.str10729)
			ComboBox0602.Items.Add(My.Resources.str10730)
			ComboBox0602.Items.Add(My.Resources.str10731)
			ComboBox0602.SelectedIndex = 0

			ComboBox0603.SelectedIndex = 0

			ComboBox0501.Items.Add(My.Resources.str00223 + ":")
			ComboBox0501.Items.Add(My.Resources.str00224 + ":")
			ComboBox0501.SelectedIndex = 0

			'=====================================================
			Frame0702.Text = My.Resources.str10811
			'Frame0701.Text = My.Resources.str10802	'"Участок FAF-SDF"
			Frame0701.Text = My.Resources.str10803  '"Участок SIL-SDF"

			CheckBox0701.Text = My.Resources.str10801
			_Label0701_0.Text = My.Resources.str10806

			'_Label0701_22.Text = My.Resources.str10804	'"Расстояние FAF-SDF:"
			_Label0701_22.Text = My.Resources.str10805  '"Расстояние SIL-SDF:"
			_Label0701_2.Text = My.Resources.str10813


			_Label0701_9.Text = My.Resources.str10812
			_Label0701_10.Text = My.Resources.str10809
			_Label0701_11.Text = My.Resources.str10808

			_Label0701_21.Text = My.Resources.str10806
			_Label0701_24.Text = My.Resources.str10808

			ComboBox0701.Items.Add(My.Resources.str00223)
			ComboBox0701.Items.Add(My.Resources.str00224)
			ComboBox0701.SelectedIndex = 0

			ComboBox0703.Items.Add(My.Resources.str00223)
			ComboBox0703.Items.Add(My.Resources.str00224)
			ComboBox0703.SelectedIndex = 0
			'=====================================================
			Frame0801.Text = My.Resources.str10901
			OptionButton0801.Text = My.Resources.str10902

			OptionButton0802.Text = My.Resources.str10903
			OptionButton0802.Text = My.Resources.str10918

			_Label0801_0.Text = My.Resources.str10904
			_Label0801_9.Text = My.Resources.str10911
			_Label0801_10.Text = My.Resources.str10912

			_Label0801_11.Text = My.Resources.str10920

			_Label0801_12.Text = My.Resources.str10914
			_Label0801_13.Text = My.Resources.str10917
			_Label0801_14.Text = My.Resources.str10915
			'_Label0801_19.Text = My.Resources.str16005

			CheckBox0801.Text = My.Resources.str10916
			'=======================================================
			_Label0901_0.Text = My.Resources.str11001
			_Label0901_1.Text = My.Resources.str11002
			_Label0901_3.Text = My.Resources.str11003
			_Label0901_4.Text = My.Resources.str11004
			_Label0901_7.Text = My.Resources.str10920

			Frame0901.Text = My.Resources.str11005
			OptionButton0901.Text = My.Resources.str11006
			OptionButton0902.Text = My.Resources.str11007
			CheckBox0901.Text = My.Resources.str11008
			CheckBox0902.Text = My.Resources.str11009

			ComboBox0901.Items.Add(My.Resources.str00226)
			ComboBox0901.Items.Add(My.Resources.str00225)
			ComboBox0901.SelectedIndex = 0
			'=======================================================
			Label1001_0.Text = My.Resources.str11051
			Label1001_1.Text = My.Resources.str11052

			ComboBox1001.Items.Add(My.Resources.str11043)
			ComboBox1001.Items.Add(My.Resources.str11053)

			ComboBox1002.Items.Add(My.Resources.str11044)
			ComboBox1002.Items.Add(My.Resources.str11054)

			_Label1001_4.Text = My.Resources.str11055
			_Label1001_5.Text = My.Resources.str11056
			_Label1001_6.Text = My.Resources.str11057
			_Label1001_7.Text = My.Resources.str11057
			_Label1001_8.Text = My.Resources.str11058
			_Label1001_9.Text = My.Resources.str11059
			_Label1001_10.Text = My.Resources.str11060
			_Label1001_19.Text = My.Resources.str11066
			'=======================================================
			Frame1101.Text = My.Resources.str12001
			Frame1102.Text = My.Resources.str12007
			_Label1101_0.Text = My.Resources.str12002
			OptionButton1101.Text = My.Resources.str10512
			OptionButton1102.Text = My.Resources.str10513

			_Label1101_10.Text = My.Resources.str12012
			_Label1101_12.Text = My.Resources.str12006
			'==========================================================
			OptionButton1201.Text = My.Resources.str13001
			OptionButton1202.Text = My.Resources.str13002
			OptionButton1203.Text = My.Resources.str13003
			_Label1201_0.Text = My.Resources.str13004
			_Label1201_5.Text = My.Resources.str13005
			_Label1201_6.Text = My.Resources.str13006
			CheckBox1201.Text = My.Resources.str13007
			_Label1201_4.Text = My.Resources.str00015
			_Label1201_2.Text = My.Resources.str13010
			_Label1201_11.Text = My.Resources.str13012
			_Label1201_12.Text = My.Resources.str13016

			ComboBox1203.Items.Clear()
			ComboBox1203.Items.Add(My.Resources.str13014)
			ComboBox1203.Items.Add(My.Resources.str13015)

			'===================================================
			Frame1301.Text = My.Resources.str14001
			_Label1301_0.Text = My.Resources.str14002
			OptionButton1301.Text = My.Resources.str14003
			OptionButton1302.Text = My.Resources.str14004
			OptionButton1303.Text = My.Resources.str14005
			Frame1302.Text = My.Resources.str14006
			_Label1301_1.Text = My.Resources.str14007
			OptionButton1304.Text = My.Resources.str14003
			OptionButton1305.Text = My.Resources.str14004
			OptionButton1306.Text = My.Resources.str14005
			'==================================================
			Frame1401.Text = My.Resources.str15001
			CheckBox1401.Text = My.Resources.str15002
			CheckBox1402.Text = My.Resources.str15003
			CheckBox1403.Text = My.Resources.str15004
			Frame1402.Text = My.Resources.str15005
			CheckBox1404.Text = My.Resources.str15002
			CheckBox1405.Text = My.Resources.str15003
			CheckBox1406.Text = My.Resources.str15006
			'===================================================
			_Label1501_0.Text = My.Resources.str16001
			_Label1501_2.Text = My.Resources.str16005

			_Label1501_6.Text = My.Resources.str10720           '16003
			_Label1501_9.Text = My.Resources.str16004
			_Label1501_11.Text = My.Resources.str16006          '	'
			_Label1501_13.Text = My.Resources.str12002
			_Label1501_17.Text = My.Resources.str16009          '	'
			label1501_18.Text = My.Resources.str10720
			_Label1501_19.Text = My.Resources.str16007
			_Label1501_26.Text = My.Resources.str13016          'LoadResString(13013)+":"
			CheckBox1501.Text = My.Resources.str16011

			OptionButton1501.Text = My.Resources.str10512
			OptionButton1502.Text = My.Resources.str10513

			Frame1501.Text = My.Resources.str16003
			ComboBox1503.Items.Add(My.Resources.str10118)
			ComboBox1503.Items.Add(My.Resources.str10113)

			'================================================
			AddSegmentBtn.Text = My.Resources.str17021
			RemoveSegmentBtn.Text = My.Resources.str17022
			SaveGeometryBtn.Text = My.Resources.str17023
			'TextBox0616.Text = CStr(ConvertHeight(arFASegmentMOC.Value, eRoundMode.rmNERAEST))

			'CreateLog(My.Resources.str1)
			For i = 0 To 11
				ListView1501.Columns(i).Text = Resources.ResourceManager.GetString("str" + (60602 + i).ToString())
			Next

			ComboBox1101.SelectedIndex = 0
			' ====== 2007
			PageLabel = {Label1_00, Label1_01, Label1_02, Label1_03, Label1_04, Label1_05, Label1_06, Label1_07, Label1_08, Label1_09, Label1_10, Label1_11, Label1_12, Label1_13, Label1_14, Label1_15, Label1_16}

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
			PageLabel(11).Text = MultiPage1.TabPages.Item(11).Text
			PageLabel(12).Text = MultiPage1.TabPages.Item(12).Text
			PageLabel(13).Text = MultiPage1.TabPages.Item(13).Text
			PageLabel(14).Text = MultiPage1.TabPages.Item(14).Text
			PageLabel(15).Text = MultiPage1.TabPages.Item(15).Text
			PageLabel(16).Text = MultiPage1.TabPages.Item(16).Text

			FocusStepCaption(0)

			MultiPage1.Top = -21
			Me.Height = Me.Height - 21
			Frame1.Top = Frame1.Top - 21

			ShowPanelBtn.Checked = False

			fRefHeight = CurrADHP.pPtGeo.Z

			ListView0001.Items.Clear()

			FillRWYList(RWYDATA, CurrADHP)
			n = UBound(RWYDATA)
			If n < 0 Then
				'gAranEnv.ShowError(My.Resources.str300)
				MsgBox(My.Resources.str00300, MsgBoxStyle.Critical, "PANDA")
				_IsClosing = True
				Me.Close()
				Return
			End If

			ReDim RWYIndex(n)

			For i = 0 To n
				RWYDATA(i).Selected = True
				If (i And 1) = 0 Then
					ListView0001.Items.Add(RWYDATA(i).Name + " / " + RWYDATA(i).PairName)
					ListView0001.Items.Item(i \ 2).Checked = True
				End If
			Next i

			If GlobalVars._settings.AnnexObstalce Then
				GetAnnexObstacles(ObstacleList, CurrADHP.Identifier, fRefHeight)
			Else
				GetObstaclesByDist(ObstacleList, CurrADHP.pPtPrj, RModel, fRefHeight)
			End If

			If ComboBox0001.SelectedIndex >= 0 Then
				ComboBox0001_SelectedIndexChanged(ComboBox0001, New System.EventArgs())
			Else
				ComboBox0001.SelectedIndex = 0
			End If

			ComboBox0802.Items.Clear()
			ComboBox0902.Items.Clear()
			For i = 0 To UBound(EnrouteMOCValues)
				ComboBox0802.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(i), eRoundMode.SPECIAL)))
				ComboBox0902.Items.Add(CStr(ConvertHeight(EnrouteMOCValues(i), eRoundMode.SPECIAL)))
			Next i
			ComboBox0802.SelectedIndex = 0
			ComboBox0902.SelectedIndex = 0

		Catch aranEx As AranException
			aranEx.ShowMessageBox()
			Me.Dispose()
			Throw aranEx
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Private Sub CArrivalFrm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim I As Integer
		Dim J As Integer

		On Error Resume Next
		screenCapture.Rollback()
		DBModule.CloseDB()
		ClearGraphics()

		'If Not bUnloadByOk Then ClearScr()

		NonPrecReportFrm.Close()
		ArrivalProfile.Close()
		'CloseLog()
		CurrCmd = 0

		IntermediateBaseAreaElem = Nothing
		IntermediateSecAreaElem = Nothing
		FinalBaseAreaElem = Nothing
		FinalSecAreaElem = Nothing
		RightPolyElement = Nothing
		PrimePolyElement = Nothing
		LeftPolyElement = Nothing
		FullPolyElement = Nothing
		PrimeMAPolyElement = Nothing
		VisualAreaElement = Nothing
		MAPtCLineElem = Nothing
		NavAreaElement = Nothing
		'FictThrElement = Nothing
		FASegmElement = Nothing
		IIAreaElement = Nothing
		IFPolyElement = Nothing
		IAreaElement = Nothing
		MAPtAreaElem = Nothing
		NomTrackElem = Nothing
		TurnAreaElem = Nothing
		MAPtFixElem = Nothing
		TracElement = Nothing
		IFFIXElem = Nothing
		CLElement = Nothing
		K1K1Elem = Nothing
		MAPtElem = Nothing
		PrimElem = Nothing
		SecRElem = Nothing
		SecLElem = Nothing
		Sec0Elem = Nothing
		SOCElem = Nothing
		KKElem = Nothing
		SDFElem = Nothing
		PtSDFElem = Nothing
		FAFFIXElem = Nothing
		FAFAreaElem = Nothing
		FarFAFElem = Nothing

		mPoly = Nothing
		BufferedStandart45_180Area = Nothing
		BufferedStandart80_260Area = Nothing
		IntermediateSecondArea = Nothing
		IntermediateBaseArea = Nothing
		BufferedBaseTurnArea = Nothing
		BufferedHoldingArea = Nothing
		Standart80_260Area = Nothing
		Standart45_180Area = Nothing
		FinalSecondArea = Nothing
		FinalBaseArea = Nothing
		MPtCollection = Nothing
		pMAPtFIXPoly = Nothing
		NavGuadArea2 = Nothing
		BaseTurnArea = Nothing
		NavGuadArea = Nothing
		HoldingArea = Nothing
		ConvexPoly = Nothing
		pFIXAreaPolygon = Nothing
		pPolyPrime = Nothing
		pPolyRight = Nothing
		pPolyLeft = Nothing
		m_BasePoints = Nothing
		pFullPoly = Nothing
		pFullPolyCopy = Nothing
		PrimaryArea = Nothing
		RightLine = Nothing
		pSDFTolerArea = Nothing

		pMAPtArea = Nothing
		pMissAproachArea = Nothing
		SecPoly0 = Nothing
		LeftLine = Nothing
		m_pFixPoly = Nothing
		ZNR_Poly = Nothing
		m_TurnArea = Nothing
		BaseArea = Nothing
		SecPoly = Nothing
		pCircle = Nothing
		SecL = Nothing
		SecR = Nothing
		Prim = Nothing
		pIFPoly = Nothing
		pFAFLine = Nothing
		KK = Nothing
		K1K1 = Nothing
		KKhMin = Nothing
		KKFixMax = Nothing
		RWYTHRPrj = Nothing
		pMAPt = Nothing
		PtFAF = Nothing
		PtSDF = Nothing
		PtSOC = Nothing
		TurnFixPnt = Nothing
		NJoinPt = Nothing
		FJoinPt = Nothing
		FictTHR = Nothing
		FarFAF = Nothing
		FOutPt = Nothing
		NOutPt = Nothing
		m_OutPt = Nothing
		FlyBy = Nothing
		IFPnt = Nothing
		PtTOB = Nothing
		PtSOL = Nothing
		PtEOL = Nothing

		FinalNav.pPtGeo = Nothing
		FinalNav.pPtPrj = Nothing
		TurnDirector.pPtGeo = Nothing
		TurnDirector.pPtPrj = Nothing

		If Not FixAngl Is Nothing Then
			For I = 0 To UBound(FixAngl)
				FixAngl(I).pPtGeo = Nothing
				FixAngl(I).pPtPrj = Nothing
			Next I
			Erase FixAngl
		End If

		If Not WPTList Is Nothing Then
			For I = 0 To UBound(WPTList)
				WPTList(I).pPtGeo = Nothing
				WPTList(I).pPtPrj = Nothing
			Next I
			Erase WPTList
		End If

		If Not RWYDATA Is Nothing Then
			For I = 0 To UBound(RWYDATA)
				For J = eRWY.PtStart To eRWY.PtEnd
					RWYDATA(I).pPtGeo(J) = Nothing
					RWYDATA(I).pPtPrj(J) = Nothing
				Next J
			Next I
			Erase RWYDATA
		End If

		Erase RWYIndex

		If Not MSAObstacleList Is Nothing Then
			For I = 0 To UBound(MSAObstacleList)
				MSAObstacleList(I).pGeomGeo = Nothing
				MSAObstacleList(I).pGeomPrj = Nothing
			Next I
			Erase MSAObstacleList
		End If

		'For I = 0 To 2
		'    Set HoldSectors(I).DominicObstacle = Nothing
		'    Set HoldSectors(I).Sector = Nothing
		'Next I

		'For I = 0 To UBound(Sectors)
		'    Set Sectors(I).DominicObstacle = Nothing
		'    Set Sectors(I).Sector = Nothing
		'Next I
		'Erase Sectors

		''For I = 0 To UBound(ObstacleFinalMOCList)
		''    Set ObstacleFinalMOCList(I).pPtGeo = Nothing
		''    Set ObstacleFinalMOCList(I).pPtPrj = Nothing
		''Next I
		''Erase ObstacleFinalMOCList
		''
		''For I = 0 To UBound(FAFWorkObstacleList)
		''    Set FAFWorkObstacleList(I).pPtGeo = Nothing
		''    Set FAFWorkObstacleList(I).pPtPrj = Nothing
		''Next I
		''Erase FAFWorkObstacleList
		''
		''For I = 0 To UBound(FinDistObstacles)
		''    Set FinDistObstacles(I).pPtGeo = Nothing
		''    Set FinDistObstacles(I).pPtPrj = Nothing
		''Next I
		''Erase FinDistObstacles
		''
		''For I = 0 To UBound(BuffObstacleList)
		''    Set BuffObstacleList(I).pPtGeo = Nothing
		''    Set BuffObstacleList(I).pPtPrj = Nothing
		''Next I
		''Erase BuffObstacleList
		''
		''For I = 0 To UBound(NavObstacleList)
		''    Set NavObstacleList(I).pPtGeo = Nothing
		''    Set NavObstacleList(I).pPtPrj = Nothing
		''Next I
		''Erase NavObstacleList
		''
		''For I = 0 To UBound(DetOCHObstacles)
		''    Set DetOCHObstacles(I).pPtGeo = Nothing
		''    Set DetOCHObstacles(I).pPtPrj = Nothing
		''Next I
		''Erase DetOCHObstacles
		''
		''For I = 0 To UBound(FAFObstacleList)
		''    Set FAFObstacleList(I).pPtGeo = Nothing
		''    Set FAFObstacleList(I).pPtPrj = Nothing
		''Next I
		''Erase FAFObstacleList
		''
		''For I = 0 To UBound(MAPtObstacles)
		''    Set MAPtObstacles(I).pPtGeo = Nothing
		''    Set MAPtObstacles(I).pPtPrj = Nothing
		''Next I
		''Erase MAPtObstacles
		''
		''For I = 0 To UBound(ObstacleList)
		''    Set ObstacleList(I).pPtGeo = Nothing
		''    Set ObstacleList(I).pPtPrj = Nothing
		''Next I
		''Erase ObstacleList
		''
		''For I = 0 To UBound(ImObstacles)
		''    Set ImObstacles(I).pPtGeo = Nothing
		''    Set ImObstacles(I).pPtPrj = Nothing
		''Next I
		''Erase ImObstacles
		''
		''For I = 0 To UBound(MAPtNavDat)
		''    Set MAPtNavDat(I).pPtGeo = Nothing
		''    Set MAPtNavDat(I).pPtPrj = Nothing
		''    Erase MAPtNavDat(I).ValMax
		''    Erase MAPtNavDat(I).ValMin
		''Next I
		''Erase MAPtNavDat
		''
		''For I = 0 To UBound(FAFNavDat)
		''    Set FAFNavDat(I).pPtGeo = Nothing
		''    Set FAFNavDat(I).pPtPrj = Nothing
		''    Erase FAFNavDat(I).ValMax
		''    Erase FAFNavDat(I).ValMin
		''Next I
		''Erase FAFNavDat
		''
		''For I = 0 To UBound(SDFNavDat)
		''    Set SDFNavDat(I).pPtGeo = Nothing
		''    Set SDFNavDat(I).pPtPrj = Nothing
		''    Erase SDFNavDat(I).ValMax
		''    Erase SDFNavDat(I).ValMin
		''Next I
		''Erase SDFNavDat
		''
		''For I = 0 To UBound(SDFInterNavs)
		''    Set SDFInterNavs(I).pPtGeo = Nothing
		''    Set SDFInterNavs(I).pPtPrj = Nothing
		''Next I
		''Erase SDFInterNavs
		''
		''For I = 0 To UBound(IFNavDat)
		''    Set IFNavDat(I).pPtGeo = Nothing
		''    Set IFNavDat(I).pPtPrj = Nothing
		''    Erase IFNavDat(I).ValMax
		''    Erase IFNavDat(I).ValMin
		''Next I
		''Erase IFNavDat
		''
		''
		''For I = 0 To UBound(InSectList)
		''    Set InSectList(I).pPtGeo = Nothing
		''    Set InSectList(I).pPtPrj = Nothing
		''Next I

		Erase InSectList
		Erase InSectListIx
		On Error GoTo 0
	End Sub

	Private Sub CArrivalFrm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
	End Sub

	Property IsClosing As Boolean Implements IProcedureForm.IsClosing
		Get
			Return _IsClosing
		End Get

		Set(value As Boolean)
			_IsClosing = value
		End Set
	End Property

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

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

	Private Sub ShowPanelBtn_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPanelBtn.CheckedChanged
		If Not bFormInitialised Then Return

		If ShowPanelBtn.Checked Then
			Me.Width = 785
			ShowPanelBtn.Image = Arrival.My.Resources.bmpHIDE_INFO
		Else
			Me.Width = 601
			ShowPanelBtn.Image = Arrival.My.Resources.bmpSHOW_INFO
		End If

		If NextBtn.Enabled Then
			NextBtn.Focus()
		Else
			PrevBtn.Focus()
		End If
	End Sub

	Private Sub OkBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OkBtn.Click
		Dim RepFileName As String
		Dim RepFileTitle As String

		If Not ShowSaveDialog(RepFileName, RepFileTitle) Then Return

		screenCapture.Save(Me)

		Dim pReport As ReportHeader
		'Dim sProcName As String

		'If OptionButton0101.Checked Then
		'	sProcName = _Label0101_5.Text + "_RWY" + ComboBox0101.Text + "_CAT_" + ComboBox0001.Text
		'Else
		'	sProcName = _Label0101_5.Text + "_CAT_" + ComboBox0001.Text
		'End If

		'pReport.Procedure = sProcName
		pReport.Procedure = RepFileTitle

		pReport.Aerodrome = CurrADHP.Name
		pReport.Database = gAranEnv.ConnectionInfo.Database
		'pReport.EffectiveDate = pProcedure.TimeSlice.ValidTime.BeginPosition

		pReport.Category = ComboBox0001.Text

		SaveAccuracy(RepFileName, RepFileTitle, pReport)
		SaveLog(RepFileName, RepFileTitle, pReport)
		SaveProtocol(RepFileName, RepFileTitle, pReport)
		SaveGeometry(RepFileName, RepFileTitle, pReport)
		SaveInputData(RepFileName)

		Try
			If Not SaveProcedure(RepFileTitle) Then Return
		Catch ex As Exception
			gAranEnv.GetLogger("Non precision Arrival").Error(ex, "Ok button click")
			Throw
		End Try

		_IsClosing = True
		saveReportToDB()
		saveScreenshotToDB()
		'bUnloadByOk = True
		Me.Close()
	End Sub

	Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
		_IsClosing = True
		Me.Close()
	End Sub

	Private Sub ReportButton_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ReportButton.CheckedChanged
		If Not bFormInitialised Then Return

		If ReportButton.Checked Then
			NonPrecReportFrm.Show(_Win32Window)
		Else
			NonPrecReportFrm.Hide()
		End If
	End Sub

	Private Sub ProfileBtn_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ProfileBtn.CheckedChanged
		If Not bFormInitialised Then Return

		If ProfileBtn.Checked Then
			ArrivalProfile.Show(_Win32Window)
		Else
			ArrivalProfile.Hide()
		End If
	End Sub

#End Region

#Region "Utilitary routines"
	Private Function GetRDH() As Double
		If OptionButton0102.Checked Then Return fMinOCH
		Return arAbv_Treshold.Value + FictTHR.Z - fRefHeight
	End Function

	Private Sub FocusStepCaption(ByRef StIndex As Integer)
		Dim I As Integer

		For I = 0 To UBound(PageLabel)
			PageLabel(I).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
			PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
		Next

		PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
		PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

		'"PANDA 5.1.102.1.0"
		Me.Text = My.Resources.str00001 + "  [" + MultiPage1.TabPages.Item(StIndex).Text + "]"      '+ " " + My.Resources.str00818'
	End Sub

	Private Sub ClearGraphics()
		Dim I As Integer
		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement

		On Error Resume Next

		For Each elem As ArcGIS.Carto.IElement In pSubtrahendElem
			SafeDeleteElement(elem)
		Next elem

		If Not IntermediateBaseAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateBaseAreaElem)
		If Not IntermediateSecAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateSecAreaElem)
		If Not TerminationFIXElem Is Nothing Then pGraphics.DeleteElement(TerminationFIXElem)
		If Not PrimeMAPolyElement Is Nothing Then pGraphics.DeleteElement(PrimeMAPolyElement)
		If Not VisualAreaElement Is Nothing Then pGraphics.DeleteElement(VisualAreaElement)
		If Not FinalBaseAreaElem Is Nothing Then pGraphics.DeleteElement(FinalBaseAreaElem)
		If Not FinalSecAreaElem Is Nothing Then pGraphics.DeleteElement(FinalSecAreaElem)
		If Not RightPolyElement Is Nothing Then pGraphics.DeleteElement(RightPolyElement)
		If Not PrimePolyElement Is Nothing Then pGraphics.DeleteElement(PrimePolyElement)
		If Not LeftPolyElement Is Nothing Then pGraphics.DeleteElement(LeftPolyElement)
		If Not FullPolyElement Is Nothing Then pGraphics.DeleteElement(FullPolyElement)
		If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
		'If Not FictThrElement Is Nothing Then pGraphics.DeleteElement(FictThrElement)
		If Not FASegmElement Is Nothing Then pGraphics.DeleteElement(FASegmElement)
		If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
		If Not IFPolyElement Is Nothing Then pGraphics.DeleteElement(IFPolyElement)
		If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
		If Not MAPtAreaElem Is Nothing Then pGraphics.DeleteElement(MAPtAreaElem)
		If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)

		If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
		If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
		If Not FAFAreaElem Is Nothing Then pGraphics.DeleteElement(FAFAreaElem)
		If Not MAPtFixElem Is Nothing Then pGraphics.DeleteElement(MAPtFixElem)
		If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
		If Not FarFAFElem Is Nothing Then pGraphics.DeleteElement(FarFAFElem)
		If Not FAFFIXElem Is Nothing Then pGraphics.DeleteElement(FAFFIXElem)
		If Not PtSDFElem Is Nothing Then pGraphics.DeleteElement(PtSDFElem)
		If Not IFFIXElem Is Nothing Then pGraphics.DeleteElement(IFFIXElem)
		If Not CLElement Is Nothing Then pGraphics.DeleteElement(CLElement)
		If Not K1K1Elem Is Nothing Then pGraphics.DeleteElement(K1K1Elem)
		If Not MAPtElem Is Nothing Then pGraphics.DeleteElement(MAPtElem)
		If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
		If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
		If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
		If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
		If Not SOCElem Is Nothing Then pGraphics.DeleteElement(SOCElem)
		If Not SDFElem Is Nothing Then pGraphics.DeleteElement(SDFElem)
		If Not (pShablonElem Is Nothing) Then pGraphics.DeleteElement(pShablonElem)

		Functions.SafeDeleteElement(pTraceSelectElem)
		Functions.SafeDeleteElement(pProtectSelectElem)
		Functions.SafeDeleteElement(pTraceElem)
		Functions.SafeDeleteElement(pProtectElem)

		If Not FIXElem Is Nothing Then
			pGroupElement = FIXElem
			For I = 0 To pGroupElement.ElementCount - 1
				If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
			Next I
		End If

		If Not KKElem Is Nothing Then pGraphics.DeleteElement(KKElem)

		If Not TerminationFIXElem Is Nothing Then
			pGroupElement = TerminationFIXElem
			For I = 0 To pGroupElement.ElementCount - 1
				If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
			Next I
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		On Error GoTo 0
	End Sub

	Function PointInInterval(ByRef iMin As Double, ByRef iMax As Double, ByRef Val_Renamed As Double) As Integer
		If Val_Renamed < iMin Then Return -1
		If Val_Renamed > iMax Then Return 1
		Return 0
	End Function

#End Region

#Region "Page 01"

	Private Function CalcVsMOC(ByVal Bank As Double, ByVal cIx As Integer, ByRef OIx As Integer) As Double
		Dim I As Integer
		Dim R__ As Double
		Dim Result As Double

		Dim pTopo As ITopologicalOperator2
		Dim sumPoly As IPointCollection
		Dim pGeometry As IGeometry
		Dim ptCentr As IPoint


		'=============================================================
		R__ = DeConvertDistance(CDbl(TextBox0010.Text)) '2.0 * Res + carStraightSegmen.Values(cIx)

		sumPoly = New ESRI.ArcGIS.Geometry.Polygon

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
		ConvexPoly = pTopo.ConvexHull

		pTopo = ConvexPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Result = MaxObstacleHeightInPoly(ObstacleList, MSAObstacleList, arObsClearance.Values(cIx), ConvexPoly, OIx)

		Result = Result + arObsClearance.Values(cIx)

		If Result < arMinOCH.Values(cIx) Then
			Result = arMinOCH.Values(cIx)
			OIx = -1
		End If
		NonPrecReportFrm.FillPage01(MSAObstacleList, arObsClearance.Values(cIx), OIx)

		SafeDeleteElement(VisualAreaElement)

		VisualAreaElement = DrawPolygon(ConvexPoly, RGB(0, 0, 255), , False)

		pGraphics.AddElement(VisualAreaElement, 0)
		VisualAreaElement.Locked = True
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		Return Result
	End Function

	Private Sub VsManev() 'Optional ByRef newH As Double = -1.0
		Dim OIx As Integer

		Dim Res As Double
		Dim Bank As Double
		Dim TASwWind As Double

		TASwWind = _visualTAS + 3.6 * arVisualWS.Value

		Bank = RadToDeg(System.Math.Atan(0.003 * PI * TASwWind / 6.355))
		If Bank > arVisAverBank.Value Then Bank = arVisAverBank.Value

		TextBox0002.Text = CStr(System.Math.Round(Bank))
		TextBox0003.Text = CStr(System.Math.Round(6355.0 * System.Math.Tan(DegToRad(Bank)) / (PI * TASwWind), 1))
		TextBox0007.Text = CStr(ConvertSpeed(TASwWind * 0.277777777777778, eRoundMode.NEAREST))

		Res = Bank2Radius(Bank, TASwWind)

		'R__ = 2.0 * Res + carStraightSegmen.Values(cat)
		'TextBox0010.Text = CStr(ConvertDistance(R__, eRoundMode.rmNERAEST))

		TextBox0008.Text = CStr(ConvertDistance(Res, eRoundMode.NEAREST))
		TextBox0009.Text = CStr(ConvertDistance(carStraightSegmen.Values(Category), eRoundMode.NEAREST))

		fVisAprOCH = CalcVsMOC(Bank, Category, OIx)
		fNewVisAprOCH = fVisAprOCH
		'=============================================================
		TextBox0004.Text = CStr(ConvertHeight(CurrADHP.pPtGeo.Z, eRoundMode.NEAREST))
		TextBox0005.Text = CStr(ConvertHeight(arObsClearance.Values(Category), eRoundMode.NEAREST))
		TextBox0006.Text = CStr(ConvertHeight(arMinOCH.Values(Category), eRoundMode.NEAREST))

		TextBox0011.Text = CStr(ConvertHeight(fVisAprOCH, eRoundMode.NEAREST))
		TextBox0012.Text = CStr(ConvertHeight(fVisAprOCH + CurrADHP.pPtGeo.Z, eRoundMode.NEAREST))

		If OIx >= 0 Then
			TextBox0013.Text = MSAObstacleList(OIx).UnicalName
		Else
			TextBox0013.Text = ""
		End If
	End Sub

	Private Sub ComboBox0001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0001.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim Res As Double
		Dim R__ As Double
		Dim Bank As Double
		Dim TASwWind As Double

		Category = ComboBox0001.SelectedIndex
		TextBox0903.Text = ComboBox0001.Text

		_visualIAS = cVva.Values(Category)

		TextBox0001.Text = ConvertSpeed(_visualIAS, eRoundMode.SPECIAL).ToString()
		TextBox0001.Tag = TextBox0001.Text

		_visualTAS = IAS2TAS(3.6 * _visualIAS, CurrADHP.pPtGeo.Z + 300.0, CurrADHP.ISAtC)
		TASwWind = _visualTAS + 3.6 * arVisualWS.Value

		Bank = RadToDeg(System.Math.Atan(0.003 * PI * TASwWind / 6.355))
		If Bank > arVisAverBank.Value Then Bank = arVisAverBank.Value

		Res = Bank2Radius(Bank, TASwWind)
		R__ = 2.0 * Res + carStraightSegmen.Values(Category)
		TextBox0010.Text = CStr(ConvertDistance(R__, eRoundMode.NEAREST))

		VsManev()                                                                                                   '/ arMinHV_FAS.Multiplier													/ arMaxHV_FAS.Multiplier
	End Sub

	'Private Sub ComboBox0002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) 'Handles ComboBox0002.SelectedIndexChanged
	'	Dim I As Integer
	'	Dim N As Integer

	'	I = ComboBox0002.SelectedIndex
	'	If I < 0 Then Return

	'	'm_CurrADHP = ADHPList(I)
	'	'FillADHPFields(m_CurrADHP)
	'	'If m_CurrADHP.pPtGeo Is Nothing Then
	'	'	MsgBox("Error reading ADHP.", MsgBoxStyle.Critical, "PANDA")
	'	'	Return
	'	'End If

	'	fRefHeight = CurrADHP.pPtGeo.Z

	'	ListView0001.Items.Clear()

	'	FillRWYList(RWYDATA, CurrADHP)
	'	N = UBound(RWYDATA)
	'	If N < 0 Then
	'		'gAranEnv.ShowError(My.Resources.str300)
	'		MsgBox(My.Resources.str300, MsgBoxStyle.Critical, "PANDA")
	'		Return
	'	End If

	'	ReDim RWYIndex(N)

	'	For I = 0 To N
	'		RWYDATA(I).Selected = True
	'		If (I And 1) = 0 Then
	'			ListView0001.Items.Add(RWYDATA(I).Name + " / " + RWYDATA(I).PairName)
	'			ListView0001.Items.Item(I \ 2).Checked = True
	'		End If
	'	Next I

	'	GetArObstacles(ObstacleList, CurrADHP, RModel, fRefHeight)

	'	If ComboBox0001.SelectedIndex >= 0 Then
	'		ComboBox0001_SelectedIndexChanged(ComboBox0001, New System.EventArgs())
	'	Else
	'		ComboBox0001.SelectedIndex = 0
	'	End If
	'End Sub

	Private Sub TextBox0001_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0001.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0001_Validating(TextBox0001, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0001.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0001_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0001.Validating
		Dim fTmp As Double

		If Not Double.TryParse(TextBox0001.Text, fTmp) Then
			If Double.TryParse(TextBox0001.Tag, fTmp) Then
				TextBox0001.Text = TextBox0001.Tag
				Return
			End If

			TextBox0001.Text = ConvertSpeed(_visualIAS).ToString()
			Return
		End If

		_visualIAS = DeConvertSpeed(fTmp)
		fTmp = _visualIAS

		If _visualIAS < cVfafMin.Values(Category) Then _visualIAS = cVfafMin.Values(Category)
		If _visualIAS > cVva.Values(Category) Then _visualIAS = cVva.Values(Category)

		If fTmp <> _visualIAS Then
			TextBox0001.Text = ConvertSpeed(_visualIAS).ToString()
		End If
		TextBox0001.Tag = TextBox0001.Text

		_visualTAS = IAS2TAS(3.6 * _visualIAS, CurrADHP.pPtGeo.Z + 300.0, CurrADHP.ISAtC)
		'===============================================================================================

		Dim TASwWind As Double
		Dim Bank As Double
		Dim Res As Double
		Dim R__ As Double

		TASwWind = _visualTAS + 3.6 * arVisualWS.Value

		Bank = RadToDeg(System.Math.Atan(0.003 * PI * TASwWind / 6.355))
		If Bank > arVisAverBank.Value Then Bank = arVisAverBank.Value

		Res = Bank2Radius(Bank, TASwWind)
		R__ = 2.0 * Res + carStraightSegmen.Values(Category)
		TextBox0010.Text = CStr(ConvertDistance(R__, eRoundMode.NEAREST))

		VsManev()                                                                                                   '/ arMinHV_FAS.Multiplier													/ arMaxHV_FAS.Multiplier
	End Sub

	Private Sub TextBox0010_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0010.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0010_Validating(TextBox0010, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0010.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0010_Validating(ByVal eventSender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox0010.Validating
		Dim Res As Double
		Dim localR As Double
		Dim Bank As Double
		Dim fTmp As Double
		Dim TASwWind As Double

		If TextBox0010.ReadOnly Then Return

		TASwWind = _visualTAS + 3.6 * arVisualWS.Value

		Bank = RadToDeg(System.Math.Atan(0.003 * PI * TASwWind / 6.355))
		If Bank > arVisAverBank.Value Then Bank = arVisAverBank.Value
		Res = Bank2Radius(Bank, TASwWind)

		localR = 2.0 * Res + carStraightSegmen.Values(Category)

		If Not IsNumeric(TextBox0010.Text) Then
			TextBox0010.Text = ConvertDistance(localR, eRoundMode.NEAREST).ToString()
		End If

		fTmp = DeConvertDistance(CDbl(TextBox0010.Text))
		If fTmp > localR * 1.43 Then fTmp = localR * 1.43
		If fTmp < 0.7 * localR Then fTmp = 0.7 * localR

		TextBox0010.Text = ConvertDistance(fTmp, eRoundMode.NEAREST).ToString()
		VsManev()
	End Sub

	Private Sub ListView0001_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView0001.ItemChecked
		Static ListView0001InUse As Boolean = False

		If ComboBox0001.SelectedIndex < 0 Then Return
		If ListView0001InUse Then Return
		ListView0001InUse = True

		Dim Item As System.Windows.Forms.ListViewItem = e.Item 'ListView0001.Items(eventArgs.Index)
		Dim I As Integer
		Dim N As Integer
		Dim Checked As Boolean

		Try
			N = 0
			For I = 0 To ListView0001.Items.Count - 1
				Checked = ListView0001.Items.Item(I).Checked
				RWYDATA(I * 2).Selected = Checked
				RWYDATA(I * 2 + 1).Selected = Checked
				If Checked Then N = N + 2
			Next I

			If N = 0 Then '        msgbox "Выберите ВПП для выполнения данной команды.", vbExclamation'        Return
				Item.Checked = True
				I = Item.Index
				RWYDATA(I * 2).Selected = True
				RWYDATA(I * 2 + 1).Selected = True
			Else
				VsManev()
			End If
		Finally
			ListView0001InUse = False
		End Try
	End Sub

#End Region

#Region "Page 02"

	Private Function FillArNavListForCircling(ByVal pCentroid As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim N As Integer
		Dim M As Integer
		Dim K As Integer
		Dim I As Integer
		Dim J As Integer
		Dim ILS As NavaidData
		Dim fDist As Double
		Dim MaxNavDist As Double

		N = UBound(NavaidList)
		M = UBound(RWYDATA)
		ReDim ArNavList(N + M + 2)
		J = -1

		For I = 0 To M
			If RWYDATA(I).Selected Then
				K = GetILS(RWYDATA(I), ILS, CurrADHP)
				If (K And 1) = 1 Then
					J += 1
					ArNavList(J) = ILS
					'ArNavList(J).pPtGeo = ILS.pPtGeo
					'ArNavList(J).pPtPrj = ILS.pPtPrj

					'ArNavList(J).Name = ILS.CallSign
					'ArNavList(J).CallSign = ILS.CallSign
					'ArNavList(J).Identifier = ILS.Identifier
					'ArNavList(J).NAV_Ident = ILS.NAV_Ident

					'ArNavList(J).MagVar = ILS.MagVar
					ArNavList(J).TypeCode = eNavaidType.LLZ

					ArNavList(J).Range = 70000.0 'LLZData.Range
					ArNavList(J).Index = ILS.Index
					ArNavList(J).PairNavaidIndex = -1

					'ArNavList(J).GPAngle = ILS.GPAngle
					'ArNavList(J).GP_RDH = ILS.GP_RDH

					'ArNavList(J).Course = ILS.Course
					'ArNavList(J).LLZ_THR = ILS.LLZ_THR
					'ArNavList(J).SecWidth = ILS.SecWidth
					'ArNavList(J).AngleWidth = ILS.AngleWidth
					'ArNavList(J).pFeature = ILS.pFeature
				End If
			End If
		Next

		If (N < 0) And (J < 0) Then
			ReDim ArNavList(-1)
			Return -1
		End If

		For I = 0 To N
			fDist = ReturnDistanceInMeters(NavaidList(I).pPtPrj, pCentroid)

			If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
				MaxNavDist = VOR.FA_Range
			ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
				MaxNavDist = NDB.FA_Range
			ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
				MaxNavDist = LLZ.Range
			End If

			If fDist < MaxNavDist Then
				J += 1
				ArNavList(J) = NavaidList(I)
			End If
		Next

		If J >= 0 Then
			ReDim Preserve ArNavList(J)
		Else
			ReDim ArNavList(-1)
		End If
		Return J
	End Function

	Private Function FillArNavListForStraightIn(ByVal RWY As RWYType, ByVal AircraftCategory As Integer) As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer

		Dim MinDistStraightInApproach As Double
		Dim MaxDistStraightInApproach As Double
		Dim InterceptAngle As Double
		Dim MaxNavDist As Double
		Dim fDist As Double
		Dim fDir0 As Double
		Dim fDir1 As Double

		Dim ILS As NavaidData
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint

		N = UBound(NavaidList)
		K = GetILS(RWY, ILS, CurrADHP)

		M = N

		If (K And 1) = 1 Then M += 1

		If M < 0 Then
			ReDim ArNavList(-1)
			Return -1
		End If

		ReDim ArNavList(M)
		J = -1
		If (K And 1) = 1 Then
			J = 0
			ArNavList(J) = ILS

			ArNavList(J).pPtGeo = ILS.pPtGeo
			ArNavList(J).pPtPrj = ILS.pPtPrj

			'ArNavList(J).Name = ILS.CallSign
			'ArNavList(J).CallSign = ILS.CallSign
			'ArNavList(J).Identifier = ILS.Identifier
			'ArNavList(J).NAV_Ident = ILS.Identifier
			'ArNavList(J).MagVar = ILS.MagVar
			ArNavList(J).TypeCode = eNavaidType.LLZ

			ArNavList(J).Range = 70000.0 'LLZData.range
			ArNavList(J).Index = ILS.Index
			ArNavList(J).PairNavaidIndex = -1

			'ArNavList(J).GPAngle = ILS.GPAngle
			'ArNavList(J).GP_RDH = ILS.GP_RDH

			'ArNavList(J).Course = ILS.Course
			'ArNavList(J).LLZ_THR = ILS.LLZ_THR
			'ArNavList(J).SecWidth = ILS.SecWidth
			'ArNavList(J).AngleWidth = ILS.AngleWidth
		End If

		MinDistStraightInApproach = arMinInterDist.Value
		MaxDistStraightInApproach = MaxDistStraightInApproach8000 'arMinInterDist.Value + arMinInterToler.Value / System.Math.Tan(DegToRad(arStrInAlignment.Value)) 'arFAFLenght.Value
		InterceptAngle = arMaxInterAngle.Values(AircraftCategory)
		'RWYAngle = ReturnAngleInDegrees(RWYTHRPrj, RWYEndPrj)

		ptMin = PointAlongPlane(RWY.pPtPrj(eRWY.PtTHR), RWY.pPtPrj(eRWY.PtTHR).M + 180.0, MinDistStraightInApproach)
		ptMax = PointAlongPlane(RWY.pPtPrj(eRWY.PtTHR), RWY.pPtPrj(eRWY.PtTHR).M + 180.0, MaxDistStraightInApproach)

		For I = 0 To N
			fDist = ReturnDistanceInMeters(NavaidList(I).pPtPrj, RWY.pPtPrj(eRWY.PtTHR))
			If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
				MaxNavDist = VOR.FA_Range
			ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
				MaxNavDist = NDB.FA_Range
			ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
				MaxNavDist = LLZ.Range
			End If

			If (fDist > MaxNavDist) Then Continue For

			fDir0 = Modulus(RWY.pPtPrj(eRWY.PtTHR).M - ReturnAngleInDegrees(NavaidList(I).pPtPrj, ptMin), 360.0)
			fDir1 = Modulus(RWY.pPtPrj(eRWY.PtTHR).M - ReturnAngleInDegrees(NavaidList(I).pPtPrj, ptMax), 360.0)

			If fDir0 > 180.0 Then fDir0 = 360.0 - fDir0
			If fDir0 > 90.0 Then fDir0 = 180.0 - fDir0

			If fDir1 > 180.0 Then fDir1 = 360.0 - fDir1
			If fDir1 > 90.0 Then fDir1 = 180.0 - fDir1

			If (fDir0 < InterceptAngle) Or (fDir1 < InterceptAngle) Then
				J += 1
				ArNavList(J) = NavaidList(I)
			End If
		Next

		If J >= 0 Then
			ReDim Preserve ArNavList(J)
		Else
			ReDim ArNavList(-1)
		End If
		Return J
	End Function

	Private Sub OptionButton0101_CheckedChanged(eventSender As System.Object, eventArgs As System.EventArgs) Handles OptionButton0101.CheckedChanged, OptionButton0102.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim N As Integer
		Dim I As Integer

		If OptionButton0101.Checked Then
			_Label0601_6.Text = My.Resources.str10312
			'_Label0601_6.Width = 99
			_Label0601_6.Top = 27

			ListView0202.Visible = False
			_Label0301_16.Visible = False
			TextBox0308.Visible = False
			_Label0301_17.Visible = False

			ComboBox0101.Visible = True
			TextBox0101.Visible = True
			TextBox0103.Visible = True

			_Label0201_3.Text = My.Resources.str10304 + CStr(ConvertDistance(arMinInterDist.Value, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str10310

			Label0101_0.Visible = True
			Label0101_1.Visible = True
			_Label0101_4.Visible = True
			_Label0101_6.Visible = True
			_Label0101_9.Visible = True

			_Label0201_8.Visible = True
			_Label0201_1.Visible = True
			_Label0201_2.Visible = True
			_Label0201_9.Visible = True
			_Label0201_14.Visible = True
			_Label0201_13.Visible = True
			_Label0201_5.Visible = True
			_Label0201_12.Visible = True
			_Label0201_17.Visible = True

			If ComboBox0101.SelectedIndex >= 0 Then
				ComboBox0101_SelectedIndexChanged(ComboBox0101, New System.EventArgs())
			Else
				ComboBox0101.SelectedIndex = 0
			End If

			TextBox0102.Text = CStr(ConvertDistance(ReturnDistanceInMeters(FinalNav.pPtPrj, RWYTHRPrj), eRoundMode.NEAREST))
			_Label0101_3.Text = My.Resources.str10204  '"Удаление РНС от порога ВПП"
		Else
			_Label0601_6.Text = My.Resources.str10313  '"Высота над ближащей точкой ВПП:"
			'_Label0601_6.Width = 96
			_Label0601_6.Top = 18

			ListView0202.Visible = True
			_Label0301_16.Visible = True
			TextBox0308.Visible = True
			_Label0301_17.Visible = True

			ComboBox0101.Visible = False
			TextBox0101.Visible = False
			TextBox0103.Visible = False

			_Label0201_3.Text = My.Resources.str10222  '"Расстояние РНС от ближайщей" + vbCrLf + "поверхности ВПП:"

			Label0101_0.Visible = False
			Label0101_1.Visible = False
			_Label0101_4.Visible = False
			_Label0101_6.Visible = False
			_Label0101_9.Visible = False

			_Label0201_8.Visible = False
			_Label0201_1.Visible = False
			_Label0201_2.Visible = False
			_Label0201_9.Visible = False
			_Label0201_14.Visible = False
			_Label0201_13.Visible = False
			_Label0201_5.Visible = False
			_Label0201_12.Visible = False
			_Label0201_17.Visible = False

			_Label0101_3.Text = My.Resources.str10209  '"Расстояние от РНС до КТА "
			'===============================================

			fRefHeight = CurrADHP.pPtGeo.Z
			_Label0101_8.Text = My.Resources.str10212 + " ref. elev = " + CStr(ConvertHeight(fRefHeight, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit  '"OCH определяется относительно превышения аэродрома"

			ComboBox0102.Items.Clear()

			N = FillArNavListForCircling(_pCentroid)

			If N >= 0 Then
				For I = 0 To N
					ComboBox0102.Items.Add(ArNavList(I).CallSign)
				Next I
				ComboBox0102.SelectedIndex = 0
			Else
				MessageBox.Show("There are not suitable facility for guidance.")
				Return
			End If

			TextBox0102.Text = CStr(ConvertDistance(ReturnDistanceInMeters(FinalNav.pPtPrj, CurrADHP.pPtPrj), eRoundMode.NEAREST))
		End If
	End Sub

	Private Sub ComboBox0101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0101.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim K As Integer
		Dim I As Integer
		Dim N As Integer

		K = ComboBox0101.SelectedIndex
		If K < 0 Then Return

		SelectedRWY = RWYDATA(RWYIndex(K))

		RWYTHRPrj = SelectedRWY.pPtPrj(eRWY.PtTHR)

		TextBox0101.Text = CStr(ConvertAngle(Modulus(SelectedRWY.TrueBearing, 360.0), eRoundMode.NEAREST)) 'ik
		TextBox0103.Text = CStr(ConvertAngle(Modulus(SelectedRWY.TrueBearing - CurrADHP.MagVar, 360.0), eRoundMode.NEAREST)) 'mk

		If OptionButton0101.Checked And (RWYTHRPrj.Z < CurrADHP.pPtGeo.Z - 2.0) Then
			fRefHeight = RWYTHRPrj.Z
			_Label0101_8.Text = My.Resources.str10213 + " ref. elev = " + CStr(ConvertHeight(fRefHeight, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit  '"OCH определяется относительно уровня порога ВПП"
		Else
			fRefHeight = CurrADHP.pPtGeo.Z
			_Label0101_8.Text = My.Resources.str10212 + " ref. elev = " + CStr(ConvertHeight(fRefHeight, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit  '"OCH определяется относительно превышения аэродрома"
		End If

		ComboBox0102.Items.Clear()

		N = FillArNavListForStraightIn(SelectedRWY, ComboBox0001.SelectedIndex)
		If N >= 0 Then
			For I = 0 To N
				ComboBox0102.Items.Add(ArNavList(I).CallSign)
			Next I
			ComboBox0102.SelectedIndex = 0
		Else
			MessageBox.Show("There are not suitable facility for guidance.")
			Return
		End If
	End Sub

	Private Sub ComboBox0102_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0102.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim K As Integer
		Dim Side1 As Integer
		Dim Side2 As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim bSolution As Boolean
		Dim NavOn As Boolean 'Средство находится дальше 1400 м от ВПП
		Dim TopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRoxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pRWYsPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pRWYLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint

		Dim Rad1 As Double
		Dim Rad2 As Double
		Dim Rad3 As Double
		Dim Rad4 As Double
		Dim fTmp1 As Double
		Dim fTmp As Double

		'===========Constants==================
		Dim MinDistStraightInApproach As Double
		Dim MaxDistStraightInApproach As Double
		Dim OnAeroRange As Double
		Dim TolerDist As Double
		Dim Theta1 As Double
		Dim Theta2 As Double

		Dim dRad As Double
		Dim dRad1 As Double
		Dim dRad2 As Double
		Dim dRad3 As Double
		Dim dRad4 As Double

		Dim Dist As Double
		Dim Dist1 As Double
		Dim Dist2 As Double

		Dim RadToAirport As Double

		Dim SideNav As Integer
		Dim ConstrPoint As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim PtR1400 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtL1400 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim Prestr(1) As String
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim ItemStr As String

		On Error Resume Next
		If Not CLElement Is Nothing Then pGraphics.DeleteElement(CLElement)
		On Error GoTo 0

		K = ComboBox0102.SelectedIndex
		If K < 0 Then Return

		FinalNav = ArNavList(K)
		_Label0101_5.Text = GetNavTypeName(FinalNav.TypeCode)

		If FinalNav.TypeCode = eNavaidType.LLZ Then
			Frame0102.Visible = True
			OptionButton0202.Enabled = False
			CheckBox0603.Enabled = False

			OptionButton0201.Checked = True
		Else
			Frame0102.Visible = False
			OptionButton0202.Enabled = True
			CheckBox0603.Enabled = FinalNav.PairNavaidIndex >= 0

			FinalNav.GP_RDH = arAbv_Treshold.Value
		End If
		'===========================================================================

		OnAeroRange = arCirclAprShift.Value
		TolerDist = arMinInterToler.Value
		Theta2 = arStrInAlignment.Value
		Theta1 = arMaxInterAngle.Values(ComboBox0001.SelectedIndex)

		MinDistStraightInApproach = arMinInterDist.Value
		MaxDistStraightInApproach = MaxDistStraightInApproach8000 'arMinInterDist.Value + arMinInterToler.Value / System.Math.Tan(DegToRad(arStrInAlignment.Value)) 'arFAFLenght.Value

		ListView0101.Items.Clear()

		pRWYLine = New ESRI.ArcGIS.Geometry.Polyline
		pRWYLine.FromPoint = RWYCollection.Point(0)

		If RWYCollection.PointCount > 1 Then pRWYLine.ToPoint = RWYCollection.Point(1)

		'========== Проверка приаэродромного средства =====
		'If RWYCollection.PointCount < 3 Then 'Расстояние РНС от порога ВПП :
		If OptionButton0101.Checked Then
			_Label0101_3.Text = My.Resources.str10204  ' "Ближайшее расстояние от РНС до порога ВПП :"

			Dist = ReturnDistanceInMeters(FinalNav.pPtPrj, RWYTHRPrj)
			TextBox0102.Text = CStr(ConvertDistance(Dist, eRoundMode.NEAREST))

			pRoxi = pRWYLine
		Else
			_Label0101_3.Text = My.Resources.str10209  '"Ближайшее расстояние от РНС до КТА :"

			Dist = ReturnDistanceInMeters(FinalNav.pPtPrj, CurrADHP.pPtPrj)
			TextBox0102.Text = CStr(ConvertDistance(Dist, eRoundMode.NEAREST))

			'Dist = ReturnDistanceInMeters(FinalNav.pPtPrj, m_pCentroid)

			If RWYCollection.PointCount < 3 Then
				pRoxi = pRWYLine
			Else
				TopoOper = RWYCollection
				pRWYsPolygon = TopoOper.ConvexHull

				TopoOper = pRWYsPolygon
				TopoOper.IsKnownSimple_2 = False
				TopoOper.Simplify()
				pRoxi = pRWYsPolygon
			End If
		End If

		'If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
		'	If VOR.FA_Range <= Dist Then
		'		TextBox0102.ForeColor = Color.Red
		'	Else
		'		TextBox0102.ForeColor = Color.Black
		'	End If
		'ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
		'	If NDB.FA_Range <= Dist Then
		'		TextBox0102.ForeColor = Color.Red
		'	Else
		'		TextBox0102.ForeColor = Color.Black
		'	End If
		'ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
		'	If LLZ.Range <= Dist Then
		'		TextBox0102.ForeColor = Color.Red
		'	Else
		'		TextBox0102.ForeColor = Color.Black
		'	End If
		'End If
		'bSolution = ArPANSOPS_MaxNavDist > Dist

		bSolution = True

		Dist = pRoxi.ReturnDistance(FinalNav.pPtPrj)
		_OnAero = Dist < OnAeroRange

		If _OnAero Then
			_Label0101_7.Text = My.Resources.str10210  '"Аэродромный"
		Else
			_Label0101_7.Text = My.Resources.str10211  '"Не аэродромный"
		End If

		OptionButton0201.Checked = True
		'OptionButton0202.Checked = False

		ReDim Solutions(-1)

		If Not bSolution Then
			ItemStr = My.Resources.str00303
			Replace(ItemStr, Chr(10), " ")
			itmX = ListView0101.Items.Add(ItemStr)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			ReDim Solutions(0)
			Solutions(0).Low = FinalNav.Course
			Solutions(0).High = FinalNav.Course
			Solutions(0).Tag = 0

			itmX = ListView0101.Items.Add("LLZ")
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(0).Low)) + "°"))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(0).High)) + "°"))
		ElseIf OptionButton0101.Checked Then
			'===========Проверка сопряжения пути ================
			ptMin = PointAlongPlane(RWYTHRPrj, RWYTHRPrj.M + 180.0, MinDistStraightInApproach)
			ptMax = PointAlongPlane(RWYTHRPrj, RWYTHRPrj.M + 180.0, MaxDistStraightInApproach)

			pRWYLine.FromPoint = ptMax
			pRWYLine.ToPoint = ptMin

			CLElement = DrawPolyLine(pRWYLine, RGB(255, 0, 0), , False)
			'If CLState Then
			pGraphics.AddElement(CLElement, 0)
			CLElement.Locked = True
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
			'End If
			'===========Проверка Theta <= 5 ======
			Side1 = SideDef(ptMin, RWYTHRPrj.M - 90.0, FinalNav.pPtPrj)
			NavOn = Side1 > 0

			PtR1400 = PointAlongPlane(ptMin, RWYTHRPrj.M - 90.0, TolerDist)
			PtL1400 = PointAlongPlane(ptMin, RWYTHRPrj.M + 90.0, TolerDist)

			If NavOn Then '   Средство дальше 1400 м
				Side1 = SideDef(PtR1400, RWYTHRPrj.M + 180.0 + Theta2, FinalNav.pPtPrj)
				Side2 = SideDef(PtL1400, RWYTHRPrj.M + 180.0 - Theta2, FinalNav.pPtPrj)
				If (Side1 >= 0) And (Side2 <= 0) Then '   Решение существует
					Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, PtR1400)
					Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, PtL1400)

					fTmp1 = SubtractAngles(Rad1, RWYTHRPrj.M)
					If fTmp1 > Theta2 Then Rad1 = RWYTHRPrj.M - Theta2

					fTmp1 = SubtractAngles(Rad2, RWYTHRPrj.M)
					If fTmp1 > Theta2 Then Rad2 = RWYTHRPrj.M + Theta2

					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)

					If SubtractAnglesWithSign(ConvertAngle(Rad2, eRoundMode.CEIL), ConvertAngle(Rad1, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If

						Solutions(J).Low = ConvertAngle(Rad2, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(Rad1, eRoundMode.FLOOR)
						Solutions(J).Tag = 0
					Else
						ItemStr = My.Resources.str10215
						Replace(ItemStr, Chr(10), " ")

						itmX = ListView0101.Items.Add(ItemStr)
						ItemStr = My.Resources.str00303
						Replace(ItemStr, Chr(10), " ")
						itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					End If
				Else 'Решения нет
					ItemStr = My.Resources.str10215
					Replace(ItemStr, Chr(10), " ")
					itmX = ListView0101.Items.Add(ItemStr)
					ItemStr = My.Resources.str00303
					Replace(ItemStr, Chr(10), " ")
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
				End If
			Else 'Средство ближе 1400 м
				Side1 = SideDef(PtR1400, RWYTHRPrj.M - Theta2, FinalNav.pPtPrj)
				Side2 = SideDef(PtL1400, RWYTHRPrj.M + Theta2, FinalNav.pPtPrj)
				If (Side1 <= 0) And (Side2 >= 0) Then 'Решение существует
					Rad1 = ReturnAngleInDegrees(PtR1400, FinalNav.pPtPrj)
					Rad2 = ReturnAngleInDegrees(PtL1400, FinalNav.pPtPrj)

					fTmp1 = SubtractAngles(Rad1, RWYTHRPrj.M)
					If fTmp1 > Theta2 Then Rad1 = RWYTHRPrj.M + Theta2

					fTmp1 = SubtractAngles(Rad2, RWYTHRPrj.M)
					If fTmp1 > Theta2 Then Rad2 = RWYTHRPrj.M - Theta2

					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
					If FinalNav.TypeCode = eNavaidType.LLZ Then
						If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(0)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If

							Solutions(J).Low = FinalNav.pPtGeo.M
							Solutions(J).High = FinalNav.pPtGeo.M
							Solutions(J).Tag = 0
						Else
							ItemStr = My.Resources.str10215
							Replace(ItemStr, Chr(10), " ")
							itmX = ListView0101.Items.Add(ItemStr)
							ItemStr = My.Resources.str00303
							Replace(ItemStr, Chr(10), " ")
							itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						End If
					ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If
						Solutions(J).Low = ConvertAngle(Rad1, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
						Solutions(J).Tag = 0
					Else 'Решения нет
						ItemStr = My.Resources.str10215
						Replace(ItemStr, Chr(10), " ")
						itmX = ListView0101.Items.Add(ItemStr)

						ItemStr = My.Resources.str00303
						Replace(ItemStr, Chr(10), " ")
						itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					End If
				Else 'Решения нет
					ItemStr = My.Resources.str10215
					Replace(ItemStr, Chr(10), " ")
					itmX = ListView0101.Items.Add(ItemStr)

					ItemStr = My.Resources.str00303
					Replace(ItemStr, Chr(10), " ")
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
				End If
			End If
			'=========== Проверка 15 > Theta >= 0 ======
			SideNav = SideDef(RWYTHRPrj, RWYTHRPrj.M, FinalNav.pPtPrj)

			pt1 = New ESRI.ArcGIS.Geometry.Point
			ConstrPoint = pt1
			ConstrPoint.ConstructAngleIntersection(ptMin, DegToRad(RWYTHRPrj.M), FinalNav.pPtPrj, DegToRad(RWYTHRPrj.M + Theta1))

			pt2 = New ESRI.ArcGIS.Geometry.Point
			ConstrPoint = pt2
			ConstrPoint.ConstructAngleIntersection(ptMin, DegToRad(RWYTHRPrj.M), FinalNav.pPtPrj, DegToRad(RWYTHRPrj.M - Theta1))

			Side1 = SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, pt1)
			Dist1 = Point2LineDistancePrj(pt1, RWYTHRPrj, RWYTHRPrj.M - 90.0)
			Dist1 = Dist1 * Side1

			Side1 = SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, pt2)
			Dist2 = Point2LineDistancePrj(pt2, RWYTHRPrj, RWYTHRPrj.M - 90.0)
			Dist2 = Dist2 * Side1

			Side1 = PointInInterval(MinDistStraightInApproach, MaxDistStraightInApproach, Dist1)
			Side2 = PointInInterval(MinDistStraightInApproach, MaxDistStraightInApproach, Dist2)

			If Side1 * Side2 > 0 Then
				If Side1 > 0 Then
					If SideNav < 0 Then
						Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
						Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMax)
					Else
						Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMax)
						Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
					End If
				Else
					If SideNav < 0 Then
						Rad1 = ReturnAngleInDegrees(ptMin, FinalNav.pPtPrj)
						Rad2 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj)
					Else
						Rad1 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj)
						Rad2 = ReturnAngleInDegrees(ptMin, FinalNav.pPtPrj)
					End If
				End If

				Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
				Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)

				If FinalNav.TypeCode = eNavaidType.LLZ Then
					If AngleInSector(FinalNav.pPtGeo.M, Rad2, Rad1) Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If
						Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
						Solutions(J).High = Solutions(J).Low
						Solutions(J).Tag = 1
					Else
						ItemStr = My.Resources.str10214
						Replace(ItemStr, Chr(10), " ")
						itmX = ListView0101.Items.Add(ItemStr)

						ItemStr = My.Resources.str00303
						Replace(ItemStr, Chr(10), " ")
						itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					End If
				ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) < 0.0 Then
					ItemStr = My.Resources.str10214
					Replace(ItemStr, Chr(10), " ")
					itmX = ListView0101.Items.Add(ItemStr)

					ItemStr = My.Resources.str00303
					Replace(ItemStr, Chr(10), " ")
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
				Else
					J = UBound(Solutions)
					If J < 0 Then
						ReDim Solutions(0)
						J = 0
					Else
						J = J + 1
						ReDim Preserve Solutions(J)
					End If

					Solutions(J).Low = ConvertAngle(Rad1, eRoundMode.CEIL)
					Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
					Solutions(J).Tag = 1
				End If
			ElseIf Side1 * Side2 < 0 Then  'Решения нет
				ItemStr = My.Resources.str10214
				Replace(ItemStr, Chr(10), " ")
				itmX = ListView0101.Items.Add(ItemStr)

				ItemStr = My.Resources.str00303
				Replace(ItemStr, Chr(10), " ")
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
			Else
				If (Side1 = 0) And (Side2 < 0) Then                     '      3/\          /\2
					Rad1 = ReturnAngleInDegrees(pt1, FinalNav.pPtPrj)   '      /  \        /  \
					Rad2 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj) '<__\_/_+__\____\_/+___\____
					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)               '    \    / \    \    / \
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)               '     \  /        \  /
					OptRad = Rad1                                       '     1\/          \/4

					If FinalNav.TypeCode = eNavaidType.LLZ Then
						If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(0)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If
							Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
							Solutions(J).High = Solutions(J).Low
							Solutions(J).Tag = 1
						Else
							ItemStr = My.Resources.str10214
							Replace(ItemStr, Chr(10), " ")
							itmX = ListView0101.Items.Add(ItemStr)

							ItemStr = My.Resources.str00303
							Replace(ItemStr, Chr(10), " ")
							itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						End If
					ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If

						Solutions(J).Low = ConvertAngle(OptRad, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
						Solutions(J).Tag = 1
					End If
				ElseIf (Side1 = 0) And (Side2 > 0) Then
					Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, pt1)
					Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
					OptRad = Rad2

					If FinalNav.TypeCode = eNavaidType.LLZ Then
						If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(0)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If
							Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
							Solutions(J).High = Solutions(J).Low
							Solutions(J).Tag = 1
						Else
							ItemStr = My.Resources.str10214
							Replace(ItemStr, Chr(10), " ")
							itmX = ListView0101.Items.Add(ItemStr)

							ItemStr = My.Resources.str00303
							Replace(ItemStr, Chr(10), " ")
							itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						End If
					ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If

						Solutions(J).Low = ConvertAngle(Rad1, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(OptRad, eRoundMode.FLOOR)
						Solutions(J).Tag = 1
					End If
				ElseIf (Side1 < 0) And (Side2 = 0) Then
					Rad1 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj)
					Rad2 = ReturnAngleInDegrees(pt2, FinalNav.pPtPrj)
					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
					OptRad = Rad2

					If FinalNav.TypeCode = eNavaidType.LLZ Then
						If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(0)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If
							Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
							Solutions(J).High = Solutions(J).Low
							Solutions(J).Tag = 1
						Else
							ItemStr = My.Resources.str10214
							Replace(ItemStr, Chr(10), " ")
							itmX = ListView0101.Items.Add(ItemStr)

							ItemStr = My.Resources.str00303
							Replace(ItemStr, Chr(10), " ")
							itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						End If
					ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If

						Solutions(J).Low = ConvertAngle(Rad1, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(OptRad, eRoundMode.FLOOR)
						Solutions(J).Tag = 1
					End If
				ElseIf (Side1 > 0) And (Side2 = 0) Then
					Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
					Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, pt2)
					Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
					OptRad = Rad1

					If FinalNav.TypeCode = eNavaidType.LLZ Then
						If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(0)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If
							Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
							Solutions(J).High = Solutions(J).Low
							Solutions(J).Tag = 1
						Else
							ItemStr = My.Resources.str10214
							Replace(ItemStr, Chr(10), " ")
							itmX = ListView0101.Items.Add(ItemStr)

							ItemStr = My.Resources.str00303
							Replace(ItemStr, Chr(10), " ")
							itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
						End If
					ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
						J = UBound(Solutions)
						If J < 0 Then
							ReDim Solutions(0)
							J = 0
						Else
							J = J + 1
							ReDim Preserve Solutions(J)
						End If

						Solutions(J).Low = ConvertAngle(OptRad, eRoundMode.CEIL)
						Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
						Solutions(J).Tag = 1
					End If
				ElseIf (Side1 = 0) And (Side2 = 0) Then
					If Dist1 < Dist2 Then
						Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
						Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, pt1)
						Rad3 = ReturnAngleInDegrees(pt2, FinalNav.pPtPrj)
						Rad4 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj)

						Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
						Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
						Rad3 = Dir2Azt(FinalNav.pPtPrj, Rad3)
						Rad4 = Dir2Azt(FinalNav.pPtPrj, Rad4)
						OptRad = Rad1

						If FinalNav.TypeCode = eNavaidType.LLZ Then
							If AngleInSector(FinalNav.pPtGeo.M, Rad2, Rad1) Or AngleInSector(FinalNav.pPtGeo.M, Rad4, Rad3) Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If
								Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
								Solutions(J).High = Solutions(J).Low
								Solutions(J).Tag = 1
							Else
								ItemStr = My.Resources.str10214
								Replace(ItemStr, Chr(10), " ")
								itmX = ListView0101.Items.Add(ItemStr)

								ItemStr = My.Resources.str00303
								Replace(ItemStr, Chr(10), " ")
								itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
								itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							End If
						Else
							If SubtractAnglesWithSign(ConvertAngle(Rad4, eRoundMode.CEIL), ConvertAngle(Rad3, eRoundMode.FLOOR), 1) >= 0.0 Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If

								Solutions(J).Low = ConvertAngle(Rad4, eRoundMode.CEIL)
								Solutions(J).High = ConvertAngle(Rad3, eRoundMode.FLOOR)
								Solutions(J).Tag = 1
							End If

							If SubtractAnglesWithSign(ConvertAngle(Rad2, eRoundMode.CEIL), ConvertAngle(Rad1, eRoundMode.FLOOR), 1) >= 0.0 Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If

								Solutions(J).Low = ConvertAngle(Rad2, eRoundMode.CEIL)
								Solutions(J).High = ConvertAngle(OptRad, eRoundMode.FLOOR)
								Solutions(J).Tag = 1
							End If
						End If
					ElseIf Dist1 > Dist2 Then
						Rad1 = ReturnAngleInDegrees(FinalNav.pPtPrj, ptMin)
						Rad2 = ReturnAngleInDegrees(FinalNav.pPtPrj, pt2)
						Rad3 = ReturnAngleInDegrees(pt1, FinalNav.pPtPrj)
						Rad4 = ReturnAngleInDegrees(ptMax, FinalNav.pPtPrj)

						Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
						Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)
						Rad3 = Dir2Azt(FinalNav.pPtPrj, Rad3)
						Rad4 = Dir2Azt(FinalNav.pPtPrj, Rad4)
						OptRad = Rad1

						If FinalNav.TypeCode = eNavaidType.LLZ Then
							If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Or AngleInSector(FinalNav.pPtGeo.M, Rad3, Rad4) Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If
								Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
								Solutions(J).High = Solutions(J).Low
								Solutions(J).Tag = 1
							Else
								ItemStr = My.Resources.str10214
								Replace(ItemStr, Chr(10), " ")
								itmX = ListView0101.Items.Add(ItemStr)

								ItemStr = My.Resources.str00303
								Replace(ItemStr, Chr(10), " ")
								itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
								itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							End If
						Else
							If SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If

								Solutions(J).Low = ConvertAngle(OptRad, eRoundMode.CEIL)
								Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
								Solutions(J).Tag = 1
							End If

							If SubtractAnglesWithSign(ConvertAngle(Rad3, eRoundMode.CEIL), ConvertAngle(Rad4, eRoundMode.FLOOR), 1) >= 0.0 Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J = J + 1
									ReDim Preserve Solutions(J)
								End If
								Solutions(J).Low = ConvertAngle(Rad3, eRoundMode.CEIL)
								Solutions(J).High = ConvertAngle(Rad4, eRoundMode.FLOOR)
								Solutions(J).Tag = 1
							End If
						End If
					Else
						Rad1 = Modulus(RWYTHRPrj.M + Theta1, 360.0)
						Rad2 = Modulus(RWYTHRPrj.M - Theta1, 360.0)
						Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad1)
						Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad2)

						If FinalNav.TypeCode = eNavaidType.LLZ Then
							If AngleInSector(FinalNav.pPtGeo.M, Rad1, Rad2) Then
								J = UBound(Solutions)
								If J < 0 Then
									ReDim Solutions(0)
									J = 0
								Else
									J += 1
									ReDim Preserve Solutions(J)
								End If
								Solutions(J).Low = Modulus(FinalNav.pPtGeo.M)
								Solutions(J).High = Solutions(J).Low
								Solutions(J).Tag = 1
							Else
								ItemStr = My.Resources.str10214
								Replace(ItemStr, Chr(10), " ")
								itmX = ListView0101.Items.Add(ItemStr)

								ItemStr = My.Resources.str00303
								Replace(ItemStr, Chr(10), " ")
								itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
								itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ItemStr))
							End If
						ElseIf SubtractAnglesWithSign(ConvertAngle(Rad1, eRoundMode.CEIL), ConvertAngle(Rad2, eRoundMode.FLOOR), 1) >= 0.0 Then
							J = UBound(Solutions)
							If J < 0 Then
								ReDim Solutions(1)
								J = 0
							Else
								J = J + 1
								ReDim Preserve Solutions(J)
							End If

							Solutions(J).Low = ConvertAngle(Rad1, eRoundMode.CEIL)
							Solutions(J).High = ConvertAngle(Rad2, eRoundMode.FLOOR)
							Solutions(J).Tag = 1
						End If
					End If
				End If
			End If

			N = UBound(Solutions)

			For I = 1 To N
				If AngleInSector(Solutions(0).Low, Solutions(I).Low, Solutions(I).High) Then
					Solutions(I).High = Solutions(0).Low
				ElseIf AngleInSector(Solutions(0).High, Solutions(I).Low, Solutions(I).High) Then
					Solutions(I).Low = Solutions(0).High
				End If
			Next I

			ItemStr = My.Resources.str10215
			Replace(ItemStr, Chr(10), " ")
			Prestr(0) = ItemStr
			ItemStr = My.Resources.str10214
			Replace(ItemStr, Chr(10), " ")
			Prestr(1) = ItemStr

			If (N >= 0) Then
				If Solutions(0).Tag = 0 Then
					If ListView0101.Items.Count = 0 Then
						itmX = ListView0101.Items.Add(Prestr(0))
					Else
						itmX = ListView0101.Items.Insert(1, Prestr(0))
					End If

					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(0).Low)) + "°"))
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(0).High)) + "°"))
					J = 1
				Else
					J = 0
				End If

				For I = J To N
					itmX = ListView0101.Items.Add(Prestr(Solutions(I).Tag))
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(I).Low)) + "°"))
					itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(I).High)) + "°"))
				Next I
			End If
		Else '=========По кругу
			If _OnAero Then
				itmX = ListView0101.Items.Add(My.Resources.str10221)

				ReDim Solutions(0)
				Solutions(0).Low = 0.0
				Solutions(0).High = 359.0
				Solutions(0).Tag = 0
			Else
				ReDim Solutions(5)
				RadToAirport = ReturnAngleInDegrees(FinalNav.pPtPrj, _pCentroid)

				Solutions(0).Low = Dir2Azt(FinalNav.pPtPrj, RadToAirport)
				Solutions(0).High = Solutions(0).Low
				Solutions(0).Tag = 0

				Solutions(1).Low = Modulus(Solutions(0).Low + 180.0, 360.0)
				Solutions(1).High = Solutions(1).Low
				Solutions(1).Tag = 0

				itmX = ListView0101.Items.Add(My.Resources.str10218)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(0).Low)) + "°"))

				itmX = ListView0101.Items.Add(My.Resources.str10218)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(1).Low)) + "°"))

				dRad1 = 0.0
				dRad2 = 0.0
				dRad3 = 0.0
				dRad4 = 0.0

				For I = 0 To RWYCollection.PointCount - 1
					Side1 = SideDef(FinalNav.pPtPrj, RadToAirport, RWYCollection.Point(I))
					Dist1 = ReturnDistanceInMeters(FinalNav.pPtPrj, RWYCollection.Point(I))

					fTmp1 = OnAeroRange / Dist1
					fTmp = 180.0 * System.Math.Atan(fTmp1 / System.Math.Sqrt(-fTmp1 * fTmp1 + 1)) / PI
					fTmp1 = ReturnAngleInDegrees(FinalNav.pPtPrj, RWYCollection.Point(I))

					If Side1 < 0 Then
						dRad = SubtractAngles(RadToAirport, fTmp1)
						If dRad > dRad1 Then
							dRad1 = dRad
							Rad1 = fTmp1
						End If
					Else
						dRad = SubtractAngles(RadToAirport, fTmp1)
						If dRad > dRad2 Then
							dRad2 = dRad
							Rad2 = fTmp1
						End If
					End If

					dRad = SubtractAngles(RadToAirport, fTmp1 + fTmp)
					If dRad > dRad3 Then
						dRad3 = dRad
						Rad3 = fTmp1 + fTmp
					End If

					dRad = SubtractAngles(RadToAirport, fTmp1 - fTmp)
					If dRad > dRad4 Then
						dRad4 = dRad
						Rad4 = fTmp1 - fTmp
					End If
				Next I

				Solutions(2).Low = Dir2Azt(FinalNav.pPtPrj, Rad1)
				Solutions(2).High = Dir2Azt(FinalNav.pPtPrj, Rad2)
				If Solutions(2).Low > Solutions(2).High Then
					Solutions(2).Low = Dir2Azt(FinalNav.pPtPrj, Rad1)
					Solutions(2).High = Dir2Azt(FinalNav.pPtPrj, Rad2)
				End If
				Solutions(2).Tag = 1

				Solutions(3).Low = Modulus(Solutions(2).Low + 180.0, 360.0)
				Solutions(3).High = Modulus(Solutions(2).High + 180.0, 360.0)
				Solutions(3).Tag = 1

				itmX = ListView0101.Items.Add(My.Resources.str10219)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(2).Low)) + "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(2).High)) + "°"))

				itmX = ListView0101.Items.Add(My.Resources.str10219)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(3).Low)) + "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(3).High)) + "°"))

				'        fTmp = OnAeroRange / Dist1
				'        Rad1 = Rad1 + 180.0 * Atn(fTmp / Sqr(-fTmp * fTmp + 1)) / PI

				'        fTmp = OnAeroRange / Dist2
				'        Rad2 = Rad2 - 180.0 * Atn(fTmp / Sqr(-fTmp * fTmp + 1)) / PI

				Rad1 = Dir2Azt(FinalNav.pPtPrj, Rad3)
				Rad2 = Dir2Azt(FinalNav.pPtPrj, Rad4)

				Rad3 = Modulus(Rad1 + 180.0, 360.0)
				Rad4 = Modulus(Rad2 + 180.0, 360.0)

				Solutions(4).Low = Rad1
				Solutions(4).High = Rad2
				Solutions(4).Tag = 2

				Solutions(5).Low = Rad3
				Solutions(5).High = Rad4
				Solutions(5).Tag = 2

				itmX = ListView0101.Items.Add(My.Resources.str10220)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(4).Low)) + "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(4).High)) + "°"))

				itmX = ListView0101.Items.Add(My.Resources.str10220)
				itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(5).Low)) + "°"))
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(Solutions(5).High)) + "°"))
			End If
		End If

		'===================================
		NextBtn.Enabled = UBound(Solutions) >= 0
	End Sub

	Private Sub TextBox0104_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0104.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0104_Validating(TextBox0104, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0104.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0104_Validating(ByVal eventSender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox0104.Validating
		Dim lSs As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox0104.Text) Then Return
		lSs = CDbl(TextBox0104.Text)
		fTmp = lSs

		If lSs < arSemiSpan.Values(Category) Then lSs = arSemiSpan.Values(Category)
		If lSs > 60.0 Then lSs = 60.0
		If lSs <> fTmp Then TextBox0104.Text = CStr(lSs)
	End Sub

	Private Sub TextBox0105_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0105.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0105_Validating(TextBox0105, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(e.KeyChar)
		End If

		If e.KeyChar = Chr(0) Then
			e.Handled = True
		End If
	End Sub

	Private Sub TextBox0105_Validating(ByVal eventSender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox0105.Validating
		Dim lVs As Double
		Dim fTmp As Double
		If Not IsNumeric(TextBox0105.Text) Then Return
		lVs = CDbl(TextBox0105.Text)
		fTmp = lVs

		If lVs < arVerticalSize.Values(Category) Then lVs = arVerticalSize.Values(Category)
		If lVs > 10.0 Then lVs = 10.0
		If lVs <> fTmp Then TextBox0105.Text = CStr(lVs)
	End Sub

#End Region

#Region "Page 03"
	Private Sub CraeteNSubtrahends()
		If Not OptionButton0102.Checked Then Exit Sub

		Dim I As Integer
		Dim J As Integer

		Dim pt0 As IPoint
		Dim pt1 As IPoint
		Dim ptTmp As IPoint
		Dim ptCentr As IPoint

		Dim pRing As IRing
		Dim pLeftPoly As IPolygon
		Dim pRightPoly As IPolygon
		Dim pPrimePoly As IPolygon
		Dim pRWYSubtrahend As IPolygon
		Dim pTmpPoly As IPointCollection
		Dim pRWYPoly As IPointCollection
		Dim pSubtrahend2 As IPointCollection

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IGeometryCollection
		Dim pPolygons As IGeometryCollection
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		Dim LengthOfInnerEdge As Double
		Const DistanceFromThreshold As Double = 60.0
		Const Divergence As Double = 0.15
		Dim DivergenceAngle As Double = RadToDeg(Math.Atan(Divergence))

		pRWYSubtrahend = New ESRI.ArcGIS.Geometry.Polygon

		For I = 0 To UBound(RWYDATA)
			If RWYDATA(I).Selected Then
				ptCentr = RWYDATA(I).pPtPrj(eRWY.PtTHR)

				If RWYDATA(I).Index = 0 Then
					LengthOfInnerEdge = 150.0
				Else
					LengthOfInnerEdge = 300.0
				End If

				pRWYPoly = New ESRI.ArcGIS.Geometry.Polygon
				pt0 = LocalToPrj(ptCentr, ptCentr.M, -DistanceFromThreshold, LengthOfInnerEdge)
				pt1 = LocalToPrj(ptCentr, ptCentr.M, -DistanceFromThreshold, -LengthOfInnerEdge)

				pRWYPoly.AddPoint(pt1)
				pRWYPoly.AddPoint(pt0)

				ptTmp = LocalToPrj(pt1, ptCentr.M - DivergenceAngle, -50000, 0.0)
				pRWYPoly.AddPoint(ptTmp)

				ptTmp = LocalToPrj(pt0, ptCentr.M + DivergenceAngle, -50000, 0.0)
				pRWYPoly.AddPoint(ptTmp)

				pTopo = pRWYPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pRWYSubtrahend = pTopo.Union(pRWYSubtrahend)
			End If
		Next I

		pClone = pPolyPrime
		pPrimePoly = pClone.Clone
		pTopo = pPrimePoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pClone = pPolyLeft
		pLeftPoly = pClone.Clone()
		pTopo = pLeftPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSubtrahend2 = pTopo.Union(pPrimePoly)
		pTopo = pSubtrahend2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pClone = pPolyRight
		pRightPoly = pClone.Clone()
		pTopo = pRightPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSubtrahend2 = pTopo.Union(pSubtrahend2)
		pTopo = pSubtrahend2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pRWYSubtrahend
		pSubtrahend2 = pTopo.Union(pSubtrahend2)
		pTopo = pSubtrahend2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = ConvexPoly
		pTmpPoly = pTopo.Buffer(0.5)
		pTopo = pTmpPoly
		pPolygons = pTopo.Difference(pSubtrahend2)

		ListView0202.Items.Clear()

		pSubtrahend.Clear()

		For Each elem As ArcGIS.Carto.IElement In pSubtrahendElem
			SafeDeleteElement(elem)
		Next elem

		pSubtrahendElem.Clear()

		pTopo = ConvexPoly
		J = 1
		For I = 0 To pPolygons.GeometryCount - 1
			pRing = pPolygons.Geometry(I)
			If Not pRing.IsExterior Then Continue For

			pPolygon = New ESRI.ArcGIS.Geometry.Polygon()
			pPolygon.AddGeometry(pRing)
			pSubtrahend.Add(pPolygon)

			pSubtrahendElem.Add(Nothing)

			ListView0202.Items.Add(New ListViewItem("Sector " + J.ToString()))
			J += 1
		Next
	End Sub

	Private Function CalcNewVsMOC(ByVal Bank As Double, ByVal cIx As Integer, ByRef OIx As Integer) As Double
		Dim I As Integer
		Dim Result As Double
		Dim pPolyElement As ArcGIS.Carto.IElement
		Dim pPolygon As IPolygon
		Dim pTmpPoly As IPolygon
		Dim pTopo As ITopologicalOperator2
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		'=============================================================

		pClone = ConvexPoly
		VsArea = pClone.Clone

		For Each elem As ArcGIS.Carto.IElement In pSubtrahendElem
			SafeDeleteElement(elem)
		Next elem

		For I = 0 To pSubtrahend.Count - 1
			pPolygon = pSubtrahend(I)

			Dim bContains As Boolean
			Try
				bContains = ListView0202.CheckedIndices.Contains(I)
			Catch ex As Exception
				bContains = False
			End Try

			If bContains Then
				pTopo = VsArea
				VsArea = pTopo.Difference(pPolygon)
				pSubtrahendElem.Item(I) = Nothing
			Else
				pTopo = ConvexPoly
				pTmpPoly = pTopo.Intersect(pPolygon, esriGeometryDimension.esriGeometry2Dimension)

				pPolyElement = DrawPolygon(pTmpPoly, RGB(192, 192, 192), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
				pPolyElement.Locked = True
				pSubtrahendElem.Item(I) = pPolyElement
			End If
		Next

		Result = MaxObstacleHeightInPoly(ObstacleList, MSAObstacleList, arObsClearance.Values(cIx), VsArea, OIx)
		Result = Result + arObsClearance.Values(cIx)

		If Result < arMinOCH.Values(cIx) Then
			Result = arMinOCH.Values(cIx)
			OIx = -1
		End If
		NonPrecReportFrm.FillPage01(MSAObstacleList, arObsClearance.Values(cIx), OIx)

		SafeDeleteElement(VisualAreaElement)

		VisualAreaElement = DrawPolygon(VsArea, RGB(0, 0, 255), , False)
		'If VisualAreaState Then
		pGraphics.AddElement(VisualAreaElement, 0)
		VisualAreaElement.Locked = True
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'End If

		Return Result
	End Function

	Private Sub UpDateNavaidZone(ByVal NewAzt As Double)
		Dim FA_Range As Double
		Dim fDist As Double
		Dim MaxDh As Double
		Dim Dist As Double

		Dim K As Double

		Dim pReleation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint

		Dim pPoly As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pPath As ESRI.ArcGIS.Geometry.IPath

		Dim Side0 As Integer

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		pTmpPoint = New ESRI.ArcGIS.Geometry.Point

		If FinalNav.TypeCode = eNavaidType.LLZ Then
			_Label0201_3.Visible = False
			_Label0201_10.Visible = False
			_Label0201_15.Visible = False

			SpinButton0201.SelectedItem = FinalNav.pPtGeo.M
			SpinButton0201.Text = CStr(Math.Round(FinalNav.pPtGeo.M, 2))

			_ArDir = FinalNav.pPtPrj.M
			'FictTHR = PointAlongPlane(FinalNav.pPtPrj, FinalNav.pPtPrj.M + 180.0, FinalNav.LLZ_THR)
			FictTHR = New ESRI.ArcGIS.Geometry.Point
			pConstruct = FictTHR
			pConstruct.ConstructAngleIntersection(RWYTHRPrj, DegToRad(FinalNav.pPtPrj.M + 90.0), FinalNav.pPtPrj, DegToRad(FinalNav.pPtPrj.M))

			If OptionButton0101.Checked Then
				FictTHR.Z = RWYTHRPrj.Z
			Else
				FictTHR.Z = CurrADHP.pPtGeo.Z
			End If
		Else
			_Label0201_3.Visible = True
			_Label0201_10.Visible = True
			_Label0201_15.Visible = True

			'SpinButton0201.SelectedItem = Modulus(NewAzt)
			SpinButton0201.SelectedItem = Modulus(ConvertAngle(NewAzt))
			SpinButton0201.Text = CStr(Modulus(ConvertAngle(NewAzt)))

			_ArDir = Azt2Dir(FinalNav.pPtGeo, NewAzt)

			If OptionButton0101.Checked Then
				If SubtractAngles(RWYTHRPrj.M, _ArDir) < 1.0 Then
					FictTHR = New ESRI.ArcGIS.Geometry.Point
					pConstruct = FictTHR
					pConstruct.ConstructAngleIntersection(RWYTHRPrj, DegToRad(_ArDir + 90.0), FinalNav.pPtPrj, DegToRad(_ArDir))
				Else
					pConstruct = pTmpPoint
					pConstruct.ConstructAngleIntersection(RWYTHRPrj, DegToRad(RWYTHRPrj.M), FinalNav.pPtPrj, DegToRad(_ArDir))

					Dist = ReturnDistanceInMeters(RWYTHRPrj, pTmpPoint)
					Side0 = SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, pTmpPoint)
					FictTHR = PointAlongPlane(pTmpPoint, _ArDir + 90.0 * (Side0 - 1), Dist)
				End If
				FictTHR.Z = RWYTHRPrj.Z
			Else
				pLine = New ESRI.ArcGIS.Geometry.Polyline
				pPoly = pLine
				N = UBound(RWYDATA)

				For I = 0 To N Step 2
					If RWYDATA(I).Selected Then
						pPath = New ESRI.ArcGIS.Geometry.Path
						pPath.FromPoint = RWYDATA(I).pPtPrj(eRWY.PtTHR)
						pPath.ToPoint = RWYDATA(I + 1).pPtPrj(eRWY.PtTHR)
						pPoly.AddGeometry(pPath)
					End If
				Next I

				pLine1 = New ESRI.ArcGIS.Geometry.Polyline
				pLine1.FromPoint = PointAlongPlane(FinalNav.pPtPrj, _ArDir, 400000.0)
				pLine1.ToPoint = PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0, 400000.0)
				pReleation = pLine

				If pReleation.Crosses(pLine1) Then
					pTopo = pLine
					pPoints = pTopo.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)

					pProxi = pLine1.ToPoint
					MaxDh = 900000.0
					J = -1

					For I = 0 To pPoints.PointCount - 1
						fDist = pProxi.ReturnDistance(pPoints.Point(I))
						'fTmp = Azt2Dir(CurrADHP.pPtGeo, pPoints.Point(I).M)
						If fDist < MaxDh Then 'And (SubtractAngles(fTmp + 180.0, ArDir) <= 90.0)
							J = I
							MaxDh = fDist
						End If
					Next I

					If J >= 0 Then
						FictTHR = pPoints.Point(J)
					ElseIf pPoints.PointCount > 0 Then
						FictTHR = pPoints.Point(0)
					Else
						FictTHR = New ESRI.ArcGIS.Geometry.Point
					End If
				Else
					pProxi = pLine1
					pPoints = pLine
					MaxDh = 90000.0
					J = -1
					For I = 0 To pPoints.PointCount - 1
						fDist = pProxi.ReturnDistance(pPoints.Point(I))
						'fTmp = pPoints.Point(I).M 'Azt2Dir(CurrADHP.pPtGeo, pPoints.Point(I).M)
						'If (fDist < MaxDh) And (SubtractAngles(fTmp + 180.0, m_ArDir) <= 90.0) Then
						If fDist < MaxDh Then
							J = I
							MaxDh = fDist
						End If
					Next I

					FictTHR = New ESRI.ArcGIS.Geometry.Point
					If J >= 0 Then
						ptTmp = pPoints.Point(J)
						pConstruct = FictTHR
						pConstruct.ConstructAngleIntersection(FinalNav.pPtPrj, DegToRad(_ArDir), ptTmp, DegToRad(_ArDir + 90.0))
					Else
						FictTHR.PutCoords(pPoints.Point(0).X, pPoints.Point(0).Y)
					End If
				End If

				_Label0201_10.Text = CStr(ConvertHeight(ReturnDistanceInMeters(FictTHR, FinalNav.pPtPrj), eRoundMode.NEAREST))
				FictTHR.Z = CurrADHP.pPtGeo.Z
			End If
		End If
		'================================================================
		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			FA_Range = VOR.FA_Range
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			FA_Range = NDB.FA_Range
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			FA_Range = LLZ.Range
		End If
		K = 2 * FA_Range

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pLine.AddPoint(FinalNav.pPtPrj)
		pLine.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0, FA_Range))

		Ss = CDbl(TextBox0104.Text)
		Vs = CDbl(TextBox0105.Text)

		CreateNavaidZone(FinalNav, _ArDir, FictTHR, Ss, Vs, FA_Range, K, pPolyLeft, pPolyRight, pPolyPrime)

		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		'Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection

		SafeDeleteElement(MAPtElem)
		SafeDeleteElement(FASegmElement)
		SafeDeleteElement(LeftPolyElement)
		SafeDeleteElement(RightPolyElement)
		SafeDeleteElement(PrimePolyElement)

		'On Error Resume Next
		pClone = pPolyLeft
		pTmpPoly = pClone.Clone
		pTopo = pTmpPoly
		pTopo.Simplify()
		LeftPolyElement = DrawPolygon(pTmpPoly, RGB(0, 0, 255))

		pTopo = pPolyPrime
		pTopo.Simplify()
		'Set pClone = pPolyPrime

		NavGuadArea2 = pTopo.Union(pTmpPoly)

		pClone = pPolyRight
		pTmpPoly = pClone.Clone
		pTopo = pTmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		RightPolyElement = DrawPolygon(pTmpPoly, RGB(0, 0, 255))

		pTopo = NavGuadArea2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		NavGuadArea2 = pTopo.Union(pTmpPoly)

		pTopo = NavGuadArea2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pLine.AddPoint(FinalNav.pPtPrj)
		pLine.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0, FA_Range))

		PrimePolyElement = DrawPolygon(pPolyPrime, 0)
		FASegmElement = DrawPolyLine(pLine, RGB(0, 0, 255))

		MAPtElem = DrawPointWithText(FictTHR, "MAPt", WPTColor)

		FASegmElement.Locked = True
		LeftPolyElement.Locked = True
		RightPolyElement.Locked = True
		PrimePolyElement.Locked = True
		MAPtElem.Locked = True

		'On Error GoTo 0
	End Sub

	Private Sub AzimuthChanged()
		Dim CurrTheta As Double
		Dim FA_Range As Double
		Dim NomGrad As Double
		Dim fTurnR As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim Dist As Double
		Dim Bank As Double
		Dim lTAS As Double
		Dim fTmp As Double
		Dim K As Double

		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pTmpPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1400 As ESRI.ArcGIS.Geometry.IPoint

		Dim Dushdu As Boolean
		Dim Side1 As Integer
		Dim cIx As Integer
		Dim aStr(2) As String

		aStr(0) = My.Resources.str10315
		aStr(1) = ""
		aStr(2) = My.Resources.str10314

		fAlignOCH = 0
		Bank = CDbl(TextBox0002.Text)

		If OptionButton0101.Checked Then
			CurrTheta = SubtractAngles(RWYTHRPrj.M, _ArDir)
			_Label0201_9.Text = CStr(ConvertAngle(CurrTheta))
			pPt1400 = PointAlongPlane(RWYTHRPrj, RWYTHRPrj.M + 180.0, arMinInterDist.Value)

			pTmpPoint = New ESRI.ArcGIS.Geometry.Point
			pConstruct = pTmpPoint
			pConstruct.ConstructAngleIntersection(pPt1400, DegToRad(RWYTHRPrj.M + 90.0), FinalNav.pPtPrj, DegToRad(_ArDir))
			Dist1 = ReturnDistanceInMeters(pPt1400, pTmpPoint)

			_Label0201_10.Text = CStr(ConvertHeight(Dist1, eRoundMode.NEAREST))
			_Label0201_15.Text = HeightConverter(HeightUnit).Unit + aStr(1 + SideDef(pPt1400, (RWYTHRPrj.M), pTmpPoint))

			Dushdu = Dist1 <= arMinInterToler.Value
			If (CurrTheta <= arStrInAlignment.Value) And Dushdu Then
				Side1 = SideDef(pTmpPoint, _ArDir + 90.0, FinalNav.pPtPrj)
				Dist1 = FA_Range - Side1 * ReturnDistanceInMeters(FinalNav.pPtPrj, pTmpPoint)
				Dist = arMinInterDist.Value + Dist1
				_Label0201_8.Text = "/--/"
				_Label0201_13.Text = ""
			Else
				pConstruct = pTmpPoint
				pConstruct.ConstructAngleIntersection(FinalNav.pPtPrj, DegToRad(_ArDir), SelectedRWY.pPtPrj(eRWY.PtEnd), DegToRad(RWYTHRPrj.M))
				Dist0 = ReturnDistanceInMeters(pTmpPoint, RWYTHRPrj)

				If (Dist0 > arMaxRangeFAS.Value) Or (Dist0 < arMinInterDist.Value) Then
					CreateNavaidZone(FinalNav, _ArDir, FictTHR, Ss, Vs, FA_Range + Dist0, K, pPolyLeft, pPolyRight, pPolyPrime)
				End If

				Side1 = SideDef(pTmpPoint, _ArDir + 90.0, FinalNav.pPtPrj)
				Dist = ReturnDistanceInMeters(FinalNav.pPtPrj, pTmpPoint)

				'If Dist >= FA_Range Then
				'	'                Label26.Caption = "Tочка сопряжения находится за пределом обеспечиваемом наведением:"
				'	Dist0 = Dist
				'Else
				Dist1 = FA_Range - Side1 * ReturnDistanceInMeters(FinalNav.pPtPrj, pTmpPoint)
				Dist = Dist1
				'End If
				_Label0201_8.Text = CStr(ConvertDistance(Dist0, eRoundMode.NEAREST))
			End If

			'If Dist0 < FA_Range Then
			'	Label26.Caption = "Максимальная длина конечного участка, обеспечиваемая наведением:"
			'End If
			'Label26.Width = 3500
			'Label27.Caption = CStr(Round(Dist, 1))

			NomGrad = arFADescent_Nom.Value

			cIx = ComboBox0001.SelectedIndex
			lTAS = IAS2TAS(3.6 * cVfafMax.Values(cIx), fRefHeight, CurrADHP.ISAtC)
			'fTurnR = Bank2Radius(Bank, lTAS + 3.6 * arVisualWS.Value )
			fTurnR = Bank2Radius(Bank, lTAS + 18.52)

			If (CurrTheta <= arStrInAlignment.Value) And Dushdu Then
				If OptionButton0201.Checked Then
					fMinOCH = arFASeg_FAF_MOC.Value
				Else
					fMinOCH = arFASegmentMOC.Value
				End If

				_Label0201_12.Text = CStr(ConvertHeight(fMinOCH, eRoundMode.NEAREST))
				_Label0201_12.ForeColor = System.Drawing.Color.Black
			Else
				fTmp = arMaxInterAngle.Values(Category)
				If CurrTheta < arMaxInterAngle.Values(3) Then fTmp = arMaxInterAngle.Values(3)
				fAlignOCH = arAbv_Treshold.Value + (Dist0 + fTurnR * System.Math.Tan(DegToRad(0.5 * fTmp)) + 5.0 * 0.27777777777777779 * (lTAS + 18.52)) * NomGrad

				fMinOCH = fAlignOCH
				_Label0201_12.Text = CStr(ConvertHeight(fMinOCH, eRoundMode.NEAREST))

				If fVisAprOCH <= fMinOCH Then
					_Label0201_12.ForeColor = Color.Red
				Else
					_Label0201_12.ForeColor = Color.Black
				End If
			End If
		Else
			Dim OIx As Integer
			fNewVisAprOCH = CalcNewVsMOC(Bank, Category, OIx)
			fMinOCH = fNewVisAprOCH
			_Label0201_11.Text = CStr(ConvertHeight(fNewVisAprOCH, eRoundMode.NEAREST))
		End If
	End Sub

	Private Sub ListView0201_ItemChecked(ByVal sender As Object, ByVal eventArgs As ItemCheckedEventArgs) Handles ListView0201.ItemChecked
		Static ListView0201InUse As Boolean = False

		If Not bFormInitialised Then Return
		If ListView0201InUse Then Return

		Dim Item As System.Windows.Forms.ListViewItem = eventArgs.Item
		Dim N As Integer
		Dim K As Integer
		Dim i As Integer
		Dim j As Integer
		Dim Imin As Integer
		Dim Optima As Double

		Dim fI As Double
		Dim fJ As Double
		Dim fTmp As Double
		Dim fLow As Double
		Dim RWYAzt As Double
		Dim resInt As Interval

		If InSectList(Item.Index).Tag = 1 Then
			Item.Checked = True
			Return
		End If

		If Not Item.Tag Then
			ListView0201InUse = True
			Item.Checked = False
			ListView0201InUse = False
			Return
		End If

		K = ComboBox0201.SelectedIndex
		If K < 0 Then Return

		N = UBound(InSectList)

		For i = 0 To N
			ListView0201.Items.Item(i).Tag = True
			ListView0201.Items.Item(i).Font = New Font(ListView0201.Items.Item(i).Font, FontStyle.Bold)
			ListView0201.Items.Item(i).SubItems.Item(1).Font = ListView0201.Items.Item(i).Font
			ListView0201.Items.Item(i).SubItems.Item(2).Font = ListView0201.Items.Item(i).Font
			ListView0201.Items.Item(i).SubItems.Item(3).Font = ListView0201.Items.Item(i).Font
			ListView0201.Items.Item(i).ForeColor = Color.Black
			ListView0201.Items.Item(i).SubItems.Item(1).ForeColor = Color.Black
			ListView0201.Items.Item(i).SubItems.Item(2).ForeColor = Color.Black
			ListView0201.Items.Item(i).SubItems.Item(3).ForeColor = Color.Black
		Next i

		resInt.Left = Azt2Dir(FinalNav.pPtGeo, Solutions(K).High)
		resInt.Right = Azt2Dir(FinalNav.pPtGeo, Solutions(K).Low)

		j = 0

		For i = 0 To N
			If (InSectList(i).Tag = 0) And ListView0201.Items.Item(i).Checked Then
				j = j + 1
				If AngleInSector(InSectList(i).FromAngle, resInt.Left, resInt.Right) Then
					resInt.Left = InSectList(i).FromAngle
				End If

				If AngleInSector(InSectList(i).ToAngle, resInt.Left, resInt.Right) Then
					resInt.Right = InSectList(i).ToAngle
				End If
			End If
		Next i

		'If j > 0 Then
		For i = 0 To N
			If Not ((i = Item.Index) Or ListView0201.Items.Item(i).Checked Or (InSectList(i).Tag = 1)) Then
				If (AnglesSideDef(InSectList(i).ToAngle, resInt.Left) < 0) Or (AnglesSideDef(InSectList(i).FromAngle, resInt.Right) > 0) Then
					ListView0201.Items.Item(i).Tag = False
					ListView0201.Items.Item(i).Font = New Font(ListView0201.Items.Item(i).Font, FontStyle.Regular)
					ListView0201.Items.Item(i).SubItems.Item(1).Font = ListView0201.Items.Item(i).Font
					ListView0201.Items.Item(i).SubItems.Item(2).Font = ListView0201.Items.Item(i).Font
					ListView0201.Items.Item(i).SubItems.Item(3).Font = ListView0201.Items.Item(i).Font
					ListView0201.Items.Item(i).ForeColor = Color.FromArgb(&H8F8F8F)
					ListView0201.Items.Item(i).SubItems.Item(1).ForeColor = Color.FromArgb(&H8F8F8F)
					ListView0201.Items.Item(i).SubItems.Item(2).ForeColor = Color.FromArgb(&H8F8F8F)
					ListView0201.Items.Item(i).SubItems.Item(3).ForeColor = Color.FromArgb(&H8F8F8F)
				End If
			End If
		Next i
		'End If

		'Label1411.Caption = CStr(Round(Dir2Azt(FinalNav.pPtPrj, resInt.Right), 3)) + _
		''              "-" + CStr(Round(Dir2Azt(FinalNav.pPtPrj, resInt.Left), 3))

		fI = Dir2Azt(FinalNav.pPtPrj, resInt.Right)
		fJ = Dir2Azt(FinalNav.pPtPrj, resInt.Left)
		i = ConvertAngle(fI, eRoundMode.CEIL)
		j = ConvertAngle(fJ, eRoundMode.FLOOR)

		If Modulus(fJ - fI, 360.0) < 1 Then
			If SideFrom2Angle(resInt.Right, 0.0) * SideFrom2Angle(resInt.Left, 0.0) < 0 Then
				If SideFrom2Angle(resInt.Right, 0.0) < 0 Then
					resInt.Right = resInt.Right + 360.0
				End If

				If SideFrom2Angle(resInt.Left, 0.0) < 0 Then
					resInt.Left = resInt.Left + 360.0
				End If
			End If

			fTmp = Dir2Azt(FinalNav.pPtPrj, 0.5 * (resInt.Right + resInt.Left))
			'SpinButton0201.SelectedItem = fTmp '.ToString()
			SpinButton0201.Enabled = False
			SpinButton0201.Text = ConvertAngle(fTmp).ToString()

			ComboBox0201.Tag = "A"
			ComboBox0201.Items(K) = SpinButton0201.Text
			ComboBox0201.Tag = ""

			UpDateNavaidZone(fTmp)
			CraeteNSubtrahends()
			AzimuthChanged()
		Else
			ComboBox0201.Tag = "A"

			'SpinButton0201.Minimum = I
			Imin = i
			Optima = fI

			If fI = fJ Then                          'Solutions(K).Low = Solutions(K).High
				ComboBox0201.Items(K) = CStr(ConvertAngle(fI))
				ComboBox0201.Tag = ""

				FillDomainControl(SpinButton0201, fI, fJ)

				SpinButton0201.ReadOnly = True
				SpinButton0201.BackColor = System.Drawing.SystemColors.ButtonFace
				'SpinButton0201.Maximum = j
			Else
				ComboBox0201.Items(K) = CStr(ConvertAngle(fI)) + "-" + CStr(ConvertAngle(fJ))
				ComboBox0201.Tag = ""

				If fI >= fJ Then j = j + 360

				FillDomainControl(SpinButton0201, fI, fJ)

				SpinButton0201.ReadOnly = False
				SpinButton0201.BackColor = System.Drawing.SystemColors.Window
				'SpinButton0201.Maximum = j

				RWYAzt = Dir2Azt(FinalNav.pPtPrj, RWYTHRPrj.M)

				fLow = SubtractAngles(RWYAzt, Optima)

				For i = Imin To j
					fTmp = SubtractAngles(RWYAzt, i)
					If fTmp < fLow Then
						fLow = fTmp
						Optima = Modulus(i)
					End If
				Next i
			End If

			SpinButton0201.Enabled = True
			Optima = Math.Round(Optima)             '???????????????????????????????
			SpinButton0201.SelectedItem = Optima

			UpDateNavaidZone(Optima)
			CraeteNSubtrahends()
			AzimuthChanged()

			'If SpinButton0201.SelectedItem = Optima Then
			'	AzimuthChanged(Optima)
			'Else
			'	SpinButton0201.SelectedItem = Optima
			'	If SpinButton0201.SelectedItem Is Nothing Then
			'		AzimuthChanged(Optima)
			'	End If
			'End If
		End If
	End Sub

	Private Sub SpinButton0201_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles SpinButton0201.KeyPress
		If e.KeyChar = Chr(13) Then
			If Not CBool(SpinButton0201.Tag) Then Return
			If Not SpinButton0201.SelectedItem Is Nothing Then Return
			If Not IsNumeric(SpinButton0201.Text) Then Return

			e.Handled = True

			Dim fTmp As Double = CDbl(SpinButton0201.Text)

			UpDateNavaidZone(fTmp)
			CraeteNSubtrahends()
			AzimuthChanged()
		End If
	End Sub

	Private Sub SpinButton0201_SelectedItemChanged(sender As System.Object, e As System.EventArgs) Handles SpinButton0201.SelectedItemChanged
		If Not bFormInitialised Then Return
		If Not CBool(SpinButton0201.Tag) Then Return

		If Not SpinButton0201.SelectedItem Is Nothing Then
			Dim fTmp As Double = SpinButton0201.SelectedItem
			UpDateNavaidZone(fTmp)
			CraeteNSubtrahends()
			AzimuthChanged()
		End If
	End Sub

	Private Sub ListView0202_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView0202.ItemChecked
		AzimuthChanged()
	End Sub

	Private Sub ListView0202_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView0202.SelectedIndexChanged
		Dim I As Integer
		Dim pTmpPoly As IPolygon
		Dim pPolyElement As ArcGIS.Carto.IElement
		Dim pTopo As ITopologicalOperator = ConvexPoly

		For I = 0 To pSubtrahend.Count - 1
			Dim bContains As Boolean
			Try
				bContains = ListView0202.CheckedIndices.Contains(I)
			Catch ex As Exception
				bContains = False
			End Try

			If bContains Then Continue For

			pTmpPoly = pTopo.Intersect(pSubtrahend(I), esriGeometryDimension.esriGeometry2Dimension)

			SafeDeleteElement(pSubtrahendElem.Item(I))

			If ListView0202.SelectedIndices.Contains(I) Then
				pPolyElement = DrawPolygon(pTmpPoly, RGB(255, 192, 192), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			Else
				pPolyElement = DrawPolygon(pTmpPoly, RGB(192, 192, 192), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			End If

			pPolyElement.Locked = True
			pSubtrahendElem.Item(I) = pPolyElement
		Next
	End Sub

	Private Sub OptionButton0201_CheckedChanged(eventSender As System.Object, eventArgs As System.EventArgs) Handles OptionButton0201.CheckedChanged, OptionButton0202.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		If eventSender Is OptionButton0201 Then
			Frame0701.Text = My.Resources.str10802          '"Участок FAF-SDF"
			_Label0701_22.Text = My.Resources.str10804      '"Расстояние FAF-SDF:"
		Else
			Frame0701.Text = My.Resources.str10803          '"Участок SIL-SDF"
			_Label0701_22.Text = My.Resources.str10805      '"Расстояние SIL-SDF:"

			'	ComboBox0703.SelectedIndex = 0
			'	SpinButton0201_SelectedItemChanged(SpinButton0201, New EventArgs())
			'MAPtDef = 0
		End If

		ComboBox0703.SelectedIndex = 0

		If SpinButton0201.Items.Count > 0 Then
			Dim prevIndex As Integer = SpinButton0201.SelectedIndex
			SpinButton0201.SelectedIndex = 0
			If prevIndex = 0 Then
				SpinButton0201_SelectedItemChanged(SpinButton0201, New EventArgs())
			End If
		End If

		'If SpinButton0201.SelectedIndex < 0 Then
		'	If SpinButton0201.Items.Count > 0 Then SpinButton0201.SelectedIndex = 0
		'Else
		'	Dim prevIndex As Integer = SpinButton0201.SelectedIndex
		'	SpinButton0201.SelectedIndex = 0
		'	SpinButton0201_SelectedItemChanged(SpinButton0201, New EventArgs())
		'End If

	End Sub

	Private Sub ComboBox0201_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0201.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		Dim Optima As Double
		Dim fTmp As Double
		Dim fLow As Double
		Dim fHigh As Double
		Dim RWYAzt As Double

		Dim itmX As System.Windows.Forms.ListViewItem
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pSector As ESRI.ArcGIS.Geometry.IPointCollection

		K = ComboBox0201.SelectedIndex
		If (K < 0) Or (ComboBox0201.Tag = "A") Then Return
		ComboBox0201.Tag = "A"

		N = UBound(Solutions)

		For I = 0 To N
			If Solutions(I).Low = Solutions(I).High Then
				ComboBox0201.Items(I) = CStr(ConvertAngle(Solutions(I).Low))
			Else
				ComboBox0201.Items(I) = CStr(ConvertAngle(Solutions(I).Low)) + "-" + CStr(ConvertAngle(Solutions(I).High))
			End If
		Next I

		ComboBox0201.Tag = ""

		SpinButton0201.Enabled = True
		'SpinButton0201.Minimum = Solutions(K).Low

		Optima = Solutions(K).Low
		If Solutions(K).Low <> Solutions(K).High Then
			RWYAzt = Dir2Azt(FinalNav.pPtPrj, RWYTHRPrj.M)
			fLow = SubtractAngles(RWYAzt, Optima)

			If Solutions(K).Low > Solutions(K).High Then
				For I = Solutions(K).Low To Solutions(K).High + 360.0
					fTmp = SubtractAngles(RWYAzt, I)
					If fTmp < fLow Then
						fLow = fTmp
						Optima = I
					End If
				Next I
			Else
				For I = Solutions(K).Low To Solutions(K).High
					fTmp = SubtractAngles(RWYAzt, I)
					If fTmp < fLow Then
						fLow = fTmp
						Optima = I
					End If
				Next I
			End If
		End If

		If OptionButton0101.Checked Then
			Select Case Solutions(K).Tag
				Case 0
					_Label0201_7.Text = My.Resources.str10228   'My.Resources.str10215
				Case 1
					_Label0201_7.Text = My.Resources.str10227   'My.Resources.str10214
			End Select
		Else
			If _OnAero Then
				_Label0201_7.Text = ""
			Else
				Select Case Solutions(K).Tag
					Case 0
						_Label0201_7.Text = My.Resources.str10218
					Case 1
						_Label0201_7.Text = My.Resources.str10219
					Case 2
						_Label0201_7.Text = My.Resources.str10220
				End Select
			End If
		End If

		SpinButton0201.Tag = False

		'SpinButton0201.Minimum = Solutions(K).Low
		'If Solutions(K).Low <= Solutions(K).High Then
		'	SpinButton0201.Maximum = Solutions(K).High
		'Else
		'	SpinButton0201.Maximum = Solutions(K).High + 360.0
		'End If

		FillDomainControl(SpinButton0201, Solutions(K).Low, IIf(Solutions(K).Low <= Solutions(K).High, Solutions(K).High, Solutions(K).High + 360.0))

		UpDateNavaidZone(Optima)
		CraeteNSubtrahends()
		AzimuthChanged()
		SpinButton0201.SelectedItem = Modulus(Math.Round(Optima))

		SpinButton0201.Tag = True
		'SpinButton0201.Refresh()

		'If (SpinButton0201.SelectedIndex < SpinButton0201.Items.Count) And (SpinButton0201.SelectedItem = Math.Round(Optima)) Then
		'	AzimuthChanged(Optima)
		'Else
		'SpinButton0201.SelectedItem = Optima
		'If SpinButton0201.SelectedItem Is Nothing Then
		'AzimuthChanged(Optima)
		'End If

		'If SpinButton0201.Value = Optima Then
		'	SpinButton0201_Validating(SpinButton0201, New EventArgs())
		'Else
		'	SpinButton0201.Value = Optima
		'End If

		If Solutions(K).Low = Solutions(K).High Then
			SpinButton0201.ReadOnly = True
			SpinButton0201.BackColor = System.Drawing.SystemColors.Control
		Else
			SpinButton0201.ReadOnly = False
			SpinButton0201.BackColor = System.Drawing.SystemColors.Window
		End If

		If SubtractAngles(Solutions(K).Low, Solutions(K).High) < 1.0 Then
			fHigh = Azt2Dir(FinalNav.pPtGeo, Solutions(K).High + 0.5 * LLZ.TrackingTolerance)
			fLow = Azt2Dir(FinalNav.pPtGeo, Solutions(K).Low - 0.5 * LLZ.TrackingTolerance)
		Else
			fHigh = Azt2Dir(FinalNav.pPtGeo, Solutions(K).High)
			fLow = Azt2Dir(FinalNav.pPtGeo, Solutions(K).Low)
		End If

		If _OnAero And OptionButton0102.Checked Then
			pSector = CreatePrjCircle(FinalNav.pPtPrj, 10.0 * RModel)
		Else
			pSector = New ESRI.ArcGIS.Geometry.Polygon
			pSector.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fHigh, 10.0 * RModel))
			pSector.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fLow, 10.0 * RModel))
			pSector.AddPoint(FinalNav.pPtPrj)
			pSector.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fLow + 180.0, 10.0 * RModel))
			pSector.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fHigh + 180.0, 10.0 * RModel))
			pSector.AddPoint(FinalNav.pPtPrj)

			pTopo = pSector
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If
		'DrawPolygon pSector, 0

		'NavInSector(FinalNav, NavaidList, InSectList, pSector)
		InSectList = WPTInSector(FinalNav, WPTList, pSector)

		N = UBound(InSectList)
		ListView0201.Items.Clear()

		For I = 0 To N
			itmX = ListView0201.Items.Add(InSectList(I).CallSign)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ConvertDistance(InSectList(I).Distance, eRoundMode.NEAREST).ToString()))

			If InSectList(I).Tag = 1 Then
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "0"))
				itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "360"))
				itmX.Checked = True
			ElseIf InSectList(I).Tag = -1 Then
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "-"))
				itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "-"))
			Else
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ConvertAngle(Dir2Azt(FinalNav.pPtPrj, InSectList(I).ToAngle)).ToString()))
				itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ConvertAngle(Dir2Azt(FinalNav.pPtPrj, InSectList(I).FromAngle)).ToString()))
			End If

			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, GetNavTypeName(InSectList(I).TypeCode)))

			itmX.Tag = True
			itmX.Font = New Font(itmX.Font, FontStyle.Bold)

			itmX.SubItems.Item(1).Font = New Font(itmX.SubItems.Item(1).Font, FontStyle.Bold)
			itmX.SubItems.Item(2).Font = New Font(itmX.SubItems.Item(2).Font, FontStyle.Bold)
			itmX.SubItems.Item(3).Font = New Font(itmX.SubItems.Item(3).Font, FontStyle.Bold)
		Next I
	End Sub

#End Region

#Region "Page 04"

	Private Sub ListView0301_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView0301.ItemChecked
		Static ListView0301InUse As Boolean = False

		If Not bFormInitialised Then Return

		If ListView0301InUse Then Return
		ListView0301InUse = True

		Dim I As Integer
		Dim N As Integer
		Dim Item As System.Windows.Forms.ListViewItem = e.Item
		Dim itmX As System.Windows.Forms.ListViewItem

		Try
			If Item.Checked Then
				If Item.SubItems.Count < 2 Then Return
				fNearDist = DeConvertDistance(CDbl(Item.Text))
				fFarDist = DeConvertDistance(CDbl(Item.SubItems(1).Text))

				N = ListView0301.Items.Count
				For I = 0 To N - 1
					If I <> Item.Index Then
						itmX = ListView0301.Items.Item(I)
						itmX.Checked = False
					End If
				Next I
			Else
				For I = 0 To N - 1
					If I <> Item.Index Then
						itmX = ListView0301.Items.Item(I)
						If itmX.Checked Then Return
					End If
				Next I
				ListView0301InUse = False
				Item.Checked = True
			End If
		Finally
			ListView0301InUse = False
		End Try
	End Sub

	Private Sub ListView0301_Click(sender As System.Object, e As System.EventArgs) Handles ListView0301.Click
		If DirectCast(sender, ListView).SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = DirectCast(sender, ListView).SelectedItems.Item(0)

		Dim fFar As Double
		Dim fNear As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pLineTmp As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon

		If Item Is Nothing Then Return

		fNear = DeConvertDistance(CDbl(Item.Text))
		fFar = DeConvertDistance(CDbl(Item.SubItems(1).Text))

		pClone = IntermediateBaseArea
		pIFPoly = pClone.Clone

		pLineTmp = New ESRI.ArcGIS.Geometry.Polyline

		ptTmp = PointAlongPlane(PtFAF, _ArDir + 180.0, fNear)
		pLineTmp.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 2.0 * RModel)
		pLineTmp.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 2.0 * RModel)

		ClipByLine(pIFPoly, pLineTmp, Nothing, pTmpPoly, Nothing)
		'Set pTmpPoly =
		'CutPoly pIFPoly, pLineTmp, -1

		If Not pTmpPoly.IsEmpty() Then pIFPoly = pTmpPoly

		ptTmp = PointAlongPlane(PtFAF, _ArDir + 180.0, fFar)
		pLineTmp.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 2.0 * RModel)
		pLineTmp.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 2.0 * RModel)

		'DrawPoint ptFAF, 255
		'DrawPolyLine pLineTmp, 255
		'DrawPoint pLineTmp.FromPoint, 0
		'DrawPoint pLineTmp.ToPoint, 255

		ClipByLine(pIFPoly, pLineTmp, pTmpPoly, Nothing, Nothing)

		If Not pTmpPoly.IsEmpty() Then pIFPoly = pTmpPoly

		'Set pIFPoly =
		'CutPoly pIFPoly, pLineTmp, 1
		On Error Resume Next
		If Not IFPolyElement Is Nothing Then pGraphics.DeleteElement(IFPolyElement)
		IFPolyElement = DrawPolygon(pIFPoly, RGB(192, 192, 192))
		IFPolyElement.Locked = True
		On Error GoTo 0
	End Sub

	Private Sub CheckBox0301_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0301.CheckedChanged
		ComboBox0305.Enabled = CheckBox0301.Checked
		TextBox0301.ReadOnly = CheckBox0301.Checked
		TextBox0303.ReadOnly = CheckBox0301.Checked

		If CheckBox0301.Checked Then
			TextBox0301.BackColor = SystemColors.ButtonFace
			TextBox0303.BackColor = SystemColors.ButtonFace
			ComboBox0305_SelectedIndexChanged(ComboBox0305, New System.EventArgs())
		Else
			TextBox0301.BackColor = SystemColors.Window
			TextBox0303.BackColor = SystemColors.Window
		End If
	End Sub

	Private Sub ComboBox0301_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0301.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fFAFRange As Double
		arMinISlen = MinISlensArray(ComboBox0301.SelectedIndex)
		TextBox0307.Text = CStr(ConvertDistance(arMinISlen, eRoundMode.NEAREST))
		fFAFRange = DeConvertDistance(CDbl(TextBox0301.Text))
		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())
	End Sub

	Private Sub ComboBox0302_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0302.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim fAppHeight As Double
		Dim fFAFRange As Double
		Dim fPlane15h As Double
		Dim fMaxDist As Double
		Dim fDist As Double
		Dim fReqH As Double
		Dim fL As Double

		Dim IxO As Integer
		Dim Ix As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer
		Dim M As Integer

		If ComboBox0302.Tag <> "" Then Return
		I = ComboBox0302.SelectedIndex
		If I < 0 Then Return

		ComboBox0302.Tag = "I"
		'CheckBox0301 = FAFNavDat(I).ValCnt = -2
		_Label0301_10.Text = GetNavTypeName(FAFNavDat(I).TypeCode)

		If FAFNavDat(I).IntersectionType = eIntersectionType.OnNavaid Then
			'_Label0301_4.Text = My.Resources.str106	 '"Над средством"
			L = InSectListIx(I)
			fFAFRange = Point2LineDistancePrj(FAFNavDat(I).pPtPrj, FictTHR, _ArDir - 90.0)  ' * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, FAFNavDat(i).pPtGeo)

			Caller(CurrOCH, fFAFRange)
			ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())

			For I = 0 To ComboBox0302.Items.Count - 1
				If InSectListIx(I) = L Then
					ComboBox0302.SelectedIndex = I
					Exit For
				End If
			Next I
		End If

		CreateFinalBaseArea()
		ComboBox0302.Tag = ""

		fMaxDist = 0.0

		For I = 0 To FinalBaseArea.PointCount - 1
			fDist = Point2LineDistancePrj(FictTHR, FinalBaseArea.Point(I), _ArDir + 90.0)
			If fDist > fMaxDist Then
				fMaxDist = fDist
				Ix = I
			End If
		Next I

		N = UBound(FAFObstacleList.Parts)
		M = UBound(FAFObstacleList.Obstacles)
		If N >= 0 Then
			ReDim FAFWorkObstacleList.Parts(N)
			ReDim FAFWorkObstacleList.Obstacles(M)
		Else
			ReDim FAFWorkObstacleList.Parts(-1)
			ReDim FAFWorkObstacleList.Obstacles(-1)
		End If

		For I = 0 To M
			FAFObstacleList.Obstacles(I).NIx = -1
		Next

		fAppHeight = PtFAF.Z - arISegmentMOC.Value
		CurrOCH = fMinOCH
		fL = fAppHeight / arFixMaxIgnorGrd.Value
		pRelational = FinalBaseArea

		IxO = -1
		IxWorkJO = -1
		J = -1
		K = -1

		For I = 0 To N
			If Not pRelational.Contains(FAFObstacleList.Parts(I).pPtPrj) Then Continue For

			fDist = Point2LineDistancePrj(FAFObstacleList.Parts(I).pPtPrj, FinalBaseArea.Point(Ix), _ArDir + 90.0)
			If fDist < arFIX15PlaneRang.Value Then
				fPlane15h = arFixMaxIgnorGrd.Value * (fL - fDist)
				If fPlane15h > FAFObstacleList.Parts(I).Height Then
					fReqH = 0.0
					FAFObstacleList.Parts(I).Flags = FAFObstacleList.Parts(I).Flags Or 2
				Else
					fReqH = FAFObstacleList.Parts(I).ReqH
					FAFObstacleList.Parts(I).Flags = FAFObstacleList.Parts(I).Flags And 1
				End If
			Else
				fReqH = FAFObstacleList.Parts(I).ReqH
				FAFObstacleList.Parts(I).Flags = FAFObstacleList.Parts(I).Flags And 1
			End If

			J = J + 1
			FAFWorkObstacleList.Parts(J) = FAFObstacleList.Parts(I)

			If FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner).NIx < 0 Then
				K += 1
				FAFWorkObstacleList.Obstacles(K) = FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner)
				FAFWorkObstacleList.Obstacles(K).PartsNum = 0
				ReDim FAFWorkObstacleList.Obstacles(K).Parts(FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner).PartsNum - 1)
				FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner).NIx = K
			End If

			FAFWorkObstacleList.Parts(J).Owner = FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner).NIx
			FAFWorkObstacleList.Obstacles(FAFWorkObstacleList.Parts(J).Owner).Parts(FAFWorkObstacleList.Obstacles(FAFWorkObstacleList.Parts(J).Owner).PartsNum) = J
			FAFWorkObstacleList.Obstacles(FAFWorkObstacleList.Parts(J).Owner).PartsNum += 1

			If fReqH > CurrOCH Then
				CurrOCH = fReqH
				IxO = I
				IxWorkJO = J
			End If
		Next I

		If K >= 0 Then
			ReDim Preserve FAFWorkObstacleList.Obstacles(K)
		Else
			ReDim FAFWorkObstacleList.Obstacles(-1)
		End If

		If J >= 0 Then
			ReDim Preserve FAFWorkObstacleList.Parts(J)
		Else
			ReDim FAFWorkObstacleList.Parts(-1)
		End If

		_FinalOCH = CurrOCH
		ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())

		NextObstInfo(IxO)
		NonPrecReportFrm.FillPage03(FAFWorkObstacleList, IxWorkJO, True)

		SDFDominikObstacle = -1

		TextBox0305.Text = "--"
		TextBox0304.Text = "--"

		If IxO >= 0 Then
			TextBox0305.Text = CStr(ConvertDistance(FAFObstacleList.Parts(IxO).Dist, eRoundMode.NEAREST))
			TextBox0304.Text = FAFObstacleList.Obstacles(FAFObstacleList.Parts(IxO).Owner).UnicalName
			SDFDominikObstacle = IxO
		End If
	End Sub

	Private Sub ComboBox0303_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0303.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		If Not IsNumeric(TextBox0303.Tag) Then Return
		fTmp = CDbl(TextBox0303.Tag)

		If ComboBox0303.SelectedIndex = 0 Then
			TextBox0303.Text = CStr(ConvertHeight(fTmp))
		Else
			TextBox0303.Text = CStr(ConvertHeight(fTmp - fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0304_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0304.SelectedIndexChanged
		If ComboBox0304.SelectedIndex = 0 Then
			TextBox0302.Text = CStr(ConvertHeight(_FinalOCH))
		Else
			TextBox0302.Text = CStr(ConvertHeight(_FinalOCH + fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0305_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0305.SelectedIndexChanged
		If Not bFormInitialised Then Return

		If Not CheckBox0301.Checked Then Return
		If ComboBox0305.SelectedIndex < 0 Then Return
		If ComboBox0305.Tag <> "" Then Return

		Dim fFAFRange As Double
		Dim WPT As NavaidData

		WPT = ComboBox0305.SelectedItem
		ComboBox0305.Tag = "I"

		fFAFRange = Point2LineDistancePrj(WPT.pPtPrj, FictTHR, _ArDir - 90.0)   ' * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, FAFNavDat(i).pPtGeo)
		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())
		ComboBox0305.Tag = ""
	End Sub

	Private Sub TextBox0301_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0301.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0301_Validating(TextBox0301, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0301.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0301_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0301.Validating
		Dim fFAFRange As Double
		If Not IsNumeric(TextBox0301.Text) Then Return
		'If Not TextBox0301.Enabled Then Return
		If TextBox0301.ReadOnly Then Return

		fFAFRange = DeConvertDistance(CDbl(TextBox0301.Text))
		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())
	End Sub

	Private Sub TextBox0303_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0303.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0303_Validating(TextBox0303, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0303.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0303_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0303.Validating
		Dim fFAFRange As Double
		Dim fRDH As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox0303.Text) Then Return
		'If Not TextBox0303.Enabled Then Return
		If TextBox0303.ReadOnly Then Return

		fTmp = DeConvertHeight(CDbl(TextBox0303.Text))
		If ComboBox0303.SelectedIndex <> 0 Then fTmp = fTmp + fRefHeight

		'    If IsNumeric(TextBox0303.Tag) Then
		'        If fTmp = CDbl(TextBox0303.Tag) Then Return
		'    End If
		CurrhFAF = fTmp - fRefHeight
		'=====================================================

		fRDH = GetRDH()

		If (CurrhFAF < fMinOCH) And OptionButton0101.Checked Then CurrhFAF = fMinOCH

		Dim fHorRange As Double

		If OptionButton0102.Checked Then
			fHorRange = DeConvertDistance(CDbl(TextBox0308.Text))
		Else
			fHorRange = 0.0
		End If

		fFAFRange = (CurrhFAF - fRDH) / _CurrPDG + fHorRange

		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())
	End Sub

	Private Sub TextBox0306_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0306.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0306_Validating(TextBox0306, Nothing)
		Else
			TextBoxFloat(e.KeyChar, TextBox0306.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0306_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0306.Validating
		Dim fFAFRange As Double
		If Not IsNumeric(TextBox0306.Text) Then Return

		_CurrPDG = 0.01 * CDbl(TextBox0306.Text)
		If _CurrPDG < arFADescent_Min.Value Then
			_CurrPDG = arFADescent_Min.Value
			TextBox0306.Text = CStr(100.0 * _CurrPDG)
		ElseIf _CurrPDG > arFADescent_Max.Values(Category) Then
			_CurrPDG = arFADescent_Max.Values(Category)
			TextBox0306.Text = CStr(100.0 * _CurrPDG)
		End If

		FinalAreaPDG = _CurrPDG
		'    CheckBox0301.Value = False
		fFAFRange = DeConvertDistance(CDbl(TextBox0301.Text))
		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, Nothing)
	End Sub

	Private Sub TextBox0308_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0308.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0308_Validating(Nothing, Nothing)
		Else
			TextBoxFloat(e.KeyChar, TextBox0308.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0308_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0308.Validating
		If Not TextBox0308.Visible Then Return
		If Not IsNumeric(TextBox0308.Text) Then Return

		'Dim fHorRange As Double
		'fHorRange = DeConvertDistance(CDbl(TextBox0308.Text))

		'Dim fMaxDist As Double = (fNewVisAprOCH - arAbv_Treshold.Value) / _CurrPDG
		'Dim fMinDist As Double = arISegmentMOC.Value / _CurrPDG

		'If fHorRange > fMaxDist Then
		'	fHorRange = fMaxDist
		'	TextBox0308.Text = ConvertDistance(fHorRange).ToString()
		'End If

		Dim fFAFRange As Double = DeConvertDistance(CDbl(TextBox0301.Text))
		Caller(CurrOCH, fFAFRange)
		ComboBox0304_SelectedIndexChanged(ComboBox0304, Nothing)
	End Sub

#End Region

#Region "Page 05"
	Private Sub OptionButton0401_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0401.CheckedChanged, OptionButton0402.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		TextBox0403.Tag = "A"
		TextBox0403_Validating(TextBox0403, Nothing)
	End Sub

	Private Sub CheckBox0401_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0401.CheckedChanged
		ComboBox0403.Enabled = CheckBox0401.Checked
		TextBox0403.ReadOnly = CheckBox0401.Checked

		If CheckBox0401.Checked Then
			TextBox0403.BackColor = SystemColors.ButtonFace
			ComboBox0403_SelectedIndexChanged(ComboBox0403, New System.EventArgs())
		Else
			TextBox0403.BackColor = SystemColors.Window
		End If
	End Sub

	Private Sub ComboBox0401_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0401.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim pConstructor As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim tipStr As String
		Dim FafStr As String
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint
		Dim fTmp As Double
		Dim Kmin As Integer
		Dim Kmax As Integer
		Dim I As Integer
		Dim N As Integer
		Dim K As Integer

		K = ComboBox0401.SelectedIndex
		If K < 0 Then Return

		label0401_08.Text = GetNavTypeName(IFNavDat(K).TypeCode)
		FafStr = My.Resources.str10415

		If IFNavDat(K).IntersectionType = eIntersectionType.ByDistance Then
			label0401_09.Visible = True
			label0401_14.Visible = True
			TextBox0407.Visible = True

			N = UBound(IFNavDat(K).ValMin)

			OptionButton0401.Enabled = N > 0
			OptionButton0402.Enabled = N > 0

			TextBox0403.Visible = True
			If OptionButton0402.Checked Or (N = 0) Then
				TextBox0403.Text = CStr(ConvertDistance(IFNavDat(K).ValMin(0) - IFNavDat(K).Disp, eRoundMode.NEAREST))
			Else
				TextBox0403.Text = CStr(ConvertDistance(IFNavDat(K).ValMin(1) - IFNavDat(K).Disp, eRoundMode.NEAREST))
			End If

			If N = 0 Then
				If IFNavDat(K).ValCnt > 0 Then
					OptionButton0401.Checked = True
				Else
					OptionButton0402.Checked = True
				End If
			End If
			label0401_03.Text = My.Resources.str00229
			tipStr = My.Resources.str10514 + vbCrLf

			For I = N To 0 Step -1
				If ((N = 0) And (IFNavDat(K).ValCnt <= 0)) Or ((N <> 0) And (I > 0)) Then
					CircleVectorIntersect(IFNavDat(K).pPtPrj, IFNavDat(K).ValMax(I), PtFAF, _ArDir, ptMin)
					CircleVectorIntersect(IFNavDat(K).pPtPrj, IFNavDat(K).ValMin(I), PtFAF, _ArDir, ptMax)
				Else
					CircleVectorIntersect(IFNavDat(K).pPtPrj, IFNavDat(K).ValMin(I), PtFAF, _ArDir + 180.0, ptMin)
					CircleVectorIntersect(IFNavDat(K).pPtPrj, IFNavDat(K).ValMax(I), PtFAF, _ArDir + 180.0, ptMax)
				End If

				fIF2FAFMin = ReturnDistanceInMeters(ptMin, PtFAF)
				fIF2FAFMax = ReturnDistanceInMeters(ptMax, PtFAF)
				FafStr = FafStr + My.Resources.str00221 + CStr(ConvertDistance(fIF2FAFMin, 3)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(fIF2FAFMax, 1)) + " " + DistanceConverter(DistanceUnit).Unit

				tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(IFNavDat(K).ValMin(I) - IFNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(IFNavDat(K).ValMax(I) - IFNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
				If I > 0 Then
					tipStr = tipStr + vbCrLf
					FafStr = FafStr + " "
				End If
			Next I
			label0401_13.Text = DistanceConverter(DistanceUnit).Unit
		Else
			label0401_09.Visible = False
			label0401_14.Visible = False
			TextBox0407.Visible = False

			OptionButton0401.Enabled = False
			OptionButton0402.Enabled = False
			OptionButton0401.Checked = False
			OptionButton0402.Checked = False

			If IFNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
				TextBox0403.Visible = False '            TextBox0403.Text = ""
				tipStr = ""
				label0401_03.Text = My.Resources.str00106
				label0401_13.Text = ""
				FafStr = ""
			Else
				TextBox0403.Visible = True
				label0401_13.Text = "°"

				ptMin = New ESRI.ArcGIS.Geometry.Point
				pConstructor = ptMin
				fIF2FAFMin = Azt2Dir(IFNavDat(K).pPtGeo, IFNavDat(K).ValMin(0))
				pConstructor.ConstructAngleIntersection(PtFAF, DegToRad(_ArDir), IFNavDat(K).pPtPrj, DegToRad(fIF2FAFMin))

				ptMax = New ESRI.ArcGIS.Geometry.Point
				pConstructor = ptMax
				fIF2FAFMax = Azt2Dir(IFNavDat(K).pPtGeo, IFNavDat(K).ValMax(0))
				pConstructor.ConstructAngleIntersection(PtFAF, DegToRad(_ArDir), IFNavDat(K).pPtPrj, DegToRad(fIF2FAFMax))

				If IFNavDat(K).TypeCode = eNavaidType.NDB Then
					label0401_03.Text = My.Resources.str00228
					Kmax = Modulus(IFNavDat(K).ValMax(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
					Kmin = Modulus(IFNavDat(K).ValMin(0) + 180.0 - IFNavDat(K).MagVar, 360.0)
					'                tipStr = My.Resources.str220 + vbCrLf + My.Resources.str221
				Else
					label0401_03.Text = My.Resources.str00227
					Kmax = Modulus(IFNavDat(K).ValMax(0) - IFNavDat(K).MagVar, 360.0)
					Kmin = Modulus(IFNavDat(K).ValMin(0) - IFNavDat(K).MagVar, 360.0)
					'                tipStr = My.Resources.str220 + vbCrLf + My.Resources.str221
				End If
				tipStr = My.Resources.str00220 + vbCrLf + My.Resources.str00221
				fIF2FAFMin = ReturnDistanceInMeters(ptMin, PtFAF)
				fIF2FAFMax = ReturnDistanceInMeters(ptMax, PtFAF)

				If fIF2FAFMin > fIF2FAFMax Then
					fTmp = fIF2FAFMin
					fIF2FAFMin = fIF2FAFMax
					fIF2FAFMax = fTmp
				End If
				FafStr = FafStr + My.Resources.str00221 + CStr(ConvertDistance(fIF2FAFMin, 3)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(fIF2FAFMax, 1)) + " " + DistanceConverter(DistanceUnit).Unit

				If IFNavDat(K).IntersectionType = eIntersectionType.ByAngle Then
					TextBox0403.Text = Kmin.ToString()
				Else
					TextBox0403.Text = Kmax.ToString()
				End If
				tipStr = tipStr + CStr(Kmin) + " °" + My.Resources.str00222 + CStr(Kmax) + " °"
			End If
		End If

		label0401_04.Text = tipStr
		tipStr = Replace(tipStr, vbCrLf, "   ")
		ToolTip1.SetToolTip(TextBox0403, tipStr)
		ToolTip1.SetToolTip(TextBox0401, FafStr)

		If CheckBox0401.Checked Then
			ComboBox0403_SelectedIndexChanged(ComboBox0403, New System.EventArgs())
		Else
			TextBox0403.Tag = "A"
			TextBox0403_Validating(TextBox0403, New CancelEventArgs())
		End If
	End Sub

	Private Sub ComboBox0402_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0402.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		If Not IsNumeric(TextBox0404.Tag) Then Return
		fTmp = CDbl(TextBox0404.Tag)

		If ComboBox0402.SelectedIndex = 0 Then
			TextBox0404.Text = CStr(ConvertHeight(fTmp))
		Else
			TextBox0404.Text = CStr(ConvertHeight(fTmp - fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0403_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0403.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim WPT As NavaidData
		Dim fDist As Double
		Dim fDir As Double
		Dim fAzt As Double
		Dim K As Integer
		Dim N As Integer

		If Not CheckBox0401.Checked Then Return
		If ComboBox0403.SelectedIndex < 0 Then Return

		WPT = ComboBox0403.SelectedItem

		K = ComboBox0401.SelectedIndex
		If K < 0 Then Return

		N = UBound(IFNavDat)
		If N < 0 Then Return

		'Label0401_25.Text = GetNavTypeName(WPT.TypeCode)
		'PtFAF

		If (IFNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (IFNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
			fDist = ReturnDistanceInMeters(IFNavDat(K).pPtPrj, WPT.pPtPrj)
			TextBox0403.Text = CStr(ConvertDistance(fDist - IFNavDat(K).Disp, eRoundMode.NEAREST))
			If SideDef(IFNavDat(K).pPtPrj, _ArDir + 90.0, WPT.pPtPrj) < 0 Then
				OptionButton0401.Checked = True
			Else
				OptionButton0402.Checked = True
			End If
		Else
			fDir = ReturnAngleInDegrees(IFNavDat(K).pPtPrj, WPT.pPtPrj)
			fAzt = Dir2Azt(IFNavDat(K).pPtPrj, fDir)
			If (IFNavDat(K).TypeCode = eNavaidType.NDB) Then
				TextBox0403.Text = Modulus(fAzt + 180.0 - IFNavDat(K).MagVar).ToString("0.0")
			Else
				TextBox0403.Text = Modulus(fAzt - IFNavDat(K).MagVar).ToString("0.0")
			End If
		End If

		TextBox0403.Tag = "-"
		TextBox0403_Validating(TextBox0403, New CancelEventArgs())
	End Sub

	Private Sub TextBox0401_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0401.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0401_Validating(TextBox0401, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0401.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0401_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0401.Validating
		Dim K As Integer
		Dim Side As Integer

		Dim Dist As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		K = ComboBox0401.SelectedIndex
		If (Not IsNumeric(TextBox0401.Text)) Or (K < 0) Then Return
		If TextBox0401.Text = TextBox0401.Tag Then Return

		Dist = DeConvertDistance(CDbl(TextBox0401.Text))

		If Dist < fIF2FAFMin Then
			Dist = fIF2FAFMin
			TextBox0401.Text = CStr(ConvertDistance(Dist, 3))
		ElseIf Dist > fIF2FAFMax Then
			Dist = fIF2FAFMax
			TextBox0401.Text = CStr(ConvertDistance(Dist, 1))
		End If

		ptTmp = PointAlongPlane(PtFAF, _ArDir + 180.0, Dist)

		Dim fTmp As Double
		If IFNavDat(K).IntersectionType = eIntersectionType.ByDistance Then
			fTmp = ReturnDistanceInMeters(ptTmp, IFNavDat(K).pPtPrj)
			Side = SideDef(IFNavDat(K).pPtPrj, _ArDir + 90.0, ptTmp)

			TextBox0403.Text = CStr(ConvertDistance(fTmp - IFNavDat(K).Disp, eRoundMode.NEAREST))
			If Side < 0 Then
				OptionButton0401.Checked = True
			Else
				OptionButton0402.Checked = True
			End If
		Else
			fTmp = Dir2Azt(IFNavDat(K).pPtPrj, ReturnAngleInDegrees(IFNavDat(K).pPtPrj, ptTmp))

			If IFNavDat(K).TypeCode = eNavaidType.NDB Then
				TextBox0403.Text = CStr(ConvertAngle(Modulus(fTmp + 180.0)))
			Else
				TextBox0403.Text = CStr(ConvertAngle(fTmp))
			End If
		End If

		TextBox0403.Tag = ""
		TextBox0403_Validating(TextBox0403, New System.ComponentModel.CancelEventArgs())
		TextBox0401.Tag = TextBox0401.Text
	End Sub

	Private Sub TextBox0402_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0402.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0402_Validating(TextBox0402, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0402.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0402_Validating(ByVal eventSender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox0402.Validating
		Dim pInitialFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInitialPrimPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFullRelat As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pPrimRelat As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim K As Double
		Dim fTmp As Double
		Dim fDist As Double
		Dim fMinHeight As Double
		Dim fObstHeight As Double

		Dim I As Integer
		Dim N As Integer

		If Not IsNumeric(TextBox0402.Text) Then Return
		fDist = DeConvertDistance(CDbl(TextBox0402.Text))

		If fDist < arIFHalfWidth.Value Then
			fDist = arIFHalfWidth.Value
			TextBox0402.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))
		End If

		If fDist > 60000.0 Then
			fDist = 60000.0
			TextBox0402.Text = CStr(ConvertDistance(fDist, eRoundMode.NEAREST))
		End If

		pInitialFullPoly = New ESRI.ArcGIS.Geometry.Polygon
		pInitialPrimPoly = New ESRI.ArcGIS.Geometry.Polygon
		pPtTmp = PointAlongPlane(IFPnt, _ArDir + 180.0, fDist)

		pInitialFullPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir + 90.0, arIFHalfWidth.Value))
		pInitialFullPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir - 90.0, arIFHalfWidth.Value))

		pInitialFullPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir - 90.0, arIFHalfWidth.Value))
		pInitialFullPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir + 90.0, arIFHalfWidth.Value))

		pTopo = pInitialFullPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pInitialPrimPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
		pInitialPrimPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))

		pInitialPrimPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
		pInitialPrimPoly.AddPoint(PointAlongPlane(pPtTmp, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))

		pTopo = pInitialPrimPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pFullRelat = pInitialFullPoly
		pPrimRelat = pInitialPrimPoly

		CreateObstacleParts(ObstacleList, pInitialFullPoly, pInitialPrimPoly)

		N = UBound(ObstacleList.Parts)
		fMinHeight = arIASegmentMOC.Value

		For I = 0 To N
			If pFullRelat.Contains(ObstacleList.Parts(I).pPtPrj) Then
				If pPrimRelat.Contains(ObstacleList.Parts(I).pPtPrj) Then
					K = 1.0
				Else
					K = 2 * (arIFHalfWidth.Value - Point2LineDistancePrj(IFPnt, ObstacleList.Parts(I).pPtPrj, _ArDir)) / arIFHalfWidth.Value
				End If

				fObstHeight = ObstacleList.Parts(I).Height + arIASegmentMOC.Value * K
				If fObstHeight > fMinHeight Then
					fMinHeight = fObstHeight
				End If
			End If
		Next I

		fTmp = ConvertHeight(fMinHeight + fRefHeight, eRoundMode.NONE)
		If System.Math.Abs(HeightConverter(HeightUnit).Multiplier - 1.0 / 0.3048) < mEps Then
			fTmp = 100.0 * (System.Math.Round(0.01 * fTmp + 0.499999))
		Else
			fTmp = 50.0 * (System.Math.Round(0.02 * fTmp + 0.499999))
		End If

		TextBox0406.Text = CStr(fTmp)
	End Sub

	Private Sub TextBox0403_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0403.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0403_Validating(TextBox0403, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0403.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0403_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0403.Validating
		Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pIFFIXPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim Clone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		Dim InterToler As Double
		Dim TrackToler As Double

		Dim hIFFix As Double
		Dim fDirl As Double
		Dim fDis As Double
		Dim hDis As Double
		Dim d0 As Double
		Dim d1 As Double
		Dim d2 As Double
		Dim D As Double

		Dim I As Integer
		Dim N As Integer

		Dim K As Integer

		ptTmpFAF = PtFAF

		K = ComboBox0401.SelectedIndex
		If IFNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			If Not IsNumeric(TextBox0403.Text) Then Return
			If TextBox0403.Tag = TextBox0403.Text Then Return
			fDirl = CDbl(TextBox0403.Text)
		End If

		Try
			If Not IFFIXElem Is Nothing Then pGraphics.DeleteElement(IFFIXElem)
			If Not IntermediateBaseAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateBaseAreaElem)
			If Not IntermediateSecAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateSecAreaElem)
		Catch ex As Exception

		End Try

		Clone = pFullPoly
		If FinalNav.TypeCode > eNavaidType.NONE Then
			If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
				TrackToler = VOR.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
				TrackToler = NDB.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
				TrackToler = LLZ.TrackingTolerance
			End If

			pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, 3.0 * RModel))
		Else
			pPolyClone = Clone.Clone
		End If

		pTopo = pPolyClone
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Select Case IFNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				D = Point2LineDistancePrj(ptTmpFAF, IFNavDat(K).pPtPrj, _ArDir + 90.0)
				Clone = IFNavDat(K).pPtPrj
				IFPnt = Clone.Clone
				IFPnt.M = _ArDir

				hIFFix = D * arImDescent_Max.Value + CurrhFAF + fRefHeight - IFNavDat(K).pPtPrj.Z
				OnNavaidFIXTolerArea(IFNavDat(K), _ArDir, hIFFix, pTmpPoly)

				'If IFNavDat(K).TypeCode = eNavaidType.NDB Then
				'	NDBFIXTolerArea(IFNavDat(K).pPtPrj, m_ArDir, hIFFix, pTmpPoly)
				'Else
				'	VORFIXTolerArea(IFNavDat(K).pPtPrj, m_ArDir, hIFFix, pTmpPoly)
				'End If
				'pTopo = pTmpPoly
				'pTopo.IsKnownSimple_2 = False
				'pTopo.Simplify()
			Case eIntersectionType.ByAngle
				If IFNavDat(K).TypeCode = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance
					fDis = NDB.Range
					fDirl = fDirl + 180.0
				Else
					InterToler = VOR.IntersectingTolerance
					fDis = VOR.Range
				End If
				'            fDis = IFNavDat(K).Range
				fDirl = fDirl + IFNavDat(K).MagVar
				'==============================================================
				d0 = Modulus(IFNavDat(K).ValMax(0) - IFNavDat(K).ValMin(0), 360.0)
				d1 = Modulus(fDirl - IFNavDat(K).ValMin(0), 360.0)
				If d0 < d1 Then
					d1 = Modulus(IFNavDat(K).ValMin(0) - fDirl, 360.0)
					d2 = Modulus(fDirl - IFNavDat(K).ValMax(0), 360.0)
					If d1 < d2 Then
						fDirl = IFNavDat(K).ValMin(0)
					Else
						fDirl = IFNavDat(K).ValMax(0)
					End If

					If IFNavDat(K).TypeCode = eNavaidType.NDB Then
						d0 = Modulus(fDirl + 180.0 - IFNavDat(K).MagVar, 360.0)
					Else
						d0 = Modulus(fDirl - IFNavDat(K).MagVar, 360.0)
					End If
					TextBox0403.Text = CStr(d0)
				End If
				'==============================================================
				fDirl = Azt2Dir(IFNavDat(K).pPtGeo, fDirl)

				pt1 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl + InterToler, fDis)
				pt2 = PointAlongPlane(IFNavDat(K).pPtPrj, fDirl - InterToler, fDis)

				IFPnt = New ESRI.ArcGIS.Geometry.Point
				pConstruct = IFPnt
				pConstruct.ConstructAngleIntersection(IFNavDat(K).pPtPrj, DegToRad(fDirl), ptTmpFAF, DegToRad(_ArDir))
				IFPnt.M = _ArDir

				pSect0 = New ESRI.ArcGIS.Geometry.Polygon
				pSect0.AddPoint(IFNavDat(K).pPtPrj)
				pSect0.AddPoint(pt1)
				pSect0.AddPoint(pt2)
				pSect0.AddPoint(IFNavDat(K).pPtPrj)

				pTopo = pSect0
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			Case eIntersectionType.ByDistance
				fDirl = DeConvertDistance(fDirl) + IFNavDat(K).Disp
				N = UBound(IFNavDat(K).ValMax)

				If (OptionButton0402.Enabled And OptionButton0402.Checked) Or (N = 0) Then
					If (fDirl < IFNavDat(K).ValMin(0)) Or (fDirl > IFNavDat(K).ValMax(0)) Then
						If fDirl < IFNavDat(K).ValMin(0) Then fDirl = IFNavDat(K).ValMin(0)
						If fDirl > IFNavDat(K).ValMax(0) Then fDirl = IFNavDat(K).ValMax(0)
						TextBox0403.Text = CStr(ConvertDistance(fDirl - IFNavDat(K).Disp, eRoundMode.NEAREST))
					End If
				Else
					If (fDirl < IFNavDat(K).ValMin(1)) Or (fDirl > IFNavDat(K).ValMax(1)) Then
						If fDirl < IFNavDat(K).ValMin(1) Then fDirl = IFNavDat(K).ValMin(1)
						If fDirl > IFNavDat(K).ValMax(1) Then fDirl = IFNavDat(K).ValMax(1)
						TextBox0403.Text = CStr(ConvertDistance(fDirl - IFNavDat(K).Disp, eRoundMode.NEAREST))
					End If
				End If

				If (IFNavDat(K).ValCnt < 0) Or (OptionButton0402.Enabled And OptionButton0402.Checked) Then
					CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptTmpFAF, _ArDir, IFPnt)
				Else
					CircleVectorIntersect(IFNavDat(K).pPtPrj, fDirl, ptTmpFAF, _ArDir + 180.0, IFPnt)
				End If

				IFPnt.M = _ArDir

				fDis = Point2LineDistancePrj(IFPnt, ptTmpFAF, _ArDir + 90.0)
				hDis = DeConvertDistance(CDbl(TextBox0409.Text))

				Dim fdH As Double = (fDis - hDis) * IntermAreaPDG + ptTmpFAF.Z + FictTHR.Z - IFNavDat(K).pPtPrj.Z

				d0 = System.Math.Sqrt(fDirl * fDirl + fdH * fdH)
				TextBox0407.Text = CStr(ConvertDistance(d0 - IFNavDat(K).Disp, eRoundMode.NEAREST))

				d0 = d0 * DME.ErrorScalingUp + DME.MinimalError

				D = fDirl + d0
				pSect0 = CreatePrjCircle(IFNavDat(K).pPtPrj, D)

				pCutter = New ESRI.ArcGIS.Geometry.Polyline
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

				If (IFNavDat(K).ValCnt < 0) Or (OptionButton0402.Enabled And OptionButton0402.Checked) Then
					pTopo.Cut(pCutter, pSect1, pSect0)
				Else
					pTopo.Cut(pCutter, pSect0, pSect1)
				End If

				pTopo = pSect0
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				pTmpPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
		End Select

		pTopo = pTmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pIFTolerArea = pTmpPoly
		'===================================================================
		pIFFIXPoly = New ESRI.ArcGIS.Geometry.Polygon
		pIFFIXPoly.AddPoint(pFAFLine.FromPoint)
		pIFFIXPoly.AddPoint(pFAFLine.ToPoint)

		pIFFIXPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir - 90.0, arIFHalfWidth.Value))
		pIFFIXPoly.AddPoint(PointAlongPlane(IFPnt, _ArDir + 90.0, arIFHalfWidth.Value))

		pSect0 = New ESRI.ArcGIS.Geometry.Polygon
		If FinalNav.TypeCode <> eNavaidType.LLZ Then
			pSect0.AddPoint(PointAlongPlane(pFAFLine.ToPoint, _ArDir + 90.0, 0.25 * pFAFLine.Length))
		End If

		pSect0.AddPoint(pIFFIXPoly.Point(1))
		pSect0.AddPoint(pIFFIXPoly.Point(2))
		pSect0.AddPoint(PointAlongPlane(IFPnt, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
		pSect0.AddPoint(pSect0.Point(0))

		pTopo = pSect0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSect1 = New ESRI.ArcGIS.Geometry.Polygon
		If FinalNav.TypeCode <> eNavaidType.LLZ Then
			pSect1.AddPoint(PointAlongPlane(pFAFLine.FromPoint, _ArDir - 90.0, 0.25 * pFAFLine.Length))
		End If
		pSect1.AddPoint(PointAlongPlane(IFPnt, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
		pSect1.AddPoint(pIFFIXPoly.Point(3))
		pSect1.AddPoint(pIFFIXPoly.Point(0))
		pSect1.AddPoint(pSect1.Point(0))

		pTopo = pIFFIXPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		IntermediateBaseArea = pTopo.Union(pTmpPoly)

		pTopo = pSect1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		IntermediateSecondArea = pTopo.Union(pSect0)

		D = Point2LineDistancePrj(ptTmpFAF, IFPnt, _ArDir + 90.0)
		'    hIFFix = D * arImDescent_Max.Value + ptTmpFAF.Z    '+ ptDerPrj.Z
		hDis = DeConvertDistance(CDbl(TextBox0409.Text))
		hIFFix = (D - hDis) * IntermAreaPDG + ptTmpFAF.Z

		'========================================================

		TextBox0404.Tag = CStr(hIFFix + fRefHeight)
		ComboBox0402_SelectedIndexChanged(ComboBox0402, New System.EventArgs()) '    TextBox0404.Text = CStr(ConvertHeight(hIFFix + fRefHeight, eRoundMode.rmNERAEST))
		TextBox0401.Text = CStr(ConvertDistance(D, eRoundMode.NEAREST))

		IFPnt.Z = hIFFix
		IFPnt.M = _ArDir

		IntermediateBaseAreaElem = DrawPolygon(IntermediateBaseArea, 255)
		IntermediateSecAreaElem = DrawPolygon(IntermediateSecondArea, 0)
		IFFIXElem = DrawPointWithText(IFPnt, "IF", WPTColor)

		IntermediateBaseAreaElem.Locked = True
		IntermediateSecAreaElem.Locked = True
		IFFIXElem.Locked = True

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		''        RefreshCommandBar mTool, 128

		'========================================================
		Dim Wfaf As Double
		Dim fIf2FAFDist As Double

		Wfaf = 0.5 * pFAFLine.Length
		fIf2FAFDist = Point2LineDistancePrj(ptTmpFAF, IFPnt, _ArDir + 90.0)

		GetIntermedObstacles(ObstacleList, ImObstacles, IntermediateBaseArea, IntermediateSecondArea, Wfaf, fIf2FAFDist, _ArDir, CurrhFAF, FinalNav.TypeCode, ptTmpFAF, True)

		N = UBound(ImObstacles.Parts)
		_InterMOCH = arISegmentMOC.Value
		K = -1
		For I = 0 To N
			If _InterMOCH < ImObstacles.Parts(I).ReqH Then
				_InterMOCH = ImObstacles.Parts(I).ReqH
				K = I
			End If
		Next I

		If K >= 0 Then
			TextBox0412.Text = ImObstacles.Obstacles(ImObstacles.Parts(K).Owner).UnicalName
		Else
			If OptionButton0101.Checked And (RWYTHRPrj.Z < CurrADHP.pPtGeo.Z - 2.0) Then
				TextBox0412.Text = Resources.str10520
			Else
				TextBox0412.Text = Resources.str10519
			End If
		End If

		TextBox0411.Text = ConvertHeight(_InterMOCH + fRefHeight).ToString()
		TextBox0402_Validating(TextBox0402, New CancelEventArgs())

		NonPrecReportFrm.FillPage04(ImObstacles)
		TextBox0403.Tag = TextBox0403.Text
		'========================================================
		If (ArrivalProfile.PointsNo = 4) Or (ArrivalProfile.PointsNo = 5) Then
			ArrivalProfile.RemovePointByIndex(0)
			ArrivalProfile.RemovePointByIndex(0)
		End If

		fDis = Point2LineDistancePrj(ptTmpFAF, FictTHR, _ArDir + 90.0)

		ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, hIFFix, Modulus(ArAzt - FinalNav.MagVar, 360.0), -IntermAreaPDG, -1, 0)
		If IntermAreaPDG <> 0 Then
			ArrivalProfile.InsertPoint(fIf2FAFDist + fDis - (hIFFix - ptTmpFAF.Z) / IntermAreaPDG, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.FAF, 1)
		Else
			ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.FAF, 1)
		End If
	End Sub

	Private Sub TextBox0404_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0404.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0404_Validating(TextBox0404, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0404.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0404_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0404.Validating
		Dim fTmp As Double
		Dim fDis As Double
		Dim hIFFix As Double
		Dim fIFhMin As Double
		Dim fIFhMax As Double
		Dim fHorDist As Double
		Dim fDistIF2FAF As Double
		Dim fIf2FAFDist As Double

		If Not IsNumeric(TextBox0404.Text) Then Return
		If TextBox0404.Text = TextBox0404.Tag Then Return

		hIFFix = DeConvertHeight(CDbl(TextBox0404.Text))

		If ComboBox0402.SelectedIndex = 0 Then hIFFix = hIFFix - fRefHeight
		fTmp = hIFFix

		fDistIF2FAF = Point2LineDistancePrj(PtFAF, IFPnt, _ArDir + 90.0)

		fIFhMin = PtFAF.Z
		'    fIFhMax = (fIF2FAFMax - fHorDist) * IntermAreaPDG + ptTmpFAF.Z
		fIFhMax = (fDistIF2FAF - arImHorSegLen.Values(Category)) * IntermAreaPDG + PtFAF.Z

		If hIFFix < fIFhMin Then
			hIFFix = fIFhMin
		ElseIf hIFFix > fIFhMax Then
			hIFFix = fIFhMax
		End If

		If fTmp <> hIFFix Then
			TextBox0404.Tag = CStr(hIFFix + fRefHeight)
			If ComboBox0402.SelectedIndex = 0 Then
				TextBox0404.Text = CStr(ConvertHeight(hIFFix + fRefHeight))
			Else
				TextBox0404.Text = CStr(ConvertHeight(hIFFix))
			End If
		End If
		IFPnt.Z = hIFFix

		If IntermAreaPDG <> 0.0 Then
			fHorDist = fDistIF2FAF - (hIFFix - PtFAF.Z) / IntermAreaPDG
		Else
			fHorDist = fDistIF2FAF
		End If

		TextBox0409.Text = CStr(ConvertDistance(fHorDist, eRoundMode.NEAREST))
		TextBox0404.Tag = TextBox0404.Text

		'========================================================
		Dim K As Integer = ComboBox0401.SelectedIndex

		If IFNavDat(K).IntersectionType = eIntersectionType.ByDistance Then

			Dim fDirl As Double = DeConvertDistance(CDbl(TextBox0403.Text)) + IFNavDat(K).Disp

			Dim fdH As Double = IFPnt.Z + FictTHR.Z - IFNavDat(K).pPtPrj.Z

			Dim d0 As Double = System.Math.Sqrt(fDirl * fDirl + fdH * fdH)
			TextBox0407.Text = CStr(ConvertDistance(d0 - IFNavDat(K).Disp, eRoundMode.NEAREST))
		End If
		'========================================================

		fIf2FAFDist = Point2LineDistancePrj(PtFAF, IFPnt, _ArDir + 90.0)

		If (ArrivalProfile.PointsNo = 4) Or (ArrivalProfile.PointsNo = 5) Then
			ArrivalProfile.RemovePointByIndex(0)
			ArrivalProfile.RemovePointByIndex(0)
		End If

		fDis = Point2LineDistancePrj(PtFAF, FictTHR, _ArDir + 90.0)

		'    ArrivalProfile.InsertPoint fIf2FAFDist + fDis, hIFFix, 0
		'    ArrivalProfile.InsertPoint fIf2FAFDist + fDis - (hIFFix - PtFAF.Z) / arImDescent_Max.Value, PtFAF.Z, 1

		ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, hIFFix, Modulus(ArAzt - FinalNav.MagVar, 360.0), -IntermAreaPDG, -1, 0)

		If IntermAreaPDG = 0 Then
			ArrivalProfile.InsertPoint(fIf2FAFDist + fDis, PtFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.FAF, 1)
		Else
			ArrivalProfile.InsertPoint(fIf2FAFDist + fDis - (hIFFix - PtFAF.Z) / IntermAreaPDG, PtFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.FAF, 1)
		End If
	End Sub

	Private Sub TextBox0408_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0408.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0408_Validating(TextBox0408, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0408.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0408_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0408.Validating
		Dim fTmp As Double

		If Not IsNumeric(TextBox0408.Text) Then Return
		If TextBox0408.Text = TextBox0408.Tag Then Return

		IntermAreaPDG = 0.01 * CDbl(TextBox0408.Text)
		fTmp = IntermAreaPDG

		If IntermAreaPDG < 0.0 Then IntermAreaPDG = 0.0
		If IntermAreaPDG > 2 * arImDescent_Max.Value Then IntermAreaPDG = 2 * arImDescent_Max.Value
		If fTmp <> IntermAreaPDG Then TextBox0408.Text = CStr(100.0 * IntermAreaPDG)

		TextBox0403.Tag = ""
		TextBox0403_Validating(TextBox0403, New System.ComponentModel.CancelEventArgs())
		TextBox0408.Tag = TextBox0408.Text
	End Sub

	Private Sub TextBox0409_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0409.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0409_Validating(TextBox0409, Nothing)
		Else
			TextBoxFloat(e.KeyChar, TextBox0409.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0409_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0409.Validating
		Dim fTmp As Double
		Dim hIFFix As Double
		Dim fHorDist As Double
		Dim fDistIF2FAF As Double

		If Not IsNumeric(TextBox0409.Text) Then Return
		fHorDist = DeConvertDistance(CDbl(TextBox0409.Text))
		fTmp = fHorDist
		fDistIF2FAF = Point2LineDistancePrj(PtFAF, IFPnt, _ArDir + 90.0)

		If fHorDist < arImHorSegLen.Values(Category) Then fHorDist = arImHorSegLen.Values(Category)
		If fHorDist > fDistIF2FAF Then fHorDist = fDistIF2FAF
		If fTmp <> fHorDist Then TextBox0409.Text = CStr(ConvertDistance(fHorDist, eRoundMode.NEAREST))

		hIFFix = PtFAF.Z + (fDistIF2FAF - fHorDist) * IntermAreaPDG

		TextBox0404.Tag = CStr(hIFFix + fRefHeight)
		If ComboBox0402.SelectedIndex = 0 Then
			TextBox0404.Text = CStr(ConvertHeight(hIFFix + fRefHeight))
		Else
			TextBox0404.Text = CStr(ConvertHeight(hIFFix))
		End If

		IFPnt.Z = hIFFix
	End Sub

	Private Sub TextBox0410_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0410.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0410_Validating(TextBox0410, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxLimitCount(e.KeyChar, TextBox0410.Text, 5)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0410_Validating(ByVal eventSender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox0410.Validating
		'
	End Sub

#End Region

#Region "Page 06"
	Private Sub ComboBox0501_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0501.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double

		If Not IsNumeric(TextBox0501.Tag) Then Return
		fTmp = CDbl(TextBox0501.Tag)

		If ComboBox0501.SelectedIndex = 0 Then
			TextBox0501.Text = CStr(ConvertHeight(fTmp + fRefHeight))
			_Label0601_1.Text = My.Resources.str10721
			_Label0601_5.Text = My.Resources.str10719
			_Label0601_13.Text = My.Resources.str10717
		Else
			TextBox0501.Text = CStr(ConvertHeight(fTmp))
			_Label0601_1.Text = My.Resources.str10714
			_Label0601_5.Text = My.Resources.str10712
			_Label0601_13.Text = My.Resources.str10711
		End If
	End Sub

	Private Sub TextBox0501_KeyPress(ByVal eventSender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0501.KeyPress
		'If (eventArgs.KeyChar >= "a") And (eventArgs.KeyChar <= "z") Then eventArgs.KeyChar = Chr(Asc(eventArgs.KeyChar) - 32)

		If e.KeyChar = Chr(13) Then
			TextBox0501_Validating(TextBox0501, Nothing)
		Else
			TextBoxFloat(e.KeyChar, TextBox0501.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0501_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0501.Validating
		Dim fTmp As Double
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim LocSectors() As SectorMSA

		If Not IsNumeric(TextBox0501.Text) Then
			MessageBox.Show("IAF " + ComboBox0501.Text + My.Resources.str00302)
			Return
		End If

		fTmp = DeConvertHeight(CDbl(TextBox0501.Text))
		If ComboBox0501.SelectedIndex = 0 Then fTmp = fTmp - fRefHeight

		If fTmp < MinHeight Then
			fTmp = MinHeight
			If ComboBox0501.SelectedIndex = 0 Then
				TextBox0501.Text = CStr(ConvertHeight(MinHeight + fRefHeight))
			Else
				TextBox0501.Text = CStr(ConvertHeight(MinHeight))
			End If
		End If

		TextBox0501.Tag = CStr(fTmp)

		If _bHaveMSA Then
			N = UBound(MSAList(0).Sectors)
			ReDim LocSectors(N)
			J = 0

			fTmp = fTmp + fRefHeight
			For I = 0 To N
				If MSAList(0).Sectors(I).LowerLimit <= fTmp Then
					LocSectors(J) = MSAList(0).Sectors(I)
					J = J + 1
				End If
			Next I

			M = J - 1
			N = 0
			For I = 0 To M
				J = (I + 1) Mod (M + 1)
				If SubtractAngles(LocSectors(I).FromAngle, LocSectors(J).ToAngle) < degEps Then
					LocSectors(J).ToAngle = LocSectors(I).ToAngle
					LocSectors(J).AbsAngle = SubtractAngles(LocSectors(J).FromAngle, LocSectors(J).ToAngle)
					N = N + 1
				End If
			Next I

			For I = 0 To M - N
				LocSectors(I) = LocSectors(I + N)
			Next I

			N = M - N

			'????????????????????????
			If N > 0 Then
				If SubtractAngles(LocSectors(N).FromAngle, LocSectors(0).ToAngle) < degEps Then
					LocSectors(0).FromAngle = LocSectors(I).FromAngle
					LocSectors(0).AbsAngle = SubtractAngles(LocSectors(0).FromAngle, LocSectors(0).ToAngle)
					N = N - 1
				End If
			End If
			'????????????????????????

			If N < 0 Then
				N = 0
				LocSectors(0).FromAngle = 0
				LocSectors(0).ToAngle = 360
			End If

			ReDim Preserve LocSectors(N)

			ListBox0501.Items.Clear()
			For I = 0 To N
				itmX = ListBox0501.Items.Add(CStr(ConvertAngle(LocSectors(I).FromAngle)))
				If itmX.SubItems.Count > 1 Then
					itmX.SubItems(1).Text = CStr(ConvertAngle(LocSectors(I).ToAngle))
				Else
					itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(LocSectors(I).ToAngle))))
				End If
			Next I
		End If
	End Sub

#End Region

#Region "Page 07"

	Private Sub CreateScheme()
		Dim Hhold As Double

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		_initialTAS = IAS2TAS(_initialIAS, Hhold, CurrADHP.ISAtC)
		TextBox0617.Text = ConvertSpeed(_initialTAS / 3.6).ToString()

		'TextBox0616.ReadOnly = (Not CheckBox0602.Checked) Or (Not OptionButton0601.Checked)

		Select Case SchemeType
			Case 0
				OptionButton0601_CheckedChanged(OptionButton0601, Nothing)
			Case 1
				OptionButton0602_CheckedChanged(OptionButton0602, Nothing)
			Case 2
				OptionButton0603_CheckedChanged(OptionButton0603, Nothing)
			Case 3
				OptionButton0604_CheckedChanged(OptionButton0604, Nothing)
		End Select
	End Sub

	Private Sub CheckBox0601_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0601.CheckedChanged, CheckBox0602.CheckedChanged, CheckBox0603.CheckedChanged,
				CheckBox0604.CheckedChanged, ComboBox0601.SelectedIndexChanged, ComboBox0603.SelectedIndexChanged, ComboBox0604.SelectedIndexChanged
		If Not bFormInitialised Then Return
		CreateScheme()
	End Sub

	Private Sub OptionButton0601_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0601.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim Hhold As Double
		Dim fTmp As Double
		Dim T As Double
		Dim Tn As Double
		Dim Turn As Integer
		Dim Ix As Integer
		Dim ptHoldingArea As IPointCollection

		SchemeType = 0

		Frame0605.Text = My.Resources.str10732
		ComboBox0602.Visible = False
		ComboBox0603.Visible = False
		CheckBox0603.Visible = True
		CheckBox0604.Visible = False

		Label0601_30.Visible = True
		Label0601_31.Visible = True
		TextBox0616.Visible = True
		TextBox0616.ReadOnly = Not CheckBox0603.Checked

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		Tn = CDbl(TextBox0602.Text)
		'm_IAS = 3.6 * DeConvertSpeed(CDbl(TextBox0603.Text))

		'ArDir = Azt2Dir(FinalNav.pPtGeo, SpinButton0201.Value)
		Turn = 2 * ComboBox0601.SelectedIndex - 1

		Do
			T = Tn
			'    CreateNavPolys IAS, Tn, Hhold

			Ipodrom(Hhold, T, _ArDir, Turn, _initialIAS, RModel, ptHoldingArea)
			FarFAF = ptHoldingArea.Point(3)
			PtEOL = ptHoldingArea.Point(2)
			PtSOL = ptHoldingArea.Point(1)
			PtTOB = ptHoldingArea.Point(0)

			PtFAF = FarFAF

			CalcArObstaclesMOC(ObstacleList, BuffObstacleList, HoldingArea, BufferedHoldingArea, Ix)
			PrimaryArea = HoldingArea
			BufferedArea = BufferedHoldingArea

			If Ix >= 0 Then
				fTmp = BuffObstacleList.Parts(Ix).ReqH
				TextBox0614.Text = BuffObstacleList.Obstacles(BuffObstacleList.Parts(Ix).Owner).UnicalName
			Else
				fTmp = arIASegmentMOC.Value
				TextBox0614.Text = ""
			End If

			If ComboBox0501.SelectedIndex = 0 Then
				TextBox0608.Text = CStr(ConvertHeight(fTmp + fRefHeight))
			Else
				TextBox0608.Text = CStr(ConvertHeight(fTmp))
			End If

			NonPrecReportFrm.FillPage02(BuffObstacleList, Ix)

			TextBox0602.Text = T.ToString("0.0000")
			PtEOL.Z = FarFAF.Z

			fTmp = ReturnAngleInDegrees(PtSOL, PtEOL)
			TextBox0612.Text = ConvertAngle(Modulus(Dir2Azt(PtSOL, fTmp) - FinalNav.MagVar, 360.0)).ToString()
			TextBox0613.Text = ConvertDistance(ReturnDistanceInMeters(PtSOL, PtEOL), eRoundMode.NEAREST).ToString()

			Tn = CalcNoFAF(T)
		Loop While Tn > T

		Dim dh As Double = Hhold - FinalNav.pPtGeo.Z
		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			Dim cooNav As NavaidData = DMEList(FinalNav.PairNavaidIndex)
			dh = Hhold - cooNav.pPtGeo.Z
		End If

		Dim dd As Double = ReturnDistanceInMeters(FinalNav.pPtPrj, PtEOL)
		fdLimit = Math.Sqrt(dh * dh + dd * dd)
		TextBox0616.Text = ConvertDistance(fdLimit, eRoundMode.NEAREST).ToString()
	End Sub

	Private Sub OptionButton0602_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0602.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		'PK
		Dim Hhold As Double
		Dim fTmp As Double
		Dim T As Double
		Dim Tn As Double

		Dim Turn As Integer
		Dim Ix As Integer
		Dim ptTrack As IPointCollection

		SafeDeleteElement(pShablonElem)

		SchemeType = 1

		Frame0605.Text = My.Resources.str10727
		ComboBox0602.Visible = True
		ComboBox0603.Visible = True

		CheckBox0601.Visible = False
		CheckBox0602.Visible = False
		CheckBox0603.Visible = True
		CheckBox0604.Visible = False

		Label0601_30.Visible = True
		Label0601_31.Visible = True
		TextBox0616.Visible = True
		TextBox0616.ReadOnly = Not CheckBox0603.Checked

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		Tn = CDbl(TextBox0602.Text)

		'm_IAS = 3.6 * DeConvertSpeed(CDbl(TextBox0603.Text))
		'ArDir = Azt2Dir(FinalNav.pPtGeo, SpinButton0201.Value)

		Turn = 2 * ComboBox0601.SelectedIndex - 1
		PtTOB = Nothing

		Do
			T = Tn

			BaseTurn(Hhold, T, _ArDir, 30.0, Turn, _initialIAS, RModel, ptTrack)

			FarFAF = ptTrack.Point(2)
			PtSOL = ptTrack.Point(0)
			PtEOL = ptTrack.Point(1)
			PtFAF = FarFAF

			TextBox0602.Text = T.ToString("0.0000")

			CalcArObstaclesMOC(ObstacleList, BuffObstacleList, BaseTurnArea, BufferedBaseTurnArea, Ix)
			PrimaryArea = BaseTurnArea
			BufferedArea = BufferedBaseTurnArea

			If Ix >= 0 Then
				fTmp = BuffObstacleList.Parts(Ix).ReqH
				TextBox0614.Text = BuffObstacleList.Obstacles(BuffObstacleList.Parts(Ix).Owner).UnicalName
			Else
				fTmp = arIASegmentMOC.Value
				TextBox0614.Text = ""
			End If

			If ComboBox0501.SelectedIndex = 0 Then
				TextBox0608.Text = CStr(ConvertHeight(fTmp + fRefHeight))
			Else
				TextBox0608.Text = CStr(ConvertHeight(fTmp))
			End If

			NonPrecReportFrm.FillPage02(BuffObstacleList, Ix)
			PtEOL.Z = FarFAF.Z

			fTmp = ReturnAngleInDegrees(ptTrack.Point(0), ptTrack.Point(1))
			TextBox0612.Text = CStr(ConvertAngle(Modulus(Dir2Azt(ptTrack.Point(0), fTmp) - FinalNav.MagVar)))
			TextBox0613.Text = CStr(ConvertDistance(ReturnDistanceInMeters(ptTrack.Point(0), ptTrack.Point(1)), eRoundMode.NEAREST))

			Tn = CalcNoFAF(T) 'Ix
		Loop While Tn > T

		Dim dh As Double = Hhold - FinalNav.pPtGeo.Z
		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			Dim cooNav As NavaidData = DMEList(FinalNav.PairNavaidIndex)
			dh = Hhold - cooNav.pPtGeo.Z
		End If

		Dim dd As Double = ReturnDistanceInMeters(FinalNav.pPtPrj, PtEOL)
		fdLimit = Math.Sqrt(dh * dh + dd * dd)
		TextBox0616.Text = ConvertDistance(fdLimit, eRoundMode.NEAREST).ToString()
	End Sub

	Private Sub OptionButton0603_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0603.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim Hhold As Double
		Dim fTmp As Double
		Dim Tn As Double
		Dim T As Double

		Dim Turn As Integer
		Dim Ix As Integer
		Dim ptTurn45_180 As ESRI.ArcGIS.Geometry.IPointCollection

		On Error Resume Next
		If Not (pShablonElem Is Nothing) Then pGraphics.DeleteElement(pShablonElem)
		On Error GoTo 0

		SchemeType = 2

		Frame0605.Text = My.Resources.str10727
		ComboBox0602.Visible = True
		ComboBox0603.Visible = True

		CheckBox0601.Visible = False
		CheckBox0602.Visible = False
		CheckBox0603.Visible = False
		CheckBox0604.Visible = True

		Label0601_30.Visible = False
		Label0601_31.Visible = False
		TextBox0616.Visible = False
		TextBox0616.ReadOnly = True

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		Tn = CDbl(TextBox0602.Text)
		'm_IAS = 3.6 * DeConvertSpeed(CDbl(TextBox0603.Text))
		'ArDir = Azt2Dir(FinalNav.pPtGeo, SpinButton0201.Value)

		Turn = 2 * ComboBox0601.SelectedIndex - 1
		PtTOB = Nothing

		Do
			T = Tn
			'    CreateNavPolys IAS, Tn, Hhold

			Standart45_180(Hhold, T, _ArDir, Turn, _initialIAS, RModel, ptTurn45_180)

			If CheckBox0604.Checked Then
				FarFAF = ptTurn45_180.Point(ptTurn45_180.PointCount - 1)
			Else
				FarFAF = ptTurn45_180.Point(1)
			End If

			PtSOL = ptTurn45_180.Point(0)
			PtEOL = ptTurn45_180.Point(1)
			PtFAF = FarFAF

			CalcArObstaclesMOC(ObstacleList, BuffObstacleList, Standart45_180Area, BufferedStandart45_180Area, Ix)
			PrimaryArea = Standart45_180Area
			BufferedArea = BufferedStandart45_180Area

			If Ix >= 0 Then
				fTmp = BuffObstacleList.Parts(Ix).ReqH
				TextBox0614.Text = BuffObstacleList.Obstacles(BuffObstacleList.Parts(Ix).Owner).UnicalName
			Else
				fTmp = arIASegmentMOC.Value
				TextBox0614.Text = ""
			End If

			If ComboBox0501.SelectedIndex = 0 Then
				TextBox0608.Text = CStr(ConvertHeight(fTmp + fRefHeight))
			Else
				TextBox0608.Text = CStr(ConvertHeight(fTmp))
			End If

			NonPrecReportFrm.FillPage02(BuffObstacleList, Ix)

			TextBox0602.Text = T.ToString("0.0000")

			fTmp = ReturnAngleInDegrees(ptTurn45_180.Point(0), ptTurn45_180.Point(1))
			TextBox0612.Text = CStr(ConvertAngle(Modulus(Dir2Azt(ptTurn45_180.Point(0), fTmp) - FinalNav.MagVar)))
			TextBox0613.Text = CStr(ConvertDistance(ReturnDistanceInMeters(ptTurn45_180.Point(0), ptTurn45_180.Point(1)), eRoundMode.NEAREST))

			Tn = CalcNoFAF(T)
		Loop While Tn > T
		'NonPrecReportFrm.FillPage1(BuffObstacleList, Ix)
	End Sub

	Private Sub OptionButton0604_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0604.CheckedChanged
		If Not bFormInitialised Then Return

		If Not eventSender.Checked Then Return
		Dim Hhold As Double
		Dim fTmp As Double
		Dim T As Double
		Dim Tn As Double

		Dim Turn As Integer
		Dim Ix As Integer
		Dim ptTurn80_260 As ESRI.ArcGIS.Geometry.IPointCollection

		On Error Resume Next
		If Not (pShablonElem Is Nothing) Then pGraphics.DeleteElement(pShablonElem)
		On Error GoTo 0

		SchemeType = 3

		Frame0605.Text = My.Resources.str10727
		ComboBox0602.Visible = True
		ComboBox0603.Visible = True

		CheckBox0601.Visible = False
		CheckBox0602.Visible = False
		CheckBox0603.Visible = False
		CheckBox0604.Visible = False

		Label0601_30.Visible = False
		Label0601_31.Visible = False
		TextBox0616.Visible = False
		TextBox0616.ReadOnly = True

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		Tn = CDbl(TextBox0602.Text)
		'm_IAS = 3.6 * DeConvertSpeed(CDbl(TextBox0603.Text))

		'ArDir = Azt2Dir(FinalNav.pPtGeo, SpinButton0201.Value)
		Turn = 2 * ComboBox0601.SelectedIndex - 1
		PtTOB = Nothing

		Do
			T = Tn
			'    CreateNavPolys IAS, Tn, Hhold

			Standart80_260(Hhold, T, _ArDir, Turn, _initialIAS, RModel, ptTurn80_260)
			FarFAF = ptTurn80_260.Point(1)
			PtSOL = ptTurn80_260.Point(0)
			PtEOL = ptTurn80_260.Point(1)
			PtFAF = FarFAF

			CalcArObstaclesMOC(ObstacleList, BuffObstacleList, Standart80_260Area, BufferedStandart80_260Area, Ix)
			PrimaryArea = Standart80_260Area
			BufferedArea = BufferedStandart80_260Area

			If Ix >= 0 Then
				fTmp = BuffObstacleList.Parts(Ix).ReqH
				TextBox0614.Text = BuffObstacleList.Obstacles(BuffObstacleList.Parts(Ix).Owner).UnicalName
			Else
				fTmp = arIASegmentMOC.Value
				TextBox0614.Text = ""
			End If

			If ComboBox0501.SelectedIndex = 0 Then
				TextBox0608.Text = CStr(ConvertHeight(fTmp + fRefHeight))
			Else
				TextBox0608.Text = CStr(ConvertHeight(fTmp))
			End If

			TextBox0602.Text = T.ToString("0.0000")
			fTmp = ReturnAngleInDegrees(ptTurn80_260.Point(0), ptTurn80_260.Point(1))
			TextBox0612.Text = CStr(ConvertAngle(Modulus(Dir2Azt(ptTurn80_260.Point(0), fTmp) - FinalNav.MagVar)))
			TextBox0613.Text = CStr(ConvertDistance(ReturnDistanceInMeters(ptTurn80_260.Point(0), ptTurn80_260.Point(1)), eRoundMode.NEAREST))

			Tn = CalcNoFAF(T)
		Loop While Tn > T

		NonPrecReportFrm.FillPage02(BuffObstacleList, Ix)
	End Sub

	Private Sub ComboBox0602_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0602.SelectedIndexChanged
		If Not bFormInitialised Then Return

		ComboBox0603.Enabled = ComboBox0602.SelectedIndex > 1
		CreateScheme()
	End Sub

	'Private Sub ComboBox0601_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) 'Handles ComboBox0601.SelectedIndexChanged
	'	If Not bFormInitialised Then Return
	'	CreateScheme()
	'End Sub

	'Private Sub ComboBox0603_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) 'Handles ComboBox0603.SelectedIndexChanged
	'	If Not bFormInitialised Then Return
	'	CreateScheme()
	'End Sub

	'Private Sub ComboBox0604_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) ' Handles ComboBox0604.SelectedIndexChanged
	'	If Not bFormInitialised Then Return
	'	CreateScheme()
	'End Sub

	Private Sub ComboBox0605_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox0605.SelectedIndexChanged
		If (ComboBox0605.SelectedIndex < 0) Then
			ComboBox0605.SelectedIndex = 0
			Return
		End If

		If ComboBox0605.SelectedIndex = 0 Then
			TextBox0609.Text = CStr(ConvertHeight(fFinalOCH))
		Else
			TextBox0609.Text = CStr(ConvertHeight(fFinalOCH + fRefHeight))
		End If
	End Sub


	Private Sub TextBox0602_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0602.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0602_Validating(TextBox0602, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0602.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0602_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0602.Validating
		Dim fTmp As Double
		Dim fTmp1 As Double

		If Not Double.TryParse(TextBox0602.Text, fTmp) Then
			If IsNumeric(TextBox0602.Tag) Then TextBox0602.Text = TextBox0602.Tag
			Return
		End If

		If fTmp < 0.5 Then
			fTmp = 0.5
			TextBox0602.Text = fTmp.ToString("0.0000")
		End If

		If Not TextBox0602.Tag Is Nothing Then
			If Double.TryParse(TextBox0602.Tag.ToString(), fTmp1) Then If fTmp = fTmp1 Then Return
		End If

		CreateScheme()
		TextBox0602.Tag = TextBox0602.Text
	End Sub

	Private Sub TextBox0603_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0603.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0603_Validating(TextBox0603, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0603.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0603_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0603.Validating
		Dim fTmp As Double
		Dim fVMax As Double
		Dim fVMin As Double

		If Not Double.TryParse(TextBox0603.Text, fTmp) Then
			If Double.TryParse(TextBox0603.Tag.ToString(), fTmp) Then
				TextBox0603.Text = TextBox0603.Tag.ToString()
				Return
			End If
		End If

		fVMin = ConvertSpeed(cViafMin.Values(Category), eRoundMode.SPECIAL)
		fVMax = ConvertSpeed(cViafStar.Values(Category), eRoundMode.SPECIAL)

		Dim lIAS As Double = fTmp

		If lIAS > fVMax Then
			lIAS = fVMax
		ElseIf lIAS < fVMin Then
			lIAS = fVMin
		End If

		If lIAS <> fTmp Then TextBox0603.Text = lIAS.ToString()

		If TextBox0603.Text = TextBox0603.Tag Then Return

		_initialIAS = 3.6 * DeConvertSpeed(lIAS)

		CreateScheme()
		TextBox0603.Tag = TextBox0603.Text
	End Sub

	Private Sub TextBox0605_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0605.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0605_Validating(TextBox0605, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0605.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0605_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0605.Validating
		Dim fVal As Double
		Dim fRDH As Double
		Dim fTime As Double
		Dim Hhold As Double

		If TextBox0605.ReadOnly Then Return
		If Not IsNumeric(TextBox0605.Text) Then Return

		fRDH = GetRDH()

		fVal = DeConvertHeight(CDbl(TextBox0605.Text))

		If fVal < LimitTextBox0605.Low Then
			fVal = LimitTextBox0605.Low
			TextBox0605.Text = CStr(ConvertHeight(fVal))
		ElseIf fVal > LimitTextBox0605.High Then
			fVal = LimitTextBox0605.High
			TextBox0605.Text = CStr(ConvertHeight(fVal))
		End If

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) - fRefHeight
			fVal = fVal - fRefHeight
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		End If

		FarFAF.Z = fVal + fRefHeight
		fTime = CDbl(TextBox0602.Text)

		CalcDescentSpeed(fTime, fVal)
	End Sub

	Private Sub TextBox0610_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0610.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0610_Validating(TextBox0610, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0610.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0610_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0610.Validating
		Dim fTmp As Double
		Dim fVMax As Double
		Dim fVMin As Double

		If (Not IsNumeric(TextBox0610.Text)) And IsNumeric(TextBox0610.Tag) Then TextBox0610.Text = TextBox0610.Tag

		fVMin = ConvertSpeed(cVfafMin.Values(Category), eRoundMode.SPECIAL)
		fVMax = ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL)

		If Not IsNumeric(TextBox0610.Text) Then
			fTmp = fVMax
		Else
			fTmp = CDbl(TextBox0610.Text)
		End If

		If fTmp > fVMax Then
			fTmp = fVMax
		ElseIf fTmp < fVMin Then
			fTmp = fVMin
		End If

		_finalIAS = 3.6 * DeConvertSpeed(fTmp)
		TextBox0610.Text = CStr(System.Math.Round(fTmp, 1))

		Dim fTime As Double
		Dim fHFarFap As Double

		fHFarFap = FarFAF.Z - fRefHeight
		fTime = CDbl(TextBox0602.Text)

		CalcDescentSpeed(fTime, fHFarFap)
	End Sub

	Private Sub TextBox0616_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0616.KeyPress
		If TextBox0616.ReadOnly Then Return

		If e.KeyChar = Chr(13) Then
			TextBox0616_Validating(TextBox0616, Nothing)
		Else
			TextBoxFloat(e.KeyChar, TextBox0616.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True

	End Sub

	Private Sub TextBox0616_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0616.Validating
		If TextBox0616.ReadOnly Then Return

		Dim fTmp As Double

		If (Not Double.TryParse(TextBox0616.Text, fTmp)) Then
			If IsNumeric(TextBox0616.Tag) Then TextBox0616.Text = TextBox0616.Tag
			Return
		End If

		Dim Vsan As Double = 0.277777777777778 * _initialTAS
		Dim Rs As Double

		If OptionButton0601.Checked Then
			Dim Rv As Double = 943.27 / _initialTAS
			If Rv > 3.0 Then Rv = 3.0
			Rs = 1000.0 * _initialTAS / (62.83 * Rv)
		Else
			Rs = 0.0
		End If

		Dim Hhold As Double

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		'0.5 - 5 min
		Dim fdMin As Double
		Dim fdMax As Double
		Dim dh As Double = Hhold - FinalNav.pPtGeo.Z

		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			Dim cooNav As NavaidData = DMEList(FinalNav.PairNavaidIndex)
			dh = Hhold - cooNav.pPtGeo.Z
		End If

		fdMin = Math.Sqrt(30.0 * 30.0 * Vsan * Vsan + 4.0 * Rs * Rs + dh * dh)
		fdMax = Math.Sqrt(300.0 * 300.0 * Vsan * Vsan + 4.0 * Rs * Rs + dh * dh)

		'fVMin = ConvertSpeed(cVfafMin.Values(Category), eRoundMode.SPECIAL)
		'fVMax = ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL)

		fTmp = DeConvertDistance(fTmp)
		fdLimit = fTmp

		If fdLimit < fdMin Then fdLimit = fdMin
		If fdLimit > fdMax Then fdLimit = fdMax
		If fdLimit <> fTmp Then TextBox0616.Text = ConvertDistance(fdLimit).ToString()

		Dim fTime As Double
		fTime = Math.Sqrt(fdLimit * fdLimit - dh * dh - 4.0 * Rs * Rs) / Vsan

		TextBox0616.Tag = TextBox0616.Text

		TextBox0602.Text = (fTime / 60.0).ToString("0.0000")
		TextBox0602_Validating(TextBox0602, Nothing)
	End Sub

#End Region

#Region "Page 08"

	Sub FillCombo0706()
		Dim I As Integer
		Dim N As Integer
		'Dim fLinePtDist As Double
		Dim fMaxSDF As Double
		Dim fMinSDF As Double

		ComboBox0706.Items.Clear()
		N = UBound(InSectList)
		fMaxSDF = dMaxSDF '- 2.0*arFAFTolerance.Value
		fMinSDF = dMinSDF ' + 2.0 * arFAFTolerance.Value

		For I = 0 To N
			If (InSectList(I).TypeCode <> eNavaidType.NONE) Then Continue For
			If (InSectList(I).Distance > fMaxSDF) Then Continue For
			If (InSectList(I).Distance < fMinSDF) Then Continue For

			'fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)

			'If fLinePtDist < 20.0 Then
			ComboBox0706.Items.Add(InSectList(I))
			'End If
		Next I

		If ComboBox0706.Items.Count > 0 Then
			CheckBox0702.Enabled = True
			ComboBox0706.SelectedIndex = 0
		Else
			CheckBox0702.Enabled = False
			CheckBox0702.Checked = False
		End If
	End Sub

	Private Sub CheckBox0701_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0701.CheckedChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		Dim fTAS As Double
		Dim CPDG As Double
		Dim fRDH As Double
		Dim MinPDG As Double
		Dim MaxPDG As Double
		Dim Hhold As Double
		Dim fDist As Double

		If Not CheckBox0701.Enabled Then Return

		'=================================================================================
		_Label0701_0.Enabled = CheckBox0701.Checked

		_Label0701_2.Enabled = CheckBox0701.Checked
		ComboBox0705.Enabled = CheckBox0701.Checked

		_Label0701_5.Enabled = CheckBox0701.Checked
		_Label0701_6.Enabled = CheckBox0701.Checked
		_Label0701_7.Enabled = CheckBox0701.Checked

		_Label0701_9.Enabled = CheckBox0701.Checked
		_Label0701_10.Enabled = CheckBox0701.Checked
		_Label0701_11.Enabled = CheckBox0701.Checked
		_Label0701_13.Enabled = CheckBox0701.Checked
		_Label0701_14.Enabled = CheckBox0701.Checked
		_Label0701_17.Enabled = CheckBox0701.Checked
		_Label0701_18.Enabled = CheckBox0701.Checked
		ComboBox0703.Enabled = CheckBox0701.Checked

		_Label0701_21.Enabled = CheckBox0701.Checked
		_Label0701_22.Enabled = CheckBox0701.Checked
		ComboBox0704.Enabled = CheckBox0701.Checked '_Label0701_23.Enabled = CheckBox0701.Checked
		_Label0701_24.Enabled = CheckBox0701.Checked
		_Label0701_25.Enabled = CheckBox0701.Checked
		_Label0701_26.Enabled = CheckBox0701.Checked
		_Label0701_27.Enabled = CheckBox0701.Checked
		_Label0701_28.Enabled = CheckBox0701.Checked

		TextBox0701.Enabled = CheckBox0701.Checked
		TextBox0702.Enabled = CheckBox0701.Checked
		TextBox0703.Enabled = CheckBox0701.Checked
		TextBox0706.Enabled = CheckBox0701.Checked

		TextBox0707.Enabled = CheckBox0701.Checked

		TextBox0708.Enabled = CheckBox0701.Checked
		TextBox0710.Enabled = CheckBox0701.Checked
		TextBox0711.Enabled = CheckBox0701.Checked
		TextBox0712.Enabled = CheckBox0701.Checked
		TextBox0713.Enabled = CheckBox0701.Checked
		TextBox0714.Enabled = CheckBox0701.Checked

		ComboBox0701.Enabled = CheckBox0701.Checked
		ComboBox0702.Enabled = CheckBox0701.Checked

		Frame0701.Enabled = CheckBox0701.Checked
		Frame0702.Enabled = CheckBox0701.Checked
		'=================================================================================

		If OptionButton0101.Checked Then
			If OptionButton0201.Checked Or CheckBox0701.Checked Then
				fMinOCH = arFASeg_FAF_MOC.Value
			Else
				fMinOCH = arFASegmentMOC.Value
			End If

			If fAlignOCH > fMinOCH Then fMinOCH = fAlignOCH
		Else
			fMinOCH = fNewVisAprOCH
		End If

		If OptionButton0201.Checked Then
			If CheckBox0701.Checked Then
				OptionButton0802.Text = My.Resources.str10918
				ComboBox0702_SelectedIndexChanged(ComboBox0702, New System.EventArgs())
			Else
				OptionButton0802.Text = My.Resources.str10903
				Try
					If Not SDFElem Is Nothing Then pGraphics.DeleteElement(SDFElem)
				Catch ex As Exception
				End Try

				Try
					If Not PtSDFElem Is Nothing Then pGraphics.DeleteElement(PtSDFElem)
				Catch ex As Exception
				End Try

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				AdjustSDFProfile(0)
				NonPrecReportFrm.FillPage03(FAFWorkObstacleList, IxWorkJO, True)
			End If

			Return
		End If
		'=================================================================================
		If CheckBox0701.Checked Then ComboBox0702_SelectedIndexChanged(ComboBox0702, New System.EventArgs())

		fRDH = GetRDH()

		'    Frame1505.Enabled = CheckBox0701.Checked
		fDist = ReturnDistanceInMeters(FictTHR, FarFAF)
		CPDG = (FarFAF.Z - fRefHeight - fRDH) / fDist

		Hhold = DeConvertHeight(CDbl(TextBox0601.Text))

		If ComboBox0501.SelectedIndex = 1 Then
			Hhold = Hhold + fRefHeight
		End If

		fTAS = IAS2TAS(3.6 * cViafMax.Values(Category), Hhold, CurrADHP.ISAtC)
		CPDGmin = 3.6 * fDesV / fTAS
		'CPDGmin = 3.6 * fDesV / IAS2TAS(3.6 * cVfafMin.Values(Category), FarFAF.Z, CurrADHP.ISAtC)

		If CPDGmin > arFADescent_Nom.Value Then CPDGmin = arFADescent_Nom.Value

		'fTAS = IAS2TAS(3.6 * cViafMin.Values(Category), Hhold, CurrADHP.ISAtC)
		MinPDG = Max(CPDG, Max(CPDGmin, arFADescent_Min.Value))

		If CheckBox0701.Checked Then
			OptionButton0802.Text = My.Resources.str10918
			MaxPDG = arFADescent_Max.Values(Category) 'Min(CPDGmax, arFADescent_Max.Values(Category))
			_CurrPDG = Max(MinPDG, arFADescent_Nom.Value)

			TextBox0701.Text = CStr(System.Math.Round(_CurrPDG * 100.0 + 0.04999999, 1))

			LimitTextBox0701.Low = System.Math.Round(MinPDG * 100.0 + 0.04999999, 1)
			LimitTextBox0701.High = System.Math.Round(MaxPDG * 100.0 + 0.04999999, 1)

			ToolTip1.SetToolTip(TextBox0701, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(MinPDG * 100.0 + 0.04999999, 1)) + "%" + My.Resources.str00222 + CStr(System.Math.Round(MaxPDG * 100.0, 1)) + "%")
			'        _Label0701_1.Caption = TextBox0701.ToolTipText
			TextBox0701.Tag = ""
			TextBox0701_Validating(TextBox0701, New CancelEventArgs())
		Else
			'NextBtn.Enabled = True
			OptionButton0802.Text = My.Resources.str10919
			Try
				If Not FAFFIXElem Is Nothing Then pGraphics.DeleteElement(FAFFIXElem)
			Catch ex As Exception
			End Try

			Try
				If Not FAFAreaElem Is Nothing Then pGraphics.DeleteElement(FAFAreaElem)
			Catch ex As Exception
			End Try

			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

			_CurrPDG = MinPDG

			fTmp = (FarFAF.Z - fRefHeight - fRDH) / _CurrPDG
			'fTmp = (FarFAF.Z - fRefHeight - arAbv_Treshold.Value) / m_CurrPDG
			If fTmp > fDist Then fTmp = fDist

			PtFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, fTmp)

			PtFAF.Z = FarFAF.Z - fRefHeight
			PtFAF.M = FarFAF.M

			CreateFinalBaseArea()
			NonPrecReportFrm.FillPage03(NavObstacleList, NavIx, NavJx)

			'/////////////
			ArrivalProfile.ClearPoints()
			fTmp = Point2LineDistancePrj(FictTHR, FinalNav.pPtPrj, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, FinalNav.pPtPrj)

			ArrivalProfile.AddPoint(fTmp, Hhold - fRefHeight, CDbl(TextBox0612.Text), 0, -1)
			ArrivalProfile.AddPoint(fDist, PtFAF.Z, CDbl(TextBox0612.Text), 0, -1) 'CodeProcedureDistance.FAF

			'fTAS = IAS2TAS(3.6 * cVfafMax.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
			'fTmp = 0.06 * fDesV / fTAS

			CPDGmin = 3.6 * fDesV / IAS2TAS(3.6 * cVfafMin.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
			If CPDGmin > arFADescent_Nom.Value Then CPDGmin = arFADescent_Nom.Value

			fDist = (PtFAF.Z - fhTHR) / CPDGmin
			ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), CPDGmin, -1)

			fDist = (fFinalOCH - fhTHR) / CPDGmin
			ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), CPDGmin, -1)
		End If
	End Sub

	Private Sub CheckBox0702_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox0702.CheckedChanged
		ComboBox0706.Enabled = CheckBox0702.Checked
		TextBox0702.ReadOnly = CheckBox0702.Checked

		If CheckBox0702.Checked Then
			TextBox0702.BackColor = SystemColors.ButtonFace
			ComboBox0706_SelectedIndexChanged(ComboBox0706, New System.EventArgs())
		Else
			TextBox0702.BackColor = SystemColors.Window
		End If
	End Sub

	Private Sub ComboBox0701_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0701.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		If Not IsNumeric(TextBox0702.Tag) Then Return
		fTmp = CDbl(TextBox0702.Tag)
		If ComboBox0701.SelectedIndex = 0 Then
			TextBox0702.Text = CStr(ConvertHeight(fTmp + fRefHeight))
		Else
			TextBox0702.Text = CStr(ConvertHeight(fTmp))
		End If

		If OptionButton0201.Checked Then
			If ComboBox0701.SelectedIndex = 0 Then
				ToolTip1.SetToolTip(TextBox0702, My.Resources.str00210 + CStr(ConvertHeight(LimitTextBox0702.Low + fRefHeight, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High + fRefHeight, eRoundMode.NEAREST)))
			Else
				ToolTip1.SetToolTip(TextBox0702, My.Resources.str00210 + CStr(ConvertHeight(LimitTextBox0702.Low, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High, eRoundMode.NEAREST)))
			End If
		Else
			If ComboBox0701.SelectedIndex = 0 Then
				ToolTip1.SetToolTip(TextBox0702, My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(LimitTextBox0702.Low + fRefHeight, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High + fRefHeight, eRoundMode.NEAREST)))
			Else
				ToolTip1.SetToolTip(TextBox0702, My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(LimitTextBox0702.Low, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High, eRoundMode.NEAREST)))
			End If
		End If
	End Sub

	Private Sub ComboBox0702_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0702.SelectedIndexChanged
		If Not bFormInitialised Then Return

		If OptionButton0201.Checked Then
			IntersectNavChanged_WithFAF()
		Else
			IntersectNavChanged_WithoutFAF()
		End If
	End Sub

	Private Sub ComboBox0703_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0703.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		If Not IsNumeric(TextBox0710.Tag) Then Return

		fTmp = CDbl(TextBox0710.Tag)
		If ComboBox0703.SelectedIndex = 0 Then
			TextBox0710.Text = CStr(ConvertHeight(fTmp))
		Else
			TextBox0710.Text = CStr(ConvertHeight(fTmp - fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0704_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0704.SelectedIndexChanged
		If ComboBox0704.SelectedIndex = 0 Then
			TextBox0713.Text = CStr(ConvertHeight(SDFOCH13))
		Else
			TextBox0713.Text = CStr(ConvertHeight(SDFOCH13 + fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0705_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0705.SelectedIndexChanged
		If ComboBox0705.SelectedIndex = 0 Then
			TextBox0703.Text = CStr(ConvertHeight(SDFOCH))
		Else
			TextBox0703.Text = CStr(ConvertHeight(SDFOCH + fRefHeight))
		End If
	End Sub

	Private Sub ComboBox0706_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox0706.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fFAFDist As Double
		Dim fSDFDist As Double
		Dim prevPDG As Double
		Dim fSDFPDG As Double
		Dim fHSDF As Double
		Dim fRDH As Double
		Dim WPT As NavaidData

		If Not CheckBox0702.Checked Then Return
		If ComboBox0706.SelectedIndex < 0 Then Return

		WPT = ComboBox0706.SelectedItem

		If OptionButton0101.Checked Then
			fRDH = arAbv_Treshold.Value
		Else
			fRDH = fMinOCH
		End If

		If PtSDF Is Nothing Then PtSDF = New ESRI.ArcGIS.Geometry.Point()

		PtSDF.PutCoords(WPT.pPtPrj.X, WPT.pPtPrj.Y)
		fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
		'fSDFDist = Point2LineDistancePrj(FictTHR, PtSDF, _ArDir + 90.0)
		TextBox0706.Text = CStr(ConvertDistance(fSDFDist, eRoundMode.NEAREST))
		'=====================================================================
		fSDFPDG = 0.01 * CDbl(TextBox0701.Text)
		fHSDF = fSDFDist * fSDFPDG + fRDH

		If ComboBox0701.SelectedIndex = 0 Then
			TextBox0702.Text = CStr(ConvertHeight(fHSDF + fRefHeight))
		Else
			TextBox0702.Text = CStr(ConvertHeight(fHSDF))
		End If

		fFAFDist = ReturnDistanceInMeters(FictTHR, PtFAF)
		TextBox0711.Text = CStr(ConvertDistance(fFAFDist - fSDFDist, eRoundMode.NEAREST))

		PtSDF.Z = fHSDF
		PtSDF.M = _ArDir

		If OptionButton0201.Checked Then
			prevPDGmin = (CurrhFAF - fHSDF) / (fFAFDist - fSDFDist)
		Else
			PtFAF = PtSDF
			prevPDGmin = (FarFAF.Z - fRefHeight - fHSDF) / (fFAFDist - fSDFDist)
		End If

		'=====================================================================
		prevPDGmax = arFADescent_Max.Values(Category)
		If prevPDGmin < arFADescent_Min.Value Then prevPDGmin = arFADescent_Min.Value
		If prevPDGmin > prevPDGmax Then prevPDGmin = prevPDGmax
		prevPDG = arFADescent_Nom.Value

		If prevPDG < prevPDGmin Then prevPDG = prevPDGmin
		If prevPDG > prevPDGmax Then prevPDG = prevPDGmax

		ToolTip1.SetToolTip(TextBox0712, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(100.0 * prevPDGmin + 0.04999999, 1)) + My.Resources.str00222 + CStr(System.Math.Round(100.0 * prevPDGmax - 0.04999999, 1)))
		TextBox0712.Text = CStr(System.Math.Round(100.0 * prevPDG + 0.04999999, 1))
		TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
		'============================================================================

		TextBox0702.Tag = fHSDF
		TextBox0702_Validating(TextBox0702, New CancelEventArgs())
	End Sub

	Private Sub TextBox0701_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0701.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0701_Validating(TextBox0701, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0701.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0701_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0701.Validating
		Dim fNewPDG As Double
		If CheckBox0701.Checked Then
			If Not IsNumeric(TextBox0701.Text) Then Return
			fNewPDG = CDbl(TextBox0701.Text)

			If fNewPDG < LimitTextBox0701.Low Then
				fNewPDG = LimitTextBox0701.Low
				TextBox0701.Text = CStr(LimitTextBox0701.Low)
			ElseIf fNewPDG > LimitTextBox0701.High Then
				fNewPDG = LimitTextBox0701.High
				TextBox0701.Text = CStr(LimitTextBox0701.High)
			End If
			If TextBox0701.Tag = TextBox0701.Text Then Return

			fNewPDG = 0.01 * fNewPDG
			PDGChanged(fNewPDG)
		End If
	End Sub

	Private Sub TextBox0702_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0702.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0702_Validating(TextBox0702, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0702.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0702_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0702.Validating
		Dim fH As Double

		If Not IsNumeric(TextBox0702.Text) Then Return

		If ComboBox0701.SelectedIndex = 0 Then
			fH = DeConvertHeight(CDbl(TextBox0702.Text)) - fRefHeight
		Else
			fH = DeConvertHeight(CDbl(TextBox0702.Text))
		End If

		If IsNumeric(TextBox0702.Tag) Then
			If fH = CDbl(TextBox0702.Tag) Then Return
		End If

		If fH > LimitTextBox0702.High Then
			fH = LimitTextBox0702.High
			If ComboBox0701.SelectedIndex = 0 Then
				TextBox0702.Text = CStr(ConvertHeight(fH + fRefHeight, eRoundMode.NEAREST))
			Else
				TextBox0702.Text = CStr(ConvertHeight(fH, eRoundMode.NEAREST))
			End If
		ElseIf fH < LimitTextBox0702.Low Then
			fH = LimitTextBox0702.Low
			If ComboBox0701.SelectedIndex = 0 Then
				TextBox0702.Text = CStr(ConvertHeight(fH + fRefHeight, eRoundMode.NEAREST))
			Else
				TextBox0702.Text = CStr(ConvertHeight(fH, eRoundMode.NEAREST))
			End If
		End If

		If OptionButton0201.Checked Then
			hKTChanged_WithFAF(fH)
		Else
			hKTChanged_WithoutFAF(fH)
		End If
		TextBox0702.Tag = fH
	End Sub

	Private Sub TextBox0712_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0712.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0712.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0712_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0712.Validating
		Dim fPDG As Double
		Dim fHSDF As Double
		Dim fHMax As Double
		Dim fHorDist As Double
		Dim fSDFDist As Double
		Dim fFAFDist As Double
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		If Not IsNumeric(TextBox0712.Text) Then Return
		fPDG = 0.01 * CDbl(TextBox0712.Text)
		If (fPDG < prevPDGmin) Or (fPDG > prevPDGmax) Then
			If fPDG < prevPDGmin Then fPDG = prevPDGmin
			'If fPDG > prevPDGmax Then fPDG = prevPDGmax
			TextBox0712.Text = CStr(System.Math.Round(100.0 * fPDG, 1))
		End If

		If Not CheckBox0702.Checked Then
			fSDFDist = DeConvertDistance(CDbl(TextBox0706.Text))
		Else
			fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
		End If

		If ComboBox0701.SelectedIndex = 0 Then
			fHSDF = DeConvertHeight(CDbl(TextBox0702.Text)) - fRefHeight
		Else
			fHSDF = DeConvertHeight(CDbl(TextBox0702.Text))
		End If

		If OptionButton0201.Checked Then
			ptTmpFAF = PtFAF
			fHMax = ptTmpFAF.Z
		Else
			ptTmpFAF = FarFAF
			fHMax = FarFAF.Z - fRefHeight
		End If

		fFAFDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR)

		fHorDist = fFAFDist - (fHMax - fHSDF) / fPDG - fSDFDist

		If (fHorDist < 0.0) And (fHorDist > -0.5) Then fHorDist = 0.0

		If (fHorDist < 0.0) Then
			fPDG = (fHMax - fHSDF) / (fFAFDist - fSDFDist)
			fHorDist = 0.0
			TextBox0712.Text = Math.Round(100 * fPDG, 1).ToString()
		End If

		TextBox0707.Text = CStr(ConvertDistance(fHorDist, 1))

		'    If _OptionButton0201_1.Value Then
		AdjustSDFProfile(fHorDist)
	End Sub

#End Region

#Region "Page 09"

	Private Sub CheckBox0801_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0801.CheckedChanged
		If Not bFormInitialised Then Return

		OkBtn.Enabled = Not CheckBox0801.Checked
		NextBtn.Enabled = CheckBox0801.Checked
		_Label0801_13.Enabled = Not CheckBox0801.Checked

		TextBox0809.ReadOnly = CheckBox0801.Checked
		'CheckBox0802.Visible = Not CheckBox0801.Checked

		label0801_21.Visible = Not CheckBox0801.Checked
		TextBox0810.Visible = Not CheckBox0801.Checked
		label0801_22.Visible = Not CheckBox0801.Checked

		If CheckBox0801.Checked Then
			On Error Resume Next
			If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
			On Error GoTo 0
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		Else
			TextBox0809_Validating(TextBox0809, Nothing)
		End If
	End Sub

	Private Sub CheckBox0802_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox0802.CheckedChanged
		Return

		TextBox0810.ReadOnly = Not CheckBox0802.Checked

		If CheckBox0802.Checked Then Return

		Dim fTmp As Double
		fTmp = (_TerminationAltitude - MAPtOCH - fRefHeight) / fMissAprPDG + dMAPt2SOC
		TextBox0810.Text = ConvertDistance(fTmp).ToString()
	End Sub

	Private Sub OptionButton0801_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0801.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		ComboBox0801.Enabled = True
		_Label0801_6.Enabled = True
		ComboBox0801_SelectedIndexChanged(ComboBox0801, New System.EventArgs())

		'TextBox0804.ReadOnly = True
		'TextBox0804.BackColor = System.Drawing.SystemColors.Control

		_Label0801_5.Visible = True
	End Sub

	Private Sub OptionButton0802_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0802.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim dTHR2FAF As Double
		Dim fDistMin As Double
		Dim fDistMax As Double
		Dim fTmp0 As Double
		Dim fTmp1 As Double

		Dim tipStr As String
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		'TextBox0804.ReadOnly = False
		'TextBox0804.BackColor = System.Drawing.SystemColors.Window

		_Label0801_5.Visible = False

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		dTHR2FAF = ReturnDistanceInMeters(FictTHR, ptTmpFAF)

		fDistMax = dTHR2FAF - fFAF2NEARMAPt
		fDistMin = dTHR2FAF - fFAF2FARMAPt
		'====================================================
		fTmp0 = ConvertDistance(fDistMin, 3)
		fTmp1 = ConvertDistance(fDistMax, 1)
		If fTmp0 > fTmp1 Then
			fTmp0 = ConvertDistance(0.5 * (fDistMin + fDistMax), eRoundMode.NEAREST)
			fTmp1 = fTmp0
		End If

		tipStr = My.Resources.str00220 + vbCrLf + My.Resources.str00221 + CStr(fTmp0) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(fTmp1) + " " + DistanceConverter(DistanceUnit).Unit

		_Label0801_1.Text = tipStr
		tipStr = Replace(tipStr, vbCrLf, "   ")
		ToolTip1.SetToolTip(TextBox0801, tipStr)

		TextBox0801.Text = CStr(fTmp1)

		ComboBox0801.Enabled = False
		_Label0801_6.Enabled = False
		TextBox0801.ReadOnly = False
		_Label0801_1.Visible = True

		TextBox0801.BackColor = System.Drawing.SystemColors.Window
		TextBox0801_Validating(TextBox0801, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub ComboBox0801_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0801.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim tipStr As String
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim MaxTreshold As Double
		Dim dTHR2FAF As Double
		Dim fVal0 As Double
		Dim fVal1 As Double
		Dim fTmp As Double
		Dim K As Integer
		Dim I As Integer
		Dim N As Integer

		'If Not OptionButton0801.Value Then Return
		If Not ComboBox0801.Enabled Then Return
		K = ComboBox0801.SelectedIndex
		If K < 0 Then Return

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		dTHR2FAF = ReturnDistanceInMeters(FictTHR, ptTmpFAF)
		'MaxTreshold = dTHR2FAF - Max(dNearMAPt + 100.0, arFAFTolerance.Value)
		MaxTreshold = dTHR2FAF - arFAFTolerance.Value
		'If fTmp > dTHR2FAF - MaxTreshold Then fTmp = dTHR2FAF - MaxTreshold
		'===================================================================================
		tipStr = My.Resources.str10514 + vbCrLf
		_Label0801_6.Text = GetNavTypeName(MAPtNavDat(K).TypeCode)

		Select Case MAPtNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				tipStr = "MAPt " + My.Resources.str00106
				TextBox0801.ReadOnly = True
				TextBox0801.BackColor = System.Drawing.SystemColors.Control
				fTmp = dTHR2FAF - Point2LineDistancePrj(ptTmpFAF, MAPtNavDat(K).pPtPrj, _ArDir + 90.0)
				TextBox0801.Text = CStr(ConvertDistance(fTmp, eRoundMode.NEAREST))
				_Label0801_5.Text = tipStr
			Case eIntersectionType.ByAngle
				'            _Label0801_1.Caption = "Рекомендуемый интервал значений:" + vbCrLf + _
				''                "от " + CStr(MAPtNavDat(k).ValMin(0)) + _
				''                " до " + CStr(MAPtNavDat(k).ValMax(0))
				TextBox0801.ReadOnly = False
				TextBox0801.BackColor = System.Drawing.SystemColors.Window
				fVal0 = dTHR2FAF - MAPtNavDat(K).ValMax(0)
				fVal1 = dTHR2FAF - MAPtNavDat(K).ValMin(0)
				If fVal0 > MaxTreshold Then fVal0 = MaxTreshold
				If fVal1 > MaxTreshold Then fVal1 = MaxTreshold

				tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(fVal0, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(fVal1, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit

				TextBox0801.Text = CStr(ConvertDistance(fVal0, eRoundMode.NEAREST))
			Case eIntersectionType.ByDistance
				N = UBound(MAPtNavDat(K).ValMax)

				For I = 0 To N
					fVal0 = dTHR2FAF - MAPtNavDat(K).ValMax(I)
					fVal1 = dTHR2FAF - MAPtNavDat(K).ValMin(I)
					If fVal0 > MaxTreshold Then fVal0 = MaxTreshold
					If fVal1 > MaxTreshold Then fVal1 = MaxTreshold

					tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(fVal0, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(fVal1, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
					If I < N Then
						tipStr = tipStr + vbCrLf
					End If
				Next I
				TextBox0801.ReadOnly = False
				TextBox0801.BackColor = System.Drawing.SystemColors.Window
				N = UBound(MAPtNavDat(K).ValMin)
				fTmp = dTHR2FAF - MAPtNavDat(K).ValMin(0)
				If fTmp > MaxTreshold Then fTmp = MaxTreshold
				TextBox0801.Text = CStr(ConvertDistance(fTmp, eRoundMode.NEAREST))
		End Select

		_Label0801_1.Text = tipStr
		tipStr = Replace(tipStr, vbCrLf, "   ")
		ToolTip1.SetToolTip(TextBox0801, tipStr)
		TextBox0801_Validating(TextBox0801, New System.ComponentModel.CancelEventArgs(True))
	End Sub

	Private Sub ComboBox0802_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0802.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim N As Integer
		Dim fMOC As Double
		Dim fReqH As Double

		If (MultiPage1.SelectedIndex < 2) Or (ComboBox0802.SelectedIndex < 0) Then Return

		fMOC = DeConvertHeight(CDbl(ComboBox0802.Text))
		fStraightMissedTermHght = fMOC

		N = UBound(MAPtObstacles.Parts)
		For I = 0 To N
			fReqH = MAPtObstacles.Parts(I).Height + fMOC * MAPtObstacles.Parts(I).fTmp

			If fReqH > fStraightMissedTermHght Then
				fStraightMissedTermHght = fReqH
			End If
		Next I

		_TerminationAltitude = fStraightMissedTermHght + fRefHeight
		TextBox0809.Text = CStr(ConvertHeight(_TerminationAltitude, eRoundMode.NEAREST))
		TextBox0809_Validating(TextBox0809, Nothing)

		'TextBox0809.Tag = TextBox0809.Text
	End Sub

	Private Sub ComboBox0803_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0803.SelectedIndexChanged
		If ComboBox0803.SelectedIndex = 0 Then
			TextBox0802.Text = CStr(ConvertHeight(MAPtOCH + fRefHeight))
		Else
			TextBox0802.Text = CStr(ConvertHeight(MAPtOCH))
		End If
		TextBox0802.Tag = TextBox0802.Text
	End Sub

	Private Sub TextBox0801_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0801.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0801_Validating(TextBox0801, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloatWithMinus(eventArgs.KeyChar, (TextBox0801.Text))
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0801_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0801.Validating
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPolygon
		Dim pTmpPoly2 As ESRI.ArcGIS.Geometry.IPolygon
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		Dim ObsName As String
		Dim fMAPtDist As Double

		Dim MaxTreshold As Double
		Dim LSOCShift As Double
		Dim dTHR2FAF As Double
		Dim fMinVal0 As Double
		Dim fMaxVal0 As Double
		Dim fMinVal1 As Double
		Dim fMaxVal1 As Double
		Dim fDist As Double
		Dim d0 As Double
		Dim d1 As Double
		Dim d2 As Double
		Dim d3 As Double
		Dim Ix As Integer
		Dim Jx As Integer
		Dim N As Integer
		Dim K As Integer

		If Not IsNumeric(TextBox0801.Text) Then Return
		K = ComboBox0801.SelectedIndex
		If OptionButton0801.Checked And (K < 0) Then
			MessageBox.Show(My.Resources.str00301)
			Return
		End If

		'DrawPolygon(IntermediateBaseArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolygon(IntermediateSecondArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'Application.DoEvents()

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		dTHR2FAF = ReturnDistanceInMeters(ptTmpFAF, FictTHR)
		'MaxTreshold = Max(dNearMAPt + 100.0, arFAFTolerance.Value)
		MaxTreshold = arFAFTolerance.Value

		fMAPtDist = DeConvertDistance(CDbl(TextBox0801.Text))
		If fMAPtDist > dTHR2FAF - MaxTreshold Then
			fMAPtDist = dTHR2FAF - MaxTreshold
			'    TextBox0801.Text = CStr(ConvertDistance(fMAPtDist, eRoundMode.rmNERAEST))
		End If

		'==============================================================
		If OptionButton0801.Checked And (K >= 0) Then
			Select Case MAPtNavDat(K).IntersectionType
				Case eIntersectionType.OnNavaid
					fMAPtDist = dTHR2FAF - Point2LineDistancePrj(ptTmpFAF, MAPtNavDat(K).pPtPrj, _ArDir + 90.0)
				Case eIntersectionType.ByAngle
					fMaxVal0 = dTHR2FAF - MAPtNavDat(K).ValMax(0)
					fMinVal0 = dTHR2FAF - MAPtNavDat(K).ValMin(0)
					If fMinVal0 > fMaxVal0 Then
						d0 = fMinVal0
						fMinVal0 = fMaxVal0
						fMaxVal0 = d0
					End If

					If fMaxVal0 > dTHR2FAF - MaxTreshold Then fMaxVal0 = dTHR2FAF - MaxTreshold
					If fMAPtDist < fMinVal0 Then fMAPtDist = fMinVal0
					If fMAPtDist > fMaxVal0 Then fMAPtDist = fMaxVal0
					'        TextBox0801.Text = CStr(ConvertDistance(fMAPtDist, eRoundMode.rmNERAEST))
				Case eIntersectionType.ByDistance
					N = UBound(MAPtNavDat(K).ValMax)
					fMinVal0 = dTHR2FAF - MAPtNavDat(K).ValMax(0)
					fMaxVal0 = dTHR2FAF - MAPtNavDat(K).ValMin(0)
					If fMaxVal0 > dTHR2FAF - MaxTreshold Then fMaxVal0 = dTHR2FAF - MaxTreshold

					If N = 0 Then
						If fMAPtDist < fMinVal0 Then fMAPtDist = fMinVal0
						If fMAPtDist > fMaxVal0 Then fMAPtDist = fMaxVal0
					Else
						If (fMAPtDist < fMinVal0) Or (fMAPtDist > fMaxVal0) Then
							fMinVal1 = dTHR2FAF - MAPtNavDat(K).ValMax(1)
							fMaxVal1 = dTHR2FAF - MAPtNavDat(K).ValMin(1)
							If (fMAPtDist < fMinVal0) Or (fMAPtDist > fMaxVal0) Then
								d0 = System.Math.Abs(fMinVal0 - fMAPtDist)
								d1 = System.Math.Abs(fMaxVal0 - fMAPtDist)
								d2 = System.Math.Abs(fMinVal1 - fMAPtDist)
								d3 = System.Math.Abs(fMaxVal1 - fMAPtDist)
								fMAPtDist = Min(d0, Min(d1, Min(d2, d3)))
								If fMAPtDist = d0 Then
									fMAPtDist = fMinVal0
								ElseIf fMAPtDist = d1 Then
									fMAPtDist = fMaxVal0
								ElseIf fMAPtDist = d2 Then
									fMAPtDist = fMinVal1
								ElseIf fMAPtDist = d3 Then
									fMAPtDist = fMaxVal1
								End If
							End If
						End If
					End If
			End Select
		End If

		TextBox0801.Text = CStr(ConvertDistance(fMAPtDist, eRoundMode.NEAREST))
		'==============================================================
		'    fDistMax = dTHR2FAF - fFAF2NEARMAPt
		'    fDistMin = dTHR2FAF - fFAF2FARMAPt
		'==============================================================
		dMAPt2FAF = dTHR2FAF - fMAPtDist
		pMAPt = PointAlongPlane(ptTmpFAF, _ArDir, dMAPt2FAF)
		pMAPt.M = _ArDir
		If OptionButton0802.Checked Then
			pMAPt.Z = MinOCH + fRefHeight
		End If

		SafeDeleteElement(MAPtElem)
		MAPtElem = DrawPointWithText(pMAPt, "MAPt", WPTColor)
		MAPtElem.Locked = True

		'=============================================  13:28 12.06.2003
		'CreateNavaidZone(FinalNav, m_ArDir, FictTHR, Ss, Vs, 2 * VOR.Range, 2 * VOR.Range, pPolyLeft, pPolyRight, pPolyPrime)

		pClone = pPolyRight
		pTmpPoly1 = pClone.Clone
		pTopo = pTmpPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pClone = pPolyLeft
		pTmpPoly2 = pClone.Clone
		pTopo = pTmpPoly2
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		SecPoly = pTopo.Union(pTmpPoly1)

		If OptionButton0802.Checked Then
			LSOCShift = 0.0
			MAPtByDist(LSOCShift, dMAPt2FAF)
		Else
			MAPtOnFIX()
		End If

		CreateFinalBaseArea()

		'DrawPolygon(SecPoly, 128, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
		'DrawPolygon(ZNR_Poly, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolygon(pMAPtArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
		'DrawPolygon(FinalBaseArea, RGB(0, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(FinalSecondArea, RGB(0, 0, 255), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'Application.DoEvents()
		'=============================================

		_Label0801_2.Text = My.Resources.str10905 + CStr(ConvertDistance(dNearMAPt, 3)) + " " + DistanceConverter(DistanceUnit).Unit
		_Label0801_3.Text = My.Resources.str10906 + CStr(ConvertDistance(dFarMAPt, 1)) + " " + DistanceConverter(DistanceUnit).Unit
		_Label0801_4.Text = My.Resources.str10907 + CStr(ConvertDistance(dMAPt2SOC, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit

		PtSOC = PointAlongPlane(pMAPt, _ArDir, dMAPt2SOC)
		PtSOC.M = _ArDir

		SafeDeleteElement(SOCElem)
		SOCElem = DrawPointWithText(PtSOC, "SOC", WPTColor)
		SOCElem.Locked = True
		'    GetActiveView().PartialRefresh esriViewGraphics, Nothing, Nothing

		'If OptionButton0802.Value Then
		MAPtOCH = CalcMAPtOCH(ObsName)
		PtSOC.Z = MAPtOCH
		If Not OptionButton0802.Checked Then
			pMAPt.Z = Max(MinOCH, MAPtOCH) + fRefHeight
		End If

		TextBox0807.Text = ObsName

		ComboBox0803_SelectedIndexChanged(ComboBox0803, New System.EventArgs())
		'End If
		fMAInterRange = MissApprInterRange(MAPtOCH, fMissAprPDG, Ix, Jx)
		'Label1535 = "Длина пр. участка ухода II " + vbCrLf + fMAPtDist
		TextBox0805.Text = CStr(ConvertDistance(fMAInterRange, eRoundMode.NEAREST))

		N = ArrivalProfile.MAPtIndex - 1

		While ArrivalProfile.PointsNo > N
			ArrivalProfile.RemovePoint()
		End While

		fDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR) - (ptTmpFAF.Z - MAPtOCH) / FinalAreaPDG

		ArrivalProfile.AddPoint(fDist, MAPtOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, -1)
		ArrivalProfile.AddPoint(fMAPtDist, MAPtOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, CodeProcedureDistance.MAP)
	End Sub

	Private Sub TextBox0802_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) 'Handles TextBox0802.Validating
		'Dim MaxOCH As Double
		'Dim fTmp As Double
		'Dim d0 As Double
		'Dim I As Integer
		'Dim J As Integer

		'If Not IsNumeric(TextBox0802.Text) Then Return
		'If TextBox0802.Text = TextBox0802.Tag Then Return

		'fTmp = DeConvertHeight(CDbl(TextBox0802.Text))
		'If ComboBox0803.SelectedIndex = 0 Then fTmp = fTmp - fRefHeight
		''If MAPtOCH = fTmp Then Return    '?????????????

		'MAPtOCH = fTmp
		'If FindOptimalMAPt(MAPtOCH, fFAF2NEARMAPt, d0, I, J, MaxOCH) < 0.0 Then Return

		'fFAF2FARMAPt = fFAF2NEARMAPt + d0

		'If I >= 0 Then
		'	TextBox0807.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).UnicalName
		'Else
		'	TextBox0807.Text = ""
		'End If

		'If J >= 0 Then
		'	TextBox0808.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(J).Owner).UnicalName
		'Else
		'	TextBox0808.Text = ""
		'End If

		'ComboBox0803_SelectedIndexChanged(ComboBox0803, New System.EventArgs())

		'If OptionButton0802.Checked Then OptionButton0802_CheckedChanged(OptionButton0802, New System.EventArgs())
	End Sub

	Private Sub TextBox0804_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0804.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0804_Validating(TextBox0804, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0804.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0804_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0804.Validating
		If Not IsNumeric(TextBox0804.Text) Then
			If IsNumeric(TextBox0804.Tag) Then
				TextBox0804.Text = TextBox0804.Tag
			End If
			Return
		End If

		If TextBox0804.Tag = TextBox0804.Text Then Return

		fMissAprPDG = 0.01 * CDbl(TextBox0804.Text)

		If fMissAprPDG < arMAS_Climb_Min.Value Then
			fMissAprPDG = arMAS_Climb_Min.Value
			TextBox0804.Text = CStr(100.0 * fMissAprPDG)
		ElseIf fMissAprPDG > arMAS_Climb_Max.Value Then
			fMissAprPDG = arMAS_Climb_Max.Value
			TextBox0804.Text = CStr(100.0 * fMissAprPDG)
		End If

		Dim MaxOCH As Double
		Dim d0 As Double
		Dim I As Integer
		Dim J As Integer

		MAPtOCH = arFASeg_FAF_MOC.Value
		If FindOptimalMAPt(MAPtOCH, fFAF2NEARMAPt, d0, I, J, MaxOCH) < 0.0 Then Return

		fFAF2FARMAPt = fFAF2NEARMAPt + d0
		If I >= 0 Then
			TextBox0807.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).UnicalName
		Else
			TextBox0807.Text = ""
		End If

		If J >= 0 Then
			TextBox0808.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(J).Owner).UnicalName
		Else
			TextBox0808.Text = ""
		End If

		ComboBox0803_SelectedIndexChanged(ComboBox0803, Nothing)

		If OptionButton0802.Checked Then OptionButton0802_CheckedChanged(OptionButton0802, Nothing)
		If OptionButton0801.Checked Then OptionButton0801_CheckedChanged(OptionButton0801, Nothing)
		TextBox0804.Tag = TextBox0804.Text
	End Sub

	Private Sub TextBox0809_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0809.KeyPress
		If e.KeyChar = Chr(13) Then
			TextBox0809_Validating(TextBox0809, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0809.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0809_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0809.Validating
		If CheckBox0801.Checked Then Return
		Dim fTmp As Double

		If Not Double.TryParse(TextBox0809.Text, fTmp) Then
			If Not Double.TryParse(TextBox0809.Tag, fTmp) Then TextBox0809.Text = TextBox0809.Tag
			Return
		End If

		Dim fNevValue As Double = DeConvertHeight(fTmp)
		Dim fMinVal As Double = fStraightMissedTermHght + fRefHeight
		Dim fMaxVal As Double = fMissAprPDG * 20.0 * 1852.0 + MAPtOCH + fRefHeight

		If fMaxVal < fMinVal Then fMaxVal = fMinVal

		If fNevValue < fMinVal Then fNevValue = fMinVal
		If fNevValue > fMaxVal Then fNevValue = fMaxVal

		If fNevValue <> fTmp Then TextBox0809.Text = ConvertHeight(fNevValue, eRoundMode.NEAREST).ToString()
		_TerminationAltitude = fNevValue

		fTmp = (_TerminationAltitude - MAPtOCH - fRefHeight) / fMissAprPDG + dMAPt2SOC
		TextBox0810.Text = ConvertDistance(fTmp).ToString()

		TextBox0809.Tag = TextBox0809.Text

		pStraightNomLine = New Polyline()

		'fTmp = DeConvertDistance(fNevValue)
		'pTermPt = PointAlongPlane(PtSOC, _ArDir, fTmp)
		'fTmp = ConvertDistance(fTmp + dMAPt2SOC, eRoundMode.NEAREST)
		'pMAPt = PointAlongPlane(PtFAF, _ArDir, dMAPt2FAF)
		'pMAPt.M = _ArDir

		pTermPt = PointAlongPlane(pMAPt, _ArDir, fTmp)
		pTermPt.Z = _TerminationAltitude
		'fTmp = ConvertDistance(fTmp, eRoundMode.NEAREST)

		pStraightNomLine.FromPoint = pMAPt
		pStraightNomLine.ToPoint = pTermPt

		SafeDeleteElement(MAPtCLineElem)

		MAPtCLineElem = DrawPolyLine(pStraightNomLine, RGB(255, 0, 128), 2)
	End Sub

	Private Sub TextBox0810_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox0810.KeyPress
		Return
		If e.KeyChar = Chr(13) Then
			TextBox0810_Validating(TextBox0810, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(e.KeyChar, TextBox0810.Text)
		End If

		If e.KeyChar = Chr(0) Then e.Handled = True
	End Sub

	Private Sub TextBox0810_Validating(sender As Object, e As CancelEventArgs) Handles TextBox0810.Validating
		Return

		If CheckBox0801.Checked Then Return
		If Not CheckBox0802.Checked Then Return

		Dim fTmp As Double

		If Not Double.TryParse(TextBox0810.Text, fTmp) Then
			If Not Double.TryParse(TextBox0810.Tag, fTmp) Then TextBox0810.Text = TextBox0810.Tag
			Return
		End If

		Dim fNevValue As Double = fTmp
		Dim fMinVal As Double = 2.0 '+ ConvertDistance(fStraightMissedTermAlt / fMissAprPDG)
		Dim fMaxVal As Double = 20.0 + ConvertDistance(dMAPt2SOC)

		If fNevValue < fMinVal Then fNevValue = fMinVal
		If fNevValue > fMaxVal Then fNevValue = fMaxVal

		If fNevValue <> fTmp Then
			fTmp = DeConvertDistance(fNevValue)
			TextBox0810.Text = ConvertDistance(fTmp, eRoundMode.NEAREST).ToString()
		End If

		TextBox0810.Tag = TextBox0810.Text

		Dim pTermPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolyline As IPolyline

		pPolyline = New Polyline()

		fTmp = DeConvertDistance(fNevValue)

		'pTermPt = PointAlongPlane(PtSOC, _ArDir, fTmp)
		'fTmp = ConvertDistance(fTmp + dMAPt2SOC, eRoundMode.NEAREST)

		pTermPt = PointAlongPlane(pMAPt, _ArDir, fTmp)
		'fTmp = ConvertDistance(fTmp, eRoundMode.NEAREST)

		pPolyline.FromPoint = pMAPt
		pPolyline.ToPoint = pTermPt

		On Error Resume Next
		If Not MAPtCLineElem Is Nothing Then pGraphics.DeleteElement(MAPtCLineElem)
		On Error GoTo 0

		MAPtCLineElem = DrawPolyLine(pPolyline, RGB(255, 0, 128), 2)
	End Sub

#End Region

#Region "Page 10"

	Private Sub OptionButton0901_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0901.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		TextBox1101.BackColor = System.Drawing.SystemColors.Window
		TextBox1101.ReadOnly = False
		Frame1102.Visible = False
		Frame1101.Visible = True
		TextBox1110.Visible = False
		_Label1101_12.Visible = False
		_Label1101_20.Visible = False
		_Label1501_4.Text = My.Resources.str16008
	End Sub

	Private Sub OptionButton0902_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton0902.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		TextBox1101.BackColor = System.Drawing.SystemColors.Control
		TextBox1101.ReadOnly = True
		Frame1102.Visible = True
		Frame1101.Visible = False
		TextBox1110.Visible = True
		_Label1101_12.Visible = True
		_Label1101_20.Visible = True
		_Label1501_4.Text = My.Resources.str16002
	End Sub

	Private Sub ComboBox0901_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox0901.SelectedIndexChanged
		If Not bFormInitialised Then Return

		TurnDir = 1 - 2 * ComboBox0901.SelectedIndex
	End Sub

	Private Sub TextBox0901_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0901.KeyPress
		TextBoxInteger(eventArgs.KeyChar)
		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0901_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0901.Validating
		Dim fTmp As Double
		Dim fNewVal As Double
		'arMAT_Bank
		If Not IsNumeric(TextBox0901.Text) Then
			TextBox0901.Text = CStr(15.0)
		Else
			fTmp = CDbl(TextBox0901.Text)
			fNewVal = fTmp
			If fTmp < 15.0 Then fTmp = 15.0
			If fTmp > 25.0 Then fTmp = 25.0
			If fNewVal <> fTmp Then TextBox0901.Text = CStr(fTmp)
		End If
	End Sub

	Private Sub TextBox0902_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox0902.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox0902_Validating(TextBox0902, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox0902.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox0902_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0902.Validating
		Dim K As Integer
		Dim fTmp As Double
		Dim fMinVal As Double
		Dim fMaxVal As Double

		K = ComboBox0001.SelectedIndex
		If (K < 0) Or Not IsNumeric(TextBox0902.Text) Then Return

		fMinVal = ConvertSpeed(cVmaInter.Values(K), eRoundMode.NEAREST)
		fMaxVal = ConvertSpeed(cVmaFaf.Values(K), eRoundMode.NEAREST)

		fTmp = CDbl(TextBox0902.Text)
		If fTmp < fMinVal Then TextBox0902.Text = CStr(fMinVal)
		If fTmp > fMaxVal Then TextBox0902.Text = CStr(fMaxVal)
	End Sub

#End Region

#Region "Page 11"

	Private Sub ComboBox1001_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1001.SelectedIndexChanged
		If IxMaxOCH > -1 Then
			If ComboBox1001.SelectedIndex = 0 Then
				TextBox1003.Text = CStr(ConvertHeight(AvdOCH + fRefHeight, eRoundMode.CEIL))
			Else
				TextBox1003.Text = CStr(ConvertHeight(AvdOCH, eRoundMode.CEIL))
			End If
		End If
	End Sub

	Private Sub ComboBox1002_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1002.SelectedIndexChanged
		If ComboBox1002.SelectedIndex = 0 Then
			TextBox1004.Text = CStr(ConvertHeight(OverOCH + fRefHeight, eRoundMode.CEIL))
		Else
			TextBox1004.Text = CStr(ConvertHeight(OverOCH, eRoundMode.CEIL))
		End If
	End Sub

	Private Sub TextBox1004_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1004.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1004_Validating(TextBox1004, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1004.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1004_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1004.Validating
		Dim N As Integer
		Dim fTmp As Double
		Dim fFAFDist As Double
		Dim fMAPtDist As Double
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		If Not IsNumeric(TextBox1004.Text) Then Return

		fTmp = CDbl(TextBox1002.Tag)

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		If fTmp > ptTmpFAF.Z Then
			MessageBox.Show(My.Resources.str00504)  '"Поднимите высоту FAF"
			NextBtn.Enabled = False
			Return
		End If

		OverOCH = DeConvertHeight(CDbl(TextBox1004.Text))
		If ComboBox1002.SelectedIndex = 0 Then OverOCH = OverOCH - fRefHeight

		If OverOCH > ptTmpFAF.Z Then OverOCH = ptTmpFAF.Z
		If OverOCH < fTmp Then OverOCH = fTmp

		CalcAvoidOCH(OverOCH)
		ClosestToMOC50(OverOCH, _CurrPDG)

		If ComboBox1002.SelectedIndex < 0 Then
			ComboBox1002.SelectedIndex = 0
		Else
			ComboBox1002_SelectedIndexChanged(ComboBox1002, New System.EventArgs())
		End If

		PtSOC.Z = OverOCH
		pMAPt.Z = OverOCH + fRefHeight
		'====
		N = ArrivalProfile.MAPtIndex - 1

		While ArrivalProfile.PointsNo > N
			ArrivalProfile.RemovePoint()
		End While

		fFAFDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR) - (ptTmpFAF.Z - OverOCH) / FinalAreaPDG
		fMAPtDist = DeConvertDistance(CDbl(TextBox0801.Text))

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ArrivalProfile.AddPoint(fFAFDist, OverOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.OTHER_SDF)
		Else
			ArrivalProfile.AddPoint(fFAFDist, OverOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, CodeProcedureDistance.FAF)
		End If

		ArrivalProfile.AddPoint(fMAPtDist, OverOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, CodeProcedureDistance.MAP)
	End Sub

#End Region

#Region "Page 12"

	Private Sub OptionButton1101_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton1101.CheckedChanged, OptionButton1102.CheckedChanged
		If Not bFormInitialised Then Return
		If Not DirectCast(eventSender, RadioButton).Checked Then Return

		If DirectCast(eventSender, RadioButton).Enabled Then
			ComboBox1102_SelectedIndexChanged(ComboBox1102, Nothing)
		End If
	End Sub

	Private Sub ComboBox1101_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1101.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim fTmp As Double
		Dim fTmpH1 As Double
		Dim fTmpH2 As Double

		If Not IsNumeric(TextBox1101.Tag) Then Return
		fTmp = CDbl(TextBox1101.Tag)
		fTmpH1 = DeConvertHeight(CDbl(TextBox1005.Text))
		fTmpH2 = DeConvertHeight(CDbl(TextBox1006.Text))

		If ComboBox1101.SelectedIndex = 0 Then
			TextBox1101.Text = CStr(ConvertHeight(fTmp + fRefHeight, eRoundMode.NEAREST))
			_Label1101_2.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(fTmpH1 + fRefHeight, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(fTmpH2 + fRefHeight, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		Else
			TextBox1101.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))
			_Label1101_2.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(fTmpH1, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(fTmpH2, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		End If
		ToolTip1.SetToolTip(TextBox1101, _Label1101_2.Text)
	End Sub

	Private Sub ComboBox1102_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1102.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim Kmin As Integer
		Dim Kmax As Integer
		Dim tipStr As String

		If OptionButton0901.Checked Then Return

		K = ComboBox1102.SelectedIndex
		If K < 0 Then Return

		_Label1101_3.Text = GetNavTypeName(TurnInterNavDat(K).TypeCode)

		If (TurnInterNavDat(K).IntersectionType = eIntersectionType.ByAngle) Or (TurnInterNavDat(K).IntersectionType = eIntersectionType.OnNavaid) Then
			OptionButton1101.Enabled = False
			OptionButton1102.Enabled = False

			TextBox1102.Visible = TurnInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid

			If TurnInterNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
				TextBox1102.Text = ""
				tipStr = "FIX " + My.Resources.str00106
				_Label1101_1.Text = tipStr
				_Label1101_13.Text = ""
			Else
				_Label1101_13.Text = "°"
				If TurnInterNavDat(K).TypeCode = eNavaidType.NDB Then
					_Label1101_1.Text = My.Resources.str00228
					Kmax = Modulus(TurnInterNavDat(K).ValMax(0) + 180.0 - TurnInterNavDat(K).MagVar, 360.0)
					Kmin = Modulus(TurnInterNavDat(K).ValMin(0) + 180.0 - TurnInterNavDat(K).MagVar, 360.0)
				Else
					_Label1101_1.Text = My.Resources.str00227
					Kmax = Modulus(TurnInterNavDat(K).ValMax(0) - TurnInterNavDat(K).MagVar, 360.0)
					Kmin = Modulus(TurnInterNavDat(K).ValMin(0) - TurnInterNavDat(K).MagVar, 360.0)
				End If
				tipStr = My.Resources.str00220 + My.Resources.str00221

				If TurnInterNavDat(K).ValCnt > 0 Then
					TextBox1102.Text = CStr(Kmin)
				Else
					TextBox1102.Text = CStr(Kmax)
				End If

				tipStr = tipStr + CStr(Kmin) + " °" + My.Resources.str00222 + CStr(Kmax) + " °"
			End If
		Else
			TextBox1102.Visible = True

			_Label1101_13.Text = DistanceConverter(DistanceUnit).Unit

			N = UBound(TurnInterNavDat(K).ValMin)

			OptionButton1101.Enabled = N > 0
			OptionButton1102.Enabled = N > 0

			If OptionButton1101.Checked Or (N = 0) Then
				TextBox1102.Text = CStr(ConvertDistance(TurnInterNavDat(K).ValMin(0) - TurnInterNavDat(K).Disp, eRoundMode.CEIL))
			Else
				TextBox1102.Text = CStr(ConvertDistance(TurnInterNavDat(K).ValMin(1) - TurnInterNavDat(K).Disp, eRoundMode.CEIL))
			End If

			If N = 0 Then
				If TurnInterNavDat(K).ValCnt > 0 Then
					OptionButton1101.Checked = True
				Else
					OptionButton1102.Checked = True
				End If
			End If

			_Label1101_1.Text = My.Resources.str00229
			tipStr = My.Resources.str10514

			For I = 0 To N
				tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(TurnInterNavDat(K).ValMin(I) - TurnInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TurnInterNavDat(K).ValMax(I) - TurnInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
				If I < N Then
					tipStr = My.Resources.str00230 + " - " + tipStr + vbCrLf + My.Resources.str00231 + " - "
				End If
			Next I
			'===
		End If

		_Label1101_2.Text = tipStr

		If TurnInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			tipStr = Replace(tipStr, vbCrLf, "   ")
			ToolTip1.SetToolTip(TextBox1102, tipStr)
		End If

		TextBox1102.Tag = "-"
		TextBox1102_Validating(TextBox1102, New System.ComponentModel.CancelEventArgs(True))
	End Sub

	Private Sub TextBox1101_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1101.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1101_Validating(TextBox1101, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1101.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1101_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1101.Validating
		Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pGeo As ESRI.ArcGIS.Geometry.IGeometry
		'====================================================
		Dim I As Integer

		Dim VTotal As Double
		Dim hTurn As Double
		Dim hCalc As Double
		Dim fTASl As Double
		Dim fTmp As Double
		Dim newH As Double
		Dim Dist As Double
		Dim iniH As Double
		Dim OCH As Double
		Dim Ls As Double
		Dim L0 As Double

		Dim Rv As Double
		Dim r0 As Double
		Dim E As Double

		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement

		If Not IsNumeric(TextBox1101.Text) Or OptionButton0902.Checked Then Return
		If ComboBox1101.SelectedIndex = 0 Then
			If IsNumeric(TextBox1101.Tag) Then
				fTmp = CDbl(TextBox1101.Tag)
				If TextBox1101.Text = CStr(ConvertHeight(fTmp + fRefHeight, eRoundMode.NEAREST)) Then Return
			End If
			iniH = DeConvertHeight(CDbl(TextBox1101.Text)) - fRefHeight
		Else
			If IsNumeric(TextBox1101.Tag) Then
				fTmp = CDbl(TextBox1101.Tag)
				If TextBox1101.Text = CStr(ConvertHeight(fTmp, eRoundMode.NEAREST)) Then Return
			End If
			iniH = DeConvertHeight(CDbl(TextBox1101.Text))
		End If

		newH = GetClosestTNH(iniH, CurrOCH, _CurrPDG)
		hTurn = newH + fRefHeight

		If newH < arMATurnAlt.Value Then
			hCalc = arMATurnAlt.Value + fRefHeight
		Else
			hCalc = hTurn
		End If
		'        Range = (newH - dpH_abv_DER.Value) / CurrPDG
		'        GetObstInRange PtInList, TmpInList, Range
		'        NonPrecReportFrm.FillPage31 TmpInList
		'============================================================
		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)
		VTotal = fTASl + CurrADHP.WindSpeed
		Ls = VTotal * arT_TechToleranc.Value * 0.277777777777778
		'============================================================
		TextBox1103.Text = CStr(ConvertSpeed(_missedIAS * 0.277777777777778, eRoundMode.NEAREST))
		TextBox1104.Text = CStr(ConvertSpeed(fTASl * 0.277777777777778, eRoundMode.NEAREST))

		TextBox1108.Text = CStr(ConvertHeight(hCalc, eRoundMode.NEAREST))
		TextBox1109.Text = CStr(ConvertDistance(Ls, eRoundMode.NEAREST))

		Rv = 6355.0 * System.Math.Tan(DegToRadValue * fBankAngle) / (PI * fTASl)
		If (Rv > 3.0) Then
			Rv = 3.0
		End If

		TextBox1105.Text = CStr(System.Math.Round(Rv, 2))

		Rv = System.Math.Round(Rv, 2)
		If (Rv > 0.0) Then
			r0 = fTASl / (0.02 * PI * Rv)
			TextBox1106.Text = CStr(ConvertDistance(r0, eRoundMode.NEAREST))
		Else
			r0 = -1
			TextBox1106.Text = "-"
		End If

		E = 25.0 * CurrADHP.WindSpeed / Rv
		TextBox1107.Text = CStr(ConvertDistance(E, eRoundMode.NEAREST))
		'==============================
		pClone = pFullPoly
		pPolyClone = pClone.Clone

		pTopo = pPolyClone
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		m_pFixPoly = pClone.Clone
		ZNR_Poly = pClone.Clone
		'====================
		pLine = New ESRI.ArcGIS.Geometry.Polyline
		ptTmp = PointAlongPlane(pMAPt, _ArDir + 180.0, dNearMAPt)

		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir - 90, RModel))
		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir + 90, RModel))
		'DrawPolyLine pLine, 255
		'DrawPoint MAPtPrj, 255
		CutPoly(m_pFixPoly, pLine, -1)
		pGeo = m_pFixPoly
		If pGeo.IsEmpty() Then m_pFixPoly = pClone.Clone

		KKhMin = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If SideDef(KKhMin.ToPoint, _ArDir, KKhMin.FromPoint) < 0 Then
			KKhMin.ReverseOrientation()
		End If
		'==========================
		CutPoly(ZNR_Poly, pLine, -1)
		pGeo = ZNR_Poly
		If pGeo.IsEmpty() Then ZNR_Poly = pClone.Clone
		'        DrawPolygon pFIXPoly, 0
		'        DrawPolyLine pLine, 255
		'        DrawPolygon pPolyClone, 0
		OCH = OverOCH
		L0 = (newH - OCH) / _CurrPDG
		ptTmp = PointAlongPlane(PtSOC, _ArDir, L0)

		'        Set pLine = New Polyline
		pLine.RemovePoints(0, 2)
		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir - 90.0, RModel))
		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir + 90.0, RModel))

		CutPoly(m_pFixPoly, pLine, 1)
		CutPoly(ZNR_Poly, pLine, 1)

		KK = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If SideDef(KK.ToPoint, _ArDir, KK.FromPoint) < 0 Then
			KK.ReverseOrientation()
		End If
		'==========================
		TurnFixPnt = New ESRI.ArcGIS.Geometry.Point
		pConstruct = TurnFixPnt
		pConstruct.ConstructAngleIntersection(PtSOC, DegToRad(_ArDir), KK.ToPoint, DegToRad(_ArDir + 90.0))
		TurnFixPnt.Z = hTurn
		TurnFixPnt.M = _ArDir

		pLine.RemovePoints(0, 2)
		ptTmp = PointAlongPlane(PtSOC, _ArDir, L0 + Ls)
		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir - 90.0, RModel))
		pLine.AddPoint(PointAlongPlane(ptTmp, _ArDir + 90.0, RModel))

		'DrawPolyLine pLine, 0
		'    CutPoly ZNR_Poly, pLine, 1
		'DrawPolygon pFIXPoly, 0

		K1K1 = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If SideDef(K1K1.ToPoint, _ArDir, K1K1.FromPoint) < 0 Then K1K1.ReverseOrientation()

		KKFixMax = KK
		'============================================================
		TextBox1101.Tag = CStr(newH)
		If ComboBox1101.SelectedIndex = 0 Then
			TextBox1101.Text = CStr(ConvertHeight(newH + fRefHeight, eRoundMode.NEAREST))
		Else
			TextBox1101.Text = CStr(ConvertHeight(newH, eRoundMode.NEAREST))
		End If
		'============================================================
		On Error Resume Next
		If Not FIXElem Is Nothing Then
			pGroupElement = FIXElem
			For I = 0 To pGroupElement.ElementCount - 1
				If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
			Next I
		End If

		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement

		pGroupElement.AddElement(DrawPolygon(m_pFixPoly, 0, , False))
		pGroupElement.AddElement(DrawPointWithText(TurnFixPnt, "TP", WPTColor, False))
		FIXElem = pGroupElement
		'DrawPolygon pFIXPoly, RGB(128, 128, 0)
		'DrawPoint TurnFixPnt, 255

		For I = 0 To pGroupElement.ElementCount - 1
			pGraphics.AddElement(pGroupElement.Element(I), 0)
			pGroupElement.Element(I).Locked = True
		Next I
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		'        If ButtonControl5State Then
		'        If True Then
		'            pGraphics.AddElement StrTrackElem, 0
		'            StrTrackElem.Locked = True
		'        End If

		'        RefreshCommandBar mTool, 128
		On Error GoTo 0

		While ArrivalProfile.PointsNo > ArrivalProfile.MAPtIndex + 1
			ArrivalProfile.RemovePoint()
		End While

		Dist = Point2LineDistancePrj(FictTHR, TurnFixPnt, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, TurnFixPnt)
		ArrivalProfile.AddPoint(Dist, newH, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, -1, 1)
	End Sub

	Private Sub TextBox1102_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1102.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1102_Validating(TextBox1102, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1102.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1102_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1102.Validating
		Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
		Dim Clone As ESRI.ArcGIS.esriSystem.IClone

		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline

		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

		Dim DepDistMax As Double
		Dim DepDistMin As Double
		Dim InterToler As Double
		Dim TrackToler As Double
		Dim VTotal As Double
		'Dim hTurn As Double
		Dim hCalc As Double
		Dim fTASl As Double
		Dim hFix As Double
		Dim fDirl As Double
		Dim fDis As Double
		Dim dMin As Double
		Dim dMax As Double
		Dim OCH As Double
		Dim Ls As Double
		Dim d0 As Double
		Dim Rv As Double
		Dim dD As Double
		Dim D As Double

		Dim I As Integer
		Dim K As Integer

		K = ComboBox1102.SelectedIndex

		If TurnInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			If TextBox1102.Tag = TextBox1102.Text Then Return
			If IsNumeric(TextBox1102.Text) Then
				fDirl = CDbl(TextBox1102.Text)
			Else 'If (TurnInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid) Then
				Return
			End If
		End If

		OCH = OverOCH

		If Not FIXElem Is Nothing Then
			pGroupElement = FIXElem
			For I = 0 To pGroupElement.ElementCount - 1
				SafeDeleteElement(pGroupElement.Element(I))
			Next I
		End If

		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement

		If Not (pFIXAreaPolygon Is Nothing) Then
			Dim pElem As ESRI.ArcGIS.Carto.IElement
			pElem = DrawPolygon(pFIXAreaPolygon, RGB(195, 195, 195), , False)
			pGroupElement.AddElement(pElem)
		End If

		Select Case TurnInterNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				Clone = TurnInterNavDat(K).pPtPrj
				TurnFixPnt = Clone.Clone

				D = Point2LineDistancePrj(PtSOC, TurnInterNavDat(K).pPtPrj, _ArDir + 90.0) * SideDef(PtSOC, _ArDir + 90.0, TurnInterNavDat(K).pPtPrj)
				If D < 0.0 Then D = 0.0
				hFix = fRefHeight + OCH + D * _CurrPDG - TurnInterNavDat(K).pPtPrj.Z
				If hFix < arMATurnAlt.Value Then hFix = arMATurnAlt.Value

				OnNavaidFIXTolerArea(TurnInterNavDat(K), _ArDir, hFix + fRefHeight, pSect0)
				'If TurnInterNavDat(K).TypeCode = eNavaidType.NDB Then
				'	NDBFIXTolerArea(TurnInterNavDat(K).pPtPrj, m_ArDir, hFix + fRefHeight, pSect0)
				'Else
				'	VORFIXTolerArea(TurnInterNavDat(K).pPtPrj, m_ArDir, hFix + fRefHeight, pSect0)
				'End If
				'pTopo = pSect0
				'pTopo.IsKnownSimple_2 = False
				'pTopo.Simplify()

				pGroupElement.AddElement(DrawPolygon(pSect0, RGB(0, 0, 255), , False))

				DepDistMax = RModel
				DepDistMin = 0.0
			Case eIntersectionType.ByAngle
				If (TurnInterNavDat(K).TypeCode = eNavaidType.VOR) Or (TurnInterNavDat(K).TypeCode = eNavaidType.TACAN) Then
					InterToler = VOR.IntersectingTolerance                  '                fDis = VOR.Range
				ElseIf TurnInterNavDat(K).TypeCode = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance                  '                fDis = NDB.Range
					fDirl = fDirl + 180.0
				ElseIf TurnInterNavDat(K).TypeCode = eNavaidType.LLZ Then
					InterToler = LLZ.IntersectingTolerance
				End If

				fDis = TurnInterNavDat(K).Range
				fDirl = Azt2Dir(TurnInterNavDat(K).pPtGeo, fDirl + TurnInterNavDat(K).MagVar)

				TurnFixPnt = New ESRI.ArcGIS.Geometry.Point
				pConstruct = TurnFixPnt
				pConstruct.ConstructAngleIntersection(TurnInterNavDat(K).pPtPrj, DegToRad(fDirl), PtSOC, DegToRad(_ArDir))

				pSect0 = New ESRI.ArcGIS.Geometry.Polyline
				pt1 = PointAlongPlane(TurnInterNavDat(K).pPtPrj, fDirl + InterToler, fDis)
				pt2 = PointAlongPlane(TurnInterNavDat(K).pPtPrj, fDirl - InterToler, fDis)

				pSect0.AddPoint(pt1)
				pSect0.AddPoint(TurnInterNavDat(K).pPtPrj)
				pSect0.AddPoint(pt2)

				pGroupElement.AddElement(DrawPolyLine(pSect0, RGB(0, 0, 255), 1, False))
				DepDistMax = RModel
				DepDistMin = 0.0
			Case eIntersectionType.ByDistance
				fDirl = DeConvertDistance(fDirl) + TurnInterNavDat(K).Disp

				If (TurnInterNavDat(K).ValCnt < 0) Or (OptionButton1102.Enabled And OptionButton1102.Checked) Then
					CircleVectorIntersect(TurnInterNavDat(K).pPtPrj, fDirl, PtSOC, _ArDir, TurnFixPnt)
				Else
					CircleVectorIntersect(TurnInterNavDat(K).pPtPrj, fDirl, PtSOC, _ArDir + 180.0, TurnFixPnt)
				End If

				fDis = Point2LineDistancePrj(TurnFixPnt, PtSOC, _ArDir + 90.0)
				'       hFix = fDis * dpOv_Nav_PDG.Value + dpH_abv_DER.Value - TurnInterNavDat(k).pPtGeo.Z + ptDerPrj.Z
				hFix = fDis * _CurrPDG + OCH + fRefHeight - TurnInterNavDat(K).pPtPrj.Z

				d0 = System.Math.Sqrt(fDirl * fDirl + hFix * hFix) * DME.ErrorScalingUp + DME.MinimalError

				D = fDirl + d0
				pSect0 = CreatePrjCircle(TurnInterNavDat(K).pPtPrj, D)

				pCutter = New ESRI.ArcGIS.Geometry.Polyline
				pCutter.FromPoint = PointAlongPlane(TurnInterNavDat(K).pPtPrj, _ArDir - 90.0, D + D)
				pCutter.ToPoint = PointAlongPlane(TurnInterNavDat(K).pPtPrj, _ArDir + 90.0, D + D)

				D = fDirl - d0
				pSect1 = CreatePrjCircle(TurnInterNavDat(K).pPtPrj, D)

				pTopo = pSect0
				pPoly = pTopo.Difference(pSect1)

				pTopo = pPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				If SideDef(pCutter.FromPoint, _ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

				If (TurnInterNavDat(K).ValCnt < 0) Or (OptionButton1102.Enabled And OptionButton1102.Checked) Then
					pTopo.Cut(pCutter, pSect1, pSect0)
				Else
					pTopo.Cut(pCutter, pSect0, pSect1)
				End If

				pTopo = pSect0
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pGroupElement.AddElement(DrawPolygon(pSect0, RGB(0, 0, 255), , False))

				DepDistMin = -10.0        '(hMinTurn - dpH_abv_DER.Value) / CurrPDG - 10.0
				DepDistMax = (hMaxTurn - hMinTurn) / _CurrPDG + 10.0
		End Select

		pTopo = pSect0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Clone = pFullPoly
		If FinalNav.TypeCode > eNavaidType.NONE Then
			If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
				TrackToler = VOR.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
				TrackToler = NDB.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
				TrackToler = LLZ.TrackingTolerance
			End If

			pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, 3.0 * RModel))
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			'Set pFIXPoly = Clone.Clone
		Else
			'Set Clone = pPolygon
			'Set pPolygon1 = Clone.Clone
			pPolyClone = Clone.Clone
		End If

		m_pFixPoly = Clone.Clone

		pTopo = pPolyClone
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If (pLine.PointCount = 0) And (TurnInterNavDat(K).IntersectionType <> eIntersectionType.ByAngle) Then
			pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
		End If

		dMax = -RModel
		dMin = RModel

		For I = 0 To pLine.PointCount - 1
			D = Point2LineDistancePrj(PtSOC, pLine.Point(I), _ArDir + 90.0) * SideDef(PtSOC, _ArDir + 90.0, pLine.Point(I))
			If D < dMin Then
				dMin = D
				pt1 = pLine.Point(I)
			End If

			If D > dMax Then
				dMax = D
				pt2 = pLine.Point(I)
			End If
		Next

		If dMax <= dMin Then
			MessageBox.Show(_Label1101_1.Text + My.Resources.str00302)  '        msgbox "Неверное значение параметра."
			Return
		End If

		_bTurnFIXSameMAPtFIX = dMax < 0.0
		pLine = New ESRI.ArcGIS.Geometry.Polyline

		If _bTurnFIXSameMAPtFIX Then
			pt2 = PtSOC
			ZNR_Poly = Clone.Clone
			pLine.AddPoint(PointAlongPlane(pMAPt, _ArDir + 90.0, RModel))
			pLine.AddPoint(PointAlongPlane(pMAPt, _ArDir - 90.0, RModel))
			CutPoly(ZNR_Poly, pLine, 1)
			pLine.RemovePoints(0, 2)
			'========================================
			_Label1201_11.Visible = True
			_Label1201_4.Visible = True
			_Label1201_6.Visible = True
			_Label1201_9.Visible = True
			_Label1201_2.Visible = True
			_Label1201_7.Visible = True
			_Label1201_10.Visible = True
			_Label1301_1.Visible = False

			'        TextBox1203.ToolTipText = RecommendStr
			TextBox1205.Visible = True
			TextBox1204.Visible = True
			TextBox1202.Visible = True
			TextBox1301_1.Visible = False

			OptionButton1302.Visible = True
			OptionButton1305.Visible = True
			OptionButton1306.Visible = False
			'========================================
		Else
			'========================================
			_Label1201_11.Visible = False
			_Label1201_4.Visible = False
			_Label1201_6.Visible = False
			_Label1201_9.Visible = False
			_Label1201_2.Visible = False
			_Label1201_7.Visible = False
			_Label1201_10.Visible = False
			_Label1301_1.Visible = True

			'        TextBox1203.ToolTipText = ""
			TextBox1205.Visible = False
			TextBox1204.Visible = False
			TextBox1202.Visible = False
			TextBox1301_1.Visible = True
			'========================================
			OptionButton1302.Visible = False
			OptionButton1305.Visible = False
			OptionButton1306.Visible = True
			OptionButton1305.Checked = False
		End If

		pTopo = pFullPoly

		pLine.AddPoint(PointAlongPlane(pt1, _ArDir + 90.0, RModel))
		pLine.AddPoint(PointAlongPlane(pt1, _ArDir - 90.0, RModel))

		KK = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If SideDef(PtSOC, _ArDir, KK.FromPoint) < 0 Then KK.ReverseOrientation()
		'==========================
		'    fDis = dMin 'Point2LineDistancePrj(PtSOC, pt1, ArDir + 90.0)
		fDis = IIf(dMin > 0, dMin, 0)

		Dim Hkk As Double

		Hkk = OCH + fDis * _CurrPDG
		TextBox1101.Tag = Hkk 'CStr(OCH + fDis * _CurrPDG)
		If ComboBox1101.SelectedIndex = 0 Then
			TextBox1101.Text = CStr(ConvertHeight(Hkk + fRefHeight, eRoundMode.NEAREST))
		Else
			TextBox1101.Text = CStr(ConvertHeight(Hkk, eRoundMode.NEAREST))
		End If

		D = Point2LineDistancePrj(PtSOC, TurnFixPnt, _ArDir + 90.0)

		TurnFixPnt.Z = D * _CurrPDG + OCH + fRefHeight
		TurnFixPnt.M = _ArDir

		CutPoly(m_pFixPoly, pLine, 1)

		pLine.RemovePoints(0, pLine.PointCount)
		pLine.AddPoint(PointAlongPlane(pt2, _ArDir + 90.0, RModel))
		pLine.AddPoint(PointAlongPlane(pt2, _ArDir - 90.0, RModel))

		KKFixMax = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If SideDef(PtSOC, _ArDir, KKFixMax.FromPoint) < 0 Then
			KKFixMax.ReverseOrientation()
		End If

		CutPoly(m_pFixPoly, pLine, -1)
		pTPTolerArea = m_pFixPoly

		If _bTurnFIXSameMAPtFIX Then CutPoly(ZNR_Poly, pLine, -1)
		'====
		'IAS = CDbl(TextBox0902.Text)
		hCalc = arMATurnAlt.Value + fRefHeight
		If TurnFixPnt.Z > hCalc Then hCalc = TurnFixPnt.Z

		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)
		VTotal = fTASl + CurrADHP.WindSpeed
		Ls = VTotal * arT_TechToleranc.Value * 0.277777777777778
		'============================================

		TextBox1103.Text = CStr(ConvertSpeed(_missedIAS * 0.277777777777778))
		TextBox1104.Text = CStr(ConvertSpeed(fTASl * 0.277777777777778))

		TextBox1108.Text = CStr(ConvertHeight(hCalc))
		TextBox1109.Text = CStr(ConvertDistance(Ls))

		Rv = 6355.0 * System.Math.Tan(DegToRadValue * fBankAngle) / (PI * fTASl)
		If (Rv > 3.0) Then
			Rv = 3.0
		End If

		TextBox1105.Text = CStr(System.Math.Round(Rv, 2))

		Rv = System.Math.Round(Rv, 2)
		If (Rv > 0.0) Then
			d0 = fTASl / (0.02 * PI * Rv)
			TextBox1106.Text = CStr(ConvertDistance(d0, eRoundMode.NEAREST))
		Else
			'        r0 = -1
			TextBox1106.Text = "-"
		End If

		d0 = 25.0 * CurrADHP.WindSpeed / Rv
		TextBox1107.Text = CStr(ConvertDistance(d0, eRoundMode.NEAREST))

		pProxi = KK
		TextBox1110.Text = CStr(ConvertDistance(pProxi.ReturnDistance(KKFixMax), eRoundMode.NEAREST))
		'=============================================
		d0 = Point2LineDistancePrj(PtSOC, KKFixMax.FromPoint, _ArDir + 90.0)
		pt1 = PointAlongPlane(PtSOC, _ArDir, d0 + Ls)

		pLine.RemovePoints(0, pLine.PointCount)

		pLine.AddPoint(PointAlongPlane(pt1, _ArDir + 90.0, RModel))
		pLine.AddPoint(PointAlongPlane(pt1, _ArDir - 90.0, RModel))

		K1K1 = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If SideDef(K1K1.ToPoint, _ArDir, K1K1.FromPoint) < 0 Then
			K1K1.ReverseOrientation()
		End If
		'=============================================
		pGroupElement.AddElement(DrawPolygon(m_pFixPoly, 0, , False))
		pGroupElement.AddElement(DrawPointWithText(TurnFixPnt, "TP", WPTColor, False))
		FIXElem = pGroupElement

		'    pGraphics.DeleteElement StrTrackElem
		'    Set StrTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

		'	If ButtonControl8State Then
		For I = 0 To pGroupElement.ElementCount - 1
			pGraphics.AddElement(pGroupElement.Element(I), 0)
			pGroupElement.Element(I).Locked = True
		Next I
		'	End If

		'	If ButtonControl5State Then
		'		pGraphics.AddElement StrTrackElem, 0
		'		StrTrackElem.Locked = True
		'	End If

		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		'	RefreshCommandBar mTool, 128

		'=================================================
		TextBox1102.Tag = TextBox1102.Text

		While ArrivalProfile.PointsNo > ArrivalProfile.MAPtIndex + 1
			ArrivalProfile.RemovePoint()
		End While

		dD = Point2LineDistancePrj(FictTHR, TurnFixPnt, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, TurnFixPnt)
		ArrivalProfile.AddPoint(dD, Hkk, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, -1, 1)   'TurnFixPnt.Z - fRefHeight
	End Sub

#End Region

#Region "Page 13"

	Private Sub CheckBox1201_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1201.CheckedChanged
		If Not bFormInitialised Then Return

		ComboBox1201_SelectedIndexChanged(ComboBox1201, New System.EventArgs())
	End Sub

	Private Sub OptionButton1201_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton1201.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		ComboBox1201.Visible = False
		ComboBox1202.Visible = False
		ComboBox1203.Visible = False

		CheckBox1201.Visible = False

		_Label1201_1.Visible = False
		_Label1201_2.Visible = False
		_Label1201_3.Visible = False
		_Label1201_4.Visible = False
		_Label1201_6.Visible = False
		_Label1201_7.Visible = False
		_Label1201_9.Visible = False
		_Label1201_10.Visible = False
		_Label1201_11.Visible = False
		_Label1201_12.Visible = False

		TextBox1202.Visible = False

		TextBox1203.ReadOnly = False
		TextBox1203.BackColor = System.Drawing.SystemColors.Window
		ToolTip1.SetToolTip(TextBox1203, "")
		TextBox1203.Tag = "a"

		'TextBox1505.ReadOnly = False
		'TextBox1505.BackColor = System.Drawing.SystemColors.Window

		TextBox1204.Visible = False
		TextBox1205.Visible = False
		Frame1501.Visible = False

		TextBox1203_Validating(TextBox1203, New System.ComponentModel.CancelEventArgs(True))
	End Sub

	Private Sub OptionButton1202_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton1202.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		_Label1201_1.Text = My.Resources.str13011
		_Label1201_1.Visible = True
		_Label1201_2.Visible = True
		_Label1201_3.Visible = True
		_Label1201_4.Visible = True
		_Label1201_6.Visible = True
		_Label1201_7.Visible = True
		_Label1201_9.Visible = True
		_Label1201_10.Visible = True
		_Label1201_11.Visible = True
		_Label1201_12.Visible = True
		ComboBox1201.Visible = True
		ComboBox1202.Visible = True
		ComboBox1203.Visible = True
		ComboBox1203.Enabled = UBound(WPTList) > 0

		TextBox1202.Visible = True

		TextBox1203.ReadOnly = False
		TextBox1203.BackColor = System.Drawing.SystemColors.Window
		ToolTip1.SetToolTip(TextBox1203, RecommendStr)
		TextBox1203.Tag = "a"

		'TextBox1505.ReadOnly = True
		'TextBox1505.BackColor = System.Drawing.SystemColors.Control

		TextBox1204.Visible = True
		TextBox1205.Visible = True
		Frame1501.Visible = True

		CheckBox1201.Visible = False
		'=======
		'    CheckBox806.Visible = False
		'    CheckBox806.Value = 0
		'    CheckBox806.Value = False

		OptionButton1302.Visible = True
		OptionButton1305.Visible = True
		OptionButton1306.Visible = False

		_Label1301_1.Visible = False
		TextBox1301_1.Visible = False
		'=======
		ComboBox1201.Items.Clear()
		N = UBound(FixAngl)
		J = -1
		ReDim SelectedFixAngl(N)

		If N >= 0 Then
			For I = 0 To N
				If ReturnDistanceInMeters(TurnFixPnt, FixAngl(I).pPtPrj) < RModel Then
					J = J + 1
					SelectedFixAngl(J) = FixAngl(I)
					ComboBox1201.Items.Add(SelectedFixAngl(J).Name)
				End If
			Next I
		End If

		If J >= 0 Then
			ReDim Preserve SelectedFixAngl(J)
			ComboBox1201.Enabled = True
			ComboBox1201.SelectedIndex = 0
		Else
			ComboBox1201.Enabled = False
		End If
	End Sub

	Private Sub OptionButton1203_CheckedChanged(ByVal eventSender As System.Object, ByVal e As System.EventArgs) Handles OptionButton1203.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		ComboBox1201.Visible = True
		ComboBox1202.Visible = False
		ComboBox1203.Visible = False

		CheckBox1201.Visible = Not _bTurnFIXSameMAPtFIX

		_Label1201_1.Text = My.Resources.str13013
		_Label1201_1.Visible = True
		_Label1201_2.Visible = False
		_Label1201_3.Visible = True
		_Label1201_4.Visible = False
		_Label1201_6.Visible = False
		_Label1201_7.Visible = False
		_Label1201_9.Visible = False
		_Label1201_10.Visible = False
		_Label1201_11.Visible = False
		_Label1201_12.Visible = False

		OptionButton1302.Visible = False
		OptionButton1305.Visible = False
		OptionButton1306.Visible = True

		TextBox1202.Visible = False

		TextBox1203.ReadOnly = True
		TextBox1203.BackColor = System.Drawing.SystemColors.Control
		ToolTip1.SetToolTip(TextBox1203, "")
		TextBox1203.Tag = "a"

		TextBox1204.Visible = False
		TextBox1205.Visible = False
		Frame1501.Visible = False

		'TextBox1505.ReadOnly = True
		'TextBox1505.BackColor = System.Drawing.SystemColors.Control

		ComboBox1201.Items.Clear()
		N = UBound(WPTList)
		J = -1
		If N >= 0 Then
			ReDim SelectedFixAll(N)
			For I = 0 To N
				If ReturnDistanceInMeters(CurrADHP.pPtPrj, WPTList(I).pPtPrj) < RModel Then
					J = J + 1
					SelectedFixAll(J) = WPTList(I)
					ComboBox1201.Items.Add(WPTList(I).Name)
				End If
			Next I
		End If

		If J >= 0 Then
			ReDim Preserve SelectedFixAll(J)
			ComboBox1201.Enabled = True

			If _bTurnFIXSameMAPtFIX Then
				UpdateIntervals()
				_Label1201_7.Visible = True
			End If
			ComboBox1201.SelectedIndex = 0
		Else
			ComboBox1201.Enabled = False
		End If
	End Sub

	Private Sub ComboBox1201_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1201.SelectedIndexChanged
		If Not bFormInitialised Then Return

		If ComboBox1201.Items.Count < 1 Then Return
		If MultiPage1.SelectedIndex < 11 Then Return

		If OptionButton1203.Checked Then
			TurnDirector = SelectedFixAll(ComboBox1201.SelectedIndex)
		ElseIf OptionButton1202.Checked Then
			TurnDirector = SelectedFixAngl(ComboBox1201.SelectedIndex)
		Else
			Return
		End If

		_Label1201_3.Text = GetNavTypeName(TurnDirector.TypeCode)

		If OptionButton1202.Checked Then
			UpdateIntervals(True)

			If ComboBox1203.SelectedIndex = 0 Then
				ComboBox1203_SelectedIndexChanged(ComboBox1203, New EventArgs())
			Else
				ComboBox1203.SelectedIndex = 0
			End If
		ElseIf _bTurnFIXSameMAPtFIX Then
			Dim TurnNav As NavaidData
			Dim K As Integer

			K = ComboBox1102.SelectedIndex
			TurnNav = TurnInterNavDat(K)

			_OutDir = ReturnAngleInDegrees(TurnFixPnt, TurnDirector.pPtPrj)
			UpdateToNavCourse(_OutDir, TurnNav)
		ElseIf OptionButton1203.Checked Then
			If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.NDB) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
				CheckBox1201.Enabled = True
			Else
				CheckBox1201.Checked = False
				CheckBox1201.Enabled = False
			End If
			UpdateToFix()
		End If
	End Sub

	Private Sub ComboBox1202_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1202.SelectedIndexChanged
		Dim K As Integer
		Dim BoolRes As Boolean
		Dim TurnNav As NavaidData

		K = ComboBox1202.SelectedIndex
		If K < 0 Then Return
		If K = CInt(ComboBox1202.Tag) Then Return

		WPt1202 = FixBox1202(K)

		_OutDir = ReturnAngleInDegrees(TurnDirector.pPtPrj, WPt1202.pPtPrj)

		TurnNav = WPT_FIXToNavaid(TurnDirector)

		BoolRes = UpdateToNavCourse(_OutDir, TurnNav) = -1

		If Not BoolRes Then
			TextBox1203.Tag = CStr(ConvertAngle(Modulus(Dir2Azt(TurnDirector.pPtPrj, _OutDir) - TurnDirector.MagVar)))
			TextBox1203.Text = TextBox1203.Tag
		End If

		If (Not BoolRes) Or (CInt(ComboBox1202.Tag) = -1) Then
			ComboBox1202.Tag = ComboBox1202.SelectedIndex
		Else
			ComboBox1202.SelectedIndex = ComboBox1202.Tag
		End If
	End Sub

	Private Sub ComboBox1203_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1203.SelectedIndexChanged
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		If OptionButton1202.Checked Then
			If ComboBox1203.SelectedIndex = 0 Then
				TextBox1203.ReadOnly = False
				'TextBox1203.BackColor = System.Drawing.SystemColors.Window

				ComboBox1202.Enabled = False
				TextBox1203.Tag = "a"
				TextBox1203_Validating(TextBox1203, New CancelEventArgs())
			Else
				N = UBound(WPTList)
				If N >= 0 Then
					Dim OldWPT As WPT_FIXType
					Dim k As Integer
					k = ComboBox1202.SelectedIndex
					If k >= 0 Then
						OldWPT.Identifier = WPTList(k).Identifier
					End If

					TextBox1203.ReadOnly = True
					'TextBox1203.BackColor = System.Drawing.SystemColors.Control
					ComboBox1202.Enabled = True

					ComboBox1202.Tag = "-1"
					ComboBox1202.Items.Clear()

					ReDim FixBox1202(N)
					J = -1
					k = 0
					For I = 0 To N
						If WPTList(I).Name <> ComboBox1201.Text Then
							J = J + 1
							FixBox1202(J) = WPTList(I)
							ComboBox1202.Items.Add(WPTList(I).Name)
							If OldWPT.Identifier = WPTList(k).Identifier Then k = J
						End If
					Next I

					If J >= 0 Then
						ReDim Preserve FixBox1202(J)
						ComboBox1202.SelectedIndex = k
					Else
						ComboBox1203.Enabled = False
						ComboBox1203.SelectedIndex = 0
					End If
				Else
					ComboBox1203.Enabled = False
					ComboBox1203.SelectedIndex = 0
				End If
			End If
		End If
	End Sub

	Private Sub TextBox1203_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1203.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1203_Validating(TextBox1203, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1203.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1203_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1203.Validating
		Dim TurnNav As NavaidData

		If (Not IsNumeric(TextBox1203.Text)) And IsNumeric(TextBox1203.Tag) Then TextBox1203.Text = TextBox1203.Tag
		If (MultiPage1.SelectedIndex < 10) Or (TextBox1203.Tag = TextBox1203.Text) Or (Not IsNumeric(TextBox1203.Text)) Then Return

		If OptionButton1201.Checked Then
			_OutDir = Azt2Dir(ToGeo(TurnFixPnt), CDbl(TextBox1203.Text) + CurrADHP.MagVar)
		Else
			_OutDir = Azt2Dir(TurnDirector.pPtGeo, CDbl(TextBox1203.Text) + TurnDirector.MagVar)
		End If

		If OptionButton1202.Checked Then
			TurnNav = WPT_FIXToNavaid(TurnDirector)
			UpdateToNavCourse(_OutDir, TurnNav)
		ElseIf OptionButton1201.Checked Then
			UpdateToCourse(_OutDir)
		End If

		TextBox1203.Tag = TextBox1203.Text
	End Sub

	Private Sub TextBox1204_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1204.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1204_Validating(TextBox1204, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxInteger(eventArgs.KeyChar)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1204_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1204.Validating
		Dim fTmp As Double
		If Not IsNumeric(TextBox1204.Text) Then Return

		fTmp = CDbl(TextBox1204.Text)

		If fTmp < 2.0 Then
			TextBox1204.Text = "2"
		ElseIf fTmp > 75.0 Then       'arImMaxIntercept.Value
			TextBox1204.Text = arImMaxIntercept.Value.ToString()
		End If

		If TextBox1204.Text = TextBox1204.Tag Then Return

		UpdateIntervals((Not OptionButton1202.Checked) Or (ComboBox1203.SelectedIndex = 0))

		If OptionButton1202.Checked And (ComboBox1203.SelectedIndex = 1) Then
			ComboBox1202.Tag = "-1"
			ComboBox1202_SelectedIndexChanged(ComboBox1202, New EventArgs())
		End If
		TextBox1204.Tag = TextBox1204.Text
	End Sub

	Private Sub TextBox1205_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1205.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1205_Validating(TextBox1205, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1205.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1205_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1205.Validating
		If Not IsNumeric(TextBox1205.Text) Then Return
		If TextBox1205.Text = TextBox1205.Tag Then Return

		TextBox1205.Tag = TextBox1205.Text
		If OptionButton1202.Checked Then UpdateIntervals(True)
	End Sub

#End Region

#Region "Page 14"
	Private Sub ApplayOptions()
		Dim pNewPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim TurnNav As NavaidData

		If MultiPage1.SelectedIndex < 12 Then Return
		If OptionButton1201.Checked Then Return

		TurnNav = TurnWPTToTurnNav(_bTurnFIXSameMAPtFIX, MAPtNavDat(ComboBox0801.SelectedIndex), TurnDirector)
		CreateNavaidZone(TurnNav, _OutDir, FictTHR, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

		If (OptionButton1203.Checked And (TurnNav.TypeCode = eNavaidType.VOR Or TurnNav.TypeCode = eNavaidType.NDB Or TurnNav.TypeCode = eNavaidType.TACAN)) Then
			pNewPoly = ApplayJoining(TurnNav.TypeCode, TurnDir, m_TurnArea, m_OutPt, _OutDir)

			pNewPoly = RemoveAgnails(pNewPoly)

			pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon
			pTmpPoly.AddPointCollection(m_BasePoints)

			pTopoOper = pTmpPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pNewPoly = pTopoOper.Union(pNewPoly)
			pTopoOper = pNewPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			BaseArea = RemoveHoles(pTopoOper.Union(pNewPoly))

			'If OptionButton0901.Checked Then
			'	pTopoOper = BaseArea
			'	BaseArea = pTopoOper.Difference(ZNR_Poly)
			'End If

			'pTopoOper = pCircle
			'BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
			BaseArea = PolygonIntersection(pCircle, BaseArea)
			'================================================================
			Try
				If Not TurnAreaElem Is Nothing Then pGraphics.DeleteElement(TurnAreaElem)
			Catch
			End Try

			Try
				If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
			Catch
			End Try

			Try
				If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
			Catch
			End Try

			Try
				If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
			Catch
			End Try
			Try
				If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
			Catch
			End Try

			TurnAreaElem = DrawPolygon(BaseArea, 255, , False)

			pTmpPoly = PolygonIntersection(pCircle, Prim)
			'    Set tmpPoly1 = pTopoOper.Intersect(Prim, esriGeometry2Dimension)
			PrimElem = DrawPolygon(pTmpPoly, 0, , False)

			pTmpPoly = PolygonIntersection(pCircle, SecL)
			'    Set tmpPoly1 = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
			SecLElem = DrawPolygon(pTmpPoly, RGB(0, 0, 255), , False)

			pTmpPoly = PolygonIntersection(pCircle, SecR)
			'    Set tmpPoly1 = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
			SecRElem = DrawPolygon(pTmpPoly, RGB(0, 0, 255), , False)

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
		ElseIf OptionButton1202.Checked Then  'bTurnFIXSameMAPtFIX
			UpdateToNavCourse(_OutDir, TurnNav) 'Azt2Dir(FinalNav.pPtGeo, CDbl(TextBox1203.Text) + FinalNav.MagVar), TurnNav
		End If
	End Sub

	Private Sub OptionButton1301_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton1301.CheckedChanged, OptionButton1302.CheckedChanged,
													OptionButton1303.CheckedChanged, OptionButton1304.CheckedChanged, OptionButton1305.CheckedChanged, OptionButton1306.CheckedChanged
		If Not bFormInitialised Then Return
		If Not eventSender.Checked Then Return

		ApplayOptions()
	End Sub

	'Private Sub TextBox1301_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles _TextBox1301_0.KeyPress, _TextBox1301_1.KeyPress
	'	TextBoxInteger(eventArgs.KeyChar)
	'	If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	'End Sub

#End Region

#Region "Page 15"

	Private Sub SecondArea(ByVal iTurnDir As Integer, ByVal pTurnArea As ESRI.ArcGIS.Geometry.IPointCollection)
		Dim bFlg As Boolean
		Dim SelfFIX As Boolean

		Dim Side0 As Integer
		Dim Side1 As Integer
		Dim SideIn As Integer

		Dim DrDir As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim InDist As Double
		Dim TmpDir As Double
		Dim OutDist As Double
		Dim fTmpAzt As Double
		Dim InitWidth As Double
		Dim TmpDepDir As Double
		Dim SplayAngle As Double

		Dim ptFar As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCut As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCutOuter As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCutInner As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCutInner0 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCutInner1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptLeftStriction As ESRI.ArcGIS.Geometry.IPoint
		Dim ptRightStriction As ESRI.ArcGIS.Geometry.IPoint

		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pIArea As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim GeneralArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim TurnNav As NavaidData

		TurnNav = TurnWPTToTurnNav(_bTurnFIXSameMAPtFIX, MAPtNavDat(ComboBox0801.SelectedIndex), TurnDirector)

		ptOut = MPtCollection.Point(MPtCollection.PointCount - 1)
		If _bTurnFIXSameMAPtFIX Or OptionButton1202.Checked Then DrDir = MPtCollection.Point(1).M

		If ((TurnNav.TypeCode = eNavaidType.VOR) Or (TurnNav.TypeCode = eNavaidType.NDB) Or (TurnNav.TypeCode = eNavaidType.TACAN)) And (OptionButton1202.Checked Or OptionButton1203.Checked Or _bTurnFIXSameMAPtFIX) Then
			Prim = New ESRI.ArcGIS.Geometry.Polygon
			SecL = New ESRI.ArcGIS.Geometry.Polygon
			SecR = New ESRI.ArcGIS.Geometry.Polygon

			If TurnNav.TypeCode = eNavaidType.NDB Then
				Dist0 = NDB.Range * 10.0
			Else
				Dist0 = VOR.Range * 10.0
			End If

			CreateNavaidZone(TurnNav, _OutDir, FictTHR, Ss, Vs, 10.0 * VOR.Range, 10.0 * VOR.Range, SecL, SecR, Prim)

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
		End If

		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			InitWidth = 0.25 * VOR.InitWidth
			SplayAngle = 0.5 * System.Math.Tan(DegToRad(VOR.SplayAngle))
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			InitWidth = 0.25 * NDB.InitWidth
			SplayAngle = 0.5 * System.Math.Tan(DegToRad(NDB.SplayAngle))
		Else 'If FinalNav.TypeCode = eNavaidType.CodeLLZ Then
			InitWidth = 0.25 * VOR.InitWidth
			SplayAngle = 0.0
		End If

		SplayAngle = RadToDeg(System.Math.Atan(SplayAngle))
		ptLeftStriction = PointAlongPlane(FinalNav.pPtPrj, _ArDir + 90.0, InitWidth)
		ptRightStriction = PointAlongPlane(FinalNav.pPtPrj, _ArDir - 90.0, InitWidth)

		pClone = pTurnArea
		GeneralArea = pClone.Clone

		If FinalNav.TypeCode = eNavaidType.LLZ Then
			SelfFIX = False
		ElseIf _bTurnFIXSameMAPtFIX Then
			SelfFIX = True
		ElseIf TurnNav.TypeCode = eNavaidType.NONE Then
			SelfFIX = False
		Else
			'SelfFIX = OptionButton1203.Checked And ((TurnWPT.pPtPrj.X - FinalNav.pPtPrj.X) ^ 2 + (TurnWPT.pPtPrj.Y - FinalNav.pPtPrj.Y) ^ 2 < distEps * distEps)
			SelfFIX = False
		End If

		If _bTurnFIXSameMAPtFIX Or ((Not SelfFIX) And (TurnNav.TypeCode <> eNavaidType.NONE)) Then
			If iTurnDir > 0 Then
				fTmpAzt = ReturnAngleInDegrees(SecL.Point(0), SecL.Point(5))
				Side1 = SideDef(SecL.Point(0), fTmpAzt, NJoinPt)
			Else
				fTmpAzt = ReturnAngleInDegrees(SecR.Point(3), SecR.Point(4))
				Side1 = SideDef(SecR.Point(3), fTmpAzt, NJoinPt)
			End If
		End If

		Dim I As Integer = System.Math.Round(Modulus((_OutDir - _ArDir) * iTurnDir))

		If SelfFIX Or ((I <= 270.0 - NCorrAngle) And OptionButton1203.Checked) Then
			pClone = GeneralArea
			SecPoly0 = pClone.Clone
		Else
			pClone = m_TurnArea
			SecPoly0 = pClone.Clone
		End If

		pTopo = SecPoly0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = GeneralArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pCutter = New ESRI.ArcGIS.Geometry.Polyline
		If TurnNav.TypeCode <> eNavaidType.NONE Then
			ptFar = PointAlongPlane(TurnDirector.pPtPrj, _OutDir, Dist0)

			pCutter.ToPoint = ptFar
			pCutter.FromPoint = PointAlongPlane(TurnDirector.pPtPrj, _OutDir + 180.0, Dist0)
			ClipByLine(GeneralArea, pCutter, SecL, SecR, pTmpPoly)

			pTopo = SecR
			SecR = pTopo.Difference(pIArea)

			pTopo = SecL
			SecL = pTopo.Difference(pIArea)

			If (Not SelfFIX) Or (iTurnDir > 0) Then
				pTopo = SecR
				SecR = pTopo.Difference(m_pFixPoly)
			End If

			If (Not SelfFIX) Or (iTurnDir < 0) Then
				pTopo = SecL
				SecL = pTopo.Difference(m_pFixPoly)
			End If

			SecR = RemoveFars(SecR, ptFar)
			SecL = RemoveFars(SecL, ptFar)
		End If

		If OptionButton1203.Checked Or (TurnNav.TypeCode = eNavaidType.NONE) Then
			If _bTurnFIXSameMAPtFIX Or (Not (SelfFIX Or (TurnNav.TypeCode = eNavaidType.NONE))) Then
				'If NState = eExpansion.noChange Then
				'	fTmp = NCorrAngle 'NReqCorrAngle
				'Else
				'	fTmp = 0.0
				'End If

				'        If (iTurnDir * Side1 > 0) Or (SubtractAngles(ArDir + 180.0, OutAzt) < 90.0 - fTmp) Then
				'            Cutter.FromPoint = PointAlongPlane(NJoinPt, ArDir - 90.0, 2.0 * RModel)
				'            Cutter.ToPoint = PointAlongPlane(NJoinPt, ArDir + 90.0, 2.0 * RModel)
				'
				'            ClipByLine SecR, Cutter, pTmpPoly, Nothing, Nothing
				'            If Not pTmpPoly.IsEmpty() Then
				'                Set SecR = RemoveFars(pTmpPoly, ptFar)
				'            End If
				'
				'            ClipByLine SecL, Cutter, pTmpPoly, Nothing, Nothing
				'            If Not pTmpPoly.IsEmpty() Then
				'                Set SecL = RemoveFars(pTmpPoly, ptFar)
				'            End If
				'        End If

				If (Modulus((_OutDir - _ArDir) * TurnDir, 360.0) < 90.0) Then
					pCutter.FromPoint = PointAlongPlane(ptRightStriction, _ArDir - SplayAngle, 2.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(ptRightStriction, _ArDir - SplayAngle + 180.0, 2.0 * RModel)

					If iTurnDir > 0 Then
						ClipByLine(SecR, pCutter, Nothing, pTmpPoly, Nothing)
					Else
						ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
					End If

					If Not pTmpPoly.IsEmpty() Then
						SecR = RemoveSmalls(pTmpPoly)
					End If

					pCutter.FromPoint = PointAlongPlane(ptLeftStriction, _ArDir + SplayAngle, 2.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(ptLeftStriction, _ArDir + SplayAngle + 180.0, 2.0 * RModel)

					If iTurnDir > 0 Then
						ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
					Else
						ClipByLine(SecL, pCutter, pTmpPoly, Nothing, Nothing)
					End If

					If Not pTmpPoly.IsEmpty() Then
						SecL = RemoveSmalls(pTmpPoly)
					End If
				End If
			ElseIf TurnNav.TypeCode <> eNavaidType.NONE Then
				TmpDir = ReturnAngleInDegrees(NOutPt, pMAPt)

				pCutter.FromPoint = PointAlongPlane(pMAPt, TmpDir, 2.0 * RModel)
				pCutter.ToPoint = PointAlongPlane(pMAPt, TmpDir + 180.0, 2.0 * RModel)

				If iTurnDir > 0 Then
					ClipByLine(SecL, pCutter, Nothing, pTmpPoly, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecL = RemoveFars(pTmpPoly, ptFar)
					End If

					ClipByLine(SecPoly0, pCutter, pTmpPoly, Nothing, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecPoly0 = RemoveFars(pTmpPoly, ptFar)
					Else
						SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
					End If
				Else
					ClipByLine(SecR, pCutter, pTmpPoly, Nothing, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecR = RemoveFars(pTmpPoly, ptFar)
					End If

					ClipByLine(SecPoly0, pCutter, Nothing, pTmpPoly, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecPoly0 = RemoveFars(pTmpPoly, ptFar)
					Else
						SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
					End If
				End If
			End If
			'=============SecPoly0===
			If FinalNav.TypeCode <> eNavaidType.LLZ Then
				If (_bTurnFIXSameMAPtFIX Or (Not SelfFIX)) Then
					pCutter.FromPoint = PointAlongPlane(KKFixMax.FromPoint, _ArDir + 90.0, 2.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(KKFixMax.FromPoint, _ArDir - 90.0, 2.0 * RModel)

					ClipByLine(SecPoly0, pCutter, pTmpPoly, Nothing, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecPoly0 = RemoveSmalls(pTmpPoly)
					End If

					pCutter.FromPoint = PointAlongPlane(KK.FromPoint, _ArDir + 90.0, 2.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(KK.FromPoint, _ArDir - 90.0, 2.0 * RModel)

					ClipByLine(SecPoly0, pCutter, pTmpPoly, Nothing, Nothing)
					If Not pTmpPoly.IsEmpty() Then
						SecPoly0 = RemoveSmalls(pTmpPoly)
					End If
				End If
				'DrawPolygon SecPoly0, 0

				pCutter.FromPoint = PointAlongPlane(K1K1.FromPoint, _ArDir + 90.0, 2.0 * RModel)
				pCutter.ToPoint = PointAlongPlane(K1K1.FromPoint, _ArDir - 90.0, 2.0 * RModel)
				If iTurnDir > 0 Then
					ClipByLine(pPolyRight, pCutter, Nothing, pTmpPoly, Nothing)
				Else
					ClipByLine(pPolyLeft, pCutter, Nothing, pTmpPoly, Nothing)
				End If

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTopo = SecPoly0
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pCutter.FromPoint = PointAlongPlane(KK.FromPoint, _ArDir + 90.0, 2.0 * RModel)
				pCutter.ToPoint = PointAlongPlane(KK.FromPoint, _ArDir - 90.0, 2.0 * RModel)

				ClipByLine(pTmpPoly, pCutter, pTmpPoly, Nothing, Nothing)

				If Not pTmpPoly.IsEmpty() Then
					pTopo = pTmpPoly
					pTmpPoly = RemoveSmalls(pTopo.Intersect(GeneralArea, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension))

					pTopo = pTmpPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					SecPoly0 = pTopo.Union(SecPoly0)
				End If

				If iTurnDir > 0 Then
					pCutter.FromPoint = PointAlongPlane(ptRightStriction, _ArDir - SplayAngle, 2.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(ptRightStriction, _ArDir - SplayAngle + 180.0, 2.0 * RModel)
				Else
					pCutter.ToPoint = PointAlongPlane(ptLeftStriction, _ArDir + SplayAngle, 2.0 * RModel)
					pCutter.FromPoint = PointAlongPlane(ptLeftStriction, _ArDir + SplayAngle + 180.0, 2.0 * RModel)
				End If

				ClipByLine(SecPoly0, pCutter, pTmpPoly, Nothing, Nothing)

				If Not pTmpPoly.IsEmpty() Then
					SecPoly0 = RemoveSmalls(pTmpPoly)
				Else
					SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
				End If
			Else
				SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
			End If
		ElseIf _bTurnFIXSameMAPtFIX Then
			SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
		Else
			SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon
		End If

		'DrawPolygon SecPoly0, 0
		'DrawPolygon pTmpPoly, 0
		'If iTurnDir > 0 Then
		'    ClipByLine pTmpPoly, Cutter, pTmpPoly, Nothing, Nothing
		'Else
		'    ClipByLine pPolyLeft, Cutter, pTmpPoly, Nothing, Nothing
		'End If
		'DrawPolygon SecPoly0, 0
		'If Not SelfFIX And (iTurnDir * Side1 >= 0) Then
		If TurnNav.TypeCode <> eNavaidType.NONE Then
			If CheckBox1401.Checked Then
				If (Not _bTurnFIXSameMAPtFIX) And OptionButton1203.Checked Then 'Na FIX
					'        SelfFIX = (TurnWPT.pPtPrj.X - FinalNav.pPtPrj.X) ^ 2 + (TurnWPT.pPtPrj.Y - FinalNav.pPtPrj.Y) ^ 2 < distEps * distEps
					'==================== Outer ==================
					ptCutOuter = New ESRI.ArcGIS.Geometry.Point
					Construct = ptCutOuter
					'        Return
					Construct.ConstructAngleIntersection(ptOut, DegToRad(_OutDir), FOutPt, DegToRad(_OutDir + 90.0))
					SideIn = SideDef(ptOut, _OutDir + 90.0, ptCutOuter)

					If SideIn < 0 Then
						ptCutOuter.PutCoords(ptOut.X, ptOut.Y)
					End If

					OutDist = SideIn * ReturnDistanceInMeters(ptOut, ptCutOuter)
					'==================== Inner ==================
					ptCutInner0 = New ESRI.ArcGIS.Geometry.Point
					ptCutInner1 = New ESRI.ArcGIS.Geometry.Point

					Construct = ptCutInner0
					Construct.ConstructAngleIntersection(ptOut, DegToRad(_OutDir), NOutPt, DegToRad(_OutDir + 90.0))

					If ptCutInner0.IsEmpty() Then
						ptCutInner0.PutCoords(ptCutOuter.X, ptCutOuter.Y)
					End If

					Construct = ptCutInner1
					Construct.ConstructAngleIntersection(ptOut, DegToRad(_OutDir), NOutPt, DegToRad(_ArDir))

					If ptCutInner1.IsEmpty() Then
						ptCutInner1.PutCoords(ptCutInner0.X, ptCutInner0.Y)
					End If

					Side0 = SideDef(ptOut, _OutDir + 90.0, ptCutInner0)
					Dist0 = Side0 * ReturnDistanceInMeters(ptOut, ptCutInner0)

					Side1 = SideDef(ptOut, _OutDir + 90.0, ptCutInner1)
					Dist1 = Side1 * ReturnDistanceInMeters(ptOut, ptCutInner1)

					TmpDir = ReturnAngleInDegrees(ptCutInner0, ptCutInner1)

					If Side0 < 0 Then
						ptCutInner0.PutCoords(ptOut.X, ptOut.Y)
					End If

					If Side1 < 0 Then
						ptCutInner1.PutCoords(ptOut.X, ptOut.Y)
					End If

					bFlg = SubtractAngles(_OutDir, TmpDir) > 0.5
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
					pCutter.ToPoint = PointAlongPlane(ptCut, _OutDir - 90.0 * TurnDir, 2.0 * RModel)

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
					If SelfFIX Then
						'            Set ptTmp = PointAlongPlane(FinalNav.pPtPrj, ArDir - 90.0 * iTurnDir, InitWidth)
						'
						'            Cutter.FromPoint = PointAlongPlane(ptTmp, fTmp, 2.0 * RModel)
						'            Cutter.ToPoint = PointAlongPlane(ptTmp, fTmp + 180.0, 2.0 * R)
						''        DrawPolyLine Cutter, 255
						'            If iTurnDir > 0 Then
						'                ClipByLine SecL, Cutter, pTmpPoly, Nothing, Nothing
						'                If Not pTmpPoly.IsEmpty() Then
						'                    Set SecL = RemoveFars(pTmpPoly, ptFar)
						'                End If
						'            Else
						'                ClipByLine SecR, Cutter, Nothing, pTmpPoly, Nothing
						'                If Not pTmpPoly.IsEmpty() Then
						'                    Set SecR = RemoveFars(pTmpPoly, ptFar)
						'                End If
						'            End If
					Else
						If bFlg Then
							pCutter.ToPoint = PointAlongPlane(ptCut, _OutDir + 90.0 * TurnDir, 2.0 * RModel)
						Else
							pCutter.ToPoint = PointAlongPlane(ptCut, _ArDir + 180.0, 2.0 * RModel)
							pCutter.FromPoint = PointAlongPlane(ptCut, _ArDir, 2.0 * RModel)

							If SideDef(pCutter.ToPoint, _OutDir, pCutter.FromPoint) <> TurnDir Then
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
					End If
				Else 'Na curs FIX
					'==================== Outer ==================
					pCutter.FromPoint = FlyBy

					If SideDef(FlyBy, DrDir, ptOut) = TurnDir Then
						pCutter.ToPoint = PointAlongPlane(FlyBy, _OutDir - 90.0 * TurnDir, 2.0 * RModel)
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

					If SideDef(FlyBy, _OutDir, ptTmp) = TurnDir Then
						TmpDepDir = _ArDir + 180.0
					Else
						TmpDepDir = _ArDir
					End If

					If SideDef(FlyBy, DrDir, ptOut) <> TurnDir Then
						TmpDir = _OutDir + 90.0 * TurnDir
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

			'DrawPolygon SecL, 255 * 256#
			'DrawPolygon SecR, 255 * 256#

			'If CheckBox1402.Value = 1 Then
			'If True Then
			ptCut = New ESRI.ArcGIS.Geometry.Point

			'If Not SelfFIX Then
			If (_bTurnFIXSameMAPtFIX Or (Not SelfFIX)) And CheckBox1402.Checked Then
				'If Not SelfFIX And (Side * Side1 > 0) Then
				TmpDir = _OutDir - arSecAreaCutAngl.Value * TurnDir
				Construct = ptCut

				Construct.ConstructAngleIntersection(TurnDirector.pPtPrj, DegToRad(_OutDir), NJoinPt, DegToRad(TmpDir))
				'DrawPoint ptCut, 255

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

			'DrawPolygon SecL, 255 * 256#
			'If CheckBox1405.Value = 1 Then
			If CheckBox1405.Checked Then
				'If Not SelfFIX And (Side * Side1 > 0) Then
				'    Set ptCut = New Point
				Construct = ptCut

				TmpDir = _OutDir + arSecAreaCutAngl.Value * TurnDir
				Construct.ConstructAngleIntersection(TurnDirector.pPtPrj, DegToRad(_OutDir), FJoinPt, DegToRad(TmpDir))

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
			'DrawPolygon SecR, 255 * 256#

			ptCut.PutCoords(TurnDirector.pPtPrj.X, TurnDirector.pPtPrj.Y)

			'If CheckBox1403.Value = 1 Then
			If Not SelfFIX And CheckBox1403.Checked Then
				'SecPoly0
				'If Not SelfFIX Then
				'If Not SelfFIX And (Side * Side1 > 0) Then
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

			'DrawPolygon SecL, 255 * 256#
			'If CheckBox1406.Value = 1 Then
			If CheckBox1406.Checked Then
				'If Not SelfFIX And (Side * Side1 > 0) Then
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
			'DrawPolygon SecR, 255 * 256#
			'DrawPolygon SecL, 255 * 256#
			'DrawPolygon SecR, 255 * 256#

			'If Not OptionButton0901 Then
			'If True Then
			'    Set Topo = SecR
			'    Set SecR = Topo.Difference(pFIXPoly)
			'
			'    Set Topo = SecL
			'    Set SecL = Topo.Difference(pFIXPoly)
			'End If

			'==========================================
			If False Then
				pClone = m_TurnArea
				pTmpPoly = pClone.Clone
				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTopo = SecL
				SecL = pTopo.Difference(pTmpPoly)

				pTopo = SecR
				SecR = pTopo.Difference(pTmpPoly)
			End If
			'===========================================

			SecL = RemoveFars(SecL, ptFar)
			SecR = RemoveFars(SecR, ptFar)

			SecL = RemoveAgnails(SecL)
			SecR = RemoveAgnails(SecR)

			pTopo = SecR
			SecPoly = pTopo.Union(SecL)
		Else
			SecPoly = New ESRI.ArcGIS.Geometry.Polygon
		End If

		On Error Resume Next
		If Not PrimElem Is Nothing Then pGraphics.DeleteElement(PrimElem)
		PrimElem = Nothing

		If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
		If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
		If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)

		SecLElem = DrawPolygon(SecL, RGB(0, 0, 255))
		SecLElem.Locked = True

		SecRElem = DrawPolygon(SecR, RGB(0, 0, 255))
		SecRElem.Locked = True

		Sec0Elem = DrawPolygon(SecPoly0, RGB(0, 0, 255))
		Sec0Elem.Locked = True
		On Error GoTo 0

		'DrawPolygon SecR, RGB(0, 0, 255)
		'DrawPolygon SecL, RGB(0, 0, 255)
	End Sub

	Private Sub CheckBox1401_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1401.CheckedChanged, CheckBox1402.CheckedChanged, CheckBox1403.CheckedChanged,
													CheckBox1404.CheckedChanged, CheckBox1405.CheckedChanged, CheckBox1406.CheckedChanged
		If Not bFormInitialised Then Return

		If sender Is CheckBox1401 Then
			CheckBox1404.Checked = CheckBox1401.Checked
		ElseIf sender Is CheckBox1404 Then
			CheckBox1401.Checked = CheckBox1404.Checked
		End If

		'If CurrPage <> 14 Then Return
		SecondArea(TurnDir, BaseArea)
	End Sub

#End Region

#Region "Page 16"
	Private Sub CheckBox1501_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1501.CheckedChanged
		If Not bFormInitialised Then Return

		OkBtn.Enabled = Not CheckBox1501.Checked

		_Label1501_9.Enabled = Not CheckBox1501.Checked
		NextBtn.Enabled = CheckBox1501.Checked
	End Sub

	Private Sub OptionButton1501_CheckedChanged(ByVal sender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptionButton1501.CheckedChanged, OptionButton1502.CheckedChanged
		If Not sender.Checked Then Return
		If Not bFormInitialised Then Return
		If Not sender.Enabled Then Return

		ComboBox1502_SelectedIndexChanged(ComboBox1502, Nothing)
	End Sub

	Private Sub ComboBox1501_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1501.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim pInterceptPt As ESRI.ArcGIS.Geometry.IPoint
		Dim fDist As Double
		Dim fDir As Double
		Dim fAzt As Double
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		I = ComboBox1501.SelectedIndex - 1
		If I < 0 Then
			TextBox1507.ReadOnly = False
		Else
			If Not OptionButton1202.Checked Then Return

			TextBox1507.ReadOnly = True

			K = ComboBox1502.SelectedIndex
			If K < 0 Then Return

			N = UBound(TerInterNavDat)
			If N < 0 Then Return

			_Label1501_25.Text = GetNavTypeName(FixBox1501(I).TypeCode)

			'Dim pPoly As IPolyline
			'pPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
			'DrawPolyLine(pPoly, RGB(255, 0, 0), 2)
			'Application.DoEvents()

			pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

			If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
				fDist = ReturnDistanceInMeters(TerInterNavDat(K).pPtPrj, FixBox1501(I).pPtPrj)
				TextBox1507.Text = ConvertDistance(fDist - TerInterNavDat(K).Disp, 0).ToString("0.0000")
				If SideDef(TerInterNavDat(K).pPtPrj, pInterceptPt.M + 90.0, FixBox1501(I).pPtPrj) < 0 Then
					OptionButton1501.Checked = True
				Else
					OptionButton1502.Checked = True
				End If
			Else
				fDir = ReturnAngleInDegrees(TerInterNavDat(K).pPtPrj, FixBox1501(I).pPtPrj)
				fAzt = Dir2Azt(TerInterNavDat(K).pPtPrj, fDir)
				If (TerInterNavDat(K).TypeCode = eNavaidType.NDB) Then
					TextBox1507.Text = CStr(ConvertAngle(Modulus(fAzt + 180.0 - TerInterNavDat(K).MagVar)))
				Else
					TextBox1507.Text = CStr(ConvertAngle(Modulus(fAzt - TerInterNavDat(K).MagVar)))
				End If
			End If
		End If

		TextBox1507.Tag = "-"
		TextBox1507_Validating(TextBox1507, New CancelEventArgs())
	End Sub

	Private Sub ComboBox1502_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1502.SelectedIndexChanged
		If Not bFormInitialised Then Return

		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim Kmin As Integer
		Dim Kmax As Integer
		Dim tipStr As String

		If Not OptionButton1202.Checked Then Return
		K = ComboBox1502.SelectedIndex
		If K < 0 Then Return

		N = UBound(TerInterNavDat)

		If N < 0 Then Return

		_Label1501_14.Text = GetNavTypeName(TerInterNavDat(K).TypeCode)

		If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
			TextBox1507.Visible = True

			_Label1501_16.Text = DistanceConverter(DistanceUnit).Unit

			N = UBound(TerInterNavDat(K).ValMin)

			OptionButton1501.Enabled = N > 0
			OptionButton1502.Enabled = N > 0

			If OptionButton1501.Checked Or (N = 0) Then
				TextBox1507.Text = CStr(ConvertDistance(TerInterNavDat(K).ValMin(0) - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
			Else
				TextBox1507.Text = CStr(ConvertDistance(TerInterNavDat(K).ValMin(1) - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
			End If

			If N = 0 Then
				If TerInterNavDat(K).ValCnt > 0 Then
					OptionButton1501.Checked = True
				Else
					OptionButton1502.Checked = True
				End If
			End If

			_Label1501_15.Text = My.Resources.str00229
			tipStr = My.Resources.str10514

			For I = 0 To N
				tipStr = tipStr + My.Resources.str00221 + CStr(ConvertDistance(TerInterNavDat(K).ValMin(I) - TerInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + My.Resources.str00222 + CStr(ConvertDistance(TerInterNavDat(K).ValMax(I) - TerInterNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
				If I < N Then
					tipStr = My.Resources.str00230 + " - " + tipStr + vbCrLf + My.Resources.str00231 + " - "
				End If
			Next I
			'===
		Else
			OptionButton1501.Enabled = False
			OptionButton1502.Enabled = False

			TextBox1507.Visible = TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid
			If TerInterNavDat(K).IntersectionType = eIntersectionType.OnNavaid Then
				TextBox1507.Text = ""
				tipStr = "FIX " + My.Resources.str00106
				_Label1501_15.Text = tipStr
				_Label1501_16.Text = ""
			Else
				_Label1501_16.Text = "°"
				If TerInterNavDat(K).TypeCode = eNavaidType.NDB Then
					_Label1501_15.Text = My.Resources.str00228
					Kmax = Modulus(TerInterNavDat(K).ValMax(0) + 180.0 - TerInterNavDat(K).MagVar, 360.0)
					Kmin = Modulus(TerInterNavDat(K).ValMin(0) + 180.0 - TerInterNavDat(K).MagVar, 360.0)
				Else
					_Label1501_15.Text = My.Resources.str00227
					Kmax = Modulus(TerInterNavDat(K).ValMax(0) - TerInterNavDat(K).MagVar, 360.0)
					Kmin = Modulus(TerInterNavDat(K).ValMin(0) - TerInterNavDat(K).MagVar, 360.0)
				End If
				tipStr = My.Resources.str00220 + My.Resources.str00221

				If TerInterNavDat(K).ValCnt > 0 Then
					TextBox1507.Text = CStr(ConvertAngle(Kmin))
				Else
					TextBox1507.Text = CStr(ConvertAngle(Kmax))
				End If
				tipStr = tipStr + CStr(ConvertAngle(Kmin)) + " °" + My.Resources.str00222 + CStr(ConvertAngle(Kmax)) + " °"
			End If
		End If

		'_Label1101_2.Caption = tipStr
		If TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			tipStr = Replace(tipStr, vbCrLf, "   ")
			ToolTip1.SetToolTip(TextBox1507, tipStr)
		End If

		FillCombo1501()
	End Sub

	Private Sub ComboBox1503_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboBox1503.SelectedIndexChanged
		If ComboBox1503.SelectedIndex = 0 Then
			TextBox1504.Text = CStr(ConvertHeight(resultOCH + fRefHeight, eRoundMode.NEAREST))
		Else
			TextBox1504.Text = CStr(ConvertHeight(resultOCH, eRoundMode.NEAREST))
		End If
	End Sub

	Private Sub TextBox1505_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1505.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1505_Validating(TextBox1505, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1505.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1505_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1505.Validating
		Dim fVal As Double
		Dim fTmp As Double

		If Not Double.TryParse(TextBox1505.Text, fVal) Then fVal = -1.0

		fVal = DeConvertDistance(fVal)
		fTmp = fVal

		If fVal > TurnAreaMaxd0 Then fVal = TurnAreaMaxd0
		If fVal < arBufferMSA.Value Then fVal = arBufferMSA.Value
		If fTmp <> fVal Then TextBox1505.Text = ConvertDistance(fVal, eRoundMode.NEAREST).ToString()

		TerminationOCH()
	End Sub

	Private Sub TextBox1507_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1507.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1507_Validating(TextBox1507, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1507.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1507_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1507.Validating
		Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

		Dim InterToler As Double
		Dim TrackToler As Double
		Dim fDirl As Double
		Dim hFix As Double
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

		pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

		If Not OptionButton1202.Checked Then Return
		K = ComboBox1502.SelectedIndex
		If K < 0 Then Return

		If TerInterNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			If TextBox1507.Tag = TextBox1507.Text Then Return
			If IsNumeric(TextBox1507.Text) Then
				fDirl = CDbl(TextBox1507.Text)
			Else 'If (TerInterNavDat(K).ValCnt <> -2) Or (TerInterNavDat(K).TypeCode = 1) Then
				Return
			End If
		End If

		If Not TerminationFIXElem Is Nothing Then
			pGroupElement = TerminationFIXElem
			For I = 0 To pGroupElement.ElementCount - 1
				Try
					If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
				Catch
				End Try
			Next I
		End If

		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement


		Select Case TerInterNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				pClone = TerInterNavDat(K).pPtPrj
				TerFixPnt = pClone.Clone
			Case eIntersectionType.ByAngle
				fDirl = fDirl + TerInterNavDat(K).MagVar
				If (TerInterNavDat(K).TypeCode = eNavaidType.VOR) Or (TerInterNavDat(K).TypeCode = eNavaidType.TACAN) Then
					InterToler = VOR.IntersectingTolerance                  'fDis = VOR.Range
				ElseIf (TerInterNavDat(K).TypeCode = eNavaidType.NDB) Then
					InterToler = NDB.IntersectingTolerance                  'fDis = NDB.Range
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
						TextBox1507.Text = CStr(ConvertAngle(Modulus(fDirl - TerInterNavDat(K).MagVar + 180.0)))
					Else
						TextBox1507.Text = CStr(ConvertAngle(Modulus(fDirl - TerInterNavDat(K).MagVar)))
					End If
				End If

				fDirl = Azt2Dir(TerInterNavDat(K).pPtGeo, fDirl)

				TerFixPnt = New ESRI.ArcGIS.Geometry.Point
				pConstruct = TerFixPnt

				pConstruct.ConstructAngleIntersection(TerInterNavDat(K).pPtPrj, DegToRad(fDirl), pInterceptPt, DegToRad(pInterceptPt.M))
			Case eIntersectionType.ByDistance, eIntersectionType.RadarFIX
				fDirl = DeConvertDistance(fDirl) + TerInterNavDat(K).Disp
				fTmp = fDirl

				N = UBound(TerInterNavDat(K).ValMin)

				If OptionButton1501.Checked Or (N = 0) Then
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
					TextBox1507.Text = CStr(ConvertDistance(fDirl - TerInterNavDat(K).Disp, eRoundMode.NEAREST))
				End If

				If fDirl < 1 Then fDirl = 1

				If (TerInterNavDat(K).IntersectionType <> eIntersectionType.RadarFIX) And ((TerInterNavDat(K).ValCnt < 0) Or (OptionButton1501.Enabled And OptionButton1501.Checked)) Then
					CircleVectorIntersect(TerInterNavDat(K).pPtPrj, fDirl, pInterceptPt, pInterceptPt.M + 180, TerFixPnt)
				Else
					CircleVectorIntersect(TerInterNavDat(K).pPtPrj, fDirl, pInterceptPt, (pInterceptPt.M), TerFixPnt)
				End If
		End Select

		TerFixPnt.M = pInterceptPt.M

		TerminationOCH()

		If OptionButton0901.Checked Then
			pProxi = ZNR_Poly
		Else
			pProxi = KK
		End If

		fDis = pProxi.ReturnDistance(TerFixPnt)
		hKK = DeConvertHeight(CDbl(TextBox1502.Text))

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
			Case eIntersectionType.OnNavaid
				OnNavaidFIXTolerArea(TerInterNavDat(K), pInterceptPt.M, TerFixPnt.Z, pSect0)
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

				If (TerInterNavDat(K).IntersectionType <> eIntersectionType.RadarFIX) And ((TerInterNavDat(K).ValCnt < 0) Or (OptionButton1501.Enabled And OptionButton1501.Checked)) Then
					pTopo.Cut(pCutter, pSect1, pSect0)
				Else
					pTopo.Cut(pCutter, pSect0, pSect1)
				End If

				pGroupElement.AddElement(DrawPolygon(pSect0, RGB(0, 0, 255), , False))
		End Select

		pTopo = pSect0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		TurnNav = TurnWPTToTurnNav(_bTurnFIXSameMAPtFIX, MAPtNavDat(ComboBox0801.SelectedIndex), TurnDirector)
		pClone = Prim
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
			pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180.0 - TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + 180.0 + TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(TurnNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M - TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(TurnNav.pPtPrj, pInterceptPt.M + TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(TurnNav.pPtPrj)
			'    Set pFIXPoly__ = Clone.Clone
		Else
			'    Set Clone = pPolygon
			'    Set pPolygon1 = Clone.Clone
			pPolyClone = pClone.Clone
		End If

		pTermFIXTolerArea = pClone.Clone

		pTopo = pPolyClone
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPointWithText(NJoinPt, "NJoinPt")
		'DrawPointWithText(FJoinPt, "FJoinPt")

		'DrawPointWithText(FOutPt, "FOutPt")
		'DrawPointWithText(NOutPt, "NOutPt")
		'Application.DoEvents()

		pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If (pLine.PointCount = 0) And (TerInterNavDat(K).IntersectionType <> eIntersectionType.ByAngle) Then
			pLine = pTopo.Intersect(pSect0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
		End If

		dMax = -RModel
		dMin = RModel

		For I = 0 To pLine.PointCount - 1
			PrjToLocal(pInterceptPt, pInterceptPt.M, pLine.Point(I), D, fTmp)
			If D < dMin Then
				dMin = D
				pt2 = pLine.Point(I)
			End If
			If D > dMax Then
				dMax = D
				pt1 = pLine.Point(I)
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

		Try
			'If ButtonControl8State Then
			For I = 0 To pGroupElement.ElementCount - 1
				pGraphics.AddElement(pGroupElement.Element(I), 0)
				pGroupElement.Element(I).Locked = True
			Next I
			'End If

			'If ButtonControl5State Then
			'	pGraphics.AddElement(StrTrackElem, 0)
			'	StrTrackElem.Locked = True
			'End If

			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		Catch
		End Try

		'RefreshCommandBar(mTool, 128)

		'==========================================================================
		TextBox1507.Tag = TextBox1507.Text
	End Sub

	Private Sub TextBox1510_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1510.KeyPress
		If eventArgs.KeyChar = Chr(13) Then
			TextBox1510_Validating(TextBox1510, New System.ComponentModel.CancelEventArgs())
		Else
			TextBoxFloat(eventArgs.KeyChar, TextBox1510.Text)
		End If

		If eventArgs.KeyChar = Chr(0) Then eventArgs.Handled = True
	End Sub

	Private Sub TextBox1510_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox1510.Validating
		Dim MinVal As Double
		Dim MaxVal As Double
		Dim NevVal As Double
		Dim fTmp As Double

		If Not IsNumeric(TextBox1510.Text) Then Return
		MinVal = CDbl(TextBox1503.Text)
		MaxVal = CDbl(TextBox1508.Text)
		NevVal = CDbl(TextBox1510.Text)
		fTmp = NevVal

		If NevVal < MinVal Then NevVal = MinVal
		If NevVal > MaxVal Then NevVal = MaxVal

		'fTermAlt = DeConvertHeight(NevVal)
		If fTmp <> NevVal Then
			TextBox1510.Text = CStr(NevVal)
		End If
	End Sub

#End Region

#Region "Page 17"

	Private Sub ReListTrace()
		ListView1501.Items.Clear()

		For i As Integer = 0 To TSC - 1
			Dim itmX As ListViewItem = ListView1501.Items.Add((i + 1).ToString())
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
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HStart, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HStart - fRefHeight, eRoundMode.NEAREST).ToString()))

			itmX.SubItems(2).Text = itmX.SubItems(2).Text + " " + GlobalVars.HeightConverter(GlobalVars.HeightUnit).Unit

			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, Functions.ConvertHeight(Trace(i).HFinish, eRoundMode.NEAREST).ToString() + " / " + Functions.ConvertHeight(Trace(i).HFinish - fRefHeight, eRoundMode.NEAREST).ToString()))
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

		For i = 0 To ListView1501.Items.Count - 1
			If ListView1501.Items(i).Selected Then
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

	Private Sub ListView1501_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1501.SelectedIndexChanged
		ReSelectTrace()
	End Sub

	Private Sub AddSegmentBtn_Click(sender As Object, e As EventArgs) Handles AddSegmentBtn.Click
		For i As Integer = 0 To ListView1501.Items.Count - 1
			ListView1501.Items(i).Selected = False
		Next
		ReSelectTrace()

		If TSC = MaxTraceSegments Then
			MessageBox.Show(Resources.str00151, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Return
		End If

		Me.Hide()

		AddSegmentFrm.CreateNextSegment(GlobalVars.CurrADHP, fRefHeight, _missedIAS, segPDG, Trace(TSC - 1), Me)
	End Sub

	Private Sub RemoveSegmentBtn_Click(sender As Object, e As EventArgs) Handles RemoveSegmentBtn.Click
		screenCapture.Delete()
		For i As Integer = 0 To ListView1501.Items.Count - 1
			ListView1501.Items(i).Selected = False
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

#End Region

	Private Function FindOptimalMAPt(ByRef MAOCH As Double, ByRef NearMinXi As Double, ByRef FarMinXiDist As Double, ByRef Ix As Integer, ByRef Kx As Integer, ByRef maxMAOCH As Double) As Double
		Dim Side As Integer
		Dim I As Integer
		Dim N As Integer

		Dim fMA_ReqOCH As Double
		Dim OldMAOCH As Double
		Dim NewMAOCH As Double
		Dim FarMinXi As Double
		Dim fInterv As Double
		Dim TASmin As Double
		Dim TASmax As Double
		Dim MinXi As Double
		Dim Dist As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim fTmp As Double
		Dim XI As Double
		Dim X1 As Double
		Dim X2 As Double
		Dim K As Double
		Dim K1 As Double
		Dim H As Double
		Dim B As Double
		Dim D As Double

		Dim bFlg As Boolean
		Dim bOuterLoop As Boolean

		Dim pProximity As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pPoly As ESRI.ArcGIS.Geometry.IPolygon

		Dim pFarPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		ptTmp = PointAlongPlane(ptTmpFAF, _ArDir, 5.0)
		pCutter = New ESRI.ArcGIS.Geometry.Polyline
		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)

		ClipByLine(pFullPoly, pCutter, pPoly, Nothing, Nothing)

		pTopo = FinalBaseArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			pMissAproachArea = pTopo.Union(pSDFTolerArea)
		Else
			pMissAproachArea = pTopo.Union(FinalBaseArea)
		End If

		pTopo = pMissAproachArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'=========================================================================================================

		ptTmp = PointAlongPlane(FinalNav.pPtPrj, _ArDir, 1000000.0)
		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)
		pProximity = pCutter
		Dist1 = 1000000.0 - pProximity.ReturnDistance(pMissAproachArea)

		ptTmp = PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0, 1000000.0)
		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)
		pProximity = pCutter
		Dist0 = 1000000.0 - pProximity.ReturnDistance(pMissAproachArea)

		Dist = Max(Dist0, Dist1)
		'=========================================================================================================

		Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly2 As ESRI.ArcGIS.Geometry.IPointCollection
		pTmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
		pTmpPoly2 = New ESRI.ArcGIS.Geometry.Polygon

		CreateNavaidZone(FinalNav, _ArDir, FictTHR, Ss, Vs, Dist, Dist, pTmpPoly1, pTmpPoly2, pPolyPrime)
		pTmpPoly1 = Nothing
		pTmpPoly2 = Nothing

		pTopo = pPolyPrime
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(pMissAproachArea, RGB(0, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolygon(pPolyPrime, RGB(0, 0, 255), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
		'Application.DoEvents()

		'=========================================================================================================

		ptTmp = PointAlongPlane(ptTmpFAF, _ArDir + 180.0, 100000.0)
		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)

		pProximity = pCutter
		Dist = pProximity.ReturnDistance(pMissAproachArea)
		'=========================================================================================================
		pFarPoint = PointAlongPlane(ptTmp, _ArDir, Dist)

		GetMAPtObstacles(ObstacleList, MAPtObstacles, ptTmpFAF, pFarPoint, FinalNav.pPtPrj, FinalNav.TypeCode, _ArDir, pMissAproachArea)
		Sort(MAPtObstacles, 0)

		OldMAOCH = MAOCH
		maxMAOCH = MAOCH

		B = arFAFTolerance.Value
		N = UBound(MAPtObstacles.Parts)

		K1 = 0.0

		bFlg = True
		MinXi = ReturnDistanceInMeters(ptTmpFAF, FictTHR)
		If MinXi > arMOCChangeDist.Value Then
			K = 0.0 '(MinXi - arMOCChangeDist.Value) * arAddMOCCoef.Value
		Else
			K = 0.0
		End If

		Do      'Outer Loop
			bOuterLoop = False
			Ix = -1
			Kx = -1

			For I = 0 To N
				XI = (ptTmpFAF.Z + MAPtObstacles.Parts(I).Dist * fMissAprPDG - MAPtObstacles.Parts(I).Height - arMA_InterMOC.Value * MAPtObstacles.Parts(I).fTmp) / (fMissAprPDG + FinalAreaPDG)

				MAPtObstacles.Parts(I).MOC = (K + arFASeg_FAF_MOC.Value) * MAPtObstacles.Parts(I).fTmp
				MAPtObstacles.Parts(I).ReqH = MAPtObstacles.Parts(I).Height + MAPtObstacles.Parts(I).MOC

				If XI < MAPtObstacles.Parts(I).Dist Then
					fTmp = (MAPtObstacles.Parts(I).Height + arMA_InterMOC.Value * MAPtObstacles.Parts(I).fTmp) - fMissAprPDG * (MAPtObstacles.Parts(I).Dist - XI)
					fMA_ReqOCH = Min(fTmp, MAPtObstacles.Parts(I).ReqH)
				Else
					fMA_ReqOCH = MAPtObstacles.Parts(I).ReqH
				End If

				If (XI > MAPtObstacles.Parts(I).Dist) Or (XI < K1) Then
					If fMA_ReqOCH > ptTmpFAF.Z Then
						If fMA_ReqOCH > maxMAOCH Then maxMAOCH = fMA_ReqOCH
					Else
						If fMA_ReqOCH > MAOCH Then
							MAOCH = fMA_ReqOCH
							Ix = I
						End If
					End If
				Else
					fMA_ReqOCH = ptTmpFAF.Z - XI * FinalAreaPDG
					If fMA_ReqOCH > ptTmpFAF.Z Then
						If fMA_ReqOCH > maxMAOCH Then
							maxMAOCH = fMA_ReqOCH
						End If
					Else
						If fMA_ReqOCH > MAOCH Then
							MAOCH = fMA_ReqOCH
							Ix = I
						End If

						'If (XI < MinXi) And (MAOCH < fMA_ReqOCH) Then
						If (XI < MinXi) And (MAOCH < MAPtObstacles.Parts(I).ReqH) Then
							MinXi = XI
							Kx = I
						End If
					End If
				End If
			Next I

			NearMinXi = (ptTmpFAF.Z - MAOCH) / FinalAreaPDG

			If NearMinXi < 0.0 Then
				If CheckBox0701.Checked Then
					MessageBox.Show(My.Resources.str00527)  '"OCH больше высоты SDF. Увеличьте высоту SDF."
				ElseIf OptionButton0201.Checked Then
					MessageBox.Show(My.Resources.str00500)  '"OCH больше высоты FAF. Увеличьте высоту FAF."
				Else
					MessageBox.Show(My.Resources.str00528)  '"OCH больше высоты SoIL. Увеличьте высоту SoIL."
				End If

				MAOCH = OldMAOCH
				Return -1
			End If

			FarMinXi = (MAOCH - ptTmpFAF.Z + MinXi * FinalAreaPDG) / fMissAprPDG + MinXi
			'========================================================================
			NewMAOCH = MAOCH

			Do                  'Inner Loop
				H = fRefHeight + NewMAOCH
				D = NearMinXi

				TASmin = IAS2TAS(3.6 * cVfafMin.Values(Category), H, arISAmin.Value)
				TASmax = IAS2TAS(3.6 * cVfafMax.Values(Category), H, arISAmax.Value)

				X1 = System.Math.Sqrt(B * B + (0.00361111111111111 * TASmin) ^ 2 + (CurrADHP.WindSpeed * D / TASmin) ^ 2) + 4.16666666666667 * (TASmin + 19.0)
				X2 = System.Math.Sqrt(B * B + (0.00361111111111111 * TASmax) ^ 2 + (CurrADHP.WindSpeed * D / TASmax) ^ 2) + 4.16666666666667 * (TASmax + 19.0)
				Dist = Max(X1, X2) '   If X1 > X2 Then Dist = X1 Else Dist = X2

				fInterv = FarMinXi - NearMinXi
				If (Dist > fInterv) Or Not bFlg Then
					fTmp = (Dist * fMissAprPDG * FinalAreaPDG) / (fMissAprPDG + FinalAreaPDG)
					NewMAOCH = ptTmpFAF.Z - MinXi * FinalAreaPDG + fTmp
					X1 = fTmp / FinalAreaPDG

					NearMinXi = MinXi - X1
					FarMinXi = NearMinXi + Dist

					bFlg = False
				ElseIf bFlg Then
					Exit Do
				End If
			Loop While System.Math.Abs(Dist - fInterv) > 0.1

			If NewMAOCH > MAOCH Then MAOCH = NewMAOCH

			If NearMinXi < B Then
				K1 = B - NearMinXi + MinXi
				If K1 > 0.0 Then
					MinXi = K1 + 500.0              '========================
					MAOCH = OldMAOCH
					bFlg = False
					bOuterLoop = True
				End If
			End If
		Loop While bOuterLoop

		'========================================================================
		'DrawPolygon(pMissAproachArea, RGB(0, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolygon(pPolyPrime, RGB(0, 0, 255), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
		'While True
		'	Application.DoEvents()
		'End While

		FarMinXiDist = fInterv - Dist
		Side = SideDef(FictTHR, _ArDir + 90.0, FinalNav.pPtPrj)

		If _OnAero And (Side > 0) Then
			fTmp = Point2LineDistancePrj(FinalNav.pPtPrj, ptTmpFAF, _ArDir + 90.0) - NearMinXi
		Else
			fTmp = Point2LineDistancePrj(FictTHR, ptTmpFAF, _ArDir + 90.0) - NearMinXi
		End If

		If FarMinXiDist > fTmp Then FarMinXiDist = fTmp

		ComboBox0802_SelectedIndexChanged(ComboBox0802, New System.EventArgs())
		Return MAOCH
	End Function

	Private Function CalcMAPtOCH(ByRef ObsName As String) As Double
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer

		Dim Ix As Integer
		Dim Jx As Integer
		Dim Ix0 As Integer
		Dim Jx0 As Integer
		Dim Imx As Integer
		Dim Jmx As Integer

		Dim pProxy As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pBuffer As ESRI.ArcGIS.Geometry.Polygon
		Dim pFarPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim fAppHeight As Double
		Dim fFASeg_MOC As Double
		Dim fPlane15h As Double
		Dim AddMOC As Double
		Dim fTmp2 As Double
		Dim fReqH As Double
		Dim fDist As Double
		Dim fTmp As Double
		Dim fL As Double

		ObsName = ""
		pCutter = New ESRI.ArcGIS.Geometry.Polyline
		pCutter.FromPoint = PointAlongPlane(pMAPt, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(pMAPt, _ArDir - 90.0, RModel)
		'================================================================
		If (CheckBox0701.Checked) And OptionButton0201.Checked Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		fDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR)

		If OptionButton0201.Checked Then                'With FAF
			If (fDist > arMOCChangeDist.Value) Then
				AddMOC = 0.0    '(fDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
				_FinalMOC = arFASeg_FAF_MOC.Value + AddMOC
			Else
				AddMOC = 0.0
				_FinalMOC = arFASeg_FAF_MOC.Value
			End If
		Else                                            'Without FAF
			If CheckBox0701.Checked Then
				If fDist > arMOCChangeDist.Value Then
					_FinalMOC = arFASeg_FAF_MOC.Value   '+ (fDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
				Else
					_FinalMOC = arFASeg_FAF_MOC.Value
				End If
			Else
				_FinalMOC = arFASegmentMOC.Value
			End If
		End If

		ClipByLine(pMissAproachArea, pCutter, Nothing, pBuffer, Nothing)
		CalcArObstaclesNavMOC(ObstacleList, FAFObstacleList, FinalNav.pPtPrj, FictTHR, _ArDir, pPolyPrime, pBuffer, _FinalMOC, Imx)
		N = UBound(FAFObstacleList.Parts)

		'DrawPolygon(pBuffer, RGB(255, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSVertical)
		'Application.DoEvents()

		For I = 0 To N
			FAFObstacleList.Parts(I).Dist = Point2LineDistancePrj(FAFObstacleList.Parts(I).pPtPrj, FictTHR, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, FAFObstacleList.Parts(I).pPtPrj)
			FAFObstacleList.Parts(I).DistStar = Point2LineDistancePrj(FAFObstacleList.Parts(I).pPtPrj, FictTHR, _ArDir) * SideDef(FictTHR, _ArDir + 180.0, FAFObstacleList.Parts(I).pPtPrj)
		Next I

		Sort(FAFObstacleList, 0)
		'=====================
		If OptionButton0201.Checked Or CheckBox0701.Checked Then
			If OptionButton0201.Checked Then
				fFASeg_MOC = arFASeg_FAF_MOC.Value
			Else
				fFASeg_MOC = arFASeg_FAF_MOC.Value '+ (fDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
			End If

			ptTmp = PointAlongPlane(ptTmpFAF, _ArDir + 180.0, 100000.0)
			pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
			pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)

			pProxy = pCutter
			fDist = pProxy.ReturnDistance(pMissAproachArea)
			pFarPoint = PointAlongPlane(ptTmpFAF, _ArDir + 180.0, 100000.0 - fDist)

			fAppHeight = ptTmpFAF.Z - fFASeg_MOC
			fL = fAppHeight / arFixMaxIgnorGrd.Value
		End If

		Ix = -1
		fTmp = 0.0

		For I = 0 To N
			fReqH = FAFObstacleList.Parts(I).ReqH
			FAFObstacleList.Parts(I).Flags = FAFObstacleList.Parts(I).Flags And 1

			If OptionButton0201.Checked Or CheckBox0701.Checked Then
				fDist = Point2LineDistancePrj(FAFObstacleList.Parts(I).pPtPrj, pFarPoint, _ArDir + 90.0)
				If fDist < arFIX15PlaneRang.Value Then
					fPlane15h = arFixMaxIgnorGrd.Value * (fL - fDist)
					If fPlane15h > FAFObstacleList.Parts(I).Height Then
						fReqH = 0.0
						FAFObstacleList.Parts(I).Flags = FAFObstacleList.Parts(I).Flags Or 2
					End If
				End If
			End If

			If fReqH > fTmp Then
				fTmp = fReqH
				Ix = I
			End If
		Next I

		'======================
		If Ix >= 0 Then
			fFinalOCH = FAFObstacleList.Parts(Ix).ReqH
			ObsName = FAFObstacleList.Obstacles(FAFObstacleList.Parts(Ix).Owner).UnicalName
			IxFinalOCH = Ix
			JxFinalOCH = Jx
		Else
			fFinalOCH = _FinalMOC
			ObsName = ""
			IxFinalOCH = -4 'OCH определяется MOC конечного участка
		End If

		fFinalOCA = fFinalOCH + fRefHeight

		NonPrecReportFrm.FillPage03(FAFObstacleList, Ix, True)

		fTmp = ptTmpFAF.Z - dMAPt2FAF * FinalAreaPDG '- fRefHeight
		Ix0 = -3            'OCH определяется местоположением MAPt

		''needs for test +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		'Dim minOCH As Double
		'If OptionButton0101.Checked Then
		'	If OptionButton0201.Checked Then
		'		minOCH = arFASeg_FAF_MOC.Value
		'	Else
		'		minOCH = arFASegmentMOC.Value
		'	End If

		'	If minOCH < fAlignOCH Then minOCH = fAlignOCH
		'Else
		'	minOCH = fNewVisAprOCH
		'End If
		''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		If fTmp < fMinOCH Then
			fTmp = fMinOCH

			If OptionButton0101.Checked Then
				Ix0 = -2    'OCH определяется сопряжением конечного участка
			Else
				Ix0 = -1    'OCH определяется OCH в Зоне Визуального Маневрирования
			End If
		End If

		If fFinalOCH < fTmp Then
			fFinalOCH = fTmp
			ObsName = ""
			IxFinalOCH = Ix0
		End If

		ptTmp = PointAlongPlane(pMAPt, _ArDir + 180.0, dNearMAPt)
		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, RModel)

		ClipByLine(pMissAproachArea, pCutter, pBuffer, Nothing, Nothing)

		'DrawPolygon(pMissAproachArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolyLine(pBuffer, 0, 2)
		'Application.DoEvents()

		LeftLine = ReturnPolygonPartAsPolyline(pBuffer, ptTmp, _ArDir, -1)
		RightLine = ReturnPolygonPartAsPolyline(pBuffer, ptTmp, _ArDir, 1)

		CalcArObstaclesNavMOC(ObstacleList, MAPtObstacles, FinalNav.pPtPrj, FictTHR, _ArDir, pPolyPrime, pBuffer, 2, Imx)

		On Error Resume Next
		If Not FullPolyElement Is Nothing Then pGraphics.DeleteElement(FullPolyElement)
		If Not PrimeMAPolyElement Is Nothing Then pGraphics.DeleteElement(PrimeMAPolyElement)

		FullPolyElement = DrawPolygon(pBuffer, RGB(0, 0, 255))
		ClipByLine(pPolyPrime, pCutter, pBuffer, Nothing, Nothing)
		PrimeMAPolyElement = DrawPolygon(pBuffer, 255)
		FullPolyElement.Locked = True
		PrimeMAPolyElement.Locked = True
		On Error GoTo 0

		Dim Result As Double = fFinalOCH

		N = UBound(MAPtObstacles.Parts)
		For I = 0 To N
			MAPtObstacles.Parts(I).Dist = Point2LineDistancePrj(MAPtObstacles.Parts(I).pPtPrj, pMAPt, _ArDir + 90.0) * SideDef(pMAPt, _ArDir + 90.0, MAPtObstacles.Parts(I).pPtPrj)
			fTmp = _FinalMOC * MAPtObstacles.Parts(I).fTmp + MAPtObstacles.Parts(I).Height
			fTmp2 = MAPtObstacles.Parts(I).Height + arMA_InterMOC.Value * MAPtObstacles.Parts(I).fTmp + (dMAPt2SOC - MAPtObstacles.Parts(I).Dist) * fMissAprPDG

			If fTmp2 < fTmp Then fTmp = fTmp2
			MAPtObstacles.Parts(I).EffectiveHeight = fTmp

			If Result < fTmp Then
				Result = fTmp
				ObsName = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).UnicalName
			End If
		Next I
		Sort(MAPtObstacles, 0)

		'DrawPolygon(pPolyPrime, RGB(255, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSVertical)
		'While True
		'	Application.DoEvents()
		'End While

		Return Result
	End Function

	Private Function CalcDescentSpeed(ByVal fTime As Double, ByVal hFARFAP As Double, Optional ByVal ignoreNegativ As Boolean = True) As Double
		Dim fTs As Double
		Dim fTx As Double
		Dim fRDH As Double
		Dim fTmp As Double
		Dim fTAS As Double
		Dim Hhold As Double
		Dim fDist As Double
		Dim flTime As Double
		Dim fDistNav2FTHR As Double

		fRDH = GetRDH()

		If ComboBox0501.SelectedIndex = 0 Then
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) - fRefHeight
		Else
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		End If

		flTime = IIf(OptionButton0603.Checked And CheckBox0604.Checked, 1.0 + fTime, fTime)

		fDesV = 0.016666666666666666 * (Hhold - hFARFAP) / flTime
		TextBox0604.Text = CStr(ConvertDSpeed(fDesV, 2))

		_initialTAS = IAS2TAS(_initialIAS, Hhold + fRefHeight, CurrADHP.ISAtC)
		fTAS = IAS2TAS(_finalIAS, FarFAF.Z, CurrADHP.ISAtC)

		PrjToLocal(FinalNav.pPtPrj, _ArDir, FictTHR, fDistNav2FTHR, fTmp)
		'fDistNav2FTHR = ReturnDistanceInMeters(FinalNav.pPtPrj, FictTHR) * SideDef(FinalNav.pPtPrj, m_ArDir + 90.0, FictTHR)
		fTs = 0.06 * fDistNav2FTHR / fTAS
		fTx = flTime * _initialTAS / fTAS + fTs

		If (Not ignoreNegativ) And (fTx <= 0.0) Then
			Return -fTs + 0.25
		End If

		ToolTip1.SetToolTip(TextBox0607, My.Resources.str00502 + fTx.ToString("0.00"))
		TextBox0607.Text = fTx.ToString("0.00")

		If fTx <> 0.0 Then
			fDesV = 0.016666666666666666 * (hFARFAP - fRDH) / fTx
		Else
			fDesV = arMaxHV_FAS.Values(Category)
		End If

		If fDesV >= arMaxHV_FAS.Values(Category) Then fDesV = arMaxHV_FAS.Values(Category)

		TextBox0611.ForeColor = IIf(fDesV < arMinHV_FAS.Values(Category), Color.Red, Color.Black)

		TextBox0611.Text = CStr(ConvertDSpeed(fDesV, 2))

		fhTHR = hFARFAP - 60.0 * fTx * fDesV + fRefHeight - FictTHR.Z
		fTmp = fRDH + fRefHeight - FictTHR.Z

		If fhTHR - 0.05 > fTmp Then
			TextBox0606.ForeColor = Color.Red
		Else
			TextBox0606.ForeColor = Color.Black
			If fhTHR < fTmp Then fhTHR = fTmp
		End If

		TextBox0606.Text = CStr(ConvertHeight(fhTHR, eRoundMode.NEAREST))

		'=========================================================================

		ArrivalProfile.ClearPoints()

		fTmp = Point2LineDistancePrj(FictTHR, FinalNav.pPtPrj, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90, FinalNav.pPtPrj)
		ArrivalProfile.AddPoint(fTmp, Hhold, CDbl(TextBox0612.Text), 0, -1)

		fDist = Point2LineDistancePrj(FictTHR, FarFAF, _ArDir + 90.0)
		ArrivalProfile.AddPoint(fDist, hFARFAP, CDbl(TextBox0612.Text), 0, -1)

		'fTAS = IAS2TAS(3.6 * cVfafMax.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
		'fTmp = 0.06 * fDesV / fTAS

		CPDGmin = 3.6 * fDesV / IAS2TAS(3.6 * cVfafMin.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
		If CPDGmin > arFADescent_Nom.Value Then CPDGmin = arFADescent_Nom.Value

		fDist = (hFARFAP - fhTHR) / CPDGmin
		ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), CPDGmin, CodeProcedureDistance.FAF)

		fDist = (fFinalOCH - fhTHR) / CPDGmin
		ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), CPDGmin, -1)

		Return fTime
	End Function

	Private Function CalcNoFAF(ByRef fTime As Double) As Double
		Dim fRDH As Double
		Dim fSDFh As Double
		Dim Hhold As Double
		Dim HIafMin As Double
		Dim HIafMax As Double
		Dim FinalMOC As Double

		fRDH = GetRDH()

		CreateFinalBaseArea()   '?????????????????????????????????????????????????

		'    fDist = ReturnDistanceInMeters(FarFAF, FictTHR)
		'    If fDist > arMOCChangeDist.Value Then
		'        FinalMOC = arFASeg_FAF_MOC.Value + (fDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
		'        If FinalMOC < arFASegmentMOC.Value Then FinalMOC = arFASegmentMOC.Value
		'    Else
		'        FinalMOC = arFASegmentMOC.Value
		'    End If
		'    If FinalMOC > arISegmentMOC.Value Then FinalMOC = arISegmentMOC.Value

		FinalMOC = DeConvertHeight(CDbl(ComboBox0604.Text)) 'arFASegmentMOC.Value

		CalcArObstaclesNavMOC(BuffObstacleList, NavObstacleList, FinalNav.pPtPrj, FictTHR, _ArDir, NavGuadArea, FinalBaseArea, FinalMOC, NavIx)
		NonPrecReportFrm.FillPage03(NavObstacleList, NavIx, NavJx)

		TextBox0615.Text = ""
		fFinalOCH = fMinOCH

		If NavIx >= 0 Then
			If NavObstacleList.Parts(NavIx).ReqH > fMinOCH Then
				fFinalOCH = NavObstacleList.Parts(NavIx).ReqH
				IxFinalOCH = NavIx
				TextBox0615.Text = NavObstacleList.Obstacles(NavObstacleList.Parts(NavIx).Owner).UnicalName
			End If
		End If

		ComboBox0605_SelectedIndexChanged(ComboBox0605, Nothing)

		If ComboBox0501.SelectedIndex = 0 Then
			fSDFh = DeConvertHeight(CDbl(TextBox0608.Text)) - fRefHeight
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) - fRefHeight
		Else
			fSDFh = DeConvertHeight(CDbl(TextBox0608.Text))
			Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
		End If

		If fSDFh < fRDH Then fSDFh = fRDH

		If fSDFh > Hhold Then
			MessageBox.Show(My.Resources.str00501)
			TextBox0608.ForeColor = Color.Red
		Else
			TextBox0608.ForeColor = Color.Black
		End If

		'OutBoundDes = arMaxOutBoundDes.Values(Category)	'/ arMaxOutBoundDes.Multiplier
		'    OutBoundDes = DeConvertDSpeed(CDbl(TextBox0604.Text))
		If OptionButton0603.Checked And CheckBox0604.Checked Then
			HIafMin = Hhold - 60.0 * (1.0 + fTime) * arMaxOutBoundDes.Values(Category)
		Else
			HIafMin = Hhold - 60.0 * fTime * arMaxOutBoundDes.Values(Category)
		End If

		If HIafMin < fSDFh Then HIafMin = fSDFh

		HIafMax = Hhold
		If HIafMax < HIafMin Then HIafMax = HIafMin

		If ComboBox0501.SelectedIndex = 0 Then
			LimitTextBox0605.Low = HIafMin + fRefHeight
			LimitTextBox0605.High = HIafMax + fRefHeight
		Else
			LimitTextBox0605.Low = HIafMin
			LimitTextBox0605.High = HIafMax
		End If

		ToolTip1.SetToolTip(TextBox0605, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(LimitTextBox0605.Low, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0605.High, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit)

		FarFAF.Z = HIafMin + fRefHeight
		If ComboBox0501.SelectedIndex = 0 Then
			TextBox0605.Text = CStr(ConvertHeight(FarFAF.Z, eRoundMode.NEAREST))
		Else
			TextBox0605.Text = CStr(ConvertHeight(HIafMin, eRoundMode.NEAREST))
		End If

		'=========================================================================

		On Error Resume Next
		If Not FarFAFElem Is Nothing Then pGraphics.DeleteElement(FarFAFElem)
		On Error GoTo 0

		FarFAFElem = DrawPointWithText(FarFAF, "", WPTColor)
		FarFAFElem.Locked = True
		'=========================================================================

		Return CalcDescentSpeed(fTime, HIafMin, False)
	End Function

	Private Function MissApprInterRange(ByRef OCH As Double, ByRef PDG As Double, ByRef Ix As Integer, ByRef Jx As Integer) As Double
		Dim i As Integer
		Dim n As Integer
		Dim m As Integer
		Dim XI As Double
		Dim fTmp As Double

		Dim TmpObstacles As ObstacleContainer

		n = UBound(MAPtObstacles.Parts)
		m = UBound(MAPtObstacles.Obstacles)

		If n >= 0 Then
			ReDim TmpObstacles.Parts(n)
			ReDim TmpObstacles.Obstacles(m)
		Else
			ReDim TmpObstacles.Parts(-1)
			ReDim TmpObstacles.Obstacles(-1)
		End If

		MissApprInterRange = dMAPt2SOC
		Ix = -1
		For i = 0 To m
			TmpObstacles.Obstacles(i) = MAPtObstacles.Obstacles(i)
		Next

		For i = 0 To n
			MAPtObstacles.Parts(i).Rmin = arMA_FinalMOC.Value * MAPtObstacles.Parts(i).fTmp 'MOC50
			MAPtObstacles.Parts(i).Rmax = MAPtObstacles.Parts(i).Height + MAPtObstacles.Parts(i).Rmin   'ReqH50

			fTmp = (MAPtObstacles.Parts(i).Dist - dMAPt2SOC) * PDG
			MAPtObstacles.Parts(i).ReqOCH = MAPtObstacles.Parts(i).ReqH - fTmp
			'    If fTmp < 0.0 Then fTmp = 0.0
			MAPtObstacles.Parts(i).hPenet = OCH + fTmp  'H over Obst

			XI = (MAPtObstacles.Parts(i).Rmax - OCH) / PDG + dMAPt2SOC

			If (XI > MAPtObstacles.Parts(i).Dist) And (XI > MissApprInterRange) Then
				MissApprInterRange = XI
				Ix = i
			End If
		Next i

		For i = 0 To n
			MAPtObstacles.Parts(i).Flags = MAPtObstacles.Parts(i).Flags And 1
			If MAPtObstacles.Parts(i).Dist > MissApprInterRange Then
				MAPtObstacles.Parts(i).Flags = MAPtObstacles.Parts(i).Flags + 4
			ElseIf MAPtObstacles.Parts(i).Dist > dMAPt2SOC Then
				MAPtObstacles.Parts(i).Flags = MAPtObstacles.Parts(i).Flags + 2
			Else
				If (MAPtObstacles.Parts(i).ReqH < MAPtObstacles.Parts(i).hPenet) And (MAPtObstacles.Parts(i).Height + _FinalMOC * MAPtObstacles.Parts(i).fTmp > OCH) Then
					MAPtObstacles.Parts(i).Flags = MAPtObstacles.Parts(i).Flags Or 8
					'        ElseIf MAPtObstacles(I).Height + FinalMOC * MAPtObstacles(I).fTmp > OCH Then
					'            MAPtObstacles(I).Flags = MAPtObstacles(I).Flags Or 16
				End If
			End If

			TmpObstacles.Parts(i) = MAPtObstacles.Parts(i)
			TmpObstacles.Parts(i).Dist = MAPtObstacles.Parts(i).Dist - dMAPt2SOC
		Next i

		MissApprInterRange = MissApprInterRange - dMAPt2SOC
		NonPrecReportFrm.FillPage05(TmpObstacles, _FinalMOC, PDG, Ix)
	End Function

	Private Sub CreateFinalBaseArea()
		Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLSPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRSPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFixTolerArea As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pFALFullPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTrackPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pTmpPoly0 As ESRI.ArcGIS.Geometry.IPolygon
		Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPolygon

		Dim Clone As ESRI.ArcGIS.esriSystem.IClone
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		Dim InterToler As Double
		Dim TrackToler As Double
		Dim fTmpH As Double
		Dim fDirl As Double
		Dim fDis As Double
		Dim fTmp As Double
		Dim d0 As Double
		Dim D As Double

		Dim Side As Integer

		Dim sdfNavaid As NavaidData
		Dim bHaveSDF As Boolean
		Dim bHaveFIX As Boolean

		bHaveSDF = (MultiPage1.SelectedIndex > 6) And (CheckBox0701.Checked)
		bHaveFIX = True
		If bHaveSDF Then
			If ComboBox0702.SelectedIndex < 0 Then Return
			sdfNavaid = SDFNavDat(ComboBox0702.SelectedIndex)
			ptTmpFAF = PtSDF
		ElseIf OptionButton0201.Checked Then
			If ComboBox0302.SelectedIndex < 0 Then Return
			sdfNavaid = FAFNavDat(ComboBox0302.SelectedIndex)
			If sdfNavaid.IntersectionType = eIntersectionType.OnNavaid Then _Label0301_4.Text = My.Resources.str00106
			ptTmpFAF = PtFAF
		Else
			bHaveFIX = False
		End If

		'=======================================
		On Error Resume Next

		If Not FinalBaseAreaElem Is Nothing Then pGraphics.DeleteElement(FinalBaseAreaElem)
		If Not FinalSecAreaElem Is Nothing Then pGraphics.DeleteElement(FinalSecAreaElem)
		On Error GoTo 0

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		If OptionButton0201.Checked Then
			Clone = pFullPoly
			pFALFullPoly = Clone.Clone

			Clone = pPolyLeft
			pLSPoly = Clone.Clone

			Clone = pPolyRight
			pRSPoly = Clone.Clone

			pCutter.FromPoint = PointAlongPlane(PtFAF, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(PtFAF, _ArDir + 90.0, 10.0 * RModel)

			CutPoly(pFALFullPoly, pCutter, -1)
			CutPoly(pLSPoly, pCutter, -1)
			CutPoly(pRSPoly, pCutter, -1)
		Else
			pCutter.FromPoint = PointAlongPlane(FarFAF, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(FarFAF, _ArDir + 90.0, 10.0 * RModel)
			'==================================================================================
			Clone = pFullPoly
			pTmpPoly0 = Clone.Clone

			pTopo = PrimaryArea
			pTmpPoly1 = pTopo.Intersect(pTmpPoly0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			pTopo = pTmpPoly1
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			CutPoly(pTmpPoly0, pCutter, -1)
			pFALFullPoly = pTopo.Union(pTmpPoly0)
			'==================================================================================
			Clone = pPolyLeft
			pTmpPoly0 = Clone.Clone

			pTopo = PrimaryArea
			pTmpPoly1 = pTopo.Intersect(pTmpPoly0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			pTopo = pTmpPoly1
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			CutPoly(pTmpPoly0, pCutter, -1)
			pLSPoly = pTopo.Union(pTmpPoly0)

			pTopo = pLSPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
			'==================================================================================
			Clone = pPolyRight
			pTmpPoly0 = Clone.Clone

			pTopo = PrimaryArea
			pTmpPoly1 = pTopo.Intersect(pTmpPoly0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			pTopo = pTmpPoly1
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			CutPoly(pTmpPoly0, pCutter, -1)
			pRSPoly = pTopo.Union(pTmpPoly0)

			pTopo = pRSPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If

		If bHaveSDF Then
			Clone = pFALFullPoly
			SDFFinalBaseArea = Clone.Clone

			Clone = pLSPoly
			pTmpPoly0 = Clone.Clone

			Clone = pRSPoly
			pTmpPoly1 = Clone.Clone

			'CutPoly(SDFFinalBaseArea, pCutter, -1)
			'CutPoly(pTmpPoly0, pCutter, -1)
			'CutPoly(pTmpPoly1, pCutter, -1)

			'=======================
			pCutter.FromPoint = PointAlongPlane(ptTmpFAF, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(ptTmpFAF, _ArDir + 90.0, 10.0 * RModel)

			CutPoly(SDFFinalBaseArea, pCutter, 1)
			CutPoly(pTmpPoly0, pCutter, 1)
			CutPoly(pTmpPoly1, pCutter, 1)

			'=======================
			If OptionButton0201.Checked Then
				pTopo = SDFFinalBaseArea
				SDFFinalBaseArea = pTopo.Union(pFAFTolerArea)
				pTopo = SDFFinalBaseArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
			End If

			pTopo = pTmpPoly0
			SDFFinalSecondArea = pTopo.Union(pTmpPoly1)

			pTopo = SDFFinalSecondArea
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			CutPoly(pFALFullPoly, pCutter, -1)
			CutPoly(pLSPoly, pCutter, -1)
			CutPoly(pRSPoly, pCutter, -1)
		End If

		FinalBaseArea = pFALFullPoly

		If bHaveFIX Then
			If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
				TrackToler = VOR.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
				TrackToler = NDB.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
				TrackToler = LLZ.TrackingTolerance
			End If

			pTrackPoly = New ESRI.ArcGIS.Geometry.Polygon
			pTrackPoly.AddPoint(FinalNav.pPtPrj)
			pTrackPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, 5.0 * RModel))
			pTrackPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, 5.0 * RModel))
			pTrackPoly.AddPoint(FinalNav.pPtPrj)

			If FinalNav.TypeCode <> eNavaidType.LLZ Then
				pTrackPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, 5.0 * RModel))
				pTrackPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, 5.0 * RModel))
				pTrackPoly.AddPoint(FinalNav.pPtPrj)
			End If

			pTopo = pTrackPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			Select Case sdfNavaid.IntersectionType
				Case eIntersectionType.OnNavaid
					OnNavaidFIXTolerArea(sdfNavaid, _ArDir, CurrhFAF + fRefHeight, pFixTolerArea)
				Case eIntersectionType.ByAngle
					fDirl = ReturnAngleInDegrees(sdfNavaid.pPtPrj, ptTmpFAF)
					fTmp = Dir2Azt(sdfNavaid.pPtPrj, fDirl)
					If (sdfNavaid.TypeCode = eNavaidType.VOR) Or (sdfNavaid.TypeCode = eNavaidType.TACAN) Then
						InterToler = VOR.IntersectingTolerance
						fDis = VOR.Range
					ElseIf sdfNavaid.TypeCode = eNavaidType.NDB Then
						InterToler = NDB.IntersectingTolerance
						fDis = NDB.Range
					ElseIf sdfNavaid.TypeCode = eNavaidType.LLZ Then
						InterToler = LLZ.IntersectingTolerance
						fDis = LLZ.Range
					Else
						Return
					End If

					If Not bHaveSDF Then
						If sdfNavaid.TypeCode = eNavaidType.NDB Then
							_Label0301_4.Text = My.Resources.str00228 + CStr(ConvertAngle(Modulus(fTmp + 180.0))) + "°"
						Else
							_Label0301_4.Text = My.Resources.str00227 + CStr(ConvertAngle(fTmp)) + "°"
						End If
					End If

					pt1 = PointAlongPlane(sdfNavaid.pPtPrj, fDirl + InterToler, fDis)
					pt2 = PointAlongPlane(sdfNavaid.pPtPrj, fDirl - InterToler, fDis)

					pSect0 = New ESRI.ArcGIS.Geometry.Polygon
					pSect0.AddPoint(sdfNavaid.pPtPrj)
					pSect0.AddPoint(pt1)
					pSect0.AddPoint(pt2)
					pSect0.AddPoint(sdfNavaid.pPtPrj)
					pTopo = pSect0
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					pFixTolerArea = pTopo.Intersect(pTrackPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
					pTopo = pFixTolerArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
				Case eIntersectionType.ByDistance
					fDirl = ReturnDistanceInMeters(sdfNavaid.pPtPrj, ptTmpFAF)
					fTmpH = CurrhFAF - sdfNavaid.pPtPrj.Z + fRefHeight

					d0 = System.Math.Sqrt(fDirl * fDirl + fTmpH * fTmpH)

					If Not bHaveSDF Then
						_Label0301_4.Text = My.Resources.str00229 + CStr(ConvertDistance(fDirl - sdfNavaid.Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + vbCrLf _
						 + My.Resources.str10511 + CStr(ConvertDistance(d0 - sdfNavaid.Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit
					End If

					d0 = DME.MinimalError + d0 * DME.ErrorScalingUp
					D = fDirl + d0
					pSect0 = CreatePrjCircle(sdfNavaid.pPtPrj, D)

					pCutter.FromPoint = PointAlongPlane(sdfNavaid.pPtPrj, _ArDir - 90.0, D + D)
					pCutter.ToPoint = PointAlongPlane(sdfNavaid.pPtPrj, _ArDir + 90.0, D + D)


					pSect1 = CreatePrjCircle(sdfNavaid.pPtPrj, fDirl - d0)

					pTopo = pSect0
					pFixTolerArea = pTopo.Difference(pSect1)

					pTopo = pFixTolerArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					If SideDef(pCutter.FromPoint, _ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

					If SideDef(sdfNavaid.pPtPrj, _ArDir + 90.0, ptTmpFAF) > 0 Then
						pTopo.Cut(pCutter, pSect1, pSect0)
					Else
						pTopo.Cut(pCutter, pSect0, pSect1)
					End If

					pTopo = pSect0
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					pFixTolerArea = pTopo.Intersect(pTrackPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
					pTopo = pFixTolerArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
			End Select

			If bHaveSDF Then
				pSDFTolerArea = pFixTolerArea
			Else
				pFAFTolerArea = pFixTolerArea
			End If
			'pFAFElem = DrawPolygon(pTmpPoly, 0)

			pTopo = pFALFullPoly
			FinalBaseArea = pTopo.Union(pFixTolerArea)
		End If

		pTopo = FinalBaseArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'====
		If Not pMAPt Is Nothing Then
			ptTmp = PointAlongPlane(pMAPt, _ArDir, 0.05)
		Else
			Side = SideDef(FictTHR, _ArDir + 90.0, FinalNav.pPtPrj)
			If (FinalNav.TypeCode <> eNavaidType.LLZ) And _OnAero And (Side > 0) Then
				ptTmp = PointAlongPlane(FinalNav.pPtPrj, _ArDir, 0.05)
			Else
				ptTmp = PointAlongPlane(FictTHR, _ArDir, 0.05)
			End If
		End If

		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 10.0 * RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 10.0 * RModel)

		CutPoly(FinalBaseArea, pCutter, 1)
		CutPoly(pLSPoly, pCutter, 1)
		CutPoly(pRSPoly, pCutter, 1)
		'====
		pTopo = pRSPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pLSPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		FinalSecondArea = pTopo.Union(pRSPoly)
		pTopo = FinalSecondArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		On Error Resume Next
		FinalBaseAreaElem = DrawPolygon(FinalBaseArea, RGB(255, 0, 127))
		FinalBaseAreaElem.Locked = True

		FinalSecAreaElem = DrawPolygon(FinalSecondArea, RGB(0, 127, 255))
		FinalSecAreaElem.Locked = True

		If bHaveSDF Then
			'If Not SDFElem Is Nothing Then pGraphics.DeleteElement(SDFElem)		????
			If Not PtSDFElem Is Nothing Then pGraphics.DeleteElement(PtSDFElem)
			PtSDFElem = DrawPointWithText(ptTmpFAF, "SDF", WPTColor)
			PtSDFElem.Locked = True
		ElseIf bHaveFIX Then
			If Not FAFFIXElem Is Nothing Then pGraphics.DeleteElement(FAFFIXElem)
			FAFFIXElem = DrawPointWithText(ptTmpFAF, "FAF", WPTColor)
			FAFFIXElem.Locked = True
		End If

		On Error GoTo 0

		pCutter = Nothing
	End Sub

	Private Sub NextObstInfo(ByRef Ix As Integer)
		Dim I As Integer
		Dim N As Integer

		ToolTip1.SetToolTip(TextBox0302, "")
		ToolTip1.SetToolTip(TextBox0305, "")
		ToolTip1.SetToolTip(TextBox0304, "")

		If Ix < 0 Then Return
		N = FAFObstacleList.Parts.Length - 1

		For I = Ix + 1 To N
			If FAFObstacleList.Parts(Ix).ReqH < FAFObstacleList.Parts(I).ReqH Then
				'ToolTip1.SetToolTip(TextBox0302, My.Resources.str10412 + CStr(ConvertHeight(FAFObstacleList(I).ReqH, eRoundMode.rmNERAEST))
				ToolTip1.SetToolTip(TextBox0305, My.Resources.str10413 + CStr(ConvertDistance(FAFObstacleList.Parts(I).Dist, eRoundMode.NEAREST)))
				ToolTip1.SetToolTip(TextBox0304, My.Resources.str10414 + FAFObstacleList.Obstacles(FAFObstacleList.Parts(I).Owner).UnicalName)
				Exit For
			End If
		Next I
	End Sub

	Private Sub CalcSDF_NoFAFOCH()
		Dim I As Integer
		Dim N As Integer
		Dim M As Integer

		Dim Ix As Integer

		Dim fL As Double
		Dim fReqH As Double
		Dim fDist As Double
		Dim fNewOCH As Double
		Dim f93Dist As Double
		Dim fObsDist As Double
		Dim fPlane15h As Double
		Dim fFASeg_MOC As Double
		Dim fAppHeight As Double

		Dim ptFar As ESRI.ArcGIS.Geometry.IPoint
		Dim pDistLine As ESRI.ArcGIS.Geometry.IPolyline

		Dim tmpObstacleList As ObstacleContainer
		Dim pProximity As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		If CheckBox0701.Checked Then
			pDistLine = New ESRI.ArcGIS.Geometry.Polyline
			ptFar = PointAlongPlane(PtFAF, _ArDir + 180.0, 100000.0)

			pDistLine.FromPoint = PointAlongPlane(ptFar, _ArDir + 90.0, 100000.0)
			pDistLine.ToPoint = PointAlongPlane(ptFar, _ArDir - 90.0, 100000.0)

			pProximity = pDistLine
			fDist = pProximity.ReturnDistance(FinalBaseArea)

			ptFar = PointAlongPlane(PtFAF, _ArDir + 180.0, 100000.0 - fDist)
		End If
		'==========================================================

		If OptionButton0101.Checked Then
			fNewOCH = Max(arFASeg_FAF_MOC.Value, fAlignOCH)
		Else
			fNewOCH = fNewVisAprOCH
		End If

		SDFOCH = fNewOCH

		fDist = ReturnDistanceInMeters(PtFAF, FictTHR)

		If fDist > arMOCChangeDist.Value Then
			fFASeg_MOC = arFASeg_FAF_MOC.Value '+ (fDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
			'    If fFASeg_MOC < arFASegmentMOC.Value Then fFASeg_MOC = arFASegmentMOC.Value
		Else
			fFASeg_MOC = arFASeg_FAF_MOC.Value 'arFASegmentMOC.Value
		End If

		'If fFASeg_MOC > arISegmentMOC.Value Then fFASeg_MOC = arISegmentMOC.Value

		dMAPt2FAF = ReturnDistanceInMeters(PtFAF, FictTHR)

		If CheckBox0701.Checked Then
			If dMAPt2FAF > arMOCChangeDist.Value Then
				_FinalMOC = arFASeg_FAF_MOC.Value '+ (dMAPt2FAF - arMOCChangeDist.Value) * arAddMOCCoef.Value
			Else
				_FinalMOC = arFASeg_FAF_MOC.Value
			End If
		Else
			'    If dMAPt2FAF > arMOCChangeDist.Value Then
			'        FinalMOC = arFASeg_FAF_MOC.Value '+ (dMAPt2FAF - arMOCChangeDist.Value) * arAddMOCCoef.Value
			'        If FinalMOC < arFASegmentMOC.Value Then FinalMOC = arFASegmentMOC.Value
			'    Else
			_FinalMOC = arFASegmentMOC.Value
			'    End If
		End If

		If _FinalMOC > arISegmentMOC.Value Then _FinalMOC = arISegmentMOC.Value

		fAppHeight = PtFAF.Z - fFASeg_MOC
		fL = fAppHeight / arFixMaxIgnorGrd.Value

		'==========================================================
		N = UBound(NavObstacleList.Parts)
		M = UBound(NavObstacleList.Obstacles)
		If N >= 0 Then
			ReDim tmpObstacleList.Parts(N)
			ReDim tmpObstacleList.Obstacles(M)
		Else
			ReDim tmpObstacleList.Parts(-1)
			ReDim tmpObstacleList.Obstacles(-1)
		End If

		Ix = -1
		pRelation = FinalBaseArea
		For I = 0 To M
			tmpObstacleList.Obstacles(I) = NavObstacleList.Obstacles(I)
		Next

		For I = 0 To N
			tmpObstacleList.Parts(I) = NavObstacleList.Parts(I)
			If pRelation.Contains(tmpObstacleList.Parts(I).pPtPrj) Then
				fObsDist = Point2LineDistancePrj(FictTHR, tmpObstacleList.Parts(I).pPtPrj, _ArDir - 90.0)
				tmpObstacleList.Parts(I).Dist = fObsDist
				tmpObstacleList.Parts(I).MOC = _FinalMOC * tmpObstacleList.Parts(I).fTmp
				tmpObstacleList.Parts(I).ReqH = tmpObstacleList.Parts(I).Height + tmpObstacleList.Parts(I).MOC

				fReqH = tmpObstacleList.Parts(I).ReqH
				tmpObstacleList.Parts(I).Flags = tmpObstacleList.Parts(I).Flags And 1

				If CheckBox0701.Checked Then
					f93Dist = Point2LineDistancePrj(tmpObstacleList.Parts(I).pPtPrj, ptFar, _ArDir + 90.0)
					If f93Dist < arFIX15PlaneRang.Value Then
						fPlane15h = arFixMaxIgnorGrd.Value * (fL - f93Dist)
						If fPlane15h > tmpObstacleList.Parts(I).Height Then
							fReqH = 0.0
							tmpObstacleList.Parts(I).Flags = tmpObstacleList.Parts(I).Flags Or 2
						End If
					End If
				End If

				If SDFOCH < fReqH Then
					SDFOCH = tmpObstacleList.Parts(I).ReqH
					Ix = I
				End If
			End If
		Next I

		NonPrecReportFrm.FillPage03(tmpObstacleList, Ix, True)
		'=====================================================

		'If _OptionButton0101_0 Then
		'    IxFinalOCH = -2             'OCH определяется сопряжением конечного участка
		'Else
		'    IxFinalOCH = -1             'OCH определяет OCH Зоны Визуального Маневрирования
		'End If
		TextBox0708.Text = ""

		If ComboBox0705.SelectedIndex < 0 Then
			ComboBox0705.SelectedIndex = 0
		Else
			ComboBox0705_SelectedIndexChanged(ComboBox0705, New System.EventArgs())
		End If

		If Ix >= 0 Then
			If tmpObstacleList.Parts(Ix).ReqH > fMinOCH Then
				TextBox0708.Text = tmpObstacleList.Obstacles(tmpObstacleList.Parts(Ix).Owner).UnicalName
			End If
		End If
	End Sub

	Private Sub CalcSDF_FAFOCH()
		Dim I As Integer
		Dim N As Integer
		Dim M As Integer

		Dim IxJO As Integer
		Dim iSide As Integer

		Dim fL As Double
		Dim fDist As Double
		Dim fReqH As Double
		Dim fNew_MOC As Double
		Dim fSDFDist As Double
		Dim fPlane15h As Double
		Dim fFAFRange As Double
		Dim fFASeg_MOC As Double
		Dim fAppHeight As Double

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pFarPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pBorderLine As ESRI.ArcGIS.Geometry.IPolyline

		Dim tmpObstacleList As ObstacleContainer
		Dim pProximity As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator

		fFAFRange = DeConvertDistance(CDbl(TextBox0301.Text))

		If fFAFRange > arMOCChangeDist.Value Then
			fFASeg_MOC = arFASeg_FAF_MOC.Value '+ (fFAFRange - arMOCChangeDist.Value) * arAddMOCCoef.Value
		Else
			fFASeg_MOC = arFASeg_FAF_MOC.Value
		End If

		fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
		If fSDFDist > arMOCChangeDist.Value Then
			fNew_MOC = arFASeg_FAF_MOC.Value '+ (fSDFDist - arMOCChangeDist.Value) * arAddMOCCoef.Value
		Else
			fNew_MOC = arFASeg_FAF_MOC.Value
		End If

		M = UBound(FAFWorkObstacleList.Obstacles)
		If N >= 0 Then
			ReDim tmpObstacleList.Obstacles(M)
		Else
			ReDim tmpObstacleList.Obstacles(-1)
		End If

		For I = 0 To M
			tmpObstacleList.Obstacles(I) = FAFWorkObstacleList.Obstacles(I)
		Next

		N = UBound(FAFWorkObstacleList.Parts)
		If N >= 0 Then
			ReDim tmpObstacleList.Parts(N)
		Else
			ReDim tmpObstacleList.Parts(-1)
		End If

		pBorderLine = New ESRI.ArcGIS.Geometry.Polyline

		ptTmp = PointAlongPlane(PtSDF, _ArDir + 180.0, 1000000.0)
		pBorderLine.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 1000000.0)
		pBorderLine.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 1000000.0)

		pProximity = pBorderLine
		fDist = pProximity.ReturnDistance(pSDFTolerArea)
		pFarPt = PointAlongPlane(ptTmp, _ArDir, fDist)

		'Set pProximity = pSDFTolerArea
		'Set pFarPt = pProximity.ReturnNearestPoint(ptTmp, esriNoExtension)

		IxJO = -1

		SDFOCH = fMinOCH
		fAppHeight = PtSDF.Z - fFASeg_MOC 'arISegmentMOC.Value
		fL = fAppHeight / arFixMaxIgnorGrd.Value
		pRelational = pSDFTolerArea

		For I = 0 To N
			tmpObstacleList.Parts(I) = FAFWorkObstacleList.Parts(I)

			iSide = SideDef(PtSDF, _ArDir + 90.0, tmpObstacleList.Parts(I).pPtPrj)

			If (pRelational.Contains(tmpObstacleList.Parts(I).pPtPrj)) Or (iSide > 0) Then
				tmpObstacleList.Parts(I).MOC = fNew_MOC * tmpObstacleList.Parts(I).fTmp
				tmpObstacleList.Parts(I).ReqH = tmpObstacleList.Parts(I).Height + tmpObstacleList.Parts(I).MOC

				fDist = Point2LineDistancePrj(tmpObstacleList.Parts(I).pPtPrj, pFarPt, _ArDir + 90.0)
				If fDist < arFIX15PlaneRang.Value Then
					fPlane15h = arFixMaxIgnorGrd.Value * (fL - fDist)
					If fPlane15h > tmpObstacleList.Parts(I).Height Then
						fReqH = 0.0
						tmpObstacleList.Parts(I).Flags = tmpObstacleList.Parts(I).Flags Or 2
					Else
						fReqH = tmpObstacleList.Parts(I).ReqH
						tmpObstacleList.Parts(I).Flags = tmpObstacleList.Parts(I).Flags And 1
					End If
				Else
					fReqH = tmpObstacleList.Parts(I).ReqH
					tmpObstacleList.Parts(I).Flags = tmpObstacleList.Parts(I).Flags And 1
				End If

				If fReqH > SDFOCH Then
					SDFOCH = fReqH
					IxJO = I
				End If
			End If
		Next I

		If ComboBox0705.SelectedIndex < 0 Then
			ComboBox0705.SelectedIndex = 0
		Else
			ComboBox0705_SelectedIndexChanged(ComboBox0705, New System.EventArgs())
		End If

		TextBox0708.Text = ""

		If IxJO >= 0 Then
			TextBox0708.Text = tmpObstacleList.Obstacles(tmpObstacleList.Parts(IxJO).Owner).UnicalName
		End If

		NonPrecReportFrm.FillPage03(tmpObstacleList, IxJO, True)
		TextBox0712_Validating(TextBox0712, New CancelEventArgs())
	End Sub

	'Private Function CreateGuidPoly(ByRef GuidNav As NavaidData, ByRef ptFIX As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.Polygon
	'	Dim fDist As Double
	'	Dim fTmp As Double
	'	Dim fIADir As Double
	'	Dim fSlDist As Double
	'	Dim fRadius As Double
	'	Dim TrackRange As Double
	'	Dim TrackToler As Double
	'	Dim pBaseLine As ESRI.ArcGIS.Geometry.ILine
	'	Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

	'	If GuidNav.TypeCode = eNavaidType.DME Then
	'		pBaseLine = New ESRI.ArcGIS.Geometry.Line
	'		pBaseLine.FromPoint = GuidNav.pPtPrj
	'		pBaseLine.ToPoint = ptFIX
	'		fDist = pBaseLine.Length

	'		fTmp = ptFIX.Z - GuidNav.pPtPrj.Z
	'		fSlDist = System.Math.Sqrt(fDist * fDist + fTmp * fTmp)

	'		fTmp = fSlDist * DME.ErrorScalingUp + DME.MinimalError
	'		fRadius = fDist + fTmp

	'		pTmpPoly1 = CreatePrjCircle(GuidNav.pPtPrj, fRadius)
	'		'    fTmp = DME.MinimalError - fSlDist * DME.ErrorScalingUp
	'		fRadius = fDist - fTmp

	'		pTopoOper = pTmpPoly1
	'		pPoly = pTopoOper.Difference(CreatePrjCircle(GuidNav.pPtPrj, fRadius))
	'		'===============================================================
	'	Else
	'		'=================================================================
	'		fIADir = ReturnAngleInDegrees(ptFIX, GuidNav.pPtPrj)
	'		If GuidNav.TypeCode = eNavaidType.NDB Then
	'			TrackToler = NDB.TrackingTolerance
	'		Else 'If GuidNav.TypeCode = codevor and codetacan Then
	'			TrackToler = VOR.TrackingTolerance
	'		End If

	'		TrackRange = GuidNav.Range / System.Math.Cos(DegToRad(TrackToler))

	'		pPoly = New ESRI.ArcGIS.Geometry.Polygon
	'		pPoly.AddPoint(GuidNav.pPtPrj)
	'		pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler, 100.0 * TrackRange))
	'		pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler, 100.0 * TrackRange))
	'		pPoly.AddPoint(GuidNav.pPtPrj)
	'		pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir - TrackToler + 180.0, 100.0 * TrackRange))
	'		pPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, fIADir + TrackToler + 180.0, 100.0 * TrackRange))
	'		pPoly.AddPoint(GuidNav.pPtPrj)

	'		pTopoOper = pPoly
	'		pTopoOper.IsKnownSimple_2 = False
	'		pTopoOper.Simplify()
	'	End If

	'	Return pPoly
	'End Function

	Private Sub IntersectNavChanged_WithFAF()
		Dim K As Integer
		Dim fTmp As Double
		Dim fRDH As Double
		Dim fDir As Double
		Dim fHSDF As Double
		Dim fTmpH As Double
		Dim fDist As Double
		Dim fSDist As Double
		Dim prevPDG As Double
		Dim fSDFPDG As Double
		Dim fSDFDist As Double
		Dim fFAFDist As Double
		Dim fDMEError As Double
		Dim TrackToler As Double
		Dim fInterToler As Double

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.Polygon
		Dim pBackPoly As ESRI.ArcGIS.Geometry.Polygon
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInnerCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pOuterCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pIntersectPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		SafeDeleteElement(SDFElem)
		SafeDeleteElement(PtSDFElem)

		'On Error Resume Next
		'If Not SDFElem Is Nothing Then pGraphics.DeleteElement(SDFElem)
		'If Not PtSDFElem Is Nothing Then pGraphics.DeleteElement(PtSDFElem)
		'On Error GoTo 0

		_Label0701_6.Text = ""
		_Label0701_7.Text = ""

		K = ComboBox0702.SelectedIndex
		If K < 0 Then Return

		_Label0701_6.Text = GetNavTypeName(SDFNavDat(K).TypeCode)

		fRDH = GetRDH()

		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
			fTmp = VOR.Range
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
			fTmp = NDB.Range
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
			fTmp = LLZ.Range
		Else
			'   Unknown Navaid type
		End If

		pLGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		pLGuidPoly.AddPoint(FinalNav.pPtPrj)
		pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, fTmp))
		pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, fTmp))
		pLGuidPoly.AddPoint(FinalNav.pPtPrj)

		If FinalNav.TypeCode <> eNavaidType.LLZ Then
			pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, fTmp))
			pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, fTmp))
			pLGuidPoly.AddPoint(FinalNav.pPtPrj)
		End If

		pTopoOper = pLGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		If SDFNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			pCutter = New ESRI.ArcGIS.Geometry.Polyline
			fDist = ReturnDistanceInMeters(SDFNavDat(K).pPtPrj, PtSDF)
			fDir = ReturnAngleInDegrees(SDFNavDat(K).pPtPrj, PtSDF)

			If CheckBox0702.Checked Then
				fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
			Else
				fSDFDist = DeConvertDistance(CDbl(TextBox0706.Text))
				ptTmp = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
				PtSDF.PutCoords(ptTmp.X, ptTmp.Y)               'presrve Z & M
			End If
		End If

		Select Case SDFNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				_Label0701_7.Text = My.Resources.str10814
				OnNavaidFIXTolerArea(SDFNavDat(K), _ArDir, PtFAF.Z + fRefHeight, pSDFTolerArea)
			Case eIntersectionType.ByAngle
				If (SDFNavDat(K).TypeCode = eNavaidType.VOR) Or (SDFNavDat(K).TypeCode = eNavaidType.TACAN) Then
					fInterToler = VOR.IntersectingTolerance
				ElseIf SDFNavDat(K).TypeCode = eNavaidType.NDB Then
					fInterToler = NDB.IntersectingTolerance
				ElseIf SDFNavDat(K).TypeCode = eNavaidType.LLZ Then
					fInterToler = LLZ.IntersectingTolerance
				End If

				pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
				pIntersectPoly.AddPoint(SDFNavDat(K).pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(SDFNavDat(K).pPtPrj, fDir + fInterToler + 180.0, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(PointAlongPlane(SDFNavDat(K).pPtPrj, fDir - fInterToler + 180.0, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(SDFNavDat(K).pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(SDFNavDat(K).pPtPrj, fDir + fInterToler, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(PointAlongPlane(SDFNavDat(K).pPtPrj, fDir - fInterToler, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(SDFNavDat(K).pPtPrj)
			Case eIntersectionType.ByDistance
				fTmpH = PtSDF.Z - SDFNavDat(K).pPtPrj.Z + fRefHeight
				fSDist = System.Math.Sqrt(fDist * fDist + fTmpH * fTmpH)

				_Label0701_7.Text = My.Resources.str10815 + CStr(ConvertDistance(fSDist - SDFNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit

				fDMEError = DME.MinimalError + fSDist * DME.ErrorScalingUp

				pInnerCircle = CreatePrjCircle(SDFNavDat(K).pPtPrj, fDist - fDMEError)
				pOuterCircle = CreatePrjCircle(SDFNavDat(K).pPtPrj, fDist + fDMEError)

				pCutter.FromPoint = PointAlongPlane(SDFNavDat(K).pPtPrj, fDir + 90.0, 5.0 * (fSDist + fDMEError))
				pCutter.ToPoint = PointAlongPlane(SDFNavDat(K).pPtPrj, fDir - 90.0, 5.0 * (fSDist + fDMEError))

				pTopoOper = pOuterCircle
				pTmpPoly = pTopoOper.Difference(pInnerCircle)
				pTopoOper = pTmpPoly
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()

				pTopoOper.Cut(pCutter, pIntersectPoly, pBackPoly)
		End Select

		If SDFNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			pTopoOper = pIntersectPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pSDFTolerArea = pTopoOper.Intersect(pLGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)

			pTopoOper = pSDFTolerArea
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		Else
			PtSDF.PutCoords(SDFNavDat(K).pPtPrj.X, SDFNavDat(K).pPtPrj.Y)
			fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
			TextBox0706.Text = CStr(ConvertDistance(fSDFDist, eRoundMode.NEAREST))
			'=====================================================================
			fSDFPDG = 0.01 * CDbl(TextBox0701.Text)
			fHSDF = fSDFDist * fSDFPDG + fRDH
			If ComboBox0701.SelectedIndex = 0 Then
				TextBox0702.Text = CStr(ConvertHeight(fHSDF + fRefHeight, eRoundMode.NEAREST))
			Else
				TextBox0702.Text = CStr(ConvertHeight(fHSDF, eRoundMode.NEAREST))
			End If

			fFAFDist = ReturnDistanceInMeters(FictTHR, PtFAF) 'CDbl(TextBox1306.Text)
			TextBox0711.Text = CStr(ConvertDistance(fFAFDist - fSDFDist, eRoundMode.NEAREST))
			'============================================================================
			prevPDGmax = arFADescent_Max.Values(Category)

			prevPDGmin = (CurrhFAF - fHSDF) / (fFAFDist - fSDFDist)
			If prevPDGmin < arFADescent_Min.Value Then prevPDGmin = arFADescent_Min.Value

			If prevPDGmin > prevPDGmax Then prevPDGmin = prevPDGmax
			prevPDG = arFADescent_Nom.Value
			If prevPDG < prevPDGmin Then prevPDG = prevPDGmin
			If prevPDG > prevPDGmax Then prevPDG = prevPDGmax

			ToolTip1.SetToolTip(TextBox0712, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(100.0 * prevPDGmin + 0.04999999, 1)) + My.Resources.str00222 + CStr(System.Math.Round(100.0 * prevPDGmax - 0.04999999, 1)))
			TextBox0712.Text = CStr(System.Math.Round(100.0 * prevPDG + 0.04999999, 1))
			TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
			'============================================================================
			PtSDF.Z = fHSDF
			PtSDF.M = _ArDir
			TextBox0702.Tag = fHSDF
			'=====================================================================
		End If

		If CheckBox0701.Checked Then
			SDFElem = DrawPolygon(pSDFTolerArea, RGB(0, 255, 0))
			PtSDFElem = DrawPointWithText(PtSDF, "SDF", WPTColor)
			SDFElem.Locked = True
			PtSDFElem.Locked = True
			CalcSDF_FAFOCH()
		End If

		pCutter = Nothing
		pLGuidPoly = Nothing
	End Sub

	Private Sub IntersectNavChanged_WithoutFAF()
		Dim I As Integer
		Dim fTmp As Double
		Dim fRDH As Double
		Dim fHSDF As Double
		Dim fDist As Double
		Dim fAngle As Double
		Dim prevPDG As Double
		Dim fSDFPDG As Double
		Dim fSlDist As Double
		Dim fRadius As Double
		Dim fSDFDist As Double
		Dim fFAFDist As Double
		Dim fInterDir As Double
		Dim fInterDist As Double
		Dim TrackToler As Double
		Dim IntersectToler As Double

		Dim InterNav As NavaidData
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim ppGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pOuterPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInnerPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ppInterPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		I = ComboBox0702.SelectedIndex
		If I < 0 Then Return

		InterNav = SDFNavDat(I)
		_Label0701_6.Text = GetNavTypeName(InterNav.TypeCode)

		On Error Resume Next
		If Not FAFAreaElem Is Nothing Then pGraphics.DeleteElement(FAFAreaElem)
		If Not FAFFIXElem Is Nothing Then pGraphics.DeleteElement(FAFFIXElem)
		On Error GoTo 0

		If (InterNav.IntersectionType <> eIntersectionType.OnNavaid) Then
			'fSDFDist = DeConvertDistance(CDbl(TextBox0706.Text)) 'CDbl(TextBox1545.Text) '
			'ptTmp = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
			'PtFAF.PutCoords(ptTmp.X, ptTmp.Y)
			'PtSDF = PtFAF

			If CheckBox0702.Checked Then
				fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
			Else
				fSDFDist = DeConvertDistance(CDbl(TextBox0706.Text))
				ptTmp = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
				PtSDF.PutCoords(ptTmp.X, ptTmp.Y)               'presrve Z & M
			End If
			PtSDF.Z = PtFAF.Z
			PtSDF.M = PtFAF.M
			PtFAF = PtSDF
		End If

		fDist = ReturnDistanceInMeters(PtFAF, FinalNav.pPtPrj)
		fInterDist = ReturnDistanceInMeters(InterNav.pPtPrj, PtFAF)
		fInterDir = ReturnAngleInDegrees(InterNav.pPtPrj, PtFAF)

		fRDH = GetRDH()

		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
		Else
			'   Unknown Navaid type
		End If

		ppGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		ppGuidPoly.AddPoint(FinalNav.pPtPrj)
		ppGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0 - TrackToler, fDist + 500000.0))
		ppGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + 180.0 + TrackToler, fDist + 500000.0))
		ppGuidPoly.AddPoint(FinalNav.pPtPrj)
		ppGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, fDist + 500000.0))
		ppGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, fDist + 500000.0))
		ppGuidPoly.AddPoint(FinalNav.pPtPrj)

		pTopoOper = ppGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		'ComboBox0702.Enabled = True
		'ComboBox0702.BackColor = System.Drawing.SystemColors.Window

		Select Case InterNav.IntersectionType
			Case eIntersectionType.OnNavaid
				_Label0701_7.Text = My.Resources.str10814
				'ComboBox0702.Enabled = False
				'ComboBox0702.BackColor = System.Drawing.SystemColors.Control

				OnNavaidFIXTolerArea(InterNav, _ArDir, PtFAF.Z + fRefHeight, pSDFTolerArea)

				'=========================================================================================
				PtFAF.PutCoords(InterNav.pPtPrj.X, InterNav.pPtPrj.Y)

				fSDFDist = ReturnDistanceInMeters(FictTHR, PtFAF)
				TextBox0706.Text = CStr(ConvertDistance(fSDFDist, eRoundMode.NEAREST))
				'=====================================================================
				fSDFPDG = 0.01 * CDbl(TextBox0701.Text)
				fHSDF = fSDFDist * fSDFPDG + fRDH
				If ComboBox0701.SelectedIndex = 0 Then
					TextBox0702.Text = CStr(ConvertHeight(fHSDF + fRefHeight))
				Else
					TextBox0702.Text = CStr(ConvertHeight(fHSDF))
				End If

				fFAFDist = ReturnDistanceInMeters(FictTHR, FarFAF)
				TextBox0711.Text = CStr(ConvertDistance(fFAFDist - fSDFDist, eRoundMode.NEAREST))
				'============================================================================
				prevPDGmax = arFADescent_Max.Values(Category)

				prevPDGmin = (FarFAF.Z - fRefHeight - fHSDF) / (fFAFDist - fSDFDist)
				If prevPDGmin < arFADescent_Min.Value Then prevPDGmin = arFADescent_Min.Value

				If prevPDGmin > prevPDGmax Then prevPDGmin = prevPDGmax
				prevPDG = arFADescent_Nom.Value
				If prevPDG < prevPDGmin Then prevPDG = prevPDGmin
				If prevPDG > prevPDGmax Then prevPDG = prevPDGmax
				'
				ToolTip1.SetToolTip(TextBox0712, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(100.0 * prevPDGmin + 0.04999999, 1)) + My.Resources.str00222 + CStr(System.Math.Round(100.0 * prevPDGmax - 0.04999999, 1)))
				TextBox0712.Text = CStr(System.Math.Round(100.0 * prevPDG + 0.04999999, 1))
				'============================================================================
				PtFAF.Z = fHSDF
				PtFAF.M = _ArDir
				PtSDF = PtFAF

				TextBox0702.Tag = DeConvertHeight(ConvertHeight(fHSDF))

				TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
				'=========================================================================================
			Case eIntersectionType.ByAngle
				_Label0701_7.Text = ""
				If (InterNav.TypeCode = eNavaidType.VOR) Or (InterNav.TypeCode = eNavaidType.TACAN) Then
					IntersectToler = VOR.IntersectingTolerance
				ElseIf InterNav.TypeCode = eNavaidType.NDB Then
					IntersectToler = NDB.IntersectingTolerance
				ElseIf InterNav.TypeCode = eNavaidType.LLZ Then
					IntersectToler = LLZ.IntersectingTolerance
				Else
					'   Unknown Navaid type
				End If

				ppInterPoly = New ESRI.ArcGIS.Geometry.Polygon
				ppInterPoly.AddPoint(InterNav.pPtPrj)
				ppInterPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir - IntersectToler, fInterDist + 500000.0))
				ppInterPoly.AddPoint(PointAlongPlane(InterNav.pPtPrj, fInterDir + IntersectToler, fInterDist + 500000.0))
				ppInterPoly.AddPoint(InterNav.pPtPrj)

				pTopoOper = ppInterPoly
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()

				pSDFTolerArea = pTopoOper.Intersect(ppGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
				pTopoOper = pSDFTolerArea
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
			Case eIntersectionType.ByDistance
				fTmp = PtFAF.Z - InterNav.pPtPrj.Z + fRefHeight
				fSlDist = System.Math.Sqrt(fInterDist * fInterDist + fTmp * fTmp)

				Dim fSlDistFD As Double
				fSlDistFD = System.Math.Sqrt((fInterDist - InterNav.Disp) * (fInterDist - InterNav.Disp) + fTmp * fTmp)
				_Label0701_7.Text = My.Resources.str10815 + CStr(ConvertDistance(fSlDistFD, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit

				'    _Label0701_7.Caption = "Наклонная дальность до КТ:  " + CStr(Round(0.001 * fSlDist, 1)) + " км;" + vbCrLf + _
				''                         "                            " + CStr(Round(fSlDist / 1852#, 1)) + " м.м"
				fTmp = fSlDist * DME.ErrorScalingUp + DME.MinimalError
				fRadius = fInterDist + fTmp
				pOuterPoly = CreatePrjCircle(InterNav.pPtPrj, fRadius)

				pCutter.FromPoint = PointAlongPlane(InterNav.pPtPrj, fInterDir + 90.0, 10.0 * fRadius)
				pCutter.ToPoint = PointAlongPlane(InterNav.pPtPrj, fInterDir - 90.0, 10.0 * fRadius)

				'    fTmp = fSlDist - fSlDist * (1# - DME.ErrorScalingUp) + DME.MinimalError
				fRadius = fInterDist - fTmp
				pInnerPoly = CreatePrjCircle(InterNav.pPtPrj, fRadius)

				pTopoOper = pOuterPoly
				pInnerPoly = pTopoOper.Difference(pInnerPoly)

				pTopoOper = pInnerPoly
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()

				pTopoOper.Cut(pCutter, ppInterPoly, pOuterPoly)

				pTopoOper = ppInterPoly
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()

				pSDFTolerArea = pTopoOper.Intersect(ppGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
				pTopoOper = pSDFTolerArea
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
		End Select

		If CheckBox0701.Checked Then
			FAFAreaElem = DrawPolygon(pSDFTolerArea, RGB(128, 128, 0))
			FAFAreaElem.Locked = True

			FAFFIXElem = DrawPointWithText(PtFAF, "SDF", WPTColor)
			FAFFIXElem.Locked = True
		End If

		ppGuidPoly = Nothing
		ppGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		'================================================================================
		If FinalNav.TypeCode = eNavaidType.NDB Then
			fAngle = NDB.SplayAngle
			fTmp = 0.5 * NDB.InitWidth
		ElseIf (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			fAngle = VOR.SplayAngle
			fTmp = 0.5 * VOR.InitWidth
			'ElseIf FinalNav.TypeCode = eNavaidType.CodeLLZ Then
			'    fAngle = LLZ.SplayAngle
			'    fTmp = 0.5 * LLZ.InitWidth
		Else
			MessageBox.Show(My.Resources.str00531)  '"Unknown type of Facility."
			Return
		End If

		fDist = ReturnDistanceInMeters(FarFAF, FinalNav.pPtPrj)
		pt0 = PointAlongPlane(FinalNav.pPtPrj, _ArDir + 90.0, fTmp)
		pt1 = PointAlongPlane(FinalNav.pPtPrj, _ArDir - 90.0, fTmp)

		ppGuidPoly.AddPoint(PointAlongPlane(pt0, _ArDir + 180.0 - fAngle, fDist + 100000.0))
		ppGuidPoly.AddPoint(pt0)
		ppGuidPoly.AddPoint(PointAlongPlane(pt0, _ArDir + fAngle, fDist + 100000.0))

		ppGuidPoly.AddPoint(PointAlongPlane(pt1, _ArDir - fAngle, fDist + 100000.0))
		ppGuidPoly.AddPoint(pt1)
		ppGuidPoly.AddPoint(PointAlongPlane(pt1, _ArDir + 180.0 + fAngle, fDist + 100000.0))

		pTopoOper = ppGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
		'================================================================================
		pCutter.FromPoint = PointAlongPlane(PtFAF, _ArDir + 90.0, 500000.0)
		pCutter.ToPoint = PointAlongPlane(PtFAF, _ArDir - 90.0, 500000.0)
		pTopoOper.Cut(pCutter, ppGuidPoly, pInnerPoly)
		'================================================================================

		pCutter.FromPoint = PointAlongPlane(FictTHR, _ArDir + 90.0, 500000.0)
		pCutter.ToPoint = PointAlongPlane(FictTHR, _ArDir - 90.0, 500000.0)

		pTopoOper = ppGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pTopoOper.Cut(pCutter, pInnerPoly, pOuterPoly)

		pTopoOper = pOuterPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		FinalBaseArea = pTopoOper.Union(pSDFTolerArea)

		If CheckBox0701.Checked Then CalcSDF_NoFAFOCH()

		ppGuidPoly = Nothing
		pCutter = Nothing
	End Sub

	Private Function GetDetOCHObstacles() As Integer
		Dim PrevOCH As Double
		Dim I As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer

		PrevOCH = fFinalOCH

		M = UBound(MAPtObstacles.Obstacles)
		If M < 0 Then
			ReDim DetOCHObstacles.Obstacles(-1)
		Else
			ReDim DetOCHObstacles.Obstacles(M)
		End If

		For I = 0 To M
			MAPtObstacles.Obstacles(I).NIx = -1
		Next

		N = UBound(MAPtObstacles.Parts)
		If N < 0 Then
			ReDim DetOCHObstacles.Parts(-1)
		Else
			ReDim DetOCHObstacles.Parts(N)
		End If

		L = -1
		K = -1
		For I = 0 To N
			If MAPtObstacles.Parts(I).EffectiveHeight <= PrevOCH Then Continue For
			PrevOCH = MAPtObstacles.Parts(I).EffectiveHeight

			L = L + 1
			DetOCHObstacles.Parts(L) = MAPtObstacles.Parts(I)
			DetOCHObstacles.Parts(L).ReqOCH = MAPtObstacles.Parts(I).EffectiveHeight

			If MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx < 0 Then
				K += 1
				DetOCHObstacles.Obstacles(K) = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner)
				DetOCHObstacles.Obstacles(K).PartsNum = 0
				ReDim DetOCHObstacles.Obstacles(K).Parts(MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).PartsNum - 1)
				MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx = K
			End If

			DetOCHObstacles.Parts(L).Owner = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx
			DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(L).Owner).Parts(DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(L).Owner).PartsNum) = L
			DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(L).Owner).PartsNum += 1
		Next I

		If K < 0 Then
			ReDim DetOCHObstacles.Obstacles(-1)
		Else
			ReDim Preserve DetOCHObstacles.Obstacles(K)
		End If

		If L < 0 Then
			ReDim DetOCHObstacles.Parts(-1)
		Else
			ReDim Preserve DetOCHObstacles.Parts(L)
		End If

		Return L
	End Function

	Private Sub CalcTurnRange()
		Dim HAbsTurn As Double
		Dim RTurn As Double
		Dim lTAS As Double
		Dim E As Double
		Dim L As Double

		Dim TurnRange As Interval
		Dim I As Integer
		Dim N As Integer

		HAbsTurn = fRefHeight + arMATurnAlt.Value

		lTAS = IAS2TAS(_missedIAS, HAbsTurn, CurrADHP.ISAtC)
		RTurn = Bank2Radius(fBankAngle, lTAS)
		E = PI * CurrADHP.WindSpeed * lTAS / (254.168 * System.Math.Tan(DegToRad(fBankAngle))) / 90.0
		N = UBound(DetOCHObstacles.Parts)

		For I = 0 To N
			If TurnDir > 0 Then
				TurnRange = CalcSpiralStartPoint(RightLine, DetOCHObstacles.Parts(I), E, RTurn, _ArDir, TurnDir)
			Else
				TurnRange = CalcSpiralStartPoint(LeftLine, DetOCHObstacles.Parts(I), E, RTurn, _ArDir, TurnDir)
			End If

			DetOCHObstacles.Parts(I).TurnDistL = TurnRange.Left
			DetOCHObstacles.Parts(I).TurnAngleL = TurnRange.Right
		Next I

		Sort(DetOCHObstacles, 1)

		NonPrecReportFrm.FillPage06(DetOCHObstacles)
		L = dMAPt2SOC + arT_TechToleranc.Value * (lTAS + CurrADHP.WindSpeed) * 0.277777777777778

		OverOCH = fFinalOCH

		IxMinOCH = -1
		IxMaxOCH = -1

		For I = 0 To N
			If (L > DetOCHObstacles.Parts(I).TurnDistL) And (OverOCH <= DetOCHObstacles.Parts(I).EffectiveHeight) Then
				OverOCH = DetOCHObstacles.Parts(I).EffectiveHeight
				IxMinOCH = I
			End If

			If (L < DetOCHObstacles.Parts(I).TurnDistL) And (AvdOCH <= DetOCHObstacles.Parts(I).EffectiveHeight) Then
				AvdOCH = DetOCHObstacles.Parts(I).EffectiveHeight
				IxMaxOCH = I
				Exit For
			End If
		Next I

		TextBox1001.Text = CStr(ConvertHeight(fFinalOCH, eRoundMode.CEIL))
		TextBox1002.Text = CStr(ConvertHeight(OverOCH, eRoundMode.CEIL))
		TextBox1002.Tag = OverOCH

		If ComboBox1002.SelectedIndex < 0 Then
			ComboBox1002.SelectedIndex = 0
		Else
			ComboBox1002_SelectedIndexChanged(ComboBox1002, New System.EventArgs())
		End If

		Select Case IxFinalOCH
			Case -4
				_Label1001_6.Text = My.Resources.str11061
			Case -3
				_Label1001_6.Text = My.Resources.str11062
			Case -2
				_Label1001_6.Text = My.Resources.str11063
			Case -1
				_Label1001_6.Text = My.Resources.str11064
			Case Else
				If IxFinalOCH >= 0 Then
					'        If _OptionButton0201_0.Value Or (CheckBox0701.Value = 1) Then
					_Label1001_6.Text = My.Resources.str11065 + " " + FAFObstacleList.Obstacles(FAFObstacleList.Parts(IxFinalOCH).Owner).UnicalName
					'        Else
					'            _Label1001_6.Caption = "OCH определяется препятствием " + NavObstacleList(IxFinalOCH).ID
					'        End If
				End If
		End Select

		TextBox1004_Validating(TextBox1004, New System.ComponentModel.CancelEventArgs())

		If IxMinOCH > -1 Then
			TextBox1007.Text = DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(IxMinOCH).Owner).UnicalName
		Else
			TextBox1007.Text = "-"
		End If

		ComboBox1001.Enabled = IxMaxOCH > -1
		If IxMaxOCH > -1 Then
			If ComboBox1001.SelectedIndex < 0 Then
				ComboBox1001.SelectedIndex = 0
			Else
				ComboBox1001_SelectedIndexChanged(ComboBox1001, New System.EventArgs())
			End If

			TextBox1008.Text = DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(IxMaxOCH).Owner).UnicalName
		Else
			ComboBox1001.SelectedIndex = 0
			TextBox1003.Text = "-"
			TextBox1008.Text = "-"
		End If
	End Sub

	Private Sub CalcAvoidOCH(ByRef OvrOCH_ As Double)
		Dim I As Integer
		Dim N As Integer

		Dim L As Double

		L = dMAPt2SOC
		N = UBound(DetOCHObstacles.Parts)
		IxMaxOCH = -1

		For I = 0 To N
			If (L < DetOCHObstacles.Parts(I).TurnDistL) And (OvrOCH_ < DetOCHObstacles.Parts(I).ReqOCH) Then
				AvdOCH = DetOCHObstacles.Parts(I).ReqOCH
				IxMaxOCH = I
				Exit For
			End If
		Next I

		ComboBox1001.Enabled = IxMaxOCH > -1
		If IxMaxOCH > -1 Then
			If ComboBox1001.SelectedIndex < 0 Then
				ComboBox1001.SelectedIndex = 0
			Else
				ComboBox1001_SelectedIndexChanged(ComboBox1001, New System.EventArgs())
			End If

			TextBox1008.Text = DetOCHObstacles.Obstacles(DetOCHObstacles.Parts(IxMaxOCH).Owner).UnicalName
		Else
			ComboBox1001.SelectedIndex = 0
			TextBox1003.Text = "-"
			TextBox1008.Text = "-"
		End If

	End Sub

	Private Function ClosestToMOC50(ByRef OCH As Double, ByRef PDG As Double) As Double
		Dim PrevLminPos As Double
		Dim fFinalMOC As Double
		Dim LminPos As Double
		Dim LmaxPos As Double
		Dim LfiMax As Double
		Dim dLmin As Double
		Dim L6Sec As Double
		Dim lTAS As Double
		Dim Lfi As Double
		Dim dl As Double

		Dim ContinueDo As Boolean

		Dim Ix As Integer
		Dim Jx As Integer
		Dim I0 As Integer

		Dim I As Integer
		Dim N As Integer

		LminPos = dMAPt2SOC
		PrevLminPos = LminPos

		lTAS = IAS2TAS(_missedIAS, fRefHeight + arMATurnAlt.Value, CurrADHP.ISAtC)
		L6Sec = arT_TechToleranc.Value * (lTAS + CurrADHP.WindSpeed) * 0.277777777777778

		If IxMaxOCH > -1 Then
			LmaxPos = DetOCHObstacles.Parts(IxMaxOCH).TurnDistL - L6Sec
		Else
			LmaxPos = RModel 'fStrMisAprRng
		End If

		If OptionButton0901.Checked Then
			N = UBound(MAPtObstacles.Parts)
			LfiMax = 0.0
			Ix = -1
			Jx = -1
			I0 = 0
			dLmin = 5.0 * RModel

			If CheckBox0902.Checked Then
				fFinalMOC = arMA_FinalMOC.Value
			Else
				fFinalMOC = arMA_InterMOC.Value
			End If

			Do
				For I = I0 To N
					If (MAPtObstacles.Parts(I).Dist > LminPos) Then
						I0 = I
						Exit For
					End If

					'Lfi = (MAPtObstacles.Parts(I).Rmax - OCH) / PDG + dMAPt2SOC
					Lfi = (MAPtObstacles.Parts(I).Height + fFinalMOC - OCH) / PDG + dMAPt2SOC
					If (Lfi > MAPtObstacles.Parts(I).Dist) And (LfiMax < Lfi) Then
						LfiMax = Lfi
						Ix = I
					End If
				Next I

				If LfiMax > LmaxPos Then
					dl = LfiMax - LmaxPos
					If dl < dLmin Then
						dLmin = dl
						Jx = Ix
					End If
					LfiMax = LmaxPos
				End If

				ContinueDo = LminPos < LfiMax

				If ContinueDo Then
					If PrevLminPos <> LminPos Then
						dl = LminPos - MAPtObstacles.Parts(Ix).Dist
						If (dl > 0.0) And (dl < dLmin) Then
							dLmin = dl
							Jx = Ix
						End If
					End If

					PrevLminPos = LminPos
					LminPos = LfiMax
				End If

			Loop While ContinueDo

			If Jx > -1 Then
				'OCH = System.Math.Round(OCH + dLmin * PDG + 0.4999)
				'        LminPos = (MAPtObstacles(Jx).Rmax - OCH) / PDG + dMAPt2SOC
				LminPos = (MAPtObstacles.Parts(Jx).Height + fFinalMOC - OCH) / PDG + dMAPt2SOC
			End If
		End If

		hMinTurn = (LminPos - dMAPt2SOC) * PDG + OCH
		hMaxTurn = (LmaxPos - dMAPt2SOC) * PDG + OCH

		TextBox1005.Text = CStr(ConvertHeight((LminPos - dMAPt2SOC) * PDG + OCH, eRoundMode.NEAREST))
		TextBox1006.Text = CStr(ConvertHeight((LmaxPos - dMAPt2SOC) * PDG + OCH, eRoundMode.NEAREST))

		If hMinTurn < hMaxTurn Then
			TextBox1009.Text = CStr(ConvertDistance(LminPos, 3))
			TextBox1010.Text = CStr(ConvertDistance(LmaxPos, 1))
		Else
			TextBox1009.Text = CStr(ConvertDistance(LmaxPos, 1))
			TextBox1010.Text = CStr(ConvertDistance(LmaxPos, 1))
		End If

		'PtSOC.Z = OCH                                          '??????????????????
		'If IxMaxOCH > -1 Then
		'    TextBox1006.Text = CStr(Round((DetOCHObstacles(IxMaxOCH).TurnDistL - dMAPt2SOC) * PDG + OCH))
		'Else
		'    TextBox1006.Text = "-"
		'End If

		ClosestToMOC50 = LminPos
	End Function

	Private Function GetClosestTNH(ByRef TNH As Double, ByRef OCH As Double, ByRef PDG As Double) As Double
		Dim ReqDist As Double
		Dim Range As Double
		Dim Lhmin As Double
		Dim Ltna As Double
		Dim dl As Double
		Dim TmpInList As ObstacleContainer

		Dim Ist As Integer

		Dim I As Integer
		Dim N As Integer

		If TNH < hMinTurn Then TNH = hMinTurn
		If TNH > hMaxTurn Then TNH = hMaxTurn

		Range = (hMaxTurn - OCH) / PDG + dMAPt2SOC
		Ltna = (TNH - OCH) / PDG + dMAPt2SOC
		Lhmin = (hMinTurn - OCH) / PDG + dMAPt2SOC

		If Range = RModel Then
			TmpInList = MAPtObstacles
		Else
			GetObstInRange(MAPtObstacles, TmpInList, Range)
		End If

		N = UBound(TmpInList.Parts)
		For I = 0 To N
			If TmpInList.Parts(I).Dist >= Lhmin Then
				Ist = I
				Exit For
			End If
		Next I

		dl = Ltna

		For I = Ist To N
			If TmpInList.Parts(I).Dist > dl Then Exit For
			ReqDist = (TmpInList.Parts(I).Rmax - OCH) / PDG + dMAPt2SOC

			If (TmpInList.Parts(I).Dist < ReqDist) And (dl < ReqDist) Then
				dl = ReqDist
			End If
		Next I

		If dl > Range Then
			dl = Ltna
			For I = N To Ist Step -1
				If TmpInList.Parts(I).Dist <= Ltna Then
					ReqDist = (TmpInList.Parts(I).Rmax - OCH) / PDG + dMAPt2SOC

					If (TmpInList.Parts(I).Dist < ReqDist) And (dl < ReqDist) Then
						dl = TmpInList.Parts(I).Dist
					End If
				End If
			Next I

		End If

		GetClosestTNH = (dl - dMAPt2SOC) * PDG + OCH
		If GetClosestTNH < hMinTurn Then GetClosestTNH = hMinTurn
		If GetClosestTNH > hMaxTurn Then GetClosestTNH = hMaxTurn
	End Function

	Private Function CalcImIntervals(ByRef CurOCH As Double, ByRef MinImRange As Double, ByRef Ix As Integer) As LowHigh()
		Dim Wif As Double
		Dim fTmp As Double
		Dim hFAF As Double
		Dim Wfaf As Double
		Dim Rmax0 As Double
		Dim Rmax1 As Double

		Dim pt28 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt9_3 As ESRI.ArcGIS.Geometry.IPoint
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pSecL As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSecR As ESRI.ArcGIS.Geometry.IPointCollection

		Dim ObsRange As LowHigh
		Dim ImRange() As LowHigh
		Dim ResRange() As LowHigh

		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim O As Integer
		Dim P As Integer


		pFAFLine = New ESRI.ArcGIS.Geometry.Polyline
		PtFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, MinImRange)
		PtFAF.M = _ArDir

		pFAFLine.FromPoint = PointAlongPlane(PtFAF, _ArDir + 90.0, 50000.0)
		pFAFLine.ToPoint = PointAlongPlane(PtFAF, _ArDir - 90.0, 50000.0)
		'===================================================================

		pTopo = pFullPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pFAFLine = pTopo.Intersect(pFAFLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If pFAFLine.IsEmpty() Then
			MessageBox.Show(My.Resources.str00301)
			ReDim ImRange(-1)
			Return ImRange
		End If

		If SideDef(pFAFLine.FromPoint, _ArDir, pFAFLine.ToPoint) < 0 Then pFAFLine.ReverseOrientation()

		'===================================================================
		pt9_3 = PointAlongPlane(PtFAF, _ArDir + 180.0, arMinISlen)
		pt28 = PointAlongPlane(PtFAF, _ArDir + 180.0, arImRange_Max.Value)

		IntermediateBaseArea = New ESRI.ArcGIS.Geometry.Polygon

		IntermediateBaseArea.AddPoint(pFAFLine.FromPoint)
		IntermediateBaseArea.AddPoint(pFAFLine.ToPoint)
		IntermediateBaseArea.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, arIFHalfWidth.Value))
		IntermediateBaseArea.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, arIFHalfWidth.Value))
		IntermediateBaseArea.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, arIFHalfWidth.Value))
		IntermediateBaseArea.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, arIFHalfWidth.Value))
		IntermediateBaseArea.AddPoint(pFAFLine.FromPoint)

		pSecL = New ESRI.ArcGIS.Geometry.Polygon
		pSecR = New ESRI.ArcGIS.Geometry.Polygon

		If FinalNav.TypeCode = eNavaidType.LLZ Then
			'    pSecL.AddPoint PointAlongPlane(PtFAF, ArDir - 90.0, pFAFLine.Length)
			pSecL.AddPoint(IntermediateBaseArea.Point(1))
			pSecL.AddPoint(IntermediateBaseArea.Point(2))
			pSecL.AddPoint(IntermediateBaseArea.Point(3))
			pSecL.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(pSecL.Point(0))

			pSecR.AddPoint(IntermediateBaseArea.Point(0))
			'    pSecR.AddPoint PointAlongPlane(PtFAF, ArDir + 90.0, 0.25 * pFAFLine.Length)
			pSecR.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(IntermediateBaseArea.Point(4))
			pSecR.AddPoint(IntermediateBaseArea.Point(5))
			pSecR.AddPoint(pSecR.Point(0))
		Else
			pSecL.AddPoint(PointAlongPlane(PtFAF, _ArDir - 90.0, 0.25 * pFAFLine.Length))
			pSecL.AddPoint(IntermediateBaseArea.Point(1))
			pSecL.AddPoint(IntermediateBaseArea.Point(2))
			pSecL.AddPoint(IntermediateBaseArea.Point(3))
			pSecL.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(pSecL.Point(0))

			pSecR.AddPoint(IntermediateBaseArea.Point(0))
			pSecR.AddPoint(PointAlongPlane(PtFAF, _ArDir + 90.0, 0.25 * pFAFLine.Length))
			pSecR.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(IntermediateBaseArea.Point(4))
			pSecR.AddPoint(IntermediateBaseArea.Point(5))
			pSecR.AddPoint(pSecR.Point(0))
		End If

		pTopo = pSecR
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pSecL
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		IntermediateSecondArea = pTopo.Union(pSecR)

		On Error Resume Next
		If Not IntermediateBaseAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateBaseAreaElem)
		If Not IntermediateSecAreaElem Is Nothing Then pGraphics.DeleteElement(IntermediateSecAreaElem)
		IntermediateBaseAreaElem = DrawPolygon(IntermediateBaseArea, 255)
		IntermediateSecAreaElem = DrawPolygon(IntermediateSecondArea, 0)
		IntermediateBaseAreaElem.Locked = True
		IntermediateSecAreaElem.Locked = True
		On Error GoTo 0

		hFAF = CurOCH
		Wfaf = 0.5 * pFAFLine.Length
		Wif = arIFHalfWidth.Value

		pTopo = IntermediateBaseArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		GetIntermedObstacles(ObstacleList, ImObstacles, IntermediateBaseArea, IntermediateSecondArea, Wfaf, arMinISlen, _ArDir, hFAF, FinalNav.TypeCode, PtFAF)

		Sort(ImObstacles, 0)

		N = UBound(ImObstacles.Parts)

		I = 0
		While I <= N
			fTmp = ImObstacles.Parts(I).DistStar - Wfaf
			If fTmp <= 0.0 Then
				ImObstacles.Parts(I).Rmax = arImRange_Max.Value
			Else
				Rmax0 = ImObstacles.Parts(I).Dist * (Wif - Wfaf) / fTmp
				ImObstacles.Parts(I).Rmax = Rmax0
				fTmp = 0.5 * (hFAF - ImObstacles.Parts(I).Height) / arISegmentMOC.Value

				If fTmp > 0.0 Then
					Rmax1 = ImObstacles.Parts(I).Dist * (Wif - Wfaf) / (ImObstacles.Parts(I).DistStar / (1.0 - fTmp) - Wfaf)
					If Rmax1 < Rmax0 Then ImObstacles.Parts(I).Rmax = Rmax1
				End If

				If ImObstacles.Parts(I).Rmax > arImRange_Max.Value Then ImObstacles.Parts(I).Rmax = arImRange_Max.Value
			End If

			If ImObstacles.Parts(I).Dist < arMinISlen Then
				ImObstacles.Parts(I).Rmin = arMinISlen
			Else
				ImObstacles.Parts(I).Rmin = ImObstacles.Parts(I).Dist
			End If

			If ImObstacles.Parts(I).Rmax <= ImObstacles.Parts(I).Rmin Then
				For J = I To N - 1
					ImObstacles.Parts(J) = ImObstacles.Parts(J + 1)
				Next J
				N -= 1
			Else
				I += 1
			End If
		End While

		ReDim ImRange(0)

		ImRange(0).Low = arMinISlen
		ImRange(0).High = arImRange_Max.Value
		ImRange(0).Tag = -1
		Ix = -1

		If N < 0 Then
			Return ImRange '    ReDim ImObstacles(-1)    ??????????????????????
		Else
			ReDim Preserve ImObstacles.Parts(N)
		End If

		M = UBound(ImRange)
		For I = 0 To N
			ObsRange.Low = ImObstacles.Parts(I).Rmin
			ObsRange.High = ImObstacles.Parts(I).Rmax
			J = 0
			While J <= M
				ResRange = LowHighDifference(ImRange(J), ObsRange)
				L = UBound(ResRange)
				If L >= 0 Then
					If (ImRange(J).Low <> ResRange(0).Low) Or (ImRange(J).High <> ResRange(0).High) Then
						ImRange(J) = ResRange(0)
						ImRange(J).Tag = I
					End If

					If L > 0 Then
						M = M + 1
						ReDim Preserve ImRange(M)
						For K = M To J + 2 Step -1
							ImRange(K) = ImRange(K - 1)
						Next K
						ImRange(J + 1) = ResRange(1)
						ImRange(J + 1).Tag = I
						J = J + 2
					Else
						J = J + 1
					End If
				ElseIf L < 0 Then
					For K = J To M - 1
						ImRange(K) = ImRange(K + 1)
					Next K

					M = M - 1
					If M < 0 Then
						'ReDim ImRange(-1)
						Ix = I
						Exit For
					Else
						'ReDim Preserve ImRange(M)
					End If
				End If
			End While
		Next I

		If M < 0 Then
			ReDim ImRange(-1)
		Else
			ReDim Preserve ImRange(M)
		End If

		Return ImRange
	End Function

	Private Function CalcImIntervalsLocal(ByRef CurOCH As Double, ByRef MinImRange As Double, ByRef Ix As Integer, ByRef LocalObstacles As ObstacleContainer) As LowHigh()
		Dim fTmp As Double
		Dim Rmax0 As Double
		Dim Rmax1 As Double

		Dim hFAF As Double
		Dim Wfaf As Double
		Dim Wif As Double

		Dim ptlFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim pt9_3 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt28 As ESRI.ArcGIS.Geometry.IPoint

		Dim pLocalFAFLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLocalSecondArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pLocalBaseArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSecL As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSecR As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim ResRange() As LowHigh
		Dim ImRange() As LowHigh
		Dim ObsRange As LowHigh

		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer
		Dim L As Integer
		'Dim O As Integer
		'Dim P As Integer

		Ix = -1
		pLocalFAFLine = New ESRI.ArcGIS.Geometry.Polyline
		ptlFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, MinImRange)
		pLocalFAFLine.FromPoint = PointAlongPlane(ptlFAF, _ArDir + 90.0, 50000.0)
		pLocalFAFLine.ToPoint = PointAlongPlane(ptlFAF, _ArDir - 90.0, 50000.0)
		'===================================================================
		pTopo = pFullPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pLocalFAFLine = pTopo.Intersect(pLocalFAFLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If pLocalFAFLine.IsEmpty() Then
			MessageBox.Show(My.Resources.str00303)   '"Средство находится слишкои далеко"
			ReDim ImRange(-1)
			Return ImRange
		End If

		If SideDef(pLocalFAFLine.FromPoint, _ArDir, pLocalFAFLine.ToPoint) < 0 Then
			pLocalFAFLine.ReverseOrientation()
		End If
		'===================================================================
		pt9_3 = PointAlongPlane(ptlFAF, _ArDir + 180.0, arMinISlen)
		pt28 = PointAlongPlane(ptlFAF, _ArDir + 180.0, arImRange_Max.Value)

		pLocalBaseArea = New ESRI.ArcGIS.Geometry.Polygon

		pLocalBaseArea.AddPoint(pLocalFAFLine.FromPoint)
		pLocalBaseArea.AddPoint(pLocalFAFLine.ToPoint)
		pLocalBaseArea.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, arIFHalfWidth.Value))
		pLocalBaseArea.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, arIFHalfWidth.Value))
		pLocalBaseArea.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, arIFHalfWidth.Value))
		pLocalBaseArea.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, arIFHalfWidth.Value))
		pLocalBaseArea.AddPoint(pLocalBaseArea.Point(0))

		pSecL = New ESRI.ArcGIS.Geometry.Polygon
		pSecR = New ESRI.ArcGIS.Geometry.Polygon

		If FinalNav.TypeCode = eNavaidType.LLZ Then
			'pSecL.AddPoint(PointAlongPlane(ptlFAF, m_ArDir - 90.0, 0.25 * pLocalFAFLine.Length))
			pSecL.AddPoint(pLocalBaseArea.Point(1))
			pSecL.AddPoint(pLocalBaseArea.Point(2))
			pSecL.AddPoint(pLocalBaseArea.Point(3))
			pSecL.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(pSecL.Point(0))

			pSecR.AddPoint(pLocalBaseArea.Point(0))
			pSecR.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(pLocalBaseArea.Point(4))
			pSecR.AddPoint(pLocalBaseArea.Point(5))
			pSecR.AddPoint(pSecR.Point(0))
		Else
			pSecL.AddPoint(PointAlongPlane(ptlFAF, _ArDir - 90.0, 0.25 * pLocalFAFLine.Length))
			pSecL.AddPoint(pLocalBaseArea.Point(1))
			pSecL.AddPoint(pLocalBaseArea.Point(2))
			pSecL.AddPoint(pLocalBaseArea.Point(3))
			pSecL.AddPoint(PointAlongPlane(pt28, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(PointAlongPlane(pt9_3, _ArDir - 90.0, 0.5 * arIFHalfWidth.Value))
			pSecL.AddPoint(pSecL.Point(0))

			pSecR.AddPoint(pLocalBaseArea.Point(0))
			pSecR.AddPoint(PointAlongPlane(ptlFAF, _ArDir + 90.0, 0.25 * pLocalFAFLine.Length))
			pSecR.AddPoint(PointAlongPlane(pt9_3, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(PointAlongPlane(pt28, _ArDir + 90.0, 0.5 * arIFHalfWidth.Value))
			pSecR.AddPoint(pLocalBaseArea.Point(4))
			pSecR.AddPoint(pLocalBaseArea.Point(5))
			pSecR.AddPoint(pSecR.Point(0))
		End If

		pTopo = pSecR
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTopo = pSecL
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pLocalSecondArea = pTopo.Union(pSecR)

		hFAF = CurOCH
		Wfaf = 0.5 * pLocalFAFLine.Length
		Wif = arIFHalfWidth.Value

		pTopo = pLocalBaseArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		GetIntermedObstacles(ObstacleList, LocalObstacles, pLocalBaseArea, pLocalSecondArea, Wfaf, arMinISlen, _ArDir, hFAF, FinalNav.TypeCode, ptlFAF)

		Sort(LocalObstacles, 0)

		N = UBound(LocalObstacles.Parts)
		'arISegmentMOC_ = 1.0 / arISegmentMOC.Value

		I = 0
		While I <= N
			fTmp = LocalObstacles.Parts(I).DistStar - Wfaf
			If fTmp <= 0.0 Then
				LocalObstacles.Parts(I).Rmax = arImRange_Max.Value
			Else
				LocalObstacles.Parts(I).Rmax = LocalObstacles.Parts(I).Dist * (Wif - Wfaf) / fTmp
				Rmax0 = LocalObstacles.Parts(I).Dist * (Wif - Wfaf) / fTmp
				LocalObstacles.Parts(I).Rmax = Rmax0
				fTmp = 0.5 * (hFAF - LocalObstacles.Parts(I).Height) / arISegmentMOC.Value

				If fTmp > 0.0 Then
					Rmax1 = LocalObstacles.Parts(I).Dist * (Wif - Wfaf) / (LocalObstacles.Parts(I).DistStar / (1.0 - fTmp) - Wfaf)
					If Rmax1 < Rmax0 Then LocalObstacles.Parts(I).Rmax = Rmax1
				End If

				'        If LocalObstacles(I).Rmax < arMinISlen Then LocalObstacles(I).Rmax = arMinISlen

				If LocalObstacles.Parts(I).Rmax > arImRange_Max.Value Then
					LocalObstacles.Parts(I).Rmax = arImRange_Max.Value
					'        ElseIf LocalObstacles(I).Rmax < arIFHalfWidth.Value Then
					'            LocalObstacles(I).Rmax = arIFHalfWidth.Value
				End If
			End If

			If LocalObstacles.Parts(I).Dist < arMinISlen Then
				LocalObstacles.Parts(I).Rmin = arMinISlen
			Else
				LocalObstacles.Parts(I).Rmin = LocalObstacles.Parts(I).Dist
			End If

			If LocalObstacles.Parts(I).Rmax < LocalObstacles.Parts(I).Rmin Then
				For M = I To N - 1
					LocalObstacles.Parts(M) = LocalObstacles.Parts(M + 1)
				Next M
				N = N - 1
			Else
				I = I + 1
			End If
		End While

		ReDim ImRange(0)

		ImRange(0).Low = arMinISlen
		ImRange(0).High = arImRange_Max.Value
		ImRange(0).Tag = -1

		If N < 0 Then
			Return ImRange '    ReDim LocalObstacles(-1)         ?????????????????????????
		Else
			ReDim Preserve LocalObstacles.Parts(N)          'recalc LocalObstacles.Obstacles
		End If

		M = 0
		For I = 0 To N
			ObsRange.Low = LocalObstacles.Parts(I).Rmin
			ObsRange.High = LocalObstacles.Parts(I).Rmax
			J = 0
			While J <= M
				ResRange = LowHighDifference(ImRange(J), ObsRange)
				L = UBound(ResRange)
				If L >= 0 Then
					If (ImRange(J).Low <> ResRange(0).Low) Or (ImRange(J).High <> ResRange(0).High) Then
						ImRange(J) = ResRange(0)
						ImRange(J).Tag = I
					End If

					If L > 0 Then
						M = M + 1
						ReDim Preserve ImRange(M)
						For K = M To J + 2 Step -1
							ImRange(K) = ImRange(K - 1)
						Next K
						ImRange(J + 1) = ResRange(1)
						ImRange(J + 1).Tag = I
						J = J + 2
					Else
						J = J + 1
					End If
				ElseIf L < 0 Then
					For K = J To M - 1
						ImRange(K) = ImRange(K + 1)
					Next K

					M = M - 1
					If M < 0 Then
						'ReDim ImRange(-1)
						Ix = I
						Exit For
					Else
						'ReDim Preserve ImRange(O)
					End If
				End If
			End While
		Next I

		If M < 0 Then
			ReDim ImRange(-1)
		Else
			ReDim Preserve ImRange(M)
		End If

		Return ImRange
	End Function

	Private Function CalcFAFDispRange(ByRef OCH As Double, ByRef CurrPDG As Double) As LowHigh
		Dim fTmp As Double
		Dim fDist As Double
		Dim fRDH As Double

		Dim I As Integer
		Dim Ix As Integer
		Dim N As Integer
		Dim NavSide As Integer

		Dim bFlag As Boolean

		Dim Result As LowHigh
		Dim FASRange As LowHigh
		Dim ObsRange As LowHigh
		Dim NavRange As LowHigh

		fRDH = GetRDH()

		fDist = ReturnDistanceInMeters(FinalNav.pPtPrj, FictTHR)
		NavSide = SideDef(FictTHR, _ArDir - 90.0, FinalNav.pPtPrj)

		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			NavRange.High = NavSide * fDist + VOR.FA_Range
			NavRange.Low = NavSide * fDist - VOR.FA_Range
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			NavRange.High = NavSide * fDist + NDB.FA_Range
			NavRange.Low = NavSide * fDist - NDB.FA_Range
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			NavRange.High = NavSide * fDist + LLZ.Range
			NavRange.Low = NavSide * fDist - LLZ.Range
		End If

		If _OnAero And (NavSide < 0) Then
			FASRange.Low = NavSide * fDist
		Else
			FASRange.Low = 0.0
		End If

		FASRange.High = arFAFLenght.Value

		If LowHighIntersection(FASRange, NavRange, ObsRange) Then
			FASRange = ObsRange
		Else
			MessageBox.Show(My.Resources.str00303)  '"Данное средство не удовлетворяет требованиям наведения"
			Exit Function
		End If

		If FASRange.Low > 0.0 Then
			fTmp = CurrPDG * FASRange.Low + fRDH
			If fTmp > OCH Then OCH = fTmp
		End If

		If OCH > arISegmentMOC.Value Then
			Result.Low = (OCH - fRDH) / CurrPDG
		Else
			Result.Low = (arISegmentMOC.Value - fRDH) / CurrPDG
		End If

		Dim D As Double
		Dim Y As Double

		Result.High = arFAFLenght.Value
		lFAFmin = Result.Low

		N = UBound(FAFObstacleList.Parts)
		Ix = -1
		bFlag = True

		For I = 0 To N
			If (FAFObstacleList.Parts(I).ReqH > OCH) Then
				If bFlag Then
					If FAFObstacleList.Parts(I).Dist < lFAFmin Then
						Result.Low = FAFObstacleList.Parts(I).Dist
						OCH = FAFObstacleList.Parts(I).ReqH
						Ix = I
					End If
					bFlag = False
				End If
			End If

			If Not bFlag And (FAFObstacleList.Parts(I).ReqH > OCH) Then
				'        lFAFRange.High = FAFObstacleList(i).Dist
				D = (FAFObstacleList.Parts(I).Height - fRDH) / arFixMaxIgnorGrd.Value
				Y = (FAFObstacleList.Parts(I).Dist * CurrPDG - D * arFixMaxIgnorGrd.Value) / (arFixMaxIgnorGrd.Value - CurrPDG)
				If Y > 0.0 Then
					Result.High = FAFObstacleList.Parts(I).Dist + Y
				Else
					Result.High = FAFObstacleList.Parts(I).Dist
				End If
				Exit For
			End If
		Next I

		Result.Tag = Ix
		lFAFmin = Result.Low
		Return Result
	End Function

	Private Function CalcNewRange(ByRef Ix As Integer, ByRef ObstList As ObstacleContainer, ByRef OldFAFRange As Double) As Double
		Dim Wfaf As Double
		Dim fTmp As Double
		Dim fRange As Double
		Dim MinNewRange As Double

		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim X0 As Double
		Dim X1 As Double

		Dim I As Integer

		Dim Sol As Integer
		Dim fRDH As Double

		fRDH = GetRDH()

		Wfaf = 0.5 * pFAFLine.Length
		MinNewRange = 40000000.0

		For I = 0 To Ix
			'    fTmp = 2.0 * (arISegmentMOC.Value * ObstacleAr.Parts(I).Rmin)
			A = _CurrPDG * (arIFHalfWidth.Value - Wfaf)
			B = -(_CurrPDG * Wfaf * ObstList.Parts(I).Rmin + (_CurrPDG * (ObstList.Parts(I).Dist + OldFAFRange) + 2.0 * arISegmentMOC.Value + ObstList.Parts(I).Height - fRDH) * (arIFHalfWidth.Value - Wfaf))
			C = (2.0 * arISegmentMOC.Value + ObstList.Parts(I).Height - fRDH) * (Wfaf * ObstList.Parts(I).Rmin + (ObstList.Parts(I).Dist + OldFAFRange) * (arIFHalfWidth.Value - Wfaf)) - 2.0 * (arISegmentMOC.Value * ObstList.Parts(I).Rmin) * ObstList.Parts(I).DistStar

			Sol = Quadric(A, B, C, X0, X1)

			fRange = (arISegmentMOC.Value + ObstList.Parts(I).Height - fRDH) / _CurrPDG

			If (Sol > 1) And (OldFAFRange < fRange) And (fRange < ObstList.Parts(I).Dist + OldFAFRange) And (fRange < X0) Then
				fTmp = fRange + 0.5 'Rnd
			ElseIf (Sol > 1) And (OldFAFRange < X0) And (X0 < ObstList.Parts(I).Dist + OldFAFRange) Then
				fTmp = X0 + 0.5 'Rnd
			Else
				fTmp = ObstList.Parts(I).Dist + OldFAFRange + 0.5   '2.0 * Rnd
			End If

			If MinNewRange > fTmp Then MinNewRange = fTmp
		Next I

		Return MinNewRange
	End Function

	Private Sub Caller(ByRef OCH As Double, ByVal fFAFRange As Double) ', Optional ByRef HeightPrior As Boolean = True
		Dim I As Integer
		Dim K As Integer
		Dim N As Integer

		Dim Ix As Integer
		Dim iCR As Integer

		Dim fDist As Double
		Dim fRDH As Double
		Dim AddMOC As Double
		Dim fOldFAFRange As Double

		Dim tmpStr As String
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim hRange As LowHigh
		Dim ImIntervals() As LowHigh

		'Dim J As Integer
		'Dim L As Integer
		'Dim M As Integer
		'Dim Jx As Integer

		ShowPandaBox(Me.Handle.ToInt32)

		fRDH = GetRDH()

		N = UBound(FAFObstacleList.Parts)

		OCH = fMinOCH

		fOldFAFRange = fFAFRange

		'If fFAFRange > arMOCChangeDist.Value Then
		'	AddMOC = 0.0 '(fFAFRange - arMOCChangeDist.Value) * arAddMOCCoef.Value
		'Else
		'	AddMOC = 0.0
		'End If

		Dim fMinDist As Double
		Dim fHorRange As Double

		If OptionButton0102.Checked Then
			fHorRange = DeConvertDistance(CDbl(TextBox0308.Text))
			Dim fMaxDist As Double
			fMaxDist = (fNewVisAprOCH - arAbv_Treshold.Value) / _CurrPDG

			fMinDist = arISegmentMOC.Value / _CurrPDG

			If fHorRange > fMaxDist Then
				fHorRange = fMaxDist
				TextBox0308.Text = ConvertDistance(fHorRange).ToString()
			End If
			If fHorRange > fFAFRange Then
				fHorRange = fHorRange
				TextBox0308.Text = ConvertDistance(fHorRange).ToString()
			End If
		Else
			fHorRange = 0.0
			fMinDist = 0.0
		End If

		K = -1
		For I = 0 To N
			If FAFObstacleList.Parts(I).Dist > fFAFRange Then Exit For
			FAFObstacleList.Parts(I).MOC = (arFASeg_FAF_MOC.Value + AddMOC) * FAFObstacleList.Parts(I).fTmp
			FAFObstacleList.Parts(I).ReqH = FAFObstacleList.Parts(I).Height + FAFObstacleList.Parts(I).MOC
			If OCH < FAFObstacleList.Parts(I).ReqH Then
				OCH = FAFObstacleList.Parts(I).ReqH
				K = I
			End If
		Next I

		Do
			FAFRange = CalcFAFDispRange(OCH, _CurrPDG)
			If fFAFRange < FAFRange.Low Then
				fFAFRange = FAFRange.Low
			End If

			If fFAFRange > FAFRange.High Then
				FAFRange.High = fFAFRange
			End If

			CurrhFAF = (fFAFRange - fHorRange) * _CurrPDG + fRDH
			If CurrhFAF < fMinOCH Then
				CurrhFAF = fMinOCH
				fFAFRange = (CurrhFAF - fRDH) / _CurrPDG + fHorRange
			End If

			If fOldFAFRange <> fFAFRange Then
				'If fFAFRange > arMOCChangeDist.Value Then
				'	AddMOC = 0.0 '(fFAFRange - arMOCChangeDist.Value) * arAddMOCCoef.Value
				'Else
				'	AddMOC = 0.0
				'End If

				K = -1
				For I = 0 To N
					If FAFObstacleList.Parts(I).Dist <= fFAFRange Then

						FAFObstacleList.Parts(I).MOC = (arFASeg_FAF_MOC.Value + AddMOC) * FAFObstacleList.Parts(I).fTmp
						FAFObstacleList.Parts(I).ReqH = FAFObstacleList.Parts(I).Height + FAFObstacleList.Parts(I).MOC
						If OCH < FAFObstacleList.Parts(I).ReqH Then
							OCH = FAFObstacleList.Parts(I).ReqH
							K = I
						End If
					End If
				Next I
				fOldFAFRange = fFAFRange
			End If

			PtFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, fFAFRange)
			PtFAF.Z = CurrhFAF ' + fRefHeight
			PtFAF.M = _ArDir

			iCR = FillFAFCombo(FAFRange.High)

			If FAFRange.Tag >= 0 Then K = -1

			hRange.Low = FAFRange.Low * _CurrPDG + fRDH
			hRange.High = FAFRange.High * _CurrPDG + fRDH
			Do
				ImIntervals = CalcImIntervals(CurrhFAF, fFAFRange, Ix)
				If (UBound(ImIntervals) < 0) And (Ix < 0) Then
					_FinalOCH = OCH
					HidePandaBox()
					Return
				End If

				If Ix >= 0 Then
					If hRange.High < ImObstacles.Parts(Ix).ReqH Then
						fFAFRange = CalcNewRange(Ix, ImObstacles, fFAFRange - fHorRange) + fHorRange
						If fOldFAFRange <> fFAFRange Then
							'If fFAFRange > arMOCChangeDist.Value Then
							'	AddMOC = 0.0 '(fFAFRange - arMOCChangeDist.Value) * arAddMOCCoef.Value
							'Else
							'	AddMOC = 0.0
							'End If

							N = UBound(FAFObstacleList.Parts)

							For I = 0 To N
								If FAFObstacleList.Parts(I).Dist > fFAFRange + distEps Then Exit For

								FAFObstacleList.Parts(I).MOC = (arFASeg_FAF_MOC.Value + AddMOC) * FAFObstacleList.Parts(I).fTmp
								FAFObstacleList.Parts(I).ReqH = FAFObstacleList.Parts(I).Height + FAFObstacleList.Parts(I).MOC

								If OCH < FAFObstacleList.Parts(I).ReqH Then
									OCH = FAFObstacleList.Parts(I).ReqH
									K = I
								End If
							Next I
							fOldFAFRange = fFAFRange
						End If
						Exit Do
					Else
						CurrhFAF = ImObstacles.Parts(Ix).ReqH
						fFAFRange = (CurrhFAF - fRDH) / _CurrPDG + fHorRange
						If fOldFAFRange <> fFAFRange Then
							'If fFAFRange > arMOCChangeDist.Value Then
							'	AddMOC = 0.0 '(fFAFRange - arMOCChangeDist.Value) * arAddMOCCoef.Value
							'Else
							'	AddMOC = 0.0
							'End If

							N = UBound(FAFObstacleList.Parts)

							For I = 0 To N
								If FAFObstacleList.Parts(I).Dist > fFAFRange + distEps Then Exit For

								FAFObstacleList.Parts(I).MOC = (arFASeg_FAF_MOC.Value + AddMOC) * FAFObstacleList.Parts(I).fTmp
								FAFObstacleList.Parts(I).ReqH = FAFObstacleList.Parts(I).Height + FAFObstacleList.Parts(I).MOC

								If OCH < FAFObstacleList.Parts(I).ReqH Then
									OCH = FAFObstacleList.Parts(I).ReqH
									K = I
								End If
							Next I
							fOldFAFRange = fFAFRange
						End If
					End If
				End If
			Loop While Ix >= 0
		Loop While Ix >= 0

		If K >= 0 Then
			TextBox0304.Text = FAFObstacleList.Obstacles(FAFObstacleList.Parts(K).Owner).UnicalName
			TextBox0305.Text = CStr(ConvertDistance(FAFObstacleList.Parts(K).Dist, eRoundMode.NEAREST))
			NextObstInfo(K)
		ElseIf FAFRange.Tag >= 0 Then
			TextBox0304.Text = FAFObstacleList.Obstacles(FAFObstacleList.Parts(FAFRange.Tag).Owner).UnicalName
			TextBox0305.Text = CStr(ConvertDistance(FAFObstacleList.Parts(FAFRange.Tag).Dist, eRoundMode.NEAREST))
			NextObstInfo(FAFRange.Tag)
			K = FAFRange.Tag
		Else
			TextBox0304.Text = "--"
			TextBox0305.Text = "--"
			NextObstInfo(-1)
		End If

		'================================================
		N = UBound(ImIntervals)

		SafeDeleteElement(IFPolyElement)

		ListView0301.Items.Clear()

		For I = 0 To N
			tmpStr = CStr(ConvertDistance(ImIntervals(I).Low, 3))
			itmX = ListView0301.Items.Add(tmpStr)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(ImIntervals(I).High, 1))))
			If ImIntervals(I).Tag >= 0 Then
				itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ImObstacles.Obstacles(ImIntervals(I).Tag).UnicalName))
			End If
		Next I

		'fNearDist = DeConvertDistance(CDbl(Item.Text))
		'fFarDist = DeConvertDistance(CDbl(Item.SubItems(1).Text))

		ListView0301.Items.Item(0).Checked = False
		ListView0301.Items.Item(0).Checked = True
		ListView0301.Items.Item(0).Selected = True
		ListView0301.Items.Item(0).Focused = True

		ListView0301_Click(ListView0301, New EventArgs())
		ListView0301_ItemChecked(ListView0301, New System.Windows.Forms.ItemCheckedEventArgs(ListView0301.Items.Item(0)))

		'TextBox0302.Text = CStr(ConvertHeight(OCH, eRoundMode.rmNERAEST))
		'dMinFAF = FAFRange.Low

		dMaxFAF = FAFRange.High
		'fFAFRange = hFAP2FAPDist(CurrhFAF)

		TextBox0301.Text = CStr(ConvertDistance(fFAFRange, eRoundMode.NEAREST))
		If (fFAFRange < arFAFOptimalDist.Value) Or (fFAFRange > arMOCChangeDist.Value) Then
			TextBox0301.ForeColor = Color.Red
		Else
			TextBox0301.ForeColor = Color.Black
		End If

		TextBox0303.Tag = CStr(CurrhFAF + fRefHeight)

		If ComboBox0303.SelectedIndex = 0 Then
			TextBox0303.Text = CStr(ConvertHeight(CurrhFAF + fRefHeight, eRoundMode.NEAREST))
		Else
			TextBox0303.Text = CStr(ConvertHeight(CurrhFAF, eRoundMode.NEAREST))
		End If

		PtFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, fFAFRange)
		PtFAF.Z = CurrhFAF
		PtFAF.M = _ArDir

		iCR = FillFAFCombo(dMaxFAF)

		If (iCR < 0) And (CurrPage >= 3) Then MessageBox.Show(My.Resources.str00301) '"Нет подходящего средства для создания FAF"

		ArrivalProfile.ClearPoints()

		ArrivalProfile.AddPoint(fFAFRange, CurrhFAF, Modulus(ArAzt - FinalNav.MagVar, 360.0), -RadToDeg(System.Math.Atan(_CurrPDG)), CodeProcedureDistance.FAF)

		fDist = fFAFRange - (CurrhFAF - OCH) / _CurrPDG

		If fHorRange > 0 Then
			ArrivalProfile.AddPoint(fDist + fHorRange, OCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), -RadToDeg(System.Math.Atan(_CurrPDG)), -1)     '!!!!!!!!!!!!!!!!!!!!!!!!
		End If

		ArrivalProfile.AddPoint(fDist, OCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), -RadToDeg(System.Math.Atan(_CurrPDG)), -1)

		_FinalOCH = OCH
		HidePandaBox()
	End Sub

	'Private Sub NavInSector(ByRef ForNavaid As NavaidData, ByRef InList() As NavaidData, ByRef OutList() As NavaidData, ByRef pSector As ESRI.ArcGIS.Geometry.IPolygon2)
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim N As Integer

	'	Dim tmpDist As Double
	'	Dim TmpDir As Double
	'	Dim fTmp As Double
	'	Dim d0 As Double

	'	Dim pSectProxi As ESRI.ArcGIS.Geometry.IProximityOperator

	'	N = UBound(InList)

	'	If pSector.IsEmpty() Or (N < 0) Then
	'		ReDim OutList(-1)
	'		Return
	'	Else
	'		ReDim OutList(N + 1)
	'	End If

	'	pSectProxi = pSector

	'	J = -1

	'	For I = 0 To N
	'		tmpDist = ReturnDistanceInMeters(ForNavaid.pPtPrj, InList(I).pPtPrj)
	'		If tmpDist <= distEps Then Continue For

	'		If pSectProxi.ReturnDistance(InList(I).pPtPrj) > 0.0 Then Continue For

	'		If (InList(I).TypeCode = eNavaidType.VOR) Or (InList(I).TypeCode = eNavaidType.NDB) Or (InList(I).TypeCode = eNavaidType.TACAN) Then
	'			J += 1
	'			OutList(J) = InList(I)
	'			OutList(J).Distance = Point2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)

	'			d0 = OnNAVShift(OutList(J).TypeCode, 150.0)

	'			TmpDir = ReturnAngleInDegrees(ForNavaid.pPtPrj, OutList(J).pPtPrj)
	'			If SubtractAngles(TmpDir, _ArDir) > 90.0 Then TmpDir = TmpDir + 180.0

	'			fTmp = RadToDeg(ArcSin(d0 / tmpDist))
	'			OutList(J).FromAngle = Modulus(TmpDir - fTmp, 360.0)
	'			OutList(J).ToAngle = Modulus(TmpDir + fTmp, 360.0)
	'			OutList(J).Tag = 0
	'		End If
	'	Next I

	'	J += 1
	'	OutList(J) = ForNavaid

	'	OutList(J).Distance = Point2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)

	'	'            If tmpDist > distEps Then
	'	'            If OutList(J).Type = 0 Then
	'	'                d0 = VOR.OnNAVRadius
	'	'            Else
	'	'                d0 = NDB.OnNAVRadius
	'	'            End If

	'	d0 = OnNAVShift(OutList(J).TypeCode, 150.0)

	'	OutList(J).FromAngle = 0
	'	OutList(J).ToAngle = 360.0
	'	OutList(J).Tag = 1

	'	If J < 0 Then
	'		ReDim OutList(-1)
	'	Else
	'		ReDim Preserve OutList(J)
	'	End If
	'End Sub

	Private Function WPTInSector(ByRef ForNavaid As NavaidData, ByRef InList() As WPT_FIXType, ByRef pSector As ESRI.ArcGIS.Geometry.IPolygon2) As NavaidData()
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim tmpDist As Double
		Dim TmpDir As Double
		Dim fTmp As Double
		Dim d0 As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim X As Double
		Dim Y As Double

		Dim OutList() As NavaidData

		If pSector.IsEmpty() Then
			ReDim OutList(-1)
			Return OutList
		End If

		N = InList.Length
		ReDim OutList(N - 1)
		If (N <= 0) Then Return OutList

		Dim pSectProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		pSectProxi = pSector

		'DrawPolygon(pSector, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal)
		'Application.DoEvents()

		J = -1
		'Dim TestId As Guid = New Guid("dee1d1dd-1c32-4775-881d-76c14fcce6f2")

		For I = 0 To N - 1
			tmpDist = ReturnDistanceInMeters(ForNavaid.pPtPrj, InList(I).pPtPrj)
			If tmpDist <= distEps Then Continue For

			If pSectProxi.ReturnDistance(InList(I).pPtPrj) > 0.0 Then Continue For

			PrjToLocal(ForNavaid.pPtPrj, _ArDir, InList(I).pPtPrj, X, Y)

			d0 = 0.1
			If (Y < -d0) Or (Y > d0) Then Continue For

			If (InList(I).TypeCode = eNavaidType.VOR) Or (InList(I).TypeCode = eNavaidType.NDB) Or (InList(I).TypeCode = eNavaidType.TACAN) Then
				d0 = OnNAVShift(InList(I).TypeCode, 150.0)
				'If Y > d0 Then Continue For

				TmpDir = ReturnAngleInDegrees(ForNavaid.pPtPrj, InList(I).pPtPrj)
				If SubtractAngles(TmpDir, _ArDir) > 90.0 Then TmpDir = TmpDir + 180.0
				fTmp = RadToDeg(ArcSin(d0 / tmpDist))
				FromAngle = Modulus(TmpDir - fTmp, 360.0)
				ToAngle = Modulus(TmpDir + fTmp, 360.0)

				If (ForNavaid.TypeCode = eNavaidType.LLZ) And ((AnglesSideDef(ToAngle, _ArDir) < 0) Or (AnglesSideDef(FromAngle, _ArDir) > 0)) Then Continue For

				J += 1

				FindNavaid(InList(I).Name, InList(I).TypeCode, OutList(J))
				OutList(J).FromAngle = FromAngle
				OutList(J).ToAngle = ToAngle
			Else
				'd0 = 20.0                            '2  metr
				'If Y > d0 Then Continue For

				TmpDir = ReturnAngleInDegrees(ForNavaid.pPtPrj, InList(I).pPtPrj)
				If SubtractAngles(TmpDir, _ArDir) > 90.0 Then TmpDir = TmpDir + 180.0
				fTmp = RadToDeg(ArcSin(d0 / tmpDist))
				FromAngle = Modulus(TmpDir - fTmp, 360.0)
				ToAngle = Modulus(TmpDir + fTmp, 360.0)
				If (ForNavaid.TypeCode = eNavaidType.LLZ) And ((AnglesSideDef(ToAngle, _ArDir) < 0) Or (AnglesSideDef(FromAngle, _ArDir) > 0)) Then Continue For

				J += 1
				OutList(J) = WPT_FIXToNavaid(InList(I))
				OutList(J).FromAngle = FromAngle
				OutList(J).ToAngle = ToAngle
			End If

			OutList(J).Tag = 0
			OutList(J).Distance = Point2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)
		Next I

		J += 1
		OutList(J) = ForNavaid
		OutList(J).Distance = Point2LineDistancePrj(OutList(J).pPtPrj, RWYTHRPrj, RWYTHRPrj.M - 90.0) * SideDef(RWYTHRPrj, RWYTHRPrj.M - 90.0, OutList(J).pPtPrj)

		'            If tmpDist > distEps Then
		'            If OutList(J).Type = 0 Then
		'                d0 = VOR.OnNAVRadius
		'            Else
		'                d0 = NDB.OnNAVRadius
		'            End If

		OutList(J).FromAngle = 0
		OutList(J).ToAngle = 360.0
		OutList(J).Tag = 1

		d0 = OnNAVShift(OutList(J).TypeCode, 150.0)
		If d0 < 0 Then
			OutList(J).FromAngle = ForNavaid.Course
			OutList(J).ToAngle = ForNavaid.Course
			'OutList(J).Tag = 0
		End If

		If J < 0 Then
			ReDim OutList(-1)
		Else
			ReDim Preserve OutList(J)
		End If

		Return OutList
	End Function

	Private Sub FillCombo1501()
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim nN As Integer

		Dim iSide As Integer
		Dim fDist As Double
		Dim fDir As Double
		'Dim fAzt As Double
		Dim pInterceptPt As IPoint

		ComboBox1501.Items.Clear()
		ComboBox1501.Items.Add("Create new")

		If Not OptionButton1202.Checked Then Return
		K = ComboBox1502.SelectedIndex
		If K < 0 Then Return

		pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)

		N = UBound(WPTList)
		ReDim FixBox1501(N)

		J = -1

		For I = 0 To N
			If SideDef(pInterceptPt, pInterceptPt.M + 90, WPTList(I).pPtPrj) > 0 Then
				fDir = ReturnAngleInDegrees(pInterceptPt, WPTList(I).pPtPrj)

				If (SubtractAngles(fDir, pInterceptPt.M) < 0.5) Then
					fDist = ReturnDistanceInMeters(pInterceptPt, WPTList(I).pPtPrj)
					If (fDist < RModel) Then
						If (TerInterNavDat(K).IntersectionType = eIntersectionType.ByDistance) Or (TerInterNavDat(K).IntersectionType = eIntersectionType.RadarFIX) Then
							fDist = ReturnDistanceInMeters(TerInterNavDat(K).pPtPrj, WPTList(I).pPtPrj)
							iSide = SideDef(TerInterNavDat(K).pPtPrj, pInterceptPt.M + 90, WPTList(I).pPtPrj)
							nN = UBound(TerInterNavDat(K).ValMin)

							If (iSide < 0) Or (nN = 0) Then
								If (fDist >= TerInterNavDat(K).ValMin(0)) And (fDist < TerInterNavDat(K).ValMax(0)) Then
									J = J + 1
									FixBox1501(J) = WPTList(I)
									ComboBox1501.Items.Add(FixBox1501(J).Name)
								End If
							ElseIf (fDist >= TerInterNavDat(K).ValMin(1)) And (fDist < TerInterNavDat(K).ValMax(1)) Then
								J = J + 1
								FixBox1501(J) = WPTList(I)
								ComboBox1501.Items.Add(FixBox1501(J).Name)
							End If
						ElseIf TerInterNavDat(K).IntersectionType = eIntersectionType.ByAngle Then
							fDir = ReturnAngleInDegrees(TerInterNavDat(K).pPtPrj, WPTList(I).pPtPrj)
							'fAzt = Dir2Azt(TerInterNavDat(K).pPtPrj, fDir)

							If AngleInSector(fDir, TerInterNavDat(K).ValMin(0), TerInterNavDat(K).ValMax(0)) Then
								J = J + 1
								FixBox1501(J) = WPTList(I)
								ComboBox1501.Items.Add(FixBox1501(J).Name)
							End If
						End If
					End If
				End If
			End If
		Next I

		ComboBox1501.SelectedIndex = 0
	End Sub

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		Dim e As New EventArgs

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

	'Private Sub CheckBox501_Click()
	'    OKBtn.Enabled = CheckBox501.Value = 0
	'    NextBtn.Enabled = CheckBox501.Value > 0
	'End Sub

	Private Sub AdjustSDFProfile(ByVal fHorDist As Double)
		Dim fDist As Double
		Dim fSDFDist As Double
		Dim fOCH As Double
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint
		Dim N As Integer

		If OptionButton0201.Checked And CheckBox0701.Checked Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		N = 3
		While ArrivalProfile.PointsNo > N
			ArrivalProfile.RemovePoint()
		End While
		'===========================================================================
		fSDFDist = ReturnDistanceInMeters(FictTHR, ptTmpFAF)

		If CheckBox0701.Checked Then
			fOCH = SDFOCH

			If OptionButton0202.Checked Then ArrivalProfile.RemovePoint()

			ArrivalProfile.AddPoint(fHorDist + fSDFDist, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0.0, -1)
			ArrivalProfile.AddPoint(fSDFDist, ptTmpFAF.Z, Modulus(ArAzt - FinalNav.MagVar, 360.0), -RadToDeg(System.Math.Atan(CDbl(0.01 * CDbl(TextBox0701.Text)))), CodeProcedureDistance.OTHER_SDF)
		Else
			fOCH = LimitTextBox0702.Low
		End If

		fDist = fSDFDist - (ptTmpFAF.Z - fOCH) / FinalAreaPDG
		ArrivalProfile.AddPoint(fDist, fOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), -RadToDeg(System.Math.Atan(FinalAreaPDG)), -1)
	End Sub

	Private Sub MAPtByDist(ByRef LSOCShift As Double, ByRef Dist As Double)
		Dim A As Double
		Dim B As Double
		Dim D As Double
		Dim X1 As Double
		Dim X2 As Double
		Dim hMAPt As Double
		Dim TASmin As Double
		Dim TASmax As Double

		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		SafeDeleteElement(MAPtFixElem)
		SafeDeleteElement(MAPtAreaElem)

		'pMAPt.Z = MinOCH + fRefHeight

		'If MinOCH < arMATurnAlt.Value Then
		'	hMAPt = arMATurnAlt.Value + fRefHeight
		'Else
		'	hMAPt = MinOCH + fRefHeight
		'End If

		hMAPt = CurrADHP.Elev

		A = arFAFTolerance.Value 'MAXnear допуска на FAF
		B = arFAFTolerance.Value 'MAXfar допуска на FAF
		D = Dist - LSOCShift

		TASmin = IAS2TAS(3.6 * cVfafMin.Values(Category), hMAPt, arISAmin.Value)
		TASmax = IAS2TAS(3.6 * cVfafMax.Values(Category), hMAPt, arISAmax.Value)

		X1 = System.Math.Sqrt(A * A + (2.77777777777778 * TASmin) ^ 2 + (CurrADHP.WindSpeed * D / TASmin) ^ 2)
		X2 = System.Math.Sqrt(A * A + (2.77777777777778 * TASmax) ^ 2 + (CurrADHP.WindSpeed * D / TASmax) ^ 2)
		dNearMAPt = Math.Max(X1, X2)

		X1 = System.Math.Sqrt(B * B + (3.61111111111111 * TASmin) ^ 2 + (CurrADHP.WindSpeed * D / TASmin) ^ 2)
		X2 = System.Math.Sqrt(B * B + (3.61111111111111 * TASmax) ^ 2 + (CurrADHP.WindSpeed * D / TASmax) ^ 2)
		dFarMAPt = Math.Max(X1, X2)

		X1 = System.Math.Sqrt(B * B + (3.61111111111111 * TASmin) ^ 2 + (CurrADHP.WindSpeed * D / TASmin) ^ 2) + 4.16666666666667 * (TASmin + 3.6 * arNearTerrWindSp.Value)
		X2 = System.Math.Sqrt(B * B + (3.61111111111111 * TASmax) ^ 2 + (CurrADHP.WindSpeed * D / TASmax) ^ 2) + 4.16666666666667 * (TASmax + 3.6 * arNearTerrWindSp.Value)
		dMAPt2SOC = Math.Max(X1, X2)

		pClone = pPolyLeft
		pMAStraightSecLPoly = pClone.Clone
		pClone = pPolyRight
		pMAStraightSecRPoly = pClone.Clone

		pClone = pFullPoly
		pMAPtArea = pClone.Clone
		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		ptTmp = PointAlongPlane(pMAPt, _ArDir + 180.0, dNearMAPt)

		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 10.0 * RModel)            'I-
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 10.0 * RModel)              'I-
		CutPoly(pMAPtArea, pCutter, -1)

		pMAStraightPrimPoly = pClone.Clone
		CutPoly(pMAStraightPrimPoly, pCutter, -1)
		CutPoly(pMAStraightSecLPoly, pCutter, -1)
		CutPoly(pMAStraightSecRPoly, pCutter, -1)

		pTopo = pMAStraightSecLPoly
		pMAStraightSecPoly = pTopo.Union(pMAStraightSecRPoly)
		pTopo = pMAStraightSecPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(pMAStraightPrimPoly, RGB(0, 229, 128), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pMAStraightSecLPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(pMAStraightSecRPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		'====
		ptTmp = PointAlongPlane(pMAPt, _ArDir, dFarMAPt)

		pCutter.FromPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 10.0 * RModel)
		pCutter.ToPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 10.0 * RModel)

		'DrawPolyLine(pCutter, , 2)
		'Application.DoEvents()
		'====
		CutPoly(pMAPtArea, pCutter, 1)
		'====

		MAPtAreaElem = DrawPolygon(pMAPtArea, 0)
		MAPtAreaElem.Locked = True
		'MAPtDef = 1
	End Sub

	Private Sub MAPtOnFIX()
		Dim K As Integer
		Dim I As Integer
		Dim N As Integer

		Dim MAPtDistD As Double
		Dim TrackToler As Double
		Dim InterToler As Double
		Dim hMAPtFix As Double
		Dim fTmp As Double
		Dim fDirl As Double
		Dim fDis As Double
		Dim d0 As Double
		Dim D As Double

		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

		Dim pPolyClone As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSect1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline

		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		K = ComboBox0801.SelectedIndex

		On Error Resume Next
		If Not MAPtFixElem Is Nothing Then pGraphics.DeleteElement(MAPtFixElem)
		If Not MAPtAreaElem Is Nothing Then pGraphics.DeleteElement(MAPtAreaElem)
		On Error GoTo 0

		If FinalNav.TypeCode > eNavaidType.NONE Then
			If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
				TrackToler = VOR.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
				TrackToler = NDB.TrackingTolerance
			ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
				TrackToler = LLZ.TrackingTolerance
			End If

			pPolyClone = New ESRI.ArcGIS.Geometry.Polygon
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, 3.0 * RModel))
			pPolyClone.AddPoint(FinalNav.pPtPrj)
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, 3.0 * RModel))
			pPolyClone.AddPoint(FinalNav.pPtPrj)
		Else
			pClone = pFullPoly
			pPolyClone = pClone.Clone
		End If

		pTopo = pPolyClone
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'pClone = pPolyClone
		'===================================================================================
		'pMAPt.Z = Max(MinOCH, MAPtOCH) + fRefHeight

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		Select Case MAPtNavDat(K).IntersectionType
			Case eIntersectionType.OnNavaid
				pt1 = PointAlongPlane(MAPtNavDat(K).pPtPrj, _ArDir, 0.05)
				pt2 = PointAlongPlane(MAPtNavDat(K).pPtPrj, _ArDir + 180.0, 0.05)
				pSect0 = New ESRI.ArcGIS.Geometry.Polygon
				pSect0.AddPoint(PointAlongPlane(pt1, _ArDir + 90.0, 2.0 * RModel))
				pSect0.AddPoint(PointAlongPlane(pt2, _ArDir + 90.0, 2.0 * RModel))
				pSect0.AddPoint(PointAlongPlane(pt2, _ArDir - 90.0, 2.0 * RModel))
				pSect0.AddPoint(PointAlongPlane(pt1, _ArDir - 90.0, 2.0 * RModel))
				pSect0.AddPoint(pSect0.Point(0))
				MAPtDistD = 0.0
				'MAPtDef = 0
			Case eIntersectionType.ByAngle
				If MAPtNavDat(K).TypeCode = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance
					fDis = NDB.Range
				Else
					InterToler = VOR.IntersectingTolerance
					fDis = VOR.Range
				End If

				fDirl = ReturnAngleInDegrees(MAPtNavDat(K).pPtPrj, pMAPt)

				pt1 = PointAlongPlane(MAPtNavDat(K).pPtPrj, fDirl + InterToler, fDis)
				pt2 = PointAlongPlane(MAPtNavDat(K).pPtPrj, fDirl - InterToler, fDis)

				pSect0 = New ESRI.ArcGIS.Geometry.Polygon
				pSect0.AddPoint(MAPtNavDat(K).pPtPrj)
				pSect0.AddPoint(pt1)
				pSect0.AddPoint(pt2)
				pSect0.AddPoint(MAPtNavDat(K).pPtPrj)

				If MAPtNavDat(K).TypeCode = eNavaidType.NDB Then
					fTmp = Dir2Azt(pMAPt, ReturnAngleInDegrees(pMAPt, MAPtNavDat(K).pPtPrj))
					_Label0801_5.Text = My.Resources.str00228 + CStr(ConvertAngle(fTmp)) + " °"
				Else
					fTmp = Dir2Azt(MAPtNavDat(K).pPtPrj, ReturnAngleInDegrees(MAPtNavDat(K).pPtPrj, pMAPt))
					_Label0801_5.Text = My.Resources.str00227 + CStr(ConvertAngle(fTmp)) + " °"
				End If
			Case eIntersectionType.ByDistance
				fDirl = ReturnDistanceInMeters(pMAPt, MAPtNavDat(K).pPtPrj)
				hMAPtFix = Max(MinOCH, MAPtOCH) + fRefHeight - MAPtNavDat(K).pPtGeo.Z
				d0 = System.Math.Sqrt(fDirl * fDirl + hMAPtFix * hMAPtFix)

				_Label0801_5.Text = My.Resources.str10908 + CStr(ConvertDistance(fDirl - MAPtNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit + vbCrLf + My.Resources.str10909 + CStr(ConvertDistance(d0 - MAPtNavDat(K).Disp, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit

				d0 = d0 * DME.ErrorScalingUp + DME.MinimalError

				D = fDirl + d0
				pSect0 = CreatePrjCircle(MAPtNavDat(K).pPtPrj, D)

				pCutter.FromPoint = PointAlongPlane(MAPtNavDat(K).pPtPrj, _ArDir - 90.0, 10.0 * D)
				pCutter.ToPoint = PointAlongPlane(MAPtNavDat(K).pPtPrj, _ArDir + 90.0, 10.0 * D)

				D = fDirl - d0
				pSect1 = CreatePrjCircle(MAPtNavDat(K).pPtPrj, D)

				pTopo = pSect0
				pTmpPoly = pTopo.Difference(pSect1)

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				If SideDef(pCutter.FromPoint, _ArDir, pCutter.ToPoint) > 0 Then pCutter.ReverseOrientation()

				If SideDef(MAPtNavDat(K).pPtPrj, _ArDir + 90.0, pMAPt) > 0 Then
					pTopo.Cut(pCutter, pSect1, pSect0)
				Else
					pTopo.Cut(pCutter, pSect0, pSect1)
				End If
		End Select

		pTopo = pSect0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pMAPtFIXPoly = pTopo.Intersect(pPolyClone, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)

		If MAPtNavDat(K).IntersectionType <> eIntersectionType.OnNavaid Then
			dFarMAPt = -1000000.0
			dNearMAPt = 1000000.0
			N = pMAPtFIXPoly.PointCount

			For I = 0 To N - 1
				fDis = Point2LineDistancePrj(ptTmpFAF, pMAPtFIXPoly.Point(I), _ArDir + 90.0)
				If fDis > dFarMAPt Then dFarMAPt = fDis
				If fDis < dNearMAPt Then dNearMAPt = fDis
			Next I
		Else
			dFarMAPt = dMAPt2FAF
			dNearMAPt = dMAPt2FAF
		End If

		MAPtDistD = CalcMAPtDistD(MinOCH + fRefHeight, Category)
		dMAPt2SOC = CalcMAPtDistX(MinOCH + fRefHeight, Category)
		dMAPt2SOC = dMAPt2SOC + MAPtDistD + (dFarMAPt - dMAPt2FAF)

		pt1 = PointAlongPlane(ptTmpFAF, _ArDir, dNearMAPt)
		pt2 = PointAlongPlane(ptTmpFAF, _ArDir, dFarMAPt + MAPtDistD)

		dFarMAPt = dFarMAPt - dMAPt2FAF
		dNearMAPt = dMAPt2FAF - dNearMAPt

		pClone = pFullPoly
		pMAPtArea = pClone.Clone

		'pMAPtArea = New ESRI.ArcGIS.Geometry.Polygon
		'pMAPtArea.AddPointCollection(pFullPoly)
		'
		''    If _OptionButton0201_0 Then
		'        pMAPtArea.AddPoint pPolyLeft.Point(5)
		'        pMAPtArea.AddPoint pPolyLeft.Point(0)
		'        pMAPtArea.AddPoint pPolyLeft.Point(1)
		'        pMAPtArea.AddPoint pPolyRight.Point(2)
		'        pMAPtArea.AddPoint pPolyRight.Point(3)
		'        pMAPtArea.AddPoint pPolyRight.Point(4)
		'        pMAPtArea.AddPoint pMAPtArea.Point(0)
		''    Else
		''        pMAPtArea.AddPoint pPolyLeft.Point(0)
		''        pMAPtArea.AddPoint pPolyLeft.Point(1)
		' ''        pMAPtArea.AddPoint pPolyLeft.Point(2)
		'
		' ''       pMAPtArea.AddPoint pPolyRight.Point(1)
		''        pMAPtArea.AddPoint pPolyRight.Point(2)
		''        pMAPtArea.AddPoint pPolyRight.Point(3)
		''        pMAPtArea.AddPoint pMAPtArea.Point(0)
		' ''       Set pTopo = pMAPtArea
		' ''       pTopo.IsKnownSimple_2 = False
		' ''       pTopo.Simplify
		''    End If

		pCutter.FromPoint = PointAlongPlane(pt1, _ArDir - 90.0, 10.0 * RModel)          'II-
		pCutter.ToPoint = PointAlongPlane(pt1, _ArDir + 90.0, 10.0 * RModel)            'II-
		CutPoly(pMAPtArea, pCutter, -1)

		pMAStraightPrimPoly = pClone.Clone
		pClone = pPolyLeft
		pMAStraightSecLPoly = pClone.Clone
		pClone = pPolyRight
		pMAStraightSecRPoly = pClone.Clone

		CutPoly(pMAStraightPrimPoly, pCutter, -1)
		CutPoly(pMAStraightSecLPoly, pCutter, -1)
		CutPoly(pMAStraightSecRPoly, pCutter, -1)

		pTopo = pMAStraightSecLPoly
		pMAStraightSecPoly = pTopo.Union(pMAStraightSecRPoly)
		pTopo = pMAStraightSecPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(pMAStraightPrimPoly, RGB(128, 128, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pMAStraightSecLPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(pMAStraightSecRPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		pCutter.FromPoint = PointAlongPlane(pt2, _ArDir - 90.0, 10.0 * RModel)
		pCutter.ToPoint = PointAlongPlane(pt2, _ArDir + 90.0, 10.0 * RModel)
		CutPoly(pMAPtArea, pCutter, 1)

		'DrawPolyLine(pCutter, , 2)
		'Application.DoEvents()

		On Error Resume Next
		MAPtAreaElem = DrawPolygon(pMAPtArea, RGB(128, 128, 0))
		MAPtAreaElem.Locked = True

		MAPtFixElem = DrawPolygon(pMAPtFIXPoly, RGB(192, 192, 0))
		MAPtFixElem.Locked = True
		'        GetActiveView().PartialRefresh esriViewGraphics, Nothing, Nothing
		'        RefreshCommandBar mTool, 128

		'DrawPolygon(pMAPtArea, RGB(128, 128, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'Application.DoEvents()
		'DrawPolygon(pMAPtFIXPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		On Error GoTo 0
		'==========================================================================
	End Sub

	Private Sub PDGChanged(ByRef NewPDG As Double)
		Dim fSDFDist As Double
		Dim fFAFDist As Double
		Dim fMinSDF As Double
		Dim fMaxSDF As Double
		Dim fSDFPDG As Double
		Dim fHSDF As Double
		Dim fHMin As Double
		Dim fHMax As Double
		Dim fMOC As Double
		Dim fTmp As Double
		Dim fRDH As Double

		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		fRDH = GetRDH()

		If OptionButton0201.Checked Then
			ptTmpFAF = PtFAF
			fHMax = ptTmpFAF.Z
		Else
			ptTmpFAF = FarFAF
			fHMax = FarFAF.Z - fRefHeight
		End If

		If OptionButton0102.Checked Then
			fMOC = IIf(OptionButton0201.Checked, arFASeg_FAF_MOC.Value, arFASegmentMOC.Value)
			fHMin = SDFOCH13 + 2 * fMOC
			If fHMin > fHMax Then fHMin = fHMax
		Else
			fHMin = SDFOCH13
		End If

		fSDFPDG = NewPDG '0.01 * CDbl(TextBox1544.Text)

		fFAFDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR)
		fTmp = (fHMax - fRDH) / fSDFPDG

		If OptionButton0202.Checked Then
			'fSDFDist = Min(arMaxRangeFAS.Value, fFAFDist)
			fSDFDist = Min((fRDH + fFAFDist * nextPDGmax - fHMax) / (nextPDGmax - fSDFPDG), fFAFDist)
		Else
			If Math.Abs(fSDFPDG - nextPDGmax) > PDGEps Then
				fSDFDist = (fRDH + fFAFDist * nextPDGmax - fHMax) / (nextPDGmax - fSDFPDG)
			Else
				fSDFDist = fFAFDist
			End If
		End If

		dMaxSDF = Min(fSDFDist, fTmp)
		dMinSDF = (fHMin - fRDH) / fSDFPDG

		LimitTextBox0702.Low = fHMin 'Round(fHMin + 0.4999)
		LimitTextBox0702.High = dMaxSDF * fSDFPDG + fRDH 'Round(dMaxSDF * fSDFPDG + fRDH - 0.4999)

		fMinSDF = ConvertDistance(dMinSDF, 3)
		fMaxSDF = ConvertDistance(dMaxSDF, 1)
		ToolTip1.SetToolTip(TextBox0706, My.Resources.str00210 + My.Resources.str00221 + CStr(fMinSDF) + My.Resources.str00222 + CStr(fMaxSDF))

		If ComboBox0701.SelectedIndex = 0 Then
			ToolTip1.SetToolTip(TextBox0702, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(LimitTextBox0702.Low + fRefHeight, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High + fRefHeight, eRoundMode.NEAREST)))
		Else
			ToolTip1.SetToolTip(TextBox0702, My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertHeight(LimitTextBox0702.Low, eRoundMode.NEAREST)) + My.Resources.str00222 + CStr(ConvertHeight(LimitTextBox0702.High, eRoundMode.NEAREST)))
		End If

		If IsNumeric(TextBox0702.Text) Then
			fHSDF = DeConvertHeight(CDbl(TextBox0702.Text))
			If ComboBox0701.SelectedIndex = 0 Then fHSDF = fHSDF - fRefHeight
		Else
			fHSDF = -100000.0
		End If

		If (fHSDF < LimitTextBox0702.Low) Or (fHSDF > LimitTextBox0702.High) Then
			If ComboBox0701.SelectedIndex = 0 Then
				If fHSDF < LimitTextBox0702.Low Then fTmp = ConvertHeight(LimitTextBox0702.Low + fRefHeight, eRoundMode.NEAREST)
				If fHSDF > LimitTextBox0702.High Then fTmp = ConvertHeight(LimitTextBox0702.High + fRefHeight, eRoundMode.NEAREST)
			Else
				If fHSDF < LimitTextBox0702.Low Then fTmp = ConvertHeight(LimitTextBox0702.Low, eRoundMode.NEAREST)
				If fHSDF > LimitTextBox0702.High Then fTmp = ConvertHeight(LimitTextBox0702.High, eRoundMode.NEAREST)
			End If
			TextBox0702.Text = CStr(fHSDF)
		End If

		FillCombo0706()

		TextBox0702.Tag = ""
		TextBox0702_Validating(TextBox0702, New System.ComponentModel.CancelEventArgs())
		TextBox0701.Tag = TextBox0701.Text
	End Sub

	Private Sub hKTChanged_WithoutFAF(ByRef fHSDF As Double)
		Dim I As Integer
		Dim N As Integer
		Dim K As Integer
		Dim Ix As Integer

		Dim H As Double
		Dim d0 As Double
		Dim fRDH As Double
		Dim fhFAF As Double
		Dim Toler As Double
		Dim fSDFPDG As Double
		Dim prevPDG As Double

		Dim fSDFDist As Double
		Dim fFAFDist As Double
		Dim ConeAngle As Double
		Dim fLinePtDist As Double
		Dim OldNav As Guid

		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		Try
			If Not FAFAreaElem Is Nothing Then pGraphics.DeleteElement(FAFAreaElem)
		Catch ex As Exception
		End Try

		Try
			If Not FAFFIXElem Is Nothing Then pGraphics.DeleteElement(FAFFIXElem)
		Catch ex As Exception
		End Try

		fRDH = GetRDH()

		If OptionButton0201.Checked Then
			ptTmpFAF = PtFAF
			fhFAF = ptTmpFAF.Z
		Else
			ptTmpFAF = FarFAF
			fhFAF = FarFAF.Z - fRefHeight
		End If

		fSDFPDG = 0.01 * CDbl(TextBox0701.Text)

		'====================================
		If Not CheckBox0702.Checked Then
			fSDFDist = (fHSDF - fRDH) / fSDFPDG
			PtSDF = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
		Else
			fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
		End If

		PtSDF.Z = fHSDF
		PtSDF.M = _ArDir
		PtFAF = PtSDF

		'====================================
		'PtFAF = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
		'PtFAF.Z = fHSDF
		'PtFAF.M = _ArDir
		'PtSDF = PtFAF
		'fSDFDist = (fHSDF - fRDH) / fSDFPDG
		'============================================================================

		fFAFDist = ReturnDistanceInMeters(FictTHR, ptTmpFAF)

		TextBox0706.Text = CStr(ConvertDistance(fSDFDist, eRoundMode.NEAREST))
		TextBox0711.Text = CStr(ConvertDistance(fFAFDist - fSDFDist, eRoundMode.NEAREST))

		'============================================================================
		prevPDGmax = arFADescent_Max.Values(Category)
		prevPDGmin = (fhFAF - fHSDF) / (fFAFDist - fSDFDist)

		If prevPDGmin < arFADescent_Min.Value Then prevPDGmin = arFADescent_Min.Value

		If prevPDGmin > prevPDGmax Then prevPDGmin = prevPDGmax
		prevPDG = arFADescent_Nom.Value
		If prevPDG < prevPDGmin Then prevPDG = prevPDGmin
		If prevPDG > prevPDGmax Then prevPDG = prevPDGmax

		ToolTip1.SetToolTip(TextBox0712, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(100.0 * prevPDGmin + 0.04999999, 1)) + My.Resources.str00222 + CStr(System.Math.Round(100.0 * prevPDGmax - 0.04999999, 1)))
		TextBox0712.Text = CStr(System.Math.Round(100.0 * prevPDG + 0.04999999, 1))
		TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
		'============================================================================
		If ComboBox0702.SelectedIndex >= 0 Then
			OldNav = SDFNavDat(ComboBox0702.SelectedIndex).Identifier
		Else
			OldNav = Guid.Empty
		End If

		SDFNavDat = GetValidFAFNavs(FictTHR, 100000.0, _ArDir, PtFAF, PtFAF.Z, FinalNav.TypeCode, FinalNav.pPtPrj)
		ComboBox0702.Items.Clear()

		N = UBound(SDFNavDat)
		ComboBox0702.Items.Clear()

		Ix = 0
		For I = 0 To N
			ComboBox0702.Items.Add(SDFNavDat(I).CallSign)
			If OldNav = SDFNavDat(I).Identifier Then Ix = I
		Next I

		K = UBound(InSectList)
		If N >= 0 Then
			ReDim Preserve SDFNavDat(K + N + 1)
		ElseIf K >= 0 Then
			ReDim SDFNavDat(K)
		Else
			ReDim SDFNavDat(-1)
		End If

		For I = 0 To K
			'If (InSectList(I).TypeCode = eNavaidType.LLZ) Or (InSectList(I).TypeCode = eNavaidType.DME) Or (InSectList(I).TypeCode = eNavaidType.NONE) Then Continue For
			If (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then Continue For

			fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)
			H = InSectList(I).Distance * fSDFPDG + fRDH + fRefHeight - InSectList(I).pPtGeo.Z
			d0 = OnNAVShift(InSectList(I).TypeCode, H)

			If (InSectList(I).Distance > dMinSDF) And (fLinePtDist <= d0) Then
				If InSectList(I).TypeCode = eNavaidType.NDB Then
					ConeAngle = NDB.ConeAngle '            d0 = NDB.OnNAVRadius
				Else    'If InSectList(I).TypeCode = eNavaidType.CodeVOR Then
					ConeAngle = VOR.ConeAngle '            d0 = VOR.OnNAVRadius
				End If

				Toler = H * System.Math.Tan(DegToRad(ConeAngle))

				If (InSectList(I).Distance + Toler <= fFAFDist) Then                    'And (Abs(fNewFAFRange - InSectList(I).Distance) <= D0) Then
					N = N + 1
					SDFNavDat(N) = InSectList(I)
					SDFNavDat(N).IntersectionType = eIntersectionType.OnNavaid
					SDFNavDat(N).ValCnt = -2
					ComboBox0702.Items.Add(SDFNavDat(N).CallSign)
					If OldNav = SDFNavDat(N).Identifier Then Ix = N
				End If
			End If
		Next I
		'=========================================================================================
		NextBtn.Enabled = N >= 0

		If N >= 0 Then ComboBox0702.SelectedIndex = Ix
		TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
	End Sub

	Private Sub hKTChanged_WithFAF(ByRef fHSDF As Double)
		Dim fRDH As Double
		Dim prevPDG As Double
		Dim fSDFPDG As Double
		Dim fSDFDist As Double
		Dim fFAFDist As Double

		fRDH = GetRDH()

		fSDFPDG = 0.01 * CDbl(TextBox0701.Text)
		If Not CheckBox0702.Checked Then
			fSDFDist = (fHSDF - fRDH) / fSDFPDG
			PtSDF = PointAlongPlane(FictTHR, _ArDir + 180.0, fSDFDist)
		Else
			fSDFDist = ReturnDistanceInMeters(FictTHR, PtSDF)
		End If
		PtSDF.Z = fHSDF
		PtSDF.M = _ArDir

		fFAFDist = ReturnDistanceInMeters(PtFAF, FictTHR) 'CDbl(TextBox1306.Text)
		TextBox0711.Text = CStr(ConvertDistance(fFAFDist - fSDFDist, eRoundMode.NEAREST))
		TextBox0706.Text = CStr(ConvertDistance(fSDFDist, eRoundMode.NEAREST))
		'============================================================================
		'    prevPDGmax = (CurrhFAF - fHSDF) / (fFAFDist - dMaxSDF)
		'    If prevPDGmax > arFADescent_Max.Values(Category) Then
		prevPDGmax = arFADescent_Max.Values(Category)

		prevPDGmin = (CurrhFAF - fHSDF) / (fFAFDist - fSDFDist)
		If prevPDGmin < arFADescent_Min.Value Then prevPDGmin = arFADescent_Min.Value

		If prevPDGmin > prevPDGmax Then prevPDGmin = prevPDGmax
		prevPDG = arFADescent_Nom.Value
		If prevPDG < prevPDGmin Then prevPDG = prevPDGmin
		If prevPDG > prevPDGmax Then prevPDG = prevPDGmax

		ToolTip1.SetToolTip(TextBox0712, My.Resources.str00210 + My.Resources.str00221 + CStr(System.Math.Round(100.0 * prevPDGmin + 0.04999999, 1)) + My.Resources.str00222 + CStr(System.Math.Round(100.0 * prevPDGmax - 0.04999999, 1)))
		TextBox0712.Text = CStr(System.Math.Round(100.0 * prevPDG + 0.04999999, 1))
		'TextBox0712_Validating(TextBox0712, New System.ComponentModel.CancelEventArgs())
		'============================================================================
		FillSDFCombo(fSDFDist)
		'    fSDFDist = CDbl(TextBox1545.Text)
		'    fNewPDG = (fHSDF - fRDH) / fSDFDist
		'    TextBox0701.Text = CStr(Round(100# * fNewPDG, 1))
		'
		'    TextBox0701_AfterUpdate

		'    fPDG = 0.01 * CDbl(TextBox0701.Text)
		'
		'    fSDFDist = ConvertDistance((fHSDF - fRDH) / fPDG, eRoundMode.rmNERAEST)
		'
		'    If fSDFDist > dMaxSDF Then fSDFDist = dMaxSDF
		'    If fSDFDist < dMinSDF Then fSDFDist = dMinSDF
		'
		'    TextBox0706.Text = CStr(fSDFDist)
		'    TextBox0706_Validate False
	End Sub

	Private Function GetNewFAFRange(ByVal fFAFRange As Double, ByVal OCH As Double) As Double
		Dim I As Integer
		Dim N As Integer
		Dim Ix As Integer

		Dim fRDH As Double
		Dim lCurrhFAF As Double

		Dim hRange As LowHigh
		Dim lFAFRange As LowHigh
		Dim ImIntervals() As LowHigh
		Dim LocalObstacles As ObstacleContainer

		fRDH = GetRDH()

		N = UBound(FAFObstacleList.Parts)

		For I = 0 To N
			If FAFObstacleList.Parts(I).Dist > fFAFRange Then Exit For
			If OCH < FAFObstacleList.Parts(I).ReqH Then OCH = FAFObstacleList.Parts(I).ReqH
		Next I

		'Do
		lFAFRange = CalcFAFDispRange(OCH, _CurrPDG)
		If fFAFRange < lFAFRange.Low Then
			fFAFRange = lFAFRange.Low
		End If

		If fFAFRange > lFAFRange.High Then
			fFAFRange = lFAFRange.High
		End If

		lCurrhFAF = fFAFRange * _CurrPDG + fRDH

		hRange.Low = lFAFRange.Low * _CurrPDG + fRDH
		hRange.High = lFAFRange.High * _CurrPDG + fRDH
		Do
			ImIntervals = CalcImIntervalsLocal(lCurrhFAF, fFAFRange, Ix, LocalObstacles)
			If (UBound(ImIntervals) < 0) And (Ix < 0) Then
				Return 0.0
			End If

			If Ix >= 0 Then
				If hRange.High < LocalObstacles.Parts(Ix).ReqH Then
					fFAFRange = CalcNewRange(Ix, LocalObstacles, fFAFRange)

					N = UBound(FAFObstacleList.Parts)

					For I = 0 To N
						If FAFObstacleList.Parts(I).Dist > fFAFRange + distEps Then Exit For
						If OCH < FAFObstacleList.Parts(I).ReqH Then OCH = FAFObstacleList.Parts(I).ReqH
					Next I
					Exit Do
				Else
					lCurrhFAF = LocalObstacles.Parts(Ix).ReqH
					fFAFRange = (lCurrhFAF - fRDH) / _CurrPDG
				End If
			End If
		Loop While Ix >= 0

		Return fFAFRange
	End Function

	Private Function FillFAFCombo(ByRef MaxDist As Double) As Integer
		Dim N As Integer
		Dim M As Integer
		Dim L As Integer
		Dim I As Integer
		Dim J As Integer
		Dim OldNav As Guid
		Dim fNewFAFRange As Double
		Dim fLinePtDist As Double
		Dim ConeAngle As Double
		Dim fRDH As Double
		Dim Toler As Double
		Dim d0 As Double
		Dim H As Double
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pTmpPLine As ESRI.ArcGIS.Geometry.IPolyline

		fRDH = GetRDH()

		pTopo = pFullPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		If ComboBox0302.SelectedIndex >= 0 Then
			OldNav = FAFNavDat(ComboBox0302.SelectedIndex).Identifier
		Else
			OldNav = Guid.Empty
		End If

		'=================================================================
		ComboBox0302.Items.Clear()
		FAFNavDat = GetValidFAFNavs(FictTHR, MaxDist, _ArDir, PtFAF, CurrhFAF, FinalNav.TypeCode, FinalNav.pPtPrj)
		'=================================================================

		M = UBound(InSectList)
		N = UBound(FAFNavDat)
		'If N < 0 Then
		'	msgbox "Для этого значения средства обеспечивающего пересечение не существует", vbCritical
		'	Exit Function
		'End If

		If M >= 0 Then
			If N >= 0 Then
				ReDim Preserve FAFNavDat(M + N + 1)
				ReDim InSectListIx(M + N + 1)
			Else
				ReDim FAFNavDat(M)
				ReDim InSectListIx(M)
			End If
		Else
			If N >= 0 Then
				ReDim InSectListIx(N)
			Else
				ReDim InSectListIx(-1)
			End If
		End If
		L = N

		For I = 0 To N
			ComboBox0302.Items.Add(FAFNavDat(I).CallSign)
			InSectListIx(I) = -1
		Next I

		pTmpPLine = pFAFLine
		pFAFLine = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To M
			If (InSectList(I).TypeCode <= eNavaidType.NONE) Or (InSectList(I).TypeCode = eNavaidType.LLZ) Or (InSectList(I).TypeCode = eNavaidType.DME) Then Continue For

			fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)
			H = InSectList(I).Distance * _CurrPDG + fRDH - InSectList(I).pPtGeo.Z + fRefHeight
			d0 = OnNAVShift(InSectList(I).TypeCode, H)

			If InSectList(I).TypeCode = eNavaidType.NDB Then
				ConeAngle = NDB.ConeAngle
			Else
				ConeAngle = VOR.ConeAngle
			End If

			If (System.Math.Round(InSectList(I).Distance + 0.4999) >= FAFRange.Low) And (InSectList(I).Distance <= MaxDist) And (fLinePtDist <= d0) Then
				Toler = H * System.Math.Tan(DegToRad(ConeAngle))
				If Toler < arOverHeadToler.Value Then Toler = arOverHeadToler.Value
				'====================================================================
				ptTmp = PointAlongPlane(FictTHR, _ArDir + 180.0, InSectList(I).Distance)
				pFAFLine.FromPoint = PointAlongPlane(ptTmp, _ArDir + 90.0, 50000.0)
				pFAFLine.ToPoint = PointAlongPlane(ptTmp, _ArDir - 90.0, 50000.0)
				pFAFLine = pTopo.Intersect(pFAFLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
				If pFAFLine.IsEmpty() Then Continue For
				'====================================================================

				If SideDef(pFAFLine.FromPoint, _ArDir, pFAFLine.ToPoint) < 0 Then pFAFLine.ReverseOrientation()
				fNewFAFRange = GetNewFAFRange(InSectList(I).Distance, CurrOCH)
				'===================================================================
				If (InSectList(I).Distance + Toler <= FAFRange.High) And (System.Math.Abs(fNewFAFRange - InSectList(I).Distance) <= d0) Then
					L = L + 1
					FAFNavDat(L) = InSectList(I)
					FAFNavDat(L).IntersectionType = eIntersectionType.OnNavaid
					FAFNavDat(L).ValCnt = -2

					ComboBox0302.Items.Add(FAFNavDat(L).CallSign)
					InSectListIx(L) = I
				End If
			End If
		Next I

		pFAFLine = pTmpPLine
		pTmpPLine = Nothing
		'=================================================================
		If L >= 0 Then
			ReDim Preserve FAFNavDat(L)
			ComboBox0302.Enabled = True
			J = 0
			For I = 0 To ComboBox0302.Items.Count - 1
				If OldNav = FAFNavDat(I).Identifier Then
					J = I
					Exit For
				End If
			Next I
			ComboBox0302.SelectedIndex = J
			FillFAFCombo = 0
		Else
			_Label0301_10.Text = ""
			ReDim FAFNavDat(-1)
			ComboBox0302.Enabled = False
			ComboBox0302.SelectedIndex = -1
			FillFAFCombo = -1
		End If
	End Function

	Private Function CalcSDFParam(ByRef fHSDF As Double, ByRef maxHorDist As Double) As Integer
		Dim fRDH As Double
		Dim fFAFDist As Double
		Dim fPDG1 As Double
		Dim fPDG2 As Double
		Dim fhFAF As Double
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		fRDH = GetRDH()

		If OptionButton0201.Checked Then
			ptTmpFAF = PtFAF
			fhFAF = ptTmpFAF.Z
		Else
			ptTmpFAF = FarFAF
			fhFAF = FarFAF.Z - fRefHeight '        DominikObstacle.Dist = Point2LineDistancePrj(FictTHR, DominikObstacle.pPtPrj, ArDir + 90.0)
			fHSDF = fFinalOCH
		End If

		CalcSDFParam = 0
		fFAFDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR)

		fPDG2 = (fhFAF - fRDH) / fFAFDist
		fPDG1 = arFADescent_Max.Values(Category) * (fHSDF - fRDH) / (fFAFDist * arFADescent_Max.Values(Category) - fhFAF + fHSDF)
		If fPDG1 < fPDG2 Then fPDG1 = fPDG2

		'    fPDG2 = (fHSDF - fRDH) / DominikObstacle.Dist
		'    nextPDGmin = Round(Max(fPDG1, Max(fPDG2, arFADescent_Min.Value)), 3)
		nextPDGmin = System.Math.Round(Max(fPDG1, arFADescent_Min.Value), 3)

		If nextPDGmin > arFADescent_Max.Values(Category) Then
			CalcSDFParam = -1
			Exit Function
		End If

		nextPDGmax = arFADescent_Max.Values(Category)

		LimitTextBox0701.Low = System.Math.Round(100.0 * nextPDGmin + 0.04999999, 1)
		LimitTextBox0701.High = System.Math.Round(100.0 * nextPDGmax - 0.04999999, 1)

		ToolTip1.SetToolTip(TextBox0701, My.Resources.str00210 + My.Resources.str00221 + CStr(LimitTextBox0701.Low) + My.Resources.str00222 + CStr(LimitTextBox0701.High))
		If arFADescent_Nom.Value > nextPDGmin Then
			TextBox0701.Text = CStr(System.Math.Round(100.0 * arFADescent_Nom.Value, 1))
		Else
			TextBox0701.Text = CStr(System.Math.Round(100.0 * nextPDGmin + 0.04999999, 1))
		End If

		PDGChanged(0.01 * CDbl(TextBox0701.Text))
	End Function

	Private Function FillSDFCombo(ByRef SDFDist As Double) As Integer
		Dim N As Integer
		Dim M As Integer
		Dim K As Integer
		Dim L As Integer
		Dim I As Integer
		Dim J As Integer
		Dim OldNav As Guid

		Dim TrackToler As Double
		Dim fTmp As Double
		Dim fTmpH As Double

		Dim fInterToler As Double
		Dim fRConus As Double
		Dim fDir As Double
		Dim fDist As Double
		Dim fSDist As Double

		Dim fLinePtDist As Double
		Dim fDMEError As Double

		Dim ConeAngle As Double
		Dim fRDH As Double
		Dim Toler As Double
		Dim d0 As Double
		Dim H As Double

		Dim pIntersectPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInnerCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pOuterCircle As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pLGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFixPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pGuidLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTolerLine As ESRI.ArcGIS.Geometry.IPolyline

		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTopoOper1 As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		fRDH = GetRDH()     'arAbv_Treshold.Value + FictTHR.Z - fRefHeight

		If ComboBox0702.SelectedIndex >= 0 Then
			OldNav = SDFNavDat(ComboBox0702.SelectedIndex).Identifier
		Else
			OldNav = Guid.Empty
		End If

		ComboBox0702.Items.Clear()

		N = UBound(NavaidList)
		M = UBound(DMEList)
		K = UBound(InSectList)

		If (N >= 0) Or (M >= 0) Then
			ReDim SDFNavDat(M + N + 1)
		Else
			ReDim SDFNavDat(-1)
			Return -1
		End If

		If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
			fTmp = VOR.Range
		ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
			fTmp = NDB.Range
		ElseIf FinalNav.TypeCode = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
			fTmp = LLZ.Range
		Else
			'   Unknown Navaid type
		End If

		pLGuidPoly = New ESRI.ArcGIS.Geometry.Polygon

		pLGuidPoly.AddPoint(FinalNav.pPtPrj)
		pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler + 180.0, fTmp))
		pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler + 180.0, fTmp))
		pLGuidPoly.AddPoint(FinalNav.pPtPrj)

		If FinalNav.TypeCode <> eNavaidType.LLZ Then
			pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir + TrackToler, fTmp))
			pLGuidPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, _ArDir - TrackToler, fTmp))
			pLGuidPoly.AddPoint(FinalNav.pPtPrj)
		End If

		pTopoOper = pLGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pIntersectPoly = New ESRI.ArcGIS.Geometry.Polygon
		pGuidLine = New ESRI.ArcGIS.Geometry.Polyline
		pGuidLine.FromPoint = PtSDF
		pGuidLine.ToPoint = PointAlongPlane(PtSDF, _ArDir + 180.0, 100000.0)

		L = -1
		For I = 0 To N
			fTmpH = CurrhFAF + fRefHeight - NavaidList(I).pPtPrj.Z
			If (NavaidList(I).TypeCode = eNavaidType.VOR) Or (NavaidList(I).TypeCode = eNavaidType.TACAN) Then
				fInterToler = VOR.IntersectingTolerance
				fRConus = fTmpH * System.Math.Tan(DegToRad(VOR.ConeAngle))
			ElseIf NavaidList(I).TypeCode = eNavaidType.NDB Then
				fInterToler = NDB.IntersectingTolerance
				fRConus = fTmpH * System.Math.Tan(DegToRad(NDB.ConeAngle))
			ElseIf NavaidList(I).TypeCode = eNavaidType.LLZ Then
				'Check azimuth
			End If

			pProxi = pLGuidPoly
			If (NavaidList(I).TypeCode <> eNavaidType.LLZ) And (pProxi.ReturnDistance(NavaidList(I).pPtPrj) > fRConus) Then
				If pIntersectPoly.PointCount > 0 Then pIntersectPoly.RemovePoints(0, pIntersectPoly.PointCount)

				fDist = ReturnDistanceInMeters(NavaidList(I).pPtPrj, PtSDF)
				fDir = ReturnAngleInDegrees(NavaidList(I).pPtPrj, PtSDF)

				pIntersectPoly.AddPoint(NavaidList(I).pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir + fInterToler + 180.0, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir - fInterToler + 180.0, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(NavaidList(I).pPtPrj)
				pIntersectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir + fInterToler, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(PointAlongPlane(NavaidList(I).pPtPrj, fDir - fInterToler, 10.0 * fDist + 100000.0))
				pIntersectPoly.AddPoint(NavaidList(I).pPtPrj)

				pTopoOper1 = pIntersectPoly
				pTopoOper1.IsKnownSimple_2 = False
				pTopoOper1.Simplify()

				pFixPoly = pTopoOper.Intersect(pIntersectPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
				pTopoOper1 = pFixPoly
				pTopoOper1.IsKnownSimple_2 = False
				pTopoOper1.Simplify()

				If pFixPoly.PointCount > 0 Then
					pProxi = pFixPoly
					'If pProxi.ReturnDistance(SDFDominikObstacle.pPtPrj) > 0.0 Then
					pTolerLine = pTopoOper1.Intersect(pGuidLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
					If pTolerLine.Length <= arFAFTolerance.Value Then
						L = L + 1
						SDFNavDat(L) = NavaidList(I)
						SDFNavDat(L).ValCnt = 1
						SDFNavDat(L).IntersectionType = eIntersectionType.ByAngle
						ComboBox0702.Items.Add(SDFNavDat(L).CallSign)
					End If
					'End If
				End If
			End If
		Next I

		For I = 0 To M
			'If pIntersectPoly.PointCount > 0 Then pIntersectPoly.RemovePoints 0, pIntersectPoly.PointCount
			fDir = ReturnAngleInDegrees(DMEList(I).pPtPrj, PtSDF)
			fTmp = SubtractAngles(fDir, _ArDir)
			If (fTmp <= DME.TP_div) Or (180.0 - fTmp <= DME.TP_div) Then
				fTmpH = CurrhFAF + fRefHeight - DMEList(I).pPtPrj.Z
				fDist = ReturnDistanceInMeters(DMEList(I).pPtPrj, PtSDF)
				fSDist = System.Math.Sqrt(fDist * fDist + fTmpH * fTmpH)
				fDMEError = DME.MinimalError + fSDist * DME.ErrorScalingUp

				pInnerCircle = CreatePrjCircle(DMEList(I).pPtPrj, fDist - fDMEError)
				pOuterCircle = CreatePrjCircle(DMEList(I).pPtPrj, fDist + fDMEError)
				pTopoOper1 = pOuterCircle

				pIntersectPoly = pTopoOper1.Difference(pInnerCircle)
				pTopoOper1 = pIntersectPoly
				pTopoOper1.IsKnownSimple_2 = False
				pTopoOper1.Simplify()

				pFixPoly = pTopoOper.Intersect(pIntersectPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
				pTopoOper1 = pFixPoly
				pTopoOper1.IsKnownSimple_2 = False
				pTopoOper1.Simplify()

				If pFixPoly.PointCount > 0 Then
					pProxi = pFixPoly
					'If pProxi.ReturnDistance(SDFDominikObstacle.pPtPrj) > 0.0 Then
					pTolerLine = pTopoOper1.Intersect(pGuidLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
					If pTolerLine.Length <= arFAFTolerance.Value Then
						L = L + 1
						SDFNavDat(L) = DMEList(I)
						SDFNavDat(L).ValCnt = 2
						SDFNavDat(L).IntersectionType = eIntersectionType.ByDistance
						ComboBox0702.Items.Add(SDFNavDat(L).CallSign)
					End If
					'End If
				End If
			End If
		Next I

		'=================================================================

		'    For I = 0 To N
		'        ComboBox0303.AddItem SDFNavDat(I).CallSign
		''        InSectListSDFIx(I) = -1
		'    Next I

		Dim fSDFPDG As Double
		fSDFPDG = 0.01 * CDbl(TextBox0701.Text)

		For I = 0 To K
			If (InSectList(I).TypeCode <> eNavaidType.NDB) And (InSectList(I).TypeCode <> eNavaidType.VOR) And (InSectList(I).TypeCode <> eNavaidType.TACAN) Then Continue For

			fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)
			H = InSectList(I).Distance * fSDFPDG + fRDH - InSectList(I).pPtGeo.Z + fRefHeight
			d0 = OnNAVShift(InSectList(I).TypeCode, H)

			If (InSectList(I).Distance > dMinSDF) And (fLinePtDist <= d0) Then
				If InSectList(I).TypeCode = eNavaidType.NDB Then
					ConeAngle = NDB.ConeAngle '            d0 = NDB.OnNAVRadius
				Else
					ConeAngle = VOR.ConeAngle '            d0 = VOR.OnNAVRadius
				End If

				Toler = H * System.Math.Tan(DegToRad(ConeAngle))
				If (InSectList(I).Distance + Toler <= dMaxSDF) Then 'And (Abs(fNewFAFRange - InSectList(I).Distance) <= D0) Then
					L = L + 1
					SDFNavDat(L) = InSectList(I)
					SDFNavDat(L).IntersectionType = eIntersectionType.OnNavaid
					SDFNavDat(L).ValCnt = -2

					ComboBox0702.Items.Add(SDFNavDat(L).CallSign)
					'                InSectListSDFIx(L) = I
				End If
			End If
		Next I

		'=================================================================
		NextBtn.Enabled = L >= 0

		If L >= 0 Then
			ReDim Preserve SDFNavDat(L)
			J = 0
			For I = 0 To L
				If (OldNav = SDFNavDat(I).Identifier) And (SDFNavDat(I).IntersectionType <> eIntersectionType.OnNavaid) Then
					J = I
					Exit For
				End If
			Next I
			ComboBox0702.SelectedIndex = J
			FillSDFCombo = L + 1
		Else
			_Label0701_6.Text = ""
			_Label0701_7.Text = ""
			ReDim SDFNavDat(-1)
			ComboBox0702.SelectedIndex = -1
			FillSDFCombo = -1
		End If
	End Function

	Private Sub CalcTAParams()
		Dim ptAreaEnd As IPoint
		Dim pTurnComplitPt As IPoint

		pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)
		CircleVectorIntersect(CurrADHP.pPtPrj, RModel, pTurnComplitPt, pTurnComplitPt.M, ptAreaEnd)

		Dim pProxi As IProximityOperator
		If OptionButton0901.Checked Or _bTurnFIXSameMAPtFIX Then
			pProxi = ZNR_Poly
		Else
			pProxi = KK
		End If

		TurnAreaMaxd0 = pProxi.ReturnDistance(ptAreaEnd)

		If OptionButton1203.Checked Then
			TurnAreaMaxd0 = Functions.ReturnDistanceInMeters(ptAreaEnd, TurnDirector.pPtPrj)
		End If

		TextBox1505.Text = ConvertDistance(arBufferMSA.Value, eRoundMode.NEAREST).ToString()
		' =========================================================================================================

		If OptionButton1202.Checked Then
			Dim I As Integer
			Dim N As Integer
			Dim DistMin As Double
			Dim DistMax As Double

			DistMin = 0
			DistMax = ReturnDistanceInMeters(ptAreaEnd, pTurnComplitPt)
			pTurnComplitPt.Z = TurnFixPnt.Z

			TerInterNavDat = GetValidIFNavs(pTurnComplitPt, fRefHeight, DistMin, DistMax, pTurnComplitPt.M + 180.0, fMissAprPDG, TurnDirector.TypeCode, TurnDirector.pPtPrj)

			ComboBox1502.Items.Clear()
			N = UBound(TerInterNavDat)

			For I = 0 To N
				ComboBox1502.Items.Add(TerInterNavDat(I).CallSign)
			Next I

			If SideDef(pTurnComplitPt, pTurnComplitPt.M + 90, TurnDirector.pPtPrj) > 0 Then
				N = N + 1
				If UBound(TerInterNavDat) >= 0 Then
					ReDim Preserve TerInterNavDat(N)
				Else
					ReDim TerInterNavDat(N)
				End If

				TerInterNavDat(N) = WPT_FIXToNavaid(TurnDirector)
				TerInterNavDat(N).IntersectionType = eIntersectionType.OnNavaid
				TerInterNavDat(N).ValCnt = -2

				ComboBox1502.Items.Add(TurnDirector.Name)
			End If

			If N < 0 Then
				Dim Side0 As Integer
				Dim Side1 As Integer

				Dim Dist0 As Double
				Dim Dist1 As Double

				Dim pt0 As IPoint
				Dim pt1 As IPoint

				ReDim TerInterNavDat(0)

				TerInterNavDat(0).pPtGeo = TurnDirector.pPtGeo
				TerInterNavDat(0).pPtPrj = TurnDirector.pPtPrj

				TerInterNavDat(0).CallSign = "Radar FIX"
				TerInterNavDat(0).NAV_Ident = Guid.Empty
				TerInterNavDat(0).Identifier = Guid.Empty
				TerInterNavDat(0).Name = "Radar FIX"
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
					TerInterNavDat(0).ValCnt = -Side0
					ReDim TerInterNavDat(0).ValMax(0)
					ReDim TerInterNavDat(0).ValMin(0)

					TerInterNavDat(0).ValMin(0) = Min(Dist0, Dist1)
					TerInterNavDat(0).ValMax(0) = Max(Dist0, Dist1)
				End If
				'===============================================================
				ComboBox1502.Items.Add("Radar FIX")
			End If

			ComboBox1502.SelectedIndex = 0
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

		If OptionButton0901.Checked Then
			DistPoly = ZNR_Poly
		Else
			DistPoly = KK
		End If

		If Not Double.TryParse(TextBox1505.Text, dBuffer) Then
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

		'DrawPolyLine(pPolyline, -1, 2)
		'Application.DoEvents()

		If OptionButton1201.Checked Then    'Or (TurnDirector.TypeCode <= eNavaidType.NONE) 
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
				PrjToLocal(ptCnt, _OutDir, pPointCollection.Point(I), fCurrX, fCurrY)
				If Math.Abs(fCurrY) > distEps Then Continue For
				If fCurrX > 0.0 Then ptCnt = pPointCollection.Point(I)
			Next

			'ptCnt = pTurnComplitPt
			'If Not pPoly.IsEmpty() Then
			'	If ReturnDistanceInMeters(pPoly.FromPoint, mPoly.FromPoint) > distEps Then pPoly.ReverseOrientation()

			'	PrjToLocal(pTurnComplitPt, m_OutDir, pPoly.ToPoint, fCurrX, fCurrY)
			'	If fCurrX > 0.0 Then ptCnt = pPoly.ToPoint
			'End If
		ElseIf OptionButton1202.Checked Then
			ptCnt = TerFixPnt
		Else 'If OptionButton1203.Checked Then
			ptCnt = TurnDirector.pPtPrj
		End If

		pPath.AddPoint(ptCnt)
		pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
		pPolyline.AddGeometry(pPath)

		If Not OptionButton1203.Checked Then
			For I = 0 To m_BasePoints.PointCount - 1
				PrjToLocal(ptCnt, _OutDir, m_BasePoints.Point(I), fCurrX, fCurrY)
				If fCurrX > 0.0 Then ptCnt = m_BasePoints.Point(I)
			Next

			PrjToLocal(ptCnt, _OutDir, TurnFixPnt, fCurrX, fCurrY)
			If fCurrX > 0.0 Then ptCnt = TurnFixPnt
		End If

		' =====================================================================================
		Dim pCutter As IPolyline
		Dim pFullArea As IPolygon
		Dim pTmpPoly As IGeometry
		Dim pRelation As IRelationalOperator

		pCutter = New Polyline()

		If OptionButton1201.Checked Then
			pCutter.FromPoint = Functions.LocalToPrj(ptCnt, _OutDir, 0.0, 100000.0)     '2.0 * GlobalVars.RModel
			pCutter.ToPoint = Functions.LocalToPrj(ptCnt, _OutDir, 0.0, -100000.0)
		Else
			pCutter.FromPoint = Functions.LocalToPrj(ptCnt, _OutDir, dBuffer, 100000.0) '2.0 * GlobalVars.RModel
			pCutter.ToPoint = Functions.LocalToPrj(ptCnt, _OutDir, dBuffer, -100000.0)
		End If

		pRelation = pCutter

		If (pRelation.Disjoint(BaseArea) Or pRelation.Disjoint(pCircle)) Then
			pFullArea = BaseArea
		Else
			pTopo = BaseArea
			pTopo.Cut(pCutter, pTmpPoly, pFullArea)
		End If

		'DrawPolygon(BaseArea, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'DrawPolyLine(pCutter, 255, 2)
		'Application.DoEvents()

		' =========================================================================================================
		Dim Imx As Integer
		Dim TurnAngle As Double

		TurnAngle = CDbl(TextBox1201.Text)
		GetFinalObstacles(ObstacleList, ObstacleFinalMOCList, pFullArea, SecPoly, SecPoly0, DistPoly, TurnAngle, _ArDir, _OutDir, FinalNav.pPtPrj, pTurnComplitPt, Imx)
		' =========================================================================================================
		Dim N As Integer
		Dim IxDh As Integer
		Dim IxMaxReq As Integer

		Dim Dz As Double
		Dim OCH As Double
		Dim MaxDh As Double
		Dim fAltKK As Double
		Dim MaxReq As Double
		Dim CurrReq As Double
		Dim fEnrouteMOC As Double

		OCH = OverOCH
		Dz = Point2LineDistancePrj(PtSOC, KK.FromPoint, _ArDir + 90.0)
		fAltKK = OCH + Dz * fMissAprPDG
		fEnrouteMOC = DeConvertHeight(CDbl(ComboBox0902.Text))

		IxDh = -1
		MaxDh = 0.0

		IxMaxReq = -1
		MaxReq = fEnrouteMOC

		N = UBound(ObstacleFinalMOCList.Parts)
		For I = 0 To N
			ObstacleFinalMOCList.Parts(I).EffectiveHeight = fAltKK + ObstacleFinalMOCList.Parts(I).Dist * fMissAprPDG
			ObstacleFinalMOCList.Parts(I).hPenet = ObstacleFinalMOCList.Parts(I).ReqH - ObstacleFinalMOCList.Parts(I).EffectiveHeight
			'If TurnAreaMaxd0 < ObstacleFinalMOCList.Parts(I).Dist Then TurnAreaMaxd0 = ObstacleFinalMOCList.Parts(I).Dist

			If ObstacleFinalMOCList.Parts(I).hPenet > MaxDh Then
				MaxDh = ObstacleFinalMOCList.Parts(I).hPenet
				IxDh = I
			End If

			CurrReq = ObstacleFinalMOCList.Parts(I).Height + ObstacleFinalMOCList.Parts(I).fTmp * fEnrouteMOC
			If MaxReq < CurrReq Then
				MaxReq = CurrReq
				IxMaxReq = I
			End If
		Next I

		MaxReq = MaxReq + fRefHeight
		NonPrecReportFrm.FillPage08(ObstacleFinalMOCList, IxDh)
		'=============================================================================================================================================================================================================
		Dim d0 As Double
		If OptionButton1201.Checked Then    'Or (TurnDirector.TypeCode <= eNavaidType.NONE) 
			d0 = dBuffer
		Else
			Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator = DistPoly
			d0 = pProxi.ReturnDistance(ptCnt)
		End If

		'DrawPolyLine(DistPoly, 255, 2)
		'DrawPointWithText(ptCnt, "ptCnt")
		'Application.DoEvents()

		resultOCH = OCH + MaxDh
		If ComboBox1503.SelectedIndex < 0 Then
			ComboBox1503.SelectedIndex = 0
		Else
			ComboBox1503_SelectedIndexChanged(ComboBox1503, New System.EventArgs())
		End If

		fAltKK = resultOCH + Dz * fMissAprPDG
		fReachedA = fAltKK + d0 * fMissAprPDG + fRefHeight
		'fTermAlt = MaxReq
		'*********************************************************************************
		Dim pIZAware As IZAware
		pIZAware = mPoly
		pIZAware.ZAware = True

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pIZ As IZ

		ptTmp = mPoly.ToPoint
		ptTmp.Z = MaxReq - fRefHeight
		mPoly.ToPoint = ptTmp

		pIZ = mPoly
		pIZ.CalculateNonSimpleZs()

		'*********************************************************************************

		TextBox1502.Text = CStr(ConvertHeight(fAltKK + fRefHeight, eRoundMode.NEAREST))
		TextBox1503.Text = CStr(ConvertHeight(MaxReq, eRoundMode.NEAREST))
		TextBox1510.Text = TextBox1503.Text
		TextBox1508.Text = CStr(ConvertHeight(fReachedA, eRoundMode.NEAREST))

		If MaxReq > fReachedA Then
			TextBox1510.ReadOnly = True
			TextBox1510.BackColor = System.Drawing.SystemColors.Control
			OkBtnEnabled = False
		Else
			TextBox1510.ReadOnly = False
			TextBox1510.BackColor = System.Drawing.SystemColors.Window
			OkBtnEnabled = True
		End If

		If IxDh > -1 Then
			TextBox1506.Text = ObstacleFinalMOCList.Obstacles(ObstacleFinalMOCList.Parts(IxDh).Owner).UnicalName
		Else
			TextBox1506.Text = "-"
		End If

		If IxMaxReq > -1 Then
			TextBox1509.Text = ObstacleFinalMOCList.Obstacles(ObstacleFinalMOCList.Parts(IxMaxReq).Owner).UnicalName
		Else
			TextBox1509.Text = "-"
		End If

		OkBtnEnabled = True '???????????????????????????????????????????????????????
		'==================================================================
		Dim dD As Double
		Dim fDist As Double
		Dim fMAPtDist As Double
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		If OptionButton0201.Checked And (CheckBox0701.Checked) Then
			ptTmpFAF = PtSDF
		Else
			ptTmpFAF = PtFAF
		End If

		N = ArrivalProfile.MAPtIndex - 1
		While ArrivalProfile.PointsNo > N
			ArrivalProfile.RemovePoint()
		End While

		'fMAPtDist = ReturnDistanceInMeters(pMAPt, FictTHR)
		fMAPtDist = DeConvertDistance(CDbl(TextBox0801.Text))
		fDist = ReturnDistanceInMeters(ptTmpFAF, FictTHR) - (ptTmpFAF.Z - OCH - MaxDh) / FinalAreaPDG
		'If fDist < fMAPtDist Then fDist = fMAPtDist

		ArrivalProfile.AddPoint(fDist, OCH + MaxDh, Modulus(ArAzt - FinalNav.MagVar, 360.0), 0, -1)
		ArrivalProfile.AddPoint(fMAPtDist, OCH + MaxDh, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, CodeProcedureDistance.MAP)

		dD = Point2LineDistancePrj(FictTHR, TurnFixPnt, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, TurnFixPnt)
		ArrivalProfile.AddPoint(dD, fAltKK, Modulus(ArAzt - FinalNav.MagVar, 360.0), fMissAprPDG, -1, 1)
		'==================================================================
		SafeDeleteElement(NomTrackElem)

		NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

		pGraphics.AddElement(NomTrackElem, 0)
		NomTrackElem.Locked = True
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		TextBox1511.Text = CStr(ConvertDistance(mPoly.Length, eRoundMode.NEAREST))
		TextBox1512.Text = CStr(ConvertHeight(TurnFixPnt.Z + mPoly.Length * fMissAprPDG, eRoundMode.NEAREST))
	End Sub

	Private Sub NextBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles NextBtn.Click
		Dim bOnAero As Boolean
		Dim MAPtFlg As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim Imx As Integer
		Dim Side As Integer
		Dim NavIndex As Integer

		Dim A As Double
		Dim B As Double
		Dim Z As Double
		Dim d0 As Double
		Dim d1 As Double
		Dim Dz As Double
		Dim X1 As Double
		Dim X2 As Double
		Dim COCH As Double
		Dim fTmp As Double
		Dim CntX As Double
		Dim CntY As Double
		Dim fRDH As Double
		Dim fDist As Double
		Dim fHrel As Double
		Dim MaxOCH As Double
		Dim TASmin As Double
		Dim TASmax As Double
		Dim fTmpH1 As Double
		Dim fTmpH2 As Double
		Dim fhMAPt As Double
		Dim fConRad As Double
		Dim dTHR2FAF As Double
		Dim fFinalMOC As Double
		Dim maxHorDist As Double
		Dim DistTHR2NAV As Double
		Dim fLinePtDist As Double

		Dim pReleation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pLine1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pArea As ESRI.ArcGIS.Geometry.IArea

		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptTmpFAF As ESRI.ArcGIS.Geometry.IPoint

		Dim tmpObstacleList As ObstacleContainer
		Dim itmX As System.Windows.Forms.ListViewItem

		Dim pRWYsPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pRWYLine As ESRI.ArcGIS.Geometry.IPolyline

		CurrPage = MultiPage1.SelectedIndex + 1

		If CurrPage > 2 Then fRDH = GetRDH()

		ShowPandaBox(Me.Handle.ToInt32)

		Select Case MultiPage1.SelectedIndex
			Case 0
				'CLState = True
				ComboBox0101.Items.Clear()

				N = UBound(RWYDATA)
				If RWYCollection Is Nothing Then
					RWYCollection = New ESRI.ArcGIS.Geometry.Multipoint
				Else
					RWYCollection.RemovePoints(0, RWYCollection.PointCount)
				End If

				K = -1
				For I = 0 To N
					If RWYDATA(I).Selected Then
						K = K + 1
						RWYIndex(K) = I
						RWYCollection.AddPoint(RWYDATA(I).pPtPrj(eRWY.PtTHR))
						ComboBox0101.Items.Add(RWYDATA(I).Name)
					End If
				Next
				'==========================================================================

				If K < 1 Then
					CurrPage = MultiPage1.SelectedIndex
					HidePandaBox()
					MessageBox.Show(My.Resources.str00300)
					Return
				Else
					If K < 2 Then
						CntX = 0.0
						CntY = 0.0
						For I = 0 To 1
							CntX = CntX + RWYCollection.Point(I).X
							CntY = CntY + RWYCollection.Point(I).Y
						Next I

						CntX = 0.5 * CntX
						CntY = 0.5 * CntY

						_pCentroid = New ESRI.ArcGIS.Geometry.Point
						_pCentroid.PutCoords(CntX, CntY)
						'    pCentroid.PutCoords CurrADHP.pPtPrj.X, CurrADHP.pPtPrj.Y
					Else
						pTopo = RWYCollection
						pRWYsPolygon = pTopo.ConvexHull

						pTopo = pRWYsPolygon
						pTopo.IsKnownSimple_2 = False
						pTopo.Simplify()

						pArea = pRWYsPolygon
						_pCentroid = pArea.Centroid
					End If
				End If
				'==========================================================================

				'FillNavaidList(NavaidList, DMEList, TACANList, CurrADHP, MaxNAVDist)
				'    N = UBound(NavaidList)
				'    If N < 0 Then
				'        HidePandaBox
				'        MessageBox.Show(My.Resources.str505)
				'        CurrPage = MultiPage1.Tab
				'        Return
				'    End If
				'FillWPT_FIXList(WPTList, CurrADHP, MaxNAVDist)

				N = UBound(WPTList)
				If N >= 0 Then
					ReDim FixAngl(N)
					K = -1
					For I = 0 To N
						If (WPTList(I).TypeCode = eNavaidType.VOR) Or (WPTList(I).TypeCode = eNavaidType.NDB) Or (WPTList(I).TypeCode = eNavaidType.TACAN) Then
							K = K + 1
							FixAngl(K) = WPTList(I)
						End If
					Next I

					If K >= 0 Then
						ReDim Preserve FixAngl(K)
					Else
						ReDim FixAngl(-1)
						OptionButton1202.Enabled = False
					End If
				Else
					ReDim FixAngl(-1)
					OptionButton1202.Enabled = False
					OptionButton1203.Enabled = False
					ComboBox1201.Enabled = False
				End If

				TextBox0104.Text = CStr(arSemiSpan.Values(Category))
				TextBox0105.Text = CStr(arVerticalSize.Values(Category))

				If OptionButton0101.Checked Then
					OptionButton0101_CheckedChanged(OptionButton0101, New System.EventArgs())
				Else
					OptionButton0101.Checked = True
				End If

				_Label0201_11.Text = CStr(ConvertHeight(fVisAprOCH, eRoundMode.NEAREST))

				_finalIAS = 3.6 * cVfafMax.Values(Category)
				TextBox0610.Text = CStr(ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL))

				Label0601_12.Text = My.Resources.str00210 + My.Resources.str00221 + CStr(ConvertDSpeed(arMinHV_FAS.Values(Category), 2)) + My.Resources.str00222 + CStr(ConvertDSpeed(arMaxHV_FAS.Values(Category), 2))
				'	Label52.Caption = "Макс. значение: " + CStr(arMaxOutBoundDes.Values(ComboBox0001.ListIndex) / arMaxOutBoundDes.Multiplier)
				'																		/ arMaxHV_FAS.Multiplier
				TextBox0611.Text = CStr(ConvertDSpeed(arMaxHV_FAS.Values(Category), 2)) '/ arMaxOutBoundDes.Multiplier
				TextBox0604.Text = CStr(ConvertDSpeed(arMaxOutBoundDes.Values(Category), 2))

				_Label0901_2.Text = My.Resources.str00220 + vbCrLf + My.Resources.str00221 + CStr(ConvertSpeed(cVmaInter.Values(Category), eRoundMode.SPECIAL)) + My.Resources.str00222 + CStr(ConvertSpeed(cVmaFaf.Values(Category), eRoundMode.SPECIAL)) + " " + SpeedConverter(SpeedUnit).Unit
				TextBox0902.Text = CStr(ConvertSpeed(cVmaFaf.Values(Category), eRoundMode.SPECIAL))

				'	Me.HelpContextID = 8200
				'=================== Start Log =========Визуальный маневр=======================
				'LogStr(My.Resources.str601 + MultiPage1.TabPages.Item(0).Text)
				'LogStr(My.Resources.str602)

				'For I = 0 To ListView0001.Items.Count - 1
				'	If ListView0001.Items.Item(I).Checked Then
				'		LogStr("    " + ListView0001.Items.Item(I).Text)
				'	End If
				'Next I

				'LogStr(My.Resources.str603 + ComboBox0001.Text)
				'LogStr(My.Resources.str604 + TextBox0001.Text + " " + Label0001_07.Text)
				'LogStr(My.Resources.str605 + TextBox0002.Text + " " + Label0001_08.Text)
				'LogStr(My.Resources.str606 + TextBox0003.Text + " " + Label0001_09.Text)

				'LogStr(My.Resources.str607 + TextBox0007.Text + " " + Label0001_20.Text)
				'LogStr(My.Resources.str608 + TextBox0008.Text + " " + Label0001_21.Text)
				'LogStr(My.Resources.str609 + TextBox0009.Text + " " + Label0001_22.Text)
				'LogStr(My.Resources.str610 + TextBox0010.Text + " " + Label0001_23.Text)

				'LogStr(My.Resources.str611 + TextBox0004.Text + " " + Label0001_10.Text)
				'LogStr(My.Resources.str612 + TextBox0005.Text + " " + Label0001_11.Text)
				'LogStr(My.Resources.str613 + TextBox0006.Text + " " + Label0001_12.Text)
				'LogStr(My.Resources.str614 + TextBox0011.Text + " " + Label0001_24.Text)
				'LogStr(My.Resources.str615 + TextBox0012.Text + " " + Label0001_25.Text)
				'LogStr(My.Resources.str616 + TextBox0013.Text)
				'=================== End Log ================================
			Case 1
				'ComboBox0201

				'    If _OptionButton0101_0.Value And (RWYTHRPrj.Z < CurrADHP.pPtGeo.Z - 2.0) Then
				'        fRefHeight = RWYTHRPrj.Z
				'    Else
				'        fRefHeight = CurrADHP.pPtGeo.Z
				'    End If


				If GlobalVars._settings.AnnexObstalce = True Then
					GetAnnexObstacles(ObstacleList, CurrADHP.Identifier, fRefHeight)
				Else
					GetObstaclesByDist(ObstacleList, CurrADHP.pPtPrj, RModel, fRefHeight)
				End If

				ComboBox0201.Items.Clear()
				N = UBound(Solutions)
				For I = 0 To N
					If Solutions(I).Low = Solutions(I).High Then
						ComboBox0201.Items.Add(CStr(ConvertAngle(Solutions(I).Low)))
					Else
						ComboBox0201.Items.Add(CStr(ConvertAngle(Solutions(I).Low)) + "-" + CStr(ConvertAngle(Solutions(I).High)))
					End If
				Next I

				ComboBox0201.SelectedIndex = 0

				If OptionButton0201.Checked Then
					OptionButton0201_CheckedChanged(OptionButton0201, Nothing)
				ElseIf OptionButton0202.Checked Then
					OptionButton0201_CheckedChanged(OptionButton0202, Nothing)
				End If

				ArrivalProfile.InitWOFAF(SelectedRWY.Length, 1 + 2 * CShort(SelectedRWY.TrueBearing > 180.0), FictTHR.Z, fRefHeight, ProfileBtn)
				ProfileBtn.Enabled = True

				'=================== Start Log =========ВПП и тип захода=======================
				'LogStr(My.Resources.str621 + MultiPage1.TabPages.Item(1).Text)

				'If OptionButton0101.Checked Then
				'	LogStr(My.Resources.str622 + ComboBox0101.Text)
				'End If

				'LogStr(My.Resources.str623 + ComboBox0102.Text + " " + _Label0101_5.Text)
				'LogStr("    " + _Label0101_7.Text)
				'LogStr(My.Resources.str624 + TextBox0102.Text + " " + _Label0101_10.Text)

				'If OptionButton0101.Checked Then
				'	LogStr(My.Resources.str625 + OptionButton0101.Text)
				'Else
				'	LogStr(My.Resources.str625 + OptionButton0102.Text)
				'End If

				'LogStr(My.Resources.str628 + TextBox0101.Text + " " + _Label0101_4.Text)
				'LogStr(My.Resources.str626 + TextBox0103.Text + " " + _Label0101_9.Text)

				'LogStr("    " + _Label0101_8.Text)
				'   LogStr "               " + ListView0101
				'=================== End Log ================================
			Case 2
				If FinalNav.TypeCode = eNavaidType.LLZ Then
					ArAzt = FinalNav.pPtGeo.M
				Else
					If SpinButton0201.SelectedItem Is Nothing Then
						ArAzt = CDbl(SpinButton0201.Text)
					Else
						ArAzt = SpinButton0201.SelectedItem
					End If
				End If

				_ArDir = Azt2Dir(FinalNav.pPtGeo, ArAzt)
				pCircle = CreatePrjCircle(FinalNav.pPtPrj, RModel)

				N = UBound(InSectList)

				For I = 0 To N
					InSectList(I).Distance = Point2LineDistancePrj(InSectList(I).pPtPrj, FictTHR, _ArDir - 90.0) * SideDef(FictTHR, _ArDir - 90.0, InSectList(I).pPtPrj)
				Next I
				'=============================================  16:10 03.07.2003
				Ss = CDbl(TextBox0104.Text)
				Vs = CDbl(TextBox0105.Text)

				CreateNavaidZone(FinalNav, _ArDir, FictTHR, Ss, Vs, 2 * VOR.Range, 2 * VOR.Range, pPolyLeft, pPolyRight, pPolyPrime)    ', True

				pTopo = pPolyLeft
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTopo = pPolyRight
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTopo = pPolyPrime
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pFullPoly = pTopo.Union(pPolyRight)
				pTopo = pFullPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pFullPoly = pTopo.Union(pPolyLeft)
				pTopo = pFullPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				'===========================================================
				If OptionButton0202.Checked Then ' Без FAF
					'===========================================================
					ListView0501.Items.Clear()
					ListBox0501.Items.Clear()

					_bHaveMSA = GetMSASectors(FinalNav, MSAList)
					ListView0501.Enabled = _bHaveMSA
					ListBox0501.Enabled = _bHaveMSA

					If _bHaveMSA Then
						N = UBound(MSAList(0).Sectors)
						MinHeight = MSAList(0).Sectors(0).LowerLimit
						MaxHeight = MSAList(0).Sectors(0).UpperLimit

						For I = 0 To N
							If MinHeight > MSAList(0).Sectors(I).LowerLimit Then MinHeight = MSAList(0).Sectors(I).LowerLimit
							If MaxHeight < MSAList(0).Sectors(I).UpperLimit Then MaxHeight = MSAList(0).Sectors(I).UpperLimit

							itmX = ListView0501.Items.Add(My.Resources.str00627 + (I + 1).ToString())
							If N > 0 Then
								itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(MSAList(0).Sectors(I).FromAngle)) + "°"))
								itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertAngle(MSAList(0).Sectors(I).ToAngle)) + "°"))
							Else
								itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "0°"))
								itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "360°"))
							End If

							itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(MSAList(0).Sectors(I).LowerLimit, eRoundMode.NEAREST))))
						Next I

						MinHeight = MinHeight - fRefHeight
						MaxHeight = MaxHeight - fRefHeight
					Else
						MinHeight = FinalNav.pPtPrj.Z + arMSAMOC.Value - fRefHeight
						MaxHeight = MinHeight + 2 * arMSAMOC.Value
					End If

					If ComboBox0501.SelectedIndex = 0 Then
						TextBox0501.Text = CStr(ConvertHeight(MaxHeight + fRefHeight, eRoundMode.NEAREST))
						_Label0601_1.Text = My.Resources.str10721
						_Label0601_5.Text = My.Resources.str10719
						_Label0601_13.Text = My.Resources.str10717
					Else
						TextBox0501.Text = CStr(ConvertHeight(MaxHeight, eRoundMode.NEAREST))
						_Label0601_1.Text = My.Resources.str10714
						_Label0601_5.Text = My.Resources.str10712
						_Label0601_13.Text = My.Resources.str10711
					End If
					TextBox0501_Validating(TextBox0501, New System.ComponentModel.CancelEventArgs(True))

					CurrPage = CurrPage + 2
					dFarMAPt = 0.0
					dNearMAPt = 0.0
				Else ' С FAF
					pLine = New ESRI.ArcGIS.Geometry.Polyline
					pLine.FromPoint = PointAlongPlane(FictTHR, _ArDir + 90.0, 5.0 * RModel)
					pLine.ToPoint = PointAlongPlane(FictTHR, _ArDir - 90.0, 5.0 * RModel)

					ClipByLine(pFullPoly, pLine, Nothing, pTmpPoly, Nothing)

					CalcArObstaclesNavMOC(ObstacleList, tmpObstacleList, FinalNav.pPtPrj, FictTHR, _ArDir, pPolyPrime, pTmpPoly, 4, Imx)
					fTmp = ReturnDistanceInMeters(FinalNav.pPtPrj, FictTHR)

					N = UBound(tmpObstacleList.Parts)
					M = UBound(tmpObstacleList.Obstacles)
					If N < 0 Then
						ReDim FAFObstacleList.Parts(-1)
						ReDim FAFObstacleList.Obstacles(-1)
					Else
						ReDim FAFObstacleList.Obstacles(M)
						For I = 0 To M
							tmpObstacleList.Obstacles(I).NIx = -1
						Next

						ReDim FAFObstacleList.Parts(N)
						K = -1
						L = -1
						For I = 0 To N
							fDist = Point2LineDistancePrj(tmpObstacleList.Parts(I).pPtPrj, FictTHR, _ArDir + 90.0)
							If fDist > arFAFLenght.Value Then Continue For

							K = K + 1
							FAFObstacleList.Parts(K) = tmpObstacleList.Parts(I)

							If tmpObstacleList.Obstacles(tmpObstacleList.Parts(I).Owner).NIx < 0 Then
								L += 1
								FAFObstacleList.Obstacles(L) = tmpObstacleList.Obstacles(tmpObstacleList.Parts(I).Owner)
								FAFObstacleList.Obstacles(L).PartsNum = 0
								ReDim FAFObstacleList.Obstacles(L).Parts(tmpObstacleList.Obstacles(tmpObstacleList.Parts(I).Owner).PartsNum - 1)
								tmpObstacleList.Obstacles(tmpObstacleList.Parts(I).Owner).NIx = L
							End If

							FAFObstacleList.Parts(K).Owner = tmpObstacleList.Obstacles(tmpObstacleList.Parts(I).Owner).NIx
							FAFObstacleList.Obstacles(FAFObstacleList.Parts(K).Owner).Parts(FAFObstacleList.Obstacles(FAFObstacleList.Parts(K).Owner).PartsNum) = K
							FAFObstacleList.Obstacles(FAFObstacleList.Parts(K).Owner).PartsNum += 1

							FAFObstacleList.Parts(K).Dist = fDist
							FAFObstacleList.Parts(K).DistStar = Point2LineDistancePrj(tmpObstacleList.Parts(I).pPtPrj, FictTHR, _ArDir) * SideDef(FictTHR, _ArDir, tmpObstacleList.Parts(I).pPtPrj)
						Next I

						If K >= 0 Then
							ReDim Preserve FAFObstacleList.Obstacles(L)
							ReDim Preserve FAFObstacleList.Parts(K)
						Else
							ReDim FAFObstacleList.Obstacles(-1)
							ReDim FAFObstacleList.Parts(-1)
						End If
						Sort(FAFObstacleList, 0)
					End If

					ComboBox0305.Items.Clear()

					N = UBound(InSectList)

					For I = 0 To N
						If InSectList(I).TypeCode <> eNavaidType.NONE Then Continue For

						fDist = Point2LineDistanceSigned(InSectList(I).pPtPrj, FictTHR, _ArDir + 90.0)
						If fDist > arFAFLenght.Value Then Continue For
						If fDist < 5556.0 Then Continue For

						ComboBox0305.Items.Add(InSectList(I))
					Next

					If ComboBox0305.Items.Count > 0 Then
						CheckBox0301.Enabled = True
						ComboBox0305.SelectedIndex = 0
					Else
						CheckBox0301.Enabled = False
						CheckBox0301.Checked = False
					End If

					_CurrPDG = 0.01 * CDbl(TextBox0306.Text)
					FinalAreaPDG = _CurrPDG
					CurrhFAF = arFAFOptimalDist.Value * _CurrPDG + fRDH

					If fMinOCH > CurrhFAF Then CurrhFAF = fMinOCH

					TextBox0301.Text = CStr(ConvertDistance(arFAFOptimalDist.Value, eRoundMode.NEAREST))

					'        CheckBox0301.Value = False
					ComboBox0301.SelectedIndex = 0
					'Caller(CurrOCH, arFAFOptimalDist.Value, False)
					MinOCH = CurrOCH
					_FinalOCH = CurrOCH
					If ComboBox0304.SelectedIndex = -1 Then
						ComboBox0304.SelectedIndex = 0
					Else
						ComboBox0304_SelectedIndexChanged(ComboBox0304, New System.EventArgs())
					End If
				End If

				'=================== Start Log =========Курс конечного этапа=======================
				'LogStr(My.Resources.str631 + MultiPage1.TabPages.Item(2).Text)
				'LogStr(My.Resources.str632 + ComboBox0201.Text)
				'LogStr(My.Resources.str633 + SpinButton0201.Text)
				'LogStr(My.Resources.str634 + CStr(System.Math.Round(Modulus(ArAzt - FinalNav.MagVar, 360.0))))

				'If OptionButton0102.Checked Then
				'	LogStr("    " + _Label0201_7.Text)
				'	LogStr("    " + _Label0201_1.Text + ": " + _Label0201_8.Text + " " + _Label0201_13.Text)
				'	LogStr("    " + _Label0201_2.Text + ":                         " + _Label0201_9.Text + " " + _Label0201_14.Text)
				'End If

				'LogStr("    " + _Label0201_3.Text + " " + _Label0201_10.Text + " " + _Label0201_15.Text)

				'If OptionButton0101.Checked Then
				'	'        LogStr "    Удаление точки сопряжения от порога ВПП :" + Label27.Caption + " " + _Label0201_1.Caption
				'	LogStr(My.Resources.str635 + _Label0201_12.Text + " " + _Label0201_17.Text)
				'End If
				'LogStr(My.Resources.str636 + _Label0201_11.Text + " " + _Label0201_16.Text)
				'If OptionButton0201.Checked Then
				'	LogStr("    " + OptionButton0201.Text)
				'Else
				'	LogStr("    " + OptionButton0202.Text)
				'End If
				'    LogStr "               " + ListView0201
				'=================== End Log ================================
			Case 3
				If ComboBox0302.Items.Count = 0 Then
					CurrPage = MultiPage1.SelectedIndex
					HidePandaBox()
					MessageBox.Show(My.Resources.str00638 + My.Resources.str00301)
					Return
				End If

				pClone = NavGuadArea2
				'====================================================IF
				IFNavDat = GetValidIFNavs(PtFAF, fRefHeight, fNearDist, fFarDist, _ArDir, arImDescent_Max.Value, FinalNav.TypeCode, FinalNav.pPtPrj)

				M = UBound(IFNavDat)
				N = UBound(InSectList)

				If N >= 0 Then
					If M >= 0 Then
						ReDim Preserve IFNavDat(M + N + 2)
					Else
						ReDim IFNavDat(N + 1)
					End If
				Else
					If M >= 0 Then
						ReDim Preserve IFNavDat(M + 1)
					Else
						ReDim IFNavDat(0)
					End If
				End If

				TextBox0409.Text = CStr(ConvertDistance(arImHorSegLen.Values(Category), eRoundMode.NEAREST))
				pReleation = pIFPoly

				If pReleation.Contains(FinalNav.pPtPrj) Then
					fDist = Point2LineDistancePrj(PtFAF, FinalNav.pPtPrj, _ArDir + 90.0)
					fTmp = fDist * arImDescent_Max.Value + PtFAF.Z + FictTHR.Z - FinalNav.pPtPrj.Z

					If (FinalNav.TypeCode = eNavaidType.VOR) Or (FinalNav.TypeCode = eNavaidType.TACAN) Then
						fConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * fTmp
					ElseIf FinalNav.TypeCode = eNavaidType.NDB Then
						fConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * fTmp
					Else
						fConRad = 0.0
					End If

					If fFarDist - fDist >= fConRad Then
						M = M + 1
						IFNavDat(M) = FinalNav
						IFNavDat(M).IntersectionType = eIntersectionType.OnNavaid
						IFNavDat(M).ValCnt = -2
					End If
				End If
				'===============================================
				ComboBox0403.Items.Clear()

				For I = 0 To N
					If InSectList(I).TypeCode = eNavaidType.NONE Then
						PrjToLocal(PtFAF, _ArDir + 180.0, InSectList(I).pPtPrj, fDist, fTmp)
						If (fDist >= fNearDist) And (fDist <= fFarDist) And (Math.Abs(fTmp) < 2) Then ComboBox0403.Items.Add(InSectList(I))
					Else
						fDist = Point2LineDistancePrj(PtFAF, InSectList(I).pPtPrj, _ArDir + 90.0)
						fTmp = fDist * arImDescent_Max.Value + PtFAF.Z + FictTHR.Z - InSectList(I).pPtGeo.Z

						d0 = OnNAVShift(InSectList(I).TypeCode, fTmp)

						fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)

						If pReleation.Contains(InSectList(I).pPtPrj) And (fLinePtDist <= d0) Then
							fDist = Point2LineDistancePrj(PtFAF, InSectList(I).pPtPrj, _ArDir + 90.0)

							If InSectList(I).TypeCode = eNavaidType.NDB Then
								fConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * fTmp
							Else
								fConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * fTmp
							End If

							If fFarDist - fDist >= fConRad Then
								M = M + 1
								IFNavDat(M) = InSectList(I)
								IFNavDat(M).IntersectionType = eIntersectionType.OnNavaid
								IFNavDat(M).ValCnt = -2
							End If
						End If
					End If
				Next I

				If ComboBox0403.Items.Count > 0 Then
					CheckBox0401.Enabled = True
					ComboBox0403.SelectedIndex = 0
				Else
					CheckBox0401.Enabled = False
					CheckBox0401.Checked = False
				End If

				If M >= 0 Then
					ReDim Preserve IFNavDat(M)
				Else
					CurrPage = MultiPage1.SelectedIndex
					ReDim IFNavDat(-1)
					HidePandaBox()
					MessageBox.Show(My.Resources.str00637 + My.Resources.str00301) '"Нет подходящего средства для создания IF" Verify
					Return
				End If

				ComboBox0401.Items.Clear()

				For I = 0 To M
					ComboBox0401.Items.Add(IFNavDat(I).CallSign)
				Next I

				ComboBox0401.SelectedIndex = 0
				'=================== Start Log ========Конечный этап========================
				'LogStr(My.Resources.str641 + MultiPage1.TabPages.Item(3).Text)
				'LogStr(My.Resources.str642 + TextBox0301.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + ComboBox0303.Text + "                      " + TextBox0303.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str643 + TextBox0306.Text + " " + _Label0301_9.Text)
				'LogStr(My.Resources.str644)
				'LogStr(ComboBox0304.Text + ": " + TextBox0302.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str646 + TextBox0304.Text)
				'LogStr(My.Resources.str647 + ComboBox0302.Text + " " + _Label0301_10.Text)
				'LogStr(My.Resources.str648 + TextBox0305.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + _Label0301_4.Text)

				'LogStr(My.Resources.str649)
				'LogStr(My.Resources.str650 + ComboBox0301.Text)
				'LogStr(My.Resources.str651 + TextBox0307.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'=================== End Log ================================
			Case 4
				CurrPage = CurrPage + 2
				CheckBox0701.Enabled = False
				CheckBox0701.Checked = False

				TextBox0710.Tag = CStr(PtFAF.Z + fRefHeight)
				SDFOCH13 = CurrOCH

				TextBox0701.Tag = TextBox0306.Text
				TextBox0712.Tag = TextBox0306.Text

				ComboBox0703_SelectedIndexChanged(ComboBox0703, New System.EventArgs())

				If ComboBox0704.SelectedIndex < 0 Then
					ComboBox0704.SelectedIndex = 0
				Else
					ComboBox0704_SelectedIndexChanged(ComboBox0704, New System.EventArgs())
				End If

				TextBox0714.Text = TextBox0304.Text

				If (Not OptionButton0101.Checked) Or (SDFDominikObstacle >= 0) Then
					CalcSDFParam(SDFOCH13, maxHorDist)
					CheckBox0701.Enabled = True
					If Not CheckBox0701.Checked Then
						CheckBox0701.Checked = True
					Else
						CheckBox0701_CheckedChanged(CheckBox0701, New System.EventArgs())
					End If
				End If

				'??????????????????????????????????????????????????????????
				'ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo
				'=================== Start Log ========Промежуточный этап========================
				'LogStr(My.Resources.str656 + MultiPage1.TabPages.Item(4).Text)
				'LogStr(My.Resources.str657 + TextBox0401.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + ComboBox0402.Text + ":              " + TextBox0404.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str658 + TextBox0408.Text + " °")
				'LogStr(My.Resources.str659 + TextBox0409.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr(My.Resources.str663)
				'LogStr(My.Resources.str664 + ComboBox0401.Text + " " + _Label0401_8.Text)
				'LogStr("    " + _Label0401_3.Text + "   " + TextBox0403.Text + " " + _Label0401_13.Text)
				'LogStr(My.Resources.str665 + TextBox0407.Text + " " + _Label0401_14.Text)

				'If OptionButton0401.Checked Then
				'	LogStr("    " + OptionButton0401.Text)
				'Else
				'	LogStr("    " + OptionButton0402.Text)
				'End If
				'LogStr("    " + _Label0401_4.Text)
				'=================== End Log ================================
			Case 5
				NavGuadArea = pPolyPrime
				NavGuadArea2 = pFullPoly

				TextBox0601.Text = TextBox0501.Text

				_initialIAS = 3.6 * cViafStar.Values(Category)
				TextBox0603.Text = CStr(ConvertSpeed(cViafStar.Values(Category), eRoundMode.SPECIAL))
				TextBox0603.Tag = TextBox0603.Text

				Dim Hhold As Double

				If ComboBox0501.SelectedIndex = 0 Then
					Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
				Else
					Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
				End If

				_initialTAS = IAS2TAS(_initialIAS, Hhold, CurrADHP.ISAtC)
				TextBox0617.Text = ConvertSpeed(_initialTAS / 3.6).ToString()

				If OptionButton0604.Checked Then
					OptionButton0604_CheckedChanged(OptionButton0604, New System.EventArgs())
				Else
					OptionButton0604.Checked = True
				End If
				'=================== Start Log =========IAF над средством=======================
				'LogStr(My.Resources.str671 + MultiPage1.TabPages.Item(5).Text)
				'If ComboBox0501.SelectedIndex = 0 Then
				'	LogStr(My.Resources.str672 + TextBox0501.Text + " " + _Label0501_0.Text)
				'Else
				'	LogStr(My.Resources.str673 + TextBox0501.Text + " " + _Label0501_0.Text)
				'End If
				'=================== End Log ================================
			Case 6
				If TextBox0606.ForeColor = Color.Red Then
					CurrPage = MultiPage1.SelectedIndex
					HidePandaBox()
					MessageBox.Show(My.Resources.str01001) '"Не обеспечена необходимая высота над торцом ВПП"
					Return
				End If

				TextBox0710.Tag = CStr(FarFAF.Z)
				SDFOCH13 = fFinalOCH
				ComboBox0703_SelectedIndexChanged(ComboBox0703, New System.EventArgs())

				If ComboBox0704.SelectedIndex < 0 Then
					ComboBox0704.SelectedIndex = 0
				Else
					ComboBox0704_SelectedIndexChanged(ComboBox0704, New System.EventArgs())
				End If

				TextBox0714.Text = TextBox0615.Text
				ToolTip1.SetToolTip(TextBox0714, ToolTip1.GetToolTip(TextBox0615))

				If (Not OptionButton0101.Checked) Or (SDFDominikObstacle >= 0) Then
					CalcSDFParam(SDFOCH13, maxHorDist)
					CheckBox0701.Enabled = True

					If Not CheckBox0701.Checked Then
						CheckBox0701.Checked = True
					Else
						CheckBox0701_CheckedChanged(CheckBox0701, New System.EventArgs())
					End If
				End If

				'    MinOCH = fFinalOCH
				'    CurrhFAF = Round(PtFAF.Z - fRefHeight + 0.4999)
				'    TextBox0701_Validate False

				'??????????????????????????????????????????????????????????
				'ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo

				'=================== Start Log =========Тип схемы=======================
				'LogStr(My.Resources.str681 + MultiPage1.TabPages.Item(6).Text)
				'LogStr(My.Resources.str682)
				'LogStr(My.Resources.str683 + ComboBox0601.Text)
				'LogStr(My.Resources.str684 + TextBox0601.Text + " " + _Label0601_8.Text)
				'LogStr(My.Resources.str685 + TextBox0602.Text + " " + _Label0601_9.Text)
				'LogStr(My.Resources.str686 + TextBox0603.Text + " " + _Label0601_10.Text)

				'If OptionButton0601.Checked Then LogStr(My.Resources.str687 + OptionButton0601.Text)
				'If OptionButton0602.Checked Then LogStr(My.Resources.str687 + OptionButton0602.Text)
				'If OptionButton0603.Checked Then LogStr(My.Resources.str687 + OptionButton0603.Text)
				'If OptionButton0604.Checked Then LogStr(My.Resources.str687 + OptionButton0604.Text)
				'If CheckBox0601.Checked Then LogStr((CheckBox0601.Text))
				'If CheckBox0602.Checked Then LogStr((CheckBox0602.Text))

				''    LogStr "    " + Label1534.Caption
				'LogStr(My.Resources.str688 + TextBox0604.Text + " " + _Label0601_11.Text)
				'LogStr(My.Resources.str689 + TextBox0611.Text + " " + _Label0601_19.Text)
				''    LogStr "    " + Label52.Caption
				''    LogStr "    " + _Label0601_12.Caption
				''    LogStr "    " + CheckBox501.Caption
				'LogStr(My.Resources.str691)
				'LogStr(My.Resources.str692 + TextBox0605.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str693 + TextBox0608.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str694 + TextBox0614.Text)
				'LogStr(My.Resources.str695)
				'LogStr(My.Resources.str696 + TextBox0606.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str697 + TextBox0609.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str698 + TextBox0610.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr(My.Resources.str699 + TextBox0615.Text)
				'LogStr(My.Resources.str700 + TextBox0607.Text)
				'LogStr(My.Resources.str701 + ComboBox0604.Text + " " + HeightConverter(HeightUnit).Unit)
				'=================== End Log ================================
			Case 7
				If CheckBox0701.Checked And (ComboBox0702.SelectedIndex < 0) Then
					CurrPage = MultiPage1.SelectedIndex
					HidePandaBox()
					MessageBox.Show(My.Resources.str00301)
					Return
				End If

				ArrivalProfile.MAPtIndex = ArrivalProfile.PointsNo
				MinOCH = SDFOCH13

				If OptionButton0201.Checked Then
					If CheckBox0701.Checked Then
						ptTmpFAF = PtSDF

						_CurrPDG = 0.01 * CDbl(TextBox0701.Text)
						FinalAreaPDG = _CurrPDG
						MinOCH = SDFOCH
					Else
						ptTmpFAF = PtFAF
					End If
				Else
					ptTmpFAF = PtFAF
					If CheckBox0701.Checked Then
						_CurrPDG = 0.01 * CDbl(TextBox0701.Text)
						FinalAreaPDG = _CurrPDG
						MinOCH = SDFOCH
					End If
				End If

				CurrhFAF = System.Math.Round(ptTmpFAF.Z + 0.4999)
				fMissAprPDG = arMAS_Climb_Nom.Value
				TextBox0804.Text = CStr(100.0 * fMissAprPDG)
				'==================================================== MAPt
				Z = fRefHeight + MinOCH
				A = arFAFTolerance.Value 'MAXnear допуска на MAPt
				TASmin = IAS2TAS(3.6 * cVfafMin.Values(Category), Z, arISAmin.Value)
				TASmax = IAS2TAS(3.6 * cVfafMax.Values(Category), Z, arISAmax.Value)

				d0 = System.Math.Sqrt((A * A + (0.00277777777777778 * TASmin) ^ 2) / (1.0 - (CurrADHP.WindSpeed / TASmin) ^ 2))
				d1 = System.Math.Sqrt((A * A + (0.00277777777777778 * TASmax) ^ 2) / (1.0 - (CurrADHP.WindSpeed / TASmax) ^ 2))

				If d0 < d1 Then d0 = d1

				fhMAPt = MinOCH
				If fhMAPt < arMATurnAlt.Value Then fhMAPt = arMATurnAlt.Value

				dTHR2FAF = Point2LineDistancePrj(ptTmpFAF, FictTHR, _ArDir + 90.0)
				'    fTmp = (CurrhFAF - MinOCH) / FinalAreaPDG

				fTmp = Math.Max(dTHR2FAF - (CurrhFAF - MinOCH) / FinalAreaPDG, 4000.0)
				If fTmp < d0 Then fTmp = d0
				fTmp = dTHR2FAF - fTmp
				'=========================================================
				'    DistTHR2NAV = Point2LineDistancePrj(FinalNav.pPtPrj, FictTHR, ArDir + 90.0) + dTHR2FAF
				DistTHR2NAV = SideDef(FictTHR, _ArDir + 90.0, FinalNav.pPtPrj) * Point2LineDistancePrj(FinalNav.pPtPrj, FictTHR, _ArDir + 90.0) + dTHR2FAF  '???

				B = arFAFTolerance.Value 'MAXfar допуска на MAPt
				X1 = System.Math.Sqrt(B * B + (0.00361111111111111 * TASmin) ^ 2 + (CurrADHP.WindSpeed * dTHR2FAF / TASmin) ^ 2)
				X2 = System.Math.Sqrt(B * B + (0.00361111111111111 * TASmax) ^ 2 + (CurrADHP.WindSpeed * dTHR2FAF / TASmax) ^ 2)
				If X1 > X2 Then fFAF2FARMAPt = X1 Else fFAF2FARMAPt = X2
				fFAF2FARMAPt = fFAF2FARMAPt + dTHR2FAF
				'=========================================================
				If (fFAF2FARMAPt < DistTHR2NAV) Then
					fFAF2FARMAPt = DistTHR2NAV
					fFAF2FARMAPt = fFAF2FARMAPt + CalcMAPtDistD(Z, Category)
				End If
				'=========================================================
				MAPtNavDat = GetValidMAPtNavs(ptTmpFAF, fTmp, fFAF2FARMAPt - CalcMAPtDistD(Z, Category), _ArDir, fhMAPt, FinalNav)

				N = UBound(MAPtNavDat)
				M = UBound(InSectList)

				If N >= 0 Then
					ReDim Preserve MAPtNavDat(M + N + 2)
				Else
					ReDim MAPtNavDat(M + 1)
				End If

				Side = SideDef(FictTHR, _ArDir + 90.0, FinalNav.pPtPrj)

				If _OnAero And (Side > 0) Then
					If fFAF2FARMAPt > DistTHR2NAV Then fFAF2FARMAPt = DistTHR2NAV
				Else
					fFAF2FARMAPt = dTHR2FAF
				End If

				fTmp = Math.Max(dTHR2FAF - (CurrhFAF - MinOCH) / FinalAreaPDG, 4000.0)

				If RWYCollection.PointCount < 3 Then
					pRWYLine = New ESRI.ArcGIS.Geometry.Polyline
					pRWYLine.FromPoint = RWYCollection.Point(0)
					pRWYLine.ToPoint = RWYCollection.Point(1)
					pProxi = pRWYLine
				Else
					pTopo = RWYCollection
					pRWYsPolygon = pTopo.ConvexHull

					pTopo = pRWYsPolygon
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
					pProxi = pRWYsPolygon
				End If

				For I = 0 To M
					fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, FinalNav.pPtPrj, _ArDir)
					d0 = OnNAVShift(InSectList(I).TypeCode, fhMAPt)

					If (InSectList(I).TypeCode <> eNavaidType.LLZ) And (fLinePtDist <= d0) Then
						fDist = pProxi.ReturnDistance(InSectList(I).pPtPrj)
						bOnAero = fDist < arCirclAprShift.Value

						fDist = Point2LineDistancePrj(ptTmpFAF, InSectList(I).pPtPrj, _ArDir - 90.0) * SideDef(ptTmpFAF, _ArDir - 90.0, InSectList(I).pPtPrj) + dTHR2FAF
						'            If (fDist >= fTmp) And (fDist <= fFAF2FARMAPt) Then
						'            If bOnAero And (fDist <= fTmp) Then
						If bOnAero Or ((fDist > 0.0) And (fDist <= fTmp)) Then
							N = N + 1
							MAPtNavDat(N) = InSectList(I)
							MAPtNavDat(N).IntersectionType = eIntersectionType.OnNavaid
							MAPtNavDat(N).ValCnt = -2
						End If
					End If
				Next I
				'=================================================
				_Label0801_6.Text = ""

				MAPtOCH = arFASeg_FAF_MOC.Value

				If (FindOptimalMAPt(MAPtOCH, fFAF2NEARMAPt, d0, I, K, MaxOCH) < 0.0) Or (MaxOCH - 0.5 > MAPtOCH) Then
					If CheckBox0701.Checked Then
						MessageBox.Show(My.Resources.str00527)  '"OCH больше высоты SDF. Увеличьте высоту SDF."
					ElseIf OptionButton0201.Checked Then
						MessageBox.Show(My.Resources.str00500)  '"OCH больше высоты FAF. Увеличьте высоту FAF."
					Else
						MessageBox.Show(My.Resources.str00528)  '"OCH больше высоты SoIL. Увеличьте высоту SoIL."
					End If
				End If

				fFAF2FARMAPt = fFAF2NEARMAPt + d0
				If I >= 0 Then
					TextBox0807.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).UnicalName
				Else
					TextBox0807.Text = ""
				End If

				If K >= 0 Then
					TextBox0808.Text = MAPtObstacles.Obstacles(MAPtObstacles.Parts(K).Owner).UnicalName
				Else
					TextBox0808.Text = ""
				End If
				'===========================================================================
				If N > -1 Then
					ComboBox0801.Enabled = True
					OptionButton0801.Enabled = True
					OptionButton0802.Enabled = True
					ReDim Preserve MAPtNavDat(N)
					ComboBox0801.Items.Clear()
					For I = 0 To N
						ComboBox0801.Items.Add(MAPtNavDat(I).CallSign)
					Next I
					ComboBox0801.SelectedIndex = 0
				Else
					OptionButton0801.Checked = False
					ComboBox0801.Enabled = False
					OptionButton0801.Enabled = False
					OptionButton0802.Enabled = False
					ReDim MAPtNavDat(-1)
				End If
				'===========================================================================
				If ComboBox0803.SelectedIndex < 1 Then
					ComboBox0803.SelectedIndex = 1
				Else
					ComboBox0803_SelectedIndexChanged(ComboBox0803, New System.EventArgs())
				End If

				If OptionButton0201.Checked Or (CheckBox0701.Checked) Then
					If Not OptionButton0802.Checked Then
						OptionButton0802.Checked = True
					Else
						OptionButton0802_CheckedChanged(OptionButton0802, New System.EventArgs())
					End If
					Frame0801.Enabled = True
				Else
					If Not OptionButton0801.Checked Then
						OptionButton0801.Checked = True
					Else
						OptionButton0801_CheckedChanged(OptionButton0801, New System.EventArgs())
					End If
					Frame0801.Enabled = False
				End If

				_TerminationAltitude = fStraightMissedTermHght + fRefHeight
				TextBox0809.Text = CStr(ConvertHeight(_TerminationAltitude, eRoundMode.NEAREST))
				TextBox0809_Validating(TextBox0809, Nothing)

				'TextBox0809.Tag = TextBox0809.Text
				'fTmp = (fStraightMissedTermHght - MAPtOCH) / fMissAprPDG + dMAPt2SOC
				'TextBox0810.Text = ConvertDistance(fTmp).ToString()

				'=================== Start Log ======== КТ снижения ========================
				'LogStr(My.Resources.str711 + MultiPage1.TabPages.Item(7).Text)
				'If CheckBox0701.Checked Then
				'	LogStr(My.Resources.str712)
				'	LogStr(My.Resources.str713 + TextBox0701.Text + " °")

				'	'        If _OptionButton0201_1.Value Then
				'	'            LogStr "    TAS :                        " + TextBox0705.Text + " " + SpeedConverter(SpeedUnit).Unit
				'	'        End If

				'	If ComboBox0701.SelectedIndex = 0 Then
				'		LogStr(My.Resources.str714 + TextBox0702.Text + " " + HeightConverter(HeightUnit).Unit)
				'	Else
				'		LogStr(My.Resources.str715 + TextBox0702.Text + " " + HeightConverter(HeightUnit).Unit)
				'	End If

				'	LogStr(My.Resources.str716 + TextBox0706.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'	LogStr(My.Resources.str717 + ComboBox0702.Text + " " + _Label0701_6.Text)
				'	If _Label0701_6.Text = "DME" Then LogStr("    " + _Label0701_7.Text)
				'	LogStr(My.Resources.str718 + TextBox0707.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'	LogStr(ComboBox0705.Text + TextBox0703.Text + " " + HeightConverter(HeightUnit).Unit)

				'	LogStr(My.Resources.str721 + TextBox0708.Text)
				'End If
				'=================== End Log ================================
			Case 8
				If OptionButton0801.Checked Then
					If ComboBox0801.SelectedIndex < 0 Then
						CurrPage = MultiPage1.SelectedIndex
						HidePandaBox()
						MessageBox.Show(My.Resources.str00301)
						Return
					End If
				End If

				_CurrPDG = fMissAprPDG
				TextBox1501.Text = TextBox0804.Text
				If OptionButton0901.Checked Then
					OptionButton0901_CheckedChanged(OptionButton0901, Nothing)
				ElseIf OptionButton0902.Checked Then
					OptionButton0902_CheckedChanged(OptionButton0902, Nothing)
				Else
					OptionButton0901.Checked = True
				End If
				'=================== Start Log ========= Создание MAPt ======
				'LogStr(My.Resources.str731 + MultiPage1.TabPages.Item(8).Text)
				'If OptionButton0801.Checked Then
				'	LogStr(Frame0801.Text + " " + OptionButton0801.Text)
				'	LogStr(My.Resources.str732 + ComboBox0801.Text + " " + _Label0801_6.Text)
				'	LogStr("    " + _Label0801_5.Text)
				'Else
				'	LogStr(Frame0801.Text + " " + OptionButton0802.Text)
				'End If
				'LogStr("    " + _Label0801_0.Text + TextBox0801.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + _Label0801_1.Text)
				'LogStr("    " + _Label0801_2.Text)
				'LogStr("    " + _Label0801_3.Text)
				'LogStr("    " + _Label0801_4.Text)

				'LogStr("    " + _Label0801_9.Text + "            " + TextBox0804.Text + " %")
				'LogStr("    " + _Label0801_10.Text + "      " + TextBox0805.Text + " " + DistanceConverter(DistanceUnit).Unit)
				''    LogStr "    " + _Label0801_11 + " " + TextBox0806.Text + " " + DistanceConverter(DistanceUnit).Unit
				'LogStr("    " + ComboBox0803.Text + "                                      " + TextBox0802.Text + " " + HeightConverter(HeightUnit).Unit)

				'LogStr("    " + _Label0801_12.Text + "           " + TextBox0807.Text)
				'LogStr("    " + _Label0801_14.Text + " " + TextBox0808.Text)
				'If CheckBox0801.Checked Then LogStr("    " + CheckBox0801.Text)
				'=================== End Log ================================
			Case 9
				fBankAngle = CDbl(TextBox0901.Text)
				_missedIAS = 3.6 * DeConvertSpeed(CDbl(TextBox0902.Text))

				GetDetOCHObstacles()
				CalcTurnRange()
				'OverOCH = DeConvertHeight(CDbl(TextBox1004.Text))
				'If ComboBox1002.SelectedIndex = 0 Then COCH = COCH - fRefHeight

				COCH = OverOCH

				ClosestToMOC50(COCH, _CurrPDG)
				TextBox1004.Text = CStr(ConvertHeight(COCH, eRoundMode.CEIL))
				If ComboBox1002.SelectedIndex = 0 Then TextBox1004.Text = CStr(ConvertHeight(COCH + fRefHeight, eRoundMode.CEIL))

				PtSOC.Z = COCH
				'=================== Start Log =========Условия разворота=======================
				'LogStr(My.Resources.str741 + MultiPage1.TabPages.Item(9).Text)
				'LogStr("    " + _Label0901_0.Text + " " + TextBox0901.Text + " " + _Label0901_5.Text)
				'LogStr("    " + _Label0901_3.Text + " " + ComboBox0901.Text)
				'LogStr("    " + _Label0901_1.Text + " " + TextBox0902.Text + " " + SpeedConverter(SpeedUnit).Unit)
				'LogStr("    " + _Label0901_2.Text)

				'If OptionButton0901.Checked Then
				'	LogStr("    " + Frame0901.Text + " " + OptionButton0901.Text)
				'Else
				'	LogStr("    " + Frame0901.Text + " " + OptionButton0902.Text)
				'End If
				'If CheckBox0901.Checked Then LogStr("    " + CheckBox0901.Text)
				'=================== End Log ====================================================
			Case 10
				CurrOCH = OverOCH
				If OptionButton0902.Checked Then
					TurnInterNavDat = GetValidNavs(pFullPoly, fRefHeight, hMinTurn, hMaxTurn, CurrOCH, _ArDir, _CurrPDG, PtSOC, pFIXAreaPolygon, FinalNav.TypeCode, FinalNav.pPtPrj)
					ComboBox1102.Items.Clear()
					'========================================================== ??????????????????????
					d0 = (hMinTurn - CurrOCH) / _CurrPDG + dMAPt2SOC
					d1 = (hMaxTurn - CurrOCH) / _CurrPDG + dMAPt2SOC
					M = UBound(TurnInterNavDat)
					N = UBound(InSectList)

					If N >= 0 Then
						If M >= 0 Then
							ReDim Preserve TurnInterNavDat(M + N + 2)
						Else
							ReDim TurnInterNavDat(N + 1)
						End If
					Else
						If M >= 0 Then
							ReDim Preserve TurnInterNavDat(M + 1)
						Else
							ReDim TurnInterNavDat(0)
						End If
					End If

					'If _OptionButton0201_0.Value Then
					If OptionButton0801.Checked Then
						NavIndex = ComboBox0801.SelectedIndex
						MAPtFlg = MAPtNavDat(NavIndex).IntersectionType = eIntersectionType.OnNavaid
					End If
					'Else
					'	NavIndex = 0
					'	MAPtFlg = True
					'End If

					If MAPtFlg Then
						M = M + 1
						TurnInterNavDat(M) = MAPtNavDat(NavIndex)
					End If

					For I = 0 To N
						'===============================================
						If MAPtFlg Then
							If MAPtNavDat(NavIndex).Identifier = InSectList(I).Identifier Then Continue For
						End If

						fLinePtDist = Point2LineDistancePrj(InSectList(I).pPtPrj, pMAPt, _ArDir)
						fDist = Point2LineDistancePrj(InSectList(I).pPtPrj, pMAPt, _ArDir + 90.0) * SideDef(InSectList(I).pPtPrj, _ArDir - 90.0, pMAPt)
						fHrel = Point2LineDistancePrj(PtSOC, InSectList(I).pPtPrj, _ArDir + 90.0) * SideDef(PtSOC, _ArDir + 90.0, InSectList(I).pPtPrj)
						If fHrel < 0.0 Then fHrel = 0.0

						fHrel = fRefHeight + CurrOCH + fHrel * _CurrPDG - InSectList(I).pPtGeo.Z
						If fHrel < arMATurnAlt.Value Then fHrel = arMATurnAlt.Value
						'fHrel = fRefHeight + CurrOCH - InSectList(I).pPtGeo.Z
						'If fHrel < arMATurnAlt.Value Then fHrel = arMATurnAlt.Value
						fTmp = OnNAVShift(InSectList(I).TypeCode, fHrel)

						'If InSectList(I).Type = 0 Then
						'	fTmp = VOR.OnNAVRadius
						'ElseIf InSectList(I).Type = 2 Then
						'	fTmp = NDB.OnNAVRadius
						'End If

						If (fDist > d0) And (fDist < d1) And (fLinePtDist <= fTmp) Then
							If InSectList(I).TypeCode = eNavaidType.NDB Then
								fConRad = System.Math.Tan(DegToRad(NDB.ConeAngle)) * fHrel
							Else
								fConRad = System.Math.Tan(DegToRad(VOR.ConeAngle)) * fHrel
							End If

							If d1 - fDist >= fConRad Then
								M = M + 1
								TurnInterNavDat(M) = InSectList(I)
								TurnInterNavDat(M).IntersectionType = eIntersectionType.OnNavaid
								TurnInterNavDat(M).ValCnt = -2
							End If
						End If
					Next I

					If M >= 0 Then
						ReDim Preserve TurnInterNavDat(M)
					Else
						ReDim TurnInterNavDat(-1)
						CurrPage = MultiPage1.SelectedIndex
						HidePandaBox()
						MessageBox.Show(My.Resources.str00304)  '"Нет подходящего средства для создания FIX"
						Return
					End If
					'========================================================== ??????????????????????
					For I = 0 To M
						ComboBox1102.Items.Add(TurnInterNavDat(I).CallSign)
					Next I
					ComboBox1102.SelectedIndex = 0
					ToolTip1.SetToolTip(TextBox1101, "")
					Frame1102.Visible = True
					Frame1101.Visible = False
				Else
					'=====
					If ComboBox1101.SelectedIndex = 0 Then
						fTmpH1 = DeConvertHeight(CDbl(TextBox1005.Text))
						fTmpH2 = DeConvertHeight(CDbl(TextBox1006.Text))
						TextBox1101.Text = CStr(ConvertHeight(fTmpH1 + fRefHeight, eRoundMode.NEAREST))
						_Label1101_2.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(fTmpH1 + fRefHeight, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(fTmpH2 + fRefHeight, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
					Else
						TextBox1101.Text = TextBox1005.Text
						_Label1101_2.Text = My.Resources.str00220 + My.Resources.str00221 + CStr(ConvertHeight(fTmpH1, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit + My.Resources.str00222 + CStr(ConvertHeight(fTmpH2, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
					End If

					ToolTip1.SetToolTip(TextBox1101, _Label1101_2.Text)
					ToolTip1.SetToolTip(TextBox1102, "")
					Frame1102.Visible = False
					Frame1101.Visible = True
					TextBox1101.Tag = ""
					TextBox1101_Validating(TextBox1101, New System.ComponentModel.CancelEventArgs())
				End If

				_Label1101_11.Text = My.Resources.str12005 + CStr(arT_TechToleranc.Value) + " " + My.Resources.str00015 + " :"
				'=================== Start Log =========Выбор OCH=======================
				'LogStr(My.Resources.str751 + MultiPage1.TabPages.Item(10).Text)
				'LogStr("    " + Label1001_0.Text + "               " + TextBox1001.Text + " " + _Label1001_11.Text)
				'LogStr("    " + _Label1001_6.Text)
				'LogStr("    " + Label1001_0.Text + " " + TextBox1002.Text + " " + _Label1001_12.Text)
				'LogStr("    " + _Label1001_7.Text + "           " + TextBox1007.Text)
				'LogStr("    " + ComboBox1001.Text + " " + TextBox1003.Text + " " + _Label1001_13.Text)
				'LogStr("    " + _Label1001_8.Text + "  " + TextBox1008.Text)
				'LogStr("    " + ComboBox1002.Text + "                " + TextBox1004.Text + " " + _Label1001_14.Text)
				'LogStr("    " + _Label1001_4.Text + "    " + TextBox1005.Text + " " + _Label1001_15.Text)
				'LogStr("    " + _Label1001_5.Text + " " + TextBox1006.Text + " " + _Label1001_16.Text)
				'LogStr("    " + _Label1001_9.Text + "  " + TextBox1009.Text + " " + _Label1001_17.Text)
				'LogStr("    " + _Label1001_10.Text + " " + TextBox1010.Text + " " + _Label1001_18.Text)
				'=================== End Log ============================================
			Case 11
				pLine1 = New ESRI.ArcGIS.Geometry.Polyline
				If _bTurnFIXSameMAPtFIX Then
					pLine1.FromPoint = PointAlongPlane(KK.FromPoint, _ArDir - 90.0, 50000.0)
					pLine1.ToPoint = PointAlongPlane(KK.ToPoint, _ArDir + 90.0, 50000.0)
				Else
					pLine1.FromPoint = PointAlongPlane(pMAPt, _ArDir - 90.0, 50000.0)
					pLine1.ToPoint = PointAlongPlane(pMAPt, _ArDir + 90.0, 50000.0)
				End If
				pClone = pFullPoly
				pFullPolyCopy = pClone.Clone

				ClipByLine(pFullPoly, pLine1, Nothing, pFullPoly, Nothing)

				If OptionButton0902.Checked Then
					'	ComboBox1102_Click
				Else
					'	TextBox1101_Validate True
					If CheckBox0902.Checked Then
						fFinalMOC = arMA_FinalMOC.Value
					Else
						fFinalMOC = arMA_InterMOC.Value
					End If
				End If

				N = UBound(MAPtObstacles.Parts)
				M = UBound(MAPtObstacles.Obstacles)

				For I = 0 To M
					MAPtObstacles.Obstacles(I).NIx = -1
				Next

				If N >= 0 Then
					ReDim tmpObstacleList.Parts(N)
					ReDim tmpObstacleList.Obstacles(M)
				Else
					ReDim tmpObstacleList.Parts(-1)
					ReDim tmpObstacleList.Obstacles(-1)
				End If

				Dz = Point2LineDistancePrj(pMAPt, KK.FromPoint, _ArDir + 90.0)
				K = -1
				L = -1
				J = 0
				For I = 0 To N
					If MAPtObstacles.Parts(I).Dist > Dz Then Exit For

					K = K + 1
					tmpObstacleList.Parts(K) = MAPtObstacles.Parts(I)

					If MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx < 0 Then
						L += 1
						tmpObstacleList.Obstacles(L) = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner)
						tmpObstacleList.Obstacles(L).PartsNum = 0
						ReDim tmpObstacleList.Obstacles(L).Parts(MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).PartsNum - 1)
						MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx = L
					End If

					tmpObstacleList.Parts(K).Owner = MAPtObstacles.Obstacles(MAPtObstacles.Parts(I).Owner).NIx
					tmpObstacleList.Obstacles(tmpObstacleList.Parts(K).Owner).Parts(tmpObstacleList.Obstacles(tmpObstacleList.Parts(K).Owner).PartsNum) = K
					tmpObstacleList.Obstacles(tmpObstacleList.Parts(K).Owner).PartsNum += 1

					fTmp = (tmpObstacleList.Parts(K).Dist - dMAPt2SOC) * _CurrPDG
					tmpObstacleList.Parts(K).ReqOCH = tmpObstacleList.Parts(K).ReqH - fTmp
					tmpObstacleList.Parts(K).hPenet = CurrOCH + fTmp    'H over Obst
					If CheckBox0902.Checked Then
						tmpObstacleList.Parts(K).Rmax = tmpObstacleList.Parts(K).Height + fFinalMOC
						tmpObstacleList.Parts(K).Rmin = fFinalMOC
					End If

					tmpObstacleList.Parts(K).Flags = tmpObstacleList.Parts(K).Flags And 1
					If tmpObstacleList.Parts(K).Dist > dMAPt2SOC + fMAInterRange Then
						tmpObstacleList.Parts(K).Flags = tmpObstacleList.Parts(K).Flags + 4
					ElseIf tmpObstacleList.Parts(K).Dist > dMAPt2SOC Then
						tmpObstacleList.Parts(K).Flags = tmpObstacleList.Parts(K).Flags + 2
					Else
						If (tmpObstacleList.Parts(K).ReqH < tmpObstacleList.Parts(K).hPenet) And (tmpObstacleList.Parts(K).Height + _FinalMOC * tmpObstacleList.Parts(K).fTmp > CurrOCH) Then
							tmpObstacleList.Parts(K).Flags = tmpObstacleList.Parts(K).Flags Or 8
							'            ElseIf tmpObstacleList(K).Height + FinalMOC * tmpObstacleList(K).fTmp > CurrOCH Then
							'                tmpObstacleList(K).Flags = tmpObstacleList(K).Flags Or 16
						End If
					End If
					tmpObstacleList.Parts(K).Dist = tmpObstacleList.Parts(K).Dist - dMAPt2SOC
				Next I

				If K >= 0 Then
					ReDim Preserve tmpObstacleList.Parts(K)
					ReDim Preserve tmpObstacleList.Obstacles(L)

					NonPrecReportFrm.FillPage07(tmpObstacleList, _FinalMOC, _CurrPDG)
				End If

				'=======================================================
				hTMA = CalcHTMA(ObstacleList, TurnFixPnt, DeConvertHeight(CDbl(ComboBox0902.Text)), fRefHeight, fMissAprPDG)

				_CheckState = True
				If OptionButton1201.Checked Then OptionButton1201_CheckedChanged(OptionButton1201, New System.EventArgs())
				If OptionButton1202.Checked Then OptionButton1202_CheckedChanged(OptionButton1202, New System.EventArgs())
				If OptionButton1203.Checked Then OptionButton1203_CheckedChanged(OptionButton1203, New System.EventArgs())
				'=================== Start Log =========ТР и параметры разворота ======================
				'LogStr(My.Resources.str761 + MultiPage1.TabPages.Item(11).Text)
				''    LogStr "    Разворот в заданной ТР"

				'If Frame1102.Enabled = True Then
				'	LogStr(My.Resources.str762)
				'	LogStr(My.Resources.str763 + ComboBox1102.Text + " " + _Label1101_3.Text)
				'	LogStr("    " + _Label1101_1.Text + " " + TextBox1102.Text + " " + _Label1101_13.Text)
				'	If OptionButton1101.Checked Then
				'		LogStr("    " + OptionButton1101.Text)
				'	Else
				'		LogStr("    " + OptionButton1102.Text)
				'	End If
				'Else
				'	LogStr(My.Resources.str764)
				'End If

				'LogStr("    " + ComboBox1101.Text + ":        " + TextBox1101.Text + " " + _Label1101_4.Text)
				'LogStr("    " + _Label1101_2.Text)
				''    LogStr "    " + padr(_Label1101_5.Caption, 25) + TextBox1103.Text
				''    LogStr "    " + padr(_Label1101_6.Caption, 25) + TextBox1104.Text
				''    LogStr "    " + padr(_Label1101_7.Caption, 25) + TextBox1105.Text
				''    LogStr "    " + padr(_Label1101_8.Caption, 25) + TextBox1106.Text
				''    LogStr "    " + padr(_Label1101_9.Caption, 25) + TextBox1107.Text
				''    LogStr "    " + padr(_Label1101_10.Caption, 25) + TextBox1108.Text
				''    LogStr "    " + padr(_Label1101_12.Caption, 25) + TextBox1110.Text
				''    LogStr "    " + padr(_Label1101_11.Caption, 25) + TextBox1109.Text
				'LogStr("    " + _Label1101_5.Text + TextBox1103.Text + " " + SpeedConverter(SpeedUnit).Unit)
				'LogStr("    " + _Label1101_6.Text + TextBox1104.Text + " " + SpeedConverter(SpeedUnit).Unit)
				'LogStr("    " + _Label1101_7.Text + TextBox1105.Text + " " + My.Resources.str14)
				'LogStr("    " + _Label1101_8.Text + TextBox1106.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + _Label1101_9.Text + TextBox1107.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + _Label1101_10.Text + TextBox1108.Text + " " + HeightConverter(HeightUnit).Unit)
				'LogStr("    " + _Label1101_12.Text + TextBox1110.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'LogStr("    " + _Label1101_11.Text + TextBox1109.Text + " " + DistanceConverter(DistanceUnit).Unit)
				'=================== End Log ==========================================
			Case 12
				If Not (OptionButton1203.Checked Or OptionButton1202.Checked Or OptionButton1201.Checked) Then
					HidePandaBox()
					MessageBox.Show(My.Resources.str00503)  '"Выберите один из вариантов."
					Return
				End If

				If (OptionButton1202.Checked Or OptionButton1201.Checked) And Not IsNumeric(TextBox1203.Text) Then
					HidePandaBox()
					TextBox1203.Focus()
					MessageBox.Show(_Label1201_5.Text + My.Resources.str00302)  '"Поле 'Курс следующего сегмента' заполнено некорректно."
					Return
				End If

				If OptionButton1202.Checked Then
					If Not IsNumeric(TextBox1204.Text) Then
						HidePandaBox()
						TextBox1204.Focus()
						MessageBox.Show(_Label1201_6.Text + My.Resources.str00302)  '"Поле 'Угол сопряжения' заполнено некорректно."
						Return
					End If

					If Not IsNumeric(TextBox1205.Text) Then
						HidePandaBox()
						TextBox1205.Focus()
						MessageBox.Show(_Label1201_11.Text + My.Resources.str00302) '"Поле 'Время стабилизации' заполнено некорректно."
						Return
					End If
				End If

				'    If (Not bTurnFIXSameMAPtFIX) And (OptionButton1201.Value Or TurnWPT.TypeCode < 0) Then
				If OptionButton1201.Checked Or (TurnDirector.TypeCode <= eNavaidType.NONE) Then
					CalcTAParams()
					CurrPage = CurrPage + 2
					'NonPrecReportFrm.FillPage08(ObstacleFinalMOCList, IxDh)
				Else
					_CheckState = False
					ApplayOptions()
				End If
				'=================== Start Log ========= Направление ухода ======================
				'LogStr(My.Resources.str771 + MultiPage1.TabPages.Item(12).Text)
				'If OptionButton1201.Checked Then
				'	LogStr("    " + OptionButton1201.Text)
				'Else
				'	LogStr("    " + OptionButton1203.Text)
				'End If

				'If OptionButton1203.Checked Then
				'	LogStr("    " + _Label1201_1.Text + " " + ComboBox1201.Text + " " + _Label1201_3.Text)
				'	If CheckBox1201.Checked Then
				'		LogStr("    " + CheckBox1201.Text)
				'	End If
				'End If
				'LogStr(My.Resources.str772 + TextBox1203.Text)
				'LogStr(My.Resources.str773 + TextBox1201.Text)
				'=================== End Log ==========================================
			Case 13
				CheckBox1403.Enabled = OptionButton1303.Checked And OptionButton1303.Enabled
				If Not CheckBox1403.Enabled Then CheckBox1403.Checked = False

				CheckBox1406.Enabled = OptionButton1306.Checked And OptionButton1306.Enabled
				If Not CheckBox1406.Enabled Then CheckBox1406.Checked = False
				SecondArea(TurnDir, BaseArea)
				'=================== Start Log =========Стыковка зон ======================
				'LogStr(My.Resources.str781 + MultiPage1.TabPages.Item(13).Text)
				'If Frame1301.Enabled Then
				'	LogStr("    " + Frame1301.Text)
				'	LogStr(My.Resources.str782)
				'	If OptionButton1301.Checked Then	LogStr("    " + OptionButton1301.Text)
				'	If OptionButton1302.Checked Then	LogStr("    " + OptionButton1302.Text)
				'	If OptionButton1303.Checked Then	LogStr("    " + OptionButton1303.Text)
				'End If

				'If Frame1302.Enabled Then
				'	LogStr("    " + Frame1302.Text)
				'	LogStr(My.Resources.str783 + _TextBox1301_1.Text)
				'	If OptionButton1304.Checked Then	LogStr("    " + OptionButton1304.Text)
				'	If OptionButton1305.Checked Then	LogStr("    " + OptionButton1305.Text)
				'	If OptionButton1306.Checked Then	LogStr("    " + OptionButton1306.Text)
				'End If
				'=================== End Log ==========================================
			Case 14
				'    TextBox1501.Text = CStr(100# * fMissAprPDG)
				If Not ((CheckBox1401.Checked) Or (CheckBox1402.Checked) Or (CheckBox1403.Checked)) Then
					HidePandaBox()
					MessageBox.Show(My.Resources.str00503)  '"Выберите один из вариантов."
					Return
				End If

				If Not ((CheckBox1404.Checked) Or (CheckBox1405.Checked) Or (CheckBox1406.Checked)) Then
					HidePandaBox()
					MessageBox.Show(My.Resources.str00503)  '"Выберите один из вариантов."
					Return
				End If

				'=======================================================
				Frame1501.Visible = OptionButton1202.Checked
				'fEnrouteMOC = DeConvertHeight(CDbl(ComboBox0902.Text))

				CalcTAParams()
				'NonPrecReportFrm.FillPage08(ObstacleFinalMOCList, IxDh)
				'=================== Start Log =========Дополнительная зона ======================
				'LogStr(My.Resources.str791 + MultiPage1.TabPages.Item(14).Text)
				''    If _Frame1401_0.Enabled = True Then
				'LogStr("    " + Frame1401.Text)
				''    End If

				'If CheckBox1401.Checked Then LogStr("    " + CheckBox1401.Text)
				'If CheckBox1402.Checked Then LogStr("    " + CheckBox1402.Text)
				'If CheckBox1403.Checked Then LogStr("    " + CheckBox1403.Text)
				'If Frame1402.Enabled = True Then LogStr("    " + Frame1402.Text)
				'If CheckBox1404.Checked Then LogStr("    " + CheckBox1404.Text)
				'If CheckBox1405.Checked Then LogStr("    " + CheckBox1405.Text)
				'If CheckBox1406.Checked Then LogStr("    " + CheckBox1406.Text)
				'=================== End Log ==========================================
			Case 15
				segPDG = fMissAprPDG
				Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline

				If OptionButton1202.Checked Then
					pPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
					CType(pPoly, IPointCollection).AddPoint(TerFixPnt)
				Else
					TerFixPnt = mPoly.ToPoint
					TerFixPnt.M = _OutDir
					'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
					If OptionButton0401.Checked Then
						pProxi = ZNR_Poly
					Else
						pProxi = KK
					End If

					Dim fDis As Double
					Dim hKK As Double

					fDis = pProxi.ReturnDistance(TerFixPnt)
					hKK = DeConvertHeight(CDbl(TextBox1502.Text))
					TerFixPnt.Z = hKK + fDis * fMissAprPDG
					'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
					'TerFixPnt.Z = DeConvertHeight(CDbl(TextBox1510.Text))
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
				Trace(0) = AddSegmentFrm.CreateInitialSegment(TerFixPnt, pPoly, BaseArea, _OutDir, fMissAprPDG, TurnFixPnt.Z)

				If OptionButton1202.Checked Then
					Trace(0).GuidanceNav = WPT_FIXToNavaid(TurnDirector)
					Trace(0).InterceptionNav = TerInterNavDat(ComboBox1502.SelectedIndex)
				End If

				TSC = 1

				RemoveSegmentBtn.Enabled = False

				ReDrawTrace()
				ReListTrace()
		End Select

		HidePandaBox()
		screenCapture.Save(Me)
		MultiPage1.SelectedIndex = CurrPage
		'NextBtn.Enabled = (CurrPage < MultiPage1.Tabs - 1) And ((CurrPage <> 2) Or (OnAero))
		If CurrPage <> 7 Then
			NextBtn.Enabled = (CurrPage < MultiPage1.TabPages.Count() - 1) And ((CurrPage <> 1) Or (UBound(Solutions) >= 0))
		End If

		OkBtn.Enabled = (CurrPage = MultiPage1.TabPages.Count() - 1) And OkBtnEnabled
		'If CurrPage = MultiPage1.Tabs - 1 Then OkBtn.Enabled = OkBtnEnabled

		PrevBtn.Enabled = MultiPage1.SelectedIndex > 0
		Me.HelpContextID = 8000 + 100 * (MultiPage1.SelectedIndex + 1)
		Me.Activate()
		' 2007
		FocusStepCaption((MultiPage1.SelectedIndex))

		Me.Visible = False
		Me.Show(_Win32Window)
	End Sub

	Private Sub PrevBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PrevBtn.Click

		CurrPage = MultiPage1.SelectedIndex - 1
		screenCapture.Delete()
		Select Case MultiPage1.SelectedIndex
			'Case 0

			Case 1
				SafeDeleteElement(CLElement)
				CLElement = Nothing

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				fRefHeight = CurrADHP.pPtGeo.Z
				_Label0101_8.Text = My.Resources.str10212 + " ref. elev = " + CStr(ConvertHeight(fRefHeight, eRoundMode.NEAREST)) + HeightConverter(HeightUnit).Unit  '"OCH определяется относительно превышения аэродрома"
			Case 2
				SafeDeleteElement(FASegmElement)
				SafeDeleteElement(LeftPolyElement)
				SafeDeleteElement(RightPolyElement)
				SafeDeleteElement(PrimePolyElement)
				'SafeDeleteElement(pSubtrahendElem)

				For Each elem As ArcGIS.Carto.IElement In pSubtrahendElem
					SafeDeleteElement(elem)
				Next elem
				pSubtrahendElem.Clear()

				SafeDeleteElement(VisualAreaElement)

				VisualAreaElement = DrawPolygon(ConvexPoly, RGB(0, 0, 255), , False)
				pGraphics.AddElement(VisualAreaElement, 0)
				VisualAreaElement.Locked = True

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				FASegmElement = Nothing
				LeftPolyElement = Nothing
				RightPolyElement = Nothing
				PrimePolyElement = Nothing
				'SpinButton0201.Enabled = False
				ProfileBtn.Checked = False
				ProfileBtn.Enabled = False
			Case 3
				SafeDeleteElement(IntermediateBaseAreaElem)
				SafeDeleteElement(IntermediateSecAreaElem)
				SafeDeleteElement(IFPolyElement)
				SafeDeleteElement(FAFFIXElem)
				SafeDeleteElement(FinalBaseAreaElem)
				SafeDeleteElement(FinalSecAreaElem)

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				IntermediateBaseAreaElem = Nothing
				IntermediateSecAreaElem = Nothing
				IFPolyElement = Nothing
				FAFFIXElem = Nothing
				FinalBaseAreaElem = Nothing
				FinalSecAreaElem = Nothing
			Case 4
				SafeDeleteElement(IFFIXElem)
				'GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				IFFIXElem = Nothing
				'pMAPt = Nothing
				TextBox0301_Validating(TextBox0301, New System.ComponentModel.CancelEventArgs())
			Case 5
				'pMAPt = Nothing
				CurrPage = CurrPage - 2
				'    If _OptionButton0201_1.Value Then ' Без FAF
				'    Else
				'    End If
			Case 6
				SafeDeleteElement(FAFFIXElem)
				SafeDeleteElement(FinalBaseAreaElem)
				SafeDeleteElement(FinalSecAreaElem)
				SafeDeleteElement(FarFAFElem)
				SafeDeleteElement(IAreaElement)
				SafeDeleteElement(IIAreaElement)
				SafeDeleteElement(NavAreaElement)
				SafeDeleteElement(TracElement)
				'SafeDeleteElement(FAFAreaElem)

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				ArrivalProfile.ClearPoints()
			Case 7
				If OptionButton0202.Checked Then    ' Без FAF
					SafeDeleteElement(FAFFIXElem)
					SafeDeleteElement(FAFAreaElem)

					Dim fDist As Double
					Dim Hhold As Double
					Dim fTmp As Double

					If OptionButton0101.Checked Then
						If OptionButton0201.Checked Then
							fMinOCH = arFASeg_FAF_MOC.Value
						Else
							fMinOCH = arFASegmentMOC.Value
						End If

						If fAlignOCH > fMinOCH Then fMinOCH = fAlignOCH
					Else
						fMinOCH = fNewVisAprOCH
					End If

					CreateFinalBaseArea()
					NonPrecReportFrm.FillPage03(NavObstacleList, NavIx, NavJx)
					PtFAF.Z = FarFAF.Z - fRefHeight

					If ComboBox0501.SelectedIndex = 0 Then
						Hhold = DeConvertHeight(CDbl(TextBox0601.Text)) - fRefHeight
					Else
						Hhold = DeConvertHeight(CDbl(TextBox0601.Text))
					End If

					fDist = Point2LineDistancePrj(FictTHR, FarFAF, _ArDir + 90)

					ArrivalProfile.ClearPoints()

					fTmp = Point2LineDistancePrj(FictTHR, FinalNav.pPtPrj, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90, FinalNav.pPtPrj)
					ArrivalProfile.AddPoint(fTmp, Hhold, CDbl(TextBox0612.Text), 0, -1)
					ArrivalProfile.AddPoint(fDist, PtFAF.Z, CDbl(TextBox0612.Text), 0, -1)

					'fTAS = IAS2TAS(3.6 * cVfafMax.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
					'fTmp = 0.06 * fDesV / fTAS

					CPDGmin = 3.6 * fDesV / IAS2TAS(3.6 * cVfafMin.Values(Category), FarFAF.Z, CurrADHP.ISAtC)
					If CPDGmin > arFADescent_Nom.Value Then CPDGmin = arFADescent_Nom.Value

					fDist = (PtFAF.Z - fhTHR) / CPDGmin
					ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), -CPDGmin, CodeProcedureDistance.FAF)

					fDist = (fFinalOCH - fhTHR) / CPDGmin
					ArrivalProfile.AddPoint(fDist, fFinalOCH, Modulus(ArAzt - FinalNav.MagVar, 360.0), -CPDGmin, -1)
				Else
					SafeDeleteElement(PtSDFElem)
					SafeDeleteElement(SDFElem)
				End If

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

				If Not OptionButton0202.Checked Then    ' Без FAF
					CurrPage = CurrPage - 2
				End If
			Case 8
				SafeDeleteElement(MAPtElem)
				SafeDeleteElement(SOCElem)
				SafeDeleteElement(MAPtAreaElem)
				SafeDeleteElement(MAPtFixElem)
				SafeDeleteElement(MAPtCLineElem)
				SafeDeleteElement(FinalBaseAreaElem)
				SafeDeleteElement(FinalSecAreaElem)
				SafeDeleteElement(FullPolyElement)
				SafeDeleteElement(PrimeMAPolyElement)

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				MAPtElem = Nothing
				SOCElem = Nothing
				MAPtAreaElem = Nothing
				MAPtFixElem = Nothing
				FinalBaseAreaElem = Nothing
				FinalSecAreaElem = Nothing
				FullPolyElement = Nothing
				PrimeMAPolyElement = Nothing
				ArrivalProfile.RemovePoint()
		'Case 9
		'Case 10
		'Case 11

			Case 12
				pFullPoly = pFullPolyCopy
				'    Set pFullPolyCopy = pClone.Clone
				SafeDeleteElement(KKElem)
				SafeDeleteElement(K1K1Elem)
				SafeDeleteElement(TurnAreaElem)
				SafeDeleteElement(PrimElem)
				SafeDeleteElement(SecRElem)
				SafeDeleteElement(SecLElem)
				SafeDeleteElement(Sec0Elem)
				SafeDeleteElement(NomTrackElem)

				GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
				KKElem = Nothing
				K1K1Elem = Nothing
				TurnAreaElem = Nothing
				PrimElem = Nothing
				SecRElem = Nothing
				SecLElem = Nothing
				Sec0Elem = Nothing
				NomTrackElem = Nothing
			Case 13
				_CheckState = True
				'Case 14
			Case 15
				Dim I As Integer
				Dim pGroupElement As ESRI.ArcGIS.Carto.GroupElement

				If Not TerminationFIXElem Is Nothing Then
					pGroupElement = TerminationFIXElem
					For I = 0 To pGroupElement.ElementCount - 1
						Try
							If Not pGroupElement.Element(I) Is Nothing Then pGraphics.DeleteElement(pGroupElement.Element(I))
						Catch ex As Exception
						End Try

					Next I
				End If

				If OptionButton1201.Checked Or (TurnDirector.TypeCode <= eNavaidType.NONE) Then
					CurrPage = CurrPage - 2
				End If
		End Select

		OkBtn.Enabled = False
		If CurrPage = 8 Then OkBtn.Enabled = CheckBox0801.Checked

		MultiPage1.SelectedIndex = CurrPage
		NextBtn.Enabled = MultiPage1.SelectedIndex < MultiPage1.TabPages.Count() - 1
		PrevBtn.Enabled = MultiPage1.SelectedIndex > 0
		Me.HelpContextID = 8000 + 100 * (MultiPage1.SelectedIndex + 1)
		Me.Activate()

		' 2007
		FocusStepCaption((MultiPage1.SelectedIndex))
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

	'Private Sub TextBox0611_Validate(KeepFocus As Boolean)
	'Dim fH As Double
	'Dim fTmp As Double
	'Dim fMaxH As Double
	'Dim fMinH As Double
	'Dim fTime As Double
	'    If IsNumeric(TextBox0611.Text) Then
	'        fTmp = CDbl(TextBox0611.Text)
	'        fTime = CDbl(TextBox0602.Text)
	'        fMaxH = CDbl(TextBox14.Text)
	'        fMinH = CDbl(TextBox0608.Text)
	'        fH = fTmp * fTime
	'        fTmp = fMinH - fH
	'        If fTmp < fMaxH Then fTmp = fMaxH
	'        TextBox13.Text = CStr(Round(fTmp + 0.4999))
	'        fFinalOCH = fTmp
	'    End If
	'End Sub

	'Private Sub TextBox0604_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles TextBox0604.Validating
	'	Return

	'	If Not IsNumeric(TextBox0604.Text) Then Return
	'	Dim Val_Renamed As Double
	'	Dim fTmp As Double
	'	Val_Renamed = DeConvertDSpeed(CDbl(TextBox0604.Text))
	'	fTmp = arMaxOutBoundDes.Values(ComboBox0001.SelectedIndex) / arMaxOutBoundDes.Multiplier
	'	If Val_Renamed > fTmp Then
	'		Val_Renamed = fTmp
	'		TextBox0604.Text = CStr(ConvertDSpeed(fTmp, 2))
	'	End If
	'	CalcNoFAF(CDbl(TextBox0602.Text))
	'End Sub

	'Private Sub TextBox0004_Validate(Cancel As Boolean)
	'    If (IsNumeric(TextBox0004.Text)) And (TextBox0004.Tag = "") Then
	'        VsManev CDbl(TextBox0004.Text)
	'    End If
	'End Sub

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
		If (Rv > 3.0) Then Rv = 3.0

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
			If OptionButton1203.Checked And (CheckBox1201.Checked) Then
				ThetaTouch = FixToTouchSpiral(BasePoints.Point(I), coef, TurnR, iTurnDir, AztOut, TurnDirector.pPtPrj, _ArDir)
				If ThetaTouch < -370.0 Then
					MsgBox(My.Resources.str00303, MsgBoxStyle.Critical) '"С заданными параметрами разворота выйти в указанную точку невозможно.", vbCritical
					'                ShowMessage "Средство находиться слишком не там", vbCritical, "PANDA"
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

			If TurnAng < 1.0 Then
				CreateTurnArea.AddPoint(BasePoints.Point(0))
				Exit Function
			End If

			If SideFrom2Angle(AztNext, azt12) * TurnDir < 0 Then
				dAng0 = Modulus((AztCur - azt12) * iTurnDir, 360.0)
				dAng = dAng - dAng0
				AztNext = azt12

				CreateWindSpiral(BasePoints.Point(I), AztEnter, azt12 - 90.0 * TurnDir, azt12, TurnR, coef, iTurnDir, TmpSpiral)

				'DrawPoint BasePoints.Point(i), 0
				PtIntersect = New ESRI.ArcGIS.Geometry.Point
				Constructor = PtIntersect

				Constructor.ConstructAngleIntersection(CreateTurnArea.Point(CreateTurnArea.PointCount - 1), DegToRad(AztCur), TmpSpiral.Point(TmpSpiral.PointCount - 1), DegToRad(azt12))

				CreateTurnArea.AddPoint(PtIntersect)
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
			'DrawPolygon CreateTurnArea, 0

			I = (I + 1) Mod N
			AztCur = AztNext
			K = K + 1
		Loop While SubtractAngles(AztNext, wAztOut) > degEps

		If OptionButton1203.Checked And (CheckBox1201.Checked) Then AztOut = wAztOut
	End Function

	Private Function CalcAreaIntersection(ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutAzt As Double, ByRef DirToNav As Double, ByRef iTurnDir As Integer, ByRef iTurnDir2 As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim RightPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim LeftPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim OutLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim InLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim PtInter15 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtInterP As ESRI.ArcGIS.Geometry.IPoint
		Dim PtInterE As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
		Dim ptIn As ESRI.ArcGIS.Geometry.IPoint

		Dim SplayAngle As Double
		Dim ExtAngle As Double
		Dim Dir2Int As Double
		Dim OutDir As Double
		Dim InDir As Double
		Dim Dist1 As Double
		Dim Dist2 As Double

		Dim SideE As Integer
		Dim SideP As Integer
		Dim Side As Integer
		Dim I As Integer

		CalcAreaIntersection = New ESRI.ArcGIS.Geometry.Polygon
		CalcAreaIntersection.AddPointCollection(TurnArea)

		LeftPolys = New ESRI.ArcGIS.Geometry.Polyline
		RightPolys = New ESRI.ArcGIS.Geometry.Polyline

		OutLine = New ESRI.ArcGIS.Geometry.Polyline
		InLine = New ESRI.ArcGIS.Geometry.Polyline

		PtInterE = New ESRI.ArcGIS.Geometry.Point
		PtInterP = New ESRI.ArcGIS.Geometry.Point

		TextBox1301_0.Text = ""
		TextBox1301_1.Text = ""

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

		pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon

		If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
			SplayAngle = VOR.SplayAngle
		ElseIf TurnDirector.TypeCode = eNavaidType.NDB Then
			SplayAngle = NDB.SplayAngle
		Else
			SplayAngle = 0.0
		End If

		If iTurnDir > 0 Then
			pTmpPoly.AddPoint(ptIn)
			pTmpPoly.AddPoint(ptOut)

			pTopoOper = LeftPolys
			pPoints = pTopoOper.Intersect(InLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)

			If pPoints.PointCount > 0 Then
				If _CheckState Then
					OptionButton1301.Enabled = False
					OptionButton1302.Enabled = False
					OptionButton1303.Enabled = False
					Frame1301.Enabled = False
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

				pTmpPoly.AddPoint(NJoinPt, 0)

				Side = SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)
				If Side < 0 Then
					ptTmp = PointAlongPlane(NJoinPt, DirToNav + SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(LeftPolys.Point(1), 0)
					ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp, 0)
			Else
				If _CheckState Then
					Frame1301.Enabled = True
				End If

				SideE = -1
				Side = -1
				SideP = -1

				If RayPolylineIntersect(LeftPolys, ptIn, DirToNav, PtInterP) Then
					SideP = SideDef(ptIn, DirToNav + 90.0, PtInterP)
				End If

				Dir2Int = ReturnAngleInDegrees(ptIn, LeftPolys.Point(1))
				ExtAngle = SubtractAngles(Dir2Int, DirToNav)
				TextBox1301_0.Text = CStr(ConvertAngle(ExtAngle))

				If ExtAngle <= 75.0 Then 'If ExtAngle <= 90.0 Then 16:49 02.04.2007
					PtInterE = New ESRI.ArcGIS.Geometry.Point
					PtInterE.PutCoords(ptIn.X, ptIn.Y)
					SideE = 1 'SideDef(ptIn, Dir2Int + 90.0, PtInterE)   16:49 02.04.2007
				End If

				If RayPolylineIntersect(LeftPolys, ptIn, DirToNav + arafTrn_OSplay.Value, PtInter15) Then
					Side = SideDef(ptIn, DirToNav + arafTrn_OSplay.Value + 90.0, PtInter15)
				End If

				OptionButton1301.Enabled = (SideP > 0)
				OptionButton1302.Enabled = (Side > 0)
				OptionButton1303.Enabled = (SideE > 0)

				'        msgbox "OptionButton1301.Enabled = " + CStr(OptionButton1301.Enabled) + vbCrLf + _
				''                              "OptionButton1303.Enabled = " + CStr(OptionButton1303.Enabled) + vbCrLf + _
				''                              "OptionButton1302.Enabled = " + CStr(OptionButton1302.Enabled)
				'        Frame1301.Enabled = OptionButton1301.Enabled And OptionButton1303.Enabled
				'======================================================================================
				If _CheckState Then
					If OptionButton1303.Enabled Then
						OptionButton1303.Checked = True
						ptTmp = PtInterE
					ElseIf OptionButton1301.Enabled Then
						OptionButton1301.Checked = True
						ptTmp = PtInterP
					ElseIf OptionButton1302.Enabled Then
						OptionButton1302.Checked = True
						ptTmp = PtInter15
					End If
				Else
					If OptionButton1303.Checked Then
						ptTmp = PtInterE
					ElseIf OptionButton1301.Checked Then
						ptTmp = PtInterP
					Else
						ptTmp = PtInter15
					End If
				End If

				NJoinPt = New ESRI.ArcGIS.Geometry.Point
				NJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

				pTmpPoly.AddPoint(ptTmp, 0)
				Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(LeftPolys.Point(1), 0)
					ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp, 0)
			End If
			'======================================================================================

			pTopoOper = RightPolys
			pPoints = pTopoOper.Intersect(OutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
			If pPoints.PointCount > 0 Then
				If _CheckState Then
					Frame1302.Enabled = False
					OptionButton1304.Enabled = False
					OptionButton1305.Enabled = False
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

				pTmpPoly.AddPoint(FJoinPt)
				Side = SideDef(FJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(FJoinPt, DirToNav - SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(RightPolys.Point(1))
					ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp)
			Else
				If _CheckState Then
					Frame1302.Enabled = True
					OptionButton1304.Enabled = True
					OptionButton1305.Enabled = True
				End If

				SideP = -1
				Side = -1

				If RayPolylineIntersect(RightPolys, ptOut, DirToNav, PtInterP) Then
					Dist1 = ReturnDistanceInMeters(PtInterP, ptOut)
					SideP = SideDef(ptOut, DirToNav + 90.0, PtInterP)
				End If

				If RayPolylineIntersect(RightPolys, ptOut, DirToNav - arafTrn_OSplay.Value, PtInter15) Then
					Dist2 = ReturnDistanceInMeters(PtInter15, ptOut)
					Side = SideDef(ptOut, DirToNav - arafTrn_OSplay.Value + 90.0, PtInter15)
				End If

				OptionButton1304.Enabled = SideP > 0
				OptionButton1305.Enabled = Side > 0

				'        Frame1302.Enabled = OptionButton1304.Enabled And OptionButton1306.Enabled

				'======================================================================================
				If _CheckState Then
					If OptionButton1305.Enabled Then
						OptionButton1305.Checked = True
						ptTmp = PtInter15
					ElseIf OptionButton1304.Enabled Then
						OptionButton1304.Checked = True
						ptTmp = PtInterP
					End If
				Else
					If OptionButton1305.Checked Then
						ptTmp = PtInter15
					ElseIf OptionButton1304.Checked Then
						ptTmp = PtInterP
					End If
				End If

				FJoinPt = New ESRI.ArcGIS.Geometry.Point
				FJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

				pTmpPoly.AddPoint(ptTmp)
				Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(RightPolys.Point(1))
					ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp)
			End If
			'======================================================================================
		Else
			pTmpPoly.AddPoint(ptIn)
			pTmpPoly.AddPoint(ptOut)

			pTopoOper = RightPolys
			pPoints = pTopoOper.Intersect(InLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
			If pPoints.PointCount > 0 Then
				If _CheckState Then
					OptionButton1301.Enabled = False
					OptionButton1302.Enabled = False
					OptionButton1303.Enabled = False
					Frame1301.Enabled = False
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

				pTmpPoly.AddPoint(NJoinPt, 0)
				Side = SideDef(NJoinPt, DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(NJoinPt, DirToNav - SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(RightPolys.Point(1), 0)
					ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp, 0)
			Else
				If _CheckState Then
					Frame1301.Enabled = True
					OptionButton1301.Enabled = True
					OptionButton1302.Enabled = True
					OptionButton1303.Enabled = True
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
				TextBox1301_0.Text = CStr(ConvertAngle(ExtAngle))

				If ExtAngle <= 75.0 Then 'If ExtAngle <= 90.0 Then                 <-17:50 02.04.2007
					PtInterE = New ESRI.ArcGIS.Geometry.Point
					PtInterE.PutCoords(ptIn.X, ptIn.Y)
					'            Dist2 = ReturnDistanceInMeters(PtInterE, ptIn)
					SideE = 1 'SideDef(ptIn, Dir2Int + 90.0, PtInterE) <-17:50 02.04.2007
				End If

				If RayPolylineIntersect(RightPolys, ptIn, DirToNav - arafTrn_OSplay.Value, PtInter15) Then
					'            Dist2 = ReturnDistanceInMeters(PtInter15, ptIn)
					'            msgbox  "15° Dist = " + CStr(Round(Dist2)) + " Side = " + CStr(SideE)
					Side = SideDef(ptIn, DirToNav - arafTrn_OSplay.Value + 90.0, PtInter15)
				End If

				OptionButton1301.Enabled = SideP > 0
				OptionButton1302.Enabled = Side > 0
				OptionButton1303.Enabled = SideE > 0
				'        Frame1301.Enabled = OptionButton1301.Enabled And OptionButton1303.Enabled
				'======================================================================================
				If _CheckState Then
					If OptionButton1303.Enabled Then
						OptionButton1303.Checked = True
						ptTmp = PtInterE
					ElseIf OptionButton1301.Enabled Then
						OptionButton1301.Checked = True
						ptTmp = PtInterP
					ElseIf OptionButton1302.Enabled Then
						OptionButton1302.Checked = True
						ptTmp = PtInter15
					End If
				Else
					If OptionButton1303.Checked Then
						ptTmp = PtInterE
					ElseIf OptionButton1301.Checked Then
						ptTmp = PtInterP
					Else
						ptTmp = PtInter15
					End If
				End If

				NJoinPt = New ESRI.ArcGIS.Geometry.Point
				NJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

				pTmpPoly.AddPoint(ptTmp, 0)
				Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(ptTmp, DirToNav - SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(RightPolys.Point(1), 0)
					ptTmp = PointAlongPlane(RightPolys.Point(1), DirToNav - SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp, 0)
			End If
			'======================================================================================

			pTopoOper = LeftPolys
			pPoints = pTopoOper.Intersect(OutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
			If pPoints.PointCount > 0 Then
				If _CheckState Then
					OptionButton1304.Enabled = False
					OptionButton1305.Enabled = False
					Frame1302.Enabled = False
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

				pTmpPoly.AddPoint(pPoints.Point(0))
				Side = SideDef(pPoints.Point(0), DirToNav + 90.0, TurnDirector.pPtPrj)

				If Side < 0 Then
					ptTmp = PointAlongPlane(pPoints.Point(0), DirToNav + SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(LeftPolys.Point(1))
					ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp)
			Else
				If _CheckState Then
					Frame1302.Enabled = True
					OptionButton1304.Enabled = True
					OptionButton1305.Enabled = True
				End If

				SideP = -1
				Side = -1

				If RayPolylineIntersect(LeftPolys, ptOut, DirToNav, PtInterP) Then
					Dist1 = ReturnDistanceInMeters(PtInterP, ptOut)
					SideP = SideDef(ptOut, DirToNav + 90.0, PtInterP)
				End If

				If RayPolylineIntersect(LeftPolys, ptOut, DirToNav + arafTrn_OSplay.Value, PtInter15) Then
					Dist2 = ReturnDistanceInMeters(PtInter15, ptOut)
					Side = SideDef(ptOut, DirToNav + arafTrn_OSplay.Value + 90.0, PtInter15)
				End If

				OptionButton1304.Enabled = SideP > 0
				OptionButton1305.Enabled = Side > 0

				'        Frame1302.Enabled = OptionButton1304.Enabled And OptionButton1306.Enabled
				'======================================================================================
				If _CheckState Then
					If OptionButton1305.Enabled Then
						OptionButton1305.Checked = True
						ptTmp = PtInter15
					ElseIf OptionButton1304.Enabled Then
						OptionButton1304.Checked = True
						ptTmp = PtInterP
					End If
				Else
					If OptionButton1305.Checked Then
						ptTmp = PtInter15
					ElseIf OptionButton1304.Checked Then
						ptTmp = PtInterP
					End If
				End If

				FJoinPt = New ESRI.ArcGIS.Geometry.Point
				FJoinPt.PutCoords(ptTmp.X, ptTmp.Y)

				pTmpPoly.AddPoint(ptTmp)
				Side = SideDef(ptTmp, DirToNav + 90.0, TurnDirector.pPtPrj)

				'DrawPolygon TmpPoly, 255
				'DrawPoint TurnWPT.pPtPrj, 0
				If Side < 0 Then
					ptTmp = PointAlongPlane(ptTmp, DirToNav + SplayAngle, 10.0 * RModel)
				Else
					pTmpPoly.AddPoint(LeftPolys.Point(1))
					ptTmp = PointAlongPlane(LeftPolys.Point(1), DirToNav + SplayAngle, 10.0 * RModel)
				End If

				pTmpPoly.AddPoint(ptTmp)
			End If
			'======================================================================================
		End If

		pTopoOper = pTmpPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		CalcAreaIntersection = RemoveAgnails(CalcAreaIntersection)

		pTopoOper = CalcAreaIntersection
		'TopoOper.IsKnownSimple_2 = False
		'TopoOper.Simplify
		CalcAreaIntersection = pTopoOper.Union(pTmpPoly)
	End Function

	Private Function GetNearPoint(ByRef OutDir As Double, ByRef iTurnDir As Integer, ByRef ptFIX As ESRI.ArcGIS.Geometry.Point, ByRef CheckDir As Double, Optional ByRef Genishlanir As Boolean = False, Optional ByRef pCheckpt As ESRI.ArcGIS.Geometry.Point = Nothing) As ESRI.ArcGIS.Geometry.Point
		Dim I As Integer
		Dim N As Integer
		Dim fCurrDir As Double
		Dim fMaxVal As Double
		Dim fTmp As Double

		Dim ptTmp As ESRI.ArcGIS.Geometry.Point
		Dim ptResult As ESRI.ArcGIS.Geometry.IPoint
		Dim pPline As ESRI.ArcGIS.Geometry.IPolyline
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pCheckPoints As ESRI.ArcGIS.Geometry.IPointCollection

		pCheckPoints = New ESRI.ArcGIS.Geometry.Polygon

		If OptionButton0902.Checked Then
			If iTurnDir > 0 Then
				pCheckPoints.AddPoint(KKFixMax.FromPoint)
				pCheckPoints.AddPoint(KKFixMax.ToPoint)
				pCheckPoints.AddPoint(KK.ToPoint)
				pCheckPoints.AddPoint(KK.FromPoint)
			Else
				pCheckPoints.AddPoint(KKFixMax.ToPoint)
				pCheckPoints.AddPoint(KKFixMax.FromPoint)
				pCheckPoints.AddPoint(KK.FromPoint)
				pCheckPoints.AddPoint(KK.ToPoint)
			End If
		Else
			If iTurnDir > 0 Then
				pCheckPoints.AddPoint(KK.FromPoint)
				pCheckPoints.AddPoint(KK.ToPoint)
				pCheckPoints.AddPoint(KKhMin.ToPoint)
				pCheckPoints.AddPoint(KKhMin.FromPoint)
			Else
				pCheckPoints.AddPoint(KK.ToPoint)
				pCheckPoints.AddPoint(KK.FromPoint)
				pCheckPoints.AddPoint(KKhMin.FromPoint)
				pCheckPoints.AddPoint(KKhMin.ToPoint)
			End If
		End If

		N = pCheckPoints.PointCount
		ptResult = pCheckPoints.Point(0)

		If Genishlanir Then
			fMaxVal = 0.0

			For I = 0 To N - 1
				fCurrDir = ReturnAngleInDegrees(pCheckpt, pCheckPoints.Point(I))
				fTmp = SubtractAnglesWithSign(fCurrDir, CheckDir, iTurnDir)
				If fTmp > fMaxVal Then
					fMaxVal = fTmp
					ptResult = pCheckPoints.Point(I)
				End If
			Next I
		Else
			ptTmp = PointAlongPlane(ptFIX, OutDir + 90.0 * iTurnDir, 100000.0)
			pPline = New ESRI.ArcGIS.Geometry.Polyline

			pPline.FromPoint = PointAlongPlane(ptTmp, CheckDir, 100000.0)
			pPline.ToPoint = PointAlongPlane(ptTmp, CheckDir + 180.0, 100000.0)

			'DrawPointWithText(ptTmp, "ptTmp ")
			'DrawPolyLine(pPline, -1, 2)

			'While True
			'	Application.DoEvents()
			'End While

			fMaxVal = 100000.0
			pProxi = pPline
			For I = 0 To N - 1
				fTmp = pProxi.ReturnDistance(pCheckPoints.Point(I))
				If fTmp < fMaxVal Then
					fMaxVal = fTmp
					ptResult = pCheckPoints.Point(I)
				End If
			Next I
		End If

		Return ptResult
	End Function

	Private Sub CalcJoiningParams(ByRef TypeCode As Integer, ByRef iTurnDir As Integer, ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutPt As ESRI.ArcGIS.Geometry.IPoint, ByRef OutAzt As Double)
		Dim AllPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim votPovorot As ESRI.ArcGIS.Geometry.IPoint
		Dim antiPovorot As ESRI.ArcGIS.Geometry.IPoint
		Dim Side As Integer
		Dim Side1 As Integer
		Dim KendineGayidis As Boolean
		Dim fTmpAzt As Double
		Dim TurnNav As NavaidData

		Prim = New ESRI.ArcGIS.Geometry.Polygon
		SecL = New ESRI.ArcGIS.Geometry.Polygon
		SecR = New ESRI.ArcGIS.Geometry.Polygon
		AllPolys = New ESRI.ArcGIS.Geometry.Polygon

		KendineGayidis = (TurnDirector.pPtPrj.X - FinalNav.pPtPrj.X) ^ 2 + (TurnDirector.pPtPrj.Y - FinalNav.pPtPrj.Y) ^ 2 < distEps * distEps

		TurnNav = TurnWPTToTurnNav(_bTurnFIXSameMAPtFIX, MAPtNavDat(ComboBox0801.SelectedIndex), TurnDirector)

		CreateNavaidZone(TurnNav, OutAzt, FictTHR, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

		votPovorot = TurnArea.Point(TurnArea.PointCount - 1)

		AllPolys.AddPoint(SecL.Point(5)) '0
		AllPolys.AddPoint(SecL.Point(0)) '1
		AllPolys.AddPoint(SecL.Point(1)) '2

		AllPolys.AddPoint(SecR.Point(2)) '3
		AllPolys.AddPoint(SecR.Point(3)) '4
		AllPolys.AddPoint(SecR.Point(4)) '5

		TextBox1301_0.Text = ""
		TextBox1301_1.Text = ""

		NReqCorrAngle = -1.0
		FReqCorrAngle = -1.0

		If iTurnDir < 0 Then 'sag
			'================== xarici ========================
			fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
			Side = SideFrom2Angle(OutAzt, fTmpAzt)
			Side1 = SideDef(AllPolys.Point(1), fTmpAzt, votPovorot)

			If Side * Side1 < 0 Then
				If _CheckState Then
					Frame1302.Enabled = False
					OptionButton1304.Enabled = False
					OptionButton1306.Enabled = False
				End If
			Else
				If _CheckState Then
					Frame1302.Enabled = True
					OptionButton1304.Enabled = True
					OptionButton1306.Enabled = True
				End If

				FReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(1), votPovorot)
				FReqCorrAngle = Modulus(FReqCorrAngle - fTmpAzt, 360.0)
				If FReqCorrAngle > 180.0 Then FReqCorrAngle = 360.0 - FReqCorrAngle
				TextBox1301_1.Text = ConvertAngle(FReqCorrAngle, eRoundMode.CEIL).ToString()

				If FReqCorrAngle > fCorrAngle Then
					OptionButton1304.Checked = True
				Else
					OptionButton1306.Checked = True
				End If
			End If
			'================== daxili ========================
			If KendineGayidis Then
				If _CheckState Then
					Frame1301.Enabled = False
					OptionButton1301.Enabled = False
					OptionButton1303.Enabled = False
					OptionButton1303.Checked = True
				End If
			Else
				fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))
				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, fTmpAzt, True, AllPolys.Point(4))

				If antiPovorot Is Nothing Then
					If _CheckState Then
						OptionButton1301.Enabled = False
						OptionButton1303.Enabled = False
						Frame1301.Enabled = False
					End If
				Else
					If _CheckState Then
						Frame1301.Enabled = True
						OptionButton1301.Enabled = True
						OptionButton1303.Enabled = True
					End If

					NReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(4), antiPovorot)
					NReqCorrAngle = Modulus(fTmpAzt - NReqCorrAngle, 360.0)

					If NReqCorrAngle > 180.0 Then NReqCorrAngle = 360.0 - NReqCorrAngle
					TextBox1301_0.Text = ConvertAngle(NReqCorrAngle, eRoundMode.CEIL).ToString()

					If _CheckState Then
						If NReqCorrAngle > NCorrAngle Then
							OptionButton1301.Checked = True
						Else
							OptionButton1303.Checked = True
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
				If _CheckState Then
					OptionButton1304.Enabled = False
					OptionButton1306.Enabled = False
					Frame1302.Enabled = False
				End If
			Else
				If _CheckState Then
					Frame1302.Enabled = True
					OptionButton1304.Enabled = True
					OptionButton1306.Enabled = True
				End If

				FReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(4), votPovorot)
				FReqCorrAngle = Modulus(FReqCorrAngle - fTmpAzt, 360.0)
				If FReqCorrAngle > 180.0 Then FReqCorrAngle = 360.0 - FReqCorrAngle
				TextBox1301_1.Text = ConvertAngle(FReqCorrAngle, eRoundMode.CEIL).ToString()

				If _CheckState Then
					If FReqCorrAngle > fCorrAngle Then
						OptionButton1304.Checked = True
					Else
						OptionButton1306.Checked = True
					End If
				End If
			End If
			'================== daxili ========================
			If KendineGayidis Then
				If _CheckState Then
					Frame1301.Enabled = False
					OptionButton1301.Enabled = False
					OptionButton1303.Enabled = False
					OptionButton1303.Checked = True
				End If
			Else
				fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, fTmpAzt, True, AllPolys.Point(1))

				If antiPovorot Is Nothing Then
					If _CheckState Then
						OptionButton1301.Enabled = False
						OptionButton1303.Enabled = False
						Frame1301.Enabled = False
					End If
				Else
					If _CheckState Then
						Frame1301.Enabled = True
						OptionButton1301.Enabled = True
						OptionButton1303.Enabled = True
					End If

					NReqCorrAngle = ReturnAngleInDegrees(AllPolys.Point(1), antiPovorot)
					NReqCorrAngle = Modulus(fTmpAzt - NReqCorrAngle, 360.0)

					If NReqCorrAngle > 180.0 Then NReqCorrAngle = 360.0 - NReqCorrAngle
					TextBox1301_0.Text = ConvertAngle(NReqCorrAngle, eRoundMode.CEIL).ToString()

					If _CheckState Then
						If NReqCorrAngle > NCorrAngle Then
							OptionButton1301.Checked = True
						Else
							OptionButton1303.Checked = True
						End If
					End If
				End If
			End If
		End If

	End Sub

	Private Function ApplayJoining(ByRef TypeCode As Integer, ByRef iTurnDir As Integer, ByRef TurnArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef OutPt As ESRI.ArcGIS.Geometry.Point, ByRef OutAzt As Double) As ESRI.ArcGIS.Geometry.Polygon
		Dim Side1 As Integer
		Dim Side As Integer


		Dim NavOuterAzt As Double
		Dim InitWidth As Double
		Dim antiAngle As Double
		Dim turnAngle As Double
		Dim fTmpAzt As Double
		Dim fTmp As Double

		Dim AllPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Result As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim Clone As ESRI.ArcGIS.esriSystem.IClone

		Dim pReleation As ESRI.ArcGIS.Geometry.IRelationalOperator

		Dim antiPovorot As ESRI.ArcGIS.Geometry.IPoint
		Dim votPovorot As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim TurnNav As NavaidData

		votPovorot = TurnArea.Point(TurnArea.PointCount - 1)
		FJoinPt = votPovorot

		Result = New ESRI.ArcGIS.Geometry.Polygon
		Result.AddPoint(votPovorot)

		TurnNav = TurnWPTToTurnNav(_bTurnFIXSameMAPtFIX, MAPtNavDat(ComboBox0801.SelectedIndex), TurnDirector)
		CreateNavaidZone(TurnNav, OutAzt, FictTHR, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

		AllPolys = New ESRI.ArcGIS.Geometry.Polygon
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
					Result.AddPoint(FOutPt)
					Result.AddPoint(AllPolys.Point(1))
				Else
					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(0), AllPolys.Point(2))
					Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(0), DegToRad(fTmpAzt))
					Result.AddPoint(FOutPt)
				End If

				Result.AddPoint(AllPolys.Point(2))
			Else
				If OptionButton1304.Checked Then
					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
					Construct.ConstructAngleIntersection(votPovorot, DegToRad(OutAzt), AllPolys.Point(1), DegToRad(fTmpAzt))

					Result.AddPoint(FOutPt)
					Side = SideDef(FOutPt, OutAzt, AllPolys.Point(2))
					If Side < 0 Then
						Result.AddPoint(AllPolys.Point(2))
					End If
				Else 'Genishlanir
					FOutPt.PutCoords(votPovorot.X, votPovorot.Y)

					Result.AddPoint(AllPolys.Point(1))
					Result.AddPoint(AllPolys.Point(2))
				End If
			End If
			'================== daxili ========================
			Construct = NOutPt

			If (TurnDirector.pPtPrj.X - FinalNav.pPtPrj.X) ^ 2 + (TurnDirector.pPtPrj.Y - FinalNav.pPtPrj.Y) ^ 2 < distEps * distEps Then
				Result.AddPoint(AllPolys.Point(3))

				If SubtractAngles(_ArDir, OutAzt) > 15.0 Then
					If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
						InitWidth = VOR.InitWidth
						fTmp = VOR.SplayAngle
					ElseIf TurnDirector.TypeCode = eNavaidType.NDB Then
						InitWidth = NDB.InitWidth
						fTmp = NDB.SplayAngle
					End If

					ptTmp = PointAlongPlane(FinalNav.pPtPrj, _ArDir - 90.0 * TurnDir, 0.5 * InitWidth)
					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))

					Construct.ConstructAngleIntersection(ptTmp, DegToRad(_ArDir - fTmp * TurnDir), AllPolys.Point(4), DegToRad(fTmpAzt))
				Else
					Result.AddPoint(AllPolys.Point(4))
					If OptionButton0902.Checked Then
						NOutPt.PutCoords(KKFixMax.FromPoint.X, KKFixMax.FromPoint.Y)
					Else
						NOutPt.PutCoords(KK.FromPoint.X, KK.FromPoint.Y)
					End If
				End If
				Result.AddPoint(NOutPt)

				turnAngle = Modulus((OutAzt - _ArDir) * TurnDir)

				If turnAngle > 270 - 15 Then
					antiAngle = OutAzt - arafTrn_ISplay.Value * iTurnDir
				Else
					antiAngle = OutAzt + arafTrn_ISplay.Value * iTurnDir
				End If

				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, antiAngle)

				'antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, OutAzt + arafTrn_ISplay.Value * iTurnDir)
				NJoinPt = antiPovorot
			Else
				'===============================================================================
				fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(5))
				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, fTmpAzt, True, AllPolys.Point(4))

				If antiPovorot Is Nothing Then
					antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, OutAzt + arafTrn_ISplay.Value * iTurnDir)

					NavOuterAzt = OutAzt + arafTrn_ISplay.Value * iTurnDir
					Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

					Result.AddPoint(AllPolys.Point(3))

					fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(4))
					Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt)
					If (Side * iTurnDir < 0) Then
						Result.AddPoint(AllPolys.Point(4))
					Else
						fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
						Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))
					End If

					Result.AddPoint(NOutPt)
					NJoinPt = antiPovorot
				Else
					If OptionButton1301.Checked Then
						antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, OutAzt)

						fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
						Construct.ConstructAngleIntersection(antiPovorot, DegToRad(OutAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

						Side = SideDef(NOutPt, OutAzt, AllPolys.Point(3))
						If Side * iTurnDir < 0 Then
							Result.AddPoint(AllPolys.Point(3))
						End If
						Result.AddPoint(NOutPt)
						NJoinPt = antiPovorot
					Else 'Genishlanir
						NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y)

						Result.AddPoint(AllPolys.Point(3))
						Result.AddPoint(AllPolys.Point(4))
						Result.AddPoint(NOutPt)
						NJoinPt = NOutPt
					End If
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
					Result.AddPoint(FOutPt)
					Result.AddPoint(AllPolys.Point(4))
				Else
					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
					Construct.ConstructAngleIntersection(votPovorot, DegToRad(NavOuterAzt), AllPolys.Point(4), DegToRad(fTmpAzt))
					Result.AddPoint(FOutPt)
				End If
				Result.AddPoint(AllPolys.Point(3))
			Else
				If OptionButton1304.Checked Then 'Genishlanmir
					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(4), AllPolys.Point(3))
					Construct.ConstructAngleIntersection(votPovorot, DegToRad(OutAzt), AllPolys.Point(4), DegToRad(fTmpAzt))

					Result.AddPoint(FOutPt)
					Side = SideDef(FOutPt, OutAzt, AllPolys.Point(3))
					If Side > 0 Then
						Result.AddPoint(AllPolys.Point(3))
					End If
				Else 'Genishlanir
					FOutPt.PutCoords(votPovorot.X, votPovorot.Y)
					Result.AddPoint(AllPolys.Point(4))
					Result.AddPoint(AllPolys.Point(3))
				End If
			End If
			'================== daxili ========================'eExpansion.selfExpand
			Construct = NOutPt

			If (TurnDirector.pPtPrj.X - FinalNav.pPtPrj.X) ^ 2 + (TurnDirector.pPtPrj.Y - FinalNav.pPtPrj.Y) ^ 2 < distEps * distEps Then
				Result.AddPoint(AllPolys.Point(2))


				If SubtractAngles(_ArDir, OutAzt) > 15.0 Then
					If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
						InitWidth = VOR.InitWidth
						fTmp = VOR.SplayAngle
					ElseIf TurnDirector.TypeCode = eNavaidType.NDB Then
						InitWidth = NDB.InitWidth
						fTmp = NDB.SplayAngle
					End If

					ptTmp = PointAlongPlane(FinalNav.pPtPrj, _ArDir - 90.0 * TurnDir, 0.5 * InitWidth)

					fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
					Construct.ConstructAngleIntersection(ptTmp, DegToRad(_ArDir - fTmp * TurnDir), AllPolys.Point(1), DegToRad(fTmpAzt))
				Else
					Result.AddPoint(AllPolys.Point(1))
					If OptionButton0902.Checked Then
						NOutPt.PutCoords(KKFixMax.ToPoint.X, KKFixMax.ToPoint.Y)
					Else
						NOutPt.PutCoords(KK.ToPoint.X, KK.ToPoint.Y)
					End If
				End If

				Result.AddPoint(NOutPt)

				turnAngle = Modulus((OutAzt - _ArDir) * TurnDir)

				If turnAngle > 270 - 15 Then
					antiAngle = OutAzt - arafTrn_ISplay.Value * iTurnDir
				Else
					antiAngle = OutAzt + arafTrn_ISplay.Value * iTurnDir
				End If

				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, antiAngle)
				NJoinPt = antiPovorot
			Else
				'========================================================
				fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(0))
				antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, fTmpAzt, True, AllPolys.Point(1))

				If antiPovorot Is Nothing Then
					NavOuterAzt = OutAzt + arafTrn_ISplay.Value * iTurnDir
					antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, NavOuterAzt)

					Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(1), DegToRad(fTmpAzt))
					Result.AddPoint(AllPolys.Point(2))

					fTmpAzt = ReturnAngleInDegrees(TurnDirector.pPtPrj, AllPolys.Point(1))
					Side = SideDef(TurnDirector.pPtPrj, fTmpAzt, NOutPt)
					If (Side * iTurnDir < 0) Then
						Result.AddPoint(AllPolys.Point(1))
					Else
						fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
						Construct.ConstructAngleIntersection(antiPovorot, DegToRad(NavOuterAzt), AllPolys.Point(1), DegToRad(fTmpAzt))
					End If
					Result.AddPoint(NOutPt)
					NJoinPt = antiPovorot
				Else
					'========================================================
					If OptionButton1301.Checked Then 'Genishlanmir
						antiPovorot = GetNearPoint(OutAzt, iTurnDir, TurnFixPnt, OutAzt)

						fTmpAzt = ReturnAngleInDegrees(AllPolys.Point(1), AllPolys.Point(2))
						Construct.ConstructAngleIntersection(antiPovorot, DegToRad(OutAzt), AllPolys.Point(1), DegToRad(fTmpAzt))

						Side = SideDef(NOutPt, OutAzt, AllPolys.Point(2))
						If Side * iTurnDir < 0 Then
							Result.AddPoint(AllPolys.Point(2))
						End If

						Result.AddPoint(NOutPt)
						NJoinPt = antiPovorot
					Else 'Genishlanir
						NOutPt.PutCoords(antiPovorot.X, antiPovorot.Y)
						Result.AddPoint(AllPolys.Point(2))
						Result.AddPoint(AllPolys.Point(1))
						Result.AddPoint(NOutPt)
						NJoinPt = NOutPt
					End If
				End If
			End If
		End If

		Result.AddPoint(antiPovorot)
		'Result.AddPoint TurnArea.Point(0)

		Clone = Result
		tmpPoly = Clone.Clone

		pTopoOper = tmpPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pReleation = tmpPoly

		If Not pReleation.Contains(TurnArea.Point(0)) Then
			If Modulus((OutAzt - _ArDir) * TurnDir, 360.0) < 175.0 Then
				Result.AddPoint(TurnArea.Point(0))
			End If
		End If

		Result.AddPoint(votPovorot)

		pTopoOper = Result
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		Clone = TurnArea
		tmpPoly = Clone.Clone

		pTopoOper = tmpPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		Result = pTopoOper.Union(Result)

		pTopoOper = Result
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		Return Result
	End Function

	Private Sub UpdateIntervals(Optional ChangeCurDir As Boolean = False)
		Dim Intervals() As Interval
		Dim CurPnt As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint

		Dim tmpStr As String

		Dim VTotal As Double
		Dim lMinDR As Double
		Dim tStabl As Double
		Dim hCalc As Double
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
		Dim FixPntPrj As ESRI.ArcGIS.Geometry.IPoint

		If MultiPage1.SelectedIndex < 10 Then Return

		If _bTurnFIXSameMAPtFIX Then
			FixPntPrj = TurnFixPnt
		Else
			FixPntPrj = TurnDirector.pPtPrj
		End If

		Snap = CDbl(TextBox1204.Text)
		tStabl = CDbl(TextBox1205.Text)

		hCalc = arMATurnAlt.Value + fRefHeight
		If TurnFixPnt.Z > hCalc Then
			hCalc = TurnFixPnt.Z
		End If

		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)

		VTotal = fTASl + CurrADHP.WindSpeed

		If OptionButton0901.Checked Then
			L0 = Point2LineDistancePrj(PtSOC, KK.FromPoint, _ArDir + 90.0)
			CurPnt = PointAlongPlane(PtSOC, _ArDir, L0)
		Else
			L0 = Point2LineDistancePrj(PtSOC, KKFixMax.FromPoint, _ArDir + 90.0)
			CurPnt = New ESRI.ArcGIS.Geometry.Point
			CurPnt.PutCoords(TurnFixPnt.X, TurnFixPnt.Y)
		End If

		TurnR = Bank2Radius(fBankAngle, fTASl)
		'==========================================
		ddr = TurnR / (System.Math.Tan(DegToRad((180.0 - Snap) * 0.5)))
		dr = arT_Gui_dist.Value + ddr
		lMinDR = VTotal * tStabl * 0.277777777777778 + ddr
		MinDR = lMinDR - ddr
		ToolTip1.SetToolTip(TextBox1205, My.Resources.str13009 + CStr(ConvertDistance(MinDR, eRoundMode.NEAREST)) + " " + DistanceConverter(DistanceUnit).Unit)
		'==========================================
		ptCnt = PointAlongPlane(CurPnt, _ArDir + 90.0 * TurnDir, TurnR)
		bAz = ReturnAngleInDegrees(FixPntPrj, ptCnt)
		D = ReturnDistanceInMeters(ptCnt, FixPntPrj)

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

		'TextBox403 = CStr(Round(Modulus(Dir2Azt(ptDerPrj, DepDir) - MagVar, 360.0)))

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

		RecommendStr = My.Resources.str13008  '"Рекомендуемые интервалы курса: "
		tmpStr = RecommendStr + vbCrLf

		For I = 0 To N
			If SubtractAngles(ConvertAngle(Intervals(I).Left), ConvertAngle(Intervals(I).Right)) <= degEps Then
				RecommendStr = RecommendStr + CStr(ConvertAngle(Intervals(I).Left)) + "°"
				tmpStr = tmpStr + CStr(ConvertAngle(Intervals(I).Left)) + "°"
				If (I = 0) And ChangeCurDir Then TextBox1203.Text = CStr(ConvertAngle(Intervals(0).Left))
			Else
				RecommendStr = RecommendStr + My.Resources.str00221 + CStr(ConvertAngle(Intervals(I).Left, eRoundMode.CEIL)) + "°" + My.Resources.str00222 + CStr(ConvertAngle(Intervals(I).Right, eRoundMode.FLOOR)) + "°"
				tmpStr = tmpStr + My.Resources.str00221 + CStr(ConvertAngle(Intervals(I).Left, eRoundMode.CEIL)) + "°" + My.Resources.str00222 + CStr(ConvertAngle(Intervals(I).Right, eRoundMode.FLOOR)) + "°"
				If (I = 0) And ChangeCurDir Then TextBox1203.Text = CStr(ConvertAngle(Intervals(0).Left, eRoundMode.CEIL))
			End If

			If I <> N Then
				RecommendStr = RecommendStr + "; "
				tmpStr = tmpStr + vbCrLf
			End If
		Next

		If ChangeCurDir Then _OutDir = Azt2Dir(TurnDirector.pPtGeo, CDbl(TextBox1203.Text) + TurnDirector.MagVar)

		_Label1201_7.Text = tmpStr

		TextBox1204.Tag = TextBox1204.Text
		TextBox1205.Tag = TextBox1205.Text

		If _bTurnFIXSameMAPtFIX Or OptionButton1202.Checked Then
			ToolTip1.SetToolTip(TextBox1203, RecommendStr)
		Else
			ToolTip1.SetToolTip(TextBox1203, "")
		End If

		If ChangeCurDir Then
			TextBox1203.Tag = ""
			TextBox1203_Validating(TextBox1203, New CancelEventArgs())
		End If
	End Sub

	Private Sub DrawTrail()
		Dim d0 As Double
		Dim fTmpDist As Double

		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pPoly As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pBufferPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
		pPolyline = mPoly
		pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

		If OptionButton1203.Checked Then
			pPath.AddPoint(TurnDirector.pPtPrj)
		Else
			pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)

			If OptionButton0901.Checked Then
				pTopo = ZNR_Poly
			Else
				pTopo = KK
			End If

			pProxi = pTopo
			fTmpDist = pProxi.ReturnDistance(pTurnComplitPt) + 0.5

			d0 = (hTMA + fRefHeight - TurnFixPnt.Z) / fMissAprPDG
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

		TextBox1511.Text = CStr(ConvertDistance(mPoly.Length, eRoundMode.NEAREST))
		TextBox1512.Text = CStr(ConvertHeight(TurnFixPnt.Z + mPoly.Length * fMissAprPDG, eRoundMode.NEAREST))
	End Sub

	Private Function UpdateToNavCourse(ByVal OutAzt As Double, ByRef TurnNav As NavaidData) As Integer
		Dim I As Integer
		Dim K As Integer
		Dim Side As Integer
		Dim TurnDir2 As Integer

		Dim InterAngle As Double
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
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim NewPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSource As ESRI.ArcGIS.esriSystem.IClone
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		'====================
		UpdateToNavCourse = 0

		pTopoOper = pMAPtArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		If OptionButton0902.Checked Then 'на Высоте
			pSource = pFullPoly
			tmpPoly = pSource.Clone

			NewPoly = New ESRI.ArcGIS.Geometry.Polyline
			NewPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
			NewPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

			CutPoly(tmpPoly, NewPoly, -1)
			pTopoOper = tmpPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		Else
			tmpPoly = pTopoOper.Union(pFullPoly)
		End If

		tmpPoly = RemoveAgnails(tmpPoly)
		pRPolygon = ReArrangePolygon(tmpPoly, PtSOC, _ArDir)

		hCalc = Max(fRefHeight + arMATurnAlt.Value, TurnFixPnt.Z)
		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)
		TurnR = Bank2Radius(fBankAngle, fTASl)
		''=========  End Params
		'fTmp = Modulus(Dir2Azt(PtSOC, OutAzt) - CurrADHP.MagVar, 360.0)
		fTmp = Modulus(Dir2Azt(TurnDirector.pPtPrj, OutAzt) - TurnDirector.MagVar)
		TextBox1203.Tag = CStr(ConvertAngle(fTmp))
		TextBox1203.Text = TextBox1203.Tag
		InterAngle = CDbl(TextBox1204.Text)

		'=====
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		MPtCollection = CalcTouchByFixDir(TurnFixPnt, TurnDirector.pPtPrj, TurnR, _ArDir, OutAzt, TurnDir, TurnDir2, InterAngle, dDir, FlyBy)
		mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)

		ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
		pPolyline = mPoly
		pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

		If ComboBox1203.SelectedIndex = 1 Then
			If SideDef(ptCur, OutAzt + 90.0, WPt1202.pPtPrj) > 0 Then
				pPath.AddPoint(WPt1202.pPtPrj)
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

		TextBox1202.Text = CStr(ConvertDistance(dDir, eRoundMode.NEAREST))

		If (ConvertAngle(dDir) > arT_Gui_dist.Value) Or (ConvertAngle(dDir) < MinDR) Then
			TextBox1202.ForeColor = Color.Red
		Else
			TextBox1202.ForeColor = Color.Black
		End If

		If dDir > RModel Then MessageBox.Show(My.Resources.str00303) '"При данном значении угла выход на курс средства невозможен."

		m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)

		'If TurnDir * TurnDir2 < 0 Then
		'	fTmp = OutAzt
		'Else
		fTmp = MPtCollection.Point(1).M
		'End If

		m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, fTmp, TurnDir, m_BasePoints)

		fTmp = ConvertAngle(Modulus(TurnDir * (MPtCollection.Point(1).M - _ArDir)))
		TextBox1201.Text = CStr(fTmp)

		If fTmp > 255.0 Then
			TextBox1201.ForeColor = Color.Red
		Else
			TextBox1201.ForeColor = Color.Black
		End If

		'If False Then
		'	If TurnDir > 0 Then
		'		If m_TurnArea.PointCount > 0 Then
		'			m_TurnArea.AddPoint(KK.FromPoint, 0)
		'			m_TurnArea.AddPoint(KK.ToPoint, 0)
		'		Else
		'			m_TurnArea.AddPoint(KK.ToPoint)
		'			m_TurnArea.AddPoint(KK.FromPoint)
		'		End If
		'	Else
		'		If m_TurnArea.PointCount > 0 Then
		'			m_TurnArea.AddPoint(KK.ToPoint, 0)
		'			m_TurnArea.AddPoint(KK.FromPoint, 0)
		'		Else
		'			m_TurnArea.AddPoint(KK.FromPoint)
		'			m_TurnArea.AddPoint(KK.ToPoint)
		'		End If
		'	End If

		'	If CheckBox0901.Checked Then
		'		If TurnDir > 0 Then
		'			m_TurnArea.AddPoint(pFullPoly.Point(pFullPoly.PointCount - 2), 0)
		'		Else
		'			m_TurnArea.AddPoint(pFullPoly.Point(1), 0)
		'		End If
		'	End If
		'Else
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
		'End If

		'DrawPolygon(m_TurnArea, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSVertical)
		'Application.DoEvents()

		'====================================================================
		Dim LeftPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim RightPolys As ESRI.ArcGIS.Geometry.IPointCollection
		Dim InLine As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Prim = New ESRI.ArcGIS.Geometry.Polygon
		SecL = New ESRI.ArcGIS.Geometry.Polygon
		SecR = New ESRI.ArcGIS.Geometry.Polygon

		LeftPolys = New ESRI.ArcGIS.Geometry.Polyline
		RightPolys = New ESRI.ArcGIS.Geometry.Polyline
		InLine = New ESRI.ArcGIS.Geometry.Polyline

		_DirToNav = MPtCollection.Point(MPtCollection.PointCount - 1).M

		CreateNavaidZone(TurnNav, _DirToNav, FictTHR, Ss, Vs, 2.0 * VOR.Range, 2.0 * VOR.Range, SecL, SecR, Prim)

		ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)

		'If TurnDir * TurnDir2 < 0 Then
		fTmp = MPtCollection.Point(1).M
		'Else
		'	fTmp = OutAzt
		'End If
		AztCur = fTmp + arafTrn_OSplay.Value * TurnDir

		ptTmp = GetNearPoint(OutAzt, TurnDir, TurnFixPnt, AztCur)

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

		If pPoints.PointCount <= 0 Then ptTmp = GetNearPoint(OutAzt, TurnDir, TurnFixPnt, OutAzt)

		'===========================================================================

		m_TurnArea.AddPoint(ptTmp, 0)

		NewPoly = CalcAreaIntersection(m_TurnArea, fTmp, _DirToNav, TurnDir, TurnDir2)

		pTopoOper = m_BasePoints
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pTopoOper = NewPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()


		BaseArea = RemoveHoles(pTopoOper.Union(m_BasePoints))

		'    If OptionButton0901 Then
		'        Set pTopoOper = BaseArea
		'        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
		'    End If

		'    Set pTopoOper = pCircle
		'    Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
		'ge = New GeometryEnvironment()
		'ge.UseAlternativeTopoOps = True

		'DrawPolygon(BaseArea, 0)

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
		On Error GoTo 0

		DrawPoly = PolygonIntersection(pCircle, Prim)
		PrimElem = DrawPolygon(DrawPoly, 0, , False)

		DrawPoly = PolygonIntersection(pCircle, SecL)
		'    DrawPoly = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
		SecLElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

		'    TurnAreaElem = DrawPolygon(BaseArea, 255, False)
		'    NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)

		DrawPoly = PolygonIntersection(pCircle, SecR)
		'    DrawPoly = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
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
	End Function

	Private Function UpdateToFix() As Integer
		Dim TurnR As Double
		Dim hCalc As Double
		Dim fTASl As Double

		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim pLPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRPolygon As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim NewPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim DrawPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pSource As ESRI.ArcGIS.esriSystem.IClone

		pTopoOper = pMAPtArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		If OptionButton0902.Checked Then 'не на Высоте
			pSource = pFullPoly
			tmpPoly1 = pSource.Clone

			tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
			tmpPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
			tmpPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

			CutPoly(tmpPoly1, tmpPoly, -1)

			pTopoOper = tmpPoly1
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
			tmpPoly = pTopoOper.Union(tmpPoly1)
		Else
			tmpPoly = pTopoOper.Union(pFullPoly)
		End If

		tmpPoly = RemoveAgnails(tmpPoly)
		pRPolygon = ReArrangePolygon(tmpPoly, PtSOC, _ArDir)

		hCalc = Max(fRefHeight + arMATurnAlt.Value, TurnFixPnt.Z)
		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)
		TurnR = Bank2Radius(fBankAngle, fTASl)

		MPtCollection = TurnToFixPrj(TurnFixPnt, TurnR, TurnDir, TurnDirector.pPtPrj)

		If MPtCollection.PointCount = 0 Then
			MessageBox.Show(My.Resources.str00303)  '"С заданным направлением разворота и " + vbCrLf |        + "радиусом выход на данный пункт невозможен" + vbCrLf |        + "В заданном направлении Rmax =" + CStr(Fix(TurnR)) + "метр"
			Return -1
		End If

		m_OutPt = MPtCollection.Point(1)

		_DirToNav = MPtCollection.Point(MPtCollection.PointCount - 1).M
		_OutDir = MPtCollection.Point(1).M


		TextBox1203.Tag = CStr(ConvertAngle(Modulus(Dir2Azt(TurnDirector.pPtPrj, _OutDir) - TurnDirector.MagVar)))
		TextBox1203.Text = TextBox1203.Tag

		If (TurnDirector.TypeCode <> eNavaidType.VOR) And (TurnDirector.TypeCode <> eNavaidType.NDB) And (TurnDirector.TypeCode <> eNavaidType.TACAN) Then
			Return UpdateToCourse(_OutDir)
		End If

		m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)
		m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, _OutDir, TurnDir, m_BasePoints)

		Dim fTmp As Double

		fTmp = ConvertAngle(Modulus((_OutDir - _ArDir) * TurnDir))
		TextBox1201.Text = ConvertAngle(Modulus((_OutDir - _ArDir) * TurnDir)).ToString()

		If fTmp > 255 Then
			TextBox1201.ForeColor = Color.Red
		Else
			TextBox1201.ForeColor = Color.Black
		End If
		'TextBox1201.Text = CStr(Round(Modulus((OutAzt - ArDir) * TurnDir, 360.0)))
		'DrawPolygon TurnArea, 0

		mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)

		'If Not CheckBox1201.Value Then
		'        Set ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
		'        Dim pProxi As IProximityOperator
		'        Set pProxi = pCircle
		'        If pProxi.ReturnDistance(ptCur) = 0.0 Then
		'            CircleVectorIntersect CurrADHP.pPtPrj, RModel, ptCur, ptCur.M, ptTmp
		'            mPoly.AddPoint ptTmp
		'        End If
		'=====

		pPolyline = mPoly
		pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)
		pPath.AddPoint(TurnDirector.pPtPrj)
		'=====
		pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
		pPolyline.AddGeometry(pPath)
		'End If

		'    MPtCollection.Point(1).M = OutAzt
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
			'    If (CheckBox0901 ) And OptionButton0901 Then
			'        If TurnDir > 0 Then
			'            TurnArea.AddPoint pPolygon.Point(pPolygon.PointCount - 2), 0
			'        Else
			'            TurnArea.AddPoint pPolygon.Point(1), 0
			'        End If
			'    End If
		Else
			Dim I, K, Side As Integer
			'DrawPolyLine mPoly, 255, 2

			If True Then
				K = m_BasePoints.PointCount - 1
				If m_TurnArea.PointCount > 0 Then
					pLPolygon = New ESRI.ArcGIS.Geometry.Polygon
					For I = 1 To K
						Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
						If Side = TurnDir Then
							pLPolygon.AddPoint(m_BasePoints.Point(I))
						End If
					Next
					pLPolygon.AddPointCollection(m_TurnArea)
					m_TurnArea = pLPolygon
					'DrawPolygon TurnArea, 0
					ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
				Else
					ptCur = m_BasePoints.Point(0)
					For I = 1 To K
						Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
						If Side = TurnDir Then
							m_TurnArea.AddPoint(m_BasePoints.Point(I))
						End If
					Next
					m_TurnArea.AddPoint(ptCur)
				End If
			End If

			If False Then
				If m_TurnArea.PointCount > 0 Then
					m_TurnArea.AddPoint(tmpPoly1.Point(0), 0)
				Else
					m_TurnArea.AddPoint(tmpPoly1.Point(0))
					m_TurnArea.AddPoint(m_BasePoints.Point(0))
				End If
			End If

			'    k = BasePoints.PointCount - 1
			'
			'    Set pLPolygon = New Polygon
			'    For i = 1 To k
			'        Side = SideDef(ptDerPrj, DepDir, BasePoints.Point(i))
			'        If Side = TurnDir Then
			'            pLPolygon.AddPoint BasePoints.Point(i)
			'        End If
			'    Next i
			'    pLPolygon.AddPointCollection TurnArea
			'    Set TurnArea = pLPolygon
		End If

		'Set ptCur = TurnArea.Point(TurnArea.PointCount - 1)
		'AztCur = OutAzt + arafTrn_OSplay.Value * Turndir
		'
		''If OptionButton0901 Then
		''    Set tmpPoly1 = ZNR_Poly
		''Else
		'    Set tmpPoly1 = pFIXPoly
		''End If

		'curDist0 = 0.0
		'K = tmpPoly1.PointCount - 1
		'For I = 0 To K
		'    curDist = Point2LineDistancePrj(tmpPoly1.Point(I), ptCur, AztCur)
		'    If (curDist0 < curDist) Then
		'        iMax = I
		'        curDist0 = curDist
		'    End If
		'Next I
		'Set ptTmp = tmpPoly1.Point(iMax)
		'TurnArea.AddPoint ptTmp, 0
		'
		'Set pSource = TurnArea
		'Set pRPolygon = pSource.Clone
		'
		'Set tmpPoly1 = New Polygon
		'tmpPoly1.AddPoint ptTmp
		'tmpPoly1.AddPoint TurnArea.Point(TurnArea.PointCount - 1)

		'============= To FIX ============

		CalcJoiningParams(TurnDirector.TypeCode, TurnDir, m_TurnArea, m_OutPt, _OutDir)
		NewPoly = ApplayJoining(TurnDirector.TypeCode, TurnDir, m_TurnArea, m_OutPt, _OutDir)

		'    fTmp = ReturnAngleInDegrees(TurnArea.Point(TurnArea.PointCount - 1), TurnArea.Point(TurnArea.PointCount - 2))
		''================
		'    Set NewPoly = RemoveAgnails(NewPoly)

		'    Set pTopoOper = tmpPoly1
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify

		'    Set pTopoOper = pRPolygon
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify

		'    Set pTopoOper = BasePoints
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify

		''    Set pTopoOper = NewPoly

		'    Set NewPoly = pTopoOper.Union(NewPoly)
		'    Set pTopoOper = NewPoly
		'
		'    Set NewPoly = pTopoOper.Union(pRPolygon)
		'    Set pTopoOper = NewPoly

		'    If Modulus((OutAzt - ArDir) * Turndir, 360.0) > 90.0 Then
		''    If SideFrom2Angle(DepDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
		'        Set NewPoly = pTopoOper.Union(tmpPoly1)
		'        Set pTopoOper = NewPoly
		'    End If

		'    Set tmpPoly1 = New Polygon
		'    tmpPoly1.AddPointCollection TurnArea

		'    Set pTopoOper = tmpPoly1
		'    pTopoOper.IsKnownSimple_2 = False
		'    pTopoOper.Simplify

		'    Set BaseArea = RemoveHoles(pTopoOper.Union(NewPoly)) '    Set BaseArea = RemoveHoles(NewPoly)

		'DrawPolygon BaseArea, 0
		'    If OptionButton401 Then
		'        Set pTopoOper = BaseArea
		'        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
		'    End If

		pTopoOper = NewPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pTopoOper = m_BasePoints
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		BaseArea = pTopoOper.Union(NewPoly)

		pTopoOper = BaseArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		'    Set pTopoOper = pCircle
		'    Set BaseArea = pTopoOper.Intersect(BaseArea, esriGeometry2Dimension)
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
		On Error GoTo 0

		KKElem = DrawPolyLine(KK, 0, 1.0, False)
		K1K1Elem = DrawPolyLine(K1K1, 0, 1.0, False)

		DrawPoly = PolygonIntersection(pCircle, Prim)
		'    Set DrawPoly = pTopoOper.Intersect(Prim, esriGeometry2Dimension)
		PrimElem = DrawPolygon(DrawPoly, 0, , False)

		DrawPoly = PolygonIntersection(pCircle, SecL)
		'    Set DrawPoly = pTopoOper.Intersect(SecL, esriGeometry2Dimension)
		SecLElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

		DrawPoly = PolygonIntersection(pCircle, SecR)
		'    Set DrawPoly = pTopoOper.Intersect(SecR, esriGeometry2Dimension)
		SecRElem = DrawPolygon(DrawPoly, RGB(0, 0, 255), , False)

		TurnAreaElem = DrawPolygon(BaseArea, 255, , False)
		NomTrackElem = DrawPolyLine(mPoly, 0, 1.0, False)
		'DrawPolyLine mPoly, 0, 2
		'DrawPolyLine mPoly, 255, 2

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
		Dim AztCur As Double
		Dim TurnR As Double
		Dim hCalc As Double
		Dim fTASl As Double

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pLPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pSource As ESRI.ArcGIS.esriSystem.IClone

		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim DrawPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		'    Set NewFullPoly = ReArrangePolygon(pFullPoly, PtSOC, ArDir)
		'    If bTurnFIXSameMAPtFIX Then
		'        UpdateToCourse = UpdateToNavCourse(OutAzt)
		'        Exit Function
		'    End If

		pTopoOper = pMAPtArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		If OptionButton0902.Checked Then 'на Высоте
			pSource = pFullPoly
			tmpPoly1 = pSource.Clone

			tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
			tmpPoly.AddPoint(PointAlongPlane(KK.ToPoint, _ArDir - 90.0, RModel))
			tmpPoly.AddPoint(PointAlongPlane(KK.FromPoint, _ArDir + 90.0, RModel))

			CutPoly(tmpPoly1, tmpPoly, -1)
			pTopoOper = tmpPoly1

			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
			tmpPoly = pTopoOper.Union(tmpPoly1)
		Else
			tmpPoly = pTopoOper.Union(pFullPoly)
		End If

		tmpPoly = RemoveAgnails(tmpPoly)
		pRPolygon = ReArrangePolygon(tmpPoly, PtSOC, _ArDir)

		hCalc = Max(fRefHeight + arMATurnAlt.Value, TurnFixPnt.Z)
		fTASl = IAS2TAS(_missedIAS, hCalc, CurrADHP.ISAtC)
		TurnR = Bank2Radius(fBankAngle, fTASl)
		''=========  End Params

		ptTmp = PointAlongPlane(TurnFixPnt, _ArDir + 90.0 * TurnDir, TurnR)
		pt0 = PointAlongPlane(ptTmp, OutAzt - 90.0 * TurnDir, TurnR)
		pt0.M = OutAzt

		MPtCollection = New ESRI.ArcGIS.Geometry.Multipoint
		MPtCollection.AddPoint(TurnFixPnt)
		MPtCollection.AddPoint(pt0)
		mPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
		'=====
		pPolyline = mPoly
		pPath = pPolyline.Geometry(pPolyline.GeometryCount - 1)

		If OptionButton1203.Checked Then
			pPath.AddPoint(TurnDirector.pPtPrj)
		Else
			ptCur = MPtCollection.Point(MPtCollection.PointCount - 1)
			pProxi = pCircle
			If pProxi.ReturnDistance(ptCur) = 0.0 Then
				CircleVectorIntersect(FinalNav.pPtPrj, RModel, ptCur, (ptCur.M), ptTmp)
				pPath.AddPoint(ptTmp)
			End If
		End If
		'=====
		pPolyline.RemoveGeometries(pPolyline.GeometryCount - 1, 1)
		pPolyline.AddGeometry(pPath)
		'=====

		m_BasePoints = CreateBasePoints(pRPolygon, K1K1, _ArDir, TurnDir)
		m_TurnArea = CreateTurnArea(depWS.Value, TurnR, fTASl, _ArDir, OutAzt, TurnDir, m_BasePoints)

		Dim fTmp As Double = ConvertAngle(Modulus((OutAzt - _ArDir) * TurnDir))
		TextBox1201.Text = fTmp.ToString()

		If fTmp > 255 Then
			TextBox1201.ForeColor = Color.Red
		Else
			TextBox1201.ForeColor = Color.Black
		End If
		'TextBox1201.Text = CStr(Round(Modulus((OutAzt - ArDir) * TurnDir, 360.0)))

		'DrawPolygon TurnArea, RGB(0, 255, 255)
		'    If OptionButton0901 Then

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
			'        If CheckBox0901 Then
			'            If TurnDir > 0 Then
			'                TurnArea.AddPoint pFIXPoly.Point(pFIXPoly.PointCount - 2), 0
			'            Else
			'                TurnArea.AddPoint pFIXPoly.Point(1), 0
			'            End If
			'        End If
		ElseIf True Then
			Dim I, K, Side As Integer

			K = m_BasePoints.PointCount - 1

			If m_TurnArea.PointCount > 0 Then
				pLPolygon = New ESRI.ArcGIS.Geometry.Polygon
				For I = 1 To K
					Side = SideDef(PtSOC, _ArDir, m_BasePoints.Point(I))
					If Side = TurnDir Then
						pLPolygon.AddPoint(m_BasePoints.Point(I))
					End If
				Next
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
				Next
				m_TurnArea.AddPoint(ptCur)
			End If
		Else
			ptCur = m_TurnArea.Point(m_TurnArea.PointCount - 1)
		End If

		'DrawPolygon BasePoints, 0
		'DrawPoint TurnArea.Point(0), 0
		'msgbox "TurnArea.PointCount = " + CStr(TurnArea.PointCount)
		'DrawPolygon TurnArea, 0

		AztCur = OutAzt + arafTrn_OSplay.Value * TurnDir
		ptTmp = GetNearPoint(OutAzt, TurnDir, TurnFixPnt, OutAzt + arafTrn_ISplay.Value * TurnDir)

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
		SecPoly = New ESRI.ArcGIS.Geometry.Polygon
		SecPoly0 = New ESRI.ArcGIS.Geometry.Polygon

		'    If Modulus((OutAzt - ArDir) * Turndir, 360.0) > 90.0 Then
		'    If SideFrom2Angle(DepDir + 90.0 * TurnDir, fTmp) * TurnDir < 0 Then
		BaseArea = pTopoOper.Union(tmpPoly1)
		'    End If

		pTopoOper = BaseArea
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		tmpPoly1 = New ESRI.ArcGIS.Geometry.Polygon
		tmpPoly1.AddPointCollection(m_TurnArea)

		pTopoOper = tmpPoly1
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		tmpPoly1 = pTopoOper.Union(BaseArea)
		BaseArea = RemoveHoles(tmpPoly1)

		'    If OptionButton1201 Then
		'        Set pTopoOper = BaseArea
		''        Set BaseArea = pTopoOper.Difference(ZNR_Poly)
		'    End If

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
		If Not SecRElem Is Nothing Then pGraphics.DeleteElement(SecRElem)
		If Not SecLElem Is Nothing Then pGraphics.DeleteElement(SecLElem)
		If Not Sec0Elem Is Nothing Then pGraphics.DeleteElement(Sec0Elem)
		If Not NomTrackElem Is Nothing Then pGraphics.DeleteElement(NomTrackElem)
		On Error GoTo 0

		PrimElem = Nothing
		SecRElem = Nothing
		SecLElem = Nothing
		Sec0Elem = Nothing

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

	'Private Function BaseTurnByDistance(ByRef Hhold As Double, ByRef T As Double, ByRef ArDir As Double, ByRef InterAngle As Double, ByRef Turn As Integer, ByRef IAS As Double, ByRef MaxR As Double, ByRef ptTrack As IPointCollection, Optional ByRef drawFlg As Boolean = True) As Integer
	'	Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'	Dim pClone As ESRI.ArcGIS.esriSystem.IClone

	'	Dim pHalfCircle1 As ESRI.ArcGIS.Geometry.IPolygon
	'	Dim pHalfCircle2 As ESRI.ArcGIS.Geometry.IPolygon
	'	Dim pApproachCircle As ESRI.ArcGIS.Geometry.IPolygon

	'	Dim Shablon As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim NavArea As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pCutter As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

	'	Dim pPtTmp As ESRI.ArcGIS.Geometry.Point
	'	Dim pPt0 As ESRI.ArcGIS.Geometry.Point
	'	Dim pPt1 As ESRI.ArcGIS.Geometry.Point

	'	Dim iTurnDir As Integer
	'	Dim Result As Integer

	'	Dim fAxis As Double
	'	Dim fDist As Double
	'	Dim fCource As Double
	'	Dim fApproach As Double
	'	Dim cooNav As NavaidData

	'	If drawFlg Then
	'		On Error Resume Next
	'		If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
	'		If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
	'		If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
	'		If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
	'		On Error GoTo 0
	'	End If

	'	fAxis = Modulus(ArDir + 180.0, 360.0)

	'	cooNav = DMEList(FinalNav.PairNavaidIndex)
	'	Result = ReversalShablonByDistance(FinalNav.pPtPrj, cooNav.pPtPrj, FinalNav.TypeCode, T, IAS, Hhold, CurrADHP.ISAtC, fAxis, InterAngle, Turn, Shablon, NavArea, ptTrack)
	'	If Result < 1 Then Return Result

	'	fCource = Modulus(ptTrack.Point(0).M + 180.0, 360.0)

	'	pClone = Shablon
	'	BaseTurnArea = pClone.Clone

	'	pTopo = BaseTurnArea
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	pApproachCircle = CreatePrjCircle(FinalNav.pPtPrj, arHoldingBuffer.Value)

	'	Dim pMultiPoint As ESRI.ArcGIS.Geometry.IPointCollection
	'	If ComboBox0602.SelectedIndex > 0 Then
	'		If ComboBox0602.SelectedIndex = 1 Then
	'			BaseTurnArea = pTopo.Union(pApproachCircle)
	'			pTopo = BaseTurnArea

	'			pTopo.IsKnownSimple_2 = False
	'			pTopo.Simplify()
	'			BaseTurnArea = pTopo.ConvexHull
	'		Else
	'			iTurnDir = (5 - 2 * ComboBox0602.SelectedIndex)
	'			fApproach = fCource + CDbl(ComboBox0603.Text) * iTurnDir

	'			pCutter = New ESRI.ArcGIS.Geometry.Polyline
	'			pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource, 200000.0))
	'			pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource + 180.0, 200000.0))

	'			If ComboBox0602.SelectedIndex = 2 Then
	'				ClipByLine(pApproachCircle, pCutter, pHalfCircle1, pHalfCircle2, Nothing)
	'			ElseIf ComboBox0602.SelectedIndex = 3 Then
	'				ClipByLine(pApproachCircle, pCutter, pHalfCircle2, pHalfCircle1, Nothing)
	'			End If

	'			pCutter.RemovePoints(0, 2)
	'			pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0, 200000.0))
	'			pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0, 200000.0))
	'			ClipByLine(pHalfCircle1, pCutter, pTmpPoly, Nothing, Nothing)

	'			BaseTurnArea = pTopo.Union(pTmpPoly)
	'			pTopo = BaseTurnArea
	'			pTopo.IsKnownSimple_2 = False
	'			pTopo.Simplify()

	'			pTmpPoly = New ESRI.ArcGIS.Geometry.Multipoint
	'			pTmpPoly.AddPointCollection(BaseTurnArea)
	'			pTopo = pTmpPoly

	'			BaseTurnArea = pTopo.ConvexHull
	'			pTopo = BaseTurnArea
	'			pTopo.IsKnownSimple_2 = False
	'			pTopo.Simplify()

	'			pPt0 = PointAlongPlane(FinalNav.pPtPrj, fApproach + iTurnDir * 90.0, 2 * arHoldingBuffer.Value)
	'			pPt1 = FindCommonTochCircle(BaseTurnArea, fApproach, arHoldingBuffer.Value, iTurnDir, pPt0)

	'			If System.Math.Abs(ReturnDistanceInMeters(pPt1, pPt0) - arHoldingBuffer.Value) > 0.3 Then
	'				If iTurnDir * Turn < 0 Then
	'					ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
	'				Else
	'					ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
	'				End If

	'				pTopo = pTmpPoly

	'				pTmpPoly = pTopo.Union(BaseTurnArea)
	'				pMultiPoint = New ESRI.ArcGIS.Geometry.Multipoint
	'				pMultiPoint.AddPointCollection(pTmpPoly)
	'				pTopo = pMultiPoint

	'				pTmpPoly = pTopo.ConvexHull
	'				pTopo = pTmpPoly
	'				pTopo.IsKnownSimple_2 = False
	'				pTopo.Simplify()

	'				BaseTurnArea = pTopo.Union(pApproachCircle)

	'				pTopo = BaseTurnArea
	'				pTopo.IsKnownSimple_2 = False
	'				pTopo.Simplify()
	'				fDist = 0.0

	'				pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon

	'				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)
	'				'            Set pPtTmp = PointAlongPlane(pPt1, fApproach, arHoldingBuffer.Value)
	'				'            pTmpPoly.AddPoint pPtTmp
	'				pTmpPoly.AddPoint(pPt1)
	'			Else
	'				fAxis = ReturnAngleInDegrees(pPt0, FinalNav.pPtPrj)

	'				pCutter.RemovePoints(0, 2)
	'				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis, 200000.0))
	'				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis + 180.0, 200000.0))

	'				If ComboBox0602.SelectedIndex = 2 Then
	'					ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
	'				ElseIf ComboBox0602.SelectedIndex = 3 Then
	'					ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
	'				End If

	'				BaseTurnArea = pTopo.Union(pTmpPoly)
	'				pTopo = BaseTurnArea
	'				pTopo.IsKnownSimple_2 = False
	'				pTopo.Simplify()

	'				fDist = Point2LineDistancePrj(pPt0, FinalNav.pPtPrj, fApproach - 90.0) * SideDef(FinalNav.pPtPrj, fApproach + 90.0, pPt0)

	'				pPtTmp = PointAlongPlane(pPt0, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
	'				pTmpPoly = CreateArcPrj(pPt0, pPtTmp, pPt1, -iTurnDir)
	'				pTmpPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource + 180.0, arHoldingBuffer.Value))
	'			End If

	'			pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)

	'			If fDist < arHoldingBuffer.Value Then
	'				fDist = arHoldingBuffer.Value
	'				pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)
	'			End If

	'			pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
	'			pTmpPoly.AddPoint(pPt1)
	'			pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist))	', 0
	'			'===========================================================

	'			pTopo = pTmpPoly
	'			pTopo.IsKnownSimple_2 = False
	'			pTopo.Simplify()

	'			BaseTurnArea = pTopo.Union(BaseTurnArea)
	'		End If
	'	Else
	'		pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon
	'		pTmpPoly.AddPoint(ptTrack.Point(ptTrack.PointCount - 2))
	'		pTmpPoly.AddPoint(PointAlongPlane(ptTrack.Point(ptTrack.PointCount - 2), fCource - 90.0 * Turn, arHoldingBuffer.Value))

	'		pPt0 = PointAlongPlane(FinalNav.pPtPrj, fCource - 90.0 * Turn, arHoldingBuffer.Value)
	'		pPt1 = PointAlongPlane(FinalNav.pPtPrj, fCource + 90.0 * Turn, arHoldingBuffer.Value)

	'		pTmpPoly.AddPoint(pPt0)
	'		pTmpPoly.AddPoint(pPt1)

	'		pTopo = pTmpPoly
	'		pTopo.IsKnownSimple_2 = False
	'		pTopo.Simplify()

	'		BaseTurnArea = pTopo.Union(BaseTurnArea)

	'		pTopo = BaseTurnArea
	'		pTopo.IsKnownSimple_2 = False
	'		pTopo.Simplify()

	'		BaseTurnArea = pTopo.ConvexHull

	'		pTopo = BaseTurnArea
	'		pTopo.IsKnownSimple_2 = False
	'		pTopo.Simplify()

	'		pTmpPoly.RemovePoints(0, pTmpPoly.PointCount)

	'		pTmpPoly.AddPoint(ptTrack.Point(ptTrack.PointCount - 2))

	'		pTmpPoly.AddPoint(pPt0)
	'		pTmpPoly.AddPoint(PointAlongPlane(pPt0, fCource, arHoldingBuffer.Value))
	'		pTmpPoly.AddPoint(PointAlongPlane(pPt1, fCource, arHoldingBuffer.Value))
	'		pTmpPoly.AddPoint(pPt1)

	'		pTopo = pTmpPoly
	'		pTopo.IsKnownSimple_2 = False
	'		pTopo.Simplify()

	'		BaseTurnArea = pTopo.Union(BaseTurnArea)
	'	End If

	'	pTopo = BaseTurnArea
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	BufferedBaseTurnArea = pTopo.Buffer(arHoldingBuffer.Value)
	'	pTopo = BufferedBaseTurnArea
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	pApproachCircle = Nothing
	'	pHalfCircle1 = Nothing
	'	pHalfCircle2 = Nothing
	'	pCutter = Nothing
	'	pTmpPoly = Nothing
	'	pPtTmp = Nothing
	'	pPt0 = Nothing
	'	pPt1 = Nothing

	'	If drawFlg Then
	'		IIAreaElement = DrawPolygon(BufferedBaseTurnArea, RGB(255, 0, 255))
	'		IAreaElement = DrawPolygon(BaseTurnArea, 0)
	'		NavAreaElement = DrawPolygon(NavArea, 255)

	'		trBaseTurn = CalcTrajectoryFromMultiPoint(ptTrack)
	'		TracElement = DrawPolyLine(trBaseTurn, 0, 1)

	'		NavAreaElement.Locked = True
	'		IIAreaElement.Locked = True
	'		IAreaElement.Locked = True
	'		TracElement.Locked = True
	'	End If

	'	Return Result
	'End Function

	Private Function BaseTurn(ByRef Hhold As Double, ByRef T As Double, ByRef ArDir As Double, ByRef InterAngle As Double, ByRef Turn As Integer, ByRef IAS As Double, ByRef MaxR As Double, ByRef ptTrack As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByRef drawFlg As Boolean = True) As Integer
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		Dim pHalfCircle1 As ESRI.ArcGIS.Geometry.IPolygon
		Dim pHalfCircle2 As ESRI.ArcGIS.Geometry.IPolygon
		Dim pApproachCircle As ESRI.ArcGIS.Geometry.IPolygon

		Dim Shablon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim NavArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutter As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pPtTmp As ESRI.ArcGIS.Geometry.Point
		Dim pPt0 As ESRI.ArcGIS.Geometry.Point
		Dim pPt1 As ESRI.ArcGIS.Geometry.Point

		Dim iTurnDir As Integer
		Dim Result As Integer

		Dim fAxis As Double
		Dim fDist As Double
		Dim fCource As Double
		Dim fApproach As Double
		Dim cooNav As NavaidData

		If drawFlg Then
			On Error Resume Next
			If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
			If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
			If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
			If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
			On Error GoTo 0
		End If

		fAxis = Modulus(ArDir + 180.0, 360.0)

		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			cooNav = DMEList(FinalNav.PairNavaidIndex)
			Result = ReversalShablonByDistance(FinalNav.pPtPrj, cooNav.pPtPrj, FinalNav.TypeCode, T, IAS, Hhold, CurrADHP.ISAtC, fAxis, InterAngle, Turn, Shablon, NavArea, ptTrack)
		Else
			Result = ReversalShablon(FinalNav.pPtPrj, IAS, Hhold, CurrADHP.ISAtC, T, fAxis, InterAngle, Turn, FinalNav.TypeCode, Shablon, NavArea, ptTrack)
		End If

		If Result < 1 Then Return Result

		fCource = Modulus(ptTrack.Point(0).M + 180.0, 360.0)

		pClone = Shablon
		BaseTurnArea = pClone.Clone

		pTopo = BaseTurnArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pApproachCircle = CreatePrjCircle(FinalNav.pPtPrj, arHoldingBuffer.Value)

		Dim pMultiPoint As ESRI.ArcGIS.Geometry.IPointCollection
		If ComboBox0602.SelectedIndex > 0 Then
			If ComboBox0602.SelectedIndex = 1 Then
				BaseTurnArea = pTopo.Union(pApproachCircle)
				pTopo = BaseTurnArea

				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				BaseTurnArea = pTopo.ConvexHull
			Else
				iTurnDir = (5 - 2 * ComboBox0602.SelectedIndex)
				fApproach = fCource + CDbl(ComboBox0603.Text) * iTurnDir

				pCutter = New ESRI.ArcGIS.Geometry.Polyline
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource, 200000.0))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource + 180.0, 200000.0))

				If ComboBox0602.SelectedIndex = 2 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle1, pHalfCircle2, Nothing)
				ElseIf ComboBox0602.SelectedIndex = 3 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle2, pHalfCircle1, Nothing)
				End If

				pCutter.RemovePoints(0, 2)
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0, 200000.0))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0, 200000.0))
				ClipByLine(pHalfCircle1, pCutter, pTmpPoly, Nothing, Nothing)

				BaseTurnArea = pTopo.Union(pTmpPoly)
				pTopo = BaseTurnArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTmpPoly = New ESRI.ArcGIS.Geometry.Multipoint
				pTmpPoly.AddPointCollection(BaseTurnArea)
				pTopo = pTmpPoly

				BaseTurnArea = pTopo.ConvexHull
				pTopo = BaseTurnArea
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPt0 = PointAlongPlane(FinalNav.pPtPrj, fApproach + iTurnDir * 90.0, 2 * arHoldingBuffer.Value)
				pPt1 = FindCommonTochCircle(BaseTurnArea, fApproach, arHoldingBuffer.Value, iTurnDir, pPt0)

				If System.Math.Abs(ReturnDistanceInMeters(pPt1, pPt0) - arHoldingBuffer.Value) > 0.3 Then
					If iTurnDir * Turn < 0 Then
						ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
					Else
						ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
					End If

					pTopo = pTmpPoly

					pTmpPoly = pTopo.Union(BaseTurnArea)
					pMultiPoint = New ESRI.ArcGIS.Geometry.Multipoint
					pMultiPoint.AddPointCollection(pTmpPoly)
					pTopo = pMultiPoint

					pTmpPoly = pTopo.ConvexHull
					pTopo = pTmpPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					BaseTurnArea = pTopo.Union(pApproachCircle)

					pTopo = BaseTurnArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
					fDist = 0.0

					pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon

					pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)
					'            Set pPtTmp = PointAlongPlane(pPt1, fApproach, arHoldingBuffer.Value)
					'            pTmpPoly.AddPoint pPtTmp
					pTmpPoly.AddPoint(pPt1)
				Else
					fAxis = ReturnAngleInDegrees(pPt0, FinalNav.pPtPrj)

					pCutter.RemovePoints(0, 2)
					pCutter.AddPoint(PointAlongPlane(pPt0, fAxis, 200000.0))
					pCutter.AddPoint(PointAlongPlane(pPt0, fAxis + 180.0, 200000.0))

					If ComboBox0602.SelectedIndex = 2 Then
						ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
					ElseIf ComboBox0602.SelectedIndex = 3 Then
						ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
					End If

					BaseTurnArea = pTopo.Union(pTmpPoly)
					pTopo = BaseTurnArea
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					fDist = Point2LineDistancePrj(pPt0, FinalNav.pPtPrj, fApproach - 90.0) * SideDef(FinalNav.pPtPrj, fApproach + 90.0, pPt0)

					pPtTmp = PointAlongPlane(pPt0, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
					pTmpPoly = CreateArcPrj(pPt0, pPtTmp, pPt1, -iTurnDir)
					pTmpPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fCource + 180.0, arHoldingBuffer.Value))
				End If

				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)

				If fDist < arHoldingBuffer.Value Then
					fDist = arHoldingBuffer.Value
					pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)
				End If

				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
				pTmpPoly.AddPoint(pPt1)
				pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist))  ', 0
				'===========================================================

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				BaseTurnArea = pTopo.Union(BaseTurnArea)
			End If
		Else
			pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon
			pTmpPoly.AddPoint(ptTrack.Point(ptTrack.PointCount - 2))
			pTmpPoly.AddPoint(PointAlongPlane(ptTrack.Point(ptTrack.PointCount - 2), fCource - 90.0 * Turn, arHoldingBuffer.Value))

			pPt0 = PointAlongPlane(FinalNav.pPtPrj, fCource - 90.0 * Turn, arHoldingBuffer.Value)
			pPt1 = PointAlongPlane(FinalNav.pPtPrj, fCource + 90.0 * Turn, arHoldingBuffer.Value)

			pTmpPoly.AddPoint(pPt0)
			pTmpPoly.AddPoint(pPt1)

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			BaseTurnArea = pTopo.Union(BaseTurnArea)

			pTopo = BaseTurnArea
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			BaseTurnArea = pTopo.ConvexHull

			pTopo = BaseTurnArea
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTmpPoly.RemovePoints(0, pTmpPoly.PointCount)

			pTmpPoly.AddPoint(ptTrack.Point(ptTrack.PointCount - 2))

			pTmpPoly.AddPoint(pPt0)
			pTmpPoly.AddPoint(PointAlongPlane(pPt0, fCource, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(PointAlongPlane(pPt1, fCource, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(pPt1)

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			BaseTurnArea = pTopo.Union(BaseTurnArea)
		End If

		pTopo = BaseTurnArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		BufferedBaseTurnArea = pTopo.Buffer(arHoldingBuffer.Value)
		pTopo = BufferedBaseTurnArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pApproachCircle = Nothing
		pHalfCircle1 = Nothing
		pHalfCircle2 = Nothing
		pCutter = Nothing
		pTmpPoly = Nothing
		pPtTmp = Nothing
		pPt0 = Nothing
		pPt1 = Nothing

		If drawFlg Then
			IIAreaElement = DrawPolygon(BufferedBaseTurnArea, RGB(255, 0, 255))
			IAreaElement = DrawPolygon(BaseTurnArea, 0)
			NavAreaElement = DrawPolygon(NavArea, 255)

			trBaseTurn = CalcTrajectoryFromMultiPoint(ptTrack)
			TracElement = DrawPolyLine(trBaseTurn, 0, 1)

			NavAreaElement.Locked = True
			IIAreaElement.Locked = True
			IAreaElement.Locked = True
			TracElement.Locked = True
		End If

		Return Result
	End Function

	Private Function Standart45_180(ByRef Hhold As Double, ByRef T As Double, ByRef ArDir As Double, ByRef Turn As Integer, ByRef IAS As Double, ByRef MaxR As Double, ByRef ptTurn45_180 As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByRef drawFlg As Boolean = True) As Integer
		Dim ptStandart45_180Area As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pHalfCircle1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pHalfCircle2 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D
		Dim pNavArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim LeftPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim NavArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Shablon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutter As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pApproachCircle As ESRI.ArcGIS.Geometry.IPolygon
		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint

		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim Ix As Integer
		Dim iTurnDir As Integer
		Dim lT As Double
		Dim fAxis As Double
		Dim fDist As Double
		Dim fApproach As Double

		If drawFlg Then
			On Error Resume Next
			If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
			If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
			If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
			If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
			On Error GoTo 0
		End If
		lT = T * 60.0
		fAxis = Modulus(ArDir + 180.0, 360.0)

		Standart45_180 = Shablon45_180(FinalNav.pPtPrj, IAS, Hhold, CurrADHP.ISAtC, fAxis, Turn, ComboBox0001.SelectedIndex, Shablon, ptTurn45_180, Ix) '

		If Standart45_180 < 1 Then Exit Function
		'DrawPolygon Shablon, 0

		NavArea = CreateReckoningToleranceArea(lT, fAxis, Hhold, IAS, CurrADHP.ISAtC, FinalNav.pPtPrj, FinalNav.TypeCode)
		N = NavArea.PointCount - 1
		pNavArea = New ESRI.ArcGIS.Geometry.Polygon
		pNavArea.AddPointCollection(NavArea)
		pNavArea.RemovePoints(N, 1)

		pTopo = pNavArea
		pTopo.Simplify()

		pTransform2D = ptTurn45_180
		pTransform2D.Move(NavArea.Point(N).X - ptTurn45_180.Point(0).X, NavArea.Point(N).Y - ptTurn45_180.Point(0).Y)

		ptTurn45_180.AddPoint(FinalNav.pPtPrj, 0)
		ptTurn45_180.Point(0).M = fAxis

		pTransform2D = Shablon
		pTransform2D.Move(NavArea.Point(0).X - Shablon.Point(0).X, NavArea.Point(0).Y - Shablon.Point(0).Y)

		ptStandart45_180Area = New ESRI.ArcGIS.Geometry.Multipoint
		ptStandart45_180Area.AddPointCollection(Shablon)

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		For I = 1 To N - 1
			pTransform2D.Move(NavArea.Point(I).X - NavArea.Point(I - 1).X, NavArea.Point(I).Y - NavArea.Point(I - 1).Y)
			ptStandart45_180Area.AddPointCollection(Shablon)

			If I = (3 - Turn) \ 2 Then
				For J = 0 To Ix + 45
					pCutter.AddPoint(Shablon.Point(J))
				Next J
			End If
		Next

		ptStandart45_180Area.AddPoint(FinalNav.pPtPrj)

		pTopo = ptStandart45_180Area
		tmpPoly = pTopo.ConvexHull

		pCutter.AddPoint(PointAlongPlane(pCutter.Point(Ix + 45), ArDir + 90.0 * Turn, 200.0))
		pCutter.AddPoint(PointAlongPlane(pCutter.Point(0), ArDir, 200.0), 0)

		pTopo = tmpPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pRelation = tmpPoly
		If Not pRelation.Contains(pCutter.Point(0)) Then
			pTopo.Cut(pCutter, LeftPoly, Standart45_180Area)
			If (Standart45_180Area.PointCount < LeftPoly.PointCount) Then
				Standart45_180Area = LeftPoly
			End If
		Else
			Standart45_180Area = tmpPoly
		End If

		pTopo = Standart45_180Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pCutter.RemovePoints(0, pCutter.PointCount)
		pApproachCircle = CreatePrjCircle(FinalNav.pPtPrj, arHoldingBuffer.Value)

		If ComboBox0602.SelectedIndex > 0 Then
			If ComboBox0602.SelectedIndex = 1 Then
				Standart45_180Area = pTopo.Union(pApproachCircle)
				pTmpPoly = New ESRI.ArcGIS.Geometry.Multipoint
				pTmpPoly.AddPointCollection(Standart45_180Area)

				pTopo = pTmpPoly
				Standart45_180Area = pTopo.ConvexHull
			Else
				iTurnDir = (5 - 2 * ComboBox0602.SelectedIndex)
				fApproach = ArDir + CDbl(ComboBox0603.Text) * iTurnDir

				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir, 200000))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, 200000))

				If ComboBox0602.SelectedIndex = 2 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle1, pHalfCircle2, Nothing)
				ElseIf ComboBox0602.SelectedIndex = 3 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle2, pHalfCircle1, Nothing)
				End If

				pCutter.RemovePoints(0, pCutter.PointCount)
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0, 200000))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0, 200000))

				ClipByLine(pHalfCircle1, pCutter, pTmpPoly, Nothing, Nothing)

				Standart45_180Area = pTopo.Union(pTmpPoly)
				pTopo = Standart45_180Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pTmpPoly = New ESRI.ArcGIS.Geometry.Multipoint
				pTmpPoly.AddPointCollection(Standart45_180Area)
				pTopo = pTmpPoly

				Standart45_180Area = pTopo.ConvexHull
				pTopo = Standart45_180Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPt0 = PointAlongPlane(FinalNav.pPtPrj, fApproach + iTurnDir * 90.0, 2 * arHoldingBuffer.Value)
				pPt1 = FindCommonTochCircle(Standart45_180Area, fApproach, arHoldingBuffer.Value, iTurnDir, pPt0)

				fAxis = ReturnAngleInDegrees(pPt0, FinalNav.pPtPrj)

				pCutter.RemovePoints(0, pCutter.PointCount)
				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis, 200000.0))
				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis + 180.0, 200000.0))

				If ComboBox0602.SelectedIndex = 2 Then
					ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
				ElseIf ComboBox0602.SelectedIndex = 3 Then
					ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
				End If

				Standart45_180Area = pTopo.Union(pTmpPoly)
				pTopo = Standart45_180Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				fDist = Point2LineDistancePrj(pPt0, FinalNav.pPtPrj, fApproach - 90.0) * SideDef(FinalNav.pPtPrj, fApproach + 90.0, pPt0)

				pPtTmp = PointAlongPlane(pPt0, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
				pTmpPoly = CreateArcPrj(pPt0, pPtTmp, pPt1, -iTurnDir)
				pTmpPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, arHoldingBuffer.Value))

				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)

				If fDist < arHoldingBuffer.Value Then
					fDist = arHoldingBuffer.Value
					pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)
				End If

				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
				pTmpPoly.AddPoint(pPt1)
				pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
				Standart45_180Area = pTopo.Union(Standart45_180Area)
			End If
		Else
			pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon
			pPt1 = PointAlongPlane(FinalNav.pPtPrj, ArDir, arHoldingBuffer.Value)

			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir - 90.0, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir + 90.0, arHoldingBuffer.Value))

			pPt0 = ptTurn45_180.Point(ptTurn45_180.PointCount - 1)

			fDist = ReturnDistanceInMeters(FinalNav.pPtPrj, pPt0)
			pPt1 = PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, fDist)

			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir + 90.0, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir - 90.0, arHoldingBuffer.Value))

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			Standart45_180Area = pTopo.Union(Standart45_180Area)
		End If

		pTopo = Standart45_180Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		BufferedStandart45_180Area = pTopo.Buffer(arHoldingBuffer.Value)
		pTopo = BufferedStandart45_180Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pApproachCircle = Nothing
		pHalfCircle1 = Nothing
		pHalfCircle2 = Nothing
		pCutter = Nothing
		pTmpPoly = Nothing
		pPtTmp = Nothing
		pPt0 = Nothing
		pPt1 = Nothing

		If drawFlg Then
			IIAreaElement = DrawPolygon(BufferedStandart45_180Area, RGB(255, 0, 255))
			IAreaElement = DrawPolygon(Standart45_180Area, 0)
			NavAreaElement = DrawPolygon(pNavArea, 255)

			trTurn45_180 = CalcTrajectoryFromMultiPoint(ptTurn45_180)
			TracElement = DrawPolyLine(trTurn45_180, 0, 1)

			NavAreaElement.Locked = True
			IIAreaElement.Locked = True
			IAreaElement.Locked = True
			TracElement.Locked = True
		End If
	End Function

	Private Function Standart80_260(ByRef Hhold As Double, ByRef T As Double, ByRef ArDir As Double, ByRef Turn As Integer, ByRef IAS As Double, ByRef MaxR As Double, ByRef ptTurn80_260 As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByRef drawFlg As Boolean = True) As Integer
		Dim ptStandart80_260Area As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pNavArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim NavArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Shablon As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pApproachCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pHalfCircle1 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pHalfCircle2 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutter As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint

		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D
		Dim I As Integer
		Dim N As Integer
		Dim iTurnDir As Integer

		Dim fAxis As Double
		Dim fDist As Double
		Dim fApproach As Double

		If drawFlg Then
			On Error Resume Next
			If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
			If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
			If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
			If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
			On Error GoTo 0
		End If

		fAxis = Modulus(ArDir + 180.0, 360.0)
		Standart80_260 = Shablon80_260(FinalNav.pPtPrj, IAS, Hhold, CurrADHP.ISAtC, fAxis, Turn, Shablon, ptTurn80_260)

		If Standart80_260 < 1 Then Exit Function

		NavArea = CreateReckoningToleranceArea(T * 60.0, fAxis, Hhold, IAS, CurrADHP.ISAtC, FinalNav.pPtPrj, FinalNav.TypeCode)
		N = NavArea.PointCount - 1

		pNavArea = New ESRI.ArcGIS.Geometry.Polygon

		pNavArea.AddPointCollection(NavArea)
		pNavArea.RemovePoints(N, 1)
		pTopo = pNavArea
		pTopo.Simplify()

		pTransform2D = ptTurn80_260
		pTransform2D.Move(NavArea.Point(N).X - ptTurn80_260.Point(0).X, NavArea.Point(N).Y - ptTurn80_260.Point(0).Y)

		ptTurn80_260.AddPoint(FinalNav.pPtPrj, 0)
		ptTurn80_260.Point(0).M = fAxis

		pTransform2D = Shablon
		pTransform2D.Move(NavArea.Point(0).X - Shablon.Point(0).X, NavArea.Point(0).Y - Shablon.Point(0).Y)

		ptStandart80_260Area = New ESRI.ArcGIS.Geometry.Multipoint
		ptStandart80_260Area.AddPointCollection(Shablon)

		For I = 1 To N - 1
			pTransform2D.Move(NavArea.Point(I).X - NavArea.Point(I - 1).X, NavArea.Point(I).Y - NavArea.Point(I - 1).Y)
			ptStandart80_260Area.AddPointCollection(Shablon)
		Next
		ptStandart80_260Area.AddPoint(FinalNav.pPtPrj)

		pTopo = ptStandart80_260Area
		Standart80_260Area = pTopo.ConvexHull

		pTopo = Standart80_260Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pApproachCircle = CreatePrjCircle(FinalNav.pPtPrj, arHoldingBuffer.Value)

		If ComboBox0602.SelectedIndex > 0 Then
			If ComboBox0602.SelectedIndex = 1 Then
				Standart80_260Area = pTopo.Union(pApproachCircle)

				pTmpPoly = New ESRI.ArcGIS.Geometry.Multipoint
				pTmpPoly.AddPointCollection(Standart80_260Area)
				pTopo = pTmpPoly

				Standart80_260Area = pTopo.ConvexHull
			Else
				iTurnDir = (5 - 2 * ComboBox0602.SelectedIndex)
				fApproach = ArDir + CDbl(ComboBox0603.Text) * iTurnDir

				pCutter = New ESRI.ArcGIS.Geometry.Polyline
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir, 200000))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, 200000))

				If ComboBox0602.SelectedIndex = 2 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle1, pHalfCircle2, Nothing)
				ElseIf ComboBox0602.SelectedIndex = 3 Then
					ClipByLine(pApproachCircle, pCutter, pHalfCircle2, pHalfCircle1, Nothing)
				End If

				pCutter.RemovePoints(0, 2)
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0, 200000))
				pCutter.AddPoint(PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0, 200000))

				ClipByLine(pHalfCircle1, pCutter, pTmpPoly, Nothing, Nothing)

				Standart80_260Area = pTopo.Union(pTmpPoly)
				pTopo = Standart80_260Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				Standart80_260Area = pTopo.ConvexHull
				pTopo = Standart80_260Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				pPt0 = PointAlongPlane(FinalNav.pPtPrj, fApproach + iTurnDir * 90.0, 2 * arHoldingBuffer.Value)
				pPt1 = FindCommonTochCircle(Standart80_260Area, fApproach, arHoldingBuffer.Value, iTurnDir, pPt0)

				fAxis = ReturnAngleInDegrees(pPt0, FinalNav.pPtPrj)

				pCutter.RemovePoints(0, 2)
				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis, 200000.0))
				pCutter.AddPoint(PointAlongPlane(pPt0, fAxis + 180.0, 200000.0))

				If ComboBox0602.SelectedIndex = 2 Then
					ClipByLine(pHalfCircle2, pCutter, pTmpPoly, Nothing, Nothing)
				ElseIf ComboBox0602.SelectedIndex = 3 Then
					ClipByLine(pHalfCircle2, pCutter, Nothing, pTmpPoly, Nothing)
				End If

				Standart80_260Area = pTopo.Union(pTmpPoly)
				pTopo = Standart80_260Area
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				fDist = Point2LineDistancePrj(pPt0, FinalNav.pPtPrj, fApproach - 90.0) * SideDef(FinalNav.pPtPrj, fApproach + 90.0, pPt0)

				pPtTmp = PointAlongPlane(pPt0, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
				pTmpPoly = CreateArcPrj(pPt0, pPtTmp, pPt1, -iTurnDir)

				pTmpPoly.AddPoint(PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, arHoldingBuffer.Value))
				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach + 90.0 * iTurnDir, arHoldingBuffer.Value)

				If fDist < arHoldingBuffer.Value Then
					fDist = arHoldingBuffer.Value
					pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)
				End If

				pPt1 = PointAlongPlane(FinalNav.pPtPrj, fApproach - 90.0 * iTurnDir, arHoldingBuffer.Value)
				pTmpPoly.AddPoint(pPt1)
				pTmpPoly.AddPoint(PointAlongPlane(pPt1, fApproach, fDist), 0)

				pTopo = pTmpPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				Standart80_260Area = pTopo.Union(Standart80_260Area)
			End If
		Else
			pTmpPoly = New ESRI.ArcGIS.Geometry.Polygon
			pPt1 = PointAlongPlane(FinalNav.pPtPrj, ArDir, arHoldingBuffer.Value)

			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir - 90.0, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir + 90.0, arHoldingBuffer.Value))

			pPt0 = ptTurn80_260.Point(ptTurn80_260.PointCount - 1)

			fDist = ReturnDistanceInMeters(FinalNav.pPtPrj, pPt0)
			pPt1 = PointAlongPlane(FinalNav.pPtPrj, ArDir + 180.0, fDist)

			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir + 90.0, arHoldingBuffer.Value))
			pTmpPoly.AddPoint(PointAlongPlane(pPt1, ArDir - 90.0, arHoldingBuffer.Value))

			pTopo = pTmpPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			Standart80_260Area = pTopo.Union(Standart80_260Area)
		End If

		pTopo = Standart80_260Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		BufferedStandart80_260Area = pTopo.Buffer(arHoldingBuffer.Value)
		pTopo = BufferedStandart80_260Area
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pApproachCircle = Nothing
		pHalfCircle1 = Nothing
		pHalfCircle2 = Nothing
		pCutter = Nothing
		pTmpPoly = Nothing
		pPtTmp = Nothing
		pPt0 = Nothing
		pPt1 = Nothing

		If drawFlg Then
			IIAreaElement = DrawPolygon(BufferedStandart80_260Area, RGB(255, 0, 255))
			IAreaElement = DrawPolygon(Standart80_260Area, 0)
			NavAreaElement = DrawPolygon(pNavArea, 255)

			trTurn80_260 = CalcTrajectoryFromMultiPoint(ptTurn80_260)
			TracElement = DrawPolyLine(trTurn80_260, 0, 1)

			NavAreaElement.Locked = True
			IIAreaElement.Locked = True
			IAreaElement.Locked = True
			TracElement.Locked = True
		End If
	End Function

	Private Sub Ipodrom(ByRef Hhold As Double, ByRef T As Double, ByVal ArDir As Double, ByRef Turn As Integer, ByRef IAS As Double, ByRef MaxR As Double, ByRef ptHoldingArea As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByRef drawFlg As Boolean = True)
		Dim trRotatedTurn180 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptInpHoldArea67 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptBaseHoldArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptInpHoldArea8 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim InpHoldArea8 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim InpHoldArea5 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim BaseHoldArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pClipPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim trTurn180 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim TolerArea As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Shablon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptLine3 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim trLine3 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pArc675 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptArea As ESRI.ArcGIS.Geometry.IPointCollection

		Dim LTrackingTolerLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim RTrackingTolerLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pDrawingShablon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ShiftedRLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPolyline As ESRI.ArcGIS.Geometry.IPolyline
		Dim pCutLine As ESRI.ArcGIS.Geometry.IPolyline2

		Dim InpHoldArea67 As ESRI.ArcGIS.Geometry.IPolygon

		Dim pGeometryCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pAT2D As ESRI.ArcGIS.Geometry.IAffineTransformation2D
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D
		Dim pGeom As ESRI.ArcGIS.Geometry.IGeometry2
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pLine As ESRI.ArcGIS.Geometry.ILine

		Dim ptCut67 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt00 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt01 As ESRI.ArcGIS.Geometry.IPoint

		Dim TrackingToler As Double
		Dim MaxDist As Double
		Dim BuffR As Double
		Dim fTmp As Double
		Dim Dist As Double
		Dim Axis As Double
		Dim I As Integer

		If drawFlg Then
			On Error Resume Next
			If Not IAreaElement Is Nothing Then pGraphics.DeleteElement(IAreaElement)
			If Not IIAreaElement Is Nothing Then pGraphics.DeleteElement(IIAreaElement)
			If Not NavAreaElement Is Nothing Then pGraphics.DeleteElement(NavAreaElement)
			If Not TracElement Is Nothing Then pGraphics.DeleteElement(TracElement)
			On Error GoTo 0
		End If

		If FinalNav.TypeCode = eNavaidType.NDB Then
			BuffR = NDBFIXTolerArea(FinalNav.pPtPrj, ArDir, Hhold, TolerArea)
			TrackingToler = NDB.TrackingTolerance
		Else 'If FinalNav.TypeCode = eNavaidType.CodeVOR or eNavaidType.CodeTACAN Then
			BuffR = VORFIXTolerArea(FinalNav.pPtPrj, ArDir, Hhold, TolerArea)
			TrackingToler = VOR.TrackingTolerance
		End If

		ptBaseHoldArea = New ESRI.ArcGIS.Geometry.Multipoint
		Axis = Modulus(ArDir + 180.0, 360.0)

		If HoldingShablon(FinalNav.pPtPrj, FinalNav.TypeCode, IAS, Hhold, CurrADHP.ISAtC, T, Axis, Turn, ptHoldingArea, Shablon, ptLine3, trTurn180) < 1 Then
			Return
		End If

		trHoldingArea = CalcTrajectoryFromMultiPoint(ptHoldingArea)
		trLine3 = CalcTrajectoryFromMultiPoint(ptLine3)

		pTransform2D = Shablon
		pTransform2D.Move(TolerArea.Point(0).X - FinalNav.pPtPrj.X, TolerArea.Point(0).Y - FinalNav.pPtPrj.Y)
		ptBaseHoldArea.AddPointCollection(Shablon)

		pDrawingShablon = New Polyline
		pDrawingShablon.AddPointCollection(Shablon)

		'DrawPointWithText(Shablon.Point(1), "1")
		'Application.DoEvents()

		pTransform2D = pDrawingShablon
		pTransform2D.Move(FinalNav.pPtPrj.X - Shablon.Point(1).X, FinalNav.pPtPrj.Y - Shablon.Point(1).Y)

		On Error Resume Next
		If Not (pShablonElem Is Nothing) Then pGraphics.DeleteElement(pShablonElem)
		On Error GoTo 0
		pShablonElem = DrawPolyLine(pDrawingShablon, RGB(0, 255, 0), 2)

		pTransform2D = Shablon

		For I = 1 To TolerArea.PointCount - 1
			pTransform2D.Move(TolerArea.Point(I).X - TolerArea.Point(I - 1).X, TolerArea.Point(I).Y - TolerArea.Point(I - 1).Y)
			ptBaseHoldArea.AddPointCollection(Shablon)
		Next

		pTopo = ptBaseHoldArea
		BaseHoldArea = pTopo.ConvexHull

		'Set pTopo = Shablon

		'Application.DoEvents()

		pClone = Shablon
		ptArea = pClone.Clone
		pTopo = ptArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		InpHoldArea5 = pTopo.Buffer(BuffR)
		pTransform2D = InpHoldArea5
		pTransform2D.Move(FinalNav.pPtPrj.X - Shablon.Point(0).X, FinalNav.pPtPrj.Y - Shablon.Point(0).Y)

		ptArea = New ESRI.ArcGIS.Geometry.Multipoint
		ptInpHoldArea67 = New ESRI.ArcGIS.Geometry.Multipoint
		pTransform2D = TolerArea
		pTransform2D.Rotate(FinalNav.pPtPrj, DegToRad(-Turn * arHoldAreaEdge.Value))

		I = 3 * (1 - Turn) / 2
		pTransform2D = Shablon
		pTransform2D.Rotate(Shablon.Point(1), DegToRad(-Turn * arHoldAreaEdge.Value))

		pTransform2D.Move(TolerArea.Point(I).X - Shablon.Point(1).X, TolerArea.Point(I).Y - Shablon.Point(1).Y)
		ptInpHoldArea67.AddPointCollection(Shablon)

		I = 3 - I
		pTransform2D.Move(TolerArea.Point(I).X - Shablon.Point(1).X, TolerArea.Point(I).Y - Shablon.Point(1).Y)
		ptInpHoldArea67.AddPointCollection(Shablon)
		pTopo = ptInpHoldArea67

		InpHoldArea67 = pTopo.ConvexHull

		pClone = trTurn180
		trRotatedTurn180 = pClone.Clone
		pTransform2D = trRotatedTurn180

		pTransform2D.Rotate(trRotatedTurn180.Point(0), DegToRad(-Turn * arHoldAreaEdge.Value))
		pTransform2D.Move(TolerArea.Point(I).X - trRotatedTurn180.Point(0).X, TolerArea.Point(I).Y - trRotatedTurn180.Point(0).Y)

		MaxDist = ReturnDistanceInMeters(FinalNav.pPtPrj, trRotatedTurn180.Point(0))
		ptCut67 = trRotatedTurn180.Point(0)

		For I = 1 To trRotatedTurn180.PointCount - 1
			Dist = ReturnDistanceInMeters(FinalNav.pPtPrj, trRotatedTurn180.Point(I))
			If Dist < MaxDist Then Exit For
			If Dist > MaxDist Then
				ptCut67 = trRotatedTurn180.Point(I)
				MaxDist = Dist
			End If
		Next I
		fTmp = ReturnAngleInDegrees(FinalNav.pPtPrj, ptCut67)

		pCutLine = New ESRI.ArcGIS.Geometry.Polyline

		pCutLine.FromPoint = PointAlongPlane(FinalNav.pPtPrj, fTmp, 30.0 * MaxDist)
		pCutLine.ToPoint = PointAlongPlane(FinalNav.pPtPrj, fTmp + 180.0, 30.0 * MaxDist)

		If Turn > 0 Then
			ClipByLine(InpHoldArea67, pCutLine, InpHoldArea67, Nothing, Nothing)
		Else
			ClipByLine(InpHoldArea67, pCutLine, Nothing, InpHoldArea67, Nothing)
		End If

		pCutLine.FromPoint = PointAlongPlane(FinalNav.pPtPrj, ArDir - Turn * arHoldAreaEdge.Value, 30.0 * MaxDist)
		pCutLine.ToPoint = PointAlongPlane(FinalNav.pPtPrj, ArDir - Turn * arHoldAreaEdge.Value + 180.0, 30.0 * MaxDist)

		If Turn < 0 Then
			ClipByLine(InpHoldArea67, pCutLine, InpHoldArea67, Nothing, Nothing)
		Else
			ClipByLine(InpHoldArea67, pCutLine, Nothing, InpHoldArea67, Nothing)
		End If

		pt00 = PointAlongPlane(FinalNav.pPtPrj, fTmp, MaxDist)
		pt01 = PointAlongPlane(FinalNav.pPtPrj, fTmp + Turn * arHoldAreaEdge.Value, MaxDist)
		pArc675 = CreateArcPrj(FinalNav.pPtPrj, pt00, pt01, Turn)

		ptInpHoldArea67.RemovePoints(0, ptInpHoldArea67.PointCount)
		ptInpHoldArea67.AddPointCollection(InpHoldArea67)

		pClone = ptInpHoldArea67
		ptInpHoldArea8 = pClone.Clone

		ptInpHoldArea67.AddPointCollection(pArc675)
		pTopo = ptInpHoldArea67

		InpHoldArea67 = pTopo.ConvexHull

		ptArea.AddPointCollection(InpHoldArea5)
		ptArea.AddPointCollection(BaseHoldArea)
		ptArea.AddPointCollection(InpHoldArea67)

		pLine = New ESRI.ArcGIS.Geometry.Line
		pAT2D = New ESRI.ArcGIS.Geometry.AffineTransformation2D

		pLine.FromPoint = pCutLine.FromPoint
		pLine.ToPoint = pCutLine.ToPoint

		pAT2D.DefineReflection(pLine)

		pTransform2D = ptInpHoldArea8
		pTransform2D.Transform(ESRI.ArcGIS.Geometry.esriTransformDirection.esriTransformForward, pAT2D)

		pTopo = ptInpHoldArea8
		InpHoldArea8 = pTopo.ConvexHull
		ptInpHoldArea67.RemovePoints(0, ptInpHoldArea67.PointCount)

		ptInpHoldArea67.AddPointCollection(InpHoldArea67)
		ptInpHoldArea67.AddPointCollection(pArc675)
		ptArea.AddPointCollection(InpHoldArea8)

		pTopo = ptArea
		HoldingArea = pTopo.ConvexHull
		pTopo = HoldingArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'==========================================================================================================
		Dim LimitDME As NavaidData

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOB As ESRI.ArcGIS.Geometry.IPoint

		Dim NomDist As Double
		Dim fTAS As Double
		Dim Vsan As Double
		Dim DL2 As Double
		Dim dH As Double
		Dim Rv As Double
		Dim Rs As Double
		Dim ac As Double
		Dim w As Double
		Dim E As Double

		Dim StartDir As Double
		Dim currDir As Double
		Dim Dist0 As Double
		Dim r0 As Double

		Dim pLeftPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRightPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCutSpiral As ESRI.ArcGIS.Geometry.IPointCollection

		If (T >= 1) And CheckBox0603.Enabled And (CheckBox0603.Checked) Then
			fTAS = IAS2TAS(IAS, Hhold, CurrADHP.ISAtC)
			Vsan = 0.277777777777778 * fTAS

			Rv = 943.27 / fTAS
			If Rv > 3.0 Then Rv = 3.0

			w = 0.277777777777778 * (0.012 * Hhold + 87.0)
			E = w / Rv

			ac = 11.0 * Vsan
			Rs = 1000.0 * fTAS / (62.83 * Rv)

			r0 = System.Math.Sqrt(ac * ac + Rs * Rs)
			StartDir = 90.0 + RadToDeg(Math.Atan2(ac, Rs))

			LimitDME = DMEList(FinalNav.PairNavaidIndex)
			dH = Hhold - LimitDME.pPtPrj.Z

			ptOB = ptHoldingArea.Point(2)
			NomDist = ReturnDistanceInMeters(LimitDME.pPtPrj, ptOB)

			DL2 = NomDist + DME.MinimalError + DME.ErrorScalingUp * System.Math.Sqrt(NomDist * NomDist + dH * dH)
			Dist0 = DL2 + r0

			pCutSpiral = New ESRI.ArcGIS.Geometry.Polyline

			For I = -120 To 90
				currDir = ArDir + 180.0 + Turn * I
				Dist = Dist0 + (I + StartDir) * E
				ptTmp = PointAlongPlane(LimitDME.pPtPrj, currDir, Dist)

				'DrawPointWithText(ptTmp, "Sp-" + (I + 121).ToString(), -1)
				'Application.DoEvents()

				pCutSpiral.AddPoint(ptTmp)
			Next

			'DrawPolyLine(pCutSpiral, RGB(0, 0, 255), 2)
			'DrawPolygon(CreatePrjCircle(LimitDME.pPtPrj, DL2))
			'Application.DoEvents()

			'DrawPolyLine(pCutSpiral0, 0, 2)
			''While (True)
			'	Application.DoEvents()
			''End While

			pTopo = HoldingArea
			pTopo.Cut(pCutSpiral, pLeftPoly, pRightPoly)

			If Turn < 0 Then
				HoldingArea = pRightPoly
			Else
				HoldingArea = pLeftPoly
			End If

			pTopo = HoldingArea
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If

		'==========================================================================================================
		pTopo = HoldingArea
		BufferedHoldingArea = pTopo.Buffer(arHoldingBuffer.Value)

		pTopo = BufferedHoldingArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'=================== Input Sectors =================
		Dim FromAngle(2) As Double
		Dim ToAngle(2) As Double
		Dim Color As Integer
		Dim pSector As ESRI.ArcGIS.Geometry.Polygon
		'===================================================
		If drawFlg Then
			If CheckBox0601.Checked Then
				'==================Sector 1
				FromAngle(0) = ArDir
				ToAngle(0) = ArDir + Turn * (180.0 - arHoldAreaEdge.Value)
				Color = 255
				pSector = CreateSectorPrj(FinalNav.pPtPrj, MaxR, FromAngle(0), ToAngle(0), Turn)
				DrawPolygon(pSector, Color)
				'==================Sector 2
				ToAngle(1) = FromAngle(0)
				FromAngle(1) = ToAngle(1) - Turn * arHoldAreaEdge.Value
				Color = RGB(128, 128, 0)
				pSector = CreateSectorPrj(FinalNav.pPtPrj, MaxR, FromAngle(1), ToAngle(1), Turn)
				DrawPolygon(pSector, Color)
				'==================Sector 3
				ToAngle(2) = FromAngle(1)
				FromAngle(2) = ToAngle(0)
				Color = 0
				pSector = CreateSectorPrj(FinalNav.pPtPrj, MaxR, FromAngle(2), ToAngle(2), Turn)
				DrawPolygon(pSector, Color)
			End If
		End If
		'===================================================

		If Not CheckBox0602.Checked Then
			If drawFlg Then
				IIAreaElement = DrawPolygon(BufferedHoldingArea, RGB(255, 0, 255))
				IAreaElement = DrawPolygon(HoldingArea, 0)
				TracElement = DrawPolyLine(trHoldingArea, 0, 1)

				IIAreaElement.Locked = True
				IAreaElement.Locked = True
				TracElement.Locked = True
			End If
			Return
		End If

		LTrackingTolerLine = New ESRI.ArcGIS.Geometry.Polyline
		LTrackingTolerLine.FromPoint = FinalNav.pPtPrj
		LTrackingTolerLine.ToPoint = PointAlongPlane(FinalNav.pPtPrj, ArDir - Turn * TrackingToler + 180.0, MaxR)

		pTopo = LTrackingTolerLine
		If pTopo.Intersect(trLine3, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension).IsEmpty() Then
			If drawFlg Then
				IIAreaElement = DrawPolygon(BufferedHoldingArea, RGB(255, 0, 255))
				IAreaElement = DrawPolygon(HoldingArea, 0)
				TracElement = DrawPolyLine(trHoldingArea, 0, 1)

				IIAreaElement.Locked = True
				IAreaElement.Locked = True
				TracElement.Locked = True
			End If
			Return
		End If

		' ClipPolygon ====================================================================================================
		RTrackingTolerLine = New ESRI.ArcGIS.Geometry.Polyline
		RTrackingTolerLine.FromPoint = FinalNav.pPtPrj
		RTrackingTolerLine.ToPoint = PointAlongPlane(FinalNav.pPtPrj, ArDir + Turn * TrackingToler + 180.0, MaxR)

		pClone = trTurn180
		trRotatedTurn180 = pClone.Clone
		pTransform2D = trRotatedTurn180
		pTransform2D.Rotate(trRotatedTurn180.Point(0), PI)

		pClone = RTrackingTolerLine
		ShiftedRLine = pClone.Clone
		pTransform2D = ShiftedRLine
		pTransform2D.Move(trTurn180.Point(0).X - trTurn180.Point(trTurn180.PointCount - 1).X, trTurn180.Point(0).Y - trTurn180.Point(trTurn180.PointCount - 1).Y)

		'Dist = ReturnDistanceInMeters(trTurn180.Point(0), trTurn180.Point(trTurn180.PointCount - 1))
		pTmpPolyline = New ESRI.ArcGIS.Geometry.Polyline
		pTmpPolyline.FromPoint = PointAlongPlane(ShiftedRLine.FromPoint, ArDir + Turn * TrackingToler + 180.0, 2.0 * MaxR)
		pTmpPolyline.ToPoint = ShiftedRLine.FromPoint
		pProxi = ShiftedRLine.FromPoint

		pTopo = HoldingArea
		pClipPolygon = pTopo.Intersect(pTmpPolyline, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		Dist = 0

		For I = 0 To pClipPolygon.PointCount - 1
			fTmp = pProxi.ReturnDistance(pClipPolygon.Point(I))
			If fTmp > Dist Then Dist = fTmp
		Next I
		Dist = Dist - 1.0

		pClipPolygon = New ESRI.ArcGIS.Geometry.Polygon
		pClipPolygon.AddPoint(PointAlongPlane(RTrackingTolerLine.FromPoint, ArDir + Turn * TrackingToler + 180.0, Dist))
		pClipPolygon.AddPoint(PointAlongPlane(RTrackingTolerLine.FromPoint, ArDir + Turn * TrackingToler + 180.0, Dist + 2.0 * MaxR))
		pClipPolygon.AddPoint(PointAlongPlane(ShiftedRLine.FromPoint, ArDir + Turn * TrackingToler + 180.0, Dist + 2.0 * MaxR))
		pClipPolygon.AddPoint(PointAlongPlane(ShiftedRLine.FromPoint, ArDir + Turn * TrackingToler + 180.0, Dist))
		'==========================================================================================================
		Dim CosA As Double
		Dim SinA As Double

		pTopo = pClipPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon pClipPolygon, 255

		pTopo = HoldingArea
		pClone = pTopo.Boundary
		ptArea = pClone.Clone
		pTopo = ptArea

		pGeometryCollection = pTopo.IntersectMultidimension(pClipPolygon)
		ptArea = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To pGeometryCollection.GeometryCount - 1
			pGeom = pGeometryCollection.Geometry(I)
			If pGeom.Dimension = ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension Then
				pTopo = ptArea
				ptArea = pTopo.Union(pGeom)
			End If
		Next I

		pTransform2D = trRotatedTurn180
		pProxi = ptArea

		CosA = System.Math.Cos(DegToRad(ArDir + Turn * TrackingToler + 180.0))
		SinA = System.Math.Sin(DegToRad(ArDir + Turn * TrackingToler + 180.0))

		Dist = pProxi.ReturnDistance(trRotatedTurn180)
		Do While Dist > 0.5
			pTransform2D.Move(Dist * CosA, Dist * SinA)
			Dist = pProxi.ReturnDistance(trRotatedTurn180)
		Loop

		pTransform2D.Move(1.5 * CosA, 1.5 * SinA)
		trRotatedTurn180.AddPoint(PointAlongPlane(trRotatedTurn180.Point(trRotatedTurn180.PointCount - 1), ArDir + Turn * TrackingToler, 3.0 * MaxR))

		pTopo = HoldingArea

		If Turn < 0 Then
			pTopo.Cut(trRotatedTurn180, pClipPolygon, HoldingArea)
		Else
			pTopo.Cut(trRotatedTurn180, HoldingArea, pClipPolygon)
		End If

		pTopo = HoldingArea
		BufferedHoldingArea = pTopo.Buffer(arHoldingBuffer.Value)
		pTopo = BufferedHoldingArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		If drawFlg Then
			IIAreaElement = DrawPolygon(BufferedHoldingArea, RGB(255, 0, 255))
			IAreaElement = DrawPolygon(HoldingArea, 0)
			TracElement = DrawPolyLine(trHoldingArea, 0, 1)

			IIAreaElement.Locked = True
			IAreaElement.Locked = True
			TracElement.Locked = True
		End If
	End Sub

	Private Function ConvertTracToPoints(ByRef TrackPoints() As ReportPoint) As Double
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer

		Dim PDG As Double
		Dim fTmp As Double
		Dim fCourse As Double

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
		fCourse = CDbl(SpinButton0201.Text)

		'\============================================================================
		If OptionButton0201.Checked Then
			'IF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			pPtGeo.PutCoords(IFPnt.X, IFPnt.Y)
			pGeometry = pPtGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			TrackPoints(0).Description = "IF"
			TrackPoints(0).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(0).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(0).Direction = ConvertAngle(fCourse)
			TrackPoints(0).PDG = NO_DATA_VALUE
			TrackPoints(0).Altitude = IFPnt.Z + fRefHeight

			TrackPoints(0).Radius = NO_DATA_VALUE
			TrackPoints(0).Turn = 0

			TrackPoints(0).CenterLat = ""
			TrackPoints(0).CenterLon = ""

			TrackPoints(0).TurnAngle = NO_DATA_VALUE
			TrackPoints(0).TurnArcLen = NO_DATA_VALUE
			pPoints(0) = IFPnt
			'FAF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			pPtGeo.PutCoords(PtFAF.X, PtFAF.Y)
			pGeometry = pPtGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			TrackPoints(1).Description = "FAF"
			TrackPoints(1).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(1).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(1).Direction = ConvertAngle(fCourse)
			TrackPoints(1).PDG = NO_DATA_VALUE
			TrackPoints(1).Altitude = PtFAF.Z + fRefHeight

			TrackPoints(1).Radius = NO_DATA_VALUE
			TrackPoints(1).Turn = 0
			TrackPoints(1).CenterLat = ""
			TrackPoints(1).CenterLon = ""

			TrackPoints(1).TurnAngle = NO_DATA_VALUE
			TrackPoints(1).TurnArcLen = NO_DATA_VALUE
			pPoints(1) = PtFAF
			N = 2
			'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		Else
			'          IAF
			'    Set PtTOB = ptHoldingArea.Point(0)
			'    Set PtSOL = ptHoldingArea.Point(1)
			'    Set PtEOL = ptHoldingArea.Point(2)
			'    Set FarFAF = ptHoldingArea.Point(3)
			'IAF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			pPtGeo.PutCoords(FinalNav.pPtGeo.X, FinalNav.pPtGeo.Y)

			If OptionButton0601.Checked Then
				pPtGeo.M = PtTOB.M
				FillTurnParams(PtTOB, PtSOL, TrackPoints(0))
				pPoints(0) = PtTOB
			Else
				pPtGeo.M = PtSOL.M
				TrackPoints(0).Radius = NO_DATA_VALUE
				TrackPoints(0).Turn = 0
				TrackPoints(0).CenterLat = ""
				TrackPoints(0).CenterLon = ""

				TrackPoints(0).TurnAngle = NO_DATA_VALUE
				TrackPoints(0).TurnArcLen = NO_DATA_VALUE
				pPoints(0) = PtSOL
			End If

			fTmp = Dir2Azt(FictTHR, pPtGeo.M)

			TrackPoints(0).Description = "IAF"
			TrackPoints(0).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(0).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(0).Direction = ConvertAngle(fTmp)
			TrackPoints(0).PDG = NO_DATA_VALUE
			TrackPoints(0).Altitude = DeConvertHeight(CDbl(TextBox0601.Text))
			If ComboBox0501.SelectedIndex = 1 Then TrackPoints(0).Altitude += fRefHeight
			N = 1
			'SOL++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			If OptionButton0601.Checked Then
				PtSOL.Z = CDbl(TrackPoints(0).Altitude)

				pPtGeo.PutCoords(PtSOL.X, PtSOL.Y)
				pGeometry = pPtGeo
				pGeometry.SpatialReference = pSpRefPrj
				pGeometry.Project(pSpRefGeo)

				fTmp = Dir2Azt(FictTHR, PtSOL.M)
				TrackPoints(1).Description = "SOL"
				TrackPoints(1).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
				TrackPoints(1).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
				TrackPoints(1).Direction = ConvertAngle(fTmp)
				TrackPoints(1).PDG = NO_DATA_VALUE
				TrackPoints(1).Altitude = TrackPoints(0).Altitude 'CStr(Round(CDbl(TextBox0601.Text) + fRefHeight))

				TrackPoints(1).Radius = NO_DATA_VALUE
				TrackPoints(1).Turn = 0
				TrackPoints(1).CenterLat = ""
				TrackPoints(1).CenterLon = ""
				TrackPoints(1).TurnAngle = NO_DATA_VALUE
				TrackPoints(1).TurnArcLen = NO_DATA_VALUE
				pPoints(1) = PtSOL
				N = 2
			End If

			'EOL++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			If OptionButton0601.Checked Or OptionButton0602.Checked Then
				PtEOL.Z = FarFAF.Z
				pPtGeo.PutCoords(PtEOL.X, PtEOL.Y)
				pGeometry = pPtGeo
				pGeometry.SpatialReference = pSpRefPrj
				pGeometry.Project(pSpRefGeo)
				fTmp = Dir2Azt(FictTHR, PtEOL.M)

				TrackPoints(N).Description = "End of Outbound Leg (EOL)"
				TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
				TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
				TrackPoints(N).Direction = ConvertAngle(fTmp)
				TrackPoints(N).PDG = NO_DATA_VALUE
				TrackPoints(N).Altitude = PtEOL.Z
				FillTurnParams(PtEOL, FarFAF, TrackPoints(N))

				'        TrackPoints(N).Radius = NO_DATA_VALUE
				'        TrackPoints(N).Turn = 0
				'        TrackPoints(N).CenterLat = ""
				'        TrackPoints(N).CenterLon = ""
				'        TrackPoints(N).TurnAngle = NO_DATA_VALUE
				'        TrackPoints(N).TurnArcLen = NO_DATA_VALUE
				'        If OptionButton3.Value Then
				'            TrackPoints(N).Radius = TrackPoints(0).Radius
				'        Else
				'            Set ptConstr = pPt
				'            ptConstr.ConstructAngleIntersection PtEOL, DegToRad(PtEOL.M + 90.0), FarFAF, DegToRad(FarFAF.M + 90.0)
				'            TrackPoints(N).Radius = ReturnDistanceInMeters(pPt, PtEOL)
				'        End If
				pPoints(N) = PtEOL
				N = N + 1
			End If

			'SIL ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			pPtGeo.PutCoords(FarFAF.X, FarFAF.Y)
			pGeometry = pPtGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			fTmp = Dir2Azt(FictTHR, FarFAF.M)

			TrackPoints(N).Description = "Start of Inbound Leg (SIL)" 'Verify
			TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(N).Direction = ConvertAngle(fTmp)
			TrackPoints(N).PDG = NO_DATA_VALUE
			TrackPoints(N).Altitude = FarFAF.Z

			TrackPoints(N).Radius = NO_DATA_VALUE
			TrackPoints(N).Turn = 0
			TrackPoints(N).CenterLat = ""
			TrackPoints(N).CenterLon = ""
			TrackPoints(N).TurnAngle = NO_DATA_VALUE
			TrackPoints(N).TurnArcLen = NO_DATA_VALUE
			pPoints(N) = FarFAF
			N = N + 1
		End If

		'SDF++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		If CheckBox0701.Checked Then
			pPtGeo.PutCoords(PtSDF.X, PtSDF.Y)
			pGeometry = pPtGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			TrackPoints(N).Description = "SDF"
			TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(N).Direction = ConvertAngle(fCourse)
			TrackPoints(N).PDG = NO_DATA_VALUE
			TrackPoints(N).Altitude = PtSDF.Z + fRefHeight

			TrackPoints(N).Radius = NO_DATA_VALUE
			TrackPoints(N).Turn = 0
			TrackPoints(N).CenterLat = ""
			TrackPoints(N).CenterLon = ""
			TrackPoints(N).TurnAngle = NO_DATA_VALUE
			TrackPoints(N).TurnArcLen = NO_DATA_VALUE
			pPoints(N) = PtSDF
			N = N + 1
		End If

		'++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		'fTmp = Modulus(Dir2Azt(FictTHR, CDbl(TextBox1.Text)), 360.0)
		'fTmp = CDbl(TextBox0201.Text)

		'MAPt++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		pPtGeo.PutCoords(pMAPt.X, pMAPt.Y)
		pGeometry = pPtGeo
		pGeometry.SpatialReference = pSpRefPrj
		pGeometry.Project(pSpRefGeo)

		TrackPoints(N).Description = "MAPt"
		TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
		TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
		TrackPoints(N).Direction = ConvertAngle(fCourse)
		TrackPoints(N).PDG = NO_DATA_VALUE
		TrackPoints(N).Altitude = pMAPt.Z

		TrackPoints(N).Radius = NO_DATA_VALUE
		TrackPoints(N).Turn = 0
		TrackPoints(N).CenterLat = ""
		TrackPoints(N).CenterLon = ""
		TrackPoints(N).TurnAngle = NO_DATA_VALUE
		TrackPoints(N).TurnArcLen = NO_DATA_VALUE
		pPoints(N) = pMAPt
		N = N + 1

		'SOC++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		pPtGeo.PutCoords(PtSOC.X, PtSOC.Y)
		pGeometry = pPtGeo
		pGeometry.SpatialReference = pSpRefPrj
		pGeometry.Project(pSpRefGeo)

		TrackPoints(N).Description = "SOC"
		TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
		TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
		TrackPoints(N).Direction = ConvertAngle(fCourse)
		TrackPoints(N).PDG = NO_DATA_VALUE
		TrackPoints(N).Altitude = PtSOC.Z + fRefHeight

		TrackPoints(N).Radius = NO_DATA_VALUE
		TrackPoints(N).Turn = 0
		TrackPoints(N).CenterLat = ""
		TrackPoints(N).CenterLon = ""
		TrackPoints(N).TurnAngle = NO_DATA_VALUE
		TrackPoints(N).TurnArcLen = NO_DATA_VALUE
		pPoints(N) = PtSOC

		ReDim Preserve TrackPoints(N + 1)

		ConvertTracToPoints = 0.0

		For I = 0 To N - 1
			If pPtC1.PointCount > 0 Then pPtC1.RemovePoints(0, pPtC1.PointCount)
			pPtC1.AddPoint(pPoints(I))
			pPtC1.AddPoint(pPoints(I + 1))

			pPolyline = CalcTrajectoryFromMultiPoint(pPtC1)
			TrackPoints(I).ToNext = pPolyline.Length
			ConvertTracToPoints = ConvertTracToPoints + TrackPoints(I).ToNext
		Next I
		N = N + 1

		If Not CheckBox0801.Checked Then
			pPtGeo.PutCoords(pTermPt.X, pTermPt.Y)
			pGeometry = pPtGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			TrackPoints(N).Description = "Straight MA termination point"
			TrackPoints(N).Lat = DegreeToString(pPtGeo.Y, Degree2StringMode.DMSLat)
			TrackPoints(N).Lon = DegreeToString(pPtGeo.X, Degree2StringMode.DMSLon)
			TrackPoints(N).Direction = ConvertAngle(fCourse)
			TrackPoints(N).PDG = fMissAprPDG
			TrackPoints(N).Altitude = pTermPt.Z
			TrackPoints(N - 1).ToNext = pStraightNomLine.Length - TrackPoints(N - 2).ToNext

			TrackPoints(N).Radius = NO_DATA_VALUE
			TrackPoints(N).Turn = 0
			TrackPoints(N).CenterLat = ""
			TrackPoints(N).CenterLon = ""
			TrackPoints(N).TurnAngle = NO_DATA_VALUE
			TrackPoints(N).TurnArcLen = NO_DATA_VALUE

			ConvertTracToPoints += TrackPoints(N - 1).ToNext
			'pPoints(N) = pTermPt
		Else
			M = MPtCollection.PointCount + 1
			ReDim Preserve TrackPoints(N + M)

			'TP++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
			TrackPoints(N).Direction = ConvertAngle(fCourse)
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
			PDG = fMissAprPDG 'CDbl(TextBox1501.Text) * 0.01

			'    TrackPoints(0).Description = "Начальная точка процедуры вылета"
			'    TrackPoints(0).Lat = MyDD2Str(PtDer.Y, 3)
			'    TrackPoints(0).Lon = MyDD2Str(PtDer.X, 4)
			'    TrackPoints(0).Direction = CStr(Round(Modulus(Dir2Azt(ptDerPrj, AztDir), 360.0), 2))
			'    TrackPoints(0).Height = dpH_abv_DER.Value + ptDerPrj.Z
			'    TrackPoints(0).Radius = NO_DATA_VALUE

			'pPtNext.PutCoords(TurnFixPnt.X, TurnFixPnt.Y)
			'pPtNext.Z = TurnFixPnt.Z
			'pPtNext.M = TurnFixPnt.M ' Azt2Dir(pPt, fCourse)

			pPtNext = TurnFixPnt

			For J = N + 1 To M + N
				I = J - N
				TrackPoints(J).Radius = NO_DATA_VALUE
				'=========================================================================
				If (I < M) Then
					pPtPrj = MPtCollection.Point(I - 1)
					pPtGeo.PutCoords(pPtPrj.X, pPtPrj.Y)

					pGeometry = pPtGeo
					pGeometry.SpatialReference = pSpRefPrj
					pGeometry.Project(pSpRefGeo)
					If (I And 1) = 1 Then
						FillTurnParams(MPtCollection.Point(I - 1), MPtCollection.Point(I), TrackPoints(J))
						TrackPoints(J).Description = My.Resources.str00521 + CStr((I + 1) \ 2) + My.Resources.str00523
					Else
						TrackPoints(J).Description = My.Resources.str00522 + CStr((I + 1) \ 2) + My.Resources.str00523
						TrackPoints(J).Direction = ConvertAngle(Dir2Azt(FictTHR, pPtPrj.M))
					End If
				Else
					'=========================================================================
					pPtPrj = mPoly.ToPoint
					pPtPrj.M = pPtNext.M
					pPtGeo.PutCoords(pPtPrj.X, pPtPrj.Y)

					pGeometry = pPtGeo
					pGeometry.SpatialReference = pSpRefPrj
					pGeometry.Project(pSpRefGeo)
					TrackPoints(J).Description = My.Resources.str00524  '"Конечная точка процедуры вылета" 'Verify
					'	TrackPoints(J).Direction = TrackPoints(J - 1).Direction
				End If
				'=========================================================================

				pPtPrev = pPtNext
				pPtNext = pPtPrj

				pPtC1 = New ESRI.ArcGIS.Geometry.Polyline
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

	End Function

	Private Sub SaveInputData(ByRef repFileName As String)
		DBModule.pObjectDir.SaveAsXml(repFileName + "_InputData.xml")
	End Sub

	Private Sub SaveGeometry(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
		Dim I As Integer
		Dim TraceLen As Double
		Dim TrackPoints() As ReportPoint

		GeomRep = New ReportFile()
		TraceLen = ConvertTracToPoints(TrackPoints)

		'GeomRep.RefHeight = fRefHeight
		GeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + My.Resources.str00811)

		'GeomRep.WriteMessage(My.Resources.str812)
		'GeomRep.WriteMessage()
		'GeomRep.WriteMessage(RepFileTitle)

		GeomRep.WriteMessage(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00811)
		GeomRep.WriteHeader(pReport)

		GeomRep.WriteMessage()
		GeomRep.WriteMessage()

		For I = 0 To UBound(TrackPoints)
			GeomRep.WritePoint(TrackPoints(I))
		Next I

		GeomRep.WriteMessage()
		GeomRep.Param(My.Resources.str00814, CStr(ConvertDistance(TraceLen, eRoundMode.NEAREST)), DistanceConverter(DistanceUnit).Unit)

		GeomRep.CloseFile()
	End Sub

	Private Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String, ByRef pReport As ReportHeader)
		ProtRep = New ReportFile()
		'ProtRep.RefHeight = fRefHeight

		ProtRep.OpenFile(RepFileName + "_Report", RepFileTitle + ": " + My.Resources.str00815)
		ProtRep.WriteMessage(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00815)
		ProtRep.WriteHeader(pReport, True)

		ProtRep.WriteMessage()
		ProtRep.WriteMessage()

		'Dim vsList As List(Of VerticalStructure)

		NonPrecReportFrm.SortForSave()

		ProtRep.lListView = NonPrecReportFrm.ListView01
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(0).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(1)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView02
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(1).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(2)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView03
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(2).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(3)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView04
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(3).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(4)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView05
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(4).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(5)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView06
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(5).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(6)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView07
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(6).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(7)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.lListView = NonPrecReportFrm.ListView08
		ProtRep.WriteTab(NonPrecReportFrm.MultiPage1.TabPages.Item(7).Text)
		'vsList = NonPrecReportFrm.GetPageObstacles(8)
		'pObjectDir.AddToSrcLocalStorage(vsList)

		ProtRep.CloseFile()
	End Sub

	Private Sub SaveAccuracy(RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader)
		AccurRep = New ReportFile()

		AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + My.Resources.str00805)
		'AccurRep.H1(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00805)
		AccurRep.WriteMessage(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00805)
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		AccurRep.WriteHeader(pReport)
		AccurRep.Param("Distance accuracy", _settings.DistancePrecision, DistanceConverter(DistanceUnit).Unit)
		AccurRep.Param("Angle accuracy", _settings.AnglePrecision, "degrees")

		AccurRep.WriteMessage()
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		AccurRep.WriteMessage("=================================================")
		AccurRep.WriteMessage()

		Dim HaveFAF As Boolean = OptionButton0201.Checked
		Dim HaveSDF As Boolean = CheckBox0701.Checked
		Dim HaveTP As Boolean = CheckBox0801.Checked
		Dim HaveIntercept As Boolean = OptionButton1202.Checked

		Dim GuidNav As NavaidData = FinalNav
		Dim IntersectNav As NavaidData

		'Leg 1 Initial to Final Approach =================================================================================
		If HaveFAF Then
			IntersectNav = IFNavDat(ComboBox0401.SelectedIndex)
			SaveFixAccurasyInfo(AccurRep, IFPnt, "IF", GuidNav, IntersectNav)

			IntersectNav = FAFNavDat(ComboBox0302.SelectedIndex)
			SaveFixAccurasyInfo(AccurRep, PtFAF, "FAF", GuidNav, IntersectNav)
		End If

		'Leg 2 Final Approach To SDF ======================================================================================
		If HaveSDF Then
			IntersectNav = SDFNavDat(ComboBox0702.SelectedIndex)
			SaveFixAccurasyInfo(AccurRep, PtSDF, "SDF", GuidNav, IntersectNav)
		End If

		'Leg 3-4 Final Approach / Straight Missed Approach ================================================================
		If OptionButton0801.Checked Then
			IntersectNav = MAPtNavDat(ComboBox0801.SelectedIndex)
			SaveFixAccurasyInfo(AccurRep, pMAPt, "MAPt", GuidNav, IntersectNav)
		End If

		If Not HaveTP Then
			AccurRep.CloseFile()
			Return
		End If

		'Leg 5 Straight Missed Approach ============================================================================
		If OptionButton0902.Checked Then
			IntersectNav = TurnInterNavDat(ComboBox1102.SelectedIndex)
			SaveFixAccurasyInfo(AccurRep, TurnFixPnt, "MATF", GuidNav, IntersectNav)
		End If

		'Leg 6-7 Missed Approach Termination ==========================================================================

		If HaveIntercept Then
			GuidNav = WPT_FIXToNavaid(TurnDirector)
			IntersectNav = TerInterNavDat(ComboBox1502.SelectedIndex)

			SaveFixAccurasyInfo(AccurRep, TerFixPnt, IIf(CheckBox1501.Checked, "MATF", "MAHF"), GuidNav, IntersectNav, Not CheckBox1501.Checked)
		End If

		'=============================================================================================================

		If CheckBox1501.Checked Then
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

		'Transition II ==========================================================================
		'If Not HaveFAF Then
		''	Legs ======================================================================================================
		''	Leg 1 Intermediate Approach ===============================================================================
		'	If OptionButton0601.Checked Then
		'		pSegmentLeg = RacetrackLeg(pProcedure, IsLimitedTo)

		'	ElseIf OptionButton0602.Checked Then
		'		pSegmentLeg = BaseTurnOutboundLeg(pProcedure, IsLimitedTo, pEndPoint)
		'		pSegmentLeg = BaseTurnInbound(ptFAFDescent, pProcedure, IsLimitedTo, pEndPoint)
		'	Else
		'		pSegmentLeg = ProcTurnOutboundLeg(pProcedure, IsLimitedTo, pEndPoint)
		'		pSegmentLeg = ProcTurnInbound(ptFAFDescent, pProcedure, IsLimitedTo, pEndPoint)
		'	End If
		'End If
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		AccurRep.CloseFile()
	End Sub

	Private Sub SaveLog(RepFileName As String, RepFileTitle As String, ByRef pReport As ReportHeader)
		Dim I As Integer
		Dim J As Integer
		Dim sTmp As String
		Dim fTmp As Double
		Dim fOCA As Double
		Dim fOCH As Double
		Dim itmX As System.Windows.Forms.ListViewItem
		LogRep = New ReportFile()

		'LogRep.RefHeight = fRefHeight

		LogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + My.Resources.str00817)
		LogRep.H1(My.Resources.str00001 + " - " + RepFileTitle + ": " + My.Resources.str00817)
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		LogRep.WriteHeader(pReport)
		LogRep.WriteMessage()
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		'LogRep.WriteMessage(My.Resources.str00835)
		LogRep.WriteMessage()
		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		LogRep.WriteText("<TABLE border=1 style= ""font-family: Arial, Sans-Serif; font-size:12"">")
		LogRep.WriteText("<TD><CAPTION><STRONG>Publication data</STRONG></CAPTION></TD>")
		LogRep.WriteText("<TBODY>")
		LogRep.WriteText("<TR align=center><TD><b>Straight-in approach</b></TD></TR>")
		LogRep.WriteText("<TR><TD>")

		LogRep.Page("Publication data")
		LogRep.ExH1("Straight-in approach")
		If Not CheckBox0801.Checked Then
			fOCH = MAPtOCH
		Else
			fOCH = resultOCH
		End If

		fOCA = fOCH + fRefHeight

		'=============================================================================
		sTmp = CStr(ConvertHeight(fOCA, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00827 + sTmp)

		If HeightUnit = 0 Then
			fTmp = System.Math.Round(fOCA * 0.2 + 0.4999) * 5
		ElseIf HeightUnit = 1 Then
			fTmp = System.Math.Round(ConvertHeight(fOCA, 0) * 0.1 + 0.4999) * 10
		End If

		sTmp = CStr(fTmp) + " " + HeightConverter(HeightUnit).Unit

		LogRep.WriteString(My.Resources.str00828 + sTmp)
		LogRep.WriteString(My.Resources.str00829 + sTmp)
		LogRep.WriteString()
		'=============================================================================
		sTmp = CStr(ConvertHeight(fRefHeight, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00830 + sTmp)

		sTmp = CStr(ConvertHeight(fOCH, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00831 + sTmp)

		If HeightUnit = 0 Then
			fTmp = System.Math.Round(fOCH * 0.2 + 0.4999) * 5
		ElseIf HeightUnit = 1 Then
			fTmp = System.Math.Round(ConvertHeight(fOCH, 0) * 0.1 + 0.4999) * 10
		End If

		sTmp = CStr(fTmp) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00832 + sTmp)
		LogRep.WriteString(My.Resources.str00833 + sTmp)

		If Not CheckBox0801.Checked Then
			sTmp = TextBox0804.Text
		Else
			sTmp = TextBox1501.Text
		End If
		LogRep.WriteMessage(My.Resources.str00834 + sTmp + " %")


		LogRep.WriteText("</TD></TR></TBODY>")
		LogRep.WriteString()
		'=============================================================================

		LogRep.ExH2("Circling to land")
		LogRep.WriteText("<TBODY>")
		LogRep.WriteText("<TR align=center><TD><b>Circling to land</b></TD></TR>")
		LogRep.WriteText("<TR><TD>")

		fOCH = fVisAprOCH
		fOCA = fVisAprOCH + CurrADHP.pPtGeo.Z

		'Straight-in approach for aircraft categories – (указать)

		sTmp = CStr(ConvertHeight(fOCA, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00827 + sTmp)

		If HeightUnit = 0 Then
			fTmp = System.Math.Round(fOCA * 0.2 + 0.4999) * 5
		ElseIf HeightUnit = 1 Then
			fTmp = System.Math.Round(ConvertHeight(fOCA, 0) * 0.1 + 0.4999) * 10
		End If

		sTmp = CStr(fTmp) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00828 + sTmp)
		LogRep.WriteString(My.Resources.str00829 + sTmp)
		LogRep.WriteString()
		'=============================================================================

		sTmp = CStr(ConvertHeight(CurrADHP.pPtGeo.Z, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00830 + sTmp)

		sTmp = CStr(ConvertHeight(fOCH, eRoundMode.NEAREST)) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00831 + sTmp)

		If HeightUnit = 0 Then
			fTmp = System.Math.Round(fOCH * 0.2 + 0.4999) * 5
		ElseIf HeightUnit = 1 Then
			fTmp = System.Math.Round(ConvertHeight(fOCH, 0) * 0.1 + 0.4999) * 10
		End If
		sTmp = CStr(fTmp) + " " + HeightConverter(HeightUnit).Unit
		LogRep.WriteString(My.Resources.str00832 + sTmp)
		LogRep.WriteString(My.Resources.str00831 + sTmp)
		LogRep.WriteText("</TD></TR></TBODY></TABLE>")
		LogRep.WriteMessage()

		'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		'===========================================================================
		LogRep.Page("Log")

		LogRep.ExH2(MultiPage1.TabPages.Item(0).Text)

		LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(0).Text + " ]")
		LogRep.WriteMessage()

		LogRep.WriteMessage(My.Resources.str00836)
		For I = 0 To ListView0001.Items.Count - 1
			itmX = ListView0001.Items.Item(I)
			If itmX.Checked Then LogRep.WriteMessage(itmX.Text) ', itmX.SubItems(1)
		Next I

		LogRep.WriteMessage()

		'===========================================================================

		LogRep.Param(Label0001_00.Text, ComboBox0001.Text)
		LogRep.Param(Label0001_01.Text, TextBox0001.Text, Label0001_07.Text)
		LogRep.Param(Label0001_02.Text, TextBox0002.Text, Label0001_08.Text)
		LogRep.Param(Label0001_03.Text, TextBox0003.Text, Label0001_09.Text)
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.Param(Label0001_13.Text, TextBox0007.Text, Label0001_20.Text)
		LogRep.Param(Label0001_14.Text, TextBox0008.Text, Label0001_21.Text)
		LogRep.Param(Label0001_15.Text, TextBox0009.Text, Label0001_22.Text)
		LogRep.Param(Label0001_16.Text, TextBox0010.Text, Label0001_23.Text)
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.Param(Label0001_04.Text, TextBox0004.Text, Label0001_10.Text)
		LogRep.Param(Label0001_05.Text, TextBox0005.Text, Label0001_11.Text)
		LogRep.Param(Label0001_06.Text, TextBox0006.Text, Label0001_12.Text)
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.Param(Label0001_17.Text, TextBox0011.Text, Label0001_24.Text)
		LogRep.Param(Label0001_18.Text, TextBox0012.Text, Label0001_25.Text)
		LogRep.Param(Label0001_19.Text, TextBox0013.Text)
		'===========================================================================

		LogRep.WriteMessage()
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(1).Text + " ]")
		LogRep.WriteMessage()
		If OptionButton0101.Checked Then
			LogRep.Param(Frame0101.Text, OptionButton0101.Text)
			LogRep.Param(Label0101_0.Text, ComboBox0101.Text)
		Else
			LogRep.Param(Frame0101.Text, OptionButton0102.Text)
		End If

		LogRep.Param(Label0101_1.Text, TextBox0101.Text, _Label0101_4.Text)
		LogRep.Param(_Label0101_6.Text, TextBox0103.Text, _Label0101_9.Text)

		LogRep.Param(_Label0101_2.Text, ComboBox0102.Text, _Label0101_5.Text)
		LogRep.WriteMessage(_Label0101_7.Text)
		LogRep.Param(_Label0101_3.Text, TextBox0102.Text, _Label0101_10.Text)

		LogRep.WriteMessage()
		LogRep.WriteMessage(_Label0101_8.Text)

		For I = 0 To ListView0101.Items.Count - 1
			itmX = ListView0101.Items.Item(I)
			If itmX.SubItems.Count > 1 Then
				LogRep.WriteMessage(My.Resources.str00837 + itmX.Text + My.Resources.str00838 + itmX.SubItems(1).Text + My.Resources.str00839 + itmX.SubItems(2).Text)
			Else
				LogRep.WriteMessage(My.Resources.str00837 + itmX.Text)
			End If
		Next I

		LogRep.WriteMessage()
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.ExH2(MultiPage1.TabPages.Item(2).Text)
		LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(2).Text + " ]")
		LogRep.WriteMessage()
		'===========================================================================

		LogRep.Param(_Label0201_0.Text, ComboBox0201.Text, "°")
		LogRep.Param(My.Resources.str00840, SpinButton0201.Text, _Label0201_7.Text)
		'===========================================================================
		If OptionButton0102.Checked Then
			J = 0
			LogRep.WriteMessage(ColumnHeader0202_01.Text)
			For I = 0 To ListView0202.Items.Count - 1
				itmX = ListView0202.Items.Item(I)
				If itmX.Checked Then
					LogRep.WriteMessage((itmX.Text))
					J += 1
				End If
			Next I
			If J = 0 Then LogRep.WriteMessage("--")
		End If

		LogRep.Param(_Label0201_1.Text, _Label0201_8.Text, _Label0201_13.Text)
		LogRep.Param(_Label0201_2.Text, _Label0201_9.Text, _Label0201_14.Text)

		If FinalNav.TypeCode <> eNavaidType.LLZ Then LogRep.Param(_Label0201_3.Text, _Label0201_10.Text, _Label0201_15.Text)

		LogRep.Param(_Label0201_4.Text, _Label0201_11.Text, _Label0201_16.Text)
		LogRep.Param(_Label0201_5.Text, _Label0201_12.Text, _Label0201_17.Text)
		'    End If
		'===========================================================================
		'    LogRep.WriteParam Label1419.Caption, _Label0201_10.Caption, Label1421.Caption
		If OptionButton0201.Checked Then
			LogRep.WriteMessage(OptionButton0201.Text)
		Else
			LogRep.WriteMessage(OptionButton0202.Text)
		End If


		LogRep.WriteMessage()
		LogRep.WriteMessage(_Label0201_6.Text)

		For I = 0 To ListView0201.Items.Count - 1
			itmX = ListView0201.Items.Item(I)
			If itmX.Checked Then LogRep.WriteMessage((itmX.Text)) '+ " " + itmX.SubItems(1) + " " + itmX.SubItems(2)
		Next I

		LogRep.WriteMessage()
		LogRep.WriteMessage()

		Dim SchemeNames(3) As String

		If OptionButton0201.Checked Then
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(3).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(3).Text + " ]")
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.Param(_Label0301_0.Text, TextBox0301.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(ComboBox0303.Text, TextBox0303.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0301_6.Text, TextBox0306.Text, _Label0301_9.Text)
			'===========================================================================

			fTmp = RadToDeg(System.Math.Atan(FinalAreaPDG))
			LogRep.Param(My.Resources.str10518, CStr(System.Math.Round(fTmp, 2)), "°")
			'===========================================================================

			LogRep.WriteMessage()
			LogRep.WriteMessage(Frame0301.Text)
			LogRep.Param(ComboBox0304.Text + ":", TextBox0302.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0301_5.Text, TextBox0304.Text)
			LogRep.Param(_Label0301_7.Text, ComboBox0302.Text, _Label0301_10.Text)
			LogRep.Param(_Label0301_2.Text, TextBox0305.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.WriteMessage(_Label0301_4.Text)
			'===========================================================================

			LogRep.WriteMessage()
			LogRep.WriteMessage(Frame0302.Text)
			LogRep.Param(_Label0301_3.Text, ComboBox0301.Text, "°")
			LogRep.Param(_Label0301_8.Text, TextBox0307.Text, DistanceConverter(DistanceUnit).Unit)
			'===========================================================================

			LogRep.WriteMessage()
			For I = 0 To ListView0301.Items.Count - 1
				itmX = ListView0301.Items.Item(I)
				LogRep.WriteMessage(itmX.Text + " " + itmX.SubItems(1).Text) '+ " " + itmX.SubItems(2).Text)
			Next I

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(4).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(4).Text + " ]")
			LogRep.WriteMessage()
			LogRep.Param(label0401_00.Text, TextBox0401.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(ComboBox0402.Text, TextBox0404.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(label0401_10.Text, TextBox0408.Text, "%")

			fTmp = RadToDeg(System.Math.Atan(IntermAreaPDG))
			LogRep.Param(My.Resources.str10518, CStr(System.Math.Round(fTmp, 2)), "°")
			'//    TextBox0408.Text = CStr(100# * IntermAreaPDG)

			LogRep.Param(label0401_11.Text, TextBox0409.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(label0401_20.Text, TextBox0411.Text, label0401_21.Text)
			LogRep.Param(label0401_22.Text, TextBox0412.Text)

			LogRep.WriteMessage()
			LogRep.WriteMessage((Frame0401.Text))
			LogRep.Param(label0401_02.Text, ComboBox0401.Text, label0401_08.Text)
			LogRep.Param(label0401_03.Text, TextBox0403.Text, label0401_13.Text)
			LogRep.WriteMessage(label0401_04.Text)

			If label0401_08.Text = "DME" Then
				'            LogRep.WriteParam Label1536.Caption, TextBox0806.Text, Label1563.Caption
				If OptionButton0401.Checked Then
					LogRep.WriteMessage((OptionButton0401.Text))
				Else
					LogRep.WriteMessage((OptionButton0402.Text))
				End If
			End If

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================
			'LogRep.WriteMessage "[ " + MultiPage1.TabCaption(5) + " ]"
			'LogRep.WriteMessage
			'===========================================================================
		Else
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(5).Text)

			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(5).Text + " ]")
			LogRep.WriteMessage()

			For I = 0 To ListView0501.Items.Count - 1
				itmX = ListView0501.Items.Item(I)
				LogRep.WriteMessage(itmX.Text + " " + itmX.SubItems(1).Text + " " + itmX.SubItems(2).Text + " " + itmX.SubItems(3).Text)
			Next I
			LogRep.Param(ComboBox0501.Text, TextBox0501.Text, _Label0501_0.Text)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			SchemeNames(0) = OptionButton0601.Text
			SchemeNames(1) = OptionButton0602.Text
			SchemeNames(2) = OptionButton0603.Text
			SchemeNames(3) = OptionButton0604.Text

			LogRep.ExH2(MultiPage1.TabPages.Item(6).Text)

			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(6).Text + " ]")
			LogRep.WriteMessage()

			LogRep.Param(Frame0602.Text, SchemeNames(SchemeType))
			LogRep.WriteMessage()

			'        LogRep.WriteMessage Frame0601.Caption
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.Param(_Label0601_0.Text, ComboBox0601.Text)
			LogRep.Param(_Label0601_1.Text, TextBox0601.Text, _Label0601_8.Text)
			LogRep.Param(_Label0601_2.Text, TextBox0602.Text, _Label0601_9.Text)
			LogRep.Param(_Label0601_3.Text, TextBox0603.Text, _Label0601_10.Text)
			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.Param(_Label0601_4.Text, TextBox0604.Text, _Label0601_11.Text)
			LogRep.Param(_Label0601_16.Text, TextBox0611.Text, _Label0601_19.Text)
			'        LogRep.WriteParam Label1598.Caption, TextBox1554.Text
			LogRep.WriteMessage(Label0601_12.Text)
			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.WriteMessage(Frame0603.Text)
			LogRep.Param(_Label0601_5.Text, TextBox0605.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0601_13.Text, TextBox0608.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0601_20.Text, TextBox0614.Text)
			LogRep.Param(_Label0601_17.Text, TextBox0612.Text, "°")
			LogRep.Param(_Label0601_18.Text, TextBox0613.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.WriteMessage(Frame0604.Text)
			LogRep.Param(_Label0601_6.Text, TextBox0606.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0601_7.Text, TextBox0607.Text, My.Resources.str00011)

			LogRep.Param(ComboBox0605.Text, TextBox0609.Text, HeightConverter(HeightUnit).Unit)

			LogRep.Param(_Label0601_21.Text, TextBox0615.Text)
			LogRep.Param(_Label0601_14.Text, TextBox0610.Text, _Label0601_15.Text) 'SpeedConverter(SpeedUnit).Unit)
			LogRep.Param(_Label0601_22.Text, ComboBox0604.Text, _Label0601_29.Text) 'HeightConverter(HeightUnit).Unit)
			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			If CheckBox0601.Enabled And CheckBox0601.Checked Then LogRep.WriteMessage(CheckBox0601.Text)
			If CheckBox0602.Enabled And CheckBox0602.Checked Then LogRep.WriteMessage(CheckBox0602.Text)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
		End If
		'===========================================================================

		If CheckBox0701.Checked Then
			LogRep.ExH2(MultiPage1.TabPages.Item(7).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(7).Text + " ]")
			LogRep.WriteMessage()

			LogRep.Param(_Label0701_0.Text, TextBox0701.Text, _Label0701_5.Text)

			fTmp = RadToDeg(Math.Atan(0.01 * CDbl(TextBox0701.Text)))
			LogRep.Param(My.Resources.str10518, CStr(Math.Round(fTmp, 2)), "°")

			LogRep.Param(ComboBox0701.Text, TextBox0702.Text, HeightConverter(HeightUnit).Unit)
			'    LogRep.WriteParam _Label0701_8.Caption, TextBox0705.Text, SpeedConverter(SpeedUnit).Unit
			LogRep.Param(_Label0701_9.Text, TextBox0706.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(_Label0701_2.Text, ComboBox0702.Text, _Label0701_6.Text)
			LogRep.WriteMessage()

			LogRep.Param(_Label0701_10.Text, TextBox0707.Text, DistanceConverter(DistanceUnit).Unit)
			If _Label0701_6.Text = "DME" Then
				LogRep.WriteMessage(_Label0701_7.Text)
			End If

			LogRep.Param(ComboBox0705.Text, TextBox0703.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label0701_11.Text, TextBox0708.Text)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
		End If

		'===========================================================================
		LogRep.ExH2(MultiPage1.TabPages.Item(8).Text)
		LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(8).Text + " ]")
		LogRep.WriteMessage()

		If OptionButton0802.Checked Then
			LogRep.Param(Frame0801.Text, OptionButton0802.Text)
		Else
			LogRep.Param(Frame0801.Text, OptionButton0801.Text)
		End If

		LogRep.Param(_Label0801_0.Text, TextBox0801.Text, DistanceConverter(DistanceUnit).Unit)
		LogRep.WriteMessage(_Label0801_1.Text)
		LogRep.WriteMessage(_Label0801_2.Text)
		LogRep.WriteMessage(_Label0801_3.Text)
		LogRep.WriteMessage(_Label0801_4.Text)
		LogRep.Param(ComboBox0803.Text, TextBox0802.Text, HeightConverter(HeightUnit).Unit)

		LogRep.Param(_Label0801_9.Text, TextBox0804.Text, "%")
		LogRep.Param(_Label0801_10.Text, TextBox0805.Text, DistanceConverter(DistanceUnit).Unit)

		LogRep.Param(_Label0801_12.Text, TextBox0807.Text)
		LogRep.Param(_Label0801_14.Text, TextBox0808.Text)
		LogRep.WriteMessage()

		LogRep.Param(ComboBox0801.Text, _Label0801_6.Text)
		LogRep.WriteMessage(_Label0801_5.Text)

		LogRep.Param(_Label0801_19.Text, TextBox0809.Text, _Label0801_20.Text)
		LogRep.Param(label0801_21.Text, TextBox0810.Text, label0801_22.Text)
		'===========================================================================

		If CheckBox0801.Checked Then
			LogRep.WriteMessage()
			LogRep.WriteMessage()

			LogRep.ExH2(MultiPage1.TabPages.Item(9).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(9).Text + " ]")
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.Param(_Label0901_0.Text, TextBox0901.Text, "°")
			LogRep.Param(_Label0901_3.Text, ComboBox0901.Text)
			LogRep.Param(_Label0901_1.Text, TextBox0902.Text, SpeedConverter(SpeedUnit).Unit)
			LogRep.Param(_Label0901_4.Text, TextBox0903.Text)

			LogRep.WriteMessage(_Label0901_2.Text)

			If OptionButton0901.Checked Then
				LogRep.Param(Frame0901.Text, OptionButton0901.Text)
			Else
				LogRep.Param(Frame0901.Text, OptionButton0902.Text)
			End If

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(10).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(10).Text + " ]")
			LogRep.WriteMessage()

			LogRep.Param(Label1001_0.Text, TextBox1001.Text, HeightConverter(HeightUnit).Unit)
			LogRep.WriteMessage(_Label1001_6.Text)
			LogRep.Param(Label1001_1.Text, TextBox1002.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label1001_7.Text, TextBox1007.Text)
			LogRep.Param(ComboBox1001.Text, TextBox1003.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label1001_8.Text, TextBox1008.Text)
			LogRep.Param(ComboBox1002.Text, TextBox1004.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label1001_4.Text, TextBox1005.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label1001_9.Text, TextBox1009.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(_Label1001_5.Text, TextBox1006.Text, HeightConverter(HeightUnit).Unit)
			LogRep.Param(_Label1001_10.Text, TextBox1010.Text, DistanceConverter(DistanceUnit).Unit)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(11).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(11).Text + " ]")
			LogRep.WriteMessage()

			If Frame1101.Visible Then
				LogRep.WriteMessage(Frame1101.Text)
			Else
				LogRep.WriteMessage(Frame1102.Text)
				LogRep.Param(_Label1101_0.Text, ComboBox1102.Text, _Label1101_3.Text)
				LogRep.Param(_Label1101_1.Text, TextBox1102.Text, _Label1101_13.Text)
				If _Label1101_3.Text = "DME" Then
					If OptionButton1101.Checked Then
						LogRep.WriteMessage(OptionButton1101.Text)
					Else
						LogRep.WriteMessage(OptionButton1102.Text)
					End If
				End If
			End If

			LogRep.WriteMessage(_Label1101_2.Text)
			LogRep.WriteMessage()

			LogRep.Param(ComboBox1101.Text, TextBox1101.Text, _Label1101_4.Text)
			LogRep.Param(_Label1101_5.Text, TextBox1103.Text, SpeedConverter(SpeedUnit).Unit)
			LogRep.Param(_Label1101_6.Text, TextBox1104.Text, SpeedConverter(SpeedUnit).Unit)
			LogRep.Param(_Label1101_7.Text, TextBox1105.Text, My.Resources.str00014)
			LogRep.Param(_Label1101_8.Text, TextBox1106.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(_Label1101_11.Text, TextBox1109.Text, DistanceConverter(DistanceUnit).Unit)
			LogRep.Param(_Label1101_12.Text, TextBox1110.Text, DistanceConverter(DistanceUnit).Unit)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			LogRep.ExH2(MultiPage1.TabPages.Item(12).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(12).Text + " ]")
			LogRep.WriteMessage()

			If OptionButton1201.Checked Then
				LogRep.WriteMessage((OptionButton1201.Text))
			ElseIf OptionButton1202.Checked Then
				LogRep.WriteMessage((OptionButton1202.Text))
			Else
				LogRep.WriteMessage((OptionButton1203.Text))
			End If

			If _Label1201_7.Visible Then LogRep.WriteMessage(_Label1201_7.Text)

			LogRep.Param(_Label1201_0.Text, TextBox1201.Text, _Label1201_8.Text)
			LogRep.Param(_Label1201_5.Text, TextBox1203.Text, _Label1201_8.Text)

			If _Label1201_1.Visible Then LogRep.Param(_Label1201_1.Text, ComboBox1201.Text, _Label1201_3.Text)
			If _Label1201_6.Visible Then LogRep.Param(_Label1201_6.Text, TextBox1204.Text, _Label1201_9.Text)
			If _Label1201_11.Visible Then LogRep.Param(_Label1201_11.Text, TextBox1205.Text, _Label1201_4.Text)
			If _Label1201_2.Visible Then LogRep.Param(_Label1201_2.Text, TextBox1202.Text, DistanceConverter(DistanceUnit).Unit)
			If CheckBox1201.Enabled And CheckBox1201.Checked Then LogRep.WriteMessage(CheckBox1201.Text)

			LogRep.WriteMessage()
			LogRep.WriteMessage()
			'===========================================================================

			If OptionButton1202.Checked Or (OptionButton1203.Checked And CheckBox1201.Enabled) Then
				'===========================================================================
				LogRep.ExH2(MultiPage1.TabPages.Item(13).Text)
				LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(13).Text + " ]")
				LogRep.WriteMessage()

				LogRep.WriteMessage((Frame1301.Text))
				If TextBox1301_0.Text <> "" Then LogRep.Param(_Label1301_0.Text, TextBox1301_0.Text)
				If OptionButton1301.Checked Then
					LogRep.WriteMessage((OptionButton1301.Text))
				ElseIf OptionButton1302.Checked Then
					LogRep.WriteMessage((OptionButton1302.Text))
				Else
					LogRep.WriteMessage((OptionButton1303.Text))
				End If
				LogRep.WriteMessage()

				LogRep.WriteMessage((Frame1302.Text))
				If TextBox1301_1.Text <> "" Then LogRep.Param(_Label1301_1.Text, TextBox1301_1.Text)
				If OptionButton1304.Checked Then
					LogRep.WriteMessage((OptionButton1304.Text))
				ElseIf OptionButton1305.Checked Then
					LogRep.WriteMessage((OptionButton1305.Text))
				Else
					LogRep.WriteMessage((OptionButton1306.Text))
				End If

				LogRep.WriteMessage()
				LogRep.WriteMessage()
				'===========================================================================

				LogRep.ExH2(MultiPage1.TabPages.Item(14).Text)
				LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(14).Text + " ]")
				LogRep.WriteMessage()

				LogRep.WriteMessage(Frame1401.Text)

				If CheckBox1401.Checked Then LogRep.WriteMessage((CheckBox1401.Text))
				If CheckBox1402.Checked Then LogRep.WriteMessage((CheckBox1402.Text))
				If CheckBox1403.Checked Then LogRep.WriteMessage((CheckBox1403.Text))

				LogRep.WriteMessage()

				LogRep.WriteMessage(Frame1402.Text)
				If CheckBox1404.Checked Then LogRep.WriteMessage((CheckBox1404.Text))
				If CheckBox1405.Checked Then LogRep.WriteMessage((CheckBox1405.Text))
				If CheckBox1406.Checked Then LogRep.WriteMessage((CheckBox1406.Text))

				LogRep.WriteMessage()
				LogRep.WriteMessage()
				'===========================================================================
			End If

			LogRep.ExH2(MultiPage1.TabPages.Item(15).Text)
			LogRep.HTMLMessage("[ " + MultiPage1.TabPages.Item(15).Text + " ]")
			LogRep.WriteMessage()

			LogRep.Param(_Label1501_0.Text, TextBox1501.Text, _Label1501_3.Text)
			'        LogRep.WriteParam _Label1501_1.Caption, TextBox1502.Text, _Label1501_4.Caption
			'        LogRep.WriteParam Label1209.Caption, Label1210.Caption
			'        LogRep.WriteMessage

			LogRep.Param(ComboBox1503.Text, TextBox1504.Text, _Label1501_7.Text)
			LogRep.Param(_Label1501_6.Text, TextBox1506.Text)
			'        LogRep.WriteParam Label1211.Caption, TextBox1204.Text
			LogRep.WriteMessage()
			LogRep.Param(_Label1501_2.Text, TextBox1503.Text, _Label1501_10.Text)
			'===========================================================================
		End If
		'===========================================================================
		LogRep.CloseFile()
	End Sub

	Private Function MissedApproachLegs(segment As TraceSegment, IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim result As ApproachLeg = DBModule.pObjectDir.CreateFeature(Of MissedApproachLeg)()
		'result.Approach = pProcedure.GetFeatureRef()
		result.AircraftCategory.Add(IsLimitedTo)

		Dim pSegmentLeg As SegmentLeg = result

		pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER
		pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL

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
		pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK

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
		'pSpeed.Value = CDbl(TextBox0902.Text)
		pSpeed.Value = Double.Parse(TextBox0902.Text)
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
			Else 'If (GuidNav.TypeCode <> eNavaidType.NONE) Then
				Dim pGuidNavSignPt As SignificantPoint = GuidNav.GetSignificantPoint()

				fDir = Functions.ReturnAngleInDegrees(GuidNav.pPtPrj, ptEnd)
				Angle = NativeMethods.Modulus(fCourse - GuidNav.MagVar)

				pAngleIndication = DBModule.CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)

				pAngleIndication.TrueAngle = fCourse
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
				If (GuidNav.Identifier <> Guid.Empty) And (GuidNav.TypeCode <> eNavaidType.NONE) And (SttIntersectNav.TypeCode <> eNavaidType.NONE) Then
					HorAccuracy = CalcHorisontalAccuracy(ptEnd, GuidNav, SttIntersectNav)
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

	Private Function IntermediateApproachLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		Dim fDir As Double

		Dim Angle As Double
		Dim fDist As Double

		Dim fDistToNav As Double
		Dim fAltitudeDif As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pFIXSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint

		Dim pSegmentPoint As SegmentPoint
		Dim pPointReference As PointReference
		Dim pStartPoint As TerminalSegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pAngleUse As AngleUse
		Dim pAngleIndication As AngleIndication
		Dim pStAngleIndication As AngleIndication
		Dim pStDistanceIndication As DistanceIndication

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical

		Dim GuidNav As NavaidData
		Dim IFIntesectNav As NavaidData
		Dim FAFIntesectNav As NavaidData

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

		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

		GuidNav = FinalNav

		If GuidNav.TypeCode = eNavaidType.DME Then
			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.AF
		Else
			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.TF
		End If

		IFIntesectNav = IFNavDat(ComboBox0401.SelectedIndex)
		FAFIntesectNav = FAFNavDat(ComboBox0302.SelectedIndex)

		'=======================================================================================
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Dir2Azt(IFPnt, _ArDir)

		If SideDef(IFPnt, _ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If
		'=======================================================================================
		'    pSegmentLeg.BankAngle.Value = fBankAngle
		'=======================================================================================

		If ComboBox0303.SelectedIndex = 0 Then
			fTmp = CDbl(TextBox0303.Text)
		Else
			fTmp = ConvertHeight(DeConvertHeight(CDbl(TextBox0303.Text)) + fRefHeight, eRoundMode.NEAREST)
		End If

		pDistanceVertical = New ValDistanceVertical(fTmp, mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		If ComboBox0402.SelectedIndex = 0 Then
			fTmp = CDbl(TextBox0404.Text)
		Else
			fTmp = ConvertHeight(DeConvertHeight(CDbl(TextBox0404.Text)) + fRefHeight, eRoundMode.NEAREST)
		End If

		pDistanceVertical = New ValDistanceVertical(fTmp, mUomVDistance)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical

		If pSegmentLeg.UpperLimitAltitude.Value <> pSegmentLeg.LowerLimitAltitude.Value Then
			pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		Else
			pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.AT_LOWER
		End If
		'======

		pDistance = New ValDistance(ConvertDistance(ReturnDistanceInMeters(PtFAF, IFPnt), eRoundMode.NEAREST), mUomHDistance)
		pSegmentLeg.Length = pDistance
		'======
		pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(IntermAreaPDG))
		'======

		pSpeed = New ValSpeed(ConvertSpeed(cViafMax.Values(Category), eRoundMode.SPECIAL), mUomSpeed)
		pSegmentLeg.SpeedLimit = pSpeed

		' Angle ========================

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, IFPnt)
		Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = Dir2Azt(IFPnt, fDir)
		pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

		pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()

		' Start Point ========================
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

		If (IFIntesectNav.IntersectionType = eIntersectionType.OnNavaid) Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			fDir = ReturnAngleInDegrees(IFIntesectNav.pPtPrj, IFPnt)
			Dim HorAccuracy As Double = CalcHorisontalAccuracy(IFPnt, GuidNav, IFIntesectNav)

			pFixDesignatedPoint = CreateDesignatedPoint(IFPnt, "IF", Azt2DirPrj(IFPnt, pSegmentLeg.Course))     '_ArDir -> Azt2DirPrj(IFPnt, pSegmentLeg.Course)

			If IFIntesectNav.TypeCode = eNavaidType.DME Then
				fDistToNav = ReturnDistanceInMeters(IFIntesectNav.pPtPrj, IFPnt)
				fAltitudeDif = IFPnt.Z - IFIntesectNav.pPtPrj.Z + fRefHeight

				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeDif * fAltitudeDif)
				pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
				pPointReference.Role = CodeReferenceRole.RAD_DME
			Else
				pAngleUse = New AngleUse
				'fDir = ReturnAngleInDegrees(IFIntesectNav.pPtPrj, IFPnt)

				Angle = Modulus(Dir2Azt(IFIntesectNav.pPtPrj, fDir) - IFIntesectNav.MagVar, 360.0)
				pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
				pStAngleIndication.TrueAngle = Dir2Azt(IFPnt, fDir)
				pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = False

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If

			pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
			pAngleUse = New AngleUse()
			pAngleUse.AlongCourseGuidance = True
			pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
			pPointReference.FacilityAngle.Add(pAngleUse)

			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

		End If
		'=======================
		PriorPostFixTolerance(pIFTolerArea, IFPnt, _ArDir, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pIFTolerArea))
		'========
		'pStartPoint. 		pPointReference
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

		pInterNavSignPt = FAFIntesectNav.GetSignificantPoint()

		If (FAFIntesectNav.IntersectionType = eIntersectionType.OnNavaid) Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			Dim HorAccuracy As Double = CalcHorisontalAccuracy(PtFAF, GuidNav, FAFIntesectNav)

			pFixDesignatedPoint = CreateDesignatedPoint(PtFAF, "FAF", Azt2DirPrj(PtFAF, pSegmentLeg.Course))

			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			If FAFIntesectNav.IntersectionType = eIntersectionType.ByDistance Then
				fDistToNav = ReturnDistanceInMeters(FAFIntesectNav.pPtPrj, PtFAF)
				fAltitudeDif = PtFAF.Z - FAFIntesectNav.pPtPrj.Z + fRefHeight
				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitudeDif * fAltitudeDif)
				pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
				pPointReference.Role = CodeReferenceRole.RAD_DME
			Else
				fDir = ReturnAngleInDegrees(FAFIntesectNav.pPtPrj, PtFAF)
				Angle = Modulus(Dir2Azt(FAFIntesectNav.pPtPrj, fDir) - FAFIntesectNav.MagVar, 360.0)

				pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
				pStAngleIndication.TrueAngle = Dir2Azt(PtFAF, fDir)
				pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse = New AngleUse
				pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = False

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If
		End If

		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, PtFAF)
		Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = Dir2Azt(PtFAF, fDir)
		pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
		'pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
		pAngleIndication.Fix = pFIXSignPt.FixDesignatedPoint

		pAngleUse = New AngleUse()
		pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		pAngleUse.AlongCourseGuidance = True
		pPointReference.FacilityAngle.Add(pAngleUse)
		'=========================================================================

		PriorPostFixTolerance(pFAFTolerArea, PtFAF, _ArDir, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pFAFTolerArea))

		pEndPoint.FacilityMakeup.Add(pPointReference)
		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.EndPoint = pEndPoint
		' End of EndPoint ========================

		' Trajectory =====================================================
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pLocation As Aran.Geometries.Point

		pLineStringSegment = New LineString

		pLocation = ESRIPointToARANPoint(ToGeo(IFPnt))
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(PtFAF))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve()
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve

		' protected Area =======================================================
		'DrawPolygon(IntermediateBaseArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(IntermediateSecondArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface

		pTopo = IntermediateBaseArea
		pPolygon = pTopo.Difference(IntermediateSecondArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(IntermediateSecondArea))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(_InterMOCH + fRefHeight), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = ImObstacles
		Dim i As Integer
		'Dim j As Integer

		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(obstacles.Obstacles(obstacles.Parts(i).Owner).Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		'//  END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END =====================================================
		Return pApproachLeg
	End Function

	Private Function FinalApproachToSDFLeg(ByVal ptFAFDescent As ESRI.ArcGIS.Geometry.IPoint, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		Dim fDir As Double
		Dim Angle As Double
		Dim fDist As Double

		Dim fAltitude As Double
		Dim fDistToNav As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim HaveFAF As Boolean

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pFIXSignPt As SignificantPoint
		Dim pPointReference As PointReference
		Dim pGuidNavSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint
		Dim pStartPoint As TerminalSegmentPoint

		Dim pAngleUse As AngleUse
		Dim pAngleIndication As AngleIndication
		Dim pStAngleIndication As AngleIndication
		Dim pStDistanceIndication As DistanceIndication

		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim GuidNav As NavaidData
		Dim IntersectNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical

		Dim pFromPoint As ESRI.ArcGIS.Geometry.IPoint

		HaveFAF = OptionButton0201.Checked

		GuidNav = FinalNav
		IntersectNav = SDFNavDat(ComboBox0702.SelectedIndex)

		'If HaveFAF Then
		'	pFromPoint = PtFAF
		'Else
		pFromPoint = ptFAFDescent
		'End If

		'=======================================================================================
		If (Not HaveFAF) And (ComboBox0604.SelectedIndex = 1) Then
			pApproachLeg = pObjectDir.CreateFeature(Of IntermediateLeg)()
		Else
			pApproachLeg = pObjectDir.CreateFeature(Of FinalLeg)()
			Dim pFinalLeg As FinalLeg
			pFinalLeg = pApproachLeg
			pFinalLeg.LandingSystemCategory = CodeApproachGuidance.NON_PRECISION
			If GuidNav.TypeCode = eNavaidType.LLZ Then
				pFinalLeg.GuidanceSystem = CodeFinalGuidance.LOC
			ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
				pFinalLeg.GuidanceSystem = CodeFinalGuidance.NDB
			End If
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.VOR
		End If

		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pApproachLeg

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =
		'    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired
		'    pSegmentLeg.ReqNavPerformance
		'	pSegmentLeg.SpeedInterpretation =

		If GuidNav.TypeCode = eNavaidType.DME Then
			If HaveFAF Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.AF
			Else
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.AF
			End If
		Else
			If HaveFAF Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.TF
			Else
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
			End If
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS
		'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

		If SideDef(pFromPoint, _ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Dir2Azt(pFromPoint, _ArDir)

		'=======================================================================================
		'    pSegmentLeg.BankAngle.Value = fBankAngle
		'=======================================================================================
		If ComboBox0701.SelectedIndex = 0 Then
			fTmp = CDbl(TextBox0702.Text)
		Else
			fTmp = ConvertHeight(DeConvertHeight(CDbl(TextBox0702.Text)) + fRefHeight, eRoundMode.NEAREST)
		End If

		pDistanceVertical = New ValDistanceVertical(fTmp, mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'======
		If ComboBox0703.SelectedIndex = 0 Then
			fTmp = CDbl(TextBox0710.Text)
		Else
			fTmp = ConvertHeight(DeConvertHeight(CDbl(TextBox0710.Text)) + fRefHeight, eRoundMode.NEAREST)
		End If

		pDistanceVertical = New ValDistanceVertical(fTmp, mUomVDistance)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical
		'======

		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = ConvertDistance(ReturnDistanceInMeters(pFromPoint, PtSDF), eRoundMode.NEAREST)
		pSegmentLeg.Length = pDistance
		'======

		pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(0.01 * CDbl(TextBox0712.Text)))

		pSpeed = New ValSpeed(ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL), mUomSpeed)
		pSegmentLeg.SpeedLimit = pSpeed

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Angle ========================

		' Start Point ========================
		pStartPoint = pEndPoint

		'If HaveFAF Then
		'	'pStartPoint.Assign(pEndPoint)
		'	pStartPoint = pEndPoint
		'Else
		'	pStartPoint = New TerminalSegmentPoint()
		'	'        pStartPoint.IndicatorFACF =      ??????????
		'	'        pStartPoint.LeadDME =            ??????????
		'	'        pStartPoint.LeadRadial =         ??????????
		'	pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF
		'	'==
		'	pSegmentPoint = pStartPoint
		'	pSegmentPoint.FlyOver = False
		'	pSegmentPoint.RadarGuidance = False

		'	pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		'	pSegmentPoint.Waypoint = False

		'	' ========================
		'	fDir = Azt2DirPrj(pFromPoint, pSegmentLeg.Course)

		'	pFixDesignatedPoint = CreateDesignatedPoint(pFromPoint, , fDir)
		'	pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.CNF
		'	pFIXSignPt = New SignificantPoint()
		'	pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

		'	pSegmentPoint.PointChoice = pFIXSignPt
		'End If

		pSegmentLeg.StartPoint = pStartPoint

		' End Of Start Point ========================

		' EndPoint ========================
		pEndPoint = New TerminalSegmentPoint
		'        pEndPoint.IndicatorFACF =      ??????????
		'        pEndPoint.LeadDME =            ??????????
		'        pEndPoint.LeadRadial =         ??????????
		If (Not HaveFAF) And (ComboBox0604.SelectedIndex = 1) Then
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FAF
		Else
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.SDF
		End If

		pSegmentPoint = pEndPoint

		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False
		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False

		' ========================
		pPointReference = New PointReference()

		pInterNavSignPt = IntersectNav.GetSignificantPoint()

		If (IntersectNav.IntersectionType = eIntersectionType.OnNavaid) Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			Dim HorAccuracy As Double = CalcHorisontalAccuracy(PtSDF, GuidNav, IntersectNav)

			pFixDesignatedPoint = CreateDesignatedPoint(PtSDF, "SDF", Azt2DirPrj(PtSDF, pSegmentLeg.Course))

			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			If IntersectNav.IntersectionType = eIntersectionType.ByDistance Then
				fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, PtSDF)
				fAltitude = PtSDF.Z - IntersectNav.pPtPrj.Z + fRefHeight
				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
				pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
				pPointReference.Role = CodeReferenceRole.RAD_DME
			Else
				pAngleUse = New AngleUse
				fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, PtSDF)

				Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
				pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
				pStAngleIndication.TrueAngle = Dir2Azt(PtSDF, fDir)
				pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = False

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If
		End If

		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, PtSDF)
		Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = Dir2Azt(PtSDF, fDir)
		pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
		pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()

		'pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef() 'pFIXSignPt.FixDesignatedPoint
		pAngleIndication.Fix = pFIXSignPt.FixDesignatedPoint

		pAngleUse = New AngleUse()
		pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		pAngleUse.AlongCourseGuidance = True

		pPointReference.FacilityAngle.Add(pAngleUse)
		'==============================================================

		PriorPostFixTolerance(pSDFTolerArea, PtSDF, _ArDir, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pSDFTolerArea))

		pEndPoint.FacilityMakeup.Add(pPointReference)
		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.EndPoint = pEndPoint

		' End Of EndPoint  ========================

		'Dim pPolyline As ipolyline
		'pPolyline = New polyline
		'pPolyline.FromPoint = pFromPoint
		'pPolyline.ToPoint = PtSDF
		'DrawPolyLine(pPolyline, 255, 2)
		'Application.DoEvents()
		' Trajectory =====================================================
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		pLineStringSegment = New LineString

		pLocation = ESRIPointToARANPoint(ToGeo(pFromPoint))
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(PtSDF))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve
		' Trajectory =====================================================

		' protected Area =======================================================
		'Private SDFFinalSecondArea As ESRI.ArcGIS.Geometry.IPointCollection
		'Private SDFFinalBaseArea As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface

		'DrawPolygon(SDFFinalBaseArea, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(SDFFinalSecondArea, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'Application.DoEvents()

		pTopo = SDFFinalBaseArea
		pPolygon = pTopo.Difference(SDFFinalSecondArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'While True
		'	Application.DoEvents()
		'End While

		'DrawPolygon(pPolygon, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))
		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		pSurface = ESRIPolygonToAIXMSurface(ToGeo(SDFFinalSecondArea))
		Dim pSecProtectedArea As ObstacleAssessmentArea
		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(SDFOCH + fRefHeight), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		'If Not HaveFAF Then Return pApproachLeg
		'pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		'pSegmentLeg.DesignSurface.Add(pSecProtectedArea)
		'Return pApproachLeg

		' Protection Area Obstructions list ==================================================

		Dim obstacles As ObstacleContainer   'FAFWorkObstacleList	'BuffObstacleList

		If HaveFAF Then
			obstacles = FAFWorkObstacleList
		Else
			obstacles = NavObstacleList
		End If

		Dim pReleational As IRelationalOperator = SDFFinalBaseArea
		Dim i As Integer
		'Dim j As Integer

		'For i = 0 To obstacles.Obstacles.Length - 1
		'	For j = 0 To obstacles.Obstacles(i).PartsNum - 1
		'		DrawPointWithText(obstacles.Parts(obstacles.Obstacles(i).Parts(j)).pPtPrj, "")
		'	Next
		'Next

		'While True
		'	Application.DoEvents()
		'End While

		'Added by Agshin
		'Save max 5 obstacles as obstruction

		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		'For i = 0 To obstacles.Obstacles.Length - 1
		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0
			Dim bContains As Boolean = False

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			If pReleational.Disjoint(obstacles.Parts(i).pPtPrj) Then Continue For
			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			bContains = True
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			If Not bContains Then Continue For

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================
		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END of FinalApproachToSDFLeg =================================================

		Return pApproachLeg
	End Function

	Private Function FinalApproachLeg(ByVal ptFAFDescent As ESRI.ArcGIS.Geometry.IPoint, ByRef ToBeContinued As Integer, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pStartPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		Dim fDir As Double
		Dim Angle As Double
		Dim fDist As Double
		Dim fMAPtPDG As Double
		Dim fAltitude As Double
		Dim fDistToNav As Double
		Dim MAPtDistToTHR As Double
		Dim MAPtDistVerLower As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim HaveTP As Boolean
		Dim HaveSDF As Boolean
		Dim HaveFAF As Boolean

		Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pSegmentPoint As SegmentPoint
		Dim pEndPoint As TerminalSegmentPoint
		Dim pPointReference As PointReference
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pAngleUse As AngleUse
		Dim pAngleIndication As AngleIndication
		Dim pStAngleIndication As AngleIndication
		Dim pStDistanceIndication As DistanceIndication

		Dim GuidNav As NavaidData
		Dim IntersectNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical

		Dim pToPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pFromPoint As ESRI.ArcGIS.Geometry.IPoint

		HaveSDF = CheckBox0701.Checked
		HaveFAF = OptionButton0201.Checked
		HaveTP = CheckBox0801.Checked

		GuidNav = FinalNav

		pApproachLeg = pObjectDir.CreateFeature(Of FinalLeg)()

		Dim pFinalLeg As FinalLeg
		pFinalLeg = pApproachLeg
		pFinalLeg.LandingSystemCategory = CodeApproachGuidance.NON_PRECISION
		If GuidNav.TypeCode = eNavaidType.LLZ Then
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.LOC
		ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.NDB
		End If
		pFinalLeg.GuidanceSystem = CodeFinalGuidance.VOR

		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		'=======================================================================================
		pSegmentLeg = pApproachLeg

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =
		'    pSegmentLeg.Duration = '???????????????????????????????? pLegs(I).valDur
		'    pSegmentLeg.Note =
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance =
		'    pSegmentLeg.SpeedInterpretation =

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT

		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		If HaveSDF Then
			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.TF
			fMAPtPDG = 0.01 * CDbl(TextBox0701.Text)
			pFromPoint = PtSDF
		Else
			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
			If HaveFAF Then
				fMAPtPDG = FinalAreaPDG
				pFromPoint = PtFAF
			Else
				fMAPtPDG = CPDGmin
				pFromPoint = ptFAFDescent
			End If
		End If

		'   0 - Far To THR
		'   1 - In THR Range
		'   2 - Ahead THR

		ToBeContinued = 1 ' In THR Range
		'= 0 - 1 ==================================
		pToPoint = pMAPt
		If OptionButton0801.Checked Then
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
		Else
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.DISTANCE
		End If
		'= 1 - 2 ==================================
		If OptionButton0101.Checked Then
			MAPtDistVerLower = arAbv_Treshold.Value + FictTHR.Z
		Else
			MAPtDistVerLower = fMinOCH + fRefHeight
		End If

		'= 0 ==================================
		MAPtDistToTHR = Point2LineDistancePrj(FictTHR, pMAPt, _ArDir + 90.0) * SideDef(FictTHR, _ArDir - 90.0, pMAPt)

		If MAPtDistToTHR > THRAccuracy Then
			ToBeContinued = 0 ' Far To THR
			MAPtDistVerLower = MAPtDistToTHR * fMAPtPDG + arAbv_Treshold.Value + FictTHR.Z

			'If HaveTP Then
			'	MAPtDistVerLower = DeConvertHeight(CDbl(TextBox1504.Text)) + fRefHeight
			'Else
			'	MAPtDistVerLower = DeConvertHeight(CDbl(TextBox0802.Text)) + fRefHeight
			'End If
		ElseIf MAPtDistToTHR < -THRAccuracy Then            '= 2 ==================================
			ToBeContinued = 2 ' Ahead THR
			pToPoint = FictTHR
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
		End If

		If SideDef(pFromPoint, _ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Dir2Azt(pFromPoint, _ArDir)
		'=======================================================================================
		'    Set pSegmentLeg.BankAngle.Value = fBankAngle
		'=======================================================================================

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(MAPtDistVerLower, eRoundMode.NEAREST), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'======
		If HaveSDF Then
			If ComboBox0701.SelectedIndex = 0 Then
				fTmp = CDbl(TextBox0702.Text)
			Else
				fTmp = ConvertHeight(DeConvertHeight(CDbl(TextBox0702.Text)) + fRefHeight, eRoundMode.NEAREST)
			End If
		ElseIf HaveFAF Then
			fTmp = ConvertHeight(PtFAF.Z + fRefHeight, eRoundMode.NEAREST)
		Else
			fTmp = ConvertHeight(ptFAFDescent.Z, eRoundMode.NEAREST)
		End If

		pDistanceVertical = New ValDistanceVertical(fTmp, mUomVDistance)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical

		'=============
		pDistance = New ValDistance(ConvertDistance(ReturnDistanceInMeters(pFromPoint, pToPoint), eRoundMode.NEAREST), mUomHDistance)
		pSegmentLeg.Length = pDistance
		'=============
		pSegmentLeg.VerticalAngle = -RadToDeg(System.Math.Atan(fMAPtPDG))
		'=============
		pSpeed = New ValSpeed(ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL), mUomSpeed)
		pSegmentLeg.SpeedLimit = pSpeed
		'=============

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Start Point ========================
		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point =================

		' EndPoint ===========================

		pEndPoint = New TerminalSegmentPoint
		'pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'pTerminalSegmentPoint.LeadDME =            ??????????
		'pTerminalSegmentPoint.LeadRadial =         ??????????
		'==

		pSegmentPoint = pEndPoint

		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False
		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False
		'=======================================================================
		'   0 - Far To THR
		'   1 - In THR Range
		'   2 - Ahead THR

		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, pToPoint)
		Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

		Dim bOnNav As Boolean = False
		IntersectNav.TypeCode = eNavaidType.NONE

		If ToBeContinued <> 2 And OptionButton0801.Checked Then
			IntersectNav = MAPtNavDat(ComboBox0801.SelectedIndex)
			bOnNav = (IntersectNav.IntersectionType = eIntersectionType.OnNavaid)
		End If

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
		pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
		pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()

		If Not bOnNav Then
			'pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

			pAngleUse = New AngleUse()
			pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
			pAngleUse.AlongCourseGuidance = True

			pPointReference = New PointReference()
			pPointReference.FacilityAngle.Add(pAngleUse)

			pFIXSignPt = New SignificantPoint()
		End If

		' ========================
		If ToBeContinued = 2 Then
			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FTP
			pFixDesignatedPoint = CreateDesignatedPoint(pToPoint, "FTHR", Azt2DirPrj(pToPoint, pSegmentLeg.Course))

			pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			pEndPoint.FacilityMakeup.Add(pPointReference)
		Else

			' ========================
			'If Not bOnNav Then
			'pFIXSignPt = New SignificantPoint()
			'pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
			'pAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
			'pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
			'pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

			'pAngleUse = New AngleUse()
			'pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
			'pAngleUse.AlongCourseGuidance = True

			'pPointReference = New PointReference()
			'pPointReference.FacilityAngle.Add(pAngleUse)
			'End If

			'=============================================================

			If bOnNav Then
				pFIXSignPt = pInterNavSignPt
			Else
				Dim HorAccuracy As Double = 0.0

				pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.MAPT
				If IntersectNav.TypeCode <> eNavaidType.NONE Then
					pInterNavSignPt = IntersectNav.GetSignificantPoint()
					HorAccuracy = CalcHorisontalAccuracy(pToPoint, GuidNav, IntersectNav)
				End If

				pFixDesignatedPoint = CreateDesignatedPoint(pToPoint, "MAPT", Azt2DirPrj(pToPoint, pSegmentLeg.Course))

				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				If IntersectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, pToPoint)
					fAltitude = pToPoint.Z - IntersectNav.pPtPrj.Z

					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
					pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
					pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
					pPointReference.Role = CodeReferenceRole.RAD_DME
				ElseIf IntersectNav.TypeCode <> eNavaidType.NONE Then
					fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, pToPoint)

					Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
					pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
					pStAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
					pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
					pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pAngleUse = New AngleUse
					pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
					pAngleUse.AlongCourseGuidance = False

					pPointReference.FacilityAngle.Add(pAngleUse)
					pPointReference.Role = CodeReferenceRole.INTERSECTION
				End If

				If IntersectNav.TypeCode <> eNavaidType.NONE Then
					PriorPostFixTolerance(pMAPtFIXPoly, pToPoint, _ArDir, PriorFixTolerance, PostFixTolerance)

					pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
					pDistanceSigned.Uom = mUomHDistance
					pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
					pPointReference.PriorFixTolerance = pDistanceSigned

					pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
					pDistanceSigned.Uom = mUomHDistance
					pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

					pPointReference.PostFixTolerance = pDistanceSigned
					pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pMAPtFIXPoly))
				End If

				pEndPoint.FacilityMakeup.Add(pPointReference)
			End If
		End If

		'If Not bOnNav Then pEndPoint.FacilityMakeup.Add(pPointReference)

		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.EndPoint = pEndPoint

		' End Of EndPoint ================================================
		Dim i As Integer
		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface
		Dim saveCount As Integer

		Dim pCondition As ApproachCondition = New ApproachCondition()
		pCondition.AircraftCategory.Add(IsLimitedTo)

		'pCondition.ClimbGradient = -ILS.GPAngle
		'pCondition.DesignSurface.Add(pPrimProtectedArea)

		Dim pMinimumSet As Minima = New Minima()

		If CheckBox0801.Checked Then
			pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(resultOCH + fRefHeight, eRoundMode.NEAREST), mUomVDistance)
			pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(resultOCH, eRoundMode.NEAREST), mUomVDistance)
		Else
			pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fFinalOCH + fRefHeight, eRoundMode.NEAREST), mUomVDistance)
			pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fFinalOCH, eRoundMode.NEAREST), mUomVDistance)
		End If

		pMinimumSet.AltitudeCode = CodeMinimumAltitude.OCA
		pMinimumSet.AltitudeReference = CodeVerticalReference.MSL
		pMinimumSet.HeightCode = CodeMinimumHeight.OCH
		pMinimumSet.HeightReference = CodeHeightReference.HAT

		pCondition.MinimumSet = pMinimumSet

		If OptionButton0102.Checked Then
			pSurface = ESRIPolygonToAIXMSurface(ToGeo(VsArea))

			' Assessment Area Obstructions list ==================================================
			Dim pAssessmentArea As ObstacleAssessmentArea = New ObstacleAssessmentArea()

			pAssessmentArea.Surface = pSurface
			pAssessmentArea.SectionNumber = 0
			pAssessmentArea.Type = CodeObstacleAssessmentSurface.FINAL
			'pDistanceVertical = New ValDistanceVertical(ConvertHeight(fNewVisAprOCH + fRefHeight), mUomVDistance)
			pAssessmentArea.AssessedAltitude = New ValDistanceVertical(ConvertHeight(fNewVisAprOCH + fRefHeight), mUomVDistance)

			Dim obstaclesMSA As ObstacleMSA() = MSAObstacleList

			Functions.SortMSAByReqH(obstaclesMSA, LBound(obstaclesMSA), UBound(obstaclesMSA))
			saveCount = Math.Min(obstaclesMSA.Length, 5)

			For i = 0 To saveCount - 1
				Dim RequiredClearance As Double = obstaclesMSA(i).ReqH - obstaclesMSA(i).Height

				Dim obs As Obstruction = New Obstruction()
				obs.VerticalStructureObstruction = New FeatureRef(obstaclesMSA(i).Identifier)

				'ReqH
				Dim MinimumAltitude As Double = obstaclesMSA(i).ReqH + fRefHeight

				pDistanceVertical = New ValDistanceVertical()
				pDistanceVertical.Uom = mUomVDistance
				pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
				obs.MinimumAltitude = pDistanceVertical

				'MOC
				pDistance = New ValDistance()
				pDistance.Uom = UomDistance.M
				pDistance.Value = RequiredClearance
				obs.RequiredClearance = pDistance

				pAssessmentArea.SignificantObstacle.Add(obs)
				pDistanceVertical = Nothing
				pDistance = Nothing
			Next
			pCondition.DesignSurface.Add(pAssessmentArea)
			' Assessment Area Obstructions list ==================================================
			' CirclingRestriction ================================================================
			Dim pCirclingRestriction As New CirclingRestriction

			Dim pTmpPoly As IPolygon
			Dim pRestrictionArea As IPolygon = New ArcGIS.Geometry.Polygon

			For i = 0 To pSubtrahend.Count - 1

				Dim bContains As Boolean
				Try
					bContains = ListView0202.CheckedIndices.Contains(i)
				Catch ex As Exception
					bContains = False
				End Try

				If Not bContains Then Continue For
				pTopo = ConvexPoly
				pTmpPoly = pTopo.Intersect(pSubtrahend(i), esriGeometryDimension.esriGeometry2Dimension)
				pTopo = pRestrictionArea
				pRestrictionArea = pTopo.Union(pTmpPoly)
			Next

			pCirclingRestriction.RestrictionArea = ESRIPolygonToAIXMSurface(ToGeo(pRestrictionArea))
			pCondition.CirclingRestriction.Add(pCirclingRestriction)
			' CirclingRestriction ================================================================
		End If

		pFinalLeg.Condition.Add(pCondition)

		'Dim pPolyline As ipolyline
		'pPolyline = New polyline
		'pPolyline.FromPoint = pFromPoint
		'pPolyline.ToPoint = pToPoint
		'DrawPolyLine(pPolyline, -1, 2)
		''While True
		'Application.DoEvents()
		''End While
		' Trajectory =====================================================
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		pLineStringSegment = New LineString

		pLocation = ESRIPointToARANPoint(ToGeo(pFromPoint))
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(pToPoint))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve
		' Trajectory =====================================================

		' protected Area =======================================================
		'DrawPolygon(FinalBaseArea, RGB(0, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(FinalSecondArea, RGB(0, 0, 255), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'Application.DoEvents()


		pTopo = FinalBaseArea
		pPolygon = pTopo.Difference(FinalSecondArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'While True
		'	Application.DoEvents()
		'End While

		'DrawPolygon(pPolygon, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(FinalSecondArea))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		' Protection Area Obstructions list ==================================================
		Dim pReleational As IRelationalOperator = FinalBaseArea
		Dim obstacles As ObstacleContainer
		'Dim j As Integer

		'If (Not HaveFAF) And (Not HaveSDF) Then
		'	obstacles = BuffObstacleList
		'Else
		obstacles = FAFObstacleList
		'End If

		'Added by Agshin
		'Save max 5 obstacles as obstruction
		' sort by effective height


		Functions.Sort(obstacles, 2)
		saveCount = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0
			Dim bContains As Boolean = False

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			If pReleational.Disjoint(obstacles.Parts(i).pPtPrj) Then Continue For
			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			bContains = True
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			If Not bContains Then Continue For

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================
		pDistanceVertical = New ValDistanceVertical(ConvertHeight(fFinalOCA), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END of FinalApproachLeg =================================================
		pStartPoint = pEndPoint
		Return pApproachLeg
	End Function

	Private Function FinalApproachContinuedLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fDir As Double
		Dim Angle As Double
		Dim fFinalAprLength As Double
		Dim MAPtDistVerLower As Double
		Dim HaveTP As Boolean

		Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pSegmentPoint As SegmentPoint
		Dim pStartPoint As TerminalSegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint
		Dim pLocation As Aran.Geometries.Point

		Dim GuidNav As NavaidData
		Dim IntersectNav As NavaidData

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceVertical As ValDistanceVertical

		Dim pToPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pFromPoint As ESRI.ArcGIS.Geometry.IPoint

		HaveTP = CheckBox0801.Checked

		GuidNav = FinalNav

		If HaveTP Then
			MAPtDistVerLower = resultOCH + fRefHeight
			'missAprPDG = 0.01 * CDbl(TextBox1501.Text)
		Else
			MAPtDistVerLower = MAPtOCH + fRefHeight
			'missAprPDG = 0.01 * CDbl(TextBox0804.Text)
		End If

		'=======================================================================================

		pFromPoint = FictTHR
		pToPoint = pMAPt
		pApproachLeg = pObjectDir.CreateFeature(Of FinalLeg)()

		Dim pFinalLeg As FinalLeg
		pFinalLeg = pApproachLeg
		pFinalLeg.LandingSystemCategory = CodeApproachGuidance.NON_PRECISION

		If GuidNav.TypeCode = eNavaidType.LLZ Then
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.LOC
		ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
			pFinalLeg.GuidanceSystem = CodeFinalGuidance.NDB
		End If
		pFinalLeg.GuidanceSystem = CodeFinalGuidance.VOR

		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF

		If OptionButton0801.Checked Then
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
		Else
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.DISTANCE
		End If

		If SideDef(pFromPoint, _ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Dir2Azt(pFromPoint, _ArDir)
		'=======================================================================================
		'    pSegmentLeg.BankAngle.Value = fBankAngle
		'=======================================================================================
		pDistanceVertical = New ValDistanceVertical(ConvertHeight(MAPtDistVerLower, eRoundMode.NEAREST), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'======
		'    fTmp = ConvertHeight(fTmp, eRoundMode.rmNERAEST)
		'    Set pDistance = New ValDistanceTypeCom
		'    pDistance.Uom = mVUomDistance
		'    pDistance.Value = CStr(fTmp)
		'    Set pSegmentLeg.UpperLimitAltitude = pDistance
		'======
		fFinalAprLength = 0.0
		fFinalAprLength = ReturnDistanceInMeters(pFromPoint, pToPoint)

		pDistance = New ValDistance(ConvertDistance(fFinalAprLength, eRoundMode.NEAREST), mUomHDistance)
		pSegmentLeg.Length = pDistance
		'======
		'pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(missAprPDG))
		'======

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed

		pSpeed.Value = ConvertSpeed(cVfafMax.Values(Category), eRoundMode.SPECIAL)

		pSegmentLeg.SpeedLimit = pSpeed
		'===================================================================
		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Start Point ======================================================
		pStartPoint = pEndPoint
		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ===============================================

		' EndPoint =========================================================


		pEndPoint = New TerminalSegmentPoint
		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????

		pSegmentPoint = pEndPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False
		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False

		' ========================

		If ReturnDistanceInMeters(GuidNav.pPtPrj, pToPoint) < 2.0 Then
			fDir = pSegmentLeg.Course
		Else
			fDir = Dir2Azt(GuidNav.pPtPrj, ReturnAngleInDegrees(GuidNav.pPtPrj, pToPoint))
		End If

		Angle = Modulus(fDir - GuidNav.MagVar, 360.0)

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		'pAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
		'pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
		'pSegmentLeg.Angle = pAngleIndication.Clone()

		' ========================
		Dim pPointReference As PointReference
		Dim fDistToNav As Double
		Dim fAltitude As Double
		Dim fDist As Double

		Dim pAngleIndication As AngleIndication
		Dim pStDistanceIndication As DistanceIndication
		Dim pStAngleIndication As AngleIndication
		Dim pAngleUse As AngleUse

		'pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

		'=============================================================
		If OptionButton0801.Checked Then
			IntersectNav = MAPtNavDat(ComboBox0801.SelectedIndex)
			pInterNavSignPt = IntersectNav.GetSignificantPoint()

			Dim bOnNav As Boolean = False
			Dim HorAccuracy As Double = CalcHorisontalAccuracy(pToPoint, GuidNav, IntersectNav)

			pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.MAPT
			pFixDesignatedPoint = CreateDesignatedPoint(pToPoint, "MAPT", Azt2DirPrj(pToPoint, pSegmentLeg.Course))

			bOnNav = (IntersectNav.IntersectionType = eIntersectionType.OnNavaid)
			' ========================
			If Not bOnNav Then
				pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse = New AngleUse()
				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = True

				pPointReference = New PointReference()
				pPointReference.FacilityAngle.Add(pAngleUse)
			End If

			If bOnNav Then
				pFIXSignPt = pInterNavSignPt
				pPointReference = New PointReference()
				pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
			Else
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				If IntersectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, pToPoint)
					fAltitude = pToPoint.Z - IntersectNav.pPtPrj.Z
					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
					pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
					pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pPointReference.FacilityDistance.Add(pStDistanceIndication.GetFeatureRefObject())
					pPointReference.Role = CodeReferenceRole.RAD_DME
				Else
					fDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, pToPoint)

					Angle = Modulus(Dir2Azt(IntersectNav.pPtPrj, fDir) - IntersectNav.MagVar, 360.0)
					pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pInterNavSignPt)
					pStAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
					pStAngleIndication.IndicationDirection = CodeDirectionReference.FROM
					pStAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

					pAngleUse = New AngleUse
					pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
					pAngleUse.AlongCourseGuidance = False

					pPointReference.FacilityAngle.Add(pAngleUse)
					pPointReference.Role = CodeReferenceRole.INTERSECTION
				End If

				'If not bOnNav Then
				Dim pDistanceSigned As ValDistanceSigned
				Dim PriorFixTolerance As Double
				Dim PostFixTolerance As Double
				PriorPostFixTolerance(pMAPtFIXPoly, pToPoint, _ArDir, PriorFixTolerance, PostFixTolerance)
				pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				pDistanceSigned.Uom = mUomHDistance
				pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
				pPointReference.PriorFixTolerance = pDistanceSigned

				pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
				pDistanceSigned.Uom = mUomHDistance
				pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

				pPointReference.PostFixTolerance = pDistanceSigned
				pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pMAPtFIXPoly))
				'End If
			End If
			'Else
			'	pFixDesignatedPoint = CreateDesignatedPoint(pToPoint, "MAPT", Azt2DirPrj(pToPoint, pSegmentLeg.Course))

			'	pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
			'	pAngleIndication.TrueAngle = Dir2Azt(pToPoint, fDir)
			'	pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
			'	pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

			'	pAngleUse = New AngleUse()
			'	pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
			'	pAngleUse.AlongCourseGuidance = True

			'	pPointReference.FacilityAngle.Add(pAngleUse)

			'	pFIXSignPt = New SignificantPoint()
			'	pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()
			'End If

			'If Not bOnNav Then pEndPoint.FacilityMakeup.Add(pPointReference)
			pEndPoint.FacilityMakeup.Add(pPointReference)
			pSegmentPoint.PointChoice = pFIXSignPt
		End If

		pSegmentLeg.EndPoint = pEndPoint

		' End Of EndPoint ========================
		' Trajectory =====================================================

		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		pLineStringSegment = New LineString

		pLocation = ESRIPointToARANPoint(ToGeo(pFromPoint))
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(pToPoint))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve

		' Trajectory =====================================================
		' END of FinalApproachLeg Continued ==============================

		Return pApproachLeg
	End Function

	Private Function MissedApproachStraight(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim hMATF As Double
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

		Dim pApproachLeg As ApproachLeg
		Dim pSegmentLeg As SegmentLeg

		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint

		Dim pStDistanceIndication As DistanceIndication
		Dim pStAngleIndication As AngleIndication
		Dim pAngleIndication As AngleIndication
		Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidData
		Dim IntersectNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical
		Dim pSpeed As ValSpeed

		'=======================================================================================

		GuidNav = FinalNav

		pApproachLeg = pObjectDir.CreateFeature(Of MissedApproachLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.ABOVE_LOWER
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER

		'If ComboBox0901.SelectedIndex = 0 Then
		'	pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		'Else
		'	pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		'End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT

		If OptionButton0901.Checked Then
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
			pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CA
		Else
			pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT

			IntersectNav = TurnInterNavDat(ComboBox1102.SelectedIndex)

			If IntersectNav.IntersectionType = eIntersectionType.ByDistance Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CD
			ElseIf IntersectNav.IntersectionType <> eIntersectionType.OnNavaid Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CR
			Else
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
			End If
		End If

		If SideDef(pMAPt, _ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Dir2Azt(pMAPt, _ArDir)
		'=======================================================================================
		'    pSegmentLeg.BankAngle.Value = fBankAngle
		'=======================================================================================

		If ComboBox1101.SelectedIndex = 0 Then
			hMATF = DeConvertHeight(CDbl(TextBox1101.Text))
		Else
			hMATF = DeConvertHeight(CDbl(TextBox1101.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(hMATF, eRoundMode.NEAREST), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical
		'======
		'	Set pDistance = New ValDistanceTypeCom
		'	pDistance.Uom = mVUomDistance
		'	fTmp = ConvertHeight(fTmp, eRoundMode.rmNERAEST)
		'	pDistance.Value = CStr(fTmp)
		'	Set pSegmentLeg.UpperLimitAltitude = pDistance
		'======
		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = ConvertDistance(ReturnDistanceInMeters(TurnFixPnt, pMAPt), eRoundMode.NEAREST)
		pSegmentLeg.Length = pDistance
		'======
		pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
		'======
		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0902.Text)
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Start Point ==========================================================
		pSegmentLeg.StartPoint = pEndPoint
		' End Of Start Point ===================================================

		' EndPoint =============================================================
		pEndPoint = New TerminalSegmentPoint
		'		pEndPoint.IndicatorFACF =      ??????????
		'		pEndPoint.LeadDME =            ??????????
		'		pEndPoint.LeadRadial =         ??????????
		pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP

		pSegmentPoint = pEndPoint

		'pSegmentPoint.FlyOver = True
		pSegmentPoint.RadarGuidance = False
		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False

		'=======================================================================
		pPointReference = New PointReference()
		'=======================================================================

		If OptionButton0901.Checked Then
			pFixDesignatedPoint = CreateDesignatedPoint(TurnFixPnt, "MATF", Azt2DirPrj(TurnFixPnt, pSegmentLeg.Course))
			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, TurnFixPnt)
			Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

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

				pFixDesignatedPoint = CreateDesignatedPoint(TurnFixPnt, "MATF", Azt2DirPrj(TurnFixPnt, pSegmentLeg.Course))
				pFIXSignPt = New SignificantPoint()
				pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

				fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, TurnFixPnt)
				Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

				pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
				pAngleIndication.TrueAngle = Dir2Azt(TurnFixPnt, fDir)
				pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
				pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

				pAngleUse = New AngleUse()
				pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				pAngleUse.AlongCourseGuidance = True

				pPointReference.FacilityAngle.Add(pAngleUse)

				If IntersectNav.TypeCode = eNavaidType.DME Then
					fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, TurnFixPnt)
					fAltitude = TurnFixPnt.Z - IntersectNav.pPtPrj.Z

					fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
					pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
					pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

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

			pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
			pPointReference.PriorFixTolerance = pDistanceSigned

			pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
			pDistanceSigned.Uom = mUomHDistance
			pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

			pPointReference.PostFixTolerance = pDistanceSigned
			pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(pTPTolerArea))

			pEndPoint.FacilityMakeup.Add(pPointReference)
		End If

		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.EndPoint = pEndPoint
		' End Of EndPoint ========================
		'Dim pPolyline As ipolyline
		'pPolyline = New polyline
		'pPolyline.FromPoint = MAPtPrj
		'pPolyline.ToPoint = TurnFixPnt
		'DrawPolyLine(pPolyline, -1, 2)
		'Application.DoEvents()

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

		Dim dd As Double = Point2LineDistancePrj(pMAPt, TurnFixPnt, _ArDir + 90.0)
		If dd <= 10.0 Then Return pApproachLeg
		' protected Area =======================================================

		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTopo As ITopologicalOperator2

		pCutter = New ESRI.ArcGIS.Geometry.Polyline

		'If bTurnFIXSameMAPtFIX Then
		'	pCutter1.FromPoint = PointAlongPlane(KK.FromPoint, m_ArDir - 90.0, 10.0 * RModel)
		'	pCutter1.ToPoint = PointAlongPlane(KK.ToPoint, m_ArDir + 90.0, 10.0 * RModel)
		'Else
		'	pCutter1.FromPoint = PointAlongPlane(MAPtPrj, m_ArDir - 90.0, 10.0 * RModel)
		'	pCutter1.ToPoint = PointAlongPlane(MAPtPrj, m_ArDir + 90.0, 10.0 * RModel)
		'End If

		If OptionButton0901.Checked Then
			pCutter.FromPoint = PointAlongPlane(TurnFixPnt, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(TurnFixPnt, _ArDir + 90.0, 10.0 * RModel)
		Else
			pCutter.FromPoint = PointAlongPlane(KK.FromPoint, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(KK.ToPoint, _ArDir + 90.0, 10.0 * RModel)
		End If

		'pTopo = pMAStraightSecLPoly
		'pMAStraightSecPoly = pTopo.Union(pMAStraightSecRPoly)
		'pTopo = pMAStraightSecPoly
		'pTopo.IsKnownSimple_2 = False
		'pTopo.Simplify()

		'DrawPolygon(pMAStraightPrimPoly, RGB(0, 128, 128), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pMAStraightSecPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolyLine(pCutter, 255, 2)
		'Application.DoEvents()

		Dim pFullAreaPoly As IPolygon
		Dim pSecndAreaPoly As IPolygon
		Dim pPolygon As IPolygon
		Dim Relation As IRelationalOperator

		'DrawPolygon(pMAStraightPrimPoly, RGB(0, 128, 128), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pMAStraightSecPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolyLine(pCutter, 255, 2)

		'DrawPointWithText(TurnFixPnt, "TurnFixPnt")
		'DrawPointWithText(pMAPt, "pMAPt")

		'While True
		'	Application.DoEvents()
		'End While

		pTopo = pMAStraightPrimPoly
		pTopo.Cut(pCutter, pFullAreaPoly, pPolygon)
		pTopo = pFullAreaPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'If TurnDir < 0 Then
		'	pTopo = pMAStraightSecLPoly
		'Else
		'	pTopo = pMAStraightSecRPoly
		'End If

		'DrawPolygon(pMAStraightSecPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolyLine(pCutter, 255, 2)
		'Application.DoEvents()

		Relation = pMAStraightSecPoly

		If Relation.Disjoint(pCutter) Then
			pSecndAreaPoly = pMAStraightSecPoly
		Else
			pTopo = pMAStraightSecPoly
			pTopo.Cut(pCutter, pSecndAreaPoly, pPolygon)
			pTopo = pSecndAreaPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		End If


		'While True
		'	Application.DoEvents()
		'End While

		'CutPoly(pMAStraightPrimPoly, pCutter, 1)
		'CutPoly(pMAStraightSecPoly, pCutter, 1)

		'DrawPolygon(pFullAreaPoly, RGB(128, 128, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pSecndAreaPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)

		'DrawPolygon(pMAStraightSecLPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()
		'DrawPolygon(pMAStraightSecRPoly, RGB(192, 192, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolyLine(pCutter, 255, 2)
		'Application.DoEvents()

		Dim pSurface As Surface

		pTopo = pFullAreaPoly
		pPolygon = pTopo.Difference(pSecndAreaPoly)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pSecndAreaPoly))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = MAPtObstacles
		Dim i As Integer
		'Dim j As Integer

		'Added by Agshin
		'Save max 5 obstacles as obstruction

		'sort by effectiveHeight
		Functions.Sort(obstacles, 4)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0


			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)
			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================
		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END of MissedApproachStraight =================================================
		Return pApproachLeg
	End Function

	Private Function MissedApproachTermination(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fDir As Double
		Dim fTmp As Double
		Dim i As Integer
		Dim j As Integer

		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim HaveTP As Boolean
		Dim HaveIntercept As Boolean

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pSegmentPoint As SegmentPoint
		Dim pFIXSignPt As SignificantPoint
		Dim pPointReference As PointReference
		Dim pLocation As Aran.Geometries.Point

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical

		HaveTP = CheckBox0801.Checked
		HaveIntercept = OptionButton1202.Checked
		'=======================================================================================
		pApproachLeg = pObjectDir.CreateFeature(Of MissedApproachLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)
		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.ABOVE_LOWER
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		'pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.EITHER
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		' ======================================================================
		Dim sTermAlt As String
		Dim fTermAlt As Double
		Dim fFixAltitide As Double
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		If HaveTP Then
			If HaveIntercept Then
				sTermAlt = TextBox1502.Text
			Else
				sTermAlt = TextBox1510.Text
			End If
		Else
			sTermAlt = TextBox0809.Text
		End If

		fFixAltitide = Functions.DeConvertHeight(CDbl(sTermAlt))

		' Start Point ========================
		pSegmentLeg.StartPoint = pEndPoint
		' End Of Start Point ========================
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK

		If HaveTP Then
			If OptionButton1201.Checked Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VA
				pSegmentLeg.Course = Modulus(CDbl(TextBox1203.Text) + CurrADHP.MagVar, 360.0)
				pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
				pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
			ElseIf OptionButton1202.Checked Then
				pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.VI
				pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
				'fTmp = CDbl(TextBox1204.Text)
				pSegmentLeg.Course = ConvertAngle(Dir2Azt(MPtCollection.Point(1), MPtCollection.Point(1).M))
				pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT
				'IntersectNav = TerInterNavDat(ComboBox1502.SelectedIndex)
			Else            'If OptionButton1203.Value Then
				TerFixPnt = TurnDirector.pPtPrj

				If CheckBox1201.Checked Or (TurnDirector.TypeCode = eNavaidType.NONE) Then
					pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.DF
				Else
					pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF
				End If

				pSegmentLeg.Course = Modulus(CDbl(TextBox1203.Text) + CurrADHP.MagVar, 360.0)
				pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO

				'pFIXSignPt = pInterNavSignPt

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
				'	bOnNav = True
				'	'If ReturnDistanceInMeters(TurnWPT.pPtPrj, TerFixPnt) < 2.0 Then
				'	'	Angle = Modulus(pSegmentLeg.Course - TurnWPT.MagVar, 360.0)
				'	'	bOnNav = True
				'	'Else
				'	'	fDir = ReturnAngleInDegrees(TurnWPT.pPtPrj, TerFixPnt)
				'	'	Angle = Modulus(Dir2Azt(TurnWPT.pPtPrj, fDir) - TurnWPT.MagVar, 360.0)
				'	'End If

				'	'If Not bOnNav Then
				'	'	pGuidNavSignPt = New SignificantPoint()
				'	'	pGuidNavSignPt.NavaidSystem = TurnWPT.pFeature.GetFeatureRef()

				'	'	pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
				'	'	pAngleIndication.TrueAngle = Dir2Azt(TerFixPnt, fDir)
				'	'	'pAngleIndication.Fix =
				'	'	pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

				'	'	pAngleUse = New AngleUse()
				'	'	pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
				'	'	pAngleUse.AlongCourseGuidance = True

				'	'	pPointReference = New PointReference()
				'	'	pPointReference.FacilityAngle.Add(pAngleUse)
				'	'End If
				'End If
				'=======================================================================

				If TurnDirector.TypeCode <> eNavaidType.NONE Then
					Dim pSect0 As ESRI.ArcGIS.Geometry.IPointCollection
					Dim pLine As ESRI.ArcGIS.Geometry.IPointCollection
					Dim pClone As ESRI.ArcGIS.esriSystem.IClone

					Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
					Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

					Dim TrackToler As Double
					Dim dMin As Double
					Dim dMax As Double
					Dim D As Double

					Dim pInterceptPt As ESRI.ArcGIS.Geometry.IPoint
					pInterceptPt = MPtCollection.Point(MPtCollection.PointCount - 1)
					'fDir = pInterceptPt.M
					fDir = Azt2DirPrj(TerFixPnt, pSegmentLeg.Course)

					'=============================================
					If (TurnDirector.TypeCode = eNavaidType.VOR) Or (TurnDirector.TypeCode = eNavaidType.TACAN) Then
						TrackToler = VOR.TrackingTolerance
						VORFIXTolerArea(TurnDirector.pPtPrj, fDir, fFixAltitide, pSect0)
					Else
						TrackToler = NDB.TrackingTolerance
						NDBFIXTolerArea(TurnDirector.pPtPrj, fDir, fFixAltitide, pSect0)
					End If

					pTopo = pSect0
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()

					dMax = -RModel
					dMin = RModel

					For i = 0 To pSect0.PointCount - 1
						D = Point2LineDistancePrj(pInterceptPt, pSect0.Point(i), fDir - 90.0) * SideDef(pInterceptPt, fDir - 90.0, pSect0.Point(i))
						If D < dMin Then
							dMin = D
							pt1 = pSect0.Point(i)
						End If
						If D > dMax Then
							dMax = D
							pt2 = pSect0.Point(i)
						End If
					Next

					pLine = New ESRI.ArcGIS.Geometry.Polyline
					pLine.AddPoint(PointAlongPlane(pt1, fDir - 90.0, RModel))
					pLine.AddPoint(PointAlongPlane(pt1, fDir + 90.0, RModel))

					pClone = Prim
					pTermFIXTolerArea = pClone.Clone

					CutPoly(pTermFIXTolerArea, pLine, 1)

					pLine.RemovePoints(0, pLine.PointCount)
					pLine.AddPoint(PointAlongPlane(pt2, fDir - 90.0, RModel))
					pLine.AddPoint(PointAlongPlane(pt2, fDir + 90.0, RModel))

					CutPoly(pTermFIXTolerArea, pLine, -1)

					'=============================================

					pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
					'pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF

					pPointReference = New PointReference()
					pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD


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
				Else
					pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE
				End If

				pFIXSignPt = TurnDirector.GetSignificantPoint
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
		End If

		'=======================================================================================

		pDistanceVertical = New ValDistanceVertical(CDbl(sTermAlt), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		fTermAlt = fFixAltitide 'DeConvertHeight(pDistanceVertical.Value)
		'====================================================================================================

		pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
		'======
		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed

		If HaveTP Then
			pSpeed.Value = CDbl(TextBox0902.Text)
		Else
			pSpeed.Value = ConvertSpeed(cVmaFaf.Values(Category), eRoundMode.SPECIAL)
		End If
		pSegmentLeg.SpeedLimit = pSpeed

		'======
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		Dim pCutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim pPrimeAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pSecndAreaPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pTmpPoly As IPolygon
		Dim pPolygon As IPolygon
		Dim pSurface As Surface
		Dim obstacles As ObstacleContainer
		Dim sortIndex As Integer


		pCurve = New Curve
		pCutter = New ESRI.ArcGIS.Geometry.Polyline

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

			Dim pPath As IPointCollection
			Dim pPolyline As IGeometryCollection

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
			obstacles = ObstacleFinalMOCList
			sortIndex = 5

			Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
			Dim fCurrX, fCurrY, dBuffer As Double

			If Not Double.TryParse(TextBox1505.Text, dBuffer) Then
				dBuffer = -1.0
			Else
				dBuffer = Functions.DeConvertDistance(dBuffer)
			End If

			If dBuffer > TurnAreaMaxd0 Then dBuffer = TurnAreaMaxd0
			If dBuffer < arBufferMSA.Value Then dBuffer = arBufferMSA.Value

			If OptionButton1201.Checked Then                        'Or (TurnDirector.TypeCode <= eNavaidType.NONE) 
				Dim pClone As ESRI.ArcGIS.esriSystem.IClone
				Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection
				Dim pTurnComplitPt As ESRI.ArcGIS.Geometry.IPoint

				pTurnComplitPt = MPtCollection.Point(MPtCollection.PointCount - 1)

				pClone = mPoly
				pPointCollection = pClone.Clone
				pPointCollection.AddPoint(PointAlongPlane(pTurnComplitPt, pTurnComplitPt.M, 10.0 * RModel))

				ptCnt = pTurnComplitPt

				For i = 0 To pPointCollection.PointCount - 1
					PrjToLocal(ptCnt, _OutDir, pPointCollection.Point(i), fCurrX, fCurrY)
					If Math.Abs(fCurrY) > distEps Then Continue For
					If fCurrX > 0.0 Then
						ptCnt = pPointCollection.Point(i)
					End If
				Next
			ElseIf OptionButton1202.Checked Then
				ptCnt = TerFixPnt
			Else 'If OptionButton1203.Checked Then
				ptCnt = TurnDirector.pPtPrj
			End If

			For i = 0 To m_BasePoints.PointCount - 1
				PrjToLocal(ptCnt, _OutDir, m_BasePoints.Point(i), fCurrX, fCurrY)
				If fCurrX > 0.0 Then ptCnt = m_BasePoints.Point(i)
			Next

			PrjToLocal(ptCnt, _OutDir, TurnFixPnt, fCurrX, fCurrY)
			If fCurrX > 0.0 Then ptCnt = TurnFixPnt

			' =====================================================================================
			pCutter = New Polyline()

			If OptionButton1201.Checked Then
				pCutter.FromPoint = Functions.LocalToPrj(ptCnt, _OutDir, 0.0, -10.0 * RModel)       '2.0 * GlobalVars.RModel
				pCutter.ToPoint = Functions.LocalToPrj(ptCnt, _OutDir, 0.0, 10.0 * RModel)
			Else
				pCutter.FromPoint = Functions.LocalToPrj(ptCnt, _OutDir, dBuffer, -10.0 * RModel)   '2.0 * GlobalVars.RModel
				pCutter.ToPoint = Functions.LocalToPrj(ptCnt, _OutDir, dBuffer, 10.0 * RModel)
			End If

			'pCutter.FromPoint = PointAlongPlane(TerFixPnt, m_OutDir - 90.0, 10.0 * RModel)
			'pCutter.ToPoint = PointAlongPlane(TerFixPnt, m_OutDir + 90.0, 10.0 * RModel)
			' =====================================================================================
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

			If OptionButton1202.Checked Then
				pPrimeAreaPoly = pTopo.Union(pTermFIXTolerArea)
				pTopo = pPrimeAreaPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
			End If

			If (Not OptionButton1201.Checked) And (TurnDirector.TypeCode > eNavaidType.NONE) Then
				pTmpPoly = SecPoly

				If Not pTmpPoly.IsEmpty Then
					ClipByLine(SecPoly, pCutter, pSecndAreaPoly, pTmpPoly, pPolygon)
				End If

				If OptionButton0901.Checked Then
					pCutter.FromPoint = PointAlongPlane(TurnFixPnt, _ArDir - 90.0, 10.0 * RModel)
					pCutter.ToPoint = PointAlongPlane(TurnFixPnt, _ArDir + 90.0, 10.0 * RModel)

					Dim Relation As IRelationalOperator

					If TurnDir < 0 Then
						pTopo = pMAStraightSecLPoly
					Else
						pTopo = pMAStraightSecRPoly
					End If
					Relation = pTopo

					If Relation.Disjoint(pCutter) Then
						pTmpPoly = Relation
					Else
						pTopo.Cut(pCutter, pTmpPoly, pPolygon)
						pTopo = pTmpPoly
						pTopo.IsKnownSimple_2 = False
						pTopo.Simplify()
					End If

					If pSecndAreaPoly Is Nothing Then
						pSecndAreaPoly = pTmpPoly
					Else
						pSecndAreaPoly = pTopo.Union(pSecndAreaPoly)
						pTopo = pSecndAreaPoly
						pTopo.IsKnownSimple_2 = False
						pTopo.Simplify()
					End If
				End If

				If Not (pSecndAreaPoly Is Nothing) Then
					pTopo = pPrimeAreaPoly
					pPrimeAreaPoly = pTopo.Difference(pSecndAreaPoly)

					pTopo = pPrimeAreaPoly
					pTopo.IsKnownSimple_2 = False
					pTopo.Simplify()
				End If
			End If

			'DrawPolygon(BaseArea, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
			'DrawPolygon(SecL, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
			'DrawPolygon(SecR, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
			'DrawPolygon(SecPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
			'DrawPolygon(SecPoly0, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal)
			'Application.DoEvents()

			' Protection Area ================================================
		Else
			' Trajectory =====================================================
			obstacles = MAPtObstacles
			sortIndex = 4

			'Dim fMAPtOCH As Double
			'Dim pTermPt As ESRI.ArcGIS.Geometry.IPoint

			''fMAPtOCH = DeConvertHeight(CDbl(TextBox0802.Text))
			''fTmp = (fTermAlt - fMAPtOCH - fRefHeight) / fMissAprPDG

			'fTmp = DeConvertDistance(CDbl(TextBox0810.Text))

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
			' Protection Area ================================================

			pCutter.FromPoint = PointAlongPlane(pTermPt, _ArDir - 90.0, 10.0 * RModel)
			pCutter.ToPoint = PointAlongPlane(pTermPt, _ArDir + 90.0, 10.0 * RModel)

			'CutPoly(pMAStraightPrimPoly, pCutter, 1)
			'CutPoly(pMAStraightSecPoly, pCutter, 1)

			pTopo = pMAStraightPrimPoly
			pPolygon = pTopo.Difference(pMAStraightSecPoly)
			pTopo = pPolygon
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTopo.Cut(pCutter, pPrimeAreaPoly, pTmpPoly)
			pTopo = pPrimeAreaPoly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			'DrawPolygon(pMAStraightSecPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
			'DrawPolyLine(pCutter, -1, 2)
			'While True
			'	Application.DoEvents()
			'End While

			pTopo = pMAStraightSecPoly
			Try
				pTopo.Cut(pCutter, pSecndAreaPoly, pTmpPoly)
				pTopo = pSecndAreaPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()
			Catch
				pSecndAreaPoly = Nothing
			End Try
		End If

		'DrawPolygon(pPrimeAreaPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'DrawPolygon(pSecndAreaPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'Application.DoEvents()

		pDistance = New ValDistance(fTmp, mUomHDistance)
		pSegmentLeg.Length = pDistance

		pSegmentLeg.Trajectory = pCurve
		'While True
		'	Application.DoEvents()
		'End While

		'DrawPolygon(pPrimeAreaPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pSecndAreaPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		' I Protection Area ================================================
		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPrimeAreaPoly))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		' II Protection Area ================================================
		Dim pSecProtectedArea As ObstacleAssessmentArea = Nothing

		If Not (pSecndAreaPoly Is Nothing) Then
			pSurface = ESRIPolygonToAIXMSurface(ToGeo(pSecndAreaPoly))

			pSecProtectedArea = New ObstacleAssessmentArea
			pSecProtectedArea.Surface = pSurface
			pSecProtectedArea.SectionNumber = 1
			pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

			'pSegmentLeg.DesignSurface.Add(pSecProtectedArea)
		End If

		' Protection Area Obstructions list ==================================================
		'Dim i As Integer
		'Dim j As Integer


		'Added by Agshin
		'Save max 5 obstacles as obstruction

		'
		'Based on choiec sort change beetween  EffectiveHeight and ReqH

		Functions.Sort(obstacles, sortIndex)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0


			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		If Not (pSecProtectedArea Is Nothing) Then pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END of Protection Area =========================================

		' END of MissedApproachTermination ===============================
		Return pApproachLeg
	End Function

	Private Function MissedApproachTerminationContinued(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fDir As Double
		Dim Angle As Double
		Dim fDist As Double
		Dim fCourseDir As Double

		Dim fAltitude As Double
		Dim fDistToNav As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pInterNavSignPt As SignificantPoint

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg

		Dim pAngleUse As AngleUse
		Dim pAngleIndication As AngleIndication
		Dim pStAngleIndication As AngleIndication
		Dim pStDistanceIndication As DistanceIndication

		Dim pSegmentPoint As SegmentPoint
		Dim pPointReference As PointReference
		Dim pFixDesignatedPoint As DesignatedPoint
		Dim pLocation As Aran.Geometries.Point

		Dim GuidNav As NavaidData
		Dim IntersectNav As NavaidData

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical

		Dim pInterceptPt As ESRI.ArcGIS.Geometry.IPoint


		GuidNav = WPT_FIXToNavaid(TurnDirector)
		IntersectNav = TerInterNavDat(ComboBox1502.SelectedIndex)
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
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS
		' Course ===============
		If SideDef(pInterceptPt, pInterceptPt.M + 90.0, GuidNav.pPtPrj) < 0 Then
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		Else
			pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Modulus(CDbl(TextBox1203.Text) + GuidNav.MagVar, 360.0)

		' LowerLimitAltitude =======================================================================================
		pDistanceVertical = New ValDistanceVertical(CDbl(TextBox1510.Text), mUomVDistance)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		' VerticalAngle ====================================================================================================
		pSegmentLeg.VerticalAngle = RadToDeg(System.Math.Atan(fMissAprPDG))
		' SpeedLimit ======

		pSpeed = New ValSpeed(CDbl(TextBox0902.Text), mUomSpeed)
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

		' ========================
		pPointReference = New PointReference()

		fDir = ReturnAngleInDegrees(GuidNav.pPtPrj, TerFixPnt)
		Angle = Modulus(Dir2Azt(GuidNav.pPtPrj, fDir) - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = Dir2Azt(TerFixPnt, fDir)
		pAngleIndication.IndicationDirection = CodeDirectionReference.FROM
		'pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

		pAngleUse = New AngleUse()
		pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		pAngleUse.AlongCourseGuidance = True

		pPointReference.FacilityAngle.Add(pAngleUse)

		pInterNavSignPt = IntersectNav.GetSignificantPoint()

		fCourseDir = Azt2DirPrj(TerFixPnt, pSegmentLeg.Course)

		If IntersectNav.IntersectionType = eIntersectionType.OnNavaid Then
			pFIXSignPt = pInterNavSignPt
			pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		Else
			Dim HorAccuracy As Double = CalcHorisontalAccuracy(TerFixPnt, GuidNav, IntersectNav)

			pFIXSignPt = New SignificantPoint()
			pFixDesignatedPoint = CreateDesignatedPoint(TerFixPnt, "MAHF", fCourseDir)
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			If IntersectNav.TypeCode = eNavaidType.DME Then
				fDistToNav = ReturnDistanceInMeters(IntersectNav.pPtPrj, TerFixPnt)
				fAltitude = TerFixPnt.Z - IntersectNav.pPtPrj.Z + fRefHeight
				fDist = Math.Sqrt(fDistToNav * fDistToNav + fAltitude * fAltitude)
				pStDistanceIndication = CreateDistanceIndication(ConvertDistance(fDist, eRoundMode.NEAREST), mUomHDistance, pInterNavSignPt)
				pStDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

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

				pPointReference.FacilityAngle.Add(pAngleUse)
				pPointReference.Role = CodeReferenceRole.INTERSECTION
			End If
		End If

		PriorPostFixTolerance(pTermFIXTolerArea, TerFixPnt, fCourseDir, PriorFixTolerance, PostFixTolerance)

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
		'Dim pPolyline As ipolyline
		'pPolyline = New polyline
		'pPolyline.FromPoint = pInterceptPt
		'pPolyline.ToPoint = TerFixPnt
		'DrawPolyLine(pPolyline, 255, 2)
		'Application.DoEvents()

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
		' END of MissedApproachTermination =================================================
		Return pApproachLeg
	End Function

	Private Function RacetrackLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic) As ApproachLeg
		Dim UpperLimitAltitude As Double
		Dim LowerLimitAltitude As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		'Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pSegmentLeg As SegmentLeg
		Dim pApproachLeg As ApproachLeg
		Dim pStartPoint As TerminalSegmentPoint

		Dim pSegmentPoint As SegmentPoint

		Dim GuidNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pSpeed As ValSpeed
		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical
		Dim pDuration As ValDuration

		GuidNav = FinalNav
		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)
		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.DURATION

		If ComboBox0601.SelectedIndex = 0 Then
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		Else
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.HOLDING
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.HA

		'	pSegmentLeg.AltitudeOverrideATC =
		'	pSegmentLeg.AltitudeOverrideReference =

		'If SideDef(pFromPoint, m_ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		'Else
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		'End If

		'=======================================================================================
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK

		If SpinButton0201.SelectedItem Is Nothing Then
			pSegmentLeg.Course = CDbl(SpinButton0201.Text)
		Else
			pSegmentLeg.Course = SpinButton0201.SelectedItem
		End If

		'' Indication ======================================================================
		'Dim Angle As Double = Modulus(pSegmentLeg.Course.Value - FinalNav.MagVar)
		'Dim pAngleIndication As AngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)

		'pAngleIndication.TrueAngle = pSegmentLeg.Course
		'pAngleIndication.IndicationDirection = CodeDirectionReference.TO
		'pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
		'' End Of Indication ======================================================================

		pSegmentLeg.BankAngle = arInitApprBank

		'=======================================================================================
		pDuration = New ValDuration
		pDuration.Uom = Aran.Aim.Enums.UomDuration.MIN
		pDuration.Value = CDbl(TextBox0602.Text)
		pSegmentLeg.Duration = pDuration

		'	pSegmentLeg.EndConditionDesignator =
		'	pSegmentLeg.Note
		'	pSegmentLeg.ProcedureTurnRequired =
		'	pSegmentLeg.ReqNavPerformance
		'	pSegmentLeg.SpeedInterpretation =

		'	pSegmentLeg.CourseDirection =
		'	pSegmentLeg.ProcedureTransition
		'	pSegmentLeg.StartPoint
		'=======================================================================================

		LowerLimitAltitude = DeConvertHeight(CDbl(TextBox0605.Text))
		UpperLimitAltitude = DeConvertHeight(CDbl(TextBox0601.Text))

		If ComboBox0501.SelectedIndex = 1 Then
			LowerLimitAltitude = LowerLimitAltitude + fRefHeight
			UpperLimitAltitude = UpperLimitAltitude + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(LowerLimitAltitude, eRoundMode.NEAREST)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(UpperLimitAltitude, eRoundMode.NEAREST)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical
		'===
		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = CDbl(TextBox0613.Text)
		pSegmentLeg.Length = pDistance
		'===
		'    Set pSegmentLeg.verticalAngle =  ( -RadToDeg(Atn(IntermAreaPDG)))

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0603.Text) 'ConvertSpeed(cViafMax.Values(Category))
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		'===

		' Start Point ========================

		pStartPoint = New TerminalSegmentPoint
		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????
		pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF
		pSegmentPoint = pStartPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False

		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False

		'=======================================================================
		pPointReference = New PointReference()
		pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD

		'=======================================================================

		OnNavaidFIXTolerArea(FinalNav, _ArDir, UpperLimitAltitude, ReckoningFIXToleranceArea)

		PriorPostFixTolerance(ReckoningFIXToleranceArea, GuidNav.pPtPrj, _ArDir, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(ReckoningFIXToleranceArea))

		pStartPoint.FacilityMakeup.Add(pPointReference)

		pSegmentPoint.PointChoice = pGuidNavSignPt
		pSegmentLeg.StartPoint = pStartPoint

		'=======================================================================

		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			Dim cooNav As NavaidData = DMEList(FinalNav.PairNavaidIndex)
			'Dim fdAltitude As Double = UpperLimitAltitude - cooNav.pPtPrj.Z
			'Dim fDist As Double = Math.Sqrt(fdLimit * fdLimit + fdAltitude * fdAltitude)

			Dim pDistanceIndication As DistanceIndication

			pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fdLimit, eRoundMode.NEAREST), mUomHDistance, cooNav.GetSignificantPoint())

			Dim pFixDesignatedPoint As DesignatedPoint = DBModule.CreateDesignatedPoint(PtEOL, "COORD")
			pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
			pSegmentLeg.Distance = pDistanceIndication.GetFeatureRef()
		End If

		' End Of Start Point =============================================
		'=================================================================
		' Trajectory =====================================================

		Dim I As Integer
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		pCurve = New Curve

		pPolyline = trHoldingArea
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

		' protected Area =======================================================

		'HoldingArea, BufferedHoldingArea
		''DrawPolygon(HoldingArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		''DrawPolygon(BufferedHoldingArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		''Application.DoEvents()

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(HoldingArea))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pTopo = BufferedHoldingArea
		pPolygon = pTopo.Difference(HoldingArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		Dim AssessedAltitude As Double
		If ComboBox0501.SelectedIndex = 0 Then
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text)) - fRefHeight
		Else
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text))
		End If

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(AssessedAltitude), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = BuffObstacleList
		'Dim i As Integer
		'Dim j As Integer

		'Added by Agshin
		'Save max 5 obstacles as obstruction

		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For I = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0


			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(I).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(I).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(I).MOC

			If obstacles.Parts(I).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================

		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END =====================================================

		Return pApproachLeg
		' END =====================================================
	End Function

	Private Function BaseTurnOutboundLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		'Dim Angle As Double

		Dim fAltitude As Double
		Dim fDirection As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pApproachLeg As ApproachLeg
		Dim pSegmentLeg As SegmentLeg
		Dim pStartPoint As TerminalSegmentPoint
		Dim pSegmentPoint As SegmentPoint

		'Dim pStAngleIndication As AngleIndication
		'Dim pAngleIndication As AngleIndication
		'Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical
		Dim pDuration As ValDuration
		Dim pSpeed As ValSpeed

		'===========================================================

		GuidNav = FinalNav
		pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pApproachLeg

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =

		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL

		If ComboBox0601.SelectedIndex = 0 Then
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		Else
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.BASETURN
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.FC
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.DURATION

		'=======================================================================================
		'If SideDef(pFromPoint, m_ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		'Else
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		'End If
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK

		pSegmentLeg.Course = Modulus(CDbl(TextBox0612.Text) + GuidNav.MagVar, 360.0)
		fDirection = Azt2Dir(GuidNav.pPtGeo, pSegmentLeg.Course)
		pSegmentLeg.BankAngle = arInitApprBank
		'=======================================================================================

		pDuration = New ValDuration
		pDuration.Uom = Aran.Aim.Enums.UomDuration.MIN
		pDuration.Value = CDbl(TextBox0602.Text)
		pSegmentLeg.Duration = pDuration

		If ComboBox0501.SelectedIndex = 0 Then
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text))
		Else
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(fTmp, eRoundMode.NEAREST)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical
		'======

		If ComboBox0501.SelectedIndex = 0 Then
			fAltitude = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			fAltitude = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(fAltitude, eRoundMode.NEAREST)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical
		'===
		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = CDbl(TextBox0613.Text)
		pSegmentLeg.Length = pDistance
		'===
		'    Set pSegmentLeg.verticalAngle =  ( -RadToDeg(Atn(IntermAreaPDG)))

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0603.Text) 'ConvertSpeed(cViafMax.Values(Category))
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		'' Indication ======================================================================
		''    pSignificantPoint.SignificantPointType = SignificantPointType_Navaid
		''    pSignificantPoint.featurePoint.identifier = FinalNav.ID
		''    pSignificantPoint.featurePoint.featureName = FeatureNames(FinalNav.TypeCode)

		'Angle = CDbl(TextBox0612.Text)	 'Modulus(pSegmentLeg.Course - FinalNav.MagVar, 360))

		'pSignificantPoint = New SignificantPoint()
		'pSignificantPoint.NavaidSystem = FinalNav.pFeature.GetFeatureRef()

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pSignificantPoint)
		'pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
		'' End Of Indication ======================================================================

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Start Point ========================
		pStartPoint = New TerminalSegmentPoint()

		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????

		pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF
		'==
		pSegmentPoint = pStartPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False

		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False
		' ========================
		pPointReference = New PointReference()
		'Angle = Modulus(pSegmentLeg.Course - GuidNav.MagVar + 180.0, 360.0)

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		'pAngleIndication.TrueAngle = pSegmentLeg.Course

		'pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

		'pAngleUse = New AngleUse()
		'pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		'pAngleUse.AlongCourseGuidance = True

		'pPointReference.FacilityAngle.Add(pAngleUse)

		'=======================================================================

		pFIXSignPt = pGuidNavSignPt

		'Angle = Modulus(pSegmentLeg.Course + 90.0 - GuidNav.MagVar, 360.0)
		'pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pFIXSignPt)
		'pStAngleIndication.TrueAngle = Modulus(pSegmentLeg.Course + 90.0, 360.0)

		'pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef()

		'pAngleUse = New AngleUse
		'pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
		'pAngleUse.AlongCourseGuidance = False
		'pPointReference.FacilityAngle.Add(pAngleUse)

		pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD

		'=============================================================================================
		'CreateReckoningFIXToleranceArea(fDirection, fAltitude)
		'FinalNav.pPtPrj.Z = FinalNav.pPtGeo.Z
		OnNavaidFIXTolerArea(FinalNav, fDirection, fAltitude, ReckoningFIXToleranceArea)
		PriorPostFixTolerance(ReckoningFIXToleranceArea, GuidNav.pPtPrj, fDirection, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(ReckoningFIXToleranceArea))

		pStartPoint.FacilityMakeup.Add(pPointReference)

		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ========================
		pEndPoint = Nothing

		'=======================================================================

		If CheckBox0603.Enabled And CheckBox0603.Checked Then
			Dim cooNav As NavaidData = DMEList(FinalNav.PairNavaidIndex)

			'Dim fdAltitude As Double = fAltitude - cooNav.pPtPrj.Z
			'Dim fDist As Double = Math.Sqrt(fdLimit * fdLimit + fdAltitude * fdAltitude)
			Dim pDistanceIndication As DistanceIndication

			pDistanceIndication = DBModule.CreateDistanceIndication(Functions.ConvertDistance(fdLimit, eRoundMode.NEAREST), mUomHDistance, cooNav.GetSignificantPoint())

			Dim pFixDesignatedPoint As DesignatedPoint = DBModule.CreateDesignatedPoint(PtEOL, "COORD")
			pDistanceIndication.Fix = pFixDesignatedPoint.GetFeatureRef()
			pSegmentLeg.Distance = pDistanceIndication.GetFeatureRef()
		End If

		' Trajectory =====================================================
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		pLineStringSegment = New LineString
		pClone = trBaseTurn
		pPointCollection = pClone.Clone

		pLocation = ESRIPointToARANPoint(FinalNav.pPtGeo)
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(pPointCollection.Point(1)))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve
		' Trajectory =====================================================
		' protected Area =================================================
		'PrimaryArea BufferedArea
		'BaseTurnArea, BufferedBaseTurnArea
		''DrawPolygon(BaseTurnArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		''DrawPolygon(BufferedBaseTurnArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		''Application.DoEvents()

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(BaseTurnArea))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pTopo = BufferedBaseTurnArea
		pPolygon = pTopo.Difference(BaseTurnArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		Dim AssessedAltitude As Double
		If ComboBox0501.SelectedIndex = 0 Then
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text)) - fRefHeight
		Else
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text))
		End If

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(AssessedAltitude), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = BuffObstacleList
		Dim i As Integer

		'Added by Agshin
		'Save max 5 obstacles as obstruction

		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For i = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0

			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(i).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(i).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(i).MOC

			If obstacles.Parts(i).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================
		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)

		' END =====================================================

		Return pApproachLeg
	End Function

	Private Function BaseTurnInbound(ByRef ptFAFDescent As ESRI.ArcGIS.Geometry.IPoint, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		Dim fDir As Double
		Dim Angle As Double

		Dim pApproachLeg As ApproachLeg
		Dim pSegmentLeg As SegmentLeg
		Dim pSignificantPoint As SignificantPoint

		'Dim pDistance As ValDistance
		Dim pAngleIndication As AngleIndication
		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint
		Dim pDistanceVertical As ValDistanceVertical

		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pAngleUse As AngleUse
		Dim GuidNav As NavaidData
		Dim pSpeed As ValSpeed

		Dim pLocation As Aran.Geometries.Point

		GuidNav = FinalNav

		pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)
		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.AT_LOWER
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE

		If ComboBox0601.SelectedIndex = 0 Then
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		Else
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.BASETURN
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.CF

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =

		'Dim pDuration As IDuration
		'    Set pDuration = New Duration
		'    pDuration.Uom = UomDuration_MIN
		'    pDuration.Value = CDbl(TextBox0602.Text)
		'    pSegmentLeg.Duration = pDuration

		'    pSegmentLeg.EndConditionDesignator =
		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		'        pSegmentLeg.CourseDirection =
		'        pSegmentLeg.ProcedureTransition
		'        pSegmentLeg.StartPoint

		'=======================================================================================
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK

		If SpinButton0201.SelectedItem Is Nothing Then
			pSegmentLeg.Course = CDbl(SpinButton0201.Text)
		Else
			pSegmentLeg.Course = SpinButton0201.SelectedItem
		End If

		pSegmentLeg.BankAngle = arInitApprBank

		'=======================================================================================
		If ComboBox0501.SelectedIndex = 0 Then
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text))
		Else
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance

		pDistanceVertical.Value = ConvertHeight(fTmp, eRoundMode.NEAREST)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'Dim fAltitude As Double
		'
		'    If ComboBox0501.ListIndex = 0 Then
		'        fAltitude = DeConvertHeight(CDbl(TextBox0601.Text))
		'    Else
		'        fAltitude = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		'    End If
		'
		'    Set pDistance = New ValDistanceTypeCom
		'    pDistance.Uom = mVUomDistance
		'    fTmp = ConvertHeight(fAltitude, eRoundMode.rmNERAEST)
		'    pDistance.Value = CStr(fTmp)
		'    Set pSegmentLeg.UpperLimitAltitude = pDistance
		'===
		Dim I As Integer
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPaths As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pTrajectory As ESRI.ArcGIS.Geometry.IPolyline
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pTrajectory = New ESRI.ArcGIS.Geometry.Polyline
		pPolyline = pTrajectory
		pPaths = trBaseTurn

		pClone = pPaths.Geometry(0)
		pPath = pClone.Clone

		For I = 1 To pPaths.GeometryCount - 2
			pPolyline.AddGeometry(pPaths.Geometry(I))
		Next I

		pClone = pPaths.Geometry(pPaths.GeometryCount - 1)
		pPath = pClone.Clone
		pPath.AddPoint(ptFAFDescent)
		pPolyline.AddGeometry(pPath)

		'pPointcollection = pClone.Clone
		'pPointcollection.RemovePoints 0, 1
		'pPointcollection.AddPoint ptFAFDescent

		'pDistance = New ValDistance
		'pDistance.Uom = mHUomDistance
		'pDistance.Value = 0	'ConvertDistance(pTrajectory.Length, eRoundMode.rmNERAEST)
		'pSegmentLeg.Length = pDistance
		'===
		'    Set pSegmentLeg.verticalAngle =  ( -RadToDeg(Atn(IntermAreaPDG)))

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0603.Text) 'ConvertSpeed(cViafMax.Values(Category), eRoundMode.rmNERAEST)
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		'' Indication ======================================================================
		''    pSignificantPoint.SignificantPointType = SignificantPointType_Navaid
		''    pSignificantPoint.featurePoint.identifier = FinalNav.ID
		''    pSignificantPoint.featurePoint.featureName = FeatureNames(FinalNav.TypeCode)

		'Angle = Modulus(pSegmentLeg.Course.Value + 180 - FinalNav.MagVar, 360)

		'pSignificantPoint = New SignificantPoint()
		'pSignificantPoint.NavaidSystem = FinalNav.pFeature.GetFeatureRef()

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pSignificantPoint)
		'pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
		'' End Of Indication ======================================================================

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' End Point ========================

		pEndPoint = New TerminalSegmentPoint
		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????
		pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FPAP
		pSegmentPoint = pEndPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False

		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False
		'=======================================================================
		pPointReference = New PointReference()

		fDir = Azt2DirPrj(ptFAFDescent, pSegmentLeg.Course)
		pFixDesignatedPoint = CreateDesignatedPoint(ptFAFDescent, "FPAP", fDir)

		pSignificantPoint = New SignificantPoint()
		pSignificantPoint.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()
		' ========================

		Angle = Modulus(pSegmentLeg.Course - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = pSegmentLeg.Course
		pAngleIndication.IndicationDirection = CodeDirectionReference.TO
		pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

		pAngleUse = New AngleUse()
		pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		pAngleUse.AlongCourseGuidance = True

		pPointReference.FacilityAngle.Add(pAngleUse)

		pSegmentPoint.PointChoice = pSignificantPoint
		pSegmentLeg.EndPoint = pEndPoint
		' End Of End Point ========================

		' Trajectory =====================================================
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		pCurve = New Curve

		'pPolyline = pTrajectory
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
		Return pApproachLeg
		' END =====================================================
	End Function

	Private Function ProcTurnOutboundLeg(ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		'Dim Angle As Double

		Dim fAltitude As Double
		Dim fDirection As Double
		Dim PostFixTolerance As Double
		Dim PriorFixTolerance As Double

		Dim pFIXSignPt As SignificantPoint
		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pApproachLeg As ApproachLeg
		Dim pSegmentLeg As SegmentLeg
		Dim pStartPoint As TerminalSegmentPoint
		Dim pSegmentPoint As SegmentPoint

		'Dim pStAngleIndication As AngleIndication
		'Dim pAngleIndication As AngleIndication
		'Dim pAngleUse As AngleUse

		Dim GuidNav As NavaidData
		Dim pLocation As Aran.Geometries.Point

		Dim pDistance As ValDistance
		Dim pDistanceSigned As ValDistanceSigned
		Dim pDistanceVertical As ValDistanceVertical
		Dim pDuration As ValDuration
		Dim pSpeed As ValSpeed

		'===========================================================

		GuidNav = FinalNav
		pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)

		pSegmentLeg = pApproachLeg

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =

		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.BETWEEN
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL

		If ComboBox0601.SelectedIndex = 0 Then
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		Else
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.PT
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.FC
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.DURATION

		'=======================================================================================
		'If SideDef(pFromPoint, m_ArDir + 90.0, GuidNav.pPtPrj) < 0 Then
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.FROM
		'Else
		'	pSegmentLeg.CourseDirection = Aran.Aim.Enums.CodeDirectionReference.TO
		'End If

		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Modulus(CDbl(TextBox0612.Text) + FinalNav.MagVar, 360.0)
		fDirection = Azt2Dir(GuidNav.pPtGeo, pSegmentLeg.Course)
		pSegmentLeg.BankAngle = arInitApprBank
		'=======================================================================================

		pDuration = New ValDuration
		pDuration.Uom = Aran.Aim.Enums.UomDuration.MIN
		pDuration.Value = CDbl(TextBox0602.Text)
		pSegmentLeg.Duration = pDuration

		If ComboBox0501.SelectedIndex = 0 Then
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text))
		Else
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(fTmp, eRoundMode.NEAREST)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical
		'======

		If ComboBox0501.SelectedIndex = 0 Then
			fAltitude = DeConvertHeight(CDbl(TextBox0601.Text))
		Else
			fAltitude = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance
		pDistanceVertical.Value = ConvertHeight(fAltitude, eRoundMode.NEAREST)
		pSegmentLeg.UpperLimitAltitude = pDistanceVertical
		'===
		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = CDbl(TextBox0613.Text)
		pSegmentLeg.Length = pDistance
		'===
		'    Set pSegmentLeg.verticalAngle =  ( -RadToDeg(Atn(IntermAreaPDG)))

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0603.Text) 'ConvertSpeed(cViafMax.Values(Category))
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		'' Indication ======================================================================
		''    pSignificantPoint.SignificantPointType = SignificantPointType_Navaid
		''    pSignificantPoint.featurePoint.identifier = FinalNav.ID
		''    pSignificantPoint.featurePoint.featureName = FeatureNames(FinalNav.TypeCode)

		'Angle = CDbl(TextBox0612.Text)

		'pSignificantPoint = New SignificantPoint()
		'pSignificantPoint.NavaidSystem = FinalNav.pFeature.GetFeatureRef()

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pSignificantPoint)
		'pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
		'' End Of Indication ======================================================================

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' Start Point ========================
		pStartPoint = New TerminalSegmentPoint()

		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????

		pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF
		'==
		pSegmentPoint = pStartPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False

		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False
		' ========================
		pPointReference = New PointReference()

		'Angle = Modulus(pSegmentLeg.Course - GuidNav.MagVar + 180.0, 360.0)

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		'pAngleIndication.TrueAngle = pSegmentLeg.Course

		'pAngleIndication.IndicationDirection = CodeDirectionReference.FROM

		'pAngleUse = New AngleUse()
		'pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		'pAngleUse.AlongCourseGuidance = True

		'pPointReference.FacilityAngle.Add(pAngleUse)

		'=======================================================================
		pFIXSignPt = pGuidNavSignPt

		'Angle = Modulus(pSegmentLeg.Course + 90.0 - GuidNav.MagVar, 360.0)
		'pStAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pFIXSignPt)
		'pStAngleIndication.TrueAngle = Modulus(pSegmentLeg.Course + 90.0, 360.0)

		'pStAngleIndication.Fix = pFIXSignPt.GetFeatureRef()

		'pAngleUse = New AngleUse
		'pAngleUse.TheAngleIndication = pStAngleIndication.GetFeatureRef()
		'pAngleUse.AlongCourseGuidance = False

		'pPointReference.FacilityAngle.Add(pAngleUse)
		'=======================================================================
		pPointReference.Role = CodeReferenceRole.OTHER_OVERHEAD
		'=============================================================================================
		'CreateReckoningFIXToleranceArea(fDirection, fAltitude)
		'FinalNav.pPtPrj.Z = FinalNav.pPtGeo.Z
		OnNavaidFIXTolerArea(FinalNav, fDirection, fAltitude, ReckoningFIXToleranceArea)

		PriorPostFixTolerance(ReckoningFIXToleranceArea, GuidNav.pPtPrj, fDirection, PriorFixTolerance, PostFixTolerance)

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PriorFixTolerance, eRoundMode.NEAREST))
		pPointReference.PriorFixTolerance = pDistanceSigned

		pDistanceSigned = New Aran.Aim.DataTypes.ValDistanceSigned()
		pDistanceSigned.Uom = mUomHDistance
		pDistanceSigned.Value = Math.Abs(ConvertDistance(PostFixTolerance, eRoundMode.NEAREST))

		pPointReference.PostFixTolerance = pDistanceSigned
		pPointReference.FixToleranceArea = ESRIPolygonToAIXMSurface(ToGeo(ReckoningFIXToleranceArea))

		pStartPoint.FacilityMakeup.Add(pPointReference)

		pSegmentPoint.PointChoice = pFIXSignPt
		pSegmentLeg.StartPoint = pStartPoint
		' End Of Start Point ========================
		pEndPoint = Nothing
		' Trajectory =====================================================
		Dim I As Integer
		Dim J As Integer
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString
		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		pCurve = New Curve
		If OptionButton0603.Checked Then
			pPolyline = trTurn45_180
		Else
			pPolyline = trTurn80_260
		End If

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
		' protected Area =================================================
		'PrimaryArea BufferedArea
		''DrawPolygon(PrimaryArea, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		''DrawPolygon(BufferedArea, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		''Application.DoEvents()

		Dim pTopo As ITopologicalOperator2
		Dim pPolygon As IPolygon
		Dim pSurface As Surface

		Dim pPrimProtectedArea As ObstacleAssessmentArea
		pSurface = ESRIPolygonToAIXMSurface(ToGeo(PrimaryArea))

		pPrimProtectedArea = New ObstacleAssessmentArea
		pPrimProtectedArea.Surface = pSurface
		pPrimProtectedArea.SectionNumber = 0
		pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY

		Dim pSecProtectedArea As ObstacleAssessmentArea
		pTopo = BufferedArea
		pPolygon = pTopo.Difference(PrimaryArea)
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pSurface = ESRIPolygonToAIXMSurface(ToGeo(pPolygon))

		pSecProtectedArea = New ObstacleAssessmentArea
		pSecProtectedArea.Surface = pSurface
		pSecProtectedArea.SectionNumber = 1
		pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY

		Dim AssessedAltitude As Double
		If ComboBox0501.SelectedIndex = 0 Then
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text)) - fRefHeight
		Else
			AssessedAltitude = DeConvertHeight(CDbl(TextBox0608.Text))
		End If

		pDistanceVertical = New ValDistanceVertical(ConvertHeight(AssessedAltitude), mUomVDistance)
		pPrimProtectedArea.AssessedAltitude = pDistanceVertical
		pSecProtectedArea.AssessedAltitude = pDistanceVertical

		' Protection Area Obstructions list ==================================================
		Dim obstacles As ObstacleContainer = BuffObstacleList

		'Added by Agshin
		'Save max 5 obstacles as obstruction

		Functions.Sort(obstacles, 2)
		Dim saveCount As Int32 = Math.Min(obstacles.Obstacles.Length, 5)

		Dim k As Integer
		For k = 0 To obstacles.Obstacles.Length - 1
			obstacles.Obstacles(k).NIx = -1
		Next

		For I = 0 To obstacles.Parts.Length - 1
			If (saveCount = 0) Then Exit For

			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0


			Dim tmpObstacle As Obstacle = obstacles.Obstacles(obstacles.Parts(I).Owner)
			If (tmpObstacle.NIx > -1) Then Continue For

			obstacles.Obstacles(obstacles.Parts(I).Owner).NIx = 0

			'//MinimumAltitude = Math.Max(MinimumAltitude, obstacles.Parts[obstacles.Obstacles[i].Parts[j]].ReqH)
			RequiredClearance = obstacles.Parts(I).MOC

			If obstacles.Parts(I).Flags Then
				isPrimary = isPrimary Or 1
			Else
				isPrimary = isPrimary Or 2
			End If

			Dim obs As Obstruction = New Obstruction()
			obs.VerticalStructureObstruction = New FeatureRef(tmpObstacle.Identifier)

			'ReqH
			Dim MinimumAltitude As Double = tmpObstacle.Height + RequiredClearance + fRefHeight

			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If (isPrimary And 1) <> 0 Then pPrimProtectedArea.SignificantObstacle.Add(obs)
			If (Not (pSecProtectedArea Is Nothing)) And ((isPrimary And 2) <> 0) Then pSecProtectedArea.SignificantObstacle.Add(obs)

			pDistanceVertical = Nothing
			pDistance = Nothing
			saveCount -= 1
		Next

		' END ======================================================
		pSegmentLeg.DesignSurface.Add(pPrimProtectedArea)
		pSegmentLeg.DesignSurface.Add(pSecProtectedArea)
		' END =====================================================

		Return pApproachLeg
	End Function

	Private Function ProcTurnInbound(ByRef ptFAFDescent As ESRI.ArcGIS.Geometry.IPoint, ByVal pProcedure As InstrumentApproachProcedure, ByVal IsLimitedTo As AircraftCharacteristic, ByRef pEndPoint As TerminalSegmentPoint) As ApproachLeg
		Dim fTmp As Double
		'Dim fDir As Double
		Dim Angle As Double

		Dim pApproachLeg As ApproachLeg
		Dim pSegmentLeg As SegmentLeg
		Dim pSignificantPoint As SignificantPoint

		Dim pDistance As ValDistance
		Dim pAngleIndication As AngleIndication
		Dim pSegmentPoint As SegmentPoint
		Dim pFixDesignatedPoint As DesignatedPoint
		Dim pDistanceVertical As ValDistanceVertical

		Dim pGuidNavSignPt As SignificantPoint
		Dim pPointReference As PointReference

		Dim pAngleUse As AngleUse
		Dim GuidNav As NavaidData

		Dim pSpeed As ValSpeed
		Dim pLocation As Aran.Geometries.Point
		Dim pFromPoint As ESRI.ArcGIS.Geometry.IPoint

		GuidNav = FinalNav

		pApproachLeg = pObjectDir.CreateFeature(Of InitialLeg)()
		'pApproachLeg.Approach = pProcedure.GetFeatureRef()
		pApproachLeg.AircraftCategory.Add(IsLimitedTo)
		pSegmentLeg = pApproachLeg

		pSegmentLeg.AltitudeInterpretation = Aran.Aim.Enums.CodeAltitudeUse.AT_LOWER
		pSegmentLeg.UpperLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.LowerLimitReference = Aran.Aim.Enums.CodeVerticalReference.MSL
		pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.ALTITUDE

		If ComboBox0601.SelectedIndex = 0 Then
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.LEFT
		Else
			pSegmentLeg.TurnDirection = Aran.Aim.Enums.CodeDirectionTurn.RIGHT
		End If

		pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.PT
		pSegmentLeg.LegTypeARINC = Aran.Aim.Enums.CodeSegmentPath.PI

		'    pSegmentLeg.AltitudeOverrideATC =
		'    pSegmentLeg.AltitudeOverrideReference =

		'Dim pDuration As IDuration
		'    Set pDuration = New Duration
		'    pDuration.Uom = UomDuration_MIN
		'    pDuration.Value = CDbl(TextBox0602.Text)
		'    pSegmentLeg.Duration = pDuration

		'    pSegmentLeg.EndConditionDesignator =
		'    pSegmentLeg.Note
		'    pSegmentLeg.ProcedureTurnRequired =
		'    pSegmentLeg.ReqNavPerformance
		'    pSegmentLeg.SpeedInterpretation =

		'        pSegmentLeg.CourseDirection =
		'        pSegmentLeg.ProcedureTransition
		'        pSegmentLeg.StartPoint

		'=======================================================================================
		pSegmentLeg.CourseType = Aran.Aim.Enums.CodeCourse.TRUE_TRACK
		pSegmentLeg.Course = Modulus(CDbl(TextBox0612.Text) + 45.0 * (2.0 * ComboBox0601.SelectedIndex - 1.0) + FinalNav.MagVar, 360.0)
		pSegmentLeg.BankAngle = arInitApprBank
		'=======================================================================================
		If ComboBox0501.SelectedIndex = 0 Then
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text))
		Else
			fTmp = DeConvertHeight(CDbl(TextBox0605.Text)) + fRefHeight
		End If

		pDistanceVertical = New ValDistanceVertical
		pDistanceVertical.Uom = mUomVDistance

		pDistanceVertical.Value = ConvertHeight(fTmp, eRoundMode.NEAREST)
		pSegmentLeg.LowerLimitAltitude = pDistanceVertical

		'Dim fAltitude As Double
		'
		'    If ComboBox0501.ListIndex = 0 Then
		'        fAltitude = DeConvertHeight(CDbl(TextBox0601.Text))
		'    Else
		'        fAltitude = DeConvertHeight(CDbl(TextBox0601.Text)) + fRefHeight
		'    End If
		'
		'    Set pDistance = New ValDistanceTypeCom
		'    pDistance.Uom = mVUomDistance
		'    fTmp = ConvertHeight(fAltitude, eRoundMode.rmNERAEST)
		'    pDistance.Value = CStr(fTmp)
		'    Set pSegmentLeg.UpperLimitAltitude = pDistance
		'===

		If OptionButton0603.Checked Then
			pFromPoint = trTurn45_180.ToPoint '.Point(trTurn45_180.PointCount - 1)
		Else
			pFromPoint = trTurn80_260.ToPoint '.Point(trTurn80_260.PointCount - 1)
		End If

		fTmp = ReturnDistanceInMeters(pFromPoint, ptFAFDescent)

		pDistance = New ValDistance
		pDistance.Uom = mUomHDistance
		pDistance.Value = 0.0 'ConvertDistance(fTmp, eRoundMode.rmNERAEST)
		pSegmentLeg.Length = pDistance
		'===
		'    Set pSegmentLeg.verticalAngle =  ( -RadToDeg(Atn(IntermAreaPDG)))

		pSpeed = New ValSpeed
		pSpeed.Uom = mUomSpeed
		pSpeed.Value = CDbl(TextBox0603.Text)
		pSegmentLeg.SpeedLimit = pSpeed
		pSegmentLeg.SpeedReference = Aran.Aim.Enums.CodeSpeedReference.IAS

		'' Indication ======================================================================
		''    pSignificantPoint.SignificantPointType = SignificantPointType_Navaid
		''    pSignificantPoint.featurePoint.identifier = FinalNav.ID
		''   pSignificantPoint.featurePoint.featureName = FeatureNames(FinalNav.TypeCode)

		'Angle = Modulus(pSegmentLeg.Course.Value + 180 - FinalNav.MagVar, 360)

		'pSignificantPoint = New SignificantPoint()
		'pSignificantPoint.NavaidSystem = FinalNav.pFeature.GetFeatureRef()

		'pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pSignificantPoint)
		'pSegmentLeg.Angle = pAngleIndication.GetFeatureRef()
		'' End Of Indication =============================================

		pGuidNavSignPt = GuidNav.GetSignificantPoint()

		' End Point ======================================================

		pEndPoint = New TerminalSegmentPoint()
		'        pTerminalSegmentPoint.IndicatorFACF =      ??????????
		'        pTerminalSegmentPoint.LeadDME =            ??????????
		'        pTerminalSegmentPoint.LeadRadial =         ??????????
		pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.FPAP
		pSegmentPoint = pEndPoint
		'pSegmentPoint.FlyOver = False
		pSegmentPoint.RadarGuidance = False

		pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
		pSegmentPoint.Waypoint = False
		'=================================================================
		pPointReference = New PointReference()
		pFixDesignatedPoint = CreateDesignatedPoint(ptFAFDescent, "FPAP", Azt2DirPrj(ptFAFDescent, pSegmentLeg.Course))
		pSignificantPoint = New SignificantPoint()
		pSignificantPoint.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

		Angle = Modulus(pSegmentLeg.Course - GuidNav.MagVar, 360.0)

		pAngleIndication = CreateAngleIndication(Angle, Aran.Aim.Enums.CodeBearing.MAG, pGuidNavSignPt)
		pAngleIndication.TrueAngle = pSegmentLeg.Course
		pAngleIndication.IndicationDirection = CodeDirectionReference.TO
		pAngleIndication.Fix = pFixDesignatedPoint.GetFeatureRef()

		pAngleUse = New AngleUse()
		pAngleUse.TheAngleIndication = pAngleIndication.GetFeatureRef()
		pAngleUse.AlongCourseGuidance = True

		pPointReference.FacilityAngle.Add(pAngleUse)

		' ================================================================

		pSegmentPoint.PointChoice = pSignificantPoint
		pSegmentLeg.EndPoint = pEndPoint
		' End Of End Point ===============================================

		' Trajectory =====================================================
		Dim pCurve As Curve
		Dim pLineStringSegment As LineString

		pLineStringSegment = New LineString

		pLocation = ESRIPointToARANPoint(ToGeo(pFromPoint))
		pLineStringSegment.Add(pLocation)

		pLocation = ESRIPointToARANPoint(ToGeo(ptFAFDescent))
		pLineStringSegment.Add(pLocation)

		pCurve = New Curve
		pCurve.Geo.Add(pLineStringSegment)

		pSegmentLeg.Trajectory = pCurve
		' Trajectory =====================================================

		Return pApproachLeg
		' END ============================================================
	End Function

	Private Function SaveProcedure(sProcName As String) As Boolean
		Dim HaveTP As Boolean
		Dim HaveFAF As Boolean
		Dim HaveSDF As Boolean
		Dim HaveIntercept As Boolean

		Dim i As Integer
		Dim n As Integer
		Dim NO_SEQ As Integer
		Dim Continued As Integer

		'Dim sProcName As String
		Dim ptFAFDescent As ESRI.ArcGIS.Geometry.IPoint

		Dim pTransition As ProcedureTransition
		Dim IsLimitedTo As AircraftCharacteristic

		Dim featureRef As FeatureRef
		Dim featureRefObject As FeatureRefObject
		Dim pGuidanceServiceChose As New GuidanceService
		Dim pLandingTakeoffAreaCollection As LandingTakeoffAreaCollection
		Dim pEndPoint As TerminalSegmentPoint = Nothing

		Dim uomDistHorzTab() As UomDistance
		Dim uomDistVerTab() As UomDistanceVertical
		Dim uomSpeedTab() As UomSpeed

		uomDistHorzTab = New UomDistance() {UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI}
		uomDistVerTab = New UomDistanceVertical() {UomDistanceVertical.M, UomDistanceVertical.FT} ', UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER, UOMDistanceVertical.OTHER
		uomSpeedTab = New UomSpeed() {UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC}

		mUomHDistance = uomDistHorzTab(DistanceUnit)
		mUomVDistance = uomDistVerTab(HeightUnit)
		mUomSpeed = uomSpeedTab(SpeedUnit)

		'pObjectDir.ClearAllFeatures()

		HaveFAF = OptionButton0201.Checked
		HaveSDF = CheckBox0701.Checked
		HaveTP = CheckBox0801.Checked
		HaveIntercept = OptionButton1202.Checked

		' Procedure =================================================================================================
		pProcedure = DBModule.pObjectDir.CreateFeature(Of InstrumentApproachProcedure)()

		pLandingTakeoffAreaCollection = New LandingTakeoffAreaCollection

		If OptionButton0102.Checked Then
			'sProcName = _Label0101_5.Text + "_CAT_" + ComboBox0001.Text

			n = UBound(RWYDATA)

			For i = 0 To n
				If RWYDATA(i).Selected Then pLandingTakeoffAreaCollection.Runway.Add(RWYDATA(i).GetFeatureRefObject())
			Next i
		Else
			'sProcName = _Label0101_5.Text + "_RWY" + ComboBox0101.Text + "_CAT_" + ComboBox0001.Text
			pLandingTakeoffAreaCollection.Runway.Add(SelectedRWY.GetFeatureRefObject())
		End If

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
		'pProcedure.ProtectsSafeAltitudeAreaId = 

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

		pGuidanceServiceChose.Navaid = FinalNav.GetFeatureRef()

		pProcedure.GuidanceFacility.Add(pGuidanceServiceChose)

		gAranEnv.GetLogger("Non precision Arrival").Info("Procedure Transition Create")
		'Transition ==========================================================================
		pTransition = New ProcedureTransition

		'	pTransition.Description =
		'	pTransition.ID =
		'	pTransition.Procedure =
		'pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection
		pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.FINAL

		'Legs ======================================================================================================
		'    Set pSegmentLegList = New SegmentLegComList
		NO_SEQ = 0

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 1 Intermediate Approach")
		'Leg 1 Intermediate Approach ===============================================================================
		Dim pSegmentLeg As SegmentLeg
		Dim ptl As ProcedureTransitionLeg

		If HaveFAF Then
			NO_SEQ = NO_SEQ + 1
			pSegmentLeg = IntermediateApproachLeg(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
			ptFAFDescent = PtFAF
		Else
			'Dim fRDH As Double = GetRDH()
			'Dim fFAFDescentDist As Double = (FarFAF.Z - fRefHeight - fRDH) / arFADescent_Nom.Value

			'fTmp = ReturnDistanceInMeters(FictTHR, FarFAF)
			'If fTmp > fFAFDescentDist Then
			'	ptFAFDescent = PointAlongPlane(FictTHR, m_ArDir + 180.0, fFAFDescentDist)
			'	ptFAFDescent.Z = FarFAF.Z
			'Else
			'	ptFAFDescent = FarFAF
			'End If

			Dim pFixDesignatedPoint As DesignatedPoint
			pFixDesignatedPoint = CreateDesignatedPoint(FarFAF, , _ArDir)
			pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.CNF

			Dim pFIXSignPt As SignificantPoint
			pFIXSignPt = New SignificantPoint()
			pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef()

			' ========================
			pEndPoint = New TerminalSegmentPoint()
			If ComboBox0604.SelectedIndex = 1 Then
				pEndPoint.Role = CodeProcedureFixRole.IF
			Else
				pEndPoint.Role = CodeProcedureFixRole.FAF
			End If
			' ========================
			Dim pSegmentPoint As SegmentPoint
			pSegmentPoint = pEndPoint
				'pSegmentPoint.FlyOver = False
				pSegmentPoint.RadarGuidance = False
				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT
				pSegmentPoint.Waypoint = False
				pSegmentPoint.PointChoice = pFIXSignPt

				ptFAFDescent = FarFAF
			End If

			gAranEnv.GetLogger("Non precision Arrival").Info("Leg 2 Final Approach To SDF")
		'Leg 2 Final Approach To SDF ======================================================================================
		If HaveSDF Then
			NO_SEQ = NO_SEQ + 1
			pSegmentLeg = FinalApproachToSDFLeg(ptFAFDescent, pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 3 Final Approach")
		'Leg 3 Final Approach ============================================================================
		NO_SEQ = NO_SEQ + 1
		pSegmentLeg = FinalApproachLeg(ptFAFDescent, Continued, pProcedure, IsLimitedTo, pEndPoint)

		ptl = New ProcedureTransitionLeg()
		ptl.SeqNumberARINC = NO_SEQ
		ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
		pTransition.TransitionLeg.Add(ptl)

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 4 Straight Missed Approach")
		'Leg 4 Straight Missed Approach ============================================================================

		If Continued = 2 Then
			NO_SEQ = NO_SEQ + 1                     'Continued,
			pSegmentLeg = FinalApproachContinuedLeg(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 5 Straight Missed Approach")
		'Leg 5 Straight Missed Approach ============================================================================
		If HaveTP Then
			NO_SEQ = NO_SEQ + 1                 'Continued,
			pSegmentLeg = MissedApproachStraight(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 6 Missed Approach Termination")
		'Leg 6 Missed Approach Termination ==========================================================================
		NO_SEQ = NO_SEQ + 1                     'Continued, 
		pSegmentLeg = MissedApproachTermination(pProcedure, IsLimitedTo, pEndPoint)

		ptl = New ProcedureTransitionLeg()
		ptl.SeqNumberARINC = NO_SEQ
		ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
		pTransition.TransitionLeg.Add(ptl)

		gAranEnv.GetLogger("Non precision Arrival").Info("Leg 7 Missed Approach Termination Continued")
		'Leg 7 Missed Approach Termination Continued =================================================================

		If HaveIntercept Then
			NO_SEQ = NO_SEQ + 1
			pSegmentLeg = MissedApproachTerminationContinued(pProcedure, IsLimitedTo, pEndPoint)

			ptl = New ProcedureTransitionLeg()
			ptl.SeqNumberARINC = NO_SEQ
			ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
			pTransition.TransitionLeg.Add(ptl)
		End If

		'=============================================================================================================

		If CheckBox1501.Checked Then
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

				pSegmentLeg = MissedApproachLegs(currSegment, IsLimitedTo, pEndPoint)

				ptl = New ProcedureTransitionLeg()
				NO_SEQ += 1
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)

				i += 1
			Loop

		End If

		pProcedure.FlightTransition.Add(pTransition)

		gAranEnv.GetLogger("Non precision Arrival").Info("Transition II")
		'Transition II ==========================================================================
		If Not HaveFAF Then
			pTransition = New ProcedureTransition

			'    pTransition.Description =
			'    pTransition.ID =
			'    pTransition.TransitionId = TextBox0???.Text

			'Legs ======================================================================================================
			'pSegmentLegList = New List(Of SegmentLeg)
			NO_SEQ = 0

			gAranEnv.GetLogger("Non precision Arrival").Info("Leg 1 Intermediate Approach")
			'Leg 1 Intermediate Approach ===============================================================================
			If OptionButton0601.Checked Then
				NO_SEQ = NO_SEQ + 1
				pSegmentLeg = RacetrackLeg(pProcedure, IsLimitedTo)

				ptl = New ProcedureTransitionLeg()
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)
			ElseIf OptionButton0602.Checked Then
				NO_SEQ = NO_SEQ + 1
				pSegmentLeg = BaseTurnOutboundLeg(pProcedure, IsLimitedTo, pEndPoint)

				ptl = New ProcedureTransitionLeg()
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)
				'===

				NO_SEQ = NO_SEQ + 1
				pSegmentLeg = BaseTurnInbound(ptFAFDescent, pProcedure, IsLimitedTo, pEndPoint)
				'fTmp = DirectCast(pSegmentLeg, Aran.Aim.Features.InitialLeg).Length.Value

				ptl = New ProcedureTransitionLeg()
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)
			Else
				NO_SEQ = NO_SEQ + 1
				pSegmentLeg = ProcTurnOutboundLeg(pProcedure, IsLimitedTo, pEndPoint)

				ptl = New ProcedureTransitionLeg()
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)
				'===
				NO_SEQ = NO_SEQ + 1
				pSegmentLeg = ProcTurnInbound(ptFAFDescent, pProcedure, IsLimitedTo, pEndPoint)

				ptl = New ProcedureTransitionLeg()
				ptl.SeqNumberARINC = NO_SEQ
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef(Of AbstractSegmentLegRef)()
				pTransition.TransitionLeg.Add(ptl)
			End If

			'Transition II ==========================================================================

			'        For I = 0 To pSegmentLegList.Count - 1
			'            Set pFeatureCodeIdentifier = New FeatureCodeIdentifierCom
			'            Set pFeatureCodeIdentifier.Feature = pSegmentLegList.Item(I)
			'            pTransition.transitionLeg.Add pFeatureCodeIdentifier
			'        Next I

			'    pTransition.Procedure =
			'pLandingTakeoffAreaCollection = New LandingTakeoffAreaCollection

			'If OptionButton0101_1.Checked Then
			'	N = UBound(RWYDATA)

			'	For I = 0 To N
			'		If RWYDATA(I).Selected Then
			'			pLandingTakeoffAreaCollection.Runway.Add(RWYDATA(I).pRunwayDir.GetFeatureRefObject())
			'		End If
			'	Next I
			'Else
			'	pLandingTakeoffAreaCollection.Runway.Add(SelectedRWY.pRunwayDir.GetFeatureRefObject())
			'End If

			'pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection

			pTransition.Type = Aran.Aim.Enums.CodeProcedurePhase.APPROACH
			pProcedure.FlightTransition.Add(pTransition)
		End If

		'Circling Area =================================================================================================
		If OptionButton0102.Checked Then
			gAranEnv.GetLogger("Non precision Arrival").Info("Circling Area")

			Dim pCirclingArea As CirclingArea = pObjectDir.CreateFeature(Of CirclingArea)()
			Dim pCondition As ApproachCondition = New ApproachCondition()
			Dim pSurface As Surface = ESRIPolygonToAIXMSurface(ToGeo(VsArea))

			' Assessment Area Obstructions list ==================================================
			Dim pAssessmentArea As ObstacleAssessmentArea = New ObstacleAssessmentArea()

			pAssessmentArea.Surface = pSurface
			pAssessmentArea.SectionNumber = 0
			pAssessmentArea.Type = CodeObstacleAssessmentSurface.FINAL
			'Dim pDistanceVertical As ValDistanceVertical = New ValDistanceVertical(ConvertHeight(fNewVisAprOCH + fRefHeight), mUomVDistance)
			pAssessmentArea.AssessedAltitude = New ValDistanceVertical(ConvertHeight(fNewVisAprOCH + fRefHeight), mUomVDistance)

			Dim obstacles As ObstacleMSA() = MSAObstacleList

			Functions.SortMSAByReqH(obstacles, LBound(obstacles), UBound(obstacles))
			Dim saveCount As Int32 = Math.Min(obstacles.Length, 5)

			For i = 0 To saveCount - 1
				Dim RequiredClearance As Double = obstacles(i).ReqH - obstacles(i).Height

				Dim obs As Obstruction = New Obstruction()
				obs.VerticalStructureObstruction = New FeatureRef(obstacles(i).Identifier)

				'ReqH
				Dim MinimumAltitude As Double = obstacles(i).ReqH + fRefHeight

				Dim pDistanceVertical As ValDistanceVertical = New ValDistanceVertical()
				pDistanceVertical.Uom = mUomVDistance
				pDistanceVertical.Value = Functions.ConvertHeight(MinimumAltitude)
				obs.MinimumAltitude = pDistanceVertical

				'MOC
				Dim pDistance As ValDistance = New ValDistance()
				pDistance.Uom = UomDistance.M
				pDistance.Value = RequiredClearance
				obs.RequiredClearance = pDistance

				pAssessmentArea.SignificantObstacle.Add(obs)
				pDistanceVertical = Nothing
				pDistance = Nothing
			Next
			pCondition.DesignSurface.Add(pAssessmentArea)
			' Assessment Area Obstructions list ==================================================
			' CirclingRestriction ================================================================
			Dim pCirclingRestriction As New CirclingRestriction

			Dim pTmpPoly As IPolygon
			Dim pRestrictionArea As IPolygon = New ESRI.ArcGIS.Geometry.Polygon

			Dim pTopo As ITopologicalOperator

			For i = 0 To pSubtrahend.Count - 1
				Dim bContains As Boolean
				Try
					bContains = ListView0202.CheckedIndices.Contains(i)
				Catch ex As Exception
					bContains = False
				End Try

				If Not bContains Then Continue For
				pTopo = ConvexPoly
				pTmpPoly = pTopo.Intersect(pSubtrahend(i), esriGeometryDimension.esriGeometry2Dimension)
				pTopo = pRestrictionArea
				pRestrictionArea = pTopo.Union(pTmpPoly)
			Next

			pCirclingRestriction.RestrictionArea = ESRIPolygonToAIXMSurface(ToGeo(pRestrictionArea))
			pCondition.CirclingRestriction.Add(pCirclingRestriction)
			' CirclingRestriction ================================================================

			pCondition.AircraftCategory.Add(IsLimitedTo)

			pCirclingArea.Extent = pSurface
			pCirclingArea.Condition.Add(pCondition)
			pCirclingArea.AircraftCategory = IsLimitedTo
			pCirclingArea.Approach = pProcedure.GetFeatureRef

			'pProcedure.ApproachType = CodeApproach.ARA
		End If

		'IAPProfile ====================================================================================================
		gAranEnv.GetLogger("Non precision Arrival").Info("IAPProfile")
		Dim pIAPProfile As FinalProfile
		Dim pApproachAltitudeTable As ApproachAltitudeTable
		Dim pApproachDistanceTable As ApproachDistanceTable

		Dim distance As Double
		Dim tmpY As Double

		pIAPProfile = New FinalProfile
		'========================================================================

		If HaveFAF Then
			PrjToLocal(FictTHR, _ArDir + 180.0, PtFAF, distance, tmpY)
			'distance = Point2LineDistancePrj(PtFAF, FictTHR, _ArDir + 90.0)
			pApproachDistanceTable = New ApproachDistanceTable
			pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.FAF
			pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.THLD
			pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
			pIAPProfile.Distance.Add(pApproachDistanceTable)

			pApproachAltitudeTable = New ApproachAltitudeTable
			pApproachAltitudeTable.AltitudeReference = CodeVerticalReference.MSL
			pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.FAF
			pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(PtFAF.Z + fRefHeight, eRoundMode.NEAREST))
			pIAPProfile.Altitude.Add(pApproachAltitudeTable)
		Else
			PrjToLocal(FictTHR, _ArDir + 180.0, FarFAF, distance, tmpY)
			'distance = Point2LineDistancePrj(FarFAF, FictTHR, _ArDir + 90.0)
			pApproachDistanceTable = New ApproachDistanceTable
			pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.FAF
			pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.THLD
			pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
			pIAPProfile.Distance.Add(pApproachDistanceTable)

			pApproachAltitudeTable = New ApproachAltitudeTable
			pApproachAltitudeTable.AltitudeReference = CodeVerticalReference.MSL
			pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.FAF
			pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(FarFAF.Z, eRoundMode.NEAREST))
			pIAPProfile.Altitude.Add(pApproachAltitudeTable)
		End If

		If HaveSDF Then
			PrjToLocal(FictTHR, _ArDir + 180.0, PtSDF, distance, tmpY)
			'distance = Point2LineDistancePrj(PtSDF, FictTHR, _ArDir + 90.0)
			pApproachDistanceTable = New ApproachDistanceTable
			pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF
			pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.THLD
			pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
			pIAPProfile.Distance.Add(pApproachDistanceTable)

			pApproachAltitudeTable = New ApproachAltitudeTable
			pApproachAltitudeTable.AltitudeReference = CodeVerticalReference.MSL
			pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.OTHER_SDF
			pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(PtSDF.Z + fRefHeight, eRoundMode.NEAREST))
			pIAPProfile.Altitude.Add(pApproachAltitudeTable)
		End If

		PrjToLocal(FictTHR, _ArDir + 180.0, pMAPt, distance, tmpY)

		If distance > 0.0 Then
			pApproachAltitudeTable = New ApproachAltitudeTable
			pApproachAltitudeTable.AltitudeReference = CodeVerticalReference.MSL
			pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.THLD
			pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(arAbv_Treshold.Value + FictTHR.Z, eRoundMode.NEAREST))
			pIAPProfile.Altitude.Add(pApproachAltitudeTable)
		End If

		pApproachDistanceTable = New ApproachDistanceTable
		pApproachDistanceTable.StartingMeasurementPoint = CodeProcedureDistance.THLD
		pApproachDistanceTable.EndingMeasurementPoint = CodeProcedureDistance.MAP
		pApproachDistanceTable.Distance = CreateValDistanceType(mUomHDistance, ConvertDistance(distance, eRoundMode.NEAREST))
		pIAPProfile.Distance.Add(pApproachDistanceTable)

		pApproachAltitudeTable = New ApproachAltitudeTable
		pApproachAltitudeTable.AltitudeReference = CodeVerticalReference.MSL
		pApproachAltitudeTable.MeasurementPoint = CodeProcedureDistance.MAP
		pApproachAltitudeTable.Altitude = CreateValAltitudeType(mUomVDistance, ConvertHeight(pMAPt.Z, eRoundMode.NEAREST))
		pIAPProfile.Altitude.Add(pApproachAltitudeTable)

		pProcedure.FinalProfile = pIAPProfile

		'======================================================================

		'======================================================================================
		'Dim pObstacleAssessmentArea As ObstacleAssessmentArea
		'Dim pObstacleAssessmentAreaList As List(Of ObstacleAssessmentArea)
		'pObstacleAssessmentAreaList = New List(Of ObstacleAssessmentArea)

		'pObstacleAssessmentArea = New ObstacleAssessmentArea
		'pObstacleAssessmentArea.AircraftCategory.Add(IsLimitedTo)
		'pObstacleAssessmentAreaList.Add(pObstacleAssessmentArea)
		Dim result As Boolean = False
		'======================================================================
		Try
			Try
				pObjectDir.AddCreatedRefToSrcLocalStorage()
			Catch exc As Exception
				gAranEnv.GetLogger("Non precision Arrival").Error(exc, "Add Created Ref To Src Local Storage")
				MsgBox("Error on find ref features." + vbCrLf + exc.Message)
				Return False
			End Try

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
				FeatureType.InstrumentApproachProcedure,
				FeatureType.CirclingArea}

			Dim metadataFeatures As List(Of FeatureType) = New List(Of FeatureType) From {FeatureType.InstrumentApproachProcedure}

			gAranEnv.GetLogger("Non precision Arrival").Info("Commit start")
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

			If result Then
				pObjectDir.SaveSrcLocalStorage("")
				gAranEnv.RefreshAllAimLayers()
			End If

			gAranEnv.GetLogger("Non precision Arrival").Info("Non precision Arrival Procedure Save/Commit End")
		Catch ex As Exception
			gAranEnv.GetLogger("Non precision Arrival").Error(ex, "Save procedure")
			MsgBox("Error on commit." + vbCrLf + ex.Message)
			result = False
		End Try

		If (Not result) Then
			pObjectDir.ClearAllFeatures()
		End If

		Return result
	End Function

	Private Function GetNumericalData() As List(Of GeoNumericalDataModel)
		Dim NumericalData As New List(Of GeoNumericalDataModel)

		Dim HaveFAF As Boolean = OptionButton0201.Checked
		Dim HaveSDF As Boolean = CheckBox0701.Checked
		Dim HaveTP As Boolean = CheckBox0801.Checked
		Dim HaveIntercept As Boolean = OptionButton1202.Checked

		Dim GuidanceNav As NavaidData = FinalNav
		Dim IntersectNav As NavaidData

		'Leg 1 Initial to Final Approach =================================================================================
		If HaveFAF Then
			IntersectNav = IFNavDat(ComboBox0401.SelectedIndex)
			Dim IfHorAccuracy As Double = CalcHorisontalAccuracy(IFPnt, GuidanceNav, IntersectNav)

			NumericalData.Add(New GeoNumericalDataModel With
			{
				.Role = "IF",
				.Accuracy = IfHorAccuracy,
				.Resolution = 0.0,
				.DesignatorDescription = GetDesignatedPointDescription(IFPnt),
				.LegType = "Intermediate"
			})

			IntersectNav = FAFNavDat(ComboBox0302.SelectedIndex)
			Dim FafHorAccuracy As Double = CalcHorisontalAccuracy(PtFAF, GuidanceNav, IntersectNav)
			NumericalData.Add(New GeoNumericalDataModel With
			{
				.Role = "FAF",
				.Accuracy = FafHorAccuracy,
				.Resolution = 0.0,
				.DesignatorDescription = GetDesignatedPointDescription(PtFAF),
				.LegType = "Intermediate"
			})
		End If

		'Leg 2 Final Approach To SDF ======================================================================================
		If HaveSDF Then
			IntersectNav = SDFNavDat(ComboBox0702.SelectedIndex)
			Dim SdfHorAccuracy As Double = CalcHorisontalAccuracy(PtSDF, GuidanceNav, IntersectNav)
			NumericalData.Add(New GeoNumericalDataModel With
			{
				.Role = "SDF",
				.Accuracy = SdfHorAccuracy,
				.Resolution = 0.0,
				.DesignatorDescription = GetDesignatedPointDescription(PtSDF),
				.LegType = IIf((Not HaveFAF) And (ComboBox0604.SelectedIndex = 1), "Intermediate", "Final")
			})
		End If

		'Leg 3-4 Final Approach / Straight Missed Approach ================================================================
		If OptionButton0801.Checked Then
			IntersectNav = MAPtNavDat(ComboBox0801.SelectedIndex)
			Dim MaptHorAccuracy As Double = CalcHorisontalAccuracy(pMAPt, GuidanceNav, IntersectNav)
			NumericalData.Add(New GeoNumericalDataModel With
			{
				.Role = "MAPT",
				.Accuracy = MaptHorAccuracy,
				.Resolution = 0.0,
				.DesignatorDescription = GetDesignatedPointDescription(pMAPt),
				.LegType = "Final"
			})
		End If

		If HaveTP Then
			'Leg 5 Straight Missed Approach ============================================================================
			If OptionButton0902.Checked Then
				IntersectNav = TurnInterNavDat(ComboBox1102.SelectedIndex)
				Dim MatfHorAccuracy As Double = CalcHorisontalAccuracy(TurnFixPnt, GuidanceNav, IntersectNav)
				NumericalData.Add(New GeoNumericalDataModel With
				{
					.Role = "MAHF",
					.Accuracy = MatfHorAccuracy,
					.Resolution = 0.0,
					.DesignatorDescription = GetDesignatedPointDescription(TurnFixPnt),
					.LegType = "MissedApproach"
				})
			End If

			'Leg 6-7 Missed Approach Termination ==========================================================================

			If HaveIntercept Then
				GuidanceNav = WPT_FIXToNavaid(TurnDirector)
				IntersectNav = TerInterNavDat(ComboBox1502.SelectedIndex)
				Dim MatfMahfHorAccuracy As Double = CalcHorisontalAccuracy(TerFixPnt, GuidanceNav, IntersectNav)
				NumericalData.Add(New GeoNumericalDataModel With
				{
					.Role = IIf(CheckBox1501.Checked, "MATF", "MAHF"),
					.Accuracy = MatfMahfHorAccuracy,
					.Resolution = 0.0,
					.DesignatorDescription = GetDesignatedPointDescription(TerFixPnt),
					.LegType = "MissedApproach"
				})
			End If
			'=============================================================================================================

			If CheckBox1501.Checked Then
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
					GuidanceNav = currSegment.GuidanceNav
					Dim MatfMahfHorAccuracy As Double = CalcHorisontalAccuracy(currSegment.ptOut, GuidanceNav, IntersectNav)
					NumericalData.Add(New GeoNumericalDataModel With
					{
						.Role = IIf(i < TSC, "MATF", "MAHF"),
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

End Class
