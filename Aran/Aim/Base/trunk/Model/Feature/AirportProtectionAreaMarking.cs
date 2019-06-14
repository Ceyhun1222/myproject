using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirportProtectionAreaMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportProtectionAreaMarking; }
		}
		
		public CodeProtectAreaSection? MarkingLocation
		{
			get { return GetNullableFieldValue <CodeProtectAreaSection> ((int) PropertyAirportProtectionAreaMarking.MarkingLocation); }
			set { SetNullableFieldValue <CodeProtectAreaSection> ((int) PropertyAirportProtectionAreaMarking.MarkingLocation, value); }
		}
		
		public AbstractAirportHeliportProtectionAreaRef MarkedProtectionArea
		{
			get { return (AbstractAirportHeliportProtectionAreaRef ) GetValue ((int) PropertyAirportProtectionAreaMarking.MarkedProtectionArea); }
			set { SetValue ((int) PropertyAirportProtectionAreaMarking.MarkedProtectionArea, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportProtectionAreaMarking
	{
		MarkingLocation = PropertyMarking.NEXT_CLASS,
		MarkedProtectionArea,
		NEXT_CLASS
	}
	
	public static class MetadataAirportProtectionAreaMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportProtectionAreaMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportProtectionAreaMarking.MarkingLocation, (int) EnumType.CodeProtectAreaSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportProtectionAreaMarking.MarkedProtectionArea, (int) DataType.AbstractAirportHeliportProtectionAreaRef, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
