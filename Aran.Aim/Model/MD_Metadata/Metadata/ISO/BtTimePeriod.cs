using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class BtTimePeriod : BtAbstractTimePrimitive
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.BtTimePeriod; }
		}
		
		public BtTimePosition BeginPosition
		{
			get { return GetObject <BtTimePosition> ((int) PropertyBtTimePeriod.BeginPosition); }
			set { SetValue ((int) PropertyBtTimePeriod.BeginPosition, value); }
		}
		
		public BtTimePosition EndPosition
		{
			get { return GetObject <BtTimePosition> ((int) PropertyBtTimePeriod.EndPosition); }
			set { SetValue ((int) PropertyBtTimePeriod.EndPosition, value); }
		}
		
		public string Duration
		{
			get { return GetFieldValue <string> ((int) PropertyBtTimePeriod.Duration); }
			set { SetFieldValue <string> ((int) PropertyBtTimePeriod.Duration, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtTimePeriod
	{
		BeginPosition = PropertyBtAbstractTimePrimitive.NEXT_CLASS,
		EndPosition,
		Duration,
		NEXT_CLASS
	}
	
	public static class MetadataBtTimePeriod
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtTimePeriod ()
		{
			PropInfoList = MetadataBtAbstractTimePrimitive.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyBtTimePeriod.BeginPosition, (int) ObjectType.BtTimePosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtTimePeriod.EndPosition, (int) ObjectType.BtTimePosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtTimePeriod.Duration, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
