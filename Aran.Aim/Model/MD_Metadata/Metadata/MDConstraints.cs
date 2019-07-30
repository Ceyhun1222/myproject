using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Metadata
{
	public class MDConstraints : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MDConstraints; }
		}
		
		public string UseLimitation
		{
			get { return GetFieldValue <string> ((int) PropertyMDConstraints.UseLimitation); }
			set { SetFieldValue <string> ((int) PropertyMDConstraints.UseLimitation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMDConstraints
	{
		UseLimitation = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMDConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMDConstraints ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMDConstraints.UseLimitation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
