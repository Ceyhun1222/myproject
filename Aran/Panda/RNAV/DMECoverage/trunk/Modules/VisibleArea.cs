using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.RNAV.DMECoverage.Modules
{
    class VisibleArea
    {
        private RasterSurfaceClass _rasterSurface;

        public VisibleArea()
        {
            _rasterSurface = new RasterSurfaceClass();
            LoadRasterLayer();

            if (RasterLayers.Count>0)
                _rasterSurface.PutRaster(RasterLayers[0].Raster,0);

        }

        public List<IRasterLayer> RasterLayers { get; private set; }

        public void CalculateLineOfSight(Aran.Geometries.Point source, Aran.Geometries.Point target)
        {
            ISurface surface =_rasterSurface as ISurface;

            var esriSource = new ESRI.ArcGIS.Geometry.PointClass
            {
                X = source.X,
                Y = source.Y,
                Z = source.Z
            };

            var esriTarget = new ESRI.ArcGIS.Geometry.PointClass
            {
                X = target.X,
                Y = target.Y,
                Z = target.Z
            };



            IPoint ppObstruction;
            IPolyline ppInvisibleLines, ppVisibleLines;
            bool isVisible;

            _rasterSurface.GetLineOfSight(esriSource,esriTarget,out ppObstruction,out ppVisibleLines, out ppInvisibleLines,out isVisible,true,true);

        }

        private void LoadRasterLayer()
        {
            RasterLayers = new List<IRasterLayer>();
            var layers = GlobalVars.gAranEnv.GetAllLayers;
            foreach (object lay in layers)
            {
                var layer = lay as IRasterLayer;
                if (layer != null)
                {
                    try
                    {
                        if (layer.Visible)
                            RasterLayers.Add(layer);
                    }
                    catch(Exception e)
                    {
                        GlobalVars.gAranEnv.GetLogger("DmeDmeCoverage").Error(e.Message);
                    }
                }
            }
        }
    }
}
