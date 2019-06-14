using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using System.IO;
using System.Xml;
using Aran.Aim.Converter;
using Aran.Geometries;

namespace Aran.Aim.Features
{
	//Editing
	partial class Surface
	{
		public Surface()
		{
			SetFieldValue<Aran.Geometries.MultiPolygon>((int)PropertySurface.Geo, 
				new Aran.Geometries.MultiPolygon ());
		}

		public Aran.Geometries.MultiPolygon Geo
		{
			get { return GetFieldValue<Aran.Geometries.MultiPolygon> ((int) PropertySurface.Geo); }
		}

		protected override bool AixmDeserialize (XmlContext context)
		{
			try
			{
				using (TextReader textReader = new StringReader (context.Element.OuterXml))
				{
					using (XmlReader reader = XmlReader.Create (textReader))
					{
						reader.Read ();

						var isElevated = (ObjectType == Aim.ObjectType.ElevatedSurface);
						var geo = GeomConverterFromXml.ToMultiPolygon (reader, isElevated);
						Geo.Assign (geo);
					}
				}

				context.ElementIndex.Start++;

				base.AixmDeserialize (context);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	//Editing
	public enum PropertySurface
	{
		Geo = PropertyAObject.NEXT_CLASS,
		HorizontalAccuracy,
		//Extension,
		NEXT_CLASS
	}

	//Editing
	public static class MetadataSurface
	{
		public static AimPropInfoList PropInfoList;

		static MetadataSurface ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();

			PropInfoList.Add (PropertySurface.Geo, (int) AimFieldType.GeoPolygon, "");
			PropInfoList.Add (PropertySurface.HorizontalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
		    //PropInfoList.Add(PropertySurface.Extension, (int)ObjectType.SurfaceExtension, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
