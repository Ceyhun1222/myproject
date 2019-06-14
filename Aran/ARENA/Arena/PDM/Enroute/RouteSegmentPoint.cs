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
    [Serializable()]
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
        public override double? Elev
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

        [ReadOnly(true)]
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

        [ReadOnly(true)]
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
        public override double? LeadDME
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

        [ReadOnly(true)]
        public override double? LeadRadial
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

        [ReadOnly(true)]
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

        [ReadOnly(true)]
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

        [ReadOnly(true)]
        public override ProcedureFixRoleType? PointRole
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
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.RouteSegmentPoint;
            }
        } 

        public RouteSegmentPoint()
        {
        }



        public override void RebuildGeo()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            double elevM = this.Elev != null && this.Elev.HasValue ? this.ConvertValueToMeter(this.Elev.Value, this.Elev_UOM.ToString()) : this.ConvertValueToMeter(0, this.Elev_UOM.ToString());
            if (this.Lat != null && this.Lat.Length > 0 && this.Lon != null && this.Lon.Length > 0)
                this.Geo = ArnUtil.Create_ESRI_POINT(this.Lat, this.Lon, elevM.ToString(), "M");

        }
    }
}
