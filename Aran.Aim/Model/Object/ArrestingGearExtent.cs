using System;
using System.Collections.Generic;
using System.ComponentModel;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class ArrestingGearExtent : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ArrestingGearExtent; }
		}
		
		public ArrestingGearExtentChoice Choice
		{
			get { return (ArrestingGearExtentChoice) RefType; }
		}
		
      
		public ElevatedCurve CurveExtent
		{
			get { return (ElevatedCurve) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) ArrestingGearExtentChoice.ElevatedCurve;
			}
		}
		
		public ElevatedSurface SurfaceExtent
		{
			get { return (ElevatedSurface) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) ArrestingGearExtentChoice.ElevatedSurface;
			}
		}
		
		public ElevatedPoint PointExtent
		{
			get { return (ElevatedPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) ArrestingGearExtentChoice.ElevatedPoint;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyArrestingGearExtent
	{
		CurveExtent = PropertyAObject.NEXT_CLASS,
		SurfaceExtent,
		PointExtent,
		NEXT_CLASS
	}
	
	public static class MetadataArrestingGearExtent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataArrestingGearExtent ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyArrestingGearExtent.CurveExtent, (int) ObjectType.ElevatedCurve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGearExtent.SurfaceExtent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGearExtent.PointExtent, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
		}
	}
}
