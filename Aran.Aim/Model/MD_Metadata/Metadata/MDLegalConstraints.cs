using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class MDLegalConstraints : MDConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MDLegalConstraints; }
		}
		
		public string OtherConstraints
		{
			get { return GetFieldValue <string> ((int) PropertyMDLegalConstraints.OtherConstraints); }
			set { SetFieldValue <string> ((int) PropertyMDLegalConstraints.OtherConstraints, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMDLegalConstraints
	{
		OtherConstraints = PropertyMDConstraints.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMDLegalConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMDLegalConstraints ()
		{
			PropInfoList = MetadataMDConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMDLegalConstraints.OtherConstraints, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
