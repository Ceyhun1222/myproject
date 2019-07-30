using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.DataTypes
{
	public class AbstractMarkingRef : AbstractFeatureRef <MarkingType>
	{
		public AbstractMarkingRef ()
		{
		}
		
		public AbstractMarkingRef (MarkingRefType markingRef, FeatureRef feature)
		 : base (markingRef, feature)
		{
		}
		
		public override DataType DataType
		{
			get { return DataType.AbstractMarkingRef; }
		}
		
		public int Type
		{
			get { return GetFieldValue <int> ((int) PropertyAbstractMarkingRef.Type); }
			set { SetFieldValue <int> ((int) PropertyAbstractMarkingRef.Type, value); }
		}
		
		public FeatureRef Feature
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAbstractMarkingRef.Feature); }
			set { SetValue ((int) PropertyAbstractMarkingRef.Feature, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAbstractMarkingRef
	{
		Type,
		Feature,
		NEXT_CLASS
	}
	
	public static class MetadataAbstractMarkingRef
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAbstractMarkingRef ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAbstractMarkingRef.Type, (int) AimFieldType.Sysint);
			PropInfoList.Add (PropertyAbstractMarkingRef.Feature, (int) DataType.FeatureRef);
		}
	}
}
