using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmsControll
{
    public class Functions
    {
        public static double DMS2DD(double xDeg, double xMin, double xSec, int Sign)
        {
            double x;
            x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
            return System.Math.Abs(x);
        }

        public static void DD2DMS(double val, out double xDeg, out double xMin, out double xSec, int Sign)
        {
            double x;
            double dx;

            x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

            xDeg = Fix(x);
            dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);
        }
      
        private static int Fix(double x)
        {
            // TODO: fix temporary solution
            if (double.IsNaN(x))
                return 0;
            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }


        public static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }
    }
}
