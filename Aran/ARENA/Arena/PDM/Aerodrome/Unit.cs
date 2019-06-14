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
    public class Unit:PDMObject
    {
        public string Name { get; set; }


        public CodeUnitType Type { get; set; }

        public CodeYesNoType CompliantICAO { get; set; }

        public string Designator { get; set; }

        public CodeMilitaryOperationsType Military { get; set; }

        public string ID_AirportHeliport { get; set; }

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

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.Value[findx] = this.ID;
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.Value[findx] = this.ID_AirportHeliport;
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.Value[findx] = this.Type.ToString();
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.Value[findx] = this.Designator;
            findx = row.Fields.FindField("Name"); if (findx >= 0) row.Value[findx] = this.Name;
            findx = row.Fields.FindField("CompliantICAO"); if (findx >= 0) row.Value[findx] = this.CompliantICAO.ToString();
            findx = row.Fields.FindField("Military"); if (findx >= 0) row.Value[findx] = this.Military.ToString();

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.Value[findx] = this.Geo;
            }


        }
    }
}
