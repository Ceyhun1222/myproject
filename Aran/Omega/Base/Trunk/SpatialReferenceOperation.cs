using System.Linq;
using System.Runtime.InteropServices;
using Aran.Geometries.SpatialReferences;
using Aran.Geometries;
using Aran.Panda.Common;
using System;

namespace Aran.Omega
{
    //public class SpatialReferenceOperation_
    //{
    //    [DllImport("MathFunctions.dll", EntryPoint = "_InitProjection@40")]
    //    public static extern void InitProjection(double Lm0, double Lp0, double Sc, double Efalse, double Nfalse);

    //    [DllImport("MathFunctions.dll", EntryPoint = "_InitEllipsoid@16")]
    //    public static extern void InitEllipsoid(double equatoralRadius, double inverseFlatting);

    //    [DllImport("MathFunctions.dll", EntryPoint = "_InitAll@0")]
    //    public static extern void InitAll();
        
    //    public SpatialReferenceOperation(Point ptGeo)
    //    {
    //        _ptGeo = ptGeo;
    //        CreateGeoSp();
    //    }

    //    public SpatialReferenceOperation()
    //    {
    //        CentralMeridian = GlobalParams.UI.ViewProjection.ParamList.
    //            Where(spatialParam => (spatialParam.SRParamType == SpatialReferenceParamType.srptCentralMeridian))
    //            .Select(spatialParam => spatialParam.Value).FirstOrDefault();
    //        CreateGeoSp();
    //        PrjSp = GlobalParams.GeomOperators.
    //        //InitAll();

    //        //InitProjection(24, 0,
    //        //     0.9996,
    //        //    500000.0,
    //        //    0.0);
    //        //InitEllipsoid(6378137.0, 298.25722356300003);
    //    }

    //    public SpatialReference PrjSp { get; private set; }

    //    public SpatialReference GeoSp { get; private set; }

    //    public double CentralMeridian { get; private set; }
                
    //    public T PrjToGeo<T>(T ptPrj) where T:Geometry
    //    {
    //        if (ptPrj == null)
    //            return null;
    //        return GlobalParams.GeomOperators.GeoTransformations(ptPrj, PrjSp, GeoSp) as T;			
    //    }

    //    public T ToPrj<T>(T ptGeo) where T:Geometry
    //    {
    //        if (ptGeo == null)
    //            return null;
    //        return GlobalParams.GeomOperators.GeoTransformations(ptGeo, GeoSp, PrjSp) as T;
    //    }

    //    public double AzToDirection(double azimuthInAngle,Point ptGeo)
    //    {   
    //        return ARANFunctions.AztToDirection(ptGeo, azimuthInAngle, GeoSp, PrjSp);
    //    }

    //    public double DirToAzimuth(double directionInRadian,Point ptPrj)
    //    {
    //        return ARANFunctions.DirToAzimuth(ptPrj, directionInRadian, PrjSp, GeoSp);
    //    }

    //    public Point ChangeCentralMeridian(Point ptGeo)
    //    {
    //         if (ptGeo == null)
    //            return null;

    //        CentralMeridian = ptGeo.X;
    //        foreach (SpatialReferenceParam item in PrjSp.ParamList)
    //        {
    //            if (item.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
    //                item.Value = Math.Round(CentralMeridian);
    //        }
            
    //        double xMin,yMin,xMax,yMax;
    //        GlobalParams.UI.GetExtent(out xMin, out yMin, out xMax, out yMax);
        
    //        GlobalParams.UI.ViewProjection = PrjSp;
    //        Aran.Geometries.Point ptPrj = ToPrj(ptGeo);
            
    //        double radX = (xMax-xMin)/2,radY = (yMax-yMin)/2;

    //        GlobalParams.UI.SetExtent(ptPrj.X - radX, ptPrj.Y - radY, ptPrj.X + radX, ptPrj.Y + radY);
    //        return ptPrj;
    //    }

    //    //public bool CheckPtInLIcenseArea(Point ptGeo)
    //    //{
    //    //    return InitOmega.LicenseRectGeo.IsPointInside(ToPrj(ptGeo));
    //    //}

    //    private void CreateGeoSp()
    //    {
    //        GeoSp = new SpatialReference();
    //        GeoSp.Name = "WGS1984";
    //        GeoSp.SpatialReferenceType = SpatialReferenceType.srtGeographic;
    //        GeoSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
    //        GeoSp.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
    //        GeoSp.Ellipsoid.SemiMajorAxis = 6378137.0;
    //        GeoSp.Ellipsoid.Flattening = 1 / 298.25722356300003;
    //    }

    //    private Point _ptGeo;
    //}
}
