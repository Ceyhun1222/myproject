using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    [Serializable()]
    public class DistanceIndication : PDMObject

    {
        private string _facilityMakeUp_ID;
        [Browsable(false)]
        public string FacilityMakeUp_ID
        {
            get { return _facilityMakeUp_ID; }
            set { _facilityMakeUp_ID = value; }
        }

        private double? _distance = null;

        public double? Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        private UOM_DIST_HORZ _distanceUOM;

        public UOM_DIST_HORZ DistanceUOM
        {
            get { return _distanceUOM; }
            set { _distanceUOM = value; }
        }

        private bool _alongDMEARC;

        public bool AlongDMEARC
        {
            get { return _alongDMEARC; }
            set { _alongDMEARC = value; }
        }

        private double? _minimumReceptionAltitude = null;

        public double? MinimumReceptionAltitude
        {
            get { return _minimumReceptionAltitude; }
            set { _minimumReceptionAltitude = value; }
        }

        private UOM_DIST_VERT _minimumReceptionAltitudeUOM;

        public UOM_DIST_VERT MinimumReceptionAltitudeUOM
        {
            get { return _minimumReceptionAltitudeUOM; }
            set { _minimumReceptionAltitudeUOM = value; }
        }

        private CodeDistanceIndication _type;

        public CodeDistanceIndication Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _fixID;
        [Browsable(false)]
        public string FixID
        {
            get { return _fixID; }
            set { _fixID = value; }
        }

        private string _SignificantPointID;
        [Browsable(false)]
        public string SignificantPointID
        {
            get { return _SignificantPointID; }
            set { _SignificantPointID = value; }
        }

        private string _SignificantPointType;

        public string SignificantPointType
        {
            get { return _SignificantPointType; }
            set { _SignificantPointType = value; }
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
                return PDM_ENUM.DistanceIndication;
            }
        } 

        public DistanceIndication()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("facilityMakeUp_ID"); if (findx >= 0) row.set_Value(findx, this.FacilityMakeUp_ID);
            findx = row.Fields.FindField("distance"); if (findx >= 0) row.set_Value(findx, this.Distance);
            findx = row.Fields.FindField("distanceUOM"); if (findx >= 0) row.set_Value(findx, this.DistanceUOM.ToString());
            findx = row.Fields.FindField("alongDMEARC"); if (findx >= 0) row.set_Value(findx, this.AlongDMEARC);
            findx = row.Fields.FindField("minimumReceptionAltitude"); if (findx >= 0) row.set_Value(findx, this.MinimumReceptionAltitude);
            findx = row.Fields.FindField("minimumReceptionAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.MinimumReceptionAltitudeUOM.ToString());
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("fixID"); if (findx >= 0) row.set_Value(findx, this.FixID);
            findx = row.Fields.FindField("SignificantPointID"); if (findx >= 0) row.set_Value(findx, this.SignificantPointID);
            findx = row.Fields.FindField("SignificantPointType"); if (findx >= 0) row.set_Value(findx, this.SignificantPointType);


        }

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();


            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 



            return res;
        }

        public override string GetObjectLabel()
        {
            return this.SignificantPointType.ToString();
        }

    }
}
