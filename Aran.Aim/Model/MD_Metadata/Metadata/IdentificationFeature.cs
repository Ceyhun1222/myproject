using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class IdentificationFeature : MDIdentification
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.IdentificationFeature; }
		}
		
		public Citiation Citation
		{
			get { return (Citiation ) GetValue ((int) PropertyIdentificationFeature.Citation); }
			set { SetValue ((int) PropertyIdentificationFeature.Citation, value); }
		}
		
		public ResponsibleParty PointOfContact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyIdentificationFeature.PointOfContact); }
			set { SetValue ((int) PropertyIdentificationFeature.PointOfContact, value); }
		}
		
		public LanguageCodeType? Language
		{
			get { return GetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationFeature.Language); }
			set { SetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationFeature.Language, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyIdentificationFeature
	{
		Citation = PropertyMDIdentification.NEXT_CLASS,
		PointOfContact,
		Language,
		NEXT_CLASS
	}
	
	public static class MetadataIdentificationFeature
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataIdentificationFeature ()
		{
			PropInfoList = MetadataMDIdentification.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyIdentificationFeature.Citation, (int) DataType.Citiation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationFeature.PointOfContact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationFeature.Language, (int) EnumType.LanguageCodeType, PropertyTypeCharacter.Nullable);
		}
	}
}
