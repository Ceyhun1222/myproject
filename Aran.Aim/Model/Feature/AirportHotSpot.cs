using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirportHotSpot : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportHotSpot; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHotSpot.Designator); }
			set { SetFieldValue <string> ((int) PropertyAirportHotSpot.Designator, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHotSpot.Instruction); }
			set { SetFieldValue <string> ((int) PropertyAirportHotSpot.Instruction, value); }
		}
		
		public ElevatedSurface Area
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyAirportHotSpot.Area); }
			set { SetValue ((int) PropertyAirportHotSpot.Area, value); }
		}
		
		public FeatureRef AffectedAirport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirportHotSpot.AffectedAirport); }
			set { SetValue ((int) PropertyAirportHotSpot.AffectedAirport, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHotSpot
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Instruction,
		Area,
		AffectedAirport,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHotSpot
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHotSpot ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHotSpot.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHotSpot.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHotSpot.Area, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHotSpot.AffectedAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
