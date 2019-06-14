using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AltimeterSourceStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AltimeterSourceStatus; }
		}
		
		public CodeStatusOperations? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyAltimeterSourceStatus.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyAltimeterSourceStatus.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAltimeterSourceStatus
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAltimeterSourceStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAltimeterSourceStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAltimeterSourceStatus.OperationalStatus, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
		}
	}
}
