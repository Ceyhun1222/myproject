using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.Omega.Enums;
using Aran.Omega.Extensions;
using Aran.Omega.Models;
using Aran.PANDA.Constants;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.Export
{
    public class ExportToGdb
    {
        private  ISpatialReference _spGeo;
        private readonly List<DrawingSurface> _surfaceList;
        private IWorkspace _workspace;
        private IFeatureWorkspace _featureWorkspace;
        private const int DomainRadius = 3000000;
        private ILogger _logger = GlobalParams.Logger;

        public ExportToGdb(List<DrawingSurface> surfaceList)
        {
            _surfaceList = surfaceList;
            SetDomainParametrs();
        }

        public ExportToGdb(DrawingSurface surface)
        {
            if (surface == null)
                throw new ArgumentNullException();

            _surfaceList = new List<DrawingSurface> {surface};
            SetDomainParametrs();
        }

        private void SetDomainParametrs()
        {
            var spatRefConverter = new SpatRefConverter();
            _spGeo = spatRefConverter.ToEsriSpatRef(GlobalParams.AranEnvironment.Graphics.WGS84SR);
        }

        private IFeatureWorkspace CreateFeatureWorkspace(string folderPath, ExportType exportType)
        {
            IWorkspaceFactory workspaceFactory;

            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (exportType== ExportType.Geodatabase)
                workspaceFactory = new FileGDBWorkspaceFactory();
            else
                workspaceFactory = new AccessWorkspaceFactoryClass();

            var wn = workspaceFactory.Create(folderPath, "OmegaExport", null, 0);
            return (wn as IName)?.Open() as IFeatureWorkspace;
        }

        public void ExportAll(string folderPath, ExportType exportType,string surfaceName="")
        {
            _featureWorkspace = CreateFeatureWorkspace(folderPath, exportType);
            _workspace = _featureWorkspace as IWorkspace;

            var obsAreaClass =CreateObstacleAreaFeatClass(surfaceName);

            var obsPoinClass = CreateObstacleFeatClass(surfaceName+"Obstacle_point", ObstacleGeomType.Point);
            var obsPolygonClass = CreateObstacleFeatClass(surfaceName+"Obstacle_polygon",ObstacleGeomType.Polygon);
            var obsPolylineClass = CreateObstacleFeatClass(surfaceName+"Obstacle_polyline",ObstacleGeomType.PolyLine);

            var vsPoinClass = CreateVsFeatClass(surfaceName + "VerticalStructure_point", ObstacleGeomType.Point);
            var vsPolygonClass = CreateVsFeatClass(surfaceName + "VerticalStructure_polygon", ObstacleGeomType.Polygon);
            var vsPolylineClass = CreateVsFeatClass(surfaceName + "VerticalStructure_polyline", ObstacleGeomType.PolyLine);
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            try
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                foreach (var surface in _surfaceList)
                {
                    WriteObstacleAreaToGdb(surface, obsAreaClass.CreateFeature());
                    WriteObstacleToGdb(surface, obsPoinClass, obsPolylineClass, obsPolygonClass);
                }
                WriteVsToGdb(GlobalParams.AdhpObstacleList,vsPoinClass,vsPolylineClass,vsPolygonClass);
            }
            finally
            {
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }

        }

        public IFeatureClass CreateObstacleAreaFeatClass(string surfaceName)
        {
            var nameOfShapeFile =surfaceName+"ObstacleArea";

            var shapeFieldName = "Shape";
            try
            {
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
                // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

                IField field = new Field(); ///###########
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Shape";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

                IGeometryDef geomDef = new GeometryDef(); ///#########
                IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
                geomDefEdit.HasZ_2 = true;

                geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryMultiPatch;
                geomDefEdit.SpatialReference_2 = _spGeo;

                fieldEdit.GeometryDef_2 = geomDef;
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit= (IFieldEdit)field;
                fieldEdit.Name_2 = "ownerrunway";
                fieldEdit.AliasName_2 = "Used runway direction";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "timeslice_interpretation";
                fieldEdit.AliasName_2 = "Time slice interpretation";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "identifier";
                fieldEdit.AliasName_2 = "UUID identifier";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGUID);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "type";
                fieldEdit.AliasName_2 = "Type";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "ownerairport";
                fieldEdit.AliasName_2 = "Owner airport";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimebegin";
                fieldEdit.AliasName_2 = "Validtime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimebegin";
                fieldEdit.AliasName_2 = "Lifetime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimeend";
                fieldEdit.AliasName_2 = "Lifetime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimeend";
                fieldEdit.AliasName_2 = "Validtime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "usedrunway";
                fieldEdit.AliasName_2 = "Used runway";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "gen_data";
                fieldEdit.AliasName_2 = "gen_data";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "User_name";
                fieldEdit.AliasName_2 = "Operator name";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "axis_meridian";
                fieldEdit.AliasName_2 = "Axis meridian";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "length_inner_edge";
                fieldEdit.AliasName_2 = "Length of inner edge";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "divergence";
                fieldEdit.AliasName_2 = "Divergence (each side)";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "slope";
                fieldEdit.AliasName_2 = "Slope";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "length";
                fieldEdit.AliasName_2 = "Length";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Height";
                fieldEdit.AliasName_2 = "Height";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "radius";
                fieldEdit.AliasName_2 = "Radius";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Width";
                fieldEdit.AliasName_2 = "Width";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "surface_slope";
                fieldEdit.AliasName_2 = "Surface slope";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "final_width";
                fieldEdit.AliasName_2 = "Final width";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "distance_rwy_end";
                fieldEdit.AliasName_2 = "Distance from runway end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(fieldEdit);

                //Add another miscellaneous text field

                var clsId = new UIDClass();
                clsId.Value = "esriGeoDatabase.Feature";
                IFeatureClass featureClass = _featureWorkspace?.CreateFeatureClass(nameOfShapeFile, fields, clsId, null,
                    esriFeatureType.esriFTSimple, shapeFieldName, null);

                return featureClass;

            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw;
            }
        }

        public IFeatureClass CreateObstacleFeatClass(string name,ObstacleGeomType geomType)
        {
            var nameOfShapeFile = name;

            var shapeFieldName = "Shape";
            try
            {
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
                // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

                IField field = new Field(); ///###########
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Shape";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

                IGeometryDef geomDef = new GeometryDef(); ///#########
                IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
                geomDefEdit.HasZ_2 = true;

                geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                if (geomType== ObstacleGeomType.Polygon)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                else if (geomType== ObstacleGeomType.PolyLine)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

                geomDefEdit.SpatialReference_2 = _spGeo;

                fieldEdit.GeometryDef_2 = geomDef;
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "timeslice_interpretation";
                fieldEdit.AliasName_2 = "Time slice interpretation";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "elevation_val";
                fieldEdit.AliasName_2 = "Elevation";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "markingicaostandard";
                fieldEdit.AliasName_2 = "Marking ICAO standard";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "vert_struct_identifier";
                fieldEdit.AliasName_2 = "Vertical structure identifier";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "ownerrunway";
                fieldEdit.AliasName_2 = "Used runway";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalextent_val";
                fieldEdit.AliasName_2 = "Vertical extent";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "horizontalaccuracy_uom";
                fieldEdit.AliasName_2 = "Horizontal accuracy UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "frangible";
                fieldEdit.AliasName_2 = "Frangible";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "vert_struct_type";
                fieldEdit.AliasName_2 = "Vertical structure type";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lighted";
                fieldEdit.AliasName_2 = "Lighted";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalaccuracy_val";
                fieldEdit.AliasName_2 = "Vertical accuracy";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "is_group";
                fieldEdit.AliasName_2 = "Is group";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "obst_area_type";
                fieldEdit.AliasName_2 = "Obstacle area type";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalaccuracy_uom";
                fieldEdit.AliasName_2 = "Vertical accuracy UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "horizontalaccuracy_val";
                fieldEdit.AliasName_2 = "Horizontal accuracy";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "usedrunway";
                fieldEdit.AliasName_2 = "Used runway direction";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "ownerairport";
                fieldEdit.AliasName_2 = "Owner airport";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticaldatum";
                fieldEdit.AliasName_2 = "Vertical datum";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "vert_struct_designator";
                fieldEdit.AliasName_2 = "Vertical structure designator";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "elevation_uom";
                fieldEdit.AliasName_2 = "Elevation UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "vert_struct_name";
                fieldEdit.AliasName_2 = "Vertical structure name";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalextent_uom";
                fieldEdit.AliasName_2 = "Vertical extent UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "mobile";
                fieldEdit.AliasName_2 = "Mobile";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "gen_data";
                fieldEdit.AliasName_2 = "Creation date";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "user_name";
                fieldEdit.AliasName_2 = "Operator name";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "penetrate";
                fieldEdit.AliasName_2 = "Penetration";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);



                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "penetrate_uom";
                fieldEdit.AliasName_2 = "Penetration UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "constructionstatus";
                fieldEdit.AliasName_2 = "Construction status";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);




                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimebegin";
                fieldEdit.AliasName_2 = "Validtime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimebegin";
                fieldEdit.AliasName_2 = "Lifetime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimeend";
                fieldEdit.AliasName_2 = "Lifetime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimeend";
                fieldEdit.AliasName_2 = "Validtime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                //Add another miscellaneous text field

                var clsId = new UIDClass();
                clsId.Value = "esriGeoDatabase.Feature";
                IFeatureClass featureClass = _featureWorkspace?.CreateFeatureClass(nameOfShapeFile, fields, clsId, null,
                    esriFeatureType.esriFTSimple, shapeFieldName, null);

                return featureClass;

            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw;
            }
        }

        public IFeatureClass CreateVsFeatClass(string name, ObstacleGeomType geomType)
        {
            var nameOfShapeFile = name;

            var shapeFieldName = "Shape";
            try
            {
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                IField field = new Field();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Shape";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

                IGeometryDef geomDef = new GeometryDef();
                IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
                geomDefEdit.HasZ_2 = true;

                geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                if (geomType == ObstacleGeomType.Point)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                else if (geomType == ObstacleGeomType.PolyLine)
                    geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

                geomDefEdit.SpatialReference_2 = _spGeo;

                fieldEdit.GeometryDef_2 = geomDef;
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "designator";
                fieldEdit.AliasName_2 = "Designator";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "constructionstatus";
                fieldEdit.AliasName_2 = "Construction status";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "timeslice_interpretation";
                fieldEdit.AliasName_2 = "Time slice interpretation";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "elevation_val";
                fieldEdit.AliasName_2 = "Elevation";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "elevation_uom";
                fieldEdit.AliasName_2 = "Elevation UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "frangible";
                fieldEdit.AliasName_2 = "Frangible";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "horizontalaccuracy_uom";
                fieldEdit.AliasName_2 = "Horizontal accuracy UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "horizontalaccuracy_val";
                fieldEdit.AliasName_2 = "Horizontal accuracy";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "identifier";
                fieldEdit.AliasName_2 = "UUID identifier";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "is_group";
                fieldEdit.AliasName_2 = "Is group";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lighted";
                fieldEdit.AliasName_2 = "Lighted";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "markingicaostandard";
                fieldEdit.AliasName_2 = "Marking ICAO standard";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "mobile";
                fieldEdit.AliasName_2 = "Mobile";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "name";
                fieldEdit.AliasName_2 = "Name";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "type";
                fieldEdit.AliasName_2 = "Type";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);


                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalaccuracy_val";
                fieldEdit.AliasName_2 = "Vertical accuracy";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalaccuracy_uom";
                fieldEdit.AliasName_2 = "Vertical accuracy UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticaldatum";
                fieldEdit.AliasName_2 = "Vertical datum";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalextent_uom";
                fieldEdit.AliasName_2 = "Vertical extent UOM";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "verticalextent_val";
                fieldEdit.AliasName_2 = "Vertical extent";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "gen_data";
                fieldEdit.AliasName_2 = "Creation date";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldEdit.IsNullable_2 = true;
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "user_name";
                fieldEdit.AliasName_2 = "Operator name";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "used_for_oa";
                fieldEdit.AliasName_2 = "Surface name (preliminary assessment)";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimebegin";
                fieldEdit.AliasName_2 = "Validtime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimebegin";
                fieldEdit.AliasName_2 = "Lifetime begin";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "lifetimeend";
                fieldEdit.AliasName_2 = "Lifetime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);

                field = new Field();
                fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "validtimeend";
                fieldEdit.AliasName_2 = "Validtime end";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDate);
                fieldsEdit.AddField(field);


                //Add another miscellaneous text field

                var clsId = new UIDClass();
                clsId.Value = "esriGeoDatabase.Feature";
                IFeatureClass featureClass = _featureWorkspace?.CreateFeatureClass(nameOfShapeFile, fields, clsId, null,
                    esriFeatureType.esriFTSimple, shapeFieldName, null);

                return featureClass;

            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw;
            }
        }

        private void WriteObstacleAreaToGdb(DrawingSurface surface, IFeature feat)
        {
            try
            {
                var surfaceGeoAsMultiPatch= surface.SurfaceBase.GetGeomAsMultiPatch();

                if (surfaceGeoAsMultiPatch == null)
                {
                    GlobalParams.AranEnvironment.GetLogger("Omega").Info(surface.ViewCaption+" geometry is not found!");
                    return;
                }

                surfaceGeoAsMultiPatch.SpatialReference = _spGeo;
                feat.Shape = surfaceGeoAsMultiPatch;
                feat.Value[1] = surface.RwyDirClass?.Name;
                feat.Value[2] = surface.RwyDirClass?.RwyDir.TimeSlice.Interpretation.ToString();

                if (surface.SurfaceBase.ObsArea != null)
                {
                    UID uid = new UID();
                    uid.Value = "{" + surface.SurfaceBase.ObsArea?.Identifier.ToString() + "}";
                    feat.Value[3] = uid;

                    feat.Value[6] = surface.SurfaceBase.ObsArea.TimeSlice.ValidTime.BeginPosition;
                    feat.Value[7] = surface.SurfaceBase.ObsArea.TimeSlice.FeatureLifetime.BeginPosition;
                    if (surface.SurfaceBase.ObsArea.TimeSlice.FeatureLifetime?.EndPosition!=null)
                        feat.Value[8] = surface.SurfaceBase.ObsArea.TimeSlice.FeatureLifetime.EndPosition;
                    if (surface.SurfaceBase.ObsArea.TimeSlice.ValidTime?.EndPosition!=null)
                        feat.Value[9] = surface.SurfaceBase.ObsArea.TimeSlice.ValidTime.EndPosition;
                }

                feat.Value[4] = surface.ViewCaption;
                feat.Value[5] = GlobalParams.Database.AirportHeliport.Name;
              
                var usedRunway =
                    GlobalParams.Database.Runways.FirstOrDefault(
                        rwy => rwy.Identifier == surface.RwyDirClass.RwyDir.UsedRunway.Identifier);
                feat.Value[10] = usedRunway?.Designator;
                feat.Value[11] = DateTime.Now;
                //gen data

                //
                feat.Value[13] = GlobalParams.SpatialRefOperation.SpRefPrj.ParamList
                    .FirstOrDefault(param => param.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                    ?.Value;

                foreach (var prop in surface.SurfaceBase.PropertyList)
                {
                    var fieldNumber = feat.Fields.FindField(prop.Name);
                    if (fieldNumber == -1)
                        fieldNumber =feat.Fields.FindFieldByAliasName(prop.Name);
                    if (fieldNumber>-1)
                        feat.Value[fieldNumber] = prop.Value;

                }

                feat.Store();
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw new Exception("Error exporting " + surface.ViewCaption + " surface");
            }
        }

        public void WriteObstacleToGdb(DrawingSurface surface, IFeatureClass ptFeatClass,
            IFeatureClass polyLineFeatClass, IFeatureClass polygonFeatureClass)
        {
            try
            {
                var penetratedReport = surface.SurfaceBase.GetReport.
                                        Where(obs => obs.Penetrate > 0);

                foreach (var obsReport in penetratedReport)
                {
                    var vstructure = obsReport.Obstacle;
                    //Geometry obs = GlobalParams.SpatialRefOperation.ToGeo(obsReport.GeomPrj);

                    Geometry obs = obsReport.Geo;
                    var feat =ExportHelper.CreateFeature(obs.Type,ptFeatClass,polyLineFeatClass,polygonFeatureClass);

                    if (feat == null) throw new ArgumentException(obsReport.Name + " geometry type is not correct!");

                    SetZToGeo(obs,obsReport.Elevation);

                    var esriGeo = Converters.ConvertToEsriGeom.FromGeometry(obs, true);
                    esriGeo.SpatialReference = _spGeo;
                    try
                    {
                        feat.Shape = esriGeo;
                    }
                    catch (Exception e)
                    {
                        _logger.Error(obsReport.Name);
                        continue;
                    }

                    //feat.Value[1] = esriGeo;

                    var featId = feat.Fields.FindField("timeslice_interpretation");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Obstacle.TimeSlice.Interpretation.ToString();

                    featId = feat.Fields.FindField("elevation_val");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Elevation;

                    featId = feat.Fields.FindField("markingicaostandard");
                    if (featId > -1)
                    {
                        if (vstructure.MarkingICAOStandard.HasValue)
                            feat.Value[featId] = vstructure.MarkingICAOStandard.Value ? "Yes" : "No";
                    }

                    featId = feat.Fields.FindField("vert_struct_identifier");
                    if (featId > -1)
                        feat.Value[featId] = vstructure.Identifier.ToString();

                    featId = feat.Fields.FindField("usedrunway");
                    if (featId > -1)
                    {
                        var usedRunway =
                            GlobalParams.Database.Runways.FirstOrDefault(
                                rwy => rwy.Identifier == surface.RwyDirClass.RwyDir.UsedRunway.Identifier);
                        feat.Value[featId] = usedRunway?.Designator;
                    }

                    featId = feat.Fields.FindField("verticalextent_val");
                    if (featId > -1)
                    {
                        if (double.TryParse(obsReport.Height,out double val))
                            feat.Value[featId] = val;
                    }

                    featId = feat.Fields.FindField("horizontalaccuracy_uom");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Settings.OLSInterface.DistanceUnit.ToString();

                    featId = feat.Fields.FindField("frangible");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Frangible;

                    featId = feat.Fields.FindField("vert_struct_type");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.VsType?.ToString();

                    featId = feat.Fields.FindField("lighted");
                    if (featId > -1)
                    {
                        if (vstructure.Lighted.HasValue)
                            feat.Value[featId] = vstructure.Lighted.Value ? "Yes" : "No";
                    }

                    featId = feat.Fields.FindField("verticalaccuracy_val");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.VerticalAccuracy;

                    featId = feat.Fields.FindField("is_group");
                    if (featId > -1)
                    {
                        if (vstructure.Group.HasValue)
                            feat.Value[featId] = vstructure.Group.Value ? "Yes" : "No";
                    }

                    featId = feat.Fields.FindField("obst_area_type");
                    if (featId > -1)
                        feat.Value[featId] = surface.ViewCaption;

                    featId = feat.Fields.FindField("verticalaccuracy_uom");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Settings.OLSInterface.HeightUnit.ToString();

                    featId = feat.Fields.FindField("horizontalaccuracy_val");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.HorizontalAccuracy;

                    featId = feat.Fields.FindField("ownerrunway");
                    if (featId > -1)
                        feat.Value[featId] = surface.RwyDirClass.RwyDir.Designator;

                    featId = feat.Fields.FindField("ownerairport");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Database.AirportHeliport.Name;

                    //Vertical Datum

                    //

                    featId = feat.Fields.FindField("vert_struct_name");
                    if (featId > -1)
                        feat.Value[featId] = vstructure.Name;

                    featId = feat.Fields.FindField("elevation_uom");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Settings.OLSInterface.HeightUnit.ToString();

                    featId = feat.Fields.FindField("verticalextent_uom");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Settings.OLSInterface.HeightUnit.ToString();


                    featId = feat.Fields.FindField("vert_struct_designator");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Part?.Designator;

                    featId = feat.Fields.FindField("constructionstatus");
                    if (featId > -1)
                    {
                        if (obsReport.Part.ConstructionStatus.HasValue)
                            feat.Value[featId] = obsReport.Part.ConstructionStatus.Value.ToString();
                    }

                    featId = feat.Fields.FindField("frangible");
                    if (featId > -1)
                    {
                        if (obsReport.Part.Frangible.HasValue)
                            feat.Value[featId] = obsReport.Part.Frangible.Value ? "Yes" : "No";
                    }

                    featId = feat.Fields.FindField("mobile");
                    if (featId > -1)
                    {
                        if (obsReport.Part.Mobile.HasValue)
                            feat.Value[featId] = obsReport.Part.Mobile.Value ? "Yes" : "No";
                    }

                    featId = feat.Fields.FindField("verticalextent_uom");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Part.VerticalExtent?.Uom.ToString();

                    featId = feat.Fields.FindField("verticalextent_val");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Part.VerticalExtent?.Value;

                    featId = feat.Fields.FindField("verticaldatum");
                    if (featId > -1)
                    {
                        CodeVerticalDatum? verticalDatum;
                        if (obsReport.Part.HorizontalProjection.Choice== VerticalStructurePartGeometryChoice.ElevatedCurve)
                            verticalDatum = obsReport.Part.HorizontalProjection.LinearExtent?.VerticalDatum;
                        else if (obsReport.Part.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                            verticalDatum = obsReport.Part.HorizontalProjection.SurfaceExtent?.VerticalDatum;
                        else
                            verticalDatum = obsReport.Part.HorizontalProjection.Location?.VerticalDatum;

                        if (verticalDatum != null)
                            feat.Value[featId] = verticalDatum.Value.ToString();
                    }

                    featId = feat.Fields.FindField("gen_data");
                    if (featId > -1)
                        feat.Value[featId] = DateTime.Now;

                    featId = feat.Fields.FindField("penetrate");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Penetrate;


                    featId = feat.Fields.FindField("penetrate_uom");
                    if (featId > -1)
                        feat.Value[featId] = GlobalParams.Settings.OLSInterface.HeightUnit.ToString();

                    featId = feat.Fields.FindField("validtimebegin");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Obstacle.TimeSlice.ValidTime.BeginPosition;

                    featId = feat.Fields.FindField("lifetimebegin");
                    if (featId > -1)
                        feat.Value[featId] = obsReport.Obstacle.TimeSlice.FeatureLifetime.BeginPosition;

                    var lifeTimeEnd = obsReport.Obstacle.TimeSlice.FeatureLifetime.EndPosition;
                    featId = feat.Fields.FindField("lifetimeend");
                    if (featId > -1 && lifeTimeEnd != null)
                            feat.Value[featId] = lifeTimeEnd.Value;

                    var validTimeEnd = obsReport.Obstacle.TimeSlice.FeatureLifetime?.EndPosition;
                    featId = feat.Fields.FindField("validtimeend");
                    if (featId > -1 && validTimeEnd!=null)
                    {
                        feat.Value[featId] = validTimeEnd.Value;
                    }

                    feat.Store();
                }

            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw new ArgumentException("Error exporting "+surface.ViewCaption+" surface's obstacles", e);
            }
        }

        public void WriteVsToGdb(List<VerticalStructure> vsList, IFeatureClass ptFeatClass,
            IFeatureClass polyLineFeatClass, IFeatureClass polygonFeatureClass)
        {
            try
            {
                foreach (var vs in vsList)
                {
                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {
                        try
                        {
                            Geometry obs = null;

                            var feat =  ExportHelper.CreateFeature(vsPart.HorizontalProjection.Choice, ptFeatClass, polyLineFeatClass, polygonFeatureClass);

                            var extent = vsPart.GetExtent();
                            
                            obs =  vsPart.GetPartGeometry();
                            var elevation = Converters.ConverterToSI.Convert(extent.Elevation, 0);

                            SetZToGeo(obs, elevation);

                            if (obs==null) throw new ArgumentException("Vertical structure part has not geometry");

                            var esriGeo = Converters.ConvertToEsriGeom.FromGeometry(obs, true);
                            esriGeo.SpatialReference = _spGeo;
                            feat.Shape = esriGeo;

                            var featId = feat.Fields.FindField("designator");
                            if (featId > -1)
                                feat.Value[featId] = vsPart.Designator;

                            featId = feat.Fields.FindField("constructionstatus");
                            if (featId > -1)
                            {
                                if (vsPart.ConstructionStatus.HasValue)
                                    feat.Value[featId] = vsPart.ConstructionStatus.Value.ToString();
                            }

                            featId = feat.Fields.FindField("timeslice_interpretation");
                            if (featId > -1)
                                feat.Value[featId] = vs.TimeSlice.Interpretation.ToString();

                            featId = feat.Fields.FindField("elevation_val");
                            if (featId > -1)
                                feat.Value[featId] = extent.Elevation?.Value;

                            featId = feat.Fields.FindField("elevation_uom");
                            if (featId > -1)
                                feat.Value[featId] = extent.Elevation?.Uom.ToString();

                            featId = feat.Fields.FindField("frangible");
                            if (featId > -1)
                            {
                                if (vsPart.Frangible.HasValue)
                                    feat.Value[featId] = vsPart.Frangible.Value ? "Yes" : "No";
                            }

                            featId = feat.Fields.FindField("horizontalaccuracy_uom");
                            if (featId > -1)
                                feat.Value[featId] = extent.HorizontalAccuracy?.Value;

                            featId = feat.Fields.FindField("horizontalaccuracy_val");
                            if (featId > -1)
                                feat.Value[featId] = extent.HorizontalAccuracy?.Value;

                            featId = feat.Fields.FindField("identifier");
                            if (featId > -1)
                                feat.Value[featId] = vs.Identifier.ToString();


                            featId = feat.Fields.FindField("is_group");
                            if (featId > -1)
                            {
                                if (vs.Group.HasValue)
                                    feat.Value[featId] = vs.Group.Value ? "Yes" : "No";
                            }

                            featId = feat.Fields.FindField("lighted");
                            if (featId > -1)
                            {
                                if (vs.Lighted.HasValue)
                                    feat.Value[featId] = vs.Lighted.Value ? "Yes" : "No";
                            }

                            featId = feat.Fields.FindField("markingicaostandard");
                            if (featId > -1)
                            {
                                if (vs.MarkingICAOStandard.HasValue)
                                    feat.Value[featId] = vs.MarkingICAOStandard.Value ? "Yes" : "No";
                            }

                            featId = feat.Fields.FindField("mobile");
                            if (featId > -1)
                            {
                                if (vsPart.Mobile.HasValue)
                                    feat.Value[featId] = vsPart.Mobile.Value ? "Yes" : "No";
                            }


                            featId = feat.Fields.FindField("name");
                            if (featId > -1)
                                feat.Value[featId] = vs.Name;

                            featId = feat.Fields.FindField("type");
                            if (featId > -1)
                                feat.Value[featId] = vs.Type?.ToString();


                            featId = feat.Fields.FindField("verticalaccuracy_val");
                            if (featId > -1)
                                feat.Value[featId] = extent.VerticalAccuracy?.Value;

                           

                            featId = feat.Fields.FindField("verticalaccuracy_uom");
                            if (featId > -1)
                                feat.Value[featId] = extent.VerticalAccuracy?.Uom.ToString();


                            featId = feat.Fields.FindField("verticaldatum");
                            if (featId > -1)
                                feat.Value[featId] = extent.VerticalDatum?.ToString();

                            featId = feat.Fields.FindField("verticalextent_uom");
                            if (featId > -1)
                                feat.Value[featId] = vsPart.VerticalExtent?.Uom.ToString();

                            featId = feat.Fields.FindField("verticalextent_val");
                            if (featId > -1)
                                feat.Value[featId] = vsPart.VerticalExtent?.Value;



                            featId = feat.Fields.FindField("gen_data");
                            if (featId > -1)
                                feat.Value[featId] = DateTime.Now;


                            featId = feat.Fields.FindField("validtimebegin");
                            if (featId > -1)
                                feat.Value[featId] = vs.TimeSlice.ValidTime.BeginPosition.ToString();

                            featId = feat.Fields.FindField("lifetimebegin");
                            if (featId > -1)
                                feat.Value[featId] = vs.TimeSlice.FeatureLifetime
                                    .BeginPosition.ToString(CultureInfo.InvariantCulture);

                            var lifeTimeEnd = vs.TimeSlice.FeatureLifetime.EndPosition;
                            featId = feat.Fields.FindField("lifetimeend");
                            if (featId > -1 && lifeTimeEnd!=null)
                            {
                                feat.Value[featId] = lifeTimeEnd.Value.ToString(CultureInfo.InvariantCulture);
                            }

                            var validTimeEnd = vs.TimeSlice.FeatureLifetime.EndPosition;
                            featId = feat.Fields.FindField("validtimeend");
                            if (featId > -1 && validTimeEnd!=null)
                                feat.Value[featId] = validTimeEnd.Value.ToString(CultureInfo.InvariantCulture);

                            feat.Store();
                        }
                        catch (Exception e)
                        {
                            GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                            continue;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw new Exception("Error exporting vertical structures!");
            }
        }


        private void SetZToGeo(Geometry geo, double height)
        {
            if (geo.Type == GeometryType.Point)
                ((Point) geo).Z = height;
            else
            {
                var mltPoint = geo.ToMultiPoint();
                if (mltPoint == null) throw new ArgumentException("Geometry type is not correct!");

                foreach (Aran.Geometries.Point pt in mltPoint)
                    pt.Z = height;
            }

        }
    }
}
