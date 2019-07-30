using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ObstaclePlacement : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ObstaclePlacement; }
		}
		
		public double? ObstacleBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyObstaclePlacement.ObstacleBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyObstaclePlacement.ObstacleBearing, value); }
		}
		
		public ValDistance ObstacleDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertyObstaclePlacement.ObstacleDistance); }
			set { SetValue ((int) PropertyObstaclePlacement.ObstacleDistance, value); }
		}
		
		public string PointType
		{
			get { return GetFieldValue <string> ((int) PropertyObstaclePlacement.PointType); }
			set { SetFieldValue <string> ((int) PropertyObstaclePlacement.PointType, value); }
		}
		
		public CodeSide? ObstaclePlacementP
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyObstaclePlacement.ObstaclePlacementP); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyObstaclePlacement.ObstaclePlacementP, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyObstaclePlacement
	{
		ObstacleBearing = PropertyAObject.NEXT_CLASS,
		ObstacleDistance,
		PointType,
		ObstaclePlacementP,
		NEXT_CLASS
	}
	
	public static class MetadataObstaclePlacement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataObstaclePlacement ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyObstaclePlacement.ObstacleBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstaclePlacement.ObstacleDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstaclePlacement.PointType, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstaclePlacement.ObstaclePlacementP, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
