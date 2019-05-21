using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran;
using Aran.Package;

namespace Aran.Geometries.SpatialReferences
{
	public class SpatialReferenceParam : AranObject, IAranCloneable, IPackable
	{
		public SpatialReferenceParam()
			: base()
		{
			SRParamType = SpatialReferenceParamType.srptFalseEasting;
			Value = 0;
		}

		public SpatialReferenceParam(SpatialReferenceParamType aSRParamType, double aValue)
			: base()
		{
			SRParamType = aSRParamType;
			Value = aValue;
		}

		public SpatialReferenceParamType SRParamType { get; set; }

		public double Value { get; set; }

		public AranObject Clone()
		{
			SpatialReferenceParam result = new SpatialReferenceParam();
			result.Assign(this);
			return result;
		}

		public void Assign(AranObject source)
		{
			SRParamType = ((SpatialReferenceParam)source).SRParamType;
			Value = ((SpatialReferenceParam)source).Value;
		}

		public void Pack(PackageWriter writer)
		{
			writer.PutEnum<SpatialReferenceParamType>(SRParamType);
			writer.PutDouble(Value);
		}

		public void Unpack(PackageReader reader)
		{
			SRParamType = reader.GetEnum<SpatialReferenceParamType>();
			Value = reader.GetDouble();
		}
	}
}

