using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractNavaidEquipmentRef : AbstractFeatureRef <NavaidEquipmentType>
	{
		public AbstractNavaidEquipmentRef ()
		{
		}
		
		public AbstractNavaidEquipmentRef (NavaidEquipmentRefType navaidEquipmentRef, FeatureRef feature)
		 : base (navaidEquipmentRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractNavaidEquipmentRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractNavaidEquipmentRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractNavaidEquipmentRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractNavaidEquipmentRef.Feature); }
			set { SetValue ((int) PropertyAbstractNavaidEquipmentRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractNavaidEquipmentRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractNavaidEquipmentRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractNavaidEquipmentRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractNavaidEquipmentRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractNavaidEquipmentRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
