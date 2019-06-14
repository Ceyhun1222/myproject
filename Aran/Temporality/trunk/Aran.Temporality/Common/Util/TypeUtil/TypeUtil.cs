using System;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Util.TypeUtil
{
    public class TypeUtil
    {
        public static ITypeUtil CurrentTypeUtil = new KnownTypeUtil();

        public static int GetFeatureType(Type type)
        {
            return CurrentTypeUtil.GetFeatureType(type);
        }

        public static string GetTypeName(int featureType)
        {
            return CurrentTypeUtil.GetTypeName(featureType);
        }
    }
}