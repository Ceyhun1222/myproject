using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    class HorizontalAccuracyCalculator
    {
        public HorizontalAccuracyCalculator()
        {

        }

        public double CalcHorisontalAccuracy(Aran.Geometries.Point ptFix, NavaidType GuidanceNav, NavaidType IntersectNav)
        {
            if (GuidanceNav.TypeCode == eNavaidType.DME) return 0;
            if (IntersectNav.TypeCode == eNavaidType.NONE) return 0;
            //if (IntersectNav.ValCnt == -2) return 0;

            const double distEps = 0.0001;
            double sqrt1_2 = 0.5 * Math.Sqrt(2.0);

            double sigL2;

            double GuidDir =ARANFunctions.ReturnAngleInDegrees(GuidanceNav.GeoPrj, ptFix);
            double dNavNav, LNavNav = ARANFunctions.ReturnDistanceInMeters(GuidanceNav.GeoPrj, IntersectNav.GeoPrj);

            if (LNavNav < distEps * distEps)
            {
                sigL2 = 0.5 * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
                dNavNav = GuidDir;
            }
            else
            {
                dNavNav =ARANFunctions.ReturnAngleInDegrees(GuidanceNav.GeoPrj, IntersectNav.GeoPrj);

                double dX = IntersectNav.GeoPrj.X - GuidanceNav.GeoPrj.X;
                double dY = IntersectNav.GeoPrj.Y - GuidanceNav.GeoPrj.Y;

                double sigX = 0.5 * dX * dX / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
                double sigY = 0.5 * dY * dY / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);

                sigL2 = sigX + sigY;
            }

            double sigT2 = GlobalParams.Settings.DeltaInterface.AnglePrecision* ARANMath.DegToRadValue;
            sigT2 = sigT2 * sigT2;

            double ted1 = ARANMath.SubtractAngles(dNavNav, GuidDir) * ARANMath.DegToRadValue;
            double sinT1 = Math.Sin(ted1);
            double cosT1 = Math.Cos(ted1);
            double dY3dX3 = Math.Tan(ted1);

            double dX3dT1, dY3dT1;
            double sigY3, sigX3;

            if (IntersectNav.TypeCode == eNavaidType.DME)
            {
                double sigD2 = Common.DeConvertDistance(GlobalParams.Settings.DeltaInterface.DistancePrecision);
                sigD2 = sigD2 * sigD2;

                double GuidDist = ARANFunctions.ReturnDistanceInMeters(IntersectNav.GeoPrj, ptFix);
                double sqRoot = Math.Sqrt(GuidDist * GuidDist - LNavNav * LNavNav * sinT1 * sinT1);
                double recip = 1.0 / sqRoot;

                double dX3dL = cosT1 * cosT1 + LNavNav * cosT1 * sinT1 * sinT1 * recip;    //(14)
                double dX3dD = GuidDist * cosT1 * recip;                                   //(15)
                dX3dT1 = 2.0 * LNavNav * cosT1 * sinT1 + sinT1 * sqRoot + cosT1 * cosT1 * sinT1 * LNavNav * LNavNav * recip;    //(16)

                double sigX3_2 = dX3dL * dX3dL * sigL2 + dX3dD * dX3dD * sigD2 + dX3dT1 * dX3dT1 * sigT2;
                sigX3 = Math.Sqrt(sigX3_2);                                         //(17)

                double X3 = LNavNav * cosT1 * cosT1 + cosT1 * sqRoot;                      //(13)
                dY3dT1 = X3 / (cosT1 * cosT1);

                sigY3 = Math.Sqrt(dY3dX3 * dY3dX3 * sigX3_2 + dY3dT1 * dY3dT1 * sigT2);
            }
            else
            {
                double IntersectDir = ARANFunctions.ReturnAngleInDegrees(IntersectNav.GeoPrj, ptFix);
                double ted2 = ARANMath.SubtractAngles(dNavNav, IntersectDir) * ARANMath.DegToRadValue;

                double dX3dY3 = 1.0 / dY3dX3;                                              //(7)
                double ctT1T2 = dX3dY3 + 1.0 / Math.Tan(ted2);

                double dY3dL = 1.0 / ctT1T2;
                double Y3 = LNavNav * dY3dL;

                dX3dT1 = -Y3 / (sinT1 * sinT1);                                       //(8)

                double fTmp = sinT1 * ctT1T2;
                dY3dT1 = -LNavNav / (fTmp * fTmp);

                fTmp = Math.Sin(ted2) * ctT1T2;
                double dY3dT2 = -LNavNav / (fTmp * fTmp);

                double sigY3_2 = dY3dL * dY3dL * sigL2 + dY3dT1 * dY3dT1 * sigT2 + dY3dT2 * dY3dT2 * sigT2;
                sigY3 = Math.Sqrt(sigY3_2);
                sigX3 = Math.Sqrt(dX3dY3 * dX3dY3 * sigY3_2 + dX3dT1 * dX3dT1 * sigT2);
            }

            double result = Math.Sqrt(sigX3 * sigX3 + sigY3 * sigY3);
            return result;
        }
    }
}
