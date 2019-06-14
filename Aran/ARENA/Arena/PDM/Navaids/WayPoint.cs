using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;
using PDM.PropertyExtension;
using System.ComponentModel;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;

namespace PDM
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class WayPoint : PDMObject
    {

        private string _designator;
        [PropertyOrder(10)]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string _name;
        [PropertyOrder(30)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DesignatorType _type;
        [PropertyOrder(20)]
        public DesignatorType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _function;
        [PropertyOrder(40)]
        [Browsable(false)]
        public string Function
        {
            get { return _function; }
            set { _function = value; }
        }

        private string _using;
        [Browsable(false)]
        public string Using
        {
            get { return _using; }
            set { _using = value; }
        }


        private string _Enroute_Terminal;
        [Browsable(false)]
        public string Enroute_Terminal
        {
            get { return _Enroute_Terminal; }
            set { _Enroute_Terminal = value; }
        }


        private string _ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }


        //private CodeATCReporting _reportingATC;
        //[Browsable(false)]
        //public virtual CodeATCReporting ReportingATC
        //{
        //    get { return _reportingATC; }
        //    set { _reportingATC = value; }
        //}

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.WayPoint;
            }
        } 

        public WayPoint()
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

                if (EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();
            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 
            return res;

        }

      

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("name"); if (findx >= 0) row.set_Value(findx, this.Name);
            findx = row.Fields.FindField("Enroute_Terminal"); if (findx >= 0) row.set_Value(findx, this.Enroute_Terminal);
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("using"); if (findx >= 0) row.set_Value(findx, this.Using);
            findx = row.Fields.FindField("function"); if (findx >= 0) row.set_Value(findx, this.Function);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            
            if ((this.Geo != null) && (this.Geo.GeometryType == esriGeometryType.esriGeometryPoint) && !(this.Geo.IsEmpty))
            {
                if ((this.Lat == null) || (this.Lat.Length <= 0))
                {
                    this.Lat = ((IPoint)this.Geo).Y.ToString();
                }

                if ((this.Lon == null) || (this.Lon.Length <= 0))
                {
                    this.Lon = ((IPoint)this.Geo).X.ToString();
                }
            }


            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());

                //findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_DDMMSS_NS());
                //findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_DDMMSS_EW_());

                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }
        }

        public override string GetObjectLabel()
        {
            return this.Designator;
        }

        public override string ToString()
        {
            return this.Designator;
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
