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
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class GlidePath : NavaidComponent
    {
        //private string _ID_NavaidSystem;
        //[Browsable(false)]
        //public string ID_NavaidSystem
        //{
        //    get { return _ID_NavaidSystem; }
        //    set { _ID_NavaidSystem = value; }
        //}


        private double? _antennaPosition = null;
        [Description("Field defines the location of the antenna with respect to the approach end of the runway.")]
        [PropertyOrder(10)]
        public double? AntennaPosition
        {
            get { return _antennaPosition; }
            set { _antennaPosition = value; }
        }

        private double? _angle;
        [Description("Field defines the glide slope angle of an ILS facility")]
        [PropertyOrder(20)]
        public double? Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        private double? _thresholdCrossingHeight;
        [Description("Field specifies the height above the landing threshold on a normal glide path.")]
        [PropertyOrder(30)]
        [DisplayName ("Rdh")]
        public double? ThresholdCrossingHeight
        {
            get { return _thresholdCrossingHeight; }
            set { _thresholdCrossingHeight = value; }
        }

        private UOM_DIST_VERT _uomDistVer;
        [Description("The unit of measurement for the vertical dimensions.")]
        [PropertyOrder(40)]
        public UOM_DIST_VERT UomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }

        private double? _Freq;
        [PropertyOrder(45)]
        public double? Freq
        {
            get { return _Freq; }
            set { _Freq = value; }
        }

        private UOM_FREQ _uomFreq;
        [Description("The unit of measurement of the frequency.")]
        [PropertyOrder(50)]
        public UOM_FREQ UomFreq
        {
            get { return _uomFreq; }
            set { _uomFreq = value; }
        }

        private UOM_DIST_HORZ _uomDistHor;
        [Description("The unit of measurement for the horizontal dimensions.")]
        [PropertyOrder(60)]
        [Browsable(false)]
        public UOM_DIST_HORZ UomDistHor
        {
            get { return _uomDistHor; }
            set { _uomDistHor = value; }
        }


        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.GlidePath;
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

        public GlidePath()
        {
           // throw new System.NotImplementedException();
        }

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
            catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}
           
           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("antennaPosition"); if (findx >= 0 && this.AntennaPosition.HasValue) row.set_Value(findx, this.AntennaPosition);
            findx = row.Fields.FindField("angle"); if (findx >= 0 && this.Angle.HasValue) row.set_Value(findx, this.Angle);
            findx = row.Fields.FindField("thresholdCrossingHeight"); if (findx >= 0 && this.ThresholdCrossingHeight.HasValue) row.set_Value(findx, this.ThresholdCrossingHeight);
            //findx = row.Fields.FindField("elevation"); if (findx >= 0) row.set_Value(findx, this.Elev_m);
            findx = row.Fields.FindField("Glide_Slope_Latitude"); if (findx >= 0) row.set_Value(findx, this.Lat);
            findx = row.Fields.FindField("Glide_Slope_Longitude"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("UomDistVer"); if (findx >= 0) row.set_Value(findx, this.UomDistVer.ToString());
            findx = row.Fields.FindField("UomFreq"); if (findx >= 0) row.set_Value(findx, this.UomFreq.ToString());
            findx = row.Fields.FindField("UomThresholdCrossingHeight"); if (findx >= 0) row.set_Value(findx, this.UomDistHor.ToString());
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
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
            return this.GetType().Name.ToString();
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
