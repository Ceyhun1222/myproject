using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PDM
{
    [TypeConverter(typeof(PropertySorter))]
    public class NavaidSystem : PDMObject
    {
        //private string _ID_NavaidSystem;
        //[Browsable(false)]
        //public string ID_NavaidSystem
        //{
        //    get { return _ID_NavaidSystem; }
        //    set { _ID_NavaidSystem = value; }
        //}

        private string _ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }

        private string _ID_RunwayDirection;
        [Browsable(false)]
        public string ID_RunwayDirection
        {
            get { return _ID_RunwayDirection; }
            set { _ID_RunwayDirection = value; }
        }

        private string _ID_Feature;
        [Browsable(false)]
        public string ID_Feature
        {
            get { return _ID_Feature; }
            set { _ID_Feature = value; }
        }

        private string _designator;
        [Mandatory(true)]
        [PropertyOrder(10)]
        [Description("The coded identifier given to the navaid system.")]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private NavaidSystemType _codeNavaidSystemType;
        [Mandatory(true)]
        [PropertyOrder(30)]
        [Description("Type of the navaid service.")]
        public NavaidSystemType CodeNavaidSystemType
        {
            get { return _codeNavaidSystemType; }
            set { _codeNavaidSystemType = value; }
        }

        private string _name;
        [PropertyOrder(20)]
        [Description("The long name given to the composite navaid.")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _services;
        [Browsable(false)]
        public string Services
        {
            get { return _services; }
            set { _services = value; }
        }

        private string _NAVAID_Class;
        //[Browsable(false)]
        [DisplayName("Class/Category")]
        public string NAVAID_Class
        {
            get { return _NAVAID_Class; }
            set { _NAVAID_Class = value; }
        }


        private List<PDMObject> _Components;
        [Browsable(false)]
        public List<PDMObject> Components
        {
            get { return _Components; }
            set { _Components = value; }
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
                return PDM_ENUM.NavaidSystem.ToString();
            }
        } 

        public NavaidSystem()
        {
            //throw new System.NotImplementedException();
        }
 
        //public NavaidSystem(List<Object_AIRTRACK> objAirtrackList)
        //{
        //    this.ID_Feature = Guid.NewGuid().ToString();
        //    //this.ID_NavaidSystem = Guid.NewGuid().ToString();
        //    this.Components = new List<PDMObject>();
        //    this.CodeNavaidSystemType = NavaidSystemType.MKR;



        //    foreach (var objAirtrack in objAirtrackList)
        //    {
        //        if (objAirtrack is Marker_AIRTRACK)
        //        {
        //            this.Designator = ((ARINC_Airport_Marker) ((Marker_AIRTRACK)objAirtrack).ARINC_OBJ).Localizer_Identifier;
        //            this.Name = ((ARINC_Airport_Marker)((Marker_AIRTRACK)objAirtrack).ARINC_OBJ).Localizer_Identifier;
        //            this.CodeNavaidSystemType = NavaidSystemType.NDB;
        //            this.ID_AirportHeliport = ((ARINC_Airport_Marker)((Marker_AIRTRACK)objAirtrack).ARINC_OBJ).Airport_Identifier;
        //            this.ID_RunwayDirection = ((ARINC_Airport_Marker)((Marker_AIRTRACK)objAirtrack).ARINC_OBJ).Runway_Identifier;

        //            Marker mkr = new Marker(objAirtrack);
        //            if (mkr != null)
        //            {
        //                mkr.ID_NavaidSystem = this.ID;
        //                this.Components.Add(mkr);
        //            }

        //        }
        //    }

        //    if (this.Components.Count == 0) this.CodeNavaidSystemType = NavaidSystemType.OTHER;
        //}

        //private NavaidSystem CreateNavaid(Object_AIRTRACK objAirtrack)
        //{
        //    this.Components = new List<PDMObject>();
        //    this.ID_Feature = objAirtrack.ID_AIRTRACK;
        //    //this.ID_NavaidSystem = Guid.NewGuid().ToString();

        //    if (objAirtrack is ILS_AIRTRACK)
        //    {
        //        GlidePath gp = new GlidePath();
        //        Localizer lc = new Localizer();

        //        this.Components.Add(gp );
        //        this.Components.Add(lc);
        //    }



        //    return this;
        //}

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
            ITable tbl = AIRTRACK_TableDic[this.GetType()];
            IRow row = tbl.CreateRow();
            //string ID_NavaidSystem = Guid.NewGuid().ToString();

            CompileRow(ref row);

            row.Store();

            if (this.Components != null)
            {
                foreach (PDMObject component in this.Components)
                {
                    component.ID = Guid.NewGuid().ToString(); //this.ID;
                    component.StoreToDB(AIRTRACK_TableDic);
                }
            }

            }
            catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}

           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("ID_RunwayDirection"); if (findx >= 0) row.set_Value(findx, this.ID_RunwayDirection);
            //findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID_Feature);
            findx = row.Fields.FindField("codeNavaidSystemType"); if (findx >= 0) row.set_Value(findx, this.CodeNavaidSystemType.ToString());
            findx = row.Fields.FindField("name"); if (findx >= 0) row.set_Value(findx, this.Name);
            findx = row.Fields.FindField("services"); if (findx >= 0) row.set_Value(findx, this.Services);
            findx = row.Fields.FindField("NAVAID_Class"); if (findx >= 0) row.set_Value(findx, this.NAVAID_Class);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
        }

        public override string GetObjectLabel()
        {
            return this.CodeNavaidSystemType.ToString() + " " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            ITable tbl = AIRTRACK_TableDic[this.GetType()];

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "ID_NavaidSystem = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            
            return 1;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
            List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

             if (this.Components != null)
             {
                 foreach (PDMObject component in this.Components)
                 {
                     List<string> part = component.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(part);
                 }
             }
             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Components != null)
            {
                foreach (PDMObject component in this.Components)
                {
                    List<string> part = component.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }
            return res;
        }

        public override string ToString()
        {
            return this.CodeNavaidSystemType.ToString() + " " + this.Designator;
        }

    }

    public class NavaidComponent : PDMObject
    {
        private string _ID_NavaidSystem;
        [Browsable(false)]
        public string ID_NavaidSystem
        {
            get { return _ID_NavaidSystem; }
            set { _ID_NavaidSystem = value; }
        }

        private string _codeId;

        public string CodeId
        {
            get { return _codeId; }
            set { _codeId = value; }
        }

        public NavaidComponent()
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
