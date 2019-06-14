using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    public class VisualFeatureCreatorHelper
    {
        public void DeleteAllVisualFeaturesDrawings()
        {
            for (int i = 0; i < VMManager.Instance.AllVisualFeatureElements.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.AllVisualFeatureElements[i]);
                VMManager.Instance.AllVisualFeatureElements.RemoveAt(0);
                i--;
            }
        }

        public void DrawAllVisualFeatures()
        {
            foreach (VM_VisualFeature vf in VMManager.Instance.AllVisualFeatures)
            {
                int elem = GlobalVars.gAranEnv.Graphics.DrawPointWithText(vf.pShape, ARANFunctions.RGB(0, 172, 237), vf.Name);
                VMManager.Instance.AllVisualFeatureElements.Add(elem);
            }
        }
    }
}
