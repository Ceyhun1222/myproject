using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class SeaplaneRampSite : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SeaplaneRampSite; }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertySeaplaneRampSite.Extent); }
			set { SetValue ((int) PropertySeaplaneRampSite.Extent, value); }
		}
		
		public ElevatedCurve Centreline
		{
			get { return GetObject <ElevatedCurve> ((int) PropertySeaplaneRampSite.Centreline); }
			set { SetValue ((int) PropertySeaplaneRampSite.Centreline, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySeaplaneRampSite
	{
		Extent = PropertyFeature.NEXT_CLASS,
		Centreline,
		NEXT_CLASS
	}
	
	public static class MetadataSeaplaneRampSite
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSeaplaneRampSite ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySeaplaneRampSite.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySeaplaneRampSite.Centreline, (int) ObjectType.ElevatedCurve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
