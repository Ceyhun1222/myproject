using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractMarkingRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractMarkingRefObject; }
		}
		
		public AbstractMarkingRef Feature
		{
			get { return (AbstractMarkingRef ) GetValue ((int) PropertyAbstractMarkingRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractMarkingRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractMarkingRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractMarkingRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractMarkingRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractMarkingRefObject.Feature, (int) DataType.AbstractMarkingRef);
		}
	}
}
