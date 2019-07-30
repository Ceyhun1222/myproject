using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AirportHeliportContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirportHeliportContamination; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportContamination
	{
        NEXT_CLASS = PropertySurfaceContamination.NEXT_CLASS
    }
	
	public static class MetadataAirportHeliportContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
		}
	}
}
