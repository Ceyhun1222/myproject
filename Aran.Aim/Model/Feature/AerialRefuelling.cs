using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AerialRefuelling : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AerialRefuelling; }
		}
		
		public CodeAerialRefuellingPrefix? DesignatorPrefix
		{
			get { return GetNullableFieldValue <CodeAerialRefuellingPrefix> ((int) PropertyAerialRefuelling.DesignatorPrefix); }
			set { SetNullableFieldValue <CodeAerialRefuellingPrefix> ((int) PropertyAerialRefuelling.DesignatorPrefix, value); }
		}
		
		public uint? DesignatorNumber
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.DesignatorNumber); }
			set { SetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.DesignatorNumber, value); }
		}
		
		public string DesignatorSuffix
		{
			get { return GetFieldValue <string> ((int) PropertyAerialRefuelling.DesignatorSuffix); }
			set { SetFieldValue <string> ((int) PropertyAerialRefuelling.DesignatorSuffix, value); }
		}
		
		public CodeCardinalDirection? DesignatorDirection
		{
			get { return GetNullableFieldValue <CodeCardinalDirection> ((int) PropertyAerialRefuelling.DesignatorDirection); }
			set { SetNullableFieldValue <CodeCardinalDirection> ((int) PropertyAerialRefuelling.DesignatorDirection, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyAerialRefuelling.Name); }
			set { SetFieldValue <string> ((int) PropertyAerialRefuelling.Name, value); }
		}
		
		public CodeAerialRefuelling? Type
		{
			get { return GetNullableFieldValue <CodeAerialRefuelling> ((int) PropertyAerialRefuelling.Type); }
			set { SetNullableFieldValue <CodeAerialRefuelling> ((int) PropertyAerialRefuelling.Type, value); }
		}
		
		public uint? RadarBeaconSetting
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.RadarBeaconSetting); }
			set { SetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.RadarBeaconSetting, value); }
		}
		
		public uint? XbandRadarSetting
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.XbandRadarSetting); }
			set { SetNullableFieldValue <uint> ((int) PropertyAerialRefuelling.XbandRadarSetting, value); }
		}
		
		public CodeTACANChannel? TankerChannel
		{
			get { return GetNullableFieldValue <CodeTACANChannel> ((int) PropertyAerialRefuelling.TankerChannel); }
			set { SetNullableFieldValue <CodeTACANChannel> ((int) PropertyAerialRefuelling.TankerChannel, value); }
		}
		
		public CodeTACANChannel? ReceiverChannel
		{
			get { return GetNullableFieldValue <CodeTACANChannel> ((int) PropertyAerialRefuelling.ReceiverChannel); }
			set { SetNullableFieldValue <CodeTACANChannel> ((int) PropertyAerialRefuelling.ReceiverChannel, value); }
		}
		
		public bool? HelicopterRoute
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.HelicopterRoute); }
			set { SetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.HelicopterRoute, value); }
		}
		
		public bool? SpecialRefuelling
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.SpecialRefuelling); }
			set { SetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.SpecialRefuelling, value); }
		}
		
		public bool? BidirectionalUse
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.BidirectionalUse); }
			set { SetNullableFieldValue <bool> ((int) PropertyAerialRefuelling.BidirectionalUse, value); }
		}
		
		public CodeDirectionTurn? ReverseDirectionTurn
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertyAerialRefuelling.ReverseDirectionTurn); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertyAerialRefuelling.ReverseDirectionTurn, value); }
		}
		
		public List <RouteAvailability> Availability
		{
			get { return GetObjectList <RouteAvailability> ((int) PropertyAerialRefuelling.Availability); }
		}
		
		public List <FeatureRefObject> ProtectingAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAerialRefuelling.ProtectingAirspace); }
		}
		
		public List <AerialRefuellingTrack> Track
		{
			get { return GetObjectList <AerialRefuellingTrack> ((int) PropertyAerialRefuelling.Track); }
		}
		
		public List <AerialRefuellingAnchor> Anchor
		{
			get { return GetObjectList <AerialRefuellingAnchor> ((int) PropertyAerialRefuelling.Anchor); }
		}
		
		public FeatureRef OppositeTrack
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAerialRefuelling.OppositeTrack); }
			set { SetValue ((int) PropertyAerialRefuelling.OppositeTrack, value); }
		}
		
		public List <AuthorityForAerialRefuelling> ManagingOrganisation
		{
			get { return GetObjectList <AuthorityForAerialRefuelling> ((int) PropertyAerialRefuelling.ManagingOrganisation); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAerialRefuelling
	{
		DesignatorPrefix = PropertyFeature.NEXT_CLASS,
		DesignatorNumber,
		DesignatorSuffix,
		DesignatorDirection,
		Name,
		Type,
		RadarBeaconSetting,
		XbandRadarSetting,
		TankerChannel,
		ReceiverChannel,
		HelicopterRoute,
		SpecialRefuelling,
		BidirectionalUse,
		ReverseDirectionTurn,
		Availability,
		ProtectingAirspace,
		Track,
		Anchor,
		OppositeTrack,
		ManagingOrganisation,
		NEXT_CLASS
	}
	
	public static class MetadataAerialRefuelling
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAerialRefuelling ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAerialRefuelling.DesignatorPrefix, (int) EnumType.CodeAerialRefuellingPrefix, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.DesignatorNumber, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.DesignatorSuffix, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.DesignatorDirection, (int) EnumType.CodeCardinalDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.Type, (int) EnumType.CodeAerialRefuelling, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.RadarBeaconSetting, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.XbandRadarSetting, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.TankerChannel, (int) EnumType.CodeTACANChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.ReceiverChannel, (int) EnumType.CodeTACANChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.HelicopterRoute, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.SpecialRefuelling, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.BidirectionalUse, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.ReverseDirectionTurn, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.Availability, (int) ObjectType.RouteAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.ProtectingAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.Track, (int) ObjectType.AerialRefuellingTrack, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.Anchor, (int) ObjectType.AerialRefuellingAnchor, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.OppositeTrack, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAerialRefuelling.ManagingOrganisation, (int) ObjectType.AuthorityForAerialRefuelling, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
