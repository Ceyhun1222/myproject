using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractServiceRef : AbstractFeatureRef <ServiceType>
	{
		public AbstractServiceRef ()
		{
		}
		
		public AbstractServiceRef (ServiceRefType serviceRef, FeatureRef feature)
		 : base (serviceRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractServiceRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractServiceRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractServiceRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractServiceRef.Feature); }
			set { SetValue ((int) PropertyAbstractServiceRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractServiceRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractServiceRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractServiceRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractServiceRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractServiceRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
