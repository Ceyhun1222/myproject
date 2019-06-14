using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public abstract class AirportGroundService : Service
	{
		public virtual AirportGroundServiceType AirportGroundServiceType 
		{
			get { return (AirportGroundServiceType) FeatureType; }
		}
		
		public List <FeatureRefObject> AirportHeliport
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirportGroundService.AirportHeliport); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportGroundService
	{
		AirportHeliport = PropertyService.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAirportGroundService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportGroundService ()
		{
			PropInfoList = MetadataService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportGroundService.AirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
