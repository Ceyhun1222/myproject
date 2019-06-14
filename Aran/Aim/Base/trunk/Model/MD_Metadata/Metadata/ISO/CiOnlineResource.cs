using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class CiOnlineResource : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiOnlineResource; }
		}
		
		public string Linkage
		{
			get { return GetFieldValue <string> ((int) PropertyCiOnlineResource.Linkage); }
			set { SetFieldValue <string> ((int) PropertyCiOnlineResource.Linkage, value); }
		}
		
		public string Protocol
		{
			get { return GetFieldValue <string> ((int) PropertyCiOnlineResource.Protocol); }
			set { SetFieldValue <string> ((int) PropertyCiOnlineResource.Protocol, value); }
		}
		
		public string ApplicationProfile
		{
			get { return GetFieldValue <string> ((int) PropertyCiOnlineResource.ApplicationProfile); }
			set { SetFieldValue <string> ((int) PropertyCiOnlineResource.ApplicationProfile, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyCiOnlineResource.Name); }
			set { SetFieldValue <string> ((int) PropertyCiOnlineResource.Name, value); }
		}
		
		public string Description
		{
			get { return GetFieldValue <string> ((int) PropertyCiOnlineResource.Description); }
			set { SetFieldValue <string> ((int) PropertyCiOnlineResource.Description, value); }
		}
		
		public MdOnLineFunctionCode? Function
		{
			get { return GetNullableFieldValue <MdOnLineFunctionCode> ((int) PropertyCiOnlineResource.Function); }
			set { SetNullableFieldValue <MdOnLineFunctionCode> ((int) PropertyCiOnlineResource.Function, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiOnlineResource
	{
		Linkage = PropertyBtAbstractObject.NEXT_CLASS,
		Protocol,
		ApplicationProfile,
		Name,
		Description,
		Function,
		NEXT_CLASS
	}
	
	public static class MetadataCiOnlineResource
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiOnlineResource ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiOnlineResource.Linkage, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiOnlineResource.Protocol, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiOnlineResource.ApplicationProfile, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiOnlineResource.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiOnlineResource.Description, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiOnlineResource.Function, (int) EnumType.MdOnLineFunctionCode, PropertyTypeCharacter.Nullable);
		}
	}
}
