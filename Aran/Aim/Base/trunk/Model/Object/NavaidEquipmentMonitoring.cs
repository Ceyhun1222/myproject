using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class NavaidEquipmentMonitoring : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.NavaidEquipmentMonitoring; }
		}
		
		public bool? Monitored
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyNavaidEquipmentMonitoring.Monitored); }
			set { SetNullableFieldValue <bool> ((int) PropertyNavaidEquipmentMonitoring.Monitored, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaidEquipmentMonitoring
	{
		Monitored = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataNavaidEquipmentMonitoring
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaidEquipmentMonitoring ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaidEquipmentMonitoring.Monitored, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
