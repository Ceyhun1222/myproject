using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractTrafficSeparationServiceRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractTrafficSeparationServiceRefObject; }
		}
		
		public AbstractTrafficSeparationServiceRef Feature
		{
			get { return (AbstractTrafficSeparationServiceRef ) GetValue ((int) PropertyAbstractTrafficSeparationServiceRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractTrafficSeparationServiceRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractTrafficSeparationServiceRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractTrafficSeparationServiceRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractTrafficSeparationServiceRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractTrafficSeparationServiceRefObject.Feature, (int) DataType.AbstractTrafficSeparationServiceRef);
		}
	}
}
