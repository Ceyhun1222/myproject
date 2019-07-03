using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class HoldingPatternLength : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.HoldingPatternLength; }
		}
		
		public HoldingPatternLengthChoice Choice
		{
			get { return (HoldingPatternLengthChoice) RefType; }
		}
		
		public HoldingPatternDuration EndTime
		{
			get { return (HoldingPatternDuration) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) HoldingPatternLengthChoice.HoldingPatternDuration;
			}
		}
		
		public HoldingPatternDistance EndDistance
		{
			get { return (HoldingPatternDistance) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) HoldingPatternLengthChoice.HoldingPatternDistance;
			}
		}
		
		public SegmentPoint EndPoint
		{
			get { return (SegmentPoint) GetChoiceAbstractObject (AbstractType.SegmentPoint); }
			set
			{
				RefValue = value;
				RefType = (int) HoldingPatternLengthChoice.SegmentPoint;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingPatternLength
	{
		EndTime = PropertyAObject.NEXT_CLASS,
		EndDistance,
		EndPoint,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingPatternLength
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingPatternLength ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingPatternLength.EndTime, (int) ObjectType.HoldingPatternDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPatternLength.EndDistance, (int) ObjectType.HoldingPatternDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPatternLength.EndPoint, (int) AbstractType.SegmentPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
