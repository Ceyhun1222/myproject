using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractSegmentLegRef : AbstractFeatureRef <SegmentLegType>
	{
		public AbstractSegmentLegRef ()
		{
		}
		
		public AbstractSegmentLegRef (SegmentLegRefType segmentLegRef, FeatureRef feature)
		 : base (segmentLegRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractSegmentLegRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractSegmentLegRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractSegmentLegRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractSegmentLegRef.Feature); }
			set { SetValue ((int) PropertyAbstractSegmentLegRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractSegmentLegRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractSegmentLegRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractSegmentLegRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractSegmentLegRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractSegmentLegRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
