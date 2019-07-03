using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran;

namespace Aran.Geometries.Operators
{
	public class SpatialReferenceParam : AranObject, IAranCloneable
	{
		public SpatialReferenceParam ( )
			: base ( )
		{
			srParamType = SpatialReferenceParamType.srptFalseEasting;
			value = 0;
		}

		public SpatialReferenceParam ( SpatialReferenceParamType aSRParamType, double aValue )
			: base ( )
		{
			srParamType = aSRParamType;
			value = aValue;
		}


		//Registry_Contract.PutInt32(handle, (int)srParamType);		
		//Registry_Contract.PutDouble(handle, value);

		#region IAranCloneable Members

		AranObject IAranCloneable.Clone ( )
		{
			SpatialReferenceParam result = new SpatialReferenceParam ( );
			result.Assign ( this );
			return result;
		}

		public void Assign ( AranObject source )
		{
			srParamType = ( ( SpatialReferenceParam ) source ).srParamType;
			value = ( ( SpatialReferenceParam ) source ).value;
		}

		#endregion

		public SpatialReferenceParamType srParamType;
		public double value;
	}
}
