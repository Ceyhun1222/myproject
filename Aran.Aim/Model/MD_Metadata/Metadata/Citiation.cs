using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class Citiation : CICitation
	{
		public override DataType DataType
		{
			get { return DataType.Citiation; }
		}
		
		public string processCertification
		{
			get { return GetFieldValue <string> ((int) PropertyCitiation.processCertification); }
			set { SetFieldValue <string> ((int) PropertyCitiation.processCertification, value); }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCitiation
	{
		processCertification = PropertyCICitation.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataCitiation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCitiation ()
		{
			PropInfoList = MetadataCICitation.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCitiation.processCertification, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
