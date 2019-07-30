using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ApronAreaUsage : UsageCondition
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApronAreaUsage; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronAreaUsage
	{
		NEXT_CLASS = PropertyUsageCondition.NEXT_CLASS
    }
	
	public static class MetadataApronAreaUsage
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronAreaUsage ()
		{
			PropInfoList = MetadataUsageCondition.PropInfoList.Clone ();
			
		}
	}
}
