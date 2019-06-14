using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Geometries.SpatialReferences
{
	public enum SpatialReferenceGeoType
	{
		srgtWGS1984 = 1,
		srgtKrasovsky1940,
		srgtNAD1983
	};

	public enum SpatialReferenceType
	{
		srtGeographic = 1,
		srtMercator,
		srtTransverse_Mercator,
		srtGauss_Krueger,
        srtLambert_Conformal_Conic
    };

	public enum SpatialReferenceParamType
	{
		srptFalseEasting = 1,
		srptFalseNorthing,
		srptScaleFactor,
		srptAzimuth,
		srptCentralMeridian,
		srptLatitudeOfOrigin,
		srptLongitudeOfCenter,
		srptStandardParallel1,
	    srptStandardParallel2,
    };

	public enum SpatialReferenceUnit
	{
		sruMeter = 1,
		sruFoot,
		sruNauticalMile,
		sruKilometer
	};

	public enum GeometryCommands
	{
		gcUnion = 1,
		gcConvexHull,		// 2
		gcCut,				// 3
		gcIntersect,		// 4
		gcBoundary,			// 5
		gcBuffer,			// 6

		gcDifference,		// 7

		gcGetNearestPoint,	// 8
		gcGetDistance,		// 9

		gcContains,			// 10
		gcCrosses,			// 11
		gcDisjoint,			// 12
		gcEquals,			// 13
		gcGeoTransformations
	};
}

