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
	public class AirspaceVolume : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceVolume; }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceVolume.UpperLimit); }
			set { SetValue ((int) PropertyAirspaceVolume.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical MaximumLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceVolume.MaximumLimit); }
			set { SetValue ((int) PropertyAirspaceVolume.MaximumLimit, value); }
		}
		
		public CodeVerticalReference? MaximumLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.MaximumLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.MaximumLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceVolume.LowerLimit); }
			set { SetValue ((int) PropertyAirspaceVolume.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.LowerLimitReference, value); }
		}
		
		public ValDistanceVertical MinimumLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirspaceVolume.MinimumLimit); }
			set { SetValue ((int) PropertyAirspaceVolume.MinimumLimit, value); }
		}
		
		public CodeVerticalReference? MinimumLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.MinimumLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyAirspaceVolume.MinimumLimitReference, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyAirspaceVolume.Width); }
			set { SetValue ((int) PropertyAirspaceVolume.Width, value); }
		}
		
		public Surface HorizontalProjection
		{
			get { return GetObject <Surface> ((int) PropertyAirspaceVolume.HorizontalProjection); }
			set { SetValue ((int) PropertyAirspaceVolume.HorizontalProjection, value); }
		}
		
		public Curve Centreline
		{
			get { return GetObject <Curve> ((int) PropertyAirspaceVolume.Centreline); }
			set { SetValue ((int) PropertyAirspaceVolume.Centreline, value); }
		}
		
		public AirspaceVolumeDependency ContributorAirspace
		{
			get { return GetObject <AirspaceVolumeDependency> ((int) PropertyAirspaceVolume.ContributorAirspace); }
			set { SetValue ((int) PropertyAirspaceVolume.ContributorAirspace, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceVolume
	{
		UpperLimit = PropertyAObject.NEXT_CLASS,
		UpperLimitReference,
		MaximumLimit,
		MaximumLimitReference,
		LowerLimit,
		LowerLimitReference,
		MinimumLimit,
		MinimumLimitReference,
		Width,
		HorizontalProjection,
		Centreline,
		ContributorAirspace,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceVolume
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceVolume ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceVolume.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.MaximumLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.MaximumLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.MinimumLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.MinimumLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.HorizontalProjection, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.Centreline, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceVolume.ContributorAirspace, (int) ObjectType.AirspaceVolumeDependency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
