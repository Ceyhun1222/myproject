using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TouchDownLiftOffMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TouchDownLiftOffMarking; }
		}
		
		public CodeTLOFSection? MarkingLocation
		{
			get { return GetNullableFieldValue <CodeTLOFSection> ((int) PropertyTouchDownLiftOffMarking.MarkingLocation); }
			set { SetNullableFieldValue <CodeTLOFSection> ((int) PropertyTouchDownLiftOffMarking.MarkingLocation, value); }
		}
		
		public FeatureRef MarkedTouchDownLiftOff
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTouchDownLiftOffMarking.MarkedTouchDownLiftOff); }
			set { SetValue ((int) PropertyTouchDownLiftOffMarking.MarkedTouchDownLiftOff, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTouchDownLiftOffMarking
	{
		MarkingLocation = PropertyMarking.NEXT_CLASS,
		MarkedTouchDownLiftOff,
		NEXT_CLASS
	}
	
	public static class MetadataTouchDownLiftOffMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTouchDownLiftOffMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTouchDownLiftOffMarking.MarkingLocation, (int) EnumType.CodeTLOFSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOffMarking.MarkedTouchDownLiftOff, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
