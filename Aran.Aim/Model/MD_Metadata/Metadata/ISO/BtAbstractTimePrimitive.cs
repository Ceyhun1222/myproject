using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Metadata.ISO
{
	public abstract class BtAbstractTimePrimitive : AObject
	{
		public virtual BtAbstractTimePrimitiveType BtAbstractTimePrimitiveType 
		{
			get { return (BtAbstractTimePrimitiveType) ObjectType; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtAbstractTimePrimitive
	{
		NEXT_CLASS = PropertyAObject.NEXT_CLASS
	}
	
	public static class MetadataBtAbstractTimePrimitive
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtAbstractTimePrimitive ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
		}
	}
}
