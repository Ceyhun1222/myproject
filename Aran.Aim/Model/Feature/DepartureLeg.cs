using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class DepartureLeg : SegmentLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DepartureLeg; }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyDepartureLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyDepartureLeg.RequiredNavigationPerformance, value); }
		}
		
		public ValDistanceVertical MinimumObstacleClearanceAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyDepartureLeg.MinimumObstacleClearanceAltitude); }
			set { SetValue ((int) PropertyDepartureLeg.MinimumObstacleClearanceAltitude, value); }
		}
		
		public FeatureRef Departure
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDepartureLeg.Departure); }
			set { SetValue ((int) PropertyDepartureLeg.Departure, value); }
		}
		
		public List <DepartureArrivalCondition> Condition
		{
			get { return GetObjectList <DepartureArrivalCondition> ((int) PropertyDepartureLeg.Condition); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDepartureLeg
	{
		RequiredNavigationPerformance = PropertySegmentLeg.NEXT_CLASS,
		MinimumObstacleClearanceAltitude,
		Departure,
		Condition,
		NEXT_CLASS
	}
	
	public static class MetadataDepartureLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDepartureLeg ()
		{
			PropInfoList = MetadataSegmentLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDepartureLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureLeg.MinimumObstacleClearanceAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureLeg.Departure, (int) FeatureType.StandardInstrumentDeparture, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDepartureLeg.Condition, (int) ObjectType.DepartureArrivalCondition, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
