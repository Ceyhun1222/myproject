using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class SearchRescueService : Service
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SearchRescueService; }
		}
		
		public CodeServiceSAR? Type
		{
			get { return GetNullableFieldValue <CodeServiceSAR> ((int) PropertySearchRescueService.Type); }
			set { SetNullableFieldValue <CodeServiceSAR> ((int) PropertySearchRescueService.Type, value); }
		}
		
		public List <FeatureRefObject> ClientAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertySearchRescueService.ClientAirspace); }
		}
		
		public List <RoutePortion> ClientRoute
		{
			get { return GetObjectList <RoutePortion> ((int) PropertySearchRescueService.ClientRoute); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySearchRescueService
	{
		Type = PropertyService.NEXT_CLASS,
		ClientAirspace,
		ClientRoute,
		NEXT_CLASS
	}
	
	public static class MetadataSearchRescueService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSearchRescueService ()
		{
			PropInfoList = MetadataService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySearchRescueService.Type, (int) EnumType.CodeServiceSAR, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySearchRescueService.ClientAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySearchRescueService.ClientRoute, (int) ObjectType.RoutePortion, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
