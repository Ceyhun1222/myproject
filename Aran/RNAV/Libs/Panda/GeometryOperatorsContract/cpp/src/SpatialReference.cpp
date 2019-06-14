#include "../include/SpatialReference.h"

namespace Panda
{
	SpatialReferenceParam::SpatialReferenceParam() :
		srParamType(0),
		value(0)
	{
	}

	SpatialReferenceParam::SpatialReferenceParam(int SRParamType, double Value)
	{
		srParamType = SRParamType;
		value = Value;
	}

	SpatialReferenceParam::~SpatialReferenceParam()
	{
	}

	void SpatialReferenceParam::pack (Handle handle) const throw (Registry::Exception)
	{
		Registry::putInt32 (handle, srParamType);
		Registry::putDouble(handle, value);
	}

	void SpatialReferenceParam::unpack (Handle handle) throw (Registry::Exception)
	{
		Registry::getInt32 (handle, srParamType);
		Registry::getDouble(handle, value);
	}

	void SpatialReferenceParam::assign (const SpatialReferenceParam& src)
	{
		srParamType = src.srParamType;
		value = src.value;
	}

	SpatialReferenceParam* SpatialReferenceParam::clone () const
	{
		SpatialReferenceParam* val = new SpatialReferenceParam();
		val->assign (*this);
		return val;
	}

//--------------------------------------------------------------------
//--------------------------------------------------------------------
//--------------------------------------------------------------------

	SpatialReference::SpatialReference() :
		name(L""),
		spatialReferenceType(0),
		spatialReferenceUnit(0)
	{
	}

	SpatialReference::~SpatialReference()
	{
	}

	void SpatialReference::pack (Handle handle) const throw (Registry::Exception)
	{
		Registry::putStdString (handle, name);
		Registry::putInt32 (handle, spatialReferenceType);
		Registry::putInt32 (handle, spatialReferenceUnit);
		ellipsoid.pack (handle);
		paramList.pack (handle);
	}

	void SpatialReference::unpack (Handle handle) throw (Registry::Exception)
	{
		Registry::getStdString (handle, name);
		spatialReferenceType = Registry::getInt32 (handle);
		Registry::getInt32 (handle, spatialReferenceUnit);
		ellipsoid.unpack (handle);
		paramList.unpack (handle);
	}
}