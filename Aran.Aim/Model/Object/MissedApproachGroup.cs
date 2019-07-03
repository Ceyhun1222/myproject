using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class MissedApproachGroup : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MissedApproachGroup; }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyMissedApproachGroup.Instruction); }
			set { SetFieldValue <string> ((int) PropertyMissedApproachGroup.Instruction, value); }
		}
		
		public string AlternateClimbInstruction
		{
			get { return GetFieldValue <string> ((int) PropertyMissedApproachGroup.AlternateClimbInstruction); }
			set { SetFieldValue <string> ((int) PropertyMissedApproachGroup.AlternateClimbInstruction, value); }
		}
		
		public ValDistanceVertical AlternateClimbAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMissedApproachGroup.AlternateClimbAltitude); }
			set { SetValue ((int) PropertyMissedApproachGroup.AlternateClimbAltitude, value); }
		}
		
		public List <FeatureRefObject> Altimeter
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyMissedApproachGroup.Altimeter); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMissedApproachGroup
	{
		Instruction = PropertyAObject.NEXT_CLASS,
		AlternateClimbInstruction,
		AlternateClimbAltitude,
		Altimeter,
		NEXT_CLASS
	}
	
	public static class MetadataMissedApproachGroup
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMissedApproachGroup ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMissedApproachGroup.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachGroup.AlternateClimbInstruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachGroup.AlternateClimbAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachGroup.Altimeter, (int) FeatureType.AltimeterSource, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
