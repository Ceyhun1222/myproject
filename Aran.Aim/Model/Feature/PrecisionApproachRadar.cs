using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class PrecisionApproachRadar : RadarEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.PrecisionApproachRadar; }
		}
		
		public CodePAR? PrecisionApproachRadarType
		{
			get { return GetNullableFieldValue <CodePAR> ((int) PropertyPrecisionApproachRadar.PrecisionApproachRadarType); }
			set { SetNullableFieldValue <CodePAR> ((int) PropertyPrecisionApproachRadar.PrecisionApproachRadarType, value); }
		}
		
		public double? Slope
		{
			get { return GetNullableFieldValue <double> ((int) PropertyPrecisionApproachRadar.Slope); }
			set { SetNullableFieldValue <double> ((int) PropertyPrecisionApproachRadar.Slope, value); }
		}
		
		public double? SlopeAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyPrecisionApproachRadar.SlopeAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyPrecisionApproachRadar.SlopeAccuracy, value); }
		}
		
		public List <Reflector> Reflector
		{
			get { return GetObjectList <Reflector> ((int) PropertyPrecisionApproachRadar.Reflector); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPrecisionApproachRadar
	{
		PrecisionApproachRadarType = PropertyRadarEquipment.NEXT_CLASS,
		Slope,
		SlopeAccuracy,
		Reflector,
		NEXT_CLASS
	}
	
	public static class MetadataPrecisionApproachRadar
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPrecisionApproachRadar ()
		{
			PropInfoList = MetadataRadarEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPrecisionApproachRadar.PrecisionApproachRadarType, (int) EnumType.CodePAR, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPrecisionApproachRadar.Slope, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPrecisionApproachRadar.SlopeAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPrecisionApproachRadar.Reflector, (int) ObjectType.Reflector, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
