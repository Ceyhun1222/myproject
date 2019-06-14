using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class FeatureTimeSliceMetadata : MDMetadata
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FeatureTimeSliceMetadata; }
		}
		
		public ResponsibleParty Contact
		{
			get { return (ResponsibleParty ) GetValue ((int) PropertyFeatureTimeSliceMetadata.Contact); }
			set { SetValue ((int) PropertyFeatureTimeSliceMetadata.Contact, value); }
		}
		
		public MeasureClassCode? MeasureClass
		{
			get { return GetNullableFieldValue <MeasureClassCode> ((int) PropertyFeatureTimeSliceMetadata.MeasureClass); }
			set { SetNullableFieldValue <MeasureClassCode> ((int) PropertyFeatureTimeSliceMetadata.MeasureClass, value); }
		}
		
		public string MeasEquipClass
		{
			get { return GetFieldValue <string> ((int) PropertyFeatureTimeSliceMetadata.MeasEquipClass); }
			set { SetFieldValue <string> ((int) PropertyFeatureTimeSliceMetadata.MeasEquipClass, value); }
		}
		
		public double? DataIntegrity
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.DataIntegrity); }
			set { SetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.DataIntegrity, value); }
		}
		
		public double? HorizontalResolution
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.HorizontalResolution); }
			set { SetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.HorizontalResolution, value); }
		}
		
		public double? VerticalResolution
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.VerticalResolution); }
			set { SetNullableFieldValue <double> ((int) PropertyFeatureTimeSliceMetadata.VerticalResolution, value); }
		}
		
		public List <DataQuality> DataQualityInfo
		{
			get { return GetObjectList <DataQuality> ((int) PropertyFeatureTimeSliceMetadata.DataQualityInfo); }
		}
		
		public IdentificationTimesliceFeature FeatureTimesliceIdentificationInfo
		{
			get { return GetObject <IdentificationTimesliceFeature> ((int) PropertyFeatureTimeSliceMetadata.FeatureTimesliceIdentificationInfo); }
			set { SetValue ((int) PropertyFeatureTimeSliceMetadata.FeatureTimesliceIdentificationInfo, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFeatureTimeSliceMetadata
	{
		Contact = PropertyMDMetadata.NEXT_CLASS,
		MeasureClass,
		MeasEquipClass,
		DataIntegrity,
		HorizontalResolution,
		VerticalResolution,
		DataQualityInfo,
		FeatureTimesliceIdentificationInfo,
		NEXT_CLASS
	}
	
	public static class MetadataFeatureTimeSliceMetadata
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFeatureTimeSliceMetadata ()
		{
			PropInfoList = MetadataMDMetadata.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.Contact, (int) DataType.ResponsibleParty, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.MeasureClass, (int) EnumType.MeasureClassCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.MeasEquipClass, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.DataIntegrity, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.HorizontalResolution, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.VerticalResolution, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.DataQualityInfo, (int) ObjectType.DataQuality, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeatureTimeSliceMetadata.FeatureTimesliceIdentificationInfo, (int) ObjectType.IdentificationTimesliceFeature, PropertyTypeCharacter.Nullable);
		}
	}
}
