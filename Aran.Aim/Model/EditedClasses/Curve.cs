using Aran.Aim.PropertyEnum;
using Aran.Aixm;
using System.Xml;
using System.IO;
using Aran.Aim.Converter;
using Aran.Geometries;

namespace Aran.Aim.Features
{
	//Editing
	partial class Curve
	{
		public Curve()
		{
			SetFieldValue<Aran.Geometries.MultiLineString> (
				(int) PropertyCurve.Geo, 
				new Geometries.MultiLineString ());
		}

		public Aran.Geometries.MultiLineString Geo
		{
			get { return GetFieldValue<Aran.Geometries.MultiLineString> ((int) PropertyCurve.Geo); }    
		}

        protected override bool AixmDeserialize(XmlContext context)
        {
            using (TextReader textReader = new StringReader(context.Element.OuterXml))
            {
                using (XmlReader reader = XmlReader.Create(textReader))
                {
                    reader.Read();
                    var mls = GeomConverterFromXml.ToMultiLineString(reader);

                    if (mls.Count == 0 || mls[0].Count == 0)
                        throw new System.Exception("Curve geometry is empty.");

                    Geo.Assign(mls);
                }
            }

            context.ElementIndex.Start++;

            base.AixmDeserialize(context);
            return true;
        }
	}
}

namespace Aran.Aim.PropertyEnum
{
	//Editing
	public enum PropertyCurve
	{
		Geo = PropertyAObject.NEXT_CLASS,
		HorizontalAccuracy,
		//Extension,
		NEXT_CLASS
	}

	//Editing
	public static class MetadataCurve
	{
		public static AimPropInfoList PropInfoList;

		static MetadataCurve ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();

			PropInfoList.Add (PropertyCurve.Geo, (int) AimFieldType.GeoPolyline, "");
			PropInfoList.Add (PropertyCurve.HorizontalAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			//PropInfoList.Add(PropertyCurve.Extension, (int)ObjectType.CurveExtension, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
