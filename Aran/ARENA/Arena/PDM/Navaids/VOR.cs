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
    public class VOR : NavaidComponent
    {
       
        private string _figureOfMerit;

        public string FigureOfMerit
        {
            get { return _figureOfMerit; }
            set { _figureOfMerit = value; }
        }
        private string _stationDeclination;

        public string StationDeclination
        {
            get { return _stationDeclination; }
            set { _stationDeclination = value; }
        }

        private CodeVOR? _vorType;
        public CodeVOR? VorType
        {
            get { return _vorType; }
            set { _vorType = value; }
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
                return PDM_ENUM.VOR;
            }
        }


        public VOR()
        {
            
        }

        //public VOR(Object_AIRTRACK objAirtrack)
        //{
        //    new NavaidSystem(objAirtrack);

        //}

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
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
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                res = false;
            }

            return res;

        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("frequency"); if (findx >= 0 && this.Frequency.HasValue) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());
            findx = row.Fields.FindField("figureOfMerit"); if (findx >= 0) row.set_Value(findx, this.FigureOfMerit);
            findx = row.Fields.FindField("stationDeclination"); if (findx >= 0) row.set_Value(findx, this.StationDeclination);           
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);

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
            return this.VorType.HasValue? this.VorType.ToString() : this.GetType().Name.ToString();
        }

        public override string ToString()
        {
            return this.CodeId;
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
