﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Package;
using Aran.Temporality.Common.Util;

namespace ATRACC.Converter.Util
{
    public class GeoFormatter
    {
        public static void CopyGeometry(Aran.Geometries.Geometry from, Aran.Geometries.Geometry to)
        {
            if (from == null) return;
            if (to == null) return;
            if (to.Type != from.Type) return;

            using (var s = MemoryUtil.GetMemoryStream())
            {
                using (var packageWriter = new BinaryPackageWriter(s))
                {
                    from.Pack(packageWriter);
                }
                s.Position = 0;
                using (var packageReader = new BinaryPackageReader(s))
                {
                    to.Unpack(packageReader);
                }
            }
        }



        public static string FormatX(double v)
        {
            return v.ToString();
        }

        public static string FormatY(double v)
        {
            return v.ToString();
        }

        public static string FormatZ(double v)
        {
            return v.ToString();
        }

        public static string FormatYXdms(double x, double y)
        {
            string result = "";
            //
            try
            {
                int degree;
                int minute;
                double second;

                int sgn = double.IsNaN(y) ? 1 : Math.Sign(y);

                Dd2Dms(y, out degree, out minute, out second, sgn);

                var direction = (EarthXDirection)((sgn + 1) >> 1);
                result += degree.ToString("D2") + minute.ToString("D2") + second.ToString("00.") + direction;
            }
            catch
            {
                return null;
            }
            //
            try
            {
                int degree;
                int minute;
                double second;

                int sgn = double.IsNaN(x) ? 1 : Math.Sign(x);

                Dd2Dms(x, out degree, out minute, out second, sgn);

                var direction = (EarthXDirection)((sgn + 1) >> 1);
                result += degree.ToString("D2") + minute.ToString("D2") + second.ToString("00.") + direction;
            }
            catch
            {
                return null;
            }
            //
         
            return result;
        }

        public static string FormatXdms(double v)
        {
            try
            {
                int degree;
                int minute;
                double second;

                int sgn = double.IsNaN(v) ? 1 : Math.Sign(v);

                Dd2Dms(v, out degree, out minute, out second, sgn);

                //if (Math.Round(m_Sec, ntSeconds.Accuracy) == 60)
                //{
                //    m_Sec = 0;
                //    m_Min++;
                //    if (Math.Round(m_Min) == 60)
                //    {
                //        m_Min = 0;
                //        m_Deg++;
                //    }
                //}



                var direction = (EarthXDirection)((sgn + 1) >> 1);
                return degree + "° " + minute.ToString(MinuteFormat) + "' " + second.ToString(SecondFormat) + "\" " + direction;
            }
            catch
            {
                return "NaN";
            }

        }

        const string MinuteFormat = "D2";
        const string SecondFormat = "00.00";

        public static string FormatYdms(double v)
        {
            try
            {
                int degree;
                int minute;
                double second;

                int sgn = Math.Sign(v);

                Dd2Dms(v, out degree, out minute, out second, sgn);

                //if (Math.Round(m_Sec, ntSeconds.Accuracy) == 60)
                //{
                //    m_Sec = 0;
                //    m_Min++;
                //    if (Math.Round(m_Min) == 60)
                //    {
                //        m_Min = 0;
                //        m_Deg++;
                //    }
                //}

                var direction = (EarthYDirection)((sgn + 1) >> 1);
                return degree + "° " + minute.ToString(MinuteFormat) + "' " + second.ToString(SecondFormat) + "\" " + direction;
            }
            catch
            {
                return "NaN";
            }

        }

        public enum EarthXDirection
        {
            W,
            E,
        }

        public enum EarthYDirection
        {
            S,
            N,
        }

        private static int Fix(double x)
        {
            return (int)(Math.Sign(x) * Math.Floor(Math.Abs(x)));
        }

        private static void Dd2Dms(double val, out int xDeg, out int xMin, out double xSec, int sign)
        {
            var x = Math.Abs(Math.Round(Math.Abs(val) * sign, 10));
            xDeg = Fix(x);

            var dx = (x - xDeg) * 60;
            dx = Math.Round(dx, 8);

            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            //xSec = Math.Round(xSec, 6);
        }


    }
}
