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
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class NavaidSystem : PDMObject
    {

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
        //[Browsable(false)]
        [ReadOnly(true)]
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

        private CodeSignalPerformanceILS? signalPerformance;
        [PropertyOrder(25)]
        public CodeSignalPerformanceILS? SignalPerformance
        {
            get { return signalPerformance; }
            set { signalPerformance = value; }
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


        private List<NavaidComponent> _Components;
        [Browsable(false)]
        public List<NavaidComponent> Components
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
                return PDM_ENUM.NavaidSystem;
            }
        } 

        public NavaidSystem()
        {
            //throw new System.NotImplementedException();
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
                //string ID_NavaidSystem = Guid.NewGuid().ToString();

                CompileRow(ref row);

                row.Store();

                if (this.Components != null)
                {
                    foreach (PDMObject component in this.Components)
                    {
                        // component.ID = Guid.NewGuid().ToString(); //this.ID;
                        component.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("ID_RunwayDirection"); if (findx >= 0) row.set_Value(findx, this.ID_RunwayDirection);
            //findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_Feature);//!!!!!!!!!!
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);//!!!!!!!!!!!!!!!
            findx = row.Fields.FindField("codeNavaidSystemType"); if (findx >= 0 ) row.set_Value(findx, this.CodeNavaidSystemType.ToString());
            findx = row.Fields.FindField("name"); if (findx >= 0 && this.Name!=null) row.set_Value(findx, this.Name.ToString());
            findx = row.Fields.FindField("services"); if (findx >= 0 && this.Services != null) row.set_Value(findx, this.Services);
            findx = row.Fields.FindField("NAVAID_Class"); if (findx >= 0 && this.NAVAID_Class != null) row.set_Value(findx, this.NAVAID_Class);
            findx = row.Fields.FindField("designator"); if (findx >= 0 && this.Designator != null) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
        }

        public override string GetObjectLabel()
        {
            string lbl = (this.CodeNavaidSystemType.ToString()).Replace("_",@"/") + " " + this.Designator;

            if (this.Components != null && this.Components.Count > 0)
            {
                foreach (var item in this.Components)
                {
                    if (item.PDM_Type == PDM_ENUM.VOR)
                    {
                        if (((VOR)item).VorType.HasValue && ((VOR)item).VorType.Value == CodeVOR.DVOR)
                        {
                            lbl = lbl.Replace("VOR", "DVOR");
                        }
                        break;
                    }
                }
            }
            return lbl;
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
            qry.WhereClause = "ID_NavaidSystem = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            
            return 1;
        }

        public override void RebuildGeo()
        {
            foreach (var item in this.Components)
            {
                item.RebuildGeo();
            }

           
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

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.Components != null)
                foreach (var item in Components)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
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


        private string _designator;
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string _navName;
        public string NavName
        {
            get { return _navName; }
            set { _navName = value; }
        }

        private double? _MagVar = null;

        public double? MagVar
        {
            get { return _MagVar; }
            set { _MagVar = value; }
        }


        private double? _frequency = null;

        public virtual double? Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
        private UOM_FREQ _frequency_UOM;

        public virtual UOM_FREQ Frequency_UOM
        {
            get { return _frequency_UOM; }
            set { _frequency_UOM = value; }
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
