using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractTrafficSeparationServiceRef : AbstractFeatureRef <TrafficSeparationServiceType>
	{
		public AbstractTrafficSeparationServiceRef ()
		{
		}
		
		public AbstractTrafficSeparationServiceRef (TrafficSeparationServiceRefType trafficSeparationServiceRef, FeatureRef feature)
		 : base (trafficSeparationServiceRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractTrafficSeparationServiceRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractTrafficSeparationServiceRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractTrafficSeparationServiceRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractTrafficSeparationServiceRef.Feature); }
			set { SetValue ((int) PropertyAbstractTrafficSeparationServiceRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractTrafficSeparationServiceRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractTrafficSeparationServiceRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractTrafficSeparationServiceRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractTrafficSeparationServiceRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractTrafficSeparationServiceRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
