using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ProcedureDME : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ProcedureDME; }
		}
		
		public bool? CriticalDME
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyProcedureDME.CriticalDME); }
			set { SetNullableFieldValue <bool> ((int) PropertyProcedureDME.CriticalDME, value); }
		}
		
		public bool? Satisfactory
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyProcedureDME.Satisfactory); }
			set { SetNullableFieldValue <bool> ((int) PropertyProcedureDME.Satisfactory, value); }
		}
		
		public FeatureRef DME
		{
			get { return (FeatureRef ) GetValue ((int) PropertyProcedureDME.DME); }
			set { SetValue ((int) PropertyProcedureDME.DME, value); }
		}
		
		public AbstractSegmentLegRef SegmentLeg
		{
			get { return (AbstractSegmentLegRef ) GetValue ((int) PropertyProcedureDME.SegmentLeg); }
			set { SetValue ((int) PropertyProcedureDME.SegmentLeg, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyProcedureDME
	{
		CriticalDME = PropertyFeature.NEXT_CLASS,
		Satisfactory,
		DME,
		SegmentLeg,
		NEXT_CLASS
	}
	
	public static class MetadataProcedureDME
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataProcedureDME ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyProcedureDME.CriticalDME, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureDME.Satisfactory, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureDME.DME, (int) FeatureType.DME, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedureDME.SegmentLeg, (int) DataType.AbstractSegmentLegRef, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
