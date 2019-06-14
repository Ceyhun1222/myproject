using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FireFightingService : AirportGroundService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.FireFightingService; }
		}
		
		public CodeFireFighting? Category
		{
			get { return GetNullableFieldValue <CodeFireFighting> ((int) PropertyFireFightingService.Category); }
			set { SetNullableFieldValue <CodeFireFighting> ((int) PropertyFireFightingService.Category, value); }
		}
		
		public CodeAviationStandards? Standard
		{
			get { return GetNullableFieldValue <CodeAviationStandards> ((int) PropertyFireFightingService.Standard); }
			set { SetNullableFieldValue <CodeAviationStandards> ((int) PropertyFireFightingService.Standard, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFireFightingService
	{
		Category = PropertyAirportGroundService.NEXT_CLASS,
		Standard,
		NEXT_CLASS
	}
	
	public static class MetadataFireFightingService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFireFightingService ()
		{
			PropInfoList = MetadataAirportGroundService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFireFightingService.Category, (int) EnumType.CodeFireFighting, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFireFightingService.Standard, (int) EnumType.CodeAviationStandards, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
