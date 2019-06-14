using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;

namespace PDM
{
    [TypeConverter(typeof(PropertySorter))]
    public class RunwayDirection : PDMObject
    {
        private string _ID_Runway;
        [Browsable(false)]
        public string ID_Runway
        {
            get { return _ID_Runway; }
            set { _ID_Runway = value; }
        }

        private string _ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }

        private int _ID_Feature;
        [Browsable(false)]
        public int ID_Feature
        {
            get { return _ID_Feature; }
            set { _ID_Feature = value; }
        }

        private string _designator;
        [Mandatory(true)]
        [PropertyOrder(10)]
        [Description("The full textual designator of the landing and take-off direction.")]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private double _truebearing;
        [Mandatory(true)]
        [PropertyOrder(20)]
        [Description("The true bearing of the Runway direction.")]
        public double TrueBearing
        {
            get { return _truebearing; }
            set { _truebearing = value; }
        }

        private double _magBearing;
        [Mandatory(true)]
        [PropertyOrder(30)]
        [Description("The magnetic bearing.")]
        public double MagBearing
        {
            get { return _magBearing; }
            set { _magBearing = value; }
        }

        private double _landingThresholdElevation;
        [Mandatory(true)]
        [PropertyOrder(40)]
        [Description("The value of the elevation of the RDN.")]
        public double LandingThresholdElevation
        {
            get { return _landingThresholdElevation; }
            set { _landingThresholdElevation = value; }
        }

        //private double _displacedThresholdDistance;
        //[PropertyOrder(50)]
        //[Description("The value of the displaced threshold distance.")]
        //public double DisplacedThresholdDistance
        //{
        //    get { return _displacedThresholdDistance; }
        //    set { _displacedThresholdDistance = value; }
        //}

        private double _ClearWay;
        [PropertyOrder(50)]
        public double ClearWay
        {
            get { return _ClearWay; }
            set { _ClearWay = value; }
        }

        private List<DeclaredDistance> _RdnDeclaredDistance;
        public List<DeclaredDistance> RdnDeclaredDistance
        {
            get { return _RdnDeclaredDistance; }
            set { _RdnDeclaredDistance = value; }
        }

        private double _stopway;
        [PropertyOrder(60)]
        [Description("The value of the stopway.")]
        public double Stopway
        {
            get { return _stopway; }
            set { _stopway = value; }
        }

        private UOM_DIST_HORZ _uom;
        [Mandatory(true)]
        [PropertyOrder(70)]
        [Description("The unit of measurement for the horizontal dimensions of the Runway direction.")]
        public UOM_DIST_HORZ Uom
        {
            get { return _uom; }
            set { _uom = value; }
        }

        private List<NavaidSystem> _Related_NavaidSystem;
        [Browsable(false)]
        public List<NavaidSystem> Related_NavaidSystem
        {
            get { return _Related_NavaidSystem; }
            set { _Related_NavaidSystem = value; }
        }


        private List<RunwayCenterLinePoint> _CenterLinePoints;
        [Browsable(false)]
        public List<RunwayCenterLinePoint> CenterLinePoints
        {
            get { return _CenterLinePoints; }
            set { _CenterLinePoints = value; }
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.RunwayDirection.ToString();
            }
        } 

        public RunwayDirection()
        {
            //throw new System.NotImplementedException();
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

                if (this.Related_NavaidSystem != null)
                {

                    foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                    {
                        nvd.ID_AirportHeliport = this.ID_AirportHeliport;
                        nvd.ID_RunwayDirection = this.ID;
                        nvd.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.CenterLinePoints != null)
                {

                    foreach (RunwayCenterLinePoint clp in this.CenterLinePoints)
                    {
                        clp.ID_RunwayDirection = this.ID;
                        clp.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}
            {
                res = false;
            }

           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Runway"); if (findx >= 0) row.set_Value(findx, this.ID_Runway);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("magBearing"); if (findx >= 0) row.set_Value(findx, this.MagBearing);
            findx = row.Fields.FindField("trueBearing"); if (findx >= 0) row.set_Value(findx, this.TrueBearing);
            findx = row.Fields.FindField("landingThresholdElevation"); if (findx >= 0) row.set_Value(findx, this.LandingThresholdElevation);
            //findx = row.Fields.FindField("displacedThresholdDistance"); if (findx >= 0) row.set_Value(findx, this.DisplacedThresholdDistance);
            findx = row.Fields.FindField("stopway"); if (findx >= 0) row.set_Value(findx, this.Stopway);
            findx = row.Fields.FindField("Lat"); if (findx >= 0) row.set_Value(findx, this.Lat);
            findx = row.Fields.FindField("Lon"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("Uom"); if (findx >= 0) row.set_Value(findx, this.Uom.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }
        }

        public override string GetObjectLabel()
        {
            return "Rdn " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            ITable tbl = AIRTRACK_TableDic[this.GetType()];



            if (this.Related_NavaidSystem != null)
            {
                foreach (NavaidSystem ns in this.Related_NavaidSystem)
                {
                    ns.DeleteObject(AIRTRACK_TableDic);
                }
            }


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

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
             List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

             if (this.Related_NavaidSystem != null)
             {

                 foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                 {
                     List<string> part =nvd.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(part);
                 }
             }

             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Related_NavaidSystem != null)
            {

                foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                {
                    List<string> part = nvd.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return "Rdn " + this.Designator;
        }

    }

    public class DeclaredDistance
    {
        private CodeDeclaredDistance _distanceType;
        public CodeDeclaredDistance DistanceType
        {
            get { return _distanceType; }
            set { _distanceType = value; }
        }

        private double _distanceValue;
        public double DistanceValue
        {
            get { return _distanceValue; }
            set { _distanceValue = value; }
        }

        private UOM_DIST_HORZ _distanceUOM;
        public UOM_DIST_HORZ DistanceUOM
        {
            get { return _distanceUOM; }
            set { _distanceUOM = value; }
        }

        public DeclaredDistance()
        {
        }

        public override string ToString()
        {
            return this.DistanceType.ToString();
        }
    }
}
