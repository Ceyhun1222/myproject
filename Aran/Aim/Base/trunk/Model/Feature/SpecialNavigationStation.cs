using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SpecialNavigationStation : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SpecialNavigationStation; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertySpecialNavigationStation.Name); }
			set { SetFieldValue <string> ((int) PropertySpecialNavigationStation.Name, value); }
		}
		
		public CodeSpecialNavigationStation? Type
		{
			get { return GetNullableFieldValue <CodeSpecialNavigationStation> ((int) PropertySpecialNavigationStation.Type); }
			set { SetNullableFieldValue <CodeSpecialNavigationStation> ((int) PropertySpecialNavigationStation.Type, value); }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertySpecialNavigationStation.Frequency); }
			set { SetValue ((int) PropertySpecialNavigationStation.Frequency, value); }
		}
		
		public CodeRadioEmission? Emission
		{
			get { return GetNullableFieldValue <CodeRadioEmission> ((int) PropertySpecialNavigationStation.Emission); }
			set { SetNullableFieldValue <CodeRadioEmission> ((int) PropertySpecialNavigationStation.Emission, value); }
		}
		
		public FeatureRef SystemChain
		{
			get { return (FeatureRef ) GetValue ((int) PropertySpecialNavigationStation.SystemChain); }
			set { SetValue ((int) PropertySpecialNavigationStation.SystemChain, value); }
		}
		
		public AuthorityForSpecialNavigationStation ResponsibleOrganisation
		{
			get { return GetObject <AuthorityForSpecialNavigationStation> ((int) PropertySpecialNavigationStation.ResponsibleOrganisation); }
			set { SetValue ((int) PropertySpecialNavigationStation.ResponsibleOrganisation, value); }
		}
		
		public ElevatedPoint Position
		{
			get { return GetObject <ElevatedPoint> ((int) PropertySpecialNavigationStation.Position); }
			set { SetValue ((int) PropertySpecialNavigationStation.Position, value); }
		}
		
		public List <SpecialNavigationStationStatus> Availability
		{
			get { return GetObjectList <SpecialNavigationStationStatus> ((int) PropertySpecialNavigationStation.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySpecialNavigationStation
	{
		Name = PropertyFeature.NEXT_CLASS,
		Type,
		Frequency,
		Emission,
		SystemChain,
		ResponsibleOrganisation,
		Position,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataSpecialNavigationStation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSpecialNavigationStation ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySpecialNavigationStation.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.Type, (int) EnumType.CodeSpecialNavigationStation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.Emission, (int) EnumType.CodeRadioEmission, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.SystemChain, (int) FeatureType.SpecialNavigationSystem, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.ResponsibleOrganisation, (int) ObjectType.AuthorityForSpecialNavigationStation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.Position, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationStation.Availability, (int) ObjectType.SpecialNavigationStationStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
