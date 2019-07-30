using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractGroundLightSystemRef : AbstractFeatureRef <GroundLightSystemType>
	{
		public AbstractGroundLightSystemRef ()
		{
		}
		
		public AbstractGroundLightSystemRef (GroundLightSystemRefType groundLightSystemRef, FeatureRef feature)
		 : base (groundLightSystemRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractGroundLightSystemRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractGroundLightSystemRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractGroundLightSystemRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractGroundLightSystemRef.Feature); }
			set { SetValue ((int) PropertyAbstractGroundLightSystemRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractGroundLightSystemRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractGroundLightSystemRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractGroundLightSystemRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractGroundLightSystemRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractGroundLightSystemRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
