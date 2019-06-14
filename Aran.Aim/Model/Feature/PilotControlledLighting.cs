using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class PilotControlledLighting : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.PilotControlledLighting; }
		}
		
		public CodePilotControlledLighting? Type
		{
			get { return GetNullableFieldValue <CodePilotControlledLighting> ((int) PropertyPilotControlledLighting.Type); }
			set { SetNullableFieldValue <CodePilotControlledLighting> ((int) PropertyPilotControlledLighting.Type, value); }
		}
		
		public ValDuration Duration
		{
			get { return (ValDuration ) GetValue ((int) PropertyPilotControlledLighting.Duration); }
			set { SetValue ((int) PropertyPilotControlledLighting.Duration, value); }
		}
		
		public uint? IntensitySteps
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyPilotControlledLighting.IntensitySteps); }
			set { SetNullableFieldValue <uint> ((int) PropertyPilotControlledLighting.IntensitySteps, value); }
		}
		
		public CodeIntensityStandBy? StandByIntensity
		{
			get { return GetNullableFieldValue <CodeIntensityStandBy> ((int) PropertyPilotControlledLighting.StandByIntensity); }
			set { SetNullableFieldValue <CodeIntensityStandBy> ((int) PropertyPilotControlledLighting.StandByIntensity, value); }
		}
		
		public ValFrequency RadioFrequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyPilotControlledLighting.RadioFrequency); }
			set { SetValue ((int) PropertyPilotControlledLighting.RadioFrequency, value); }
		}
		
		public string ActivationInstruction
		{
			get { return GetFieldValue <string> ((int) PropertyPilotControlledLighting.ActivationInstruction); }
			set { SetFieldValue <string> ((int) PropertyPilotControlledLighting.ActivationInstruction, value); }
		}
		
		public List <LightActivation> ControlledLightIntensity
		{
			get { return GetObjectList <LightActivation> ((int) PropertyPilotControlledLighting.ControlledLightIntensity); }
		}
		
		public List <AbstractGroundLightSystemRefObject> ActivatedGroundLighting
		{
			get { return GetObjectList <AbstractGroundLightSystemRefObject> ((int) PropertyPilotControlledLighting.ActivatedGroundLighting); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPilotControlledLighting
	{
		Type = PropertyFeature.NEXT_CLASS,
		Duration,
		IntensitySteps,
		StandByIntensity,
		RadioFrequency,
		ActivationInstruction,
		ControlledLightIntensity,
		ActivatedGroundLighting,
		NEXT_CLASS
	}
	
	public static class MetadataPilotControlledLighting
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPilotControlledLighting ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPilotControlledLighting.Type, (int) EnumType.CodePilotControlledLighting, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.Duration, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.IntensitySteps, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.StandByIntensity, (int) EnumType.CodeIntensityStandBy, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.RadioFrequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.ActivationInstruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.ControlledLightIntensity, (int) ObjectType.LightActivation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPilotControlledLighting.ActivatedGroundLighting, (int) ObjectType.AbstractGroundLightSystemRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
