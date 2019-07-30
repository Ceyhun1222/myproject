using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RouteDME : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RouteDME; }
		}
		
		public bool? CriticalDME
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRouteDME.CriticalDME); }
			set { SetNullableFieldValue <bool> ((int) PropertyRouteDME.CriticalDME, value); }
		}
		
		public bool? Satisfactory
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRouteDME.Satisfactory); }
			set { SetNullableFieldValue <bool> ((int) PropertyRouteDME.Satisfactory, value); }
		}
		
		public FeatureRef ReferencedDME
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRouteDME.ReferencedDME); }
			set { SetValue ((int) PropertyRouteDME.ReferencedDME, value); }
		}
		
		public RoutePortion ApplicableRoutePortion
		{
			get { return GetObject <RoutePortion> ((int) PropertyRouteDME.ApplicableRoutePortion); }
			set { SetValue ((int) PropertyRouteDME.ApplicableRoutePortion, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRouteDME
	{
		CriticalDME = PropertyFeature.NEXT_CLASS,
		Satisfactory,
		ReferencedDME,
		ApplicableRoutePortion,
		NEXT_CLASS
	}
	
	public static class MetadataRouteDME
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRouteDME ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRouteDME.CriticalDME, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteDME.Satisfactory, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteDME.ReferencedDME, (int) FeatureType.DME, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteDME.ApplicableRoutePortion, (int) ObjectType.RoutePortion, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
