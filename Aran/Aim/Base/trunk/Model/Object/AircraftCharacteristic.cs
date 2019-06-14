using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AircraftCharacteristic : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AircraftCharacteristic; }
		}
		
		public CodeAircraft? Type
		{
			get { return GetNullableFieldValue <CodeAircraft> ((int) PropertyAircraftCharacteristic.Type); }
			set { SetNullableFieldValue <CodeAircraft> ((int) PropertyAircraftCharacteristic.Type, value); }
		}
		
		public CodeAircraftEngine? Engine
		{
			get { return GetNullableFieldValue <CodeAircraftEngine> ((int) PropertyAircraftCharacteristic.Engine); }
			set { SetNullableFieldValue <CodeAircraftEngine> ((int) PropertyAircraftCharacteristic.Engine, value); }
		}
		
		public CodeAircraftEngineNumber? NumberEngine
		{
			get { return GetNullableFieldValue <CodeAircraftEngineNumber> ((int) PropertyAircraftCharacteristic.NumberEngine); }
			set { SetNullableFieldValue <CodeAircraftEngineNumber> ((int) PropertyAircraftCharacteristic.NumberEngine, value); }
		}
		
		public string TypeAircraftICAO
		{
			get { return GetFieldValue <string> ((int) PropertyAircraftCharacteristic.TypeAircraftICAO); }
			set { SetFieldValue <string> ((int) PropertyAircraftCharacteristic.TypeAircraftICAO, value); }
		}
		
		public CodeAircraftCategory? AircraftLandingCategory
		{
			get { return GetNullableFieldValue <CodeAircraftCategory> ((int) PropertyAircraftCharacteristic.AircraftLandingCategory); }
			set { SetNullableFieldValue <CodeAircraftCategory> ((int) PropertyAircraftCharacteristic.AircraftLandingCategory, value); }
		}
		
		public ValDistance WingSpan
		{
			get { return (ValDistance ) GetValue ((int) PropertyAircraftCharacteristic.WingSpan); }
			set { SetValue ((int) PropertyAircraftCharacteristic.WingSpan, value); }
		}
		
		public CodeValueInterpretation? WingSpanInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.WingSpanInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.WingSpanInterpretation, value); }
		}
		
		public CodeAircraftWingspanClass? ClassWingSpan
		{
			get { return GetNullableFieldValue <CodeAircraftWingspanClass> ((int) PropertyAircraftCharacteristic.ClassWingSpan); }
			set { SetNullableFieldValue <CodeAircraftWingspanClass> ((int) PropertyAircraftCharacteristic.ClassWingSpan, value); }
		}
		
		public ValWeight Weight
		{
			get { return (ValWeight ) GetValue ((int) PropertyAircraftCharacteristic.Weight); }
			set { SetValue ((int) PropertyAircraftCharacteristic.Weight, value); }
		}
		
		public CodeValueInterpretation? WeightInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.WeightInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.WeightInterpretation, value); }
		}
		
		public uint? Passengers
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAircraftCharacteristic.Passengers); }
			set { SetNullableFieldValue <uint> ((int) PropertyAircraftCharacteristic.Passengers, value); }
		}
		
		public CodeValueInterpretation? PassengersInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.PassengersInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.PassengersInterpretation, value); }
		}
		
		public ValSpeed Speed
		{
			get { return (ValSpeed ) GetValue ((int) PropertyAircraftCharacteristic.Speed); }
			set { SetValue ((int) PropertyAircraftCharacteristic.Speed, value); }
		}
		
		public CodeValueInterpretation? SpeedInterpretation
		{
			get { return GetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.SpeedInterpretation); }
			set { SetNullableFieldValue <CodeValueInterpretation> ((int) PropertyAircraftCharacteristic.SpeedInterpretation, value); }
		}
		
		public CodeWakeTurbulence? WakeTurbulence
		{
			get { return GetNullableFieldValue <CodeWakeTurbulence> ((int) PropertyAircraftCharacteristic.WakeTurbulence); }
			set { SetNullableFieldValue <CodeWakeTurbulence> ((int) PropertyAircraftCharacteristic.WakeTurbulence, value); }
		}
		
		public CodeNavigationEquipment? NavigationEquipment
		{
			get { return GetNullableFieldValue <CodeNavigationEquipment> ((int) PropertyAircraftCharacteristic.NavigationEquipment); }
			set { SetNullableFieldValue <CodeNavigationEquipment> ((int) PropertyAircraftCharacteristic.NavigationEquipment, value); }
		}
		
		public CodeNavigationSpecification? NavigationSpecification
		{
			get { return GetNullableFieldValue <CodeNavigationSpecification> ((int) PropertyAircraftCharacteristic.NavigationSpecification); }
			set { SetNullableFieldValue <CodeNavigationSpecification> ((int) PropertyAircraftCharacteristic.NavigationSpecification, value); }
		}
		
		public CodeRVSM? VerticalSeparationCapability
		{
			get { return GetNullableFieldValue <CodeRVSM> ((int) PropertyAircraftCharacteristic.VerticalSeparationCapability); }
			set { SetNullableFieldValue <CodeRVSM> ((int) PropertyAircraftCharacteristic.VerticalSeparationCapability, value); }
		}
		
		public CodeEquipmentAntiCollision? AntiCollisionAndSeparationEquipment
		{
			get { return GetNullableFieldValue <CodeEquipmentAntiCollision> ((int) PropertyAircraftCharacteristic.AntiCollisionAndSeparationEquipment); }
			set { SetNullableFieldValue <CodeEquipmentAntiCollision> ((int) PropertyAircraftCharacteristic.AntiCollisionAndSeparationEquipment, value); }
		}
		
		public CodeCommunicationMode? CommunicationEquipment
		{
			get { return GetNullableFieldValue <CodeCommunicationMode> ((int) PropertyAircraftCharacteristic.CommunicationEquipment); }
			set { SetNullableFieldValue <CodeCommunicationMode> ((int) PropertyAircraftCharacteristic.CommunicationEquipment, value); }
		}
		
		public CodeTransponder? SurveillanceEquipment
		{
			get { return GetNullableFieldValue <CodeTransponder> ((int) PropertyAircraftCharacteristic.SurveillanceEquipment); }
			set { SetNullableFieldValue <CodeTransponder> ((int) PropertyAircraftCharacteristic.SurveillanceEquipment, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAircraftCharacteristic
	{
		Type = PropertyAObject.NEXT_CLASS,
		Engine,
		NumberEngine,
		TypeAircraftICAO,
		AircraftLandingCategory,
		WingSpan,
		WingSpanInterpretation,
		ClassWingSpan,
		Weight,
		WeightInterpretation,
		Passengers,
		PassengersInterpretation,
		Speed,
		SpeedInterpretation,
		WakeTurbulence,
		NavigationEquipment,
		NavigationSpecification,
		VerticalSeparationCapability,
		AntiCollisionAndSeparationEquipment,
		CommunicationEquipment,
		SurveillanceEquipment,
		NEXT_CLASS
	}
	
	public static class MetadataAircraftCharacteristic
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAircraftCharacteristic ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAircraftCharacteristic.Type, (int) EnumType.CodeAircraft, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.Engine, (int) EnumType.CodeAircraftEngine, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.NumberEngine, (int) EnumType.CodeAircraftEngineNumber, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.TypeAircraftICAO, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.AircraftLandingCategory, (int) EnumType.CodeAircraftCategory, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.WingSpan, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.WingSpanInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.ClassWingSpan, (int) EnumType.CodeAircraftWingspanClass, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.Weight, (int) DataType.ValWeight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.WeightInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.Passengers, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.PassengersInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.Speed, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.SpeedInterpretation, (int) EnumType.CodeValueInterpretation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.WakeTurbulence, (int) EnumType.CodeWakeTurbulence, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.NavigationEquipment, (int) EnumType.CodeNavigationEquipment, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.NavigationSpecification, (int) EnumType.CodeNavigationSpecification, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.VerticalSeparationCapability, (int) EnumType.CodeRVSM, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.AntiCollisionAndSeparationEquipment, (int) EnumType.CodeEquipmentAntiCollision, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.CommunicationEquipment, (int) EnumType.CodeCommunicationMode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftCharacteristic.SurveillanceEquipment, (int) EnumType.CodeTransponder, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
