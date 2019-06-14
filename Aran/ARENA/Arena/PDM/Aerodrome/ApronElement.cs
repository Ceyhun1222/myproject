using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PDM
{
    public class ApronElement:PDMObject
    {
        [XmlElement]
       // [Browsable(false)]
        public CodeApronElementType ElementType { get; set; }

        [XmlElement]
        [Browsable(false)]
        public string Apron_ID { get; set; }

        [XmlIgnore]
        public IGeometry Extent { get; set; } = null;

        //[Browsable(false)]
        public List<AircraftStand> AircrafrStandList { get; set; }
        
        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.ApronElement;

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

                if (this.AircrafrStandList != null)
                {

                    foreach (AircraftStand prt in this.AircrafrStandList)
                    {
                        prt.ID_ApronElement = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Apron_ID"); if (findx >= 0) row.set_Value(findx, this.Apron_ID);
            findx = row.Fields.FindField("ElementType"); if (findx >= 0) row.set_Value(findx, this.ElementType.ToString());
            findx = row.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("HorizontalAccuracy"); if (findx >= 0 && this.GeoProperties.HorizontalAccuracy.HasValue) row.set_Value(findx, this.GeoProperties.HorizontalAccuracy);
            findx = row.Fields.FindField("HorizontalAccuracy_UOM"); if (findx >= 0) row.set_Value(findx, this.GeoProperties.HorizontalAccuracy_UOM.ToString());
            findx = row.Fields.FindField("VerticalAccuracy"); if (findx >= 0 && this.GeoProperties.VerticalAccuracy.HasValue) row.set_Value(findx, this.GeoProperties.VerticalAccuracy);
            findx = row.Fields.FindField("VerticalAccuracy_UOM"); if (findx >= 0) row.set_Value(findx, this.GeoProperties.VerticalAccuracy_UOM.ToString());
            findx = row.Fields.FindField("VerticalDatum"); if (findx >= 0) row.set_Value(findx, this.GeoProperties.VerticalDatum.ToString());
            findx = row.Fields.FindField("GeoidUndulation"); if (findx >= 0 && this.GeoProperties.GeoidUndulation.HasValue) row.set_Value(findx, this.GeoProperties.GeoidUndulation);
            findx = row.Fields.FindField("GeoidUndulation_UOM"); if (findx >= 0) row.set_Value(findx, this.GeoProperties.GeoidUndulation_UOM.ToString());
            findx = row.Fields.FindField("Elev_UOM"); if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
            }
        }
    }
}
