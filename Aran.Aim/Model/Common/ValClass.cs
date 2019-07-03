using System;
using System.Collections.Generic;
using System.Xml;
using Aran.Aixm;
using Aran.Aim.PropertyEnum;
using Aran.Package;

namespace Aran.Aim.DataTypes
{
    public interface IEditValClass
    {
        double Value { get; set; }
        int Uom { get; set; }
    }

    public abstract class ValClassBase : ADataType, IEditValClass
    {
        protected ValClassBase ()
        {
        }

        protected ValClassBase (double value, int uom)
        {
            ValueBase = value;
            UomValue = uom;
        }

        protected double ValueBase
        {
            get { return GetFieldValue<double> ((int) PropertyValClassBase.Value); }
            set { SetFieldValue<double> ((int) PropertyValClassBase.Value, value); }
        }

        protected int UomValue
        {
            get { return GetFieldValue<int> ((int) PropertyValClassBase.Uom); }
            set { SetFieldValue<int> ((int) PropertyValClassBase.Uom, value); }
        }

        #region IEditValClass Members

        double IEditValClass.Value
        {
            get { return ValueBase; }
            set { ValueBase = value; }
        }

        int IEditValClass.Uom
        {
            get { return UomValue; }
            set { UomValue = value; }
        }

        #endregion

        protected override void Pack(PackageWriter writer)
        {
            writer.PutDouble(ValueBase);
            writer.PutInt32(UomValue);
        }

        protected override void Unpack(PackageReader reader)
        {
            ValueBase = reader.GetDouble();
            UomValue = reader.GetInt32();
        }
    }

    public abstract class ValClass<TUom, TValue> : ValClassBase
            where TValue : struct
            where TUom : struct
    {
        protected ValClass ()
        {
        }

        protected ValClass (TValue value, TUom uom)
            : base(Convert.ToDouble(value), (int)(object)uom)
        {
        }

        public TValue Value
        {
			get
			{
                return (TValue)Convert.ChangeType(ValueBase, typeof(TValue));
			}
            set
            {
                ValueBase = Convert.ToDouble (value);
            }
        }

        public TUom Uom
        {
            get { return (TUom) (object) UomValue; }
            set { UomValue = (int) (object) value; }
        }

        protected override bool AixmDeserialize(XmlContext context)
        {
            XmlElement xmlElement = context.Element;

            if (string.IsNullOrEmpty(xmlElement.InnerText))
                return false;

            var uomAttr = xmlElement.Attributes["uom"];
            if (uomAttr == null)
                throw new Exception("UOM not defined!");

            Uom = CommonFunctions.ParseEnum<TUom>(uomAttr.Value);
            Value = (TValue)Convert.ChangeType(xmlElement.InnerText, typeof(TValue));

            return true;
        }

        public override string ToString()
        {
            double d = Convert.ToDouble(Value);
            return string.Format("{0} {1}", d, Uom);
        }

        public string StringValue
        {
            get
            {
                return ToString();
            }
        }

        protected bool Equals(ValClass<TUom, TValue> other)
        {
            if (other == null)
            {
                return false;
            }

            return EqualityComparer<ValClass<TUom, TValue>>.Default.Equals(this, other);
        }

    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyValClassBase
    {
        Value,
        Uom
    }

    public static class MetadataValClassBase
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValClassBase ()
        {
            PropInfoList = MetadataAimObject.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Value, (int) AimFieldType.SysDouble, "");
        }
    }
}
