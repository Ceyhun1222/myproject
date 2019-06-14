using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Aim.DataTypes;
using Aran.Geometries;

namespace Aran.Aim.CAWProvider
{
    public static class CommonXmlWriter
    {
        public static void WriteElement (TimePeriod value,
            XmlWriter writer, NamespaceInfo nsInfo, string localName)
        {
            writer.WriteStartElement (nsInfo.Prefix, localName, nsInfo.Namespace);
            writer.WriteStartElement (AimdbNamespaces.GML, "TimePeriod");
            writer.WriteAttributeString (AimdbNamespaces.GML, "id", "aran1");

            writer.WriteElementString (AimdbNamespaces.GML, "beginPosition", CommonXmlWriter.GetString (value.BeginPosition));

            writer.WriteElementString (AimdbNamespaces.GML, "endPosition",
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

        public static void WriteElement (Aran.Geometries.Geometry value, XmlWriter writer, bool createElement)
        {
            switch (value.Type)
            {
                case GeometryType.Point:
                    {
                        if (createElement)
                        {
                            writer.WriteStartElement (AimdbNamespaces.GML, "Point");
                            writer.WriteAttributeString (AimdbNamespaces.GML, "id", "aran1");
                            GeomConvertToXml.FromPoint ((Aran.Geometries.Point) value, writer);
                            writer.WriteEndElement ();
                        }
                        else
                        {
                            GeomConvertToXml.FromPoint ((Aran.Geometries.Point) value, writer);
                        }
                    }
                    break;
                case GeometryType.MultiLineString:
                    {
                        if (createElement)
                        {
                            writer.WriteStartElement (AimdbNamespaces.GML, "Curve");
                            GeomConvertToXml.FromMultiLineString ((MultiLineString) value, writer);
                            writer.WriteFullEndElement ();
                        }
                        else
                        {
                            GeomConvertToXml.FromMultiLineString ((MultiLineString) value, writer);
                        }
                    }
                    break;
                case GeometryType.MultiPolygon:
                    {
                        if (createElement)
                        {
                            writer.WriteStartElement (AimdbNamespaces.GML, "Surface");
                            writer.WriteAttributeString (AimdbNamespaces.GML, "id", "aran1");
                            GeomConvertToXml.FromMultiPolygon ((Aran.Geometries.MultiPolygon) value, writer);
                            writer.WriteEndElement ();
                        }
                        else
                        {
                            GeomConvertToXml.FromMultiPolygon ((Aran.Geometries.MultiPolygon) value, writer);
                        }
                    }
                    break;
                case GeometryType.SpecialGeometry:
                    {
                        SpecialGeometry spGeom = (SpecialGeometry) value;
                        if (spGeom.SpecGeomType == SpecialGeometryType.Box)
                        {
                            if (createElement)
                            {
                                writer.WriteStartElement (AimdbNamespaces.GML, "Envelope");
                                WriteElement ((Aran.Geometries.Box) spGeom, writer);
                                writer.WriteEndElement ();
                            }
                            else
                            {
                                WriteElement ((Aran.Geometries.Box) spGeom, writer);
                            }
                        }
                    }
                    break;
            }
        }

        private static void WriteElement (Aran.Geometries.Box value, XmlWriter writer)
        {
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:OGC:1.3:CRS84");
            writer.WriteElementString (AimdbNamespaces.GML, "lowerCorner", value [0].X + " " + value [0].Y);
            writer.WriteElementString (AimdbNamespaces.GML, "upperCorner", value [1].X + " " + value [1].Y);
        }

        public static string GetString (DateTime value)
        {
            return value.ToString ("yyyy-MM-ddThh:mm:ssZ");
        }

        public static string GetString (Guid value)
        {
            return value.ToString ();
        }

        public static string GetEnumString (object enumValue)
        {
            string enumText = enumValue.ToString ();
            if (enumText.StartsWith ("OTHER_"))
                enumText = "OTHER:" + enumText.Substring (6);
            return enumText;
        }
    }

    internal static class GeomConvertToXml
    {
        #region Point
        
        private static void FromPoint_20120113 (Point pnt, XmlWriter writer)
        {
            bool hasZ = !double.IsNaN (pnt.Z);
            if (hasZ)
                writer.WriteAttributeString ("srsDimension", "3");

            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:EPSG:4326");

            string pos = pnt.Y + " " + pnt.X + (hasZ ? " " + pnt.Z : "");

            writer.WriteElementString (AimdbNamespaces.GML, "pos", pos);
        }

        public static void FromPoint (Point pnt, XmlWriter writer)
        {

#if EUROCONTROL_NAMESPACE
            FromPoint_20120113 (pnt, writer);
            return;
#endif

#if COMSOFT_NEWGEO_NAMESPACE
            FromPoint_20120113 (pnt, writer);
            return;
#endif

            bool hasZ = !double.IsNaN (pnt.Z);
            //hasZ = false;
            if (hasZ)
                writer.WriteAttributeString ("srsDimension", "3");
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:OGC:1.3:CRS84");
            
            string pos = pnt.X + " " + pnt.Y + (hasZ ? " " + pnt.Z : "");

            writer.WriteElementString (AimdbNamespaces.GML, "pos", pos);
        }
        
        #endregion

        #region MultiLineString
        
        private static void FromMultiLineString_20120113 (MultiLineString mltLineString, XmlWriter writer)
        {
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:EPSG:4326");

            string posList = "";

            writer.WriteStartElement (AimdbNamespaces.GML, "segments");

            foreach (LineString lnString in mltLineString)
            {
                writer.WriteStartElement (AimdbNamespaces.GML, "LineStringSegment");
                writer.WriteStartElement (AimdbNamespaces.GML, "posList");

                posList = "";

                if (lnString.Count > 0 && !double.IsNaN (lnString [0].Z) && HasAnyNonZeroZ (lnString))
                {
                    writer.WriteAttributeString ("srsDimension", "3");
                    foreach (Point pnt in lnString)
                        posList += pnt.Y + " " + pnt.X + " " + pnt.Z + " ";
                }
                else
                {
                    writer.WriteAttributeString ("srsDimension", "2");
                    foreach (Point pnt in lnString)
                        posList += pnt.Y + " " + pnt.X + " ";
                }

                writer.WriteString (posList);
                writer.WriteFullEndElement ();
                writer.WriteFullEndElement ();
            }

            writer.WriteFullEndElement ();
        }

        public static void FromMultiLineString (MultiLineString mltLineString, XmlWriter writer)
        {
#if EUROCONTROL_NAMESPACE
            FromMultiLineString_20120113 (mltLineString, writer);
            return;
#endif
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:OGC:1.3:CRS84");
            
            string posList = "";

            writer.WriteStartElement (AimdbNamespaces.GML, "segments");

            foreach (LineString lnString in mltLineString)
            {
                writer.WriteStartElement (AimdbNamespaces.GML, "LineStringSegment");
                writer.WriteStartElement (AimdbNamespaces.GML, "posList");

                posList = "";
                //if (lnString.Count > 0 && !double.IsNaN (lnString [0].Z) && HasAnyNonZeroZ (lnString))
                //{
                //    writer.WriteAttributeString ("srsDimension", "3");
                //    foreach (Point pnt in lnString)
                //        posList += pnt.X + " " + pnt.Y + " " + pnt.Z + " ";
                //}
                //else
                //{
                //    //writer.WriteAttributeString ("srsDimension", "2");
                //    foreach (Point pnt in lnString)
                //        posList += pnt.X + " " + pnt.Y + " ";
                //}

                foreach (Point pnt in lnString)
                    posList += pnt.X + " " + pnt.Y + " ";


                writer.WriteString (posList);
                writer.WriteFullEndElement ();
                writer.WriteFullEndElement ();
            }

            writer.WriteFullEndElement ();
        }
        
        #endregion

        #region MultiPolygon

        private static void FromMultiPolygon_201201013 (MultiPolygon mltPolygon, XmlWriter writer)
        {
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:EPSG:4326");

            writer.WriteStartElement (AimdbNamespaces.GML, "patches");
            writer.WriteStartElement (AimdbNamespaces.GML, "PolygonPatch");
            writer.WriteStartElement (AimdbNamespaces.GML, "exterior");
            writer.WriteStartElement (AimdbNamespaces.GML, "Ring");
            {
                foreach (Polygon plygon in mltPolygon)
                {
                    writer.WriteStartElement (AimdbNamespaces.GML, "curveMember");
                    {
                        writer.WriteStartElement (AimdbNamespaces.GML, "Curve");
                        {
                            writer.WriteAttributeString (AimdbNamespaces.GML, "id", AranAixmWriter.GenerateNewGmlId ());

                            writer.WriteStartElement (AimdbNamespaces.GML, "segments");
                            {
                                writer.WriteStartElement (AimdbNamespaces.GML, "GeodesicString");
                                {
                                    writer.WriteStartElement (AimdbNamespaces.GML, "posList");
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
                        writer.WriteStartElement (AimdbNamespaces.GML, "interior");
                        writer.WriteStartElement (AimdbNamespaces.GML, "LinearRing");
                        writer.WriteStartElement (AimdbNamespaces.GML, "posList");

                        if (!double.IsNaN (rng [0].Z) && HasAnyNonZeroZ (rng))
                            writer.WriteAttributeString ("srsDimension", "3");

                        string posList = "";

                        foreach (Point pnt in rng)
                        {
                            posList += pnt.Y + " " + pnt.X + " ";
                        }

                        writer.WriteString (posList);

                        writer.WriteFullEndElement ();  //--- posList
                        writer.WriteFullEndElement ();  //--- LinearRing
                        writer.WriteFullEndElement ();  //--- interior
                    }

                    #endregion interior
                }
            }
            writer.WriteFullEndElement ();  //--- Ring
            writer.WriteFullEndElement ();  //--- exterior
            writer.WriteFullEndElement ();  //--- PolygonPatch
            writer.WriteFullEndElement ();  //--- patches
        }

        public static void FromMultiPolygon (MultiPolygon mltPolygon, XmlWriter writer)
        {
#if EUROCONTROL_NAMESPACE
            FromMultiPolygon_201201013 (mltPolygon, writer);
            return;
#endif
            writer.WriteAttributeString ("srsName", "urn:ogc:def:crs:OGC:1.3:CRS84");
            string posList = "";

            writer.WriteStartElement (AimdbNamespaces.GML, "patches");

            foreach (Polygon plygon in mltPolygon)
            {
                writer.WriteStartElement (AimdbNamespaces.GML, "PolygonPatch");
                writer.WriteStartElement (AimdbNamespaces.GML, "exterior");
                writer.WriteStartElement (AimdbNamespaces.GML, "LinearRing");
                writer.WriteStartElement (AimdbNamespaces.GML, "posList");

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

                foreach (Point pnt in plygon.ExteriorRing)
                    posList += pnt.X + " " + pnt.Y + " ";

                writer.WriteString (posList);

                writer.WriteFullEndElement ();  //--- posList
                writer.WriteFullEndElement ();  //--- LinearRing
                writer.WriteFullEndElement ();  //--- exterior

                foreach (Ring rng in plygon.InteriorRingList)
                {
                    writer.WriteStartElement (AimdbNamespaces.GML, "interior");
                    writer.WriteStartElement (AimdbNamespaces.GML, "LinearRing");
                    writer.WriteStartElement (AimdbNamespaces.GML, "posList");

                    if (!double.IsNaN (rng [0].Z) && HasAnyNonZeroZ (rng))
                        writer.WriteAttributeString ("srsDimension", "3");
                    //else
                    //    writer.WriteAttributeString ("srsDimension", "2");
                    
                    posList = "";
                    
                    foreach (Point pnt in rng)
                    {
                        posList += pnt.X + " " + pnt.Y + " ";
                    }
                    
                    writer.WriteString (posList);

                    writer.WriteFullEndElement ();  //--- interior
                    writer.WriteFullEndElement ();  //--- LinearRing
                    writer.WriteFullEndElement ();  //--- posList
                }

                writer.WriteFullEndElement ();  //--- PolygonPatch
            }
            writer.WriteFullEndElement ();  //--- patches
        }

        #endregion

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
    }
}
