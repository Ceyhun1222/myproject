using Aran.Aim.Enums;
using Aran.Aixm;
using System.Xml;
using System;

namespace Aran.Aim.DataTypes
{
	public class ValDistanceVertical : ValClass <UomDistanceVertical, double>
	{
        public ValDistanceVertical ()
        {
        }

        public ValDistanceVertical (double value, UomDistanceVertical uom)
            : base (value, uom)
        {
        }

		public override DataType DataType
		{
			get { return DataType.ValDistanceVertical; }
		}

        public CodeValDistanceOtherValue? OtherValue { get; set; }

        protected override bool AixmDeserialize (XmlContext context)
        {
            XmlElement xmlElem = context.Element;
            XmlAttribute attr = xmlElem.Attributes ["uom"];
            if (attr != null && attr.Value == "OTHER")
            {
                CodeValDistanceOtherValue val;
                if (Enum.TryParse<CodeValDistanceOtherValue> (xmlElem.InnerText, out val))
                {
                    OtherValue = val;
                    return true;
                }
                return false;
            }

            if (xmlElem.InnerText == "UNL")
                xmlElem.InnerText = "999";
            else if (xmlElem.InnerText == "GND")
                xmlElem.InnerText = "0";
            
            return base.AixmDeserialize (context);
        }
	}
}

namespace Aran.Aim.PropertyEnum
{
    public static class MetadataValDistanceVertical
    {
        public static AimPropInfoList PropInfoList;

        static MetadataValDistanceVertical ()
        {
            PropInfoList = MetadataValClassBase.PropInfoList.Clone ();

            PropInfoList.Add (PropertyValClassBase.Uom, (int) EnumType.UomDistanceVertical);
        }
    }
}