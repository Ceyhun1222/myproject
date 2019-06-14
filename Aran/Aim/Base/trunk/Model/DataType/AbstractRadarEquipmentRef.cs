using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractRadarEquipmentRef : AbstractFeatureRef <RadarEquipmentType>
	{
		public AbstractRadarEquipmentRef ()
		{
		}
		
		public AbstractRadarEquipmentRef (RadarEquipmentRefType radarEquipmentRef, FeatureRef feature)
		 : base (radarEquipmentRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractRadarEquipmentRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractRadarEquipmentRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractRadarEquipmentRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractRadarEquipmentRef.Feature); }
			set { SetValue ((int) PropertyAbstractRadarEquipmentRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractRadarEquipmentRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractRadarEquipmentRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractRadarEquipmentRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractRadarEquipmentRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractRadarEquipmentRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
