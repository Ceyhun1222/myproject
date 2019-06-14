using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Omega.Extensions;
using Aran.Omega.Models;
using Aran.Omega.SettingsUI;
using Aran.PANDA.Constants;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.Export
{
    public class EtodExportToGdbHelper
    {
        private readonly ISpatialReference _spGeo;
        private IWorkspace _workspace;
        private IFeatureWorkspace _featureWorkspace;
        private readonly InterfaceModel _interfaceSettings = GlobalParams.Settings.OLSInterface;
        private readonly List<ObstacleArea> _obstacleAreas;
        private readonly AirportHeliport _adhp;
        private readonly RwyDirClass _rwyDir;
        private ILogger _logger = GlobalParams.Logger;
        private OmegaSettings _settings = GlobalParams.Settings;
        private ITable obsClass;
        private IFeatureClass obsPoinClass;
        private IFeatureClass obsPolylineClass;
        private IFeatureClass obsPolygonClass;

        public EtodExportToGdbHelper(List<ObstacleArea> obstacleAreas,AirportHeliport adhp,RwyDirClass rwyDir)
        {
            _obstacleAreas = obstacleAreas;
            _adhp = adhp;
            _rwyDir = rwyDir;
            SpatRefConverter spatRefConverter = new SpatRefConverter();
            _spGeo = spatRefConverter.ToEsriSpatRef(GlobalParams.SpatialRefOperation.SpRefGeo);
        }

        public void Export(IEnumerable<DrawingSurface> surfaceList,string folderPath,string fileName)
        {
            InitializeEsriFeatureWorkSpace(folderPath,fileName);

            var etodFeatClassFactory = new EtodFeatureClassFactory(_featureWorkspace);
            
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            try
            {
                CreateFeatureClasses(folderPath, fileName);

                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                var obstacleList = new List<ObstacleReport>();
                foreach (var surface in surfaceList)
                {
                    obstacleList.AddRange(surface.SurfaceBase.GetReport.Where
                        (obs => surface.SurfaceBase.EtodSurfaceType == EtodSurfaceType.Area3 || obs.Penetrate >= 0));
                }

                WriteObstacleToGdb(obstacleList, false);

            }
            finally
            {
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }
        }

        public void ExportAnnex14(IEnumerable<DrawingSurface> surfaceList, string folderPath, string fileName)
        {
            InitializeEsriFeatureWorkSpace(folderPath, fileName);

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            try
            {
                CreateFeatureClasses(folderPath, fileName);

                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                var obstacleList = new List<ObstacleReport>();
                foreach (var surface in surfaceList)
                {
                    obstacleList.AddRange(surface.SurfaceBase.GetReport.Where(obs=>obs.Penetrate > 0));
                }

                WriteObstacleToGdb(obstacleList,true);

            }
            finally
            {
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }
        }

        private void CreateFeatureClasses(string folderPath, string fileName)
        {
            var etodFeatClassFactory = new EtodFeatureClassFactory(_featureWorkspace);

            obsClass = etodFeatClassFactory.CreateVSFeatClass("Obstacle");

            obsPoinClass =
                etodFeatClassFactory.CreateObstaclePartFeatClass("PointObstacle",
                    ObstacleGeomType.Point);
            obsPolygonClass =
                etodFeatClassFactory.CreateObstaclePartFeatClass("PolygonObstacle",
                    ObstacleGeomType.Polygon);
            obsPolylineClass =
                etodFeatClassFactory.CreateObstaclePartFeatClass("PolylineObstacle",
                    ObstacleGeomType.PolyLine);

        }

        private void InitializeEsriFeatureWorkSpace(string folderPath,string fileName)
        {
            var workspaceFactory =(IWorkspaceFactory2)new AccessWorkspaceFactoryClass();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var  workspaceName = workspaceFactory.Create(folderPath, fileName, null, 0) as IName;
            _featureWorkspace = workspaceName.Open() as IFeatureWorkspace;

            _workspace = _featureWorkspace as IWorkspace;
        }

        private void WriteObstacleToGdb(IEnumerable<ObstacleReport> obstacles,bool isAnnex14)
        {
            try
            {
                var vsList = obstacles.Select(obs => obs.Obstacle)
                                      .Distinct();

                foreach (var verticalStructure in vsList)
                {
                    WriteVerticalStructure(obsClass, verticalStructure);
                    WriteParts(obstacles, isAnnex14, verticalStructure);
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw new ArgumentException("Etod export error ", e);
            }
        }

        private void WriteParts(IEnumerable<ObstacleReport> obstacles, bool isAnnex14, VerticalStructure verticalStructure)
        {
            foreach (var vsPart in verticalStructure.Part)
            {
                Geometry vsPartGeometry = vsPart.GetPartGeometry();
                var feat = ExportHelper.CreateFeature(vsPart.HorizontalProjection.Choice, obsPoinClass, obsPolylineClass,
                    obsPolygonClass);

                var obstacleReport = obstacles.
                    Where(obs => Equals(obs.Name, verticalStructure.Name)).
                    OrderByDescending(obs => obs.Penetrate).FirstOrDefault();

                var extent = vsPart.GetExtent();

                SetZToGeo(vsPartGeometry, ConverterToSI.Convert(extent.Elevation, 0));

                var esriGeo = ConvertToEsriGeom.FromGeometry(vsPartGeometry, true);
                esriGeo.SpatialReference = _spGeo;
                try
                {
                    feat.Shape = esriGeo;
                }
                catch (Exception e)
                {
                    _logger.Error(verticalStructure.Name);
                    continue;
                }


                var featId = feat.Fields.FindField("Designator");
                if (featId > -1)
                {
                    feat.Value[featId] = vsPart.Designator;
                }

                featId = feat.Fields.FindField("VerticalStructure_Id");
                if (featId > -1)
                {
                    feat.Value[featId] = verticalStructure.Identifier.ToString();
                }

                featId = feat.Fields.FindField("Area");
                if (featId > -1)
                {

                    if (obstacleReport != null)
                        feat.Value[featId] = isAnnex14 ? obstacleReport.SurfaceType.ToString() :
                                                    obstacleReport.EtodSurfaceType.ToString();
                }

                featId = feat.Fields.FindField("HPos");
                if (featId > -1)
                {
                    feat.Value[featId] = "Keep in shape field";
                }

                featId = feat.Fields.FindField("HRefSys");
                if (featId > -1)
                {
                    feat.Value[featId] = "WGS-84";
                }

                featId = feat.Fields.FindField("HConf");
                if (featId > -1)
                {
                    feat.Value[featId] = 90;
                }

                featId = feat.Fields.FindField("HAcc");
                if (featId > -1)
                {
                    var horAccuracy = ConverterToSI.Convert(extent.HorizontalAccuracy, 0);
                    if (Math.Abs(horAccuracy) > 0.01)
                        feat.Value[featId] = horAccuracy;
                }

                featId = feat.Fields.FindField("Elev");
                if (featId > -1)
                {
                    var elevation = ConverterToSI.Convert(extent.Elevation, 0);
                    if (Math.Abs(elevation) > 0.01)
                        feat.Value[featId] = elevation;
                }

                featId = feat.Fields.FindField("Height");
                if (featId > -1)
                {
                    var height = ConverterToSI.Convert(vsPart.VerticalExtent, 0);
                    if (Math.Abs(height) > 0)
                        feat.Value[featId] = height;
                }

                featId = feat.Fields.FindField("Unit");
                if (featId > -1)
                {
                    feat.Value[featId] = "Meter";
                }

                featId = feat.Fields.FindField("VRefSys");
                if (featId > -1)
                {
                    if (extent.VerticalDatum != null)
                    {
                        feat.Value[featId] = GetVerticalDatum(extent.VerticalDatum);
                    }
                }

                featId = feat.Fields.FindField("VConf");
                if (featId > -1)
                {
                    feat.Value[featId] = 90;
                }

                featId = feat.Fields.FindField("VAcc");
                if (featId > -1)
                {
                    var verticalAccuracy = ConverterToSI.Convert(extent.VerticalAccuracy, 0);
                    if (Math.Abs(verticalAccuracy) > 0.01)
                        feat.Value[featId] = verticalAccuracy;
                }

                if (obstacleReport != null)
                {
                    featId = feat.Fields.FindField("HRes");
                    if (featId > -1)
                    {
                        if (!isAnnex14 && obstacleReport.EtodSurfaceType == EtodSurfaceType.Area1)
                            feat.Value[featId] = 0.00001;
                        else
                            feat.Value[featId] = 0.000001;
                    }

                    featId = feat.Fields.FindField("VRes");
                    if (featId > -1)
                    {
                        if (isAnnex14)
                            feat.Value[featId] = 0.1;
                        else if (obstacleReport.EtodSurfaceType == EtodSurfaceType.Area1)
                            feat.Value[featId] = 1;
                        else if (obstacleReport.EtodSurfaceType == EtodSurfaceType.Area3)
                            feat.Value[featId] = 0.01;
                        else
                            feat.Value[featId] = 0.1;
                    }
                }

                featId = feat.Fields.FindField("Type");
                if (featId > -1)
                {
                    feat.Value[featId] = vsPart.Type?.ToString().ToLower().UppercaseWords();
                }

                featId = feat.Fields.FindField("Integrity");
                if (featId > -1)
                {
                    if (isAnnex14 || obstacleReport.EtodSurfaceType != EtodSurfaceType.Area1)
                        feat.Value[featId] = $"Essential";
                    else
                        feat.Value[featId] = $"Routine";
                }
                featId = feat.Fields.FindField("StValid");
                if (featId > -1)
                {
                    feat.Value[featId] = verticalStructure.TimeSlice.ValidTime?.BeginPosition;
                }

                featId = feat.Fields.FindField("EndValid");
                if (featId > -1)
                {
                    if (verticalStructure.TimeSlice.ValidTime.EndPosition.HasValue)
                        feat.Value[featId] = verticalStructure.TimeSlice.ValidTime.EndPosition;
                }

                featId = feat.Fields.FindField("StFeat");
                if (featId > -1)
                {
                    feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.BeginPosition;
                }

                featId = feat.Fields.FindField("RevDate");
                if (featId > -1)
                {
                    feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.BeginPosition;
                }

                featId = feat.Fields.FindField("EndFeat");
                if (featId > -1)
                {
                    if (verticalStructure.TimeSlice.FeatureLifetime.EndPosition.HasValue)
                        feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.EndPosition.Value;
                }

                featId = feat.Fields.FindField("Status");
                if (featId > -1)
                {
                    var status = GetFeatStatus(vsPart);
                    if (status != null)
                        feat.Value[featId] = status;
                }

                featId = feat.Fields.FindField("GeomType");
                if (featId > -1)
                {
                    feat.Value[featId] = getGeometryType(vsPartGeometry.Type);
                }

                featId = feat.Fields.FindField("HExt");
                if (featId > -1)
                {
                    if (getHExt(vsPartGeometry).HasValue)
                        feat.Value[featId] = getHExt(vsPartGeometry);
                }

                //if (isAnnex14)
                //{
                //    WriteObstacleAreaParams(feat,obstacleReport);
                //}

                feat.Store();
            }
        }

        private void WriteVerticalStructure(ITable obsClass,VerticalStructure verticalStructure)
        {
            var feat = obsClass.CreateRow();

            var featId = feat.Fields.FindField("OrigId");
            if (featId > -1)
            {
                feat.Value[featId] = GetOrigId(verticalStructure);
            }

            featId = feat.Fields.FindField("SourceId");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.Name;
            }

            featId = feat.Fields.FindField("ObsType");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.Type?.ToString().ToLower().UppercaseWords();
            }
            featId = feat.Fields.FindField("StValid");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.TimeSlice.ValidTime?.BeginPosition;
            }

            featId = feat.Fields.FindField("EndValid");
            if (featId > -1)
            {
                if (verticalStructure.TimeSlice.ValidTime.EndPosition.HasValue)
                    feat.Value[featId] = verticalStructure.TimeSlice.ValidTime.EndPosition;
            }

            featId = feat.Fields.FindField("StFeat");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.BeginPosition;
            }

            featId = feat.Fields.FindField("RevDate");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.BeginPosition;
            }

            featId = feat.Fields.FindField("EndFeat");
            if (featId > -1)
            {
                if (verticalStructure.TimeSlice.FeatureLifetime.EndPosition.HasValue)
                    feat.Value[featId] = verticalStructure.TimeSlice.FeatureLifetime.EndPosition.Value;
            }

            featId = feat.Fields.FindField("Marking");
            if (featId > -1)
            {
                if (verticalStructure.MarkingICAOStandard.HasValue)
                    feat.Value[featId] = verticalStructure.MarkingICAOStandard.Value ? "Available" : "UnAvailable";
            }

            featId = feat.Fields.FindField("Lighting");
            if (featId > -1)
            {
                if (verticalStructure.Lighted.HasValue)
                    feat.Value[featId] = verticalStructure.Lighted.Value ? "Available" : "UnAvailable";
            }

            featId = feat.Fields.FindField("LightingICAOStandard");
            if (featId > -1)
            {
                if (verticalStructure.LightingICAOStandard.HasValue)
                    feat.Value[featId] = verticalStructure.LightingICAOStandard.Value ? "Available" : "UnAvailable";
            }

            featId = feat.Fields.FindField("Grouped");
            if (featId > -1)
            {
                if (verticalStructure.Group.HasValue)
                    feat.Value[featId] = verticalStructure.Group.Value ? "Yes" : "No";
            }

            featId = feat.Fields.FindField("Interp");
            if (featId > -1)
            {
                feat.Value[featId] = "Baseline";
            }

            featId = feat.Fields.FindField("ObsId");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.Identifier.ToString();
            }

            featId = feat.Fields.FindField("IdNumber");
            if (featId > -1)
            {
                feat.Value[featId] = verticalStructure.Identifier.ToString();
            }

            featId = feat.Fields.FindField("Width");
            if (featId > -1)
            {
                var width = ConverterToSI.Convert(verticalStructure.Width, 0);
                if (Math.Abs(width) > 0.01)
                    feat.Value[featId] = width;
            }

            featId = feat.Fields.FindField("Length");
            if (featId > -1)
            {
                var length = ConverterToSI.Convert(verticalStructure.Length, 0);
                if (Math.Abs(length) > 0.01)
                    feat.Value[featId] = length;
            }

            featId = feat.Fields.FindField("Radius");
            if (featId > -1)
            {
                var radius = ConverterToSI.Convert(verticalStructure.Radius, 0);
                if (Math.Abs(radius) > 0.01)
                    feat.Value[featId] = radius;
            }

            feat.Store();
        }

        private string GetOrigId(VerticalStructure verticalStructure)
        {
            if (verticalStructure?.Name == null)
                return null;
            var vsLowerCaseName = verticalStructure.Name.ToLower();
            if (vsLowerCaseName[2] == 'o')
                return "Air Control";

            if (vsLowerCaseName.EndsWith("ob"))
                return "Kazgeodesy";

            if (vsLowerCaseName.Contains("kan"))
                return "Kazaeronavigatsiya";

            if (vsLowerCaseName.Contains("aerant"))
                return "AERANT";

            if (vsLowerCaseName.Contains("ob"))
                return "Kazgeodesy";

            if (verticalStructure.Type == CodeVerticalStructure.NAVAID)
                return "Kazaeronavigatsiya";

            return null;
        }

        private string getGeometryType(GeometryType geometryType)
        {
            if (geometryType == GeometryType.Point)
                return "Point";
            else if (geometryType == GeometryType.Polygon || geometryType == GeometryType.MultiPolygon)
                return "Polygon";
            return "Polyline";
        }

        private double? getHExt(Geometry vsPartGeometry)
        {
            double? area = null;
            if (vsPartGeometry is MultiLineString multiLine)
            {
                var multilinePrj = GlobalParams.SpatialRefOperation.ToPrj(multiLine);
                return Math.Round(multilinePrj.Length,1);
            }

            return area;
        }

        private double? getArea(Geometry vsPartGeometry)
        {
            double? area = null;
            if (vsPartGeometry is MultiPolygon multiPolygon)
            {
                var multilinePrj = GlobalParams.SpatialRefOperation.ToPrj(multiPolygon);
                return multilinePrj.Area;
            }

            return area;
        }

        private string GetFeatStatus(VerticalStructurePart verticalStructurePart)
        {
            switch (verticalStructurePart.ConstructionStatus)
            {
                case CodeStatusConstruction.COMPLETED:
                    return $"Completed";
                case CodeStatusConstruction.IN_CONSTRUCTION:
                    return $"Under Construction";
                case CodeStatusConstruction.DEMOLITION_PLANNED:
                    return $"Planned";
                default:
                    return null;
            }
        }

        private string GetVerticalDatum(CodeVerticalDatum codeVerticalDatum)
        {
            if (codeVerticalDatum == CodeVerticalDatum.OTHER_EGM_08)
                return $"EGM_08";

            return codeVerticalDatum.ToString();
            
        }

        private void SetZToGeo(Geometry geo, double height)
        {
            if (geo.Type == GeometryType.Point)
                ((Point)geo).Z = height;
            else
            {
                var mltPoint = geo.ToMultiPoint();
                if (mltPoint == null) throw new ArgumentException("Geometry type is not correct!");

                foreach (Aran.Geometries.Point pt in mltPoint)
                    pt.Z = height;
            }

        }

        private void WriteObstacleAreaParams(IFeature feat, ObstacleReport obstacleReport)
        {
            var featId = feat.Fields.FindField("ADId");

            if (obstacleReport.SurfaceType == SurfaceType.CONICAL ||
                obstacleReport.SurfaceType == SurfaceType.InnerHorizontal ||
                obstacleReport.SurfaceType == SurfaceType.OuterHorizontal)
            {
                if (featId > -1)
                {
                    feat.Value[featId] = _adhp.Identifier.ToString();
                }
            }
            else
            {
                featId = feat.Fields.FindField("RwyDirId");
                if (featId > -1)
                {
                    feat.Value[featId] = _rwyDir.RwyDir.Identifier.ToString();
                }
            }

            featId = feat.Fields.FindField("ObsAreaId");
            if (featId > -1)
            {
                var obsArea =
                    _obstacleAreas.FirstOrDefault(obs => obs.Type.ToString().ToLower().
                        Contains(obstacleReport.SurfaceType.ToString().ToLower()));
                if (obsArea != null)
                    feat.Value[featId] = obsArea.Identifier.ToString();
            }
        }
    
    }

}
