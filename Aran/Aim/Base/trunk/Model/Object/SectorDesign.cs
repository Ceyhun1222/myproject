using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SectorDesign : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SectorDesign; }
		}
		
		public CodeDirectionTurn? TurnDirection
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertySectorDesign.TurnDirection); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertySectorDesign.TurnDirection, value); }
		}
		
		public double? DesignGradient
		{
			get { return GetNullableFieldValue <double> ((int) PropertySectorDesign.DesignGradient); }
			set { SetNullableFieldValue <double> ((int) PropertySectorDesign.DesignGradient, value); }
		}
		
		public ValDistanceVertical TerminationAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertySectorDesign.TerminationAltitude); }
			set { SetValue ((int) PropertySectorDesign.TerminationAltitude, value); }
		}
		
		public bool? TurnPermitted
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySectorDesign.TurnPermitted); }
			set { SetNullableFieldValue <bool> ((int) PropertySectorDesign.TurnPermitted, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySectorDesign
	{
		TurnDirection = PropertyAObject.NEXT_CLASS,
		DesignGradient,
		TerminationAltitude,
		TurnPermitted,
		NEXT_CLASS
	}
	
	public static class MetadataSectorDesign
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSectorDesign ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySectorDesign.TurnDirection, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySectorDesign.DesignGradient, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySectorDesign.TerminationAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySectorDesign.TurnPermitted, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
