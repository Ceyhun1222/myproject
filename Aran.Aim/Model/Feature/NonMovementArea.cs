using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class NonMovementArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.NonMovementArea; }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyNonMovementArea.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyNonMovementArea.AssociatedAirportHeliport, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyNonMovementArea.Extent); }
			set { SetValue ((int) PropertyNonMovementArea.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNonMovementArea
	{
		AssociatedAirportHeliport = PropertyFeature.NEXT_CLASS,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataNonMovementArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNonMovementArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNonMovementArea.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNonMovementArea.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
