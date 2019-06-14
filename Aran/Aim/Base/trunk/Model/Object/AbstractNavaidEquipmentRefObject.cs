using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Objects
{
	public class AbstractNavaidEquipmentRefObject : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AbstractNavaidEquipmentRefObject; }
		}
		
		public AbstractNavaidEquipmentRef Feature
		{
			get { return (AbstractNavaidEquipmentRef ) GetValue ((int) PropertyAbstractNavaidEquipmentRefObject.Feature); }
			set { SetValue ((int) PropertyAbstractNavaidEquipmentRefObject.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractNavaidEquipmentRefObject
	{
		Feature = PropertyAObject.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractNavaidEquipmentRefObject
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractNavaidEquipmentRefObject ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractNavaidEquipmentRefObject.Feature, (int) DataType.AbstractNavaidEquipmentRef);
		}
	}
}
