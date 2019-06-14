using ObstacleCalculator.Domain.Models;
using System;

namespace ObstacleCalculator.Domain.Utils
{
    public class PlaneParamCalculator
    {
        public static PlaneParam CalcPlaneParam(GeoAPI.Geometries.Coordinate ptA, GeoAPI.Geometries.Coordinate ptB, GeoAPI.Geometries.Coordinate ptC)
        {
            PlaneParam fPlane = new PlaneParam();
            fPlane.C = ptA.X * (ptB.Y - ptC.Y) + ptB.X * (ptC.Y - ptA.Y) + ptC.X * (ptA.Y - ptB.Y);
            fPlane.A = (ptA.Y * (ptB.Z - ptC.Z) + ptB.Y * (ptC.Z - ptA.Z) + ptC.Y * (ptA.Z - ptB.Z))/fPlane.C;
            fPlane.B = (ptA.Z * (ptB.X - ptC.X) + ptB.Z * (ptC.X - ptA.X) + ptC.Z * (ptA.X - ptB.X))/fPlane.C;
            fPlane.D = (-(ptA.X * (ptB.Y * ptC.Z - ptC.Y * ptB.Z) + ptB.X * (ptC.Y * ptA.Z - ptA.Y * ptC.Z) + ptC.X * (ptA.Y * ptB.Z - ptB.Y * ptA.Z)))/fPlane.C;
            return fPlane;
        }

        //public static PlaneParam CalcPlaneParam(GeoAPI.Geometries.Coordinate ptA, GeoAPI.Geometries.Coordinate ptB, GeoAPI.Geometries.Coordinate ptC)
        //{
        //    if (ptA == null || ptB == null || ptC == null)
        //        throw new ArgumentNullException("Parametrs cannot be null");

        //    PlaneParam fPlane = new PlaneParam();
        //    fPlane.C = (ptB.X-ptA.X)*(ptC.Y-ptA.Y)-(ptC.X-ptA.X)*(ptB.Y-ptA.Y);
        //    fPlane.A = (ptB.Y-ptA.Y)* (ptC.Z - ptA.Z) - (ptC.Y - ptA.Y) *(ptB.Z - ptA.Z);
        //    fPlane.B = (ptB.Z -ptA.Z) *(ptC.X - ptA.X) - (ptC.Z - ptA.Z) * (ptB.X - ptA.X);
        //    fPlane.D = -(fPlane.A*ptA.X+fPlane.B*ptA.Y+fPlane.C*ptA.Z);
        //    return fPlane;
        //}
    }
}
