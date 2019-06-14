using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ArrivalLeg : SegmentLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ArrivalLeg; }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyArrivalLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyArrivalLeg.RequiredNavigationPerformance, value); }
		}
		
		public FeatureRef Arrival
		{
			get { return (FeatureRef ) GetValue ((int) PropertyArrivalLeg.Arrival); }
			set { SetValue ((int) PropertyArrivalLeg.Arrival, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyArrivalLeg
	{
		RequiredNavigationPerformance = PropertySegmentLeg.NEXT_CLASS,
		Arrival,
		NEXT_CLASS
	}
	
	public static class MetadataArrivalLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataArrivalLeg ()
		{
			PropInfoList = MetadataSegmentLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyArrivalLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrivalLeg.Arrival, (int) FeatureType.StandardInstrumentArrival, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
