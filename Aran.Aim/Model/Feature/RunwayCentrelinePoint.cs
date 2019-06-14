using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayCentrelinePoint : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayCentrelinePoint; }
		}
		
		public CodeRunwayPointRole? Role
		{
			get { return GetNullableFieldValue <CodeRunwayPointRole> ((int) PropertyRunwayCentrelinePoint.Role); }
			set { SetNullableFieldValue <CodeRunwayPointRole> ((int) PropertyRunwayCentrelinePoint.Role, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyRunwayCentrelinePoint.Designator); }
			set { SetFieldValue <string> ((int) PropertyRunwayCentrelinePoint.Designator, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyRunwayCentrelinePoint.Location); }
			set { SetValue ((int) PropertyRunwayCentrelinePoint.Location, value); }
		}
		
		public FeatureRef OnRunway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayCentrelinePoint.OnRunway); }
			set { SetValue ((int) PropertyRunwayCentrelinePoint.OnRunway, value); }
		}
		
		public List <RunwayDeclaredDistance> AssociatedDeclaredDistance
		{
			get { return GetObjectList <RunwayDeclaredDistance> ((int) PropertyRunwayCentrelinePoint.AssociatedDeclaredDistance); }
		}
		
		public List <NavaidEquipmentDistance> NavaidEquipment
		{
			get { return GetObjectList <NavaidEquipmentDistance> ((int) PropertyRunwayCentrelinePoint.NavaidEquipment); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayCentrelinePoint
	{
		Role = PropertyFeature.NEXT_CLASS,
		Designator,
		Location,
		OnRunway,
		AssociatedDeclaredDistance,
		NavaidEquipment,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayCentrelinePoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayCentrelinePoint ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayCentrelinePoint.Role, (int) EnumType.CodeRunwayPointRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayCentrelinePoint.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayCentrelinePoint.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayCentrelinePoint.OnRunway, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayCentrelinePoint.AssociatedDeclaredDistance, (int) ObjectType.RunwayDeclaredDistance, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayCentrelinePoint.NavaidEquipment, (int) ObjectType.NavaidEquipmentDistance, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
