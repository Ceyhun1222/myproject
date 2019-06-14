using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Interface;

namespace Aran.Temporality.Common.Util.TypeUtil
{
    internal class KnownTypeUtil : ITypeUtil
    {
        public static List<Type> KnownTypes = new List<Type>();

        #region ITypeUtil Members

        public int GetFeatureType(Type type)
        {
            return KnownTypes.IndexOf(type) + 1;
        }

        public string GetTypeName(int featureType)
        {
            if (featureType == 0) return "All";
            return KnownTypes[featureType - 1].Name;
        }

        #endregion
    }
}