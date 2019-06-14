
using Aran.Package;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Aran.Aim.Data.Filters
{
    [Serializable]
    public class ComparisonOps : IPackable
    {
        public ComparisonOps()
        {
        }

        public ComparisonOps(ComparisonOpType operationType,
            string propertyName, object value = null)
        {
            OperationType = operationType;
            PropertyName = propertyName;
            Value = value;
        }

        public ComparisonOpType OperationType
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32((int)OperationType);
            writer.PutString(PropertyName);
            PackObject(Value, writer);
        }

        public void Unpack(PackageReader reader)
        {
            OperationType = (ComparisonOpType)reader.GetInt32();
            PropertyName = reader.GetString();
            Value = UnpackObject(reader);
        }

        public Filter ToFilter()
        {
            return new Filter(new OperationChoice(this));
        }

        
        private static void PackObject(object value, PackageWriter writer)
        {
            bool notNull = (value != null);
            writer.PutBool(notNull);
            if (!notNull)
                return;

            if (value is IConvertible) {
                var cnv = value as IConvertible;
                var typeCode = cnv.GetTypeCode();
                writer.PutInt32((int)typeCode);

                switch (typeCode) {
                    case TypeCode.Char:
                        writer.PutInt32((int)value);
                        break;
                    case TypeCode.String:
                        writer.PutString((string)value);
                        break;
                    case TypeCode.DateTime:
                        writer.PutDateTime((DateTime)value);
                        break;
                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        break;
                    default:
                        double dv = Convert.ToDouble(value);
                        writer.PutDouble(dv);
                        break;
                }
            }
            else if (value is Guid) {
                writer.PutInt32(100);
                writer.PutString(((Guid)value).ToString("B"));
            }
            else if (value is AimObject) {
                writer.PutInt32(101);
                var aimObj = value as AimObject;
                int aimTypeIndex = AimMetadata.GetAimTypeIndex(aimObj);
                writer.PutInt32(aimTypeIndex);
                (aimObj as IPackable).Pack(writer);
            }
            else if (value is IEnumerable<Guid>) {
                writer.PutInt32(102);
                var list = value as IList;
                writer.PutInt32(list.Count);
                foreach (Guid item in list) {
                    writer.PutString(item.ToString());
                }
            }
            else {
                throw new Exception("Value not supported for pack");
            }
        }

        private static object UnpackObject(PackageReader reader)
        {
            bool notNull = reader.GetBool();
            if (!notNull)
                return null;

            int typeCodeIndex = reader.GetInt32();

            if (typeCodeIndex == 100) {
                string s = reader.GetString();
                return new Guid(s);
            }
            else if (typeCodeIndex == 101) {
                int aimTypeIndex = reader.GetInt32();
                AimObject aimObj = Aran.Aim.AimObjectFactory.Create(aimTypeIndex);
                (aimObj as IPackable).Unpack(reader);
                return aimObj;
            }
            else if (typeCodeIndex == 102) {
                var count = reader.GetInt32();
                var list = new List<Guid>();
                for (int i = 0; i < count; i++) {
                    var guid = new Guid(reader.GetString());
                    list.Add(guid);
                }
                return list;
            }
            else {
                TypeCode typeCode = (TypeCode)typeCodeIndex;
                switch (typeCode) {
                    case TypeCode.Char:
                        return (char)reader.GetInt32();
                    case TypeCode.String:
                        return reader.GetString();
                    case TypeCode.DateTime:
                        return reader.GetDateTime();
                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        return null;
                    default:
                        double dv = reader.GetDouble();
                        return Convert.ChangeType(dv, typeCode);
                }
            }
        }
    }

    public enum ComparisonOpType
    {
        EqualTo,
        NotEqualTo,
        LessThan,
        GreaterThan,
        LessThanOrEqualTo,
        GreaterThanOrEqualTo,
        Null,
        NotNull,
        Like,
        NotLike,
        Is,
        In
    }
}
