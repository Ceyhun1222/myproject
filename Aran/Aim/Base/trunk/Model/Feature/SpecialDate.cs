using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SpecialDate : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SpecialDate; }
		}
		
		public CodeSpecialDate? Type
		{
			get { return GetNullableFieldValue <CodeSpecialDate> ((int) PropertySpecialDate.Type); }
			set { SetNullableFieldValue <CodeSpecialDate> ((int) PropertySpecialDate.Type, value); }
		}
		
		public string DateDay
		{
			get { return GetFieldValue <string> ((int) PropertySpecialDate.DateDay); }
			set { SetFieldValue <string> ((int) PropertySpecialDate.DateDay, value); }
		}
		
		public string DateYear
		{
			get { return GetFieldValue <string> ((int) PropertySpecialDate.DateYear); }
			set { SetFieldValue <string> ((int) PropertySpecialDate.DateYear, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertySpecialDate.Name); }
			set { SetFieldValue <string> ((int) PropertySpecialDate.Name, value); }
		}
		
		public FeatureRef Authority
		{
			get { return (FeatureRef ) GetValue ((int) PropertySpecialDate.Authority); }
			set { SetValue ((int) PropertySpecialDate.Authority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySpecialDate
	{
		Type = PropertyFeature.NEXT_CLASS,
		DateDay,
		DateYear,
		Name,
		Authority,
		NEXT_CLASS
	}
	
	public static class MetadataSpecialDate
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSpecialDate ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySpecialDate.Type, (int) EnumType.CodeSpecialDate, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialDate.DateDay, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialDate.DateYear, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialDate.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialDate.Authority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
