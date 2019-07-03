using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class PrimarySurveillanceRadar : SurveillanceRadar
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.PrimarySurveillanceRadar; }
		}
		
		public CodePrimaryRadar? Type
		{
			get { return GetNullableFieldValue <CodePrimaryRadar> ((int) PropertyPrimarySurveillanceRadar.Type); }
			set { SetNullableFieldValue <CodePrimaryRadar> ((int) PropertyPrimarySurveillanceRadar.Type, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPrimarySurveillanceRadar
	{
		Type = PropertySurveillanceRadar.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataPrimarySurveillanceRadar
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPrimarySurveillanceRadar ()
		{
			PropInfoList = MetadataSurveillanceRadar.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPrimarySurveillanceRadar.Type, (int) EnumType.CodePrimaryRadar, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
