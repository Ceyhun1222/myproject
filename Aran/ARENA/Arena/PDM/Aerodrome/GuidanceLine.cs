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
    public class GuidanceLine:PDMObject
    {

        public string Designator { get; set; }

        [XmlIgnore]
        public Dictionary<string, string> ParentList =new Dictionary<string, string>();

        public CodeGuidanceLineType Type { get; set; }
                
        public double? MaxSpeed { get; set; }

        public SpeedType MaxSpeed_UOM { get; set; }

        public CodeDirectionType UsageDirection { get; set; }

        [XmlElement]
        public string TaxiwayName { get; set; } = null;

        [XmlElement]
        //[Browsable(false)]
        public GuidanceLineLightSystem LightSystem { get; set; }

        [XmlElement]
        //[Browsable(false)]
        public List<GuidanceLineMarking> GuidanceLineMarkingList { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.GuidanceLine;


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

                if (this.GuidanceLineMarkingList != null)
                {

                    foreach (GuidanceLineMarking prt in this.GuidanceLineMarkingList)
                    {
                        prt.GuidanceLine_ID = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                

                if (this.LightSystem != null)
                {
                    this.LightSystem.GuidanceLine_ID = this.ID;
                    this.LightSystem.StoreToDB(AIRTRACK_TableDic);
                }


            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);           
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("MaxSpeed"); if (findx >= 0 && this.MaxSpeed!=null) row.set_Value(findx, this.MaxSpeed);
            findx = row.Fields.FindField("MaxSpeed_UOM"); if (findx >= 0) row.set_Value(findx, this.MaxSpeed_UOM);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("UsageDirection"); if (findx >= 0) row.set_Value(findx, this.UsageDirection.ToString());
            findx = row.Fields.FindField("TaxiwayName"); if (findx >= 0) row.set_Value(findx, this.TaxiwayName);

            if (this.ParentList != null)
            {
                foreach (var parent in this.ParentList)
                {
                    findx = row.Fields.FindField(parent.Value.ToString());
                    if (findx >= 0)
                        row.Value[findx] = parent.Key;
                }

            }

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape");
                row.set_Value(findx, this.Geo);
            }


        }

        [Browsable(false)]
        public override string Lat { get => base.Lat; set => base.Lat = value; }

    }
}
