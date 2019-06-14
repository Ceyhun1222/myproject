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
    public class TouchDownLiftOffSafeArea:AirportHeliportProtectionArea
    {
        public  string ID_TouchDownLiftOff { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.TouchDownLiftOffSafeArea;

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

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_TouchDownLiftOff"); if (findx >= 0) row.set_Value(findx, this.ID_TouchDownLiftOff);           
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width.Value);
            findx = row.Fields.FindField("Width_UOM"); if (findx >= 0) row.set_Value(findx, this.Width_UOM.ToString());
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length.Value);
            findx = row.Fields.FindField("Length_UOM"); if (findx >= 0) row.set_Value(findx, this.Length_UOM.ToString());
            findx = row.Fields.FindField("Lighting"); if (findx >= 0) row.set_Value(findx, this.Lighting.ToString());
            findx = row.Fields.FindField("ObstacleFree"); if (findx >= 0) row.set_Value(findx, this.ObstacleFree.ToString());



            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
            }
        }
    }
}
