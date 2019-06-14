using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class PassengerLoadingBridge : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.PassengerLoadingBridge; }
		}
		
		public CodeLoadingBridge? Type
		{
			get { return GetNullableFieldValue <CodeLoadingBridge> ((int) PropertyPassengerLoadingBridge.Type); }
			set { SetNullableFieldValue <CodeLoadingBridge> ((int) PropertyPassengerLoadingBridge.Type, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyPassengerLoadingBridge.Extent); }
			set { SetValue ((int) PropertyPassengerLoadingBridge.Extent, value); }
		}
		
		public List <FeatureRefObject> AssociatedStand
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyPassengerLoadingBridge.AssociatedStand); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPassengerLoadingBridge
	{
		Type = PropertyFeature.NEXT_CLASS,
		Extent,
		AssociatedStand,
		NEXT_CLASS
	}
	
	public static class MetadataPassengerLoadingBridge
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPassengerLoadingBridge ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPassengerLoadingBridge.Type, (int) EnumType.CodeLoadingBridge, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPassengerLoadingBridge.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPassengerLoadingBridge.AssociatedStand, (int) FeatureType.AircraftStand, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
