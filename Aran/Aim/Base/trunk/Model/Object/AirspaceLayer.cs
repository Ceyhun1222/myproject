using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AirspaceLayer : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceLayer; }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceLayer.UpperLimit); }
			set { SetValue ((int) PropertyAirspaceLayer.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceLayer.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceLayer.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceLayer.LowerLimit); }
			set { SetValue ((int) PropertyAirspaceLayer.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceLayer.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceLayer.LowerLimitReference, value); }
		}
		
		public CodeAltitudeUse? AltitudeInterpretation
		{
			get { return GetNullableFieldValue <CodeAltitudeUse> ((int) PropertyAirspaceLayer.AltitudeInterpretation); }
			set { SetNullableFieldValue <CodeAltitudeUse> ((int) PropertyAirspaceLayer.AltitudeInterpretation, value); }
		}
		
		public FeatureRef DiscreteLevelSeries
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirspaceLayer.DiscreteLevelSeries); }
			set { SetValue ((int) PropertyAirspaceLayer.DiscreteLevelSeries, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceLayer
	{
		UpperLimit = PropertyAObject.NEXT_CLASS,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		AltitudeInterpretation,
		DiscreteLevelSeries,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceLayer
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceLayer ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceLayer.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayer.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayer.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayer.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayer.AltitudeInterpretation, (int) EnumType.CodeAltitudeUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayer.DiscreteLevelSeries, (int) FeatureType.StandardLevelColumn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
