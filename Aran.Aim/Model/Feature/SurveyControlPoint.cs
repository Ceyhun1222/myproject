using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SurveyControlPoint : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SurveyControlPoint; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertySurveyControlPoint.Designator); }
			set { SetFieldValue <string> ((int) PropertySurveyControlPoint.Designator, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertySurveyControlPoint.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertySurveyControlPoint.AssociatedAirportHeliport, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertySurveyControlPoint.Location); }
			set { SetValue ((int) PropertySurveyControlPoint.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurveyControlPoint
	{
		Designator = PropertyFeature.NEXT_CLASS,
		AssociatedAirportHeliport,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataSurveyControlPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurveyControlPoint ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurveyControlPoint.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveyControlPoint.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveyControlPoint.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
