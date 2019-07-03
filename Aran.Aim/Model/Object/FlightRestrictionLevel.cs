using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FlightRestrictionLevel : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightRestrictionLevel; }
		}
		
		public ValDistanceVertical UpperLevel
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyFlightRestrictionLevel.UpperLevel); }
			set { SetValue ((int) PropertyFlightRestrictionLevel.UpperLevel, value); }
		}
		
		public CodeVerticalReference? UpperLevelReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyFlightRestrictionLevel.UpperLevelReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyFlightRestrictionLevel.UpperLevelReference, value); }
		}
		
		public ValDistanceVertical LowerLevel
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyFlightRestrictionLevel.LowerLevel); }
			set { SetValue ((int) PropertyFlightRestrictionLevel.LowerLevel, value); }
		}
		
		public CodeVerticalReference? LowerLevelReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyFlightRestrictionLevel.LowerLevelReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyFlightRestrictionLevel.LowerLevelReference, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightRestrictionLevel
	{
		UpperLevel = PropertyAObject.NEXT_CLASS,
		UpperLevelReference,
		LowerLevel,
		LowerLevelReference,
		NEXT_CLASS
	}
	
	public static class MetadataFlightRestrictionLevel
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightRestrictionLevel ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightRestrictionLevel.UpperLevel, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestrictionLevel.UpperLevelReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestrictionLevel.LowerLevel, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestrictionLevel.LowerLevelReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
