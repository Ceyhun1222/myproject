using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class Obstruction : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Obstruction; }
		}
		
		public ValDistance RequiredClearance
		{
			get { return (ValDistance ) GetValue ((int) PropertyObstruction.RequiredClearance); }
			set { SetValue ((int) PropertyObstruction.RequiredClearance, value); }
		}
		
		public ValDistanceVertical MinimumAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyObstruction.MinimumAltitude); }
			set { SetValue ((int) PropertyObstruction.MinimumAltitude, value); }
		}
		
		public bool? SurfacePenetration
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyObstruction.SurfacePenetration); }
			set { SetNullableFieldValue <bool> ((int) PropertyObstruction.SurfacePenetration, value); }
		}
		
		public double? SlopePenetration
		{
			get { return GetNullableFieldValue <double> ((int) PropertyObstruction.SlopePenetration); }
			set { SetNullableFieldValue <double> ((int) PropertyObstruction.SlopePenetration, value); }
		}
		
		public bool? Controlling
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyObstruction.Controlling); }
			set { SetNullableFieldValue <bool> ((int) PropertyObstruction.Controlling, value); }
		}
		
		public bool? CloseIn
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyObstruction.CloseIn); }
			set { SetNullableFieldValue <bool> ((int) PropertyObstruction.CloseIn, value); }
		}
		
		public FeatureRef VerticalStructureObstruction
		{
			get { return (FeatureRef ) GetValue ((int) PropertyObstruction.VerticalStructureObstruction); }
			set { SetValue ((int) PropertyObstruction.VerticalStructureObstruction, value); }
		}
		
		public List <AltitudeAdjustment> Adjustment
		{
			get { return GetObjectList <AltitudeAdjustment> ((int) PropertyObstruction.Adjustment); }
		}
		
		public List <ObstaclePlacement> ObstaclePlacement
		{
			get { return GetObjectList <ObstaclePlacement> ((int) PropertyObstruction.ObstaclePlacement); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyObstruction
	{
		RequiredClearance = PropertyAObject.NEXT_CLASS,
		MinimumAltitude,
		SurfacePenetration,
		SlopePenetration,
		Controlling,
		CloseIn,
		VerticalStructureObstruction,
		Adjustment,
		ObstaclePlacement,
		NEXT_CLASS
	}
	
	public static class MetadataObstruction
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataObstruction ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyObstruction.RequiredClearance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.MinimumAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.SurfacePenetration, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.SlopePenetration, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.Controlling, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.CloseIn, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			
            //PropInfoList.Add (PropertyObstruction.VerticalStructureObstruction, (int) FeatureType.VerticalStructure, PropertyTypeCharacter.Nullable);
            PropInfoList.Add("VerticalStructureObstruction", (int)PropertyObstruction.VerticalStructureObstruction, (int)FeatureType.VerticalStructure, PropertyTypeCharacter.Nullable, "theVerticalStructure");

			PropInfoList.Add (PropertyObstruction.Adjustment, (int) ObjectType.AltitudeAdjustment, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstruction.ObstaclePlacement, (int) ObjectType.ObstaclePlacement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
