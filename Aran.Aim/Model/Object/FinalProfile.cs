using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class FinalProfile : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FinalProfile; }
		}
		
		public List <ApproachAltitudeTable> Altitude
		{
			get { return GetObjectList <ApproachAltitudeTable> ((int) PropertyFinalProfile.Altitude); }
		}
		
		public List <ApproachDistanceTable> Distance
		{
			get { return GetObjectList <ApproachDistanceTable> ((int) PropertyFinalProfile.Distance); }
		}
		
		public List <ApproachTimingTable> Timing
		{
			get { return GetObjectList <ApproachTimingTable> ((int) PropertyFinalProfile.Timing); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFinalProfile
	{
		Altitude = PropertyAObject.NEXT_CLASS,
		Distance,
		Timing,
		NEXT_CLASS
	}
	
	public static class MetadataFinalProfile
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFinalProfile ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFinalProfile.Altitude, (int) ObjectType.ApproachAltitudeTable, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalProfile.Distance, (int) ObjectType.ApproachDistanceTable, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalProfile.Timing, (int) ObjectType.ApproachTimingTable, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
