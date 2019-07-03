using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class IdentificationMessage : MDIdentification
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.IdentificationMessage; }
		}
		
		public ResponsibleParty PointOfContact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyIdentificationMessage.PointOfContact); }
			set { SetValue ((int) PropertyIdentificationMessage.PointOfContact, value); }
		}
		
		public LanguageCodeType? Language
		{
			get { return GetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationMessage.Language); }
			set { SetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationMessage.Language, value); }
		}
		
		public List <MDConstraints> MessageConstraintInfo
		{
			get { return GetObjectList <MDConstraints> ((int) PropertyIdentificationMessage.MessageConstraintInfo); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyIdentificationMessage
	{
		PointOfContact = PropertyMDIdentification.NEXT_CLASS,
		Language,
		MessageConstraintInfo,
		NEXT_CLASS
	}
	
	public static class MetadataIdentificationMessage
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataIdentificationMessage ()
		{
			PropInfoList = MetadataMDIdentification.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyIdentificationMessage.PointOfContact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationMessage.Language, (int) EnumType.LanguageCodeType, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationMessage.MessageConstraintInfo, (int) ObjectType.MDConstraints, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
