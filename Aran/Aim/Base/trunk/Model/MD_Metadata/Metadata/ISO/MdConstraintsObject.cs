using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdConstraintsObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdConstraintsObject; }
		}
		
		public MdConstraints Value
		{
			get { return (MdConstraints ) GetAbstractObject ((int) PropertyMdConstraintsObject.Value, AbstractType.MdConstraints); }
			set { SetValue ((int) PropertyMdConstraintsObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdConstraintsObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMdConstraintsObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdConstraintsObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdConstraintsObject.Value, (int) AbstractType.MdConstraints, PropertyTypeCharacter.Nullable);
		}
	}
}
