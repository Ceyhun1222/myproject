using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AirportClearanceService : AirportGroundService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportClearanceService; }
		}
		
		public string SnowPlan
		{
			get { return GetFieldValue <string> ((int) PropertyAirportClearanceService.SnowPlan); }
			set { SetFieldValue <string> ((int) PropertyAirportClearanceService.SnowPlan, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportClearanceService
	{
		SnowPlan = PropertyAirportGroundService.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAirportClearanceService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportClearanceService ()
		{
			PropInfoList = MetadataAirportGroundService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportClearanceService.SnowPlan, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
