using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractNavigationSystemCheckpointRef : AbstractFeatureRef <NavigationSystemCheckpointType>
	{
		public AbstractNavigationSystemCheckpointRef ()
		{
		}
		
		public AbstractNavigationSystemCheckpointRef (NavigationSystemCheckpointRefType navigationSystemCheckpointRef, FeatureRef feature)
		 : base (navigationSystemCheckpointRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractNavigationSystemCheckpointRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractNavigationSystemCheckpointRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractNavigationSystemCheckpointRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractNavigationSystemCheckpointRef.Feature); }
			set { SetValue ((int) PropertyAbstractNavigationSystemCheckpointRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractNavigationSystemCheckpointRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractNavigationSystemCheckpointRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractNavigationSystemCheckpointRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractNavigationSystemCheckpointRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractNavigationSystemCheckpointRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
