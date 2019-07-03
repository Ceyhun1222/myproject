using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Aran.Aim.GmlParser
{
    public static class AranMathFunctions
    {
        [DllImport("MathFunctions.dll", EntryPoint = "_InitAll@0")]
        public static extern void InitAll();

        [DllImport("MathFunctions.dll", EntryPoint = "_PointAlongGeodesic@40")]
        public static extern int PointAlongGeodesic(double X, double Y, double Dist, double Azimuth, out double ResX, out double ResY);

        [DllImport("MathFunctions.dll", EntryPoint = "_ReturnGeodesicDistance@32", CallingConvention = CallingConvention.StdCall)]
        public static extern double ReturnGeodesicDistance(double X0, double Y0, double X1, double Y1);

        [DllImport("MathFunctions.dll", EntryPoint = "_ReturnGeodesicAzimuth@40")]
        public static extern int ReturnGeodesicAzimuth(double X0, double y0, double X1, double Y1, out double DirectAzimuth, out double InverseAzimuth);

        [DllImport("MathFunctions.dll", EntryPoint = "_Calc2VectIntersect@56", CallingConvention = CallingConvention.StdCall)]
        public static extern System.Int32 Calc2VectIntersect(double x0, double y0, double azimuth0, double x1, double y1, double Azimuth1, out double resx, out double resy);

        public static double Modulus(double x, double y = 360.0)
        {
            x = x - Math.Floor(x / y) * y;

            if (x < 0.0)
                x = x + y;

            if (x == y)
                return 0.0;

            return x;
        }
    }
}
