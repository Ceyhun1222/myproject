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
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class Apron : PDMObject
    {
        
       // [Browsable(false)]
        public string Name { get; set; }
        
        
        [Browsable(false)]
        public string ID_AirportHeliport { get; set; }
        
        
        [XmlElement]
       // [Browsable(false)]
        public List<ApronMarking> ApronMarkingList { get; set; }
        
       
        [XmlElement]
        //[Browsable(false)]
        public List<ApronElement> ApronElementList { get; set; }
        
        
        [XmlElement]
        //[Browsable(false)]
        public ApronLightSystem LightSystem { get; set; }
        
       
        [XmlElement]
        //[Browsable(false)]
        public List<DeicingArea> DeicingAreaList { get; set; }


        [XmlElement]
        //[Browsable(false)]
        public List<GuidanceLine> GuidanceLineList { get; set; }


        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.Apron;


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

                if (this.ApronMarkingList != null)
                {

                    foreach (ApronMarking prt in this.ApronMarkingList)
                    {
                        prt.ID_Apron = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.ApronElementList != null)
                {

                    foreach (ApronElement prt in this.ApronElementList)
                    {
                        prt.Apron_ID = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.LightSystem != null)
                {
                    this.LightSystem.Apron_ID = this.ID;
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
            findx = row.Fields.FindField("Name"); if (findx >= 0) row.set_Value(findx, this.Name);

            //findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate.ToString());
            
        }

        public override string GetObjectLabel()
        {
            return this.Name;
        }
    }
}
