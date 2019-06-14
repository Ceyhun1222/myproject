using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;

namespace PDM
{
     [Serializable()]
    public class Airspace : PDMObject
    {
        private string _codeID;
        [ReadOnly(true)]
        [Mandatory(true)]
		[Description("Designator")]
        public string CodeID
        {
            get { return _codeID; }
            set { _codeID = value; }
        }

        private string _txtName;
        [ReadOnly(true)]
        [Mandatory(true)]
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private AirspaceType _codeType;
        [Mandatory(true)]
        public AirspaceType CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private CodeStatusAirspaceType _activationStatus;
        [ReadOnly(true)]
        [Browsable(false)]
        public CodeStatusAirspaceType ActivationStatus
        {
            get { return _activationStatus; }
            set { _activationStatus = value; }
        }

        private List<AirspaceClass> _classAirspace;
        //[ReadOnly(true)]
        //[Browsable(false)]
        public List<AirspaceClass> ClassAirspace
        {
            get { return _classAirspace; }
            set { _classAirspace = value; }
        }


        private List<string> _class;
        [Browsable(false)]
        public List<string> Class { get => _class; set => _class = value; }


        private List<RadioCommunicationChanel> _communicationChanels;
        public List<RadioCommunicationChanel> CommunicationChanels
        {
            get { return _communicationChanels; }
            set { _communicationChanels = value; }
        }

        private string _LocalType;
        public string LocalType { get => _LocalType; set => _LocalType = value; }


        private CodeDayBase _activationStartDay;
		[ReadOnly(true)]
        [Browsable(false)]
        public CodeDayBase ActivationStartDay
		{
			get
			{
				return _activationStartDay;
			}
			set
			{
				_activationStartDay = value;
			}
		}

		private CodeDayBase _activationEndDay;
		[ReadOnly(true)]
        [Browsable(false)]
        public CodeDayBase ActivationEndDay
		{
			get
			{
				return _activationEndDay;
			}
			set
			{
				_activationEndDay = value;
			}
		}

		private string _activationStartTime;
		[ReadOnly(true)]
        [Browsable(false)]
        public string ActivationStartTime
		{
			get
			{
				return _activationStartTime;
			}
			set
			{
				_activationStartTime = value;
			}
		}

		private string _activationEndTime;
		[ReadOnly(true)]
        [Browsable(false)]
        public string ActivationEndTime
		{
			get
			{
				return _activationEndTime;
			}
			set
			{
				_activationEndTime = value;
			}
		}

		private DateTime _validStartDateTime;
		[ReadOnly(true)]
        [Browsable(false)]
        public DateTime ValidStartDateTime
		{
			get
			{
				return _validStartDateTime;
			}
			set
			{
				_validStartDateTime = value;
			}
		}

		private DateTime _validEndDateTime;
		[ReadOnly(true)]
        [Browsable(false)]
        public DateTime ValidEndDateTime
		{
			get
			{
				return _validEndDateTime;
			}
			set
			{
				_validEndDateTime = value;
			}
		}
		

        private List<string> _activationDescription;
        //[Browsable(false)]
        public List<string> ActivationDescription
        {
            get { return _activationDescription; }
            set { _activationDescription = value; }
        }

        private List<AirspaceVolume> _AirspaceVolumeList;
        [Browsable(false)]
        public List<AirspaceVolume> AirspaceVolumeList
        {
            get { return _AirspaceVolumeList; }
            set { _AirspaceVolumeList = value; }
        }

        private Enroute _protectedRoute;
        [Browsable(false)]
        public Enroute ProtectedRoute
        {
            get { return _protectedRoute; }
            set { _protectedRoute = value; }
        }

        [Browsable(false)]
        public string AirTrafficControlServiceName
        {
            get
            {
                //if (this.CommunicationChanels == null || this.CommunicationChanels.Count <= 0) return null;
                //RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                //                                     where (element != null) &&
                //                                         (((RadioCommunicationChanel)element).Rank == CodeFacilityRanking.PRIMARY)
                //                                     select element).FirstOrDefault();
                RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                                                     where (element != null) &&
                                                         (((RadioCommunicationChanel)element).Rank != CodeFacilityRanking.OTHER)
                                                     select element).FirstOrDefault();
                if (trChanel != null && trChanel.CallSign.Length > 0) return trChanel.CallSign;
                else return null;

            }
        }

        [Browsable(false)]
        public string RadioCommunicationFrequencyTransmission
        {
            get
            {
                //if (this.CommunicationChanels == null || this.CommunicationChanels.Count <= 0) return null;
                //RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                //                                     where (element != null) &&
                //                                         (((RadioCommunicationChanel)element).Rank == CodeFacilityRanking.PRIMARY)
                //                                     select element).FirstOrDefault();
                if (this.CommunicationChanels == null || this.CommunicationChanels.Count <= 0) return null;
                RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                                                     where (element != null) &&
                                                         (((RadioCommunicationChanel)element).Rank != CodeFacilityRanking.OTHER)
                                                     select element).FirstOrDefault();
                if (trChanel != null && trChanel.FrequencyTransmission.HasValue) return trChanel.FrequencyTransmission.Value.ToString();
                else return null;

            }
           
        }

        [Browsable(false)]
        public string RadioCommunicationFrequencyReception
        {
            get
            {
                //if (this.CommunicationChanels == null || this.CommunicationChanels.Count <= 0) return null;
                //RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                //                                     where (element != null) &&
                //                                         (((RadioCommunicationChanel)element).Rank == CodeFacilityRanking.PRIMARY)
                //                                     select element).FirstOrDefault();
                if (this.CommunicationChanels == null || this.CommunicationChanels.Count <= 0) return null;
                RadioCommunicationChanel trChanel = (from element in this.CommunicationChanels
                                                     where (element != null) &&
                                                         (((RadioCommunicationChanel)element).Rank != CodeFacilityRanking.OTHER)
                                                     select element).FirstOrDefault();
                if (trChanel != null && trChanel.FrequencyReception.HasValue) return trChanel.FrequencyReception.Value.ToString();
                else return null;

            }
        }

        //[Browsable(false)]
        private List<VolumeGeometryComponent> _VolumeGeometryComponents;
        public List<VolumeGeometryComponent> VolumeGeometryComponents { get => _VolumeGeometryComponents; set => _VolumeGeometryComponents = value; }


        [Browsable(false)]
        public override double? Elev
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

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.Airspace;
            }
        }


        public Airspace()
        {
            //this.ActivationDescription = new List<string>();

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


                if (this.AirspaceVolumeList != null)
                {

                    foreach (AirspaceVolume vol in this.AirspaceVolumeList)
                    {
                        vol.ID_Airspace = this.ID;
                        vol.TxtLocalType = this.LocalType; // debug version!!!!
                        vol.StoreToDB(AIRTRACK_TableDic);
                    }
                }

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}


            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("txtName"); if (findx >= 0) row.set_Value(findx, this.TxtName);
            findx = row.Fields.FindField("codeId"); if (findx >= 0) row.set_Value(findx, this.CodeID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("localType"); if (findx >= 0) row.set_Value(findx, this.LocalType);

        }

        public override string GetObjectLabel()
        {
            return "AIRSPACE " + this.TxtName;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return 0;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }

            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
             List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

             if (this.AirspaceVolumeList != null)
             {

                 foreach (AirspaceVolume vol in this.AirspaceVolumeList)
                 {
                   List<string> part =  vol.HideBranch(AIRTRACK_TableDic, Visibility);
                   res.AddRange(part);
                 }
             }

             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.AirspaceVolumeList != null)
            {

                foreach (AirspaceVolume vol in this.AirspaceVolumeList)
                {
                    List<string> part = vol.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.AirspaceVolumeList != null)
                foreach (var item in AirspaceVolumeList)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
        }

        public override List<string> GetIdList()
        {
            List<string> res = base.GetIdList();

            if (this.AirspaceVolumeList != null)
                foreach (var item in AirspaceVolumeList)
                {
                    res.Add(item.ID);
                }

            return res;
        }
    }

    [Serializable()]
    public class VolumeGeometryComponent
    {
        public VolumeGeometryComponent()
        { }

        public CodeAirspaceAggregation operation { get; set; }
        public int operationSequence { get; set; }
        public string theAirspace { get; set; }
        public string theAirspaceName { get; set; }
        public CodeAirspaceDependency airspaceDependency { get; set; }
    }

    [Serializable()]
    public class AirspaceClass
    {
        public AirspaceClass()
        { }

        public string Classification { get; set; }

        public List<AssociatedLevels> ClassAssociatedLevels { get; set; }

    }

    [Serializable()]
    public class AssociatedLevels
    {
        public AssociatedLevels()
        { }

        public UOM_DIST_VERT upperLimitUOM { get; set; }
        public CODE_DIST_VER upperLimitReference { get; set; }
        public double upperLimit { get; set; }


        public UOM_DIST_VERT lowerLimitUOM { get; set; }
        public CODE_DIST_VER lowerLimitReference { get; set; }
        public double lowerLimit { get; set; }
    }


}
