using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AltitudeAdjustment : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AltitudeAdjustment; }
		}
		
		public CodeAltitudeAdjustment? AltitudeAdjustmentType
		{
			get { return GetNullableFieldValue <CodeAltitudeAdjustment> ((int) PropertyAltitudeAdjustment.AltitudeAdjustmentType); }
			set { SetNullableFieldValue <CodeAltitudeAdjustment> ((int) PropertyAltitudeAdjustment.AltitudeAdjustmentType, value); }
		}
		
		public bool? PrimaryAlternateMinimum
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAltitudeAdjustment.PrimaryAlternateMinimum); }
			set { SetNullableFieldValue <bool> ((int) PropertyAltitudeAdjustment.PrimaryAlternateMinimum, value); }
		}
		
		public ValDistanceVertical AltitudeAdjustmentP
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAltitudeAdjustment.AltitudeAdjustmentP); }
			set { SetValue ((int) PropertyAltitudeAdjustment.AltitudeAdjustmentP, value); }
		}
		
		public bool? LocalRemoteCode
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAltitudeAdjustment.LocalRemoteCode); }
			set { SetNullableFieldValue <bool> ((int) PropertyAltitudeAdjustment.LocalRemoteCode, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAltitudeAdjustment
	{
		AltitudeAdjustmentType = PropertyAObject.NEXT_CLASS,
		PrimaryAlternateMinimum,
		AltitudeAdjustmentP,
		LocalRemoteCode,
		NEXT_CLASS
	}
	
	public static class MetadataAltitudeAdjustment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAltitudeAdjustment ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAltitudeAdjustment.AltitudeAdjustmentType, (int) EnumType.CodeAltitudeAdjustment, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAltitudeAdjustment.PrimaryAlternateMinimum, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAltitudeAdjustment.AltitudeAdjustmentP, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAltitudeAdjustment.LocalRemoteCode, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
