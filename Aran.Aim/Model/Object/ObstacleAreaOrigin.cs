using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Model.Attribute;

namespace Aran.Aim.Features
{
	public class ObstacleAreaOrigin : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ObstacleAreaOrigin; }
		}
		
		public ObstacleAreaOriginChoice Choice
		{
			get { return (ObstacleAreaOriginChoice) RefType; }
		}

        [LinkedFeature(FeatureType.AirportHeliport)]
		public FeatureRef OwnerAirport
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) ObstacleAreaOriginChoice.AirportHeliport;
			}
		}

        [LinkedFeature(FeatureType.RunwayDirection)]
		public FeatureRef OwnerRunway
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) ObstacleAreaOriginChoice.RunwayDirection;
			}
		}

        [LinkedFeature(FeatureType.OrganisationAuthority)]
		public FeatureRef OwnerOrganisation
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) ObstacleAreaOriginChoice.OrganisationAuthority;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyObstacleAreaOrigin
	{
		OwnerAirport = PropertyAObject.NEXT_CLASS,
		OwnerRunway,
		OwnerOrganisation,
		NEXT_CLASS
	}
	
	public static class MetadataObstacleAreaOrigin
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataObstacleAreaOrigin ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyObstacleAreaOrigin.OwnerAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAreaOrigin.OwnerRunway, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleAreaOrigin.OwnerOrganisation, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
