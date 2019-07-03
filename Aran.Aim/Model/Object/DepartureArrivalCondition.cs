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
	public class DepartureArrivalCondition : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DepartureArrivalCondition; }
		}
		
		public ValDistanceVertical MinimumEnrouteAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyDepartureArrivalCondition.MinimumEnrouteAltitude); }
			set { SetValue ((int) PropertyDepartureArrivalCondition.MinimumEnrouteAltitude, value); }
		}
		
		public ValDistanceVertical MinimumCrossingAtEnd
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyDepartureArrivalCondition.MinimumCrossingAtEnd); }
			set { SetValue ((int) PropertyDepartureArrivalCondition.MinimumCrossingAtEnd, value); }
		}
		
		public CodeVerticalReference? MinimumCrossingAtEndReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyDepartureArrivalCondition.MinimumCrossingAtEndReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyDepartureArrivalCondition.MinimumCrossingAtEndReference, value); }
		}
		
		public ValDistanceVertical MaximumCrossingAtEnd
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyDepartureArrivalCondition.MaximumCrossingAtEnd); }
			set { SetValue ((int) PropertyDepartureArrivalCondition.MaximumCrossingAtEnd, value); }
		}
		
		public CodeVerticalReference? MaximumCrossingAtEndReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyDepartureArrivalCondition.MaximumCrossingAtEndReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyDepartureArrivalCondition.MaximumCrossingAtEndReference, value); }
		}
		
		public AircraftCharacteristic EngineType
		{
			get { return GetObject <AircraftCharacteristic> ((int) PropertyDepartureArrivalCondition.EngineType); }
			set { SetValue ((int) PropertyDepartureArrivalCondition.EngineType, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDepartureArrivalCondition
	{
		MinimumEnrouteAltitude = PropertyAObject.NEXT_CLASS,
		MinimumCrossingAtEnd,
		MinimumCrossingAtEndReference,
		MaximumCrossingAtEnd,
		MaximumCrossingAtEndReference,
		EngineType,
		NEXT_CLASS
	}
	
	public static class MetadataDepartureArrivalCondition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDepartureArrivalCondition ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDepartureArrivalCondition.MinimumEnrouteAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureArrivalCondition.MinimumCrossingAtEnd, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureArrivalCondition.MinimumCrossingAtEndReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureArrivalCondition.MaximumCrossingAtEnd, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureArrivalCondition.MaximumCrossingAtEndReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureArrivalCondition.EngineType, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
