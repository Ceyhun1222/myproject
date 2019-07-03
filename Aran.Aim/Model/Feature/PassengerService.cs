using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class PassengerService : AirportGroundService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.PassengerService; }
		}
		
		public CodePassengerService? Type
		{
			get { return GetNullableFieldValue <CodePassengerService> ((int) PropertyPassengerService.Type); }
			set { SetNullableFieldValue <CodePassengerService> ((int) PropertyPassengerService.Type, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPassengerService
	{
		Type = PropertyAirportGroundService.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataPassengerService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPassengerService ()
		{
			PropInfoList = MetadataAirportGroundService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPassengerService.Type, (int) EnumType.CodePassengerService, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
