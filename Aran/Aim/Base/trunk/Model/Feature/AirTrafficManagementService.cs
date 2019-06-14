using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class AirTrafficManagementService : Service
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirTrafficManagementService; }
		}
		
		public CodeServiceATFM? Type
		{
			get { return GetNullableFieldValue <CodeServiceATFM> ((int) PropertyAirTrafficManagementService.Type); }
			set { SetNullableFieldValue <CodeServiceATFM> ((int) PropertyAirTrafficManagementService.Type, value); }
		}
		
		public List <FeatureRefObject> ClientAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficManagementService.ClientAirspace); }
		}
		
		public List <FeatureRefObject> ClientAerialRefuelling
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficManagementService.ClientAerialRefuelling); }
		}
		
		public List <RoutePortion> ClientRoute
		{
			get { return GetObjectList <RoutePortion> ((int) PropertyAirTrafficManagementService.ClientRoute); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirTrafficManagementService
	{
		Type = PropertyService.NEXT_CLASS,
		ClientAirspace,
		ClientAerialRefuelling,
		ClientRoute,
		NEXT_CLASS
	}
	
	public static class MetadataAirTrafficManagementService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirTrafficManagementService ()
		{
			PropInfoList = MetadataService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirTrafficManagementService.Type, (int) EnumType.CodeServiceATFM, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficManagementService.ClientAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficManagementService.ClientAerialRefuelling, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficManagementService.ClientRoute, (int) ObjectType.RoutePortion, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
