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
    public class TaxiHoldingPosition:PDMObject
    {
        public CodeHoldingCategoryType LandingCategory { get; set; }

        public CodeStatusOperationsType Status { get; set; }

        public string ID_GuidanceLine { get; set; }

        public string ID_Runway { get; set; }

        public string RwyDesignator { get; set; }

        [XmlElement]
        //[Browsable(false)]
        public TaxiHoldingPositionLightSystem LightSystem { get; set; }

        [XmlElement]
        //[Browsable(false)]
        public List<TaxiHoldingPositionMarking> TaxiHoldingMarkingList { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.TaxiHoldingPosition;

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

                if (this.TaxiHoldingMarkingList != null)
                {

                    foreach (TaxiHoldingPositionMarking prt in this.TaxiHoldingMarkingList)
                    {
                        prt.ID_TaxiHolding = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.LightSystem != null)
                {
                    this.LightSystem.ID_TaxiHolding = this.ID;
                    this.LightSystem.StoreToDB(AIRTRACK_TableDic);
                }


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

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Runway"); if (findx >= 0) row.set_Value(findx, this.ID_Runway);
            findx = row.Fields.FindField("RwyDesignator"); if (findx >= 0) row.set_Value(findx, this.RwyDesignator);
            findx = row.Fields.FindField("ID_GuidanceLine"); if (findx >= 0) row.set_Value(findx, this.ID_GuidanceLine);            
            findx = row.Fields.FindField("LandingCategory"); if (findx >= 0) row.set_Value(findx, this.LandingCategory.ToString());            
            findx = row.Fields.FindField("Status"); if (findx >= 0) row.set_Value(findx, this.Status.ToString());
            


            if (this.Geo != null)
            {
                
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }

        }
    }
}
