using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Vss
{
    public static class VssExtensions
    {
        public static Feature GetFeature(this FeatureRef featureRef)
        {
            if (featureRef == null)
                return null;

            int featureTypeIndex;
            if (AimMetadata.IsAbstractFeatureRef(featureRef)) {
                featureTypeIndex = ((IAbstractFeatureRef)featureRef).FeatureTypeIndex;
                return Globals.Qpi.GetFeature((FeatureType)featureTypeIndex, featureRef.Identifier);
            }
            else {

                if (featureRef.FeatureType != null)
                    return Globals.Qpi.GetFeature(featureRef.FeatureType.Value, featureRef.Identifier);

                featureTypeIndex = AimMetadata.GetAimTypeIndex(featureRef);
                return Globals.Qpi.GetFeature((FeatureType)featureTypeIndex, featureRef.Identifier);
            }
        }

        public static int ToAranRgb(this System.Drawing.Color color)
        {
            return Aran.PANDA.Common.ARANFunctions.RGB(color.R, color.G, color.B);
        }
    }
}
