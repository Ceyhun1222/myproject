using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RoutePortion : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RoutePortion; }
		}
		
		public SignificantPoint Start
		{
			get { return GetObject <SignificantPoint> ((int) PropertyRoutePortion.Start); }
			set { SetValue ((int) PropertyRoutePortion.Start, value); }
		}
		
		public SignificantPoint IntermediatePoint
		{
			get { return GetObject <SignificantPoint> ((int) PropertyRoutePortion.IntermediatePoint); }
			set { SetValue ((int) PropertyRoutePortion.IntermediatePoint, value); }
		}
		
		public FeatureRef ReferencedRoute
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRoutePortion.ReferencedRoute); }
			set { SetValue ((int) PropertyRoutePortion.ReferencedRoute, value); }
		}
		
		public SignificantPoint End
		{
			get { return GetObject <SignificantPoint> ((int) PropertyRoutePortion.End); }
			set { SetValue ((int) PropertyRoutePortion.End, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRoutePortion
	{
		Start = PropertyAObject.NEXT_CLASS,
		IntermediatePoint,
		ReferencedRoute,
		End,
		NEXT_CLASS
	}
	
	public static class MetadataRoutePortion
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRoutePortion ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRoutePortion.Start, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoutePortion.IntermediatePoint, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoutePortion.ReferencedRoute, (int) FeatureType.Route, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoutePortion.End, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
