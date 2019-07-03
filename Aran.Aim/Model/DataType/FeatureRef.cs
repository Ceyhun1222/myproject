using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class FeatureRef : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.FeatureRef; }
		}
		
		public long Id
		{
			get { return GetFieldValue <long> ((int) PropertyFeatureRef.Id); }
			set { SetFieldValue <long> ((int) PropertyFeatureRef.Id, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureRef
	{
		Id,
		NEXT_CLASS
	}
	
	public static class MetadataFeatureRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFeatureRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFeatureRef.Id, (int) AimFieldType.SysInt64);
		}
	}
}
