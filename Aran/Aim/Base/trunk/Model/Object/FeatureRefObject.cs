using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class FeatureRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FeatureRefObject; }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyFeatureRefObject.Feature); }
			set { SetValue ((int) PropertyFeatureRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataFeatureRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFeatureRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFeatureRefObject.Feature, (int) DataType.FeatureRef);
		}
	}
}
