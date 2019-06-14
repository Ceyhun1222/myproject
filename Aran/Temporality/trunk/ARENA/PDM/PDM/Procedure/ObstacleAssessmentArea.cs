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
    public class ObstacleAssessmentArea : PDMObject
    {
        private CodeObstacleAssessmentSurface _type;
        [PropertyOrder(10)]
        public CodeObstacleAssessmentSurface Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _sectionNumber;
        [PropertyOrder(20)]
        public int SectionNumber
        {
            get { return _sectionNumber; }
            set { _sectionNumber = value; }
        }

        private double _slope;
        [PropertyOrder(30)]
        public double Slope
        {
            get { return _slope; }
            set { _slope = value; }
        }

        private double _assessedAltitude;
        [PropertyOrder(40)]
        public double AssessedAltitude
        {
            get { return _assessedAltitude; }
            set { _assessedAltitude = value; }
        }

        private UOM_DIST_VERT _assessedAltitudeUOM;
        [PropertyOrder(50)]
        public UOM_DIST_VERT AssessedAltitudeUOM
        {
            get { return _assessedAltitudeUOM; }
            set { _assessedAltitudeUOM = value; }
        }

        private double _slopeLowerAltitude;
        [PropertyOrder(60)]
        public double SlopeLowerAltitude
        {
            get { return _slopeLowerAltitude; }
            set { _slopeLowerAltitude = value; }
        }

        private UOM_DIST_VERT _slopeLowerAltitudeUOM;
        [PropertyOrder(70)]
        public UOM_DIST_VERT SlopeLowerAltitudeUOM
        {
            get { return _slopeLowerAltitudeUOM; }
            set { _slopeLowerAltitudeUOM = value; }
        }

        private double _gradientLowHigh;
        [PropertyOrder(80)]
        public double GradientLowHigh
        {
            get { return _gradientLowHigh; }
            set { _gradientLowHigh = value; }
        }

        private CodeObstructionIdSurfaceZone _surfaceZone;
        [PropertyOrder(90)]
        public CodeObstructionIdSurfaceZone SurfaceZone
        {
            get { return _surfaceZone; }
            set { _surfaceZone = value; }
        }

        private string _safetyRegulation;
        [PropertyOrder(100)]
        public string SafetyRegulation
        {
            get { return _safetyRegulation; }
            set { _safetyRegulation = value; }
        }

        private List<Obstruction> _significantObstacle;
        [PropertyOrder(110)]
        public List<Obstruction> SignificantObstacle
        {
            get { return _significantObstacle; }
            set { _significantObstacle = value; }
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

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.ObstacleAssessmentArea.ToString();
            }
        } 

    }
}
