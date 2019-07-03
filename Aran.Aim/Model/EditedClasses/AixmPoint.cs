using System.Xml;
using Aran.Aixm;
using Aran.Aim.PropertyEnum;
using System.IO;
using Aran.Aim.Converter;
using System;

namespace Aran.Aim.Features
{
	//Editing
	partial class AixmPoint
	{
		public AixmPoint()
		{
			SetFieldValue<Aran.Geometries.Point>((int)PropertyAixmPoint.Geo, 
				new Aran.Geometries.Point ());
			SetValue ( ( int ) PropertyAixmPoint.Extension, new PointExtension ( ) );
		}

		public Aran.Geometries.Point Geo
		{
			get { return GetFieldValue<Aran.Geometries.Point> ((int) PropertyAixmPoint.Geo); }
		}

		public PointExtension Extension
		{
			get
			{
				return GetObject<PointExtension> ( ( int ) PropertyAixmPoint.Extension );
			}
			set { SetValue((int)PropertyAixmPoint.Extension, value); }
		}

		protected override bool AixmDeserialize (XmlContext context)
		{
			using (TextReader textReader = new StringReader (context.Element.OuterXml))
			{
				using (XmlReader reader = XmlReader.Create (textReader))
				{
					reader.Read ();
					Geometries.Point point = GeomConverterFromXml.ToPoint (reader);
					Geo.Assign (point);
				}
			}

			context.ElementIndex.Start++;

			base.AixmDeserialize (context);
			return true;
		}
	}
}


namespace Aran.Aim.PropertyEnum
{
	//Editing
	public enum PropertyAixmPoint
	{
		Geo = PropertyAObject.NEXT_CLASS,
		HorizontalAccuracy,
		Extension,
		NEXT_CLASS
	}

	//Editing
	public static class MetadataAixmPoint
	{
		public static AimPropInfoList PropInfoList;

		static MetadataAixmPoint ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();

			PropInfoList.Add (PropertyAixmPoint.Geo, (int) AimFieldType.GeoPoint, "");
			PropInfoList.Add (PropertyAixmPoint.HorizontalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add(PropertyAixmPoint.Extension, (int)ObjectType.PointExtension, PropertyTypeCharacter.Nullable,"CRC");
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
