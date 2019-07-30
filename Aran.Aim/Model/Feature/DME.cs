using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class DME : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DME; }
		}
		
		public CodeDME? Type
		{
			get { return GetNullableFieldValue <CodeDME> ((int) PropertyDME.Type); }
			set { SetNullableFieldValue <CodeDME> ((int) PropertyDME.Type, value); }
		}
		
		public CodeDMEChannel? Channel
		{
			get { return GetNullableFieldValue <CodeDMEChannel> ((int) PropertyDME.Channel); }
			set { SetNullableFieldValue <CodeDMEChannel> ((int) PropertyDME.Channel, value); }
		}
		
		public ValFrequency GhostFrequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyDME.GhostFrequency); }
			set { SetValue ((int) PropertyDME.GhostFrequency, value); }
		}
		
		public ValDistance Displace
		{
			get { return (ValDistance ) GetValue ((int) PropertyDME.Displace); }
			set { SetValue ((int) PropertyDME.Displace, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDME
	{
		Type = PropertyNavaidEquipment.NEXT_CLASS,
		Channel,
		GhostFrequency,
		Displace,
		NEXT_CLASS
	}
	
	public static class MetadataDME
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDME ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDME.Type, (int) EnumType.CodeDME, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDME.Channel, (int) EnumType.CodeDMEChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDME.GhostFrequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDME.Displace, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
