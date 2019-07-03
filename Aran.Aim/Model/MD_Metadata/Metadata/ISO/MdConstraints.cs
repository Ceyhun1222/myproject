using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public abstract class MdConstraints : BtAbstractObject
	{
		public virtual MdConstraintsType MdConstraintsType 
		{
			get { return (MdConstraintsType) ObjectType; }
		}
		
		public List <BtString> UseLimitation
		{
			get { return GetObjectList <BtString> ((int) PropertyMdConstraints.UseLimitation); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdConstraints
	{
		UseLimitation = PropertyBtAbstractObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMdConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdConstraints ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdConstraints.UseLimitation, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
