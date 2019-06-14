using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Omega.Models;
using Aran.Omega.SettingsUI;
using Aran.PANDA.Common;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;
using Polygon = Aran.Geometries.Polygon;
using Ring = Aran.Geometries.Ring;

namespace Aran.Omega
{
    public static class CommonFunctions
    {
        public static PlaneParam CalcPlaneParam(Aran.Geometries.Point pt1, Aran.Geometries.Point pt2, Aran.Geometries.Point pt3)
        {
            PlaneParam fPlane = new PlaneParam();
            fPlane.C = pt1.X * (pt2.Y - pt3.Y) + pt2.X * (pt3.Y - pt1.Y) + pt3.X * (pt1.Y - pt2.Y);
            fPlane.A = (pt1.Y * (pt2.Z - pt3.Z) + pt2.Y * (pt3.Z - pt1.Z) + pt3.Y * (pt1.Z - pt2.Z))/fPlane.C;
            fPlane.B = (pt1.Z * (pt2.X - pt3.X) + pt2.Z * (pt3.X - pt1.X) + pt3.Z * (pt1.X - pt2.X))/fPlane.C;
            fPlane.D = (-(pt1.X * (pt2.Y * pt3.Z - pt3.Y * pt2.Z) + pt2.X * (pt3.Y * pt1.Z - pt1.Y * pt3.Z) + pt3.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z)))/fPlane.C;
            return fPlane;
        }

        public static PlaneParam CalcPlaneParamFromPrjPts(Aran.Geometries.Point center,double axis ,Aran.Geometries.Point ptPrj1, Aran.Geometries.Point ptPrj2, Aran.Geometries.Point ptPrj3)
        {
            Aran.Geometries.Point pt1= ARANFunctions.PrjToLocal(center, axis, ptPrj1);
            Aran.Geometries.Point pt2 = ARANFunctions.PrjToLocal(center, axis, ptPrj2);
            Aran.Geometries.Point pt3 = ARANFunctions.PrjToLocal(center, axis, ptPrj3);

            PlaneParam fPlane = new PlaneParam();
            fPlane.C = pt1.X * (pt2.Y - pt3.Y) + pt2.X * (pt3.Y - pt1.Y) + pt3.X * (pt1.Y - pt2.Y);
            fPlane.A = (pt1.Y * (pt2.Z - pt3.Z) + pt2.Y * (pt3.Z - pt1.Z) + pt3.Y * (pt1.Z - pt2.Z)) / fPlane.C;
            fPlane.B = (pt1.Z * (pt2.X - pt3.X) + pt2.Z * (pt3.X - pt1.X) + pt3.Z * (pt1.X - pt2.X)) / fPlane.C;
            fPlane.D = (-(pt1.X * (pt2.Y * pt3.Z - pt3.Y * pt2.Z) + pt2.X * (pt3.Y * pt1.Z - pt1.Y * pt3.Z) + pt3.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z))) / fPlane.C;
            return fPlane;
        }

        public static PlaneParam CalcPlaneParamY(Aran.Geometries.Point pt1,Aran.Geometries.Point pt2,double yCoef)
        {
            PlaneParam planeParam =new PlaneParam();
            planeParam.A= (pt2.Z - pt1.Z + yCoef * Math.Abs(pt1.Y) - yCoef * Math.Abs(pt2.Y)) / (pt2.X - pt1.X);
            planeParam.B = yCoef;
            planeParam.C = -1;
            planeParam.D = pt1.Z - yCoef * Math.Abs(pt1.Y) - pt1.X * planeParam.A;
            return planeParam;
        }

        public static Aran.Geometries.MultiPolygon CreateMultipolygonFromPoints(Aran.Geometries.Point[] points)
        {
            var ring = new Ring();
            foreach (var pt in points)
            {
                ring.Add(pt);
            }
            return  new MultiPolygon { new Polygon { ExteriorRing = ring } };
        }

        public static Aran.Geometries.Point GetXyPoint(Aran.Geometries.Point pt, Aran.Geometries.Point ptBase,double direction)
        {
            var ptResult = ARANFunctions.PrjToLocal(ptBase, direction, pt);
            return ptResult;
        }

        public static Aran.Omega.SettingsUI.SurfaceModel GetSurfaceModel(Aran.PANDA.Constants.SurfaceType surfaceType)
        {
            var surfaceModel = (SurfaceModel)GlobalParams.Settings.OLSModelList.FirstOrDefault(x =>
            {
                var sur = x as SurfaceModel;
                if (sur != null && sur.Surface == surfaceType)
                    return true;

                return false;
            });
            if (surfaceModel == null)
                return (SurfaceModel)GlobalParams.Settings.OLSModelList[3];
            return surfaceModel;
        }

        public static Aran.Geometries.Polygon CreateExtent(double minX, double minY, double maxX, double maxY)
        {
            var result = new Aran.Geometries.Polygon();
            var ring = new Aran.Geometries.Ring
            {
                new Aran.Geometries.Point(minX, minY),
                new Aran.Geometries.Point(minX, maxY),
                new Aran.Geometries.Point(maxX, maxY),
                new Aran.Geometries.Point(maxX, minY)
            };
            result.ExteriorRing = ring;
            return result;

        }

        public static double GetDistance(Aran.Geometries.Point ptRefPt, Aran.Geometries.MultiPoint mltPoint)
        {
            var ptToSegmentDistance = ARANFunctions.PointToSegmentDistance(ptRefPt, mltPoint[0], mltPoint[1]);
            var minDistance = ptToSegmentDistance;
            for (var i = 1; i < mltPoint.Count - 1; i++)
            {
                ptToSegmentDistance = ARANFunctions.PointToSegmentDistance(ptRefPt, mltPoint[i],
                    mltPoint[i + 1]);
             
                if (ptToSegmentDistance < minDistance && Math.Abs(ptToSegmentDistance)>0.00001)
                    minDistance = ptToSegmentDistance;
            }
            return minDistance;
        }

        public static double GetDistance(Aran.Geometries.MultiPolygon multiPolygon, Aran.Geometries.MultiPoint mltPoint)
        {
            var distance = (from Polygon polygon in multiPolygon
                from Point pt in polygon.ExteriorRing
                select CommonFunctions.GetDistance(pt, mltPoint)).DefaultIfEmpty().Min();

            return distance;
        }

        public static double GetDistance(Aran.Geometries.MultiLineString multiLineString,
            Aran.Geometries.MultiPoint mltPoint)
        {
            var distance = (from LineString lineString in multiLineString
                from Point pt in lineString
                select CommonFunctions.GetDistance(pt, mltPoint)).DefaultIfEmpty().Min();

            return distance;
        }

        public static void GetVerticalHorizontalAccuracy(VerticalStructurePartGeometry horizontalProj, ref double verAccuracy, ref double horAccuracy)
        {
            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
            {
                verAccuracy = ConverterToSI.Convert(horizontalProj.Location.VerticalAccuracy, verAccuracy);
                horAccuracy = ConverterToSI.Convert(horizontalProj.Location.HorizontalAccuracy, horAccuracy);
            }
            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
            {
                verAccuracy = ConverterToSI.Convert(horizontalProj.LinearExtent.VerticalAccuracy, verAccuracy);
                horAccuracy = ConverterToSI.Convert(horizontalProj.LinearExtent.HorizontalAccuracy, horAccuracy);
            }
            else
            {
                verAccuracy = ConverterToSI.Convert(horizontalProj.SurfaceExtent.VerticalAccuracy, verAccuracy);
                horAccuracy = ConverterToSI.Convert(horizontalProj.SurfaceExtent.HorizontalAccuracy, horAccuracy);
            }
        }

        public static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }

        public static IFeatureClass CreateVsFeatureClass(string folderPath, esriGeometryType geometryType, string shapeName)
        {
            var spatRefConverter = new SpatRefConverter();
            var spGeo = spatRefConverter.ToEsriSpatRef(GlobalParams.SpatialRefOperation.SpRefPrj);

            var folder = folderPath;
            var nameOfShapeFile = shapeName;
            var shapeFieldName = "Shape";

            IWorkspaceFactory workspaceFactory = null;
            workspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace workspace = workspaceFactory.OpenFromFile(folder, 0);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

            IFields ptFields = new Fields();
            IFieldsEdit fieldsEdit = (IFieldsEdit)ptFields;
            // fieldsEdit.FieldCount_2 = 2 + SurfaceBase.PropertyList.Count;

            IField field = new Field(); ///###########
            IFieldEdit fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = "Shape";
            fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

            IGeometryDef geomDef = new GeometryDef(); ///#########
            IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
            geomDefEdit.HasZ_2 = true;

            geomDefEdit.GeometryType_2 = geometryType;
            geomDefEdit.SpatialReference_2 = spGeo;

            fieldEdit.GeometryDef_2 = geomDef;
            fieldsEdit.AddField(field);

            IField tmpField = new Field();
            IFieldEdit tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Name";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "FeatureId";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeInteger);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Vs Type";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Penetrate";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeDouble);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Surface";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "X";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Y";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);

            tmpField = new Field();
            tmpFieldEdit = (IFieldEdit)tmpField;
            tmpFieldEdit.Name_2 = "Elevation";
            tmpFieldEdit.Type_2 = (esriFieldType.esriFieldTypeString);
            fieldsEdit.AddField(tmpField);


            //Add another miscellaneous text field

            IFeatureClass featClass = null;
            featClass = featureWorkspace.CreateFeatureClass(nameOfShapeFile, ptFields, null, null,
                esriFeatureType.esriFTSimple, shapeFieldName, "");

            return featClass;

            //IPointCollection ptColl = new ESRI.ArcGIS.Geometry.Ring();
        }


        public static async Task GetandConvertVsToLocalParam()
        {
            List<VerticalStructure> vsList = await GlobalParams.Database.GetVerticalStructureListAsync(GlobalParams.Database.AirportHeliport.ARP.Geo, GlobalParams.Settings.OLSQuery.Radius);
            CommonFunctions.AnalysesAirportVs(vsList);
        }


        public static void AnalysesAirportVs(List<VerticalStructure> vsList)
        {
            int i = 0;
            foreach (var vs in vsList)
            {
                try
                {
                    i++;
                    int partNumber = -1;
                    foreach (var vsPart in vs.Part)
                    {
                        partNumber++;
                        if (vsPart.HorizontalProjection == null)
                            continue;
                        if (vsPart.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                        {
                            var pt = GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo);
                            // var esriGeom = ConvertToEsriGeom.FromGeometry(pt);

                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0));
                            if (!pt.IsEmpty)
                            {
                                vs.SetGeom(partNumber, pt);

                                var jtsGeom = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromGeometry(pt);
                                vs.SetJtsGeom(partNumber, jtsGeom);

                                var horAccuracy = Aran.Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.Location.HorizontalAccuracy, 0);
                                if (Math.Abs(horAccuracy) > 0.01)
                                {
                                    var buffer = jtsGeom.Buffer(horAccuracy);
                                    vs.SetJtsBuffer(partNumber, buffer);
                                }
                            }
                        }
                        else if (vsPart.HorizontalProjection.Choice ==
                                 VerticalStructurePartGeometryChoice.ElevatedSurface)
                        {
                            var surface =
                                GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo);


                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0));
                            vs.SetGeom(partNumber, surface);

                            var jtsGeom = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromGeometry(surface);
                            vs.SetJtsGeom(partNumber, jtsGeom);

                            //GlobalParams.UI.DrawMultiPolygon(surface, 1, Aran.AranEnvironment.Symbols.eFillStyle.sfsHorizontal);

                            var horAccuracy = Aran.Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.HorizontalAccuracy, 0);
                            if (Math.Abs(horAccuracy) > 0.01)
                            {
                                var buffer = jtsGeom.Buffer(horAccuracy);
                                vs.SetJtsBuffer(partNumber, buffer);
                            }
                        }
                        else if (vsPart.HorizontalProjection.Choice ==
                                 VerticalStructurePartGeometryChoice.ElevatedCurve)
                        {
                            var curve =
                                GlobalParams.SpatialRefOperation.ToPrj(
                                    vsPart.HorizontalProjection.LinearExtent.Geo);

                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0));
                            vs.SetGeom(partNumber, curve);

                            var jtsGeom = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromGeometry(curve);
                            vs.SetJtsGeom(partNumber, jtsGeom);

                            var horAccuracy = Aran.Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.HorizontalAccuracy, 0);
                            if (Math.Abs(horAccuracy) > 0.01)
                            {
                                var buffer = jtsGeom.Buffer(horAccuracy);
                                vs.SetJtsBuffer(partNumber, buffer);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error happened when analayze Obstacles!");
                    throw e;
                }
            }
            GlobalParams.AdhpObstacleList = vsList;
        }


        public static ObstacleGeomType GetGeomType(Geometry geo)
        {
            if (geo.Type == GeometryType.Point)
                return ObstacleGeomType.Point;
            else if (geo.Type == GeometryType.MultiPolygon)
                return ObstacleGeomType.Polygon;

            return ObstacleGeomType.PolyLine;
        }

    }
}
