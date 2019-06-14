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
    public class RadioCommunicationChanel : PDMObject
    {
   
         private string _chanelType;
        public string ChanelType
        {
            get { return _chanelType; }
            set { _chanelType = value; }
        }

        public string Name { get; set; }

        public string ID_AirportHeliport { get; set; }

        private double? _frequencyReception;
        public double? FrequencyReception
        {
            get { return _frequencyReception; }
            set { _frequencyReception = value; }
        }

        private UOM_FREQ _receptionFrequencyUOM;
        public UOM_FREQ ReceptionFrequencyUOM
        {
            get { return _receptionFrequencyUOM; }
            set { _receptionFrequencyUOM = value; }
        }

        private double? _frequencyTransmission;
        public double? FrequencyTransmission
        {
            get { return _frequencyTransmission; }
            set { _frequencyTransmission = value; }
        }

        private UOM_FREQ _transmissionFrequencyUOM;
        public UOM_FREQ TransmissionFrequencyUOM
        {
            get { return _transmissionFrequencyUOM; }
            set { _transmissionFrequencyUOM = value; }
        }

        private CodeFacilityRanking _rank;
        public CodeFacilityRanking Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }
       
        public CodeCommunicationModeType Mode { get; set; }

        public string Logon { get; set; }

        public CodeRadioEmissionType EmissionType { get; set; }

        public CodeCommunicationDirectionType TrafficDirection { get; set; }

        private string _callSign;
        public string CallSign
        {
            get { return _callSign; }
            set { _callSign = value; }
        }

        [Browsable(false)]
        public override string Lat { get => base.Lat; set => base.Lat = value; }

        [Browsable(false)]
        public override string Lon { get => base.Lon; set => base.Lon = value; }

        public RadioCommunicationChanel()
        {
        }

        public override string ToString()
        {
            if (CallSign == null) CallSign = "";
            if (ChanelType == null) ChanelType = "";

            if (FrequencyReception.HasValue && FrequencyTransmission.HasValue)
                return ChanelType + " " + FrequencyReception.Value.ToString() + "/" + FrequencyTransmission.Value.ToString()+ "(" + CallSign + ")" + Rank.ToString();
            else
                return ChanelType + "(" + CallSign + ")";

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
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
            

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;
          
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Mode"); if (findx >= 0) row.set_Value(findx, this.Mode.ToString());
            findx = row.Fields.FindField("Rank"); if (findx >= 0) row.set_Value(findx, this.Rank.ToString());
            findx = row.Fields.FindField("FrequencyReception"); if (findx >= 0 && this.FrequencyReception.HasValue) row.set_Value(findx, this.FrequencyReception);
            findx = row.Fields.FindField("FrequencyReception_UOM"); if (findx >= 0) row.set_Value(findx, this.ReceptionFrequencyUOM.ToString());
            findx = row.Fields.FindField("FrequencyTransmission"); if (findx >= 0 && this.FrequencyTransmission.HasValue) row.set_Value(findx, this.FrequencyTransmission);
            findx = row.Fields.FindField("FrequencyTransmission_UOM"); if (findx >= 0) row.set_Value(findx, this.TransmissionFrequencyUOM.ToString());
            findx = row.Fields.FindField("EmissionType"); if (findx >= 0) row.set_Value(findx, this.EmissionType.ToString());
            findx = row.Fields.FindField("TrafficDirection"); if (findx >= 0) row.set_Value(findx, this.TrafficDirection.ToString());            
            findx = row.Fields.FindField("CallSign"); if (findx >= 0) row.set_Value(findx, this.CallSign);
            findx = row.Fields.FindField("ChanelType"); if (findx >= 0) row.set_Value(findx, this.ChanelType);
            findx = row.Fields.FindField("Logon"); if (findx >= 0) row.set_Value(findx, this.Logon);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("Name"); if (findx >= 0) row.set_Value(findx, this.Name);

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }


    }
}
