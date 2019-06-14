using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class DataQuality : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DataQuality; }
		}
		
		public string attributes
		{
			get { return GetFieldValue <string> ((int) PropertyDataQuality.attributes); }
			set { SetFieldValue <string> ((int) PropertyDataQuality.attributes, value); }
		}
		
		public DataQualityElement report
		{
			get { return (DataQualityElement ) GetValue ((int) PropertyDataQuality.report); }
			set { SetValue ((int) PropertyDataQuality.report, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDataQuality
	{
		attributes = PropertyAObject.NEXT_CLASS,
		report,
		NEXT_CLASS
	}
	
	public static class MetadataDataQuality
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDataQuality ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDataQuality.attributes, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDataQuality.report, (int) DataType.DataQualityElement, PropertyTypeCharacter.Nullable);
		}
	}
}
