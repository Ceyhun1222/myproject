using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class MarkingExtent : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.MarkingExtent; }
		}
		
		public MarkingExtentChoice Choice
		{
			get { return (MarkingExtentChoice) RefType; }
		}
		
		public ElevatedSurface SurfaceExtent
		{
			get { return (ElevatedSurface) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) MarkingExtentChoice.ElevatedSurface;
			}
		}
		
		public ElevatedCurve CurveExtent
		{
			get { return (ElevatedCurve) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) MarkingExtentChoice.ElevatedCurve;
			}
		}
		
		public ElevatedPoint Location
		{
			get { return (ElevatedPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) MarkingExtentChoice.ElevatedPoint;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMarkingExtent
	{
		SurfaceExtent = PropertyAObject.NEXT_CLASS,
		CurveExtent,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataMarkingExtent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMarkingExtent ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMarkingExtent.SurfaceExtent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingExtent.CurveExtent, (int) ObjectType.ElevatedCurve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingExtent.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
