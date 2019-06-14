using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{

    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class SafeAltitudeArea : PDMObject
    {
        private CodeSafeAltitude _safeAreaType;
        [Description("Indicates the type of area, either MSA or ESA")]
        [PropertyOrder(10)]
        public CodeSafeAltitude SafeAreaType
        {
            get { return _safeAreaType; }
            set { _safeAreaType = value; }
        }

        private RouteSegmentPoint _centrePoint;
        [PropertyOrder(20)]
        [ReadOnly(true)]
        public RouteSegmentPoint CentrePoint
        {
            get { return _centrePoint; }
            set { _centrePoint = value; }
        }

        private List<SafeAltitudeAreaSector> _safeAltitudeAreaSector;
        [PropertyOrder(30)]
        //[Browsable(false)]
        public List<SafeAltitudeAreaSector> SafeAltitudeAreaSector
        {
            get { return _safeAltitudeAreaSector; }
            set { _safeAltitudeAreaSector = value; }
        }


        public SafeAltitudeArea()
        {
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.SafeAltitudeArea;
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


                if (this.SafeAltitudeAreaSector != null)
                {

                    foreach (SafeAltitudeAreaSector vol in this.SafeAltitudeAreaSector)
                    {
                        vol.SafeArea_ID = this.ID;
                        vol.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }


            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("CenterPointID"); if (findx >= 0) row.set_Value(findx, this.CentrePoint.PointChoiceID);
            findx = row.Fields.FindField("CenterPointName"); if (findx >= 0) row.set_Value(findx, this.CentrePoint.SegmentPointDesignator);
            findx = row.Fields.FindField("PointChoice"); if (findx >= 0) row.set_Value(findx, this.CentrePoint.PointChoice.ToString());
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

        }

        public override string GetObjectLabel()
        {
            return "Safe Area " + this.CentrePoint.SegmentPointDesignator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return 0;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.SafeAltitudeAreaSector != null)
                foreach (var item in SafeAltitudeAreaSector)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
        }


    }

    

    [TypeConverter(typeof(PropertySorter))]
    public class SafeAltitudeAreaSector : PDMObject
    {
        private CodeArcDirection _arcDirection; 
        [Description("Direction indicating clock-wise or counter-clockwise")]
        [PropertyOrder(10)]
        public CodeArcDirection ArcDirection
        {
            get { return _arcDirection; }
            set { _arcDirection = value; }
        }

        private double? _fromAngle;
        [Description("Beginning of angle")]
        [PropertyOrder(20)]
        public double? FromAngle
        {
            get { return _fromAngle; }
            set { _fromAngle = value; }
        }

        private double? _toAngle;	
        [Description("Ending of angle")]
        [PropertyOrder(30)]
        public double? ToAngle
        {
            get { return _toAngle; }
            set { _toAngle = value; }
        }

        private BearingType _angleType; 
        [Description("A code indicating the type of angle: magnetic, bearing, VOR radial.")]
        [PropertyOrder(40)]
        public BearingType AngleType
        {
            get { return _angleType; }
            set { _angleType = value; }
        }

        private CodeDirectionReference _angleDirectionReference;	
        [Description("A code indicating a direction with regard to a reference point.")]
        [PropertyOrder(50)]
        public CodeDirectionReference AngleDirectionReference
        {
            get { return _angleDirectionReference; }
            set { _angleDirectionReference = value; }
        }

        private double? _innerDistance;	
        [Description("Angle sector volume inner limit")]
        [PropertyOrder(60)]
        public double? InnerDistance
        {
            get { return _innerDistance; }
            set { _innerDistance = value; }
        }

        private double? _outerDistance;	
        [Description("Angle sector volume outer limit")]
        [PropertyOrder(70)]
        public double? OuterDistance
        {
            get { return _outerDistance; }
            set { _outerDistance = value; }
        }

        private double? _upperLimitVal;		
        [Description("The uppermost altitude or level that is included in the sector.")]
        [PropertyOrder(80)]
        public double? UpperLimitVal
        {
            get { return _upperLimitVal; }
            set { _upperLimitVal = value; }
        }

        private UOM_DIST_VERT _upperLimitUOM;
        [Description("")]
        [PropertyOrder(90)]
        public UOM_DIST_VERT UpperLimitUOM
        {
            get { return _upperLimitUOM; }
            set { _upperLimitUOM = value; }
        }

        private CODE_DIST_VER _upperLimitReference;		
        [Description("The reference surface used for the value of the upper limit. For example, Mean Sea Level, Ground, standard pressure, etc..")]
        [PropertyOrder(100)]
        public CODE_DIST_VER UpperLimitReference
        {
            get { return _upperLimitReference; }
            set { _upperLimitReference = value; }
        }

        private double? _lowerLimitVal;		
        [Description("The lowermost altitude or level that is included in the sector.")]
        [PropertyOrder(110)]
        public double? LowerLimitVal
        {
            get { return _lowerLimitVal; }
            set { _lowerLimitVal = value; }
        }

        private UOM_DIST_VERT _lowerLimitUOM;
        [Description("")]
        [PropertyOrder(120)]
        public UOM_DIST_VERT LowerLimitUOM
        {
            get { return _lowerLimitUOM; }
            set { _lowerLimitUOM = value; }
        }

        private CODE_DIST_VER _lowerLimitReference;		
        [Description("The reference surface used for the value of the lower limit. For example, Mean Sea Level, Ground, standard pressure, etc.")]
        [PropertyOrder(130)]
        public CODE_DIST_VER LowerLimitReference
        {
            get { return _lowerLimitReference; }
            set { _lowerLimitReference = value; }
        }


        private string safeArea_ID;
        [Browsable(false)]
        public string SafeArea_ID
        {
            get { return safeArea_ID; }
            set { safeArea_ID = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.SafeAltitudeAreaSector;
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

        public SafeAltitudeAreaSector()
        {
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
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }


            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("SafeArea_ID"); if (findx >= 0) row.set_Value(findx, this.safeArea_ID);
            findx = row.Fields.FindField("ArcDirection"); if (findx >= 0) row.set_Value(findx, this.ArcDirection.ToString());
            findx = row.Fields.FindField("FromAngle"); if (findx >= 0) row.set_Value(findx, this.FromAngle);
            findx = row.Fields.FindField("ToAngle"); if (findx >= 0) row.set_Value(findx, this.ToAngle);
            findx = row.Fields.FindField("AngleType"); if (findx >= 0) row.set_Value(findx, this.AngleType.ToString());
            findx = row.Fields.FindField("AngleDirectionReference"); if (findx >= 0) row.set_Value(findx, this.AngleDirectionReference.ToString());
            findx = row.Fields.FindField("InnerDistance"); if (findx >= 0) row.set_Value(findx, this.InnerDistance);
            findx = row.Fields.FindField("OuterDistance"); if (findx >= 0) row.set_Value(findx, this.OuterDistance);
            findx = row.Fields.FindField("UpperLimitVal"); if (findx >= 0) row.set_Value(findx, this.UpperLimitVal);
            findx = row.Fields.FindField("UpperLimitUOM"); if (findx >= 0) row.set_Value(findx, this.UpperLimitUOM.ToString());
            findx = row.Fields.FindField("UpperLimitReference"); if (findx >= 0) row.set_Value(findx, this.UpperLimitReference.ToString());
            findx = row.Fields.FindField("LowerLimit"); if (findx >= 0) row.set_Value(findx, this.LowerLimitVal);
            findx = row.Fields.FindField("LowerLimitUOM"); if (findx >= 0) row.set_Value(findx, this.LowerLimitUOM.ToString());
            findx = row.Fields.FindField("LowerLimitReference"); if (findx >= 0) row.set_Value(findx, this.LowerLimitReference.ToString());


        }

        public override string GetObjectLabel()
        {
            return "Sector" + this.FromAngle.ToString()+" - "+this.ToAngle.ToString();
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return 0;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }




    }

}
