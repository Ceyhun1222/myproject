using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SurveillanceGroundStation : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SurveillanceGroundStation; }
		}
		
		public bool? VideoMap
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySurveillanceGroundStation.VideoMap); }
			set { SetNullableFieldValue <bool> ((int) PropertySurveillanceGroundStation.VideoMap, value); }
		}
		
		public FeatureRef TheUnit
		{
			get { return (FeatureRef ) GetValue ((int) PropertySurveillanceGroundStation.TheUnit); }
			set { SetValue ((int) PropertySurveillanceGroundStation.TheUnit, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurveillanceGroundStation
	{
		VideoMap = PropertyAObject.NEXT_CLASS,
		TheUnit,
		NEXT_CLASS
	}
	
	public static class MetadataSurveillanceGroundStation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurveillanceGroundStation ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurveillanceGroundStation.VideoMap, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceGroundStation.TheUnit, (int) FeatureType.Unit, PropertyTypeCharacter.Nullable);
		}
	}
}
