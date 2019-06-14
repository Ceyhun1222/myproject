#pragma once

#ifndef SPATIAL_REFERENCE_H
#define SPATIAL_REFERENCE_H

#include <string>
#include "Ellipsoid.h"
#include <Panda/Common/cpp/include/List.h>

namespace Panda
{
	
	class SpatialReferenceParam
	{
		public:
			SpatialReferenceParam();
			SpatialReferenceParam(int SRParamType, double Value);
			~SpatialReferenceParam();

			void pack (Handle handle) const throw (Registry::Exception);
			void unpack (Handle handle) throw (Registry::Exception);
			
			void assign (const SpatialReferenceParam& src);
			SpatialReferenceParam* clone () const;

		public:
			int srParamType;
			double value;
	};
	
	
	class SpatialReference
	{
		public:
			SpatialReference();
			~SpatialReference();
			
			void pack (Handle handle) const throw (Registry::Exception);
			void unpack (Handle handle) throw (Registry::Exception);
			
		public:
			std::wstring name;
			int spatialReferenceType;
			int spatialReferenceUnit;
			Ellipsoid ellipsoid;
			List<SpatialReferenceParam> paramList;
	};
}
	
#endif //SPATIAL_REFERENCE_H
	