using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class VerticalStructureLightingStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.VerticalStructureLightingStatus; }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyVerticalStructureLightingStatus.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyVerticalStructureLightingStatus.Status, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVerticalStructureLightingStatus
	{
		Status = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataVerticalStructureLightingStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVerticalStructureLightingStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVerticalStructureLightingStatus.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
		}
	}
}
