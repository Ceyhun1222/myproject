using Aran.AranEnvironment;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace Aran.AimEnvironment.Tools
{
    class Context
    {
        public static IHookHelper HookHelper { get; private set; }
        public static IMap Map { get; private set; }
        public static IActiveView ActiveView { get; private set; }
        public static SpatialReferenceOperation SpatialReferenceOperation { get; private set; }

        public static void Load(IAranEnvironment aranEnv)
        {
            HookHelper = new HookHelper();
            HookHelper.Hook = aranEnv.HookObject;

            SpatialReferenceOperation = new SpatialReferenceOperation(aranEnv);
            Map = HookHelper.FocusMap;
            ActiveView = HookHelper.ActiveView;
        }
    }
}
