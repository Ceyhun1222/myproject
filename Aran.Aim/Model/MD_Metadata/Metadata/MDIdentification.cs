using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Metadata
{
	public abstract class MDIdentification : AObject
	{
		public virtual MDIdentificationType MDIdentificationType 
		{
			get { return (MDIdentificationType) ObjectType; }
		}
		
		public string Abstract
		{
			get { return GetFieldValue <string> ((int) PropertyMDIdentification.Abstract); }
			set { SetFieldValue <string> ((int) PropertyMDIdentification.Abstract, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMDIdentification
	{
		Abstract = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataMDIdentification
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMDIdentification ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMDIdentification.Abstract, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
