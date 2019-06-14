using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.ComponentModel;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;

namespace PDM
{
    [Serializable()]
    public class RouteSegment : PDMObject
    {
        private string _ID_Route;
        [Browsable(false)]
        public string ID_Route
        {
            get { return _ID_Route; }
            set { _ID_Route = value; }
        }

        private RouteSegmentPoint _StartPoint;
        [PropertyOrder(10)]
        [ReadOnly(true)]
        public RouteSegmentPoint StartPoint
        {
            get { return _StartPoint; }
            set { _StartPoint = value; }
        }

        private RouteSegmentPoint _EndPoint;
        [ReadOnly(true)]
        [PropertyOrder(20)]
        public RouteSegmentPoint EndPoint
        {
            get { return _EndPoint; }
            set { _EndPoint = value; }
        }

        private string _codeType;
        [PropertyOrder(30)]
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private CODE_ROUTE_SEGMENT_CODE_LVL _codeLvl;
        [PropertyOrder(40)]
        public CODE_ROUTE_SEGMENT_CODE_LVL CodeLvl
        {
            get { return _codeLvl; }
            set { _codeLvl = value; }
        }

        private CODE_ROUTE_SEGMENT_CODE_INTL _codeIntl;
        [PropertyOrder(50)]
        public CODE_ROUTE_SEGMENT_CODE_INTL CodeIntl
        {
            get { return _codeIntl; }
            set { _codeIntl = value; }
        }

        private CODE_ROUTE_SEGMENT_DIR _CodeDir;
        [PropertyOrder(60)]
        public CODE_ROUTE_SEGMENT_DIR CodeDir
        {
            get { return _CodeDir; }
            set { _CodeDir = value; }
        }

        private CodeRouteNavigation _navigationType;
        [PropertyOrder(65)]
        public CodeRouteNavigation NavigationType
        {
            get { return _navigationType; }
            set { _navigationType = value; }
        }

        private double? _valDistVerUpper;
        [PropertyOrder(70)]
        public double? ValDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }

        private UOM_DIST_VERT _uomValDistVerUpper;
        [PropertyOrder(80)]
        public UOM_DIST_VERT UomValDistVerUpper
        {
            get { return _uomValDistVerUpper; }
            set { _uomValDistVerUpper = value; }
        }

        private CODE_DIST_VER _codeDistVerUpper;
        [PropertyOrder(90)]
        public CODE_DIST_VER CodeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }

        private double? _valDistVerLower;
        [PropertyOrder(100)]
        public double? ValDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }

        private UOM_DIST_VERT _uomValDistVerLower;
        [PropertyOrder(110)]
        public UOM_DIST_VERT UomValDistVerLower
        {
            get { return _uomValDistVerLower; }
            set { _uomValDistVerLower = value; }
        }

        private CODE_DIST_VER _codeDistVerLower;
        public CODE_DIST_VER CodeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }

        private double? _valDistVerMnm;
        [PropertyOrder(120)]
        public double? ValDistVerMnm
        {
            get { return _valDistVerMnm; }
            set { _valDistVerMnm = value; }
        }

        private UOM_DIST_VERT _uomValDistVerMnm;
        [PropertyOrder(130)]
        public UOM_DIST_VERT UomValDistVerMnm
        {
            get { return _uomValDistVerMnm; }
            set { _uomValDistVerMnm = value; }
        }

        private CODE_DIST_VER _codeDistVerMnm;
        [PropertyOrder(140)]
        public CODE_DIST_VER CodeDistVerMnm
        {
            get { return _codeDistVerMnm; }
            set { _codeDistVerMnm = value; }
        }

        private double? _valDistVerLowerOvrde;
        [PropertyOrder(150)]
        public double? ValDistVerLowerOvrde
        {
            get { return _valDistVerLowerOvrde; }
            set { _valDistVerLowerOvrde = value; }
        }

        private UOM_DIST_VERT _uomValDistVerLowerOvrde;
        [PropertyOrder(160)]
        public UOM_DIST_VERT UomValDistVerLowerOvrde
        {
            get { return _uomValDistVerLowerOvrde; }
            set { _uomValDistVerLowerOvrde = value; }
        }

        private CODE_DIST_VER _codeDistVerLowerOvrde;
        [PropertyOrder(170)]
        public CODE_DIST_VER CodeDistVerLowerOvrde
        {
            get { return _codeDistVerLowerOvrde; }
            set { _codeDistVerLowerOvrde = value; }
        }

        private double? _valWidLeft;
        [PropertyOrder(180)]
        public double? ValWidLeft
        {
            get { return _valWidLeft; }
            set { _valWidLeft = value; }
        }

        private double? _valWidRight;
        [PropertyOrder(185)]
        public double? ValWidRight
        {
            get { return _valWidRight; }
            set { _valWidRight = value; }
        }

        private UOM_DIST_HORZ _uomValWid;
        [PropertyOrder(190)]
        public UOM_DIST_HORZ UomValWid
        {
            get { return _uomValWid; }
            set { _uomValWid = value; }
        }

        private double? _valTrueTrack;
        [PropertyOrder(200)]
        public double? ValTrueTrack
        {
            get { return _valTrueTrack; }
            set { _valTrueTrack = value; }
        }

        private double? _valMagTrack;
        [PropertyOrder(210)]
        public double? ValMagTrack
        {
            get { return _valMagTrack; }
            set { _valMagTrack = value; }
        }

        private double? _valReversTrueTrack = null;
        [PropertyOrder(220)]
        public double? ValReversTrueTrack
        {
            get { return _valReversTrueTrack; }
            set { _valReversTrueTrack = value; }
        }

        private double? _valReversMagTrack = null;
        [PropertyOrder(230)]
        public double? ValReversMagTrack
        {
            get { return _valReversMagTrack; }
            set { _valReversMagTrack = value; }
        }

        private double? _valLen;
        [PropertyOrder(240)]
        public double? ValLen
        {
            get { return _valLen; }
            set { _valLen = value; }
        }

        private UOM_DIST_HORZ _uomValLen;
        [PropertyOrder(250)]
        public UOM_DIST_HORZ UomValLen
        {
            get { return _uomValLen; }
            set { _uomValLen = value; }
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

		private byte[] _legBlobGeometry;
		[Browsable(false)]
		public byte[] LegBlobGeometry
		{
			get { return _legBlobGeometry; }
			set { _legBlobGeometry = value; }
		}

        //[Browsable(false)]
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

        //[Browsable(false)]
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

        private string _RouteFormed;
        [Browsable(false)]
        public string RouteFormed
        {
            get { return _RouteFormed; }
            set { _RouteFormed = value; }
        }


        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.RouteSegment;
            }
        } 

        public RouteSegment()
        {
        }

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {

                //if (this.ID.CompareTo("d25e03eb-2967-4f6e-b46e-54316ab66499") == 0)
                //    System.Diagnostics.Debug.WriteLine("");

                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();

                if (this.StartPoint != null)
                {
                    this.StartPoint.StoreToDB(AIRTRACK_TableDic);
                }

                if (this.EndPoint != null)
                {
                    this.EndPoint.StoreToDB(AIRTRACK_TableDic);
                }


            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 



            return res;
        }

        public override List<string> GetExceptionMessage()
        {
            List<string> res = base.GetExceptionMessage();

            if (this.StartPoint != null)
            {
                res.AddRange( this.StartPoint.GetExceptionMessage());
            }

            if (this.EndPoint != null)
            {
                res.AddRange(this.EndPoint.GetExceptionMessage());
            }

            return res;
        }

        public override void CompileRow(ref ESRI.ArcGIS.Geodatabase.IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("navigationType"); if (findx >= 0) row.set_Value(findx, this.NavigationType.ToString());

            findx = row.Fields.FindField("designator"); 
               if (findx >= 0)
               {
                   if (this.StartPoint != null && this.EndPoint != null) row.set_Value(findx, this.StartPoint.SegmentPointDesignator + " : " + this.EndPoint.SegmentPointDesignator);
                   else row.set_Value(findx, "NONE");
               }

            findx = row.Fields.FindField("ID_Enroute"); if (findx >= 0) row.set_Value(findx, this.ID_Route);
            findx = row.Fields.FindField("RouteFormed"); if (findx >= 0) row.set_Value(findx,this.RouteFormed);

            findx = row.Fields.FindField("StartWayPointDesignator"); if (findx >= 0 && this.StartPoint!=null) row.set_Value(findx, this.StartPoint.SegmentPointDesignator);
            findx = row.Fields.FindField("EndWayPointDesignator"); if (findx >= 0 && this.EndPoint!=null) row.set_Value(findx, this.EndPoint.SegmentPointDesignator);

            //findx = row.Fields.FindField("codeRnp"); if (findx >= 0) row.set_Value(findx, this.c);
            findx = row.Fields.FindField("codeLvl"); if (findx >= 0) row.set_Value(findx, this.CodeLvl.ToString());
            findx = row.Fields.FindField("codeIntl"); if (findx >= 0) row.set_Value(findx, this.CodeIntl.ToString());
            findx = row.Fields.FindField("codeDir"); if (findx >= 0) row.set_Value(findx, this.CodeDir.ToString());
            //findx = row.Fields.FindField("codeRouteAvailability"); if (findx >= 0) row.set_Value(findx, this.TxtDesig);
            findx = row.Fields.FindField("valDistVerUpper"); if (findx >= 0) row.set_Value(findx, this.ValDistVerUpper);
            findx = row.Fields.FindField("uomDistVerUpper"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerUpper.ToString());
            findx = row.Fields.FindField("codeDistVerUpper"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerUpper.ToString());
            findx = row.Fields.FindField("valDistVerLower"); if (findx >= 0) row.set_Value(findx, this.ValDistVerLower);
            findx = row.Fields.FindField("uomDistVerLower"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerLower.ToString());
            findx = row.Fields.FindField("codeDistVerLower"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerLower.ToString());
            findx = row.Fields.FindField("valDistVerMnm"); if (findx >= 0) row.set_Value(findx, this.ValDistVerMnm);
            findx = row.Fields.FindField("uomDistVerMnm"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerMnm.ToString());
            findx = row.Fields.FindField("codeDistVerMnm"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerMnm.ToString());
            findx = row.Fields.FindField("valDistVerLowerOvrde"); if (findx >= 0) row.set_Value(findx, this.ValDistVerLowerOvrde);
            findx = row.Fields.FindField("uomDistVerLowerOvrde"); if (findx >= 0) row.set_Value(findx, this.UomValDistVerLowerOvrde.ToString());
            findx = row.Fields.FindField("codeDistVerLowerOvrde"); if (findx >= 0) row.set_Value(findx, this.CodeDistVerLowerOvrde.ToString());
            findx = row.Fields.FindField("ValWidLeft"); if (findx >= 0) row.set_Value(findx, this.ValWidLeft);
            findx = row.Fields.FindField("ValWidRight"); if (findx >= 0) row.set_Value(findx, this.ValWidRight);
            findx = row.Fields.FindField("uomWid"); if (findx >= 0) row.set_Value(findx, this.UomValWid.ToString());
            findx = row.Fields.FindField("valTrueTrack"); if (findx >= 0) row.set_Value(findx, this.ValTrueTrack);
            findx = row.Fields.FindField("valMagTrack"); if (findx >= 0) row.set_Value(findx, this.ValMagTrack);
            findx = row.Fields.FindField("valReversTrueTrack"); if (findx >= 0) row.set_Value(findx, this.ValReversTrueTrack);
            findx = row.Fields.FindField("valReversMagTrack"); if (findx >= 0) row.set_Value(findx, this.ValReversMagTrack);
            findx = row.Fields.FindField("valLen"); if (findx >= 0) row.set_Value(findx, this.ValLen);
            findx = row.Fields.FindField("uomValLen"); if (findx >= 0) row.set_Value(findx, this.UomValLen.ToString());
            findx = row.Fields.FindField("StartSegmentPointID"); if ((findx >= 0) && (this.StartPoint !=null)) row.set_Value(findx, this.StartPoint.ID);
            findx = row.Fields.FindField("EndSegmentPointID"); if ((findx >= 0) && (this.EndPoint != null)) row.set_Value(findx, this.EndPoint.ID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());

                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }
        }


        public override string GetObjectLabel()
        {
            string StartText = this.StartPoint != null ? this.StartPoint.SegmentPointDesignator : "NONE";
            string EndText = this.EndPoint != null ? this.EndPoint.SegmentPointDesignator : "NONE";
            return StartText + " : " + EndText;
        }

        public void RebuildGeo2()
        {
            

            if ((this.StartPoint != null) && (this.EndPoint != null))
            {
                if (this.StartPoint.Geo == null) this.StartPoint.RebuildGeo();
                if (this.EndPoint.Geo == null) this.EndPoint.RebuildGeo();

                IPolyline ln = new PolylineClass();
                ln.FromPoint = this.StartPoint.Geo as IPoint;
                ln.ToPoint = this.EndPoint.Geo as IPoint;

                IZAware zAware = ln as IZAware;
                zAware.ZAware = true;

                IMAware mAware = ln as IMAware;
                mAware.MAware = true;
                this.Geo = ln;
            }
            else
            {
                AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
                this.Geo = ArnUtil.GetGeometry(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB());
            }
        }

        public override void RebuildGeo()
		{
            if (this.LegBlobGeometry == null) return;

            byte[] bytes = (byte[])this.LegBlobGeometry;

            // сконвертируем его в геометрию 
            IMemoryBlobStream memBlobStream = new MemoryBlobStream();

			IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

			varBlobStream.ImportFromVariant(bytes);

			IObjectStream anObjectStream = new ObjectStreamClass();
			anObjectStream.Stream = memBlobStream;

			IPropertySet aPropSet = new PropertySetClass();

			IPersistStream aPersistStream = (IPersistStream)aPropSet;
			aPersistStream.Load(anObjectStream);

			this.Geo = aPropSet.GetProperty("Leg") as IGeometry;
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

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.StartPoint != null)
            {

                List<string> part = this.StartPoint.GetBranch(AIRTRACK_TableDic);
                res.AddRange(part);
                
            }

            if (this.EndPoint != null)
            {

                List<string> part = this.EndPoint.GetBranch(AIRTRACK_TableDic);
                res.AddRange(part);

            }
            return res;
        }

        public override string ToString()
        {
            if ((this.StartPoint != null) && (this.EndPoint != null))
                return this.StartPoint.SegmentPointDesignator + " : " + this.EndPoint.SegmentPointDesignator;
            else 
                return base.ToString();
        }

        public string EndStartString()
        {
            if ((this.StartPoint != null) && (this.EndPoint != null))
                return this.EndPoint.SegmentPointDesignator + " : " + this.StartPoint.SegmentPointDesignator;
            else
                return "";
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.StartPoint != null) res = res || this.StartPoint.CompareId(AnotherID);
            if (this.EndPoint != null) res = res || this.EndPoint.CompareId(AnotherID);

            return res;
        }

    }

}
