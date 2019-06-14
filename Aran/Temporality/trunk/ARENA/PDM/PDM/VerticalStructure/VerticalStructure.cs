using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using System.ComponentModel;
using PDM.PropertyExtension;

namespace PDM
{
    public class VerticalStructure: PDMObject
    {
        private string _name;
        //[Mandatory(true)]
        [PropertyOrder(300)]
        //[Description("Type of the navaid service.")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private VerticalStructureType _type;
        //[Mandatory(true)]
        [PropertyOrder(310)]
        //[Description("Type of the navaid service.")]
        public VerticalStructureType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _lighted;
        //[Mandatory(true)]
        [PropertyOrder(320)]
        //[Description("Type of the navaid service.")]
        public bool Lighted
        {
            get { return _lighted; }
            set { _lighted = value; }
        }

        private bool _markingICAOStandard;
        //[Mandatory(true)]
        [PropertyOrder(330)]
        //[Description("Type of the navaid service.")]
        public bool MarkingICAOStandard
        {
            get { return _markingICAOStandard; }
            set { _markingICAOStandard = value; }
        }

        private bool _group;
        //[Mandatory(true)]
        [PropertyOrder(340)]
        //[Description("Type of the navaid service.")]
        public bool Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private double _length;
        //[Mandatory(true)]
        [PropertyOrder(350)]
        //[Description("Type of the navaid service.")]
        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private UOM_DIST_HORZ _length_UOM;
        //[Mandatory(true)]
        [PropertyOrder(360)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Length_UOM
        {
            get { return _length_UOM; }
            set { _length_UOM = value; }
        }

        private double _width;
        //[Mandatory(true)]
        [PropertyOrder(370)]
        //[Description("Type of the navaid service.")]
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private UOM_DIST_HORZ _width_UOM;
        //[Mandatory(true)]
        [PropertyOrder(380)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Width_UOM
        {
            get { return _width_UOM; }
            set { _width_UOM = value; }
        }

        private double _radius;
        //[Mandatory(true)]
        [PropertyOrder(390)]
        //[Description("Type of the navaid service.")]
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        private UOM_DIST_HORZ _radius_UOM;
        //[Mandatory(true)]
        [PropertyOrder(400)]
        //[Description("Type of the navaid service.")]
        public UOM_DIST_HORZ Radius_UOM
        {
            get { return _radius_UOM; }
            set { _radius_UOM = value; }
        }

        private bool _lightingICAOStandard;
        //[Mandatory(true)]
        [PropertyOrder(410)]
        //[Description("Type of the navaid service.")]
        public bool LightingICAOStandard
        {
            get { return _lightingICAOStandard; }
            set { _lightingICAOStandard = value; }
        }

        private bool _synchronisedLighting;
        //[Mandatory(true)]
        [PropertyOrder(420)]
        //[Description("Type of the navaid service.")]
        public bool SynchronisedLighting
        {
            get { return _synchronisedLighting; }
            set { _synchronisedLighting = value; }
        }

        private List<VerticalStructurePart> _parts;
        [Browsable(false)]
        public List<VerticalStructurePart> Parts
        {
            get { return _parts; }
            set { _parts = value; }
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

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.VerticalStructure.ToString();
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
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
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
            findx = row.Fields.FindField("Length"); if (findx >= 0) row.set_Value(findx, this.Length);
            findx = row.Fields.FindField("Length_UOM"); if (findx >= 0) row.set_Value(findx, this.Length_UOM.ToString());
            findx = row.Fields.FindField("Width"); if (findx >= 0) row.set_Value(findx, this.Width);
            findx = row.Fields.FindField("Width_UOM"); if (findx >= 0) row.set_Value(findx, this.Width_UOM.ToString());
            findx = row.Fields.FindField("Radius"); if (findx >= 0) row.set_Value(findx, this.Radius);
            findx = row.Fields.FindField("Radius_UOM"); if (findx >= 0) row.set_Value(findx, this.Radius_UOM.ToString());
            findx = row.Fields.FindField("LightingICAOStandard"); if (findx >= 0) row.set_Value(findx, this.LightingICAOStandard);
            findx = row.Fields.FindField("SynchronisedLighting"); if (findx >= 0) row.set_Value(findx, this.SynchronisedLighting);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);


           // findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }
    
        public override string GetObjectLabel()
        {
            return "VerticalStructure  " + this.Name;
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
    }
}
