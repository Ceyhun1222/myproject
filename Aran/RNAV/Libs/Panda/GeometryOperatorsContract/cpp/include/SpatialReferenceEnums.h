#ifndef SPATIAL_REFERENCE_ENUMS
#define SPATIAL_REFERENCE_ENUMS

namespace Panda
{
	class SpatialReferenceGeoType
	{
		public:
			enum
			{
				WGS1984 = 1,
				Krasovsky1940,
				NAD1983
			};
	};
	class SpatialReferenceType
	{
		public:
			enum
			{
				Geographic = 1,
				Mercator,
				Transverse_Mercator,
				Gauss_Krueger
			};
	};	

	class SpatialReferenceParamType
	{
		public:
			enum
			{
				FalseEasting = 1,
				FalseNorthing,
				ScaleFactor,
				Azimuth,
				CentralMeridian,
				LatitudeOfOrigin,
				LongitudeOfCenter
			};
	};

	class SpatialReferenceUnit
	{
		public:
			enum
			{
				Meter = 1,
				Foot,
				NauticalMile,
				Kilometer 
			};
	};
}

#endif //SPATIAL_REFERENCE_ENUMS
