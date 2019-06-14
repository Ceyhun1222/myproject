using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    public class SegmentPoint : PDMObject
    {
        private FacilityMakeUp _pointFacilityMakeUp;
        [Browsable(false)]
        public virtual FacilityMakeUp PointFacilityMakeUp
        {
            get { return _pointFacilityMakeUp; }
            set { _pointFacilityMakeUp = value; }
        }

        private bool _radarGuidance;

        public virtual bool RadarGuidance
        {
            get { return _radarGuidance; }
            set { _radarGuidance = value; }
        }

        private bool _flyOver;

        public virtual bool FlyOver
        {
            get { return _flyOver; }
            set { _flyOver = value; }
        }

        private CodeATCReporting _reportingATC;

        public virtual CodeATCReporting ReportingATC
        {
            get { return _reportingATC; }
            set { _reportingATC = value; }
        }

        private bool _isWaypoint;

        public virtual bool IsWaypoint
        {
            get { return _isWaypoint; }
            set { _isWaypoint = value; }
        }

        private ProcedureFixRoleType _pointRole;

        public virtual ProcedureFixRoleType PointRole
        {
            get { return _pointRole; }
            set { _pointRole = value; }
        }

        private double _leadRadial;

        public virtual double LeadRadial
        {
            get { return _leadRadial; }
            set { _leadRadial = value; }
        }

        private double _leadDME;

        public virtual double LeadDME
        {
            get { return _leadDME; }
            set { _leadDME = value; }
        }

        private UOM_DIST_HORZ _leadDMUOM;

        public virtual UOM_DIST_HORZ LeadDMEUOM
        {
            get { return _leadDMUOM; }
            set { _leadDMUOM = value; }
        }

        private bool _indicatorFACF;

        public virtual bool IndicatorFACF
        {
            get { return _indicatorFACF; }
            set { _indicatorFACF = value; }
        }

        private string _PointChoiceID;
        [Browsable(false)]
        public virtual string PointChoiceID
        {
            get { return _PointChoiceID; }
            set { _PointChoiceID = value; }
        }

        private PointChoice _PointChoice;

        public virtual PointChoice PointChoice
        {
            get { return _PointChoice; }
            set { _PointChoice = value; }
        }


        private ProcedureSegmentPointUse _pointUse;

        public virtual ProcedureSegmentPointUse PointUse
        {
            get { return _pointUse; }
            set { _pointUse = value; }
        }

        private string _Route_LEG_ID;
        [Browsable(false)]
        public string Route_LEG_ID
        {
            get { return _Route_LEG_ID; }
            set { _Route_LEG_ID = value; }
        }

        private string _SegmentPointDesignator;

        public string SegmentPointDesignator
        {
            get { return _SegmentPointDesignator; }
            set { _SegmentPointDesignator = value; }
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.SegmentPoint.ToString();
            }
        } 

        public SegmentPoint()
        {
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

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                int findx = -1;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
                IRow row = tbl.CreateRow();

                findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);

                System.Diagnostics.Debug.WriteLine(this.ID);
                findx = row.Fields.FindField("Route_LEG_ID"); if (findx >= 0) row.set_Value(findx, this.Route_LEG_ID);

                findx = row.Fields.FindField("PointUse"); if (findx >= 0) row.set_Value(findx, this.PointUse.ToString());
                findx = row.Fields.FindField("radarGuidance"); if (findx >= 0) row.set_Value(findx, this.RadarGuidance);
                findx = row.Fields.FindField("reportingATC"); if (findx >= 0) row.set_Value(findx, this.ReportingATC.ToString());
                findx = row.Fields.FindField("waypoint"); if (findx >= 0) row.set_Value(findx, this.IsWaypoint);
                findx = row.Fields.FindField("flyOver"); if (findx >= 0) row.set_Value(findx, this.FlyOver);
                findx = row.Fields.FindField("role"); if (findx >= 0) row.set_Value(findx, this.PointRole.ToString());
                findx = row.Fields.FindField("leadRadial"); if (findx >= 0) row.set_Value(findx, this.LeadRadial);
                findx = row.Fields.FindField("leadDME"); if (findx >= 0) row.set_Value(findx, this.LeadDME);
                findx = row.Fields.FindField("leadDME_UOM"); if (findx >= 0) row.set_Value(findx, this.LeadDMEUOM.ToString());
                findx = row.Fields.FindField("indicatorFACF"); if (findx >= 0) row.set_Value(findx, this.IndicatorFACF);
                findx = row.Fields.FindField("PointChoice"); if (findx >= 0) row.set_Value(findx, this.PointChoice.ToString());
                findx = row.Fields.FindField("PointChoiceID"); if (findx >= 0) row.set_Value(findx, this.PointChoiceID);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                findx = row.Fields.FindField("SegmentPointDesignator"); if (findx >= 0) row.set_Value(findx, this.SegmentPointDesignator);

                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());




                row.Store();

                if (this.PointFacilityMakeUp != null)
                {
                    this.PointFacilityMakeUp.SegmentPointID = this.ID;
                    findx = row.Fields.FindField("facilityMakeUpID"); if (findx >= 0) row.set_Value(findx, this.PointFacilityMakeUp.ID);
                    this.PointFacilityMakeUp.StoreToDB(AIRTRACK_TableDic);
                }




            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 

            return res;
        }


        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.PointFacilityMakeUp != null)
            {
                List<string> part = this.PointFacilityMakeUp.GetBranch(AIRTRACK_TableDic);
                res.AddRange(part);
            }

            return res;
        }

        public override string X_to_EW_DDMMSS()
        {
            string res = "";

            try
            {


                double Coord = Convert.ToDouble(this.Lon);
                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 0);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lon = degSTR + minSTR + secSTR + sign;

                res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}

            return res;
        }

        public override string Y_to_NS_DDMMSS()
        {
            string res = "";

            try
            {


                double Coord = Convert.ToDouble(this.Lat);

                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);
                //X = RealMode(X, 360);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 2);



                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lat = degSTR + minSTR + secSTR + sign;

                res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;
            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = "";}

            return res;
        }

        public override string GetObjectLabel()
        {
            return this.PointUse.ToString();
        }
    }
}
