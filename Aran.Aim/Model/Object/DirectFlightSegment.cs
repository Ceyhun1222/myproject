using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class DirectFlightSegment : DirectFlight
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.DirectFlightSegment; }
		}
		
		public SignificantPoint End
		{
			get { return GetObject <SignificantPoint> ((int) PropertyDirectFlightSegment.End); }
			set { SetValue ((int) PropertyDirectFlightSegment.End, value); }
		}
		
		public SignificantPoint Start
		{
			get { return GetObject <SignificantPoint> ((int) PropertyDirectFlightSegment.Start); }
			set { SetValue ((int) PropertyDirectFlightSegment.Start, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDirectFlightSegment
	{
		End = PropertyDirectFlight.NEXT_CLASS,
		Start,
		NEXT_CLASS
	}
	
	public static class MetadataDirectFlightSegment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDirectFlightSegment ()
		{
			PropInfoList = MetadataDirectFlight.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDirectFlightSegment.End, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDirectFlightSegment.Start, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
