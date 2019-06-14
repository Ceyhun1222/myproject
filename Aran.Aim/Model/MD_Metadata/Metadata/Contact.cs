using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class Contact : CIContact
	{
		public override DataType DataType
		{
			get { return DataType.Contact; }
		}
		
		public Telephone Phone
		{
			get { return (Telephone ) GetValue ((int) PropertyContact.Phone); }
			set { SetValue ((int) PropertyContact.Phone, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyContact
	{
		Phone = PropertyCIContact.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataContact
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataContact ()
		{
			PropInfoList = MetadataCIContact.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyContact.Phone, (int) DataType.Telephone, PropertyTypeCharacter.Nullable);
		}
	}
}
