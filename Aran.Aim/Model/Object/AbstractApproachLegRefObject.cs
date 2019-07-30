using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractApproachLegRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractApproachLegRefObject; }
		}
		
		public AbstractApproachLegRef Feature
		{
			get { return (AbstractApproachLegRef ) GetValue ((int) PropertyAbstractApproachLegRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractApproachLegRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractApproachLegRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractApproachLegRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractApproachLegRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractApproachLegRefObject.Feature, (int) DataType.AbstractApproachLegRef);
		}
	}
}
