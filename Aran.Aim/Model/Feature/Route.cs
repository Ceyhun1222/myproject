using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Route : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Route; }
		}
		
		public CodeRouteDesignatorPrefix? DesignatorPrefix
		{
			get { return GetNullableFieldValue <CodeRouteDesignatorPrefix> ((int) PropertyRoute.DesignatorPrefix); }
			set { SetNullableFieldValue <CodeRouteDesignatorPrefix> ((int) PropertyRoute.DesignatorPrefix, value); }
		}
		
		public CodeRouteDesignatorLetter? DesignatorSecondLetter
		{
			get { return GetNullableFieldValue <CodeRouteDesignatorLetter> ((int) PropertyRoute.DesignatorSecondLetter); }
			set { SetNullableFieldValue <CodeRouteDesignatorLetter> ((int) PropertyRoute.DesignatorSecondLetter, value); }
		}
		
		public uint? DesignatorNumber
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyRoute.DesignatorNumber); }
			set { SetNullableFieldValue <uint> ((int) PropertyRoute.DesignatorNumber, value); }
		}
		
		public CodeUpperAlpha? MultipleIdentifier
		{
			get { return GetNullableFieldValue <CodeUpperAlpha> ((int) PropertyRoute.MultipleIdentifier); }
			set { SetNullableFieldValue <CodeUpperAlpha> ((int) PropertyRoute.MultipleIdentifier, value); }
		}
		
		public string LocationDesignator
		{
			get { return GetFieldValue <string> ((int) PropertyRoute.LocationDesignator); }
			set { SetFieldValue <string> ((int) PropertyRoute.LocationDesignator, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyRoute.Name); }
			set { SetFieldValue <string> ((int) PropertyRoute.Name, value); }
		}
		
		public CodeRoute? Type
		{
			get { return GetNullableFieldValue <CodeRoute> ((int) PropertyRoute.Type); }
			set { SetNullableFieldValue <CodeRoute> ((int) PropertyRoute.Type, value); }
		}
		
		public CodeFlightRule? FlightRule
		{
			get { return GetNullableFieldValue <CodeFlightRule> ((int) PropertyRoute.FlightRule); }
			set { SetNullableFieldValue <CodeFlightRule> ((int) PropertyRoute.FlightRule, value); }
		}
		
		public CodeRouteOrigin? InternationalUse
		{
			get { return GetNullableFieldValue <CodeRouteOrigin> ((int) PropertyRoute.InternationalUse); }
			set { SetNullableFieldValue <CodeRouteOrigin> ((int) PropertyRoute.InternationalUse, value); }
		}
		
		public CodeMilitaryStatus? MilitaryUse
		{
			get { return GetNullableFieldValue <CodeMilitaryStatus> ((int) PropertyRoute.MilitaryUse); }
			set { SetNullableFieldValue <CodeMilitaryStatus> ((int) PropertyRoute.MilitaryUse, value); }
		}
		
		public CodeMilitaryTraining? MilitaryTrainingType
		{
			get { return GetNullableFieldValue <CodeMilitaryTraining> ((int) PropertyRoute.MilitaryTrainingType); }
			set { SetNullableFieldValue <CodeMilitaryTraining> ((int) PropertyRoute.MilitaryTrainingType, value); }
		}
		
		public FeatureRef UserOrganisation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRoute.UserOrganisation); }
			set { SetValue ((int) PropertyRoute.UserOrganisation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRoute
	{
		DesignatorPrefix = PropertyFeature.NEXT_CLASS,
		DesignatorSecondLetter,
		DesignatorNumber,
		MultipleIdentifier,
		LocationDesignator,
		Name,
		Type,
		FlightRule,
		InternationalUse,
		MilitaryUse,
		MilitaryTrainingType,
		UserOrganisation,
		NEXT_CLASS
	}
	
	public static class MetadataRoute
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRoute ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRoute.DesignatorPrefix, (int) EnumType.CodeRouteDesignatorPrefix, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.DesignatorSecondLetter, (int) EnumType.CodeRouteDesignatorLetter, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.DesignatorNumber, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.MultipleIdentifier, (int) EnumType.CodeUpperAlpha, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.LocationDesignator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.Type, (int) EnumType.CodeRoute, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.FlightRule, (int) EnumType.CodeFlightRule, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.InternationalUse, (int) EnumType.CodeRouteOrigin, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.MilitaryUse, (int) EnumType.CodeMilitaryStatus, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.MilitaryTrainingType, (int) EnumType.CodeMilitaryTraining, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoute.UserOrganisation, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
