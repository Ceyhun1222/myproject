#include "../include/Ellipsoid.h"

namespace Panda
{
	Ellipsoid::Ellipsoid():
		srGeoType(0),
		semiMajorAxis(0),
		flattening(0),
		isValid(false)
	{
	}

	Ellipsoid::~Ellipsoid()
	{
	}

	void Ellipsoid::pack (Handle handle) const throw (Registry::Exception)
	{
		Registry::putInt32 (handle, srGeoType);
		Registry::putDouble (handle, semiMajorAxis);
		Registry::putDouble (handle, flattening);
		Registry::putBool (handle, isValid);
	}

	void Ellipsoid::unpack (Handle handle) throw (Registry::Exception)
	{
		Registry::getInt32 (handle, srGeoType);
		Registry::getDouble (handle, semiMajorAxis);
		Registry::getDouble (handle, flattening);
		Registry::getBool (handle, isValid);
	}
}