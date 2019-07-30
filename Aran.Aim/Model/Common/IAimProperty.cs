using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aixm;
using Aran.Package;

namespace Aran.Aim
{
    public interface IAimProperty
    {
        AimPropertyType PropertyType { get; }
        IAixmSerializable GetAixmSerializable ();
        IPackable GetPackable ();
    }

    public enum AimPropertyType
    {
        AranField = AimObjectType.Field,
        DataType = AimObjectType.DataType,
        Object = AimObjectType.Object,
        List = AimObjectType.Object + 1
    }


}
