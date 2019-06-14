using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;

namespace PDM
{
    public class RadioFrequencyArea:PDMObject
    {
        public CodeRadioFrequencyAreaType Type { get; set; }

        public double? AngleScallop { get; set; }

        public CodeRadioSignalType SignalType { get; set; }

        public string RadioCommChannel_ID { get; set; }

        public string AirportHeliport_ID { get; set; }

        public double? FrequencyReception { get; set; }
       
        public UOM_FREQ ReceptionFrequencyUOM { get; set; }
                
        public double? FrequencyTransmission { get; set; }
             
        public UOM_FREQ TransmissionFrequencyUOM { get; set; }

        public string ChannelName { get; set; }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.RadioFrequencyArea;
            }
        }
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
            findx = row.Fields.FindField("RadioCommChannel_ID"); if (findx >= 0) row.set_Value(findx, this.RadioCommChannel_ID);
            findx = row.Fields.FindField("AirportHeliport_ID"); if (findx >= 0) row.set_Value(findx, this.AirportHeliport_ID);
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("AngleScallop"); if (findx >= 0 && this.AngleScallop!=null) row.set_Value(findx, this.AngleScallop);
            findx = row.Fields.FindField("SignalType"); if (findx >= 0) row.set_Value(findx, this.SignalType.ToString());
            findx = row.Fields.FindField("FrequencyReception"); if (findx >= 0 && this.FrequencyReception.HasValue) row.set_Value(findx, this.FrequencyReception);
            findx = row.Fields.FindField("FrequencyReception_UOM"); if (findx >= 0) row.set_Value(findx, this.ReceptionFrequencyUOM.ToString());
            findx = row.Fields.FindField("FrequencyTransmission"); if (findx >= 0 && this.FrequencyTransmission.HasValue) row.set_Value(findx, this.FrequencyTransmission);
            findx = row.Fields.FindField("FrequencyTransmission_UOM"); if (findx >= 0) row.set_Value(findx, this.TransmissionFrequencyUOM.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);

            if (this.Geo != null)
            {

                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }

        }

        public override void RebuildGeo()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.Geo = ArnUtil.GetGeometry(this.ID, "RadioFrequencyArea", ArenaStatic.ArenaStaticProc.GetTargetDB());
        }
    }
}
