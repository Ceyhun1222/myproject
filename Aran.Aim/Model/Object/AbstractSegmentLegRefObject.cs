using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractSegmentLegRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractSegmentLegRefObject; }
		}
		
		public AbstractSegmentLegRef Feature
		{
			get { return (AbstractSegmentLegRef ) GetValue ((int) PropertyAbstractSegmentLegRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractSegmentLegRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractSegmentLegRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractSegmentLegRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractSegmentLegRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractSegmentLegRefObject.Feature, (int) DataType.AbstractSegmentLegRef);
		}
	}
}
