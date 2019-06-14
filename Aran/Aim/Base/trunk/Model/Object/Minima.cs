using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class Minima : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Minima; }
		}
		
		public ValDistanceVertical Altitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMinima.Altitude); }
			set { SetValue ((int) PropertyMinima.Altitude, value); }
		}
		
		public CodeMinimumAltitude? AltitudeCode
		{
			get { return GetNullableFieldValue <CodeMinimumAltitude> ((int) PropertyMinima.AltitudeCode); }
			set { SetNullableFieldValue <CodeMinimumAltitude> ((int) PropertyMinima.AltitudeCode, value); }
		}
		
		public CodeVerticalReference? AltitudeReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyMinima.AltitudeReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyMinima.AltitudeReference, value); }
		}
		
		public ValDistanceVertical Height
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMinima.Height); }
			set { SetValue ((int) PropertyMinima.Height, value); }
		}
		
		public ValDistanceVertical MilitaryHeight
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMinima.MilitaryHeight); }
			set { SetValue ((int) PropertyMinima.MilitaryHeight, value); }
		}
		
		public ValDistanceVertical RadioHeight
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMinima.RadioHeight); }
			set { SetValue ((int) PropertyMinima.RadioHeight, value); }
		}
		
		public CodeMinimumHeight? HeightCode
		{
			get { return GetNullableFieldValue <CodeMinimumHeight> ((int) PropertyMinima.HeightCode); }
			set { SetNullableFieldValue <CodeMinimumHeight> ((int) PropertyMinima.HeightCode, value); }
		}
		
		public CodeHeightReference? HeightReference
		{
			get { return GetNullableFieldValue <CodeHeightReference> ((int) PropertyMinima.HeightReference); }
			set { SetNullableFieldValue <CodeHeightReference> ((int) PropertyMinima.HeightReference, value); }
		}
		
		public ValDistance Visibility
		{
			get { return (ValDistance ) GetValue ((int) PropertyMinima.Visibility); }
			set { SetValue ((int) PropertyMinima.Visibility, value); }
		}
		
		public ValDistance MilitaryVisibility
		{
			get { return (ValDistance ) GetValue ((int) PropertyMinima.MilitaryVisibility); }
			set { SetValue ((int) PropertyMinima.MilitaryVisibility, value); }
		}
		
		public bool? MandatoryRVR
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyMinima.MandatoryRVR); }
			set { SetNullableFieldValue <bool> ((int) PropertyMinima.MandatoryRVR, value); }
		}
		
		public bool? RemoteAltimeterMinima
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyMinima.RemoteAltimeterMinima); }
			set { SetNullableFieldValue <bool> ((int) PropertyMinima.RemoteAltimeterMinima, value); }
		}
		
		public List <EquipmentUnavailableAdjustment> AdjustmentINOP
		{
			get { return GetObjectList <EquipmentUnavailableAdjustment> ((int) PropertyMinima.AdjustmentINOP); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMinima
	{
		Altitude = PropertyAObject.NEXT_CLASS,
		AltitudeCode,
		AltitudeReference,
		Height,
		MilitaryHeight,
		RadioHeight,
		HeightCode,
		HeightReference,
		Visibility,
		MilitaryVisibility,
		MandatoryRVR,
		RemoteAltimeterMinima,
		AdjustmentINOP,
		NEXT_CLASS
	}
	
	public static class MetadataMinima
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMinima ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMinima.Altitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.AltitudeCode, (int) EnumType.CodeMinimumAltitude, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.AltitudeReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.Height, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.MilitaryHeight, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.RadioHeight, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.HeightCode, (int) EnumType.CodeMinimumHeight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.HeightReference, (int) EnumType.CodeHeightReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.Visibility, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.MilitaryVisibility, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.MandatoryRVR, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.RemoteAltimeterMinima, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMinima.AdjustmentINOP, (int) ObjectType.EquipmentUnavailableAdjustment, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
