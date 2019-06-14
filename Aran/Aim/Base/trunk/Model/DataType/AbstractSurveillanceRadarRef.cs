using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractSurveillanceRadarRef : AbstractFeatureRef <SurveillanceRadarType>
	{
		public AbstractSurveillanceRadarRef ()
		{
		}
		
		public AbstractSurveillanceRadarRef (SurveillanceRadarRefType surveillanceRadarRef, FeatureRef feature)
		 : base (surveillanceRadarRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractSurveillanceRadarRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractSurveillanceRadarRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractSurveillanceRadarRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractSurveillanceRadarRef.Feature); }
			set { SetValue ((int) PropertyAbstractSurveillanceRadarRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractSurveillanceRadarRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractSurveillanceRadarRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractSurveillanceRadarRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractSurveillanceRadarRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractSurveillanceRadarRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
