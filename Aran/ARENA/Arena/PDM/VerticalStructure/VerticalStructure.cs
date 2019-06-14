using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Xml.Serialization;

namespace PDM
{
     [Serializable()]
    public class VerticalStructure: PDMObject
    {
        private string _name;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(300)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private VerticalStructureType _type;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(310)]
        public VerticalStructureType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _lighted;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(320)]
        public bool Lighted
        {
            get { return _lighted; }
            set { _lighted = value; }
        }

        private bool _markingICAOStandard;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(330)]
        public bool MarkingICAOStandard
        {
            get { return _markingICAOStandard; }
            set { _markingICAOStandard = value; }
        }

        private bool _group;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(340)]
        public bool Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private double? _length = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(350)]
        public double? Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private UOM_DIST_HORZ _length_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(360)]
        public UOM_DIST_HORZ Length_UOM
        {
            get { return _length_UOM; }
            set { _length_UOM = value; }
        }

        private double? _width = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(370)]
        public double? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private UOM_DIST_HORZ _width_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(380)]
        public UOM_DIST_HORZ Width_UOM
        {
            get { return _width_UOM; }
            set { _width_UOM = value; }
        }
        private double? _radius = null;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(390)]
        public double? Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
        private UOM_DIST_HORZ _radius_UOM;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(400)]
        public UOM_DIST_HORZ Radius_UOM
        {
            get { return _radius_UOM; }
            set { _radius_UOM = value; }
        }

        private bool _lightingICAOStandard;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(410)]
        public bool LightingICAOStandard
        {
            get { return _lightingICAOStandard; }
            set { _lightingICAOStandard = value; }
        }

        private bool _synchronisedLighting;
        //[Mandatory(true)]
        [XmlElement]
        [PropertyOrder(420)]
        public bool SynchronisedLighting
        {
            get { return _synchronisedLighting; }
            set { _synchronisedLighting = value; }
        }

        private bool _airportAssociated;
        [XmlElement]
        [PropertyOrder(430)]
        public bool AirportAssociated { get => _airportAssociated; set => _airportAssociated = value; }


        private List<VerticalStructurePart> _parts;
        [XmlElement]
        [Browsable(false)]
        public List<VerticalStructurePart> Parts
        {
            get { return _parts; }
            set { _parts = value; }
        }

        private List<CodeObstacleAreaType> _obstacleAreaType;
        [XmlElement]
        [PropertyOrder(420)]
         [ReadOnly(true)]
        public List<CodeObstacleAreaType> ObstacleAreaType
        {
            get { return _obstacleAreaType; }
            set { _obstacleAreaType = value; }
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
                return PDM_ENUM.VerticalStructure;
            }
        }

     
        public VerticalStructure()
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

                if (this.Parts != null)
                {

                    foreach (VerticalStructurePart prt in this.Parts)
                    {
                        prt.VerticalStructure_ID = this.ID;
                        prt.StoreToDB(AIRTRACK_TableDic);

                    }
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
            findx = row.Fields.FindField("Name"); if (findx >= 0) row.set_Value(findx, this.Name);
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("Lighted"); if (findx >= 0) row.set_Value(findx, this.Lighted);
            findx = row.Fields.FindField("MarkingICAOStandard"); if (findx >= 0) row.set_Value(findx, this.MarkingICAOStandard);
            findx = row.Fields.FindField("Group"); if (findx >= 0) row.set_Value(findx, this.Group);
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length);
            findx = row.Fields.FindField("Length_UOM"); if (findx >= 0) row.set_Value(findx, this.Length_UOM.ToString());
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width);
            findx = row.Fields.FindField("Width_UOM"); if (findx >= 0) row.set_Value(findx, this.Width_UOM.ToString());
            findx = row.Fields.FindField("Radius"); if (findx >= 0 && this.Radius.HasValue) row.set_Value(findx, this.Radius);
            findx = row.Fields.FindField("Radius_UOM"); if (findx >= 0) row.set_Value(findx, this.Radius_UOM.ToString());
            findx = row.Fields.FindField("LightingICAOStandard"); if (findx >= 0) row.set_Value(findx, this.LightingICAOStandard);
            findx = row.Fields.FindField("SynchronisedLighting"); if (findx >= 0) row.set_Value(findx, this.SynchronisedLighting);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("AirportAssociated"); if (findx >= 0) row.set_Value(findx, this.AirportAssociated);


            // findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }

        public override string GetObjectLabel()
        {
            return  this.Name + " VerticalStructure";
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

             if (this.Parts != null)
             {

                 foreach (VerticalStructurePart prt in this.Parts)
                 {
                     List<string> parts = prt.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(parts);
                 }
             }

             return res;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.Parts != null)
                foreach (var item in Parts)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
        }
    }
}
