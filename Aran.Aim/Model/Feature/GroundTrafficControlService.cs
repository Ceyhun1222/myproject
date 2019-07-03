using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class GroundTrafficControlService : TrafficSeparationService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.GroundTrafficControlService; }
		}
		
		public CodeServiceGroundControl? Type
		{
			get { return GetNullableFieldValue <CodeServiceGroundControl> ((int) PropertyGroundTrafficControlService.Type); }
			set { SetNullableFieldValue <CodeServiceGroundControl> ((int) PropertyGroundTrafficControlService.Type, value); }
		}
		
		public FeatureRef ClientAirport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyGroundTrafficControlService.ClientAirport); }
			set { SetValue ((int) PropertyGroundTrafficControlService.ClientAirport, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGroundTrafficControlService
	{
		Type = PropertyTrafficSeparationService.NEXT_CLASS,
		ClientAirport,
		NEXT_CLASS
	}
	
	public static class MetadataGroundTrafficControlService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGroundTrafficControlService ()
		{
			PropInfoList = MetadataTrafficSeparationService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGroundTrafficControlService.Type, (int) EnumType.CodeServiceGroundControl, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGroundTrafficControlService.ClientAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
