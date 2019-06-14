using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Meteorology : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Meteorology; }
		}
		
		public CodeMeteoConditions? FlightConditions
		{
			get { return GetNullableFieldValue <CodeMeteoConditions> ((int) PropertyMeteorology.FlightConditions); }
			set { SetNullableFieldValue <CodeMeteoConditions> ((int) PropertyMeteorology.FlightConditions, value); }
		}
		
		public ValDistance Visibility
		{
			get { return (ValDistance ) GetValue ((int) PropertyMeteorology.Visibility); }
			set { SetValue ((int) PropertyMeteorology.Visibility, value); }
		}
		
		public CodeValueInterpretation? VisibilityInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyMeteorology.VisibilityInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyMeteorology.VisibilityInterpretation, value); }
		}
		
		public ValDistance RunwayVisualRange
		{
			get { return (ValDistance ) GetValue ((int) PropertyMeteorology.RunwayVisualRange); }
			set { SetValue ((int) PropertyMeteorology.RunwayVisualRange, value); }
		}
		
		public CodeValueInterpretation? RunwayVisualRangeInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyMeteorology.RunwayVisualRangeInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyMeteorology.RunwayVisualRangeInterpretation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMeteorology
	{
		FlightConditions = PropertyAObject.NEXT_CLASS,
		Visibility,
		VisibilityInterpretation,
		RunwayVisualRange,
		RunwayVisualRangeInterpretation,
		NEXT_CLASS
	}
	
	public static class MetadataMeteorology
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMeteorology ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMeteorology.FlightConditions, (int) EnumType.CodeMeteoConditions, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMeteorology.Visibility, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMeteorology.VisibilityInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMeteorology.RunwayVisualRange, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMeteorology.RunwayVisualRangeInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
