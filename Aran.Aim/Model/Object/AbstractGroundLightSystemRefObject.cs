using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractGroundLightSystemRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractGroundLightSystemRefObject; }
		}
		
		public AbstractGroundLightSystemRef Feature
		{
			get { return (AbstractGroundLightSystemRef ) GetValue ((int) PropertyAbstractGroundLightSystemRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractGroundLightSystemRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractGroundLightSystemRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractGroundLightSystemRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractGroundLightSystemRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractGroundLightSystemRefObject.Feature, (int) DataType.AbstractGroundLightSystemRef);
		}
	}
}
