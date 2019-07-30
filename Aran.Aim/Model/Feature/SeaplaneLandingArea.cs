using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class SeaplaneLandingArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SeaplaneLandingArea; }
		}
		
		public List <FeatureRefObject> RampSite
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertySeaplaneLandingArea.RampSite); }
		}
		
		public List <FeatureRefObject> DockSite
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertySeaplaneLandingArea.DockSite); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertySeaplaneLandingArea.Extent); }
			set { SetValue ((int) PropertySeaplaneLandingArea.Extent, value); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertySeaplaneLandingArea.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySeaplaneLandingArea
	{
		RampSite = PropertyFeature.NEXT_CLASS,
		DockSite,
		Extent,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataSeaplaneLandingArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSeaplaneLandingArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySeaplaneLandingArea.RampSite, (int) FeatureType.SeaplaneRampSite, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySeaplaneLandingArea.DockSite, (int) FeatureType.FloatingDockSite, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySeaplaneLandingArea.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySeaplaneLandingArea.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
