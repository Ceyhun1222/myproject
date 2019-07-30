using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class StandardInstrumentArrival : Procedure
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandardInstrumentArrival; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyStandardInstrumentArrival.Designator); }
			set { SetFieldValue <string> ((int) PropertyStandardInstrumentArrival.Designator, value); }
		}
		
		public LandingTakeoffAreaCollection Arrival
		{
			get { return GetObject <LandingTakeoffAreaCollection> ((int) PropertyStandardInstrumentArrival.Arrival); }
			set { SetValue ((int) PropertyStandardInstrumentArrival.Arrival, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardInstrumentArrival
	{
		Designator = PropertyProcedure.NEXT_CLASS,
		Arrival,
		NEXT_CLASS
	}
	
	public static class MetadataStandardInstrumentArrival
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardInstrumentArrival ()
		{
			PropInfoList = MetadataProcedure.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardInstrumentArrival.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardInstrumentArrival.Arrival, (int) ObjectType.LandingTakeoffAreaCollection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
