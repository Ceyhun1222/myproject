using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class AerialRefuellingTrack : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AerialRefuellingTrack; }
		}
		
		public Curve Extent
		{
			get { return GetObject <Curve> ((int) PropertyAerialRefuellingTrack.Extent); }
			set { SetValue ((int) PropertyAerialRefuellingTrack.Extent, value); }
		}
		
		public List <AerialRefuellingPoint> Point
		{
			get { return GetObjectList <AerialRefuellingPoint> ((int) PropertyAerialRefuellingTrack.Point); }
		}
		
		public List <AirspaceLayer> VerticalExtent
		{
			get { return GetObjectList <AirspaceLayer> ((int) PropertyAerialRefuellingTrack.VerticalExtent); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAerialRefuellingTrack
	{
		Extent = PropertyAObject.NEXT_CLASS,
		Point,
		VerticalExtent,
		NEXT_CLASS
	}
	
	public static class MetadataAerialRefuellingTrack
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAerialRefuellingTrack ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAerialRefuellingTrack.Extent, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingTrack.Point, (int) ObjectType.AerialRefuellingPoint, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuellingTrack.VerticalExtent, (int) ObjectType.AirspaceLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
