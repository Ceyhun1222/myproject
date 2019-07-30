using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class LegalConstraints : MDLegalConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LegalConstraints; }
		}
		
		public RestrictionCode? accessConstraints
		{
			get { return GetNullableFieldValue <RestrictionCode> ((int) PropertyLegalConstraints.accessConstraints); }
			set { SetNullableFieldValue <RestrictionCode> ((int) PropertyLegalConstraints.accessConstraints, value); }
		}
		
		public RestrictionCode? useConstraints
		{
			get { return GetNullableFieldValue <RestrictionCode> ((int) PropertyLegalConstraints.useConstraints); }
			set { SetNullableFieldValue <RestrictionCode> ((int) PropertyLegalConstraints.useConstraints, value); }
		}
		
		public string userNote
		{
			get { return GetFieldValue <string> ((int) PropertyLegalConstraints.userNote); }
			set { SetFieldValue <string> ((int) PropertyLegalConstraints.userNote, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLegalConstraints
	{
		accessConstraints = PropertyMDLegalConstraints.NEXT_CLASS,
		useConstraints,
		userNote,
		NEXT_CLASS
	}
	
	public static class MetadataLegalConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLegalConstraints ()
		{
			PropInfoList = MetadataMDLegalConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLegalConstraints.accessConstraints, (int) EnumType.RestrictionCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLegalConstraints.useConstraints, (int) EnumType.RestrictionCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLegalConstraints.userNote, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
