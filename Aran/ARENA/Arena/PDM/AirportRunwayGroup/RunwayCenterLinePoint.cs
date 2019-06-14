using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class RunwayCenterLinePoint : PDMObject
    {
        private CodeRunwayCenterLinePointRoleType _role;

        public CodeRunwayCenterLinePointRoleType Role
        {
            get { return _role; }
            set { _role = value; }
        }

        private string _designator;

        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string _ID_RunwayDirection;
        [Browsable(false)]
        public string ID_RunwayDirection
        {
            get { return _ID_RunwayDirection; }
            set { _ID_RunwayDirection = value; }
        }

        public CodeRVRDirectionType Direction { get; set; }

        private List<DeclaredDistance> _declDist;
        [Browsable(false)]
        public List<DeclaredDistance> DeclDist
        {
            get { return _declDist; }
            set { _declDist = value; }
        }

        private List<GuidanceLine> _GuidanceLineList;
        [XmlElement]
       // [Browsable(false)]
        public List<GuidanceLine> GuidanceLineList
        {
            get { return _GuidanceLineList; }
            set { _GuidanceLineList = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type => PDM_ENUM.RunwayCenterLinePoint;

        public RunwayCenterLinePoint()
        {
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID");
            if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_RunwayDirection");
            if (findx >= 0) row.set_Value(findx, this.ID_RunwayDirection);
            findx = row.Fields.FindField("Designator");
            if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("Role");
            if (findx >= 0) row.set_Value(findx, this.Role.ToString());
            findx = row.Fields.FindField("Direction"); if (findx >= 0) row.set_Value(findx, this.Direction.ToString());
            findx = row.Fields.FindField("Elevation");
            if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("Elev_UOM");
            if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            findx = row.Fields.FindField("GUND");
            if (findx >= 0 && this.GeoProperties.GeoidUndulation.HasValue) row.set_Value(findx, this.GeoProperties.GeoidUndulation);

            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LatCoord");
                if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord");
                if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());

                //findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_DDMMSS_NS());
                //findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_DDMMSS_EW_());

                findx = row.Fields.FindField("Shape");
                row.set_Value(findx, this.Geo);

            }

            if (this.DeclDist != null)
            {
                foreach (var declaredDistance in this.DeclDist)
                {
                    findx = row.Fields.FindField(declaredDistance.DistanceType.ToString());
                    if (findx >= 0)
                        row.Value[findx] = declaredDistance.DistanceValue;
                }

                findx = row.Fields.FindField("DeclaredDistance_UOM");
                if (findx >= 0)
                {
                    var firstOrDefault = this.DeclDist.FirstOrDefault();
                    if (firstOrDefault != null)
                        row.set_Value(findx, firstOrDefault.DistanceUOM.ToString());
                }
            }


        }

        public override string GetObjectLabel()
        {
            return this.Designator + ":" + this.Role.ToString();
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
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}

            return res;
        }


        public override string ToString()
        {
            return this.Designator + ":" + this.Role.ToString();
        }
  
    }
}
