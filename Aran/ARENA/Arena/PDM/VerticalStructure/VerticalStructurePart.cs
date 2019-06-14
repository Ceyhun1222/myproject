using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;
using System.ComponentModel;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml.Serialization;

namespace PDM
{
    [Serializable()]
    public class VerticalStructurePart : PDMObject
    {
        private string _designator;
        [Mandatory(true)]
        [XmlElement]
        [PropertyOrder(300)]
        //[Description("Type of the navaid service.")]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private double? _height = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(310)]
        //[Description("Type of the navaid service.")]
        public double? Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private UOM_DIST_VERT _height_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(320)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT Height_UOM
        {
            get { return _height_UOM; }
            set { _height_UOM = value; }
        }

        private StatusConstructionType _constructionStatus;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(340)]
        //[Description("Type of the navaid service.")]
        public StatusConstructionType ConstructionStatus
        {
            get { return _constructionStatus; }
            set { _constructionStatus = value; }
        }

        private bool _frangible;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(350)]
        //[Description("Type of the navaid service.")]
        public bool Frangible
        {
            get { return _frangible; }
            set { _frangible = value; }
        }

        private ColourType _markingFirstColour;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(360)]
        //[Description("Type of the navaid service.")]
        public ColourType MarkingFirstColour
        {
            get { return _markingFirstColour; }
            set { _markingFirstColour = value; }
        }

        private VerticalStructureMarkingType _markingPattern;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(370)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureMarkingType MarkingPattern
        {
            get { return _markingPattern; }
            set { _markingPattern = value; }
        }

        private ColourType _markingSecondColour;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(380)]
        //[Description("Type of the navaid service.")]
        public ColourType MarkingSecondColour
        {
            get { return _markingSecondColour; }
            set { _markingSecondColour = value; }
        }

        private bool _mobile;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(390)]
        //[Description("Type of the navaid service.")]
        public bool Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        private VerticalStructureType _type;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(400)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private double? _verticalExtent = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(410)]
        //[Description("Type of the navaid service.")]
        public double? VerticalExtent
        {
            get { return _verticalExtent; }
            set { _verticalExtent = value; }
        }

        private UOM_DIST_VERT _verticalExtent_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(420)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT VerticalExtent_UOM
        {
            get { return _verticalExtent_UOM; }
            set { _verticalExtent_UOM = value; }
        }

        private double? _verticalExtentAccuracy = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(430)]
        //[Description("Type of the navaid service.")]
        public double? VerticalExtentAccuracy
        {
            get { return _verticalExtentAccuracy; }
            set { _verticalExtentAccuracy = value; }
        }

        private UOM_DIST_VERT _verticalExtentAccuracy_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(440)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_VERT VerticalExtentAccuracy_UOM
        {
            get { return _verticalExtentAccuracy_UOM; }
            set { _verticalExtentAccuracy_UOM = value; }
        }

        private VerticalStructureMaterialType _visibleMaterial;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(450)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureMaterialType VisibleMaterial
        {
            get { return _visibleMaterial; }
            set { _visibleMaterial = value; }
        }

        private string _VerticalStructure_ID;
        [XmlElement]
        [Browsable(false)]
        public string VerticalStructure_ID
        {
            get { return _VerticalStructure_ID; }
            set { _VerticalStructure_ID = value; }
        }

        private byte[] _vertex;
        [XmlElement]
        [Browsable(false)]
        public byte[] Vertex
        {
            get { return _vertex; }
            set { _vertex = value; }
        }

        private string _vertexType;
        [XmlElement]
        [Browsable(false)]
        public string VertexType { get => _vertexType; set => _vertexType = value; }

        private VerticalStructureGeoType _vsGeotype;

        [XmlElement]
        [Browsable(false)]
        public VerticalStructureGeoType VSGeoType
        {
            get { return _vsGeotype; }
            set { _vsGeotype = value; }
        }


        [XmlElement]
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

        //[Browsable(false)]
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

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.VerticalStructurePart;
            }
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


        public VerticalStructurePart()
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

                int findx = -1;

                if (this.Geo != null)
                {
                    

                    switch (this.Geo.GeometryType)
                    {
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint):

                            ITable tblGeoPnt = AIRTRACK_TableDic[typeof(PointClass)];
                            IRow rowGeoPnt = tblGeoPnt.CreateRow();
                            findx = -1;
                            findx = rowGeoPnt.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.ID);
                            findx = rowGeoPnt.Fields.FindField("partID"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.ID);
                            findx = rowGeoPnt.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoPnt.set_Value(findx, this.Geo);
                            findx = rowGeoPnt.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) rowGeoPnt.set_Value(findx, this.Elev);
                            findx = rowGeoPnt.Fields.FindField("Elev_UOM"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.Elev_UOM.ToString());

                            rowGeoPnt.Store();

                        break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline):
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine):

                        ITable tblGeoLn = AIRTRACK_TableDic[typeof(LineClass)];
                            IRow rowGeoLn = tblGeoLn.CreateRow();
                            findx = -1;
                            findx = rowGeoLn.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoLn.set_Value(findx, this.ID);
                            findx = rowGeoLn.Fields.FindField("partID"); if (findx >= 0) rowGeoLn.set_Value(findx, this.ID);
                            findx = rowGeoLn.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoLn.set_Value(findx, this.Geo);
                            findx = rowGeoLn.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) rowGeoLn.set_Value(findx, this.Elev);
                            findx = rowGeoLn.Fields.FindField("Elev_UOM"); if (findx >= 0) rowGeoLn.set_Value(findx, this.Elev_UOM.ToString());

                            rowGeoLn.Store();

                            break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon):

                            ITable tblGeoPlg = AIRTRACK_TableDic[typeof(PolygonClass)];
                            IRow rowGeoPlg = tblGeoPlg.CreateRow();
                            findx = -1;
                            findx = rowGeoPlg.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.ID);
                            findx = rowGeoPlg.Fields.FindField("partID"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.ID);
                            findx = rowGeoPlg.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoPlg.set_Value(findx, this.Geo);
                            findx = rowGeoPlg.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) rowGeoPlg.set_Value(findx, this.Elev);
                            findx = rowGeoPlg.Fields.FindField("Elev_UOM"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.Elev_UOM.ToString());

                            rowGeoPlg.Store();

                            break;
                    }

                }

            }
             catch(Exception ex) 
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
            findx = row.Fields.FindField("VerticalStructure_ID"); if (findx >= 0) row.set_Value(findx, this.VerticalStructure_ID);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ConstructionStatus"); if (findx >= 0) row.set_Value(findx, this.ConstructionStatus.ToString());
            findx = row.Fields.FindField("Frangible"); if (findx >= 0) row.set_Value(findx, this.Frangible);
            findx = row.Fields.FindField("MarkingFirstColour"); if (findx >= 0) row.set_Value(findx, this.MarkingFirstColour.ToString());
            findx = row.Fields.FindField("MarkingPattern"); if (findx >= 0) row.set_Value(findx, this.MarkingPattern.ToString());
            findx = row.Fields.FindField("MarkingSecondColour"); if (findx >= 0) row.set_Value(findx, this.MarkingSecondColour.ToString());
            findx = row.Fields.FindField("Mobile"); if (findx >= 0) row.set_Value(findx, this.Mobile);
            findx = row.Fields.FindField("VerticalStructureType"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("VerticalExtent"); if (findx >= 0 && this.VerticalExtent.HasValue) row.set_Value(findx, this.VerticalExtent);
            findx = row.Fields.FindField("VerticalExtent_UOM"); if (findx >= 0) row.set_Value(findx, this.VerticalExtent_UOM.ToString());
            findx = row.Fields.FindField("VerticalExtentAccuracy"); if (findx >= 0 && this.VerticalExtentAccuracy.HasValue) row.set_Value(findx, this.VerticalExtentAccuracy);
            findx = row.Fields.FindField("VerticalExtentAccuracy_UOM"); if (findx >= 0) row.set_Value(findx, this.VerticalExtentAccuracy_UOM.ToString());
            findx = row.Fields.FindField("VisibleMaterial"); if (findx >= 0) row.set_Value(findx, this.VisibleMaterial.ToString());
            findx = row.Fields.FindField("Elev"); if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("Height"); if (findx >= 0 && this.Height.HasValue) row.set_Value(findx, this.Height);
            findx = row.Fields.FindField("Height_UOM"); if (findx >= 0) row.set_Value(findx, this.Height_UOM.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
            

            

        }

        public override string GetObjectLabel()
        {
            return this.Designator;
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

        public override void RebuildGeo()
        {

            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            string objtp = "VerticalStructure_Point";

            if (this.VertexType != null && this.VertexType.Length > 0)
            {
                switch (this.VertexType)
                {
                    case ("esriGeometryPoint"):
                        objtp = "VerticalStructure_Point";
                        break;
                    case ("esriGeometryPolygon"):
                        objtp = "VerticalStructure_Surface";
                        break;
                    case ("esriGeometryPolyline"):
                    case ("esriGeometryLine"):
                        objtp = "VerticalStructure_Curve";
                        break;
                    default:
                        break;
                }

                this.Geo = ArnUtil.GetGeometry(this.ID, objtp, ArenaStatic.ArenaStaticProc.GetTargetDB());
            }

            else
            {
                this.Geo = ArnUtil.GetGeometry(this.ID, objtp, ArenaStatic.ArenaStaticProc.GetTargetDB());

                if (this.Geo == null) objtp = "VerticalStructure_Surface";
                this.Geo = ArnUtil.GetGeometry(this.ID, objtp, ArenaStatic.ArenaStaticProc.GetTargetDB());
                if (this.Geo == null) objtp = "VerticalStructure_Curve";
                this.Geo = ArnUtil.GetGeometry(this.ID, objtp, ArenaStatic.ArenaStaticProc.GetTargetDB());
            }
        }

        public override int GetIDfromDB()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            return ArnUtil.GetIDfromDB(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB(),this.Geo.GeometryType);

        }

        public override string ToString()
        {
            return this.Designator;
        }
    }
}
