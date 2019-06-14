using System;

namespace Aran.Package
{
    public enum PackedTypeInfo
    {
        Null = 0,
        Byte = TypeCode.Byte,
        Boolean = TypeCode.Boolean,
        Int32 = TypeCode.Int32,
        Int64 = TypeCode.Int64,
        Double = TypeCode.Double,
        String = TypeCode.String,
        AranObject = 50,
        Geometry,
        List
    }
}
