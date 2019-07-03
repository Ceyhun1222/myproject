using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class VerticalStructurePartGeometry : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.VerticalStructurePartGeometry; }
		}
		
		public VerticalStructurePartGeometryChoice Choice
		{
			get { return (VerticalStructurePartGeometryChoice) RefType; }
		}
		
		public ElevatedPoint Location
		{
			get { return (ElevatedPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) VerticalStructurePartGeometryChoice.ElevatedPoint;
			}
		}
		
		public ElevatedCurve LinearExtent
		{
			get { return (ElevatedCurve) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) VerticalStructurePartGeometryChoice.ElevatedCurve;
			}
		}
		
		public ElevatedSurface SurfaceExtent
		{
			get { return (ElevatedSurface) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) VerticalStructurePartGeometryChoice.ElevatedSurface;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVerticalStructurePartGeometry
	{
		Location = PropertyAObject.NEXT_CLASS,
		LinearExtent,
		SurfaceExtent,
		NEXT_CLASS
	}
	
	public static class MetadataVerticalStructurePartGeometry
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVerticalStructurePartGeometry ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVerticalStructurePartGeometry.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePartGeometry.LinearExtent, (int) ObjectType.ElevatedCurve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructurePartGeometry.SurfaceExtent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
		}
	}
}
