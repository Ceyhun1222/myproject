using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace Aran.Omega.TypeBEsri.Models
{
    public class EsriFunctions
    {

        public static ILayer GetLayerByName(string layerName)
        {
            var upLayerName = layerName.ToUpper();
            for (int i = 0; i < GlobalParams.Map.LayerCount; i++)
            {
                var layer = GlobalParams.Map.Layer[i];
                if (layer.Name.ToUpper() == upLayerName)
                    return layer;
            }
            return null;

        }
    }
}
