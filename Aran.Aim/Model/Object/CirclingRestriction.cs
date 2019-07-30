using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class CirclingRestriction : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CirclingRestriction; }
		}
		
		public CircleSector SectorDescription
		{
			get { return GetObject <CircleSector> ((int) PropertyCirclingRestriction.SectorDescription); }
			set { SetValue ((int) PropertyCirclingRestriction.SectorDescription, value); }
		}
		
		public Surface RestrictionArea
		{
			get { return GetObject <Surface> ((int) PropertyCirclingRestriction.RestrictionArea); }
			set { SetValue ((int) PropertyCirclingRestriction.RestrictionArea, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCirclingRestriction
	{
		SectorDescription = PropertyPropertiesWithSchedule.NEXT_CLASS,
		RestrictionArea,
		NEXT_CLASS
	}
	
	public static class MetadataCirclingRestriction
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCirclingRestriction ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCirclingRestriction.SectorDescription, (int) ObjectType.CircleSector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCirclingRestriction.RestrictionArea, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
		}
	}
}
