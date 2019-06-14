using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEnv.Layers
{
    public static class LayersGlobals
    {
        public static ISpatialReference MapSpatialReference
        {
            get { return Globals.MainForm.Map.SpatialReference; }
        }
    }
}
