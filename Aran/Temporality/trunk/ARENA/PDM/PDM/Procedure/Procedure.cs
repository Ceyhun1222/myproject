using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;


namespace PDM
{
    [TypeConverter(typeof(PropertySorter))]
    public class Procedure : PDMObject
    {
        private string _AirportIdentifier;
        [Browsable(false)]
        public string AirportIdentifier
        {
            get { return _AirportIdentifier; }
            set { _AirportIdentifier = value; }
        }

        private string _Airport_ICAO_Code;
        [Description("Airport ICAO code")]
        [PropertyOrder(5)]
        public string Airport_ICAO_Code
        {
            get { return _Airport_ICAO_Code; }
            set { _Airport_ICAO_Code = value; }
        }



        private string _ProcedureIdentifier;
        [Description("Field contains the name of the procedure")]
        [PropertyOrder(10)]
        public string ProcedureIdentifier
        {
            get { return _ProcedureIdentifier; }
            set { _ProcedureIdentifier = value; }
        }

        private PROC_TYPE_code _ProcedureType;
        [Description("Procedure type (SID, STAR or IAP)")]
        [PropertyOrder(20)]
        public PROC_TYPE_code ProcedureType
        {
            get { return _ProcedureType; }
            set { _ProcedureType = value; }
        }

        private List<ProcedureTransitions> _Transitions;
        [Browsable(false)]
        public List<ProcedureTransitions> Transitions
        {
            get { return _Transitions; }
            set { _Transitions = value; }
        }

        private ProcedureCodingStandardType _codingStandard;
        [PropertyOrder(30)]
        public ProcedureCodingStandardType CodingStandard
        {
            get { return _codingStandard; }
            set { _codingStandard = value; }
        }

        private string _communicationFailureDescription;
        [PropertyOrder(40)]
        public string CommunicationFailureDescription
        {
            get { return _communicationFailureDescription; }
            set { _communicationFailureDescription = value; }
        }

        private string _description;
        [PropertyOrder(50)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private ProcedureDesignStandardType _designCriteria;
        [PropertyOrder(60)]
        public ProcedureDesignStandardType DesignCriteria
        {
            get { return _designCriteria; }
            set { _designCriteria = value; }
        }

        private bool _flightChecked;
        [PropertyOrder(70)]
        public bool FlightChecked
        {
            get { return _flightChecked; }
            set { _flightChecked = value; }
        }

        private bool _RNAV;
        [PropertyOrder(80)]
        public bool RNAV
        {
            get { return _RNAV; }
            set { _RNAV = value; }
        }

        private List<AircraftCharacteristic> _aircraftCharacteristic;
        [PropertyOrder(90)]
        public List<AircraftCharacteristic> AircraftCharacteristic
        {
            get { return _aircraftCharacteristic; }
            set { _aircraftCharacteristic = value; }
        }

        private PDMObject _landingArea;
        [PropertyOrder(100)]
        [ReadOnly(true)]
        public PDMObject LandingArea
        {
            get { return _landingArea; }
            set { _landingArea = value; }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }
        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }
        //[Browsable(false)]
        //public override double Elev_m
        //{
        //    get
        //    {
        //        return base.Elev_m;
        //    }
        //    set
        //    {
        //        base.Elev_m = value;
        //    }
        //}
        [Browsable(false)]
        public override double Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }
        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

        public Procedure()
        {
            //this.FeatureGUID = Guid.NewGuid().ToString();
        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                int findx = -1;
                ITable tbl = AIRTRACK_TableDic[typeof(Procedure)];
                IRow row = tbl.CreateRow();

                findx = row.Fields.FindField("AirportIdentifier"); if (findx >= 0) row.set_Value(findx, this.AirportIdentifier);
                findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
                findx = row.Fields.FindField("ProcedureIdentifier"); if (findx >= 0) row.set_Value(findx, this.ProcedureIdentifier);
                findx = row.Fields.FindField("ProcedureType"); if (findx >= 0) row.set_Value(findx, this.ProcedureType.ToString());
                findx = row.Fields.FindField("codingStandard"); if (findx >= 0) row.set_Value(findx, this.CodingStandard.ToString());
                findx = row.Fields.FindField("communicationFailureDescription"); if (findx >= 0) row.set_Value(findx, this.CommunicationFailureDescription);
                findx = row.Fields.FindField("description"); if (findx >= 0) row.set_Value(findx, this.Description);
                findx = row.Fields.FindField("designCriteria"); if (findx >= 0) row.set_Value(findx, this.DesignCriteria.ToString());
                findx = row.Fields.FindField("flightChecked"); if (findx >= 0) row.set_Value(findx, this.FlightChecked);
                findx = row.Fields.FindField("RNAV"); if (findx >= 0) row.set_Value(findx, this.RNAV);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

                findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

                row.Store();

                if (this.Transitions != null)
                {
                    foreach (ProcedureTransitions trans in this.Transitions)
                    {
                        trans.StoreToDB(AIRTRACK_TableDic);
                    }
                }



                #region iap

                if ((this.ProcedureType == PROC_TYPE_code.Approach) && (this is InstrumentApproachProcedure))
                {
                    InstrumentApproachProcedure iap = (InstrumentApproachProcedure)this;

                    tbl = AIRTRACK_TableDic[typeof(InstrumentApproachProcedure)];
                    row = tbl.CreateRow();

                    iap.CompileRow(ref row);
                }

                #endregion

                #region sid

                if ((this.ProcedureType == PROC_TYPE_code.SID) && (this is StandardInstrumentDeparture))
                {
                    StandardInstrumentDeparture sid = (StandardInstrumentDeparture)this;

                    tbl = AIRTRACK_TableDic[typeof(StandardInstrumentDeparture)];
                    row = tbl.CreateRow();

                    sid.CompileRow(ref row);
                }

                #endregion

                #region star

                if ((this.ProcedureType == PROC_TYPE_code.STAR) && (this is StandardInstrumentArrival))
                {
                    StandardInstrumentArrival star = (StandardInstrumentArrival)this;

                    tbl = AIRTRACK_TableDic[typeof(StandardInstrumentArrival)];
                    row = tbl.CreateRow();

                    star.CompileRow(ref row);
                }

                #endregion

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 
            {
                res = false;
            }

            return res;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
            List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

            if (this.Transitions != null)
            {
                foreach (ProcedureTransitions trans in this.Transitions)
                {
                    List<string> part = trans.HideBranch(AIRTRACK_TableDic, Visibility);
                    res.AddRange(part);

                }
            }

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Transitions != null)
            {
                foreach (ProcedureTransitions trans in this.Transitions)
                {
                    List<string> part = trans.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);

                }
            }

            return res;
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.Procedure.ToString();
            }
        } 

        public override string GetObjectLabel()
        {
            return this.ProcedureIdentifier;
        }

    }

  
}
