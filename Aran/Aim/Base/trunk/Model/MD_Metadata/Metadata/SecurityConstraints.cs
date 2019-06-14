using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class SecurityConstraints : MDSecurityConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SecurityConstraints; }
		}
		
		public ClassificationCode? Classification
		{
			get { return GetNullableFieldValue <ClassificationCode> ((int) PropertySecurityConstraints.Classification); }
			set { SetNullableFieldValue <ClassificationCode> ((int) PropertySecurityConstraints.Classification, value); }
		}
		
		public string OtherClassification
		{
			get { return GetFieldValue <string> ((int) PropertySecurityConstraints.OtherClassification); }
			set { SetFieldValue <string> ((int) PropertySecurityConstraints.OtherClassification, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySecurityConstraints
	{
		Classification = PropertyMDSecurityConstraints.NEXT_CLASS,
		OtherClassification,
		NEXT_CLASS
	}
	
	public static class MetadataSecurityConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSecurityConstraints ()
		{
			PropInfoList = MetadataMDSecurityConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySecurityConstraints.Classification, (int) EnumType.ClassificationCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySecurityConstraints.OtherClassification, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
