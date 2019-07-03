using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AerialRefuellingAnchor : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AerialRefuellingAnchor; }
		}
		
		public double? OutboundCourse
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAerialRefuellingAnchor.OutboundCourse); }
			set { SetNullableFieldValue <double> ((int) PropertyAerialRefuellingAnchor.OutboundCourse, value); }
		}
		
		public CodeCourse? OutboundCourseType
		{
			get { return GetNullableFieldValue <CodeCourse> ((int) PropertyAerialRefuellingAnchor.OutboundCourseType); }
			set { SetNullableFieldValue <CodeCourse> ((int) PropertyAerialRefuellingAnchor.OutboundCourseType, value); }
		}
		
		public double? InboundCourse
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAerialRefuellingAnchor.InboundCourse); }
			set { SetNullableFieldValue <double> ((int) PropertyAerialRefuellingAnchor.InboundCourse, value); }
		}
		
		public CodeDirectionTurn? TurnDirection
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertyAerialRefuellingAnchor.TurnDirection); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertyAerialRefuellingAnchor.TurnDirection, value); }
		}
		
		public ValSpeed SpeedLimit
		{
			get { return (ValSpeed ) GetValue ((int) PropertyAerialRefuellingAnchor.SpeedLimit); }
			set { SetValue ((int) PropertyAerialRefuellingAnchor.SpeedLimit, value); }
		}
		
		public ValDistance LegSeparation
		{
			get { return (ValDistance ) GetValue ((int) PropertyAerialRefuellingAnchor.LegSeparation); }
			set { SetValue ((int) PropertyAerialRefuellingAnchor.LegSeparation, value); }
		}
		
		public ValDistance LegLength
		{
			get { return (ValDistance ) GetValue ((int) PropertyAerialRefuellingAnchor.LegLength); }
			set { SetValue ((int) PropertyAerialRefuellingAnchor.LegLength, value); }
		}
		
		public ValDistanceVertical RefuellingBaseLevel
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAerialRefuellingAnchor.RefuellingBaseLevel); }
			set { SetValue ((int) PropertyAerialRefuellingAnchor.RefuellingBaseLevel, value); }
		}
		
		public CodeVerticalReference? RefuellingBaseLevelReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAerialRefuellingAnchor.RefuellingBaseLevelReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAerialRefuellingAnchor.RefuellingBaseLevelReference, value); }
		}
		
		public Surface Extent
		{
			get { return GetObject <Surface> ((int) PropertyAerialRefuellingAnchor.Extent); }
			set { SetValue ((int) PropertyAerialRefuellingAnchor.Extent, value); }
		}
		
		public List <AirspaceLayer> VerticalExtent
		{
			get { return GetObjectList <AirspaceLayer> ((int) PropertyAerialRefuellingAnchor.VerticalExtent); }
		}
		
		public List <AerialRefuellingPoint> Point
		{
			get { return GetObjectList <AerialRefuellingPoint> ((int) PropertyAerialRefuellingAnchor.Point); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAerialRefuellingAnchor
	{
		OutboundCourse = PropertyAObject.NEXT_CLASS,
		OutboundCourseType,
		InboundCourse,
		TurnDirection,
		SpeedLimit,
		LegSeparation,
		LegLength,
		RefuellingBaseLevel,
		RefuellingBaseLevelReference,
		Extent,
		VerticalExtent,
		Point,
		NEXT_CLASS
	}
	
	public static class MetadataAerialRefuellingAnchor
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAerialRefuellingAnchor ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAerialRefuellingAnchor.OutboundCourse, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.OutboundCourseType, (int) EnumType.CodeCourse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.InboundCourse, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.TurnDirection, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.SpeedLimit, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.LegSeparation, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.LegLength, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.RefuellingBaseLevel, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.RefuellingBaseLevelReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.VerticalExtent, (int) ObjectType.AirspaceLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingAnchor.Point, (int) ObjectType.AerialRefuellingPoint, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
