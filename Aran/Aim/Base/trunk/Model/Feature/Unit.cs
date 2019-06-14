using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Unit : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Unit; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyUnit.Name); }
			set { SetFieldValue <string> ((int) PropertyUnit.Name, value); }
		}
		
		public CodeUnit? Type
		{
			get { return GetNullableFieldValue <CodeUnit> ((int) PropertyUnit.Type); }
			set { SetNullableFieldValue <CodeUnit> ((int) PropertyUnit.Type, value); }
		}
		
		public bool? CompliantICAO
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyUnit.CompliantICAO); }
			set { SetNullableFieldValue <bool> ((int) PropertyUnit.CompliantICAO, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyUnit.Designator); }
			set { SetFieldValue <string> ((int) PropertyUnit.Designator, value); }
		}
		
		public CodeMilitaryOperations? Military
		{
			get { return GetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyUnit.Military); }
			set { SetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyUnit.Military, value); }
		}
		
		public ElevatedPoint Position
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyUnit.Position); }
			set { SetValue ((int) PropertyUnit.Position, value); }
		}
		
		public FeatureRef AirportLocation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyUnit.AirportLocation); }
			set { SetValue ((int) PropertyUnit.AirportLocation, value); }
		}
		
		public FeatureRef OwnerOrganisation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyUnit.OwnerOrganisation); }
			set { SetValue ((int) PropertyUnit.OwnerOrganisation, value); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyUnit.Contact); }
		}
		
		public List <UnitDependency> RelatedUnit
		{
			get { return GetObjectList <UnitDependency> ((int) PropertyUnit.RelatedUnit); }
		}
		
		public List <UnitAvailability> Availability
		{
			get { return GetObjectList <UnitAvailability> ((int) PropertyUnit.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyUnit
	{
		Name = PropertyFeature.NEXT_CLASS,
		Type,
		CompliantICAO,
		Designator,
		Military,
		Position,
		AirportLocation,
		OwnerOrganisation,
		Contact,
		RelatedUnit,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataUnit
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataUnit ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyUnit.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Type, (int) EnumType.CodeUnit, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.CompliantICAO, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Military, (int) EnumType.CodeMilitaryOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Position, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.AirportLocation, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.OwnerOrganisation, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.RelatedUnit, (int) ObjectType.UnitDependency, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyUnit.Availability, (int) ObjectType.UnitAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
