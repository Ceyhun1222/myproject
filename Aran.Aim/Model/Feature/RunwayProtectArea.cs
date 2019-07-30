using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayProtectArea : AirportHeliportProtectionArea
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayProtectArea; }
		}
		
		public CodeRunwayProtectionArea? Type
		{
			get { return GetNullableFieldValue <CodeRunwayProtectionArea> ((int) PropertyRunwayProtectArea.Type); }
			set { SetNullableFieldValue <CodeRunwayProtectionArea> ((int) PropertyRunwayProtectArea.Type, value); }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyRunwayProtectArea.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyRunwayProtectArea.Status, value); }
		}
		
		public FeatureRef ProtectedRunwayDirection
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayProtectArea.ProtectedRunwayDirection); }
			set { SetValue ((int) PropertyRunwayProtectArea.ProtectedRunwayDirection, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayProtectArea
	{
		Type = PropertyAirportHeliportProtectionArea.NEXT_CLASS,
		Status,
		ProtectedRunwayDirection,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayProtectArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayProtectArea ()
		{
			PropInfoList = MetadataAirportHeliportProtectionArea.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayProtectArea.Type, (int) EnumType.CodeRunwayProtectionArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayProtectArea.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayProtectArea.ProtectedRunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
