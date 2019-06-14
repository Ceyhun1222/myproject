using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public abstract class DqAbstractResult : BtAbstractObject
	{
		public virtual DqAbstractResultType DqAbstractResultType 
		{
			get { return (DqAbstractResultType) ObjectType; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDqAbstractResult
	{
		NEXT_CLASS = PropertyBtAbstractObject.NEXT_CLASS
    }
	
	public static class MetadataDqAbstractResult
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDqAbstractResult ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
		}
	}
}
