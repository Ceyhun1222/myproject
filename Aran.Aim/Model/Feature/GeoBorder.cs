using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class GeoBorder : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.GeoBorder; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyGeoBorder.Name); }
			set { SetFieldValue <string> ((int) PropertyGeoBorder.Name, value); }
		}
		
		public CodeGeoBorder? Type
		{
			get { return GetNullableFieldValue <CodeGeoBorder> ((int) PropertyGeoBorder.Type); }
			set { SetNullableFieldValue <CodeGeoBorder> ((int) PropertyGeoBorder.Type, value); }
		}
		
		public Curve Border
		{
			get { return GetObject <Curve> ((int) PropertyGeoBorder.Border); }
			set { SetValue ((int) PropertyGeoBorder.Border, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGeoBorder
	{
		Name = PropertyFeature.NEXT_CLASS,
		Type,
		Border,
		NEXT_CLASS
	}
	
	public static class MetadataGeoBorder
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGeoBorder ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGeoBorder.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGeoBorder.Type, (int) EnumType.CodeGeoBorder, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGeoBorder.Border, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
