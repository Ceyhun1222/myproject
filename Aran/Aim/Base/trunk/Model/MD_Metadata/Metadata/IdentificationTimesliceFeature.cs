using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class IdentificationTimesliceFeature : MDIdentification
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.IdentificationTimesliceFeature; }
		}
		
		public Citiation Citiation
		{
			get { return (Citiation ) GetValue ((int) PropertyIdentificationTimesliceFeature.Citiation); }
			set { SetValue ((int) PropertyIdentificationTimesliceFeature.Citiation, value); }
		}
		
		public ResponsibleParty PointOfContact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyIdentificationTimesliceFeature.PointOfContact); }
			set { SetValue ((int) PropertyIdentificationTimesliceFeature.PointOfContact, value); }
		}
		
		public MDProgressCode? DataStatus
		{
			get { return GetNullableFieldValue <MDProgressCode> ((int) PropertyIdentificationTimesliceFeature.DataStatus); }
			set { SetNullableFieldValue <MDProgressCode> ((int) PropertyIdentificationTimesliceFeature.DataStatus, value); }
		}
		
		public LanguageCodeType? Language
		{
			get { return GetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationTimesliceFeature.Language); }
			set { SetNullableFieldValue <LanguageCodeType> ((int) PropertyIdentificationTimesliceFeature.Language, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyIdentificationTimesliceFeature
	{
		Citiation = PropertyMDIdentification.NEXT_CLASS,
		PointOfContact,
		DataStatus,
		Language,
		NEXT_CLASS
	}
	
	public static class MetadataIdentificationTimesliceFeature
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataIdentificationTimesliceFeature ()
		{
			PropInfoList = MetadataMDIdentification.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyIdentificationTimesliceFeature.Citiation, (int) DataType.Citiation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationTimesliceFeature.PointOfContact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationTimesliceFeature.DataStatus, (int) EnumType.MDProgressCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyIdentificationTimesliceFeature.Language, (int) EnumType.LanguageCodeType, PropertyTypeCharacter.Nullable);
		}
	}
}
