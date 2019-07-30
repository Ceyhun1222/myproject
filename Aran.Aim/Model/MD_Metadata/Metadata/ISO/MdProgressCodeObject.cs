using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class MdProgressCodeObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdProgressCodeObject; }
		}
		
		public MdProgressCode? Value
		{
			get { return GetNullableFieldValue <MdProgressCode> ((int) PropertyMdProgressCodeObject.Value); }
			set { SetNullableFieldValue <MdProgressCode> ((int) PropertyMdProgressCodeObject.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdProgressCodeObject
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMdProgressCodeObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdProgressCodeObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdProgressCodeObject.Value, (int) EnumType.MdProgressCode, PropertyTypeCharacter.Nullable);
		}
	}
}
