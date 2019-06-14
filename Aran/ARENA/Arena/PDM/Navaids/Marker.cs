using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PDM
{
    [Serializable()]
    public class Marker : NavaidComponent
    {
        private string _class;

        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        //private double? _frequency = null;

        //public double? Frequency
        //{
        //    get { return _frequency; }
        //    set { _frequency = value; }
        //}
        //private UOM_FREQ _frequency_UOM;

        //public UOM_FREQ Frequency_UOM
        //{
        //    get { return _frequency_UOM; }
        //    set { _frequency_UOM = value; }
        //}
        private double? _axis_Bearing = null;

        public double? Axis_Bearing
        {
            get { return _axis_Bearing; }
            set { _axis_Bearing = value; }
        }
        private string _codeMorse;

        public string CodeMorse
        {
            get { return _codeMorse; }
            set { _codeMorse = value; }
        }


        //private string _txtName;
        //public string TxtName
        //{
        //    get { return _txtName; }
        //    set { _txtName = value; }
        //}

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
                return PDM_ENUM.Marker;
            }
        } 



        public Marker()
        {

        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
             bool res =true;
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

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("frequency"); if (findx >= 0 && this.Frequency.HasValue) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());
            findx = row.Fields.FindField("class"); if (findx >= 0) row.set_Value(findx, this.Class);
            findx = row.Fields.FindField("axis_Bearing"); if (findx >= 0 && this.Axis_Bearing.HasValue) row.set_Value(findx, this.Axis_Bearing);
            findx = row.Fields.FindField("codeMorse"); if (findx >= 0) row.set_Value(findx, this.CodeMorse);
            //findx = row.Fields.FindField("Localizer_ID"); if (findx >= 0) row.set_Value(findx, this.lo);
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
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
            if ((this.CodeId != null) && (this.CodeId.Trim().Length >0)) return CodeId;
            else
                return this.GetType().Name.ToString();
        }

        public override string ToString()
        {
            if ((this.CodeId != null) && (this.CodeId.Trim().Length > 0)) return CodeId;
            else
                return base.ToString();
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
            qry.WhereClause = "ID_NavaidComponent = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }
    }
}
