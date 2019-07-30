using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aran.Geometries;
using System.Runtime.InteropServices;
using Aran.Aim.GmlParser;

namespace Aran.Aim.Converter
{
    internal class GeomConverterFromXml
    {
        private GeomConverterFromXml (XmlReader xmlReader)
        {
            _xmlReader = xmlReader;
            _gmlNS = "http://www.opengis.net/gml/3.2";
            _aixmNS = "http://www.aixm.aero/schema/5.1";
        }

        #region Static Functions

        public static Point ToPoint (XmlReader reader)
        {
            var conv = new GeomConverterFromXml (reader);
            return conv.ToPoint ();
        }

        public static MultiLineString ToMultiLineString(XmlReader reader)
        {
            return new GeomConverterFromXml(reader).ToMultiLineString();
        }

        public static MultiPolygon ToMultiPolygon(XmlReader reader, bool isElevated)
        {
            return new GeomConverterFromXml(reader).ToMultiPolygon(isElevated);
        }

        #endregion

        private Point ToPoint()
        {
            Point result = new Point();
            string[] coords = null;
            double x, y, z;

            bool? isLatitudeFirst = IsLatitudeFirst();

            coords = ParsePointElement(isLatitudeFirst);
            if (coords == null)
            {
                return result;
            }

            double.TryParse(coords[0].Trim(), out x);
            double.TryParse(coords[1].Trim(), out y);

            result.SetCoords(x, y);

            if (coords.Length > 2)
            {
                double.TryParse(coords[2], out z);
                result.Z = z;
            }
            return result;
        }

        private MultiLineString ToMultiLineString ()
        {
            MultiLineString result = new MultiLineString ();

            int dimension = GetDimension ();
            bool? isLatitudeFirst = IsLatitudeFirst ();

            int depth = _xmlReader.Depth;

            while (_xmlReader.Read ())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element &&
                    depth == _xmlReader.Depth - 1 &&
                    _xmlReader.LocalName == "segments")
                {
                    var mls = ParseCurveSegments (dimension, isLatitudeFirst);
                    result.Add (mls);
                }
                else if (_xmlReader.NodeType == XmlNodeType.EndElement &&
                    depth == _xmlReader.Depth)
                {
                    break;
                }
            }

            return result;
        }

		private MultiPolygon ToMultiPolygon ( bool isElevated )
		{
			MultiPolygon multiPolygon = null;

			string localName = "";
			string ns = "";


			bool isLatitudeFirst = false;
			string srsName = _xmlReader.GetAttribute ( "srsName" );
			if ( srsName != null )
			{
				if ( srsName.Contains ( "EPSG" ) && srsName.EndsWith ( "4326" ) )
					isLatitudeFirst = true;
			}

			int dimension = GetDimension ( );

			localName = ( isElevated ? "ElevatedSurface" : "Surface" );

			ns = _aixmNS;
			_xmlReader.ReadStartElement ( localName, ns );
			PassWhiteSpaces ( );

			localName = "patches";
			ns = _gmlNS;
			_xmlReader.ReadStartElement ( localName, ns );
			PassWhiteSpaces ( );

			multiPolygon = new MultiPolygon ( );

			int depth = _xmlReader.Depth;

			while ( _xmlReader.Depth >= depth )
			{
				if ( _xmlReader.NodeType == XmlNodeType.EndElement )
				{
					_xmlReader.ReadEndElement ( );
					PassWhiteSpaces ( );
					continue;
				}

				PassWhiteSpaces ( );

				localName = "PolygonPatch";
				ns = _gmlNS;
				_xmlReader.ReadStartElement ( localName, ns );
				PassWhiteSpaces ( );

				localName = "exterior";
				ns = _gmlNS;
				_xmlReader.ReadStartElement ( localName, ns );
				PassWhiteSpaces ( );

				Ring rng = ReadRingOrLinearRing ( dimension, isLatitudeFirst );
				rng.IsExterior = true;

				Polygon polygon = new Polygon ( );
				polygon.ExteriorRing = rng;

				while ( _xmlReader.LocalName != "exterior" )
				{
					PassWhiteSpaces ( );
					//if ( _xmlReader.NodeType == XmlNodeType.EndElement )
					_xmlReader.ReadEndElement ( );
					//else
					//break;
				}
				_xmlReader.ReadEndElement ( );

				//by Andrey, added support of interior rings
				try
				{
					localName = "interior";
					ns = _gmlNS;
					//if (_xmlReader.LocalName == localName)
					//{
					while ( _xmlReader.LocalName == localName )
					{
						_xmlReader.ReadStartElement ( localName, ns );
						PassWhiteSpaces ( );

						Ring interiorRing = ReadRingOrLinearRing ( dimension, isLatitudeFirst );

						interiorRing.IsExterior = false;
						polygon.InteriorRingList.Add ( interiorRing );
						//while ( true )
						//{
						//	PassWhiteSpaces ( );
						//	if ( _xmlReader.NodeType == XmlNodeType.EndElement )
						//		_xmlReader.ReadEndElement ( );
						//	else
						//		break;
						//}
						while ( _xmlReader.LocalName != localName )
						{
							PassWhiteSpaces ( );
							_xmlReader.ReadEndElement ( );
						}
						_xmlReader.ReadEndElement ( );
						//_xmlReader.ReadEndElement ( );
						//_xmlReader.ReadEndElement ( );
						//_xmlReader.ReadEndElement ( );
						//PassWhiteSpaces ( );

						//if (_xmlReader.LocalName == localName)
						//{
						//	break;
						//}
						//}
					}

				}
				catch ( Exception )
				{
				}

				multiPolygon.Add ( polygon );
			}

			//while (_xmlReader.Depth > depth)
			//{
			//    PassWhiteSpaces();
			//    if (_xmlReader.NodeType == XmlNodeType.EndElement)
			//        _xmlReader.ReadEndElement();
			//    else
			//        break;
			//}

			return multiPolygon;
		}

		private MultiPolygon ToMultiPolygon_ORG (bool isElevated)
        {
            MultiPolygon multiPolygon = null;

            string localName = "";
            string ns = "";
            int depth = _xmlReader.Depth;

            bool isLatitudeFirst = false;
            string srsName = _xmlReader.GetAttribute ("srsName");
            if (srsName != null)
            {
                if (srsName.Contains ("EPSG") && srsName.EndsWith ("4326"))
                    isLatitudeFirst = true;
            }

            int dimension = GetDimension ();

            try
            {
				localName = (isElevated ? "ElevatedSurface" : "Surface");

                ns = _aixmNS;
                _xmlReader.ReadStartElement (localName, ns);
                PassWhiteSpaces ();

                localName = "patches";
                ns = _gmlNS;
                _xmlReader.ReadStartElement (localName, ns);
                PassWhiteSpaces ();

                localName = "PolygonPatch";
                ns = _gmlNS;
                _xmlReader.ReadStartElement (localName, ns);
                PassWhiteSpaces ();

                localName = "exterior";
                ns = _gmlNS;
                _xmlReader.ReadStartElement (localName, ns);
                PassWhiteSpaces ();

                Ring rng = ReadRingOrLinearRing (dimension, isLatitudeFirst);
                rng.IsExterior = true;

                Polygon polygon = new Polygon ();
                polygon.ExteriorRing = rng;

                while (_xmlReader.Depth > depth)
                {
                    PassWhiteSpaces();
                    if (_xmlReader.NodeType == XmlNodeType.EndElement)
                        _xmlReader.ReadEndElement();
                    else
                        break;
                }

                //by Andrey, added support of interior rings
                try
                {
                    localName = "interior";
                    ns = _gmlNS;
					if(_xmlReader.LocalName == localName)
					{
						_xmlReader.ReadStartElement (localName, ns);
						PassWhiteSpaces ();

						while (true)
						{
							Ring interiorRing = ReadRingOrLinearRing (dimension, isLatitudeFirst);

							interiorRing.IsExterior = false;
							polygon.InteriorRingList.Add (interiorRing);
							PassWhiteSpaces ();
							_xmlReader.ReadEndElement ();
							PassWhiteSpaces ();

							if (_xmlReader.LocalName == localName)
							{
								break;
							}
						}
					}

                }
                catch (Exception)
                {
                }


                while (_xmlReader.Depth > depth)
                {
                    PassWhiteSpaces();
                    if (_xmlReader.NodeType == XmlNodeType.EndElement)
                        _xmlReader.ReadEndElement();
                    else
                        break;
                }

                multiPolygon = new MultiPolygon ();
                multiPolygon.Add (polygon);
            }
            catch (Exception ex)
            {
                throw new Exception ("Expected element name " + localName, ex);
            }

            return multiPolygon;
        }

        private Ring ReadRingOrLinearRing (int dimension, bool isLatitudeFirst)
        {
            Ring result = null;

            PassWhiteSpaces ();

            if (_xmlReader.NamespaceURI == _gmlNS)
            {
                if (_xmlReader.LocalName == "Ring")
                {
                    _xmlReader.ReadStartElement ();
                    result = ReadRing (dimension, isLatitudeFirst);
                }
                else if (_xmlReader.LocalName == "LinearRing")
                {
                    _xmlReader.ReadStartElement ();
                    result = new Ring ();
                    ParsePointListElement (result, dimension, isLatitudeFirst);
                }
                else
                {
                    throw new Exception ("Expected element name Ring or LinearRing");
                }
            }
            else
            {
                throw new Exception ("Expected element namespace " + _gmlNS);
            }

            PassWhiteSpaces ();
            //_xmlReader.ReadEndElement ();
            return result;
        }


        private bool CheckForLattitudeFirst()
        {
            if (_xmlReader.HasAttributes)
            {
                string srsName;
                if ((srsName = _xmlReader.GetAttribute("srsName")) != null)
                {
                    if (srsName.Contains("EPSG") && srsName.EndsWith("4326"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

//        private Ring ReadRing (int dimension, bool isLatitudeFirst)
//        {
//            string localName = null;
//            string ns = null;
//            var ring = new Ring();

//            PassWhiteSpaces ();

//            try
//            {
//                var depth = _xmlReader.Depth;
//                var dontRead = true;
//                MultiPoint multiPoint = new MultiPoint();
//                while (dontRead || _xmlReader.Read())
//                {
//                    localName = "curveMember";
//                    ns = _gmlNS;
//                    _xmlReader.ReadStartElement(localName, ns);
//                    isLatitudeFirst |= CheckForLattitudeFirst();
//                    PassWhiteSpaces();
//                    localName = "Curve";
//                    ns = _gmlNS;
//                    _xmlReader.ReadStartElement(localName, ns);

//                    #region

//#warning 2015.12.11 LEVC_LNAV Data Curve namespace is not gml. To parse the xml file we changed it.
//                    //_xmlReader.ReadStartElement(localName, _aixmNS);
//                    #endregion
//                    isLatitudeFirst |= CheckForLattitudeFirst();
//                    dontRead = false;

//                    if (_xmlReader.NodeType == XmlNodeType.Element &&
//                        depth == _xmlReader.Depth - 1)
//                    {
//                        if (_xmlReader.LocalName == "ArcByCenterPoint")
//                        {
//                            multiPoint.AddMultiPoint(
//                                ComplexGeomParser.ArcToMultiLineString(_xmlReader.ReadInnerXml(), isLatitudeFirst));
//                            if (multiPoint.Count > 0)
//                                ring.AddMultiPoint(multiPoint);
//                            dontRead = true;
//                        }
//                        else if (_xmlReader.LocalName == "Arc")
//                        {
//                            multiPoint.AddMultiPoint(ComplexGeomParser.ArcBy3PointsToMultiLineString(
//                                _xmlReader.ReadInnerXml(),
//                                isLatitudeFirst == true));
//                            if (multiPoint.Count > 0)
//                                ring.AddMultiPoint(multiPoint);
//                            //dontRead = true;
//                        }
//                        else if (_xmlReader.LocalName == "GeodesicString" || _xmlReader.LocalName == "Geodesic" ||
//                                 _xmlReader.LocalName == "LineStringSegment")
//                        {
//                            _xmlReader.ReadStartElement(_xmlReader.LocalName, _gmlNS);
//                            isLatitudeFirst |= CheckForLattitudeFirst();
//                            PassWhiteSpaces();

//                            ParsePointListElement(ring, dimension, isLatitudeFirst);
//                        }
//                        else if (_xmlReader.LocalName == "CircleByCenterPoint")
//                        {
//                            var mp = ComplexGeomParser.CircleToRing(_xmlReader.ReadOuterXml(), isLatitudeFirst);
//                            if (mp != null)
//                                ring.AddMultiPoint(mp);
//                        }
//                    }
//                    if (_xmlReader.NodeType == XmlNodeType.EndElement &&
//                        depth == _xmlReader.Depth)
//                    {
//                        break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (localName != null)
//                    ex = new Exception("Expected element " + localName, ex);

//                throw ex;
//            }

//            return ring;
//        }

        private Ring ReadRing(int dimension, bool isLatitudeFirst)
        {
            string localName = null;
            string ns = null;
            var resultMltPnt = new MultiPoint();

            PassWhiteSpaces();

            try
            {
                //var depth = _xmlReader.Depth;
                //var dontRead = true;
                MultiPoint multiPoint = new MultiPoint();
                do
                {
                    localName = "curveMember";
                    ns = _gmlNS;
                    _xmlReader.ReadStartElement(localName, ns);
                    isLatitudeFirst |= CheckForLattitudeFirst();
                    PassWhiteSpaces();

                    localName = "Curve";
                    ns = _gmlNS;
                    _xmlReader.ReadStartElement(localName, ns);
                    PassWhiteSpaces();
                    isLatitudeFirst |= CheckForLattitudeFirst();

                    localName = "segments";
                    ns = _gmlNS;
                    _xmlReader.ReadStartElement(localName, ns);
                    PassWhiteSpaces();
                    isLatitudeFirst |= CheckForLattitudeFirst();

                    #region

                    do
                    {
                        if (_xmlReader.LocalName == "ArcByCenterPoint")
                        {
                            multiPoint =
                                ComplexGeomParser.ArcToMultiLineString(_xmlReader.ReadInnerXml(), isLatitudeFirst);
                            if (multiPoint.Count > 0)
                                resultMltPnt.AddMultiPoint(multiPoint);
                            //dontRead = true;
                        }
                        else if (_xmlReader.LocalName == "Arc")
                        {
                            multiPoint = ComplexGeomParser.ArcBy3PointsToMultiLineString(
                                _xmlReader.ReadInnerXml(),
                                isLatitudeFirst == true);
                            if (multiPoint.Count > 0)
                                resultMltPnt.AddMultiPoint(multiPoint);
                            //dontRead = true;
                        }
                        else if (_xmlReader.LocalName == "GeodesicString" || _xmlReader.LocalName == "Geodesic" ||
                                 _xmlReader.LocalName == "LineStringSegment")
                        {
                            var beginTagName = _xmlReader.LocalName;
                            _xmlReader.ReadStartElement(_xmlReader.LocalName, _gmlNS);
                            isLatitudeFirst |= CheckForLattitudeFirst();
                            PassWhiteSpaces();

                            ParsePointListElement(resultMltPnt, dimension, isLatitudeFirst);
                            _xmlReader.ReadEndElement();
                            while (_xmlReader.NodeType == XmlNodeType.EndElement && _xmlReader.LocalName == beginTagName)
                            {
                                _xmlReader.ReadEndElement();
                            }
                        }
                        else if (_xmlReader.LocalName == "CircleByCenterPoint")
                        {
                            var mp = ComplexGeomParser.CircleToRing(_xmlReader.ReadOuterXml(), isLatitudeFirst);
                            if (mp != null)
                                resultMltPnt.AddMultiPoint(mp);
                        }
                    } while (_xmlReader.LocalName != "segments" || _xmlReader.NodeType != XmlNodeType.EndElement);
                    ////End of segments
                    _xmlReader.ReadEndElement();
                    ////End of Curve
                    _xmlReader.ReadEndElement();
                    ////End of CurveMember
                    _xmlReader.ReadEndElement();
                    #endregion
                } while (_xmlReader.LocalName != "Ring" || _xmlReader.NodeType != XmlNodeType.EndElement);
            }
            catch (Exception ex)
            {
                if (localName != null)
                    ex = new Exception("Expected element " + localName, ex);
                throw ex;
            }
            var rng = new Ring();
            rng.AddMultiPoint(resultMltPnt);
            return rng;
        }

        private string [] ParsePointElement (bool? isLatitudeFirst = null)
        {
            if (isLatitudeFirst == null)
                isLatitudeFirst = IsLatitudeFirst();

            string [] result = null;
            _xmlReader.ReadStartElement ();
            PassWhiteSpaces ();

            switch (_xmlReader.LocalName)
            {
                case "pos":
                    result = _xmlReader.ReadString().Trim().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                    _xmlReader.ReadEndElement ();
                    break;
                case "coordinates":
                    result = _xmlReader.ReadString().Trim().Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    _xmlReader.ReadEndElement ();
                    break;
                default:
                    result = null;
                    _xmlReader.Skip ();
                    break;
            }

            if (result != null && (isLatitudeFirst != null && isLatitudeFirst.Value))
            {
                if (result.Length > 1)
                {
                    string tmp = result [0];
                    result [0] = result [1];
                    result [1] = tmp;
                }
            }

            return result;
        }

        private int GetDimension ()
        {
            int result = 2;
            if (_xmlReader.MoveToAttribute ("srsDimension"))
                int.TryParse (_xmlReader.Value, out result);
            _xmlReader.MoveToElement ();
            return result;
        }

        private MultiLineString ParseCurveSegments (int dimension, bool? isLatitudeFirst)
        {
            MultiLineString result = new MultiLineString ();

            int depth = _xmlReader.Depth;

            while (_xmlReader.Read ())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element &&
                    depth == _xmlReader.Depth - 1)
                {
                    if (_xmlReader.LocalName == "ArcByCenterPoint")
					{
                        var multiPoint = ComplexGeomParser.ArcToMultiLineString(_xmlReader.ReadInnerXml(), true == isLatitudeFirst);
                        if (multiPoint != null && multiPoint.Count > 0)
                        {
                            var ls = new LineString();
                            ls.AddMultiPoint(multiPoint);
                            result.Add(ls);
                        }
					}
                    else if (_xmlReader.LocalName == "Arc")
                    {
                        var multiPoint =
                            ComplexGeomParser.ArcBy3PointsToMultiLineString(_xmlReader.ReadInnerXml(),
                                isLatitudeFirst == true);
                        if (multiPoint != null && multiPoint.Count > 0)
                            result.Add(multiPoint);
                    }

                    if (_xmlReader.LocalName == "LineStringSegment")
					{
						var lineString = new LineString ();
						PassWhiteSpaces ();
						_xmlReader.ReadStartElement ();
						ParsePointListElement (lineString, dimension, isLatitudeFirst);
						result.Add (lineString);
					}
                    else if (_xmlReader.LocalName == "Geodesic" || _xmlReader.LocalName == "GeodesicString")
					{
						var lineString = new LineString ();
						_xmlReader.ReadStartElement ();
						ParsePointListElement (lineString, dimension, isLatitudeFirst);
						result.Add (lineString);
					}

                }
                else if (_xmlReader.NodeType == XmlNodeType.EndElement &&
                    depth == _xmlReader.Depth)
                {
                    break;
                }
            }

            return result;
        }

        private void ParsePointListElement(MultiPoint result, int dimension, bool? isLatitudeFirst)
        {
            string[] coords = null;
            var tmpCoordList = new List<string>();

            string srsDimensionText;
            string srsName;

            int depth = _xmlReader.Depth;

            do
            {
                if (_xmlReader.NodeType == XmlNodeType.EndElement && _xmlReader.Depth < depth)
                {
                    break;
                }
                else if (_xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (_xmlReader.HasAttributes)
                    {
                        if ((srsDimensionText = _xmlReader.GetAttribute("srsDimension")) != null)
                        {
                            int.TryParse(srsDimensionText, out dimension);
                        }
                        if ((srsName = _xmlReader.GetAttribute("srsName")) != null)
                        {
                            if (srsName.Contains("EPSG") && srsName.EndsWith("4326"))
                                isLatitudeFirst = true;
                        }
                    }

                    if (_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Depth == depth)
                    {
                        string innerTxt;
                        switch (_xmlReader.LocalName)
                        {
                            case "posList":
                                innerTxt = _xmlReader.ReadString().Replace(Environment.NewLine, " ")
                                    .Replace("\n", " ")
                                    .Replace("\r", " ").Trim();
                                coords = innerTxt.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case "coordinates":
                                innerTxt = _xmlReader.ReadString().Replace(Environment.NewLine, " ")
                                    .Replace("\n", " ")
                                    .Replace("\r", " ").Trim();
                                coords = innerTxt.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case "pos":
                            case "pointProperty":
                            case "pointRep":
                                GetCoordList(tmpCoordList);
                                break;
                            default:
                                result = null;
                                break;
                        }

                        if (coords != null)
                            break;
                    }
                }
            } while (_xmlReader.Read());

            if (coords == null)
            {
                if (tmpCoordList.Count > 0)
                    coords = tmpCoordList.ToArray();
                else
                    return;
            }

            int index = 0;
            double x, y, z;
            Aran.Geometries.Point pnt;

            if (dimension == 0)
                dimension = 2;

            while (index <= coords.Length - 1)
            {
                if (true == isLatitudeFirst)
                {
                    double.TryParse(coords[index].Trim(), out y);
                    index++;
                    double.TryParse(coords[index].Trim(), out x);
                }
                else
                {
                    double.TryParse(coords[index].Trim(), out x);
                    index++;
                    double.TryParse(coords[index].Trim(), out y);
                }

                index++;
                pnt = new Aran.Geometries.Point(x, y);

                if (dimension == 3)
                {
                    double.TryParse(coords[index].Trim(), out z);
                    index++;
                    pnt.Z = z;
                }
                result.Add(pnt);
            }
        }

        private void GetCoordList (List<string> coordList)
        {
            List<string> result = new List<string> ();
			string [] coords = null;

			if (_xmlReader.LocalName == "pos")
			{
                coords = _xmlReader.ReadString().Trim().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			}
			else if (_xmlReader.LocalName == "pointProperty" || _xmlReader.LocalName == "pointRep")
			{
				_xmlReader.ReadStartElement ();
				PassWhiteSpaces ();
				coords = ParsePointElement ();
				_xmlReader.ReadEndElement ();
			}

			if (coords != null)
				coordList.AddRange (coords);

			//while (_xmlReader.NodeType != XmlNodeType.EndElement ||
			//    _xmlReader.LocalName != "LineStringSegment" &&
			//    _xmlReader.LocalName != "LinearRing")
			//{
			//    string [] coords = null;

			//    if (_xmlReader.LocalName == "pos")
			//    {
			//        var s = _xmlReader.ReadString ().Trim ();
			//        coords = s.Split (' ');
			//    }
			//    else if (_xmlReader.LocalName == "pointProperty" || _xmlReader.LocalName == "pointRep")
			//    {
			//        _xmlReader.ReadStartElement ();
			//        PassWhiteSpaces ();
			//        coords = ParsePointElement ();
			//        _xmlReader.ReadEndElement ();
			//    }

			//    if (coords != null)
			//    {
			//        for (int j = 0; j < coords.Length; j++)
			//            result.Add (coords [j]);
			//    }

			//    _xmlReader.ReadEndElement ();
			//    PassWhiteSpaces ();
			//}
			//return result.ToArray ();
        }

        private void PassWhiteSpaces ()
        {
            while (_xmlReader.NodeType == XmlNodeType.Whitespace)
                _xmlReader.Skip ();
        }

        private bool? IsLatitudeFirst ()
        {
            string srsName = _xmlReader.GetAttribute ("srsName");
            if (srsName != null)
            {
                return (srsName.Contains ("EPSG") && srsName.EndsWith ("4326"));
            }
            return null;
        }

        private XmlReader _xmlReader;
        private string _gmlNS;
        private string _aixmNS;
    }

	internal class CommonMathFunctions
	{
		[DllImport ("MathFunctions.dll", CharSet = CharSet.Ansi, EntryPoint = "_ATan2@16")]
		public static extern double ATan2 (double Y, double X);

		[DllImport ("MathFunctions.dll", CharSet = CharSet.Ansi, EntryPoint = "_Modulus@16")]
		public static extern double Modulus (double X, double Y);

		public static Point PointAlongPlane (Point from, double dirAngle, double dist)
		{
			var x = from.X + dist * Math.Cos (DegToRad (dirAngle));
			var y = from.Y + dist * Math.Sin (DegToRad (dirAngle));
			return new Point (x, y);
		}

		public static double DegToRad (double x)
		{
			return x * Math.PI / 180.0;
		}

		public static double RadToDeg (double x)
		{
			return x * 180.0 / Math.PI;
		}

		public static MultiPoint CreateArcPrj (Point ptCnt, Point ptFrom, Point ptTo, int ClWise)
		{
			var Pt = new Point ();
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;
			double R = Math.Sqrt (dX * dX + dY * dY);
			double AztFrom = RadToDeg (ATan2 (dY, dX));
			AztFrom = Modulus (AztFrom, 360.0);
			double AztTo = RadToDeg (ATan2 (ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X));
			AztTo = Modulus (AztTo, 360.0);
			double daz = Modulus ((AztTo - AztFrom) * (double) ClWise, 360.0);
			double AngStep = 1.0;

			var I = (int) (daz / AngStep);

			if (I < 1)
				I = 1;
			else
			{
				if (I < 5)
					I = 5;
				else
				{
					if (I < 10)
						I = 10;
				}
			}

			var result = new MultiPoint ();

			AngStep = daz / (double) I;
			result.Add (ptFrom);

			for (int J = 1; J < I; J++)
			{
				double iInRad = DegToRad (AztFrom + (double) J * AngStep * (double) ClWise);
				var ptX = ptCnt.X + R * Math.Cos (iInRad);
				var ptY = ptCnt.Y + R * Math.Sin (iInRad);
				result.Add (new Point (ptX, ptY));
			}
			result.Add (ptTo);
			return result;
		}

		public static MultiPoint CreateAcrByAnglePrj (Point centerPoint, double radius, double startDir, double endDir)
		{
			var pt1 = PointAlongPlane (centerPoint, startDir, radius);
			var pt2 = PointAlongPlane (centerPoint, endDir, radius);
			return CreateArcPrj (centerPoint, pt1, pt2, 1);
		}
	}

	//public class ArcByMultiLineStringText : MultiLineString
	//{
	//	public string XmlText { get; set; }
	//}

	//public class CircleByCenterPointText : Ring
	//{
	//	public string XmlText { get; set; }
	//}
}

//namespace Aran.Aim.Other
//{
//	using Aran.Aim.Converter;

	//public static class ArcByCenterPointReadingAction
	//{
	//	public static Action<MultiLineString, ArcByMultiLineStringText> Action { get; set; }
	//}

	//public static class CircleByCenterPointReadingAction
	//{
	//	public static Action<Ring, CircleByCenterPointText> Action {get;set;}
	//}
//}
