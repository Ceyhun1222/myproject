using System;

namespace Aran.Temporality.Common.Interface
{
    public interface ITypeUtil
    {
        int GetFeatureType(Type type);
        string GetTypeName(int featureType);
    }
}