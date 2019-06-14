using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class CICitation : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.CICitation; }
		}
		
		public string Title
		{
			get { return GetFieldValue <string> ((int) PropertyCICitation.Title); }
			set { SetFieldValue <string> ((int) PropertyCICitation.Title, value); }
		}
		
		public CIDate Date
		{
			get { return (CIDate ) GetValue ((int) PropertyCICitation.Date); }
			set { SetValue ((int) PropertyCICitation.Date, value); }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCICitation
	{
		Title,
		Date,
		NEXT_CLASS
	}
	
	public static class MetadataCICitation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCICitation ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCICitation.Title, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCICitation.Date, (int) DataType.CIDate, PropertyTypeCharacter.Nullable);
		}
	}
}
