using ESRI.ArcGIS.Controls;

namespace EnrouteChartCompare.Helper
{
    internal class GlobalParams
    {
        private static SpatialReferenceOperation _spatialReferenceOperation;
        public static IHookHelper HookHelper { get; set; }

        public static SpatialReferenceOperation SpatialOperation => _spatialReferenceOperation ?? (_spatialReferenceOperation = new SpatialReferenceOperation());

        public static Graphics Graphics { get; set; }
    }
}