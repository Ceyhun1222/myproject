using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class LandingTakeoffAreaCollection : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LandingTakeoffAreaCollection; }
		}
		
		public List <FeatureRefObject> Runway
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyLandingTakeoffAreaCollection.Runway); }
		}
		
		public List <FeatureRefObject> TLOF
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyLandingTakeoffAreaCollection.TLOF); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLandingTakeoffAreaCollection
	{
		Runway = PropertyAObject.NEXT_CLASS,
		TLOF,
		NEXT_CLASS
	}
	
	public static class MetadataLandingTakeoffAreaCollection
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLandingTakeoffAreaCollection ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLandingTakeoffAreaCollection.Runway, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLandingTakeoffAreaCollection.TLOF, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
