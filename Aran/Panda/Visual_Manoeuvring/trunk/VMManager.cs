using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.Common;
using Aran.Aim.Features;
using GeoAPI.Geometries;
using System.ComponentModel;

namespace Aran.Panda.VisualManoeuvring
{
    public class VMManager
    {
        static VMManager _instance;

        public static VMManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VMManager();
                    _instance.getAllObstacles();
                }
                return _instance;
            }
        }

        private VMManager()
        {
            GeomOper = new JtsGeometryOperators();
            _maxModelRadius = 30000;
            DBModule.GetObstaclesByDist(out AllObstaclesAIXM, GlobalVars.CurrADHP.pPtPrj, _maxModelRadius);
            //DBModule.GetVisualFeaturesByDist(out AllVisualFeatures, GlobalVars.CurrADHP.pPtPrj, _maxModelRadius);
            IAPList = new BindingList<InstrumentApproachProcedure>();
            CirclingAreaPolyElement = -1;
            TrackSegmentsList = new System.ComponentModel.BindingList<VM_TrackSegment>();
            TrackSegmentObstacles = new List<List<VM_VerticalStructure>>();
            TrackSegmentVisualFeatures = new List<List<VM_VisualFeature>>();
            StepCentrelinElements = new List<int>();
            StepBufferPolyElements = new List<int>();
            StepVisibilityPolyElements = new List<int>();
            DivergenceLineElements = new List<int>();
            ConvergenceLineElements = new List<int>();
            StepMainPointsElements = new List<List<int>>();
            StepsTurnStartEndCrossLines = new List<List<int>>();
            StepTurnStartEndCrossLines = new List<int>();
            AllVisualFeatures = new List<VM_VisualFeature>();
            AllVisualFeatureElements = new List<int>();
            isAllVisualFeaturesVisible = true;

            isFinalSegmentPolygonVisible = true;
            isFinalSegmentCentrelineVisible = true;
            isRighthandCircuitPolygonVisible = true;
            isRighthandCircuitCentrelineVisible = true;
            isLefthandCircuitPolygonVisible = true;
            isLefthandCircuitCentrelineVisible = true;


            isDestinationRWYTHRVisible = true;
            isDestinationRWYVisible = true;
            isDivergenceVFSelectionPolyVisible = true;
            isIAPFinalSegmentLegsPointsVisible = true;
            isFinalSegmentLegsPointsVisible = true;
            isIAPFinalSegmentLegsVisible = true;
            isTrackInitialDirectionVisible = true;
            isTrackInitialPositionVisible = true;
            isTrackVisibilityBufferVisible = false;


            DivergenceVFList = new BindingList<VM_VisualFeature>();
            ConvergenceVFList = new BindingList<VM_VisualFeature>();

            //CA_OCA = GlobalVars.CurrADHP.pPtPrj.Z + 300;
            RemoveTempMainPointElements = true;
            FinalSegmentLegsPointsElements = new List<int>();
        }

        private void getAllObstacles()
        {
            AllObstacles = new List<VM_VerticalStructure>();
            for (int i = 0; i < AllObstaclesAIXM.Count; i++)
            {
                VM_VerticalStructure vs = new VM_VerticalStructure(AllObstaclesAIXM[i], null);
                AllObstacles.Add(vs);
            }
            VMManager._instance.GeomOper = new JtsGeometryOperators();
        }


        #region Private
        private double _maxModelRadius;
        #endregion

        #region General
        public JtsGeometryOperators GeomOper;

        public List<VerticalStructure> AllObstaclesAIXM;
        public List<VM_VerticalStructure> AllObstacles;
        public List<VM_VerticalStructure> MSAObstacles;

        public System.ComponentModel.BindingList<VM_TrackSegment> TrackSegmentsList;

        public List<List<VM_VerticalStructure>> TrackSegmentObstacles;
        public List<List<VM_VisualFeature>> TrackSegmentVisualFeatures;
        
        public List<VM_VisualFeature> AllVisualFeatures;    
        public double MOC;
        public double MinOCH;
        public double MinOCA;

        public bool isAllVisualFeaturesVisible;
        public List<int> AllVisualFeatureElements;
        #endregion

        #region Main Form Pages

        #region MF_Page1
        public int Category;
        public MultiPolygon ConvexPoly;
        public double ConvextPolyElevation;
        public BindingList<InstrumentApproachProcedure> IAPList;
        public InstrumentApproachProcedure SelectedIAP;
        public Point TrackInitialPosition;
        public double TrackInitialDirection;
        public double TrackInitialPositionAltitude;
        //CA = Circling Area
        public double CA_OCA;
        public double CA_OCA_Actual;
        public double CA_OCH;
        public double CA_OCH_Actual;
        public double CA_IAS;
        public double CA_TAS;
        public double CA_TASWind;
        public double CA_RadiusOfTurn;
        public double CA_RadiusFromTHR;
        public double CA_BankAngle;
        public double CA_RateOfTurn;
        public double CA_StraightSegment;
        //public double ADElevation;
        public string CA_ObstacleID;

        public double ArrivalRadius;
        public double ArrivalRadiusMax;
        public double ArrivalRadiusMin;

        public Polygon DivergenceVFSelectionPoly;
        public BindingList<VM_VisualFeature> DivergenceVFList;

        public List<DesignatedPoint> DesignatedPointsList;
        public DesignatedPoint SelectedDesignatedPoint;
        public int DesignatedPointElement;
        public int RWYTHRSelectElement;
        public int DesignatedPointToNavaidSegmentElement;
        public int CirclingAreaPolyElement;
        public int ArrivalAreaPolyElement;
        public int RWYElement;
        public List<int> FinalSegmentLegsPointsElements;
        public int finalSegmentLegsTrajectoryPrjMergedElement;
        public int TrackInitialPositionElement;
        public int TrackInitialDirectionElement;
        public int DivergenceVFSelectionPolyElement;
        public int SelectedDivergenceVFElement;
        public int initPointElement;

        public bool isFinalSegmentLegsPointsVisible;
        public bool isIAPFinalSegmentLegsVisible;
        public bool isIAPFinalSegmentLegsPointsVisible;
        public bool isTrackInitialDirectionVisible;
        public bool isTrackInitialPositionVisible;
        public bool isDestinationRWYVisible;
        public bool isDestinationRWYTHRVisible;
        public bool isDivergenceVFSelectionPolyVisible;
        #endregion

        #region MF_Page2
        public NavaidType FinalNavaid;
        public RWYType SelectedRWY;
        public double TrueBRGAngle;

        public int FASegmElement;
        public int LeftPolyElement;
        public int RightPolyElement;
        public int PrimePolyElement;
        #endregion

        #region MF_Page3
        public MultiPolygon FinalSegmentPolygon;
        public LineString FinalSegmentCentreline;
        public MultiPolygon RighthandCircuitPolygon;
        public LineString RighthandCircuitCentreline;
        public MultiPolygon LefthandCircuitPolygon;
        public LineString LefthandCircuitCentreline;
        public List<VM_VerticalStructure> RighthandCircuitObstaclesList;
        public List<VM_VerticalStructure> LefthandCircuitObstaclesList;
        public double RighthandCircuitOCA;
        public double LefthandCircuitOCA;
        public double RighthandCircuitOCH;
        public double LefthandCircuitOCH;
        public double CorridorSemiWidth;
        public double FinalSegmentLength;
        public Point FinalSegmentStartPoint;
        public double FinalSegmentStartPointDirection;
        public BestCircuit bestCircuit;
        public int RighthandCircuitHighestObstacleIndex;
        public int LefthandCircuitHighestObstacleIndex;
        public int rightExtendedDownwindStartIndex;
        public int rightExtendedDownwindEndIndex;
        public int leftExtendedDownwindStartIndex;
        public int leftExtendedDownwindEndIndex;

        public double VM_IAS;
        public double Min_VM_IAS;
        public double Max_VM_IAS;
        public double VM_OCA;
        public double VM_TAS;
        public double VM_TASWind;
        public double MaxRateOfDescent;
        public double MinRateOfDescent;
        public double MinDescentGradient;
        public double MaxDescentGradient;
        public double FinalSegmentIAS;
        public double MinFinalSegmentIAS;
        public double MaxFinalSegmentIAS;
        public double FinalSegmentTAS;
        public double FinalSegmentTASWind;
        public double FinalSegmentTime;
        public double ISA;
        public double VM_TurnRadius;
        public double VM_BankAngle;
        public double MinVisibilityDistance;
        public double VisibilityDistance;
        public double BankEstablishmentTime;
        public double PilotReactionTime;
        //public double FinalSegmentStartPointHeight;
        public double FinalSegmentStartPointAltitude;
        public double RateOfDescent;

        public bool isFinalSegmentPolygonVisible;
        public bool isFinalSegmentCentrelineVisible;
        public bool isRighthandCircuitPolygonVisible;
        public bool isRighthandCircuitCentrelineVisible;
        public bool isLefthandCircuitPolygonVisible;
        public bool isLefthandCircuitCentrelineVisible;

        public int FinalSegmentPolygonElement;
        public int FinalSegmentCentrelineElement;
        public int RighthandCircuitPolygonElement;
        public int RighthandCircuitCentrelineElement;
        public int LefthandCircuitPolygonElement;
        public int LefthandCircuitCentrelineElement;
        #endregion

        #endregion

        #region New Step Form Pages
        #region NS_Page1
        public Point InitialPosition;
        public double InitialDirection;
        public bool isFinalStep;
        public int InitialPositionElement;
        public int InitialDirectionElement;
        #endregion

        #region NS_Page2
        public double MaxDivergenceAngle;
        public double MaxConvergenceAngle;
        #endregion

        #region NS_Page3
        public List<VM_VisualFeature> ReachableVFs;        
        public double MinConvergenceDirection;
        public double MaxConvergenceDirection;
        public MultiPolygon ConvergencePoly;
        public Point ConvergenceFlyByPoint;
        public Point ConvergenceStartPoint;
        public double IntermediateDirection;
        public LineString DivergenceLine;

        public List<int> DivergenceLineElements;
        public int ConvergencePolyElement;
        public int SelectedVFElement;
        public List<int> ReachableVFsElements;
        public int FinalSegmentStartPntElement;
        #endregion

        #region NS_Page4
        //public List<VM_VisualFeature> VFsWithinPoly;
        public List<double> FinalDirections;
        public double DivergenceAngle;
        public double ConvergenceAngle;        
        public double StepLength;
        public Point FinalPosition;
        public double FinalDirection;
        public LineString ConvergenceLine;

        public List<int> ConvergenceLineElements;
        #endregion

        #region NS_Page
        public LineString StepCentreline;
        public MultiPolygon StepBufferPoly;
        public MultiPolygon StepVisibilityPoly;
        public String StepDescription;
        public double ActualFinalSegmentTime;
        public double ActualFinalSegmentLength;
        public double ActualRateOfDescent;
        public double DescentGradient;

        public double DistanceTillDivergence;
        public double DistanceTillConvergence;
        public double DistanceTillEndPoint;
        public TurnDirection DivergenceSide;
        public TurnDirection ConvergenceSide;

        public int StepVisibilityPolyElement;
        public int StepBufferPolyElement;
        public int StepCentrelineElement;
        public List<int> StepTurnStartEndCrossLines;
        public List<int> StepCentrelinElements;
        public List<int> StepBufferPolyElements;
        public List<int> StepVisibilityPolyElements;
        public List<List<int>> StepMainPointsElements;
        public List<List<int>> StepsTurnStartEndCrossLines;
        public List<int> TempMainPointElements;
        public bool RemoveTempMainPointElements;

        public int ConvergenceVFSelectionPolyElement;
        public int SelectedConvergenceVFElement;

        public Polygon ConvergenceVFSelectionPoly;
        public BindingList<VM_VisualFeature> ConvergenceVFList;


        public bool isTrackVisibilityBufferVisible;

        #endregion



        #endregion

    }
}
