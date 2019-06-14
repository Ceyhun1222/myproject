using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ArrivalFeederLeg : ApproachLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ArrivalFeederLeg; }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyArrivalFeederLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyArrivalFeederLeg.RequiredNavigationPerformance, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyArrivalFeederLeg
	{
		RequiredNavigationPerformance = PropertyApproachLeg.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataArrivalFeederLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataArrivalFeederLeg ()
		{
			PropInfoList = MetadataApproachLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyArrivalFeederLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
