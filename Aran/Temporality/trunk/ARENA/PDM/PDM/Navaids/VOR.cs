﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PDM
{
    public class VOR : NavaidComponent
    {
        //private string _ID_NavaidSystem;
        //[Browsable(false)]
        //public string ID_NavaidSystem
        //{
        //    get { return _ID_NavaidSystem; }
        //    set { _ID_NavaidSystem = value; }
        //}
        private double _frequency;

        public double Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
        private UOM_FREQ _frequency_UOM;

        public UOM_FREQ Frequency_UOM
        {
            get { return _frequency_UOM; }
            set { _frequency_UOM = value; }
        }
        private string _figureOfMerit;

        public string FigureOfMerit
        {
            get { return _figureOfMerit; }
            set { _figureOfMerit = value; }
        }
        private string _stationDeclination;

        public string StationDeclination
        {
            get { return _stationDeclination; }
            set { _stationDeclination = value; }
        }

        private string _vorIdentifier;

        public string VorIdentifier
        {
            get { return _vorIdentifier; }
            set { _vorIdentifier = value; }
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


        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.VOR.ToString();
            }
        } 

        public VOR()
        {
            
        }

        //public VOR(Object_AIRTRACK objAirtrack)
        //{
        //    new NavaidSystem(objAirtrack);

        //}

        public override bool StoreToDB(Dictionary<Type, ESRI.ArcGIS.Geodatabase.ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();

            }
            catch
            {
                res = false;
            }

            return res;

        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_NavaidSystem"); if (findx >= 0) row.set_Value(findx, this.ID_NavaidSystem);
            findx = row.Fields.FindField("frequency"); if (findx >= 0) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());
            findx = row.Fields.FindField("figureOfMerit"); if (findx >= 0) row.set_Value(findx, this.FigureOfMerit);
            findx = row.Fields.FindField("stationDeclination"); if (findx >= 0) row.set_Value(findx, this.StationDeclination);
            findx = row.Fields.FindField("Identifier"); if (findx >= 0) row.set_Value(findx, this.VorIdentifier);
            findx = row.Fields.FindField("ID_NavaidComponent"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);

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
            return this.GetType().Name.ToString();
        }

        public override string ToString()
        {
            return this.CodeId;
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
            qry.WhereClause = "ID_NavaidComponent = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);

            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }
    }
}
