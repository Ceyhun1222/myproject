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
    public class AircraftStand:PDMObject
    {
        public string Designator { get; set; }

        public CodeAircraftStandType AircraftStandType { get; set; }

        public CodeVisualDockingGuidanceType VisualDockingGuidanceType { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public string ID_ApronElement { get; set; }

        [XmlIgnore]
        public IGeometry Extent { get; set; }

        [XmlElement]
       // [Browsable(false)]
        public List<StandMarking> StandMarkingList { get; set; }

        [XmlElement]
        //[Browsable(false)]
        public List<DeicingArea> DeicingAreaList { get; set; }

        [XmlElement]
       // [Browsable(false)]
        public List<GuidanceLine> GuidanceLineList { get; set; }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type => PDM_ENUM.AircraftStand;

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

                if (this.StandMarkingList != null)
                {

                    foreach (StandMarking prt in this.StandMarkingList)
                    {
                        prt.ID_AircraftStand = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

              
                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);


                if (this.Extent != null)
                {
                    int findx = -1;
                    ITable tblGeoPnt = AIRTRACK_TableDic[typeof(AircraftStandExtent)];
                    IRow rowGeoExtent = tblGeoPnt.CreateRow();
                    findx = -1;
                    findx = rowGeoExtent.Fields.FindField("FeatureGUID");
                    if (findx >= 0) rowGeoExtent.Value[findx]= this.ID;

                    findx = rowGeoExtent.Fields.FindField("Shape");
                    rowGeoExtent.Value[findx] = this.Extent;

                    rowGeoExtent.Store();
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.Value[findx]= this.ID;
            findx = row.Fields.FindField("ID_ApronElement"); if (findx >= 0) row.Value[findx] = this.ID_ApronElement;
            findx = row.Fields.FindField("ElementType"); if (findx >= 0) row.Value[findx] = this.AircraftStandType.ToString();
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.Value[findx] = this.Designator;
            findx = row.Fields.FindField("VisualDockingGuidanceType"); if (findx >= 0) row.Value[findx] = this.VisualDockingGuidanceType.ToString();

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.Value[findx] = this.Geo;
            }
           

        }
    }

    public class AircraftStandExtent
    {

    }
}
