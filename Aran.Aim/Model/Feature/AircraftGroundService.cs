using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AircraftGroundService : AirportGroundService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AircraftGroundService; }
		}
		
		public CodeAircraftGroundService? Type
		{
			get { return GetNullableFieldValue <CodeAircraftGroundService> ((int) PropertyAircraftGroundService.Type); }
			set { SetNullableFieldValue <CodeAircraftGroundService> ((int) PropertyAircraftGroundService.Type, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAircraftGroundService
	{
		Type = PropertyAirportGroundService.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAircraftGroundService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAircraftGroundService ()
		{
			PropInfoList = MetadataAirportGroundService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAircraftGroundService.Type, (int) EnumType.CodeAircraftGroundService, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
