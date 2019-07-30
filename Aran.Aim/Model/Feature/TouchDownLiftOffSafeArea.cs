using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TouchDownLiftOffSafeArea : AirportHeliportProtectionArea
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TouchDownLiftOffSafeArea; }
		}
		
		public FeatureRef ProtectedTouchDownLiftOff
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTouchDownLiftOffSafeArea.ProtectedTouchDownLiftOff); }
			set { SetValue ((int) PropertyTouchDownLiftOffSafeArea.ProtectedTouchDownLiftOff, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTouchDownLiftOffSafeArea
	{
		ProtectedTouchDownLiftOff = PropertyAirportHeliportProtectionArea.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataTouchDownLiftOffSafeArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTouchDownLiftOffSafeArea ()
		{
			PropInfoList = MetadataAirportHeliportProtectionArea.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTouchDownLiftOffSafeArea.ProtectedTouchDownLiftOff, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
