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
    public class Obstruction : PDMObject
    {
        private double _requiredClearance;
        [PropertyOrder(10)]
        public double RequiredClearance
        {
            get { return _requiredClearance; }
            set { _requiredClearance = value; }
        }

        private UOM_DIST_HORZ _requiredClearanceUOM;
        [PropertyOrder(20)]
        public UOM_DIST_HORZ RequiredClearanceUOM
        {
            get { return _requiredClearanceUOM; }
            set { _requiredClearanceUOM = value; }
        }

        private double _minimumAltitude;
        [PropertyOrder(30)]
        public double MinimumAltitude
        {
            get { return _minimumAltitude; }
            set { _minimumAltitude = value; }
        }

        private UOM_DIST_HORZ _minimumAltitudeUOM;
        [PropertyOrder(40)]
        public UOM_DIST_HORZ MinimumAltitudeUOM
        {
            get { return _minimumAltitudeUOM; }
            set { _minimumAltitudeUOM = value; }
        }

        private bool _surfacePenetration;
        [PropertyOrder(50)]
        public bool SurfacePenetration
        {
            get { return _surfacePenetration; }
            set { _surfacePenetration = value; }
        }

        private double _slopePenetration;
        [PropertyOrder(60)]
        public double SlopePenetration
        {
            get { return _slopePenetration; }
            set { _slopePenetration = value; }
        }

        private bool _controlling;
        [PropertyOrder(70)]
        public bool Controlling
        {
            get { return _controlling; }
            set { _controlling = value; }
        }

        private bool _closeIn;
        [PropertyOrder(80)]
        public bool CloseIn
        {
            get { return _closeIn; }
            set { _closeIn = value; }
        }

        private VerticalStructure _verticalStructure;
        [PropertyOrder(90)]
        public VerticalStructure VerticalStructure
        {
            get { return _verticalStructure; }
            set { _verticalStructure = value; }
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
                return PDM_ENUM.Obstruction.ToString();
            }
        } 
    }
}
