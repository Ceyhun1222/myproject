using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class ExTemporalExtent : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ExTemporalExtent; }
		}
		
		public BtAbstractTimePrimitive Extent
		{
			get { return (BtAbstractTimePrimitive ) GetAbstractObject ((int) PropertyExTemporalExtent.Extent, AbstractType.BtAbstractTimePrimitive); }
			set { SetValue ((int) PropertyExTemporalExtent.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyExTemporalExtent
	{
		Extent = PropertyBtAbstractObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataExTemporalExtent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataExTemporalExtent ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyExTemporalExtent.Extent, (int) AbstractType.BtAbstractTimePrimitive, PropertyTypeCharacter.Nullable);
		}
	}
}
