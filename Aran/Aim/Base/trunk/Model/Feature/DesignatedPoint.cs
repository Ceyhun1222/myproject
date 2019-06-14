using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class DesignatedPoint : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DesignatedPoint; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyDesignatedPoint.Designator); }
			set { SetFieldValue <string> ((int) PropertyDesignatedPoint.Designator, value); }
		}
		
		public CodeDesignatedPoint? Type
		{
			get { return GetNullableFieldValue <CodeDesignatedPoint> ((int) PropertyDesignatedPoint.Type); }
			set { SetNullableFieldValue <CodeDesignatedPoint> ((int) PropertyDesignatedPoint.Type, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyDesignatedPoint.Name); }
			set { SetFieldValue <string> ((int) PropertyDesignatedPoint.Name, value); }
		}
		
		public AixmPoint Location
		{
			get { return GetObject <AixmPoint> ((int) PropertyDesignatedPoint.Location); }
			set { SetValue ((int) PropertyDesignatedPoint.Location, value); }
		}
		
		public FeatureRef AimingPoint
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDesignatedPoint.AimingPoint); }
			set { SetValue ((int) PropertyDesignatedPoint.AimingPoint, value); }
		}
		
		public FeatureRef AirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDesignatedPoint.AirportHeliport); }
			set { SetValue ((int) PropertyDesignatedPoint.AirportHeliport, value); }
		}
		
		public FeatureRef RunwayPoint
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDesignatedPoint.RunwayPoint); }
			set { SetValue ((int) PropertyDesignatedPoint.RunwayPoint, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDesignatedPoint
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		Name,
		Location,
		AimingPoint,
		AirportHeliport,
		RunwayPoint,
		NEXT_CLASS
	}
	
	public static class MetadataDesignatedPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDesignatedPoint ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDesignatedPoint.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.Type, (int) EnumType.CodeDesignatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.Location, (int) ObjectType.AixmPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.AimingPoint, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.AirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDesignatedPoint.RunwayPoint, (int) FeatureType.RunwayCentrelinePoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
