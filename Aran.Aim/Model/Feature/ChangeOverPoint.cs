using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ChangeOverPoint : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ChangeOverPoint; }
		}
		
		public ValDistance Distance
		{
			get { return (ValDistance ) GetValue ((int) PropertyChangeOverPoint.Distance); }
			set { SetValue ((int) PropertyChangeOverPoint.Distance, value); }
		}
		
		public SignificantPoint Location
		{
			get { return GetObject <SignificantPoint> ((int) PropertyChangeOverPoint.Location); }
			set { SetValue ((int) PropertyChangeOverPoint.Location, value); }
		}
		
		public RoutePortion ApplicableRoutePortion
		{
			get { return GetObject <RoutePortion> ((int) PropertyChangeOverPoint.ApplicableRoutePortion); }
			set { SetValue ((int) PropertyChangeOverPoint.ApplicableRoutePortion, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyChangeOverPoint
	{
		Distance = PropertyFeature.NEXT_CLASS,
		Location,
		ApplicableRoutePortion,
		NEXT_CLASS
	}
	
	public static class MetadataChangeOverPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataChangeOverPoint ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyChangeOverPoint.Distance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyChangeOverPoint.Location, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyChangeOverPoint.ApplicableRoutePortion, (int) ObjectType.RoutePortion, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
