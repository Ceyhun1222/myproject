using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM.PropertyExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDM
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class GeoBorder : PDMObject
    {
        private string _name;

        public string GeoBorderName
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _neighborName;

        public string NeighborName
        {
            get { return _neighborName; }
            set { _neighborName = value; }
        }

        private CodeGeoBorder _CodeGeoBorderType;

        public CodeGeoBorder CodeGeoBorderType
        {
            get { return _CodeGeoBorderType; }
            set { _CodeGeoBorderType = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.GeoBorder;
            }
        }

        private byte[] _borderBlobGeometry;
        [Browsable(false)]
        public byte[] BorderBlobGeometry
        {
            get { return _borderBlobGeometry; }
            set { _borderBlobGeometry = value; }
        }


        public GeoBorder()
        {
        }

        public override string GetObjectLabel()
        {
            return this.GeoBorderName;
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

                CompileRow(ref row);

                row.Store();

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }


            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            if (this.Geo != null)
            {

                findx = row.Fields.FindField("Shape");
                if (findx >= 0)
                    row.set_Value(findx, this.Geo);

            }

            if (this.GeoBorderName == null || this.GeoBorderName.Length <= 0) this.GeoBorderName = " ";
            if (this.NeighborName == null || this.NeighborName.Length <= 0) this.NeighborName = " ";


            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("GeoBorderName"); if (findx >= 0) row.set_Value(findx, this.GeoBorderName);
            findx = row.Fields.FindField("CodeGeoBorderType"); if (findx >= 0) row.set_Value(findx, this.CodeGeoBorderType.ToString());
            findx = row.Fields.FindField("NeighborName"); if (findx >= 0) row.set_Value(findx, this.NeighborName);

           

        }

        public override void RebuildGeo()
        {

            //string[] words = ((string)this.BorderBlobGeometry).Split(':');

            //byte[] bytes = new byte[words.Length];

            //for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);
            if (this.BorderBlobGeometry == null) return;

            byte[] bytes = (byte[])this.BorderBlobGeometry;

            // сконвертируем его в геометрию 
            IMemoryBlobStream memBlobStream = new MemoryBlobStream();

            IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

            varBlobStream.ImportFromVariant(bytes);

            IObjectStream anObjectStream = new ObjectStreamClass();
            anObjectStream.Stream = memBlobStream;

            IPropertySet aPropSet = new PropertySetClass();

            IPersistStream aPersistStream = (IPersistStream)aPropSet;
            aPersistStream.Load(anObjectStream);

            this.Geo = aPropSet.GetProperty("Border") as IGeometry;


          
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
    }
}
