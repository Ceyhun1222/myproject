using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AirspaceLayerClass : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceLayerClass; }
		}
		
		public CodeAirspaceClassification? Classification
		{
			get { return GetNullableFieldValue <CodeAirspaceClassification> ((int) PropertyAirspaceLayerClass.Classification); }
			set { SetNullableFieldValue <CodeAirspaceClassification> ((int) PropertyAirspaceLayerClass.Classification, value); }
		}
		
		public List <AirspaceLayer> AssociatedLevels
		{
			get { return GetObjectList <AirspaceLayer> ((int) PropertyAirspaceLayerClass.AssociatedLevels); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceLayerClass
	{
		Classification = PropertyPropertiesWithSchedule.NEXT_CLASS,
		AssociatedLevels,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceLayerClass
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceLayerClass ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceLayerClass.Classification, (int) EnumType.CodeAirspaceClassification, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceLayerClass.AssociatedLevels, (int) ObjectType.AirspaceLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
