using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PDM
{
    public class RouteSegmentPoint : SegmentPoint
    {


        private SegmentPointType _PointType;
        [Browsable(false)]
        public SegmentPointType PointType
        {
            get { return _PointType; }
            set { _PointType = value; }
        }

        private string _pointID;
        [Browsable(false)]
        public string PointID
        {
            get { return _pointID; }
            set { _pointID = value; }
        }

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

        [Browsable(false)]
        public override FacilityMakeUp PointFacilityMakeUp
        {
            get
            {
                return base.PointFacilityMakeUp;
            }
            set
            {
                base.PointFacilityMakeUp = value;
            }
        }

        [Browsable(false)]
        public override bool FlyOver
        {
            get
            {
                return base.FlyOver;
            }
            set
            {
                base.FlyOver = value;
            }
        }

        [Browsable(false)]
        public override bool IndicatorFACF
        {
            get
            {
                return base.IndicatorFACF;
            }
            set
            {
                base.IndicatorFACF = value;
            }
        }

        [Browsable(false)]
        public override double LeadDME
        {
            get
            {
                return base.LeadDME;
            }
            set
            {
                base.LeadDME = value;
            }
        }

        [Browsable(false)]
        public override UOM_DIST_HORZ LeadDMEUOM
        {
            get
            {
                return base.LeadDMEUOM;
            }
            set
            {
                base.LeadDMEUOM = value;
            }
        }

        [Browsable(false)]
        public override double LeadRadial
        {
            get
            {
                return base.LeadRadial;
            }
            set
            {
                base.LeadRadial = value;
            }
        }

        [Browsable(false)]
        public override bool RadarGuidance
        {
            get
            {
                return base.RadarGuidance;
            }
            set
            {
                base.RadarGuidance = value;
            }
        }

        [Browsable(false)]
        public override CodeATCReporting ReportingATC
        {
            get
            {
                return base.ReportingATC;
            }
            set
            {
                base.ReportingATC = value;
            }
        }

        [Browsable(false)]
        public override ProcedureFixRoleType PointRole
        {
            get
            {
                return base.PointRole;
            }
            set
            {
                base.PointRole = value;
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
                return PDM_ENUM.RouteSegmentPoint.ToString();
            }
        } 

        public RouteSegmentPoint()
        {
        }



        public override void RebuildGeo()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            double elevM = this.ConvertValueToMeter(this.Elev, this.Elev_UOM.ToString());
            this.Geo = ArnUtil.Create_ESRI_POINT(this.Lat, this.Lon, elevM.ToString(), "M");

        }
    }
}
