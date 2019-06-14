using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class NDB : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.NDB; }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyNDB.Frequency); }
			set { SetValue ((int) PropertyNDB.Frequency, value); }
		}
		
		public CodeNDBUsage? Class
		{
			get { return GetNullableFieldValue <CodeNDBUsage> ((int) PropertyNDB.Class); }
			set { SetNullableFieldValue <CodeNDBUsage> ((int) PropertyNDB.Class, value); }
		}
		
		public CodeEmissionBand? EmissionBand
		{
			get { return GetNullableFieldValue <CodeEmissionBand> ((int) PropertyNDB.EmissionBand); }
			set { SetNullableFieldValue <CodeEmissionBand> ((int) PropertyNDB.EmissionBand, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNDB
	{
		Frequency = PropertyNavaidEquipment.NEXT_CLASS,
		Class,
		EmissionBand,
		NEXT_CLASS
	}
	
	public static class MetadataNDB
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNDB ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNDB.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNDB.Class, (int) EnumType.CodeNDBUsage, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNDB.EmissionBand, (int) EnumType.CodeEmissionBand, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
