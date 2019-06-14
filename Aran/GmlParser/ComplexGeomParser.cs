using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aran.Geometries;

namespace Aran.Aim.GmlParser
{
	public static class ComplexGeomParser
    {
        public static Ring CircleToRing(string xmlText, bool isLatitudeFirst)
		{
            double? radius;
            var pt = GetCircleByCenterPoint(xmlText, isLatitudeFirst, out radius);

            if (pt == null || radius == null)
                throw new Exception("Point or radius has not been determined");

            double resX = double.NaN;
            double resY = double.NaN;

            var multiPoint = new MultiPoint();
            AranMathFunctions.InitAll();
            for (int i = 0; i <= 359; i++)
            {
                AranMathFunctions.PointAlongGeodesic(pt.X, pt.Y, radius.Value, i, out resX, out resY);
                multiPoint.Add(new Point(resX, resY));
            }
            var ring = new Ring();
            ring.AddMultiPoint(multiPoint);
            return ring;



            //var doc = new XmlDocument ();
            //doc.LoadXml (xmlText);

            //Point pt = null;
            //double? radius = null;

            //try
            //{
            //    #region Get Params

            //    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            //    {
            //        if (node.NodeType == XmlNodeType.Element)
            //        {
            //            var elem = node as XmlElement;

            //            switch (elem.LocalName)
            //            {
            //                case "pos":
            //                    var sa = elem.InnerText.ToString ().Split (" ".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
            //                    pt = new Point (double.Parse (sa[0]), double.Parse (sa[1]));
            //                    break;
            //                case "radius":
            //                    var uomAttr = elem.Attributes["uom"];
            //                    var radiusVal = double.Parse (elem.InnerText);
            //                    radius = GetRadius (uomAttr, radiusVal);
            //                    break;
            //                case "pointProperty":
            //                    {
            //                        var pointElem = elem.FirstChild as XmlElement;
            //                        if (pointElem.LocalName == "Point")
            //                            return CircleToRing(pointElem.OuterXml, isLatitudeFirst);
            //                    }
            //                    break;
                                
            //            }
            //        }
            //    }

            //    #endregion

            //    if (pt != null && radius != null)
            //    {
            //        double resX = double.NaN;
            //        double resY = double.NaN;

            //        var multiPoint = new MultiPoint ();
            //        AranMathFunctions.InitAll();
            //        for (int i = 0; i <= 359; i++)
            //        {
            //            AranMathFunctions.PointAlongGeodesic (pt.X, pt.Y, radius.Value, i, out resX, out resY);
            //            multiPoint.Add (new Point (resX, resY));
            //        }
            //        var ring = new Ring ();
            //        ring.AddMultiPoint (multiPoint);
            //        return ring;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //throw new Exception ("Point or radius has not been determined");
		}

        private static Point GetCircleByCenterPoint(string xmlText, bool isLatitudeFirst, out double? radius)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlText);

            Point pt = null;
            radius = null;

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    var elem = node as XmlElement;

                    switch (elem.LocalName)
                    {
                        case "pos":
                            var sa = elem.InnerText.ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            pt = new Point(double.Parse(sa[0]), double.Parse(sa[1]));
                            break;
                        case "radius":
                            var uomAttr = elem.Attributes["uom"];
                            var radiusVal = double.Parse(elem.InnerText);
                            radius = GetRadius(uomAttr, radiusVal);
                            break;
                        case "pointProperty":
                            {
                                var pointElem = elem.FirstChild as XmlElement;
                                if (pointElem.LocalName == "Point")
                                    pt = GetCircleByCenterPoint(pointElem.OuterXml, isLatitudeFirst, out radius);
                                isLatitudeFirst = false;
                                break;
                            }
                    }
                }
            }

            if (pt != null && isLatitudeFirst)
                pt = new Point(pt.Y, pt.X);

            return pt;
        }

		public static MultiPoint ArcToMultiLineString (string xmlText, bool isLatitudeFirst)
		{
			var doc = new XmlDocument ();
			doc.LoadXml ("<doc>" + xmlText + "</doc>");

			Point pt = null;
			double? radius = null;
			double? startAngle = null;
			double? endAngle = null;

			try
			{
				#region Get Params

				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
				{
					if (node.NodeType == XmlNodeType.Element)
					{
						var elem = node as XmlElement;

						switch (elem.LocalName)
						{
							case "pos":
								var sa = elem.InnerText.ToString ().Split (" ".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
                                double d1, d2;
                                if (!double.TryParse(sa[0], out d1) || !double.TryParse(sa[1], out d2))
                                    throw new Exception("pos is invalid format!");
                                
                                if (isLatitudeFirst)
								    pt = new Point(d2, d1);
                                else
                                    pt = new Point(d1, d2);

								break;
							case "radius":
								var uomAttr = elem.Attributes["uom"];
								var radiusVal = double.Parse (elem.InnerText);
								radius = GetRadius (uomAttr, radiusVal);
								break;
							case "startAngle":
								startAngle = double.Parse (elem.InnerText);
								break;
							case "endAngle":
								endAngle = double.Parse (elem.InnerText);
								break;
						}
					}
				}

				#endregion

				if (pt != null && radius != null && startAngle != null && endAngle != null)
				{
                    double sAngle;
                    double eAngle;

                    if (!isLatitudeFirst)
                    {
                        sAngle = 90D - startAngle.Value;
                        eAngle = 90D - endAngle.Value;
                    }
                    else
                    {
                        sAngle = startAngle.Value;
                        eAngle = endAngle.Value;
                    }

                    var dir = 1;

                    var dAz = eAngle - sAngle;
					//var dAz = AranMathFunctions.Modulus(eAngle - sAngle, 360D);

#warning Must change and adapt to documented algorithm.
                    //if (dAz > 180D)
                    if (dAz < 0)
                    {
                        dir = -dir;
                        //dAz = AranMathFunctions.Modulus((eAngle - sAngle) * dir, 360D);
                    }

                    dAz = AranMathFunctions.Modulus((eAngle - sAngle) * dir, 360D);

					int n = (int) Math.Floor (dAz);

					if (n == 0) n = 1;
					else if (n <= 5) n = 5;
					else if (n <= 10) n = 10;

					double AngStep = dAz / n;
					double r = radius.Value;
					double resX = double.NaN;
					double resY = double.NaN;

					var multiPoint = new MultiPoint ();

					for (int i = 0; i <= n; i++)
					{
						var azmt = sAngle + i * AngStep * dir;
					    AranMathFunctions.InitAll();
                        AranMathFunctions.PointAlongGeodesic (pt.X, pt.Y, r, azmt, out resX, out resY);
						multiPoint.Add (new Point (resX, resY));
					}

                    return multiPoint;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			throw new Exception ("Some of {point, radius, startAngle, endAngle} has not been determined.");
		}

		//private AG.MultiLineString ArcToMultiLineString_OLD (Aran.Aim.Converter.ArcByMultiLineStringText arcBy, ASR.SpatialReference geoSR)
		//{
		//	var doc = new XmlDocument ();
		//	doc.LoadXml (arcBy.XmlText);

		//	AG.Point pt = null;
		//	double? radius = null;
		//	double? startAngle = null;
		//	double? endAngle = null;

		//	try
		//	{
		//		foreach (XmlNode node in doc.DocumentElement.ChildNodes)
		//		{
		//			if (node.NodeType == XmlNodeType.Element)
		//			{
		//				var elem = node as XmlElement;

		//				switch (elem.LocalName)
		//				{
		//					case "pos":
		//						var sa = elem.InnerText.ToString ().Split (" ".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
		//						pt = new AG.Point (double.Parse (sa[0]), double.Parse (sa[1]));
		//						break;
		//					case "radius":
		//						var uomAttr = elem.Attributes["uom"];
		//						var radiusVal = double.Parse (elem.InnerText);
		//						UomDistance uomDist;
		//						if (Enum.TryParse<UomDistance> (uomAttr.Value, true, out uomDist))
		//						{
		//							var vd = new ValDistance (radiusVal, uomDist);
		//							var tmp = Aran.Converters.ConverterToSI.Convert (vd, double.NaN);
		//							if (tmp != double.NaN)
		//								radius = tmp;
		//						}
		//						break;
		//					case "startAngle":
		//						startAngle = double.Parse (elem.InnerText);
		//						break;
		//					case "endAngle":
		//						endAngle = double.Parse (elem.InnerText);
		//						break;
		//				}
		//			}
		//		}

		//		if (pt != null && radius != null && startAngle != null && endAngle != null)
		//		{
		//			var prjSR = CreateUtmPrjSR (pt.X);
		//			//var prjSR = CreateUtmPrjSR(54);

		//			var geoOper = new Aran.Geometries.Operators.GeometryOperators ();
		//			var prjPoint = geoOper.GeoTransformations (pt, geoSR, prjSR) as AG.Point;

		//			if (prjPoint != null)
		//			{
		//				var startAngleDirInRad = 90 - startAngle.Value;
		//				var endAngleDirInRad = 90 - endAngle.Value;

		//				startAngleDirInRad = PANDA.Common.ARANFunctions.AztToDirection (pt, startAngleDirInRad, geoSR, prjSR);
		//				endAngleDirInRad = PANDA.Common.ARANFunctions.AztToDirection (pt, endAngleDirInRad, geoSR, prjSR);

		//				var cw = PANDA.Common.TurnDirection.CW;

		//				double dAz = PANDA.Common.ARANMath.Modulus ((startAngleDirInRad - endAngleDirInRad) * (int) cw, PANDA.Common.ARANMath.C_2xPI);

		//				if (dAz < PANDA.Common.ARANMath.C_PI)
		//					cw = PANDA.Common.TurnDirection.CCW;

		//				var prjPoints = PANDA.Common.ARANFunctions.CreateArcGeo (prjPoint, radius.Value,
		//					startAngleDirInRad,
		//					endAngleDirInRad,
		//					cw);

		//				var mls = new AG.MultiLineString ();
		//				var ls = new AG.LineString ();
		//				foreach (AG.Point prjPointItem in prjPoints)
		//				{
		//					var geoPointItem = geoOper.GeoTransformations (prjPointItem, prjSR, geoSR) as AG.Point;
		//					ls.Add (geoPointItem);
		//				}
		//				mls.Add (ls);

		//				return mls;
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}

		//	return null;
		//}

		private static double GetRadius (XmlAttribute uomAttr, double radiusVal)
		{
            if (uomAttr != null && uomAttr.Value != null)
            {
                var upperVal = uomAttr.Value.ToUpper();

                if (upperVal == "NM" || upperVal == "[NMI_I]")
                    return radiusVal * 1852.2;
                if (upperVal == "KM")
                    return radiusVal * 1000.0;
                if (upperVal == "M")
                    return radiusVal;
                if (upperVal == "FT")
                    return radiusVal * 0.3048;
                if (upperVal == "MI")
                    return radiusVal * 1609.344;
                if (upperVal == "CM")
                    return radiusVal * 0.01;
                else
                    throw new Exception(uomAttr.Value + " (UOM) is not implemented for Circle");
            }
            throw new Exception("UOM is not defined for Circle");
		}

        public static LineString ArcBy3PointsToMultiLineString(string xmlText, bool isLatitudeFirst)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<doc>" + xmlText + "</doc>");

            string[] coords = null;
            var tmpCoordList = new List<string>();

            int dimension = 2;
            try
            {
                #region Get Params

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var elem = node as XmlElement;
                        switch (elem.LocalName)
                        {
                            case "posList":
                                coords = elem.InnerText.ToString().Trim()
                                    .Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case "coordinates":
                                coords = elem.InnerText.ToString().Trim().Split(',');
                                break;
                        }
                    }
                }

                #endregion

                if (coords == null)
                    throw new Exception("Couldn't parse points of Arc");

                int index = 0;
                double x, y, z;
                List<Point> pointList = new List<Point>();
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
                    var pnt = new Aran.Geometries.Point(x, y);

                    if (dimension == 3)
                    {
                        double.TryParse(coords[index].Trim(), out z);
                        index++;
                        pnt.Z = z;
                    }
                    pointList.Add(pnt);
                }

                AranMathFunctions.InitAll();
                double dirAz1, dirInverseAz1, dirAz2, dirInverseAz2, midAngle, tmp, startAngle, endAngle;
                AranMathFunctions.ReturnGeodesicAzimuth(pointList[0].X, pointList[0].Y, pointList[1].X, pointList[1].Y,
                    out dirAz1, out dirInverseAz1);
                var dist1 = AranMathFunctions.ReturnGeodesicDistance(pointList[0].X, pointList[0].Y, pointList[1].X,
                    pointList[1].Y);


                AranMathFunctions.ReturnGeodesicAzimuth(pointList[1].X, pointList[1].Y, pointList[2].X, pointList[2].Y,
                    out dirAz2, out dirInverseAz2);
                var dist2 = AranMathFunctions.ReturnGeodesicDistance(pointList[1].X, pointList[1].Y, pointList[2].X,
                    pointList[2].Y);

                AranMathFunctions.PointAlongGeodesic(pointList[0].X, pointList[0].Y, dist1 * 0.5, dirAz1, out x,
                        out y);
                var pointMid12 = new Point(x, y);

                AranMathFunctions.PointAlongGeodesic(pointList[1].X, pointList[1].Y, dist2 * 0.5, dirAz2, out x,
                    out y);
                var pointMid23 = new Point(x, y);


                AranMathFunctions.Calc2VectIntersect(pointMid12.X, pointMid12.Y, dirAz1 + 90, pointMid23.X,
                    pointMid23.Y, dirAz2 + 90, out x, out y);

                var centerPoint = new Point(x, y);

                AranMathFunctions.ReturnGeodesicAzimuth(centerPoint.X, centerPoint.Y, pointList[0].X, pointList[0].Y, out startAngle, out tmp);
                AranMathFunctions.ReturnGeodesicAzimuth(centerPoint.X, centerPoint.Y, pointList[2].X, pointList[2].Y, out endAngle, out tmp);

                AranMathFunctions.ReturnGeodesicAzimuth(centerPoint.X, centerPoint.Y, pointList[1].X, pointList[1].Y,
                    out midAngle, out tmp);

                var dir = 1;
                var dAz = AranMathFunctions.Modulus(midAngle - startAngle, 360D);
#warning Must change and adapt to documented algorithm.
                if (dAz > 180D)
                //if (dAz < 0)
                {
                    dir = -dir;
                    //dAz = AranMathFunctions.Modulus((eAngle - sAngle) * dir, 360D);
                }

                dAz = AranMathFunctions.Modulus((endAngle - startAngle) * dir, 360D);
                int n = (int) Math.Floor(dAz);

                if (n == 0) n = 1;
                else if (n <= 5) n = 5;
                else if (n <= 10) n = 10;

                double angStep = dAz / n;
                double r = AranMathFunctions.ReturnGeodesicDistance(pointList[0].X, pointList[0].Y, centerPoint.X,
                    centerPoint.Y);
                double resX, resY;
                var result = new LineString();
                for (int i = 0; i <= n; i++)
                {
                    var azmt = startAngle + i * angStep * dir;
                    AranMathFunctions.PointAlongGeodesic(centerPoint.X, centerPoint.Y, r, azmt, out resX, out resY);
                    result.Add(new Point(resX, resY));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

            throw new Exception("Some of points have not been determined.");
        }
    }
}
