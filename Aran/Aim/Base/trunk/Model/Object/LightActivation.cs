using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class LightActivation : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LightActivation; }
		}
		
		public uint? Clicks
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyLightActivation.Clicks); }
			set { SetNullableFieldValue <uint> ((int) PropertyLightActivation.Clicks, value); }
		}
		
		public CodeLightIntensity? IntensityLevel
		{
			get { return GetNullableFieldValue <CodeLightIntensity> ((int) PropertyLightActivation.IntensityLevel); }
			set { SetNullableFieldValue <CodeLightIntensity> ((int) PropertyLightActivation.IntensityLevel, value); }
		}
		
		public CodeSystemActivation? Activation
		{
			get { return GetNullableFieldValue <CodeSystemActivation> ((int) PropertyLightActivation.Activation); }
			set { SetNullableFieldValue <CodeSystemActivation> ((int) PropertyLightActivation.Activation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLightActivation
	{
		Clicks = PropertyAObject.NEXT_CLASS,
		IntensityLevel,
		Activation,
		NEXT_CLASS
	}
	
	public static class MetadataLightActivation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLightActivation ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLightActivation.Clicks, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightActivation.IntensityLevel, (int) EnumType.CodeLightIntensity, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightActivation.Activation, (int) EnumType.CodeSystemActivation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
