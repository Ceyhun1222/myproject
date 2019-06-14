using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class WorkareaActivity : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.WorkareaActivity; }
		}
		
		public bool? IsActive
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyWorkareaActivity.IsActive); }
			set { SetNullableFieldValue <bool> ((int) PropertyWorkareaActivity.IsActive, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyWorkareaActivity
	{
		IsActive = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataWorkareaActivity
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataWorkareaActivity ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyWorkareaActivity.IsActive, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
