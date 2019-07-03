using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;
using Aran.Geometries;
using System.Xml;
using Aran.Aixm;
using System.Reflection;

namespace Aran.Aim
{
    public interface IEditAimField
    {
        object FieldValue { get; set; }
    }

    public abstract class AimField :
        AimObject,
        IEditAimField,
        IAimProperty
    {
        public abstract AimFieldType FieldType { get; }

        object IEditAimField.FieldValue
        {
            get
            {
                return _value;
            }
            set
            {
                SetValue (value);
            }
        }

        #region IAranProperty Members

        AimPropertyType IAimProperty.PropertyType
        {
            get { return AimPropertyType.AranField; }
        }

        IAixmSerializable IAimProperty.GetAixmSerializable ()
        {
            return this;
        }

        IPackable IAimProperty.GetPackable ()
        {
            return this;
        }

        #endregion

        protected virtual void SetValue (object value)
        {
            _value = value;
        }

        protected object _value;
    }

    //*** Value property always is NonNull value.
    public class AimField<T> : AimField 
    {
        public AimField ()
            : this (default (T))
        {
        }

        public AimField (T value)
        {
            Value = value;
            _fieldType = GetValueFieldType (value);
        }

        public AimField (AimFieldType aranFieldType)
            : this (default (T), aranFieldType)
        {
        }

        public AimField (T value, AimFieldType aranFieldType)
        {
            Value = value;
            _fieldType = aranFieldType;
        }

        protected override AimObjectType AimObjectType
        {
            get { return AimObjectType.Field; }
        }

        public override AimFieldType FieldType
        {
            get { return _fieldType; }
        }

        public T Value
        {
            get { return (T) _value; }
            set
            {
                if (value == null)
                    throw new Exception ("Field Value is null");

                _value = value;
            }
        }

        protected override void SetValue (object value)
        {
            Value = (T) value;
        }

        protected override void Pack (PackageWriter writer)
        {
            switch (_fieldType)
            {
                case AimFieldType.SysBool:
                    writer.PutBool ((bool) _value);
                    break;
                case AimFieldType.SysDateTime:
                    writer.PutDateTime ((DateTime) _value);
                    break;
                case AimFieldType.SysDouble:
                    writer.PutDouble ((double) _value);
                    break;
                case AimFieldType.SysGuid:
                    writer.PutString (_value.ToString ());
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    writer.PutInt32 ((Int32) _value);
                    break;
                case AimFieldType.SysInt64:
                    writer.PutInt64 ((Int64) _value);
                    break;
                case AimFieldType.SysString:
                    writer.PutString (_value as string);
                    break;
                case AimFieldType.SysUInt32:
                    writer.PutUInt32 ((UInt32) _value);
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    ((Geometry) _value).Pack (writer);
                    break;
            }
        }

        protected override void Unpack (PackageReader reader)
        {
            switch (_fieldType)
            {
                case AimFieldType.SysBool:
                    _value = reader.GetBool ();
                    break;
                case AimFieldType.SysDateTime:
                    _value = reader.GetDateTime ();
                    break;
                case AimFieldType.SysDouble:
                    _value = reader.GetDouble ();
                    break;
                case AimFieldType.SysGuid:
                    _value = CommonXmlFunctions.ParseAixmGuid (reader.GetString ());
                    break;
                case AimFieldType.SysEnum:
                    _value = (T) (object)  reader.GetInt32 ();
                    break;
                case AimFieldType.SysInt32:
                    _value = reader.GetInt32 ();
                    break;
                case AimFieldType.SysInt64:
                    _value = reader.GetInt64 ();
                    break;
                case AimFieldType.SysString:
                    _value = reader.GetString ();
                    break;
                case AimFieldType.SysUInt32:
                    _value = reader.GetUInt32 ();
                    break;
                case AimFieldType.GeoPoint:
                    Aran.Geometries.Point point = new Aran.Geometries.Point ();
                    point.Unpack (reader);
                    _value = point;
                    break;
                case AimFieldType.GeoPolyline:
                    MultiLineString polyline = new MultiLineString ();
                    polyline.Unpack (reader);
                    _value = polyline;
                    break;
                case AimFieldType.GeoPolygon:
                    MultiPolygon polygon = new MultiPolygon ();
                    polygon.Unpack (reader);
                    _value = polygon;
                    break;
            }
        }

        protected override bool AixmDeserialize (XmlContext xmlContext)
        {
            try
            {
                XmlElement xmlElement = xmlContext.Element;

                if (string.IsNullOrEmpty (xmlElement.InnerText))
                    return false;
                
                string valueText = xmlElement.InnerText;

				switch (_fieldType)
				{
					case AimFieldType.SysBool:
						{
							bool isOk;
							_value = CommonXmlFunctions.ParseAixmBoolean (valueText, out isOk);
							if (!isOk)
								return false;
							break;
						}
					case AimFieldType.SysDateTime:
						_value = DateTime.Parse (valueText);
						break;
					case AimFieldType.SysDouble:
						_value = double.Parse (valueText);
						break;
					case AimFieldType.SysGuid:
						_value = CommonXmlFunctions.ParseAixmGuid (valueText);
						break;
					case AimFieldType.SysInt32:
						_value = Int32.Parse (valueText);
						break;
					case AimFieldType.SysInt64:
						_value = Int64.Parse (valueText);
						break;
					case AimFieldType.SysString:
						_value = valueText;
						break;
					case AimFieldType.SysUInt32:
						_value = UInt32.Parse (valueText);
						break;
					case AimFieldType.SysEnum:
						{
							if (valueText == "OTHER")
								return false;

                            object retValue;
							if (valueText.ToLower().StartsWith("other:"))
								valueText = valueText.Replace(':', '_');
                            var isOk = Enum_TryParse(valueText, out retValue);
                            
                            if (!isOk) {
                                if (!valueText.StartsWith("OTHER:"))
                                    DeserializeLastException.Exception = new Exception(string.Format("Enum value was not in a correct format: {0}, type: {1}", valueText, _value.GetType().Name));
                                return false;
                            }

                            _value = retValue;
                            return isOk;
						}
					default:
						return false;
				}
            }
            catch (Exception ex)
            {
                DeserializeLastException.Exception = ex;
                return false;
            }

            return true;
        }

        public override void Assign (AranObject source)
        {
            AimField<T> aranField = (AimField<T>) source;

            switch (_fieldType)
            {
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    Geometry sourceGeom = (Geometry) (object) aranField.Value;
                    Value = (T) (object) sourceGeom.Clone ();
                    break;
                default:
                    Value = aranField.Value;
                    break;
            }
        }

        public override AranObject Clone ()
        {
			AimField<T> val = new AimField<T> ( Value );
			val.Assign ( this );
			return val;
        }

        public override bool Equals (object obj)
        {
            if (obj is AimField<T>)
            {
                AimField<T> other = (AimField<T>) obj;

                if (_fieldType != other.FieldType)
                    return false;

                switch (_fieldType)
                {
                    case AimFieldType.GeoPoint:
                    case AimFieldType.GeoPolygon:
                    case AimFieldType.GeoPolyline:
                        return CommonFunctions.GeometryEquals (_value as Geometry, other.Value as Geometry);
                    case AimFieldType.SysDateTime:
                        var date = (DateTime)_value;
                        var otherDate = (DateTime)other._value;
                        return date.Ticks / 10000 == otherDate.Ticks / 10000;
                    default:
                        if ( _fieldType == AimFieldType.SysEnum )
                        {
                            int sourcVal = ( int ) _value;
                            int destVal = ( int ) other._value;
                            return sourcVal.Equals ( destVal );
                        }
                        return _value.Equals ( other._value );
                }
            }
            return base.Equals (obj);
        }

        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        private static bool Enum_TryParse(string text, out object retValue)
        {
            if (text.Length > 0 && char.IsDigit(text[0]))
                text = "_" + text;
                

            retValue = null;
            var enumType = typeof(T);

            #region Get MethodInfo -> [Could not be able to get from Type.GetMothodInfo]

            if (_enumTryParseMethodInfo == null)
            {
                var methods = typeof(Enum).GetMethods(BindingFlags.Static | BindingFlags.Public);

                foreach (var mi in methods)
                {
                    if (mi.Name == "TryParse" && mi.GetParameters().Length == 3)
                    {
                        _enumTryParseMethodInfo = mi.MakeGenericMethod(enumType);
                        break;
                    }
                }
            }

            #endregion

            var parameters = new object[] { text, true, null };
            var isOK = (bool)_enumTryParseMethodInfo.Invoke(null, parameters);

            if (isOK)
                retValue = parameters[2];

            return isOK;
        }

        private AimFieldType GetValueFieldType (T val)
        {
            if (val is IConvertible)
            {
                TypeCode typeCode = (val as IConvertible).GetTypeCode ();

                switch (typeCode)
                {
                    case TypeCode.Double:
                        return AimFieldType.SysDouble;
                    case TypeCode.String:
                        return AimFieldType.SysString;
                    case TypeCode.UInt32:
                        return AimFieldType.SysUInt32;
                    case TypeCode.Int32:
                        {
                            if (typeof (T).IsEnum)
                                return AimFieldType.SysEnum;
                            return AimFieldType.SysInt32;
                        }
                    case TypeCode.Int64:
                        return AimFieldType.SysInt64;
                    case TypeCode.Boolean:
                        return AimFieldType.SysBool;
                    case TypeCode.DateTime:
                        return AimFieldType.SysDateTime;
                    default:
                        throw new Exception ("System Type not supported for the type: " + typeCode);
                }
            }
            else if (val is Guid)
            {
                return AimFieldType.SysGuid;
            }
            else if (val is Geometry)
            {
                Geometry geom = (Geometry) (object) val;

                switch (geom.Type)
                {
                    case GeometryType.Point:
                        return AimFieldType.GeoPoint;
                    case GeometryType.MultiLineString:
                        return AimFieldType.GeoPolyline;
                    case GeometryType.MultiPolygon:
                        return AimFieldType.GeoPolygon;
                }

                throw new Exception ("Geometry Type not supported for the type: " + geom.Type);
            }
            else
            {
                throw new Exception ("Field Type not supported for the type: " + val.GetType ().Name);
            }
        }        

        private AimFieldType _fieldType;
        private static MethodInfo _enumTryParseMethodInfo;
    }
}
