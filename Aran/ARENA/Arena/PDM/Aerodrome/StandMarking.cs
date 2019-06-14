using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;
using System.ComponentModel;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml.Serialization;

namespace PDM
{
    public class StandMarking:Marking
    {
        [Description("Parent AircraftStand")]
        [Browsable(false)]
        [PropertyOrder(10)]
        public string ID_AircraftStand { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.StandMarking;

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

                if (this.MarkingElementList != null)
                {

                    foreach (MarkingElement markingElem in this.MarkingElementList)
                    {
                        markingElem.Marking_ID = this.ID;
                        markingElem.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
           
            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AircraftStand"); if (findx >= 0) row.set_Value(findx, this.ID_AircraftStand);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);            

            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);

            //findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }
    }
}
