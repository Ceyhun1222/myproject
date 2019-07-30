using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class IntermediateLeg : ApproachLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.IntermediateLeg; }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyIntermediateLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyIntermediateLeg.RequiredNavigationPerformance, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyIntermediateLeg
	{
		RequiredNavigationPerformance = PropertyApproachLeg.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataIntermediateLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataIntermediateLeg ()
		{
			PropInfoList = MetadataApproachLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyIntermediateLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
