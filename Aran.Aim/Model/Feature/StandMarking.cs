using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class StandMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandMarking; }
		}
		
		public FeatureRef MarkedStand
		{
			get { return (FeatureRef ) GetValue ((int) PropertyStandMarking.MarkedStand); }
			set { SetValue ((int) PropertyStandMarking.MarkedStand, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandMarking
	{
		MarkedStand = PropertyMarking.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataStandMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandMarking.MarkedStand, (int) FeatureType.AircraftStand, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
