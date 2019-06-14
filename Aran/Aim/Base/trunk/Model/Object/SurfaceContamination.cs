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
	public abstract class SurfaceContamination : AObject
	{
		public virtual SurfaceContaminationType SurfaceContaminationType 
		{
			get { return (SurfaceContaminationType) ObjectType; }
		}
		
		public DateTime? ObservationTime
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertySurfaceContamination.ObservationTime); }
			set { SetNullableFieldValue <DateTime> ((int) PropertySurfaceContamination.ObservationTime, value); }
		}
		
		public ValDepth Depth
		{
			get { return (ValDepth ) GetValue ((int) PropertySurfaceContamination.Depth); }
			set { SetValue ((int) PropertySurfaceContamination.Depth, value); }
		}
		
		public double? FrictionCoefficient
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurfaceContamination.FrictionCoefficient); }
			set { SetNullableFieldValue <double> ((int) PropertySurfaceContamination.FrictionCoefficient, value); }
		}
		
		public CodeFrictionEstimate? FrictionEstimation
		{
			get { return GetNullableFieldValue <CodeFrictionEstimate> ((int) PropertySurfaceContamination.FrictionEstimation); }
			set { SetNullableFieldValue <CodeFrictionEstimate> ((int) PropertySurfaceContamination.FrictionEstimation, value); }
		}
		
		public CodeFrictionDevice? FrictionDevice
		{
			get { return GetNullableFieldValue <CodeFrictionDevice> ((int) PropertySurfaceContamination.FrictionDevice); }
			set { SetNullableFieldValue <CodeFrictionDevice> ((int) PropertySurfaceContamination.FrictionDevice, value); }
		}
		
		public bool? ObscuredLights
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySurfaceContamination.ObscuredLights); }
			set { SetNullableFieldValue <bool> ((int) PropertySurfaceContamination.ObscuredLights, value); }
		}
		
		public string FurtherClearanceTime
		{
			get { return GetFieldValue <string> ((int) PropertySurfaceContamination.FurtherClearanceTime); }
			set { SetFieldValue <string> ((int) PropertySurfaceContamination.FurtherClearanceTime, value); }
		}
		
		public bool? FurtherTotalClearance
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySurfaceContamination.FurtherTotalClearance); }
			set { SetNullableFieldValue <bool> ((int) PropertySurfaceContamination.FurtherTotalClearance, value); }
		}
		
		public DateTime? NextObservationTime
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertySurfaceContamination.NextObservationTime); }
			set { SetNullableFieldValue <DateTime> ((int) PropertySurfaceContamination.NextObservationTime, value); }
		}
		
		public double? Proportion
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurfaceContamination.Proportion); }
			set { SetNullableFieldValue <double> ((int) PropertySurfaceContamination.Proportion, value); }
		}
		
		public List <Ridge> CriticalRidge
		{
			get { return GetObjectList <Ridge> ((int) PropertySurfaceContamination.CriticalRidge); }
		}
		
		public List <SurfaceContaminationLayer> Layer
		{
			get { return GetObjectList <SurfaceContaminationLayer> ((int) PropertySurfaceContamination.Layer); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurfaceContamination
	{
		ObservationTime = PropertyAObject.NEXT_CLASS,
		Depth,
		FrictionCoefficient,
		FrictionEstimation,
		FrictionDevice,
		ObscuredLights,
		FurtherClearanceTime,
		FurtherTotalClearance,
		NextObservationTime,
		Proportion,
		CriticalRidge,
		Layer,
		NEXT_CLASS
	}
	
	public static class MetadataSurfaceContamination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurfaceContamination ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurfaceContamination.ObservationTime, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.Depth, (int) DataType.ValDepth, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.FrictionCoefficient, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.FrictionEstimation, (int) EnumType.CodeFrictionEstimate, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.FrictionDevice, (int) EnumType.CodeFrictionDevice, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.ObscuredLights, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.FurtherClearanceTime, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.FurtherTotalClearance, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.NextObservationTime, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.Proportion, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.CriticalRidge, (int) ObjectType.Ridge, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceContamination.Layer, (int) ObjectType.SurfaceContaminationLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
