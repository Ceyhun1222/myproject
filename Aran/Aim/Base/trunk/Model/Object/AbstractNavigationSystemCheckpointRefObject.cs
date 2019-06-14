using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractNavigationSystemCheckpointRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractNavigationSystemCheckpointRefObject; }
		}
		
		public AbstractNavigationSystemCheckpointRef Feature
		{
			get { return (AbstractNavigationSystemCheckpointRef ) GetValue ((int) PropertyAbstractNavigationSystemCheckpointRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractNavigationSystemCheckpointRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractNavigationSystemCheckpointRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractNavigationSystemCheckpointRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractNavigationSystemCheckpointRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractNavigationSystemCheckpointRefObject.Feature, (int) DataType.AbstractNavigationSystemCheckpointRef);
		}
	}
}
