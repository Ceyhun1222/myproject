using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ProcedureAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ProcedureAvailability; }
		}
		
		public CodeProcedureAvailability? Status
		{
			get { return GetNullableFieldValue <CodeProcedureAvailability> ((int) PropertyProcedureAvailability.Status); }
			set { SetNullableFieldValue <CodeProcedureAvailability> ((int) PropertyProcedureAvailability.Status, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyProcedureAvailability
	{
		Status = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataProcedureAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataProcedureAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyProcedureAvailability.Status, (int) EnumType.CodeProcedureAvailability, PropertyTypeCharacter.Nullable);
		}
	}
}
