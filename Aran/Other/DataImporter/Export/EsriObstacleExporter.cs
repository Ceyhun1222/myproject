using System;
using System.Collections.Generic;
using Aran.Converters;
using Aran.PANDA.Common;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DataImporter.Export
{
    public class EsriObstacleExporter
    {
        private IFeatureWorkspace _featureWorkspace;
        private IWorkspace _workspace;
        private readonly SpatialReferenceOperation _spOperation;

        public EsriObstacleExporter(SpatialReferenceOperation spOperation)
        {
            _spOperation = spOperation??
                throw new ArgumentNullException($"SpatialReferenceOperation doesn't configure properly in Exporter");
        }

        public void SaveVs(List<Obstacle> vsList,string folderPath)
        {
            IWorkspaceFactory workspaceFactory = new FileGDBWorkspaceFactory();
            var wn = workspaceFactory.Create(folderPath, "OmegaGDB", null, 0);
            _featureWorkspace = (wn as IName)?.Open() as IFeatureWorkspace;
            _workspace = _featureWorkspace as IWorkspace;

            var vsFeatClass = CreateVsFeatClass("Verticalstructure_point");

            WriteVsToGdb(vsList,vsFeatClass);
        }

        public IFeatureClass CreateVsFeatClass(string name)
        {
            var nameOfShapeFile = name;

            var shapeFieldName = "Shape";

            IFields fields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit) fields;


            IField field = new Field();
            IFieldEdit fieldEdit = (IFieldEdit) field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef();
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit) geomDef;
            geomDefEdit.HasZ_2 = true;


            geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            geomDefEdit.SpatialReference_2 = GetWgsSpatialReference();

            
            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            field = new Field();
            fieldEdit = (IFieldEdit) field;
            fieldEdit.Name_2 = "designator";
            fieldEdit.AliasName_2 = "Designator";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(field);

            field = new Field();
            fieldEdit = (IFieldEdit) field;
            fieldEdit.Name_2 = "elevation_val";
            fieldEdit.AliasName_2 = "Elevation";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(field);

            field = new Field();
            fieldEdit = (IFieldEdit) field;
            fieldEdit.Name_2 = "elevation_uom";
            fieldEdit.AliasName_2 = "Elevation UOM";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(field);



            //Add another miscellaneous text field

            var clsId = new UIDClass();
            clsId.Value = "esriGeoDatabase.Feature";
            IFeatureClass featureClass = _featureWorkspace?.CreateFeatureClass(nameOfShapeFile, fields, clsId, null,
                esriFeatureType.esriFTSimple, shapeFieldName, null);

            return featureClass;
        }

        public void WriteVsToGdb(List<Obstacle> vsList, IFeatureClass ptFeatClass)
        {
            foreach (var vs in vsList)
            {
                var feat = ptFeatClass.CreateFeature();

                var esriGeo = ConvertToEsriGeom.FromGeometry(vs.Geo, true);
                //esriGeo.SpatialReference = _spGeo;
                feat.Shape = esriGeo;
                //feat.Value[1] = esriGeo;

                var featId = feat.Fields.FindField("designator");
                if (featId > -1)
                    feat.Value[featId] = vs.Type;

                featId = feat.Fields.FindField("elevation_val");
                if (featId > -1)
                    feat.Value[featId] = vs.Elev;

                feat.Store();
            }
        }

        private ISpatialReference GetWgsSpatialReference()
        {
            var spRefConverter = new Aran.Geometries.Operators.SpatRefConverter();
            return spRefConverter.ToEsriSpatRef(_spOperation.SpRefGeo);
        }
    }
}
