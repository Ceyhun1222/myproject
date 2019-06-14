using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class MdSecurityConstraints : MdConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdSecurityConstraints; }
		}
		
		public MdClassificationCode? AccessConstraints
		{
			get { return GetNullableFieldValue <MdClassificationCode> ((int) PropertyMdSecurityConstraints.AccessConstraints); }
			set { SetNullableFieldValue <MdClassificationCode> ((int) PropertyMdSecurityConstraints.AccessConstraints, value); }
		}
		
		public string UserNote
		{
			get { return GetFieldValue <string> ((int) PropertyMdSecurityConstraints.UserNote); }
			set { SetFieldValue <string> ((int) PropertyMdSecurityConstraints.UserNote, value); }
		}
		
		public string ClassificationSystemField
		{
			get { return GetFieldValue <string> ((int) PropertyMdSecurityConstraints.ClassificationSystemField); }
			set { SetFieldValue <string> ((int) PropertyMdSecurityConstraints.ClassificationSystemField, value); }
		}
		
		public string HandlingDescriptionField
		{
			get { return GetFieldValue <string> ((int) PropertyMdSecurityConstraints.HandlingDescriptionField); }
			set { SetFieldValue <string> ((int) PropertyMdSecurityConstraints.HandlingDescriptionField, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdSecurityConstraints
	{
		AccessConstraints = PropertyMdConstraints.NEXT_CLASS,
		UserNote,
		ClassificationSystemField,
		HandlingDescriptionField,
		NEXT_CLASS
	}
	
	public static class MetadataMdSecurityConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdSecurityConstraints ()
		{
			PropInfoList = MetadataMdConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdSecurityConstraints.AccessConstraints, (int) EnumType.MdClassificationCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdSecurityConstraints.UserNote, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdSecurityConstraints.ClassificationSystemField, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdSecurityConstraints.HandlingDescriptionField, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
