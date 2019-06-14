using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using ARAN.Contracts.GeometryOperators;

namespace ObjectDirectory
{
    internal class Global
    {
        static Global()
        { 
            CreateGeoSp();
        }
        public static IAranEnvironment Env { get; set; }
        public static SpatialReference GeoSp { get; set; }
        private static void CreateGeoSp()
        {
            GeoSp = new SpatialReference();
            GeoSp.Name = "WGS1984";
            GeoSp.SpatialReferenceType = SpatialReferenceType.srtGeographic;
            GeoSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
            GeoSp.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
            GeoSp.Ellipsoid.SemiMajorAxis = 6378137.0;
            GeoSp.Ellipsoid.Flattening = 1 / 298.25722356300003;
        }

        
    }
}
