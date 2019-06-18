﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;

namespace PDM
{
    [TypeConverter(typeof(PropertySorter))]
    public class Runway : PDMObject
    {
        private string _ID_AirportHeliport;
        [Description("Parent Airport")]
        [Browsable(false)]
        [PropertyOrder(10)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }


        private string _designator;
        [Description("The full textual designator of the runway, used to uniquely identify it at an aerodrome which has more than one.")]
        [Mandatory(true)]
        [PropertyOrder(20)]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private double _length;
        [Description("The value of the physical length of the runway.")]
        [Mandatory(true)]
        [PropertyOrder(30)]
        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private double _width;
        [Description("The value of the physical width of the runway.")]
        [Mandatory(true)]
        [PropertyOrder(40)]
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private UOM_DIST_HORZ _uom;
        [Description("The unit of measurement for the horizontal dimensions of the runway.")]
        [Mandatory(true)]
        [PropertyOrder(50)]
        public UOM_DIST_HORZ Uom
        {
            get { return _uom; }
            set { _uom = value; }
        }

        private string _codeComposition;
        [Description("A code indicating the composition of a runway surface.")]
        [PropertyOrder(60)]
        public string CodeComposition
        {
            get { return _codeComposition; }
            set { _codeComposition = value; }
        }

        private string _codeCondSfc;
        [Description("A code indicating the condition of a runway surface")]
        [PropertyOrder(70)]
        public string CodeCondSfc
        {
            get { return _codeCondSfc; }
            set { _codeCondSfc = value; }
        }

        private List<RunwayDirection> _RunwayDirectionList;
        [Browsable(false)]
        public List<RunwayDirection> RunwayDirectionList
        {
            get { return _RunwayDirectionList; }
            set { _RunwayDirectionList = value; }
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

        public Runway()
        {
            //throw new System.NotImplementedException();
        }

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.Runway.ToString();
            }
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

                if (this.RunwayDirectionList != null)
                {

                    foreach (RunwayDirection rdn in this.RunwayDirectionList)
                    {
                        rdn.ID_Runway = this.ID;
                        rdn.ID_AirportHeliport = this.ID_AirportHeliport;
                        rdn.StoreToDB(AIRTRACK_TableDic);

                    }
                }


            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}
            {
                res = false;
            }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("length"); if (findx >= 0) row.set_Value(findx, this.Length);
            findx = row.Fields.FindField("width"); if (findx >= 0) row.set_Value(findx, this.Width);
            findx = row.Fields.FindField("codeComposition"); if (findx >= 0) row.set_Value(findx, this.CodeComposition);
            findx = row.Fields.FindField("codeCondSfc"); if (findx >= 0) row.set_Value(findx, this.CodeCondSfc);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("uom"); if (findx >= 0) row.set_Value(findx, this.Uom.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }

       public override string GetObjectLabel()
        {
            return "RWY " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            ITable tbl = AIRTRACK_TableDic[this.GetType()];


                    if (this.RunwayDirectionList != null)
                    {
                        foreach (RunwayDirection rdn in this.RunwayDirectionList)
                        {
                            if (rdn.Related_NavaidSystem != null)
                            {
                                foreach (NavaidSystem ns in rdn.Related_NavaidSystem)
                                {
                                    ns.DeleteObject(AIRTRACK_TableDic);
                                }
                            }
                        }
                    }


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

            if (this.RunwayDirectionList != null)
            {

                foreach (RunwayDirection rdn in this.RunwayDirectionList)
                {
                   List<string> part = rdn.HideBranch(AIRTRACK_TableDic, Visibility);
                   res.AddRange(part);
                }
            }

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.RunwayDirectionList != null)
            {

                foreach (RunwayDirection rdn in this.RunwayDirectionList)
                {
                    List<string> part = rdn.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return this.Designator;
        }

    }
}