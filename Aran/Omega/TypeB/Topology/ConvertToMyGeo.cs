
using Aran.Geometries;
using System;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Collections;
using System.Collections.Generic;

namespace Aran.Omega.Topology
{
    public static class ConvertToMyGeo
    {
        public static GeoAPI.Geometries.IGeometry FromGeometry(Aran.Geometries.Geometry aranGeom)
        {
            switch (aranGeom.Type)
            {
                case GeometryType.Point:
                    return FromPoint((Aran.Geometries.Point)aranGeom);
                case GeometryType.LineString:
                    return FromLineString((Aran.Geometries.LineString)aranGeom);
                case GeometryType.Polygon:
                    return FromPolygon((Aran.Geometries.Polygon)aranGeom);
                case GeometryType.MultiLineString:
                    return FromMultiLineString((Aran.Geometries.MultiLineString)aranGeom);
                case GeometryType.MultiPolygon:
                    return FromMultiPolygon((Aran.Geometries.MultiPolygon)aranGeom);
                case GeometryType.Ring:
                    return (IGeometry)FromRing((Aran.Geometries.Ring)aranGeom);

                //Aran.Geometries.Polygon plygon = new Aran.Geometries.Polygon();
                //plygon.Add((Aran.Geometries.Ring)aranGeom);
                //return FromPolygon ( plygon );

            }
            throw new Exception("Geometry Type is not implemented (in FromGeometry)!");
        }

        public static ILineString FromLineString(Aran.Geometries.LineString lineString)
        {
            Coordinate[] points = new Coordinate[lineString.Count];
            for (int i = 0; i < lineString.Count; i++)
            {
                points[i] = FromPoint(lineString[i]).Coordinate;
            }
            ILineString result = new NetTopologySuite.Geometries.LineString(points);
            return result;
        }

        public static ILinearRing FromRing(Aran.Geometries.Ring rng)
        {
            var list = new List<Coordinate>();
            for (int i = 0; i < rng.Count; i++)
			{
			    list.Add((Coordinate)FromPoint(rng[i]).Coordinate);
			}
          
            if (!list[0].Equals2D(list[rng.Count-1]))
                list.Add(list[0]);

            ILinearRing result = new LinearRing(list.ToArray());
            return result;
        }

        public static IMultiLineString FromMultiLineString(Aran.Geometries.MultiLineString aranMultiLineString)
        {

            ILineString[] lineStrings = new NetTopologySuite.Geometries.LineString[aranMultiLineString.Count];
            for (int i = 0; i < aranMultiLineString.Count; i++)
			{
			    lineStrings[i] = ConvertToMyGeo.FromLineString(aranMultiLineString[i]);
			}
            IMultiLineString result = new NetTopologySuite.Geometries.MultiLineString(lineStrings);
            return result;
        }

        public static IPolygon FromPolygon(Aran.Geometries.Polygon aranPolygon)
        {
            IPolygon result = null;
            ILinearRing extRng = ConvertToMyGeo.FromRing(aranPolygon.ExteriorRing);

            var interiorRingCount = aranPolygon.InteriorRingList.Count;
            if (interiorRingCount > 0)
            {
                ILinearRing[] interiorRings = new LinearRing[interiorRingCount];
                for (int i = 0; i < interiorRingCount; i++)
                {
                    ILinearRing rng = FromRing(aranPolygon.InteriorRingList[i]);
                    interiorRings[i] = rng;
                }
                result = new NetTopologySuite.Geometries.Polygon(extRng, interiorRings);
            }
            else
                result = new NetTopologySuite.Geometries.Polygon(extRng);

            return result;
        }

        public static IMultiPolygon FromMultiPolygon(Aran.Geometries.MultiPolygon aranMultiPolygon)
        {
            IPolygon[] polygons = new NetTopologySuite.Geometries.Polygon[aranMultiPolygon.Count];
            for (int i = 0; i < aranMultiPolygon.Count; i++)
            {
                polygons[i]= FromPolygon(aranMultiPolygon[i]);
            }
            IMultiPolygon result = new NetTopologySuite.Geometries.MultiPolygon(polygons);
            return result;
        }

        //With Z aware
        public static IPoint FromPoint(Aran.Geometries.Point aranPoint)
        {
            IPoint result = new NetTopologySuite.Geometries.Point(aranPoint.X,aranPoint.Y,aranPoint.Z);
            return result;
        }
     
    }
}
