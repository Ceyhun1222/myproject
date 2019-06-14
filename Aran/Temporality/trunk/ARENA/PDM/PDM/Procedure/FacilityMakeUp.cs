using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace PDM
{
    public class FacilityMakeUp : PDMObject
    {
        private AngleIndication _AngleIndication;
        [Browsable(false)]
        public AngleIndication AngleIndication
        {
            get { return _AngleIndication; }
            set { _AngleIndication = value; }
        }

        private DistanceIndication _DistanceIndication;
        [Browsable(false)]
        public DistanceIndication DistanceIndication
        {
            get { return _DistanceIndication; }
            set { _DistanceIndication = value; }
        }

        //private DesignatorType _designator_type;

        //public DesignatorType Designator_type
        //{
        //    get { return _designator_type; }
        //    set { _designator_type = value; }
        //}

        private CodeReferenceRole _role;

        public CodeReferenceRole Role
        {
            get { return _role; }
            set { _role = value; }
        }

        //private string _designatedPontID;

        //public string DesignatedPontID
        //{
        //    get { return _designatedPontID; }
        //    set { _designatedPontID = value; }
        //}

        //private PointChoice _designatedPointType;

        //public PointChoice DesignatedPointType
        //{
        //    get { return _designatedPointType; }
        //    set { _designatedPointType = value; }
        //}

        private string _SegmentPointID;
        [Browsable(false)]
        public string SegmentPointID
        {
            get { return _SegmentPointID; }
            set { _SegmentPointID = value; }
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
                return PDM_ENUM.FacilityMakeUp.ToString();
            }
        } 

        private double _PriorFixTolerance;
        public double PriorFixTolerance
        {
            get { return _PriorFixTolerance; }
            set { _PriorFixTolerance = value; }
        }

        private UOM_DIST_HORZ _PriorFixToleranceUom;
        public UOM_DIST_HORZ PriorFixToleranceUom
        {
            get { return _PriorFixToleranceUom; }
            set { _PriorFixToleranceUom = value; }
        }

        private double _PostFixTolerance;
        public double PostFixTolerance
        {
            get { return _PostFixTolerance; }
            set { _PostFixTolerance = value; }
        }

        private UOM_DIST_HORZ _PostFixToleranceUom;
        public UOM_DIST_HORZ PostFixToleranceUom
        {
            get { return _PostFixToleranceUom; }
            set { _PostFixToleranceUom = value; }
        }


        public FacilityMakeUp()
        {
        }

        public FacilityMakeUp(bool ARINC_TYPE)
        {
            this.ID = Guid.NewGuid().ToString();
            this.AngleIndication = new AngleIndication();
            this.DistanceIndication = new DistanceIndication();

            this.AngleIndication.FacilityMakeUp_ID = this.ID;
            this.DistanceIndication.FacilityMakeUp_ID = this.ID;
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
                findx = row.Fields.FindField("SegmentPointID"); if (findx >= 0) row.set_Value(findx, this.SegmentPointID);
                //findx = row.Fields.FindField("designator_type"); if (findx >= 0) row.set_Value(findx, this.Designator_type);
                
                //findx = row.Fields.FindField("pointType"); if (findx >= 0) row.set_Value(findx, this.DesignatedPointType.ToString());
                //findx = row.Fields.FindField("pointChoiceID"); if (findx >= 0) row.set_Value(findx, this.DesignatedPontID);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

                findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
                
                row.Store();



                if (this.DistanceIndication != null)
                {
                    this.DistanceIndication.FacilityMakeUp_ID = this.ID;
                    this.DistanceIndication.StoreToDB(AIRTRACK_TableDic);
                }
                if (this.AngleIndication != null)
                {
                    this.AngleIndication.FacilityMakeUp_ID = this.ID;
                    this.AngleIndication.StoreToDB(AIRTRACK_TableDic);
                }


              



            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}

            return res;
        }

        public override string GetObjectLabel()
        {
            return "";
        }

    }
}
