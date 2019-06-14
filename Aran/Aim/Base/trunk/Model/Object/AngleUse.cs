using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AngleUse : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AngleUse; }
		}
		
		public bool? AlongCourseGuidance
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAngleUse.AlongCourseGuidance); }
			set { SetNullableFieldValue <bool> ((int) PropertyAngleUse.AlongCourseGuidance, value); }
		}
		
		public FeatureRef TheAngleIndication
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAngleUse.TheAngleIndication); }
			set { SetValue ((int) PropertyAngleUse.TheAngleIndication, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAngleUse
	{
		AlongCourseGuidance = PropertyAObject.NEXT_CLASS,
		TheAngleIndication,
		NEXT_CLASS
	}
	
	public static class MetadataAngleUse
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAngleUse ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAngleUse.AlongCourseGuidance, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleUse.TheAngleIndication, (int) FeatureType.AngleIndication, PropertyTypeCharacter.Nullable);
		}
	}
}
