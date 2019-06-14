using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    public class AngleIndication : PDMObject
    {
        private string _facilityMakeUp_ID;
         [Browsable(false)]
        public string FacilityMakeUp_ID
        {
            get { return _facilityMakeUp_ID; }
            set { _facilityMakeUp_ID = value; }
        }

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        private CodeBearing _angleType;

        public CodeBearing AngleType
        {
            get { return _angleType; }
            set { _angleType = value; }
        }

        private CodeDirectionReference _indicationDirection;

        public CodeDirectionReference IndicationDirection
        {
            get { return _indicationDirection; }
            set { _indicationDirection = value; }
        }

        private double _trueAngle;

        public double TrueAngle
        {
            get { return _trueAngle; }
            set { _trueAngle = value; }
        }

        private CodeCardinalDirection _cardinalDirection;

        public CodeCardinalDirection CardinalDirection
        {
            get { return _cardinalDirection; }
            set { _cardinalDirection = value; }
        }

        private bool _alongCourseGuidance;

        public bool AlongCourseGuidance
        {
            get { return _alongCourseGuidance; }
            set { _alongCourseGuidance = value; }
        }

        private double _minimumReceptionAltitude;

        public double MinimumReceptionAltitude
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
                return PDM_ENUM.AngleIndication.ToString();
            }
        } 

        public AngleIndication()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public override void CompileRow(ref IRow row)
        {

            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("facilityMakeUp_ID"); if (findx >= 0) row.set_Value(findx, this.FacilityMakeUp_ID);
            findx = row.Fields.FindField("angle"); if (findx >= 0) row.set_Value(findx, this.Angle);
            findx = row.Fields.FindField("angleType"); if (findx >= 0) row.set_Value(findx, this.AngleType.ToString());
            findx = row.Fields.FindField("indicationDirection"); if (findx >= 0) row.set_Value(findx, this.IndicationDirection.ToString());
            findx = row.Fields.FindField("trueAngle"); if (findx >= 0) row.set_Value(findx, this.TrueAngle);
            findx = row.Fields.FindField("cardinalDirection"); if (findx >= 0) row.set_Value(findx, this.CardinalDirection.ToString());
            findx = row.Fields.FindField("alongCourseGuidance"); if (findx >= 0) row.set_Value(findx, this.AlongCourseGuidance);
            findx = row.Fields.FindField("minimumReceptionAltitude"); if (findx >= 0) row.set_Value(findx, this.MinimumReceptionAltitude);
            findx = row.Fields.FindField("minimumReceptionAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.MinimumReceptionAltitudeUOM.ToString());
            findx = row.Fields.FindField("fixID"); if (findx >= 0) row.set_Value(findx, this.FixID);
            findx = row.Fields.FindField("SignificantPointID"); if (findx >= 0) row.set_Value(findx, this.SignificantPointID);
            findx = row.Fields.FindField("SignificantPointType"); if (findx >= 0) row.set_Value(findx, this.SignificantPointType);

        }

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

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
