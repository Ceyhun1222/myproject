using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AircraftStandContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AircraftStandContamination; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAircraftStandContamination
	{
        NEXT_CLASS = PropertySurfaceContamination.NEXT_CLASS
	}
	
	public static class MetadataAircraftStandContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAircraftStandContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
		}
	}
}
