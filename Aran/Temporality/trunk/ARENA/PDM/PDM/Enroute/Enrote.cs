using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.ComponentModel;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;

namespace PDM
{
    public class Enroute : PDMObject
    {
        string  _txtDesig;
        [PropertyOrder(10)]
        public string TxtDesig
        {
          get { return _txtDesig; }
          set { _txtDesig = value; }
        }

        private List<RouteSegment> _Routes;
        [Browsable(false)]
        public List<RouteSegment> Routes
        {
            get { return _Routes; }
            set { _Routes = value; }
        }

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

        private double _RouteLength;
        [PropertyOrder(20)]
        public double RouteLength
        {
            get { return _RouteLength; }
            set { _RouteLength = value; }
        }

        private UOM_DIST_HORZ _RouteLength_UOM;
        [PropertyOrder(30)]
        public UOM_DIST_HORZ RouteLength_UOM
        {
            get { return _RouteLength_UOM; }
            set { _RouteLength_UOM = value; }
        }

        private CodeRouteOrigin _RouteOrigin;

        public CodeRouteOrigin InternationalUse
        {
            get { return _RouteOrigin; }
            set { _RouteOrigin = value; }
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
                return PDM_ENUM.Enroute.ToString();
            }
        } 

        public Enroute()
        {
        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();


                if (this.Routes != null)
                {

                    foreach (RouteSegment seg in this.Routes)
                    {
                        //seg.ID_Route = this.ID;
                        seg.StoreToDB(AIRTRACK_TableDic);

                    }
                }

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 



            return res;
        }

        public override void CompileRow(ref ESRI.ArcGIS.Geodatabase.IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            //findx = row.Fields.FindField("timeslice"); if (findx >= 0) row.set_Value(findx, this.timeslice);
            findx = row.Fields.FindField("txtDesig"); if (findx >= 0) row.set_Value(findx, this.TxtDesig);
            findx = row.Fields.FindField("RouteLength"); if (findx >= 0) row.set_Value(findx, this.RouteLength);
            findx = row.Fields.FindField("RouteLength_UOM"); if (findx >= 0) row.set_Value(findx, this.RouteLength_UOM.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("InternationalUse"); if (findx >= 0) row.set_Value(findx, this.InternationalUse.ToString());

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }

        public override string GetObjectLabel()
        {
            return "Enroute " + this.TxtDesig;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (base.DeleteObject(AIRTRACK_TableDic) == 0) return 0;

            ITable tbl = AIRTRACK_TableDic[this.GetType()];


            if (this.Routes != null)
            {

                foreach (RouteSegment seg in this.Routes)
                {
                    if (seg.StartPoint != null)
                    {
                        seg.DeleteObject(AIRTRACK_TableDic);
                    }
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

             if (this.Routes != null)
             {

                 foreach (RouteSegment seg in this.Routes)
                 {
                     List<string> part = seg.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(part);
                 }
             }

             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Routes != null)
            {

                foreach (RouteSegment seg in this.Routes)
                {
                    List<string> part = seg.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return this.TxtDesig;
        }

    }
  
}
