using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class NavaidEquipmentDistance : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.NavaidEquipmentDistance; }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyNavaidEquipmentDistance.Distance); }
			set { SetValue ((int) PropertyNavaidEquipmentDistance.Distance, value); }
		}
		
		public ValDistance DistanceAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyNavaidEquipmentDistance.DistanceAccuracy); }
			set { SetValue ((int) PropertyNavaidEquipmentDistance.DistanceAccuracy, value); }
		}
		
		public AbstractNavaidEquipmentRef TheNavaidEquipment
		{
			get { return (AbstractNavaidEquipmentRef ) GetValue ((int) PropertyNavaidEquipmentDistance.TheNavaidEquipment); }
			set { SetValue ((int) PropertyNavaidEquipmentDistance.TheNavaidEquipment, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaidEquipmentDistance
	{
		Distance = PropertyAObject.NEXT_CLASS,
		DistanceAccuracy,
		TheNavaidEquipment,
		NEXT_CLASS
	}
	
	public static class MetadataNavaidEquipmentDistance
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaidEquipmentDistance ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaidEquipmentDistance.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipmentDistance.DistanceAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipmentDistance.TheNavaidEquipment, (int) DataType.AbstractNavaidEquipmentRef, PropertyTypeCharacter.Nullable);
		}
	}
}
