using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace Aran.AimEnvironment.Tools
{
    public class SnapClass
    {

        private static SnapClass _snapClass = null;

        public static SnapClass Current
        {
            get {
                if (_snapClass == null)
                    _snapClass = new SnapClass();
                return _snapClass;
            }
        }

        public IPointSnapper Snapper { get; private set; }

        public ISnappingFeedback SnappingFeedback { get; private set; }

        public ISnappingEnvironment SnapEnvoirment { get; set; }

        private SnapClass()
        {
            IHookHelper2 hookHelper2 = Context.HookHelper as IHookHelper2;

            IExtensionManager extensionManager = hookHelper2.ExtensionManager;

            if (extensionManager != null)
            {
                UID guid = new UIDClass();
                guid.Value = "{E07B4C52-C894-4558-B8D4-D4050018D1DA}"; //Snapping extension.
                IExtension extension = extensionManager.FindExtension(guid);
                SnapEnvoirment = extension as ISnappingEnvironment;

                //ISet excludedLayerSet = new SetClass();
                //excludedLayerSet.Add(Globals.MainForm.AimLayers[0].Layer as IFeatureLayer);

                Snapper = SnapEnvoirment.PointSnapper;

                // m_Snapper.ExcludedLayers(ref excludedLayerSet);

                SnappingFeedback = new SnappingFeedbackClass();
                SnapEnvoirment.SnappingType = esriSnappingType.esriSnappingTypePoint | esriSnappingType.esriSnappingTypeVertex;
                SnapEnvoirment.ShowSnapTips = false;
                SnapEnvoirment.SnapTipSymbol.Text = "";
                SnapEnvoirment.TextSnapping = false;
                SnapEnvoirment.Tolerance = 5;
                SnappingFeedback.Initialize(Context.HookHelper.Hook, SnapEnvoirment, true);
            }
        }
    }
}
