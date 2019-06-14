using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class OnlineContact : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.OnlineContact; }
		}
		
		public CodeTelecomNetwork? Network
		{
			get { return GetNullableFieldValue <CodeTelecomNetwork> ((int) PropertyOnlineContact.Network); }
			set { SetNullableFieldValue <CodeTelecomNetwork> ((int) PropertyOnlineContact.Network, value); }
		}
		
		public string Linkage
		{
			get { return GetFieldValue <string> ((int) PropertyOnlineContact.Linkage); }
			set { SetFieldValue <string> ((int) PropertyOnlineContact.Linkage, value); }
		}
		
		public string Protocol
		{
			get { return GetFieldValue <string> ((int) PropertyOnlineContact.Protocol); }
			set { SetFieldValue <string> ((int) PropertyOnlineContact.Protocol, value); }
		}
		
		public string eMail
		{
			get { return GetFieldValue <string> ((int) PropertyOnlineContact.eMail); }
			set { SetFieldValue <string> ((int) PropertyOnlineContact.eMail, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyOnlineContact
	{
		Network = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Linkage,
		Protocol,
		eMail,
		NEXT_CLASS
	}
	
	public static class MetadataOnlineContact
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataOnlineContact ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyOnlineContact.Network, (int) EnumType.CodeTelecomNetwork, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOnlineContact.Linkage, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOnlineContact.Protocol, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOnlineContact.eMail, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
