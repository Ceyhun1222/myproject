using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class InitialLeg : ApproachLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.InitialLeg; }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyInitialLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyInitialLeg.RequiredNavigationPerformance, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyInitialLeg
	{
		RequiredNavigationPerformance = PropertyApproachLeg.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataInitialLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataInitialLeg ()
		{
			PropInfoList = MetadataApproachLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyInitialLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
