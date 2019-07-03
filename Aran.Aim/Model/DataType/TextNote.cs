using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class TextNote : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.TextNote; }
		}
		
		public string Value
		{
			get { return GetFieldValue <string> ((int) PropertyTextNote.Value); }
			set { SetFieldValue <string> ((int) PropertyTextNote.Value, value); }
		}
		
		public language Lang
		{
			get { return GetFieldValue <language> ((int) PropertyTextNote.Lang); }
			set { SetFieldValue <language> ((int) PropertyTextNote.Lang, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTextNote
	{
		Value,
		Lang,
		NEXT_CLASS
	}
	
	public static class MetadataTextNote
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTextNote ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTextNote.Value, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyTextNote.Lang, (int) EnumType.Language);
		}
	}
}
