using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractAirportHeliportProtectionAreaRef : AbstractFeatureRef <AirportHeliportProtectionAreaType>
	{
		public AbstractAirportHeliportProtectionAreaRef ()
		{
		}
		
		public AbstractAirportHeliportProtectionAreaRef (AirportHeliportProtectionAreaRefType airportHeliportProtectionAreaRef, FeatureRef feature)
		 : base (airportHeliportProtectionAreaRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractAirportHeliportProtectionAreaRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractAirportHeliportProtectionAreaRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractAirportHeliportProtectionAreaRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractAirportHeliportProtectionAreaRef.Feature); }
			set { SetValue ((int) PropertyAbstractAirportHeliportProtectionAreaRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractAirportHeliportProtectionAreaRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractAirportHeliportProtectionAreaRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractAirportHeliportProtectionAreaRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractAirportHeliportProtectionAreaRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractAirportHeliportProtectionAreaRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
