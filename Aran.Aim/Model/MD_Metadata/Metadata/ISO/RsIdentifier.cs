using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class RsIdentifier : MdIdentifier
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RsIdentifier; }
		}
		
		public string CodeSpace
		{
			get { return GetFieldValue <string> ((int) PropertyRsIdentifier.CodeSpace); }
			set { SetFieldValue <string> ((int) PropertyRsIdentifier.CodeSpace, value); }
		}
		
		public string Version
		{
			get { return GetFieldValue <string> ((int) PropertyRsIdentifier.Version); }
			set { SetFieldValue <string> ((int) PropertyRsIdentifier.Version, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRsIdentifier
	{
		CodeSpace = PropertyMdIdentifier.NEXT_CLASS,
		Version,
		NEXT_CLASS
	}
	
	public static class MetadataRsIdentifier
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRsIdentifier ()
		{
			PropInfoList = MetadataMdIdentifier.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRsIdentifier.CodeSpace, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRsIdentifier.Version, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
