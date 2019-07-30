using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class CallsignDetail : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CallsignDetail; }
		}
		
		public string CallSign
		{
			get { return GetFieldValue <string> ((int) PropertyCallsignDetail.CallSign); }
			set { SetFieldValue <string> ((int) PropertyCallsignDetail.CallSign, value); }
		}
		
		public string Language
		{
			get { return GetFieldValue <string> ((int) PropertyCallsignDetail.Language); }
			set { SetFieldValue <string> ((int) PropertyCallsignDetail.Language, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCallsignDetail
	{
		CallSign = PropertyAObject.NEXT_CLASS,
		Language,
		NEXT_CLASS
	}
	
	public static class MetadataCallsignDetail
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCallsignDetail ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCallsignDetail.CallSign, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable, "callSign");
			PropInfoList.Add (PropertyCallsignDetail.Language, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
