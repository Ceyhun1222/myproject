using System;

namespace Aran.Aim
{
    public interface IAimObject
    {
        AimObjectType AimObjectType { get; }
        IAimProperty GetValue (int propertyIndex);
        void SetValue (int propertyIndex, IAimProperty value);
        int [] GetPropertyIndexes ();
    }
}
