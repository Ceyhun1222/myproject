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
    public class Localizer : NavaidComponent
    {

        //private double? _frequency = null;
        //[Description("The frequency of the localizer.")]
        //[PropertyOrder(10)]
        //public double? Frequency
        //{
        //    get { return _frequency; }
        //    set { _frequency = value; }
        //}

        //private UOM_FREQ _frequency_UOM;
        //[Description("The unit of measurement of the frequency.")]
        //[PropertyOrder(20)]
        //public UOM_FREQ Frequency_UOM
        //{
        //    get { return _frequency_UOM; }
        //    set { _frequency_UOM = value; }
        //}

        private double? _trueBearing = null;
        [Description("The true bearing of the localizer beam, towards the localizer antenna.")]
        [PropertyOrder(30)]
        public double? trueBearing
        {
            get { return _trueBearing; }
            set { _trueBearing = value; }
        }

        private double? _MagBrg = null;
        [Description("The magnetic bearing of the localizer beam, towards the localizer antenna.")]
        [PropertyOrder(35)]
        public double? MagBrg
        {
            get { return _MagBrg; }
            set { _MagBrg = value; }
        }

        private double? _width = null;
        [Description("The localizer course width, in degrees.")]
        [PropertyOrder(50)]
        public double? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private string _declination = null;
        [Description("The localizer course width, in degrees.")]
        [PropertyOrder(60)]
        public string Declination
        {
            get { return _declination; }
            set { _declination = value; }
        }

        private double? _localizer_Position = null;
        [Description("The Localizer Position field defines the location of the facility antenna relative to one end of the runway.")]
        [PropertyOrder(70)]
        public double? Localizer_Position
        {
            get { return _localizer_Position; }
            set { _localizer_Position = value; }
        }

        private UOM_DIST_HORZ _uom;
        [Description("The unit of measurement for the horizontal dimensions.")]
        [PropertyOrder(80)]
        public UOM_DIST_HORZ Uom
        {
            get { return _uom; }
            set { _uom = value; }
        }

        private int? _category = null;
        [PropertyOrder(90)]
        public int? Category
        {
            get { return _category; }
            set { _category = value; }
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
                return PDM_ENUM.Localizer;
            }
        } 


        public Localizer()
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
            findx = row.Fields.FindField("frequency"); if (findx >= 0 && this.Frequency.HasValue) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());
            findx = row.Fields.FindField("true_bearing"); if (findx >= 0 && this.trueBearing.HasValue) row.set_Value(findx, this.trueBearing);
            findx = row.Fields.FindField("MagBrg"); if (findx >= 0 && this.MagBrg.HasValue) row.set_Value(findx, this.MagBrg);
            findx = row.Fields.FindField("width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width);
            findx = row.Fields.FindField("declination"); if (findx >= 0) row.set_Value(findx, this.Declination);
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            //findx = row.Fields.FindField("Localizer_Latitude"); if (findx >= 0) row.set_Value(findx, this.Lat);
            //findx = row.Fields.FindField("Localizer_Position"); if (findx >= 0) row.set_Value(findx, this.Localizer_Position);
            findx = row.Fields.FindField("uom"); if (findx >= 0) row.set_Value(findx, this.Uom.ToString());
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
