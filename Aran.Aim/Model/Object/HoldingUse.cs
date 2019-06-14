using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class HoldingUse : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.HoldingUse; }
		}
		
		public CodeHoldingUse? HoldingUseP
		{
			get { return GetNullableFieldValue <CodeHoldingUse> ((int) PropertyHoldingUse.HoldingUseP); }
			set { SetNullableFieldValue <CodeHoldingUse> ((int) PropertyHoldingUse.HoldingUseP, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyHoldingUse.Instruction); }
			set { SetFieldValue <string> ((int) PropertyHoldingUse.Instruction, value); }
		}
		
		public ValDistanceVertical InstructedAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyHoldingUse.InstructedAltitude); }
			set { SetValue ((int) PropertyHoldingUse.InstructedAltitude, value); }
		}
		
		public CodeVerticalReference? InstructionAltitudeReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingUse.InstructionAltitudeReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingUse.InstructionAltitudeReference, value); }
		}
		
		public FeatureRef TheHoldingPattern
		{
			get { return (FeatureRef ) GetValue ((int) PropertyHoldingUse.TheHoldingPattern); }
			set { SetValue ((int) PropertyHoldingUse.TheHoldingPattern, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingUse
	{
		HoldingUseP = PropertyAObject.NEXT_CLASS,
		Instruction,
		InstructedAltitude,
		InstructionAltitudeReference,
		TheHoldingPattern,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingUse
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingUse ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingUse.HoldingUseP, (int) EnumType.CodeHoldingUse, PropertyTypeCharacter.Nullable, "holdingUse");
			PropInfoList.Add (PropertyHoldingUse.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingUse.InstructedAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingUse.InstructionAltitudeReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingUse.TheHoldingPattern, (int) FeatureType.HoldingPattern, PropertyTypeCharacter.Nullable);
		}
	}
}
