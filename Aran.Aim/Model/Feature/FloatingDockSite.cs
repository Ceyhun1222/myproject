using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class FloatingDockSite : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.FloatingDockSite; }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyFloatingDockSite.Extent); }
			set { SetValue ((int) PropertyFloatingDockSite.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFloatingDockSite
	{
		Extent = PropertyFeature.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataFloatingDockSite
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFloatingDockSite ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFloatingDockSite.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
