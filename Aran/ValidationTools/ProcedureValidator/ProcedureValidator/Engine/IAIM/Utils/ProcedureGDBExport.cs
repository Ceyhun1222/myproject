using Aran.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Enums;
using Aran.Geometries.Operators;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PVT.Model;

namespace PVT.Engine.IAIM.Utils
{
    class ProcedureGDBExport:GDBExportBase
    {
        private readonly Dictionary<Guid, Navaid> _navaids = new Dictionary<Guid, Navaid>();
        public void ExportToGDB(string folder, ProcedureBase procedure)
        {
            _navaids.Clear();
            var commonFolder = System.IO.Path.Combine(folder, procedure.Type.ToString());
            if (!Directory.Exists(commonFolder))
                Directory.CreateDirectory(commonFolder);

            var procFolder = System.IO.Path.Combine(commonFolder,
                $"{procedure.Name}_{procedure.Identifier}_{DateTime.Now.ToString(GDBExportBase.Format)}");
            if (!Directory.Exists(procFolder))
                Directory.CreateDirectory(procFolder);
            var types = new Dictionary<string, int>();
            foreach (var t in procedure.Transtions)
            {
                int count = 0;
                if (types.ContainsKey(t.Type))
                {
                    types[t.Type] = types[t.Type] + 1;
                    count = types[t.Type];
                }
                else
                {
                    types.Add(t.Type, 1);
                    count = 1;
                }
                var transitionFolder = System.IO.Path.Combine(procFolder, count == 1 ? t.Type : $"{t.Type}_{count}");
                if (!Directory.Exists(transitionFolder))
                    Directory.CreateDirectory(transitionFolder);


                int legCount = 1;
                string legType = null;

               // SetZValueToSegments(t.TransitionLegs,procedure);

                foreach (var trLeg in t.TransitionLegs)
                {
                    if (trLeg.IsEmpty)
                        continue;
                    var leg = trLeg.SegmentLeg;
                    if (legType != leg.Type)
                    {
                        legCount = 1;
                        legType = leg.Type;
                    }
                    var legFolder = System.IO.Path.Combine(transitionFolder, legType);
                    if (!Directory.Exists(legFolder))
                        Directory.CreateDirectory(legFolder);


                    ExportToGdb(legFolder, procedure, leg, t.Type, legCount++);
                }
            }
            ExportRunway(procFolder, procedure.RunwayDirection);
            ExportNavaids(procFolder);
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

        public void ExportToGdb(string folder, ProcedureBase procedure, SegmentLeg leg, string transtion, int count)
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

            ExportAssesmentAreas(procedure, leg, spGeo, featureWorkspace, transtion);
            ExportAssesmentAreasObstacles(procedure, leg, spGeo, featureWorkspace, transtion);
            ExportNominalTrack(procedure, leg, spGeo, featureWorkspace, transtion);
            ExportTerminalPoints(procedure, leg, spGeo, featureWorkspace, transtion);
            ExportTerminalPointsFixToleranceArea(procedure, leg, spGeo, featureWorkspace, transtion);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }
        private void ExportTerminalPointsFixToleranceArea(ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPolygon));
            AddStandartFields(fieldsEdit);
            fieldsEdit.AddField(CreateField("Type", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionName", esriFieldType.esriFieldTypeString));

            var featClass = featureWorkspace.CreateFeatureClass("FixToleranceArea", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            var featureClass = featClass;

            FillPointArea(procedure, leg, leg.StartPoint, spGeo, transtion, featureClass, "start point");
            FillPointArea(procedure, leg, leg.EndPoint, spGeo, transtion, featureClass, "end point");
        }


        private void ExportTerminalPoints(ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPoint));
            AddStandartFields(fieldsEdit);
            fieldsEdit.AddField(CreateField("PointType", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Type", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("NavaidIdIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("DesignatedPointIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("GuidanceName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionIdentifier", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("IntersectionName", esriFieldType.esriFieldTypeString));

            var featClass = featureWorkspace.CreateFeatureClass("Fix", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            var featureClass = featClass;


            FillPoint(procedure, leg, leg.StartPoint, spGeo, transtion, featureClass, "start point");
            FillPoint(procedure, leg, leg.EndPoint, spGeo, transtion, featureClass, "end point");
        }

        private void AddNavaid(Navaid navaid)
        {
            if (navaid != null)
                if (!_navaids.ContainsKey(navaid.Original.Identifier))
                    _navaids.Add(navaid.Original.Identifier, navaid);
        }
        private void FillPointArea(ProcedureBase procedure, SegmentLeg leg, TerminalSegmentPoint pnt, ISpatialReference spGeo, string transtion, IFeatureClass featureClass, string type)
        {
            if (pnt == null) return;
            AddNavaid(pnt.Guidance);
            AddNavaid(pnt.Intersection);
            var references = pnt.Original.FacilityMakeup;
            foreach (var t in references)
            {
                if (t.FixToleranceArea != null)
                {
                    var poly = t.FixToleranceArea.Geo;
                    var esriGeo = ConvertToEsriGeom.FromGeometry(poly, true);
                    esriGeo.SpatialReference = spGeo;
                    var feat = featureClass.CreateFeature();
                    feat.Shape = esriGeo;
                    FillStandartFields(procedure, leg, transtion, feat);
                    feat.set_Value(6, type);
                    if (pnt.Guidance != null)
                    {
                        feat.set_Value(7, pnt.Guidance?.Original?.Identifier.ToString());
                        feat.set_Value(8, pnt.Guidance?.Name);
                    }
                    if (pnt.Intersection != null)
                    {
                        feat.set_Value(9, pnt.Intersection?.Original?.Identifier.ToString());
                        feat.set_Value(10, pnt.Intersection?.Name);
                    }
                    feat.Store();
                }
            }
        }

        private void FillPoint(ProcedureBase procedure, SegmentLeg leg, TerminalSegmentPoint pnt, ISpatialReference spGeo, string transtion, IFeatureClass featureClass, string type)
        {
            if (pnt == null || pnt.Geo == null) return;

            var point = pnt.Geo.ToAranPoint();
            var esriGeo = ConvertToEsriGeom.FromGeometry(point, true);
            esriGeo.SpatialReference = spGeo;
            var feat = featureClass.CreateFeature();
            feat.Shape = esriGeo;
            FillStandartFields(procedure, leg, transtion, feat);
            feat.set_Value(6, type);
            feat.set_Value(7, pnt.PointChoice.Type.ToString());
            switch (pnt.PointChoice.Type)
            {
                case SignificantPointType.Navaid:
                    feat.set_Value(8, pnt.PointChoice.Navaid.Original.Identifier.ToString());
                    break;
                case SignificantPointType.DesignatedPoint:
                    feat.set_Value(9, pnt.PointChoice.DesignatedPoint.Original.Identifier.ToString());
                    break;
                default:
                    break;
            }
            if (pnt.Guidance != null)
            {
                feat.set_Value(10, pnt.Guidance?.Original?.Identifier.ToString());
                feat.set_Value(11, pnt.Guidance?.Name);
            }
            if (pnt.Intersection != null)
            {
                feat.set_Value(12, pnt.Intersection?.Original?.Identifier.ToString());
                feat.set_Value(13, pnt.Intersection?.Name);
            }

            feat.Store();
        }

        private void ExportNominalTrack(ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion)
        {
            IFields fields = new Fields();
            var fieldsEdit = (IFieldsEdit)fields;

            fieldsEdit.AddField(CreateGeomField(spGeo, esriGeometryType.esriGeometryPolyline));
            AddStandartFields(fieldsEdit);

            var featClass = featureWorkspace.CreateFeatureClass("NominalTrack", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");


            var featureClass = featClass;
            var line = leg.NominalTrack;
            if (leg.NominalTrack == null) return;

            var esriGeo = ConvertToEsriGeom.FromGeometry(line, true);
            esriGeo.SpatialReference = spGeo;
            var feat = featureClass.CreateFeature();
            feat.Shape = (IGeometry)esriGeo;
            FillStandartFields(procedure, leg, transtion, feat);
            feat.Store();
        }

        

        private void ExportAssesmentAreasObstacles(ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion)
        {
            ExportAssesmentAreaObstacles("PrimaryProttectedAreaObstacles", procedure, leg, spGeo, featureWorkspace, transtion, leg.PrimaryProttectedArea);
            ExportAssesmentAreaObstacles("SecondaryProttectedAreaObstacles", procedure, leg, spGeo, featureWorkspace, transtion, leg.SecondaryProttectedArea);
        }

        private void ExportAssesmentAreaObstacles(string name, ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion, ObstacleAssessmentArea area)
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
                            AddObstacleData(procedure, leg, featureClassPoint, transtion, t, part);
                            break;
                        case VerticalStructurePartGeometryChoice.ElevatedCurve:
                            AddObstacleData(procedure, leg, featureClassPolyline, transtion, t, part);
                            break;
                        case VerticalStructurePartGeometryChoice.ElevatedSurface:
                            AddObstacleData(procedure, leg, featureClassPolygon, transtion, t, part);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddObstacleData(ProcedureBase procedure, SegmentLeg leg, IFeatureClass featureClass, string transtion, VerticalStructure obs, VerticalStructurePart part)
        {
            var wrapper = GetObstacleData(part);

            var feat = featureClass.CreateFeature();
            feat.Shape = wrapper.EsriGeo;
            FillStandartFields(procedure, leg, transtion, feat);
            feat.set_Value(6, obs.Name);
            feat.set_Value(7, obs.Identifier.ToString());
            feat.set_Value(8, wrapper.Elevation);
            feat.set_Value(9, wrapper.ElevationUnit);
            feat.set_Value(10, wrapper.Accuracy);
            feat.set_Value(11, wrapper.AccuracyUnit);
            feat.Store();
        }

        private void ExportAssesmentAreas(ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion)
        {
            ExportAssesmentArea("PrimaryProttectedArea", procedure, leg, spGeo, featureWorkspace, transtion, leg.PrimaryProttectedArea);
            ExportAssesmentArea("SecondaryProttectedArea", procedure, leg, spGeo, featureWorkspace, transtion, leg.SecondaryProttectedArea);
        }

        private void ExportAssesmentArea(string name, ProcedureBase procedure, SegmentLeg leg, ISpatialReference spGeo, IFeatureWorkspace featureWorkspace, string transtion, ObstacleAssessmentArea area)
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
            FillStandartFields(procedure, leg, transtion, feat);
            feat.Store();
        }

        private static void FillStandartFields(ProcedureBase procedure, SegmentLeg leg, string transtion, IFeature feat)
        {
            feat.set_Value(1, procedure.Name);
            feat.set_Value(2, procedure.Identifier.ToString());
            feat.set_Value(3, transtion);
            feat.set_Value(4, leg.Identifier.ToString());
            feat.set_Value(5, leg.Type);
        }

        protected override void AddStandartFields(IFieldsEdit fieldsEdit)
        {
            fieldsEdit.AddField(CreateField("ProcedureName", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("ProcedureId", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("Transtition", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("LegId", esriFieldType.esriFieldTypeString));
            fieldsEdit.AddField(CreateField("LegType", esriFieldType.esriFieldTypeString));
        }


        private void SetZValueToSegments(List<TransitionLeg> legs,ProcedureBase proc)
        {
            var refPoint = Engine.Environment.Current.DbProvider.GetAirport(Engine.Environment.Current.CurrentAeroport);

            double baseElevaation;
            if (proc.Type == ProcedureType.StandardInstrumentDeparture)
                baseElevaation = Aran.Converters.ConverterToSI.Convert(refPoint.ARP.Elevation, 0);
            else
                baseElevaation = Aran.Converters.ConverterToSI.Convert(legs[0].SegmentLeg.Altitude, 0);

            int startIndex = 0;
            if (legs[0].SegmentLeg.LegTypeARINC == CodeSegmentPath.IF)
                startIndex += 1;

            double segmentElev = AssignZValueToNominalTrack(legs[startIndex].SegmentLeg,baseElevaation); 


            for (int i = startIndex+1; i < legs.Count; i++)
            {
                segmentElev = AssignZValueToNominalTrack(legs[i].SegmentLeg, segmentElev
                );
            }

        }

        private double AssignZValueToNominalTrack(SegmentLeg leg, double prevSegmentAltitude,Aran.Geometries.Point sortBasePoint=null)
        {
            var legGeometry = leg.NominalTrack;

            double prevVertexAltitude = prevSegmentAltitude;
            var vertexes = leg.NominalTrack.ToMultiPoint();

            List<Aran.Geometries.Point> sortedVertexes = vertexes.ToList();
            if (sortBasePoint!=null)
                vertexes.OrderBy(ver => Aran.PANDA.Common.ARANFunctions.ReturnGeodesicDistance(ver, sortBasePoint)).ToList();

            Aran.Geometries.Point ptPrev = sortedVertexes[0];
            ptPrev.Z = prevSegmentAltitude;
            for (int i = 1; i < sortedVertexes.Count; i++)
            {
                var pt = vertexes[i];
                if (!leg.VerticalAngle.HasValue)
                {
                    pt.Z = prevSegmentAltitude;
                    continue;
                }

                var distance = Aran.PANDA.Common.ARANFunctions.ReturnGeodesicDistance(pt, ptPrev);
                prevVertexAltitude += Math.Tan(Aran.PANDA.Common.ARANMath.DegToRad(leg.VerticalAngle.Value)) * distance;
                pt.Z = prevVertexAltitude;
                ptPrev = pt;
                
            }
            sortBasePoint = ptPrev;
            return prevVertexAltitude;

        }
    }
}
