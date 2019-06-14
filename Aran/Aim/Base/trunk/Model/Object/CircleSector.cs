using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class CircleSector : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CircleSector; }
		}
		
		public CodeArcDirection? ArcDirection
		{
			get { return GetNullableFieldValue <CodeArcDirection> ((int) PropertyCircleSector.ArcDirection); }
			set { SetNullableFieldValue <CodeArcDirection> ((int) PropertyCircleSector.ArcDirection, value); }
		}
		
		public double? FromAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyCircleSector.FromAngle); }
			set { SetNullableFieldValue <double> ((int) PropertyCircleSector.FromAngle, value); }
		}
		
		public double? ToAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyCircleSector.ToAngle); }
			set { SetNullableFieldValue <double> ((int) PropertyCircleSector.ToAngle, value); }
		}
		
		public CodeBearing? AngleType
		{
			get { return GetNullableFieldValue <CodeBearing> ((int) PropertyCircleSector.AngleType); }
			set { SetNullableFieldValue <CodeBearing> ((int) PropertyCircleSector.AngleType, value); }
		}
		
		public CodeDirectionReference? AngleDirectionReference
		{
			get { return GetNullableFieldValue <CodeDirectionReference> ((int) PropertyCircleSector.AngleDirectionReference); }
			set { SetNullableFieldValue <CodeDirectionReference> ((int) PropertyCircleSector.AngleDirectionReference, value); }
		}
		
		public ValDistance InnerDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertyCircleSector.InnerDistance); }
			set { SetValue ((int) PropertyCircleSector.InnerDistance, value); }
		}
		
		public ValDistance OuterDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertyCircleSector.OuterDistance); }
			set { SetValue ((int) PropertyCircleSector.OuterDistance, value); }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyCircleSector.UpperLimit); }
			set { SetValue ((int) PropertyCircleSector.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyCircleSector.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyCircleSector.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyCircleSector.LowerLimit); }
			set { SetValue ((int) PropertyCircleSector.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyCircleSector.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyCircleSector.LowerLimitReference, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCircleSector
	{
		ArcDirection = PropertyAObject.NEXT_CLASS,
		FromAngle,
		ToAngle,
		AngleType,
		AngleDirectionReference,
		InnerDistance,
		OuterDistance,
		UpperLimit,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		NEXT_CLASS
	}
	
	public static class MetadataCircleSector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCircleSector ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCircleSector.ArcDirection, (int) EnumType.CodeArcDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.FromAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.ToAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.AngleType, (int) EnumType.CodeBearing, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.AngleDirectionReference, (int) EnumType.CodeDirectionReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.InnerDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.OuterDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCircleSector.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
