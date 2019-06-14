using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SurfaceCharacteristics : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SurfaceCharacteristics; }
		}
		
		public CodeSurfaceComposition? Composition
		{
			get { return GetNullableFieldValue <CodeSurfaceComposition> ((int) PropertySurfaceCharacteristics.Composition); }
			set { SetNullableFieldValue <CodeSurfaceComposition> ((int) PropertySurfaceCharacteristics.Composition, value); }
		}
		
		public CodeSurfacePreparation? Preparation
		{
			get { return GetNullableFieldValue <CodeSurfacePreparation> ((int) PropertySurfaceCharacteristics.Preparation); }
			set { SetNullableFieldValue <CodeSurfacePreparation> ((int) PropertySurfaceCharacteristics.Preparation, value); }
		}
		
		public CodeSurfaceCondition? SurfaceCondition
		{
			get { return GetNullableFieldValue <CodeSurfaceCondition> ((int) PropertySurfaceCharacteristics.SurfaceCondition); }
			set { SetNullableFieldValue <CodeSurfaceCondition> ((int) PropertySurfaceCharacteristics.SurfaceCondition, value); }
		}
		
		public double? ClassPCN
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurfaceCharacteristics.ClassPCN); }
			set { SetNullableFieldValue <double> ((int) PropertySurfaceCharacteristics.ClassPCN, value); }
		}
		
		public CodePCNPavement? PavementTypePCN
		{
			get { return GetNullableFieldValue <CodePCNPavement> ((int) PropertySurfaceCharacteristics.PavementTypePCN); }
			set { SetNullableFieldValue <CodePCNPavement> ((int) PropertySurfaceCharacteristics.PavementTypePCN, value); }
		}
		
		public CodePCNSubgrade? PavementSubgradePCN
		{
			get { return GetNullableFieldValue <CodePCNSubgrade> ((int) PropertySurfaceCharacteristics.PavementSubgradePCN); }
			set { SetNullableFieldValue <CodePCNSubgrade> ((int) PropertySurfaceCharacteristics.PavementSubgradePCN, value); }
		}
		
		public CodePCNTyrePressure? MaxTyrePressurePCN
		{
			get { return GetNullableFieldValue <CodePCNTyrePressure> ((int) PropertySurfaceCharacteristics.MaxTyrePressurePCN); }
			set { SetNullableFieldValue <CodePCNTyrePressure> ((int) PropertySurfaceCharacteristics.MaxTyrePressurePCN, value); }
		}
		
		public CodePCNMethod? EvaluationMethodPCN
		{
			get { return GetNullableFieldValue <CodePCNMethod> ((int) PropertySurfaceCharacteristics.EvaluationMethodPCN); }
			set { SetNullableFieldValue <CodePCNMethod> ((int) PropertySurfaceCharacteristics.EvaluationMethodPCN, value); }
		}
		
		public double? ClassLCN
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurfaceCharacteristics.ClassLCN); }
			set { SetNullableFieldValue <double> ((int) PropertySurfaceCharacteristics.ClassLCN, value); }
		}
		
		public ValWeight WeightSIWL
		{
			get { return (ValWeight ) GetValue ((int) PropertySurfaceCharacteristics.WeightSIWL); }
			set { SetValue ((int) PropertySurfaceCharacteristics.WeightSIWL, value); }
		}
		
		public ValPressure TyrePressureSIWL
		{
			get { return (ValPressure ) GetValue ((int) PropertySurfaceCharacteristics.TyrePressureSIWL); }
			set { SetValue ((int) PropertySurfaceCharacteristics.TyrePressureSIWL, value); }
		}
		
		public ValWeight WeightAUW
		{
			get { return (ValWeight ) GetValue ((int) PropertySurfaceCharacteristics.WeightAUW); }
			set { SetValue ((int) PropertySurfaceCharacteristics.WeightAUW, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurfaceCharacteristics
	{
		Composition = PropertyAObject.NEXT_CLASS,
		Preparation,
		SurfaceCondition,
		ClassPCN,
		PavementTypePCN,
		PavementSubgradePCN,
		MaxTyrePressurePCN,
		EvaluationMethodPCN,
		ClassLCN,
		WeightSIWL,
		TyrePressureSIWL,
		WeightAUW,
		NEXT_CLASS
	}
	
	public static class MetadataSurfaceCharacteristics
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurfaceCharacteristics ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurfaceCharacteristics.Composition, (int) EnumType.CodeSurfaceComposition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.Preparation, (int) EnumType.CodeSurfacePreparation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.SurfaceCondition, (int) EnumType.CodeSurfaceCondition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.ClassPCN, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.PavementTypePCN, (int) EnumType.CodePCNPavement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.PavementSubgradePCN, (int) EnumType.CodePCNSubgrade, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.MaxTyrePressurePCN, (int) EnumType.CodePCNTyrePressure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.EvaluationMethodPCN, (int) EnumType.CodePCNMethod, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.ClassLCN, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.WeightSIWL, (int) DataType.ValWeight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.TyrePressureSIWL, (int) DataType.ValPressure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurfaceCharacteristics.WeightAUW, (int) DataType.ValWeight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
