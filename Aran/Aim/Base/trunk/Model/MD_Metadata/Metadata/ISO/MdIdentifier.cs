using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class MdIdentifier : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MdIdentifier; }
		}
		
		public CiCitation Authority
		{
			get { return GetObject <CiCitation> ((int) PropertyMdIdentifier.Authority); }
			set { SetValue ((int) PropertyMdIdentifier.Authority, value); }
		}
		
		public string Code
		{
			get { return GetFieldValue <string> ((int) PropertyMdIdentifier.Code); }
			set { SetFieldValue <string> ((int) PropertyMdIdentifier.Code, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMdIdentifier
	{
		Authority = PropertyBtAbstractObject.NEXT_CLASS,
		Code,
		NEXT_CLASS
	}
	
	public static class MetadataMdIdentifier
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMdIdentifier ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMdIdentifier.Authority, (int) ObjectType.CiCitation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMdIdentifier.Code, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
