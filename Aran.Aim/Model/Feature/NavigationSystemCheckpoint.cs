using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class NavigationSystemCheckpoint : Feature
	{
		public virtual NavigationSystemCheckpointType NavigationSystemCheckpointType 
		{
			get { return (NavigationSystemCheckpointType) FeatureType; }
		}
		
		public CodeCheckpointCategory? Category
		{
			get { return GetNullableFieldValue <CodeCheckpointCategory> ((int) PropertyNavigationSystemCheckpoint.Category); }
			set { SetNullableFieldValue <CodeCheckpointCategory> ((int) PropertyNavigationSystemCheckpoint.Category, value); }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyNavigationSystemCheckpoint.UpperLimit); }
			set { SetValue ((int) PropertyNavigationSystemCheckpoint.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyNavigationSystemCheckpoint.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyNavigationSystemCheckpoint.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyNavigationSystemCheckpoint.LowerLimit); }
			set { SetValue ((int) PropertyNavigationSystemCheckpoint.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyNavigationSystemCheckpoint.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyNavigationSystemCheckpoint.LowerLimitReference, value); }
		}
		
		public CodeAltitudeUse? AltitudeInterpretation
		{
			get { return GetNullableFieldValue <CodeAltitudeUse> ((int) PropertyNavigationSystemCheckpoint.AltitudeInterpretation); }
			set { SetNullableFieldValue <CodeAltitudeUse> ((int) PropertyNavigationSystemCheckpoint.AltitudeInterpretation, value); }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyNavigationSystemCheckpoint.Distance); }
			set { SetValue ((int) PropertyNavigationSystemCheckpoint.Distance, value); }
		}
		
		public double? Angle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyNavigationSystemCheckpoint.Angle); }
			set { SetNullableFieldValue <double> ((int) PropertyNavigationSystemCheckpoint.Angle, value); }
		}
		
		public ElevatedPoint Position
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyNavigationSystemCheckpoint.Position); }
			set { SetValue ((int) PropertyNavigationSystemCheckpoint.Position, value); }
		}
		
		public FeatureRef AirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyNavigationSystemCheckpoint.AirportHeliport); }
			set { SetValue ((int) PropertyNavigationSystemCheckpoint.AirportHeliport, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavigationSystemCheckpoint
	{
		Category = PropertyFeature.NEXT_CLASS,
		UpperLimit,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		AltitudeInterpretation,
		Distance,
		Angle,
		Position,
		AirportHeliport,
		NEXT_CLASS
	}
	
	public static class MetadataNavigationSystemCheckpoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavigationSystemCheckpoint ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.Category, (int) EnumType.CodeCheckpointCategory, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.AltitudeInterpretation, (int) EnumType.CodeAltitudeUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.Angle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.Position, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationSystemCheckpoint.AirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
