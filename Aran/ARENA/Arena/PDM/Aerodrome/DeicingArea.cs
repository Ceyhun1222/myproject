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
    public class DeicingArea:PDMObject
    {
        [XmlIgnore]
        public Dictionary<string, string> ParentList = new Dictionary<string, string>();

        [XmlElement]
        //[Browsable(false)]
        public List<DeicingAreaMarking> DeicingAreaMarkingList { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.DeicingArea;


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

                if (this.DeicingAreaMarkingList != null)
                {

                    foreach (DeicingAreaMarking prt in this.DeicingAreaMarkingList)
                    {
                        prt.DeicingArea_ID = this.ID;
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

            if (this.ParentList != null)
            {
                foreach (var parent in this.ParentList)
                {
                    findx = row.Fields.FindField(parent.Key.ToString());
                    if (findx >= 0)
                        row.Value[findx] = parent.Value;
                }

            }

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
            }
        }
    }
}
