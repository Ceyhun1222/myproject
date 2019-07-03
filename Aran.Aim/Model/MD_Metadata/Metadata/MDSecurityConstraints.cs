using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class MDSecurityConstraints : MDConstraints
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MDSecurityConstraints; }
		}
		
		public string ClassificationSystem
		{
			get { return GetFieldValue <string> ((int) PropertyMDSecurityConstraints.ClassificationSystem); }
			set { SetFieldValue <string> ((int) PropertyMDSecurityConstraints.ClassificationSystem, value); }
		}
		
		public string HandlingDescription
		{
			get { return GetFieldValue <string> ((int) PropertyMDSecurityConstraints.HandlingDescription); }
			set { SetFieldValue <string> ((int) PropertyMDSecurityConstraints.HandlingDescription, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMDSecurityConstraints
	{
		ClassificationSystem = PropertyMDConstraints.NEXT_CLASS,
		HandlingDescription,
		NEXT_CLASS
	}
	
	public static class MetadataMDSecurityConstraints
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMDSecurityConstraints ()
		{
			PropInfoList = MetadataMDConstraints.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMDSecurityConstraints.ClassificationSystem, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMDSecurityConstraints.HandlingDescription, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
