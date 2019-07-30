using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class InstrumentApproachProcedure : Procedure
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.InstrumentApproachProcedure; }
		}
		
		public CodeApproachPrefix? ApproachPrefix
		{
			get { return GetNullableFieldValue <CodeApproachPrefix> ((int) PropertyInstrumentApproachProcedure.ApproachPrefix); }
			set { SetNullableFieldValue <CodeApproachPrefix> ((int) PropertyInstrumentApproachProcedure.ApproachPrefix, value); }
		}
		
		public CodeApproach? ApproachType
		{
			get { return GetNullableFieldValue <CodeApproach> ((int) PropertyInstrumentApproachProcedure.ApproachType); }
			set { SetNullableFieldValue <CodeApproach> ((int) PropertyInstrumentApproachProcedure.ApproachType, value); }
		}
		
		public CodeUpperAlpha? MultipleIdentification
		{
			get { return GetNullableFieldValue <CodeUpperAlpha> ((int) PropertyInstrumentApproachProcedure.MultipleIdentification); }
			set { SetNullableFieldValue <CodeUpperAlpha> ((int) PropertyInstrumentApproachProcedure.MultipleIdentification, value); }
		}
		
		public double? CopterTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyInstrumentApproachProcedure.CopterTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyInstrumentApproachProcedure.CopterTrack, value); }
		}
		
		public CodeUpperAlpha? CirclingIdentification
		{
			get { return GetNullableFieldValue <CodeUpperAlpha> ((int) PropertyInstrumentApproachProcedure.CirclingIdentification); }
			set { SetNullableFieldValue <CodeUpperAlpha> ((int) PropertyInstrumentApproachProcedure.CirclingIdentification, value); }
		}
		
		public string CourseReversalInstruction
		{
			get { return GetFieldValue <string> ((int) PropertyInstrumentApproachProcedure.CourseReversalInstruction); }
			set { SetFieldValue <string> ((int) PropertyInstrumentApproachProcedure.CourseReversalInstruction, value); }
		}
		
		public CodeApproachEquipmentAdditional? AdditionalEquipment
		{
			get { return GetNullableFieldValue <CodeApproachEquipmentAdditional> ((int) PropertyInstrumentApproachProcedure.AdditionalEquipment); }
			set { SetNullableFieldValue <CodeApproachEquipmentAdditional> ((int) PropertyInstrumentApproachProcedure.AdditionalEquipment, value); }
		}
		
		public double? ChannelGNSS
		{
			get { return GetNullableFieldValue <double> ((int) PropertyInstrumentApproachProcedure.ChannelGNSS); }
			set { SetNullableFieldValue <double> ((int) PropertyInstrumentApproachProcedure.ChannelGNSS, value); }
		}
		
		public bool? WAASReliable
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyInstrumentApproachProcedure.WAASReliable); }
			set { SetNullableFieldValue <bool> ((int) PropertyInstrumentApproachProcedure.WAASReliable, value); }
		}
		
		public LandingTakeoffAreaCollection Landing
		{
			get { return GetObject <LandingTakeoffAreaCollection> ((int) PropertyInstrumentApproachProcedure.Landing); }
			set { SetValue ((int) PropertyInstrumentApproachProcedure.Landing, value); }
		}
		
		public List <MissedApproachGroup> MissedInstruction
		{
			get { return GetObjectList <MissedApproachGroup> ((int) PropertyInstrumentApproachProcedure.MissedInstruction); }
		}
		
		public FinalProfile FinalProfile
		{
			get { return GetObject <FinalProfile> ((int) PropertyInstrumentApproachProcedure.FinalProfile); }
			set { SetValue ((int) PropertyInstrumentApproachProcedure.FinalProfile, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyInstrumentApproachProcedure
	{
		ApproachPrefix = PropertyProcedure.NEXT_CLASS,
		ApproachType,
		MultipleIdentification,
		CopterTrack,
		CirclingIdentification,
		CourseReversalInstruction,
		AdditionalEquipment,
		ChannelGNSS,
		WAASReliable,
		Landing,
		MissedInstruction,
		FinalProfile,
		NEXT_CLASS
	}
	
	public static class MetadataInstrumentApproachProcedure
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataInstrumentApproachProcedure ()
		{
			PropInfoList = MetadataProcedure.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyInstrumentApproachProcedure.ApproachPrefix, (int) EnumType.CodeApproachPrefix, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.ApproachType, (int) EnumType.CodeApproach, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.MultipleIdentification, (int) EnumType.CodeUpperAlpha, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.CopterTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.CirclingIdentification, (int) EnumType.CodeUpperAlpha, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.CourseReversalInstruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.AdditionalEquipment, (int) EnumType.CodeApproachEquipmentAdditional, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.ChannelGNSS, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.WAASReliable, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.Landing, (int) ObjectType.LandingTakeoffAreaCollection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.MissedInstruction, (int) ObjectType.MissedApproachGroup, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInstrumentApproachProcedure.FinalProfile, (int) ObjectType.FinalProfile, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
