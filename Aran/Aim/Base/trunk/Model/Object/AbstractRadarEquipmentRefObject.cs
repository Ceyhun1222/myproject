using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractRadarEquipmentRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractRadarEquipmentRefObject; }
		}
		
		public AbstractRadarEquipmentRef Feature
		{
			get { return (AbstractRadarEquipmentRef ) GetValue ((int) PropertyAbstractRadarEquipmentRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractRadarEquipmentRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractRadarEquipmentRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractRadarEquipmentRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractRadarEquipmentRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractRadarEquipmentRefObject.Feature, (int) DataType.AbstractRadarEquipmentRef);
		}
	}
}
