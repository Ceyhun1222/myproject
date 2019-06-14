using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RadarComponent : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RadarComponent; }
		}
		
		public uint? CollocationGroup
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyRadarComponent.CollocationGroup); }
			set { SetNullableFieldValue <uint> ((int) PropertyRadarComponent.CollocationGroup, value); }
		}
		
		public AbstractRadarEquipmentRef TheRadarEquipment
		{
			get { return (AbstractRadarEquipmentRef ) GetValue ((int) PropertyRadarComponent.TheRadarEquipment); }
			set { SetValue ((int) PropertyRadarComponent.TheRadarEquipment, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadarComponent
	{
		CollocationGroup = PropertyAObject.NEXT_CLASS,
		TheRadarEquipment,
		NEXT_CLASS
	}
	
	public static class MetadataRadarComponent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadarComponent ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadarComponent.CollocationGroup, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarComponent.TheRadarEquipment, (int) DataType.AbstractRadarEquipmentRef, PropertyTypeCharacter.Nullable);
		}
	}
}
