using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
//using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    public class ApproachCondition : PDMObject
    {

        private string _ID_FinalLeg;
        [PropertyOrder(5)]
        [Browsable(false)]
        public string ID_FinalLeg
        {
            get { return _ID_FinalLeg; }
            set { _ID_FinalLeg = value; }
        }


        private CodeMinimaFinalApproachPath _finalApproachPath;
        [PropertyOrder(10)]
        public CodeMinimaFinalApproachPath FinalApproachPath
        {
            get { return _finalApproachPath; }
            set { _finalApproachPath = value; }
        }

        private double _requiredNavigationPerformance;
        [PropertyOrder(20)]
        public double RequiredNavigationPerformance
        {
            get { return _requiredNavigationPerformance; }
            set { _requiredNavigationPerformance = value; }
        }

        private double _climbGradient;
        [PropertyOrder(30)]
        public double ClimbGradient
        {
            get { return _climbGradient; }
            set { _climbGradient = value; }
        }


        private double _minAltitude;
        [PropertyOrder(40)]
        public double MinAltitude
        {
            get { return _minAltitude; }
            set { _minAltitude = value; }
        }

        private UOM_DIST_VERT _minAltitudeUOM;
        [PropertyOrder(50)]
        public UOM_DIST_VERT MinAltitudeUOM
        {
            get { return _minAltitudeUOM; }
            set { _minAltitudeUOM = value; }
        }

        private CodeMinimumAltitude _minAltitudeCode;
        [PropertyOrder(60)]
        public CodeMinimumAltitude MinAltitudeCode
        {
            get { return _minAltitudeCode; }
            set { _minAltitudeCode = value; }
        }

        private CodeVerticalReference _minAltitudeReference;
        [PropertyOrder(70)]
        public CodeVerticalReference MinAltitudeReference
        {
            get { return _minAltitudeReference; }
            set { _minAltitudeReference = value; }
        }

        private double _minHeight;
        [PropertyOrder(80)]
        public double MinHeight
        {
            get { return _minHeight; }
            set { _minHeight = value; }
        }

        private double _minHeightUOM;
        [PropertyOrder(90)]
        public double MinHeightUOM
        {
            get { return _minHeightUOM; }
            set { _minHeightUOM = value; }
        }

        private UOM_DIST_VERT _minMilitaryHeight;
        [PropertyOrder(100)]
        public UOM_DIST_VERT MinMilitaryHeight
        {
            get { return _minMilitaryHeight; }
            set { _minMilitaryHeight = value; }
        }

        private double _minRadiHeight;
        [PropertyOrder(110)]
        public double MinRadioheight
        {
            get { return _minRadiHeight; }
            set { _minRadiHeight = value; }
        }

        private UOM_DIST_VERT _minRadioHeightUOM;
        [PropertyOrder(120)]
        public UOM_DIST_VERT MinRadioHeightUOM
        {
            get { return _minRadioHeightUOM; }
            set { _minRadioHeightUOM = value; }
        }

        private CodeMinimumHeight _minHeightCode;
        [PropertyOrder(130)]
        public CodeMinimumHeight MinHeightCode
        {
            get { return _minHeightCode; }
            set { _minHeightCode = value; }
        }

        private CodeHeightReference _minHeightReference;
        [PropertyOrder(140)]
        public CodeHeightReference MinHeightReference
        {
            get { return _minHeightReference; }
            set { _minHeightReference = value; }
        }

        private double _minVisibility;
        [PropertyOrder(150)]
        public double MinVisibility
        {
            get { return _minVisibility; }
            set { _minVisibility = value; }
        }

        private UOM_DIST_HORZ _minVisibilityUOM;
        [PropertyOrder(160)]
        public UOM_DIST_HORZ MinVisibilityUOM
        {
            get { return _minVisibilityUOM; }
            set { _minVisibilityUOM = value; }
        }

        private double _minMilitaryVisibility;
        [PropertyOrder(170)]
        public double MinMilitaryVisibility
        {
            get { return _minMilitaryVisibility; }
            set { _minMilitaryVisibility = value; }
        }

        private UOM_DIST_HORZ _minMilitaryVisibilityUOM;
        [PropertyOrder(180)]
        public UOM_DIST_HORZ MinMilitaryVisibilityUOM
        {
            get { return _minMilitaryVisibilityUOM; }
            set { _minMilitaryVisibilityUOM = value; }
        }

        private bool _minMandatoryRVR;
        [PropertyOrder(190)]
        public bool MinMandatoryRVR
        {
            get { return _minMandatoryRVR; }
            set { _minMandatoryRVR = value; }
        }

        private bool _minRemoteAltimeterMinima;
        [PropertyOrder(200)]
        public bool MinRemoteAltimeterMinima
        {
            get { return _minRemoteAltimeterMinima; }
            set { _minRemoteAltimeterMinima = value; }
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.ApproachCondition.ToString();
            }
        } 



        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }
        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }
        //[Browsable(false)]
        //public override double Elev_m
        //{
        //    get
        //    {
        //        return base.Elev_m;
        //    }
        //    set
        //    {
        //        base.Elev_m = value;
        //    }
        //}
        [Browsable(false)]
        public override double Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }
        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }
    }
}
