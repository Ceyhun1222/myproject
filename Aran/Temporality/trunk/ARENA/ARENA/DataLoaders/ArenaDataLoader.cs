using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace ARENA.DataLoaders
{

    public interface IARENA_DATA_Converter
    {
        bool Convert_Data(IFeatureClass _FeatureClass);
    }

  


}
