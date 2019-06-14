#ifndef ELLIPSOID_H
#define ELLIPSOID_H

#include <string>
#include <Panda/Registry/cpp/include/Contract.h>

namespace Panda
{	
	class Ellipsoid
	{
		public:
			Ellipsoid();
			~Ellipsoid();
		
			void pack (Handle handle) const throw (Registry::Exception);
			void unpack (Handle handle) throw (Registry::Exception);
		
		public:
			int srGeoType;
			double semiMajorAxis;
			double flattening;
			bool isValid;
	};
}	
#endif //ELLIPSOID_H