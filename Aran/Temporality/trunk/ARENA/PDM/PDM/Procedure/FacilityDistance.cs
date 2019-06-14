using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    public class FacilityDistance : PDMObject
    {

        private string _facilityMakeUp_ID;

        public string FacilityMakeUp_ID
        {
            get { return _facilityMakeUp_ID; }
            set { _facilityMakeUp_ID = value; }
        }
        private double _Distance;

        public double Distance
        {
            get { return _Distance; }
            set { _Distance = value; }
        }
        private UOM_DIST_HORZ _DistanceUOM;

        public UOM_DIST_HORZ DistanceUOM
        {
            get { return _DistanceUOM; }
            set { _DistanceUOM = value; }
        }
        private string _designatedPontID;

        public string DesignatedPontID
        {
            get { return _designatedPontID; }
            set { _designatedPontID = value; }
        }
        private PointChoice _designatedPointType;

        public PointChoice DesignatedPointType
        {
            get { return _designatedPointType; }
            set { _designatedPointType = value; }
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

        public FacilityDistance()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                int findx = -1;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
                IRow row = tbl.CreateRow();

                findx = row.Fields.FindField("facilityMakeUp_ID"); if (findx >= 0) row.set_Value(findx, this.FacilityMakeUp_ID);
                findx = row.Fields.FindField("Distance"); if (findx >= 0) row.set_Value(findx, this.Distance);
                findx = row.Fields.FindField("DistanceUOM"); if (findx >= 0) row.set_Value(findx, this.DistanceUOM.ToString());

                findx = row.Fields.FindField("pointType"); if (findx >= 0) row.set_Value(findx, this.DesignatedPointType.ToString());
                findx = row.Fields.FindField("pointChoiceID"); if (findx >= 0) row.set_Value(findx, this.DesignatedPontID);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

                findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
                row.Store();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                res = false;
            }

            return res;
        }


    }
}
