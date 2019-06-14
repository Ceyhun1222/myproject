using System.Linq;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Geometries.SpatialReferences;
using System;

namespace Aran.Panda.Conventional.Racetrack
{
	public class SpatialReferenceOperation
	{
		public SpatialReferenceOperation ( )
		{
			if ( GlobalParams.AranEnvironment.Graphics.ViewProjection == null )
			{
				throw new Exception ( "Spatial Reference is null" );
			}

			CentralMeridian = GlobalParams.AranEnvironment.Graphics.ViewProjection.ParamList.
				Where ( spatialParam => ( spatialParam.SRParamType == SpatialReferenceParamType.srptCentralMeridian ) )
				.Select ( spatialParam => spatialParam.Value ).FirstOrDefault ( );

			CreateGeoSp ( );
			CreatePrjSp ( );	
		}
		
		public SpatialReferenceOperation ( Point ptGeo )
		{
			_ptGeo = ptGeo;
			CreateGeoSp();
			CreatePrjSp();
		}

        public SpatialReference PrjSp { get; private set; }

        public SpatialReference GeoSp { get; private set; }

		public double CentralMeridian { get; private set; }
                
        public T PrjToGeo<T>(T ptPrj) where T:Geometry
		{
            if (ptPrj == null)
                return null;
			return GlobalParams.GeomOperators.GeoTransformations(ptPrj, PrjSp, GeoSp) as T;			
		}

		public T GeoToPrj<T>(T ptGeo) where T:Geometry
		{
            if (ptGeo == null)
                return null;
			return GlobalParams.GeomOperators.GeoTransformations(ptGeo, GeoSp, PrjSp) as T;
		}

        public double AzToDirection(double azimuthInRad,Point ptGeo)
        {   
            return ARANFunctions.AztToDirection(ptGeo, azimuthInRad, GeoSp, PrjSp);
        }

        public double DirToAzimuth(double directionInAngle,Point ptPrj)
        { 
            return ARANFunctions.DirToAzimuth(ptPrj,directionInAngle,PrjSp,GeoSp);
        }

        public Point ChangeCentralMeridian(Point ptGeo)
        {
             if (ptGeo == null)
                return null;

            CentralMeridian = ptGeo.X;
            foreach (SpatialReferenceParam item in PrjSp.ParamList)
            {
                if (item.SRParamType == SpatialReferenceParamType.srptCentralMeridian)
                    item.Value = CentralMeridian;
            }
            
            double xMin,yMin,xMax,yMax;
			GlobalParams.AranEnvironment.Graphics.GetExtent ( out xMin, out yMin, out xMax, out yMax );

			GlobalParams.AranEnvironment.Graphics.ViewProjection = PrjSp;
            Point ptPrj = GeoToPrj(ptGeo);
            
            double radX = (xMax-xMin)/2,radY = (yMax-yMin)/2;

            GlobalParams.AranEnvironment.Graphics.SetExtent(ptPrj.X - radX, ptPrj.Y - radY, ptPrj.X + radX, ptPrj.Y + radY);
            return ptPrj;
        }

        public bool CheckPtInLIcenseArea(Point ptGeo)
        {
			return InitHolding.LicenseRectPrj.IsPointInside ( GeoToPrj ( ptGeo ) );
        }

		private void CreateGeoSp()
		{
			GeoSp = new SpatialReference();
			GeoSp.Name = "WGS1984";
			GeoSp.SpatialReferenceType = SpatialReferenceType.srtGeographic;
			GeoSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			GeoSp.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;			
			GeoSp.Ellipsoid.semiMajorAxis = 6378137.0;
			GeoSp.Ellipsoid.flattening = 1 / 298.25722356300003;
		}

		private void CreatePrjSp()
		{
			PrjSp = new SpatialReference();
			switch ( GlobalParams.AranEnvironment.Graphics.ViewProjection.SpatialReferenceType )
			{
				case SpatialReferenceType.srtGeographic:
					throw new NotImplementedException ( );
				case SpatialReferenceType.srtMercator:
					CreateMercatorPrjSp ( );
					break;
				case SpatialReferenceType.srtTransverse_Mercator:
					CreateTransverse_MercatorPrjSp ( );
					break;
				case SpatialReferenceType.srtGauss_Krueger:
					CreateGaussKruegerPrjSp ( );
					break;
				default:
					throw new NotImplementedException ( );
			}
		}

		private void CreateGaussKruegerPrjSp ( )
		{
			PrjSp.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
			PrjSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			PrjSp.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			PrjSp.Name = "WGS1984";

			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseEasting ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseNorthing ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, CentralMeridian ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptLatitudeOfOrigin ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptScaleFactor ] ) );
		}

		private void CreateTransverse_MercatorPrjSp ( )
		{
			PrjSp.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
			PrjSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			PrjSp.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			PrjSp.Name = "WGS1984";

			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseEasting ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseNorthing ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, CentralMeridian ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptLatitudeOfOrigin ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptScaleFactor ] ) );
		}


		private void CreateMercatorPrjSp ( )
		{
			PrjSp.SpatialReferenceType = SpatialReferenceType.srtMercator;
			PrjSp.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
			PrjSp.Ellipsoid.srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			PrjSp.Name = "WGS1984";

			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseEasting ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptFalseNorthing ] ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, CentralMeridian ) );
			PrjSp.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, GlobalParams.AranEnvironment.Graphics.ViewProjection [ SpatialReferenceParamType.srptStandardParallel1 ] ) );
		}
				   				   
		private Point _ptGeo;
	}
}
