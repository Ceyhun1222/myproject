using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AirportSuppliesService : AirportGroundService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportSuppliesService; }
		}
		
		public List <Fuel> FuelSupply
		{
			get { return GetObjectList <Fuel> ((int) PropertyAirportSuppliesService.FuelSupply); }
		}
		
		public List <Oil> OilSupply
		{
			get { return GetObjectList <Oil> ((int) PropertyAirportSuppliesService.OilSupply); }
		}
		
		public List <Nitrogen> NitrogenSupply
		{
			get { return GetObjectList <Nitrogen> ((int) PropertyAirportSuppliesService.NitrogenSupply); }
		}
		
		public List <Oxygen> OxygenSupply
		{
			get { return GetObjectList <Oxygen> ((int) PropertyAirportSuppliesService.OxygenSupply); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportSuppliesService
	{
		FuelSupply = PropertyAirportGroundService.NEXT_CLASS,
		OilSupply,
		NitrogenSupply,
		OxygenSupply,
		NEXT_CLASS
	}
	
	public static class MetadataAirportSuppliesService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportSuppliesService ()
		{
			PropInfoList = MetadataAirportGroundService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportSuppliesService.FuelSupply, (int) ObjectType.Fuel, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportSuppliesService.OilSupply, (int) ObjectType.Oil, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportSuppliesService.NitrogenSupply, (int) ObjectType.Nitrogen, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportSuppliesService.OxygenSupply, (int) ObjectType.Oxygen, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
