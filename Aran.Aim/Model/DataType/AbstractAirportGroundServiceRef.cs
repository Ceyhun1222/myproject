using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractAirportGroundServiceRef : AbstractFeatureRef <AirportGroundServiceType>
	{
		public AbstractAirportGroundServiceRef ()
		{
		}
		
		public AbstractAirportGroundServiceRef (AirportGroundServiceRefType airportGroundServiceRef, FeatureRef feature)
		 : base (airportGroundServiceRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractAirportGroundServiceRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractAirportGroundServiceRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractAirportGroundServiceRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractAirportGroundServiceRef.Feature); }
			set { SetValue ((int) PropertyAbstractAirportGroundServiceRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractAirportGroundServiceRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractAirportGroundServiceRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractAirportGroundServiceRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractAirportGroundServiceRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractAirportGroundServiceRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
