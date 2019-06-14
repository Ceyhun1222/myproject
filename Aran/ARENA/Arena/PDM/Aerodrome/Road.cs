using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
//using System.Diagnostics.Debug;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
    public class Road : PDMObject
    {
        public string Designator { get; set; }

        public CodeStatusOperationsType Status { get; set; }

        public CodeRoadType Type { get; set; }

        public CodeYesNoType Abandoned { get; set; }

        public string ID_AirportHeliport { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public List<AircraftStand> AccessibleStand { get; set; }

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
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
            
            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Status"); if (findx >= 0) row.set_Value(findx, this.Status.ToString());
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("Abandoned"); if (findx >= 0) row.set_Value(findx, this.Abandoned.ToString());

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
            }
        }
    }
}
