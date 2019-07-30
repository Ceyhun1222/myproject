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
	public class PointReference : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.PointReference; }
		}
		
		public CodeReferenceRole? Role
		{
			get { return GetNullableFieldValue <CodeReferenceRole> ((int) PropertyPointReference.Role); }
			set { SetNullableFieldValue <CodeReferenceRole> ((int) PropertyPointReference.Role, value); }
		}
		
		public ValDistanceSigned PriorFixTolerance
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyPointReference.PriorFixTolerance); }
			set { SetValue ((int) PropertyPointReference.PriorFixTolerance, value); }
		}
		
		public ValDistanceSigned PostFixTolerance
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyPointReference.PostFixTolerance); }
			set { SetValue ((int) PropertyPointReference.PostFixTolerance, value); }
		}
		
		public FeatureRef Point
		{
			get { return (FeatureRef ) GetValue ((int) PropertyPointReference.Point); }
			set { SetValue ((int) PropertyPointReference.Point, value); }
		}
		
		public List <AngleUse> FacilityAngle
		{
			get { return GetObjectList <AngleUse> ((int) PropertyPointReference.FacilityAngle); }
		}
		
		public List <FeatureRefObject> FacilityDistance
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyPointReference.FacilityDistance); }
		}
		
		public Surface FixToleranceArea
		{
			get { return GetObject <Surface> ((int) PropertyPointReference.FixToleranceArea); }
			set { SetValue ((int) PropertyPointReference.FixToleranceArea, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPointReference
	{
		Role = PropertyAObject.NEXT_CLASS,
		PriorFixTolerance,
		PostFixTolerance,
		Point,
		FacilityAngle,
		FacilityDistance,
		FixToleranceArea,
		NEXT_CLASS
	}
	
	public static class MetadataPointReference
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPointReference ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPointReference.Role, (int) EnumType.CodeReferenceRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.PriorFixTolerance, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.PostFixTolerance, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.Point, (int) FeatureType.DesignatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.FacilityAngle, (int) ObjectType.AngleUse, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.FacilityDistance, (int) FeatureType.DistanceIndication, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPointReference.FixToleranceArea, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
