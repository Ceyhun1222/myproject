using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class DirectionFinder : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DirectionFinder; }
		}
		
		public bool? Doppler
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyDirectionFinder.Doppler); }
			set { SetNullableFieldValue <bool> ((int) PropertyDirectionFinder.Doppler, value); }
		}
		
		public List <FeatureRefObject> InformationProvision
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyDirectionFinder.InformationProvision); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDirectionFinder
	{
		Doppler = PropertyNavaidEquipment.NEXT_CLASS,
		InformationProvision,
		NEXT_CLASS
	}
	
	public static class MetadataDirectionFinder
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDirectionFinder ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDirectionFinder.Doppler, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDirectionFinder.InformationProvision, (int) FeatureType.InformationService, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
