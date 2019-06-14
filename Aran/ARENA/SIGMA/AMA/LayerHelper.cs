using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaChart
{
    public class LayerHelper
    {
        private IMap _focusMap;
        public LayerHelper(IMap focusMap)
        {
            _focusMap = focusMap;
            if (_focusMap == null)
                throw new ArgumentException("Map is empty!");
        }
        public ILayer GetLayerByName(string layerName)
        {
            var upLayerName = layerName.ToUpper();
            for (int i = 0; i < _focusMap.LayerCount; i++)
            {
                var layer = GlobalParams.HookHelper.FocusMap.Layer[i];
                if (layer.Name.ToUpper() == upLayerName)
                    return layer;
            }
            return null;
        }

        public List<IMapGrid> GetMapGrid()
        {
            var gridList = new List<IMapGrid>();
            var map = GlobalParams.ActiveView.FocusMap;
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            if (graphicsContainer != null)
            {
                var frameElement = graphicsContainer.FindFrame(map);
                var mapFrame = frameElement as IMapFrame;
                var mapGrids = mapFrame as IMapGrids;

                for (int i = 0; i < mapGrids.MapGridCount; i++)
                    gridList.Add(mapGrids.MapGrid[i]);
            }
            return gridList;
        }

        public List<IRasterLayer> GetRasterLayers()
        {
            var rasterLayers = new List<IRasterLayer>();
            for (int i = 0; i < _focusMap.LayerCount; i++)
            {
                ILayer layer = _focusMap.Layer[i];
                if (layer is IRasterLayer && layer.Visible)
                {
                    if (layer.Visible)
                        rasterLayers.Add((IRasterLayer)layer);
                }
            }
            return rasterLayers;
        }
    }
}
