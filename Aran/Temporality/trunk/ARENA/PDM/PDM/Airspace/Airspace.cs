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

        private CodeStatusAirspaceType _activationStatus;
        [ReadOnly(true)]
        public CodeStatusAirspaceType ActivationStatus
        {
            get { return _activationStatus; }
            set { _activationStatus = value; }
        }

		private CodeDayBase _activationStartDay;
		[ReadOnly(true)]
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
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.Airspace.ToString();
            }
        } 

        public Airspace()
        {
            this.ActivationDescription = new List<string>();

        }




        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();


                if (this.AirspaceVolumeList != null)
                {

                    foreach (AirspaceVolume vol in this.AirspaceVolumeList)
                    {
                        vol.ID_Airspace = this.ID;
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
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

        }

        public override string GetObjectLabel()
        {
            return "AIRSPACE " + this.TxtName;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
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
    }
}
