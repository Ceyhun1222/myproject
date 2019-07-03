using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Geometries.Operators
{
	public class Ellipsoid
	{
		public SpatialReferenceGeoType srGeoType;
		public double semiMajorAxis;
		public double flattening;
		public bool isValid;

		public Ellipsoid ( )
		{
			srGeoType = SpatialReferenceGeoType.srgtWGS1984;
			semiMajorAxis = 0;
			flattening = 0;
			isValid = false;
		}

		//Registry_Contract.PutInt32(handle, (int)srGeoType);
		//Registry_Contract.PutDouble(handle, semiMajorAxis);
		//Registry_Contract.PutDouble(handle, flattening);
		//Registry_Contract.PutBool(handle, isValid);
	}
}
