using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    [TypeConverter(typeof(PropertySorter))]
    public class ProcedureTransitions : PDMObject
    {
        private string _FeatureGUID;
        [Browsable(false)]
        public string FeatureGUID
        {
            get { return _FeatureGUID; }
            set { _FeatureGUID = value; }
        }

        private string _TransitionIdentifier;
        [Browsable(false)]
        public string TransitionIdentifier
        {
            get { return _TransitionIdentifier; }
            set { _TransitionIdentifier = value; }
        }

        //private ProcedurePhaseType _RouteType;
        //[Description("Route Type description")]
        //[PropertyOrder(10)]
        //public ProcedurePhaseType RouteType
        //{
        //    get { return _RouteType; }
        //    set { _RouteType = value; }
        //}

        private ProcedurePhaseType _RouteType;
        [Description("Route Type description")]
        [PropertyOrder(10)]
        public ProcedurePhaseType RouteType
        {
            get { return _RouteType; }
            set { _RouteType = value; }
        }

        private string _description;
        [Description("Transition description")]
        [PropertyOrder(20)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        
        private List<ProcedureLeg> _Legs;
        [Browsable(false)]
        public List<ProcedureLeg> Legs
        {
            get { return _Legs; }
            set { _Legs = value; }
        }

        private string _ID_procedure;
        [Browsable(false)]
        public string ID_procedure
        {
            get { return _ID_procedure; }
            set { _ID_procedure = value; }
        }

        private double _vectorHeading;
        [Browsable(false)]
        public double VectorHeading
        {
            get { return _vectorHeading; }
            set { _vectorHeading = value; }
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
                return PDM_ENUM.ProcedureTransitions.ToString();
            }
        } 

        public ProcedureTransitions()
        {
            this.FeatureGUID = Guid.NewGuid().ToString();
        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                int findx = -1;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
                IRow row = tbl.CreateRow();


                findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.FeatureGUID);
                findx = row.Fields.FindField("TransitionIdentifier"); if (findx >= 0) row.set_Value(findx, this.TransitionIdentifier);
                findx = row.Fields.FindField("ID_procedure"); if (findx >= 0) row.set_Value(findx, this.ID_procedure.ToString());
                findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.RouteType.ToString());
                findx = row.Fields.FindField("vectorHeading"); if (findx >= 0) row.set_Value(findx, this.VectorHeading);
                findx = row.Fields.FindField("description"); if (findx >= 0) row.set_Value(findx, this.Description);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
                
                findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

                row.Store();

                if (this.Legs != null)
                {
                    foreach (ProcedureLeg leg in this.Legs)
                    {
                        leg.ProcedureIdentifier = this._ID_procedure;
                        leg.TransitionIdentifier = this.FeatureGUID;
                        leg.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}

            return res;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
             List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

             if (this.Legs != null)
             {
                 foreach (ProcedureLeg leg in this.Legs)
                 {
                     List<string> part = leg.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(part);
                 }
             }

             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Legs != null)
            {
                foreach (ProcedureLeg leg in this.Legs)
                {
                    List<string> part = leg.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string GetObjectLabel()
        {
            return this.RouteType.ToString();
        }

    }

}
