using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApproachDistanceTable : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApproachDistanceTable; }
		}
		
		public CodeProcedureDistance? StartingMeasurementPoint
		{
			get { return GetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachDistanceTable.StartingMeasurementPoint); }
			set { SetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachDistanceTable.StartingMeasurementPoint, value); }
		}
		
		public ValDistanceVertical ValueHAT
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyApproachDistanceTable.ValueHAT); }
			set { SetValue ((int) PropertyApproachDistanceTable.ValueHAT, value); }
		}
		
		public CodeProcedureDistance? EndingMeasurementPoint
		{
			get { return GetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachDistanceTable.EndingMeasurementPoint); }
			set { SetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachDistanceTable.EndingMeasurementPoint, value); }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyApproachDistanceTable.Distance); }
			set { SetValue ((int) PropertyApproachDistanceTable.Distance, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachDistanceTable
	{
		StartingMeasurementPoint = PropertyAObject.NEXT_CLASS,
		ValueHAT,
		EndingMeasurementPoint,
		Distance,
		NEXT_CLASS
	}
	
	public static class MetadataApproachDistanceTable
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachDistanceTable ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachDistanceTable.StartingMeasurementPoint, (int) EnumType.CodeProcedureDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachDistanceTable.ValueHAT, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachDistanceTable.EndingMeasurementPoint, (int) EnumType.CodeProcedureDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachDistanceTable.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
