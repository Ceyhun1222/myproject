using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public abstract class TrafficSeparationService : Service
	{
		public virtual TrafficSeparationServiceType TrafficSeparationServiceType 
		{
			get { return (TrafficSeparationServiceType) FeatureType; }
		}
		
		public bool? RadarAssisted
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTrafficSeparationService.RadarAssisted); }
			set { SetNullableFieldValue <bool> ((int) PropertyTrafficSeparationService.RadarAssisted, value); }
		}
		
		public bool? DataLinkEnabled
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTrafficSeparationService.DataLinkEnabled); }
			set { SetNullableFieldValue <bool> ((int) PropertyTrafficSeparationService.DataLinkEnabled, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTrafficSeparationService
	{
		RadarAssisted = PropertyService.NEXT_CLASS,
		DataLinkEnabled,
		NEXT_CLASS
	}
	
	public static class MetadataTrafficSeparationService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTrafficSeparationService ()
		{
			PropInfoList = MetadataService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTrafficSeparationService.RadarAssisted, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTrafficSeparationService.DataLinkEnabled, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
