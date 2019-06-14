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
	public class ObstacleAssessmentArea : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ObstacleAssessmentArea; }
		}
		
		public CodeObstacleAssessmentSurface? Type
		{
			get { return GetNullableFieldValue <CodeObstacleAssessmentSurface> ((int) PropertyObstacleAssessmentArea.Type); }
			set { SetNullableFieldValue <CodeObstacleAssessmentSurface> ((int) PropertyObstacleAssessmentArea.Type, value); }
		}
		
		public uint? SectionNumber
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyObstacleAssessmentArea.SectionNumber); }
			set { SetNullableFieldValue <uint> ((int) PropertyObstacleAssessmentArea.SectionNumber, value); }
		}
		
		public double? Slope
		{
			get { return GetNullableFieldValue <double> ((int) PropertyObstacleAssessmentArea.Slope); }
			set { SetNullableFieldValue <double> ((int) PropertyObstacleAssessmentArea.Slope, value); }
		}
		
		public ValDistanceVertical AssessedAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyObstacleAssessmentArea.AssessedAltitude); }
			set { SetValue ((int) PropertyObstacleAssessmentArea.AssessedAltitude, value); }
		}
		
		public ValDistanceVertical SlopeLowerAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyObstacleAssessmentArea.SlopeLowerAltitude); }
			set { SetValue ((int) PropertyObstacleAssessmentArea.SlopeLowerAltitude, value); }
		}
		
		public double? GradientLowHigh
		{
			get { return GetNullableFieldValue <double> ((int) PropertyObstacleAssessmentArea.GradientLowHigh); }
			set { SetNullableFieldValue <double> ((int) PropertyObstacleAssessmentArea.GradientLowHigh, value); }
		}
		
		public CodeObstructionIdSurfaceZone? SurfaceZone
		{
			get { return GetNullableFieldValue <CodeObstructionIdSurfaceZone> ((int) PropertyObstacleAssessmentArea.SurfaceZone); }
			set { SetNullableFieldValue <CodeObstructionIdSurfaceZone> ((int) PropertyObstacleAssessmentArea.SurfaceZone, value); }
		}
		
		public string SafetyRegulation
		{
			get { return GetFieldValue <string> ((int) PropertyObstacleAssessmentArea.SafetyRegulation); }
			set { SetFieldValue <string> ((int) PropertyObstacleAssessmentArea.SafetyRegulation, value); }
		}
		
		public List <AircraftCharacteristic> AircraftCategory
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertyObstacleAssessmentArea.AircraftCategory); }
		}
		
		public List <Obstruction> SignificantObstacle
		{
			get { return GetObjectList <Obstruction> ((int) PropertyObstacleAssessmentArea.SignificantObstacle); }
		}
		
		public Surface Surface
		{
			get { return GetObject <Surface> ((int) PropertyObstacleAssessmentArea.Surface); }
			set { SetValue ((int) PropertyObstacleAssessmentArea.Surface, value); }
		}
		
		public Curve StartingCurve
		{
			get { return GetObject <Curve> ((int) PropertyObstacleAssessmentArea.StartingCurve); }
			set { SetValue ((int) PropertyObstacleAssessmentArea.StartingCurve, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyObstacleAssessmentArea
	{
		Type = PropertyAObject.NEXT_CLASS,
		SectionNumber,
		Slope,
		AssessedAltitude,
		SlopeLowerAltitude,
		GradientLowHigh,
		SurfaceZone,
		SafetyRegulation,
		AircraftCategory,
		SignificantObstacle,
		Surface,
		StartingCurve,
		NEXT_CLASS
	}
	
	public static class MetadataObstacleAssessmentArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataObstacleAssessmentArea ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyObstacleAssessmentArea.Type, (int) EnumType.CodeObstacleAssessmentSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.SectionNumber, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.Slope, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.AssessedAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.SlopeLowerAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.GradientLowHigh, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.SurfaceZone, (int) EnumType.CodeObstructionIdSurfaceZone, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.SafetyRegulation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.AircraftCategory, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.SignificantObstacle, (int) ObjectType.Obstruction, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.Surface, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAssessmentArea.StartingCurve, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
