using System;
using System.Xml;
using Aran.Aim.Enums;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;

namespace Aran.Aim.DataTypes
{
	public class TextNote : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.TextNote; }
		}
		
		public string Value
		{
			get { return GetFieldValue <string> ((int) PropertyTextNote.Value); }
			set { SetFieldValue <string> ((int) PropertyTextNote.Value, value); }
		}
		
		public language Lang
		{
			get { return GetFieldValue <language> ((int) PropertyTextNote.Lang); }
			set { SetFieldValue <language> ((int) PropertyTextNote.Lang, value); }
		}

        protected override bool AixmDeserialize (XmlContext context)
        {
            XmlElement element = context.Element;

            string attrValue = CommonXmlFunctions.GetElementAttribute (context.Element, "lang");
            if (attrValue != null)
                Lang = CommonFunctions.ParseEnum<language> (attrValue);

            Value = element.InnerText;            
            return true;
        }
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTextNote
	{
		Value,
		Lang
	}

    public static class MetadataTextNote
    {
        public static AimPropInfoList PropInfoList;

        static MetadataTextNote ()
        {
            PropInfoList = MetadataADataType.PropInfoList.Clone ();

            PropInfoList.Add (PropertyTextNote.Value, (int) AimFieldType.SysString);
            PropInfoList.Add (PropertyTextNote.Lang, (int) EnumType.language);
        }
    }
}
