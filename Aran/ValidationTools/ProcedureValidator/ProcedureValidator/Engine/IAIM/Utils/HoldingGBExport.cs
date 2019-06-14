using System;
using System.Collections.Generic;
using System.IO;
using Aran.Converters;
using Aran.Geometries.Operators;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PVT.Model;

namespace PVT.Engine.IAIM.Utils
{
    class HoldingGDBExport : GDBExportBase
    {
        private readonly Dictionary<Guid, Navaid> _navaids = new Dictionary<Guid, Navaid>();
        public void ExportToGDB(string folder, HoldingPattern pattern)
        {
            _navaids.Clear();
            var commonFolder = System.IO.Path.Combine(folder, "HoldingPattern");
            if (!Directory.Exists(commonFolder))
                Directory.CreateDirectory(commonFolder);

            var holdingFolder = System.IO.Path.Combine(commonFolder, $"holding_{pattern.Identifier}_{DateTime.Now.ToString(GDBExportBase.Format)}");
            if (!Directory.Exists(holdingFolder))
                Directory.CreateDirectory(holdingFolder);

            ExportToGdb(holdingFolder, pattern, 1);
            ExportNavaids(holdingFolder);
        }

        public void ExportNavaids(string folder)
        {
            var spatRefConverter = new SpatRefConverter();
            var geom = Environment.Current.Geometry as Geometry.Geometry;
            var spGeo = spatRefConverter.ToEsriSpatRef(geom.SpartialReferenceOpration.SpRefGeo);

            IWorkspaceFactory workspaceFactory = null;
            workspaceFactory = new FileGDBWorkspaceFactory();
            var wn = workspaceFactory.Create(folder, "Navaid", null, 0);
            var featureWorkspace = (wn as IName).Open() as IFeatureWorkspace;

            var workspaceEdit = (IWorkspaceEdit)featureWorkspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;
            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPoint, false));
            fieldsEdit.AddField(CreateField("Identifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Name", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("FullName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Type", esriFieldType.esriFieldTypeString));

            //Add another miscellaneous text field

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("Navaid", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");


            foreach (var navaid in _navaids)
            {
                try
                {
                    //var tmpPt =GlobalParams.SpatialRefOperation.ToPrj(centerLine.Location.Geo);
                    if (navaid.Value.Original.Location != null && navaid.Value.Original.Location.Geo != null)
                    {
                        var tmpPt = navaid.Value.Original.Location.Geo;
                        IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                        esriPt.PutCoords(tmpPt.X, tmpPt.Y);
                        var feat = featClass.CreateFeature();
                        feat.set_Value(0, esriPt);
                        feat.set_Value(1, navaid.Value.Original.Identifier.ToString());
                        feat.set_Value(2, navaid.Value.Name);
                        feat.set_Value(3, navaid.Value.FullName);
                        if (navaid.Value.Original.Type != null)
                            feat.set_Value(4, navaid.Value.Original.Type.ToString());

                        feat.Store();
                    }
                }
                catch (Exception ex)
                {
                    Environment.Current.Logger.Error(ex, "Navaid exception");
                    continue;
                }
            }

            _navaids.Clear();
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

        }

        public void ExportRunway(string folder, RunwayDirection runway)
        {
            var spatRefConverter = new SpatRefConverter();
            var geom = Environment.Current.Geometry as Geometry.Geometry;
            var spGeo = spatRefConverter.ToEsriSpatRef(geom.SpartialReferenceOpration.SpRefGeo);

            IWorkspaceFactory workspaceFactory = null;
            workspaceFactory = new FileGDBWorkspaceFactory();
            var wn = workspaceFactory.Create(folder, "RwyCenterLinePoint_" + runway.Designator, null, 0);
            var featureWorkspace = (wn as IName).Open() as IFeatureWorkspace;

            var workspaceEdit = (IWorkspaceEdit)featureWorkspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;
            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPoint, false));
            fieldsEdit.AddField(CreateField("RunwayIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("RunwayDesignator", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("RunwayDirectionDesignator", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Type", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Height", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("HUnit", esriFieldType.esriFieldTypeString));

            //Add another miscellaneous text field

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass("RwyCenterLinePoint", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");


            foreach (var centerLine in runway.CentreLinePoints)
            {
                //var tmpPt =GlobalParams.SpatialRefOperation.ToPrj(centerLine.Location.Geo);
                var tmpPt = centerLine.Value.Original.Location.Geo;
                IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                esriPt.PutCoords(tmpPt.X, tmpPt.Y);
                var feat = featClass.CreateFeature();
                feat.set_Value(0, esriPt);
                feat.set_Value(1, runway.Runway.Identifier.ToString());
                feat.set_Value(2, runway.Runway.Designator);
                feat.set_Value(3, runway.Designator);
                feat.set_Value(4, centerLine.Value.Type.ToString());
                feat.set_Value(5, centerLine.Value.Original.Location.Elevation.Value.ToString());
                feat.set_Value(6, centerLine.Value.Original.Location.Elevation.Uom.ToString());
                feat.Store();
            }


            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        public void ExportToGdb(string folder, HoldingPattern pattern, int count)
        {

            var spatRefConverter = new SpatRefConverter();
            var geom = Environment.Current.Geometry as Geometry.Geometry;
            var spGeo = spatRefConverter.ToEsriSpatRef(geom.SpartialReferenceOpration.SpRefGeo);

            IWorkspaceFactory workspaceFactory = null;
            workspaceFactory = new FileGDBWorkspaceFactory();
            var wn = workspaceFactory.Create(folder, count.ToString(), null, 0); ;
            var featureWorkspace = (wn as IName).Open() as IFeatureWorkspace;

            var workspaceEdit = (IWorkspaceEdit)featureWorkspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            ExportAssesmentAreas(pattern, spGeo, featureWorkspace);
            ExportAssesmentAreasObstacles(pattern, spGeo, featureWorkspace);
            ExportNominalTrack(pattern, spGeo, featureWorkspace);
            ExportTerminalPoints(pattern, spGeo, featureWorkspace);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }
       

        private void ExportTerminalPoints(HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPoint));
            AddStandartFields(fieldsEdit);
            fieldsEdit.AddField(CreateField("Type", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("NavaidIdIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("DesignatedPointIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionName", esriFieldType.esriFieldTypeString));


            var featClass = featureWorkspace.CreateFeatureClass("Fix", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            var featureClass = featClass;


            FillPoint(pattern, pattern.HoldingPoint.Value, spGeo, featureClass);
        }

        private void AddNavaid(Navaid navaid)
        {
            if (navaid != null)
                if (!_navaids.ContainsKey(navaid.Original.Identifier))
                    _navaids.Add(navaid.Original.Identifier, navaid);
        }
        

        private void FillPoint(HoldingPattern pattern, SegmentPoint pnt, ISpatialReference spGeo, IFeatureClass featureClass)
        {
            if (pnt == null) return;
            foreach (var navaid in pnt.Navaids.Values)
            {
                AddNavaid(navaid);
            }
            if (pnt.Geo == null) return;
            var point = pnt.Geo.ToAranPoint();
            var esriGeo = ConvertToEsriGeom.FromGeometry(point, true);
            esriGeo.SpatialReference = spGeo;
            var feat = featureClass.CreateFeature();
            feat.Shape = esriGeo;
            FillStandartFields(pattern, feat);
            feat.set_Value(2, pnt.PointChoice.Type.ToString());
            switch (pnt.PointChoice.Type)
            {
                case SignificantPointType.Navaid:
                    feat.set_Value(3, pnt.PointChoice.Navaid.Original.Identifier.ToString());
                    break;
                case SignificantPointType.DesignatedPoint:
                    feat.set_Value(4, pnt.PointChoice.DesignatedPoint.Original.Identifier.ToString());
                    break;
                default:
                    break;
            }
            if (pnt.Guidance != null)
            {
                feat.set_Value(5, pnt.Guidance?.Original?.Identifier.ToString());
                feat.set_Value(6, pnt.Guidance?.Name);
            }
            if (pnt.Intersection != null)
            {
                feat.set_Value(7, pnt.Intersection?.Original?.Identifier.ToString());
                feat.set_Value(8, pnt.Intersection?.Name);
            }

            feat.Store();
        }

        private void ExportNominalTrack(HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPolyline));
            AddStandartFields(fieldsEdit);

            var featClass = featureWorkspace.CreateFeatureClass("Extent", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");


            var featureClass = featClass;
            var line = pattern.Extent;
            if (line == null) return;

            var esriGeo = ConvertToEsriGeom.FromGeometry(line, true);
            esriGeo.SpatialReference = spGeo;
            var feat = featureClass.CreateFeature();
            feat.Shape = (IGeometry)esriGeo;
            FillStandartFields(pattern, feat);
            feat.Store();
        }

        private void ExportAssesmentAreasObstacles(HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace)
        {
            if (pattern.AssessmentAreas != null)
                for (int i = 0; i < pattern.AssessmentAreas.Count; i++)
                {
                    ExportAssesmentAreaObstacles($"AssessmentArea_Obstacles{i + 1}", pattern, spGeo, featureWorkspace, pattern.AssessmentAreas[i]);
                }
        }

        private void ExportAssesmentAreaObstacles(string name, HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace,  ObstacleAssessmentArea area)
        {
            var featureClassPoint = CreateFeatureClassForVs(name + "_point", spGeo, featureWorkspace, esriGeometryType.esriGeometryPoint);
            var featureClassPolyline = CreateFeatureClassForVs(name + "_polyline", spGeo, featureWorkspace, esriGeometryType.esriGeometryPolyline);
            var featureClassPolygon = CreateFeatureClassForVs(name + "_polygon", spGeo, featureWorkspace, esriGeometryType.esriGeometryPolygon);
            if (area?.Geo == null) return;

            foreach (var t in area.Obstacles)
            {
                foreach (var part in t.Parts)
                {
                    switch (part.Type)
                    {
                        case VerticalStructurePartGeometryChoice.ElevatedPoint:
                            AddObstacleData(pattern, featureClassPoint, t, part);
                            break;
                        case VerticalStructurePartGeometryChoice.ElevatedCurve:
                            AddObstacleData(pattern, featureClassPolyline, t, part);
                            break;
                        case VerticalStructurePartGeometryChoice.ElevatedSurface:
                            AddObstacleData(pattern, featureClassPolygon, t, part);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddObstacleData(HoldingPattern pattern,  IFeatureClass featureClass,  VerticalStructure obs, VerticalStructurePart part)
        {
            var wrapper = GetObstacleData(part);

            var feat = featureClass.CreateFeature();
            feat.Shape = wrapper.EsriGeo;
            FillStandartFields(pattern,  feat);
            feat.set_Value(2, obs.Name);
            feat.set_Value(3, obs.Identifier.ToString());
            feat.set_Value(4, wrapper.Elevation);
            feat.set_Value(5, wrapper.ElevationUnit);
            feat.set_Value(6, wrapper.Accuracy);
            feat.set_Value(7, wrapper.AccuracyUnit);
            feat.Store();
        }


        private void ExportAssesmentAreas(HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace)
        {
            if (pattern.AssessmentAreas != null)
                for (int i = 0; i < pattern.AssessmentAreas.Count; i++)
                {
                    ExportAssesmentArea($"AssessmentArea_{i + 1}", pattern, spGeo, featureWorkspace, pattern.AssessmentAreas[i]);
                }
        }

        private void ExportAssesmentArea(string name, HoldingPattern pattern, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, ObstacleAssessmentArea area)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPolygon));
            AddStandartFields(fieldsEdit);

            var featClass = featureWorkspace.CreateFeatureClass(name, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            var featureClass = featClass;

            if (area == null || area.Geo == null) return;

            var surfaceGeo = area.Geo;
            var esriGeo = ConvertToEsriGeom.FromGeometry(surfaceGeo, true);
            esriGeo.SpatialReference = spGeo;
            var feat = featureClass.CreateFeature();
            feat.Shape = esriGeo;
            FillStandartFields(pattern, feat);
            feat.Store();
        }

        private static void FillStandartFields(HoldingPattern pattern, IFeature feat)
        {
            feat.set_Value(1, pattern.Identifier.ToString());
        }

        protected override void AddStandartFields(IFieldsEdit fieldsEdit)
        {
            fieldsEdit.AddField(CreateField("HoldingId", esriFieldType.esriFieldTypeString));
        }
    }
}