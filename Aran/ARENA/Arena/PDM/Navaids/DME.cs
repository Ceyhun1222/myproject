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
    public class DME : NavaidComponent
    {
        //private string _ID_NavaidSystem;
        //[Browsable(false)]
        //public string ID_NavaidSystem
        //{
        //    get { return _ID_NavaidSystem; }
        //    set { _ID_NavaidSystem = value; }
        //}
        private double? _figureOfMerit = null;
        public double? FigureOfMerit
        {
            get { return _figureOfMerit; }
            set { _figureOfMerit = value; }
        }

        private double? _ilsDmeBias = null;
        public double? IlsDmeBias
        {
            get { return _ilsDmeBias; }
            set { _ilsDmeBias = value; }
        }

        private double? _ghostFrequency = null;
        public double? GhostFrequency
        {
            get { return _ghostFrequency; }
            set { _ghostFrequency = value; }
        }

        private double? _stationDeclination = null;
        public double? StationDeclination
        {
            get { return _stationDeclination; }
            set { _stationDeclination = value; }
        }

        private double? _displace = null;
        public double? Displace
        {
            get { return _displace; }
            set { _displace = value; }
        }

        private CodeDME _dmeType;
        public CodeDME DmeType
        {
            get { return _dmeType; }
            set { _dmeType = value; }
        }

        private string _channel;
        public string Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        //private string _dmeIdentifier;
        //public string DmeIdentifier
        //{
        //    get { return _dmeIdentifier; }
        //    set { _dmeIdentifier = value; }
        //}


        [Browsable(false)]
        public override double? Frequency
        {
            get
            {
                return base.Frequency;
            }
            set
            {
                base.Frequency = value;
            }
        }

        [Browsable(false)]
        public override UOM_FREQ Frequency_UOM
        {
            get
            {
                return base.Frequency_UOM;
            }
            set
            {
                base.Frequency_UOM = value;
            }
        }
        
        [Browsable(true)]
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
        [Browsable(true)]
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
                return PDM_ENUM.DME;
            }
        } 

        public DME()
        {
            //throw new System.NotImplementedException();
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
            findx = row.Fields.FindField("figureOfMerit"); if (findx >= 0 && this.FigureOfMerit.HasValue) row.set_Value(findx, this.FigureOfMerit);
            findx = row.Fields.FindField("ilsDmeBias"); if (findx >= 0 && this.IlsDmeBias.HasValue) row.set_Value(findx, this.IlsDmeBias);
            findx = row.Fields.FindField("stationDeclination"); if (findx >= 0 && this.StationDeclination.HasValue) row.set_Value(findx, this.StationDeclination);
            findx = row.Fields.FindField("Identifier"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);

            findx = row.Fields.FindField("GhostFrequency"); if (findx >= 0) row.set_Value(findx, this.GhostFrequency);
            //findx = row.Fields.FindField("DME_Longitude"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("DmeType"); if (findx >= 0) row.set_Value(findx, this.DmeType.ToString());
            findx = row.Fields.FindField("Channel"); if (findx >= 0) row.set_Value(findx, this.Channel);
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
            return this.GetType().Name.ToString() + " " +this.Designator;
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
            qry.WhereClause = "ID_NavaidComponent = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }

    }
}
