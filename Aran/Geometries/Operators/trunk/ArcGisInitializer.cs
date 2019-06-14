using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Geometries.Operators
{
    public static class ArcGisInitializer
    {
        public static void Init()
        {
            RuntimeManager.Bind(ProductCode.Desktop);
            AoInitialize ao = new AoInitialize();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);
        }
    }
}
