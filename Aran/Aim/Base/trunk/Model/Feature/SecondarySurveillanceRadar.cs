using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class SecondarySurveillanceRadar : SurveillanceRadar
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SecondarySurveillanceRadar; }
		}
		
		public CodeTransponder? Transponder
		{
			get { return GetNullableFieldValue <CodeTransponder> ((int) PropertySecondarySurveillanceRadar.Transponder); }
			set { SetNullableFieldValue <CodeTransponder> ((int) PropertySecondarySurveillanceRadar.Transponder, value); }
		}
		
		public bool? Autonomous
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySecondarySurveillanceRadar.Autonomous); }
			set { SetNullableFieldValue <bool> ((int) PropertySecondarySurveillanceRadar.Autonomous, value); }
		}
		
		public bool? Monopulse
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySecondarySurveillanceRadar.Monopulse); }
			set { SetNullableFieldValue <bool> ((int) PropertySecondarySurveillanceRadar.Monopulse, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySecondarySurveillanceRadar
	{
		Transponder = PropertySurveillanceRadar.NEXT_CLASS,
		Autonomous,
		Monopulse,
		NEXT_CLASS
	}
	
	public static class MetadataSecondarySurveillanceRadar
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSecondarySurveillanceRadar ()
		{
			PropInfoList = MetadataSurveillanceRadar.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySecondarySurveillanceRadar.Transponder, (int) EnumType.CodeTransponder, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySecondarySurveillanceRadar.Autonomous, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySecondarySurveillanceRadar.Monopulse, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
