using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class SafeAltitudeAreaSector : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SafeAltitudeAreaSector; }
		}
		
		public ValDistance BufferWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertySafeAltitudeAreaSector.BufferWidth); }
			set { SetValue ((int) PropertySafeAltitudeAreaSector.BufferWidth, value); }
		}
		
		public Surface Extent
		{
			get { return GetObject <Surface> ((int) PropertySafeAltitudeAreaSector.Extent); }
			set { SetValue ((int) PropertySafeAltitudeAreaSector.Extent, value); }
		}
		
		public List <Obstruction> SignificantObstacle
		{
			get { return GetObjectList <Obstruction> ((int) PropertySafeAltitudeAreaSector.SignificantObstacle); }
		}
		
		public CircleSector SectorDefinition
		{
			get { return GetObject <CircleSector> ((int) PropertySafeAltitudeAreaSector.SectorDefinition); }
			set { SetValue ((int) PropertySafeAltitudeAreaSector.SectorDefinition, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySafeAltitudeAreaSector
	{
		BufferWidth = PropertyAObject.NEXT_CLASS,
		Extent,
		SignificantObstacle,
		SectorDefinition,
		NEXT_CLASS
	}
	
	public static class MetadataSafeAltitudeAreaSector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSafeAltitudeAreaSector ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySafeAltitudeAreaSector.BufferWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeAreaSector.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeAreaSector.SignificantObstacle, (int) ObjectType.Obstruction, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeAreaSector.SectorDefinition, (int) ObjectType.CircleSector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
