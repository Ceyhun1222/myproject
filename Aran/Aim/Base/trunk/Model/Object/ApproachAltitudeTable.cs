using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApproachAltitudeTable : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApproachAltitudeTable; }
		}
		
		public CodeProcedureDistance? MeasurementPoint
		{
			get { return GetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachAltitudeTable.MeasurementPoint); }
			set { SetNullableFieldValue <CodeProcedureDistance> ((int) PropertyApproachAltitudeTable.MeasurementPoint, value); }
		}
		
		public ValDistanceVertical Altitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyApproachAltitudeTable.Altitude); }
			set { SetValue ((int) PropertyApproachAltitudeTable.Altitude, value); }
		}
		
		public CodeVerticalReference? AltitudeReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyApproachAltitudeTable.AltitudeReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyApproachAltitudeTable.AltitudeReference, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachAltitudeTable
	{
		MeasurementPoint = PropertyAObject.NEXT_CLASS,
		Altitude,
		AltitudeReference,
		NEXT_CLASS
	}
	
	public static class MetadataApproachAltitudeTable
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachAltitudeTable ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachAltitudeTable.MeasurementPoint, (int) EnumType.CodeProcedureDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachAltitudeTable.Altitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachAltitudeTable.AltitudeReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
