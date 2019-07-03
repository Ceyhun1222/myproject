using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class LightElementStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LightElementStatus; }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyLightElementStatus.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyLightElementStatus.Status, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLightElementStatus
	{
		Status = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataLightElementStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLightElementStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLightElementStatus.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
		}
	}
}
