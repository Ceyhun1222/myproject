using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class TouchDownLiftOffContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.TouchDownLiftOffContamination; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTouchDownLiftOffContamination
	{
        NEXT_CLASS = PropertySurfaceContamination.NEXT_CLASS
    }
	
	public static class MetadataTouchDownLiftOffContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTouchDownLiftOffContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
		}
	}
}
