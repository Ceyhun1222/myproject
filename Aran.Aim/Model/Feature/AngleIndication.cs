using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AngleIndication : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AngleIndication; }
		}
		
		public double? Angle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAngleIndication.Angle); }
			set { SetNullableFieldValue <double> ((int) PropertyAngleIndication.Angle, value); }
		}
		
		public CodeBearing? AngleType
		{
			get { return GetNullableFieldValue <CodeBearing> ((int) PropertyAngleIndication.AngleType); }
			set { SetNullableFieldValue <CodeBearing> ((int) PropertyAngleIndication.AngleType, value); }
		}
		
		public CodeDirectionReference? IndicationDirection
		{
			get { return GetNullableFieldValue <CodeDirectionReference> ((int) PropertyAngleIndication.IndicationDirection); }
			set { SetNullableFieldValue <CodeDirectionReference> ((int) PropertyAngleIndication.IndicationDirection, value); }
		}
		
		public double? TrueAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAngleIndication.TrueAngle); }
			set { SetNullableFieldValue <double> ((int) PropertyAngleIndication.TrueAngle, value); }
		}
		
		public CodeCardinalDirection? CardinalDirection
		{
			get { return GetNullableFieldValue <CodeCardinalDirection> ((int) PropertyAngleIndication.CardinalDirection); }
			set { SetNullableFieldValue <CodeCardinalDirection> ((int) PropertyAngleIndication.CardinalDirection, value); }
		}
		
		public ValDistanceVertical MinimumReceptionAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAngleIndication.MinimumReceptionAltitude); }
			set { SetValue ((int) PropertyAngleIndication.MinimumReceptionAltitude, value); }
		}
		
		public FeatureRef Fix
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAngleIndication.Fix); }
			set { SetValue ((int) PropertyAngleIndication.Fix, value); }
		}
		
		public SignificantPoint PointChoice
		{
			get { return GetObject <SignificantPoint> ((int) PropertyAngleIndication.PointChoice); }
			set { SetValue ((int) PropertyAngleIndication.PointChoice, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAngleIndication
	{
		Angle = PropertyFeature.NEXT_CLASS,
		AngleType,
		IndicationDirection,
		TrueAngle,
		CardinalDirection,
		MinimumReceptionAltitude,
		Fix,
		PointChoice,
		NEXT_CLASS
	}
	
	public static class MetadataAngleIndication
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAngleIndication ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAngleIndication.Angle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.AngleType, (int) EnumType.CodeBearing, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.IndicationDirection, (int) EnumType.CodeDirectionReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.TrueAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.CardinalDirection, (int) EnumType.CodeCardinalDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.MinimumReceptionAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.Fix, (int) FeatureType.DesignatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAngleIndication.PointChoice, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
