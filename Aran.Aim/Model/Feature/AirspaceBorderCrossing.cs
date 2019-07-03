using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirspaceBorderCrossing : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirspaceBorderCrossing; }
		}
		
		public FeatureRef ExitedAirspace
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirspaceBorderCrossing.ExitedAirspace); }
			set { SetValue ((int) PropertyAirspaceBorderCrossing.ExitedAirspace, value); }
		}
		
		public FeatureRef EnteredAirspace
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirspaceBorderCrossing.EnteredAirspace); }
			set { SetValue ((int) PropertyAirspaceBorderCrossing.EnteredAirspace, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceBorderCrossing
	{
		ExitedAirspace = PropertyFeature.NEXT_CLASS,
		EnteredAirspace,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceBorderCrossing
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceBorderCrossing ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceBorderCrossing.ExitedAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceBorderCrossing.EnteredAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
