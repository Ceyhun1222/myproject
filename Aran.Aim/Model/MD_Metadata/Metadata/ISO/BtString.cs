using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Metadata.ISO
{
	public class BtString : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.BtString; }
		}
		
		public string Value
		{
			get { return GetFieldValue <string> ((int) PropertyBtString.Value); }
			set { SetFieldValue <string> ((int) PropertyBtString.Value, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtString
	{
		Value = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataBtString
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtString ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyBtString.Value, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
