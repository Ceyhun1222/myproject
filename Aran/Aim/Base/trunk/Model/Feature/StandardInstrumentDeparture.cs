using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class StandardInstrumentDeparture : Procedure
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandardInstrumentDeparture; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyStandardInstrumentDeparture.Designator); }
			set { SetFieldValue <string> ((int) PropertyStandardInstrumentDeparture.Designator, value); }
		}
		
		public bool? ContingencyRoute
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyStandardInstrumentDeparture.ContingencyRoute); }
			set { SetNullableFieldValue <bool> ((int) PropertyStandardInstrumentDeparture.ContingencyRoute, value); }
		}
		
		public LandingTakeoffAreaCollection Takeoff
		{
			get { return GetObject <LandingTakeoffAreaCollection> ((int) PropertyStandardInstrumentDeparture.Takeoff); }
			set { SetValue ((int) PropertyStandardInstrumentDeparture.Takeoff, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardInstrumentDeparture
	{
		Designator = PropertyProcedure.NEXT_CLASS,
		ContingencyRoute,
		Takeoff,
		NEXT_CLASS
	}
	
	public static class MetadataStandardInstrumentDeparture
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardInstrumentDeparture ()
		{
			PropInfoList = MetadataProcedure.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardInstrumentDeparture.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardInstrumentDeparture.ContingencyRoute, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardInstrumentDeparture.Takeoff, (int) ObjectType.LandingTakeoffAreaCollection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
