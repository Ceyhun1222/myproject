using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class OrganisationAuthority : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.OrganisationAuthority; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyOrganisationAuthority.Name); }
			set { SetFieldValue <string> ((int) PropertyOrganisationAuthority.Name, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyOrganisationAuthority.Designator); }
			set { SetFieldValue <string> ((int) PropertyOrganisationAuthority.Designator, value); }
		}
		
		public CodeOrganisation? Type
		{
			get { return GetNullableFieldValue <CodeOrganisation> ((int) PropertyOrganisationAuthority.Type); }
			set { SetNullableFieldValue <CodeOrganisation> ((int) PropertyOrganisationAuthority.Type, value); }
		}
		
		public CodeMilitaryOperations? Military
		{
			get { return GetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyOrganisationAuthority.Military); }
			set { SetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyOrganisationAuthority.Military, value); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyOrganisationAuthority.Contact); }
		}
		
		public List <OrganisationAuthorityAssociation> RelatedOrganisationAuthority
		{
			get { return GetObjectList <OrganisationAuthorityAssociation> ((int) PropertyOrganisationAuthority.RelatedOrganisationAuthority); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyOrganisationAuthority
	{
		Name = PropertyFeature.NEXT_CLASS,
		Designator,
		Type,
		Military,
		Contact,
		RelatedOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataOrganisationAuthority
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataOrganisationAuthority ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyOrganisationAuthority.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthority.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthority.Type, (int) EnumType.CodeOrganisation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthority.Military, (int) EnumType.CodeMilitaryOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthority.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthority.RelatedOrganisationAuthority, (int) ObjectType.OrganisationAuthorityAssociation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
