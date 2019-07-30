using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractApproachLegRef : AbstractFeatureRef <ApproachLegType>
	{
		public AbstractApproachLegRef ()
		{
		}
		
		public AbstractApproachLegRef (ApproachLegRefType approachLegRef, FeatureRef feature)
		 : base (approachLegRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractApproachLegRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractApproachLegRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractApproachLegRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractApproachLegRef.Feature); }
			set { SetValue ((int) PropertyAbstractApproachLegRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractApproachLegRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractApproachLegRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractApproachLegRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractApproachLegRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractApproachLegRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
