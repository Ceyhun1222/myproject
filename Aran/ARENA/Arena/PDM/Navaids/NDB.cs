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
    public class NDB : NavaidComponent
    {

        private double? _stationDeclination = null;

        public double? StationDeclination
        {
            get { return _stationDeclination; }
            set { _stationDeclination = value; }
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
                return PDM_ENUM.NDB;
            }
        } 

        public NDB()
        {
            //throw new System.NotImplementedException();
        }


        //public NDB(Object_AIRTRACK objAirtrack)
        //{
        //    new NavaidSystem(objAirtrack);

        //}

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
            findx = row.Fields.FindField("frequency"); if (findx >= 0 && this.Frequency.HasValue) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());
            findx = row.Fields.FindField("stationDeclination"); if (findx >= 0 && this.StationDeclination.HasValue) row.set_Value(findx, this.StationDeclination);
            //findx = row.Fields.FindField("NDB_Latitude"); if (findx >= 0) row.set_Value(findx, this.Lat);
            //findx = row.Fields.FindField("NDB_Longitude"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("MagVar"); if (findx >= 0 && this.MagVar.HasValue) row.set_Value(findx, this.MagVar);
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

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
