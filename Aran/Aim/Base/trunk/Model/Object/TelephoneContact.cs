using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class TelephoneContact : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.TelephoneContact; }
		}
		
		public string Voice
		{
			get { return GetFieldValue <string> ((int) PropertyTelephoneContact.Voice); }
			set { SetFieldValue <string> ((int) PropertyTelephoneContact.Voice, value); }
		}
		
		public string Facsimile
		{
			get { return GetFieldValue <string> ((int) PropertyTelephoneContact.Facsimile); }
			set { SetFieldValue <string> ((int) PropertyTelephoneContact.Facsimile, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTelephoneContact
	{
		Voice = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Facsimile,
		NEXT_CLASS
	}
	
	public static class MetadataTelephoneContact
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTelephoneContact ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTelephoneContact.Voice, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTelephoneContact.Facsimile, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
