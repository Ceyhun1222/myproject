using PVT.Engine.Common.Utils;
using System;
using System.Collections.Generic;
using PVT.Model;
using Aran.Geometries.Operators;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using Aran.Converters;
using ESRI.ArcGIS.esriSystem;

namespace PVT.Engine.IAIM.Utils
{
    public abstract class GDBExportBase
    {
        public static readonly string Format = "yyyy-MM-dd-HH-mm-ss";
        protected static IField CreateGeomField(ISpatialReference spGeo, esriGeometryType type, bool zAware = true)
        {
            IField field = new Field(); ///###########
            var fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDef geomDef = new GeometryDef(); ///#########
            var geomDefEdit = (IGeometryDefEdit)geomDef;
            if (zAware)
                geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = type;
            geomDefEdit.SpatialReference_2 = spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            return field;
        }

        protected virtual void AddStandartFields(IFieldsEdit fieldsEdit){}

        protected IField CreateField(string name, esriFieldType type)
        {
            IField field = new Field();
            var fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = name;
            fieldEdit.Type_2 = type;
            return field;
        }


        protected void AddObstacleFields(IFieldsEdit fieldsEdit)
        {
            fieldsEdit.AddField(CreateField("ObstacleName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("ObstacleIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Elevation", esriFieldType.esriFieldTypeDouble));
            fieldsEdit.AddField(CreateField("ElevationUnit", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Accuracy", esriFieldType.esriFieldTypeDouble));
            fieldsEdit.AddField(CreateField("AccuracyUnit", esriFieldType.esriFieldTypeString));
        }

        protected IFeatureClass CreateFeatureClassForVs(string name, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, esriGeometryType type)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, type));
            AddStandartFields(fieldsEdit);
            AddObstacleFields(fieldsEdit);


            var featClass = featureWorkspace.CreateFeatureClass(name, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            var featureClass = featClass;

            return featureClass;
        }

        protected class VsWrapper
        {
            public IGeometry EsriGeo { get; set; } = null;
            public double Elevation { get; set; } = 0;
            public string ElevationUnit { get; set; } = null;
            public double Accuracy { get; set; } = 0;
            public string AccuracyUnit { get; set; } = null;
        }

        protected static VsWrapper GetObstacleData(VerticalStructurePart part)
        {
            VsWrapper wrapper = new VsWrapper();

            switch (part.Type)
            {
                case VerticalStructurePartGeometryChoice.ElevatedPoint:
                    var point = part.Point.Geo;
                    wrapper.EsriGeo = ConvertToEsriGeom.FromGeometry(point, true);
                    wrapper.Elevation = part.Original.HorizontalProjection.Location.Elevation.Value;
                    wrapper.ElevationUnit = part.Original.HorizontalProjection.Location.Elevation.Uom.ToString();
                    wrapper.Accuracy = part.Original.HorizontalProjection.Location.VerticalAccuracy?.Value ?? 0;
                    wrapper.AccuracyUnit = part.Original.HorizontalProjection.Location.VerticalAccuracy?.Uom.ToString();
                    break;
                case VerticalStructurePartGeometryChoice.ElevatedCurve:
                    wrapper.EsriGeo = ConvertToEsriGeom.FromGeometry(part.Geo, true);
                    wrapper.Elevation = part.Original.HorizontalProjection.LinearExtent.Elevation.Value;
                    wrapper.ElevationUnit = part.Original.HorizontalProjection.LinearExtent.Elevation.Uom.ToString();
                    wrapper.Accuracy = part.Original.HorizontalProjection.LinearExtent.VerticalAccuracy?.Value ?? 0;
                    wrapper.AccuracyUnit = part.Original.HorizontalProjection.LinearExtent.VerticalAccuracy?.Uom.ToString();
                    break;
                case VerticalStructurePartGeometryChoice.ElevatedSurface:
                    wrapper.EsriGeo = ConvertToEsriGeom.FromGeometry(part.Geo, true);
                    wrapper.Elevation = part.Original.HorizontalProjection.SurfaceExtent.Elevation.Value;
                    wrapper.ElevationUnit = part.Original.HorizontalProjection.SurfaceExtent.Elevation.Uom.ToString();
                    wrapper.Accuracy = part.Original.HorizontalProjection.SurfaceExtent.VerticalAccuracy?.Value ?? 0;
                    wrapper.AccuracyUnit = part.Original.HorizontalProjection.SurfaceExtent.VerticalAccuracy?.Uom.ToString();
                    break;
                default:
                    break;
            }
            return wrapper;
        }
    }
}