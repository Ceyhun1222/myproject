using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class DqAbstractResultObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DqAbstractResultObject; }
		}
		
		public DqAbstractResult Value
		{
			get { return (DqAbstractResult ) GetAbstractObject ((int) PropertyDqAbstractResultObject.Value, AbstractType.DqAbstractResult); }
			set { SetValue ((int) PropertyDqAbstractResultObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDqAbstractResultObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataDqAbstractResultObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDqAbstractResultObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDqAbstractResultObject.Value, (int) AbstractType.DqAbstractResult, PropertyTypeCharacter.Nullable);
		}
	}
}
