using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
    [Serializable()]
    public class Taxiway : PDMObject
    {
        private string _ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }

        private string _designator;
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private CodeTaxiwayType _taxiwayTypeType;
        public CodeTaxiwayType TaxiwayType
        {
            get { return _taxiwayTypeType; }
            set { _taxiwayTypeType = value; }
        }
        
        private double? _width;
        public double? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private double? _length;
        public double? Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private string _codeComposition;
        public string CodeComposition
        {
            get { return _codeComposition; }
            set { _codeComposition = value; }
        }

        private UOM_DIST_HORZ _lengthUom;
        public UOM_DIST_HORZ LengthUom
        {
            get { return _lengthUom; }
            set { _lengthUom = value; }
        }

        private UOM_DIST_HORZ _widthUom;
        public UOM_DIST_HORZ WidthUom
        {
            get { return _widthUom; }
            set { _widthUom = value; }
        }

        private List<TaxiwayElement> _taxiWayElements;
        public List<TaxiwayElement> TaxiWayElementsList
        {
            get { return _taxiWayElements; }
            set { _taxiWayElements = value; }
        }

        private List<TaxiwayMarking> _TaxiwayMarkingList;

        public List<TaxiwayMarking> TaxiwayMarkingList
        {
            get { return _TaxiwayMarkingList; }
            set { _TaxiwayMarkingList = value; }
        }

        private TaxiwayLightSystem _LightSystem;
        [XmlElement]
        [Browsable(false)]
        public TaxiwayLightSystem LightSystem
        {
            get { return _LightSystem; }
            set { _LightSystem = value; }
        }

        private List<DeicingArea> _DeicingAreaList;
        [XmlElement]
        [Browsable(false)]
        public List<DeicingArea> DeicingAreaList
        {
            get { return _DeicingAreaList; }
            set { _DeicingAreaList = value; }
        }

        private List<GuidanceLine> _GuidanceLineList;
        [XmlElement]
        //[Browsable(false)]
        public List<GuidanceLine> GuidanceLineList
        {
            get { return _GuidanceLineList; }
            set { _GuidanceLineList = value; }
        }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.Taxiway;

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
 
        public override string GetObjectLabel()
        {
 	        return this.Designator;
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

                if (this.TaxiWayElementsList != null)
                {

                    foreach (TaxiwayElement txwEl in this.TaxiWayElementsList)
                    {
                        txwEl.ID_AirportHeliport = this.ID_AirportHeliport;
                        txwEl.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.TaxiwayMarkingList != null)
                {

                    foreach (TaxiwayMarking rel in this.TaxiwayMarkingList)
                    {
                        rel.ID_Taxiway = this.ID;
                        rel.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.LightSystem != null)
                {
                    this.LightSystem.Taxiway_ID = this.ID;
                    this.LightSystem.StoreToDB(AIRTRACK_TableDic);
                }

               
                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);


            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
            
            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length.Value);
            findx = row.Fields.FindField("lengthUom"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.LengthUom.ToString());
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width.Value);
            findx = row.Fields.FindField("widthUom"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.WidthUom.ToString());
            findx = row.Fields.FindField("CodeComposition"); if (findx >= 0) row.set_Value(findx, this.CodeComposition);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.TaxiwayType.ToString());

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.TaxiWayElementsList!=null)
                foreach (var item in TaxiWayElementsList)
                {
                    res = res || item.CompareId(AnotherID);
                }

                return res;
        }

    }

    public class TaxiwayElement : PDMObject
    {
        public string ID_Taxiway { get; set; }

        public string Designator { get; set; }

        [Browsable(false)]
        public string ID_AirportHeliport { get; set; }


        public CodeTaxiwayElementType TaxiwayElementType { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        private double? _width;
        public double? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private double? _length;
        public double? Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private string _codeComposition;
        public string CodeComposition
        {
            get { return _codeComposition; }
            set { _codeComposition = value; }
        }

        private UOM_DIST_HORZ _lengthUom;
        public UOM_DIST_HORZ LengthUom
        {
            get { return _lengthUom; }
            set { _lengthUom = value; }
        }

        private UOM_DIST_HORZ _widthUom;
        public UOM_DIST_HORZ WidthUom
        {
            get { return _widthUom; }
            set { _widthUom = value; }
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

        [Browsable(false)]
        public byte[] TwyGeometry { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.TaxiwayElement;

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

                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);

            }
            catch (Exception ex)  
            {
                this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; 
                res = false;
            }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);//
            findx = row.Fields.FindField("ID_Taxiway"); if (findx >= 0) row.set_Value(findx, this.ID_Taxiway);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length.Value);
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width.Value);
            findx = row.Fields.FindField("widthUom"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.WidthUom.ToString());
            findx = row.Fields.FindField("lengthUom"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.LengthUom.ToString());
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.TaxiwayElementType.ToString());
            findx = row.Fields.FindField("CodeComposition"); if (findx >= 0) row.set_Value(findx, this.CodeComposition);
            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

        }

        public override void RebuildGeo()
        {

            //string[] words = ((string)this.TwyGeometry).Split(':');

            //byte[] bytes = new byte[words.Length];

            //for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);
            if (this.TwyGeometry == null) return;

            byte[] bytes = (byte[])this.TwyGeometry;
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

       

 
    }
}

