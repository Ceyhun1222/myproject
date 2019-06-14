using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Utilities;
using Aran.Converters;
using Aran.Aim;
using Aran.Aim.Features;

namespace MapEnv.Layers
{
    public class AimComplexTable
    {
        public AimComplexTable ()
        {
            ShapeInfoList = new List<TableShapeInfo> ();
            Rows = new List<AimComplexRow> ();

            _mapSpatialReference = LayersGlobals.MapSpatialReference;
            //Globals.Environment.MapSpatialReferenceChanged += new EventHandler (Map_SpatialReferenceChanged);
        }

        public FeatureType FeatureType { get; set; }

        public List<AimComplexRow> Rows { get; private set; }

        public List<TableShapeInfo> ShapeInfoList { get; private set;}

        public List<Feature> GetAllFeatures()
        {
            var featureList = new List<Feature>();
            GetFeatureInComplex(this, featureList);
            return featureList;
        }

        private static void GetFeatureInComplex(AimComplexTable compTable, List<Feature> featureList)
        {
            foreach (var row in compTable.Rows) {
                featureList.Add(row.Row.AimFeature);

                foreach (var item in row.SubQueryList)
                    GetFeatureInComplex(item.ComplexTable, featureList);

                foreach (var item in row.RefQueryList)
                    GetFeatureInComplex(item.ComplexTable, featureList);
            }
        }

        private ISpatialReference _mapSpatialReference;
    }

    public class AimComplexRow
    {
        public AimComplexRow ()
        {
            SubQueryList = new List<AimSubComplex> ();
            RefQueryList = new List<AimRefComplex> ();
        }

        public AimRow Row { get; set; }

        public List<AimSubComplex> SubQueryList { get; private set; }

        public List<AimRefComplex> RefQueryList { get; private set; }
    }

    public class AimSubComplex
    {
        public string PropertyPath { get; set; }

        public AimComplexTable ComplexTable { get; set; }
    }

    public class AimRefComplex
    {
        public string PropertyPath { get; set; }

        public FeatureType FeatureType { get; set; }

        public AimComplexTable ComplexTable { get; set; }
    }
}
