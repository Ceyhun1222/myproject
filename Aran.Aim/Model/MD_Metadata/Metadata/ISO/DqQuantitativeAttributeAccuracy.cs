using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class DqQuantitativeAttributeAccuracy : DqAbstractElement
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DqQuantitativeAttributeAccuracy; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDqQuantitativeAttributeAccuracy
	{
		NEXT_CLASS = PropertyDqAbstractElement.NEXT_CLASS
	}
	
	public static class MetadataDqQuantitativeAttributeAccuracy
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDqQuantitativeAttributeAccuracy ()
		{
			PropInfoList = MetadataDqAbstractElement.PropInfoList.Clone ();
			
		}
	}
}
