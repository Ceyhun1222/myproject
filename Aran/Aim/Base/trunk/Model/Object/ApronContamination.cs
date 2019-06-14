using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ApronContamination : SurfaceContamination
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApronContamination; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronContamination
	{
        NEXT_CLASS = PropertySurfaceContamination.NEXT_CLASS
	}
	
	public static class MetadataApronContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronContamination ()
		{
			PropInfoList = MetadataSurfaceContamination.PropInfoList.Clone ();
			
		}
	}
}
