using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Geometries;

namespace Aran.Aim.AixmMessage
{
    public static class CommonXmlWriter
    {
        public static void WriteElement (TimePeriod value,
            XmlWriter writer, NamespaceInfo nsInfo, string localName)
        {
            writer.WriteStartElement (nsInfo.Prefix, localName, nsInfo.Namespace);
            writer.WriteStartElement (AimDbNamespaces.GML, "TimePeriod");
            writer.WriteAttributeString (AimDbNamespaces.GML, "id", "aran1");

            writer.WriteElementString (AimDbNamespaces.GML, "beginPosition", CommonXmlWriter.GetString (value.BeginPosition));

            writer.WriteElementString (AimDbNamespaces.GML, "endPosition",
                (value.EndPosition != null ? CommonXmlWriter.GetString (value.EndPosition.Value) : null));

            writer.WriteEndElement ();
            writer.WriteEndElement ();
        }

        public static void WriteElement (ValDistance value,
            XmlWriter writer, NamespaceInfo nsInfo, string localName)
        {
            writer.WriteStartElement (nsInfo, localName);
            writer.WriteAttributeString ("uom", value.Uom.ToString ());
            writer.WriteValue (value.Value);
            writer.WriteEndElement ();
        }

        public static void WriteElement(Aran.Geometries.Geometry value, XmlWriter writer, bool createElement, bool write3DIfExists = true, 
            //SrsNameType srsType = SrsNameType.CRS84
            SrsNameType srsType = SrsNameType.EPSG_4326
            )
        {
            switch (value.Type) {
                case GeometryType.Point: {
                        if (createElement) {
                            writer.WriteStartElement(AimDbNamespaces.GML, "Point");
                            writer.WriteAttributeString(AimDbNamespaces.GML, "id", "aran1");
                            GeomConvertToXml.FromPoint((Aran.Geometries.Point)value, writer, write3DIfExists, srsType);
                            writer.WriteEndElement();
                        }
                        else {
                            GeomConvertToXml.FromPoint((Aran.Geometries.Point)value, writer, write3DIfExists, srsType);
                        }
                    }
                    break;
                case GeometryType.MultiLineString: {
                        if (createElement) {
                            writer.WriteStartElement(AimDbNamespaces.GML, "Curve");
                            GeomConvertToXml.FromMultiLineString((MultiLineString)value, writer, write3DIfExists, srsType);
                            writer.WriteFullEndElement();
                        }
                        else {
                            GeomConvertToXml.FromMultiLineString((MultiLineString)value, writer, write3DIfExists, srsType);
                        }
                    }
                    break;
                case GeometryType.MultiPolygon: {
                        if (createElement) {
                            writer.WriteStartElement(AimDbNamespaces.GML, "Surface");
                            writer.WriteAttributeString(AimDbNamespaces.GML, "id", "aran1");
                            GeomConvertToXml.FromMultiPolygon((Aran.Geometries.MultiPolygon)value, writer, write3DIfExists, srsType);
                            writer.WriteEndElement();
                        }
                        else {
                            GeomConvertToXml.FromMultiPolygon((Aran.Geometries.MultiPolygon)value, writer, write3DIfExists, srsType);
                        }
                    }
                    break;
                case GeometryType.SpecialGeometry: {
                        SpecialGeometry spGeom = (SpecialGeometry)value;
                        if (spGeom.SpecGeomType == SpecialGeometryType.Box) {
                            if (createElement) {
                                writer.WriteStartElement(AimDbNamespaces.GML, "Envelope");
                                WriteElement((Aran.Geometries.Box)spGeom, writer);
                                writer.WriteEndElement();
                            }
                            else {
                                WriteElement((Aran.Geometries.Box)spGeom, writer);
                            }
                        }
                    }
                    break;
            }
        }

        private static void WriteElement (Aran.Geometries.Box value, XmlWriter writer)
        {
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:OGC:1.3:CRS84");
            writer.WriteElementString (AimDbNamespaces.GML, "lowerCorner", value [0].X + " " + value [0].Y);
            writer.WriteElementString (AimDbNamespaces.GML, "upperCorner", value [1].X + " " + value [1].Y);
        }

        public static string GetString(DateTime value, bool isDateTime = true)
        {
            if (isDateTime)
                return value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            else
                return value.ToString("yyyy-MM-dd");
        }

        public static string GetString (Guid value)
        {
            return value.ToString ();
        }

        public static string GetString(bool value)
        {
            return value.ToString().ToLower();
        }

        public static string GetEnumString (object enumValue)
        {
            string enumText = enumValue.ToString ();

            if (enumText.Length == 0)
                return string.Empty;

#if LGS
		//	if ( enumText == "PERMDELTA" )
		//		return "BASELINE";
#endif 
			if (enumText.StartsWith("OTHER_"))
                return "OTHER:" + enumText.Substring(6);
            else if (enumText[0] == '_')
                return enumText.Substring(1);
            return enumText;
        }

        public static string GenerateNewGmlId()
        {
            return AranAixmWriter.GenerateNewGmlId();
        }
    }

    static class GeomConvertToXml
    {
        public static void FromPoint(Point point, XmlWriter writer, bool write3DIfExists, SrsNameType srsType)
        {

            bool hasZ = write3DIfExists && !double.IsNaN(point.Z);
            if (hasZ)
                writer.WriteAttributeString("srsDimension", "3");

            bool isXFirst;
            writer.WriteAttributeString("srsName", GetSrsName(srsType, out isXFirst));

            if(point.IsEmpty)
                return;
            string pos;

            if (isXFirst)
                pos = point.X + " " + point.Y;
            else
                pos = point.Y + " " + point.X;

            if (hasZ)
                pos += " " + point.Z;

            writer.WriteElementString(AimDbNamespaces.GML, "pos", pos);
        }

        public static void FromMultiLineString(MultiLineString mltLineString, XmlWriter writer, bool write3DIfExists, SrsNameType srsType)
        {
            bool isXFirst;
            writer.WriteAttributeString ("srsName", GetSrsName(srsType, out isXFirst));
            
            string posList = "";
            
            writer.WriteStartElement (AimDbNamespaces.GML, "segments");

            foreach (LineString lnString in mltLineString)
            {
                //writer.WriteStartElement (AimDbNamespaces.GML, "LineStringSegment");
                writer.WriteStartElement (AimDbNamespaces.GML, "GeodesicString");
                writer.WriteStartElement (AimDbNamespaces.GML, "posList");

                posList = "";
                
                if (write3DIfExists && (lnString.Count > 0 && !double.IsNaN(lnString[0].Z) && HasAnyNonZeroZ(lnString))) {
                    writer.WriteAttributeString("srsDimension", "3");
                    if (isXFirst) {
                        foreach (Point point in lnString)
                            posList += point.X + " " + point.Y + " " + point.Z + " ";
                    }
                    else {
                        foreach (Point point in lnString)
                            posList += point.Y + " " + point.X + " " + point.Z + " ";
                    }
                }
                else {
                    //writer.WriteAttributeString ("srsDimension", "2");
                    if (isXFirst) {
                        foreach (Point point in lnString)
                            posList += point.X + " " + point.Y + " ";
                    }
                    else {
                        foreach (Point point in lnString)
                            posList += point.Y + " " + point.X + " ";
                    }
                }

                writer.WriteString (posList);
                writer.WriteFullEndElement ();
                writer.WriteFullEndElement ();
            }

            writer.WriteFullEndElement ();
        }
        
        private static void FromMultiPolygon_201201013 (MultiPolygon mltPolygon, XmlWriter writer)
        {
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:EPSG:4326");

            writer.WriteStartElement (AimDbNamespaces.GML, "patches");
            writer.WriteStartElement (AimDbNamespaces.GML, "PolygonPatch");
            writer.WriteStartElement (AimDbNamespaces.GML, "exterior");
            writer.WriteStartElement (AimDbNamespaces.GML, "Ring");
            {
                foreach (Polygon plygon in mltPolygon)
                {
                    writer.WriteStartElement (AimDbNamespaces.GML, "curveMember");
                    {
                        writer.WriteStartElement (AimDbNamespaces.GML, "Curve");
                        {
                            writer.WriteAttributeString (AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ());

                            writer.WriteStartElement (AimDbNamespaces.GML, "segments");
                            {
                                writer.WriteStartElement (AimDbNamespaces.GML, "GeodesicString");
                                {
                                    writer.WriteStartElement (AimDbNamespaces.GML, "posList");
                                    {
                                        string posList = "";
                                        if (!double.IsNaN (plygon.ExteriorRing [0].Z) && HasAnyNonZeroZ (plygon.ExteriorRing))
                                        {
                                            writer.WriteAttributeString ("srsDimension", "3");
                                            foreach (Point pnt in plygon.ExteriorRing)
                                                posList += pnt.Y + " " + pnt.X + " " + pnt.Z + " ";
                                        }
                                        else
                                        {
                                            foreach (Point pnt in plygon.ExteriorRing)
                                                posList += pnt.Y + " " + pnt.X + " ";
                                        }

                                        writer.WriteString (posList);
                                    }
                                    writer.WriteFullEndElement ();  //--- posList
                                }
                                writer.WriteFullEndElement ();  //--- GeodesicString
                            }
                            writer.WriteFullEndElement ();  //--- segments
                        }
                        writer.WriteFullEndElement ();  //--- Curve
                    }
                    writer.WriteFullEndElement ();  //--- curveMember

#region interior (hove to anylize. no any simple in "Use of GML for aviation data" doucment)

                    foreach (Ring rng in plygon.InteriorRingList)
                    {
                        writer.WriteStartElement (AimDbNamespaces.GML, "interior");
						writer.WriteStartElement ( AimDbNamespaces.GML, "Ring" );
						writer.WriteStartElement ( AimDbNamespaces.GML, "curveMember" );
						writer.WriteStartElement ( AimDbNamespaces.GML, "Curve" );
						writer.WriteAttributeString ( AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ( ) );
						writer.WriteStartElement ( AimDbNamespaces.GML, "segments" );
						writer.WriteStartElement ( AimDbNamespaces.GML, "LineStringSegment" );
                        writer.WriteStartElement (AimDbNamespaces.GML, "posList");

                        if (!double.IsNaN (rng [0].Z) && HasAnyNonZeroZ (rng))
                            writer.WriteAttributeString ("srsDimension", "3");

                        string posList = "";

                        foreach (Point pnt in rng)
                        {
                            posList += pnt.Y + " " + pnt.X + " ";
                        }

                        writer.WriteString (posList);

						writer.WriteFullEndElement ( );  //--- posList
						writer.WriteFullEndElement ( );  //--- LineStringSegment
						writer.WriteFullEndElement ( );  //--- segments
						writer.WriteFullEndElement ( );  //--- Curve
						writer.WriteFullEndElement ( );  //--- curveMember
						writer.WriteFullEndElement ( );  //--- Ring
						writer.WriteFullEndElement ( );  //--- interior
                    }

#endregion interior
                }
            }
            writer.WriteFullEndElement ();  //--- Ring
            writer.WriteFullEndElement ();  //--- exterior
            writer.WriteFullEndElement ();  //--- PolygonPatch
            writer.WriteFullEndElement ();  //--- patches
        }

        public static void FromMultiPolygon(MultiPolygon mltPolygon, XmlWriter writer, bool write3DIfExists, SrsNameType srsType)
        {
#if EUROCONTROL_NAMESPACE
            FromMultiPolygon_201201013 (mltPolygon, writer);
            return;
#endif
            bool isXFirst;
            writer.WriteAttributeString ("srsName", GetSrsName(srsType, out isXFirst));
            string posList = "";

            writer.WriteStartElement (AimDbNamespaces.GML, "patches");

            foreach (Polygon plygon in mltPolygon)
            {
                writer.WriteStartElement (AimDbNamespaces.GML, "PolygonPatch");
                writer.WriteStartElement (AimDbNamespaces.GML, "exterior");
                writer.WriteStartElement (AimDbNamespaces.GML, "Ring");
				writer.WriteStartElement ( AimDbNamespaces.GML, "curveMember" );
				writer.WriteStartElement ( AimDbNamespaces.GML, "Curve" );
				writer.WriteAttributeString ( AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ( ) );
				writer.WriteStartElement ( AimDbNamespaces.GML, "segments" );
				//writer.WriteStartElement ( AimDbNamespaces.GML, "LineStringSegment" );
				writer.WriteStartElement ( AimDbNamespaces.GML, "GeodesicString");
				writer.WriteStartElement (AimDbNamespaces.GML, "posList");

                posList = "";

                //if (!double.IsNaN (plygon.ExteriorRing [0].Z) && HasAnyNonZeroZ (plygon.ExteriorRing))
                //{
                //    writer.WriteAttributeString ("srsDimension", "3");
                //    foreach (Point pnt in plygon.ExteriorRing)
                //        posList += pnt.X + " " + pnt.Y + " " + pnt.Z + " ";
                //}
                //else
                //{
                //    //writer.WriteAttributeString ("srsDimension", "2");
                //    foreach (Point pnt in plygon.ExteriorRing)
                //        posList += pnt.X + " " + pnt.Y + " ";
                //}

                if (isXFirst) {
                    foreach (Point pnt in plygon.ExteriorRing)
                        posList += pnt.X + " " + pnt.Y + " ";
                }
                else {
                    foreach (Point pnt in plygon.ExteriorRing)
                        posList += pnt.Y + " " + pnt.X + " ";
                }

                writer.WriteString (posList);

                writer.WriteFullEndElement ();  //--- posList
				writer.WriteFullEndElement ( );  //--- LineStringSegment
				writer.WriteFullEndElement ( );  //--- segments
				writer.WriteFullEndElement ( );  //--- Curve
				writer.WriteFullEndElement ( );  //--- curveMember
				writer.WriteFullEndElement ();  //--- Ring
                writer.WriteFullEndElement ();  //--- exterior

                foreach (Ring rng in plygon.InteriorRingList)
                {
                    writer.WriteStartElement (AimDbNamespaces.GML, "interior");
					writer.WriteStartElement ( AimDbNamespaces.GML, "Ring" );
					writer.WriteStartElement ( AimDbNamespaces.GML, "curveMember" );
					writer.WriteStartElement ( AimDbNamespaces.GML, "Curve" );
					writer.WriteAttributeString ( AimDbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ( ) );
					writer.WriteStartElement ( AimDbNamespaces.GML, "segments" );
					//writer.WriteStartElement ( AimDbNamespaces.GML, "LineStringSegment" );
					writer.WriteStartElement ( AimDbNamespaces.GML, "GeodesicString" );
					writer.WriteStartElement ( AimDbNamespaces.GML, "posList" );

                    //if (write3DIfExists && (!double.IsNaN (rng [0].Z) && HasAnyNonZeroZ (rng)))
                    //    writer.WriteAttributeString ("srsDimension", "3");
                    
                    posList = "";

                    if (isXFirst) {
                        foreach (Point pnt in rng) 
                            posList += pnt.X + " " + pnt.Y + " ";
                    }
                    else {
                        foreach (Point pnt in rng)
                            posList += pnt.Y + " " + pnt.X + " ";
                    }
                    
                    writer.WriteString (posList);

					writer.WriteFullEndElement ( );  //--- posList
					writer.WriteFullEndElement ( );  //--- LineStringSegment
					writer.WriteFullEndElement ( );  //--- segments
					writer.WriteFullEndElement ( );  //--- Curve
					writer.WriteFullEndElement ( );  //--- curveMember
					writer.WriteFullEndElement ( );  //--- Ring
					writer.WriteFullEndElement ();  //--- interior
                }

                writer.WriteFullEndElement ();  //--- PolygonPatch
            }
            writer.WriteFullEndElement ();  //--- patches
        }

        private static bool HasAnyNonZeroZ (MultiPoint multiPoint)
        {
            for (int i = 0; i < multiPoint.Count; i++)
            {
                if (double.IsNaN (multiPoint [i].Z))
                    return false;
            }

            for (int i = 0; i < multiPoint.Count; i++)
            {
                if (multiPoint [i].Z != 0)
                    return true;
            }
            return false;
        }

        private static string GetSrsName(SrsNameType srsType, out bool isXFirst)
        {
            switch (srsType) {
                case SrsNameType.CRS84: {
                        isXFirst = true;
                        return "urn:ogc:def:crs:OGC:1.3:CRS84";
                    }
                case SrsNameType.EPSG_4326: {
                        isXFirst = false;
                        return "urn:ogc:def:crs:EPSG:4326";
                    }
            }
            throw new Exception("SrsName is invalid.");
        }
    }
}
