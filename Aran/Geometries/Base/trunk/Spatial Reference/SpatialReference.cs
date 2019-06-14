using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran;
using Aran.Package;

namespace Aran.Geometries.SpatialReferences
{
	public class SpatialReference : IPackable
	{
		public SpatialReference()
		{
			_ellipsoid = new Ellipsoid();
			_paramList = new AranObjectList<SpatialReferenceParam>();
			SpatialReferenceType = SpatialReferenceType.srtGeographic;
			SpatialReferenceUnit = SpatialReferenceUnit.sruMeter;
		}

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

		public double this[SpatialReferenceParamType param]
		{

			get
			{
				SpatialReferenceParam PandaItem;
				for (int i = 0; i < ParamList.Count; i++)
				{
					PandaItem = ParamList[i];
					if (PandaItem.SRParamType == param)
						return PandaItem.Value;
				}
				return 0;
			}

			set
			{
				SpatialReferenceParam PandaItem;
				for (int i = 0; i < ParamList.Count; i++)
				{
					PandaItem = ParamList[i];
					if (PandaItem.SRParamType == param)
						PandaItem.Value = value;
				}
			}
		}

		public string Name { get; set; }

		public SpatialReferenceType SpatialReferenceType { get; set; }

		public SpatialReferenceUnit SpatialReferenceUnit { get; set; }

		public void Pack(PackageWriter writer)
		{
			writer.PutString(Name);
			writer.PutEnum<SpatialReferenceType>(SpatialReferenceType);
			writer.PutEnum<SpatialReferenceUnit>(SpatialReferenceUnit);
			Ellipsoid.Pack(writer);

			writer.PutInt32(_paramList.Count);
			foreach (SpatialReferenceParam srParam in _paramList)
				srParam.Pack(writer);
		}

		public void Unpack(PackageReader reader)
		{
			Name = reader.GetString();
			SpatialReferenceType = reader.GetEnum<SpatialReferenceType>();
			SpatialReferenceUnit = reader.GetEnum<SpatialReferenceUnit>();
			Ellipsoid.Unpack(reader);

			int count = reader.GetInt32();
			for (int i = 0; i < count; i++)
			{
				SpatialReferenceParam srParam = new SpatialReferenceParam();
				srParam.Unpack(reader);
				_paramList.Add(srParam);
			}
		}

		private Ellipsoid _ellipsoid;
		private AranObjectList<SpatialReferenceParam> _paramList;
	}
}
