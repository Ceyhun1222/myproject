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
using EsriWorkEnvironment;
using AranSupport;
using System.Drawing;
using System.Xml.Serialization;

namespace PDM
{
    //public struct AirspaceVolumeEnvelope
    //{
    //    public double LowerLeft_X;
    //    public double LowerLeft_Y;

    //    public double LowerRight_X;
    //    public double LowerRight_Y;

    //    public double TopLeft_X;
    //    public double TopLeft_Y;

    //    public double TopRight_X;
    //    public double TopRight_Y;

    //    public double Width;
    //    public double Height;
    //};

    [Serializable()]
    public class AirspaceVolume : PDMObject
    {
        

        private string _ID_Airspace;
        [Browsable(false)]
        public string ID_Airspace
        {
            get { return _ID_Airspace; }
            set { _ID_Airspace = value; }
        }

        private string _codeId;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(10)]
        [Mandatory(true)]
        public string CodeId
        {
            get { return _codeId; }
            set { _codeId = value; }
        }

        private string _txtName;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(20)]
        [Mandatory(true)]
        [DisplayName("Designator")]
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _codeClass;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(30)]
        [Mandatory(false)]
        //[Browsable(false)]
        public string CodeClass
        {
            get { return _codeClass; }
            set { _codeClass = value; }
        }

        private string _codeLocInd;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(40)]
        [Mandatory(false)]
        public string CodeLocInd
        {
            get { return _codeLocInd; }
            set { _codeLocInd = value; }
        }

        private string _codeActivity;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(50)]
        [Mandatory(false)]
        public string CodeActivity
        {
            get { return _codeActivity; }
            set { _codeActivity = value; }
        }

        private string _codeMil;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(60)]
        [Mandatory(false)]
        public string CodeMil
        {
            get { return _codeMil; }
            set { _codeMil = value; }
        }

        private AirspaceType _codeType;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(70)]
        [Mandatory(true)]
        public AirspaceType CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private string _airspaceCenterUID;
        [Browsable(false)]
        public string AirspaceCenterUID
        {
            get { return _airspaceCenterUID; }
            set { _airspaceCenterUID = value; }
        }

        private string _airspaceCenterType;
        [Browsable(false)]
        public string AirspaceCenterType
        {
            get { return _airspaceCenterType; }
            set { _airspaceCenterType = value; }
        }

        private string _txtLocalType;
        [Description("")]
        [Category("Desription")]
        [PropertyOrder(80)]
        [Mandatory(false)]
        [DisplayName("Name")]
        public string TxtLocalType
        {
            get { return _txtLocalType; }
            set { _txtLocalType = value; }
        }



        private CODE_DIST_VER _codeDistVerUpper;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(90)]
        [Mandatory(false)]
        [Browsable(false)]
        public CODE_DIST_VER CodeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }

        private double? _valDistVerUpper = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(100)]
        [Mandatory(false)]
        //[Browsable(false)]
        public double? ValDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }

        private CodeVerticalReference? _uperLimitReference = null;
        [Category("Restrictions")]
        public CodeVerticalReference? UperLimitReference
        {
            get { return _uperLimitReference; }
            set { _uperLimitReference = value; }
        }


        private double? _valDistVerUpper_M = null;
        [Browsable(false)]
        public double? ValDistVerUpper_M
        {
            get { return _valDistVerUpper_M; }
            set { _valDistVerUpper_M = value; }
        }

        private UOM_DIST_VERT _uomValDistVerUpper;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(110)]
        [Mandatory(false)]
        public UOM_DIST_VERT UomValDistVerUpper
        {
            get { return _uomValDistVerUpper; }
            set { _uomValDistVerUpper = value; }
        }

        private CODE_DIST_VER _codeDistVerLower;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(120)]
        [Mandatory(false)]
        [Browsable(false)]
        public CODE_DIST_VER CodeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }

        private double? _valDistVerLower = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(130)]
        [Mandatory(false)]
        //[Browsable(false)]
        public double? ValDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }

        private CodeVerticalReference? _lowerLimitReference = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(130)]
        [Mandatory(false)]
        public CodeVerticalReference? LowerLimitReference
        {
            get { return _lowerLimitReference; }
            set { _lowerLimitReference = value; }
        }

        private double? _valDistVerLower_M = null;
        [Browsable(false)]
        public double? ValDistVerLower_M
        {
            get { return _valDistVerLower_M; }
            set { _valDistVerLower_M = value; }
        }

        private UOM_DIST_VERT _uomValDistVerLower;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(140)]
        [Mandatory(false)]
        public UOM_DIST_VERT UomValDistVerLower
        {
            get { return _uomValDistVerLower; }
            set { _uomValDistVerLower = value; }
        }

        private CODE_DIST_VER _codeDistVerMax;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(150)]
        [Mandatory(false)]
        [Browsable(false)]
        public CODE_DIST_VER CodeDistVerMax
        {
            get { return _codeDistVerMax; }
            set { _codeDistVerMax = value; }
        }

        private double? _valDistVerMax = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(160)]
        [Mandatory(false)]
        [Browsable(false)]
        public double? ValDistVerMax
        {
            get { return _valDistVerMax; }
            set { _valDistVerMax = value; }
        }

        private double? _valDistVerMax_M = null;
        [Browsable(false)]
        public double? ValDistVerMax_M
        {
            get { return _valDistVerMax_M; }
            set { _valDistVerMax_M = value; }
        }

        private UOM_DIST_VERT _uomValDistVerMax;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(170)]
        [Mandatory(false)]
        public UOM_DIST_VERT UomValDistVerMax
        {
            get { return _uomValDistVerMax; }
            set { _uomValDistVerMax = value; }
        }

        private CODE_DIST_VER _codeDistVerMnm;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(180)]
        [Mandatory(false)]
        [Browsable(false)]
        public CODE_DIST_VER CodeDistVerMnm
        {
            get { return _codeDistVerMnm; }
            set { _codeDistVerMnm = value; }
        }

        private double? _valDistVerMnm = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(190)]
        [Mandatory(false)]
        [Browsable(false)]
        public double? ValDistVerMnm
        {
            get { return _valDistVerMnm; }
            set { _valDistVerMnm = value; }
        }

        private double? _valDistVerMnm_M = null;
        [Browsable(false)]
        public double? ValDistVerMnm_M
        {
            get { return _valDistVerMnm_M; }
            set { _valDistVerMnm_M = value; }
        }

        private UOM_DIST_VERT _uomValDistVerMnm;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(200)]
        [Mandatory(false)]
        public UOM_DIST_VERT UomValDistVerMnm
        {
            get { return _uomValDistVerMnm; }
            set { _uomValDistVerMnm = value; }
        }

        private double? _valLowerLimit = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(210)]
        [Mandatory(false)]
        public double? ValLowerLimit
        {
            get { return _valLowerLimit; }
            set { _valLowerLimit = value; }
        }

        private double? _valWidth = null;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(220)]
        [Mandatory(false)]
        public double? ValWidth
        {
            get { return _valWidth; }
            set { _valWidth = value; }
        }

        private UOM_DIST_HORZ _uomValWidth;
        [Description("")]
        [Category("Restrictions")]
        [PropertyOrder(230)]
        [Mandatory(false)]
        public UOM_DIST_HORZ UomValWidth
        {
            get { return _uomValWidth; }
            set { _uomValWidth = value; }
        }

        private PDMObject _centreline;
        [Browsable(false)]
        public PDMObject Centreline
        {
            get { return _centreline; }
            set { _centreline = value; }
        }


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
                return PDM_ENUM.AirspaceVolume;
            }
        }

        private byte[] _brdrGeometry;
        [Browsable(false)]

        public byte[] BrdrGeometry
        {
            get { return _brdrGeometry; }
            set { _brdrGeometry = value; }
        }

   
        [Browsable(false)]
        public override DateTime ActualDate
        {
            get
            {
                return base.ActualDate;
            }
            set
            {
                base.ActualDate = value;
            }
        }

        
        //public AirspaceVolumeEnvelope GetVolumeEnvelope()
        //{
        //    AirspaceVolumeEnvelope res = new AirspaceVolumeEnvelope { LowerLeft_X =0};

        //    if (this.Geo == null) this.RebuildGeo();
        //    if (this.Geo !=null)
        //    {
        //        res.LowerLeft_X = (float) ((IPolygon)this.Geo).Envelope.LowerLeft.X;
        //        res.LowerLeft_Y = (float) ((IPolygon)this.Geo).Envelope.LowerLeft.Y;

        //        res.LowerRight_X = (float)((IPolygon)this.Geo).Envelope.LowerRight.X;
        //        res.LowerRight_Y = (float)((IPolygon)this.Geo).Envelope.LowerRight.Y;

        //        res.TopLeft_X = (float)((IPolygon)this.Geo).Envelope.UpperLeft.X;
        //        res.TopLeft_Y = (float)((IPolygon)this.Geo).Envelope.UpperLeft.Y;

        //        res.TopRight_X = (float)((IPolygon)this.Geo).Envelope.UpperRight.X;
        //        res.TopRight_Y = (float)((IPolygon)this.Geo).Envelope.UpperRight.Y;

        //        res.Width = (float)((IPolygon)this.Geo).Envelope.LowerRight.X - (float)((IPolygon)this.Geo).Envelope.LowerLeft.X;
        //        if (res.Width < 0) res.Width = res.Width * -1;

        //        res.Height = (float)((IPolygon)this.Geo).Envelope.UpperRight.Y - (float)((IPolygon)this.Geo).Envelope.LowerRight.Y;
        //        if (res.Height < 0) res.Height = res.Height * -1;

        //    }

        //    return res;
        //}


        //private AirspaceVolumeEnvelope _volumeEnvilope;
        //[Browsable(false)]
        //public AirspaceVolumeEnvelope VolumeEnvilope { get => GetVolumeEnvelope();}



        public AirspaceVolume()
        {
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

        public override void CompileRow(ref ESRI.ArcGIS.Geodatabase.IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);

            findx = row.Fields.FindField("codeId"); if (findx >= 0) row.set_Value(findx, this.CodeId);
            findx = row.Fields.FindField("txtName"); if (findx >= 0) row.set_Value(findx, this.TxtName);
            findx = row.Fields.FindField("codeClass"); if (findx >= 0) row.set_Value(findx, this.CodeClass);
            findx = row.Fields.FindField("codeLocInd"); if (findx >= 0) row.set_Value(findx, this.CodeLocInd);
            findx = row.Fields.FindField("codeActivity"); if (findx >= 0) row.set_Value(findx, this.CodeActivity);
            findx = row.Fields.FindField("codeMil"); if (findx >= 0) row.set_Value(findx, this.CodeMil);
            findx = row.Fields.FindField("codeDistVerUpper"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerUpper.ToString());
            findx = row.Fields.FindField("valDistVerUpper"); if (findx >= 0 && this.ValDistVerUpper.HasValue) row.set_Value(findx, this.ValDistVerUpper);
            findx = row.Fields.FindField("uomDistVerUpper"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerUpper.ToString());
            findx = row.Fields.FindField("codeDistVerLower"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerLower.ToString());
            findx = row.Fields.FindField("valDistVerLower"); if (findx >= 0 && this.ValDistVerLower.HasValue) row.set_Value(findx, this.ValDistVerLower);
            findx = row.Fields.FindField("uomDistVerLower"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerLower.ToString());
            findx = row.Fields.FindField("codeDistVerMax"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerMax.ToString());
            findx = row.Fields.FindField("valDistVerMax"); if (findx >= 0 && this.ValDistVerMax.HasValue) row.set_Value(findx, this.ValDistVerMax);
            findx = row.Fields.FindField("uomDistVerMax"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerMax);
            findx = row.Fields.FindField("codeDistVerMnm"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerMnm.ToString());
            findx = row.Fields.FindField("valDistVerMnm"); if (findx >= 0 && this.ValDistVerMnm.HasValue) row.set_Value(findx, this.ValDistVerMnm);
            findx = row.Fields.FindField("uomDistVerMnm"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerMnm.ToString());
            findx = row.Fields.FindField("valLowerLimit"); if (findx >= 0 && this.ValLowerLimit.HasValue) row.set_Value(findx, this.ValLowerLimit);
            findx = row.Fields.FindField("codeType"); if (findx >= 0) row.set_Value(findx, this.CodeType.ToString());
            findx = row.Fields.FindField("airspaceCenterUID"); if (findx >= 0) row.set_Value(findx, this.AirspaceCenterUID);
            findx = row.Fields.FindField("airspaceCenterType"); if (findx >= 0) row.set_Value(findx, this.AirspaceCenterType);
            findx = row.Fields.FindField("txtLocalType"); if (findx >= 0) row.set_Value(findx, this.TxtLocalType);
            findx = row.Fields.FindField("AirspaceID"); if (findx >= 0) row.set_Value(findx, this.ID_Airspace);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("uniqVolumID"); if (findx >= 0) row.set_Value(findx, this.ID);


            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null))
            {
                row.set_Value(findx, this.Geo);
                //findx = row.Fields.FindField("uniqVolumID"); if (findx >= 0) row.set_Value(findx, this.ID_Airspace+ Math.Round((this.Geo as IArea).Area,8).ToString());
            }
        }

        public override void RebuildGeo()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.Geo = ArnUtil.GetGeometry(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB());
           
        }

        public void RebuildGeo2(bool RecoverBrdrGeometry = false)
        {
            if (this.Geo == null && this.BrdrGeometry != null)
            {
                this.Geo = (IGeometry)HelperClass.GetObjectFromBlob(this.BrdrGeometry, "Border");
            }

            if (this.Geo == null)
            {
                this.RebuildGeo();
            }



            
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

        public override string GetObjectLabel()
        {
            return "AIRSPACE Vol. " + this.CodeId;
        }

    }
}
