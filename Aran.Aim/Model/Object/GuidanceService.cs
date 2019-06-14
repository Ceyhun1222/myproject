using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Model.Attribute;

namespace Aran.Aim.Features
{
	public class GuidanceService : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.GuidanceService; }
		}
		
		public GuidanceServiceChoice Choice
		{
			get { return (GuidanceServiceChoice) RefType; }
		}

        [LinkedFeature(FeatureType.Navaid)]
		public FeatureRef Navaid
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) GuidanceServiceChoice.Navaid;
			}
		}

        [LinkedFeature(FeatureType.SpecialNavigationSystem)]
		public FeatureRef SpecialNavigationSystem
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) GuidanceServiceChoice.SpecialNavigationSystem;
			}
		}

        [LinkedFeature(FeatureType.RadarSystem)]
		public FeatureRef Radar
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) GuidanceServiceChoice.RadarSystem;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGuidanceService
	{
		Navaid = PropertyAObject.NEXT_CLASS,
		SpecialNavigationSystem,
		Radar,
		NEXT_CLASS
	}
	
	public static class MetadataGuidanceService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGuidanceService ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGuidanceService.Navaid, (int) FeatureType.Navaid, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceService.SpecialNavigationSystem, (int) FeatureType.SpecialNavigationSystem, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGuidanceService.Radar, (int) FeatureType.RadarSystem, PropertyTypeCharacter.Nullable);
		}
	}
}
