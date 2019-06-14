using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public abstract class Marking : Feature
	{
		public virtual MarkingType MarkingType 
		{
			get { return (MarkingType) FeatureType; }
		}
		
		public bool? MarkingICAOStandard
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyMarking.MarkingICAOStandard); }
			set { SetNullableFieldValue <bool> ((int) PropertyMarking.MarkingICAOStandard, value); }
		}
		
		public CodeMarkingCondition? Condition
		{
			get { return GetNullableFieldValue <CodeMarkingCondition> ((int) PropertyMarking.Condition); }
			set { SetNullableFieldValue <CodeMarkingCondition> ((int) PropertyMarking.Condition, value); }
		}
		
		public List <MarkingElement> Element
		{
			get { return GetObjectList <MarkingElement> ((int) PropertyMarking.Element); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMarking
	{
		MarkingICAOStandard = PropertyFeature.NEXT_CLASS,
		Condition,
		Element,
		NEXT_CLASS
	}
	
	public static class MetadataMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMarking ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMarking.MarkingICAOStandard, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarking.Condition, (int) EnumType.CodeMarkingCondition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarking.Element, (int) ObjectType.MarkingElement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
