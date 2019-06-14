using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim.Data.Filters;
using Aran.Package;
using ESRI.ArcGIS.Display;
using Aran.Aim;

namespace MapEnv.Layers
{
    public static class ClassExtensions
    {
        public static IGeometry Clone(this IGeometry geom)
        {
            return (IGeometry)((IClone)geom).Clone();
        }
    }

    public static class LayerPackage
    {
        //public static void Pack(this Filter filter, PackageWriter writer)
        //{
        //    Pack(filter.Operation, writer);
        //}

        //public static Filter UnpackFilter(PackageReader reader)
        //{
        //    var operChoice = UnpackOperationChoice(reader);
        //    return new Filter(operChoice);
        //}

        public static void Pack(this IPersistStream perStream, PackageWriter writer)
        {
            IMemoryBlobStream memBlobStream = new MemoryBlobStream();
            perStream.Save(memBlobStream, 0);

            IMemoryBlobStreamVariant mbsv = memBlobStream as IMemoryBlobStreamVariant;
            object memObjVal;
            mbsv.ExportToVariant(out memObjVal);

            Guid guid;
            perStream.GetClassID(out guid);

            byte[] buffer = (byte[])memObjVal;
            int count = buffer.Length;
            writer.PutString(guid.ToString("B"));
            writer.PutInt32(count);
            for (int i = 0; i < count; i++)
                writer.PutByte(buffer[i]);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(memBlobStream);
        }

        public static IPersistStream UnpackPersistStream(PackageReader reader)
        {
            string s = reader.GetString();
            int count = reader.GetInt32();
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
                buffer[i] = reader.GetByte();

            Guid guid = new Guid(s);
            Type symbolObjType = Type.GetTypeFromCLSID(guid);
            var perObj = Activator.CreateInstance(symbolObjType, true);
            IPersistStream perStream = perObj as IPersistStream;

            IMemoryBlobStream memBlobStream = new MemoryBlobStream();
            IMemoryBlobStreamVariant mbsv = memBlobStream as IMemoryBlobStreamVariant;
            mbsv.ImportFromVariant(buffer);

            perStream.Load(memBlobStream);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(memBlobStream);

            return perStream;
        }


        //private static void Pack(OperationChoice operChoice, PackageWriter writer)
        //{
        //    writer.PutEnum<OperationChoiceType>(operChoice.Choice);

        //    switch (operChoice.Choice) {
        //        case OperationChoiceType.Comparison:
        //            Pack(operChoice.ComparisonOps, writer);
        //            break;
        //        case OperationChoiceType.Logic:
        //            Pack(operChoice.LogicOps as BinaryLogicOp, writer);
        //            break;
        //        case OperationChoiceType.Spatial:
        //            Pack(operChoice.SpatialOps, writer);
        //            break;
        //    }
        //}

        //private static OperationChoice UnpackOperationChoice(PackageReader reader)
        //{
        //    var oct = (OperationChoiceType)reader.GetInt32();

        //    switch (oct) {
        //        case OperationChoiceType.Comparison:
        //            ComparisonOps compOp = new ComparisonOps();
        //            Unpack(compOp, reader);
        //            return new OperationChoice(compOp);
        //        case OperationChoiceType.Logic:
        //            BinaryLogicOp binLogicOp = new BinaryLogicOp();
        //            Unpack(binLogicOp, reader);
        //            return new OperationChoice(binLogicOp);
        //        case OperationChoiceType.Spatial:
        //            SpatialOps spOp = UnpackSpatialOps(reader);
        //            return new OperationChoice(spOp);
        //    }

        //    return null;
        //}

        //private static void Pack(ComparisonOps compOp, PackageWriter writer)
        //{
        //    writer.PutInt32((int)compOp.OperationType);
        //    writer.PutString(compOp.PropertyName);
        //    PackObject(compOp.Value, writer);
        //}

        //private static void Unpack(ComparisonOps compOp, PackageReader reader)
        //{
        //    compOp.OperationType = (ComparisonOpType)reader.GetInt32();
        //    compOp.PropertyName = reader.GetString();
        //    compOp.Value = UnpackObject(reader);
        //}

        //private static void Pack(SpatialOps spOp, PackageWriter writer)
        //{
        //    writer.PutString(spOp.GetType().Name);
        //    writer.PutString(spOp.PropertyName);

        //    if (spOp is DWithin) {
        //        var dwithin = spOp as DWithin;
        //        var isDistanceNull = (dwithin.Distance == null);
        //        writer.PutBool(isDistanceNull);
        //        if (!isDistanceNull)
        //            (dwithin.Distance as IPackable).Pack(writer);
        //        var isGeomNull = (dwithin.Geometry == null);
        //        writer.PutBool(isGeomNull);
        //        if (!isGeomNull) {
        //            writer.PutEnum<Aran.Geometries.GeometryType>(dwithin.Geometry.Type);
        //            dwithin.Geometry.Pack(writer);
        //        }
        //    }
        //    else if (spOp is Within) {
        //        var within = spOp as Within;

        //        var isGeomNull = (within.Geometry == null);
        //        writer.PutBool(isGeomNull);
        //        if (!isGeomNull) {
        //            writer.PutEnum<Aran.Geometries.GeometryType>(within.Geometry.Type);
        //            within.Geometry.Pack(writer);
        //        }
        //    }
        //}

        //private static SpatialOps UnpackSpatialOps(PackageReader reader)
        //{
        //    var className = reader.GetString();
        //    var propName = reader.GetString();

        //    if (className == "DWithin") {
        //        var dwithin = new DWithin();
        //        dwithin.PropertyName = propName;
        //        var isDistanceNull = reader.GetBool();
        //        if (!isDistanceNull){
        //            dwithin.Distance = new Aran.Aim.DataTypes.ValDistance();
        //            (dwithin.Distance as IPackable).Unpack(reader);
        //        }
        //        var isGeomNull = reader.GetBool();
        //        if (!isGeomNull) {
        //            var geomType = reader.GetEnum<Aran.Geometries.GeometryType>();
        //            dwithin.Geometry = Aran.Geometries.Geometry.Create(geomType);
        //            dwithin.Geometry.Unpack(reader);
        //        }
        //    }
        //    else if (className == "Within") {
        //        var within = new Within();
        //        var isGeomNull = reader.GetBool();
        //        if (!isGeomNull) {
        //            var geomType = reader.GetEnum<Aran.Geometries.GeometryType>();
        //            within.Geometry = Aran.Geometries.Geometry.Create(geomType);
        //            within.Geometry.Unpack(reader);
        //        }
        //    }

        //    return null;
        //}

        //private static void Pack(BinaryLogicOp logicOp, PackageWriter writer)
        //{
        //    writer.PutEnum<BinaryLogicOpType>(logicOp.Type);
        //    writer.PutInt32(logicOp.OperationList.Count);
        //    foreach (var operChoice in logicOp.OperationList)
        //        Pack(operChoice, writer);
        //}

        //private static void Unpack(BinaryLogicOp binLogicOp, PackageReader reader)
        //{
        //    binLogicOp.Type = reader.GetEnum<BinaryLogicOpType>();
        //    int count = reader.GetInt32();
        //    for (int i = 0; i < count; i++) {
        //        var operChoice = UnpackOperationChoice(reader);
        //        binLogicOp.OperationList.Add(operChoice);
        //    }
        //}

        //private static void PackObject(object value, PackageWriter writer)
        //{
        //    bool notNull = (value != null);
        //    writer.PutBool(notNull);
        //    if (!notNull)
        //        return;

        //    if (value is IConvertible) {
        //        IConvertible cnv = value as IConvertible;
        //        TypeCode typeCode = cnv.GetTypeCode();
        //        writer.PutInt32((int)typeCode);

        //        switch (typeCode) {
        //            case TypeCode.Char:
        //                writer.PutInt32((int)value);
        //                break;
        //            case TypeCode.String:
        //                writer.PutString((string)value);
        //                break;
        //            case TypeCode.DateTime:
        //                writer.PutDateTime((DateTime)value);
        //                break;
        //            case TypeCode.DBNull:
        //            case TypeCode.Empty:
        //            case TypeCode.Object:
        //                break;
        //            default:
        //                double dv = Convert.ToDouble(value);
        //                writer.PutDouble(dv);
        //                break;
        //        }
        //    }
        //    else if (value is Guid) {
        //        writer.PutInt32(100);
        //        writer.PutString(((Guid)value).ToString("B"));
        //    }
        //    else if (value is AimObject) {
        //        writer.PutInt32(101);
        //        var aimObj = value as AimObject;
        //        int aimTypeIndex = AimMetadata.GetAimTypeIndex(aimObj);
        //        writer.PutInt32(aimTypeIndex);
        //        (aimObj as IPackable).Pack(writer);
        //    }
        //    else {
        //        throw new Exception("Value not supported for pack");
        //    }
        //}

        //private static object UnpackObject(PackageReader reader)
        //{
        //    bool notNull = reader.GetBool();
        //    if (!notNull)
        //        return null;

        //    int typeCodeIndex = reader.GetInt32();

        //    if (typeCodeIndex == 100) {
        //        string s = reader.GetString();
        //        return new Guid(s);
        //    }
        //    else if (typeCodeIndex == 101) {
        //        int aimTypeIndex = reader.GetInt32();
        //        AimObject aimObj = Aran.Aim.AimObjectFactory.Create(aimTypeIndex);
        //        (aimObj as IPackable).Unpack(reader);
        //        return aimObj;
        //    }
        //    else {
        //        TypeCode typeCode = (TypeCode)typeCodeIndex;
        //        switch (typeCode) {
        //            case TypeCode.Char:
        //                return (char)reader.GetInt32();
        //            case TypeCode.String:
        //                return reader.GetString();
        //            case TypeCode.DateTime:
        //                return reader.GetDateTime();
        //            case TypeCode.DBNull:
        //            case TypeCode.Empty:
        //            case TypeCode.Object:
        //                return null;
        //            default:
        //                double dv = reader.GetDouble();
        //                return Convert.ChangeType(dv, typeCode);
        //        }
        //    }
        //}
    }
}
