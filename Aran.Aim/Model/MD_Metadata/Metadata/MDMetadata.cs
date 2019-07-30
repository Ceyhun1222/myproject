using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using System.Xml;
using System.Text;

namespace Aran.Aim.Metadata
{
	public class MDMetadata : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MDMetadata; }
		}
		
		public DateTime? DateStamp
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyMDMetadata.DateStamp); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyMDMetadata.DateStamp, value); }
		}

		public string XmlText { get; set; }

		public void ReadXml (XmlReader reader)
		{
			XmlText = reader.ReadInnerXml ();
			XmlText = XmlText.Trim ();
		}

		public void WriterXml (XmlWriter writer)
		{
			if (XmlText == null)
				return;

			System.IO.MemoryStream ms = new System.IO.MemoryStream ();
			byte [] ba = Encoding.UTF8.GetBytes (XmlText);
			ms.Write (ba, 0, ba.Length);
			ms.Seek (0, System.IO.SeekOrigin.Begin);

			XmlReader reader = XmlReader.Create (ms);

			writer.WriteNode (reader, true);

			reader.Close ();
			ms.Close ();
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMDMetadata
	{
		DateStamp = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMDMetadata
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMDMetadata ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMDMetadata.DateStamp, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
		}
	}
}
