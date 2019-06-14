using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AerialRefuellingPoint : SegmentPoint
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AerialRefuellingPoint; }
		}
		
		public uint? Sequence
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAerialRefuellingPoint.Sequence); }
			set { SetNullableFieldValue <uint> ((int) PropertyAerialRefuellingPoint.Sequence, value); }
		}
		
		public CodeAerialRefuellingPoint? UsageType
		{
			get { return GetNullableFieldValue <CodeAerialRefuellingPoint> ((int) PropertyAerialRefuellingPoint.UsageType); }
			set { SetNullableFieldValue <CodeAerialRefuellingPoint> ((int) PropertyAerialRefuellingPoint.UsageType, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAerialRefuellingPoint
	{
		Sequence = PropertySegmentPoint.NEXT_CLASS,
		UsageType,
		NEXT_CLASS
	}
	
	public static class MetadataAerialRefuellingPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAerialRefuellingPoint ()
		{
			PropInfoList = MetadataSegmentPoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAerialRefuellingPoint.Sequence, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingPoint.UsageType, (int) EnumType.CodeAerialRefuellingPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
