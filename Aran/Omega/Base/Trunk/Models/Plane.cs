using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.Omega.Enums;
using NetTopologySuite.Geometries;
using Geometry = Aran.Geometries.Geometry;

namespace Aran.Omega.Models
{
    public enum CalculationType 
    {
        ByDistance,
        ByCoord
    }
    public class Plane
    {
        public Plane()
        {
            CalcType = CalculationType.ByCoord;
        }
        public PlaneParam Param { get; set; }

        public Ring Geo
        {
            get { return _geo; }
            set
            {
                _geo = value;
                if (_geo != null)
                {
                    JtsGeo =
                        Converters.ConverterJtsGeom.ConvertToJtsGeo.FromPolygon(new Aran.Geometries.Polygon
                        {
                            ExteriorRing = Geo
                        });
                }
            }
        }

        public GeoAPI.Geometries.IPolygon JtsGeo { get; set; }
        public CalculationType CalcType { get; set; }
        public Geometry RefGeometry;
        private Ring _geo;
    }

    public class PlaneParam
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }

        public double GetZ(Aran.Geometries.Point pt)
        {
            if (pt != null)
                return (-(A * pt.X + B * pt.Y + D));
            return 0;
        }

        public string CreateEquationStr(double elevation)
        {
            string equationStr = "Z = ";

            if (Math.Abs(A) > 0.0001)
            {
                double aValue = Math.Round(A, 5);
                if (Math.Abs(aValue) > 0.00001)
                    equationStr += (-1)*aValue + "*X";
            }

            if (Math.Abs(B) > 0.00001)
            {
                if (B > 0)
                    equationStr += "-" + Math.Round(B, 3) + "*Y ";
                else
                {
                    if (equationStr.Length > 5)
                        equationStr += "+" + (-1)*Math.Round(B, 3) + "*Y ";
                    else
                        equationStr += (-1)*Math.Round(B, 3) + "*Y ";
                }
            }

            if (equationStr.Length > 4)
            {
                if (D > 0)
                    equationStr += "-" + Math.Round(D, 1) + "=";
                else
                    equationStr += "+" + (-1)*Math.Round(D, 1) + "=";
            }

            equationStr += Common.ConvertAccuracy(elevation, RoundType.ToNearest, InitOmega.HeightConverter);

            if (InitOmega.HeightConverter.Unit == "ft")
                 equationStr += " (m) =" + Common.ConvertHeight(elevation, RoundType.ToNearest) + " ft ";
            
            return equationStr;
        }
    }


}
