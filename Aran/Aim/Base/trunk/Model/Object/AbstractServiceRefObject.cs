using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractServiceRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractServiceRefObject; }
		}
		
		public AbstractServiceRef Feature
		{
			get { return (AbstractServiceRef ) GetValue ((int) PropertyAbstractServiceRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractServiceRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractServiceRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractServiceRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractServiceRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractServiceRefObject.Feature, (int) DataType.AbstractServiceRef);
		}
	}
}
