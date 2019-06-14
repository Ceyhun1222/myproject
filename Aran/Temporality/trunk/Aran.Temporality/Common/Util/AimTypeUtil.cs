#region

using System;
using Aran.Aim;
using Aran.Temporality.Common.Interface;

#endregion

namespace Aran.Temporality.Common.Util
{
    public class AimTypeUtil : ITypeUtil
    {
        #region Implementation of ITypeUtil

        public int GetFeatureType(Type type)
        {
            return (int) System.Enum.Parse(typeof (FeatureType), type.Name);
        }

        public string GetTypeName(int featureType)
        {
            return System.Enum.ToObject(typeof (FeatureType), featureType).ToString();
        }

        #endregion
    }
}