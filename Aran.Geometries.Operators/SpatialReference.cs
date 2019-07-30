using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran;

namespace Aran.Geometries.Operators
{
	public class SpatialReference
	{
		public SpatialReference ( )
		{
			_ellipsoid = new Ellipsoid ( );
			_paramList = new AranObjectList<SpatialReferenceParam> ( );
			spatialReferenceType = SpatialReferenceType.srtGeographic;
			spatialReferenceUnit = SpatialReferenceUnit.sruMeter;
		}

		//Registry_Contract.PutString(handle, name);
		//Registry_Contract.PutInt32(handle, (int)spatialReferenceType);
		//Registry_Contract.PutInt32(handle, (int)spatialReferenceUnit);
		//Ellipsoid.Pack(handle);
		//ParamList.Pack(handle);

		public Ellipsoid Ellipsoid
		{
			get
			{
				return _ellipsoid;
			}
		}

		public AranObjectList<SpatialReferenceParam> ParamList
		{
			get
			{
				return _paramList;
			}
		}

		public double this [ SpatialReferenceParamType param ]
		{

			get
			{
				SpatialReferenceParam PandaItem;
				for ( int i = 0; i < ParamList.Count; i++ )
				{
					PandaItem = ParamList [ i ];
					if ( PandaItem.srParamType == param )
						return PandaItem.value;
				}
				return 0;
			}

			set
			{
				SpatialReferenceParam PandaItem;
				for ( int i = 0; i < ParamList.Count; i++ )
				{
					PandaItem = ParamList [ i ];
					if ( PandaItem.srParamType == param )
						PandaItem.value = value;
				}
			}
		}

		public string name;
		public SpatialReferenceType spatialReferenceType;
		public SpatialReferenceUnit spatialReferenceUnit;

		private Ellipsoid _ellipsoid;
		private AranObjectList<SpatialReferenceParam> _paramList;
	}
}
