using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ChartTypeA.Models;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using PDM;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Utils
{
    class GdbReader
    {
        public static List<RunwayCenterLinePoint> GetRunwayCenterlineList()
        {
            var result = new List<RunwayCenterLinePoint>();
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "RwyPoint");
            if (layer == null) throw new Exception("RwyCenterPoint layer not found!");

            var fc = ((IFeatureLayer)layer).FeatureClass;

            var featCursor = fc.Search(null, true);
            IFeature feat = featCursor.NextFeature();
            while (feat != null)
            {
                var rwyCenterPoint = new RunwayCenterLinePoint();
                var fields = feat.Fields;
                rwyCenterPoint.Designator = (string)feat.get_Value(2);
                CodeRunwayCenterLinePointRoleType role;
                if (Enum.TryParse((string)feat.get_Value(3), out role))
                    rwyCenterPoint.Role = role;

                UOM_DIST_VERT uomElev;
                if (Enum.TryParse((string)feat.get_Value(5), out uomElev))
                    rwyCenterPoint.Elev_UOM = uomElev;

                rwyCenterPoint.Elev = (double)feat.get_Value(6);

                rwyCenterPoint.Geo = feat.ShapeCopy;
                rwyCenterPoint.X = (feat.Shape as IPoint).X;
                rwyCenterPoint.Y = (feat.Shape as IPoint).Y;
                result.Add(rwyCenterPoint);
                feat = featCursor.NextFeature();
            }

            Marshal.ReleaseComObject(featCursor);

            return result;
        }

        public static List<ObstacleReport> GetObstacles()
        {
            var result = new List<ObstacleReport>();
            ILayer layer = EsriUtils.getLayerByName(GlobalParams.Map, "VerticalStructurePoint");
            if (layer == null) throw new Exception("Obstacle Point layer not found!");

            ILayer layerPolyline = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureCurve");
            if (layerPolyline == null) throw new Exception("ObstacleCurve layer not found!");

            ILayer layerPolygon = EsriUtils.getLayerByName(GlobalParams.Map, "VertStructureSurface");
            if (layerPolygon == null) throw new Exception("ObstacleSurface layer not found!");

            var fc = ((IFeatureLayer)layer).FeatureClass;
            var fcPolyline = ((IFeatureLayer)layerPolyline).FeatureClass;
            var fcPolygon = ((IFeatureLayer)layerPolygon).FeatureClass;

            var featClass = fc;
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                    featClass = fcPolyline;
                else if (i == 2)
                    featClass = fcPolygon;

                var featCursor = featClass.Search(null, true);
                IFeature feat = featCursor.NextFeature();
                while (feat != null)
                {
                    var obstacleReport = new ObstacleReport();
                    var fields = feat.Fields;
                    obstacleReport.Name = (string)feat.get_Value(fields.FindField("NAME"));
                    obstacleReport.Elevation = (double)feat.get_Value(6);

                    var geo = GlobalParams.SpatialRefOperation.ToEsriPrj(feat.Shape);
                    obstacleReport.GeomPrj = geo;
                    result.Add(obstacleReport);
                    feat = featCursor.NextFeature();
                }
            }
            return result;

        }
    }
}
