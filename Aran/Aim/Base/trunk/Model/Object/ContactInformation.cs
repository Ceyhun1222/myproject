using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ContactInformation : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ContactInformation; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyContactInformation.Name); }
			set { SetFieldValue <string> ((int) PropertyContactInformation.Name, value); }
		}
		
		public string Title
		{
			get { return GetFieldValue <string> ((int) PropertyContactInformation.Title); }
			set { SetFieldValue <string> ((int) PropertyContactInformation.Title, value); }
		}
		
		public List <PostalAddress> Address
		{
			get { return GetObjectList <PostalAddress> ((int) PropertyContactInformation.Address); }
		}
		
		public List <OnlineContact> NetworkNode
		{
			get { return GetObjectList <OnlineContact> ((int) PropertyContactInformation.NetworkNode); }
		}
		
		public List <TelephoneContact> PhoneFax
		{
			get { return GetObjectList <TelephoneContact> ((int) PropertyContactInformation.PhoneFax); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyContactInformation
	{
		Name = PropertyAObject.NEXT_CLASS,
		Title,
		Address,
		NetworkNode,
		PhoneFax,
		NEXT_CLASS
	}
	
	public static class MetadataContactInformation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataContactInformation ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyContactInformation.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyContactInformation.Title, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyContactInformation.Address, (int) ObjectType.PostalAddress, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyContactInformation.NetworkNode, (int) ObjectType.OnlineContact, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyContactInformation.PhoneFax, (int) ObjectType.TelephoneContact, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
