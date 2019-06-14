using ESRI.ArcGIS.Geometry;
using Aran.Geometries.SpatialReferences;
using System;

namespace Aran.Geometries.Operators
{
	public class SpatRefConverter
    {
		public SpatRefConverter ( )
		{
		}

		public SpatialReference FromEsriSpatRef (ISpatialReference esriSpatialReference )
		{
            if (esriSpatialReference == null)
                return null;

			SpatialReference resultSpatRef = new SpatialReference ( );
			
            IGeographicCoordinateSystem geogCoordSystem = null;
			IProjection projection;
			ILinearUnit coordinateUnit;
			ISpheroid spheroid = null;
			ISpatialReference spatRefShp;

            resultSpatRef.Name = esriSpatialReference.Name;

			IProjectedCoordinateSystem projectedCoordSystem = esriSpatialReference as IProjectedCoordinateSystem;
			
            if ( projectedCoordSystem != null )
			{
				geogCoordSystem = projectedCoordSystem.GeographicCoordinateSystem;
				projection = projectedCoordSystem.Projection as IProjection;
				if ( projection != null )
				{
                    if ( projectedCoordSystem.Projection.Name == "Transverse_Mercator" )
						resultSpatRef.SpatialReferenceType = SpatialReferenceType.srtTransverse_Mercator;
					else if ( projectedCoordSystem.Projection.Name == "Mercator" )
						resultSpatRef.SpatialReferenceType = SpatialReferenceType.srtMercator;
					else if ( projectedCoordSystem.Projection.Name == "Gauss_Krueger" )
						resultSpatRef.SpatialReferenceType = SpatialReferenceType.srtGauss_Krueger;
                    else if (projectedCoordSystem.Projection.Name.Contains("Lambert"))
                        resultSpatRef.SpatialReferenceType = SpatialReferenceType.srtLambert_Conformal_Conic;
				}
				coordinateUnit = projectedCoordSystem.CoordinateUnit as ILinearUnit;
				if ( coordinateUnit != null )
				{
					if ( projectedCoordSystem.CoordinateUnit.Name == "Meter" )
						resultSpatRef.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
				}
			}

			if ( geogCoordSystem == null )
			{
				geogCoordSystem = esriSpatialReference as IGeographicCoordinateSystem;
				if ( geogCoordSystem != null )
					resultSpatRef.SpatialReferenceType = SpatialReferenceType.srtGeographic;
			}

			if ( geogCoordSystem != null )
			{
				spheroid = geogCoordSystem.Datum.Spheroid;
				spatRefShp = geogCoordSystem;
			}
			resultSpatRef.Ellipsoid.IsValid = ( spheroid != null );

			if ( resultSpatRef.Ellipsoid.IsValid )
			{
				if ( spheroid.Name == "WGS_1984" )
					resultSpatRef.Ellipsoid.SrGeoType = SpatialReferenceGeoType.srgtWGS1984;
				resultSpatRef.Ellipsoid.SemiMajorAxis = spheroid.SemiMajorAxis;
				resultSpatRef.Ellipsoid.Flattening = spheroid.Flattening;
			}

			switch ( resultSpatRef.SpatialReferenceType )
			{
				case SpatialReferenceType.srtGeographic:
					resultSpatRef.SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
					break;
				
				case SpatialReferenceType.srtMercator:
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian [ true ] ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptStandardParallel1, projectedCoordSystem.StandardParallel1 ) );
					break;
				
				case SpatialReferenceType.srtTransverse_Mercator:
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian [ true ] ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, 0.0 ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, projectedCoordSystem.ScaleFactor ) );
					break;
				
				case SpatialReferenceType.srtGauss_Krueger:
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian [ true ] ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptLatitudeOfOrigin, 0.0 ) );
					resultSpatRef.ParamList.Add ( new SpatialReferenceParam ( SpatialReferenceParamType.srptScaleFactor, projectedCoordSystem.ScaleFactor ) );
					break;
			    case SpatialReferenceType.srtLambert_Conformal_Conic:
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseEasting, projectedCoordSystem.FalseEasting));
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptFalseNorthing, projectedCoordSystem.FalseNorthing));
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptCentralMeridian, projectedCoordSystem.CentralMeridian[true]));
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptLatitudeOfOrigin, 0.0));
			        //resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptScaleFactor, projectedCoordSystem.ScaleFactor));
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptStandardParallel1, projectedCoordSystem.StandardParallel1));
			        resultSpatRef.ParamList.Add(new SpatialReferenceParam(SpatialReferenceParamType.srptStandardParallel2, projectedCoordSystem.StandardParallel2));
                    break;

                default:
					throw new NotImplementedException ( );
			}

			projectedCoordSystem = null;
			geogCoordSystem = null;
			spatRefShp = null;
			projection = null;
			coordinateUnit = null;
			spheroid = null;



			return resultSpatRef;
		}

		public ISpatialReference ToEsriSpatRef ( SpatialReference spatialReference )
		{
			IGeographicCoordinateSystem geogCoordSystem;
			ISpatialReferenceFactory spatRefFactory = new SpatialReferenceEnvironment ( );// as ISpatialReferenceFactory;
			IProjection projection;
			int paramCount;
			int esriParamCount;
			IParameter [ ] parameters = new IParameter [ 19 ];
			ILinearUnit linearUnit = null;
			IProjectedCoordinateSystem projCS;
			IProjectedCoordinateSystemEdit pcsEdit;
			IUnit projectedXYUnit;

			if ( spatRefFactory == null )
				return null;

			if ( spatialReference.SpatialReferenceType == SpatialReferenceType.srtGeographic )
			{
				geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.SrGeoType ) );
				spatRefFactory = null;
				return ( geogCoordSystem as ISpatialReference );
			}
			else
			{
				projection = spatRefFactory.CreateProjection ( ConvertSpatialReferenceType ( spatialReference.SpatialReferenceType ) );
				if ( projection == null )
				{
					spatRefFactory = null;
					return null;
				}
				paramCount = spatialReference.ParamList.Count;
				esriParamCount = paramCount;

				if ( esriParamCount < 20 )
					esriParamCount = 20;

				for ( int i = 0; i <= paramCount - 1; i++ )
				{
					parameters [ i ] = spatRefFactory.CreateParameter ( ConvertSpatialReferenceParamType ( spatialReference.ParamList [ i ].SRParamType ) );
					parameters [ i ].Value = spatialReference.ParamList [ i ].Value;
				}

				geogCoordSystem = spatRefFactory.CreateGeographicCoordinateSystem ( ConvertSpatialReferenceGeoType ( spatialReference.Ellipsoid.SrGeoType ) );
				if ( geogCoordSystem == null )
				{
					SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
					return null;
				}

				projectedXYUnit = spatRefFactory.CreateUnit ( ConvertSpatialReferenceUnit ( spatialReference.SpatialReferenceUnit ) );
				if ( projectedXYUnit == null )
				{
					SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
					return null;
				}

				linearUnit = projectedXYUnit as ILinearUnit;
				if ( linearUnit == null )
				{
					SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
					return null;
				}

				projCS = new ProjectedCoordinateSystem ( ) as IProjectedCoordinateSystem;
				if ( projCS == null )
				{
					SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
					return null;
				}

				pcsEdit = projCS as IProjectedCoordinateSystemEdit;
				if ( pcsEdit == null )
				{
					projCS = null;
					SetNull ( parameters, paramCount, projection, linearUnit, spatRefFactory, geogCoordSystem );
					return null;
				}

				object objParams = parameters as object;
				object name = spatialReference.Name as object;
				object geogCoordSystemObj = geogCoordSystem as object;
				object linearUnitObj = linearUnit as object;
				object projectionObj = projection as object;
				object emptyString = "";

				pcsEdit.Define ( ref name, ref name, ref emptyString, ref emptyString, ref emptyString, ref geogCoordSystemObj, ref linearUnitObj, ref projectionObj, ref objParams );

				return ( projCS as ISpatialReference );
			}
		}

		private int ConvertSpatialReferenceGeoType ( SpatialReferenceGeoType pandaSrGeoType )
		{
			switch ( pandaSrGeoType )
			{
				case SpatialReferenceGeoType.srgtWGS1984:
					return ( int )esriSRGeoCSType.esriSRGeoCS_WGS1984;
				case SpatialReferenceGeoType.srgtKrasovsky1940:
					return ( int )esriSRGeoCSType.esriSRGeoCS_Krasovsky1940;
				case SpatialReferenceGeoType.srgtNAD1983:
					return ( int )esriSRGeoCSType.esriSRGeoCS_NAD1983;
				default:
					return (int) esriSRGeoCSType.esriSRGeoCS_WGS1984;
			}
		}

		private int ConvertSpatialReferenceType ( SpatialReferenceType pandaSpatRefType )
		{
			switch ( pandaSpatRefType )
			{
				case SpatialReferenceType.srtMercator:
					return ( int ) esriSRProjectionType.esriSRProjection_Mercator;
				case SpatialReferenceType.srtTransverse_Mercator:
					return (int)esriSRProjectionType.esriSRProjection_TransverseMercator;
				case SpatialReferenceType.srtGauss_Krueger:
					return (int)esriSRProjectionType.esriSRProjection_GaussKruger;
			    case SpatialReferenceType.srtLambert_Conformal_Conic:
			        return (int)esriSRProjectionType.esriSRProjection_LambertConformalConic;
                default:
					return 0;
			}			
		}

		private int ConvertSpatialReferenceUnit ( SpatialReferenceUnit pandaSpatRefUnit )
		{
			switch ( pandaSpatRefUnit )
			{
				case SpatialReferenceUnit.sruMeter:
					return (int)esriSRUnitType.esriSRUnit_Meter;
				case SpatialReferenceUnit.sruFoot:
					return (int)esriSRUnitType.esriSRUnit_Foot;
				case SpatialReferenceUnit.sruNauticalMile:
					return (int)esriSRUnitType.esriSRUnit_NauticalMile;
				case SpatialReferenceUnit.sruKilometer:
					return (int)esriSRUnitType.esriSRUnit_Kilometer;
				default:
					return 0;
			}
		}

		private int ConvertSpatialReferenceParamType ( SpatialReferenceParamType pandaSpatRefParamType )
		{
			switch ( pandaSpatRefParamType )
			{
				case SpatialReferenceParamType.srptFalseEasting:
					return (int)esriSRParameterType.esriSRParameter_FalseEasting;
				case SpatialReferenceParamType.srptFalseNorthing:
					return (int)esriSRParameterType.esriSRParameter_FalseNorthing;
				case SpatialReferenceParamType.srptScaleFactor:
					return (int)esriSRParameterType.esriSRParameter_ScaleFactor;
				case SpatialReferenceParamType.srptAzimuth:
					return (int)esriSRParameterType.esriSRParameter_Azimuth;
				case SpatialReferenceParamType.srptCentralMeridian:
					return (int)esriSRParameterType.esriSRParameter_CentralMeridian;
				case SpatialReferenceParamType.srptLatitudeOfOrigin:
					return (int)esriSRParameterType.esriSRParameter_LatitudeOfOrigin;
				case SpatialReferenceParamType.srptLongitudeOfCenter:
					return (int)esriSRParameterType.esriSRParameter_LongitudeOfCenter;
                case SpatialReferenceParamType.srptStandardParallel1:
                    return (int)esriSRParameterType.esriSRParameter_StandardParallel1;
			    case SpatialReferenceParamType.srptStandardParallel2:
			        return (int)esriSRParameterType.esriSRParameter_StandardParallel2;
                default:
					return 0;
			}
		}

		private void SetNull ( IParameter [ ] parameters, int paramCount, IProjection projection, ILinearUnit linearUnit,
							ISpatialReferenceFactory spatRefFactory, IGeographicCoordinateSystem geogCoordSystem )
		{
			for ( int i = 0; i <= paramCount - 1; i++ )
			{
				parameters [ i ] = null;
			}
			projection = null;
			linearUnit = null;
			spatRefFactory = null;
			geogCoordSystem = null;
		}
	}
}
