using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApproachTimingTable : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApproachTimingTable; }
		}
		
		public CodeProcedureDistance? StartingMeasurementPoint
		{
			get { return GetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachTimingTable.StartingMeasurementPoint); }
			set { SetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachTimingTable.StartingMeasurementPoint, value); }
		}
		
		public CodeProcedureDistance? EndingMeasurementPoint
		{
			get { return GetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachTimingTable.EndingMeasurementPoint); }
			set { SetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachTimingTable.EndingMeasurementPoint, value); }
		}
		
		public ValDuration Time
		{
			get { return (ValDuration ) GetValue ((int) PropertyApproachTimingTable.Time); }
			set { SetValue ((int) PropertyApproachTimingTable.Time, value); }
		}
		
		public ValSpeed Speed
		{
			get { return (ValSpeed ) GetValue ((int) PropertyApproachTimingTable.Speed); }
			set { SetValue ((int) PropertyApproachTimingTable.Speed, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachTimingTable
	{
		StartingMeasurementPoint = PropertyAObject.NEXT_CLASS,
		EndingMeasurementPoint,
		Time,
		Speed,
		NEXT_CLASS
	}
	
	public static class MetadataApproachTimingTable
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachTimingTable ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachTimingTable.StartingMeasurementPoint, (int) EnumType.CodeProcedureDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachTimingTable.EndingMeasurementPoint, (int) EnumType.CodeProcedureDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachTimingTable.Time, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachTimingTable.Speed, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
