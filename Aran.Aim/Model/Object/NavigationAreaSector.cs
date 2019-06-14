using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class NavigationAreaSector : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.NavigationAreaSector; }
		}
		
		public CircleSector SectorDefinition
		{
			get { return GetObject <CircleSector> ((int) PropertyNavigationAreaSector.SectorDefinition); }
			set { SetValue ((int) PropertyNavigationAreaSector.SectorDefinition, value); }
		}
		
		public List <Obstruction> SignificantObstacle
		{
			get { return GetObjectList <Obstruction> ((int) PropertyNavigationAreaSector.SignificantObstacle); }
		}
		
		public Surface Extent
		{
			get { return GetObject <Surface> ((int) PropertyNavigationAreaSector.Extent); }
			set { SetValue ((int) PropertyNavigationAreaSector.Extent, value); }
		}
		
		public List <SectorDesign> SectorCriteria
		{
			get { return GetObjectList <SectorDesign> ((int) PropertyNavigationAreaSector.SectorCriteria); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavigationAreaSector
	{
		SectorDefinition = PropertyAObject.NEXT_CLASS,
		SignificantObstacle,
		Extent,
		SectorCriteria,
		NEXT_CLASS
	}
	
	public static class MetadataNavigationAreaSector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavigationAreaSector ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavigationAreaSector.SectorDefinition, (int) ObjectType.CircleSector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaSector.SignificantObstacle, (int) ObjectType.Obstruction, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaSector.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaSector.SectorCriteria, (int) ObjectType.SectorDesign, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
