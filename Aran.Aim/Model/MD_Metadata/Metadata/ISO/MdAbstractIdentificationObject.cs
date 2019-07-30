using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdAbstractIdentificationObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdAbstractIdentificationObject; }
		}
		
		public MdAbstractIdentification Value
		{
			get { return (MdAbstractIdentification ) GetAbstractObject ((int) PropertyMdAbstractIdentificationObject.Value, AbstractType.MdAbstractIdentification); }
			set { SetValue ((int) PropertyMdAbstractIdentificationObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdAbstractIdentificationObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMdAbstractIdentificationObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdAbstractIdentificationObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdAbstractIdentificationObject.Value, (int) AbstractType.MdAbstractIdentification, PropertyTypeCharacter.Nullable);
		}
	}
}
