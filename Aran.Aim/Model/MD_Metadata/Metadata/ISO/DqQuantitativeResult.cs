using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class DqQuantitativeResult : DqAbstractResult
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DqQuantitativeResult; }
		}
		
		public string ErrorStatistic
		{
			get { return GetFieldValue <string> ((int) PropertyDqQuantitativeResult.ErrorStatistic); }
			set { SetFieldValue <string> ((int) PropertyDqQuantitativeResult.ErrorStatistic, value); }
		}
		
		public List <BtString> Value
		{
			get { return GetObjectList <BtString> ((int) PropertyDqQuantitativeResult.Value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDqQuantitativeResult
	{
		ErrorStatistic = PropertyDqAbstractResult.NEXT_CLASS,
		Value,
		NEXT_CLASS
	}
	
	public static class MetadataDqQuantitativeResult
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDqQuantitativeResult ()
		{
			PropInfoList = MetadataDqAbstractResult.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDqQuantitativeResult.ErrorStatistic, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDqQuantitativeResult.Value, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
