using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirportHeliportCollocation : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportHeliportCollocation; }
		}
		
		public CodeAirportHeliportCollocation? Type
		{
			get { return GetNullableFieldValue <CodeAirportHeliportCollocation> ((int) PropertyAirportHeliportCollocation.Type); }
			set { SetNullableFieldValue <CodeAirportHeliportCollocation> ((int) PropertyAirportHeliportCollocation.Type, value); }
		}
		
		public FeatureRef HostAirport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirportHeliportCollocation.HostAirport); }
			set { SetValue ((int) PropertyAirportHeliportCollocation.HostAirport, value); }
		}
		
		public FeatureRef DependentAirport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirportHeliportCollocation.DependentAirport); }
			set { SetValue ((int) PropertyAirportHeliportCollocation.DependentAirport, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportCollocation
	{
		Type = PropertyFeature.NEXT_CLASS,
		HostAirport,
		DependentAirport,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliportCollocation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportCollocation ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliportCollocation.Type, (int) EnumType.CodeAirportHeliportCollocation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportCollocation.HostAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportCollocation.DependentAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
