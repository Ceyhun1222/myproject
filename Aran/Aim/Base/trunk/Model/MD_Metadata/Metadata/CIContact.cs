using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class CIContact : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.CIContact; }
		}
		
		public CIAddress Address
		{
			get { return (CIAddress ) GetValue ((int) PropertyCIContact.Address); }
			set { SetValue ((int) PropertyCIContact.Address, value); }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCIContact
	{
		Address,
		NEXT_CLASS
	}
	
	public static class MetadataCIContact
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCIContact ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCIContact.Address, (int) DataType.CIAddress, PropertyTypeCharacter.Nullable);
		}
	}
}
